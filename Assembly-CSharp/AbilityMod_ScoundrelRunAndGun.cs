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
			AbilityMod.AddToken(tokens, this.m_damageMod, "DamageAmount", string.Empty, scoundrelRunAndGun.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_techPointGainWithNoHits, "EnergyGainIfNoHits", string.Empty, 0, false, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScoundrelRunAndGun scoundrelRunAndGun = base.GetTargetAbilityOnAbilityData(abilityData) as ScoundrelRunAndGun;
		bool flag = scoundrelRunAndGun != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix = "[Damage]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ScoundrelRunAndGun.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = scoundrelRunAndGun.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(damageMod, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_techPointGainWithNoHits, "[Energy Gain With No Hits]", flag, 0);
		if (this.m_numTargeters > 1)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"Using ",
				this.m_numTargeters,
				" targeters, make sure Target Data Override is set properly\n"
			});
		}
		return text;
	}
}
