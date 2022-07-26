using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RobotAnimalBite : AbilityMod
{
	[Header("-- Life Gain Mod")]
	public AbilityModPropertyFloat m_lifeOnFirstHitMod;
	public AbilityModPropertyFloat m_lifePerHitMod;
	[Header("-- Damage and Hit Effect Mod")]
	public AbilityModPropertyInt m_damageMod;
	public StandardEffectInfo m_effectOnEnemyOverride;
	[Header("-- Extra Damage on Consecutive Use")]
	public int m_extraDamageOnConsecutiveCast;
	public int m_extraDamageOnConsecutiveHit;
	[Header("-- Effect for Next Turn for Self, per adjacent enemy")]
	public StandardEffectInfo m_perAdjacentEnemyEffectOnSelfNextTurn;
	[Header("-- Minimum Variance Extra Damage")]
	public int m_varianceExtraDamageMin;
	[Header("-- Maximum Variance Extra Damage")]
	public int m_varianceExtraDamageMax;
	[Header("-- Portion of Variance Extra Damage to Apply to Self Also, 0-1")]
	public float m_varianceExtraDamageToSelf;

	public override Type GetTargetAbilityType()
	{
		return typeof(RobotAnimalBite);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RobotAnimalBite robotAnimalBite = targetAbility as RobotAnimalBite;
		if (robotAnimalBite != null)
		{
			AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, robotAnimalBite.m_damageAmount);
			AddToken(tokens, m_lifeOnFirstHitMod, "LifeOnFirstHit", string.Empty, robotAnimalBite.m_lifeOnFirstHit);
			AddToken(tokens, m_lifePerHitMod, "LifePerHit", string.Empty, robotAnimalBite.m_lifePerHit);
			if (m_extraDamageOnConsecutiveCast > 0)
			{
				AddToken_IntDiff(tokens, "ExtraDamage_ConsecutiveCast", string.Empty, m_extraDamageOnConsecutiveCast, false, 0);
			}
			if (m_extraDamageOnConsecutiveHit > 0)
			{
				AddToken_IntDiff(tokens, "ExtraDamage_ConsecutiveHit", string.Empty, m_extraDamageOnConsecutiveHit, false, 0);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalBite robotAnimalBite = GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalBite;
		bool isAbilityPresent = robotAnimalBite != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_lifeOnFirstHitMod, "[Life On First Hit]", isAbilityPresent, isAbilityPresent ? robotAnimalBite.m_lifeOnFirstHit : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_lifePerHitMod, "[Life Per Hit Mod]", isAbilityPresent, isAbilityPresent ? robotAnimalBite.m_lifePerHit : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? robotAnimalBite.m_damageAmount : 0);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnEnemyOverride, "{ Effect on Enemy }", string.Empty, isAbilityPresent);
		if (m_extraDamageOnConsecutiveCast > 0)
		{
			desc += "[Extra Damage on Consecutive Casts] = " + m_extraDamageOnConsecutiveCast + "\n";
		}
		if (m_extraDamageOnConsecutiveHit > 0)
		{
			desc += "[Extra Damage on Consecutive Hit] = " + m_extraDamageOnConsecutiveHit + "\n";
		}
		if (m_perAdjacentEnemyEffectOnSelfNextTurn != null && m_perAdjacentEnemyEffectOnSelfNextTurn.m_applyEffect)
		{
			desc += AbilityModHelper.GetModEffectInfoDesc(m_perAdjacentEnemyEffectOnSelfNextTurn, "[Additional Effect Next Turn On Self, Per Adjacent Enemy]");
		}
		if (m_varianceExtraDamageMin >= 0 && m_varianceExtraDamageMax - m_varianceExtraDamageMin > 0)
		{
			desc += "[Variance Extra Damage] = " + m_varianceExtraDamageMin + " - " + m_varianceExtraDamageMax + "\n";
			if (m_varianceExtraDamageToSelf > Mathf.Epsilon)
			{
				desc += "[Variance Extra Damage to Self] = " + Mathf.RoundToInt(m_varianceExtraDamageMin * m_varianceExtraDamageToSelf) + " - " + Mathf.RoundToInt(m_varianceExtraDamageMax * m_varianceExtraDamageToSelf) + "\n";
			}
		}
		return desc;
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		RobotAnimalBite robotAnimalBite = abilityAsBase as RobotAnimalBite;
		if (robotAnimalBite != null)
		{
			numbers.Add(m_damageMod.GetModifiedValue(robotAnimalBite.m_damageAmount) + m_extraDamageOnConsecutiveCast);
			numbers.Add(m_damageMod.GetModifiedValue(robotAnimalBite.m_damageAmount) + m_extraDamageOnConsecutiveHit);
		}
	}
}
