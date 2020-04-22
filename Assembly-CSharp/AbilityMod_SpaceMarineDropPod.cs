using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SpaceMarineDropPod : AbilityMod
{
	[Header("-- Damage and Knockback Distance")]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyFloat m_knockbackDistanceMod;

	[Header("-- Energy Refund if no hit")]
	public AbilityModPropertyInt m_energyRefundIfNoEnemyHitMod;

	[Header(" -- Power Up Heal and Energy Mod")]
	public AbilityModPropertyInt m_powerupHealMod;

	public AbilityModPropertyInt m_powerupTechPointMod;

	[Header("-- Ground Effect on Drop Pod")]
	public StandardGroundEffectInfo m_groundEffectInfoOnDropPod;

	[Space(10f)]
	public AbilityModPropertyInt m_extraPowerupHealIfDirectHitMod;

	public AbilityModPropertyInt m_extraPowerupEnergyIfDirectHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SpaceMarineDropPod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SpaceMarineDropPod spaceMarineDropPod = targetAbility as SpaceMarineDropPod;
		if (!(spaceMarineDropPod != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, spaceMarineDropPod.m_damageAmount);
			AbilityMod.AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", string.Empty, spaceMarineDropPod.m_knockbackDistance);
			AbilityMod.AddToken(tokens, m_energyRefundIfNoEnemyHitMod, "EnergyRefundIfNoEnemyHit", string.Empty, spaceMarineDropPod.m_energyRefundIfNoEnemyHit);
			AbilityMod.AddToken(tokens, m_extraPowerupHealIfDirectHitMod, "ExtraPowerupHealIfDirectHit", string.Empty, spaceMarineDropPod.m_extraPowerupHealIfDirectHit);
			AbilityMod.AddToken(tokens, m_extraPowerupEnergyIfDirectHitMod, "ExtraPowerupEnergy", string.Empty, spaceMarineDropPod.m_extraPowerupEnergyIfDirectHit);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SpaceMarineDropPod spaceMarineDropPod = GetTargetAbilityOnAbilityData(abilityData) as SpaceMarineDropPod;
		bool flag = spaceMarineDropPod != null;
		int num = 0;
		int num2 = 0;
		if (flag)
		{
			while (true)
			{
				switch (1)
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
			if (spaceMarineDropPod.m_powerupPrefab != null)
			{
				PowerUp_Standard_Ability component = spaceMarineDropPod.m_powerupPrefab.GetComponent<PowerUp_Standard_Ability>();
				if (component != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					num = component.m_healAmount;
					num2 = component.m_techPointsAmount;
				}
			}
		}
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal = spaceMarineDropPod.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(damageMod, "[Damage]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat knockbackDistanceMod = m_knockbackDistanceMod;
		float baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = spaceMarineDropPod.m_knockbackDistance;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(knockbackDistanceMod, "[Knockback Distance]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt energyRefundIfNoEnemyHitMod = m_energyRefundIfNoEnemyHitMod;
		int baseVal3;
		if (flag)
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
			baseVal3 = spaceMarineDropPod.m_energyRefundIfNoEnemyHit;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(energyRefundIfNoEnemyHitMod, "[EnergyRefundIfNoEnemyHit]", flag, baseVal3);
		empty += AbilityModHelper.GetModPropertyDesc(m_powerupHealMod, "[PowerUp Heal Amount]", flag, flag ? num : 0);
		string str4 = empty;
		AbilityModPropertyInt powerupTechPointMod = m_powerupTechPointMod;
		int baseVal4;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = num2;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(powerupTechPointMod, "[PowerUp TechPoint Amount]", flag, baseVal4);
		empty += AbilityModHelper.GetModGroundEffectInfoDesc(m_groundEffectInfoOnDropPod, "-- Ground Effect on Drop Pod Location --", flag);
		string str5 = empty;
		AbilityModPropertyInt extraPowerupHealIfDirectHitMod = m_extraPowerupHealIfDirectHitMod;
		int baseVal5;
		if (flag)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = spaceMarineDropPod.m_extraPowerupHealIfDirectHit;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(extraPowerupHealIfDirectHitMod, "[ExtraPowerupHealIfDirectHit]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt extraPowerupEnergyIfDirectHitMod = m_extraPowerupEnergyIfDirectHitMod;
		int baseVal6;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal6 = spaceMarineDropPod.m_extraPowerupEnergyIfDirectHit;
		}
		else
		{
			baseVal6 = 0;
		}
		return str6 + PropDesc(extraPowerupEnergyIfDirectHitMod, "[ExtraPowerupEnergy]", flag, baseVal6);
	}
}
