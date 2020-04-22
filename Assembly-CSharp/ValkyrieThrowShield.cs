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
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_widthMod.GetModifiedValue(m_width);
		}
		else
		{
			result = m_width;
		}
		return result;
	}

	public float GetMaxDistancePerBounce()
	{
		return (!m_abilityMod) ? m_maxDistancePerBounce : m_abilityMod.m_maxDistancePerBounceMod.GetModifiedValue(m_maxDistancePerBounce);
	}

	public float GetMaxTotalDistance()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxTotalDistanceMod.GetModifiedValue(m_maxTotalDistance);
		}
		else
		{
			result = m_maxTotalDistance;
		}
		return result;
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

	public int GetMaxTargetsHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxTargetsHitMod.GetModifiedValue(m_maxTargetsHit);
		}
		else
		{
			result = m_maxTargetsHit;
		}
		return result;
	}

	public bool BounceOnHitActor()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = (m_abilityMod.m_bounceOnHitActorMod.GetModifiedValue(false) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public int GetBaseDamage()
	{
		return (!m_abilityMod) ? m_damageAmount : m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
	}

	public int GetBonusDamagePerBounce()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_bonusDamagePerBounceMod.GetModifiedValue(m_bonusDamagePerBounce);
		}
		else
		{
			result = m_bonusDamagePerBounce;
		}
		return result;
	}

	public int GetLessDamagePerTarget()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_lessDamagePerTargetMod.GetModifiedValue(0);
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
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance);
		}
		else
		{
			result = m_knockbackDistance;
		}
		return result;
	}

	public float GetBonusKnockbackPerBounce()
	{
		return (!m_abilityMod) ? 0f : m_abilityMod.m_bonusKnockbackDistancePerBounceMod.GetModifiedValue(0f);
	}

	public KnockbackType GetKnockbackType()
	{
		KnockbackType result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType);
		}
		else
		{
			result = m_knockbackType;
		}
		return result;
	}

	public int GetMaxKnockbackTargets()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxKnockbackTargetsMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public AbilityModCooldownReduction GetCooldownReductionOnLaserHitCaster()
	{
		return (!m_abilityMod) ? null : m_abilityMod.m_cooldownReductionOnLaserHitCaster;
	}

	public int GetExtraDamage()
	{
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_syncComp.m_extraDamageNextShieldThrow;
				}
			}
		}
		return 0;
	}

	public float GetExtraKnockbackDistance(ActorData hitActor)
	{
		AbilityUtil_Targeter_BounceLaser abilityUtil_Targeter_BounceLaser = base.Targeter as AbilityUtil_Targeter_BounceLaser;
		if (abilityUtil_Targeter_BounceLaser != null)
		{
			ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContext = abilityUtil_Targeter_BounceLaser.GetHitActorContext();
			if (!hitActorContext.IsNullOrEmpty())
			{
				using (IEnumerator<AbilityUtil_Targeter_BounceLaser.HitActorContext> enumerator = hitActorContext.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityUtil_Targeter_BounceLaser.HitActorContext current = enumerator.Current;
						if (current.actor == hitActor)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									return GetBonusKnockbackPerBounce() * (float)current.segmentIndex;
								}
							}
						}
					}
				}
			}
		}
		return 0f;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ValkyrieThrowShield))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ValkyrieThrowShield);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		AbilityUtil_Targeter_BounceLaser abilityUtil_Targeter_BounceLaser = new AbilityUtil_Targeter_BounceLaser(this, GetLaserWidth(), GetMaxDistancePerBounce(), GetMaxTotalDistance(), GetMaxBounces(), GetMaxTargetsHit(), BounceOnHitActor());
		abilityUtil_Targeter_BounceLaser.InitKnockbackData(GetKnockbackDistance(), GetKnockbackType(), GetMaxKnockbackTargets(), GetExtraKnockbackDistance);
		abilityUtil_Targeter_BounceLaser.m_penetrateTargetsAndHitCaster = (GetCooldownReductionOnLaserHitCaster() != null && GetCooldownReductionOnLaserHitCaster().HasCooldownReduction());
		base.Targeter = abilityUtil_Targeter_BounceLaser;
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
		ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContext = (base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_BounceLaser).GetHitActorContext();
		for (int i = 0; i < hitActorContext.Count; i++)
		{
			AbilityUtil_Targeter_BounceLaser.HitActorContext hitActorContext2 = hitActorContext[i];
			if (hitActorContext2.actor == targetActor)
			{
				int bonusDamagePerBounce = GetBonusDamagePerBounce();
				AbilityUtil_Targeter_BounceLaser.HitActorContext hitActorContext3 = hitActorContext[i];
				int num = bonusDamagePerBounce * hitActorContext3.segmentIndex;
				int num3 = dictionary[AbilityTooltipSymbol.Damage] = GetBaseDamage() + num + GetExtraDamage() - i * GetLessDamagePerTarget();
			}
		}
		while (true)
		{
			return dictionary;
		}
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
