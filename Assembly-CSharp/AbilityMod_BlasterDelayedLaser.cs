using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_BlasterDelayedLaser : AbilityMod
{
	[Header("-- Laser Data")]
	public AbilityModPropertyBool m_penetrateLineOfSightMod;
	public AbilityModPropertyFloat m_lengthMod;
	public AbilityModPropertyFloat m_widthMod;
	[Space(10f)]
	public AbilityModPropertyBool m_triggerAimAtBlasterMod;
	[Header("-- On Hit")]
	public AbilityModPropertyInt m_damageAmountMod;
	public AbilityModPropertyEffectInfo m_effectOnHitMod;
	public AbilityModPropertyInt m_extraDamageToNearEnemyMod;
	public AbilityModPropertyFloat m_nearDistanceMod;
	[Header("-- On Cast Hit Effect")]
	public AbilityModPropertyEffectInfo m_onCastEnemyHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(BlasterDelayedLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BlasterDelayedLaser blasterDelayedLaser = targetAbility as BlasterDelayedLaser;
		if (blasterDelayedLaser == null)
		{
			return;
		}
		AddToken(tokens, m_lengthMod, "Length", string.Empty, blasterDelayedLaser.m_length);
		AddToken(tokens, m_widthMod, "Width", string.Empty, blasterDelayedLaser.m_width);
		AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, blasterDelayedLaser.m_damageAmount);
		AddToken_EffectMod(tokens, m_effectOnHitMod, "EffectOnHit", blasterDelayedLaser.m_effectOnHit);
		AddToken(tokens, m_extraDamageToNearEnemyMod, "ExtraDamageToNearEnemy", string.Empty, blasterDelayedLaser.m_extraDamageToNearEnemy);
		AddToken(tokens, m_nearDistanceMod, "NearDistance", string.Empty, blasterDelayedLaser.m_nearDistance);
		if (m_nearDistanceMod != null)
		{
			AddToken_IntDiff(tokens, "NearDist_MinusOne", string.Empty, Mathf.RoundToInt(m_nearDistanceMod.GetModifiedValue(blasterDelayedLaser.m_nearDistance)) - 1, false, 0);
		}
		AddToken_EffectMod(tokens, m_onCastEnemyHitEffectMod, "OnCastEnemyHitEffect", blasterDelayedLaser.m_onCastEnemyHitEffect);
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterDelayedLaser blasterDelayedLaser = GetTargetAbilityOnAbilityData(abilityData) as BlasterDelayedLaser;
		bool isAbilityPresent = blasterDelayedLaser != null;
		string desc = string.Empty;
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isAbilityPresent, isAbilityPresent && blasterDelayedLaser.m_penetrateLineOfSight);
		desc += PropDesc(m_lengthMod, "[Length]", isAbilityPresent, isAbilityPresent ? blasterDelayedLaser.m_length : 0f);
		desc += PropDesc(m_widthMod, "[Width]", isAbilityPresent, isAbilityPresent ? blasterDelayedLaser.m_width : 0f);
		desc += PropDesc(m_triggerAimAtBlasterMod, "[TriggerAimAtBlaster]", isAbilityPresent, isAbilityPresent && blasterDelayedLaser.m_triggerAimAtBlaster);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isAbilityPresent, isAbilityPresent ? blasterDelayedLaser.m_damageAmount : 0);
		desc += PropDesc(m_effectOnHitMod, "[EffectOnHit]", isAbilityPresent, isAbilityPresent ? blasterDelayedLaser.m_effectOnHit : null);
		desc += PropDesc(m_extraDamageToNearEnemyMod, "[ExtraDamageToNearEnemy]", isAbilityPresent, isAbilityPresent ? blasterDelayedLaser.m_extraDamageToNearEnemy : 0);
		desc += PropDesc(m_nearDistanceMod, "[NearDistance]", isAbilityPresent, isAbilityPresent ? blasterDelayedLaser.m_nearDistance : 0f);
		return new StringBuilder().Append(desc).Append(PropDesc(m_onCastEnemyHitEffectMod, "[OnCastEnemyHitEffect]", isAbilityPresent, isAbilityPresent ? blasterDelayedLaser.m_onCastEnemyHitEffect : null)).ToString();
	}
}
