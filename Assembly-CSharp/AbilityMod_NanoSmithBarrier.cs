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
			AbilityMod.AddToken_BarrierMod(tokens, this.m_barrierDataMod, "BarrierData", nanoSmithBarrier.m_barrierData);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NanoSmithBarrier nanoSmithBarrier = base.GetTargetAbilityOnAbilityData(abilityData) as NanoSmithBarrier;
		bool flag = nanoSmithBarrier != null;
		string empty = string.Empty;
		return empty + AbilityModHelper.GetModPropertyDesc(this.m_barrierDataMod, "{ Barrier Data (with mod) }", (!flag) ? null : nanoSmithBarrier.m_barrierData);
	}
}
