using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityMod_ScampDashAndAoe : GenericAbility_AbilityMod
{
	[Separator("Shield Down on hit data mod", "yellow")]
	public OnHitDataMod m_shieldDownOnHitDataMod;

	[Separator("Target Select Mods", true)]
	public TargetSelectMod_ChargeSingleStep m_inSuitTargetSelectMod;

	public TargetSelectMod_ChargeSingleStep m_shieldDownTargetSelectMod;

	[Separator("Shield Cost on Cast", true)]
	public AbilityModPropertyInt m_shieldCostMod;

	[Separator("Cooldown for Shield Down mode. If <= 0, use same cooldown for both modes", true)]
	public AbilityModPropertyInt m_shieldDownCooldownMod;

	[Header("-- Cdr on suit dash when going into suit form")]
	public AbilityModPropertyInt m_cdrOnSuitApplyMod;

	[Header("-- if > 0 and health below threshold, shield down form of dash has no cooldowns")]
	public AbilityModPropertyInt m_shieldDownNoCooldownHealthThreshMod;

	[Separator("Extra Energy for dashing through or onto orb", true)]
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
			base.AddOnHitDataTokens(tokens, this.m_shieldDownOnHitDataMod, scampDashAndAoe.m_shieldDownOnHitData);
			AbilityMod.AddToken(tokens, this.m_shieldCostMod, "ShieldCost", string.Empty, scampDashAndAoe.m_shieldCost, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldDownCooldownMod, "ShieldDownCooldown", string.Empty, scampDashAndAoe.m_shieldDownCooldown, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrOnSuitApplyMod, "CdrOnSuitApply", string.Empty, scampDashAndAoe.m_cdrOnSuitApply, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldDownNoCooldownHealthThreshMod, "ShieldDownNoCooldownHealthThresh", string.Empty, scampDashAndAoe.m_shieldDownNoCooldownHealthThresh, true, false);
			AbilityMod.AddToken(tokens, this.m_extraEnergyForDashOnOrbMod, "ExtraEnergyForDashOnOrb", string.Empty, scampDashAndAoe.m_extraEnergyForDashOnOrb, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampDashAndAoe scampDashAndAoe = base.GetTargetAbilityOnAbilityData(abilityData) as ScampDashAndAoe;
		bool flag = scampDashAndAoe != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (scampDashAndAoe != null)
		{
			text += base.GetOnHitDataDesc(this.m_shieldDownOnHitDataMod, scampDashAndAoe.m_shieldDownOnHitData, "-- Shield Down On Hit Data Mod --");
			text += base.GetTargetSelectModDesc(this.m_inSuitTargetSelectMod, scampDashAndAoe.m_targetSelectComp, "-- In-Suit Target Select Mod --");
			text += base.GetTargetSelectModDesc(this.m_shieldDownTargetSelectMod, scampDashAndAoe.m_shieldDownTargetSelect, "-- Shield Down Target Select Mod --");
		}
		text += base.PropDesc(this.m_shieldCostMod, "[ShieldCost]", flag, (!flag) ? 0 : scampDashAndAoe.m_shieldCost);
		string str = text;
		AbilityModPropertyInt shieldDownCooldownMod = this.m_shieldDownCooldownMod;
		string prefix = "[ShieldDownCooldown]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = scampDashAndAoe.m_shieldDownCooldown;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(shieldDownCooldownMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt cdrOnSuitApplyMod = this.m_cdrOnSuitApplyMod;
		string prefix2 = "[CdrOnSuitApply]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = scampDashAndAoe.m_cdrOnSuitApply;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(cdrOnSuitApplyMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt shieldDownNoCooldownHealthThreshMod = this.m_shieldDownNoCooldownHealthThreshMod;
		string prefix3 = "[ShieldDownNoCooldownHealthThresh]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = scampDashAndAoe.m_shieldDownNoCooldownHealthThresh;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(shieldDownNoCooldownHealthThreshMod, prefix3, showBaseVal3, baseVal3);
		return text + base.PropDesc(this.m_extraEnergyForDashOnOrbMod, "[ExtraEnergyForDashOnOrb]", flag, (!flag) ? 0 : scampDashAndAoe.m_extraEnergyForDashOnOrb);
	}
}
