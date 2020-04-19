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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SniperOverwatch.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_durationMod, "Duration", string.Empty, sniperOverwatch.m_duration, true, false);
			AbilityMod.AddToken(tokens, this.m_enemyMaxHitsMod, "MaxHits", string.Empty, sniperOverwatch.m_maxHits, true, false);
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, sniperOverwatch.m_onEnemyMoveThrough.m_damage, true, false);
			if (this.m_useEnemyHitEffectOverride)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffectOverride, "EnemyHitEffect", sniperOverwatch.m_onEnemyMoveThrough.m_effect, true);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SniperOverwatch sniperOverwatch = base.GetTargetAbilityOnAbilityData(abilityData) as SniperOverwatch;
		bool flag = sniperOverwatch != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt durationMod = this.m_durationMod;
		string prefix = "[Barrier Duration]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SniperOverwatch.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = sniperOverwatch.m_duration;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(durationMod, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_enemyMaxHitsMod, "[Barrier Max Hits]", flag, (!flag) ? 0 : sniperOverwatch.m_maxHits);
		string str2 = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix2 = "[Move Through Damage]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = sniperOverwatch.m_onEnemyMoveThrough.m_damage;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(damageMod, prefix2, showBaseVal2, baseVal2);
		if (this.m_useEnemyHitEffectOverride)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			text += AbilityModHelper.GetModEffectInfoDesc(this.m_enemyHitEffectOverride, "{ Enemy On Move Through Effect Override }", string.Empty, flag, (!flag) ? null : sniperOverwatch.m_onEnemyMoveThrough.m_effect);
		}
		return text;
	}
}
