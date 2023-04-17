using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ThiefOnTheRun : AbilityMod
{
	[Header("-- Targeter")]
	public AbilityModPropertyFloat m_minDistanceBetweenStepsMod;
	public AbilityModPropertyFloat m_minDistanceBetweenAnyStepsMod;
	public AbilityModPropertyFloat m_maxDistanceBetweenStepsMod;
	[Header("-- Dash Hit Size")]
	public AbilityModPropertyFloat m_dashRadiusMod;
	public AbilityModPropertyBool m_dashPenetrateLineOfSightMod;
	[Header("-- Hit Damage and Effect")]
	public AbilityModPropertyInt m_damageAmountMod;
	public AbilityModPropertyInt m_subsequentDamageMod;
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;
	[Header("-- Hid On Self")]
	public AbilityModPropertyEffectInfo m_effectOnSelfThroughSmokeFieldMod;
	public AbilityModPropertyInt m_cooldownReductionIfNoEnemyMod;
	[Header("-- Spoil Powerup Spawn")]
	public AbilityModPropertySpoilsSpawnData m_spoilSpawnInfoMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ThiefOnTheRun);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ThiefOnTheRun thiefOnTheRun = targetAbility as ThiefOnTheRun;
		if (thiefOnTheRun != null)
		{
			AddToken(tokens, m_minDistanceBetweenStepsMod, "MinDistanceBetweenSteps", string.Empty, thiefOnTheRun.m_minDistanceBetweenSteps);
			AddToken(tokens, m_minDistanceBetweenAnyStepsMod, "MinDistanceBetweenAnySteps", string.Empty, thiefOnTheRun.m_minDistanceBetweenAnySteps);
			AddToken(tokens, m_maxDistanceBetweenStepsMod, "MaxDistanceBetweenSteps", string.Empty, thiefOnTheRun.m_maxDistanceBetweenSteps);
			AddToken(tokens, m_dashRadiusMod, "DashRadius", string.Empty, thiefOnTheRun.m_dashRadius);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, thiefOnTheRun.m_damageAmount);
			AddToken(tokens, m_subsequentDamageMod, "SubsequentDamage", string.Empty, thiefOnTheRun.m_subsequentDamage);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", thiefOnTheRun.m_enemyHitEffect);
			AddToken_EffectMod(tokens, m_effectOnSelfThroughSmokeFieldMod, "EffectOnSelfThroughSmokeField", thiefOnTheRun.m_effectOnSelfThroughSmokeField);
			AddToken(tokens, m_cooldownReductionIfNoEnemyMod, "CooldownReductionIfNoEnemy", string.Empty, thiefOnTheRun.m_cooldownReductionIfNoEnemy);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefOnTheRun thiefOnTheRun = GetTargetAbilityOnAbilityData(abilityData) as ThiefOnTheRun;
		bool isValid = thiefOnTheRun != null;
		string desc = string.Empty;
		desc += PropDesc(m_minDistanceBetweenStepsMod, "[MinDistanceBetweenSteps]", isValid, isValid ? thiefOnTheRun.m_minDistanceBetweenSteps : 0f);
		desc += PropDesc(m_minDistanceBetweenAnyStepsMod, "[MinDistanceBetweenAnySteps]", isValid, isValid ? thiefOnTheRun.m_minDistanceBetweenAnySteps : 0f);
		desc += PropDesc(m_maxDistanceBetweenStepsMod, "[MaxDistanceBetweenSteps]", isValid, isValid ? thiefOnTheRun.m_maxDistanceBetweenSteps : 0f);
		desc += PropDesc(m_dashRadiusMod, "[DashRadius]", isValid, isValid ? thiefOnTheRun.m_dashRadius : 0f);
		desc += PropDesc(m_dashPenetrateLineOfSightMod, "[DashPenetrateLineOfSight]", isValid, isValid && thiefOnTheRun.m_dashPenetrateLineOfSight);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? thiefOnTheRun.m_damageAmount : 0);
		desc += PropDesc(m_subsequentDamageMod, "[SubsequentDamage]", isValid, isValid ? thiefOnTheRun.m_subsequentDamage : 0);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isValid, isValid ? thiefOnTheRun.m_enemyHitEffect : null);
		desc += PropDesc(m_effectOnSelfThroughSmokeFieldMod, "[EffectOnSelfThroughSmokeField]", isValid, isValid ? thiefOnTheRun.m_effectOnSelfThroughSmokeField : null);
		desc += PropDesc(m_cooldownReductionIfNoEnemyMod, "[CooldownReductionIfNoEnemy]", isValid, isValid ? thiefOnTheRun.m_cooldownReductionIfNoEnemy : 0);
		return desc + PropDesc(m_spoilSpawnInfoMod, "[SpoilSpawnInfo]", isValid, isValid ? thiefOnTheRun.m_spoilSpawnInfo : null);
	}
}
