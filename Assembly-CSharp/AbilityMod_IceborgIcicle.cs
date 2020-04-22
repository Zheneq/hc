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
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgIcicle iceborgIcicle = targetAbility as IceborgIcicle;
		if (!(iceborgIcicle != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, m_energyOnCasterIfTargetHasNovaCoreMod, "EnergyOnCasterIfTargetHasNovaCore", string.Empty, iceborgIcicle.m_energyOnCasterIfTargetHasNovaCore);
			AbilityMod.AddToken(tokens, m_cdrIfHasHitMod, "CdrIfHasHit", string.Empty, iceborgIcicle.m_cdrIfHasHit);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgIcicle iceborgIcicle = GetTargetAbilityOnAbilityData(abilityData) as IceborgIcicle;
		bool flag = iceborgIcicle != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (iceborgIcicle != null)
		{
			while (true)
			{
				switch (3)
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
			text += GetTargetSelectModDesc(m_targetSelectMod, iceborgIcicle.m_targetSelectComp, "-- Target Select --");
			string str = text;
			AbilityModPropertyInt energyOnCasterIfTargetHasNovaCoreMod = m_energyOnCasterIfTargetHasNovaCoreMod;
			int baseVal;
			if (flag)
			{
				while (true)
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
			text = str + PropDesc(energyOnCasterIfTargetHasNovaCoreMod, "[EnergyOnCasterIfTargetHasNovaCore]", flag, baseVal);
			string str2 = text;
			AbilityModPropertyInt cdrIfHasHitMod = m_cdrIfHasHitMod;
			int baseVal2;
			if (flag)
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
				baseVal2 = iceborgIcicle.m_cdrIfHasHit;
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + PropDesc(cdrIfHasHitMod, "[CdrIfHasHit]", flag, baseVal2);
		}
		return text;
	}
}
