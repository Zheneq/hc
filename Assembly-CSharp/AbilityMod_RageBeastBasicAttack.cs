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
		if (!(rageBeastBasicAttack != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_coneAngleMod, "ConeWidthAngle", string.Empty, rageBeastBasicAttack.m_coneWidthAngle);
			AbilityMod.AddToken(tokens, m_coneInnerRadiusMod, "ConeLengthInner", string.Empty, rageBeastBasicAttack.m_coneLengthInner);
			AbilityMod.AddToken(tokens, m_coneOuterRadiusMod, "ConeLengthOuter", string.Empty, rageBeastBasicAttack.m_coneLengthOuter);
			AbilityMod.AddToken(tokens, m_innerDamageMod, "DamageAmountInner", string.Empty, rageBeastBasicAttack.m_damageAmountInner);
			AbilityMod.AddToken(tokens, m_outerDamageMod, "DamageAmountOuter", string.Empty, rageBeastBasicAttack.m_damageAmountOuter);
			AbilityMod.AddToken_EffectMod(tokens, m_effectInnerMod, "EffectInner", rageBeastBasicAttack.m_effectInner);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOuterMod, "EffectOuter", rageBeastBasicAttack.m_effectOuter);
			AbilityMod.AddToken(tokens, m_innerTpGain, "TpGainInner", string.Empty, rageBeastBasicAttack.m_tpGainInner);
			AbilityMod.AddToken(tokens, m_outerTpGain, "TpGainOuter", string.Empty, rageBeastBasicAttack.m_tpGainOuter);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastBasicAttack rageBeastBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as RageBeastBasicAttack;
		bool flag = rageBeastBasicAttack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat coneAngleMod = m_coneAngleMod;
		float baseVal;
		if (flag)
		{
			baseVal = rageBeastBasicAttack.m_coneWidthAngle;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(coneAngleMod, "[Cone Angle]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat coneInnerRadiusMod = m_coneInnerRadiusMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = rageBeastBasicAttack.m_coneLengthInner;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(coneInnerRadiusMod, "[Inner Radius]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat coneOuterRadiusMod = m_coneOuterRadiusMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = rageBeastBasicAttack.m_coneLengthOuter;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(coneOuterRadiusMod, "[Outer Radius]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyInt innerDamageMod = m_innerDamageMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = rageBeastBasicAttack.m_damageAmountInner;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(innerDamageMod, "[Inner Damage]", flag, baseVal4);
		empty += AbilityModHelper.GetModPropertyDesc(m_outerDamageMod, "[Outer Damage]", flag, flag ? rageBeastBasicAttack.m_damageAmountOuter : 0);
		empty += PropDesc(m_effectInnerMod, "[EffectInner]", flag, (!flag) ? null : rageBeastBasicAttack.m_effectInner);
		string str5 = empty;
		AbilityModPropertyEffectInfo effectOuterMod = m_effectOuterMod;
		object baseVal5;
		if (flag)
		{
			baseVal5 = rageBeastBasicAttack.m_effectOuter;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(effectOuterMod, "[EffectOuter]", flag, (StandardEffectInfo)baseVal5);
		empty += AbilityModHelper.GetModPropertyDesc(m_innerTpGain, "[Inner TechPoint Gain]", flag, flag ? rageBeastBasicAttack.m_tpGainInner : 0);
		string str6 = empty;
		AbilityModPropertyInt outerTpGain = m_outerTpGain;
		int baseVal6;
		if (flag)
		{
			baseVal6 = rageBeastBasicAttack.m_tpGainOuter;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + AbilityModHelper.GetModPropertyDesc(outerTpGain, "[Outer TechPoint Gain]", flag, baseVal6);
		if (m_extraDamagePerAdjacentEnemy != 0)
		{
			string text = empty;
			empty = text + "[Extra Damage Per Adjacent Enemy] = " + m_extraDamagePerAdjacentEnemy + "\n";
		}
		if (m_extraTechPointsPerAdjacentEnemy != 0)
		{
			string text = empty;
			empty = text + "[Extra Tech Points Per Adjacent Enemy] = " + m_extraTechPointsPerAdjacentEnemy + "\n";
		}
		return empty;
	}
}
