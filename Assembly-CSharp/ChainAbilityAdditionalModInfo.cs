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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ChainAbilityAdditionalModInfo.AddTooltipTokens(List<TooltipTokenEntry>, Ability, AbilityMod, string)).MethodHandle;
				}
				array = mod.m_chainAbilityOverrides;
			}
			Ability x = null;
			if (array != null)
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
				if (array.Length > this.m_chainAbilityIndex)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					x = array[this.m_chainAbilityIndex];
				}
			}
			if (x != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				string text = name + "_" + this.m_chainAbilityIndex.ToString();
				AbilityMod.AddToken_EffectInfo(entries, this.m_effectOnSelf, text + "_EffectOnSelf", null, true);
				AbilityMod.AddToken_EffectInfo(entries, this.m_effectOnAlly, text + "_EffectOnAlly", null, true);
				AbilityMod.AddToken_EffectInfo(entries, this.m_effectOnEnemy, text + "_EffectOnEnemy", null, true);
				if (this.m_cooldownReductionsOnSelf.HasCooldownReduction())
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ChainAbilityAdditionalModInfo.GetDescription(AbilityData, Ability, AbilityMod)).MethodHandle;
			}
			Ability[] array = ability.m_chainAbilities;
			if (mod.m_useChainAbilityOverrides)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				array = mod.m_chainAbilityOverrides;
			}
			Ability ability2 = null;
			if (array != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (array.Length > this.m_chainAbilityIndex)
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
					ability2 = array[this.m_chainAbilityIndex];
				}
			}
			if (ability2 != null)
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
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
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
