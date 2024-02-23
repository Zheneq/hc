using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken(tokens, m_coneWidthMinAngleMod, "ConeWidthMinAngle", string.Empty, valkyrieStab.m_coneWidthMinAngle);
			AddToken(tokens, m_coneWidthMaxAngleMod, "ConeWidthMaxAngle", string.Empty, valkyrieStab.m_coneWidthMaxAngle);
			AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, valkyrieStab.m_coneBackwardOffset);
			AddToken(tokens, m_coneMinLengthMod, "ConeMinLength", string.Empty, valkyrieStab.m_coneMinLength);
			AddToken(tokens, m_coneMaxLengthMod, "ConeMaxLength", string.Empty, valkyrieStab.m_coneMaxLength);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, valkyrieStab.m_maxTargets);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, valkyrieStab.m_damageAmount);
			AddToken(tokens, m_lessDamagePerTargetMod, "LessDamagePerTarget", string.Empty, valkyrieStab.m_lessDamagePerTarget);
			AddToken(tokens, m_extraDamageOnSpearTip, "ExtraDamageOnSpearTip", string.Empty, 0);
			AddToken(tokens, m_extraDamageFirstTarget, "ExtraDamageFirstTarget", string.Empty, 0);
			AddToken_EffectMod(tokens, m_targetHitEffectMod, "TargetHitEffect", valkyrieStab.m_targetHitEffect);
			AddToken(tokens, m_perHitExtraAbsorbNextShieldBlock, "PerHitExtraAbsorbNextShieldBlock", string.Empty, 0);
			AddToken(tokens, m_maxExtraAbsorbNextShieldBlock, "MaxExtraAbsorbNextShieldBlock", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ValkyrieStab valkyrieStab = GetTargetAbilityOnAbilityData(abilityData) as ValkyrieStab;
		bool isValid = valkyrieStab != null;
		string desc = string.Empty;
		desc += PropDesc(m_coneWidthMinAngleMod, "[ConeWidthMinAngle]", isValid, isValid ? valkyrieStab.m_coneWidthMinAngle : 0f);
		desc += PropDesc(m_coneWidthMaxAngleMod, "[ConeWidthMaxAngle]", isValid, isValid ? valkyrieStab.m_coneWidthMaxAngle : 0f);
		desc += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", isValid, isValid ? valkyrieStab.m_coneBackwardOffset : 0f);
		desc += PropDesc(m_coneMinLengthMod, "[ConeMinLength]", isValid, isValid ? valkyrieStab.m_coneMinLength : 0f);
		desc += PropDesc(m_coneMaxLengthMod, "[ConeMaxLength]", isValid, isValid ? valkyrieStab.m_coneMaxLength : 0f);
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isValid, isValid && valkyrieStab.m_penetrateLineOfSight);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? valkyrieStab.m_maxTargets : 0);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? valkyrieStab.m_damageAmount : 0);
		desc += PropDesc(m_lessDamagePerTargetMod, "[LessDamagePerTarget]", isValid, isValid ? valkyrieStab.m_lessDamagePerTarget : 0);
		desc += PropDesc(m_extraDamageOnSpearTip, "[ExtraDamageOnSpearTip]", isValid);
		desc += PropDesc(m_extraDamageFirstTarget, "[ExtraDamageFirstTarget]", isValid);
		desc += PropDesc(m_targetHitEffectMod, "[TargetHitEffect]", isValid, isValid ? valkyrieStab.m_targetHitEffect : null);
		desc += PropDesc(m_perHitExtraAbsorbNextShieldBlock, "[PerHitExtraAbsorbNextShieldBlock]", isValid);
		return new StringBuilder().Append(desc).Append(PropDesc(m_maxExtraAbsorbNextShieldBlock, "[MaxExtraAbsorbNextShieldBlock]", isValid)).ToString();
	}
}
