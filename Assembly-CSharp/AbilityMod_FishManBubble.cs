using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_FishManBubble : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyShape m_targetShapeMod;
	public AbilityModPropertyBool m_canTargetEnemiesMod;
	public AbilityModPropertyBool m_canTargetAlliesMod;
	public AbilityModPropertyBool m_canTargetSelfMod;
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
	[Header("-- Explosion Hit")]
	public AbilityModPropertyInt m_explosionHealingToAlliesMod;
	public AbilityModPropertyInt m_explosionDamageToEnemiesMod;
	public AbilityModPropertyEffectInfo m_explosionEffectToAlliesMod;
	public AbilityModPropertyEffectInfo m_explosionEffectToEnemiesMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FishManBubble);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FishManBubble fishManBubble = targetAbility as FishManBubble;
		if (fishManBubble != null)
		{
			AddToken_EffectMod(tokens, m_effectOnAlliesMod, "EffectOnAllies", fishManBubble.m_effectOnAllies);
			AddToken_EffectMod(tokens, m_effectOnEnemiesMod, "EffectOnEnemies", fishManBubble.m_effectOnEnemies);
			AddToken(tokens, m_initialHitHealingToAlliesMod, "InitialHitHealingToAllies", string.Empty, fishManBubble.m_initialHitHealingToAllies);
			AddToken(tokens, m_initialHitDamageToEnemiesMod, "InitialHitDamageToEnemies", string.Empty, fishManBubble.m_initialHitDamageToEnemies);
			AddToken(tokens, m_numTurnsBeforeFirstExplosionMod, "NumTurnsBeforeFirstExplosionMod", string.Empty, fishManBubble.m_numTurnsBeforeFirstExplosion);
			AddToken(tokens, m_numExplosionsBeforeEndingMod, "NumExplosionsBeforeEndingMod", string.Empty, fishManBubble.m_numExplosionsBeforeEnding);
			AddToken(tokens, m_explosionHealingToAlliesMod, "ExplosionHealingToAllies", string.Empty, fishManBubble.m_explosionHealingToAllies);
			AddToken(tokens, m_explosionDamageToEnemiesMod, "ExplosionDamageToEnemies", string.Empty, fishManBubble.m_explosionDamageToEnemies);
			AddToken_EffectMod(tokens, m_explosionEffectToAlliesMod, "ExplosionEffectToAllies", fishManBubble.m_explosionEffectToAllies);
			AddToken_EffectMod(tokens, m_explosionEffectToEnemiesMod, "ExplosionEffectToEnemies", fishManBubble.m_explosionEffectToEnemies);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManBubble fishManBubble = GetTargetAbilityOnAbilityData(abilityData) as FishManBubble;
		bool isValid = fishManBubble != null;
		string desc = string.Empty;
		desc += PropDesc(m_targetShapeMod, "[TargetShape]", isValid, isValid ? fishManBubble.m_targetShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_canTargetEnemiesMod, "[CanTargetEnemies]", isValid, isValid && fishManBubble.m_canTargetEnemies);
		desc += PropDesc(m_canTargetAlliesMod, "[CanTargetAllies]", isValid, isValid && fishManBubble.m_canTargetAllies);
		desc += PropDesc(m_canTargetSelfMod, "[CanTargetSelf]", isValid, isValid && fishManBubble.m_canTargetSelf);
		desc += PropDesc(m_effectOnAlliesMod, "[EffectOnAllies]", isValid, isValid ? fishManBubble.m_effectOnAllies : null);
		desc += PropDesc(m_effectOnEnemiesMod, "[EffectOnEnemies]", isValid, isValid ? fishManBubble.m_effectOnEnemies : null);
		desc += PropDesc(m_initialHitHealingToAlliesMod, "[InitialHitHealingToAllies]", isValid, isValid ? fishManBubble.m_initialHitHealingToAllies : 0);
		desc += PropDesc(m_initialHitDamageToEnemiesMod, "[InitialHitDamageToEnemies]", isValid, isValid ? fishManBubble.m_initialHitDamageToEnemies : 0);
		desc += PropDesc(m_numTurnsBeforeFirstExplosionMod, "[NumTurnsBeforeFirstExplosionMod]", isValid, isValid ? fishManBubble.m_numTurnsBeforeFirstExplosion : 0);
		desc += PropDesc(m_numExplosionsBeforeEndingMod, "[NumExplosionsBeforeEndingMod]", isValid, isValid ? fishManBubble.m_numExplosionsBeforeEnding : 0);
		desc += PropDesc(m_explosionShapeMod, "[ExplosionShape]", isValid, isValid ? fishManBubble.m_explosionShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_explosionIgnoresLineOfSightMod, "[ExplosionIgnoresLineOfSight]", isValid, isValid && fishManBubble.m_explosionIgnoresLineOfSight);
		desc += PropDesc(m_explosionCanAffectEffectHolderMod, "[ExplosionCanAffectEffectHolder]", isValid, isValid && fishManBubble.m_explosionCanAffectEffectHolder);
		desc += PropDesc(m_explosionHealingToAlliesMod, "[ExplosionHealingToAllies]", isValid, isValid ? fishManBubble.m_explosionHealingToAllies : 0);
		desc += PropDesc(m_explosionDamageToEnemiesMod, "[ExplosionDamageToEnemies]", isValid, isValid ? fishManBubble.m_explosionDamageToEnemies : 0);
		desc += PropDesc(m_explosionEffectToAlliesMod, "[ExplosionEffectToAllies]", isValid, isValid ? fishManBubble.m_explosionEffectToAllies : null);
		return new StringBuilder().Append(desc).Append(PropDesc(m_explosionEffectToEnemiesMod, "[ExplosionEffectToEnemies]", isValid, isValid ? fishManBubble.m_explosionEffectToEnemies : null)).ToString();
	}
}
