using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TricksterBasicAttack : AbilityMod
{
	[Header("-- Laser Targeting")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;
	[Header("-- Damage and Effect")]
	public AbilityModPropertyInt m_laserDamageAmountMod;
	public AbilityModPropertyInt m_laserSubsequentDamageAmountMod;
	public AbilityModPropertyInt m_extraDamageForSingleHitMod;
	public AbilityModPropertyEffectInfo m_enemySingleHitHitEffectMod;
	public AbilityModPropertyEffectInfo m_enemyMultiHitEffectMod;
	[Header("-- Effect on Self for Multi Hit")]
	public AbilityModPropertyEffectInfo m_selfEffectForMultiHitMod;
	[Header("-- Energy Gain --")]
	public AbilityModPropertyInt m_energyGainPerLaserHitMod;
	[Header("-- For spawning spoils")]
	public AbilityModPropertySpoilsSpawnData m_spoilSpawnInfoMod;
	public AbilityModPropertyBool m_onlySpawnSpoilOnMultiHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(TricksterBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TricksterBasicAttack tricksterBasicAttack = targetAbility as TricksterBasicAttack;
		if (tricksterBasicAttack != null)
		{
			AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", tricksterBasicAttack.m_laserInfo);
			AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, tricksterBasicAttack.m_laserDamageAmount);
			AddToken(tokens, m_laserSubsequentDamageAmountMod, "LaserSubsequentDamageAmount", string.Empty, tricksterBasicAttack.m_laserSubsequentDamageAmount);
			AddToken(tokens, m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, tricksterBasicAttack.m_extraDamageForSingleHit);
			AddToken_EffectMod(tokens, m_enemySingleHitHitEffectMod, "EnemySingleHitHitEffect", tricksterBasicAttack.m_enemySingleHitHitEffect);
			AddToken_EffectMod(tokens, m_enemyMultiHitEffectMod, "EnemyMultiHitEffect", tricksterBasicAttack.m_enemyMultiHitEffect);
			AddToken_EffectMod(tokens, m_selfEffectForMultiHitMod, "SelfEffectForMultiHit", tricksterBasicAttack.m_selfEffectForMultiHit);
			AddToken(tokens, m_energyGainPerLaserHitMod, "EnergyGainPerLaserHit", string.Empty, tricksterBasicAttack.m_energyGainPerLaserHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TricksterBasicAttack tricksterBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as TricksterBasicAttack;
		bool isValid = tricksterBasicAttack != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserInfoMod, "[LaserInfo]", isValid, isValid ? tricksterBasicAttack.m_laserInfo : null);
		desc += PropDesc(m_laserDamageAmountMod, "[LaserDamageAmount]", isValid, isValid ? tricksterBasicAttack.m_laserDamageAmount : 0);
		desc += PropDesc(m_laserSubsequentDamageAmountMod, "[LaserSubsequentDamageAmount]", isValid, isValid ? tricksterBasicAttack.m_laserSubsequentDamageAmount : 0);
		desc += PropDesc(m_extraDamageForSingleHitMod, "[ExtraDamageForSingleHit]", isValid, isValid ? tricksterBasicAttack.m_extraDamageForSingleHit : 0);
		desc += PropDesc(m_enemySingleHitHitEffectMod, "[EnemySingleHitHitEffect]", isValid, isValid ? tricksterBasicAttack.m_enemySingleHitHitEffect : null);
		desc += PropDesc(m_enemyMultiHitEffectMod, "[EnemyMultiHitEffect]", isValid, isValid ? tricksterBasicAttack.m_enemyMultiHitEffect : null);
		desc += PropDesc(m_selfEffectForMultiHitMod, "[SelfEffectForMultiHit]", isValid, isValid ? tricksterBasicAttack.m_selfEffectForMultiHit : null);
		desc += PropDesc(m_energyGainPerLaserHitMod, "[EnergyGainPerLaserHit]", isValid, isValid ? tricksterBasicAttack.m_energyGainPerLaserHit : 0);
		desc += PropDesc(m_spoilSpawnInfoMod, "[SpoilSpawnInfo]", isValid, isValid ? tricksterBasicAttack.m_spoilSpawnInfo : null);
		return desc + PropDesc(m_onlySpawnSpoilOnMultiHitMod, "[OnlySpawnSpoilOnMultiHit]", isValid, isValid && tricksterBasicAttack.m_onlySpawnSpoilOnMultiHit);
	}
}
