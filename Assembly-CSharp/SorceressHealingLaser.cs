using System;
using System.Collections.Generic;
using UnityEngine;

public class SorceressHealingLaser : Ability
{
	public bool m_includeAllies = true;

	public bool m_penetrateLineOfSight;

	public float m_width = 1f;

	public float m_distance = 15f;

	[Header("-- Damage")]
	public int m_damageAmount = 0xA;

	public int m_minDamageAmount;

	public int m_damageChangePerHit;

	[Header("-- Heal")]
	public int m_selfHealAmount = 1;

	public int m_allyHealAmount = 1;

	public int m_minHealAmount;

	public int m_healChangePerHit;

	private AbilityUtil_Targeter_Laser m_laserTargeter;

	private AbilityMod_SorceressHealingLaser m_abilityMod;

	private void Start()
	{
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.m_laserTargeter = new AbilityUtil_Targeter_Laser(this, this.ModdedLaserWidth(), this.ModdedLaserRange(), this.m_penetrateLineOfSight, -1, this.m_includeAllies, this.ModdedBaseHealOnSelf() > 0);
		base.Targeter = this.m_laserTargeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.ModdedLaserRange();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.m_damageAmount));
		if (this.m_includeAllies)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, this.m_allyHealAmount));
			int num = this.ModdedAllyTechPointGain();
			if (num > 0)
			{
				list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Ally, num));
			}
		}
		if (this.m_selfHealAmount > 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, this.m_selfHealAmount));
		}
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.m_damageAmount));
		if (this.m_includeAllies)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, this.m_allyHealAmount));
			int num = this.ModdedAllyTechPointGain();
			if (num > 0)
			{
				list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Ally, num));
			}
		}
		if (this.ModdedBaseHealOnSelf() > 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, this.ModdedBaseHealOnSelf()));
		}
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (this.m_laserTargeter != null)
		{
			ActorData component = base.GetComponent<ActorData>();
			if (component != null)
			{
				List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContext = this.m_laserTargeter.GetHitActorContext();
				for (int i = 0; i < hitActorContext.Count; i++)
				{
					if (hitActorContext[i].actor == targetActor)
					{
						Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
						if (component == targetActor)
						{
							int value = this.ModdedBaseHealOnSelf();
							dictionary[AbilityTooltipSymbol.Healing] = value;
						}
						else if (targetActor.GetTeam() == component.GetTeam())
						{
							int healAmountByHitOrder = this.GetHealAmountByHitOrder(i);
							dictionary[AbilityTooltipSymbol.Healing] = healAmountByHitOrder;
						}
						else
						{
							int damageAmountByHitOrder = this.GetDamageAmountByHitOrder(i);
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
		AbilityMod_SorceressHealingLaser abilityMod_SorceressHealingLaser = modAsBase as AbilityMod_SorceressHealingLaser;
		string name = "DamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_SorceressHealingLaser)
		{
			val = abilityMod_SorceressHealingLaser.m_damageMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			val = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "MinDamageAmount";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_SorceressHealingLaser)
		{
			val2 = abilityMod_SorceressHealingLaser.m_minDamageMod.GetModifiedValue(this.m_minDamageAmount);
		}
		else
		{
			val2 = this.m_minDamageAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "DamageChangePerHit";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_SorceressHealingLaser)
		{
			val3 = abilityMod_SorceressHealingLaser.m_damageChangePerHitMod.GetModifiedValue(this.m_damageChangePerHit);
		}
		else
		{
			val3 = this.m_damageChangePerHit;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		base.AddTokenInt(tokens, "SelfHealAmount", string.Empty, (!abilityMod_SorceressHealingLaser) ? this.m_selfHealAmount : abilityMod_SorceressHealingLaser.m_selfHealMod.GetModifiedValue(this.m_selfHealAmount), false);
		base.AddTokenInt(tokens, "AllyHealAmount", string.Empty, (!abilityMod_SorceressHealingLaser) ? this.m_allyHealAmount : abilityMod_SorceressHealingLaser.m_allyHealMod.GetModifiedValue(this.m_allyHealAmount), false);
		string name4 = "MinHealAmount";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_SorceressHealingLaser)
		{
			val4 = abilityMod_SorceressHealingLaser.m_minHealMod.GetModifiedValue(this.m_minHealAmount);
		}
		else
		{
			val4 = this.m_minHealAmount;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		base.AddTokenInt(tokens, "HealChangePerHit", string.Empty, (!abilityMod_SorceressHealingLaser) ? this.m_healChangePerHit : abilityMod_SorceressHealingLaser.m_healChangePerHitMod.GetModifiedValue(this.m_healChangePerHit), false);
	}

	public int GetHealAmountByHitOrder(int hitOrder)
	{
		int num = this.ModdedBaseHealOnAlly();
		num += this.ModdedHealChangePerHit() * hitOrder;
		return Mathf.Max(this.ModdedMinHealAmountOnAlly(), num);
	}

	public int GetDamageAmountByHitOrder(int hitOrder)
	{
		int num = this.ModdedBaseDamage();
		num += this.ModdedDamageChangePerHit() * hitOrder;
		return Mathf.Max(this.ModdedMinDamage(), num);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SorceressHealingLaser))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SorceressHealingLaser);
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

	public int ModdedBaseHealOnSelf()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_selfHealAmount;
		}
		else
		{
			result = this.m_abilityMod.m_selfHealMod.GetModifiedValue(this.m_selfHealAmount);
		}
		return result;
	}

	public int ModdedBaseHealOnAlly()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_allyHealMod.GetModifiedValue(this.m_allyHealAmount) : this.m_allyHealAmount;
	}

	public int ModdedMinHealAmountOnAlly()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_minHealMod.GetModifiedValue(this.m_minHealAmount) : this.m_minHealAmount;
	}

	public int ModdedHealChangePerHit()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_healChangePerHitMod.GetModifiedValue(this.m_healChangePerHit) : this.m_healChangePerHit;
	}

	public int ModdedBaseDamage()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_damageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damageAmount);
		}
		return result;
	}

	public int ModdedMinDamage()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_minDamageMod.GetModifiedValue(this.m_minDamageAmount) : this.m_minDamageAmount;
	}

	public int ModdedDamageChangePerHit()
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

	public float ModdedLaserWidth()
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

	public float ModdedLaserRange()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_distance) : this.m_distance;
	}

	public int ModdedAllyTechPointGain()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_allyTechPointGain.GetModifiedValue(0) : 0;
	}
}
