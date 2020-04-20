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
			AbilityMod.AddToken(tokens, this.m_plasmaDamageMod, "PlasmaDamage", string.Empty, rageBeastUltimate.m_plasmaDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_plasmaDurationMod, "PlasmaDuration", string.Empty, rageBeastUltimate.m_plasmaDuration, true, false);
			AbilityMod.AddToken(tokens, this.m_selfHealOnCastMod, "SelfHealOnCast", string.Empty, rageBeastUltimate.m_selfHealOnCast, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_extraEffectOnSelfMod, "ExtraEffectOnSelf", rageBeastUltimate.m_extraEffectOnSelf, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastUltimate rageBeastUltimate = base.GetTargetAbilityOnAbilityData(abilityData) as RageBeastUltimate;
		bool flag = rageBeastUltimate != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt plasmaDamageMod = this.m_plasmaDamageMod;
		string prefix = "[Plasma Damage]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = rageBeastUltimate.m_plasmaDamage;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(plasmaDamageMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_plasmaDurationMod, "[Plasma Duration]", flag, (!flag) ? 0 : rageBeastUltimate.m_plasmaDuration);
		if (this.m_passiveTechPointRegen > 0)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"Passive TechPoint Regen while has Mod: ",
				this.m_passiveTechPointRegen,
				"\n"
			});
		}
		text += base.PropDesc(this.m_plasmaHealingMod, "[Plasma Ally Healing]", flag, 0);
		text += base.PropDesc(this.m_plasmaTechPointGainMod, "[Plasma Ally Tech Point Gain]", flag, 0);
		string str2 = text;
		AbilityModPropertyInt selfHealOnCastMod = this.m_selfHealOnCastMod;
		string prefix2 = "[SelfHealOnCast]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = rageBeastUltimate.m_selfHealOnCast;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(selfHealOnCastMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyEffectInfo extraEffectOnSelfMod = this.m_extraEffectOnSelfMod;
		string prefix3 = "[ExtraEffectOnSelf]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
		if (flag)
		{
			baseVal3 = rageBeastUltimate.m_extraEffectOnSelf;
		}
		else
		{
			baseVal3 = null;
		}
		return str3 + base.PropDesc(extraEffectOnSelfMod, prefix3, showBaseVal3, baseVal3);
	}
}
