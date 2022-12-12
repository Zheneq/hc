// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SpaceMarineHandCannon : AbilityMod
{
	[Header("-- Laser Damage and Size Mod")]
	public AbilityModPropertyInt m_laserDamageMod;
	public AbilityModPropertyInt m_coneDamageMod;
	public AbilityModPropertyFloat m_laserLengthMod;
	public AbilityModPropertyFloat m_laserWidthMod;
	[Header("-- Explosion Mod")]
	public AbilityModPropertyBool m_shouldExplodeMod;
	public AbilityModPropertyFloat m_coneAngleMod;
	public AbilityModPropertyFloat m_coneLengthMod;
	[Header("-- On Hit Effect Overrides")]
	public bool m_useLaserHitEffectOverride;
	public StandardEffectInfo m_laserHitEffectOverride;
	public bool m_useConeHitEffectOverride;
	public StandardEffectInfo m_coneHitEffectOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(SpaceMarineHandCannon);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SpaceMarineHandCannon spaceMarineHandCannon = targetAbility as SpaceMarineHandCannon;
		if (spaceMarineHandCannon != null)
		{
			AddToken(tokens, m_laserDamageMod, "PrimaryDamage", string.Empty, spaceMarineHandCannon.m_primaryDamage);
			AddToken(tokens, m_laserWidthMod, "PrimaryWidth", string.Empty, spaceMarineHandCannon.m_primaryWidth);
			AddToken(tokens, m_laserLengthMod, "PrimaryLength", string.Empty, spaceMarineHandCannon.m_primaryLength);
			AddToken(tokens, m_coneDamageMod, "ConeDamage", string.Empty, spaceMarineHandCannon.m_coneDamage);
			AddToken(tokens, m_coneAngleMod, "ConeWidthAngle", string.Empty, spaceMarineHandCannon.m_coneWidthAngle);
			AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, spaceMarineHandCannon.m_coneLength);
			if (m_useLaserHitEffectOverride)
			{
				AddToken_EffectInfo(tokens, m_laserHitEffectOverride, "EffectOnLaserTarget", spaceMarineHandCannon.m_effectInfoOnPrimaryTarget);
			}
			if (m_useConeHitEffectOverride)
			{
				AddToken_EffectInfo(tokens, m_coneHitEffectOverride, "EffectInfoOnConeTargets", spaceMarineHandCannon.m_effectInfoOnConeTargets);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		SpaceMarineHandCannon spaceMarineHandCannon = GetTargetAbilityOnAbilityData(abilityData) as SpaceMarineHandCannon;
		// rogues
		// SpaceMarineHandCannon spaceMarineHandCannon = targetAbility as SpaceMarineHandCannon;
		bool isValid = spaceMarineHandCannon != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_laserDamageMod, "[Laser Damage]", isValid, isValid ? spaceMarineHandCannon.m_primaryDamage : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_coneDamageMod, "[Cone Damage]", isValid, isValid ? spaceMarineHandCannon.m_coneDamage : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserLengthMod, "[Laser Length]", isValid, isValid ? spaceMarineHandCannon.m_primaryLength : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserWidthMod, "[Laser Width]", isValid, isValid ? spaceMarineHandCannon.m_primaryWidth : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_shouldExplodeMod, "[Should Explode?]", isValid, true);
		desc += AbilityModHelper.GetModPropertyDesc(m_coneAngleMod, "[Cone Angle]", isValid, isValid ? spaceMarineHandCannon.m_coneWidthAngle : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_coneLengthMod, "[Cone Length]", isValid, isValid ? spaceMarineHandCannon.m_coneLength : 0f);
		if (m_useLaserHitEffectOverride)
		{
			desc += AbilityModHelper.GetModEffectInfoDesc(m_laserHitEffectOverride, "{ Laser Hit Effect Override }", string.Empty, isValid, isValid ? spaceMarineHandCannon.m_effectInfoOnPrimaryTarget : null);
		}
		if (m_useConeHitEffectOverride)
		{
			desc += AbilityModHelper.GetModEffectInfoDesc(m_coneHitEffectOverride, "{ Cone Hit Effect Override }", string.Empty, isValid, isValid ? spaceMarineHandCannon.m_effectInfoOnConeTargets : null);
		}
		return desc;
	}
}
