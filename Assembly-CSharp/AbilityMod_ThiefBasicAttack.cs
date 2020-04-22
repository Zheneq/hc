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
			AbilityMod.AddToken(tokens, m_targeterMaxAngleMod, "TargeterMaxAngle", string.Empty, thiefBasicAttack.m_targeterMaxAngle);
			AbilityMod.AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, thiefBasicAttack.m_laserDamageAmount);
			AbilityMod.AddToken(tokens, m_laserSubsequentDamageAmountMod, "LaserSubsequentDamageAmount", string.Empty, thiefBasicAttack.m_laserSubsequentDamageAmount);
			AbilityMod.AddToken(tokens, m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, thiefBasicAttack.m_extraDamageForSingleHit);
			AbilityMod.AddToken(tokens, m_extraDamageForHittingPowerupMod, "ExtraDamageForHittingPowerup", string.Empty, thiefBasicAttack.m_extraDamageForHittingPowerup);
			AbilityMod.AddToken(tokens, m_healOnSelfIfHitEnemyAndPowerupMod, "HealOnSelfIfHitEnemyAndPowerup", string.Empty, thiefBasicAttack.m_healOnSelfIfHitEnemyAndPowerup);
			AbilityMod.AddToken(tokens, m_energyGainPerLaserHitMod, "EnergyGainPerLaserHit", string.Empty, thiefBasicAttack.m_energyGainPerLaserHit);
			AbilityMod.AddToken(tokens, m_energyGainPerPowerupHitMod, "EnergyGainPerPowerupHit", string.Empty, thiefBasicAttack.m_energyGainPerPowerupHit);
			AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, thiefBasicAttack.m_laserRange);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, thiefBasicAttack.m_laserWidth);
			AbilityMod.AddToken(tokens, m_laserMaxTargetsMod, "LaserMaxTargets", string.Empty, thiefBasicAttack.m_laserMaxTargets);
			AbilityMod.AddToken(tokens, m_laserCountMod, "LaserCount", string.Empty, thiefBasicAttack.m_laserCount);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefBasicAttack thiefBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as ThiefBasicAttack;
		bool flag = thiefBasicAttack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat targeterMaxAngleMod = m_targeterMaxAngleMod;
		float baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = thiefBasicAttack.m_targeterMaxAngle;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(targeterMaxAngleMod, "[TargeterMaxAngle]", flag, baseVal);
		empty += PropDesc(m_laserDamageAmountMod, "[LaserDamageAmount]", flag, flag ? thiefBasicAttack.m_laserDamageAmount : 0);
		string str2 = empty;
		AbilityModPropertyInt laserSubsequentDamageAmountMod = m_laserSubsequentDamageAmountMod;
		int baseVal2;
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
			baseVal2 = thiefBasicAttack.m_laserSubsequentDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(laserSubsequentDamageAmountMod, "[LaserSubsequentDamageAmount]", flag, baseVal2);
		empty += PropDesc(m_extraDamageForSingleHitMod, "[ExtraDamageForSingleHit]", flag, flag ? thiefBasicAttack.m_extraDamageForSingleHit : 0);
		string str3 = empty;
		AbilityModPropertyInt extraDamageForHittingPowerupMod = m_extraDamageForHittingPowerupMod;
		int baseVal3;
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
			baseVal3 = thiefBasicAttack.m_extraDamageForHittingPowerup;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(extraDamageForHittingPowerupMod, "[ExtraDamageForHittingPowerup]", flag, baseVal3);
		empty += PropDesc(m_healOnSelfIfHitEnemyAndPowerupMod, "[HealOnSelfIfHitEnemyAndPowerup]", flag, flag ? thiefBasicAttack.m_healOnSelfIfHitEnemyAndPowerup : 0);
		string str4 = empty;
		AbilityModPropertyInt energyGainPerLaserHitMod = m_energyGainPerLaserHitMod;
		int baseVal4;
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
			baseVal4 = thiefBasicAttack.m_energyGainPerLaserHit;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(energyGainPerLaserHitMod, "[EnergyGainPerLaserHit]", flag, baseVal4);
		empty += PropDesc(m_energyGainPerPowerupHitMod, "[EnergyGainPerPowerupHit]", flag, flag ? thiefBasicAttack.m_energyGainPerPowerupHit : 0);
		string str5 = empty;
		AbilityModPropertyFloat laserRangeMod = m_laserRangeMod;
		float baseVal5;
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
			baseVal5 = thiefBasicAttack.m_laserRange;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + PropDesc(laserRangeMod, "[LaserRange]", flag, baseVal5);
		empty += PropDesc(m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : thiefBasicAttack.m_laserWidth);
		string str6 = empty;
		AbilityModPropertyInt laserMaxTargetsMod = m_laserMaxTargetsMod;
		int baseVal6;
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
			baseVal6 = thiefBasicAttack.m_laserMaxTargets;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(laserMaxTargetsMod, "[LaserMaxTargets]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyInt laserCountMod = m_laserCountMod;
		int baseVal7;
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
			baseVal7 = thiefBasicAttack.m_laserCount;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(laserCountMod, "[LaserCount]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyBool laserPenetrateLosMod = m_laserPenetrateLosMod;
		int baseVal8;
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
			baseVal8 = (thiefBasicAttack.m_laserPenetrateLos ? 1 : 0);
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(laserPenetrateLosMod, "[LaserPenetrateLos]", flag, (byte)baseVal8 != 0);
		string str9 = empty;
		AbilityModPropertyBool stopOnPowerupHitMod = m_stopOnPowerupHitMod;
		int baseVal9;
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
			baseVal9 = (thiefBasicAttack.m_stopOnPowerupHit ? 1 : 0);
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(stopOnPowerupHitMod, "[StopOnPowerupHit]", flag, (byte)baseVal9 != 0);
		string str10 = empty;
		AbilityModPropertyBool includeSpoilsPowerupsMod = m_includeSpoilsPowerupsMod;
		int baseVal10;
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
			baseVal10 = (thiefBasicAttack.m_includeSpoilsPowerups ? 1 : 0);
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(includeSpoilsPowerupsMod, "[IncludeSpoilsPowerups]", flag, (byte)baseVal10 != 0);
		return empty + PropDesc(m_ignorePickupTeamRestrictionMod, "[IgnorePickupTeamRestriction]", flag, flag && thiefBasicAttack.m_ignorePickupTeamRestriction);
	}
}
