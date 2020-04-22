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
		if (!(mantaDashThroughWall != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_aoeConeWidthMod, "AoeConeWidth", string.Empty, mantaDashThroughWall.m_aoeConeWidth);
			AbilityMod.AddToken(tokens, m_aoeConeLengthMod, "AoeConeLength", string.Empty, mantaDashThroughWall.m_aoeConeLength);
			AbilityMod.AddToken(tokens, m_aoeThroughWallConeWidthMod, "AoeThroughWallConeWidth", string.Empty, mantaDashThroughWall.m_aoeThroughWallConeWidth);
			AbilityMod.AddToken(tokens, m_aoeThroughWallConeLengthMod, "AoeThroughWallConeLength", string.Empty, mantaDashThroughWall.m_aoeThroughWallConeLength);
			AbilityMod.AddToken(tokens, m_widthMod, "Width", string.Empty, mantaDashThroughWall.m_width);
			AbilityMod.AddToken(tokens, m_maxRangeMod, "MaxRange", string.Empty, mantaDashThroughWall.m_maxRange);
			AbilityMod.AddToken(tokens, m_maxWidthOfWallMod, "MaxWidthOfWall", string.Empty, mantaDashThroughWall.m_maxWidthOfWall);
			AbilityMod.AddToken(tokens, m_extraTotalDistanceIfThroughWallsMod, "ExtraTotalDistanceIfThroughWalls", string.Empty, mantaDashThroughWall.m_extraTotalDistanceIfThroughWalls);
			AbilityMod.AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, mantaDashThroughWall.m_coneBackwardOffset);
			AbilityMod.AddToken(tokens, m_directHitDamageMod, "DirectHitDamage", string.Empty, mantaDashThroughWall.m_directHitDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_directEnemyHitEffectMod, "DirectEnemyHitEffect", mantaDashThroughWall.m_directEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_aoeDamageMod, "AoeDamage", string.Empty, mantaDashThroughWall.m_aoeDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_aoeEnemyHitEffectMod, "AoeEnemyHitEffect", mantaDashThroughWall.m_aoeEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_aoeThroughWallsDamageMod, "AoeThroughWallsDamage", string.Empty, mantaDashThroughWall.m_aoeThroughWallsDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_aoeThroughWallsEffectMod, "AoeThroughWallsEffect", mantaDashThroughWall.m_aoeThroughWallsEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_additionalDirtyFightingExplosionEffect, "ExtraDirtyFightingExplosionEffect");
			m_cooldownReductionsWhenNoHits.AddTooltipTokens(tokens, "CooldownReductionOnMiss");
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MantaDashThroughWall mantaDashThroughWall = GetTargetAbilityOnAbilityData(abilityData) as MantaDashThroughWall;
		bool flag = mantaDashThroughWall != null;
		string empty = string.Empty;
		empty += PropDesc(m_aoeConeWidthMod, "[AoeConeWidth]", flag, (!flag) ? 0f : mantaDashThroughWall.m_aoeConeWidth);
		string str = empty;
		AbilityModPropertyFloat aoeConeLengthMod = m_aoeConeLengthMod;
		float baseVal;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = mantaDashThroughWall.m_aoeConeLength;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(aoeConeLengthMod, "[AoeConeLength]", flag, baseVal);
		empty += PropDesc(m_aoeThroughWallConeWidthMod, "[AoeThroughWallConeWidth]", flag, (!flag) ? 0f : mantaDashThroughWall.m_aoeThroughWallConeWidth);
		string str2 = empty;
		AbilityModPropertyFloat aoeThroughWallConeLengthMod = m_aoeThroughWallConeLengthMod;
		float baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = mantaDashThroughWall.m_aoeThroughWallConeLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(aoeThroughWallConeLengthMod, "[AoeThroughWallConeLength]", flag, baseVal2);
		empty += PropDesc(m_widthMod, "[Width]", flag, (!flag) ? 0f : mantaDashThroughWall.m_width);
		string str3 = empty;
		AbilityModPropertyFloat maxRangeMod = m_maxRangeMod;
		float baseVal3;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = mantaDashThroughWall.m_maxRange;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(maxRangeMod, "[MaxRange]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat maxWidthOfWallMod = m_maxWidthOfWallMod;
		float baseVal4;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = mantaDashThroughWall.m_maxWidthOfWall;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(maxWidthOfWallMod, "[MaxWidthOfWall]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyFloat extraTotalDistanceIfThroughWallsMod = m_extraTotalDistanceIfThroughWallsMod;
		float baseVal5;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = mantaDashThroughWall.m_extraTotalDistanceIfThroughWalls;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + PropDesc(extraTotalDistanceIfThroughWallsMod, "[ExtraTotalDistanceIfThroughWalls]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyBool clampConeToWallMod = m_clampConeToWallMod;
		int baseVal6;
		if (flag)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal6 = (mantaDashThroughWall.m_clampConeToWall ? 1 : 0);
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(clampConeToWallMod, "[ClampConeToWall]", flag, (byte)baseVal6 != 0);
		string str7 = empty;
		AbilityModPropertyBool aoeWithMissMod = m_aoeWithMissMod;
		int baseVal7;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal7 = (mantaDashThroughWall.m_aoeWithMiss ? 1 : 0);
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(aoeWithMissMod, "[AoeWithMiss]", flag, (byte)baseVal7 != 0);
		string str8 = empty;
		AbilityModPropertyFloat coneBackwardOffsetMod = m_coneBackwardOffsetMod;
		float baseVal8;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal8 = mantaDashThroughWall.m_coneBackwardOffset;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str8 + PropDesc(coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyInt directHitDamageMod = m_directHitDamageMod;
		int baseVal9;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal9 = mantaDashThroughWall.m_directHitDamage;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(directHitDamageMod, "[DirectHitDamage]", flag, baseVal9);
		empty += PropDesc(m_directEnemyHitEffectMod, "[DirectEnemyHitEffect]", flag, (!flag) ? null : mantaDashThroughWall.m_directEnemyHitEffect);
		string str10 = empty;
		AbilityModPropertyBool directHitIgnoreCoverMod = m_directHitIgnoreCoverMod;
		int baseVal10;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal10 = (mantaDashThroughWall.m_directHitIgnoreCover ? 1 : 0);
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(directHitIgnoreCoverMod, "[DirectHitIgnoreCover]", flag, (byte)baseVal10 != 0);
		string str11 = empty;
		AbilityModPropertyInt aoeDamageMod = m_aoeDamageMod;
		int baseVal11;
		if (flag)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal11 = mantaDashThroughWall.m_aoeDamage;
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str11 + PropDesc(aoeDamageMod, "[AoeDamage]", flag, baseVal11);
		empty += PropDesc(m_aoeEnemyHitEffectMod, "[AoeEnemyHitEffect]", flag, (!flag) ? null : mantaDashThroughWall.m_aoeEnemyHitEffect);
		empty += PropDesc(m_aoeThroughWallsDamageMod, "[AoeThroughWallsDamage]", flag, flag ? mantaDashThroughWall.m_aoeThroughWallsDamage : 0);
		empty += PropDesc(m_aoeThroughWallsEffectMod, "[AoeThroughWallsEffect]", flag, (!flag) ? null : mantaDashThroughWall.m_aoeThroughWallsEffect);
		empty += PropDesc(m_additionalDirtyFightingExplosionEffect, "[ExtraDirtyFightingExplosionEffect]", flag);
		if (m_cooldownReductionsWhenNoHits.HasCooldownReduction())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			empty += m_cooldownReductionsWhenNoHits.GetDescription(abilityData);
		}
		return empty;
	}
}
