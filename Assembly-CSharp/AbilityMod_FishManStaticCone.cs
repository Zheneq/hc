using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_FishManStaticCone : AbilityMod
{
	[Header("-- Cone Data")]
	public AbilityModPropertyFloat m_coneWidthAngleMod;
	public AbilityModPropertyFloat m_coneWidthAngleMinMod;
	public AbilityModPropertyFloat m_coneLengthMod;
	public AbilityModPropertyFloat m_coneBackwardOffsetMod;
	public AbilityModPropertyBool m_penetrateLineOfSightMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	[Header("-- On Hit Target")]
	public AbilityModPropertyInt m_damageToEnemiesMod;
	public AbilityModPropertyInt m_damageToEnemiesMaxMod;
	public AbilityModPropertyEffectInfo m_effectToEnemiesMod;
	[Space(10f)]
	public AbilityModPropertyInt m_healingToAlliesMod;
	public AbilityModPropertyInt m_healingToAlliesMaxMod;
	public AbilityModPropertyEffectInfo m_effectToAlliesMod;
	public AbilityModPropertyInt m_extraAllyHealForSingleHitMod;
	public AbilityModPropertyEffectInfo m_extraEffectOnClosestAllyMod;
	[Header("-- Self-Healing")]
	public AbilityModPropertyInt m_healToCasterOnCastMod;
	public AbilityModPropertyInt m_healToCasterPerEnemyHitMod;
	public AbilityModPropertyInt m_healToCasterPerAllyHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FishManStaticCone);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FishManStaticCone fishManStaticCone = targetAbility as FishManStaticCone;
		if (fishManStaticCone != null)
		{
			AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, fishManStaticCone.m_coneWidthAngle);
			AddToken(tokens, m_coneWidthAngleMod, "ConeWidthMinAngle", string.Empty, fishManStaticCone.m_coneWidthAngleMin);
			AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, fishManStaticCone.m_coneLength);
			AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, fishManStaticCone.m_coneBackwardOffset);
			AddToken(tokens, m_damageToEnemiesMod, "DamageToEnemies", string.Empty, fishManStaticCone.m_damageToEnemies);
			AddToken(tokens, m_damageToEnemiesMaxMod, "DamageToEnemiesMax", string.Empty, fishManStaticCone.m_damageToEnemiesMax);
			AddToken_EffectMod(tokens, m_effectToEnemiesMod, "EffectToEnemies", fishManStaticCone.m_effectToEnemies);
			AddToken(tokens, m_healingToAlliesMod, "HealingToAllies", string.Empty, fishManStaticCone.m_healingToAllies);
			AddToken(tokens, m_healingToAlliesMaxMod, "HealingToAlliesMax", string.Empty, fishManStaticCone.m_healingToAlliesMax);
			AddToken_EffectMod(tokens, m_effectToAlliesMod, "EffectToAllies", fishManStaticCone.m_effectToAllies);
			AddToken(tokens, m_extraAllyHealForSingleHitMod, "ExtraAllyHealForSingleHit", string.Empty, fishManStaticCone.m_extraAllyHealForSingleHit);
			AddToken_EffectMod(tokens, m_extraEffectOnClosestAllyMod, "ExtraEffectOnClosestAlly", fishManStaticCone.m_extraEffectOnClosestAlly);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, fishManStaticCone.m_maxTargets);
			AddToken(tokens, m_healToCasterOnCastMod, "HealToCasterOnCast", string.Empty, fishManStaticCone.m_healToCasterOnCast);
			AddToken(tokens, m_healToCasterPerEnemyHitMod, "HealToCasterPerEnemyHit", string.Empty, fishManStaticCone.m_healToCasterPerEnemyHit);
			AddToken(tokens, m_healToCasterPerAllyHitMod, "HealToCasterPerAllyHit", string.Empty, fishManStaticCone.m_healToCasterPerAllyHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManStaticCone fishManStaticCone = GetTargetAbilityOnAbilityData(abilityData) as FishManStaticCone;
		bool isValid = fishManStaticCone != null;
		string desc = string.Empty;
		desc += PropDesc(m_coneWidthAngleMod, "[ConeWidthAngle]", isValid, isValid ? fishManStaticCone.m_coneWidthAngle : 0f);
		desc += PropDesc(m_coneWidthAngleMinMod, "[ConeWidthMinAngle]", isValid, isValid ? fishManStaticCone.m_coneWidthAngleMin : 0f);
		desc += PropDesc(m_coneLengthMod, "[ConeLength]", isValid, isValid ? fishManStaticCone.m_coneLength : 0f);
		desc += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", isValid, isValid ? fishManStaticCone.m_coneBackwardOffset : 0f);
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isValid, isValid && fishManStaticCone.m_penetrateLineOfSight);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? fishManStaticCone.m_maxTargets : 0);
		desc += PropDesc(m_damageToEnemiesMod, "[DamageToEnemies]", isValid, isValid ? fishManStaticCone.m_damageToEnemies : 0);
		desc += PropDesc(m_damageToEnemiesMaxMod, "[DamageToEnemiesMax]", isValid, isValid ? fishManStaticCone.m_damageToEnemiesMax : 0);
		desc += PropDesc(m_effectToEnemiesMod, "[EffectToEnemies]", isValid, isValid ? fishManStaticCone.m_effectToEnemies : null);
		desc += PropDesc(m_healingToAlliesMod, "[HealingToAllies]", isValid, isValid ? fishManStaticCone.m_healingToAllies : 0);
		desc += PropDesc(m_healingToAlliesMaxMod, "[HealingToAlliesMax]", isValid, isValid ? fishManStaticCone.m_healingToAlliesMax : 0);
		desc += PropDesc(m_effectToAlliesMod, "[EffectToAllies]", isValid, isValid ? fishManStaticCone.m_effectToAllies : null);
		desc += PropDesc(m_extraAllyHealForSingleHitMod, "[ExtraAllyHealForSingleHit]", isValid, isValid ? fishManStaticCone.m_extraAllyHealForSingleHit : 0);
		desc += PropDesc(m_extraEffectOnClosestAllyMod, "[ExtraEffectOnClosestAlly]", isValid, isValid ? fishManStaticCone.m_extraEffectOnClosestAlly : null);
		desc += PropDesc(m_healToCasterOnCastMod, "[HealToCasterOnCast]", isValid, isValid ? fishManStaticCone.m_healToCasterOnCast : 0);
		desc += PropDesc(m_healToCasterPerEnemyHitMod, "[HealToCasterPerEnemyHit]", isValid, isValid ? fishManStaticCone.m_healToCasterPerEnemyHit : 0);
		return desc + PropDesc(m_healToCasterPerAllyHitMod, "[HealToCasterPerAllyHit]", isValid, isValid ? fishManStaticCone.m_healToCasterPerAllyHit : 0);
	}
}
