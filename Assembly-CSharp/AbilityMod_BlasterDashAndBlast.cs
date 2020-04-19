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
			AbilityMod.AddToken(tokens, this.m_minLengthMod, "MinLength", string.Empty, blasterDashAndBlast.m_minLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxLengthMod, "MaxLength", string.Empty, blasterDashAndBlast.m_maxLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_minAngleMod, "MinAngle", string.Empty, blasterDashAndBlast.m_minAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxAngleMod, "MaxAngle", string.Empty, blasterDashAndBlast.m_maxAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, blasterDashAndBlast.m_coneBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_distancePerStockMod, "DistancePerStock", string.Empty, blasterDashAndBlast.m_distancePerStock, true, false, false);
			AbilityMod.AddToken(tokens, this.m_stockBasedDistMaxSquareCoordDistMod, "StockBasedDistMaxSquareCoordDist", string.Empty, blasterDashAndBlast.m_stockBasedDistMaxSquareCoordDist, true, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountNormalMod, "DamageAmountNormal", string.Empty, blasterDashAndBlast.m_damageAmountNormal, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, blasterDashAndBlast.m_extraDamageForSingleHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyEffectNormalMod, "EnemyEffectNormal", blasterDashAndBlast.m_enemyEffectNormal, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyEffectOverchargedMod, "EnemyEffectOvercharged", blasterDashAndBlast.m_enemyEffectOvercharged, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfEffectOnCastMod, "SelfEffectOnCast", blasterDashAndBlast.m_selfEffectOnCast, true);
			if (this.m_useTargetDataOverrides && this.m_targetDataOverrides.Length > 0)
			{
				AbilityMod.AddToken_IntDiff(tokens, "MaxDashRange", string.Empty, Mathf.FloorToInt(this.m_targetDataOverrides[0].m_range), false, 0);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterDashAndBlast blasterDashAndBlast = base.GetTargetAbilityOnAbilityData(abilityData) as BlasterDashAndBlast;
		bool flag = blasterDashAndBlast != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_minLengthMod, "[MinLength]", flag, (!flag) ? 0f : blasterDashAndBlast.m_minLength);
		string str = text;
		AbilityModPropertyFloat maxLengthMod = this.m_maxLengthMod;
		string prefix = "[MaxLength]";
		bool showBaseVal = flag;
		float baseVal;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BlasterDashAndBlast.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = blasterDashAndBlast.m_maxLength;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(maxLengthMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat minAngleMod = this.m_minAngleMod;
		string prefix2 = "[MinAngle]";
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
			baseVal2 = blasterDashAndBlast.m_minAngle;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(minAngleMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat maxAngleMod = this.m_maxAngleMod;
		string prefix3 = "[MaxAngle]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = blasterDashAndBlast.m_maxAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(maxAngleMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, (!flag) ? 0f : blasterDashAndBlast.m_coneBackwardOffset);
		string str4 = text;
		AbilityModPropertyBool penetrateLineOfSightMod = this.m_penetrateLineOfSightMod;
		string prefix4 = "[PenetrateLineOfSight]";
		bool showBaseVal4 = flag;
		bool baseVal4;
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
			baseVal4 = blasterDashAndBlast.m_penetrateLineOfSight;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(penetrateLineOfSightMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_useStockBasedEvadeDistanceMod, "[UseStockBasedEvadeDistance]", flag, flag && blasterDashAndBlast.m_useStockBasedEvadeDistance);
		string str5 = text;
		AbilityModPropertyFloat distancePerStockMod = this.m_distancePerStockMod;
		string prefix5 = "[DistancePerStock]";
		bool showBaseVal5 = flag;
		float baseVal5;
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
			baseVal5 = blasterDashAndBlast.m_distancePerStock;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + base.PropDesc(distancePerStockMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyBool stockBasedDistUseSquareCoordDistMod = this.m_stockBasedDistUseSquareCoordDistMod;
		string prefix6 = "[StockBasedDistUseSquareCoordDist]";
		bool showBaseVal6 = flag;
		bool baseVal6;
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
			baseVal6 = blasterDashAndBlast.m_stockBasedDistUseSquareCoordDist;
		}
		else
		{
			baseVal6 = false;
		}
		text = str6 + base.PropDesc(stockBasedDistUseSquareCoordDistMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt stockBasedDistMaxSquareCoordDistMod = this.m_stockBasedDistMaxSquareCoordDistMod;
		string prefix7 = "[StockBasedDistMaxSquareCoordDist]";
		bool showBaseVal7 = flag;
		int baseVal7;
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
			baseVal7 = blasterDashAndBlast.m_stockBasedDistMaxSquareCoordDist;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(stockBasedDistMaxSquareCoordDistMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt damageAmountNormalMod = this.m_damageAmountNormalMod;
		string prefix8 = "[DamageAmountNormal]";
		bool showBaseVal8 = flag;
		int baseVal8;
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
			baseVal8 = blasterDashAndBlast.m_damageAmountNormal;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(damageAmountNormalMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt extraDamageForSingleHitMod = this.m_extraDamageForSingleHitMod;
		string prefix9 = "[ExtraDamageForSingleHit]";
		bool showBaseVal9 = flag;
		int baseVal9;
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
			baseVal9 = blasterDashAndBlast.m_extraDamageForSingleHit;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(extraDamageForSingleHitMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyEffectInfo enemyEffectNormalMod = this.m_enemyEffectNormalMod;
		string prefix10 = "[EnemyEffectNormal]";
		bool showBaseVal10 = flag;
		StandardEffectInfo baseVal10;
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
			baseVal10 = blasterDashAndBlast.m_enemyEffectNormal;
		}
		else
		{
			baseVal10 = null;
		}
		text = str10 + base.PropDesc(enemyEffectNormalMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyEffectInfo enemyEffectOverchargedMod = this.m_enemyEffectOverchargedMod;
		string prefix11 = "[EnemyEffectOvercharged]";
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
			baseVal11 = blasterDashAndBlast.m_enemyEffectOvercharged;
		}
		else
		{
			baseVal11 = null;
		}
		text = str11 + base.PropDesc(enemyEffectOverchargedMod, prefix11, showBaseVal11, baseVal11);
		return text + base.PropDesc(this.m_selfEffectOnCastMod, "[SelfEffectOnCast]", flag, (!flag) ? null : blasterDashAndBlast.m_selfEffectOnCast);
	}
}
