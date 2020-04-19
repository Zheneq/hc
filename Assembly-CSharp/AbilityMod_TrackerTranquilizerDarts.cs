using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TrackerTranquilizerDarts : AbilityMod
{
	[Space(10f)]
	public AbilityModPropertyInt m_laserCountMod;

	public StandardEffectInfo m_additionalEnemyEffect;

	[Header("-- Targeting Mods")]
	public AbilityModPropertyLaserInfo m_laserTargetingInfoMod;

	[Header("-- On Hit Effect Mods")]
	public AbilityModPropertyEffectInfo m_enemySingleHitEffectMod;

	public AbilityModPropertyEffectInfo m_enemyMultiHitEffectMod;

	[Space(10f)]
	public AbilityModPropertyEffectInfo m_allySingleHitEffectMod;

	public AbilityModPropertyEffectInfo m_allyMultiHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(TrackerTranquilizerDarts);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TrackerTranquilizerDarts trackerTranquilizerDarts = targetAbility as TrackerTranquilizerDarts;
		if (trackerTranquilizerDarts != null)
		{
			AbilityMod.AddToken(tokens, this.m_laserCountMod, "NumLasers", "number of lasers", trackerTranquilizerDarts.m_laserCount, true, false);
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserTargetingInfoMod, "LaserTargetingInfo", trackerTranquilizerDarts.m_laserTargetingInfo, true);
			AbilityMod.AddToken_EffectInfo(tokens, this.m_additionalEnemyEffect, "Effect_AdditionalEnemyHit", null, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemySingleHitEffectMod, "Effect_EnemySingleHit", trackerTranquilizerDarts.m_enemySingleHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyMultiHitEffectMod, "Effect_EnemyMultiHit", trackerTranquilizerDarts.m_enemyMultiHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allySingleHitEffectMod, "Effect_AllySingleHit", trackerTranquilizerDarts.m_allySingleHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyMultiHitEffectMod, "Effect_AllyMultiHit", trackerTranquilizerDarts.m_allyMultiHitEffect, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerTranquilizerDarts trackerTranquilizerDarts = base.GetTargetAbilityOnAbilityData(abilityData) as TrackerTranquilizerDarts;
		bool flag = trackerTranquilizerDarts != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt laserCountMod = this.m_laserCountMod;
		string prefix = "[Number of Darts]";
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_TrackerTranquilizerDarts.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = trackerTranquilizerDarts.m_laserCount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(laserCountMod, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_additionalEnemyEffect, "{ Additional Enemy Hit Effect }", string.Empty, flag, null);
		string str2 = text;
		AbilityModPropertyLaserInfo laserTargetingInfoMod = this.m_laserTargetingInfoMod;
		string prefix2 = "[LaserTargetingInfo]";
		bool showBaseVal2 = flag;
		LaserTargetingInfo baseLaserInfo;
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseLaserInfo = trackerTranquilizerDarts.m_laserTargetingInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		text = str2 + base.PropDesc(laserTargetingInfoMod, prefix2, showBaseVal2, baseLaserInfo);
		string str3 = text;
		AbilityModPropertyEffectInfo enemySingleHitEffectMod = this.m_enemySingleHitEffectMod;
		string prefix3 = "{ Enemy Single Hit Effect Override }";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = trackerTranquilizerDarts.m_enemySingleHitEffect;
		}
		else
		{
			baseVal2 = null;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(enemySingleHitEffectMod, prefix3, showBaseVal3, baseVal2);
		string str4 = text;
		AbilityModPropertyEffectInfo enemyMultiHitEffectMod = this.m_enemyMultiHitEffectMod;
		string prefix4 = "{ Enemy Multi Hit Effect Override }";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal3;
		if (flag)
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
			baseVal3 = trackerTranquilizerDarts.m_enemyMultiHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(enemyMultiHitEffectMod, prefix4, showBaseVal4, baseVal3);
		string str5 = text;
		AbilityModPropertyEffectInfo allySingleHitEffectMod = this.m_allySingleHitEffectMod;
		string prefix5 = "{ Ally Single Hit Effect Override }";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal4;
		if (flag)
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
			baseVal4 = trackerTranquilizerDarts.m_allySingleHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(allySingleHitEffectMod, prefix5, showBaseVal5, baseVal4);
		string str6 = text;
		AbilityModPropertyEffectInfo allyMultiHitEffectMod = this.m_allyMultiHitEffectMod;
		string prefix6 = "{ Ally Multi Hit Effect Override }";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal5;
		if (flag)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = trackerTranquilizerDarts.m_allyMultiHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		return str6 + AbilityModHelper.GetModPropertyDesc(allyMultiHitEffectMod, prefix6, showBaseVal6, baseVal5);
	}
}
