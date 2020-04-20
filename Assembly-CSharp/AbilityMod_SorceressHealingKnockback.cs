using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SorceressHealingKnockback : AbilityMod
{
	[Header("-- Healing")]
	public float m_lowHealthThreshold;

	public AbilityModPropertyInt m_normalHealingMod;

	public AbilityModPropertyInt m_lowHealthHealingMod;

	public AbilityModPropertyInt m_onCastAllyEnergyGainMod;

	[Header("-- Damage and Effect Mod")]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectOverride;

	[Header("-- Knockback")]
	public AbilityModPropertyFloat m_knockbackDistanceMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SorceressHealingKnockback);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SorceressHealingKnockback sorceressHealingKnockback = targetAbility as SorceressHealingKnockback;
		if (sorceressHealingKnockback != null)
		{
			AbilityMod.AddToken(tokens, this.m_normalHealingMod, "OnCastHealAmount_Normal", string.Empty, sorceressHealingKnockback.m_onCastHealAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_lowHealthHealingMod, "OnCastHealAmount_LowHealth", string.Empty, sorceressHealingKnockback.m_onCastHealAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_onCastAllyEnergyGainMod, "OnCastAllyEnergyGain", string.Empty, sorceressHealingKnockback.m_onCastAllyEnergyGain, true, false);
			AbilityMod.AddToken(tokens, this.m_damageMod, "OnDetonateDamageAmount", string.Empty, sorceressHealingKnockback.m_onDetonateDamageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectOverride, "OnDetonateEnemyEffect", sorceressHealingKnockback.m_onDetonateEnemyEffect, true);
			AbilityMod.AddToken(tokens, this.m_knockbackDistanceMod, "KnockbackDistance", string.Empty, sorceressHealingKnockback.m_knockbackDistance, true, false, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SorceressHealingKnockback sorceressHealingKnockback = base.GetTargetAbilityOnAbilityData(abilityData) as SorceressHealingKnockback;
		bool flag = sorceressHealingKnockback != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt normalHealingMod = this.m_normalHealingMod;
		string prefix = "[Normal Healing]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = sorceressHealingKnockback.m_onCastHealAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(normalHealingMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt lowHealthHealingMod = this.m_lowHealthHealingMod;
		string prefix2 = "[Healing when Low Health]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = sorceressHealingKnockback.m_onCastHealAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(lowHealthHealingMod, prefix2, showBaseVal2, baseVal2);
		if (this.m_lowHealthThreshold > 0f)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"Health considered Low if portion is below ",
				this.m_lowHealthThreshold,
				"\n"
			});
		}
		else if (this.m_lowHealthHealingMod != null)
		{
			if (this.m_lowHealthHealingMod.operation != AbilityModPropertyInt.ModOp.Ignore)
			{
				text += "Low Health Threshold not used, ignore [Healing when Low Health]\n";
			}
		}
		string str3 = text;
		AbilityModPropertyInt onCastAllyEnergyGainMod = this.m_onCastAllyEnergyGainMod;
		string prefix3 = "[OnCastAllyEnergyGain]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = sorceressHealingKnockback.m_onCastAllyEnergyGain;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(onCastAllyEnergyGainMod, prefix3, showBaseVal3, baseVal3);
		text += AbilityModHelper.GetModPropertyDesc(this.m_damageMod, "[Damage]", flag, (!flag) ? 0 : sorceressHealingKnockback.m_onDetonateDamageAmount);
		string str4 = text;
		AbilityModPropertyEffectInfo enemyHitEffectOverride = this.m_enemyHitEffectOverride;
		string prefix4 = "{ Enemy Hit Effect Override }";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
		if (flag)
		{
			baseVal4 = sorceressHealingKnockback.m_onDetonateEnemyEffect;
		}
		else
		{
			baseVal4 = null;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(enemyHitEffectOverride, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyFloat knockbackDistanceMod = this.m_knockbackDistanceMod;
		string prefix5 = "[Knockback Distance]";
		bool showBaseVal5 = flag;
		float baseVal5;
		if (flag)
		{
			baseVal5 = sorceressHealingKnockback.m_knockbackDistance;
		}
		else
		{
			baseVal5 = 0f;
		}
		return str5 + AbilityModHelper.GetModPropertyDesc(knockbackDistanceMod, prefix5, showBaseVal5, baseVal5);
	}
}
