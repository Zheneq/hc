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
		if (exoAnchorLaser != null)
		{
			AbilityMod.AddToken(tokens, this.m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, exoAnchorLaser.m_laserDamageAmount, true, false);
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "LaserInfo", exoAnchorLaser.m_laserInfo, true);
			AbilityMod.AddToken_BarrierMod(tokens, this.m_laserBarrierMod, "LaserBarrier", exoAnchorLaser.m_laserBarrier);
			AbilityMod.AddToken(tokens, this.m_sweepDamageAmountMod, "SweepDamageAmount", string.Empty, exoAnchorLaser.m_sweepDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_sweepConeBackwardOffsetMod, "SweepConeBackwardOffset", string.Empty, exoAnchorLaser.m_sweepConeBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_minConeAngleMod, "MinConeAngle", string.Empty, exoAnchorLaser.m_minConeAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxConeAngleMod, "MaxConeAngle", string.Empty, exoAnchorLaser.m_maxConeAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_extraDamagePerTurnAnchoredMod, "ExtraDamagePerTurnAnchored", string.Empty, exoAnchorLaser.m_extraDamagePerTurnAnchored, true, false);
			AbilityMod.AddToken(tokens, this.m_maxExtraDamageForAnchoredMod, "MaxExtraDamageForAnchored", string.Empty, exoAnchorLaser.m_maxExtraDamageForAnchored, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageAtZeroDistMod, "ExtraDamageAtZeroDist", string.Empty, exoAnchorLaser.m_extraDamageAtZeroDist, true, false, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageChangePerDistMod, "ExtraDamageChangePerDist", string.Empty, exoAnchorLaser.m_extraDamageChangePerDist, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnCasterMod, "EffectOnCaster", exoAnchorLaser.m_effectOnCaster, true);
			AbilityMod.AddToken(tokens, this.m_cooldownOnEndMod, "CooldownOnEnd", string.Empty, exoAnchorLaser.m_cooldownOnEnd, true, false);
			AbilityMod.AddToken(tokens, this.m_anchoredTechPointCostMod, "AnchoredTechPointCost", string.Empty, exoAnchorLaser.m_anchoredTechPointCost, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnAnchorEndMod, "EffectOnAnchorEnd", exoAnchorLaser.m_effectOnAnchorEnd, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ExoAnchorLaser exoAnchorLaser = base.GetTargetAbilityOnAbilityData(abilityData) as ExoAnchorLaser;
		bool flag = exoAnchorLaser != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_laserDamageAmountMod, "[LaserDamageAmount]", flag, (!flag) ? 0 : exoAnchorLaser.m_laserDamageAmount);
		text += base.PropDesc(this.m_laserInfoMod, "[LaserInfo]", flag, (!flag) ? null : exoAnchorLaser.m_laserInfo);
		text += base.PropDescBarrierMod(this.m_laserBarrierMod, "{ LaserBarrier }", exoAnchorLaser.m_laserBarrier);
		text += base.PropDesc(this.m_sweepDamageAmountMod, "[SweepDamageAmount]", flag, (!flag) ? 0 : exoAnchorLaser.m_sweepDamageAmount);
		string str = text;
		AbilityModPropertyFloat sweepConeBackwardOffsetMod = this.m_sweepConeBackwardOffsetMod;
		string prefix = "[SweepConeBackwardOffset]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = exoAnchorLaser.m_sweepConeBackwardOffset;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(sweepConeBackwardOffsetMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat minConeAngleMod = this.m_minConeAngleMod;
		string prefix2 = "[MinConeAngle]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = exoAnchorLaser.m_minConeAngle;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(minConeAngleMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat maxConeAngleMod = this.m_maxConeAngleMod;
		string prefix3 = "[MaxConeAngle]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = exoAnchorLaser.m_maxConeAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(maxConeAngleMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt extraDamagePerTurnAnchoredMod = this.m_extraDamagePerTurnAnchoredMod;
		string prefix4 = "[ExtraDamagePerTurnAnchored]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = exoAnchorLaser.m_extraDamagePerTurnAnchored;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(extraDamagePerTurnAnchoredMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_maxExtraDamageForAnchoredMod, "[MaxExtraDamageForAnchored]", flag, (!flag) ? 0 : exoAnchorLaser.m_maxExtraDamageForAnchored);
		string str5 = text;
		AbilityModPropertyFloat extraDamageAtZeroDistMod = this.m_extraDamageAtZeroDistMod;
		string prefix5 = "[ExtraDamageAtZeroDist]";
		bool showBaseVal5 = flag;
		float baseVal5;
		if (flag)
		{
			baseVal5 = exoAnchorLaser.m_extraDamageAtZeroDist;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + base.PropDesc(extraDamageAtZeroDistMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyFloat extraDamageChangePerDistMod = this.m_extraDamageChangePerDistMod;
		string prefix6 = "[ExtraDamageChangePerDist]";
		bool showBaseVal6 = flag;
		float baseVal6;
		if (flag)
		{
			baseVal6 = exoAnchorLaser.m_extraDamageChangePerDist;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + base.PropDesc(extraDamageChangePerDistMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_effectOnCasterMod, "[EffectOnCaster]", flag, (!flag) ? null : exoAnchorLaser.m_effectOnCaster);
		text += base.PropDesc(this.m_cooldownOnEndMod, "[CooldownOnEnd]", flag, (!flag) ? 0 : exoAnchorLaser.m_cooldownOnEnd);
		text += base.PropDesc(this.m_anchoredTechPointCostMod, "[AnchoredTechPointCost]", flag, (!flag) ? 0 : exoAnchorLaser.m_anchoredTechPointCost);
		text += base.PropDesc(this.m_effectOnAnchorEndMod, "[EffectOnAnchorEnd]", flag, (!flag) ? null : exoAnchorLaser.m_effectOnAnchorEnd);
		if (this.m_useStatusWhenAnchoredAndNotSweepingOverride)
		{
			text += "Using Status when anchored and not sweeping override\n";
			for (int i = 0; i < this.m_statusWhenAnchoredAndNotSweepingOverride.Count; i++)
			{
				text = text + "\t" + this.m_statusWhenAnchoredAndNotSweepingOverride[i].ToString() + "\n";
			}
		}
		return text;
	}
}
