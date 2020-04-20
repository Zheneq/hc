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
			AbilityMod.AddToken(tokens, this.m_allySelectRadiusMod, "AllySelectRadius", string.Empty, battleMonkBuffCharge_Prep.m_allySelectRadius, true, false, false);
			BattleMonkBuffCharge_Dash battleMonkBuffCharge_Dash = null;
			if (battleMonkBuffCharge_Prep != null)
			{
				if (battleMonkBuffCharge_Prep.m_chainAbilities.Length > 0)
				{
					foreach (Ability ability in battleMonkBuffCharge_Prep.m_chainAbilities)
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
					AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, battleMonkBuffCharge_Dash.m_damage, true, false);
					AbilityMod.AddToken_EffectMod(tokens, this.m_allyEffectOverride, "AllyBuff", battleMonkBuffCharge_Prep.m_allyBuff, true);
					AbilityMod.AddToken_EffectMod(tokens, this.m_selfEffectOverride, "SelfBuff", battleMonkBuffCharge_Prep.m_selfBuff, true);
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BattleMonkBuffCharge_Prep battleMonkBuffCharge_Prep = base.GetTargetAbilityOnAbilityData(abilityData) as BattleMonkBuffCharge_Prep;
		BattleMonkBuffCharge_Dash battleMonkBuffCharge_Dash = null;
		if (battleMonkBuffCharge_Prep != null && battleMonkBuffCharge_Prep.m_chainAbilities.Length > 0)
		{
			foreach (Ability ability in battleMonkBuffCharge_Prep.m_chainAbilities)
			{
				if (ability != null)
				{
					if (ability is BattleMonkBuffCharge_Dash)
					{
						battleMonkBuffCharge_Dash = (ability as BattleMonkBuffCharge_Dash);
						goto IL_93;
					}
				}
			}
		}
		IL_93:
		bool flag;
		if (battleMonkBuffCharge_Prep != null)
		{
			flag = (battleMonkBuffCharge_Dash != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool requireHitAlliesMod = this.m_requireHitAlliesMod;
		string prefix = "[Require Hit Ally?]";
		bool showBaseVal = flag2;
		bool baseVal;
		if (flag2)
		{
			baseVal = battleMonkBuffCharge_Prep.m_mustHitAllies;
		}
		else
		{
			baseVal = false;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(requireHitAlliesMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_allySelectRadiusMod, "[AllySelectRadius]", flag2, (!flag2) ? 0f : battleMonkBuffCharge_Prep.m_allySelectRadius);
		string str2 = text;
		AbilityModPropertyShape allyShapeMod = this.m_allyShapeMod;
		string prefix2 = "[Ally Hit Shape]";
		bool showBaseVal2 = flag2;
		AbilityAreaShape baseVal2;
		if (flag2)
		{
			baseVal2 = battleMonkBuffCharge_Prep.m_buffAlliesShape;
		}
		else
		{
			baseVal2 = AbilityAreaShape.SingleSquare;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(allyShapeMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyShape enemyShapeMod = this.m_enemyShapeMod;
		string prefix3 = "[Enemy Hit Shape]";
		bool showBaseVal3 = flag2;
		AbilityAreaShape baseVal3;
		if (flag2)
		{
			baseVal3 = battleMonkBuffCharge_Dash.m_damageEnemiesShape;
		}
		else
		{
			baseVal3 = AbilityAreaShape.SingleSquare;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(enemyShapeMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix4 = "[Damage]";
		bool showBaseVal4 = flag2;
		int baseVal4;
		if (flag2)
		{
			baseVal4 = battleMonkBuffCharge_Dash.m_damage;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(damageMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectInfo selfEffectOverride = this.m_selfEffectOverride;
		string prefix5 = "{ Self Buff Effect Override }";
		bool showBaseVal5 = flag2;
		StandardEffectInfo baseVal5;
		if (flag2)
		{
			baseVal5 = battleMonkBuffCharge_Prep.m_selfBuff;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(selfEffectOverride, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo allyEffectOverride = this.m_allyEffectOverride;
		string prefix6 = "{ Ally Buff Effect Override }";
		bool showBaseVal6 = flag2;
		StandardEffectInfo baseVal6;
		if (flag2)
		{
			baseVal6 = battleMonkBuffCharge_Prep.m_allyBuff;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + AbilityModHelper.GetModPropertyDesc(allyEffectOverride, prefix6, showBaseVal6, baseVal6);
		if (this.m_removeAllNegativeStatusFromAllies)
		{
			text += "[Removes All Negative Status From Targeted Allies]\n";
		}
		return text;
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		if (abilityAsBase != null)
		{
			if (abilityAsBase.GetType() == this.GetTargetAbilityType())
			{
				BattleMonkBuffCharge_Prep battleMonkBuffCharge_Prep = abilityAsBase as BattleMonkBuffCharge_Prep;
				BattleMonkBuffCharge_Dash battleMonkBuffCharge_Dash = null;
				if (battleMonkBuffCharge_Prep.m_chainAbilities.Length > 0)
				{
					foreach (Ability ability in battleMonkBuffCharge_Prep.m_chainAbilities)
					{
						if (ability != null)
						{
							if (ability is BattleMonkBuffCharge_Dash)
							{
								battleMonkBuffCharge_Dash = (ability as BattleMonkBuffCharge_Dash);
								break;
							}
						}
					}
				}
				if (battleMonkBuffCharge_Dash != null)
				{
					numbers.Add(this.m_damageMod.GetModifiedValue(battleMonkBuffCharge_Dash.m_damage));
				}
			}
		}
	}
}
