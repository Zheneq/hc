using System;
using System.Collections.Generic;
using AbilityContextNamespace;

public class AbilityMod_IceborgDetonateNova : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_Shape m_targetSelectMod;

	[Separator("Empowered Nova Core On Hit Data", "yellow")]
	public OnHitDataMod m_empoweredDelayedAoeOnHitDataMod;

	[Separator("Shield Per Detonate on NovaOnReact ability (Arctic Armor)", true)]
	public AbilityModPropertyInt m_novaOnReactShieldPerDetonateMod;

	public AbilityModPropertyInt m_shieldOnDetonateDurationMod;

	[Separator("Cdr Per Target Killed", true)]
	public AbilityModPropertyInt m_cdrPerKillMod;

	public AbilityModPropertyInt m_cdrIfAnyKillMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(IceborgDetonateNova);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgDetonateNova iceborgDetonateNova = targetAbility as IceborgDetonateNova;
		if (iceborgDetonateNova != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			base.AddOnHitDataTokens(tokens, this.m_empoweredDelayedAoeOnHitDataMod, iceborgDetonateNova.m_empoweredDelayedAoeOnHitData);
			AbilityMod.AddToken(tokens, this.m_novaOnReactShieldPerDetonateMod, "NovaOnReactShieldPerDetonate", string.Empty, iceborgDetonateNova.m_novaOnReactShieldPerDetonate, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldOnDetonateDurationMod, "ShieldOnDetonateDuration", string.Empty, iceborgDetonateNova.m_shieldOnDetonateDuration, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrPerKillMod, "CdrPerKill", string.Empty, iceborgDetonateNova.m_cdrPerKill, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrIfAnyKillMod, "CdrIfAnyKill", string.Empty, iceborgDetonateNova.m_cdrIfAnyKill, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgDetonateNova iceborgDetonateNova = base.GetTargetAbilityOnAbilityData(abilityData) as IceborgDetonateNova;
		bool flag = iceborgDetonateNova != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (iceborgDetonateNova != null)
		{
			text += base.GetTargetSelectModDesc(this.m_targetSelectMod, iceborgDetonateNova.m_targetSelectComp, "-- Target Select --");
			text += base.GetOnHitDataDesc(this.m_empoweredDelayedAoeOnHitDataMod, iceborgDetonateNova.m_empoweredDelayedAoeOnHitData, "-- On Hit Data Mod --");
			string str = text;
			AbilityModPropertyInt novaOnReactShieldPerDetonateMod = this.m_novaOnReactShieldPerDetonateMod;
			string prefix = "[NovaOnReactShieldPerDetonate]";
			bool showBaseVal = flag;
			int baseVal;
			if (flag)
			{
				baseVal = iceborgDetonateNova.m_novaOnReactShieldPerDetonate;
			}
			else
			{
				baseVal = 0;
			}
			text = str + base.PropDesc(novaOnReactShieldPerDetonateMod, prefix, showBaseVal, baseVal);
			string str2 = text;
			AbilityModPropertyInt shieldOnDetonateDurationMod = this.m_shieldOnDetonateDurationMod;
			string prefix2 = "[ShieldOnDetonateDuration]";
			bool showBaseVal2 = flag;
			int baseVal2;
			if (flag)
			{
				baseVal2 = iceborgDetonateNova.m_shieldOnDetonateDuration;
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + base.PropDesc(shieldOnDetonateDurationMod, prefix2, showBaseVal2, baseVal2);
			string str3 = text;
			AbilityModPropertyInt cdrPerKillMod = this.m_cdrPerKillMod;
			string prefix3 = "[CdrPerKill]";
			bool showBaseVal3 = flag;
			int baseVal3;
			if (flag)
			{
				baseVal3 = iceborgDetonateNova.m_cdrPerKill;
			}
			else
			{
				baseVal3 = 0;
			}
			text = str3 + base.PropDesc(cdrPerKillMod, prefix3, showBaseVal3, baseVal3);
			string str4 = text;
			AbilityModPropertyInt cdrIfAnyKillMod = this.m_cdrIfAnyKillMod;
			string prefix4 = "[CdrIfAnyKill]";
			bool showBaseVal4 = flag;
			int baseVal4;
			if (flag)
			{
				baseVal4 = iceborgDetonateNova.m_cdrIfAnyKill;
			}
			else
			{
				baseVal4 = 0;
			}
			text = str4 + base.PropDesc(cdrIfAnyKillMod, prefix4, showBaseVal4, baseVal4);
		}
		return text;
	}
}
