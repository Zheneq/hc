using System.Collections.Generic;
using UnityEngine;

public class MantaRegeneration : Ability
{
	[Header("-- Healing --")]
	public int m_maxRegeneration = 500;

	public int m_turnsOfRegeneration = 2;

	public float m_damageToHealRatio = 1f;

	public int m_techPointGainPerIncomingHit;

	public AbilityPriority m_healInPhase = AbilityPriority.Combat_Damage;

	[Header("  (( base effect data for healing, no need to specify healing here ))")]
	public StandardActorEffectData m_healEffectData;

	public StandardEffectInfo m_otherSelfEffect;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	public GameObject m_incomingHitImpactSequencePrefab;

	private AbilityMod_MantaRegeneration m_abilityMod;

	private StandardActorEffectData m_cachedHealEffectData;

	private StandardEffectInfo m_cachedOtherSelfEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Regeneration";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always);
		base.Targeter.SetShowArcToShape(false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetOtherSelfEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedHealEffectData;
		if ((bool)m_abilityMod)
		{
			cachedHealEffectData = m_abilityMod.m_healEffectDataMod.GetModifiedValue(m_healEffectData);
		}
		else
		{
			cachedHealEffectData = m_healEffectData;
		}
		m_cachedHealEffectData = cachedHealEffectData;
		StandardEffectInfo cachedOtherSelfEffect;
		if ((bool)m_abilityMod)
		{
			cachedOtherSelfEffect = m_abilityMod.m_otherSelfEffectMod.GetModifiedValue(m_otherSelfEffect);
		}
		else
		{
			cachedOtherSelfEffect = m_otherSelfEffect;
		}
		m_cachedOtherSelfEffect = cachedOtherSelfEffect;
	}

	public int GetMaxRegeneration()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxRegenerationMod.GetModifiedValue(m_maxRegeneration);
		}
		else
		{
			result = m_maxRegeneration;
		}
		return result;
	}

	public int GetTurnsOfRegeneration()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_turnsOfRegenerationMod.GetModifiedValue(m_turnsOfRegeneration);
		}
		else
		{
			result = m_turnsOfRegeneration;
		}
		return result;
	}

	public float GetDamageToHealRatio()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageToHealRatioMod.GetModifiedValue(m_damageToHealRatio);
		}
		else
		{
			result = m_damageToHealRatio;
		}
		return result;
	}

	public int GetTechPointGainPerHit()
	{
		return (!m_abilityMod) ? m_techPointGainPerIncomingHit : m_abilityMod.m_techPointGainPerIncomingHit.GetModifiedValue(m_techPointGainPerIncomingHit);
	}

	public AbilityModCooldownReduction GetCooldownReductionOnNoDamage()
	{
		object result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_cooldownReductionsWhenNoHits;
		}
		else
		{
			result = null;
		}
		return (AbilityModCooldownReduction)result;
	}

	public StandardActorEffectData GetHealEffectData()
	{
		return (m_cachedHealEffectData == null) ? m_healEffectData : m_cachedHealEffectData;
	}

	public StandardEffectInfo GetOtherSelfEffect()
	{
		return (m_cachedOtherSelfEffect == null) ? m_otherSelfEffect : m_cachedOtherSelfEffect;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_MantaRegeneration))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_MantaRegeneration);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxRegeneration", string.Empty, m_maxRegeneration);
		AddTokenInt(tokens, "TurnsOfRegeneration", string.Empty, m_turnsOfRegeneration);
		AddTokenFloatAsPct(tokens, "DamageToHealRatio", string.Empty, m_damageToHealRatio);
		m_healEffectData.AddTooltipTokens(tokens, "HealEffectData");
		AbilityMod.AddToken_EffectInfo(tokens, m_otherSelfEffect, "OtherSelfEffect", m_otherSelfEffect);
	}
}
