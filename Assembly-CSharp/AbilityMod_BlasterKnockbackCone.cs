using System;
using System.Collections.Generic;
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
		if (!(blasterKnockbackCone != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_minLengthMod, "MinLength", string.Empty, blasterKnockbackCone.m_minLength);
			AbilityMod.AddToken(tokens, m_maxLengthMod, "MaxLength", string.Empty, blasterKnockbackCone.m_maxLength);
			AbilityMod.AddToken(tokens, m_minAngleMod, "MinAngle", string.Empty, blasterKnockbackCone.m_minAngle);
			AbilityMod.AddToken(tokens, m_maxAngleMod, "MaxAngle", string.Empty, blasterKnockbackCone.m_maxAngle);
			AbilityMod.AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, blasterKnockbackCone.m_coneBackwardOffset);
			AbilityMod.AddToken(tokens, m_damageAmountNormalMod, "DamageAmountNormal", string.Empty, blasterKnockbackCone.m_damageAmountNormal);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyEffectNormalMod, "EnemyEffectNormal", blasterKnockbackCone.m_enemyEffectNormal);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyEffectOverchargedMod, "EnemyEffectOvercharged", blasterKnockbackCone.m_enemyEffectOvercharged);
			AbilityMod.AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", string.Empty, blasterKnockbackCone.m_knockbackDistance);
			AbilityMod.AddToken(tokens, m_extraKnockbackDistOnOverchargedMod, "ExtraKnockbackDistOnOvercharged", string.Empty, blasterKnockbackCone.m_extraKnockbackDistOnOvercharged);
			AbilityMod.AddToken(tokens, m_knockbackDistanceOnSelfMod, "KnockbackDistanceOnSelf", string.Empty, blasterKnockbackCone.m_knockbackDistanceOnSelf);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterKnockbackCone blasterKnockbackCone = GetTargetAbilityOnAbilityData(abilityData) as BlasterKnockbackCone;
		bool flag = blasterKnockbackCone != null;
		string empty = string.Empty;
		empty += PropDesc(m_minLengthMod, "[MinLength]", flag, (!flag) ? 0f : blasterKnockbackCone.m_minLength);
		string str = empty;
		AbilityModPropertyFloat maxLengthMod = m_maxLengthMod;
		float baseVal;
		if (flag)
		{
			baseVal = blasterKnockbackCone.m_maxLength;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(maxLengthMod, "[MaxLength]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat minAngleMod = m_minAngleMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = blasterKnockbackCone.m_minAngle;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(minAngleMod, "[MinAngle]", flag, baseVal2);
		empty += PropDesc(m_maxAngleMod, "[MaxAngle]", flag, (!flag) ? 0f : blasterKnockbackCone.m_maxAngle);
		string str3 = empty;
		AbilityModPropertyFloat coneBackwardOffsetMod = m_coneBackwardOffsetMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = blasterKnockbackCone.m_coneBackwardOffset;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, baseVal3);
		empty += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, flag && blasterKnockbackCone.m_penetrateLineOfSight);
		string str4 = empty;
		AbilityModPropertyInt damageAmountNormalMod = m_damageAmountNormalMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = blasterKnockbackCone.m_damageAmountNormal;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(damageAmountNormalMod, "[DamageAmountNormal]", flag, baseVal4);
		empty += PropDesc(m_enemyEffectNormalMod, "[EnemyEffectNormal]", flag, (!flag) ? null : blasterKnockbackCone.m_enemyEffectNormal);
		string str5 = empty;
		AbilityModPropertyEffectInfo enemyEffectOverchargedMod = m_enemyEffectOverchargedMod;
		object baseVal5;
		if (flag)
		{
			baseVal5 = blasterKnockbackCone.m_enemyEffectOvercharged;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(enemyEffectOverchargedMod, "[EnemyEffectOvercharged]", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyFloat knockbackDistanceMod = m_knockbackDistanceMod;
		float baseVal6;
		if (flag)
		{
			baseVal6 = blasterKnockbackCone.m_knockbackDistance;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + PropDesc(knockbackDistanceMod, "[KnockbackDistance]", flag, baseVal6);
		empty += PropDesc(m_extraKnockbackDistOnOverchargedMod, "[ExtraKnockbackDistOnOvercharged]", flag, (!flag) ? 0f : blasterKnockbackCone.m_extraKnockbackDistOnOvercharged);
		string str7 = empty;
		AbilityModPropertyFloat knockbackDistanceOnSelfMod = m_knockbackDistanceOnSelfMod;
		float baseVal7;
		if (flag)
		{
			baseVal7 = blasterKnockbackCone.m_knockbackDistanceOnSelf;
		}
		else
		{
			baseVal7 = 0f;
		}
		empty = str7 + PropDesc(knockbackDistanceOnSelfMod, "[KnockbackDistanceOnSelf]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyBool overchargeAsFreeActionAfterCastMod = m_overchargeAsFreeActionAfterCastMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = (blasterKnockbackCone.m_overchargeAsFreeActionAfterCast ? 1 : 0);
		}
		else
		{
			baseVal8 = 0;
		}
		return str8 + PropDesc(overchargeAsFreeActionAfterCastMod, "[OverchargeAsFreeActionAfterCast]", flag, (byte)baseVal8 != 0);
	}
}
