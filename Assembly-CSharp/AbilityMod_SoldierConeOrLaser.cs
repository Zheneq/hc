using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SoldierConeOrLaser : AbilityMod
{
	[Header("  Targeting: For Cone")]
	public AbilityModPropertyConeInfo m_coneInfoMod;

	[Header("  Targeting: For Laser")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;

	[Header("-- On Hit Stuff --")]
	public AbilityModPropertyInt m_coneDamageMod;

	public AbilityModPropertyEffectInfo m_coneEnemyHitEffectMod;

	[Space(10f)]
	public AbilityModPropertyInt m_laserDamageMod;

	public AbilityModPropertyEffectInfo m_laserEnemyHitEffectMod;

	[Header("-- Extra Damage --")]
	public AbilityModPropertyInt m_extraDamageForAlternatingMod;

	public AbilityModPropertyFloat m_closeDistThresholdMod;

	public AbilityModPropertyInt m_extraDamageForNearTargetMod;

	public AbilityModPropertyInt m_extraDamageForFromCoverMod;

	public AbilityModPropertyInt m_extraDamageToEvadersMod;

	[Header("-- Extra Energy --")]
	public AbilityModPropertyInt m_extraEnergyForConeMod;

	public AbilityModPropertyInt m_extraEnergyForLaserMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SoldierConeOrLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SoldierConeOrLaser soldierConeOrLaser = targetAbility as SoldierConeOrLaser;
		if (soldierConeOrLaser != null)
		{
			AbilityMod.AddToken_ConeInfo(tokens, this.m_coneInfoMod, "ConeInfo", soldierConeOrLaser.m_coneInfo, true);
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "LaserInfo", soldierConeOrLaser.m_laserInfo, true);
			AbilityMod.AddToken(tokens, this.m_coneDamageMod, "ConeDamage", string.Empty, soldierConeOrLaser.m_coneDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_coneEnemyHitEffectMod, "ConeEnemyHitEffect", soldierConeOrLaser.m_coneEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_laserDamageMod, "LaserDamage", string.Empty, soldierConeOrLaser.m_laserDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_laserEnemyHitEffectMod, "LaserEnemyHitEffect", soldierConeOrLaser.m_laserEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraDamageForAlternatingMod, "ExtraDamageForAlternating", string.Empty, soldierConeOrLaser.m_extraDamageForAlternating, true, false);
			AbilityMod.AddToken(tokens, this.m_closeDistThresholdMod, "CloseDistThreshold", string.Empty, soldierConeOrLaser.m_closeDistThreshold, true, false, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForNearTargetMod, "ExtraDamageForNearTarget", string.Empty, soldierConeOrLaser.m_extraDamageForNearTarget, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForFromCoverMod, "ExtraDamageForFromCover", string.Empty, soldierConeOrLaser.m_extraDamageForFromCover, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageToEvadersMod, "ExtraDamageToEvaders", string.Empty, soldierConeOrLaser.m_extraDamageToEvaders, true, false);
			AbilityMod.AddToken(tokens, this.m_extraEnergyForConeMod, "ExtraEnergyForCone", string.Empty, soldierConeOrLaser.m_extraEnergyForCone, true, true);
			AbilityMod.AddToken(tokens, this.m_extraEnergyForLaserMod, "ExtraEnergyForLaser", string.Empty, soldierConeOrLaser.m_extraEnergyForLaser, true, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierConeOrLaser soldierConeOrLaser = base.GetTargetAbilityOnAbilityData(abilityData) as SoldierConeOrLaser;
		bool flag = soldierConeOrLaser != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyConeInfo coneInfoMod = this.m_coneInfoMod;
		string prefix = "[ConeInfo]";
		bool showBaseVal = flag;
		ConeTargetingInfo baseConeInfo;
		if (flag)
		{
			baseConeInfo = soldierConeOrLaser.m_coneInfo;
		}
		else
		{
			baseConeInfo = null;
		}
		text = str + base.PropDesc(coneInfoMod, prefix, showBaseVal, baseConeInfo);
		text += base.PropDesc(this.m_laserInfoMod, "[LaserInfo]", flag, (!flag) ? null : soldierConeOrLaser.m_laserInfo);
		string str2 = text;
		AbilityModPropertyInt coneDamageMod = this.m_coneDamageMod;
		string prefix2 = "[ConeDamage]";
		bool showBaseVal2 = flag;
		int baseVal;
		if (flag)
		{
			baseVal = soldierConeOrLaser.m_coneDamage;
		}
		else
		{
			baseVal = 0;
		}
		text = str2 + base.PropDesc(coneDamageMod, prefix2, showBaseVal2, baseVal);
		string str3 = text;
		AbilityModPropertyEffectInfo coneEnemyHitEffectMod = this.m_coneEnemyHitEffectMod;
		string prefix3 = "[ConeEnemyHitEffect]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal2;
		if (flag)
		{
			baseVal2 = soldierConeOrLaser.m_coneEnemyHitEffect;
		}
		else
		{
			baseVal2 = null;
		}
		text = str3 + base.PropDesc(coneEnemyHitEffectMod, prefix3, showBaseVal3, baseVal2);
		string str4 = text;
		AbilityModPropertyInt laserDamageMod = this.m_laserDamageMod;
		string prefix4 = "[LaserDamage]";
		bool showBaseVal4 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = soldierConeOrLaser.m_laserDamage;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str4 + base.PropDesc(laserDamageMod, prefix4, showBaseVal4, baseVal3);
		text += base.PropDesc(this.m_laserEnemyHitEffectMod, "[LaserEnemyHitEffect]", flag, (!flag) ? null : soldierConeOrLaser.m_laserEnemyHitEffect);
		text += base.PropDesc(this.m_extraDamageForAlternatingMod, "[ExtraDamageForAlternating]", flag, (!flag) ? 0 : soldierConeOrLaser.m_extraDamageForAlternating);
		text += base.PropDesc(this.m_closeDistThresholdMod, "[CloseDistThreshold]", flag, (!flag) ? 0f : soldierConeOrLaser.m_closeDistThreshold);
		string str5 = text;
		AbilityModPropertyInt extraDamageForNearTargetMod = this.m_extraDamageForNearTargetMod;
		string prefix5 = "[ExtraDamageForNearTarget]";
		bool showBaseVal5 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = soldierConeOrLaser.m_extraDamageForNearTarget;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str5 + base.PropDesc(extraDamageForNearTargetMod, prefix5, showBaseVal5, baseVal4);
		string str6 = text;
		AbilityModPropertyInt extraDamageForFromCoverMod = this.m_extraDamageForFromCoverMod;
		string prefix6 = "[ExtraDamageForFromCover]";
		bool showBaseVal6 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = soldierConeOrLaser.m_extraDamageForFromCover;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str6 + base.PropDesc(extraDamageForFromCoverMod, prefix6, showBaseVal6, baseVal5);
		text += base.PropDesc(this.m_extraDamageToEvadersMod, "[ExtraDamageToEvaders]", flag, (!flag) ? 0 : soldierConeOrLaser.m_extraDamageToEvaders);
		text += base.PropDesc(this.m_extraEnergyForConeMod, "[ExtraEnergyForCone]", flag, (!flag) ? 0 : soldierConeOrLaser.m_extraEnergyForCone);
		string str7 = text;
		AbilityModPropertyInt extraEnergyForLaserMod = this.m_extraEnergyForLaserMod;
		string prefix7 = "[ExtraEnergyForLaser]";
		bool showBaseVal7 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = soldierConeOrLaser.m_extraEnergyForLaser;
		}
		else
		{
			baseVal6 = 0;
		}
		return str7 + base.PropDesc(extraEnergyForLaserMod, prefix7, showBaseVal7, baseVal6);
	}
}
