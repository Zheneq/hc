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
			AbilityMod.AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, bazookaGirlRocketJump.m_damageAmount);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlRocketJump bazookaGirlRocketJump = GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlRocketJump;
		bool flag = bazookaGirlRocketJump != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = bazookaGirlRocketJump.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(damageMod, "[Damage]", flag, baseVal);
		if (m_resetCooldownOnKill)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			empty += "Resets Cooldown on Kill (on beginning of next turn)\n";
		}
		return empty;
	}
}
