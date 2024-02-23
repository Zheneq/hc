using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken_EffectMod(tokens, m_enemyBaseEffectDataMod, "EnemyBaseEffectData", martyrAoeOnReactHit.m_enemyBaseEffectData);
			AddToken_EffectMod(tokens, m_allyBaseEffectDataMod, "AllyBaseEffectData", martyrAoeOnReactHit.m_allyBaseEffectData);
			AddToken(tokens, m_extraAbsorbPerCrystalMod, "ExtraAbsorbPerCrystal", "", martyrAoeOnReactHit.m_extraAbsorbPerCrystal);
			AddToken(tokens, m_reactBaseRadiusMod, "ReactBaseRadius", "", martyrAoeOnReactHit.m_reactBaseRadius);
			AddToken(tokens, m_reactRadiusPerCrystalMod, "ReactRadiusPerCrystal", "", martyrAoeOnReactHit.m_reactRadiusPerCrystal);
			AddToken(tokens, m_reactAoeDamageMod, "ReactAoeDamage", "", martyrAoeOnReactHit.m_reactAoeDamage);
			AddToken(tokens, m_reactDamagePerCrystalMod, "ReactDamagePerCrystal", "", martyrAoeOnReactHit.m_reactDamagePerCrystal);
			AddToken_EffectMod(tokens, m_reactEnemyHitEffectMod, "ReactEnemyHitEffect", martyrAoeOnReactHit.m_reactEnemyHitEffect);
			AddToken(tokens, m_reactHealOnTargetMod, "ReactHealOnTarget", "", martyrAoeOnReactHit.m_reactHealOnTarget);
			AddToken(tokens, m_reactEnergyOnCasterPerReactMod, "ReactEnergyOnCasterPerReact", "", martyrAoeOnReactHit.m_reactEnergyOnCasterPerReact);
			AddToken(tokens, m_cdrIfNoReactionTriggeredMod, "CdrIfNoReactionTriggered", "", martyrAoeOnReactHit.m_cdrIfNoReactionTriggered);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrAoeOnReactHit martyrAoeOnReactHit = GetTargetAbilityOnAbilityData(abilityData) as MartyrAoeOnReactHit;
		bool isValid = martyrAoeOnReactHit != null;
		string desc = "";
		desc += PropDesc(m_canTargetEnemyMod, "[CanTargetEnemy]", isValid, isValid && martyrAoeOnReactHit.m_canTargetEnemy);
		desc += PropDesc(m_canTargetAllyMod, "[CanTargetAlly]", isValid, isValid && martyrAoeOnReactHit.m_canTargetAlly);
		desc += PropDesc(m_canTargetSelfMod, "[CanTargetSelf]", isValid, isValid && martyrAoeOnReactHit.m_canTargetSelf);
		desc += PropDesc(m_targetingIgnoreLosMod, "[TargetingIgnoreLos]", isValid, isValid && martyrAoeOnReactHit.m_targetingIgnoreLos);
		desc += PropDesc(m_enemyBaseEffectDataMod, "[EnemyBaseEffectData]", isValid, isValid ? martyrAoeOnReactHit.m_enemyBaseEffectData : null);
		desc += PropDesc(m_allyBaseEffectDataMod, "[AllyBaseEffectData]", isValid, isValid ? martyrAoeOnReactHit.m_allyBaseEffectData : null);
		desc += PropDesc(m_extraAbsorbPerCrystalMod, "[ExtraAbsorbPerCrystal]", isValid, isValid ? martyrAoeOnReactHit.m_extraAbsorbPerCrystal : 0);
		desc += PropDesc(m_reactBaseRadiusMod, "[ReactBaseRadius]", isValid, isValid ? martyrAoeOnReactHit.m_reactBaseRadius : 0f);
		desc += PropDesc(m_reactRadiusPerCrystalMod, "[ReactRadiusPerCrystal]", isValid, isValid ? martyrAoeOnReactHit.m_reactRadiusPerCrystal : 0f);
		desc += PropDesc(m_reactOnlyOncePerTurnMod, "[ReactOnlyOncePerTurn]", isValid, isValid && martyrAoeOnReactHit.m_reactOnlyOncePerTurn);
		desc += PropDesc(m_reactPenetrateLosMod, "[ReactPenetrateLos]", isValid, isValid && martyrAoeOnReactHit.m_reactPenetrateLos);
		desc += PropDesc(m_reactIncludeEffectTargetMod, "[ReactIncludeEffectTarget]", isValid, isValid && martyrAoeOnReactHit.m_reactIncludeEffectTarget);
		desc += PropDesc(m_reactAoeDamageMod, "[ReactAoeDamage]", isValid, isValid ? martyrAoeOnReactHit.m_reactAoeDamage : 0);
		desc += PropDesc(m_reactDamagePerCrystalMod, "[ReactDamagePerCrystal]", isValid, isValid ? martyrAoeOnReactHit.m_reactDamagePerCrystal : 0);
		desc += PropDesc(m_reactEnemyHitEffectMod, "[ReactEnemyHitEffect]", isValid, isValid ? martyrAoeOnReactHit.m_reactEnemyHitEffect : null);
		desc += PropDesc(m_reactHealOnTargetMod, "[ReactHealOnTarget]", isValid, isValid ? martyrAoeOnReactHit.m_reactHealOnTarget : 0);
		desc += PropDesc(m_reactEnergyOnCasterPerReactMod, "[ReactEnergyOnCasterPerReact]", isValid, isValid ? martyrAoeOnReactHit.m_reactEnergyOnCasterPerReact : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_cdrIfNoReactionTriggeredMod, "[CdrIfNoReactionTriggered]", isValid, isValid ? martyrAoeOnReactHit.m_cdrIfNoReactionTriggered : 0)).ToString();
	}
}
