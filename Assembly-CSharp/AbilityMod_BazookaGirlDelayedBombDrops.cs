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
		if (bazookaGirlDelayedBombDrops == null)
		{
			return;
		}
		AddToken(tokens, m_damageMod, "Damage", string.Empty, bazookaGirlDelayedBombDrops.m_bombInfo.m_damageAmount);
		AddToken(tokens, m_coneAngleMod, "ConeWidthAngle", string.Empty, bazookaGirlDelayedBombDrops.m_coneWidthAngle);
		AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, bazookaGirlDelayedBombDrops.m_coneLength);
		AddToken(tokens, m_maxNumOfAreasForExtraDamageMod, "MaxNumOfAreasForExtraDamage", string.Empty, bazookaGirlDelayedBombDrops.m_maxNumOfAreasForExtraDamage);
		AddToken(tokens, m_extraDamagePerFewerAreaMod, "ExtraDamagePerFewerArea", string.Empty, bazookaGirlDelayedBombDrops.m_extraDamagePerFewerArea);
		if (m_maxNumOfAreasForExtraDamageMod != null)
		{
			int modifiedValue = m_maxNumOfAreasForExtraDamageMod.GetModifiedValue(bazookaGirlDelayedBombDrops.m_maxNumOfAreasForExtraDamage);
			if (modifiedValue > 0)
			{
				int modifiedValue2 = m_extraDamagePerFewerAreaMod.GetModifiedValue(bazookaGirlDelayedBombDrops.m_extraDamagePerFewerArea);
				int val = modifiedValue2 * (modifiedValue - 1);
				AddToken_IntDiff(tokens, "MaxExtraDamage_Diff", string.Empty, val, false, 0);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlDelayedBombDrops bazookaGirlDelayedBombDrops = GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlDelayedBombDrops;
		bool isAbilityPresent = bazookaGirlDelayedBombDrops != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? bazookaGirlDelayedBombDrops.m_bombInfo.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_coneLengthMod, "[Cone Length]", isAbilityPresent, isAbilityPresent ? bazookaGirlDelayedBombDrops.m_coneLength : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_coneAngleMod, "[Cone Angle]", isAbilityPresent, isAbilityPresent ? bazookaGirlDelayedBombDrops.m_coneWidthAngle : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_targetAllMod, "[Target All?]", isAbilityPresent, isAbilityPresent && bazookaGirlDelayedBombDrops.m_targetAll);
		desc += PropDesc(m_penetrateLosMod, "[PenetrateLos]", isAbilityPresent, isAbilityPresent && bazookaGirlDelayedBombDrops.m_penetrateLos);
		desc += PropDesc(m_maxNumOfAreasForExtraDamageMod, "[MaxNumOfAreasForExtraDamage]", isAbilityPresent, isAbilityPresent ? bazookaGirlDelayedBombDrops.m_maxNumOfAreasForExtraDamage : 0);
		return desc + PropDesc(m_extraDamagePerFewerAreaMod, "[ExtraDamagePerFewerArea]", isAbilityPresent, isAbilityPresent ? bazookaGirlDelayedBombDrops.m_extraDamagePerFewerArea : 0);
	}
}
