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
		return m_cachedEffectData ?? m_effectOnEnemy;
	}

	private StandardEffectInfo GetMultiHitEnemyEffectData()
	{
		return m_cachedMultiHitEffectData ?? m_effectForMultiHitsOnEnemy;
	}

	private StandardEffectInfo GetAdditionalMultiHitEnemyEffectData()
	{
		return m_cachedAdditionalMultiHitEffectData ?? m_additionalEffectForMultiHitsOnEnemy;
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
		Targeter = new AbilityUtil_Targeter_FanOfBouncingLasers(
			this,
			GetFanAngle(),
			GetDistancePerBounce(),
			GetMaxTotalDistance(),
			GetLaserWidth(),
			GetMaxBounces(),
			GetMaxTargetHits(),
			GetLaserCount());
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
		ReadOnlyCollection<AbilityUtil_Targeter_FanOfBouncingLasers.HitActorContext> hitActorContext =
			(Targeters[currentTargeterIndex] as AbilityUtil_Targeter_FanOfBouncingLasers).GetHitActorContext();
		foreach (AbilityUtil_Targeter_FanOfBouncingLasers.HitActorContext hit in hitActorContext)
		{
			if (hit.actor == targetActor)
			{
				int bonusDamage = GetBonusDamagePerBounce() * hit.segmentIndex;
				int firstHitDamage = GetBaseDamage() + bonusDamage;
				int additionalHitDamage = GetDamageForAdditionalHit() + bonusDamage;
				if (dictionary.ContainsKey(AbilityTooltipSymbol.Damage))
				{
					dictionary[AbilityTooltipSymbol.Damage] += additionalHitDamage;
				}
				else
				{
					dictionary[AbilityTooltipSymbol.Damage] = firstHitDamage;
				}
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		return m_techPointGainPerLaserHit > 0
			? m_techPointGainPerLaserHit * Targeter.GetTooltipSubjectCountTotalWithDuplicates(AbilityTooltipSubject.Primary)
			: 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmount", string.Empty, modAsBase != null ? 0 : m_damageAmount);
		AddTokenInt(tokens, "DamageAdditionalHit", string.Empty, modAsBase != null ? 0 : m_damageAmountForAdditionalHits);
		AddTokenInt(tokens, "BonusDamagePerBounce", string.Empty, modAsBase != null ? 0 : m_bonusDamagePerBounce);
		AddTokenInt(tokens, "NumLasers", string.Empty, modAsBase != null ? 0 : m_numLasers);
		AddTokenInt(tokens, "MaxBounces", string.Empty, modAsBase != null ? 0 : m_maxBounces);
		AddTokenInt(tokens, "MaxTargetsHit", string.Empty, modAsBase != null ? 0 : m_maxTargetsHit);
	}
}
