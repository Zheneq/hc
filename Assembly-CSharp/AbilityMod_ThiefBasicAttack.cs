using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_ThiefBasicAttack : AbilityMod
{
	[Header("-- Targeter")]
	public AbilityModPropertyFloat m_targeterMaxAngleMod;
	[Header("-- Damage")]
	public AbilityModPropertyInt m_laserDamageAmountMod;
	public AbilityModPropertyInt m_laserSubsequentDamageAmountMod;
	public AbilityModPropertyInt m_extraDamageForSingleHitMod;
	public AbilityModPropertyInt m_extraDamageForHittingPowerupMod;
	[Header("-- Healing")]
	public AbilityModPropertyInt m_healOnSelfIfHitEnemyAndPowerupMod;
	[Header("-- Energy")]
	public AbilityModPropertyInt m_energyGainPerLaserHitMod;
	public AbilityModPropertyInt m_energyGainPerPowerupHitMod;
	[Header("-- Laser Properties")]
	public AbilityModPropertyFloat m_laserRangeMod;
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyInt m_laserMaxTargetsMod;
	public AbilityModPropertyInt m_laserCountMod;
	public AbilityModPropertyBool m_laserPenetrateLosMod;
	[Header("-- PowerUp/Spoils Interaction")]
	public AbilityModPropertyBool m_stopOnPowerupHitMod;
	public AbilityModPropertyBool m_includeSpoilsPowerupsMod;
	public AbilityModPropertyBool m_ignorePickupTeamRestrictionMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ThiefBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ThiefBasicAttack thiefBasicAttack = targetAbility as ThiefBasicAttack;
		if (thiefBasicAttack != null)
		{
			AddToken(tokens, m_targeterMaxAngleMod, "TargeterMaxAngle", string.Empty, thiefBasicAttack.m_targeterMaxAngle);
			AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, thiefBasicAttack.m_laserDamageAmount);
			AddToken(tokens, m_laserSubsequentDamageAmountMod, "LaserSubsequentDamageAmount", string.Empty, thiefBasicAttack.m_laserSubsequentDamageAmount);
			AddToken(tokens, m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, thiefBasicAttack.m_extraDamageForSingleHit);
			AddToken(tokens, m_extraDamageForHittingPowerupMod, "ExtraDamageForHittingPowerup", string.Empty, thiefBasicAttack.m_extraDamageForHittingPowerup);
			AddToken(tokens, m_healOnSelfIfHitEnemyAndPowerupMod, "HealOnSelfIfHitEnemyAndPowerup", string.Empty, thiefBasicAttack.m_healOnSelfIfHitEnemyAndPowerup);
			AddToken(tokens, m_energyGainPerLaserHitMod, "EnergyGainPerLaserHit", string.Empty, thiefBasicAttack.m_energyGainPerLaserHit);
			AddToken(tokens, m_energyGainPerPowerupHitMod, "EnergyGainPerPowerupHit", string.Empty, thiefBasicAttack.m_energyGainPerPowerupHit);
			AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, thiefBasicAttack.m_laserRange);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, thiefBasicAttack.m_laserWidth);
			AddToken(tokens, m_laserMaxTargetsMod, "LaserMaxTargets", string.Empty, thiefBasicAttack.m_laserMaxTargets);
			AddToken(tokens, m_laserCountMod, "LaserCount", string.Empty, thiefBasicAttack.m_laserCount);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefBasicAttack thiefBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as ThiefBasicAttack;
		bool isValid = thiefBasicAttack != null;
		string desc = string.Empty;
		desc += PropDesc(m_targeterMaxAngleMod, "[TargeterMaxAngle]", isValid, isValid ? thiefBasicAttack.m_targeterMaxAngle : 0f);
		desc += PropDesc(m_laserDamageAmountMod, "[LaserDamageAmount]", isValid, isValid ? thiefBasicAttack.m_laserDamageAmount : 0);
		desc += PropDesc(m_laserSubsequentDamageAmountMod, "[LaserSubsequentDamageAmount]", isValid, isValid ? thiefBasicAttack.m_laserSubsequentDamageAmount : 0);
		desc += PropDesc(m_extraDamageForSingleHitMod, "[ExtraDamageForSingleHit]", isValid, isValid ? thiefBasicAttack.m_extraDamageForSingleHit : 0);
		desc += PropDesc(m_extraDamageForHittingPowerupMod, "[ExtraDamageForHittingPowerup]", isValid, isValid ? thiefBasicAttack.m_extraDamageForHittingPowerup : 0);
		desc += PropDesc(m_healOnSelfIfHitEnemyAndPowerupMod, "[HealOnSelfIfHitEnemyAndPowerup]", isValid, isValid ? thiefBasicAttack.m_healOnSelfIfHitEnemyAndPowerup : 0);
		desc += PropDesc(m_energyGainPerLaserHitMod, "[EnergyGainPerLaserHit]", isValid, isValid ? thiefBasicAttack.m_energyGainPerLaserHit : 0);
		desc += PropDesc(m_energyGainPerPowerupHitMod, "[EnergyGainPerPowerupHit]", isValid, isValid ? thiefBasicAttack.m_energyGainPerPowerupHit : 0);
		desc += PropDesc(m_laserRangeMod, "[LaserRange]", isValid, isValid ? thiefBasicAttack.m_laserRange : 0f);
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isValid, isValid ? thiefBasicAttack.m_laserWidth : 0f);
		desc += PropDesc(m_laserMaxTargetsMod, "[LaserMaxTargets]", isValid, isValid ? thiefBasicAttack.m_laserMaxTargets : 0);
		desc += PropDesc(m_laserCountMod, "[LaserCount]", isValid, isValid ? thiefBasicAttack.m_laserCount : 0);
		desc += PropDesc(m_laserPenetrateLosMod, "[LaserPenetrateLos]", isValid, isValid && thiefBasicAttack.m_laserPenetrateLos);
		desc += PropDesc(m_stopOnPowerupHitMod, "[StopOnPowerupHit]", isValid, isValid && thiefBasicAttack.m_stopOnPowerupHit);
		desc += PropDesc(m_includeSpoilsPowerupsMod, "[IncludeSpoilsPowerups]", isValid, isValid && thiefBasicAttack.m_includeSpoilsPowerups);
		return new StringBuilder().Append(desc).Append(PropDesc(m_ignorePickupTeamRestrictionMod, "[IgnorePickupTeamRestriction]", isValid, isValid && thiefBasicAttack.m_ignorePickupTeamRestriction)).ToString();
	}
}
