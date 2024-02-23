using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_SorceressDebuffLaser : AbilityMod
{
	[Header("-- Laser Size Mod")]
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_laserRangeMod;
	[Header("-- Effect Duration Mods")]
	public AbilityModPropertyInt m_enemyEffectDurationMod;
	public AbilityModPropertyInt m_allyEffectDurationMod;
	public AbilityModPropertyInt m_casterEffectDurationMod;
	[Header("-- Hit Effect Override")]
	public AbilityModPropertyEffectInfo m_enemyHitEffectOverride;
	public AbilityModPropertyEffectInfo m_allyHitEffectOverride;
	public AbilityModPropertyEffectInfo m_casterHitEffectOverride;
	[Header("-- Additional Effect on Self if hit others, duration = numHit")]
	public StandardEffectInfo m_additionalEffectOnSelfIfHit;
	[Header("-- Cooldown Reduction, finalReduction = modOnNumHit(numHit) + flatReduction")]
	public AbilityModPropertyInt m_cooldownReductionOnNumHit;
	public int m_cooldownFlatReduction;
	[Header("Max value for cooldown reduction, <= 0 if no limit")]
	public int m_maxCooldownReduction;

	public override Type GetTargetAbilityType()
	{
		return typeof(SorceressDebuffLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SorceressDebuffLaser sorceressDebuffLaser = targetAbility as SorceressDebuffLaser;
		if (sorceressDebuffLaser != null)
		{
			AddToken(tokens, m_laserWidthMod, "Width", string.Empty, sorceressDebuffLaser.m_width);
			AddToken(tokens, m_laserRangeMod, "Distance", string.Empty, sorceressDebuffLaser.m_distance);
			AddToken_EffectMod(tokens, m_enemyHitEffectOverride, "EnemyHitEffect", sorceressDebuffLaser.m_enemyHitEffect);
			AddToken_EffectMod(tokens, m_allyHitEffectOverride, "AllyHitEffect", sorceressDebuffLaser.m_allyHitEffect);
			AddToken_EffectMod(tokens, m_casterHitEffectOverride, "CasterHitEffect", sorceressDebuffLaser.m_casterHitEffect);
			AddToken(tokens, m_cooldownReductionOnNumHit, "CDR_OnNumHit", "cooldown reduction per hit", 1, false);
			if (m_cooldownFlatReduction > 0)
			{
				AddToken_IntDiff(tokens, "CDR_FlatAmount", "cooldown reduction", m_cooldownFlatReduction, false, 0);
			}

			AddToken_IntDiff(tokens, "CDR_Max", "max cooldown reduction", m_maxCooldownReduction, false, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SorceressDebuffLaser sorceressDebuffLaser = GetTargetAbilityOnAbilityData(abilityData) as SorceressDebuffLaser;
		bool isAbilityPresent = sorceressDebuffLaser != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_laserWidthMod, "[Laser Width]", isAbilityPresent, isAbilityPresent ? sorceressDebuffLaser.m_width : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_laserRangeMod, "[Laser Range]", isAbilityPresent, isAbilityPresent ? sorceressDebuffLaser.m_distance : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_enemyEffectDurationMod, "[Enemy Effect Duration Mod]", isAbilityPresent, isAbilityPresent ? sorceressDebuffLaser.m_enemyHitEffect.m_effectData.m_duration : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_allyEffectDurationMod, "[Ally Effect Duration Mod]", isAbilityPresent, isAbilityPresent ? sorceressDebuffLaser.m_allyHitEffect.m_effectData.m_duration : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_casterEffectDurationMod, "[Caster Effect Duration Mod]", isAbilityPresent, isAbilityPresent ? sorceressDebuffLaser.m_casterHitEffect.m_effectData.m_duration : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_enemyHitEffectOverride, "{ Enemy Hit Effect Override }", isAbilityPresent, isAbilityPresent ? sorceressDebuffLaser.m_enemyHitEffect : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_allyHitEffectOverride, "{ Ally Hit Effect Override }", isAbilityPresent, isAbilityPresent ? sorceressDebuffLaser.m_allyHitEffect : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_casterHitEffectOverride, "{ Caster Hit Effect Override }", isAbilityPresent, isAbilityPresent ? sorceressDebuffLaser.m_casterHitEffect : null);
		if (m_additionalEffectOnSelfIfHit != null && m_additionalEffectOnSelfIfHit.m_applyEffect)
		{
			desc += AbilityModHelper.GetModEffectDataDesc(m_additionalEffectOnSelfIfHit.m_effectData, "{ Additional Effect on Self if Hit (duration = numHit) }", string.Empty);
		}
		desc += AbilityModHelper.GetModPropertyDesc(m_cooldownReductionOnNumHit, "[Cooldown Reduction Op on Num Hit]");
		if (m_cooldownFlatReduction != 0)
		{
			desc += new StringBuilder().Append("[Flat Cooldown Reduction (after applying modValueOnNumHit)] = ").Append(m_cooldownFlatReduction).Append("\n").ToString();
		}
		if (m_maxCooldownReduction > 0)
		{
			desc += new StringBuilder().Append("[Max Cooldown Reduction] = ").Append(m_maxCooldownReduction).Append("\n").ToString();
		}
		return desc;
	}
}
