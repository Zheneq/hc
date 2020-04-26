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
		if (!(senseiHealAoE != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_circleRadiusMod, "CircleRadius", string.Empty, senseiHealAoE.m_circleRadius);
			AbilityMod.AddToken(tokens, m_selfHealMod, "SelfHeal", string.Empty, senseiHealAoE.m_selfHeal);
			AbilityMod.AddToken(tokens, m_selfLowHealthThreshMod, "SelfLowHealthThresh", string.Empty, senseiHealAoE.m_selfLowHealthThresh);
			AbilityMod.AddToken(tokens, m_extraSelfHealForLowHealthMod, "ExtraSelfHealForLowHealth", string.Empty, senseiHealAoE.m_extraSelfHealForLowHealth);
			AbilityMod.AddToken(tokens, m_allyHealMod, "AllyHeal", string.Empty, senseiHealAoE.m_allyHeal);
			AbilityMod.AddToken(tokens, m_extraAllyHealIfSingleHitMod, "ExtraAllyHealIfSingleHit", string.Empty, senseiHealAoE.m_extraAllyHealIfSingleHit);
			AbilityMod.AddToken(tokens, m_extraHealForAdjacentMod, "ExtraDamageForAdjacent", string.Empty, senseiHealAoE.m_extraHealForAdjacent);
			AbilityMod.AddToken(tokens, m_healChangeStartDistMod, "HealChangeStartDist", string.Empty, senseiHealAoE.m_healChangeStartDist);
			AbilityMod.AddToken(tokens, m_healChangePerDistMod, "HealChangePerDist", string.Empty, senseiHealAoE.m_healChangePerDist);
			AbilityMod.AddToken(tokens, m_allyLowHealthThreshMod, "AllyLowHealthThresh", string.Empty, senseiHealAoE.m_allyLowHealthThresh);
			AbilityMod.AddToken(tokens, m_extraAllyHealForLowHealthMod, "ExtraAllyHealForLowHealth", string.Empty, senseiHealAoE.m_extraAllyHealForLowHealth);
			AbilityMod.AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", senseiHealAoE.m_allyHitEffect);
			AbilityMod.AddToken(tokens, m_allyEnergyGainMod, "AllyEnergyGain", string.Empty, senseiHealAoE.m_allyEnergyGain);
			AbilityMod.AddToken(tokens, m_cdrForAnyDamageMod, "CdrForAnyDamage", string.Empty, senseiHealAoE.m_cdrForAnyDamage);
			AbilityMod.AddToken(tokens, m_cdrForDamagePerUniqueAbilityMod, "CdrForDamagePerUniqueAbility", string.Empty, senseiHealAoE.m_cdrForDamagePerUniqueAbility);
			AbilityMod.AddToken(tokens, m_turnsAfterInitialCastMod, "TurnsAfterInitialCast", string.Empty, senseiHealAoE.m_turnsAfterInitialCast);
			AbilityMod.AddToken(tokens, m_allyHealOnSubsequentTurnsMod, "AllyHealOnSubsequentTurns", string.Empty, senseiHealAoE.m_allyHealOnSubsequentTurns);
			AbilityMod.AddToken(tokens, m_selfHealOnSubsequentTurnsMod, "SelfHealOnSubsequentTurns", string.Empty, senseiHealAoE.m_selfHealOnSubsequentTurns);
			AbilityMod.AddToken_EffectMod(tokens, m_allyEffectOnSubsequentTurnsMod, "AllyEffectOnSubsequentTurns", senseiHealAoE.m_allyEffectOnSubsequentTurns);
			AbilityMod.AddToken(tokens, m_energyPerAllyHitOnSubseqTurnsMod, "EnergyPerAllyHitOnSubseqTurns", string.Empty, senseiHealAoE.m_energyPerAllyHitOnSubseqTurns);
			AbilityMod.AddToken(tokens, m_energyOnSelfHitOnSubseqTurnsMod, "EnergyOnSelfHitOnSubseqTurns", string.Empty, senseiHealAoE.m_energyOnSelfHitOnSubseqTurns);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SenseiHealAoE senseiHealAoE = GetTargetAbilityOnAbilityData(abilityData) as SenseiHealAoE;
		bool flag = senseiHealAoE != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat circleRadiusMod = m_circleRadiusMod;
		float baseVal;
		if (flag)
		{
			baseVal = senseiHealAoE.m_circleRadius;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(circleRadiusMod, "[CircleRadius]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyBool penetrateLoSMod = m_penetrateLoSMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = (senseiHealAoE.m_penetrateLoS ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(penetrateLoSMod, "[PenetrateLoS]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyBool includeSelfMod = m_includeSelfMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = (senseiHealAoE.m_includeSelf ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(includeSelfMod, "[IncludeSelf]", flag, (byte)baseVal3 != 0);
		string str4 = empty;
		AbilityModPropertyInt selfHealMod = m_selfHealMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = senseiHealAoE.m_selfHeal;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(selfHealMod, "[SelfHeal]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyFloat selfLowHealthThreshMod = m_selfLowHealthThreshMod;
		float baseVal5;
		if (flag)
		{
			baseVal5 = senseiHealAoE.m_selfLowHealthThresh;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + PropDesc(selfLowHealthThreshMod, "[SelfLowHealthThresh]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt extraSelfHealForLowHealthMod = m_extraSelfHealForLowHealthMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = senseiHealAoE.m_extraSelfHealForLowHealth;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(extraSelfHealForLowHealthMod, "[ExtraSelfHealForLowHealth]", flag, baseVal6);
		empty += PropDesc(m_allyHealMod, "[AllyHeal]", flag, flag ? senseiHealAoE.m_allyHeal : 0);
		string str7 = empty;
		AbilityModPropertyInt extraAllyHealIfSingleHitMod = m_extraAllyHealIfSingleHitMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = senseiHealAoE.m_extraAllyHealIfSingleHit;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(extraAllyHealIfSingleHitMod, "[ExtraAllyHealIfSingleHit]", flag, baseVal7);
		empty += PropDesc(m_extraHealForAdjacentMod, "[ExtraHealForAdjacent]", flag, flag ? senseiHealAoE.m_extraHealForAdjacent : 0);
		string str8 = empty;
		AbilityModPropertyFloat healChangeStartDistMod = m_healChangeStartDistMod;
		float baseVal8;
		if (flag)
		{
			baseVal8 = senseiHealAoE.m_healChangeStartDist;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str8 + PropDesc(healChangeStartDistMod, "[HealChangeStartDist]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyFloat healChangePerDistMod = m_healChangePerDistMod;
		float baseVal9;
		if (flag)
		{
			baseVal9 = senseiHealAoE.m_healChangePerDist;
		}
		else
		{
			baseVal9 = 0f;
		}
		empty = str9 + PropDesc(healChangePerDistMod, "[HealChangePerDist]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyFloat allyLowHealthThreshMod = m_allyLowHealthThreshMod;
		float baseVal10;
		if (flag)
		{
			baseVal10 = senseiHealAoE.m_allyLowHealthThresh;
		}
		else
		{
			baseVal10 = 0f;
		}
		empty = str10 + PropDesc(allyLowHealthThreshMod, "[AllyLowHealthThresh]", flag, baseVal10);
		string str11 = empty;
		AbilityModPropertyInt extraAllyHealForLowHealthMod = m_extraAllyHealForLowHealthMod;
		int baseVal11;
		if (flag)
		{
			baseVal11 = senseiHealAoE.m_extraAllyHealForLowHealth;
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str11 + PropDesc(extraAllyHealForLowHealthMod, "[ExtraAllyHealForLowHealth]", flag, baseVal11);
		string str12 = empty;
		AbilityModPropertyEffectInfo allyHitEffectMod = m_allyHitEffectMod;
		object baseVal12;
		if (flag)
		{
			baseVal12 = senseiHealAoE.m_allyHitEffect;
		}
		else
		{
			baseVal12 = null;
		}
		empty = str12 + PropDesc(allyHitEffectMod, "[AllyHitEffect]", flag, (StandardEffectInfo)baseVal12);
		string str13 = empty;
		AbilityModPropertyInt allyEnergyGainMod = m_allyEnergyGainMod;
		int baseVal13;
		if (flag)
		{
			baseVal13 = senseiHealAoE.m_allyEnergyGain;
		}
		else
		{
			baseVal13 = 0;
		}
		empty = str13 + PropDesc(allyEnergyGainMod, "[AllyEnergyGain]", flag, baseVal13);
		string str14 = empty;
		AbilityModPropertyInt cdrForAnyDamageMod = m_cdrForAnyDamageMod;
		int baseVal14;
		if (flag)
		{
			baseVal14 = senseiHealAoE.m_cdrForAnyDamage;
		}
		else
		{
			baseVal14 = 0;
		}
		empty = str14 + PropDesc(cdrForAnyDamageMod, "[CdrForAnyDamage]", flag, baseVal14);
		string str15 = empty;
		AbilityModPropertyInt cdrForDamagePerUniqueAbilityMod = m_cdrForDamagePerUniqueAbilityMod;
		int baseVal15;
		if (flag)
		{
			baseVal15 = senseiHealAoE.m_cdrForDamagePerUniqueAbility;
		}
		else
		{
			baseVal15 = 0;
		}
		empty = str15 + PropDesc(cdrForDamagePerUniqueAbilityMod, "[CdrForDamagePerUniqueAbility]", flag, baseVal15);
		string str16 = empty;
		AbilityModPropertyInt turnsAfterInitialCastMod = m_turnsAfterInitialCastMod;
		int baseVal16;
		if (flag)
		{
			baseVal16 = senseiHealAoE.m_turnsAfterInitialCast;
		}
		else
		{
			baseVal16 = 0;
		}
		empty = str16 + PropDesc(turnsAfterInitialCastMod, "[TurnsAfterInitialCast]", flag, baseVal16);
		empty += PropDesc(m_allyHealOnSubsequentTurnsMod, "[AllyHealOnSubsequentTurns]", flag, flag ? senseiHealAoE.m_allyHealOnSubsequentTurns : 0);
		string str17 = empty;
		AbilityModPropertyInt selfHealOnSubsequentTurnsMod = m_selfHealOnSubsequentTurnsMod;
		int baseVal17;
		if (flag)
		{
			baseVal17 = senseiHealAoE.m_selfHealOnSubsequentTurns;
		}
		else
		{
			baseVal17 = 0;
		}
		empty = str17 + PropDesc(selfHealOnSubsequentTurnsMod, "[SelfHealOnSubsequentTurns]", flag, baseVal17);
		empty += PropDesc(m_allyEffectOnSubsequentTurnsMod, "[AllyEffectOnSubsequentTurns]", flag, (!flag) ? null : senseiHealAoE.m_allyEffectOnSubsequentTurns);
		empty += PropDesc(m_ignoreDefaultEnergyOnSubseqTurnsMod, "[IgnoreDefaultEnergyOnSubseqTurns]", flag, flag && senseiHealAoE.m_ignoreDefaultEnergyOnSubseqTurns);
		string str18 = empty;
		AbilityModPropertyInt energyPerAllyHitOnSubseqTurnsMod = m_energyPerAllyHitOnSubseqTurnsMod;
		int baseVal18;
		if (flag)
		{
			baseVal18 = senseiHealAoE.m_energyPerAllyHitOnSubseqTurns;
		}
		else
		{
			baseVal18 = 0;
		}
		empty = str18 + PropDesc(energyPerAllyHitOnSubseqTurnsMod, "[EnergyPerAllyHitOnSubseqTurns]", flag, baseVal18);
		return empty + PropDesc(m_energyOnSelfHitOnSubseqTurnsMod, "[EnergyOnSelfHitOnSubseqTurns]", flag, flag ? senseiHealAoE.m_energyOnSelfHitOnSubseqTurns : 0);
	}
}
