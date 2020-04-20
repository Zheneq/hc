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
			AbilityMod.AddToken(tokens, this.m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, fishManCone.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneWidthAngleMinMod, "ConeWidthAngleMin", string.Empty, fishManCone.m_coneWidthAngleMin, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneLengthMod, "ConeLength", string.Empty, fishManCone.m_coneLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, fishManCone.m_coneBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageToEnemiesMod, "DamageToEnemies", string.Empty, fishManCone.m_damageToEnemies, true, false);
			AbilityMod.AddToken(tokens, this.m_damageToEnemiesMaxMod, "DamageToEnemiesMax", string.Empty, fishManCone.m_damageToEnemiesMax, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectToEnemiesMod, "EffectToEnemies", fishManCone.m_effectToEnemies, true);
			AbilityMod.AddToken(tokens, this.m_healingToAlliesMod, "HealingToAllies", string.Empty, fishManCone.m_healingToAllies, true, false);
			AbilityMod.AddToken(tokens, this.m_healingToAlliesMaxMod, "HealingToAlliesMax", string.Empty, fishManCone.m_healingToAlliesMax, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectToAlliesMod, "EffectToAllies", fishManCone.m_effectToAllies, true);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, fishManCone.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_healToCasterOnCastMod, "HealToCasterOnCast", string.Empty, fishManCone.m_healToCasterOnCast, true, false);
			AbilityMod.AddToken(tokens, this.m_healToCasterPerEnemyHitMod, "HealToCasterPerEnemyHit", string.Empty, fishManCone.m_healToCasterPerEnemyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_healToCasterPerAllyHitMod, "HealToCasterPerAllyHit", string.Empty, fishManCone.m_healToCasterPerAllyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_extraHealPerEnemyHitForNextHealConeMod, "ExtraHealPerEnemyHitForNextHealCone", string.Empty, fishManCone.m_extraHealPerEnemyHitForNextHealCone, true, false);
			AbilityMod.AddToken(tokens, this.m_extraEnergyForSingleEnemyHitMod, "ExtraEnergyForSingleEnemyHit", string.Empty, fishManCone.m_extraEnergyForSingleEnemyHit, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManCone fishManCone = base.GetTargetAbilityOnAbilityData(abilityData) as FishManCone;
		bool flag = fishManCone != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat coneWidthAngleMod = this.m_coneWidthAngleMod;
		string prefix = "[ConeWidthAngle]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = fishManCone.m_coneWidthAngle;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(coneWidthAngleMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat coneWidthAngleMinMod = this.m_coneWidthAngleMinMod;
		string prefix2 = "[ConeWidthAngleMin]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = fishManCone.m_coneWidthAngleMin;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(coneWidthAngleMinMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat coneLengthMod = this.m_coneLengthMod;
		string prefix3 = "[ConeLength]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = fishManCone.m_coneLength;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(coneLengthMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat coneBackwardOffsetMod = this.m_coneBackwardOffsetMod;
		string prefix4 = "[ConeBackwardOffset]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = fishManCone.m_coneBackwardOffset;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(coneBackwardOffsetMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool penetrateLineOfSightMod = this.m_penetrateLineOfSightMod;
		string prefix5 = "[PenetrateLineOfSight]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = fishManCone.m_penetrateLineOfSight;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(penetrateLineOfSightMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_damageToEnemiesMod, "[DamageToEnemies]", flag, (!flag) ? 0 : fishManCone.m_damageToEnemies);
		text += base.PropDesc(this.m_damageToEnemiesMaxMod, "[DamageToEnemiesMax]", flag, (!flag) ? 0 : fishManCone.m_damageToEnemiesMax);
		string str6 = text;
		AbilityModPropertyEffectInfo effectToEnemiesMod = this.m_effectToEnemiesMod;
		string prefix6 = "[EffectToEnemies]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
		if (flag)
		{
			baseVal6 = fishManCone.m_effectToEnemies;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(effectToEnemiesMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt healingToAlliesMod = this.m_healingToAlliesMod;
		string prefix7 = "[HealingToAllies]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = fishManCone.m_healingToAllies;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(healingToAlliesMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt healingToAlliesMaxMod = this.m_healingToAlliesMaxMod;
		string prefix8 = "[HealingToAlliesMax]";
		bool showBaseVal8 = flag;
		int baseVal8;
		if (flag)
		{
			baseVal8 = fishManCone.m_healingToAlliesMax;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(healingToAlliesMaxMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyEffectInfo effectToAlliesMod = this.m_effectToAlliesMod;
		string prefix9 = "[EffectToAllies]";
		bool showBaseVal9 = flag;
		StandardEffectInfo baseVal9;
		if (flag)
		{
			baseVal9 = fishManCone.m_effectToAllies;
		}
		else
		{
			baseVal9 = null;
		}
		text = str9 + base.PropDesc(effectToAlliesMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt maxTargetsMod = this.m_maxTargetsMod;
		string prefix10 = "[MaxTargets]";
		bool showBaseVal10 = flag;
		int baseVal10;
		if (flag)
		{
			baseVal10 = fishManCone.m_maxTargets;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(maxTargetsMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyInt healToCasterOnCastMod = this.m_healToCasterOnCastMod;
		string prefix11 = "[HealToCasterOnCast]";
		bool showBaseVal11 = flag;
		int baseVal11;
		if (flag)
		{
			baseVal11 = fishManCone.m_healToCasterOnCast;
		}
		else
		{
			baseVal11 = 0;
		}
		text = str11 + base.PropDesc(healToCasterOnCastMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyInt healToCasterPerEnemyHitMod = this.m_healToCasterPerEnemyHitMod;
		string prefix12 = "[HealToCasterPerEnemyHit]";
		bool showBaseVal12 = flag;
		int baseVal12;
		if (flag)
		{
			baseVal12 = fishManCone.m_healToCasterPerEnemyHit;
		}
		else
		{
			baseVal12 = 0;
		}
		text = str12 + base.PropDesc(healToCasterPerEnemyHitMod, prefix12, showBaseVal12, baseVal12);
		string str13 = text;
		AbilityModPropertyInt healToCasterPerAllyHitMod = this.m_healToCasterPerAllyHitMod;
		string prefix13 = "[HealToCasterPerAllyHit]";
		bool showBaseVal13 = flag;
		int baseVal13;
		if (flag)
		{
			baseVal13 = fishManCone.m_healToCasterPerAllyHit;
		}
		else
		{
			baseVal13 = 0;
		}
		text = str13 + base.PropDesc(healToCasterPerAllyHitMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyInt extraHealPerEnemyHitForNextHealConeMod = this.m_extraHealPerEnemyHitForNextHealConeMod;
		string prefix14 = "[ExtraHealPerEnemyHitForNextHealCone]";
		bool showBaseVal14 = flag;
		int baseVal14;
		if (flag)
		{
			baseVal14 = fishManCone.m_extraHealPerEnemyHitForNextHealCone;
		}
		else
		{
			baseVal14 = 0;
		}
		text = str14 + base.PropDesc(extraHealPerEnemyHitForNextHealConeMod, prefix14, showBaseVal14, baseVal14);
		string str15 = text;
		AbilityModPropertyInt extraEnergyForSingleEnemyHitMod = this.m_extraEnergyForSingleEnemyHitMod;
		string prefix15 = "[ExtraEnergyForSingleEnemyHit]";
		bool showBaseVal15 = flag;
		int baseVal15;
		if (flag)
		{
			baseVal15 = fishManCone.m_extraEnergyForSingleEnemyHit;
		}
		else
		{
			baseVal15 = 0;
		}
		return str15 + base.PropDesc(extraEnergyForSingleEnemyHitMod, prefix15, showBaseVal15, baseVal15);
	}
}
