using AbilityContextNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_FireborgReactLasers : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_Laser m_targetSelectMod;

	[Separator("On Hit Data - For Second Laser", "yellow")]
	public OnHitDataMod m_onHitDataForSecondLaserMod;

	[Separator("Extra Shield", true)]
	public AbilityModPropertyInt m_extraShieldIfLowHealthMod;

	public AbilityModPropertyInt m_lowHealthThreshMod;

	[Header("-- shield per damaging hit, applied on next turn")]
	public AbilityModPropertyInt m_shieldPerHitReceivedForNextTurnMod;

	[Header("-- shield applied on next turn if depleted this turn")]
	public AbilityModPropertyInt m_earlyDepleteShieldOnNextTurnMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FireborgReactLasers);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FireborgReactLasers fireborgReactLasers = targetAbility as FireborgReactLasers;
		if (!(fireborgReactLasers != null))
		{
			return;
		}
		while (true)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddOnHitDataTokens(tokens, m_onHitDataForSecondLaserMod, fireborgReactLasers.m_onHitDataForSecondLaser);
			AbilityMod.AddToken(tokens, m_extraShieldIfLowHealthMod, "ExtraShieldIfLowHealth", string.Empty, fireborgReactLasers.m_extraShieldIfLowHealth);
			AbilityMod.AddToken(tokens, m_lowHealthThreshMod, "LowHealthThresh", string.Empty, fireborgReactLasers.m_lowHealthThresh);
			AbilityMod.AddToken(tokens, m_shieldPerHitReceivedForNextTurnMod, "ShieldPerHitReceivedForNextTurn", string.Empty, fireborgReactLasers.m_shieldPerHitReceivedForNextTurn);
			AbilityMod.AddToken(tokens, m_earlyDepleteShieldOnNextTurnMod, "EarlyDepleteShieldOnNextTurn", string.Empty, fireborgReactLasers.m_earlyDepleteShieldOnNextTurn);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FireborgReactLasers fireborgReactLasers = GetTargetAbilityOnAbilityData(abilityData) as FireborgReactLasers;
		bool flag = fireborgReactLasers != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (fireborgReactLasers != null)
		{
			text += GetTargetSelectModDesc(m_targetSelectMod, fireborgReactLasers.m_targetSelectComp, "-- Target Select --");
			text += GetOnHitDataDesc(m_onHitDataForSecondLaserMod, fireborgReactLasers.m_onHitDataForSecondLaser);
			string str = text;
			AbilityModPropertyInt extraShieldIfLowHealthMod = m_extraShieldIfLowHealthMod;
			int baseVal;
			if (flag)
			{
				baseVal = fireborgReactLasers.m_extraShieldIfLowHealth;
			}
			else
			{
				baseVal = 0;
			}
			text = str + PropDesc(extraShieldIfLowHealthMod, "[ExtraShieldIfLowHealth]", flag, baseVal);
			string str2 = text;
			AbilityModPropertyInt lowHealthThreshMod = m_lowHealthThreshMod;
			int baseVal2;
			if (flag)
			{
				baseVal2 = fireborgReactLasers.m_lowHealthThresh;
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + PropDesc(lowHealthThreshMod, "[LowHealthThresh]", flag, baseVal2);
			string str3 = text;
			AbilityModPropertyInt shieldPerHitReceivedForNextTurnMod = m_shieldPerHitReceivedForNextTurnMod;
			int baseVal3;
			if (flag)
			{
				baseVal3 = fireborgReactLasers.m_shieldPerHitReceivedForNextTurn;
			}
			else
			{
				baseVal3 = 0;
			}
			text = str3 + PropDesc(shieldPerHitReceivedForNextTurnMod, "[ShieldPerHitReceivedForNextTurn]", flag, baseVal3);
			string str4 = text;
			AbilityModPropertyInt earlyDepleteShieldOnNextTurnMod = m_earlyDepleteShieldOnNextTurnMod;
			int baseVal4;
			if (flag)
			{
				baseVal4 = fireborgReactLasers.m_earlyDepleteShieldOnNextTurn;
			}
			else
			{
				baseVal4 = 0;
			}
			text = str4 + PropDesc(earlyDepleteShieldOnNextTurnMod, "[EarlyDepleteShieldOnNextTurn]", flag, baseVal4);
		}
		return text;
	}
}
