using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NanoSmithChainLightning : AbilityMod
{
	[Space(10f)]
	public AbilityModPropertyInt m_laserDamageMod;
	public AbilityModPropertyEffectInfo m_laserEnemyHitEffectMod;
	public AbilityModPropertyFloat m_laserRangeMod;
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyBool m_penetrateLosMod;
	public AbilityModPropertyInt m_laserMaxHitsMod;
	[Header("-- Chain Mods")]
	public AbilityModPropertyFloat m_chainRadiusMod;
	public AbilityModPropertyInt m_chainDamageMod;
	public AbilityModPropertyInt m_chainMaxHitsMod;
	public AbilityModPropertyInt m_energyPerChainHitMod;
	public AbilityModPropertyEffectInfo m_chainEnemyHitEffectMod;
	public AbilityModPropertyBool m_chainCanHitInvisibleActorsMod;
	[Separator("Extra Absob for Vacuum Bomb cast target")]
	public AbilityModPropertyInt m_extraAbsorbPerHitForVacuumBombMod;
	public AbilityModPropertyInt m_maxExtraAbsorbForVacuumBombMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NanoSmithChainLightning);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NanoSmithChainLightning nanoSmithChainLightning = targetAbility as NanoSmithChainLightning;
		if (nanoSmithChainLightning != null)
		{
			AddToken(tokens, m_laserDamageMod, "LaserDamage", string.Empty, nanoSmithChainLightning.m_laserDamage);
			AddToken_EffectMod(tokens, m_laserEnemyHitEffectMod, "LaserEnemyHitEffect", nanoSmithChainLightning.m_laserEnemyHitEffect);
			AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, nanoSmithChainLightning.m_laserRange);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, nanoSmithChainLightning.m_laserWidth);
			AddToken(tokens, m_laserMaxHitsMod, "LaserMaxHits", string.Empty, nanoSmithChainLightning.m_laserMaxHits);
			AddToken(tokens, m_chainRadiusMod, "ChainRadius", string.Empty, nanoSmithChainLightning.m_chainRadius);
			AddToken(tokens, m_chainMaxHitsMod, "ChainMaxHits", string.Empty, nanoSmithChainLightning.m_chainMaxHits);
			AddToken(tokens, m_chainDamageMod, "ChainDamage", string.Empty, nanoSmithChainLightning.m_chainDamage);
			AddToken(tokens, m_energyPerChainHitMod, "EnergyGainPerChainHit", string.Empty, nanoSmithChainLightning.m_energyGainPerChainHit);
			AddToken_EffectMod(tokens, m_chainEnemyHitEffectMod, "ChainEnemyHitEffect", nanoSmithChainLightning.m_chainEnemyHitEffect);
			AddToken(tokens, m_extraAbsorbPerHitForVacuumBombMod, "ExtraAbsorbPerHitForVacuumBomb", string.Empty, nanoSmithChainLightning.m_extraAbsorbPerHitForVacuumBomb);
			AddToken(tokens, m_maxExtraAbsorbForVacuumBombMod, "MaxExtraAbsorbForVacuumBomb", string.Empty, nanoSmithChainLightning.m_maxExtraAbsorbForVacuumBomb);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NanoSmithChainLightning nanoSmithChainLightning = GetTargetAbilityOnAbilityData(abilityData) as NanoSmithChainLightning;
		bool isValid = nanoSmithChainLightning != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_laserDamageMod, "[Laser Damage]", isValid, isValid ? nanoSmithChainLightning.m_laserDamage : 0);
		desc += PropDesc(m_laserEnemyHitEffectMod, "[LaserEnemyHitEffect]", isValid, isValid ? nanoSmithChainLightning.m_laserEnemyHitEffect : null);
		desc += PropDesc(m_laserRangeMod, "[LaserRange]", isValid, isValid ? nanoSmithChainLightning.m_laserRange : 0f);
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isValid, isValid ? nanoSmithChainLightning.m_laserWidth : 0f);
		desc += PropDesc(m_penetrateLosMod, "[PenetrateLos]", isValid, isValid && nanoSmithChainLightning.m_penetrateLos);
		desc += PropDesc(m_laserMaxHitsMod, "[LaserMaxHits]", isValid, isValid ? nanoSmithChainLightning.m_laserMaxHits : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_chainRadiusMod, "[Chain Radius]", isValid, isValid ? nanoSmithChainLightning.m_chainRadius : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_chainDamageMod, "[Chain Damage]", isValid, isValid ? nanoSmithChainLightning.m_chainDamage : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_chainMaxHitsMod, "[Chain Max Hits]", isValid, isValid ? nanoSmithChainLightning.m_chainMaxHits : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_energyPerChainHitMod, "[Energy Per Chain Hit]", isValid, isValid ? nanoSmithChainLightning.m_energyGainPerChainHit : 0);
		desc += PropDesc(m_chainEnemyHitEffectMod, "[ChainEnemyHitEffect]", isValid, isValid ? nanoSmithChainLightning.m_chainEnemyHitEffect : null);
		desc += PropDesc(m_chainCanHitInvisibleActorsMod, "[ChainCanHitInvisibleActors]", isValid, isValid && nanoSmithChainLightning.m_chainCanHitInvisibleActors);
		desc += PropDesc(m_extraAbsorbPerHitForVacuumBombMod, "[ExtraAbsorbPerHitForVacuumBomb]", isValid, isValid ? nanoSmithChainLightning.m_extraAbsorbPerHitForVacuumBomb : 0);
		return desc + PropDesc(m_maxExtraAbsorbForVacuumBombMod, "[MaxExtraAbsorbForVacuumBomb]", isValid, isValid ? nanoSmithChainLightning.m_maxExtraAbsorbForVacuumBomb : 0);
	}
}
