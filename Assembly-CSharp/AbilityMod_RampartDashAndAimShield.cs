using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RampartDashAndAimShield : AbilityMod
{
	[Header("-- Charge Size")]
	public AbilityModPropertyFloat m_chargeRadiusMod;

	public AbilityModPropertyFloat m_radiusAroundStartMod;

	public AbilityModPropertyFloat m_radiusAroundEndMod;

	public AbilityModPropertyBool m_chargePenetrateLosMod;

	[Header("-- Hit Damage and Effect")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	public AbilityModPropertyInt m_allyHealAmountMod;

	public AbilityModPropertyEffectInfo m_allyHitEffectMod;

	[Header("-- Shield Barrier (Barrier Data specified on Passive)")]
	public AbilityModPropertyBool m_allowAimAtDiagonalsMod;

	[Header("-- Cooldown by distance, [ Cooldown = Max(minCooldown, distance+cooldownModifierAdd) ], add modifier can be negative")]
	public AbilityModPropertyBool m_setCooldownByDistanceMod;

	public AbilityModPropertyInt m_minCooldownMod;

	public AbilityModPropertyInt m_cooldownModifierAddMod;

	[Header("-- Distance by Energy")]
	public AbilityModPropertyBool m_useEnergyForMoveDistanceMod;

	public AbilityModPropertyInt m_minEnergyToCastMod;

	public AbilityModPropertyInt m_energyPerMoveMod;

	public AbilityModPropertyBool m_useAllEnergyIfUsedForDistanceMod;

	[Header("-- For Hitting In Front of Shield")]
	public AbilityModPropertyBool m_hitInFrontOfShieldMod;

	public AbilityModPropertyFloat m_shieldFrontHitLengthMod;

	public AbilityModPropertyInt m_damageForShieldFrontMod;

	public AbilityModPropertyEffectInfo m_shieldFrontEnemyEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(RampartDashAndAimShield);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RampartDashAndAimShield rampartDashAndAimShield = targetAbility as RampartDashAndAimShield;
		if (!(rampartDashAndAimShield != null))
		{
			return;
		}
		AbilityMod.AddToken(tokens, m_chargeRadiusMod, "ChargeRadius", string.Empty, rampartDashAndAimShield.m_chargeRadius);
		AbilityMod.AddToken(tokens, m_radiusAroundStartMod, "RadiusAroundStart", string.Empty, rampartDashAndAimShield.m_radiusAroundStart);
		AbilityMod.AddToken(tokens, m_radiusAroundEndMod, "RadiusAroundEnd", string.Empty, rampartDashAndAimShield.m_radiusAroundEnd);
		AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, rampartDashAndAimShield.m_damageAmount);
		AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", rampartDashAndAimShield.m_enemyHitEffect);
		AbilityMod.AddToken(tokens, m_allyHealAmountMod, "AllyHealAmount", string.Empty, rampartDashAndAimShield.m_allyHealAmount);
		AbilityMod.AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", rampartDashAndAimShield.m_allyHitEffect);
		AbilityMod.AddToken(tokens, m_minCooldownMod, "MinCooldown", string.Empty, rampartDashAndAimShield.m_minCooldown);
		AbilityMod.AddToken(tokens, m_cooldownModifierAddMod, "CooldownModifierAdd", string.Empty, rampartDashAndAimShield.m_cooldownModifierAdd);
		AbilityMod.AddToken(tokens, m_minEnergyToCastMod, "MinEnergyToCast", string.Empty, rampartDashAndAimShield.m_minEnergyToCast);
		AbilityMod.AddToken(tokens, m_energyPerMoveMod, "EnergyPerMove", string.Empty, rampartDashAndAimShield.m_energyPerMove);
		AbilityMod.AddToken(tokens, m_shieldFrontHitLengthMod, "ShieldFrontHitLength", string.Empty, rampartDashAndAimShield.m_shieldFrontHitLength);
		AbilityMod.AddToken(tokens, m_damageForShieldFrontMod, "DamageForShieldFront", string.Empty, rampartDashAndAimShield.m_damageForShieldFront);
		AbilityMod.AddToken_EffectMod(tokens, m_shieldFrontEnemyEffectMod, "ShieldFrontEnemyEffect", rampartDashAndAimShield.m_shieldFrontEnemyEffect);
		if (m_useTargetDataOverrides)
		{
			if (m_targetDataOverrides != null)
			{
				if (m_targetDataOverrides.Length > 0)
				{
					if (rampartDashAndAimShield.m_targetData.Length > 0)
					{
						AbilityMod.AddToken_IntDiff(tokens, "TargeterRange", string.Empty, Mathf.RoundToInt(m_targetDataOverrides[0].m_range), true, Mathf.RoundToInt(rampartDashAndAimShield.m_targetData[0].m_range));
					}
				}
			}
		}
		if (m_statModsWhileEquipped == null)
		{
			return;
		}
		while (true)
		{
			if (m_statModsWhileEquipped.Length <= 0)
			{
				return;
			}
			while (true)
			{
				for (int i = 0; i < m_statModsWhileEquipped.Length; i++)
				{
					AbilityStatMod abilityStatMod = m_statModsWhileEquipped[i];
					AbilityMod.AddToken_IntDiff(tokens, "StatMod_" + abilityStatMod.stat, string.Empty, Mathf.RoundToInt(abilityStatMod.modValue), false, 0);
				}
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartDashAndAimShield rampartDashAndAimShield = GetTargetAbilityOnAbilityData(abilityData) as RampartDashAndAimShield;
		bool flag = rampartDashAndAimShield != null;
		string empty = string.Empty;
		empty += PropDesc(m_chargeRadiusMod, "[ChargeRadius]", flag, (!flag) ? 0f : rampartDashAndAimShield.m_chargeRadius);
		string str = empty;
		AbilityModPropertyFloat radiusAroundStartMod = m_radiusAroundStartMod;
		float baseVal;
		if (flag)
		{
			baseVal = rampartDashAndAimShield.m_radiusAroundStart;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(radiusAroundStartMod, "[RadiusAroundStart]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat radiusAroundEndMod = m_radiusAroundEndMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = rampartDashAndAimShield.m_radiusAroundEnd;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(radiusAroundEndMod, "[RadiusAroundEnd]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyBool chargePenetrateLosMod = m_chargePenetrateLosMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = (rampartDashAndAimShield.m_chargePenetrateLos ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(chargePenetrateLosMod, "[ChargePenetrateLos]", flag, (byte)baseVal3 != 0);
		string str4 = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = rampartDashAndAimShield.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal5;
		if (flag)
		{
			baseVal5 = rampartDashAndAimShield.m_enemyHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyInt allyHealAmountMod = m_allyHealAmountMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = rampartDashAndAimShield.m_allyHealAmount;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(allyHealAmountMod, "[AllyHealAmount]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectInfo allyHitEffectMod = m_allyHitEffectMod;
		object baseVal7;
		if (flag)
		{
			baseVal7 = rampartDashAndAimShield.m_allyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(allyHitEffectMod, "[AllyHitEffect]", flag, (StandardEffectInfo)baseVal7);
		string str8 = empty;
		AbilityModPropertyBool allowAimAtDiagonalsMod = m_allowAimAtDiagonalsMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = (rampartDashAndAimShield.m_allowAimAtDiagonals ? 1 : 0);
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(allowAimAtDiagonalsMod, "[AllowAimAtDiagonals]", flag, (byte)baseVal8 != 0);
		string str9 = empty;
		AbilityModPropertyBool setCooldownByDistanceMod = m_setCooldownByDistanceMod;
		int baseVal9;
		if (flag)
		{
			baseVal9 = (rampartDashAndAimShield.m_setCooldownByDistance ? 1 : 0);
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(setCooldownByDistanceMod, "[SetCooldownByDistance]", flag, (byte)baseVal9 != 0);
		string str10 = empty;
		AbilityModPropertyInt minCooldownMod = m_minCooldownMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = rampartDashAndAimShield.m_minCooldown;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(minCooldownMod, "[MinCooldown]", flag, baseVal10);
		empty += PropDesc(m_cooldownModifierAddMod, "[CooldownModifierAdd]", flag, flag ? rampartDashAndAimShield.m_cooldownModifierAdd : 0);
		string str11 = empty;
		AbilityModPropertyBool useEnergyForMoveDistanceMod = m_useEnergyForMoveDistanceMod;
		int baseVal11;
		if (flag)
		{
			baseVal11 = (rampartDashAndAimShield.m_useEnergyForMoveDistance ? 1 : 0);
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str11 + PropDesc(useEnergyForMoveDistanceMod, "[UseEnergyForMoveDistance]", flag, (byte)baseVal11 != 0);
		string str12 = empty;
		AbilityModPropertyInt minEnergyToCastMod = m_minEnergyToCastMod;
		int baseVal12;
		if (flag)
		{
			baseVal12 = rampartDashAndAimShield.m_minEnergyToCast;
		}
		else
		{
			baseVal12 = 0;
		}
		empty = str12 + PropDesc(minEnergyToCastMod, "[MinEnergyToCast]", flag, baseVal12);
		empty += PropDesc(m_energyPerMoveMod, "[EnergyPerMove]", flag, flag ? rampartDashAndAimShield.m_energyPerMove : 0);
		string str13 = empty;
		AbilityModPropertyBool useAllEnergyIfUsedForDistanceMod = m_useAllEnergyIfUsedForDistanceMod;
		int baseVal13;
		if (flag)
		{
			baseVal13 = (rampartDashAndAimShield.m_useAllEnergyIfUsedForDistance ? 1 : 0);
		}
		else
		{
			baseVal13 = 0;
		}
		empty = str13 + PropDesc(useAllEnergyIfUsedForDistanceMod, "[UseAllEnergyIfUsedForDistance]", flag, (byte)baseVal13 != 0);
		string str14 = empty;
		AbilityModPropertyBool hitInFrontOfShieldMod = m_hitInFrontOfShieldMod;
		int baseVal14;
		if (flag)
		{
			baseVal14 = (rampartDashAndAimShield.m_hitInFrontOfShield ? 1 : 0);
		}
		else
		{
			baseVal14 = 0;
		}
		empty = str14 + PropDesc(hitInFrontOfShieldMod, "[HitInFrontOfShield]", flag, (byte)baseVal14 != 0);
		int num;
		if (m_hitInFrontOfShieldMod != null)
		{
			AbilityModPropertyBool hitInFrontOfShieldMod2 = m_hitInFrontOfShieldMod;
			int input;
			if (flag)
			{
				input = (rampartDashAndAimShield.m_hitInFrontOfShield ? 1 : 0);
			}
			else
			{
				input = 0;
			}
			num = (hitInFrontOfShieldMod2.GetModifiedValue((byte)input != 0) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		if (num != 0)
		{
			string str15 = empty;
			AbilityModPropertyFloat shieldFrontHitLengthMod = m_shieldFrontHitLengthMod;
			float baseVal15;
			if (flag)
			{
				baseVal15 = rampartDashAndAimShield.m_shieldFrontHitLength;
			}
			else
			{
				baseVal15 = 0f;
			}
			empty = str15 + PropDesc(shieldFrontHitLengthMod, "[ShieldFrontHitLength]", flag, baseVal15);
			string str16 = empty;
			AbilityModPropertyInt damageForShieldFrontMod = m_damageForShieldFrontMod;
			int baseVal16;
			if (flag)
			{
				baseVal16 = rampartDashAndAimShield.m_damageForShieldFront;
			}
			else
			{
				baseVal16 = 0;
			}
			empty = str16 + PropDesc(damageForShieldFrontMod, "[DamageForShieldFront]", flag, baseVal16);
			string str17 = empty;
			AbilityModPropertyEffectInfo shieldFrontEnemyEffectMod = m_shieldFrontEnemyEffectMod;
			object baseVal17;
			if (flag)
			{
				baseVal17 = rampartDashAndAimShield.m_shieldFrontEnemyEffect;
			}
			else
			{
				baseVal17 = null;
			}
			empty = str17 + PropDesc(shieldFrontEnemyEffectMod, "[ShieldFrontEnemyEffect]", flag, (StandardEffectInfo)baseVal17);
		}
		return empty;
	}
}
