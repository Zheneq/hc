// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SorceressHealingLaser : AbilityMod
{
	[Header("-- Heal Mod")]
	public AbilityModPropertyInt m_selfHealMod;
	public AbilityModPropertyInt m_allyHealMod;
	public AbilityModPropertyInt m_minHealMod;
	public AbilityModPropertyInt m_healChangePerHitMod;
	public AbilityModPropertyInt m_allyTechPointGain;
	[Header("-- Damage Mod")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyInt m_minDamageMod;
	public AbilityModPropertyInt m_damageChangePerHitMod;
	[Header("-- Laser Size Mod")]
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_laserRangeMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SorceressHealingLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SorceressHealingLaser sorceressHealingLaser = targetAbility as SorceressHealingLaser;
		if (sorceressHealingLaser != null)
		{
			AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, sorceressHealingLaser.m_damageAmount);
			AddToken(tokens, m_minDamageMod, "MinDamageAmount", string.Empty, sorceressHealingLaser.m_minDamageAmount);
			AddToken(tokens, m_damageChangePerHitMod, "DamageChangePerHit", string.Empty, sorceressHealingLaser.m_damageChangePerHit);
			AddToken(tokens, m_selfHealMod, "SelfHealAmount", string.Empty, sorceressHealingLaser.m_selfHealAmount);
			AddToken(tokens, m_allyHealMod, "AllyHealAmount", string.Empty, sorceressHealingLaser.m_allyHealAmount);
			AddToken(tokens, m_minHealMod, "MinHealAmount", string.Empty, sorceressHealingLaser.m_minHealAmount);
			AddToken(tokens, m_healChangePerHitMod, "HealChangePerHit", string.Empty, sorceressHealingLaser.m_healChangePerHit);
			AddToken(tokens, m_allyTechPointGain, "AllyTechPointGain", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		SorceressHealingLaser sorceressHealingLaser = GetTargetAbilityOnAbilityData(abilityData) as SorceressHealingLaser;
		// rogues
		//SorceressHealingLaser sorceressHealingLaser = targetAbility as SorceressHealingLaser;
		bool isAbilityPresent = sorceressHealingLaser != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_selfHealMod, "[Self Heal]", isAbilityPresent, isAbilityPresent ? sorceressHealingLaser.m_selfHealAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_allyHealMod, "[Ally Heal]", isAbilityPresent, isAbilityPresent ? sorceressHealingLaser.m_allyHealAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_minHealMod, "[Min Heal]", isAbilityPresent, isAbilityPresent ? sorceressHealingLaser.m_minHealAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_healChangePerHitMod, "[Heal Change Per Hit]", isAbilityPresent, isAbilityPresent ? sorceressHealingLaser.m_healChangePerHit : 0);
		desc += PropDesc(m_allyTechPointGain, "[Ally Tech Point Gain]", isAbilityPresent);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? sorceressHealingLaser.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_minDamageMod, "[Min Damage]", isAbilityPresent, isAbilityPresent ? sorceressHealingLaser.m_minDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageChangePerHitMod, "[Damage Change Per Hit]", isAbilityPresent, isAbilityPresent ? sorceressHealingLaser.m_damageChangePerHit : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserWidthMod, "[Laser Width]", isAbilityPresent, isAbilityPresent ? sorceressHealingLaser.m_width : 0f);
		return desc + AbilityModHelper.GetModPropertyDesc(m_laserRangeMod, "[Laser Range]", isAbilityPresent, isAbilityPresent ? sorceressHealingLaser.m_distance : 0f);
	}
}
