using System;
using System.Collections.Generic;

public class AbilityMod_IceborgIcicle : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_Laser m_targetSelectMod;

	[Separator("Energy on caster if target has nova core on start of turn", true)]
	public AbilityModPropertyInt m_energyOnCasterIfTargetHasNovaCoreMod;

	[Separator("Cdr if has hit", true)]
	public AbilityModPropertyInt m_cdrIfHasHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(IceborgIcicle);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgIcicle iceborgIcicle = targetAbility as IceborgIcicle;
		if (iceborgIcicle != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_IceborgIcicle.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, this.m_energyOnCasterIfTargetHasNovaCoreMod, "EnergyOnCasterIfTargetHasNovaCore", string.Empty, iceborgIcicle.m_energyOnCasterIfTargetHasNovaCore, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrIfHasHitMod, "CdrIfHasHit", string.Empty, iceborgIcicle.m_cdrIfHasHit, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgIcicle iceborgIcicle = base.GetTargetAbilityOnAbilityData(abilityData) as IceborgIcicle;
		bool flag = iceborgIcicle != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (iceborgIcicle != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_IceborgIcicle.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			text += base.GetTargetSelectModDesc(this.m_targetSelectMod, iceborgIcicle.m_targetSelectComp, "-- Target Select --");
			string str = text;
			AbilityModPropertyInt energyOnCasterIfTargetHasNovaCoreMod = this.m_energyOnCasterIfTargetHasNovaCoreMod;
			string prefix = "[EnergyOnCasterIfTargetHasNovaCore]";
			bool showBaseVal = flag;
			int baseVal;
			if (flag)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				baseVal = iceborgIcicle.m_energyOnCasterIfTargetHasNovaCore;
			}
			else
			{
				baseVal = 0;
			}
			text = str + base.PropDesc(energyOnCasterIfTargetHasNovaCoreMod, prefix, showBaseVal, baseVal);
			string str2 = text;
			AbilityModPropertyInt cdrIfHasHitMod = this.m_cdrIfHasHitMod;
			string prefix2 = "[CdrIfHasHit]";
			bool showBaseVal2 = flag;
			int baseVal2;
			if (flag)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				baseVal2 = iceborgIcicle.m_cdrIfHasHit;
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + base.PropDesc(cdrIfHasHitMod, prefix2, showBaseVal2, baseVal2);
		}
		return text;
	}
}
