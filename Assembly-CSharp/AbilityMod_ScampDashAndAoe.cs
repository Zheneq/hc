using AbilityContextNamespace;
using System;
using System.Collections.Generic;
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
		if (!(scampDashAndAoe != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddOnHitDataTokens(tokens, m_shieldDownOnHitDataMod, scampDashAndAoe.m_shieldDownOnHitData);
			AbilityMod.AddToken(tokens, m_shieldCostMod, "ShieldCost", string.Empty, scampDashAndAoe.m_shieldCost);
			AbilityMod.AddToken(tokens, m_shieldDownCooldownMod, "ShieldDownCooldown", string.Empty, scampDashAndAoe.m_shieldDownCooldown);
			AbilityMod.AddToken(tokens, m_cdrOnSuitApplyMod, "CdrOnSuitApply", string.Empty, scampDashAndAoe.m_cdrOnSuitApply);
			AbilityMod.AddToken(tokens, m_shieldDownNoCooldownHealthThreshMod, "ShieldDownNoCooldownHealthThresh", string.Empty, scampDashAndAoe.m_shieldDownNoCooldownHealthThresh);
			AbilityMod.AddToken(tokens, m_extraEnergyForDashOnOrbMod, "ExtraEnergyForDashOnOrb", string.Empty, scampDashAndAoe.m_extraEnergyForDashOnOrb);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampDashAndAoe scampDashAndAoe = GetTargetAbilityOnAbilityData(abilityData) as ScampDashAndAoe;
		bool flag = scampDashAndAoe != null;
		string str = base.ModSpecificAutogenDesc(abilityData);
		if (scampDashAndAoe != null)
		{
			while (true)
			{
				switch (5)
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
			str += GetOnHitDataDesc(m_shieldDownOnHitDataMod, scampDashAndAoe.m_shieldDownOnHitData, "-- Shield Down On Hit Data Mod --");
			str += GetTargetSelectModDesc(m_inSuitTargetSelectMod, scampDashAndAoe.m_targetSelectComp, "-- In-Suit Target Select Mod --");
			str += GetTargetSelectModDesc(m_shieldDownTargetSelectMod, scampDashAndAoe.m_shieldDownTargetSelect, "-- Shield Down Target Select Mod --");
		}
		str += PropDesc(m_shieldCostMod, "[ShieldCost]", flag, flag ? scampDashAndAoe.m_shieldCost : 0);
		string str2 = str;
		AbilityModPropertyInt shieldDownCooldownMod = m_shieldDownCooldownMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal = scampDashAndAoe.m_shieldDownCooldown;
		}
		else
		{
			baseVal = 0;
		}
		str = str2 + PropDesc(shieldDownCooldownMod, "[ShieldDownCooldown]", flag, baseVal);
		string str3 = str;
		AbilityModPropertyInt cdrOnSuitApplyMod = m_cdrOnSuitApplyMod;
		int baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = scampDashAndAoe.m_cdrOnSuitApply;
		}
		else
		{
			baseVal2 = 0;
		}
		str = str3 + PropDesc(cdrOnSuitApplyMod, "[CdrOnSuitApply]", flag, baseVal2);
		string str4 = str;
		AbilityModPropertyInt shieldDownNoCooldownHealthThreshMod = m_shieldDownNoCooldownHealthThreshMod;
		int baseVal3;
		if (flag)
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
			baseVal3 = scampDashAndAoe.m_shieldDownNoCooldownHealthThresh;
		}
		else
		{
			baseVal3 = 0;
		}
		str = str4 + PropDesc(shieldDownNoCooldownHealthThreshMod, "[ShieldDownNoCooldownHealthThresh]", flag, baseVal3);
		return str + PropDesc(m_extraEnergyForDashOnOrbMod, "[ExtraEnergyForDashOnOrb]", flag, flag ? scampDashAndAoe.m_extraEnergyForDashOnOrb : 0);
	}
}
