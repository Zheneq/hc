using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ScoundrelEvasionRoll : AbilityMod
{
	[Header("-- Energy gain per step")]
	public AbilityModPropertyInt m_extraEnergyPerStepMod;
	[Header("-- Trap Wire on Start, assuming centered at start square of caster on turn start")]
	public bool m_dropTrapWireOnStart;
	public AbilityGridPattern m_trapwirePattern = AbilityGridPattern.Plus_Two_x_Two;
	public StandardBarrierData m_trapWireBarrierData;
	public GameObject m_trapwireCastSequencePrefab;
	[Header("-- Effect on Start for Absorb/Health")]
	public StandardEffectInfo m_additionalEffectOnStart;
	public GameObject m_additionalEffectCastSequencePrefab;
	[Header("-- Energy gained per adjacent ally at destination")]
	public int m_techPointGainPerAdjacentAlly;
	public int m_techPointGrantedToAdjacentAllies;
	[Header("-- Effect On Self If You End The Dash In Brush")]
	public StandardEffectInfo m_effectToSelfForLandingInBrush;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScoundrelEvasionRoll);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScoundrelEvasionRoll scoundrelEvasionRoll = targetAbility as ScoundrelEvasionRoll;
		if (scoundrelEvasionRoll != null)
		{
			AddToken(tokens, m_extraEnergyPerStepMod, "ExtraEnergyPerStep", string.Empty, scoundrelEvasionRoll.m_extraEnergyPerStep);
			if (m_dropTrapWireOnStart
				&& m_trapWireBarrierData != null
				&& m_trapwirePattern != AbilityGridPattern.NoPattern)
			{
				m_trapWireBarrierData.AddTooltipTokens(tokens, "TrapBarrier");
			}
			AddToken_EffectInfo(tokens, m_additionalEffectOnStart, "AdditionalEffectOnStart", null, false);
			AddToken_EffectInfo(tokens, m_effectToSelfForLandingInBrush, "EffectOnSelfIfLandInBrush", null, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScoundrelEvasionRoll scoundrelEvasionRoll = GetTargetAbilityOnAbilityData(abilityData) as ScoundrelEvasionRoll;
		bool isAbilityPresent = scoundrelEvasionRoll != null;
		string desc = string.Empty;
		desc += PropDesc(m_extraEnergyPerStepMod, "[ExtraEnergyPerStep]", isAbilityPresent, isAbilityPresent ? scoundrelEvasionRoll.m_extraEnergyPerStep : 0);
		if (m_dropTrapWireOnStart && m_trapWireBarrierData != null && m_trapwirePattern != AbilityGridPattern.NoPattern)
		{
			desc += "Drops TrapWire with Pattern[ " + m_trapwirePattern + " ]\n";
			desc += m_trapWireBarrierData.GetInEditorDescription("{ Barrier Data }", string.Empty, isAbilityPresent);
		}
		desc += AbilityModHelper.GetModEffectInfoDesc(m_additionalEffectOnStart, "{ Additional Effect On Start (can have Absorb/Heal) }", string.Empty, isAbilityPresent);
		if (m_techPointGainPerAdjacentAlly > 0)
		{
			desc += "[Tech Point Gain Per Adjacent Ally] = " + m_techPointGainPerAdjacentAlly + "\n";
		}
		if (m_techPointGrantedToAdjacentAllies > 0)
		{
			desc += "[Tech Point Granted To Adjacent Allies] = " + m_techPointGrantedToAdjacentAllies + "\n";
		}
		return desc + AbilityModHelper.GetModEffectInfoDesc(m_effectToSelfForLandingInBrush, "{ Effect On Self If You Land In Brush }", string.Empty, isAbilityPresent);
	}
}
