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
		if (!(scoundrelEvasionRoll != null))
		{
			return;
		}
		AbilityMod.AddToken(tokens, m_extraEnergyPerStepMod, "ExtraEnergyPerStep", string.Empty, scoundrelEvasionRoll.m_extraEnergyPerStep);
		if (m_dropTrapWireOnStart)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_trapWireBarrierData != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_trapwirePattern != 0)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					m_trapWireBarrierData.AddTooltipTokens(tokens, "TrapBarrier");
				}
			}
		}
		AbilityMod.AddToken_EffectInfo(tokens, m_additionalEffectOnStart, "AdditionalEffectOnStart", null, false);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectToSelfForLandingInBrush, "EffectOnSelfIfLandInBrush", null, false);
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScoundrelEvasionRoll scoundrelEvasionRoll = GetTargetAbilityOnAbilityData(abilityData) as ScoundrelEvasionRoll;
		bool flag = scoundrelEvasionRoll != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt extraEnergyPerStepMod = m_extraEnergyPerStepMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = scoundrelEvasionRoll.m_extraEnergyPerStep;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(extraEnergyPerStepMod, "[ExtraEnergyPerStep]", flag, baseVal);
		if (m_dropTrapWireOnStart && m_trapWireBarrierData != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_trapwirePattern != 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				empty = empty + "Drops TrapWire with Pattern[ " + m_trapwirePattern.ToString() + " ]\n";
				empty += m_trapWireBarrierData.GetInEditorDescription("{ Barrier Data }", string.Empty, flag);
			}
		}
		empty += AbilityModHelper.GetModEffectInfoDesc(m_additionalEffectOnStart, "{ Additional Effect On Start (can have Absorb/Heal) }", string.Empty, flag);
		if (m_techPointGainPerAdjacentAlly > 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			string text = empty;
			empty = text + "[Tech Point Gain Per Adjacent Ally] = " + m_techPointGainPerAdjacentAlly + "\n";
		}
		if (m_techPointGrantedToAdjacentAllies > 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			string text = empty;
			empty = text + "[Tech Point Granted To Adjacent Allies] = " + m_techPointGrantedToAdjacentAllies + "\n";
		}
		return empty + AbilityModHelper.GetModEffectInfoDesc(m_effectToSelfForLandingInBrush, "{ Effect On Self If You Land In Brush }", string.Empty, flag);
	}
}
