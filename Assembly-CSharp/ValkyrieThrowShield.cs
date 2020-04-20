using System;
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
	public int m_damageAmount = 0x14;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Ricoshield";
		}
		this.m_syncComp = base.GetComponent<Valkyrie_SyncComponent>();
		this.SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetMaxDistancePerBounce();
	}

	public float GetLaserWidth()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_widthMod.GetModifiedValue(this.m_width);
		}
		else
		{
			result = this.m_width;
		}
		return result;
	}

	public float GetMaxDistancePerBounce()
	{
		return (!this.m_abilityMod) ? this.m_maxDistancePerBounce : this.m_abilityMod.m_maxDistancePerBounceMod.GetModifiedValue(this.m_maxDistancePerBounce);
	}

	public float GetMaxTotalDistance()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxTotalDistanceMod.GetModifiedValue(this.m_maxTotalDistance);
		}
		else
		{
			result = this.m_maxTotalDistance;
		}
		return result;
	}

	public int GetMaxBounces()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxBouncesMod.GetModifiedValue(this.m_maxBounces);
		}
		else
		{
			result = this.m_maxBounces;
		}
		return result;
	}

	public int GetMaxTargetsHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxTargetsHitMod.GetModifiedValue(this.m_maxTargetsHit);
		}
		else
		{
			result = this.m_maxTargetsHit;
		}
		return result;
	}

	public bool BounceOnHitActor()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_bounceOnHitActorMod.GetModifiedValue(false);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public int GetBaseDamage()
	{
		return (!this.m_abilityMod) ? this.m_damageAmount : this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
	}

	public int GetBonusDamagePerBounce()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_bonusDamagePerBounceMod.GetModifiedValue(this.m_bonusDamagePerBounce);
		}
		else
		{
			result = this.m_bonusDamagePerBounce;
		}
		return result;
	}

	public int GetLessDamagePerTarget()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_lessDamagePerTargetMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public float GetKnockbackDistance()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(this.m_knockbackDistance);
		}
		else
		{
			result = this.m_knockbackDistance;
		}
		return result;
	}

	public float GetBonusKnockbackPerBounce()
	{
		return (!this.m_abilityMod) ? 0f : this.m_abilityMod.m_bonusKnockbackDistancePerBounceMod.GetModifiedValue(0f);
	}

	public KnockbackType GetKnockbackType()
	{
		KnockbackType result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_knockbackTypeMod.GetModifiedValue(this.m_knockbackType);
		}
		else
		{
			result = this.m_knockbackType;
		}
		return result;
	}

	public int GetMaxKnockbackTargets()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxKnockbackTargetsMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public AbilityModCooldownReduction GetCooldownReductionOnLaserHitCaster()
	{
		return (!this.m_abilityMod) ? null : this.m_abilityMod.m_cooldownReductionOnLaserHitCaster;
	}

	public int GetExtraDamage()
	{
		if (this.m_syncComp != null)
		{
			return this.m_syncComp.m_extraDamageNextShieldThrow;
		}
		return 0;
	}

	public float GetExtraKnockbackDistance(ActorData hitActor)
	{
		AbilityUtil_Targeter_BounceLaser abilityUtil_Targeter_BounceLaser = base.Targeter as AbilityUtil_Targeter_BounceLaser;
		if (abilityUtil_Targeter_BounceLaser != null)
		{
			ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContext = abilityUtil_Targeter_BounceLaser.GetHitActorContext();
			if (!hitActorContext.IsNullOrEmpty<AbilityUtil_Targeter_BounceLaser.HitActorContext>())
			{
				using (IEnumerator<AbilityUtil_Targeter_BounceLaser.HitActorContext> enumerator = hitActorContext.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityUtil_Targeter_BounceLaser.HitActorContext hitActorContext2 = enumerator.Current;
						if (hitActorContext2.actor == hitActor)
						{
							return this.GetBonusKnockbackPerBounce() * (float)hitActorContext2.segmentIndex;
						}
					}
				}
			}
		}
		return 0f;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyrieThrowShield))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ValkyrieThrowShield);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		AbilityUtil_Targeter_BounceLaser abilityUtil_Targeter_BounceLaser = new AbilityUtil_Targeter_BounceLaser(this, this.GetLaserWidth(), this.GetMaxDistancePerBounce(), this.GetMaxTotalDistance(), this.GetMaxBounces(), this.GetMaxTargetsHit(), this.BounceOnHitActor());
		abilityUtil_Targeter_BounceLaser.InitKnockbackData(this.GetKnockbackDistance(), this.GetKnockbackType(), this.GetMaxKnockbackTargets(), new AbilityUtil_Targeter_BounceLaser.ExtraKnockbackDelegate(this.GetExtraKnockbackDistance));
		abilityUtil_Targeter_BounceLaser.m_penetrateTargetsAndHitCaster = (this.GetCooldownReductionOnLaserHitCaster() != null && this.GetCooldownReductionOnLaserHitCaster().HasCooldownReduction());
		base.Targeter = abilityUtil_Targeter_BounceLaser;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetBaseDamage());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContext = (base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_BounceLaser).GetHitActorContext();
		for (int i = 0; i < hitActorContext.Count; i++)
		{
			if (hitActorContext[i].actor == targetActor)
			{
				int num = this.GetBonusDamagePerBounce() * hitActorContext[i].segmentIndex;
				int value = this.GetBaseDamage() + num + this.GetExtraDamage() - i * this.GetLessDamagePerTarget();
				dictionary[AbilityTooltipSymbol.Damage] = value;
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxBounces", string.Empty, this.m_maxBounces, false);
		base.AddTokenInt(tokens, "MaxTargetsHit", string.Empty, this.m_maxTargetsHit, false);
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
		base.AddTokenInt(tokens, "BonusDamagePerBounce", string.Empty, this.m_bonusDamagePerBounce, false);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Normal;
	}
}
