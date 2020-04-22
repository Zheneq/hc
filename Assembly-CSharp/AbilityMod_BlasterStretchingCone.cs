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
		if (!(blasterStretchingCone != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_minLengthMod, "MinLength", string.Empty, blasterStretchingCone.m_minLength);
			AbilityMod.AddToken(tokens, m_maxLengthMod, "MaxLength", string.Empty, blasterStretchingCone.m_maxLength);
			AbilityMod.AddToken(tokens, m_minAngleMod, "MinAngle", string.Empty, blasterStretchingCone.m_minAngle);
			AbilityMod.AddToken(tokens, m_maxAngleMod, "MaxAngle", string.Empty, blasterStretchingCone.m_maxAngle);
			AbilityMod.AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, blasterStretchingCone.m_coneBackwardOffset);
			AbilityMod.AddToken(tokens, m_damageAmountNormalMod, "DamageAmountNormal", string.Empty, blasterStretchingCone.m_damageAmountNormal);
			AbilityMod.AddToken(tokens, m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, blasterStretchingCone.m_extraDamageForSingleHit);
			AbilityMod.AddToken(tokens, m_extraDamagePerSquareDistanceFromEnemyMod, "ExtraDamagePerSquareDistanceFromEnemy", string.Empty, blasterStretchingCone.m_extraDamagePerSquareDistanceFromEnemy);
			AbilityMod.AddToken(tokens, m_anglesPerDamageChangeMod, "AnglesPerDamageChange", string.Empty, blasterStretchingCone.m_anglesPerDamageChange);
			AbilityMod.AddToken(tokens, m_maxDamageChangeMod, "MaxDamageChange", string.Empty, blasterStretchingCone.m_maxDamageChange);
			AbilityMod.AddToken_EffectMod(tokens, m_normalEnemyEffectMod, "NormalEnemyEffect", blasterStretchingCone.m_normalEnemyEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_overchargedEnemyEffectMod, "OverchargedEnemyEffect", blasterStretchingCone.m_overchargedEnemyEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_singleEnemyHitEffectMod, "SingleEnemyHitEffect", blasterStretchingCone.m_singleEnemyHitEffect);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterStretchingCone blasterStretchingCone = GetTargetAbilityOnAbilityData(abilityData) as BlasterStretchingCone;
		bool flag = blasterStretchingCone != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat minLengthMod = m_minLengthMod;
		float baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = blasterStretchingCone.m_minLength;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(minLengthMod, "[MinLength]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat maxLengthMod = m_maxLengthMod;
		float baseVal2;
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
			baseVal2 = blasterStretchingCone.m_maxLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(maxLengthMod, "[MaxLength]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat minAngleMod = m_minAngleMod;
		float baseVal3;
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
			baseVal3 = blasterStretchingCone.m_minAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(minAngleMod, "[MinAngle]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat maxAngleMod = m_maxAngleMod;
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
			baseVal4 = blasterStretchingCone.m_maxAngle;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(maxAngleMod, "[MaxAngle]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyFloat coneBackwardOffsetMod = m_coneBackwardOffsetMod;
		float baseVal5;
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
			baseVal5 = blasterStretchingCone.m_coneBackwardOffset;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + PropDesc(coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, baseVal5);
		empty += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, flag && blasterStretchingCone.m_penetrateLineOfSight);
		empty += PropDesc(m_damageAmountNormalMod, "[DamageAmountNormal]", flag, flag ? blasterStretchingCone.m_damageAmountNormal : 0);
		string str6 = empty;
		AbilityModPropertyInt extraDamageForSingleHitMod = m_extraDamageForSingleHitMod;
		int baseVal6;
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
			baseVal6 = blasterStretchingCone.m_extraDamageForSingleHit;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(extraDamageForSingleHitMod, "[ExtraDamageForSingleHit]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyFloat extraDamagePerSquareDistanceFromEnemyMod = m_extraDamagePerSquareDistanceFromEnemyMod;
		float baseVal7;
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
			baseVal7 = blasterStretchingCone.m_extraDamagePerSquareDistanceFromEnemy;
		}
		else
		{
			baseVal7 = 0f;
		}
		empty = str7 + PropDesc(extraDamagePerSquareDistanceFromEnemyMod, "[ExtraDamagePerSquareDistanceFromEnemy]", flag, baseVal7);
		empty += PropDesc(m_anglesPerDamageChangeMod, "[AnglesPerDamageChange]", flag, flag ? blasterStretchingCone.m_anglesPerDamageChange : 0);
		string str8 = empty;
		AbilityModPropertyFloat distPerDamageChangeMod = m_distPerDamageChangeMod;
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
			baseVal8 = blasterStretchingCone.m_distPerDamageChange;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str8 + PropDesc(distPerDamageChangeMod, "[DistPerDamageChange]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyInt maxDamageChangeMod = m_maxDamageChangeMod;
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
			baseVal9 = blasterStretchingCone.m_maxDamageChange;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(maxDamageChangeMod, "[MaxDamageChange]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyEffectInfo normalEnemyEffectMod = m_normalEnemyEffectMod;
		object baseVal10;
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
			baseVal10 = blasterStretchingCone.m_normalEnemyEffect;
		}
		else
		{
			baseVal10 = null;
		}
		empty = str10 + PropDesc(normalEnemyEffectMod, "[NormalEnemyEffect]", flag, (StandardEffectInfo)baseVal10);
		string str11 = empty;
		AbilityModPropertyEffectInfo overchargedEnemyEffectMod = m_overchargedEnemyEffectMod;
		object baseVal11;
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
			baseVal11 = blasterStretchingCone.m_overchargedEnemyEffect;
		}
		else
		{
			baseVal11 = null;
		}
		empty = str11 + PropDesc(overchargedEnemyEffectMod, "[OverchargedEnemyEffect]", flag, (StandardEffectInfo)baseVal11);
		string str12 = empty;
		AbilityModPropertyEffectInfo singleEnemyHitEffectMod = m_singleEnemyHitEffectMod;
		object baseVal12;
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
			baseVal12 = blasterStretchingCone.m_singleEnemyHitEffect;
		}
		else
		{
			baseVal12 = null;
		}
		return str12 + PropDesc(singleEnemyHitEffectMod, "[SingleEnemyHitEffect]", flag, (StandardEffectInfo)baseVal12);
	}
}
