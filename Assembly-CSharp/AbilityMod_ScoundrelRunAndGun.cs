using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ScoundrelRunAndGun : AbilityMod
{
	[Header("-- Run and Gun Specific Mods --------------------------------------------")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyInt m_techPointGainWithNoHits;
	[Header("-- Multistep Targeter Config ---------------------------------------------")]
	public int m_numTargeters = 1;
	public float m_minDistanceBetweenSteps;
	public float m_maxDistanceBetweenSteps = 5f;
	public float m_minDistanceBetweenAnySteps = -1f;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScoundrelRunAndGun);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScoundrelRunAndGun scoundrelRunAndGun = targetAbility as ScoundrelRunAndGun;
		if (scoundrelRunAndGun != null)
		{
			AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, scoundrelRunAndGun.m_damageAmount);
			AddToken(tokens, m_techPointGainWithNoHits, "EnergyGainIfNoHits", string.Empty, 0, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScoundrelRunAndGun scoundrelRunAndGun = GetTargetAbilityOnAbilityData(abilityData) as ScoundrelRunAndGun;
		bool isAbilityPresent = scoundrelRunAndGun != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? scoundrelRunAndGun.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_techPointGainWithNoHits, "[Energy Gain With No Hits]", isAbilityPresent);
		if (m_numTargeters > 1)
		{
			desc += "Using " + m_numTargeters + " targeters, make sure Target Data Override is set properly\n";
		}
		return desc;
	}
}
