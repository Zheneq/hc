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
			AbilityMod.AddToken(tokens, this.m_damageMod, "DamageAmount", string.Empty, robotAnimalBite.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_lifeOnFirstHitMod, "LifeOnFirstHit", string.Empty, robotAnimalBite.m_lifeOnFirstHit, true, false, false);
			AbilityMod.AddToken(tokens, this.m_lifePerHitMod, "LifePerHit", string.Empty, robotAnimalBite.m_lifePerHit, true, false, false);
			if (this.m_extraDamageOnConsecutiveCast > 0)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RobotAnimalBite.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
				}
				AbilityMod.AddToken_IntDiff(tokens, "ExtraDamage_ConsecutiveCast", string.Empty, this.m_extraDamageOnConsecutiveCast, false, 0);
			}
			if (this.m_extraDamageOnConsecutiveHit > 0)
			{
				AbilityMod.AddToken_IntDiff(tokens, "ExtraDamage_ConsecutiveHit", string.Empty, this.m_extraDamageOnConsecutiveHit, false, 0);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalBite robotAnimalBite = base.GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalBite;
		bool flag = robotAnimalBite != null;
		string text = string.Empty;
		text += AbilityModHelper.GetModPropertyDesc(this.m_lifeOnFirstHitMod, "[Life On First Hit]", flag, (!flag) ? 0f : robotAnimalBite.m_lifeOnFirstHit);
		string str = text;
		AbilityModPropertyFloat lifePerHitMod = this.m_lifePerHitMod;
		string prefix = "[Life Per Hit Mod]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RobotAnimalBite.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = robotAnimalBite.m_lifePerHit;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(lifePerHitMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix2 = "[Damage]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = robotAnimalBite.m_damageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(damageMod, prefix2, showBaseVal2, baseVal2);
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnEnemyOverride, "{ Effect on Enemy }", string.Empty, flag, null);
		if (this.m_extraDamageOnConsecutiveCast > 0)
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
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Extra Damage on Consecutive Casts] = ",
				this.m_extraDamageOnConsecutiveCast,
				"\n"
			});
		}
		if (this.m_extraDamageOnConsecutiveHit > 0)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Extra Damage on Consecutive Hit] = ",
				this.m_extraDamageOnConsecutiveHit,
				"\n"
			});
		}
		if (this.m_perAdjacentEnemyEffectOnSelfNextTurn != null && this.m_perAdjacentEnemyEffectOnSelfNextTurn.m_applyEffect)
		{
			text += AbilityModHelper.GetModEffectInfoDesc(this.m_perAdjacentEnemyEffectOnSelfNextTurn, "[Additional Effect Next Turn On Self, Per Adjacent Enemy]", string.Empty, false, null);
		}
		if (this.m_varianceExtraDamageMin >= 0 && this.m_varianceExtraDamageMax - this.m_varianceExtraDamageMin > 0)
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
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Variance Extra Damage] = ",
				this.m_varianceExtraDamageMin,
				" - ",
				this.m_varianceExtraDamageMax,
				"\n"
			});
			if (this.m_varianceExtraDamageToSelf > Mathf.Epsilon)
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
				text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"[Variance Extra Damage to Self] = ",
					Mathf.RoundToInt((float)this.m_varianceExtraDamageMin * this.m_varianceExtraDamageToSelf),
					" - ",
					Mathf.RoundToInt((float)this.m_varianceExtraDamageMax * this.m_varianceExtraDamageToSelf),
					"\n"
				});
			}
		}
		return text;
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		RobotAnimalBite robotAnimalBite = abilityAsBase as RobotAnimalBite;
		if (robotAnimalBite != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RobotAnimalBite.AppendModSpecificTooltipCheckNumbers(Ability, List<int>)).MethodHandle;
			}
			numbers.Add(this.m_damageMod.GetModifiedValue(robotAnimalBite.m_damageAmount) + this.m_extraDamageOnConsecutiveCast);
			numbers.Add(this.m_damageMod.GetModifiedValue(robotAnimalBite.m_damageAmount) + this.m_extraDamageOnConsecutiveHit);
		}
	}
}
