using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ExoShield : AbilityMod
{
	[Header("-- Shield/Absorb Effect")]
	public AbilityModPropertyEffectData m_absorbEffectMod;
	[Header("-- Extra shielding when using ult")]
	public AbilityModPropertyInt m_extraAbsorbIfSiegingMod;
	[Header("-- Cooldowwn Reduction if no shield used")]
	public AbilityModPropertyInt m_cdrIfShieldNotUsedMod;
	[Header("-- Shielding lost to energy conversion (on effect end")]
	public AbilityModPropertyInt m_shieldLostPerEnergyGainMod;
	public AbilityModPropertyInt m_maxShieldLostForEnergyGainMod;
	[Header("-- (If using energy to shield conversion) Energy to use for conversion, use 0 if there is no max")]
	public AbilityModPropertyInt m_maxTechPointsCostMod;
	public AbilityModPropertyInt m_minTechPointsForCastMod;
	public AbilityModPropertyBool m_freeActionWhileAnchoredMod;
	[Header("-- Targeter shape - use for doing stuff to nearby actors")]
	public AbilityModPropertyShape m_targeterShapeMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ExoShield);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ExoShield exoShield = targetAbility as ExoShield;
		if (exoShield != null)
		{
			AddToken_EffectMod(tokens, m_absorbEffectMod, "AbsorbEffect", exoShield.m_absorbEffect);
			AddToken(tokens, m_extraAbsorbIfSiegingMod, "ExtraAbsorbIfSieging", string.Empty, exoShield.m_extraAbsorbIfSieging);
			AddToken(tokens, m_cdrIfShieldNotUsedMod, "CdrIfShieldNotUsed", string.Empty, exoShield.m_cdrIfShieldNotUsed);
			AddToken(tokens, m_shieldLostPerEnergyGainMod, "ShieldLostPerEnergyGain", string.Empty, exoShield.m_shieldLostPerEnergyGain);
			AddToken(tokens, m_maxShieldLostForEnergyGainMod, "MaxShieldLostForEnergyGain", string.Empty, exoShield.m_maxShieldLostForEnergyGain);
			AddToken(tokens, m_maxTechPointsCostMod, "MaxTechPointsCost", string.Empty, exoShield.m_maxTechPointsCost);
			AddToken(tokens, m_minTechPointsForCastMod, "MinTechPointsForCast", string.Empty, exoShield.m_minTechPointsForCast);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ExoShield exoShield = GetTargetAbilityOnAbilityData(abilityData) as ExoShield;
		bool isValid = exoShield != null;
		string desc = string.Empty;
		desc += PropDesc(m_absorbEffectMod, "[AbsorbEffect]", isValid, isValid ? exoShield.m_absorbEffect : null);
		desc += PropDesc(m_extraAbsorbIfSiegingMod, "[ExtraAbsorbIfSieging]", isValid, isValid ? exoShield.m_extraAbsorbIfSieging : 0);
		desc += PropDesc(m_cdrIfShieldNotUsedMod, "[CdrIfShieldNotUsed]", isValid, isValid ? exoShield.m_cdrIfShieldNotUsed : 0);
		desc += PropDesc(m_shieldLostPerEnergyGainMod, "[ShieldLostPerEnergyGain]", isValid, isValid ? exoShield.m_shieldLostPerEnergyGain : 0);
		desc += PropDesc(m_maxShieldLostForEnergyGainMod, "[MaxShieldLostForEnergyGain]", isValid, isValid ? exoShield.m_maxShieldLostForEnergyGain : 0);
		desc += PropDesc(m_maxTechPointsCostMod, "[MaxTechPointsCost]", isValid, isValid ? exoShield.m_maxTechPointsCost : 0);
		desc += PropDesc(m_minTechPointsForCastMod, "[MinTechPointsForCast]", isValid, isValid ? exoShield.m_minTechPointsForCast : 0);
		desc += PropDesc(m_freeActionWhileAnchoredMod, "[FreeActionWhileAnchored]", isValid, isValid && exoShield.m_freeActionWhileAnchored);
		return desc + PropDesc(m_targeterShapeMod, "[TargeterShape]", isValid, isValid ? exoShield.m_targeterShape : AbilityAreaShape.SingleSquare);
	}
}
