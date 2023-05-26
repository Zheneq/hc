using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClericMeleeKnockback : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyBool m_penetrateLineOfSightMod;
	public AbilityModPropertyFloat m_minSeparationBetweenAoeAndCasterMod;
	public AbilityModPropertyFloat m_maxSeparationBetweenAoeAndCasterMod;
	public AbilityModPropertyFloat m_aoeRadiusMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	[Header("-- On Hit Damage/Effect")]
	public AbilityModPropertyInt m_damageAmountMod;
	public AbilityModPropertyFloat m_knockbackDistanceMod;
	public AbilityModPropertyKnockbackType m_knockbackTypeMod;
	public AbilityModPropertyEffectInfo m_targetHitEffectMod;
	public AbilityModPropertyEffectInfo m_singleTargetHitEffectMod;
	public AbilityModPropertyInt m_extraTechPointsPerHitWithAreaBuff;
	[Separator("Connecting Laser between caster and aoe center", true)]
	public AbilityModPropertyFloat m_connectLaserWidthMod;
	public AbilityModPropertyInt m_connectLaserDamageMod;
	public AbilityModPropertyEffectInfo m_connectLaserEnemyHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClericMeleeKnockback);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClericMeleeKnockback clericMeleeKnockback = targetAbility as ClericMeleeKnockback;
		if (clericMeleeKnockback != null)
		{
			AddToken(tokens, m_minSeparationBetweenAoeAndCasterMod, "MinSeparationBetweenAoeAndCaster", string.Empty, clericMeleeKnockback.m_minSeparationBetweenAoeAndCaster);
			AddToken(tokens, m_maxSeparationBetweenAoeAndCasterMod, "MaxSeparationBetweenAoeAndCaster", string.Empty, clericMeleeKnockback.m_maxSeparationBetweenAoeAndCaster);
			AddToken(tokens, m_aoeRadiusMod, "AoeRadius", string.Empty, clericMeleeKnockback.m_aoeRadius);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, clericMeleeKnockback.m_maxTargets);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, clericMeleeKnockback.m_damageAmount);
			AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", string.Empty, clericMeleeKnockback.m_knockbackDistance);
			AddToken_EffectMod(tokens, m_targetHitEffectMod, "TargetHitEffect", clericMeleeKnockback.m_targetHitEffect);
			AddToken_EffectMod(tokens, m_singleTargetHitEffectMod, "SingleTargetHitEffect", clericMeleeKnockback.m_targetHitEffect);
			AddToken(tokens, m_extraTechPointsPerHitWithAreaBuff, "ExtraEnergyPerHitWithAreaBuff", string.Empty, 0);
			AddToken(tokens, m_connectLaserWidthMod, "ConnectLaserWidth", string.Empty, clericMeleeKnockback.m_connectLaserWidth);
			AddToken(tokens, m_connectLaserDamageMod, "ConnectLaserDamage", string.Empty, clericMeleeKnockback.m_connectLaserDamage);
			AddToken_EffectMod(tokens, m_connectLaserEnemyHitEffectMod, "ConnectLaserEnemyHitEffect", clericMeleeKnockback.m_connectLaserEnemyHitEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericMeleeKnockback clericMeleeKnockback = GetTargetAbilityOnAbilityData(abilityData) as ClericMeleeKnockback;
		bool isValid = clericMeleeKnockback != null;
		string desc = string.Empty;
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isValid, isValid && clericMeleeKnockback.m_penetrateLineOfSight);
		desc += PropDesc(m_minSeparationBetweenAoeAndCasterMod, "[MinSeparationBetweenAoeAndCaster]", isValid, isValid ? clericMeleeKnockback.m_minSeparationBetweenAoeAndCaster : 0f);
		desc += PropDesc(m_maxSeparationBetweenAoeAndCasterMod, "[MaxSeparationBetweenAoeAndCaster]", isValid, isValid ? clericMeleeKnockback.m_maxSeparationBetweenAoeAndCaster : 0f);
		desc += PropDesc(m_aoeRadiusMod, "[AoeRadius]", isValid, isValid ? clericMeleeKnockback.m_aoeRadius : 0f);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? clericMeleeKnockback.m_maxTargets : 0);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? clericMeleeKnockback.m_damageAmount : 0);
		desc += PropDesc(m_knockbackDistanceMod, "[KnockbackDistance]", isValid, isValid ? clericMeleeKnockback.m_knockbackDistance : 0f);
		desc += PropDesc(m_knockbackTypeMod, "[KnockbackType]", isValid, isValid ? clericMeleeKnockback.m_knockbackType : KnockbackType.AwayFromSource);
		desc += PropDesc(m_targetHitEffectMod, "[TargetHitEffect]", isValid, isValid ? clericMeleeKnockback.m_targetHitEffect : null);
		desc += PropDesc(m_extraTechPointsPerHitWithAreaBuff, "[ExtraEnergyPerHitWithAreaBuff]", isValid);
		desc += PropDesc(m_connectLaserWidthMod, "[ConnectLaserWidth]", isValid, isValid ? clericMeleeKnockback.m_connectLaserWidth : 0f);
		desc += PropDesc(m_connectLaserDamageMod, "[ConnectLaserDamage]", isValid, isValid ? clericMeleeKnockback.m_connectLaserDamage : 0);
		return desc + PropDesc(m_connectLaserEnemyHitEffectMod, "[ConnectLaserEnemyHitEffect]", isValid, isValid ? clericMeleeKnockback.m_connectLaserEnemyHitEffect : null);
	}
}
