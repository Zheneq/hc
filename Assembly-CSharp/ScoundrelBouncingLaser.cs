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
		int result;
		if (m_abilityMod == null)
		{
			result = m_maxBounces;
		}
		else
		{
			result = m_abilityMod.m_maxBounceMod.GetModifiedValue(m_maxBounces);
		}
		return result;
	}

	public int GetMaxTargetHits()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_maxTargetsHit;
		}
		else
		{
			result = m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargetsHit);
		}
		return result;
	}

	public float GetLaserWidth()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_width;
		}
		else
		{
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_width);
		}
		return result;
	}

	public float GetDistancePerBounce()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_maxDistancePerBounce;
		}
		else
		{
			result = m_abilityMod.m_distancePerBounceMod.GetModifiedValue(m_maxDistancePerBounce);
		}
		return result;
	}

	public float GetMaxTotalDistance()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_maxTotalDistanceMod.GetModifiedValue(m_maxTotalDistance) : m_maxTotalDistance;
	}

	public int GetBaseDamage()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_baseDamageMod.GetModifiedValue(m_damageAmount) : m_damageAmount;
	}

	public int GetMinDamage()
	{
		int b;
		if (m_abilityMod == null)
		{
			b = m_minDamageAmount;
		}
		else
		{
			b = m_abilityMod.m_minDamageMod.GetModifiedValue(m_minDamageAmount);
		}
		return Mathf.Max(0, b);
	}

	public int GetBonusDamagePerBounce()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_bonusDamagePerBounceMod.GetModifiedValue(m_bonusDamagePerBounce) : m_bonusDamagePerBounce;
	}

	public int GetDamageChangePerHit()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_damageChangePerHit;
		}
		else
		{
			result = m_abilityMod.m_damageChangePerHitMod.GetModifiedValue(m_damageChangePerHit);
		}
		return result;
	}

	private void SetupTargeter()
	{
		ClearTargeters();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					base.Targeter = new AbilityUtil_Targeter_BounceLaser(this, GetLaserWidth(), GetDistancePerBounce(), GetMaxTotalDistance(), GetMaxBounces(), GetMaxTargetHits(), false);
					return;
				}
			}
		}
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			base.Targeters.Add(new AbilityUtil_Targeter_BounceLaser(this, GetLaserWidth(), GetDistancePerBounce(), GetMaxTotalDistance(), GetMaxBounces(), GetMaxTargetHits(), false));
			base.Targeters[i].SetUseMultiTargetUpdate(true);
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		float num = GetDistancePerBounce();
		if (CollectTheCoins.Get() != null)
		{
			float bonus_Client = CollectTheCoins.Get().m_bouncingLaserBounceDistance.GetBonus_Client(caster);
			num += bonus_Client;
		}
		return num;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result = 1;
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_useTargetDataOverrides && m_abilityMod.m_targetDataOverrides.Length > 1)
			{
				result = m_abilityMod.m_targetDataOverrides.Length;
			}
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		int num = GetBaseDamage();
		if (CollectTheCoins.Get() != null)
		{
			int num2 = Mathf.RoundToInt(CollectTheCoins.Get().m_bouncingLaserDamage.GetBonus_Client(base.ActorData));
			num += num2;
		}
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, num));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContexts =
			(base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_BounceLaser).GetHitActorContext();
		for (int i = 0; i < hitActorContexts.Count; i++)
		{
			AbilityUtil_Targeter_BounceLaser.HitActorContext hitActorContext = hitActorContexts[i];
			if (hitActorContext.actor == targetActor)
			{
				int damage = GetBaseDamage();
				if (CollectTheCoins.Get() != null)
				{
					damage += Mathf.RoundToInt(CollectTheCoins.Get().m_bouncingLaserDamage.GetBonus_Client(base.ActorData));
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
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ScoundrelBouncingLaser)
		{
			val = abilityMod_ScoundrelBouncingLaser.m_baseDamageMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			val = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_ScoundrelBouncingLaser)
		{
			val2 = abilityMod_ScoundrelBouncingLaser.m_minDamageMod.GetModifiedValue(m_minDamageAmount);
		}
		else
		{
			val2 = m_minDamageAmount;
		}
		AddTokenInt(tokens, "MinDamageAmount", empty2, val2);
		AddTokenInt(tokens, "DamageChangePerHit", string.Empty, (!abilityMod_ScoundrelBouncingLaser) ? m_damageChangePerHit : abilityMod_ScoundrelBouncingLaser.m_damageChangePerHitMod.GetModifiedValue(m_damageChangePerHit));
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_ScoundrelBouncingLaser)
		{
			val3 = abilityMod_ScoundrelBouncingLaser.m_bonusDamagePerBounceMod.GetModifiedValue(m_bonusDamagePerBounce);
		}
		else
		{
			val3 = m_bonusDamagePerBounce;
		}
		AddTokenInt(tokens, "BonusDamagePerBounce", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_ScoundrelBouncingLaser)
		{
			val4 = abilityMod_ScoundrelBouncingLaser.m_maxBounceMod.GetModifiedValue(m_maxBounces);
		}
		else
		{
			val4 = m_maxBounces;
		}
		AddTokenInt(tokens, "MaxBounces", empty4, val4);
		AddTokenInt(tokens, "MaxTargetsHit", string.Empty, (!abilityMod_ScoundrelBouncingLaser) ? m_maxTargetsHit : abilityMod_ScoundrelBouncingLaser.m_maxTargetsMod.GetModifiedValue(m_maxTargetsHit));
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ScoundrelBouncingLaser))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ScoundrelBouncingLaser);
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
