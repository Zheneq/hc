// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// custom
public class DinoTargetedKnockbackPassiveEffect : Effect
{
    private readonly Passive_Dino m_passive;
    private readonly bool m_doHitsAroundKnockbackDest;
    private readonly AbilityAreaShape m_shape;
    private readonly OnHitAuthoredData m_hitData;
    private readonly GameObject m_onKnockbackDestHitSeqPrefab;

    public DinoTargetedKnockbackPassiveEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        ActorData caster,
        Passive_Dino passive,
        bool doHitsAroundKnockbackDest,
        AbilityAreaShape shape,
        OnHitAuthoredData hitData,
        GameObject onKnockbackDestHitSeqPrefab)
        : base(parent, targetSquare, null, caster)
    {
        m_passive = passive;
        m_doHitsAroundKnockbackDest = doHitsAroundKnockbackDest;
        m_shape = shape;
        m_hitData = hitData;
        m_onKnockbackDestHitSeqPrefab = onKnockbackDestHitSeqPrefab;
        m_time.duration = 0;
    }

    public override void GatherMovementResults(
        MovementCollection movement,
        ref List<MovementResults> movementResultsList)
    {
        base.GatherMovementResults(movement, ref movementResultsList);

        if (movement.m_movementStage != MovementStage.Knockback
            || m_passive == null
            || m_passive.GetActorsPendingKnockback().IsNullOrEmpty())
        {
            return;
        }
        // TODO DINO ServerKnockbackManager.GetKnockbackSourceActorsOnTarget()? No ability info though

        foreach (MovementInstance movementInstance in movement.m_movementInstances)
        {
            if (movementInstance == null
                || movementInstance.m_path == null
                || !m_passive.GetActorsPendingKnockback().Contains(movementInstance.m_mover))
            {
                continue;
            }

            BoardSquarePathInfo pathEndpoint = movementInstance.m_path.GetPathEndpoint();
            BoardSquare targetSquare = pathEndpoint.square;
            ActorData mover = movementInstance.m_mover;
            ServerAbilityUtils.TriggeringPathInfo triggeringPathInfo =
                new ServerAbilityUtils.TriggeringPathInfo(mover, pathEndpoint);

            MovementResults movementResults = new MovementResults(movement.m_movementStage);
            movementResults.SetupTriggerData(triggeringPathInfo);
            movementResults.SetupGameplayData(this, null);
            movementResults.SetupSequenceData(m_onKnockbackDestHitSeqPrefab, targetSquare, SequenceSource);
            movementResultsList.Add(movementResults);

            if (m_doHitsAroundKnockbackDest)
            {
                List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
                    m_shape,
                    targetSquare.ToVector3(),
                    targetSquare,
                    true,
                    Caster,
                    Caster.GetOtherTeams(),
                    null);

                foreach (ActorData hitActor in actorsInShape)
                {
                    if (hitActor == mover)
                    {
                        continue;
                    }

                    ActorHitParameters hitParams = new ActorHitParameters(hitActor, targetSquare.ToVector3());
                    ActorHitResults actorHitResults = new ActorHitResults(hitParams);
                    GenericAbility_Container.ApplyActorHitData(Caster, hitActor, actorHitResults, m_hitData);
                    movementResults.GetEffectResults().StoreActorHit(actorHitResults);
                }
            }
        }
    }
}
#endif