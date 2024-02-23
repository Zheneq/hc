using System;
using System.Collections.Generic;
using System.Text;
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
		bool isAbilityPresent = trackerTeslaPrison != null;
		AddToken_BarrierMod(tokens, m_barrierDataMod, "Wall", isAbilityPresent ? trackerTeslaPrison.m_prisonBarrierData : null);
		TrackerDroneInfoComponent trackerDroneInfoComponent = trackerTeslaPrison != null ? trackerTeslaPrison.GetComponent<TrackerDroneInfoComponent>() : null;
		AddToken(tokens, m_droneTargeterMaxRangeFromCasterMod, "TargeterMaxRangeFromCaster", "", trackerDroneInfoComponent != null ? trackerDroneInfoComponent.m_targeterMaxRangeFromCaster : 0f, trackerDroneInfoComponent != null);
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerTeslaPrison trackerTeslaPrison = GetTargetAbilityOnAbilityData(abilityData) as TrackerTeslaPrison;
		bool isAbilityPresent = trackerTeslaPrison != null;
		string desc = "";
		desc += AbilityModHelper.GetModPropertyDesc(m_barrierDataMod, "{ Barrier Data Mod }", isAbilityPresent ? trackerTeslaPrison.m_prisonBarrierData : null);
		desc += AbilityModHelper.GetModGroundEffectInfoDesc(m_groundEffectInfoInCage, "-- Ground Effect Inside Cage --", isAbilityPresent);
		desc += PropDesc(m_additionalEffectOnEnemiesInShapeMod, "[AdditionalEffectOnEnemiesInShape]", isAbilityPresent, isAbilityPresent ? trackerTeslaPrison.m_additionalEffectOnEnemiesInShape : null);
		TrackerDroneInfoComponent trackerDroneInfoComponent = trackerTeslaPrison != null ? trackerTeslaPrison.GetComponent<TrackerDroneInfoComponent>() : null;
		return new StringBuilder().Append(desc).Append(PropDesc(m_droneTargeterMaxRangeFromCasterMod, "[DroneTargeterMaxRangeFromCaster]", trackerDroneInfoComponent != null, trackerDroneInfoComponent != null ? trackerDroneInfoComponent.m_targeterMaxRangeFromCaster : 0f)).ToString();
	}
}
