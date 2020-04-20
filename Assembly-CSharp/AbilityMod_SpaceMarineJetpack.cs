using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SpaceMarineJetpack : AbilityMod
{
	[Header("-- Effect on Self --")]
	public AbilityModPropertyEffectInfo m_effectOnSelfMod;

	[Header("-- Damage Mod")]
	public AbilityModPropertyInt m_damageMod;

	[Header("-- Effect on Caster per Enemy Hit")]
	public StandardEffectInfo m_effectOnCasterPerEnemyHit;

	[Header("-- Additional Effect on Enemy")]
	public AbilityModPropertyEffectData m_additionalEffectOnEnemy;

	[Header("-- Cooldown Reset Health Threshold (Less Than)")]
	public AbilityModPropertyInt m_cooldownResetThreshold;

	public override Type GetTargetAbilityType()
	{
		return typeof(SpaceMarineJetpack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SpaceMarineJetpack spaceMarineJetpack = targetAbility as SpaceMarineJetpack;
		if (spaceMarineJetpack != null)
		{
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnSelfMod, "EffectOnSelf", spaceMarineJetpack.m_effectOnSelf, true);
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, spaceMarineJetpack.m_damage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_additionalEffectOnEnemy, "DebuffData", spaceMarineJetpack.m_debuffData, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SpaceMarineJetpack spaceMarineJetpack = base.GetTargetAbilityOnAbilityData(abilityData) as SpaceMarineJetpack;
		bool flag = spaceMarineJetpack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyEffectInfo effectOnSelfMod = this.m_effectOnSelfMod;
		string prefix = "[EffectOnSelf]";
		bool showBaseVal = flag;
		StandardEffectInfo baseVal;
		if (flag)
		{
			baseVal = spaceMarineJetpack.m_effectOnSelf;
		}
		else
		{
			baseVal = null;
		}
		text = str + base.PropDesc(effectOnSelfMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix2 = "[Damage]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = spaceMarineJetpack.m_damage;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(damageMod, prefix2, showBaseVal2, baseVal2);
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnCasterPerEnemyHit, "Effect on Caster per Enemy Hit", string.Empty, flag, null);
		text += base.PropDesc(this.m_additionalEffectOnEnemy, "Additional Effect on Enemy", flag, spaceMarineJetpack.m_debuffData);
		return text + AbilityModHelper.GetModPropertyDesc(this.m_cooldownResetThreshold, "[Cooldown Reset Health Threshold]", flag, 0);
	}
}
