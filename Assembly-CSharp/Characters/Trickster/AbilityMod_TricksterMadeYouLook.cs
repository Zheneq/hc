// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TricksterMadeYouLook : AbilityMod
{
	[Header("-- Target Actors In-Between")]
	public AbilityModPropertyBool m_hitActorsInBetweenMod;
	public AbilityModPropertyFloat m_radiusFromLineMod;
	public AbilityModPropertyFloat m_radiusAroundEndsMod;
	public AbilityModPropertyBool m_penetrateLosMod;
	[Header("-- Cooldown Reduction for Hitting Targets In Between --")]
	public AbilityModCooldownReduction m_cooldownReductionForTravelHit;
	[Header("-- Enemy Hit Damage and Effect")]
	public AbilityModPropertyInt m_damageAmountMod;
	public AbilityModPropertyEffectInfo m_enemyOnHitEffectMod;
	[Header("-- Spoils to spawn on clone disappear --")]
	public AbilityModPropertySpoilsSpawnData m_spoilsSpawnDataOnDisappear;

	public override Type GetTargetAbilityType()
	{
		return typeof(TricksterMadeYouLook);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TricksterMadeYouLook tricksterMadeYouLook = targetAbility as TricksterMadeYouLook;
		if (tricksterMadeYouLook != null)
		{
			AddToken(tokens, m_radiusFromLineMod, "RadiusFromLine", string.Empty, tricksterMadeYouLook.m_radiusFromLine);
			AddToken(tokens, m_radiusAroundEndsMod, "RadiusAroundEnds", string.Empty, tricksterMadeYouLook.m_radiusAroundEnds);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, tricksterMadeYouLook.m_damageAmount);
			AddToken_EffectMod(tokens, m_enemyOnHitEffectMod, "EnemyOnHitEffect", tricksterMadeYouLook.m_enemyOnHitEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		TricksterMadeYouLook tricksterMadeYouLook = GetTargetAbilityOnAbilityData(abilityData) as TricksterMadeYouLook;
		// rogues
		// TricksterMadeYouLook tricksterMadeYouLook = targetAbility as TricksterMadeYouLook;
		bool isValid = tricksterMadeYouLook != null;
		string desc = string.Empty;
		if (m_spoilsSpawnDataOnDisappear != null)
		{
			desc += PropDesc(m_spoilsSpawnDataOnDisappear, "[SpoilSpawnDataOnDisappear]");
		}
		desc += PropDesc(m_hitActorsInBetweenMod, "[HitActorsInBetween]", isValid, isValid && tricksterMadeYouLook.m_hitActorsInBetween);
		desc += PropDesc(m_radiusFromLineMod, "[RadiusFromLine]", isValid, isValid ? tricksterMadeYouLook.m_radiusFromLine : 0f);
		desc += PropDesc(m_radiusAroundEndsMod, "[RadiusAroundEnds]", isValid, isValid ? tricksterMadeYouLook.m_radiusAroundEnds : 0f);
		desc += PropDesc(m_penetrateLosMod, "[PenetrateLos]", isValid, isValid && tricksterMadeYouLook.m_penetrateLos);
		if (m_cooldownReductionForTravelHit != null && m_cooldownReductionForTravelHit.HasCooldownReduction())
		{
			desc += "Cooldown Reductions For Enemy Hit In Travel:\n";
			desc += m_cooldownReductionForTravelHit.GetDescription(abilityData);
		}
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? tricksterMadeYouLook.m_damageAmount : 0);
		return desc + PropDesc(m_enemyOnHitEffectMod, "[EnemyOnHitEffect]", isValid, isValid ? tricksterMadeYouLook.m_enemyOnHitEffect : null);
	}
}
