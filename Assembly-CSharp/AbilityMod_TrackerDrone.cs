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
		object obj;
		if (trackerDrone != null)
		{
			obj = trackerDrone.GetComponent<TrackerDroneInfoComponent>();
		}
		else
		{
			obj = null;
		}
		TrackerDroneInfoComponent trackerDroneInfoComponent = (TrackerDroneInfoComponent)obj;
		if (!(trackerDroneInfoComponent != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_trackedHitDamageMod, "Damage_Tracked", "Tracked damage", trackerDroneInfoComponent.m_droneHitDamageAmount);
			AbilityMod.AddToken(tokens, m_untrackedHitDamageMod, "Damage_Untracked", "Untracked damage", trackerDroneInfoComponent.m_untrackedDroneHitDamageAmount);
			if (m_extraDamageWhenMovingOnTracked > 0 && m_trackedHitDamageMod != null)
			{
				int modifiedValue = m_trackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent.m_droneHitDamageAmount);
				int val = modifiedValue + m_extraDamageWhenMovingOnTracked;
				AbilityMod.AddToken_IntDiff(tokens, "Damage_TrackedWhenMoving", "Total damage for Tracked, when moving", val, true, modifiedValue);
			}
			if (m_extraDamageWhenMovingOnUntracked > 0)
			{
				int modifiedValue2 = m_untrackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent.m_untrackedDroneHitDamageAmount);
				int val2 = modifiedValue2 + m_extraDamageWhenMovingOnUntracked;
				AbilityMod.AddToken_IntDiff(tokens, "Damage_UntrackedWhenMoving", "Total damage for Untracked, when moving", val2, true, modifiedValue2);
			}
			AbilityMod.AddToken_EffectMod(tokens, m_trackedHitEffectOverride, "Effect_TrackedHit", trackerDroneInfoComponent.m_droneHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_untrackedHitEffectOverride, "Effect_TrackedHit", trackerDroneInfoComponent.m_untrackedDroneHitEffect);
			AbilityMod.AddToken(tokens, m_droneTargeterMaxRangeFromCasterMod, "TargeterMaxRangeFromCaster", string.Empty, trackerDroneInfoComponent.m_targeterMaxRangeFromCaster, trackerDroneInfoComponent);
			AbilityMod.AddToken(tokens, m_droneVisionRadiusMod, "DroneVisionRadius", string.Empty, trackerDroneInfoComponent.m_droneVisionRadius, trackerDroneInfoComponent);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerDrone trackerDrone = GetTargetAbilityOnAbilityData(abilityData) as TrackerDrone;
		object obj;
		if (trackerDrone != null)
		{
			obj = trackerDrone.GetComponent<TrackerDroneInfoComponent>();
		}
		else
		{
			obj = null;
		}
		TrackerDroneInfoComponent trackerDroneInfoComponent = (TrackerDroneInfoComponent)obj;
		int num;
		if (trackerDroneInfoComponent != null)
		{
			num = ((trackerDrone != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool hitInvisibleTargetsMod = m_hitInvisibleTargetsMod;
		int baseVal;
		if (flag)
		{
			baseVal = (trackerDroneInfoComponent.m_hitInvisibleTargets ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(hitInvisibleTargetsMod, "[Hit Invisible Targets]", flag, (byte)baseVal != 0);
		if (m_applyHuntedEffect)
		{
			empty += "Applies Tracked effect to targets hit\n";
		}
		string str2 = empty;
		AbilityModPropertyInt trackedHitDamageMod = m_trackedHitDamageMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = trackerDroneInfoComponent.m_droneHitDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(trackedHitDamageMod, "[Damage on Tracked]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt untrackedHitDamageMod = m_untrackedHitDamageMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = trackerDroneInfoComponent.m_untrackedDroneHitDamageAmount;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(untrackedHitDamageMod, "[Damage on Untracked]", flag, baseVal3);
		if (m_extraDamageWhenMovingOnTracked > 0)
		{
			empty = empty + "[Extra Damage on Tracked] = " + InEditorDescHelper.ColoredString(m_extraDamageWhenMovingOnTracked.ToString()) + "\n";
			if (flag)
			{
				if (m_trackedHitDamageMod != null)
				{
					int modifiedValue = m_trackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent.m_droneHitDamageAmount);
					empty = empty + "\tTotal Damage on Tracked = " + InEditorDescHelper.ColoredString((modifiedValue + m_extraDamageWhenMovingOnTracked).ToString()) + "\n";
				}
			}
		}
		if (m_extraDamageWhenMovingOnUntracked > 0)
		{
			empty = empty + "[Extra Damage on Untracked] = " + InEditorDescHelper.ColoredString(m_extraDamageWhenMovingOnUntracked.ToString()) + "\n";
			if (flag)
			{
				if (m_untrackedHitDamageMod != null)
				{
					int modifiedValue2 = m_untrackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent.m_untrackedDroneHitDamageAmount);
					empty = empty + "\tTotal Damage on Untracked = " + InEditorDescHelper.ColoredString((modifiedValue2 + m_extraDamageWhenMovingOnUntracked).ToString()) + "\n";
				}
			}
		}
		string str4 = empty;
		AbilityModPropertyEffectInfo trackedHitEffectOverride = m_trackedHitEffectOverride;
		object baseVal4;
		if (flag)
		{
			baseVal4 = trackerDroneInfoComponent.m_droneHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(trackedHitEffectOverride, "{ Tracked Hit Effect Override }", flag, (StandardEffectInfo)baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectInfo untrackedHitEffectOverride = m_untrackedHitEffectOverride;
		object baseVal5;
		if (flag)
		{
			baseVal5 = trackerDroneInfoComponent.m_untrackedDroneHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(untrackedHitEffectOverride, "{ Untracked Hit Effect Override }", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyFloat droneTargeterMaxRangeFromCasterMod = m_droneTargeterMaxRangeFromCasterMod;
		bool showBaseVal = trackerDroneInfoComponent != null;
		float baseVal6;
		if (trackerDroneInfoComponent != null)
		{
			baseVal6 = trackerDroneInfoComponent.m_targeterMaxRangeFromCaster;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + PropDesc(droneTargeterMaxRangeFromCasterMod, "[DroneTargeterMaxRangeFromCaster]", showBaseVal, baseVal6);
		string str7 = empty;
		AbilityModPropertyFloat droneVisionRadiusMod = m_droneVisionRadiusMod;
		bool showBaseVal2 = trackerDroneInfoComponent != null;
		float baseVal7;
		if (trackerDroneInfoComponent != null)
		{
			baseVal7 = trackerDroneInfoComponent.m_droneVisionRadius;
		}
		else
		{
			baseVal7 = 0f;
		}
		return str7 + PropDesc(droneVisionRadiusMod, "[DroneVisionRadius]", showBaseVal2, baseVal7);
	}
}
