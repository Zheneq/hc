using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class MantaOutwardLasers : Ability
{
	[Header("-- Targeting")]
	public int m_numLasers = 5;

	public float m_totalAngleForLaserFan = 288f;

	public float m_width = 1f;

	public float m_maxDistancePerBounce = 15f;

	public float m_maxTotalDistance = 50f;

	public int m_maxBounces = 1;

	public int m_maxTargetsHit = 1;

	[Header("-- Damage")]
	public int m_damageAmount = 20;

	public int m_damageAmountForAdditionalHits = 10;

	public int m_bonusDamagePerBounce;

	public int m_techPointGainPerLaserHit;

	public StandardEffectInfo m_effectOnEnemy;

	public StandardEffectInfo m_effectForMultiHitsOnEnemy;

	[Tooltip("For when we want to apply 2 statuses that have different durations")]
	public StandardEffectInfo m_additionalEffectForMultiHitsOnEnemy;

	[Header("-- Sequences")]
	public GameObject m_projectileSequence;

	private StandardEffectInfo m_cachedEffectData;

	private StandardEffectInfo m_cachedMultiHitEffectData;

	private StandardEffectInfo m_cachedAdditionalMultiHitEffectData;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Fissure Nova";
		}
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		m_cachedEffectData = m_effectOnEnemy;
		m_cachedMultiHitEffectData = m_effectForMultiHitsOnEnemy;
		m_cachedAdditionalMultiHitEffectData = m_additionalEffectForMultiHitsOnEnemy;
	}

	public float GetFanAngle()
	{
		return m_totalAngleForLaserFan;
	}

	public int GetLaserCount()
	{
		return m_numLasers;
	}

	public int GetMaxBounces()
	{
		return m_maxBounces;
	}

	public int GetMaxTargetHits()
	{
		return m_maxTargetsHit;
	}

	public float GetLaserWidth()
	{
		return m_width;
	}

	public float GetDistancePerBounce()
	{
		return m_maxDistancePerBounce;
	}

	public float GetMaxTotalDistance()
	{
		return m_maxTotalDistance;
	}

	private StandardEffectInfo GetEnemyEffectData()
	{
		StandardEffectInfo result;
		if (m_cachedEffectData == null)
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
			result = m_effectOnEnemy;
		}
		else
		{
			result = m_cachedEffectData;
		}
		return result;
	}

	private StandardEffectInfo GetMultiHitEnemyEffectData()
	{
		StandardEffectInfo result;
		if (m_cachedMultiHitEffectData == null)
		{
			while (true)
			{
				switch (5)
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
			result = m_effectForMultiHitsOnEnemy;
		}
		else
		{
			result = m_cachedMultiHitEffectData;
		}
		return result;
	}

	private StandardEffectInfo GetAdditionalMultiHitEnemyEffectData()
	{
		return (m_cachedAdditionalMultiHitEffectData != null) ? m_cachedAdditionalMultiHitEffectData : m_additionalEffectForMultiHitsOnEnemy;
	}

	public int GetBaseDamage()
	{
		return m_damageAmount;
	}

	public int GetDamageForAdditionalHit()
	{
		return m_damageAmountForAdditionalHits;
	}

	public int GetBonusDamagePerBounce()
	{
		return m_bonusDamagePerBounce;
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_FanOfBouncingLasers(this, GetFanAngle(), GetDistancePerBounce(), GetMaxTotalDistance(), GetLaserWidth(), GetMaxBounces(), GetMaxTargetHits(), GetLaserCount());
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetBaseDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		ReadOnlyCollection<AbilityUtil_Targeter_FanOfBouncingLasers.HitActorContext> hitActorContext = (base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_FanOfBouncingLasers).GetHitActorContext();
		for (int i = 0; i < hitActorContext.Count; i++)
		{
			AbilityUtil_Targeter_FanOfBouncingLasers.HitActorContext hitActorContext2 = hitActorContext[i];
			if (!(hitActorContext2.actor == targetActor))
			{
				continue;
			}
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
			int bonusDamagePerBounce = GetBonusDamagePerBounce();
			AbilityUtil_Targeter_FanOfBouncingLasers.HitActorContext hitActorContext3 = hitActorContext[i];
			int num = bonusDamagePerBounce * hitActorContext3.segmentIndex;
			int value = GetBaseDamage() + num;
			int num2 = GetDamageForAdditionalHit() + num;
			if (dictionary.ContainsKey(AbilityTooltipSymbol.Damage))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				dictionary[AbilityTooltipSymbol.Damage] += num2;
			}
			else
			{
				dictionary[AbilityTooltipSymbol.Damage] = value;
			}
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			return dictionary;
		}
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (m_techPointGainPerLaserHit > 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					int tooltipSubjectCountTotalWithDuplicates = base.Targeter.GetTooltipSubjectCountTotalWithDuplicates(AbilityTooltipSubject.Primary);
					return m_techPointGainPerLaserHit * tooltipSubjectCountTotalWithDuplicates;
				}
				}
			}
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		string empty = string.Empty;
		int val;
		if ((bool)modAsBase)
		{
			while (true)
			{
				switch (5)
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
			val = 0;
		}
		else
		{
			val = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)modAsBase)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = 0;
		}
		else
		{
			val2 = m_damageAmountForAdditionalHits;
		}
		AddTokenInt(tokens, "DamageAdditionalHit", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)modAsBase)
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
			val3 = 0;
		}
		else
		{
			val3 = m_bonusDamagePerBounce;
		}
		AddTokenInt(tokens, "BonusDamagePerBounce", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)modAsBase)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			val4 = 0;
		}
		else
		{
			val4 = m_numLasers;
		}
		AddTokenInt(tokens, "NumLasers", empty4, val4);
		string empty5 = string.Empty;
		int val5;
		if ((bool)modAsBase)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			val5 = 0;
		}
		else
		{
			val5 = m_maxBounces;
		}
		AddTokenInt(tokens, "MaxBounces", empty5, val5);
		string empty6 = string.Empty;
		int val6;
		if ((bool)modAsBase)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			val6 = 0;
		}
		else
		{
			val6 = m_maxTargetsHit;
		}
		AddTokenInt(tokens, "MaxTargetsHit", empty6, val6);
	}
}
