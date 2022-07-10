using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BazookaGirlRocketJump : AbilityMod
{
	[Header("-- Damage Mod")]
	public AbilityModPropertyInt m_damageMod;
	[Header("-- Reset cooldown on Kill (on beginning of next turn)")]
	public bool m_resetCooldownOnKill;

	public override Type GetTargetAbilityType()
	{
		return typeof(BazookaGirlRocketJump);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BazookaGirlRocketJump bazookaGirlRocketJump = targetAbility as BazookaGirlRocketJump;
		if (bazookaGirlRocketJump != null)
		{
			AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, bazookaGirlRocketJump.m_damageAmount);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlRocketJump bazookaGirlRocketJump = GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlRocketJump;
		bool isAbilityPresent = bazookaGirlRocketJump != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? bazookaGirlRocketJump.m_damageAmount : 0);
		if (m_resetCooldownOnKill)
		{
			desc += "Resets Cooldown on Kill (on beginning of next turn)\n";
		}
		return desc;
	}
}
