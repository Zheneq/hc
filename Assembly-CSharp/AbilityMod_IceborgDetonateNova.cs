using AbilityContextNamespace;
using System;
using System.Collections.Generic;

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
		targetSelect.SetTargetSelectMod(m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		IceborgDetonateNova iceborgDetonateNova = targetAbility as IceborgDetonateNova;
		if (!(iceborgDetonateNova != null))
		{
			return;
		}
		while (true)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddOnHitDataTokens(tokens, m_empoweredDelayedAoeOnHitDataMod, iceborgDetonateNova.m_empoweredDelayedAoeOnHitData);
			AbilityMod.AddToken(tokens, m_novaOnReactShieldPerDetonateMod, "NovaOnReactShieldPerDetonate", string.Empty, iceborgDetonateNova.m_novaOnReactShieldPerDetonate);
			AbilityMod.AddToken(tokens, m_shieldOnDetonateDurationMod, "ShieldOnDetonateDuration", string.Empty, iceborgDetonateNova.m_shieldOnDetonateDuration);
			AbilityMod.AddToken(tokens, m_cdrPerKillMod, "CdrPerKill", string.Empty, iceborgDetonateNova.m_cdrPerKill);
			AbilityMod.AddToken(tokens, m_cdrIfAnyKillMod, "CdrIfAnyKill", string.Empty, iceborgDetonateNova.m_cdrIfAnyKill);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		IceborgDetonateNova iceborgDetonateNova = GetTargetAbilityOnAbilityData(abilityData) as IceborgDetonateNova;
		bool flag = iceborgDetonateNova != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (iceborgDetonateNova != null)
		{
			text += GetTargetSelectModDesc(m_targetSelectMod, iceborgDetonateNova.m_targetSelectComp, "-- Target Select --");
			text += GetOnHitDataDesc(m_empoweredDelayedAoeOnHitDataMod, iceborgDetonateNova.m_empoweredDelayedAoeOnHitData);
			string str = text;
			AbilityModPropertyInt novaOnReactShieldPerDetonateMod = m_novaOnReactShieldPerDetonateMod;
			int baseVal;
			if (flag)
			{
				baseVal = iceborgDetonateNova.m_novaOnReactShieldPerDetonate;
			}
			else
			{
				baseVal = 0;
			}
			text = str + PropDesc(novaOnReactShieldPerDetonateMod, "[NovaOnReactShieldPerDetonate]", flag, baseVal);
			string str2 = text;
			AbilityModPropertyInt shieldOnDetonateDurationMod = m_shieldOnDetonateDurationMod;
			int baseVal2;
			if (flag)
			{
				baseVal2 = iceborgDetonateNova.m_shieldOnDetonateDuration;
			}
			else
			{
				baseVal2 = 0;
			}
			text = str2 + PropDesc(shieldOnDetonateDurationMod, "[ShieldOnDetonateDuration]", flag, baseVal2);
			string str3 = text;
			AbilityModPropertyInt cdrPerKillMod = m_cdrPerKillMod;
			int baseVal3;
			if (flag)
			{
				baseVal3 = iceborgDetonateNova.m_cdrPerKill;
			}
			else
			{
				baseVal3 = 0;
			}
			text = str3 + PropDesc(cdrPerKillMod, "[CdrPerKill]", flag, baseVal3);
			string str4 = text;
			AbilityModPropertyInt cdrIfAnyKillMod = m_cdrIfAnyKillMod;
			int baseVal4;
			if (flag)
			{
				baseVal4 = iceborgDetonateNova.m_cdrIfAnyKill;
			}
			else
			{
				baseVal4 = 0;
			}
			text = str4 + PropDesc(cdrIfAnyKillMod, "[CdrIfAnyKill]", flag, baseVal4);
		}
		return text;
	}
}
