using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken(tokens, m_normalHealingMod, "OnCastHealAmount_Normal", string.Empty, sorceressHealingKnockback.m_onCastHealAmount);
			AddToken(tokens, m_lowHealthHealingMod, "OnCastHealAmount_LowHealth", string.Empty, sorceressHealingKnockback.m_onCastHealAmount);
			AddToken(tokens, m_onCastAllyEnergyGainMod, "OnCastAllyEnergyGain", string.Empty, sorceressHealingKnockback.m_onCastAllyEnergyGain);
			AddToken(tokens, m_damageMod, "OnDetonateDamageAmount", string.Empty, sorceressHealingKnockback.m_onDetonateDamageAmount);
			AddToken_EffectMod(tokens, m_enemyHitEffectOverride, "OnDetonateEnemyEffect", sorceressHealingKnockback.m_onDetonateEnemyEffect);
			AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", string.Empty, sorceressHealingKnockback.m_knockbackDistance);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SorceressHealingKnockback sorceressHealingKnockback = GetTargetAbilityOnAbilityData(abilityData) as SorceressHealingKnockback;
		bool isAbilityPresent = sorceressHealingKnockback != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_normalHealingMod, "[Normal Healing]", isAbilityPresent, isAbilityPresent ? sorceressHealingKnockback.m_onCastHealAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_lowHealthHealingMod, "[Healing when Low Health]", isAbilityPresent, isAbilityPresent ? sorceressHealingKnockback.m_onCastHealAmount : 0);
		if (m_lowHealthThreshold > 0f)
		{
			desc += new StringBuilder().Append("Health considered Low if portion is below ").Append(m_lowHealthThreshold).Append("\n").ToString();
		}
		else if (m_lowHealthHealingMod != null && m_lowHealthHealingMod.operation != AbilityModPropertyInt.ModOp.Ignore)
		{
			desc += "Low Health Threshold not used, ignore [Healing when Low Health]\n";
		}
		desc += PropDesc(m_onCastAllyEnergyGainMod, "[OnCastAllyEnergyGain]", isAbilityPresent, isAbilityPresent ? sorceressHealingKnockback.m_onCastAllyEnergyGain : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? sorceressHealingKnockback.m_onDetonateDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_enemyHitEffectOverride, "{ Enemy Hit Effect Override }", isAbilityPresent, isAbilityPresent ? sorceressHealingKnockback.m_onDetonateEnemyEffect : null);
		return new StringBuilder().Append(desc).Append(AbilityModHelper.GetModPropertyDesc(m_knockbackDistanceMod, "[Knockback Distance]", isAbilityPresent, isAbilityPresent ? sorceressHealingKnockback.m_knockbackDistance : 0f)).ToString();
	}
}
