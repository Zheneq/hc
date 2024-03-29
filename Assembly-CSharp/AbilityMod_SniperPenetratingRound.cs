﻿// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SniperPenetratingRound : AbilityMod
{
	[Header("-- Laser Mods")]
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_laserRangeMod;
	[Header("-- Enemy Hit Effect Override")]
	public bool m_useEnemyHitEffectOverride;
	public StandardEffectInfo m_enemyHitEffectOverride;
	[Header("-- Knockback")]
	public bool m_knockbackHitEnemy;
	public float m_knockbackThresholdDistance = -1f;
	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;
	public float m_knockbackDistance;
	[Header("-- Damage")]
	public AbilityModPropertyInt m_laserDamage;
	[Header("-- Bonus Damage from Target Health Threshold --")]
	public AbilityModPropertyInt m_additionalDamageOnLowHealthTargetMod;
	public AbilityModPropertyFloat m_lowHealthThresholdMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SniperPenetratingRound);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SniperPenetratingRound sniperPenetratingRound = targetAbility as SniperPenetratingRound;
		if (sniperPenetratingRound != null)
		{
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, sniperPenetratingRound.m_laserInfo.width);
			AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, sniperPenetratingRound.m_laserInfo.range);
			if (m_useEnemyHitEffectOverride)
			{
				AddToken_EffectInfo(tokens, m_enemyHitEffectOverride, "EnemyHitEffect", sniperPenetratingRound.m_laserHitEffect);
			}
			AddToken(tokens, m_laserDamage, "Damage", "base damage", sniperPenetratingRound.m_laserDamageAmount);
			AddToken(tokens, m_additionalDamageOnLowHealthTargetMod, "AdditionalDamageOnLowHealthTarget", string.Empty, sniperPenetratingRound.m_additionalDamageOnLowHealthTarget);
			AddToken(tokens, m_lowHealthThresholdMod, "LowHealthThreshold", string.Empty, sniperPenetratingRound.m_lowHealthThreshold);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		SniperPenetratingRound sniperPenetratingRound = GetTargetAbilityOnAbilityData(abilityData) as SniperPenetratingRound;
		// rogues
		// SniperPenetratingRound sniperPenetratingRound = targetAbility as SniperPenetratingRound;
		bool isValid = sniperPenetratingRound != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_laserWidthMod, "[Laser Width]", isValid, isValid ? sniperPenetratingRound.m_laserInfo.width : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserRangeMod, "[Laser Range]", isValid, isValid ? sniperPenetratingRound.m_laserInfo.range : 0f);
		if (m_useEnemyHitEffectOverride)
		{
			desc += AbilityModHelper.GetModEffectInfoDesc(m_enemyHitEffectOverride, "{ Enemy Hit Effect Override }", string.Empty, isValid, isValid ? sniperPenetratingRound.m_laserHitEffect : null);
		}
		if (m_knockbackHitEnemy)
		{
			desc += "Can knock back hit enemy within " + m_knockbackThresholdDistance + " squares\n";
		}
		desc += AbilityModHelper.GetModPropertyDesc(m_laserDamage, "[Laser Damage]", isValid, isValid ? sniperPenetratingRound.m_laserDamageAmount : 0);
		desc += PropDesc(m_additionalDamageOnLowHealthTargetMod, "[AdditionalDamageOnLowHealthTarget]", isValid, isValid ? sniperPenetratingRound.m_additionalDamageOnLowHealthTarget : 0);
		return desc + PropDesc(m_lowHealthThresholdMod, "[LowHealthThreshold]", isValid, isValid ? sniperPenetratingRound.m_lowHealthThreshold : 0f);
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		if (m_useEnemyHitEffectOverride && m_enemyHitEffectOverride.m_applyEffect)
		{
			numbers.Add(m_enemyHitEffectOverride.m_effectData.m_duration - 1);
		}
	}
}
