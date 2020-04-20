using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod : MonoBehaviour
{
	[Header("-- ID (only need to be unique among mods of the same ability)")]
	public int m_abilityScopeId;

	public string m_name = string.Empty;

	public bool m_availableInGame = true;

	public AbilityModGameTypeReq m_gameTypeReq;

	[Space(5f)]
	public int m_equipCost = 1;

	public bool m_defaultEquip;

	[TextArea(1, 0x14)]
	public string m_tooltip = string.Empty;

	public string m_flavorText = string.Empty;

	[HideInInspector]
	public string m_debugUnlocalizedTooltip = string.Empty;

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
	public AbilityMod.TagOverrideType m_tagsModType;

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
		string text = StringUtil.TR_AbilityModName(this.GetTargetAbilityType().ToString(), this.m_name);
		if (text.Length == 0)
		{
			if (this.m_name.Length > 0)
			{
				text = this.m_name;
			}
		}
		return text;
	}

	public string GetFullTooltip(Ability ability)
	{
		string text = StringUtil.TR_AbilityModFinalTooltip(this.GetTargetAbilityType().ToString(), this.m_name);
		if (text.Length == 0)
		{
			if (this.m_tooltip.Length > 0)
			{
				text = this.m_tooltip;
			}
		}
		return TooltipTokenEntry.GetTooltipWithSubstitutes(text, this.GetTooltipTokenEntries(ability), false);
	}

	public string GetUnlocalizedFullTooltip(Ability ability)
	{
		if (string.IsNullOrEmpty(this.m_debugUnlocalizedTooltip))
		{
			return TooltipTokenEntry.GetTooltipWithSubstitutes(this.m_tooltip, this.GetTooltipTokenEntries(ability), false);
		}
		return TooltipTokenEntry.GetTooltipWithSubstitutes(this.m_debugUnlocalizedTooltip, this.GetTooltipTokenEntries(ability), false);
	}

	public virtual OnHitAuthoredData GenModImpl_GetModdedOnHitData(OnHitAuthoredData onHitDataFromBase)
	{
		Log.Error("Please implement GenModImpl_GetModdedOnHitData in derived class " + base.GetType(), new object[0]);
		return onHitDataFromBase;
	}

	public virtual void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		Log.Error("Please implement GenModImpl_SetTargetSelectMod in derived class " + base.GetType(), new object[0]);
	}

	public virtual List<StatusType> GetStatusTypesForTooltip()
	{
		if (this.m_savedStatusTypesForTooltips != null)
		{
			if (this.m_savedStatusTypesForTooltips.Count != 0)
			{
				return this.m_savedStatusTypesForTooltips;
			}
		}
		return TooltipTokenEntry.GetStatusTypesFromTooltip(this.m_tooltip);
	}

	public ChainAbilityAdditionalModInfo GetChainModInfoAtIndex(int chainIndex)
	{
		using (List<ChainAbilityAdditionalModInfo>.Enumerator enumerator = this.m_chainAbilityModInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ChainAbilityAdditionalModInfo chainAbilityAdditionalModInfo = enumerator.Current;
				if (chainAbilityAdditionalModInfo.m_chainAbilityIndex == chainIndex)
				{
					return chainAbilityAdditionalModInfo;
				}
			}
		}
		return null;
	}

	public int GetModdedTechPointForInteraction(TechPointInteractionType interactionType, int baseAmount)
	{
		foreach (TechPointInteractionMod techPointInteractionMod in this.m_techPointInteractionMods)
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
		bool result = true;
		if (this.m_gameTypeReq == AbilityModGameTypeReq.ExcludeFromRanked)
		{
			ModStrictness requiredModStrictnessForGameSubType = AbilityMod.GetRequiredModStrictnessForGameSubType();
			if (requiredModStrictnessForGameSubType == ModStrictness.Ranked)
			{
				result = false;
			}
		}
		return result;
	}

	public static ModStrictness GetRequiredModStrictnessForGameSubType()
	{
		ModStrictness result = ModStrictness.AllModes;
		if (GameManager.Get().GameConfig != null)
		{
			if (!GameManager.Get().GameConfig.SubTypes.IsNullOrEmpty<GameSubType>())
			{
				if (GameManager.Get().GameConfig.InstanceSubType != null)
				{
					if (GameManager.Get().GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.StricterMods))
					{
						result = ModStrictness.Ranked;
					}
					return result;
				}
			}
		}
		if (ClientGameManager.Get() != null)
		{
			if (ClientGameManager.Get().GroupInfo != null)
			{
				GameType selectedQueueType = ClientGameManager.Get().GroupInfo.SelectedQueueType;
				int subTypeMask = (int)ClientGameManager.Get().GroupInfo.SubTypeMask;
				if (selectedQueueType == GameType.Ranked)
				{
					result = ModStrictness.Ranked;
				}
				else
				{
					Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(selectedQueueType);
					using (Dictionary<ushort, GameSubType>.Enumerator enumerator = gameTypeSubTypes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<ushort, GameSubType> keyValuePair = enumerator.Current;
							if (((int)keyValuePair.Key & subTypeMask) != 0 && keyValuePair.Value.HasMod(GameSubType.SubTypeMods.StricterMods))
							{
								result = ModStrictness.Ranked;
								goto IL_18A;
							}
						}
					}
				}
				IL_18A:
				return result;
			}
		}
		Log.Error("Failed to check mod strictness for unknown game type on client.", new object[0]);
		return result;
	}

	protected virtual void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
	}

	public List<TooltipTokenEntry> GetTooltipTokenEntries(Ability ability)
	{
		List<TooltipTokenEntry> list = new List<TooltipTokenEntry>();
		try
		{
			this.AddModSpecificTooltipTokens(list, ability);
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Exception while trying to add mod specific tooltip tokens for ",
				this.GetDebugIdentifier(string.Empty),
				" | ",
				this.GetTargetAbilityType().ToString(),
				"\nStackTrace:\n",
				ex.StackTrace
			}));
		}
		if (ability != null)
		{
			AbilityMod.AddToken(list, this.m_techPointCostMod, "EnergyCost", "energy cost", ability.m_techPointsCost, true, false);
			AbilityMod.AddToken(list, this.m_maxCooldownMod, "MaxCooldown", "max cooldown", ability.m_cooldown, true, false);
			AbilityMod.AddToken(list, this.m_maxStocksMod, "MaxStocks", "max stocks", ability.m_maxStocks, true, false);
			AbilityMod.AddToken(list, this.m_stockRefreshDurationMod, "StockRefreshDur", "stock refresh duration", ability.m_stockRefreshDuration, true, false);
			if (ability.m_targetData != null)
			{
				if (ability.m_targetData.Length > 0)
				{
					AbilityMod.AddToken(list, this.m_targetDataMinRangeMod, "TargetDataMinRange", "min range of targeter when using square based targeters", ability.m_targetData[0].m_minRange, true, false, false);
					AbilityMod.AddToken(list, this.m_targetDataMaxRangeMod, "TargetDataMaxRange", "MAX range of targeter when using square based targeters", ability.m_targetData[0].m_range, true, false, false);
				}
			}
			if (this.m_useTargetDataOverrides)
			{
				if (this.m_targetDataOverrides != null)
				{
					if (this.m_targetDataOverrides.Length > 0)
					{
						if (ability.m_targetData.Length > 0)
						{
							AbilityMod.AddToken_IntDiff(list, "BM_TargetDataMaxRange_0", string.Empty, Mathf.RoundToInt(this.m_targetDataOverrides[0].m_range), false, 0);
							AbilityMod.AddToken_IntDiff(list, "BM_TargetDataMaxRange_0_Diff", string.Empty, Mathf.RoundToInt(this.m_targetDataOverrides[0].m_range - ability.m_targetData[0].m_range), false, 0);
						}
					}
				}
			}
			AbilityMod.AddToken_EffectInfo(list, this.m_effectToSelfOnCast, "BM_EffectToSelf", null, true);
			AbilityMod.AddToken_EffectInfo(list, this.m_effectToTargetAllyOnHit, "BM_EffectToAllyHit", null, true);
			AbilityMod.AddToken_EffectInfo(list, this.m_effectToTargetEnemyOnHit, "BM_EffetToEnemyHit", null, true);
			int num = Mathf.RoundToInt(100f * this.m_effectTriggerChance);
			if (num > 0)
			{
				if (num < 0x64)
				{
					list.Add(new TooltipTokenInt("BM_EffectApplyChance", "chance to apply effects on base mod", num));
				}
			}
			this.AddTokensForTechPointInteractions(list, ability);
			if (this.m_cooldownReductionsOnSelf != null)
			{
				this.m_cooldownReductionsOnSelf.AddTooltipTokens(list, "OnSelf");
			}
			if (this.m_chainAbilityModInfo != null)
			{
				using (List<ChainAbilityAdditionalModInfo>.Enumerator enumerator = this.m_chainAbilityModInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ChainAbilityAdditionalModInfo chainAbilityAdditionalModInfo = enumerator.Current;
						chainAbilityAdditionalModInfo.AddTooltipTokens(list, ability, this, "ChainMod");
					}
				}
			}
		}
		return list;
	}

	public string AssembleFinalTooltip(Ability targetAbility)
	{
		return TooltipTokenEntry.GetTooltipWithSubstitutes(this.m_tooltip, this.GetTooltipTokenEntries(targetAbility), false);
	}

	public void SetUnlocalizedTooltipAndStatusTypes(Ability targetAbility)
	{
		this.m_debugUnlocalizedTooltip = this.AssembleFinalTooltip(targetAbility);
		this.m_savedStatusTypesForTooltips = TooltipTokenEntry.GetStatusTypesFromTooltip(this.m_tooltip);
	}

	private void AddTokensForTechPointInteractions(List<TooltipTokenEntry> tokens, Ability ability)
	{
		if (this.m_techPointInteractionMods != null)
		{
			bool flag = false;
			if (ability != null)
			{
				if (ability.m_techPointInteractions != null)
				{
					flag = true;
				}
			}
			foreach (TechPointInteractionMod techPointInteractionMod in this.m_techPointInteractionMods)
			{
				int baseVal = 0;
				if (flag)
				{
					foreach (TechPointInteraction techPointInteraction in ability.m_techPointInteractions)
					{
						if (techPointInteraction.m_type == techPointInteractionMod.interactionType)
						{
							baseVal = techPointInteraction.m_amount;
							goto IL_C4;
						}
					}
				}
				IL_C4:
				AbilityMod.AddToken(tokens, techPointInteractionMod.modAmount, techPointInteractionMod.interactionType.ToString(), "Energy Gain", baseVal, true, false);
			}
		}
	}

	public static void AddToken(List<TooltipTokenEntry> entries, AbilityModPropertyInt modProp, string tokenName, string desc, int baseVal, bool addCompare = true, bool addForZeroBase = false)
	{
		if (modProp != null)
		{
			if (modProp.operation != AbilityModPropertyInt.ModOp.Ignore)
			{
				int modifiedValue = modProp.GetModifiedValue(baseVal);
				entries.Add(new TooltipTokenInt(tokenName + "_Final", desc + " | Final Value", Mathf.Abs(modifiedValue)));
				if (addCompare)
				{
					if (baseVal == 0)
					{
						if (!addForZeroBase)
						{
							return;
						}
					}
					int val = Mathf.Abs(modifiedValue - baseVal);
					entries.Add(new TooltipTokenInt(tokenName + "_Diff", desc + " | Difference", val));
				}
			}
		}
	}

	public static void AddToken(List<TooltipTokenEntry> entries, AbilityModPropertyFloat modProp, string tokenName, string desc, float baseVal, bool addCompare = true, bool addDecimal = false, bool addAsPct = false)
	{
		if (modProp != null && modProp.operation != AbilityModPropertyFloat.ModOp.Ignore)
		{
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
	}

	public static void AddToken_IntDiff(List<TooltipTokenEntry> tokens, string name, string desc, int val, bool addDiff, int otherVal)
	{
		if (val > 0)
		{
			tokens.Add(new TooltipTokenInt(name, desc + " | Final Value", val));
			if (addDiff)
			{
				if (otherVal > 0)
				{
					int num = Mathf.Abs(val - otherVal);
					if (num > 0)
					{
						tokens.Add(new TooltipTokenInt(name + "_Diff", desc + " | Difference", num));
					}
				}
			}
		}
	}

	public static void AddToken_LaserInfo(List<TooltipTokenEntry> tokens, AbilityModPropertyLaserInfo laserInfoMod, string tokenName, LaserTargetingInfo baseLaserInfo = null, bool compareWithBase = true)
	{
		if (laserInfoMod != null)
		{
			bool flag;
			if (compareWithBase)
			{
				flag = (baseLaserInfo != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			AbilityModPropertyFloat rangeMod = laserInfoMod.m_rangeMod;
			string tokenName2 = tokenName + "_Range";
			string desc = "laser range";
			float baseVal;
			if (flag2)
			{
				baseVal = baseLaserInfo.range;
			}
			else
			{
				baseVal = 0f;
			}
			AbilityMod.AddToken(tokens, rangeMod, tokenName2, desc, baseVal, flag2, false, false);
			AbilityModPropertyFloat widthMod = laserInfoMod.m_widthMod;
			string tokenName3 = tokenName + "_Width";
			string desc2 = "laser width";
			float baseVal2;
			if (flag2)
			{
				baseVal2 = baseLaserInfo.width;
			}
			else
			{
				baseVal2 = 0f;
			}
			AbilityMod.AddToken(tokens, widthMod, tokenName3, desc2, baseVal2, flag2, false, false);
			AbilityModPropertyInt maxTargetsMod = laserInfoMod.m_maxTargetsMod;
			string tokenName4 = tokenName + "_MaxTargets";
			string desc3 = "laser max targets";
			int baseVal3;
			if (flag2)
			{
				baseVal3 = baseLaserInfo.maxTargets;
			}
			else
			{
				baseVal3 = 0;
			}
			AbilityMod.AddToken(tokens, maxTargetsMod, tokenName4, desc3, baseVal3, flag2, false);
		}
	}

	public static void AddToken_ConeInfo(List<TooltipTokenEntry> tokens, AbilityModPropertyConeInfo coneInfoMod, string tokenName, ConeTargetingInfo baseConeInfo = null, bool compareWithBase = true)
	{
		if (coneInfoMod != null)
		{
			bool flag;
			if (compareWithBase)
			{
				flag = (baseConeInfo != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			AbilityModPropertyFloat radiusMod = coneInfoMod.m_radiusMod;
			string tokenName2 = tokenName + "_Radius";
			string desc = "cone radius";
			float baseVal;
			if (flag2)
			{
				baseVal = baseConeInfo.m_radiusInSquares;
			}
			else
			{
				baseVal = 0f;
			}
			AbilityMod.AddToken(tokens, radiusMod, tokenName2, desc, baseVal, flag2, false, false);
			AbilityMod.AddToken(tokens, coneInfoMod.m_widthAngleMod, tokenName + "_Width", "cone width angle", (!flag2) ? 0f : baseConeInfo.m_widthAngleDeg, flag2, false, false);
		}
	}

	public static void AddToken_EffectInfo(List<TooltipTokenEntry> entries, StandardEffectInfo effectInfo, string tokenName, StandardEffectInfo baseVal = null, bool compareWithBase = true)
	{
		if (effectInfo != null)
		{
			if (effectInfo.m_applyEffect)
			{
				bool flag;
				if (compareWithBase && baseVal != null)
				{
					flag = baseVal.m_applyEffect;
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				effectInfo.m_effectData.AddTooltipTokens(entries, tokenName, flag2, (!flag2) ? null : baseVal.m_effectData);
			}
		}
	}

	public static void AddToken_EffectMod(List<TooltipTokenEntry> entries, AbilityModPropertyEffectInfo modProp, string tokenName, StandardEffectInfo baseVal = null, bool compareWithBase = true)
	{
		if (modProp != null)
		{
			if (modProp.operation != AbilityModPropertyEffectInfo.ModOp.Ignore && modProp.effectInfo.m_applyEffect)
			{
				AbilityMod.AddToken_EffectInfo(entries, modProp.effectInfo, tokenName, baseVal, compareWithBase);
				return;
			}
		}
	}

	public static void AddToken_EffectMod(List<TooltipTokenEntry> entries, AbilityModPropertyEffectData modProp, string tokenName, StandardActorEffectData baseVal = null, bool compareWithBase = true)
	{
		if (modProp != null)
		{
			if (modProp.operation != AbilityModPropertyEffectData.ModOp.Ignore)
			{
				if (modProp.effectData != null)
				{
					modProp.effectData.AddTooltipTokens(entries, tokenName, compareWithBase, baseVal);
					return;
				}
			}
		}
	}

	public static void AddToken_BarrierMod(List<TooltipTokenEntry> entries, AbilityModPropertyBarrierDataV2 modProp, string tokenName, StandardBarrierData baseVal)
	{
		if (modProp != null && baseVal != null)
		{
			if (modProp.operation != AbilityModPropertyBarrierDataV2.ModOp.Ignore)
			{
				if (modProp.barrierModData != null)
				{
					StandardBarrierData modifiedCopy = modProp.barrierModData.GetModifiedCopy(baseVal);
					modifiedCopy.AddTooltipTokens(entries, tokenName, true, baseVal);
					return;
				}
			}
		}
	}

	public static void AddToken_GroundFieldMod(List<TooltipTokenEntry> entries, AbilityModPropertyGroundEffectField modProp, string tokenName, GroundEffectField baseVal)
	{
		if (modProp != null)
		{
			if (baseVal != null)
			{
				if (modProp.operation != AbilityModPropertyGroundEffectField.ModOp.Ignore)
				{
					if (modProp.groundFieldModData != null)
					{
						GroundEffectField modifiedCopy = modProp.groundFieldModData.GetModifiedCopy(baseVal);
						modifiedCopy.AddTooltipTokens(entries, tokenName, true, baseVal);
						return;
					}
				}
			}
		}
	}

	public string GetAutogenDesc(AbilityData abilityData = null)
	{
		Ability targetAbilityOnAbilityData = this.GetTargetAbilityOnAbilityData(abilityData);
		bool flag = targetAbilityOnAbilityData != null;
		string text = string.Empty;
		string color = "lime";
		if (this.m_useRunPriorityOverride)
		{
			text += InEditorDescHelper.ColoredString("[Run Phase Override] = " + this.m_runPriorityOverride.ToString() + "\n", color, false);
		}
		text += this.PropDesc(this.m_techPointCostMod, InEditorDescHelper.ColoredString("[TechPoint Cost]", color, false), flag, (!flag) ? 0 : targetAbilityOnAbilityData.m_techPointsCost);
		string str = text;
		AbilityModPropertyInt maxCooldownMod = this.m_maxCooldownMod;
		string prefix = InEditorDescHelper.ColoredString("[Max Cooldown]", color, false);
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = targetAbilityOnAbilityData.m_cooldown;
		}
		else
		{
			baseVal = 0;
		}
		text = str + this.PropDesc(maxCooldownMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt maxStocksMod = this.m_maxStocksMod;
		string prefix2 = InEditorDescHelper.ColoredString("[Max Stock]", color, false);
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = targetAbilityOnAbilityData.m_maxStocks;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + this.PropDesc(maxStocksMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt stockRefreshDurationMod = this.m_stockRefreshDurationMod;
		string prefix3 = InEditorDescHelper.ColoredString("[Stock Refresh Duration]", color, false);
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = targetAbilityOnAbilityData.m_stockRefreshDuration;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + this.PropDesc(stockRefreshDurationMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool refillAllStockOnRefreshMod = this.m_refillAllStockOnRefreshMod;
		string prefix4 = InEditorDescHelper.ColoredString("[Refill All Stock on Refresh]", color, false);
		bool showBaseVal4 = flag;
		bool baseVal4;
		if (flag)
		{
			baseVal4 = targetAbilityOnAbilityData.m_refillAllStockOnRefresh;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + this.PropDesc(refillAllStockOnRefreshMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool isFreeActionMod = this.m_isFreeActionMod;
		string prefix5 = InEditorDescHelper.ColoredString("[Free Action Override]", color, false);
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = targetAbilityOnAbilityData.m_freeAction;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + this.PropDesc(isFreeActionMod, prefix5, showBaseVal5, baseVal5);
		text += this.PropDesc(this.m_autoQueueIfValidMod, InEditorDescHelper.ColoredString("[Auto Queue if Valid]", color, false), false, false);
		text += this.PropDesc(this.m_targetDataMaxRangeMod, InEditorDescHelper.ColoredString("[TargetData Max Range]", color, false), false, 0f);
		text += this.PropDesc(this.m_targetDataMinRangeMod, InEditorDescHelper.ColoredString("[TargetData Min Range]", color, false), false, 0f);
		text += this.PropDesc(this.m_targetDataCheckLosMod, InEditorDescHelper.ColoredString("[TargetData Check LoS]", color, false), false, false);
		if (this.m_useTargetDataOverrides)
		{
			if (this.m_targetDataOverrides != null)
			{
				text += InEditorDescHelper.ColoredString("Using Target Data override, with " + this.m_targetDataOverrides.Length + " entries:\n", color, false);
				foreach (TargetData targetData in this.m_targetDataOverrides)
				{
					string str6 = text;
					string str7 = "    [Paradigm] ";
					string str8;
					if (targetData.m_targetingParadigm > (Ability.TargetingParadigm)0)
					{
						str8 = targetData.m_targetingParadigm.ToString();
					}
					else
					{
						str8 = "INVALID";
					}
					text = str6 + str7 + str8;
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						", [Range (without range mods)] ",
						targetData.m_minRange,
						" to ",
						targetData.m_range
					});
					text = text + ", [Require Los] = " + targetData.m_checkLineOfSight.ToString() + "\n";
				}
			}
		}
		bool flag2 = false;
		if (this.m_effectToSelfOnCast != null)
		{
			if (this.m_effectToSelfOnCast.m_applyEffect)
			{
				text += InEditorDescHelper.ColoredString("Applies effect to Self on Cast:\n", color, false);
				text = text + this.m_effectToSelfOnCast.m_effectData.GetInEditorDescription(string.Empty, true, false, null) + "\n";
				flag2 = true;
			}
		}
		if (this.m_effectToTargetEnemyOnHit != null)
		{
			if (this.m_effectToTargetEnemyOnHit.m_applyEffect)
			{
				text += InEditorDescHelper.ColoredString("Applies effect to Targeted Enemy on Hit:\n", color, false);
				text = text + this.m_effectToTargetEnemyOnHit.m_effectData.GetInEditorDescription(string.Empty, true, false, null) + "\n";
				flag2 = true;
			}
		}
		if (this.m_effectToTargetAllyOnHit != null)
		{
			if (this.m_effectToTargetAllyOnHit.m_applyEffect)
			{
				text += InEditorDescHelper.ColoredString("Applies effect to Targeted Ally on Hit:\n", color, false);
				text = text + this.m_effectToTargetAllyOnHit.m_effectData.GetInEditorDescription(string.Empty, true, false, null) + "\n";
				if (this.m_useAllyEffectForTargetedCaster)
				{
					text += "\t(also applies to self if targeted)\n";
				}
				flag2 = true;
			}
		}
		if (this.m_effectTriggerChance < 1f)
		{
			if (flag2)
			{
				text += InEditorDescHelper.ColoredString(string.Format("        {0}% of the time", this.m_effectTriggerChance * 100f), color, false);
				if (this.m_effectTriggerChanceMultipliedPerHit)
				{
					text += InEditorDescHelper.ColoredString(" (per hit)", color, false);
				}
				text += "\n";
			}
		}
		if (this.m_cooldownReductionsOnSelf != null && this.m_cooldownReductionsOnSelf.HasCooldownReduction())
		{
			text += InEditorDescHelper.ColoredString("Cooldown Reductions on Cast:\n", color, false);
			text += this.m_cooldownReductionsOnSelf.GetDescription(abilityData);
		}
		if (this.m_techPointInteractionMods != null)
		{
			bool flag3 = false;
			if (targetAbilityOnAbilityData != null)
			{
				if (targetAbilityOnAbilityData.m_techPointInteractions != null)
				{
					flag3 = true;
				}
			}
			foreach (TechPointInteractionMod techPointInteractionMod in this.m_techPointInteractionMods)
			{
				int baseVal6 = 0;
				if (flag3)
				{
					foreach (TechPointInteraction techPointInteraction in targetAbilityOnAbilityData.m_techPointInteractions)
					{
						if (techPointInteraction.m_type == techPointInteractionMod.interactionType)
						{
							baseVal6 = techPointInteraction.m_amount;
							goto IL_64F;
						}
					}
				}
				IL_64F:
				text += InEditorDescHelper.ColoredString(AbilityModHelper.GetTechPointModDesc(techPointInteractionMod, flag3, baseVal6), color, false);
			}
		}
		if (this.m_useActionAnimTypeOverride)
		{
			text += InEditorDescHelper.ColoredString("Using Action Anim Type Override, " + this.m_actionAnimTypeOverride.ToString() + "\n\n", color, false);
		}
		if (this.m_useMovementAdjustmentOverride)
		{
			text += InEditorDescHelper.ColoredString("Using Movement Adjustment Override, " + this.m_movementAdjustmentOverride.ToString() + "\n\n", color, false);
		}
		if (this.m_chainAbilityModInfo != null)
		{
			using (List<ChainAbilityAdditionalModInfo>.Enumerator enumerator = this.m_chainAbilityModInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ChainAbilityAdditionalModInfo chainAbilityAdditionalModInfo = enumerator.Current;
					text += chainAbilityAdditionalModInfo.GetDescription(abilityData, targetAbilityOnAbilityData, this);
				}
			}
		}
		if (this.m_useChainAbilityOverrides)
		{
			text += InEditorDescHelper.ColoredString("Using Chain Ability Override\n", color, false);
			foreach (Ability ability in this.m_chainAbilityOverrides)
			{
				if (ability != null)
				{
					text = text + "    Chain Ability: " + ability.m_abilityName + "\n";
					if (ability.m_abilityName == "Base Ability")
					{
						text += "        (Please give a name to this ability for easier identification)\n";
					}
				}
			}
		}
		if (this.m_tagsModType != AbilityMod.TagOverrideType.Ignore)
		{
			text += InEditorDescHelper.ColoredString("Using Tag Mods, type = " + this.m_tagsModType.ToString() + ":\n", color, false);
			foreach (AbilityTags abilityTags in this.m_abilityTagsInMod)
			{
				text = text + "    " + abilityTags.ToString() + "\n";
			}
		}
		if (this.m_statModsWhileEquipped != null)
		{
			if (this.m_statModsWhileEquipped.Length > 0)
			{
				text += InEditorDescHelper.ColoredString("Stat Mods while Equipped:\n", color, false);
				foreach (AbilityStatMod abilityStatMod in this.m_statModsWhileEquipped)
				{
					text = text + "    " + abilityStatMod.ToString() + "\n";
				}
				text += "\n";
			}
		}
		if (this.m_useStatusWhenRequestedOverride)
		{
			if (this.m_statusWhenRequestedOverride.Count > 0)
			{
				text += InEditorDescHelper.ColoredString("Buff/Debuff Status on ability request (in Decision):\n", color, false);
				using (List<StatusType>.Enumerator enumerator3 = this.m_statusWhenRequestedOverride.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						StatusType statusType = enumerator3.Current;
						text = text + "    [ " + InEditorDescHelper.ColoredString(statusType.ToString(), "cyan", false) + " ]\n";
					}
				}
				text += "\n";
			}
		}
		if (this.beginningOfModSpecificData)
		{
			text += "Meow! (Beginning of Mod Specific Data checkbox does nothing)";
		}
		if (text.Length > 0)
		{
			text += "\n";
		}
		string str9 = string.Empty;
		try
		{
			str9 = this.ModSpecificAutogenDesc(abilityData);
		}
		catch (Exception ex)
		{
			if (Application.isEditor)
			{
				Debug.LogError("Exception while trying to generate mod specific description. StackTrace:\n" + ex.StackTrace);
			}
		}
		return text + str9;
	}

	protected virtual string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		return string.Empty;
	}

	protected Ability GetTargetAbilityOnAbilityData(AbilityData abilityData)
	{
		if (abilityData != null)
		{
			List<Ability> abilitiesAsList = abilityData.GetAbilitiesAsList();
			foreach (Ability ability in abilitiesAsList)
			{
				if (ability != null)
				{
					if (ability.GetType() == this.GetTargetAbilityType())
					{
						return ability;
					}
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

	public List<int> symbol_001D(Ability symbol_001D)
	{
		List<int> list = new List<int>();
		if (symbol_001D != null)
		{
			int techPointsCost = symbol_001D.m_techPointsCost;
			int modifiedValue = this.m_techPointCostMod.GetModifiedValue(techPointsCost);
			if (techPointsCost != modifiedValue)
			{
				list.Add(modifiedValue);
				list.Add(Mathf.Abs(techPointsCost - modifiedValue));
			}
		}
		this.AppendTooltipCheckNumbersFromTargetDataEntresi(symbol_001D, list);
		this.AppendTooltipCheckNumbersFromTechPointInteractions(symbol_001D, list);
		if (this.m_maxCooldownMod.operation != AbilityModPropertyInt.ModOp.Ignore)
		{
			int num = (int)Mathf.Abs(this.m_maxCooldownMod.value);
			if (num != 0)
			{
				list.Add(num);
			}
			if (symbol_001D != null)
			{
				int modifiedValue2 = this.m_maxCooldownMod.GetModifiedValue(symbol_001D.m_cooldown);
				int num2 = symbol_001D.m_cooldown - modifiedValue2;
				if (num2 != 0)
				{
					list.Add(Mathf.Abs(num2));
					list.Add(Mathf.Abs(modifiedValue2));
				}
			}
		}
		this.AppendModSpecificTooltipCheckNumbers(symbol_001D, list);
		return list;
	}

	private void AppendTooltipCheckNumbersFromTargetDataEntresi(Ability ability, List<int> numbers)
	{
		if (ability != null)
		{
			if (ability.m_targetData != null)
			{
				if (ability.m_targetData.Length > 0)
				{
					float minRange = ability.m_targetData[0].m_minRange;
					float range = ability.m_targetData[0].m_range;
					float modifiedValue = this.m_targetDataMinRangeMod.GetModifiedValue(minRange);
					float modifiedValue2 = this.m_targetDataMaxRangeMod.GetModifiedValue(range);
					int num = Mathf.Abs(Mathf.RoundToInt(minRange - modifiedValue));
					int num2 = Mathf.Abs(Mathf.RoundToInt(range - modifiedValue2));
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
						int num = techPointInteraction.m_amount;
						num = this.GetModdedTechPointForInteraction(techPointInteraction.m_type, num);
						if (num > 0)
						{
							dictionary.Add(techPointInteraction.m_type, num);
						}
					}
				}
			}
			foreach (TechPointInteractionMod techPointInteractionMod in this.m_techPointInteractionMods)
			{
				if (!dictionary.ContainsKey(techPointInteractionMod.interactionType))
				{
					int moddedTechPointForInteraction = this.GetModdedTechPointForInteraction(techPointInteractionMod.interactionType, 0);
					if (moddedTechPointForInteraction > 0)
					{
						dictionary.Add(techPointInteractionMod.interactionType, moddedTechPointForInteraction);
					}
				}
			}
			using (Dictionary<TechPointInteractionType, int>.Enumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<TechPointInteractionType, int> keyValuePair = enumerator.Current;
					if (keyValuePair.Value > 0)
					{
						numbers.Add(keyValuePair.Value);
					}
				}
			}
		}
	}

	public string GetDebugIdentifier(string colorString = "")
	{
		string text = string.Concat(new object[]
		{
			"Mod[ ",
			this.m_abilityScopeId,
			" ] ",
			this.m_name,
			" "
		});
		if (colorString.Length > 0)
		{
			return string.Concat(new string[]
			{
				"<color=",
				colorString,
				">",
				text,
				"</color>"
			});
		}
		return text;
	}

	public enum TagOverrideType
	{
		Ignore,
		Override,
		Append
	}
}
