using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NanoSmithWeaponsOfWar : AbilityMod
{
	[Space(10f)]
	public AbilityModPropertyInt m_sweepDurationMod;

	public AbilityModPropertyInt m_sweepDamageMod;

	public AbilityModPropertyInt m_shieldGainPerTurnMod;

	public AbilityModPropertyEffectInfo m_allyTargetEffectOverride;

	public AbilityModPropertyEffectInfo m_enemySweepOnHitEffectOverride;

	public AbilityModPropertyEffectInfo m_allySweepOnHitEffectOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(NanoSmithWeaponsOfWar);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NanoSmithWeaponsOfWar nanoSmithWeaponsOfWar = targetAbility as NanoSmithWeaponsOfWar;
		if (nanoSmithWeaponsOfWar != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NanoSmithWeaponsOfWar.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyTargetEffectOverride, "TargetAllyOnHitEffect", nanoSmithWeaponsOfWar.m_targetAllyOnHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_sweepDamageMod, "SweepDamageAmount", string.Empty, nanoSmithWeaponsOfWar.m_sweepDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_sweepDurationMod, "SweepDuration", string.Empty, nanoSmithWeaponsOfWar.m_sweepDuration, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemySweepOnHitEffectOverride, "EnemySweepOnHitEffect", nanoSmithWeaponsOfWar.m_enemySweepOnHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allySweepOnHitEffectOverride, "AllySweepOnHitEffect", nanoSmithWeaponsOfWar.m_allySweepOnHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_shieldGainPerTurnMod, "ShieldGainPerTurn", string.Empty, 0, false, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NanoSmithWeaponsOfWar nanoSmithWeaponsOfWar = base.GetTargetAbilityOnAbilityData(abilityData) as NanoSmithWeaponsOfWar;
		bool flag = nanoSmithWeaponsOfWar != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt sweepDurationMod = this.m_sweepDurationMod;
		string prefix = "[Sweep Duration]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NanoSmithWeaponsOfWar.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = nanoSmithWeaponsOfWar.m_sweepDuration;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(sweepDurationMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt sweepDamageMod = this.m_sweepDamageMod;
		string prefix2 = "[Sweep Damage]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = nanoSmithWeaponsOfWar.m_sweepDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(sweepDamageMod, prefix2, showBaseVal2, baseVal2);
		text += AbilityModHelper.GetModPropertyDesc(this.m_shieldGainPerTurnMod, "[Shield Gain Per Round]", flag, 0);
		string str3 = text;
		AbilityModPropertyEffectInfo allyTargetEffectOverride = this.m_allyTargetEffectOverride;
		string prefix3 = "{ Ally Target Effect Override }";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
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
			baseVal3 = nanoSmithWeaponsOfWar.m_targetAllyOnHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(allyTargetEffectOverride, prefix3, showBaseVal3, baseVal3);
		text += AbilityModHelper.GetModPropertyDesc(this.m_enemySweepOnHitEffectOverride, "{ Enemy Sweep On Hit Effect Override }", flag, (!flag) ? null : nanoSmithWeaponsOfWar.m_enemySweepOnHitEffect);
		return text + AbilityModHelper.GetModPropertyDesc(this.m_allySweepOnHitEffectOverride, "{ Ally Sweep On Hit Effect Override }", flag, (!flag) ? null : nanoSmithWeaponsOfWar.m_allySweepOnHitEffect);
	}
}
