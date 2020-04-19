using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TrackerFlewTheCoop : AbilityMod
{
	[Space(10f)]
	public AbilityModPropertyShape m_landingShapeMod;

	public StandardEffectInfo m_additionalEffectOnSelf;

	[Header("-- Add vision on start square, if radius <= 0, uses actor's sight range")]
	public bool m_addVisionAroundStartSquare;

	public float m_visionRadius = -1f;

	public int m_visionDuration = 2;

	public VisionProviderInfo.BrushRevealType m_brushRevealType = VisionProviderInfo.BrushRevealType.BaseOnCenterPosition;

	[Header("-- Drone extra damage after escape")]
	public AbilityModPropertyInt m_extraDroneDamageDuration;

	public AbilityModPropertyInt m_extraDroneDamage;

	public AbilityModPropertyInt m_extraDroneUntrackedDamage;

	public override Type GetTargetAbilityType()
	{
		return typeof(TrackerFlewTheCoop);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TrackerFlewTheCoop x = targetAbility as TrackerFlewTheCoop;
		if (x != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_TrackerFlewTheCoop.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken_EffectInfo(tokens, this.m_additionalEffectOnSelf, "Effect_AdditionalOnSelf", null, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerFlewTheCoop trackerFlewTheCoop = base.GetTargetAbilityOnAbilityData(abilityData) as TrackerFlewTheCoop;
		bool flag = trackerFlewTheCoop != null;
		string str = string.Empty;
		str += AbilityModHelper.GetModPropertyDesc(this.m_landingShapeMod, "[Landing Shape]", flag, (!flag) ? AbilityAreaShape.SingleSquare : trackerFlewTheCoop.m_hookshotShape);
		str += AbilityModHelper.GetModEffectInfoDesc(this.m_additionalEffectOnSelf, "[Additional Effect on Self]", string.Empty, flag, null);
		if (this.m_addVisionAroundStartSquare)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_TrackerFlewTheCoop.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			str += "* Adding Vision on Starting Square\n";
			str = str + "[Vision Radius] = " + InEditorDescHelper.ColoredString(this.m_visionRadius.ToString(), "cyan", false) + "\n";
			str = str + "[Vision Duration] = " + InEditorDescHelper.ColoredString(this.m_visionDuration.ToString(), "cyan", false) + "\n";
			str = str + "[Vision Brush Reveal Type] = " + InEditorDescHelper.ColoredString(this.m_brushRevealType.ToString(), "cyan", false) + "\n";
		}
		str += base.PropDesc(this.m_extraDroneDamageDuration, "[For this many turns:", flag, 0);
		str += base.PropDesc(this.m_extraDroneDamage, "[Extra Drone Damage]", flag, 0);
		return str + base.PropDesc(this.m_extraDroneUntrackedDamage, "[Extra Drone Untracked Damage]", flag, 0);
	}
}
