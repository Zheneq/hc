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
		if (tricksterCones != null)
		{
			AbilityMod.AddToken_ConeInfo(tokens, this.m_coneInfoMod, "ConeInfo", tricksterCones.m_coneInfo, true);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, tricksterCones.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_subsequentDamageAmountMod, "SubsequentDamageAmount", string.Empty, tricksterCones.m_subsequentDamageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", tricksterCones.m_enemyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyMultipleHitEffectMod, "EnemyMultipleHitEffect", tricksterCones.m_enemyMultipleHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_allyHealAmountMod, "AllyHealAmount", string.Empty, tricksterCones.m_allyHealAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_allySubsequentHealAmountMod, "AllySubsequentHealAmount", string.Empty, tricksterCones.m_allySubsequentHealAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyHitEffectMod, "AllyHitEffect", tricksterCones.m_allyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyMultipleHitEffectMod, "AllyMultipleHitEffect", tricksterCones.m_allyMultipleHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_selfHealAmountMod, "SelfHealAmount", string.Empty, tricksterCones.m_selfHealAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfHitEffectMod, "SelfHitEffect", tricksterCones.m_selfHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfEffectForMultiHitMod, "SelfEffectForMultiHit", tricksterCones.m_selfEffectForMultiHit, true);
			AbilityMod.AddToken(tokens, this.m_cooldownReductionPerHitByCloneMod, "CooldownReductionPerHitByClone", string.Empty, tricksterCones.m_cooldownReductionPerHitByClone, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TricksterCones tricksterCones = base.GetTargetAbilityOnAbilityData(abilityData) as TricksterCones;
		bool flag = tricksterCones != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_coneInfoMod, "[ConeInfo]", flag, (!flag) ? null : tricksterCones.m_coneInfo);
		string str = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix = "[DamageAmount]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = tricksterCones.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(damageAmountMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt subsequentDamageAmountMod = this.m_subsequentDamageAmountMod;
		string prefix2 = "[SubsequentDamageAmount]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = tricksterCones.m_subsequentDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(subsequentDamageAmountMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix3 = "[EnemyHitEffect]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
		if (flag)
		{
			baseVal3 = tricksterCones.m_enemyHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		text = str3 + base.PropDesc(enemyHitEffectMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_useEnemyMultiHitEffectMod, "[UseEnemyMultiHitEffect]", flag, flag && tricksterCones.m_useEnemyMultiHitEffect);
		text += base.PropDesc(this.m_enemyMultipleHitEffectMod, "[EnemyMultipleHitEffect]", flag, (!flag) ? null : tricksterCones.m_enemyMultipleHitEffect);
		string str4 = text;
		AbilityModPropertyInt allyHealAmountMod = this.m_allyHealAmountMod;
		string prefix4 = "[AllyHealAmount]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = tricksterCones.m_allyHealAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(allyHealAmountMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_allySubsequentHealAmountMod, "[AllySubsequentHealAmount]", flag, (!flag) ? 0 : tricksterCones.m_allySubsequentHealAmount);
		text += base.PropDesc(this.m_allyHitEffectMod, "[AllyHitEffect]", flag, (!flag) ? null : tricksterCones.m_allyHitEffect);
		string str5 = text;
		AbilityModPropertyBool useAllyMultiHitEffectMod = this.m_useAllyMultiHitEffectMod;
		string prefix5 = "[UseAllyMultiHitEffect]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = tricksterCones.m_useAllyMultiHitEffect;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(useAllyMultiHitEffectMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo allyMultipleHitEffectMod = this.m_allyMultipleHitEffectMod;
		string prefix6 = "[AllyMultipleHitEffect]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
		if (flag)
		{
			baseVal6 = tricksterCones.m_allyMultipleHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(allyMultipleHitEffectMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt selfHealAmountMod = this.m_selfHealAmountMod;
		string prefix7 = "[SelfHealAmount]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = tricksterCones.m_selfHealAmount;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(selfHealAmountMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyEffectInfo selfHitEffectMod = this.m_selfHitEffectMod;
		string prefix8 = "[SelfHitEffect]";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
		if (flag)
		{
			baseVal8 = tricksterCones.m_selfHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		text = str8 + base.PropDesc(selfHitEffectMod, prefix8, showBaseVal8, baseVal8);
		text += base.PropDesc(this.m_selfEffectForMultiHitMod, "[SelfEffectForMultiHit]", flag, (!flag) ? null : tricksterCones.m_selfEffectForMultiHit);
		string str9 = text;
		AbilityModPropertyInt cooldownReductionPerHitByCloneMod = this.m_cooldownReductionPerHitByCloneMod;
		string prefix9 = "[CooldownReductionPerHitByClone]";
		bool showBaseVal9 = flag;
		int baseVal9;
		if (flag)
		{
			baseVal9 = tricksterCones.m_cooldownReductionPerHitByClone;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(cooldownReductionPerHitByCloneMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyBool spawnSpoilForEnemyHitMod = this.m_spawnSpoilForEnemyHitMod;
		string prefix10 = "[SpawnSpoilForEnemyHit]";
		bool showBaseVal10 = flag;
		bool baseVal10;
		if (flag)
		{
			baseVal10 = tricksterCones.m_spawnSpoilForEnemyHit;
		}
		else
		{
			baseVal10 = false;
		}
		text = str10 + base.PropDesc(spawnSpoilForEnemyHitMod, prefix10, showBaseVal10, baseVal10);
		text += base.PropDesc(this.m_spawnSpoilForAllyHitMod, "[SpawnSpoilForAllyHit]", flag, flag && tricksterCones.m_spawnSpoilForAllyHit);
		string str11 = text;
		AbilityModPropertySpoilsSpawnData spoilSpawnInfoMod = this.m_spoilSpawnInfoMod;
		string prefix11 = "[SpoilSpawnInfo]";
		bool showBaseVal11 = flag;
		SpoilsSpawnData baseVal11;
		if (flag)
		{
			baseVal11 = tricksterCones.m_spoilSpawnInfo;
		}
		else
		{
			baseVal11 = null;
		}
		text = str11 + base.PropDesc(spoilSpawnInfoMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyBool onlySpawnSpoilOnMultiHitMod = this.m_onlySpawnSpoilOnMultiHitMod;
		string prefix12 = "[OnlySpawnSpoilOnMultiHit]";
		bool showBaseVal12 = flag;
		bool baseVal12;
		if (flag)
		{
			baseVal12 = tricksterCones.m_onlySpawnSpoilOnMultiHit;
		}
		else
		{
			baseVal12 = false;
		}
		return str12 + base.PropDesc(onlySpawnSpoilOnMultiHitMod, prefix12, showBaseVal12, baseVal12);
	}
}
