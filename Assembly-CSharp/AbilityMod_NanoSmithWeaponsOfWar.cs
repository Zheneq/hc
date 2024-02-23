using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken_EffectMod(tokens, m_allyTargetEffectOverride, "TargetAllyOnHitEffect", nanoSmithWeaponsOfWar.m_targetAllyOnHitEffect);
			AddToken(tokens, m_sweepDamageMod, "SweepDamageAmount", string.Empty, nanoSmithWeaponsOfWar.m_sweepDamageAmount);
			AddToken(tokens, m_sweepDurationMod, "SweepDuration", string.Empty, nanoSmithWeaponsOfWar.m_sweepDuration);
			AddToken_EffectMod(tokens, m_enemySweepOnHitEffectOverride, "EnemySweepOnHitEffect", nanoSmithWeaponsOfWar.m_enemySweepOnHitEffect);
			AddToken_EffectMod(tokens, m_allySweepOnHitEffectOverride, "AllySweepOnHitEffect", nanoSmithWeaponsOfWar.m_allySweepOnHitEffect);
			AddToken(tokens, m_shieldGainPerTurnMod, "ShieldGainPerTurn", string.Empty, 0, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NanoSmithWeaponsOfWar nanoSmithWeaponsOfWar = GetTargetAbilityOnAbilityData(abilityData) as NanoSmithWeaponsOfWar;
		bool isValid = nanoSmithWeaponsOfWar != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_sweepDurationMod, "[Sweep Duration]", isValid, isValid ? nanoSmithWeaponsOfWar.m_sweepDuration : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_sweepDamageMod, "[Sweep Damage]", isValid, isValid ? nanoSmithWeaponsOfWar.m_sweepDamageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_shieldGainPerTurnMod, "[Shield Gain Per Round]", isValid);
		desc += AbilityModHelper.GetModPropertyDesc(m_allyTargetEffectOverride, "{ Ally Target Effect Override }", isValid, isValid ? nanoSmithWeaponsOfWar.m_targetAllyOnHitEffect : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_enemySweepOnHitEffectOverride, "{ Enemy Sweep On Hit Effect Override }", isValid, isValid ? nanoSmithWeaponsOfWar.m_enemySweepOnHitEffect : null);
		return new StringBuilder().Append(desc).Append(AbilityModHelper.GetModPropertyDesc(m_allySweepOnHitEffectOverride, "{ Ally Sweep On Hit Effect Override }", isValid, isValid ? nanoSmithWeaponsOfWar.m_allySweepOnHitEffect : null)).ToString();
	}
}
