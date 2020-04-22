using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ScoundrelBlindFire : AbilityMod
{
	[Header("-- Cone Damage and Angle ------------------------")]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyFloat m_coneWidthAngleMod;

	public AbilityModPropertyBool m_penetrateLineOfSight;

	[Header("-- Effect to apply on Target hit ----------------")]
	public StandardEffectInfo m_effectOnTargetsHit;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScoundrelBlindFire);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScoundrelBlindFire scoundrelBlindFire = targetAbility as ScoundrelBlindFire;
		if (!(scoundrelBlindFire != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, scoundrelBlindFire.m_coneWidthAngle);
			AbilityMod.AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, scoundrelBlindFire.m_damageAmount);
			AbilityMod.AddToken_EffectInfo(tokens, m_effectOnTargetsHit, "EffectOnTargetHit", null, false);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScoundrelBlindFire scoundrelBlindFire = GetTargetAbilityOnAbilityData(abilityData) as ScoundrelBlindFire;
		bool flag = scoundrelBlindFire != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal;
		if (flag)
		{
			baseVal = scoundrelBlindFire.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(damageMod, "[Cone Hit Damage]", flag, baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_coneWidthAngleMod, "[Cone Width Angle]", flag, (!flag) ? 0f : scoundrelBlindFire.m_coneWidthAngle);
		string str2 = empty;
		AbilityModPropertyBool penetrateLineOfSight = m_penetrateLineOfSight;
		int baseVal2;
		if (flag)
		{
			baseVal2 = (scoundrelBlindFire.m_penetrateLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(penetrateLineOfSight, "[Penetrate Line Of Sight]", flag, (byte)baseVal2 != 0);
		return empty + AbilityModHelper.GetModEffectInfoDesc(m_effectOnTargetsHit, "{ Effect on Target Hit }", string.Empty, flag);
	}
}
