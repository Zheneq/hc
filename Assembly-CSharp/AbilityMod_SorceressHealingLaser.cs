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
		if (!(sorceressHealingLaser != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, sorceressHealingLaser.m_damageAmount);
			AbilityMod.AddToken(tokens, m_minDamageMod, "MinDamageAmount", string.Empty, sorceressHealingLaser.m_minDamageAmount);
			AbilityMod.AddToken(tokens, m_damageChangePerHitMod, "DamageChangePerHit", string.Empty, sorceressHealingLaser.m_damageChangePerHit);
			AbilityMod.AddToken(tokens, m_selfHealMod, "SelfHealAmount", string.Empty, sorceressHealingLaser.m_selfHealAmount);
			AbilityMod.AddToken(tokens, m_allyHealMod, "AllyHealAmount", string.Empty, sorceressHealingLaser.m_allyHealAmount);
			AbilityMod.AddToken(tokens, m_minHealMod, "MinHealAmount", string.Empty, sorceressHealingLaser.m_minHealAmount);
			AbilityMod.AddToken(tokens, m_healChangePerHitMod, "HealChangePerHit", string.Empty, sorceressHealingLaser.m_healChangePerHit);
			AbilityMod.AddToken(tokens, m_allyTechPointGain, "AllyTechPointGain", string.Empty, 0);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SorceressHealingLaser sorceressHealingLaser = GetTargetAbilityOnAbilityData(abilityData) as SorceressHealingLaser;
		bool flag = sorceressHealingLaser != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt selfHealMod = m_selfHealMod;
		int baseVal;
		if (flag)
		{
			baseVal = sorceressHealingLaser.m_selfHealAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(selfHealMod, "[Self Heal]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt allyHealMod = m_allyHealMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = sorceressHealingLaser.m_allyHealAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(allyHealMod, "[Ally Heal]", flag, baseVal2);
		empty += AbilityModHelper.GetModPropertyDesc(m_minHealMod, "[Min Heal]", flag, flag ? sorceressHealingLaser.m_minHealAmount : 0);
		string str3 = empty;
		AbilityModPropertyInt healChangePerHitMod = m_healChangePerHitMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = sorceressHealingLaser.m_healChangePerHit;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(healChangePerHitMod, "[Heal Change Per Hit]", flag, baseVal3);
		empty += PropDesc(m_allyTechPointGain, "[Ally Tech Point Gain]", flag);
		string str4 = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = sorceressHealingLaser.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(damageMod, "[Damage]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt minDamageMod = m_minDamageMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = sorceressHealingLaser.m_minDamageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(minDamageMod, "[Min Damage]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt damageChangePerHitMod = m_damageChangePerHitMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = sorceressHealingLaser.m_damageChangePerHit;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + AbilityModHelper.GetModPropertyDesc(damageChangePerHitMod, "[Damage Change Per Hit]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
		float baseVal7;
		if (flag)
		{
			baseVal7 = sorceressHealingLaser.m_width;
		}
		else
		{
			baseVal7 = 0f;
		}
		empty = str7 + AbilityModHelper.GetModPropertyDesc(laserWidthMod, "[Laser Width]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyFloat laserRangeMod = m_laserRangeMod;
		float baseVal8;
		if (flag)
		{
			baseVal8 = sorceressHealingLaser.m_distance;
		}
		else
		{
			baseVal8 = 0f;
		}
		return str8 + AbilityModHelper.GetModPropertyDesc(laserRangeMod, "[Laser Range]", flag, baseVal8);
	}
}
