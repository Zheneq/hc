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
		if (rageBeastSelfHeal != null)
		{
			AbilityMod.AddToken_EffectMod(tokens, this.m_standardActorEffectDataMod, "StandardActorEffectData", rageBeastSelfHeal.m_standardActorEffectData, true);
			AbilityMod.AddToken(tokens, this.m_lowHealthHealOnCastMod, "HealingOnCastIfUnder", string.Empty, rageBeastSelfHeal.m_healingOnCastIfUnder, true, false);
			AbilityMod.AddToken(tokens, this.m_lowHealthHealOnTickMod, "HealingOnTickIfUnder", string.Empty, rageBeastSelfHeal.m_healingOnTickIfUnder, true, false);
			AbilityMod.AddToken(tokens, this.m_highHealthOnCastMod, "HealingOnCastIfOver", string.Empty, rageBeastSelfHeal.m_healingOnCastIfOver, true, false);
			AbilityMod.AddToken(tokens, this.m_highHealthOnTickMod, "HealingOnTickIfOver", string.Empty, rageBeastSelfHeal.m_healingOnTickIfOver, true, false);
			AbilityMod.AddToken(tokens, this.m_healthThresholdMod, "HealthThreshold", string.Empty, rageBeastSelfHeal.m_healthThreshold, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastSelfHeal rageBeastSelfHeal = base.GetTargetAbilityOnAbilityData(abilityData) as RageBeastSelfHeal;
		bool flag = rageBeastSelfHeal != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyEffectData standardActorEffectDataMod = this.m_standardActorEffectDataMod;
		string prefix = "[Base Effect on Self]";
		bool showBaseVal = flag;
		StandardActorEffectData baseVal;
		if (flag)
		{
			baseVal = rageBeastSelfHeal.m_standardActorEffectData;
		}
		else
		{
			baseVal = null;
		}
		text = str + base.PropDesc(standardActorEffectDataMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyBool healOverTimeMod = this.m_healOverTimeMod;
		string prefix2 = "[Should Heal Over-Time?]";
		bool showBaseVal2 = flag;
		bool baseVal2;
		if (flag)
		{
			baseVal2 = rageBeastSelfHeal.m_healOverTime;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(healOverTimeMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt healthThresholdMod = this.m_healthThresholdMod;
		string prefix3 = "[Health Threshold]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = rageBeastSelfHeal.m_healthThreshold;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(healthThresholdMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt lowHealthHealOnCastMod = this.m_lowHealthHealOnCastMod;
		string prefix4 = "[Low Health, Heal on Cast]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = rageBeastSelfHeal.m_healingOnCastIfUnder;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(lowHealthHealOnCastMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt lowHealthHealOnTickMod = this.m_lowHealthHealOnTickMod;
		string prefix5 = "[Low Health, Heal on Tick]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = rageBeastSelfHeal.m_healingOnTickIfUnder;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(lowHealthHealOnTickMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt highHealthOnCastMod = this.m_highHealthOnCastMod;
		string prefix6 = "[High Health, Heal on Cast]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = rageBeastSelfHeal.m_healingOnCastIfOver;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + AbilityModHelper.GetModPropertyDesc(highHealthOnCastMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt highHealthOnTickMod = this.m_highHealthOnTickMod;
		string prefix7 = "[High Health, Heal on Tick]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = rageBeastSelfHeal.m_healingOnTickIfOver;
		}
		else
		{
			baseVal7 = 0;
		}
		return str7 + AbilityModHelper.GetModPropertyDesc(highHealthOnTickMod, prefix7, showBaseVal7, baseVal7);
	}
}
