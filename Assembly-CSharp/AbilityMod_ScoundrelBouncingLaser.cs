using System;
using System.Collections.Generic;
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
			AbilityMod.AddToken(tokens, this.m_baseDamageMod, "DamageAmount", string.Empty, scoundrelBouncingLaser.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_minDamageMod, "MinDamageAmount", string.Empty, scoundrelBouncingLaser.m_minDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_damageChangePerHitMod, "DamageChangePerHit", string.Empty, scoundrelBouncingLaser.m_damageChangePerHit, true, false);
			AbilityMod.AddToken(tokens, this.m_bonusDamagePerBounceMod, "BonusDamagePerBounce", string.Empty, scoundrelBouncingLaser.m_bonusDamagePerBounce, true, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "Width", string.Empty, scoundrelBouncingLaser.m_width, true, false, false);
			AbilityMod.AddToken(tokens, this.m_distancePerBounceMod, "MaxDistancePerBounce", string.Empty, scoundrelBouncingLaser.m_maxDistancePerBounce, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTotalDistanceMod, "MaxTotalDistance", string.Empty, scoundrelBouncingLaser.m_maxTotalDistance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxBounceMod, "MaxBounces", string.Empty, scoundrelBouncingLaser.m_maxBounces, true, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargetsHit", string.Empty, scoundrelBouncingLaser.m_maxTargetsHit, true, false);
			if (this.m_baseDamageMod != null)
			{
				if (this.m_damageChangePerHitMod != null)
				{
					int modifiedValue = this.m_baseDamageMod.GetModifiedValue(scoundrelBouncingLaser.m_damageAmount);
					int modifiedValue2 = this.m_damageChangePerHitMod.GetModifiedValue(scoundrelBouncingLaser.m_damageChangePerHit);
					if (modifiedValue2 != 0)
					{
						AbilityMod.AddToken_IntDiff(tokens, "FirstDamageAfterChange", string.Empty, modifiedValue + modifiedValue2, false, 0);
					}
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScoundrelBouncingLaser scoundrelBouncingLaser = base.GetTargetAbilityOnAbilityData(abilityData) as ScoundrelBouncingLaser;
		bool flag = scoundrelBouncingLaser != null;
		string text = string.Empty;
		text += AbilityModHelper.GetModPropertyDesc(this.m_maxTargetsMod, "[Max Target Hits]", flag, (!flag) ? 0 : scoundrelBouncingLaser.m_maxTargetsHit);
		string str = text;
		AbilityModPropertyInt maxBounceMod = this.m_maxBounceMod;
		string prefix = "[Max Bounces]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = scoundrelBouncingLaser.m_maxBounces;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(maxBounceMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix2 = "[Laser Width]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = scoundrelBouncingLaser.m_width;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(laserWidthMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat distancePerBounceMod = this.m_distancePerBounceMod;
		string prefix3 = "[Distance Per Bounce]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = scoundrelBouncingLaser.m_maxDistancePerBounce;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(distancePerBounceMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat maxTotalDistanceMod = this.m_maxTotalDistanceMod;
		string prefix4 = "[Max Total Distance]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = scoundrelBouncingLaser.m_maxTotalDistance;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(maxTotalDistanceMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt baseDamageMod = this.m_baseDamageMod;
		string prefix5 = "[Base Damage]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = scoundrelBouncingLaser.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(baseDamageMod, prefix5, showBaseVal5, baseVal5);
		text += AbilityModHelper.GetModPropertyDesc(this.m_minDamageMod, "[Min Base Damage Mod]", flag, (!flag) ? 0 : scoundrelBouncingLaser.m_minDamageAmount);
		string str6 = text;
		AbilityModPropertyInt damageChangePerHitMod = this.m_damageChangePerHitMod;
		string prefix6 = "[Damage Change Per Hit]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = scoundrelBouncingLaser.m_damageChangePerHit;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + AbilityModHelper.GetModPropertyDesc(damageChangePerHitMod, prefix6, showBaseVal6, baseVal6);
		return text + AbilityModHelper.GetModPropertyDesc(this.m_bonusDamagePerBounceMod, "[Bonus Damage Per Bounce]", flag, (!flag) ? 0 : scoundrelBouncingLaser.m_bonusDamagePerBounce);
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		ScoundrelBouncingLaser scoundrelBouncingLaser = abilityAsBase as ScoundrelBouncingLaser;
		if (scoundrelBouncingLaser != null)
		{
			int modifiedValue = this.m_baseDamageMod.GetModifiedValue(scoundrelBouncingLaser.m_damageAmount);
			int modifiedValue2 = this.m_damageChangePerHitMod.GetModifiedValue(scoundrelBouncingLaser.m_damageChangePerHit);
			numbers.Add(modifiedValue + modifiedValue2);
		}
	}
}
