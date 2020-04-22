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
		if (!(spaceMarineHandCannon != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_laserDamageMod, "PrimaryDamage", string.Empty, spaceMarineHandCannon.m_primaryDamage);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "PrimaryWidth", string.Empty, spaceMarineHandCannon.m_primaryWidth);
			AbilityMod.AddToken(tokens, m_laserLengthMod, "PrimaryLength", string.Empty, spaceMarineHandCannon.m_primaryLength);
			AbilityMod.AddToken(tokens, m_coneDamageMod, "ConeDamage", string.Empty, spaceMarineHandCannon.m_coneDamage);
			AbilityMod.AddToken(tokens, m_coneAngleMod, "ConeWidthAngle", string.Empty, spaceMarineHandCannon.m_coneWidthAngle);
			AbilityMod.AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, spaceMarineHandCannon.m_coneLength);
			if (m_useLaserHitEffectOverride)
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
				AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffectOverride, "EffectOnLaserTarget", spaceMarineHandCannon.m_effectInfoOnPrimaryTarget);
			}
			if (m_useConeHitEffectOverride)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					AbilityMod.AddToken_EffectInfo(tokens, m_coneHitEffectOverride, "EffectInfoOnConeTargets", spaceMarineHandCannon.m_effectInfoOnConeTargets);
					return;
				}
			}
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SpaceMarineHandCannon spaceMarineHandCannon = GetTargetAbilityOnAbilityData(abilityData) as SpaceMarineHandCannon;
		bool flag = spaceMarineHandCannon != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt laserDamageMod = m_laserDamageMod;
		int baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = spaceMarineHandCannon.m_primaryDamage;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(laserDamageMod, "[Laser Damage]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt coneDamageMod = m_coneDamageMod;
		int baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = spaceMarineHandCannon.m_coneDamage;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(coneDamageMod, "[Cone Damage]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat laserLengthMod = m_laserLengthMod;
		float baseVal3;
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
			baseVal3 = spaceMarineHandCannon.m_primaryLength;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(laserLengthMod, "[Laser Length]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
		float baseVal4;
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
			baseVal4 = spaceMarineHandCannon.m_primaryWidth;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(laserWidthMod, "[Laser Width]", flag, baseVal4);
		empty += AbilityModHelper.GetModPropertyDesc(m_shouldExplodeMod, "[Should Explode?]", flag, true);
		string str5 = empty;
		AbilityModPropertyFloat coneAngleMod = m_coneAngleMod;
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
			baseVal5 = spaceMarineHandCannon.m_coneWidthAngle;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(coneAngleMod, "[Cone Angle]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyFloat coneLengthMod = m_coneLengthMod;
		float baseVal6;
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
			baseVal6 = spaceMarineHandCannon.m_coneLength;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + AbilityModHelper.GetModPropertyDesc(coneLengthMod, "[Cone Length]", flag, baseVal6);
		if (m_useLaserHitEffectOverride)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			string str7 = empty;
			StandardEffectInfo laserHitEffectOverride = m_laserHitEffectOverride;
			string empty2 = string.Empty;
			object baseVal7;
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
				baseVal7 = spaceMarineHandCannon.m_effectInfoOnPrimaryTarget;
			}
			else
			{
				baseVal7 = null;
			}
			empty = str7 + AbilityModHelper.GetModEffectInfoDesc(laserHitEffectOverride, "{ Laser Hit Effect Override }", empty2, flag, (StandardEffectInfo)baseVal7);
		}
		if (m_useConeHitEffectOverride)
		{
			string str8 = empty;
			StandardEffectInfo coneHitEffectOverride = m_coneHitEffectOverride;
			string empty3 = string.Empty;
			object baseVal8;
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
				baseVal8 = spaceMarineHandCannon.m_effectInfoOnConeTargets;
			}
			else
			{
				baseVal8 = null;
			}
			empty = str8 + AbilityModHelper.GetModEffectInfoDesc(coneHitEffectOverride, "{ Cone Hit Effect Override }", empty3, flag, (StandardEffectInfo)baseVal8);
		}
		return empty;
	}
}
