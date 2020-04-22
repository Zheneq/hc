using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SoldierConeOrLaser : AbilityMod
{
	[Header("  Targeting: For Cone")]
	public AbilityModPropertyConeInfo m_coneInfoMod;

	[Header("  Targeting: For Laser")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;

	[Header("-- On Hit Stuff --")]
	public AbilityModPropertyInt m_coneDamageMod;

	public AbilityModPropertyEffectInfo m_coneEnemyHitEffectMod;

	[Space(10f)]
	public AbilityModPropertyInt m_laserDamageMod;

	public AbilityModPropertyEffectInfo m_laserEnemyHitEffectMod;

	[Header("-- Extra Damage --")]
	public AbilityModPropertyInt m_extraDamageForAlternatingMod;

	public AbilityModPropertyFloat m_closeDistThresholdMod;

	public AbilityModPropertyInt m_extraDamageForNearTargetMod;

	public AbilityModPropertyInt m_extraDamageForFromCoverMod;

	public AbilityModPropertyInt m_extraDamageToEvadersMod;

	[Header("-- Extra Energy --")]
	public AbilityModPropertyInt m_extraEnergyForConeMod;

	public AbilityModPropertyInt m_extraEnergyForLaserMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SoldierConeOrLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SoldierConeOrLaser soldierConeOrLaser = targetAbility as SoldierConeOrLaser;
		if (!(soldierConeOrLaser != null))
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
			AbilityMod.AddToken_ConeInfo(tokens, m_coneInfoMod, "ConeInfo", soldierConeOrLaser.m_coneInfo);
			AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", soldierConeOrLaser.m_laserInfo);
			AbilityMod.AddToken(tokens, m_coneDamageMod, "ConeDamage", string.Empty, soldierConeOrLaser.m_coneDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_coneEnemyHitEffectMod, "ConeEnemyHitEffect", soldierConeOrLaser.m_coneEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_laserDamageMod, "LaserDamage", string.Empty, soldierConeOrLaser.m_laserDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_laserEnemyHitEffectMod, "LaserEnemyHitEffect", soldierConeOrLaser.m_laserEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_extraDamageForAlternatingMod, "ExtraDamageForAlternating", string.Empty, soldierConeOrLaser.m_extraDamageForAlternating);
			AbilityMod.AddToken(tokens, m_closeDistThresholdMod, "CloseDistThreshold", string.Empty, soldierConeOrLaser.m_closeDistThreshold);
			AbilityMod.AddToken(tokens, m_extraDamageForNearTargetMod, "ExtraDamageForNearTarget", string.Empty, soldierConeOrLaser.m_extraDamageForNearTarget);
			AbilityMod.AddToken(tokens, m_extraDamageForFromCoverMod, "ExtraDamageForFromCover", string.Empty, soldierConeOrLaser.m_extraDamageForFromCover);
			AbilityMod.AddToken(tokens, m_extraDamageToEvadersMod, "ExtraDamageToEvaders", string.Empty, soldierConeOrLaser.m_extraDamageToEvaders);
			AbilityMod.AddToken(tokens, m_extraEnergyForConeMod, "ExtraEnergyForCone", string.Empty, soldierConeOrLaser.m_extraEnergyForCone, true, true);
			AbilityMod.AddToken(tokens, m_extraEnergyForLaserMod, "ExtraEnergyForLaser", string.Empty, soldierConeOrLaser.m_extraEnergyForLaser, true, true);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierConeOrLaser soldierConeOrLaser = GetTargetAbilityOnAbilityData(abilityData) as SoldierConeOrLaser;
		bool flag = soldierConeOrLaser != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyConeInfo coneInfoMod = m_coneInfoMod;
		object baseConeInfo;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseConeInfo = soldierConeOrLaser.m_coneInfo;
		}
		else
		{
			baseConeInfo = null;
		}
		empty = str + PropDesc(coneInfoMod, "[ConeInfo]", flag, (ConeTargetingInfo)baseConeInfo);
		empty += PropDesc(m_laserInfoMod, "[LaserInfo]", flag, (!flag) ? null : soldierConeOrLaser.m_laserInfo);
		string str2 = empty;
		AbilityModPropertyInt coneDamageMod = m_coneDamageMod;
		int baseVal;
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
			baseVal = soldierConeOrLaser.m_coneDamage;
		}
		else
		{
			baseVal = 0;
		}
		empty = str2 + PropDesc(coneDamageMod, "[ConeDamage]", flag, baseVal);
		string str3 = empty;
		AbilityModPropertyEffectInfo coneEnemyHitEffectMod = m_coneEnemyHitEffectMod;
		object baseVal2;
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
			baseVal2 = soldierConeOrLaser.m_coneEnemyHitEffect;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str3 + PropDesc(coneEnemyHitEffectMod, "[ConeEnemyHitEffect]", flag, (StandardEffectInfo)baseVal2);
		string str4 = empty;
		AbilityModPropertyInt laserDamageMod = m_laserDamageMod;
		int baseVal3;
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
			baseVal3 = soldierConeOrLaser.m_laserDamage;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str4 + PropDesc(laserDamageMod, "[LaserDamage]", flag, baseVal3);
		empty += PropDesc(m_laserEnemyHitEffectMod, "[LaserEnemyHitEffect]", flag, (!flag) ? null : soldierConeOrLaser.m_laserEnemyHitEffect);
		empty += PropDesc(m_extraDamageForAlternatingMod, "[ExtraDamageForAlternating]", flag, flag ? soldierConeOrLaser.m_extraDamageForAlternating : 0);
		empty += PropDesc(m_closeDistThresholdMod, "[CloseDistThreshold]", flag, (!flag) ? 0f : soldierConeOrLaser.m_closeDistThreshold);
		string str5 = empty;
		AbilityModPropertyInt extraDamageForNearTargetMod = m_extraDamageForNearTargetMod;
		int baseVal4;
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
			baseVal4 = soldierConeOrLaser.m_extraDamageForNearTarget;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str5 + PropDesc(extraDamageForNearTargetMod, "[ExtraDamageForNearTarget]", flag, baseVal4);
		string str6 = empty;
		AbilityModPropertyInt extraDamageForFromCoverMod = m_extraDamageForFromCoverMod;
		int baseVal5;
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
			baseVal5 = soldierConeOrLaser.m_extraDamageForFromCover;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str6 + PropDesc(extraDamageForFromCoverMod, "[ExtraDamageForFromCover]", flag, baseVal5);
		empty += PropDesc(m_extraDamageToEvadersMod, "[ExtraDamageToEvaders]", flag, flag ? soldierConeOrLaser.m_extraDamageToEvaders : 0);
		empty += PropDesc(m_extraEnergyForConeMod, "[ExtraEnergyForCone]", flag, flag ? soldierConeOrLaser.m_extraEnergyForCone : 0);
		string str7 = empty;
		AbilityModPropertyInt extraEnergyForLaserMod = m_extraEnergyForLaserMod;
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
			baseVal6 = soldierConeOrLaser.m_extraEnergyForLaser;
		}
		else
		{
			baseVal6 = 0;
		}
		return str7 + PropDesc(extraEnergyForLaserMod, "[ExtraEnergyForLaser]", flag, baseVal6);
	}
}
