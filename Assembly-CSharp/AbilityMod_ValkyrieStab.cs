using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ValkyrieStab : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_coneWidthMinAngleMod;

	public AbilityModPropertyFloat m_coneWidthMaxAngleMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	public AbilityModPropertyFloat m_coneMinLengthMod;

	public AbilityModPropertyFloat m_coneMaxLengthMod;

	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Header("-- On Hit Damage/Effect")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyInt m_lessDamagePerTargetMod;

	public AbilityModPropertyInt m_extraDamageOnSpearTip;

	public AbilityModPropertyInt m_extraDamageFirstTarget;

	public AbilityModPropertyEffectInfo m_targetHitEffectMod;

	[Header("-- Misc ability interactions --")]
	public AbilityModPropertyInt m_perHitExtraAbsorbNextShieldBlock;

	public AbilityModPropertyInt m_maxExtraAbsorbNextShieldBlock;

	public override Type GetTargetAbilityType()
	{
		return typeof(ValkyrieStab);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ValkyrieStab valkyrieStab = targetAbility as ValkyrieStab;
		if (valkyrieStab != null)
		{
			AbilityMod.AddToken(tokens, this.m_coneWidthMinAngleMod, "ConeWidthMinAngle", string.Empty, valkyrieStab.m_coneWidthMinAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneWidthMaxAngleMod, "ConeWidthMaxAngle", string.Empty, valkyrieStab.m_coneWidthMaxAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, valkyrieStab.m_coneBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneMinLengthMod, "ConeMinLength", string.Empty, valkyrieStab.m_coneMinLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneMaxLengthMod, "ConeMaxLength", string.Empty, valkyrieStab.m_coneMaxLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, valkyrieStab.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, valkyrieStab.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_lessDamagePerTargetMod, "LessDamagePerTarget", string.Empty, valkyrieStab.m_lessDamagePerTarget, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageOnSpearTip, "ExtraDamageOnSpearTip", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageFirstTarget, "ExtraDamageFirstTarget", string.Empty, 0, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_targetHitEffectMod, "TargetHitEffect", valkyrieStab.m_targetHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_perHitExtraAbsorbNextShieldBlock, "PerHitExtraAbsorbNextShieldBlock", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_maxExtraAbsorbNextShieldBlock, "MaxExtraAbsorbNextShieldBlock", string.Empty, 0, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ValkyrieStab valkyrieStab = base.GetTargetAbilityOnAbilityData(abilityData) as ValkyrieStab;
		bool flag = valkyrieStab != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat coneWidthMinAngleMod = this.m_coneWidthMinAngleMod;
		string prefix = "[ConeWidthMinAngle]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = valkyrieStab.m_coneWidthMinAngle;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(coneWidthMinAngleMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat coneWidthMaxAngleMod = this.m_coneWidthMaxAngleMod;
		string prefix2 = "[ConeWidthMaxAngle]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = valkyrieStab.m_coneWidthMaxAngle;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(coneWidthMaxAngleMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, (!flag) ? 0f : valkyrieStab.m_coneBackwardOffset);
		string str3 = text;
		AbilityModPropertyFloat coneMinLengthMod = this.m_coneMinLengthMod;
		string prefix3 = "[ConeMinLength]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = valkyrieStab.m_coneMinLength;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(coneMinLengthMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat coneMaxLengthMod = this.m_coneMaxLengthMod;
		string prefix4 = "[ConeMaxLength]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = valkyrieStab.m_coneMaxLength;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(coneMaxLengthMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, flag && valkyrieStab.m_penetrateLineOfSight);
		string str5 = text;
		AbilityModPropertyInt maxTargetsMod = this.m_maxTargetsMod;
		string prefix5 = "[MaxTargets]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = valkyrieStab.m_maxTargets;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(maxTargetsMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix6 = "[DamageAmount]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = valkyrieStab.m_damageAmount;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(damageAmountMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_lessDamagePerTargetMod, "[LessDamagePerTarget]", flag, (!flag) ? 0 : valkyrieStab.m_lessDamagePerTarget);
		text += base.PropDesc(this.m_extraDamageOnSpearTip, "[ExtraDamageOnSpearTip]", flag, 0);
		text += base.PropDesc(this.m_extraDamageFirstTarget, "[ExtraDamageFirstTarget]", flag, 0);
		string str7 = text;
		AbilityModPropertyEffectInfo targetHitEffectMod = this.m_targetHitEffectMod;
		string prefix7 = "[TargetHitEffect]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
		if (flag)
		{
			baseVal7 = valkyrieStab.m_targetHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(targetHitEffectMod, prefix7, showBaseVal7, baseVal7);
		text += base.PropDesc(this.m_perHitExtraAbsorbNextShieldBlock, "[PerHitExtraAbsorbNextShieldBlock]", flag, 0);
		return text + base.PropDesc(this.m_maxExtraAbsorbNextShieldBlock, "[MaxExtraAbsorbNextShieldBlock]", flag, 0);
	}
}
