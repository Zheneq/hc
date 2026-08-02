// SERVER
// ROGUES
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class VectorUtils
{
    public struct LaserCoords
    {
        public Vector3 start;
        public Vector3 end;

        public float Length()
        {
            return Vector3.Magnitude(start - end);
        }

        public Vector3 Direction()
        {
            return (end - start).normalized;
        }
    }

    private static float s_positionOffset = 0.3f;
    public static float s_laserOffset = 0.3f;
    private static float s_laserInitialLengthOffset = 0.71f;
    public static int s_raycastLayerLineOfSight = LayerMask.NameToLayer("LineOfSight"); // private in rogues
    public static int s_raycastLayerDynamicLineOfSight = LayerMask.NameToLayer("DynamicLineOfSight"); // private in rogues

    public static ActorCover.CoverDirections GetCoverDirection(BoardSquare srcSquare, BoardSquare destSquare)
    {
        int x = srcSquare.x;
        int y = srcSquare.y;
        int x2 = destSquare.x;
        int y2 = destSquare.y;
        if (Mathf.Abs(x - x2) > Mathf.Abs(y - y2))
        {
            return x > x2
                ? ActorCover.CoverDirections.X_NEG
                : ActorCover.CoverDirections.X_POS;
        }

        return y > y2
            ? ActorCover.CoverDirections.Y_NEG
            : ActorCover.CoverDirections.Y_POS;
    }

    private static BoardSquare GetAdjSquare(BoardSquare square, ActorCover.CoverDirections direction)
    {
        switch (direction)
        {
            case ActorCover.CoverDirections.X_POS:
                return Board.Get().GetSquareFromIndex(square.x + 1, square.y);
            case ActorCover.CoverDirections.X_NEG:
                return Board.Get().GetSquareFromIndex(square.x - 1, square.y);
            case ActorCover.CoverDirections.Y_POS:
                return Board.Get().GetSquareFromIndex(square.x, square.y + 1);
            case ActorCover.CoverDirections.Y_NEG:
                return Board.Get().GetSquareFromIndex(square.x, square.y - 1);
        }

        return null;
    }

    public static bool HasCoverInDirection(BoardSquare square, ActorCover.CoverDirections coverDirection)
    {
        BoardSquare adjSquare = GetAdjSquare(square, coverDirection);
        return adjSquare != null && adjSquare.height - square.height > 1f;
    }

    private static bool IsSuitableAdditionalCoverSquare(
        BoardSquare src,
        ActorCover.CoverDirections adjDirection,
        ActorCover.CoverDirections coverDirection)
    {
        if (HasCoverInDirection(src, adjDirection))
        {
            return false;
        }
        
        BoardSquare adjSquare = GetAdjSquare(src, adjDirection);
        return adjSquare != null && !HasCoverInDirection(adjSquare, coverDirection);
    }

    // TODO unused?
    private static List<BoardSquare> GetAdditionalCoverSquares(BoardSquare src, BoardSquare dst)
    {
        List<BoardSquare> result = new List<BoardSquare>();
        ActorCover.CoverDirections coverDirection = GetCoverDirection(src, dst);
        result.Add(src);
        if (HasCoverInDirection(src, coverDirection))
        {
            if (coverDirection == ActorCover.CoverDirections.X_NEG
                || coverDirection == ActorCover.CoverDirections.X_POS)
            {
                if (IsSuitableAdditionalCoverSquare(src, ActorCover.CoverDirections.Y_NEG, coverDirection))
                {
                    result.Add(GetAdjSquare(src, ActorCover.CoverDirections.Y_NEG));
                }

                if (IsSuitableAdditionalCoverSquare(src, ActorCover.CoverDirections.Y_POS, coverDirection))
                {
                    result.Add(GetAdjSquare(src, ActorCover.CoverDirections.Y_POS));
                }
            }
            else if (coverDirection == ActorCover.CoverDirections.Y_NEG
                     || coverDirection == ActorCover.CoverDirections.Y_POS)
            {
                if (IsSuitableAdditionalCoverSquare(src, ActorCover.CoverDirections.X_NEG, coverDirection))
                {
                    result.Add(GetAdjSquare(src, ActorCover.CoverDirections.X_NEG));
                }

                if (IsSuitableAdditionalCoverSquare(src, ActorCover.CoverDirections.X_POS, coverDirection))
                {
                    result.Add(GetAdjSquare(src, ActorCover.CoverDirections.X_POS));
                }
            }
        }

        return result;
    }

    public static bool HasLineOfSightFromIndex(
        int startX,
        int startY,
        int endX,
        int endY,
        Board board,
        float heightOffset,
        string layerName)
    {
        float squareSize = board.squareSize;
        float startHeight = board.GetHeightAt(startX, startY) < 0f
            ? heightOffset + board.BaselineHeight
            : heightOffset + board.GetHeightAt(startX, startY);

        float endHeight = board.GetHeightAt(endX, endY) < 0f
            ? heightOffset + board.BaselineHeight
            : heightOffset + board.GetHeightAt(endX, endY);

        Vector3 startPos = new Vector3(startX * squareSize, startHeight, startY * squareSize);
        Vector3 endPos = new Vector3(endX * squareSize, endHeight, endY * squareSize);
        Vector3[] startPoints = new Vector3[3];
        Vector3[] endPoints = new Vector3[3];

        if (Mathf.Abs(startX - endX) > Mathf.Abs(startY - endY))
        {
            Vector3 adjustment = new Vector3(0f, 0f, squareSize * s_positionOffset);
            startPoints[0] = startPos - adjustment;
            startPoints[1] = startPos;
            startPoints[2] = startPos + adjustment;
            endPoints[0] = endPos - adjustment;
            endPoints[1] = endPos;
            endPoints[2] = endPos + adjustment;
        }
        else
        {
            Vector3 adjustment = new Vector3(squareSize * s_positionOffset, 0f, 0f);
            startPoints[0] = startPos - adjustment;
            startPoints[1] = startPos;
            startPoints[2] = startPos + adjustment;
            endPoints[0] = endPos - adjustment;
            endPoints[1] = endPos;
            endPoints[2] = endPos + adjustment;
        }

        foreach (Vector3 startPoint in startPoints)
        {
            foreach (Vector3 endPoint in endPoints)
            {
                if (HasLineOfSight(startPoint, endPoint, layerName))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool HasLineOfSight(Vector3 startPos, Vector3 endPos, string layerName)
    {
        Vector3 direction = endPos - startPos;
        float magnitude = direction.magnitude;
        direction.Normalize();
        LayerMask mask = (1 << LayerMask.NameToLayer(layerName)) | (1 << s_raycastLayerDynamicLineOfSight);
        return !Physics.Raycast(startPos, direction, out _, magnitude, mask);
    }

    public static float GetLineOfSightPercentDistance(
        int startX,
        int startY,
        int endX,
        int endY,
        Board board,
        float heightOffset,
        string layerName)
    {
        float squareSize = board.squareSize;
        float startHeight = board.GetHeightAt(startX, startY) < 0f
            ? heightOffset + board.BaselineHeight
            : heightOffset + board.GetHeightAt(startX, startY);

        float endHeight = board.GetHeightAt(endX, endY) < 0f
            ? heightOffset + board.BaselineHeight
            : heightOffset + board.GetHeightAt(endX, endY);

        Vector3 startPos = new Vector3(startX * squareSize, startHeight, startY * squareSize);
        Vector3 endPos = new Vector3(endX * squareSize, endHeight, endY * squareSize);
        Vector3[] startPoints = new Vector3[3];
        Vector3[] endPoints = new Vector3[3];
        
        if (Mathf.Abs(startX - endX) > Mathf.Abs(startY - endY))
        {
            Vector3 adjustment = new Vector3(0f, 0f, squareSize * s_positionOffset);
            startPoints[0] = startPos - adjustment;
            startPoints[1] = startPos;
            startPoints[2] = startPos + adjustment;
            endPoints[0] = endPos - adjustment;
            endPoints[1] = endPos;
            endPoints[2] = endPos + adjustment;
        }
        else
        {
            Vector3 adjustment = new Vector3(squareSize * s_positionOffset, 0f, 0f);
            startPoints[0] = startPos - adjustment;
            startPoints[1] = startPos;
            startPoints[2] = startPos + adjustment;
            endPoints[0] = endPos - adjustment;
            endPoints[1] = endPos;
            endPoints[2] = endPos + adjustment;
        }

        float result = 0f;
        foreach (Vector3 startPoint in startPoints)
        {
            foreach (Vector3 endPoint in endPoints)
            {
                float lineOfSightPercentDistance = GetLineOfSightPercentDistance(startPoint, endPoint, layerName);
                if (lineOfSightPercentDistance > result)
                {
                    result = lineOfSightPercentDistance;
                    if (result == 1f)
                    {
                        break;
                    }
                }
            }

            if (result == 1f)
            {
                break;
            }
        }

        return result;
    }

    private static float GetLineOfSightPercentDistance(Vector3 startPos, Vector3 endPos, string layerName)
    {
        Vector3 direction = endPos - startPos;
        float magnitude = direction.magnitude;
        direction.Normalize();
        LayerMask mask = (1 << LayerMask.NameToLayer(layerName)) | (1 << s_raycastLayerDynamicLineOfSight);
        return Physics.Raycast(startPos, direction, out RaycastHit hitInfo, magnitude, mask)
            ? hitInfo.distance / magnitude
            : 1f;
    }

    public static LaserCoords GetLaserCoordinates(
        Vector3 startPos,
        Vector3 dir,
        float maxDistanceInWorld,
        float widthInWorld,
        bool penetrateLoS,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo = null)
    {
        return new LaserCoords
        {
            start = startPos,
            end = GetLaserEndPoint(startPos, dir, maxDistanceInWorld, penetrateLoS, caster, nonActorTargetInfo)
        };
    }

    public static Vector3 GetLaserEndPoint(
        Vector3 startPos,
        Vector3 dir,
        float maxDistanceInWorld,
        bool penetrateLoS,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo = null,
        bool checkBarriers = true)
    {
        dir.y = 0f;
        if (penetrateLoS)
        {
            return startPos + dir * maxDistanceInWorld;
        }

        
        float offsetInWorld = s_laserOffset * Board.Get().squareSize;
        float initialDistance = s_laserInitialLengthOffset * Board.Get().squareSize;
        
        Vector3 right = Vector3.Cross(Vector3.up, dir);
        right.Normalize();
        right *= offsetInWorld;
        
        Vector3[] startTestPoints = {
            startPos,
            startPos + right,
            startPos - right
        };
        
        float distanceSquared = 0f;
        bool hasAbilityBlockingBarriers = BarrierManager.Get() != null
                                          && BarrierManager.Get().HasAbilityBlockingBarriers(); // moved in rogues
        bool isObscured = true;
        bool hasLos = false; // removed in rogues
        Vector3 initialEndPoint = startTestPoints[0] + initialDistance * dir;
        
        if (maxDistanceInWorld > initialDistance)
        {
            foreach (Vector3 startTestPoint in startTestPoints)
            {
                Vector3 testDir = initialEndPoint - startTestPoint;
                float magnitude = testDir.magnitude;
                testDir.Normalize();
                
                if (RaycastInDirection(startTestPoint, testDir, magnitude, out RaycastHit hit))
                {
                    isObscured = false;

                    // removed in rogues
                    if ((hit.collider.gameObject.layer & s_raycastLayerDynamicLineOfSight) != 0)
                    {
                        hasLos = true;
                    }
                    // end removed in rogues

                    break;
                }
            }
        }

        List<NonActorTargetInfo> nonActorTargetInfos = null;
        if (maxDistanceInWorld > initialDistance && isObscured)
        {
            Vector3 initialEndPoint2 = startTestPoints[0] + initialDistance * dir;
            Vector3 lineEndPoint = GetLineEndPoint(initialEndPoint2, dir, maxDistanceInWorld - initialDistance);
            float dist = (initialEndPoint2 - lineEndPoint).magnitude + initialDistance;
            float distSquared = dist * dist;
            if (distSquared > distanceSquared)
            {
                distanceSquared = distSquared;
            }
        }
        else
        {
            foreach (Vector3 startTestPoint in startTestPoints)
            {
                List<NonActorTargetInfo> list2 = nonActorTargetInfo != null
                    ? new List<NonActorTargetInfo>()
                    : null;
                Vector3 vector4 = GetLineEndPoint(startTestPoint, dir, maxDistanceInWorld);
                if (checkBarriers && hasAbilityBlockingBarriers)
                {
                    vector4 = BarrierManager.Get().GetAbilityLineEndpoint(
                        caster,
                        startTestPoint,
                        vector4,
                        out bool _,
                        out Vector3 _,
                        list2);
                }

                float sqrMagnitude = (startTestPoint - vector4).sqrMagnitude;
                if (sqrMagnitude > distanceSquared)
                {
                    Vector3 b3 = startTestPoint - startTestPoints[0];
                    Vector3 vector5 = (startTestPoint + vector4) / 2f - b3;

                    // removed in rogues
                    if (hasLos)
                    {
                        vector5 = startPos + (initialDistance + 0.3f) * dir;
                    }
                    // end removed in rogues

                    float maxDistance = Mathf.Max(0f, (vector5 - startPos).magnitude - initialDistance);
                    Vector3 lineEndPoint2 = GetLineEndPoint(vector5, -dir, maxDistance);
                    float maxDistance2 = maxDistanceInWorld - (lineEndPoint2 - startPos).magnitude;
                    Vector3 lineEndPoint3 = GetLineEndPoint(lineEndPoint2, dir, maxDistance2);
                    float sqrMagnitude2 = (startTestPoints[0] - lineEndPoint3).sqrMagnitude;
                    float num7 = Mathf.Min(sqrMagnitude, sqrMagnitude2);
                    if (num7 > distanceSquared)
                    {
                        distanceSquared = num7;
                        nonActorTargetInfos = list2;
                    }
                    else if (Mathf.Approximately(num7, distanceSquared)
                             && nonActorTargetInfos != null
                             && nonActorTargetInfos.Count == 0)
                    {
                        nonActorTargetInfos = list2;
                    }
                }
            }
        }

        float distance = Mathf.Sqrt(distanceSquared);
        if (distance < maxDistanceInWorld - 0.1f)
        {
            distance = Mathf.Max(0f, distance - 0.05f);
        }

        Vector3 result = startPos + dir * distance;
        if (BarrierManager.Get() != null && checkBarriers)
        {
            result = BarrierManager.Get().GetAbilityLineEndpoint(
                caster,
                startPos,
                result,
                out bool collision2,
                out Vector3 _,
                nonActorTargetInfo);
            if (!collision2 && nonActorTargetInfo != null && nonActorTargetInfos != null)
            {
                nonActorTargetInfo.AddRange(nonActorTargetInfos);
            }
        }

        return result;
    }

    public static Vector3 GetLineEndPoint(Vector3 startPos, Vector3 dir, float maxDistance)
    {
        dir.Normalize();
        LayerMask mask = (1 << s_raycastLayerLineOfSight) | (1 << s_raycastLayerDynamicLineOfSight);
        return Physics.Raycast(startPos, dir, out RaycastHit hitInfo, maxDistance, mask)
            ? hitInfo.point
            : startPos + dir * maxDistance;
    }

    public static bool RaycastInDirection(Vector3 startPos, Vector3 dir, float maxDistance, out RaycastHit hit)
    {
        dir.Normalize();
        LayerMask mask = (1 << s_raycastLayerLineOfSight) | (1 << s_raycastLayerDynamicLineOfSight);
        return Physics.Raycast(startPos, dir, out hit, maxDistance, mask);
    }

    public static bool RaycastInDirection(Vector3 startPos, Vector3 endPos, out RaycastHit hit)
    {
        Vector3 direction = endPos - startPos;
        direction.y = 0f;
        direction.Normalize();
        float magnitude = direction.magnitude; // TODO Client bug? getting magnitude after normalization
        LayerMask mask = (1 << s_raycastLayerLineOfSight) | (1 << s_raycastLayerDynamicLineOfSight);
        return Physics.Raycast(startPos, direction, out hit, magnitude, mask);
    }

    public static Vector3 GetAdjustedStartPosWithOffset(Vector3 startPos, Vector3 endPos, float offsetInSquares)
    {
        Vector3 vector = endPos - startPos;
        vector.y = 0f;
        Vector3 result = startPos;
        if (offsetInSquares != 0f)
        {
            float d = Mathf.Min(vector.magnitude, offsetInSquares * Board.Get().squareSize);
            result = startPos + d * vector.normalized;
        }

        return result;
    }

    public static bool SquareOnSameSideAsBounceBend(
        BoardSquare testSquare,
        Vector3 bounceFromPos,
        Vector3 collisionNormal)
    {
        Vector3 rhs = testSquare.ToVector3() - bounceFromPos;
        rhs.y = 0f;
        return Vector3.Dot(collisionNormal, rhs) >= 0f;
    }

    public static List<Vector3> CalculateBouncingLaserEndpoints(
        Vector3 laserStartPos,
        Vector3 forwardDirection,
        float maxDistancePerBounceInSquares,
        float totalMaxDistanceInSquares,
        int maxBounces,
        ActorData caster,
        float widthInSquares,
        int maxTargets,
        bool includeInvisibles,
        List<Team> validTeams,
        bool bounceOnActors,
        out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors,
        out List<ActorData> orderedHitActors,
        List<List<NonActorTargetInfo>> nonActorTargetInfoInSegments,
        bool calculateLaserPastMaxTargets = false,
        bool skipHitsOnCaster = true)
    {
        Vector3 vector = laserStartPos;
        List<Vector3> result = new List<Vector3>();
        bounceHitActors = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
        orderedHitActors = new List<ActorData>();
        float num = Board.Get().squareSize * AreaEffectUtils.GetActorTargetingRadius();
        float maxDistPerBounce = maxDistancePerBounceInSquares * Board.Get().squareSize;
        float maxTotalDist = totalMaxDistanceInSquares * Board.Get().squareSize;
        Vector3 vector2 = forwardDirection;
        int numBounces = 0;
        int num5 = 0;
        float totalDist = 0f;
        bool isMiss = false;
        bool hasReachedMaxTotalDist = false;
        ActorData excludeActor = null;
        float maxDistanceInWorld = Board.Get().squareSize * 1.8f;
        List<NonActorTargetInfo> list2 = new List<NonActorTargetInfo>();
        LaserCoords laserCoordinates = GetLaserCoordinates(
            laserStartPos,
            forwardDirection,
            maxDistanceInWorld,
            0f,
            false,
            caster,
            list2);
        Vector3 end = laserCoordinates.end;
        Vector3 a = end - laserStartPos;
        Vector3 vector3 = a * 0.5f;
        float magnitude = vector3.magnitude;
        laserStartPos += vector3;
        totalDist += magnitude;
        maxDistPerBounce -= magnitude;
        for (int i = 0;; i++)
        {
            bool flag4 = nonActorTargetInfoInSegments != null;
            if (nonActorTargetInfoInSegments != null)
            {
                nonActorTargetInfoInSegments.Add(new List<NonActorTargetInfo>());
            }

            vector2.Normalize();
            float maxDistance = Mathf.Min(maxDistPerBounce, maxTotalDist - totalDist);
            maxDistPerBounce = maxDistancePerBounceInSquares * Board.Get().squareSize;
            Vector3 prevStartPos = Vector3.zero;
            if (i == 1)
            {
                prevStartPos = vector;
            }
            else if (i > 1)
            {
                prevStartPos = result[i - 2];
            }

            Vector3 startPosForBounce = laserStartPos;
            Vector3 startPosForGameplay = i == 0 ? vector : laserStartPos;
            Vector3 dir = vector2;
            List<NonActorTargetInfo> nonActorTargetInfo = flag4 ? nonActorTargetInfoInSegments[i] : null;
            List<ActorData> list3 = CalculateLaserBounce(
                startPosForBounce,
                startPosForGameplay,
                dir,
                maxDistance,
                caster,
                out Vector3 endPoint,
                out bool collisionWithGeo,
                out Vector3 collisionNormal,
                widthInSquares,
                validTeams,
                nonActorTargetInfo,
                includeInvisibles,
                i,
                prevStartPos,
                bounceOnActors,
                excludeActor,
                out bool hitActorFirst,
                out ActorData bounceHitActor,
                skipHitsOnCaster);
            excludeActor = bounceHitActor;
            foreach (ActorData current in list3)
            {
                if (!bounceHitActors.ContainsKey(current)
                    && (maxTargets <= 0 || orderedHitActors.Count < maxTargets))
                {
                    AreaEffectUtils.BouncingLaserInfo value =
                        new AreaEffectUtils.BouncingLaserInfo(i != 0 ? laserStartPos : vector, i);
                    bounceHitActors.Add(current, value);
                    orderedHitActors.Add(current);
                }
            }

            bool hasReachedMaxTargets = maxTargets > 0
                         && orderedHitActors.Count >= maxTargets
                         && !calculateLaserPastMaxTargets;
            if (hasReachedMaxTargets)
            {
                Vector3 normalized = (endPoint - laserStartPos).normalized;
                Vector3 rhs = orderedHitActors[orderedHitActors.Count - 1].GetFreePos() - laserStartPos;
                endPoint = laserStartPos + (Vector3.Dot(normalized, rhs) + num) * normalized;
                if (flag4)
                {
                    nonActorTargetInfoInSegments[i].Clear();
                    flag4 = false;
                }
            }

            if (flag4 && result.Count == 0 && list2.Count > 0)
            {
                foreach (NonActorTargetInfo current2 in list2)
                {
                    nonActorTargetInfoInSegments[i].Add(current2);
                }
            }

            result.Add(endPoint);
            float magnitude2 = (laserStartPos - endPoint).magnitude;
            totalDist += magnitude2;
            if (totalDist >= maxTotalDist - 0.01f)
            {
                hasReachedMaxTotalDist = true;
            }

            if (hitActorFirst)
            {
                numBounces++;
                num5++;
                laserStartPos = endPoint;
                vector2 -= 2f * Vector3.Dot(vector2, collisionNormal) * collisionNormal;
            }
            else if (collisionWithGeo)
            {
                numBounces++;
                laserStartPos = endPoint;
                vector2 -= 2f * Vector3.Dot(vector2, collisionNormal) * collisionNormal;
            }
            else
            {
                isMiss = true;
            }

            if (isMiss
                || hasReachedMaxTotalDist
                || numBounces > maxBounces
                || hasReachedMaxTargets)
            {
                break;
            }
        }

        return result;
    }

    public static List<ActorData> CalculateLaserBounce(
        Vector3 startPosForBounce,
        Vector3 startPosForGameplay,
        Vector3 dir,
        float maxDistance,
        ActorData caster,
        out Vector3 endPoint,
        out bool collisionWithGeo,
        out Vector3 collisionNormal,
        float widthInSquares,
        List<Team> validTeams,
        List<NonActorTargetInfo> nonActorTargetInfo,
        bool includeInvisibles,
        int segmentIndex,
        Vector3 prevStartPos,
        bool bounceOnActors,
        ActorData excludeActor,
        out bool hitActorFirst,
        out ActorData bounceHitActor,
        bool skipHitsOnCaster = true)
    {
        LayerMask mask = (1 << s_raycastLayerLineOfSight) | (1 << s_raycastLayerDynamicLineOfSight);
        collisionWithGeo = Physics.Raycast(startPosForBounce, dir, out RaycastHit hitInfo, maxDistance, mask);
        if (collisionWithGeo)
        {
            Vector3 a = hitInfo.point - startPosForBounce;
            a.y = 0f;
            float magnitude = a.magnitude;
            a.Normalize();
            endPoint = startPosForBounce + a * Mathf.Max(0f, magnitude - 0.1f);
            collisionNormal = hitInfo.normal;
        }
        else
        {
            endPoint = startPosForBounce + dir * maxDistance;
            collisionNormal = Vector3.zero;
        }

        if (BarrierManager.Get() != null)
        {
            Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(
                caster,
                startPosForBounce,
                endPoint,
                out bool collision,
                out Vector3 collisionNormal2,
                nonActorTargetInfo);
            if (collision)
            {
                endPoint = abilityLineEndpoint;
                collisionWithGeo = true;
                collisionNormal = collisionNormal2;
            }
        }

        List<ActorData> actors = GameWideData.Get().UseActorRadiusForLaser()
            ? AreaEffectUtils.GetActorsInBoxByActorRadius(
                startPosForGameplay,
                endPoint,
                widthInSquares,
                false,
                caster,
                validTeams)
            : AreaEffectUtils.GetActorsInBox(
                startPosForGameplay,
                endPoint,
                widthInSquares,
                true,
                caster,
                validTeams);

        if (skipHitsOnCaster)
        {
            actors.Remove(caster);
        }

        if (!includeInvisibles)
        {
            if (NetworkServer.active)
            {
                TargeterUtils.RemoveActorsInvisibleToActor(ref actors, caster);
            }
            else
            {
                TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
            }
        }

        if (excludeActor != null)
        {
            actors.Remove(excludeActor);
        }

        Vector3 a2 = endPoint - startPosForBounce;
        a2.y = 0f;
        a2.Normalize();
        if (segmentIndex > 0 && actors.Count > 0)
        {
            Vector3 b = prevStartPos - startPosForBounce;
            b.y = 0f;
            b.Normalize();
            Vector3 collisionNormal3 = 0.5f * (a2 + b);
            for (int num = actors.Count - 1; num >= 0; num--)
            {
                BoardSquare currentBoardSquare = actors[num].GetCurrentBoardSquare();
                if (!SquareOnSameSideAsBounceBend(currentBoardSquare, startPosForBounce, collisionNormal3))
                {
                    actors.RemoveAt(num);
                }
            }
        }

        TargeterUtils.SortActorsByDistanceToPos(ref actors, startPosForBounce);
        hitActorFirst = false;
        bounceHitActor = null;
        if (bounceOnActors)
        {
            int num2 = 0;
            float num3 = GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
            for (int i = 0; i < actors.Count; i++)
            {
                if (hitActorFirst)
                {
                    break;
                }

                ActorData actorData = actors[i];
                Vector3 vector = actorData.GetFreePos() - startPosForBounce;
                vector.y = 0f;
                float magnitude2 = vector.magnitude;
                Vector3 vector2 = endPoint - startPosForBounce;
                vector2.y = 0f;
                float magnitude3 = vector2.magnitude;
                if (magnitude2 < magnitude3)
                {
                    Vector3 travelBoardSquareWorldPosition = actorData.GetFreePos();
                    float num4 = 0.5f * widthInSquares * Board.Get().squareSize;
                    if (GameWideData.Get().UseActorRadiusForLaser())
                    {
                        num4 += num3;
                    }

                    num4 = Mathf.Min(0.4f * Board.Get().squareSize, num4);
                    int lineCircleIntersections = GetLineCircleIntersections(
                        startPosForGameplay,
                        endPoint,
                        travelBoardSquareWorldPosition,
                        num4,
                        out Vector3 intersectP1,
                        out Vector3 intersectP2);
                    if (lineCircleIntersections <= 1)
                    {
                        continue;
                    }

                    float num5 = HorizontalPlaneDistInWorld(intersectP1, startPosForGameplay);
                    float num6 = HorizontalPlaneDistInWorld(intersectP2, startPosForGameplay);

                    endPoint = num5 <= num6 ? intersectP1 : intersectP2;
                    endPoint.y = startPosForBounce.y;
                    collisionNormal = endPoint - travelBoardSquareWorldPosition;
                    collisionNormal.y = 0f;
                    collisionNormal.Normalize();
                    float num7 = 0.5f * AreaEffectUtils.GetMaxAngleForActorBounce();
                    collisionNormal = Vector3.RotateTowards(-a2, collisionNormal, (float)Math.PI / 180f * num7, 0f);
                    hitActorFirst = true;
                    collisionWithGeo = false;
                    bounceHitActor = actorData;
                    num2 = i;
                }
            }

            if (hitActorFirst)
            {
                TargeterUtils.LimitActorsToMaxNumber(ref actors, num2 + 1);
            }
        }

        return actors;
    }

    public static List<Vector3> CalculateBouncingActorEndpoints(
        Vector3 laserStartPos,
        Vector3 forwardDirection,
        float maxDistancePerBounceInSquares,
        float totalMaxDistanceInSquares,
        int maxBounces,
        ActorData caster,
        bool bounceOnActors,
        float bounceTestWidthInSquares,
        List<Team> bounceActorTeams,
        int maxTargets,
        out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors,
        out List<ActorData> orderedHitActors,
        bool includeInvisibles,
        List<List<NonActorTargetInfo>> nonActorTargetInfoInSegments)
    {
        Vector3 initialStartPos = laserStartPos;

        List<Vector3> startPositions = new List<Vector3>();
        bounceHitActors = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
        orderedHitActors = new List<ActorData>();

        float maxDistancePerBounce = maxDistancePerBounceInSquares * Board.Get().squareSize;
        float totalMaxDistance = totalMaxDistanceInSquares * Board.Get().squareSize;
        Vector3 bounceDirection = forwardDirection;
        int numBounces = 0;
        float totalDist = 0f;
        int numBouncesOnActors = 0;
        bool isMiss = false;
        bool hasReachedMaxTotalDist = false;
        laserStartPos.y = caster.GetLoSCheckPos().y;

        List<NonActorTargetInfo> nonActorTargetInfoInitial = new List<NonActorTargetInfo>();
        LaserCoords laserCoordinatesInitial = GetLaserCoordinates(
            laserStartPos,
            forwardDirection,
            Board.Get().squareSize * 1.8f,
            0f,
            false,
            caster,
            nonActorTargetInfoInitial);

        Vector3 adjustment = (laserCoordinatesInitial.end - laserStartPos) * 0.5f;
        float adjustmentMagnitude = adjustment.magnitude;
        laserStartPos += adjustment;
        totalDist += adjustmentMagnitude;
        maxDistancePerBounce -= adjustmentMagnitude; // it is reset back for all other segments

        ActorData lastHitActor = null;
        for (int i = 0;; i++)
        {
            bool isCollectingNonActorInfo = nonActorTargetInfoInSegments != null;
            nonActorTargetInfoInSegments?.Add(new List<NonActorTargetInfo>());

            bounceDirection.Normalize();

            float maxDistance = Mathf.Min(maxDistancePerBounce, totalMaxDistance - totalDist);
            maxDistancePerBounce = maxDistancePerBounceInSquares * Board.Get().squareSize;

            Vector3 startPosForGameplay = i == 0
                ? initialStartPos
                : laserStartPos;
            Vector3 prevStartPos = i == 0
                ? Vector3.zero
                : i == 1
                    ? initialStartPos
                    : startPositions[i - 2];

            Vector3 startPosForBounce = laserStartPos;

            List<ActorData> hitActors = CalculateActorBounce(
                startPosForBounce,
                startPosForGameplay,
                bounceDirection,
                maxDistance,
                caster,
                bounceOnActors,
                bounceTestWidthInSquares,
                bounceActorTeams,
                lastHitActor,
                includeInvisibles,
                out Vector3 endPoint,
                out bool collisionWithGeo,
                out Vector3 collisionNormal,
                out bool hitActorFirst,
                out ActorData bounceHitActor,
                isCollectingNonActorInfo ? nonActorTargetInfoInSegments[i] : null,
                i,
                prevStartPos);
            lastHitActor = bounceHitActor;

            foreach (ActorData hitActor in hitActors)
            {
                if (!bounceHitActors.ContainsKey(hitActor)
                    && (maxTargets <= 0 || orderedHitActors.Count < maxTargets))
                {
                    AreaEffectUtils.BouncingLaserInfo value =
                        new AreaEffectUtils.BouncingLaserInfo(laserStartPos, i);
                    bounceHitActors.Add(hitActor, value);
                    orderedHitActors.Add(hitActor);
                }
            }

            bool hasReachedMaxTargets = maxTargets > 0 && orderedHitActors.Count >= maxTargets;
            if (hasReachedMaxTargets && isCollectingNonActorInfo)
            {
                nonActorTargetInfoInSegments[i].Clear();
                isCollectingNonActorInfo = false;
            }

            if (isCollectingNonActorInfo
                && startPositions.Count == 0
                && nonActorTargetInfoInitial.Count > 0)
            {
                foreach (NonActorTargetInfo info in nonActorTargetInfoInitial)
                {
                    nonActorTargetInfoInSegments[i].Add(info);
                }
            }

            startPositions.Add(endPoint);
            float bounceDist = (laserStartPos - endPoint).magnitude;
            totalDist += bounceDist;
            if (totalDist >= totalMaxDistance - 0.01f)
            {
                hasReachedMaxTotalDist = true;
            }

            if (hitActorFirst)
            {
                numBounces++;
                numBouncesOnActors++;
                laserStartPos = endPoint;
                bounceDirection -= 2f * Vector3.Dot(bounceDirection, collisionNormal) * collisionNormal;
            }
            else if (collisionWithGeo)
            {
                numBounces++;
                laserStartPos = endPoint;
                bounceDirection -= 2f * Vector3.Dot(bounceDirection, collisionNormal) * collisionNormal;
            }
            else
            {
                isMiss = true;
            }

            if (isMiss
                || hasReachedMaxTotalDist
                || numBounces > maxBounces
                || hasReachedMaxTargets)
            {
                break;
            }
        }

        return startPositions;
    }

    public static List<ActorData> CalculateActorBounce(
        Vector3 startPosForBounce,
        Vector3 startPosForGameplay,
        Vector3 dir,
        float maxDistance,
        ActorData caster,
        bool bounceOnActors,
        float bounceTestWidthInSquares,
        List<Team> hitTeams,
        ActorData excludeActor,
        bool includeInvisibles,
        out Vector3 endPoint,
        out bool collisionWithGeo,
        out Vector3 collisionNormal,
        out bool hitActorFirst,
        out ActorData bounceHitActor,
        List<NonActorTargetInfo> nonActorTargetInfo,
        int segmentIndex,
        Vector3 prevStartPos)
    {
        LayerMask mask = (1 << s_raycastLayerLineOfSight) | (1 << s_raycastLayerDynamicLineOfSight);
        collisionWithGeo = Physics.Raycast(startPosForBounce, dir, out RaycastHit hitInfo, maxDistance, mask);
        if (collisionWithGeo)
        {
            Vector3 actualDirection = hitInfo.point - startPosForBounce;
            actualDirection.y = 0f;
            float distToHit = actualDirection.magnitude;
            actualDirection.Normalize();
            endPoint = startPosForBounce + actualDirection * Mathf.Max(0f, distToHit - 0.05f);
            collisionNormal = hitInfo.normal;
        }
        else
        {
            endPoint = startPosForBounce + dir * maxDistance;
            collisionNormal = Vector3.zero;
        }

        if (BarrierManager.Get() != null)
        {
            Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(
                caster,
                startPosForBounce,
                endPoint,
                out bool collision,
                out Vector3 endpointCollisionNormal,
                nonActorTargetInfo);
            if (collision)
            {
                endPoint = abilityLineEndpoint;
                collisionWithGeo = true;
                collisionNormal = endpointCollisionNormal;
            }
        }

        hitActorFirst = false;
        bounceHitActor = null;
        float actorTargetingRadius = AreaEffectUtils.GetActorTargetingRadius();
        List<ActorData> actors = GameWideData.Get().UseActorRadiusForLaser()
            ? AreaEffectUtils.GetActorsInBoxByActorRadius(
                startPosForGameplay,
                endPoint,
                bounceTestWidthInSquares,
                false,
                caster,
                hitTeams)
            : AreaEffectUtils.GetActorsInBox(
                startPosForGameplay,
                endPoint,
                bounceTestWidthInSquares,
                true,
                caster,
                hitTeams);
        actors.Remove(caster);

#if SERVER
        // added in rogues
        if (includeInvisibles)
        {
            ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actors);
        }
#endif

        if (excludeActor != null)
        {
            actors.Remove(excludeActor);
        }

        if (!includeInvisibles)
        {
            if (NetworkServer.active)
            {
                TargeterUtils.RemoveActorsInvisibleToActor(ref actors, caster);
            }
            else
            {
                TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
            }
        }

        Vector3 finalDirection = endPoint - startPosForBounce;
        finalDirection.y = 0f;
        finalDirection.Normalize();

        if (segmentIndex > 0 && actors.Count > 0)
        {
            Vector3 prevDir = prevStartPos - startPosForBounce;
            prevDir.y = 0f;
            prevDir.Normalize();

            Vector3 bounceNormal = 0.5f * (finalDirection + prevDir);
            for (int i = actors.Count - 1; i >= 0; i--)
            {
                BoardSquare currentBoardSquare = actors[i].GetCurrentBoardSquare();
                if (!SquareOnSameSideAsBounceBend(currentBoardSquare, startPosForBounce, bounceNormal))
                {
                    actors.RemoveAt(i);
                }
            }
        }

        TargeterUtils.SortActorsByDistanceToPos(ref actors, startPosForBounce);

        if (bounceOnActors)
        {
            int max = 0;
            for (int i = 0; i < actors.Count; i++)
            {
                if (hitActorFirst)
                {
                    break;
                }

                ActorData actorData = actors[i];

                Vector3 startToHitActor = actorData.GetFreePos() - startPosForBounce;
                startToHitActor.y = 0f;
                float distToHitActor = startToHitActor.magnitude;

                Vector3 startToEnd = endPoint - startPosForBounce;
                startToEnd.y = 0f;
                float distToEnd = startToEnd.magnitude;

                if (distToHitActor >= distToEnd)
                {
                    continue;
                }

                Vector3 actorPos = actorData.GetFreePos();

                float radius = 0.5f * bounceTestWidthInSquares * Board.Get().squareSize;
                if (GameWideData.Get().UseActorRadiusForLaser())
                {
                    radius += actorTargetingRadius * Board.Get().squareSize;
                }

                radius = Mathf.Min(0.4f * Board.Get().squareSize, radius);

                int lineCircleIntersections = GetLineCircleIntersections(
                    startPosForBounce,
                    endPoint,
                    actorPos,
                    radius,
                    out Vector3 intersectP1,
                    out Vector3 intersectP2);

                if (lineCircleIntersections > 1)
                {
                    float distToIntersectP1 = HorizontalPlaneDistInWorld(intersectP1, startPosForBounce);
                    float distToIntersectP2 = HorizontalPlaneDistInWorld(intersectP2, startPosForBounce);
                    endPoint = distToIntersectP1 <= distToIntersectP2 ? intersectP1 : intersectP2;
                    endPoint.y = startPosForBounce.y;

                    collisionNormal = endPoint - actorPos;
                    collisionNormal.y = 0f;
                    collisionNormal.Normalize();

                    float maxHalfAngle = 0.5f * AreaEffectUtils.GetMaxAngleForActorBounce();
                    collisionNormal = Vector3.RotateTowards(
                        -finalDirection,
                        collisionNormal,
                        (float)Math.PI / 180f * maxHalfAngle,
                        0f);

                    hitActorFirst = true;
                    collisionWithGeo = false;
                    bounceHitActor = actorData;
                    max = i;
                }
            }

            if (hitActorFirst)
            {
                TargeterUtils.LimitActorsToMaxNumber(ref actors, max + 1);
            }
        }

        return actors;
    }

    public static Vector3 GetLineLineIntersection(
        Vector3 pointOnFirst,
        Vector3 directionOfFirst,
        Vector3 pointOnSecond,
        Vector3 directionOfSecond,
        out bool intersecting)
    {
        Vector3 vector = pointOnFirst;
        Vector3 a = pointOnSecond;
        Vector3 vector2 = directionOfFirst;
        Vector3 rhs = directionOfSecond;
        vector.y = 0f;
        a.y = 0f;
        vector2.y = 0f;
        vector2.Normalize();
        rhs.y = 0f;
        rhs.Normalize();
        Vector3 vector3 = Vector3.Cross(vector2, rhs);
        Vector3 lhs = a - vector;
        if (vector3.magnitude == 0f)
        {
            intersecting = false;
            return Vector3.zero;
        }
        else
        {
            intersecting = true;
            float d = Vector3.Cross(lhs, rhs).magnitude / vector3.magnitude;
            return vector + d * vector2;
        }
    }

    public static int GetLineCircleIntersections(
        Vector3 testPoint1,
        Vector3 testPoint2,
        Vector3 circleCenter,
        float radius,
        out Vector3 intersectP1,
        out Vector3 intersectP2)
    {
        intersectP1 = Vector3.zero;
        intersectP2 = Vector3.zero;
        testPoint1.y = 0f;
        testPoint2.y = 0f;
        circleCenter.y = 0f;
        Vector3 vector = testPoint1 - circleCenter;
        Vector3 vector2 = testPoint2 - circleCenter;
        float num = vector2.x - vector.x;
        float num2 = vector2.z - vector.z;
        float num3 = num * num + num2 * num2;
        float num4 = vector.x * vector2.z - vector2.x * vector.z;
        float num5 = radius * radius * num3 - num4 * num4;
        if (num3 == 0f)
        {
            return 0;
        }

        if (Mathf.Abs(num5) < 0.001f)
        {
            intersectP1.x = num4 * num2 / num3;
            intersectP1.z = -num4 * num / num3;
            intersectP1 += circleCenter;
            return 1;
        }

        if (num5 < 0f)
        {
            return 0;
        }

        float num7 = num2 < 0f ? -1f : 1f;
        float num8 = Mathf.Sqrt(num5);
        intersectP1.x = (num4 * num2 + num7 * num * num8) / num3;
        intersectP1.z = (-num4 * num + Mathf.Abs(num2) * num8) / num3;
        intersectP2.x = (num4 * num2 - num7 * num * num8) / num3;
        intersectP2.z = (-num4 * num - Mathf.Abs(num2) * num8) / num3;
        intersectP1 += circleCenter;
        intersectP2 += circleCenter;
        return 2;
    }

    public static bool IsSegmentIntersectingCircle(Vector3 startPos, Vector3 endPos, Vector3 circleCenter, float radius)
    {
        startPos.y = 0f;
        endPos.y = 0f;
        circleCenter.y = 0f;
        Vector3 vector = endPos - startPos;
        int lineCircleIntersections = GetLineCircleIntersections(
            startPos,
            endPos,
            circleCenter,
            radius,
            out Vector3 intersectP1,
            out Vector3 intersectP2);
        if (lineCircleIntersections >= 1)
        {
            float num = Vector3.Dot(vector, vector);
            Vector3 rhs = intersectP1 - startPos;
            float num2 = Vector3.Dot(vector, rhs);
            if (num2 >= 0f && num2 <= num)
            {
                return true;
            }

            if (lineCircleIntersections >= 2)
            {
                rhs = intersectP2 - startPos;
                num2 = Vector3.Dot(vector, rhs);
                if (num2 >= 0f && num2 <= num)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool OnSameSideOfLine(Vector3 testPoint1, Vector3 testPoint2, Vector3 linePtA, Vector3 linePtB)
    {
        Vector3 lhs = linePtB - linePtA;
        lhs.y = 0f;
        Vector3 rhs = testPoint1 - linePtA;
        rhs.y = 0f;
        Vector3 rhs2 = testPoint2 - linePtA;
        rhs2.y = 0f;
        Vector3 lhs2 = Vector3.Cross(lhs, rhs);
        Vector3 rhs3 = Vector3.Cross(lhs, rhs2);
        float num = Vector3.Dot(lhs2, rhs3);
        return num >= 0f;
    }

    public static bool IsPointInTriangle(Vector3 triA, Vector3 triB, Vector3 triC, Vector3 testPt)
    {
        bool flag = OnSameSideOfLine(testPt, triA, triB, triC);
        bool flag2 = OnSameSideOfLine(testPt, triB, triA, triC);
        return OnSameSideOfLine(testPt, triC, triA, triB) && flag && flag2;
    }

    public static bool IsPointInLaser(
        Vector3 testPoint,
        Vector3 laserStartPos,
        Vector3 laserEndPos,
        float laserWidthInWorld)
    {
        testPoint.y = 0f;
        laserStartPos.y = 0f;
        laserEndPos.y = 0f;
        float sqrMagnitude = (laserEndPos - laserStartPos).sqrMagnitude;
        Vector3 normalized = (laserEndPos - laserStartPos).normalized;
        Vector3 lhs = testPoint - laserStartPos;
        Vector3 vector = laserStartPos + Vector3.Dot(lhs, normalized) * normalized;
        float sqrMagnitude2 = (vector - laserStartPos).sqrMagnitude;
        float sqrMagnitude3 = (laserEndPos - vector).sqrMagnitude;
        bool flag = sqrMagnitude2 < sqrMagnitude && sqrMagnitude3 < sqrMagnitude;
        float sqrMagnitude4 = (vector - testPoint).sqrMagnitude;
        float num2 = laserWidthInWorld / 2f * (laserWidthInWorld / 2f);
        bool flag2 = sqrMagnitude4 < num2;
        return flag && flag2;
    }

    // removed in rogues
    public static Vector3 GetProjectionPoint(Vector3 normalizedDir, Vector3 startPos, Vector3 pointToProject)
    {
        Vector3 lhs = pointToProject - startPos;
        return startPos + Vector3.Dot(lhs, normalizedDir) * normalizedDir;
    }

    public static float HorizontalAngle_Rad(Vector3 vec)
    {
        Vector2 vector = new Vector2(vec.x, vec.z);
        vector.Normalize();
        return Mathf.Atan2(vector.y, vector.x);
    }

    public static float HorizontalAngle_Deg(Vector3 vec)
    {
        float num = HorizontalAngle_Rad(vec);
        float num2 = num * 57.29578f;
        if (num2 < 0f)
        {
            num2 += 360f;
        }

        return num2;
    }

    public static float ClampAngle_Deg(float angle, float min, float max)
    {
        float num;
        if (min > max)
        {
            Debug.LogError(
                $"Clamping an angle {angle} to a min of {min} and a max of {max}, but min is greater than max.");
            num = angle;
        }
        else if (min == max)
        {
            num = min;
        }
        else
        {
            if (angle >= min && angle <= max)
            {
                num = angle;
            }
            else if (angle + 360f >= min && angle + 360f <= max)
            {
                num = angle;
            }
            else if (angle - 360f >= min && angle - 360f <= max)
            {
                num = angle;
            }
            else
            {
                float num2 = Mathf.Clamp(angle, min, max);
                float num3 = Mathf.Clamp(angle, min + 360f, max + 360f);
                float num4 = Mathf.Clamp(angle, min - 360f, max - 360f);
                float num5 = Mathf.Abs(angle - num2);
                float num6 = Mathf.Abs(angle - num3);
                float num7 = Mathf.Abs(angle - num4);
                if (num5 <= num6 && num5 <= num7)
                {
                    num = num2;
                }
                else if (num6 <= num5 && num6 <= num7)
                {
                    num = num3;
                }
                else
                {
                    num = num4;
                }
            }
        }

        while (num > 360f)
        {
            num -= 360f;
        }

        while (num < 0f)
        {
            num += 360f;
        }

        return num;
    }

    public static Vector3 AngleRadToVector(float angle)
    {
        return new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
    }

    public static Vector3 AngleDegreesToVector(float angle)
    {
        float angle2 = angle * ((float)Math.PI / 180f);
        return AngleRadToVector(angle2);
    }

    public static Vector3 GetDirectionToClosestSide(BoardSquare square, Vector3 testPos)
    {
        if (square == null)
        {
            return new Vector3(1f, 0f, 0f);
        }
        
        Vector3 b = square.ToVector3();
        Vector3 vec = testPos - b;
        vec.y = 0f;
        
        if (vec.magnitude < 0.1f)
        {
            return new Vector3(1f, 0f, 0f);
        }
            
        int angleWithHorizontal = Mathf.RoundToInt(HorizontalAngle_Deg(vec));
        return HorizontalAngleToClosestCardinalDirection(angleWithHorizontal);
    }

    public static Vector3 HorizontalAngleToClosestCardinalDirection(int angleWithHorizontal)
    {
        Vector3 result;
        if (angleWithHorizontal > 45 && angleWithHorizontal <= 135)
        {
            result = new Vector3(0f, 0f, 1f);
        }
        else if (angleWithHorizontal > 135 && angleWithHorizontal <= 225)
        {
            result = new Vector3(-1f, 0f, 0f);
        }
        else if (angleWithHorizontal > 225 && angleWithHorizontal <= 315)
        {
            result = new Vector3(0f, 0f, -1f);
        }
        else
        {
            result = new Vector3(1f, 0f, 0f);
        }

        return result;
    }

    public static Vector3 GetDirectionAndOffsetToClosestSide(
        BoardSquare square,
        Vector3 testPos,
        bool allowDiagonalAim,
        out Vector3 offset)
    {
        Vector3 vector = new Vector3(1f, 0f, 0f);
        offset = 0.5f * Board.Get().squareSize * vector;
        if (square == null)
        {
            return vector;
        }
        
        Vector3 b = square.ToVector3();
        Vector3 vec = testPos - b;
        vec.y = 0f;
        if (vec.magnitude < 0.1f)
        {
            return vector;
        }

        if (allowDiagonalAim)
        {
            int num = Mathf.RoundToInt(HorizontalAngle_Deg(vec));
            float num2 = 0f;
            bool flag = false;
            if (num < 337 && num > 23)
            {
                if (num < 67)
                {
                    num2 = 45f;
                    flag = true;
                }
                else if (num <= 113)
                {
                    num2 = 90f;
                }
                else if (num < 157)
                {
                    num2 = 135f;
                    flag = true;
                }
                else if (num <= 203)
                {
                    num2 = 180f;
                }
                else if (num < 247)
                {
                    num2 = 225f;
                    flag = true;
                }
                else if (num <= 293)
                {
                    num2 = 270f;
                }
                else
                {
                    num2 = 315f;
                    flag = true;
                }
            }
            else
            {
                num2 = 0f;
            }

            vector = AngleDegreesToVector(num2);
            if (flag)
            {
                float num3 = 0.5f * Mathf.Sqrt(2f) - 0.01f;
                offset = num3 * Board.Get().squareSize * vector;
            }
            else
            {
                offset = 0.5f * Board.Get().squareSize * vector;
            }

            return vector;
        }

        vector = GetDirectionToClosestSide(square, testPos);
        offset = 0.5f * Board.Get().squareSize * vector;

        return vector;
    }

    public static float HorizontalPlaneDistInWorld(Vector3 a, Vector3 b)
    {
        Vector3 vector = b - a;
        vector.y = 0f;
        return vector.magnitude;
    }

    public static float HorizontalPlaneDistInSquares(Vector3 a, Vector3 b)
    {
        return HorizontalPlaneDistInWorld(a, b) / Board.Get().squareSize;
    }
}