using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken(tokens, m_minLengthMod, "MinLength", string.Empty, blasterDashAndBlast.m_minLength);
			AddToken(tokens, m_maxLengthMod, "MaxLength", string.Empty, blasterDashAndBlast.m_maxLength);
			AddToken(tokens, m_minAngleMod, "MinAngle", string.Empty, blasterDashAndBlast.m_minAngle);
			AddToken(tokens, m_maxAngleMod, "MaxAngle", string.Empty, blasterDashAndBlast.m_maxAngle);
			AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, blasterDashAndBlast.m_coneBackwardOffset);
			AddToken(tokens, m_distancePerStockMod, "DistancePerStock", string.Empty, blasterDashAndBlast.m_distancePerStock);
			AddToken(tokens, m_stockBasedDistMaxSquareCoordDistMod, "StockBasedDistMaxSquareCoordDist", string.Empty, blasterDashAndBlast.m_stockBasedDistMaxSquareCoordDist);
			AddToken(tokens, m_damageAmountNormalMod, "DamageAmountNormal", string.Empty, blasterDashAndBlast.m_damageAmountNormal);
			AddToken(tokens, m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, blasterDashAndBlast.m_extraDamageForSingleHit);
			AddToken_EffectMod(tokens, m_enemyEffectNormalMod, "EnemyEffectNormal", blasterDashAndBlast.m_enemyEffectNormal);
			AddToken_EffectMod(tokens, m_enemyEffectOverchargedMod, "EnemyEffectOvercharged", blasterDashAndBlast.m_enemyEffectOvercharged);
			AddToken_EffectMod(tokens, m_selfEffectOnCastMod, "SelfEffectOnCast", blasterDashAndBlast.m_selfEffectOnCast);
			if (m_useTargetDataOverrides && m_targetDataOverrides.Length > 0)
			{
				AddToken_IntDiff(tokens, "MaxDashRange", string.Empty, Mathf.FloorToInt(m_targetDataOverrides[0].m_range), false, 0);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterDashAndBlast blasterDashAndBlast = GetTargetAbilityOnAbilityData(abilityData) as BlasterDashAndBlast;
		bool isAbilityPresent = blasterDashAndBlast != null;
		string desc = string.Empty;
		desc += PropDesc(m_minLengthMod, "[MinLength]", isAbilityPresent, isAbilityPresent ? blasterDashAndBlast.m_minLength : 0f);
		desc += PropDesc(m_maxLengthMod, "[MaxLength]", isAbilityPresent, isAbilityPresent ? blasterDashAndBlast.m_maxLength : 0f);
		desc += PropDesc(m_minAngleMod, "[MinAngle]", isAbilityPresent, isAbilityPresent ? blasterDashAndBlast.m_minAngle : 0f);
		desc += PropDesc(m_maxAngleMod, "[MaxAngle]", isAbilityPresent, isAbilityPresent ? blasterDashAndBlast.m_maxAngle : 0f);
		desc += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", isAbilityPresent, isAbilityPresent ? blasterDashAndBlast.m_coneBackwardOffset : 0f);
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isAbilityPresent, isAbilityPresent && blasterDashAndBlast.m_penetrateLineOfSight);
		desc += PropDesc(m_useStockBasedEvadeDistanceMod, "[UseStockBasedEvadeDistance]", isAbilityPresent, isAbilityPresent && blasterDashAndBlast.m_useStockBasedEvadeDistance);
		desc += PropDesc(m_distancePerStockMod, "[DistancePerStock]", isAbilityPresent, isAbilityPresent ? blasterDashAndBlast.m_distancePerStock : 0f);
		desc += PropDesc(m_stockBasedDistUseSquareCoordDistMod, "[StockBasedDistUseSquareCoordDist]", isAbilityPresent, isAbilityPresent && blasterDashAndBlast.m_stockBasedDistUseSquareCoordDist);
		desc += PropDesc(m_stockBasedDistMaxSquareCoordDistMod, "[StockBasedDistMaxSquareCoordDist]", isAbilityPresent, isAbilityPresent ? blasterDashAndBlast.m_stockBasedDistMaxSquareCoordDist : 0);
		desc += PropDesc(m_damageAmountNormalMod, "[DamageAmountNormal]", isAbilityPresent, isAbilityPresent ? blasterDashAndBlast.m_damageAmountNormal : 0);
		desc += PropDesc(m_extraDamageForSingleHitMod, "[ExtraDamageForSingleHit]", isAbilityPresent, isAbilityPresent ? blasterDashAndBlast.m_extraDamageForSingleHit : 0);
		desc += PropDesc(m_enemyEffectNormalMod, "[EnemyEffectNormal]", isAbilityPresent, isAbilityPresent ? blasterDashAndBlast.m_enemyEffectNormal : null);
		desc += PropDesc(m_enemyEffectOverchargedMod, "[EnemyEffectOvercharged]", isAbilityPresent, isAbilityPresent ? blasterDashAndBlast.m_enemyEffectOvercharged : null);
		return new StringBuilder().Append(desc).Append(PropDesc(m_selfEffectOnCastMod, "[SelfEffectOnCast]", isAbilityPresent, isAbilityPresent ? blasterDashAndBlast.m_selfEffectOnCast : null)).ToString();
	}
}
