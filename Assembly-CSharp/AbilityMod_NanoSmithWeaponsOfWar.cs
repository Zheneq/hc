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
		if (!(nanoSmithWeaponsOfWar != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken_EffectMod(tokens, m_allyTargetEffectOverride, "TargetAllyOnHitEffect", nanoSmithWeaponsOfWar.m_targetAllyOnHitEffect);
			AbilityMod.AddToken(tokens, m_sweepDamageMod, "SweepDamageAmount", string.Empty, nanoSmithWeaponsOfWar.m_sweepDamageAmount);
			AbilityMod.AddToken(tokens, m_sweepDurationMod, "SweepDuration", string.Empty, nanoSmithWeaponsOfWar.m_sweepDuration);
			AbilityMod.AddToken_EffectMod(tokens, m_enemySweepOnHitEffectOverride, "EnemySweepOnHitEffect", nanoSmithWeaponsOfWar.m_enemySweepOnHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_allySweepOnHitEffectOverride, "AllySweepOnHitEffect", nanoSmithWeaponsOfWar.m_allySweepOnHitEffect);
			AbilityMod.AddToken(tokens, m_shieldGainPerTurnMod, "ShieldGainPerTurn", string.Empty, 0, false);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NanoSmithWeaponsOfWar nanoSmithWeaponsOfWar = GetTargetAbilityOnAbilityData(abilityData) as NanoSmithWeaponsOfWar;
		bool flag = nanoSmithWeaponsOfWar != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt sweepDurationMod = m_sweepDurationMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (2)
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
			baseVal = nanoSmithWeaponsOfWar.m_sweepDuration;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(sweepDurationMod, "[Sweep Duration]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt sweepDamageMod = m_sweepDamageMod;
		int baseVal2;
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
			baseVal2 = nanoSmithWeaponsOfWar.m_sweepDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(sweepDamageMod, "[Sweep Damage]", flag, baseVal2);
		empty += AbilityModHelper.GetModPropertyDesc(m_shieldGainPerTurnMod, "[Shield Gain Per Round]", flag);
		string str3 = empty;
		AbilityModPropertyEffectInfo allyTargetEffectOverride = m_allyTargetEffectOverride;
		object baseVal3;
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
			baseVal3 = nanoSmithWeaponsOfWar.m_targetAllyOnHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(allyTargetEffectOverride, "{ Ally Target Effect Override }", flag, (StandardEffectInfo)baseVal3);
		empty += AbilityModHelper.GetModPropertyDesc(m_enemySweepOnHitEffectOverride, "{ Enemy Sweep On Hit Effect Override }", flag, (!flag) ? null : nanoSmithWeaponsOfWar.m_enemySweepOnHitEffect);
		return empty + AbilityModHelper.GetModPropertyDesc(m_allySweepOnHitEffectOverride, "{ Ally Sweep On Hit Effect Override }", flag, (!flag) ? null : nanoSmithWeaponsOfWar.m_allySweepOnHitEffect);
	}
}
