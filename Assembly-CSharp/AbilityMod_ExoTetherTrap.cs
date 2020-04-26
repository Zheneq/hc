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
		if (!(exoTetherTrap != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, exoTetherTrap.m_laserDamageAmount);
			AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", exoTetherTrap.m_laserInfo);
			AbilityMod.AddToken_EffectMod(tokens, m_baseEffectDataMod, "BaseEffectData", exoTetherTrap.m_baseEffectData);
			AbilityMod.AddToken_EffectMod(tokens, m_laserOnHitEffectMod, "LaserOnHitEffect", exoTetherTrap.m_laserOnHitEffect);
			AbilityMod.AddToken(tokens, m_tetherDistanceMod, "TetherDistance", string.Empty, exoTetherTrap.m_tetherDistance);
			AbilityMod.AddToken(tokens, m_tetherBreakDamageMod, "TetherBreakDamage", string.Empty, exoTetherTrap.m_tetherBreakDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_tetherBreakEffectMod, "TetherBreakEffect", exoTetherTrap.m_tetherBreakEffect);
			AbilityMod.AddToken(tokens, m_extraDamagePerMoveDistMod, "ExtraDamagePerMoveDist", string.Empty, exoTetherTrap.m_extraDamagePerMoveDist);
			AbilityMod.AddToken(tokens, m_maxExtraDamageFromMoveDistMod, "MaxExtraDamageFromMoveDist", string.Empty, exoTetherTrap.m_maxExtraDamageFromMoveDist);
			AbilityMod.AddToken(tokens, m_cdrOnTetherEndIfNotTriggeredMod, "CdrOnTetherEndIfNotTriggered", string.Empty, exoTetherTrap.m_cdrOnTetherEndIfNotTriggered);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ExoTetherTrap exoTetherTrap = GetTargetAbilityOnAbilityData(abilityData) as ExoTetherTrap;
		bool flag = exoTetherTrap != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt laserDamageAmountMod = m_laserDamageAmountMod;
		int baseVal;
		if (flag)
		{
			baseVal = exoTetherTrap.m_laserDamageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(laserDamageAmountMod, "[LaserDamageAmount]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyLaserInfo laserInfoMod = m_laserInfoMod;
		object baseLaserInfo;
		if (flag)
		{
			baseLaserInfo = exoTetherTrap.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		empty = str2 + PropDesc(laserInfoMod, "[LaserInfo]", flag, (LaserTargetingInfo)baseLaserInfo);
		string str3 = empty;
		AbilityModPropertyEffectData baseEffectDataMod = m_baseEffectDataMod;
		object baseVal2;
		if (flag)
		{
			baseVal2 = exoTetherTrap.m_baseEffectData;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str3 + PropDesc(baseEffectDataMod, "[TetherBaseEffectData]", flag, (StandardActorEffectData)baseVal2);
		empty += PropDesc(m_laserOnHitEffectMod, "[LaserOnHitEffect]", flag, (!flag) ? null : exoTetherTrap.m_laserOnHitEffect);
		string str4 = empty;
		AbilityModPropertyFloat tetherDistanceMod = m_tetherDistanceMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = exoTetherTrap.m_tetherDistance;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str4 + PropDesc(tetherDistanceMod, "[TetherDistance]", flag, baseVal3);
		string str5 = empty;
		AbilityModPropertyInt tetherBreakDamageMod = m_tetherBreakDamageMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = exoTetherTrap.m_tetherBreakDamage;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str5 + PropDesc(tetherBreakDamageMod, "[TetherBreakDamage]", flag, baseVal4);
		string str6 = empty;
		AbilityModPropertyEffectInfo tetherBreakEffectMod = m_tetherBreakEffectMod;
		object baseVal5;
		if (flag)
		{
			baseVal5 = exoTetherTrap.m_tetherBreakEffect;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str6 + PropDesc(tetherBreakEffectMod, "[TetherBreakEffect]", flag, (StandardEffectInfo)baseVal5);
		string str7 = empty;
		AbilityModPropertyBool breakTetherOnNonGroundBasedMovementMod = m_breakTetherOnNonGroundBasedMovementMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = (exoTetherTrap.m_breakTetherOnNonGroundBasedMovement ? 1 : 0);
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str7 + PropDesc(breakTetherOnNonGroundBasedMovementMod, "[BreakTetherOnNonGroundBasedMovement]", flag, (byte)baseVal6 != 0);
		empty += PropDesc(m_extraDamagePerMoveDistMod, "[ExtraDamagePerMoveDist]", flag, (!flag) ? 0f : exoTetherTrap.m_extraDamagePerMoveDist);
		string str8 = empty;
		AbilityModPropertyInt maxExtraDamageFromMoveDistMod = m_maxExtraDamageFromMoveDistMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = exoTetherTrap.m_maxExtraDamageFromMoveDist;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str8 + PropDesc(maxExtraDamageFromMoveDistMod, "[MaxExtraDamageFromMoveDist]", flag, baseVal7);
		return empty + PropDesc(m_cdrOnTetherEndIfNotTriggeredMod, "[CdrOnTetherEndIfNotTriggered]", flag, flag ? exoTetherTrap.m_cdrOnTetherEndIfNotTriggered : 0);
	}
}
