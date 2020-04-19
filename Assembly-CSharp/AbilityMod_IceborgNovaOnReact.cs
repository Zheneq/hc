using System;
using System.Collections.Generic;
using AbilityContextNamespace;
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
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgNovaOnReact iceborgNovaOnReact = targetAbility as IceborgNovaOnReact;
		if (iceborgNovaOnReact != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_IceborgNovaOnReact.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			base.AddOnHitDataTokens(tokens, this.m_reactOnHitDataMod, iceborgNovaOnReact.m_reactOnHitData);
			AbilityMod.AddToken(tokens, this.m_reactDurationMod, "ReactDuration", string.Empty, iceborgNovaOnReact.m_reactDuration, true, false);
			AbilityMod.AddToken(tokens, this.m_energyOnTargetPerReactionMod, "EnergyOnTargetPerReaction", string.Empty, iceborgNovaOnReact.m_energyOnTargetPerReaction, true, false);
			AbilityMod.AddToken(tokens, this.m_energyOnCasterPerReactionMod, "EnergyOnCasterPerReaction", string.Empty, iceborgNovaOnReact.m_energyOnCasterPerReaction, true, false);
			AbilityMod.AddToken(tokens, this.m_extraEnergyPerNovaCoreTriggerMod, "ExtraEnergyPerNovaCoreTrigger", string.Empty, iceborgNovaOnReact.m_extraEnergyPerNovaCoreTrigger, true, false);
			AbilityMod.AddToken(tokens, this.m_damageThreshForInstanceOnSelfMod, "DamageThreshForInstanceOnSelf", string.Empty, iceborgNovaOnReact.m_damageThreshForInstanceOnSelf, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgNovaOnReact iceborgNovaOnReact = base.GetTargetAbilityOnAbilityData(abilityData) as IceborgNovaOnReact;
		bool flag = iceborgNovaOnReact != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (iceborgNovaOnReact != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_IceborgNovaOnReact.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			text += base.GetTargetSelectModDesc(this.m_targetSelectMod, iceborgNovaOnReact.m_targetSelectComp, "-- Target Select --");
			text += base.GetOnHitDataDesc(this.m_reactOnHitDataMod, iceborgNovaOnReact.m_reactOnHitData, "-- On Hit Data Mod --");
			text += base.PropDesc(this.m_reactDurationMod, "[ReactDuration]", flag, (!flag) ? 0 : iceborgNovaOnReact.m_reactDuration);
			string str = text;
			AbilityModPropertyBool reactRequireDamageMod = this.m_reactRequireDamageMod;
			string prefix = "[ReactRequireDamage]";
			bool showBaseVal = flag;
			bool baseVal;
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
				baseVal = iceborgNovaOnReact.m_reactRequireDamage;
			}
			else
			{
				baseVal = false;
			}
			text = str + base.PropDesc(reactRequireDamageMod, prefix, showBaseVal, baseVal);
			string str2 = text;
			AbilityModPropertyBool reactEffectEndEarlyIfTriggeredMod = this.m_reactEffectEndEarlyIfTriggeredMod;
			string prefix2 = "[ReactEffectEndEarlyIfTriggered]";
			bool showBaseVal2 = flag;
			bool baseVal2;
			if (flag)
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
				baseVal2 = iceborgNovaOnReact.m_reactEffectEndEarlyIfTriggered;
			}
			else
			{
				baseVal2 = false;
			}
			text = str2 + base.PropDesc(reactEffectEndEarlyIfTriggeredMod, prefix2, showBaseVal2, baseVal2);
			text += base.PropDesc(this.m_energyOnTargetPerReactionMod, "[EnergyOnTargetPerReaction]", flag, (!flag) ? 0 : iceborgNovaOnReact.m_energyOnTargetPerReaction);
			text += base.PropDesc(this.m_energyOnCasterPerReactionMod, "[EnergyOnCasterPerReaction]", flag, (!flag) ? 0 : iceborgNovaOnReact.m_energyOnCasterPerReaction);
			string str3 = text;
			AbilityModPropertyInt extraEnergyPerNovaCoreTriggerMod = this.m_extraEnergyPerNovaCoreTriggerMod;
			string prefix3 = "[ExtraEnergyPerNovaCoreTrigger]";
			bool showBaseVal3 = flag;
			int baseVal3;
			if (flag)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				baseVal3 = iceborgNovaOnReact.m_extraEnergyPerNovaCoreTrigger;
			}
			else
			{
				baseVal3 = 0;
			}
			text = str3 + base.PropDesc(extraEnergyPerNovaCoreTriggerMod, prefix3, showBaseVal3, baseVal3);
			string str4 = text;
			AbilityModPropertyInt damageThreshForInstanceOnSelfMod = this.m_damageThreshForInstanceOnSelfMod;
			string prefix4 = "[DamageThreshForInstanceOnSelf]";
			bool showBaseVal4 = flag;
			int baseVal4;
			if (flag)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				baseVal4 = iceborgNovaOnReact.m_damageThreshForInstanceOnSelf;
			}
			else
			{
				baseVal4 = 0;
			}
			text = str4 + base.PropDesc(damageThreshForInstanceOnSelfMod, prefix4, showBaseVal4, baseVal4);
		}
		return text;
	}
}
