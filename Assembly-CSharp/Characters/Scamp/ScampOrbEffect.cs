// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if SERVER
// custom
public class ScampOrbEffect : Effect
{
    private readonly int m_orbEnergyGainOnTrigger;
    private readonly StandardEffectInfo m_orbTriggerEffect;
    private readonly GameObject m_orbPersistentSeqPrefab;
    private readonly GameObject m_orbTriggerSeqPrefab;

    private bool m_triggered;
    
    public ScampOrbEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        ActorData caster,
        int duration,
        int orbEnergyGainOnTrigger,
        StandardEffectInfo orbTriggerEffect,
        GameObject orbPersistentSeqPrefab,
        GameObject orbTriggerSeqPrefab)
        : base(parent, targetSquare, null, caster)
    {
        m_orbEnergyGainOnTrigger = orbEnergyGainOnTrigger;
        m_orbTriggerEffect = orbTriggerEffect;
        m_orbPersistentSeqPrefab = orbPersistentSeqPrefab;
        m_orbTriggerSeqPrefab = orbTriggerSeqPrefab;

        m_time.duration = duration;
    }
    
    public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
    {
        return new ServerClientUtils.SequenceStartData(
            m_orbPersistentSeqPrefab,
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

    public ServerClientUtils.SequenceStartData GetEffectEndSeqData(SequenceSource sequenceSource)
    {
        return new ServerClientUtils.SequenceStartData(
            m_orbTriggerSeqPrefab,
            TargetSquare.ToVector3(),
            Caster.AsArray(),
            Caster,
            sequenceSource);
    }

    public override bool ShouldEndEarly()
    {
        return base.ShouldEndEarly() || m_triggered;
    }

    public override ServerClientUtils.SequenceStartData GetEffectHitSeqData()
    {
        return GetEffectEndSeqData(SequenceSource);
    }

	public override void GatherMovementResults(MovementCollection movement, ref List<MovementResults> movementResultsList)
	{
		if (m_triggered)
		{
			return;
		}

		MovementInstance movementInstance = movement.m_movementInstances.FirstOrDefault(mi => mi.m_mover == Caster);
		if (movementInstance == null)
		{
			return;
		}

		for (BoardSquarePathInfo step = movementInstance.m_path; step != null; step = step.next)
		{
			if (step.square == TargetSquare && (movementInstance.m_groundBased || step.IsPathEndpoint()))
			{
				movementResultsList.Add(MakeMovementResults(movement.m_movementStage, step));
				m_triggered = true;
				return;
			}
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
		if (!m_triggered
		    && mover == Caster
		    && (movementInstance.m_groundBased || destPath.IsPathEndpoint())
		    && destPath.square == TargetSquare)
		{
			movementResultsList.Add(MakeMovementResults(movementStage, destPath));
			m_triggered = true;
		}
	}

	private MovementResults MakeMovementResults(MovementStage movementStage, BoardSquarePathInfo step)
	{
		ActorHitParameters hitParams = new ActorHitParameters(Caster, TargetSquare.ToVector3());
		ActorHitResults actorHitResults = new ActorHitResults(hitParams);
		actorHitResults.AddTechPointGainOnCaster(m_orbEnergyGainOnTrigger);
		actorHitResults.SetIgnoreTechpointInteractionForHit(true);
		actorHitResults.AddEffectForRemoval(this);
		actorHitResults.AddStandardEffectInfo(m_orbTriggerEffect);
		MovementResults movementResults = new MovementResults(movementStage);
		movementResults.SetupTriggerData(new ServerAbilityUtils.TriggeringPathInfo(Caster, step));
		movementResults.SetupGameplayData(this, actorHitResults);
		movementResults.SetupSequenceData(m_orbTriggerSeqPrefab, TargetSquare, SequenceSource);
		return movementResults;
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam())
		{
			squaresToAvoid.Add(TargetSquare);
		}
	}
}
#endif