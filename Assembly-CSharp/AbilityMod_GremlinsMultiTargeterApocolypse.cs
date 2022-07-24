// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_GremlinsMultiTargeterApocolypse : AbilityMod
{
	[Header("-- Damage Mods")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyInt m_subsequentDamageMod;
	[Header("-- Leave Landmine on Empty Squares?")]
	public AbilityModPropertyBool m_leaveLandmineOnEmptySquaresMod;
	[Header("-- Energy Gain per Miss (no enemy hit)--")]
	public AbilityModPropertyInt m_energyGainPerMissMod;
	[Header("-- Targeting Mods")]
	public AbilityModPropertyShape m_shapeMod;
	public AbilityModPropertyBool m_penetrateLosMod;
	public AbilityModPropertyFloat m_minDistanceBetweenBombsMod;
	public AbilityModPropertyFloat m_maxAngleWithFirstMod;
	[Space(10f)]
	[Header("-- Global Mine Data Mods")]
	public AbilityModPropertyInt m_mineDamageMod;
	public AbilityModPropertyInt m_mineDurationMod;
	public AbilityModPropertyEffectInfo m_effectOnEnemyOverride;
	public AbilityModPropertyInt m_energyOnMineExplosionMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(GremlinsMultiTargeterApocolypse);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		GremlinsMultiTargeterApocolypse gremlinsMultiTargeterApocolypse = targetAbility as GremlinsMultiTargeterApocolypse;
		if (gremlinsMultiTargeterApocolypse != null)
		{
			AddToken(tokens, m_energyGainPerMissMod, "EnergyGainPerMiss", string.Empty, gremlinsMultiTargeterApocolypse.m_energyGainPerMiss);
			AddToken(tokens, m_minDistanceBetweenBombsMod, "MinDistanceBetweenBombs", string.Empty, gremlinsMultiTargeterApocolypse.m_minDistanceBetweenBombs);
			AddToken(tokens, m_maxAngleWithFirstMod, "MaxAngleWithFirst", string.Empty, gremlinsMultiTargeterApocolypse.m_maxAngleWithFirst);
			AddToken(tokens, m_damageMod, "BombDamageAmount", string.Empty, gremlinsMultiTargeterApocolypse.m_bombDamageAmount);
			AddToken(tokens, m_subsequentDamageMod, "BombSubsequentDamageAmount", string.Empty, gremlinsMultiTargeterApocolypse.m_bombSubsequentDamageAmount);
			if (m_useTargetDataOverrides && m_targetDataOverrides != null)
			{
				AddToken_IntDiff(tokens, "NumBombs", string.Empty, m_targetDataOverrides.Length, true, gremlinsMultiTargeterApocolypse.m_targetData.Length);
				if (m_targetDataOverrides.Length > 0
				    && gremlinsMultiTargeterApocolypse.m_targetData.Length > 0)
				{
					AddToken_IntDiff(tokens, "TargeterRange_Diff", string.Empty, Mathf.RoundToInt(m_targetDataOverrides[0].m_range - gremlinsMultiTargeterApocolypse.m_targetData[0].m_range), false, 0);
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		GremlinsMultiTargeterApocolypse gremlinsMultiTargeterApocolypse = GetTargetAbilityOnAbilityData(abilityData) as GremlinsMultiTargeterApocolypse;
		// rogues
		//GremlinsMultiTargeterApocolypse gremlinsMultiTargeterApocolypse = targetAbility as GremlinsMultiTargeterApocolypse;
		
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent = gremlinsMultiTargeterApocolypse != null
			? gremlinsMultiTargeterApocolypse.GetComponent<GremlinsLandMineInfoComponent>()
			: null;
		bool isAbilityPresent = gremlinsLandMineInfoComponent != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Base Damage]", isAbilityPresent, isAbilityPresent ? gremlinsMultiTargeterApocolypse.m_bombDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_subsequentDamageMod, "[Subsequent Damage]", isAbilityPresent, isAbilityPresent ? gremlinsMultiTargeterApocolypse.m_bombSubsequentDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_leaveLandmineOnEmptySquaresMod, "[Leave Mine on Empty Squares?]", isAbilityPresent, isAbilityPresent && gremlinsMultiTargeterApocolypse.m_leaveLandmineOnEmptySquare);
		desc += PropDesc(m_energyGainPerMissMod, "[EnergyGainPerMiss]", isAbilityPresent, isAbilityPresent ? gremlinsMultiTargeterApocolypse.m_energyGainPerMiss : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_shapeMod, "[Bomb Shape]", isAbilityPresent, isAbilityPresent ? gremlinsMultiTargeterApocolypse.m_bombShape : AbilityAreaShape.SingleSquare);
		desc += AbilityModHelper.GetModPropertyDesc(m_penetrateLosMod, "[Penetrate Los?]", isAbilityPresent, isAbilityPresent && gremlinsMultiTargeterApocolypse.m_penetrateLos);
		desc += AbilityModHelper.GetModPropertyDesc(m_minDistanceBetweenBombsMod, "[Min Dist Between Bombs]", isAbilityPresent, isAbilityPresent ? gremlinsMultiTargeterApocolypse.m_minDistanceBetweenBombs : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_maxAngleWithFirstMod, "[Max Angle With First Segment]", isAbilityPresent, isAbilityPresent ? gremlinsMultiTargeterApocolypse.m_maxAngleWithFirst : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_mineDamageMod, "[Mine Damage]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_mineDurationMod, "[Mine Duration]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_mineDuration : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_effectOnEnemyOverride, "{ Effect on Enemy Hit Override }", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_enemyHitEffect : null);
		return desc + AbilityModHelper.GetModPropertyDesc(m_energyOnMineExplosionMod, "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]", isAbilityPresent, isAbilityPresent ? gremlinsLandMineInfoComponent.m_energyGainOnExplosion : 0);
	}
}
