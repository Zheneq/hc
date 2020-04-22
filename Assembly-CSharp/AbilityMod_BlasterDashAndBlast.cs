using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BlasterDashAndBlast : AbilityMod
{
	[Header("-- Cone Limits")]
	public AbilityModPropertyFloat m_minLengthMod;

	public AbilityModPropertyFloat m_maxLengthMod;

	public AbilityModPropertyFloat m_minAngleMod;

	public AbilityModPropertyFloat m_maxAngleMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	[Header("-- Stock based Evade distance")]
	public AbilityModPropertyBool m_useStockBasedEvadeDistanceMod;

	public AbilityModPropertyFloat m_distancePerStockMod;

	[Header("-- Whether to use square coordinate distance to limit stock-based evade distance")]
	public AbilityModPropertyBool m_stockBasedDistUseSquareCoordDistMod;

	[Header("-- If <= 0, dist only limited by stock remaining")]
	public AbilityModPropertyInt m_stockBasedDistMaxSquareCoordDistMod;

	[Header("-- On Hit")]
	public AbilityModPropertyInt m_damageAmountNormalMod;

	public AbilityModPropertyInt m_extraDamageForSingleHitMod;

	[Space(10f)]
	public AbilityModPropertyEffectInfo m_enemyEffectNormalMod;

	public AbilityModPropertyEffectInfo m_enemyEffectOverchargedMod;

	[Space(10f)]
	public AbilityModPropertyEffectInfo m_selfEffectOnCastMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(BlasterDashAndBlast);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BlasterDashAndBlast blasterDashAndBlast = targetAbility as BlasterDashAndBlast;
		if (blasterDashAndBlast != null)
		{
			AbilityMod.AddToken(tokens, m_minLengthMod, "MinLength", string.Empty, blasterDashAndBlast.m_minLength);
			AbilityMod.AddToken(tokens, m_maxLengthMod, "MaxLength", string.Empty, blasterDashAndBlast.m_maxLength);
			AbilityMod.AddToken(tokens, m_minAngleMod, "MinAngle", string.Empty, blasterDashAndBlast.m_minAngle);
			AbilityMod.AddToken(tokens, m_maxAngleMod, "MaxAngle", string.Empty, blasterDashAndBlast.m_maxAngle);
			AbilityMod.AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, blasterDashAndBlast.m_coneBackwardOffset);
			AbilityMod.AddToken(tokens, m_distancePerStockMod, "DistancePerStock", string.Empty, blasterDashAndBlast.m_distancePerStock);
			AbilityMod.AddToken(tokens, m_stockBasedDistMaxSquareCoordDistMod, "StockBasedDistMaxSquareCoordDist", string.Empty, blasterDashAndBlast.m_stockBasedDistMaxSquareCoordDist);
			AbilityMod.AddToken(tokens, m_damageAmountNormalMod, "DamageAmountNormal", string.Empty, blasterDashAndBlast.m_damageAmountNormal);
			AbilityMod.AddToken(tokens, m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, blasterDashAndBlast.m_extraDamageForSingleHit);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyEffectNormalMod, "EnemyEffectNormal", blasterDashAndBlast.m_enemyEffectNormal);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyEffectOverchargedMod, "EnemyEffectOvercharged", blasterDashAndBlast.m_enemyEffectOvercharged);
			AbilityMod.AddToken_EffectMod(tokens, m_selfEffectOnCastMod, "SelfEffectOnCast", blasterDashAndBlast.m_selfEffectOnCast);
			if (m_useTargetDataOverrides && m_targetDataOverrides.Length > 0)
			{
				AbilityMod.AddToken_IntDiff(tokens, "MaxDashRange", string.Empty, Mathf.FloorToInt(m_targetDataOverrides[0].m_range), false, 0);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterDashAndBlast blasterDashAndBlast = GetTargetAbilityOnAbilityData(abilityData) as BlasterDashAndBlast;
		bool flag = blasterDashAndBlast != null;
		string empty = string.Empty;
		empty += PropDesc(m_minLengthMod, "[MinLength]", flag, (!flag) ? 0f : blasterDashAndBlast.m_minLength);
		string str = empty;
		AbilityModPropertyFloat maxLengthMod = m_maxLengthMod;
		float baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = blasterDashAndBlast.m_maxLength;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(maxLengthMod, "[MaxLength]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat minAngleMod = m_minAngleMod;
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
			baseVal2 = blasterDashAndBlast.m_minAngle;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(minAngleMod, "[MinAngle]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat maxAngleMod = m_maxAngleMod;
		float baseVal3;
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
			baseVal3 = blasterDashAndBlast.m_maxAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(maxAngleMod, "[MaxAngle]", flag, baseVal3);
		empty += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, (!flag) ? 0f : blasterDashAndBlast.m_coneBackwardOffset);
		string str4 = empty;
		AbilityModPropertyBool penetrateLineOfSightMod = m_penetrateLineOfSightMod;
		int baseVal4;
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
			baseVal4 = (blasterDashAndBlast.m_penetrateLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, (byte)baseVal4 != 0);
		empty += PropDesc(m_useStockBasedEvadeDistanceMod, "[UseStockBasedEvadeDistance]", flag, flag && blasterDashAndBlast.m_useStockBasedEvadeDistance);
		string str5 = empty;
		AbilityModPropertyFloat distancePerStockMod = m_distancePerStockMod;
		float baseVal5;
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
			baseVal5 = blasterDashAndBlast.m_distancePerStock;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + PropDesc(distancePerStockMod, "[DistancePerStock]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyBool stockBasedDistUseSquareCoordDistMod = m_stockBasedDistUseSquareCoordDistMod;
		int baseVal6;
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
			baseVal6 = (blasterDashAndBlast.m_stockBasedDistUseSquareCoordDist ? 1 : 0);
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(stockBasedDistUseSquareCoordDistMod, "[StockBasedDistUseSquareCoordDist]", flag, (byte)baseVal6 != 0);
		string str7 = empty;
		AbilityModPropertyInt stockBasedDistMaxSquareCoordDistMod = m_stockBasedDistMaxSquareCoordDistMod;
		int baseVal7;
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
			baseVal7 = blasterDashAndBlast.m_stockBasedDistMaxSquareCoordDist;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(stockBasedDistMaxSquareCoordDistMod, "[StockBasedDistMaxSquareCoordDist]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyInt damageAmountNormalMod = m_damageAmountNormalMod;
		int baseVal8;
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
			baseVal8 = blasterDashAndBlast.m_damageAmountNormal;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(damageAmountNormalMod, "[DamageAmountNormal]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyInt extraDamageForSingleHitMod = m_extraDamageForSingleHitMod;
		int baseVal9;
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
			baseVal9 = blasterDashAndBlast.m_extraDamageForSingleHit;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(extraDamageForSingleHitMod, "[ExtraDamageForSingleHit]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyEffectInfo enemyEffectNormalMod = m_enemyEffectNormalMod;
		object baseVal10;
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
			baseVal10 = blasterDashAndBlast.m_enemyEffectNormal;
		}
		else
		{
			baseVal10 = null;
		}
		empty = str10 + PropDesc(enemyEffectNormalMod, "[EnemyEffectNormal]", flag, (StandardEffectInfo)baseVal10);
		string str11 = empty;
		AbilityModPropertyEffectInfo enemyEffectOverchargedMod = m_enemyEffectOverchargedMod;
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
			baseVal11 = blasterDashAndBlast.m_enemyEffectOvercharged;
		}
		else
		{
			baseVal11 = null;
		}
		empty = str11 + PropDesc(enemyEffectOverchargedMod, "[EnemyEffectOvercharged]", flag, (StandardEffectInfo)baseVal11);
		return empty + PropDesc(m_selfEffectOnCastMod, "[SelfEffectOnCast]", flag, (!flag) ? null : blasterDashAndBlast.m_selfEffectOnCast);
	}
}
