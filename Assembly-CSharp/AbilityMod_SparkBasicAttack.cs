using System;
using System.Collections.Generic;
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
			AbilityMod.AddToken(tokens, this.m_initialDamageMod, "Damage_FirstTurn", "damage on first turn", sparkBasicAttack.m_laserDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_damagePerTurnMod, "Damage_PerTurnAfterFirst", "damage per turn after first turn", sparkBasicAttack.m_laserHitEffect.m_effectData.m_damagePerTurn, true, false);
			AbilityMod.AddToken(tokens, this.m_additionalDamageOnRadiatedMod, "Damage_AdditionalOnRadiated", "additional damage on Radiated", sparkBasicAttack.m_additionalEnergizedDamage, true, false);
			if (this.m_useBonusDamageOverTime)
			{
				AbilityMod.AddToken(tokens, this.m_bonusDamageIncreaseRateMod, "BonusDamageGrowthRate", string.Empty, 0, false, false);
				AbilityMod.AddToken(tokens, this.m_maxBonusDamageAmountMod, "MaxBonusDamageFromGrowth", string.Empty, 0, false, false);
			}
			AbilityMod.AddToken(tokens, this.m_healOnCasterOnTickMod, "Heal_OnCasterPerTurn", "heal on caster per turn", sparkBasicAttack.m_healOnCasterOnTick, true, false);
			AbilityMod.AddToken(tokens, this.m_energyOnCasterPerTurnMod, "EnergyOnCasterPerTurn", string.Empty, sparkBasicAttack.m_energyOnCasterPerTurn, true, false);
			AbilityMod.AddToken(tokens, this.m_maxBonusEnergyFromGrowingGainMod, "MaxBonusEnergyFromGrowingGain", string.Empty, sparkBasicAttack.m_maxBonusEnergyFromGrowingGain, true, false);
			AbilityMod.AddToken(tokens, this.m_bonusEnergyGrowthRateMod, "BonusEnergyGrowthRate", string.Empty, sparkBasicAttack.m_bonusEnergyGrowthRate, true, false);
			AbilityMod.AddToken(tokens, this.m_tetherDistanceMod, "TetherDistance", "tether distance before breaking", sparkBasicAttack.m_tetherDistance, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_tetherBaseEffectOverride, "TetherEffect", sparkBasicAttack.m_laserHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_tetherDurationMod, "TetherDuration", string.Empty, sparkBasicAttack.m_tetherDuration, true, false);
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "Laser", sparkBasicAttack.m_laserInfo, true);
			AbilityMod.AddToken(tokens, this.m_energyGainCyclePeriod, "Energy_CyclePeriod", "energy gain once every X turns", 0, true, false);
			AbilityMod.AddToken(tokens, this.m_energyGainPerCycle, "Energy_AmountPerCycle", "energy gain amount", 0, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkBasicAttack sparkBasicAttack = base.GetTargetAbilityOnAbilityData(abilityData) as SparkBasicAttack;
		bool flag = sparkBasicAttack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt initialDamageMod = this.m_initialDamageMod;
		string prefix = "[Initial Damage On Attach]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SparkBasicAttack.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = sparkBasicAttack.m_laserDamageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(initialDamageMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt damagePerTurnMod = this.m_damagePerTurnMod;
		string prefix2 = "[Damage per Turn]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = sparkBasicAttack.m_laserHitEffect.m_effectData.m_damagePerTurn;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(damagePerTurnMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt additionalDamageOnRadiatedMod = this.m_additionalDamageOnRadiatedMod;
		string prefix3 = "[Additional Damage on Radiated]";
		bool showBaseVal3 = flag;
		int baseVal3;
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
			baseVal3 = sparkBasicAttack.m_additionalEnergizedDamage;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(additionalDamageOnRadiatedMod, prefix3, showBaseVal3, baseVal3);
		if (this.m_useBonusDamageOverTime)
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
			text += "Using Bonus Damage Over Time (please remember to put in a max cap)\n";
			text += base.PropDesc(this.m_bonusDamageIncreaseRateMod, "[Bonus Damage Increase Per Turn]", flag, 0);
			text += base.PropDesc(this.m_maxBonusDamageAmountMod, "[Max Bonus Damage]", flag, 0);
		}
		string str4 = text;
		AbilityModPropertyInt healOnCasterOnTickMod = this.m_healOnCasterOnTickMod;
		string prefix4 = "[Heal on Caster per Turn]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = sparkBasicAttack.m_healOnCasterOnTick;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(healOnCasterOnTickMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt energyOnCasterPerTurnMod = this.m_energyOnCasterPerTurnMod;
		string prefix5 = "[EnergyOnCasterPerTurn]";
		bool showBaseVal5 = flag;
		int baseVal5;
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
			baseVal5 = sparkBasicAttack.m_energyOnCasterPerTurn;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(energyOnCasterPerTurnMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_maxBonusEnergyFromGrowingGainMod, "[MaxBonusEnergyFromGrowingGain]", flag, (!flag) ? 0 : sparkBasicAttack.m_maxBonusEnergyFromGrowingGain);
		string str6 = text;
		AbilityModPropertyInt bonusEnergyGrowthRateMod = this.m_bonusEnergyGrowthRateMod;
		string prefix6 = "[BonusEnergyGrowthRate]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = sparkBasicAttack.m_bonusEnergyGrowthRate;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(bonusEnergyGrowthRateMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyFloat tetherDistanceMod = this.m_tetherDistanceMod;
		string prefix7 = "[Tether Distance]";
		bool showBaseVal7 = flag;
		float baseVal7;
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
			baseVal7 = sparkBasicAttack.m_tetherDistance;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str7 + AbilityModHelper.GetModPropertyDesc(tetherDistanceMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyEffectInfo tetherBaseEffectOverride = this.m_tetherBaseEffectOverride;
		string prefix8 = "{ Tether Base Effect Override }";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
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
			baseVal8 = sparkBasicAttack.m_laserHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		text = str8 + AbilityModHelper.GetModPropertyDesc(tetherBaseEffectOverride, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt tetherDurationMod = this.m_tetherDurationMod;
		string prefix9 = "[TetherDuration]";
		bool showBaseVal9 = flag;
		int baseVal9;
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
			baseVal9 = sparkBasicAttack.m_tetherDuration;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(tetherDurationMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyLaserInfo laserInfoMod = this.m_laserInfoMod;
		string prefix10 = "LaserInfo";
		bool showBaseVal10 = flag;
		LaserTargetingInfo baseLaserInfo;
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
			baseLaserInfo = sparkBasicAttack.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		text = str10 + AbilityModHelper.GetModPropertyDesc(laserInfoMod, prefix10, showBaseVal10, baseLaserInfo);
		text += AbilityModHelper.GetModPropertyDesc(this.m_energyGainCyclePeriod, "Energy Gain Every X Turns", flag, 0);
		return text + AbilityModHelper.GetModPropertyDesc(this.m_energyGainPerCycle, "Energy Gain Per Cycle", flag, 0);
	}
}
