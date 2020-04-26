using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClericRangedHeal : AbilityMod
{
	[Separator("On Hit Heal/Effect", true)]
	public AbilityModPropertyInt m_healAmountMod;

	public AbilityModPropertyInt m_selfHealIfTargetingAllyMod;

	public AbilityModPropertyEffectInfo m_targetHitEffectMod;

	[Separator("Extra Heal Based on Enemy Hits", true)]
	public AbilityModPropertyInt m_extraHealOnEnemyHitMod;

	public AbilityModPropertyInt m_extraHealOnSubseqEnemyHitMod;

	[Separator("Extra Heal Based on Distance", true)]
	public AbilityModPropertyInt m_extraHealPerTargetDistanceMod;

	[Header("For mod to adjust self healing when only targeting self")]
	public AbilityModPropertyInt m_selfHealAdjustIfTargetingSelfMod;

	[Separator("Extra Heal Based on Current Health", true)]
	public AbilityModPropertyFloat m_healPerPercentHealthLostMod;

	[Separator("On Self", true)]
	public AbilityModPropertyEffectInfo m_effectOnSelfMod;

	[Separator("Effect in Radius", true)]
	public AbilityModPropertyFloat m_enemyDebuffRadiusAroundTargetMod;

	public AbilityModPropertyFloat m_enemyDebuffRadiusAroundCasterMod;

	public AbilityModPropertyEffectInfo m_enemyDebuffInRadiusEffectMod;

	[Separator("Reactions", true)]
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
		if (!(clericRangedHeal != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_healAmountMod, "HealAmount", string.Empty, clericRangedHeal.m_healAmount);
			AbilityMod.AddToken(tokens, m_selfHealIfTargetingAllyMod, "SelfHealIfTargetingAlly", string.Empty, clericRangedHeal.m_selfHealIfTargetingAlly);
			AbilityMod.AddToken_EffectMod(tokens, m_targetHitEffectMod, "TargetHitEffect", clericRangedHeal.m_targetHitEffect);
			AbilityMod.AddToken(tokens, m_extraHealOnEnemyHitMod, "ExtraHealOnEnemyHit", string.Empty, clericRangedHeal.m_extraHealOnEnemyHit);
			AbilityMod.AddToken(tokens, m_extraHealOnSubseqEnemyHitMod, "ExtraHealOnSubseqEnemyHit", string.Empty, clericRangedHeal.m_extraHealOnSubseqEnemyHit);
			AbilityMod.AddToken(tokens, m_extraHealPerTargetDistanceMod, "ExtraHealPerTargetDistance", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_selfHealAdjustIfTargetingSelfMod, "SelfHealAdjustIfTargetingSelf", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_healPerPercentHealthLostMod, "HealPerPercentHealthLost", string.Empty, clericRangedHeal.m_healPerPercentHealthLost);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnSelfMod, "EffectOnSelf", clericRangedHeal.m_effectOnSelf);
			AbilityMod.AddToken_EffectMod(tokens, m_reactionEffectForHealTargetMod, "ReactionEffectForHealTarget", clericRangedHeal.m_reactionEffectForHealTarget);
			AbilityMod.AddToken_EffectMod(tokens, m_reactionEffectForCasterMod, "ReactionEffectForCaster", clericRangedHeal.m_reactionEffectForCaster);
			AbilityMod.AddToken(tokens, m_techPointGainPerIncomingHitThisTurn, "EnergyPerIncomingHitThisTurn", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_enemyDebuffRadiusAroundTargetMod, "EnemyDebuffRadiusAroundTarget", string.Empty, clericRangedHeal.m_enemyDebuffRadiusAroundTarget);
			AbilityMod.AddToken(tokens, m_enemyDebuffRadiusAroundCasterMod, "EnemyDebuffRadiusAroundCaster", string.Empty, clericRangedHeal.m_enemyDebuffRadiusAroundCaster);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyDebuffInRadiusEffectMod, "EnemyDebuffInRadiusEffect", clericRangedHeal.m_enemyDebuffInRadiusEffect);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericRangedHeal clericRangedHeal = GetTargetAbilityOnAbilityData(abilityData) as ClericRangedHeal;
		bool flag = clericRangedHeal != null;
		string empty = string.Empty;
		empty += PropDesc(m_healAmountMod, "[HealAmount]", flag, flag ? clericRangedHeal.m_healAmount : 0);
		empty += PropDesc(m_selfHealIfTargetingAllyMod, "[SelfHealIfTargetingAlly]", flag, flag ? clericRangedHeal.m_selfHealIfTargetingAlly : 0);
		empty += PropDesc(m_targetHitEffectMod, "[TargetHitEffect]", flag, (!flag) ? null : clericRangedHeal.m_targetHitEffect);
		string str = empty;
		AbilityModPropertyInt extraHealOnEnemyHitMod = m_extraHealOnEnemyHitMod;
		int baseVal;
		if (flag)
		{
			baseVal = clericRangedHeal.m_extraHealOnEnemyHit;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(extraHealOnEnemyHitMod, "[ExtraHealOnEnemyHit]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt extraHealOnSubseqEnemyHitMod = m_extraHealOnSubseqEnemyHitMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = clericRangedHeal.m_extraHealOnSubseqEnemyHit;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(extraHealOnSubseqEnemyHitMod, "[ExtraHealOnSubseqEnemyHit]", flag, baseVal2);
		empty += PropDesc(m_extraHealPerTargetDistanceMod, "[ExtraHealPerTargetDistance]", flag);
		empty += PropDesc(m_selfHealAdjustIfTargetingSelfMod, "[SelfHealAdjustIfTargetingSelf]", flag);
		string str3 = empty;
		AbilityModPropertyFloat healPerPercentHealthLostMod = m_healPerPercentHealthLostMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = clericRangedHeal.m_healPerPercentHealthLost;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(healPerPercentHealthLostMod, "[HealPerPercentHealthLost]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyEffectInfo effectOnSelfMod = m_effectOnSelfMod;
		object baseVal4;
		if (flag)
		{
			baseVal4 = clericRangedHeal.m_effectOnSelf;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + PropDesc(effectOnSelfMod, "[EffectOnSelf]", flag, (StandardEffectInfo)baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectInfo reactionEffectForHealTargetMod = m_reactionEffectForHealTargetMod;
		object baseVal5;
		if (flag)
		{
			baseVal5 = clericRangedHeal.m_reactionEffectForHealTarget;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(reactionEffectForHealTargetMod, "[ReactionEffectForHealTarget]", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyEffectInfo reactionEffectForCasterMod = m_reactionEffectForCasterMod;
		object baseVal6;
		if (flag)
		{
			baseVal6 = clericRangedHeal.m_reactionEffectForCaster;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(reactionEffectForCasterMod, "[ReactionEffectForCaster]", flag, (StandardEffectInfo)baseVal6);
		empty += PropDesc(m_techPointGainPerIncomingHitThisTurn, "[EnergyPerIncomingHitThisTurn]", flag);
		empty += PropDesc(m_enemyDebuffRadiusAroundTargetMod, "[EnemyDebuffRadiusAroundTarget]", flag, (!flag) ? 0f : clericRangedHeal.m_enemyDebuffRadiusAroundTarget);
		empty += PropDesc(m_enemyDebuffRadiusAroundCasterMod, "[EnemyDebuffRadiusAroundCaster]", flag, (!flag) ? 0f : clericRangedHeal.m_enemyDebuffRadiusAroundCaster);
		string str7 = empty;
		AbilityModPropertyEffectInfo enemyDebuffInRadiusEffectMod = m_enemyDebuffInRadiusEffectMod;
		object baseVal7;
		if (flag)
		{
			baseVal7 = clericRangedHeal.m_enemyDebuffInRadiusEffect;
		}
		else
		{
			baseVal7 = null;
		}
		return str7 + PropDesc(enemyDebuffInRadiusEffectMod, "[EnemyDebuffInRadiusEffect]", flag, (StandardEffectInfo)baseVal7);
	}
}
