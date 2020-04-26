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
		if (!(valkyrieThrowShield != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_widthMod, "Width", string.Empty, valkyrieThrowShield.m_width);
			AbilityMod.AddToken(tokens, m_maxDistancePerBounceMod, "MaxDistancePerBounce", string.Empty, valkyrieThrowShield.m_maxDistancePerBounce);
			AbilityMod.AddToken(tokens, m_maxTotalDistanceMod, "MaxTotalDistance", string.Empty, valkyrieThrowShield.m_maxTotalDistance);
			AbilityMod.AddToken(tokens, m_maxBouncesMod, "MaxBounces", string.Empty, valkyrieThrowShield.m_maxBounces);
			AbilityMod.AddToken(tokens, m_maxTargetsHitMod, "MaxTargetsHit", string.Empty, valkyrieThrowShield.m_maxTargetsHit);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, valkyrieThrowShield.m_damageAmount);
			AbilityMod.AddToken(tokens, m_bonusDamagePerBounceMod, "BonusDamagePerBounce", string.Empty, valkyrieThrowShield.m_bonusDamagePerBounce);
			AbilityMod.AddToken(tokens, m_lessDamagePerTargetMod, "LessDamagePerTarget", string.Empty, 0);
			tokens.Add(new TooltipTokenInt("LessDamagePerTarget_WithBaseValue", string.Empty, Mathf.FloorToInt((float)valkyrieThrowShield.m_damageAmount - m_lessDamagePerTargetMod.value)));
			AbilityMod.AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", string.Empty, valkyrieThrowShield.m_knockbackDistance);
			AbilityMod.AddToken(tokens, m_bonusKnockbackDistancePerBounceMod, "BonusKnockbackPerBounce", string.Empty, 0f);
			m_cooldownReductionOnLaserHitCaster.AddTooltipTokens(tokens, "CooldownReductionOnLaserHitCaster");
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ValkyrieThrowShield valkyrieThrowShield = GetTargetAbilityOnAbilityData(abilityData) as ValkyrieThrowShield;
		bool flag = valkyrieThrowShield != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat widthMod = m_widthMod;
		float baseVal;
		if (flag)
		{
			baseVal = valkyrieThrowShield.m_width;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(widthMod, "[Width]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat maxDistancePerBounceMod = m_maxDistancePerBounceMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = valkyrieThrowShield.m_maxDistancePerBounce;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(maxDistancePerBounceMod, "[MaxDistancePerBounce]", flag, baseVal2);
		empty += PropDesc(m_maxTotalDistanceMod, "[MaxTotalDistance]", flag, (!flag) ? 0f : valkyrieThrowShield.m_maxTotalDistance);
		string str3 = empty;
		AbilityModPropertyInt maxBouncesMod = m_maxBouncesMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = valkyrieThrowShield.m_maxBounces;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(maxBouncesMod, "[MaxBounces]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyInt maxTargetsHitMod = m_maxTargetsHitMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = valkyrieThrowShield.m_maxTargetsHit;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(maxTargetsHitMod, "[MaxTargetsHit]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = valkyrieThrowShield.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal5);
		empty += PropDesc(m_bonusDamagePerBounceMod, "[BonusDamagePerBounce]", flag, flag ? valkyrieThrowShield.m_bonusDamagePerBounce : 0);
		empty += PropDesc(m_lessDamagePerTargetMod, "[LessDamagePerTarget]", flag);
		string str6 = empty;
		AbilityModPropertyFloat knockbackDistanceMod = m_knockbackDistanceMod;
		float baseVal6;
		if (flag)
		{
			baseVal6 = valkyrieThrowShield.m_knockbackDistance;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + PropDesc(knockbackDistanceMod, "[KnockbackDistance]", flag, baseVal6);
		empty += PropDesc(m_bonusKnockbackDistancePerBounceMod, "[BonusKnockbackPerBounce]", flag);
		string str7 = empty;
		AbilityModPropertyKnockbackType knockbackTypeMod = m_knockbackTypeMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = (int)valkyrieThrowShield.m_knockbackType;
		}
		else
		{
			baseVal7 = 4;
		}
		empty = str7 + PropDesc(knockbackTypeMod, "[KnockbackType]", flag, (KnockbackType)baseVal7);
		empty += PropDesc(m_bounceOnHitActorMod, "[BounceOnHitActor]", flag);
		empty += PropDesc(m_maxKnockbackTargetsMod, "[MaxKnockbackTargets]", flag);
		if (m_cooldownReductionOnLaserHitCaster.HasCooldownReduction())
		{
			empty += m_cooldownReductionOnLaserHitCaster.GetDescription(abilityData);
		}
		return empty;
	}
}
