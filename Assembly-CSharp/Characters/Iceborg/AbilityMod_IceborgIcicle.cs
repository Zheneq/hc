using System;
using System.Collections.Generic;

public class AbilityMod_IceborgIcicle : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod")]
	public TargetSelectMod_Laser m_targetSelectMod;
	[Separator("Energy on caster if target has nova core on start of turn")]
	public AbilityModPropertyInt m_energyOnCasterIfTargetHasNovaCoreMod;
	[Separator("Cdr if has hit")]
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
		if (iceborgIcicle != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddToken(tokens, m_energyOnCasterIfTargetHasNovaCoreMod, "EnergyOnCasterIfTargetHasNovaCore", string.Empty, iceborgIcicle.m_energyOnCasterIfTargetHasNovaCore);
			AddToken(tokens, m_cdrIfHasHitMod, "CdrIfHasHit", string.Empty, iceborgIcicle.m_cdrIfHasHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgIcicle iceborgIcicle = GetTargetAbilityOnAbilityData(abilityData) as IceborgIcicle;
		bool isValid = iceborgIcicle != null;
		string desc = base.ModSpecificAutogenDesc(abilityData);
		if (isValid)
		{
			desc += GetTargetSelectModDesc(m_targetSelectMod, iceborgIcicle.m_targetSelectComp, "-- Target Select --");
			AbilityModPropertyInt energyOnCasterIfTargetHasNovaCoreMod = m_energyOnCasterIfTargetHasNovaCoreMod;
			desc += PropDesc(energyOnCasterIfTargetHasNovaCoreMod, "[EnergyOnCasterIfTargetHasNovaCore]", isValid, isValid ? iceborgIcicle.m_energyOnCasterIfTargetHasNovaCore : 0);
			desc += PropDesc(m_cdrIfHasHitMod, "[CdrIfHasHit]", isValid, isValid ? iceborgIcicle.m_cdrIfHasHit : 0);
		}
		return desc;
	}
}
