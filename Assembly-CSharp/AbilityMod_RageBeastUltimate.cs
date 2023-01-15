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
		if (rageBeastUltimate != null)
		{
			AddToken(tokens, m_plasmaDamageMod, "PlasmaDamage", string.Empty, rageBeastUltimate.m_plasmaDamage);
			AddToken(tokens, m_plasmaDurationMod, "PlasmaDuration", string.Empty, rageBeastUltimate.m_plasmaDuration);
			AddToken(tokens, m_selfHealOnCastMod, "SelfHealOnCast", string.Empty, rageBeastUltimate.m_selfHealOnCast);
			AddToken_EffectMod(tokens, m_extraEffectOnSelfMod, "ExtraEffectOnSelf", rageBeastUltimate.m_extraEffectOnSelf);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastUltimate rageBeastUltimate = GetTargetAbilityOnAbilityData(abilityData) as RageBeastUltimate;
		bool isValid = rageBeastUltimate != null;
		string desc = string.Empty;
		desc += PropDesc(m_plasmaDamageMod, "[Plasma Damage]", isValid, isValid ? rageBeastUltimate.m_plasmaDamage : 0);
		desc += PropDesc(m_plasmaDurationMod, "[Plasma Duration]", isValid, isValid ? rageBeastUltimate.m_plasmaDuration : 0);
		if (m_passiveTechPointRegen > 0)
		{
			desc += "Passive TechPoint Regen while has Mod: " + m_passiveTechPointRegen + "\n";
		}
		desc += PropDesc(m_plasmaHealingMod, "[Plasma Ally Healing]", isValid);
		desc += PropDesc(m_plasmaTechPointGainMod, "[Plasma Ally Tech Point Gain]", isValid);
		desc += PropDesc(m_selfHealOnCastMod, "[SelfHealOnCast]", isValid, isValid ? rageBeastUltimate.m_selfHealOnCast : 0);
		return desc + PropDesc(m_extraEffectOnSelfMod, "[ExtraEffectOnSelf]", isValid, isValid ? rageBeastUltimate.m_extraEffectOnSelf : null);
	}
}
