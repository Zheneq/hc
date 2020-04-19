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
			AbilityMod.AddToken(tokens, this.m_extraEnergyPerStepMod, "ExtraEnergyPerStep", string.Empty, scoundrelEvasionRoll.m_extraEnergyPerStep, true, false);
			if (this.m_dropTrapWireOnStart)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ScoundrelEvasionRoll.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
				}
				if (this.m_trapWireBarrierData != null)
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
					if (this.m_trapwirePattern != AbilityGridPattern.NoPattern)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_trapWireBarrierData.AddTooltipTokens(tokens, "TrapBarrier", false, null);
					}
				}
			}
			AbilityMod.AddToken_EffectInfo(tokens, this.m_additionalEffectOnStart, "AdditionalEffectOnStart", null, false);
			AbilityMod.AddToken_EffectInfo(tokens, this.m_effectToSelfForLandingInBrush, "EffectOnSelfIfLandInBrush", null, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScoundrelEvasionRoll scoundrelEvasionRoll = base.GetTargetAbilityOnAbilityData(abilityData) as ScoundrelEvasionRoll;
		bool flag = scoundrelEvasionRoll != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt extraEnergyPerStepMod = this.m_extraEnergyPerStepMod;
		string prefix = "[ExtraEnergyPerStep]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ScoundrelEvasionRoll.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = scoundrelEvasionRoll.m_extraEnergyPerStep;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(extraEnergyPerStepMod, prefix, showBaseVal, baseVal);
		if (this.m_dropTrapWireOnStart && this.m_trapWireBarrierData != null)
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
			if (this.m_trapwirePattern != AbilityGridPattern.NoPattern)
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
				text = text + "Drops TrapWire with Pattern[ " + this.m_trapwirePattern.ToString() + " ]\n";
				text += this.m_trapWireBarrierData.GetInEditorDescription("{ Barrier Data }", string.Empty, flag, null);
			}
		}
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_additionalEffectOnStart, "{ Additional Effect On Start (can have Absorb/Heal) }", string.Empty, flag, null);
		if (this.m_techPointGainPerAdjacentAlly > 0)
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
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Tech Point Gain Per Adjacent Ally] = ",
				this.m_techPointGainPerAdjacentAlly,
				"\n"
			});
		}
		if (this.m_techPointGrantedToAdjacentAllies > 0)
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
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Tech Point Granted To Adjacent Allies] = ",
				this.m_techPointGrantedToAdjacentAllies,
				"\n"
			});
		}
		return text + AbilityModHelper.GetModEffectInfoDesc(this.m_effectToSelfForLandingInBrush, "{ Effect On Self If You Land In Brush }", string.Empty, flag, null);
	}
}
