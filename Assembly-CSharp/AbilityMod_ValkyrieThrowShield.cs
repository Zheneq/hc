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
			AbilityMod.AddToken(tokens, this.m_widthMod, "Width", string.Empty, valkyrieThrowShield.m_width, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxDistancePerBounceMod, "MaxDistancePerBounce", string.Empty, valkyrieThrowShield.m_maxDistancePerBounce, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTotalDistanceMod, "MaxTotalDistance", string.Empty, valkyrieThrowShield.m_maxTotalDistance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxBouncesMod, "MaxBounces", string.Empty, valkyrieThrowShield.m_maxBounces, true, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsHitMod, "MaxTargetsHit", string.Empty, valkyrieThrowShield.m_maxTargetsHit, true, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, valkyrieThrowShield.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_bonusDamagePerBounceMod, "BonusDamagePerBounce", string.Empty, valkyrieThrowShield.m_bonusDamagePerBounce, true, false);
			AbilityMod.AddToken(tokens, this.m_lessDamagePerTargetMod, "LessDamagePerTarget", string.Empty, 0, true, false);
			tokens.Add(new TooltipTokenInt("LessDamagePerTarget_WithBaseValue", string.Empty, Mathf.FloorToInt((float)valkyrieThrowShield.m_damageAmount - this.m_lessDamagePerTargetMod.value)));
			AbilityMod.AddToken(tokens, this.m_knockbackDistanceMod, "KnockbackDistance", string.Empty, valkyrieThrowShield.m_knockbackDistance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_bonusKnockbackDistancePerBounceMod, "BonusKnockbackPerBounce", string.Empty, 0f, true, false, false);
			this.m_cooldownReductionOnLaserHitCaster.AddTooltipTokens(tokens, "CooldownReductionOnLaserHitCaster");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ValkyrieThrowShield valkyrieThrowShield = base.GetTargetAbilityOnAbilityData(abilityData) as ValkyrieThrowShield;
		bool flag = valkyrieThrowShield != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat widthMod = this.m_widthMod;
		string prefix = "[Width]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = valkyrieThrowShield.m_width;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(widthMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat maxDistancePerBounceMod = this.m_maxDistancePerBounceMod;
		string prefix2 = "[MaxDistancePerBounce]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = valkyrieThrowShield.m_maxDistancePerBounce;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(maxDistancePerBounceMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_maxTotalDistanceMod, "[MaxTotalDistance]", flag, (!flag) ? 0f : valkyrieThrowShield.m_maxTotalDistance);
		string str3 = text;
		AbilityModPropertyInt maxBouncesMod = this.m_maxBouncesMod;
		string prefix3 = "[MaxBounces]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = valkyrieThrowShield.m_maxBounces;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(maxBouncesMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt maxTargetsHitMod = this.m_maxTargetsHitMod;
		string prefix4 = "[MaxTargetsHit]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = valkyrieThrowShield.m_maxTargetsHit;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(maxTargetsHitMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix5 = "[DamageAmount]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = valkyrieThrowShield.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(damageAmountMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_bonusDamagePerBounceMod, "[BonusDamagePerBounce]", flag, (!flag) ? 0 : valkyrieThrowShield.m_bonusDamagePerBounce);
		text += base.PropDesc(this.m_lessDamagePerTargetMod, "[LessDamagePerTarget]", flag, 0);
		string str6 = text;
		AbilityModPropertyFloat knockbackDistanceMod = this.m_knockbackDistanceMod;
		string prefix6 = "[KnockbackDistance]";
		bool showBaseVal6 = flag;
		float baseVal6;
		if (flag)
		{
			baseVal6 = valkyrieThrowShield.m_knockbackDistance;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + base.PropDesc(knockbackDistanceMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_bonusKnockbackDistancePerBounceMod, "[BonusKnockbackPerBounce]", flag, 0f);
		string str7 = text;
		AbilityModPropertyKnockbackType knockbackTypeMod = this.m_knockbackTypeMod;
		string prefix7 = "[KnockbackType]";
		bool showBaseVal7 = flag;
		KnockbackType baseVal7;
		if (flag)
		{
			baseVal7 = valkyrieThrowShield.m_knockbackType;
		}
		else
		{
			baseVal7 = KnockbackType.AwayFromSource;
		}
		text = str7 + base.PropDesc(knockbackTypeMod, prefix7, showBaseVal7, baseVal7);
		text += base.PropDesc(this.m_bounceOnHitActorMod, "[BounceOnHitActor]", flag, false);
		text += base.PropDesc(this.m_maxKnockbackTargetsMod, "[MaxKnockbackTargets]", flag, 0);
		if (this.m_cooldownReductionOnLaserHitCaster.HasCooldownReduction())
		{
			text += this.m_cooldownReductionOnLaserHitCaster.GetDescription(abilityData);
		}
		return text;
	}
}
