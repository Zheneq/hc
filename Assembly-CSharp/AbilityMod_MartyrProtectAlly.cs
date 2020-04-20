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
			AbilityMod.AddToken(tokens, this.m_damageReductionOnTargetMod, "DamageReductionOnTarget", string.Empty, martyrProtectAlly.m_damageReductionOnTarget, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageRedirectToCasterMod, "DamageRedirectToCaster", string.Empty, martyrProtectAlly.m_damageRedirectToCaster, true, false, false);
			AbilityMod.AddToken(tokens, this.m_techPointGainPerRedirectMod, "TechPointGainPerRedirect", string.Empty, martyrProtectAlly.m_techPointGainPerRedirect, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_laserHitEffectMod, "LaserHitEffect", martyrProtectAlly.m_laserHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_thornsEffectMod, "ThornsEffect", martyrProtectAlly.m_thornsEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_returnEffectOnEnemyMod, "ReturnEffectOnEnemy", martyrProtectAlly.m_returnEffectOnEnemy, true);
			AbilityMod.AddToken(tokens, this.m_thornsDamagePerHitMod, "ThornsDamagePerHit", string.Empty, martyrProtectAlly.m_thornsDamagePerHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnSelfMod, "EffectOnSelf", martyrProtectAlly.m_effectOnSelf, true);
			AbilityMod.AddToken(tokens, this.m_baseAbsorbMod, "BaseAbsorb", string.Empty, martyrProtectAlly.m_baseAbsorb, true, false);
			AbilityMod.AddToken(tokens, this.m_absorbPerCrystalSpentMod, "AbsorbPerCrystalSpent", string.Empty, martyrProtectAlly.m_absorbPerCrystalSpent, true, false);
			AbilityMod.AddToken(tokens, this.m_baseAbsorbOnAllyMod, "BaseAbsorbOnAlly", string.Empty, martyrProtectAlly.m_baseAbsorbOnAlly, true, false);
			AbilityMod.AddToken(tokens, this.m_absorbOnAllyPerCrystalSpentMod, "AbsorbOnAllyPerCrystalSpent", string.Empty, martyrProtectAlly.m_absorbOnAllyPerCrystalSpent, true, false);
			AbilityMod.AddToken(tokens, this.m_extraEnergyPerRedirectDamageMod, "ExtraEnergyPerRedirectDamage", string.Empty, martyrProtectAlly.m_extraEnergyPerRedirectDamage, true, false, true);
			AbilityMod.AddToken(tokens, this.m_healOnTurnStartPerRedirectDamageMod, "HealOnTurnStartPerRedirectDamage", string.Empty, martyrProtectAlly.m_healOnTurnStartPerRedirectDamage, true, false, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrProtectAlly martyrProtectAlly = base.GetTargetAbilityOnAbilityData(abilityData) as MartyrProtectAlly;
		bool flag = martyrProtectAlly != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat damageReductionOnTargetMod = this.m_damageReductionOnTargetMod;
		string prefix = "[DamageReductionOnTarget]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = martyrProtectAlly.m_damageReductionOnTarget;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(damageReductionOnTargetMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_damageRedirectToCasterMod, "[DamageRedirectToCaster]", flag, (!flag) ? 0f : martyrProtectAlly.m_damageRedirectToCaster);
		string str2 = text;
		AbilityModPropertyInt techPointGainPerRedirectMod = this.m_techPointGainPerRedirectMod;
		string prefix2 = "[TechPointGainPerRedirect]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = martyrProtectAlly.m_techPointGainPerRedirect;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(techPointGainPerRedirectMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_laserHitEffectMod, "[LaserHitEffect]", flag, (!flag) ? null : martyrProtectAlly.m_laserHitEffect);
		string str3 = text;
		AbilityModPropertyBool affectsEnemiesMod = this.m_affectsEnemiesMod;
		string prefix3 = "[AffectsEnemies]";
		bool showBaseVal3 = flag;
		bool baseVal3;
		if (flag)
		{
			baseVal3 = martyrProtectAlly.m_affectsEnemies;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(affectsEnemiesMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool affectsAlliesMod = this.m_affectsAlliesMod;
		string prefix4 = "[AffectsAllies]";
		bool showBaseVal4 = flag;
		bool baseVal4;
		if (flag)
		{
			baseVal4 = martyrProtectAlly.m_affectsAllies;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(affectsAlliesMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool penetratesLoSMod = this.m_penetratesLoSMod;
		string prefix5 = "[PenetratesLoS]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = martyrProtectAlly.m_penetratesLoS;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(penetratesLoSMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo thornsEffectMod = this.m_thornsEffectMod;
		string prefix6 = "[ThornsEffect]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
		if (flag)
		{
			baseVal6 = martyrProtectAlly.m_thornsEffect;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(thornsEffectMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectInfo returnEffectOnEnemyMod = this.m_returnEffectOnEnemyMod;
		string prefix7 = "[ReturnEffectOnEnemy]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
		if (flag)
		{
			baseVal7 = martyrProtectAlly.m_returnEffectOnEnemy;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(returnEffectOnEnemyMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt thornsDamagePerHitMod = this.m_thornsDamagePerHitMod;
		string prefix8 = "[ThornsDamagePerHit]";
		bool showBaseVal8 = flag;
		int baseVal8;
		if (flag)
		{
			baseVal8 = martyrProtectAlly.m_thornsDamagePerHit;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(thornsDamagePerHitMod, prefix8, showBaseVal8, baseVal8);
		text += base.PropDesc(this.m_effectOnSelfMod, "[EffectOnSelf]", flag, (!flag) ? null : martyrProtectAlly.m_effectOnSelf);
		string str9 = text;
		AbilityModPropertyInt baseAbsorbMod = this.m_baseAbsorbMod;
		string prefix9 = "[BaseAbsorb]";
		bool showBaseVal9 = flag;
		int baseVal9;
		if (flag)
		{
			baseVal9 = martyrProtectAlly.m_baseAbsorb;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(baseAbsorbMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt absorbPerCrystalSpentMod = this.m_absorbPerCrystalSpentMod;
		string prefix10 = "[AbsorbPerCrystalSpent]";
		bool showBaseVal10 = flag;
		int baseVal10;
		if (flag)
		{
			baseVal10 = martyrProtectAlly.m_absorbPerCrystalSpent;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(absorbPerCrystalSpentMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyInt baseAbsorbOnAllyMod = this.m_baseAbsorbOnAllyMod;
		string prefix11 = "[BaseAbsorbOnAlly]";
		bool showBaseVal11 = flag;
		int baseVal11;
		if (flag)
		{
			baseVal11 = martyrProtectAlly.m_baseAbsorbOnAlly;
		}
		else
		{
			baseVal11 = 0;
		}
		text = str11 + base.PropDesc(baseAbsorbOnAllyMod, prefix11, showBaseVal11, baseVal11);
		text += base.PropDesc(this.m_absorbOnAllyPerCrystalSpentMod, "[AbsorbOnAllyPerCrystalSpent]", flag, (!flag) ? 0 : martyrProtectAlly.m_absorbOnAllyPerCrystalSpent);
		text += base.PropDesc(this.m_extraEnergyPerRedirectDamageMod, "[ExtraEnergyPerRedirectDamage]", flag, (!flag) ? 0f : martyrProtectAlly.m_extraEnergyPerRedirectDamage);
		string str12 = text;
		AbilityModPropertyFloat healOnTurnStartPerRedirectDamageMod = this.m_healOnTurnStartPerRedirectDamageMod;
		string prefix12 = "[HealOnTurnStartPerRedirectDamage]";
		bool showBaseVal12 = flag;
		float baseVal12;
		if (flag)
		{
			baseVal12 = martyrProtectAlly.m_healOnTurnStartPerRedirectDamage;
		}
		else
		{
			baseVal12 = 0f;
		}
		return str12 + base.PropDesc(healOnTurnStartPerRedirectDamageMod, prefix12, showBaseVal12, baseVal12);
	}
}
