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
			AbilityMod.AddToken(tokens, m_laserCountMod, "NumLasers", "number of lasers", trackerTranquilizerDarts.m_laserCount);
			AbilityMod.AddToken_LaserInfo(tokens, m_laserTargetingInfoMod, "LaserTargetingInfo", trackerTranquilizerDarts.m_laserTargetingInfo);
			AbilityMod.AddToken_EffectInfo(tokens, m_additionalEnemyEffect, "Effect_AdditionalEnemyHit");
			AbilityMod.AddToken_EffectMod(tokens, m_enemySingleHitEffectMod, "Effect_EnemySingleHit", trackerTranquilizerDarts.m_enemySingleHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyMultiHitEffectMod, "Effect_EnemyMultiHit", trackerTranquilizerDarts.m_enemyMultiHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_allySingleHitEffectMod, "Effect_AllySingleHit", trackerTranquilizerDarts.m_allySingleHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_allyMultiHitEffectMod, "Effect_AllyMultiHit", trackerTranquilizerDarts.m_allyMultiHitEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerTranquilizerDarts trackerTranquilizerDarts = GetTargetAbilityOnAbilityData(abilityData) as TrackerTranquilizerDarts;
		bool flag = trackerTranquilizerDarts != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt laserCountMod = m_laserCountMod;
		int baseVal;
		if (flag)
		{
			baseVal = trackerTranquilizerDarts.m_laserCount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(laserCountMod, "[Number of Darts]", flag, baseVal);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_additionalEnemyEffect, "{ Additional Enemy Hit Effect }", string.Empty, flag);
		string str2 = empty;
		AbilityModPropertyLaserInfo laserTargetingInfoMod = m_laserTargetingInfoMod;
		object baseLaserInfo;
		if (flag)
		{
			baseLaserInfo = trackerTranquilizerDarts.m_laserTargetingInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		empty = str2 + PropDesc(laserTargetingInfoMod, "[LaserTargetingInfo]", flag, (LaserTargetingInfo)baseLaserInfo);
		string str3 = empty;
		AbilityModPropertyEffectInfo enemySingleHitEffectMod = m_enemySingleHitEffectMod;
		object baseVal2;
		if (flag)
		{
			baseVal2 = trackerTranquilizerDarts.m_enemySingleHitEffect;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(enemySingleHitEffectMod, "{ Enemy Single Hit Effect Override }", flag, (StandardEffectInfo)baseVal2);
		string str4 = empty;
		AbilityModPropertyEffectInfo enemyMultiHitEffectMod = m_enemyMultiHitEffectMod;
		object baseVal3;
		if (flag)
		{
			baseVal3 = trackerTranquilizerDarts.m_enemyMultiHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(enemyMultiHitEffectMod, "{ Enemy Multi Hit Effect Override }", flag, (StandardEffectInfo)baseVal3);
		string str5 = empty;
		AbilityModPropertyEffectInfo allySingleHitEffectMod = m_allySingleHitEffectMod;
		object baseVal4;
		if (flag)
		{
			baseVal4 = trackerTranquilizerDarts.m_allySingleHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(allySingleHitEffectMod, "{ Ally Single Hit Effect Override }", flag, (StandardEffectInfo)baseVal4);
		string str6 = empty;
		AbilityModPropertyEffectInfo allyMultiHitEffectMod = m_allyMultiHitEffectMod;
		object baseVal5;
		if (flag)
		{
			baseVal5 = trackerTranquilizerDarts.m_allyMultiHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		return str6 + AbilityModHelper.GetModPropertyDesc(allyMultiHitEffectMod, "{ Ally Multi Hit Effect Override }", flag, (StandardEffectInfo)baseVal5);
	}
}
