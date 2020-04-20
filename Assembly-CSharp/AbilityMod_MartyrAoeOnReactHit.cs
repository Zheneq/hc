using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_MartyrAoeOnReactHit : AbilityMod
{
	[Header("-- Targeting --")]
	public AbilityModPropertyBool m_canTargetEnemyMod;

	public AbilityModPropertyBool m_canTargetAllyMod;

	public AbilityModPropertyBool m_canTargetSelfMod;

	[Space(10f)]
	public AbilityModPropertyBool m_targetingIgnoreLosMod;

	[Header("-- Base Effect Data")]
	public AbilityModPropertyEffectData m_enemyBaseEffectDataMod;

	public AbilityModPropertyEffectData m_allyBaseEffectDataMod;

	[Header("-- Extra Shielding for Allies")]
	public AbilityModPropertyInt m_extraAbsorbPerCrystalMod;

	[Header("-- For React Area --")]
	public AbilityModPropertyFloat m_reactBaseRadiusMod;

	public AbilityModPropertyFloat m_reactRadiusPerCrystalMod;

	public AbilityModPropertyBool m_reactOnlyOncePerTurnMod;

	public AbilityModPropertyBool m_reactPenetrateLosMod;

	public AbilityModPropertyBool m_reactIncludeEffectTargetMod;

	[Header("-- On React Hit --")]
	public AbilityModPropertyInt m_reactAoeDamageMod;

	public AbilityModPropertyInt m_reactDamagePerCrystalMod;

	public AbilityModPropertyEffectInfo m_reactEnemyHitEffectMod;

	public AbilityModPropertyInt m_reactHealOnTargetMod;

	public AbilityModPropertyInt m_reactEnergyOnCasterPerReactMod;

	[Header("-- Cooldown reduction if no reacts")]
	public AbilityModPropertyInt m_cdrIfNoReactionTriggeredMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(MartyrAoeOnReactHit);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MartyrAoeOnReactHit martyrAoeOnReactHit = targetAbility as MartyrAoeOnReactHit;
		if (martyrAoeOnReactHit != null)
		{
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyBaseEffectDataMod, "EnemyBaseEffectData", martyrAoeOnReactHit.m_enemyBaseEffectData, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyBaseEffectDataMod, "AllyBaseEffectData", martyrAoeOnReactHit.m_allyBaseEffectData, true);
			AbilityMod.AddToken(tokens, this.m_extraAbsorbPerCrystalMod, "ExtraAbsorbPerCrystal", string.Empty, martyrAoeOnReactHit.m_extraAbsorbPerCrystal, true, false);
			AbilityMod.AddToken(tokens, this.m_reactBaseRadiusMod, "ReactBaseRadius", string.Empty, martyrAoeOnReactHit.m_reactBaseRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_reactRadiusPerCrystalMod, "ReactRadiusPerCrystal", string.Empty, martyrAoeOnReactHit.m_reactRadiusPerCrystal, true, false, false);
			AbilityMod.AddToken(tokens, this.m_reactAoeDamageMod, "ReactAoeDamage", string.Empty, martyrAoeOnReactHit.m_reactAoeDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_reactDamagePerCrystalMod, "ReactDamagePerCrystal", string.Empty, martyrAoeOnReactHit.m_reactDamagePerCrystal, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_reactEnemyHitEffectMod, "ReactEnemyHitEffect", martyrAoeOnReactHit.m_reactEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_reactHealOnTargetMod, "ReactHealOnTarget", string.Empty, martyrAoeOnReactHit.m_reactHealOnTarget, true, false);
			AbilityMod.AddToken(tokens, this.m_reactEnergyOnCasterPerReactMod, "ReactEnergyOnCasterPerReact", string.Empty, martyrAoeOnReactHit.m_reactEnergyOnCasterPerReact, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrIfNoReactionTriggeredMod, "CdrIfNoReactionTriggered", string.Empty, martyrAoeOnReactHit.m_cdrIfNoReactionTriggered, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrAoeOnReactHit martyrAoeOnReactHit = base.GetTargetAbilityOnAbilityData(abilityData) as MartyrAoeOnReactHit;
		bool flag = martyrAoeOnReactHit != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool canTargetEnemyMod = this.m_canTargetEnemyMod;
		string prefix = "[CanTargetEnemy]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			baseVal = martyrAoeOnReactHit.m_canTargetEnemy;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(canTargetEnemyMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyBool canTargetAllyMod = this.m_canTargetAllyMod;
		string prefix2 = "[CanTargetAlly]";
		bool showBaseVal2 = flag;
		bool baseVal2;
		if (flag)
		{
			baseVal2 = martyrAoeOnReactHit.m_canTargetAlly;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(canTargetAllyMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool canTargetSelfMod = this.m_canTargetSelfMod;
		string prefix3 = "[CanTargetSelf]";
		bool showBaseVal3 = flag;
		bool baseVal3;
		if (flag)
		{
			baseVal3 = martyrAoeOnReactHit.m_canTargetSelf;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(canTargetSelfMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool targetingIgnoreLosMod = this.m_targetingIgnoreLosMod;
		string prefix4 = "[TargetingIgnoreLos]";
		bool showBaseVal4 = flag;
		bool baseVal4;
		if (flag)
		{
			baseVal4 = martyrAoeOnReactHit.m_targetingIgnoreLos;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(targetingIgnoreLosMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectData enemyBaseEffectDataMod = this.m_enemyBaseEffectDataMod;
		string prefix5 = "[EnemyBaseEffectData]";
		bool showBaseVal5 = flag;
		StandardActorEffectData baseVal5;
		if (flag)
		{
			baseVal5 = martyrAoeOnReactHit.m_enemyBaseEffectData;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(enemyBaseEffectDataMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectData allyBaseEffectDataMod = this.m_allyBaseEffectDataMod;
		string prefix6 = "[AllyBaseEffectData]";
		bool showBaseVal6 = flag;
		StandardActorEffectData baseVal6;
		if (flag)
		{
			baseVal6 = martyrAoeOnReactHit.m_allyBaseEffectData;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(allyBaseEffectDataMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_extraAbsorbPerCrystalMod, "[ExtraAbsorbPerCrystal]", flag, (!flag) ? 0 : martyrAoeOnReactHit.m_extraAbsorbPerCrystal);
		string str7 = text;
		AbilityModPropertyFloat reactBaseRadiusMod = this.m_reactBaseRadiusMod;
		string prefix7 = "[ReactBaseRadius]";
		bool showBaseVal7 = flag;
		float baseVal7;
		if (flag)
		{
			baseVal7 = martyrAoeOnReactHit.m_reactBaseRadius;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str7 + base.PropDesc(reactBaseRadiusMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyFloat reactRadiusPerCrystalMod = this.m_reactRadiusPerCrystalMod;
		string prefix8 = "[ReactRadiusPerCrystal]";
		bool showBaseVal8 = flag;
		float baseVal8;
		if (flag)
		{
			baseVal8 = martyrAoeOnReactHit.m_reactRadiusPerCrystal;
		}
		else
		{
			baseVal8 = 0f;
		}
		text = str8 + base.PropDesc(reactRadiusPerCrystalMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyBool reactOnlyOncePerTurnMod = this.m_reactOnlyOncePerTurnMod;
		string prefix9 = "[ReactOnlyOncePerTurn]";
		bool showBaseVal9 = flag;
		bool baseVal9;
		if (flag)
		{
			baseVal9 = martyrAoeOnReactHit.m_reactOnlyOncePerTurn;
		}
		else
		{
			baseVal9 = false;
		}
		text = str9 + base.PropDesc(reactOnlyOncePerTurnMod, prefix9, showBaseVal9, baseVal9);
		text += base.PropDesc(this.m_reactPenetrateLosMod, "[ReactPenetrateLos]", flag, flag && martyrAoeOnReactHit.m_reactPenetrateLos);
		string str10 = text;
		AbilityModPropertyBool reactIncludeEffectTargetMod = this.m_reactIncludeEffectTargetMod;
		string prefix10 = "[ReactIncludeEffectTarget]";
		bool showBaseVal10 = flag;
		bool baseVal10;
		if (flag)
		{
			baseVal10 = martyrAoeOnReactHit.m_reactIncludeEffectTarget;
		}
		else
		{
			baseVal10 = false;
		}
		text = str10 + base.PropDesc(reactIncludeEffectTargetMod, prefix10, showBaseVal10, baseVal10);
		text += base.PropDesc(this.m_reactAoeDamageMod, "[ReactAoeDamage]", flag, (!flag) ? 0 : martyrAoeOnReactHit.m_reactAoeDamage);
		text += base.PropDesc(this.m_reactDamagePerCrystalMod, "[ReactDamagePerCrystal]", flag, (!flag) ? 0 : martyrAoeOnReactHit.m_reactDamagePerCrystal);
		string str11 = text;
		AbilityModPropertyEffectInfo reactEnemyHitEffectMod = this.m_reactEnemyHitEffectMod;
		string prefix11 = "[ReactEnemyHitEffect]";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal11;
		if (flag)
		{
			baseVal11 = martyrAoeOnReactHit.m_reactEnemyHitEffect;
		}
		else
		{
			baseVal11 = null;
		}
		text = str11 + base.PropDesc(reactEnemyHitEffectMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyInt reactHealOnTargetMod = this.m_reactHealOnTargetMod;
		string prefix12 = "[ReactHealOnTarget]";
		bool showBaseVal12 = flag;
		int baseVal12;
		if (flag)
		{
			baseVal12 = martyrAoeOnReactHit.m_reactHealOnTarget;
		}
		else
		{
			baseVal12 = 0;
		}
		text = str12 + base.PropDesc(reactHealOnTargetMod, prefix12, showBaseVal12, baseVal12);
		string str13 = text;
		AbilityModPropertyInt reactEnergyOnCasterPerReactMod = this.m_reactEnergyOnCasterPerReactMod;
		string prefix13 = "[ReactEnergyOnCasterPerReact]";
		bool showBaseVal13 = flag;
		int baseVal13;
		if (flag)
		{
			baseVal13 = martyrAoeOnReactHit.m_reactEnergyOnCasterPerReact;
		}
		else
		{
			baseVal13 = 0;
		}
		text = str13 + base.PropDesc(reactEnergyOnCasterPerReactMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyInt cdrIfNoReactionTriggeredMod = this.m_cdrIfNoReactionTriggeredMod;
		string prefix14 = "[CdrIfNoReactionTriggered]";
		bool showBaseVal14 = flag;
		int baseVal14;
		if (flag)
		{
			baseVal14 = martyrAoeOnReactHit.m_cdrIfNoReactionTriggered;
		}
		else
		{
			baseVal14 = 0;
		}
		return str14 + base.PropDesc(cdrIfNoReactionTriggeredMod, prefix14, showBaseVal14, baseVal14);
	}
}
