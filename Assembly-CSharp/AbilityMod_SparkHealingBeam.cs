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
		AddToken(tokens, m_initialHealingMod, "Heal_FirstTurn", "heal on first turn", sparkHealingBeam.m_laserHealingAmount);
		AddToken(tokens, m_healPerTurnMod, "Heal_PerTurnAfterFirst", "heal per turn after first turn", sparkHealingBeam.m_laserHitEffect.m_effectData.m_healingPerTurn);
		AddToken(tokens, m_additionalHealOnRadiatedMod, "Heal_AdditionalOnRadiated", "additional damage on Radiated", sparkHealingBeam.m_additionalEnergizedHealing);
		if (m_useBonusHealOverTime)
		{
			AddToken(tokens, m_bonusAllyHealIncreaseRate, "BonusAllyHeal_GrowthRate", "increase in bonus heal per turn", 0);
			AddToken(tokens, m_maxAllyBonusHealAmount, "BonusAllyHeal_MaxHealAmount", "max bonus heal amount", 0);
		}
		AddToken(tokens, m_healOnCasterOnTickMod, "Heal_OnCasterPerTurn", "heal on caster per turn", sparkHealingBeam.m_healOnSelfOnTick);
		AddToken(tokens, m_energyOnCasterPerTurnMod, "EnergyOnCasterPerTurn", "", sparkHealingBeam.m_energyOnCasterPerTurn);
		AddToken(tokens, m_tetherDistanceMod, "TetherDistance", "tether distance before breaking", sparkHealingBeam.m_tetherDistance);
		AddToken_EffectMod(tokens, m_tetherBaseEffectOverride, "TetherEffect", sparkHealingBeam.m_laserHitEffect);
		AddToken(tokens, m_tetherDurationMod, "TetherDuration", "", sparkHealingBeam.m_tetherDuration);
		AddToken_LaserInfo(tokens, m_laserInfoMod, "Laser", sparkHealingBeam.m_laserInfo);
		if (m_xDamageThreshold > 0)
		{
			tokens.Add(new TooltipTokenInt("XDamageThresholdForEffect", "how much damage to trigger extra effect on target", m_xDamageThreshold));
			AddToken_EffectInfo(tokens, m_effectOnTargetForTakingXDamage, "EffectForTakingXDamage");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkHealingBeam sparkHealingBeam = GetTargetAbilityOnAbilityData(abilityData) as SparkHealingBeam;
		bool isAbilityPresent = sparkHealingBeam != null;
		string desc = "";
		desc += AbilityModHelper.GetModPropertyDesc(m_initialHealingMod, "[Initial Heal On Attach]", isAbilityPresent, isAbilityPresent ? sparkHealingBeam.m_laserHealingAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_healPerTurnMod, "[Heal per Turn]", isAbilityPresent, isAbilityPresent ? sparkHealingBeam.m_laserHitEffect.m_effectData.m_healingPerTurn : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_additionalHealOnRadiatedMod, "[Additional Healing on Radiated]", isAbilityPresent, isAbilityPresent ? sparkHealingBeam.m_additionalEnergizedHealing : 0);
		if (m_useBonusHealOverTime)
		{
			desc += "Using Bonus Heal Over Time (please remember to put in a max cap)\n";
			desc += AbilityModHelper.GetModPropertyDesc(m_bonusAllyHealIncreaseRate, "[Bonus Ally Heal Increase Rate]", isAbilityPresent);
			desc += AbilityModHelper.GetModPropertyDesc(m_maxAllyBonusHealAmount, "[Max Bonus Ally Heal Amount]", isAbilityPresent);
		}
		desc += AbilityModHelper.GetModPropertyDesc(m_healOnCasterOnTickMod, "[Heal on Caster per Turn]", isAbilityPresent, isAbilityPresent ? sparkHealingBeam.m_healOnSelfOnTick : 0);
		desc += PropDesc(m_energyOnCasterPerTurnMod, "[EnergyOnCasterPerTurn]", isAbilityPresent, isAbilityPresent ? sparkHealingBeam.m_energyOnCasterPerTurn : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_tetherDistanceMod, "[Tether Distance]", isAbilityPresent, isAbilityPresent ? sparkHealingBeam.m_tetherDistance : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_tetherBaseEffectOverride, "{ Tether Base Effect Override }", isAbilityPresent, isAbilityPresent ? sparkHealingBeam.m_laserHitEffect : null);
		desc += PropDesc(m_tetherDurationMod, "[TetherDuration]", isAbilityPresent, isAbilityPresent ? sparkHealingBeam.m_tetherDuration : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserInfoMod, "LaserInfo", isAbilityPresent, isAbilityPresent ? sparkHealingBeam.m_laserInfo : null);
		if (m_xDamageThreshold > 0)
		{
			desc += "Applying Effect for taking X Damage, threshold = " + m_xDamageThreshold + "\n";
			desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnTargetForTakingXDamage, "{ Effect on Target for Taking X Damage }", "", isAbilityPresent);
		}
		return desc;
	}
}
