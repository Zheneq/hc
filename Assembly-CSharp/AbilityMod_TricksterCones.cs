using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_TricksterCones : AbilityMod
{
	[Header("-- Cone Targeting")]
	public AbilityModPropertyConeInfo m_coneInfoMod;
	[Header("-- Enemy Hit Damage and Effects")]
	public AbilityModPropertyInt m_damageAmountMod;
	public AbilityModPropertyInt m_subsequentDamageAmountMod;
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;
	[Space(10f)]
	public AbilityModPropertyBool m_useEnemyMultiHitEffectMod;
	public AbilityModPropertyEffectInfo m_enemyMultipleHitEffectMod;
	[Header("-- Ally Hit Heal and Effects")]
	public AbilityModPropertyInt m_allyHealAmountMod;
	public AbilityModPropertyInt m_allySubsequentHealAmountMod;
	public AbilityModPropertyEffectInfo m_allyHitEffectMod;
	[Space(10f)]
	public AbilityModPropertyBool m_useAllyMultiHitEffectMod;
	public AbilityModPropertyEffectInfo m_allyMultipleHitEffectMod;
	[Header("-- Self Hit Heal and Effects")]
	public AbilityModPropertyInt m_selfHealAmountMod;
	public AbilityModPropertyEffectInfo m_selfHitEffectMod;
	public AbilityModPropertyEffectInfo m_selfEffectForMultiHitMod;
	[Header("-- Cooldown Reduction Per Enemy Hit By Clone --")]
	public AbilityModPropertyInt m_cooldownReductionPerHitByCloneMod;
	[Header("-- For spawning spoils")]
	public AbilityModPropertyBool m_spawnSpoilForEnemyHitMod;
	public AbilityModPropertyBool m_spawnSpoilForAllyHitMod;
	public AbilityModPropertyBool m_onlySpawnSpoilOnMultiHitMod;
	public AbilityModPropertySpoilsSpawnData m_spoilSpawnInfoMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(TricksterCones);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TricksterCones tricksterCones = targetAbility as TricksterCones;
		if (tricksterCones != null)
		{
			AddToken_ConeInfo(tokens, m_coneInfoMod, "ConeInfo", tricksterCones.m_coneInfo);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, tricksterCones.m_damageAmount);
			AddToken(tokens, m_subsequentDamageAmountMod, "SubsequentDamageAmount", string.Empty, tricksterCones.m_subsequentDamageAmount);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", tricksterCones.m_enemyHitEffect);
			AddToken_EffectMod(tokens, m_enemyMultipleHitEffectMod, "EnemyMultipleHitEffect", tricksterCones.m_enemyMultipleHitEffect);
			AddToken(tokens, m_allyHealAmountMod, "AllyHealAmount", string.Empty, tricksterCones.m_allyHealAmount);
			AddToken(tokens, m_allySubsequentHealAmountMod, "AllySubsequentHealAmount", string.Empty, tricksterCones.m_allySubsequentHealAmount);
			AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", tricksterCones.m_allyHitEffect);
			AddToken_EffectMod(tokens, m_allyMultipleHitEffectMod, "AllyMultipleHitEffect", tricksterCones.m_allyMultipleHitEffect);
			AddToken(tokens, m_selfHealAmountMod, "SelfHealAmount", string.Empty, tricksterCones.m_selfHealAmount);
			AddToken_EffectMod(tokens, m_selfHitEffectMod, "SelfHitEffect", tricksterCones.m_selfHitEffect);
			AddToken_EffectMod(tokens, m_selfEffectForMultiHitMod, "SelfEffectForMultiHit", tricksterCones.m_selfEffectForMultiHit);
			AddToken(tokens, m_cooldownReductionPerHitByCloneMod, "CooldownReductionPerHitByClone", string.Empty, tricksterCones.m_cooldownReductionPerHitByClone);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TricksterCones tricksterCones = GetTargetAbilityOnAbilityData(abilityData) as TricksterCones;
		bool isValid = tricksterCones != null;
		string desc = string.Empty;
		desc += PropDesc(m_coneInfoMod, "[ConeInfo]", isValid, isValid ? tricksterCones.m_coneInfo : null);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? tricksterCones.m_damageAmount : 0);
		desc += PropDesc(m_subsequentDamageAmountMod, "[SubsequentDamageAmount]", isValid, isValid ? tricksterCones.m_subsequentDamageAmount : 0);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isValid, isValid ? tricksterCones.m_enemyHitEffect : null);
		desc += PropDesc(m_useEnemyMultiHitEffectMod, "[UseEnemyMultiHitEffect]", isValid, isValid && tricksterCones.m_useEnemyMultiHitEffect);
		desc += PropDesc(m_enemyMultipleHitEffectMod, "[EnemyMultipleHitEffect]", isValid, isValid ? tricksterCones.m_enemyMultipleHitEffect : null);
		desc += PropDesc(m_allyHealAmountMod, "[AllyHealAmount]", isValid, isValid ? tricksterCones.m_allyHealAmount : 0);
		desc += PropDesc(m_allySubsequentHealAmountMod, "[AllySubsequentHealAmount]", isValid, isValid ? tricksterCones.m_allySubsequentHealAmount : 0);
		desc += PropDesc(m_allyHitEffectMod, "[AllyHitEffect]", isValid, isValid ? tricksterCones.m_allyHitEffect : null);
		desc += PropDesc(m_useAllyMultiHitEffectMod, "[UseAllyMultiHitEffect]", isValid, isValid && tricksterCones.m_useAllyMultiHitEffect);
		desc += PropDesc(m_allyMultipleHitEffectMod, "[AllyMultipleHitEffect]", isValid, isValid ? tricksterCones.m_allyMultipleHitEffect : null);
		desc += PropDesc(m_selfHealAmountMod, "[SelfHealAmount]", isValid, isValid ? tricksterCones.m_selfHealAmount : 0);
		desc += PropDesc(m_selfHitEffectMod, "[SelfHitEffect]", isValid, isValid ? tricksterCones.m_selfHitEffect : null);
		desc += PropDesc(m_selfEffectForMultiHitMod, "[SelfEffectForMultiHit]", isValid, isValid ? tricksterCones.m_selfEffectForMultiHit : null);
		desc += PropDesc(m_cooldownReductionPerHitByCloneMod, "[CooldownReductionPerHitByClone]", isValid, isValid ? tricksterCones.m_cooldownReductionPerHitByClone : 0);
		desc += PropDesc(m_spawnSpoilForEnemyHitMod, "[SpawnSpoilForEnemyHit]", isValid, isValid && tricksterCones.m_spawnSpoilForEnemyHit);
		desc += PropDesc(m_spawnSpoilForAllyHitMod, "[SpawnSpoilForAllyHit]", isValid, isValid && tricksterCones.m_spawnSpoilForAllyHit);
		desc += PropDesc(m_spoilSpawnInfoMod, "[SpoilSpawnInfo]", isValid, isValid ? tricksterCones.m_spoilSpawnInfo : null);
		return new StringBuilder().Append(desc).Append(PropDesc(m_onlySpawnSpoilOnMultiHitMod, "[OnlySpawnSpoilOnMultiHit]", isValid, isValid && tricksterCones.m_onlySpawnSpoilOnMultiHit)).ToString();
	}
}
