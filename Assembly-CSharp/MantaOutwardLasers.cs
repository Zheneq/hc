using System;
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
	public int m_damageAmount = 0x14;

	public int m_damageAmountForAdditionalHits = 0xA;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Fissure Nova";
		}
		this.SetupTargeter();
	}

	private void SetCachedFields()
	{
		this.m_cachedEffectData = this.m_effectOnEnemy;
		this.m_cachedMultiHitEffectData = this.m_effectForMultiHitsOnEnemy;
		this.m_cachedAdditionalMultiHitEffectData = this.m_additionalEffectForMultiHitsOnEnemy;
	}

	public float GetFanAngle()
	{
		return this.m_totalAngleForLaserFan;
	}

	public int GetLaserCount()
	{
		return this.m_numLasers;
	}

	public int GetMaxBounces()
	{
		return this.m_maxBounces;
	}

	public int GetMaxTargetHits()
	{
		return this.m_maxTargetsHit;
	}

	public float GetLaserWidth()
	{
		return this.m_width;
	}

	public float GetDistancePerBounce()
	{
		return this.m_maxDistancePerBounce;
	}

	public float GetMaxTotalDistance()
	{
		return this.m_maxTotalDistance;
	}

	private StandardEffectInfo GetEnemyEffectData()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectData == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaOutwardLasers.GetEnemyEffectData()).MethodHandle;
			}
			result = this.m_effectOnEnemy;
		}
		else
		{
			result = this.m_cachedEffectData;
		}
		return result;
	}

	private StandardEffectInfo GetMultiHitEnemyEffectData()
	{
		StandardEffectInfo result;
		if (this.m_cachedMultiHitEffectData == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaOutwardLasers.GetMultiHitEnemyEffectData()).MethodHandle;
			}
			result = this.m_effectForMultiHitsOnEnemy;
		}
		else
		{
			result = this.m_cachedMultiHitEffectData;
		}
		return result;
	}

	private StandardEffectInfo GetAdditionalMultiHitEnemyEffectData()
	{
		return (this.m_cachedAdditionalMultiHitEffectData != null) ? this.m_cachedAdditionalMultiHitEffectData : this.m_additionalEffectForMultiHitsOnEnemy;
	}

	public int GetBaseDamage()
	{
		return this.m_damageAmount;
	}

	public int GetDamageForAdditionalHit()
	{
		return this.m_damageAmountForAdditionalHits;
	}

	public int GetBonusDamagePerBounce()
	{
		return this.m_bonusDamagePerBounce;
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_FanOfBouncingLasers(this, this.GetFanAngle(), this.GetDistancePerBounce(), this.GetMaxTotalDistance(), this.GetLaserWidth(), this.GetMaxBounces(), this.GetMaxTargetHits(), this.GetLaserCount());
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetBaseDamage());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		ReadOnlyCollection<AbilityUtil_Targeter_FanOfBouncingLasers.HitActorContext> hitActorContext = (base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_FanOfBouncingLasers).GetHitActorContext();
		for (int i = 0; i < hitActorContext.Count; i++)
		{
			if (hitActorContext[i].actor == targetActor)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(MantaOutwardLasers.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
				}
				int num = this.GetBonusDamagePerBounce() * hitActorContext[i].segmentIndex;
				int value = this.GetBaseDamage() + num;
				int num2 = this.GetDamageForAdditionalHit() + num;
				if (dictionary.ContainsKey(AbilityTooltipSymbol.Damage))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					Dictionary<AbilityTooltipSymbol, int> dictionary2;
					(dictionary2 = dictionary)[AbilityTooltipSymbol.Damage] = dictionary2[AbilityTooltipSymbol.Damage] + num2;
				}
				else
				{
					dictionary[AbilityTooltipSymbol.Damage] = value;
				}
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.m_techPointGainPerLaserHit > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaOutwardLasers.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			int tooltipSubjectCountTotalWithDuplicates = base.Targeter.GetTooltipSubjectCountTotalWithDuplicates(AbilityTooltipSubject.Primary);
			return this.m_techPointGainPerLaserHit * tooltipSubjectCountTotalWithDuplicates;
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		string name = "DamageAmount";
		string empty = string.Empty;
		int val;
		if (modAsBase)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaOutwardLasers.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = 0;
		}
		else
		{
			val = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "DamageAdditionalHit";
		string empty2 = string.Empty;
		int val2;
		if (modAsBase)
		{
			for (;;)
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
			val2 = this.m_damageAmountForAdditionalHits;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "BonusDamagePerBounce";
		string empty3 = string.Empty;
		int val3;
		if (modAsBase)
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
			val3 = 0;
		}
		else
		{
			val3 = this.m_bonusDamagePerBounce;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "NumLasers";
		string empty4 = string.Empty;
		int val4;
		if (modAsBase)
		{
			for (;;)
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
			val4 = this.m_numLasers;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		string name5 = "MaxBounces";
		string empty5 = string.Empty;
		int val5;
		if (modAsBase)
		{
			for (;;)
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
			val5 = this.m_maxBounces;
		}
		base.AddTokenInt(tokens, name5, empty5, val5, false);
		string name6 = "MaxTargetsHit";
		string empty6 = string.Empty;
		int val6;
		if (modAsBase)
		{
			for (;;)
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
			val6 = this.m_maxTargetsHit;
		}
		base.AddTokenInt(tokens, name6, empty6, val6, false);
	}
}
