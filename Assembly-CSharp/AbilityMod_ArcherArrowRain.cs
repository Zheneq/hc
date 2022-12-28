using System;
using System.Collections.Generic;

public class AbilityMod_ArcherArrowRain : AbilityMod
{
	[Separator("Targeting Info")]
	public AbilityModPropertyFloat m_startRadiusMod;
	public AbilityModPropertyFloat m_endRadiusMod;
	public AbilityModPropertyFloat m_lineRadiusMod;
	public AbilityModPropertyFloat m_minRangeBetweenMod;
	public AbilityModPropertyFloat m_maxRangeBetweenMod;
	public AbilityModPropertyBool m_linePenetrateLoSMod;
	public AbilityModPropertyBool m_aoePenetrateLoSMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	[Separator("Enemy Hit")]
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
			AddToken(tokens, m_startRadiusMod, "StartRadius", string.Empty, archerArrowRain.m_startRadius);
			AddToken(tokens, m_endRadiusMod, "EndRadius", string.Empty, archerArrowRain.m_endRadius);
			AddToken(tokens, m_lineRadiusMod, "LineRadius", string.Empty, archerArrowRain.m_lineRadius);
			AddToken(tokens, m_minRangeBetweenMod, "MinRangeBetween", string.Empty, archerArrowRain.m_minRangeBetween);
			AddToken(tokens, m_maxRangeBetweenMod, "MaxRangeBetween", string.Empty, archerArrowRain.m_maxRangeBetween);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, archerArrowRain.m_maxTargets);
			AddToken(tokens, m_damageMod, "Damage", string.Empty, archerArrowRain.m_damage);
			AddToken(tokens, m_damageBelowHealthThresholdMod, "DamageBelowHealthThreshold", string.Empty, archerArrowRain.m_damage);
			AddToken(tokens, m_healthThresholdForDamageMod, "HealthThresholdForBonusDamage", string.Empty, 0f, true, false, true);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", archerArrowRain.m_enemyHitEffect);
			AddToken_EffectMod(tokens, m_additionalEnemyHitEffect, "AdditionalEnemyHitEffect");
			AddToken_EffectMod(tokens, m_singleEnemyHitEffectMod, "SingleEnemyHitEffect");
			AddToken(tokens, m_techPointRefundNoHits, "EnergyRefundIfNoTargetsHit", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherArrowRain archerArrowRain = GetTargetAbilityOnAbilityData(abilityData) as ArcherArrowRain;
		bool isValid = archerArrowRain != null;
		string desc = string.Empty;
		desc += PropDesc(m_startRadiusMod, "[StartRadius]", isValid, isValid ? archerArrowRain.m_startRadius : 0f);
		desc += PropDesc(m_endRadiusMod, "[EndRadius]", isValid, isValid ? archerArrowRain.m_endRadius : 0f);
		desc += PropDesc(m_lineRadiusMod, "[LineRadius]", isValid, isValid ? archerArrowRain.m_lineRadius : 0f);
		desc += PropDesc(m_minRangeBetweenMod, "[MinRangeBetween]", isValid, isValid ? archerArrowRain.m_minRangeBetween : 0f);
		desc += PropDesc(m_maxRangeBetweenMod, "[MaxRangeBetween]", isValid, isValid ? archerArrowRain.m_maxRangeBetween : 0f);
		desc += PropDesc(m_linePenetrateLoSMod, "[LinePenetrateLoS]", isValid, isValid && archerArrowRain.m_linePenetrateLoS);
		desc += PropDesc(m_aoePenetrateLoSMod, "[AoePenetrateLoS]", isValid, isValid && archerArrowRain.m_aoePenetrateLoS);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? archerArrowRain.m_maxTargets : 0);
		desc += PropDesc(m_damageMod, "[Damage]", isValid, isValid ? archerArrowRain.m_damage : 0);
		desc += PropDesc(m_damageBelowHealthThresholdMod, "[DamageBelowHealthThreshold]", isValid, isValid ? archerArrowRain.m_damage : 0);
		desc += PropDesc(m_healthThresholdForDamageMod, "[HealthThresholdForBonusDamage]", isValid);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isValid, isValid ? archerArrowRain.m_enemyHitEffect : null);
		desc += PropDesc(m_additionalEnemyHitEffect, "[AdditionalEnemyHitEffect]");
		desc += PropDesc(m_singleEnemyHitEffectMod, "[SingleEnemyHitEffect]");
		return desc + PropDesc(m_techPointRefundNoHits, "[EnergyRefundIfNoTargetsHit]", isValid);
	}
}
