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
		if (nanoSmithChainLightning != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NanoSmithChainLightning.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_laserDamageMod, "LaserDamage", string.Empty, nanoSmithChainLightning.m_laserDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_laserEnemyHitEffectMod, "LaserEnemyHitEffect", nanoSmithChainLightning.m_laserEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", string.Empty, nanoSmithChainLightning.m_laserRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, nanoSmithChainLightning.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserMaxHitsMod, "LaserMaxHits", string.Empty, nanoSmithChainLightning.m_laserMaxHits, true, false);
			AbilityMod.AddToken(tokens, this.m_chainRadiusMod, "ChainRadius", string.Empty, nanoSmithChainLightning.m_chainRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_chainMaxHitsMod, "ChainMaxHits", string.Empty, nanoSmithChainLightning.m_chainMaxHits, true, false);
			AbilityMod.AddToken(tokens, this.m_chainDamageMod, "ChainDamage", string.Empty, nanoSmithChainLightning.m_chainDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_energyPerChainHitMod, "EnergyGainPerChainHit", string.Empty, nanoSmithChainLightning.m_energyGainPerChainHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_chainEnemyHitEffectMod, "ChainEnemyHitEffect", nanoSmithChainLightning.m_chainEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraAbsorbPerHitForVacuumBombMod, "ExtraAbsorbPerHitForVacuumBomb", string.Empty, nanoSmithChainLightning.m_extraAbsorbPerHitForVacuumBomb, true, false);
			AbilityMod.AddToken(tokens, this.m_maxExtraAbsorbForVacuumBombMod, "MaxExtraAbsorbForVacuumBomb", string.Empty, nanoSmithChainLightning.m_maxExtraAbsorbForVacuumBomb, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NanoSmithChainLightning nanoSmithChainLightning = base.GetTargetAbilityOnAbilityData(abilityData) as NanoSmithChainLightning;
		bool flag = nanoSmithChainLightning != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt laserDamageMod = this.m_laserDamageMod;
		string prefix = "[Laser Damage]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NanoSmithChainLightning.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = nanoSmithChainLightning.m_laserDamage;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(laserDamageMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyEffectInfo laserEnemyHitEffectMod = this.m_laserEnemyHitEffectMod;
		string prefix2 = "[LaserEnemyHitEffect]";
		bool showBaseVal2 = flag;
		StandardEffectInfo baseVal2;
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
			baseVal2 = nanoSmithChainLightning.m_laserEnemyHitEffect;
		}
		else
		{
			baseVal2 = null;
		}
		text = str2 + base.PropDesc(laserEnemyHitEffectMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix3 = "[LaserRange]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = nanoSmithChainLightning.m_laserRange;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(laserRangeMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix4 = "[LaserWidth]";
		bool showBaseVal4 = flag;
		float baseVal4;
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
			baseVal4 = nanoSmithChainLightning.m_laserWidth;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(laserWidthMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool penetrateLosMod = this.m_penetrateLosMod;
		string prefix5 = "[PenetrateLos]";
		bool showBaseVal5 = flag;
		bool baseVal5;
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
			baseVal5 = nanoSmithChainLightning.m_penetrateLos;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(penetrateLosMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt laserMaxHitsMod = this.m_laserMaxHitsMod;
		string prefix6 = "[LaserMaxHits]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = nanoSmithChainLightning.m_laserMaxHits;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(laserMaxHitsMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyFloat chainRadiusMod = this.m_chainRadiusMod;
		string prefix7 = "[Chain Radius]";
		bool showBaseVal7 = flag;
		float baseVal7;
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
			baseVal7 = nanoSmithChainLightning.m_chainRadius;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str7 + AbilityModHelper.GetModPropertyDesc(chainRadiusMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt chainDamageMod = this.m_chainDamageMod;
		string prefix8 = "[Chain Damage]";
		bool showBaseVal8 = flag;
		int baseVal8;
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
			baseVal8 = nanoSmithChainLightning.m_chainDamage;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + AbilityModHelper.GetModPropertyDesc(chainDamageMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt chainMaxHitsMod = this.m_chainMaxHitsMod;
		string prefix9 = "[Chain Max Hits]";
		bool showBaseVal9 = flag;
		int baseVal9;
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
			baseVal9 = nanoSmithChainLightning.m_chainMaxHits;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + AbilityModHelper.GetModPropertyDesc(chainMaxHitsMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt energyPerChainHitMod = this.m_energyPerChainHitMod;
		string prefix10 = "[Energy Per Chain Hit]";
		bool showBaseVal10 = flag;
		int baseVal10;
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
			baseVal10 = nanoSmithChainLightning.m_energyGainPerChainHit;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + AbilityModHelper.GetModPropertyDesc(energyPerChainHitMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyEffectInfo chainEnemyHitEffectMod = this.m_chainEnemyHitEffectMod;
		string prefix11 = "[ChainEnemyHitEffect]";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal11;
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
			baseVal11 = nanoSmithChainLightning.m_chainEnemyHitEffect;
		}
		else
		{
			baseVal11 = null;
		}
		text = str11 + base.PropDesc(chainEnemyHitEffectMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyBool chainCanHitInvisibleActorsMod = this.m_chainCanHitInvisibleActorsMod;
		string prefix12 = "[ChainCanHitInvisibleActors]";
		bool showBaseVal12 = flag;
		bool baseVal12;
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
			baseVal12 = nanoSmithChainLightning.m_chainCanHitInvisibleActors;
		}
		else
		{
			baseVal12 = false;
		}
		text = str12 + base.PropDesc(chainCanHitInvisibleActorsMod, prefix12, showBaseVal12, baseVal12);
		string str13 = text;
		AbilityModPropertyInt extraAbsorbPerHitForVacuumBombMod = this.m_extraAbsorbPerHitForVacuumBombMod;
		string prefix13 = "[ExtraAbsorbPerHitForVacuumBomb]";
		bool showBaseVal13 = flag;
		int baseVal13;
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
			baseVal13 = nanoSmithChainLightning.m_extraAbsorbPerHitForVacuumBomb;
		}
		else
		{
			baseVal13 = 0;
		}
		text = str13 + base.PropDesc(extraAbsorbPerHitForVacuumBombMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyInt maxExtraAbsorbForVacuumBombMod = this.m_maxExtraAbsorbForVacuumBombMod;
		string prefix14 = "[MaxExtraAbsorbForVacuumBomb]";
		bool showBaseVal14 = flag;
		int baseVal14;
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal14 = nanoSmithChainLightning.m_maxExtraAbsorbForVacuumBomb;
		}
		else
		{
			baseVal14 = 0;
		}
		return str14 + base.PropDesc(maxExtraAbsorbForVacuumBombMod, prefix14, showBaseVal14, baseVal14);
	}
}
