using AbilityContextNamespace;
using System.Collections.Generic;

public class GenericAbility_AbilityMod : AbilityMod
{
	[Separator("On Hit Data Mod", "yellow")]
	public OnHitDataMod m_onHitDataMod;

	public override OnHitAuthoredData GenModImpl_GetModdedOnHitData(OnHitAuthoredData onHitDataFromBase)
	{
		return m_onHitDataMod.GetModdedOnHitData(onHitDataFromBase);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		GenericAbility_Container genericAbility_Container = targetAbility as GenericAbility_Container;
		if (genericAbility_Container != null)
		{
			AddOnHitDataTokens(tokens, m_onHitDataMod, genericAbility_Container.m_onHitData);
		}
	}

	protected void AddOnHitDataTokens(List<TooltipTokenEntry> tokens, OnHitDataMod mod, OnHitAuthoredData baseData)
	{
		if (mod != null && baseData != null)
		{
			mod.AddTooltipTokens(tokens, baseData);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		string text = "";
		GenericAbility_Container genericAbility_Container = GetTargetAbilityOnAbilityData(abilityData) as GenericAbility_Container;
		if (genericAbility_Container != null)
		{
			text += GetOnHitDataDesc(m_onHitDataMod, genericAbility_Container.m_onHitData);
		}
		return text;
	}

	protected string GetOnHitDataDesc(OnHitDataMod mod, OnHitAuthoredData baseData, string header = "-- On Hit Data Mod --")
	{
		if (mod != null && baseData != null)
		{
			return mod.GetInEditorDesc(header, baseData);
		}
		return "";
	}

	protected string GetTargetSelectModDesc(TargetSelectModBase mod, GenericAbility_TargetSelectBase baseTargetSelect, string header = "-- Target Select Mod --")
	{
		if (mod != null && baseTargetSelect != null)
		{
			return mod.GetInEditorDesc(baseTargetSelect, header);
		}
		return "";
	}
}
