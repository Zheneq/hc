using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_ScampDelayedAoe : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod")]
	public TargetSelectMod_AoeRadius m_targetSelectMod;
	[Separator("Effect Data for Delayed Aoe (for caster)")]
	public AbilityModPropertyEffectData m_delayedEffectBaseMod;
	[Separator("Extra Damage if in Shield Down form")]
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
		if (scampDelayedAoe != null)
		{
			AddToken_EffectMod(tokens, m_delayedEffectBaseMod, "DelayedEffectBase", scampDelayedAoe.m_delayedEffectBase);
			AddToken(tokens, m_subseqTurnDamageMultiplierMod, "SubseqTurnDamageMultiplier", string.Empty, scampDelayedAoe.m_subseqTurnDamageMultiplier);
			AddToken(tokens, m_extraDamageIfShieldDownFormMod, "ExtraDamageIfShieldDownForm", string.Empty, scampDelayedAoe.m_extraDamageIfShieldDownForm);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampDelayedAoe scampDelayedAoe = GetTargetAbilityOnAbilityData(abilityData) as ScampDelayedAoe;
		bool isValid = scampDelayedAoe != null;
		string desc = base.ModSpecificAutogenDesc(abilityData);
		desc += GetTargetSelectModDesc(m_targetSelectMod, scampDelayedAoe.m_targetSelectComp, "-- Target Select --");
		desc += PropDesc(m_delayedEffectBaseMod, "[DelayedEffectBase]", isValid, isValid ? scampDelayedAoe.m_delayedEffectBase : null);
		desc += PropDesc(m_extraDamageIfShieldDownFormMod, "[ExtraDamageIfShieldDownForm]", isValid, isValid ? scampDelayedAoe.m_extraDamageIfShieldDownForm : 0);
		desc += PropDesc(m_subseqTurnDamageMultiplierMod, "[SubseqTurnDamageMultiplier]", isValid, isValid ? scampDelayedAoe.m_subseqTurnDamageMultiplier : 0f);
		return new StringBuilder().Append(desc).Append(PropDesc(m_subseqTurnNoEnergyGainMod, "[SubseqTurnNoEnergyGain]", isValid, isValid && scampDelayedAoe.m_subseqTurnNoEnergyGain)).ToString();
	}
}
