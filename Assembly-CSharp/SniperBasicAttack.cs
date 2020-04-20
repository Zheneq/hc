using System;
using System.Collections.Generic;
using UnityEngine;

public class SniperBasicAttack : Ability
{
	public int m_laserDamageAmount = 5;

	public int m_minDamageAmount;

	public int m_damageChangePerHit;

	public LaserTargetingInfo m_laserInfo;

	public StandardEffectInfo m_laserHitEffect;

	private AbilityMod_SniperBasicAttack m_abilityMod;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Sniper Shot";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Laser(this, this.GetLaserWidth(), this.GetLaserRange(), this.GetLaserPenetratesLoS(), this.GetMaxTargets(), false, false);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserRange();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.m_laserDamageAmount > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_laserDamageAmount);
		}
		this.m_laserHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = base.Targeter as AbilityUtil_Targeter_Laser;
		if (abilityUtil_Targeter_Laser != null)
		{
			ActorData component = base.GetComponent<ActorData>();
			if (component != null)
			{
				List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContext = abilityUtil_Targeter_Laser.GetHitActorContext();
				for (int i = 0; i < hitActorContext.Count; i++)
				{
					if (hitActorContext[i].actor == targetActor)
					{
						Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
						if (targetActor.GetTeam() != component.GetTeam())
						{
							int damageAmountByHitOrder = this.GetDamageAmountByHitOrder(i, hitActorContext[i].squaresFromCaster);
							dictionary[AbilityTooltipSymbol.Damage] = damageAmountByHitOrder;
						}
						return dictionary;
					}
				}
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SniperBasicAttack abilityMod_SniperBasicAttack = modAsBase as AbilityMod_SniperBasicAttack;
		string name = "LaserDamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_SniperBasicAttack)
		{
			val = abilityMod_SniperBasicAttack.m_damageMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			val = this.m_laserDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "MinDamageAmount";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_SniperBasicAttack)
		{
			val2 = abilityMod_SniperBasicAttack.m_minDamageMod.GetModifiedValue(this.m_minDamageAmount);
		}
		else
		{
			val2 = this.m_minDamageAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "DamageChangePerHit";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_SniperBasicAttack)
		{
			val3 = abilityMod_SniperBasicAttack.m_damageChangePerHitMod.GetModifiedValue(this.m_damageChangePerHit);
		}
		else
		{
			val3 = this.m_damageChangePerHit;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_laserHitEffect, "LaserHitEffect", this.m_laserHitEffect, true);
	}

	public int GetDamageAmountByHitOrder(int hitOrder, float distanceFromCasterInSquares)
	{
		int num = this.GetBaseDamage();
		if (this.GetFarDistanceThreshold() > 0f)
		{
			if (distanceFromCasterInSquares > this.GetFarDistanceThreshold() && this.GetFarEnemyDamageAmount() > 0)
			{
				num = this.GetFarEnemyDamageAmount();
			}
		}
		int b = num + hitOrder * this.GetDamageChangePerHit();
		return Mathf.Max(this.GetMinDamage(), b);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SniperBasicAttack))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SniperBasicAttack);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public float GetLaserWidth()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserInfo.width) : this.m_laserInfo.width;
	}

	public float GetLaserRange()
	{
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_useTargetDataOverrides)
			{
				if (this.m_abilityMod.m_targetDataOverrides.Length > 0)
				{
					return this.m_abilityMod.m_targetDataOverrides[0].m_range;
				}
			}
			return this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_laserInfo.range);
		}
		return this.m_laserInfo.range;
	}

	public bool GetLaserPenetratesLoS()
	{
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_useTargetDataOverrides)
			{
				if (this.m_abilityMod.m_targetDataOverrides.Length > 0)
				{
					return !this.m_abilityMod.m_targetDataOverrides[0].m_checkLineOfSight;
				}
			}
		}
		return this.m_laserInfo.penetrateLos;
	}

	public int GetMaxTargets()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_laserInfo.maxTargets;
		}
		else
		{
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_laserInfo.maxTargets);
		}
		return result;
	}

	public int GetBaseDamage()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_laserDamageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		return result;
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

	public float GetFarDistanceThreshold()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_farDistanceThreshold : 0f;
	}

	public int GetFarEnemyDamageAmount()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_laserDamageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_farEnemyDamageMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		return result;
	}
}
