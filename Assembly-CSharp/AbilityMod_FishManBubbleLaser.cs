using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_FishManBubbleLaser : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;
	[Header("-- Initial Hit")]
	public AbilityModPropertyEffectInfo m_effectOnAlliesMod;
	public AbilityModPropertyEffectInfo m_effectOnEnemiesMod;
	public AbilityModPropertyInt m_initialHitHealingToAlliesMod;
	public AbilityModPropertyInt m_initialHitDamageToEnemiesMod;
	[Header("-- Explosion Data")]
	public AbilityModPropertyInt m_numTurnsBeforeFirstExplosionMod;
	public AbilityModPropertyInt m_numExplosionsBeforeEndingMod;
	public AbilityModPropertyShape m_explosionShapeMod;
	public AbilityModPropertyBool m_explosionIgnoresLineOfSightMod;
	public AbilityModPropertyBool m_explosionCanAffectEffectHolderMod;
	[Header("-- Explosion Results")]
	public AbilityModPropertyInt m_explosionHealingToAlliesMod;
	public AbilityModPropertyInt m_explosionDamageToEnemiesMod;
	public AbilityModPropertyEffectInfo m_explosionEffectToAlliesMod;
	public AbilityModPropertyEffectInfo m_explosionEffectToEnemiesMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FishManBubbleLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FishManBubbleLaser fishManBubbleLaser = targetAbility as FishManBubbleLaser;
		if (fishManBubbleLaser != null)
		{
			AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", fishManBubbleLaser.m_laserInfo);
			AddToken_EffectMod(tokens, m_effectOnAlliesMod, "EffectOnAllies", fishManBubbleLaser.m_effectOnAllies);
			AddToken_EffectMod(tokens, m_effectOnEnemiesMod, "EffectOnEnemies", fishManBubbleLaser.m_effectOnEnemies);
			AddToken(tokens, m_initialHitHealingToAlliesMod, "InitialHitHealingToAllies", string.Empty, fishManBubbleLaser.m_initialHitHealingToAllies);
			AddToken(tokens, m_initialHitDamageToEnemiesMod, "InitialHitDamageToEnemies", string.Empty, fishManBubbleLaser.m_initialHitDamageToEnemies);
			AddToken(tokens, m_numTurnsBeforeFirstExplosionMod, "NumTurnsBeforeFirstExplosionMod", string.Empty, fishManBubbleLaser.m_numTurnsBeforeFirstExplosion);
			AddToken(tokens, m_numExplosionsBeforeEndingMod, "NumExplosionsBeforeEndingMod", string.Empty, fishManBubbleLaser.m_numExplosionsBeforeEnding);
			AddToken(tokens, m_explosionHealingToAlliesMod, "ExplosionHealingToAllies", string.Empty, fishManBubbleLaser.m_explosionHealingToAllies);
			AddToken(tokens, m_explosionDamageToEnemiesMod, "ExplosionDamageToEnemies", string.Empty, fishManBubbleLaser.m_explosionDamageToEnemies);
			AddToken_EffectMod(tokens, m_explosionEffectToAlliesMod, "ExplosionEffectToAllies", fishManBubbleLaser.m_explosionEffectToAllies);
			AddToken_EffectMod(tokens, m_explosionEffectToEnemiesMod, "ExplosionEffectToEnemies", fishManBubbleLaser.m_explosionEffectToEnemies);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManBubbleLaser fishManBubbleLaser = GetTargetAbilityOnAbilityData(abilityData) as FishManBubbleLaser;
		bool isValid = fishManBubbleLaser != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserInfoMod, "[LaserInfo]", isValid, isValid ? fishManBubbleLaser.m_laserInfo : null);
		desc += PropDesc(m_effectOnAlliesMod, "[EffectOnAllies]", isValid, isValid ? fishManBubbleLaser.m_effectOnAllies : null);
		desc += PropDesc(m_effectOnEnemiesMod, "[EffectOnEnemies]", isValid, isValid ? fishManBubbleLaser.m_effectOnEnemies : null);
		desc += PropDesc(m_initialHitHealingToAlliesMod, "[InitialHitHealingToAllies]", isValid, isValid ? fishManBubbleLaser.m_initialHitHealingToAllies : 0);
		desc += PropDesc(m_initialHitDamageToEnemiesMod, "[InitialHitDamageToEnemies]", isValid, isValid ? fishManBubbleLaser.m_initialHitDamageToEnemies : 0);
		desc += PropDesc(m_numTurnsBeforeFirstExplosionMod, "[NumTurnsBeforeFirstExplosionMod]", isValid, isValid ? fishManBubbleLaser.m_numTurnsBeforeFirstExplosion : 0);
		desc += PropDesc(m_numExplosionsBeforeEndingMod, "[NumExplosionsBeforeEndingMod]", isValid, isValid ? fishManBubbleLaser.m_numExplosionsBeforeEnding : 0);
		desc += PropDesc(m_explosionShapeMod, "[ExplosionShape]", isValid, isValid ? fishManBubbleLaser.m_explosionShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_explosionIgnoresLineOfSightMod, "[ExplosionIgnoresLineOfSight]", isValid, isValid && fishManBubbleLaser.m_explosionIgnoresLineOfSight);
		desc += PropDesc(m_explosionCanAffectEffectHolderMod, "[ExplosionCanAffectEffectHolder]", isValid, isValid && fishManBubbleLaser.m_explosionCanAffectEffectHolder);
		desc += PropDesc(m_explosionHealingToAlliesMod, "[ExplosionHealingToAllies]", isValid, isValid ? fishManBubbleLaser.m_explosionHealingToAllies : 0);
		desc += PropDesc(m_explosionDamageToEnemiesMod, "[ExplosionDamageToEnemies]", isValid, isValid ? fishManBubbleLaser.m_explosionDamageToEnemies : 0);
		desc += PropDesc(m_explosionEffectToAlliesMod, "[ExplosionEffectToAllies]", isValid, isValid ? fishManBubbleLaser.m_explosionEffectToAllies : null);
		return new StringBuilder().Append(desc).Append(PropDesc(m_explosionEffectToEnemiesMod, "[ExplosionEffectToEnemies]", isValid, isValid ? fishManBubbleLaser.m_explosionEffectToEnemies : null)).ToString();
	}
}
