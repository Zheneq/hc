using System;
using System.Collections.Generic;
using AbilityContextNamespace;
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
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FireborgReactLasers fireborgReactLasers = targetAbility as FireborgReactLasers;
		if (fireborgReactLasers != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			base.AddOnHitDataTokens(tokens, this.m_onHitDataForSecondLaserMod, fireborgReactLasers.m_onHitDataForSecondLaser);
			AbilityMod.AddToken(tokens, this.m_extraShieldIfLowHealthMod, "ExtraShieldIfLowHealth", string.Empty, fireborgReactLasers.m_extraShieldIfLowHealth, true, false);
			AbilityMod.AddToken(tokens, this.m_lowHealthThreshMod, "LowHealthThresh", string.Empty, fireborgReactLasers.m_lowHealthThresh, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldPerHitReceivedForNextTurnMod, "ShieldPerHitReceivedForNextTurn", string.Empty, fireborgReactLasers.m_shieldPerHitReceivedForNextTurn, true, false);
			AbilityMod.AddToken(tokens, this.m_earlyDepleteShieldOnNextTurnMod, "EarlyDepleteShieldOnNextTurn", string.Empty, fireborgReactLasers.m_earlyDepleteShieldOnNextTurn, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FireborgReactLasers fireborgReactLasers = base.GetTargetAbilityOnAbilityData(abilityData) as FireborgReactLasers;
		bool flag = fireborgReactLasers != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (fireborgReactLasers != null)
		{
			text += base.GetTargetSelectModDesc(this.m_targetSelectMod, fireborgReactLasers.m_targetSelectComp, "-- Target Select --");
			text += base.GetOnHitDataDesc(this.m_onHitDataForSecondLaserMod, fireborgReactLasers.m_onHitDataForSecondLaser, "-- On Hit Data Mod --");
			string str = text;
			AbilityModPropertyInt extraShieldIfLowHealthMod = this.m_extraShieldIfLowHealthMod;
			string prefix = "[ExtraShieldIfLowHealth]";
			bool showBaseVal = flag;
			int baseVal;
			if (flag)
			{
				baseVal = fireborgReactLasers.m_extraShieldIfLowHealth;
			}
			else
			{
				baseVal = 0;
			}
			text = str + base.PropDesc(extraShieldIfLowHealthMod, prefix, showBaseVal, baseVal);
			string str2 = text;
			AbilityModPropertyInt lowHealthThreshMod = this.m_lowHealthThreshMod;
			string prefix2 = "[LowHealthThresh]";
			bool showBaseVal2 = flag;
			int baseVal2;
			if (flag)
			{
				baseVal2 = fireborgReactLasers.m_lowHealthThresh;
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + base.PropDesc(lowHealthThreshMod, prefix2, showBaseVal2, baseVal2);
			string str3 = text;
			AbilityModPropertyInt shieldPerHitReceivedForNextTurnMod = this.m_shieldPerHitReceivedForNextTurnMod;
			string prefix3 = "[ShieldPerHitReceivedForNextTurn]";
			bool showBaseVal3 = flag;
			int baseVal3;
			if (flag)
			{
				baseVal3 = fireborgReactLasers.m_shieldPerHitReceivedForNextTurn;
			}
			else
			{
				baseVal3 = 0;
			}
			text = str3 + base.PropDesc(shieldPerHitReceivedForNextTurnMod, prefix3, showBaseVal3, baseVal3);
			string str4 = text;
			AbilityModPropertyInt earlyDepleteShieldOnNextTurnMod = this.m_earlyDepleteShieldOnNextTurnMod;
			string prefix4 = "[EarlyDepleteShieldOnNextTurn]";
			bool showBaseVal4 = flag;
			int baseVal4;
			if (flag)
			{
				baseVal4 = fireborgReactLasers.m_earlyDepleteShieldOnNextTurn;
			}
			else
			{
				baseVal4 = 0;
			}
			text = str4 + base.PropDesc(earlyDepleteShieldOnNextTurnMod, prefix4, showBaseVal4, baseVal4);
		}
		return text;
	}
}
