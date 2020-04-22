using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChainAbilityAdditionalModInfo
{
	[Header("-- 0 based index of chain ability as it appears in main ability's list")]
	public int m_chainAbilityIndex;

	[Header("-- Effects")]
	public StandardEffectInfo m_effectOnSelf;

	public StandardEffectInfo m_effectOnAlly;

	public StandardEffectInfo m_effectOnEnemy;

	[Header("-- For Cooldown Reductions")]
	public AbilityModCooldownReduction m_cooldownReductionsOnSelf;

	[Header("-- Sequence for Timing (for self hit if not already hitting)")]
	public GameObject m_timingSequencePrefab;

	public void AddTooltipTokens(List<TooltipTokenEntry> entries, Ability ability, AbilityMod mod, string name)
	{
		if (!(ability != null))
		{
			return;
		}
		Ability[] array = ability.m_chainAbilities;
		if (mod.m_useChainAbilityOverrides)
		{
			array = mod.m_chainAbilityOverrides;
		}
		Ability x = null;
		if (array != null)
		{
			if (array.Length > m_chainAbilityIndex)
			{
				x = array[m_chainAbilityIndex];
			}
		}
		if (!(x != null))
		{
			return;
		}
		while (true)
		{
			string text = name + "_" + m_chainAbilityIndex;
			AbilityMod.AddToken_EffectInfo(entries, m_effectOnSelf, text + "_EffectOnSelf");
			AbilityMod.AddToken_EffectInfo(entries, m_effectOnAlly, text + "_EffectOnAlly");
			AbilityMod.AddToken_EffectInfo(entries, m_effectOnEnemy, text + "_EffectOnEnemy");
			if (m_cooldownReductionsOnSelf.HasCooldownReduction())
			{
				while (true)
				{
					m_cooldownReductionsOnSelf.AddTooltipTokens(entries, text);
					return;
				}
			}
			return;
		}
	}

	public string GetDescription(AbilityData abilityData, Ability ability, AbilityMod mod)
	{
		string text = string.Empty;
		if (ability != null)
		{
			Ability[] array = ability.m_chainAbilities;
			if (mod.m_useChainAbilityOverrides)
			{
				array = mod.m_chainAbilityOverrides;
			}
			Ability ability2 = null;
			if (array != null)
			{
				if (array.Length > m_chainAbilityIndex)
				{
					ability2 = array[m_chainAbilityIndex];
				}
			}
			if (ability2 != null)
			{
				string text2 = text;
				text = text2 + "Additional Mod Info for Chain Ability at index " + m_chainAbilityIndex + ": " + ability2.GetDebugIdentifier("white") + "\n";
				text += AbilityModHelper.GetModEffectInfoDesc(m_effectOnSelf, "{ ChainAbility Effect on Self }", "        ");
				text += AbilityModHelper.GetModEffectInfoDesc(m_effectOnAlly, "{ ChainAbility Effect on Ally }", "        ");
				text += AbilityModHelper.GetModEffectInfoDesc(m_effectOnEnemy, "{ ChainAbility Effect on Enemy }", "        ");
				if (m_cooldownReductionsOnSelf.HasCooldownReduction())
				{
					text += "Chain Ability Cooldown Reductions:\n";
					text += m_cooldownReductionsOnSelf.GetDescription(abilityData);
				}
			}
			else
			{
				string text2 = text;
				text = text2 + "No Chain Ability at index " + m_chainAbilityIndex + ", ignoring chain ability mod info\n";
			}
		}
		return text;
	}
}
