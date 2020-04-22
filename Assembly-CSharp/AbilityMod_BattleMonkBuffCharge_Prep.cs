using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BattleMonkBuffCharge_Prep : AbilityMod
{
	[Header("-- Ability Targeting Mod")]
	public AbilityModPropertyBool m_requireHitAlliesMod;

	public AbilityModPropertyFloat m_allySelectRadiusMod;

	public AbilityModPropertyShape m_allyShapeMod;

	public AbilityModPropertyShape m_enemyShapeMod;

	[Header("-- Damage Mod")]
	public AbilityModPropertyInt m_damageMod;

	[Header("-- Self Effect Overrides")]
	public AbilityModPropertyEffectInfo m_selfEffectOverride;

	[Header("-- Ally Effect Override")]
	public AbilityModPropertyEffectInfo m_allyEffectOverride;

	[Header("-- Remove Debuffs From Targeted Allies")]
	public bool m_removeAllNegativeStatusFromAllies;

	public override Type GetTargetAbilityType()
	{
		return typeof(BattleMonkBuffCharge_Prep);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BattleMonkBuffCharge_Prep battleMonkBuffCharge_Prep = targetAbility as BattleMonkBuffCharge_Prep;
		if (!(battleMonkBuffCharge_Prep != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_allySelectRadiusMod, "AllySelectRadius", string.Empty, battleMonkBuffCharge_Prep.m_allySelectRadius);
			BattleMonkBuffCharge_Dash battleMonkBuffCharge_Dash = null;
			if (!(battleMonkBuffCharge_Prep != null))
			{
				return;
			}
			while (true)
			{
				if (battleMonkBuffCharge_Prep.m_chainAbilities.Length > 0)
				{
					Ability[] chainAbilities = battleMonkBuffCharge_Prep.m_chainAbilities;
					foreach (Ability ability in chainAbilities)
					{
						if (ability != null && ability is BattleMonkBuffCharge_Dash)
						{
							battleMonkBuffCharge_Dash = (ability as BattleMonkBuffCharge_Dash);
							break;
						}
					}
				}
				if (battleMonkBuffCharge_Dash != null)
				{
					while (true)
					{
						AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, battleMonkBuffCharge_Dash.m_damage);
						AbilityMod.AddToken_EffectMod(tokens, m_allyEffectOverride, "AllyBuff", battleMonkBuffCharge_Prep.m_allyBuff);
						AbilityMod.AddToken_EffectMod(tokens, m_selfEffectOverride, "SelfBuff", battleMonkBuffCharge_Prep.m_selfBuff);
						return;
					}
				}
				return;
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BattleMonkBuffCharge_Prep battleMonkBuffCharge_Prep = GetTargetAbilityOnAbilityData(abilityData) as BattleMonkBuffCharge_Prep;
		BattleMonkBuffCharge_Dash battleMonkBuffCharge_Dash = null;
		if (battleMonkBuffCharge_Prep != null && battleMonkBuffCharge_Prep.m_chainAbilities.Length > 0)
		{
			Ability[] chainAbilities = battleMonkBuffCharge_Prep.m_chainAbilities;
			int num = 0;
			while (true)
			{
				if (num < chainAbilities.Length)
				{
					Ability ability = chainAbilities[num];
					if (ability != null)
					{
						if (ability is BattleMonkBuffCharge_Dash)
						{
							battleMonkBuffCharge_Dash = (ability as BattleMonkBuffCharge_Dash);
							break;
						}
					}
					num++;
					continue;
				}
				break;
			}
		}
		int num2;
		if (battleMonkBuffCharge_Prep != null)
		{
			num2 = ((battleMonkBuffCharge_Dash != null) ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool flag = (byte)num2 != 0;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool requireHitAlliesMod = m_requireHitAlliesMod;
		int baseVal;
		if (flag)
		{
			baseVal = (battleMonkBuffCharge_Prep.m_mustHitAllies ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(requireHitAlliesMod, "[Require Hit Ally?]", flag, (byte)baseVal != 0);
		empty += PropDesc(m_allySelectRadiusMod, "[AllySelectRadius]", flag, (!flag) ? 0f : battleMonkBuffCharge_Prep.m_allySelectRadius);
		string str2 = empty;
		AbilityModPropertyShape allyShapeMod = m_allyShapeMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = (int)battleMonkBuffCharge_Prep.m_buffAlliesShape;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(allyShapeMod, "[Ally Hit Shape]", flag, (AbilityAreaShape)baseVal2);
		string str3 = empty;
		AbilityModPropertyShape enemyShapeMod = m_enemyShapeMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = (int)battleMonkBuffCharge_Dash.m_damageEnemiesShape;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(enemyShapeMod, "[Enemy Hit Shape]", flag, (AbilityAreaShape)baseVal3);
		string str4 = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = battleMonkBuffCharge_Dash.m_damage;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(damageMod, "[Damage]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectInfo selfEffectOverride = m_selfEffectOverride;
		object baseVal5;
		if (flag)
		{
			baseVal5 = battleMonkBuffCharge_Prep.m_selfBuff;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(selfEffectOverride, "{ Self Buff Effect Override }", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyEffectInfo allyEffectOverride = m_allyEffectOverride;
		object baseVal6;
		if (flag)
		{
			baseVal6 = battleMonkBuffCharge_Prep.m_allyBuff;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + AbilityModHelper.GetModPropertyDesc(allyEffectOverride, "{ Ally Buff Effect Override }", flag, (StandardEffectInfo)baseVal6);
		if (m_removeAllNegativeStatusFromAllies)
		{
			empty += "[Removes All Negative Status From Targeted Allies]\n";
		}
		return empty;
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		if (!(abilityAsBase != null))
		{
			return;
		}
		while (true)
		{
			if (abilityAsBase.GetType() != GetTargetAbilityType())
			{
				return;
			}
			while (true)
			{
				BattleMonkBuffCharge_Prep battleMonkBuffCharge_Prep = abilityAsBase as BattleMonkBuffCharge_Prep;
				BattleMonkBuffCharge_Dash battleMonkBuffCharge_Dash = null;
				if (battleMonkBuffCharge_Prep.m_chainAbilities.Length > 0)
				{
					Ability[] chainAbilities = battleMonkBuffCharge_Prep.m_chainAbilities;
					foreach (Ability ability in chainAbilities)
					{
						if (!(ability != null))
						{
							continue;
						}
						if (ability is BattleMonkBuffCharge_Dash)
						{
							battleMonkBuffCharge_Dash = (ability as BattleMonkBuffCharge_Dash);
							break;
						}
					}
				}
				if (battleMonkBuffCharge_Dash != null)
				{
					while (true)
					{
						numbers.Add(m_damageMod.GetModifiedValue(battleMonkBuffCharge_Dash.m_damage));
						return;
					}
				}
				return;
			}
		}
	}
}
