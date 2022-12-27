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
			AddToken_ConeInfo(tokens, m_coneInfoMod, "ConeInfo", soldierConeOrLaser.m_coneInfo);
			AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", soldierConeOrLaser.m_laserInfo);
			AddToken(tokens, m_coneDamageMod, "ConeDamage", string.Empty, soldierConeOrLaser.m_coneDamage);
			AddToken_EffectMod(tokens, m_coneEnemyHitEffectMod, "ConeEnemyHitEffect", soldierConeOrLaser.m_coneEnemyHitEffect);
			AddToken(tokens, m_laserDamageMod, "LaserDamage", string.Empty, soldierConeOrLaser.m_laserDamage);
			AddToken_EffectMod(tokens, m_laserEnemyHitEffectMod, "LaserEnemyHitEffect", soldierConeOrLaser.m_laserEnemyHitEffect);
			AddToken(tokens, m_extraDamageForAlternatingMod, "ExtraDamageForAlternating", string.Empty, soldierConeOrLaser.m_extraDamageForAlternating);
			AddToken(tokens, m_closeDistThresholdMod, "CloseDistThreshold", string.Empty, soldierConeOrLaser.m_closeDistThreshold);
			AddToken(tokens, m_extraDamageForNearTargetMod, "ExtraDamageForNearTarget", string.Empty, soldierConeOrLaser.m_extraDamageForNearTarget);
			AddToken(tokens, m_extraDamageForFromCoverMod, "ExtraDamageForFromCover", string.Empty, soldierConeOrLaser.m_extraDamageForFromCover);
			AddToken(tokens, m_extraDamageToEvadersMod, "ExtraDamageToEvaders", string.Empty, soldierConeOrLaser.m_extraDamageToEvaders);
			AddToken(tokens, m_extraEnergyForConeMod, "ExtraEnergyForCone", string.Empty, soldierConeOrLaser.m_extraEnergyForCone, true, true);
			AddToken(tokens, m_extraEnergyForLaserMod, "ExtraEnergyForLaser", string.Empty, soldierConeOrLaser.m_extraEnergyForLaser, true, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierConeOrLaser soldierConeOrLaser = GetTargetAbilityOnAbilityData(abilityData) as SoldierConeOrLaser;
		bool isValid = soldierConeOrLaser != null;
		string desc = string.Empty;
		desc += PropDesc(m_coneInfoMod, "[ConeInfo]", isValid, isValid ? soldierConeOrLaser.m_coneInfo : null);
		desc += PropDesc(m_laserInfoMod, "[LaserInfo]", isValid, isValid ? soldierConeOrLaser.m_laserInfo : null);
		desc += PropDesc(m_coneDamageMod, "[ConeDamage]", isValid, isValid ? soldierConeOrLaser.m_coneDamage : 0);
		desc += PropDesc(m_coneEnemyHitEffectMod, "[ConeEnemyHitEffect]", isValid, isValid ? soldierConeOrLaser.m_coneEnemyHitEffect : null);
		desc += PropDesc(m_laserDamageMod, "[LaserDamage]", isValid, isValid ? soldierConeOrLaser.m_laserDamage : 0);
		desc += PropDesc(m_laserEnemyHitEffectMod, "[LaserEnemyHitEffect]", isValid, isValid ? soldierConeOrLaser.m_laserEnemyHitEffect : null);
		desc += PropDesc(m_extraDamageForAlternatingMod, "[ExtraDamageForAlternating]", isValid, isValid ? soldierConeOrLaser.m_extraDamageForAlternating : 0);
		desc += PropDesc(m_closeDistThresholdMod, "[CloseDistThreshold]", isValid, isValid ? soldierConeOrLaser.m_closeDistThreshold : 0f);
		desc += PropDesc(m_extraDamageForNearTargetMod, "[ExtraDamageForNearTarget]", isValid, isValid ? soldierConeOrLaser.m_extraDamageForNearTarget : 0);
		desc += PropDesc(m_extraDamageForFromCoverMod, "[ExtraDamageForFromCover]", isValid, isValid ? soldierConeOrLaser.m_extraDamageForFromCover : 0);
		desc += PropDesc(m_extraDamageToEvadersMod, "[ExtraDamageToEvaders]", isValid, isValid ? soldierConeOrLaser.m_extraDamageToEvaders : 0);
		desc += PropDesc(m_extraEnergyForConeMod, "[ExtraEnergyForCone]", isValid, isValid ? soldierConeOrLaser.m_extraEnergyForCone : 0);
		return desc + PropDesc(m_extraEnergyForLaserMod, "[ExtraEnergyForLaser]", isValid, isValid ? soldierConeOrLaser.m_extraEnergyForLaser : 0);
	}
}
