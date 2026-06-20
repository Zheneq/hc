// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TricksterCatchMeIfYouCan : AbilityMod
{
	[Header("-- Hit actors in path")]
	public AbilityModPropertyBool m_hitActorsInPathMod;
	public AbilityModPropertyFloat m_pathRadiusMod;
	public AbilityModPropertyFloat m_pathStartRadiusMod;
	public AbilityModPropertyFloat m_pathEndRadiusMod;
	public AbilityModPropertyBool m_penetrateLosMod;
	[Header("-- Enemy Hit")]
	public AbilityModPropertyInt m_damageAmountMod;
	public AbilityModPropertyInt m_subsequentDamageAmountMod;
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;
	[Space(10f)]
	public AbilityModPropertyBool m_useEnemyMultiHitEffectMod;
	public AbilityModPropertyEffectInfo m_enemyMultipleHitEffectMod;
	[Header("-- Ally Hit")]
	public AbilityModPropertyInt m_allyHealingAmountMod;
	public AbilityModPropertyInt m_subsequentHealingAmountMod;
	public AbilityModPropertyInt m_allyEnergyGainMod;
	public AbilityModPropertyEffectInfo m_allyHitEffectMod;
	[Space(10f)]
	public AbilityModPropertyBool m_useAllyMultiHitEffectMod;
	public AbilityModPropertyEffectInfo m_allyMultipleHitEffectMod;
	[Space(10f)]
	public AbilityModPropertyInt m_selfHealingAmountMod;
	public AbilityModPropertyEffectInfo m_selfHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(TricksterCatchMeIfYouCan);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TricksterCatchMeIfYouCan tricksterCatchMeIfYouCan = targetAbility as TricksterCatchMeIfYouCan;
		if (tricksterCatchMeIfYouCan != null)
		{
			AddToken(tokens, m_pathRadiusMod, "PathRadius", string.Empty, tricksterCatchMeIfYouCan.m_pathRadius);
			AddToken(tokens, m_pathStartRadiusMod, "PathStartRadius", string.Empty, tricksterCatchMeIfYouCan.m_pathStartRadius);
			AddToken(tokens, m_pathEndRadiusMod, "PathEndRadius", string.Empty, tricksterCatchMeIfYouCan.m_pathEndRadius);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, tricksterCatchMeIfYouCan.m_damageAmount);
			AddToken(tokens, m_subsequentDamageAmountMod, "SubsequentDamageAmount", string.Empty, tricksterCatchMeIfYouCan.m_subsequentDamageAmount);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", tricksterCatchMeIfYouCan.m_enemyHitEffect);
			AddToken_EffectMod(tokens, m_enemyMultipleHitEffectMod, "EnemyMultipleHitEffect", tricksterCatchMeIfYouCan.m_enemyMultipleHitEffect);
			AddToken(tokens, m_allyHealingAmountMod, "AllyHealingAmount", string.Empty, tricksterCatchMeIfYouCan.m_allyHealingAmount);
			AddToken(tokens, m_subsequentHealingAmountMod, "SubsequentHealingAmount", string.Empty, tricksterCatchMeIfYouCan.m_subsequentHealingAmount);
			AddToken(tokens, m_allyEnergyGainMod, "AllyEnergyGain", string.Empty, tricksterCatchMeIfYouCan.m_allyEnergyGain);
			AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", tricksterCatchMeIfYouCan.m_allyHitEffect);
			AddToken_EffectMod(tokens, m_allyMultipleHitEffectMod, "AllyMultipleHitEffect", tricksterCatchMeIfYouCan.m_allyMultipleHitEffect);
			AddToken(tokens, m_selfHealingAmountMod, "SelfHealingAmount", string.Empty, tricksterCatchMeIfYouCan.m_selfHealingAmount);
			AddToken_EffectMod(tokens, m_selfHitEffectMod, "SelfHitEffect", tricksterCatchMeIfYouCan.m_selfHitEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		TricksterCatchMeIfYouCan tricksterCatchMeIfYouCan = GetTargetAbilityOnAbilityData(abilityData) as TricksterCatchMeIfYouCan;
		// rogues
		// TricksterCatchMeIfYouCan tricksterCatchMeIfYouCan = targetAbility as TricksterCatchMeIfYouCan;
		bool isValid = tricksterCatchMeIfYouCan != null;
		string desc = string.Empty;
		desc += PropDesc(m_hitActorsInPathMod, "[HitActorsInPath]", isValid, isValid && tricksterCatchMeIfYouCan.m_hitActorsInPath);
		desc += PropDesc(m_pathRadiusMod, "[PathRadius]", isValid, isValid ? tricksterCatchMeIfYouCan.m_pathRadius : 0f);
		desc += PropDesc(m_pathStartRadiusMod, "[PathStartRadius]", isValid, isValid ? tricksterCatchMeIfYouCan.m_pathStartRadius : 0f);
		desc += PropDesc(m_pathEndRadiusMod, "[PathEndRadius]", isValid, isValid ? tricksterCatchMeIfYouCan.m_pathEndRadius : 0f);
		desc += PropDesc(m_penetrateLosMod, "[PenetrateLos]", isValid, isValid && tricksterCatchMeIfYouCan.m_penetrateLos);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? tricksterCatchMeIfYouCan.m_damageAmount : 0);
		desc += PropDesc(m_subsequentDamageAmountMod, "[SubsequentDamageAmount]", isValid, isValid ? tricksterCatchMeIfYouCan.m_subsequentDamageAmount : 0);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isValid, isValid ? tricksterCatchMeIfYouCan.m_enemyHitEffect : null);
		desc += PropDesc(m_useEnemyMultiHitEffectMod, "[UseEnemyMultiHitEffect]", isValid, isValid && tricksterCatchMeIfYouCan.m_useEnemyMultiHitEffect);
		desc += PropDesc(m_enemyMultipleHitEffectMod, "[EnemyMultipleHitEffect]", isValid, isValid ? tricksterCatchMeIfYouCan.m_enemyMultipleHitEffect : null);
		desc += PropDesc(m_allyHealingAmountMod, "[AllyHealingAmount]", isValid, isValid ? tricksterCatchMeIfYouCan.m_allyHealingAmount : 0);
		desc += PropDesc(m_allyEnergyGainMod, "[AllyEnergyGain]", isValid, isValid ? tricksterCatchMeIfYouCan.m_allyEnergyGain : 0);
		desc += PropDesc(m_subsequentHealingAmountMod, "[SubsequentHealingAmount]", isValid, isValid ? tricksterCatchMeIfYouCan.m_subsequentHealingAmount : 0);
		desc += PropDesc(m_allyHitEffectMod, "[AllyHitEffect]", isValid, isValid ? tricksterCatchMeIfYouCan.m_allyHitEffect : null);
		desc += PropDesc(m_useAllyMultiHitEffectMod, "[UseAllyMultiHitEffect]", isValid, isValid && tricksterCatchMeIfYouCan.m_useAllyMultiHitEffect);
		desc += PropDesc(m_allyMultipleHitEffectMod, "[AllyMultipleHitEffect]", isValid, isValid ? tricksterCatchMeIfYouCan.m_allyMultipleHitEffect : null);
		desc += PropDesc(m_selfHealingAmountMod, "[SelfHealingAmount]", isValid, isValid ? tricksterCatchMeIfYouCan.m_selfHealingAmount : 0);
		return desc + PropDesc(m_selfHitEffectMod, "[SelfHitEffect]", isValid, isValid ? tricksterCatchMeIfYouCan.m_selfHitEffect : null);
	}
}
