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
		if (bazookaGirlDelayedBombDrops != null)
		{
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, bazookaGirlDelayedBombDrops.m_bombInfo.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_coneAngleMod, "ConeWidthAngle", string.Empty, bazookaGirlDelayedBombDrops.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneLengthMod, "ConeLength", string.Empty, bazookaGirlDelayedBombDrops.m_coneLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxNumOfAreasForExtraDamageMod, "MaxNumOfAreasForExtraDamage", string.Empty, bazookaGirlDelayedBombDrops.m_maxNumOfAreasForExtraDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamagePerFewerAreaMod, "ExtraDamagePerFewerArea", string.Empty, bazookaGirlDelayedBombDrops.m_extraDamagePerFewerArea, true, false);
			if (this.m_maxNumOfAreasForExtraDamageMod != null)
			{
				int modifiedValue = this.m_maxNumOfAreasForExtraDamageMod.GetModifiedValue(bazookaGirlDelayedBombDrops.m_maxNumOfAreasForExtraDamage);
				if (modifiedValue > 0)
				{
					int modifiedValue2 = this.m_extraDamagePerFewerAreaMod.GetModifiedValue(bazookaGirlDelayedBombDrops.m_extraDamagePerFewerArea);
					int val = modifiedValue2 * (modifiedValue - 1);
					AbilityMod.AddToken_IntDiff(tokens, "MaxExtraDamage_Diff", string.Empty, val, false, 0);
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlDelayedBombDrops bazookaGirlDelayedBombDrops = base.GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlDelayedBombDrops;
		bool flag = bazookaGirlDelayedBombDrops != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix = "[Damage]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = bazookaGirlDelayedBombDrops.m_bombInfo.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(damageMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat coneLengthMod = this.m_coneLengthMod;
		string prefix2 = "[Cone Length]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = bazookaGirlDelayedBombDrops.m_coneLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(coneLengthMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat coneAngleMod = this.m_coneAngleMod;
		string prefix3 = "[Cone Angle]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = bazookaGirlDelayedBombDrops.m_coneWidthAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(coneAngleMod, prefix3, showBaseVal3, baseVal3);
		text += AbilityModHelper.GetModPropertyDesc(this.m_targetAllMod, "[Target All?]", flag, flag && bazookaGirlDelayedBombDrops.m_targetAll);
		string str4 = text;
		AbilityModPropertyBool penetrateLosMod = this.m_penetrateLosMod;
		string prefix4 = "[PenetrateLos]";
		bool showBaseVal4 = flag;
		bool baseVal4;
		if (flag)
		{
			baseVal4 = bazookaGirlDelayedBombDrops.m_penetrateLos;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(penetrateLosMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_maxNumOfAreasForExtraDamageMod, "[MaxNumOfAreasForExtraDamage]", flag, (!flag) ? 0 : bazookaGirlDelayedBombDrops.m_maxNumOfAreasForExtraDamage);
		string str5 = text;
		AbilityModPropertyInt extraDamagePerFewerAreaMod = this.m_extraDamagePerFewerAreaMod;
		string prefix5 = "[ExtraDamagePerFewerArea]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = bazookaGirlDelayedBombDrops.m_extraDamagePerFewerArea;
		}
		else
		{
			baseVal5 = 0;
		}
		return str5 + base.PropDesc(extraDamagePerFewerAreaMod, prefix5, showBaseVal5, baseVal5);
	}
}
