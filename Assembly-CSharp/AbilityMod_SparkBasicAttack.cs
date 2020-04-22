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
			AbilityMod.AddToken(tokens, m_initialDamageMod, "Damage_FirstTurn", "damage on first turn", sparkBasicAttack.m_laserDamageAmount);
			AbilityMod.AddToken(tokens, m_damagePerTurnMod, "Damage_PerTurnAfterFirst", "damage per turn after first turn", sparkBasicAttack.m_laserHitEffect.m_effectData.m_damagePerTurn);
			AbilityMod.AddToken(tokens, m_additionalDamageOnRadiatedMod, "Damage_AdditionalOnRadiated", "additional damage on Radiated", sparkBasicAttack.m_additionalEnergizedDamage);
			if (m_useBonusDamageOverTime)
			{
				AbilityMod.AddToken(tokens, m_bonusDamageIncreaseRateMod, "BonusDamageGrowthRate", string.Empty, 0, false);
				AbilityMod.AddToken(tokens, m_maxBonusDamageAmountMod, "MaxBonusDamageFromGrowth", string.Empty, 0, false);
			}
			AbilityMod.AddToken(tokens, m_healOnCasterOnTickMod, "Heal_OnCasterPerTurn", "heal on caster per turn", sparkBasicAttack.m_healOnCasterOnTick);
			AbilityMod.AddToken(tokens, m_energyOnCasterPerTurnMod, "EnergyOnCasterPerTurn", string.Empty, sparkBasicAttack.m_energyOnCasterPerTurn);
			AbilityMod.AddToken(tokens, m_maxBonusEnergyFromGrowingGainMod, "MaxBonusEnergyFromGrowingGain", string.Empty, sparkBasicAttack.m_maxBonusEnergyFromGrowingGain);
			AbilityMod.AddToken(tokens, m_bonusEnergyGrowthRateMod, "BonusEnergyGrowthRate", string.Empty, sparkBasicAttack.m_bonusEnergyGrowthRate);
			AbilityMod.AddToken(tokens, m_tetherDistanceMod, "TetherDistance", "tether distance before breaking", sparkBasicAttack.m_tetherDistance);
			AbilityMod.AddToken_EffectMod(tokens, m_tetherBaseEffectOverride, "TetherEffect", sparkBasicAttack.m_laserHitEffect);
			AbilityMod.AddToken(tokens, m_tetherDurationMod, "TetherDuration", string.Empty, sparkBasicAttack.m_tetherDuration);
			AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "Laser", sparkBasicAttack.m_laserInfo);
			AbilityMod.AddToken(tokens, m_energyGainCyclePeriod, "Energy_CyclePeriod", "energy gain once every X turns", 0);
			AbilityMod.AddToken(tokens, m_energyGainPerCycle, "Energy_AmountPerCycle", "energy gain amount", 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkBasicAttack sparkBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as SparkBasicAttack;
		bool flag = sparkBasicAttack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt initialDamageMod = m_initialDamageMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = sparkBasicAttack.m_laserDamageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(initialDamageMod, "[Initial Damage On Attach]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt damagePerTurnMod = m_damagePerTurnMod;
		int baseVal2;
		if (flag)
		{
			while (true)
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
		empty = str2 + AbilityModHelper.GetModPropertyDesc(damagePerTurnMod, "[Damage per Turn]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt additionalDamageOnRadiatedMod = m_additionalDamageOnRadiatedMod;
		int baseVal3;
		if (flag)
		{
			while (true)
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
		empty = str3 + AbilityModHelper.GetModPropertyDesc(additionalDamageOnRadiatedMod, "[Additional Damage on Radiated]", flag, baseVal3);
		if (m_useBonusDamageOverTime)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			empty += "Using Bonus Damage Over Time (please remember to put in a max cap)\n";
			empty += PropDesc(m_bonusDamageIncreaseRateMod, "[Bonus Damage Increase Per Turn]", flag);
			empty += PropDesc(m_maxBonusDamageAmountMod, "[Max Bonus Damage]", flag);
		}
		string str4 = empty;
		AbilityModPropertyInt healOnCasterOnTickMod = m_healOnCasterOnTickMod;
		int baseVal4;
		if (flag)
		{
			while (true)
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
		empty = str4 + AbilityModHelper.GetModPropertyDesc(healOnCasterOnTickMod, "[Heal on Caster per Turn]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt energyOnCasterPerTurnMod = m_energyOnCasterPerTurnMod;
		int baseVal5;
		if (flag)
		{
			while (true)
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
		empty = str5 + PropDesc(energyOnCasterPerTurnMod, "[EnergyOnCasterPerTurn]", flag, baseVal5);
		empty += PropDesc(m_maxBonusEnergyFromGrowingGainMod, "[MaxBonusEnergyFromGrowingGain]", flag, flag ? sparkBasicAttack.m_maxBonusEnergyFromGrowingGain : 0);
		string str6 = empty;
		AbilityModPropertyInt bonusEnergyGrowthRateMod = m_bonusEnergyGrowthRateMod;
		int baseVal6;
		if (flag)
		{
			while (true)
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
		empty = str6 + PropDesc(bonusEnergyGrowthRateMod, "[BonusEnergyGrowthRate]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyFloat tetherDistanceMod = m_tetherDistanceMod;
		float baseVal7;
		if (flag)
		{
			while (true)
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
		empty = str7 + AbilityModHelper.GetModPropertyDesc(tetherDistanceMod, "[Tether Distance]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyEffectInfo tetherBaseEffectOverride = m_tetherBaseEffectOverride;
		object baseVal8;
		if (flag)
		{
			while (true)
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
		empty = str8 + AbilityModHelper.GetModPropertyDesc(tetherBaseEffectOverride, "{ Tether Base Effect Override }", flag, (StandardEffectInfo)baseVal8);
		string str9 = empty;
		AbilityModPropertyInt tetherDurationMod = m_tetherDurationMod;
		int baseVal9;
		if (flag)
		{
			while (true)
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
		empty = str9 + PropDesc(tetherDurationMod, "[TetherDuration]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyLaserInfo laserInfoMod = m_laserInfoMod;
		object baseLaserInfo;
		if (flag)
		{
			while (true)
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
		empty = str10 + AbilityModHelper.GetModPropertyDesc(laserInfoMod, "LaserInfo", flag, (LaserTargetingInfo)baseLaserInfo);
		empty += AbilityModHelper.GetModPropertyDesc(m_energyGainCyclePeriod, "Energy Gain Every X Turns", flag);
		return empty + AbilityModHelper.GetModPropertyDesc(m_energyGainPerCycle, "Energy Gain Per Cycle", flag);
	}
}
