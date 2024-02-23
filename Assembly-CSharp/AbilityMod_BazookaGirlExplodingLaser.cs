using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_BazookaGirlExplodingLaser : AbilityMod
{
	[Header("-- Targeting: If using Cone")]
	public AbilityModPropertyFloat m_coneWidthAngleMod;
	public AbilityModPropertyFloat m_coneLengthMod;
	public AbilityModPropertyFloat m_coneBackwardOffsetMod;
	[Header("-- Laser Params")]
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_laserRangeMod;
	public AbilityModPropertyBool m_laserPenetrateLosMod;
	[Header("-- Laser Hit Mods")]
	public AbilityModPropertyInt m_laserDamageMod;
	public AbilityModPropertyBool m_laserIgnoreCoverMod;
	public AbilityModPropertyEffectInfo m_laserHitEffectOverride;
	public AbilityModPropertyInt m_cdrOnDirectHitMod;
	[Header("-- Explosion Hit Mods")]
	public AbilityModPropertyInt m_explosionDamageMod;
	public AbilityModPropertyBool m_explosionIgnoreLosMod;
	public AbilityModPropertyBool m_explosionIgnoreCoverMod;
	public AbilityModPropertyEffectInfo m_explosionEffectOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(BazookaGirlExplodingLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BazookaGirlExplodingLaser bazookaGirlExplodingLaser = targetAbility as BazookaGirlExplodingLaser;
		if (bazookaGirlExplodingLaser != null)
		{
			AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, bazookaGirlExplodingLaser.m_coneWidthAngle);
			AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, bazookaGirlExplodingLaser.m_coneLength);
			AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, bazookaGirlExplodingLaser.m_coneBackwardOffset);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, bazookaGirlExplodingLaser.m_laserWidth);
			AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, bazookaGirlExplodingLaser.m_laserRange);
			AddToken(tokens, m_laserDamageMod, "LaserDamageAmount", string.Empty, bazookaGirlExplodingLaser.m_laserDamageAmount);
			AddToken_EffectMod(tokens, m_laserHitEffectOverride, "EffectOnLaserHitTargets", bazookaGirlExplodingLaser.m_effectOnLaserHitTargets);
			AddToken(tokens, m_explosionDamageMod, "ExplosionDamageAmount", string.Empty, bazookaGirlExplodingLaser.m_explosionDamageAmount);
			AddToken_EffectMod(tokens, m_explosionEffectOverride, "EffectOnExplosionHitTargets", bazookaGirlExplodingLaser.m_effectOnExplosionHitTargets);
			AddToken(tokens, m_cdrOnDirectHitMod, "CdrOnDirectHit", string.Empty, bazookaGirlExplodingLaser.m_cdrOnDirectHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlExplodingLaser bazookaGirlExplodingLaser = GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlExplodingLaser;
		bool isAbilityPresent = bazookaGirlExplodingLaser != null;
		string desc = string.Empty;
		desc += PropDesc(m_coneWidthAngleMod, "[ConeWidthAngle]", isAbilityPresent, isAbilityPresent ? bazookaGirlExplodingLaser.m_coneWidthAngle : 0f);
		desc += PropDesc(m_coneLengthMod, "[ConeLength]", isAbilityPresent, isAbilityPresent ? bazookaGirlExplodingLaser.m_coneLength : 0f);
		desc += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", isAbilityPresent, isAbilityPresent ? bazookaGirlExplodingLaser.m_coneBackwardOffset : 0f);
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isAbilityPresent, isAbilityPresent ? bazookaGirlExplodingLaser.m_laserWidth : 0f);
		desc += PropDesc(m_laserRangeMod, "[LaserRange]", isAbilityPresent, isAbilityPresent ? bazookaGirlExplodingLaser.m_laserRange : 0f);
		desc += PropDesc(m_laserPenetrateLosMod, "[LaserPenetrateLos]", isAbilityPresent, isAbilityPresent && bazookaGirlExplodingLaser.m_laserPenetrateLos);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserDamageMod, "[Laser Damage]", isAbilityPresent, isAbilityPresent ? bazookaGirlExplodingLaser.m_laserDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserIgnoreCoverMod, "[Laser Ignore Cover?]", isAbilityPresent, isAbilityPresent && bazookaGirlExplodingLaser.m_laserIgnoreCover);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserHitEffectOverride, "{ Laser Enemy Hit Effect Override }", isAbilityPresent, isAbilityPresent ? bazookaGirlExplodingLaser.m_effectOnLaserHitTargets : null);
		desc += PropDesc(m_cdrOnDirectHitMod, "[CdrOnDirectHit]", isAbilityPresent, isAbilityPresent ? bazookaGirlExplodingLaser.m_cdrOnDirectHit : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_explosionDamageMod, "[Explosion Damage]", isAbilityPresent, isAbilityPresent ? bazookaGirlExplodingLaser.m_explosionDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_explosionIgnoreLosMod, "[Explosion Ignore LoS?]", isAbilityPresent, isAbilityPresent && bazookaGirlExplodingLaser.m_explosionPenetrateLos);
		desc += AbilityModHelper.GetModPropertyDesc(m_explosionIgnoreCoverMod, "[Explosion Ignore Cover?]", isAbilityPresent, isAbilityPresent && bazookaGirlExplodingLaser.m_explosionIgnoreCover);
		return new StringBuilder().Append(desc).Append(AbilityModHelper.GetModPropertyDesc(m_explosionEffectOverride, "{ Explosion Enemy Hit Effect Override }", isAbilityPresent, isAbilityPresent ? bazookaGirlExplodingLaser.m_effectOnExplosionHitTargets : null)).ToString();
	}
}
