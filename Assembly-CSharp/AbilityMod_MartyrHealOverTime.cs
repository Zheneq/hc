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
		if (martyrHealOverTime != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_MartyrHealOverTime.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_healBaseMod, "HealBase", string.Empty, martyrHealOverTime.m_healBase, true, false);
			AbilityMod.AddToken(tokens, this.m_healPerCrystalMod, "HealPerCrystal", string.Empty, martyrHealOverTime.m_healPerCrystal, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_healEffectDataMod, "HealEffectData", martyrHealOverTime.m_healEffectData, true);
			AbilityMod.AddToken(tokens, this.m_extraHealingIfHasAoeOnReactMod, "ExtraHealingIfHasAoeOnReact", string.Empty, martyrHealOverTime.m_extraHealingIfHasAoeOnReact, true, false);
			AbilityMod.AddToken(tokens, this.m_lowHealthThresholdMod, "LowHealthThreshold", string.Empty, martyrHealOverTime.m_lowHealthThreshold, true, false, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_extraEffectForLowHealthMod, "ExtraEffectForLowHealth", martyrHealOverTime.m_extraEffectForLowHealth, true);
			AbilityMod.AddToken(tokens, this.m_baseSelfHealIfTargetAllyMod, "BaseSelfHealIfTargetAlly", string.Empty, martyrHealOverTime.m_baseSelfHealIfTargetAlly, true, false);
			AbilityMod.AddToken(tokens, this.m_selfHealPerCrystalIfTargetAllyMod, "SelfHealPerCrystalIfTargetAlly", string.Empty, martyrHealOverTime.m_selfHealPerCrystalIfTargetAlly, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_healEffectOnSelfIfTargetAllyMod, "HealEffectOnSelfIfTargetAlly", martyrHealOverTime.m_healEffectOnSelfIfTargetAlly, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MartyrHealOverTime martyrHealOverTime = base.GetTargetAbilityOnAbilityData(abilityData) as MartyrHealOverTime;
		bool flag = martyrHealOverTime != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_canTargetAllyMod, "[CanTargetAlly]", flag, flag && martyrHealOverTime.m_canTargetAlly);
		string str = text;
		AbilityModPropertyBool targetingPenetrateLosMod = this.m_targetingPenetrateLosMod;
		string prefix = "[TargetingPenetrateLos]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_MartyrHealOverTime.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = martyrHealOverTime.m_targetingPenetrateLos;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(targetingPenetrateLosMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_healBaseMod, "[HealBase]", flag, (!flag) ? 0 : martyrHealOverTime.m_healBase);
		string str2 = text;
		AbilityModPropertyInt healPerCrystalMod = this.m_healPerCrystalMod;
		string prefix2 = "[HealPerCrystal]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = martyrHealOverTime.m_healPerCrystal;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(healPerCrystalMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_healEffectDataMod, "[HealEffectData]", flag, (!flag) ? null : martyrHealOverTime.m_healEffectData);
		text += base.PropDesc(this.m_extraHealingIfHasAoeOnReactMod, "[ExtraHealingIfHasAoeOnReact]", flag, (!flag) ? 0 : martyrHealOverTime.m_extraHealingIfHasAoeOnReact);
		text += base.PropDesc(this.m_onlyAddExtraEffecForFirstTurnMod, "[OnlyAddExtraEffecForFirstTurn]", flag, flag && martyrHealOverTime.m_onlyAddExtraEffecForFirstTurn);
		string str3 = text;
		AbilityModPropertyFloat lowHealthThresholdMod = this.m_lowHealthThresholdMod;
		string prefix3 = "[LowHealthThreshold]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = martyrHealOverTime.m_lowHealthThreshold;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(lowHealthThresholdMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_extraEffectForLowHealthMod, "[ExtraEffectForLowHealth]", flag, (!flag) ? null : martyrHealOverTime.m_extraEffectForLowHealth);
		string str4 = text;
		AbilityModPropertyInt baseSelfHealIfTargetAllyMod = this.m_baseSelfHealIfTargetAllyMod;
		string prefix4 = "[BaseSelfHealIfTargetAlly]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = martyrHealOverTime.m_baseSelfHealIfTargetAlly;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(baseSelfHealIfTargetAllyMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt selfHealPerCrystalIfTargetAllyMod = this.m_selfHealPerCrystalIfTargetAllyMod;
		string prefix5 = "[SelfHealPerCrystalIfTargetAlly]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = martyrHealOverTime.m_selfHealPerCrystalIfTargetAlly;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(selfHealPerCrystalIfTargetAllyMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_addHealEffectOnSelfIfTargetAllyMod, "[AddHealEffectOnSelfIfTargetAlly]", flag, flag && martyrHealOverTime.m_addHealEffectOnSelfIfTargetAlly);
		string str6 = text;
		AbilityModPropertyEffectData healEffectOnSelfIfTargetAllyMod = this.m_healEffectOnSelfIfTargetAllyMod;
		string prefix6 = "[HealEffectOnSelfIfTargetAlly]";
		bool showBaseVal6 = flag;
		StandardActorEffectData baseVal6;
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal6 = martyrHealOverTime.m_healEffectOnSelfIfTargetAlly;
		}
		else
		{
			baseVal6 = null;
		}
		return str6 + base.PropDesc(healEffectOnSelfIfTargetAllyMod, prefix6, showBaseVal6, baseVal6);
	}
}
