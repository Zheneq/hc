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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Drop Pod";
		}
		AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape;
		AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
		AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Possible;
		base.Targeter = new AbilityUtil_Targeter_KnockbackAoE(this, m_knockbackShape, m_penetrateLoS, damageOriginType, true, false, affectsCaster, affectsBestTarget, m_knockbackDistance, m_knockbackType);
	}

	public int ModdedDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_damageAmount;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount);
		}
		return result;
	}

	public float ModdedKnockbackDistance()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_knockbackDistance;
		}
		else
		{
			result = m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance);
		}
		return result;
	}

	public int GetEnergyRefundIfNoEnemyHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_energyRefundIfNoEnemyHitMod.GetModifiedValue(m_energyRefundIfNoEnemyHit);
		}
		else
		{
			result = m_energyRefundIfNoEnemyHit;
		}
		return result;
	}

	public int GetExtraPowerupHealIfDirectHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraPowerupHealIfDirectHitMod.GetModifiedValue(m_extraPowerupHealIfDirectHit);
		}
		else
		{
			result = m_extraPowerupHealIfDirectHit;
		}
		return result;
	}

	public int GetExtraPowerupEnergyIfDirectHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraPowerupEnergyIfDirectHitMod.GetModifiedValue(m_extraPowerupEnergyIfDirectHit);
		}
		else
		{
			result = m_extraPowerupEnergyIfDirectHit;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, ModdedDamage()));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_groundEffectInfoOnDropPod.m_applyGroundEffect)
			{
				if (m_abilityMod.m_groundEffectInfoOnDropPod.m_groundEffectData.damageAmount > 0)
				{
					List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
					BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeter.LastUpdatingGridPos);
					if (tooltipSubjectTypes != null && boardSquareSafe != null)
					{
						if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
						{
							GroundEffectField groundEffectData = m_abilityMod.m_groundEffectInfoOnDropPod.m_groundEffectData;
							if (AreaEffectUtils.IsSquareInShape(targetActor.GetCurrentBoardSquare(), groundEffectData.shape, base.Targeter.LastUpdateFreePos, boardSquareSafe, m_penetrateLoS, base.ActorData))
							{
								dictionary = new Dictionary<AbilityTooltipSymbol, int>();
								dictionary[AbilityTooltipSymbol.Damage] = ModdedDamage() + groundEffectData.damageAmount;
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
		if (GetEnergyRefundIfNoEnemyHit() > 0)
		{
			if (base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy) == 0)
			{
				return GetEnergyRefundIfNoEnemyHit();
			}
		}
		return 0;
	}

	public override bool StatusAdjustAdditionalTechPointForTargeting()
	{
		return m_energyRefundAffectedByBuff;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SpaceMarineDropPod abilityMod_SpaceMarineDropPod = modAsBase as AbilityMod_SpaceMarineDropPod;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_SpaceMarineDropPod)
		{
			val = abilityMod_SpaceMarineDropPod.m_damageMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			val = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_SpaceMarineDropPod)
		{
			val2 = abilityMod_SpaceMarineDropPod.m_energyRefundIfNoEnemyHitMod.GetModifiedValue(m_energyRefundIfNoEnemyHit);
		}
		else
		{
			val2 = m_energyRefundIfNoEnemyHit;
		}
		AddTokenInt(tokens, "EnergyRefundIfNoEnemyHit", empty2, val2);
		AddTokenInt(tokens, "NumPowerupToSpawn", string.Empty, m_numPowerupToSpawn);
		AddTokenInt(tokens, "PowerupDuration", string.Empty, m_powerupDuration);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_SpaceMarineDropPod)
		{
			val3 = abilityMod_SpaceMarineDropPod.m_extraPowerupHealIfDirectHitMod.GetModifiedValue(m_extraPowerupHealIfDirectHit);
		}
		else
		{
			val3 = m_extraPowerupHealIfDirectHit;
		}
		AddTokenInt(tokens, "ExtraPowerupHealIfDirectHit", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_SpaceMarineDropPod)
		{
			val4 = abilityMod_SpaceMarineDropPod.m_extraPowerupEnergyIfDirectHitMod.GetModifiedValue(m_extraPowerupEnergyIfDirectHit);
		}
		else
		{
			val4 = m_extraPowerupEnergyIfDirectHit;
		}
		AddTokenInt(tokens, "ExtraPowerupEnergy", empty4, val4);
		if (!(m_powerupPrefab != null))
		{
			return;
		}
		while (true)
		{
			if (!(m_powerupPrefab.m_ability != null))
			{
				return;
			}
			while (true)
			{
				PowerUp_Standard_Ability powerUp_Standard_Ability = m_powerupPrefab.m_ability as PowerUp_Standard_Ability;
				if (powerUp_Standard_Ability != null)
				{
					while (true)
					{
						AddTokenInt(tokens, "PowerupHealing", string.Empty, powerUp_Standard_Ability.m_healAmount);
						return;
					}
				}
				return;
			}
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SpaceMarineDropPod))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					m_abilityMod = (abilityMod as AbilityMod_SpaceMarineDropPod);
					AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape;
					AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
					AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Possible;
					base.Targeter = new AbilityUtil_Targeter_KnockbackAoE(this, m_knockbackShape, m_penetrateLoS, damageOriginType, true, false, affectsCaster, affectsBestTarget, ModdedKnockbackDistance(), m_knockbackType);
					return;
				}
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape;
		AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
		AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Possible;
		base.Targeter = new AbilityUtil_Targeter_KnockbackAoE(this, m_knockbackShape, m_penetrateLoS, damageOriginType, true, false, affectsCaster, affectsBestTarget, m_knockbackDistance, m_knockbackType);
	}
}
