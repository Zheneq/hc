using System;
using System.Collections.Generic;
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
		if (!(sorceressDebuffLaser != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_laserWidthMod, "Width", string.Empty, sorceressDebuffLaser.m_width);
			AbilityMod.AddToken(tokens, m_laserRangeMod, "Distance", string.Empty, sorceressDebuffLaser.m_distance);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectOverride, "EnemyHitEffect", sorceressDebuffLaser.m_enemyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_allyHitEffectOverride, "AllyHitEffect", sorceressDebuffLaser.m_allyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_casterHitEffectOverride, "CasterHitEffect", sorceressDebuffLaser.m_casterHitEffect);
			AbilityMod.AddToken(tokens, m_cooldownReductionOnNumHit, "CDR_OnNumHit", "cooldown reduction per hit", 1, false);
			if (m_cooldownFlatReduction > 0)
			{
				AbilityMod.AddToken_IntDiff(tokens, "CDR_FlatAmount", "cooldown reduction", m_cooldownFlatReduction, false, 0);
			}
			AbilityMod.AddToken_IntDiff(tokens, "CDR_Max", "max cooldown reduction", m_maxCooldownReduction, false, 0);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SorceressDebuffLaser sorceressDebuffLaser = GetTargetAbilityOnAbilityData(abilityData) as SorceressDebuffLaser;
		bool flag = sorceressDebuffLaser != null;
		string empty = string.Empty;
		empty += AbilityModHelper.GetModPropertyDesc(m_laserWidthMod, "[Laser Width]", flag, (!flag) ? 0f : sorceressDebuffLaser.m_width);
		string str = empty;
		AbilityModPropertyFloat laserRangeMod = m_laserRangeMod;
		float baseVal;
		if (flag)
		{
			baseVal = sorceressDebuffLaser.m_distance;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(laserRangeMod, "[Laser Range]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt enemyEffectDurationMod = m_enemyEffectDurationMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = sorceressDebuffLaser.m_enemyHitEffect.m_effectData.m_duration;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(enemyEffectDurationMod, "[Enemy Effect Duration Mod]", flag, baseVal2);
		empty += AbilityModHelper.GetModPropertyDesc(m_allyEffectDurationMod, "[Ally Effect Duration Mod]", flag, flag ? sorceressDebuffLaser.m_allyHitEffect.m_effectData.m_duration : 0);
		string str3 = empty;
		AbilityModPropertyInt casterEffectDurationMod = m_casterEffectDurationMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = sorceressDebuffLaser.m_casterHitEffect.m_effectData.m_duration;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(casterEffectDurationMod, "[Caster Effect Duration Mod]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectOverride = m_enemyHitEffectOverride;
		object baseVal4;
		if (flag)
		{
			baseVal4 = sorceressDebuffLaser.m_enemyHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(enemyHitEffectOverride, "{ Enemy Hit Effect Override }", flag, (StandardEffectInfo)baseVal4);
		empty += AbilityModHelper.GetModPropertyDesc(m_allyHitEffectOverride, "{ Ally Hit Effect Override }", flag, (!flag) ? null : sorceressDebuffLaser.m_allyHitEffect);
		empty += AbilityModHelper.GetModPropertyDesc(m_casterHitEffectOverride, "{ Caster Hit Effect Override }", flag, (!flag) ? null : sorceressDebuffLaser.m_casterHitEffect);
		if (m_additionalEffectOnSelfIfHit != null)
		{
			if (m_additionalEffectOnSelfIfHit.m_applyEffect)
			{
				empty += AbilityModHelper.GetModEffectDataDesc(m_additionalEffectOnSelfIfHit.m_effectData, "{ Additional Effect on Self if Hit (duration = numHit) }", string.Empty);
			}
		}
		empty += AbilityModHelper.GetModPropertyDesc(m_cooldownReductionOnNumHit, "[Cooldown Reduction Op on Num Hit]");
		if (m_cooldownFlatReduction != 0)
		{
			string text = empty;
			empty = text + "[Flat Cooldown Reduction (after applying modValueOnNumHit)] = " + m_cooldownFlatReduction + "\n";
		}
		if (m_maxCooldownReduction > 0)
		{
			string text = empty;
			empty = text + "[Max Cooldown Reduction] = " + m_maxCooldownReduction + "\n";
		}
		return empty;
	}
}
