// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

public class GrydCardinalSegmentInfo
{
    public BoardSquare m_startSquare;
    public BoardSquare m_endSquare;
    public Vector3 m_direction;
    public List<GrydCardinalSegmentInfo> m_childSegments;
    public int m_segmentIndex = -1;
    public int m_parentSegIndex = -1;
    public List<NonActorTargetInfo> m_nonActorTargetInfo;
    public Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> m_hitActorsMap;

    public GrydCardinalSegmentInfo(BoardSquare startSquare, Vector3 direction)
    {
        m_startSquare = startSquare;
        m_direction = direction;
        m_childSegments = new List<GrydCardinalSegmentInfo>();
    }

    public bool IsValidSegment()
    {
        return m_endSquare != null && m_endSquare != m_startSquare;
    }

    public void TrackActorHitInfo(Dictionary<ActorData, ActorMultiHitContext> actorToHitContext)
    {
        if (m_hitActorsMap == null)
        {
            return;
        }

        foreach (KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> hitActorToLaserInfo in m_hitActorsMap)
        {
            ActorData hitActor = hitActorToLaserInfo.Key;
            AreaEffectUtils.BouncingLaserInfo laserInfo = hitActorToLaserInfo.Value;
            Vector3 segmentOrigin = laserInfo.m_segmentOrigin;
            ActorCover actorCover = hitActor.GetActorCover();
            // reactor
            bool isInCover = actorCover.IsInCoverWrt(segmentOrigin);
            // rogues
            // bool isInCover = actorCover.IsInCoverWrt(segmentOrigin, out HitChanceBracketType hitChanceBracketType);
            if (actorToHitContext.ContainsKey(hitActor))
            {
                actorToHitContext[hitActor].m_numHits++;
                if (isInCover)
                {
                    actorToHitContext[hitActor].m_numHitsFromCover++;
                }

                if (actorCover != null
                    // reactor
                    && !actorCover.IsInCoverWrt(actorToHitContext[hitActor].m_hitOrigin)
                    && actorCover.IsInCoverWrt(segmentOrigin))
                    // rogues
                    // && !actorCover.IsInCoverWrt(actorToHitContext[hitActor].m_hitOrigin, out hitChanceBracketType)
                    // && actorCover.IsInCoverWrt(segmentOrigin, out hitChanceBracketType))
                {
                    actorToHitContext[hitActor].m_hitOrigin = segmentOrigin;
                }
            }
            else
            {
                actorToHitContext[hitActor] = new ActorMultiHitContext
                {
                    m_numHits = 1,
                    m_numHitsFromCover = isInCover ? 1 : 0,
                    m_hitOrigin = segmentOrigin
                };
            }
        }
    }

    public static void CalculateSegmentInfo(
        GrydCardinalSegmentInfo parentSegment,
        float maxDistInSquares,
        float maxBranchDistInSquares,
        int splitsRemaining,
        bool splitOnWall,
        bool splitOnActor,
        bool continueAfterActorHit,
        bool includeInvisibles,
        int segmentIndex,
        ActorData caster,
        List<Team> relevantTeams,
        List<NonActorTargetInfo> nonActorTargetInfo,
        Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> hitActorsMap,
        List<ActorData> actorsToExclude)
    {
        Vector3 initialStartPos = parentSegment.m_startSquare.ToVector3();
        parentSegment.m_segmentIndex = segmentIndex;
        initialStartPos.y = Board.Get().LosCheckHeight;
        float maxDistInWorld = maxDistInSquares * Board.SquareSizeStatic;
        Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(
            initialStartPos,
            parentSegment.m_direction,
            maxDistInWorld,
            false,
            caster,
            nonActorTargetInfo);
        Vector3 dir = laserEndPoint - initialStartPos;
        float dist = dir.magnitude;
        dir.Normalize();
        bool isWallHit = dist < maxDistInWorld - 0.1f;
        BoardSquare lastSquare = KnockbackUtils.GetLastValidBoardSquareInLine(initialStartPos, laserEndPoint, true);
        if (lastSquare == null || lastSquare == parentSegment.m_startSquare)
        {
            lastSquare = parentSegment.m_startSquare;
            return;
        }

        bool isSplitting = splitOnWall && isWallHit;
        int maxTargets = continueAfterActorHit ? -1 : 1;
        Vector3 startPos = initialStartPos;
        if (segmentIndex == 0)
        {
            startPos += 0.49f * Board.SquareSizeStatic * dir;
        }

        List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
            startPos,
            dir,
            maxDistInWorld / Board.SquareSizeStatic,
            0.5f,
            caster,
            relevantTeams,
            false,
            maxTargets,
            false,
            includeInvisibles,
            out _,
            nonActorTargetInfo,
            actorsToExclude,
            true);
        BoardSquare startSquare = lastSquare;
        if (actorsInLaser.Count > 0 && splitOnActor)
        {
            startSquare = actorsInLaser[0].GetCurrentBoardSquare();
            isSplitting = true;
        }

        if (actorsInLaser.Count > 0 && !continueAfterActorHit)
        {
            lastSquare = actorsInLaser[0].GetCurrentBoardSquare();
        }

        foreach (ActorData actorData in actorsInLaser)
        {
            if (!hitActorsMap.ContainsKey(actorData))
            {
                AreaEffectUtils.BouncingLaserInfo bouncingLaserInfo = new AreaEffectUtils.BouncingLaserInfo(
                    initialStartPos,
                    segmentIndex);
                hitActorsMap.Add(actorData, bouncingLaserInfo);
                actorsToExclude.Add(actorData);
            }
        }

        parentSegment.m_endSquare = lastSquare;
        if (splitsRemaining > 0 && isSplitting)
        {
            Vector3 left = Quaternion.AngleAxis(-90f, Vector3.up) * parentSegment.m_direction;
            GrydCardinalSegmentInfo segmentLeft = new GrydCardinalSegmentInfo(startSquare, left);
            CalculateSegmentInfo(
                segmentLeft,
                maxBranchDistInSquares,
                maxBranchDistInSquares,
                splitsRemaining - 1,
                splitOnWall,
                splitOnActor,
                continueAfterActorHit,
                includeInvisibles,
                segmentIndex + 1,
                caster,
                relevantTeams,
                nonActorTargetInfo,
                hitActorsMap,
                actorsToExclude);
            Vector3 right = Quaternion.AngleAxis(90f, Vector3.up) * parentSegment.m_direction;
            GrydCardinalSegmentInfo segmentRight = new GrydCardinalSegmentInfo(startSquare, right);
            CalculateSegmentInfo(
                segmentRight,
                maxBranchDistInSquares,
                maxBranchDistInSquares,
                splitsRemaining - 1,
                splitOnWall,
                splitOnActor,
                continueAfterActorHit,
                includeInvisibles,
                segmentIndex + 2,
                caster,
                relevantTeams,
                nonActorTargetInfo,
                hitActorsMap,
                actorsToExclude);
            segmentLeft.m_parentSegIndex = segmentIndex;
            segmentRight.m_parentSegIndex = segmentIndex;
            parentSegment.m_childSegments.Add(segmentLeft);
            parentSegment.m_childSegments.Add(segmentRight);
        }

        Debug.DrawLine(parentSegment.m_startSquare.ToVector3(), parentSegment.m_endSquare.ToVector3(), Color.red, 3f);
    }

    public static void AssembleSequenceParamData(
        GrydCardinalSegmentInfo parentSegment,
        List<GrydCardinalBombSequence.SegmentDataEntry> segmentDataList)
    {
        if (!parentSegment.IsValidSegment())
        {
            return;
        }

        segmentDataList.Add(
            new GrydCardinalBombSequence.SegmentDataEntry
            {
                m_segmentIndex = (sbyte)parentSegment.m_segmentIndex,
                m_prevSegmentIndex = (sbyte)parentSegment.m_parentSegIndex,
                m_startSquare = parentSegment.m_startSquare,
                m_endSquare = parentSegment.m_endSquare
            });
        foreach (GrydCardinalSegmentInfo childSegment in parentSegment.m_childSegments)
        {
            AssembleSequenceParamData(childSegment, segmentDataList);
        }
    }

    public static void EncapsulateBoundForCamPos(GrydCardinalSegmentInfo parentSegment, ref Bounds bound)
    {
        if (!parentSegment.IsValidSegment())
        {
            return;
        }

        bound.Encapsulate(parentSegment.m_startSquare.ToVector3());
        bound.Encapsulate(parentSegment.m_endSquare.ToVector3());
        foreach (GrydCardinalSegmentInfo childSegment in parentSegment.m_childSegments)
        {
            EncapsulateBoundForCamPos(childSegment, ref bound);
        }
    }

    public static void HandleTargeterHighlights(
        GrydCardinalSegmentInfo parentSegment,
        bool thinnerLine,
        List<GameObject> highlights,
        ref int nextHighlightIndex)
    {
        if (!parentSegment.IsValidSegment())
        {
            return;
        }

        Vector3 startPos = parentSegment.m_startSquare.ToVector3();
        Vector3 dir = parentSegment.m_endSquare.ToVector3() - startPos;
        dir.y = 0f;
        float dist = dir.magnitude;
        GameObject highlightObject;
        if (highlights.Count <= nextHighlightIndex)
        {
            highlightObject = HighlightUtils.Get().CreateRectangularCursor(1f, dist);
            highlights.Add(highlightObject);
        }
        else
        {
            highlightObject = highlights[nextHighlightIndex];
            highlightObject.SetActive(true);
        }

        nextHighlightIndex++;
        float lineWidth = Board.SquareSizeStatic;
        if (thinnerLine)
        {
            lineWidth *= 0.5f;
        }

        HighlightUtils.Get().ResizeRectangularCursor(lineWidth, dist, highlightObject);
        startPos.y = HighlightUtils.GetHighlightHeight();
        highlightObject.transform.position = startPos;
        highlightObject.transform.rotation = Quaternion.LookRotation(parentSegment.m_direction);
        foreach (GrydCardinalSegmentInfo childSegment in parentSegment.m_childSegments)
        {
            HandleTargeterHighlights(childSegment, true, highlights, ref nextHighlightIndex);
        }
    }
}