using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NekoFanOfDiscs : AbilityMod
{
	[Separator("Targeting", true)]
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

	[Separator("Hit On Throw", true)]
	public AbilityModPropertyInt m_directDamageMod;

	public AbilityModPropertyInt m_directSubsequentHitDamageMod;

	public AbilityModPropertyEffectInfo m_directEnemyHitEffectMod;

	[Separator("Return Trip", true)]
	public AbilityModPropertyInt m_returnTripDamageMod;

	public AbilityModPropertyInt m_returnTripSubsequentHitDamageMod;

	public AbilityModPropertyBool m_returnTripIgnoreCoverMod;

	public AbilityModPropertyInt m_returnTripEnergyOnCasterPerDiscMissMod;

	[Separator("Effect on Self for misses", true)]
	public AbilityModPropertyEffectInfo m_effectOnSelfIfMissOnCastMod;

	public AbilityModPropertyEffectInfo m_effectOnSelfIfMissOnDiscReturnMod;

	[Separator("Zero Energy cost after N consecutive use", true)]
	public AbilityModPropertyInt m_zeroEnergyRequiredTurnsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NekoFanOfDiscs);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NekoFanOfDiscs nekoFanOfDiscs = targetAbility as NekoFanOfDiscs;
		if (!(nekoFanOfDiscs != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_numDiscsMod, "NumDiscs", string.Empty, nekoFanOfDiscs.m_numDiscs);
			AbilityMod.AddToken(tokens, m_minAngleForLaserFanMod, "MinAngleForLaserFan", string.Empty, nekoFanOfDiscs.m_minAngleForLaserFan);
			AbilityMod.AddToken(tokens, m_totalAngleForLaserFanMod, "TotalAngleForLaserFan", string.Empty, nekoFanOfDiscs.m_totalAngleForLaserFan);
			AbilityMod.AddToken(tokens, m_angleInterpMinDistMod, "AngleInterpMinDist", string.Empty, nekoFanOfDiscs.m_angleInterpMinDist);
			AbilityMod.AddToken(tokens, m_angleInterpMaxDistMod, "AngleInterpMaxDist", string.Empty, nekoFanOfDiscs.m_angleInterpMaxDist);
			AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, nekoFanOfDiscs.m_laserRange);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, nekoFanOfDiscs.m_laserWidth);
			AbilityMod.AddToken(tokens, m_aoeRadiusAtEndMod, "AoeRadiusAtEnd", string.Empty, nekoFanOfDiscs.m_aoeRadiusAtEnd);
			AbilityMod.AddToken(tokens, m_maxTargetsPerLaserMod, "MaxTargetsPerLaser", string.Empty, nekoFanOfDiscs.m_maxTargetsPerLaser);
			AbilityMod.AddToken(tokens, m_interpStepInSquaresMod, "InterpStepInSquares", string.Empty, nekoFanOfDiscs.m_interpStepInSquares);
			AbilityMod.AddToken(tokens, m_discReturnEndRadiusMod, "DiscReturnEndRadius", string.Empty, nekoFanOfDiscs.m_discReturnEndRadius);
			AbilityMod.AddToken(tokens, m_directDamageMod, "DirectDamage", string.Empty, nekoFanOfDiscs.m_directDamage);
			AbilityMod.AddToken(tokens, m_directSubsequentHitDamageMod, "DirectSubsequentHitDamage", string.Empty, nekoFanOfDiscs.m_directSubsequentHitDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_directEnemyHitEffectMod, "DirectEnemyHitEffect", nekoFanOfDiscs.m_directEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_returnTripDamageMod, "ReturnTripDamage", string.Empty, nekoFanOfDiscs.m_returnTripDamage);
			AbilityMod.AddToken(tokens, m_returnTripSubsequentHitDamageMod, "ReturnTripSubsequentHitDamage", string.Empty, nekoFanOfDiscs.m_returnTripSubsequentHitDamage);
			AbilityMod.AddToken(tokens, m_returnTripEnergyOnCasterPerDiscMissMod, "ReturnTripEnergyOnCasterPerDiscMiss", string.Empty, nekoFanOfDiscs.m_returnTripEnergyOnCasterPerDiscMiss);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnSelfIfMissOnCastMod, "EffectOnSelfIfMissOnCast", nekoFanOfDiscs.m_effectOnSelfIfMissOnCast);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnSelfIfMissOnDiscReturnMod, "EffectOnSelfIfMissOnDiscReturn", nekoFanOfDiscs.m_effectOnSelfIfMissOnDiscReturn);
			AbilityMod.AddToken(tokens, m_zeroEnergyRequiredTurnsMod, "ZeroEnergyRequiredTurns", string.Empty, nekoFanOfDiscs.m_zeroEnergyRequiredTurns);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoFanOfDiscs nekoFanOfDiscs = GetTargetAbilityOnAbilityData(abilityData) as NekoFanOfDiscs;
		bool flag = nekoFanOfDiscs != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt numDiscsMod = m_numDiscsMod;
		int baseVal;
		if (flag)
		{
			baseVal = nekoFanOfDiscs.m_numDiscs;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(numDiscsMod, "[NumDiscs]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat minAngleForLaserFanMod = m_minAngleForLaserFanMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = nekoFanOfDiscs.m_minAngleForLaserFan;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(minAngleForLaserFanMod, "[MinAngleForLaserFan]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat totalAngleForLaserFanMod = m_totalAngleForLaserFanMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = nekoFanOfDiscs.m_totalAngleForLaserFan;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(totalAngleForLaserFanMod, "[TotalAngleForLaserFan]", flag, baseVal3);
		empty += PropDesc(m_angleInterpMinDistMod, "[AngleInterpMinDist]", flag, (!flag) ? 0f : nekoFanOfDiscs.m_angleInterpMinDist);
		string str4 = empty;
		AbilityModPropertyFloat angleInterpMaxDistMod = m_angleInterpMaxDistMod;
		float baseVal4;
		if (flag)
		{
			baseVal4 = nekoFanOfDiscs.m_angleInterpMaxDist;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(angleInterpMaxDistMod, "[AngleInterpMaxDist]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyFloat laserRangeMod = m_laserRangeMod;
		float baseVal5;
		if (flag)
		{
			baseVal5 = nekoFanOfDiscs.m_laserRange;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + PropDesc(laserRangeMod, "[LaserRange]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
		float baseVal6;
		if (flag)
		{
			baseVal6 = nekoFanOfDiscs.m_laserWidth;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + PropDesc(laserWidthMod, "[LaserWidth]", flag, baseVal6);
		empty += PropDesc(m_aoeRadiusAtEndMod, "[AoeRadiusAtEnd]", flag, (!flag) ? 0f : nekoFanOfDiscs.m_aoeRadiusAtEnd);
		string str7 = empty;
		AbilityModPropertyInt maxTargetsPerLaserMod = m_maxTargetsPerLaserMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = nekoFanOfDiscs.m_maxTargetsPerLaser;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(maxTargetsPerLaserMod, "[MaxTargetsPerLaser]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyFloat interpStepInSquaresMod = m_interpStepInSquaresMod;
		float baseVal8;
		if (flag)
		{
			baseVal8 = nekoFanOfDiscs.m_interpStepInSquares;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str8 + PropDesc(interpStepInSquaresMod, "[InterpStepInSquares]", flag, baseVal8);
		empty += PropDesc(m_discReturnEndRadiusMod, "[DiscReturnEndRadius]", flag, (!flag) ? 0f : nekoFanOfDiscs.m_discReturnEndRadius);
		string str9 = empty;
		AbilityModPropertyInt directDamageMod = m_directDamageMod;
		int baseVal9;
		if (flag)
		{
			baseVal9 = nekoFanOfDiscs.m_directDamage;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(directDamageMod, "[DirectDamage]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyInt directSubsequentHitDamageMod = m_directSubsequentHitDamageMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = nekoFanOfDiscs.m_directSubsequentHitDamage;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(directSubsequentHitDamageMod, "[DirectSubsequentHitDamage]", flag, baseVal10);
		string str11 = empty;
		AbilityModPropertyEffectInfo directEnemyHitEffectMod = m_directEnemyHitEffectMod;
		object baseVal11;
		if (flag)
		{
			baseVal11 = nekoFanOfDiscs.m_directEnemyHitEffect;
		}
		else
		{
			baseVal11 = null;
		}
		empty = str11 + PropDesc(directEnemyHitEffectMod, "[DirectEnemyHitEffect]", flag, (StandardEffectInfo)baseVal11);
		string str12 = empty;
		AbilityModPropertyInt returnTripDamageMod = m_returnTripDamageMod;
		int baseVal12;
		if (flag)
		{
			baseVal12 = nekoFanOfDiscs.m_returnTripDamage;
		}
		else
		{
			baseVal12 = 0;
		}
		empty = str12 + PropDesc(returnTripDamageMod, "[ReturnTripDamage]", flag, baseVal12);
		string str13 = empty;
		AbilityModPropertyInt returnTripSubsequentHitDamageMod = m_returnTripSubsequentHitDamageMod;
		int baseVal13;
		if (flag)
		{
			baseVal13 = nekoFanOfDiscs.m_returnTripSubsequentHitDamage;
		}
		else
		{
			baseVal13 = 0;
		}
		empty = str13 + PropDesc(returnTripSubsequentHitDamageMod, "[ReturnTripSubsequentHitDamage]", flag, baseVal13);
		string str14 = empty;
		AbilityModPropertyBool returnTripIgnoreCoverMod = m_returnTripIgnoreCoverMod;
		int baseVal14;
		if (flag)
		{
			baseVal14 = (nekoFanOfDiscs.m_returnTripIgnoreCover ? 1 : 0);
		}
		else
		{
			baseVal14 = 0;
		}
		empty = str14 + PropDesc(returnTripIgnoreCoverMod, "[ReturnTripIgnoreCover]", flag, (byte)baseVal14 != 0);
		string str15 = empty;
		AbilityModPropertyInt returnTripEnergyOnCasterPerDiscMissMod = m_returnTripEnergyOnCasterPerDiscMissMod;
		int baseVal15;
		if (flag)
		{
			baseVal15 = nekoFanOfDiscs.m_returnTripEnergyOnCasterPerDiscMiss;
		}
		else
		{
			baseVal15 = 0;
		}
		empty = str15 + PropDesc(returnTripEnergyOnCasterPerDiscMissMod, "[EnergyOnCasterPerDiscMiss]", flag, baseVal15);
		string str16 = empty;
		AbilityModPropertyEffectInfo effectOnSelfIfMissOnCastMod = m_effectOnSelfIfMissOnCastMod;
		object baseVal16;
		if (flag)
		{
			baseVal16 = nekoFanOfDiscs.m_effectOnSelfIfMissOnCast;
		}
		else
		{
			baseVal16 = null;
		}
		empty = str16 + PropDesc(effectOnSelfIfMissOnCastMod, "[EffectOnSelfIfMissOnCast]", flag, (StandardEffectInfo)baseVal16);
		string str17 = empty;
		AbilityModPropertyEffectInfo effectOnSelfIfMissOnDiscReturnMod = m_effectOnSelfIfMissOnDiscReturnMod;
		object baseVal17;
		if (flag)
		{
			baseVal17 = nekoFanOfDiscs.m_effectOnSelfIfMissOnDiscReturn;
		}
		else
		{
			baseVal17 = null;
		}
		empty = str17 + PropDesc(effectOnSelfIfMissOnDiscReturnMod, "[EffectOnSelfIfMissOnDiscReturn]", flag, (StandardEffectInfo)baseVal17);
		string str18 = empty;
		AbilityModPropertyInt zeroEnergyRequiredTurnsMod = m_zeroEnergyRequiredTurnsMod;
		int baseVal18;
		if (flag)
		{
			baseVal18 = nekoFanOfDiscs.m_zeroEnergyRequiredTurns;
		}
		else
		{
			baseVal18 = 0;
		}
		return str18 + PropDesc(zeroEnergyRequiredTurnsMod, "[ZeroEnergyRequiredTurns]", flag, baseVal18);
	}
}
