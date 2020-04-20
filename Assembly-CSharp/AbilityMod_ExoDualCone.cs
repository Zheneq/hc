using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ExoDualCone : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyConeInfo m_coneInfoMod;

	public AbilityModPropertyFloat m_leftConeHorizontalOffsetMod;

	public AbilityModPropertyFloat m_rightConeHorizontalOffsetMod;

	public AbilityModPropertyFloat m_coneForwardOffsetMod;

	[Space(10f)]
	public AbilityModPropertyFloat m_leftConeDegreesFromForwardMod;

	public AbilityModPropertyFloat m_rightConeDegreesFromForwardMod;

	[Header("-- Targeting, if interpolating angle")]
	public AbilityModPropertyBool m_interpolateAngleMod;

	public AbilityModPropertyFloat m_interpolateMinAngleMod;

	public AbilityModPropertyFloat m_interpolateMaxAngleMod;

	public AbilityModPropertyFloat m_interpolateMinDistMod;

	public AbilityModPropertyFloat m_interpolateMaxDistMod;

	[Header("-- Damage")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyInt m_extraDamageForOverlapMod;

	public AbilityModPropertyInt m_extraDamageForSingleHitMod;

	public AbilityModPropertyEffectInfo m_effectOnHitMod;

	public AbilityModPropertyEffectInfo m_effectOnOverlapHitMod;

	[Header("-- Extra Damage for hitting on consecutive turns")]
	public AbilityModPropertyInt m_extraDamageForConsecitiveHitMod;

	public AbilityModPropertyInt m_extraEnergyForConsecutiveUseMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ExoDualCone);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ExoDualCone exoDualCone = targetAbility as ExoDualCone;
		if (exoDualCone != null)
		{
			AbilityMod.AddToken_ConeInfo(tokens, this.m_coneInfoMod, "ConeInfo", exoDualCone.m_coneInfo, true);
			AbilityMod.AddToken(tokens, this.m_leftConeHorizontalOffsetMod, "LeftConeHorizontalOffset", string.Empty, exoDualCone.m_leftConeHorizontalOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_rightConeHorizontalOffsetMod, "RightConeHorizontalOffset", string.Empty, exoDualCone.m_rightConeHorizontalOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneForwardOffsetMod, "ConeForwardOffset", string.Empty, exoDualCone.m_coneForwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_leftConeDegreesFromForwardMod, "LeftConeDegreesFromForward", string.Empty, exoDualCone.m_leftConeDegreesFromForward, true, false, false);
			AbilityMod.AddToken(tokens, this.m_rightConeDegreesFromForwardMod, "RightConeDegreesFromForward", string.Empty, exoDualCone.m_rightConeDegreesFromForward, true, false, false);
			AbilityMod.AddToken(tokens, this.m_interpolateMinAngleMod, "InterpolateMinAngle", string.Empty, exoDualCone.m_interpolateMinAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_interpolateMaxAngleMod, "InterpolateMaxAngle", string.Empty, exoDualCone.m_interpolateMaxAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_interpolateMinDistMod, "InterpolateMinDist", string.Empty, exoDualCone.m_interpolateMinDist, true, false, false);
			AbilityMod.AddToken(tokens, this.m_interpolateMaxDistMod, "InterpolateMaxDist", string.Empty, exoDualCone.m_interpolateMaxDist, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, exoDualCone.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForOverlapMod, "ExtraDamageForOverlap", string.Empty, exoDualCone.m_extraDamageForOverlap, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, exoDualCone.m_extraDamageForSingleHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnHitMod, "EffectOnHit", exoDualCone.m_effectOnHit, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnOverlapHitMod, "EffectOnOverlapHit", exoDualCone.m_effectOnOverlapHit, true);
			AbilityMod.AddToken(tokens, this.m_extraDamageForConsecitiveHitMod, "ExtraDamageForConsecitiveHit", string.Empty, exoDualCone.m_extraDamageForConsecutiveUse, true, false);
			AbilityMod.AddToken(tokens, this.m_extraEnergyForConsecutiveUseMod, "ExtraEnergyForConsecutiveUse", string.Empty, exoDualCone.m_extraEnergyForConsecutiveUse, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ExoDualCone exoDualCone = base.GetTargetAbilityOnAbilityData(abilityData) as ExoDualCone;
		bool flag = exoDualCone != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyConeInfo coneInfoMod = this.m_coneInfoMod;
		string prefix = "[ConeInfo]";
		bool showBaseVal = flag;
		ConeTargetingInfo baseConeInfo;
		if (flag)
		{
			baseConeInfo = exoDualCone.m_coneInfo;
		}
		else
		{
			baseConeInfo = null;
		}
		text = str + base.PropDesc(coneInfoMod, prefix, showBaseVal, baseConeInfo);
		text += base.PropDesc(this.m_leftConeHorizontalOffsetMod, "[LeftConeHorizontalOffset]", flag, (!flag) ? 0f : exoDualCone.m_leftConeHorizontalOffset);
		string str2 = text;
		AbilityModPropertyFloat rightConeHorizontalOffsetMod = this.m_rightConeHorizontalOffsetMod;
		string prefix2 = "[RightConeHorizontalOffset]";
		bool showBaseVal2 = flag;
		float baseVal;
		if (flag)
		{
			baseVal = exoDualCone.m_rightConeHorizontalOffset;
		}
		else
		{
			baseVal = 0f;
		}
		text = str2 + base.PropDesc(rightConeHorizontalOffsetMod, prefix2, showBaseVal2, baseVal);
		string str3 = text;
		AbilityModPropertyFloat coneForwardOffsetMod = this.m_coneForwardOffsetMod;
		string prefix3 = "[ConeForwardOffset]";
		bool showBaseVal3 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = exoDualCone.m_coneForwardOffset;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str3 + base.PropDesc(coneForwardOffsetMod, prefix3, showBaseVal3, baseVal2);
		string str4 = text;
		AbilityModPropertyFloat leftConeDegreesFromForwardMod = this.m_leftConeDegreesFromForwardMod;
		string prefix4 = "[LeftConeDegreesFromForward]";
		bool showBaseVal4 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = exoDualCone.m_leftConeDegreesFromForward;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str4 + base.PropDesc(leftConeDegreesFromForwardMod, prefix4, showBaseVal4, baseVal3);
		string str5 = text;
		AbilityModPropertyFloat rightConeDegreesFromForwardMod = this.m_rightConeDegreesFromForwardMod;
		string prefix5 = "[RightConeDegreesFromForward]";
		bool showBaseVal5 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = exoDualCone.m_rightConeDegreesFromForward;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str5 + base.PropDesc(rightConeDegreesFromForwardMod, prefix5, showBaseVal5, baseVal4);
		string str6 = text;
		AbilityModPropertyBool interpolateAngleMod = this.m_interpolateAngleMod;
		string prefix6 = "[InterpolateAngle]";
		bool showBaseVal6 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = exoDualCone.m_interpolateAngle;
		}
		else
		{
			baseVal5 = false;
		}
		text = str6 + base.PropDesc(interpolateAngleMod, prefix6, showBaseVal6, baseVal5);
		string str7 = text;
		AbilityModPropertyFloat interpolateMinAngleMod = this.m_interpolateMinAngleMod;
		string prefix7 = "[InterpolateMinAngle]";
		bool showBaseVal7 = flag;
		float baseVal6;
		if (flag)
		{
			baseVal6 = exoDualCone.m_interpolateMinAngle;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str7 + base.PropDesc(interpolateMinAngleMod, prefix7, showBaseVal7, baseVal6);
		string str8 = text;
		AbilityModPropertyFloat interpolateMaxAngleMod = this.m_interpolateMaxAngleMod;
		string prefix8 = "[InterpolateMaxAngle]";
		bool showBaseVal8 = flag;
		float baseVal7;
		if (flag)
		{
			baseVal7 = exoDualCone.m_interpolateMaxAngle;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str8 + base.PropDesc(interpolateMaxAngleMod, prefix8, showBaseVal8, baseVal7);
		string str9 = text;
		AbilityModPropertyFloat interpolateMinDistMod = this.m_interpolateMinDistMod;
		string prefix9 = "[InterpolateMinDist]";
		bool showBaseVal9 = flag;
		float baseVal8;
		if (flag)
		{
			baseVal8 = exoDualCone.m_interpolateMinDist;
		}
		else
		{
			baseVal8 = 0f;
		}
		text = str9 + base.PropDesc(interpolateMinDistMod, prefix9, showBaseVal9, baseVal8);
		string str10 = text;
		AbilityModPropertyFloat interpolateMaxDistMod = this.m_interpolateMaxDistMod;
		string prefix10 = "[InterpolateMaxDist]";
		bool showBaseVal10 = flag;
		float baseVal9;
		if (flag)
		{
			baseVal9 = exoDualCone.m_interpolateMaxDist;
		}
		else
		{
			baseVal9 = 0f;
		}
		text = str10 + base.PropDesc(interpolateMaxDistMod, prefix10, showBaseVal10, baseVal9);
		text += base.PropDesc(this.m_damageAmountMod, "[DamageAmount]", flag, (!flag) ? 0 : exoDualCone.m_damageAmount);
		text += base.PropDesc(this.m_extraDamageForOverlapMod, "[ExtraDamageForOverlap]", flag, (!flag) ? 0 : exoDualCone.m_extraDamageForOverlap);
		text += base.PropDesc(this.m_extraDamageForSingleHitMod, "[ExtraDamageForSingleHit]", flag, (!flag) ? 0 : exoDualCone.m_extraDamageForSingleHit);
		string str11 = text;
		AbilityModPropertyEffectInfo effectOnHitMod = this.m_effectOnHitMod;
		string prefix11 = "[EffectOnHit]";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal10;
		if (flag)
		{
			baseVal10 = exoDualCone.m_effectOnHit;
		}
		else
		{
			baseVal10 = null;
		}
		text = str11 + base.PropDesc(effectOnHitMod, prefix11, showBaseVal11, baseVal10);
		text += base.PropDesc(this.m_effectOnOverlapHitMod, "[EffectOnOverlapHit]", flag, (!flag) ? null : exoDualCone.m_effectOnOverlapHit);
		string str12 = text;
		AbilityModPropertyInt extraDamageForConsecitiveHitMod = this.m_extraDamageForConsecitiveHitMod;
		string prefix12 = "[ExtraDamageForConsecitiveHit]";
		bool showBaseVal12 = flag;
		int baseVal11;
		if (flag)
		{
			baseVal11 = exoDualCone.m_extraDamageForConsecutiveUse;
		}
		else
		{
			baseVal11 = 0;
		}
		text = str12 + base.PropDesc(extraDamageForConsecitiveHitMod, prefix12, showBaseVal12, baseVal11);
		return text + base.PropDesc(this.m_extraEnergyForConsecutiveUseMod, "[ExtraEnergyForConsecutiveUse]", flag, (!flag) ? 0 : exoDualCone.m_extraEnergyForConsecutiveUse);
	}
}
