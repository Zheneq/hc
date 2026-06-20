// ROGUES
// SERVER
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class GenericAbility_AbilityMod : AbilityMod
{
	[Separator("On Hit Data Mod", "yellow")]
	public OnHitDataMod m_onHitDataMod;

#if SERVER
	// rogues
	//[Header("don't set directly, use the button below!")]
	//public GenericAbility_TargetSelectBase m_targetSelectOverride;

	// added in rogues
	public static string s_genericAbilityLocKey = "genericAbility";
#endif

	public override OnHitAuthoredData GenModImpl_GetModdedOnHitData(OnHitAuthoredData onHitDataFromBase) // , GenericAbility_Container targetAbility in rogues
	{
		return m_onHitDataMod.GetModdedOnHitData(onHitDataFromBase);
	}

	// added in rogues
#if SERVER
	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
	}
#endif

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

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		string text = "";
		GenericAbility_Container genericAbility_Container = GetTargetAbilityOnAbilityData(abilityData) as GenericAbility_Container;
		if (genericAbility_Container != null)
		{
			text += GetOnHitDataDesc(m_onHitDataMod, genericAbility_Container.m_onHitData);

			// rogues
			// if (m_targetSelectOverride != null)
			// {
			// 	text += $"Target select {m_targetSelectOverride.GetType()} overriding " +
			// 		((genericAbility_Container.m_targetSelectComp != null) ? genericAbility_Container.m_targetSelectComp.GetType().ToString() : "[not found]");
			// }
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

	// added in rogues
#if SERVER
	public override string GetTargetAbilityTypeName()
	{
		return s_genericAbilityLocKey;
	}
#endif
}
