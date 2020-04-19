using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SenseiHealAoE : AbilityMod
{
	[Separator("Targeting Info", true)]
	public AbilityModPropertyFloat m_circleRadiusMod;

	public AbilityModPropertyBool m_penetrateLoSMod;

	[Space(10f)]
	public AbilityModPropertyBool m_includeSelfMod;

	[Separator("Self Hit", true)]
	public AbilityModPropertyInt m_selfHealMod;

	[Space(10f)]
	public AbilityModPropertyFloat m_selfLowHealthThreshMod;

	public AbilityModPropertyInt m_extraSelfHealForLowHealthMod;

	[Separator("Ally Hit", true)]
	public AbilityModPropertyInt m_allyHealMod;

	public AbilityModPropertyInt m_extraAllyHealIfSingleHitMod;

	[Space(10f)]
	public AbilityModPropertyInt m_extraHealForAdjacentMod;

	public AbilityModPropertyFloat m_healChangeStartDistMod;

	public AbilityModPropertyFloat m_healChangePerDistMod;

	public AbilityModPropertyEffectInfo m_allyHitEffectMod;

	[Header("-- Extra Ally Heal for low health")]
	public AbilityModPropertyFloat m_allyLowHealthThreshMod;

	public AbilityModPropertyInt m_extraAllyHealForLowHealthMod;

	[Space(10f)]
	public AbilityModPropertyInt m_allyEnergyGainMod;

	[Header("-- Cooldown Reduction for damaging hits")]
	public AbilityModPropertyInt m_cdrForAnyDamageMod;

	public AbilityModPropertyInt m_cdrForDamagePerUniqueAbilityMod;

	[Separator("For trigger on Subsequent Turns", true)]
	public AbilityModPropertyInt m_turnsAfterInitialCastMod;

	public AbilityModPropertyInt m_allyHealOnSubsequentTurnsMod;

	public AbilityModPropertyInt m_selfHealOnSubsequentTurnsMod;

	public AbilityModPropertyEffectInfo m_allyEffectOnSubsequentTurnsMod;

	[Header("-- Energy gain on subsequent turns")]
	public AbilityModPropertyBool m_ignoreDefaultEnergyOnSubseqTurnsMod;

	public AbilityModPropertyInt m_energyPerAllyHitOnSubseqTurnsMod;

	public AbilityModPropertyInt m_energyOnSelfHitOnSubseqTurnsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SenseiHealAoE);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SenseiHealAoE senseiHealAoE = targetAbility as SenseiHealAoE;
		if (senseiHealAoE != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SenseiHealAoE.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_circleRadiusMod, "CircleRadius", string.Empty, senseiHealAoE.m_circleRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_selfHealMod, "SelfHeal", string.Empty, senseiHealAoE.m_selfHeal, true, false);
			AbilityMod.AddToken(tokens, this.m_selfLowHealthThreshMod, "SelfLowHealthThresh", string.Empty, senseiHealAoE.m_selfLowHealthThresh, true, false, false);
			AbilityMod.AddToken(tokens, this.m_extraSelfHealForLowHealthMod, "ExtraSelfHealForLowHealth", string.Empty, senseiHealAoE.m_extraSelfHealForLowHealth, true, false);
			AbilityMod.AddToken(tokens, this.m_allyHealMod, "AllyHeal", string.Empty, senseiHealAoE.m_allyHeal, true, false);
			AbilityMod.AddToken(tokens, this.m_extraAllyHealIfSingleHitMod, "ExtraAllyHealIfSingleHit", string.Empty, senseiHealAoE.m_extraAllyHealIfSingleHit, true, false);
			AbilityMod.AddToken(tokens, this.m_extraHealForAdjacentMod, "ExtraDamageForAdjacent", string.Empty, senseiHealAoE.m_extraHealForAdjacent, true, false);
			AbilityMod.AddToken(tokens, this.m_healChangeStartDistMod, "HealChangeStartDist", string.Empty, senseiHealAoE.m_healChangeStartDist, true, false, false);
			AbilityMod.AddToken(tokens, this.m_healChangePerDistMod, "HealChangePerDist", string.Empty, senseiHealAoE.m_healChangePerDist, true, false, false);
			AbilityMod.AddToken(tokens, this.m_allyLowHealthThreshMod, "AllyLowHealthThresh", string.Empty, senseiHealAoE.m_allyLowHealthThresh, true, false, false);
			AbilityMod.AddToken(tokens, this.m_extraAllyHealForLowHealthMod, "ExtraAllyHealForLowHealth", string.Empty, senseiHealAoE.m_extraAllyHealForLowHealth, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyHitEffectMod, "AllyHitEffect", senseiHealAoE.m_allyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_allyEnergyGainMod, "AllyEnergyGain", string.Empty, senseiHealAoE.m_allyEnergyGain, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrForAnyDamageMod, "CdrForAnyDamage", string.Empty, senseiHealAoE.m_cdrForAnyDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrForDamagePerUniqueAbilityMod, "CdrForDamagePerUniqueAbility", string.Empty, senseiHealAoE.m_cdrForDamagePerUniqueAbility, true, false);
			AbilityMod.AddToken(tokens, this.m_turnsAfterInitialCastMod, "TurnsAfterInitialCast", string.Empty, senseiHealAoE.m_turnsAfterInitialCast, true, false);
			AbilityMod.AddToken(tokens, this.m_allyHealOnSubsequentTurnsMod, "AllyHealOnSubsequentTurns", string.Empty, senseiHealAoE.m_allyHealOnSubsequentTurns, true, false);
			AbilityMod.AddToken(tokens, this.m_selfHealOnSubsequentTurnsMod, "SelfHealOnSubsequentTurns", string.Empty, senseiHealAoE.m_selfHealOnSubsequentTurns, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyEffectOnSubsequentTurnsMod, "AllyEffectOnSubsequentTurns", senseiHealAoE.m_allyEffectOnSubsequentTurns, true);
			AbilityMod.AddToken(tokens, this.m_energyPerAllyHitOnSubseqTurnsMod, "EnergyPerAllyHitOnSubseqTurns", string.Empty, senseiHealAoE.m_energyPerAllyHitOnSubseqTurns, true, false);
			AbilityMod.AddToken(tokens, this.m_energyOnSelfHitOnSubseqTurnsMod, "EnergyOnSelfHitOnSubseqTurns", string.Empty, senseiHealAoE.m_energyOnSelfHitOnSubseqTurns, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SenseiHealAoE senseiHealAoE = base.GetTargetAbilityOnAbilityData(abilityData) as SenseiHealAoE;
		bool flag = senseiHealAoE != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat circleRadiusMod = this.m_circleRadiusMod;
		string prefix = "[CircleRadius]";
		bool showBaseVal = flag;
		float baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SenseiHealAoE.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = senseiHealAoE.m_circleRadius;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(circleRadiusMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyBool penetrateLoSMod = this.m_penetrateLoSMod;
		string prefix2 = "[PenetrateLoS]";
		bool showBaseVal2 = flag;
		bool baseVal2;
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
			baseVal2 = senseiHealAoE.m_penetrateLoS;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(penetrateLoSMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool includeSelfMod = this.m_includeSelfMod;
		string prefix3 = "[IncludeSelf]";
		bool showBaseVal3 = flag;
		bool baseVal3;
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
			baseVal3 = senseiHealAoE.m_includeSelf;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(includeSelfMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt selfHealMod = this.m_selfHealMod;
		string prefix4 = "[SelfHeal]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = senseiHealAoE.m_selfHeal;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(selfHealMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyFloat selfLowHealthThreshMod = this.m_selfLowHealthThreshMod;
		string prefix5 = "[SelfLowHealthThresh]";
		bool showBaseVal5 = flag;
		float baseVal5;
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
			baseVal5 = senseiHealAoE.m_selfLowHealthThresh;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + base.PropDesc(selfLowHealthThreshMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt extraSelfHealForLowHealthMod = this.m_extraSelfHealForLowHealthMod;
		string prefix6 = "[ExtraSelfHealForLowHealth]";
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
			baseVal6 = senseiHealAoE.m_extraSelfHealForLowHealth;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(extraSelfHealForLowHealthMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_allyHealMod, "[AllyHeal]", flag, (!flag) ? 0 : senseiHealAoE.m_allyHeal);
		string str7 = text;
		AbilityModPropertyInt extraAllyHealIfSingleHitMod = this.m_extraAllyHealIfSingleHitMod;
		string prefix7 = "[ExtraAllyHealIfSingleHit]";
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
			baseVal7 = senseiHealAoE.m_extraAllyHealIfSingleHit;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(extraAllyHealIfSingleHitMod, prefix7, showBaseVal7, baseVal7);
		text += base.PropDesc(this.m_extraHealForAdjacentMod, "[ExtraHealForAdjacent]", flag, (!flag) ? 0 : senseiHealAoE.m_extraHealForAdjacent);
		string str8 = text;
		AbilityModPropertyFloat healChangeStartDistMod = this.m_healChangeStartDistMod;
		string prefix8 = "[HealChangeStartDist]";
		bool showBaseVal8 = flag;
		float baseVal8;
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
			baseVal8 = senseiHealAoE.m_healChangeStartDist;
		}
		else
		{
			baseVal8 = 0f;
		}
		text = str8 + base.PropDesc(healChangeStartDistMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyFloat healChangePerDistMod = this.m_healChangePerDistMod;
		string prefix9 = "[HealChangePerDist]";
		bool showBaseVal9 = flag;
		float baseVal9;
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
			baseVal9 = senseiHealAoE.m_healChangePerDist;
		}
		else
		{
			baseVal9 = 0f;
		}
		text = str9 + base.PropDesc(healChangePerDistMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyFloat allyLowHealthThreshMod = this.m_allyLowHealthThreshMod;
		string prefix10 = "[AllyLowHealthThresh]";
		bool showBaseVal10 = flag;
		float baseVal10;
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
			baseVal10 = senseiHealAoE.m_allyLowHealthThresh;
		}
		else
		{
			baseVal10 = 0f;
		}
		text = str10 + base.PropDesc(allyLowHealthThreshMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyInt extraAllyHealForLowHealthMod = this.m_extraAllyHealForLowHealthMod;
		string prefix11 = "[ExtraAllyHealForLowHealth]";
		bool showBaseVal11 = flag;
		int baseVal11;
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
			baseVal11 = senseiHealAoE.m_extraAllyHealForLowHealth;
		}
		else
		{
			baseVal11 = 0;
		}
		text = str11 + base.PropDesc(extraAllyHealForLowHealthMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyEffectInfo allyHitEffectMod = this.m_allyHitEffectMod;
		string prefix12 = "[AllyHitEffect]";
		bool showBaseVal12 = flag;
		StandardEffectInfo baseVal12;
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
			baseVal12 = senseiHealAoE.m_allyHitEffect;
		}
		else
		{
			baseVal12 = null;
		}
		text = str12 + base.PropDesc(allyHitEffectMod, prefix12, showBaseVal12, baseVal12);
		string str13 = text;
		AbilityModPropertyInt allyEnergyGainMod = this.m_allyEnergyGainMod;
		string prefix13 = "[AllyEnergyGain]";
		bool showBaseVal13 = flag;
		int baseVal13;
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
			baseVal13 = senseiHealAoE.m_allyEnergyGain;
		}
		else
		{
			baseVal13 = 0;
		}
		text = str13 + base.PropDesc(allyEnergyGainMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyInt cdrForAnyDamageMod = this.m_cdrForAnyDamageMod;
		string prefix14 = "[CdrForAnyDamage]";
		bool showBaseVal14 = flag;
		int baseVal14;
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
			baseVal14 = senseiHealAoE.m_cdrForAnyDamage;
		}
		else
		{
			baseVal14 = 0;
		}
		text = str14 + base.PropDesc(cdrForAnyDamageMod, prefix14, showBaseVal14, baseVal14);
		string str15 = text;
		AbilityModPropertyInt cdrForDamagePerUniqueAbilityMod = this.m_cdrForDamagePerUniqueAbilityMod;
		string prefix15 = "[CdrForDamagePerUniqueAbility]";
		bool showBaseVal15 = flag;
		int baseVal15;
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
			baseVal15 = senseiHealAoE.m_cdrForDamagePerUniqueAbility;
		}
		else
		{
			baseVal15 = 0;
		}
		text = str15 + base.PropDesc(cdrForDamagePerUniqueAbilityMod, prefix15, showBaseVal15, baseVal15);
		string str16 = text;
		AbilityModPropertyInt turnsAfterInitialCastMod = this.m_turnsAfterInitialCastMod;
		string prefix16 = "[TurnsAfterInitialCast]";
		bool showBaseVal16 = flag;
		int baseVal16;
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
			baseVal16 = senseiHealAoE.m_turnsAfterInitialCast;
		}
		else
		{
			baseVal16 = 0;
		}
		text = str16 + base.PropDesc(turnsAfterInitialCastMod, prefix16, showBaseVal16, baseVal16);
		text += base.PropDesc(this.m_allyHealOnSubsequentTurnsMod, "[AllyHealOnSubsequentTurns]", flag, (!flag) ? 0 : senseiHealAoE.m_allyHealOnSubsequentTurns);
		string str17 = text;
		AbilityModPropertyInt selfHealOnSubsequentTurnsMod = this.m_selfHealOnSubsequentTurnsMod;
		string prefix17 = "[SelfHealOnSubsequentTurns]";
		bool showBaseVal17 = flag;
		int baseVal17;
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
			baseVal17 = senseiHealAoE.m_selfHealOnSubsequentTurns;
		}
		else
		{
			baseVal17 = 0;
		}
		text = str17 + base.PropDesc(selfHealOnSubsequentTurnsMod, prefix17, showBaseVal17, baseVal17);
		text += base.PropDesc(this.m_allyEffectOnSubsequentTurnsMod, "[AllyEffectOnSubsequentTurns]", flag, (!flag) ? null : senseiHealAoE.m_allyEffectOnSubsequentTurns);
		text += base.PropDesc(this.m_ignoreDefaultEnergyOnSubseqTurnsMod, "[IgnoreDefaultEnergyOnSubseqTurns]", flag, flag && senseiHealAoE.m_ignoreDefaultEnergyOnSubseqTurns);
		string str18 = text;
		AbilityModPropertyInt energyPerAllyHitOnSubseqTurnsMod = this.m_energyPerAllyHitOnSubseqTurnsMod;
		string prefix18 = "[EnergyPerAllyHitOnSubseqTurns]";
		bool showBaseVal18 = flag;
		int baseVal18;
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
			baseVal18 = senseiHealAoE.m_energyPerAllyHitOnSubseqTurns;
		}
		else
		{
			baseVal18 = 0;
		}
		text = str18 + base.PropDesc(energyPerAllyHitOnSubseqTurnsMod, prefix18, showBaseVal18, baseVal18);
		return text + base.PropDesc(this.m_energyOnSelfHitOnSubseqTurnsMod, "[EnergyOnSelfHitOnSubseqTurns]", flag, (!flag) ? 0 : senseiHealAoE.m_energyOnSelfHitOnSubseqTurns);
	}
}
