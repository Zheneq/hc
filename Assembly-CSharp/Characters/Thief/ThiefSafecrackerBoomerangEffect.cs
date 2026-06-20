// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class ThiefSafecrackerBoomerangEffect : Effect
{
	private Vector3 m_startPosition;
	private int m_returnDelay;
	private int m_damageAmount;
	private float m_laserWidthInSquares;
	private KnockbackType m_knockbackType;
	private float m_knockbackDistance;
	private GameObject m_groundSequencePrefab;
	private GameObject m_returnSequencePrefab;
	private int m_returnAnimationIndex;

	public ThiefSafecrackerBoomerangEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		Vector3 startPos,
		int returnDelay,
		int damageAmount,
		float laserRange, // TODO THIEF unused
		float laserWidth,
		KnockbackType knockbackType,
		float knockbackDistance,
		GameObject returnSequencePrefab,
		GameObject groundSequencePrefab,
		int returnAnimIndex)
		: base(parent, targetSquare, target, caster)
	{
		m_returnDelay = returnDelay;
		m_startPosition = startPos;
		m_damageAmount = damageAmount;
		m_laserWidthInSquares = laserWidth;
		m_knockbackType = knockbackType;
		m_knockbackDistance = knockbackDistance;
		m_groundSequencePrefab = groundSequencePrefab;
		m_returnSequencePrefab = returnSequencePrefab;
		m_returnAnimationIndex = returnAnimIndex;
		HitPhase = AbilityPriority.Combat_Knockback;
		m_time.duration = returnDelay + 1;
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(m_groundSequencePrefab, m_startPosition, null, Caster, SequenceSource);
	}

	public override ServerClientUtils.SequenceStartData GetEffectHitSeqData()
	{
		if (!ShouldTrigger())
		{
			return null;
		}
		return new ServerClientUtils.SequenceStartData(
			m_returnSequencePrefab,
			Caster.GetLoSCheckPos(),
			m_effectResults.HitActorsArray(),
			Caster,
			SequenceSource);
	}

	public override bool HitsCanBeReactedTo()
	{
		return true;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (!ShouldTrigger())
		{
			return;
		}
		List<ActorData> hitActors = GetHitActors(out List<NonActorTargetInfo> nonActorTargetInfo);
		Vector3 aimDir = Caster.GetFreePos() - m_startPosition;
		foreach (ActorData hitActor in hitActors)
		{
			ActorHitParameters hitParams = new ActorHitParameters(hitActor, m_startPosition);
			ActorHitResults actorHitResults = new ActorHitResults(m_damageAmount, HitActionType.Damage, hitParams);
			KnockbackHitData knockbackData = new KnockbackHitData(hitActor,
				Caster,
				m_knockbackType,
				aimDir,
				m_startPosition,
				m_knockbackDistance);
			actorHitResults.AddKnockbackData(knockbackData);
			effectResults.StoreActorHit(actorHitResults);
		}
		effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
	{
		if (phaseIndex == HitPhase && ShouldTrigger())
		{
			return m_returnAnimationIndex;
		}
		return 0;
	}

	private bool ShouldTrigger()
	{
		return m_time.age >= m_returnDelay && !Caster.IsDead();
	}

	private List<ActorData> GetHitActors(out List<NonActorTargetInfo> nonActorTargetInfo)
	{
		nonActorTargetInfo = new List<NonActorTargetInfo>();
		Vector3 loSCheckPos = Caster.GetLoSCheckPos();
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = m_startPosition;
		return AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			loSCheckPos - m_startPosition,
			(loSCheckPos - m_startPosition).magnitude / Board.Get().squareSize,
			m_laserWidthInSquares,
			Caster,
			Caster.GetOtherTeams(),
			true,
			-1,
			false,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() == Caster.GetTeam() || m_returnDelay <= 0)
		{
			return;
		}
		List<BoardSquare> squaresInBoxByActorRadius = AreaEffectUtils.GetSquaresInBoxByActorRadius(
			m_startPosition,
			Caster.GetLoSCheckPos(),
			m_laserWidthInSquares,
			false,
			Caster);
		squaresToAvoid.UnionWith(squaresInBoxByActorRadius);
	}
}
#endif
