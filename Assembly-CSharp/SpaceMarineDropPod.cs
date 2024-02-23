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
		Targeter = new AbilityUtil_Targeter_KnockbackAoE(
			this,
			m_knockbackShape,
			m_penetrateLoS,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			true,
			false,
			AbilityUtil_Targeter.AffectsActor.Never,
			AbilityUtil_Targeter.AffectsActor.Possible,
			m_knockbackDistance,
			m_knockbackType);
	}

	public int ModdedDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public float ModdedKnockbackDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance)
			: m_knockbackDistance;
	}

	public int GetEnergyRefundIfNoEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyRefundIfNoEnemyHitMod.GetModifiedValue(m_energyRefundIfNoEnemyHit)
			: m_energyRefundIfNoEnemyHit;
	}

	public int GetExtraPowerupHealIfDirectHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraPowerupHealIfDirectHitMod.GetModifiedValue(m_extraPowerupHealIfDirectHit)
			: m_extraPowerupHealIfDirectHit;
	}

	public int GetExtraPowerupEnergyIfDirectHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraPowerupEnergyIfDirectHitMod.GetModifiedValue(m_extraPowerupEnergyIfDirectHit)
			: m_extraPowerupEnergyIfDirectHit;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, ModdedDamage())
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(
		ActorData targetActor,
		int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (m_abilityMod != null
		    && m_abilityMod.m_groundEffectInfoOnDropPod.m_applyGroundEffect
		    && m_abilityMod.m_groundEffectInfoOnDropPod.m_groundEffectData.damageAmount > 0)
		{
			List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
			BoardSquare targetSquare = Board.Get().GetSquare(Targeter.LastUpdatingGridPos);
			if (tooltipSubjectTypes != null
			    && targetSquare != null
			    && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				GroundEffectField groundEffectData = m_abilityMod.m_groundEffectInfoOnDropPod.m_groundEffectData;
				bool isHit = AreaEffectUtils.IsSquareInShape(
					targetActor.GetCurrentBoardSquare(),
					groundEffectData.shape,
					Targeter.LastUpdateFreePos,
					targetSquare,
					m_penetrateLoS,
					ActorData);
				if (isHit)
				{
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					dictionary[AbilityTooltipSymbol.Damage] = ModdedDamage() + groundEffectData.damageAmount;
				}
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetEnergyRefundIfNoEnemyHit() > 0
		    && Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy) == 0)
		{
			return GetEnergyRefundIfNoEnemyHit();
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
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_SpaceMarineDropPod != null
			? abilityMod_SpaceMarineDropPod.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AddTokenInt(tokens, "EnergyRefundIfNoEnemyHit", string.Empty, abilityMod_SpaceMarineDropPod != null
			? abilityMod_SpaceMarineDropPod.m_energyRefundIfNoEnemyHitMod.GetModifiedValue(m_energyRefundIfNoEnemyHit)
			: m_energyRefundIfNoEnemyHit);
		AddTokenInt(tokens, "NumPowerupToSpawn", string.Empty, m_numPowerupToSpawn);
		AddTokenInt(tokens, "PowerupDuration", string.Empty, m_powerupDuration);
		AddTokenInt(tokens, "ExtraPowerupHealIfDirectHit", string.Empty, abilityMod_SpaceMarineDropPod != null
			? abilityMod_SpaceMarineDropPod.m_extraPowerupHealIfDirectHitMod.GetModifiedValue(m_extraPowerupHealIfDirectHit)
			: m_extraPowerupHealIfDirectHit);
		AddTokenInt(tokens, "ExtraPowerupEnergy", string.Empty, abilityMod_SpaceMarineDropPod != null
			? abilityMod_SpaceMarineDropPod.m_extraPowerupEnergyIfDirectHitMod.GetModifiedValue(m_extraPowerupEnergyIfDirectHit)
			: m_extraPowerupEnergyIfDirectHit);
		if (m_powerupPrefab != null && m_powerupPrefab.m_ability != null)
		{
			PowerUp_Standard_Ability powerUp_Standard_Ability = m_powerupPrefab.m_ability as PowerUp_Standard_Ability;
			if (powerUp_Standard_Ability != null)
			{
				AddTokenInt(tokens, "PowerupHealing", string.Empty, powerUp_Standard_Ability.m_healAmount);
			}
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SpaceMarineDropPod))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		
		m_abilityMod = abilityMod as AbilityMod_SpaceMarineDropPod;
		Targeter = new AbilityUtil_Targeter_KnockbackAoE(
			this,
			m_knockbackShape,
			m_penetrateLoS,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			true,
			false,
			AbilityUtil_Targeter.AffectsActor.Never,
			AbilityUtil_Targeter.AffectsActor.Possible,
			ModdedKnockbackDistance(),
			m_knockbackType);
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Targeter = new AbilityUtil_Targeter_KnockbackAoE(
			this,
			m_knockbackShape,
			m_penetrateLoS,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			true,
			false,
			AbilityUtil_Targeter.AffectsActor.Never,
			AbilityUtil_Targeter.AffectsActor.Possible,
			m_knockbackDistance,
			m_knockbackType);
	}
}
