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

	[Separator("Chase On Hit Data", true)]
	public bool m_chaseHitActor;

	public StandardEffectInfo m_chaserEffect;

	[Separator("Bounce", true)]
	public float m_width = 1f;

	public float m_maxDistancePerBounce = 15f;

	public float m_maxTotalDistance = 50f;

	public int m_maxBounces = 1;

	public int m_maxTargetsHit = 1;

	public bool m_bounceOffEnemyActor;

	[Separator("Bounce Anim", true)]
	public float m_recoveryTime = 0.5f;

	[Separator("Sequences", true)]
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
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		float width = m_width;
		float maxDistancePerBounce = GetMaxDistancePerBounce();
		float maxTotalDistance = GetMaxTotalDistance();
		int maxBounces = GetMaxBounces();
		int maxTargets = GetMaxTargets();
		bool bounceOnEnemyActor = ShouldBounceOffEnemyActors();
		bool includeAlliesInBetween = IncludeAlliesInBetween();
		int includeSelf;
		if (moddedEffectForSelf != null)
		{
			includeSelf = (moddedEffectForSelf.m_applyEffect ? 1 : 0);
		}
		else
		{
			includeSelf = 0;
		}
		base.Targeter = new AbilityUtil_Targeter_BounceActor(this, width, maxDistancePerBounce, maxTotalDistance, maxBounces, maxTargets, bounceOnEnemyActor, includeAlliesInBetween, (byte)includeSelf != 0);
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
		int result;
		if (m_abilityMod == null)
		{
			result = m_damageAmount;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount);
		}
		return result;
	}

	public int GetDamageAfterFirstHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageAfterFirstHitMod.GetModifiedValue(m_damageAfterFirstHit);
		}
		else
		{
			result = m_damageAfterFirstHit;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_maxTargetsHit;
		}
		else
		{
			result = m_abilityMod.m_maxHitTargetsMod.GetModifiedValue(m_maxTargetsHit);
		}
		return result;
	}

	public bool ShouldBounceOffEnemyActors()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_bounceOffEnemyActorMod.GetModifiedValue(m_bounceOffEnemyActor) : m_bounceOffEnemyActor;
	}

	public bool IncludeAlliesInBetween()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = (m_abilityMod.m_hitAlliesInBetween.GetModifiedValue(false) ? 1 : 0);
		}
		return (byte)result != 0;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		object result;
		if (m_abilityMod == null)
		{
			result = null;
		}
		else
		{
			result = m_abilityMod.m_allyHitEffect;
		}
		return (StandardEffectInfo)result;
	}

	public int GetHealAmountIfNotDamagedThisTurn()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_healAmountIfNotDamagedThisTurn.GetModifiedValue(0) : 0;
	}

	public float GetMaxDistancePerBounce()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxDistancePerBounceMod.GetModifiedValue(m_maxDistancePerBounce);
		}
		else
		{
			result = m_maxDistancePerBounce;
		}
		return result;
	}

	public float GetMaxTotalDistance()
	{
		return (!m_abilityMod) ? m_maxTotalDistance : m_abilityMod.m_maxTotalDistanceMod.GetModifiedValue(m_maxTotalDistance);
	}

	public int GetMaxBounces()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxBouncesMod.GetModifiedValue(m_maxBounces);
		}
		else
		{
			result = m_maxBounces;
		}
		return result;
	}

	public int CalcDamageForOrderIndex(int hitOrder)
	{
		int damageAfterFirstHit = GetDamageAfterFirstHit();
		if (damageAfterFirstHit > 0 && hitOrder > 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return damageAfterFirstHit;
				}
			}
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
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			AbilityUtil_Targeter_BounceActor abilityUtil_Targeter_BounceActor = base.Targeter as AbilityUtil_Targeter_BounceActor;
			if (abilityUtil_Targeter_BounceActor != null)
			{
				List<AbilityUtil_Targeter_BounceActor.HitActorContext> hitActorContext = abilityUtil_Targeter_BounceActor.GetHitActorContext();
				int num = 0;
				while (true)
				{
					if (num < hitActorContext.Count)
					{
						AbilityUtil_Targeter_BounceActor.HitActorContext hitActorContext2 = hitActorContext[num];
						if (hitActorContext2.actor == targetActor)
						{
							results.m_damage = CalcDamageForOrderIndex(num);
							break;
						}
						num++;
						continue;
					}
					break;
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
		if (abilityMod.GetType() != typeof(AbilityMod_BattleMonkBoundingLeap))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_BattleMonkBoundingLeap);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
