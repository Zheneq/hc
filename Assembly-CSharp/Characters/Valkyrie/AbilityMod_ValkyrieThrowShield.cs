// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ValkyrieThrowShield : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_widthMod;
	public AbilityModPropertyFloat m_maxDistancePerBounceMod;
	public AbilityModPropertyFloat m_maxTotalDistanceMod;
	public AbilityModPropertyInt m_maxBouncesMod;
	public AbilityModPropertyInt m_maxTargetsHitMod;
	public AbilityModPropertyBool m_bounceOnHitActorMod;
	[Header("-- Damage")]
	public AbilityModPropertyInt m_damageAmountMod;
	public AbilityModPropertyInt m_bonusDamagePerBounceMod;
	public AbilityModPropertyInt m_lessDamagePerTargetMod;
	[Header("-- Knockback")]
	public AbilityModPropertyFloat m_knockbackDistanceMod;
	public AbilityModPropertyFloat m_bonusKnockbackDistancePerBounceMod;
	public AbilityModPropertyKnockbackType m_knockbackTypeMod;
	public AbilityModPropertyInt m_maxKnockbackTargetsMod;
	[Header("-- Cooldown reduction")]
	public AbilityModCooldownReduction m_cooldownReductionOnLaserHitCaster;

	public override Type GetTargetAbilityType()
	{
		return typeof(ValkyrieThrowShield);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ValkyrieThrowShield valkyrieThrowShield = targetAbility as ValkyrieThrowShield;
		if (valkyrieThrowShield != null)
		{
			AddToken(tokens, m_widthMod, "Width", string.Empty, valkyrieThrowShield.m_width);
			AddToken(tokens, m_maxDistancePerBounceMod, "MaxDistancePerBounce", string.Empty, valkyrieThrowShield.m_maxDistancePerBounce);
			AddToken(tokens, m_maxTotalDistanceMod, "MaxTotalDistance", string.Empty, valkyrieThrowShield.m_maxTotalDistance);
			AddToken(tokens, m_maxBouncesMod, "MaxBounces", string.Empty, valkyrieThrowShield.m_maxBounces);
			AddToken(tokens, m_maxTargetsHitMod, "MaxTargetsHit", string.Empty, valkyrieThrowShield.m_maxTargetsHit);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, valkyrieThrowShield.m_damageAmount);
			AddToken(tokens, m_bonusDamagePerBounceMod, "BonusDamagePerBounce", string.Empty, valkyrieThrowShield.m_bonusDamagePerBounce);
			AddToken(tokens, m_lessDamagePerTargetMod, "LessDamagePerTarget", string.Empty, 0);
			tokens.Add(new TooltipTokenInt("LessDamagePerTarget_WithBaseValue", string.Empty, Mathf.FloorToInt(valkyrieThrowShield.m_damageAmount - m_lessDamagePerTargetMod.value)));
			AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", string.Empty, valkyrieThrowShield.m_knockbackDistance);
			AddToken(tokens, m_bonusKnockbackDistancePerBounceMod, "BonusKnockbackPerBounce", string.Empty, 0f);
			m_cooldownReductionOnLaserHitCaster.AddTooltipTokens(tokens, "CooldownReductionOnLaserHitCaster");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		ValkyrieThrowShield valkyrieThrowShield = GetTargetAbilityOnAbilityData(abilityData) as ValkyrieThrowShield;
		// rogues
		// ValkyrieThrowShield valkyrieThrowShield = targetAbility as ValkyrieThrowShield;
		bool isValid = valkyrieThrowShield != null;
		string desc = string.Empty;
		desc += PropDesc(m_widthMod, "[Width]", isValid, isValid ? valkyrieThrowShield.m_width : 0f); // prefix : "[ConeWidthMinAngle]" in rogues
		desc += PropDesc(m_maxDistancePerBounceMod, "[MaxDistancePerBounce]", isValid, isValid ? valkyrieThrowShield.m_maxDistancePerBounce : 0f);
		desc += PropDesc(m_maxTotalDistanceMod, "[MaxTotalDistance]", isValid, isValid ? valkyrieThrowShield.m_maxTotalDistance : 0f);
		desc += PropDesc(m_maxBouncesMod, "[MaxBounces]", isValid, isValid ? valkyrieThrowShield.m_maxBounces : 0);
		desc += PropDesc(m_maxTargetsHitMod, "[MaxTargetsHit]", isValid, isValid ? valkyrieThrowShield.m_maxTargetsHit : 0);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? valkyrieThrowShield.m_damageAmount : 0);
		desc += PropDesc(m_bonusDamagePerBounceMod, "[BonusDamagePerBounce]", isValid, isValid ? valkyrieThrowShield.m_bonusDamagePerBounce : 0);
		desc += PropDesc(m_lessDamagePerTargetMod, "[LessDamagePerTarget]", isValid);
		desc += PropDesc(m_knockbackDistanceMod, "[KnockbackDistance]", isValid, isValid ? valkyrieThrowShield.m_knockbackDistance : 0f);
		desc += PropDesc(m_bonusKnockbackDistancePerBounceMod, "[BonusKnockbackPerBounce]", isValid);
		desc += PropDesc(m_knockbackTypeMod, "[KnockbackType]", isValid, isValid ? valkyrieThrowShield.m_knockbackType : KnockbackType.AwayFromSource);
		desc += PropDesc(m_bounceOnHitActorMod, "[BounceOnHitActor]", isValid);
		desc += PropDesc(m_maxKnockbackTargetsMod, "[MaxKnockbackTargets]", isValid);
		if (m_cooldownReductionOnLaserHitCaster.HasCooldownReduction())
		{
			desc += m_cooldownReductionOnLaserHitCaster.GetDescription(abilityData);
		}
		return desc;
	}
}
