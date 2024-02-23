using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_FishManRoamingDebuff : AbilityMod
{
	[Header("-- Laser Targeting")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;
	[Header("-- Effect Data")]
	public AbilityModPropertyEffectInfo m_effectWhileOnEnemyMod;
	public AbilityModPropertyEffectInfo m_effectWhileOnAllyMod;
	[Header("-- Jump Params")]
	public AbilityModPropertyFloat m_jumpRadiusMod;
	public AbilityModPropertyBool m_jumpIgnoresLineOfSightMod;
	public AbilityModPropertyInt m_numJumpsMod;
	public AbilityModPropertyBool m_canJumpToEnemiesMod;
	public AbilityModPropertyBool m_canJumpToAlliesMod;
	public AbilityModPropertyBool m_canJumpToInvisibleTargetsMod;
	[Header("-- Initial Hit Damage/Healing")]
	public AbilityModPropertyInt m_damageToEnemyOnInitialHitMod;
	public AbilityModPropertyInt m_healingToAllyOnInitialHitMod;
	[Header("-- Jump Damage/Healing")]
	public AbilityModPropertyInt m_damageToEnemiesOnJumpMod;
	public AbilityModPropertyInt m_healingToAlliesOnJumpMod;
	public AbilityModPropertyInt m_damageIncreasePerJumpMod;
	[Space(10f)]
	public AbilityModPropertyInt m_jumpAnimationIndexMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FishManRoamingDebuff);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FishManRoamingDebuff fishManRoamingDebuff = targetAbility as FishManRoamingDebuff;
		if (fishManRoamingDebuff != null)
		{
			AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", fishManRoamingDebuff.m_laserInfo);
			AddToken_EffectMod(tokens, m_effectWhileOnEnemyMod, "EffectWhileOnEnemy", fishManRoamingDebuff.m_effectWhileOnEnemy);
			AddToken_EffectMod(tokens, m_effectWhileOnAllyMod, "EffectWhileOnAlly", fishManRoamingDebuff.m_effectWhileOnAlly);
			AddToken(tokens, m_jumpRadiusMod, "JumpRadius", string.Empty, fishManRoamingDebuff.m_jumpRadius);
			AddToken(tokens, m_numJumpsMod, "NumJumps", string.Empty, fishManRoamingDebuff.m_numJumps);
			AddToken(tokens, m_damageToEnemiesOnJumpMod, "DamageToEnemiesOnJump", string.Empty, fishManRoamingDebuff.m_damageToEnemiesOnJump);
			AddToken(tokens, m_healingToAlliesOnJumpMod, "HealingToAlliesOnJump", string.Empty, fishManRoamingDebuff.m_healingToAlliesOnJump);
			AddToken(tokens, m_damageIncreasePerJumpMod, "DamageIncreasePerJump", string.Empty, fishManRoamingDebuff.m_damageIncreasePerJump);
			AddToken(tokens, m_damageToEnemyOnInitialHitMod, "DamageToEnemyOnInitialHit", string.Empty, fishManRoamingDebuff.m_damageToEnemyOnInitialHit);
			AddToken(tokens, m_healingToAllyOnInitialHitMod, "HealingToAllyOnInitialHit", string.Empty, fishManRoamingDebuff.m_healingToAllyOnInitialHit);
			AddToken(tokens, m_jumpAnimationIndexMod, "JumpAnimationIndex", string.Empty, fishManRoamingDebuff.m_jumpAnimationIndex);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManRoamingDebuff fishManRoamingDebuff = GetTargetAbilityOnAbilityData(abilityData) as FishManRoamingDebuff;
		bool isValid = fishManRoamingDebuff != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserInfoMod, "[LaserInfo]", isValid, isValid ? fishManRoamingDebuff.m_laserInfo : null);
		desc += PropDesc(m_effectWhileOnEnemyMod, "[EffectWhileOnEnemy]", isValid, isValid ? fishManRoamingDebuff.m_effectWhileOnEnemy : null);
		desc += PropDesc(m_effectWhileOnAllyMod, "[EffectWhileOnAlly]", isValid, isValid ? fishManRoamingDebuff.m_effectWhileOnAlly : null);
		desc += PropDesc(m_jumpRadiusMod, "[JumpRadius]", isValid, isValid ? fishManRoamingDebuff.m_jumpRadius : 0f);
		desc += PropDesc(m_jumpIgnoresLineOfSightMod, "[JumpIgnoresLineOfSight]", isValid, isValid && fishManRoamingDebuff.m_jumpIgnoresLineOfSight);
		desc += PropDesc(m_numJumpsMod, "[NumJumps]", isValid, isValid ? fishManRoamingDebuff.m_numJumps : 0);
		desc += PropDesc(m_canJumpToEnemiesMod, "[CanJumpToEnemies]", isValid, isValid && fishManRoamingDebuff.m_canJumpToEnemies);
		desc += PropDesc(m_canJumpToAlliesMod, "[CanJumpToAllies]", isValid, isValid && fishManRoamingDebuff.m_canJumpToAllies);
		desc += PropDesc(m_canJumpToInvisibleTargetsMod, "[CanJumpToInvisibleTargets]", isValid, isValid && fishManRoamingDebuff.m_canJumpToInvisibleTargets);
		desc += PropDesc(m_damageToEnemyOnInitialHitMod, "[DamageToEnemyOnInitialHit]", isValid, isValid ? fishManRoamingDebuff.m_damageToEnemyOnInitialHit : 0);
		desc += PropDesc(m_healingToAllyOnInitialHitMod, "[HealingToAllyOnInitialHit]", isValid, isValid ? fishManRoamingDebuff.m_healingToAllyOnInitialHit : 0);
		desc += PropDesc(m_damageToEnemiesOnJumpMod, "[DamageToEnemiesOnJump]", isValid, isValid ? fishManRoamingDebuff.m_damageToEnemiesOnJump : 0);
		desc += PropDesc(m_healingToAlliesOnJumpMod, "[HealingToAlliesOnJump]", isValid, isValid ? fishManRoamingDebuff.m_healingToAlliesOnJump : 0);
		desc += PropDesc(m_damageIncreasePerJumpMod, "[DamageIncreasePerJump]", isValid, isValid ? fishManRoamingDebuff.m_damageIncreasePerJump : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_jumpAnimationIndexMod, "[JumpAnimationIndex]", isValid, isValid ? fishManRoamingDebuff.m_jumpAnimationIndex : 0)).ToString();
	}
}
