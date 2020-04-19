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
		TrackerDroneInfoComponent trackerDroneInfoComponent;
		if (trackerDrone != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_TrackerDrone.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			trackerDroneInfoComponent = trackerDrone.GetComponent<TrackerDroneInfoComponent>();
		}
		else
		{
			trackerDroneInfoComponent = null;
		}
		TrackerDroneInfoComponent trackerDroneInfoComponent2 = trackerDroneInfoComponent;
		if (trackerDroneInfoComponent2 != null)
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
			AbilityMod.AddToken(tokens, this.m_trackedHitDamageMod, "Damage_Tracked", "Tracked damage", trackerDroneInfoComponent2.m_droneHitDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_untrackedHitDamageMod, "Damage_Untracked", "Untracked damage", trackerDroneInfoComponent2.m_untrackedDroneHitDamageAmount, true, false);
			if (this.m_extraDamageWhenMovingOnTracked > 0 && this.m_trackedHitDamageMod != null)
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
				int modifiedValue = this.m_trackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent2.m_droneHitDamageAmount);
				int val = modifiedValue + this.m_extraDamageWhenMovingOnTracked;
				AbilityMod.AddToken_IntDiff(tokens, "Damage_TrackedWhenMoving", "Total damage for Tracked, when moving", val, true, modifiedValue);
			}
			if (this.m_extraDamageWhenMovingOnUntracked > 0)
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
				int modifiedValue2 = this.m_untrackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent2.m_untrackedDroneHitDamageAmount);
				int val2 = modifiedValue2 + this.m_extraDamageWhenMovingOnUntracked;
				AbilityMod.AddToken_IntDiff(tokens, "Damage_UntrackedWhenMoving", "Total damage for Untracked, when moving", val2, true, modifiedValue2);
			}
			AbilityMod.AddToken_EffectMod(tokens, this.m_trackedHitEffectOverride, "Effect_TrackedHit", trackerDroneInfoComponent2.m_droneHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_untrackedHitEffectOverride, "Effect_TrackedHit", trackerDroneInfoComponent2.m_untrackedDroneHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_droneTargeterMaxRangeFromCasterMod, "TargeterMaxRangeFromCaster", string.Empty, trackerDroneInfoComponent2.m_targeterMaxRangeFromCaster, trackerDroneInfoComponent2, false, false);
			AbilityMod.AddToken(tokens, this.m_droneVisionRadiusMod, "DroneVisionRadius", string.Empty, trackerDroneInfoComponent2.m_droneVisionRadius, trackerDroneInfoComponent2, false, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerDrone trackerDrone = base.GetTargetAbilityOnAbilityData(abilityData) as TrackerDrone;
		TrackerDroneInfoComponent trackerDroneInfoComponent;
		if (trackerDrone != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_TrackerDrone.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			trackerDroneInfoComponent = trackerDrone.GetComponent<TrackerDroneInfoComponent>();
		}
		else
		{
			trackerDroneInfoComponent = null;
		}
		TrackerDroneInfoComponent trackerDroneInfoComponent2 = trackerDroneInfoComponent;
		bool flag;
		if (trackerDroneInfoComponent2 != null)
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
			flag = (trackerDrone != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool hitInvisibleTargetsMod = this.m_hitInvisibleTargetsMod;
		string prefix = "[Hit Invisible Targets]";
		bool showBaseVal = flag2;
		bool baseVal;
		if (flag2)
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
			baseVal = trackerDroneInfoComponent2.m_hitInvisibleTargets;
		}
		else
		{
			baseVal = false;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(hitInvisibleTargetsMod, prefix, showBaseVal, baseVal);
		if (this.m_applyHuntedEffect)
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
			text += "Applies Tracked effect to targets hit\n";
		}
		string str2 = text;
		AbilityModPropertyInt trackedHitDamageMod = this.m_trackedHitDamageMod;
		string prefix2 = "[Damage on Tracked]";
		bool showBaseVal2 = flag2;
		int baseVal2;
		if (flag2)
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
			baseVal2 = trackerDroneInfoComponent2.m_droneHitDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(trackedHitDamageMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt untrackedHitDamageMod = this.m_untrackedHitDamageMod;
		string prefix3 = "[Damage on Untracked]";
		bool showBaseVal3 = flag2;
		int baseVal3;
		if (flag2)
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
			baseVal3 = trackerDroneInfoComponent2.m_untrackedDroneHitDamageAmount;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(untrackedHitDamageMod, prefix3, showBaseVal3, baseVal3);
		if (this.m_extraDamageWhenMovingOnTracked > 0)
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
			text = text + "[Extra Damage on Tracked] = " + InEditorDescHelper.ColoredString(this.m_extraDamageWhenMovingOnTracked.ToString(), "cyan", false) + "\n";
			if (flag2)
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
				if (this.m_trackedHitDamageMod != null)
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
					int modifiedValue = this.m_trackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent2.m_droneHitDamageAmount);
					text = text + "\tTotal Damage on Tracked = " + InEditorDescHelper.ColoredString((modifiedValue + this.m_extraDamageWhenMovingOnTracked).ToString(), "cyan", false) + "\n";
				}
			}
		}
		if (this.m_extraDamageWhenMovingOnUntracked > 0)
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
			text = text + "[Extra Damage on Untracked] = " + InEditorDescHelper.ColoredString(this.m_extraDamageWhenMovingOnUntracked.ToString(), "cyan", false) + "\n";
			if (flag2)
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
				if (this.m_untrackedHitDamageMod != null)
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
					int modifiedValue2 = this.m_untrackedHitDamageMod.GetModifiedValue(trackerDroneInfoComponent2.m_untrackedDroneHitDamageAmount);
					text = text + "\tTotal Damage on Untracked = " + InEditorDescHelper.ColoredString((modifiedValue2 + this.m_extraDamageWhenMovingOnUntracked).ToString(), "cyan", false) + "\n";
				}
			}
		}
		string str4 = text;
		AbilityModPropertyEffectInfo trackedHitEffectOverride = this.m_trackedHitEffectOverride;
		string prefix4 = "{ Tracked Hit Effect Override }";
		bool showBaseVal4 = flag2;
		StandardEffectInfo baseVal4;
		if (flag2)
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
			baseVal4 = trackerDroneInfoComponent2.m_droneHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(trackedHitEffectOverride, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectInfo untrackedHitEffectOverride = this.m_untrackedHitEffectOverride;
		string prefix5 = "{ Untracked Hit Effect Override }";
		bool showBaseVal5 = flag2;
		StandardEffectInfo baseVal5;
		if (flag2)
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
			baseVal5 = trackerDroneInfoComponent2.m_untrackedDroneHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(untrackedHitEffectOverride, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyFloat droneTargeterMaxRangeFromCasterMod = this.m_droneTargeterMaxRangeFromCasterMod;
		string prefix6 = "[DroneTargeterMaxRangeFromCaster]";
		bool showBaseVal6 = trackerDroneInfoComponent2 != null;
		float baseVal6;
		if (trackerDroneInfoComponent2 != null)
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
			baseVal6 = trackerDroneInfoComponent2.m_targeterMaxRangeFromCaster;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + base.PropDesc(droneTargeterMaxRangeFromCasterMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyFloat droneVisionRadiusMod = this.m_droneVisionRadiusMod;
		string prefix7 = "[DroneVisionRadius]";
		bool showBaseVal7 = trackerDroneInfoComponent2 != null;
		float baseVal7;
		if (trackerDroneInfoComponent2 != null)
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
			baseVal7 = trackerDroneInfoComponent2.m_droneVisionRadius;
		}
		else
		{
			baseVal7 = 0f;
		}
		return str7 + base.PropDesc(droneVisionRadiusMod, prefix7, showBaseVal7, baseVal7);
	}
}
