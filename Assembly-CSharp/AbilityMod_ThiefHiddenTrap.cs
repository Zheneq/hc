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
		if (!(thiefHiddenTrap != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_GroundFieldMod(tokens, m_trapFieldInfoMod, "TrapFieldInfo", thiefHiddenTrap.m_trapFieldInfo);
			AbilityMod.AddToken(tokens, m_extraDamagePerTurnMod, "ExtraDamagePerTurn", string.Empty, thiefHiddenTrap.m_extraDamagePerTurn);
			AbilityMod.AddToken(tokens, m_maxExtraDamageMod, "MaxExtraDamage", string.Empty, thiefHiddenTrap.m_maxExtraDamage);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefHiddenTrap thiefHiddenTrap = GetTargetAbilityOnAbilityData(abilityData) as ThiefHiddenTrap;
		bool flag = thiefHiddenTrap != null;
		string empty = string.Empty;
		empty += PropDescGroundFieldMod(m_trapFieldInfoMod, "{ TrapFieldInfo }", thiefHiddenTrap.m_trapFieldInfo);
		string str = empty;
		AbilityModPropertyInt extraDamagePerTurnMod = m_extraDamagePerTurnMod;
		int baseVal;
		if (flag)
		{
			baseVal = thiefHiddenTrap.m_extraDamagePerTurn;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(extraDamagePerTurnMod, "[ExtraDamagePerTurn]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt maxExtraDamageMod = m_maxExtraDamageMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = thiefHiddenTrap.m_maxExtraDamage;
		}
		else
		{
			baseVal2 = 0;
		}
		return str2 + PropDesc(maxExtraDamageMod, "[MaxExtraDamage]", flag, baseVal2);
	}
}
