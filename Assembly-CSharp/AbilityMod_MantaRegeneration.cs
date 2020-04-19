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
			AbilityMod.AddToken(tokens, this.m_maxRegenerationMod, "MaxRegeneration", string.Empty, mantaRegeneration.m_maxRegeneration, true, false);
			AbilityMod.AddToken(tokens, this.m_turnsOfRegenerationMod, "TurnsOfRegeneration", string.Empty, mantaRegeneration.m_turnsOfRegeneration, true, false);
			AbilityMod.AddToken(tokens, this.m_damageToHealRatioMod, "DamageToHealRatio", string.Empty, mantaRegeneration.m_damageToHealRatio, true, false, false);
			AbilityMod.AddToken(tokens, this.m_techPointGainPerIncomingHit, "EnergyPerHit", string.Empty, mantaRegeneration.m_techPointGainPerIncomingHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_healEffectDataMod, "HealEffectData", mantaRegeneration.m_healEffectData, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_otherSelfEffectMod, "OtherSelfEffect", mantaRegeneration.m_otherSelfEffect, true);
			this.m_cooldownReductionsWhenNoHits.AddTooltipTokens(tokens, "CooldownReductionOnNoDamage");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MantaRegeneration mantaRegeneration = base.GetTargetAbilityOnAbilityData(abilityData) as MantaRegeneration;
		bool flag = mantaRegeneration != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_maxRegenerationMod, "[MaxRegeneration]", flag, (!flag) ? 0 : mantaRegeneration.m_maxRegeneration);
		text += base.PropDesc(this.m_turnsOfRegenerationMod, "[TurnsOfRegeneration]", flag, (!flag) ? 0 : mantaRegeneration.m_turnsOfRegeneration);
		text += base.PropDesc(this.m_damageToHealRatioMod, "[DamageToHealRatio]", flag, (!flag) ? 0f : mantaRegeneration.m_damageToHealRatio);
		text += base.PropDesc(this.m_techPointGainPerIncomingHit, "[EnergyPerHit]", flag, (!flag) ? 0 : mantaRegeneration.m_techPointGainPerIncomingHit);
		string str = text;
		AbilityModPropertyEffectData healEffectDataMod = this.m_healEffectDataMod;
		string prefix = "[HealEffectData]";
		bool showBaseVal = flag;
		StandardActorEffectData baseVal;
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_MantaRegeneration.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = mantaRegeneration.m_healEffectData;
		}
		else
		{
			baseVal = null;
		}
		text = str + base.PropDesc(healEffectDataMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyEffectInfo otherSelfEffectMod = this.m_otherSelfEffectMod;
		string prefix2 = "[OtherSelfEffect]";
		bool showBaseVal2 = flag;
		StandardEffectInfo baseVal2;
		if (flag)
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
			baseVal2 = mantaRegeneration.m_otherSelfEffect;
		}
		else
		{
			baseVal2 = null;
		}
		text = str2 + base.PropDesc(otherSelfEffectMod, prefix2, showBaseVal2, baseVal2);
		return text + this.m_cooldownReductionsWhenNoHits.GetDescription(abilityData);
	}
}
