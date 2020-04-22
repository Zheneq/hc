using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ExoDualCone : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyConeInfo m_coneInfoMod;

	public AbilityModPropertyFloat m_leftConeHorizontalOffsetMod;

	public AbilityModPropertyFloat m_rightConeHorizontalOffsetMod;

	public AbilityModPropertyFloat m_coneForwardOffsetMod;

	[Space(10f)]
	public AbilityModPropertyFloat m_leftConeDegreesFromForwardMod;

	public AbilityModPropertyFloat m_rightConeDegreesFromForwardMod;

	[Header("-- Targeting, if interpolating angle")]
	public AbilityModPropertyBool m_interpolateAngleMod;

	public AbilityModPropertyFloat m_interpolateMinAngleMod;

	public AbilityModPropertyFloat m_interpolateMaxAngleMod;

	public AbilityModPropertyFloat m_interpolateMinDistMod;

	public AbilityModPropertyFloat m_interpolateMaxDistMod;

	[Header("-- Damage")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyInt m_extraDamageForOverlapMod;

	public AbilityModPropertyInt m_extraDamageForSingleHitMod;

	public AbilityModPropertyEffectInfo m_effectOnHitMod;

	public AbilityModPropertyEffectInfo m_effectOnOverlapHitMod;

	[Header("-- Extra Damage for hitting on consecutive turns")]
	public AbilityModPropertyInt m_extraDamageForConsecitiveHitMod;

	public AbilityModPropertyInt m_extraEnergyForConsecutiveUseMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ExoDualCone);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ExoDualCone exoDualCone = targetAbility as ExoDualCone;
		if (!(exoDualCone != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken_ConeInfo(tokens, m_coneInfoMod, "ConeInfo", exoDualCone.m_coneInfo);
			AbilityMod.AddToken(tokens, m_leftConeHorizontalOffsetMod, "LeftConeHorizontalOffset", string.Empty, exoDualCone.m_leftConeHorizontalOffset);
			AbilityMod.AddToken(tokens, m_rightConeHorizontalOffsetMod, "RightConeHorizontalOffset", string.Empty, exoDualCone.m_rightConeHorizontalOffset);
			AbilityMod.AddToken(tokens, m_coneForwardOffsetMod, "ConeForwardOffset", string.Empty, exoDualCone.m_coneForwardOffset);
			AbilityMod.AddToken(tokens, m_leftConeDegreesFromForwardMod, "LeftConeDegreesFromForward", string.Empty, exoDualCone.m_leftConeDegreesFromForward);
			AbilityMod.AddToken(tokens, m_rightConeDegreesFromForwardMod, "RightConeDegreesFromForward", string.Empty, exoDualCone.m_rightConeDegreesFromForward);
			AbilityMod.AddToken(tokens, m_interpolateMinAngleMod, "InterpolateMinAngle", string.Empty, exoDualCone.m_interpolateMinAngle);
			AbilityMod.AddToken(tokens, m_interpolateMaxAngleMod, "InterpolateMaxAngle", string.Empty, exoDualCone.m_interpolateMaxAngle);
			AbilityMod.AddToken(tokens, m_interpolateMinDistMod, "InterpolateMinDist", string.Empty, exoDualCone.m_interpolateMinDist);
			AbilityMod.AddToken(tokens, m_interpolateMaxDistMod, "InterpolateMaxDist", string.Empty, exoDualCone.m_interpolateMaxDist);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, exoDualCone.m_damageAmount);
			AbilityMod.AddToken(tokens, m_extraDamageForOverlapMod, "ExtraDamageForOverlap", string.Empty, exoDualCone.m_extraDamageForOverlap);
			AbilityMod.AddToken(tokens, m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, exoDualCone.m_extraDamageForSingleHit);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnHitMod, "EffectOnHit", exoDualCone.m_effectOnHit);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnOverlapHitMod, "EffectOnOverlapHit", exoDualCone.m_effectOnOverlapHit);
			AbilityMod.AddToken(tokens, m_extraDamageForConsecitiveHitMod, "ExtraDamageForConsecitiveHit", string.Empty, exoDualCone.m_extraDamageForConsecutiveUse);
			AbilityMod.AddToken(tokens, m_extraEnergyForConsecutiveUseMod, "ExtraEnergyForConsecutiveUse", string.Empty, exoDualCone.m_extraEnergyForConsecutiveUse);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ExoDualCone exoDualCone = GetTargetAbilityOnAbilityData(abilityData) as ExoDualCone;
		bool flag = exoDualCone != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyConeInfo coneInfoMod = m_coneInfoMod;
		object baseConeInfo;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseConeInfo = exoDualCone.m_coneInfo;
		}
		else
		{
			baseConeInfo = null;
		}
		empty = str + PropDesc(coneInfoMod, "[ConeInfo]", flag, (ConeTargetingInfo)baseConeInfo);
		empty += PropDesc(m_leftConeHorizontalOffsetMod, "[LeftConeHorizontalOffset]", flag, (!flag) ? 0f : exoDualCone.m_leftConeHorizontalOffset);
		string str2 = empty;
		AbilityModPropertyFloat rightConeHorizontalOffsetMod = m_rightConeHorizontalOffsetMod;
		float baseVal;
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
			baseVal = exoDualCone.m_rightConeHorizontalOffset;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str2 + PropDesc(rightConeHorizontalOffsetMod, "[RightConeHorizontalOffset]", flag, baseVal);
		string str3 = empty;
		AbilityModPropertyFloat coneForwardOffsetMod = m_coneForwardOffsetMod;
		float baseVal2;
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
			baseVal2 = exoDualCone.m_coneForwardOffset;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str3 + PropDesc(coneForwardOffsetMod, "[ConeForwardOffset]", flag, baseVal2);
		string str4 = empty;
		AbilityModPropertyFloat leftConeDegreesFromForwardMod = m_leftConeDegreesFromForwardMod;
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
			baseVal3 = exoDualCone.m_leftConeDegreesFromForward;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str4 + PropDesc(leftConeDegreesFromForwardMod, "[LeftConeDegreesFromForward]", flag, baseVal3);
		string str5 = empty;
		AbilityModPropertyFloat rightConeDegreesFromForwardMod = m_rightConeDegreesFromForwardMod;
		float baseVal4;
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
			baseVal4 = exoDualCone.m_rightConeDegreesFromForward;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str5 + PropDesc(rightConeDegreesFromForwardMod, "[RightConeDegreesFromForward]", flag, baseVal4);
		string str6 = empty;
		AbilityModPropertyBool interpolateAngleMod = m_interpolateAngleMod;
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
			baseVal5 = (exoDualCone.m_interpolateAngle ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str6 + PropDesc(interpolateAngleMod, "[InterpolateAngle]", flag, (byte)baseVal5 != 0);
		string str7 = empty;
		AbilityModPropertyFloat interpolateMinAngleMod = m_interpolateMinAngleMod;
		float baseVal6;
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
			baseVal6 = exoDualCone.m_interpolateMinAngle;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str7 + PropDesc(interpolateMinAngleMod, "[InterpolateMinAngle]", flag, baseVal6);
		string str8 = empty;
		AbilityModPropertyFloat interpolateMaxAngleMod = m_interpolateMaxAngleMod;
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
			baseVal7 = exoDualCone.m_interpolateMaxAngle;
		}
		else
		{
			baseVal7 = 0f;
		}
		empty = str8 + PropDesc(interpolateMaxAngleMod, "[InterpolateMaxAngle]", flag, baseVal7);
		string str9 = empty;
		AbilityModPropertyFloat interpolateMinDistMod = m_interpolateMinDistMod;
		float baseVal8;
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
			baseVal8 = exoDualCone.m_interpolateMinDist;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str9 + PropDesc(interpolateMinDistMod, "[InterpolateMinDist]", flag, baseVal8);
		string str10 = empty;
		AbilityModPropertyFloat interpolateMaxDistMod = m_interpolateMaxDistMod;
		float baseVal9;
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
			baseVal9 = exoDualCone.m_interpolateMaxDist;
		}
		else
		{
			baseVal9 = 0f;
		}
		empty = str10 + PropDesc(interpolateMaxDistMod, "[InterpolateMaxDist]", flag, baseVal9);
		empty += PropDesc(m_damageAmountMod, "[DamageAmount]", flag, flag ? exoDualCone.m_damageAmount : 0);
		empty += PropDesc(m_extraDamageForOverlapMod, "[ExtraDamageForOverlap]", flag, flag ? exoDualCone.m_extraDamageForOverlap : 0);
		empty += PropDesc(m_extraDamageForSingleHitMod, "[ExtraDamageForSingleHit]", flag, flag ? exoDualCone.m_extraDamageForSingleHit : 0);
		string str11 = empty;
		AbilityModPropertyEffectInfo effectOnHitMod = m_effectOnHitMod;
		object baseVal10;
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
			baseVal10 = exoDualCone.m_effectOnHit;
		}
		else
		{
			baseVal10 = null;
		}
		empty = str11 + PropDesc(effectOnHitMod, "[EffectOnHit]", flag, (StandardEffectInfo)baseVal10);
		empty += PropDesc(m_effectOnOverlapHitMod, "[EffectOnOverlapHit]", flag, (!flag) ? null : exoDualCone.m_effectOnOverlapHit);
		string str12 = empty;
		AbilityModPropertyInt extraDamageForConsecitiveHitMod = m_extraDamageForConsecitiveHitMod;
		int baseVal11;
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
			baseVal11 = exoDualCone.m_extraDamageForConsecutiveUse;
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str12 + PropDesc(extraDamageForConsecitiveHitMod, "[ExtraDamageForConsecitiveHit]", flag, baseVal11);
		return empty + PropDesc(m_extraEnergyForConsecutiveUseMod, "[ExtraEnergyForConsecutiveUse]", flag, flag ? exoDualCone.m_extraEnergyForConsecutiveUse : 0);
	}
}
