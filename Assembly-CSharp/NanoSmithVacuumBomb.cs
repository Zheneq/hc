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
		float knockbackDistance = (m_knockbackDelay > 0) ? 0f : m_knockbackDistance;
		AbilityUtil_Targeter.AffectsActor affectsTargetOnGridposSquare = AbilityUtil_Targeter.AffectsActor.Never;
		if (GetModdedEffectForAllies() != null)
		{
			if (GetModdedEffectForAllies().m_applyEffect)
			{
				goto IL_006e;
			}
		}
		if (GetCenterActorEffect().m_applyEffect)
		{
			goto IL_006e;
		}
		goto IL_0070;
		IL_006e:
		affectsTargetOnGridposSquare = AbilityUtil_Targeter.AffectsActor.Always;
		goto IL_0070;
		IL_0070:
		base.Targeter = new AbilityUtil_Targeter_KnockbackRingAoE(this, m_bombShape, m_bombPenetrateLineOfSight, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Never, affectsTargetOnGridposSquare, m_bombShape, knockbackDistance, m_knockbackType, m_knockbackAdjacentActorsIfPull, true);
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
		if (m_syncComp != null && m_syncComp.m_extraAbsorbOnVacuumBomb > 0)
		{
			if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) == 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						StandardEffectInfo centerActorEffect = GetCenterActorEffect();
						int absorbAmount = centerActorEffect.m_effectData.m_absorbAmount;
						absorbAmount = (results.m_absorb = absorbAmount + m_syncComp.m_extraAbsorbOnVacuumBomb);
						return true;
					}
					}
				}
			}
		}
		return false;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(caster, currentBestActorTarget, false, true, true, ValidateCheckPath.Ignore, true, true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithVacuumBomb abilityMod_NanoSmithVacuumBomb = modAsBase as AbilityMod_NanoSmithVacuumBomb;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_NanoSmithVacuumBomb)
		{
			val = abilityMod_NanoSmithVacuumBomb.m_damageMod.GetModifiedValue(m_bombDamageAmount);
		}
		else
		{
			val = m_bombDamageAmount;
		}
		AddTokenInt(tokens, "BombDamageAmount", empty, val);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_NanoSmithVacuumBomb)
		{
			effectInfo = abilityMod_NanoSmithVacuumBomb.m_enemyHitEffectOverride.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			effectInfo = m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", m_enemyHitEffect);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_NanoSmithVacuumBomb)
		{
			effectInfo2 = abilityMod_NanoSmithVacuumBomb.m_onCenterActorEffectOverride.GetModifiedValue(m_onCenterActorEffect);
		}
		else
		{
			effectInfo2 = m_onCenterActorEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "OnCenterActorEffect", m_onCenterActorEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NanoSmithVacuumBomb))
		{
			m_abilityMod = (abilityMod as AbilityMod_NanoSmithVacuumBomb);
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
		StandardEffectInfo cachedOnCenterActorEffect;
		if ((bool)m_abilityMod)
		{
			cachedOnCenterActorEffect = m_abilityMod.m_onCenterActorEffectOverride.GetModifiedValue(m_onCenterActorEffect);
		}
		else
		{
			cachedOnCenterActorEffect = m_onCenterActorEffect;
		}
		m_cachedOnCenterActorEffect = cachedOnCenterActorEffect;
		StandardEffectInfo cachedEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectOverride.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
	}

	private int GetDamageAmount()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_bombDamageAmount;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_bombDamageAmount);
		}
		return result;
	}

	private int GetCooldownChangePerHit()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_cooldownChangePerHitMod.GetModifiedValue(0);
		}
		return result;
	}

	private StandardEffectInfo GetCenterActorEffect()
	{
		StandardEffectInfo result;
		if (m_cachedOnCenterActorEffect != null)
		{
			result = m_cachedOnCenterActorEffect;
		}
		else
		{
			result = m_onCenterActorEffect;
		}
		return result;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		return (m_cachedEnemyHitEffect == null) ? m_enemyHitEffect : m_cachedEnemyHitEffect;
	}

	public int GetExtraAbsorb()
	{
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_syncComp.m_extraAbsorbOnVacuumBomb;
				}
			}
		}
		return 0;
	}
}
