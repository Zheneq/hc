using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class ScoundrelBouncingLaser : Ability
{
	public int m_damageAmount = 20;
	public int m_minDamageAmount;
	public int m_damageChangePerHit;
	public int m_bonusDamagePerBounce;
	public float m_width = 1f;
	public float m_maxDistancePerBounce = 15f;
	public float m_maxTotalDistance = 50f;
	public int m_maxBounces = 1;
	public int m_maxTargetsHit = 1;

	private const bool c_penetrateLoS = false;

	private AbilityMod_ScoundrelBouncingLaser m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Bouncing Laser";
		}
		SetupTargeter();
	}

	public int GetMaxBounces()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxBounceMod.GetModifiedValue(m_maxBounces)
			: m_maxBounces;
	}

	public int GetMaxTargetHits()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargetsHit)
			: m_maxTargetsHit;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_width)
			: m_width;
	}

	public float GetDistancePerBounce()
	{
		return m_abilityMod != null
			? m_abilityMod.m_distancePerBounceMod.GetModifiedValue(m_maxDistancePerBounce)
			: m_maxDistancePerBounce;
	}

	public float GetMaxTotalDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTotalDistanceMod.GetModifiedValue(m_maxTotalDistance)
			: m_maxTotalDistance;
	}

	public int GetBaseDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_baseDamageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetMinDamage()
	{
		int minDamage = m_abilityMod == null
			? m_minDamageAmount
			: m_abilityMod.m_minDamageMod.GetModifiedValue(m_minDamageAmount);
		return Mathf.Max(0, minDamage);
	}

	public int GetBonusDamagePerBounce()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bonusDamagePerBounceMod.GetModifiedValue(m_bonusDamagePerBounce)
			: m_bonusDamagePerBounce;
	}

	public int GetDamageChangePerHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageChangePerHitMod.GetModifiedValue(m_damageChangePerHit)
			: m_damageChangePerHit;
	}

	private void SetupTargeter()
	{
		ClearTargeters();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			Targeter = new AbilityUtil_Targeter_BounceLaser(this, GetLaserWidth(), GetDistancePerBounce(), GetMaxTotalDistance(), GetMaxBounces(), GetMaxTargetHits(), false);
		}
		else
		{
			for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
			{
				Targeters.Add(new AbilityUtil_Targeter_BounceLaser(this, GetLaserWidth(), GetDistancePerBounce(), GetMaxTotalDistance(), GetMaxBounces(), GetMaxTargetHits(), false));
				Targeters[i].SetUseMultiTargetUpdate(true);
			}
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		float dist = GetDistancePerBounce();
		if (CollectTheCoins.Get() != null)
		{
			float bonus_Client = CollectTheCoins.Get().m_bouncingLaserBounceDistance.GetBonus_Client(caster);
			dist += bonus_Client;
		}
		return dist;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_abilityMod != null && m_abilityMod.m_useTargetDataOverrides && m_abilityMod.m_targetDataOverrides.Length > 1
			? m_abilityMod.m_targetDataOverrides.Length
			: 1;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		int damage = GetBaseDamage();
		if (CollectTheCoins.Get() != null)
		{
			damage += Mathf.RoundToInt(CollectTheCoins.Get().m_bouncingLaserDamage.GetBonus_Client(ActorData));
		}
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, damage)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContexts =
			(Targeters[currentTargeterIndex] as AbilityUtil_Targeter_BounceLaser).GetHitActorContext();
		for (int i = 0; i < hitActorContexts.Count; i++)
		{
			AbilityUtil_Targeter_BounceLaser.HitActorContext hitActorContext = hitActorContexts[i];
			if (hitActorContext.actor == targetActor)
			{
				int damage = GetBaseDamage();
				if (CollectTheCoins.Get() != null)
				{
					damage += Mathf.RoundToInt(CollectTheCoins.Get().m_bouncingLaserDamage.GetBonus_Client(ActorData));
				}
				damage += GetDamageChangePerHit() * i;
				damage += GetBonusDamagePerBounce() * hitActorContext.segmentIndex;
				damage = Mathf.Max(GetMinDamage(), damage);
				return new Dictionary<AbilityTooltipSymbol, int>()
				{
					{ AbilityTooltipSymbol.Damage, damage }
				};
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ScoundrelBouncingLaser abilityMod_ScoundrelBouncingLaser = modAsBase as AbilityMod_ScoundrelBouncingLaser;
		AddTokenInt(tokens, "DamageAmount", "", abilityMod_ScoundrelBouncingLaser != null
			? abilityMod_ScoundrelBouncingLaser.m_baseDamageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AddTokenInt(tokens, "MinDamageAmount", "", abilityMod_ScoundrelBouncingLaser != null
			? abilityMod_ScoundrelBouncingLaser.m_minDamageMod.GetModifiedValue(m_minDamageAmount)
			: m_minDamageAmount);
		AddTokenInt(tokens, "DamageChangePerHit", "", abilityMod_ScoundrelBouncingLaser
			? abilityMod_ScoundrelBouncingLaser.m_damageChangePerHitMod.GetModifiedValue(m_damageChangePerHit)
			: m_damageChangePerHit);
		AddTokenInt(tokens, "BonusDamagePerBounce", "", abilityMod_ScoundrelBouncingLaser != null
			? abilityMod_ScoundrelBouncingLaser.m_bonusDamagePerBounceMod.GetModifiedValue(m_bonusDamagePerBounce)
			: m_bonusDamagePerBounce);
		AddTokenInt(tokens, "MaxBounces", "", abilityMod_ScoundrelBouncingLaser != null
			? abilityMod_ScoundrelBouncingLaser.m_maxBounceMod.GetModifiedValue(m_maxBounces)
			: m_maxBounces);
		AddTokenInt(tokens, "MaxTargetsHit", "", (!abilityMod_ScoundrelBouncingLaser) ? m_maxTargetsHit : abilityMod_ScoundrelBouncingLaser.m_maxTargetsMod.GetModifiedValue(m_maxTargetsHit));
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScoundrelBouncingLaser))
		{
			m_abilityMod = (abilityMod as AbilityMod_ScoundrelBouncingLaser);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
