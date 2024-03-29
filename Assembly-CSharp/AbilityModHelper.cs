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
		GameObject modPrefabForAbilityType = GetModPrefabForAbilityType(abilityType);
		if (modPrefabForAbilityType != null)
		{
			AbilityMod[] components = modPrefabForAbilityType.GetComponents<AbilityMod>();
			AbilityMod[] array = components;
			foreach (AbilityMod abilityMod in array)
			{
				if (!abilityMod.m_availableInGame)
				{
					continue;
				}
				if (abilityMod.m_name.Length > 0)
				{
					list.Add(abilityMod);
				}
			}
		}
		return list;
	}

	public static List<AbilityMod> GetAvailableModsForAbilityType(Type abilityType, int abilityIndex, PersistedCharacterData characterData, bool unlockedOnly = false)
	{
		List<AbilityMod> list = new List<AbilityMod>();
		GameObject modPrefabForAbilityType = GetModPrefabForAbilityType(abilityType);
		if (modPrefabForAbilityType != null)
		{
			AbilityMod[] components = modPrefabForAbilityType.GetComponents<AbilityMod>();
			AbilityMod[] array = components;
			foreach (AbilityMod abilityMod in array)
			{
				int num2;
				if (unlockedOnly && characterData != null)
				{
					if (!characterData.CharacterComponent.IsModUnlocked(abilityIndex, abilityMod.m_abilityScopeId))
					{
						num2 = (GameManager.Get().GameplayOverrides.EnableAllMods ? 1 : 0);
						goto IL_008c;
					}
				}
				num2 = 1;
				goto IL_008c;
				IL_008c:
				bool flag = (byte)num2 != 0;
				if (abilityMod.m_availableInGame)
				{
					if (abilityMod.EquippableForGameType())
					{
						if (flag)
						{
							list.Add(abilityMod);
						}
					}
				}
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
			for (int i = 0; i < abilitiesAsList.Count; i++)
			{
				if (!flag)
				{
					Ability ability = abilitiesAsList[i];
					if (!(ability != null))
					{
						continue;
					}
					if (GetAvailableModsForAbilityType(ability.GetType(), i, playerCharacterData, true).Count > 0)
					{
						flag = true;
					}
					continue;
				}
				break;
			}
		}
		return flag;
	}

	public static List<AbilityMod> GetAvailableModsForAbility(Ability ability)
	{
		if (ability != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return GetAvailableModsForAbilityType(ability.GetType());
				}
			}
		}
		return new List<AbilityMod>();
	}

	public static AbilityMod GetModForAbility(Ability ability, int abilityScopeId)
	{
		IEnumerable<AbilityMod> source = from a in GetAvailableModsForAbility(ability)
			where a.m_abilityScopeId == abilityScopeId
			select a;
		if (source.Count() == 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		return source.First();
	}

	public static string GetModPropertyDesc(AbilityModPropertyInt modProp, string prefix, bool showBaseVal = false, int baseVal = 0)
	{
		if (modProp == null || modProp.operation == AbilityModPropertyInt.ModOp.Ignore)
		{
			return string.Empty;
		}
		float value = modProp.value;
		AbilityModPropertyInt.ModOp operation = modProp.operation;
		string str = string.Empty;
		if (operation == AbilityModPropertyInt.ModOp.Add)
		{
			str = ((!(value >= 0f)) ? " " : " +");
			str += InEditorDescHelper.ColoredString(Mathf.RoundToInt(value).ToString());
		}
		else if (operation == AbilityModPropertyInt.ModOp.Override)
		{
			str = " = " + InEditorDescHelper.ColoredString(Mathf.RoundToInt(value).ToString());
		}
		else if (operation == AbilityModPropertyInt.ModOp.MultiplyAndFloor)
		{
			str = " x " + InEditorDescHelper.ColoredString(value.ToString()) + " and round down";
		}
		else if (operation == AbilityModPropertyInt.ModOp.MultiplyAndCeil)
		{
			str = " x " + InEditorDescHelper.ColoredString(value.ToString()) + " and round up";
		}
		else if (operation == AbilityModPropertyInt.ModOp.MultiplyAndRound)
		{
			str = " x " + InEditorDescHelper.ColoredString(value.ToString()) + " and round nearest";
		}
		string str2 = prefix + str;
		if (showBaseVal)
		{
			int modifiedValue = modProp.GetModifiedValue(baseVal);
			str2 += GetDiffString(modifiedValue, baseVal, operation);
		}
		return str2 + "\n";
	}

	public static string GetModPropertyDesc(AbilityModPropertyFloat modProp, string prefix, bool showBaseVal = false, float baseVal = 0f)
	{
		if (modProp != null)
		{
			if (modProp.operation != 0)
			{
				float value = modProp.value;
				AbilityModPropertyFloat.ModOp operation = modProp.operation;
				string str = string.Empty;
				if (operation == AbilityModPropertyFloat.ModOp.Add)
				{
					object obj;
					if (value >= 0f)
					{
						obj = " +";
					}
					else
					{
						obj = " ";
					}
					str = (string)obj;
					str += InEditorDescHelper.ColoredString(value.ToString());
				}
				else if (operation == AbilityModPropertyFloat.ModOp.Override)
				{
					str = " = " + InEditorDescHelper.ColoredString(value.ToString());
				}
				else if (operation == AbilityModPropertyFloat.ModOp.Multiply)
				{
					str = " x " + InEditorDescHelper.ColoredString(value.ToString());
				}
				string text = prefix + str;
				if (showBaseVal)
				{
					float modifiedValue = modProp.GetModifiedValue(baseVal);
					string text2 = text;
					text = text2 + SourceColorStr("\n\tBaseValue = " + baseVal) + ((operation == AbilityModPropertyFloat.ModOp.Add) ? string.Empty : (" diff = " + Mathf.Abs(Mathf.RoundToInt(modifiedValue - baseVal)))) + "\n\tFinalValue => " + InEditorDescHelper.ColoredString(modifiedValue.ToString()) + "\n";
				}
				return text + "\n";
			}
		}
		return string.Empty;
	}

	public static string GetModPropertyDesc(AbilityModPropertyBool modProp, string prefix, bool showBaseVal = false, bool baseVal = false)
	{
		if (modProp != null)
		{
			if (modProp.operation != 0)
			{
				bool value = modProp.value;
				AbilityModPropertyBool.ModOp operation = modProp.operation;
				string str = string.Empty;
				if (operation == AbilityModPropertyBool.ModOp.Override)
				{
					object input;
					if (value)
					{
						input = "True";
					}
					else
					{
						input = "False";
					}
					string str2 = InEditorDescHelper.ColoredString((string)input);
					str = " = " + str2;
				}
				string text = prefix + str;
				if (showBaseVal)
				{
					string text2 = text;
					text = text2 + SourceColorStr("\n\tBaseValue = " + baseVal) + "\n\tFinalValue => " + InEditorDescHelper.ColoredString(modProp.GetModifiedValue(baseVal).ToString()) + "\n";
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
			if (modProp.operation != 0)
			{
				AbilityAreaShape value = modProp.value;
				string str = string.Empty;
				if (modProp.operation == AbilityModPropertyShape.ModOp.Override)
				{
					str = " = " + InEditorDescHelper.ColoredString(value.ToString());
				}
				string text = prefix + str;
				if (showBaseVal)
				{
					string text2 = text;
					text = text2 + SourceColorStr("\n\tBaseValue = " + baseVal) + "\n\tFinalValue => " + InEditorDescHelper.ColoredString(modProp.GetModifiedValue(baseVal).ToString()) + "\n";
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
			if (modProp.operation != 0)
			{
				KnockbackType value = modProp.value;
				string str = string.Empty;
				if (modProp.operation == AbilityModPropertyKnockbackType.ModOp.Override)
				{
					str = " = " + InEditorDescHelper.ColoredString(value.ToString());
				}
				string text = prefix + str;
				if (showBaseVal)
				{
					string text2 = text;
					text = text2 + SourceColorStr("\n\tBaseValue = " + baseVal) + "\n\tFinalValue => " + InEditorDescHelper.ColoredString(modProp.GetModifiedValue(baseVal).ToString()) + "\n";
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		bool flag = showBaseVal && baseLaserInfo != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat rangeMod = laserModInfo.m_rangeMod;
		string prefix2 = prefix + "_Range";
		float baseVal;
		if (flag)
		{
			baseVal = baseLaserInfo.range;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + GetModPropertyDesc(rangeMod, prefix2, flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat widthMod = laserModInfo.m_widthMod;
		string prefix3 = prefix + "_Width";
		float baseVal2;
		if (flag)
		{
			baseVal2 = baseLaserInfo.width;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + GetModPropertyDesc(widthMod, prefix3, flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt maxTargetsMod = laserModInfo.m_maxTargetsMod;
		string prefix4 = prefix + "_MaxTargets";
		int baseVal3;
		if (flag)
		{
			baseVal3 = baseLaserInfo.maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + GetModPropertyDesc(maxTargetsMod, prefix4, flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyBool penetrateLosOverride = laserModInfo.m_penetrateLosOverride;
		string prefix5 = prefix + "_PenetrateLos";
		int baseVal4;
		if (flag)
		{
			baseVal4 = (baseLaserInfo.penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + GetModPropertyDesc(penetrateLosOverride, prefix5, flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyBool affectsEnemyOverride = laserModInfo.m_affectsEnemyOverride;
		string prefix6 = prefix + "_AffectsEnemy";
		int baseVal5;
		if (flag)
		{
			baseVal5 = (baseLaserInfo.affectsEnemies ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + GetModPropertyDesc(affectsEnemyOverride, prefix6, flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertyBool affectsAllyOverride = laserModInfo.m_affectsAllyOverride;
		string prefix7 = prefix + "_AffectsAlly";
		int baseVal6;
		if (flag)
		{
			baseVal6 = (baseLaserInfo.affectsAllies ? 1 : 0);
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + GetModPropertyDesc(affectsAllyOverride, prefix7, flag, (byte)baseVal6 != 0);
		string str7 = empty;
		AbilityModPropertyBool affectsCasterOverride = laserModInfo.m_affectsCasterOverride;
		string prefix8 = prefix + "_AffectsCaster";
		int baseVal7;
		if (flag)
		{
			baseVal7 = (baseLaserInfo.affectsCaster ? 1 : 0);
		}
		else
		{
			baseVal7 = 0;
		}
		return str7 + GetModPropertyDesc(affectsCasterOverride, prefix8, flag, (byte)baseVal7 != 0);
	}

	public static string GetModPropertyDesc(AbilityModPropertyConeInfo coneInfo, string prefix, bool showBaseVal = false, ConeTargetingInfo baseConeInfo = null)
	{
		if (coneInfo == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		int num;
		if (showBaseVal)
		{
			num = ((baseConeInfo != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat radiusMod = coneInfo.m_radiusMod;
		string prefix2 = prefix + "_Radius";
		float baseVal;
		if (flag)
		{
			baseVal = baseConeInfo.m_radiusInSquares;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + GetModPropertyDesc(radiusMod, prefix2, flag, baseVal);
		empty += GetModPropertyDesc(coneInfo.m_widthAngleMod, prefix + "_WidthAngle", flag, (!flag) ? 0f : baseConeInfo.m_widthAngleDeg);
		string str2 = empty;
		AbilityModPropertyFloat backwardsOffsetMod = coneInfo.m_backwardsOffsetMod;
		string prefix3 = prefix + "_BackwardsOffset";
		float baseVal2;
		if (flag)
		{
			baseVal2 = baseConeInfo.m_backwardsOffset;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + GetModPropertyDesc(backwardsOffsetMod, prefix3, flag, baseVal2);
		empty += GetModPropertyDesc(coneInfo.m_penetrateLosMod, prefix + "_PenetrateLoS", flag, flag && baseConeInfo.m_penetrateLos);
		string str3 = empty;
		AbilityModPropertyBool affectsEnemyOverride = coneInfo.m_affectsEnemyOverride;
		string prefix4 = prefix + "_AffectEnemy";
		int baseVal3;
		if (flag)
		{
			baseVal3 = (baseConeInfo.m_affectsEnemies ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + GetModPropertyDesc(affectsEnemyOverride, prefix4, flag, (byte)baseVal3 != 0);
		string str4 = empty;
		AbilityModPropertyBool affectsAllyOverride = coneInfo.m_affectsAllyOverride;
		string prefix5 = prefix + "_AffectAlly";
		int baseVal4;
		if (flag)
		{
			baseVal4 = (baseConeInfo.m_affectsAllies ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + GetModPropertyDesc(affectsAllyOverride, prefix5, flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyBool affectsCasterOverride = coneInfo.m_affectsCasterOverride;
		string prefix6 = prefix + "_AffectCaster";
		int baseVal5;
		if (flag)
		{
			baseVal5 = (baseConeInfo.m_affectsCaster ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		return str5 + GetModPropertyDesc(affectsCasterOverride, prefix6, flag, (byte)baseVal5 != 0);
	}

	public static string GetModPropertyDesc(AbilityModPropertyEffectInfo modProp, string prefix, bool showBaseVal = false, StandardEffectInfo baseVal = null)
	{
		int num;
		if (modProp != null)
		{
			if (modProp.operation != 0)
			{
				if (showBaseVal)
				{
					if (baseVal != null)
					{
						num = (baseVal.m_applyEffect ? 1 : 0);
						goto IL_0051;
					}
				}
				num = 0;
				goto IL_0051;
			}
		}
		return string.Empty;
		IL_0051:
		bool flag = (byte)num != 0;
		string text = GetModEffectInfoDesc(modProp.effectInfo, prefix, string.Empty, flag, (!flag) ? null : baseVal);
		if (modProp.useSequencesFromSource)
		{
			if (modProp.effectInfo.m_applyEffect)
			{
				text = text + InEditorDescHelper.ColoredString("    | Using Sequences from source effect info |") + "\n";
			}
		}
		string text2 = text;
		if (showBaseVal)
		{
			if (baseVal == null)
			{
				text2 = SourceColorStr("\t(No Effect Info in Base)\n");
			}
			else if (!baseVal.m_applyEffect)
			{
				text2 += SourceColorStr("\tEffect set to not apply in Base Ability\n");
			}
		}
		return text2;
	}

	public static string GetModPropertyDesc(AbilityModPropertyEffectData modProp, string prefix, bool showBaseVal = false, StandardActorEffectData baseVal = null)
	{
		if (modProp != null)
		{
			if (modProp.operation != 0)
			{
				int num;
				if (showBaseVal)
				{
					num = ((baseVal != null) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				bool flag = (byte)num != 0;
				StandardActorEffectData effectData = modProp.effectData;
				string empty = string.Empty;
				object baseVal2;
				if (flag)
				{
					baseVal2 = baseVal;
				}
				else
				{
					baseVal2 = null;
				}
				string text = GetModEffectDataDesc(effectData, prefix, empty, flag, (StandardActorEffectData)baseVal2);
				if (modProp.useSequencesFromSource)
				{
					text = text + InEditorDescHelper.ColoredString("    | Using Sequences from source effect data |") + "\n";
				}
				string result = text;
				if (showBaseVal && baseVal == null)
				{
					result = SourceColorStr("\t(No Effect Data in Base)\n");
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
			if (modProp.operation != 0)
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
				if (modProp.operation != 0)
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
				if (modProp.operation != 0)
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
		return InEditorDescHelper.ColoredString(input, "orange");
	}

	public static string GetDiffString(int finalVal, int baseVal, AbilityModPropertyInt.ModOp operation)
	{
		string[] obj = new string[5]
		{
			SourceColorStr("\n\tBaseValue = " + baseVal),
			null,
			null,
			null,
			null
		};
		string text;
		if (operation != AbilityModPropertyInt.ModOp.Add)
		{
			text = " diff = " + Mathf.Abs(finalVal - baseVal);
		}
		else
		{
			text = string.Empty;
		}
		obj[1] = text;
		obj[2] = "\n\tFinalValue => ";
		obj[3] = InEditorDescHelper.ColoredString(finalVal.ToString());
		obj[4] = "\n";
		return string.Concat(obj);
	}

	private static string SurroundWithBrackets(string input, bool surround, bool asHeader)
	{
		if (!surround)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return input;
				}
			}
		}
		return (!asHeader) ? ("[" + input + "]") : ("{ " + input + " }");
	}

	public static string GetModEffectInfoDesc(StandardEffectInfo effectInfo, string prefix, string indent = "", bool useBaseVal = false, StandardEffectInfo baseVal = null)
	{
		string text = string.Empty;
		int num;
		if (effectInfo != null)
		{
			if (effectInfo.m_applyEffect)
			{
				if (useBaseVal)
				{
					if (baseVal != null)
					{
						num = (baseVal.m_applyEffect ? 1 : 0);
						goto IL_004f;
					}
				}
				num = 0;
				goto IL_004f;
			}
			if (baseVal != null)
			{
				text += SourceColorStr(prefix + " | SET TO NOT APPLY\n");
			}
		}
		goto IL_0105;
		IL_0105:
		return text;
		IL_004f:
		bool flag = (byte)num != 0;
		StandardActorEffectData effectData = effectInfo.m_effectData;
		object baseVal2;
		if (flag)
		{
			baseVal2 = baseVal.m_effectData;
		}
		else
		{
			baseVal2 = null;
		}
		text = GetModEffectDataDesc(effectData, prefix, indent, flag, (StandardActorEffectData)baseVal2);
		if (useBaseVal)
		{
			if (baseVal == null)
			{
				text += SourceColorStr("\t" + prefix + " Not in Base Ability\n");
			}
			else if (!baseVal.m_applyEffect)
			{
				text += SourceColorStr("\tNot applying " + prefix + " in Base Ability\n");
			}
		}
		goto IL_0105;
	}

	public static string GetModEffectDataDesc(StandardActorEffectData effectData, string prefix, string indent = "", bool useBaseVal = false, StandardActorEffectData baseVal = null)
	{
		string text = string.Empty;
		if (effectData != null)
		{
			string text2 = text;
			text = text2 + "\n" + indent + "<b>" + prefix + "</b>\n";
			int num;
			if (useBaseVal)
			{
				num = ((baseVal != null) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool diff = (byte)num != 0;
			text += effectData.GetInEditorDescription(indent, true, diff, baseVal);
			if (useBaseVal && baseVal == null)
			{
				text += SourceColorStr("\t" + prefix + " Not in Base Ability\n");
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
			text += effectInfo.m_groundEffectData.GetInEditorDescription();
			if (useBaseVal)
			{
				if (baseVal == null)
				{
					text += SourceColorStr("\t" + prefix + " Not in Base Ability\n");
				}
				else if (!baseVal.m_applyGroundEffect)
				{
					text += SourceColorStr("\tNot applying " + prefix + " in Base Ability\n");
				}
				else
				{
					text += SourceColorStr(baseVal.m_groundEffectData.GetInEditorDescription());
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
					text = text3 + "Modify cooldown on " + GetAbilityNameFromActionType(cooldownChangeInfo.abilitySlot, abilityData) + text2 + InEditorDescHelper.ColoredString(Mathf.Abs(cooldownAddAmount).ToString()) + "\n";
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
				text = string.Concat(text2, " [", ability.m_abilityName, " (", ability.GetType(), ")]");
			}
		}
		return text;
	}

	public static string GetCooldownModDesc(AbilityCooldownMod mod, string prefix, AbilityData abilityData = null)
	{
		if (mod != null && mod.modAmount != null && mod.abilitySlot != AbilityData.ActionType.INVALID_ACTION)
		{
			if (mod.modAmount.operation != 0)
			{
				string text;
				if (abilityData == null)
				{
					text = mod.abilitySlot.ToString();
				}
				else
				{
					text = GetAbilityNameFromActionType(mod.abilitySlot, abilityData);
				}
				string str = text;
				string str2 = " for ability at [ " + str + " ] ";
				string text2;
				if (mod.modAmount.operation == AbilityModPropertyInt.ModOp.Add)
				{
					object str3;
					if (mod.modAmount.value >= 0f)
					{
						str3 = " + ";
					}
					else
					{
						str3 = " - ";
					}
					text2 = (string)str3 + Mathf.Abs(mod.modAmount.value);
				}
				else
				{
					text2 = GetModPropertyDesc(mod.modAmount, string.Empty);
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
						if (mod.availableStockModAmount.operation != 0)
						{
							text = text + "    " + GetModPropertyDesc(mod.availableStockModAmount, "[Stock Available Mod]");
						}
						if (mod.refreshTimeRemainingModAmount.operation != 0)
						{
							text = text + "    " + GetModPropertyDesc(mod.refreshTimeRemainingModAmount, "[Stock Refresh Time Remaining]");
						}
						if (text.Length > 0)
						{
							text = prefix + " for ability at [" + mod.abilitySlot.ToString() + "]\n" + text;
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
			if (mod.modAmount.operation != 0)
			{
				string modPropertyDesc = GetModPropertyDesc(mod.modAmount, string.Empty);
				string text = "TechPoint on [" + mod.interactionType.ToString() + "]" + modPropertyDesc;
				if (useBase)
				{
					string text2 = text;
					text = text2 + SourceColorStr("\tBaseValue = " + baseVal) + "\n\tFinalValue => " + InEditorDescHelper.ColoredString(mod.modAmount.GetModifiedValue(baseVal).ToString()) + "\n";
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
			text = text2 + "Override Sequence " + prefix + " = " + sequencePrefab.name + "\n";
		}
		return text;
	}

	public static void AddTooltipNumbersFromEffect(StandardEffectInfo effectInfo, List<int> numbers)
	{
		if (effectInfo == null)
		{
			return;
		}
		while (true)
		{
			if (effectInfo.m_applyEffect)
			{
				while (true)
				{
					AddTooltipNumbersFromEffect(effectInfo.m_effectData, numbers);
					return;
				}
			}
			return;
		}
	}

	public static void AddTooltipNumbersFromEffect(StandardActorEffectData effectData, List<int> numbers)
	{
		if (numbers == null)
		{
			return;
		}
		while (true)
		{
			if (effectData == null)
			{
				return;
			}
			while (true)
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
					while (true)
					{
						numbers.Add(effectData.m_healingPerTurn);
						return;
					}
				}
				return;
			}
		}
	}
}
