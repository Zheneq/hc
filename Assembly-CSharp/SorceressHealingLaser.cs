using System.Collections.Generic;
using UnityEngine;

public class SorceressHealingLaser : Ability
{
	public bool m_includeAllies = true;

	public bool m_penetrateLineOfSight;

	public float m_width = 1f;

	public float m_distance = 15f;

	[Header("-- Damage")]
	public int m_damageAmount = 10;

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
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_laserTargeter = new AbilityUtil_Targeter_Laser(this, ModdedLaserWidth(), ModdedLaserRange(), m_penetrateLineOfSight, -1, m_includeAllies, ModdedBaseHealOnSelf() > 0);
		base.Targeter = m_laserTargeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return ModdedLaserRange();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount));
		if (m_includeAllies)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_allyHealAmount));
			int num = ModdedAllyTechPointGain();
			if (num > 0)
			{
				list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Ally, num));
			}
		}
		if (m_selfHealAmount > 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, m_selfHealAmount));
		}
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount));
		if (m_includeAllies)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_allyHealAmount));
			int num = ModdedAllyTechPointGain();
			if (num > 0)
			{
				list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Energy, AbilityTooltipSubject.Ally, num));
			}
		}
		if (ModdedBaseHealOnSelf() > 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, ModdedBaseHealOnSelf()));
		}
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (m_laserTargeter != null)
		{
			ActorData component = GetComponent<ActorData>();
			if (component != null)
			{
				List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContext = m_laserTargeter.GetHitActorContext();
				for (int i = 0; i < hitActorContext.Count; i++)
				{
					AbilityUtil_Targeter_Laser.HitActorContext hitActorContext2 = hitActorContext[i];
					if (!(hitActorContext2.actor == targetActor))
					{
						continue;
					}
					while (true)
					{
						Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
						if (component == targetActor)
						{
							int num2 = dictionary[AbilityTooltipSymbol.Healing] = ModdedBaseHealOnSelf();
						}
						else if (targetActor.GetTeam() != component.GetTeam())
						{
							int num3 = dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmountByHitOrder(i);
						}
						else
						{
							int num4 = dictionary[AbilityTooltipSymbol.Healing] = GetHealAmountByHitOrder(i);
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
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SorceressHealingLaser)
		{
			val = abilityMod_SorceressHealingLaser.m_damageMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			val = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_SorceressHealingLaser)
		{
			val2 = abilityMod_SorceressHealingLaser.m_minDamageMod.GetModifiedValue(m_minDamageAmount);
		}
		else
		{
			val2 = m_minDamageAmount;
		}
		AddTokenInt(tokens, "MinDamageAmount", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_SorceressHealingLaser)
		{
			val3 = abilityMod_SorceressHealingLaser.m_damageChangePerHitMod.GetModifiedValue(m_damageChangePerHit);
		}
		else
		{
			val3 = m_damageChangePerHit;
		}
		AddTokenInt(tokens, "DamageChangePerHit", empty3, val3);
		AddTokenInt(tokens, "SelfHealAmount", string.Empty, (!abilityMod_SorceressHealingLaser) ? m_selfHealAmount : abilityMod_SorceressHealingLaser.m_selfHealMod.GetModifiedValue(m_selfHealAmount));
		AddTokenInt(tokens, "AllyHealAmount", string.Empty, (!abilityMod_SorceressHealingLaser) ? m_allyHealAmount : abilityMod_SorceressHealingLaser.m_allyHealMod.GetModifiedValue(m_allyHealAmount));
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_SorceressHealingLaser)
		{
			val4 = abilityMod_SorceressHealingLaser.m_minHealMod.GetModifiedValue(m_minHealAmount);
		}
		else
		{
			val4 = m_minHealAmount;
		}
		AddTokenInt(tokens, "MinHealAmount", empty4, val4);
		AddTokenInt(tokens, "HealChangePerHit", string.Empty, (!abilityMod_SorceressHealingLaser) ? m_healChangePerHit : abilityMod_SorceressHealingLaser.m_healChangePerHitMod.GetModifiedValue(m_healChangePerHit));
	}

	public int GetHealAmountByHitOrder(int hitOrder)
	{
		int num = ModdedBaseHealOnAlly();
		num += ModdedHealChangePerHit() * hitOrder;
		return Mathf.Max(ModdedMinHealAmountOnAlly(), num);
	}

	public int GetDamageAmountByHitOrder(int hitOrder)
	{
		int num = ModdedBaseDamage();
		num += ModdedDamageChangePerHit() * hitOrder;
		return Mathf.Max(ModdedMinDamage(), num);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SorceressHealingLaser))
		{
			m_abilityMod = (abilityMod as AbilityMod_SorceressHealingLaser);
			SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public int ModdedBaseHealOnSelf()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_selfHealAmount;
		}
		else
		{
			result = m_abilityMod.m_selfHealMod.GetModifiedValue(m_selfHealAmount);
		}
		return result;
	}

	public int ModdedBaseHealOnAlly()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_allyHealMod.GetModifiedValue(m_allyHealAmount) : m_allyHealAmount;
	}

	public int ModdedMinHealAmountOnAlly()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_minHealMod.GetModifiedValue(m_minHealAmount) : m_minHealAmount;
	}

	public int ModdedHealChangePerHit()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_healChangePerHitMod.GetModifiedValue(m_healChangePerHit) : m_healChangePerHit;
	}

	public int ModdedBaseDamage()
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

	public int ModdedMinDamage()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_minDamageMod.GetModifiedValue(m_minDamageAmount) : m_minDamageAmount;
	}

	public int ModdedDamageChangePerHit()
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

	public float ModdedLaserWidth()
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

	public float ModdedLaserRange()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_distance) : m_distance;
	}

	public int ModdedAllyTechPointGain()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_allyTechPointGain.GetModifiedValue(0) : 0;
	}
}
