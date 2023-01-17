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
			AddToken_ConeInfo(tokens, m_coneInfoMod, "ConeInfo", exoDualCone.m_coneInfo);
			AddToken(tokens, m_leftConeHorizontalOffsetMod, "LeftConeHorizontalOffset", string.Empty, exoDualCone.m_leftConeHorizontalOffset);
			AddToken(tokens, m_rightConeHorizontalOffsetMod, "RightConeHorizontalOffset", string.Empty, exoDualCone.m_rightConeHorizontalOffset);
			AddToken(tokens, m_coneForwardOffsetMod, "ConeForwardOffset", string.Empty, exoDualCone.m_coneForwardOffset);
			AddToken(tokens, m_leftConeDegreesFromForwardMod, "LeftConeDegreesFromForward", string.Empty, exoDualCone.m_leftConeDegreesFromForward);
			AddToken(tokens, m_rightConeDegreesFromForwardMod, "RightConeDegreesFromForward", string.Empty, exoDualCone.m_rightConeDegreesFromForward);
			AddToken(tokens, m_interpolateMinAngleMod, "InterpolateMinAngle", string.Empty, exoDualCone.m_interpolateMinAngle);
			AddToken(tokens, m_interpolateMaxAngleMod, "InterpolateMaxAngle", string.Empty, exoDualCone.m_interpolateMaxAngle);
			AddToken(tokens, m_interpolateMinDistMod, "InterpolateMinDist", string.Empty, exoDualCone.m_interpolateMinDist);
			AddToken(tokens, m_interpolateMaxDistMod, "InterpolateMaxDist", string.Empty, exoDualCone.m_interpolateMaxDist);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, exoDualCone.m_damageAmount);
			AddToken(tokens, m_extraDamageForOverlapMod, "ExtraDamageForOverlap", string.Empty, exoDualCone.m_extraDamageForOverlap);
			AddToken(tokens, m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, exoDualCone.m_extraDamageForSingleHit);
			AddToken_EffectMod(tokens, m_effectOnHitMod, "EffectOnHit", exoDualCone.m_effectOnHit);
			AddToken_EffectMod(tokens, m_effectOnOverlapHitMod, "EffectOnOverlapHit", exoDualCone.m_effectOnOverlapHit);
			AddToken(tokens, m_extraDamageForConsecitiveHitMod, "ExtraDamageForConsecitiveHit", string.Empty, exoDualCone.m_extraDamageForConsecutiveUse);
			AddToken(tokens, m_extraEnergyForConsecutiveUseMod, "ExtraEnergyForConsecutiveUse", string.Empty, exoDualCone.m_extraEnergyForConsecutiveUse);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ExoDualCone exoDualCone = GetTargetAbilityOnAbilityData(abilityData) as ExoDualCone;
		bool isValid = exoDualCone != null;
		string desc = string.Empty;
		desc += PropDesc(m_coneInfoMod, "[ConeInfo]", isValid, isValid ? exoDualCone.m_coneInfo : null);
		desc += PropDesc(m_leftConeHorizontalOffsetMod, "[LeftConeHorizontalOffset]", isValid, isValid ? exoDualCone.m_leftConeHorizontalOffset : 0f);
		desc += PropDesc(m_rightConeHorizontalOffsetMod, "[RightConeHorizontalOffset]", isValid, isValid ? exoDualCone.m_rightConeHorizontalOffset : 0f);
		desc += PropDesc(m_coneForwardOffsetMod, "[ConeForwardOffset]", isValid, isValid ? exoDualCone.m_coneForwardOffset : 0f);
		desc += PropDesc(m_leftConeDegreesFromForwardMod, "[LeftConeDegreesFromForward]", isValid, isValid ? exoDualCone.m_leftConeDegreesFromForward : 0f);
		desc += PropDesc(m_rightConeDegreesFromForwardMod, "[RightConeDegreesFromForward]", isValid, isValid ? exoDualCone.m_rightConeDegreesFromForward : 0f);
		desc += PropDesc(m_interpolateAngleMod, "[InterpolateAngle]", isValid, isValid && exoDualCone.m_interpolateAngle);
		desc += PropDesc(m_interpolateMinAngleMod, "[InterpolateMinAngle]", isValid, isValid ? exoDualCone.m_interpolateMinAngle : 0f);
		desc += PropDesc(m_interpolateMaxAngleMod, "[InterpolateMaxAngle]", isValid, isValid ? exoDualCone.m_interpolateMaxAngle : 0f);
		desc += PropDesc(m_interpolateMinDistMod, "[InterpolateMinDist]", isValid, isValid ? exoDualCone.m_interpolateMinDist : 0f);
		desc += PropDesc(m_interpolateMaxDistMod, "[InterpolateMaxDist]", isValid, isValid ? exoDualCone.m_interpolateMaxDist : 0f);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? exoDualCone.m_damageAmount : 0);
		desc += PropDesc(m_extraDamageForOverlapMod, "[ExtraDamageForOverlap]", isValid, isValid ? exoDualCone.m_extraDamageForOverlap : 0);
		desc += PropDesc(m_extraDamageForSingleHitMod, "[ExtraDamageForSingleHit]", isValid, isValid ? exoDualCone.m_extraDamageForSingleHit : 0);
		desc += PropDesc(m_effectOnHitMod, "[EffectOnHit]", isValid, isValid ? exoDualCone.m_effectOnHit : null);
		desc += PropDesc(m_effectOnOverlapHitMod, "[EffectOnOverlapHit]", isValid, isValid ? exoDualCone.m_effectOnOverlapHit : null);
		desc += PropDesc(m_extraDamageForConsecitiveHitMod, "[ExtraDamageForConsecitiveHit]", isValid, isValid ? exoDualCone.m_extraDamageForConsecutiveUse : 0);
		return desc + PropDesc(m_extraEnergyForConsecutiveUseMod, "[ExtraEnergyForConsecutiveUse]", isValid, isValid ? exoDualCone.m_extraEnergyForConsecutiveUse : 0);
	}
}
