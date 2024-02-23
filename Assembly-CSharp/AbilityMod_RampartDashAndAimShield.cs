using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken(tokens, m_chargeRadiusMod, "ChargeRadius", "", rampartDashAndAimShield.m_chargeRadius);
			AddToken(tokens, m_radiusAroundStartMod, "RadiusAroundStart", "", rampartDashAndAimShield.m_radiusAroundStart);
			AddToken(tokens, m_radiusAroundEndMod, "RadiusAroundEnd", "", rampartDashAndAimShield.m_radiusAroundEnd);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", "", rampartDashAndAimShield.m_damageAmount);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", rampartDashAndAimShield.m_enemyHitEffect);
			AddToken(tokens, m_allyHealAmountMod, "AllyHealAmount", "", rampartDashAndAimShield.m_allyHealAmount);
			AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", rampartDashAndAimShield.m_allyHitEffect);
			AddToken(tokens, m_minCooldownMod, "MinCooldown", "", rampartDashAndAimShield.m_minCooldown);
			AddToken(tokens, m_cooldownModifierAddMod, "CooldownModifierAdd", "", rampartDashAndAimShield.m_cooldownModifierAdd);
			AddToken(tokens, m_minEnergyToCastMod, "MinEnergyToCast", "", rampartDashAndAimShield.m_minEnergyToCast);
			AddToken(tokens, m_energyPerMoveMod, "EnergyPerMove", "", rampartDashAndAimShield.m_energyPerMove);
			AddToken(tokens, m_shieldFrontHitLengthMod, "ShieldFrontHitLength", "", rampartDashAndAimShield.m_shieldFrontHitLength);
			AddToken(tokens, m_damageForShieldFrontMod, "DamageForShieldFront", "", rampartDashAndAimShield.m_damageForShieldFront);
			AddToken_EffectMod(tokens, m_shieldFrontEnemyEffectMod, "ShieldFrontEnemyEffect", rampartDashAndAimShield.m_shieldFrontEnemyEffect);
			if (m_useTargetDataOverrides
				&& m_targetDataOverrides != null
				&& m_targetDataOverrides.Length > 0
				&& rampartDashAndAimShield.m_targetData.Length > 0)
			{
				AddToken_IntDiff(tokens, "TargeterRange", "", Mathf.RoundToInt(m_targetDataOverrides[0].m_range), true, Mathf.RoundToInt(rampartDashAndAimShield.m_targetData[0].m_range));
			}
			if (m_statModsWhileEquipped != null && m_statModsWhileEquipped.Length > 0)
			{
				foreach (AbilityStatMod abilityStatMod in m_statModsWhileEquipped)
				{
					AddToken_IntDiff(tokens, new StringBuilder().Append("StatMod_").Append(abilityStatMod.stat).ToString(), "", Mathf.RoundToInt(abilityStatMod.modValue), false, 0);
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartDashAndAimShield rampartDashAndAimShield = GetTargetAbilityOnAbilityData(abilityData) as RampartDashAndAimShield;
		bool isAbilityPresent = rampartDashAndAimShield != null;
		string desc = "";
		desc += PropDesc(m_chargeRadiusMod, "[ChargeRadius]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_chargeRadius : 0f);
		desc += PropDesc(m_radiusAroundStartMod, "[RadiusAroundStart]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_radiusAroundStart : 0f);
		desc += PropDesc(m_radiusAroundEndMod, "[RadiusAroundEnd]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_radiusAroundEnd : 0f);
		desc += PropDesc(m_chargePenetrateLosMod, "[ChargePenetrateLos]", isAbilityPresent, isAbilityPresent && rampartDashAndAimShield.m_chargePenetrateLos);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_damageAmount : 0);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_enemyHitEffect : null);
		desc += PropDesc(m_allyHealAmountMod, "[AllyHealAmount]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_allyHealAmount : 0);
		desc += PropDesc(m_allyHitEffectMod, "[AllyHitEffect]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_allyHitEffect : null);
		desc += PropDesc(m_allowAimAtDiagonalsMod, "[AllowAimAtDiagonals]", isAbilityPresent, isAbilityPresent && rampartDashAndAimShield.m_allowAimAtDiagonals);
		desc += PropDesc(m_setCooldownByDistanceMod, "[SetCooldownByDistance]", isAbilityPresent, isAbilityPresent && rampartDashAndAimShield.m_setCooldownByDistance);
		desc += PropDesc(m_minCooldownMod, "[MinCooldown]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_minCooldown : 0);
		desc += PropDesc(m_cooldownModifierAddMod, "[CooldownModifierAdd]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_cooldownModifierAdd : 0);
		desc += PropDesc(m_useEnergyForMoveDistanceMod, "[UseEnergyForMoveDistance]", isAbilityPresent, isAbilityPresent && rampartDashAndAimShield.m_useEnergyForMoveDistance);
		desc += PropDesc(m_minEnergyToCastMod, "[MinEnergyToCast]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_minEnergyToCast : 0);
		desc += PropDesc(m_energyPerMoveMod, "[EnergyPerMove]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_energyPerMove : 0);
		desc += PropDesc(m_useAllEnergyIfUsedForDistanceMod, "[UseAllEnergyIfUsedForDistance]", isAbilityPresent, isAbilityPresent && rampartDashAndAimShield.m_useAllEnergyIfUsedForDistance);
		desc += PropDesc(m_hitInFrontOfShieldMod, "[HitInFrontOfShield]", isAbilityPresent, isAbilityPresent && rampartDashAndAimShield.m_hitInFrontOfShield);
		if (m_hitInFrontOfShieldMod != null
			&& m_hitInFrontOfShieldMod.GetModifiedValue(isAbilityPresent && rampartDashAndAimShield.m_hitInFrontOfShield))
		{
			desc += PropDesc(m_shieldFrontHitLengthMod, "[ShieldFrontHitLength]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_shieldFrontHitLength : 0f);
			desc += PropDesc(m_damageForShieldFrontMod, "[DamageForShieldFront]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_damageForShieldFront : 0);
			desc += PropDesc(m_shieldFrontEnemyEffectMod, "[ShieldFrontEnemyEffect]", isAbilityPresent, isAbilityPresent ? rampartDashAndAimShield.m_shieldFrontEnemyEffect : null);
		}
		return desc;
	}
}
