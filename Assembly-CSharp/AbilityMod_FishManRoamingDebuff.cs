using System;
using System.Collections.Generic;
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
		if (!(fishManRoamingDebuff != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", fishManRoamingDebuff.m_laserInfo);
			AbilityMod.AddToken_EffectMod(tokens, m_effectWhileOnEnemyMod, "EffectWhileOnEnemy", fishManRoamingDebuff.m_effectWhileOnEnemy);
			AbilityMod.AddToken_EffectMod(tokens, m_effectWhileOnAllyMod, "EffectWhileOnAlly", fishManRoamingDebuff.m_effectWhileOnAlly);
			AbilityMod.AddToken(tokens, m_jumpRadiusMod, "JumpRadius", string.Empty, fishManRoamingDebuff.m_jumpRadius);
			AbilityMod.AddToken(tokens, m_numJumpsMod, "NumJumps", string.Empty, fishManRoamingDebuff.m_numJumps);
			AbilityMod.AddToken(tokens, m_damageToEnemiesOnJumpMod, "DamageToEnemiesOnJump", string.Empty, fishManRoamingDebuff.m_damageToEnemiesOnJump);
			AbilityMod.AddToken(tokens, m_healingToAlliesOnJumpMod, "HealingToAlliesOnJump", string.Empty, fishManRoamingDebuff.m_healingToAlliesOnJump);
			AbilityMod.AddToken(tokens, m_damageIncreasePerJumpMod, "DamageIncreasePerJump", string.Empty, fishManRoamingDebuff.m_damageIncreasePerJump);
			AbilityMod.AddToken(tokens, m_damageToEnemyOnInitialHitMod, "DamageToEnemyOnInitialHit", string.Empty, fishManRoamingDebuff.m_damageToEnemyOnInitialHit);
			AbilityMod.AddToken(tokens, m_healingToAllyOnInitialHitMod, "HealingToAllyOnInitialHit", string.Empty, fishManRoamingDebuff.m_healingToAllyOnInitialHit);
			AbilityMod.AddToken(tokens, m_jumpAnimationIndexMod, "JumpAnimationIndex", string.Empty, fishManRoamingDebuff.m_jumpAnimationIndex);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManRoamingDebuff fishManRoamingDebuff = GetTargetAbilityOnAbilityData(abilityData) as FishManRoamingDebuff;
		bool flag = fishManRoamingDebuff != null;
		string empty = string.Empty;
		empty += PropDesc(m_laserInfoMod, "[LaserInfo]", flag, (!flag) ? null : fishManRoamingDebuff.m_laserInfo);
		string str = empty;
		AbilityModPropertyEffectInfo effectWhileOnEnemyMod = m_effectWhileOnEnemyMod;
		object baseVal;
		if (flag)
		{
			baseVal = fishManRoamingDebuff.m_effectWhileOnEnemy;
		}
		else
		{
			baseVal = null;
		}
		empty = str + PropDesc(effectWhileOnEnemyMod, "[EffectWhileOnEnemy]", flag, (StandardEffectInfo)baseVal);
		string str2 = empty;
		AbilityModPropertyEffectInfo effectWhileOnAllyMod = m_effectWhileOnAllyMod;
		object baseVal2;
		if (flag)
		{
			baseVal2 = fishManRoamingDebuff.m_effectWhileOnAlly;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str2 + PropDesc(effectWhileOnAllyMod, "[EffectWhileOnAlly]", flag, (StandardEffectInfo)baseVal2);
		empty += PropDesc(m_jumpRadiusMod, "[JumpRadius]", flag, (!flag) ? 0f : fishManRoamingDebuff.m_jumpRadius);
		string str3 = empty;
		AbilityModPropertyBool jumpIgnoresLineOfSightMod = m_jumpIgnoresLineOfSightMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = (fishManRoamingDebuff.m_jumpIgnoresLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(jumpIgnoresLineOfSightMod, "[JumpIgnoresLineOfSight]", flag, (byte)baseVal3 != 0);
		string str4 = empty;
		AbilityModPropertyInt numJumpsMod = m_numJumpsMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = fishManRoamingDebuff.m_numJumps;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(numJumpsMod, "[NumJumps]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyBool canJumpToEnemiesMod = m_canJumpToEnemiesMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = (fishManRoamingDebuff.m_canJumpToEnemies ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(canJumpToEnemiesMod, "[CanJumpToEnemies]", flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertyBool canJumpToAlliesMod = m_canJumpToAlliesMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = (fishManRoamingDebuff.m_canJumpToAllies ? 1 : 0);
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(canJumpToAlliesMod, "[CanJumpToAllies]", flag, (byte)baseVal6 != 0);
		string str7 = empty;
		AbilityModPropertyBool canJumpToInvisibleTargetsMod = m_canJumpToInvisibleTargetsMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = (fishManRoamingDebuff.m_canJumpToInvisibleTargets ? 1 : 0);
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(canJumpToInvisibleTargetsMod, "[CanJumpToInvisibleTargets]", flag, (byte)baseVal7 != 0);
		string str8 = empty;
		AbilityModPropertyInt damageToEnemyOnInitialHitMod = m_damageToEnemyOnInitialHitMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = fishManRoamingDebuff.m_damageToEnemyOnInitialHit;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(damageToEnemyOnInitialHitMod, "[DamageToEnemyOnInitialHit]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyInt healingToAllyOnInitialHitMod = m_healingToAllyOnInitialHitMod;
		int baseVal9;
		if (flag)
		{
			baseVal9 = fishManRoamingDebuff.m_healingToAllyOnInitialHit;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(healingToAllyOnInitialHitMod, "[HealingToAllyOnInitialHit]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyInt damageToEnemiesOnJumpMod = m_damageToEnemiesOnJumpMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = fishManRoamingDebuff.m_damageToEnemiesOnJump;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(damageToEnemiesOnJumpMod, "[DamageToEnemiesOnJump]", flag, baseVal10);
		string str11 = empty;
		AbilityModPropertyInt healingToAlliesOnJumpMod = m_healingToAlliesOnJumpMod;
		int baseVal11;
		if (flag)
		{
			baseVal11 = fishManRoamingDebuff.m_healingToAlliesOnJump;
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str11 + PropDesc(healingToAlliesOnJumpMod, "[HealingToAlliesOnJump]", flag, baseVal11);
		empty += PropDesc(m_damageIncreasePerJumpMod, "[DamageIncreasePerJump]", flag, flag ? fishManRoamingDebuff.m_damageIncreasePerJump : 0);
		string str12 = empty;
		AbilityModPropertyInt jumpAnimationIndexMod = m_jumpAnimationIndexMod;
		int baseVal12;
		if (flag)
		{
			baseVal12 = fishManRoamingDebuff.m_jumpAnimationIndex;
		}
		else
		{
			baseVal12 = 0;
		}
		return str12 + PropDesc(jumpAnimationIndexMod, "[JumpAnimationIndex]", flag, baseVal12);
	}
}
