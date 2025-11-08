// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using AbilityContextNamespace;
using UnityEngine;

#if SERVER
// custom
public class DinoMarkedAreaEffect : Effect
{
    private readonly List<ActorData> m_targets;
    private readonly int m_delayTurns;
    internal readonly AbilityAreaShape m_shape;
    private readonly bool m_delayedHitIgnoreLos;
    private readonly int m_extraDamage;
    private readonly OnHitAuthoredData m_delayedOnHitData;
    private readonly GameObject m_markerSeqPrefab;
    private readonly GameObject m_triggerSeqPrefab;

    private List<BoardSquare> m_targetSquares;

    public DinoMarkedAreaEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        ActorData caster,
        List<ActorData> targets,
        int delayTurns,
        AbilityAreaShape shape,
        bool delayedHitIgnoreLos,
        int extraDamage,
        OnHitAuthoredData delayedOnHitData,
        GameObject markerSeqPrefab,
        GameObject triggerSeqPrefab)
        : base(parent, targetSquare, null, caster)
    {
        m_targets = targets;
        m_delayTurns = delayTurns;
        m_shape = shape;
        m_delayedHitIgnoreLos = delayedHitIgnoreLos;
        m_extraDamage = extraDamage;
        m_delayedOnHitData = delayedOnHitData;
        m_markerSeqPrefab = markerSeqPrefab;
        m_triggerSeqPrefab = triggerSeqPrefab;

        HitPhase = AbilityPriority.Combat_Damage;
        m_time.duration = m_delayTurns + 1;
        UpdateTargetSquares();
    }

    public List<ActorData> GetTargets()
    {
        return m_targets;
    }

    public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
    {
        return m_time.age >= m_delayTurns && HitPhase == phaseIndex;
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        UpdateTargetSquares();

        if (m_targetSquares.Count > 0)
        {
            MovementResults movementResults = new MovementResults(MovementStage.INVALID);
            movementResults.SetupTriggerData(Caster, null);
            movementResults.SetupGameplayDataForAbility(Parent.Ability, Caster);
            movementResults.SetupSequenceData(
                SequenceLookup.Get().GetSimpleHitSequencePrefab(),
                Caster.GetCurrentBoardSquare(),
                SequenceSource);
            movementResults.AddSequenceStartOverride(
                new ServerClientUtils.SequenceStartData(
                    SequenceLookup.Get().GetSimpleHitSequencePrefab(),
                    new Vector3(1.0f, 1.0f, 1.0f),
                    null,
                    Caster,
                    SequenceSource),
                SequenceSource);
            foreach (BoardSquare targetSquare in m_targetSquares)
            {
                movementResults.AddSequenceStartOverride(
                    new ServerClientUtils.SequenceStartData(
                        m_markerSeqPrefab,
                        targetSquare,
                        targetSquare.OccupantActor != null ? targetSquare.OccupantActor.AsArray() : null,
                        Caster,
                        SequenceSource),
                    SequenceSource,
                    false);
            }

            movementResults.ExecuteUnexecutedMovementHits(false);
            if (ServerResolutionManager.Get() != null)
            {
                ServerResolutionManager.Get().SendNonResolutionActionToClients(movementResults);
            }
        }
    }

    public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
    {
        List<ServerClientUtils.SequenceStartData> list = base.GetEffectHitSeqDataList();
        SequenceSource source = SequenceSource.GetShallowCopy();
        source.SetWaitForClientEnable(true);
        foreach (BoardSquare targetSquare in m_targetSquares)
        {
            list.Add(
                new ServerClientUtils.SequenceStartData(
                    m_triggerSeqPrefab,
                    targetSquare,
                    GetHitActorsInShape(targetSquare).ToArray(),
                    Caster,
                    source));
        }

        return list;
    }

    private void UpdateTargetSquares()
    {
        m_targetSquares = m_targets
            .Where(actor => !actor.IsDead())
            .Select(actor => actor.GetCurrentBoardSquare())
            .ToList();
    }

    public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
    {
        if (m_time.age < m_delayTurns)
        {
            return;
        }

        HashSet<ActorData> alreadyHitActors = new HashSet<ActorData>();
        foreach (BoardSquare targetSquare in m_targetSquares)
        {
            List<ActorData> actorsInShape = GetHitActorsInShape(targetSquare);
            foreach (ActorData actorData in actorsInShape)
            {
                if (!alreadyHitActors.Add(actorData))
                {
                    continue;
                }

                BoardSquare squareOverride = m_targetSquares
                    .FirstOrDefault(s => s == actorData.GetCurrentBoardSquare());

                Vector3 origin = squareOverride != null ? squareOverride.ToVector3() : targetSquare.ToVector3(); // we ignore cover, so it doesn't really matter which one we pick
                ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, origin));
                ActorHitContext actorHitContext = new ActorHitContext();
                actorHitContext.m_contextVars.SetValue(
                    DinoMarkedAreaAttack.s_cvarInCenter.GetKey(),
                    squareOverride != null ? 1 : 0);
                GenericAbility_Container.ApplyActorHitData(
                    Caster,
                    actorData,
                    actorHitResults,
                    m_delayedOnHitData,
                    actorHitContext);
                actorHitResults.AddBaseDamage(m_extraDamage);
                effectResults.StoreActorHit(actorHitResults);
            }

            PositionHitResults posHitResult =
                new PositionHitResults(new PositionHitParameters(targetSquare.ToVector3()));
            posHitResult.AddSequenceToEnd(m_markerSeqPrefab, SequenceSource, targetSquare.ToVector3());
            effectResults.StorePositionHit(posHitResult);
        }
    }

    public override List<Vector3> CalcPointsOfInterestForCamera()
    {
        return m_targetSquares.Select(s => s.ToVector3()).ToList();
    }

    private List<ActorData> GetHitActorsInShape(BoardSquare targetSquare)
    {
        List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
            m_shape,
            targetSquare.ToVector3(),
            targetSquare,
            m_delayedHitIgnoreLos,
            Caster,
            Caster.GetOtherTeams(),
            null);
        return actorsInShape;
    }

    public int GetDelayedCenterHitDamage()
    {
        foreach (OnHitIntField field in m_delayedOnHitData.m_enemyHitIntFields)
        {
            if (field.m_hitType == OnHitIntField.HitType.Damage && field.m_baseValue > 0)
            {
                return field.m_baseValue;
            }
        }
        return 0;
    }
}
#endif