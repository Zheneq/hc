// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_MantaDashThroughWall : AbilityMod
{
	[Header("-- Charge Targeting")]
	public AbilityModPropertyFloat m_aoeConeWidthMod;
	public AbilityModPropertyFloat m_aoeConeLengthMod;
	public AbilityModPropertyFloat m_aoeThroughWallConeWidthMod;
	public AbilityModPropertyFloat m_aoeThroughWallConeLengthMod;
	public AbilityModPropertyFloat m_widthMod;
	public AbilityModPropertyFloat m_maxRangeMod;
	public AbilityModPropertyFloat m_maxWidthOfWallMod;
	public AbilityModPropertyFloat m_extraTotalDistanceIfThroughWallsMod;
	public AbilityModPropertyBool m_clampConeToWallMod;
	public AbilityModPropertyBool m_aoeWithMissMod;
	public AbilityModPropertyFloat m_coneBackwardOffsetMod;
	[Header("-- Normal On Hit Damage, Effect, etc")]
	public AbilityModPropertyInt m_directHitDamageMod;
	public AbilityModPropertyEffectInfo m_directEnemyHitEffectMod;
	public AbilityModPropertyBool m_directHitIgnoreCoverMod;
	[Space(10f)]
	public AbilityModPropertyInt m_aoeDamageMod;
	public AbilityModPropertyEffectInfo m_aoeEnemyHitEffectMod;
	[Space(10f)]
	public AbilityModPropertyInt m_aoeThroughWallsDamageMod;
	public AbilityModPropertyEffectInfo m_aoeThroughWallsEffectMod;
	[Header("-- Other")]
	public AbilityModPropertyEffectInfo m_additionalDirtyFightingExplosionEffect;
	public AbilityModCooldownReduction m_cooldownReductionsWhenNoHits;

	public override Type GetTargetAbilityType()
	{
		return typeof(MantaDashThroughWall);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MantaDashThroughWall mantaDashThroughWall = targetAbility as MantaDashThroughWall;
		if (mantaDashThroughWall != null)
		{
			AddToken(tokens, m_aoeConeWidthMod, "AoeConeWidth", string.Empty, mantaDashThroughWall.m_aoeConeWidth);
			AddToken(tokens, m_aoeConeLengthMod, "AoeConeLength", string.Empty, mantaDashThroughWall.m_aoeConeLength);
			AddToken(tokens, m_aoeThroughWallConeWidthMod, "AoeThroughWallConeWidth", string.Empty, mantaDashThroughWall.m_aoeThroughWallConeWidth);
			AddToken(tokens, m_aoeThroughWallConeLengthMod, "AoeThroughWallConeLength", string.Empty, mantaDashThroughWall.m_aoeThroughWallConeLength);
			AddToken(tokens, m_widthMod, "Width", string.Empty, mantaDashThroughWall.m_width);
			AddToken(tokens, m_maxRangeMod, "MaxRange", string.Empty, mantaDashThroughWall.m_maxRange);
			AddToken(tokens, m_maxWidthOfWallMod, "MaxWidthOfWall", string.Empty, mantaDashThroughWall.m_maxWidthOfWall);
			AddToken(tokens, m_extraTotalDistanceIfThroughWallsMod, "ExtraTotalDistanceIfThroughWalls", string.Empty, mantaDashThroughWall.m_extraTotalDistanceIfThroughWalls);
			AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, mantaDashThroughWall.m_coneBackwardOffset);
			AddToken(tokens, m_directHitDamageMod, "DirectHitDamage", string.Empty, mantaDashThroughWall.m_directHitDamage);
			AddToken_EffectMod(tokens, m_directEnemyHitEffectMod, "DirectEnemyHitEffect", mantaDashThroughWall.m_directEnemyHitEffect);
			AddToken(tokens, m_aoeDamageMod, "AoeDamage", string.Empty, mantaDashThroughWall.m_aoeDamage);
			AddToken_EffectMod(tokens, m_aoeEnemyHitEffectMod, "AoeEnemyHitEffect", mantaDashThroughWall.m_aoeEnemyHitEffect);
			AddToken(tokens, m_aoeThroughWallsDamageMod, "AoeThroughWallsDamage", string.Empty, mantaDashThroughWall.m_aoeThroughWallsDamage);
			AddToken_EffectMod(tokens, m_aoeThroughWallsEffectMod, "AoeThroughWallsEffect", mantaDashThroughWall.m_aoeThroughWallsEffect);
			AddToken_EffectMod(tokens, m_additionalDirtyFightingExplosionEffect, "ExtraDirtyFightingExplosionEffect");
			m_cooldownReductionsWhenNoHits.AddTooltipTokens(tokens, "CooldownReductionOnMiss");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		MantaDashThroughWall mantaDashThroughWall = GetTargetAbilityOnAbilityData(abilityData) as MantaDashThroughWall;
		// rogues
		// MantaDashThroughWall mantaDashThroughWall = targetAbility as MantaDashThroughWall;
		
		bool isValid = mantaDashThroughWall != null;
		string desc = string.Empty;
		desc += PropDesc(m_aoeConeWidthMod, "[AoeConeWidth]", isValid, isValid ? mantaDashThroughWall.m_aoeConeWidth : 0f);
		desc += PropDesc(m_aoeConeLengthMod, "[AoeConeLength]", isValid, isValid ? mantaDashThroughWall.m_aoeConeLength : 0f);
		desc += PropDesc(m_aoeThroughWallConeWidthMod, "[AoeThroughWallConeWidth]", isValid, isValid ? mantaDashThroughWall.m_aoeThroughWallConeWidth : 0f);
		desc += PropDesc(m_aoeThroughWallConeLengthMod, "[AoeThroughWallConeLength]", isValid, isValid ? mantaDashThroughWall.m_aoeThroughWallConeLength : 0f);
		desc += PropDesc(m_widthMod, "[Width]", isValid, isValid ? mantaDashThroughWall.m_width : 0f);
		desc += PropDesc(m_maxRangeMod, "[MaxRange]", isValid, isValid ? mantaDashThroughWall.m_maxRange : 0f);
		desc += PropDesc(m_maxWidthOfWallMod, "[MaxWidthOfWall]", isValid, isValid ? mantaDashThroughWall.m_maxWidthOfWall : 0f);
		desc += PropDesc(m_extraTotalDistanceIfThroughWallsMod, "[ExtraTotalDistanceIfThroughWalls]", isValid, isValid ? mantaDashThroughWall.m_extraTotalDistanceIfThroughWalls : 0f);
		desc += PropDesc(m_clampConeToWallMod, "[ClampConeToWall]", isValid, isValid && mantaDashThroughWall.m_clampConeToWall);
		desc += PropDesc(m_aoeWithMissMod, "[AoeWithMiss]", isValid, isValid && mantaDashThroughWall.m_aoeWithMiss);
		desc += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", isValid, isValid ? mantaDashThroughWall.m_coneBackwardOffset : 0f);
		desc += PropDesc(m_directHitDamageMod, "[DirectHitDamage]", isValid, isValid ? mantaDashThroughWall.m_directHitDamage : 0);
		desc += PropDesc(m_directEnemyHitEffectMod, "[DirectEnemyHitEffect]", isValid, isValid ? mantaDashThroughWall.m_directEnemyHitEffect : null);
		desc += PropDesc(m_directHitIgnoreCoverMod, "[DirectHitIgnoreCover]", isValid, isValid && mantaDashThroughWall.m_directHitIgnoreCover);
		desc += PropDesc(m_aoeDamageMod, "[AoeDamage]", isValid, isValid ? mantaDashThroughWall.m_aoeDamage : 0);
		desc += PropDesc(m_aoeEnemyHitEffectMod, "[AoeEnemyHitEffect]", isValid, isValid ? mantaDashThroughWall.m_aoeEnemyHitEffect : null);
		desc += PropDesc(m_aoeThroughWallsDamageMod, "[AoeThroughWallsDamage]", isValid, isValid ? mantaDashThroughWall.m_aoeThroughWallsDamage : 0);
		desc += PropDesc(m_aoeThroughWallsEffectMod, "[AoeThroughWallsEffect]", isValid, isValid ? mantaDashThroughWall.m_aoeThroughWallsEffect : null);
		desc += PropDesc(m_additionalDirtyFightingExplosionEffect, "[ExtraDirtyFightingExplosionEffect]", isValid);
		if (m_cooldownReductionsWhenNoHits.HasCooldownReduction())
		{
			desc += m_cooldownReductionsWhenNoHits.GetDescription(abilityData);
		}
		return desc;
	}
}
