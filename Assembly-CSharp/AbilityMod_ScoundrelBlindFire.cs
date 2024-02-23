using System;
using System.Collections.Generic;
using System.Text;
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
		if (scoundrelBlindFire != null)
		{
			AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", "", scoundrelBlindFire.m_coneWidthAngle);
			AddToken(tokens, m_damageMod, "DamageAmount", "", scoundrelBlindFire.m_damageAmount);
			AddToken_EffectInfo(tokens, m_effectOnTargetsHit, "EffectOnTargetHit", null, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScoundrelBlindFire scoundrelBlindFire = GetTargetAbilityOnAbilityData(abilityData) as ScoundrelBlindFire;
		bool isAbilityPresent = scoundrelBlindFire != null;
		string desc = "";
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Cone Hit Damage]", isAbilityPresent, isAbilityPresent ? scoundrelBlindFire.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_coneWidthAngleMod, "[Cone Width Angle]", isAbilityPresent, isAbilityPresent ? scoundrelBlindFire.m_coneWidthAngle : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_penetrateLineOfSight, "[Penetrate Line Of Sight]", isAbilityPresent, isAbilityPresent && scoundrelBlindFire.m_penetrateLineOfSight);
		return new StringBuilder().Append(desc).Append(AbilityModHelper.GetModEffectInfoDesc(m_effectOnTargetsHit, "{ Effect on Target Hit }", "", isAbilityPresent)).ToString();
	}
}
