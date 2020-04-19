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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ExoTetherTrap.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, exoTetherTrap.m_laserDamageAmount, true, false);
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "LaserInfo", exoTetherTrap.m_laserInfo, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_baseEffectDataMod, "BaseEffectData", exoTetherTrap.m_baseEffectData, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_laserOnHitEffectMod, "LaserOnHitEffect", exoTetherTrap.m_laserOnHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_tetherDistanceMod, "TetherDistance", string.Empty, exoTetherTrap.m_tetherDistance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_tetherBreakDamageMod, "TetherBreakDamage", string.Empty, exoTetherTrap.m_tetherBreakDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_tetherBreakEffectMod, "TetherBreakEffect", exoTetherTrap.m_tetherBreakEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraDamagePerMoveDistMod, "ExtraDamagePerMoveDist", string.Empty, exoTetherTrap.m_extraDamagePerMoveDist, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxExtraDamageFromMoveDistMod, "MaxExtraDamageFromMoveDist", string.Empty, exoTetherTrap.m_maxExtraDamageFromMoveDist, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrOnTetherEndIfNotTriggeredMod, "CdrOnTetherEndIfNotTriggered", string.Empty, exoTetherTrap.m_cdrOnTetherEndIfNotTriggered, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ExoTetherTrap exoTetherTrap = base.GetTargetAbilityOnAbilityData(abilityData) as ExoTetherTrap;
		bool flag = exoTetherTrap != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt laserDamageAmountMod = this.m_laserDamageAmountMod;
		string prefix = "[LaserDamageAmount]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ExoTetherTrap.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = exoTetherTrap.m_laserDamageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(laserDamageAmountMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyLaserInfo laserInfoMod = this.m_laserInfoMod;
		string prefix2 = "[LaserInfo]";
		bool showBaseVal2 = flag;
		LaserTargetingInfo baseLaserInfo;
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseLaserInfo = exoTetherTrap.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		text = str2 + base.PropDesc(laserInfoMod, prefix2, showBaseVal2, baseLaserInfo);
		string str3 = text;
		AbilityModPropertyEffectData baseEffectDataMod = this.m_baseEffectDataMod;
		string prefix3 = "[TetherBaseEffectData]";
		bool showBaseVal3 = flag;
		StandardActorEffectData baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = exoTetherTrap.m_baseEffectData;
		}
		else
		{
			baseVal2 = null;
		}
		text = str3 + base.PropDesc(baseEffectDataMod, prefix3, showBaseVal3, baseVal2);
		text += base.PropDesc(this.m_laserOnHitEffectMod, "[LaserOnHitEffect]", flag, (!flag) ? null : exoTetherTrap.m_laserOnHitEffect);
		string str4 = text;
		AbilityModPropertyFloat tetherDistanceMod = this.m_tetherDistanceMod;
		string prefix4 = "[TetherDistance]";
		bool showBaseVal4 = flag;
		float baseVal3;
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = exoTetherTrap.m_tetherDistance;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str4 + base.PropDesc(tetherDistanceMod, prefix4, showBaseVal4, baseVal3);
		string str5 = text;
		AbilityModPropertyInt tetherBreakDamageMod = this.m_tetherBreakDamageMod;
		string prefix5 = "[TetherBreakDamage]";
		bool showBaseVal5 = flag;
		int baseVal4;
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = exoTetherTrap.m_tetherBreakDamage;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str5 + base.PropDesc(tetherBreakDamageMod, prefix5, showBaseVal5, baseVal4);
		string str6 = text;
		AbilityModPropertyEffectInfo tetherBreakEffectMod = this.m_tetherBreakEffectMod;
		string prefix6 = "[TetherBreakEffect]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal5;
		if (flag)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = exoTetherTrap.m_tetherBreakEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str6 + base.PropDesc(tetherBreakEffectMod, prefix6, showBaseVal6, baseVal5);
		string str7 = text;
		AbilityModPropertyBool breakTetherOnNonGroundBasedMovementMod = this.m_breakTetherOnNonGroundBasedMovementMod;
		string prefix7 = "[BreakTetherOnNonGroundBasedMovement]";
		bool showBaseVal7 = flag;
		bool baseVal6;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal6 = exoTetherTrap.m_breakTetherOnNonGroundBasedMovement;
		}
		else
		{
			baseVal6 = false;
		}
		text = str7 + base.PropDesc(breakTetherOnNonGroundBasedMovementMod, prefix7, showBaseVal7, baseVal6);
		text += base.PropDesc(this.m_extraDamagePerMoveDistMod, "[ExtraDamagePerMoveDist]", flag, (!flag) ? 0f : exoTetherTrap.m_extraDamagePerMoveDist);
		string str8 = text;
		AbilityModPropertyInt maxExtraDamageFromMoveDistMod = this.m_maxExtraDamageFromMoveDistMod;
		string prefix8 = "[MaxExtraDamageFromMoveDist]";
		bool showBaseVal8 = flag;
		int baseVal7;
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal7 = exoTetherTrap.m_maxExtraDamageFromMoveDist;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str8 + base.PropDesc(maxExtraDamageFromMoveDistMod, prefix8, showBaseVal8, baseVal7);
		return text + base.PropDesc(this.m_cdrOnTetherEndIfNotTriggeredMod, "[CdrOnTetherEndIfNotTriggered]", flag, (!flag) ? 0 : exoTetherTrap.m_cdrOnTetherEndIfNotTriggered);
	}
}
