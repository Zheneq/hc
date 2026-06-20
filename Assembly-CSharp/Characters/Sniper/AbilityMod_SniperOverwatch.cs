// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SniperOverwatch : AbilityMod
{
	[Header("-- Barrier Duration Mod")]
	public AbilityModPropertyInt m_durationMod;
	[Header("-- Enemy Move-Through Mods")]
	public AbilityModPropertyInt m_enemyMaxHitsMod;
	public AbilityModPropertyInt m_damageMod;
	public bool m_useEnemyHitEffectOverride;
	public StandardEffectInfo m_enemyHitEffectOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(SniperOverwatch);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SniperOverwatch sniperOverwatch = targetAbility as SniperOverwatch;
		if (sniperOverwatch != null)
		{
			AddToken(tokens, m_durationMod, "Duration", string.Empty, sniperOverwatch.m_duration);
			AddToken(tokens, m_enemyMaxHitsMod, "MaxHits", string.Empty, sniperOverwatch.m_maxHits);
			AddToken(tokens, m_damageMod, "Damage", string.Empty, sniperOverwatch.m_onEnemyMoveThrough.m_damage);
			if (m_useEnemyHitEffectOverride)
			{
				AddToken_EffectInfo(tokens, m_enemyHitEffectOverride, "EnemyHitEffect", sniperOverwatch.m_onEnemyMoveThrough.m_effect);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		SniperOverwatch sniperOverwatch = GetTargetAbilityOnAbilityData(abilityData) as SniperOverwatch;
		// rogues
		// SniperOverwatch sniperOverwatch = targetAbility as SniperOverwatch;
		bool isValid = sniperOverwatch != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_durationMod, "[Barrier Duration]", isValid, isValid ? sniperOverwatch.m_duration : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_enemyMaxHitsMod, "[Barrier Max Hits]", isValid, isValid ? sniperOverwatch.m_maxHits : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Move Through Damage]", isValid, isValid ? sniperOverwatch.m_onEnemyMoveThrough.m_damage : 0);
		if (m_useEnemyHitEffectOverride)
		{
			desc += AbilityModHelper.GetModEffectInfoDesc(m_enemyHitEffectOverride, "{ Enemy On Move Through Effect Override }", string.Empty, isValid, isValid ? sniperOverwatch.m_onEnemyMoveThrough.m_effect : null);
		}
		return desc;
	}
}
