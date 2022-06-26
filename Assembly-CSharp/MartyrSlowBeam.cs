using System.Collections.Generic;

public class MartyrSlowBeam : MartyrLaserBase
{
	public StandardEffectInfo m_laserHitEffect;
	public int m_baseDamage = 15;
	public int m_additionalDamagePerCrystalSpent;
	public List<MartyrBasicAttackThreshold> m_thresholdBasedCrystalBonuses;
	public bool m_penetrateLoS;
	public float m_targetingRadius = 2.5f;

	private Martyr_SyncComponent m_syncComponent;
	private StandardEffectInfo m_cachedLaserHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Martyr Slow Beam";
		}
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		SetCachedFields();
		SetupTargeter();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return m_syncComponent;
	}

	protected void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_MartyrSmoothAoE(this, GetCurrentTargetingRadius(), GetPenetrateLoS())
		{
			ShowArcToShape = false
		};
	}

	private void SetCachedFields()
	{
		m_cachedLaserHitEffect = m_laserHitEffect;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		return m_cachedLaserHitEffect ?? m_laserHitEffect;
	}

	public int GetBaseDamage()
	{
		return m_baseDamage;
	}

	public int GetBonusDamagePerCrystalSpent()
	{
		return m_additionalDamagePerCrystalSpent;
	}

	public bool GetPenetrateLoS()
	{
		return m_penetrateLoS;
	}

	public float GetCurrentTargetingRadius()
	{
		MartyrLaserThreshold currentPowerEntry = GetCurrentPowerEntry(ActorData);
		float additionalWidth = currentPowerEntry != null ? currentPowerEntry.m_additionalWidth : 0f;
		return m_targetingRadius
			+ m_syncComponent.SpentDamageCrystals(ActorData) * GetBonusWidthPerCrystalSpent()
			+ additionalWidth;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffect, "LaserHitEffect", m_laserHitEffect);
		tokens.Add(new TooltipTokenInt("BaseDamage", "Damage with no crystal bonus", GetBaseDamage()));
		tokens.Add(new TooltipTokenInt("DamagePerCrystal", "Damage added per crystal spent", GetBonusDamagePerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("RadiusPerCrystal", "Radius increase per crystal spent", GetBonusWidthPerCrystalSpent()));
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		foreach (MartyrBasicAttackThreshold bonus in m_thresholdBasedCrystalBonuses)
		{
			list.Add(bonus);
		}
		return list;
	}

	private int GetCurrentDamage(ActorData caster)
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = GetCurrentPowerEntry(caster) as MartyrBasicAttackThreshold;
		int additionalDamage = martyrBasicAttackThreshold != null ? martyrBasicAttackThreshold.m_additionalDamage : 0;
		return GetBaseDamage()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetBonusDamagePerCrystalSpent()
			+ additionalDamage;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetBaseDamage());
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, GetCurrentDamage(ActorData));
		return symbolToValue;
	}
}
