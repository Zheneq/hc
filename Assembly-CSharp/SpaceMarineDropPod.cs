﻿// ROGUES
// SERVER
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
					dictionary = new Dictionary<AbilityTooltipSymbol, int>
					{
						[AbilityTooltipSymbol.Damage] = ModdedDamage() + groundEffectData.damageAmount
					};
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
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_knockbackShape, targets[0]);
		List<ActorData> hitTargets = GetHitTargets(targets, caster, null);
		hitTargets.Add(caster);
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			centerOfShape,
			hitTargets.ToArray(),
			caster,
			additionalData.m_sequenceSource,
			new Sequence.IExtraSequenceParams[0]);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Dictionary<ActorData, ActorHitResults> dictionary = new Dictionary<ActorData, ActorHitResults>();
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitTargets = GetHitTargets(targets, caster, nonActorTargetInfo);
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_knockbackShape, targets[0]);
		foreach (ActorData hitActor in hitTargets)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, centerOfShape));
			actorHitResults.SetBaseDamage(ModdedDamage());
			if (ModdedKnockbackDistance() > 0f)
			{
				Vector3 centerOfShape2 = AreaEffectUtils.GetCenterOfShape(m_knockbackShape, targets[0]);
				KnockbackHitData knockbackData = new KnockbackHitData(
					hitActor, caster, m_knockbackType, targets[0].AimDirection, centerOfShape2, ModdedKnockbackDistance());
				actorHitResults.AddKnockbackData(knockbackData);
			}
			dictionary.Add(hitActor, actorHitResults);
		}
		ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		if (GetEnergyRefundIfNoEnemyHit() > 0 && hitTargets.Count == 0)
		{
			if (m_energyRefundAffectedByBuff)
			{
				actorHitResults2.SetTechPointGain(GetEnergyRefundIfNoEnemyHit());
			}
			else
			{
				actorHitResults2.AddDirectTechPointGainOnCaster(GetEnergyRefundIfNoEnemyHit());
			}
		}
		PowerUp powerupPrefab = m_powerupPrefab;
		if (powerupPrefab != null)
		{
			int num = 0;
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(m_powerupShape, targets[0], true, caster);
			BoardSquare boardSquare = null;
			foreach (BoardSquare boardSquare2 in squaresInShape)
			{
				if (num < m_numPowerupToSpawn)
				{
					num++;
					if (boardSquare == null)
					{
						boardSquare = boardSquare2;
					}
					SpoilSpawnDataForAbilityHit spoilSpawnData = CreateSpoilSpawnData(caster, boardSquare2, powerupPrefab.gameObject, 1);
					actorHitResults2.AddSpoilSpawnData(spoilSpawnData);
				}
			}
			if (num < m_numPowerupToSpawn && boardSquare != null)
			{
				int numToSpawn = Mathf.Min(20, m_numPowerupToSpawn - num);
				SpoilSpawnDataForAbilityHit spoilSpawnData2 = CreateSpoilSpawnData(caster, boardSquare, powerupPrefab.gameObject, numToSpawn);
				actorHitResults2.AddSpoilSpawnData(spoilSpawnData2);
			}
		}
		if (m_abilityMod != null && m_abilityMod.m_groundEffectInfoOnDropPod.m_applyGroundEffect)
		{
			GroundEffectField groundEffectData = m_abilityMod.m_groundEffectInfoOnDropPod.m_groundEffectData;
			StandardGroundEffect standardGroundEffect = new StandardGroundEffect(AsEffectSource(), square, targets[0].FreePos, null, caster, groundEffectData);
			List<ActorData> affectableActorsInField = m_abilityMod.m_groundEffectInfoOnDropPod.GetAffectableActorsInField(targets[0], caster, nonActorTargetInfo);
			Vector3 centerOfShape3 = AreaEffectUtils.GetCenterOfShape(groundEffectData.shape, targets[0]);
			foreach (ActorData actorData2 in affectableActorsInField)
			{
				ActorHitResults value = null;
				if (dictionary.ContainsKey(actorData2))
				{
					value = dictionary[actorData2];
				}
				else
				{
					value = new ActorHitResults(new ActorHitParameters(actorData2, centerOfShape3));
					dictionary.Add(actorData2, value);
				}
				m_abilityMod.m_groundEffectInfoOnDropPod.SetupActorHitResult(ref value, caster, actorData2.GetCurrentBoardSquare());
			}
			standardGroundEffect.AddToActorsHitThisTurn(affectableActorsInField);
			actorHitResults2.AddEffect(standardGroundEffect);
		}
		abilityResults.StoreActorHit(actorHitResults2);
		foreach (ActorHitResults hitResults in dictionary.Values)
		{
			abilityResults.StoreActorHit(hitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private SpoilSpawnDataForAbilityHit CreateSpoilSpawnData(ActorData caster, BoardSquare desiredSpawnSquare, GameObject spoilPrefab, int numToSpawn)
	{
		SpoilSpawnDataForAbilityHit spoilSpawnDataForAbilityHit = new SpoilSpawnDataForAbilityHit(
			desiredSpawnSquare,
			caster.GetTeam(),
			new List<GameObject>
			{
				spoilPrefab
			})
		{
			m_numToSpawn = numToSpawn,
			m_duration = m_powerupDuration,
			m_canSpawnOnEnemyOccupiedSquare = m_canSpawnOnEnemyOccupiedSquares,
			m_canSpawnOnAllyOccupiedSquare = m_canSpawnOnAllyOccupiedSquares
		};
		if (m_abilityMod != null || GetExtraPowerupHealIfDirectHit() > 0 || GetExtraPowerupEnergyIfDirectHit() > 0)
		{
			spoilSpawnDataForAbilityHit.m_spoilMod = new StandardPowerUpAbilityModData();
			if (m_abilityMod != null)
			{
				spoilSpawnDataForAbilityHit.m_spoilMod.m_healMod.CopyValuesFrom(m_abilityMod.m_powerupHealMod);
				spoilSpawnDataForAbilityHit.m_spoilMod.m_techPointMod.CopyValuesFrom(m_abilityMod.m_powerupTechPointMod);
			}
			spoilSpawnDataForAbilityHit.m_spoilMod.m_extraHealIfDirectHit = GetExtraPowerupHealIfDirectHit();
			spoilSpawnDataForAbilityHit.m_spoilMod.m_extraTechPointIfDirectHit = GetExtraPowerupEnergyIfDirectHit();
		}
		return spoilSpawnDataForAbilityHit;
	}

	// added in rogues
	private List<ActorData> GetHitTargets(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return AreaEffectUtils.GetActorsInShape(m_knockbackShape, targets[0], m_penetrateLoS, caster, caster.GetOtherTeams(), nonActorTargetInfo);
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SpaceMarineStats.DamageDealtByUlt, results.FinalDamage);
		}
		if (results.FinalHealing > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SpaceMarineStats.HealingDealtByUlt, results.FinalHealing);
		}
		if (results.AppliedStatus(StatusType.Rooted) || results.AppliedStatus(StatusType.Snared))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.SpaceMarineStats.NumSlowsPlusRootsApplied);
		}
	}
#endif
}
