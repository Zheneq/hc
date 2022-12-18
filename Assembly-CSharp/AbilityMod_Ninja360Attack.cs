using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_Ninja360Attack : AbilityMod
{
	[Separator("Targeting")]
	public AbilityModPropertyBool m_penetrateLineOfSightMod;
	[Header("  (( if using Laser ))")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;
	[Header("  (( if using Cone ))")]
	public AbilityModPropertyConeInfo m_coneInfoMod;
	public AbilityModPropertyFloat m_innerConeAngleMod;
	[Header("  (( if using Shape ))")]
	public AbilityModPropertyShape m_targeterShapeMod;
	[Separator("On Hit")]
	public AbilityModPropertyInt m_damageAmountMod;
	[Header("-- For Inner Area Damage --")]
	public AbilityModPropertyInt m_innerAreaDamageMod;
	[Space(10f)]
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;
	public AbilityModPropertyBool m_useDifferentEffectForInnerConeMod;
	public AbilityModPropertyEffectInfo m_innerConeEnemyHitEffectMod;
	[Header("-- Energy Gain on Marked Target --")]
	public AbilityModPropertyInt m_energyGainOnMarkedHitMod;
	public AbilityModPropertyInt m_selfHealOnMarkedHitMod;
	[Separator("[Deathmark] Effect", "magenta")]
	public AbilityModPropertyBool m_applyDeathmarkEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(Ninja360Attack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		Ninja360Attack ninja360Attack = targetAbility as Ninja360Attack;
		if (ninja360Attack != null)
		{
			AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", ninja360Attack.m_laserInfo);
			AddToken_ConeInfo(tokens, m_coneInfoMod, "ConeInfo", ninja360Attack.m_coneInfo);
			AddToken(tokens, m_innerConeAngleMod, "InnerConeAngle", string.Empty, ninja360Attack.m_innerConeAngle);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, ninja360Attack.m_damageAmount);
			AddToken(tokens, m_innerAreaDamageMod, "InnerAreaDamage", string.Empty, ninja360Attack.m_innerAreaDamage);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", ninja360Attack.m_enemyHitEffect);
			AddToken_EffectMod(tokens, m_innerConeEnemyHitEffectMod, "InnerConeEnemyHitEffect", ninja360Attack.m_innerConeEnemyHitEffect);
			AddToken(tokens, m_energyGainOnMarkedHitMod, "EnergyGainOnMarkedHit", string.Empty, ninja360Attack.m_energyGainOnMarkedHit);
			AddToken(tokens, m_selfHealOnMarkedHitMod, "SelfHealOnMarkedHit", string.Empty, ninja360Attack.m_selfHealOnMarkedHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		Ninja360Attack ninja360Attack = GetTargetAbilityOnAbilityData(abilityData) as Ninja360Attack;
		bool isValid = ninja360Attack != null;
		string desc = string.Empty;
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isValid, isValid && ninja360Attack.m_penetrateLineOfSight);
		desc += PropDesc(m_laserInfoMod, "[LaserInfo]", isValid, isValid ? ninja360Attack.m_laserInfo : null);
		desc += PropDesc(m_coneInfoMod, "[ConeInfo]", isValid, isValid ? ninja360Attack.m_coneInfo : null);
		desc += PropDesc(m_innerConeAngleMod, "[InnerConeAngle]", isValid, isValid ? ninja360Attack.m_innerConeAngle : 0f);
		desc += PropDesc(m_targeterShapeMod, "[TargeterShape]", isValid, isValid ? ninja360Attack.m_targeterShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? ninja360Attack.m_damageAmount : 0);
		desc += PropDesc(m_innerAreaDamageMod, "[InnerAreaDamage]", isValid, isValid ? ninja360Attack.m_innerAreaDamage : 0);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isValid, isValid ? ninja360Attack.m_enemyHitEffect : null);
		desc += PropDesc(m_useDifferentEffectForInnerConeMod, "[UseDifferentEffectForInnerCone]", isValid, isValid && ninja360Attack.m_useDifferentEffectForInnerCone);
		desc += PropDesc(m_innerConeEnemyHitEffectMod, "[InnerConeEnemyHitEffect]", isValid, isValid ? ninja360Attack.m_innerConeEnemyHitEffect : null);
		desc += PropDesc(m_energyGainOnMarkedHitMod, "[EnergyGainOnMarkedHit]", isValid, isValid ? ninja360Attack.m_energyGainOnMarkedHit : 0);
		desc += PropDesc(m_selfHealOnMarkedHitMod, "[SelfHealOnMarkedHit]", isValid, isValid ? ninja360Attack.m_selfHealOnMarkedHit : 0);
		return desc + PropDesc(m_applyDeathmarkEffectMod, "[ApplyDeathmarkEffect]", isValid, isValid && ninja360Attack.m_applyDeathmarkEffect);
	}
}
