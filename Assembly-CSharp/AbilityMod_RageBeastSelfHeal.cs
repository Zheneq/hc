using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken_EffectMod(tokens, m_standardActorEffectDataMod, "StandardActorEffectData", rageBeastSelfHeal.m_standardActorEffectData);
			AddToken(tokens, m_lowHealthHealOnCastMod, "HealingOnCastIfUnder", string.Empty, rageBeastSelfHeal.m_healingOnCastIfUnder);
			AddToken(tokens, m_lowHealthHealOnTickMod, "HealingOnTickIfUnder", string.Empty, rageBeastSelfHeal.m_healingOnTickIfUnder);
			AddToken(tokens, m_highHealthOnCastMod, "HealingOnCastIfOver", string.Empty, rageBeastSelfHeal.m_healingOnCastIfOver);
			AddToken(tokens, m_highHealthOnTickMod, "HealingOnTickIfOver", string.Empty, rageBeastSelfHeal.m_healingOnTickIfOver);
			AddToken(tokens, m_healthThresholdMod, "HealthThreshold", string.Empty, rageBeastSelfHeal.m_healthThreshold);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastSelfHeal rageBeastSelfHeal = GetTargetAbilityOnAbilityData(abilityData) as RageBeastSelfHeal;
		bool isValid = rageBeastSelfHeal != null;
		string desc = string.Empty;
		desc += PropDesc(m_standardActorEffectDataMod, "[Base Effect on Self]", isValid, isValid ? rageBeastSelfHeal.m_standardActorEffectData : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_healOverTimeMod, "[Should Heal Over-Time?]", isValid, isValid && rageBeastSelfHeal.m_healOverTime);
		desc += AbilityModHelper.GetModPropertyDesc(m_healthThresholdMod, "[Health Threshold]", isValid, isValid ? rageBeastSelfHeal.m_healthThreshold : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_lowHealthHealOnCastMod, "[Low Health, Heal on Cast]", isValid, isValid ? rageBeastSelfHeal.m_healingOnCastIfUnder : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_lowHealthHealOnTickMod, "[Low Health, Heal on Tick]", isValid, isValid ? rageBeastSelfHeal.m_healingOnTickIfUnder : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_highHealthOnCastMod, "[High Health, Heal on Cast]", isValid, isValid ? rageBeastSelfHeal.m_healingOnCastIfOver : 0);
		return new StringBuilder().Append(desc).Append(AbilityModHelper.GetModPropertyDesc(m_highHealthOnTickMod, "[High Health, Heal on Tick]", isValid, isValid ? rageBeastSelfHeal.m_healingOnTickIfOver : 0)).ToString();
	}
}
