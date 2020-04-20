using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BlasterDelayedLaser : AbilityMod
{
	[Header("-- Laser Data")]
	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	public AbilityModPropertyFloat m_lengthMod;

	public AbilityModPropertyFloat m_widthMod;

	[Space(10f)]
	public AbilityModPropertyBool m_triggerAimAtBlasterMod;

	[Header("-- On Hit")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyEffectInfo m_effectOnHitMod;

	public AbilityModPropertyInt m_extraDamageToNearEnemyMod;

	public AbilityModPropertyFloat m_nearDistanceMod;

	[Header("-- On Cast Hit Effect")]
	public AbilityModPropertyEffectInfo m_onCastEnemyHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(BlasterDelayedLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BlasterDelayedLaser blasterDelayedLaser = targetAbility as BlasterDelayedLaser;
		if (blasterDelayedLaser != null)
		{
			AbilityMod.AddToken(tokens, this.m_lengthMod, "Length", string.Empty, blasterDelayedLaser.m_length, true, false, false);
			AbilityMod.AddToken(tokens, this.m_widthMod, "Width", string.Empty, blasterDelayedLaser.m_width, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, blasterDelayedLaser.m_damageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnHitMod, "EffectOnHit", blasterDelayedLaser.m_effectOnHit, true);
			AbilityMod.AddToken(tokens, this.m_extraDamageToNearEnemyMod, "ExtraDamageToNearEnemy", string.Empty, blasterDelayedLaser.m_extraDamageToNearEnemy, true, false);
			AbilityMod.AddToken(tokens, this.m_nearDistanceMod, "NearDistance", string.Empty, blasterDelayedLaser.m_nearDistance, true, false, false);
			if (this.m_nearDistanceMod != null)
			{
				AbilityMod.AddToken_IntDiff(tokens, "NearDist_MinusOne", string.Empty, Mathf.RoundToInt(this.m_nearDistanceMod.GetModifiedValue(blasterDelayedLaser.m_nearDistance)) - 1, false, 0);
			}
			AbilityMod.AddToken_EffectMod(tokens, this.m_onCastEnemyHitEffectMod, "OnCastEnemyHitEffect", blasterDelayedLaser.m_onCastEnemyHitEffect, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterDelayedLaser blasterDelayedLaser = base.GetTargetAbilityOnAbilityData(abilityData) as BlasterDelayedLaser;
		bool flag = blasterDelayedLaser != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, flag && blasterDelayedLaser.m_penetrateLineOfSight);
		text += base.PropDesc(this.m_lengthMod, "[Length]", flag, (!flag) ? 0f : blasterDelayedLaser.m_length);
		string str = text;
		AbilityModPropertyFloat widthMod = this.m_widthMod;
		string prefix = "[Width]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = blasterDelayedLaser.m_width;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(widthMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyBool triggerAimAtBlasterMod = this.m_triggerAimAtBlasterMod;
		string prefix2 = "[TriggerAimAtBlaster]";
		bool showBaseVal2 = flag;
		bool baseVal2;
		if (flag)
		{
			baseVal2 = blasterDelayedLaser.m_triggerAimAtBlaster;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(triggerAimAtBlasterMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix3 = "[DamageAmount]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = blasterDelayedLaser.m_damageAmount;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(damageAmountMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyEffectInfo effectOnHitMod = this.m_effectOnHitMod;
		string prefix4 = "[EffectOnHit]";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
		if (flag)
		{
			baseVal4 = blasterDelayedLaser.m_effectOnHit;
		}
		else
		{
			baseVal4 = null;
		}
		text = str4 + base.PropDesc(effectOnHitMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt extraDamageToNearEnemyMod = this.m_extraDamageToNearEnemyMod;
		string prefix5 = "[ExtraDamageToNearEnemy]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = blasterDelayedLaser.m_extraDamageToNearEnemy;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(extraDamageToNearEnemyMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyFloat nearDistanceMod = this.m_nearDistanceMod;
		string prefix6 = "[NearDistance]";
		bool showBaseVal6 = flag;
		float baseVal6;
		if (flag)
		{
			baseVal6 = blasterDelayedLaser.m_nearDistance;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + base.PropDesc(nearDistanceMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectInfo onCastEnemyHitEffectMod = this.m_onCastEnemyHitEffectMod;
		string prefix7 = "[OnCastEnemyHitEffect]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
		if (flag)
		{
			baseVal7 = blasterDelayedLaser.m_onCastEnemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		return str7 + base.PropDesc(onCastEnemyHitEffectMod, prefix7, showBaseVal7, baseVal7);
	}
}
