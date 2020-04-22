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

	public string m_name = string.Empty;

	public bool m_availableInGame = true;

	public AbilityModGameTypeReq m_gameTypeReq;

	[Space(5f)]
	public int m_equipCost = 1;

	public bool m_defaultEquip;

	[TextArea(1, 20)]
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
		if (text.Length == 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_name.Length > 0)
			{
				text = m_name;
			}
		}
		return text;
	}

	public string GetFullTooltip(Ability ability)
	{
		string text = StringUtil.TR_AbilityModFinalTooltip(GetTargetAbilityType().ToString(), m_name);
		if (text.Length == 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_tooltip.Length > 0)
			{
				text = m_tooltip;
			}
		}
		return TooltipTokenEntry.GetTooltipWithSubstitutes(text, GetTooltipTokenEntries(ability));
	}

	public string GetUnlocalizedFullTooltip(Ability ability)
	{
		if (string.IsNullOrEmpty(m_debugUnlocalizedTooltip))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return TooltipTokenEntry.GetTooltipWithSubstitutes(m_tooltip, GetTooltipTokenEntries(ability));
				}
			}
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
		if (m_savedStatusTypesForTooltips != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_savedStatusTypesForTooltips.Count != 0)
			{
				return m_savedStatusTypesForTooltips;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return TooltipTokenEntry.GetStatusTypesFromTooltip(m_tooltip);
	}

	public ChainAbilityAdditionalModInfo GetChainModInfoAtIndex(int chainIndex)
	{
		using (List<ChainAbilityAdditionalModInfo>.Enumerator enumerator = m_chainAbilityModInfo.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ChainAbilityAdditionalModInfo current = enumerator.Current;
				if (current.m_chainAbilityIndex == chainIndex)
				{
					return current;
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					goto end_IL_000c;
				}
			}
			end_IL_000c:;
		}
		return null;
	}

	public int GetModdedTechPointForInteraction(TechPointInteractionType interactionType, int baseAmount)
	{
		TechPointInteractionMod[] techPointInteractionMods = m_techPointInteractionMods;
		foreach (TechPointInteractionMod techPointInteractionMod in techPointInteractionMods)
		{
			if (techPointInteractionMod.interactionType == interactionType)
			{
				return techPointInteractionMod.modAmount.GetModifiedValue(baseAmount);
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return baseAmount;
		}
	}

	public bool EquippableForGameType()
	{
		bool result = true;
		if (m_gameTypeReq == AbilityModGameTypeReq.ExcludeFromRanked)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ModStrictness requiredModStrictnessForGameSubType = GetRequiredModStrictnessForGameSubType();
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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!GameManager.Get().GameConfig.SubTypes.IsNullOrEmpty())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameManager.Get().GameConfig.InstanceSubType != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (GameManager.Get().GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.StricterMods))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						result = ModStrictness.Ranked;
					}
					goto IL_019c;
				}
			}
		}
		if (ClientGameManager.Get() != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (ClientGameManager.Get().GroupInfo != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				GameType selectedQueueType = ClientGameManager.Get().GroupInfo.SelectedQueueType;
				int subTypeMask = ClientGameManager.Get().GroupInfo.SubTypeMask;
				if (selectedQueueType == GameType.Ranked)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					result = ModStrictness.Ranked;
					goto IL_019c;
				}
				Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(selectedQueueType);
				using (Dictionary<ushort, GameSubType>.Enumerator enumerator = gameTypeSubTypes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ushort, GameSubType> current = enumerator.Current;
						if ((current.Key & subTypeMask) != 0 && current.Value.HasMod(GameSubType.SubTypeMods.StricterMods))
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									return ModStrictness.Ranked;
								}
							}
						}
					}
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return result;
						}
					}
				}
			}
		}
		Log.Error("Failed to check mod strictness for unknown game type on client.");
		goto IL_019c;
		IL_019c:
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
			AddModSpecificTooltipTokens(list, ability);
		}
		catch (Exception ex)
		{
			Debug.LogError("Exception while trying to add mod specific tooltip tokens for " + GetDebugIdentifier(string.Empty) + " | " + GetTargetAbilityType().ToString() + "\nStackTrace:\n" + ex.StackTrace);
		}
		if (ability != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AddToken(list, m_techPointCostMod, "EnergyCost", "energy cost", ability.m_techPointsCost);
			AddToken(list, m_maxCooldownMod, "MaxCooldown", "max cooldown", ability.m_cooldown);
			AddToken(list, m_maxStocksMod, "MaxStocks", "max stocks", ability.m_maxStocks);
			AddToken(list, m_stockRefreshDurationMod, "StockRefreshDur", "stock refresh duration", ability.m_stockRefreshDuration);
			if (ability.m_targetData != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (ability.m_targetData.Length > 0)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					AddToken(list, m_targetDataMinRangeMod, "TargetDataMinRange", "min range of targeter when using square based targeters", ability.m_targetData[0].m_minRange);
					AddToken(list, m_targetDataMaxRangeMod, "TargetDataMaxRange", "MAX range of targeter when using square based targeters", ability.m_targetData[0].m_range);
				}
			}
			if (m_useTargetDataOverrides)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_targetDataOverrides != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_targetDataOverrides.Length > 0)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (ability.m_targetData.Length > 0)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							AddToken_IntDiff(list, "BM_TargetDataMaxRange_0", string.Empty, Mathf.RoundToInt(m_targetDataOverrides[0].m_range), false, 0);
							AddToken_IntDiff(list, "BM_TargetDataMaxRange_0_Diff", string.Empty, Mathf.RoundToInt(m_targetDataOverrides[0].m_range - ability.m_targetData[0].m_range), false, 0);
						}
					}
				}
			}
			AddToken_EffectInfo(list, m_effectToSelfOnCast, "BM_EffectToSelf");
			AddToken_EffectInfo(list, m_effectToTargetAllyOnHit, "BM_EffectToAllyHit");
			AddToken_EffectInfo(list, m_effectToTargetEnemyOnHit, "BM_EffetToEnemyHit");
			int num = Mathf.RoundToInt(100f * m_effectTriggerChance);
			if (num > 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (num < 100)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					list.Add(new TooltipTokenInt("BM_EffectApplyChance", "chance to apply effects on base mod", num));
				}
			}
			AddTokensForTechPointInteractions(list, ability);
			if (m_cooldownReductionsOnSelf != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				m_cooldownReductionsOnSelf.AddTooltipTokens(list, "OnSelf");
			}
			if (m_chainAbilityModInfo != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						using (List<ChainAbilityAdditionalModInfo>.Enumerator enumerator = m_chainAbilityModInfo.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								ChainAbilityAdditionalModInfo current = enumerator.Current;
								current.AddTooltipTokens(list, ability, this, "ChainMod");
							}
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									return list;
								}
							}
						}
					}
					}
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
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			bool flag = false;
			if (ability != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (ability.m_techPointInteractions != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
				}
			}
			TechPointInteractionMod[] techPointInteractionMods = m_techPointInteractionMods;
			foreach (TechPointInteractionMod techPointInteractionMod in techPointInteractionMods)
			{
				int baseVal = 0;
				if (flag)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					TechPointInteraction[] techPointInteractions = ability.m_techPointInteractions;
					int num = 0;
					while (true)
					{
						if (num < techPointInteractions.Length)
						{
							TechPointInteraction techPointInteraction = techPointInteractions[num];
							if (techPointInteraction.m_type == techPointInteractionMod.interactionType)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								baseVal = techPointInteraction.m_amount;
								break;
							}
							num++;
							continue;
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						break;
					}
				}
				AddToken(tokens, techPointInteractionMod.modAmount, techPointInteractionMod.interactionType.ToString(), "Energy Gain", baseVal);
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public static void AddToken(List<TooltipTokenEntry> entries, AbilityModPropertyInt modProp, string tokenName, string desc, int baseVal, bool addCompare = true, bool addForZeroBase = false)
	{
		if (modProp == null)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (modProp.operation == AbilityModPropertyInt.ModOp.Ignore)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				int modifiedValue = modProp.GetModifiedValue(baseVal);
				entries.Add(new TooltipTokenInt(tokenName + "_Final", desc + " | Final Value", Mathf.Abs(modifiedValue)));
				if (!addCompare)
				{
					return;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					if (baseVal == 0)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!addForZeroBase)
						{
							return;
						}
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					int val = Mathf.Abs(modifiedValue - baseVal);
					entries.Add(new TooltipTokenInt(tokenName + "_Diff", desc + " | Difference", val));
					return;
				}
			}
		}
	}

	public static void AddToken(List<TooltipTokenEntry> entries, AbilityModPropertyFloat modProp, string tokenName, string desc, float baseVal, bool addCompare = true, bool addDecimal = false, bool addAsPct = false)
	{
		if (modProp == null || modProp.operation == AbilityModPropertyFloat.ModOp.Ignore)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			float modifiedValue = modProp.GetModifiedValue(baseVal);
			entries.Add(new TooltipTokenInt(tokenName + "_Final", desc + " | Final Value", Mathf.RoundToInt(modifiedValue)));
			if (addCompare && baseVal != 0f)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				int val = Mathf.Abs(Mathf.RoundToInt(modifiedValue - baseVal));
				entries.Add(new TooltipTokenInt(tokenName + "_Diff", desc + " | Difference", val));
			}
			if (addDecimal)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				entries.Add(new TooltipTokenFloat(tokenName + "_Decimal", desc + " | As Decimal Num", modifiedValue));
			}
			if (addAsPct)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					entries.Add(new TooltipTokenPct(tokenName + "_Pct", desc + " | as percent", Mathf.RoundToInt(100f * modifiedValue)));
					return;
				}
			}
			return;
		}
	}

	public static void AddToken_IntDiff(List<TooltipTokenEntry> tokens, string name, string desc, int val, bool addDiff, int otherVal)
	{
		if (val <= 0)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			tokens.Add(new TooltipTokenInt(name, desc + " | Final Value", val));
			if (!addDiff)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (otherVal <= 0)
				{
					return;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					int num = Mathf.Abs(val - otherVal);
					if (num > 0)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							tokens.Add(new TooltipTokenInt(name + "_Diff", desc + " | Difference", num));
							return;
						}
					}
					return;
				}
			}
		}
	}

	public static void AddToken_LaserInfo(List<TooltipTokenEntry> tokens, AbilityModPropertyLaserInfo laserInfoMod, string tokenName, LaserTargetingInfo baseLaserInfo = null, bool compareWithBase = true)
	{
		if (laserInfoMod == null)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			int num;
			if (compareWithBase)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				num = ((baseLaserInfo != null) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag = (byte)num != 0;
			AbilityModPropertyFloat rangeMod = laserInfoMod.m_rangeMod;
			string tokenName2 = tokenName + "_Range";
			float baseVal;
			if (flag)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				baseVal = baseLaserInfo.range;
			}
			else
			{
				baseVal = 0f;
			}
			AddToken(tokens, rangeMod, tokenName2, "laser range", baseVal, flag);
			AbilityModPropertyFloat widthMod = laserInfoMod.m_widthMod;
			string tokenName3 = tokenName + "_Width";
			float baseVal2;
			if (flag)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				baseVal2 = baseLaserInfo.width;
			}
			else
			{
				baseVal2 = 0f;
			}
			AddToken(tokens, widthMod, tokenName3, "laser width", baseVal2, flag);
			AbilityModPropertyInt maxTargetsMod = laserInfoMod.m_maxTargetsMod;
			string tokenName4 = tokenName + "_MaxTargets";
			int baseVal3;
			if (flag)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				baseVal3 = baseLaserInfo.maxTargets;
			}
			else
			{
				baseVal3 = 0;
			}
			AddToken(tokens, maxTargetsMod, tokenName4, "laser max targets", baseVal3, flag);
			return;
		}
	}

	public static void AddToken_ConeInfo(List<TooltipTokenEntry> tokens, AbilityModPropertyConeInfo coneInfoMod, string tokenName, ConeTargetingInfo baseConeInfo = null, bool compareWithBase = true)
	{
		if (coneInfoMod == null)
		{
			return;
		}
		int num;
		if (compareWithBase)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = ((baseConeInfo != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		AbilityModPropertyFloat radiusMod = coneInfoMod.m_radiusMod;
		string tokenName2 = tokenName + "_Radius";
		float baseVal;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal = baseConeInfo.m_radiusInSquares;
		}
		else
		{
			baseVal = 0f;
		}
		AddToken(tokens, radiusMod, tokenName2, "cone radius", baseVal, flag);
		AddToken(tokens, coneInfoMod.m_widthAngleMod, tokenName + "_Width", "cone width angle", (!flag) ? 0f : baseConeInfo.m_widthAngleDeg, flag);
	}

	public static void AddToken_EffectInfo(List<TooltipTokenEntry> entries, StandardEffectInfo effectInfo, string tokenName, StandardEffectInfo baseVal = null, bool compareWithBase = true)
	{
		if (effectInfo == null)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!effectInfo.m_applyEffect)
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				int num;
				if (compareWithBase && baseVal != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					num = (baseVal.m_applyEffect ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				bool flag = (byte)num != 0;
				effectInfo.m_effectData.AddTooltipTokens(entries, tokenName, flag, (!flag) ? null : baseVal.m_effectData);
				return;
			}
		}
	}

	public static void AddToken_EffectMod(List<TooltipTokenEntry> entries, AbilityModPropertyEffectInfo modProp, string tokenName, StandardEffectInfo baseVal = null, bool compareWithBase = true)
	{
		if (modProp == null)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (modProp.operation != 0 && modProp.effectInfo.m_applyEffect)
			{
				AddToken_EffectInfo(entries, modProp.effectInfo, tokenName, baseVal, compareWithBase);
			}
			return;
		}
	}

	public static void AddToken_EffectMod(List<TooltipTokenEntry> entries, AbilityModPropertyEffectData modProp, string tokenName, StandardActorEffectData baseVal = null, bool compareWithBase = true)
	{
		if (modProp == null)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (modProp.operation == AbilityModPropertyEffectData.ModOp.Ignore)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (modProp.effectData == null)
				{
					while (true)
					{
						switch (6)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				modProp.effectData.AddTooltipTokens(entries, tokenName, compareWithBase, baseVal);
				return;
			}
		}
	}

	public static void AddToken_BarrierMod(List<TooltipTokenEntry> entries, AbilityModPropertyBarrierDataV2 modProp, string tokenName, StandardBarrierData baseVal)
	{
		if (modProp == null || baseVal == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (modProp.operation == AbilityModPropertyBarrierDataV2.ModOp.Ignore)
			{
				return;
			}
			if (modProp.barrierModData == null)
			{
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			StandardBarrierData modifiedCopy = modProp.barrierModData.GetModifiedCopy(baseVal);
			modifiedCopy.AddTooltipTokens(entries, tokenName, true, baseVal);
			return;
		}
	}

	public static void AddToken_GroundFieldMod(List<TooltipTokenEntry> entries, AbilityModPropertyGroundEffectField modProp, string tokenName, GroundEffectField baseVal)
	{
		if (modProp == null)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (baseVal == null)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (modProp.operation == AbilityModPropertyGroundEffectField.ModOp.Ignore)
				{
					return;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					if (modProp.groundFieldModData == null)
					{
						while (true)
						{
							switch (4)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					GroundEffectField modifiedCopy = modProp.groundFieldModData.GetModifiedCopy(baseVal);
					modifiedCopy.AddTooltipTokens(entries, tokenName, true, baseVal);
					return;
				}
			}
		}
	}

	public string GetAutogenDesc(AbilityData abilityData = null)
	{
		Ability targetAbilityOnAbilityData = GetTargetAbilityOnAbilityData(abilityData);
		bool flag = targetAbilityOnAbilityData != null;
		string str = string.Empty;
		string color = "lime";
		if (m_useRunPriorityOverride)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			str += InEditorDescHelper.ColoredString("[Run Phase Override] = " + m_runPriorityOverride.ToString() + "\n", color);
		}
		str += PropDesc(m_techPointCostMod, InEditorDescHelper.ColoredString("[TechPoint Cost]", color), flag, flag ? targetAbilityOnAbilityData.m_techPointsCost : 0);
		string str2 = str;
		AbilityModPropertyInt maxCooldownMod = m_maxCooldownMod;
		string prefix = InEditorDescHelper.ColoredString("[Max Cooldown]", color);
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal = targetAbilityOnAbilityData.m_cooldown;
		}
		else
		{
			baseVal = 0;
		}
		str = str2 + PropDesc(maxCooldownMod, prefix, flag, baseVal);
		string str3 = str;
		AbilityModPropertyInt maxStocksMod = m_maxStocksMod;
		string prefix2 = InEditorDescHelper.ColoredString("[Max Stock]", color);
		int baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = targetAbilityOnAbilityData.m_maxStocks;
		}
		else
		{
			baseVal2 = 0;
		}
		str = str3 + PropDesc(maxStocksMod, prefix2, flag, baseVal2);
		string str4 = str;
		AbilityModPropertyInt stockRefreshDurationMod = m_stockRefreshDurationMod;
		string prefix3 = InEditorDescHelper.ColoredString("[Stock Refresh Duration]", color);
		int baseVal3;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = targetAbilityOnAbilityData.m_stockRefreshDuration;
		}
		else
		{
			baseVal3 = 0;
		}
		str = str4 + PropDesc(stockRefreshDurationMod, prefix3, flag, baseVal3);
		string str5 = str;
		AbilityModPropertyBool refillAllStockOnRefreshMod = m_refillAllStockOnRefreshMod;
		string prefix4 = InEditorDescHelper.ColoredString("[Refill All Stock on Refresh]", color);
		int baseVal4;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = (targetAbilityOnAbilityData.m_refillAllStockOnRefresh ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		str = str5 + PropDesc(refillAllStockOnRefreshMod, prefix4, flag, (byte)baseVal4 != 0);
		string str6 = str;
		AbilityModPropertyBool isFreeActionMod = m_isFreeActionMod;
		string prefix5 = InEditorDescHelper.ColoredString("[Free Action Override]", color);
		int baseVal5;
		if (flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = (targetAbilityOnAbilityData.m_freeAction ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		str = str6 + PropDesc(isFreeActionMod, prefix5, flag, (byte)baseVal5 != 0);
		str += PropDesc(m_autoQueueIfValidMod, InEditorDescHelper.ColoredString("[Auto Queue if Valid]", color));
		str += PropDesc(m_targetDataMaxRangeMod, InEditorDescHelper.ColoredString("[TargetData Max Range]", color));
		str += PropDesc(m_targetDataMinRangeMod, InEditorDescHelper.ColoredString("[TargetData Min Range]", color));
		str += PropDesc(m_targetDataCheckLosMod, InEditorDescHelper.ColoredString("[TargetData Check LoS]", color));
		if (m_useTargetDataOverrides)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_targetDataOverrides != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				str += InEditorDescHelper.ColoredString("Using Target Data override, with " + m_targetDataOverrides.Length + " entries:\n", color);
				TargetData[] targetDataOverrides = m_targetDataOverrides;
				foreach (TargetData targetData in targetDataOverrides)
				{
					string str7 = str;
					object str8;
					if (targetData.m_targetingParadigm > (Ability.TargetingParadigm)0)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						str8 = targetData.m_targetingParadigm.ToString();
					}
					else
					{
						str8 = "INVALID";
					}
					str = str7 + "    [Paradigm] " + (string)str8;
					string text = str;
					str = text + ", [Range (without range mods)] " + targetData.m_minRange + " to " + targetData.m_range;
					str = str + ", [Require Los] = " + targetData.m_checkLineOfSight + "\n";
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		bool flag2 = false;
		if (m_effectToSelfOnCast != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_effectToSelfOnCast.m_applyEffect)
			{
				str += InEditorDescHelper.ColoredString("Applies effect to Self on Cast:\n", color);
				str = str + m_effectToSelfOnCast.m_effectData.GetInEditorDescription(string.Empty) + "\n";
				flag2 = true;
			}
		}
		if (m_effectToTargetEnemyOnHit != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_effectToTargetEnemyOnHit.m_applyEffect)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				str += InEditorDescHelper.ColoredString("Applies effect to Targeted Enemy on Hit:\n", color);
				str = str + m_effectToTargetEnemyOnHit.m_effectData.GetInEditorDescription(string.Empty) + "\n";
				flag2 = true;
			}
		}
		if (m_effectToTargetAllyOnHit != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_effectToTargetAllyOnHit.m_applyEffect)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				str += InEditorDescHelper.ColoredString("Applies effect to Targeted Ally on Hit:\n", color);
				str = str + m_effectToTargetAllyOnHit.m_effectData.GetInEditorDescription(string.Empty) + "\n";
				if (m_useAllyEffectForTargetedCaster)
				{
					str += "\t(also applies to self if targeted)\n";
				}
				flag2 = true;
			}
		}
		if (m_effectTriggerChance < 1f)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (flag2)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				str += InEditorDescHelper.ColoredString($"        {m_effectTriggerChance * 100f}% of the time", color);
				if (m_effectTriggerChanceMultipliedPerHit)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					str += InEditorDescHelper.ColoredString(" (per hit)", color);
				}
				str += "\n";
			}
		}
		if (m_cooldownReductionsOnSelf != null && m_cooldownReductionsOnSelf.HasCooldownReduction())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			str += InEditorDescHelper.ColoredString("Cooldown Reductions on Cast:\n", color);
			str += m_cooldownReductionsOnSelf.GetDescription(abilityData);
		}
		if (m_techPointInteractionMods != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag3 = false;
			if (targetAbilityOnAbilityData != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (targetAbilityOnAbilityData.m_techPointInteractions != null)
				{
					flag3 = true;
				}
			}
			TechPointInteractionMod[] techPointInteractionMods = m_techPointInteractionMods;
			foreach (TechPointInteractionMod techPointInteractionMod in techPointInteractionMods)
			{
				int baseVal6 = 0;
				if (flag3)
				{
					TechPointInteraction[] techPointInteractions = targetAbilityOnAbilityData.m_techPointInteractions;
					int num = 0;
					while (true)
					{
						if (num < techPointInteractions.Length)
						{
							TechPointInteraction techPointInteraction = techPointInteractions[num];
							if (techPointInteraction.m_type == techPointInteractionMod.interactionType)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								baseVal6 = techPointInteraction.m_amount;
								break;
							}
							num++;
							continue;
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						break;
					}
				}
				str += InEditorDescHelper.ColoredString(AbilityModHelper.GetTechPointModDesc(techPointInteractionMod, flag3, baseVal6), color);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (m_useActionAnimTypeOverride)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			str += InEditorDescHelper.ColoredString("Using Action Anim Type Override, " + m_actionAnimTypeOverride.ToString() + "\n\n", color);
		}
		if (m_useMovementAdjustmentOverride)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			str += InEditorDescHelper.ColoredString("Using Movement Adjustment Override, " + m_movementAdjustmentOverride.ToString() + "\n\n", color);
		}
		if (m_chainAbilityModInfo != null)
		{
			using (List<ChainAbilityAdditionalModInfo>.Enumerator enumerator = m_chainAbilityModInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ChainAbilityAdditionalModInfo current = enumerator.Current;
					str += current.GetDescription(abilityData, targetAbilityOnAbilityData, this);
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (m_useChainAbilityOverrides)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			str += InEditorDescHelper.ColoredString("Using Chain Ability Override\n", color);
			Ability[] chainAbilityOverrides = m_chainAbilityOverrides;
			foreach (Ability ability in chainAbilityOverrides)
			{
				if (!(ability != null))
				{
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				str = str + "    Chain Ability: " + ability.m_abilityName + "\n";
				if (ability.m_abilityName == "Base Ability")
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					str += "        (Please give a name to this ability for easier identification)\n";
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (m_tagsModType != 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			str += InEditorDescHelper.ColoredString("Using Tag Mods, type = " + m_tagsModType.ToString() + ":\n", color);
			foreach (AbilityTags item in m_abilityTagsInMod)
			{
				str = str + "    " + item.ToString() + "\n";
			}
		}
		if (m_statModsWhileEquipped != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_statModsWhileEquipped.Length > 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				str += InEditorDescHelper.ColoredString("Stat Mods while Equipped:\n", color);
				AbilityStatMod[] statModsWhileEquipped = m_statModsWhileEquipped;
				foreach (AbilityStatMod abilityStatMod in statModsWhileEquipped)
				{
					str = str + "    " + abilityStatMod.ToString() + "\n";
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				str += "\n";
			}
		}
		if (m_useStatusWhenRequestedOverride)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_statusWhenRequestedOverride.Count > 0)
			{
				str += InEditorDescHelper.ColoredString("Buff/Debuff Status on ability request (in Decision):\n", color);
				using (List<StatusType>.Enumerator enumerator3 = m_statusWhenRequestedOverride.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						str = str + "    [ " + InEditorDescHelper.ColoredString(enumerator3.Current.ToString()) + " ]\n";
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				str += "\n";
			}
		}
		if (beginningOfModSpecificData)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			str += "Meow! (Beginning of Mod Specific Data checkbox does nothing)";
		}
		if (str.Length > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			str += "\n";
		}
		string str9 = string.Empty;
		try
		{
			str9 = ModSpecificAutogenDesc(abilityData);
		}
		catch (Exception ex)
		{
			if (Application.isEditor)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						Debug.LogError("Exception while trying to generate mod specific description. StackTrace:\n" + ex.StackTrace);
						goto end_IL_0a5d;
					}
				}
			}
			end_IL_0a5d:;
		}
		return str + str9;
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
			foreach (Ability item in abilitiesAsList)
			{
				if (item != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (item.GetType() == GetTargetAbilityType())
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return item;
							}
						}
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

	public List<int> _001D(Ability _001D)
	{
		List<int> list = new List<int>();
		if (_001D != null)
		{
			int techPointsCost = _001D.m_techPointsCost;
			int modifiedValue = m_techPointCostMod.GetModifiedValue(techPointsCost);
			if (techPointsCost != modifiedValue)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				list.Add(modifiedValue);
				list.Add(Mathf.Abs(techPointsCost - modifiedValue));
			}
		}
		AppendTooltipCheckNumbersFromTargetDataEntresi(_001D, list);
		AppendTooltipCheckNumbersFromTechPointInteractions(_001D, list);
		if (m_maxCooldownMod.operation != 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			int num = (int)Mathf.Abs(m_maxCooldownMod.value);
			if (num != 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				list.Add(num);
			}
			if (_001D != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				int modifiedValue2 = m_maxCooldownMod.GetModifiedValue(_001D.m_cooldown);
				int num2 = _001D.m_cooldown - modifiedValue2;
				if (num2 != 0)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					list.Add(Mathf.Abs(num2));
					list.Add(Mathf.Abs(modifiedValue2));
				}
			}
		}
		AppendModSpecificTooltipCheckNumbers(_001D, list);
		return list;
	}

	private void AppendTooltipCheckNumbersFromTargetDataEntresi(Ability ability, List<int> numbers)
	{
		if (!(ability != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (ability.m_targetData == null)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (ability.m_targetData.Length <= 0)
				{
					return;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					float minRange = ability.m_targetData[0].m_minRange;
					float range = ability.m_targetData[0].m_range;
					float modifiedValue = m_targetDataMinRangeMod.GetModifiedValue(minRange);
					float modifiedValue2 = m_targetDataMaxRangeMod.GetModifiedValue(range);
					int num = Mathf.Abs(Mathf.RoundToInt(minRange - modifiedValue));
					int num2 = Mathf.Abs(Mathf.RoundToInt(range - modifiedValue2));
					if (num > 0)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						numbers.Add(num);
					}
					if (num2 > 0)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							numbers.Add(num2);
							return;
						}
					}
					return;
				}
			}
		}
	}

	private void AppendTooltipCheckNumbersFromTechPointInteractions(Ability ability, List<int> numbers)
	{
		if (!(ability != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Dictionary<TechPointInteractionType, int> dictionary = new Dictionary<TechPointInteractionType, int>();
			if (ability.m_techPointInteractions != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				TechPointInteraction[] techPointInteractions = ability.m_techPointInteractions;
				for (int i = 0; i < techPointInteractions.Length; i++)
				{
					TechPointInteraction techPointInteraction = techPointInteractions[i];
					if (!dictionary.ContainsKey(techPointInteraction.m_type))
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						int amount = techPointInteraction.m_amount;
						amount = GetModdedTechPointForInteraction(techPointInteraction.m_type, amount);
						if (amount > 0)
						{
							dictionary.Add(techPointInteraction.m_type, amount);
						}
					}
				}
			}
			TechPointInteractionMod[] techPointInteractionMods = m_techPointInteractionMods;
			foreach (TechPointInteractionMod techPointInteractionMod in techPointInteractionMods)
			{
				if (dictionary.ContainsKey(techPointInteractionMod.interactionType))
				{
					continue;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				int moddedTechPointForInteraction = GetModdedTechPointForInteraction(techPointInteractionMod.interactionType, 0);
				if (moddedTechPointForInteraction > 0)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					dictionary.Add(techPointInteractionMod.interactionType, moddedTechPointForInteraction);
				}
			}
			using (Dictionary<TechPointInteractionType, int>.Enumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<TechPointInteractionType, int> current = enumerator.Current;
					if (current.Value > 0)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						numbers.Add(current.Value);
					}
				}
				while (true)
				{
					switch (7)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	public string GetDebugIdentifier(string colorString = "")
	{
		string text = "Mod[ " + m_abilityScopeId + " ] " + m_name + " ";
		if (colorString.Length > 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return "<color=" + colorString + ">" + text + "</color>";
				}
			}
		}
		return text;
	}
}
