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
			AddToken_GroundFieldMod(tokens, m_trapFieldInfoMod, "TrapFieldInfo", thiefHiddenTrap.m_trapFieldInfo);
			AddToken(tokens, m_extraDamagePerTurnMod, "ExtraDamagePerTurn", string.Empty, thiefHiddenTrap.m_extraDamagePerTurn);
			AddToken(tokens, m_maxExtraDamageMod, "MaxExtraDamage", string.Empty, thiefHiddenTrap.m_maxExtraDamage);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefHiddenTrap thiefHiddenTrap = GetTargetAbilityOnAbilityData(abilityData) as ThiefHiddenTrap;
		bool isValid = thiefHiddenTrap != null;
		string desc = string.Empty;
		desc += PropDescGroundFieldMod(m_trapFieldInfoMod, "{ TrapFieldInfo }", thiefHiddenTrap.m_trapFieldInfo);
		desc += PropDesc(m_extraDamagePerTurnMod, "[ExtraDamagePerTurn]", isValid, isValid ? thiefHiddenTrap.m_extraDamagePerTurn : 0);
		return desc + PropDesc(m_maxExtraDamageMod, "[MaxExtraDamage]", isValid, isValid ? thiefHiddenTrap.m_maxExtraDamage : 0);
	}
}
