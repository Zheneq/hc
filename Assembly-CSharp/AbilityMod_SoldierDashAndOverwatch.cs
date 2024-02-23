using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken_ConeInfo(tokens, m_coneInfoMod, "ConeInfo", soldierDashAndOverwatch.m_coneInfo);
			AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", soldierDashAndOverwatch.m_laserInfo);
			AddToken_EffectMod(tokens, m_selfHitEffectMod, "SelfHitEffect", soldierDashAndOverwatch.m_selfHitEffect);
			AddToken(tokens, m_onCastAllyHitRadiusAroundDestMod, "OnCastAllyHitRadiusAroundDest", string.Empty, soldierDashAndOverwatch.m_onCastAllyHitRadiusAroundDest);
			AddToken_EffectMod(tokens, m_onCastAllyHitEffectMod, "OnCastAllyHitEffect", soldierDashAndOverwatch.m_onCastAllyHitEffect);
			AddToken(tokens, m_overwatchDamageMod, "OverwatchDamage", string.Empty, soldierDashAndOverwatch.m_coneDamage);
			AddToken_EffectMod(tokens, m_overwatchHitEffectMod, "OverwatchHitEffect", soldierDashAndOverwatch.m_overwatchHitEffect);
			AddToken(tokens, m_nearDistThresholdMod, "NearDistThreshold", string.Empty, soldierDashAndOverwatch.m_nearDistThreshold);
			AddToken(tokens, m_extraDamageForNearTargetsMod, "ExtraDamageForNearTargets", string.Empty, soldierDashAndOverwatch.m_extraDamageForNearTargets);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierDashAndOverwatch soldierDashAndOverwatch = GetTargetAbilityOnAbilityData(abilityData) as SoldierDashAndOverwatch;
		bool isValid = soldierDashAndOverwatch != null;
		string desc = string.Empty;
		desc += PropDesc(m_onlyDashNextToCoverMod, "[OnlyDashNextToCover]", isValid, isValid && soldierDashAndOverwatch.m_onlyDashNextToCover);
		desc += PropDesc(m_coneInfoMod, "[ConeInfo]", isValid, isValid ? soldierDashAndOverwatch.m_coneInfo : null);
		desc += PropDesc(m_laserInfoMod, "[LaserInfo]", isValid, isValid ? soldierDashAndOverwatch.m_laserInfo : null);
		desc += PropDesc(m_selfHitEffectMod, "[SelfHitEffect]", isValid, isValid ? soldierDashAndOverwatch.m_selfHitEffect : null);
		desc += PropDesc(m_onCastAllyHitRadiusAroundDestMod, "[OnCastAllyHitRadiusAroundDest]", isValid, isValid ? soldierDashAndOverwatch.m_onCastAllyHitRadiusAroundDest : 0f);
		desc += PropDesc(m_onCastAllyHitEffectMod, "[OnCastAllyHitEffect]", isValid, isValid ? soldierDashAndOverwatch.m_onCastAllyHitEffect : null);
		desc += PropDesc(m_overwatchDamageMod, "[OverwatchDamage]", isValid, isValid ? soldierDashAndOverwatch.m_coneDamage : 0);
		desc += PropDesc(m_overwatchHitEffectMod, "[OverwatchHitEffect]", isValid, isValid ? soldierDashAndOverwatch.m_overwatchHitEffect : null);
		desc += PropDesc(m_nearDistThresholdMod, "[NearDistThreshold]", isValid, isValid ? soldierDashAndOverwatch.m_nearDistThreshold : 0f);
		return new StringBuilder().Append(desc).Append(PropDesc(m_extraDamageForNearTargetsMod, "[ExtraDamageForNearTargets]", isValid, isValid ? soldierDashAndOverwatch.m_extraDamageForNearTargets : 0)).ToString();
	}
}
