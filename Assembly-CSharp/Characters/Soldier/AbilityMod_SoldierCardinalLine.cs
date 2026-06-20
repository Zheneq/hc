// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SoldierCardinalLine : AbilityMod
{
	[Header("-- Targeting (shape for position targeter, line width for strafe hit area --")]
	public AbilityModPropertyBool m_useBothCardinalDirMod;
	public AbilityModPropertyShape m_positionShapeMod;
	public AbilityModPropertyFloat m_lineWidthMod;
	public AbilityModPropertyBool m_penetrateLosMod;
	[Header("-- On Hit Stuff --")]
	public AbilityModPropertyInt m_damageAmountMod;
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;
	[Header("-- Extra Damage for near center")]
	public AbilityModPropertyFloat m_nearCenterDistThresholdMod;
	public AbilityModPropertyInt m_extraDamageForNearCenterTargetsMod;
	[Header("-- AoE around targets --")]
	public AbilityModPropertyShape m_aoeShapeMod;
	public AbilityModPropertyInt m_aoeDamageMod;
	[Header("-- Subsequent Turn Hits --")]
	public AbilityModPropertyInt m_numSubsequentTurnsMod;
	public AbilityModPropertyInt m_damageOnSubsequentTurnsMod;
	public AbilityModPropertyEffectInfo m_enemyEffectOnSubsequentTurnsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SoldierCardinalLine);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SoldierCardinalLine soldierCardinalLine = targetAbility as SoldierCardinalLine;
		if (soldierCardinalLine != null)
		{
			AddToken(tokens, m_lineWidthMod, "LineWidth", string.Empty, soldierCardinalLine.m_lineWidth);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, soldierCardinalLine.m_damageAmount);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", soldierCardinalLine.m_enemyHitEffect);
			AddToken(tokens, m_aoeDamageMod, "AoeDamage", string.Empty, soldierCardinalLine.m_aoeDamage);
			AddToken(tokens, m_nearCenterDistThresholdMod, "NearCenterDistThreshold", string.Empty, soldierCardinalLine.m_nearCenterDistThreshold);
			AddToken(tokens, m_extraDamageForNearCenterTargetsMod, "ExtraDamageForNearCenterTargets", string.Empty, soldierCardinalLine.m_extraDamageForNearCenterTargets);
			AddToken(tokens, m_numSubsequentTurnsMod, "NumSubsequentTurns", string.Empty, soldierCardinalLine.m_numSubsequentTurns);
			AddToken(tokens, m_damageOnSubsequentTurnsMod, "DamageOnSubsequentTurns", string.Empty, soldierCardinalLine.m_damageOnSubsequentTurns);
			AddToken_EffectMod(tokens, m_enemyEffectOnSubsequentTurnsMod, "EnemyEffectOnSubsequentTurns", soldierCardinalLine.m_enemyEffectOnSubsequentTurns);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		SoldierCardinalLine soldierCardinalLine = GetTargetAbilityOnAbilityData(abilityData) as SoldierCardinalLine;
		// rogues
		// SoldierCardinalLine soldierCardinalLine = targetAbility as SoldierCardinalLine;
		
		bool isValid = soldierCardinalLine != null;
		string desc = string.Empty;
		desc += PropDesc(m_useBothCardinalDirMod, "[UseBothCardinalDir]", isValid, isValid && soldierCardinalLine.m_useBothCardinalDir);
		desc += PropDesc(m_positionShapeMod, "[PositionShape]", isValid, isValid ? soldierCardinalLine.m_positionShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_lineWidthMod, "[LineWidth]", isValid, isValid ? soldierCardinalLine.m_lineWidth : 0f);
		desc += PropDesc(m_penetrateLosMod, "[PenetrateLos]", isValid, isValid && soldierCardinalLine.m_penetrateLos);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? soldierCardinalLine.m_damageAmount : 0);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isValid, isValid ? soldierCardinalLine.m_enemyHitEffect : null);
		desc += PropDesc(m_nearCenterDistThresholdMod, "[NearCenterDistThreshold]", isValid, isValid ? soldierCardinalLine.m_nearCenterDistThreshold : 0f);
		desc += PropDesc(m_extraDamageForNearCenterTargetsMod, "[ExtraDamageForNearCenterTargets]", isValid, isValid ? soldierCardinalLine.m_extraDamageForNearCenterTargets : 0);
		desc += PropDesc(m_aoeShapeMod, "[AoeShape]", isValid, isValid ? soldierCardinalLine.m_aoeShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_aoeDamageMod, "[AoeDamage]", isValid, isValid ? soldierCardinalLine.m_aoeDamage : 0);
		desc += PropDesc(m_numSubsequentTurnsMod, "[NumSubsequentTurns]", isValid, isValid ? soldierCardinalLine.m_numSubsequentTurns : 0);
		desc += PropDesc(m_damageOnSubsequentTurnsMod, "[DamageOnSubsequentTurns]", isValid, isValid ? soldierCardinalLine.m_damageOnSubsequentTurns : 0);
		return desc + PropDesc(m_enemyEffectOnSubsequentTurnsMod, "[EnemyEffectOnSubsequentTurns]", isValid, isValid ? soldierCardinalLine.m_enemyEffectOnSubsequentTurns : null);
	}
}
