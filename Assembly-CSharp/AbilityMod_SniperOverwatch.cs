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
		if (!(sniperOverwatch != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_durationMod, "Duration", string.Empty, sniperOverwatch.m_duration);
			AbilityMod.AddToken(tokens, m_enemyMaxHitsMod, "MaxHits", string.Empty, sniperOverwatch.m_maxHits);
			AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, sniperOverwatch.m_onEnemyMoveThrough.m_damage);
			if (m_useEnemyHitEffectOverride)
			{
				while (true)
				{
					AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffectOverride, "EnemyHitEffect", sniperOverwatch.m_onEnemyMoveThrough.m_effect);
					return;
				}
			}
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SniperOverwatch sniperOverwatch = GetTargetAbilityOnAbilityData(abilityData) as SniperOverwatch;
		bool flag = sniperOverwatch != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt durationMod = m_durationMod;
		int baseVal;
		if (flag)
		{
			baseVal = sniperOverwatch.m_duration;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(durationMod, "[Barrier Duration]", flag, baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_enemyMaxHitsMod, "[Barrier Max Hits]", flag, flag ? sniperOverwatch.m_maxHits : 0);
		string str2 = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = sniperOverwatch.m_onEnemyMoveThrough.m_damage;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(damageMod, "[Move Through Damage]", flag, baseVal2);
		if (m_useEnemyHitEffectOverride)
		{
			empty += AbilityModHelper.GetModEffectInfoDesc(m_enemyHitEffectOverride, "{ Enemy On Move Through Effect Override }", string.Empty, flag, (!flag) ? null : sniperOverwatch.m_onEnemyMoveThrough.m_effect);
		}
		return empty;
	}
}
