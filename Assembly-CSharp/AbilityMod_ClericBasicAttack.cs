using System;
using System.Collections.Generic;
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
		if (!(clericBasicAttack != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_coneAngleMod, "ConeAngle", string.Empty, clericBasicAttack.m_coneAngle);
			AbilityMod.AddToken(tokens, m_coneLengthInnerMod, "ConeLengthInner", string.Empty, clericBasicAttack.m_coneLengthInner);
			AbilityMod.AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, clericBasicAttack.m_coneLength);
			AbilityMod.AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, clericBasicAttack.m_coneBackwardOffset);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, clericBasicAttack.m_maxTargets);
			AbilityMod.AddToken(tokens, m_damageAmountInnerMod, "DamageAmountInner", string.Empty, clericBasicAttack.m_damageAmountInner);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, clericBasicAttack.m_damageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_targetHitEffectInnerMod, "TargetHitEffectInner", clericBasicAttack.m_targetHitEffectInner);
			AbilityMod.AddToken_EffectMod(tokens, m_targetHitEffectMod, "TargetHitEffect", clericBasicAttack.m_targetHitEffect);
			AbilityMod.AddToken(tokens, m_extraDamageToTargetsWhoEvaded, "ExtraDamageToTargetsWhoEvaded", string.Empty, 0);
			if (m_useCooldownReductionOverride)
			{
				m_cooldownReductionOverrideMod.AddTooltipTokens(tokens, "CooldownReductionOverride");
				AbilityMod.AddToken(tokens, m_hitsToIgnoreForCooldownReductionMultiplier, "HitsToIgnoreForCooldownReductionMultiplier", string.Empty, 0);
			}
			AbilityMod.AddToken(tokens, m_extraTechPointGainInAreaBuff, "ExtraEnergyGainInAreaBuff", string.Empty, 0);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericBasicAttack clericBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as ClericBasicAttack;
		bool flag = clericBasicAttack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool penetrateLineOfSightMod = m_penetrateLineOfSightMod;
		int baseVal;
		if (flag)
		{
			baseVal = (clericBasicAttack.m_penetrateLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, (byte)baseVal != 0);
		empty += PropDesc(m_coneAngleMod, "[ConeAngle]", flag, (!flag) ? 0f : clericBasicAttack.m_coneAngle);
		empty += PropDesc(m_coneLengthInnerMod, "[ConeLengthInner]", flag, (!flag) ? 0f : clericBasicAttack.m_coneLengthInner);
		string str2 = empty;
		AbilityModPropertyFloat coneLengthMod = m_coneLengthMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = clericBasicAttack.m_coneLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(coneLengthMod, "[ConeLength]", flag, baseVal2);
		empty += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, (!flag) ? 0f : clericBasicAttack.m_coneBackwardOffset);
		string str3 = empty;
		AbilityModPropertyInt maxTargetsMod = m_maxTargetsMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = clericBasicAttack.m_maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(maxTargetsMod, "[MaxTargets]", flag, baseVal3);
		empty += PropDesc(m_damageAmountInnerMod, "[DamageAmountInner]", flag, flag ? clericBasicAttack.m_damageAmountInner : 0);
		string str4 = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = clericBasicAttack.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectInfo targetHitEffectInnerMod = m_targetHitEffectInnerMod;
		object baseVal5;
		if (flag)
		{
			baseVal5 = clericBasicAttack.m_targetHitEffectInner;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(targetHitEffectInnerMod, "[TargetHitEffectInner]", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyEffectInfo targetHitEffectMod = m_targetHitEffectMod;
		object baseVal6;
		if (flag)
		{
			baseVal6 = clericBasicAttack.m_targetHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(targetHitEffectMod, "[TargetHitEffect]", flag, (StandardEffectInfo)baseVal6);
		empty += PropDesc(m_extraDamageToTargetsWhoEvaded, "[ExtraDamageToTargetsWhoEvaded]", flag);
		if (m_useCooldownReductionOverride)
		{
			empty += m_cooldownReductionOverrideMod.GetDescription(abilityData);
			empty += PropDesc(m_hitsToIgnoreForCooldownReductionMultiplier, "[HitsToIgnoreForCooldownReductionMultiplier]", flag);
		}
		return empty + PropDesc(m_extraTechPointGainInAreaBuff, "[ExtraEnergyGainInAreaBuff]", flag);
	}
}
