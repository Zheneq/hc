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
		if (!(valkyrieGuard != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_EffectMod(tokens, m_shieldEffectInfoMod, "ShieldEffectInfo", valkyrieGuard.m_shieldEffectInfo);
			AbilityMod.AddToken(tokens, m_techPointGainPerCoveredHitMod, "TechPointGainPerCoveredHit", string.Empty, valkyrieGuard.m_techPointGainPerCoveredHit);
			AbilityMod.AddToken(tokens, m_techPointGainPerTooCloseForCoverHitMod, "TechPointGainPerTooCloseForCoverHit", string.Empty, valkyrieGuard.m_techPointGainPerTooCloseForCoverHit);
			AbilityMod.AddToken_EffectMod(tokens, m_coveredHitReactionEffectMod, "CoveredHitReactionEffect", valkyrieGuard.m_coveredHitReactionEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_tooCloseForCoverHitReactionEffectMod, "TooCloseForCoverHitReactionEffect", valkyrieGuard.m_tooCloseForCoverHitReactionEffect);
			AbilityMod.AddToken(tokens, m_extraDamageNextShieldThrowPerCoveredHitMod, "ExtraDamageNextShieldThrowPerCoveredHit", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_maxExtraDamageNextShieldThrow, "MaxExtraDamageNextShieldThrow", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_coverDurationMod, "CoverDuration", string.Empty, valkyrieGuard.m_coverDuration);
			m_cooldownReductionNoBlocks.AddTooltipTokens(tokens, "CooldownReductionNoBlocks");
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ValkyrieGuard valkyrieGuard = GetTargetAbilityOnAbilityData(abilityData) as ValkyrieGuard;
		bool flag = valkyrieGuard != null;
		string empty = string.Empty;
		empty += PropDesc(m_shieldEffectInfoMod, "[ShieldEffectInfo]", flag, (!flag) ? null : valkyrieGuard.m_shieldEffectInfo);
		string str = empty;
		AbilityModPropertyInt techPointGainPerCoveredHitMod = m_techPointGainPerCoveredHitMod;
		int baseVal;
		if (flag)
		{
			baseVal = valkyrieGuard.m_techPointGainPerCoveredHit;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(techPointGainPerCoveredHitMod, "[TechPointGainPerCoveredHit]", flag, baseVal);
		empty += PropDesc(m_techPointGainPerTooCloseForCoverHitMod, "[TechPointGainPerTooCloseForCoverHit]", flag, flag ? valkyrieGuard.m_techPointGainPerTooCloseForCoverHit : 0);
		string str2 = empty;
		AbilityModPropertyEffectInfo coveredHitReactionEffectMod = m_coveredHitReactionEffectMod;
		object baseVal2;
		if (flag)
		{
			baseVal2 = valkyrieGuard.m_coveredHitReactionEffect;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str2 + PropDesc(coveredHitReactionEffectMod, "[CoveredHitReactionEffect]", flag, (StandardEffectInfo)baseVal2);
		string str3 = empty;
		AbilityModPropertyEffectInfo tooCloseForCoverHitReactionEffectMod = m_tooCloseForCoverHitReactionEffectMod;
		object baseVal3;
		if (flag)
		{
			baseVal3 = valkyrieGuard.m_tooCloseForCoverHitReactionEffect;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str3 + PropDesc(tooCloseForCoverHitReactionEffectMod, "[TooCloseForCoverHitReactionEffect]", flag, (StandardEffectInfo)baseVal3);
		empty += PropDesc(m_extraDamageNextShieldThrowPerCoveredHitMod, "[ExtraDamageNextShieldThrowPerCoveredHit]", flag);
		empty += PropDesc(m_maxExtraDamageNextShieldThrow, "[MaxExtraDamageNextShieldThrow]", flag);
		string str4 = empty;
		AbilityModPropertyInt coverDurationMod = m_coverDurationMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = valkyrieGuard.m_coverDuration;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(coverDurationMod, "[CoverDuration]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyBool coverLastsForeverMod = m_coverLastsForeverMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = (valkyrieGuard.m_coverLastsForever ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(coverLastsForeverMod, "[CoverLastsForever]", flag, (byte)baseVal5 != 0);
		empty += m_cooldownReductionNoBlocks.GetDescription(abilityData);
		string str6 = empty;
		AbilityModPropertyBool coverIgnoreMinDistMod = m_coverIgnoreMinDistMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = (valkyrieGuard.m_coverIgnoreMinDist ? 1 : 0);
		}
		else
		{
			baseVal6 = 0;
		}
		return str6 + PropDesc(coverIgnoreMinDistMod, "[CoverIgnoreMinDist]", flag, (byte)baseVal6 != 0);
	}
}
