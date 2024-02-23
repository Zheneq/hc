using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_RobotAnimalStealth : AbilityMod
{
	[Header("-- Effect on Next Damage Attack")]
	public StandardEffectInfo m_effectOnNextDamageAttack;
	[Header("-- Extra Damage on Next Attack")]
	public int m_extraDamageNextAttack;
	[Header("-- Cooldown Mod")]
	public AbilityCooldownMod m_cooldownModOnCast;
	[Header("-- Override for the Stealth Effect")]
	public StandardEffectInfo m_selfEffectOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(RobotAnimalStealth);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RobotAnimalStealth x = targetAbility as RobotAnimalStealth;
		// broken code in reactor
		if (x == null)
		{
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalStealth robotAnimalStealth = GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalStealth;
		bool isAbilityPresent = robotAnimalStealth != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnNextDamageAttack, "{ Effect on Next Damage Attack }", string.Empty, isAbilityPresent);
		desc += AbilityModHelper.GetCooldownModDesc(m_cooldownModOnCast, "[Cooldown] on Cast", abilityData);
		if (m_extraDamageNextAttack != 0)
		{
			desc += new StringBuilder().Append("[Extra Damage on Next Attack] = ").Append(m_extraDamageNextAttack).Append("\n").ToString();
		}
		if (m_selfEffectOverride != null && m_selfEffectOverride.m_applyEffect)
		{
			desc += AbilityModHelper.GetModEffectDataDesc(m_selfEffectOverride.m_effectData, "{ Override for the Stealth Effect }", string.Empty, isAbilityPresent, robotAnimalStealth != null ? robotAnimalStealth.m_selfEffect : null);
		}
		return desc;
	}
}
