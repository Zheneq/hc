using System;
using System.Collections.Generic;
using UnityEngine;

public class MantaRegeneration : Ability
{
	[Header("-- Healing --")]
	public int m_maxRegeneration = 0x1F4;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Regeneration";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.SetShowArcToShape(false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetOtherSelfEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		return result;
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedHealEffectData;
		if (this.m_abilityMod)
		{
			cachedHealEffectData = this.m_abilityMod.m_healEffectDataMod.GetModifiedValue(this.m_healEffectData);
		}
		else
		{
			cachedHealEffectData = this.m_healEffectData;
		}
		this.m_cachedHealEffectData = cachedHealEffectData;
		StandardEffectInfo cachedOtherSelfEffect;
		if (this.m_abilityMod)
		{
			cachedOtherSelfEffect = this.m_abilityMod.m_otherSelfEffectMod.GetModifiedValue(this.m_otherSelfEffect);
		}
		else
		{
			cachedOtherSelfEffect = this.m_otherSelfEffect;
		}
		this.m_cachedOtherSelfEffect = cachedOtherSelfEffect;
	}

	public int GetMaxRegeneration()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxRegenerationMod.GetModifiedValue(this.m_maxRegeneration);
		}
		else
		{
			result = this.m_maxRegeneration;
		}
		return result;
	}

	public int GetTurnsOfRegeneration()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_turnsOfRegenerationMod.GetModifiedValue(this.m_turnsOfRegeneration);
		}
		else
		{
			result = this.m_turnsOfRegeneration;
		}
		return result;
	}

	public float GetDamageToHealRatio()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageToHealRatioMod.GetModifiedValue(this.m_damageToHealRatio);
		}
		else
		{
			result = this.m_damageToHealRatio;
		}
		return result;
	}

	public int GetTechPointGainPerHit()
	{
		return (!this.m_abilityMod) ? this.m_techPointGainPerIncomingHit : this.m_abilityMod.m_techPointGainPerIncomingHit.GetModifiedValue(this.m_techPointGainPerIncomingHit);
	}

	public AbilityModCooldownReduction GetCooldownReductionOnNoDamage()
	{
		AbilityModCooldownReduction result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_cooldownReductionsWhenNoHits;
		}
		else
		{
			result = null;
		}
		return result;
	}

	public StandardActorEffectData GetHealEffectData()
	{
		return (this.m_cachedHealEffectData == null) ? this.m_healEffectData : this.m_cachedHealEffectData;
	}

	public StandardEffectInfo GetOtherSelfEffect()
	{
		return (this.m_cachedOtherSelfEffect == null) ? this.m_otherSelfEffect : this.m_cachedOtherSelfEffect;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MantaRegeneration))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_MantaRegeneration);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxRegeneration", string.Empty, this.m_maxRegeneration, false);
		base.AddTokenInt(tokens, "TurnsOfRegeneration", string.Empty, this.m_turnsOfRegeneration, false);
		base.AddTokenFloatAsPct(tokens, "DamageToHealRatio", string.Empty, this.m_damageToHealRatio, false);
		this.m_healEffectData.AddTooltipTokens(tokens, "HealEffectData", false, null);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_otherSelfEffect, "OtherSelfEffect", this.m_otherSelfEffect, true);
	}
}
