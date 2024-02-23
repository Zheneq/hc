using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_MantaRegeneration : AbilityMod
{
	[Header("-- Healing --")]
	public AbilityModPropertyInt m_maxRegenerationMod;
	public AbilityModPropertyInt m_turnsOfRegenerationMod;
	public AbilityModPropertyFloat m_damageToHealRatioMod;
	public AbilityModPropertyInt m_techPointGainPerIncomingHit;
	public AbilityModCooldownReduction m_cooldownReductionsWhenNoHits;
	[Header("  (( base effect data for healing, no need to specify healing here ))")]
	public AbilityModPropertyEffectData m_healEffectDataMod;
	public AbilityModPropertyEffectInfo m_otherSelfEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(MantaRegeneration);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MantaRegeneration mantaRegeneration = targetAbility as MantaRegeneration;
		if (mantaRegeneration != null)
		{
			AddToken(tokens, m_maxRegenerationMod, "MaxRegeneration", string.Empty, mantaRegeneration.m_maxRegeneration);
			AddToken(tokens, m_turnsOfRegenerationMod, "TurnsOfRegeneration", string.Empty, mantaRegeneration.m_turnsOfRegeneration);
			AddToken(tokens, m_damageToHealRatioMod, "DamageToHealRatio", string.Empty, mantaRegeneration.m_damageToHealRatio);
			AddToken(tokens, m_techPointGainPerIncomingHit, "EnergyPerHit", string.Empty, mantaRegeneration.m_techPointGainPerIncomingHit);
			AddToken_EffectMod(tokens, m_healEffectDataMod, "HealEffectData", mantaRegeneration.m_healEffectData);
			AddToken_EffectMod(tokens, m_otherSelfEffectMod, "OtherSelfEffect", mantaRegeneration.m_otherSelfEffect);
			m_cooldownReductionsWhenNoHits.AddTooltipTokens(tokens, "CooldownReductionOnNoDamage");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MantaRegeneration mantaRegeneration = GetTargetAbilityOnAbilityData(abilityData) as MantaRegeneration;
		bool isValid = mantaRegeneration != null;
		string desc = string.Empty;
		desc += PropDesc(m_maxRegenerationMod, "[MaxRegeneration]", isValid, isValid ? mantaRegeneration.m_maxRegeneration : 0);
		desc += PropDesc(m_turnsOfRegenerationMod, "[TurnsOfRegeneration]", isValid, isValid ? mantaRegeneration.m_turnsOfRegeneration : 0);
		desc += PropDesc(m_damageToHealRatioMod, "[DamageToHealRatio]", isValid, isValid ? mantaRegeneration.m_damageToHealRatio : 0f);
		desc += PropDesc(m_techPointGainPerIncomingHit, "[EnergyPerHit]", isValid, isValid ? mantaRegeneration.m_techPointGainPerIncomingHit : 0);
		desc += PropDesc(m_healEffectDataMod, "[HealEffectData]", isValid, isValid ? mantaRegeneration.m_healEffectData : null);
		desc += PropDesc(m_otherSelfEffectMod, "[OtherSelfEffect]", isValid, isValid ? mantaRegeneration.m_otherSelfEffect : null);
		return new StringBuilder().Append(desc).Append(m_cooldownReductionsWhenNoHits.GetDescription(abilityData)).ToString();
	}
}
