using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class ScoundrelBouncingLaser : Ability
{
	public int m_damageAmount = 0x14;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Bouncing Laser";
		}
		this.SetupTargeter();
	}

	public int GetMaxBounces()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_maxBounces;
		}
		else
		{
			result = this.m_abilityMod.m_maxBounceMod.GetModifiedValue(this.m_maxBounces);
		}
		return result;
	}

	public int GetMaxTargetHits()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_maxTargetsHit;
		}
		else
		{
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargetsHit);
		}
		return result;
	}

	public float GetLaserWidth()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_width;
		}
		else
		{
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_width);
		}
		return result;
	}

	public float GetDistancePerBounce()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_maxDistancePerBounce;
		}
		else
		{
			result = this.m_abilityMod.m_distancePerBounceMod.GetModifiedValue(this.m_maxDistancePerBounce);
		}
		return result;
	}

	public float GetMaxTotalDistance()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_maxTotalDistanceMod.GetModifiedValue(this.m_maxTotalDistance) : this.m_maxTotalDistance;
	}

	public int GetBaseDamage()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_baseDamageMod.GetModifiedValue(this.m_damageAmount) : this.m_damageAmount;
	}

	public int GetMinDamage()
	{
		int a = 0;
		int b;
		if (this.m_abilityMod == null)
		{
			b = this.m_minDamageAmount;
		}
		else
		{
			b = this.m_abilityMod.m_minDamageMod.GetModifiedValue(this.m_minDamageAmount);
		}
		return Mathf.Max(a, b);
	}

	public int GetBonusDamagePerBounce()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_bonusDamagePerBounceMod.GetModifiedValue(this.m_bonusDamagePerBounce) : this.m_bonusDamagePerBounce;
	}

	public int GetDamageChangePerHit()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_damageChangePerHit;
		}
		else
		{
			result = this.m_abilityMod.m_damageChangePerHitMod.GetModifiedValue(this.m_damageChangePerHit);
		}
		return result;
	}

	private void SetupTargeter()
	{
		base.ClearTargeters();
		if (this.GetExpectedNumberOfTargeters() < 2)
		{
			base.Targeter = new AbilityUtil_Targeter_BounceLaser(this, this.GetLaserWidth(), this.GetDistancePerBounce(), this.GetMaxTotalDistance(), this.GetMaxBounces(), this.GetMaxTargetHits(), false);
		}
		else
		{
			for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
			{
				base.Targeters.Add(new AbilityUtil_Targeter_BounceLaser(this, this.GetLaserWidth(), this.GetDistancePerBounce(), this.GetMaxTotalDistance(), this.GetMaxBounces(), this.GetMaxTargetHits(), false));
				base.Targeters[i].SetUseMultiTargetUpdate(true);
			}
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		float num = this.GetDistancePerBounce();
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
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_useTargetDataOverrides && this.m_abilityMod.m_targetDataOverrides.Length > 1)
			{
				result = this.m_abilityMod.m_targetDataOverrides.Length;
			}
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		int num = this.GetBaseDamage();
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
		ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContext = (base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_BounceLaser).GetHitActorContext();
		for (int i = 0; i < hitActorContext.Count; i++)
		{
			if (hitActorContext[i].actor == targetActor)
			{
				int num = this.GetBaseDamage();
				if (CollectTheCoins.Get() != null)
				{
					int num2 = Mathf.RoundToInt(CollectTheCoins.Get().m_bouncingLaserDamage.GetBonus_Client(base.ActorData));
					num += num2;
				}
				num += this.GetDamageChangePerHit() * i;
				int num3 = this.GetBonusDamagePerBounce() * hitActorContext[i].segmentIndex;
				num += num3;
				num = Mathf.Max(this.GetMinDamage(), num);
				Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
				dictionary[AbilityTooltipSymbol.Damage] = num;
				return dictionary;
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ScoundrelBouncingLaser abilityMod_ScoundrelBouncingLaser = modAsBase as AbilityMod_ScoundrelBouncingLaser;
		string name = "DamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_ScoundrelBouncingLaser)
		{
			val = abilityMod_ScoundrelBouncingLaser.m_baseDamageMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			val = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "MinDamageAmount";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ScoundrelBouncingLaser)
		{
			val2 = abilityMod_ScoundrelBouncingLaser.m_minDamageMod.GetModifiedValue(this.m_minDamageAmount);
		}
		else
		{
			val2 = this.m_minDamageAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		base.AddTokenInt(tokens, "DamageChangePerHit", string.Empty, (!abilityMod_ScoundrelBouncingLaser) ? this.m_damageChangePerHit : abilityMod_ScoundrelBouncingLaser.m_damageChangePerHitMod.GetModifiedValue(this.m_damageChangePerHit), false);
		string name3 = "BonusDamagePerBounce";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_ScoundrelBouncingLaser)
		{
			val3 = abilityMod_ScoundrelBouncingLaser.m_bonusDamagePerBounceMod.GetModifiedValue(this.m_bonusDamagePerBounce);
		}
		else
		{
			val3 = this.m_bonusDamagePerBounce;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "MaxBounces";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_ScoundrelBouncingLaser)
		{
			val4 = abilityMod_ScoundrelBouncingLaser.m_maxBounceMod.GetModifiedValue(this.m_maxBounces);
		}
		else
		{
			val4 = this.m_maxBounces;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		base.AddTokenInt(tokens, "MaxTargetsHit", string.Empty, (!abilityMod_ScoundrelBouncingLaser) ? this.m_maxTargetsHit : abilityMod_ScoundrelBouncingLaser.m_maxTargetsMod.GetModifiedValue(this.m_maxTargetsHit), false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScoundrelBouncingLaser))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ScoundrelBouncingLaser);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
