// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class NanoSmithVacuumBomb : Ability
{
	public enum KnockbackCenterType
	{
		FromTargetSquare,
		FromTargetActor
	}

	[Header("-- Bomb Hit")]
	public int m_bombDamageAmount;
	public StandardEffectInfo m_enemyHitEffect;
	public AbilityAreaShape m_bombShape = AbilityAreaShape.Three_x_Three;
	public bool m_bombPenetrateLineOfSight;
	[Header("-- Effects")]
	public StandardEffectInfo m_onCenterActorEffect;
	[Header("-- Knockback")]
	public KnockbackCenterType m_knockbackCenterType;
	public int m_knockbackDelay;
	public KnockbackType m_knockbackType = KnockbackType.PullToSource;
	public float m_knockbackDistance = 2f;
	[Header("-- Only relevant for PullToSource, if checked, pull adjacent actors over to opposite side")]
	public bool m_knockbackAdjacentActorsIfPull = true;
	[Header("-- Sequences -----------------------------------")]
	public GameObject m_castSequencePrefab;
	public GameObject m_delayedKnockbackMarkerSequencePrefab;
	public GameObject m_delayedKnockbackHitSequencePrefab;

	private AbilityMod_NanoSmithVacuumBomb m_abilityMod;
	private NanoSmith_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedOnCenterActorEffect;
	private StandardEffectInfo m_cachedEnemyHitEffect;

	// added in rogues
#if SERVER
	private AbilityData.ActionType m_myActionType = AbilityData.ActionType.INVALID_ACTION;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Vacuum Bomb";
		}
		// added in rogues
#if SERVER
		m_myActionType = GetComponent<AbilityData>().GetActionTypeOfAbility(this);
#endif
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_syncComp = GetComponent<NanoSmith_SyncComponent>();
		SetCachedFields();
		float knockbackDistance = m_knockbackDelay > 0 ? 0f : m_knockbackDistance;
		AbilityUtil_Targeter.AffectsActor affectsTargetOnGridposSquare = AbilityUtil_Targeter.AffectsActor.Never;
		if (GetModdedEffectForAllies() != null && GetModdedEffectForAllies().m_applyEffect || GetCenterActorEffect().m_applyEffect)
		{
			affectsTargetOnGridposSquare = AbilityUtil_Targeter.AffectsActor.Always;
		}
		Targeter = new AbilityUtil_Targeter_KnockbackRingAoE(
			this,
			m_bombShape,
			m_bombPenetrateLineOfSight,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			true,
			false,
			AbilityUtil_Targeter.AffectsActor.Never,
			affectsTargetOnGridposSquare,
			m_bombShape,
			knockbackDistance,
			m_knockbackType,
			m_knockbackAdjacentActorsIfPull,
			true);
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AppendTooltipNumbersFromBaseModEffects(ref numbers, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageAmount());
		StandardEffectInfo centerActorEffect = GetCenterActorEffect();
		if (centerActorEffect.m_applyEffect)
		{
			centerActorEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
			centerActorEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (m_syncComp != null
		    && m_syncComp.m_extraAbsorbOnVacuumBomb > 0
		    && Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) == 0)
		{
			StandardEffectInfo centerActorEffect = GetCenterActorEffect();
			results.m_absorb = centerActorEffect.m_effectData.m_absorbAmount + m_syncComp.m_extraAbsorbOnVacuumBomb;
			return true;
		}
		return false;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return CanTargetActorInDecision(
			caster,
			target.GetCurrentBestActorTarget(),
			false,
			true, 
			true,
			ValidateCheckPath.Ignore,
			true,
			true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithVacuumBomb abilityMod_NanoSmithVacuumBomb = modAsBase as AbilityMod_NanoSmithVacuumBomb;
		AddTokenInt(tokens, "BombDamageAmount", string.Empty, abilityMod_NanoSmithVacuumBomb != null
			? abilityMod_NanoSmithVacuumBomb.m_damageMod.GetModifiedValue(m_bombDamageAmount)
			: m_bombDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_NanoSmithVacuumBomb != null
			? abilityMod_NanoSmithVacuumBomb.m_enemyHitEffectOverride.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_NanoSmithVacuumBomb != null
			? abilityMod_NanoSmithVacuumBomb.m_onCenterActorEffectOverride.GetModifiedValue(m_onCenterActorEffect)
			: m_onCenterActorEffect, "OnCenterActorEffect", m_onCenterActorEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NanoSmithVacuumBomb))
		{
			m_abilityMod = abilityMod as AbilityMod_NanoSmithVacuumBomb;
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
		m_cachedOnCenterActorEffect = m_abilityMod != null
			? m_abilityMod.m_onCenterActorEffectOverride.GetModifiedValue(m_onCenterActorEffect)
			: m_onCenterActorEffect;
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectOverride.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
	}

	private int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_bombDamageAmount)
			: m_bombDamageAmount;
	}

	private int GetCooldownChangePerHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownChangePerHitMod.GetModifiedValue(0)
			: 0;
	}

	private StandardEffectInfo GetCenterActorEffect()
	{
		return m_cachedOnCenterActorEffect ?? m_onCenterActorEffect;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public int GetExtraAbsorb()
	{
		return m_syncComp != null
			? m_syncComp.m_extraAbsorbOnVacuumBomb
			: 0;
	}

#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_syncComp != null)
		{
			m_syncComp.Networkm_extraAbsorbOnVacuumBomb = 0;
		}
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			targets[0].FreePos,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorData knockbackCenterActor = GetKnockbackCenterActor(targets, caster);
		if (knockbackCenterActor == null)
		{
			return;
		}
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(knockbackCenterActor, knockbackCenterActor.GetFreePos()));
		StandardEffectInfo centerActorEffect = GetCenterActorEffect();
		if (m_syncComp != null && m_syncComp.m_extraAbsorbOnVacuumBomb > 0)
		{
			centerActorEffect = centerActorEffect.GetShallowCopy();
			centerActorEffect.m_effectData.m_absorbAmount += m_syncComp.m_extraAbsorbOnVacuumBomb;
		}
		actorHitResults.AddStandardEffectInfo(centerActorEffect);
		if (m_knockbackDelay > 0)
		{
			DelayedAoeKnockbackEffect.KnockbackCenterType knockbackCenterType =
				m_knockbackCenterType == KnockbackCenterType.FromTargetActor
					? DelayedAoeKnockbackEffect.KnockbackCenterType.FromTargetActor
					: DelayedAoeKnockbackEffect.KnockbackCenterType.FromTargetSquare;
			DelayedAoeKnockbackEffect delayedAoeKnockbackEffect = new DelayedAoeKnockbackEffect(
				AsEffectSource(),
				Board.Get().GetSquare(targets[0].GridPos),
				knockbackCenterActor,
				caster,
				knockbackCenterType,
				m_knockbackDelay,
				GetDamageAmount(),
				GetEnemyHitEffect(),
				m_bombShape,
				m_bombPenetrateLineOfSight,
				m_knockbackType,
				m_knockbackDistance,
				m_knockbackAdjacentActorsIfPull,
				m_delayedKnockbackMarkerSequencePrefab,
				m_delayedKnockbackHitSequencePrefab);
			delayedAoeKnockbackEffect.SetCooldownOnHitConfig(m_myActionType, GetCooldownChangePerHit());
			actorHitResults.AddEffect(delayedAoeKnockbackEffect);
		}
		abilityResults.StoreActorHit(actorHitResults);
		if (m_knockbackDelay <= 0)
		{
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			List<ActorData> bombHitActors = GetBombHitActors(targets, caster, nonActorTargetInfo);
			bool isCooldownReductionApplied = false;
			foreach (ActorData actorData in bombHitActors)
			{
				ActorHitResults bombHitResults = new ActorHitResults(new ActorHitParameters(actorData, targets[0].FreePos));
				bombHitResults.SetBaseDamage(GetDamageAmount());
				bombHitResults.AddStandardEffectInfo(GetEnemyHitEffect());
				Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_bombShape, targets[0]);
				if (m_knockbackCenterType == KnockbackCenterType.FromTargetActor)
				{
					BoardSquare currentBoardSquare = caster.GetCurrentBoardSquare();
					centerOfShape = AreaEffectUtils.GetCenterOfShape(m_bombShape, currentBoardSquare.ToVector3(), currentBoardSquare);
				}
				KnockbackHitData knockbackData;
				if (m_knockbackType == KnockbackType.PullToSource
				    && m_knockbackAdjacentActorsIfPull
				    && Board.Get().GetSquaresAreAdjacent(actorData.GetCurrentBoardSquare(), caster.GetCurrentBoardSquare()))
				{
					Vector3 aimDir = caster.GetFreePos() - actorData.GetFreePos();
					aimDir.y = 0f;
					float distance = 2f;
					if (Board.Get().GetSquaresAreDiagonallyAdjacent(actorData.GetCurrentBoardSquare(), caster.GetCurrentBoardSquare()))
					{
						distance = 2.82f;
					}
					knockbackData = new KnockbackHitData(actorData, caster, KnockbackType.ForwardAlongAimDir, aimDir, centerOfShape, distance);
				}
				else
				{
					knockbackData = new KnockbackHitData(actorData, caster, m_knockbackType, targets[0].AimDirection, centerOfShape, m_knockbackDistance);
				}
				bombHitResults.AddKnockbackData(knockbackData);
				if (!isCooldownReductionApplied && GetCooldownChangePerHit() != 0)
				{
					int addAmount = GetCooldownChangePerHit() * bombHitActors.Count;
					MiscHitEventData_AddToCasterCooldown hitEvent = new MiscHitEventData_AddToCasterCooldown(m_myActionType, addAmount);
					bombHitResults.AddMiscHitEvent(hitEvent);
					isCooldownReductionApplied = true;
				}
				abilityResults.StoreActorHit(bombHitResults);
			}
			abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
	}

	// added in rogues
	private List<ActorData> GetBombHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return AreaEffectUtils.GetActorsInShape(m_bombShape, targets[0], m_bombPenetrateLineOfSight, caster, caster.GetOtherTeams(), nonActorTargetInfo);
	}

	// added in rogues
	private ActorData GetKnockbackCenterActor(List<AbilityTarget> targets, ActorData caster)
	{
		return targets[0].GetCurrentBestActorTarget();
	}

	// added in rogues
	public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0 || results.HasKnockback)
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.NanoSmithStats.VacuumBombHits);
		}
	}
#endif
}
