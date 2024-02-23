using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_NanoSmithVacuumBomb : AbilityMod
{
	[Space(10f)]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyInt m_cooldownChangePerHitMod;
	public AbilityModPropertyEffectInfo m_onCenterActorEffectOverride;
	public AbilityModPropertyEffectInfo m_enemyHitEffectOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(NanoSmithVacuumBomb);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NanoSmithVacuumBomb nanoSmithVacuumBomb = targetAbility as NanoSmithVacuumBomb;
		if (nanoSmithVacuumBomb != null)
		{
			AddToken(tokens, m_damageMod, "BombDamageAmount", string.Empty, nanoSmithVacuumBomb.m_bombDamageAmount);
			AddToken_EffectMod(tokens, m_enemyHitEffectOverride, "EnemyHitEffect", nanoSmithVacuumBomb.m_enemyHitEffect);
			AddToken_EffectMod(tokens, m_onCenterActorEffectOverride, "OnCenterActorEffect", nanoSmithVacuumBomb.m_onCenterActorEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NanoSmithVacuumBomb nanoSmithVacuumBomb = GetTargetAbilityOnAbilityData(abilityData) as NanoSmithVacuumBomb;
		bool isValid = nanoSmithVacuumBomb != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isValid, isValid ? nanoSmithVacuumBomb.m_bombDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_cooldownChangePerHitMod, "[Cooldown Change Per Hit]", isValid);
		desc += AbilityModHelper.GetModPropertyDesc(m_onCenterActorEffectOverride, "{ On CenterActor Effect Override }", isValid, isValid ? nanoSmithVacuumBomb.m_onCenterActorEffect : null);
		return new StringBuilder().Append(desc).Append(AbilityModHelper.GetModPropertyDesc(m_enemyHitEffectOverride, "{ Enemy Hit Effect Override }", isValid, isValid ? nanoSmithVacuumBomb.m_enemyHitEffect : null)).ToString();
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability ability, List<int> numbers)
	{
		numbers.Add(Mathf.Abs(m_cooldownChangePerHitMod.GetModifiedValue(0)));
	}
}
