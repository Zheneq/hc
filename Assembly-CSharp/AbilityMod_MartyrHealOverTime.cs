using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_MartyrHealOverTime : AbilityMod
{
	[Header("-- Targeting --")]
	public AbilityModPropertyBool m_canTargetAllyMod;
	public AbilityModPropertyBool m_targetingPenetrateLosMod;
	public AbilityModPropertyInt m_healBaseMod;
	public AbilityModPropertyInt m_healPerCrystalMod;
	[Header("  (( base effect data for healing, no need to specify healing here ))")]
	public AbilityModPropertyEffectData m_healEffectDataMod;
	[Header("-- Extra healing if has Aoe on React effect")]
	public AbilityModPropertyInt m_extraHealingIfHasAoeOnReactMod;
	[Header("-- Extra Effect for low health --")]
	public AbilityModPropertyBool m_onlyAddExtraEffecForFirstTurnMod;
	public AbilityModPropertyFloat m_lowHealthThresholdMod;
	public AbilityModPropertyEffectInfo m_extraEffectForLowHealthMod;
	[Header("-- Heal/Effect on Caster if targeting Ally")]
	public AbilityModPropertyInt m_baseSelfHealIfTargetAllyMod;
	public AbilityModPropertyInt m_selfHealPerCrystalIfTargetAllyMod;
	public AbilityModPropertyBool m_addHealEffectOnSelfIfTargetAllyMod;
	public AbilityModPropertyEffectData m_healEffectOnSelfIfTargetAllyMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(MartyrHealOverTime);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MartyrHealOverTime martyrHealOverTime = targetAbility as MartyrHealOverTime;
		if (martyrHealOverTime != null)
		{
			AddToken(tokens, m_healBaseMod, "HealBase", "", martyrHealOverTime.m_healBase);
			AddToken(tokens, m_healPerCrystalMod, "HealPerCrystal", "", martyrHealOverTime.m_healPerCrystal);
			AddToken_EffectMod(tokens, m_healEffectDataMod, "HealEffectData", martyrHealOverTime.m_healEffectData);
			AddToken(tokens, m_extraHealingIfHasAoeOnReactMod, "ExtraHealingIfHasAoeOnReact", "", martyrHealOverTime.m_extraHealingIfHasAoeOnReact);
			AddToken(tokens, m_lowHealthThresholdMod, "LowHealthThreshold", "", martyrHealOverTime.m_lowHealthThreshold, true, false, true);
			AddToken_EffectMod(tokens, m_extraEffectForLowHealthMod, "ExtraEffectForLowHealth", martyrHealOverTime.m_extraEffectForLowHealth);
			AddToken(tokens, m_baseSelfHealIfTargetAllyMod, "BaseSelfHealIfTargetAlly", "", martyrHealOverTime.m_baseSelfHealIfTargetAlly);
			AddToken(tokens, m_selfHealPerCrystalIfTargetAllyMod, "SelfHealPerCrystalIfTargetAlly", "", martyrHealOverTime.m_selfHealPerCrystalIfTargetAlly);
			AddToken_EffectMod(tokens, m_healEffectOnSelfIfTargetAllyMod, "HealEffectOnSelfIfTargetAlly", martyrHealOverTime.m_healEffectOnSelfIfTargetAlly);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrHealOverTime martyrHealOverTime = GetTargetAbilityOnAbilityData(abilityData) as MartyrHealOverTime;
		bool isValid = martyrHealOverTime != null;
		string desc = "";
		desc += PropDesc(m_canTargetAllyMod, "[CanTargetAlly]", isValid, isValid && martyrHealOverTime.m_canTargetAlly);
		desc += PropDesc(m_targetingPenetrateLosMod, "[TargetingPenetrateLos]", isValid, isValid && martyrHealOverTime.m_targetingPenetrateLos);
		desc += PropDesc(m_healBaseMod, "[HealBase]", isValid, isValid ? martyrHealOverTime.m_healBase : 0);
		desc += PropDesc(m_healPerCrystalMod, "[HealPerCrystal]", isValid, isValid ? martyrHealOverTime.m_healPerCrystal : 0);
		desc += PropDesc(m_healEffectDataMod, "[HealEffectData]", isValid, isValid ? martyrHealOverTime.m_healEffectData : null);
		desc += PropDesc(m_extraHealingIfHasAoeOnReactMod, "[ExtraHealingIfHasAoeOnReact]", isValid, isValid ? martyrHealOverTime.m_extraHealingIfHasAoeOnReact : 0);
		desc += PropDesc(m_onlyAddExtraEffecForFirstTurnMod, "[OnlyAddExtraEffecForFirstTurn]", isValid, isValid && martyrHealOverTime.m_onlyAddExtraEffecForFirstTurn);
		desc += PropDesc(m_lowHealthThresholdMod, "[LowHealthThreshold]", isValid, isValid ? martyrHealOverTime.m_lowHealthThreshold : 0f);
		desc += PropDesc(m_extraEffectForLowHealthMod, "[ExtraEffectForLowHealth]", isValid, isValid ? martyrHealOverTime.m_extraEffectForLowHealth : null);
		desc += PropDesc(m_baseSelfHealIfTargetAllyMod, "[BaseSelfHealIfTargetAlly]", isValid, isValid ? martyrHealOverTime.m_baseSelfHealIfTargetAlly : 0);
		desc += PropDesc(m_selfHealPerCrystalIfTargetAllyMod, "[SelfHealPerCrystalIfTargetAlly]", isValid, isValid ? martyrHealOverTime.m_selfHealPerCrystalIfTargetAlly : 0);
		desc += PropDesc(m_addHealEffectOnSelfIfTargetAllyMod, "[AddHealEffectOnSelfIfTargetAlly]", isValid, isValid && martyrHealOverTime.m_addHealEffectOnSelfIfTargetAlly);
		return new StringBuilder().Append(desc).Append(PropDesc(m_healEffectOnSelfIfTargetAllyMod, "[HealEffectOnSelfIfTargetAlly]", isValid, isValid ? martyrHealOverTime.m_healEffectOnSelfIfTargetAlly : null)).ToString();
	}
}
