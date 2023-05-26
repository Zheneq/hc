using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClericRangedHeal : AbilityMod
{
	[Separator("On Hit Heal/Effect")]
	public AbilityModPropertyInt m_healAmountMod;
	public AbilityModPropertyInt m_selfHealIfTargetingAllyMod;
	public AbilityModPropertyEffectInfo m_targetHitEffectMod;
	[Separator("Extra Heal Based on Enemy Hits")]
	public AbilityModPropertyInt m_extraHealOnEnemyHitMod;
	public AbilityModPropertyInt m_extraHealOnSubseqEnemyHitMod;
	[Separator("Extra Heal Based on Distance")]
	public AbilityModPropertyInt m_extraHealPerTargetDistanceMod;
	[Header("For mod to adjust self healing when only targeting self")]
	public AbilityModPropertyInt m_selfHealAdjustIfTargetingSelfMod;
	[Separator("Extra Heal Based on Current Health")]
	public AbilityModPropertyFloat m_healPerPercentHealthLostMod;
	[Separator("On Self")]
	public AbilityModPropertyEffectInfo m_effectOnSelfMod;
	[Separator("Effect in Radius")]
	public AbilityModPropertyFloat m_enemyDebuffRadiusAroundTargetMod;
	public AbilityModPropertyFloat m_enemyDebuffRadiusAroundCasterMod;
	public AbilityModPropertyEffectInfo m_enemyDebuffInRadiusEffectMod;
	[Separator("Reactions")]
	public AbilityModPropertyEffectInfo m_reactionEffectForHealTargetMod;
	public AbilityModPropertyEffectInfo m_reactionEffectForCasterMod;
	public AbilityModPropertyInt m_techPointGainPerIncomingHitThisTurn;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClericRangedHeal);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClericRangedHeal clericRangedHeal = targetAbility as ClericRangedHeal;
		if (clericRangedHeal != null)
		{
			AddToken(tokens, m_healAmountMod, "HealAmount", string.Empty, clericRangedHeal.m_healAmount);
			AddToken(tokens, m_selfHealIfTargetingAllyMod, "SelfHealIfTargetingAlly", string.Empty, clericRangedHeal.m_selfHealIfTargetingAlly);
			AddToken_EffectMod(tokens, m_targetHitEffectMod, "TargetHitEffect", clericRangedHeal.m_targetHitEffect);
			AddToken(tokens, m_extraHealOnEnemyHitMod, "ExtraHealOnEnemyHit", string.Empty, clericRangedHeal.m_extraHealOnEnemyHit);
			AddToken(tokens, m_extraHealOnSubseqEnemyHitMod, "ExtraHealOnSubseqEnemyHit", string.Empty, clericRangedHeal.m_extraHealOnSubseqEnemyHit);
			AddToken(tokens, m_extraHealPerTargetDistanceMod, "ExtraHealPerTargetDistance", string.Empty, 0);
			AddToken(tokens, m_selfHealAdjustIfTargetingSelfMod, "SelfHealAdjustIfTargetingSelf", string.Empty, 0);
			AddToken(tokens, m_healPerPercentHealthLostMod, "HealPerPercentHealthLost", string.Empty, clericRangedHeal.m_healPerPercentHealthLost);
			AddToken_EffectMod(tokens, m_effectOnSelfMod, "EffectOnSelf", clericRangedHeal.m_effectOnSelf);
			AddToken_EffectMod(tokens, m_reactionEffectForHealTargetMod, "ReactionEffectForHealTarget", clericRangedHeal.m_reactionEffectForHealTarget);
			AddToken_EffectMod(tokens, m_reactionEffectForCasterMod, "ReactionEffectForCaster", clericRangedHeal.m_reactionEffectForCaster);
			AddToken(tokens, m_techPointGainPerIncomingHitThisTurn, "EnergyPerIncomingHitThisTurn", string.Empty, 0);
			AddToken(tokens, m_enemyDebuffRadiusAroundTargetMod, "EnemyDebuffRadiusAroundTarget", string.Empty, clericRangedHeal.m_enemyDebuffRadiusAroundTarget);
			AddToken(tokens, m_enemyDebuffRadiusAroundCasterMod, "EnemyDebuffRadiusAroundCaster", string.Empty, clericRangedHeal.m_enemyDebuffRadiusAroundCaster);
			AddToken_EffectMod(tokens, m_enemyDebuffInRadiusEffectMod, "EnemyDebuffInRadiusEffect", clericRangedHeal.m_enemyDebuffInRadiusEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericRangedHeal clericRangedHeal = GetTargetAbilityOnAbilityData(abilityData) as ClericRangedHeal;
		bool isValid = clericRangedHeal != null;
		string desc = string.Empty;
		desc += PropDesc(m_healAmountMod, "[HealAmount]", isValid, isValid ? clericRangedHeal.m_healAmount : 0);
		desc += PropDesc(m_selfHealIfTargetingAllyMod, "[SelfHealIfTargetingAlly]", isValid, isValid ? clericRangedHeal.m_selfHealIfTargetingAlly : 0);
		desc += PropDesc(m_targetHitEffectMod, "[TargetHitEffect]", isValid, isValid ? clericRangedHeal.m_targetHitEffect : null);
		desc += PropDesc(m_extraHealOnEnemyHitMod, "[ExtraHealOnEnemyHit]", isValid, isValid ? clericRangedHeal.m_extraHealOnEnemyHit : 0);
		desc += PropDesc(m_extraHealOnSubseqEnemyHitMod, "[ExtraHealOnSubseqEnemyHit]", isValid, isValid ? clericRangedHeal.m_extraHealOnSubseqEnemyHit : 0);
		desc += PropDesc(m_extraHealPerTargetDistanceMod, "[ExtraHealPerTargetDistance]", isValid);
		desc += PropDesc(m_selfHealAdjustIfTargetingSelfMod, "[SelfHealAdjustIfTargetingSelf]", isValid);
		desc += PropDesc(m_healPerPercentHealthLostMod, "[HealPerPercentHealthLost]", isValid, isValid ? clericRangedHeal.m_healPerPercentHealthLost : 0f);
		desc += PropDesc(m_effectOnSelfMod, "[EffectOnSelf]", isValid, isValid ? clericRangedHeal.m_effectOnSelf : null);
		desc += PropDesc(m_reactionEffectForHealTargetMod, "[ReactionEffectForHealTarget]", isValid, isValid ? clericRangedHeal.m_reactionEffectForHealTarget : null);
		desc += PropDesc(m_reactionEffectForCasterMod, "[ReactionEffectForCaster]", isValid, isValid ? clericRangedHeal.m_reactionEffectForCaster : null);
		desc += PropDesc(m_techPointGainPerIncomingHitThisTurn, "[EnergyPerIncomingHitThisTurn]", isValid);
		desc += PropDesc(m_enemyDebuffRadiusAroundTargetMod, "[EnemyDebuffRadiusAroundTarget]", isValid, isValid ? clericRangedHeal.m_enemyDebuffRadiusAroundTarget : 0f);
		desc += PropDesc(m_enemyDebuffRadiusAroundCasterMod, "[EnemyDebuffRadiusAroundCaster]", isValid, isValid ? clericRangedHeal.m_enemyDebuffRadiusAroundCaster : 0f);
		return desc + PropDesc(m_enemyDebuffInRadiusEffectMod, "[EnemyDebuffInRadiusEffect]", isValid, isValid ? clericRangedHeal.m_enemyDebuffInRadiusEffect : null);
	}
}
