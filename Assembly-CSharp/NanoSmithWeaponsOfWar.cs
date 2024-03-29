﻿// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class NanoSmithWeaponsOfWar : Ability
{
	[Header("-- Targeting in Ability")]
	public bool m_canTargetEnemies = true;
	public bool m_canTargetAllies = true;
	[Header("-- Effect on Ability Target")]
	public StandardEffectInfo m_targetAllyOnHitEffect;
	public StandardEffectInfo m_targetEnemyOnHitEffect;
	[Header("-- Sweep Info")]
	public int m_sweepDamageAmount = 10;
	public int m_sweepDuration = 3;
	public int m_sweepDamageDelay;
	[Header("-- Sweep On Hit Effects")]
	public StandardEffectInfo m_enemySweepOnHitEffect;
	public StandardEffectInfo m_allySweepOnHitEffect;
	[Header("-- Sweep Targeting")]
	public AbilityAreaShape m_sweepShape = AbilityAreaShape.Three_x_Three;
	public bool m_sweepIncludeEnemies = true;
	public bool m_sweepIncludeAllies;
	public bool m_sweepPenetrateLineOfSight;
	public bool m_sweepIncludeTarget;
	[Header("-- Sequences -----------------------------")]
	public GameObject m_castSequencePrefab;
	public GameObject m_persistentSequencePrefab;
	public GameObject m_rangeIndicatorSequencePrefab;
	public GameObject m_bladeSequencePrefab;
	public GameObject m_shieldPerTurnSequencePrefab;

	private AbilityMod_NanoSmithWeaponsOfWar m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Weapons of War";
		}
		AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(
			this,
			m_sweepShape,
			m_sweepPenetrateLineOfSight,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			true,
			false,
			AbilityUtil_Targeter.AffectsActor.Never,
			AbilityUtil_Targeter.AffectsActor.Always);
		abilityUtil_Targeter_Shape.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Secondary);
		Targeter = abilityUtil_Targeter_Shape;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetAllyTargetEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetSweepDamage());
		return numbers;
	}

	public override bool CustomTargetValidation(
		ActorData caster,
		AbilityTarget target,
		int targetIndex,
		List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(
			caster,
			currentBestActorTarget,
			m_canTargetEnemies,
			m_canTargetAllies,
			m_canTargetAllies,
			ValidateCheckPath.Ignore,
			true,
			true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithWeaponsOfWar abilityMod_NanoSmithWeaponsOfWar = modAsBase as AbilityMod_NanoSmithWeaponsOfWar;
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_NanoSmithWeaponsOfWar != null
			? abilityMod_NanoSmithWeaponsOfWar.m_allyTargetEffectOverride.GetModifiedValue(m_targetAllyOnHitEffect)
			: m_targetAllyOnHitEffect, "TargetAllyOnHitEffect", m_targetAllyOnHitEffect);
		AddTokenInt(tokens, "SweepDamageAmount", string.Empty, abilityMod_NanoSmithWeaponsOfWar != null
			? abilityMod_NanoSmithWeaponsOfWar.m_sweepDamageMod.GetModifiedValue(m_sweepDamageAmount)
			: m_sweepDamageAmount);
		AddTokenInt(tokens, "SweepDuration", string.Empty, abilityMod_NanoSmithWeaponsOfWar != null
			? abilityMod_NanoSmithWeaponsOfWar.m_sweepDurationMod.GetModifiedValue(m_sweepDuration)
			: m_sweepDuration);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_NanoSmithWeaponsOfWar != null
			? abilityMod_NanoSmithWeaponsOfWar.m_enemySweepOnHitEffectOverride.GetModifiedValue(m_enemySweepOnHitEffect)
			: m_enemySweepOnHitEffect, "EnemySweepOnHitEffect", m_enemySweepOnHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_NanoSmithWeaponsOfWar != null
			? abilityMod_NanoSmithWeaponsOfWar.m_allySweepOnHitEffectOverride.GetModifiedValue(m_allySweepOnHitEffect)
			: m_allySweepOnHitEffect, "AllySweepOnHitEffect", m_allySweepOnHitEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NanoSmithWeaponsOfWar))
		{
			m_abilityMod = abilityMod as AbilityMod_NanoSmithWeaponsOfWar;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
	}

	private int GetSweepDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_sweepDurationMod.GetModifiedValue(m_sweepDuration)
			: m_sweepDuration;
	}

	private int GetSweepDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_sweepDamageMod.GetModifiedValue(m_sweepDamageAmount)
			: m_sweepDamageAmount;
	}

	private int GetShieldGainPerTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldGainPerTurnMod.GetModifiedValue(0)
			: 0;
	}

	private StandardEffectInfo GetAllyTargetEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyTargetEffectOverride.GetModifiedValue(m_targetAllyOnHitEffect)
			: m_targetAllyOnHitEffect;
	}

	private StandardEffectInfo GetAllySweepEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allySweepOnHitEffectOverride.GetModifiedValue(m_allySweepOnHitEffect)
			: m_allySweepOnHitEffect;
	}

	private StandardEffectInfo GetEnemySweepEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemySweepOnHitEffectOverride.GetModifiedValue(m_enemySweepOnHitEffect)
			: m_enemySweepOnHitEffect;
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		ActorData currentBestActorTarget = targets[0].GetCurrentBestActorTarget();
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab, square, currentBestActorTarget.AsArray(), caster, additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		ActorData hitActor = GetHitActor(targets, caster);
		if (hitActor == null)
		{
			return;
		}
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, hitActor.GetFreePos()));
		bool flag = hitActor.GetTeam() == caster.GetTeam();
		if ((flag && m_canTargetAllies) || (!flag && m_canTargetEnemies))
		{
			actorHitResults.AddStandardEffectInfo(flag ? GetAllyTargetEffect() : m_targetEnemyOnHitEffect);
			bool includeAllies = m_sweepIncludeAllies || (GetAllySweepEffect() != null && GetAllySweepEffect().m_applyEffect);
			NanoSmithWeaponsOfWarEffect effect = new NanoSmithWeaponsOfWarEffect(
				AsEffectSource(),
				square,
				hitActor,
				caster,
				GetSweepDuration(),
				GetSweepDamage(),
				m_sweepShape,
				m_sweepPenetrateLineOfSight,
				m_sweepIncludeTarget,
				m_sweepIncludeEnemies,
				includeAllies,
				m_sweepDamageDelay,
				GetEnemySweepEffect(),
				GetAllySweepEffect(),
				m_persistentSequencePrefab,
				m_rangeIndicatorSequencePrefab,
				m_bladeSequencePrefab);
			actorHitResults.AddEffect(effect);
			if (flag && GetShieldGainPerTurn() > 0)
			{
				ShieldOverTimeEffect shieldEffect = new ShieldOverTimeEffect(
					AsEffectSource(),
					square,
					hitActor,
					caster,
					AbilityPriority.Prep_Defense,
					GetSweepDuration(),
					GetShieldGainPerTurn(),
					1,
					m_shieldPerTurnSequencePrefab);
				actorHitResults.AddEffect(shieldEffect);
			}
		}
		abilityResults.StoreActorHit(actorHitResults);
	}

	// added in rogues
	private ActorData GetHitActor(List<AbilityTarget> targets, ActorData caster)
	{
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		if (square != null
		    && square.OccupantActor != null
		    && !square.OccupantActor.IgnoreForAbilityHits)
		{
			return square.OccupantActor;
		}
		return null;
	}
#endif
}
