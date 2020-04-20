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
		if (ability != null)
		{
			Ability[] array = ability.m_chainAbilities;
			if (mod.m_useChainAbilityOverrides)
			{
				array = mod.m_chainAbilityOverrides;
			}
			Ability x = null;
			if (array != null)
			{
				if (array.Length > this.m_chainAbilityIndex)
				{
					x = array[this.m_chainAbilityIndex];
				}
			}
			if (x != null)
			{
				string text = name + "_" + this.m_chainAbilityIndex.ToString();
				AbilityMod.AddToken_EffectInfo(entries, this.m_effectOnSelf, text + "_EffectOnSelf", null, true);
				AbilityMod.AddToken_EffectInfo(entries, this.m_effectOnAlly, text + "_EffectOnAlly", null, true);
				AbilityMod.AddToken_EffectInfo(entries, this.m_effectOnEnemy, text + "_EffectOnEnemy", null, true);
				if (this.m_cooldownReductionsOnSelf.HasCooldownReduction())
				{
					this.m_cooldownReductionsOnSelf.AddTooltipTokens(entries, text);
				}
			}
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
				if (array.Length > this.m_chainAbilityIndex)
				{
					ability2 = array[this.m_chainAbilityIndex];
				}
			}
			if (ability2 != null)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"Additional Mod Info for Chain Ability at index ",
					this.m_chainAbilityIndex,
					": ",
					ability2.GetDebugIdentifier("white"),
					"\n"
				});
				text += AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnSelf, "{ ChainAbility Effect on Self }", "        ", false, null);
				text += AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnAlly, "{ ChainAbility Effect on Ally }", "        ", false, null);
				text += AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnEnemy, "{ ChainAbility Effect on Enemy }", "        ", false, null);
				if (this.m_cooldownReductionsOnSelf.HasCooldownReduction())
				{
					text += "Chain Ability Cooldown Reductions:\n";
					text += this.m_cooldownReductionsOnSelf.GetDescription(abilityData);
				}
			}
			else
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"No Chain Ability at index ",
					this.m_chainAbilityIndex,
					", ignoring chain ability mod info\n"
				});
			}
		}
		return text;
	}
}
