using System.Collections.Generic;
using UnityEngine;

public class ClericAreaBuff : Ability
{
	[Separator("Targeting")]
	public AbilityAreaShape m_shape;
	public bool m_penetrateLoS;
	public bool m_includeEnemies;
	public bool m_includeAllies;
	public bool m_includeCaster;
	public Color m_iconColorWhileActive = Color.cyan;
	[Separator("Misc - Energy, Cooldown, Animation")]
	public int m_extraTpCostPerTurnActive = 5;
	public int m_cooldownWhenBuffLapses = 2;
	[Separator("On Hit Heal/Damage/Effect")]
	public int m_effectDuration = 2;
	public int m_healAmount;
	public StandardEffectInfo m_effectOnCaster;
	public StandardEffectInfo m_effectOnAllies;
	[Header("-- Shielding on self override, if >= 0")]
	public int m_selfShieldingOverride = -1;
	public StandardEffectInfo m_effectOnEnemies;
	[Separator("Vision on Target Square")]
	public bool m_addVisionOnTargetSquare; // TODO CLERIC unused (not used in the ability or any of the mods)
	public float m_visionRadius = 1.5f; // TODO CLERIC unused (not used in the ability or any of the mods)
	public int m_visionDuration = 1; // TODO CLERIC unused (not used in the ability or any of the mods)
	public VisionProviderInfo.BrushRevealType m_brushRevealType = VisionProviderInfo.BrushRevealType.Always; // TODO CLERIC unused (not used in the ability or any of the mods)
	public bool m_visionAreaIgnoreLos = true; // TODO CLERIC unused (not used in the ability or any of the mods)
	[Separator("Sequences")]
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
		Targeter = new AbilityUtil_Targeter_ClericAreaBuff(
			this,
			GetShape(),
			PenetrateLoS(),
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			IncludeEnemies(),
			IncludeAllies(),
			IncludeCaster()
				? AbilityUtil_Targeter.AffectsActor.Possible
				: AbilityUtil_Targeter.AffectsActor.Never);
		Targeter.SetShowArcToShape(false);
		m_syncComp = GetComponent<Cleric_SyncComponent>();
		m_buffActionType = GetActionTypeOfAbility(this);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClericAreaBuff))
		{
			m_abilityMod = abilityMod as AbilityMod_ClericAreaBuff;
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
		m_cachedEffectOnCaster = m_abilityMod != null
			? m_abilityMod.m_effectOnCasterMod.GetModifiedValue(m_effectOnCaster)
			: m_effectOnCaster;
		m_cachedEffectOnAllies = m_abilityMod != null
			? m_abilityMod.m_effectOnAlliesMod.GetModifiedValue(m_effectOnAllies)
			: m_effectOnAllies;
		m_cachedFirstTurnEffectOnAllies = m_abilityMod != null
			? m_abilityMod.m_firstTurnOnlyEffectOnAlliesMod.GetModifiedValue(m_effectOnAllies)
			: m_effectOnAllies;
		m_cachedEffectOnEnemies = m_abilityMod != null
			? m_abilityMod.m_effectOnEnemiesMod.GetModifiedValue(m_effectOnEnemies)
			: m_effectOnEnemies;
	}

	public AbilityAreaShape GetShape()
	{
		return m_abilityMod  != null
			? m_abilityMod.m_shapeMod.GetModifiedValue(m_shape)
			: m_shape;
	}

	public bool PenetrateLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS)
			: m_penetrateLoS;
	}

	public bool IncludeEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_includeEnemiesMod.GetModifiedValue(m_includeEnemies)
			: m_includeEnemies;
	}

	public bool IncludeAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_includeAlliesMod.GetModifiedValue(m_includeAllies)
			: m_includeAllies;
	}

	public bool IncludeCaster()
	{
		return m_abilityMod != null
			? m_abilityMod.m_includeCasterMod.GetModifiedValue(m_includeCaster)
			: m_includeCaster;
	}

	public int GetExtraTpCostPerTurnActive()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraTpCostPerTurnActiveMod.GetModifiedValue(m_extraTpCostPerTurnActive)
			: m_extraTpCostPerTurnActive;
	}

	public int GetCooldownWhenBuffLapses()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownWhenBuffLapsesMod.GetModifiedValue(m_cooldownWhenBuffLapses)
			: m_cooldownWhenBuffLapses;
	}

	public int GetEffectDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_effectDurationMod.GetModifiedValue(m_effectDuration)
			: m_effectDuration;
	}

	public int GetHealAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healAmountMod.GetModifiedValue(m_healAmount)
			: m_healAmount;
	}

	public StandardEffectInfo GetEffectOnCaster()
	{
		return m_cachedEffectOnCaster ?? m_effectOnCaster;
	}

	public StandardEffectInfo GetEffectOnAllies()
	{
		return m_syncComp != null
		       && m_syncComp.m_turnsAreaBuffActive == 0
		       && m_cachedFirstTurnEffectOnAllies != null
		       && m_cachedFirstTurnEffectOnAllies.m_applyEffect
			? m_cachedFirstTurnEffectOnAllies
			: m_cachedEffectOnAllies ?? m_effectOnAllies;
	}

	public int GetSelfShieldingOverride()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfShieldingOverrideMod.GetModifiedValue(m_selfShieldingOverride)
			: m_selfShieldingOverride;
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		return m_cachedEffectOnEnemies ?? m_effectOnEnemies;
	}

	// TODO CLERIC unused (not used in the ability or any of the mods), always false
	public bool AddVisionOnTargetSquare()
	{
		return m_abilityMod != null
			? m_abilityMod.m_addVisionOnTargetSquareMod.GetModifiedValue(m_addVisionOnTargetSquare)
			: m_addVisionOnTargetSquare;
	}

	// TODO CLERIC unused (not used in the ability or any of the mods), always 4
	public float GetVisionRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_visionRadiusMod.GetModifiedValue(m_visionRadius)
			: m_visionRadius;
	}

	// TODO CLERIC unused (not used in the ability or any of the mods), always 1
	public int GetVisionDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_visionDurationMod.GetModifiedValue(m_visionDuration)
			: m_visionDuration;
	}

	// TODO CLERIC unused (not used in the ability or any of the mods), always true
	public bool VisionAreaIgnoreLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_visionAreaIgnoreLosMod.GetModifiedValue(m_visionAreaIgnoreLos)
			: m_visionAreaIgnoreLos;
	}

	public int GetExtraShieldsPerTurnActive()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraShieldsPerTurnActive.GetModifiedValue(0)
			: 0;
	}

	public int GetAllyTechPointGainPerTurnActive()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyTechPointGainPerTurnActive.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraSelfShieldsPerEnemyInShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraSelfShieldingPerEnemyInShape.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraHealForPurifyOnBuffedAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealForPurifyOnBuffedAllies.GetModifiedValue(0)
			: 0;
	}

	public int CalculateShieldAmount(ActorData targetActor)
	{
		int absorb = GetEffectOnAllies().m_effectData.m_absorbAmount;
		if (targetActor == ActorData)
		{
			if (GetSelfShieldingOverride() > 0)
			{
				absorb = GetSelfShieldingOverride();
			}
			else if (GetEffectOnCaster() != null)
			{
				if (GetEffectOnCaster().m_applyEffect)
				{
					absorb = GetEffectOnCaster().m_effectData.m_absorbAmount;
				}
			}
			if (GetExtraSelfShieldsPerEnemyInShape() != 0)
			{
				List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
					GetShape(),
					targetActor.GetFreePos(),
					targetActor.GetCurrentBoardSquare(),
					true,
					targetActor,
					targetActor.GetEnemyTeam(),
					null);
				absorb += actorsInShape.Count * GetExtraSelfShieldsPerEnemyInShape();
			}
		}
		if (m_syncComp != null && GetExtraShieldsPerTurnActive() != 0)
		{
			absorb += GetExtraShieldsPerTurnActive() * m_syncComp.m_turnsAreaBuffActive;
		}
		return absorb;
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
		return new Dictionary<AbilityTooltipSymbol, int>
		{
			{ AbilityTooltipSymbol.Absorb, CalculateShieldAmount(targetActor) },
			{ AbilityTooltipSymbol.Energy, targetActor != ActorData ? GetAllyTechPointGainPerTurnActive() : 0 }
		};
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		return m_syncComp != null && m_syncComp.m_turnsAreaBuffActive == 0
			? base.GetActionAnimType()
			: ActorModelData.ActionAnimationType.None;
	}

	public override bool ShouldAutoQueueIfValid()
	{
		return m_syncComp != null && m_syncComp.m_turnsAreaBuffActive > 0
		       || base.ShouldAutoQueueIfValid();
	}

	public override bool AllowCancelWhenAutoQueued()
	{
		return true;
	}

	public bool IsActorInBuffShape(ActorData targetActor)
	{
		if (ActorData.GetAbilityData().HasQueuedAbilityOfType(typeof(ClericAreaBuff)))
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetShape(), ActorData.GetFreePos(), ActorData.GetCurrentBoardSquare());
			return AreaEffectUtils.GetActorsInShape(
					GetShape(),
					centerOfShape,
					ActorData.GetCurrentBoardSquare(),
					PenetrateLoS(),
					ActorData,
					ActorData.GetTeam(),
					null)
				.Contains(targetActor);
		}
		return false;
	}

	public override bool UseCustomAbilityIconColor()
	{
		return m_syncComp != null && m_syncComp.m_turnsAreaBuffActive > 0;
	}

	public override Color GetCustomAbilityIconColor(ActorData actor)
	{
		return m_syncComp != null && m_syncComp.m_turnsAreaBuffActive > 0
			? m_iconColorWhileActive
			: base.GetCustomAbilityIconColor(actor);
	}

	public override bool ShouldShowPersistentAuraUI()
	{
		if (m_syncComp != null && m_syncComp.m_turnsAreaBuffActive > 0)
		{
			AbilityData abilityData = ActorData.GetAbilityData();
			return abilityData != null
			       && abilityData.HasQueuedAction(m_buffActionType)
			       && GetPerTurnTechPointCost() <= ActorData.TechPoints;
		}
		return false;
	}

	public override int GetModdedCost()
	{
		return m_syncComp != null && m_syncComp.m_turnsAreaBuffActive == 0
			? base.GetModdedCost()
			: 0;
	}

	public int GetPerTurnTechPointCost()
	{
		int cost = 0;
		if (m_syncComp != null)
		{
			// custom
			cost = m_syncComp.m_turnsAreaBuffActive * GetExtraTpCostPerTurnActive();
			// reactor
			// cost = m_syncComp.m_turnsAreaBuffActive * m_extraTpCostPerTurnActive;
		}
		return base.GetModdedCost() + cost;
	}
	
#if SERVER
	// added in rogues
	// It seems too early -- at the very least, if turnsAreaBuffActive replicates before cast animation, it breaks it
	// Also it looks like effect tech point cost is supposed to be calculated before this increment
	// So I moved it into ClericAreaBuffEffect::OnTurnEnd
	// public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	// {
	// 	m_syncComp.Networkm_turnsAreaBuffActive++;  // m_turnsAreaBuffActive in rogues
	// 	Log.Info($"ClericAreaBuff run: {m_syncComp.Networkm_turnsAreaBuffActive}");
	// }

	// custom
	public override bool CustomCanCastValidation(ActorData caster)
	{
		return GetPerTurnTechPointCost() <= caster.TechPoints;
	}

	// custom
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		SequenceSource sequenceSource = additionalData.m_sequenceSource;
		List<Effect> activeEffects = ServerEffectManager.Get()
			.GetEffectsOnTargetByCaster(caster, caster, typeof(ClericAreaBuffEffect));
		if (activeEffects.Count > 0)
		{
			sequenceSource = activeEffects[0].SequenceSource;
		}

		return new ServerClientUtils.SequenceStartData(
			m_syncComp.m_turnsAreaBuffActive <= 0 ? m_castSequencePrefab : null,
			Board.Get().GetSquare(caster.GetGridPos()).ToVector3(),
			additionalData.m_abilityResults.HitActorsArray(),  // aka null
			caster,
			sequenceSource);
	}
	
	// custom
	public override void GatherAbilityResults(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		if (m_syncComp.m_turnsAreaBuffActive > 0)
		{
			Log.Info($"ClericAreaBuff already active, skipping ability");
			return;
		}
		
		BoardSquare casterSquare = Board.Get().GetSquare(caster.GetGridPos());
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(casterSquare.ToVector3()));
		List<ActorData> hitActors = AreaEffectUtils.GetActorsInShape(
			GetShape(),
			casterSquare.ToVector3(),
			casterSquare,
			PenetrateLoS(),
			caster,
			GetAffectedTeams(caster),
			null);
		positionHitResults.AddEffect(new ClericAreaBuffEffect(
			AsEffectSource(),
			casterSquare,
			caster,
			caster,
			this,
			m_syncComp,
			hitActors,
			GetEffectDuration()));
		abilityResults.StorePositionHit(positionHitResults);
	}

	// custom
	public List<Team> GetAffectedTeams(ActorData caster)
	{
		List<Team> list = new List<Team>();
		if (IncludeAllies())
		{
			list.Add(caster.GetTeam());
		}
		if (IncludeEnemies())
		{
			list.AddRange(caster.GetOtherTeams());
		}
		return list;
	}
#endif
}
