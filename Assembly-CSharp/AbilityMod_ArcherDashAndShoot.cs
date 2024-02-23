using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken(tokens, m_maxAngleForLaserMod, "MaxAngleForLaser", string.Empty, archerDashAndShoot.m_maxAngleForLaser);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, archerDashAndShoot.m_laserWidth);
			AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, archerDashAndShoot.m_laserRange);
			AddToken(tokens, m_aoeRadiusMod, "AoeRadius", string.Empty, archerDashAndShoot.m_aoeRadius);
			AddToken(tokens, m_directDamageMod, "DirectDamage", string.Empty, archerDashAndShoot.m_directDamage);
			AddToken(tokens, m_aoeDamageMod, "AoeDamage", string.Empty, archerDashAndShoot.m_aoeDamage);
			AddToken_EffectMod(tokens, m_directTargetEffectMod, "DirectTargetEffect", archerDashAndShoot.m_directTargetEffect);
			AddToken_EffectMod(tokens, m_aoeTargetEffectMod, "AoeTargetEffect", archerDashAndShoot.m_aoeTargetEffect);
			AddToken_EffectMod(tokens, m_healingDebuffTargetEffect, "HealingDebuffTargetEffect");
			AddToken(tokens, m_cooldownAdjustmentEachTurnUnderHealthThreshold, "CooldownAdjustmentEachTurnUnderHealthThreshold", string.Empty, archerDashAndShoot.m_cooldown);
			AddToken(tokens, m_healthThresholdForCooldownOverride, "HealthThresholdForCooldownOverride", string.Empty, 0f, false, false, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherDashAndShoot archerDashAndShoot = GetTargetAbilityOnAbilityData(abilityData) as ArcherDashAndShoot;
		bool isValid = archerDashAndShoot != null;
		string desc = string.Empty;
		desc += PropDesc(m_maxAngleForLaserMod, "[MaxAngleForLaser]", isValid, isValid ? archerDashAndShoot.m_maxAngleForLaser : 0f);
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isValid, isValid ? archerDashAndShoot.m_laserWidth : 0f);
		desc += PropDesc(m_laserRangeMod, "[LaserRange]", isValid, isValid ? archerDashAndShoot.m_laserRange : 0f);
		desc += PropDesc(m_aoeRadiusMod, "[AoeRadius]", isValid, isValid ? archerDashAndShoot.m_aoeRadius : 0f);
		desc += PropDesc(m_aoePenetratesLoSMod, "[AoePenetratesLoS]", isValid, isValid && archerDashAndShoot.m_aoePenetratesLoS);
		desc += PropDesc(m_directDamageMod, "[DirectDamage]", isValid, isValid ? archerDashAndShoot.m_directDamage : 0);
		desc += PropDesc(m_aoeDamageMod, "[AoeDamage]", isValid, isValid ? archerDashAndShoot.m_aoeDamage : 0);
		desc += PropDesc(m_directTargetEffectMod, "[DirectTargetEffect]", isValid, isValid ? archerDashAndShoot.m_directTargetEffect : null);
		desc += PropDesc(m_aoeTargetEffectMod, "[AoeTargetEffect]", isValid, isValid ? archerDashAndShoot.m_aoeTargetEffect : null);
		desc += PropDesc(m_healingDebuffTargetEffect, "[HealingDebuffTargetEffect]");
		desc += PropDesc(m_cooldownAdjustmentEachTurnUnderHealthThreshold, "[CooldownAdjustmentEachTurnUnderHealthThreshold]", isValid, isValid ? archerDashAndShoot.m_cooldown : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_healthThresholdForCooldownOverride, "[HealthThresholdForCooldownOverride]", isValid)).ToString();
	}
}
