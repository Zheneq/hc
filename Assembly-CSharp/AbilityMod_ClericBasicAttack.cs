using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_ClericBasicAttack : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyBool m_penetrateLineOfSightMod;
	public AbilityModPropertyFloat m_coneAngleMod;
	public AbilityModPropertyFloat m_coneLengthInnerMod;
	public AbilityModPropertyFloat m_coneLengthMod;
	public AbilityModPropertyFloat m_coneBackwardOffsetMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	[Header("-- On Hit Damage/Effect")]
	public AbilityModPropertyInt m_damageAmountInnerMod;
	public AbilityModPropertyInt m_damageAmountMod;
	public AbilityModPropertyEffectInfo m_targetHitEffectInnerMod;
	public AbilityModPropertyEffectInfo m_targetHitEffectMod;
	public AbilityModPropertyInt m_extraDamageToTargetsWhoEvaded;
	[Header("-- Cooldown Reduction")]
	public bool m_useCooldownReductionOverride;
	public AbilityModCooldownReduction m_cooldownReductionOverrideMod;
	public AbilityModPropertyInt m_hitsToIgnoreForCooldownReductionMultiplier;
	[Header("-- Ability interactions")]
	public AbilityModPropertyInt m_extraTechPointGainInAreaBuff;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClericBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClericBasicAttack clericBasicAttack = targetAbility as ClericBasicAttack;
		if (clericBasicAttack != null)
		{
			AddToken(tokens, m_coneAngleMod, "ConeAngle", string.Empty, clericBasicAttack.m_coneAngle);
			AddToken(tokens, m_coneLengthInnerMod, "ConeLengthInner", string.Empty, clericBasicAttack.m_coneLengthInner);
			AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, clericBasicAttack.m_coneLength);
			AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, clericBasicAttack.m_coneBackwardOffset);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, clericBasicAttack.m_maxTargets);
			AddToken(tokens, m_damageAmountInnerMod, "DamageAmountInner", string.Empty, clericBasicAttack.m_damageAmountInner);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, clericBasicAttack.m_damageAmount);
			AddToken_EffectMod(tokens, m_targetHitEffectInnerMod, "TargetHitEffectInner", clericBasicAttack.m_targetHitEffectInner);
			AddToken_EffectMod(tokens, m_targetHitEffectMod, "TargetHitEffect", clericBasicAttack.m_targetHitEffect);
			AddToken(tokens, m_extraDamageToTargetsWhoEvaded, "ExtraDamageToTargetsWhoEvaded", string.Empty, 0);
			if (m_useCooldownReductionOverride)
			{
				m_cooldownReductionOverrideMod.AddTooltipTokens(tokens, "CooldownReductionOverride");
				AddToken(tokens, m_hitsToIgnoreForCooldownReductionMultiplier, "HitsToIgnoreForCooldownReductionMultiplier", string.Empty, 0);
			}
			AddToken(tokens, m_extraTechPointGainInAreaBuff, "ExtraEnergyGainInAreaBuff", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericBasicAttack clericBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as ClericBasicAttack;
		bool isValid = clericBasicAttack != null;
		string desc = string.Empty;
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isValid, isValid && clericBasicAttack.m_penetrateLineOfSight);
		desc += PropDesc(m_coneAngleMod, "[ConeAngle]", isValid, isValid ? clericBasicAttack.m_coneAngle : 0f);
		desc += PropDesc(m_coneLengthInnerMod, "[ConeLengthInner]", isValid, (!isValid) ? 0f : clericBasicAttack.m_coneLengthInner);
		desc += PropDesc(m_coneLengthMod, "[ConeLength]", isValid, isValid ? clericBasicAttack.m_coneLength : 0f);
		desc += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", isValid, isValid ? clericBasicAttack.m_coneBackwardOffset : 0f);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? clericBasicAttack.m_maxTargets : 0);
		desc += PropDesc(m_damageAmountInnerMod, "[DamageAmountInner]", isValid, isValid ? clericBasicAttack.m_damageAmountInner : 0);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? clericBasicAttack.m_damageAmount : 0);
		desc += PropDesc(m_targetHitEffectInnerMod, "[TargetHitEffectInner]", isValid, isValid ? clericBasicAttack.m_targetHitEffectInner : null);
		desc += PropDesc(m_targetHitEffectMod, "[TargetHitEffect]", isValid, isValid ? clericBasicAttack.m_targetHitEffect : null);
		desc += PropDesc(m_extraDamageToTargetsWhoEvaded, "[ExtraDamageToTargetsWhoEvaded]", isValid);
		if (m_useCooldownReductionOverride)
		{
			desc += m_cooldownReductionOverrideMod.GetDescription(abilityData);
			desc += PropDesc(m_hitsToIgnoreForCooldownReductionMultiplier, "[HitsToIgnoreForCooldownReductionMultiplier]", isValid);
		}
		return new StringBuilder().Append(desc).Append(PropDesc(m_extraTechPointGainInAreaBuff, "[ExtraEnergyGainInAreaBuff]", isValid)).ToString();
	}
}
