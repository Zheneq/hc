// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class TricksterBasicAttack : Ability
{
	[Header("-- Laser Targeting")]
	public LaserTargetingInfo m_laserInfo;
	[Header("-- Damage and Effect")]
	public int m_laserDamageAmount = 3;
	public int m_laserSubsequentDamageAmount = 2;
	public int m_extraDamageForSingleHit;
	public StandardEffectInfo m_enemySingleHitHitEffect;
	public StandardEffectInfo m_enemyMultiHitEffect;
	[Header("-- Effect on Self for Multi Hit")]
	public StandardEffectInfo m_selfEffectForMultiHit;
	[Header("-- Energy Gain --")]
	public int m_energyGainPerLaserHit;
	[Header("-- For spawning spoils")]
	public SpoilsSpawnData m_spoilSpawnInfo;
	public bool m_onlySpawnSpoilOnMultiHit = true;
	[Header("-- Sequences")]
	public GameObject m_projectileSequencePrefab;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;
	private AbilityMod_TricksterBasicAttack m_abilityMod;
	private LaserTargetingInfo m_cachedLaserInfo;
	private StandardEffectInfo m_cachedEnemySingleHitHitEffect;
	private StandardEffectInfo m_cachedEnemyMultiHitEffect;
	private StandardEffectInfo m_cachedSelfEffectForMultiHit;
	private SpoilsSpawnData m_cachedSpoilSpawnInfo;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Trickster Laser";
		}
		Setup();
	}

	private void Setup()
	{
		m_afterImageSyncComp = GetComponent<TricksterAfterImageNetworkBehaviour>();
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_TricksterLaser(this, m_afterImageSyncComp, GetLaserInfo(), 2);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	public override TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		return TargetingParadigm.Position;
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_abilityMod != null
			? m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo)
			: m_laserInfo;
		m_cachedEnemySingleHitHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemySingleHitHitEffectMod.GetModifiedValue(m_enemySingleHitHitEffect)
			: m_enemySingleHitHitEffect;
		m_cachedEnemyMultiHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyMultiHitEffectMod.GetModifiedValue(m_enemyMultiHitEffect)
			: m_enemyMultiHitEffect;
		m_cachedSelfEffectForMultiHit = m_abilityMod != null
			? m_abilityMod.m_selfEffectForMultiHitMod.GetModifiedValue(m_selfEffectForMultiHit)
			: m_selfEffectForMultiHit;
		m_cachedSpoilSpawnInfo = m_abilityMod != null
			? m_abilityMod.m_spoilSpawnInfoMod.GetModifiedValue(m_spoilSpawnInfo)
			: m_spoilSpawnInfo;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public int GetLaserDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public int GetLaserSubsequentDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserSubsequentDamageAmountMod.GetModifiedValue(m_laserSubsequentDamageAmount)
			: m_laserSubsequentDamageAmount;
	}

	public int GetExtraDamageForSingleHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit)
			: m_extraDamageForSingleHit;
	}

	public StandardEffectInfo GetEnemySingleHitHitEffect()
	{
		return m_cachedEnemySingleHitHitEffect ?? m_enemySingleHitHitEffect;
	}

	public StandardEffectInfo GetEnemyMultiHitEffect()
	{
		return m_cachedEnemyMultiHitEffect ?? m_enemyMultiHitEffect;
	}

	public StandardEffectInfo GetSelfEffectForMultiHit()
	{
		return m_cachedSelfEffectForMultiHit ?? m_selfEffectForMultiHit;
	}

	public int GetEnergyGainPerLaserHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyGainPerLaserHitMod.GetModifiedValue(m_energyGainPerLaserHit)
			: m_energyGainPerLaserHit;
	}

	public SpoilsSpawnData GetSpoilSpawnInfo()
	{
		return m_cachedSpoilSpawnInfo ?? m_spoilSpawnInfo;
	}

	public bool OnlySpawnSpoilOnMultiHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_onlySpawnSpoilOnMultiHitMod.GetModifiedValue(m_onlySpawnSpoilOnMultiHit)
			: m_onlySpawnSpoilOnMultiHit;
	}

	private int CalcDamageFromNumHits(int numHits, int numFromCover)
	{
		return ActorMultiHitContext.CalcDamageFromNumHits(numHits, numFromCover, GetLaserDamageAmount(), GetLaserSubsequentDamageAmount());
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetLaserDamageAmount());
		GetEnemySingleHitHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = base.Debug_GetExpectedNumbersInTooltip();
		list.Add(GetLaserSubsequentDamageAmount());
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (Targeter is AbilityUtil_Targeter_TricksterLaser abilityUtil_Targeter_TricksterLaser
		    && abilityUtil_Targeter_TricksterLaser.m_actorToHitCount.TryGetValue(targetActor, out int numHits))
		{
			int numFromCover = abilityUtil_Targeter_TricksterLaser.m_actorToCoverCount[targetActor];
			int damage = CalcDamageFromNumHits(numHits, numFromCover);
			if (numHits == 1 && GetExtraDamageForSingleHit() > 0)
			{
				damage += GetExtraDamageForSingleHit();
			}
			dictionary[AbilityTooltipSymbol.Damage] = damage;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetEnergyGainPerLaserHit() > 0)
		{
			int hits = Targeter.GetTooltipSubjectCountTotalWithDuplicates(AbilityTooltipSubject.Primary);
			return hits * GetEnergyGainPerLaserHit();
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TricksterBasicAttack abilityMod_TricksterBasicAttack = modAsBase as AbilityMod_TricksterBasicAttack;
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, abilityMod_TricksterBasicAttack != null
			? abilityMod_TricksterBasicAttack.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount);
		AddTokenInt(tokens, "LaserSubsequentDamageAmount", string.Empty, abilityMod_TricksterBasicAttack != null
			? abilityMod_TricksterBasicAttack.m_laserSubsequentDamageAmountMod.GetModifiedValue(m_laserSubsequentDamageAmount)
			: m_laserSubsequentDamageAmount);
		AddTokenInt(tokens, "ExtraDamageForSingleHit", string.Empty, abilityMod_TricksterBasicAttack != null
			? abilityMod_TricksterBasicAttack.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit)
			: m_extraDamageForSingleHit);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterBasicAttack != null
			? abilityMod_TricksterBasicAttack.m_enemySingleHitHitEffectMod.GetModifiedValue(m_enemySingleHitHitEffect)
			: m_enemySingleHitHitEffect, "EnemySingleHitHitEffect", m_enemySingleHitHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterBasicAttack != null
			? abilityMod_TricksterBasicAttack.m_enemyMultiHitEffectMod.GetModifiedValue(m_enemyMultiHitEffect)
			: m_enemyMultiHitEffect, "EnemyMultiHitEffect", m_enemyMultiHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TricksterBasicAttack != null
			? abilityMod_TricksterBasicAttack.m_selfEffectForMultiHitMod.GetModifiedValue(m_selfEffectForMultiHit)
			: m_selfEffectForMultiHit, "SelfEffectForMultiHit", m_selfEffectForMultiHit);
		AddTokenInt(tokens, "EnergyGainPerLaserHit", string.Empty, abilityMod_TricksterBasicAttack != null
			? abilityMod_TricksterBasicAttack.m_energyGainPerLaserHitMod.GetModifiedValue(m_energyGainPerLaserHit)
			: m_energyGainPerLaserHit);
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
		if (abilityMod.GetType() == typeof(AbilityMod_TricksterBasicAttack))
		{
			m_abilityMod = abilityMod as AbilityMod_TricksterBasicAttack;
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
		if (m_afterImageSyncComp != null)
		{
			List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
			for (int i = 0; i < validAfterImages.Count; i++)
			{
				if (validAfterImages[i].IsInBrush())
				{
					list.Add(validAfterImages[i].GetBrushRegion());
				}
			}
		}
		return list;
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		GetActorToDamageMap(
			targets,
			caster,
			out List<VectorUtils.LaserCoords> endPoints,
			out List<List<ActorData>> hitActorsInLasers,
			out _,
			out _,
			out _,
			null);
		list.Add(new ServerClientUtils.SequenceStartData(
			m_projectileSequencePrefab,
			endPoints[0].end,
			hitActorsInLasers[0].ToArray(),
			caster,
			additionalData.m_sequenceSource));
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		int num2 = 1;
		foreach (ActorData afterImage in validAfterImages)
		{
			if (num2 >= endPoints.Count)
			{
				Debug.LogError("number of end points did not match number of after images");
				break;
			}

			list.Add(new ServerClientUtils.SequenceStartData(
				m_projectileSequencePrefab,
				endPoints[num2].end,
				hitActorsInLasers[num2].ToArray(),
				afterImage,
				additionalData.m_sequenceSource));
			num2++;
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Dictionary<ActorData, int> actorToDamageMap = GetActorToDamageMap(
			targets,
			caster,
			out _,
			out _,
			out Dictionary<ActorData, List<ActorData>> actorToHittingActors,
			out Dictionary<ActorData, int> actorToCoverCount,
			out int numLasersWithHits,
			nonActorTargetInfo);
		foreach (ActorData hitActor in actorToDamageMap.Keys)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, hitActor.GetFreePos()));
			actorHitResults.SetBaseDamage(actorToDamageMap[hitActor]);
			List<ActorData> hittingActors = actorToHittingActors[hitActor];
			int numHits = hittingActors.Count;
			if (GetEnergyGainPerLaserHit() > 0)
			{
				actorHitResults.SetTechPointGainOnCaster(GetEnergyGainPerLaserHit() * numHits);
			}
			actorHitResults.AddStandardEffectInfo(numHits > 1 ? GetEnemyMultiHitEffect() : GetEnemySingleHitHitEffect());
			if (numHits >= 3)
			{
				actorHitResults.AddHitResultsTag(HitResultsTags.TripleHit);
			}
			if (!OnlySpawnSpoilOnMultiHit() || numHits > 1)
			{
				actorHitResults.AddSpoilSpawnData(new SpoilSpawnDataForAbilityHit(hitActor, caster.GetTeam(), GetSpoilSpawnInfo()));
			}
			if (actorToCoverCount[hitActor] > 0)
			{
				actorHitResults.OverrideAsInCover();
			}
			foreach (ActorData afterImage in hittingActors)
			{
				actorHitResults.AddActorToReveal(afterImage);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		if (GetSelfEffectForMultiHit().m_applyEffect && numLasersWithHits > 1)
		{
			ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			casterHitResults.AddStandardEffectInfo(GetSelfEffectForMultiHit());
			abilityResults.StoreActorHit(casterHitResults);
		}
	}

	// added in rogues
	private Dictionary<ActorData, int> GetActorToDamageMap(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<VectorUtils.LaserCoords> endPoints,
		out List<List<ActorData>> hitActorsInLasers,
		out Dictionary<ActorData, List<ActorData>> actorToHittingActors,
		out Dictionary<ActorData, int> actorToCoverCount,
		out int numLasersWithHits,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		endPoints = new List<VectorUtils.LaserCoords>();
		hitActorsInLasers = new List<List<ActorData>>();
		actorToHittingActors = new Dictionary<ActorData, List<ActorData>>();
		actorToCoverCount = new Dictionary<ActorData, int>();
		numLasersWithHits = 0;
		List<ActorData> allTargetingActors = new List<ActorData>();
		allTargetingActors.Add(caster);
		allTargetingActors.AddRange(m_afterImageSyncComp.GetValidAfterImages());
		m_afterImageSyncComp.CalcTargetingCenterAndAimAtPos(
			targets[0].FreePos,
			caster,
			allTargetingActors, 
			true,
			out _,
			out Vector3 freePosForAim);
		Dictionary<ActorData, int> hitActorToDamage = new Dictionary<ActorData, int>();
		LaserTargetingInfo laserInfo = GetLaserInfo();
		foreach (ActorData actorData in allTargetingActors)
		{
			Vector3 dir = freePosForAim - actorData.GetFreePos();
			dir.y = 0f;
			dir.Normalize();
			VectorUtils.LaserCoords laserCoords;
			laserCoords.start = actorData.GetLoSCheckPos();
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
				laserCoords.start,
				dir,
				laserInfo.range,
				laserInfo.width,
				caster,
				laserInfo.GetAffectedTeams(caster),
				laserInfo.penetrateLos,
				laserInfo.maxTargets,
				false,
				true,
				out laserCoords.end,
				nonActorTargetInfo);
			endPoints.Add(laserCoords);
			if (actorsInLaser.Count > 0)
			{
				numLasersWithHits++;
			}
			foreach (ActorData hitActor in actorsInLaser)
			{
				// custom
				bool inCover = hitActor.GetActorCover().IsInCoverWrt(laserCoords.start);
				// rogues
				// HitChanceBracketType hitChanceBracketType;
				// bool flag = hitActor.GetActorCover().IsInCoverWrt(laserCoords.start, out hitChanceBracketType);
				if (actorToHittingActors.TryGetValue(hitActor, out List<ActorData> hittingActors))
				{
					hittingActors.Add(actorData);
					actorToCoverCount[hitActor] += inCover ? 1 : 0;
				}
				else
				{
					actorToHittingActors[hitActor] = new List<ActorData>(1) { actorData };
					actorToCoverCount[hitActor] = inCover ? 1 : 0;
				}
			}
			hitActorsInLasers.Add(actorsInLaser);
		}
		foreach (ActorData hitActor in actorToHittingActors.Keys)
		{
			int numHits = actorToHittingActors[hitActor].Count;
			int numFromCover = actorToCoverCount[hitActor];
			hitActorToDamage[hitActor] = CalcDamageFromNumHits(numHits, numFromCover);
		}
		if (GetExtraDamageForSingleHit() > 0)
		{
			foreach (ActorData hitActor in actorToHittingActors.Keys)
			{
				if (actorToHittingActors[hitActor].Count == 1)
				{
					hitActorToDamage[hitActor] += GetExtraDamageForSingleHit();
				}
			}
		}
		return hitActorToDamage;
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = base.CalcPointsOfInterestForCamera(targets, caster);
		if (m_afterImageSyncComp == null)
		{
			return list;
		}
		foreach (ActorData afterImage in m_afterImageSyncComp.GetValidAfterImages())
		{
			if (afterImage.GetCurrentBoardSquare() != null)
			{
				list.Add(afterImage.GetCurrentBoardSquare().ToVector3());
			}
		}
		return list;
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.HasHitResultsTag(HitResultsTags.TripleHit))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.TricksterStats.TargetsHitByThreeImages);
		}
	}
#endif
}
