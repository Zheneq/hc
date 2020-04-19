using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SamuraiWindBlade : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_minRangeBeforeBendMod;

	public AbilityModPropertyFloat m_maxRangeBeforeBendMod;

	public AbilityModPropertyFloat m_maxTotalRangeMod;

	public AbilityModPropertyFloat m_maxBendAngleMod;

	public AbilityModPropertyBool m_penetrateLoSMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Header("-- Damage")]
	public AbilityModPropertyInt m_laserDamageAmountMod;

	public AbilityModPropertyInt m_damageChangePerTargetMod;

	public AbilityModPropertyEffectInfo m_laserHitEffectMod;

	[Header("-- Shielding per enemy hit on start of Next Turn")]
	public AbilityModPropertyInt m_shieldingPerEnemyHitNextTurnMod;

	public AbilityModPropertyInt m_shieldingDurationMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SamuraiWindBlade);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SamuraiWindBlade samuraiWindBlade = targetAbility as SamuraiWindBlade;
		if (samuraiWindBlade != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SamuraiWindBlade.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, samuraiWindBlade.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_minRangeBeforeBendMod, "MinRangeBeforeBend", string.Empty, samuraiWindBlade.m_minRangeBeforeBend, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxRangeBeforeBendMod, "MaxRangeBeforeBend", string.Empty, samuraiWindBlade.m_maxRangeBeforeBend, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTotalRangeMod, "MaxTotalRange", string.Empty, samuraiWindBlade.m_maxTotalRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxBendAngleMod, "MaxBendAngle", string.Empty, samuraiWindBlade.m_maxBendAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, samuraiWindBlade.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, samuraiWindBlade.m_laserDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_damageChangePerTargetMod, "DamageChangePerTarget", string.Empty, samuraiWindBlade.m_damageChangePerTarget, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_laserHitEffectMod, "LaserHitEffect", samuraiWindBlade.m_laserHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_shieldingPerEnemyHitNextTurnMod, "ShieldingPerEnemyHitNextTurn", string.Empty, samuraiWindBlade.m_shieldingPerEnemyHitNextTurn, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldingDurationMod, "ShieldingDuration", string.Empty, samuraiWindBlade.m_shieldingDuration, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiWindBlade samuraiWindBlade = base.GetTargetAbilityOnAbilityData(abilityData) as SamuraiWindBlade;
		bool flag = samuraiWindBlade != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : samuraiWindBlade.m_laserWidth);
		text += base.PropDesc(this.m_minRangeBeforeBendMod, "[MinRangeBeforeBend]", flag, (!flag) ? 0f : samuraiWindBlade.m_minRangeBeforeBend);
		string str = text;
		AbilityModPropertyFloat maxRangeBeforeBendMod = this.m_maxRangeBeforeBendMod;
		string prefix = "[MaxRangeBeforeBend]";
		bool showBaseVal = flag;
		float baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SamuraiWindBlade.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = samuraiWindBlade.m_maxRangeBeforeBend;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(maxRangeBeforeBendMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat maxTotalRangeMod = this.m_maxTotalRangeMod;
		string prefix2 = "[MaxTotalRange]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = samuraiWindBlade.m_maxTotalRange;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(maxTotalRangeMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat maxBendAngleMod = this.m_maxBendAngleMod;
		string prefix3 = "[MaxBendAngle]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = samuraiWindBlade.m_maxBendAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(maxBendAngleMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool penetrateLoSMod = this.m_penetrateLoSMod;
		string prefix4 = "[PenetrateLoS]";
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
			baseVal4 = samuraiWindBlade.m_penetrateLoS;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(penetrateLoSMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_maxTargetsMod, "[MaxTargets]", flag, (!flag) ? 0 : samuraiWindBlade.m_maxTargets);
		text += base.PropDesc(this.m_laserDamageAmountMod, "[LaserDamageAmount]", flag, (!flag) ? 0 : samuraiWindBlade.m_laserDamageAmount);
		string str5 = text;
		AbilityModPropertyInt damageChangePerTargetMod = this.m_damageChangePerTargetMod;
		string prefix5 = "[DamageChangePerTarget]";
		bool showBaseVal5 = flag;
		int baseVal5;
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
			baseVal5 = samuraiWindBlade.m_damageChangePerTarget;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(damageChangePerTargetMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo laserHitEffectMod = this.m_laserHitEffectMod;
		string prefix6 = "[LaserHitEffect]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
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
			baseVal6 = samuraiWindBlade.m_laserHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(laserHitEffectMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt shieldingPerEnemyHitNextTurnMod = this.m_shieldingPerEnemyHitNextTurnMod;
		string prefix7 = "[ShieldingPerEnemyHitNextTurn]";
		bool showBaseVal7 = flag;
		int baseVal7;
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
			baseVal7 = samuraiWindBlade.m_shieldingPerEnemyHitNextTurn;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(shieldingPerEnemyHitNextTurnMod, prefix7, showBaseVal7, baseVal7);
		return text + base.PropDesc(this.m_shieldingDurationMod, "[ShieldingDuration]", flag, (!flag) ? 0 : samuraiWindBlade.m_shieldingDuration);
	}
}
