using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NanoSmithBarrier : AbilityMod
{
	[Space(10f)]
	public AbilityModPropertyBarrierDataV2 m_barrierDataMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NanoSmithBarrier);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NanoSmithBarrier nanoSmithBarrier = targetAbility as NanoSmithBarrier;
		if (nanoSmithBarrier != null)
		{
			AddToken_BarrierMod(tokens, m_barrierDataMod, "BarrierData", nanoSmithBarrier.m_barrierData);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NanoSmithBarrier nanoSmithBarrier = GetTargetAbilityOnAbilityData(abilityData) as NanoSmithBarrier;
		bool isValid = nanoSmithBarrier != null;
		return AbilityModHelper.GetModPropertyDesc(m_barrierDataMod, "{ Barrier Data (with mod) }", isValid ? nanoSmithBarrier.m_barrierData : null);
	}
}
