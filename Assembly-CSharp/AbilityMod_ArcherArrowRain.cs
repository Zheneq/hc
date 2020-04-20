using System;
using System.Collections.Generic;

public class AbilityMod_ArcherArrowRain : AbilityMod
{
	[Separator("Targeting Info", true)]
	public AbilityModPropertyFloat m_startRadiusMod;

	public AbilityModPropertyFloat m_endRadiusMod;

	public AbilityModPropertyFloat m_lineRadiusMod;

	public AbilityModPropertyFloat m_minRangeBetweenMod;

	public AbilityModPropertyFloat m_maxRangeBetweenMod;

	public AbilityModPropertyBool m_linePenetrateLoSMod;

	public AbilityModPropertyBool m_aoePenetrateLoSMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Separator("Enemy Hit", true)]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyInt m_damageBelowHealthThresholdMod;

	public AbilityModPropertyFloat m_healthThresholdForDamageMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	public AbilityModPropertyEffectInfo m_additionalEnemyHitEffect;

	public AbilityModPropertyEffectInfo m_singleEnemyHitEffectMod;

	public AbilityModPropertyInt m_techPointRefundNoHits;

	public override Type GetTargetAbilityType()
	{
		return typeof(ArcherArrowRain);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ArcherArrowRain archerArrowRain = targetAbility as ArcherArrowRain;
		if (archerArrowRain != null)
		{
			AbilityMod.AddToken(tokens, this.m_startRadiusMod, "StartRadius", string.Empty, archerArrowRain.m_startRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_endRadiusMod, "EndRadius", string.Empty, archerArrowRain.m_endRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_lineRadiusMod, "LineRadius", string.Empty, archerArrowRain.m_lineRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_minRangeBetweenMod, "MinRangeBetween", string.Empty, archerArrowRain.m_minRangeBetween, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxRangeBetweenMod, "MaxRangeBetween", string.Empty, archerArrowRain.m_maxRangeBetween, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, archerArrowRain.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, archerArrowRain.m_damage, true, false);
			AbilityMod.AddToken(tokens, this.m_damageBelowHealthThresholdMod, "DamageBelowHealthThreshold", string.Empty, archerArrowRain.m_damage, true, false);
			AbilityMod.AddToken(tokens, this.m_healthThresholdForDamageMod, "HealthThresholdForBonusDamage", string.Empty, 0f, true, false, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", archerArrowRain.m_enemyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_additionalEnemyHitEffect, "AdditionalEnemyHitEffect", null, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_singleEnemyHitEffectMod, "SingleEnemyHitEffect", null, true);
			AbilityMod.AddToken(tokens, this.m_techPointRefundNoHits, "EnergyRefundIfNoTargetsHit", string.Empty, 0, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherArrowRain archerArrowRain = base.GetTargetAbilityOnAbilityData(abilityData) as ArcherArrowRain;
		bool flag = archerArrowRain != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat startRadiusMod = this.m_startRadiusMod;
		string prefix = "[StartRadius]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = archerArrowRain.m_startRadius;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(startRadiusMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat endRadiusMod = this.m_endRadiusMod;
		string prefix2 = "[EndRadius]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = archerArrowRain.m_endRadius;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(endRadiusMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat lineRadiusMod = this.m_lineRadiusMod;
		string prefix3 = "[LineRadius]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = archerArrowRain.m_lineRadius;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(lineRadiusMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat minRangeBetweenMod = this.m_minRangeBetweenMod;
		string prefix4 = "[MinRangeBetween]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = archerArrowRain.m_minRangeBetween;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(minRangeBetweenMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_maxRangeBetweenMod, "[MaxRangeBetween]", flag, (!flag) ? 0f : archerArrowRain.m_maxRangeBetween);
		text += base.PropDesc(this.m_linePenetrateLoSMod, "[LinePenetrateLoS]", flag, flag && archerArrowRain.m_linePenetrateLoS);
		string str5 = text;
		AbilityModPropertyBool aoePenetrateLoSMod = this.m_aoePenetrateLoSMod;
		string prefix5 = "[AoePenetrateLoS]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = archerArrowRain.m_aoePenetrateLoS;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(aoePenetrateLoSMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_maxTargetsMod, "[MaxTargets]", flag, (!flag) ? 0 : archerArrowRain.m_maxTargets);
		text += base.PropDesc(this.m_damageMod, "[Damage]", flag, (!flag) ? 0 : archerArrowRain.m_damage);
		string str6 = text;
		AbilityModPropertyInt damageBelowHealthThresholdMod = this.m_damageBelowHealthThresholdMod;
		string prefix6 = "[DamageBelowHealthThreshold]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = archerArrowRain.m_damage;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(damageBelowHealthThresholdMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_healthThresholdForDamageMod, "[HealthThresholdForBonusDamage]", flag, 0f);
		string str7 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix7 = "[EnemyHitEffect]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
		if (flag)
		{
			baseVal7 = archerArrowRain.m_enemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(enemyHitEffectMod, prefix7, showBaseVal7, baseVal7);
		text += base.PropDesc(this.m_additionalEnemyHitEffect, "[AdditionalEnemyHitEffect]", false, null);
		text += base.PropDesc(this.m_singleEnemyHitEffectMod, "[SingleEnemyHitEffect]", false, null);
		return text + base.PropDesc(this.m_techPointRefundNoHits, "[EnergyRefundIfNoTargetsHit]", flag, 0);
	}
}
