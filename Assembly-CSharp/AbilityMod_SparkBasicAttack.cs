using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_SparkBasicAttack : AbilityMod
{
	[Header("-- Damge for initial attachment and subsequent turns")]
	public AbilityModPropertyInt m_initialDamageMod;
	public AbilityModPropertyInt m_damagePerTurnMod;
	public AbilityModPropertyInt m_additionalDamageOnRadiatedMod;
	[Header("-- Bonus Damage Over Time")]
	public bool m_useBonusDamageOverTime;
	public AbilityModPropertyInt m_bonusDamageIncreaseRateMod;
	public AbilityModPropertyInt m_maxBonusDamageAmountMod;
	[Header("-- Heal on Caster, per turn")]
	public AbilityModPropertyInt m_healOnCasterOnTickMod;
	[Header("-- Energy on Caster Per Turn")]
	public AbilityModPropertyInt m_energyOnCasterPerTurnMod;
	[Header("-- Extra Energy Gain On Caster --")]
	public AbilityModPropertyInt m_maxBonusEnergyFromGrowingGainMod;
	public AbilityModPropertyInt m_bonusEnergyGrowthRateMod;
	[Header("-- Tether")]
	public AbilityModPropertyFloat m_tetherDistanceMod;
	public AbilityModPropertyEffectInfo m_tetherBaseEffectOverride;
	[Header("-- Tether Duration")]
	public AbilityModPropertyInt m_tetherDurationMod;
	[Header("-- Laser")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;
	[Header("-- Energy Gain Per X Turns")]
	public AbilityModPropertyInt m_energyGainCyclePeriod;
	public AbilityModPropertyInt m_energyGainPerCycle;

	public override Type GetTargetAbilityType()
	{
		return typeof(SparkBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SparkBasicAttack sparkBasicAttack = targetAbility as SparkBasicAttack;
		if (sparkBasicAttack != null)
		{
			AddToken(tokens, m_initialDamageMod, "Damage_FirstTurn", "damage on first turn", sparkBasicAttack.m_laserDamageAmount);
			AddToken(tokens, m_damagePerTurnMod, "Damage_PerTurnAfterFirst", "damage per turn after first turn", sparkBasicAttack.m_laserHitEffect.m_effectData.m_damagePerTurn);
			AddToken(tokens, m_additionalDamageOnRadiatedMod, "Damage_AdditionalOnRadiated", "additional damage on Radiated", sparkBasicAttack.m_additionalEnergizedDamage);
			if (m_useBonusDamageOverTime)
			{
				AddToken(tokens, m_bonusDamageIncreaseRateMod, "BonusDamageGrowthRate", "", 0, false);
				AddToken(tokens, m_maxBonusDamageAmountMod, "MaxBonusDamageFromGrowth", "", 0, false);
			}
			AddToken(tokens, m_healOnCasterOnTickMod, "Heal_OnCasterPerTurn", "heal on caster per turn", sparkBasicAttack.m_healOnCasterOnTick);
			AddToken(tokens, m_energyOnCasterPerTurnMod, "EnergyOnCasterPerTurn", "", sparkBasicAttack.m_energyOnCasterPerTurn);
			AddToken(tokens, m_maxBonusEnergyFromGrowingGainMod, "MaxBonusEnergyFromGrowingGain", "", sparkBasicAttack.m_maxBonusEnergyFromGrowingGain);
			AddToken(tokens, m_bonusEnergyGrowthRateMod, "BonusEnergyGrowthRate", "", sparkBasicAttack.m_bonusEnergyGrowthRate);
			AddToken(tokens, m_tetherDistanceMod, "TetherDistance", "tether distance before breaking", sparkBasicAttack.m_tetherDistance);
			AddToken_EffectMod(tokens, m_tetherBaseEffectOverride, "TetherEffect", sparkBasicAttack.m_laserHitEffect);
			AddToken(tokens, m_tetherDurationMod, "TetherDuration", "", sparkBasicAttack.m_tetherDuration);
			AddToken_LaserInfo(tokens, m_laserInfoMod, "Laser", sparkBasicAttack.m_laserInfo);
			AddToken(tokens, m_energyGainCyclePeriod, "Energy_CyclePeriod", "energy gain once every X turns", 0);
			AddToken(tokens, m_energyGainPerCycle, "Energy_AmountPerCycle", "energy gain amount", 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkBasicAttack sparkBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as SparkBasicAttack;
		bool isAbilityPresent = sparkBasicAttack != null;
		string desc = "";
		desc += AbilityModHelper.GetModPropertyDesc(m_initialDamageMod, "[Initial Damage On Attach]", isAbilityPresent, isAbilityPresent ? sparkBasicAttack.m_laserDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_damagePerTurnMod, "[Damage per Turn]", isAbilityPresent, isAbilityPresent ? sparkBasicAttack.m_laserHitEffect.m_effectData.m_damagePerTurn : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_additionalDamageOnRadiatedMod, "[Additional Damage on Radiated]", isAbilityPresent, isAbilityPresent ? sparkBasicAttack.m_additionalEnergizedDamage : 0);
		if (m_useBonusDamageOverTime)
		{
			desc += "Using Bonus Damage Over Time (please remember to put in a max cap)\n";
			desc += PropDesc(m_bonusDamageIncreaseRateMod, "[Bonus Damage Increase Per Turn]", isAbilityPresent);
			desc += PropDesc(m_maxBonusDamageAmountMod, "[Max Bonus Damage]", isAbilityPresent);
		}
		desc += AbilityModHelper.GetModPropertyDesc(m_healOnCasterOnTickMod, "[Heal on Caster per Turn]", isAbilityPresent, isAbilityPresent ? sparkBasicAttack.m_healOnCasterOnTick : 0);
		desc += PropDesc(m_energyOnCasterPerTurnMod, "[EnergyOnCasterPerTurn]", isAbilityPresent, isAbilityPresent ? sparkBasicAttack.m_energyOnCasterPerTurn : 0);
		desc += PropDesc(m_maxBonusEnergyFromGrowingGainMod, "[MaxBonusEnergyFromGrowingGain]", isAbilityPresent, isAbilityPresent ? sparkBasicAttack.m_maxBonusEnergyFromGrowingGain : 0);
		desc += PropDesc(m_bonusEnergyGrowthRateMod, "[BonusEnergyGrowthRate]", isAbilityPresent, isAbilityPresent ? sparkBasicAttack.m_bonusEnergyGrowthRate : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_tetherDistanceMod, "[Tether Distance]", isAbilityPresent, isAbilityPresent ? sparkBasicAttack.m_tetherDistance : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_tetherBaseEffectOverride, "{ Tether Base Effect Override }", isAbilityPresent, isAbilityPresent ? sparkBasicAttack.m_laserHitEffect : null);
		desc += PropDesc(m_tetherDurationMod, "[TetherDuration]", isAbilityPresent, isAbilityPresent ? sparkBasicAttack.m_tetherDuration : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserInfoMod, "LaserInfo", isAbilityPresent, isAbilityPresent ? sparkBasicAttack.m_laserInfo : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_energyGainCyclePeriod, "Energy Gain Every X Turns", isAbilityPresent);
		return new StringBuilder().Append(desc).Append(AbilityModHelper.GetModPropertyDesc(m_energyGainPerCycle, "Energy Gain Per Cycle", isAbilityPresent)).ToString();
	}
}
