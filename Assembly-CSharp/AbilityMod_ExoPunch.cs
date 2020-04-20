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
			AbilityMod.AddToken(tokens, this.m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, exoPunch.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, exoPunch.m_coneBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneLengthMod, "ConeLength", string.Empty, exoPunch.m_coneLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, exoPunch.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_knockbackDistanceMod, "KnockbackDistance", string.Empty, exoPunch.m_knockbackDistance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, exoPunch.m_damageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_targetHitEffectMod, "TargetHitEffect", exoPunch.m_targetHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_nearDistThresholdMod, "NearDistThreshold", string.Empty, exoPunch.m_nearDistThreshold, true, false, false);
			AbilityMod.AddToken(tokens, this.m_nearEnemyExtraDamageMod, "NearEnemyExtraDamage", string.Empty, exoPunch.m_nearEnemyExtraDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_nearEnemyExtraEffectMod, "NearEnemyExtraEffect", exoPunch.m_nearEnemyExtraEffect, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ExoPunch exoPunch = base.GetTargetAbilityOnAbilityData(abilityData) as ExoPunch;
		bool flag = exoPunch != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_coneWidthAngleMod, "[ConeWidthAngle]", flag, (!flag) ? 0f : exoPunch.m_coneWidthAngle);
		string str = text;
		AbilityModPropertyFloat coneBackwardOffsetMod = this.m_coneBackwardOffsetMod;
		string prefix = "[ConeBackwardOffset]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = exoPunch.m_coneBackwardOffset;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(coneBackwardOffsetMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_coneLengthMod, "[ConeLength]", flag, (!flag) ? 0f : exoPunch.m_coneLength);
		string str2 = text;
		AbilityModPropertyBool penetrateLineOfSightMod = this.m_penetrateLineOfSightMod;
		string prefix2 = "[PenetrateLineOfSight]";
		bool showBaseVal2 = flag;
		bool baseVal2;
		if (flag)
		{
			baseVal2 = exoPunch.m_penetrateLineOfSight;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(penetrateLineOfSightMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt maxTargetsMod = this.m_maxTargetsMod;
		string prefix3 = "[MaxTargets]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = exoPunch.m_maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(maxTargetsMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat knockbackDistanceMod = this.m_knockbackDistanceMod;
		string prefix4 = "[KnockbackDistance]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = exoPunch.m_knockbackDistance;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(knockbackDistanceMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_knockbackTypeMod, "[KnockbackType]", flag, (!flag) ? KnockbackType.AwayFromSource : exoPunch.m_knockbackType);
		string str5 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix5 = "[DamageAmount]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = exoPunch.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(damageAmountMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo targetHitEffectMod = this.m_targetHitEffectMod;
		string prefix6 = "[TargetHitEffect]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
		if (flag)
		{
			baseVal6 = exoPunch.m_targetHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(targetHitEffectMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_nearDistThresholdMod, "[NearDistThreshold]", flag, (!flag) ? 0f : exoPunch.m_nearDistThreshold);
		string str7 = text;
		AbilityModPropertyInt nearEnemyExtraDamageMod = this.m_nearEnemyExtraDamageMod;
		string prefix7 = "[NearEnemyExtraDamage]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = exoPunch.m_nearEnemyExtraDamage;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(nearEnemyExtraDamageMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyEffectInfo nearEnemyExtraEffectMod = this.m_nearEnemyExtraEffectMod;
		string prefix8 = "[NearEnemyExtraEffect]";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
		if (flag)
		{
			baseVal8 = exoPunch.m_nearEnemyExtraEffect;
		}
		else
		{
			baseVal8 = null;
		}
		return str8 + base.PropDesc(nearEnemyExtraEffectMod, prefix8, showBaseVal8, baseVal8);
	}
}
