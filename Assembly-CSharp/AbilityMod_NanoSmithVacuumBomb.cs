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
		if (nanoSmithVacuumBomb != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NanoSmithVacuumBomb.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_damageMod, "BombDamageAmount", string.Empty, nanoSmithVacuumBomb.m_bombDamageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectOverride, "EnemyHitEffect", nanoSmithVacuumBomb.m_enemyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_onCenterActorEffectOverride, "OnCenterActorEffect", nanoSmithVacuumBomb.m_onCenterActorEffect, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NanoSmithVacuumBomb nanoSmithVacuumBomb = base.GetTargetAbilityOnAbilityData(abilityData) as NanoSmithVacuumBomb;
		bool flag = nanoSmithVacuumBomb != null;
		string text = string.Empty;
		text += AbilityModHelper.GetModPropertyDesc(this.m_damageMod, "[Damage]", flag, (!flag) ? 0 : nanoSmithVacuumBomb.m_bombDamageAmount);
		text += AbilityModHelper.GetModPropertyDesc(this.m_cooldownChangePerHitMod, "[Cooldown Change Per Hit]", flag, 0);
		string str = text;
		AbilityModPropertyEffectInfo onCenterActorEffectOverride = this.m_onCenterActorEffectOverride;
		string prefix = "{ On CenterActor Effect Override }";
		bool showBaseVal = flag;
		StandardEffectInfo baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NanoSmithVacuumBomb.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = nanoSmithVacuumBomb.m_onCenterActorEffect;
		}
		else
		{
			baseVal = null;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(onCenterActorEffectOverride, prefix, showBaseVal, baseVal);
		return text + AbilityModHelper.GetModPropertyDesc(this.m_enemyHitEffectOverride, "{ Enemy Hit Effect Override }", flag, (!flag) ? null : nanoSmithVacuumBomb.m_enemyHitEffect);
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability ability, List<int> numbers)
	{
		numbers.Add(Mathf.Abs(this.m_cooldownChangePerHitMod.GetModifiedValue(0)));
	}
}
