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
		if (sorceressDebuffLaser != null)
		{
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "Width", string.Empty, sorceressDebuffLaser.m_width, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "Distance", string.Empty, sorceressDebuffLaser.m_distance, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectOverride, "EnemyHitEffect", sorceressDebuffLaser.m_enemyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyHitEffectOverride, "AllyHitEffect", sorceressDebuffLaser.m_allyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_casterHitEffectOverride, "CasterHitEffect", sorceressDebuffLaser.m_casterHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_cooldownReductionOnNumHit, "CDR_OnNumHit", "cooldown reduction per hit", 1, false, false);
			if (this.m_cooldownFlatReduction > 0)
			{
				AbilityMod.AddToken_IntDiff(tokens, "CDR_FlatAmount", "cooldown reduction", this.m_cooldownFlatReduction, false, 0);
			}
			AbilityMod.AddToken_IntDiff(tokens, "CDR_Max", "max cooldown reduction", this.m_maxCooldownReduction, false, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SorceressDebuffLaser sorceressDebuffLaser = base.GetTargetAbilityOnAbilityData(abilityData) as SorceressDebuffLaser;
		bool flag = sorceressDebuffLaser != null;
		string text = string.Empty;
		text += AbilityModHelper.GetModPropertyDesc(this.m_laserWidthMod, "[Laser Width]", flag, (!flag) ? 0f : sorceressDebuffLaser.m_width);
		string str = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix = "[Laser Range]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = sorceressDebuffLaser.m_distance;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(laserRangeMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt enemyEffectDurationMod = this.m_enemyEffectDurationMod;
		string prefix2 = "[Enemy Effect Duration Mod]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = sorceressDebuffLaser.m_enemyHitEffect.m_effectData.m_duration;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(enemyEffectDurationMod, prefix2, showBaseVal2, baseVal2);
		text += AbilityModHelper.GetModPropertyDesc(this.m_allyEffectDurationMod, "[Ally Effect Duration Mod]", flag, (!flag) ? 0 : sorceressDebuffLaser.m_allyHitEffect.m_effectData.m_duration);
		string str3 = text;
		AbilityModPropertyInt casterEffectDurationMod = this.m_casterEffectDurationMod;
		string prefix3 = "[Caster Effect Duration Mod]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = sorceressDebuffLaser.m_casterHitEffect.m_effectData.m_duration;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(casterEffectDurationMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyEffectInfo enemyHitEffectOverride = this.m_enemyHitEffectOverride;
		string prefix4 = "{ Enemy Hit Effect Override }";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
		if (flag)
		{
			baseVal4 = sorceressDebuffLaser.m_enemyHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(enemyHitEffectOverride, prefix4, showBaseVal4, baseVal4);
		text += AbilityModHelper.GetModPropertyDesc(this.m_allyHitEffectOverride, "{ Ally Hit Effect Override }", flag, (!flag) ? null : sorceressDebuffLaser.m_allyHitEffect);
		text += AbilityModHelper.GetModPropertyDesc(this.m_casterHitEffectOverride, "{ Caster Hit Effect Override }", flag, (!flag) ? null : sorceressDebuffLaser.m_casterHitEffect);
		if (this.m_additionalEffectOnSelfIfHit != null)
		{
			if (this.m_additionalEffectOnSelfIfHit.m_applyEffect)
			{
				text += AbilityModHelper.GetModEffectDataDesc(this.m_additionalEffectOnSelfIfHit.m_effectData, "{ Additional Effect on Self if Hit (duration = numHit) }", string.Empty, false, null);
			}
		}
		text += AbilityModHelper.GetModPropertyDesc(this.m_cooldownReductionOnNumHit, "[Cooldown Reduction Op on Num Hit]", false, 0);
		if (this.m_cooldownFlatReduction != 0)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Flat Cooldown Reduction (after applying modValueOnNumHit)] = ",
				this.m_cooldownFlatReduction,
				"\n"
			});
		}
		if (this.m_maxCooldownReduction > 0)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Max Cooldown Reduction] = ",
				this.m_maxCooldownReduction,
				"\n"
			});
		}
		return text;
	}
}
