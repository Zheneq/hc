using System;
using System.Collections.Generic;

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
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FireborgSuperheat fireborgSuperheat = targetAbility as FireborgSuperheat;
		if (fireborgSuperheat != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, this.m_superheatDurationMod, "SuperheatDuration", string.Empty, fireborgSuperheat.m_superheatDuration, true, false);
			AbilityMod.AddToken(tokens, this.m_igniteExtraDamageIfSuperheatedMod, "IgniteExtraDamageIfSuperheated", string.Empty, fireborgSuperheat.m_igniteExtraDamageIfSuperheated, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FireborgSuperheat fireborgSuperheat = base.GetTargetAbilityOnAbilityData(abilityData) as FireborgSuperheat;
		bool flag = fireborgSuperheat != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (fireborgSuperheat != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_FireborgSuperheat.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			text += base.GetTargetSelectModDesc(this.m_targetSelectMod, fireborgSuperheat.m_targetSelectComp, "-- Target Select --");
			string str = text;
			AbilityModPropertyInt superheatDurationMod = this.m_superheatDurationMod;
			string prefix = "[SuperheatDuration]";
			bool showBaseVal = flag;
			int baseVal;
			if (flag)
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
				baseVal = fireborgSuperheat.m_superheatDuration;
			}
			else
			{
				baseVal = 0;
			}
			text = str + base.PropDesc(superheatDurationMod, prefix, showBaseVal, baseVal);
			string str2 = text;
			AbilityModPropertyInt igniteExtraDamageIfSuperheatedMod = this.m_igniteExtraDamageIfSuperheatedMod;
			string prefix2 = "[IgniteExtraDamageIfSuperheated]";
			bool showBaseVal2 = flag;
			int baseVal2;
			if (flag)
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
				baseVal2 = fireborgSuperheat.m_igniteExtraDamageIfSuperheated;
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + base.PropDesc(igniteExtraDamageIfSuperheatedMod, prefix2, showBaseVal2, baseVal2);
		}
		return text;
	}
}
