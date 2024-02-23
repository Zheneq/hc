using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_ScoundrelBouncingLaser : AbilityMod
{
	[Header("-- Laser Properties")]
	public AbilityModPropertyInt m_maxTargetsMod;
	public AbilityModPropertyInt m_maxBounceMod;
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_distancePerBounceMod;
	public AbilityModPropertyFloat m_maxTotalDistanceMod;
	[Header("-- Damage")]
	public AbilityModPropertyInt m_baseDamageMod;
	public AbilityModPropertyInt m_minDamageMod;
	public AbilityModPropertyInt m_damageChangePerHitMod;
	public AbilityModPropertyInt m_bonusDamagePerBounceMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScoundrelBouncingLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScoundrelBouncingLaser scoundrelBouncingLaser = targetAbility as ScoundrelBouncingLaser;
		if (scoundrelBouncingLaser != null)
		{
			AddToken(tokens, m_baseDamageMod, "DamageAmount", "", scoundrelBouncingLaser.m_damageAmount);
			AddToken(tokens, m_minDamageMod, "MinDamageAmount", "", scoundrelBouncingLaser.m_minDamageAmount);
			AddToken(tokens, m_damageChangePerHitMod, "DamageChangePerHit", "", scoundrelBouncingLaser.m_damageChangePerHit);
			AddToken(tokens, m_bonusDamagePerBounceMod, "BonusDamagePerBounce", "", scoundrelBouncingLaser.m_bonusDamagePerBounce);
			AddToken(tokens, m_laserWidthMod, "Width", "", scoundrelBouncingLaser.m_width);
			AddToken(tokens, m_distancePerBounceMod, "MaxDistancePerBounce", "", scoundrelBouncingLaser.m_maxDistancePerBounce);
			AddToken(tokens, m_maxTotalDistanceMod, "MaxTotalDistance", "", scoundrelBouncingLaser.m_maxTotalDistance);
			AddToken(tokens, m_maxBounceMod, "MaxBounces", "", scoundrelBouncingLaser.m_maxBounces);
			AddToken(tokens, m_maxTargetsMod, "MaxTargetsHit", "", scoundrelBouncingLaser.m_maxTargetsHit);
			if (m_baseDamageMod != null && m_damageChangePerHitMod != null)
			{
				int baseDamage = m_baseDamageMod.GetModifiedValue(scoundrelBouncingLaser.m_damageAmount);
				int bonusDamage = m_damageChangePerHitMod.GetModifiedValue(scoundrelBouncingLaser.m_damageChangePerHit);
				if (bonusDamage != 0)
				{
					AddToken_IntDiff(tokens, "FirstDamageAfterChange", "", baseDamage + bonusDamage, false, 0);
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScoundrelBouncingLaser scoundrelBouncingLaser = GetTargetAbilityOnAbilityData(abilityData) as ScoundrelBouncingLaser;
		bool isAbilityPresent = scoundrelBouncingLaser != null;
		string desc = "";
		desc += AbilityModHelper.GetModPropertyDesc(m_maxTargetsMod, "[Max Target Hits]", isAbilityPresent, isAbilityPresent ? scoundrelBouncingLaser.m_maxTargetsHit : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_maxBounceMod, "[Max Bounces]", isAbilityPresent, isAbilityPresent ? scoundrelBouncingLaser.m_maxBounces : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserWidthMod, "[Laser Width]", isAbilityPresent, isAbilityPresent ? scoundrelBouncingLaser.m_width : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_distancePerBounceMod, "[Distance Per Bounce]", isAbilityPresent, isAbilityPresent ? scoundrelBouncingLaser.m_maxDistancePerBounce : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_maxTotalDistanceMod, "[Max Total Distance]", isAbilityPresent, isAbilityPresent ? scoundrelBouncingLaser.m_maxTotalDistance : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_baseDamageMod, "[Base Damage]", isAbilityPresent, isAbilityPresent ? scoundrelBouncingLaser.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_minDamageMod, "[Min Base Damage Mod]", isAbilityPresent, isAbilityPresent ? scoundrelBouncingLaser.m_minDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageChangePerHitMod, "[Damage Change Per Hit]", isAbilityPresent, isAbilityPresent ? scoundrelBouncingLaser.m_damageChangePerHit : 0);
		return new StringBuilder().Append(desc).Append(AbilityModHelper.GetModPropertyDesc(m_bonusDamagePerBounceMod, "[Bonus Damage Per Bounce]", isAbilityPresent, isAbilityPresent ? scoundrelBouncingLaser.m_bonusDamagePerBounce : 0)).ToString();
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		ScoundrelBouncingLaser scoundrelBouncingLaser = abilityAsBase as ScoundrelBouncingLaser;
		if (scoundrelBouncingLaser != null)
		{
			int baseDamage = m_baseDamageMod.GetModifiedValue(scoundrelBouncingLaser.m_damageAmount);
			int bonusDamage = m_damageChangePerHitMod.GetModifiedValue(scoundrelBouncingLaser.m_damageChangePerHit);
			numbers.Add(baseDamage + bonusDamage);
		}
	}
}
