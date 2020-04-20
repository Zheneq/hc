using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClaymoreMultiRadiusCone : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_coneAngleMod;

	public AbilityModPropertyFloat m_coneInnerRadiusMod;

	public AbilityModPropertyFloat m_coneMiddleRadiusMod;

	public AbilityModPropertyFloat m_coneOuterRadiusMod;

	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	[Header("-- Damage")]
	public AbilityModPropertyInt m_innerDamageMod;

	public AbilityModPropertyInt m_middleDamageMod;

	public AbilityModPropertyInt m_outerDamageMod;

	[Header("-- Bonus Damage")]
	public AbilityModPropertyInt m_bonusDamageIfEnemyLowHealthMod;

	public AbilityModPropertyFloat m_enemyHealthThreshForBonusMod;

	public AbilityModPropertyInt m_bonusDamageIfCasterLowHealthMod;

	public AbilityModPropertyFloat m_casterHealthThreshForBonusMod;

	[Tooltip("for ~each~ X% below max HP, apply the bonus")]
	public bool m_applyBonusPerThresholdReached;

	[Header("-- Tech Point change on Hit")]
	public AbilityModPropertyInt m_innerTpGain;

	public AbilityModPropertyInt m_middleTpGain;

	public AbilityModPropertyInt m_outerTpGain;

	[Header("-- Effects")]
	public AbilityModPropertyEffectInfo m_effectInnerMod;

	public AbilityModPropertyEffectInfo m_effectMiddleMod;

	public AbilityModPropertyEffectInfo m_effectOuterMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClaymoreMultiRadiusCone);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClaymoreMultiRadiusCone claymoreMultiRadiusCone = targetAbility as ClaymoreMultiRadiusCone;
		if (claymoreMultiRadiusCone != null)
		{
			AbilityMod.AddToken(tokens, this.m_coneAngleMod, "ConeWidthAngle", string.Empty, claymoreMultiRadiusCone.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneInnerRadiusMod, "ConeLengthInner", string.Empty, claymoreMultiRadiusCone.m_coneLengthInner, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneMiddleRadiusMod, "ConeLengthMiddle", string.Empty, claymoreMultiRadiusCone.m_coneLengthMiddle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneOuterRadiusMod, "ConeLengthOuter", string.Empty, claymoreMultiRadiusCone.m_coneLengthOuter, true, false, false);
			AbilityMod.AddToken(tokens, this.m_innerDamageMod, "DamageAmountInner", string.Empty, claymoreMultiRadiusCone.m_damageAmountInner, true, false);
			AbilityMod.AddToken(tokens, this.m_middleDamageMod, "DamageAmountMiddle", string.Empty, claymoreMultiRadiusCone.m_damageAmountMiddle, true, false);
			AbilityMod.AddToken(tokens, this.m_outerDamageMod, "DamageAmountOuter", string.Empty, claymoreMultiRadiusCone.m_damageAmountOuter, true, false);
			AbilityMod.AddToken(tokens, this.m_bonusDamageIfEnemyLowHealthMod, "BonusDamageIfEnemyHealthBelow", string.Empty, claymoreMultiRadiusCone.m_bonusDamageIfEnemyLowHealth, true, false);
			AbilityMod.AddToken(tokens, this.m_enemyHealthThreshForBonusMod, "EnemyHealthThreshForBonus", string.Empty, claymoreMultiRadiusCone.m_enemyHealthThreshForBonus, true, false, true);
			AbilityMod.AddToken(tokens, this.m_bonusDamageIfCasterLowHealthMod, "BonusDamageIfCasterHealthBelow", string.Empty, claymoreMultiRadiusCone.m_bonusDamageIfCasterLowHealth, true, false);
			AbilityMod.AddToken(tokens, this.m_casterHealthThreshForBonusMod, "CasterHealthThreshForBonus", string.Empty, claymoreMultiRadiusCone.m_casterHealthThreshForBonus, true, false, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectInnerMod, "EffectInner", claymoreMultiRadiusCone.m_effectInner, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectMiddleMod, "EffectMiddle", claymoreMultiRadiusCone.m_effectMiddle, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOuterMod, "EffectOuter", claymoreMultiRadiusCone.m_effectOuter, true);
			AbilityMod.AddToken(tokens, this.m_innerTpGain, "TpGainInner", string.Empty, claymoreMultiRadiusCone.m_tpGainInner, true, false);
			AbilityMod.AddToken(tokens, this.m_middleTpGain, "TpGainMiddle", string.Empty, claymoreMultiRadiusCone.m_tpGainMiddle, true, false);
			AbilityMod.AddToken(tokens, this.m_outerTpGain, "TpGainOuter", string.Empty, claymoreMultiRadiusCone.m_tpGainOuter, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreMultiRadiusCone claymoreMultiRadiusCone = base.GetTargetAbilityOnAbilityData(abilityData) as ClaymoreMultiRadiusCone;
		bool flag = claymoreMultiRadiusCone != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat coneAngleMod = this.m_coneAngleMod;
		string prefix = "[ConeWidthAngle]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = claymoreMultiRadiusCone.m_coneWidthAngle;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(coneAngleMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat coneInnerRadiusMod = this.m_coneInnerRadiusMod;
		string prefix2 = "[ConeLengthInner]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = claymoreMultiRadiusCone.m_coneLengthInner;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(coneInnerRadiusMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat coneMiddleRadiusMod = this.m_coneMiddleRadiusMod;
		string prefix3 = "[ConeLengthMiddle]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = claymoreMultiRadiusCone.m_coneLengthMiddle;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(coneMiddleRadiusMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat coneOuterRadiusMod = this.m_coneOuterRadiusMod;
		string prefix4 = "[ConeLengthOuter]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = claymoreMultiRadiusCone.m_coneLengthOuter;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(coneOuterRadiusMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, flag && claymoreMultiRadiusCone.m_penetrateLineOfSight);
		text += base.PropDesc(this.m_innerDamageMod, "[DamageAmountInner]", flag, (!flag) ? 0 : claymoreMultiRadiusCone.m_damageAmountInner);
		string str5 = text;
		AbilityModPropertyInt middleDamageMod = this.m_middleDamageMod;
		string prefix5 = "[DamageAmountMiddle]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = claymoreMultiRadiusCone.m_damageAmountMiddle;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(middleDamageMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt outerDamageMod = this.m_outerDamageMod;
		string prefix6 = "[DamageAmountOuter]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = claymoreMultiRadiusCone.m_damageAmountOuter;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(outerDamageMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt bonusDamageIfEnemyLowHealthMod = this.m_bonusDamageIfEnemyLowHealthMod;
		string prefix7 = "[BonusDamageIfEnemyHealthBelow]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = claymoreMultiRadiusCone.m_bonusDamageIfEnemyLowHealth;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(bonusDamageIfEnemyLowHealthMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyFloat enemyHealthThreshForBonusMod = this.m_enemyHealthThreshForBonusMod;
		string prefix8 = "[EnemyHealthThreshForBonus]";
		bool showBaseVal8 = flag;
		float baseVal8;
		if (flag)
		{
			baseVal8 = claymoreMultiRadiusCone.m_enemyHealthThreshForBonus;
		}
		else
		{
			baseVal8 = 0f;
		}
		text = str8 + base.PropDesc(enemyHealthThreshForBonusMod, prefix8, showBaseVal8, baseVal8);
		text += base.PropDesc(this.m_bonusDamageIfCasterLowHealthMod, "[BonusDamageIfCasterHealthBelow]", flag, (!flag) ? 0 : claymoreMultiRadiusCone.m_bonusDamageIfCasterLowHealth);
		string str9 = text;
		AbilityModPropertyFloat casterHealthThreshForBonusMod = this.m_casterHealthThreshForBonusMod;
		string prefix9 = "[CasterHealthThreshForBonus]";
		bool showBaseVal9 = flag;
		float baseVal9;
		if (flag)
		{
			baseVal9 = claymoreMultiRadiusCone.m_casterHealthThreshForBonus;
		}
		else
		{
			baseVal9 = 0f;
		}
		text = str9 + base.PropDesc(casterHealthThreshForBonusMod, prefix9, showBaseVal9, baseVal9);
		if (this.m_applyBonusPerThresholdReached)
		{
			if (this.m_casterHealthThreshForBonusMod.operation != AbilityModPropertyFloat.ModOp.Ignore && this.m_bonusDamageIfCasterLowHealthMod.operation != AbilityModPropertyInt.ModOp.Ignore)
			{
				text += "\t{applied per [threshold]% below max hp}";
			}
		}
		string str10 = text;
		AbilityModPropertyInt innerTpGain = this.m_innerTpGain;
		string prefix10 = "[TpGainInner]";
		bool showBaseVal10 = flag;
		int baseVal10;
		if (flag)
		{
			baseVal10 = claymoreMultiRadiusCone.m_tpGainInner;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(innerTpGain, prefix10, showBaseVal10, baseVal10);
		text += base.PropDesc(this.m_middleTpGain, "[TpGainMiddle]", flag, (!flag) ? 0 : claymoreMultiRadiusCone.m_tpGainMiddle);
		string str11 = text;
		AbilityModPropertyInt outerTpGain = this.m_outerTpGain;
		string prefix11 = "[TpGainOuter]";
		bool showBaseVal11 = flag;
		int baseVal11;
		if (flag)
		{
			baseVal11 = claymoreMultiRadiusCone.m_tpGainOuter;
		}
		else
		{
			baseVal11 = 0;
		}
		text = str11 + base.PropDesc(outerTpGain, prefix11, showBaseVal11, baseVal11);
		text += base.PropDesc(this.m_effectInnerMod, "[EffectInner]", flag, (!flag) ? null : claymoreMultiRadiusCone.m_effectInner);
		string str12 = text;
		AbilityModPropertyEffectInfo effectMiddleMod = this.m_effectMiddleMod;
		string prefix12 = "[EffectMiddle]";
		bool showBaseVal12 = flag;
		StandardEffectInfo baseVal12;
		if (flag)
		{
			baseVal12 = claymoreMultiRadiusCone.m_effectMiddle;
		}
		else
		{
			baseVal12 = null;
		}
		text = str12 + base.PropDesc(effectMiddleMod, prefix12, showBaseVal12, baseVal12);
		string str13 = text;
		AbilityModPropertyEffectInfo effectOuterMod = this.m_effectOuterMod;
		string prefix13 = "[EffectOuter]";
		bool showBaseVal13 = flag;
		StandardEffectInfo baseVal13;
		if (flag)
		{
			baseVal13 = claymoreMultiRadiusCone.m_effectOuter;
		}
		else
		{
			baseVal13 = null;
		}
		return str13 + base.PropDesc(effectOuterMod, prefix13, showBaseVal13, baseVal13);
	}
}
