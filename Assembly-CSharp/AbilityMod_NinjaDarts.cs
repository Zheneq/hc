using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NinjaDarts : AbilityMod
{
	[Separator("Targeting Properties")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;
	[Space(10f)]
	public AbilityModPropertyInt m_laserCountMod;
	public AbilityModPropertyFloat m_angleInBetweenMod;
	public AbilityModPropertyBool m_changeAngleByCursorDistanceMod;
	public AbilityModPropertyFloat m_targeterMinAngleMod;
	public AbilityModPropertyFloat m_targeterMaxAngleMod;
	public AbilityModPropertyFloat m_targeterMinInterpDistanceMod;
	public AbilityModPropertyFloat m_targeterMaxInterpDistanceMod;
	[Separator("On Hit Stuff")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyInt m_extraDamagePerSubseqHitMod;
	public AbilityModPropertyFloat m_damageMultPerSubseqHitMod;
	public AbilityModPropertyBool m_changeEachSubseqDamageMod;
	[Space(10f)]
	public AbilityModPropertyEffectInfo m_enemySingleHitEffectMod;
	public AbilityModPropertyEffectInfo m_enemyMultiHitEffectMod;
	[Header("-- For effect when hitting over certain number of lasers --")]
	public AbilityModPropertyInt m_enemyExtraEffectHitCountMod;
	public AbilityModPropertyEffectInfo m_enemyExtraHitEffectForHitCountMod;
	[Header("-- For Ally Hit --")]
	public AbilityModPropertyEffectInfo m_allySingleHitEffectMod;
	public AbilityModPropertyEffectInfo m_allyMultiHitEffectMod;
	[Separator("Energy per dart hit")]
	public AbilityModPropertyInt m_energyPerDartHitMod;
	[Separator("Cooldown Reduction")]
	public AbilityModPropertyInt m_cdrOnMissMod;
	[Separator("[Deathmark] Effect", "magenta")]
	public AbilityModPropertyBool m_applyDeathmarkEffectMod;
	public AbilityModPropertyBool m_ignoreCoverOnTargetsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NinjaDarts);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NinjaDarts ninjaDarts = targetAbility as NinjaDarts;
		if (ninjaDarts != null)
		{
			AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", ninjaDarts.m_laserInfo);
			AddToken(tokens, m_laserCountMod, "LaserCount", string.Empty, ninjaDarts.m_laserCount);
			AddToken(tokens, m_angleInBetweenMod, "AngleInBetween", string.Empty, ninjaDarts.m_angleInBetween);
			AddToken(tokens, m_targeterMinAngleMod, "TargeterMinAngle", string.Empty, ninjaDarts.m_targeterMinAngle);
			AddToken(tokens, m_targeterMaxAngleMod, "TargeterMaxAngle", string.Empty, ninjaDarts.m_targeterMaxAngle);
			AddToken(tokens, m_targeterMinInterpDistanceMod, "TargeterMinInterpDistance", string.Empty, ninjaDarts.m_targeterMinInterpDistance);
			AddToken(tokens, m_targeterMaxInterpDistanceMod, "TargeterMaxInterpDistance", string.Empty, ninjaDarts.m_targeterMaxInterpDistance);
			AddToken(tokens, m_damageMod, "Damage", string.Empty, ninjaDarts.m_damage);
			AddToken(tokens, m_extraDamagePerSubseqHitMod, "ExtraDamagePerSubseqHit", string.Empty, ninjaDarts.m_extraDamagePerSubseqHit);
			AddToken_EffectMod(tokens, m_enemySingleHitEffectMod, "EnemySingleHitEffect", ninjaDarts.m_enemySingleHitEffect);
			AddToken_EffectMod(tokens, m_enemyMultiHitEffectMod, "EnemyMultiHitEffect", ninjaDarts.m_enemyMultiHitEffect);
			AddToken(tokens, m_enemyExtraEffectHitCountMod, "EnemyExtraEffectHitCount", string.Empty, ninjaDarts.m_enemyExtraEffectHitCount);
			AddToken_EffectMod(tokens, m_enemyExtraHitEffectForHitCountMod, "EnemyExtraHitEffectForHitCount", ninjaDarts.m_enemyExtraHitEffectForHitCount);
			AddToken_EffectMod(tokens, m_allySingleHitEffectMod, "AllySingleHitEffect", ninjaDarts.m_allySingleHitEffect);
			AddToken_EffectMod(tokens, m_allyMultiHitEffectMod, "AllyMultiHitEffect", ninjaDarts.m_allyMultiHitEffect);
			AddToken(tokens, m_energyPerDartHitMod, "EnergyPerDartHit", string.Empty, ninjaDarts.m_energyPerDartHit);
			AddToken(tokens, m_cdrOnMissMod, "CdrOnMiss", string.Empty, ninjaDarts.m_cdrOnMiss);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NinjaDarts ninjaDarts = GetTargetAbilityOnAbilityData(abilityData) as NinjaDarts;
		bool isValid = ninjaDarts != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserInfoMod, "[LaserInfo]", isValid, isValid ? ninjaDarts.m_laserInfo : null);
		desc += PropDesc(m_laserCountMod, "[LaserCount]", isValid, isValid ? ninjaDarts.m_laserCount : 0);
		desc += PropDesc(m_angleInBetweenMod, "[AngleInBetween]", isValid, isValid ? ninjaDarts.m_angleInBetween : 0f);
		desc += PropDesc(m_changeAngleByCursorDistanceMod, "[ChangeAngleByCursorDistance]", isValid, isValid && ninjaDarts.m_changeAngleByCursorDistance);
		desc += PropDesc(m_targeterMinAngleMod, "[TargeterMinAngle]", isValid, isValid ? ninjaDarts.m_targeterMinAngle : 0f);
		desc += PropDesc(m_targeterMaxAngleMod, "[TargeterMaxAngle]", isValid, isValid ? ninjaDarts.m_targeterMaxAngle : 0f);
		desc += PropDesc(m_targeterMinInterpDistanceMod, "[TargeterMinInterpDistance]", isValid, isValid ? ninjaDarts.m_targeterMinInterpDistance : 0f);
		desc += PropDesc(m_targeterMaxInterpDistanceMod, "[TargeterMaxInterpDistance]", isValid, isValid ? ninjaDarts.m_targeterMaxInterpDistance : 0f);
		desc += PropDesc(m_damageMod, "[Damage]", isValid, isValid ? ninjaDarts.m_damage : 0);
		desc += PropDesc(m_extraDamagePerSubseqHitMod, "[ExtraDamagePerSubseqHit]", isValid, isValid ? ninjaDarts.m_extraDamagePerSubseqHit : 0);
		desc += PropDesc(m_enemySingleHitEffectMod, "[EnemySingleHitEffect]", isValid, isValid ? ninjaDarts.m_enemySingleHitEffect : null);
		desc += PropDesc(m_enemyMultiHitEffectMod, "[EnemyMultiHitEffect]", isValid, isValid ? ninjaDarts.m_enemyMultiHitEffect : null);
		desc += PropDesc(m_enemyExtraEffectHitCountMod, "[EnemyExtraEffectHitCount]", isValid, isValid ? ninjaDarts.m_enemyExtraEffectHitCount : 0);
		desc += PropDesc(m_enemyExtraHitEffectForHitCountMod, "[EnemyExtraHitEffectForHitCount]", isValid, isValid ? ninjaDarts.m_enemyExtraHitEffectForHitCount : null);
		desc += PropDesc(m_allySingleHitEffectMod, "[AllySingleHitEffect]", isValid, isValid ? ninjaDarts.m_allySingleHitEffect : null);
		desc += PropDesc(m_allyMultiHitEffectMod, "[AllyMultiHitEffect]", isValid, isValid ? ninjaDarts.m_allyMultiHitEffect : null);
		desc += PropDesc(m_energyPerDartHitMod, "[EnergyPerDartHit]", isValid, isValid ? ninjaDarts.m_energyPerDartHit : 0);
		desc += PropDesc(m_cdrOnMissMod, "[CdrOnMiss]", isValid, isValid ? ninjaDarts.m_cdrOnMiss : 0);
		desc += PropDesc(m_applyDeathmarkEffectMod, "[ApplyDeathmarkEffect]", isValid, isValid && ninjaDarts.m_applyDeathmarkEffect);
		return desc + PropDesc(m_ignoreCoverOnTargetsMod, "[IgnoreCoverOnTargets]", isValid, isValid && ninjaDarts.m_ignoreCoverOnTargets);
	}
}
