using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ArcherBendingArrow : AbilityMod
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

	public AbilityModPropertyEffectInfo m_laserHitEffectMod;

	public AbilityModPropertyInt m_extraDamageToHealingDebuffTarget;

	public AbilityModPropertyInt m_extraDamageAfterBend;

	public AbilityModPropertyInt m_damageAfterPiercingMod;

	public AbilityModPropertyEffectInfo m_effectToHealingDebuffTarget;

	public AbilityModPropertyInt m_extraHealingFromHealingDebuffTarget;

	[Header("-- Misc Ability Interactions")]
	public AbilityModPropertyInt m_nextShieldGeneratorExtraAbsorbPerHit;

	public AbilityModPropertyInt m_nextShieldGeneratorExtraAbsorbMax;

	public override Type GetTargetAbilityType()
	{
		return typeof(ArcherBendingArrow);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ArcherBendingArrow archerBendingArrow = targetAbility as ArcherBendingArrow;
		if (archerBendingArrow != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ArcherBendingArrow.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, archerBendingArrow.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_minRangeBeforeBendMod, "MinRangeBeforeBend", string.Empty, archerBendingArrow.m_minRangeBeforeBend, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxRangeBeforeBendMod, "MaxRangeBeforeBend", string.Empty, archerBendingArrow.m_maxRangeBeforeBend, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTotalRangeMod, "MaxTotalRange", string.Empty, archerBendingArrow.m_maxTotalRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxBendAngleMod, "MaxBendAngle", string.Empty, archerBendingArrow.m_maxBendAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, archerBendingArrow.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, archerBendingArrow.m_laserDamageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_laserHitEffectMod, "LaserHitEffect", archerBendingArrow.m_laserHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_nextShieldGeneratorExtraAbsorbPerHit, "NextShieldGeneratorExtraAbsorbPerHit", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_nextShieldGeneratorExtraAbsorbMax, "NextShieldGeneratorExtraAbsorbMax", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageToHealingDebuffTarget, "ExtraDamageToHealingDebuffTarget", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageAfterBend, "ExtraDamageAfterBend", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_damageAfterPiercingMod, "DamageToSubsequentTargetsAfterPiercing", string.Empty, archerBendingArrow.m_laserDamageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectToHealingDebuffTarget, "EffectToHealingDebuffTarget", null, true);
			AbilityMod.AddToken(tokens, this.m_extraHealingFromHealingDebuffTarget, "ExtraHealingFromHealingDebuffTarget", string.Empty, 0, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherBendingArrow archerBendingArrow = base.GetTargetAbilityOnAbilityData(abilityData) as ArcherBendingArrow;
		bool flag = archerBendingArrow != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : archerBendingArrow.m_laserWidth);
		string str = text;
		AbilityModPropertyFloat minRangeBeforeBendMod = this.m_minRangeBeforeBendMod;
		string prefix = "[MinRangeBeforeBend]";
		bool showBaseVal = flag;
		float baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ArcherBendingArrow.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = archerBendingArrow.m_minRangeBeforeBend;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(minRangeBeforeBendMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat maxRangeBeforeBendMod = this.m_maxRangeBeforeBendMod;
		string prefix2 = "[MaxRangeBeforeBend]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = archerBendingArrow.m_maxRangeBeforeBend;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(maxRangeBeforeBendMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat maxTotalRangeMod = this.m_maxTotalRangeMod;
		string prefix3 = "[MaxTotalRange]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = archerBendingArrow.m_maxTotalRange;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(maxTotalRangeMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat maxBendAngleMod = this.m_maxBendAngleMod;
		string prefix4 = "[MaxBendAngle]";
		bool showBaseVal4 = flag;
		float baseVal4;
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
			baseVal4 = archerBendingArrow.m_maxBendAngle;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(maxBendAngleMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool penetrateLoSMod = this.m_penetrateLoSMod;
		string prefix5 = "[PenetrateLoS]";
		bool showBaseVal5 = flag;
		bool baseVal5;
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
			baseVal5 = archerBendingArrow.m_penetrateLoS;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(penetrateLoSMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt maxTargetsMod = this.m_maxTargetsMod;
		string prefix6 = "[MaxTargets]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = archerBendingArrow.m_maxTargets;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(maxTargetsMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt laserDamageAmountMod = this.m_laserDamageAmountMod;
		string prefix7 = "[LaserDamageAmount]";
		bool showBaseVal7 = flag;
		int baseVal7;
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
			baseVal7 = archerBendingArrow.m_laserDamageAmount;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(laserDamageAmountMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyEffectInfo laserHitEffectMod = this.m_laserHitEffectMod;
		string prefix8 = "[LaserHitEffect]";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
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
			baseVal8 = archerBendingArrow.m_laserHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		text = str8 + base.PropDesc(laserHitEffectMod, prefix8, showBaseVal8, baseVal8);
		text += base.PropDesc(this.m_nextShieldGeneratorExtraAbsorbPerHit, "[NextShieldGeneratorExtraAbsorbPerHit]", flag, 0);
		text += base.PropDesc(this.m_nextShieldGeneratorExtraAbsorbMax, "[NextShieldGeneratorExtraAbsorbMax]", flag, 0);
		text += base.PropDesc(this.m_extraDamageToHealingDebuffTarget, "[ExtraDamageToHealingDebuffTarget]", flag, 0);
		text += base.PropDesc(this.m_extraDamageAfterBend, "[ExtraDamageAfterBend]", flag, 0);
		string str9 = text;
		AbilityModPropertyInt damageAfterPiercingMod = this.m_damageAfterPiercingMod;
		string prefix9 = "[DamageToSubsequentTargetsAfterPiercing]";
		bool showBaseVal9 = flag;
		int baseVal9;
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
			baseVal9 = archerBendingArrow.m_laserDamageAmount;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(damageAfterPiercingMod, prefix9, showBaseVal9, baseVal9);
		text += base.PropDesc(this.m_effectToHealingDebuffTarget, "[EffectToHealingDebuffTarget]", false, null);
		return text + base.PropDesc(this.m_extraHealingFromHealingDebuffTarget, "[ExtraHealingFromHealingDebuffTarget]", flag, 0);
	}
}
