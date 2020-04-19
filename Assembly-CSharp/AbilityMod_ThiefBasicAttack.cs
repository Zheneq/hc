using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ThiefBasicAttack : AbilityMod
{
	[Header("-- Targeter")]
	public AbilityModPropertyFloat m_targeterMaxAngleMod;

	[Header("-- Damage")]
	public AbilityModPropertyInt m_laserDamageAmountMod;

	public AbilityModPropertyInt m_laserSubsequentDamageAmountMod;

	public AbilityModPropertyInt m_extraDamageForSingleHitMod;

	public AbilityModPropertyInt m_extraDamageForHittingPowerupMod;

	[Header("-- Healing")]
	public AbilityModPropertyInt m_healOnSelfIfHitEnemyAndPowerupMod;

	[Header("-- Energy")]
	public AbilityModPropertyInt m_energyGainPerLaserHitMod;

	public AbilityModPropertyInt m_energyGainPerPowerupHitMod;

	[Header("-- Laser Properties")]
	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyInt m_laserMaxTargetsMod;

	public AbilityModPropertyInt m_laserCountMod;

	public AbilityModPropertyBool m_laserPenetrateLosMod;

	[Header("-- PowerUp/Spoils Interaction")]
	public AbilityModPropertyBool m_stopOnPowerupHitMod;

	public AbilityModPropertyBool m_includeSpoilsPowerupsMod;

	public AbilityModPropertyBool m_ignorePickupTeamRestrictionMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ThiefBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ThiefBasicAttack thiefBasicAttack = targetAbility as ThiefBasicAttack;
		if (thiefBasicAttack != null)
		{
			AbilityMod.AddToken(tokens, this.m_targeterMaxAngleMod, "TargeterMaxAngle", string.Empty, thiefBasicAttack.m_targeterMaxAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, thiefBasicAttack.m_laserDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_laserSubsequentDamageAmountMod, "LaserSubsequentDamageAmount", string.Empty, thiefBasicAttack.m_laserSubsequentDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, thiefBasicAttack.m_extraDamageForSingleHit, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForHittingPowerupMod, "ExtraDamageForHittingPowerup", string.Empty, thiefBasicAttack.m_extraDamageForHittingPowerup, true, false);
			AbilityMod.AddToken(tokens, this.m_healOnSelfIfHitEnemyAndPowerupMod, "HealOnSelfIfHitEnemyAndPowerup", string.Empty, thiefBasicAttack.m_healOnSelfIfHitEnemyAndPowerup, true, false);
			AbilityMod.AddToken(tokens, this.m_energyGainPerLaserHitMod, "EnergyGainPerLaserHit", string.Empty, thiefBasicAttack.m_energyGainPerLaserHit, true, false);
			AbilityMod.AddToken(tokens, this.m_energyGainPerPowerupHitMod, "EnergyGainPerPowerupHit", string.Empty, thiefBasicAttack.m_energyGainPerPowerupHit, true, false);
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", string.Empty, thiefBasicAttack.m_laserRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, thiefBasicAttack.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserMaxTargetsMod, "LaserMaxTargets", string.Empty, thiefBasicAttack.m_laserMaxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_laserCountMod, "LaserCount", string.Empty, thiefBasicAttack.m_laserCount, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefBasicAttack thiefBasicAttack = base.GetTargetAbilityOnAbilityData(abilityData) as ThiefBasicAttack;
		bool flag = thiefBasicAttack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat targeterMaxAngleMod = this.m_targeterMaxAngleMod;
		string prefix = "[TargeterMaxAngle]";
		bool showBaseVal = flag;
		float baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ThiefBasicAttack.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = thiefBasicAttack.m_targeterMaxAngle;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(targeterMaxAngleMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_laserDamageAmountMod, "[LaserDamageAmount]", flag, (!flag) ? 0 : thiefBasicAttack.m_laserDamageAmount);
		string str2 = text;
		AbilityModPropertyInt laserSubsequentDamageAmountMod = this.m_laserSubsequentDamageAmountMod;
		string prefix2 = "[LaserSubsequentDamageAmount]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = thiefBasicAttack.m_laserSubsequentDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(laserSubsequentDamageAmountMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_extraDamageForSingleHitMod, "[ExtraDamageForSingleHit]", flag, (!flag) ? 0 : thiefBasicAttack.m_extraDamageForSingleHit);
		string str3 = text;
		AbilityModPropertyInt extraDamageForHittingPowerupMod = this.m_extraDamageForHittingPowerupMod;
		string prefix3 = "[ExtraDamageForHittingPowerup]";
		bool showBaseVal3 = flag;
		int baseVal3;
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
			baseVal3 = thiefBasicAttack.m_extraDamageForHittingPowerup;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(extraDamageForHittingPowerupMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_healOnSelfIfHitEnemyAndPowerupMod, "[HealOnSelfIfHitEnemyAndPowerup]", flag, (!flag) ? 0 : thiefBasicAttack.m_healOnSelfIfHitEnemyAndPowerup);
		string str4 = text;
		AbilityModPropertyInt energyGainPerLaserHitMod = this.m_energyGainPerLaserHitMod;
		string prefix4 = "[EnergyGainPerLaserHit]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = thiefBasicAttack.m_energyGainPerLaserHit;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(energyGainPerLaserHitMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_energyGainPerPowerupHitMod, "[EnergyGainPerPowerupHit]", flag, (!flag) ? 0 : thiefBasicAttack.m_energyGainPerPowerupHit);
		string str5 = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix5 = "[LaserRange]";
		bool showBaseVal5 = flag;
		float baseVal5;
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
			baseVal5 = thiefBasicAttack.m_laserRange;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + base.PropDesc(laserRangeMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : thiefBasicAttack.m_laserWidth);
		string str6 = text;
		AbilityModPropertyInt laserMaxTargetsMod = this.m_laserMaxTargetsMod;
		string prefix6 = "[LaserMaxTargets]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = thiefBasicAttack.m_laserMaxTargets;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(laserMaxTargetsMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt laserCountMod = this.m_laserCountMod;
		string prefix7 = "[LaserCount]";
		bool showBaseVal7 = flag;
		int baseVal7;
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
			baseVal7 = thiefBasicAttack.m_laserCount;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(laserCountMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyBool laserPenetrateLosMod = this.m_laserPenetrateLosMod;
		string prefix8 = "[LaserPenetrateLos]";
		bool showBaseVal8 = flag;
		bool baseVal8;
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
			baseVal8 = thiefBasicAttack.m_laserPenetrateLos;
		}
		else
		{
			baseVal8 = false;
		}
		text = str8 + base.PropDesc(laserPenetrateLosMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyBool stopOnPowerupHitMod = this.m_stopOnPowerupHitMod;
		string prefix9 = "[StopOnPowerupHit]";
		bool showBaseVal9 = flag;
		bool baseVal9;
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
			baseVal9 = thiefBasicAttack.m_stopOnPowerupHit;
		}
		else
		{
			baseVal9 = false;
		}
		text = str9 + base.PropDesc(stopOnPowerupHitMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyBool includeSpoilsPowerupsMod = this.m_includeSpoilsPowerupsMod;
		string prefix10 = "[IncludeSpoilsPowerups]";
		bool showBaseVal10 = flag;
		bool baseVal10;
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
			baseVal10 = thiefBasicAttack.m_includeSpoilsPowerups;
		}
		else
		{
			baseVal10 = false;
		}
		text = str10 + base.PropDesc(includeSpoilsPowerupsMod, prefix10, showBaseVal10, baseVal10);
		return text + base.PropDesc(this.m_ignorePickupTeamRestrictionMod, "[IgnorePickupTeamRestriction]", flag, flag && thiefBasicAttack.m_ignorePickupTeamRestriction);
	}
}
