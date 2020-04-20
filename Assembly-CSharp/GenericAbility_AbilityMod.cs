using System;
using System.Collections.Generic;
using AbilityContextNamespace;

public class GenericAbility_AbilityMod : AbilityMod
{
	[Separator("On Hit Data Mod", "yellow")]
	public OnHitDataMod m_onHitDataMod;

	public override OnHitAuthoredData GenModImpl_GetModdedOnHitData(OnHitAuthoredData onHitDataFromBase)
	{
		return this.m_onHitDataMod.symbol_001D(onHitDataFromBase);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		GenericAbility_Container genericAbility_Container = targetAbility as GenericAbility_Container;
		if (genericAbility_Container != null)
		{
			this.AddOnHitDataTokens(tokens, this.m_onHitDataMod, genericAbility_Container.m_onHitData);
		}
	}

	protected void AddOnHitDataTokens(List<TooltipTokenEntry> tokens, OnHitDataMod mod, OnHitAuthoredData baseData)
	{
		if (mod != null)
		{
			if (baseData != null)
			{
				mod.symbol_001D(tokens, baseData);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		string text = string.Empty;
		GenericAbility_Container genericAbility_Container = base.GetTargetAbilityOnAbilityData(abilityData) as GenericAbility_Container;
		if (genericAbility_Container != null)
		{
			text += this.GetOnHitDataDesc(this.m_onHitDataMod, genericAbility_Container.m_onHitData, "-- On Hit Data Mod --");
		}
		return text;
	}

	protected string GetOnHitDataDesc(OnHitDataMod mod, OnHitAuthoredData baseData, string header = "-- On Hit Data Mod --")
	{
		if (mod != null)
		{
			if (baseData != null)
			{
				return mod.symbol_001D(header, baseData);
			}
		}
		return string.Empty;
	}

	protected string GetTargetSelectModDesc(TargetSelectModBase mod, GenericAbility_TargetSelectBase baseTargetSelect, string header = "-- Target Select Mod --")
	{
		if (mod != null)
		{
			if (baseTargetSelect != null)
			{
				return mod.GetInEditorDesc(baseTargetSelect, header);
			}
		}
		return string.Empty;
	}
}
