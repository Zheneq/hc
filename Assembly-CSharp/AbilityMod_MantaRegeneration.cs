using System;
using System.Collections.Generic;
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
			AbilityMod.AddToken(tokens, m_maxRegenerationMod, "MaxRegeneration", string.Empty, mantaRegeneration.m_maxRegeneration);
			AbilityMod.AddToken(tokens, m_turnsOfRegenerationMod, "TurnsOfRegeneration", string.Empty, mantaRegeneration.m_turnsOfRegeneration);
			AbilityMod.AddToken(tokens, m_damageToHealRatioMod, "DamageToHealRatio", string.Empty, mantaRegeneration.m_damageToHealRatio);
			AbilityMod.AddToken(tokens, m_techPointGainPerIncomingHit, "EnergyPerHit", string.Empty, mantaRegeneration.m_techPointGainPerIncomingHit);
			AbilityMod.AddToken_EffectMod(tokens, m_healEffectDataMod, "HealEffectData", mantaRegeneration.m_healEffectData);
			AbilityMod.AddToken_EffectMod(tokens, m_otherSelfEffectMod, "OtherSelfEffect", mantaRegeneration.m_otherSelfEffect);
			m_cooldownReductionsWhenNoHits.AddTooltipTokens(tokens, "CooldownReductionOnNoDamage");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MantaRegeneration mantaRegeneration = GetTargetAbilityOnAbilityData(abilityData) as MantaRegeneration;
		bool flag = mantaRegeneration != null;
		string empty = string.Empty;
		empty += PropDesc(m_maxRegenerationMod, "[MaxRegeneration]", flag, flag ? mantaRegeneration.m_maxRegeneration : 0);
		empty += PropDesc(m_turnsOfRegenerationMod, "[TurnsOfRegeneration]", flag, flag ? mantaRegeneration.m_turnsOfRegeneration : 0);
		empty += PropDesc(m_damageToHealRatioMod, "[DamageToHealRatio]", flag, (!flag) ? 0f : mantaRegeneration.m_damageToHealRatio);
		empty += PropDesc(m_techPointGainPerIncomingHit, "[EnergyPerHit]", flag, flag ? mantaRegeneration.m_techPointGainPerIncomingHit : 0);
		string str = empty;
		AbilityModPropertyEffectData healEffectDataMod = m_healEffectDataMod;
		object baseVal;
		if (flag)
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
			baseVal = mantaRegeneration.m_healEffectData;
		}
		else
		{
			baseVal = null;
		}
		empty = str + PropDesc(healEffectDataMod, "[HealEffectData]", flag, (StandardActorEffectData)baseVal);
		string str2 = empty;
		AbilityModPropertyEffectInfo otherSelfEffectMod = m_otherSelfEffectMod;
		object baseVal2;
		if (flag)
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
			baseVal2 = mantaRegeneration.m_otherSelfEffect;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str2 + PropDesc(otherSelfEffectMod, "[OtherSelfEffect]", flag, (StandardEffectInfo)baseVal2);
		return empty + m_cooldownReductionsWhenNoHits.GetDescription(abilityData);
	}
}
