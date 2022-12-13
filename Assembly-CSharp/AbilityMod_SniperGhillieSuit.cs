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
		if (sniperGhillieSuit != null)
		{
			if (m_healingOnSelf > 0)
			{
				AddToken_IntDiff(tokens, "HealOnSelf", string.Empty, m_healingOnSelf, false, 0);
			}
			if (m_useStealthEffectDataOverride && m_stealthEffectDataOverride != null)
			{
				m_stealthEffectDataOverride.AddTooltipTokens(tokens, "EffectOnSelf", true, sniperGhillieSuit.m_standardActorEffectData);
			}
			AddToken(tokens, m_cooldownResetHealthThresholdMod, "CooldownResetHealthThreshold", string.Empty, sniperGhillieSuit.m_cooldownResetHealthThreshold, true, false, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SniperGhillieSuit sniperGhillieSuit = GetTargetAbilityOnAbilityData(abilityData) as SniperGhillieSuit;
		bool isValid = sniperGhillieSuit != null;
		string desc = string.Empty;
		if (m_healingOnSelf > 0)
		{
			desc += "[Healing on Self] = " + m_healingOnSelf + "\n";
		}
		if (m_useStealthEffectDataOverride)
		{
			desc += AbilityModHelper.GetModEffectDataDesc(m_stealthEffectDataOverride, "{ Stealth Effect Data Override }", string.Empty, isValid, isValid ? sniperGhillieSuit.m_standardActorEffectData : null);
		}
		return desc + PropDesc(m_cooldownResetHealthThresholdMod, "[CooldownResetHealthThreshold]", isValid, isValid ? sniperGhillieSuit.m_cooldownResetHealthThreshold : 0f);
	}
}
