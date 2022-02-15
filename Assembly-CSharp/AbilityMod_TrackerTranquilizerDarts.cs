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
			AddToken(tokens, m_laserCountMod, "NumLasers", "number of lasers", trackerTranquilizerDarts.m_laserCount);
			AddToken_LaserInfo(tokens, m_laserTargetingInfoMod, "LaserTargetingInfo", trackerTranquilizerDarts.m_laserTargetingInfo);
			AddToken_EffectInfo(tokens, m_additionalEnemyEffect, "Effect_AdditionalEnemyHit");
			AddToken_EffectMod(tokens, m_enemySingleHitEffectMod, "Effect_EnemySingleHit", trackerTranquilizerDarts.m_enemySingleHitEffect);
			AddToken_EffectMod(tokens, m_enemyMultiHitEffectMod, "Effect_EnemyMultiHit", trackerTranquilizerDarts.m_enemyMultiHitEffect);
			AddToken_EffectMod(tokens, m_allySingleHitEffectMod, "Effect_AllySingleHit", trackerTranquilizerDarts.m_allySingleHitEffect);
			AddToken_EffectMod(tokens, m_allyMultiHitEffectMod, "Effect_AllyMultiHit", trackerTranquilizerDarts.m_allyMultiHitEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerTranquilizerDarts trackerTranquilizerDarts = GetTargetAbilityOnAbilityData(abilityData) as TrackerTranquilizerDarts;
		bool isAbilityPresent = trackerTranquilizerDarts != null;
		string desc = "";
		desc += AbilityModHelper.GetModPropertyDesc(m_laserCountMod, "[Number of Darts]", isAbilityPresent, isAbilityPresent ? trackerTranquilizerDarts.m_laserCount : 0);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_additionalEnemyEffect, "{ Additional Enemy Hit Effect }", "", isAbilityPresent);
		desc += PropDesc(m_laserTargetingInfoMod, "[LaserTargetingInfo]", isAbilityPresent, isAbilityPresent ? trackerTranquilizerDarts.m_laserTargetingInfo : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_enemySingleHitEffectMod, "{ Enemy Single Hit Effect Override }", isAbilityPresent, isAbilityPresent ? trackerTranquilizerDarts.m_enemySingleHitEffect : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_enemyMultiHitEffectMod, "{ Enemy Multi Hit Effect Override }", isAbilityPresent, isAbilityPresent ? trackerTranquilizerDarts.m_enemyMultiHitEffect : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_allySingleHitEffectMod, "{ Ally Single Hit Effect Override }", isAbilityPresent, isAbilityPresent ? trackerTranquilizerDarts.m_allySingleHitEffect : null);
		return desc + AbilityModHelper.GetModPropertyDesc(m_allyMultiHitEffectMod, "{ Ally Multi Hit Effect Override }", isAbilityPresent, isAbilityPresent ? trackerTranquilizerDarts.m_allyMultiHitEffect : null);
	}
}
