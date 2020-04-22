using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClaymoreSilenceLaser : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyInt m_laserMaxTargetsMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	[Header("-- Damage and Effect")]
	public AbilityModPropertyInt m_effectExplosionDamageMod;

	public AbilityModPropertyInt m_explosionDamageAfterFirstHitMod;

	public AbilityModPropertyEffectData m_enemyHitEffectDataMod;

	[Header("-- On Reaction Hit/Explosion")]
	public AbilityModPropertyInt m_onCastDamageAmountMod;

	public AbilityModPropertyBool m_explosionReduceCooldownOnlyIfHitByAllyMod;

	public AbilityModPropertyInt m_explosionCooldownReductionMod;

	public AbilityModPropertyBool m_canExplodeOncePerTurnMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClaymoreSilenceLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClaymoreSilenceLaser claymoreSilenceLaser = targetAbility as ClaymoreSilenceLaser;
		if (!(claymoreSilenceLaser != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, claymoreSilenceLaser.m_laserRange);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, claymoreSilenceLaser.m_laserWidth);
			AbilityMod.AddToken(tokens, m_laserMaxTargetsMod, "LaserMaxTargets", string.Empty, claymoreSilenceLaser.m_laserMaxTargets);
			AbilityMod.AddToken(tokens, m_onCastDamageAmountMod, "OnCastDamageAmount", string.Empty, claymoreSilenceLaser.m_onCastDamageAmount);
			AbilityMod.AddToken(tokens, m_effectExplosionDamageMod, "EffectExplosionDamage", string.Empty, claymoreSilenceLaser.m_effectExplosionDamage);
			AbilityMod.AddToken(tokens, m_explosionDamageAfterFirstHitMod, "ExplosionDamageAfterFirstHit", string.Empty, claymoreSilenceLaser.m_explosionDamageAfterFirstHit);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectDataMod, "EnemyHitEffectData", claymoreSilenceLaser.m_enemyHitEffectData);
			AbilityMod.AddToken(tokens, m_explosionCooldownReductionMod, "ExplosionCooldownReduction", string.Empty, claymoreSilenceLaser.m_explosionCooldownReduction);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreSilenceLaser claymoreSilenceLaser = GetTargetAbilityOnAbilityData(abilityData) as ClaymoreSilenceLaser;
		bool flag = claymoreSilenceLaser != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat laserRangeMod = m_laserRangeMod;
		float baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = claymoreSilenceLaser.m_laserRange;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(laserRangeMod, "[Laser Range]", flag, baseVal);
		empty += PropDesc(m_laserWidthMod, "[Laser Width]", flag, (!flag) ? 0f : claymoreSilenceLaser.m_laserWidth);
		string str2 = empty;
		AbilityModPropertyInt laserMaxTargetsMod = m_laserMaxTargetsMod;
		int baseVal2;
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
			baseVal2 = claymoreSilenceLaser.m_laserMaxTargets;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(laserMaxTargetsMod, "[Laser Max Targets]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyBool penetrateLosMod = m_penetrateLosMod;
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
			baseVal3 = (claymoreSilenceLaser.m_penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(penetrateLosMod, "[Penetrate Los]", flag, (byte)baseVal3 != 0);
		string str4 = empty;
		AbilityModPropertyInt onCastDamageAmountMod = m_onCastDamageAmountMod;
		int baseVal4;
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
			baseVal4 = claymoreSilenceLaser.m_onCastDamageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(onCastDamageAmountMod, "[On Cast Damage Amount]", flag, baseVal4);
		empty += PropDesc(m_enemyHitEffectDataMod, "[Enemy Hit Effect Data]", flag, (!flag) ? null : claymoreSilenceLaser.m_enemyHitEffectData);
		string str5 = empty;
		AbilityModPropertyInt effectExplosionDamageMod = m_effectExplosionDamageMod;
		int baseVal5;
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
			baseVal5 = claymoreSilenceLaser.m_effectExplosionDamage;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(effectExplosionDamageMod, "[Effect Explosion Damage]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt explosionDamageAfterFirstHitMod = m_explosionDamageAfterFirstHitMod;
		int baseVal6;
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
			baseVal6 = claymoreSilenceLaser.m_explosionDamageAfterFirstHit;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(explosionDamageAfterFirstHitMod, "[ExplosionDamageAfterFirstHit]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyBool explosionReduceCooldownOnlyIfHitByAllyMod = m_explosionReduceCooldownOnlyIfHitByAllyMod;
		int baseVal7;
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
			baseVal7 = (claymoreSilenceLaser.m_explosionReduceCooldownOnlyIfHitByAlly ? 1 : 0);
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(explosionReduceCooldownOnlyIfHitByAllyMod, "[ExplosionReduceCooldownOnlyIfHitByAlly]", flag, (byte)baseVal7 != 0);
		string str8 = empty;
		AbilityModPropertyInt explosionCooldownReductionMod = m_explosionCooldownReductionMod;
		int baseVal8;
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
			baseVal8 = claymoreSilenceLaser.m_explosionCooldownReduction;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(explosionCooldownReductionMod, "[ExplosionCooldownReduction]", flag, baseVal8);
		return empty + PropDesc(m_canExplodeOncePerTurnMod, "[ExplodeOncePerTurn]", flag);
	}
}
