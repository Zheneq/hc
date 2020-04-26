using System;
using System.Collections.Generic;
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
		if (!(martyrHealOverTime != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_healBaseMod, "HealBase", string.Empty, martyrHealOverTime.m_healBase);
			AbilityMod.AddToken(tokens, m_healPerCrystalMod, "HealPerCrystal", string.Empty, martyrHealOverTime.m_healPerCrystal);
			AbilityMod.AddToken_EffectMod(tokens, m_healEffectDataMod, "HealEffectData", martyrHealOverTime.m_healEffectData);
			AbilityMod.AddToken(tokens, m_extraHealingIfHasAoeOnReactMod, "ExtraHealingIfHasAoeOnReact", string.Empty, martyrHealOverTime.m_extraHealingIfHasAoeOnReact);
			AbilityMod.AddToken(tokens, m_lowHealthThresholdMod, "LowHealthThreshold", string.Empty, martyrHealOverTime.m_lowHealthThreshold, true, false, true);
			AbilityMod.AddToken_EffectMod(tokens, m_extraEffectForLowHealthMod, "ExtraEffectForLowHealth", martyrHealOverTime.m_extraEffectForLowHealth);
			AbilityMod.AddToken(tokens, m_baseSelfHealIfTargetAllyMod, "BaseSelfHealIfTargetAlly", string.Empty, martyrHealOverTime.m_baseSelfHealIfTargetAlly);
			AbilityMod.AddToken(tokens, m_selfHealPerCrystalIfTargetAllyMod, "SelfHealPerCrystalIfTargetAlly", string.Empty, martyrHealOverTime.m_selfHealPerCrystalIfTargetAlly);
			AbilityMod.AddToken_EffectMod(tokens, m_healEffectOnSelfIfTargetAllyMod, "HealEffectOnSelfIfTargetAlly", martyrHealOverTime.m_healEffectOnSelfIfTargetAlly);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrHealOverTime martyrHealOverTime = GetTargetAbilityOnAbilityData(abilityData) as MartyrHealOverTime;
		bool flag = martyrHealOverTime != null;
		string empty = string.Empty;
		empty += PropDesc(m_canTargetAllyMod, "[CanTargetAlly]", flag, flag && martyrHealOverTime.m_canTargetAlly);
		string str = empty;
		AbilityModPropertyBool targetingPenetrateLosMod = m_targetingPenetrateLosMod;
		int baseVal;
		if (flag)
		{
			baseVal = (martyrHealOverTime.m_targetingPenetrateLos ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(targetingPenetrateLosMod, "[TargetingPenetrateLos]", flag, (byte)baseVal != 0);
		empty += PropDesc(m_healBaseMod, "[HealBase]", flag, flag ? martyrHealOverTime.m_healBase : 0);
		string str2 = empty;
		AbilityModPropertyInt healPerCrystalMod = m_healPerCrystalMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = martyrHealOverTime.m_healPerCrystal;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(healPerCrystalMod, "[HealPerCrystal]", flag, baseVal2);
		empty += PropDesc(m_healEffectDataMod, "[HealEffectData]", flag, (!flag) ? null : martyrHealOverTime.m_healEffectData);
		empty += PropDesc(m_extraHealingIfHasAoeOnReactMod, "[ExtraHealingIfHasAoeOnReact]", flag, flag ? martyrHealOverTime.m_extraHealingIfHasAoeOnReact : 0);
		empty += PropDesc(m_onlyAddExtraEffecForFirstTurnMod, "[OnlyAddExtraEffecForFirstTurn]", flag, flag && martyrHealOverTime.m_onlyAddExtraEffecForFirstTurn);
		string str3 = empty;
		AbilityModPropertyFloat lowHealthThresholdMod = m_lowHealthThresholdMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = martyrHealOverTime.m_lowHealthThreshold;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(lowHealthThresholdMod, "[LowHealthThreshold]", flag, baseVal3);
		empty += PropDesc(m_extraEffectForLowHealthMod, "[ExtraEffectForLowHealth]", flag, (!flag) ? null : martyrHealOverTime.m_extraEffectForLowHealth);
		string str4 = empty;
		AbilityModPropertyInt baseSelfHealIfTargetAllyMod = m_baseSelfHealIfTargetAllyMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = martyrHealOverTime.m_baseSelfHealIfTargetAlly;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(baseSelfHealIfTargetAllyMod, "[BaseSelfHealIfTargetAlly]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt selfHealPerCrystalIfTargetAllyMod = m_selfHealPerCrystalIfTargetAllyMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = martyrHealOverTime.m_selfHealPerCrystalIfTargetAlly;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(selfHealPerCrystalIfTargetAllyMod, "[SelfHealPerCrystalIfTargetAlly]", flag, baseVal5);
		empty += PropDesc(m_addHealEffectOnSelfIfTargetAllyMod, "[AddHealEffectOnSelfIfTargetAlly]", flag, flag && martyrHealOverTime.m_addHealEffectOnSelfIfTargetAlly);
		string str6 = empty;
		AbilityModPropertyEffectData healEffectOnSelfIfTargetAllyMod = m_healEffectOnSelfIfTargetAllyMod;
		object baseVal6;
		if (flag)
		{
			baseVal6 = martyrHealOverTime.m_healEffectOnSelfIfTargetAlly;
		}
		else
		{
			baseVal6 = null;
		}
		return str6 + PropDesc(healEffectOnSelfIfTargetAllyMod, "[HealEffectOnSelfIfTargetAlly]", flag, (StandardActorEffectData)baseVal6);
	}
}
