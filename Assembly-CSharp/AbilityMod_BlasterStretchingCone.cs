using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_BlasterStretchingCone : AbilityMod
{
	[Header("-- Cone Limits")]
	public AbilityModPropertyFloat m_minLengthMod;
	public AbilityModPropertyFloat m_maxLengthMod;
	public AbilityModPropertyFloat m_minAngleMod;
	public AbilityModPropertyFloat m_maxAngleMod;
	public AbilityModPropertyFloat m_coneBackwardOffsetMod;
	public AbilityModPropertyBool m_penetrateLineOfSightMod;
	[Header("-- On Hit")]
	public AbilityModPropertyInt m_damageAmountNormalMod;
	public AbilityModPropertyInt m_extraDamageForSingleHitMod;
	public AbilityModPropertyFloat m_extraDamagePerSquareDistanceFromEnemyMod;
	[Header("-- Damage Change by Angle/Distance")]
	public AbilityModPropertyInt m_anglesPerDamageChangeMod;
	public AbilityModPropertyFloat m_distPerDamageChangeMod;
	public AbilityModPropertyInt m_maxDamageChangeMod;
	[Header("-- Effects On Hit")]
	public AbilityModPropertyEffectInfo m_normalEnemyEffectMod;
	public AbilityModPropertyEffectInfo m_overchargedEnemyEffectMod;
	public AbilityModPropertyEffectInfo m_singleEnemyHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(BlasterStretchingCone);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BlasterStretchingCone blasterStretchingCone = targetAbility as BlasterStretchingCone;
		if (blasterStretchingCone == null)
		{
			return;
		}
		AddToken(tokens, m_minLengthMod, "MinLength", string.Empty, blasterStretchingCone.m_minLength);
		AddToken(tokens, m_maxLengthMod, "MaxLength", string.Empty, blasterStretchingCone.m_maxLength);
		AddToken(tokens, m_minAngleMod, "MinAngle", string.Empty, blasterStretchingCone.m_minAngle);
		AddToken(tokens, m_maxAngleMod, "MaxAngle", string.Empty, blasterStretchingCone.m_maxAngle);
		AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, blasterStretchingCone.m_coneBackwardOffset);
		AddToken(tokens, m_damageAmountNormalMod, "DamageAmountNormal", string.Empty, blasterStretchingCone.m_damageAmountNormal);
		AddToken(tokens, m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, blasterStretchingCone.m_extraDamageForSingleHit);
		AddToken(tokens, m_extraDamagePerSquareDistanceFromEnemyMod, "ExtraDamagePerSquareDistanceFromEnemy", string.Empty, blasterStretchingCone.m_extraDamagePerSquareDistanceFromEnemy);
		AddToken(tokens, m_anglesPerDamageChangeMod, "AnglesPerDamageChange", string.Empty, blasterStretchingCone.m_anglesPerDamageChange);
		AddToken(tokens, m_maxDamageChangeMod, "MaxDamageChange", string.Empty, blasterStretchingCone.m_maxDamageChange);
		AddToken_EffectMod(tokens, m_normalEnemyEffectMod, "NormalEnemyEffect", blasterStretchingCone.m_normalEnemyEffect);
		AddToken_EffectMod(tokens, m_overchargedEnemyEffectMod, "OverchargedEnemyEffect", blasterStretchingCone.m_overchargedEnemyEffect);
		AddToken_EffectMod(tokens, m_singleEnemyHitEffectMod, "SingleEnemyHitEffect", blasterStretchingCone.m_singleEnemyHitEffect);
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterStretchingCone blasterStretchingCone = GetTargetAbilityOnAbilityData(abilityData) as BlasterStretchingCone;
		bool isAbilityPresent = blasterStretchingCone != null;
		string desc = string.Empty;
		desc += PropDesc(m_minLengthMod, "[MinLength]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_minLength : 0f);
		desc += PropDesc(m_maxLengthMod, "[MaxLength]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_maxLength : 0f);
		desc += PropDesc(m_minAngleMod, "[MinAngle]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_minAngle : 0f);
		desc += PropDesc(m_maxAngleMod, "[MaxAngle]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_maxAngle : 0f);
		desc += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_coneBackwardOffset : 0f);
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isAbilityPresent, isAbilityPresent && blasterStretchingCone.m_penetrateLineOfSight);
		desc += PropDesc(m_damageAmountNormalMod, "[DamageAmountNormal]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_damageAmountNormal : 0);
		desc += PropDesc(m_extraDamageForSingleHitMod, "[ExtraDamageForSingleHit]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_extraDamageForSingleHit : 0);
		desc += PropDesc(m_extraDamagePerSquareDistanceFromEnemyMod, "[ExtraDamagePerSquareDistanceFromEnemy]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_extraDamagePerSquareDistanceFromEnemy : 0f);
		desc += PropDesc(m_anglesPerDamageChangeMod, "[AnglesPerDamageChange]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_anglesPerDamageChange : 0);
		desc += PropDesc(m_distPerDamageChangeMod, "[DistPerDamageChange]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_distPerDamageChange : 0f);
		desc += PropDesc(m_maxDamageChangeMod, "[MaxDamageChange]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_maxDamageChange : 0);
		desc += PropDesc(m_normalEnemyEffectMod, "[NormalEnemyEffect]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_normalEnemyEffect : null);
		desc += PropDesc(m_overchargedEnemyEffectMod, "[OverchargedEnemyEffect]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_overchargedEnemyEffect : null);
		return new StringBuilder().Append(desc).Append(PropDesc(m_singleEnemyHitEffectMod, "[SingleEnemyHitEffect]", isAbilityPresent, isAbilityPresent ? blasterStretchingCone.m_singleEnemyHitEffect : null)).ToString();
	}
}
