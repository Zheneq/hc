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
		if (!(sorceressDamageField != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_durationMod, "Duration", string.Empty, sorceressDamageField.m_duration);
			AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, sorceressDamageField.m_damage);
			AbilityMod.AddToken(tokens, m_healingMod, "Healing", string.Empty, sorceressDamageField.m_healing);
			AbilityMod.AddToken_EffectMod(tokens, m_onEnemyEffectOverride, "EffectOnEnemies", sorceressDamageField.m_effectOnEnemies);
			AbilityMod.AddToken_EffectMod(tokens, m_onAllyEffectOverride, "EffectOnAllies", sorceressDamageField.m_effectOnAllies);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SorceressDamageField sorceressDamageField = GetTargetAbilityOnAbilityData(abilityData) as SorceressDamageField;
		bool flag = sorceressDamageField != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyShape shapeOverride = m_shapeOverride;
		int baseVal;
		if (flag)
		{
			baseVal = (int)sorceressDamageField.m_shape;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(shapeOverride, "[Ground Effect Shape]", flag, (AbilityAreaShape)baseVal);
		empty += AbilityModHelper.GetSequencePrefabDesc(m_persistentSequencePrefabOverride, "[Persistent Sequence]");
		empty += AbilityModHelper.GetModPropertyDesc(m_durationMod, "[Duration]", flag, flag ? sorceressDamageField.m_duration : 0);
		empty += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", flag, flag ? sorceressDamageField.m_damage : 0);
		string str2 = empty;
		AbilityModPropertyInt healingMod = m_healingMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = sorceressDamageField.m_healing;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(healingMod, "[Heal]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyEffectInfo onEnemyEffectOverride = m_onEnemyEffectOverride;
		object baseVal3;
		if (flag)
		{
			baseVal3 = sorceressDamageField.m_effectOnEnemies;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(onEnemyEffectOverride, "{ On Enemy Effect Override }", flag, (StandardEffectInfo)baseVal3);
		return empty + AbilityModHelper.GetModPropertyDesc(m_onAllyEffectOverride, "{ On Ally Effect Override }", flag, (!flag) ? null : sorceressDamageField.m_effectOnAllies);
	}
}
