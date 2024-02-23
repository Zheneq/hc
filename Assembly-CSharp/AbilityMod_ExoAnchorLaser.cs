using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_ExoAnchorLaser : AbilityMod
{
	[Header("-- First Cast Damage (non-anchored)")]
	public AbilityModPropertyInt m_laserDamageAmountMod;
	public AbilityModPropertyLaserInfo m_laserInfoMod;
	[Header("-- Barrier For Beam")]
	public AbilityModPropertyBarrierDataV2 m_laserBarrierMod;
	[Header("-- Cone to Sweep Across")]
	public AbilityModPropertyInt m_sweepDamageAmountMod;
	public AbilityModPropertyFloat m_sweepConeBackwardOffsetMod;
	public AbilityModPropertyFloat m_minConeAngleMod;
	public AbilityModPropertyFloat m_maxConeAngleMod;
	[Header("-- Extra Damage: for anchored turns")]
	public AbilityModPropertyInt m_extraDamagePerTurnAnchoredMod;
	public AbilityModPropertyInt m_maxExtraDamageForAnchoredMod;
	[Header("-- Extra Damage: for distance")]
	public AbilityModPropertyFloat m_extraDamageAtZeroDistMod;
	public AbilityModPropertyFloat m_extraDamageChangePerDistMod;
	[Header("-- Effect while anchored and cooldown when finished")]
	public AbilityModPropertyEffectInfo m_effectOnCasterMod;
	public AbilityModPropertyInt m_cooldownOnEndMod;
	public AbilityModPropertyInt m_anchoredTechPointCostMod;
	public AbilityModPropertyEffectInfo m_effectOnAnchorEndMod;
	[Header("-- Pending Status for Anchored and NOT using sweep")]
	public bool m_useStatusWhenAnchoredAndNotSweepingOverride;
	public List<StatusType> m_statusWhenAnchoredAndNotSweepingOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(ExoAnchorLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ExoAnchorLaser exoAnchorLaser = targetAbility as ExoAnchorLaser;
		if (exoAnchorLaser != null)
		{
			AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, exoAnchorLaser.m_laserDamageAmount);
			AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", exoAnchorLaser.m_laserInfo);
			AddToken_BarrierMod(tokens, m_laserBarrierMod, "LaserBarrier", exoAnchorLaser.m_laserBarrier);
			AddToken(tokens, m_sweepDamageAmountMod, "SweepDamageAmount", string.Empty, exoAnchorLaser.m_sweepDamageAmount);
			AddToken(tokens, m_sweepConeBackwardOffsetMod, "SweepConeBackwardOffset", string.Empty, exoAnchorLaser.m_sweepConeBackwardOffset);
			AddToken(tokens, m_minConeAngleMod, "MinConeAngle", string.Empty, exoAnchorLaser.m_minConeAngle);
			AddToken(tokens, m_maxConeAngleMod, "MaxConeAngle", string.Empty, exoAnchorLaser.m_maxConeAngle);
			AddToken(tokens, m_extraDamagePerTurnAnchoredMod, "ExtraDamagePerTurnAnchored", string.Empty, exoAnchorLaser.m_extraDamagePerTurnAnchored);
			AddToken(tokens, m_maxExtraDamageForAnchoredMod, "MaxExtraDamageForAnchored", string.Empty, exoAnchorLaser.m_maxExtraDamageForAnchored);
			AddToken(tokens, m_extraDamageAtZeroDistMod, "ExtraDamageAtZeroDist", string.Empty, exoAnchorLaser.m_extraDamageAtZeroDist);
			AddToken(tokens, m_extraDamageChangePerDistMod, "ExtraDamageChangePerDist", string.Empty, exoAnchorLaser.m_extraDamageChangePerDist);
			AddToken_EffectMod(tokens, m_effectOnCasterMod, "EffectOnCaster", exoAnchorLaser.m_effectOnCaster);
			AddToken(tokens, m_cooldownOnEndMod, "CooldownOnEnd", string.Empty, exoAnchorLaser.m_cooldownOnEnd);
			AddToken(tokens, m_anchoredTechPointCostMod, "AnchoredTechPointCost", string.Empty, exoAnchorLaser.m_anchoredTechPointCost);
			AddToken_EffectMod(tokens, m_effectOnAnchorEndMod, "EffectOnAnchorEnd", exoAnchorLaser.m_effectOnAnchorEnd);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ExoAnchorLaser exoAnchorLaser = GetTargetAbilityOnAbilityData(abilityData) as ExoAnchorLaser;
		bool isValid = exoAnchorLaser != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserDamageAmountMod, "[LaserDamageAmount]", isValid, isValid ? exoAnchorLaser.m_laserDamageAmount : 0);
		desc += PropDesc(m_laserInfoMod, "[LaserInfo]", isValid, isValid ? exoAnchorLaser.m_laserInfo : null);
		desc += PropDescBarrierMod(m_laserBarrierMod, "{ LaserBarrier }", exoAnchorLaser.m_laserBarrier);
		desc += PropDesc(m_sweepDamageAmountMod, "[SweepDamageAmount]", isValid, isValid ? exoAnchorLaser.m_sweepDamageAmount : 0);
		desc += PropDesc(m_sweepConeBackwardOffsetMod, "[SweepConeBackwardOffset]", isValid, isValid ? exoAnchorLaser.m_sweepConeBackwardOffset : 0f);
		desc += PropDesc(m_minConeAngleMod, "[MinConeAngle]", isValid, isValid ? exoAnchorLaser.m_minConeAngle : 0f);
		desc += PropDesc(m_maxConeAngleMod, "[MaxConeAngle]", isValid, isValid ? exoAnchorLaser.m_maxConeAngle : 0f);
		desc += PropDesc(m_extraDamagePerTurnAnchoredMod, "[ExtraDamagePerTurnAnchored]", isValid, isValid ? exoAnchorLaser.m_extraDamagePerTurnAnchored : 0);
		desc += PropDesc(m_maxExtraDamageForAnchoredMod, "[MaxExtraDamageForAnchored]", isValid, isValid ? exoAnchorLaser.m_maxExtraDamageForAnchored : 0);
		desc += PropDesc(m_extraDamageAtZeroDistMod, "[ExtraDamageAtZeroDist]", isValid, isValid ? exoAnchorLaser.m_extraDamageAtZeroDist : 0f);
		desc += PropDesc(m_extraDamageChangePerDistMod, "[ExtraDamageChangePerDist]", isValid, isValid ? exoAnchorLaser.m_extraDamageChangePerDist : 0f);
		desc += PropDesc(m_effectOnCasterMod, "[EffectOnCaster]", isValid, isValid ? exoAnchorLaser.m_effectOnCaster : null);
		desc += PropDesc(m_cooldownOnEndMod, "[CooldownOnEnd]", isValid, isValid ? exoAnchorLaser.m_cooldownOnEnd : 0);
		desc += PropDesc(m_anchoredTechPointCostMod, "[AnchoredTechPointCost]", isValid, isValid ? exoAnchorLaser.m_anchoredTechPointCost : 0);
		desc += PropDesc(m_effectOnAnchorEndMod, "[EffectOnAnchorEnd]", isValid, isValid ? exoAnchorLaser.m_effectOnAnchorEnd : null);
		if (m_useStatusWhenAnchoredAndNotSweepingOverride)
		{
			desc += "Using Status when anchored and not sweeping override\n";
			foreach (StatusType statusOverride in m_statusWhenAnchoredAndNotSweepingOverride)
			{
				desc += new StringBuilder().Append("\t").Append(statusOverride).Append("\n").ToString();
			}
		}
		return desc;
	}
}
