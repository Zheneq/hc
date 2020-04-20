using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ArcherDashAndShoot : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_maxAngleForLaserMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyFloat m_aoeRadiusMod;

	public AbilityModPropertyBool m_aoePenetratesLoSMod;

	[Header("-- Enemy hits")]
	public AbilityModPropertyInt m_directDamageMod;

	public AbilityModPropertyInt m_aoeDamageMod;

	public AbilityModPropertyEffectInfo m_directTargetEffectMod;

	public AbilityModPropertyEffectInfo m_aoeTargetEffectMod;

	[Header("-- Misc ability interactions")]
	[Tooltip("if the target has the HealingDebuff effect, apply this effect to them also")]
	public AbilityModPropertyEffectInfo m_healingDebuffTargetEffect;

	public AbilityModPropertyInt m_cooldownAdjustmentEachTurnUnderHealthThreshold;

	public AbilityModPropertyFloat m_healthThresholdForCooldownOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(ArcherDashAndShoot);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ArcherDashAndShoot archerDashAndShoot = targetAbility as ArcherDashAndShoot;
		if (archerDashAndShoot != null)
		{
			AbilityMod.AddToken(tokens, this.m_maxAngleForLaserMod, "MaxAngleForLaser", string.Empty, archerDashAndShoot.m_maxAngleForLaser, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, archerDashAndShoot.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", string.Empty, archerDashAndShoot.m_laserRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_aoeRadiusMod, "AoeRadius", string.Empty, archerDashAndShoot.m_aoeRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_directDamageMod, "DirectDamage", string.Empty, archerDashAndShoot.m_directDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_aoeDamageMod, "AoeDamage", string.Empty, archerDashAndShoot.m_aoeDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_directTargetEffectMod, "DirectTargetEffect", archerDashAndShoot.m_directTargetEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_aoeTargetEffectMod, "AoeTargetEffect", archerDashAndShoot.m_aoeTargetEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_healingDebuffTargetEffect, "HealingDebuffTargetEffect", null, true);
			AbilityMod.AddToken(tokens, this.m_cooldownAdjustmentEachTurnUnderHealthThreshold, "CooldownAdjustmentEachTurnUnderHealthThreshold", string.Empty, archerDashAndShoot.m_cooldown, true, false);
			AbilityMod.AddToken(tokens, this.m_healthThresholdForCooldownOverride, "HealthThresholdForCooldownOverride", string.Empty, 0f, false, false, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherDashAndShoot archerDashAndShoot = base.GetTargetAbilityOnAbilityData(abilityData) as ArcherDashAndShoot;
		bool flag = archerDashAndShoot != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat maxAngleForLaserMod = this.m_maxAngleForLaserMod;
		string prefix = "[MaxAngleForLaser]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = archerDashAndShoot.m_maxAngleForLaser;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(maxAngleForLaserMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix2 = "[LaserWidth]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = archerDashAndShoot.m_laserWidth;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(laserWidthMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix3 = "[LaserRange]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = archerDashAndShoot.m_laserRange;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(laserRangeMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat aoeRadiusMod = this.m_aoeRadiusMod;
		string prefix4 = "[AoeRadius]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = archerDashAndShoot.m_aoeRadius;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(aoeRadiusMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_aoePenetratesLoSMod, "[AoePenetratesLoS]", flag, flag && archerDashAndShoot.m_aoePenetratesLoS);
		string str5 = text;
		AbilityModPropertyInt directDamageMod = this.m_directDamageMod;
		string prefix5 = "[DirectDamage]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = archerDashAndShoot.m_directDamage;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(directDamageMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_aoeDamageMod, "[AoeDamage]", flag, (!flag) ? 0 : archerDashAndShoot.m_aoeDamage);
		string str6 = text;
		AbilityModPropertyEffectInfo directTargetEffectMod = this.m_directTargetEffectMod;
		string prefix6 = "[DirectTargetEffect]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
		if (flag)
		{
			baseVal6 = archerDashAndShoot.m_directTargetEffect;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(directTargetEffectMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_aoeTargetEffectMod, "[AoeTargetEffect]", flag, (!flag) ? null : archerDashAndShoot.m_aoeTargetEffect);
		text += base.PropDesc(this.m_healingDebuffTargetEffect, "[HealingDebuffTargetEffect]", false, null);
		string str7 = text;
		AbilityModPropertyInt cooldownAdjustmentEachTurnUnderHealthThreshold = this.m_cooldownAdjustmentEachTurnUnderHealthThreshold;
		string prefix7 = "[CooldownAdjustmentEachTurnUnderHealthThreshold]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = archerDashAndShoot.m_cooldown;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(cooldownAdjustmentEachTurnUnderHealthThreshold, prefix7, showBaseVal7, baseVal7);
		return text + base.PropDesc(this.m_healthThresholdForCooldownOverride, "[HealthThresholdForCooldownOverride]", flag, 0f);
	}
}
