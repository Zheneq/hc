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
		base.Targeter = new AbilityUtil_Targeter_MartyrSmoothAoE(this, GetCurrentTargetingRadius(), GetPenetrateLoS());
		base.Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		m_cachedLaserHitEffect = m_laserHitEffect;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedLaserHitEffect != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedLaserHitEffect;
		}
		else
		{
			result = m_laserHitEffect;
		}
		return result;
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
		MartyrLaserThreshold currentPowerEntry = GetCurrentPowerEntry(base.ActorData);
		float num;
		if (currentPowerEntry != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = currentPowerEntry.m_additionalWidth;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		return m_targetingRadius + (float)m_syncComponent.SpentDamageCrystals(base.ActorData) * GetBonusWidthPerCrystalSpent() + num2;
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
		using (List<MartyrBasicAttackThreshold>.Enumerator enumerator = m_thresholdBasedCrystalBonuses.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MartyrBasicAttackThreshold current = enumerator.Current;
				list.Add(current);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return list;
					}
					/*OpCode not supported: LdMemberToken*/;
					return list;
				}
			}
		}
	}

	private int GetCurrentDamage(ActorData caster)
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = GetCurrentPowerEntry(caster) as MartyrBasicAttackThreshold;
		int num;
		if (martyrBasicAttackThreshold != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = martyrBasicAttackThreshold.m_additionalDamage;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		return GetBaseDamage() + m_syncComponent.SpentDamageCrystals(caster) * GetBonusDamagePerCrystalSpent() + num2;
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
		Ability.AddNameplateValueForSingleHit(ref symbolToValue, base.Targeter, targetActor, GetCurrentDamage(base.ActorData));
		return symbolToValue;
	}
}
