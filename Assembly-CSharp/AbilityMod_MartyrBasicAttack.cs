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
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "LaserInfo", martyrBasicAttack.m_laserInfo, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_laserHitEffectMod, "LaserHitEffect", martyrBasicAttack.m_laserHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_explosionRadiusMod, "ExplosionRadius", string.Empty, martyrBasicAttack.m_explosionRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_baseLaserDamageMod, "BaseLaserDamage", string.Empty, martyrBasicAttack.m_baseLaserDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_baseExplosionDamageMod, "BaseExplosionDamage", string.Empty, martyrBasicAttack.m_baseExplosionDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_additionalDamagePerCrystalSpentMod, "AdditionalDamagePerCrystalSpent", string.Empty, martyrBasicAttack.m_additionalDamagePerCrystalSpent, true, false);
			AbilityMod.AddToken(tokens, this.m_additionalRadiusPerCrystalSpentMod, "AdditionalRadiusPerCrystalSpent", string.Empty, martyrBasicAttack.m_additionalRadiusPerCrystalSpent, true, true, true);
			AbilityMod.AddToken(tokens, this.m_extraDamageIfSingleHitMod, "ExtraDamageIfSingleHit", string.Empty, martyrBasicAttack.m_extraDamageIfSingleHit, true, false);
			AbilityMod.AddToken(tokens, this.m_innerRingRadiusMod, "InnerRingRadius", string.Empty, martyrBasicAttack.m_innerRingRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_innerRingExtraRadiusPerCrystalMod, "InnerRingExtraRadiusPerCrystal", string.Empty, martyrBasicAttack.m_innerRingExtraRadiusPerCrystal, true, false, false);
			AbilityMod.AddToken(tokens, this.m_innerRingDamageMod, "InnerRingDamage", string.Empty, martyrBasicAttack.m_innerRingDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_innerRingDamagePerCrystalMod, "InnerRingDamagePerCrystal", string.Empty, martyrBasicAttack.m_innerRingDamagePerCrystal, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrBasicAttack martyrBasicAttack = base.GetTargetAbilityOnAbilityData(abilityData) as MartyrBasicAttack;
		bool flag = martyrBasicAttack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyLaserInfo laserInfoMod = this.m_laserInfoMod;
		string prefix = "[LaserInfo]";
		bool showBaseVal = flag;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_MartyrBasicAttack.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseLaserInfo = martyrBasicAttack.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		text = str + base.PropDesc(laserInfoMod, prefix, showBaseVal, baseLaserInfo);
		text += base.PropDesc(this.m_laserHitEffectMod, "[LaserHitEffect]", flag, (!flag) ? null : martyrBasicAttack.m_laserHitEffect);
		text += base.PropDesc(this.m_explosionRadiusMod, "[ExplosionRadius]", flag, (!flag) ? 0f : martyrBasicAttack.m_explosionRadius);
		string str2 = text;
		AbilityModPropertyInt baseLaserDamageMod = this.m_baseLaserDamageMod;
		string prefix2 = "[BaseLaserDamage]";
		bool showBaseVal2 = flag;
		int baseVal;
		if (flag)
		{
			for (;;)
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
		text = str2 + base.PropDesc(baseLaserDamageMod, prefix2, showBaseVal2, baseVal);
		string str3 = text;
		AbilityModPropertyInt baseExplosionDamageMod = this.m_baseExplosionDamageMod;
		string prefix3 = "[BaseExplosionDamage]";
		bool showBaseVal3 = flag;
		int baseVal2;
		if (flag)
		{
			for (;;)
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
		text = str3 + base.PropDesc(baseExplosionDamageMod, prefix3, showBaseVal3, baseVal2);
		string str4 = text;
		AbilityModPropertyInt additionalDamagePerCrystalSpentMod = this.m_additionalDamagePerCrystalSpentMod;
		string prefix4 = "[AdditionalDamagePerCrystalSpent]";
		bool showBaseVal4 = flag;
		int baseVal3;
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
			baseVal3 = martyrBasicAttack.m_additionalDamagePerCrystalSpent;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str4 + base.PropDesc(additionalDamagePerCrystalSpentMod, prefix4, showBaseVal4, baseVal3);
		string str5 = text;
		AbilityModPropertyFloat additionalRadiusPerCrystalSpentMod = this.m_additionalRadiusPerCrystalSpentMod;
		string prefix5 = "[AdditionalRadiusPerCrystalSpent]";
		bool showBaseVal5 = flag;
		float baseVal4;
		if (flag)
		{
			for (;;)
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
		text = str5 + base.PropDesc(additionalRadiusPerCrystalSpentMod, prefix5, showBaseVal5, baseVal4);
		string str6 = text;
		AbilityModPropertyInt extraDamageIfSingleHitMod = this.m_extraDamageIfSingleHitMod;
		string prefix6 = "[ExtraDamageIfSingleHit]";
		bool showBaseVal6 = flag;
		int baseVal5;
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
			baseVal5 = martyrBasicAttack.m_extraDamageIfSingleHit;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str6 + base.PropDesc(extraDamageIfSingleHitMod, prefix6, showBaseVal6, baseVal5);
		string str7 = text;
		AbilityModPropertyFloat innerRingRadiusMod = this.m_innerRingRadiusMod;
		string prefix7 = "[InnerRingRadius]";
		bool showBaseVal7 = flag;
		float baseVal6;
		if (flag)
		{
			for (;;)
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
		text = str7 + base.PropDesc(innerRingRadiusMod, prefix7, showBaseVal7, baseVal6);
		string str8 = text;
		AbilityModPropertyFloat innerRingExtraRadiusPerCrystalMod = this.m_innerRingExtraRadiusPerCrystalMod;
		string prefix8 = "[InnerRingExtraRadiusPerCrystal]";
		bool showBaseVal8 = flag;
		float baseVal7;
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
			baseVal7 = martyrBasicAttack.m_innerRingExtraRadiusPerCrystal;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str8 + base.PropDesc(innerRingExtraRadiusPerCrystalMod, prefix8, showBaseVal8, baseVal7);
		string str9 = text;
		AbilityModPropertyInt innerRingDamageMod = this.m_innerRingDamageMod;
		string prefix9 = "[InnerRingDamage]";
		bool showBaseVal9 = flag;
		int baseVal8;
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
			baseVal8 = martyrBasicAttack.m_innerRingDamage;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str9 + base.PropDesc(innerRingDamageMod, prefix9, showBaseVal9, baseVal8);
		return text + base.PropDesc(this.m_innerRingDamagePerCrystalMod, "[InnerRingDamagePerCrystal]", flag, (!flag) ? 0 : martyrBasicAttack.m_innerRingDamagePerCrystal);
	}
}
