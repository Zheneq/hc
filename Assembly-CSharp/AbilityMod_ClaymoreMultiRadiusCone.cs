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
			AbilityMod.AddToken(tokens, m_coneAngleMod, "ConeWidthAngle", string.Empty, claymoreMultiRadiusCone.m_coneWidthAngle);
			AbilityMod.AddToken(tokens, m_coneInnerRadiusMod, "ConeLengthInner", string.Empty, claymoreMultiRadiusCone.m_coneLengthInner);
			AbilityMod.AddToken(tokens, m_coneMiddleRadiusMod, "ConeLengthMiddle", string.Empty, claymoreMultiRadiusCone.m_coneLengthMiddle);
			AbilityMod.AddToken(tokens, m_coneOuterRadiusMod, "ConeLengthOuter", string.Empty, claymoreMultiRadiusCone.m_coneLengthOuter);
			AbilityMod.AddToken(tokens, m_innerDamageMod, "DamageAmountInner", string.Empty, claymoreMultiRadiusCone.m_damageAmountInner);
			AbilityMod.AddToken(tokens, m_middleDamageMod, "DamageAmountMiddle", string.Empty, claymoreMultiRadiusCone.m_damageAmountMiddle);
			AbilityMod.AddToken(tokens, m_outerDamageMod, "DamageAmountOuter", string.Empty, claymoreMultiRadiusCone.m_damageAmountOuter);
			AbilityMod.AddToken(tokens, m_bonusDamageIfEnemyLowHealthMod, "BonusDamageIfEnemyHealthBelow", string.Empty, claymoreMultiRadiusCone.m_bonusDamageIfEnemyLowHealth);
			AbilityMod.AddToken(tokens, m_enemyHealthThreshForBonusMod, "EnemyHealthThreshForBonus", string.Empty, claymoreMultiRadiusCone.m_enemyHealthThreshForBonus, true, false, true);
			AbilityMod.AddToken(tokens, m_bonusDamageIfCasterLowHealthMod, "BonusDamageIfCasterHealthBelow", string.Empty, claymoreMultiRadiusCone.m_bonusDamageIfCasterLowHealth);
			AbilityMod.AddToken(tokens, m_casterHealthThreshForBonusMod, "CasterHealthThreshForBonus", string.Empty, claymoreMultiRadiusCone.m_casterHealthThreshForBonus, true, false, true);
			AbilityMod.AddToken_EffectMod(tokens, m_effectInnerMod, "EffectInner", claymoreMultiRadiusCone.m_effectInner);
			AbilityMod.AddToken_EffectMod(tokens, m_effectMiddleMod, "EffectMiddle", claymoreMultiRadiusCone.m_effectMiddle);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOuterMod, "EffectOuter", claymoreMultiRadiusCone.m_effectOuter);
			AbilityMod.AddToken(tokens, m_innerTpGain, "TpGainInner", string.Empty, claymoreMultiRadiusCone.m_tpGainInner);
			AbilityMod.AddToken(tokens, m_middleTpGain, "TpGainMiddle", string.Empty, claymoreMultiRadiusCone.m_tpGainMiddle);
			AbilityMod.AddToken(tokens, m_outerTpGain, "TpGainOuter", string.Empty, claymoreMultiRadiusCone.m_tpGainOuter);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreMultiRadiusCone claymoreMultiRadiusCone = GetTargetAbilityOnAbilityData(abilityData) as ClaymoreMultiRadiusCone;
		bool flag = claymoreMultiRadiusCone != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat coneAngleMod = m_coneAngleMod;
		float baseVal;
		if (flag)
		{
			baseVal = claymoreMultiRadiusCone.m_coneWidthAngle;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(coneAngleMod, "[ConeWidthAngle]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat coneInnerRadiusMod = m_coneInnerRadiusMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = claymoreMultiRadiusCone.m_coneLengthInner;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(coneInnerRadiusMod, "[ConeLengthInner]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat coneMiddleRadiusMod = m_coneMiddleRadiusMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = claymoreMultiRadiusCone.m_coneLengthMiddle;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(coneMiddleRadiusMod, "[ConeLengthMiddle]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat coneOuterRadiusMod = m_coneOuterRadiusMod;
		float baseVal4;
		if (flag)
		{
			baseVal4 = claymoreMultiRadiusCone.m_coneLengthOuter;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(coneOuterRadiusMod, "[ConeLengthOuter]", flag, baseVal4);
		empty += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, flag && claymoreMultiRadiusCone.m_penetrateLineOfSight);
		empty += PropDesc(m_innerDamageMod, "[DamageAmountInner]", flag, flag ? claymoreMultiRadiusCone.m_damageAmountInner : 0);
		string str5 = empty;
		AbilityModPropertyInt middleDamageMod = m_middleDamageMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = claymoreMultiRadiusCone.m_damageAmountMiddle;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(middleDamageMod, "[DamageAmountMiddle]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt outerDamageMod = m_outerDamageMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = claymoreMultiRadiusCone.m_damageAmountOuter;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(outerDamageMod, "[DamageAmountOuter]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyInt bonusDamageIfEnemyLowHealthMod = m_bonusDamageIfEnemyLowHealthMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = claymoreMultiRadiusCone.m_bonusDamageIfEnemyLowHealth;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(bonusDamageIfEnemyLowHealthMod, "[BonusDamageIfEnemyHealthBelow]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyFloat enemyHealthThreshForBonusMod = m_enemyHealthThreshForBonusMod;
		float baseVal8;
		if (flag)
		{
			baseVal8 = claymoreMultiRadiusCone.m_enemyHealthThreshForBonus;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str8 + PropDesc(enemyHealthThreshForBonusMod, "[EnemyHealthThreshForBonus]", flag, baseVal8);
		empty += PropDesc(m_bonusDamageIfCasterLowHealthMod, "[BonusDamageIfCasterHealthBelow]", flag, flag ? claymoreMultiRadiusCone.m_bonusDamageIfCasterLowHealth : 0);
		string str9 = empty;
		AbilityModPropertyFloat casterHealthThreshForBonusMod = m_casterHealthThreshForBonusMod;
		float baseVal9;
		if (flag)
		{
			baseVal9 = claymoreMultiRadiusCone.m_casterHealthThreshForBonus;
		}
		else
		{
			baseVal9 = 0f;
		}
		empty = str9 + PropDesc(casterHealthThreshForBonusMod, "[CasterHealthThreshForBonus]", flag, baseVal9);
		if (m_applyBonusPerThresholdReached)
		{
			if (m_casterHealthThreshForBonusMod.operation != 0 && m_bonusDamageIfCasterLowHealthMod.operation != 0)
			{
				empty += "\t{applied per [threshold]% below max hp}";
			}
		}
		string str10 = empty;
		AbilityModPropertyInt innerTpGain = m_innerTpGain;
		int baseVal10;
		if (flag)
		{
			baseVal10 = claymoreMultiRadiusCone.m_tpGainInner;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(innerTpGain, "[TpGainInner]", flag, baseVal10);
		empty += PropDesc(m_middleTpGain, "[TpGainMiddle]", flag, flag ? claymoreMultiRadiusCone.m_tpGainMiddle : 0);
		string str11 = empty;
		AbilityModPropertyInt outerTpGain = m_outerTpGain;
		int baseVal11;
		if (flag)
		{
			baseVal11 = claymoreMultiRadiusCone.m_tpGainOuter;
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str11 + PropDesc(outerTpGain, "[TpGainOuter]", flag, baseVal11);
		empty += PropDesc(m_effectInnerMod, "[EffectInner]", flag, (!flag) ? null : claymoreMultiRadiusCone.m_effectInner);
		string str12 = empty;
		AbilityModPropertyEffectInfo effectMiddleMod = m_effectMiddleMod;
		object baseVal12;
		if (flag)
		{
			baseVal12 = claymoreMultiRadiusCone.m_effectMiddle;
		}
		else
		{
			baseVal12 = null;
		}
		empty = str12 + PropDesc(effectMiddleMod, "[EffectMiddle]", flag, (StandardEffectInfo)baseVal12);
		string str13 = empty;
		AbilityModPropertyEffectInfo effectOuterMod = m_effectOuterMod;
		object baseVal13;
		if (flag)
		{
			baseVal13 = claymoreMultiRadiusCone.m_effectOuter;
		}
		else
		{
			baseVal13 = null;
		}
		return str13 + PropDesc(effectOuterMod, "[EffectOuter]", flag, (StandardEffectInfo)baseVal13);
	}
}
