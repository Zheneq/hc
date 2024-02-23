using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken(tokens, m_coneAngleMod, "ConeWidthAngle", string.Empty, claymoreMultiRadiusCone.m_coneWidthAngle);
			AddToken(tokens, m_coneInnerRadiusMod, "ConeLengthInner", string.Empty, claymoreMultiRadiusCone.m_coneLengthInner);
			AddToken(tokens, m_coneMiddleRadiusMod, "ConeLengthMiddle", string.Empty, claymoreMultiRadiusCone.m_coneLengthMiddle);
			AddToken(tokens, m_coneOuterRadiusMod, "ConeLengthOuter", string.Empty, claymoreMultiRadiusCone.m_coneLengthOuter);
			AddToken(tokens, m_innerDamageMod, "DamageAmountInner", string.Empty, claymoreMultiRadiusCone.m_damageAmountInner);
			AddToken(tokens, m_middleDamageMod, "DamageAmountMiddle", string.Empty, claymoreMultiRadiusCone.m_damageAmountMiddle);
			AddToken(tokens, m_outerDamageMod, "DamageAmountOuter", string.Empty, claymoreMultiRadiusCone.m_damageAmountOuter);
			AddToken(tokens, m_bonusDamageIfEnemyLowHealthMod, "BonusDamageIfEnemyHealthBelow", string.Empty, claymoreMultiRadiusCone.m_bonusDamageIfEnemyLowHealth);
			AddToken(tokens, m_enemyHealthThreshForBonusMod, "EnemyHealthThreshForBonus", string.Empty, claymoreMultiRadiusCone.m_enemyHealthThreshForBonus, true, false, true);
			AddToken(tokens, m_bonusDamageIfCasterLowHealthMod, "BonusDamageIfCasterHealthBelow", string.Empty, claymoreMultiRadiusCone.m_bonusDamageIfCasterLowHealth);
			AddToken(tokens, m_casterHealthThreshForBonusMod, "CasterHealthThreshForBonus", string.Empty, claymoreMultiRadiusCone.m_casterHealthThreshForBonus, true, false, true);
			AddToken_EffectMod(tokens, m_effectInnerMod, "EffectInner", claymoreMultiRadiusCone.m_effectInner);
			AddToken_EffectMod(tokens, m_effectMiddleMod, "EffectMiddle", claymoreMultiRadiusCone.m_effectMiddle);
			AddToken_EffectMod(tokens, m_effectOuterMod, "EffectOuter", claymoreMultiRadiusCone.m_effectOuter);
			AddToken(tokens, m_innerTpGain, "TpGainInner", string.Empty, claymoreMultiRadiusCone.m_tpGainInner);
			AddToken(tokens, m_middleTpGain, "TpGainMiddle", string.Empty, claymoreMultiRadiusCone.m_tpGainMiddle);
			AddToken(tokens, m_outerTpGain, "TpGainOuter", string.Empty, claymoreMultiRadiusCone.m_tpGainOuter);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreMultiRadiusCone claymoreMultiRadiusCone = GetTargetAbilityOnAbilityData(abilityData) as ClaymoreMultiRadiusCone;
		bool isAbilityPresent = claymoreMultiRadiusCone != null;
		string desc = string.Empty;
		desc += PropDesc(m_coneAngleMod, "[ConeWidthAngle]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_coneWidthAngle : 0f);
		desc += PropDesc(m_coneInnerRadiusMod, "[ConeLengthInner]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_coneLengthInner : 0f);
		desc += PropDesc(m_coneMiddleRadiusMod, "[ConeLengthMiddle]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_coneLengthMiddle : 0f);
		desc += PropDesc(m_coneOuterRadiusMod, "[ConeLengthOuter]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_coneLengthOuter : 0f);
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isAbilityPresent, isAbilityPresent && claymoreMultiRadiusCone.m_penetrateLineOfSight);
		desc += PropDesc(m_innerDamageMod, "[DamageAmountInner]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_damageAmountInner : 0);
		desc += PropDesc(m_middleDamageMod, "[DamageAmountMiddle]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_damageAmountMiddle : 0);
		desc += PropDesc(m_outerDamageMod, "[DamageAmountOuter]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_damageAmountOuter : 0);
		desc += PropDesc(m_bonusDamageIfEnemyLowHealthMod, "[BonusDamageIfEnemyHealthBelow]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_bonusDamageIfEnemyLowHealth : 0);
		desc += PropDesc(m_enemyHealthThreshForBonusMod, "[EnemyHealthThreshForBonus]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_enemyHealthThreshForBonus : 0f);
		desc += PropDesc(m_bonusDamageIfCasterLowHealthMod, "[BonusDamageIfCasterHealthBelow]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_bonusDamageIfCasterLowHealth : 0);
		desc += PropDesc(m_casterHealthThreshForBonusMod, "[CasterHealthThreshForBonus]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_casterHealthThreshForBonus : 0f);
		if (m_applyBonusPerThresholdReached
		    && m_casterHealthThreshForBonusMod.operation != 0
		    && m_bonusDamageIfCasterLowHealthMod.operation != 0)
		{
			desc += "\t{applied per [threshold]% below max hp}";
		}
		desc += PropDesc(m_innerTpGain, "[TpGainInner]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_tpGainInner : 0);
		desc += PropDesc(m_middleTpGain, "[TpGainMiddle]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_tpGainMiddle : 0);
		desc += PropDesc(m_outerTpGain, "[TpGainOuter]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_tpGainOuter : 0);
		desc += PropDesc(m_effectInnerMod, "[EffectInner]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_effectInner : null);
		desc += PropDesc(m_effectMiddleMod, "[EffectMiddle]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_effectMiddle : null);
		return new StringBuilder().Append(desc).Append(PropDesc(m_effectOuterMod, "[EffectOuter]", isAbilityPresent, isAbilityPresent ? claymoreMultiRadiusCone.m_effectOuter : null)).ToString();
	}
}
