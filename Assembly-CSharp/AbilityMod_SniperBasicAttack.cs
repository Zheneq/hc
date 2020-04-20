using System;
using System.Collections.Generic;
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
			AbilityMod.AddToken(tokens, this.m_damageMod, "LaserDamageAmount", string.Empty, sniperBasicAttack.m_laserDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_minDamageMod, "MinDamageAmount", string.Empty, sniperBasicAttack.m_minDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_damageChangePerHitMod, "DamageChangePerHit", string.Empty, sniperBasicAttack.m_damageChangePerHit, true, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, sniperBasicAttack.m_laserInfo.width, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", string.Empty, sniperBasicAttack.m_laserInfo.range, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "LaserMaxTargets", string.Empty, sniperBasicAttack.m_laserInfo.maxTargets, true, false);
			if (this.m_farDistanceThreshold > 0f)
			{
				AbilityMod.AddToken_IntDiff(tokens, "FarDistThreshold", string.Empty, Mathf.RoundToInt(this.m_farDistanceThreshold), false, 0);
				AbilityMod.AddToken(tokens, this.m_farEnemyDamageMod, "Damage_Far", string.Empty, sniperBasicAttack.m_laserDamageAmount, true, false);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SniperBasicAttack sniperBasicAttack = base.GetTargetAbilityOnAbilityData(abilityData) as SniperBasicAttack;
		bool flag = sniperBasicAttack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix = "[Laser Width]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = sniperBasicAttack.m_laserInfo.width;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(laserWidthMod, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_laserRangeMod, "[Laser Range]", flag, (!flag) ? 0f : sniperBasicAttack.m_laserInfo.range);
		text += AbilityModHelper.GetModPropertyDesc(this.m_maxTargetsMod, "[Max Targets]", flag, (!flag) ? 0 : sniperBasicAttack.m_laserInfo.maxTargets);
		text += AbilityModHelper.GetModPropertyDesc(this.m_damageMod, "[Base Damage]", flag, (!flag) ? 0 : sniperBasicAttack.m_laserDamageAmount);
		string str2 = text;
		AbilityModPropertyInt minDamageMod = this.m_minDamageMod;
		string prefix2 = "[Min Damage]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = sniperBasicAttack.m_minDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(minDamageMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt damageChangePerHitMod = this.m_damageChangePerHitMod;
		string prefix3 = "[Damage Change Per Hit]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = sniperBasicAttack.m_damageChangePerHit;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(damageChangePerHitMod, prefix3, showBaseVal3, baseVal3);
		if (this.m_farDistanceThreshold > 0f)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Far Distance Threshold] = ",
				this.m_farDistanceThreshold,
				"\n"
			});
			text += AbilityModHelper.GetModPropertyDesc(this.m_farEnemyDamageMod, "[Far Enemy Damage]", flag, (!flag) ? 0 : sniperBasicAttack.m_laserDamageAmount);
		}
		return text;
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		SniperBasicAttack sniperBasicAttack = abilityAsBase as SniperBasicAttack;
		if (sniperBasicAttack != null)
		{
			int modifiedValue = this.m_damageMod.GetModifiedValue(sniperBasicAttack.m_laserDamageAmount);
			int modifiedValue2 = this.m_damageChangePerHitMod.GetModifiedValue(sniperBasicAttack.m_damageChangePerHit);
			if (modifiedValue2 != 0)
			{
				numbers.Add(modifiedValue + modifiedValue2);
			}
		}
	}
}
