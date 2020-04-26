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
		if (!(spaceMarineJetpack != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnSelfMod, "EffectOnSelf", spaceMarineJetpack.m_effectOnSelf);
			AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, spaceMarineJetpack.m_damage);
			AbilityMod.AddToken_EffectMod(tokens, m_additionalEffectOnEnemy, "DebuffData", spaceMarineJetpack.m_debuffData);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SpaceMarineJetpack spaceMarineJetpack = GetTargetAbilityOnAbilityData(abilityData) as SpaceMarineJetpack;
		bool flag = spaceMarineJetpack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyEffectInfo effectOnSelfMod = m_effectOnSelfMod;
		object baseVal;
		if (flag)
		{
			baseVal = spaceMarineJetpack.m_effectOnSelf;
		}
		else
		{
			baseVal = null;
		}
		empty = str + PropDesc(effectOnSelfMod, "[EffectOnSelf]", flag, (StandardEffectInfo)baseVal);
		string str2 = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = spaceMarineJetpack.m_damage;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(damageMod, "[Damage]", flag, baseVal2);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_effectOnCasterPerEnemyHit, "Effect on Caster per Enemy Hit", string.Empty, flag);
		empty += PropDesc(m_additionalEffectOnEnemy, "Additional Effect on Enemy", flag, spaceMarineJetpack.m_debuffData);
		return empty + AbilityModHelper.GetModPropertyDesc(m_cooldownResetThreshold, "[Cooldown Reset Health Threshold]", flag);
	}
}
