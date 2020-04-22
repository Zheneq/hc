using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_MartyrBasicAttack : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;

	public AbilityModPropertyEffectInfo m_laserHitEffectMod;

	public AbilityModPropertyFloat m_explosionRadiusMod;

	[Header("-- Damage & Crystal Bonuses")]
	public AbilityModPropertyInt m_baseLaserDamageMod;

	public AbilityModPropertyInt m_baseExplosionDamageMod;

	public AbilityModPropertyInt m_additionalDamagePerCrystalSpentMod;

	public AbilityModPropertyFloat m_additionalRadiusPerCrystalSpentMod;

	public AbilityModPropertyInt m_extraDamageIfSingleHitMod;

	[Header("-- Inner Ring Radius and Damage")]
	public AbilityModPropertyFloat m_innerRingRadiusMod;

	public AbilityModPropertyFloat m_innerRingExtraRadiusPerCrystalMod;

	[Space(5f)]
	public AbilityModPropertyInt m_innerRingDamageMod;

	public AbilityModPropertyInt m_innerRingDamagePerCrystalMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(MartyrBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MartyrBasicAttack martyrBasicAttack = targetAbility as MartyrBasicAttack;
		if (martyrBasicAttack != null)
		{
			AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", martyrBasicAttack.m_laserInfo);
			AbilityMod.AddToken_EffectMod(tokens, m_laserHitEffectMod, "LaserHitEffect", martyrBasicAttack.m_laserHitEffect);
			AbilityMod.AddToken(tokens, m_explosionRadiusMod, "ExplosionRadius", string.Empty, martyrBasicAttack.m_explosionRadius);
			AbilityMod.AddToken(tokens, m_baseLaserDamageMod, "BaseLaserDamage", string.Empty, martyrBasicAttack.m_baseLaserDamage);
			AbilityMod.AddToken(tokens, m_baseExplosionDamageMod, "BaseExplosionDamage", string.Empty, martyrBasicAttack.m_baseExplosionDamage);
			AbilityMod.AddToken(tokens, m_additionalDamagePerCrystalSpentMod, "AdditionalDamagePerCrystalSpent", string.Empty, martyrBasicAttack.m_additionalDamagePerCrystalSpent);
			AbilityMod.AddToken(tokens, m_additionalRadiusPerCrystalSpentMod, "AdditionalRadiusPerCrystalSpent", string.Empty, martyrBasicAttack.m_additionalRadiusPerCrystalSpent, true, true, true);
			AbilityMod.AddToken(tokens, m_extraDamageIfSingleHitMod, "ExtraDamageIfSingleHit", string.Empty, martyrBasicAttack.m_extraDamageIfSingleHit);
			AbilityMod.AddToken(tokens, m_innerRingRadiusMod, "InnerRingRadius", string.Empty, martyrBasicAttack.m_innerRingRadius);
			AbilityMod.AddToken(tokens, m_innerRingExtraRadiusPerCrystalMod, "InnerRingExtraRadiusPerCrystal", string.Empty, martyrBasicAttack.m_innerRingExtraRadiusPerCrystal);
			AbilityMod.AddToken(tokens, m_innerRingDamageMod, "InnerRingDamage", string.Empty, martyrBasicAttack.m_innerRingDamage);
			AbilityMod.AddToken(tokens, m_innerRingDamagePerCrystalMod, "InnerRingDamagePerCrystal", string.Empty, martyrBasicAttack.m_innerRingDamagePerCrystal);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrBasicAttack martyrBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as MartyrBasicAttack;
		bool flag = martyrBasicAttack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyLaserInfo laserInfoMod = m_laserInfoMod;
		object baseLaserInfo;
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
			baseLaserInfo = martyrBasicAttack.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		empty = str + PropDesc(laserInfoMod, "[LaserInfo]", flag, (LaserTargetingInfo)baseLaserInfo);
		empty += PropDesc(m_laserHitEffectMod, "[LaserHitEffect]", flag, (!flag) ? null : martyrBasicAttack.m_laserHitEffect);
		empty += PropDesc(m_explosionRadiusMod, "[ExplosionRadius]", flag, (!flag) ? 0f : martyrBasicAttack.m_explosionRadius);
		string str2 = empty;
		AbilityModPropertyInt baseLaserDamageMod = m_baseLaserDamageMod;
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
			baseVal = martyrBasicAttack.m_baseLaserDamage;
		}
		else
		{
			baseVal = 0;
		}
		empty = str2 + PropDesc(baseLaserDamageMod, "[BaseLaserDamage]", flag, baseVal);
		string str3 = empty;
		AbilityModPropertyInt baseExplosionDamageMod = m_baseExplosionDamageMod;
		int baseVal2;
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
			baseVal2 = martyrBasicAttack.m_baseExplosionDamage;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str3 + PropDesc(baseExplosionDamageMod, "[BaseExplosionDamage]", flag, baseVal2);
		string str4 = empty;
		AbilityModPropertyInt additionalDamagePerCrystalSpentMod = m_additionalDamagePerCrystalSpentMod;
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
			baseVal3 = martyrBasicAttack.m_additionalDamagePerCrystalSpent;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str4 + PropDesc(additionalDamagePerCrystalSpentMod, "[AdditionalDamagePerCrystalSpent]", flag, baseVal3);
		string str5 = empty;
		AbilityModPropertyFloat additionalRadiusPerCrystalSpentMod = m_additionalRadiusPerCrystalSpentMod;
		float baseVal4;
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
			baseVal4 = martyrBasicAttack.m_additionalRadiusPerCrystalSpent;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str5 + PropDesc(additionalRadiusPerCrystalSpentMod, "[AdditionalRadiusPerCrystalSpent]", flag, baseVal4);
		string str6 = empty;
		AbilityModPropertyInt extraDamageIfSingleHitMod = m_extraDamageIfSingleHitMod;
		int baseVal5;
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
			baseVal5 = martyrBasicAttack.m_extraDamageIfSingleHit;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str6 + PropDesc(extraDamageIfSingleHitMod, "[ExtraDamageIfSingleHit]", flag, baseVal5);
		string str7 = empty;
		AbilityModPropertyFloat innerRingRadiusMod = m_innerRingRadiusMod;
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
			baseVal6 = martyrBasicAttack.m_innerRingRadius;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str7 + PropDesc(innerRingRadiusMod, "[InnerRingRadius]", flag, baseVal6);
		string str8 = empty;
		AbilityModPropertyFloat innerRingExtraRadiusPerCrystalMod = m_innerRingExtraRadiusPerCrystalMod;
		float baseVal7;
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
			baseVal7 = martyrBasicAttack.m_innerRingExtraRadiusPerCrystal;
		}
		else
		{
			baseVal7 = 0f;
		}
		empty = str8 + PropDesc(innerRingExtraRadiusPerCrystalMod, "[InnerRingExtraRadiusPerCrystal]", flag, baseVal7);
		string str9 = empty;
		AbilityModPropertyInt innerRingDamageMod = m_innerRingDamageMod;
		int baseVal8;
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
			baseVal8 = martyrBasicAttack.m_innerRingDamage;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str9 + PropDesc(innerRingDamageMod, "[InnerRingDamage]", flag, baseVal8);
		return empty + PropDesc(m_innerRingDamagePerCrystalMod, "[InnerRingDamagePerCrystal]", flag, flag ? martyrBasicAttack.m_innerRingDamagePerCrystal : 0);
	}
}
