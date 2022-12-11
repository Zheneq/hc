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

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Vacuum Bomb";
		}
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
}
