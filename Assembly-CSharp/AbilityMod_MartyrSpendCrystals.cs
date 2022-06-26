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
			AddToken_EffectMod(tokens, m_spentCrystalsEffectMod, "SpentCrystalsEffect", martyrSpendCrystals.m_spentCrystalsEffect);
			AddToken(tokens, m_selfHealBaseMod, "SelfHealBase", "", martyrSpendCrystals.m_selfHealBase);
			AddToken(tokens, m_selfHealPerCrystalSpentMod, "SelfHealPerCrystalSpent", "", martyrSpendCrystals.m_selfHealPerCrystalSpent);
			AddToken(tokens, m_selfHealPerEnemyHitMod, "SelfHealPerEnemyHit", "", martyrSpendCrystals.m_selfHealPerEnemyHit);
			AddToken(tokens, m_extraSelfHealPerTurnAtMaxEnergyMod, "ExtraSelfHealPerTurnAtMaxEnergy", "", martyrSpendCrystals.m_extraSelfHealPerTurnAtMaxEnergy);
			AddToken(tokens, m_maxExtraSelfHealForMaxEnergyMod, "MaxExtraSelfHealForMaxEnergy", "", martyrSpendCrystals.m_maxExtraSelfHealForMaxEnergy);
			AddToken(tokens, m_selfAbsorbBaseMod, "SelfAbsorbBase", "", martyrSpendCrystals.m_selfAbsorbBase);
			AddToken(tokens, m_selfAbsorbPerCrystalSpentMod, "SelfAbsorbPerCrystalSpent", "", martyrSpendCrystals.m_selfAbsorbPerCrystalSpent);
			AddToken(tokens, m_aoeRadiusBaseMod, "AoeRadiusBase", "", martyrSpendCrystals.m_aoeRadiusBase, true, true);
			AddToken(tokens, m_aoeRadiuePerCrystalMod, "AoeRadiuePerCrystal", "", martyrSpendCrystals.m_aoeRadiuePerCrystal, true, true);
			AddToken(tokens, m_damageBaseMod, "DamageBase", "", martyrSpendCrystals.m_damageBase);
			AddToken(tokens, m_damagePerCrystalMod, "DamagePerCrystal", "", martyrSpendCrystals.m_damagePerCrystal);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", martyrSpendCrystals.m_enemyHitEffect);
			AddToken(tokens, m_allyHealBaseMod, "AllyHealBase", "", martyrSpendCrystals.m_allyHealBase);
			AddToken(tokens, m_allyHealPerCrystalMod, "AllyHealPerCrystal", "", martyrSpendCrystals.m_allyHealPerCrystal);
			AddToken(tokens, m_allyHealPerEnemyHitMod, "AllyHealPerEnemyHit", "", martyrSpendCrystals.m_allyHealPerEnemyHit);
			AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", martyrSpendCrystals.m_allyHitEffect);
			AddToken(tokens, m_selfEnergyGainOnCastMod, "SelfEnergyGainOnCast", "", martyrSpendCrystals.m_selfEnergyGainOnCast);
			AddToken(tokens, m_cdrOnProtectAllyAbilityMod, "CdrOnProtectAllyAbility", "", martyrSpendCrystals.m_cdrOnProtectAllyAbility);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrSpendCrystals martyrSpendCrystals = GetTargetAbilityOnAbilityData(abilityData) as MartyrSpendCrystals;
		bool isValid = martyrSpendCrystals != null;
		string desc = "";
		desc += PropDesc(m_spentCrystalsEffectMod, "[SpentCrystalsEffect]", isValid, isValid ? martyrSpendCrystals.m_spentCrystalsEffect : null);
		desc += PropDesc(m_selfHealBaseMod, "[SelfHealBase]", isValid, isValid ? martyrSpendCrystals.m_selfHealBase : 0);
		desc += PropDesc(m_selfHealPerCrystalSpentMod, "[SelfHealPerCrystalSpent]", isValid, isValid ? martyrSpendCrystals.m_selfHealPerCrystalSpent : 0);
		desc += PropDesc(m_selfHealPerEnemyHitMod, "[SelfHealPerEnemyHit]", isValid, isValid ? martyrSpendCrystals.m_selfHealPerEnemyHit : 0);
		desc += PropDesc(m_selfHealIsOverTimeMod, "[SelfHealIsOverTime]", isValid, isValid && martyrSpendCrystals.m_selfHealIsOverTime);
		desc += PropDesc(m_extraSelfHealPerTurnAtMaxEnergyMod, "[ExtraSelfHealPerTurnAtMaxEnergy]", isValid, isValid ? martyrSpendCrystals.m_extraSelfHealPerTurnAtMaxEnergy : 0);
		desc += PropDesc(m_maxExtraSelfHealForMaxEnergyMod, "[MaxExtraSelfHealForMaxEnergy]", isValid, isValid ? martyrSpendCrystals.m_maxExtraSelfHealForMaxEnergy : 0);
		desc += PropDesc(m_selfAbsorbBaseMod, "[SelfAbsorbBase]", isValid, isValid ? martyrSpendCrystals.m_selfAbsorbBase : 0);
		desc += PropDesc(m_selfAbsorbPerCrystalSpentMod, "[SelfAbsorbPerCrystalSpent]", isValid, isValid ? martyrSpendCrystals.m_selfAbsorbPerCrystalSpent : 0);
		desc += PropDesc(m_aoeRadiusBaseMod, "[AoeRadiusBase]", isValid, isValid ? martyrSpendCrystals.m_aoeRadiusBase : 0f);
		desc += PropDesc(m_aoeRadiuePerCrystalMod, "[AoeRadiuePerCrystal]", isValid, isValid ? martyrSpendCrystals.m_aoeRadiuePerCrystal : 0f);
		desc += PropDesc(m_penetrateLosMod, "[PenetrateLos]", isValid, isValid && martyrSpendCrystals.m_penetrateLos);
		desc += PropDesc(m_damageBaseMod, "[DamageBase]", isValid, isValid ? martyrSpendCrystals.m_damageBase : 0);
		desc += PropDesc(m_damagePerCrystalMod, "[DamagePerCrystal]", isValid, isValid ? martyrSpendCrystals.m_damagePerCrystal : 0);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isValid, isValid ? martyrSpendCrystals.m_enemyHitEffect : null);
		desc += PropDesc(m_allyHealBaseMod, "[AllyHealBase]", isValid, isValid ? martyrSpendCrystals.m_allyHealBase : 0);
		desc += PropDesc(m_allyHealPerCrystalMod, "[AllyHealPerCrystal]", isValid, isValid ? martyrSpendCrystals.m_allyHealPerCrystal : 0);
		desc += PropDesc(m_allyHitEffectMod, "[AllyHitEffect]", isValid, isValid ? martyrSpendCrystals.m_allyHitEffect : null);
		desc += PropDesc(m_allyHealPerEnemyHitMod, "[AllyHealPerEnemyHit]", isValid, isValid ? martyrSpendCrystals.m_allyHealPerEnemyHit : 0);
		desc += PropDesc(m_clearEnergyOnCastMod, "[ClearEnergyOnCast]", isValid, isValid && martyrSpendCrystals.m_clearEnergyOnCast);
		desc += PropDesc(m_selfEnergyGainOnCastMod, "[SelfEnergyGainOnCast]", isValid, isValid ? martyrSpendCrystals.m_selfEnergyGainOnCast : 0);
		return desc + PropDesc(m_cdrOnProtectAllyAbilityMod, "[CdrOnProtectAllyAbility]", isValid, isValid ? martyrSpendCrystals.m_cdrOnProtectAllyAbility : 0);
	}
}
