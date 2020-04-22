using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SoldierStimPack : AbilityMod
{
	[Header("-- On Hit Stuff --")]
	public AbilityModPropertyInt m_selfHealAmountMod;

	public AbilityModPropertyEffectInfo m_selfHitEffectMod;

	[Header("-- For other abilities when active --")]
	public AbilityModPropertyBool m_basicAttackIgnoreCoverMod;

	public AbilityModPropertyBool m_basicAttackReduceCoverEffectivenessMod;

	public AbilityModPropertyFloat m_grenadeExtraRangeMod;

	public AbilityModPropertyEffectInfo m_dashShootExtraEffectMod;

	[Header("-- Health threshold to trigger cooldown reset, value:(0-1)")]
	public AbilityModPropertyFloat m_cooldownResetHealthThresholdMod;

	[Header("-- CDR - if dash and shoot used on same turn")]
	public AbilityModPropertyInt m_cdrIfDashAndShootUsedMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SoldierStimPack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SoldierStimPack soldierStimPack = targetAbility as SoldierStimPack;
		if (!(soldierStimPack != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_selfHealAmountMod, "SelfHealAmount", string.Empty, soldierStimPack.m_selfHealAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_selfHitEffectMod, "SelfHitEffect", soldierStimPack.m_selfHitEffect);
			AbilityMod.AddToken(tokens, m_grenadeExtraRangeMod, "GrenadeExtraRange", string.Empty, soldierStimPack.m_grenadeExtraRange);
			AbilityMod.AddToken_EffectMod(tokens, m_dashShootExtraEffectMod, "DashShootExtraEffect", soldierStimPack.m_dashShootExtraEffect);
			AbilityMod.AddToken(tokens, m_cooldownResetHealthThresholdMod, "CooldownResetHealthThreshold", string.Empty, soldierStimPack.m_cooldownResetHealthThreshold, true, false, true);
			AbilityMod.AddToken(tokens, m_cdrIfDashAndShootUsedMod, "CdrIfDashAndShootUsed", string.Empty, soldierStimPack.m_cdrIfDashAndShootUsed);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierStimPack soldierStimPack = GetTargetAbilityOnAbilityData(abilityData) as SoldierStimPack;
		bool flag = soldierStimPack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt selfHealAmountMod = m_selfHealAmountMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (7)
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
			baseVal = soldierStimPack.m_selfHealAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(selfHealAmountMod, "[SelfHealAmount]", flag, baseVal);
		empty += PropDesc(m_selfHitEffectMod, "[SelfHitEffect]", flag, (!flag) ? null : soldierStimPack.m_selfHitEffect);
		string str2 = empty;
		AbilityModPropertyBool basicAttackIgnoreCoverMod = m_basicAttackIgnoreCoverMod;
		int baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = (soldierStimPack.m_basicAttackIgnoreCover ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(basicAttackIgnoreCoverMod, "[BasicAttackIgnoreCover]", flag, (byte)baseVal2 != 0);
		empty += PropDesc(m_basicAttackReduceCoverEffectivenessMod, "[BasicAttackReduceCoverEffectiveness]", flag, flag && soldierStimPack.m_basicAttackReduceCoverEffectiveness);
		string str3 = empty;
		AbilityModPropertyFloat grenadeExtraRangeMod = m_grenadeExtraRangeMod;
		float baseVal3;
		if (flag)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = soldierStimPack.m_grenadeExtraRange;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(grenadeExtraRangeMod, "[GrenadeExtraRange]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyEffectInfo dashShootExtraEffectMod = m_dashShootExtraEffectMod;
		object baseVal4;
		if (flag)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = soldierStimPack.m_dashShootExtraEffect;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + PropDesc(dashShootExtraEffectMod, "[DashShootExtraEffect]", flag, (StandardEffectInfo)baseVal4);
		empty += PropDesc(m_cooldownResetHealthThresholdMod, "[CooldownResetHealthThreshold]", flag, (!flag) ? 0f : soldierStimPack.m_cooldownResetHealthThreshold);
		return empty + PropDesc(m_cdrIfDashAndShootUsedMod, "[CdrIfDashAndShootUsed]", flag, flag ? soldierStimPack.m_cdrIfDashAndShootUsed : 0);
	}
}
