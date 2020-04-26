using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BazookaGirlDelayedBombDrops : AbilityMod
{
	[Header("-- Damage")]
	public AbilityModPropertyInt m_damageMod;

	[Header("-- Cone Targeting Mod")]
	public AbilityModPropertyFloat m_coneLengthMod;

	public AbilityModPropertyFloat m_coneAngleMod;

	[Header("-- Target All Enemies")]
	public AbilityModPropertyBool m_targetAllMod;

	[Header("-- Targeting Ignore Wall?")]
	public AbilityModPropertyBool m_penetrateLosMod;

	[Header("-- Additional Damage from fewer hit areas, = extraDmgPeArea * Max(0, (maxNumAreas - numAreas))")]
	public AbilityModPropertyInt m_maxNumOfAreasForExtraDamageMod;

	public AbilityModPropertyInt m_extraDamagePerFewerAreaMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(BazookaGirlDelayedBombDrops);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BazookaGirlDelayedBombDrops bazookaGirlDelayedBombDrops = targetAbility as BazookaGirlDelayedBombDrops;
		if (!(bazookaGirlDelayedBombDrops != null))
		{
			return;
		}
		AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, bazookaGirlDelayedBombDrops.m_bombInfo.m_damageAmount);
		AbilityMod.AddToken(tokens, m_coneAngleMod, "ConeWidthAngle", string.Empty, bazookaGirlDelayedBombDrops.m_coneWidthAngle);
		AbilityMod.AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, bazookaGirlDelayedBombDrops.m_coneLength);
		AbilityMod.AddToken(tokens, m_maxNumOfAreasForExtraDamageMod, "MaxNumOfAreasForExtraDamage", string.Empty, bazookaGirlDelayedBombDrops.m_maxNumOfAreasForExtraDamage);
		AbilityMod.AddToken(tokens, m_extraDamagePerFewerAreaMod, "ExtraDamagePerFewerArea", string.Empty, bazookaGirlDelayedBombDrops.m_extraDamagePerFewerArea);
		if (m_maxNumOfAreasForExtraDamageMod == null)
		{
			return;
		}
		while (true)
		{
			int modifiedValue = m_maxNumOfAreasForExtraDamageMod.GetModifiedValue(bazookaGirlDelayedBombDrops.m_maxNumOfAreasForExtraDamage);
			if (modifiedValue > 0)
			{
				while (true)
				{
					int modifiedValue2 = m_extraDamagePerFewerAreaMod.GetModifiedValue(bazookaGirlDelayedBombDrops.m_extraDamagePerFewerArea);
					int val = modifiedValue2 * (modifiedValue - 1);
					AbilityMod.AddToken_IntDiff(tokens, "MaxExtraDamage_Diff", string.Empty, val, false, 0);
					return;
				}
			}
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlDelayedBombDrops bazookaGirlDelayedBombDrops = GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlDelayedBombDrops;
		bool flag = bazookaGirlDelayedBombDrops != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal;
		if (flag)
		{
			baseVal = bazookaGirlDelayedBombDrops.m_bombInfo.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(damageMod, "[Damage]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat coneLengthMod = m_coneLengthMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = bazookaGirlDelayedBombDrops.m_coneLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(coneLengthMod, "[Cone Length]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat coneAngleMod = m_coneAngleMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = bazookaGirlDelayedBombDrops.m_coneWidthAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(coneAngleMod, "[Cone Angle]", flag, baseVal3);
		empty += AbilityModHelper.GetModPropertyDesc(m_targetAllMod, "[Target All?]", flag, flag && bazookaGirlDelayedBombDrops.m_targetAll);
		string str4 = empty;
		AbilityModPropertyBool penetrateLosMod = m_penetrateLosMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = (bazookaGirlDelayedBombDrops.m_penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(penetrateLosMod, "[PenetrateLos]", flag, (byte)baseVal4 != 0);
		empty += PropDesc(m_maxNumOfAreasForExtraDamageMod, "[MaxNumOfAreasForExtraDamage]", flag, flag ? bazookaGirlDelayedBombDrops.m_maxNumOfAreasForExtraDamage : 0);
		string str5 = empty;
		AbilityModPropertyInt extraDamagePerFewerAreaMod = m_extraDamagePerFewerAreaMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = bazookaGirlDelayedBombDrops.m_extraDamagePerFewerArea;
		}
		else
		{
			baseVal5 = 0;
		}
		return str5 + PropDesc(extraDamagePerFewerAreaMod, "[ExtraDamagePerFewerArea]", flag, baseVal5);
	}
}
