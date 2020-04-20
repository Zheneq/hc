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
			AbilityMod.AddToken(tokens, this.m_radiusFromLineMod, "RadiusFromLine", string.Empty, tricksterMadeYouLook.m_radiusFromLine, true, false, false);
			AbilityMod.AddToken(tokens, this.m_radiusAroundEndsMod, "RadiusAroundEnds", string.Empty, tricksterMadeYouLook.m_radiusAroundEnds, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, tricksterMadeYouLook.m_damageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyOnHitEffectMod, "EnemyOnHitEffect", tricksterMadeYouLook.m_enemyOnHitEffect, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TricksterMadeYouLook tricksterMadeYouLook = base.GetTargetAbilityOnAbilityData(abilityData) as TricksterMadeYouLook;
		bool flag = tricksterMadeYouLook != null;
		string text = string.Empty;
		if (this.m_spoilsSpawnDataOnDisappear != null)
		{
			text += base.PropDesc(this.m_spoilsSpawnDataOnDisappear, "[SpoilSpawnDataOnDisappear]", false, null);
		}
		string str = text;
		AbilityModPropertyBool hitActorsInBetweenMod = this.m_hitActorsInBetweenMod;
		string prefix = "[HitActorsInBetween]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			baseVal = tricksterMadeYouLook.m_hitActorsInBetween;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(hitActorsInBetweenMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_radiusFromLineMod, "[RadiusFromLine]", flag, (!flag) ? 0f : tricksterMadeYouLook.m_radiusFromLine);
		text += base.PropDesc(this.m_radiusAroundEndsMod, "[RadiusAroundEnds]", flag, (!flag) ? 0f : tricksterMadeYouLook.m_radiusAroundEnds);
		string str2 = text;
		AbilityModPropertyBool penetrateLosMod = this.m_penetrateLosMod;
		string prefix2 = "[PenetrateLos]";
		bool showBaseVal2 = flag;
		bool baseVal2;
		if (flag)
		{
			baseVal2 = tricksterMadeYouLook.m_penetrateLos;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(penetrateLosMod, prefix2, showBaseVal2, baseVal2);
		if (this.m_cooldownReductionForTravelHit != null)
		{
			if (this.m_cooldownReductionForTravelHit.HasCooldownReduction())
			{
				text += "Cooldown Reductions For Enemy Hit In Travel:\n";
				text += this.m_cooldownReductionForTravelHit.GetDescription(abilityData);
			}
		}
		string str3 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix3 = "[DamageAmount]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = tricksterMadeYouLook.m_damageAmount;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(damageAmountMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyEffectInfo enemyOnHitEffectMod = this.m_enemyOnHitEffectMod;
		string prefix4 = "[EnemyOnHitEffect]";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
		if (flag)
		{
			baseVal4 = tricksterMadeYouLook.m_enemyOnHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		return str4 + base.PropDesc(enemyOnHitEffectMod, prefix4, showBaseVal4, baseVal4);
	}
}
