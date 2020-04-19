using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BlasterStretchingCone : AbilityMod
{
	[Header("-- Cone Limits")]
	public AbilityModPropertyFloat m_minLengthMod;

	public AbilityModPropertyFloat m_maxLengthMod;

	public AbilityModPropertyFloat m_minAngleMod;

	public AbilityModPropertyFloat m_maxAngleMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	[Header("-- On Hit")]
	public AbilityModPropertyInt m_damageAmountNormalMod;

	public AbilityModPropertyInt m_extraDamageForSingleHitMod;

	public AbilityModPropertyFloat m_extraDamagePerSquareDistanceFromEnemyMod;

	[Header("-- Damage Change by Angle/Distance")]
	public AbilityModPropertyInt m_anglesPerDamageChangeMod;

	public AbilityModPropertyFloat m_distPerDamageChangeMod;

	public AbilityModPropertyInt m_maxDamageChangeMod;

	[Header("-- Effects On Hit")]
	public AbilityModPropertyEffectInfo m_normalEnemyEffectMod;

	public AbilityModPropertyEffectInfo m_overchargedEnemyEffectMod;

	public AbilityModPropertyEffectInfo m_singleEnemyHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(BlasterStretchingCone);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BlasterStretchingCone blasterStretchingCone = targetAbility as BlasterStretchingCone;
		if (blasterStretchingCone != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BlasterStretchingCone.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_minLengthMod, "MinLength", string.Empty, blasterStretchingCone.m_minLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxLengthMod, "MaxLength", string.Empty, blasterStretchingCone.m_maxLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_minAngleMod, "MinAngle", string.Empty, blasterStretchingCone.m_minAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxAngleMod, "MaxAngle", string.Empty, blasterStretchingCone.m_maxAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, blasterStretchingCone.m_coneBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountNormalMod, "DamageAmountNormal", string.Empty, blasterStretchingCone.m_damageAmountNormal, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, blasterStretchingCone.m_extraDamageForSingleHit, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamagePerSquareDistanceFromEnemyMod, "ExtraDamagePerSquareDistanceFromEnemy", string.Empty, blasterStretchingCone.m_extraDamagePerSquareDistanceFromEnemy, true, false, false);
			AbilityMod.AddToken(tokens, this.m_anglesPerDamageChangeMod, "AnglesPerDamageChange", string.Empty, blasterStretchingCone.m_anglesPerDamageChange, true, false);
			AbilityMod.AddToken(tokens, this.m_maxDamageChangeMod, "MaxDamageChange", string.Empty, blasterStretchingCone.m_maxDamageChange, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_normalEnemyEffectMod, "NormalEnemyEffect", blasterStretchingCone.m_normalEnemyEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_overchargedEnemyEffectMod, "OverchargedEnemyEffect", blasterStretchingCone.m_overchargedEnemyEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_singleEnemyHitEffectMod, "SingleEnemyHitEffect", blasterStretchingCone.m_singleEnemyHitEffect, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterStretchingCone blasterStretchingCone = base.GetTargetAbilityOnAbilityData(abilityData) as BlasterStretchingCone;
		bool flag = blasterStretchingCone != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat minLengthMod = this.m_minLengthMod;
		string prefix = "[MinLength]";
		bool showBaseVal = flag;
		float baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BlasterStretchingCone.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = blasterStretchingCone.m_minLength;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(minLengthMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat maxLengthMod = this.m_maxLengthMod;
		string prefix2 = "[MaxLength]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = blasterStretchingCone.m_maxLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(maxLengthMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat minAngleMod = this.m_minAngleMod;
		string prefix3 = "[MinAngle]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = blasterStretchingCone.m_minAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(minAngleMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat maxAngleMod = this.m_maxAngleMod;
		string prefix4 = "[MaxAngle]";
		bool showBaseVal4 = flag;
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
			baseVal4 = blasterStretchingCone.m_maxAngle;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(maxAngleMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyFloat coneBackwardOffsetMod = this.m_coneBackwardOffsetMod;
		string prefix5 = "[ConeBackwardOffset]";
		bool showBaseVal5 = flag;
		float baseVal5;
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
			baseVal5 = blasterStretchingCone.m_coneBackwardOffset;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + base.PropDesc(coneBackwardOffsetMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, flag && blasterStretchingCone.m_penetrateLineOfSight);
		text += base.PropDesc(this.m_damageAmountNormalMod, "[DamageAmountNormal]", flag, (!flag) ? 0 : blasterStretchingCone.m_damageAmountNormal);
		string str6 = text;
		AbilityModPropertyInt extraDamageForSingleHitMod = this.m_extraDamageForSingleHitMod;
		string prefix6 = "[ExtraDamageForSingleHit]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = blasterStretchingCone.m_extraDamageForSingleHit;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(extraDamageForSingleHitMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyFloat extraDamagePerSquareDistanceFromEnemyMod = this.m_extraDamagePerSquareDistanceFromEnemyMod;
		string prefix7 = "[ExtraDamagePerSquareDistanceFromEnemy]";
		bool showBaseVal7 = flag;
		float baseVal7;
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
			baseVal7 = blasterStretchingCone.m_extraDamagePerSquareDistanceFromEnemy;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str7 + base.PropDesc(extraDamagePerSquareDistanceFromEnemyMod, prefix7, showBaseVal7, baseVal7);
		text += base.PropDesc(this.m_anglesPerDamageChangeMod, "[AnglesPerDamageChange]", flag, (!flag) ? 0 : blasterStretchingCone.m_anglesPerDamageChange);
		string str8 = text;
		AbilityModPropertyFloat distPerDamageChangeMod = this.m_distPerDamageChangeMod;
		string prefix8 = "[DistPerDamageChange]";
		bool showBaseVal8 = flag;
		float baseVal8;
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
			baseVal8 = blasterStretchingCone.m_distPerDamageChange;
		}
		else
		{
			baseVal8 = 0f;
		}
		text = str8 + base.PropDesc(distPerDamageChangeMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt maxDamageChangeMod = this.m_maxDamageChangeMod;
		string prefix9 = "[MaxDamageChange]";
		bool showBaseVal9 = flag;
		int baseVal9;
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
			baseVal9 = blasterStretchingCone.m_maxDamageChange;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(maxDamageChangeMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyEffectInfo normalEnemyEffectMod = this.m_normalEnemyEffectMod;
		string prefix10 = "[NormalEnemyEffect]";
		bool showBaseVal10 = flag;
		StandardEffectInfo baseVal10;
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
			baseVal10 = blasterStretchingCone.m_normalEnemyEffect;
		}
		else
		{
			baseVal10 = null;
		}
		text = str10 + base.PropDesc(normalEnemyEffectMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyEffectInfo overchargedEnemyEffectMod = this.m_overchargedEnemyEffectMod;
		string prefix11 = "[OverchargedEnemyEffect]";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal11;
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
			baseVal11 = blasterStretchingCone.m_overchargedEnemyEffect;
		}
		else
		{
			baseVal11 = null;
		}
		text = str11 + base.PropDesc(overchargedEnemyEffectMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyEffectInfo singleEnemyHitEffectMod = this.m_singleEnemyHitEffectMod;
		string prefix12 = "[SingleEnemyHitEffect]";
		bool showBaseVal12 = flag;
		StandardEffectInfo baseVal12;
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
			baseVal12 = blasterStretchingCone.m_singleEnemyHitEffect;
		}
		else
		{
			baseVal12 = null;
		}
		return str12 + base.PropDesc(singleEnemyHitEffectMod, prefix12, showBaseVal12, baseVal12);
	}
}
