// ROGUES
// SERVER
using System;
using System.Collections.Generic;

public class AbilityMod_SpaceMarinePrimaryAttack : AbilityMod
{
	[Separator("Targeting")]
	public AbilityModPropertyLaserInfo m_laserTargetInfoMod;
	public AbilityModPropertyBool m_addConeOnFirstHitTargetMod;
	public AbilityModPropertyConeInfo m_coneTargetInfoMod;
	[Separator("Enemy Hit: Laser")]
	public AbilityModPropertyInt m_baseDamageMod;
	public AbilityModPropertyInt m_extraDamageOnClosestMod;
	[Separator("Enemy Hit: Cone")]
	public AbilityModPropertyInt m_coneDamageAmountMod;
	public AbilityModPropertyEffectInfo m_coneEnemyHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SpaceMarinePrimaryAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SpaceMarinePrimaryAttack spaceMarinePrimaryAttack = targetAbility as SpaceMarinePrimaryAttack;
		if (spaceMarinePrimaryAttack != null)
		{
			AddToken_LaserInfo(tokens, m_laserTargetInfoMod, "LaserTargetInfo", spaceMarinePrimaryAttack.m_laserTargetInfo);
			AddToken_ConeInfo(tokens, m_coneTargetInfoMod, "ConeTargetInfo", spaceMarinePrimaryAttack.m_coneTargetInfo);
			AddToken(tokens, m_baseDamageMod, "BaseDamage", string.Empty, spaceMarinePrimaryAttack.m_damageAmount);
			AddToken(tokens, m_extraDamageOnClosestMod, "ExtraDamageOnClosest", string.Empty, spaceMarinePrimaryAttack.m_extraDamageToClosestTarget);
			AddToken(tokens, m_coneDamageAmountMod, "ConeDamageAmount", string.Empty, spaceMarinePrimaryAttack.m_coneDamageAmount);
			AddToken_EffectMod(tokens, m_coneEnemyHitEffectMod, "ConeEnemyHitEffect", spaceMarinePrimaryAttack.m_coneEnemyHitEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		SpaceMarinePrimaryAttack spaceMarinePrimaryAttack = GetTargetAbilityOnAbilityData(abilityData) as SpaceMarinePrimaryAttack;
		// rogues
		// SpaceMarinePrimaryAttack spaceMarinePrimaryAttack = targetAbility as SpaceMarinePrimaryAttack;
		bool isValid = spaceMarinePrimaryAttack != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserTargetInfoMod, "[LaserTargetInfo]", isValid, isValid ? spaceMarinePrimaryAttack.m_laserTargetInfo : null);
		desc += PropDesc(m_addConeOnFirstHitTargetMod, "[AddConeOnFirstHitTarget]", isValid, isValid && spaceMarinePrimaryAttack.m_addConeOnFirstHitTarget);
		desc += PropDesc(m_coneTargetInfoMod, "[ConeTargetInfo]", isValid, isValid ? spaceMarinePrimaryAttack.m_coneTargetInfo : null);
		desc += PropDesc(m_baseDamageMod, "[DamageAmount]", isValid, isValid ? spaceMarinePrimaryAttack.m_damageAmount : 0);
		desc += PropDesc(m_extraDamageOnClosestMod, "[ExtraDamageToClosestTarget]", isValid, isValid ? spaceMarinePrimaryAttack.m_extraDamageToClosestTarget : 0);
		desc += PropDesc(m_coneDamageAmountMod, "[ConeDamageAmount]", isValid, isValid ? spaceMarinePrimaryAttack.m_coneDamageAmount : 0);
		return desc + PropDesc(m_coneEnemyHitEffectMod, "[ConeEnemyHitEffect]", isValid, isValid ? spaceMarinePrimaryAttack.m_coneEnemyHitEffect : null);
	}
}
