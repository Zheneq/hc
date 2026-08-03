using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BounceActor : AbilityUtil_Targeter
{
    public struct HitActorContext
    {
        public ActorData actor;
        public int segmentIndex;
    }

    public float m_width = 1f;
    public float m_maxDistancePerBounce = 15f;
    public float m_maxTotalDistance = 50f;
    public int m_maxBounces = 5;
    public int m_maxTargetsHit = 1;
    public bool m_bounceOnEnemyActor;
    public bool m_includeAlliesInBetween;

    private List<HitActorContext> m_hitActorContext = new List<HitActorContext>();
    private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

    public AbilityUtil_Targeter_BounceActor(
        Ability ability,
        float width,
        float distancePerBounce,
        float totalDistance,
        int maxBounces,
        int maxTargetsHit,
        bool bounceOnEnemyActor,
        bool includeAlliesInBetween = false,
        bool includeSelf = false)
        : base(ability)
    {
        m_width = width;
        m_maxDistancePerBounce = distancePerBounce;
        m_maxTotalDistance = totalDistance;
        m_maxBounces = maxBounces;
        m_maxTargetsHit = maxTargetsHit;
        m_bounceOnEnemyActor = bounceOnEnemyActor;
        m_includeAlliesInBetween = includeAlliesInBetween;
        m_affectsTargetingActor = includeSelf;
        m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
        m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
    }

    public List<HitActorContext> GetHitActorContext()
    {
        return m_hitActorContext;
    }

    public void SetMaxBounces(int maxBounces)
    {
        m_maxBounces = maxBounces;
    }

    public void SetMaxTargets(int maxTargets)
    {
        m_maxTargetsHit = maxTargets;
    }

    public void CreateLaserHighlights(
        Vector3 originalStart,
        List<Vector3> laserAnglePoints,
        bool showDestinationHighlight)
    {
        float y = 0.1f - BoardSquare.s_LoSHeightOffset;
        Vector3 cursorStart = originalStart + new Vector3(0f, y, 0f);
        float cursorWidth = m_width * Board.Get().squareSize;
        if (m_highlights == null || m_highlights.Count < 2)
        {
            m_highlights = new List<GameObject>
            {
                HighlightUtils.Get().CreateBouncingLaserCursor(cursorStart, laserAnglePoints, cursorWidth),
                HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, true)
            };
        }

        UIBouncingLaserCursor bouncingLaserCursor = m_highlights[0].GetComponent<UIBouncingLaserCursor>();
        bouncingLaserCursor.OnUpdated(cursorStart, laserAnglePoints, cursorWidth);

        GameObject shapeCursorObject = m_highlights[1];
        bool active = false;
        if (showDestinationHighlight)
        {
            Vector3 lastAnglePoint = laserAnglePoints[laserAnglePoints.Count - 1];
            Vector3 prevAnglePoint = laserAnglePoints.Count >= 2
                ? laserAnglePoints[laserAnglePoints.Count - 2]
                : originalStart;
            Vector3 lastSegment = lastAnglePoint - prevAnglePoint;
            float lastSegmentLen = lastSegment.magnitude;
            lastSegment.Normalize();
            Vector3 end = lastAnglePoint - Mathf.Min(0.5f, lastSegmentLen / 2f) * lastSegment;

            BoardSquare lastValidBoardSquareInLine =
                KnockbackUtils.GetLastValidBoardSquareInLine(prevAnglePoint, end, true);
            if (lastValidBoardSquareInLine != null)
            {
                active = true;
                Vector3 position = lastValidBoardSquareInLine.ToVector3();
                position.y -= 0.1f;
                shapeCursorObject.transform.position = position;
            }
        }

        shapeCursorObject.SetActive(active);
    }

    public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
    {
        Vector3 casterPos = targetingActor.GetLoSCheckPos();
        Vector3 forwardDirection = currentTarget?.AimDirection ?? targetingActor.transform.forward;
        bool bounceOnActors = m_bounceOnEnemyActor && m_maxTargetsHit != 1;
        List<Vector3> laserAnglePoints = VectorUtils.CalculateBouncingActorEndpoints(
            casterPos,
            forwardDirection,
            m_maxDistancePerBounce,
            m_maxTotalDistance,
            m_maxBounces,
            targetingActor,
            bounceOnActors,
            m_width,
            targetingActor.GetEnemyTeamAsList(),
            m_maxTargetsHit,
            out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors,
            out List<ActorData> orderedHitActors,
            false,
            null);

        ClearActorsInRange();
        m_hitActorContext.Clear();

        foreach (KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> hitActor in bounceHitActors)
        {
            AddActorInRange(hitActor.Key, hitActor.Value.m_segmentOrigin, targetingActor);
        }

        foreach (ActorData hitActor in orderedHitActors)
        {
            m_hitActorContext.Add(
                new HitActorContext
                {
                    actor = hitActor,
                    segmentIndex = bounceHitActors[hitActor].m_endpointIndex
                });
        }

        List<BoardSquare> chargePathSquares = GetChargePathSquares(
            targetingActor,
            laserAnglePoints,
            bounceHitActors,
            orderedHitActors);
        BoardSquarePathInfo chargePathFromSquareList = GetChargePathFromSquareList(targetingActor, chargePathSquares);

        int fromIndex = 0;
        EnableAllMovementArrows();

        if (chargePathFromSquareList != null)
        {
            fromIndex = AddMovementArrowWithPrevious(
                targetingActor,
                chargePathFromSquareList,
                TargeterMovementType.Movement,
                0);
        }

        SetMovementArrowEnabledFromIndex(fromIndex, false);

        if (m_maxTargetsHit > 0 && orderedHitActors.Count >= m_maxTargetsHit)
        {
            float radius = Board.Get().squareSize * AreaEffectUtils.GetActorTargetingRadius();
            Vector3 lastAnglePoint = laserAnglePoints[laserAnglePoints.Count - 1];
            Vector3 prevAnglePoint = laserAnglePoints.Count > 1
                ? laserAnglePoints[laserAnglePoints.Count - 2]
                : casterPos;
            Vector3 lastSegmentDir = (lastAnglePoint - prevAnglePoint).normalized;
            Vector3 dirToHit = orderedHitActors[orderedHitActors.Count - 1].GetFreePos() - prevAnglePoint;
            laserAnglePoints[laserAnglePoints.Count - 1] =
                prevAnglePoint + (Vector3.Dot(dirToHit, lastSegmentDir) + radius) * lastSegmentDir;
        }

        if (m_includeAlliesInBetween)
        {
            List<ActorData> orderedHitAllies = new List<ActorData>();
            Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> allyTargets =
                AreaEffectUtils.FindBouncingLaserTargets(
                    casterPos,
                    ref laserAnglePoints,
                    m_width,
                    targetingActor.GetTeamAsList(),
                    -1,
                    false,
                    targetingActor,
                    orderedHitAllies);

            foreach (KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> hitAlly in allyTargets)
            {
                AddActorInRange(
                    hitAlly.Key,
                    hitAlly.Value.m_segmentOrigin,
                    targetingActor,
                    AbilityTooltipSubject.Secondary);
            }
        }

        if (m_affectsTargetingActor)
        {
            AddActorInRange(targetingActor, targetingActor.GetFreePos(), targetingActor, AbilityTooltipSubject.Self);
        }

        CreateLaserHighlights(casterPos, laserAnglePoints, false);
        if (targetingActor == GameFlowData.Get().activeOwnedActorData)
        {
            ResetSquareIndicatorIndexToUse();
            AreaEffectUtils.OperateOnSquaresInBounceLaser(
                m_indicatorHandler,
                casterPos,
                laserAnglePoints,
                m_width,
                targetingActor,
                false);
            HideUnusedSquareIndicators();
        }
    }

    public List<BoardSquare> GetChargePathSquares(
        ActorData caster,
        List<Vector3> endPoints,
        Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> laserTargets,
        List<ActorData> orderedHitActors)
    {
        for (int i = endPoints.Count - 1; i > 0; i--)
        {
            BoardSquare lastValidBoardSquareInLine = KnockbackUtils.GetLastValidBoardSquareInLine(
                endPoints[i - 1],
                endPoints[i],
                true);

            if (lastValidBoardSquareInLine != null && lastValidBoardSquareInLine.IsValidForGameplay())
            {
                break;
            }

            endPoints.RemoveAt(i);
        }

        Vector3 lastAnglePoint = endPoints[endPoints.Count - 1];
        Vector3 prevAnglePoint = endPoints.Count >= 2
            ? endPoints[endPoints.Count - 2]
            : caster.GetLoSCheckPos();
        Vector3 lastSegmentDir = lastAnglePoint - prevAnglePoint;
        float lastSegmentLen = lastSegmentDir.magnitude;
        lastSegmentDir.Normalize();
        Vector3 testVector = lastAnglePoint - Mathf.Min(0.5f, lastSegmentLen / 2f) * lastSegmentDir;

        BoardSquare endSquare;
        float adjustment = 0f;
        if (m_maxTargetsHit > 0 && laserTargets.Count >= m_maxTargetsHit)
        {
            endSquare = orderedHitActors[orderedHitActors.Count - 1].GetCurrentBoardSquare();
            adjustment = -0.5f;
        }
        else
        {
            endSquare = KnockbackUtils.GetLastValidBoardSquareInLine(prevAnglePoint, testVector, true);
        }

        if (endSquare != null && endSquare != caster.GetCurrentBoardSquare())
        {
            float dist = VectorUtils.HorizontalPlaneDistInWorld(prevAnglePoint, endSquare.ToVector3());
            float maxDistance = Mathf.Max(0f, dist + adjustment);
            endSquare = KnockbackUtils.GetLastValidBoardSquareInLine(
                prevAnglePoint,
                lastAnglePoint,
                true,
                false,
                maxDistance);
        }

        List<BoardSquare> list = new List<BoardSquare>();
        if (endSquare != null)
        {
            list.Add(caster.GetCurrentBoardSquare());
            for (int i = 0; i < endPoints.Count; i++)
            {
                Vector3 start = i == 0
                    ? list[i].ToVector3()
                    : endPoints[i - 1];
                Vector3 end = endPoints[i];

                BoardSquare startSquare = list[list.Count - 1];

                Vector3 dir = end - start;
                dir.y = 0f;
                dir.Normalize();

                Vector3 halfDir = dir / 2f;
                if (i > 0)
                {
                    BoardSquare square1 = Board.Get().GetSquareFromVec3(start + halfDir);
                    if (square1 != null
                        && square1 != startSquare
                        && square1.IsValidForGameplay())
                    {
                        list.Add(square1);
                        startSquare = square1;
                    }
                }

                if (i == endPoints.Count - 1)
                {
                    if (endSquare != startSquare)
                    {
                        list.Add(endSquare);
                    }

                    continue;
                }

                BoardSquare square2 = Board.Get().GetSquareFromVec3(end - halfDir);
                if (square2 != null && !square2.IsValidForGameplay())
                {
                    BoardSquare lastValidSquare = KnockbackUtils.GetLastValidBoardSquareInLine(start, end, true);
                    if (lastValidSquare != null && lastValidSquare.IsValidForGameplay())
                    {
                        square2 = lastValidSquare;
                    }
                }

                if (square2 != null && square2 != startSquare)
                {
                    list.Add(square2);
                }
            }

            ActorData occupantActor = endSquare.OccupantActor;
            if (occupantActor != null
                && occupantActor != caster
                && occupantActor.IsActorVisibleToClient())
            {
                Vector3 testDir = prevAnglePoint - endSquare.ToVector3();
                testDir.y = 0f;
                testDir.Normalize();

                BoardSquare secondToLastInOrigPath = null;
                if (list.Count > 1)
                {
                    secondToLastInOrigPath = list[list.Count - 2];
                }

                BoardSquare endSquareForOccupant = GetEndSquareForOccupant(
                    endSquare,
                    testDir,
                    caster,
                    secondToLastInOrigPath);
                list.Add(endSquareForOccupant);
            }
        }
        else
        {
            list.Add(caster.GetCurrentBoardSquare());
        }

        if (list.Count == 1)
        {
            list.Add(caster.GetCurrentBoardSquare());
        }

        return list;
    }

    private BoardSquare GetEndSquareForOccupant(
        BoardSquare lastSquare,
        Vector3 testDir,
        ActorData caster,
        BoardSquare secondToLastInOrigPath)
    {
        BoardSquare bestSquare = null;
        float bestScore = -1f;

        for (int i = 0; i < 3; i++)
        {
            if (bestSquare != null)
            {
                break;
            }

            List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(lastSquare, i, true);
            foreach (var squareInBorderLayer in squaresInBorderLayer)
            {
                if (!squareInBorderLayer.IsValidForGameplay())
                {
                    continue;
                }

                if (squareInBorderLayer.OccupantActor != null
                    && squareInBorderLayer.OccupantActor != caster
                    && squareInBorderLayer.OccupantActor.IsActorVisibleToClient())
                {
                    continue;
                }

                if (!KnockbackUtils.CanBuildStraightLineChargePath(
                        caster,
                        squareInBorderLayer,
                        lastSquare,
                        false,
                        out int _))
                {
                    continue;
                }

                Vector3 recoveryDir = squareInBorderLayer.ToVector3() - lastSquare.ToVector3();
                recoveryDir.y = 0f;
                recoveryDir.Normalize();

                float score = Vector3.Dot(testDir, recoveryDir);
                if (secondToLastInOrigPath != null && secondToLastInOrigPath == squareInBorderLayer)
                {
                    score += 0.5f;
                }

                if (lastSquare.GetLOS(squareInBorderLayer.x, squareInBorderLayer.y))
                {
                    score -= 2f;
                }

                if (bestSquare == null || score > bestScore)
                {
                    bestSquare = squareInBorderLayer;
                    bestScore = score;
                }
            }
        }

        if (bestSquare == null)
        {
            bestSquare = lastSquare;
        }

        return bestSquare;
    }

    private BoardSquarePathInfo GetChargePathFromSquareList(ActorData charger, List<BoardSquare> squaresInPath)
    {
        BoardSquarePathInfo path = null;
        for (int i = 1; i < squaresInPath.Count; i++)
        {
            BoardSquare dest = squaresInPath[i];
            BoardSquare start = squaresInPath[i - 1];
            BoardSquarePathInfo step = KnockbackUtils.BuildStraightLineChargePath(charger, dest, start, true);

            // debug?
            // for (BoardSquarePathInfo boardSquarePathInfo3 = boardSquarePathInfo2;
            //      boardSquarePathInfo3 != null;
            //      boardSquarePathInfo3 = boardSquarePathInfo3.next)
            // {
            // }

            if (path != null)
            {
                BoardSquarePathInfo curEndpoint = path.GetPathEndpoint();
                if (step != null
                    && step.next != null
                    && curEndpoint.square == step.square)
                {
                    curEndpoint.m_unskippable = true;
                    curEndpoint.next = step.next;
                    step.next.prev = curEndpoint;
                }
            }
            else
            {
                path = step;
            }
        }

        return path;
    }
}