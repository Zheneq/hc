using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class ValkyrieThrowShield : Ability
{
	[Header("-- Targeting")]
	public float m_width = 1f;
	public float m_maxDistancePerBounce = 15f;
	public float m_maxTotalDistance = 50f;
	public int m_maxBounces = 1;
	public int m_maxTargetsHit = 1;
	[Header("-- Damage")]
	public int m_damageAmount = 20;
	public int m_bonusDamagePerBounce;
	[Header("-- Knockback")]
	public float m_knockbackDistance;
	public KnockbackType m_knockbackType;
	[Header("-- Sequences")]
	public GameObject m_projectileSequence;

	private Valkyrie_SyncComponent m_syncComp;
	private AbilityMod_ValkyrieThrowShield m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Ricoshield";
		}
		m_syncComp = GetComponent<Valkyrie_SyncComponent>();
		SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxDistancePerBounce();
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_widthMod.GetModifiedValue(m_width)
			: m_width;
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

	public int GetMaxTargetsHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsHitMod.GetModifiedValue(m_maxTargetsHit)
			: m_maxTargetsHit;
	}

	public bool BounceOnHitActor()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_bounceOnHitActorMod.GetModifiedValue(false);
	}

	public int GetBaseDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetBonusDamagePerBounce()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bonusDamagePerBounceMod.GetModifiedValue(m_bonusDamagePerBounce)
			: m_bonusDamagePerBounce;
	}

	public int GetLessDamagePerTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lessDamagePerTargetMod.GetModifiedValue(0)
			: 0;
	}

	public float GetKnockbackDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance)
			: m_knockbackDistance;
	}

	public float GetBonusKnockbackPerBounce()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bonusKnockbackDistancePerBounceMod.GetModifiedValue(0f)
			: 0f;
	}

	public KnockbackType GetKnockbackType()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType)
			: m_knockbackType;
	}

	public int GetMaxKnockbackTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxKnockbackTargetsMod.GetModifiedValue(0)
			: 0;
	}

	public AbilityModCooldownReduction GetCooldownReductionOnLaserHitCaster()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownReductionOnLaserHitCaster
			: null;
	}

	public int GetExtraDamage()
	{
		return m_syncComp != null
			? m_syncComp.m_extraDamageNextShieldThrow
			: 0;
	}

	public float GetExtraKnockbackDistance(ActorData hitActor)
	{
		AbilityUtil_Targeter_BounceLaser abilityUtil_Targeter_BounceLaser = Targeter as AbilityUtil_Targeter_BounceLaser;
		if (abilityUtil_Targeter_BounceLaser == null)
		{
			return 0f;
		}
		ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContext = abilityUtil_Targeter_BounceLaser.GetHitActorContext();
		if (!hitActorContext.IsNullOrEmpty())
		{
			foreach (AbilityUtil_Targeter_BounceLaser.HitActorContext current in hitActorContext)
			{
				if (current.actor == hitActor)
				{
					return GetBonusKnockbackPerBounce() * current.segmentIndex;
				}
			}
		}
		return 0f;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyrieThrowShield))
		{
			m_abilityMod = abilityMod as AbilityMod_ValkyrieThrowShield;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		AbilityUtil_Targeter_BounceLaser targeter = new AbilityUtil_Targeter_BounceLaser(
			this,
			GetLaserWidth(),
			GetMaxDistancePerBounce(),
			GetMaxTotalDistance(),
			GetMaxBounces(),
			GetMaxTargetsHit(),
			BounceOnHitActor());
		targeter.InitKnockbackData(
			GetKnockbackDistance(),
			GetKnockbackType(),
			GetMaxKnockbackTargets(),
			GetExtraKnockbackDistance);
		targeter.m_penetrateTargetsAndHitCaster = GetCooldownReductionOnLaserHitCaster() != null
		                                          && GetCooldownReductionOnLaserHitCaster().HasCooldownReduction();
		Targeter = targeter;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetBaseDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContexts = (Targeters[currentTargeterIndex] as AbilityUtil_Targeter_BounceLaser).GetHitActorContext();
		for (int i = 0; i < hitActorContexts.Count; i++)
		{
			AbilityUtil_Targeter_BounceLaser.HitActorContext hitActorContext = hitActorContexts[i];
			if (hitActorContext.actor == targetActor)
			{
				dictionary[AbilityTooltipSymbol.Damage] =
					GetBaseDamage()
					+ GetBonusDamagePerBounce() * hitActorContext.segmentIndex
					+ GetExtraDamage()
					- i * GetLessDamagePerTarget();
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxBounces", string.Empty, m_maxBounces);
		AddTokenInt(tokens, "MaxTargetsHit", string.Empty, m_maxTargetsHit);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "BonusDamagePerBounce", string.Empty, m_bonusDamagePerBounce);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Normal;
	}
}
