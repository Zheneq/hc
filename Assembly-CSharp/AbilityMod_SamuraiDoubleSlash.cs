using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken(tokens, m_maxAngleBetweenMod, "MaxAngleBetween", string.Empty, samuraiDoubleSlash.m_maxAngleBetween);
			AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, samuraiDoubleSlash.m_coneWidthAngle);
			AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, samuraiDoubleSlash.m_coneBackwardOffset);
			AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, samuraiDoubleSlash.m_coneLength);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, samuraiDoubleSlash.m_laserWidth);
			AddToken(tokens, m_laserLengthMod, "LaserLength", string.Empty, samuraiDoubleSlash.m_laserLength);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, samuraiDoubleSlash.m_damageAmount);
			AddToken(tokens, m_overlapExtraDamageMod, "OverlapExtraDamage", string.Empty, samuraiDoubleSlash.m_overlapExtraDamage);
			AddToken_EffectMod(tokens, m_targetHitEffectMod, "TargetHitEffect", samuraiDoubleSlash.m_targetHitEffect);
			AddToken_EffectMod(tokens, m_extraEnemyHitEffectIfSelfBuffedMod, "ExtraEnemyHitEffectIfSelfBuffed", samuraiDoubleSlash.m_extraEnemyHitEffectIfSelfBuffed);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiDoubleSlash samuraiDoubleSlash = GetTargetAbilityOnAbilityData(abilityData) as SamuraiDoubleSlash;
		bool isValid = samuraiDoubleSlash != null;
		string desc = string.Empty;
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isValid, isValid && samuraiDoubleSlash.m_penetrateLineOfSight);
		desc += PropDesc(m_maxAngleBetweenMod, "[MaxAngleBetween]", isValid, isValid ? samuraiDoubleSlash.m_maxAngleBetween : 0f);
		desc += PropDesc(m_coneWidthAngleMod, "[ConeWidthAngle]", isValid, isValid ? samuraiDoubleSlash.m_coneWidthAngle : 0f);
		desc += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", isValid, isValid ? samuraiDoubleSlash.m_coneBackwardOffset : 0f);
		desc += PropDesc(m_coneLengthMod, "[ConeLength]", isValid, isValid ? samuraiDoubleSlash.m_coneLength : 0f);
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isValid, isValid ? samuraiDoubleSlash.m_laserWidth : 0f);
		desc += PropDesc(m_laserLengthMod, "[LaserLength]", isValid, isValid ? samuraiDoubleSlash.m_laserLength : 0f);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? samuraiDoubleSlash.m_damageAmount : 0);
		desc += PropDesc(m_overlapExtraDamageMod, "[OverlapExtraDamage]", isValid, isValid ? samuraiDoubleSlash.m_overlapExtraDamage : 0);
		desc += PropDesc(m_targetHitEffectMod, "[TargetHitEffect]", isValid, isValid ? samuraiDoubleSlash.m_targetHitEffect : null);
		return new StringBuilder().Append(desc).Append(PropDesc(m_extraEnemyHitEffectIfSelfBuffedMod, "[ExtraEnemyHitEffectIfSelfBuffed]", isValid, isValid ? samuraiDoubleSlash.m_extraEnemyHitEffectIfSelfBuffed : null)).ToString();
	}
}
