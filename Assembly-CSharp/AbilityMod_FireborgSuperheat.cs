using System;
using System.Collections.Generic;
using System.Text;

public class AbilityMod_FireborgSuperheat : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_Shape m_targetSelectMod;

	[Separator("Superheat", true)]
	public AbilityModPropertyInt m_superheatDurationMod;

	public AbilityModPropertyInt m_igniteExtraDamageIfSuperheatedMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FireborgSuperheat);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FireborgSuperheat fireborgSuperheat = targetAbility as FireborgSuperheat;
		if (fireborgSuperheat != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, m_superheatDurationMod, "SuperheatDuration", string.Empty, fireborgSuperheat.m_superheatDuration);
			AbilityMod.AddToken(tokens, m_igniteExtraDamageIfSuperheatedMod, "IgniteExtraDamageIfSuperheated", string.Empty, fireborgSuperheat.m_igniteExtraDamageIfSuperheated);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FireborgSuperheat fireborgSuperheat = GetTargetAbilityOnAbilityData(abilityData) as FireborgSuperheat;
		bool flag = fireborgSuperheat != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (fireborgSuperheat != null)
		{
			text += GetTargetSelectModDesc(m_targetSelectMod, fireborgSuperheat.m_targetSelectComp, "-- Target Select --");
			string str = text;
			AbilityModPropertyInt superheatDurationMod = m_superheatDurationMod;
			int baseVal;
			if (flag)
			{
				baseVal = fireborgSuperheat.m_superheatDuration;
			}
			else
			{
				baseVal = 0;
			}

			text = new StringBuilder().Append(str).Append(PropDesc(superheatDurationMod, "[SuperheatDuration]", flag, baseVal)).ToString();
			string str2 = text;
			AbilityModPropertyInt igniteExtraDamageIfSuperheatedMod = m_igniteExtraDamageIfSuperheatedMod;
			int baseVal2;
			if (flag)
			{
				baseVal2 = fireborgSuperheat.m_igniteExtraDamageIfSuperheated;
			}
			else
			{
				baseVal2 = 0;
			}

			text = new StringBuilder().Append(str2).Append(PropDesc(igniteExtraDamageIfSuperheatedMod, "[IgniteExtraDamageIfSuperheated]", flag, baseVal2)).ToString();
		}
		return text;
	}
}
