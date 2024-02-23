using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_MartyrBasicAttack : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;
	public AbilityModPropertyEffectInfo m_laserHitEffectMod;
	public AbilityModPropertyFloat m_explosionRadiusMod;
	[Header("-- Damage & Crystal Bonuses")]
	public AbilityModPropertyInt m_baseLaserDamageMod;
	public AbilityModPropertyInt m_baseExplosionDamageMod;
	public AbilityModPropertyInt m_additionalDamagePerCrystalSpentMod;
	public AbilityModPropertyFloat m_additionalRadiusPerCrystalSpentMod;
	public AbilityModPropertyInt m_extraDamageIfSingleHitMod;
	[Header("-- Inner Ring Radius and Damage")]
	public AbilityModPropertyFloat m_innerRingRadiusMod;
	public AbilityModPropertyFloat m_innerRingExtraRadiusPerCrystalMod;
	[Space(5f)]
	public AbilityModPropertyInt m_innerRingDamageMod;
	public AbilityModPropertyInt m_innerRingDamagePerCrystalMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(MartyrBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MartyrBasicAttack martyrBasicAttack = targetAbility as MartyrBasicAttack;
		if (martyrBasicAttack != null)
		{
			AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", martyrBasicAttack.m_laserInfo);
			AddToken_EffectMod(tokens, m_laserHitEffectMod, "LaserHitEffect", martyrBasicAttack.m_laserHitEffect);
			AddToken(tokens, m_explosionRadiusMod, "ExplosionRadius", "", martyrBasicAttack.m_explosionRadius);
			AddToken(tokens, m_baseLaserDamageMod, "BaseLaserDamage", "", martyrBasicAttack.m_baseLaserDamage);
			AddToken(tokens, m_baseExplosionDamageMod, "BaseExplosionDamage", "", martyrBasicAttack.m_baseExplosionDamage);
			AddToken(tokens, m_additionalDamagePerCrystalSpentMod, "AdditionalDamagePerCrystalSpent", "", martyrBasicAttack.m_additionalDamagePerCrystalSpent);
			AddToken(tokens, m_additionalRadiusPerCrystalSpentMod, "AdditionalRadiusPerCrystalSpent", "", martyrBasicAttack.m_additionalRadiusPerCrystalSpent, true, true, true);
			AddToken(tokens, m_extraDamageIfSingleHitMod, "ExtraDamageIfSingleHit", "", martyrBasicAttack.m_extraDamageIfSingleHit);
			AddToken(tokens, m_innerRingRadiusMod, "InnerRingRadius", "", martyrBasicAttack.m_innerRingRadius);
			AddToken(tokens, m_innerRingExtraRadiusPerCrystalMod, "InnerRingExtraRadiusPerCrystal", "", martyrBasicAttack.m_innerRingExtraRadiusPerCrystal);
			AddToken(tokens, m_innerRingDamageMod, "InnerRingDamage", "", martyrBasicAttack.m_innerRingDamage);
			AddToken(tokens, m_innerRingDamagePerCrystalMod, "InnerRingDamagePerCrystal", "", martyrBasicAttack.m_innerRingDamagePerCrystal);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrBasicAttack martyrBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as MartyrBasicAttack;
		bool isValid = martyrBasicAttack != null;
		string desc = "";
		desc += PropDesc(m_laserInfoMod, "[LaserInfo]", isValid, isValid ? martyrBasicAttack.m_laserInfo : null);
		desc += PropDesc(m_laserHitEffectMod, "[LaserHitEffect]", isValid, isValid ? martyrBasicAttack.m_laserHitEffect : null);
		desc += PropDesc(m_explosionRadiusMod, "[ExplosionRadius]", isValid, isValid ? martyrBasicAttack.m_explosionRadius : 0f);
		desc += PropDesc(m_baseLaserDamageMod, "[BaseLaserDamage]", isValid, isValid ? martyrBasicAttack.m_baseLaserDamage : 0);
		desc += PropDesc(m_baseExplosionDamageMod, "[BaseExplosionDamage]", isValid, isValid ? martyrBasicAttack.m_baseExplosionDamage : 0);
		desc += PropDesc(m_additionalDamagePerCrystalSpentMod, "[AdditionalDamagePerCrystalSpent]", isValid, isValid ? martyrBasicAttack.m_additionalDamagePerCrystalSpent : 0);
		desc += PropDesc(m_additionalRadiusPerCrystalSpentMod, "[AdditionalRadiusPerCrystalSpent]", isValid, isValid ? martyrBasicAttack.m_additionalRadiusPerCrystalSpent : 0f);
		desc += PropDesc(m_extraDamageIfSingleHitMod, "[ExtraDamageIfSingleHit]", isValid, isValid ? martyrBasicAttack.m_extraDamageIfSingleHit : 0);
		desc += PropDesc(m_innerRingRadiusMod, "[InnerRingRadius]", isValid, isValid ? martyrBasicAttack.m_innerRingRadius : 0f);
		desc += PropDesc(m_innerRingExtraRadiusPerCrystalMod, "[InnerRingExtraRadiusPerCrystal]", isValid, isValid ? martyrBasicAttack.m_innerRingExtraRadiusPerCrystal : 0f);
		desc += PropDesc(m_innerRingDamageMod, "[InnerRingDamage]", isValid, isValid ? martyrBasicAttack.m_innerRingDamage : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_innerRingDamagePerCrystalMod, "[InnerRingDamagePerCrystal]", isValid, isValid ? martyrBasicAttack.m_innerRingDamagePerCrystal : 0)).ToString();
	}
}
