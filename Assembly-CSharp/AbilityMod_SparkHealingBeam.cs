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
		if (sparkHealingBeam != null)
		{
			AbilityMod.AddToken(tokens, this.m_initialHealingMod, "Heal_FirstTurn", "heal on first turn", sparkHealingBeam.m_laserHealingAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_healPerTurnMod, "Heal_PerTurnAfterFirst", "heal per turn after first turn", sparkHealingBeam.m_laserHitEffect.m_effectData.m_healingPerTurn, true, false);
			AbilityMod.AddToken(tokens, this.m_additionalHealOnRadiatedMod, "Heal_AdditionalOnRadiated", "additional damage on Radiated", sparkHealingBeam.m_additionalEnergizedHealing, true, false);
			if (this.m_useBonusHealOverTime)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SparkHealingBeam.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
				}
				AbilityMod.AddToken(tokens, this.m_bonusAllyHealIncreaseRate, "BonusAllyHeal_GrowthRate", "increase in bonus heal per turn", 0, true, false);
				AbilityMod.AddToken(tokens, this.m_maxAllyBonusHealAmount, "BonusAllyHeal_MaxHealAmount", "max bonus heal amount", 0, true, false);
			}
			AbilityMod.AddToken(tokens, this.m_healOnCasterOnTickMod, "Heal_OnCasterPerTurn", "heal on caster per turn", sparkHealingBeam.m_healOnSelfOnTick, true, false);
			AbilityMod.AddToken(tokens, this.m_energyOnCasterPerTurnMod, "EnergyOnCasterPerTurn", string.Empty, sparkHealingBeam.m_energyOnCasterPerTurn, true, false);
			AbilityMod.AddToken(tokens, this.m_tetherDistanceMod, "TetherDistance", "tether distance before breaking", sparkHealingBeam.m_tetherDistance, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_tetherBaseEffectOverride, "TetherEffect", sparkHealingBeam.m_laserHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_tetherDurationMod, "TetherDuration", string.Empty, sparkHealingBeam.m_tetherDuration, true, false);
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "Laser", sparkHealingBeam.m_laserInfo, true);
			if (this.m_xDamageThreshold > 0)
			{
				tokens.Add(new TooltipTokenInt("XDamageThresholdForEffect", "how much damage to trigger extra effect on target", this.m_xDamageThreshold));
				AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnTargetForTakingXDamage, "EffectForTakingXDamage", null, true);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkHealingBeam sparkHealingBeam = base.GetTargetAbilityOnAbilityData(abilityData) as SparkHealingBeam;
		bool flag = sparkHealingBeam != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt initialHealingMod = this.m_initialHealingMod;
		string prefix = "[Initial Heal On Attach]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SparkHealingBeam.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = sparkHealingBeam.m_laserHealingAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(initialHealingMod, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_healPerTurnMod, "[Heal per Turn]", flag, (!flag) ? 0 : sparkHealingBeam.m_laserHitEffect.m_effectData.m_healingPerTurn);
		string str2 = text;
		AbilityModPropertyInt additionalHealOnRadiatedMod = this.m_additionalHealOnRadiatedMod;
		string prefix2 = "[Additional Healing on Radiated]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = sparkHealingBeam.m_additionalEnergizedHealing;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(additionalHealOnRadiatedMod, prefix2, showBaseVal2, baseVal2);
		if (this.m_useBonusHealOverTime)
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
			text += "Using Bonus Heal Over Time (please remember to put in a max cap)\n";
			text += AbilityModHelper.GetModPropertyDesc(this.m_bonusAllyHealIncreaseRate, "[Bonus Ally Heal Increase Rate]", flag, 0);
			text += AbilityModHelper.GetModPropertyDesc(this.m_maxAllyBonusHealAmount, "[Max Bonus Ally Heal Amount]", flag, 0);
		}
		text += AbilityModHelper.GetModPropertyDesc(this.m_healOnCasterOnTickMod, "[Heal on Caster per Turn]", flag, (!flag) ? 0 : sparkHealingBeam.m_healOnSelfOnTick);
		string str3 = text;
		AbilityModPropertyInt energyOnCasterPerTurnMod = this.m_energyOnCasterPerTurnMod;
		string prefix3 = "[EnergyOnCasterPerTurn]";
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
			baseVal3 = sparkHealingBeam.m_energyOnCasterPerTurn;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(energyOnCasterPerTurnMod, prefix3, showBaseVal3, baseVal3);
		text += AbilityModHelper.GetModPropertyDesc(this.m_tetherDistanceMod, "[Tether Distance]", flag, (!flag) ? 0f : sparkHealingBeam.m_tetherDistance);
		string str4 = text;
		AbilityModPropertyEffectInfo tetherBaseEffectOverride = this.m_tetherBaseEffectOverride;
		string prefix4 = "{ Tether Base Effect Override }";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
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
			baseVal4 = sparkHealingBeam.m_laserHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(tetherBaseEffectOverride, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt tetherDurationMod = this.m_tetherDurationMod;
		string prefix5 = "[TetherDuration]";
		bool showBaseVal5 = flag;
		int baseVal5;
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
			baseVal5 = sparkHealingBeam.m_tetherDuration;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(tetherDurationMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyLaserInfo laserInfoMod = this.m_laserInfoMod;
		string prefix6 = "LaserInfo";
		bool showBaseVal6 = flag;
		LaserTargetingInfo baseLaserInfo;
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
			baseLaserInfo = sparkHealingBeam.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		text = str6 + AbilityModHelper.GetModPropertyDesc(laserInfoMod, prefix6, showBaseVal6, baseLaserInfo);
		if (this.m_xDamageThreshold > 0)
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
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"Applying Effect for taking X Damage, threshold = ",
				this.m_xDamageThreshold,
				"\n"
			});
			text += AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnTargetForTakingXDamage, "{ Effect on Target for Taking X Damage }", string.Empty, flag, null);
		}
		return text;
	}
}
