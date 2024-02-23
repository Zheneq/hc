using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken_EffectInfo(tokens, m_additionalEffectOnSelf, "Effect_AdditionalOnSelf");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerFlewTheCoop trackerFlewTheCoop = GetTargetAbilityOnAbilityData(abilityData) as TrackerFlewTheCoop;
		bool isAbilityPresent = trackerFlewTheCoop != null;
		string desc = "";
		desc += AbilityModHelper.GetModPropertyDesc(m_landingShapeMod, "[Landing Shape]", isAbilityPresent, isAbilityPresent ? trackerFlewTheCoop.m_hookshotShape : AbilityAreaShape.SingleSquare);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_additionalEffectOnSelf, "[Additional Effect on Self]", "", isAbilityPresent);
		if (m_addVisionAroundStartSquare)
		{
			desc += "* Adding Vision on Starting Square\n";
			desc += new StringBuilder().Append("[Vision Radius] = ").Append(InEditorDescHelper.ColoredString(m_visionRadius.ToString())).Append("\n").ToString();
			desc += new StringBuilder().Append("[Vision Duration] = ").Append(InEditorDescHelper.ColoredString(m_visionDuration.ToString())).Append("\n").ToString();
			desc += new StringBuilder().Append("[Vision Brush Reveal Type] = ").Append(InEditorDescHelper.ColoredString(m_brushRevealType.ToString())).Append("\n").ToString();
		}
		desc += PropDesc(m_extraDroneDamageDuration, "[For this many turns:", isAbilityPresent);
		desc += PropDesc(m_extraDroneDamage, "[Extra Drone Damage]", isAbilityPresent);
		return new StringBuilder().Append(desc).Append(PropDesc(m_extraDroneUntrackedDamage, "[Extra Drone Untracked Damage]", isAbilityPresent)).ToString();
	}
}
