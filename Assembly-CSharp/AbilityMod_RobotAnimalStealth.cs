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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RobotAnimalStealth.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalStealth robotAnimalStealth = base.GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalStealth;
		bool flag = robotAnimalStealth != null;
		string text = string.Empty;
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnNextDamageAttack, "{ Effect on Next Damage Attack }", string.Empty, flag, null);
		text += AbilityModHelper.GetCooldownModDesc(this.m_cooldownModOnCast, "[Cooldown] on Cast", abilityData);
		if (this.m_extraDamageNextAttack != 0)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Extra Damage on Next Attack] = ",
				this.m_extraDamageNextAttack,
				"\n"
			});
		}
		if (this.m_selfEffectOverride != null && this.m_selfEffectOverride.m_applyEffect)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RobotAnimalStealth.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			string str = text;
			StandardActorEffectData effectData = this.m_selfEffectOverride.m_effectData;
			string prefix = "{ Override for the Stealth Effect }";
			string empty = string.Empty;
			bool useBaseVal = flag;
			StandardActorEffectData baseVal;
			if (robotAnimalStealth != null)
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
				baseVal = robotAnimalStealth.m_selfEffect;
			}
			else
			{
				baseVal = null;
			}
			text = str + AbilityModHelper.GetModEffectDataDesc(effectData, prefix, empty, useBaseVal, baseVal);
		}
		return text;
	}
}
