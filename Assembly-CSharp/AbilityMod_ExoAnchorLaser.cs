using System;
using System.Collections.Generic;
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
		if (!(exoAnchorLaser != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, exoAnchorLaser.m_laserDamageAmount);
			AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", exoAnchorLaser.m_laserInfo);
			AbilityMod.AddToken_BarrierMod(tokens, m_laserBarrierMod, "LaserBarrier", exoAnchorLaser.m_laserBarrier);
			AbilityMod.AddToken(tokens, m_sweepDamageAmountMod, "SweepDamageAmount", string.Empty, exoAnchorLaser.m_sweepDamageAmount);
			AbilityMod.AddToken(tokens, m_sweepConeBackwardOffsetMod, "SweepConeBackwardOffset", string.Empty, exoAnchorLaser.m_sweepConeBackwardOffset);
			AbilityMod.AddToken(tokens, m_minConeAngleMod, "MinConeAngle", string.Empty, exoAnchorLaser.m_minConeAngle);
			AbilityMod.AddToken(tokens, m_maxConeAngleMod, "MaxConeAngle", string.Empty, exoAnchorLaser.m_maxConeAngle);
			AbilityMod.AddToken(tokens, m_extraDamagePerTurnAnchoredMod, "ExtraDamagePerTurnAnchored", string.Empty, exoAnchorLaser.m_extraDamagePerTurnAnchored);
			AbilityMod.AddToken(tokens, m_maxExtraDamageForAnchoredMod, "MaxExtraDamageForAnchored", string.Empty, exoAnchorLaser.m_maxExtraDamageForAnchored);
			AbilityMod.AddToken(tokens, m_extraDamageAtZeroDistMod, "ExtraDamageAtZeroDist", string.Empty, exoAnchorLaser.m_extraDamageAtZeroDist);
			AbilityMod.AddToken(tokens, m_extraDamageChangePerDistMod, "ExtraDamageChangePerDist", string.Empty, exoAnchorLaser.m_extraDamageChangePerDist);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnCasterMod, "EffectOnCaster", exoAnchorLaser.m_effectOnCaster);
			AbilityMod.AddToken(tokens, m_cooldownOnEndMod, "CooldownOnEnd", string.Empty, exoAnchorLaser.m_cooldownOnEnd);
			AbilityMod.AddToken(tokens, m_anchoredTechPointCostMod, "AnchoredTechPointCost", string.Empty, exoAnchorLaser.m_anchoredTechPointCost);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnAnchorEndMod, "EffectOnAnchorEnd", exoAnchorLaser.m_effectOnAnchorEnd);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ExoAnchorLaser exoAnchorLaser = GetTargetAbilityOnAbilityData(abilityData) as ExoAnchorLaser;
		bool flag = exoAnchorLaser != null;
		string empty = string.Empty;
		empty += PropDesc(m_laserDamageAmountMod, "[LaserDamageAmount]", flag, flag ? exoAnchorLaser.m_laserDamageAmount : 0);
		empty += PropDesc(m_laserInfoMod, "[LaserInfo]", flag, (!flag) ? null : exoAnchorLaser.m_laserInfo);
		empty += PropDescBarrierMod(m_laserBarrierMod, "{ LaserBarrier }", exoAnchorLaser.m_laserBarrier);
		empty += PropDesc(m_sweepDamageAmountMod, "[SweepDamageAmount]", flag, flag ? exoAnchorLaser.m_sweepDamageAmount : 0);
		string str = empty;
		AbilityModPropertyFloat sweepConeBackwardOffsetMod = m_sweepConeBackwardOffsetMod;
		float baseVal;
		if (flag)
		{
			baseVal = exoAnchorLaser.m_sweepConeBackwardOffset;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(sweepConeBackwardOffsetMod, "[SweepConeBackwardOffset]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat minConeAngleMod = m_minConeAngleMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = exoAnchorLaser.m_minConeAngle;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(minConeAngleMod, "[MinConeAngle]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat maxConeAngleMod = m_maxConeAngleMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = exoAnchorLaser.m_maxConeAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(maxConeAngleMod, "[MaxConeAngle]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyInt extraDamagePerTurnAnchoredMod = m_extraDamagePerTurnAnchoredMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = exoAnchorLaser.m_extraDamagePerTurnAnchored;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(extraDamagePerTurnAnchoredMod, "[ExtraDamagePerTurnAnchored]", flag, baseVal4);
		empty += PropDesc(m_maxExtraDamageForAnchoredMod, "[MaxExtraDamageForAnchored]", flag, flag ? exoAnchorLaser.m_maxExtraDamageForAnchored : 0);
		string str5 = empty;
		AbilityModPropertyFloat extraDamageAtZeroDistMod = m_extraDamageAtZeroDistMod;
		float baseVal5;
		if (flag)
		{
			baseVal5 = exoAnchorLaser.m_extraDamageAtZeroDist;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + PropDesc(extraDamageAtZeroDistMod, "[ExtraDamageAtZeroDist]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyFloat extraDamageChangePerDistMod = m_extraDamageChangePerDistMod;
		float baseVal6;
		if (flag)
		{
			baseVal6 = exoAnchorLaser.m_extraDamageChangePerDist;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + PropDesc(extraDamageChangePerDistMod, "[ExtraDamageChangePerDist]", flag, baseVal6);
		empty += PropDesc(m_effectOnCasterMod, "[EffectOnCaster]", flag, (!flag) ? null : exoAnchorLaser.m_effectOnCaster);
		empty += PropDesc(m_cooldownOnEndMod, "[CooldownOnEnd]", flag, flag ? exoAnchorLaser.m_cooldownOnEnd : 0);
		empty += PropDesc(m_anchoredTechPointCostMod, "[AnchoredTechPointCost]", flag, flag ? exoAnchorLaser.m_anchoredTechPointCost : 0);
		empty += PropDesc(m_effectOnAnchorEndMod, "[EffectOnAnchorEnd]", flag, (!flag) ? null : exoAnchorLaser.m_effectOnAnchorEnd);
		if (m_useStatusWhenAnchoredAndNotSweepingOverride)
		{
			empty += "Using Status when anchored and not sweeping override\n";
			for (int i = 0; i < m_statusWhenAnchoredAndNotSweepingOverride.Count; i++)
			{
				empty = empty + "\t" + m_statusWhenAnchoredAndNotSweepingOverride[i].ToString() + "\n";
			}
		}
		return empty;
	}
}
