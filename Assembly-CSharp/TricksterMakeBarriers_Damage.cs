// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class TricksterMakeBarriers_Damage : Ability
{
	[Header("-- Capsule AOE")]
	public float m_rangeFromLine = 1f;
	public float m_lineEndOffset;
	public float m_radiusAroundOrigin = 1f;
	public bool m_capsulePenetrateLos;
	[Header("-- Enemy Hit Damage and Effect")]
	public int m_damageAmount = 5;
	public StandardEffectInfo m_enemyOnHitEffect;
	[Header("-- Sequences -----------------------------")]
	public GameObject m_castSequencePrefab;

	private TricksterAfterImageNetworkBehaviour m_afterImageSyncComp;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Hollusionary Wallogram - Damage";
		}
		m_sequencePrefab = m_castSequencePrefab;
		m_afterImageSyncComp = GetComponent<TricksterAfterImageNetworkBehaviour>();
		Targeter = new AbilityUtil_Targeter_TricksterBarriers(
			this,
			m_afterImageSyncComp,
			GetRangeFromLine(),
			GetLineEndOffset(),
			GetRadiusAroundOrigin(),
			GetPenetrateLos(),
			false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
		m_enemyOnHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return m_afterImageSyncComp.GetValidAfterImages().Count > 0;
	}

	public override void OnAbilityAnimationRequest(ActorData caster, int animationIndex, bool cinecam, Vector3 targetPos)
	{
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		for (int i = 0; i < validAfterImages.Count; i++)
		{
#if SERVER
			// added in rogues
			m_afterImageSyncComp.TurnToPosition(
				i == 0 ? caster : validAfterImages[i - 1],
				validAfterImages[i].GetFreePos());
#endif
			Animator modelAnimator = validAfterImages[i].GetModelAnimator();
			modelAnimator.SetInteger("Attack", animationIndex);
			modelAnimator.SetBool("CinematicCam", cinecam);
			modelAnimator.SetTrigger("StartAttack");
		}
#if SERVER
		// added in rogues
		if (validAfterImages.Count > 0)
		{
			m_afterImageSyncComp.TurnToPosition(validAfterImages[validAfterImages.Count - 1], caster.GetFreePos());
		}
#endif
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

	public float GetRangeFromLine()
	{
		return m_rangeFromLine;
	}

	public float GetLineEndOffset()
	{
		return m_lineEndOffset;
	}

	public float GetRadiusAroundOrigin()
	{
		return m_radiusAroundOrigin;
	}

	public bool GetPenetrateLos()
	{
		return m_capsulePenetrateLos;
	}

#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		List<List<ActorData>> hitActorsFromSegments = GetHitActorsFromSegments(caster, out List<VectorUtils.LaserCoords> segments, null);
		for (int i = 0; i < hitActorsFromSegments.Count; i++)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				segments[i].end,
				hitActorsFromSegments[i].ToArray(),
				caster,
				additionalData.m_sequenceSource,
				new SplineProjectileSequence.DelayedProjectileExtraParams
				{
					useOverrideStartPos = true,
					overrideStartPos = segments[i].start
				}.ToArray()));
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<List<ActorData>> hitActorsFromSegments = GetHitActorsFromSegments(caster, out List<VectorUtils.LaserCoords> segments, nonActorTargetInfo);
		for (int i = 0; i < hitActorsFromSegments.Count; i++)
		{
			foreach (ActorData target in hitActorsFromSegments[i])
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, segments[i].start));
				actorHitResults.SetBaseDamage(m_damageAmount);
				actorHitResults.AddStandardEffectInfo(m_enemyOnHitEffect);
				abilityResults.StoreActorHit(actorHitResults);
			}
		}
	}

	// added in rogues
	private List<List<ActorData>> GetHitActorsFromSegments(
		ActorData caster,
		out List<VectorUtils.LaserCoords> segments,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<List<ActorData>> list = new List<List<ActorData>>();
		segments = GetSegmentsBetweenTricksters(caster);
		HashSet<ActorData> hashSet = new HashSet<ActorData>();
		float num = GetLineEndOffset() * Board.Get().squareSize;
		foreach (VectorUtils.LaserCoords laserCoords in segments)
		{
			Vector3 vector = laserCoords.Direction();
			Vector3 startPos = laserCoords.start - vector * num;
			Vector3 endPos = laserCoords.end + vector * num;
			List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(startPos,
				endPos,
				0f,
				0f,
				GetRangeFromLine(),
				GetPenetrateLos(),
				caster,
				caster.GetOtherTeams(),
				nonActorTargetInfo);
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
				laserCoords.start,
				GetRadiusAroundOrigin(),
				GetPenetrateLos(),
				caster,
				caster.GetOtherTeams(),
				nonActorTargetInfo);
			List<ActorData> list2 = new List<ActorData>();
			foreach (ActorData item in actorsInRadiusOfLine)
			{
				if (!hashSet.Contains(item))
				{
					list2.Add(item);
					hashSet.Add(item);
				}
			}
			foreach (ActorData item2 in actorsInRadius)
			{
				if (!hashSet.Contains(item2))
				{
					list2.Add(item2);
					hashSet.Add(item2);
				}
			}
			list.Add(list2);
		}
		return list;
	}

	// added in rogues
	private List<VectorUtils.LaserCoords> GetSegmentsBetweenTricksters(ActorData caster)
	{
		List<VectorUtils.LaserCoords> list = new List<VectorUtils.LaserCoords>();
		List<ActorData> validAfterImages = m_afterImageSyncComp.GetValidAfterImages();
		int num = validAfterImages.Count > 1
			? validAfterImages.Count
			: validAfterImages.Count - 1;
		for (int i = 0; i <= num; i++)
		{
			BoardSquare src = null;
			BoardSquare dst = null;
			if (i == 0)
			{
				src = caster.GetCurrentBoardSquare();
				dst = validAfterImages[i].GetCurrentBoardSquare();
			}
			else if (i == validAfterImages.Count)
			{
				src = validAfterImages[i - 1].GetCurrentBoardSquare();
				dst = caster.GetCurrentBoardSquare();
			}
			else if (i < validAfterImages.Count)
			{
				src = validAfterImages[i - 1].GetCurrentBoardSquare();
				dst = validAfterImages[i].GetCurrentBoardSquare();
			}
			VectorUtils.LaserCoords item;
			item.start = src.ToVector3();
			item.end = dst.ToVector3();
			list.Add(item);
		}
		return list;
	}
#endif
}
