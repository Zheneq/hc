using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_BlasterKnockbackCone : AbilityMod
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
	public AbilityModPropertyEffectInfo m_enemyEffectNormalMod;
	public AbilityModPropertyEffectInfo m_enemyEffectOverchargedMod;
	[Header("-- Knockback on Enemy")]
	public AbilityModPropertyFloat m_knockbackDistanceMod;
	public AbilityModPropertyFloat m_extraKnockbackDistOnOverchargedMod;
	[Header("-- Knockback on Self")]
	public AbilityModPropertyFloat m_knockbackDistanceOnSelfMod;
	[Header("-- Set Overcharge as Free Action after cast?")]
	public AbilityModPropertyBool m_overchargeAsFreeActionAfterCastMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(BlasterKnockbackCone);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BlasterKnockbackCone blasterKnockbackCone = targetAbility as BlasterKnockbackCone;
		if (blasterKnockbackCone == null)
		{
			return;
		}
		AddToken(tokens, m_minLengthMod, "MinLength", string.Empty, blasterKnockbackCone.m_minLength);
		AddToken(tokens, m_maxLengthMod, "MaxLength", string.Empty, blasterKnockbackCone.m_maxLength);
		AddToken(tokens, m_minAngleMod, "MinAngle", string.Empty, blasterKnockbackCone.m_minAngle);
		AddToken(tokens, m_maxAngleMod, "MaxAngle", string.Empty, blasterKnockbackCone.m_maxAngle);
		AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, blasterKnockbackCone.m_coneBackwardOffset);
		AddToken(tokens, m_damageAmountNormalMod, "DamageAmountNormal", string.Empty, blasterKnockbackCone.m_damageAmountNormal);
		AddToken_EffectMod(tokens, m_enemyEffectNormalMod, "EnemyEffectNormal", blasterKnockbackCone.m_enemyEffectNormal);
		AddToken_EffectMod(tokens, m_enemyEffectOverchargedMod, "EnemyEffectOvercharged", blasterKnockbackCone.m_enemyEffectOvercharged);
		AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", string.Empty, blasterKnockbackCone.m_knockbackDistance);
		AddToken(tokens, m_extraKnockbackDistOnOverchargedMod, "ExtraKnockbackDistOnOvercharged", string.Empty, blasterKnockbackCone.m_extraKnockbackDistOnOvercharged);
		AddToken(tokens, m_knockbackDistanceOnSelfMod, "KnockbackDistanceOnSelf", string.Empty, blasterKnockbackCone.m_knockbackDistanceOnSelf);
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterKnockbackCone blasterKnockbackCone = GetTargetAbilityOnAbilityData(abilityData) as BlasterKnockbackCone;
		bool isAbilityPresent = blasterKnockbackCone != null;
		string desc = string.Empty;
		desc += PropDesc(m_minLengthMod, "[MinLength]", isAbilityPresent, isAbilityPresent ? blasterKnockbackCone.m_minLength : 0f);
		desc += PropDesc(m_maxLengthMod, "[MaxLength]", isAbilityPresent, isAbilityPresent ? blasterKnockbackCone.m_maxLength : 0f);
		desc += PropDesc(m_minAngleMod, "[MinAngle]", isAbilityPresent, isAbilityPresent ? blasterKnockbackCone.m_minAngle : 0f);
		desc += PropDesc(m_maxAngleMod, "[MaxAngle]", isAbilityPresent, isAbilityPresent ? blasterKnockbackCone.m_maxAngle : 0f);
		desc += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", isAbilityPresent, isAbilityPresent ? blasterKnockbackCone.m_coneBackwardOffset : 0f);
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isAbilityPresent, isAbilityPresent && blasterKnockbackCone.m_penetrateLineOfSight);
		desc += PropDesc(m_damageAmountNormalMod, "[DamageAmountNormal]", isAbilityPresent, isAbilityPresent ? blasterKnockbackCone.m_damageAmountNormal : 0);
		desc += PropDesc(m_enemyEffectNormalMod, "[EnemyEffectNormal]", isAbilityPresent, isAbilityPresent ? blasterKnockbackCone.m_enemyEffectNormal : null);
		desc += PropDesc(m_enemyEffectOverchargedMod, "[EnemyEffectOvercharged]", isAbilityPresent, isAbilityPresent ? blasterKnockbackCone.m_enemyEffectOvercharged : null);
		desc += PropDesc(m_knockbackDistanceMod, "[KnockbackDistance]", isAbilityPresent, isAbilityPresent ? blasterKnockbackCone.m_knockbackDistance : 0f);
		desc += PropDesc(m_extraKnockbackDistOnOverchargedMod, "[ExtraKnockbackDistOnOvercharged]", isAbilityPresent, isAbilityPresent ? blasterKnockbackCone.m_extraKnockbackDistOnOvercharged : 0f);
		desc += PropDesc(m_knockbackDistanceOnSelfMod, "[KnockbackDistanceOnSelf]", isAbilityPresent, isAbilityPresent ? blasterKnockbackCone.m_knockbackDistanceOnSelf : 0f);
		return new StringBuilder().Append(desc).Append(PropDesc(m_overchargeAsFreeActionAfterCastMod, "[OverchargeAsFreeActionAfterCast]", isAbilityPresent, isAbilityPresent && blasterKnockbackCone.m_overchargeAsFreeActionAfterCast)).ToString();
	}
}
