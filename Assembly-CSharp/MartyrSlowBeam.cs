using System;
using System.Collections.Generic;

public class MartyrSlowBeam : MartyrLaserBase
{
	public StandardEffectInfo m_laserHitEffect;

	public int m_baseDamage = 0xF;

	public int m_additionalDamagePerCrystalSpent;

	public List<MartyrBasicAttackThreshold> m_thresholdBasedCrystalBonuses;

	public bool m_penetrateLoS;

	public float m_targetingRadius = 2.5f;

	private Martyr_SyncComponent m_syncComponent;

	private StandardEffectInfo m_cachedLaserHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Martyr Slow Beam";
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
		base.Targeter = new AbilityUtil_Targeter_MartyrSmoothAoE(this, this.GetCurrentTargetingRadius(), this.GetPenetrateLoS(), true, false, -1);
		base.Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		this.m_cachedLaserHitEffect = this.m_laserHitEffect;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedLaserHitEffect != null)
		{
			result = this.m_cachedLaserHitEffect;
		}
		else
		{
			result = this.m_laserHitEffect;
		}
		return result;
	}

	public int GetBaseDamage()
	{
		return this.m_baseDamage;
	}

	public int GetBonusDamagePerCrystalSpent()
	{
		return this.m_additionalDamagePerCrystalSpent;
	}

	public bool GetPenetrateLoS()
	{
		return this.m_penetrateLoS;
	}

	public float GetCurrentTargetingRadius()
	{
		MartyrLaserThreshold currentPowerEntry = base.GetCurrentPowerEntry(base.ActorData);
		float num;
		if (currentPowerEntry != null)
		{
			num = currentPowerEntry.m_additionalWidth;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		return this.m_targetingRadius + (float)this.m_syncComponent.SpentDamageCrystals(base.ActorData) * base.GetBonusWidthPerCrystalSpent() + num2;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_laserHitEffect, "LaserHitEffect", this.m_laserHitEffect, true);
		tokens.Add(new TooltipTokenInt("BaseDamage", "Damage with no crystal bonus", this.GetBaseDamage()));
		tokens.Add(new TooltipTokenInt("DamagePerCrystal", "Damage added per crystal spent", this.GetBonusDamagePerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("RadiusPerCrystal", "Radius increase per crystal spent", base.GetBonusWidthPerCrystalSpent()));
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		using (List<MartyrBasicAttackThreshold>.Enumerator enumerator = this.m_thresholdBasedCrystalBonuses.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MartyrBasicAttackThreshold item = enumerator.Current;
				list.Add(item);
			}
		}
		return list;
	}

	private int GetCurrentDamage(ActorData caster)
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = base.GetCurrentPowerEntry(caster) as MartyrBasicAttackThreshold;
		int num;
		if (martyrBasicAttackThreshold != null)
		{
			num = martyrBasicAttackThreshold.m_additionalDamage;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return this.GetBaseDamage() + this.m_syncComponent.SpentDamageCrystals(caster) * this.GetBonusDamagePerCrystalSpent() + num2;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetBaseDamage());
		this.m_laserHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		Ability.AddNameplateValueForSingleHit(ref result, base.Targeter, targetActor, this.GetCurrentDamage(base.ActorData), AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
		return result;
	}
}
