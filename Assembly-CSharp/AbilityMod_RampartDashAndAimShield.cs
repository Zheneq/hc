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
		if (rampartDashAndAimShield != null)
		{
			AbilityMod.AddToken(tokens, this.m_chargeRadiusMod, "ChargeRadius", string.Empty, rampartDashAndAimShield.m_chargeRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_radiusAroundStartMod, "RadiusAroundStart", string.Empty, rampartDashAndAimShield.m_radiusAroundStart, true, false, false);
			AbilityMod.AddToken(tokens, this.m_radiusAroundEndMod, "RadiusAroundEnd", string.Empty, rampartDashAndAimShield.m_radiusAroundEnd, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, rampartDashAndAimShield.m_damageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", rampartDashAndAimShield.m_enemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_allyHealAmountMod, "AllyHealAmount", string.Empty, rampartDashAndAimShield.m_allyHealAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyHitEffectMod, "AllyHitEffect", rampartDashAndAimShield.m_allyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_minCooldownMod, "MinCooldown", string.Empty, rampartDashAndAimShield.m_minCooldown, true, false);
			AbilityMod.AddToken(tokens, this.m_cooldownModifierAddMod, "CooldownModifierAdd", string.Empty, rampartDashAndAimShield.m_cooldownModifierAdd, true, false);
			AbilityMod.AddToken(tokens, this.m_minEnergyToCastMod, "MinEnergyToCast", string.Empty, rampartDashAndAimShield.m_minEnergyToCast, true, false);
			AbilityMod.AddToken(tokens, this.m_energyPerMoveMod, "EnergyPerMove", string.Empty, rampartDashAndAimShield.m_energyPerMove, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldFrontHitLengthMod, "ShieldFrontHitLength", string.Empty, rampartDashAndAimShield.m_shieldFrontHitLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageForShieldFrontMod, "DamageForShieldFront", string.Empty, rampartDashAndAimShield.m_damageForShieldFront, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_shieldFrontEnemyEffectMod, "ShieldFrontEnemyEffect", rampartDashAndAimShield.m_shieldFrontEnemyEffect, true);
			if (this.m_useTargetDataOverrides)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RampartDashAndAimShield.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
				}
				if (this.m_targetDataOverrides != null)
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
					if (this.m_targetDataOverrides.Length > 0)
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
						if (rampartDashAndAimShield.m_targetData.Length > 0)
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
							AbilityMod.AddToken_IntDiff(tokens, "TargeterRange", string.Empty, Mathf.RoundToInt(this.m_targetDataOverrides[0].m_range), true, Mathf.RoundToInt(rampartDashAndAimShield.m_targetData[0].m_range));
						}
					}
				}
			}
			if (this.m_statModsWhileEquipped != null)
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
				if (this.m_statModsWhileEquipped.Length > 0)
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
					for (int i = 0; i < this.m_statModsWhileEquipped.Length; i++)
					{
						AbilityStatMod abilityStatMod = this.m_statModsWhileEquipped[i];
						AbilityMod.AddToken_IntDiff(tokens, "StatMod_" + abilityStatMod.stat.ToString(), string.Empty, Mathf.RoundToInt(abilityStatMod.modValue), false, 0);
					}
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartDashAndAimShield rampartDashAndAimShield = base.GetTargetAbilityOnAbilityData(abilityData) as RampartDashAndAimShield;
		bool flag = rampartDashAndAimShield != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_chargeRadiusMod, "[ChargeRadius]", flag, (!flag) ? 0f : rampartDashAndAimShield.m_chargeRadius);
		string str = text;
		AbilityModPropertyFloat radiusAroundStartMod = this.m_radiusAroundStartMod;
		string prefix = "[RadiusAroundStart]";
		bool showBaseVal = flag;
		float baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RampartDashAndAimShield.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = rampartDashAndAimShield.m_radiusAroundStart;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(radiusAroundStartMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat radiusAroundEndMod = this.m_radiusAroundEndMod;
		string prefix2 = "[RadiusAroundEnd]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = rampartDashAndAimShield.m_radiusAroundEnd;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(radiusAroundEndMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool chargePenetrateLosMod = this.m_chargePenetrateLosMod;
		string prefix3 = "[ChargePenetrateLos]";
		bool showBaseVal3 = flag;
		bool baseVal3;
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
			baseVal3 = rampartDashAndAimShield.m_chargePenetrateLos;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(chargePenetrateLosMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix4 = "[DamageAmount]";
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
			baseVal4 = rampartDashAndAimShield.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(damageAmountMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix5 = "[EnemyHitEffect]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal5;
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
			baseVal5 = rampartDashAndAimShield.m_enemyHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(enemyHitEffectMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt allyHealAmountMod = this.m_allyHealAmountMod;
		string prefix6 = "[AllyHealAmount]";
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
			baseVal6 = rampartDashAndAimShield.m_allyHealAmount;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(allyHealAmountMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectInfo allyHitEffectMod = this.m_allyHitEffectMod;
		string prefix7 = "[AllyHitEffect]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
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
			baseVal7 = rampartDashAndAimShield.m_allyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(allyHitEffectMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyBool allowAimAtDiagonalsMod = this.m_allowAimAtDiagonalsMod;
		string prefix8 = "[AllowAimAtDiagonals]";
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
			baseVal8 = rampartDashAndAimShield.m_allowAimAtDiagonals;
		}
		else
		{
			baseVal8 = false;
		}
		text = str8 + base.PropDesc(allowAimAtDiagonalsMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyBool setCooldownByDistanceMod = this.m_setCooldownByDistanceMod;
		string prefix9 = "[SetCooldownByDistance]";
		bool showBaseVal9 = flag;
		bool baseVal9;
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
			baseVal9 = rampartDashAndAimShield.m_setCooldownByDistance;
		}
		else
		{
			baseVal9 = false;
		}
		text = str9 + base.PropDesc(setCooldownByDistanceMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt minCooldownMod = this.m_minCooldownMod;
		string prefix10 = "[MinCooldown]";
		bool showBaseVal10 = flag;
		int baseVal10;
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
			baseVal10 = rampartDashAndAimShield.m_minCooldown;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(minCooldownMod, prefix10, showBaseVal10, baseVal10);
		text += base.PropDesc(this.m_cooldownModifierAddMod, "[CooldownModifierAdd]", flag, (!flag) ? 0 : rampartDashAndAimShield.m_cooldownModifierAdd);
		string str11 = text;
		AbilityModPropertyBool useEnergyForMoveDistanceMod = this.m_useEnergyForMoveDistanceMod;
		string prefix11 = "[UseEnergyForMoveDistance]";
		bool showBaseVal11 = flag;
		bool baseVal11;
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
			baseVal11 = rampartDashAndAimShield.m_useEnergyForMoveDistance;
		}
		else
		{
			baseVal11 = false;
		}
		text = str11 + base.PropDesc(useEnergyForMoveDistanceMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyInt minEnergyToCastMod = this.m_minEnergyToCastMod;
		string prefix12 = "[MinEnergyToCast]";
		bool showBaseVal12 = flag;
		int baseVal12;
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
			baseVal12 = rampartDashAndAimShield.m_minEnergyToCast;
		}
		else
		{
			baseVal12 = 0;
		}
		text = str12 + base.PropDesc(minEnergyToCastMod, prefix12, showBaseVal12, baseVal12);
		text += base.PropDesc(this.m_energyPerMoveMod, "[EnergyPerMove]", flag, (!flag) ? 0 : rampartDashAndAimShield.m_energyPerMove);
		string str13 = text;
		AbilityModPropertyBool useAllEnergyIfUsedForDistanceMod = this.m_useAllEnergyIfUsedForDistanceMod;
		string prefix13 = "[UseAllEnergyIfUsedForDistance]";
		bool showBaseVal13 = flag;
		bool baseVal13;
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
			baseVal13 = rampartDashAndAimShield.m_useAllEnergyIfUsedForDistance;
		}
		else
		{
			baseVal13 = false;
		}
		text = str13 + base.PropDesc(useAllEnergyIfUsedForDistanceMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyBool hitInFrontOfShieldMod = this.m_hitInFrontOfShieldMod;
		string prefix14 = "[HitInFrontOfShield]";
		bool showBaseVal14 = flag;
		bool baseVal14;
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
			baseVal14 = rampartDashAndAimShield.m_hitInFrontOfShield;
		}
		else
		{
			baseVal14 = false;
		}
		text = str14 + base.PropDesc(hitInFrontOfShieldMod, prefix14, showBaseVal14, baseVal14);
		bool flag2;
		if (this.m_hitInFrontOfShieldMod != null)
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
			AbilityModPropertyBool hitInFrontOfShieldMod2 = this.m_hitInFrontOfShieldMod;
			bool input;
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
				input = rampartDashAndAimShield.m_hitInFrontOfShield;
			}
			else
			{
				input = false;
			}
			flag2 = hitInFrontOfShieldMod2.GetModifiedValue(input);
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		if (flag3)
		{
			string str15 = text;
			AbilityModPropertyFloat shieldFrontHitLengthMod = this.m_shieldFrontHitLengthMod;
			string prefix15 = "[ShieldFrontHitLength]";
			bool showBaseVal15 = flag;
			float baseVal15;
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
				baseVal15 = rampartDashAndAimShield.m_shieldFrontHitLength;
			}
			else
			{
				baseVal15 = 0f;
			}
			text = str15 + base.PropDesc(shieldFrontHitLengthMod, prefix15, showBaseVal15, baseVal15);
			string str16 = text;
			AbilityModPropertyInt damageForShieldFrontMod = this.m_damageForShieldFrontMod;
			string prefix16 = "[DamageForShieldFront]";
			bool showBaseVal16 = flag;
			int baseVal16;
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
				baseVal16 = rampartDashAndAimShield.m_damageForShieldFront;
			}
			else
			{
				baseVal16 = 0;
			}
			text = str16 + base.PropDesc(damageForShieldFrontMod, prefix16, showBaseVal16, baseVal16);
			string str17 = text;
			AbilityModPropertyEffectInfo shieldFrontEnemyEffectMod = this.m_shieldFrontEnemyEffectMod;
			string prefix17 = "[ShieldFrontEnemyEffect]";
			bool showBaseVal17 = flag;
			StandardEffectInfo baseVal17;
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
				baseVal17 = rampartDashAndAimShield.m_shieldFrontEnemyEffect;
			}
			else
			{
				baseVal17 = null;
			}
			text = str17 + base.PropDesc(shieldFrontEnemyEffectMod, prefix17, showBaseVal17, baseVal17);
		}
		return text;
	}
}
