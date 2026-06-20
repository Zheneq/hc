// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SorceressHealingCrossBeam : AbilityMod
{
	[Header("-- Laser Size and Number Mod")]
	public AbilityModPropertyInt m_laserNumberMod;
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_laserRangeMod;
	[Header("-- Normal Damage and Healing Mod")]
	public AbilityModPropertyInt m_normalDamageMod;
	public AbilityModPropertyInt m_normalHealingMod;
	[Header("-- Separate Damage and Healing Mod if only 1 target hit in a laser")]
	public bool m_useSingleTargetHitMods;
	public AbilityModPropertyInt m_singleTargetDamageMod;
	public AbilityModPropertyInt m_singleTargetHealingMod;
	[Header("-- Hit Effect Override")]
	public AbilityModPropertyEffectInfo m_enemyEffectOverride;
	public AbilityModPropertyEffectInfo m_allyEffectOverride;
	[Header("-- Knockback")]
	public float m_knockbackDistance;
	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;
	public float m_knockbackThresholdDistance = -1f;
	[Header("-- Spawn Ground Effect on Enemy Hit")]
	public StandardGroundEffectInfo m_groundEffectOnEnemyHit;

	public override Type GetTargetAbilityType()
	{
		return typeof(SorceressHealingCrossBeam);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SorceressHealingCrossBeam sorceressHealingCrossBeam = targetAbility as SorceressHealingCrossBeam;
		if (sorceressHealingCrossBeam != null)
		{
			AddToken(tokens, m_normalDamageMod, "DamageAmount_Normal", string.Empty, sorceressHealingCrossBeam.m_damageAmount);
			AddToken(tokens, m_singleTargetDamageMod, "DamageAmount_SingleTarget", string.Empty, sorceressHealingCrossBeam.m_damageAmount);
			AddToken_EffectMod(tokens, m_enemyEffectOverride, "EnemyHitEffect", sorceressHealingCrossBeam.m_enemyHitEffect);
			AddToken(tokens, m_normalHealingMod, "HealAmount_Normal", string.Empty, sorceressHealingCrossBeam.m_healAmount);
			AddToken(tokens, m_singleTargetHealingMod, "HealAmount_SingleTarget", string.Empty, sorceressHealingCrossBeam.m_healAmount);
			AddToken_EffectMod(tokens, m_allyEffectOverride, "AllyHitEffect", sorceressHealingCrossBeam.m_allyHitEffect);
			AddToken(tokens, m_laserWidthMod, "Width", string.Empty, sorceressHealingCrossBeam.m_width);
			AddToken(tokens, m_laserRangeMod, "Distance", string.Empty, sorceressHealingCrossBeam.m_distance);
			AddToken(tokens, m_laserNumberMod, "NumLasers", string.Empty, sorceressHealingCrossBeam.m_numLasers);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		SorceressHealingCrossBeam sorceressHealingCrossBeam = GetTargetAbilityOnAbilityData(abilityData) as SorceressHealingCrossBeam;
		// rogues
		//SorceressHealingCrossBeam sorceressHealingCrossBeam = targetAbility as SorceressHealingCrossBeam;
		bool isAbilityPresent = sorceressHealingCrossBeam != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_laserNumberMod, "[Number of Lasers]", isAbilityPresent, isAbilityPresent ? sorceressHealingCrossBeam.m_numLasers : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserWidthMod, "[Laser Width]", isAbilityPresent, isAbilityPresent ? sorceressHealingCrossBeam.m_width : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserRangeMod, "[Laser Range]", isAbilityPresent, isAbilityPresent ? sorceressHealingCrossBeam.m_distance : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_normalDamageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? sorceressHealingCrossBeam.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_normalHealingMod, "[Healing]", isAbilityPresent, isAbilityPresent ? sorceressHealingCrossBeam.m_healAmount : 0);
		if (m_useSingleTargetHitMods)
		{
			desc += AbilityModHelper.GetModPropertyDesc(m_singleTargetDamageMod, "[Damage if Target is only target in laser]", isAbilityPresent, isAbilityPresent ? sorceressHealingCrossBeam.m_damageAmount : 0);
			desc += AbilityModHelper.GetModPropertyDesc(m_singleTargetHealingMod, "[Healing if Target is the only target in laser]", isAbilityPresent, isAbilityPresent ? sorceressHealingCrossBeam.m_healAmount : 0);
		}
		desc += AbilityModHelper.GetModPropertyDesc(m_enemyEffectOverride, "{ Enemy Hit Effect Override }", isAbilityPresent, isAbilityPresent ? sorceressHealingCrossBeam.m_enemyHitEffect : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_allyEffectOverride, "{ Ally Hit Effect Override }", isAbilityPresent, isAbilityPresent ? sorceressHealingCrossBeam.m_allyHitEffect : null);
		if (m_knockbackDistance > 0f)
		{
			desc += "\nKnockback " + m_knockbackDistance + " squares, "
			        + (m_knockbackThresholdDistance > 0f ? "to Targets within " + m_knockbackThresholdDistance + " squares, " : string.Empty)
			        + "with type " + m_knockbackType + "\n";
		}
		return desc + AbilityModHelper.GetModGroundEffectInfoDesc(m_groundEffectOnEnemyHit, "{ Ground Effect on Enemy Hit }", isAbilityPresent);
	}
}
