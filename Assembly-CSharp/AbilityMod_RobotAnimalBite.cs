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
		if (!(robotAnimalBite != null))
		{
			return;
		}
		AbilityMod.AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, robotAnimalBite.m_damageAmount);
		AbilityMod.AddToken(tokens, m_lifeOnFirstHitMod, "LifeOnFirstHit", string.Empty, robotAnimalBite.m_lifeOnFirstHit);
		AbilityMod.AddToken(tokens, m_lifePerHitMod, "LifePerHit", string.Empty, robotAnimalBite.m_lifePerHit);
		if (m_extraDamageOnConsecutiveCast > 0)
		{
			AbilityMod.AddToken_IntDiff(tokens, "ExtraDamage_ConsecutiveCast", string.Empty, m_extraDamageOnConsecutiveCast, false, 0);
		}
		if (m_extraDamageOnConsecutiveHit > 0)
		{
			AbilityMod.AddToken_IntDiff(tokens, "ExtraDamage_ConsecutiveHit", string.Empty, m_extraDamageOnConsecutiveHit, false, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalBite robotAnimalBite = GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalBite;
		bool flag = robotAnimalBite != null;
		string empty = string.Empty;
		empty += AbilityModHelper.GetModPropertyDesc(m_lifeOnFirstHitMod, "[Life On First Hit]", flag, (!flag) ? 0f : robotAnimalBite.m_lifeOnFirstHit);
		string str = empty;
		AbilityModPropertyFloat lifePerHitMod = m_lifePerHitMod;
		float baseVal;
		if (flag)
		{
			baseVal = robotAnimalBite.m_lifePerHit;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(lifePerHitMod, "[Life Per Hit Mod]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = robotAnimalBite.m_damageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(damageMod, "[Damage]", flag, baseVal2);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_effectOnEnemyOverride, "{ Effect on Enemy }", string.Empty, flag);
		if (m_extraDamageOnConsecutiveCast > 0)
		{
			string text = empty;
			empty = text + "[Extra Damage on Consecutive Casts] = " + m_extraDamageOnConsecutiveCast + "\n";
		}
		if (m_extraDamageOnConsecutiveHit > 0)
		{
			string text = empty;
			empty = text + "[Extra Damage on Consecutive Hit] = " + m_extraDamageOnConsecutiveHit + "\n";
		}
		if (m_perAdjacentEnemyEffectOnSelfNextTurn != null && m_perAdjacentEnemyEffectOnSelfNextTurn.m_applyEffect)
		{
			empty += AbilityModHelper.GetModEffectInfoDesc(m_perAdjacentEnemyEffectOnSelfNextTurn, "[Additional Effect Next Turn On Self, Per Adjacent Enemy]", string.Empty);
		}
		if (m_varianceExtraDamageMin >= 0 && m_varianceExtraDamageMax - m_varianceExtraDamageMin > 0)
		{
			string text = empty;
			empty = text + "[Variance Extra Damage] = " + m_varianceExtraDamageMin + " - " + m_varianceExtraDamageMax + "\n";
			if (m_varianceExtraDamageToSelf > Mathf.Epsilon)
			{
				text = empty;
				empty = text + "[Variance Extra Damage to Self] = " + Mathf.RoundToInt((float)m_varianceExtraDamageMin * m_varianceExtraDamageToSelf) + " - " + Mathf.RoundToInt((float)m_varianceExtraDamageMax * m_varianceExtraDamageToSelf) + "\n";
			}
		}
		return empty;
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		RobotAnimalBite robotAnimalBite = abilityAsBase as RobotAnimalBite;
		if (!(robotAnimalBite != null))
		{
			return;
		}
		while (true)
		{
			numbers.Add(m_damageMod.GetModifiedValue(robotAnimalBite.m_damageAmount) + m_extraDamageOnConsecutiveCast);
			numbers.Add(m_damageMod.GetModifiedValue(robotAnimalBite.m_damageAmount) + m_extraDamageOnConsecutiveHit);
			return;
		}
	}
}
