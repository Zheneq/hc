﻿// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ExoTetherTrap : Ability
{
	[Header("-- Targeting and Direct Damage")]
	[Space(20f)]
	public int m_laserDamageAmount = 5;
	public LaserTargetingInfo m_laserInfo;
	public StandardActorEffectData m_baseEffectData;
	public StandardEffectInfo m_laserOnHitEffect;
	[Header("-- Tether Info")]
	public float m_tetherDistance = 5f;
	public int m_tetherBreakDamage = 20;
	public StandardEffectInfo m_tetherBreakEffect;
	public bool m_breakTetherOnNonGroundBasedMovement;
	[Header("-- Extra Damage based on distance")]
	public float m_extraDamagePerMoveDist;
	public int m_maxExtraDamageFromMoveDist;
	[Header("-- Cooldown Reduction if tether didn't break")]
	public int m_cdrOnTetherEndIfNotTriggered;
	[Header("-- Sequences")]
	public GameObject m_castSequence;
	public GameObject m_beamSequence;
	public GameObject m_tetherBreakHitSequence;

	private AbilityMod_ExoTetherTrap m_abilityMod;
	private LaserTargetingInfo m_cachedLaserInfo;
	private StandardEffectInfo m_cachedTetherBreakEffect;
	private StandardActorEffectData m_cachedBaseEffectData;
	private StandardEffectInfo m_cachedLaserOnHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Exo Tether Trap";
		}
		SetupTargeter();
	}

	public void SetupTargeter()
	{
		SetCachedFields();
		AbilityUtil_Targeter_ExoTether targeter = new AbilityUtil_Targeter_ExoTether(
			this, GetLaserInfo(), GetLaserInfo());
		targeter.SetAffectedGroups(true, false, false);
		Targeter = targeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_laserInfo;
		m_cachedLaserInfo = m_abilityMod != null
			? m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo)
			: m_laserInfo;
		m_cachedTetherBreakEffect = m_abilityMod != null
			? m_abilityMod.m_tetherBreakEffectMod.GetModifiedValue(m_tetherBreakEffect)
			: m_tetherBreakEffect;
		m_cachedBaseEffectData = m_abilityMod != null
			? m_abilityMod.m_baseEffectDataMod.GetModifiedValue(m_baseEffectData)
			: m_baseEffectData.GetShallowCopy();
		if (m_beamSequence != null)
		{
			m_cachedBaseEffectData.m_sequencePrefabs = new[] { m_beamSequence };
		}
		m_cachedLaserOnHitEffect = m_abilityMod != null
			? m_abilityMod.m_laserOnHitEffectMod.GetModifiedValue(m_laserOnHitEffect)
			: m_laserOnHitEffect;
	}

	public int GetLaserDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public StandardActorEffectData GetBaseEffectData()
	{
		return m_cachedBaseEffectData ?? m_baseEffectData;
	}

	public StandardEffectInfo GetLaserOnHitEffect()
	{
		return m_cachedLaserOnHitEffect ?? m_laserOnHitEffect;
	}

	public float GetTetherDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_tetherDistanceMod.GetModifiedValue(m_tetherDistance)
			: m_tetherDistance;
	}

	public int GetTetherBreakDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_tetherBreakDamageMod.GetModifiedValue(m_tetherBreakDamage)
			: m_tetherBreakDamage;
	}

	public StandardEffectInfo GetTetherBreakEffect()
	{
		return m_cachedTetherBreakEffect ?? m_tetherBreakEffect;
	}

	public bool BreakTetherOnNonGroundBasedMovement()
	{
		return m_abilityMod != null
			? m_abilityMod.m_breakTetherOnNonGroundBasedMovementMod.GetModifiedValue(m_breakTetherOnNonGroundBasedMovement)
			: m_breakTetherOnNonGroundBasedMovement;
	}

	public float GetExtraDamagePerMoveDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamagePerMoveDistMod.GetModifiedValue(m_extraDamagePerMoveDist)
			: m_extraDamagePerMoveDist;
	}

	public int GetMaxExtraDamageFromMoveDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxExtraDamageFromMoveDistMod.GetModifiedValue(m_maxExtraDamageFromMoveDist)
			: m_maxExtraDamageFromMoveDist;
	}

	public int GetCdrOnTetherEndIfNotTriggered()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrOnTetherEndIfNotTriggeredMod.GetModifiedValue(m_cdrOnTetherEndIfNotTriggered)
			: m_cdrOnTetherEndIfNotTriggered;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ExoTetherTrap abilityMod_ExoTetherTrap = modAsBase as AbilityMod_ExoTetherTrap;
		StandardActorEffectData effectData = abilityMod_ExoTetherTrap != null
			? abilityMod_ExoTetherTrap.m_baseEffectDataMod.GetModifiedValue(m_baseEffectData)
			: m_baseEffectData;
		effectData.AddTooltipTokens(tokens, "TetherBaseEffectData", abilityMod_ExoTetherTrap != null, m_baseEffectData);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ExoTetherTrap != null
			? abilityMod_ExoTetherTrap.m_laserOnHitEffectMod.GetModifiedValue(m_laserOnHitEffect)
			: m_laserOnHitEffect, "LaserOnHitEffect", m_laserOnHitEffect);
		AddTokenInt(tokens, "Damage_FirstTurn", string.Empty, abilityMod_ExoTetherTrap != null
			? abilityMod_ExoTetherTrap.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount);
		AddTokenInt(tokens, "Damage_TetherBreak", string.Empty, abilityMod_ExoTetherTrap != null
			? abilityMod_ExoTetherTrap.m_tetherBreakDamageMod.GetModifiedValue(m_tetherBreakDamage)
			: m_tetherBreakDamage);
		AddTokenInt(tokens, "TetherDistance", "distance from starting position", (int)(abilityMod_ExoTetherTrap != null
			? abilityMod_ExoTetherTrap.m_tetherDistanceMod.GetModifiedValue(m_tetherDistance)
			: m_tetherDistance));
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ExoTetherTrap != null
			? abilityMod_ExoTetherTrap.m_tetherBreakEffectMod.GetModifiedValue(m_tetherBreakEffect)
			: m_tetherBreakEffect, "TetherBreakEffect", m_tetherBreakEffect);
		AddTokenInt(tokens, "CdrOnTetherEndIfNotTriggered", string.Empty, abilityMod_ExoTetherTrap != null
			? abilityMod_ExoTetherTrap.m_cdrOnTetherEndIfNotTriggeredMod.GetModifiedValue(m_cdrOnTetherEndIfNotTriggered)
			: m_cdrOnTetherEndIfNotTriggered);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetLaserDamageAmount());
		GetBaseEffectData().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, GetLaserDamageAmount());
		return symbolToValue;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ExoTetherTrap))
		{
			m_abilityMod = abilityMod as AbilityMod_ExoTetherTrap;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// added in rogues
	public ExoTetherEffect CreateDamageTetherEffect(ActorData caster, ActorData hitActor)
	{
		return new ExoTetherEffect(
			AsEffectSource(),
			hitActor.GetCurrentBoardSquare(),
			hitActor,
			caster,
			GetBaseEffectData(),
			GetTetherDistance(),
			GetTetherBreakDamage(),
			GetExtraDamagePerMoveDist(),
			GetMaxExtraDamageFromMoveDist(),
			GetTetherBreakEffect(),
			BreakTetherOnNonGroundBasedMovement(),
			m_tetherBreakHitSequence);
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ActorData> hitActors = GetHitActors(targets, caster, out VectorUtils.LaserCoords laserCoords, null);
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequence,
				laserCoords.end,
				hitActors.ToArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		foreach (ActorData actorData in GetHitActors(targets, caster, out _, nonActorTargetInfo))
		{
			ActorHitParameters hitParams = new ActorHitParameters(actorData, caster.GetFreePos());
			ActorHitResults actorHitResults = new ActorHitResults(GetLaserDamageAmount(), HitActionType.Damage, hitParams);
			SetExistingEffectsForRemoval(caster, actorHitResults);
			ExoTetherEffect effect = CreateDamageTetherEffect(caster, actorData);
			actorHitResults.AddEffect(effect);
			actorHitResults.AddStandardEffectInfo(GetLaserOnHitEffect());
			abilityResults.StoreActorHit(actorHitResults);
			caster.GetComponent<SparkBeamTrackerComponent>().SetTetherRadiusPosition(actorData.GetFreePos());
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	protected virtual List<ActorData> GetHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out VectorUtils.LaserCoords endPoints,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, GetLaserInfo().affectsAllies, GetLaserInfo().affectsEnemies);
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		LaserTargetingInfo laserInfo = GetLaserInfo();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targets[0].AimDirection,
			laserInfo.range,
			laserInfo.width,
			caster,
			relevantTeams,
			laserInfo.penetrateLos,
			laserInfo.maxTargets,
			false,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		endPoints = laserCoords;
		return actorsInLaser;
	}

	// added in rogues
	public void SetExistingEffectsForRemoval(ActorData caster, ActorHitResults hitResult)
	{
		foreach (int actorIndex in caster.GetComponent<SparkBeamTrackerComponent>().GetBeamActorIndices())
		{
			ActorData actorOfActorIndex = GameplayUtils.GetActorOfActorIndex(actorIndex);
			if (actorOfActorIndex != null)
			{
				List<Effect> effectsOnTargetByCaster = ServerEffectManager.Get().GetEffectsOnTargetByCaster(actorOfActorIndex, caster, typeof(ExoTetherEffect));
				if (effectsOnTargetByCaster.Count > 0)
				{
					foreach (Effect effect in effectsOnTargetByCaster)
					{
						ExoTetherEffect exoTetherEffect = effect as ExoTetherEffect;
						exoTetherEffect.SetAbilityQueued(true);
						hitResult.AddEffectForRemoval(exoTetherEffect, ServerEffectManager.Get().GetActorEffects(actorOfActorIndex));
					}
				}
			}
		}
	}

	// added in rogues
	public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.ExoStats.TetherTrapTriggers);
		}
	}
#endif
}
