using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SparkHealingBeam : AbilityMod
{
	[Header("-- Healing")]
	public AbilityModPropertyInt m_initialHealingMod;

	public AbilityModPropertyInt m_healPerTurnMod;

	public AbilityModPropertyInt m_additionalHealOnRadiatedMod;

	[Header("-- Bonus Healing Over Time")]
	public bool m_useBonusHealOverTime;

	public AbilityModPropertyInt m_bonusAllyHealIncreaseRate;

	public AbilityModPropertyInt m_maxAllyBonusHealAmount;

	[Header("-- Heal on Caster, per turn")]
	public AbilityModPropertyInt m_healOnCasterOnTickMod;

	[Header("-- Energy on Caster Per Turn")]
	public AbilityModPropertyInt m_energyOnCasterPerTurnMod;

	[Header("-- Tether")]
	public AbilityModPropertyFloat m_tetherDistanceMod;

	public AbilityModPropertyEffectInfo m_tetherBaseEffectOverride;

	[Header("-- Tether Duration")]
	public AbilityModPropertyInt m_tetherDurationMod;

	[Header("-- Laser")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;

	[Header("-- Effect on Target for taking X Damage in Turn (non-positive threshold => not applying)")]
	public int m_xDamageThreshold = -1;

	public StandardEffectInfo m_effectOnTargetForTakingXDamage;

	public override Type GetTargetAbilityType()
	{
		return typeof(SparkHealingBeam);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SparkHealingBeam sparkHealingBeam = targetAbility as SparkHealingBeam;
		if (!(sparkHealingBeam != null))
		{
			return;
		}
		AbilityMod.AddToken(tokens, m_initialHealingMod, "Heal_FirstTurn", "heal on first turn", sparkHealingBeam.m_laserHealingAmount);
		AbilityMod.AddToken(tokens, m_healPerTurnMod, "Heal_PerTurnAfterFirst", "heal per turn after first turn", sparkHealingBeam.m_laserHitEffect.m_effectData.m_healingPerTurn);
		AbilityMod.AddToken(tokens, m_additionalHealOnRadiatedMod, "Heal_AdditionalOnRadiated", "additional damage on Radiated", sparkHealingBeam.m_additionalEnergizedHealing);
		if (m_useBonusHealOverTime)
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
			AbilityMod.AddToken(tokens, m_bonusAllyHealIncreaseRate, "BonusAllyHeal_GrowthRate", "increase in bonus heal per turn", 0);
			AbilityMod.AddToken(tokens, m_maxAllyBonusHealAmount, "BonusAllyHeal_MaxHealAmount", "max bonus heal amount", 0);
		}
		AbilityMod.AddToken(tokens, m_healOnCasterOnTickMod, "Heal_OnCasterPerTurn", "heal on caster per turn", sparkHealingBeam.m_healOnSelfOnTick);
		AbilityMod.AddToken(tokens, m_energyOnCasterPerTurnMod, "EnergyOnCasterPerTurn", string.Empty, sparkHealingBeam.m_energyOnCasterPerTurn);
		AbilityMod.AddToken(tokens, m_tetherDistanceMod, "TetherDistance", "tether distance before breaking", sparkHealingBeam.m_tetherDistance);
		AbilityMod.AddToken_EffectMod(tokens, m_tetherBaseEffectOverride, "TetherEffect", sparkHealingBeam.m_laserHitEffect);
		AbilityMod.AddToken(tokens, m_tetherDurationMod, "TetherDuration", string.Empty, sparkHealingBeam.m_tetherDuration);
		AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "Laser", sparkHealingBeam.m_laserInfo);
		if (m_xDamageThreshold > 0)
		{
			tokens.Add(new TooltipTokenInt("XDamageThresholdForEffect", "how much damage to trigger extra effect on target", m_xDamageThreshold));
			AbilityMod.AddToken_EffectInfo(tokens, m_effectOnTargetForTakingXDamage, "EffectForTakingXDamage");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkHealingBeam sparkHealingBeam = GetTargetAbilityOnAbilityData(abilityData) as SparkHealingBeam;
		bool flag = sparkHealingBeam != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt initialHealingMod = m_initialHealingMod;
		int baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = sparkHealingBeam.m_laserHealingAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(initialHealingMod, "[Initial Heal On Attach]", flag, baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_healPerTurnMod, "[Heal per Turn]", flag, flag ? sparkHealingBeam.m_laserHitEffect.m_effectData.m_healingPerTurn : 0);
		string str2 = empty;
		AbilityModPropertyInt additionalHealOnRadiatedMod = m_additionalHealOnRadiatedMod;
		int baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = sparkHealingBeam.m_additionalEnergizedHealing;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(additionalHealOnRadiatedMod, "[Additional Healing on Radiated]", flag, baseVal2);
		if (m_useBonusHealOverTime)
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
			empty += "Using Bonus Heal Over Time (please remember to put in a max cap)\n";
			empty += AbilityModHelper.GetModPropertyDesc(m_bonusAllyHealIncreaseRate, "[Bonus Ally Heal Increase Rate]", flag);
			empty += AbilityModHelper.GetModPropertyDesc(m_maxAllyBonusHealAmount, "[Max Bonus Ally Heal Amount]", flag);
		}
		empty += AbilityModHelper.GetModPropertyDesc(m_healOnCasterOnTickMod, "[Heal on Caster per Turn]", flag, flag ? sparkHealingBeam.m_healOnSelfOnTick : 0);
		string str3 = empty;
		AbilityModPropertyInt energyOnCasterPerTurnMod = m_energyOnCasterPerTurnMod;
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
			baseVal3 = sparkHealingBeam.m_energyOnCasterPerTurn;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(energyOnCasterPerTurnMod, "[EnergyOnCasterPerTurn]", flag, baseVal3);
		empty += AbilityModHelper.GetModPropertyDesc(m_tetherDistanceMod, "[Tether Distance]", flag, (!flag) ? 0f : sparkHealingBeam.m_tetherDistance);
		string str4 = empty;
		AbilityModPropertyEffectInfo tetherBaseEffectOverride = m_tetherBaseEffectOverride;
		object baseVal4;
		if (flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = sparkHealingBeam.m_laserHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(tetherBaseEffectOverride, "{ Tether Base Effect Override }", flag, (StandardEffectInfo)baseVal4);
		string str5 = empty;
		AbilityModPropertyInt tetherDurationMod = m_tetherDurationMod;
		int baseVal5;
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
			baseVal5 = sparkHealingBeam.m_tetherDuration;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(tetherDurationMod, "[TetherDuration]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyLaserInfo laserInfoMod = m_laserInfoMod;
		object baseLaserInfo;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseLaserInfo = sparkHealingBeam.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		empty = str6 + AbilityModHelper.GetModPropertyDesc(laserInfoMod, "LaserInfo", flag, (LaserTargetingInfo)baseLaserInfo);
		if (m_xDamageThreshold > 0)
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
			string text = empty;
			empty = text + "Applying Effect for taking X Damage, threshold = " + m_xDamageThreshold + "\n";
			empty += AbilityModHelper.GetModEffectInfoDesc(m_effectOnTargetForTakingXDamage, "{ Effect on Target for Taking X Damage }", string.Empty, flag);
		}
		return empty;
	}
}
