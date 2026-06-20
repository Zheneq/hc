// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
public class GremlinsLandMineEffect : Effect
{
	private int m_mineDamageAmount = 5;
	private StandardEffectInfo m_hitEffectInfo;
	private int m_mineEnergyGainOnExplosion;
	private AbilityAreaShape m_mineShape = AbilityAreaShape.Three_x_Three;
	private bool m_minePenetrateLineOfSight;
	private bool m_detonateOnTimeout = true;
	private GameObject m_mineGroundSequencePrefab;
	private GameObject m_mineExplosionSequencePrefab;
	private bool m_detonated;
	private int m_detonationTurn = -1;
	private bool m_willDetonate;

	public GremlinsLandMineEffect(
		EffectSource parent,
		ActorData caster,
		BoardSquare targetSquare,
		int duration,
		int damageAmount,
		StandardEffectInfo hitEffect,
		int energyGainOnExplosion,
		AbilityAreaShape detonateShape,
		bool detonateOnTimeout,
		bool minePenetrateLineOfSight,
		GameObject mineGroundSequencePrefab,
		GameObject mineExplosionSequencePrefab)
		: base(parent, targetSquare, null, caster)
	{
		m_effectName = "Gremlins Mine Effect";
		m_time.duration = duration;
		m_mineDamageAmount = damageAmount;
		m_hitEffectInfo = hitEffect;
		m_mineEnergyGainOnExplosion = energyGainOnExplosion;
		m_mineShape = AbilityAreaShape.SingleSquare;
		m_detonateOnTimeout = detonateOnTimeout;
		m_minePenetrateLineOfSight = minePenetrateLineOfSight;
		m_mineGroundSequencePrefab = mineGroundSequencePrefab;
		m_mineExplosionSequencePrefab = mineExplosionSequencePrefab;
		HitPhase = AbilityPriority.Combat_Damage;
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		return new ServerClientUtils.SequenceStartData(
			m_mineGroundSequencePrefab,
			TargetSquare,
			null,
			Caster,
			SequenceSource,
			new ProximityMineGroundSequence.ExtraParams
			{
				explosionRadius = 0.5f,
				visibleToEnemies = true
			}.ToArray());
	}

	private bool ShouldDetonateOnFuseTimeThisTurn()
	{
		return !m_detonated
		       && m_detonateOnTimeout
		       && m_time.duration > 0
		       && m_time.age >= m_time.duration - 1;
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		if (phase == AbilityPriority.Combat_Damage
		    && m_effectResults.HitActorsArray().Length != 0)
		{
			DetonateOnFuseTime();
			m_willDetonate = true;
		}
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly()
		       || (m_detonated && GameFlowData.Get().CurrentTurn - m_detonationTurn >= 1);
	}

	public override ServerClientUtils.SequenceStartData GetEffectHitSeqData()
	{
		return new ServerClientUtils.SequenceStartData(
			m_mineExplosionSequencePrefab,
			TargetSquare,
			m_effectResults.HitActorsArray(),
			Caster,
			SequenceSource);
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		ActorData occupantActor = TargetSquare.OccupantActor;
		if (ShouldDetonateOnFuseTimeThisTurn() || (occupantActor != null && occupantActor.GetTeam() != Caster.GetTeam()))
		{
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
				m_mineShape,
				TargetSquare.ToVector3(),
				TargetSquare,
				m_minePenetrateLineOfSight,
				Caster,
				Caster.GetOtherTeams(),
				nonActorTargetInfo);
			foreach (ActorData target in actorsInShape)
			{
				ActorHitParameters hitParams = new ActorHitParameters(target, TargetSquare.ToVector3());
				ActorHitResults actorHitResults = new ActorHitResults(m_mineDamageAmount, HitActionType.Damage, hitParams);
				actorHitResults.AddTechPointGainOnCaster(m_mineEnergyGainOnExplosion);
				actorHitResults.SetIgnoreTechpointInteractionForHit(true);
				actorHitResults.AddEffectSequenceToEnd(m_mineGroundSequencePrefab, m_guid);
				effectResults.StoreActorHit(actorHitResults);
			}
			effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
	}

	private void DetonateOnFuseTime()
	{
		m_detonated = true;
		m_detonationTurn = GameFlowData.Get().CurrentTurn;
	}

	public override void GatherMovementResults(MovementCollection movement, ref List<MovementResults> movementResultsList)
	{
		if (m_willDetonate)
		{
			return;
		}
		ServerAbilityUtils.TriggeringPathInfo triggeringPathInfo = ServerAbilityUtils.FindShortestPathCrossingOverSquare(movement, TargetSquare, Caster.GetEnemyTeam());
		if (triggeringPathInfo != null)
		{
			ActorHitParameters hitParams = new ActorHitParameters(triggeringPathInfo.m_mover, TargetSquare.ToVector3());
			ActorHitResults actorHitResults = new ActorHitResults(m_mineDamageAmount, HitActionType.Damage, m_hitEffectInfo, hitParams);
			actorHitResults.AddTechPointGainOnCaster(m_mineEnergyGainOnExplosion);
			actorHitResults.SetIgnoreTechpointInteractionForHit(true);
			actorHitResults.AddEffectForRemoval(this, ServerEffectManager.Get().WorldEffects);
			MovementResults movementResults = new MovementResults(movement.m_movementStage);
			movementResults.SetupTriggerData(triggeringPathInfo);
			movementResults.SetupGameplayData(this, actorHitResults);
			movementResults.SetupSequenceData(m_mineExplosionSequencePrefab, TargetSquare, SequenceSource);
			movementResultsList.Add(movementResults);
			m_willDetonate = true;
		}
	}

	public override void GatherMovementResultsFromSegment(
		ActorData mover,
		MovementInstance movementInstance,
		MovementStage movementStage,
		BoardSquarePathInfo sourcePath,
		BoardSquarePathInfo destPath,
		ref List<MovementResults> movementResultsList)
	{
		if (m_willDetonate
		    || mover.GetTeam() == Caster.GetTeam()
		    || !movementInstance.m_groundBased && !destPath.IsPathEndpoint()
		    || destPath.square != TargetSquare)
		{
			return;
		}
		ActorHitParameters hitParams = new ActorHitParameters(mover, TargetSquare.ToVector3());
		ActorHitResults actorHitResults = new ActorHitResults(m_mineDamageAmount, HitActionType.Damage, m_hitEffectInfo, hitParams);
		actorHitResults.AddTechPointGainOnCaster(m_mineEnergyGainOnExplosion);
		actorHitResults.SetIgnoreTechpointInteractionForHit(true);
		actorHitResults.AddEffectForRemoval(this, ServerEffectManager.Get().WorldEffects);
		MovementResults movementResults = new MovementResults(movementStage);
		movementResults.SetupTriggerData(mover, destPath);
		movementResults.SetupGameplayData(this, actorHitResults);
		movementResults.SetupSequenceData(m_mineExplosionSequencePrefab, TargetSquare, SequenceSource);
		movementResultsList.Add(movementResults);
		m_willDetonate = true;
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam())
		{
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(
				m_mineShape,
				TargetSquare.ToVector3(),
				TargetSquare,
				true,
				Caster);
			squaresToAvoid.UnionWith(squaresInShape);
		}
	}

	public override void DrawGizmos()
	{
		Gizmos.color = !m_detonated ? Color.blue : Color.red;
		Gizmos.DrawWireSphere(TargetSquare.ToVector3(), 1f);
	}
}
#endif
