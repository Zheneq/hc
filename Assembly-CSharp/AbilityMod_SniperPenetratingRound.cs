using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SniperPenetratingRound : AbilityMod
{
	[Header("-- Laser Mods")]
	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserRangeMod;

	[Header("-- Enemy Hit Effect Override")]
	public bool m_useEnemyHitEffectOverride;

	public StandardEffectInfo m_enemyHitEffectOverride;

	[Header("-- Knockback")]
	public bool m_knockbackHitEnemy;

	public float m_knockbackThresholdDistance = -1f;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	public float m_knockbackDistance;

	[Header("-- Damage")]
	public AbilityModPropertyInt m_laserDamage;

	[Header("-- Bonus Damage from Target Health Threshold --")]
	public AbilityModPropertyInt m_additionalDamageOnLowHealthTargetMod;

	public AbilityModPropertyFloat m_lowHealthThresholdMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SniperPenetratingRound);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SniperPenetratingRound sniperPenetratingRound = targetAbility as SniperPenetratingRound;
		if (sniperPenetratingRound != null)
		{
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, sniperPenetratingRound.m_laserInfo.width, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", string.Empty, sniperPenetratingRound.m_laserInfo.range, true, false, false);
			if (this.m_useEnemyHitEffectOverride)
			{
				AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffectOverride, "EnemyHitEffect", sniperPenetratingRound.m_laserHitEffect, true);
			}
			AbilityMod.AddToken(tokens, this.m_laserDamage, "Damage", "base damage", sniperPenetratingRound.m_laserDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_additionalDamageOnLowHealthTargetMod, "AdditionalDamageOnLowHealthTarget", string.Empty, sniperPenetratingRound.m_additionalDamageOnLowHealthTarget, true, false);
			AbilityMod.AddToken(tokens, this.m_lowHealthThresholdMod, "LowHealthThreshold", string.Empty, sniperPenetratingRound.m_lowHealthThreshold, true, false, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SniperPenetratingRound sniperPenetratingRound = base.GetTargetAbilityOnAbilityData(abilityData) as SniperPenetratingRound;
		bool flag = sniperPenetratingRound != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix = "[Laser Width]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = sniperPenetratingRound.m_laserInfo.width;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(laserWidthMod, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_laserRangeMod, "[Laser Range]", flag, (!flag) ? 0f : sniperPenetratingRound.m_laserInfo.range);
		if (this.m_useEnemyHitEffectOverride)
		{
			string str2 = text;
			StandardEffectInfo enemyHitEffectOverride = this.m_enemyHitEffectOverride;
			string prefix2 = "{ Enemy Hit Effect Override }";
			string empty = string.Empty;
			bool useBaseVal = flag;
			StandardEffectInfo baseVal2;
			if (flag)
			{
				baseVal2 = sniperPenetratingRound.m_laserHitEffect;
			}
			else
			{
				baseVal2 = null;
			}
			text = str2 + AbilityModHelper.GetModEffectInfoDesc(enemyHitEffectOverride, prefix2, empty, useBaseVal, baseVal2);
		}
		if (this.m_knockbackHitEnemy)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"Can knock back hit enemy within ",
				this.m_knockbackThresholdDistance,
				" squares\n"
			});
		}
		text += AbilityModHelper.GetModPropertyDesc(this.m_laserDamage, "[Laser Damage]", flag, (!flag) ? 0 : sniperPenetratingRound.m_laserDamageAmount);
		text += base.PropDesc(this.m_additionalDamageOnLowHealthTargetMod, "[AdditionalDamageOnLowHealthTarget]", flag, (!flag) ? 0 : sniperPenetratingRound.m_additionalDamageOnLowHealthTarget);
		string str3 = text;
		AbilityModPropertyFloat lowHealthThresholdMod = this.m_lowHealthThresholdMod;
		string prefix3 = "[LowHealthThreshold]";
		bool showBaseVal2 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = sniperPenetratingRound.m_lowHealthThreshold;
		}
		else
		{
			baseVal3 = 0f;
		}
		return str3 + base.PropDesc(lowHealthThresholdMod, prefix3, showBaseVal2, baseVal3);
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		if (this.m_useEnemyHitEffectOverride && this.m_enemyHitEffectOverride.m_applyEffect)
		{
			numbers.Add(this.m_enemyHitEffectOverride.m_effectData.m_duration - 1);
		}
	}
}
