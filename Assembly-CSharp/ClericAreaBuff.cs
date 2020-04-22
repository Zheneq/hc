using System.Collections.Generic;
using UnityEngine;

public class ClericAreaBuff : Ability
{
	[Separator("Targeting", true)]
	public AbilityAreaShape m_shape;

	public bool m_penetrateLoS;

	public bool m_includeEnemies;

	public bool m_includeAllies;

	public bool m_includeCaster;

	public Color m_iconColorWhileActive = Color.cyan;

	[Separator("Misc - Energy, Cooldown, Animation", true)]
	public int m_extraTpCostPerTurnActive = 5;

	public int m_cooldownWhenBuffLapses = 2;

	[Separator("On Hit Heal/Damage/Effect", true)]
	public int m_effectDuration = 2;

	public int m_healAmount;

	public StandardEffectInfo m_effectOnCaster;

	public StandardEffectInfo m_effectOnAllies;

	[Header("-- Shielding on self override, if >= 0")]
	public int m_selfShieldingOverride = -1;

	public StandardEffectInfo m_effectOnEnemies;

	[Separator("Vision on Target Square", true)]
	public bool m_addVisionOnTargetSquare;

	public float m_visionRadius = 1.5f;

	public int m_visionDuration = 1;

	public VisionProviderInfo.BrushRevealType m_brushRevealType = VisionProviderInfo.BrushRevealType.Always;

	public bool m_visionAreaIgnoreLos = true;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	public GameObject m_persistentSequencePrefab;

	public GameObject m_pulseSequencePrefab;

	public GameObject m_toggleOffSequencePrefab;

	private AbilityData.ActionType m_buffActionType;

	private Cleric_SyncComponent m_syncComp;

	private AbilityMod_ClericAreaBuff m_abilityMod;

	private StandardEffectInfo m_cachedEffectOnCaster;

	private StandardEffectInfo m_cachedEffectOnAllies;

	private StandardEffectInfo m_cachedFirstTurnEffectOnAllies;

	private StandardEffectInfo m_cachedEffectOnEnemies;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Area Buff";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		AbilityAreaShape shape = GetShape();
		bool penetrateLoS = PenetrateLoS();
		bool affectsEnemies = IncludeEnemies();
		bool affectsAllies = IncludeAllies();
		int affectsCaster;
		if (IncludeCaster())
		{
			affectsCaster = 1;
		}
		else
		{
			affectsCaster = 0;
		}
		base.Targeter = new AbilityUtil_Targeter_ClericAreaBuff(this, shape, penetrateLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, affectsEnemies, affectsAllies, (AbilityUtil_Targeter.AffectsActor)affectsCaster);
		base.Targeter.SetShowArcToShape(false);
		m_syncComp = GetComponent<Cleric_SyncComponent>();
		m_buffActionType = GetActionTypeOfAbility(this);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClericAreaBuff))
		{
			m_abilityMod = (abilityMod as AbilityMod_ClericAreaBuff);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnCaster;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnCaster = m_abilityMod.m_effectOnCasterMod.GetModifiedValue(m_effectOnCaster);
		}
		else
		{
			cachedEffectOnCaster = m_effectOnCaster;
		}
		m_cachedEffectOnCaster = cachedEffectOnCaster;
		m_cachedEffectOnAllies = ((!m_abilityMod) ? m_effectOnAllies : m_abilityMod.m_effectOnAlliesMod.GetModifiedValue(m_effectOnAllies));
		m_cachedFirstTurnEffectOnAllies = ((!m_abilityMod) ? m_effectOnAllies : m_abilityMod.m_firstTurnOnlyEffectOnAlliesMod.GetModifiedValue(m_effectOnAllies));
		StandardEffectInfo cachedEffectOnEnemies;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnEnemies = m_abilityMod.m_effectOnEnemiesMod.GetModifiedValue(m_effectOnEnemies);
		}
		else
		{
			cachedEffectOnEnemies = m_effectOnEnemies;
		}
		m_cachedEffectOnEnemies = cachedEffectOnEnemies;
	}

	public AbilityAreaShape GetShape()
	{
		return (!m_abilityMod) ? m_shape : m_abilityMod.m_shapeMod.GetModifiedValue(m_shape);
	}

	public bool PenetrateLoS()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS);
		}
		else
		{
			result = m_penetrateLoS;
		}
		return result;
	}

	public bool IncludeEnemies()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_includeEnemiesMod.GetModifiedValue(m_includeEnemies);
		}
		else
		{
			result = m_includeEnemies;
		}
		return result;
	}

	public bool IncludeAllies()
	{
		return (!m_abilityMod) ? m_includeAllies : m_abilityMod.m_includeAlliesMod.GetModifiedValue(m_includeAllies);
	}

	public bool IncludeCaster()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_includeCasterMod.GetModifiedValue(m_includeCaster);
		}
		else
		{
			result = m_includeCaster;
		}
		return result;
	}

	public int GetExtraTpCostPerTurnActive()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraTpCostPerTurnActiveMod.GetModifiedValue(m_extraTpCostPerTurnActive);
		}
		else
		{
			result = m_extraTpCostPerTurnActive;
		}
		return result;
	}

	public int GetCooldownWhenBuffLapses()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cooldownWhenBuffLapsesMod.GetModifiedValue(m_cooldownWhenBuffLapses);
		}
		else
		{
			result = m_cooldownWhenBuffLapses;
		}
		return result;
	}

	public int GetEffectDuration()
	{
		return (!m_abilityMod) ? m_effectDuration : m_abilityMod.m_effectDurationMod.GetModifiedValue(m_effectDuration);
	}

	public int GetHealAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healAmountMod.GetModifiedValue(m_healAmount);
		}
		else
		{
			result = m_healAmount;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnCaster()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnCaster != null)
		{
			result = m_cachedEffectOnCaster;
		}
		else
		{
			result = m_effectOnCaster;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnAllies()
	{
		if (m_syncComp != null)
		{
			if (m_syncComp.m_turnsAreaBuffActive == 0)
			{
				if (m_cachedFirstTurnEffectOnAllies != null && m_cachedFirstTurnEffectOnAllies.m_applyEffect)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return m_cachedFirstTurnEffectOnAllies;
						}
					}
				}
			}
		}
		return (m_cachedEffectOnAllies == null) ? m_effectOnAllies : m_cachedEffectOnAllies;
	}

	public int GetSelfShieldingOverride()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfShieldingOverrideMod.GetModifiedValue(m_selfShieldingOverride);
		}
		else
		{
			result = m_selfShieldingOverride;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnEnemies != null)
		{
			result = m_cachedEffectOnEnemies;
		}
		else
		{
			result = m_effectOnEnemies;
		}
		return result;
	}

	public bool AddVisionOnTargetSquare()
	{
		return (!m_abilityMod) ? m_addVisionOnTargetSquare : m_abilityMod.m_addVisionOnTargetSquareMod.GetModifiedValue(m_addVisionOnTargetSquare);
	}

	public float GetVisionRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_visionRadiusMod.GetModifiedValue(m_visionRadius);
		}
		else
		{
			result = m_visionRadius;
		}
		return result;
	}

	public int GetVisionDuration()
	{
		return (!m_abilityMod) ? m_visionDuration : m_abilityMod.m_visionDurationMod.GetModifiedValue(m_visionDuration);
	}

	public bool VisionAreaIgnoreLos()
	{
		return (!m_abilityMod) ? m_visionAreaIgnoreLos : m_abilityMod.m_visionAreaIgnoreLosMod.GetModifiedValue(m_visionAreaIgnoreLos);
	}

	public int GetExtraShieldsPerTurnActive()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraShieldsPerTurnActive.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetAllyTechPointGainPerTurnActive()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_allyTechPointGainPerTurnActive.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraSelfShieldsPerEnemyInShape()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraSelfShieldingPerEnemyInShape.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraHealForPurifyOnBuffedAllies()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraHealForPurifyOnBuffedAllies.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int CalculateShieldAmount(ActorData targetActor)
	{
		int num = GetEffectOnAllies().m_effectData.m_absorbAmount;
		if (targetActor == base.ActorData)
		{
			if (GetSelfShieldingOverride() > 0)
			{
				num = GetSelfShieldingOverride();
			}
			else if (GetEffectOnCaster() != null)
			{
				if (GetEffectOnCaster().m_applyEffect)
				{
					num = GetEffectOnCaster().m_effectData.m_absorbAmount;
				}
			}
			if (GetExtraSelfShieldsPerEnemyInShape() != 0)
			{
				List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(GetShape(), targetActor.GetTravelBoardSquareWorldPosition(), targetActor.GetCurrentBoardSquare(), true, targetActor, targetActor.GetOpposingTeam(), null);
				num += actorsInShape.Count * GetExtraSelfShieldsPerEnemyInShape();
			}
		}
		if (m_syncComp != null)
		{
			if (GetExtraShieldsPerTurnActive() != 0)
			{
				num += GetExtraShieldsPerTurnActive() * m_syncComp.m_turnsAreaBuffActive;
			}
		}
		return num;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "ExtraTpCostPerTurnActive", string.Empty, m_extraTpCostPerTurnActive);
		AddTokenInt(tokens, "CooldownWhenBuffLapses", string.Empty, m_cooldownWhenBuffLapses);
		AddTokenInt(tokens, "EffectDuration", string.Empty, m_effectDuration);
		AddTokenInt(tokens, "HealAmount", string.Empty, m_healAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnCaster, "EffectOnCaster", m_effectOnCaster);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnAllies, "EffectOnAllies", m_effectOnAllies);
		if (m_selfShieldingOverride >= 0)
		{
			AddTokenInt(tokens, "SelfShieldingOverride", string.Empty, m_selfShieldingOverride);
		}
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnEnemies, "EffectOnEnemies", m_effectOnEnemies);
		AddTokenInt(tokens, "VisionDuration", string.Empty, m_visionDuration);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetEffectOnAllies().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		if (IncludeCaster())
		{
			if (m_selfShieldingOverride >= 0)
			{
				AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, m_selfShieldingOverride);
			}
			else
			{
				GetEffectOnAllies().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
			}
		}
		GetEffectOnEnemies().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, GetHealAmount()));
		if (IncludeCaster())
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, GetHealAmount()));
		}
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Ally, 1));
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		dictionary.Add(AbilityTooltipSymbol.Absorb, CalculateShieldAmount(targetActor));
		dictionary.Add(AbilityTooltipSymbol.Energy, (targetActor != base.ActorData) ? GetAllyTechPointGainPerTurnActive() : 0);
		return dictionary;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (m_syncComp != null)
		{
			if (m_syncComp.m_turnsAreaBuffActive == 0)
			{
				return base.GetActionAnimType();
			}
		}
		return ActorModelData.ActionAnimationType.None;
	}

	public override bool ShouldAutoQueueIfValid()
	{
		if (m_syncComp != null)
		{
			if (m_syncComp.m_turnsAreaBuffActive > 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		return base.ShouldAutoQueueIfValid();
	}

	public override bool AllowCancelWhenAutoQueued()
	{
		return true;
	}

	public bool IsActorInBuffShape(ActorData targetActor)
	{
		if (base.ActorData.GetAbilityData().HasQueuedAbilityOfType(typeof(ClericAreaBuff)))
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetShape(), base.ActorData.GetTravelBoardSquareWorldPosition(), base.ActorData.GetCurrentBoardSquare());
			return AreaEffectUtils.GetActorsInShape(GetShape(), centerOfShape, base.ActorData.GetCurrentBoardSquare(), PenetrateLoS(), base.ActorData, base.ActorData.GetTeam(), null).Contains(targetActor);
		}
		return false;
	}

	public override bool UseCustomAbilityIconColor()
	{
		return m_syncComp != null && m_syncComp.m_turnsAreaBuffActive > 0;
	}

	public override Color GetCustomAbilityIconColor(ActorData actor)
	{
		if (m_syncComp != null)
		{
			if (m_syncComp.m_turnsAreaBuffActive > 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return m_iconColorWhileActive;
					}
				}
			}
		}
		return base.GetCustomAbilityIconColor(actor);
	}

	public override bool ShouldShowPersistentAuraUI()
	{
		if (m_syncComp != null)
		{
			if (m_syncComp.m_turnsAreaBuffActive > 0)
			{
				AbilityData abilityData = base.ActorData.GetAbilityData();
				if (abilityData != null)
				{
					if (abilityData.HasQueuedAction(m_buffActionType))
					{
						if (GetPerTurnTechPointCost() <= base.ActorData.TechPoints)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public override int GetModdedCost()
	{
		if (m_syncComp != null && m_syncComp.m_turnsAreaBuffActive == 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return base.GetModdedCost();
				}
			}
		}
		return 0;
	}

	public int GetPerTurnTechPointCost()
	{
		int num = 0;
		if (m_syncComp != null)
		{
			num = m_syncComp.m_turnsAreaBuffActive * m_extraTpCostPerTurnActive;
		}
		return base.GetModdedCost() + num;
	}
}
