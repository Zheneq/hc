using AbilityContextNamespace;
using System.Collections.Generic;

public class GenericAbility_AbilityMod : AbilityMod
{
	[Separator("On Hit Data Mod", "yellow")]
	public OnHitDataMod m_onHitDataMod;

	public override OnHitAuthoredData GenModImpl_GetModdedOnHitData(OnHitAuthoredData onHitDataFromBase)
	{
		return m_onHitDataMod._001D(onHitDataFromBase);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		GenericAbility_Container genericAbility_Container = targetAbility as GenericAbility_Container;
		if (!(genericAbility_Container != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AddOnHitDataTokens(tokens, m_onHitDataMod, genericAbility_Container.m_onHitData);
			return;
		}
	}

	protected void AddOnHitDataTokens(List<TooltipTokenEntry> tokens, OnHitDataMod mod, OnHitAuthoredData baseData)
	{
		if (mod == null)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (baseData != null)
			{
				mod._001D(tokens, baseData);
			}
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		string text = string.Empty;
		GenericAbility_Container genericAbility_Container = GetTargetAbilityOnAbilityData(abilityData) as GenericAbility_Container;
		if (genericAbility_Container != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			text += GetOnHitDataDesc(m_onHitDataMod, genericAbility_Container.m_onHitData);
		}
		return text;
	}

	protected string GetOnHitDataDesc(OnHitDataMod mod, OnHitAuthoredData baseData, string header = "-- On Hit Data Mod --")
	{
		if (mod != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (baseData != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return mod._001D(header, baseData);
					}
				}
			}
		}
		return string.Empty;
	}

	protected string GetTargetSelectModDesc(TargetSelectModBase mod, GenericAbility_TargetSelectBase baseTargetSelect, string header = "-- Target Select Mod --")
	{
		if (mod != null)
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
			if (baseTargetSelect != null)
			{
				return mod.GetInEditorDesc(baseTargetSelect, header);
			}
		}
		return string.Empty;
	}
}
