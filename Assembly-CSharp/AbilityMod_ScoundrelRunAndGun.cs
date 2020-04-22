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
			AbilityMod.AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, scoundrelRunAndGun.m_damageAmount);
			AbilityMod.AddToken(tokens, m_techPointGainWithNoHits, "EnergyGainIfNoHits", string.Empty, 0, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScoundrelRunAndGun scoundrelRunAndGun = GetTargetAbilityOnAbilityData(abilityData) as ScoundrelRunAndGun;
		bool flag = scoundrelRunAndGun != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal;
		if (flag)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = scoundrelRunAndGun.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(damageMod, "[Damage]", flag, baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_techPointGainWithNoHits, "[Energy Gain With No Hits]", flag);
		if (m_numTargeters > 1)
		{
			string text = empty;
			empty = text + "Using " + m_numTargeters + " targeters, make sure Target Data Override is set properly\n";
		}
		return empty;
	}
}
