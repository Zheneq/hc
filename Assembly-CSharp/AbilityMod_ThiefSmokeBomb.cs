using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ThiefSmokeBomb : AbilityMod
{
	[Header("-- Bomb Damage")]
	public AbilityModPropertyInt m_extraDamageOnCastMod;

	[Header("-- Bomb Targeting (shape is in Smoke Field Info)")]
	public AbilityModPropertyBool m_penetrateLosMod;

	public AbilityModPropertyInt m_maxAngleWithFirstSegmentMod;

	public AbilityModPropertyFloat m_maxDistanceWithFirstMod;

	public AbilityModPropertyFloat m_minDistanceBetweenBombsMod;

	[Header("-- On Cast Hit Effect")]
	public AbilityModPropertyEffectInfo m_bombHitEffectInfoMod;

	[Header("-- Smoke Field")]
	public AbilityModPropertyGroundEffectField m_smokeFieldInfoMod;

	[Header("-- Barrier (will make square out of 4 barriers around ground field)")]
	public AbilityModPropertyBool m_addBarriersMod;

	public AbilityModPropertyFloat m_barrierSquareWidthMod;

	public AbilityModPropertyBarrierDataV2 m_barrierDataMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ThiefSmokeBomb);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ThiefSmokeBomb thiefSmokeBomb = targetAbility as ThiefSmokeBomb;
		if (thiefSmokeBomb != null)
		{
			AbilityMod.AddToken(tokens, m_extraDamageOnCastMod, "ExtraDamageOnCast", string.Empty, thiefSmokeBomb.m_extraDamageOnCast);
			AbilityMod.AddToken(tokens, m_maxAngleWithFirstSegmentMod, "MaxAngleWithFirstSegment", string.Empty, thiefSmokeBomb.m_maxAngleWithFirstSegment);
			AbilityMod.AddToken(tokens, m_maxDistanceWithFirstMod, "MaxDistanceWithFirst", string.Empty, thiefSmokeBomb.m_maxDistanceWithFirst);
			AbilityMod.AddToken(tokens, m_minDistanceBetweenBombsMod, "MinDistanceBetweenBombs", string.Empty, thiefSmokeBomb.m_minDistanceBetweenBombs);
			AbilityMod.AddToken_EffectMod(tokens, m_bombHitEffectInfoMod, "BombHitEffectInfo", thiefSmokeBomb.m_bombHitEffectInfo);
			AbilityMod.AddToken_GroundFieldMod(tokens, m_smokeFieldInfoMod, "SmokeFieldInfo", thiefSmokeBomb.m_smokeFieldInfo);
			AbilityMod.AddToken(tokens, m_barrierSquareWidthMod, "BarrierSquareWidth", string.Empty, thiefSmokeBomb.m_barrierSquareWidth);
			AbilityMod.AddToken_BarrierMod(tokens, m_barrierDataMod, "BarrierData", thiefSmokeBomb.m_barrierData);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefSmokeBomb thiefSmokeBomb = GetTargetAbilityOnAbilityData(abilityData) as ThiefSmokeBomb;
		bool flag = thiefSmokeBomb != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt extraDamageOnCastMod = m_extraDamageOnCastMod;
		int baseVal;
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
			baseVal = thiefSmokeBomb.m_extraDamageOnCast;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(extraDamageOnCastMod, "[ExtraDamageonCast]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyBool penetrateLosMod = m_penetrateLosMod;
		int baseVal2;
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
			baseVal2 = (thiefSmokeBomb.m_penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(penetrateLosMod, "[PenetrateLos]", flag, (byte)baseVal2 != 0);
		empty += PropDesc(m_maxAngleWithFirstSegmentMod, "[MaxAngleWithFirstSegment]", flag, flag ? thiefSmokeBomb.m_maxAngleWithFirstSegment : 0);
		empty += PropDesc(m_maxDistanceWithFirstMod, "[MaxDistanceWithFirst]", flag, (!flag) ? 0f : thiefSmokeBomb.m_maxDistanceWithFirst);
		string str3 = empty;
		AbilityModPropertyFloat minDistanceBetweenBombsMod = m_minDistanceBetweenBombsMod;
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
			baseVal3 = thiefSmokeBomb.m_minDistanceBetweenBombs;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(minDistanceBetweenBombsMod, "[MinDistanceBetweenBombs]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyEffectInfo bombHitEffectInfoMod = m_bombHitEffectInfoMod;
		object baseVal4;
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
			baseVal4 = thiefSmokeBomb.m_bombHitEffectInfo;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + PropDesc(bombHitEffectInfoMod, "[BombHitEffectInfo]", flag, (StandardEffectInfo)baseVal4);
		empty += PropDescGroundFieldMod(m_smokeFieldInfoMod, "{ SmokeFieldInfo }", thiefSmokeBomb.m_smokeFieldInfo);
		string str5 = empty;
		AbilityModPropertyBool addBarriersMod = m_addBarriersMod;
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
			baseVal5 = (thiefSmokeBomb.m_addBarriers ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(addBarriersMod, "[AddBarriers]", flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertyFloat barrierSquareWidthMod = m_barrierSquareWidthMod;
		float baseVal6;
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
			baseVal6 = thiefSmokeBomb.m_barrierSquareWidth;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + PropDesc(barrierSquareWidthMod, "[BarrierSquareWidth]", flag, baseVal6);
		return empty + PropDescBarrierMod(m_barrierDataMod, "{ BarrierData }", thiefSmokeBomb.m_barrierData);
	}
}
