using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NanoSmithBlastShield : AbilityMod
{
	[Space(10f)]
	public AbilityModPropertyInt m_healOnEndIfHasRemainingAbsorbMod;

	public AbilityModPropertyInt m_energyGainOnShieldTargetMod;

	public AbilityModPropertyEffectData m_shieldEffectOverride;

	[Header("-- Extra Effect on Caster if targeting Ally")]
	public AbilityModPropertyEffectInfo m_extraEffectOnCasterIfTargetingAllyMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NanoSmithBlastShield);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NanoSmithBlastShield nanoSmithBlastShield = targetAbility as NanoSmithBlastShield;
		if (nanoSmithBlastShield != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NanoSmithBlastShield.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken_EffectMod(tokens, this.m_shieldEffectOverride, "ShieldEffect", nanoSmithBlastShield.m_shieldEffect, true);
			AbilityMod.AddToken(tokens, this.m_healOnEndIfHasRemainingAbsorbMod, "HealOnEndIfHasRemainingAbsorb", string.Empty, nanoSmithBlastShield.m_healOnEndIfHasRemainingAbsorb, true, false);
			AbilityMod.AddToken(tokens, this.m_energyGainOnShieldTargetMod, "EnergyGainOnShieldTarget", string.Empty, nanoSmithBlastShield.m_energyGainOnShieldTarget, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_extraEffectOnCasterIfTargetingAllyMod, "ExtraEffectOnCasterIfTargetingAlly", nanoSmithBlastShield.m_extraEffectOnCasterIfTargetingAlly, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NanoSmithBlastShield nanoSmithBlastShield = base.GetTargetAbilityOnAbilityData(abilityData) as NanoSmithBlastShield;
		bool flag = nanoSmithBlastShield != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyEffectData shieldEffectOverride = this.m_shieldEffectOverride;
		string prefix = "{ Shield Effect }";
		bool showBaseVal = flag;
		StandardActorEffectData baseVal;
		if (flag)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NanoSmithBlastShield.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = nanoSmithBlastShield.m_shieldEffect;
		}
		else
		{
			baseVal = null;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(shieldEffectOverride, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_healOnEndIfHasRemainingAbsorbMod, "[Heal If Has Remaining Absorb]", flag, (!flag) ? 0 : nanoSmithBlastShield.m_healOnEndIfHasRemainingAbsorb);
		string str2 = text;
		AbilityModPropertyInt energyGainOnShieldTargetMod = this.m_energyGainOnShieldTargetMod;
		string prefix2 = "[Energy Gain on Shield Target]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = nanoSmithBlastShield.m_energyGainOnShieldTarget;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(energyGainOnShieldTargetMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyEffectInfo extraEffectOnCasterIfTargetingAllyMod = this.m_extraEffectOnCasterIfTargetingAllyMod;
		string prefix3 = "[ExtraEffectOnCasterIfTargetingAlly]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
		if (flag)
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
			baseVal3 = nanoSmithBlastShield.m_extraEffectOnCasterIfTargetingAlly;
		}
		else
		{
			baseVal3 = null;
		}
		return str3 + base.PropDesc(extraEffectOnCasterIfTargetingAllyMod, prefix3, showBaseVal3, baseVal3);
	}
}
