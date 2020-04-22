using AbilityContextNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_IceborgNovaOnReact : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_Shape m_targetSelectMod;

	[Separator("On Hit Data for React Hits", "yellow")]
	public OnHitDataMod m_reactOnHitDataMod;

	[Space(10f)]
	public AbilityModPropertyInt m_reactDurationMod;

	public AbilityModPropertyBool m_reactRequireDamageMod;

	public AbilityModPropertyBool m_reactEffectEndEarlyIfTriggeredMod;

	[Separator("Energy on bearer per reaction", true)]
	public AbilityModPropertyInt m_energyOnTargetPerReactionMod;

	public AbilityModPropertyInt m_energyOnCasterPerReactionMod;

	[Separator("Passive Bonus Energy Gain for Nova Core Triggering", true)]
	public AbilityModPropertyInt m_extraEnergyPerNovaCoreTriggerMod;

	[Separator("Damage Threshold to apply instance to self on turn start. Ignored if <= 0", true)]
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
		if (!(iceborgNovaOnReact != null))
		{
			return;
		}
		while (true)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddOnHitDataTokens(tokens, m_reactOnHitDataMod, iceborgNovaOnReact.m_reactOnHitData);
			AbilityMod.AddToken(tokens, m_reactDurationMod, "ReactDuration", string.Empty, iceborgNovaOnReact.m_reactDuration);
			AbilityMod.AddToken(tokens, m_energyOnTargetPerReactionMod, "EnergyOnTargetPerReaction", string.Empty, iceborgNovaOnReact.m_energyOnTargetPerReaction);
			AbilityMod.AddToken(tokens, m_energyOnCasterPerReactionMod, "EnergyOnCasterPerReaction", string.Empty, iceborgNovaOnReact.m_energyOnCasterPerReaction);
			AbilityMod.AddToken(tokens, m_extraEnergyPerNovaCoreTriggerMod, "ExtraEnergyPerNovaCoreTrigger", string.Empty, iceborgNovaOnReact.m_extraEnergyPerNovaCoreTrigger);
			AbilityMod.AddToken(tokens, m_damageThreshForInstanceOnSelfMod, "DamageThreshForInstanceOnSelf", string.Empty, iceborgNovaOnReact.m_damageThreshForInstanceOnSelf);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgNovaOnReact iceborgNovaOnReact = GetTargetAbilityOnAbilityData(abilityData) as IceborgNovaOnReact;
		bool flag = iceborgNovaOnReact != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (iceborgNovaOnReact != null)
		{
			text += GetTargetSelectModDesc(m_targetSelectMod, iceborgNovaOnReact.m_targetSelectComp, "-- Target Select --");
			text += GetOnHitDataDesc(m_reactOnHitDataMod, iceborgNovaOnReact.m_reactOnHitData);
			text += PropDesc(m_reactDurationMod, "[ReactDuration]", flag, flag ? iceborgNovaOnReact.m_reactDuration : 0);
			string str = text;
			AbilityModPropertyBool reactRequireDamageMod = m_reactRequireDamageMod;
			int baseVal;
			if (flag)
			{
				baseVal = (iceborgNovaOnReact.m_reactRequireDamage ? 1 : 0);
			}
			else
			{
				baseVal = 0;
			}
			text = str + PropDesc(reactRequireDamageMod, "[ReactRequireDamage]", flag, (byte)baseVal != 0);
			string str2 = text;
			AbilityModPropertyBool reactEffectEndEarlyIfTriggeredMod = m_reactEffectEndEarlyIfTriggeredMod;
			int baseVal2;
			if (flag)
			{
				baseVal2 = (iceborgNovaOnReact.m_reactEffectEndEarlyIfTriggered ? 1 : 0);
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + PropDesc(reactEffectEndEarlyIfTriggeredMod, "[ReactEffectEndEarlyIfTriggered]", flag, (byte)baseVal2 != 0);
			text += PropDesc(m_energyOnTargetPerReactionMod, "[EnergyOnTargetPerReaction]", flag, flag ? iceborgNovaOnReact.m_energyOnTargetPerReaction : 0);
			text += PropDesc(m_energyOnCasterPerReactionMod, "[EnergyOnCasterPerReaction]", flag, flag ? iceborgNovaOnReact.m_energyOnCasterPerReaction : 0);
			string str3 = text;
			AbilityModPropertyInt extraEnergyPerNovaCoreTriggerMod = m_extraEnergyPerNovaCoreTriggerMod;
			int baseVal3;
			if (flag)
			{
				baseVal3 = iceborgNovaOnReact.m_extraEnergyPerNovaCoreTrigger;
			}
			else
			{
				baseVal3 = 0;
			}
			text = str3 + PropDesc(extraEnergyPerNovaCoreTriggerMod, "[ExtraEnergyPerNovaCoreTrigger]", flag, baseVal3);
			string str4 = text;
			AbilityModPropertyInt damageThreshForInstanceOnSelfMod = m_damageThreshForInstanceOnSelfMod;
			int baseVal4;
			if (flag)
			{
				baseVal4 = iceborgNovaOnReact.m_damageThreshForInstanceOnSelf;
			}
			else
			{
				baseVal4 = 0;
			}
			text = str4 + PropDesc(damageThreshForInstanceOnSelfMod, "[DamageThreshForInstanceOnSelf]", flag, baseVal4);
		}
		return text;
	}
}
