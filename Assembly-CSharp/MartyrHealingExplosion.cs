using System;
using System.Collections.Generic;
using UnityEngine;

public class MartyrHealingExplosion : MartyrLaserBase
{
	[Header("-- Targeting")]
	public LaserTargetingInfo m_laserInfo;

	public StandardEffectInfo m_laserHitEffect;

	public float m_explosionRadius = 2.5f;

	public bool m_laserCanHitAllies;

	public bool m_laserCanHitEnemies = true;

	public bool m_forceMaxLaserDistance = true;

	public bool m_explodeOnlyOnLaserHit = true;

	public bool m_explosionCanHitCaster;

	[Header("-- Damage, Healing & Crystal Bonuses")]
	public int m_baseLaserDamage = 0x14;

	public int m_baseExplosionHealing = 0xF;

	public int m_additionalDamagePerCrystalSpent;

	public int m_additionalHealingPerCrystalSpent;

	public float m_additionalRadiusPerCrystalSpent = 0.25f;

	public List<MartyrBasicAttackThreshold> m_thresholdBasedCrystalBonuses;

	[Header("-- Sequences")]
	public GameObject m_projectileSequence;

	private Martyr_SyncComponent m_syncComponent;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardEffectInfo m_cachedLaserHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrHealingExplosion.Start()).MethodHandle;
			}
			this.m_abilityName = "Martyr Healing Explosion";
		}
		this.m_syncComponent = base.GetComponent<Martyr_SyncComponent>();
		this.SetCachedFields();
		this.SetupTargeter();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return this.m_syncComponent;
	}

	protected void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_MartyrLaser(this, base.GetCurrentLaserWidth(), base.GetCurrentLaserRange(), base.GetCurrentLaserPenetrateLoS(), base.GetCurrentLaserMaxTargets(), this.m_laserCanHitEnemies, this.m_laserCanHitAllies, false, !this.m_forceMaxLaserDistance, this.m_explodeOnlyOnLaserHit, this.GetCurrentExplosionRadius(), this.GetCurrentInnerExplosionRadius(), true, false, this.m_explosionCanHitCaster)
		{
			m_delegateLaserWidth = new AbilityUtil_Targeter_MartyrLaser.CustomFloatValueDelegate(base.GetCurrentLaserWidth),
			m_delegateLaserRange = new AbilityUtil_Targeter_MartyrLaser.CustomFloatValueDelegate(base.GetCurrentLaserRange),
			m_delegatePenetrateLos = new AbilityUtil_Targeter_MartyrLaser.CustomBoolValueDelegate(base.GetCurrentLaserPenetrateLoS),
			m_delegateMaxTargets = new AbilityUtil_Targeter_MartyrLaser.CustomIntValueDelegate(base.GetCurrentLaserMaxTargets),
			m_delegateConeRadius = new AbilityUtil_Targeter_MartyrLaser.CustomFloatValueDelegate(this.GetCurrentExplosionRadius),
			m_delegateInnerConeRadius = new AbilityUtil_Targeter_MartyrLaser.CustomFloatValueDelegate(this.GetCurrentInnerExplosionRadius)
		};
	}

	private void SetCachedFields()
	{
		this.m_cachedLaserInfo = this.m_laserInfo;
		this.m_cachedLaserHitEffect = this.m_laserHitEffect;
	}

	public override LaserTargetingInfo GetLaserInfo()
	{
		return (this.m_cachedLaserInfo == null) ? this.m_laserInfo : this.m_cachedLaserInfo;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedLaserHitEffect != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrHealingExplosion.GetLaserHitEffect()).MethodHandle;
			}
			result = this.m_cachedLaserHitEffect;
		}
		else
		{
			result = this.m_laserHitEffect;
		}
		return result;
	}

	public float GetBaseExplosionRadius()
	{
		return this.m_explosionRadius;
	}

	public float GetBonusRadiusPerCrystalSpent()
	{
		return this.m_additionalRadiusPerCrystalSpent;
	}

	public int GetBaseDamage()
	{
		return this.m_baseLaserDamage;
	}

	public int GetBaseExplosionHealing()
	{
		return this.m_baseExplosionHealing;
	}

	public int GetBonusDamagePerCrystalSpent()
	{
		return this.m_additionalDamagePerCrystalSpent;
	}

	public int GetBonusHealingPerCrystalSpent()
	{
		return this.m_additionalHealingPerCrystalSpent;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_laserHitEffect, "LaserHitEffect", this.m_laserHitEffect, true);
		tokens.Add(new TooltipTokenInt("BaseLaserDamage", "Damage from laser hits with no crystal bonus", this.GetBaseDamage()));
		tokens.Add(new TooltipTokenInt("BaseExplosionHealing", "Healing from explosion hits with no crystal bonus", this.GetBaseExplosionHealing()));
		tokens.Add(new TooltipTokenInt("DamagePerCrystal", "Damage added per crystal spent", this.GetBonusDamagePerCrystalSpent()));
		tokens.Add(new TooltipTokenInt("HealingPerCrystal", "Healing added per crystal spent", this.GetBonusHealingPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("WidthPerCrystal", "Width added per crystal spent", base.GetBonusWidthPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("LengthPerCrystal", "Length added per crystal spent", base.GetBonusLengthPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("RadiusPerCrystal", "Explosion radius added per crystal spent", this.GetBonusRadiusPerCrystalSpent()));
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		foreach (MartyrBasicAttackThreshold item in this.m_thresholdBasedCrystalBonuses)
		{
			list.Add(item);
		}
		return list;
	}

	private int GetCurrentLaserDamage(ActorData caster)
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = base.GetCurrentPowerEntry(caster) as MartyrBasicAttackThreshold;
		int num = (martyrBasicAttackThreshold == null) ? 0 : martyrBasicAttackThreshold.m_additionalDamage;
		return this.GetBaseDamage() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetBonusDamagePerCrystalSpent() + num;
	}

	private int GetCurrentExplosionHealing(ActorData caster)
	{
		MartyrHealingExplosionThreshold martyrHealingExplosionThreshold = base.GetCurrentPowerEntry(caster) as MartyrHealingExplosionThreshold;
		int num;
		if (martyrHealingExplosionThreshold != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrHealingExplosion.GetCurrentExplosionHealing(ActorData)).MethodHandle;
			}
			num = martyrHealingExplosionThreshold.m_additionalHealing;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return this.GetBaseExplosionHealing() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetBonusHealingPerCrystalSpent() + num2;
	}

	public override float GetCurrentExplosionRadius()
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = base.GetCurrentPowerEntry(base.ActorData) as MartyrBasicAttackThreshold;
		float num;
		if (martyrBasicAttackThreshold != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrHealingExplosion.GetCurrentExplosionRadius()).MethodHandle;
			}
			num = martyrBasicAttackThreshold.m_additionalRadius;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		return this.GetBaseExplosionRadius() + (float)this.m_syncComponent.SpentDamageCrystals(base.ActorData) * this.GetBonusRadiusPerCrystalSpent() + num2;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetBaseDamage());
		this.m_laserHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Secondary, this.GetBaseExplosionHealing());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, this.GetCurrentLaserDamage(base.ActorData), AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
		Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, this.GetCurrentExplosionHealing(base.ActorData), AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Secondary);
		return result;
	}
}
