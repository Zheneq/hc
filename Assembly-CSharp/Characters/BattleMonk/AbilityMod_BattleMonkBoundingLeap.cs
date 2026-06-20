// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BattleMonkBoundingLeap : AbilityMod
{
	[Header("-- Bounce Mod")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyInt m_damageAfterFirstHitMod;
	[Header("-- Bounce")]
	public AbilityModPropertyInt m_maxHitTargetsMod;
	public AbilityModPropertyBool m_bounceOffEnemyActorMod;
	public AbilityModPropertyFloat m_maxDistancePerBounceMod;
	public AbilityModPropertyFloat m_maxTotalDistanceMod;
	public AbilityModPropertyInt m_maxBouncesMod;
	[Header("-- Whether to include allies in between")]
	public AbilityModPropertyBool m_hitAlliesInBetween;
	public StandardEffectInfo m_allyHitEffect;
	[Header("-- Heal Amount If Not Damaged This Turn")]
	public AbilityModPropertyInt m_healAmountIfNotDamagedThisTurn;

	public override Type GetTargetAbilityType()
	{
		return typeof(BattleMonkBoundingLeap);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BattleMonkBoundingLeap battleMonkBoundingLeap = targetAbility as BattleMonkBoundingLeap;
		if (battleMonkBoundingLeap != null)
		{
			AddToken(tokens, m_maxDistancePerBounceMod, "MaxDistancePerBounce", string.Empty, battleMonkBoundingLeap.m_maxDistancePerBounce);
			AddToken(tokens, m_maxTotalDistanceMod, "MaxTotalDistance", string.Empty, battleMonkBoundingLeap.m_maxTotalDistance);
			AddToken(tokens, m_maxBouncesMod, "MaxBounces", string.Empty, battleMonkBoundingLeap.m_maxBounces);
			AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, battleMonkBoundingLeap.m_damageAmount);
			AddToken(tokens, m_damageAfterFirstHitMod, "DamageAfterFirstHit", string.Empty, battleMonkBoundingLeap.m_damageAfterFirstHit);
			AddToken(tokens, m_maxBouncesMod, "MaxBounces", string.Empty, battleMonkBoundingLeap.m_maxBounces);
			AddToken(tokens, m_maxHitTargetsMod, "MaxTargetsHit", string.Empty, battleMonkBoundingLeap.m_maxTargetsHit);
			AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyHitEffectMod", null, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		BattleMonkBoundingLeap battleMonkBoundingLeap = GetTargetAbilityOnAbilityData(abilityData) as BattleMonkBoundingLeap;
		// rogues
		//BattleMonkBoundingLeap battleMonkBoundingLeap = targetAbility as BattleMonkBoundingLeap;
		
		bool isAbilityPresent = battleMonkBoundingLeap != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? battleMonkBoundingLeap.m_damageAmount : 0);
		desc += PropDesc(m_damageAfterFirstHitMod, "[DamageAfterFirstHit]", isAbilityPresent, isAbilityPresent ? battleMonkBoundingLeap.m_damageAfterFirstHit : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_maxHitTargetsMod, "[Max Hits]", isAbilityPresent, isAbilityPresent ? battleMonkBoundingLeap.m_maxTargetsHit : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_bounceOffEnemyActorMod, "[Bounce Off Enemies?]", isAbilityPresent, isAbilityPresent && battleMonkBoundingLeap.m_bounceOffEnemyActor);
		desc += PropDesc(m_maxDistancePerBounceMod, "[MaxDistancePerBounce]", isAbilityPresent, isAbilityPresent ? battleMonkBoundingLeap.m_maxDistancePerBounce : 0f);
		desc += PropDesc(m_maxTotalDistanceMod, "[MaxTotalDistance]", isAbilityPresent, isAbilityPresent ? battleMonkBoundingLeap.m_maxTotalDistance : 0f);
		desc += PropDesc(m_maxBouncesMod, "[MaxBounces]", isAbilityPresent, isAbilityPresent ? battleMonkBoundingLeap.m_maxBounces : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_hitAlliesInBetween, "[Hit Allies In-Between]", isAbilityPresent);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_allyHitEffect, "{ Ally Hit Effect }", string.Empty, isAbilityPresent);
		return desc + PropDesc(m_healAmountIfNotDamagedThisTurn, "{ Heal Amount If Not Damaged This Turn }", isAbilityPresent);
	}
}
