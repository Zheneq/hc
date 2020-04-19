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
			AbilityMod.AddToken(tokens, this.m_damageMod, "DamageAmount", string.Empty, bazookaGirlRocketJump.m_damageAmount, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlRocketJump bazookaGirlRocketJump = base.GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlRocketJump;
		bool flag = bazookaGirlRocketJump != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix = "[Damage]";
		bool showBaseVal = flag;
		int baseVal;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BazookaGirlRocketJump.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = bazookaGirlRocketJump.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(damageMod, prefix, showBaseVal, baseVal);
		if (this.m_resetCooldownOnKill)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			text += "Resets Cooldown on Kill (on beginning of next turn)\n";
		}
		return text;
	}
}
