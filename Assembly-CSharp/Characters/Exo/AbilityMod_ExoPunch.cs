// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ExoPunch : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_coneWidthAngleMod;
	public AbilityModPropertyFloat m_coneBackwardOffsetMod;
	public AbilityModPropertyFloat m_coneLengthMod;
	public AbilityModPropertyBool m_penetrateLineOfSightMod;
	// TODO EXO not actually used (set to ignore in all assets)
	public AbilityModPropertyInt m_maxTargetsMod;
	[Header("-- Knockback")]
	public AbilityModPropertyFloat m_knockbackDistanceMod;
	public AbilityModPropertyKnockbackType m_knockbackTypeMod;
	[Header("-- On Hit Damage/Effect")]
	public AbilityModPropertyInt m_damageAmountMod;
	public AbilityModPropertyEffectInfo m_targetHitEffectMod;
	[Header("-- Nearby Hit Bonus")]
	public AbilityModPropertyFloat m_nearDistThresholdMod;
	public AbilityModPropertyInt m_nearEnemyExtraDamageMod;
	public AbilityModPropertyEffectInfo m_nearEnemyExtraEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ExoPunch);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ExoPunch exoPunch = targetAbility as ExoPunch;
		if (exoPunch != null)
		{
			AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, exoPunch.m_coneWidthAngle);
			AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, exoPunch.m_coneBackwardOffset);
			AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, exoPunch.m_coneLength);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, exoPunch.m_maxTargets);
			AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", string.Empty, exoPunch.m_knockbackDistance);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, exoPunch.m_damageAmount);
			AddToken_EffectMod(tokens, m_targetHitEffectMod, "TargetHitEffect", exoPunch.m_targetHitEffect);
			AddToken(tokens, m_nearDistThresholdMod, "NearDistThreshold", string.Empty, exoPunch.m_nearDistThreshold);
			AddToken(tokens, m_nearEnemyExtraDamageMod, "NearEnemyExtraDamage", string.Empty, exoPunch.m_nearEnemyExtraDamage);
			AddToken_EffectMod(tokens, m_nearEnemyExtraEffectMod, "NearEnemyExtraEffect", exoPunch.m_nearEnemyExtraEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		ExoPunch exoPunch = GetTargetAbilityOnAbilityData(abilityData) as ExoPunch;
		// rogues
		// ExoPunch exoPunch = targetAbility as ExoPunch;
		
		bool isValid = exoPunch != null;
		string desc = string.Empty;
		desc += PropDesc(m_coneWidthAngleMod, "[ConeWidthAngle]", isValid, isValid ? exoPunch.m_coneWidthAngle : 0f);
		desc += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", isValid, isValid ? exoPunch.m_coneBackwardOffset : 0f);
		desc += PropDesc(m_coneLengthMod, "[ConeLength]", isValid, isValid ? exoPunch.m_coneLength : 0f);
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isValid, isValid && exoPunch.m_penetrateLineOfSight);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? exoPunch.m_maxTargets : 0);
		desc += PropDesc(m_knockbackDistanceMod, "[KnockbackDistance]", isValid, isValid ? exoPunch.m_knockbackDistance : 0f);
		desc += PropDesc(m_knockbackTypeMod, "[KnockbackType]", isValid, isValid ? exoPunch.m_knockbackType : KnockbackType.AwayFromSource);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? exoPunch.m_damageAmount : 0);
		desc += PropDesc(m_targetHitEffectMod, "[TargetHitEffect]", isValid, isValid ? exoPunch.m_targetHitEffect : null);
		desc += PropDesc(m_nearDistThresholdMod, "[NearDistThreshold]", isValid, isValid ? exoPunch.m_nearDistThreshold : 0f);
		desc += PropDesc(m_nearEnemyExtraDamageMod, "[NearEnemyExtraDamage]", isValid, isValid ? exoPunch.m_nearEnemyExtraDamage : 0);
		return desc + PropDesc(m_nearEnemyExtraEffectMod, "[NearEnemyExtraEffect]", isValid, isValid ? exoPunch.m_nearEnemyExtraEffect : null);
	}
}
