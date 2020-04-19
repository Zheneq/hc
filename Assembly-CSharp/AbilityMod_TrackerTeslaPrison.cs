using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TrackerTeslaPrison : AbilityMod
{
	[Header("-- Barrier Data Override")]
	public AbilityModPropertyBarrierDataV2 m_barrierDataMod;

	[Header("-- Ground Effect Inside Cage")]
	public StandardGroundEffectInfo m_groundEffectInfoInCage;

	[Header("-- Additional Effect to enemies in shape --")]
	public AbilityModPropertyEffectInfo m_additionalEffectOnEnemiesInShapeMod;

	[Header("-- Mods on Drone --")]
	public AbilityModPropertyFloat m_droneTargeterMaxRangeFromCasterMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(TrackerTeslaPrison);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TrackerTeslaPrison trackerTeslaPrison = targetAbility as TrackerTeslaPrison;
		bool flag = trackerTeslaPrison != null;
		AbilityModPropertyBarrierDataV2 barrierDataMod = this.m_barrierDataMod;
		string tokenName = "Wall";
		StandardBarrierData baseVal;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_TrackerTeslaPrison.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			baseVal = trackerTeslaPrison.m_prisonBarrierData;
		}
		else
		{
			baseVal = null;
		}
		AbilityMod.AddToken_BarrierMod(tokens, barrierDataMod, tokenName, baseVal);
		TrackerDroneInfoComponent trackerDroneInfoComponent;
		if (trackerTeslaPrison != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			trackerDroneInfoComponent = trackerTeslaPrison.GetComponent<TrackerDroneInfoComponent>();
		}
		else
		{
			trackerDroneInfoComponent = null;
		}
		TrackerDroneInfoComponent trackerDroneInfoComponent2 = trackerDroneInfoComponent;
		AbilityModPropertyFloat droneTargeterMaxRangeFromCasterMod = this.m_droneTargeterMaxRangeFromCasterMod;
		string tokenName2 = "TargeterMaxRangeFromCaster";
		string empty = string.Empty;
		float baseVal2;
		if (trackerDroneInfoComponent2 != null)
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
			baseVal2 = trackerDroneInfoComponent2.m_targeterMaxRangeFromCaster;
		}
		else
		{
			baseVal2 = 0f;
		}
		AbilityMod.AddToken(tokens, droneTargeterMaxRangeFromCasterMod, tokenName2, empty, baseVal2, trackerDroneInfoComponent2 != null, false, false);
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerTeslaPrison trackerTeslaPrison = base.GetTargetAbilityOnAbilityData(abilityData) as TrackerTeslaPrison;
		bool flag = trackerTeslaPrison != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBarrierDataV2 barrierDataMod = this.m_barrierDataMod;
		string prefix = "{ Barrier Data Mod }";
		StandardBarrierData baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_TrackerTeslaPrison.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = trackerTeslaPrison.m_prisonBarrierData;
		}
		else
		{
			baseVal = null;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(barrierDataMod, prefix, baseVal);
		text += AbilityModHelper.GetModGroundEffectInfoDesc(this.m_groundEffectInfoInCage, "-- Ground Effect Inside Cage --", flag, null);
		string str2 = text;
		AbilityModPropertyEffectInfo additionalEffectOnEnemiesInShapeMod = this.m_additionalEffectOnEnemiesInShapeMod;
		string prefix2 = "[AdditionalEffectOnEnemiesInShape]";
		bool showBaseVal = flag;
		StandardEffectInfo baseVal2;
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
			baseVal2 = trackerTeslaPrison.m_additionalEffectOnEnemiesInShape;
		}
		else
		{
			baseVal2 = null;
		}
		text = str2 + base.PropDesc(additionalEffectOnEnemiesInShapeMod, prefix2, showBaseVal, baseVal2);
		TrackerDroneInfoComponent trackerDroneInfoComponent = (!(trackerTeslaPrison != null)) ? null : trackerTeslaPrison.GetComponent<TrackerDroneInfoComponent>();
		string str3 = text;
		AbilityModPropertyFloat droneTargeterMaxRangeFromCasterMod = this.m_droneTargeterMaxRangeFromCasterMod;
		string prefix3 = "[DroneTargeterMaxRangeFromCaster]";
		bool showBaseVal2 = trackerDroneInfoComponent != null;
		float baseVal3;
		if (trackerDroneInfoComponent != null)
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
			baseVal3 = trackerDroneInfoComponent.m_targeterMaxRangeFromCaster;
		}
		else
		{
			baseVal3 = 0f;
		}
		return str3 + base.PropDesc(droneTargeterMaxRangeFromCasterMod, prefix3, showBaseVal2, baseVal3);
	}
}
