// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SamuraiWindBlade : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_minRangeBeforeBendMod;
	public AbilityModPropertyFloat m_maxRangeBeforeBendMod;
	public AbilityModPropertyFloat m_maxTotalRangeMod;
	public AbilityModPropertyFloat m_maxBendAngleMod;
	public AbilityModPropertyBool m_penetrateLoSMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	[Header("-- Damage")]
	public AbilityModPropertyInt m_laserDamageAmountMod;
	public AbilityModPropertyInt m_damageChangePerTargetMod;
	public AbilityModPropertyEffectInfo m_laserHitEffectMod;
	[Header("-- Shielding per enemy hit on start of Next Turn")]
	public AbilityModPropertyInt m_shieldingPerEnemyHitNextTurnMod;
	public AbilityModPropertyInt m_shieldingDurationMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SamuraiWindBlade);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SamuraiWindBlade samuraiWindBlade = targetAbility as SamuraiWindBlade;
		if (samuraiWindBlade != null)
		{
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, samuraiWindBlade.m_laserWidth);
			AddToken(tokens, m_minRangeBeforeBendMod, "MinRangeBeforeBend", string.Empty, samuraiWindBlade.m_minRangeBeforeBend);
			AddToken(tokens, m_maxRangeBeforeBendMod, "MaxRangeBeforeBend", string.Empty, samuraiWindBlade.m_maxRangeBeforeBend);
			AddToken(tokens, m_maxTotalRangeMod, "MaxTotalRange", string.Empty, samuraiWindBlade.m_maxTotalRange);
			AddToken(tokens, m_maxBendAngleMod, "MaxBendAngle", string.Empty, samuraiWindBlade.m_maxBendAngle);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, samuraiWindBlade.m_maxTargets);
			AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, samuraiWindBlade.m_laserDamageAmount);
			AddToken(tokens, m_damageChangePerTargetMod, "DamageChangePerTarget", string.Empty, samuraiWindBlade.m_damageChangePerTarget);
			AddToken_EffectMod(tokens, m_laserHitEffectMod, "LaserHitEffect", samuraiWindBlade.m_laserHitEffect);
			AddToken(tokens, m_shieldingPerEnemyHitNextTurnMod, "ShieldingPerEnemyHitNextTurn", string.Empty, samuraiWindBlade.m_shieldingPerEnemyHitNextTurn);
			AddToken(tokens, m_shieldingDurationMod, "ShieldingDuration", string.Empty, samuraiWindBlade.m_shieldingDuration);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility) in rogues
	{
		// reactor
		SamuraiWindBlade samuraiWindBlade = GetTargetAbilityOnAbilityData(abilityData) as SamuraiWindBlade;
		// rogues
		// SamuraiWindBlade samuraiWindBlade = targetAbility as SamuraiWindBlade;
		
		bool isValid = samuraiWindBlade != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isValid, isValid ? samuraiWindBlade.m_laserWidth : 0f);
		desc += PropDesc(m_minRangeBeforeBendMod, "[MinRangeBeforeBend]", isValid, isValid ? samuraiWindBlade.m_minRangeBeforeBend : 0f);
		desc += PropDesc(m_maxRangeBeforeBendMod, "[MaxRangeBeforeBend]", isValid, isValid ? samuraiWindBlade.m_maxRangeBeforeBend : 0f);
		desc += PropDesc(m_maxTotalRangeMod, "[MaxTotalRange]", isValid, isValid ? samuraiWindBlade.m_maxTotalRange : 0f);
		desc += PropDesc(m_maxBendAngleMod, "[MaxBendAngle]", isValid, isValid ? samuraiWindBlade.m_maxBendAngle : 0f);
		desc += PropDesc(m_penetrateLoSMod, "[PenetrateLoS]", isValid, isValid && samuraiWindBlade.m_penetrateLoS);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? samuraiWindBlade.m_maxTargets : 0);
		desc += PropDesc(m_laserDamageAmountMod, "[LaserDamageAmount]", isValid, isValid ? samuraiWindBlade.m_laserDamageAmount : 0);
		desc += PropDesc(m_damageChangePerTargetMod, "[DamageChangePerTarget]", isValid, isValid ? samuraiWindBlade.m_damageChangePerTarget : 0);
		desc += PropDesc(m_laserHitEffectMod, "[LaserHitEffect]", isValid, isValid ? samuraiWindBlade.m_laserHitEffect : null);
		desc += PropDesc(m_shieldingPerEnemyHitNextTurnMod, "[ShieldingPerEnemyHitNextTurn]", isValid, isValid ? samuraiWindBlade.m_shieldingPerEnemyHitNextTurn : 0);
		return desc + PropDesc(m_shieldingDurationMod, "[ShieldingDuration]", isValid, isValid ? samuraiWindBlade.m_shieldingDuration : 0);
	}
}
