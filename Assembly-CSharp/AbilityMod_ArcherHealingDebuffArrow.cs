using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ArcherHealingDebuffArrow : AbilityMod
{
	[Header("-- Hit")]
	public AbilityModPropertyEffectInfo m_laserHitEffectMod;
	public AbilityModPropertyEffectInfo m_extraHitEffectMod;
	[Header("-- Reaction For Allies Hitting Target")]
	public AbilityModPropertyInt m_reactionHealingMod;
	public AbilityModPropertyInt m_reactionHealingOnSelfMod;
	public AbilityModPropertyInt m_lessHealingOnSubsequentReactions;
	public AbilityModPropertyInt m_healsPerAllyMod;
	public AbilityModPropertyInt m_techPointsPerHealMod;
	public AbilityModPropertyEffectInfo m_reactionEffectMod;
	public AbilityModPropertyInt m_extraHealForShieldGeneratorTargets;
	public AbilityModCooldownReduction m_cooldownReductionIfNoHeals;
	public AbilityModPropertyInt m_extraHealBelowHealthThresholdMod;
	public AbilityModPropertyFloat m_healthThresholdMod;
	public AbilityModPropertyInt m_extraDamageToThisTargetFromCasterMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ArcherHealingDebuffArrow);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ArcherHealingDebuffArrow archerHealingDebuffArrow = targetAbility as ArcherHealingDebuffArrow;
		if (archerHealingDebuffArrow != null)
		{
			AddToken_EffectMod(tokens, m_laserHitEffectMod, "LaserHitEffect", archerHealingDebuffArrow.m_laserHitEffect);
			AddToken_EffectMod(tokens, m_extraHitEffectMod, "LaserHitEffect");
			AddToken(tokens, m_reactionHealingMod, "ReactionHealing", string.Empty, archerHealingDebuffArrow.m_reactionHealing);
			AddToken(tokens, m_reactionHealingOnSelfMod, "ReactionHealingOnSelf", string.Empty, archerHealingDebuffArrow.m_reactionHealingOnSelf);
			AddToken(tokens, m_lessHealingOnSubsequentReactions, "LessHealingOnSubsequentReactions", string.Empty, 0);
			AddToken(tokens, m_techPointsPerHealMod, "TechPointsPerHeal", string.Empty, archerHealingDebuffArrow.m_techPointsPerHeal);
			AddToken_EffectMod(tokens, m_reactionEffectMod, "ReactionEffect", archerHealingDebuffArrow.m_reactionEffect);
			AddToken(tokens, m_extraHealForShieldGeneratorTargets, "ExtraHealForShieldGeneratorTargets", string.Empty, 0);
			m_cooldownReductionIfNoHeals.AddTooltipTokens(tokens, "CooldownReductionIfNoHeals");
			AddToken(tokens, m_extraHealBelowHealthThresholdMod, "ExtraHealForAlliesBelowHealthThreshold", string.Empty, 0);
			AddToken(tokens, m_healthThresholdMod, "HealthThresholdForExtraHealing", string.Empty, 0f, true, false, true);
			AddToken(tokens, m_extraDamageToThisTargetFromCasterMod, "ExtraDamageToThisTargetFromCaster", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherHealingDebuffArrow archerHealingDebuffArrow = GetTargetAbilityOnAbilityData(abilityData) as ArcherHealingDebuffArrow;
		bool isValid = archerHealingDebuffArrow != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserHitEffectMod, "[LaserHitEffect]", isValid, isValid ? archerHealingDebuffArrow.m_laserHitEffect : null);
		desc += PropDesc(m_extraHitEffectMod, "[ExtraHitEffect]");
		desc += PropDesc(m_reactionHealingMod, "[ReactionHealing]", isValid, isValid ? archerHealingDebuffArrow.m_reactionHealing : 0);
		desc += PropDesc(m_reactionHealingOnSelfMod, "[ReactionHealingOnSelf]", isValid, isValid ? archerHealingDebuffArrow.m_reactionHealingOnSelf : 0);
		desc += PropDesc(m_lessHealingOnSubsequentReactions, "[LessHealingOnSubsequentReactions]", isValid);
		desc += PropDesc(m_healsPerAllyMod, "[NumberOfHealingReactionsPerAlly]", isValid, isValid ? archerHealingDebuffArrow.m_healsPerAlly : 0);
		desc += PropDesc(m_techPointsPerHealMod, "[TechPointsPerHeal]", isValid, isValid ? archerHealingDebuffArrow.m_techPointsPerHeal : 0);
		desc += PropDesc(m_reactionEffectMod, "[ReactionEffect]", isValid, isValid ? archerHealingDebuffArrow.m_reactionEffect : null);
		desc += PropDesc(m_extraHealForShieldGeneratorTargets, "[ExtraHealForShieldGeneratorTargets]", isValid);
		if (m_cooldownReductionIfNoHeals != null && m_cooldownReductionIfNoHeals.HasCooldownReduction())
		{
			desc += m_cooldownReductionIfNoHeals.GetDescription(abilityData);
		}
		desc += PropDesc(m_extraHealBelowHealthThresholdMod, "[ExtraHealForAlliesBelowHealthThreshold]", isValid);
		desc += PropDesc(m_healthThresholdMod, "[HealthThresholdForExtraHealing]", isValid);
		return desc + PropDesc(m_extraDamageToThisTargetFromCasterMod, "[ExtraDamageToThisTargetFromCaster]", isValid);
	}
}
