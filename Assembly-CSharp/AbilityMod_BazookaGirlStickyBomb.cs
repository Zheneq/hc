using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_BazookaGirlStickyBomb : AbilityMod
{
	[Header("-- On Cast Hit")]
	public AbilityModPropertyInt m_energyGainOnCastPerEnemyHitMod;
	public AbilityModPropertyEffectInfo m_enemyOnCastHitEffectOverride;
	[Header("-- On Explosion Hit Effect Override")]
	public AbilityModPropertyEffectInfo m_enemyOnExplosionEffectOverride;
	[Header("-- Cooldown modification on Explosion")]
	public AbilityData.ActionType m_cooldownModOnAction = AbilityData.ActionType.INVALID_ACTION;
	public int m_cooldownAddAmount;

	public override Type GetTargetAbilityType()
	{
		return typeof(BazookaGirlStickyBomb);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BazookaGirlStickyBomb bazookaGirlStickyBomb = targetAbility as BazookaGirlStickyBomb;
		if (bazookaGirlStickyBomb != null)
		{
			AddToken(tokens, m_energyGainOnCastPerEnemyHitMod, "EnergyGainOnCastPerEnemyHit", string.Empty, bazookaGirlStickyBomb.m_energyGainOnCastPerEnemyHit);
			AddToken_EffectMod(tokens, m_enemyOnCastHitEffectOverride, "EnemyOnCastHitEffect", bazookaGirlStickyBomb.m_enemyOnCastHitEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlStickyBomb bazookaGirlStickyBomb = GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlStickyBomb;
		bool isAbilityPresent = bazookaGirlStickyBomb != null;
		string desc = string.Empty;
		desc += PropDesc(m_energyGainOnCastPerEnemyHitMod, "[EnergyGainOnCastPerEnemyHit]", isAbilityPresent, isAbilityPresent ? bazookaGirlStickyBomb.m_energyGainOnCastPerEnemyHit : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_enemyOnCastHitEffectOverride, "{ Enemy On Cast Hit Effect }", isAbilityPresent, isAbilityPresent ? bazookaGirlStickyBomb.m_enemyOnCastHitEffect : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_enemyOnExplosionEffectOverride, "{ Enemy on Explode Effect }", isAbilityPresent, isAbilityPresent ? bazookaGirlStickyBomb.m_bombInfo.onExplodeEffect : null);
		if (m_cooldownModOnAction != AbilityData.ActionType.INVALID_ACTION && m_cooldownAddAmount != 0)
		{
			desc += new StringBuilder().Append(m_cooldownAddAmount < 0 ? "Reduces" : "Increases").Append(" cooldown on ").Append(AbilityModHelper.GetAbilityNameFromActionType(m_cooldownModOnAction, abilityData)).Append(" by ").Append(Mathf.Abs(m_cooldownAddAmount)).Append(" per explosion").ToString();
		}
		return desc;
	}
}
