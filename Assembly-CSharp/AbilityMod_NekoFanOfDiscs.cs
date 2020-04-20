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
		if (nekoFanOfDiscs != null)
		{
			AbilityMod.AddToken(tokens, this.m_numDiscsMod, "NumDiscs", string.Empty, nekoFanOfDiscs.m_numDiscs, true, false);
			AbilityMod.AddToken(tokens, this.m_minAngleForLaserFanMod, "MinAngleForLaserFan", string.Empty, nekoFanOfDiscs.m_minAngleForLaserFan, true, false, false);
			AbilityMod.AddToken(tokens, this.m_totalAngleForLaserFanMod, "TotalAngleForLaserFan", string.Empty, nekoFanOfDiscs.m_totalAngleForLaserFan, true, false, false);
			AbilityMod.AddToken(tokens, this.m_angleInterpMinDistMod, "AngleInterpMinDist", string.Empty, nekoFanOfDiscs.m_angleInterpMinDist, true, false, false);
			AbilityMod.AddToken(tokens, this.m_angleInterpMaxDistMod, "AngleInterpMaxDist", string.Empty, nekoFanOfDiscs.m_angleInterpMaxDist, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", string.Empty, nekoFanOfDiscs.m_laserRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, nekoFanOfDiscs.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_aoeRadiusAtEndMod, "AoeRadiusAtEnd", string.Empty, nekoFanOfDiscs.m_aoeRadiusAtEnd, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsPerLaserMod, "MaxTargetsPerLaser", string.Empty, nekoFanOfDiscs.m_maxTargetsPerLaser, true, false);
			AbilityMod.AddToken(tokens, this.m_interpStepInSquaresMod, "InterpStepInSquares", string.Empty, nekoFanOfDiscs.m_interpStepInSquares, true, false, false);
			AbilityMod.AddToken(tokens, this.m_discReturnEndRadiusMod, "DiscReturnEndRadius", string.Empty, nekoFanOfDiscs.m_discReturnEndRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_directDamageMod, "DirectDamage", string.Empty, nekoFanOfDiscs.m_directDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_directSubsequentHitDamageMod, "DirectSubsequentHitDamage", string.Empty, nekoFanOfDiscs.m_directSubsequentHitDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_directEnemyHitEffectMod, "DirectEnemyHitEffect", nekoFanOfDiscs.m_directEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_returnTripDamageMod, "ReturnTripDamage", string.Empty, nekoFanOfDiscs.m_returnTripDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_returnTripSubsequentHitDamageMod, "ReturnTripSubsequentHitDamage", string.Empty, nekoFanOfDiscs.m_returnTripSubsequentHitDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_returnTripEnergyOnCasterPerDiscMissMod, "ReturnTripEnergyOnCasterPerDiscMiss", string.Empty, nekoFanOfDiscs.m_returnTripEnergyOnCasterPerDiscMiss, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnSelfIfMissOnCastMod, "EffectOnSelfIfMissOnCast", nekoFanOfDiscs.m_effectOnSelfIfMissOnCast, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnSelfIfMissOnDiscReturnMod, "EffectOnSelfIfMissOnDiscReturn", nekoFanOfDiscs.m_effectOnSelfIfMissOnDiscReturn, true);
			AbilityMod.AddToken(tokens, this.m_zeroEnergyRequiredTurnsMod, "ZeroEnergyRequiredTurns", string.Empty, nekoFanOfDiscs.m_zeroEnergyRequiredTurns, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoFanOfDiscs nekoFanOfDiscs = base.GetTargetAbilityOnAbilityData(abilityData) as NekoFanOfDiscs;
		bool flag = nekoFanOfDiscs != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt numDiscsMod = this.m_numDiscsMod;
		string prefix = "[NumDiscs]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = nekoFanOfDiscs.m_numDiscs;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(numDiscsMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat minAngleForLaserFanMod = this.m_minAngleForLaserFanMod;
		string prefix2 = "[MinAngleForLaserFan]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = nekoFanOfDiscs.m_minAngleForLaserFan;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(minAngleForLaserFanMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat totalAngleForLaserFanMod = this.m_totalAngleForLaserFanMod;
		string prefix3 = "[TotalAngleForLaserFan]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = nekoFanOfDiscs.m_totalAngleForLaserFan;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(totalAngleForLaserFanMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_angleInterpMinDistMod, "[AngleInterpMinDist]", flag, (!flag) ? 0f : nekoFanOfDiscs.m_angleInterpMinDist);
		string str4 = text;
		AbilityModPropertyFloat angleInterpMaxDistMod = this.m_angleInterpMaxDistMod;
		string prefix4 = "[AngleInterpMaxDist]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = nekoFanOfDiscs.m_angleInterpMaxDist;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(angleInterpMaxDistMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix5 = "[LaserRange]";
		bool showBaseVal5 = flag;
		float baseVal5;
		if (flag)
		{
			baseVal5 = nekoFanOfDiscs.m_laserRange;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + base.PropDesc(laserRangeMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix6 = "[LaserWidth]";
		bool showBaseVal6 = flag;
		float baseVal6;
		if (flag)
		{
			baseVal6 = nekoFanOfDiscs.m_laserWidth;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + base.PropDesc(laserWidthMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_aoeRadiusAtEndMod, "[AoeRadiusAtEnd]", flag, (!flag) ? 0f : nekoFanOfDiscs.m_aoeRadiusAtEnd);
		string str7 = text;
		AbilityModPropertyInt maxTargetsPerLaserMod = this.m_maxTargetsPerLaserMod;
		string prefix7 = "[MaxTargetsPerLaser]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = nekoFanOfDiscs.m_maxTargetsPerLaser;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(maxTargetsPerLaserMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyFloat interpStepInSquaresMod = this.m_interpStepInSquaresMod;
		string prefix8 = "[InterpStepInSquares]";
		bool showBaseVal8 = flag;
		float baseVal8;
		if (flag)
		{
			baseVal8 = nekoFanOfDiscs.m_interpStepInSquares;
		}
		else
		{
			baseVal8 = 0f;
		}
		text = str8 + base.PropDesc(interpStepInSquaresMod, prefix8, showBaseVal8, baseVal8);
		text += base.PropDesc(this.m_discReturnEndRadiusMod, "[DiscReturnEndRadius]", flag, (!flag) ? 0f : nekoFanOfDiscs.m_discReturnEndRadius);
		string str9 = text;
		AbilityModPropertyInt directDamageMod = this.m_directDamageMod;
		string prefix9 = "[DirectDamage]";
		bool showBaseVal9 = flag;
		int baseVal9;
		if (flag)
		{
			baseVal9 = nekoFanOfDiscs.m_directDamage;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(directDamageMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt directSubsequentHitDamageMod = this.m_directSubsequentHitDamageMod;
		string prefix10 = "[DirectSubsequentHitDamage]";
		bool showBaseVal10 = flag;
		int baseVal10;
		if (flag)
		{
			baseVal10 = nekoFanOfDiscs.m_directSubsequentHitDamage;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(directSubsequentHitDamageMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyEffectInfo directEnemyHitEffectMod = this.m_directEnemyHitEffectMod;
		string prefix11 = "[DirectEnemyHitEffect]";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal11;
		if (flag)
		{
			baseVal11 = nekoFanOfDiscs.m_directEnemyHitEffect;
		}
		else
		{
			baseVal11 = null;
		}
		text = str11 + base.PropDesc(directEnemyHitEffectMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyInt returnTripDamageMod = this.m_returnTripDamageMod;
		string prefix12 = "[ReturnTripDamage]";
		bool showBaseVal12 = flag;
		int baseVal12;
		if (flag)
		{
			baseVal12 = nekoFanOfDiscs.m_returnTripDamage;
		}
		else
		{
			baseVal12 = 0;
		}
		text = str12 + base.PropDesc(returnTripDamageMod, prefix12, showBaseVal12, baseVal12);
		string str13 = text;
		AbilityModPropertyInt returnTripSubsequentHitDamageMod = this.m_returnTripSubsequentHitDamageMod;
		string prefix13 = "[ReturnTripSubsequentHitDamage]";
		bool showBaseVal13 = flag;
		int baseVal13;
		if (flag)
		{
			baseVal13 = nekoFanOfDiscs.m_returnTripSubsequentHitDamage;
		}
		else
		{
			baseVal13 = 0;
		}
		text = str13 + base.PropDesc(returnTripSubsequentHitDamageMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyBool returnTripIgnoreCoverMod = this.m_returnTripIgnoreCoverMod;
		string prefix14 = "[ReturnTripIgnoreCover]";
		bool showBaseVal14 = flag;
		bool baseVal14;
		if (flag)
		{
			baseVal14 = nekoFanOfDiscs.m_returnTripIgnoreCover;
		}
		else
		{
			baseVal14 = false;
		}
		text = str14 + base.PropDesc(returnTripIgnoreCoverMod, prefix14, showBaseVal14, baseVal14);
		string str15 = text;
		AbilityModPropertyInt returnTripEnergyOnCasterPerDiscMissMod = this.m_returnTripEnergyOnCasterPerDiscMissMod;
		string prefix15 = "[EnergyOnCasterPerDiscMiss]";
		bool showBaseVal15 = flag;
		int baseVal15;
		if (flag)
		{
			baseVal15 = nekoFanOfDiscs.m_returnTripEnergyOnCasterPerDiscMiss;
		}
		else
		{
			baseVal15 = 0;
		}
		text = str15 + base.PropDesc(returnTripEnergyOnCasterPerDiscMissMod, prefix15, showBaseVal15, baseVal15);
		string str16 = text;
		AbilityModPropertyEffectInfo effectOnSelfIfMissOnCastMod = this.m_effectOnSelfIfMissOnCastMod;
		string prefix16 = "[EffectOnSelfIfMissOnCast]";
		bool showBaseVal16 = flag;
		StandardEffectInfo baseVal16;
		if (flag)
		{
			baseVal16 = nekoFanOfDiscs.m_effectOnSelfIfMissOnCast;
		}
		else
		{
			baseVal16 = null;
		}
		text = str16 + base.PropDesc(effectOnSelfIfMissOnCastMod, prefix16, showBaseVal16, baseVal16);
		string str17 = text;
		AbilityModPropertyEffectInfo effectOnSelfIfMissOnDiscReturnMod = this.m_effectOnSelfIfMissOnDiscReturnMod;
		string prefix17 = "[EffectOnSelfIfMissOnDiscReturn]";
		bool showBaseVal17 = flag;
		StandardEffectInfo baseVal17;
		if (flag)
		{
			baseVal17 = nekoFanOfDiscs.m_effectOnSelfIfMissOnDiscReturn;
		}
		else
		{
			baseVal17 = null;
		}
		text = str17 + base.PropDesc(effectOnSelfIfMissOnDiscReturnMod, prefix17, showBaseVal17, baseVal17);
		string str18 = text;
		AbilityModPropertyInt zeroEnergyRequiredTurnsMod = this.m_zeroEnergyRequiredTurnsMod;
		string prefix18 = "[ZeroEnergyRequiredTurns]";
		bool showBaseVal18 = flag;
		int baseVal18;
		if (flag)
		{
			baseVal18 = nekoFanOfDiscs.m_zeroEnergyRequiredTurns;
		}
		else
		{
			baseVal18 = 0;
		}
		return str18 + base.PropDesc(zeroEnergyRequiredTurnsMod, prefix18, showBaseVal18, baseVal18);
	}
}
