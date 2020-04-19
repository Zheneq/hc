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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SniperGhillieSuit.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			if (this.m_healingOnSelf > 0)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				AbilityMod.AddToken_IntDiff(tokens, "HealOnSelf", string.Empty, this.m_healingOnSelf, false, 0);
			}
			if (this.m_useStealthEffectDataOverride)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_stealthEffectDataOverride != null)
				{
					this.m_stealthEffectDataOverride.AddTooltipTokens(tokens, "EffectOnSelf", true, sniperGhillieSuit.m_standardActorEffectData);
				}
			}
			AbilityMod.AddToken(tokens, this.m_cooldownResetHealthThresholdMod, "CooldownResetHealthThreshold", string.Empty, sniperGhillieSuit.m_cooldownResetHealthThreshold, true, false, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SniperGhillieSuit sniperGhillieSuit = base.GetTargetAbilityOnAbilityData(abilityData) as SniperGhillieSuit;
		bool flag = sniperGhillieSuit != null;
		string text = string.Empty;
		if (this.m_healingOnSelf > 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SniperGhillieSuit.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Healing on Self] = ",
				this.m_healingOnSelf,
				"\n"
			});
		}
		if (this.m_useStealthEffectDataOverride)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			string str = text;
			StandardActorEffectData stealthEffectDataOverride = this.m_stealthEffectDataOverride;
			string prefix = "{ Stealth Effect Data Override }";
			string empty = string.Empty;
			bool useBaseVal = flag;
			StandardActorEffectData baseVal;
			if (flag)
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
				baseVal = sniperGhillieSuit.m_standardActorEffectData;
			}
			else
			{
				baseVal = null;
			}
			text = str + AbilityModHelper.GetModEffectDataDesc(stealthEffectDataOverride, prefix, empty, useBaseVal, baseVal);
		}
		return text + base.PropDesc(this.m_cooldownResetHealthThresholdMod, "[CooldownResetHealthThreshold]", flag, (!flag) ? 0f : sniperGhillieSuit.m_cooldownResetHealthThreshold);
	}
}
