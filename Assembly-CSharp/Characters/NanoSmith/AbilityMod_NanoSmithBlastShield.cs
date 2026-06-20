// ROGUES
// SERVER
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
		if (nanoSmithBlastShield != null)
		{
			AddToken_EffectMod(tokens, m_shieldEffectOverride, "ShieldEffect", nanoSmithBlastShield.m_shieldEffect);
			AddToken(tokens, m_healOnEndIfHasRemainingAbsorbMod, "HealOnEndIfHasRemainingAbsorb", string.Empty, nanoSmithBlastShield.m_healOnEndIfHasRemainingAbsorb);
			AddToken(tokens, m_energyGainOnShieldTargetMod, "EnergyGainOnShieldTarget", string.Empty, nanoSmithBlastShield.m_energyGainOnShieldTarget);
			AddToken_EffectMod(tokens, m_extraEffectOnCasterIfTargetingAllyMod, "ExtraEffectOnCasterIfTargetingAlly", nanoSmithBlastShield.m_extraEffectOnCasterIfTargetingAlly);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		NanoSmithBlastShield nanoSmithBlastShield = GetTargetAbilityOnAbilityData(abilityData) as NanoSmithBlastShield;
		// rogues
		// NanoSmithBlastShield nanoSmithBlastShield = targetAbility as NanoSmithBlastShield;
		
		bool isValid = nanoSmithBlastShield != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_shieldEffectOverride, "{ Shield Effect }", isValid, isValid ? nanoSmithBlastShield.m_shieldEffect : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_healOnEndIfHasRemainingAbsorbMod, "[Heal If Has Remaining Absorb]", isValid, isValid ? nanoSmithBlastShield.m_healOnEndIfHasRemainingAbsorb : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_energyGainOnShieldTargetMod, "[Energy Gain on Shield Target]", isValid, isValid ? nanoSmithBlastShield.m_energyGainOnShieldTarget : 0);
		return desc + PropDesc(m_extraEffectOnCasterIfTargetingAllyMod, "[ExtraEffectOnCasterIfTargetingAlly]", isValid, isValid ? nanoSmithBlastShield.m_extraEffectOnCasterIfTargetingAlly : null);
	}
}
