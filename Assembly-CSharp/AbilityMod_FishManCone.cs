using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_FishManCone : AbilityMod
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
	[Header("-- Ally Healing and Effect")]
	public AbilityModPropertyInt m_healingToAlliesMod;
	public AbilityModPropertyInt m_healingToAlliesMaxMod;
	public AbilityModPropertyEffectInfo m_effectToAlliesMod;
	[Header("-- Self-Healing")]
	public AbilityModPropertyInt m_healToCasterOnCastMod;
	public AbilityModPropertyInt m_healToCasterPerEnemyHitMod;
	public AbilityModPropertyInt m_healToCasterPerAllyHitMod;
	[Header("-- Bonus Healing on Heal Cone ability")]
	public AbilityModPropertyInt m_extraHealPerEnemyHitForNextHealConeMod;
	[Header("-- Extra Energy")]
	public AbilityModPropertyInt m_extraEnergyForSingleEnemyHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FishManCone);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FishManCone fishManCone = targetAbility as FishManCone;
		if (fishManCone != null)
		{
			AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, fishManCone.m_coneWidthAngle);
			AddToken(tokens, m_coneWidthAngleMinMod, "ConeWidthAngleMin", string.Empty, fishManCone.m_coneWidthAngleMin);
			AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, fishManCone.m_coneLength);
			AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, fishManCone.m_coneBackwardOffset);
			AddToken(tokens, m_damageToEnemiesMod, "DamageToEnemies", string.Empty, fishManCone.m_damageToEnemies);
			AddToken(tokens, m_damageToEnemiesMaxMod, "DamageToEnemiesMax", string.Empty, fishManCone.m_damageToEnemiesMax);
			AddToken_EffectMod(tokens, m_effectToEnemiesMod, "EffectToEnemies", fishManCone.m_effectToEnemies);
			AddToken(tokens, m_healingToAlliesMod, "HealingToAllies", string.Empty, fishManCone.m_healingToAllies);
			AddToken(tokens, m_healingToAlliesMaxMod, "HealingToAlliesMax", string.Empty, fishManCone.m_healingToAlliesMax);
			AddToken_EffectMod(tokens, m_effectToAlliesMod, "EffectToAllies", fishManCone.m_effectToAllies);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, fishManCone.m_maxTargets);
			AddToken(tokens, m_healToCasterOnCastMod, "HealToCasterOnCast", string.Empty, fishManCone.m_healToCasterOnCast);
			AddToken(tokens, m_healToCasterPerEnemyHitMod, "HealToCasterPerEnemyHit", string.Empty, fishManCone.m_healToCasterPerEnemyHit);
			AddToken(tokens, m_healToCasterPerAllyHitMod, "HealToCasterPerAllyHit", string.Empty, fishManCone.m_healToCasterPerAllyHit);
			AddToken(tokens, m_extraHealPerEnemyHitForNextHealConeMod, "ExtraHealPerEnemyHitForNextHealCone", string.Empty, fishManCone.m_extraHealPerEnemyHitForNextHealCone);
			AddToken(tokens, m_extraEnergyForSingleEnemyHitMod, "ExtraEnergyForSingleEnemyHit", string.Empty, fishManCone.m_extraEnergyForSingleEnemyHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManCone fishManCone = GetTargetAbilityOnAbilityData(abilityData) as FishManCone;
		bool isValid = fishManCone != null;
		string desc = string.Empty;
		desc += PropDesc(m_coneWidthAngleMod, "[ConeWidthAngle]", isValid, isValid ? fishManCone.m_coneWidthAngle : 0f);
		desc += PropDesc(m_coneWidthAngleMinMod, "[ConeWidthAngleMin]", isValid, isValid ? fishManCone.m_coneWidthAngleMin : 0f);
		desc += PropDesc(m_coneLengthMod, "[ConeLength]", isValid, isValid ? fishManCone.m_coneLength : 0f);
		desc += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", isValid, isValid ? fishManCone.m_coneBackwardOffset : 0f);
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isValid, isValid && fishManCone.m_penetrateLineOfSight);
		desc += PropDesc(m_damageToEnemiesMod, "[DamageToEnemies]", isValid, isValid ? fishManCone.m_damageToEnemies : 0);
		desc += PropDesc(m_damageToEnemiesMaxMod, "[DamageToEnemiesMax]", isValid, isValid ? fishManCone.m_damageToEnemiesMax : 0);
		desc += PropDesc(m_effectToEnemiesMod, "[EffectToEnemies]", isValid, isValid ? fishManCone.m_effectToEnemies : null);
		desc += PropDesc(m_healingToAlliesMod, "[HealingToAllies]", isValid, isValid ? fishManCone.m_healingToAllies : 0);
		desc += PropDesc(m_healingToAlliesMaxMod, "[HealingToAlliesMax]", isValid, isValid ? fishManCone.m_healingToAlliesMax : 0);
		desc += PropDesc(m_effectToAlliesMod, "[EffectToAllies]", isValid, isValid ? fishManCone.m_effectToAllies : null);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? fishManCone.m_maxTargets : 0);
		desc += PropDesc(m_healToCasterOnCastMod, "[HealToCasterOnCast]", isValid, isValid ? fishManCone.m_healToCasterOnCast : 0);
		desc += PropDesc(m_healToCasterPerEnemyHitMod, "[HealToCasterPerEnemyHit]", isValid, isValid ? fishManCone.m_healToCasterPerEnemyHit : 0);
		desc += PropDesc(m_healToCasterPerAllyHitMod, "[HealToCasterPerAllyHit]", isValid, isValid ? fishManCone.m_healToCasterPerAllyHit : 0);
		desc += PropDesc(m_extraHealPerEnemyHitForNextHealConeMod, "[ExtraHealPerEnemyHitForNextHealCone]", isValid, isValid ? fishManCone.m_extraHealPerEnemyHitForNextHealCone : 0);
		return desc + PropDesc(m_extraEnergyForSingleEnemyHitMod, "[ExtraEnergyForSingleEnemyHit]", isValid, isValid ? fishManCone.m_extraEnergyForSingleEnemyHit : 0);
	}
}
