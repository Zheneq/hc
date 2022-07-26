using System.Collections.Generic;
using UnityEngine;

public class BattleMonkBoundingLeap : Ability
{
	[Header("-- On Hit Damage, Effect, etc")]
	public int m_damageAmount = 20;
	public int m_damageAfterFirstHit;
	[Space(10f)]
	public StandardEffectInfo m_targetEffect;
	public int m_cooldownOnHit = -1;
	[Separator("Chase On Hit Data")]
	public bool m_chaseHitActor;
	public StandardEffectInfo m_chaserEffect;
	[Separator("Bounce")]
	public float m_width = 1f;
	public float m_maxDistancePerBounce = 15f;
	public float m_maxTotalDistance = 50f;
	public int m_maxBounces = 1;
	public int m_maxTargetsHit = 1;
	public bool m_bounceOffEnemyActor;
	[Separator("Bounce Anim")]
	public float m_recoveryTime = 0.5f;
	[Separator("Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_sequenceOnCaster;

	private const bool c_penetrateLoS = false;

	private AbilityMod_BattleMonkBoundingLeap m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Bounding Leap";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_BounceActor(
			this,
			m_width,
			GetMaxDistancePerBounce(),
			GetMaxTotalDistance(),
			GetMaxBounces(),
			GetMaxTargets(),
			ShouldBounceOffEnemyActors(),
			IncludeAlliesInBetween(),
			GetModdedEffectForSelf() != null && GetModdedEffectForSelf().m_applyEffect);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxDistancePerBounce();
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetDamageAfterFirstHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAfterFirstHitMod.GetModifiedValue(m_damageAfterFirstHit)
			: m_damageAfterFirstHit;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxHitTargetsMod.GetModifiedValue(m_maxTargetsHit)
			: m_maxTargetsHit;
	}

	public bool ShouldBounceOffEnemyActors()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bounceOffEnemyActorMod.GetModifiedValue(m_bounceOffEnemyActor)
			: m_bounceOffEnemyActor;
	}

	public bool IncludeAlliesInBetween()
	{
		return m_abilityMod != null && m_abilityMod.m_hitAlliesInBetween.GetModifiedValue(false);
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHitEffect
			: null;
	}

	public int GetHealAmountIfNotDamagedThisTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healAmountIfNotDamagedThisTurn.GetModifiedValue(0)
			: 0;
	}

	public float GetMaxDistancePerBounce()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxDistancePerBounceMod.GetModifiedValue(m_maxDistancePerBounce)
			: m_maxDistancePerBounce;
	}

	public float GetMaxTotalDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTotalDistanceMod.GetModifiedValue(m_maxTotalDistance)
			: m_maxTotalDistance;
	}

	public int GetMaxBounces()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxBouncesMod.GetModifiedValue(m_maxBounces)
			: m_maxBounces;
	}

	public int CalcDamageForOrderIndex(int hitOrder)
	{
		int damageAfterFirstHit = GetDamageAfterFirstHit();
		if (damageAfterFirstHit > 0 && hitOrder > 0)
		{
			return damageAfterFirstHit;
		}
		return GetDamageAmount();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
		if (GetAllyHitEffect() != null)
		{
			GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		AppendTooltipNumbersFromBaseModEffects(ref numbers);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			AbilityUtil_Targeter_BounceActor abilityUtil_Targeter_BounceActor = Targeter as AbilityUtil_Targeter_BounceActor;
			if (abilityUtil_Targeter_BounceActor != null)
			{
				List<AbilityUtil_Targeter_BounceActor.HitActorContext> hitActorContext = abilityUtil_Targeter_BounceActor.GetHitActorContext();
				for (int i = 0; i < hitActorContext.Count; i++)
				{
					if (hitActorContext[i].actor == targetActor)
					{
						results.m_damage = CalcDamageForOrderIndex(i);
						break;
					}
				}
				return true;
			}
		}
		return false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "DamageAfterFirstHit", string.Empty, m_damageAfterFirstHit);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetEffect, "TargetEffect", m_targetEffect);
		AddTokenInt(tokens, "CooldownOnHit", string.Empty, m_cooldownOnHit);
		AbilityMod.AddToken_EffectInfo(tokens, m_chaserEffect, "ChaserEffect", m_chaserEffect);
		AddTokenInt(tokens, "MaxBounces", string.Empty, m_maxBounces);
		AddTokenInt(tokens, "MaxTargetsHit", string.Empty, m_maxTargetsHit);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BattleMonkBoundingLeap))
		{
			m_abilityMod = abilityMod as AbilityMod_BattleMonkBoundingLeap;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
