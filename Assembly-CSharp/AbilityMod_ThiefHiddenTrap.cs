using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ThiefHiddenTrap : AbilityMod
{
	[Header("-- Trap Ground Field")]
	public AbilityModPropertyGroundEffectField m_trapFieldInfoMod;

	[Header("-- Extra Damage")]
	public AbilityModPropertyInt m_extraDamagePerTurnMod;

	public AbilityModPropertyInt m_maxExtraDamageMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ThiefHiddenTrap);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ThiefHiddenTrap thiefHiddenTrap = targetAbility as ThiefHiddenTrap;
		if (thiefHiddenTrap != null)
		{
			AbilityMod.AddToken_GroundFieldMod(tokens, this.m_trapFieldInfoMod, "TrapFieldInfo", thiefHiddenTrap.m_trapFieldInfo);
			AbilityMod.AddToken(tokens, this.m_extraDamagePerTurnMod, "ExtraDamagePerTurn", string.Empty, thiefHiddenTrap.m_extraDamagePerTurn, true, false);
			AbilityMod.AddToken(tokens, this.m_maxExtraDamageMod, "MaxExtraDamage", string.Empty, thiefHiddenTrap.m_maxExtraDamage, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefHiddenTrap thiefHiddenTrap = base.GetTargetAbilityOnAbilityData(abilityData) as ThiefHiddenTrap;
		bool flag = thiefHiddenTrap != null;
		string text = string.Empty;
		text += base.PropDescGroundFieldMod(this.m_trapFieldInfoMod, "{ TrapFieldInfo }", thiefHiddenTrap.m_trapFieldInfo);
		string str = text;
		AbilityModPropertyInt extraDamagePerTurnMod = this.m_extraDamagePerTurnMod;
		string prefix = "[ExtraDamagePerTurn]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = thiefHiddenTrap.m_extraDamagePerTurn;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(extraDamagePerTurnMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt maxExtraDamageMod = this.m_maxExtraDamageMod;
		string prefix2 = "[MaxExtraDamage]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = thiefHiddenTrap.m_maxExtraDamage;
		}
		else
		{
			baseVal2 = 0;
		}
		return str2 + base.PropDesc(maxExtraDamageMod, prefix2, showBaseVal2, baseVal2);
	}
}
