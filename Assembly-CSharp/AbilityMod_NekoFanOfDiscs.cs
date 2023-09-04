using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NekoFanOfDiscs : AbilityMod
{
	[Separator("Targeting")]
	public AbilityModPropertyInt m_numDiscsMod;
	public AbilityModPropertyFloat m_minAngleForLaserFanMod;
	public AbilityModPropertyFloat m_totalAngleForLaserFanMod;
	public AbilityModPropertyFloat m_angleInterpMinDistMod;
	public AbilityModPropertyFloat m_angleInterpMaxDistMod;
	[Space(10f)]
	public AbilityModPropertyFloat m_laserRangeMod;
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_aoeRadiusAtEndMod;
	public AbilityModPropertyInt m_maxTargetsPerLaserMod;
	public AbilityModPropertyFloat m_interpStepInSquaresMod;
	[Header("-- Disc return end radius")]
	public AbilityModPropertyFloat m_discReturnEndRadiusMod;
	[Separator("Hit On Throw")]
	public AbilityModPropertyInt m_directDamageMod;
	public AbilityModPropertyInt m_directSubsequentHitDamageMod;
	public AbilityModPropertyEffectInfo m_directEnemyHitEffectMod;
	[Separator("Return Trip")]
	public AbilityModPropertyInt m_returnTripDamageMod;
	public AbilityModPropertyInt m_returnTripSubsequentHitDamageMod;
	public AbilityModPropertyBool m_returnTripIgnoreCoverMod;
	public AbilityModPropertyInt m_returnTripEnergyOnCasterPerDiscMissMod;
	[Separator("Effect on Self for misses")]
	public AbilityModPropertyEffectInfo m_effectOnSelfIfMissOnCastMod;
	public AbilityModPropertyEffectInfo m_effectOnSelfIfMissOnDiscReturnMod;
	[Separator("Zero Energy cost after N consecutive use")]
	public AbilityModPropertyInt m_zeroEnergyRequiredTurnsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NekoFanOfDiscs);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NekoFanOfDiscs nekoFanOfDiscs = targetAbility as NekoFanOfDiscs;
		if (nekoFanOfDiscs != null)
		{
			AddToken(tokens, m_numDiscsMod, "NumDiscs", string.Empty, nekoFanOfDiscs.m_numDiscs);
			AddToken(tokens, m_minAngleForLaserFanMod, "MinAngleForLaserFan", string.Empty, nekoFanOfDiscs.m_minAngleForLaserFan);
			AddToken(tokens, m_totalAngleForLaserFanMod, "TotalAngleForLaserFan", string.Empty, nekoFanOfDiscs.m_totalAngleForLaserFan);
			AddToken(tokens, m_angleInterpMinDistMod, "AngleInterpMinDist", string.Empty, nekoFanOfDiscs.m_angleInterpMinDist);
			AddToken(tokens, m_angleInterpMaxDistMod, "AngleInterpMaxDist", string.Empty, nekoFanOfDiscs.m_angleInterpMaxDist);
			AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, nekoFanOfDiscs.m_laserRange);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, nekoFanOfDiscs.m_laserWidth);
			AddToken(tokens, m_aoeRadiusAtEndMod, "AoeRadiusAtEnd", string.Empty, nekoFanOfDiscs.m_aoeRadiusAtEnd);
			AddToken(tokens, m_maxTargetsPerLaserMod, "MaxTargetsPerLaser", string.Empty, nekoFanOfDiscs.m_maxTargetsPerLaser);
			AddToken(tokens, m_interpStepInSquaresMod, "InterpStepInSquares", string.Empty, nekoFanOfDiscs.m_interpStepInSquares);
			AddToken(tokens, m_discReturnEndRadiusMod, "DiscReturnEndRadius", string.Empty, nekoFanOfDiscs.m_discReturnEndRadius);
			AddToken(tokens, m_directDamageMod, "DirectDamage", string.Empty, nekoFanOfDiscs.m_directDamage);
			AddToken(tokens, m_directSubsequentHitDamageMod, "DirectSubsequentHitDamage", string.Empty, nekoFanOfDiscs.m_directSubsequentHitDamage);
			AddToken_EffectMod(tokens, m_directEnemyHitEffectMod, "DirectEnemyHitEffect", nekoFanOfDiscs.m_directEnemyHitEffect);
			AddToken(tokens, m_returnTripDamageMod, "ReturnTripDamage", string.Empty, nekoFanOfDiscs.m_returnTripDamage);
			AddToken(tokens, m_returnTripSubsequentHitDamageMod, "ReturnTripSubsequentHitDamage", string.Empty, nekoFanOfDiscs.m_returnTripSubsequentHitDamage);
			AddToken(tokens, m_returnTripEnergyOnCasterPerDiscMissMod, "ReturnTripEnergyOnCasterPerDiscMiss", string.Empty, nekoFanOfDiscs.m_returnTripEnergyOnCasterPerDiscMiss);
			AddToken_EffectMod(tokens, m_effectOnSelfIfMissOnCastMod, "EffectOnSelfIfMissOnCast", nekoFanOfDiscs.m_effectOnSelfIfMissOnCast);
			AddToken_EffectMod(tokens, m_effectOnSelfIfMissOnDiscReturnMod, "EffectOnSelfIfMissOnDiscReturn", nekoFanOfDiscs.m_effectOnSelfIfMissOnDiscReturn);
			AddToken(tokens, m_zeroEnergyRequiredTurnsMod, "ZeroEnergyRequiredTurns", string.Empty, nekoFanOfDiscs.m_zeroEnergyRequiredTurns);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoFanOfDiscs nekoFanOfDiscs = GetTargetAbilityOnAbilityData(abilityData) as NekoFanOfDiscs;
		bool isValid = nekoFanOfDiscs != null;
		string desc = string.Empty;
		desc += PropDesc(m_numDiscsMod, "[NumDiscs]", isValid, isValid ? nekoFanOfDiscs.m_numDiscs : 0);
		desc += PropDesc(m_minAngleForLaserFanMod, "[MinAngleForLaserFan]", isValid, isValid ? nekoFanOfDiscs.m_minAngleForLaserFan : 0f);
		desc += PropDesc(m_totalAngleForLaserFanMod, "[TotalAngleForLaserFan]", isValid, isValid ? nekoFanOfDiscs.m_totalAngleForLaserFan : 0f);
		desc += PropDesc(m_angleInterpMinDistMod, "[AngleInterpMinDist]", isValid, isValid ? nekoFanOfDiscs.m_angleInterpMinDist : 0f);
		desc += PropDesc(m_angleInterpMaxDistMod, "[AngleInterpMaxDist]", isValid, isValid ? nekoFanOfDiscs.m_angleInterpMaxDist : 0f);
		desc += PropDesc(m_laserRangeMod, "[LaserRange]", isValid, isValid ? nekoFanOfDiscs.m_laserRange : 0f);
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isValid, isValid ? nekoFanOfDiscs.m_laserWidth : 0f);
		desc += PropDesc(m_aoeRadiusAtEndMod, "[AoeRadiusAtEnd]", isValid, isValid ? nekoFanOfDiscs.m_aoeRadiusAtEnd : 0f);
		desc += PropDesc(m_maxTargetsPerLaserMod, "[MaxTargetsPerLaser]", isValid, isValid ? nekoFanOfDiscs.m_maxTargetsPerLaser : 0);
		desc += PropDesc(m_interpStepInSquaresMod, "[InterpStepInSquares]", isValid, isValid ? nekoFanOfDiscs.m_interpStepInSquares : 0f);
		desc += PropDesc(m_discReturnEndRadiusMod, "[DiscReturnEndRadius]", isValid, isValid ? nekoFanOfDiscs.m_discReturnEndRadius : 0f);
		desc += PropDesc(m_directDamageMod, "[DirectDamage]", isValid, isValid ? nekoFanOfDiscs.m_directDamage : 0);
		desc += PropDesc(m_directSubsequentHitDamageMod, "[DirectSubsequentHitDamage]", isValid, isValid ? nekoFanOfDiscs.m_directSubsequentHitDamage : 0);
		desc += PropDesc(m_directEnemyHitEffectMod, "[DirectEnemyHitEffect]", isValid, isValid ? nekoFanOfDiscs.m_directEnemyHitEffect : null);
		desc += PropDesc(m_returnTripDamageMod, "[ReturnTripDamage]", isValid, isValid ? nekoFanOfDiscs.m_returnTripDamage : 0);
		desc += PropDesc(m_returnTripSubsequentHitDamageMod, "[ReturnTripSubsequentHitDamage]", isValid, isValid ? nekoFanOfDiscs.m_returnTripSubsequentHitDamage : 0);
		desc += PropDesc(m_returnTripIgnoreCoverMod, "[ReturnTripIgnoreCover]", isValid, isValid && nekoFanOfDiscs.m_returnTripIgnoreCover);
		desc += PropDesc(m_returnTripEnergyOnCasterPerDiscMissMod, "[EnergyOnCasterPerDiscMiss]", isValid, isValid ? nekoFanOfDiscs.m_returnTripEnergyOnCasterPerDiscMiss : 0);
		desc += PropDesc(m_effectOnSelfIfMissOnCastMod, "[EffectOnSelfIfMissOnCast]", isValid, isValid ? nekoFanOfDiscs.m_effectOnSelfIfMissOnCast : null);
		desc += PropDesc(m_effectOnSelfIfMissOnDiscReturnMod, "[EffectOnSelfIfMissOnDiscReturn]", isValid, isValid ? nekoFanOfDiscs.m_effectOnSelfIfMissOnDiscReturn : null);
		return desc + PropDesc(m_zeroEnergyRequiredTurnsMod, "[ZeroEnergyRequiredTurns]", isValid, isValid ? nekoFanOfDiscs.m_zeroEnergyRequiredTurns : 0);
	}
}
