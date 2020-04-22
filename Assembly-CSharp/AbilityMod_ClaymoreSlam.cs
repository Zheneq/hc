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
			AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", "laser range", claymoreSlam.m_laserRange);
			AbilityMod.AddToken(tokens, m_midLaserWidthMod, "LaserWidthMiddle", "laser width, middle portion", claymoreSlam.m_midLaserWidth);
			AbilityMod.AddToken(tokens, m_fullLaserWidthMod, "LaserWidthFull", "laser width, side", claymoreSlam.m_fullLaserWidth);
			AbilityMod.AddToken(tokens, m_laserMaxTargetsMod, "LaserMaxTargets", "max number of targets hit", claymoreSlam.m_laserMaxTargets);
			AbilityMod.AddToken(tokens, m_middleDamageMod, "Damage_Middle", "damage for middle hit", claymoreSlam.m_middleDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_middleEnemyHitEffectMod, "Effect_MiddleHit", claymoreSlam.m_middleEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_sideDamageMod, "Damage_Side", "damage for side hit", claymoreSlam.m_sideDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_sideEnemyHitEffectMod, "Effect_SideHit", claymoreSlam.m_sideEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_extraSideDamagePerMiddleHitMod, "ExtraSideDamagePerMiddleHit", string.Empty, claymoreSlam.m_extraSideDamagePerMiddleHit);
			AbilityMod.AddToken(tokens, m_extraDamageOnLowHealthTargetMod, "ExtraDamageOnLowHealthTarget", string.Empty, claymoreSlam.m_extraDamageOnLowHealthTarget);
			AbilityMod.AddToken(tokens, m_lowHealthThresholdMod, "LowHealthThreshold", string.Empty, claymoreSlam.m_lowHealthThreshold);
			AbilityMod.AddToken(tokens, m_energyLossOnMidHitMod, "EnergyLossOnMidHit", string.Empty, claymoreSlam.m_energyLossOnMidHit);
			AbilityMod.AddToken(tokens, m_energyLossOnSideHitMod, "EnergyLossOnSideHit", string.Empty, claymoreSlam.m_energyLossOnSideHit);
			AbilityMod.AddToken(tokens, m_healPerMidHit, "HealPerMidHit", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_healPerSideHit, "HealPerSideHit", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreSlam claymoreSlam = GetTargetAbilityOnAbilityData(abilityData) as ClaymoreSlam;
		bool flag = claymoreSlam != null;
		string empty = string.Empty;
		empty += PropDesc(m_laserRangeMod, "[Laser Range]", flag, (!flag) ? 0f : claymoreSlam.m_laserRange);
		string str = empty;
		AbilityModPropertyFloat midLaserWidthMod = m_midLaserWidthMod;
		float baseVal;
		if (flag)
		{
			baseVal = claymoreSlam.m_midLaserWidth;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(midLaserWidthMod, "[Laser Middle Width]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat fullLaserWidthMod = m_fullLaserWidthMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = claymoreSlam.m_fullLaserWidth;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(fullLaserWidthMod, "[Laser Full Width]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt laserMaxTargetsMod = m_laserMaxTargetsMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = claymoreSlam.m_laserMaxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(laserMaxTargetsMod, "[Laser Max Targets]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyBool penetrateLosMod = m_penetrateLosMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = (claymoreSlam.m_penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(penetrateLosMod, "[Laser Ignore Los]", flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyInt middleDamageMod = m_middleDamageMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = claymoreSlam.m_middleDamage;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(middleDamageMod, "[Damage, Middle]", flag, baseVal5);
		empty += PropDesc(m_middleEnemyHitEffectMod, "{ Enemy Effect, Middle }", flag, (!flag) ? null : claymoreSlam.m_middleEnemyHitEffect);
		string str6 = empty;
		AbilityModPropertyInt sideDamageMod = m_sideDamageMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = claymoreSlam.m_sideDamage;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(sideDamageMod, "[Damage, Side]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectInfo sideEnemyHitEffectMod = m_sideEnemyHitEffectMod;
		object baseVal7;
		if (flag)
		{
			baseVal7 = claymoreSlam.m_sideEnemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(sideEnemyHitEffectMod, "{ Enemy Effect, Side }", flag, (StandardEffectInfo)baseVal7);
		empty += PropDesc(m_extraSideDamagePerMiddleHitMod, "[ExtraSideDamagePerMiddleHit]", flag, flag ? claymoreSlam.m_extraSideDamagePerMiddleHit : 0);
		empty += PropDesc(m_extraDamageOnLowHealthTargetMod, "[ExtraDamageOnLowHealthTarget]", flag, flag ? claymoreSlam.m_extraDamageOnLowHealthTarget : 0);
		string str8 = empty;
		AbilityModPropertyFloat lowHealthThresholdMod = m_lowHealthThresholdMod;
		float baseVal8;
		if (flag)
		{
			baseVal8 = claymoreSlam.m_lowHealthThreshold;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str8 + PropDesc(lowHealthThresholdMod, "[LowHealthThreshold]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyInt energyLossOnMidHitMod = m_energyLossOnMidHitMod;
		int baseVal9;
		if (flag)
		{
			baseVal9 = claymoreSlam.m_energyLossOnMidHit;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(energyLossOnMidHitMod, "[EnergyLossOnMidHit]", flag, baseVal9);
		empty += PropDesc(m_energyLossOnSideHitMod, "[EnergyLossOnSideHit]", flag, flag ? claymoreSlam.m_energyLossOnSideHit : 0);
		empty += PropDesc(m_healPerMidHit, "[HealPerMidHit]", flag);
		return empty + PropDesc(m_healPerSideHit, "[HealPerSideHit]", flag);
	}
}
