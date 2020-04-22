using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SniperGhillieSuit : AbilityMod
{
	[Header("-- Healing on Self")]
	public int m_healingOnSelf;

	[Header("-- Stealth Effect Data Override")]
	public bool m_useStealthEffectDataOverride;

	public StandardActorEffectData m_stealthEffectDataOverride;

	[Header("-- Health threshold to trigger cooldown reset, value:(0-1)")]
	public AbilityModPropertyFloat m_cooldownResetHealthThresholdMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SniperGhillieSuit);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SniperGhillieSuit sniperGhillieSuit = targetAbility as SniperGhillieSuit;
		if (!(sniperGhillieSuit != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_healingOnSelf > 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				AbilityMod.AddToken_IntDiff(tokens, "HealOnSelf", string.Empty, m_healingOnSelf, false, 0);
			}
			if (m_useStealthEffectDataOverride)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_stealthEffectDataOverride != null)
				{
					m_stealthEffectDataOverride.AddTooltipTokens(tokens, "EffectOnSelf", true, sniperGhillieSuit.m_standardActorEffectData);
				}
			}
			AbilityMod.AddToken(tokens, m_cooldownResetHealthThresholdMod, "CooldownResetHealthThreshold", string.Empty, sniperGhillieSuit.m_cooldownResetHealthThreshold, true, false, true);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SniperGhillieSuit sniperGhillieSuit = GetTargetAbilityOnAbilityData(abilityData) as SniperGhillieSuit;
		bool flag = sniperGhillieSuit != null;
		string text = string.Empty;
		if (m_healingOnSelf > 0)
		{
			while (true)
			{
				switch (5)
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
			string text2 = text;
			text = text2 + "[Healing on Self] = " + m_healingOnSelf + "\n";
		}
		if (m_useStealthEffectDataOverride)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			string str = text;
			StandardActorEffectData stealthEffectDataOverride = m_stealthEffectDataOverride;
			string empty = string.Empty;
			object baseVal;
			if (flag)
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
				baseVal = sniperGhillieSuit.m_standardActorEffectData;
			}
			else
			{
				baseVal = null;
			}
			text = str + AbilityModHelper.GetModEffectDataDesc(stealthEffectDataOverride, "{ Stealth Effect Data Override }", empty, flag, (StandardActorEffectData)baseVal);
		}
		return text + PropDesc(m_cooldownResetHealthThresholdMod, "[CooldownResetHealthThreshold]", flag, (!flag) ? 0f : sniperGhillieSuit.m_cooldownResetHealthThreshold);
	}
}
