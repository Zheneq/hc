using System;
using System.Collections.Generic;
using System.Text;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityMod_ScampDashAndAoe : GenericAbility_AbilityMod
{
	[Separator("Shield Down on hit data mod", "yellow")]
	public OnHitDataMod m_shieldDownOnHitDataMod;
	[Separator("Target Select Mods")]
	public TargetSelectMod_ChargeSingleStep m_inSuitTargetSelectMod;
	public TargetSelectMod_ChargeSingleStep m_shieldDownTargetSelectMod;
	[Separator("Shield Cost on Cast")]
	public AbilityModPropertyInt m_shieldCostMod;
	[Separator("Cooldown for Shield Down mode. If <= 0, use same cooldown for both modes")]
	public AbilityModPropertyInt m_shieldDownCooldownMod;
	[Header("-- Cdr on suit dash when going into suit form")]
	public AbilityModPropertyInt m_cdrOnSuitApplyMod;
	[Header("-- if > 0 and health below threshold, shield down form of dash has no cooldowns")]
	public AbilityModPropertyInt m_shieldDownNoCooldownHealthThreshMod;
	[Separator("Extra Energy for dashing through or onto orb")]
	public AbilityModPropertyInt m_extraEnergyForDashOnOrbMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScampDashAndAoe);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScampDashAndAoe scampDashAndAoe = targetAbility as ScampDashAndAoe;
		if (scampDashAndAoe != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddOnHitDataTokens(tokens, m_shieldDownOnHitDataMod, scampDashAndAoe.m_shieldDownOnHitData);
			AddToken(tokens, m_shieldCostMod, "ShieldCost", string.Empty, scampDashAndAoe.m_shieldCost);
			AddToken(tokens, m_shieldDownCooldownMod, "ShieldDownCooldown", string.Empty, scampDashAndAoe.m_shieldDownCooldown);
			AddToken(tokens, m_cdrOnSuitApplyMod, "CdrOnSuitApply", string.Empty, scampDashAndAoe.m_cdrOnSuitApply);
			AddToken(tokens, m_shieldDownNoCooldownHealthThreshMod, "ShieldDownNoCooldownHealthThresh", string.Empty, scampDashAndAoe.m_shieldDownNoCooldownHealthThresh);
			AddToken(tokens, m_extraEnergyForDashOnOrbMod, "ExtraEnergyForDashOnOrb", string.Empty, scampDashAndAoe.m_extraEnergyForDashOnOrb);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampDashAndAoe scampDashAndAoe = GetTargetAbilityOnAbilityData(abilityData) as ScampDashAndAoe;
		bool isValid = scampDashAndAoe != null;
		string desc = base.ModSpecificAutogenDesc(abilityData);
		if (scampDashAndAoe != null)
		{
			desc += GetOnHitDataDesc(m_shieldDownOnHitDataMod, scampDashAndAoe.m_shieldDownOnHitData, "-- Shield Down On Hit Data Mod --");
			desc += GetTargetSelectModDesc(m_inSuitTargetSelectMod, scampDashAndAoe.m_targetSelectComp, "-- In-Suit Target Select Mod --");
			desc += GetTargetSelectModDesc(m_shieldDownTargetSelectMod, scampDashAndAoe.m_shieldDownTargetSelect, "-- Shield Down Target Select Mod --");
		}
		desc += PropDesc(m_shieldCostMod, "[ShieldCost]", isValid, isValid ? scampDashAndAoe.m_shieldCost : 0);
		desc += PropDesc(m_shieldDownCooldownMod, "[ShieldDownCooldown]", isValid, isValid ? scampDashAndAoe.m_shieldDownCooldown : 0);
		desc += PropDesc(m_cdrOnSuitApplyMod, "[CdrOnSuitApply]", isValid, isValid ? scampDashAndAoe.m_cdrOnSuitApply : 0);
		desc += PropDesc(m_shieldDownNoCooldownHealthThreshMod, "[ShieldDownNoCooldownHealthThresh]", isValid, isValid ? scampDashAndAoe.m_shieldDownNoCooldownHealthThresh : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_extraEnergyForDashOnOrbMod, "[ExtraEnergyForDashOnOrb]", isValid, isValid ? scampDashAndAoe.m_extraEnergyForDashOnOrb : 0)).ToString();
	}
}
