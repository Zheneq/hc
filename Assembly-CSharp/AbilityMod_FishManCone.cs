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
		if (!(fishManCone != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, fishManCone.m_coneWidthAngle);
			AbilityMod.AddToken(tokens, m_coneWidthAngleMinMod, "ConeWidthAngleMin", string.Empty, fishManCone.m_coneWidthAngleMin);
			AbilityMod.AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, fishManCone.m_coneLength);
			AbilityMod.AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, fishManCone.m_coneBackwardOffset);
			AbilityMod.AddToken(tokens, m_damageToEnemiesMod, "DamageToEnemies", string.Empty, fishManCone.m_damageToEnemies);
			AbilityMod.AddToken(tokens, m_damageToEnemiesMaxMod, "DamageToEnemiesMax", string.Empty, fishManCone.m_damageToEnemiesMax);
			AbilityMod.AddToken_EffectMod(tokens, m_effectToEnemiesMod, "EffectToEnemies", fishManCone.m_effectToEnemies);
			AbilityMod.AddToken(tokens, m_healingToAlliesMod, "HealingToAllies", string.Empty, fishManCone.m_healingToAllies);
			AbilityMod.AddToken(tokens, m_healingToAlliesMaxMod, "HealingToAlliesMax", string.Empty, fishManCone.m_healingToAlliesMax);
			AbilityMod.AddToken_EffectMod(tokens, m_effectToAlliesMod, "EffectToAllies", fishManCone.m_effectToAllies);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, fishManCone.m_maxTargets);
			AbilityMod.AddToken(tokens, m_healToCasterOnCastMod, "HealToCasterOnCast", string.Empty, fishManCone.m_healToCasterOnCast);
			AbilityMod.AddToken(tokens, m_healToCasterPerEnemyHitMod, "HealToCasterPerEnemyHit", string.Empty, fishManCone.m_healToCasterPerEnemyHit);
			AbilityMod.AddToken(tokens, m_healToCasterPerAllyHitMod, "HealToCasterPerAllyHit", string.Empty, fishManCone.m_healToCasterPerAllyHit);
			AbilityMod.AddToken(tokens, m_extraHealPerEnemyHitForNextHealConeMod, "ExtraHealPerEnemyHitForNextHealCone", string.Empty, fishManCone.m_extraHealPerEnemyHitForNextHealCone);
			AbilityMod.AddToken(tokens, m_extraEnergyForSingleEnemyHitMod, "ExtraEnergyForSingleEnemyHit", string.Empty, fishManCone.m_extraEnergyForSingleEnemyHit);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManCone fishManCone = GetTargetAbilityOnAbilityData(abilityData) as FishManCone;
		bool flag = fishManCone != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat coneWidthAngleMod = m_coneWidthAngleMod;
		float baseVal;
		if (flag)
		{
			baseVal = fishManCone.m_coneWidthAngle;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(coneWidthAngleMod, "[ConeWidthAngle]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat coneWidthAngleMinMod = m_coneWidthAngleMinMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = fishManCone.m_coneWidthAngleMin;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(coneWidthAngleMinMod, "[ConeWidthAngleMin]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat coneLengthMod = m_coneLengthMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = fishManCone.m_coneLength;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(coneLengthMod, "[ConeLength]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat coneBackwardOffsetMod = m_coneBackwardOffsetMod;
		float baseVal4;
		if (flag)
		{
			baseVal4 = fishManCone.m_coneBackwardOffset;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyBool penetrateLineOfSightMod = m_penetrateLineOfSightMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = (fishManCone.m_penetrateLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, (byte)baseVal5 != 0);
		empty += PropDesc(m_damageToEnemiesMod, "[DamageToEnemies]", flag, flag ? fishManCone.m_damageToEnemies : 0);
		empty += PropDesc(m_damageToEnemiesMaxMod, "[DamageToEnemiesMax]", flag, flag ? fishManCone.m_damageToEnemiesMax : 0);
		string str6 = empty;
		AbilityModPropertyEffectInfo effectToEnemiesMod = m_effectToEnemiesMod;
		object baseVal6;
		if (flag)
		{
			baseVal6 = fishManCone.m_effectToEnemies;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(effectToEnemiesMod, "[EffectToEnemies]", flag, (StandardEffectInfo)baseVal6);
		string str7 = empty;
		AbilityModPropertyInt healingToAlliesMod = m_healingToAlliesMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = fishManCone.m_healingToAllies;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(healingToAlliesMod, "[HealingToAllies]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyInt healingToAlliesMaxMod = m_healingToAlliesMaxMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = fishManCone.m_healingToAlliesMax;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(healingToAlliesMaxMod, "[HealingToAlliesMax]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyEffectInfo effectToAlliesMod = m_effectToAlliesMod;
		object baseVal9;
		if (flag)
		{
			baseVal9 = fishManCone.m_effectToAllies;
		}
		else
		{
			baseVal9 = null;
		}
		empty = str9 + PropDesc(effectToAlliesMod, "[EffectToAllies]", flag, (StandardEffectInfo)baseVal9);
		string str10 = empty;
		AbilityModPropertyInt maxTargetsMod = m_maxTargetsMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = fishManCone.m_maxTargets;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(maxTargetsMod, "[MaxTargets]", flag, baseVal10);
		string str11 = empty;
		AbilityModPropertyInt healToCasterOnCastMod = m_healToCasterOnCastMod;
		int baseVal11;
		if (flag)
		{
			baseVal11 = fishManCone.m_healToCasterOnCast;
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str11 + PropDesc(healToCasterOnCastMod, "[HealToCasterOnCast]", flag, baseVal11);
		string str12 = empty;
		AbilityModPropertyInt healToCasterPerEnemyHitMod = m_healToCasterPerEnemyHitMod;
		int baseVal12;
		if (flag)
		{
			baseVal12 = fishManCone.m_healToCasterPerEnemyHit;
		}
		else
		{
			baseVal12 = 0;
		}
		empty = str12 + PropDesc(healToCasterPerEnemyHitMod, "[HealToCasterPerEnemyHit]", flag, baseVal12);
		string str13 = empty;
		AbilityModPropertyInt healToCasterPerAllyHitMod = m_healToCasterPerAllyHitMod;
		int baseVal13;
		if (flag)
		{
			baseVal13 = fishManCone.m_healToCasterPerAllyHit;
		}
		else
		{
			baseVal13 = 0;
		}
		empty = str13 + PropDesc(healToCasterPerAllyHitMod, "[HealToCasterPerAllyHit]", flag, baseVal13);
		string str14 = empty;
		AbilityModPropertyInt extraHealPerEnemyHitForNextHealConeMod = m_extraHealPerEnemyHitForNextHealConeMod;
		int baseVal14;
		if (flag)
		{
			baseVal14 = fishManCone.m_extraHealPerEnemyHitForNextHealCone;
		}
		else
		{
			baseVal14 = 0;
		}
		empty = str14 + PropDesc(extraHealPerEnemyHitForNextHealConeMod, "[ExtraHealPerEnemyHitForNextHealCone]", flag, baseVal14);
		string str15 = empty;
		AbilityModPropertyInt extraEnergyForSingleEnemyHitMod = m_extraEnergyForSingleEnemyHitMod;
		int baseVal15;
		if (flag)
		{
			baseVal15 = fishManCone.m_extraEnergyForSingleEnemyHit;
		}
		else
		{
			baseVal15 = 0;
		}
		return str15 + PropDesc(extraEnergyForSingleEnemyHitMod, "[ExtraEnergyForSingleEnemyHit]", flag, baseVal15);
	}
}
