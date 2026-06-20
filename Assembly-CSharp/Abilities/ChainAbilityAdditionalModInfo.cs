// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

// identical in reactor and rogues
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
		if (ability == null)
		{
			return;
		}
		Ability[] chainAbilities = mod.m_useChainAbilityOverrides
			? mod.m_chainAbilityOverrides
			: ability.m_chainAbilities;

		Ability chainAbility = null;
		if (chainAbilities != null && chainAbilities.Length > m_chainAbilityIndex)
		{
			chainAbility = chainAbilities[m_chainAbilityIndex];
		}

		if (chainAbility != null)
		{
			string text = name + "_" + m_chainAbilityIndex;
			AbilityMod.AddToken_EffectInfo(entries, m_effectOnSelf, text + "_EffectOnSelf");
			AbilityMod.AddToken_EffectInfo(entries, m_effectOnAlly, text + "_EffectOnAlly");
			AbilityMod.AddToken_EffectInfo(entries, m_effectOnEnemy, text + "_EffectOnEnemy");
			if (m_cooldownReductionsOnSelf.HasCooldownReduction())
			{
				m_cooldownReductionsOnSelf.AddTooltipTokens(entries, text);
			}
		}
	}

	public string GetDescription(AbilityData abilityData, Ability ability, AbilityMod mod)
	{
		string desc = string.Empty;
		if (ability == null)
		{
			return desc;
		}
		
		Ability[] chainAbilities = mod.m_useChainAbilityOverrides
			? mod.m_chainAbilityOverrides
			: ability.m_chainAbilities;
		
		Ability chainAbility = null;
		if (chainAbilities != null && chainAbilities.Length > m_chainAbilityIndex)
		{
			chainAbility = chainAbilities[m_chainAbilityIndex];
		}
		
		if (chainAbility != null)
		{
			desc += "Additional Mod Info for Chain Ability at index " + m_chainAbilityIndex + ": " + chainAbility.GetDebugIdentifier("white") + "\n";
			desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnSelf, "{ ChainAbility Effect on Self }", "        ");
			desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnAlly, "{ ChainAbility Effect on Ally }", "        ");
			desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnEnemy, "{ ChainAbility Effect on Enemy }", "        ");
			if (m_cooldownReductionsOnSelf.HasCooldownReduction())
			{
				desc += "Chain Ability Cooldown Reductions:\n";
				desc += m_cooldownReductionsOnSelf.GetDescription(abilityData);
			}
		}
		else
		{
			desc += "No Chain Ability at index " + m_chainAbilityIndex + ", ignoring chain ability mod info\n";
		}
		return desc;
	}
}
