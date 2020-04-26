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
		if (!(thiefSpoilLaserUlt != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_targeterMaxAngleMod, "TargeterMaxAngle", string.Empty, thiefSpoilLaserUlt.m_targeterMaxAngle);
			AbilityMod.AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, thiefSpoilLaserUlt.m_laserDamageAmount);
			AbilityMod.AddToken(tokens, m_laserSubsequentDamageAmountMod, "LaserSubsequentDamageAmount", string.Empty, thiefSpoilLaserUlt.m_laserSubsequentDamageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", thiefSpoilLaserUlt.m_enemyHitEffect);
			AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, thiefSpoilLaserUlt.m_laserRange);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, thiefSpoilLaserUlt.m_laserWidth);
			AbilityMod.AddToken(tokens, m_laserMaxTargetsMod, "LaserMaxTargets", string.Empty, thiefSpoilLaserUlt.m_laserMaxTargets);
			AbilityMod.AddToken(tokens, m_laserCountMod, "LaserCount", string.Empty, thiefSpoilLaserUlt.m_laserCount);
			AbilityMod.AddToken(tokens, m_copyBuffDurationMod, "CopyBuffDuration", string.Empty, thiefSpoilLaserUlt.m_copyBuffDuration);
			AbilityMod.AddToken(tokens, m_maxPowerupsHitMod, "MaxPowerupsHit", string.Empty, thiefSpoilLaserUlt.m_maxPowerupsHit);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefSpoilLaserUlt thiefSpoilLaserUlt = GetTargetAbilityOnAbilityData(abilityData) as ThiefSpoilLaserUlt;
		bool flag = thiefSpoilLaserUlt != null;
		string empty = string.Empty;
		empty += PropDesc(m_targeterMaxAngleMod, "[TargeterMaxAngle]", flag, (!flag) ? 0f : thiefSpoilLaserUlt.m_targeterMaxAngle);
		empty += PropDesc(m_laserDamageAmountMod, "[LaserDamageAmount]", flag, flag ? thiefSpoilLaserUlt.m_laserDamageAmount : 0);
		empty += PropDesc(m_laserSubsequentDamageAmountMod, "[LaserSubsequentDamageAmount]", flag, flag ? thiefSpoilLaserUlt.m_laserSubsequentDamageAmount : 0);
		empty += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", flag, (!flag) ? null : thiefSpoilLaserUlt.m_enemyHitEffect);
		string str = empty;
		AbilityModPropertyFloat laserRangeMod = m_laserRangeMod;
		float baseVal;
		if (flag)
		{
			baseVal = thiefSpoilLaserUlt.m_laserRange;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(laserRangeMod, "[LaserRange]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = thiefSpoilLaserUlt.m_laserWidth;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(laserWidthMod, "[LaserWidth]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt laserMaxTargetsMod = m_laserMaxTargetsMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = thiefSpoilLaserUlt.m_laserMaxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(laserMaxTargetsMod, "[LaserMaxTargets]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyInt laserCountMod = m_laserCountMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = thiefSpoilLaserUlt.m_laserCount;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(laserCountMod, "[LaserCount]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyBool laserPenetrateLosMod = m_laserPenetrateLosMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = (thiefSpoilLaserUlt.m_laserPenetrateLos ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(laserPenetrateLosMod, "[LaserPenetrateLos]", flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertySpoilsSpawnData spoilSpawnDataMod = m_spoilSpawnDataMod;
		object baseVal6;
		if (flag)
		{
			baseVal6 = thiefSpoilLaserUlt.m_spoilSpawnData;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(spoilSpawnDataMod, "[SpoilSpawnData]", flag, (SpoilsSpawnData)baseVal6);
		string str7 = empty;
		AbilityModPropertyBool hitPowerupsMod = m_hitPowerupsMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = (thiefSpoilLaserUlt.m_hitPowerups ? 1 : 0);
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(hitPowerupsMod, "[HitPowerups]", flag, (byte)baseVal7 != 0);
		string str8 = empty;
		AbilityModPropertyBool stopOnPowerupHitMod = m_stopOnPowerupHitMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = (thiefSpoilLaserUlt.m_stopOnPowerupHit ? 1 : 0);
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(stopOnPowerupHitMod, "[StopOnPowerupHit]", flag, (byte)baseVal8 != 0);
		empty += PropDesc(m_includeSpoilsPowerupsMod, "[IncludeSpoilsPowerups]", flag, flag && thiefSpoilLaserUlt.m_includeSpoilsPowerups);
		string str9 = empty;
		AbilityModPropertyBool ignorePickupTeamRestrictionMod = m_ignorePickupTeamRestrictionMod;
		int baseVal9;
		if (flag)
		{
			baseVal9 = (thiefSpoilLaserUlt.m_ignorePickupTeamRestriction ? 1 : 0);
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(ignorePickupTeamRestrictionMod, "[IgnorePickupTeamRestriction]", flag, (byte)baseVal9 != 0);
		string str10 = empty;
		AbilityModPropertyInt maxPowerupsHitMod = m_maxPowerupsHitMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = thiefSpoilLaserUlt.m_maxPowerupsHit;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(maxPowerupsHitMod, "[MaxPowerupsHit]", flag, baseVal10);
		string str11 = empty;
		AbilityModPropertyBool copyBuffsOnEnemyHitMod = m_copyBuffsOnEnemyHitMod;
		int baseVal11;
		if (flag)
		{
			baseVal11 = (thiefSpoilLaserUlt.m_copyBuffsOnEnemyHit ? 1 : 0);
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str11 + PropDesc(copyBuffsOnEnemyHitMod, "[CopyBuffsOnEnemyHit]", flag, (byte)baseVal11 != 0);
		string str12 = empty;
		AbilityModPropertyInt copyBuffDurationMod = m_copyBuffDurationMod;
		int baseVal12;
		if (flag)
		{
			baseVal12 = thiefSpoilLaserUlt.m_copyBuffDuration;
		}
		else
		{
			baseVal12 = 0;
		}
		return str12 + PropDesc(copyBuffDurationMod, "[CopyBuffDuration]", flag, baseVal12);
	}
}
