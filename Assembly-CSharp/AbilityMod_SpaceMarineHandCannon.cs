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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SpaceMarineHandCannon.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_laserDamageMod, "PrimaryDamage", string.Empty, spaceMarineHandCannon.m_primaryDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "PrimaryWidth", string.Empty, spaceMarineHandCannon.m_primaryWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserLengthMod, "PrimaryLength", string.Empty, spaceMarineHandCannon.m_primaryLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneDamageMod, "ConeDamage", string.Empty, spaceMarineHandCannon.m_coneDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_coneAngleMod, "ConeWidthAngle", string.Empty, spaceMarineHandCannon.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneLengthMod, "ConeLength", string.Empty, spaceMarineHandCannon.m_coneLength, true, false, false);
			if (this.m_useLaserHitEffectOverride)
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
				AbilityMod.AddToken_EffectInfo(tokens, this.m_laserHitEffectOverride, "EffectOnLaserTarget", spaceMarineHandCannon.m_effectInfoOnPrimaryTarget, true);
			}
			if (this.m_useConeHitEffectOverride)
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
				AbilityMod.AddToken_EffectInfo(tokens, this.m_coneHitEffectOverride, "EffectInfoOnConeTargets", spaceMarineHandCannon.m_effectInfoOnConeTargets, true);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SpaceMarineHandCannon spaceMarineHandCannon = base.GetTargetAbilityOnAbilityData(abilityData) as SpaceMarineHandCannon;
		bool flag = spaceMarineHandCannon != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt laserDamageMod = this.m_laserDamageMod;
		string prefix = "[Laser Damage]";
		bool showBaseVal = flag;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SpaceMarineHandCannon.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = spaceMarineHandCannon.m_primaryDamage;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(laserDamageMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt coneDamageMod = this.m_coneDamageMod;
		string prefix2 = "[Cone Damage]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = spaceMarineHandCannon.m_coneDamage;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(coneDamageMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat laserLengthMod = this.m_laserLengthMod;
		string prefix3 = "[Laser Length]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = spaceMarineHandCannon.m_primaryLength;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(laserLengthMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix4 = "[Laser Width]";
		bool showBaseVal4 = flag;
		float baseVal4;
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
			baseVal4 = spaceMarineHandCannon.m_primaryWidth;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(laserWidthMod, prefix4, showBaseVal4, baseVal4);
		text += AbilityModHelper.GetModPropertyDesc(this.m_shouldExplodeMod, "[Should Explode?]", flag, true);
		string str5 = text;
		AbilityModPropertyFloat coneAngleMod = this.m_coneAngleMod;
		string prefix5 = "[Cone Angle]";
		bool showBaseVal5 = flag;
		float baseVal5;
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
			baseVal5 = spaceMarineHandCannon.m_coneWidthAngle;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(coneAngleMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyFloat coneLengthMod = this.m_coneLengthMod;
		string prefix6 = "[Cone Length]";
		bool showBaseVal6 = flag;
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
			baseVal6 = spaceMarineHandCannon.m_coneLength;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + AbilityModHelper.GetModPropertyDesc(coneLengthMod, prefix6, showBaseVal6, baseVal6);
		if (this.m_useLaserHitEffectOverride)
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
			string str7 = text;
			StandardEffectInfo laserHitEffectOverride = this.m_laserHitEffectOverride;
			string prefix7 = "{ Laser Hit Effect Override }";
			string empty = string.Empty;
			bool useBaseVal = flag;
			StandardEffectInfo baseVal7;
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
				baseVal7 = spaceMarineHandCannon.m_effectInfoOnPrimaryTarget;
			}
			else
			{
				baseVal7 = null;
			}
			text = str7 + AbilityModHelper.GetModEffectInfoDesc(laserHitEffectOverride, prefix7, empty, useBaseVal, baseVal7);
		}
		if (this.m_useConeHitEffectOverride)
		{
			string str8 = text;
			StandardEffectInfo coneHitEffectOverride = this.m_coneHitEffectOverride;
			string prefix8 = "{ Cone Hit Effect Override }";
			string empty2 = string.Empty;
			bool useBaseVal2 = flag;
			StandardEffectInfo baseVal8;
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
				baseVal8 = spaceMarineHandCannon.m_effectInfoOnConeTargets;
			}
			else
			{
				baseVal8 = null;
			}
			text = str8 + AbilityModHelper.GetModEffectInfoDesc(coneHitEffectOverride, prefix8, empty2, useBaseVal2, baseVal8);
		}
		return text;
	}
}
