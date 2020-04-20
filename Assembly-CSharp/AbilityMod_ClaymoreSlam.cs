using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClaymoreSlam : AbilityMod
{
	[Header("-- Laser Targeting")]
	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyFloat m_midLaserWidthMod;

	public AbilityModPropertyFloat m_fullLaserWidthMod;

	public AbilityModPropertyInt m_laserMaxTargetsMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	[Header("-- Middle Hit Damage and Effects")]
	public AbilityModPropertyInt m_middleDamageMod;

	public AbilityModPropertyEffectInfo m_middleEnemyHitEffectMod;

	[Header("-- Side Hit Damage and Effects")]
	public AbilityModPropertyInt m_sideDamageMod;

	public AbilityModPropertyEffectInfo m_sideEnemyHitEffectMod;

	public AbilityModPropertyInt m_extraSideDamagePerMiddleHitMod;

	[Header("-- Extra Damage from Target Health Threshold (0 to 1) --")]
	public AbilityModPropertyInt m_extraDamageOnLowHealthTargetMod;

	public AbilityModPropertyFloat m_lowHealthThresholdMod;

	[Header("-- Energy Loss on Enemy")]
	public AbilityModPropertyInt m_energyLossOnMidHitMod;

	public AbilityModPropertyInt m_energyLossOnSideHitMod;

	[Header("-- Self Healing")]
	public AbilityModPropertyInt m_healPerMidHit;

	public AbilityModPropertyInt m_healPerSideHit;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClaymoreSlam);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClaymoreSlam claymoreSlam = targetAbility as ClaymoreSlam;
		if (claymoreSlam != null)
		{
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", "laser range", claymoreSlam.m_laserRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_midLaserWidthMod, "LaserWidthMiddle", "laser width, middle portion", claymoreSlam.m_midLaserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_fullLaserWidthMod, "LaserWidthFull", "laser width, side", claymoreSlam.m_fullLaserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserMaxTargetsMod, "LaserMaxTargets", "max number of targets hit", claymoreSlam.m_laserMaxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_middleDamageMod, "Damage_Middle", "damage for middle hit", claymoreSlam.m_middleDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_middleEnemyHitEffectMod, "Effect_MiddleHit", claymoreSlam.m_middleEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_sideDamageMod, "Damage_Side", "damage for side hit", claymoreSlam.m_sideDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_sideEnemyHitEffectMod, "Effect_SideHit", claymoreSlam.m_sideEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraSideDamagePerMiddleHitMod, "ExtraSideDamagePerMiddleHit", string.Empty, claymoreSlam.m_extraSideDamagePerMiddleHit, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageOnLowHealthTargetMod, "ExtraDamageOnLowHealthTarget", string.Empty, claymoreSlam.m_extraDamageOnLowHealthTarget, true, false);
			AbilityMod.AddToken(tokens, this.m_lowHealthThresholdMod, "LowHealthThreshold", string.Empty, claymoreSlam.m_lowHealthThreshold, true, false, false);
			AbilityMod.AddToken(tokens, this.m_energyLossOnMidHitMod, "EnergyLossOnMidHit", string.Empty, claymoreSlam.m_energyLossOnMidHit, true, false);
			AbilityMod.AddToken(tokens, this.m_energyLossOnSideHitMod, "EnergyLossOnSideHit", string.Empty, claymoreSlam.m_energyLossOnSideHit, true, false);
			AbilityMod.AddToken(tokens, this.m_healPerMidHit, "HealPerMidHit", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_healPerSideHit, "HealPerSideHit", string.Empty, 0, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreSlam claymoreSlam = base.GetTargetAbilityOnAbilityData(abilityData) as ClaymoreSlam;
		bool flag = claymoreSlam != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_laserRangeMod, "[Laser Range]", flag, (!flag) ? 0f : claymoreSlam.m_laserRange);
		string str = text;
		AbilityModPropertyFloat midLaserWidthMod = this.m_midLaserWidthMod;
		string prefix = "[Laser Middle Width]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = claymoreSlam.m_midLaserWidth;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(midLaserWidthMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat fullLaserWidthMod = this.m_fullLaserWidthMod;
		string prefix2 = "[Laser Full Width]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = claymoreSlam.m_fullLaserWidth;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(fullLaserWidthMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt laserMaxTargetsMod = this.m_laserMaxTargetsMod;
		string prefix3 = "[Laser Max Targets]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = claymoreSlam.m_laserMaxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(laserMaxTargetsMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool penetrateLosMod = this.m_penetrateLosMod;
		string prefix4 = "[Laser Ignore Los]";
		bool showBaseVal4 = flag;
		bool baseVal4;
		if (flag)
		{
			baseVal4 = claymoreSlam.m_penetrateLos;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(penetrateLosMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt middleDamageMod = this.m_middleDamageMod;
		string prefix5 = "[Damage, Middle]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = claymoreSlam.m_middleDamage;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(middleDamageMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_middleEnemyHitEffectMod, "{ Enemy Effect, Middle }", flag, (!flag) ? null : claymoreSlam.m_middleEnemyHitEffect);
		string str6 = text;
		AbilityModPropertyInt sideDamageMod = this.m_sideDamageMod;
		string prefix6 = "[Damage, Side]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = claymoreSlam.m_sideDamage;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(sideDamageMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectInfo sideEnemyHitEffectMod = this.m_sideEnemyHitEffectMod;
		string prefix7 = "{ Enemy Effect, Side }";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
		if (flag)
		{
			baseVal7 = claymoreSlam.m_sideEnemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(sideEnemyHitEffectMod, prefix7, showBaseVal7, baseVal7);
		text += base.PropDesc(this.m_extraSideDamagePerMiddleHitMod, "[ExtraSideDamagePerMiddleHit]", flag, (!flag) ? 0 : claymoreSlam.m_extraSideDamagePerMiddleHit);
		text += base.PropDesc(this.m_extraDamageOnLowHealthTargetMod, "[ExtraDamageOnLowHealthTarget]", flag, (!flag) ? 0 : claymoreSlam.m_extraDamageOnLowHealthTarget);
		string str8 = text;
		AbilityModPropertyFloat lowHealthThresholdMod = this.m_lowHealthThresholdMod;
		string prefix8 = "[LowHealthThreshold]";
		bool showBaseVal8 = flag;
		float baseVal8;
		if (flag)
		{
			baseVal8 = claymoreSlam.m_lowHealthThreshold;
		}
		else
		{
			baseVal8 = 0f;
		}
		text = str8 + base.PropDesc(lowHealthThresholdMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt energyLossOnMidHitMod = this.m_energyLossOnMidHitMod;
		string prefix9 = "[EnergyLossOnMidHit]";
		bool showBaseVal9 = flag;
		int baseVal9;
		if (flag)
		{
			baseVal9 = claymoreSlam.m_energyLossOnMidHit;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(energyLossOnMidHitMod, prefix9, showBaseVal9, baseVal9);
		text += base.PropDesc(this.m_energyLossOnSideHitMod, "[EnergyLossOnSideHit]", flag, (!flag) ? 0 : claymoreSlam.m_energyLossOnSideHit);
		text += base.PropDesc(this.m_healPerMidHit, "[HealPerMidHit]", flag, 0);
		return text + base.PropDesc(this.m_healPerSideHit, "[HealPerSideHit]", flag, 0);
	}
}
