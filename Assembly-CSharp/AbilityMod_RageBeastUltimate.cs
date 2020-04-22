using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RageBeastUltimate : AbilityMod
{
	[Header("-- Plasma Damage/Duration Mod")]
	public AbilityModPropertyInt m_plasmaDamageMod;

	public AbilityModPropertyInt m_plasmaDurationMod;

	[Header("-- Passive TechPoint Regen (while Mod is applied)")]
	public int m_passiveTechPointRegen;

	[Header("-- Ally Boosts While In Plasma")]
	public AbilityModPropertyInt m_plasmaHealingMod;

	public AbilityModPropertyInt m_plasmaTechPointGainMod;

	[Header("-- Extra effect on Self")]
	public AbilityModPropertyInt m_selfHealOnCastMod;

	public AbilityModPropertyEffectInfo m_extraEffectOnSelfMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(RageBeastUltimate);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RageBeastUltimate rageBeastUltimate = targetAbility as RageBeastUltimate;
		if (!(rageBeastUltimate != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_plasmaDamageMod, "PlasmaDamage", string.Empty, rageBeastUltimate.m_plasmaDamage);
			AbilityMod.AddToken(tokens, m_plasmaDurationMod, "PlasmaDuration", string.Empty, rageBeastUltimate.m_plasmaDuration);
			AbilityMod.AddToken(tokens, m_selfHealOnCastMod, "SelfHealOnCast", string.Empty, rageBeastUltimate.m_selfHealOnCast);
			AbilityMod.AddToken_EffectMod(tokens, m_extraEffectOnSelfMod, "ExtraEffectOnSelf", rageBeastUltimate.m_extraEffectOnSelf);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastUltimate rageBeastUltimate = GetTargetAbilityOnAbilityData(abilityData) as RageBeastUltimate;
		bool flag = rageBeastUltimate != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt plasmaDamageMod = m_plasmaDamageMod;
		int baseVal;
		if (flag)
		{
			baseVal = rageBeastUltimate.m_plasmaDamage;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(plasmaDamageMod, "[Plasma Damage]", flag, baseVal);
		empty += PropDesc(m_plasmaDurationMod, "[Plasma Duration]", flag, flag ? rageBeastUltimate.m_plasmaDuration : 0);
		if (m_passiveTechPointRegen > 0)
		{
			string text = empty;
			empty = text + "Passive TechPoint Regen while has Mod: " + m_passiveTechPointRegen + "\n";
		}
		empty += PropDesc(m_plasmaHealingMod, "[Plasma Ally Healing]", flag);
		empty += PropDesc(m_plasmaTechPointGainMod, "[Plasma Ally Tech Point Gain]", flag);
		string str2 = empty;
		AbilityModPropertyInt selfHealOnCastMod = m_selfHealOnCastMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = rageBeastUltimate.m_selfHealOnCast;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(selfHealOnCastMod, "[SelfHealOnCast]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyEffectInfo extraEffectOnSelfMod = m_extraEffectOnSelfMod;
		object baseVal3;
		if (flag)
		{
			baseVal3 = rageBeastUltimate.m_extraEffectOnSelf;
		}
		else
		{
			baseVal3 = null;
		}
		return str3 + PropDesc(extraEffectOnSelfMod, "[ExtraEffectOnSelf]", flag, (StandardEffectInfo)baseVal3);
	}
}
