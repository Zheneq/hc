using System;
using System.Collections.Generic;
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
		if (!(tricksterCones != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_ConeInfo(tokens, m_coneInfoMod, "ConeInfo", tricksterCones.m_coneInfo);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, tricksterCones.m_damageAmount);
			AbilityMod.AddToken(tokens, m_subsequentDamageAmountMod, "SubsequentDamageAmount", string.Empty, tricksterCones.m_subsequentDamageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", tricksterCones.m_enemyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyMultipleHitEffectMod, "EnemyMultipleHitEffect", tricksterCones.m_enemyMultipleHitEffect);
			AbilityMod.AddToken(tokens, m_allyHealAmountMod, "AllyHealAmount", string.Empty, tricksterCones.m_allyHealAmount);
			AbilityMod.AddToken(tokens, m_allySubsequentHealAmountMod, "AllySubsequentHealAmount", string.Empty, tricksterCones.m_allySubsequentHealAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", tricksterCones.m_allyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_allyMultipleHitEffectMod, "AllyMultipleHitEffect", tricksterCones.m_allyMultipleHitEffect);
			AbilityMod.AddToken(tokens, m_selfHealAmountMod, "SelfHealAmount", string.Empty, tricksterCones.m_selfHealAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_selfHitEffectMod, "SelfHitEffect", tricksterCones.m_selfHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_selfEffectForMultiHitMod, "SelfEffectForMultiHit", tricksterCones.m_selfEffectForMultiHit);
			AbilityMod.AddToken(tokens, m_cooldownReductionPerHitByCloneMod, "CooldownReductionPerHitByClone", string.Empty, tricksterCones.m_cooldownReductionPerHitByClone);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TricksterCones tricksterCones = GetTargetAbilityOnAbilityData(abilityData) as TricksterCones;
		bool flag = tricksterCones != null;
		string empty = string.Empty;
		empty += PropDesc(m_coneInfoMod, "[ConeInfo]", flag, (!flag) ? null : tricksterCones.m_coneInfo);
		string str = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal;
		if (flag)
		{
			baseVal = tricksterCones.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt subsequentDamageAmountMod = m_subsequentDamageAmountMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = tricksterCones.m_subsequentDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(subsequentDamageAmountMod, "[SubsequentDamageAmount]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal3;
		if (flag)
		{
			baseVal3 = tricksterCones.m_enemyHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str3 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal3);
		empty += PropDesc(m_useEnemyMultiHitEffectMod, "[UseEnemyMultiHitEffect]", flag, flag && tricksterCones.m_useEnemyMultiHitEffect);
		empty += PropDesc(m_enemyMultipleHitEffectMod, "[EnemyMultipleHitEffect]", flag, (!flag) ? null : tricksterCones.m_enemyMultipleHitEffect);
		string str4 = empty;
		AbilityModPropertyInt allyHealAmountMod = m_allyHealAmountMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = tricksterCones.m_allyHealAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(allyHealAmountMod, "[AllyHealAmount]", flag, baseVal4);
		empty += PropDesc(m_allySubsequentHealAmountMod, "[AllySubsequentHealAmount]", flag, flag ? tricksterCones.m_allySubsequentHealAmount : 0);
		empty += PropDesc(m_allyHitEffectMod, "[AllyHitEffect]", flag, (!flag) ? null : tricksterCones.m_allyHitEffect);
		string str5 = empty;
		AbilityModPropertyBool useAllyMultiHitEffectMod = m_useAllyMultiHitEffectMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = (tricksterCones.m_useAllyMultiHitEffect ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(useAllyMultiHitEffectMod, "[UseAllyMultiHitEffect]", flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertyEffectInfo allyMultipleHitEffectMod = m_allyMultipleHitEffectMod;
		object baseVal6;
		if (flag)
		{
			baseVal6 = tricksterCones.m_allyMultipleHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(allyMultipleHitEffectMod, "[AllyMultipleHitEffect]", flag, (StandardEffectInfo)baseVal6);
		string str7 = empty;
		AbilityModPropertyInt selfHealAmountMod = m_selfHealAmountMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = tricksterCones.m_selfHealAmount;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(selfHealAmountMod, "[SelfHealAmount]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyEffectInfo selfHitEffectMod = m_selfHitEffectMod;
		object baseVal8;
		if (flag)
		{
			baseVal8 = tricksterCones.m_selfHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		empty = str8 + PropDesc(selfHitEffectMod, "[SelfHitEffect]", flag, (StandardEffectInfo)baseVal8);
		empty += PropDesc(m_selfEffectForMultiHitMod, "[SelfEffectForMultiHit]", flag, (!flag) ? null : tricksterCones.m_selfEffectForMultiHit);
		string str9 = empty;
		AbilityModPropertyInt cooldownReductionPerHitByCloneMod = m_cooldownReductionPerHitByCloneMod;
		int baseVal9;
		if (flag)
		{
			baseVal9 = tricksterCones.m_cooldownReductionPerHitByClone;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(cooldownReductionPerHitByCloneMod, "[CooldownReductionPerHitByClone]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyBool spawnSpoilForEnemyHitMod = m_spawnSpoilForEnemyHitMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = (tricksterCones.m_spawnSpoilForEnemyHit ? 1 : 0);
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(spawnSpoilForEnemyHitMod, "[SpawnSpoilForEnemyHit]", flag, (byte)baseVal10 != 0);
		empty += PropDesc(m_spawnSpoilForAllyHitMod, "[SpawnSpoilForAllyHit]", flag, flag && tricksterCones.m_spawnSpoilForAllyHit);
		string str11 = empty;
		AbilityModPropertySpoilsSpawnData spoilSpawnInfoMod = m_spoilSpawnInfoMod;
		object baseVal11;
		if (flag)
		{
			baseVal11 = tricksterCones.m_spoilSpawnInfo;
		}
		else
		{
			baseVal11 = null;
		}
		empty = str11 + PropDesc(spoilSpawnInfoMod, "[SpoilSpawnInfo]", flag, (SpoilsSpawnData)baseVal11);
		string str12 = empty;
		AbilityModPropertyBool onlySpawnSpoilOnMultiHitMod = m_onlySpawnSpoilOnMultiHitMod;
		int baseVal12;
		if (flag)
		{
			baseVal12 = (tricksterCones.m_onlySpawnSpoilOnMultiHit ? 1 : 0);
		}
		else
		{
			baseVal12 = 0;
		}
		return str12 + PropDesc(onlySpawnSpoilOnMultiHitMod, "[OnlySpawnSpoilOnMultiHit]", flag, (byte)baseVal12 != 0);
	}
}
