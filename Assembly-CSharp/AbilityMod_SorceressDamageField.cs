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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SorceressDamageField.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_durationMod, "Duration", string.Empty, sorceressDamageField.m_duration, true, false);
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, sorceressDamageField.m_damage, true, false);
			AbilityMod.AddToken(tokens, this.m_healingMod, "Healing", string.Empty, sorceressDamageField.m_healing, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_onEnemyEffectOverride, "EffectOnEnemies", sorceressDamageField.m_effectOnEnemies, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_onAllyEffectOverride, "EffectOnAllies", sorceressDamageField.m_effectOnAllies, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SorceressDamageField sorceressDamageField = base.GetTargetAbilityOnAbilityData(abilityData) as SorceressDamageField;
		bool flag = sorceressDamageField != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyShape shapeOverride = this.m_shapeOverride;
		string prefix = "[Ground Effect Shape]";
		bool showBaseVal = flag;
		AbilityAreaShape baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SorceressDamageField.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = sorceressDamageField.m_shape;
		}
		else
		{
			baseVal = AbilityAreaShape.SingleSquare;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(shapeOverride, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetSequencePrefabDesc(this.m_persistentSequencePrefabOverride, "[Persistent Sequence]");
		text += AbilityModHelper.GetModPropertyDesc(this.m_durationMod, "[Duration]", flag, (!flag) ? 0 : sorceressDamageField.m_duration);
		text += AbilityModHelper.GetModPropertyDesc(this.m_damageMod, "[Damage]", flag, (!flag) ? 0 : sorceressDamageField.m_damage);
		string str2 = text;
		AbilityModPropertyInt healingMod = this.m_healingMod;
		string prefix2 = "[Heal]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = sorceressDamageField.m_healing;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(healingMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyEffectInfo onEnemyEffectOverride = this.m_onEnemyEffectOverride;
		string prefix3 = "{ On Enemy Effect Override }";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = sorceressDamageField.m_effectOnEnemies;
		}
		else
		{
			baseVal3 = null;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(onEnemyEffectOverride, prefix3, showBaseVal3, baseVal3);
		return text + AbilityModHelper.GetModPropertyDesc(this.m_onAllyEffectOverride, "{ On Ally Effect Override }", flag, (!flag) ? null : sorceressDamageField.m_effectOnAllies);
	}
}
