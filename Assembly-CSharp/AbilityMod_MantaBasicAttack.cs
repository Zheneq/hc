using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_MantaBasicAttack : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_coneWidthAngleMod;
	public AbilityModPropertyFloat m_coneBackwardOffsetMod;
	public AbilityModPropertyFloat m_coneLengthInnerMod;
	public AbilityModPropertyFloat m_coneLengthThroughWallsMod;
	[Header("-- Damage")]
	public AbilityModPropertyInt m_damageAmountInnerMod;
	public AbilityModPropertyInt m_damageAmountThroughWallsMod;
	public AbilityModPropertyInt m_extraDamageNoLoSMod;
	public AbilityModPropertyEffectInfo m_effectInnerMod;
	public AbilityModPropertyEffectInfo m_effectOuterMod;
	[Header("-- Other")]
	public AbilityModPropertyEffectInfo m_additionalDirtyFightingExplosionEffect;
	public AbilityModPropertyBool m_disruptBrushInConeMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(MantaBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MantaBasicAttack mantaBasicAttack = targetAbility as MantaBasicAttack;
		if (mantaBasicAttack != null)
		{
			AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, mantaBasicAttack.m_coneWidthAngle);
			AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, mantaBasicAttack.m_coneBackwardOffset);
			AddToken(tokens, m_coneLengthInnerMod, "ConeLengthInner", string.Empty, mantaBasicAttack.m_coneLengthInner);
			AddToken(tokens, m_coneLengthThroughWallsMod, "ConeLengthThroughWalls", string.Empty, mantaBasicAttack.m_coneLengthThroughWalls);
			AddToken(tokens, m_damageAmountInnerMod, "DamageAmountInner", string.Empty, mantaBasicAttack.m_damageAmountInner);
			AddToken(tokens, m_damageAmountThroughWallsMod, "DamageAmountThroughWalls", string.Empty, mantaBasicAttack.m_damageAmountThroughWalls);
			AddToken(tokens, m_extraDamageNoLoSMod, "ExtraDamageNoLoS", string.Empty, 0);
			AddToken_EffectMod(tokens, m_effectInnerMod, "EffectInner", mantaBasicAttack.m_effectInner);
			AddToken_EffectMod(tokens, m_effectOuterMod, "EffectOuter", mantaBasicAttack.m_effectOuter);
			AddToken_EffectMod(tokens, m_additionalDirtyFightingExplosionEffect, "ExtraBugExplosionEffect");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MantaBasicAttack mantaBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as MantaBasicAttack;
		bool isValid = mantaBasicAttack != null;
		string desc = string.Empty;
		desc += PropDesc(m_coneWidthAngleMod, "[ConeWidthAngle]", isValid, isValid ? mantaBasicAttack.m_coneWidthAngle : 0f);
		desc += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", isValid, isValid ? mantaBasicAttack.m_coneBackwardOffset : 0f);
		desc += PropDesc(m_coneLengthInnerMod, "[ConeLengthInner]", isValid, isValid ? mantaBasicAttack.m_coneLengthInner : 0f);
		desc += PropDesc(m_coneLengthThroughWallsMod, "[ConeLengthThroughWalls]", isValid, isValid ? mantaBasicAttack.m_coneLengthThroughWalls : 0f);
		desc += PropDesc(m_damageAmountInnerMod, "[DamageAmountInner]", isValid, isValid ? mantaBasicAttack.m_damageAmountInner : 0);
		desc += PropDesc(m_damageAmountThroughWallsMod, "[DamageAmountThroughWalls]", isValid, isValid ? mantaBasicAttack.m_damageAmountThroughWalls : 0);
		desc += PropDesc(m_extraDamageNoLoSMod, "[ExtraDamageNoLoS]", isValid);
		desc += PropDesc(m_effectInnerMod, "[EffectInner]", isValid, isValid ? mantaBasicAttack.m_effectInner : null);
		desc += PropDesc(m_effectOuterMod, "[EffectOuter]", isValid, isValid ? mantaBasicAttack.m_effectOuter : null);
		desc += PropDesc(m_additionalDirtyFightingExplosionEffect, "[ExtraBugExplosionEffect]", isValid);
		return desc + PropDesc(m_disruptBrushInConeMod, "[DisruptBrushInCone]", isValid);
	}
}
