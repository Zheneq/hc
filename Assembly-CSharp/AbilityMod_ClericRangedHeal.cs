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
		if (clericRangedHeal != null)
		{
			AbilityMod.AddToken(tokens, this.m_healAmountMod, "HealAmount", string.Empty, clericRangedHeal.m_healAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_selfHealIfTargetingAllyMod, "SelfHealIfTargetingAlly", string.Empty, clericRangedHeal.m_selfHealIfTargetingAlly, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_targetHitEffectMod, "TargetHitEffect", clericRangedHeal.m_targetHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraHealOnEnemyHitMod, "ExtraHealOnEnemyHit", string.Empty, clericRangedHeal.m_extraHealOnEnemyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_extraHealOnSubseqEnemyHitMod, "ExtraHealOnSubseqEnemyHit", string.Empty, clericRangedHeal.m_extraHealOnSubseqEnemyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_extraHealPerTargetDistanceMod, "ExtraHealPerTargetDistance", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_selfHealAdjustIfTargetingSelfMod, "SelfHealAdjustIfTargetingSelf", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_healPerPercentHealthLostMod, "HealPerPercentHealthLost", string.Empty, clericRangedHeal.m_healPerPercentHealthLost, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnSelfMod, "EffectOnSelf", clericRangedHeal.m_effectOnSelf, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_reactionEffectForHealTargetMod, "ReactionEffectForHealTarget", clericRangedHeal.m_reactionEffectForHealTarget, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_reactionEffectForCasterMod, "ReactionEffectForCaster", clericRangedHeal.m_reactionEffectForCaster, true);
			AbilityMod.AddToken(tokens, this.m_techPointGainPerIncomingHitThisTurn, "EnergyPerIncomingHitThisTurn", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_enemyDebuffRadiusAroundTargetMod, "EnemyDebuffRadiusAroundTarget", string.Empty, clericRangedHeal.m_enemyDebuffRadiusAroundTarget, true, false, false);
			AbilityMod.AddToken(tokens, this.m_enemyDebuffRadiusAroundCasterMod, "EnemyDebuffRadiusAroundCaster", string.Empty, clericRangedHeal.m_enemyDebuffRadiusAroundCaster, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyDebuffInRadiusEffectMod, "EnemyDebuffInRadiusEffect", clericRangedHeal.m_enemyDebuffInRadiusEffect, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericRangedHeal clericRangedHeal = base.GetTargetAbilityOnAbilityData(abilityData) as ClericRangedHeal;
		bool flag = clericRangedHeal != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_healAmountMod, "[HealAmount]", flag, (!flag) ? 0 : clericRangedHeal.m_healAmount);
		text += base.PropDesc(this.m_selfHealIfTargetingAllyMod, "[SelfHealIfTargetingAlly]", flag, (!flag) ? 0 : clericRangedHeal.m_selfHealIfTargetingAlly);
		text += base.PropDesc(this.m_targetHitEffectMod, "[TargetHitEffect]", flag, (!flag) ? null : clericRangedHeal.m_targetHitEffect);
		string str = text;
		AbilityModPropertyInt extraHealOnEnemyHitMod = this.m_extraHealOnEnemyHitMod;
		string prefix = "[ExtraHealOnEnemyHit]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = clericRangedHeal.m_extraHealOnEnemyHit;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(extraHealOnEnemyHitMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt extraHealOnSubseqEnemyHitMod = this.m_extraHealOnSubseqEnemyHitMod;
		string prefix2 = "[ExtraHealOnSubseqEnemyHit]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = clericRangedHeal.m_extraHealOnSubseqEnemyHit;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(extraHealOnSubseqEnemyHitMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_extraHealPerTargetDistanceMod, "[ExtraHealPerTargetDistance]", flag, 0);
		text += base.PropDesc(this.m_selfHealAdjustIfTargetingSelfMod, "[SelfHealAdjustIfTargetingSelf]", flag, 0);
		string str3 = text;
		AbilityModPropertyFloat healPerPercentHealthLostMod = this.m_healPerPercentHealthLostMod;
		string prefix3 = "[HealPerPercentHealthLost]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = clericRangedHeal.m_healPerPercentHealthLost;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(healPerPercentHealthLostMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyEffectInfo effectOnSelfMod = this.m_effectOnSelfMod;
		string prefix4 = "[EffectOnSelf]";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
		if (flag)
		{
			baseVal4 = clericRangedHeal.m_effectOnSelf;
		}
		else
		{
			baseVal4 = null;
		}
		text = str4 + base.PropDesc(effectOnSelfMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectInfo reactionEffectForHealTargetMod = this.m_reactionEffectForHealTargetMod;
		string prefix5 = "[ReactionEffectForHealTarget]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal5;
		if (flag)
		{
			baseVal5 = clericRangedHeal.m_reactionEffectForHealTarget;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(reactionEffectForHealTargetMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo reactionEffectForCasterMod = this.m_reactionEffectForCasterMod;
		string prefix6 = "[ReactionEffectForCaster]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
		if (flag)
		{
			baseVal6 = clericRangedHeal.m_reactionEffectForCaster;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(reactionEffectForCasterMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_techPointGainPerIncomingHitThisTurn, "[EnergyPerIncomingHitThisTurn]", flag, 0);
		text += base.PropDesc(this.m_enemyDebuffRadiusAroundTargetMod, "[EnemyDebuffRadiusAroundTarget]", flag, (!flag) ? 0f : clericRangedHeal.m_enemyDebuffRadiusAroundTarget);
		text += base.PropDesc(this.m_enemyDebuffRadiusAroundCasterMod, "[EnemyDebuffRadiusAroundCaster]", flag, (!flag) ? 0f : clericRangedHeal.m_enemyDebuffRadiusAroundCaster);
		string str7 = text;
		AbilityModPropertyEffectInfo enemyDebuffInRadiusEffectMod = this.m_enemyDebuffInRadiusEffectMod;
		string prefix7 = "[EnemyDebuffInRadiusEffect]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
		if (flag)
		{
			baseVal7 = clericRangedHeal.m_enemyDebuffInRadiusEffect;
		}
		else
		{
			baseVal7 = null;
		}
		return str7 + base.PropDesc(enemyDebuffInRadiusEffectMod, prefix7, showBaseVal7, baseVal7);
	}
}
