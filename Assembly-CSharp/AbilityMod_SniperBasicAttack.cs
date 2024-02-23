using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_SniperBasicAttack : AbilityMod
{
	[Header("-- Laser Property Mod")]
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_laserRangeMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	[Header("-- Damage Mod")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyInt m_minDamageMod;
	public AbilityModPropertyInt m_damageChangePerHitMod;
	[Header("-- Distance Based Damage Mod")]
	public float m_farDistanceThreshold;
	public AbilityModPropertyInt m_farEnemyDamageMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SniperBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SniperBasicAttack sniperBasicAttack = targetAbility as SniperBasicAttack;
		if (sniperBasicAttack != null)
		{
			AddToken(tokens, m_damageMod, "LaserDamageAmount", string.Empty, sniperBasicAttack.m_laserDamageAmount);
			AddToken(tokens, m_minDamageMod, "MinDamageAmount", string.Empty, sniperBasicAttack.m_minDamageAmount);
			AddToken(tokens, m_damageChangePerHitMod, "DamageChangePerHit", string.Empty, sniperBasicAttack.m_damageChangePerHit);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, sniperBasicAttack.m_laserInfo.width);
			AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, sniperBasicAttack.m_laserInfo.range);
			AddToken(tokens, m_maxTargetsMod, "LaserMaxTargets", string.Empty, sniperBasicAttack.m_laserInfo.maxTargets);
			if (m_farDistanceThreshold > 0f)
			{
				AddToken_IntDiff(tokens, "FarDistThreshold", string.Empty, Mathf.RoundToInt(m_farDistanceThreshold), false, 0);
				AddToken(tokens, m_farEnemyDamageMod, "Damage_Far", string.Empty, sniperBasicAttack.m_laserDamageAmount);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SniperBasicAttack sniperBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as SniperBasicAttack;
		bool isValid = sniperBasicAttack != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_laserWidthMod, "[Laser Width]", isValid, isValid ? sniperBasicAttack.m_laserInfo.width : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserRangeMod, "[Laser Range]", isValid, isValid ? sniperBasicAttack.m_laserInfo.range : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_maxTargetsMod, "[Max Targets]", isValid, isValid ? sniperBasicAttack.m_laserInfo.maxTargets : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Base Damage]", isValid, isValid ? sniperBasicAttack.m_laserDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_minDamageMod, "[Min Damage]", isValid, isValid ? sniperBasicAttack.m_minDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageChangePerHitMod, "[Damage Change Per Hit]", isValid, isValid ? sniperBasicAttack.m_damageChangePerHit : 0);
		if (m_farDistanceThreshold > 0f)
		{
			desc += new StringBuilder().Append("[Far Distance Threshold] = ").Append(m_farDistanceThreshold).Append("\n").ToString();
			desc += AbilityModHelper.GetModPropertyDesc(m_farEnemyDamageMod, "[Far Enemy Damage]", isValid, isValid ? sniperBasicAttack.m_laserDamageAmount : 0);
		}
		return desc;
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		SniperBasicAttack sniperBasicAttack = abilityAsBase as SniperBasicAttack;
		if (sniperBasicAttack != null)
		{
			int laserDamage = m_damageMod.GetModifiedValue(sniperBasicAttack.m_laserDamageAmount);
			int damageChangePerHit = m_damageChangePerHitMod.GetModifiedValue(sniperBasicAttack.m_damageChangePerHit);
			if (damageChangePerHit != 0)
			{
				numbers.Add(laserDamage + damageChangePerHit);
			}
		}
	}
}
