// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ExoTetherTrap : AbilityMod
{
	[Header("-- Targeting and Direct Damage")]
	public AbilityModPropertyInt m_laserDamageAmountMod;
	public AbilityModPropertyLaserInfo m_laserInfoMod;
	public AbilityModPropertyEffectData m_baseEffectDataMod;
	public AbilityModPropertyEffectInfo m_laserOnHitEffectMod;
	[Header("-- Tether Info")]
	public AbilityModPropertyFloat m_tetherDistanceMod;
	public AbilityModPropertyInt m_tetherBreakDamageMod;
	public AbilityModPropertyEffectInfo m_tetherBreakEffectMod;
	public AbilityModPropertyBool m_breakTetherOnNonGroundBasedMovementMod;
	[Header("-- Extra Damage based on distance")]
	public AbilityModPropertyFloat m_extraDamagePerMoveDistMod;
	public AbilityModPropertyInt m_maxExtraDamageFromMoveDistMod;
	[Header("-- Cooldown Reduction if tether didn't break")]
	public AbilityModPropertyInt m_cdrOnTetherEndIfNotTriggeredMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ExoTetherTrap);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ExoTetherTrap exoTetherTrap = targetAbility as ExoTetherTrap;
		if (exoTetherTrap != null)
		{
			AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, exoTetherTrap.m_laserDamageAmount);
			AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", exoTetherTrap.m_laserInfo);
			AddToken_EffectMod(tokens, m_baseEffectDataMod, "BaseEffectData", exoTetherTrap.m_baseEffectData);
			AddToken_EffectMod(tokens, m_laserOnHitEffectMod, "LaserOnHitEffect", exoTetherTrap.m_laserOnHitEffect);
			AddToken(tokens, m_tetherDistanceMod, "TetherDistance", string.Empty, exoTetherTrap.m_tetherDistance);
			AddToken(tokens, m_tetherBreakDamageMod, "TetherBreakDamage", string.Empty, exoTetherTrap.m_tetherBreakDamage);
			AddToken_EffectMod(tokens, m_tetherBreakEffectMod, "TetherBreakEffect", exoTetherTrap.m_tetherBreakEffect);
			AddToken(tokens, m_extraDamagePerMoveDistMod, "ExtraDamagePerMoveDist", string.Empty, exoTetherTrap.m_extraDamagePerMoveDist);
			AddToken(tokens, m_maxExtraDamageFromMoveDistMod, "MaxExtraDamageFromMoveDist", string.Empty, exoTetherTrap.m_maxExtraDamageFromMoveDist);
			AddToken(tokens, m_cdrOnTetherEndIfNotTriggeredMod, "CdrOnTetherEndIfNotTriggered", string.Empty, exoTetherTrap.m_cdrOnTetherEndIfNotTriggered);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		ExoTetherTrap exoTetherTrap = GetTargetAbilityOnAbilityData(abilityData) as ExoTetherTrap;
		// rogues
		// ExoTetherTrap exoTetherTrap = targetAbility as ExoTetherTrap;
		
		bool isValid = exoTetherTrap != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserDamageAmountMod, "[LaserDamageAmount]", isValid, isValid ? exoTetherTrap.m_laserDamageAmount : 0);
		desc += PropDesc(m_laserInfoMod, "[LaserInfo]", isValid, isValid ? exoTetherTrap.m_laserInfo : null);
		desc += PropDesc(m_baseEffectDataMod, "[TetherBaseEffectData]", isValid, isValid ? exoTetherTrap.m_baseEffectData : null);
		desc += PropDesc(m_laserOnHitEffectMod, "[LaserOnHitEffect]", isValid, isValid ? exoTetherTrap.m_laserOnHitEffect : null);
		desc += PropDesc(m_tetherDistanceMod, "[TetherDistance]", isValid, isValid ? exoTetherTrap.m_tetherDistance : 0f);
		desc += PropDesc(m_tetherBreakDamageMod, "[TetherBreakDamage]", isValid, isValid ? exoTetherTrap.m_tetherBreakDamage : 0);
		desc += PropDesc(m_tetherBreakEffectMod, "[TetherBreakEffect]", isValid, isValid ? exoTetherTrap.m_tetherBreakEffect : null);
		desc += PropDesc(m_breakTetherOnNonGroundBasedMovementMod, "[BreakTetherOnNonGroundBasedMovement]", isValid, isValid && exoTetherTrap.m_breakTetherOnNonGroundBasedMovement);
		desc += PropDesc(m_extraDamagePerMoveDistMod, "[ExtraDamagePerMoveDist]", isValid, isValid ? exoTetherTrap.m_extraDamagePerMoveDist : 0f);
		desc += PropDesc(m_maxExtraDamageFromMoveDistMod, "[MaxExtraDamageFromMoveDist]", isValid, isValid ? exoTetherTrap.m_maxExtraDamageFromMoveDist : 0);
		return desc + PropDesc(m_cdrOnTetherEndIfNotTriggeredMod, "[CdrOnTetherEndIfNotTriggered]", isValid, isValid ? exoTetherTrap.m_cdrOnTetherEndIfNotTriggered : 0);
	}
}
