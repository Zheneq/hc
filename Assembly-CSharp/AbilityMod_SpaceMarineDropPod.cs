using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_SpaceMarineDropPod : AbilityMod
{
	[Header("-- Damage and Knockback Distance")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyFloat m_knockbackDistanceMod;
	[Header("-- Energy Refund if no hit")]
	public AbilityModPropertyInt m_energyRefundIfNoEnemyHitMod;
	[Header(" -- Power Up Heal and Energy Mod")]
	public AbilityModPropertyInt m_powerupHealMod;
	public AbilityModPropertyInt m_powerupTechPointMod;
	[Header("-- Ground Effect on Drop Pod")]
	public StandardGroundEffectInfo m_groundEffectInfoOnDropPod;
	[Space(10f)]
	public AbilityModPropertyInt m_extraPowerupHealIfDirectHitMod;
	public AbilityModPropertyInt m_extraPowerupEnergyIfDirectHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SpaceMarineDropPod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SpaceMarineDropPod spaceMarineDropPod = targetAbility as SpaceMarineDropPod;
		if (spaceMarineDropPod != null)
		{
			AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, spaceMarineDropPod.m_damageAmount);
			AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", string.Empty, spaceMarineDropPod.m_knockbackDistance);
			AddToken(tokens, m_energyRefundIfNoEnemyHitMod, "EnergyRefundIfNoEnemyHit", string.Empty, spaceMarineDropPod.m_energyRefundIfNoEnemyHit);
			AddToken(tokens, m_extraPowerupHealIfDirectHitMod, "ExtraPowerupHealIfDirectHit", string.Empty, spaceMarineDropPod.m_extraPowerupHealIfDirectHit);
			AddToken(tokens, m_extraPowerupEnergyIfDirectHitMod, "ExtraPowerupEnergy", string.Empty, spaceMarineDropPod.m_extraPowerupEnergyIfDirectHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SpaceMarineDropPod spaceMarineDropPod = GetTargetAbilityOnAbilityData(abilityData) as SpaceMarineDropPod;
		bool isValid = spaceMarineDropPod != null;
		int powerupHealMod = 0;
		int powerupTechPointMod = 0;
		if (isValid && spaceMarineDropPod.m_powerupPrefab != null)
		{
			PowerUp_Standard_Ability component = spaceMarineDropPod.m_powerupPrefab.GetComponent<PowerUp_Standard_Ability>();
			if (component != null)
			{
				powerupHealMod = component.m_healAmount;
				powerupTechPointMod = component.m_techPointsAmount;
			}
		}
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isValid, isValid ? spaceMarineDropPod.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_knockbackDistanceMod, "[Knockback Distance]", isValid, isValid ? spaceMarineDropPod.m_knockbackDistance : 0f);
		desc += PropDesc(m_energyRefundIfNoEnemyHitMod, "[EnergyRefundIfNoEnemyHit]", isValid, isValid ? spaceMarineDropPod.m_energyRefundIfNoEnemyHit : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_powerupHealMod, "[PowerUp Heal Amount]", isValid, isValid ? powerupHealMod : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_powerupTechPointMod, "[PowerUp TechPoint Amount]", isValid, isValid ? powerupTechPointMod : 0);
		desc += AbilityModHelper.GetModGroundEffectInfoDesc(m_groundEffectInfoOnDropPod, "-- Ground Effect on Drop Pod Location --", isValid);
		desc += PropDesc(m_extraPowerupHealIfDirectHitMod, "[ExtraPowerupHealIfDirectHit]", isValid, isValid ? spaceMarineDropPod.m_extraPowerupHealIfDirectHit : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_extraPowerupEnergyIfDirectHitMod, "[ExtraPowerupEnergy]", isValid, isValid ? spaceMarineDropPod.m_extraPowerupEnergyIfDirectHit : 0)).ToString();
	}
}
