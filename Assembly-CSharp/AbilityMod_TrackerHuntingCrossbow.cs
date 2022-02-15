using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TrackerHuntingCrossbow : AbilityMod
{
	[Space(10f)]
	public AbilityModPropertyInt m_damageOnUntrackedMod;
	public AbilityModPropertyInt m_damageOnTrackedMod;
	public int m_damageChangeOnSubsequentTargets;
	[Space(10f)]
	public bool m_requireFunctioningBrush;
	public int m_extraDamageWhenInBrush;
	public StandardEffectInfo m_additionalEnemyEffectWhenInBrush;
	[Header("-- Laser Targeting Mods")]
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_laserLengthMod;
	public AbilityModPropertyInt m_laserMaxTargetsMod;
	public AbilityModPropertyEffectData m_huntedEffectDataOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(TrackerHuntingCrossbow);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TrackerHuntingCrossbow trackerHuntingCrossbow = targetAbility as TrackerHuntingCrossbow;
		if (trackerHuntingCrossbow != null)
		{
			AddToken(tokens, m_damageOnUntrackedMod, "Damage_Untracked", "damage on untracked targets", trackerHuntingCrossbow.m_laserDamageAmount);
			AddToken(tokens, m_damageOnTrackedMod, "Damage_Tracked", "damage on Tracked targets", trackerHuntingCrossbow.m_laserDamageAmount);
			if (m_damageChangeOnSubsequentTargets != 0)
			{
				tokens.Add(new TooltipTokenInt("DamageChangeOnSubsequent", "damage change after first hit, if piercing", Mathf.Abs(m_damageChangeOnSubsequentTargets)));
			}
			if (m_extraDamageWhenInBrush > 0)
			{
				tokens.Add(new TooltipTokenInt("ExtraDamageIfInBrush", "extra damage on targets in Brush", m_extraDamageWhenInBrush));
			}
			if (m_additionalEnemyEffectWhenInBrush != null && m_additionalEnemyEffectWhenInBrush.m_applyEffect)
			{
				AddToken_EffectInfo(tokens, m_additionalEnemyEffectWhenInBrush, "EffectOnTargetInBrush");
			}
			AddToken(tokens, m_laserWidthMod, "LaserWidth", "laser width", trackerHuntingCrossbow.m_laserInfo.width);
			AddToken(tokens, m_laserLengthMod, "LaserRange", "laser range", trackerHuntingCrossbow.m_laserInfo.range);
			AddToken(tokens, m_laserMaxTargetsMod, "LaserMaxTargets", "laser max number of targets hit", trackerHuntingCrossbow.m_laserInfo.maxTargets);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerHuntingCrossbow trackerHuntingCrossbow = GetTargetAbilityOnAbilityData(abilityData) as TrackerHuntingCrossbow;
		bool isAbilityPresent = trackerHuntingCrossbow != null;
		string desc = "";
		desc += AbilityModHelper.GetModPropertyDesc(m_damageOnUntrackedMod, "[Damage on Untracked Target]", isAbilityPresent, isAbilityPresent ? trackerHuntingCrossbow.m_laserDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageOnTrackedMod, "[Damage on Tracked Target]", isAbilityPresent, isAbilityPresent ? trackerHuntingCrossbow.m_laserDamageAmount : 0);
		if (m_damageChangeOnSubsequentTargets != 0)
		{
			desc += "[Damage Change on Subsequent Targets] = " + m_damageChangeOnSubsequentTargets + "\n";
		}
		if (m_extraDamageWhenInBrush > 0)
		{
			desc += "[Extra Damage when in " + (m_requireFunctioningBrush ? "Functioning" : "") + " Brush] = " + m_extraDamageWhenInBrush + "\n";
		}
		if (m_additionalEnemyEffectWhenInBrush != null && m_additionalEnemyEffectWhenInBrush.m_applyEffect)
		{
			desc += AbilityModHelper.GetModEffectDataDesc(m_additionalEnemyEffectWhenInBrush.m_effectData, "{ Additional Effect on Enemy when in " + (m_requireFunctioningBrush ? "Functioning Brush" : "Brush") + " }", "", isAbilityPresent);
		}
		desc += AbilityModHelper.GetModPropertyDesc(m_laserWidthMod, "[Laser Width]", isAbilityPresent, isAbilityPresent ? trackerHuntingCrossbow.m_laserInfo.width : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserLengthMod, "[Laser Length]", isAbilityPresent, isAbilityPresent ? trackerHuntingCrossbow.m_laserInfo.range : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserMaxTargetsMod, "[Laser Max Targets]", isAbilityPresent, isAbilityPresent ? trackerHuntingCrossbow.m_laserInfo.maxTargets : 0);
		return desc + PropDesc(m_huntedEffectDataOverride, "{ Hunted/Tracked Effect Override }", isAbilityPresent, isAbilityPresent ? trackerHuntingCrossbow.m_huntedEffectData : null);
	}
}
