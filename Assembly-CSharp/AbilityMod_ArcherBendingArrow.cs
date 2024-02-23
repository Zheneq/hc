using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_ArcherBendingArrow : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_minRangeBeforeBendMod;
	public AbilityModPropertyFloat m_maxRangeBeforeBendMod;
	public AbilityModPropertyFloat m_maxTotalRangeMod;
	public AbilityModPropertyFloat m_maxBendAngleMod;
	public AbilityModPropertyBool m_penetrateLoSMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	[Header("-- Damage")]
	public AbilityModPropertyInt m_laserDamageAmountMod;
	public AbilityModPropertyEffectInfo m_laserHitEffectMod;
	public AbilityModPropertyInt m_extraDamageToHealingDebuffTarget;
	public AbilityModPropertyInt m_extraDamageAfterBend;
	public AbilityModPropertyInt m_damageAfterPiercingMod;
	public AbilityModPropertyEffectInfo m_effectToHealingDebuffTarget;
	public AbilityModPropertyInt m_extraHealingFromHealingDebuffTarget;
	[Header("-- Misc Ability Interactions")]
	public AbilityModPropertyInt m_nextShieldGeneratorExtraAbsorbPerHit;
	public AbilityModPropertyInt m_nextShieldGeneratorExtraAbsorbMax;

	public override Type GetTargetAbilityType()
	{
		return typeof(ArcherBendingArrow);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ArcherBendingArrow archerBendingArrow = targetAbility as ArcherBendingArrow;
		if (archerBendingArrow != null)
		{
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, archerBendingArrow.m_laserWidth);
			AddToken(tokens, m_minRangeBeforeBendMod, "MinRangeBeforeBend", string.Empty, archerBendingArrow.m_minRangeBeforeBend);
			AddToken(tokens, m_maxRangeBeforeBendMod, "MaxRangeBeforeBend", string.Empty, archerBendingArrow.m_maxRangeBeforeBend);
			AddToken(tokens, m_maxTotalRangeMod, "MaxTotalRange", string.Empty, archerBendingArrow.m_maxTotalRange);
			AddToken(tokens, m_maxBendAngleMod, "MaxBendAngle", string.Empty, archerBendingArrow.m_maxBendAngle);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, archerBendingArrow.m_maxTargets);
			AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, archerBendingArrow.m_laserDamageAmount);
			AddToken_EffectMod(tokens, m_laserHitEffectMod, "LaserHitEffect", archerBendingArrow.m_laserHitEffect);
			AddToken(tokens, m_nextShieldGeneratorExtraAbsorbPerHit, "NextShieldGeneratorExtraAbsorbPerHit", string.Empty, 0);
			AddToken(tokens, m_nextShieldGeneratorExtraAbsorbMax, "NextShieldGeneratorExtraAbsorbMax", string.Empty, 0);
			AddToken(tokens, m_extraDamageToHealingDebuffTarget, "ExtraDamageToHealingDebuffTarget", string.Empty, 0);
			AddToken(tokens, m_extraDamageAfterBend, "ExtraDamageAfterBend", string.Empty, 0);
			AddToken(tokens, m_damageAfterPiercingMod, "DamageToSubsequentTargetsAfterPiercing", string.Empty, archerBendingArrow.m_laserDamageAmount);
			AddToken_EffectMod(tokens, m_effectToHealingDebuffTarget, "EffectToHealingDebuffTarget");
			AddToken(tokens, m_extraHealingFromHealingDebuffTarget, "ExtraHealingFromHealingDebuffTarget", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherBendingArrow archerBendingArrow = GetTargetAbilityOnAbilityData(abilityData) as ArcherBendingArrow;
		bool isValid = archerBendingArrow != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isValid, isValid ? archerBendingArrow.m_laserWidth : 0f);
		desc += PropDesc(m_minRangeBeforeBendMod, "[MinRangeBeforeBend]", isValid, isValid ? archerBendingArrow.m_minRangeBeforeBend : 0f);
		desc += PropDesc(m_maxRangeBeforeBendMod, "[MaxRangeBeforeBend]", isValid, isValid ? archerBendingArrow.m_maxRangeBeforeBend : 0f);
		desc += PropDesc(m_maxTotalRangeMod, "[MaxTotalRange]", isValid, isValid ? archerBendingArrow.m_maxTotalRange : 0f);
		desc += PropDesc(m_maxBendAngleMod, "[MaxBendAngle]", isValid, isValid ? archerBendingArrow.m_maxBendAngle : 0f);
		desc += PropDesc(m_penetrateLoSMod, "[PenetrateLoS]", isValid, isValid && archerBendingArrow.m_penetrateLoS);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? archerBendingArrow.m_maxTargets : 0);
		desc += PropDesc(m_laserDamageAmountMod, "[LaserDamageAmount]", isValid, isValid ? archerBendingArrow.m_laserDamageAmount : 0);
		desc += PropDesc(m_laserHitEffectMod, "[LaserHitEffect]", isValid, isValid ? archerBendingArrow.m_laserHitEffect : null);
		desc += PropDesc(m_nextShieldGeneratorExtraAbsorbPerHit, "[NextShieldGeneratorExtraAbsorbPerHit]", isValid);
		desc += PropDesc(m_nextShieldGeneratorExtraAbsorbMax, "[NextShieldGeneratorExtraAbsorbMax]", isValid);
		desc += PropDesc(m_extraDamageToHealingDebuffTarget, "[ExtraDamageToHealingDebuffTarget]", isValid);
		desc += PropDesc(m_extraDamageAfterBend, "[ExtraDamageAfterBend]", isValid);
		desc += PropDesc(m_damageAfterPiercingMod, "[DamageToSubsequentTargetsAfterPiercing]", isValid, isValid ? archerBendingArrow.m_laserDamageAmount : 0);
		desc += PropDesc(m_effectToHealingDebuffTarget, "[EffectToHealingDebuffTarget]");
		return new StringBuilder().Append(desc).Append(PropDesc(m_extraHealingFromHealingDebuffTarget, "[ExtraHealingFromHealingDebuffTarget]", isValid)).ToString();
	}
}
