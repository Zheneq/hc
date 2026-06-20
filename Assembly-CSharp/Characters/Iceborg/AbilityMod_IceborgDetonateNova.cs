using System;
using System.Collections.Generic;
using AbilityContextNamespace;

public class AbilityMod_IceborgDetonateNova : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod")]
	public TargetSelectMod_Shape m_targetSelectMod;
	[Separator("Empowered Nova Core On Hit Data", "yellow")]
	public OnHitDataMod m_empoweredDelayedAoeOnHitDataMod;
	[Separator("Shield Per Detonate on NovaOnReact ability (Arctic Armor)")]
	public AbilityModPropertyInt m_novaOnReactShieldPerDetonateMod;
	public AbilityModPropertyInt m_shieldOnDetonateDurationMod;
	[Separator("Cdr Per Target Killed")]
	public AbilityModPropertyInt m_cdrPerKillMod;
	public AbilityModPropertyInt m_cdrIfAnyKillMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(IceborgDetonateNova);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgDetonateNova iceborgDetonateNova = targetAbility as IceborgDetonateNova;
		if (iceborgDetonateNova != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddOnHitDataTokens(tokens, m_empoweredDelayedAoeOnHitDataMod, iceborgDetonateNova.m_empoweredDelayedAoeOnHitData);
			AddToken(tokens, m_novaOnReactShieldPerDetonateMod, "NovaOnReactShieldPerDetonate", string.Empty, iceborgDetonateNova.m_novaOnReactShieldPerDetonate);
			AddToken(tokens, m_shieldOnDetonateDurationMod, "ShieldOnDetonateDuration", string.Empty, iceborgDetonateNova.m_shieldOnDetonateDuration);
			AddToken(tokens, m_cdrPerKillMod, "CdrPerKill", string.Empty, iceborgDetonateNova.m_cdrPerKill);
			AddToken(tokens, m_cdrIfAnyKillMod, "CdrIfAnyKill", string.Empty, iceborgDetonateNova.m_cdrIfAnyKill);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgDetonateNova iceborgDetonateNova = GetTargetAbilityOnAbilityData(abilityData) as IceborgDetonateNova;
		bool isValid = iceborgDetonateNova != null;
		string desc = base.ModSpecificAutogenDesc(abilityData);
		if (isValid)
		{
			desc += GetTargetSelectModDesc(m_targetSelectMod, iceborgDetonateNova.m_targetSelectComp, "-- Target Select --");
			desc += GetOnHitDataDesc(m_empoweredDelayedAoeOnHitDataMod, iceborgDetonateNova.m_empoweredDelayedAoeOnHitData);
			desc += PropDesc(m_novaOnReactShieldPerDetonateMod, "[NovaOnReactShieldPerDetonate]", isValid, isValid ? iceborgDetonateNova.m_novaOnReactShieldPerDetonate : 0);
			desc += PropDesc(m_shieldOnDetonateDurationMod, "[ShieldOnDetonateDuration]", isValid, isValid ? iceborgDetonateNova.m_shieldOnDetonateDuration : 0);
			desc += PropDesc(m_cdrPerKillMod, "[CdrPerKill]", isValid, isValid ? iceborgDetonateNova.m_cdrPerKill : 0);
			desc += PropDesc(m_cdrIfAnyKillMod, "[CdrIfAnyKill]", isValid, isValid ? iceborgDetonateNova.m_cdrIfAnyKill : 0);
		}
		return desc;
	}
}
