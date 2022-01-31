using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod : MonoBehaviour
{
	public enum TagOverrideType
	{
		Ignore,
		Override,
		Append
	}

	[Header("-- ID (only need to be unique among mods of the same ability)")]
	public int m_abilityScopeId;
	public string m_name = "";
	public bool m_availableInGame = true;
	public AbilityModGameTypeReq m_gameTypeReq;

	[Space(5f)]
	public int m_equipCost = 1;
	public bool m_defaultEquip;
	[TextArea(1, 20)]
	public string m_tooltip = "";
	public string m_flavorText = "";

	[HideInInspector]
	public string m_debugUnlocalizedTooltip = "";
	[HideInInspector]
	public List<StatusType> m_savedStatusTypesForTooltips;

	public Sprite m_iconSprite;

	[Separator("Run Priority Mod", "orange")]
	public bool m_useRunPriorityOverride;
	public AbilityPriority m_runPriorityOverride = AbilityPriority.Combat_Damage;

	[Separator("Energy Cost, Cooldown/Stock", "orange")]
	public AbilityModPropertyInt m_techPointCostMod;

	[Header("-- Cooldown / Stock")]
	public AbilityModPropertyInt m_maxCooldownMod;
	[Space(5f)]
	public AbilityModPropertyInt m_maxStocksMod;
	public AbilityModPropertyInt m_stockRefreshDurationMod;
	public AbilityModPropertyBool m_refillAllStockOnRefreshMod;

	[Header("-- Free Action Override")]
	public AbilityModPropertyBool m_isFreeActionMod;

	[Header("-- Auto Queue Override")]
	public AbilityModPropertyBool m_autoQueueIfValidMod;

	[Separator("Targeter Range Mods (applies to all entries in Target Data)", "orange")]
	public AbilityModPropertyFloat m_targetDataMaxRangeMod;
	public AbilityModPropertyFloat m_targetDataMinRangeMod;
	public AbilityModPropertyBool m_targetDataCheckLosMod;

	[Header("    Target Data override")]
	public bool m_useTargetDataOverrides;
	public TargetData[] m_targetDataOverrides;

	[Separator("Tech Point Interaction", "orange")]
	public TechPointInteractionMod[] m_techPointInteractionMods;

	[Header("-- Anim type override")]
	public bool m_useActionAnimTypeOverride;
	public ActorModelData.ActionAnimationType m_actionAnimTypeOverride;

	[Header("-- Movement Adjustment Override")]
	public bool m_useMovementAdjustmentOverride;
	public Ability.MovementAdjustment m_movementAdjustmentOverride = Ability.MovementAdjustment.ReducedMovement;

	[Header("-- Effect to Self / Targeted Enemies or Allies")]
	public StandardEffectInfo m_effectToSelfOnCast;
	public StandardEffectInfo m_effectToTargetEnemyOnHit;

	[Tooltip("NOTE: the ability doesn't automatically make allies targetable for this stuff. Each ability needs to do that if allies are only targetable with certain mods.")]
	public StandardEffectInfo m_effectToTargetAllyOnHit;

	[Tooltip("This would be in addition to the Effect To Self On Cast if both are set. Use mainly for single-target abilities.")]
	public bool m_useAllyEffectForTargetedCaster;

	[Header("    The above effects use this 0-1 value")]
	public float m_effectTriggerChance = 1f;

	[Header("    If you want more hits to increase the chance")]
	public bool m_effectTriggerChanceMultipliedPerHit;

	[Header("-- Cooldown Reductions on Cast")]
	public AbilityModCooldownReduction m_cooldownReductionsOnSelf;

	[Header("-- Sequence for self hit timing (for effect/cooldown reduction). If empty, will use placeholder")]
	public GameObject m_selfHitTimingSequencePrefab;

	[Header("-- Chain Ability's Additional Effects/Cooldown Reductions")]
	public List<ChainAbilityAdditionalModInfo> m_chainAbilityModInfo;

	[Header("    Chain Ability override")]
	public bool m_useChainAbilityOverrides;
	public Ability[] m_chainAbilityOverrides;

	[Header("-- Ability Tag Override")]
	public TagOverrideType m_tagsModType;
	public List<AbilityTags> m_abilityTagsInMod = new List<AbilityTags>();

	[Header("-- Stat Mods While Mod is Equipped")]
	public AbilityStatMod[] m_statModsWhileEquipped;

	[Header("-- Buff/Debuff Status when ability is requested (apply in Decision)")]
	public bool m_useStatusWhenRequestedOverride;
	public List<StatusType> m_statusWhenRequestedOverride = new List<StatusType>();

	[Space(25f)]
	[Separator("End of Base Ability Mod", "orange")]
	public bool beginningOfModSpecificData;

	public virtual Type GetTargetAbilityType()
	{
		return typeof(Ability);
	}

	public string GetName()
	{
		string text = StringUtil.TR_AbilityModName(GetTargetAbilityType().ToString(), m_name);
		if (text.Length == 0 && m_name.Length > 0)
		{
			text = m_name;
		}
		return text;
	}

	public string GetFullTooltip(Ability ability)
	{
		string text = StringUtil.TR_AbilityModFinalTooltip(GetTargetAbilityType().ToString(), m_name);
		if (text.Length == 0 && m_tooltip.Length > 0)
		{
			text = m_tooltip;
		}
		return TooltipTokenEntry.GetTooltipWithSubstitutes(text, GetTooltipTokenEntries(ability));
	}

	public string GetUnlocalizedFullTooltip(Ability ability)
	{
		if (string.IsNullOrEmpty(m_debugUnlocalizedTooltip))
		{
			return TooltipTokenEntry.GetTooltipWithSubstitutes(m_tooltip, GetTooltipTokenEntries(ability));
		}
		return TooltipTokenEntry.GetTooltipWithSubstitutes(m_debugUnlocalizedTooltip, GetTooltipTokenEntries(ability));
	}

	public virtual OnHitAuthoredData GenModImpl_GetModdedOnHitData(OnHitAuthoredData onHitDataFromBase)
	{
		Log.Error("Please implement GenModImpl_GetModdedOnHitData in derived class " + GetType());
		return onHitDataFromBase;
	}

	public virtual void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		Log.Error("Please implement GenModImpl_SetTargetSelectMod in derived class " + GetType());
	}

	public virtual List<StatusType> GetStatusTypesForTooltip()
	{
		if (m_savedStatusTypesForTooltips != null && m_savedStatusTypesForTooltips.Count != 0)
		{
			return m_savedStatusTypesForTooltips;
		}
		return TooltipTokenEntry.GetStatusTypesFromTooltip(m_tooltip);
	}

	public ChainAbilityAdditionalModInfo GetChainModInfoAtIndex(int chainIndex)
	{
		foreach (ChainAbilityAdditionalModInfo chainAbilityAdditionalModInfo in m_chainAbilityModInfo)
		{
			if (chainAbilityAdditionalModInfo.m_chainAbilityIndex == chainIndex)
			{
				return chainAbilityAdditionalModInfo;
			}
		}
		return null;
	}

	public int GetModdedTechPointForInteraction(TechPointInteractionType interactionType, int baseAmount)
	{
		foreach (TechPointInteractionMod techPointInteractionMod in m_techPointInteractionMods)
		{
			if (techPointInteractionMod.interactionType == interactionType)
			{
				return techPointInteractionMod.modAmount.GetModifiedValue(baseAmount);
			}
		}
		return baseAmount;
	}

	public bool EquippableForGameType()
	{
		if (m_gameTypeReq == AbilityModGameTypeReq.ExcludeFromRanked)
		{
			ModStrictness requiredModStrictnessForGameSubType = GetRequiredModStrictnessForGameSubType();
			if (requiredModStrictnessForGameSubType == ModStrictness.Ranked)
			{
				return false;
			}
		}
		return true;
	}

	public static ModStrictness GetRequiredModStrictnessForGameSubType()
	{
		if (GameManager.Get().GameConfig != null
			&& !GameManager.Get().GameConfig.SubTypes.IsNullOrEmpty()
			&& GameManager.Get().GameConfig.InstanceSubType != null)
		{
			if (GameManager.Get().GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.StricterMods))
			{
				return ModStrictness.Ranked;
			}
			return ModStrictness.AllModes;
		}

		if (ClientGameManager.Get() != null && ClientGameManager.Get().GroupInfo != null)
		{
			GameType selectedQueueType = ClientGameManager.Get().GroupInfo.SelectedQueueType;
			int subTypeMask = ClientGameManager.Get().GroupInfo.SubTypeMask;
			if (selectedQueueType == GameType.Ranked)
			{
				return ModStrictness.Ranked;
			}

			Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(selectedQueueType);
			foreach (KeyValuePair<ushort, GameSubType> current in gameTypeSubTypes)
			{
				if ((current.Key & subTypeMask) != 0
					&& current.Value.HasMod(GameSubType.SubTypeMods.StricterMods))
				{
					return ModStrictness.Ranked;
				}
			}
			return ModStrictness.AllModes;
		}

		Log.Error("Failed to check mod strictness for unknown game type on client.");
		return ModStrictness.AllModes;
	}

	protected virtual void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
	}

	public List<TooltipTokenEntry> GetTooltipTokenEntries(Ability ability)
	{
		List<TooltipTokenEntry> list = new List<TooltipTokenEntry>();
		try
		{
			AddModSpecificTooltipTokens(list, ability);
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception while trying to add mod specific tooltip tokens for " + GetDebugIdentifier("") + " | " + GetTargetAbilityType().ToString() + "\nStackTrace:\n" + ex.StackTrace);
		}
		if (ability != null)
		{
			AddToken(list, m_techPointCostMod, "EnergyCost", "energy cost", ability.m_techPointsCost);
			AddToken(list, m_maxCooldownMod, "MaxCooldown", "max cooldown", ability.m_cooldown);
			AddToken(list, m_maxStocksMod, "MaxStocks", "max stocks", ability.m_maxStocks);
			AddToken(list, m_stockRefreshDurationMod, "StockRefreshDur", "stock refresh duration", ability.m_stockRefreshDuration);
			if (ability.m_targetData != null && ability.m_targetData.Length > 0)
			{
				AddToken(list, m_targetDataMinRangeMod, "TargetDataMinRange", "min range of targeter when using square based targeters", ability.m_targetData[0].m_minRange);
				AddToken(list, m_targetDataMaxRangeMod, "TargetDataMaxRange", "MAX range of targeter when using square based targeters", ability.m_targetData[0].m_range);
			}
			if (m_useTargetDataOverrides
				&& m_targetDataOverrides != null
				&& m_targetDataOverrides.Length > 0
				&& ability.m_targetData.Length > 0)
			{
				AddToken_IntDiff(list, "BM_TargetDataMaxRange_0", "", Mathf.RoundToInt(m_targetDataOverrides[0].m_range), false, 0);
				AddToken_IntDiff(list, "BM_TargetDataMaxRange_0_Diff", "", Mathf.RoundToInt(m_targetDataOverrides[0].m_range - ability.m_targetData[0].m_range), false, 0);
			}
			AddToken_EffectInfo(list, m_effectToSelfOnCast, "BM_EffectToSelf");
			AddToken_EffectInfo(list, m_effectToTargetAllyOnHit, "BM_EffectToAllyHit");
			AddToken_EffectInfo(list, m_effectToTargetEnemyOnHit, "BM_EffetToEnemyHit");
			int num = Mathf.RoundToInt(100f * m_effectTriggerChance);
			if (num > 0 && num < 100)
			{
				list.Add(new TooltipTokenInt("BM_EffectApplyChance", "chance to apply effects on base mod", num));
			}
			AddTokensForTechPointInteractions(list, ability);
			if (m_cooldownReductionsOnSelf != null)
			{
				m_cooldownReductionsOnSelf.AddTooltipTokens(list, "OnSelf");
			}
			if (m_chainAbilityModInfo != null)
			{
				foreach (ChainAbilityAdditionalModInfo current in m_chainAbilityModInfo)
				{
					current.AddTooltipTokens(list, ability, this, "ChainMod");
				}
			}
		}
		return list;
	}

	public string AssembleFinalTooltip(Ability targetAbility)
	{
		return TooltipTokenEntry.GetTooltipWithSubstitutes(m_tooltip, GetTooltipTokenEntries(targetAbility));
	}

	public void SetUnlocalizedTooltipAndStatusTypes(Ability targetAbility)
	{
		m_debugUnlocalizedTooltip = AssembleFinalTooltip(targetAbility);
		m_savedStatusTypesForTooltips = TooltipTokenEntry.GetStatusTypesFromTooltip(m_tooltip);
	}

	private void AddTokensForTechPointInteractions(List<TooltipTokenEntry> tokens, Ability ability)
	{
		if (m_techPointInteractionMods == null)
		{
			return;
		}
		bool flag = false;
		if (ability != null && ability.m_techPointInteractions != null)
		{
			flag = true;
		}
		foreach (TechPointInteractionMod techPointInteractionMod in m_techPointInteractionMods)
		{
			int baseVal = 0;
			if (flag)
			{
				foreach (TechPointInteraction techPointInteraction in ability.m_techPointInteractions)
				{
					if (techPointInteraction.m_type == techPointInteractionMod.interactionType)
					{
						baseVal = techPointInteraction.m_amount;
						break;
					}
				}
			}
			AddToken(tokens, techPointInteractionMod.modAmount, techPointInteractionMod.interactionType.ToString(), "Energy Gain", baseVal);
		}
	}

	public static void AddToken(List<TooltipTokenEntry> entries, AbilityModPropertyInt modProp, string tokenName, string desc, int baseVal, bool addCompare = true, bool addForZeroBase = false)
	{
		if (modProp == null || modProp.operation == AbilityModPropertyInt.ModOp.Ignore)
		{
			return;
		}

		int modifiedValue = modProp.GetModifiedValue(baseVal);
		entries.Add(new TooltipTokenInt(tokenName + "_Final", desc + " | Final Value", Mathf.Abs(modifiedValue)));
		if (addCompare && (baseVal != 0 || addForZeroBase))
		{
			int val = Mathf.Abs(modifiedValue - baseVal);
			entries.Add(new TooltipTokenInt(tokenName + "_Diff", desc + " | Difference", val));
		}
	}

	public static void AddToken(List<TooltipTokenEntry> entries, AbilityModPropertyFloat modProp, string tokenName, string desc, float baseVal, bool addCompare = true, bool addDecimal = false, bool addAsPct = false)
	{
		if (modProp == null || modProp.operation == AbilityModPropertyFloat.ModOp.Ignore)
		{
			return;
		}

		float modifiedValue = modProp.GetModifiedValue(baseVal);
		entries.Add(new TooltipTokenInt(tokenName + "_Final", desc + " | Final Value", Mathf.RoundToInt(modifiedValue)));
		if (addCompare && baseVal != 0f)
		{
			int val = Mathf.Abs(Mathf.RoundToInt(modifiedValue - baseVal));
			entries.Add(new TooltipTokenInt(tokenName + "_Diff", desc + " | Difference", val));
		}
		if (addDecimal)
		{
			entries.Add(new TooltipTokenFloat(tokenName + "_Decimal", desc + " | As Decimal Num", modifiedValue));
		}
		if (addAsPct)
		{
			entries.Add(new TooltipTokenPct(tokenName + "_Pct", desc + " | as percent", Mathf.RoundToInt(100f * modifiedValue)));
		}
	}

	public static void AddToken_IntDiff(List<TooltipTokenEntry> tokens, string name, string desc, int val, bool addDiff, int otherVal)
	{
		if (val > 0)
		{
			tokens.Add(new TooltipTokenInt(name, desc + " | Final Value", val));
			if (addDiff && otherVal > 0)
			{
				int num = Mathf.Abs(val - otherVal);
				if (num > 0)
				{
					tokens.Add(new TooltipTokenInt(name + "_Diff", desc + " | Difference", num));
				}
			}
		}
	}

	public static void AddToken_LaserInfo(List<TooltipTokenEntry> tokens, AbilityModPropertyLaserInfo laserInfoMod, string tokenName, LaserTargetingInfo baseLaserInfo = null, bool compareWithBase = true)
	{
		if (laserInfoMod != null)
		{
			bool flag = compareWithBase && baseLaserInfo != null;
			AddToken(tokens, laserInfoMod.m_rangeMod, tokenName + "_Range", "laser range", flag ? baseLaserInfo.range : 0f, flag);
			AddToken(tokens, laserInfoMod.m_widthMod, tokenName + "_Width", "laser width", flag ? baseLaserInfo.width : 0f, flag);
			AddToken(tokens, laserInfoMod.m_maxTargetsMod, tokenName + "_MaxTargets", "laser max targets", flag ? baseLaserInfo.maxTargets : 0, flag);
		}
	}

	public static void AddToken_ConeInfo(List<TooltipTokenEntry> tokens, AbilityModPropertyConeInfo coneInfoMod, string tokenName, ConeTargetingInfo baseConeInfo = null, bool compareWithBase = true)
	{
		if (coneInfoMod != null)
		{
			bool flag = compareWithBase && baseConeInfo != null;
			AddToken(tokens, coneInfoMod.m_radiusMod, tokenName + "_Radius", "cone radius", flag ? baseConeInfo.m_radiusInSquares : 0f, flag);
			AddToken(tokens, coneInfoMod.m_widthAngleMod, tokenName + "_Width", "cone width angle", flag ? baseConeInfo.m_widthAngleDeg : 0f, flag);
		}
	}

	public static void AddToken_EffectInfo(List<TooltipTokenEntry> entries, StandardEffectInfo effectInfo, string tokenName, StandardEffectInfo baseVal = null, bool compareWithBase = true)
	{
		if (effectInfo != null && effectInfo.m_applyEffect)
		{
			bool flag = compareWithBase && baseVal != null && baseVal.m_applyEffect;
			effectInfo.m_effectData.AddTooltipTokens(entries, tokenName, flag, flag ? baseVal.m_effectData : null);
		}
	}

	public static void AddToken_EffectMod(List<TooltipTokenEntry> entries, AbilityModPropertyEffectInfo modProp, string tokenName, StandardEffectInfo baseVal = null, bool compareWithBase = true)
	{
		if (modProp != null
			&& modProp.operation != AbilityModPropertyEffectInfo.ModOp.Ignore
			&& modProp.effectInfo.m_applyEffect)
		{
			AddToken_EffectInfo(entries, modProp.effectInfo, tokenName, baseVal, compareWithBase);
		}
	}

	public static void AddToken_EffectMod(List<TooltipTokenEntry> entries, AbilityModPropertyEffectData modProp, string tokenName, StandardActorEffectData baseVal = null, bool compareWithBase = true)
	{
		if (modProp != null
			&& modProp.operation != AbilityModPropertyEffectData.ModOp.Ignore
			&& modProp.effectData != null)
		{
			modProp.effectData.AddTooltipTokens(entries, tokenName, compareWithBase, baseVal);
		}
	}

	public static void AddToken_BarrierMod(List<TooltipTokenEntry> entries, AbilityModPropertyBarrierDataV2 modProp, string tokenName, StandardBarrierData baseVal)
	{
		if (modProp != null
			&& baseVal != null
			&& modProp.operation != AbilityModPropertyBarrierDataV2.ModOp.Ignore
			&& modProp.barrierModData != null)
		{
			modProp.barrierModData.GetModifiedCopy(baseVal).AddTooltipTokens(entries, tokenName, true, baseVal);
		}
	}

	public static void AddToken_GroundFieldMod(List<TooltipTokenEntry> entries, AbilityModPropertyGroundEffectField modProp, string tokenName, GroundEffectField baseVal)
	{
		if (modProp != null
			&& baseVal != null
			&& modProp.operation != AbilityModPropertyGroundEffectField.ModOp.Ignore
			&& modProp.groundFieldModData != null)
		{
			modProp.groundFieldModData.GetModifiedCopy(baseVal).AddTooltipTokens(entries, tokenName, true, baseVal);
		}
	}

	public string GetAutogenDesc(AbilityData abilityData = null)
	{
		Ability targetAbilityOnAbilityData = GetTargetAbilityOnAbilityData(abilityData);
		bool flag = targetAbilityOnAbilityData != null;
		string text = "";
		string color = "lime";
		if (m_useRunPriorityOverride)
		{
			text += InEditorDescHelper.ColoredString("[Run Phase Override] = " + m_runPriorityOverride.ToString() + "\n", color);
		}
		text += PropDesc(m_techPointCostMod, InEditorDescHelper.ColoredString("[TechPoint Cost]", color), flag, flag ? targetAbilityOnAbilityData.m_techPointsCost : 0);
		text += PropDesc(m_maxCooldownMod, InEditorDescHelper.ColoredString("[Max Cooldown]", color), flag, flag ? targetAbilityOnAbilityData.m_cooldown : 0);
		text += PropDesc(m_maxStocksMod, InEditorDescHelper.ColoredString("[Max Stock]", color), flag, flag ? targetAbilityOnAbilityData.m_maxStocks : 0);
		text += PropDesc(m_stockRefreshDurationMod, InEditorDescHelper.ColoredString("[Stock Refresh Duration]", color), flag, flag ? targetAbilityOnAbilityData.m_stockRefreshDuration : 0);
		text += PropDesc(m_refillAllStockOnRefreshMod, InEditorDescHelper.ColoredString("[Refill All Stock on Refresh]", color), flag, flag && targetAbilityOnAbilityData.m_refillAllStockOnRefresh);
		text += PropDesc(m_isFreeActionMod, InEditorDescHelper.ColoredString("[Free Action Override]", color), flag, flag && targetAbilityOnAbilityData.m_freeAction);
		text += PropDesc(m_autoQueueIfValidMod, InEditorDescHelper.ColoredString("[Auto Queue if Valid]", color));
		text += PropDesc(m_targetDataMaxRangeMod, InEditorDescHelper.ColoredString("[TargetData Max Range]", color));
		text += PropDesc(m_targetDataMinRangeMod, InEditorDescHelper.ColoredString("[TargetData Min Range]", color));
		text += PropDesc(m_targetDataCheckLosMod, InEditorDescHelper.ColoredString("[TargetData Check LoS]", color));
		if (m_useTargetDataOverrides && m_targetDataOverrides != null)
		{
			text += InEditorDescHelper.ColoredString("Using Target Data override, with " + m_targetDataOverrides.Length + " entries:\n", color);
			foreach (TargetData targetData in m_targetDataOverrides)
			{
				text += "    [Paradigm] " + (targetData.m_targetingParadigm > 0 ? targetData.m_targetingParadigm.ToString() : "INVALID");
				text += ", [Range (without range mods)] " + targetData.m_minRange + " to " + targetData.m_range;
				text += ", [Require Los] = " + targetData.m_checkLineOfSight + "\n";
			}
		}
		bool flag2 = false;
		if (m_effectToSelfOnCast != null && m_effectToSelfOnCast.m_applyEffect)
		{
			text += InEditorDescHelper.ColoredString("Applies effect to Self on Cast:\n", color);
			text += m_effectToSelfOnCast.m_effectData.GetInEditorDescription("") + "\n";
			flag2 = true;
		}
		if (m_effectToTargetEnemyOnHit != null && m_effectToTargetEnemyOnHit.m_applyEffect)
		{
			text += InEditorDescHelper.ColoredString("Applies effect to Targeted Enemy on Hit:\n", color);
			text += m_effectToTargetEnemyOnHit.m_effectData.GetInEditorDescription("") + "\n";
			flag2 = true;
		}
		if (m_effectToTargetAllyOnHit != null && m_effectToTargetAllyOnHit.m_applyEffect)
		{
			text += InEditorDescHelper.ColoredString("Applies effect to Targeted Ally on Hit:\n", color);
			text += m_effectToTargetAllyOnHit.m_effectData.GetInEditorDescription("") + "\n";
			if (m_useAllyEffectForTargetedCaster)
			{
				text += "\t(also applies to self if targeted)\n";
			}
			flag2 = true;
		}
		if (m_effectTriggerChance < 1f && flag2)
		{
			text += InEditorDescHelper.ColoredString($"        {m_effectTriggerChance * 100f}% of the time", color);
			if (m_effectTriggerChanceMultipliedPerHit)
			{
				text += InEditorDescHelper.ColoredString(" (per hit)", color);
			}
			text += "\n";
		}
		if (m_cooldownReductionsOnSelf != null && m_cooldownReductionsOnSelf.HasCooldownReduction())
		{
			text += InEditorDescHelper.ColoredString("Cooldown Reductions on Cast:\n", color);
			text += m_cooldownReductionsOnSelf.GetDescription(abilityData);
		}
		if (m_techPointInteractionMods != null)
		{
			bool flag3 = false;
			if (targetAbilityOnAbilityData != null && targetAbilityOnAbilityData.m_techPointInteractions != null)
			{
				flag3 = true;
			}
			foreach (TechPointInteractionMod techPointInteractionMod in m_techPointInteractionMods)
			{
				int baseVal = 0;
				if (flag3)
				{
					foreach (TechPointInteraction techPointInteraction in targetAbilityOnAbilityData.m_techPointInteractions)
					{
						if (techPointInteraction.m_type == techPointInteractionMod.interactionType)
						{
							baseVal = techPointInteraction.m_amount;
							break;
						}
					}
				}
				text += InEditorDescHelper.ColoredString(AbilityModHelper.GetTechPointModDesc(techPointInteractionMod, flag3, baseVal), color);
			}
		}
		if (m_useActionAnimTypeOverride)
		{
			text += InEditorDescHelper.ColoredString("Using Action Anim Type Override, " + m_actionAnimTypeOverride.ToString() + "\n\n", color);
		}
		if (m_useMovementAdjustmentOverride)
		{
			text += InEditorDescHelper.ColoredString("Using Movement Adjustment Override, " + m_movementAdjustmentOverride.ToString() + "\n\n", color);
		}
		if (m_chainAbilityModInfo != null)
		{
			foreach (ChainAbilityAdditionalModInfo current in m_chainAbilityModInfo)
			{
				text += current.GetDescription(abilityData, targetAbilityOnAbilityData, this);
			}
		}
		if (m_useChainAbilityOverrides)
		{
			text += InEditorDescHelper.ColoredString("Using Chain Ability Override\n", color);
			foreach (Ability ability in m_chainAbilityOverrides)
			{
				if (ability != null)
				{
					text += "    Chain Ability: " + ability.m_abilityName + "\n";
					if (ability.m_abilityName == "Base Ability")
					{
						text += "        (Please give a name to this ability for easier identification)\n";
					}
				}
			}
		}
		if (m_tagsModType != TagOverrideType.Ignore)
		{
			text += InEditorDescHelper.ColoredString("Using Tag Mods, type = " + m_tagsModType.ToString() + ":\n", color);
			foreach (AbilityTags item in m_abilityTagsInMod)
			{
				text = text + "    " + item.ToString() + "\n";
			}
		}
		if (m_statModsWhileEquipped != null && m_statModsWhileEquipped.Length > 0)
		{
			text += InEditorDescHelper.ColoredString("Stat Mods while Equipped:\n", color);
			foreach (AbilityStatMod abilityStatMod in m_statModsWhileEquipped)
			{
				text += "    " + abilityStatMod.ToString() + "\n";
			}
			text += "\n";
		}
		if (m_useStatusWhenRequestedOverride && m_statusWhenRequestedOverride.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("Buff/Debuff Status on ability request (in Decision):\n", color);
			foreach (StatusType statusType in m_statusWhenRequestedOverride)
			{
				text += "    [ " + InEditorDescHelper.ColoredString(statusType.ToString()) + " ]\n";
			}
			text += "\n";
		}
		if (beginningOfModSpecificData)
		{
			text += "Meow! (Beginning of Mod Specific Data checkbox does nothing)";
		}
		if (text.Length > 0)
		{
			text += "\n";
		}
		string str = "";
		try
		{
			str = ModSpecificAutogenDesc(abilityData);
		}
		catch (Exception ex)
		{
			if (Application.isEditor)
			{
				Debug.LogError("Exception while trying to generate mod specific description. StackTrace:\n" + ex.StackTrace);
			}
		}
		return text + str;
	}

	protected virtual string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		return "";
	}

	protected Ability GetTargetAbilityOnAbilityData(AbilityData abilityData)
	{
		if (abilityData != null)
		{
			List<Ability> abilitiesAsList = abilityData.GetAbilitiesAsList();
			foreach (Ability item in abilitiesAsList)
			{
				if (item != null && item.GetType() == GetTargetAbilityType())
				{
					return item;
				}
			}
		}
		return null;
	}

	public string PropDesc(AbilityModPropertyInt prop, string prefix, bool showBaseVal = false, int baseVal = 0)
	{
		return AbilityModHelper.GetModPropertyDesc(prop, prefix, showBaseVal, baseVal);
	}

	public string PropDesc(AbilityModPropertyFloat prop, string prefix, bool showBaseVal = false, float baseVal = 0f)
	{
		return AbilityModHelper.GetModPropertyDesc(prop, prefix, showBaseVal, baseVal);
	}

	public string PropDesc(AbilityModPropertyBool prop, string prefix, bool showBaseVal = false, bool baseVal = false)
	{
		return AbilityModHelper.GetModPropertyDesc(prop, prefix, showBaseVal, baseVal);
	}

	public string PropDesc(AbilityModPropertyShape prop, string prefix, bool showBaseVal = false, AbilityAreaShape baseVal = AbilityAreaShape.SingleSquare)
	{
		return AbilityModHelper.GetModPropertyDesc(prop, prefix, showBaseVal, baseVal);
	}

	public string PropDesc(AbilityModPropertyKnockbackType prop, string prefix, bool showBaseVal = false, KnockbackType baseVal = KnockbackType.AwayFromSource)
	{
		return AbilityModHelper.GetModPropertyDesc(prop, prefix, showBaseVal, baseVal);
	}

	public string PropDesc(AbilityModPropertyLaserInfo laserModInfo, string prefix, bool showBaseVal = false, LaserTargetingInfo baseLaserInfo = null)
	{
		return AbilityModHelper.GetModPropertyDesc(laserModInfo, prefix, showBaseVal, baseLaserInfo);
	}

	public string PropDesc(AbilityModPropertyConeInfo coneModInfo, string prefix, bool showBaseVal = false, ConeTargetingInfo baseConeInfo = null)
	{
		return AbilityModHelper.GetModPropertyDesc(coneModInfo, prefix, showBaseVal, baseConeInfo);
	}

	public string PropDesc(AbilityModPropertyEffectInfo modProp, string prefix, bool showBaseVal = false, StandardEffectInfo baseVal = null)
	{
		return AbilityModHelper.GetModPropertyDesc(modProp, prefix, showBaseVal, baseVal);
	}

	public string PropDesc(AbilityModPropertyEffectData modProp, string prefix, bool showBaseVal = false, StandardActorEffectData baseVal = null)
	{
		return AbilityModHelper.GetModPropertyDesc(modProp, prefix, showBaseVal, baseVal);
	}

	public string PropDesc(AbilityModPropertySpoilsSpawnData modProp, string prefix, bool showBaseVal = false, SpoilsSpawnData baseVal = null)
	{
		return AbilityModHelper.GetModPropertyDesc(modProp, prefix, showBaseVal, baseVal);
	}

	public string PropDescBarrierMod(AbilityModPropertyBarrierDataV2 modProp, string prefix, StandardBarrierData baseVal)
	{
		return AbilityModHelper.GetModPropertyDesc(modProp, prefix, baseVal);
	}

	public string PropDescGroundFieldMod(AbilityModPropertyGroundEffectField modProp, string prefix, GroundEffectField baseVal)
	{
		return AbilityModHelper.GetModPropertyDesc(modProp, prefix, baseVal);
	}

	protected virtual void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
	}

	public List<int> Debug_GetAdditionalNumbersInTooltip(Ability ability)
	{
		List<int> list = new List<int>();
		if (ability != null)
		{
			int techPointsCost = ability.m_techPointsCost;
			int modifiedValue = m_techPointCostMod.GetModifiedValue(techPointsCost);
			if (techPointsCost != modifiedValue)
			{
				list.Add(modifiedValue);
				list.Add(Mathf.Abs(techPointsCost - modifiedValue));
			}
		}
		AppendTooltipCheckNumbersFromTargetDataEntresi(ability, list);
		AppendTooltipCheckNumbersFromTechPointInteractions(ability, list);
		if (m_maxCooldownMod.operation != AbilityModPropertyInt.ModOp.Ignore)
		{
			int num = (int)Mathf.Abs(m_maxCooldownMod.value);
			if (num != 0)
			{
				list.Add(num);
			}
			if (ability != null)
			{
				int modifiedValue2 = m_maxCooldownMod.GetModifiedValue(ability.m_cooldown);
				int num2 = ability.m_cooldown - modifiedValue2;
				if (num2 != 0)
				{
					list.Add(Mathf.Abs(num2));
					list.Add(Mathf.Abs(modifiedValue2));
				}
			}
		}
		AppendModSpecificTooltipCheckNumbers(ability, list);
		return list;
	}

	private void AppendTooltipCheckNumbersFromTargetDataEntresi(Ability ability, List<int> numbers)
	{
		if (ability != null && ability.m_targetData != null && ability.m_targetData.Length > 0)
		{
			float minRange = ability.m_targetData[0].m_minRange;
			float range = ability.m_targetData[0].m_range;
			float modifiedMinRange = m_targetDataMinRangeMod.GetModifiedValue(minRange);
			float modifiedMaxRange = m_targetDataMaxRangeMod.GetModifiedValue(range);
			int num = Mathf.Abs(Mathf.RoundToInt(minRange - modifiedMinRange));
			int num2 = Mathf.Abs(Mathf.RoundToInt(range - modifiedMaxRange));
			if (num > 0)
			{
				numbers.Add(num);
			}
			if (num2 > 0)
			{
				numbers.Add(num2);
			}
		}
	}

	private void AppendTooltipCheckNumbersFromTechPointInteractions(Ability ability, List<int> numbers)
	{
		if (ability != null)
		{
			Dictionary<TechPointInteractionType, int> dictionary = new Dictionary<TechPointInteractionType, int>();
			if (ability.m_techPointInteractions != null)
			{
				foreach (TechPointInteraction techPointInteraction in ability.m_techPointInteractions)
				{
					if (!dictionary.ContainsKey(techPointInteraction.m_type))
					{
						int amount = techPointInteraction.m_amount;
						amount = GetModdedTechPointForInteraction(techPointInteraction.m_type, amount);
						if (amount > 0)
						{
							dictionary.Add(techPointInteraction.m_type, amount);
						}
					}
				}
			}
			foreach (TechPointInteractionMod techPointInteractionMod in m_techPointInteractionMods)
			{
				if (!dictionary.ContainsKey(techPointInteractionMod.interactionType))
				{
					int moddedTechPointForInteraction = GetModdedTechPointForInteraction(techPointInteractionMod.interactionType, 0);
					if (moddedTechPointForInteraction > 0)
					{
						dictionary.Add(techPointInteractionMod.interactionType, moddedTechPointForInteraction);
					}
				}
			}
			foreach (KeyValuePair<TechPointInteractionType, int> current in dictionary)
			{
				if (current.Value > 0)
				{
					numbers.Add(current.Value);
				}
			}
		}
	}

	public string GetDebugIdentifier(string colorString = "")
	{
		string text = "Mod[ " + m_abilityScopeId + " ] " + m_name + " ";
		if (colorString.Length > 0)
		{
			return "<color=" + colorString + ">" + text + "</color>";
		}
		return text;
	}
}
