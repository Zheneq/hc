using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NanoSmithBlastShield : AbilityMod
{
	[Space(10f)]
	public AbilityModPropertyInt m_healOnEndIfHasRemainingAbsorbMod;

	public AbilityModPropertyInt m_energyGainOnShieldTargetMod;

	public AbilityModPropertyEffectData m_shieldEffectOverride;

	[Header("-- Extra Effect on Caster if targeting Ally")]
	public AbilityModPropertyEffectInfo m_extraEffectOnCasterIfTargetingAllyMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NanoSmithBlastShield);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NanoSmithBlastShield nanoSmithBlastShield = targetAbility as NanoSmithBlastShield;
		if (!(nanoSmithBlastShield != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_EffectMod(tokens, m_shieldEffectOverride, "ShieldEffect", nanoSmithBlastShield.m_shieldEffect);
			AbilityMod.AddToken(tokens, m_healOnEndIfHasRemainingAbsorbMod, "HealOnEndIfHasRemainingAbsorb", string.Empty, nanoSmithBlastShield.m_healOnEndIfHasRemainingAbsorb);
			AbilityMod.AddToken(tokens, m_energyGainOnShieldTargetMod, "EnergyGainOnShieldTarget", string.Empty, nanoSmithBlastShield.m_energyGainOnShieldTarget);
			AbilityMod.AddToken_EffectMod(tokens, m_extraEffectOnCasterIfTargetingAllyMod, "ExtraEffectOnCasterIfTargetingAlly", nanoSmithBlastShield.m_extraEffectOnCasterIfTargetingAlly);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NanoSmithBlastShield nanoSmithBlastShield = GetTargetAbilityOnAbilityData(abilityData) as NanoSmithBlastShield;
		bool flag = nanoSmithBlastShield != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyEffectData shieldEffectOverride = m_shieldEffectOverride;
		object baseVal;
		if (flag)
		{
			baseVal = nanoSmithBlastShield.m_shieldEffect;
		}
		else
		{
			baseVal = null;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(shieldEffectOverride, "{ Shield Effect }", flag, (StandardActorEffectData)baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_healOnEndIfHasRemainingAbsorbMod, "[Heal If Has Remaining Absorb]", flag, flag ? nanoSmithBlastShield.m_healOnEndIfHasRemainingAbsorb : 0);
		string str2 = empty;
		AbilityModPropertyInt energyGainOnShieldTargetMod = m_energyGainOnShieldTargetMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = nanoSmithBlastShield.m_energyGainOnShieldTarget;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(energyGainOnShieldTargetMod, "[Energy Gain on Shield Target]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyEffectInfo extraEffectOnCasterIfTargetingAllyMod = m_extraEffectOnCasterIfTargetingAllyMod;
		object baseVal3;
		if (flag)
		{
			baseVal3 = nanoSmithBlastShield.m_extraEffectOnCasterIfTargetingAlly;
		}
		else
		{
			baseVal3 = null;
		}
		return str3 + PropDesc(extraEffectOnCasterIfTargetingAllyMod, "[ExtraEffectOnCasterIfTargetingAlly]", flag, (StandardEffectInfo)baseVal3);
	}
}
