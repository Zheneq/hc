// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_GrydCardinalBomb : AbilityUtil_Targeter
{
    private float m_maxTrunkDist;
    private float m_maxBranchDist;
    private bool m_splitOnWall;
    private bool m_splitOnActor;
    private bool m_trunkContinueAfterActorHit;
    private int m_maxNumSplits;

    public Dictionary<ActorData, ActorMultiHitContext> m_actorToHitContext =
        new Dictionary<ActorData, ActorMultiHitContext>();

    public AbilityUtil_Targeter_GrydCardinalBomb(
        Ability ability,
        float maxTrunkDist,
        float maxBranchDist,
        bool splitOnWall,
        bool splitOnActor,
        bool continueAfterActorHit,
        int maxSplits)
        : base(ability)
    {
        m_maxTrunkDist = maxTrunkDist;
        m_maxBranchDist = maxBranchDist;
        m_splitOnWall = splitOnWall;
        m_splitOnActor = splitOnActor;
        m_trunkContinueAfterActorHit = continueAfterActorHit;
        m_maxNumSplits = maxSplits;
    }

    public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
    {
        ClearActorsInRange();
        m_actorToHitContext.Clear();
        List<GrydCardinalSegmentInfo> segments = new List<GrydCardinalSegmentInfo>();
        BoardSquare targetSquare = Board.Get().GetSquare(currentTarget.GridPos);
        int nextHighlightIndex = 0;
        if (targetSquare != null)
        {
            segments.Add(new GrydCardinalSegmentInfo(targetSquare, Vector3.forward));
            segments.Add(new GrydCardinalSegmentInfo(targetSquare, Vector3.back));
            segments.Add(new GrydCardinalSegmentInfo(targetSquare, Vector3.left));
            segments.Add(new GrydCardinalSegmentInfo(targetSquare, Vector3.right));
            ActorData actorData = AreaEffectUtils.GetTargetableActorOnSquare(targetSquare, true, false, targetingActor);
            if (actorData != null && !actorData.IsActorVisibleToClient())
            {
                actorData = null;
            }

            List<ActorData> actorsToExclude = new List<ActorData>();
            foreach (GrydCardinalSegmentInfo segment in segments)
            {
                segment.m_hitActorsMap = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
                actorsToExclude.Clear();
                GrydCardinalSegmentInfo.CalculateSegmentInfo(
                    segment,
                    m_maxTrunkDist,
                    m_maxBranchDist,
                    m_maxNumSplits,
                    m_splitOnWall,
                    m_splitOnActor,
                    m_trunkContinueAfterActorHit,
                    false,
                    0,
                    targetingActor,
#if VANILLA
                    targetingActor.GetEnemyTeamAsList(), // reactor
#else
                    targetingActor.GetOtherTeams(), // rogues
#endif
                    null,
                    segment.m_hitActorsMap,
                    actorsToExclude);
                GrydCardinalSegmentInfo.HandleTargeterHighlights(segment, false, m_highlights, ref nextHighlightIndex);
                segment.TrackActorHitInfo(m_actorToHitContext);
            }

            if (actorData != null)
            {
                if (m_actorToHitContext.ContainsKey(actorData))
                {
                    m_actorToHitContext[actorData].m_numHits++;
                }
                else
                {
                    m_actorToHitContext[actorData] = new ActorMultiHitContext
                    {
                        m_numHits = 1,
                        m_numHitsFromCover = 0,
                        m_hitOrigin = actorData.GetFreePos()
                    };
                }
            }

            foreach (KeyValuePair<ActorData, ActorMultiHitContext> hitActor in m_actorToHitContext)
            {
                AddActorInRange(hitActor.Key, hitActor.Value.m_hitOrigin, targetingActor);
            }
        }

        for (int i = nextHighlightIndex; i < m_highlights.Count; i++)
        {
            m_highlights[i].SetActive(false);
        }
    }
}