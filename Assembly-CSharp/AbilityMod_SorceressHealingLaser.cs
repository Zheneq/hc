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
			AbilityMod.AddToken(tokens, this.m_damageMod, "DamageAmount", string.Empty, sorceressHealingLaser.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_minDamageMod, "MinDamageAmount", string.Empty, sorceressHealingLaser.m_minDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_damageChangePerHitMod, "DamageChangePerHit", string.Empty, sorceressHealingLaser.m_damageChangePerHit, true, false);
			AbilityMod.AddToken(tokens, this.m_selfHealMod, "SelfHealAmount", string.Empty, sorceressHealingLaser.m_selfHealAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_allyHealMod, "AllyHealAmount", string.Empty, sorceressHealingLaser.m_allyHealAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_minHealMod, "MinHealAmount", string.Empty, sorceressHealingLaser.m_minHealAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_healChangePerHitMod, "HealChangePerHit", string.Empty, sorceressHealingLaser.m_healChangePerHit, true, false);
			AbilityMod.AddToken(tokens, this.m_allyTechPointGain, "AllyTechPointGain", string.Empty, 0, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SorceressHealingLaser sorceressHealingLaser = base.GetTargetAbilityOnAbilityData(abilityData) as SorceressHealingLaser;
		bool flag = sorceressHealingLaser != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt selfHealMod = this.m_selfHealMod;
		string prefix = "[Self Heal]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = sorceressHealingLaser.m_selfHealAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(selfHealMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt allyHealMod = this.m_allyHealMod;
		string prefix2 = "[Ally Heal]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = sorceressHealingLaser.m_allyHealAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(allyHealMod, prefix2, showBaseVal2, baseVal2);
		text += AbilityModHelper.GetModPropertyDesc(this.m_minHealMod, "[Min Heal]", flag, (!flag) ? 0 : sorceressHealingLaser.m_minHealAmount);
		string str3 = text;
		AbilityModPropertyInt healChangePerHitMod = this.m_healChangePerHitMod;
		string prefix3 = "[Heal Change Per Hit]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = sorceressHealingLaser.m_healChangePerHit;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(healChangePerHitMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_allyTechPointGain, "[Ally Tech Point Gain]", flag, 0);
		string str4 = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix4 = "[Damage]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = sorceressHealingLaser.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(damageMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt minDamageMod = this.m_minDamageMod;
		string prefix5 = "[Min Damage]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = sorceressHealingLaser.m_minDamageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(minDamageMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt damageChangePerHitMod = this.m_damageChangePerHitMod;
		string prefix6 = "[Damage Change Per Hit]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = sorceressHealingLaser.m_damageChangePerHit;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + AbilityModHelper.GetModPropertyDesc(damageChangePerHitMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix7 = "[Laser Width]";
		bool showBaseVal7 = flag;
		float baseVal7;
		if (flag)
		{
			baseVal7 = sorceressHealingLaser.m_width;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str7 + AbilityModHelper.GetModPropertyDesc(laserWidthMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix8 = "[Laser Range]";
		bool showBaseVal8 = flag;
		float baseVal8;
		if (flag)
		{
			baseVal8 = sorceressHealingLaser.m_distance;
		}
		else
		{
			baseVal8 = 0f;
		}
		return str8 + AbilityModHelper.GetModPropertyDesc(laserRangeMod, prefix8, showBaseVal8, baseVal8);
	}
}
