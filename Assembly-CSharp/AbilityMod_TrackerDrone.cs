using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TrackerDrone : AbilityMod
{
	[Space(10f)]
	public AbilityModPropertyBool m_hitInvisibleTargetsMod;
	[Header("-- Whether to apply <Tracked> effect")]
	public bool m_applyHuntedEffect;
	[Header("-- Damage Mods")]
	public AbilityModPropertyInt m_trackedHitDamageMod;
	public AbilityModPropertyInt m_untrackedHitDamageMod;
	public int m_extraDamageWhenMovingOnTracked;
	public int m_extraDamageWhenMovingOnUntracked;
	[Header("-- Hit Effect Overrides")]
	public AbilityModPropertyEffectInfo m_trackedHitEffectOverride;
	public AbilityModPropertyEffectInfo m_untrackedHitEffectOverride;
	[Header("-- Mods on Drone --")]
	public AbilityModPropertyFloat m_droneTargeterMaxRangeFromCasterMod;
	public AbilityModPropertyFloat m_droneVisionRadiusMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(TrackerDrone);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TrackerDrone trackerDrone = targetAbility as TrackerDrone;
		TrackerDroneInfoComponent trackerDroneInfoComponent = trackerDrone?.GetComponent<TrackerDroneInfoComponent>();
		if (trackerDroneInfoComponent != null)
		{
			AddToken(tokens, m_trackedHitDamageMod, "Damage_Tracked", "Tracked damage", trackerDroneInfoComponent.m_droneHitDamageAmount);
			AddToken(tokens, m_untrackedHitDamageMod, "Damage_Untracked", "Untracked damage", trackerDroneInfoComponent.m_untrackedDroneHitDamageAmount);
			if (m_extraDamageWhenMovingOnTracked > 0 && m_trackedHitDamageMod != null)
			{
				int damage = m_trackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent.m_droneHitDamageAmount) + m_extraDamageWhenMovingOnTracked;
				AddToken_IntDiff(tokens, "Damage_TrackedWhenMoving", "Total damage for Tracked, when moving", damage, true, m_trackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent.m_droneHitDamageAmount));
			}
			if (m_extraDamageWhenMovingOnUntracked > 0)
			{
				int damage = m_untrackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent.m_untrackedDroneHitDamageAmount) + m_extraDamageWhenMovingOnUntracked;
				AddToken_IntDiff(tokens, "Damage_UntrackedWhenMoving", "Total damage for Untracked, when moving", damage, true, m_untrackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent.m_untrackedDroneHitDamageAmount));
			}
			AddToken_EffectMod(tokens, m_trackedHitEffectOverride, "Effect_TrackedHit", trackerDroneInfoComponent.m_droneHitEffect);
			AddToken_EffectMod(tokens, m_untrackedHitEffectOverride, "Effect_TrackedHit", trackerDroneInfoComponent.m_untrackedDroneHitEffect);
			AddToken(tokens, m_droneTargeterMaxRangeFromCasterMod, "TargeterMaxRangeFromCaster", "", trackerDroneInfoComponent.m_targeterMaxRangeFromCaster, trackerDroneInfoComponent);
			AddToken(tokens, m_droneVisionRadiusMod, "DroneVisionRadius", "", trackerDroneInfoComponent.m_droneVisionRadius, trackerDroneInfoComponent);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerDrone trackerDrone = GetTargetAbilityOnAbilityData(abilityData) as TrackerDrone;
		TrackerDroneInfoComponent trackerDroneInfoComponent = trackerDrone?.GetComponent<TrackerDroneInfoComponent>();
		bool isValid = trackerDroneInfoComponent != null && trackerDrone != null;
		string desc = "";
		desc += AbilityModHelper.GetModPropertyDesc(m_hitInvisibleTargetsMod, "[Hit Invisible Targets]", isValid, isValid && trackerDroneInfoComponent.m_hitInvisibleTargets);
		if (m_applyHuntedEffect)
		{
			desc += "Applies Tracked effect to targets hit\n";
		}
		desc += AbilityModHelper.GetModPropertyDesc(m_trackedHitDamageMod, "[Damage on Tracked]", isValid, isValid ? trackerDroneInfoComponent.m_droneHitDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_untrackedHitDamageMod, "[Damage on Untracked]", isValid, isValid ? trackerDroneInfoComponent.m_untrackedDroneHitDamageAmount : 0);
		if (m_extraDamageWhenMovingOnTracked > 0)
		{
			desc = desc + "[Extra Damage on Tracked] = " + InEditorDescHelper.ColoredString(m_extraDamageWhenMovingOnTracked.ToString()) + "\n";
			if (isValid && m_trackedHitDamageMod != null)
			{
				int modifiedValue = m_trackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent.m_droneHitDamageAmount);
				desc = desc + "\tTotal Damage on Tracked = " + InEditorDescHelper.ColoredString((modifiedValue + m_extraDamageWhenMovingOnTracked).ToString()) + "\n";
			}
		}
		if (m_extraDamageWhenMovingOnUntracked > 0)
		{
			desc = desc + "[Extra Damage on Untracked] = " + InEditorDescHelper.ColoredString(m_extraDamageWhenMovingOnUntracked.ToString()) + "\n";
			if (isValid && m_untrackedHitDamageMod != null)
			{
				int modifiedValue = m_untrackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent.m_untrackedDroneHitDamageAmount);
				desc = desc + "\tTotal Damage on Untracked = " + InEditorDescHelper.ColoredString((modifiedValue + m_extraDamageWhenMovingOnUntracked).ToString()) + "\n";
			}
		}
		desc += AbilityModHelper.GetModPropertyDesc(m_trackedHitEffectOverride, "{ Tracked Hit Effect Override }", isValid, isValid ? trackerDroneInfoComponent.m_droneHitEffect : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_untrackedHitEffectOverride, "{ Untracked Hit Effect Override }", isValid, isValid ? trackerDroneInfoComponent.m_untrackedDroneHitEffect : null);
		desc += PropDesc(m_droneTargeterMaxRangeFromCasterMod, "[DroneTargeterMaxRangeFromCaster]", trackerDroneInfoComponent != null, trackerDroneInfoComponent != null ? trackerDroneInfoComponent.m_targeterMaxRangeFromCaster : 0f);
		return desc + PropDesc(m_droneVisionRadiusMod, "[DroneVisionRadius]", trackerDroneInfoComponent != null, trackerDroneInfoComponent != null ? trackerDroneInfoComponent.m_droneVisionRadius : 0f);
	}
}
