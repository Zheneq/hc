using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_MartyrProtectAlly : AbilityMod
{
	[Header("-- Damage reduction and redirection")]
	public AbilityModPropertyFloat m_damageReductionOnTargetMod;
	public AbilityModPropertyFloat m_damageRedirectToCasterMod;
	public AbilityModPropertyInt m_techPointGainPerRedirectMod;
	public AbilityModPropertyEffectInfo m_laserHitEffectMod;
	[Space(10f)]
	public AbilityModPropertyBool m_affectsEnemiesMod;
	public AbilityModPropertyBool m_affectsAlliesMod;
	public AbilityModPropertyBool m_penetratesLoSMod;
	[Header("-- Thorns effect on protected ally")]
	public AbilityModPropertyEffectInfo m_thornsEffectMod;
	public AbilityModPropertyEffectInfo m_returnEffectOnEnemyMod;
	public AbilityModPropertyInt m_thornsDamagePerHitMod;
	[Header("-- Absorb & Crystal Bonuses, Self")]
	public AbilityModPropertyEffectInfo m_effectOnSelfMod;
	public AbilityModPropertyInt m_baseAbsorbMod;
	public AbilityModPropertyInt m_absorbPerCrystalSpentMod;
	[Header("-- Absorb on Ally")]
	public AbilityModPropertyInt m_baseAbsorbOnAllyMod;
	public AbilityModPropertyInt m_absorbOnAllyPerCrystalSpentMod;
	[Header("-- Extra Energy per damage redirect")]
	public AbilityModPropertyFloat m_extraEnergyPerRedirectDamageMod;
	[Header("-- Heal per damage redirect on next turn")]
	public AbilityModPropertyFloat m_healOnTurnStartPerRedirectDamageMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(MartyrProtectAlly);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MartyrProtectAlly martyrProtectAlly = targetAbility as MartyrProtectAlly;
		if (martyrProtectAlly != null)
		{
			AddToken(tokens, m_damageReductionOnTargetMod, "DamageReductionOnTarget", "", martyrProtectAlly.m_damageReductionOnTarget);
			AddToken(tokens, m_damageRedirectToCasterMod, "DamageRedirectToCaster", "", martyrProtectAlly.m_damageRedirectToCaster);
			AddToken(tokens, m_techPointGainPerRedirectMod, "TechPointGainPerRedirect", "", martyrProtectAlly.m_techPointGainPerRedirect);
			AddToken_EffectMod(tokens, m_laserHitEffectMod, "LaserHitEffect", martyrProtectAlly.m_laserHitEffect);
			AddToken_EffectMod(tokens, m_thornsEffectMod, "ThornsEffect", martyrProtectAlly.m_thornsEffect);
			AddToken_EffectMod(tokens, m_returnEffectOnEnemyMod, "ReturnEffectOnEnemy", martyrProtectAlly.m_returnEffectOnEnemy);
			AddToken(tokens, m_thornsDamagePerHitMod, "ThornsDamagePerHit", "", martyrProtectAlly.m_thornsDamagePerHit);
			AddToken_EffectMod(tokens, m_effectOnSelfMod, "EffectOnSelf", martyrProtectAlly.m_effectOnSelf);
			AddToken(tokens, m_baseAbsorbMod, "BaseAbsorb", "", martyrProtectAlly.m_baseAbsorb);
			AddToken(tokens, m_absorbPerCrystalSpentMod, "AbsorbPerCrystalSpent", "", martyrProtectAlly.m_absorbPerCrystalSpent);
			AddToken(tokens, m_baseAbsorbOnAllyMod, "BaseAbsorbOnAlly", "", martyrProtectAlly.m_baseAbsorbOnAlly);
			AddToken(tokens, m_absorbOnAllyPerCrystalSpentMod, "AbsorbOnAllyPerCrystalSpent", "", martyrProtectAlly.m_absorbOnAllyPerCrystalSpent);
			AddToken(tokens, m_extraEnergyPerRedirectDamageMod, "ExtraEnergyPerRedirectDamage", "", martyrProtectAlly.m_extraEnergyPerRedirectDamage, true, false, true);
			AddToken(tokens, m_healOnTurnStartPerRedirectDamageMod, "HealOnTurnStartPerRedirectDamage", "", martyrProtectAlly.m_healOnTurnStartPerRedirectDamage, true, false, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrProtectAlly martyrProtectAlly = GetTargetAbilityOnAbilityData(abilityData) as MartyrProtectAlly;
		bool isValid = martyrProtectAlly != null;
		string desc = "";
		desc += PropDesc(m_damageReductionOnTargetMod, "[DamageReductionOnTarget]", isValid, isValid ? martyrProtectAlly.m_damageReductionOnTarget : 0f);
		desc += PropDesc(m_damageRedirectToCasterMod, "[DamageRedirectToCaster]", isValid, isValid ? martyrProtectAlly.m_damageRedirectToCaster : 0f);
		desc += PropDesc(m_techPointGainPerRedirectMod, "[TechPointGainPerRedirect]", isValid, isValid ? martyrProtectAlly.m_techPointGainPerRedirect : 0);
		desc += PropDesc(m_laserHitEffectMod, "[LaserHitEffect]", isValid, isValid ? martyrProtectAlly.m_laserHitEffect : null);
		desc += PropDesc(m_affectsEnemiesMod, "[AffectsEnemies]", isValid, isValid && martyrProtectAlly.m_affectsEnemies);
		desc += PropDesc(m_affectsAlliesMod, "[AffectsAllies]", isValid, isValid && martyrProtectAlly.m_affectsAllies);
		desc += PropDesc(m_penetratesLoSMod, "[PenetratesLoS]", isValid, isValid && martyrProtectAlly.m_penetratesLoS);
		desc += PropDesc(m_thornsEffectMod, "[ThornsEffect]", isValid, isValid ? martyrProtectAlly.m_thornsEffect : null);
		desc += PropDesc(m_returnEffectOnEnemyMod, "[ReturnEffectOnEnemy]", isValid, isValid ? martyrProtectAlly.m_returnEffectOnEnemy : null);
		desc += PropDesc(m_thornsDamagePerHitMod, "[ThornsDamagePerHit]", isValid, isValid ? martyrProtectAlly.m_thornsDamagePerHit : 0);
		desc += PropDesc(m_effectOnSelfMod, "[EffectOnSelf]", isValid, isValid ? martyrProtectAlly.m_effectOnSelf : null);
		desc += PropDesc(m_baseAbsorbMod, "[BaseAbsorb]", isValid, isValid ? martyrProtectAlly.m_baseAbsorb : 0);
		desc += PropDesc(m_absorbPerCrystalSpentMod, "[AbsorbPerCrystalSpent]", isValid, isValid ? martyrProtectAlly.m_absorbPerCrystalSpent : 0);
		desc += PropDesc(m_baseAbsorbOnAllyMod, "[BaseAbsorbOnAlly]", isValid, isValid ? martyrProtectAlly.m_baseAbsorbOnAlly : 0);
		desc += PropDesc(m_absorbOnAllyPerCrystalSpentMod, "[AbsorbOnAllyPerCrystalSpent]", isValid, isValid ? martyrProtectAlly.m_absorbOnAllyPerCrystalSpent : 0);
		desc += PropDesc(m_extraEnergyPerRedirectDamageMod, "[ExtraEnergyPerRedirectDamage]", isValid, isValid ? martyrProtectAlly.m_extraEnergyPerRedirectDamage : 0f);
		return new StringBuilder().Append(desc).Append(PropDesc(m_healOnTurnStartPerRedirectDamageMod, "[HealOnTurnStartPerRedirectDamage]", isValid, isValid ? martyrProtectAlly.m_healOnTurnStartPerRedirectDamage : 0f)).ToString();
	}
}
