﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class SpaceMarineDropPod : Ability
{
	public AbilityAreaShape m_powerupShape = AbilityAreaShape.Two_x_Two;

	public int m_damageAmount = 5;

	public bool m_penetrateLoS;

	[Header("-- Knockback")]
	public AbilityAreaShape m_knockbackShape = AbilityAreaShape.Four_x_Four_NoCorners;

	public float m_knockbackDistance = 3f;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	[Header("-- Energy Refund if no hit")]
	public int m_energyRefundIfNoEnemyHit;

	public bool m_energyRefundAffectedByBuff;

	[Header("-- Powerups Spawn --")]
	public PowerUp m_powerupPrefab;

	public int m_numPowerupToSpawn = 4;

	public int m_powerupDuration;

	public bool m_canSpawnOnEnemyOccupiedSquares = true;

	public bool m_canSpawnOnAllyOccupiedSquares;

	[Space(10f)]
	public int m_extraPowerupHealIfDirectHit;

	public int m_extraPowerupEnergyIfDirectHit;

	private AbilityMod_SpaceMarineDropPod m_abilityMod;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Drop Pod";
		}
		AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape;
		AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
		AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Possible;
		base.Targeter = new AbilityUtil_Targeter_KnockbackAoE(this, this.m_knockbackShape, this.m_penetrateLoS, damageOriginType, true, false, affectsCaster, affectsBestTarget, this.m_knockbackDistance, this.m_knockbackType);
	}

	public int ModdedDamage()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_damageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damageAmount);
		}
		return result;
	}

	public float ModdedKnockbackDistance()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_knockbackDistance;
		}
		else
		{
			result = this.m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(this.m_knockbackDistance);
		}
		return result;
	}

	public int GetEnergyRefundIfNoEnemyHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_energyRefundIfNoEnemyHitMod.GetModifiedValue(this.m_energyRefundIfNoEnemyHit);
		}
		else
		{
			result = this.m_energyRefundIfNoEnemyHit;
		}
		return result;
	}

	public int GetExtraPowerupHealIfDirectHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraPowerupHealIfDirectHitMod.GetModifiedValue(this.m_extraPowerupHealIfDirectHit);
		}
		else
		{
			result = this.m_extraPowerupHealIfDirectHit;
		}
		return result;
	}

	public int GetExtraPowerupEnergyIfDirectHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraPowerupEnergyIfDirectHitMod.GetModifiedValue(this.m_extraPowerupEnergyIfDirectHit);
		}
		else
		{
			result = this.m_extraPowerupEnergyIfDirectHit;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.ModdedDamage())
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_groundEffectInfoOnDropPod.m_applyGroundEffect)
			{
				if (this.m_abilityMod.m_groundEffectInfoOnDropPod.m_groundEffectData.damageAmount > 0)
				{
					List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
					BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeter.LastUpdatingGridPos);
					if (tooltipSubjectTypes != null && boardSquareSafe != null)
					{
						if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
						{
							GroundEffectField groundEffectData = this.m_abilityMod.m_groundEffectInfoOnDropPod.m_groundEffectData;
							if (AreaEffectUtils.IsSquareInShape(targetActor.GetCurrentBoardSquare(), groundEffectData.shape, base.Targeter.LastUpdateFreePos, boardSquareSafe, this.m_penetrateLoS, base.ActorData))
							{
								dictionary = new Dictionary<AbilityTooltipSymbol, int>();
								dictionary[AbilityTooltipSymbol.Damage] = this.ModdedDamage() + groundEffectData.damageAmount;
							}
						}
					}
				}
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.GetEnergyRefundIfNoEnemyHit() > 0)
		{
			if (base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy) == 0)
			{
				return this.GetEnergyRefundIfNoEnemyHit();
			}
		}
		return 0;
	}

	public override bool StatusAdjustAdditionalTechPointForTargeting()
	{
		return this.m_energyRefundAffectedByBuff;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SpaceMarineDropPod abilityMod_SpaceMarineDropPod = modAsBase as AbilityMod_SpaceMarineDropPod;
		string name = "DamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_SpaceMarineDropPod)
		{
			val = abilityMod_SpaceMarineDropPod.m_damageMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			val = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "EnergyRefundIfNoEnemyHit";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_SpaceMarineDropPod)
		{
			val2 = abilityMod_SpaceMarineDropPod.m_energyRefundIfNoEnemyHitMod.GetModifiedValue(this.m_energyRefundIfNoEnemyHit);
		}
		else
		{
			val2 = this.m_energyRefundIfNoEnemyHit;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		base.AddTokenInt(tokens, "NumPowerupToSpawn", string.Empty, this.m_numPowerupToSpawn, false);
		base.AddTokenInt(tokens, "PowerupDuration", string.Empty, this.m_powerupDuration, false);
		string name3 = "ExtraPowerupHealIfDirectHit";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_SpaceMarineDropPod)
		{
			val3 = abilityMod_SpaceMarineDropPod.m_extraPowerupHealIfDirectHitMod.GetModifiedValue(this.m_extraPowerupHealIfDirectHit);
		}
		else
		{
			val3 = this.m_extraPowerupHealIfDirectHit;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "ExtraPowerupEnergy";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_SpaceMarineDropPod)
		{
			val4 = abilityMod_SpaceMarineDropPod.m_extraPowerupEnergyIfDirectHitMod.GetModifiedValue(this.m_extraPowerupEnergyIfDirectHit);
		}
		else
		{
			val4 = this.m_extraPowerupEnergyIfDirectHit;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		if (this.m_powerupPrefab != null)
		{
			if (this.m_powerupPrefab.m_ability != null)
			{
				PowerUp_Standard_Ability powerUp_Standard_Ability = this.m_powerupPrefab.m_ability as PowerUp_Standard_Ability;
				if (powerUp_Standard_Ability != null)
				{
					base.AddTokenInt(tokens, "PowerupHealing", string.Empty, powerUp_Standard_Ability.m_healAmount, false);
				}
			}
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SpaceMarineDropPod))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SpaceMarineDropPod);
			AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape;
			AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
			AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Possible;
			base.Targeter = new AbilityUtil_Targeter_KnockbackAoE(this, this.m_knockbackShape, this.m_penetrateLoS, damageOriginType, true, false, affectsCaster, affectsBestTarget, this.ModdedKnockbackDistance(), this.m_knockbackType);
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape;
		AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
		AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Possible;
		base.Targeter = new AbilityUtil_Targeter_KnockbackAoE(this, this.m_knockbackShape, this.m_penetrateLoS, damageOriginType, true, false, affectsCaster, affectsBestTarget, this.m_knockbackDistance, this.m_knockbackType);
	}
}
