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
		if (!(trackerHuntingCrossbow != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_damageOnUntrackedMod, "Damage_Untracked", "damage on untracked targets", trackerHuntingCrossbow.m_laserDamageAmount);
			AbilityMod.AddToken(tokens, m_damageOnTrackedMod, "Damage_Tracked", "damage on Tracked targets", trackerHuntingCrossbow.m_laserDamageAmount);
			if (m_damageChangeOnSubsequentTargets != 0)
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
				tokens.Add(new TooltipTokenInt("DamageChangeOnSubsequent", "damage change after first hit, if piercing", Mathf.Abs(m_damageChangeOnSubsequentTargets)));
			}
			if (m_extraDamageWhenInBrush > 0)
			{
				tokens.Add(new TooltipTokenInt("ExtraDamageIfInBrush", "extra damage on targets in Brush", m_extraDamageWhenInBrush));
			}
			if (m_additionalEnemyEffectWhenInBrush != null && m_additionalEnemyEffectWhenInBrush.m_applyEffect)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				AbilityMod.AddToken_EffectInfo(tokens, m_additionalEnemyEffectWhenInBrush, "EffectOnTargetInBrush");
			}
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", "laser width", trackerHuntingCrossbow.m_laserInfo.width);
			AbilityMod.AddToken(tokens, m_laserLengthMod, "LaserRange", "laser range", trackerHuntingCrossbow.m_laserInfo.range);
			AbilityMod.AddToken(tokens, m_laserMaxTargetsMod, "LaserMaxTargets", "laser max number of targets hit", trackerHuntingCrossbow.m_laserInfo.maxTargets);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TrackerHuntingCrossbow trackerHuntingCrossbow = GetTargetAbilityOnAbilityData(abilityData) as TrackerHuntingCrossbow;
		bool flag = trackerHuntingCrossbow != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt damageOnUntrackedMod = m_damageOnUntrackedMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (7)
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
			baseVal = trackerHuntingCrossbow.m_laserDamageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(damageOnUntrackedMod, "[Damage on Untracked Target]", flag, baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_damageOnTrackedMod, "[Damage on Tracked Target]", flag, flag ? trackerHuntingCrossbow.m_laserDamageAmount : 0);
		if (m_damageChangeOnSubsequentTargets != 0)
		{
			string text = empty;
			empty = text + "[Damage Change on Subsequent Targets] = " + m_damageChangeOnSubsequentTargets + "\n";
		}
		if (m_extraDamageWhenInBrush > 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			string text = empty;
			object[] obj = new object[6]
			{
				text,
				"[Extra Damage when in ",
				null,
				null,
				null,
				null
			};
			object obj2;
			if (m_requireFunctioningBrush)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				obj2 = "Functioning";
			}
			else
			{
				obj2 = string.Empty;
			}
			obj[2] = obj2;
			obj[3] = " Brush] = ";
			obj[4] = m_extraDamageWhenInBrush;
			obj[5] = "\n";
			empty = string.Concat(obj);
		}
		if (m_additionalEnemyEffectWhenInBrush != null && m_additionalEnemyEffectWhenInBrush.m_applyEffect)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			object obj3;
			if (m_requireFunctioningBrush)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				obj3 = "Functioning Brush";
			}
			else
			{
				obj3 = "Brush";
			}
			string str2 = (string)obj3;
			empty += AbilityModHelper.GetModEffectDataDesc(m_additionalEnemyEffectWhenInBrush.m_effectData, "{ Additional Effect on Enemy when in " + str2 + " }", string.Empty, flag);
		}
		string str3 = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
		float baseVal2;
		if (flag)
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
			baseVal2 = trackerHuntingCrossbow.m_laserInfo.width;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(laserWidthMod, "[Laser Width]", flag, baseVal2);
		empty += AbilityModHelper.GetModPropertyDesc(m_laserLengthMod, "[Laser Length]", flag, (!flag) ? 0f : trackerHuntingCrossbow.m_laserInfo.range);
		empty += AbilityModHelper.GetModPropertyDesc(m_laserMaxTargetsMod, "[Laser Max Targets]", flag, flag ? trackerHuntingCrossbow.m_laserInfo.maxTargets : 0);
		string str4 = empty;
		AbilityModPropertyEffectData huntedEffectDataOverride = m_huntedEffectDataOverride;
		object baseVal3;
		if (flag)
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
			baseVal3 = trackerHuntingCrossbow.m_huntedEffectData;
		}
		else
		{
			baseVal3 = null;
		}
		return str4 + PropDesc(huntedEffectDataOverride, "{ Hunted/Tracked Effect Override }", flag, (StandardActorEffectData)baseVal3);
	}
}
