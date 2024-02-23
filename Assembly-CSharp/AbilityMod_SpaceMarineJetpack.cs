using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken_EffectMod(tokens, m_effectOnSelfMod, "EffectOnSelf", spaceMarineJetpack.m_effectOnSelf);
			AddToken(tokens, m_damageMod, "Damage", string.Empty, spaceMarineJetpack.m_damage);
			AddToken_EffectMod(tokens, m_additionalEffectOnEnemy, "DebuffData", spaceMarineJetpack.m_debuffData);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SpaceMarineJetpack spaceMarineJetpack = GetTargetAbilityOnAbilityData(abilityData) as SpaceMarineJetpack;
		bool isValid = spaceMarineJetpack != null;
		string desc = string.Empty;
		desc += PropDesc(m_effectOnSelfMod, "[EffectOnSelf]", isValid, isValid ? spaceMarineJetpack.m_effectOnSelf : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isValid, isValid ? spaceMarineJetpack.m_damage : 0);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnCasterPerEnemyHit, "Effect on Caster per Enemy Hit", string.Empty, isValid);
		desc += PropDesc(m_additionalEffectOnEnemy, "Additional Effect on Enemy", isValid, spaceMarineJetpack.m_debuffData);
		return new StringBuilder().Append(desc).Append(AbilityModHelper.GetModPropertyDesc(m_cooldownResetThreshold, "[Cooldown Reset Health Threshold]", isValid)).ToString();
	}
}
