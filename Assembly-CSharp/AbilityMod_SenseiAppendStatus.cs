// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SenseiAppendStatus : AbilityMod
{
	[Header("    (( Targeting: If using ActorSquare mode ))")]
	public AbilityModPropertyBool m_canTargetAllyMod;
	public AbilityModPropertyBool m_canTargetEnemyMod;
	public AbilityModPropertyBool m_canTagetSelfMod;
	public AbilityModPropertyBool m_targetingIgnoreLosMod;
	[Header("    (( Targeting: If using Laser mode ))")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;
	[Separator("On Cast Hit Stuff")]
	public AbilityModPropertyEffectData m_enemyCastHitEffectDataMod;
	public AbilityModPropertyEffectData m_allyCastHitEffectDataMod;
	public AbilityModPropertyInt m_energyToAllyTargetOnCastMod;
	[Separator("For Append Effect")]
	public AbilityModPropertyBool m_endEffectIfAppendedStatusMod;
	[Header("-- Effect to append --")]
	public AbilityModPropertyEffectInfo m_effectAddedOnEnemyAttackMod;
	public AbilityModPropertyEffectInfo m_effectAddedOnAllyAttackMod;
	[Space(10f)]
	public AbilityModPropertyInt m_energyGainOnAllyAppendHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SenseiAppendStatus);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SenseiAppendStatus senseiAppendStatus = targetAbility as SenseiAppendStatus;
		if (senseiAppendStatus != null)
		{
			AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", senseiAppendStatus.m_laserInfo);
			AddToken_EffectMod(tokens, m_enemyCastHitEffectDataMod, "EnemyCastHitEffectData", senseiAppendStatus.m_enemyCastHitEffectData);
			AddToken_EffectMod(tokens, m_allyCastHitEffectDataMod, "AllyCastHitEffectData", senseiAppendStatus.m_allyCastHitEffectData);
			AddToken(tokens, m_energyToAllyTargetOnCastMod, "EnergyToAllyTargetOnCast", string.Empty, senseiAppendStatus.m_energyToAllyTargetOnCast);
			AddToken_EffectMod(tokens, m_effectAddedOnEnemyAttackMod, "EffectAddedOnEnemyAttack", senseiAppendStatus.m_effectAddedOnEnemyAttack);
			AddToken_EffectMod(tokens, m_effectAddedOnAllyAttackMod, "EffectAddedOnAllyAttack", senseiAppendStatus.m_effectAddedOnAllyAttack);
			AddToken(tokens, m_energyGainOnAllyAppendHitMod, "EnergyGainOnAllyAppendHit", string.Empty, senseiAppendStatus.m_energyGainOnAllyAppendHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		SenseiAppendStatus senseiAppendStatus = GetTargetAbilityOnAbilityData(abilityData) as SenseiAppendStatus;
		// rogues
		// SenseiAppendStatus senseiAppendStatus = targetAbility as SenseiAppendStatus;
		
		bool isValid = senseiAppendStatus != null;
		string desc = string.Empty;
		desc += PropDesc(m_canTargetAllyMod, "[CanTargetAlly]", isValid, isValid && senseiAppendStatus.m_canTargetAlly);
		desc += PropDesc(m_canTargetEnemyMod, "[CanTargetEnemy]", isValid, isValid && senseiAppendStatus.m_canTargetEnemy);
		desc += PropDesc(m_canTagetSelfMod, "[CanTagetSelf]", isValid, isValid && senseiAppendStatus.m_canTagetSelf);
		desc += PropDesc(m_targetingIgnoreLosMod, "[TargetingIgnoreLos]", isValid, isValid && senseiAppendStatus.m_targetingIgnoreLos);
		desc += PropDesc(m_laserInfoMod, "[LaserInfo]", isValid, isValid ? senseiAppendStatus.m_laserInfo : null);
		desc += PropDesc(m_enemyCastHitEffectDataMod, "[EnemyCastHitEffectData]", isValid, isValid ? senseiAppendStatus.m_enemyCastHitEffectData : null);
		desc += PropDesc(m_allyCastHitEffectDataMod, "[AllyCastHitEffectData]", isValid, isValid ? senseiAppendStatus.m_allyCastHitEffectData : null);
		desc += PropDesc(m_energyToAllyTargetOnCastMod, "[EnergyToAllyTargetOnCast]", isValid, isValid ? senseiAppendStatus.m_energyToAllyTargetOnCast : 0);
		desc += PropDesc(m_endEffectIfAppendedStatusMod, "[EndEffectIfAppendedStatus]", isValid, isValid && senseiAppendStatus.m_endEffectIfAppendedStatus);
		desc += PropDesc(m_effectAddedOnEnemyAttackMod, "[EffectAddedOnEnemyAttack]", isValid, isValid ? senseiAppendStatus.m_effectAddedOnEnemyAttack : null);
		desc += PropDesc(m_effectAddedOnAllyAttackMod, "[EffectAddedOnAllyAttack]", isValid, isValid ? senseiAppendStatus.m_effectAddedOnAllyAttack : null);
		return desc + PropDesc(m_energyGainOnAllyAppendHitMod, "[EnergyGainOnAllyAppendHit]", isValid, isValid ? senseiAppendStatus.m_energyGainOnAllyAppendHit : 0);
	}
}
