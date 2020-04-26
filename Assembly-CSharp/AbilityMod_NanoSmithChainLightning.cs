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

	[Separator("Extra Absob for Vacuum Bomb cast target", true)]
	public AbilityModPropertyInt m_extraAbsorbPerHitForVacuumBombMod;

	public AbilityModPropertyInt m_maxExtraAbsorbForVacuumBombMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NanoSmithChainLightning);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NanoSmithChainLightning nanoSmithChainLightning = targetAbility as NanoSmithChainLightning;
		if (!(nanoSmithChainLightning != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_laserDamageMod, "LaserDamage", string.Empty, nanoSmithChainLightning.m_laserDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_laserEnemyHitEffectMod, "LaserEnemyHitEffect", nanoSmithChainLightning.m_laserEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, nanoSmithChainLightning.m_laserRange);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, nanoSmithChainLightning.m_laserWidth);
			AbilityMod.AddToken(tokens, m_laserMaxHitsMod, "LaserMaxHits", string.Empty, nanoSmithChainLightning.m_laserMaxHits);
			AbilityMod.AddToken(tokens, m_chainRadiusMod, "ChainRadius", string.Empty, nanoSmithChainLightning.m_chainRadius);
			AbilityMod.AddToken(tokens, m_chainMaxHitsMod, "ChainMaxHits", string.Empty, nanoSmithChainLightning.m_chainMaxHits);
			AbilityMod.AddToken(tokens, m_chainDamageMod, "ChainDamage", string.Empty, nanoSmithChainLightning.m_chainDamage);
			AbilityMod.AddToken(tokens, m_energyPerChainHitMod, "EnergyGainPerChainHit", string.Empty, nanoSmithChainLightning.m_energyGainPerChainHit);
			AbilityMod.AddToken_EffectMod(tokens, m_chainEnemyHitEffectMod, "ChainEnemyHitEffect", nanoSmithChainLightning.m_chainEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_extraAbsorbPerHitForVacuumBombMod, "ExtraAbsorbPerHitForVacuumBomb", string.Empty, nanoSmithChainLightning.m_extraAbsorbPerHitForVacuumBomb);
			AbilityMod.AddToken(tokens, m_maxExtraAbsorbForVacuumBombMod, "MaxExtraAbsorbForVacuumBomb", string.Empty, nanoSmithChainLightning.m_maxExtraAbsorbForVacuumBomb);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NanoSmithChainLightning nanoSmithChainLightning = GetTargetAbilityOnAbilityData(abilityData) as NanoSmithChainLightning;
		bool flag = nanoSmithChainLightning != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt laserDamageMod = m_laserDamageMod;
		int baseVal;
		if (flag)
		{
			baseVal = nanoSmithChainLightning.m_laserDamage;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(laserDamageMod, "[Laser Damage]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyEffectInfo laserEnemyHitEffectMod = m_laserEnemyHitEffectMod;
		object baseVal2;
		if (flag)
		{
			baseVal2 = nanoSmithChainLightning.m_laserEnemyHitEffect;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str2 + PropDesc(laserEnemyHitEffectMod, "[LaserEnemyHitEffect]", flag, (StandardEffectInfo)baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat laserRangeMod = m_laserRangeMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = nanoSmithChainLightning.m_laserRange;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(laserRangeMod, "[LaserRange]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
		float baseVal4;
		if (flag)
		{
			baseVal4 = nanoSmithChainLightning.m_laserWidth;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(laserWidthMod, "[LaserWidth]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyBool penetrateLosMod = m_penetrateLosMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = (nanoSmithChainLightning.m_penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(penetrateLosMod, "[PenetrateLos]", flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertyInt laserMaxHitsMod = m_laserMaxHitsMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = nanoSmithChainLightning.m_laserMaxHits;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(laserMaxHitsMod, "[LaserMaxHits]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyFloat chainRadiusMod = m_chainRadiusMod;
		float baseVal7;
		if (flag)
		{
			baseVal7 = nanoSmithChainLightning.m_chainRadius;
		}
		else
		{
			baseVal7 = 0f;
		}
		empty = str7 + AbilityModHelper.GetModPropertyDesc(chainRadiusMod, "[Chain Radius]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyInt chainDamageMod = m_chainDamageMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = nanoSmithChainLightning.m_chainDamage;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + AbilityModHelper.GetModPropertyDesc(chainDamageMod, "[Chain Damage]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyInt chainMaxHitsMod = m_chainMaxHitsMod;
		int baseVal9;
		if (flag)
		{
			baseVal9 = nanoSmithChainLightning.m_chainMaxHits;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + AbilityModHelper.GetModPropertyDesc(chainMaxHitsMod, "[Chain Max Hits]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyInt energyPerChainHitMod = m_energyPerChainHitMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = nanoSmithChainLightning.m_energyGainPerChainHit;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + AbilityModHelper.GetModPropertyDesc(energyPerChainHitMod, "[Energy Per Chain Hit]", flag, baseVal10);
		string str11 = empty;
		AbilityModPropertyEffectInfo chainEnemyHitEffectMod = m_chainEnemyHitEffectMod;
		object baseVal11;
		if (flag)
		{
			baseVal11 = nanoSmithChainLightning.m_chainEnemyHitEffect;
		}
		else
		{
			baseVal11 = null;
		}
		empty = str11 + PropDesc(chainEnemyHitEffectMod, "[ChainEnemyHitEffect]", flag, (StandardEffectInfo)baseVal11);
		string str12 = empty;
		AbilityModPropertyBool chainCanHitInvisibleActorsMod = m_chainCanHitInvisibleActorsMod;
		int baseVal12;
		if (flag)
		{
			baseVal12 = (nanoSmithChainLightning.m_chainCanHitInvisibleActors ? 1 : 0);
		}
		else
		{
			baseVal12 = 0;
		}
		empty = str12 + PropDesc(chainCanHitInvisibleActorsMod, "[ChainCanHitInvisibleActors]", flag, (byte)baseVal12 != 0);
		string str13 = empty;
		AbilityModPropertyInt extraAbsorbPerHitForVacuumBombMod = m_extraAbsorbPerHitForVacuumBombMod;
		int baseVal13;
		if (flag)
		{
			baseVal13 = nanoSmithChainLightning.m_extraAbsorbPerHitForVacuumBomb;
		}
		else
		{
			baseVal13 = 0;
		}
		empty = str13 + PropDesc(extraAbsorbPerHitForVacuumBombMod, "[ExtraAbsorbPerHitForVacuumBomb]", flag, baseVal13);
		string str14 = empty;
		AbilityModPropertyInt maxExtraAbsorbForVacuumBombMod = m_maxExtraAbsorbForVacuumBombMod;
		int baseVal14;
		if (flag)
		{
			baseVal14 = nanoSmithChainLightning.m_maxExtraAbsorbForVacuumBomb;
		}
		else
		{
			baseVal14 = 0;
		}
		return str14 + PropDesc(maxExtraAbsorbForVacuumBombMod, "[MaxExtraAbsorbForVacuumBomb]", flag, baseVal14);
	}
}
