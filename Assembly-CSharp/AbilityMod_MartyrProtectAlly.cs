using System;
using System.Collections.Generic;
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
			AbilityMod.AddToken(tokens, m_damageReductionOnTargetMod, "DamageReductionOnTarget", string.Empty, martyrProtectAlly.m_damageReductionOnTarget);
			AbilityMod.AddToken(tokens, m_damageRedirectToCasterMod, "DamageRedirectToCaster", string.Empty, martyrProtectAlly.m_damageRedirectToCaster);
			AbilityMod.AddToken(tokens, m_techPointGainPerRedirectMod, "TechPointGainPerRedirect", string.Empty, martyrProtectAlly.m_techPointGainPerRedirect);
			AbilityMod.AddToken_EffectMod(tokens, m_laserHitEffectMod, "LaserHitEffect", martyrProtectAlly.m_laserHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_thornsEffectMod, "ThornsEffect", martyrProtectAlly.m_thornsEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_returnEffectOnEnemyMod, "ReturnEffectOnEnemy", martyrProtectAlly.m_returnEffectOnEnemy);
			AbilityMod.AddToken(tokens, m_thornsDamagePerHitMod, "ThornsDamagePerHit", string.Empty, martyrProtectAlly.m_thornsDamagePerHit);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnSelfMod, "EffectOnSelf", martyrProtectAlly.m_effectOnSelf);
			AbilityMod.AddToken(tokens, m_baseAbsorbMod, "BaseAbsorb", string.Empty, martyrProtectAlly.m_baseAbsorb);
			AbilityMod.AddToken(tokens, m_absorbPerCrystalSpentMod, "AbsorbPerCrystalSpent", string.Empty, martyrProtectAlly.m_absorbPerCrystalSpent);
			AbilityMod.AddToken(tokens, m_baseAbsorbOnAllyMod, "BaseAbsorbOnAlly", string.Empty, martyrProtectAlly.m_baseAbsorbOnAlly);
			AbilityMod.AddToken(tokens, m_absorbOnAllyPerCrystalSpentMod, "AbsorbOnAllyPerCrystalSpent", string.Empty, martyrProtectAlly.m_absorbOnAllyPerCrystalSpent);
			AbilityMod.AddToken(tokens, m_extraEnergyPerRedirectDamageMod, "ExtraEnergyPerRedirectDamage", string.Empty, martyrProtectAlly.m_extraEnergyPerRedirectDamage, true, false, true);
			AbilityMod.AddToken(tokens, m_healOnTurnStartPerRedirectDamageMod, "HealOnTurnStartPerRedirectDamage", string.Empty, martyrProtectAlly.m_healOnTurnStartPerRedirectDamage, true, false, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrProtectAlly martyrProtectAlly = GetTargetAbilityOnAbilityData(abilityData) as MartyrProtectAlly;
		bool flag = martyrProtectAlly != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat damageReductionOnTargetMod = m_damageReductionOnTargetMod;
		float baseVal;
		if (flag)
		{
			baseVal = martyrProtectAlly.m_damageReductionOnTarget;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(damageReductionOnTargetMod, "[DamageReductionOnTarget]", flag, baseVal);
		empty += PropDesc(m_damageRedirectToCasterMod, "[DamageRedirectToCaster]", flag, (!flag) ? 0f : martyrProtectAlly.m_damageRedirectToCaster);
		string str2 = empty;
		AbilityModPropertyInt techPointGainPerRedirectMod = m_techPointGainPerRedirectMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = martyrProtectAlly.m_techPointGainPerRedirect;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(techPointGainPerRedirectMod, "[TechPointGainPerRedirect]", flag, baseVal2);
		empty += PropDesc(m_laserHitEffectMod, "[LaserHitEffect]", flag, (!flag) ? null : martyrProtectAlly.m_laserHitEffect);
		string str3 = empty;
		AbilityModPropertyBool affectsEnemiesMod = m_affectsEnemiesMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = (martyrProtectAlly.m_affectsEnemies ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(affectsEnemiesMod, "[AffectsEnemies]", flag, (byte)baseVal3 != 0);
		string str4 = empty;
		AbilityModPropertyBool affectsAlliesMod = m_affectsAlliesMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = (martyrProtectAlly.m_affectsAllies ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(affectsAlliesMod, "[AffectsAllies]", flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyBool penetratesLoSMod = m_penetratesLoSMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = (martyrProtectAlly.m_penetratesLoS ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(penetratesLoSMod, "[PenetratesLoS]", flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertyEffectInfo thornsEffectMod = m_thornsEffectMod;
		object baseVal6;
		if (flag)
		{
			baseVal6 = martyrProtectAlly.m_thornsEffect;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(thornsEffectMod, "[ThornsEffect]", flag, (StandardEffectInfo)baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectInfo returnEffectOnEnemyMod = m_returnEffectOnEnemyMod;
		object baseVal7;
		if (flag)
		{
			baseVal7 = martyrProtectAlly.m_returnEffectOnEnemy;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(returnEffectOnEnemyMod, "[ReturnEffectOnEnemy]", flag, (StandardEffectInfo)baseVal7);
		string str8 = empty;
		AbilityModPropertyInt thornsDamagePerHitMod = m_thornsDamagePerHitMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = martyrProtectAlly.m_thornsDamagePerHit;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(thornsDamagePerHitMod, "[ThornsDamagePerHit]", flag, baseVal8);
		empty += PropDesc(m_effectOnSelfMod, "[EffectOnSelf]", flag, (!flag) ? null : martyrProtectAlly.m_effectOnSelf);
		string str9 = empty;
		AbilityModPropertyInt baseAbsorbMod = m_baseAbsorbMod;
		int baseVal9;
		if (flag)
		{
			baseVal9 = martyrProtectAlly.m_baseAbsorb;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(baseAbsorbMod, "[BaseAbsorb]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyInt absorbPerCrystalSpentMod = m_absorbPerCrystalSpentMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = martyrProtectAlly.m_absorbPerCrystalSpent;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(absorbPerCrystalSpentMod, "[AbsorbPerCrystalSpent]", flag, baseVal10);
		string str11 = empty;
		AbilityModPropertyInt baseAbsorbOnAllyMod = m_baseAbsorbOnAllyMod;
		int baseVal11;
		if (flag)
		{
			baseVal11 = martyrProtectAlly.m_baseAbsorbOnAlly;
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str11 + PropDesc(baseAbsorbOnAllyMod, "[BaseAbsorbOnAlly]", flag, baseVal11);
		empty += PropDesc(m_absorbOnAllyPerCrystalSpentMod, "[AbsorbOnAllyPerCrystalSpent]", flag, flag ? martyrProtectAlly.m_absorbOnAllyPerCrystalSpent : 0);
		empty += PropDesc(m_extraEnergyPerRedirectDamageMod, "[ExtraEnergyPerRedirectDamage]", flag, (!flag) ? 0f : martyrProtectAlly.m_extraEnergyPerRedirectDamage);
		string str12 = empty;
		AbilityModPropertyFloat healOnTurnStartPerRedirectDamageMod = m_healOnTurnStartPerRedirectDamageMod;
		float baseVal12;
		if (flag)
		{
			baseVal12 = martyrProtectAlly.m_healOnTurnStartPerRedirectDamage;
		}
		else
		{
			baseVal12 = 0f;
		}
		return str12 + PropDesc(healOnTurnStartPerRedirectDamageMod, "[HealOnTurnStartPerRedirectDamage]", flag, baseVal12);
	}
}
