using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SoldierDashAndOverwatch : AbilityMod
{
	[Header("-- Targeting --")]
	public AbilityModPropertyBool m_onlyDashNextToCoverMod;

	[Header("  Targeting: For Cone")]
	public AbilityModPropertyConeInfo m_coneInfoMod;

	[Header("  Targeting: For Laser")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;

	[Header("-- On Cast Hit --")]
	public AbilityModPropertyEffectInfo m_selfHitEffectMod;

	public AbilityModPropertyFloat m_onCastAllyHitRadiusAroundDestMod;

	public AbilityModPropertyEffectInfo m_onCastAllyHitEffectMod;

	[Header("-- On Overwatch Hit Stuff --")]
	public AbilityModPropertyInt m_overwatchDamageMod;

	public AbilityModPropertyEffectInfo m_overwatchHitEffectMod;

	public AbilityModPropertyFloat m_nearDistThresholdMod;

	public AbilityModPropertyInt m_extraDamageForNearTargetsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SoldierDashAndOverwatch);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SoldierDashAndOverwatch soldierDashAndOverwatch = targetAbility as SoldierDashAndOverwatch;
		if (soldierDashAndOverwatch != null)
		{
			AbilityMod.AddToken_ConeInfo(tokens, this.m_coneInfoMod, "ConeInfo", soldierDashAndOverwatch.m_coneInfo, true);
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "LaserInfo", soldierDashAndOverwatch.m_laserInfo, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfHitEffectMod, "SelfHitEffect", soldierDashAndOverwatch.m_selfHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_onCastAllyHitRadiusAroundDestMod, "OnCastAllyHitRadiusAroundDest", string.Empty, soldierDashAndOverwatch.m_onCastAllyHitRadiusAroundDest, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_onCastAllyHitEffectMod, "OnCastAllyHitEffect", soldierDashAndOverwatch.m_onCastAllyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_overwatchDamageMod, "OverwatchDamage", string.Empty, soldierDashAndOverwatch.m_coneDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_overwatchHitEffectMod, "OverwatchHitEffect", soldierDashAndOverwatch.m_overwatchHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_nearDistThresholdMod, "NearDistThreshold", string.Empty, soldierDashAndOverwatch.m_nearDistThreshold, true, false, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForNearTargetsMod, "ExtraDamageForNearTargets", string.Empty, soldierDashAndOverwatch.m_extraDamageForNearTargets, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierDashAndOverwatch soldierDashAndOverwatch = base.GetTargetAbilityOnAbilityData(abilityData) as SoldierDashAndOverwatch;
		bool flag = soldierDashAndOverwatch != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_onlyDashNextToCoverMod, "[OnlyDashNextToCover]", flag, flag && soldierDashAndOverwatch.m_onlyDashNextToCover);
		string str = text;
		AbilityModPropertyConeInfo coneInfoMod = this.m_coneInfoMod;
		string prefix = "[ConeInfo]";
		bool showBaseVal = flag;
		ConeTargetingInfo baseConeInfo;
		if (flag)
		{
			baseConeInfo = soldierDashAndOverwatch.m_coneInfo;
		}
		else
		{
			baseConeInfo = null;
		}
		text = str + base.PropDesc(coneInfoMod, prefix, showBaseVal, baseConeInfo);
		text += base.PropDesc(this.m_laserInfoMod, "[LaserInfo]", flag, (!flag) ? null : soldierDashAndOverwatch.m_laserInfo);
		string str2 = text;
		AbilityModPropertyEffectInfo selfHitEffectMod = this.m_selfHitEffectMod;
		string prefix2 = "[SelfHitEffect]";
		bool showBaseVal2 = flag;
		StandardEffectInfo baseVal;
		if (flag)
		{
			baseVal = soldierDashAndOverwatch.m_selfHitEffect;
		}
		else
		{
			baseVal = null;
		}
		text = str2 + base.PropDesc(selfHitEffectMod, prefix2, showBaseVal2, baseVal);
		string str3 = text;
		AbilityModPropertyFloat onCastAllyHitRadiusAroundDestMod = this.m_onCastAllyHitRadiusAroundDestMod;
		string prefix3 = "[OnCastAllyHitRadiusAroundDest]";
		bool showBaseVal3 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = soldierDashAndOverwatch.m_onCastAllyHitRadiusAroundDest;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str3 + base.PropDesc(onCastAllyHitRadiusAroundDestMod, prefix3, showBaseVal3, baseVal2);
		string str4 = text;
		AbilityModPropertyEffectInfo onCastAllyHitEffectMod = this.m_onCastAllyHitEffectMod;
		string prefix4 = "[OnCastAllyHitEffect]";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal3;
		if (flag)
		{
			baseVal3 = soldierDashAndOverwatch.m_onCastAllyHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		text = str4 + base.PropDesc(onCastAllyHitEffectMod, prefix4, showBaseVal4, baseVal3);
		string str5 = text;
		AbilityModPropertyInt overwatchDamageMod = this.m_overwatchDamageMod;
		string prefix5 = "[OverwatchDamage]";
		bool showBaseVal5 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = soldierDashAndOverwatch.m_coneDamage;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str5 + base.PropDesc(overwatchDamageMod, prefix5, showBaseVal5, baseVal4);
		text += base.PropDesc(this.m_overwatchHitEffectMod, "[OverwatchHitEffect]", flag, (!flag) ? null : soldierDashAndOverwatch.m_overwatchHitEffect);
		text += base.PropDesc(this.m_nearDistThresholdMod, "[NearDistThreshold]", flag, (!flag) ? 0f : soldierDashAndOverwatch.m_nearDistThreshold);
		return text + base.PropDesc(this.m_extraDamageForNearTargetsMod, "[ExtraDamageForNearTargets]", flag, (!flag) ? 0 : soldierDashAndOverwatch.m_extraDamageForNearTargets);
	}
}
