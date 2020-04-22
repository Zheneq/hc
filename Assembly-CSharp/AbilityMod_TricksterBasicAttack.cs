using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TricksterBasicAttack : AbilityMod
{
	[Header("-- Laser Targeting")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;

	[Header("-- Damage and Effect")]
	public AbilityModPropertyInt m_laserDamageAmountMod;

	public AbilityModPropertyInt m_laserSubsequentDamageAmountMod;

	public AbilityModPropertyInt m_extraDamageForSingleHitMod;

	public AbilityModPropertyEffectInfo m_enemySingleHitHitEffectMod;

	public AbilityModPropertyEffectInfo m_enemyMultiHitEffectMod;

	[Header("-- Effect on Self for Multi Hit")]
	public AbilityModPropertyEffectInfo m_selfEffectForMultiHitMod;

	[Header("-- Energy Gain --")]
	public AbilityModPropertyInt m_energyGainPerLaserHitMod;

	[Header("-- For spawning spoils")]
	public AbilityModPropertySpoilsSpawnData m_spoilSpawnInfoMod;

	public AbilityModPropertyBool m_onlySpawnSpoilOnMultiHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(TricksterBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TricksterBasicAttack tricksterBasicAttack = targetAbility as TricksterBasicAttack;
		if (!(tricksterBasicAttack != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", tricksterBasicAttack.m_laserInfo);
			AbilityMod.AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, tricksterBasicAttack.m_laserDamageAmount);
			AbilityMod.AddToken(tokens, m_laserSubsequentDamageAmountMod, "LaserSubsequentDamageAmount", string.Empty, tricksterBasicAttack.m_laserSubsequentDamageAmount);
			AbilityMod.AddToken(tokens, m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, tricksterBasicAttack.m_extraDamageForSingleHit);
			AbilityMod.AddToken_EffectMod(tokens, m_enemySingleHitHitEffectMod, "EnemySingleHitHitEffect", tricksterBasicAttack.m_enemySingleHitHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyMultiHitEffectMod, "EnemyMultiHitEffect", tricksterBasicAttack.m_enemyMultiHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_selfEffectForMultiHitMod, "SelfEffectForMultiHit", tricksterBasicAttack.m_selfEffectForMultiHit);
			AbilityMod.AddToken(tokens, m_energyGainPerLaserHitMod, "EnergyGainPerLaserHit", string.Empty, tricksterBasicAttack.m_energyGainPerLaserHit);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TricksterBasicAttack tricksterBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as TricksterBasicAttack;
		bool flag = tricksterBasicAttack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyLaserInfo laserInfoMod = m_laserInfoMod;
		object baseLaserInfo;
		if (flag)
		{
			while (true)
			{
				switch (5)
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
			baseLaserInfo = tricksterBasicAttack.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		empty = str + PropDesc(laserInfoMod, "[LaserInfo]", flag, (LaserTargetingInfo)baseLaserInfo);
		string str2 = empty;
		AbilityModPropertyInt laserDamageAmountMod = m_laserDamageAmountMod;
		int baseVal;
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
			baseVal = tricksterBasicAttack.m_laserDamageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str2 + PropDesc(laserDamageAmountMod, "[LaserDamageAmount]", flag, baseVal);
		string str3 = empty;
		AbilityModPropertyInt laserSubsequentDamageAmountMod = m_laserSubsequentDamageAmountMod;
		int baseVal2;
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
			baseVal2 = tricksterBasicAttack.m_laserSubsequentDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str3 + PropDesc(laserSubsequentDamageAmountMod, "[LaserSubsequentDamageAmount]", flag, baseVal2);
		string str4 = empty;
		AbilityModPropertyInt extraDamageForSingleHitMod = m_extraDamageForSingleHitMod;
		int baseVal3;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = tricksterBasicAttack.m_extraDamageForSingleHit;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str4 + PropDesc(extraDamageForSingleHitMod, "[ExtraDamageForSingleHit]", flag, baseVal3);
		string str5 = empty;
		AbilityModPropertyEffectInfo enemySingleHitHitEffectMod = m_enemySingleHitHitEffectMod;
		object baseVal4;
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
			baseVal4 = tricksterBasicAttack.m_enemySingleHitHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str5 + PropDesc(enemySingleHitHitEffectMod, "[EnemySingleHitHitEffect]", flag, (StandardEffectInfo)baseVal4);
		string str6 = empty;
		AbilityModPropertyEffectInfo enemyMultiHitEffectMod = m_enemyMultiHitEffectMod;
		object baseVal5;
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
			baseVal5 = tricksterBasicAttack.m_enemyMultiHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str6 + PropDesc(enemyMultiHitEffectMod, "[EnemyMultiHitEffect]", flag, (StandardEffectInfo)baseVal5);
		string str7 = empty;
		AbilityModPropertyEffectInfo selfEffectForMultiHitMod = m_selfEffectForMultiHitMod;
		object baseVal6;
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
			baseVal6 = tricksterBasicAttack.m_selfEffectForMultiHit;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str7 + PropDesc(selfEffectForMultiHitMod, "[SelfEffectForMultiHit]", flag, (StandardEffectInfo)baseVal6);
		string str8 = empty;
		AbilityModPropertyInt energyGainPerLaserHitMod = m_energyGainPerLaserHitMod;
		int baseVal7;
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
			baseVal7 = tricksterBasicAttack.m_energyGainPerLaserHit;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str8 + PropDesc(energyGainPerLaserHitMod, "[EnergyGainPerLaserHit]", flag, baseVal7);
		string str9 = empty;
		AbilityModPropertySpoilsSpawnData spoilSpawnInfoMod = m_spoilSpawnInfoMod;
		object baseVal8;
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
			baseVal8 = tricksterBasicAttack.m_spoilSpawnInfo;
		}
		else
		{
			baseVal8 = null;
		}
		empty = str9 + PropDesc(spoilSpawnInfoMod, "[SpoilSpawnInfo]", flag, (SpoilsSpawnData)baseVal8);
		return empty + PropDesc(m_onlySpawnSpoilOnMultiHitMod, "[OnlySpawnSpoilOnMultiHit]", flag, flag && tricksterBasicAttack.m_onlySpawnSpoilOnMultiHit);
	}
}
