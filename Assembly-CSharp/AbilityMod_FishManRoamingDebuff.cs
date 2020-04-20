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
		if (fishManRoamingDebuff != null)
		{
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "LaserInfo", fishManRoamingDebuff.m_laserInfo, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectWhileOnEnemyMod, "EffectWhileOnEnemy", fishManRoamingDebuff.m_effectWhileOnEnemy, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectWhileOnAllyMod, "EffectWhileOnAlly", fishManRoamingDebuff.m_effectWhileOnAlly, true);
			AbilityMod.AddToken(tokens, this.m_jumpRadiusMod, "JumpRadius", string.Empty, fishManRoamingDebuff.m_jumpRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_numJumpsMod, "NumJumps", string.Empty, fishManRoamingDebuff.m_numJumps, true, false);
			AbilityMod.AddToken(tokens, this.m_damageToEnemiesOnJumpMod, "DamageToEnemiesOnJump", string.Empty, fishManRoamingDebuff.m_damageToEnemiesOnJump, true, false);
			AbilityMod.AddToken(tokens, this.m_healingToAlliesOnJumpMod, "HealingToAlliesOnJump", string.Empty, fishManRoamingDebuff.m_healingToAlliesOnJump, true, false);
			AbilityMod.AddToken(tokens, this.m_damageIncreasePerJumpMod, "DamageIncreasePerJump", string.Empty, fishManRoamingDebuff.m_damageIncreasePerJump, true, false);
			AbilityMod.AddToken(tokens, this.m_damageToEnemyOnInitialHitMod, "DamageToEnemyOnInitialHit", string.Empty, fishManRoamingDebuff.m_damageToEnemyOnInitialHit, true, false);
			AbilityMod.AddToken(tokens, this.m_healingToAllyOnInitialHitMod, "HealingToAllyOnInitialHit", string.Empty, fishManRoamingDebuff.m_healingToAllyOnInitialHit, true, false);
			AbilityMod.AddToken(tokens, this.m_jumpAnimationIndexMod, "JumpAnimationIndex", string.Empty, fishManRoamingDebuff.m_jumpAnimationIndex, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManRoamingDebuff fishManRoamingDebuff = base.GetTargetAbilityOnAbilityData(abilityData) as FishManRoamingDebuff;
		bool flag = fishManRoamingDebuff != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_laserInfoMod, "[LaserInfo]", flag, (!flag) ? null : fishManRoamingDebuff.m_laserInfo);
		string str = text;
		AbilityModPropertyEffectInfo effectWhileOnEnemyMod = this.m_effectWhileOnEnemyMod;
		string prefix = "[EffectWhileOnEnemy]";
		bool showBaseVal = flag;
		StandardEffectInfo baseVal;
		if (flag)
		{
			baseVal = fishManRoamingDebuff.m_effectWhileOnEnemy;
		}
		else
		{
			baseVal = null;
		}
		text = str + base.PropDesc(effectWhileOnEnemyMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyEffectInfo effectWhileOnAllyMod = this.m_effectWhileOnAllyMod;
		string prefix2 = "[EffectWhileOnAlly]";
		bool showBaseVal2 = flag;
		StandardEffectInfo baseVal2;
		if (flag)
		{
			baseVal2 = fishManRoamingDebuff.m_effectWhileOnAlly;
		}
		else
		{
			baseVal2 = null;
		}
		text = str2 + base.PropDesc(effectWhileOnAllyMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_jumpRadiusMod, "[JumpRadius]", flag, (!flag) ? 0f : fishManRoamingDebuff.m_jumpRadius);
		string str3 = text;
		AbilityModPropertyBool jumpIgnoresLineOfSightMod = this.m_jumpIgnoresLineOfSightMod;
		string prefix3 = "[JumpIgnoresLineOfSight]";
		bool showBaseVal3 = flag;
		bool baseVal3;
		if (flag)
		{
			baseVal3 = fishManRoamingDebuff.m_jumpIgnoresLineOfSight;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(jumpIgnoresLineOfSightMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt numJumpsMod = this.m_numJumpsMod;
		string prefix4 = "[NumJumps]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = fishManRoamingDebuff.m_numJumps;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(numJumpsMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool canJumpToEnemiesMod = this.m_canJumpToEnemiesMod;
		string prefix5 = "[CanJumpToEnemies]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = fishManRoamingDebuff.m_canJumpToEnemies;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(canJumpToEnemiesMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyBool canJumpToAlliesMod = this.m_canJumpToAlliesMod;
		string prefix6 = "[CanJumpToAllies]";
		bool showBaseVal6 = flag;
		bool baseVal6;
		if (flag)
		{
			baseVal6 = fishManRoamingDebuff.m_canJumpToAllies;
		}
		else
		{
			baseVal6 = false;
		}
		text = str6 + base.PropDesc(canJumpToAlliesMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyBool canJumpToInvisibleTargetsMod = this.m_canJumpToInvisibleTargetsMod;
		string prefix7 = "[CanJumpToInvisibleTargets]";
		bool showBaseVal7 = flag;
		bool baseVal7;
		if (flag)
		{
			baseVal7 = fishManRoamingDebuff.m_canJumpToInvisibleTargets;
		}
		else
		{
			baseVal7 = false;
		}
		text = str7 + base.PropDesc(canJumpToInvisibleTargetsMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt damageToEnemyOnInitialHitMod = this.m_damageToEnemyOnInitialHitMod;
		string prefix8 = "[DamageToEnemyOnInitialHit]";
		bool showBaseVal8 = flag;
		int baseVal8;
		if (flag)
		{
			baseVal8 = fishManRoamingDebuff.m_damageToEnemyOnInitialHit;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(damageToEnemyOnInitialHitMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt healingToAllyOnInitialHitMod = this.m_healingToAllyOnInitialHitMod;
		string prefix9 = "[HealingToAllyOnInitialHit]";
		bool showBaseVal9 = flag;
		int baseVal9;
		if (flag)
		{
			baseVal9 = fishManRoamingDebuff.m_healingToAllyOnInitialHit;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(healingToAllyOnInitialHitMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt damageToEnemiesOnJumpMod = this.m_damageToEnemiesOnJumpMod;
		string prefix10 = "[DamageToEnemiesOnJump]";
		bool showBaseVal10 = flag;
		int baseVal10;
		if (flag)
		{
			baseVal10 = fishManRoamingDebuff.m_damageToEnemiesOnJump;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(damageToEnemiesOnJumpMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyInt healingToAlliesOnJumpMod = this.m_healingToAlliesOnJumpMod;
		string prefix11 = "[HealingToAlliesOnJump]";
		bool showBaseVal11 = flag;
		int baseVal11;
		if (flag)
		{
			baseVal11 = fishManRoamingDebuff.m_healingToAlliesOnJump;
		}
		else
		{
			baseVal11 = 0;
		}
		text = str11 + base.PropDesc(healingToAlliesOnJumpMod, prefix11, showBaseVal11, baseVal11);
		text += base.PropDesc(this.m_damageIncreasePerJumpMod, "[DamageIncreasePerJump]", flag, (!flag) ? 0 : fishManRoamingDebuff.m_damageIncreasePerJump);
		string str12 = text;
		AbilityModPropertyInt jumpAnimationIndexMod = this.m_jumpAnimationIndexMod;
		string prefix12 = "[JumpAnimationIndex]";
		bool showBaseVal12 = flag;
		int baseVal12;
		if (flag)
		{
			baseVal12 = fishManRoamingDebuff.m_jumpAnimationIndex;
		}
		else
		{
			baseVal12 = 0;
		}
		return str12 + base.PropDesc(jumpAnimationIndexMod, prefix12, showBaseVal12, baseVal12);
	}
}
