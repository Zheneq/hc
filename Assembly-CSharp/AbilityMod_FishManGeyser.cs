// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_FishManGeyser : AbilityMod
{
	[Header("-- Layered Shape Override")]
	public bool m_useAdditionalShapeOverride;
	public List<FishManGeyser.ShapeToDamage> m_additionalShapeToDamageOverride = new List<FishManGeyser.ShapeToDamage>();
	[Header("-- Initial Cast")]
	public AbilityModPropertyShape m_castShapeMod;
	public AbilityModPropertyBool m_castPenetratesLoSMod;
	public AbilityModPropertyInt m_damageToEnemiesOnCastMod;
	public AbilityModPropertyInt m_healingToAlliesOnCastMod;
	public AbilityModPropertyInt m_healOnCasterPerEnemyHitMod;
	[Space(10f)]
	public AbilityModPropertyEffectInfo m_effectToEnemiesOnCastMod;
	public AbilityModPropertyEffectInfo m_effectToAlliesOnCastMod;
	[Header("-- Knockback on Cast")]
	public AbilityModPropertyBool m_applyKnockbackOnCastMod;
	public AbilityModPropertyFloat m_knockbackDistOnCastMod;
	public AbilityModPropertyKnockbackType m_knockbackTypeOnCastMod;
	[Header("-- Effect on Enemies on start of Next Turn")]
	public AbilityModPropertyEffectInfo m_enemyEffectOnNextTurnMod;
	[Header("-- Eel effect on enemies hit")]
	public AbilityModPropertyBool m_applyEelEffectOnEnemiesMod;
	public AbilityModPropertyInt m_eelDamageMod;
	public AbilityModPropertyEffectInfo m_eelEffectOnEnemiesMod;
	public AbilityModPropertyFloat m_eelRadiusMod;
	[Header("-- Explosion Timing (may be depricated if not needed)")]
	public AbilityModPropertyInt m_turnsTillFirstExplosionMod;
	public AbilityModPropertyInt m_numExplosionsBeforeEndingMod;
	[Header("-- Effect Explode (may be depricated if not needed)")]
	public AbilityModPropertyShape m_explodeShapeMod;
	public AbilityModPropertyBool m_explodePenetratesLoSMod;
	public AbilityModPropertyInt m_damageToEnemiesOnExplodeMod;
	public AbilityModPropertyInt m_healingToAlliesOnExplodeMod;
	public AbilityModPropertyBool m_applyKnockbackOnExplodeMod;
	public AbilityModPropertyFloat m_knockbackDistOnExplodeMod;
	public AbilityModPropertyEffectInfo m_effectToEnemiesOnExplodeMod;
	public AbilityModPropertyEffectInfo m_effectToAlliesOnExplodeMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FishManGeyser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FishManGeyser fishManGeyser = targetAbility as FishManGeyser;
		if (fishManGeyser != null)
		{
			AddToken(tokens, m_damageToEnemiesOnCastMod, "DamageToEnemiesOnCast", string.Empty, fishManGeyser.m_damageToEnemiesOnCast);
			AddToken(tokens, m_healingToAlliesOnCastMod, "HealingToAlliesOnCast", string.Empty, fishManGeyser.m_healingToAlliesOnCast);
			AddToken(tokens, m_healOnCasterPerEnemyHitMod, "HealOnCasterPerEnemyHit", string.Empty, fishManGeyser.m_healOnCasterPerEnemyHit);
			AddToken(tokens, m_knockbackDistOnCastMod, "KnockbackDistOnCast", string.Empty, fishManGeyser.m_knockbackDistOnCast);
			AddToken_EffectMod(tokens, m_effectToEnemiesOnCastMod, "EffectToEnemiesOnCast", fishManGeyser.m_effectToEnemiesOnCast);
			AddToken_EffectMod(tokens, m_effectToAlliesOnCastMod, "EffectToAlliesOnCast", fishManGeyser.m_effectToAlliesOnCast);
			AddToken_EffectMod(tokens, m_enemyEffectOnNextTurnMod, "EnemyEffectOnNextTurn", fishManGeyser.m_enemyEffectOnNextTurn);
			AddToken(tokens, m_eelDamageMod, "EelDamage", string.Empty, fishManGeyser.m_eelDamage);
			AddToken_EffectMod(tokens, m_eelEffectOnEnemiesMod, "EelEffectOnEnemies", fishManGeyser.m_eelEffectOnEnemies);
			AddToken(tokens, m_eelRadiusMod, "EelRadius", string.Empty, fishManGeyser.m_eelRadius);
			AddToken(tokens, m_turnsTillFirstExplosionMod, "TurnsTillFirstExplosion", string.Empty, fishManGeyser.m_turnsTillFirstExplosion);
			AddToken(tokens, m_numExplosionsBeforeEndingMod, "NumExplosionsBeforeEnding", string.Empty, fishManGeyser.m_numExplosionsBeforeEnding);
			AddToken(tokens, m_damageToEnemiesOnExplodeMod, "DamageToEnemiesOnExplode", string.Empty, fishManGeyser.m_damageToEnemiesOnExplode);
			AddToken(tokens, m_healingToAlliesOnExplodeMod, "HealingToAlliesOnExplode", string.Empty, fishManGeyser.m_healingToAlliesOnExplode);
			AddToken(tokens, m_knockbackDistOnExplodeMod, "KnockbackDistOnExplode", string.Empty, fishManGeyser.m_knockbackDistOnExplode);
			AddToken_EffectMod(tokens, m_effectToEnemiesOnExplodeMod, "EffectToEnemiesOnExplode", fishManGeyser.m_effectToEnemiesOnExplode);
			AddToken_EffectMod(tokens, m_effectToAlliesOnExplodeMod, "EffectToAlliesOnExplode", fishManGeyser.m_effectToAlliesOnExplode);
			if (m_useAdditionalShapeOverride && m_additionalShapeToDamageOverride != null)
			{
				for (int i = 0; i < m_additionalShapeToDamageOverride.Count; i++)
				{
					AddToken_IntDiff(tokens, "Damage_AdditionalLayer" + i, string.Empty, m_additionalShapeToDamageOverride[i].m_damage, true, fishManGeyser.m_damageToEnemiesOnCast);
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		FishManGeyser fishManGeyser = GetTargetAbilityOnAbilityData(abilityData) as FishManGeyser;
		// rogues
		// FishManGeyser fishManGeyser = targetAbility as FishManGeyser;
		
		bool isValid = fishManGeyser != null;
		string desc = string.Empty;
		if (m_useAdditionalShapeOverride && m_additionalShapeToDamageOverride != null)
		{
			desc += "Using Layered Shape Override, entries:\n";
			foreach (FishManGeyser.ShapeToDamage shapeToDamage in m_additionalShapeToDamageOverride)
			{
				desc += "Shape: " + shapeToDamage.m_shape + " Damage: " + shapeToDamage.m_damage + "\n";
			}
		}
		desc += PropDesc(m_castShapeMod, "[CastShape]", isValid, isValid ? fishManGeyser.m_castShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_castPenetratesLoSMod, "[CastPenetratesLoS]", isValid, isValid && fishManGeyser.m_castPenetratesLoS);
		desc += PropDesc(m_damageToEnemiesOnCastMod, "[DamageToEnemiesOnCast]", isValid, isValid ? fishManGeyser.m_damageToEnemiesOnCast : 0);
		desc += PropDesc(m_healingToAlliesOnCastMod, "[HealingToAlliesOnCast]", isValid, isValid ? fishManGeyser.m_healingToAlliesOnCast : 0);
		desc += PropDesc(m_healOnCasterPerEnemyHitMod, "[HealOnCasterPerEnemyHit]", isValid, isValid ? fishManGeyser.m_healOnCasterPerEnemyHit : 0);
		desc += PropDesc(m_applyKnockbackOnCastMod, "[ApplyKnockbackOnCast]", isValid, isValid && fishManGeyser.m_applyKnockbackOnCast);
		desc += PropDesc(m_knockbackDistOnCastMod, "[KnockbackDistOnCast]", isValid, isValid ? fishManGeyser.m_knockbackDistOnCast : 0f);
		desc += PropDesc(m_knockbackTypeOnCastMod, "[KnockbackTypeOnCast]", isValid, isValid ? fishManGeyser.m_knockbackTypeOnCast : KnockbackType.AwayFromSource);
		desc += PropDesc(m_enemyEffectOnNextTurnMod, "[EnemyEffectOnNextTurn]", isValid, isValid ? fishManGeyser.m_enemyEffectOnNextTurn : null);
		desc += PropDesc(m_applyEelEffectOnEnemiesMod, "[ApplyEelEffectOnEnemies]", isValid, isValid && fishManGeyser.m_applyEelEffectOnEnemies);
		desc += PropDesc(m_eelDamageMod, "[EelDamage]", isValid, isValid ? fishManGeyser.m_eelDamage : 0);
		desc += PropDesc(m_eelEffectOnEnemiesMod, "[EelEffectOnEnemies]", isValid, isValid ? fishManGeyser.m_eelEffectOnEnemies : null);
		desc += PropDesc(m_eelRadiusMod, "[EelRadius]", isValid, isValid ? fishManGeyser.m_eelRadius : 0f);
		desc += PropDesc(m_effectToEnemiesOnCastMod, "[EffectToEnemiesOnCast]", isValid, isValid ? fishManGeyser.m_effectToEnemiesOnCast : null);
		desc += PropDesc(m_effectToAlliesOnCastMod, "[EffectToAlliesOnCast]", isValid, isValid ? fishManGeyser.m_effectToAlliesOnCast : null);
		desc += PropDesc(m_turnsTillFirstExplosionMod, "[TurnsTillFirstExplosion]", isValid, isValid ? fishManGeyser.m_turnsTillFirstExplosion : 0);
		desc += PropDesc(m_numExplosionsBeforeEndingMod, "[NumExplosionsBeforeEnding]", isValid, isValid ? fishManGeyser.m_numExplosionsBeforeEnding : 0);
		desc += PropDesc(m_explodeShapeMod, "[ExplodeShape]", isValid, isValid ? fishManGeyser.m_explodeShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_explodePenetratesLoSMod, "[ExplodePenetratesLoS]", isValid, isValid && fishManGeyser.m_explodePenetratesLoS);
		desc += PropDesc(m_damageToEnemiesOnExplodeMod, "[DamageToEnemiesOnExplode]", isValid, isValid ? fishManGeyser.m_damageToEnemiesOnExplode : 0);
		desc += PropDesc(m_healingToAlliesOnExplodeMod, "[HealingToAlliesOnExplode]", isValid, isValid ? fishManGeyser.m_healingToAlliesOnExplode : 0);
		desc += PropDesc(m_applyKnockbackOnExplodeMod, "[ApplyKnockbackOnExplode]", isValid, isValid && fishManGeyser.m_applyKnockbackOnExplode);
		desc += PropDesc(m_knockbackDistOnExplodeMod, "[KnockbackDistOnExplode]", isValid, isValid ? fishManGeyser.m_knockbackDistOnExplode : 0f);
		desc += PropDesc(m_effectToEnemiesOnExplodeMod, "[EffectToEnemiesOnExplode]", isValid, isValid ? fishManGeyser.m_effectToEnemiesOnExplode : null);
		return desc + PropDesc(m_effectToAlliesOnExplodeMod, "[EffectToAlliesOnExplode]", isValid, isValid ? fishManGeyser.m_effectToAlliesOnExplode : null);
	}
}
