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
			AbilityMod.AddToken(tokens, this.m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, fishManStaticCone.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneWidthAngleMod, "ConeWidthMinAngle", string.Empty, fishManStaticCone.m_coneWidthAngleMin, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneLengthMod, "ConeLength", string.Empty, fishManStaticCone.m_coneLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, fishManStaticCone.m_coneBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageToEnemiesMod, "DamageToEnemies", string.Empty, fishManStaticCone.m_damageToEnemies, true, false);
			AbilityMod.AddToken(tokens, this.m_damageToEnemiesMaxMod, "DamageToEnemiesMax", string.Empty, fishManStaticCone.m_damageToEnemiesMax, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectToEnemiesMod, "EffectToEnemies", fishManStaticCone.m_effectToEnemies, true);
			AbilityMod.AddToken(tokens, this.m_healingToAlliesMod, "HealingToAllies", string.Empty, fishManStaticCone.m_healingToAllies, true, false);
			AbilityMod.AddToken(tokens, this.m_healingToAlliesMaxMod, "HealingToAlliesMax", string.Empty, fishManStaticCone.m_healingToAlliesMax, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectToAlliesMod, "EffectToAllies", fishManStaticCone.m_effectToAllies, true);
			AbilityMod.AddToken(tokens, this.m_extraAllyHealForSingleHitMod, "ExtraAllyHealForSingleHit", string.Empty, fishManStaticCone.m_extraAllyHealForSingleHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_extraEffectOnClosestAllyMod, "ExtraEffectOnClosestAlly", fishManStaticCone.m_extraEffectOnClosestAlly, true);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, fishManStaticCone.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_healToCasterOnCastMod, "HealToCasterOnCast", string.Empty, fishManStaticCone.m_healToCasterOnCast, true, false);
			AbilityMod.AddToken(tokens, this.m_healToCasterPerEnemyHitMod, "HealToCasterPerEnemyHit", string.Empty, fishManStaticCone.m_healToCasterPerEnemyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_healToCasterPerAllyHitMod, "HealToCasterPerAllyHit", string.Empty, fishManStaticCone.m_healToCasterPerAllyHit, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManStaticCone fishManStaticCone = base.GetTargetAbilityOnAbilityData(abilityData) as FishManStaticCone;
		bool flag = fishManStaticCone != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat coneWidthAngleMod = this.m_coneWidthAngleMod;
		string prefix = "[ConeWidthAngle]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = fishManStaticCone.m_coneWidthAngle;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(coneWidthAngleMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat coneWidthAngleMinMod = this.m_coneWidthAngleMinMod;
		string prefix2 = "[ConeWidthMinAngle]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = fishManStaticCone.m_coneWidthAngleMin;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(coneWidthAngleMinMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_coneLengthMod, "[ConeLength]", flag, (!flag) ? 0f : fishManStaticCone.m_coneLength);
		string str3 = text;
		AbilityModPropertyFloat coneBackwardOffsetMod = this.m_coneBackwardOffsetMod;
		string prefix3 = "[ConeBackwardOffset]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = fishManStaticCone.m_coneBackwardOffset;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(coneBackwardOffsetMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool penetrateLineOfSightMod = this.m_penetrateLineOfSightMod;
		string prefix4 = "[PenetrateLineOfSight]";
		bool showBaseVal4 = flag;
		bool baseVal4;
		if (flag)
		{
			baseVal4 = fishManStaticCone.m_penetrateLineOfSight;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(penetrateLineOfSightMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt maxTargetsMod = this.m_maxTargetsMod;
		string prefix5 = "[MaxTargets]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = fishManStaticCone.m_maxTargets;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(maxTargetsMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_damageToEnemiesMod, "[DamageToEnemies]", flag, (!flag) ? 0 : fishManStaticCone.m_damageToEnemies);
		string str6 = text;
		AbilityModPropertyInt damageToEnemiesMaxMod = this.m_damageToEnemiesMaxMod;
		string prefix6 = "[DamageToEnemiesMax]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = fishManStaticCone.m_damageToEnemiesMax;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(damageToEnemiesMaxMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectInfo effectToEnemiesMod = this.m_effectToEnemiesMod;
		string prefix7 = "[EffectToEnemies]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
		if (flag)
		{
			baseVal7 = fishManStaticCone.m_effectToEnemies;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(effectToEnemiesMod, prefix7, showBaseVal7, baseVal7);
		text += base.PropDesc(this.m_healingToAlliesMod, "[HealingToAllies]", flag, (!flag) ? 0 : fishManStaticCone.m_healingToAllies);
		string str8 = text;
		AbilityModPropertyInt healingToAlliesMaxMod = this.m_healingToAlliesMaxMod;
		string prefix8 = "[HealingToAlliesMax]";
		bool showBaseVal8 = flag;
		int baseVal8;
		if (flag)
		{
			baseVal8 = fishManStaticCone.m_healingToAlliesMax;
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
			baseVal9 = fishManStaticCone.m_effectToAllies;
		}
		else
		{
			baseVal9 = null;
		}
		text = str9 + base.PropDesc(effectToAlliesMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt extraAllyHealForSingleHitMod = this.m_extraAllyHealForSingleHitMod;
		string prefix10 = "[ExtraAllyHealForSingleHit]";
		bool showBaseVal10 = flag;
		int baseVal10;
		if (flag)
		{
			baseVal10 = fishManStaticCone.m_extraAllyHealForSingleHit;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(extraAllyHealForSingleHitMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyEffectInfo extraEffectOnClosestAllyMod = this.m_extraEffectOnClosestAllyMod;
		string prefix11 = "[ExtraEffectOnClosestAlly]";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal11;
		if (flag)
		{
			baseVal11 = fishManStaticCone.m_extraEffectOnClosestAlly;
		}
		else
		{
			baseVal11 = null;
		}
		text = str11 + base.PropDesc(extraEffectOnClosestAllyMod, prefix11, showBaseVal11, baseVal11);
		text += base.PropDesc(this.m_healToCasterOnCastMod, "[HealToCasterOnCast]", flag, (!flag) ? 0 : fishManStaticCone.m_healToCasterOnCast);
		string str12 = text;
		AbilityModPropertyInt healToCasterPerEnemyHitMod = this.m_healToCasterPerEnemyHitMod;
		string prefix12 = "[HealToCasterPerEnemyHit]";
		bool showBaseVal12 = flag;
		int baseVal12;
		if (flag)
		{
			baseVal12 = fishManStaticCone.m_healToCasterPerEnemyHit;
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
			baseVal13 = fishManStaticCone.m_healToCasterPerAllyHit;
		}
		else
		{
			baseVal13 = 0;
		}
		return str13 + base.PropDesc(healToCasterPerAllyHitMod, prefix13, showBaseVal13, baseVal13);
	}
}
