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
			AbilityMod.AddToken_EffectMod(tokens, this.m_spentCrystalsEffectMod, "SpentCrystalsEffect", martyrSpendCrystals.m_spentCrystalsEffect, true);
			AbilityMod.AddToken(tokens, this.m_selfHealBaseMod, "SelfHealBase", string.Empty, martyrSpendCrystals.m_selfHealBase, true, false);
			AbilityMod.AddToken(tokens, this.m_selfHealPerCrystalSpentMod, "SelfHealPerCrystalSpent", string.Empty, martyrSpendCrystals.m_selfHealPerCrystalSpent, true, false);
			AbilityMod.AddToken(tokens, this.m_selfHealPerEnemyHitMod, "SelfHealPerEnemyHit", string.Empty, martyrSpendCrystals.m_selfHealPerEnemyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_extraSelfHealPerTurnAtMaxEnergyMod, "ExtraSelfHealPerTurnAtMaxEnergy", string.Empty, martyrSpendCrystals.m_extraSelfHealPerTurnAtMaxEnergy, true, false);
			AbilityMod.AddToken(tokens, this.m_maxExtraSelfHealForMaxEnergyMod, "MaxExtraSelfHealForMaxEnergy", string.Empty, martyrSpendCrystals.m_maxExtraSelfHealForMaxEnergy, true, false);
			AbilityMod.AddToken(tokens, this.m_selfAbsorbBaseMod, "SelfAbsorbBase", string.Empty, martyrSpendCrystals.m_selfAbsorbBase, true, false);
			AbilityMod.AddToken(tokens, this.m_selfAbsorbPerCrystalSpentMod, "SelfAbsorbPerCrystalSpent", string.Empty, martyrSpendCrystals.m_selfAbsorbPerCrystalSpent, true, false);
			AbilityMod.AddToken(tokens, this.m_aoeRadiusBaseMod, "AoeRadiusBase", string.Empty, martyrSpendCrystals.m_aoeRadiusBase, true, true, false);
			AbilityMod.AddToken(tokens, this.m_aoeRadiuePerCrystalMod, "AoeRadiuePerCrystal", string.Empty, martyrSpendCrystals.m_aoeRadiuePerCrystal, true, true, false);
			AbilityMod.AddToken(tokens, this.m_damageBaseMod, "DamageBase", string.Empty, martyrSpendCrystals.m_damageBase, true, false);
			AbilityMod.AddToken(tokens, this.m_damagePerCrystalMod, "DamagePerCrystal", string.Empty, martyrSpendCrystals.m_damagePerCrystal, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", martyrSpendCrystals.m_enemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_allyHealBaseMod, "AllyHealBase", string.Empty, martyrSpendCrystals.m_allyHealBase, true, false);
			AbilityMod.AddToken(tokens, this.m_allyHealPerCrystalMod, "AllyHealPerCrystal", string.Empty, martyrSpendCrystals.m_allyHealPerCrystal, true, false);
			AbilityMod.AddToken(tokens, this.m_allyHealPerEnemyHitMod, "AllyHealPerEnemyHit", string.Empty, martyrSpendCrystals.m_allyHealPerEnemyHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyHitEffectMod, "AllyHitEffect", martyrSpendCrystals.m_allyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_selfEnergyGainOnCastMod, "SelfEnergyGainOnCast", string.Empty, martyrSpendCrystals.m_selfEnergyGainOnCast, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrOnProtectAllyAbilityMod, "CdrOnProtectAllyAbility", string.Empty, martyrSpendCrystals.m_cdrOnProtectAllyAbility, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrSpendCrystals martyrSpendCrystals = base.GetTargetAbilityOnAbilityData(abilityData) as MartyrSpendCrystals;
		bool flag = martyrSpendCrystals != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyEffectInfo spentCrystalsEffectMod = this.m_spentCrystalsEffectMod;
		string prefix = "[SpentCrystalsEffect]";
		bool showBaseVal = flag;
		StandardEffectInfo baseVal;
		if (flag)
		{
			baseVal = martyrSpendCrystals.m_spentCrystalsEffect;
		}
		else
		{
			baseVal = null;
		}
		text = str + base.PropDesc(spentCrystalsEffectMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt selfHealBaseMod = this.m_selfHealBaseMod;
		string prefix2 = "[SelfHealBase]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = martyrSpendCrystals.m_selfHealBase;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(selfHealBaseMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt selfHealPerCrystalSpentMod = this.m_selfHealPerCrystalSpentMod;
		string prefix3 = "[SelfHealPerCrystalSpent]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = martyrSpendCrystals.m_selfHealPerCrystalSpent;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(selfHealPerCrystalSpentMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt selfHealPerEnemyHitMod = this.m_selfHealPerEnemyHitMod;
		string prefix4 = "[SelfHealPerEnemyHit]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = martyrSpendCrystals.m_selfHealPerEnemyHit;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(selfHealPerEnemyHitMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool selfHealIsOverTimeMod = this.m_selfHealIsOverTimeMod;
		string prefix5 = "[SelfHealIsOverTime]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = martyrSpendCrystals.m_selfHealIsOverTime;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(selfHealIsOverTimeMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_extraSelfHealPerTurnAtMaxEnergyMod, "[ExtraSelfHealPerTurnAtMaxEnergy]", flag, (!flag) ? 0 : martyrSpendCrystals.m_extraSelfHealPerTurnAtMaxEnergy);
		string str6 = text;
		AbilityModPropertyInt maxExtraSelfHealForMaxEnergyMod = this.m_maxExtraSelfHealForMaxEnergyMod;
		string prefix6 = "[MaxExtraSelfHealForMaxEnergy]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = martyrSpendCrystals.m_maxExtraSelfHealForMaxEnergy;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(maxExtraSelfHealForMaxEnergyMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_selfAbsorbBaseMod, "[SelfAbsorbBase]", flag, (!flag) ? 0 : martyrSpendCrystals.m_selfAbsorbBase);
		string str7 = text;
		AbilityModPropertyInt selfAbsorbPerCrystalSpentMod = this.m_selfAbsorbPerCrystalSpentMod;
		string prefix7 = "[SelfAbsorbPerCrystalSpent]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = martyrSpendCrystals.m_selfAbsorbPerCrystalSpent;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(selfAbsorbPerCrystalSpentMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyFloat aoeRadiusBaseMod = this.m_aoeRadiusBaseMod;
		string prefix8 = "[AoeRadiusBase]";
		bool showBaseVal8 = flag;
		float baseVal8;
		if (flag)
		{
			baseVal8 = martyrSpendCrystals.m_aoeRadiusBase;
		}
		else
		{
			baseVal8 = 0f;
		}
		text = str8 + base.PropDesc(aoeRadiusBaseMod, prefix8, showBaseVal8, baseVal8);
		text += base.PropDesc(this.m_aoeRadiuePerCrystalMod, "[AoeRadiuePerCrystal]", flag, (!flag) ? 0f : martyrSpendCrystals.m_aoeRadiuePerCrystal);
		string str9 = text;
		AbilityModPropertyBool penetrateLosMod = this.m_penetrateLosMod;
		string prefix9 = "[PenetrateLos]";
		bool showBaseVal9 = flag;
		bool baseVal9;
		if (flag)
		{
			baseVal9 = martyrSpendCrystals.m_penetrateLos;
		}
		else
		{
			baseVal9 = false;
		}
		text = str9 + base.PropDesc(penetrateLosMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt damageBaseMod = this.m_damageBaseMod;
		string prefix10 = "[DamageBase]";
		bool showBaseVal10 = flag;
		int baseVal10;
		if (flag)
		{
			baseVal10 = martyrSpendCrystals.m_damageBase;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(damageBaseMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyInt damagePerCrystalMod = this.m_damagePerCrystalMod;
		string prefix11 = "[DamagePerCrystal]";
		bool showBaseVal11 = flag;
		int baseVal11;
		if (flag)
		{
			baseVal11 = martyrSpendCrystals.m_damagePerCrystal;
		}
		else
		{
			baseVal11 = 0;
		}
		text = str11 + base.PropDesc(damagePerCrystalMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix12 = "[EnemyHitEffect]";
		bool showBaseVal12 = flag;
		StandardEffectInfo baseVal12;
		if (flag)
		{
			baseVal12 = martyrSpendCrystals.m_enemyHitEffect;
		}
		else
		{
			baseVal12 = null;
		}
		text = str12 + base.PropDesc(enemyHitEffectMod, prefix12, showBaseVal12, baseVal12);
		string str13 = text;
		AbilityModPropertyInt allyHealBaseMod = this.m_allyHealBaseMod;
		string prefix13 = "[AllyHealBase]";
		bool showBaseVal13 = flag;
		int baseVal13;
		if (flag)
		{
			baseVal13 = martyrSpendCrystals.m_allyHealBase;
		}
		else
		{
			baseVal13 = 0;
		}
		text = str13 + base.PropDesc(allyHealBaseMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyInt allyHealPerCrystalMod = this.m_allyHealPerCrystalMod;
		string prefix14 = "[AllyHealPerCrystal]";
		bool showBaseVal14 = flag;
		int baseVal14;
		if (flag)
		{
			baseVal14 = martyrSpendCrystals.m_allyHealPerCrystal;
		}
		else
		{
			baseVal14 = 0;
		}
		text = str14 + base.PropDesc(allyHealPerCrystalMod, prefix14, showBaseVal14, baseVal14);
		text += base.PropDesc(this.m_allyHitEffectMod, "[AllyHitEffect]", flag, (!flag) ? null : martyrSpendCrystals.m_allyHitEffect);
		string str15 = text;
		AbilityModPropertyInt allyHealPerEnemyHitMod = this.m_allyHealPerEnemyHitMod;
		string prefix15 = "[AllyHealPerEnemyHit]";
		bool showBaseVal15 = flag;
		int baseVal15;
		if (flag)
		{
			baseVal15 = martyrSpendCrystals.m_allyHealPerEnemyHit;
		}
		else
		{
			baseVal15 = 0;
		}
		text = str15 + base.PropDesc(allyHealPerEnemyHitMod, prefix15, showBaseVal15, baseVal15);
		string str16 = text;
		AbilityModPropertyBool clearEnergyOnCastMod = this.m_clearEnergyOnCastMod;
		string prefix16 = "[ClearEnergyOnCast]";
		bool showBaseVal16 = flag;
		bool baseVal16;
		if (flag)
		{
			baseVal16 = martyrSpendCrystals.m_clearEnergyOnCast;
		}
		else
		{
			baseVal16 = false;
		}
		text = str16 + base.PropDesc(clearEnergyOnCastMod, prefix16, showBaseVal16, baseVal16);
		text += base.PropDesc(this.m_selfEnergyGainOnCastMod, "[SelfEnergyGainOnCast]", flag, (!flag) ? 0 : martyrSpendCrystals.m_selfEnergyGainOnCast);
		string str17 = text;
		AbilityModPropertyInt cdrOnProtectAllyAbilityMod = this.m_cdrOnProtectAllyAbilityMod;
		string prefix17 = "[CdrOnProtectAllyAbility]";
		bool showBaseVal17 = flag;
		int baseVal17;
		if (flag)
		{
			baseVal17 = martyrSpendCrystals.m_cdrOnProtectAllyAbility;
		}
		else
		{
			baseVal17 = 0;
		}
		return str17 + base.PropDesc(cdrOnProtectAllyAbilityMod, prefix17, showBaseVal17, baseVal17);
	}
}
