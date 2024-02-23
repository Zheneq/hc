using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_ClaymoreSilenceLaser : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_laserRangeMod;
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyInt m_laserMaxTargetsMod;
	public AbilityModPropertyBool m_penetrateLosMod;
	[Header("-- Damage and Effect")]
	public AbilityModPropertyInt m_effectExplosionDamageMod;
	public AbilityModPropertyInt m_explosionDamageAfterFirstHitMod;
	public AbilityModPropertyEffectData m_enemyHitEffectDataMod;
	[Header("-- On Reaction Hit/Explosion")]
	public AbilityModPropertyInt m_onCastDamageAmountMod;
	public AbilityModPropertyBool m_explosionReduceCooldownOnlyIfHitByAllyMod;
	public AbilityModPropertyInt m_explosionCooldownReductionMod;
	public AbilityModPropertyBool m_canExplodeOncePerTurnMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClaymoreSilenceLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClaymoreSilenceLaser claymoreSilenceLaser = targetAbility as ClaymoreSilenceLaser;
		if (claymoreSilenceLaser != null)
		{
			AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, claymoreSilenceLaser.m_laserRange);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, claymoreSilenceLaser.m_laserWidth);
			AddToken(tokens, m_laserMaxTargetsMod, "LaserMaxTargets", string.Empty, claymoreSilenceLaser.m_laserMaxTargets);
			AddToken(tokens, m_onCastDamageAmountMod, "OnCastDamageAmount", string.Empty, claymoreSilenceLaser.m_onCastDamageAmount);
			AddToken(tokens, m_effectExplosionDamageMod, "EffectExplosionDamage", string.Empty, claymoreSilenceLaser.m_effectExplosionDamage);
			AddToken(tokens, m_explosionDamageAfterFirstHitMod, "ExplosionDamageAfterFirstHit", string.Empty, claymoreSilenceLaser.m_explosionDamageAfterFirstHit);
			AddToken_EffectMod(tokens, m_enemyHitEffectDataMod, "EnemyHitEffectData", claymoreSilenceLaser.m_enemyHitEffectData);
			AddToken(tokens, m_explosionCooldownReductionMod, "ExplosionCooldownReduction", string.Empty, claymoreSilenceLaser.m_explosionCooldownReduction);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreSilenceLaser claymoreSilenceLaser = GetTargetAbilityOnAbilityData(abilityData) as ClaymoreSilenceLaser;
		bool isAbilityPresent = claymoreSilenceLaser != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserRangeMod, "[Laser Range]", isAbilityPresent, isAbilityPresent ? claymoreSilenceLaser.m_laserRange : 0f);
		desc += PropDesc(m_laserWidthMod, "[Laser Width]", isAbilityPresent, isAbilityPresent ? claymoreSilenceLaser.m_laserWidth : 0f);
		desc += PropDesc(m_laserMaxTargetsMod, "[Laser Max Targets]", isAbilityPresent, isAbilityPresent ? claymoreSilenceLaser.m_laserMaxTargets : 0);
		desc += PropDesc(m_penetrateLosMod, "[Penetrate Los]", isAbilityPresent, isAbilityPresent && claymoreSilenceLaser.m_penetrateLos);
		desc += PropDesc(m_onCastDamageAmountMod, "[On Cast Damage Amount]", isAbilityPresent, isAbilityPresent ? claymoreSilenceLaser.m_onCastDamageAmount : 0);
		desc += PropDesc(m_enemyHitEffectDataMod, "[Enemy Hit Effect Data]", isAbilityPresent, !isAbilityPresent ? null : claymoreSilenceLaser.m_enemyHitEffectData);
		desc += PropDesc(m_effectExplosionDamageMod, "[Effect Explosion Damage]", isAbilityPresent, isAbilityPresent ? claymoreSilenceLaser.m_effectExplosionDamage : 0);
		desc += PropDesc(m_explosionDamageAfterFirstHitMod, "[ExplosionDamageAfterFirstHit]", isAbilityPresent, isAbilityPresent ? claymoreSilenceLaser.m_explosionDamageAfterFirstHit : 0);
		desc += PropDesc(m_explosionReduceCooldownOnlyIfHitByAllyMod, "[ExplosionReduceCooldownOnlyIfHitByAlly]", isAbilityPresent, isAbilityPresent && claymoreSilenceLaser.m_explosionReduceCooldownOnlyIfHitByAlly);
		desc += PropDesc(m_explosionCooldownReductionMod, "[ExplosionCooldownReduction]", isAbilityPresent, isAbilityPresent ? claymoreSilenceLaser.m_explosionCooldownReduction : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_canExplodeOncePerTurnMod, "[ExplodeOncePerTurn]", isAbilityPresent)).ToString();
	}
}
