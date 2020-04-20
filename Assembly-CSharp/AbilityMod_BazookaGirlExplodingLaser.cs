using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BazookaGirlExplodingLaser : AbilityMod
{
	[Header("-- Targeting: If using Cone")]
	public AbilityModPropertyFloat m_coneWidthAngleMod;

	public AbilityModPropertyFloat m_coneLengthMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	[Header("-- Laser Params")]
	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyBool m_laserPenetrateLosMod;

	[Header("-- Laser Hit Mods")]
	public AbilityModPropertyInt m_laserDamageMod;

	public AbilityModPropertyBool m_laserIgnoreCoverMod;

	public AbilityModPropertyEffectInfo m_laserHitEffectOverride;

	public AbilityModPropertyInt m_cdrOnDirectHitMod;

	[Header("-- Explosion Hit Mods")]
	public AbilityModPropertyInt m_explosionDamageMod;

	public AbilityModPropertyBool m_explosionIgnoreLosMod;

	public AbilityModPropertyBool m_explosionIgnoreCoverMod;

	public AbilityModPropertyEffectInfo m_explosionEffectOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(BazookaGirlExplodingLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BazookaGirlExplodingLaser bazookaGirlExplodingLaser = targetAbility as BazookaGirlExplodingLaser;
		if (bazookaGirlExplodingLaser != null)
		{
			AbilityMod.AddToken(tokens, this.m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, bazookaGirlExplodingLaser.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneLengthMod, "ConeLength", string.Empty, bazookaGirlExplodingLaser.m_coneLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, bazookaGirlExplodingLaser.m_coneBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, bazookaGirlExplodingLaser.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", string.Empty, bazookaGirlExplodingLaser.m_laserRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserDamageMod, "LaserDamageAmount", string.Empty, bazookaGirlExplodingLaser.m_laserDamageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_laserHitEffectOverride, "EffectOnLaserHitTargets", bazookaGirlExplodingLaser.m_effectOnLaserHitTargets, true);
			AbilityMod.AddToken(tokens, this.m_explosionDamageMod, "ExplosionDamageAmount", string.Empty, bazookaGirlExplodingLaser.m_explosionDamageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_explosionEffectOverride, "EffectOnExplosionHitTargets", bazookaGirlExplodingLaser.m_effectOnExplosionHitTargets, true);
			AbilityMod.AddToken(tokens, this.m_cdrOnDirectHitMod, "CdrOnDirectHit", string.Empty, bazookaGirlExplodingLaser.m_cdrOnDirectHit, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlExplodingLaser bazookaGirlExplodingLaser = base.GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlExplodingLaser;
		bool flag = bazookaGirlExplodingLaser != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat coneWidthAngleMod = this.m_coneWidthAngleMod;
		string prefix = "[ConeWidthAngle]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = bazookaGirlExplodingLaser.m_coneWidthAngle;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(coneWidthAngleMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat coneLengthMod = this.m_coneLengthMod;
		string prefix2 = "[ConeLength]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = bazookaGirlExplodingLaser.m_coneLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(coneLengthMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat coneBackwardOffsetMod = this.m_coneBackwardOffsetMod;
		string prefix3 = "[ConeBackwardOffset]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = bazookaGirlExplodingLaser.m_coneBackwardOffset;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(coneBackwardOffsetMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix4 = "[LaserWidth]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = bazookaGirlExplodingLaser.m_laserWidth;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(laserWidthMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix5 = "[LaserRange]";
		bool showBaseVal5 = flag;
		float baseVal5;
		if (flag)
		{
			baseVal5 = bazookaGirlExplodingLaser.m_laserRange;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + base.PropDesc(laserRangeMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyBool laserPenetrateLosMod = this.m_laserPenetrateLosMod;
		string prefix6 = "[LaserPenetrateLos]";
		bool showBaseVal6 = flag;
		bool baseVal6;
		if (flag)
		{
			baseVal6 = bazookaGirlExplodingLaser.m_laserPenetrateLos;
		}
		else
		{
			baseVal6 = false;
		}
		text = str6 + base.PropDesc(laserPenetrateLosMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt laserDamageMod = this.m_laserDamageMod;
		string prefix7 = "[Laser Damage]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = bazookaGirlExplodingLaser.m_laserDamageAmount;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + AbilityModHelper.GetModPropertyDesc(laserDamageMod, prefix7, showBaseVal7, baseVal7);
		text += AbilityModHelper.GetModPropertyDesc(this.m_laserIgnoreCoverMod, "[Laser Ignore Cover?]", flag, flag && bazookaGirlExplodingLaser.m_laserIgnoreCover);
		string str8 = text;
		AbilityModPropertyEffectInfo laserHitEffectOverride = this.m_laserHitEffectOverride;
		string prefix8 = "{ Laser Enemy Hit Effect Override }";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
		if (flag)
		{
			baseVal8 = bazookaGirlExplodingLaser.m_effectOnLaserHitTargets;
		}
		else
		{
			baseVal8 = null;
		}
		text = str8 + AbilityModHelper.GetModPropertyDesc(laserHitEffectOverride, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt cdrOnDirectHitMod = this.m_cdrOnDirectHitMod;
		string prefix9 = "[CdrOnDirectHit]";
		bool showBaseVal9 = flag;
		int baseVal9;
		if (flag)
		{
			baseVal9 = bazookaGirlExplodingLaser.m_cdrOnDirectHit;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(cdrOnDirectHitMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt explosionDamageMod = this.m_explosionDamageMod;
		string prefix10 = "[Explosion Damage]";
		bool showBaseVal10 = flag;
		int baseVal10;
		if (flag)
		{
			baseVal10 = bazookaGirlExplodingLaser.m_explosionDamageAmount;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + AbilityModHelper.GetModPropertyDesc(explosionDamageMod, prefix10, showBaseVal10, baseVal10);
		text += AbilityModHelper.GetModPropertyDesc(this.m_explosionIgnoreLosMod, "[Explosion Ignore LoS?]", flag, flag && bazookaGirlExplodingLaser.m_explosionPenetrateLos);
		text += AbilityModHelper.GetModPropertyDesc(this.m_explosionIgnoreCoverMod, "[Explosion Ignore Cover?]", flag, flag && bazookaGirlExplodingLaser.m_explosionIgnoreCover);
		string str11 = text;
		AbilityModPropertyEffectInfo explosionEffectOverride = this.m_explosionEffectOverride;
		string prefix11 = "{ Explosion Enemy Hit Effect Override }";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal11;
		if (flag)
		{
			baseVal11 = bazookaGirlExplodingLaser.m_effectOnExplosionHitTargets;
		}
		else
		{
			baseVal11 = null;
		}
		return str11 + AbilityModHelper.GetModPropertyDesc(explosionEffectOverride, prefix11, showBaseVal11, baseVal11);
	}
}
