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
		if (spaceMarineDropPod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SpaceMarineDropPod.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_damageMod, "DamageAmount", string.Empty, spaceMarineDropPod.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_knockbackDistanceMod, "KnockbackDistance", string.Empty, spaceMarineDropPod.m_knockbackDistance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_energyRefundIfNoEnemyHitMod, "EnergyRefundIfNoEnemyHit", string.Empty, spaceMarineDropPod.m_energyRefundIfNoEnemyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_extraPowerupHealIfDirectHitMod, "ExtraPowerupHealIfDirectHit", string.Empty, spaceMarineDropPod.m_extraPowerupHealIfDirectHit, true, false);
			AbilityMod.AddToken(tokens, this.m_extraPowerupEnergyIfDirectHitMod, "ExtraPowerupEnergy", string.Empty, spaceMarineDropPod.m_extraPowerupEnergyIfDirectHit, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SpaceMarineDropPod spaceMarineDropPod = base.GetTargetAbilityOnAbilityData(abilityData) as SpaceMarineDropPod;
		bool flag = spaceMarineDropPod != null;
		int num = 0;
		int num2 = 0;
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SpaceMarineDropPod.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			if (spaceMarineDropPod.m_powerupPrefab != null)
			{
				PowerUp_Standard_Ability component = spaceMarineDropPod.m_powerupPrefab.GetComponent<PowerUp_Standard_Ability>();
				if (component != null)
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
					num = component.m_healAmount;
					num2 = component.m_techPointsAmount;
				}
			}
		}
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix = "[Damage]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			for (;;)
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
		text = str + AbilityModHelper.GetModPropertyDesc(damageMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat knockbackDistanceMod = this.m_knockbackDistanceMod;
		string prefix2 = "[Knockback Distance]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			for (;;)
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
		text = str2 + AbilityModHelper.GetModPropertyDesc(knockbackDistanceMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt energyRefundIfNoEnemyHitMod = this.m_energyRefundIfNoEnemyHitMod;
		string prefix3 = "[EnergyRefundIfNoEnemyHit]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
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
			baseVal3 = spaceMarineDropPod.m_energyRefundIfNoEnemyHit;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(energyRefundIfNoEnemyHitMod, prefix3, showBaseVal3, baseVal3);
		text += AbilityModHelper.GetModPropertyDesc(this.m_powerupHealMod, "[PowerUp Heal Amount]", flag, (!flag) ? 0 : num);
		string str4 = text;
		AbilityModPropertyInt powerupTechPointMod = this.m_powerupTechPointMod;
		string prefix4 = "[PowerUp TechPoint Amount]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			for (;;)
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
		text = str4 + AbilityModHelper.GetModPropertyDesc(powerupTechPointMod, prefix4, showBaseVal4, baseVal4);
		text += AbilityModHelper.GetModGroundEffectInfoDesc(this.m_groundEffectInfoOnDropPod, "-- Ground Effect on Drop Pod Location --", flag, null);
		string str5 = text;
		AbilityModPropertyInt extraPowerupHealIfDirectHitMod = this.m_extraPowerupHealIfDirectHitMod;
		string prefix5 = "[ExtraPowerupHealIfDirectHit]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			for (;;)
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
		text = str5 + base.PropDesc(extraPowerupHealIfDirectHitMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt extraPowerupEnergyIfDirectHitMod = this.m_extraPowerupEnergyIfDirectHitMod;
		string prefix6 = "[ExtraPowerupEnergy]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
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
			baseVal6 = spaceMarineDropPod.m_extraPowerupEnergyIfDirectHit;
		}
		else
		{
			baseVal6 = 0;
		}
		return str6 + base.PropDesc(extraPowerupEnergyIfDirectHitMod, prefix6, showBaseVal6, baseVal6);
	}
}
