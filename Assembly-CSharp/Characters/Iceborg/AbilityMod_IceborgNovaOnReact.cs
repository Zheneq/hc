using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityMod_IceborgNovaOnReact : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod")]
	public TargetSelectMod_Shape m_targetSelectMod;
	[Separator("On Hit Data for React Hits", "yellow")]
	public OnHitDataMod m_reactOnHitDataMod;
	[Space(10f)]
	public AbilityModPropertyInt m_reactDurationMod;
	public AbilityModPropertyBool m_reactRequireDamageMod;
	public AbilityModPropertyBool m_reactEffectEndEarlyIfTriggeredMod;
	[Separator("Energy on bearer per reaction")]
	public AbilityModPropertyInt m_energyOnTargetPerReactionMod;
	public AbilityModPropertyInt m_energyOnCasterPerReactionMod;
	[Separator("Passive Bonus Energy Gain for Nova Core Triggering")]
	public AbilityModPropertyInt m_extraEnergyPerNovaCoreTriggerMod;
	[Separator("Damage Threshold to apply instance to self on turn start. Ignored if <= 0")]
	public AbilityModPropertyInt m_damageThreshForInstanceOnSelfMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(IceborgNovaOnReact);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgNovaOnReact iceborgNovaOnReact = targetAbility as IceborgNovaOnReact;
		if (iceborgNovaOnReact != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddOnHitDataTokens(tokens, m_reactOnHitDataMod, iceborgNovaOnReact.m_reactOnHitData);
			AddToken(tokens, m_reactDurationMod, "ReactDuration", string.Empty, iceborgNovaOnReact.m_reactDuration);
			AddToken(tokens, m_energyOnTargetPerReactionMod, "EnergyOnTargetPerReaction", string.Empty, iceborgNovaOnReact.m_energyOnTargetPerReaction);
			AddToken(tokens, m_energyOnCasterPerReactionMod, "EnergyOnCasterPerReaction", string.Empty, iceborgNovaOnReact.m_energyOnCasterPerReaction);
			AddToken(tokens, m_extraEnergyPerNovaCoreTriggerMod, "ExtraEnergyPerNovaCoreTrigger", string.Empty, iceborgNovaOnReact.m_extraEnergyPerNovaCoreTrigger);
			AddToken(tokens, m_damageThreshForInstanceOnSelfMod, "DamageThreshForInstanceOnSelf", string.Empty, iceborgNovaOnReact.m_damageThreshForInstanceOnSelf);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgNovaOnReact iceborgNovaOnReact = GetTargetAbilityOnAbilityData(abilityData) as IceborgNovaOnReact;
		bool isValid = iceborgNovaOnReact != null;
		string desc = base.ModSpecificAutogenDesc(abilityData);
		if (isValid)
		{
			desc += GetTargetSelectModDesc(m_targetSelectMod, iceborgNovaOnReact.m_targetSelectComp, "-- Target Select --");
			desc += GetOnHitDataDesc(m_reactOnHitDataMod, iceborgNovaOnReact.m_reactOnHitData);
			desc += PropDesc(m_reactDurationMod, "[ReactDuration]", isValid, isValid ? iceborgNovaOnReact.m_reactDuration : 0);
			desc += PropDesc(m_reactRequireDamageMod, "[ReactRequireDamage]", isValid, isValid && iceborgNovaOnReact.m_reactRequireDamage);
			desc += PropDesc(m_reactEffectEndEarlyIfTriggeredMod, "[ReactEffectEndEarlyIfTriggered]", isValid, isValid && iceborgNovaOnReact.m_reactEffectEndEarlyIfTriggered);
			desc += PropDesc(m_energyOnTargetPerReactionMod, "[EnergyOnTargetPerReaction]", isValid, isValid ? iceborgNovaOnReact.m_energyOnTargetPerReaction : 0);
			desc += PropDesc(m_energyOnCasterPerReactionMod, "[EnergyOnCasterPerReaction]", isValid, isValid ? iceborgNovaOnReact.m_energyOnCasterPerReaction : 0);
			desc += PropDesc(m_extraEnergyPerNovaCoreTriggerMod, "[ExtraEnergyPerNovaCoreTrigger]", isValid, isValid ? iceborgNovaOnReact.m_extraEnergyPerNovaCoreTrigger : 0);
			desc += PropDesc(m_damageThreshForInstanceOnSelfMod, "[DamageThreshForInstanceOnSelf]", isValid, isValid ? iceborgNovaOnReact.m_damageThreshForInstanceOnSelf : 0);
		}
		return desc;
	}
}
