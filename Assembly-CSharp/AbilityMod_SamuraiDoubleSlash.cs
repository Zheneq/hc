using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SamuraiDoubleSlash : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	public AbilityModPropertyFloat m_maxAngleBetweenMod;

	[Header("    Cone(s)")]
	public AbilityModPropertyFloat m_coneWidthAngleMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	public AbilityModPropertyFloat m_coneLengthMod;

	[Header("    Laser(s)")]
	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserLengthMod;

	[Header("-- On Hit Damage/Effect")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyInt m_overlapExtraDamageMod;

	public AbilityModPropertyEffectInfo m_targetHitEffectMod;

	[Header("-- Extra Effect if SelfBuff ability is used on same turn")]
	public AbilityModPropertyEffectInfo m_extraEnemyHitEffectIfSelfBuffedMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SamuraiDoubleSlash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SamuraiDoubleSlash samuraiDoubleSlash = targetAbility as SamuraiDoubleSlash;
		if (samuraiDoubleSlash != null)
		{
			AbilityMod.AddToken(tokens, m_maxAngleBetweenMod, "MaxAngleBetween", string.Empty, samuraiDoubleSlash.m_maxAngleBetween);
			AbilityMod.AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, samuraiDoubleSlash.m_coneWidthAngle);
			AbilityMod.AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, samuraiDoubleSlash.m_coneBackwardOffset);
			AbilityMod.AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, samuraiDoubleSlash.m_coneLength);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, samuraiDoubleSlash.m_laserWidth);
			AbilityMod.AddToken(tokens, m_laserLengthMod, "LaserLength", string.Empty, samuraiDoubleSlash.m_laserLength);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, samuraiDoubleSlash.m_damageAmount);
			AbilityMod.AddToken(tokens, m_overlapExtraDamageMod, "OverlapExtraDamage", string.Empty, samuraiDoubleSlash.m_overlapExtraDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_targetHitEffectMod, "TargetHitEffect", samuraiDoubleSlash.m_targetHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_extraEnemyHitEffectIfSelfBuffedMod, "ExtraEnemyHitEffectIfSelfBuffed", samuraiDoubleSlash.m_extraEnemyHitEffectIfSelfBuffed);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiDoubleSlash samuraiDoubleSlash = GetTargetAbilityOnAbilityData(abilityData) as SamuraiDoubleSlash;
		bool flag = samuraiDoubleSlash != null;
		string empty = string.Empty;
		empty += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, flag && samuraiDoubleSlash.m_penetrateLineOfSight);
		string str = empty;
		AbilityModPropertyFloat maxAngleBetweenMod = m_maxAngleBetweenMod;
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
			baseVal = samuraiDoubleSlash.m_maxAngleBetween;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(maxAngleBetweenMod, "[MaxAngleBetween]", flag, baseVal);
		empty += PropDesc(m_coneWidthAngleMod, "[ConeWidthAngle]", flag, (!flag) ? 0f : samuraiDoubleSlash.m_coneWidthAngle);
		empty += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, (!flag) ? 0f : samuraiDoubleSlash.m_coneBackwardOffset);
		string str2 = empty;
		AbilityModPropertyFloat coneLengthMod = m_coneLengthMod;
		float baseVal2;
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
			baseVal2 = samuraiDoubleSlash.m_coneLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(coneLengthMod, "[ConeLength]", flag, baseVal2);
		empty += PropDesc(m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : samuraiDoubleSlash.m_laserWidth);
		empty += PropDesc(m_laserLengthMod, "[LaserLength]", flag, (!flag) ? 0f : samuraiDoubleSlash.m_laserLength);
		empty += PropDesc(m_damageAmountMod, "[DamageAmount]", flag, flag ? samuraiDoubleSlash.m_damageAmount : 0);
		empty += PropDesc(m_overlapExtraDamageMod, "[OverlapExtraDamage]", flag, flag ? samuraiDoubleSlash.m_overlapExtraDamage : 0);
		string str3 = empty;
		AbilityModPropertyEffectInfo targetHitEffectMod = m_targetHitEffectMod;
		object baseVal3;
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
			baseVal3 = samuraiDoubleSlash.m_targetHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str3 + PropDesc(targetHitEffectMod, "[TargetHitEffect]", flag, (StandardEffectInfo)baseVal3);
		string str4 = empty;
		AbilityModPropertyEffectInfo extraEnemyHitEffectIfSelfBuffedMod = m_extraEnemyHitEffectIfSelfBuffedMod;
		object baseVal4;
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
			baseVal4 = samuraiDoubleSlash.m_extraEnemyHitEffectIfSelfBuffed;
		}
		else
		{
			baseVal4 = null;
		}
		return str4 + PropDesc(extraEnemyHitEffectIfSelfBuffedMod, "[ExtraEnemyHitEffectIfSelfBuffed]", flag, (StandardEffectInfo)baseVal4);
	}
}
