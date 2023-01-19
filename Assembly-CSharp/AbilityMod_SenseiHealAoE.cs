// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SenseiHealAoE : AbilityMod
{
	[Separator("Targeting Info")]
	public AbilityModPropertyFloat m_circleRadiusMod;
	public AbilityModPropertyBool m_penetrateLoSMod;
	[Space(10f)]
	public AbilityModPropertyBool m_includeSelfMod;
	[Separator("Self Hit")]
	public AbilityModPropertyInt m_selfHealMod;
	[Space(10f)]
	public AbilityModPropertyFloat m_selfLowHealthThreshMod;
	public AbilityModPropertyInt m_extraSelfHealForLowHealthMod;
	[Separator("Ally Hit")]
	public AbilityModPropertyInt m_allyHealMod;
	public AbilityModPropertyInt m_extraAllyHealIfSingleHitMod;
	[Space(10f)]
	public AbilityModPropertyInt m_extraHealForAdjacentMod;
	public AbilityModPropertyFloat m_healChangeStartDistMod;
	public AbilityModPropertyFloat m_healChangePerDistMod;
	public AbilityModPropertyEffectInfo m_allyHitEffectMod;
	[Header("-- Extra Ally Heal for low health")]
	public AbilityModPropertyFloat m_allyLowHealthThreshMod;
	public AbilityModPropertyInt m_extraAllyHealForLowHealthMod;
	[Space(10f)]
	public AbilityModPropertyInt m_allyEnergyGainMod;
	[Header("-- Cooldown Reduction for damaging hits")]
	public AbilityModPropertyInt m_cdrForAnyDamageMod;
	public AbilityModPropertyInt m_cdrForDamagePerUniqueAbilityMod;
	[Separator("For trigger on Subsequent Turns")]
	public AbilityModPropertyInt m_turnsAfterInitialCastMod;
	public AbilityModPropertyInt m_allyHealOnSubsequentTurnsMod;
	public AbilityModPropertyInt m_selfHealOnSubsequentTurnsMod;
	public AbilityModPropertyEffectInfo m_allyEffectOnSubsequentTurnsMod;
	[Header("-- Energy gain on subsequent turns")]
	public AbilityModPropertyBool m_ignoreDefaultEnergyOnSubseqTurnsMod;
	public AbilityModPropertyInt m_energyPerAllyHitOnSubseqTurnsMod;
	public AbilityModPropertyInt m_energyOnSelfHitOnSubseqTurnsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SenseiHealAoE);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SenseiHealAoE senseiHealAoE = targetAbility as SenseiHealAoE;
		if (senseiHealAoE != null)
		{
			AddToken(tokens, m_circleRadiusMod, "CircleRadius", string.Empty, senseiHealAoE.m_circleRadius);
			AddToken(tokens, m_selfHealMod, "SelfHeal", string.Empty, senseiHealAoE.m_selfHeal);
			AddToken(tokens, m_selfLowHealthThreshMod, "SelfLowHealthThresh", string.Empty, senseiHealAoE.m_selfLowHealthThresh);
			AddToken(tokens, m_extraSelfHealForLowHealthMod, "ExtraSelfHealForLowHealth", string.Empty, senseiHealAoE.m_extraSelfHealForLowHealth);
			AddToken(tokens, m_allyHealMod, "AllyHeal", string.Empty, senseiHealAoE.m_allyHeal);
			AddToken(tokens, m_extraAllyHealIfSingleHitMod, "ExtraAllyHealIfSingleHit", string.Empty, senseiHealAoE.m_extraAllyHealIfSingleHit);
			AddToken(tokens, m_extraHealForAdjacentMod, "ExtraDamageForAdjacent", string.Empty, senseiHealAoE.m_extraHealForAdjacent);
			AddToken(tokens, m_healChangeStartDistMod, "HealChangeStartDist", string.Empty, senseiHealAoE.m_healChangeStartDist);
			AddToken(tokens, m_healChangePerDistMod, "HealChangePerDist", string.Empty, senseiHealAoE.m_healChangePerDist);
			AddToken(tokens, m_allyLowHealthThreshMod, "AllyLowHealthThresh", string.Empty, senseiHealAoE.m_allyLowHealthThresh);
			AddToken(tokens, m_extraAllyHealForLowHealthMod, "ExtraAllyHealForLowHealth", string.Empty, senseiHealAoE.m_extraAllyHealForLowHealth);
			AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", senseiHealAoE.m_allyHitEffect);
			AddToken(tokens, m_allyEnergyGainMod, "AllyEnergyGain", string.Empty, senseiHealAoE.m_allyEnergyGain);
			AddToken(tokens, m_cdrForAnyDamageMod, "CdrForAnyDamage", string.Empty, senseiHealAoE.m_cdrForAnyDamage);
			AddToken(tokens, m_cdrForDamagePerUniqueAbilityMod, "CdrForDamagePerUniqueAbility", string.Empty, senseiHealAoE.m_cdrForDamagePerUniqueAbility);
			AddToken(tokens, m_turnsAfterInitialCastMod, "TurnsAfterInitialCast", string.Empty, senseiHealAoE.m_turnsAfterInitialCast);
			AddToken(tokens, m_allyHealOnSubsequentTurnsMod, "AllyHealOnSubsequentTurns", string.Empty, senseiHealAoE.m_allyHealOnSubsequentTurns);
			AddToken(tokens, m_selfHealOnSubsequentTurnsMod, "SelfHealOnSubsequentTurns", string.Empty, senseiHealAoE.m_selfHealOnSubsequentTurns);
			AddToken_EffectMod(tokens, m_allyEffectOnSubsequentTurnsMod, "AllyEffectOnSubsequentTurns", senseiHealAoE.m_allyEffectOnSubsequentTurns);
			AddToken(tokens, m_energyPerAllyHitOnSubseqTurnsMod, "EnergyPerAllyHitOnSubseqTurns", string.Empty, senseiHealAoE.m_energyPerAllyHitOnSubseqTurns);
			AddToken(tokens, m_energyOnSelfHitOnSubseqTurnsMod, "EnergyOnSelfHitOnSubseqTurns", string.Empty, senseiHealAoE.m_energyOnSelfHitOnSubseqTurns);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		SenseiHealAoE senseiHealAoE = GetTargetAbilityOnAbilityData(abilityData) as SenseiHealAoE;
		// rogues
		// SenseiHealAoE senseiHealAoE = targetAbility as SenseiHealAoE;
		
		bool isValid = senseiHealAoE != null;
		string desc = string.Empty;
		desc += PropDesc(m_circleRadiusMod, "[CircleRadius]", isValid, isValid ? senseiHealAoE.m_circleRadius : 0f);
		desc += PropDesc(m_penetrateLoSMod, "[PenetrateLoS]", isValid, isValid && senseiHealAoE.m_penetrateLoS);
		desc += PropDesc(m_includeSelfMod, "[IncludeSelf]", isValid, isValid && senseiHealAoE.m_includeSelf);
		desc += PropDesc(m_selfHealMod, "[SelfHeal]", isValid, isValid ? senseiHealAoE.m_selfHeal : 0);
		desc += PropDesc(m_selfLowHealthThreshMod, "[SelfLowHealthThresh]", isValid, isValid ? senseiHealAoE.m_selfLowHealthThresh : 0f);
		desc += PropDesc(m_extraSelfHealForLowHealthMod, "[ExtraSelfHealForLowHealth]", isValid, isValid ? senseiHealAoE.m_extraSelfHealForLowHealth : 0);
		desc += PropDesc(m_allyHealMod, "[AllyHeal]", isValid, isValid ? senseiHealAoE.m_allyHeal : 0);
		desc += PropDesc(m_extraAllyHealIfSingleHitMod, "[ExtraAllyHealIfSingleHit]", isValid, isValid ? senseiHealAoE.m_extraAllyHealIfSingleHit : 0);
		desc += PropDesc(m_extraHealForAdjacentMod, "[ExtraHealForAdjacent]", isValid, isValid ? senseiHealAoE.m_extraHealForAdjacent : 0);
		desc += PropDesc(m_healChangeStartDistMod, "[HealChangeStartDist]", isValid, isValid ? senseiHealAoE.m_healChangeStartDist : 0f);
		desc += PropDesc(m_healChangePerDistMod, "[HealChangePerDist]", isValid, isValid ? senseiHealAoE.m_healChangePerDist : 0f);
		desc += PropDesc(m_allyLowHealthThreshMod, "[AllyLowHealthThresh]", isValid, isValid ? senseiHealAoE.m_allyLowHealthThresh : 0f);
		desc += PropDesc(m_extraAllyHealForLowHealthMod, "[ExtraAllyHealForLowHealth]", isValid, isValid ? senseiHealAoE.m_extraAllyHealForLowHealth : 0);
		desc += PropDesc(m_allyHitEffectMod, "[AllyHitEffect]", isValid, isValid ? senseiHealAoE.m_allyHitEffect : null);
		desc += PropDesc(m_allyEnergyGainMod, "[AllyEnergyGain]", isValid, isValid ? senseiHealAoE.m_allyEnergyGain : 0);
		desc += PropDesc(m_cdrForAnyDamageMod, "[CdrForAnyDamage]", isValid, isValid ? senseiHealAoE.m_cdrForAnyDamage : 0);
		desc += PropDesc(m_cdrForDamagePerUniqueAbilityMod, "[CdrForDamagePerUniqueAbility]", isValid, isValid ? senseiHealAoE.m_cdrForDamagePerUniqueAbility : 0);
		desc += PropDesc(m_turnsAfterInitialCastMod, "[TurnsAfterInitialCast]", isValid, isValid ? senseiHealAoE.m_turnsAfterInitialCast : 0);
		desc += PropDesc(m_allyHealOnSubsequentTurnsMod, "[AllyHealOnSubsequentTurns]", isValid, isValid ? senseiHealAoE.m_allyHealOnSubsequentTurns : 0);
		desc += PropDesc(m_selfHealOnSubsequentTurnsMod, "[SelfHealOnSubsequentTurns]", isValid, isValid ? senseiHealAoE.m_selfHealOnSubsequentTurns : 0);
		desc += PropDesc(m_allyEffectOnSubsequentTurnsMod, "[AllyEffectOnSubsequentTurns]", isValid, isValid ? senseiHealAoE.m_allyEffectOnSubsequentTurns : null);
		desc += PropDesc(m_ignoreDefaultEnergyOnSubseqTurnsMod, "[IgnoreDefaultEnergyOnSubseqTurns]", isValid, isValid && senseiHealAoE.m_ignoreDefaultEnergyOnSubseqTurns);
		desc += PropDesc(m_energyPerAllyHitOnSubseqTurnsMod, "[EnergyPerAllyHitOnSubseqTurns]", isValid, isValid ? senseiHealAoE.m_energyPerAllyHitOnSubseqTurns : 0);
		return desc + PropDesc(m_energyOnSelfHitOnSubseqTurnsMod, "[EnergyOnSelfHitOnSubseqTurns]", isValid, isValid ? senseiHealAoE.m_energyOnSelfHitOnSubseqTurns : 0);
	}
}
