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
		AbilityModPropertyBarrierDataV2 barrierDataMod = m_barrierDataMod;
		object baseVal;
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
			baseVal = trackerTeslaPrison.m_prisonBarrierData;
		}
		else
		{
			baseVal = null;
		}
		AbilityMod.AddToken_BarrierMod(tokens, barrierDataMod, "Wall", (StandardBarrierData)baseVal);
		object obj;
		if (trackerTeslaPrison != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			obj = trackerTeslaPrison.GetComponent<TrackerDroneInfoComponent>();
		}
		else
		{
			obj = null;
		}
		TrackerDroneInfoComponent trackerDroneInfoComponent = (TrackerDroneInfoComponent)obj;
		AbilityModPropertyFloat droneTargeterMaxRangeFromCasterMod = m_droneTargeterMaxRangeFromCasterMod;
		string empty = string.Empty;
		float baseVal2;
		if (trackerDroneInfoComponent != null)
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
			baseVal2 = trackerDroneInfoComponent.m_targeterMaxRangeFromCaster;
		}
		else
		{
			baseVal2 = 0f;
		}
		AbilityMod.AddToken(tokens, droneTargeterMaxRangeFromCasterMod, "TargeterMaxRangeFromCaster", empty, baseVal2, trackerDroneInfoComponent != null);
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerTeslaPrison trackerTeslaPrison = GetTargetAbilityOnAbilityData(abilityData) as TrackerTeslaPrison;
		bool flag = trackerTeslaPrison != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBarrierDataV2 barrierDataMod = m_barrierDataMod;
		object baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = trackerTeslaPrison.m_prisonBarrierData;
		}
		else
		{
			baseVal = null;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(barrierDataMod, "{ Barrier Data Mod }", (StandardBarrierData)baseVal);
		empty += AbilityModHelper.GetModGroundEffectInfoDesc(m_groundEffectInfoInCage, "-- Ground Effect Inside Cage --", flag);
		string str2 = empty;
		AbilityModPropertyEffectInfo additionalEffectOnEnemiesInShapeMod = m_additionalEffectOnEnemiesInShapeMod;
		object baseVal2;
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
			baseVal2 = trackerTeslaPrison.m_additionalEffectOnEnemiesInShape;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str2 + PropDesc(additionalEffectOnEnemiesInShapeMod, "[AdditionalEffectOnEnemiesInShape]", flag, (StandardEffectInfo)baseVal2);
		TrackerDroneInfoComponent trackerDroneInfoComponent = (!(trackerTeslaPrison != null)) ? null : trackerTeslaPrison.GetComponent<TrackerDroneInfoComponent>();
		string str3 = empty;
		AbilityModPropertyFloat droneTargeterMaxRangeFromCasterMod = m_droneTargeterMaxRangeFromCasterMod;
		bool showBaseVal = trackerDroneInfoComponent != null;
		float baseVal3;
		if (trackerDroneInfoComponent != null)
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
			baseVal3 = trackerDroneInfoComponent.m_targeterMaxRangeFromCaster;
		}
		else
		{
			baseVal3 = 0f;
		}
		return str3 + PropDesc(droneTargeterMaxRangeFromCasterMod, "[DroneTargeterMaxRangeFromCaster]", showBaseVal, baseVal3);
	}
}
