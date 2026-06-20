// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class TricksterFlare : Ability
{
	[Header("-- Targeting ")]
	public AbilityAreaShape m_flareShape = AbilityAreaShape.Three_x_Three;
	public bool m_flarePenetrateLos;
	public bool m_flareAroundSelf = true;
	[Header("-- Enemy Hit")]
	public bool m_includeEnemies = true;
	public int m_flareDamageAmount = 3;
	public int m_flareSubsequentDamageAmount = 2;
	public StandardEffectInfo m_enemyHitEffect;
	[Space(10f)]
	public bool m_useEnemyMultiHitEffect;
	public StandardEffectInfo m_enemyMultipleHitEffect;
	[Header("-- Ally Hit")]
	public bool m_includeAllies;
	public int m_flareHealAmount;
	public int m_flareSubsequentHealAmount;
	public StandardEffectInfo m_allyHitEffect;
	[Space(10f)]
	public bool m_useAllyMultiHitEffect;
	public StandardEffectInfo m_allyMultipleHitEffect;
	[Header("-- Self Hit")]
	public StandardEffectInfo m_selfHitEffectForMultiHit;
	[Header("-- Spoil spawn info")]
	public bool m_spawnSpoilForEnemyHit = true;
	public bool m_spawnSpoilForAllyHit;
	public SpoilsSpawnData m_spoilSpawnInfo;
	public bool m_onlySpawnSpoilOnMultiHit = true;
	[Header("-- Sequences ----------------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Flare";
		}
		m_afterImageSyncComp = GetComponent<TricksterAfterImageNetworkBehaviour>();
		m_sequencePrefab = m_castSequencePrefab;
		Targeter = new AbilityUtil_Targeter_TricksterFlare(
			this,
			m_afterImageSyncComp,
			m_flareShape,
			m_flarePenetrateLos,
			m_includeEnemies,
			m_includeAllies,
			m_flareAroundSelf);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_flareDamageAmount);
		m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Secondary, m_flareHealAmount);
		m_allyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		if (m_flareSubsequentDamageAmount > 0 && m_flareSubsequentDamageAmount != m_flareDamageAmount)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Tertiary, m_flareSubsequentDamageAmount);
		}
		if (m_flareSubsequentHealAmount > 0 && m_flareSubsequentHealAmount != m_flareHealAmount)
		{
			AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Quaternary, m_flareSubsequentHealAmount);
		}
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return m_flareAroundSelf
		       || m_afterImageSyncComp == null
		       || m_afterImageSyncComp.HasVaidAfterImages();
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		if (m_includeEnemies)
		{
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeter,
				targetActor,
				currentTargeterIndex,
				m_flareDamageAmount,
				m_flareSubsequentDamageAmount);
		}
		if (m_includeAllies)
		{
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeter,
				targetActor,
				currentTargeterIndex,
				m_flareHealAmount,
				m_flareSubsequentHealAmount,
				AbilityTooltipSymbol.Healing,
				AbilityTooltipSubject.Secondary);
		}
		return symbolToValue;
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

#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		GetActorToHpDeltaMap(targets, caster, out _, out List<BoardSquare> centerSquares, out _, out _, out _, null);
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		for (int i = 0; i < centerSquares.Count; i++)
		{
			ActorData targetingActor = caster;
			if (i > 0 && i <= validAfterImages.Count)
			{
				targetingActor = validAfterImages[i - 1];
			}
			list.Add(new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				centerSquares[i].ToVector3(),
				i == 0 ? additionalData.m_abilityResults.HitActorsArray() : null,
				targetingActor,
				additionalData.m_sequenceSource));
		}
		return list;
	}

	// added in rogues
	private Dictionary<ActorData, int> GetActorToHpDeltaMap(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<List<ActorData>> actorsForSequences,
		out List<BoardSquare> centerSquares,
		out Dictionary<ActorData, Vector3> actorToDamageOrigin,
		out Dictionary<ActorData, int> actorToHitCount,
		out int numShapesWithHits,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		actorsForSequences = new List<List<ActorData>>();
		centerSquares = new List<BoardSquare>();
		actorToDamageOrigin = new Dictionary<ActorData, Vector3>();
		actorToHitCount = new Dictionary<ActorData, int>();
		numShapesWithHits = 0;
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		if (m_flareAroundSelf)
		{
			centerSquares.Add(caster.GetCurrentBoardSquare());
		}
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		foreach (ActorData afterImage in validAfterImages)
		{
			centerSquares.Add(afterImage.GetCurrentBoardSquare());
		}
		for (int i = 0; i < centerSquares.Count; i++)
		{
			actorsForSequences.Add(new List<ActorData>());
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_flareShape,
				centerSquares[i].ToVector3(),
				centerSquares[i]);
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(m_flareShape,
				centerOfShape,
				centerSquares[i],
				m_flarePenetrateLos,
				caster,
				null,
				nonActorTargetInfo);
			if ((actorsInShape.Contains(caster) ? actorsInShape.Count - 1 : actorsInShape.Count) > 0)
			{
				numShapesWithHits++;
			}
			foreach (ActorData hitActor in actorsInShape)
			{
				bool isAlly = hitActor.GetTeam() == caster.GetTeam();
				if (!isAlly && m_includeEnemies)
				{
					if (dictionary.ContainsKey(hitActor))
					{
						dictionary[hitActor] += m_flareSubsequentDamageAmount;
						actorToHitCount[hitActor]++;
					}
					else
					{
						dictionary[hitActor] = m_flareDamageAmount;
						actorToDamageOrigin[hitActor] = centerSquares[i].ToVector3();
						actorsForSequences[i].Add(hitActor);
						actorToHitCount[hitActor] = 1;
					}
				}
				else if (isAlly
				         && m_includeAllies
				         && hitActor != caster
				         && !validAfterImages.Contains(hitActor))
				{
					Vector3 centerSquare = centerSquares[i].ToVector3();
					if (dictionary.ContainsKey(hitActor))
					{
						dictionary[hitActor] += m_flareSubsequentHealAmount;
						actorToHitCount[hitActor]++;
						ActorCover actorCover = hitActor.GetActorCover();
						if (actorCover != null
						    && actorCover.IsInCoverWrt(actorToDamageOrigin[hitActor]) // , out HitChanceBracketType _ in rogues
						    && !actorCover.IsInCoverWrt(centerSquare)) // , out HitChanceBracketType _ in rogues
						{
							actorToDamageOrigin[hitActor] = centerSquare;
						}
					}
					else
					{
						dictionary[hitActor] = m_flareHealAmount;
						actorToDamageOrigin[hitActor] = centerSquare;
						actorsForSequences[i].Add(hitActor);
						actorToHitCount[hitActor] = 1;
					}
				}
			}
		}
		return dictionary;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Dictionary<ActorData, int> actorToHpDeltaMap = GetActorToHpDeltaMap(
			targets,
			caster,
			out _,
			out _,
			out Dictionary<ActorData, Vector3> actorToDamageOrigin,
			out Dictionary<ActorData, int> actorToHitCount,
			out int numShapesWithHits,
			nonActorTargetInfo);
		bool casterProcessed = false;
		foreach (ActorData hitActor in actorToHpDeltaMap.Keys)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, actorToDamageOrigin[hitActor]));
			bool isSpawningSpoil = !m_onlySpawnSpoilOnMultiHit || actorToHitCount[hitActor] > 1;
			if (hitActor.GetTeam() != caster.GetTeam())
			{
				actorHitResults.SetBaseDamage(actorToHpDeltaMap[hitActor]);
				actorHitResults.AddStandardEffectInfo(actorToHitCount[hitActor] > 1 && m_useEnemyMultiHitEffect
					? m_enemyMultipleHitEffect
					: m_enemyHitEffect);
				if (isSpawningSpoil && m_spawnSpoilForEnemyHit)
				{
					actorHitResults.AddSpoilSpawnData(new SpoilSpawnDataForAbilityHit(hitActor, caster.GetTeam(), m_spoilSpawnInfo));
				}
			}
			else
			{
				if (hitActor == caster)
				{
					if (numShapesWithHits > 1)
					{
						actorHitResults.AddStandardEffectInfo(m_selfHitEffectForMultiHit);
					}
					casterProcessed = true;
				}
				actorHitResults.SetBaseHealing(actorToHpDeltaMap[hitActor]);
				actorHitResults.AddStandardEffectInfo(actorToHitCount[hitActor] > 1 && m_useAllyMultiHitEffect
					? m_allyMultipleHitEffect
					: m_allyHitEffect);
				if (isSpawningSpoil && m_spawnSpoilForAllyHit)
				{
					actorHitResults.AddSpoilSpawnData(new SpoilSpawnDataForAbilityHit(hitActor, caster.GetTeam(), m_spoilSpawnInfo));
				}
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (!casterProcessed
		    && m_selfHitEffectForMultiHit.m_applyEffect
		    && numShapesWithHits > 1)
		{
			ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			casterHitResults.AddStandardEffectInfo(m_selfHitEffectForMultiHit);
			abilityResults.StoreActorHit(casterHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif
}
