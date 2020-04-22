using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BazookaGirlExplodingLaser : AbilityMod
{
	[Header("-- Targeting: If using Cone")]
	public AbilityModPropertyFloat m_coneWidthAngleMod;

	public AbilityModPropertyFloat m_coneLengthMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	[Header("-- Laser Params")]
	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyBool m_laserPenetrateLosMod;

	[Header("-- Laser Hit Mods")]
	public AbilityModPropertyInt m_laserDamageMod;

	public AbilityModPropertyBool m_laserIgnoreCoverMod;

	public AbilityModPropertyEffectInfo m_laserHitEffectOverride;

	public AbilityModPropertyInt m_cdrOnDirectHitMod;

	[Header("-- Explosion Hit Mods")]
	public AbilityModPropertyInt m_explosionDamageMod;

	public AbilityModPropertyBool m_explosionIgnoreLosMod;

	public AbilityModPropertyBool m_explosionIgnoreCoverMod;

	public AbilityModPropertyEffectInfo m_explosionEffectOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(BazookaGirlExplodingLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BazookaGirlExplodingLaser bazookaGirlExplodingLaser = targetAbility as BazookaGirlExplodingLaser;
		if (!(bazookaGirlExplodingLaser != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, bazookaGirlExplodingLaser.m_coneWidthAngle);
			AbilityMod.AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, bazookaGirlExplodingLaser.m_coneLength);
			AbilityMod.AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, bazookaGirlExplodingLaser.m_coneBackwardOffset);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, bazookaGirlExplodingLaser.m_laserWidth);
			AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, bazookaGirlExplodingLaser.m_laserRange);
			AbilityMod.AddToken(tokens, m_laserDamageMod, "LaserDamageAmount", string.Empty, bazookaGirlExplodingLaser.m_laserDamageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_laserHitEffectOverride, "EffectOnLaserHitTargets", bazookaGirlExplodingLaser.m_effectOnLaserHitTargets);
			AbilityMod.AddToken(tokens, m_explosionDamageMod, "ExplosionDamageAmount", string.Empty, bazookaGirlExplodingLaser.m_explosionDamageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_explosionEffectOverride, "EffectOnExplosionHitTargets", bazookaGirlExplodingLaser.m_effectOnExplosionHitTargets);
			AbilityMod.AddToken(tokens, m_cdrOnDirectHitMod, "CdrOnDirectHit", string.Empty, bazookaGirlExplodingLaser.m_cdrOnDirectHit);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlExplodingLaser bazookaGirlExplodingLaser = GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlExplodingLaser;
		bool flag = bazookaGirlExplodingLaser != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat coneWidthAngleMod = m_coneWidthAngleMod;
		float baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = bazookaGirlExplodingLaser.m_coneWidthAngle;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(coneWidthAngleMod, "[ConeWidthAngle]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat coneLengthMod = m_coneLengthMod;
		float baseVal2;
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
			baseVal2 = bazookaGirlExplodingLaser.m_coneLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(coneLengthMod, "[ConeLength]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat coneBackwardOffsetMod = m_coneBackwardOffsetMod;
		float baseVal3;
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
			baseVal3 = bazookaGirlExplodingLaser.m_coneBackwardOffset;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
		float baseVal4;
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
			baseVal4 = bazookaGirlExplodingLaser.m_laserWidth;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(laserWidthMod, "[LaserWidth]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyFloat laserRangeMod = m_laserRangeMod;
		float baseVal5;
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
			baseVal5 = bazookaGirlExplodingLaser.m_laserRange;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + PropDesc(laserRangeMod, "[LaserRange]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyBool laserPenetrateLosMod = m_laserPenetrateLosMod;
		int baseVal6;
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
			baseVal6 = (bazookaGirlExplodingLaser.m_laserPenetrateLos ? 1 : 0);
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(laserPenetrateLosMod, "[LaserPenetrateLos]", flag, (byte)baseVal6 != 0);
		string str7 = empty;
		AbilityModPropertyInt laserDamageMod = m_laserDamageMod;
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
			baseVal7 = bazookaGirlExplodingLaser.m_laserDamageAmount;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + AbilityModHelper.GetModPropertyDesc(laserDamageMod, "[Laser Damage]", flag, baseVal7);
		empty += AbilityModHelper.GetModPropertyDesc(m_laserIgnoreCoverMod, "[Laser Ignore Cover?]", flag, flag && bazookaGirlExplodingLaser.m_laserIgnoreCover);
		string str8 = empty;
		AbilityModPropertyEffectInfo laserHitEffectOverride = m_laserHitEffectOverride;
		object baseVal8;
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
			baseVal8 = bazookaGirlExplodingLaser.m_effectOnLaserHitTargets;
		}
		else
		{
			baseVal8 = null;
		}
		empty = str8 + AbilityModHelper.GetModPropertyDesc(laserHitEffectOverride, "{ Laser Enemy Hit Effect Override }", flag, (StandardEffectInfo)baseVal8);
		string str9 = empty;
		AbilityModPropertyInt cdrOnDirectHitMod = m_cdrOnDirectHitMod;
		int baseVal9;
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
			baseVal9 = bazookaGirlExplodingLaser.m_cdrOnDirectHit;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(cdrOnDirectHitMod, "[CdrOnDirectHit]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyInt explosionDamageMod = m_explosionDamageMod;
		int baseVal10;
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
			baseVal10 = bazookaGirlExplodingLaser.m_explosionDamageAmount;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + AbilityModHelper.GetModPropertyDesc(explosionDamageMod, "[Explosion Damage]", flag, baseVal10);
		empty += AbilityModHelper.GetModPropertyDesc(m_explosionIgnoreLosMod, "[Explosion Ignore LoS?]", flag, flag && bazookaGirlExplodingLaser.m_explosionPenetrateLos);
		empty += AbilityModHelper.GetModPropertyDesc(m_explosionIgnoreCoverMod, "[Explosion Ignore Cover?]", flag, flag && bazookaGirlExplodingLaser.m_explosionIgnoreCover);
		string str11 = empty;
		AbilityModPropertyEffectInfo explosionEffectOverride = m_explosionEffectOverride;
		object baseVal11;
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
			baseVal11 = bazookaGirlExplodingLaser.m_effectOnExplosionHitTargets;
		}
		else
		{
			baseVal11 = null;
		}
		return str11 + AbilityModHelper.GetModPropertyDesc(explosionEffectOverride, "{ Explosion Enemy Hit Effect Override }", flag, (StandardEffectInfo)baseVal11);
	}
}
