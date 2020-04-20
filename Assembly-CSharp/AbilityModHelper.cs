using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AbilityModHelper
{
	public static bool IsModAllowed(CharacterType characterType, int actionTypeInt, int abilityScopeId)
	{
		bool result = true;
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().GameplayOverrides != null)
			{
				result = GameManager.Get().GameplayOverrides.IsAbilityModAllowed(characterType, actionTypeInt, abilityScopeId);
			}
		}
		return result;
	}

	public static GameObject GetModPrefabForAbilityType(Type abilityType)
	{
		return Resources.Load("AbilityMod_" + abilityType.ToString()) as GameObject;
	}

	public static List<AbilityMod> GetAvailableModsForAbilityType(Type abilityType)
	{
		List<AbilityMod> list = new List<AbilityMod>();
		GameObject modPrefabForAbilityType = AbilityModHelper.GetModPrefabForAbilityType(abilityType);
		if (modPrefabForAbilityType != null)
		{
			AbilityMod[] components = modPrefabForAbilityType.GetComponents<AbilityMod>();
			foreach (AbilityMod abilityMod in components)
			{
				if (abilityMod.m_availableInGame)
				{
					if (abilityMod.m_name.Length > 0)
					{
						list.Add(abilityMod);
					}
				}
			}
		}
		return list;
	}

	public static List<AbilityMod> GetAvailableModsForAbilityType(Type abilityType, int abilityIndex, PersistedCharacterData characterData, bool unlockedOnly = false)
	{
		List<AbilityMod> list = new List<AbilityMod>();
		GameObject modPrefabForAbilityType = AbilityModHelper.GetModPrefabForAbilityType(abilityType);
		if (modPrefabForAbilityType != null)
		{
			AbilityMod[] components = modPrefabForAbilityType.GetComponents<AbilityMod>();
			AbilityMod[] array = components;
			int i = 0;
			while (i < array.Length)
			{
				AbilityMod abilityMod = array[i];
				if (!unlockedOnly || characterData == null)
				{
					goto IL_8B;
				}
				if (characterData.CharacterComponent.IsModUnlocked(abilityIndex, abilityMod.m_abilityScopeId))
				{
					goto IL_8B;
				}
				bool flag = GameManager.Get().GameplayOverrides.EnableAllMods;
				IL_8C:
				bool flag2 = flag;
				if (abilityMod.m_availableInGame)
				{
					if (abilityMod.EquippableForGameType())
					{
						if (flag2)
						{
							list.Add(abilityMod);
						}
					}
				}
				i++;
				continue;
				IL_8B:
				flag = true;
				goto IL_8C;
			}
		}
		return list;
	}

	public static bool HasAnyAvailableMods(ActorData characterActorData)
	{
		bool flag = false;
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterActorData.m_characterType);
		AbilityData component = characterActorData.GetComponent<AbilityData>();
		if (component != null)
		{
			List<Ability> abilitiesAsList = component.GetAbilitiesAsList();
			int i = 0;
			while (i < abilitiesAsList.Count)
			{
				if (flag)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						return flag;
					}
				}
				else
				{
					Ability ability = abilitiesAsList[i];
					if (ability != null)
					{
						if (AbilityModHelper.GetAvailableModsForAbilityType(ability.GetType(), i, playerCharacterData, true).Count > 0)
						{
							flag = true;
						}
					}
					i++;
				}
			}
		}
		return flag;
	}

	public static List<AbilityMod> GetAvailableModsForAbility(Ability ability)
	{
		if (ability != null)
		{
			return AbilityModHelper.GetAvailableModsForAbilityType(ability.GetType());
		}
		return new List<AbilityMod>();
	}

	public static AbilityMod GetModForAbility(Ability ability, int abilityScopeId)
	{
		IEnumerable<AbilityMod> source = from a in AbilityModHelper.GetAvailableModsForAbility(ability)
		where a.m_abilityScopeId == abilityScopeId
		select a;
		if (source.Count<AbilityMod>() == 0)
		{
			return null;
		}
		return source.First<AbilityMod>();
	}

	public static string GetModPropertyDesc(AbilityModPropertyInt modProp, string prefix, bool showBaseVal = false, int baseVal = 0)
	{
		if (modProp == null || modProp.operation == AbilityModPropertyInt.ModOp.Ignore)
		{
			return string.Empty;
		}
		float value = modProp.value;
		AbilityModPropertyInt.ModOp operation = modProp.operation;
		string text = string.Empty;
		if (operation == AbilityModPropertyInt.ModOp.Add)
		{
			text = ((value < 0f) ? " " : " +");
			text += InEditorDescHelper.ColoredString(Mathf.RoundToInt(value).ToString(), "cyan", false);
		}
		else if (operation == AbilityModPropertyInt.ModOp.Override)
		{
			text = " = " + InEditorDescHelper.ColoredString(Mathf.RoundToInt(value).ToString(), "cyan", false);
		}
		else if (operation == AbilityModPropertyInt.ModOp.MultiplyAndFloor)
		{
			text = " x " + InEditorDescHelper.ColoredString(value.ToString(), "cyan", false) + " and round down";
		}
		else if (operation == AbilityModPropertyInt.ModOp.MultiplyAndCeil)
		{
			text = " x " + InEditorDescHelper.ColoredString(value.ToString(), "cyan", false) + " and round up";
		}
		else if (operation == AbilityModPropertyInt.ModOp.MultiplyAndRound)
		{
			text = " x " + InEditorDescHelper.ColoredString(value.ToString(), "cyan", false) + " and round nearest";
		}
		string str = prefix + text;
		if (showBaseVal)
		{
			int modifiedValue = modProp.GetModifiedValue(baseVal);
			str += AbilityModHelper.GetDiffString(modifiedValue, baseVal, operation);
		}
		return str + "\n";
	}

	public static string GetModPropertyDesc(AbilityModPropertyFloat modProp, string prefix, bool showBaseVal = false, float baseVal = 0f)
	{
		if (modProp != null)
		{
			if (modProp.operation != AbilityModPropertyFloat.ModOp.Ignore)
			{
				float value = modProp.value;
				AbilityModPropertyFloat.ModOp operation = modProp.operation;
				string text = string.Empty;
				if (operation == AbilityModPropertyFloat.ModOp.Add)
				{
					string text2;
					if (value >= 0f)
					{
						text2 = " +";
					}
					else
					{
						text2 = " ";
					}
					text = text2;
					text += InEditorDescHelper.ColoredString(value.ToString(), "cyan", false);
				}
				else if (operation == AbilityModPropertyFloat.ModOp.Override)
				{
					text = " = " + InEditorDescHelper.ColoredString(value.ToString(), "cyan", false);
				}
				else if (operation == AbilityModPropertyFloat.ModOp.Multiply)
				{
					text = " x " + InEditorDescHelper.ColoredString(value.ToString(), "cyan", false);
				}
				string text3 = prefix + text;
				if (showBaseVal)
				{
					float modifiedValue = modProp.GetModifiedValue(baseVal);
					string text4 = text3;
					text3 = string.Concat(new string[]
					{
						text4,
						AbilityModHelper.SourceColorStr("\n\tBaseValue = " + baseVal),
						(operation == AbilityModPropertyFloat.ModOp.Add) ? string.Empty : (" diff = " + Mathf.Abs(Mathf.RoundToInt(modifiedValue - baseVal))),
						"\n\tFinalValue => ",
						InEditorDescHelper.ColoredString(modifiedValue.ToString(), "cyan", false),
						"\n"
					});
				}
				return text3 + "\n";
			}
		}
		return string.Empty;
	}

	public static string GetModPropertyDesc(AbilityModPropertyBool modProp, string prefix, bool showBaseVal = false, bool baseVal = false)
	{
		if (modProp != null)
		{
			if (modProp.operation != AbilityModPropertyBool.ModOp.Ignore)
			{
				bool value = modProp.value;
				AbilityModPropertyBool.ModOp operation = modProp.operation;
				string str = string.Empty;
				if (operation == AbilityModPropertyBool.ModOp.Override)
				{
					string input;
					if (value)
					{
						input = "True";
					}
					else
					{
						input = "False";
					}
					string str2 = InEditorDescHelper.ColoredString(input, "cyan", false);
					str = " = " + str2;
				}
				string text = prefix + str;
				if (showBaseVal)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						AbilityModHelper.SourceColorStr("\n\tBaseValue = " + baseVal),
						"\n\tFinalValue => ",
						InEditorDescHelper.ColoredString(modProp.GetModifiedValue(baseVal).ToString(), "cyan", false),
						"\n"
					});
				}
				return text + "\n";
			}
		}
		return string.Empty;
	}

	public static string GetModPropertyDesc(AbilityModPropertyShape modProp, string prefix, bool showBaseVal = false, AbilityAreaShape baseVal = AbilityAreaShape.SingleSquare)
	{
		if (modProp != null)
		{
			if (modProp.operation != AbilityModPropertyShape.ModOp.Ignore)
			{
				AbilityAreaShape value = modProp.value;
				string str = string.Empty;
				if (modProp.operation == AbilityModPropertyShape.ModOp.Override)
				{
					str = " = " + InEditorDescHelper.ColoredString(value.ToString(), "cyan", false);
				}
				string text = prefix + str;
				if (showBaseVal)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						AbilityModHelper.SourceColorStr("\n\tBaseValue = " + baseVal),
						"\n\tFinalValue => ",
						InEditorDescHelper.ColoredString(modProp.GetModifiedValue(baseVal).ToString(), "cyan", false),
						"\n"
					});
				}
				return text + "\n";
			}
		}
		return string.Empty;
	}

	public static string GetModPropertyDesc(AbilityModPropertyKnockbackType modProp, string prefix, bool showBaseVal = false, KnockbackType baseVal = KnockbackType.AwayFromSource)
	{
		if (modProp != null)
		{
			if (modProp.operation != AbilityModPropertyKnockbackType.ModOp.Ignore)
			{
				KnockbackType value = modProp.value;
				string str = string.Empty;
				if (modProp.operation == AbilityModPropertyKnockbackType.ModOp.Override)
				{
					str = " = " + InEditorDescHelper.ColoredString(value.ToString(), "cyan", false);
				}
				string text = prefix + str;
				if (showBaseVal)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						AbilityModHelper.SourceColorStr("\n\tBaseValue = " + baseVal),
						"\n\tFinalValue => ",
						InEditorDescHelper.ColoredString(modProp.GetModifiedValue(baseVal).ToString(), "cyan", false),
						"\n"
					});
				}
				return text + "\n";
			}
		}
		return string.Empty;
	}

	public static string GetModPropertyDesc(AbilityModPropertyLaserInfo laserModInfo, string prefix, bool showBaseVal = false, LaserTargetingInfo baseLaserInfo = null)
	{
		if (laserModInfo == null)
		{
			return string.Empty;
		}
		bool flag = showBaseVal && baseLaserInfo != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat rangeMod = laserModInfo.m_rangeMod;
		string prefix2 = prefix + "_Range";
		bool showBaseVal2 = flag;
		float baseVal;
		if (flag)
		{
			baseVal = baseLaserInfo.range;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(rangeMod, prefix2, showBaseVal2, baseVal);
		string str2 = text;
		AbilityModPropertyFloat widthMod = laserModInfo.m_widthMod;
		string prefix3 = prefix + "_Width";
		bool showBaseVal3 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = baseLaserInfo.width;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(widthMod, prefix3, showBaseVal3, baseVal2);
		string str3 = text;
		AbilityModPropertyInt maxTargetsMod = laserModInfo.m_maxTargetsMod;
		string prefix4 = prefix + "_MaxTargets";
		bool showBaseVal4 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = baseLaserInfo.maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(maxTargetsMod, prefix4, showBaseVal4, baseVal3);
		string str4 = text;
		AbilityModPropertyBool penetrateLosOverride = laserModInfo.m_penetrateLosOverride;
		string prefix5 = prefix + "_PenetrateLos";
		bool showBaseVal5 = flag;
		bool baseVal4;
		if (flag)
		{
			baseVal4 = baseLaserInfo.penetrateLos;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(penetrateLosOverride, prefix5, showBaseVal5, baseVal4);
		string str5 = text;
		AbilityModPropertyBool affectsEnemyOverride = laserModInfo.m_affectsEnemyOverride;
		string prefix6 = prefix + "_AffectsEnemy";
		bool showBaseVal6 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = baseLaserInfo.affectsEnemies;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(affectsEnemyOverride, prefix6, showBaseVal6, baseVal5);
		string str6 = text;
		AbilityModPropertyBool affectsAllyOverride = laserModInfo.m_affectsAllyOverride;
		string prefix7 = prefix + "_AffectsAlly";
		bool showBaseVal7 = flag;
		bool baseVal6;
		if (flag)
		{
			baseVal6 = baseLaserInfo.affectsAllies;
		}
		else
		{
			baseVal6 = false;
		}
		text = str6 + AbilityModHelper.GetModPropertyDesc(affectsAllyOverride, prefix7, showBaseVal7, baseVal6);
		string str7 = text;
		AbilityModPropertyBool affectsCasterOverride = laserModInfo.m_affectsCasterOverride;
		string prefix8 = prefix + "_AffectsCaster";
		bool showBaseVal8 = flag;
		bool baseVal7;
		if (flag)
		{
			baseVal7 = baseLaserInfo.affectsCaster;
		}
		else
		{
			baseVal7 = false;
		}
		return str7 + AbilityModHelper.GetModPropertyDesc(affectsCasterOverride, prefix8, showBaseVal8, baseVal7);
	}

	public static string GetModPropertyDesc(AbilityModPropertyConeInfo coneInfo, string prefix, bool showBaseVal = false, ConeTargetingInfo baseConeInfo = null)
	{
		if (coneInfo == null)
		{
			return string.Empty;
		}
		bool flag;
		if (showBaseVal)
		{
			flag = (baseConeInfo != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat radiusMod = coneInfo.m_radiusMod;
		string prefix2 = prefix + "_Radius";
		bool showBaseVal2 = flag2;
		float baseVal;
		if (flag2)
		{
			baseVal = baseConeInfo.m_radiusInSquares;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(radiusMod, prefix2, showBaseVal2, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(coneInfo.m_widthAngleMod, prefix + "_WidthAngle", flag2, (!flag2) ? 0f : baseConeInfo.m_widthAngleDeg);
		string str2 = text;
		AbilityModPropertyFloat backwardsOffsetMod = coneInfo.m_backwardsOffsetMod;
		string prefix3 = prefix + "_BackwardsOffset";
		bool showBaseVal3 = flag2;
		float baseVal2;
		if (flag2)
		{
			baseVal2 = baseConeInfo.m_backwardsOffset;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(backwardsOffsetMod, prefix3, showBaseVal3, baseVal2);
		text += AbilityModHelper.GetModPropertyDesc(coneInfo.m_penetrateLosMod, prefix + "_PenetrateLoS", flag2, flag2 && baseConeInfo.m_penetrateLos);
		string str3 = text;
		AbilityModPropertyBool affectsEnemyOverride = coneInfo.m_affectsEnemyOverride;
		string prefix4 = prefix + "_AffectEnemy";
		bool showBaseVal4 = flag2;
		bool baseVal3;
		if (flag2)
		{
			baseVal3 = baseConeInfo.m_affectsEnemies;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(affectsEnemyOverride, prefix4, showBaseVal4, baseVal3);
		string str4 = text;
		AbilityModPropertyBool affectsAllyOverride = coneInfo.m_affectsAllyOverride;
		string prefix5 = prefix + "_AffectAlly";
		bool showBaseVal5 = flag2;
		bool baseVal4;
		if (flag2)
		{
			baseVal4 = baseConeInfo.m_affectsAllies;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(affectsAllyOverride, prefix5, showBaseVal5, baseVal4);
		string str5 = text;
		AbilityModPropertyBool affectsCasterOverride = coneInfo.m_affectsCasterOverride;
		string prefix6 = prefix + "_AffectCaster";
		bool showBaseVal6 = flag2;
		bool baseVal5;
		if (flag2)
		{
			baseVal5 = baseConeInfo.m_affectsCaster;
		}
		else
		{
			baseVal5 = false;
		}
		return str5 + AbilityModHelper.GetModPropertyDesc(affectsCasterOverride, prefix6, showBaseVal6, baseVal5);
	}

	public static string GetModPropertyDesc(AbilityModPropertyEffectInfo modProp, string prefix, bool showBaseVal = false, StandardEffectInfo baseVal = null)
	{
		if (modProp != null)
		{
			if (modProp.operation != AbilityModPropertyEffectInfo.ModOp.Ignore)
			{
				bool flag;
				if (showBaseVal)
				{
					if (baseVal != null)
					{
						flag = baseVal.m_applyEffect;
						goto IL_51;
					}
				}
				flag = false;
				IL_51:
				bool flag2 = flag;
				string text = AbilityModHelper.GetModEffectInfoDesc(modProp.effectInfo, prefix, string.Empty, flag2, (!flag2) ? null : baseVal);
				if (modProp.useSequencesFromSource)
				{
					if (modProp.effectInfo.m_applyEffect)
					{
						text = text + InEditorDescHelper.ColoredString("    | Using Sequences from source effect info |", "cyan", false) + "\n";
					}
				}
				string text2 = text;
				if (showBaseVal)
				{
					if (baseVal == null)
					{
						text2 = AbilityModHelper.SourceColorStr("\t(No Effect Info in Base)\n");
					}
					else if (!baseVal.m_applyEffect)
					{
						text2 += AbilityModHelper.SourceColorStr("\tEffect set to not apply in Base Ability\n");
					}
				}
				return text2;
			}
		}
		return string.Empty;
	}

	public static string GetModPropertyDesc(AbilityModPropertyEffectData modProp, string prefix, bool showBaseVal = false, StandardActorEffectData baseVal = null)
	{
		if (modProp != null)
		{
			if (modProp.operation != AbilityModPropertyEffectData.ModOp.Ignore)
			{
				bool flag;
				if (showBaseVal)
				{
					flag = (baseVal != null);
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				StandardActorEffectData effectData = modProp.effectData;
				string empty = string.Empty;
				bool useBaseVal = flag2;
				StandardActorEffectData baseVal2;
				if (flag2)
				{
					baseVal2 = baseVal;
				}
				else
				{
					baseVal2 = null;
				}
				string text = AbilityModHelper.GetModEffectDataDesc(effectData, prefix, empty, useBaseVal, baseVal2);
				if (modProp.useSequencesFromSource)
				{
					text = text + InEditorDescHelper.ColoredString("    | Using Sequences from source effect data |", "cyan", false) + "\n";
				}
				string result = text;
				if (showBaseVal && baseVal == null)
				{
					result = AbilityModHelper.SourceColorStr("\t(No Effect Data in Base)\n");
				}
				return result;
			}
		}
		return string.Empty;
	}

	public static string GetModPropertyDesc(AbilityModPropertySpoilsSpawnData modProp, string prefix, bool showBaseVal = false, SpoilsSpawnData baseVal = null)
	{
		if (modProp != null)
		{
			if (modProp.operation != AbilityModPropertySpoilsSpawnData.ModOp.Ignore)
			{
				return modProp.spoilsSpawnDataOverride.GetInEditorDescription(prefix, "    ", showBaseVal, baseVal);
			}
		}
		return string.Empty;
	}

	public static string GetModPropertyDesc(AbilityModPropertyBarrierDataV2 modProp, string prefix, StandardBarrierData baseVal)
	{
		if (modProp != null)
		{
			if (baseVal != null)
			{
				if (modProp.operation != AbilityModPropertyBarrierDataV2.ModOp.Ignore)
				{
					if (modProp.barrierModData != null)
					{
						string empty = string.Empty;
						StandardBarrierData modifiedCopy = modProp.barrierModData.GetModifiedCopy(baseVal);
						return empty + modifiedCopy.GetInEditorDescription(prefix, "    ", true, baseVal);
					}
				}
			}
		}
		return string.Empty;
	}

	public static string GetModPropertyDesc(AbilityModPropertyGroundEffectField modProp, string prefix, GroundEffectField baseVal)
	{
		if (modProp != null)
		{
			if (baseVal != null)
			{
				if (modProp.operation != AbilityModPropertyGroundEffectField.ModOp.Ignore)
				{
					if (modProp.groundFieldModData != null)
					{
						string empty = string.Empty;
						GroundEffectField modifiedCopy = modProp.groundFieldModData.GetModifiedCopy(baseVal);
						return empty + modifiedCopy.GetInEditorDescription(prefix, "    ", true, baseVal);
					}
				}
			}
		}
		return string.Empty;
	}

	public static string SourceColorStr(string input)
	{
		return InEditorDescHelper.ColoredString(input, "orange", false);
	}

	public static string GetDiffString(int finalVal, int baseVal, AbilityModPropertyInt.ModOp operation)
	{
		string[] array = new string[5];
		array[0] = AbilityModHelper.SourceColorStr("\n\tBaseValue = " + baseVal);
		int num = 1;
		string text;
		if (operation != AbilityModPropertyInt.ModOp.Add)
		{
			text = " diff = " + Mathf.Abs(finalVal - baseVal);
		}
		else
		{
			text = string.Empty;
		}
		array[num] = text;
		array[2] = "\n\tFinalValue => ";
		array[3] = InEditorDescHelper.ColoredString(finalVal.ToString(), "cyan", false);
		array[4] = "\n";
		return string.Concat(array);
	}

	private static string SurroundWithBrackets(string input, bool surround, bool asHeader)
	{
		if (!surround)
		{
			return input;
		}
		return (!asHeader) ? ("[" + input + "]") : ("{ " + input + " }");
	}

	public static string GetModEffectInfoDesc(StandardEffectInfo effectInfo, string prefix, string indent = "", bool useBaseVal = false, StandardEffectInfo baseVal = null)
	{
		string text = string.Empty;
		if (effectInfo != null)
		{
			if (effectInfo.m_applyEffect)
			{
				bool flag;
				if (useBaseVal)
				{
					if (baseVal != null)
					{
						flag = baseVal.m_applyEffect;
						goto IL_4F;
					}
				}
				flag = false;
				IL_4F:
				bool flag2 = flag;
				StandardActorEffectData effectData = effectInfo.m_effectData;
				bool useBaseVal2 = flag2;
				StandardActorEffectData baseVal2;
				if (flag2)
				{
					baseVal2 = baseVal.m_effectData;
				}
				else
				{
					baseVal2 = null;
				}
				text = AbilityModHelper.GetModEffectDataDesc(effectData, prefix, indent, useBaseVal2, baseVal2);
				if (useBaseVal)
				{
					if (baseVal == null)
					{
						text += AbilityModHelper.SourceColorStr("\t" + prefix + " Not in Base Ability\n");
					}
					else if (!baseVal.m_applyEffect)
					{
						text += AbilityModHelper.SourceColorStr("\tNot applying " + prefix + " in Base Ability\n");
					}
				}
			}
			else if (baseVal != null)
			{
				text += AbilityModHelper.SourceColorStr(prefix + " | SET TO NOT APPLY\n");
			}
		}
		return text;
	}

	public static string GetModEffectDataDesc(StandardActorEffectData effectData, string prefix, string indent = "", bool useBaseVal = false, StandardActorEffectData baseVal = null)
	{
		string text = string.Empty;
		if (effectData != null)
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"\n",
				indent,
				"<b>",
				prefix,
				"</b>\n"
			});
			bool flag;
			if (useBaseVal)
			{
				flag = (baseVal != null);
			}
			else
			{
				flag = false;
			}
			bool diff = flag;
			text += effectData.GetInEditorDescription(indent, true, diff, baseVal);
			if (useBaseVal && baseVal == null)
			{
				text += AbilityModHelper.SourceColorStr("\t" + prefix + " Not in Base Ability\n");
			}
		}
		return text;
	}

	public static string GetModGroundEffectInfoDesc(StandardGroundEffectInfo effectInfo, string prefix, bool useBaseVal = false, StandardGroundEffectInfo baseVal = null)
	{
		string text = string.Empty;
		if (effectInfo != null && effectInfo.m_applyGroundEffect)
		{
			text = text + prefix + "\n";
			text += effectInfo.m_groundEffectData.GetInEditorDescription("GroundEffectField", "    ", false, null);
			if (useBaseVal)
			{
				if (baseVal == null)
				{
					text += AbilityModHelper.SourceColorStr("\t" + prefix + " Not in Base Ability\n");
				}
				else if (!baseVal.m_applyGroundEffect)
				{
					text += AbilityModHelper.SourceColorStr("\tNot applying " + prefix + " in Base Ability\n");
				}
				else
				{
					text += AbilityModHelper.SourceColorStr(baseVal.m_groundEffectData.GetInEditorDescription("GroundEffectField", "    ", false, null));
				}
			}
		}
		return text;
	}

	public static string GetAbilityCooldownChangeDesc(AbilityCooldownChangeInfo cooldownChangeInfo, AbilityData abilityData = null)
	{
		string text = string.Empty;
		if (cooldownChangeInfo != null)
		{
			if (cooldownChangeInfo.abilitySlot != AbilityData.ActionType.INVALID_ACTION)
			{
				if (cooldownChangeInfo.cooldownAddAmount != 0)
				{
					int cooldownAddAmount = cooldownChangeInfo.cooldownAddAmount;
					string text2 = (cooldownAddAmount < 0) ? " is Reduced by " : "is Increased by ";
					string text3 = text;
					text = string.Concat(new string[]
					{
						text3,
						"Modify cooldown on ",
						AbilityModHelper.GetAbilityNameFromActionType(cooldownChangeInfo.abilitySlot, abilityData),
						text2,
						InEditorDescHelper.ColoredString(Mathf.Abs(cooldownAddAmount).ToString(), "cyan", false),
						"\n"
					});
				}
			}
		}
		return text;
	}

	public static string GetAbilityNameFromActionType(AbilityData.ActionType actionType, AbilityData abilityData = null)
	{
		string text = actionType.ToString();
		if (abilityData != null)
		{
			Ability ability = null;
			switch (actionType)
			{
			case AbilityData.ActionType.ABILITY_0:
				ability = abilityData.m_ability0;
				break;
			case AbilityData.ActionType.ABILITY_1:
				ability = abilityData.m_ability1;
				break;
			case AbilityData.ActionType.ABILITY_2:
				ability = abilityData.m_ability2;
				break;
			case AbilityData.ActionType.ABILITY_3:
				ability = abilityData.m_ability3;
				break;
			case AbilityData.ActionType.ABILITY_4:
				ability = abilityData.m_ability4;
				break;
			}
			if (ability != null)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					" [",
					ability.m_abilityName,
					" (",
					ability.GetType(),
					")]"
				});
			}
		}
		return text;
	}

	public static string GetCooldownModDesc(AbilityCooldownMod mod, string prefix, AbilityData abilityData = null)
	{
		if (mod != null && mod.modAmount != null && mod.abilitySlot != AbilityData.ActionType.INVALID_ACTION)
		{
			if (mod.modAmount.operation != AbilityModPropertyInt.ModOp.Ignore)
			{
				string text;
				if (abilityData == null)
				{
					text = mod.abilitySlot.ToString();
				}
				else
				{
					text = AbilityModHelper.GetAbilityNameFromActionType(mod.abilitySlot, abilityData);
				}
				string str = text;
				string str2 = " for ability at [ " + str + " ] ";
				string text2;
				if (mod.modAmount.operation == AbilityModPropertyInt.ModOp.Add)
				{
					string str3;
					if (mod.modAmount.value >= 0f)
					{
						str3 = " + ";
					}
					else
					{
						str3 = " - ";
					}
					text2 = str3 + Mathf.Abs(mod.modAmount.value).ToString();
				}
				else
				{
					text2 = AbilityModHelper.GetModPropertyDesc(mod.modAmount, string.Empty, false, 0);
				}
				string str4 = text2;
				return prefix + str2 + str4;
			}
		}
		return string.Empty;
	}

	public static string GetStockModDesc(AbilityStockMod mod, string prefix)
	{
		if (mod != null)
		{
			if (mod.availableStockModAmount != null)
			{
				if (mod.refreshTimeRemainingModAmount != null)
				{
					if (mod.abilitySlot != AbilityData.ActionType.INVALID_ACTION)
					{
						string text = string.Empty;
						if (mod.availableStockModAmount.operation != AbilityModPropertyInt.ModOp.Ignore)
						{
							text = text + "    " + AbilityModHelper.GetModPropertyDesc(mod.availableStockModAmount, "[Stock Available Mod]", false, 0);
						}
						if (mod.refreshTimeRemainingModAmount.operation != AbilityModPropertyInt.ModOp.Ignore)
						{
							text = text + "    " + AbilityModHelper.GetModPropertyDesc(mod.refreshTimeRemainingModAmount, "[Stock Refresh Time Remaining]", false, 0);
						}
						if (text.Length > 0)
						{
							text = string.Concat(new string[]
							{
								prefix,
								" for ability at [",
								mod.abilitySlot.ToString(),
								"]\n",
								text
							});
						}
						return text;
					}
				}
			}
		}
		return string.Empty;
	}

	public static string GetTechPointModDesc(TechPointInteractionMod mod, bool useBase = false, int baseVal = 0)
	{
		if (mod != null && mod.modAmount != null)
		{
			if (mod.modAmount.operation != AbilityModPropertyInt.ModOp.Ignore)
			{
				string modPropertyDesc = AbilityModHelper.GetModPropertyDesc(mod.modAmount, string.Empty, false, 0);
				string text = "TechPoint on [" + mod.interactionType.ToString() + "]" + modPropertyDesc;
				if (useBase)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						AbilityModHelper.SourceColorStr("\tBaseValue = " + baseVal),
						"\n\tFinalValue => ",
						InEditorDescHelper.ColoredString(mod.modAmount.GetModifiedValue(baseVal).ToString(), "cyan", false),
						"\n"
					});
				}
				return text;
			}
		}
		return string.Empty;
	}

	public static string GetSequencePrefabDesc(GameObject sequencePrefab, string prefix)
	{
		string text = string.Empty;
		if (sequencePrefab != null)
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"Override Sequence ",
				prefix,
				" = ",
				sequencePrefab.name,
				"\n"
			});
		}
		return text;
	}

	public static void AddTooltipNumbersFromEffect(StandardEffectInfo effectInfo, List<int> numbers)
	{
		if (effectInfo != null)
		{
			if (effectInfo.m_applyEffect)
			{
				AbilityModHelper.AddTooltipNumbersFromEffect(effectInfo.m_effectData, numbers);
			}
		}
	}

	public static void AddTooltipNumbersFromEffect(StandardActorEffectData effectData, List<int> numbers)
	{
		if (numbers != null)
		{
			if (effectData != null)
			{
				numbers.Add(effectData.m_duration);
				if (effectData.m_absorbAmount > 0)
				{
					numbers.Add(effectData.m_absorbAmount);
				}
				if (effectData.m_damagePerTurn > 0)
				{
					numbers.Add(effectData.m_damagePerTurn);
				}
				if (effectData.m_healingPerTurn > 0)
				{
					numbers.Add(effectData.m_healingPerTurn);
				}
			}
		}
	}
}
