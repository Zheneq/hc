using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RageBeastSelfHeal : AbilityMod
{
	[Header("-- Base Effect Data Mod")]
	public AbilityModPropertyEffectData m_standardActorEffectDataMod;

	[Header("-- Health Threshold and Healing Amount Mod")]
	public AbilityModPropertyBool m_healOverTimeMod;

	public AbilityModPropertyInt m_healthThresholdMod;

	public AbilityModPropertyInt m_lowHealthHealOnCastMod;

	public AbilityModPropertyInt m_lowHealthHealOnTickMod;

	public AbilityModPropertyInt m_highHealthOnCastMod;

	public AbilityModPropertyInt m_highHealthOnTickMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(RageBeastSelfHeal);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RageBeastSelfHeal rageBeastSelfHeal = targetAbility as RageBeastSelfHeal;
		if (!(rageBeastSelfHeal != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_EffectMod(tokens, m_standardActorEffectDataMod, "StandardActorEffectData", rageBeastSelfHeal.m_standardActorEffectData);
			AbilityMod.AddToken(tokens, m_lowHealthHealOnCastMod, "HealingOnCastIfUnder", string.Empty, rageBeastSelfHeal.m_healingOnCastIfUnder);
			AbilityMod.AddToken(tokens, m_lowHealthHealOnTickMod, "HealingOnTickIfUnder", string.Empty, rageBeastSelfHeal.m_healingOnTickIfUnder);
			AbilityMod.AddToken(tokens, m_highHealthOnCastMod, "HealingOnCastIfOver", string.Empty, rageBeastSelfHeal.m_healingOnCastIfOver);
			AbilityMod.AddToken(tokens, m_highHealthOnTickMod, "HealingOnTickIfOver", string.Empty, rageBeastSelfHeal.m_healingOnTickIfOver);
			AbilityMod.AddToken(tokens, m_healthThresholdMod, "HealthThreshold", string.Empty, rageBeastSelfHeal.m_healthThreshold);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastSelfHeal rageBeastSelfHeal = GetTargetAbilityOnAbilityData(abilityData) as RageBeastSelfHeal;
		bool flag = rageBeastSelfHeal != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyEffectData standardActorEffectDataMod = m_standardActorEffectDataMod;
		object baseVal;
		if (flag)
		{
			baseVal = rageBeastSelfHeal.m_standardActorEffectData;
		}
		else
		{
			baseVal = null;
		}
		empty = str + PropDesc(standardActorEffectDataMod, "[Base Effect on Self]", flag, (StandardActorEffectData)baseVal);
		string str2 = empty;
		AbilityModPropertyBool healOverTimeMod = m_healOverTimeMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = (rageBeastSelfHeal.m_healOverTime ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(healOverTimeMod, "[Should Heal Over-Time?]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyInt healthThresholdMod = m_healthThresholdMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = rageBeastSelfHeal.m_healthThreshold;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(healthThresholdMod, "[Health Threshold]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyInt lowHealthHealOnCastMod = m_lowHealthHealOnCastMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = rageBeastSelfHeal.m_healingOnCastIfUnder;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(lowHealthHealOnCastMod, "[Low Health, Heal on Cast]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt lowHealthHealOnTickMod = m_lowHealthHealOnTickMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = rageBeastSelfHeal.m_healingOnTickIfUnder;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(lowHealthHealOnTickMod, "[Low Health, Heal on Tick]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt highHealthOnCastMod = m_highHealthOnCastMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = rageBeastSelfHeal.m_healingOnCastIfOver;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + AbilityModHelper.GetModPropertyDesc(highHealthOnCastMod, "[High Health, Heal on Cast]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyInt highHealthOnTickMod = m_highHealthOnTickMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = rageBeastSelfHeal.m_healingOnTickIfOver;
		}
		else
		{
			baseVal7 = 0;
		}
		return str7 + AbilityModHelper.GetModPropertyDesc(highHealthOnTickMod, "[High Health, Heal on Tick]", flag, baseVal7);
	}
}
