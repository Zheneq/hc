using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_MartyrSpendCrystals : AbilityMod
{
	[Header("-- Self Healing")]
	public AbilityModPropertyEffectInfo m_spentCrystalsEffectMod;

	public AbilityModPropertyInt m_selfHealBaseMod;

	public AbilityModPropertyInt m_selfHealPerCrystalSpentMod;

	public AbilityModPropertyInt m_selfHealPerEnemyHitMod;

	public AbilityModPropertyBool m_selfHealIsOverTimeMod;

	[Space(10f)]
	public AbilityModPropertyInt m_extraSelfHealPerTurnAtMaxEnergyMod;

	public AbilityModPropertyInt m_maxExtraSelfHealForMaxEnergyMod;

	[Header("-- Self Absorb")]
	public AbilityModPropertyInt m_selfAbsorbBaseMod;

	public AbilityModPropertyInt m_selfAbsorbPerCrystalSpentMod;

	[Header("-- Enemy Hit (if using AoE targeting)")]
	public AbilityModPropertyFloat m_aoeRadiusBaseMod;

	public AbilityModPropertyFloat m_aoeRadiuePerCrystalMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	public AbilityModPropertyInt m_damageBaseMod;

	public AbilityModPropertyInt m_damagePerCrystalMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	[Header("-- Ally Hit (if using AoE targeting")]
	public AbilityModPropertyInt m_allyHealBaseMod;

	public AbilityModPropertyInt m_allyHealPerCrystalMod;

	public AbilityModPropertyInt m_allyHealPerEnemyHitMod;

	public AbilityModPropertyEffectInfo m_allyHitEffectMod;

	[Header("-- Energy Use --")]
	public AbilityModPropertyBool m_clearEnergyOnCastMod;

	public AbilityModPropertyInt m_selfEnergyGainOnCastMod;

	[Header("-- Cooldown Reduction on other abilities")]
	public AbilityModPropertyInt m_cdrOnProtectAllyAbilityMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(MartyrSpendCrystals);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MartyrSpendCrystals martyrSpendCrystals = targetAbility as MartyrSpendCrystals;
		if (martyrSpendCrystals != null)
		{
			AbilityMod.AddToken_EffectMod(tokens, m_spentCrystalsEffectMod, "SpentCrystalsEffect", martyrSpendCrystals.m_spentCrystalsEffect);
			AbilityMod.AddToken(tokens, m_selfHealBaseMod, "SelfHealBase", string.Empty, martyrSpendCrystals.m_selfHealBase);
			AbilityMod.AddToken(tokens, m_selfHealPerCrystalSpentMod, "SelfHealPerCrystalSpent", string.Empty, martyrSpendCrystals.m_selfHealPerCrystalSpent);
			AbilityMod.AddToken(tokens, m_selfHealPerEnemyHitMod, "SelfHealPerEnemyHit", string.Empty, martyrSpendCrystals.m_selfHealPerEnemyHit);
			AbilityMod.AddToken(tokens, m_extraSelfHealPerTurnAtMaxEnergyMod, "ExtraSelfHealPerTurnAtMaxEnergy", string.Empty, martyrSpendCrystals.m_extraSelfHealPerTurnAtMaxEnergy);
			AbilityMod.AddToken(tokens, m_maxExtraSelfHealForMaxEnergyMod, "MaxExtraSelfHealForMaxEnergy", string.Empty, martyrSpendCrystals.m_maxExtraSelfHealForMaxEnergy);
			AbilityMod.AddToken(tokens, m_selfAbsorbBaseMod, "SelfAbsorbBase", string.Empty, martyrSpendCrystals.m_selfAbsorbBase);
			AbilityMod.AddToken(tokens, m_selfAbsorbPerCrystalSpentMod, "SelfAbsorbPerCrystalSpent", string.Empty, martyrSpendCrystals.m_selfAbsorbPerCrystalSpent);
			AbilityMod.AddToken(tokens, m_aoeRadiusBaseMod, "AoeRadiusBase", string.Empty, martyrSpendCrystals.m_aoeRadiusBase, true, true);
			AbilityMod.AddToken(tokens, m_aoeRadiuePerCrystalMod, "AoeRadiuePerCrystal", string.Empty, martyrSpendCrystals.m_aoeRadiuePerCrystal, true, true);
			AbilityMod.AddToken(tokens, m_damageBaseMod, "DamageBase", string.Empty, martyrSpendCrystals.m_damageBase);
			AbilityMod.AddToken(tokens, m_damagePerCrystalMod, "DamagePerCrystal", string.Empty, martyrSpendCrystals.m_damagePerCrystal);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", martyrSpendCrystals.m_enemyHitEffect);
			AbilityMod.AddToken(tokens, m_allyHealBaseMod, "AllyHealBase", string.Empty, martyrSpendCrystals.m_allyHealBase);
			AbilityMod.AddToken(tokens, m_allyHealPerCrystalMod, "AllyHealPerCrystal", string.Empty, martyrSpendCrystals.m_allyHealPerCrystal);
			AbilityMod.AddToken(tokens, m_allyHealPerEnemyHitMod, "AllyHealPerEnemyHit", string.Empty, martyrSpendCrystals.m_allyHealPerEnemyHit);
			AbilityMod.AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", martyrSpendCrystals.m_allyHitEffect);
			AbilityMod.AddToken(tokens, m_selfEnergyGainOnCastMod, "SelfEnergyGainOnCast", string.Empty, martyrSpendCrystals.m_selfEnergyGainOnCast);
			AbilityMod.AddToken(tokens, m_cdrOnProtectAllyAbilityMod, "CdrOnProtectAllyAbility", string.Empty, martyrSpendCrystals.m_cdrOnProtectAllyAbility);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrSpendCrystals martyrSpendCrystals = GetTargetAbilityOnAbilityData(abilityData) as MartyrSpendCrystals;
		bool flag = martyrSpendCrystals != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyEffectInfo spentCrystalsEffectMod = m_spentCrystalsEffectMod;
		object baseVal;
		if (flag)
		{
			baseVal = martyrSpendCrystals.m_spentCrystalsEffect;
		}
		else
		{
			baseVal = null;
		}
		empty = str + PropDesc(spentCrystalsEffectMod, "[SpentCrystalsEffect]", flag, (StandardEffectInfo)baseVal);
		string str2 = empty;
		AbilityModPropertyInt selfHealBaseMod = m_selfHealBaseMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = martyrSpendCrystals.m_selfHealBase;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(selfHealBaseMod, "[SelfHealBase]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt selfHealPerCrystalSpentMod = m_selfHealPerCrystalSpentMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = martyrSpendCrystals.m_selfHealPerCrystalSpent;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(selfHealPerCrystalSpentMod, "[SelfHealPerCrystalSpent]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyInt selfHealPerEnemyHitMod = m_selfHealPerEnemyHitMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = martyrSpendCrystals.m_selfHealPerEnemyHit;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(selfHealPerEnemyHitMod, "[SelfHealPerEnemyHit]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyBool selfHealIsOverTimeMod = m_selfHealIsOverTimeMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = (martyrSpendCrystals.m_selfHealIsOverTime ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(selfHealIsOverTimeMod, "[SelfHealIsOverTime]", flag, (byte)baseVal5 != 0);
		empty += PropDesc(m_extraSelfHealPerTurnAtMaxEnergyMod, "[ExtraSelfHealPerTurnAtMaxEnergy]", flag, flag ? martyrSpendCrystals.m_extraSelfHealPerTurnAtMaxEnergy : 0);
		string str6 = empty;
		AbilityModPropertyInt maxExtraSelfHealForMaxEnergyMod = m_maxExtraSelfHealForMaxEnergyMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = martyrSpendCrystals.m_maxExtraSelfHealForMaxEnergy;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(maxExtraSelfHealForMaxEnergyMod, "[MaxExtraSelfHealForMaxEnergy]", flag, baseVal6);
		empty += PropDesc(m_selfAbsorbBaseMod, "[SelfAbsorbBase]", flag, flag ? martyrSpendCrystals.m_selfAbsorbBase : 0);
		string str7 = empty;
		AbilityModPropertyInt selfAbsorbPerCrystalSpentMod = m_selfAbsorbPerCrystalSpentMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = martyrSpendCrystals.m_selfAbsorbPerCrystalSpent;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(selfAbsorbPerCrystalSpentMod, "[SelfAbsorbPerCrystalSpent]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyFloat aoeRadiusBaseMod = m_aoeRadiusBaseMod;
		float baseVal8;
		if (flag)
		{
			baseVal8 = martyrSpendCrystals.m_aoeRadiusBase;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str8 + PropDesc(aoeRadiusBaseMod, "[AoeRadiusBase]", flag, baseVal8);
		empty += PropDesc(m_aoeRadiuePerCrystalMod, "[AoeRadiuePerCrystal]", flag, (!flag) ? 0f : martyrSpendCrystals.m_aoeRadiuePerCrystal);
		string str9 = empty;
		AbilityModPropertyBool penetrateLosMod = m_penetrateLosMod;
		int baseVal9;
		if (flag)
		{
			baseVal9 = (martyrSpendCrystals.m_penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(penetrateLosMod, "[PenetrateLos]", flag, (byte)baseVal9 != 0);
		string str10 = empty;
		AbilityModPropertyInt damageBaseMod = m_damageBaseMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = martyrSpendCrystals.m_damageBase;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(damageBaseMod, "[DamageBase]", flag, baseVal10);
		string str11 = empty;
		AbilityModPropertyInt damagePerCrystalMod = m_damagePerCrystalMod;
		int baseVal11;
		if (flag)
		{
			baseVal11 = martyrSpendCrystals.m_damagePerCrystal;
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str11 + PropDesc(damagePerCrystalMod, "[DamagePerCrystal]", flag, baseVal11);
		string str12 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal12;
		if (flag)
		{
			baseVal12 = martyrSpendCrystals.m_enemyHitEffect;
		}
		else
		{
			baseVal12 = null;
		}
		empty = str12 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal12);
		string str13 = empty;
		AbilityModPropertyInt allyHealBaseMod = m_allyHealBaseMod;
		int baseVal13;
		if (flag)
		{
			baseVal13 = martyrSpendCrystals.m_allyHealBase;
		}
		else
		{
			baseVal13 = 0;
		}
		empty = str13 + PropDesc(allyHealBaseMod, "[AllyHealBase]", flag, baseVal13);
		string str14 = empty;
		AbilityModPropertyInt allyHealPerCrystalMod = m_allyHealPerCrystalMod;
		int baseVal14;
		if (flag)
		{
			baseVal14 = martyrSpendCrystals.m_allyHealPerCrystal;
		}
		else
		{
			baseVal14 = 0;
		}
		empty = str14 + PropDesc(allyHealPerCrystalMod, "[AllyHealPerCrystal]", flag, baseVal14);
		empty += PropDesc(m_allyHitEffectMod, "[AllyHitEffect]", flag, (!flag) ? null : martyrSpendCrystals.m_allyHitEffect);
		string str15 = empty;
		AbilityModPropertyInt allyHealPerEnemyHitMod = m_allyHealPerEnemyHitMod;
		int baseVal15;
		if (flag)
		{
			baseVal15 = martyrSpendCrystals.m_allyHealPerEnemyHit;
		}
		else
		{
			baseVal15 = 0;
		}
		empty = str15 + PropDesc(allyHealPerEnemyHitMod, "[AllyHealPerEnemyHit]", flag, baseVal15);
		string str16 = empty;
		AbilityModPropertyBool clearEnergyOnCastMod = m_clearEnergyOnCastMod;
		int baseVal16;
		if (flag)
		{
			baseVal16 = (martyrSpendCrystals.m_clearEnergyOnCast ? 1 : 0);
		}
		else
		{
			baseVal16 = 0;
		}
		empty = str16 + PropDesc(clearEnergyOnCastMod, "[ClearEnergyOnCast]", flag, (byte)baseVal16 != 0);
		empty += PropDesc(m_selfEnergyGainOnCastMod, "[SelfEnergyGainOnCast]", flag, flag ? martyrSpendCrystals.m_selfEnergyGainOnCast : 0);
		string str17 = empty;
		AbilityModPropertyInt cdrOnProtectAllyAbilityMod = m_cdrOnProtectAllyAbilityMod;
		int baseVal17;
		if (flag)
		{
			baseVal17 = martyrSpendCrystals.m_cdrOnProtectAllyAbility;
		}
		else
		{
			baseVal17 = 0;
		}
		return str17 + PropDesc(cdrOnProtectAllyAbilityMod, "[CdrOnProtectAllyAbility]", flag, baseVal17);
	}
}
