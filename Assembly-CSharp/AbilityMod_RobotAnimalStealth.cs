using System;
using System.Collections.Generic;
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
		if (!(x != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalStealth robotAnimalStealth = GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalStealth;
		bool useBaseVal = robotAnimalStealth != null;
		string empty = string.Empty;
		empty += AbilityModHelper.GetModEffectInfoDesc(m_effectOnNextDamageAttack, "{ Effect on Next Damage Attack }", string.Empty, useBaseVal);
		empty += AbilityModHelper.GetCooldownModDesc(m_cooldownModOnCast, "[Cooldown] on Cast", abilityData);
		if (m_extraDamageNextAttack != 0)
		{
			string text = empty;
			empty = text + "[Extra Damage on Next Attack] = " + m_extraDamageNextAttack + "\n";
		}
		if (m_selfEffectOverride != null && m_selfEffectOverride.m_applyEffect)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			string str = empty;
			StandardActorEffectData effectData = m_selfEffectOverride.m_effectData;
			string empty2 = string.Empty;
			object baseVal;
			if (robotAnimalStealth != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				baseVal = robotAnimalStealth.m_selfEffect;
			}
			else
			{
				baseVal = null;
			}
			empty = str + AbilityModHelper.GetModEffectDataDesc(effectData, "{ Override for the Stealth Effect }", empty2, useBaseVal, (StandardActorEffectData)baseVal);
		}
		return empty;
	}
}
