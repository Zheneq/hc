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
		if (battleMonkBuffCharge_Prep != null)
		{
			AddToken(tokens, m_allySelectRadiusMod, "AllySelectRadius", string.Empty, battleMonkBuffCharge_Prep.m_allySelectRadius);
			BattleMonkBuffCharge_Dash battleMonkBuffCharge_Dash = null;
			if (battleMonkBuffCharge_Prep != null)
			{
				if (battleMonkBuffCharge_Prep.m_chainAbilities.Length > 0)
				{
					foreach (Ability ability in battleMonkBuffCharge_Prep.m_chainAbilities)
					{
						BattleMonkBuffCharge_Dash dash = ability as BattleMonkBuffCharge_Dash;
						if (ability != null && !ReferenceEquals(dash, null))
						{
							battleMonkBuffCharge_Dash = dash;
							break;
						}
					}
				}
				if (battleMonkBuffCharge_Dash != null)
				{
					AddToken(tokens, m_damageMod, "Damage", string.Empty, battleMonkBuffCharge_Dash.m_damage);
					AddToken_EffectMod(tokens, m_allyEffectOverride, "AllyBuff", battleMonkBuffCharge_Prep.m_allyBuff);
					AddToken_EffectMod(tokens, m_selfEffectOverride, "SelfBuff", battleMonkBuffCharge_Prep.m_selfBuff);
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BattleMonkBuffCharge_Prep battleMonkBuffCharge_Prep = GetTargetAbilityOnAbilityData(abilityData) as BattleMonkBuffCharge_Prep;
		BattleMonkBuffCharge_Dash battleMonkBuffCharge_Dash = null;
		if (battleMonkBuffCharge_Prep != null && battleMonkBuffCharge_Prep.m_chainAbilities.Length > 0)
		{
			foreach (Ability ability in battleMonkBuffCharge_Prep.m_chainAbilities)
			{
				BattleMonkBuffCharge_Dash dash = ability as BattleMonkBuffCharge_Dash;
				if (ability != null && !ReferenceEquals(dash, null))
				{
					battleMonkBuffCharge_Dash = dash;
					break;
				}
			}
		}

		bool isAbilityPresent = battleMonkBuffCharge_Prep != null && battleMonkBuffCharge_Dash != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_requireHitAlliesMod, "[Require Hit Ally?]", isAbilityPresent, isAbilityPresent && battleMonkBuffCharge_Prep.m_mustHitAllies);
		desc += PropDesc(m_allySelectRadiusMod, "[AllySelectRadius]", isAbilityPresent, isAbilityPresent ? battleMonkBuffCharge_Prep.m_allySelectRadius : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_allyShapeMod, "[Ally Hit Shape]", isAbilityPresent, isAbilityPresent ? battleMonkBuffCharge_Prep.m_buffAlliesShape : AbilityAreaShape.SingleSquare);
		desc += AbilityModHelper.GetModPropertyDesc(m_enemyShapeMod, "[Enemy Hit Shape]", isAbilityPresent, isAbilityPresent ? battleMonkBuffCharge_Dash.m_damageEnemiesShape : AbilityAreaShape.SingleSquare);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? battleMonkBuffCharge_Dash.m_damage : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_selfEffectOverride, "{ Self Buff Effect Override }", isAbilityPresent, isAbilityPresent ? battleMonkBuffCharge_Prep.m_selfBuff : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_allyEffectOverride, "{ Ally Buff Effect Override }", isAbilityPresent, isAbilityPresent ? battleMonkBuffCharge_Prep.m_allyBuff : null);
		if (m_removeAllNegativeStatusFromAllies)
		{
			desc += "[Removes All Negative Status From Targeted Allies]\n";
		}
		return desc;
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		if (abilityAsBase == null || abilityAsBase.GetType() != GetTargetAbilityType())
		{
			return;
		}
		BattleMonkBuffCharge_Prep battleMonkBuffCharge_Prep = abilityAsBase as BattleMonkBuffCharge_Prep;
		BattleMonkBuffCharge_Dash battleMonkBuffCharge_Dash = null;
		if (battleMonkBuffCharge_Prep.m_chainAbilities.Length > 0)
		{
			foreach (Ability ability in battleMonkBuffCharge_Prep.m_chainAbilities)
			{
				BattleMonkBuffCharge_Dash dash = ability as BattleMonkBuffCharge_Dash;
				if (ability != null && !ReferenceEquals(dash, null))
				{
					battleMonkBuffCharge_Dash = dash;
					break;
				}
			}
		}

		if (battleMonkBuffCharge_Dash != null)
		{
			numbers.Add(m_damageMod.GetModifiedValue(battleMonkBuffCharge_Dash.m_damage));
		}
	}
}
