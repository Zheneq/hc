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
			AbilityMod.AddToken(tokens, this.m_damageOnUntrackedMod, "Damage_Untracked", "damage on untracked targets", trackerHuntingCrossbow.m_laserDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_damageOnTrackedMod, "Damage_Tracked", "damage on Tracked targets", trackerHuntingCrossbow.m_laserDamageAmount, true, false);
			if (this.m_damageChangeOnSubsequentTargets != 0)
			{
				tokens.Add(new TooltipTokenInt("DamageChangeOnSubsequent", "damage change after first hit, if piercing", Mathf.Abs(this.m_damageChangeOnSubsequentTargets)));
			}
			if (this.m_extraDamageWhenInBrush > 0)
			{
				tokens.Add(new TooltipTokenInt("ExtraDamageIfInBrush", "extra damage on targets in Brush", this.m_extraDamageWhenInBrush));
			}
			if (this.m_additionalEnemyEffectWhenInBrush != null && this.m_additionalEnemyEffectWhenInBrush.m_applyEffect)
			{
				AbilityMod.AddToken_EffectInfo(tokens, this.m_additionalEnemyEffectWhenInBrush, "EffectOnTargetInBrush", null, true);
			}
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", "laser width", trackerHuntingCrossbow.m_laserInfo.width, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserLengthMod, "LaserRange", "laser range", trackerHuntingCrossbow.m_laserInfo.range, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserMaxTargetsMod, "LaserMaxTargets", "laser max number of targets hit", trackerHuntingCrossbow.m_laserInfo.maxTargets, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerHuntingCrossbow trackerHuntingCrossbow = base.GetTargetAbilityOnAbilityData(abilityData) as TrackerHuntingCrossbow;
		bool flag = trackerHuntingCrossbow != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt damageOnUntrackedMod = this.m_damageOnUntrackedMod;
		string prefix = "[Damage on Untracked Target]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = trackerHuntingCrossbow.m_laserDamageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(damageOnUntrackedMod, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_damageOnTrackedMod, "[Damage on Tracked Target]", flag, (!flag) ? 0 : trackerHuntingCrossbow.m_laserDamageAmount);
		if (this.m_damageChangeOnSubsequentTargets != 0)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Damage Change on Subsequent Targets] = ",
				this.m_damageChangeOnSubsequentTargets,
				"\n"
			});
		}
		if (this.m_extraDamageWhenInBrush > 0)
		{
			string text2 = text;
			object[] array = new object[6];
			array[0] = text2;
			array[1] = "[Extra Damage when in ";
			int num = 2;
			object obj;
			if (this.m_requireFunctioningBrush)
			{
				obj = "Functioning";
			}
			else
			{
				obj = string.Empty;
			}
			array[num] = obj;
			array[3] = " Brush] = ";
			array[4] = this.m_extraDamageWhenInBrush;
			array[5] = "\n";
			text = string.Concat(array);
		}
		if (this.m_additionalEnemyEffectWhenInBrush != null && this.m_additionalEnemyEffectWhenInBrush.m_applyEffect)
		{
			string text3;
			if (this.m_requireFunctioningBrush)
			{
				text3 = "Functioning Brush";
			}
			else
			{
				text3 = "Brush";
			}
			string str2 = text3;
			text += AbilityModHelper.GetModEffectDataDesc(this.m_additionalEnemyEffectWhenInBrush.m_effectData, "{ Additional Effect on Enemy when in " + str2 + " }", string.Empty, flag, null);
		}
		string str3 = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix2 = "[Laser Width]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = trackerHuntingCrossbow.m_laserInfo.width;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(laserWidthMod, prefix2, showBaseVal2, baseVal2);
		text += AbilityModHelper.GetModPropertyDesc(this.m_laserLengthMod, "[Laser Length]", flag, (!flag) ? 0f : trackerHuntingCrossbow.m_laserInfo.range);
		text += AbilityModHelper.GetModPropertyDesc(this.m_laserMaxTargetsMod, "[Laser Max Targets]", flag, (!flag) ? 0 : trackerHuntingCrossbow.m_laserInfo.maxTargets);
		string str4 = text;
		AbilityModPropertyEffectData huntedEffectDataOverride = this.m_huntedEffectDataOverride;
		string prefix3 = "{ Hunted/Tracked Effect Override }";
		bool showBaseVal3 = flag;
		StandardActorEffectData baseVal3;
		if (flag)
		{
			baseVal3 = trackerHuntingCrossbow.m_huntedEffectData;
		}
		else
		{
			baseVal3 = null;
		}
		return str4 + base.PropDesc(huntedEffectDataOverride, prefix3, showBaseVal3, baseVal3);
	}
}
