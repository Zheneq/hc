using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ScampDelayedAoe : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_AoeRadius m_targetSelectMod;

	[Separator("Effect Data for Delayed Aoe (for caster)", true)]
	public AbilityModPropertyEffectData m_delayedEffectBaseMod;

	[Separator("Extra Damage if in Shield Down form", true)]
	public AbilityModPropertyInt m_extraDamageIfShieldDownFormMod;

	[Header("-- If >= 0, will multiply on delayed AoE damage after first damage turn")]
	public AbilityModPropertyFloat m_subseqTurnDamageMultiplierMod;

	public AbilityModPropertyBool m_subseqTurnNoEnergyGainMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScampDelayedAoe);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScampDelayedAoe scampDelayedAoe = targetAbility as ScampDelayedAoe;
		if (scampDelayedAoe != null)
		{
			AbilityMod.AddToken_EffectMod(tokens, this.m_delayedEffectBaseMod, "DelayedEffectBase", scampDelayedAoe.m_delayedEffectBase, true);
			AbilityMod.AddToken(tokens, this.m_subseqTurnDamageMultiplierMod, "SubseqTurnDamageMultiplier", string.Empty, scampDelayedAoe.m_subseqTurnDamageMultiplier, true, false, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageIfShieldDownFormMod, "ExtraDamageIfShieldDownForm", string.Empty, scampDelayedAoe.m_extraDamageIfShieldDownForm, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampDelayedAoe scampDelayedAoe = base.GetTargetAbilityOnAbilityData(abilityData) as ScampDelayedAoe;
		bool flag = scampDelayedAoe != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		text += base.GetTargetSelectModDesc(this.m_targetSelectMod, scampDelayedAoe.m_targetSelectComp, "-- Target Select --");
		text += base.PropDesc(this.m_delayedEffectBaseMod, "[DelayedEffectBase]", flag, (!flag) ? null : scampDelayedAoe.m_delayedEffectBase);
		text += base.PropDesc(this.m_extraDamageIfShieldDownFormMod, "[ExtraDamageIfShieldDownForm]", flag, (!flag) ? 0 : scampDelayedAoe.m_extraDamageIfShieldDownForm);
		text += base.PropDesc(this.m_subseqTurnDamageMultiplierMod, "[SubseqTurnDamageMultiplier]", flag, (!flag) ? 0f : scampDelayedAoe.m_subseqTurnDamageMultiplier);
		string str = text;
		AbilityModPropertyBool subseqTurnNoEnergyGainMod = this.m_subseqTurnNoEnergyGainMod;
		string prefix = "[SubseqTurnNoEnergyGain]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			baseVal = scampDelayedAoe.m_subseqTurnNoEnergyGain;
		}
		else
		{
			baseVal = false;
		}
		return str + base.PropDesc(subseqTurnNoEnergyGainMod, prefix, showBaseVal, baseVal);
	}
}
