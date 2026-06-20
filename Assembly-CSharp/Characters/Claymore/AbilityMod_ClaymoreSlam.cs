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
			AddToken(tokens, m_laserRangeMod, "LaserRange", "laser range", claymoreSlam.m_laserRange);
			AddToken(tokens, m_midLaserWidthMod, "LaserWidthMiddle", "laser width, middle portion", claymoreSlam.m_midLaserWidth);
			AddToken(tokens, m_fullLaserWidthMod, "LaserWidthFull", "laser width, side", claymoreSlam.m_fullLaserWidth);
			AddToken(tokens, m_laserMaxTargetsMod, "LaserMaxTargets", "max number of targets hit", claymoreSlam.m_laserMaxTargets);
			AddToken(tokens, m_middleDamageMod, "Damage_Middle", "damage for middle hit", claymoreSlam.m_middleDamage);
			AddToken_EffectMod(tokens, m_middleEnemyHitEffectMod, "Effect_MiddleHit", claymoreSlam.m_middleEnemyHitEffect);
			AddToken(tokens, m_sideDamageMod, "Damage_Side", "damage for side hit", claymoreSlam.m_sideDamage);
			AddToken_EffectMod(tokens, m_sideEnemyHitEffectMod, "Effect_SideHit", claymoreSlam.m_sideEnemyHitEffect);
			AddToken(tokens, m_extraSideDamagePerMiddleHitMod, "ExtraSideDamagePerMiddleHit", string.Empty, claymoreSlam.m_extraSideDamagePerMiddleHit);
			AddToken(tokens, m_extraDamageOnLowHealthTargetMod, "ExtraDamageOnLowHealthTarget", string.Empty, claymoreSlam.m_extraDamageOnLowHealthTarget);
			AddToken(tokens, m_lowHealthThresholdMod, "LowHealthThreshold", string.Empty, claymoreSlam.m_lowHealthThreshold);
			AddToken(tokens, m_energyLossOnMidHitMod, "EnergyLossOnMidHit", string.Empty, claymoreSlam.m_energyLossOnMidHit);
			AddToken(tokens, m_energyLossOnSideHitMod, "EnergyLossOnSideHit", string.Empty, claymoreSlam.m_energyLossOnSideHit);
			AddToken(tokens, m_healPerMidHit, "HealPerMidHit", string.Empty, 0);
			AddToken(tokens, m_healPerSideHit, "HealPerSideHit", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreSlam claymoreSlam = GetTargetAbilityOnAbilityData(abilityData) as ClaymoreSlam;
		bool isAbilityPresent = claymoreSlam != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserRangeMod, "[Laser Range]", isAbilityPresent, isAbilityPresent ? claymoreSlam.m_laserRange : 0f);
		desc += PropDesc(m_midLaserWidthMod, "[Laser Middle Width]", isAbilityPresent, isAbilityPresent ? claymoreSlam.m_midLaserWidth : 0f);
		desc += PropDesc(m_fullLaserWidthMod, "[Laser Full Width]", isAbilityPresent, isAbilityPresent ? claymoreSlam.m_fullLaserWidth : 0f);
		desc += PropDesc(m_laserMaxTargetsMod, "[Laser Max Targets]", isAbilityPresent, isAbilityPresent ? claymoreSlam.m_laserMaxTargets : 0);
		desc += PropDesc(m_penetrateLosMod, "[Laser Ignore Los]", isAbilityPresent, isAbilityPresent && claymoreSlam.m_penetrateLos);
		desc += PropDesc(m_middleDamageMod, "[Damage, Middle]", isAbilityPresent, isAbilityPresent ? claymoreSlam.m_middleDamage : 0);
		desc += PropDesc(m_middleEnemyHitEffectMod, "{ Enemy Effect, Middle }", isAbilityPresent, isAbilityPresent ? claymoreSlam.m_middleEnemyHitEffect : null);
		desc += PropDesc(m_sideDamageMod, "[Damage, Side]", isAbilityPresent, isAbilityPresent ? claymoreSlam.m_sideDamage : 0);
		desc += PropDesc(m_sideEnemyHitEffectMod, "{ Enemy Effect, Side }", isAbilityPresent, isAbilityPresent ? claymoreSlam.m_sideEnemyHitEffect : null);
		desc += PropDesc(m_extraSideDamagePerMiddleHitMod, "[ExtraSideDamagePerMiddleHit]", isAbilityPresent, isAbilityPresent ? claymoreSlam.m_extraSideDamagePerMiddleHit : 0);
		desc += PropDesc(m_extraDamageOnLowHealthTargetMod, "[ExtraDamageOnLowHealthTarget]", isAbilityPresent, isAbilityPresent ? claymoreSlam.m_extraDamageOnLowHealthTarget : 0);
		desc += PropDesc(m_lowHealthThresholdMod, "[LowHealthThreshold]", isAbilityPresent, isAbilityPresent ? claymoreSlam.m_lowHealthThreshold : 0f);
		desc += PropDesc(m_energyLossOnMidHitMod, "[EnergyLossOnMidHit]", isAbilityPresent, isAbilityPresent ? claymoreSlam.m_energyLossOnMidHit : 0);
		desc += PropDesc(m_energyLossOnSideHitMod, "[EnergyLossOnSideHit]", isAbilityPresent, isAbilityPresent ? claymoreSlam.m_energyLossOnSideHit : 0);
		desc += PropDesc(m_healPerMidHit, "[HealPerMidHit]", isAbilityPresent);
		return desc + PropDesc(m_healPerSideHit, "[HealPerSideHit]", isAbilityPresent);
	}
}
