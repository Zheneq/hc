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
		if (soldierStimPack != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SoldierStimPack.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_selfHealAmountMod, "SelfHealAmount", string.Empty, soldierStimPack.m_selfHealAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfHitEffectMod, "SelfHitEffect", soldierStimPack.m_selfHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_grenadeExtraRangeMod, "GrenadeExtraRange", string.Empty, soldierStimPack.m_grenadeExtraRange, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_dashShootExtraEffectMod, "DashShootExtraEffect", soldierStimPack.m_dashShootExtraEffect, true);
			AbilityMod.AddToken(tokens, this.m_cooldownResetHealthThresholdMod, "CooldownResetHealthThreshold", string.Empty, soldierStimPack.m_cooldownResetHealthThreshold, true, false, true);
			AbilityMod.AddToken(tokens, this.m_cdrIfDashAndShootUsedMod, "CdrIfDashAndShootUsed", string.Empty, soldierStimPack.m_cdrIfDashAndShootUsed, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierStimPack soldierStimPack = base.GetTargetAbilityOnAbilityData(abilityData) as SoldierStimPack;
		bool flag = soldierStimPack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt selfHealAmountMod = this.m_selfHealAmountMod;
		string prefix = "[SelfHealAmount]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SoldierStimPack.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = soldierStimPack.m_selfHealAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(selfHealAmountMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_selfHitEffectMod, "[SelfHitEffect]", flag, (!flag) ? null : soldierStimPack.m_selfHitEffect);
		string str2 = text;
		AbilityModPropertyBool basicAttackIgnoreCoverMod = this.m_basicAttackIgnoreCoverMod;
		string prefix2 = "[BasicAttackIgnoreCover]";
		bool showBaseVal2 = flag;
		bool baseVal2;
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
			baseVal2 = soldierStimPack.m_basicAttackIgnoreCover;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(basicAttackIgnoreCoverMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_basicAttackReduceCoverEffectivenessMod, "[BasicAttackReduceCoverEffectiveness]", flag, flag && soldierStimPack.m_basicAttackReduceCoverEffectiveness);
		string str3 = text;
		AbilityModPropertyFloat grenadeExtraRangeMod = this.m_grenadeExtraRangeMod;
		string prefix3 = "[GrenadeExtraRange]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			for (;;)
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
		text = str3 + base.PropDesc(grenadeExtraRangeMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyEffectInfo dashShootExtraEffectMod = this.m_dashShootExtraEffectMod;
		string prefix4 = "[DashShootExtraEffect]";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
		if (flag)
		{
			for (;;)
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
		text = str4 + base.PropDesc(dashShootExtraEffectMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_cooldownResetHealthThresholdMod, "[CooldownResetHealthThreshold]", flag, (!flag) ? 0f : soldierStimPack.m_cooldownResetHealthThreshold);
		return text + base.PropDesc(this.m_cdrIfDashAndShootUsedMod, "[CdrIfDashAndShootUsed]", flag, (!flag) ? 0 : soldierStimPack.m_cdrIfDashAndShootUsed);
	}
}
