using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_MantaDashThroughWall : AbilityMod
{
	[Header("-- Charge Targeting")]
	public AbilityModPropertyFloat m_aoeConeWidthMod;

	public AbilityModPropertyFloat m_aoeConeLengthMod;

	public AbilityModPropertyFloat m_aoeThroughWallConeWidthMod;

	public AbilityModPropertyFloat m_aoeThroughWallConeLengthMod;

	public AbilityModPropertyFloat m_widthMod;

	public AbilityModPropertyFloat m_maxRangeMod;

	public AbilityModPropertyFloat m_maxWidthOfWallMod;

	public AbilityModPropertyFloat m_extraTotalDistanceIfThroughWallsMod;

	public AbilityModPropertyBool m_clampConeToWallMod;

	public AbilityModPropertyBool m_aoeWithMissMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	[Header("-- Normal On Hit Damage, Effect, etc")]
	public AbilityModPropertyInt m_directHitDamageMod;

	public AbilityModPropertyEffectInfo m_directEnemyHitEffectMod;

	public AbilityModPropertyBool m_directHitIgnoreCoverMod;

	[Space(10f)]
	public AbilityModPropertyInt m_aoeDamageMod;

	public AbilityModPropertyEffectInfo m_aoeEnemyHitEffectMod;

	[Space(10f)]
	public AbilityModPropertyInt m_aoeThroughWallsDamageMod;

	public AbilityModPropertyEffectInfo m_aoeThroughWallsEffectMod;

	[Header("-- Other")]
	public AbilityModPropertyEffectInfo m_additionalDirtyFightingExplosionEffect;

	public AbilityModCooldownReduction m_cooldownReductionsWhenNoHits;

	public override Type GetTargetAbilityType()
	{
		return typeof(MantaDashThroughWall);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MantaDashThroughWall mantaDashThroughWall = targetAbility as MantaDashThroughWall;
		if (mantaDashThroughWall != null)
		{
			AbilityMod.AddToken(tokens, this.m_aoeConeWidthMod, "AoeConeWidth", string.Empty, mantaDashThroughWall.m_aoeConeWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_aoeConeLengthMod, "AoeConeLength", string.Empty, mantaDashThroughWall.m_aoeConeLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_aoeThroughWallConeWidthMod, "AoeThroughWallConeWidth", string.Empty, mantaDashThroughWall.m_aoeThroughWallConeWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_aoeThroughWallConeLengthMod, "AoeThroughWallConeLength", string.Empty, mantaDashThroughWall.m_aoeThroughWallConeLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_widthMod, "Width", string.Empty, mantaDashThroughWall.m_width, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxRangeMod, "MaxRange", string.Empty, mantaDashThroughWall.m_maxRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxWidthOfWallMod, "MaxWidthOfWall", string.Empty, mantaDashThroughWall.m_maxWidthOfWall, true, false, false);
			AbilityMod.AddToken(tokens, this.m_extraTotalDistanceIfThroughWallsMod, "ExtraTotalDistanceIfThroughWalls", string.Empty, mantaDashThroughWall.m_extraTotalDistanceIfThroughWalls, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, mantaDashThroughWall.m_coneBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_directHitDamageMod, "DirectHitDamage", string.Empty, mantaDashThroughWall.m_directHitDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_directEnemyHitEffectMod, "DirectEnemyHitEffect", mantaDashThroughWall.m_directEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_aoeDamageMod, "AoeDamage", string.Empty, mantaDashThroughWall.m_aoeDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_aoeEnemyHitEffectMod, "AoeEnemyHitEffect", mantaDashThroughWall.m_aoeEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_aoeThroughWallsDamageMod, "AoeThroughWallsDamage", string.Empty, mantaDashThroughWall.m_aoeThroughWallsDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_aoeThroughWallsEffectMod, "AoeThroughWallsEffect", mantaDashThroughWall.m_aoeThroughWallsEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_additionalDirtyFightingExplosionEffect, "ExtraDirtyFightingExplosionEffect", null, true);
			this.m_cooldownReductionsWhenNoHits.AddTooltipTokens(tokens, "CooldownReductionOnMiss");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MantaDashThroughWall mantaDashThroughWall = base.GetTargetAbilityOnAbilityData(abilityData) as MantaDashThroughWall;
		bool flag = mantaDashThroughWall != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_aoeConeWidthMod, "[AoeConeWidth]", flag, (!flag) ? 0f : mantaDashThroughWall.m_aoeConeWidth);
		string str = text;
		AbilityModPropertyFloat aoeConeLengthMod = this.m_aoeConeLengthMod;
		string prefix = "[AoeConeLength]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = mantaDashThroughWall.m_aoeConeLength;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(aoeConeLengthMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_aoeThroughWallConeWidthMod, "[AoeThroughWallConeWidth]", flag, (!flag) ? 0f : mantaDashThroughWall.m_aoeThroughWallConeWidth);
		string str2 = text;
		AbilityModPropertyFloat aoeThroughWallConeLengthMod = this.m_aoeThroughWallConeLengthMod;
		string prefix2 = "[AoeThroughWallConeLength]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = mantaDashThroughWall.m_aoeThroughWallConeLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(aoeThroughWallConeLengthMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_widthMod, "[Width]", flag, (!flag) ? 0f : mantaDashThroughWall.m_width);
		string str3 = text;
		AbilityModPropertyFloat maxRangeMod = this.m_maxRangeMod;
		string prefix3 = "[MaxRange]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = mantaDashThroughWall.m_maxRange;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(maxRangeMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat maxWidthOfWallMod = this.m_maxWidthOfWallMod;
		string prefix4 = "[MaxWidthOfWall]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = mantaDashThroughWall.m_maxWidthOfWall;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(maxWidthOfWallMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyFloat extraTotalDistanceIfThroughWallsMod = this.m_extraTotalDistanceIfThroughWallsMod;
		string prefix5 = "[ExtraTotalDistanceIfThroughWalls]";
		bool showBaseVal5 = flag;
		float baseVal5;
		if (flag)
		{
			baseVal5 = mantaDashThroughWall.m_extraTotalDistanceIfThroughWalls;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + base.PropDesc(extraTotalDistanceIfThroughWallsMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyBool clampConeToWallMod = this.m_clampConeToWallMod;
		string prefix6 = "[ClampConeToWall]";
		bool showBaseVal6 = flag;
		bool baseVal6;
		if (flag)
		{
			baseVal6 = mantaDashThroughWall.m_clampConeToWall;
		}
		else
		{
			baseVal6 = false;
		}
		text = str6 + base.PropDesc(clampConeToWallMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyBool aoeWithMissMod = this.m_aoeWithMissMod;
		string prefix7 = "[AoeWithMiss]";
		bool showBaseVal7 = flag;
		bool baseVal7;
		if (flag)
		{
			baseVal7 = mantaDashThroughWall.m_aoeWithMiss;
		}
		else
		{
			baseVal7 = false;
		}
		text = str7 + base.PropDesc(aoeWithMissMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyFloat coneBackwardOffsetMod = this.m_coneBackwardOffsetMod;
		string prefix8 = "[ConeBackwardOffset]";
		bool showBaseVal8 = flag;
		float baseVal8;
		if (flag)
		{
			baseVal8 = mantaDashThroughWall.m_coneBackwardOffset;
		}
		else
		{
			baseVal8 = 0f;
		}
		text = str8 + base.PropDesc(coneBackwardOffsetMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt directHitDamageMod = this.m_directHitDamageMod;
		string prefix9 = "[DirectHitDamage]";
		bool showBaseVal9 = flag;
		int baseVal9;
		if (flag)
		{
			baseVal9 = mantaDashThroughWall.m_directHitDamage;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(directHitDamageMod, prefix9, showBaseVal9, baseVal9);
		text += base.PropDesc(this.m_directEnemyHitEffectMod, "[DirectEnemyHitEffect]", flag, (!flag) ? null : mantaDashThroughWall.m_directEnemyHitEffect);
		string str10 = text;
		AbilityModPropertyBool directHitIgnoreCoverMod = this.m_directHitIgnoreCoverMod;
		string prefix10 = "[DirectHitIgnoreCover]";
		bool showBaseVal10 = flag;
		bool baseVal10;
		if (flag)
		{
			baseVal10 = mantaDashThroughWall.m_directHitIgnoreCover;
		}
		else
		{
			baseVal10 = false;
		}
		text = str10 + base.PropDesc(directHitIgnoreCoverMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyInt aoeDamageMod = this.m_aoeDamageMod;
		string prefix11 = "[AoeDamage]";
		bool showBaseVal11 = flag;
		int baseVal11;
		if (flag)
		{
			baseVal11 = mantaDashThroughWall.m_aoeDamage;
		}
		else
		{
			baseVal11 = 0;
		}
		text = str11 + base.PropDesc(aoeDamageMod, prefix11, showBaseVal11, baseVal11);
		text += base.PropDesc(this.m_aoeEnemyHitEffectMod, "[AoeEnemyHitEffect]", flag, (!flag) ? null : mantaDashThroughWall.m_aoeEnemyHitEffect);
		text += base.PropDesc(this.m_aoeThroughWallsDamageMod, "[AoeThroughWallsDamage]", flag, (!flag) ? 0 : mantaDashThroughWall.m_aoeThroughWallsDamage);
		text += base.PropDesc(this.m_aoeThroughWallsEffectMod, "[AoeThroughWallsEffect]", flag, (!flag) ? null : mantaDashThroughWall.m_aoeThroughWallsEffect);
		text += base.PropDesc(this.m_additionalDirtyFightingExplosionEffect, "[ExtraDirtyFightingExplosionEffect]", flag, null);
		if (this.m_cooldownReductionsWhenNoHits.HasCooldownReduction())
		{
			text += this.m_cooldownReductionsWhenNoHits.GetDescription(abilityData);
		}
		return text;
	}
}
