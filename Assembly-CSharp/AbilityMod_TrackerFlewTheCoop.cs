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
		if (!(x != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_EffectInfo(tokens, m_additionalEffectOnSelf, "Effect_AdditionalOnSelf");
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerFlewTheCoop trackerFlewTheCoop = GetTargetAbilityOnAbilityData(abilityData) as TrackerFlewTheCoop;
		bool flag = trackerFlewTheCoop != null;
		string empty = string.Empty;
		empty += AbilityModHelper.GetModPropertyDesc(m_landingShapeMod, "[Landing Shape]", flag, flag ? trackerFlewTheCoop.m_hookshotShape : AbilityAreaShape.SingleSquare);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_additionalEffectOnSelf, "[Additional Effect on Self]", string.Empty, flag);
		if (m_addVisionAroundStartSquare)
		{
			empty += "* Adding Vision on Starting Square\n";
			empty = empty + "[Vision Radius] = " + InEditorDescHelper.ColoredString(m_visionRadius.ToString()) + "\n";
			empty = empty + "[Vision Duration] = " + InEditorDescHelper.ColoredString(m_visionDuration.ToString()) + "\n";
			empty = empty + "[Vision Brush Reveal Type] = " + InEditorDescHelper.ColoredString(m_brushRevealType.ToString()) + "\n";
		}
		empty += PropDesc(m_extraDroneDamageDuration, "[For this many turns:", flag);
		empty += PropDesc(m_extraDroneDamage, "[Extra Drone Damage]", flag);
		return empty + PropDesc(m_extraDroneUntrackedDamage, "[Extra Drone Untracked Damage]", flag);
	}
}
