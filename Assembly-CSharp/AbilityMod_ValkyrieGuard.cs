// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ValkyrieGuard : AbilityMod
{
	[Header("-- Shield effect")]
	public AbilityModPropertyEffectInfo m_shieldEffectInfoMod;
	[Header("-- Hit reactions")]
	public AbilityModPropertyInt m_techPointGainPerCoveredHitMod;
	public AbilityModPropertyInt m_techPointGainPerTooCloseForCoverHitMod;
	public AbilityModPropertyEffectInfo m_coveredHitReactionEffectMod;
	public AbilityModPropertyEffectInfo m_tooCloseForCoverHitReactionEffectMod;
	public AbilityModPropertyInt m_extraDamageNextShieldThrowPerCoveredHitMod;
	public AbilityModPropertyInt m_maxExtraDamageNextShieldThrow;
	[Header("-- Duration --")]
	public AbilityModPropertyInt m_coverDurationMod;
	public AbilityModPropertyBool m_coverLastsForeverMod;
	[Header("-- Cooldown reduction")]
	public AbilityModCooldownReduction m_cooldownReductionNoBlocks;
	[Header("-- Cover Ignore Min Dist?")]
	public AbilityModPropertyBool m_coverIgnoreMinDistMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ValkyrieGuard);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ValkyrieGuard valkyrieGuard = targetAbility as ValkyrieGuard;
		if (valkyrieGuard != null)
		{
			AddToken_EffectMod(tokens, m_shieldEffectInfoMod, "ShieldEffectInfo", valkyrieGuard.m_shieldEffectInfo);
			AddToken(tokens, m_techPointGainPerCoveredHitMod, "TechPointGainPerCoveredHit", string.Empty, valkyrieGuard.m_techPointGainPerCoveredHit);
			AddToken(tokens, m_techPointGainPerTooCloseForCoverHitMod, "TechPointGainPerTooCloseForCoverHit", string.Empty, valkyrieGuard.m_techPointGainPerTooCloseForCoverHit);
			AddToken_EffectMod(tokens, m_coveredHitReactionEffectMod, "CoveredHitReactionEffect", valkyrieGuard.m_coveredHitReactionEffect);
			AddToken_EffectMod(tokens, m_tooCloseForCoverHitReactionEffectMod, "TooCloseForCoverHitReactionEffect", valkyrieGuard.m_tooCloseForCoverHitReactionEffect);
			AddToken(tokens, m_extraDamageNextShieldThrowPerCoveredHitMod, "ExtraDamageNextShieldThrowPerCoveredHit", string.Empty, 0);
			AddToken(tokens, m_maxExtraDamageNextShieldThrow, "MaxExtraDamageNextShieldThrow", string.Empty, 0);
			AddToken(tokens, m_coverDurationMod, "CoverDuration", string.Empty, valkyrieGuard.m_coverDuration);
			m_cooldownReductionNoBlocks.AddTooltipTokens(tokens, "CooldownReductionNoBlocks");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		ValkyrieGuard valkyrieGuard = GetTargetAbilityOnAbilityData(abilityData) as ValkyrieGuard;
		// rogues
		// ValkyrieGuard valkyrieGuard = targetAbility as ValkyrieGuard;
		bool isValid = valkyrieGuard != null;
		string desc = string.Empty;
		desc += PropDesc(m_shieldEffectInfoMod, "[ShieldEffectInfo]", isValid, isValid ? valkyrieGuard.m_shieldEffectInfo : null);
		desc += PropDesc(m_techPointGainPerCoveredHitMod, "[TechPointGainPerCoveredHit]", isValid, isValid ? valkyrieGuard.m_techPointGainPerCoveredHit : 0);
		desc += PropDesc(m_techPointGainPerTooCloseForCoverHitMod, "[TechPointGainPerTooCloseForCoverHit]", isValid, isValid ? valkyrieGuard.m_techPointGainPerTooCloseForCoverHit : 0);
		desc += PropDesc(m_coveredHitReactionEffectMod, "[CoveredHitReactionEffect]", isValid, isValid ? valkyrieGuard.m_coveredHitReactionEffect : null);
		desc += PropDesc(m_tooCloseForCoverHitReactionEffectMod, "[TooCloseForCoverHitReactionEffect]", isValid, isValid ? valkyrieGuard.m_tooCloseForCoverHitReactionEffect : null);
		desc += PropDesc(m_extraDamageNextShieldThrowPerCoveredHitMod, "[ExtraDamageNextShieldThrowPerCoveredHit]", isValid);
		desc += PropDesc(m_maxExtraDamageNextShieldThrow, "[MaxExtraDamageNextShieldThrow]", isValid);
		desc += PropDesc(m_coverDurationMod, "[CoverDuration]", isValid, isValid ? valkyrieGuard.m_coverDuration : 0);
		desc += PropDesc(m_coverLastsForeverMod, "[CoverLastsForever]", isValid, isValid && valkyrieGuard.m_coverLastsForever);
		desc += m_cooldownReductionNoBlocks.GetDescription(abilityData);
		return desc + PropDesc(m_coverIgnoreMinDistMod, "[CoverIgnoreMinDist]", isValid, isValid && valkyrieGuard.m_coverIgnoreMinDist);
	}
}
