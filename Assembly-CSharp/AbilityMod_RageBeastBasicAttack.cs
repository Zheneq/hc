using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RageBeastBasicAttack : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_coneAngleMod;
	public AbilityModPropertyFloat m_coneInnerRadiusMod;
	public AbilityModPropertyFloat m_coneOuterRadiusMod;
	[Header("-- Damage")]
	public AbilityModPropertyInt m_innerDamageMod;
	public AbilityModPropertyInt m_outerDamageMod;
	public int m_extraDamagePerAdjacentEnemy;
	[Header("-- Effect")]
	public AbilityModPropertyEffectInfo m_effectInnerMod;
	public AbilityModPropertyEffectInfo m_effectOuterMod;
	[Header("-- Tech Point change on Hit")]
	public AbilityModPropertyInt m_innerTpGain;
	public AbilityModPropertyInt m_outerTpGain;
	public int m_extraTechPointsPerAdjacentEnemy;

	public override Type GetTargetAbilityType()
	{
		return typeof(RageBeastBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RageBeastBasicAttack rageBeastBasicAttack = targetAbility as RageBeastBasicAttack;
		if (rageBeastBasicAttack != null)
		{
			AddToken(tokens, m_coneAngleMod, "ConeWidthAngle", string.Empty, rageBeastBasicAttack.m_coneWidthAngle);
			AddToken(tokens, m_coneInnerRadiusMod, "ConeLengthInner", string.Empty, rageBeastBasicAttack.m_coneLengthInner);
			AddToken(tokens, m_coneOuterRadiusMod, "ConeLengthOuter", string.Empty, rageBeastBasicAttack.m_coneLengthOuter);
			AddToken(tokens, m_innerDamageMod, "DamageAmountInner", string.Empty, rageBeastBasicAttack.m_damageAmountInner);
			AddToken(tokens, m_outerDamageMod, "DamageAmountOuter", string.Empty, rageBeastBasicAttack.m_damageAmountOuter);
			AddToken_EffectMod(tokens, m_effectInnerMod, "EffectInner", rageBeastBasicAttack.m_effectInner);
			AddToken_EffectMod(tokens, m_effectOuterMod, "EffectOuter", rageBeastBasicAttack.m_effectOuter);
			AddToken(tokens, m_innerTpGain, "TpGainInner", string.Empty, rageBeastBasicAttack.m_tpGainInner);
			AddToken(tokens, m_outerTpGain, "TpGainOuter", string.Empty, rageBeastBasicAttack.m_tpGainOuter);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastBasicAttack rageBeastBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as RageBeastBasicAttack;
		bool isValid = rageBeastBasicAttack != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_coneAngleMod, "[Cone Angle]", isValid, isValid ? rageBeastBasicAttack.m_coneWidthAngle : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_coneInnerRadiusMod, "[Inner Radius]", isValid, isValid ? rageBeastBasicAttack.m_coneLengthInner : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_coneOuterRadiusMod, "[Outer Radius]", isValid, isValid ? rageBeastBasicAttack.m_coneLengthOuter : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_innerDamageMod, "[Inner Damage]", isValid, isValid ? rageBeastBasicAttack.m_damageAmountInner : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_outerDamageMod, "[Outer Damage]", isValid, isValid ? rageBeastBasicAttack.m_damageAmountOuter : 0);
		desc += PropDesc(m_effectInnerMod, "[EffectInner]", isValid, isValid ? rageBeastBasicAttack.m_effectInner : null);
		desc += PropDesc(m_effectOuterMod, "[EffectOuter]", isValid, isValid ? rageBeastBasicAttack.m_effectOuter : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_innerTpGain, "[Inner TechPoint Gain]", isValid, isValid ? rageBeastBasicAttack.m_tpGainInner : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_outerTpGain, "[Outer TechPoint Gain]", isValid, isValid ? rageBeastBasicAttack.m_tpGainOuter : 0);
		if (m_extraDamagePerAdjacentEnemy != 0)
		{
			desc += "[Extra Damage Per Adjacent Enemy] = " + m_extraDamagePerAdjacentEnemy + "\n";
		}
		if (m_extraTechPointsPerAdjacentEnemy != 0)
		{
			desc += "[Extra Tech Points Per Adjacent Enemy] = " + m_extraTechPointsPerAdjacentEnemy + "\n";
		}
		return desc;
	}
}
