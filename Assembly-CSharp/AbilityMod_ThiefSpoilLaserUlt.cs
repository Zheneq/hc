using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ThiefSpoilLaserUlt : AbilityMod
{
	[Header("-- Targeter")]
	public AbilityModPropertyFloat m_targeterMaxAngleMod;
	[Header("-- Damage")]
	public AbilityModPropertyInt m_laserDamageAmountMod;
	public AbilityModPropertyInt m_laserSubsequentDamageAmountMod;
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;
	[Header("-- Laser Properties")]
	public AbilityModPropertyFloat m_laserRangeMod;
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyInt m_laserMaxTargetsMod;
	public AbilityModPropertyInt m_laserCountMod;
	public AbilityModPropertyBool m_laserPenetrateLosMod;
	[Header("-- Spoil Spawn Data On Enemy Hit")]
	public AbilityModPropertySpoilsSpawnData m_spoilSpawnDataMod;
	[Header("-- PowerUp/Spoils Interaction")]
	public AbilityModPropertyBool m_hitPowerupsMod;
	public AbilityModPropertyBool m_stopOnPowerupHitMod;
	public AbilityModPropertyBool m_includeSpoilsPowerupsMod;
	public AbilityModPropertyBool m_ignorePickupTeamRestrictionMod;
	public AbilityModPropertyInt m_maxPowerupsHitMod;
	[Header("-- Buffs Copy --")]
	public AbilityModPropertyBool m_copyBuffsOnEnemyHitMod;
	public AbilityModPropertyInt m_copyBuffDurationMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ThiefSpoilLaserUlt);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ThiefSpoilLaserUlt thiefSpoilLaserUlt = targetAbility as ThiefSpoilLaserUlt;
		if (thiefSpoilLaserUlt != null)
		{
			AddToken(tokens, m_targeterMaxAngleMod, "TargeterMaxAngle", string.Empty, thiefSpoilLaserUlt.m_targeterMaxAngle);
			AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, thiefSpoilLaserUlt.m_laserDamageAmount);
			AddToken(tokens, m_laserSubsequentDamageAmountMod, "LaserSubsequentDamageAmount", string.Empty, thiefSpoilLaserUlt.m_laserSubsequentDamageAmount);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", thiefSpoilLaserUlt.m_enemyHitEffect);
			AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, thiefSpoilLaserUlt.m_laserRange);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, thiefSpoilLaserUlt.m_laserWidth);
			AddToken(tokens, m_laserMaxTargetsMod, "LaserMaxTargets", string.Empty, thiefSpoilLaserUlt.m_laserMaxTargets);
			AddToken(tokens, m_laserCountMod, "LaserCount", string.Empty, thiefSpoilLaserUlt.m_laserCount);
			AddToken(tokens, m_copyBuffDurationMod, "CopyBuffDuration", string.Empty, thiefSpoilLaserUlt.m_copyBuffDuration);
			AddToken(tokens, m_maxPowerupsHitMod, "MaxPowerupsHit", string.Empty, thiefSpoilLaserUlt.m_maxPowerupsHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefSpoilLaserUlt thiefSpoilLaserUlt = GetTargetAbilityOnAbilityData(abilityData) as ThiefSpoilLaserUlt;
		bool isValid = thiefSpoilLaserUlt != null;
		string desc = string.Empty;
		desc += PropDesc(m_targeterMaxAngleMod, "[TargeterMaxAngle]", isValid, isValid ? thiefSpoilLaserUlt.m_targeterMaxAngle : 0f);
		desc += PropDesc(m_laserDamageAmountMod, "[LaserDamageAmount]", isValid, isValid ? thiefSpoilLaserUlt.m_laserDamageAmount : 0);
		desc += PropDesc(m_laserSubsequentDamageAmountMod, "[LaserSubsequentDamageAmount]", isValid, isValid ? thiefSpoilLaserUlt.m_laserSubsequentDamageAmount : 0);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isValid, isValid ? thiefSpoilLaserUlt.m_enemyHitEffect : null);
		desc += PropDesc(m_laserRangeMod, "[LaserRange]", isValid, isValid ? thiefSpoilLaserUlt.m_laserRange : 0f);
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isValid, isValid ? thiefSpoilLaserUlt.m_laserWidth : 0f);
		desc += PropDesc(m_laserMaxTargetsMod, "[LaserMaxTargets]", isValid, isValid ? thiefSpoilLaserUlt.m_laserMaxTargets : 0);
		desc += PropDesc(m_laserCountMod, "[LaserCount]", isValid, isValid ? thiefSpoilLaserUlt.m_laserCount : 0);
		desc += PropDesc(m_laserPenetrateLosMod, "[LaserPenetrateLos]", isValid, isValid && thiefSpoilLaserUlt.m_laserPenetrateLos);
		desc += PropDesc(m_spoilSpawnDataMod, "[SpoilSpawnData]", isValid, isValid ? thiefSpoilLaserUlt.m_spoilSpawnData : null);
		desc += PropDesc(m_hitPowerupsMod, "[HitPowerups]", isValid, isValid && thiefSpoilLaserUlt.m_hitPowerups);
		desc += PropDesc(m_stopOnPowerupHitMod, "[StopOnPowerupHit]", isValid, isValid && thiefSpoilLaserUlt.m_stopOnPowerupHit);
		desc += PropDesc(m_includeSpoilsPowerupsMod, "[IncludeSpoilsPowerups]", isValid, isValid && thiefSpoilLaserUlt.m_includeSpoilsPowerups);
		desc += PropDesc(m_ignorePickupTeamRestrictionMod, "[IgnorePickupTeamRestriction]", isValid, isValid && thiefSpoilLaserUlt.m_ignorePickupTeamRestriction);
		desc += PropDesc(m_maxPowerupsHitMod, "[MaxPowerupsHit]", isValid, isValid ? thiefSpoilLaserUlt.m_maxPowerupsHit : 0);
		desc += PropDesc(m_copyBuffsOnEnemyHitMod, "[CopyBuffsOnEnemyHit]", isValid, isValid && thiefSpoilLaserUlt.m_copyBuffsOnEnemyHit);
		return desc + PropDesc(m_copyBuffDurationMod, "[CopyBuffDuration]", isValid, isValid ? thiefSpoilLaserUlt.m_copyBuffDuration : 0);
	}
}
