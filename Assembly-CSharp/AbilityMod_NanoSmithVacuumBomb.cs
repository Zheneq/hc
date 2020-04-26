using System;
using System.Collections.Generic;
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
		if (!(nanoSmithVacuumBomb != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_damageMod, "BombDamageAmount", string.Empty, nanoSmithVacuumBomb.m_bombDamageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectOverride, "EnemyHitEffect", nanoSmithVacuumBomb.m_enemyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_onCenterActorEffectOverride, "OnCenterActorEffect", nanoSmithVacuumBomb.m_onCenterActorEffect);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NanoSmithVacuumBomb nanoSmithVacuumBomb = GetTargetAbilityOnAbilityData(abilityData) as NanoSmithVacuumBomb;
		bool flag = nanoSmithVacuumBomb != null;
		string empty = string.Empty;
		empty += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", flag, flag ? nanoSmithVacuumBomb.m_bombDamageAmount : 0);
		empty += AbilityModHelper.GetModPropertyDesc(m_cooldownChangePerHitMod, "[Cooldown Change Per Hit]", flag);
		string str = empty;
		AbilityModPropertyEffectInfo onCenterActorEffectOverride = m_onCenterActorEffectOverride;
		object baseVal;
		if (flag)
		{
			baseVal = nanoSmithVacuumBomb.m_onCenterActorEffect;
		}
		else
		{
			baseVal = null;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(onCenterActorEffectOverride, "{ On CenterActor Effect Override }", flag, (StandardEffectInfo)baseVal);
		return empty + AbilityModHelper.GetModPropertyDesc(m_enemyHitEffectOverride, "{ Enemy Hit Effect Override }", flag, (!flag) ? null : nanoSmithVacuumBomb.m_enemyHitEffect);
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability ability, List<int> numbers)
	{
		numbers.Add(Mathf.Abs(m_cooldownChangePerHitMod.GetModifiedValue(0)));
	}
}
