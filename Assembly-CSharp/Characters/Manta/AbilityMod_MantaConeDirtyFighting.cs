// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_MantaConeDirtyFighting : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_coneRangeMod;
	public AbilityModPropertyFloat m_coneWidthMod;
	public AbilityModPropertyBool m_penetrateLoSMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	public AbilityModPropertyFloat m_coneBackwardOffsetMod;
	[Header("-- Hit Damage/Effects")]
	public AbilityModPropertyInt m_onCastDamageAmountMod;
	public AbilityModPropertyEffectData m_dirtyFightingEffectDataMod;
	public AbilityModPropertyEffectInfo m_enemyHitEffectDataMod;
	public AbilityModPropertyEffectInfo m_effectOnTargetFromExplosionMod;
	public AbilityModPropertyEffectInfo m_effectOnTargetWhenExpiresWithoutExplosionMod;
	[Header("-- On Reaction Hit/Explosion Triggered")]
	public AbilityModPropertyInt m_effectExplosionDamageMod;
	public AbilityModPropertyBool m_explodeOnlyFromSelfDamageMod;
	public AbilityModPropertyInt m_techPointGainPerExplosionMod;
	public AbilityModPropertyInt m_healPerExplosionMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(MantaConeDirtyFighting);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MantaConeDirtyFighting mantaConeDirtyFighting = targetAbility as MantaConeDirtyFighting;
		if (mantaConeDirtyFighting != null)
		{
			AddToken(tokens, m_coneRangeMod, "ConeRange", string.Empty, mantaConeDirtyFighting.m_coneRange);
			AddToken(tokens, m_coneWidthMod, "ConeWidth", string.Empty, mantaConeDirtyFighting.m_coneWidth);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, mantaConeDirtyFighting.m_maxTargets);
			AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, mantaConeDirtyFighting.m_coneBackwardOffset);
			AddToken(tokens, m_onCastDamageAmountMod, "OnCastDamageAmount", string.Empty, mantaConeDirtyFighting.m_onCastDamageAmount);
			AddToken_EffectMod(tokens, m_dirtyFightingEffectDataMod, "DirtyFightingEffectData", mantaConeDirtyFighting.m_dirtyFightingEffectData);
			AddToken_EffectMod(tokens, m_enemyHitEffectDataMod, "EnemyHitEffectData", mantaConeDirtyFighting.m_enemyHitEffectData);
			AddToken_EffectMod(tokens, m_effectOnTargetFromExplosionMod, "EffectOnTargetFromExplosion", mantaConeDirtyFighting.m_effectOnTargetFromExplosion);
			AddToken_EffectMod(tokens, m_effectOnTargetWhenExpiresWithoutExplosionMod, "EffectOnTargetWhenExpires");
			AddToken(tokens, m_effectExplosionDamageMod, "EffectExplosionDamage", string.Empty, mantaConeDirtyFighting.m_effectExplosionDamage);
			AddToken(tokens, m_techPointGainPerExplosionMod, "TechPointGainPerExplosion", string.Empty, mantaConeDirtyFighting.m_techPointGainPerExplosion);
			AddToken(tokens, m_healPerExplosionMod, "HealAmountPerExplosion", string.Empty, mantaConeDirtyFighting.m_healAmountPerExplosion);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		MantaConeDirtyFighting mantaConeDirtyFighting = GetTargetAbilityOnAbilityData(abilityData) as MantaConeDirtyFighting;
		// rogues
		// MantaConeDirtyFighting mantaConeDirtyFighting = targetAbility as MantaConeDirtyFighting;
		
		bool isValid = mantaConeDirtyFighting != null;
		string desc = string.Empty;
		desc += PropDesc(m_coneRangeMod, "[ConeRange]", isValid, isValid ? mantaConeDirtyFighting.m_coneRange : 0f);
		desc += PropDesc(m_coneWidthMod, "[ConeWidth]", isValid, isValid ? mantaConeDirtyFighting.m_coneWidth : 0f);
		desc += PropDesc(m_penetrateLoSMod, "[PenetrateLoS]", isValid, isValid && mantaConeDirtyFighting.m_penetrateLoS);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? mantaConeDirtyFighting.m_maxTargets : 0);
		desc += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", isValid, isValid ? mantaConeDirtyFighting.m_coneBackwardOffset : 0f);
		desc += PropDesc(m_onCastDamageAmountMod, "[OnCastDamageAmount]", isValid, isValid ? mantaConeDirtyFighting.m_onCastDamageAmount : 0);
		desc += PropDesc(m_dirtyFightingEffectDataMod, "[DirtyFightingEffectData]", isValid, isValid ? mantaConeDirtyFighting.m_dirtyFightingEffectData : null);
		desc += PropDesc(m_enemyHitEffectDataMod, "[EnemyHitEffectData]", isValid, isValid ? mantaConeDirtyFighting.m_enemyHitEffectData : null);
		desc += PropDesc(m_effectOnTargetFromExplosionMod, "[EffectOnTargetFromExplosion]", isValid, isValid ? mantaConeDirtyFighting.m_effectOnTargetFromExplosion : null);
		desc += PropDesc(m_effectOnTargetWhenExpiresWithoutExplosionMod, "[EffectOnTargetWhenExpires]", isValid);
		desc += PropDesc(m_effectExplosionDamageMod, "[EffectExplosionDamage]", isValid, isValid ? mantaConeDirtyFighting.m_effectExplosionDamage : 0);
		desc += PropDesc(m_explodeOnlyFromSelfDamageMod, "[ExplodeOnlyFromSelfDamage]", isValid, isValid && mantaConeDirtyFighting.m_explodeOnlyFromSelfDamage);
		desc += PropDesc(m_techPointGainPerExplosionMod, "[TechPointGainPerExplosion]", isValid, isValid ? mantaConeDirtyFighting.m_techPointGainPerExplosion : 0);
		return desc + PropDesc(m_healPerExplosionMod, "[HealAmountPerExplosion]", isValid, isValid ? mantaConeDirtyFighting.m_healAmountPerExplosion : 0);
	}
}
