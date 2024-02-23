using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_SniperCripplingRound : AbilityMod
{
	[Header("-- Damage Mod")]
	public AbilityModPropertyInt m_laserDamageMod;
	public AbilityModPropertyInt m_explosionDamageMod;
	[Header("-- Effect Duration Mod and Additional Effect")]
	public AbilityModPropertyInt m_enemyHitEffectDurationMod;
	public StandardEffectInfo m_additionalEnemyHitEffect;
	[Header("-- Max targets hit by laser. Explosion only on first.")]
	public AbilityModPropertyInt m_maxTargetsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SniperCripplingRound);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SniperCripplingRound sniperCripplingRound = targetAbility as SniperCripplingRound;
		if (sniperCripplingRound != null)
		{
			AddToken(tokens, m_laserDamageMod, "Damage_Laser", string.Empty, sniperCripplingRound.m_laserDamageAmount);
			AddToken(tokens, m_explosionDamageMod, "Damage_Explosion", string.Empty, sniperCripplingRound.m_explosionDamageAmount);
			AddToken(tokens, m_enemyHitEffectDurationMod, "EffectDuration", string.Empty, sniperCripplingRound.m_effectOnLaserHitTargets.m_effectData.m_duration);
			AddToken_EffectInfo(tokens, m_additionalEnemyHitEffect, "AdditionalEffect", null, false);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, 1);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SniperCripplingRound sniperCripplingRound = GetTargetAbilityOnAbilityData(abilityData) as SniperCripplingRound;
		bool isValid = sniperCripplingRound != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_laserDamageMod, "[Laser Damage]", isValid, isValid ? sniperCripplingRound.m_laserDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_explosionDamageMod, "[Explosion Damage]", isValid, isValid ? sniperCripplingRound.m_explosionDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_enemyHitEffectDurationMod, "[Effect Duration]", isValid, isValid ? sniperCripplingRound.m_effectOnLaserHitTargets.m_effectData.m_duration : 0);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_additionalEnemyHitEffect, "{ Additional Enemy Hit Effect }", string.Empty, isValid);
		return new StringBuilder().Append(desc).Append(AbilityModHelper.GetModPropertyDesc(m_maxTargetsMod, "[Max Laser Targets]", isValid, 1)).ToString();
	}
}
