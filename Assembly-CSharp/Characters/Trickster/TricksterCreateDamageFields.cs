// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class TricksterCreateDamageFields : Ability
{
	[Header("-- Targeting --")]
	public bool m_addFieldAroundSelf = true;
	public bool m_useInitialShapeOverride;
	public AbilityAreaShape m_initialShapeOverride = AbilityAreaShape.Three_x_Three;
	[Header("-- Ground Field Info --")]
	public GroundEffectField m_groundFieldInfo;
	[Header("-- Self Effect for Multi Hit")]
	public StandardEffectInfo m_selfEffectForMultiHit;
	[Header("-- Extra Enemy Hit Effect On Cast")]
	public StandardEffectInfo m_extraEnemyEffectOnCast;
	[Header("-- Spoil spawn info")]
	public bool m_spawnSpoilForEnemyHit = true;
	public bool m_spawnSpoilForAllyHit;
	public SpoilsSpawnData m_spoilSpawnInfo;
	public bool m_onlySpawnSpoilOnMultiHit = true;
	[Header("-- use [Cast Sequence Prefab] to time spawning of ground effect (including temp satellite)")]
	public GameObject m_castSequencePrefab;
	[Header("   use [Temp Satellite Sequence Prefab] for satellites above each ground field")]
	public GameObject m_tempSatelliteSequencePrefab;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;
	private AbilityMod_TricksterCreateDamageFields m_abilityMod;
	private StandardEffectInfo m_cachedSelfEffectForMultiHit;
	private StandardEffectInfo m_cachedExtraEnemyEffectOnCast;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Ground Fields";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_afterImageSyncComp == null)
		{
			m_afterImageSyncComp = GetComponent<TricksterAfterImageNetworkBehaviour>();
		}
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_TricksterFlare(
			this,
			m_afterImageSyncComp,
			UseInitialShapeOverride() ? GetInitialShapeOverride() : GetGroundFieldInfo().shape,
			GetGroundFieldInfo().penetrateLos,
			GetGroundFieldInfo().IncludeEnemies(),
			GetGroundFieldInfo().IncludeAllies(),
			AddFieldAroundSelf());
	}

	private void SetCachedFields()
	{
		m_cachedSelfEffectForMultiHit = m_abilityMod != null
			? m_abilityMod.m_selfEffectForMultiHitMod.GetModifiedValue(m_selfEffectForMultiHit)
			: m_selfEffectForMultiHit;
		m_cachedExtraEnemyEffectOnCast = m_abilityMod != null
			? m_abilityMod.m_extraEnemyEffectOnCastMod.GetModifiedValue(m_extraEnemyEffectOnCast)
			: m_extraEnemyEffectOnCast;
	}

	public bool AddFieldAroundSelf()
	{
		return m_abilityMod != null
			? m_abilityMod.m_addFieldAroundSelfMod.GetModifiedValue(m_addFieldAroundSelf)
			: m_addFieldAroundSelf;
	}

	public bool UseInitialShapeOverride()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useInitialShapeOverrideMod.GetModifiedValue(m_useInitialShapeOverride)
			: m_useInitialShapeOverride;
	}

	public AbilityAreaShape GetInitialShapeOverride()
	{
		return m_abilityMod != null
			? m_abilityMod.m_initialShapeOverrideMod.GetModifiedValue(m_initialShapeOverride)
			: m_initialShapeOverride;
	}

	public GroundEffectField GetGroundFieldInfo()
	{
		return m_abilityMod != null
			? m_abilityMod.m_groundFieldInfoMod.GetModifiedValue(m_groundFieldInfo)
			: m_groundFieldInfo;
	}

	public StandardEffectInfo GetSelfEffectForMultiHit()
	{
		return m_cachedSelfEffectForMultiHit ?? m_selfEffectForMultiHit;
	}

	public StandardEffectInfo GetExtraEnemyEffectOnCast()
	{
		return m_cachedExtraEnemyEffectOnCast ?? m_extraEnemyEffectOnCast;
	}

	public bool SpawnSpoilForEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_spawnSpoilForEnemyHitMod.GetModifiedValue(m_spawnSpoilForEnemyHit)
			: m_spawnSpoilForEnemyHit;
	}

	public bool SpawnSpoilForAllyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_spawnSpoilForAllyHitMod.GetModifiedValue(m_spawnSpoilForAllyHit)
			: m_spawnSpoilForAllyHit;
	}

	public bool OnlySpawnSpoilOnMultiHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_onlySpawnSpoilOnMultiHitMod.GetModifiedValue(m_onlySpawnSpoilOnMultiHit)
			: m_onlySpawnSpoilOnMultiHit;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GroundEffectField groundFieldInfo = GetGroundFieldInfo();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, groundFieldInfo.damageAmount);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Secondary, groundFieldInfo.healAmount);
		AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Secondary, groundFieldInfo.energyGain);
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return AddFieldAroundSelf()
		       || m_afterImageSyncComp == null
		       || m_afterImageSyncComp.HasVaidAfterImages();
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		GroundEffectField groundFieldInfo = GetGroundFieldInfo();
		if (groundFieldInfo.IncludeEnemies())
		{
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeter,
				targetActor,
				currentTargeterIndex,
				groundFieldInfo.damageAmount,
				groundFieldInfo.subsequentDamageAmount);
		}
		if (groundFieldInfo.IncludeAllies())
		{
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeter,
				targetActor,
				currentTargeterIndex,
				groundFieldInfo.healAmount,
				groundFieldInfo.subsequentHealAmount,
				AbilityTooltipSymbol.Healing,
				AbilityTooltipSubject.Secondary);
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeter,
				targetActor,
				currentTargeterIndex,
				groundFieldInfo.energyGain,
				groundFieldInfo.subsequentEnergyGain,
				AbilityTooltipSymbol.Energy,
				AbilityTooltipSubject.Secondary);
		}
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterCreateDamageFields abilityMod_TricksterCreateDamageFields = modAsBase as AbilityMod_TricksterCreateDamageFields;
		// rogues
		// if (m_groundFieldInfo == null)
		// {
		// 	m_groundFieldInfo = ScriptableObject.CreateInstance<GroundEffectField>();
		// }
		m_groundFieldInfo.AddTooltipTokens(tokens, "GroundEffect");
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCreateDamageFields != null
			? abilityMod_TricksterCreateDamageFields.m_selfEffectForMultiHitMod.GetModifiedValue(m_selfEffectForMultiHit)
			: m_selfEffectForMultiHit, "SelfEffectForMultiHit", m_selfEffectForMultiHit);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterCreateDamageFields != null
			? abilityMod_TricksterCreateDamageFields.m_extraEnemyEffectOnCastMod.GetModifiedValue(m_extraEnemyEffectOnCast)
			: m_extraEnemyEffectOnCast, "ExtraEnemyEffectOnCast", m_extraEnemyEffectOnCast);
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages())
		{
			if (afterImage == null || afterImage.IsDead())
			{
				continue;
			}
			m_afterImageSyncComp.TurnToPosition(afterImage, targetPos);
			Animator modelAnimator = afterImage.GetModelAnimator();
			modelAnimator.SetInteger("Attack", animationIndex);
			modelAnimator.SetBool("CinematicCam", cinecam);
			modelAnimator.SetTrigger("StartAttack");
		}
	}

	public override void OnAbilityAnimationRequestProcessed(ActorData caster)
	{
		foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages())
		{
			if (afterImage == null || afterImage.IsDead())
			{
				continue;
			}
			Animator modelAnimator = afterImage.GetModelAnimator();
			modelAnimator.SetInteger("Attack", 0);
			modelAnimator.SetBool("CinematicCam", false);
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TricksterCreateDamageFields))
		{
			m_abilityMod = abilityMod as AbilityMod_TricksterCreateDamageFields;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

#if SERVER
	// added in rogues
	public override List<int> GetAdditionalBrushRegionsToDisrupt(ActorData caster, List<AbilityTarget> targets)
	{
		List<int> list = new List<int>();
		if (m_afterImageSyncComp == null)
		{
			return list;
		}
		foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages())
		{
			if (afterImage.IsInBrush())
			{
				list.Add(afterImage.GetBrushRegion());
			}
		}
		return list;
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		List<BoardSquare> centerSquares = GetCenterSquares(targets, caster);
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		GroundEffectField groundFieldInfo = GetGroundFieldInfo();
		AbilityAreaShape shape = UseInitialShapeOverride() ? GetInitialShapeOverride() : groundFieldInfo.shape;
		for (int i = 0; i < centerSquares.Count; i++)
		{
			ActorData targetingActor = caster;
			if (i > 0 && i <= validAfterImages.Count)
			{
				targetingActor = validAfterImages[i - 1];
			}
			bool includeEnemies = groundFieldInfo.IncludeEnemies();
			bool includeAllies = groundFieldInfo.IncludeAllies();
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, includeAllies, includeEnemies);
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
				shape,
				centerSquares[i].ToVector3(),
				centerSquares[i],
				groundFieldInfo.penetrateLos,
				caster,
				relevantTeams,
				null);
			if (groundFieldInfo.hitPulseSequencePrefab != null && actorsInShape.Count > 0)
			{
				list.Add(new ServerClientUtils.SequenceStartData(
					groundFieldInfo.hitPulseSequencePrefab,
					centerSquares[i].ToVector3(),
					actorsInShape.ToArray(),
					caster,
					additionalData.m_sequenceSource));
			}

			list.Add(new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				centerSquares[i].ToVector3(),
				i == 0 && groundFieldInfo.hitPulseSequencePrefab == null
					? additionalData.m_abilityResults.HitActorsArray()
					: null,
				targetingActor,
				additionalData.m_sequenceSource));
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<BoardSquare> centerSquares = GetCenterSquares(targets, caster);
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		GroundEffectField groundFieldInfo = GetGroundFieldInfo();
		List<StandardMultiAreaGroundEffect.GroundAreaInfo> areaInfoList = new List<StandardMultiAreaGroundEffect.GroundAreaInfo>();
		for (int i = 0; i < centerSquares.Count; i++)
		{
			ActorData caster2 = caster;
			if (i > 0 && i <= validAfterImages.Count)
			{
				caster2 = validAfterImages[i - 1];
			}
			BoardSquare boardSquare = centerSquares[i];
			if (boardSquare != null)
			{
				StandardMultiAreaGroundEffect.GroundAreaInfo groundAreaInfo = new StandardMultiAreaGroundEffect.GroundAreaInfo(boardSquare, boardSquare.ToVector3(), groundFieldInfo.shape);
				if (m_tempSatelliteSequencePrefab != null)
				{
					ServerClientUtils.SequenceStartData seqStartData = new ServerClientUtils.SequenceStartData(m_tempSatelliteSequencePrefab, boardSquare.ToVector3(), null, caster2, abilityResults.SequenceSource);
					groundAreaInfo.AddSequenceStartDataToPersist(seqStartData);
				}
				areaInfoList.Add(groundAreaInfo);
			}
		}
		StandardMultiAreaGroundEffect standardMultiAreaGroundEffect = new StandardMultiAreaGroundEffect(AsEffectSource(), areaInfoList, caster, groundFieldInfo);
		bool isSelfEffectApplied = false;
		Dictionary<ActorData, GroundFieldActorHitInfo> hitToResultMaps = StandardMultiAreaGroundEffect.GetHitToResultMaps(
			groundFieldInfo,
			caster,
			areaInfoList,
			out var actorsInAreaList,
			out var numAreasWithHits,
			null,
			UseInitialShapeOverride(),
			GetInitialShapeOverride());
		if (RunPriority == AbilityPriority.Combat_Damage || SpawnSpoilForAllyHit() || SpawnSpoilForEnemyHit())
		{
			foreach (ActorData hitActor in hitToResultMaps.Keys)
			{
				bool isSpawningSpoils = !OnlySpawnSpoilOnMultiHit() || hitToResultMaps[hitActor].m_hitCount > 1;
				bool isAlly = hitActor.GetTeam() == caster.GetTeam();
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, hitActor.GetFreePos()));
				if (RunPriority == AbilityPriority.Combat_Damage)
				{
					if (!isAlly)
					{
						actorHitResults.SetBaseDamage(hitToResultMaps[hitActor].m_damage);
						actorHitResults.AddStandardEffectInfo(GetExtraEnemyEffectOnCast());
						for (int i = 0; i < actorsInAreaList.Count; i++)
						{
							if (actorsInAreaList[i].Contains(hitActor))
							{
								ActorData targetingActor = i > 0 && i <= validAfterImages.Count
									? validAfterImages[i - 1]
									: caster;
								actorHitResults.AddActorToReveal(targetingActor);
							}
						}
					}
					else
					{
						actorHitResults.SetBaseHealing(hitToResultMaps[hitActor].m_healing);
						actorHitResults.SetTechPointGain(hitToResultMaps[hitActor].m_energyGain);
					}
					if (hitToResultMaps[hitActor].m_effectInfo != null)
					{
						actorHitResults.AddStandardEffectInfo(hitToResultMaps[hitActor].m_effectInfo);
					}
					if (hitActor == caster && numAreasWithHits > 1 && GetSelfEffectForMultiHit().m_applyEffect)
					{
						actorHitResults.AddStandardEffectInfo(GetSelfEffectForMultiHit());
						isSelfEffectApplied = true;
					}
				}
				if (isSpawningSpoils && ((!isAlly && SpawnSpoilForEnemyHit()) || (isAlly && SpawnSpoilForAllyHit())))
				{
					actorHitResults.AddSpoilSpawnData(new SpoilSpawnDataForAbilityHit(hitActor, caster.GetTeam(), m_spoilSpawnInfo));
				}
				abilityResults.StoreActorHit(actorHitResults);
			}
			if (RunPriority == AbilityPriority.Combat_Damage)
			{
				foreach (List<ActorData> hitActors in actorsInAreaList)
				{
					standardMultiAreaGroundEffect.AddToActorsHitThisTurn(hitActors);
				}
			}
		}
		if (!isSelfEffectApplied && numAreasWithHits > 1 && GetSelfEffectForMultiHit().m_applyEffect)
		{
			ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			casterHitResults.AddStandardEffectInfo(GetSelfEffectForMultiHit());
			abilityResults.StoreActorHit(casterHitResults);
		}
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(centerSquares[0].ToVector3()));
		positionHitResults.AddEffect(standardMultiAreaGroundEffect);
		abilityResults.StorePositionHit(positionHitResults);
	}

	// added in rogues
	private List<BoardSquare> GetCenterSquares(List<AbilityTarget> targets, ActorData caster)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		if (AddFieldAroundSelf())
		{
			list.Add(caster.GetCurrentBoardSquare());
		}
		foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages())
		{
			list.Add(afterImage.GetCurrentBoardSquare());
		}
		return list;
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = base.CalcPointsOfInterestForCamera(targets, caster);
		if (m_afterImageSyncComp != null)
		{
			foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages())
			{
				if (afterImage.GetCurrentBoardSquare() != null)
				{
					list.Add(afterImage.GetCurrentBoardSquare().ToVector3());
				}
			}
		}
		return list;
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster != target)
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.TricksterStats.TargetsHitByZapTrap);
		}
	}

	// added in rogues
	public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
	{
		caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.TricksterStats.TargetsHitByZapTrap);
	}
#endif
}
