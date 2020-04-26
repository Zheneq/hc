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
			AbilityMod.AddToken_ConeInfo(tokens, m_coneInfoMod, "ConeInfo", soldierDashAndOverwatch.m_coneInfo);
			AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", soldierDashAndOverwatch.m_laserInfo);
			AbilityMod.AddToken_EffectMod(tokens, m_selfHitEffectMod, "SelfHitEffect", soldierDashAndOverwatch.m_selfHitEffect);
			AbilityMod.AddToken(tokens, m_onCastAllyHitRadiusAroundDestMod, "OnCastAllyHitRadiusAroundDest", string.Empty, soldierDashAndOverwatch.m_onCastAllyHitRadiusAroundDest);
			AbilityMod.AddToken_EffectMod(tokens, m_onCastAllyHitEffectMod, "OnCastAllyHitEffect", soldierDashAndOverwatch.m_onCastAllyHitEffect);
			AbilityMod.AddToken(tokens, m_overwatchDamageMod, "OverwatchDamage", string.Empty, soldierDashAndOverwatch.m_coneDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_overwatchHitEffectMod, "OverwatchHitEffect", soldierDashAndOverwatch.m_overwatchHitEffect);
			AbilityMod.AddToken(tokens, m_nearDistThresholdMod, "NearDistThreshold", string.Empty, soldierDashAndOverwatch.m_nearDistThreshold);
			AbilityMod.AddToken(tokens, m_extraDamageForNearTargetsMod, "ExtraDamageForNearTargets", string.Empty, soldierDashAndOverwatch.m_extraDamageForNearTargets);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierDashAndOverwatch soldierDashAndOverwatch = GetTargetAbilityOnAbilityData(abilityData) as SoldierDashAndOverwatch;
		bool flag = soldierDashAndOverwatch != null;
		string empty = string.Empty;
		empty += PropDesc(m_onlyDashNextToCoverMod, "[OnlyDashNextToCover]", flag, flag && soldierDashAndOverwatch.m_onlyDashNextToCover);
		string str = empty;
		AbilityModPropertyConeInfo coneInfoMod = m_coneInfoMod;
		object baseConeInfo;
		if (flag)
		{
			baseConeInfo = soldierDashAndOverwatch.m_coneInfo;
		}
		else
		{
			baseConeInfo = null;
		}
		empty = str + PropDesc(coneInfoMod, "[ConeInfo]", flag, (ConeTargetingInfo)baseConeInfo);
		empty += PropDesc(m_laserInfoMod, "[LaserInfo]", flag, (!flag) ? null : soldierDashAndOverwatch.m_laserInfo);
		string str2 = empty;
		AbilityModPropertyEffectInfo selfHitEffectMod = m_selfHitEffectMod;
		object baseVal;
		if (flag)
		{
			baseVal = soldierDashAndOverwatch.m_selfHitEffect;
		}
		else
		{
			baseVal = null;
		}
		empty = str2 + PropDesc(selfHitEffectMod, "[SelfHitEffect]", flag, (StandardEffectInfo)baseVal);
		string str3 = empty;
		AbilityModPropertyFloat onCastAllyHitRadiusAroundDestMod = m_onCastAllyHitRadiusAroundDestMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = soldierDashAndOverwatch.m_onCastAllyHitRadiusAroundDest;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str3 + PropDesc(onCastAllyHitRadiusAroundDestMod, "[OnCastAllyHitRadiusAroundDest]", flag, baseVal2);
		string str4 = empty;
		AbilityModPropertyEffectInfo onCastAllyHitEffectMod = m_onCastAllyHitEffectMod;
		object baseVal3;
		if (flag)
		{
			baseVal3 = soldierDashAndOverwatch.m_onCastAllyHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str4 + PropDesc(onCastAllyHitEffectMod, "[OnCastAllyHitEffect]", flag, (StandardEffectInfo)baseVal3);
		string str5 = empty;
		AbilityModPropertyInt overwatchDamageMod = m_overwatchDamageMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = soldierDashAndOverwatch.m_coneDamage;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str5 + PropDesc(overwatchDamageMod, "[OverwatchDamage]", flag, baseVal4);
		empty += PropDesc(m_overwatchHitEffectMod, "[OverwatchHitEffect]", flag, (!flag) ? null : soldierDashAndOverwatch.m_overwatchHitEffect);
		empty += PropDesc(m_nearDistThresholdMod, "[NearDistThreshold]", flag, (!flag) ? 0f : soldierDashAndOverwatch.m_nearDistThreshold);
		return empty + PropDesc(m_extraDamageForNearTargetsMod, "[ExtraDamageForNearTargets]", flag, flag ? soldierDashAndOverwatch.m_extraDamageForNearTargets : 0);
	}
}
