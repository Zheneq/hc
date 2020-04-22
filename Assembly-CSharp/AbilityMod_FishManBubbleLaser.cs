using System;
using System.Collections.Generic;
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
			AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", fishManBubbleLaser.m_laserInfo);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnAlliesMod, "EffectOnAllies", fishManBubbleLaser.m_effectOnAllies);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnEnemiesMod, "EffectOnEnemies", fishManBubbleLaser.m_effectOnEnemies);
			AbilityMod.AddToken(tokens, m_initialHitHealingToAlliesMod, "InitialHitHealingToAllies", string.Empty, fishManBubbleLaser.m_initialHitHealingToAllies);
			AbilityMod.AddToken(tokens, m_initialHitDamageToEnemiesMod, "InitialHitDamageToEnemies", string.Empty, fishManBubbleLaser.m_initialHitDamageToEnemies);
			AbilityMod.AddToken(tokens, m_numTurnsBeforeFirstExplosionMod, "NumTurnsBeforeFirstExplosionMod", string.Empty, fishManBubbleLaser.m_numTurnsBeforeFirstExplosion);
			AbilityMod.AddToken(tokens, m_numExplosionsBeforeEndingMod, "NumExplosionsBeforeEndingMod", string.Empty, fishManBubbleLaser.m_numExplosionsBeforeEnding);
			AbilityMod.AddToken(tokens, m_explosionHealingToAlliesMod, "ExplosionHealingToAllies", string.Empty, fishManBubbleLaser.m_explosionHealingToAllies);
			AbilityMod.AddToken(tokens, m_explosionDamageToEnemiesMod, "ExplosionDamageToEnemies", string.Empty, fishManBubbleLaser.m_explosionDamageToEnemies);
			AbilityMod.AddToken_EffectMod(tokens, m_explosionEffectToAlliesMod, "ExplosionEffectToAllies", fishManBubbleLaser.m_explosionEffectToAllies);
			AbilityMod.AddToken_EffectMod(tokens, m_explosionEffectToEnemiesMod, "ExplosionEffectToEnemies", fishManBubbleLaser.m_explosionEffectToEnemies);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManBubbleLaser fishManBubbleLaser = GetTargetAbilityOnAbilityData(abilityData) as FishManBubbleLaser;
		bool flag = fishManBubbleLaser != null;
		string empty = string.Empty;
		empty += PropDesc(m_laserInfoMod, "[LaserInfo]", flag, (!flag) ? null : fishManBubbleLaser.m_laserInfo);
		string str = empty;
		AbilityModPropertyEffectInfo effectOnAlliesMod = m_effectOnAlliesMod;
		object baseVal;
		if (flag)
		{
			baseVal = fishManBubbleLaser.m_effectOnAllies;
		}
		else
		{
			baseVal = null;
		}
		empty = str + PropDesc(effectOnAlliesMod, "[EffectOnAllies]", flag, (StandardEffectInfo)baseVal);
		string str2 = empty;
		AbilityModPropertyEffectInfo effectOnEnemiesMod = m_effectOnEnemiesMod;
		object baseVal2;
		if (flag)
		{
			baseVal2 = fishManBubbleLaser.m_effectOnEnemies;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str2 + PropDesc(effectOnEnemiesMod, "[EffectOnEnemies]", flag, (StandardEffectInfo)baseVal2);
		string str3 = empty;
		AbilityModPropertyInt initialHitHealingToAlliesMod = m_initialHitHealingToAlliesMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = fishManBubbleLaser.m_initialHitHealingToAllies;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(initialHitHealingToAlliesMod, "[InitialHitHealingToAllies]", flag, baseVal3);
		empty += PropDesc(m_initialHitDamageToEnemiesMod, "[InitialHitDamageToEnemies]", flag, flag ? fishManBubbleLaser.m_initialHitDamageToEnemies : 0);
		string str4 = empty;
		AbilityModPropertyInt numTurnsBeforeFirstExplosionMod = m_numTurnsBeforeFirstExplosionMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = fishManBubbleLaser.m_numTurnsBeforeFirstExplosion;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(numTurnsBeforeFirstExplosionMod, "[NumTurnsBeforeFirstExplosionMod]", flag, baseVal4);
		empty += PropDesc(m_numExplosionsBeforeEndingMod, "[NumExplosionsBeforeEndingMod]", flag, flag ? fishManBubbleLaser.m_numExplosionsBeforeEnding : 0);
		empty += PropDesc(m_explosionShapeMod, "[ExplosionShape]", flag, flag ? fishManBubbleLaser.m_explosionShape : AbilityAreaShape.SingleSquare);
		string str5 = empty;
		AbilityModPropertyBool explosionIgnoresLineOfSightMod = m_explosionIgnoresLineOfSightMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = (fishManBubbleLaser.m_explosionIgnoresLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(explosionIgnoresLineOfSightMod, "[ExplosionIgnoresLineOfSight]", flag, (byte)baseVal5 != 0);
		empty += PropDesc(m_explosionCanAffectEffectHolderMod, "[ExplosionCanAffectEffectHolder]", flag, flag && fishManBubbleLaser.m_explosionCanAffectEffectHolder);
		string str6 = empty;
		AbilityModPropertyInt explosionHealingToAlliesMod = m_explosionHealingToAlliesMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = fishManBubbleLaser.m_explosionHealingToAllies;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(explosionHealingToAlliesMod, "[ExplosionHealingToAllies]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyInt explosionDamageToEnemiesMod = m_explosionDamageToEnemiesMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = fishManBubbleLaser.m_explosionDamageToEnemies;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(explosionDamageToEnemiesMod, "[ExplosionDamageToEnemies]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyEffectInfo explosionEffectToAlliesMod = m_explosionEffectToAlliesMod;
		object baseVal8;
		if (flag)
		{
			baseVal8 = fishManBubbleLaser.m_explosionEffectToAllies;
		}
		else
		{
			baseVal8 = null;
		}
		empty = str8 + PropDesc(explosionEffectToAlliesMod, "[ExplosionEffectToAllies]", flag, (StandardEffectInfo)baseVal8);
		string str9 = empty;
		AbilityModPropertyEffectInfo explosionEffectToEnemiesMod = m_explosionEffectToEnemiesMod;
		object baseVal9;
		if (flag)
		{
			baseVal9 = fishManBubbleLaser.m_explosionEffectToEnemies;
		}
		else
		{
			baseVal9 = null;
		}
		return str9 + PropDesc(explosionEffectToEnemiesMod, "[ExplosionEffectToEnemies]", flag, (StandardEffectInfo)baseVal9);
	}
}
