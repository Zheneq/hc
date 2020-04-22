using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ExoPunch : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_coneWidthAngleMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	public AbilityModPropertyFloat m_coneLengthMod;

	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Header("-- Knockback")]
	public AbilityModPropertyFloat m_knockbackDistanceMod;

	public AbilityModPropertyKnockbackType m_knockbackTypeMod;

	[Header("-- On Hit Damage/Effect")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyEffectInfo m_targetHitEffectMod;

	[Header("-- Nearby Hit Bonus")]
	public AbilityModPropertyFloat m_nearDistThresholdMod;

	public AbilityModPropertyInt m_nearEnemyExtraDamageMod;

	public AbilityModPropertyEffectInfo m_nearEnemyExtraEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ExoPunch);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ExoPunch exoPunch = targetAbility as ExoPunch;
		if (exoPunch != null)
		{
			AbilityMod.AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, exoPunch.m_coneWidthAngle);
			AbilityMod.AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, exoPunch.m_coneBackwardOffset);
			AbilityMod.AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, exoPunch.m_coneLength);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, exoPunch.m_maxTargets);
			AbilityMod.AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", string.Empty, exoPunch.m_knockbackDistance);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, exoPunch.m_damageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_targetHitEffectMod, "TargetHitEffect", exoPunch.m_targetHitEffect);
			AbilityMod.AddToken(tokens, m_nearDistThresholdMod, "NearDistThreshold", string.Empty, exoPunch.m_nearDistThreshold);
			AbilityMod.AddToken(tokens, m_nearEnemyExtraDamageMod, "NearEnemyExtraDamage", string.Empty, exoPunch.m_nearEnemyExtraDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_nearEnemyExtraEffectMod, "NearEnemyExtraEffect", exoPunch.m_nearEnemyExtraEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ExoPunch exoPunch = GetTargetAbilityOnAbilityData(abilityData) as ExoPunch;
		bool flag = exoPunch != null;
		string empty = string.Empty;
		empty += PropDesc(m_coneWidthAngleMod, "[ConeWidthAngle]", flag, (!flag) ? 0f : exoPunch.m_coneWidthAngle);
		string str = empty;
		AbilityModPropertyFloat coneBackwardOffsetMod = m_coneBackwardOffsetMod;
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
			baseVal = exoPunch.m_coneBackwardOffset;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, baseVal);
		empty += PropDesc(m_coneLengthMod, "[ConeLength]", flag, (!flag) ? 0f : exoPunch.m_coneLength);
		string str2 = empty;
		AbilityModPropertyBool penetrateLineOfSightMod = m_penetrateLineOfSightMod;
		int baseVal2;
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
			baseVal2 = (exoPunch.m_penetrateLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyInt maxTargetsMod = m_maxTargetsMod;
		int baseVal3;
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
			baseVal3 = exoPunch.m_maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(maxTargetsMod, "[MaxTargets]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat knockbackDistanceMod = m_knockbackDistanceMod;
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
			baseVal4 = exoPunch.m_knockbackDistance;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(knockbackDistanceMod, "[KnockbackDistance]", flag, baseVal4);
		empty += PropDesc(m_knockbackTypeMod, "[KnockbackType]", flag, (!flag) ? KnockbackType.AwayFromSource : exoPunch.m_knockbackType);
		string str5 = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal5;
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
			baseVal5 = exoPunch.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyEffectInfo targetHitEffectMod = m_targetHitEffectMod;
		object baseVal6;
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
			baseVal6 = exoPunch.m_targetHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(targetHitEffectMod, "[TargetHitEffect]", flag, (StandardEffectInfo)baseVal6);
		empty += PropDesc(m_nearDistThresholdMod, "[NearDistThreshold]", flag, (!flag) ? 0f : exoPunch.m_nearDistThreshold);
		string str7 = empty;
		AbilityModPropertyInt nearEnemyExtraDamageMod = m_nearEnemyExtraDamageMod;
		int baseVal7;
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
			baseVal7 = exoPunch.m_nearEnemyExtraDamage;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(nearEnemyExtraDamageMod, "[NearEnemyExtraDamage]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyEffectInfo nearEnemyExtraEffectMod = m_nearEnemyExtraEffectMod;
		object baseVal8;
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
			baseVal8 = exoPunch.m_nearEnemyExtraEffect;
		}
		else
		{
			baseVal8 = null;
		}
		return str8 + PropDesc(nearEnemyExtraEffectMod, "[NearEnemyExtraEffect]", flag, (StandardEffectInfo)baseVal8);
	}
}
