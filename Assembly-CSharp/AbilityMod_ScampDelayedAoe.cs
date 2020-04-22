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
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScampDelayedAoe scampDelayedAoe = targetAbility as ScampDelayedAoe;
		if (!(scampDelayedAoe != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_EffectMod(tokens, m_delayedEffectBaseMod, "DelayedEffectBase", scampDelayedAoe.m_delayedEffectBase);
			AbilityMod.AddToken(tokens, m_subseqTurnDamageMultiplierMod, "SubseqTurnDamageMultiplier", string.Empty, scampDelayedAoe.m_subseqTurnDamageMultiplier);
			AbilityMod.AddToken(tokens, m_extraDamageIfShieldDownFormMod, "ExtraDamageIfShieldDownForm", string.Empty, scampDelayedAoe.m_extraDamageIfShieldDownForm);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampDelayedAoe scampDelayedAoe = GetTargetAbilityOnAbilityData(abilityData) as ScampDelayedAoe;
		bool flag = scampDelayedAoe != null;
		string str = base.ModSpecificAutogenDesc(abilityData);
		str += GetTargetSelectModDesc(m_targetSelectMod, scampDelayedAoe.m_targetSelectComp, "-- Target Select --");
		str += PropDesc(m_delayedEffectBaseMod, "[DelayedEffectBase]", flag, (!flag) ? null : scampDelayedAoe.m_delayedEffectBase);
		str += PropDesc(m_extraDamageIfShieldDownFormMod, "[ExtraDamageIfShieldDownForm]", flag, flag ? scampDelayedAoe.m_extraDamageIfShieldDownForm : 0);
		str += PropDesc(m_subseqTurnDamageMultiplierMod, "[SubseqTurnDamageMultiplier]", flag, (!flag) ? 0f : scampDelayedAoe.m_subseqTurnDamageMultiplier);
		string str2 = str;
		AbilityModPropertyBool subseqTurnNoEnergyGainMod = m_subseqTurnNoEnergyGainMod;
		int baseVal;
		if (flag)
		{
			baseVal = (scampDelayedAoe.m_subseqTurnNoEnergyGain ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		return str2 + PropDesc(subseqTurnNoEnergyGainMod, "[SubseqTurnNoEnergyGain]", flag, (byte)baseVal != 0);
	}
}
