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
			AbilityMod.AddToken(tokens, this.m_targeterMaxAngleMod, "TargeterMaxAngle", string.Empty, thiefSpoilLaserUlt.m_targeterMaxAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, thiefSpoilLaserUlt.m_laserDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_laserSubsequentDamageAmountMod, "LaserSubsequentDamageAmount", string.Empty, thiefSpoilLaserUlt.m_laserSubsequentDamageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", thiefSpoilLaserUlt.m_enemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", string.Empty, thiefSpoilLaserUlt.m_laserRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, thiefSpoilLaserUlt.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserMaxTargetsMod, "LaserMaxTargets", string.Empty, thiefSpoilLaserUlt.m_laserMaxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_laserCountMod, "LaserCount", string.Empty, thiefSpoilLaserUlt.m_laserCount, true, false);
			AbilityMod.AddToken(tokens, this.m_copyBuffDurationMod, "CopyBuffDuration", string.Empty, thiefSpoilLaserUlt.m_copyBuffDuration, true, false);
			AbilityMod.AddToken(tokens, this.m_maxPowerupsHitMod, "MaxPowerupsHit", string.Empty, thiefSpoilLaserUlt.m_maxPowerupsHit, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefSpoilLaserUlt thiefSpoilLaserUlt = base.GetTargetAbilityOnAbilityData(abilityData) as ThiefSpoilLaserUlt;
		bool flag = thiefSpoilLaserUlt != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_targeterMaxAngleMod, "[TargeterMaxAngle]", flag, (!flag) ? 0f : thiefSpoilLaserUlt.m_targeterMaxAngle);
		text += base.PropDesc(this.m_laserDamageAmountMod, "[LaserDamageAmount]", flag, (!flag) ? 0 : thiefSpoilLaserUlt.m_laserDamageAmount);
		text += base.PropDesc(this.m_laserSubsequentDamageAmountMod, "[LaserSubsequentDamageAmount]", flag, (!flag) ? 0 : thiefSpoilLaserUlt.m_laserSubsequentDamageAmount);
		text += base.PropDesc(this.m_enemyHitEffectMod, "[EnemyHitEffect]", flag, (!flag) ? null : thiefSpoilLaserUlt.m_enemyHitEffect);
		string str = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix = "[LaserRange]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = thiefSpoilLaserUlt.m_laserRange;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(laserRangeMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix2 = "[LaserWidth]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = thiefSpoilLaserUlt.m_laserWidth;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(laserWidthMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt laserMaxTargetsMod = this.m_laserMaxTargetsMod;
		string prefix3 = "[LaserMaxTargets]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = thiefSpoilLaserUlt.m_laserMaxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(laserMaxTargetsMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt laserCountMod = this.m_laserCountMod;
		string prefix4 = "[LaserCount]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = thiefSpoilLaserUlt.m_laserCount;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(laserCountMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool laserPenetrateLosMod = this.m_laserPenetrateLosMod;
		string prefix5 = "[LaserPenetrateLos]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = thiefSpoilLaserUlt.m_laserPenetrateLos;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(laserPenetrateLosMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertySpoilsSpawnData spoilSpawnDataMod = this.m_spoilSpawnDataMod;
		string prefix6 = "[SpoilSpawnData]";
		bool showBaseVal6 = flag;
		SpoilsSpawnData baseVal6;
		if (flag)
		{
			baseVal6 = thiefSpoilLaserUlt.m_spoilSpawnData;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(spoilSpawnDataMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyBool hitPowerupsMod = this.m_hitPowerupsMod;
		string prefix7 = "[HitPowerups]";
		bool showBaseVal7 = flag;
		bool baseVal7;
		if (flag)
		{
			baseVal7 = thiefSpoilLaserUlt.m_hitPowerups;
		}
		else
		{
			baseVal7 = false;
		}
		text = str7 + base.PropDesc(hitPowerupsMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyBool stopOnPowerupHitMod = this.m_stopOnPowerupHitMod;
		string prefix8 = "[StopOnPowerupHit]";
		bool showBaseVal8 = flag;
		bool baseVal8;
		if (flag)
		{
			baseVal8 = thiefSpoilLaserUlt.m_stopOnPowerupHit;
		}
		else
		{
			baseVal8 = false;
		}
		text = str8 + base.PropDesc(stopOnPowerupHitMod, prefix8, showBaseVal8, baseVal8);
		text += base.PropDesc(this.m_includeSpoilsPowerupsMod, "[IncludeSpoilsPowerups]", flag, flag && thiefSpoilLaserUlt.m_includeSpoilsPowerups);
		string str9 = text;
		AbilityModPropertyBool ignorePickupTeamRestrictionMod = this.m_ignorePickupTeamRestrictionMod;
		string prefix9 = "[IgnorePickupTeamRestriction]";
		bool showBaseVal9 = flag;
		bool baseVal9;
		if (flag)
		{
			baseVal9 = thiefSpoilLaserUlt.m_ignorePickupTeamRestriction;
		}
		else
		{
			baseVal9 = false;
		}
		text = str9 + base.PropDesc(ignorePickupTeamRestrictionMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt maxPowerupsHitMod = this.m_maxPowerupsHitMod;
		string prefix10 = "[MaxPowerupsHit]";
		bool showBaseVal10 = flag;
		int baseVal10;
		if (flag)
		{
			baseVal10 = thiefSpoilLaserUlt.m_maxPowerupsHit;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(maxPowerupsHitMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyBool copyBuffsOnEnemyHitMod = this.m_copyBuffsOnEnemyHitMod;
		string prefix11 = "[CopyBuffsOnEnemyHit]";
		bool showBaseVal11 = flag;
		bool baseVal11;
		if (flag)
		{
			baseVal11 = thiefSpoilLaserUlt.m_copyBuffsOnEnemyHit;
		}
		else
		{
			baseVal11 = false;
		}
		text = str11 + base.PropDesc(copyBuffsOnEnemyHitMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyInt copyBuffDurationMod = this.m_copyBuffDurationMod;
		string prefix12 = "[CopyBuffDuration]";
		bool showBaseVal12 = flag;
		int baseVal12;
		if (flag)
		{
			baseVal12 = thiefSpoilLaserUlt.m_copyBuffDuration;
		}
		else
		{
			baseVal12 = 0;
		}
		return str12 + base.PropDesc(copyBuffDurationMod, prefix12, showBaseVal12, baseVal12);
	}
}
