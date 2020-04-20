using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SamuraiDoubleSlash : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	public AbilityModPropertyFloat m_maxAngleBetweenMod;

	[Header("    Cone(s)")]
	public AbilityModPropertyFloat m_coneWidthAngleMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	public AbilityModPropertyFloat m_coneLengthMod;

	[Header("    Laser(s)")]
	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserLengthMod;

	[Header("-- On Hit Damage/Effect")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyInt m_overlapExtraDamageMod;

	public AbilityModPropertyEffectInfo m_targetHitEffectMod;

	[Header("-- Extra Effect if SelfBuff ability is used on same turn")]
	public AbilityModPropertyEffectInfo m_extraEnemyHitEffectIfSelfBuffedMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SamuraiDoubleSlash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SamuraiDoubleSlash samuraiDoubleSlash = targetAbility as SamuraiDoubleSlash;
		if (samuraiDoubleSlash != null)
		{
			AbilityMod.AddToken(tokens, this.m_maxAngleBetweenMod, "MaxAngleBetween", string.Empty, samuraiDoubleSlash.m_maxAngleBetween, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, samuraiDoubleSlash.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, samuraiDoubleSlash.m_coneBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneLengthMod, "ConeLength", string.Empty, samuraiDoubleSlash.m_coneLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, samuraiDoubleSlash.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserLengthMod, "LaserLength", string.Empty, samuraiDoubleSlash.m_laserLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, samuraiDoubleSlash.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_overlapExtraDamageMod, "OverlapExtraDamage", string.Empty, samuraiDoubleSlash.m_overlapExtraDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_targetHitEffectMod, "TargetHitEffect", samuraiDoubleSlash.m_targetHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_extraEnemyHitEffectIfSelfBuffedMod, "ExtraEnemyHitEffectIfSelfBuffed", samuraiDoubleSlash.m_extraEnemyHitEffectIfSelfBuffed, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiDoubleSlash samuraiDoubleSlash = base.GetTargetAbilityOnAbilityData(abilityData) as SamuraiDoubleSlash;
		bool flag = samuraiDoubleSlash != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, flag && samuraiDoubleSlash.m_penetrateLineOfSight);
		string str = text;
		AbilityModPropertyFloat maxAngleBetweenMod = this.m_maxAngleBetweenMod;
		string prefix = "[MaxAngleBetween]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = samuraiDoubleSlash.m_maxAngleBetween;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(maxAngleBetweenMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_coneWidthAngleMod, "[ConeWidthAngle]", flag, (!flag) ? 0f : samuraiDoubleSlash.m_coneWidthAngle);
		text += base.PropDesc(this.m_coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, (!flag) ? 0f : samuraiDoubleSlash.m_coneBackwardOffset);
		string str2 = text;
		AbilityModPropertyFloat coneLengthMod = this.m_coneLengthMod;
		string prefix2 = "[ConeLength]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = samuraiDoubleSlash.m_coneLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(coneLengthMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : samuraiDoubleSlash.m_laserWidth);
		text += base.PropDesc(this.m_laserLengthMod, "[LaserLength]", flag, (!flag) ? 0f : samuraiDoubleSlash.m_laserLength);
		text += base.PropDesc(this.m_damageAmountMod, "[DamageAmount]", flag, (!flag) ? 0 : samuraiDoubleSlash.m_damageAmount);
		text += base.PropDesc(this.m_overlapExtraDamageMod, "[OverlapExtraDamage]", flag, (!flag) ? 0 : samuraiDoubleSlash.m_overlapExtraDamage);
		string str3 = text;
		AbilityModPropertyEffectInfo targetHitEffectMod = this.m_targetHitEffectMod;
		string prefix3 = "[TargetHitEffect]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
		if (flag)
		{
			baseVal3 = samuraiDoubleSlash.m_targetHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		text = str3 + base.PropDesc(targetHitEffectMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyEffectInfo extraEnemyHitEffectIfSelfBuffedMod = this.m_extraEnemyHitEffectIfSelfBuffedMod;
		string prefix4 = "[ExtraEnemyHitEffectIfSelfBuffed]";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
		if (flag)
		{
			baseVal4 = samuraiDoubleSlash.m_extraEnemyHitEffectIfSelfBuffed;
		}
		else
		{
			baseVal4 = null;
		}
		return str4 + base.PropDesc(extraEnemyHitEffectIfSelfBuffedMod, prefix4, showBaseVal4, baseVal4);
	}
}
