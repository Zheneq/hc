using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SorceressDamageField : AbilityMod
{
	[Header("-- Shape Override")]
	public AbilityModPropertyShape m_shapeOverride;
	public GameObject m_persistentSequencePrefabOverride;
	[Header("-- Duration Override")]
	public AbilityModPropertyInt m_durationMod;
	[Header("-- Damage and Healing Mod")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyInt m_healingMod;
	[Header("-- Effect Overrides")]
	public AbilityModPropertyEffectInfo m_onEnemyEffectOverride;
	public AbilityModPropertyEffectInfo m_onAllyEffectOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(SorceressDamageField);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SorceressDamageField sorceressDamageField = targetAbility as SorceressDamageField;
		if (sorceressDamageField != null)
		{
			AddToken(tokens, m_durationMod, "Duration", string.Empty, sorceressDamageField.m_duration);
			AddToken(tokens, m_damageMod, "Damage", string.Empty, sorceressDamageField.m_damage);
			AddToken(tokens, m_healingMod, "Healing", string.Empty, sorceressDamageField.m_healing);
			AddToken_EffectMod(tokens, m_onEnemyEffectOverride, "EffectOnEnemies", sorceressDamageField.m_effectOnEnemies);
			AddToken_EffectMod(tokens, m_onAllyEffectOverride, "EffectOnAllies", sorceressDamageField.m_effectOnAllies);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SorceressDamageField sorceressDamageField = GetTargetAbilityOnAbilityData(abilityData) as SorceressDamageField;
		bool isAbilityPresent = sorceressDamageField != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_shapeOverride, "[Ground Effect Shape]", isAbilityPresent, isAbilityPresent ? sorceressDamageField.m_shape : AbilityAreaShape.SingleSquare);
		desc += AbilityModHelper.GetSequencePrefabDesc(m_persistentSequencePrefabOverride, "[Persistent Sequence]");
		desc += AbilityModHelper.GetModPropertyDesc(m_durationMod, "[Duration]", isAbilityPresent, isAbilityPresent ? sorceressDamageField.m_duration : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? sorceressDamageField.m_damage : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_healingMod, "[Heal]", isAbilityPresent, isAbilityPresent ? sorceressDamageField.m_healing : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_onEnemyEffectOverride, "{ On Enemy Effect Override }", isAbilityPresent, isAbilityPresent ? sorceressDamageField.m_effectOnEnemies : null);
		return desc + AbilityModHelper.GetModPropertyDesc(m_onAllyEffectOverride, "{ On Ally Effect Override }", isAbilityPresent, isAbilityPresent ? sorceressDamageField.m_effectOnAllies : null);
	}
}
