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
			AbilityMod.AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, fishManStaticCone.m_coneWidthAngle);
			AbilityMod.AddToken(tokens, m_coneWidthAngleMod, "ConeWidthMinAngle", string.Empty, fishManStaticCone.m_coneWidthAngleMin);
			AbilityMod.AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, fishManStaticCone.m_coneLength);
			AbilityMod.AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, fishManStaticCone.m_coneBackwardOffset);
			AbilityMod.AddToken(tokens, m_damageToEnemiesMod, "DamageToEnemies", string.Empty, fishManStaticCone.m_damageToEnemies);
			AbilityMod.AddToken(tokens, m_damageToEnemiesMaxMod, "DamageToEnemiesMax", string.Empty, fishManStaticCone.m_damageToEnemiesMax);
			AbilityMod.AddToken_EffectMod(tokens, m_effectToEnemiesMod, "EffectToEnemies", fishManStaticCone.m_effectToEnemies);
			AbilityMod.AddToken(tokens, m_healingToAlliesMod, "HealingToAllies", string.Empty, fishManStaticCone.m_healingToAllies);
			AbilityMod.AddToken(tokens, m_healingToAlliesMaxMod, "HealingToAlliesMax", string.Empty, fishManStaticCone.m_healingToAlliesMax);
			AbilityMod.AddToken_EffectMod(tokens, m_effectToAlliesMod, "EffectToAllies", fishManStaticCone.m_effectToAllies);
			AbilityMod.AddToken(tokens, m_extraAllyHealForSingleHitMod, "ExtraAllyHealForSingleHit", string.Empty, fishManStaticCone.m_extraAllyHealForSingleHit);
			AbilityMod.AddToken_EffectMod(tokens, m_extraEffectOnClosestAllyMod, "ExtraEffectOnClosestAlly", fishManStaticCone.m_extraEffectOnClosestAlly);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, fishManStaticCone.m_maxTargets);
			AbilityMod.AddToken(tokens, m_healToCasterOnCastMod, "HealToCasterOnCast", string.Empty, fishManStaticCone.m_healToCasterOnCast);
			AbilityMod.AddToken(tokens, m_healToCasterPerEnemyHitMod, "HealToCasterPerEnemyHit", string.Empty, fishManStaticCone.m_healToCasterPerEnemyHit);
			AbilityMod.AddToken(tokens, m_healToCasterPerAllyHitMod, "HealToCasterPerAllyHit", string.Empty, fishManStaticCone.m_healToCasterPerAllyHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManStaticCone fishManStaticCone = GetTargetAbilityOnAbilityData(abilityData) as FishManStaticCone;
		bool flag = fishManStaticCone != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat coneWidthAngleMod = m_coneWidthAngleMod;
		float baseVal;
		if (flag)
		{
			baseVal = fishManStaticCone.m_coneWidthAngle;
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
			baseVal2 = fishManStaticCone.m_coneWidthAngleMin;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(coneWidthAngleMinMod, "[ConeWidthMinAngle]", flag, baseVal2);
		empty += PropDesc(m_coneLengthMod, "[ConeLength]", flag, (!flag) ? 0f : fishManStaticCone.m_coneLength);
		string str3 = empty;
		AbilityModPropertyFloat coneBackwardOffsetMod = m_coneBackwardOffsetMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = fishManStaticCone.m_coneBackwardOffset;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyBool penetrateLineOfSightMod = m_penetrateLineOfSightMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = (fishManStaticCone.m_penetrateLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyInt maxTargetsMod = m_maxTargetsMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = fishManStaticCone.m_maxTargets;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(maxTargetsMod, "[MaxTargets]", flag, baseVal5);
		empty += PropDesc(m_damageToEnemiesMod, "[DamageToEnemies]", flag, flag ? fishManStaticCone.m_damageToEnemies : 0);
		string str6 = empty;
		AbilityModPropertyInt damageToEnemiesMaxMod = m_damageToEnemiesMaxMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = fishManStaticCone.m_damageToEnemiesMax;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(damageToEnemiesMaxMod, "[DamageToEnemiesMax]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectInfo effectToEnemiesMod = m_effectToEnemiesMod;
		object baseVal7;
		if (flag)
		{
			baseVal7 = fishManStaticCone.m_effectToEnemies;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(effectToEnemiesMod, "[EffectToEnemies]", flag, (StandardEffectInfo)baseVal7);
		empty += PropDesc(m_healingToAlliesMod, "[HealingToAllies]", flag, flag ? fishManStaticCone.m_healingToAllies : 0);
		string str8 = empty;
		AbilityModPropertyInt healingToAlliesMaxMod = m_healingToAlliesMaxMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = fishManStaticCone.m_healingToAlliesMax;
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
			baseVal9 = fishManStaticCone.m_effectToAllies;
		}
		else
		{
			baseVal9 = null;
		}
		empty = str9 + PropDesc(effectToAlliesMod, "[EffectToAllies]", flag, (StandardEffectInfo)baseVal9);
		string str10 = empty;
		AbilityModPropertyInt extraAllyHealForSingleHitMod = m_extraAllyHealForSingleHitMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = fishManStaticCone.m_extraAllyHealForSingleHit;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(extraAllyHealForSingleHitMod, "[ExtraAllyHealForSingleHit]", flag, baseVal10);
		string str11 = empty;
		AbilityModPropertyEffectInfo extraEffectOnClosestAllyMod = m_extraEffectOnClosestAllyMod;
		object baseVal11;
		if (flag)
		{
			baseVal11 = fishManStaticCone.m_extraEffectOnClosestAlly;
		}
		else
		{
			baseVal11 = null;
		}
		empty = str11 + PropDesc(extraEffectOnClosestAllyMod, "[ExtraEffectOnClosestAlly]", flag, (StandardEffectInfo)baseVal11);
		empty += PropDesc(m_healToCasterOnCastMod, "[HealToCasterOnCast]", flag, flag ? fishManStaticCone.m_healToCasterOnCast : 0);
		string str12 = empty;
		AbilityModPropertyInt healToCasterPerEnemyHitMod = m_healToCasterPerEnemyHitMod;
		int baseVal12;
		if (flag)
		{
			baseVal12 = fishManStaticCone.m_healToCasterPerEnemyHit;
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
			baseVal13 = fishManStaticCone.m_healToCasterPerAllyHit;
		}
		else
		{
			baseVal13 = 0;
		}
		return str13 + PropDesc(healToCasterPerAllyHitMod, "[HealToCasterPerAllyHit]", flag, baseVal13);
	}
}
