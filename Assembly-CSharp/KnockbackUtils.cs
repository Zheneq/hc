using System.Collections.Generic;
using UnityEngine;

public static class KnockbackUtils
{
    public static Vector2 GetKnockbackDeltaForType(KnockbackHitData hitData)
    {
        return GetKnockbackDeltaForType(
            hitData.m_target,
            hitData.m_type,
            hitData.m_aimDir,
            hitData.m_sourcePos,
            hitData.m_distance);
    }

    public static Vector2 GetKnockbackDeltaForType(
        ActorData target,
        KnockbackType type,
        Vector3 aimDir,
        Vector3 sourcePos,
        float distance)
    {
        aimDir.Normalize();
        Vector3 targetPos = target.GetFreePos();
        Vector3 delta = targetPos - sourcePos;
        Vector3 projectedTargetPos = sourcePos + Vector3.Dot(delta, aimDir) * aimDir;
        Vector3 offset = targetPos - projectedTargetPos;
        float squareSize = Board.Get().squareSize;
        Vector2 result;
        switch (type)
        {
            case KnockbackType.ForwardAlongAimDir:
            {
                result = new Vector2(aimDir.x, aimDir.z);
                result.Normalize();
                result *= distance;
                break;
            }
            case KnockbackType.BackwardAgainstAimDir:
            {
                result = new Vector2(aimDir.x, aimDir.z);
                result = -result;
                result.Normalize();
                result *= distance;
                break;
            }
            case KnockbackType.PerpendicularAwayFromAimDir:
            {
                result = new Vector2(offset.x, offset.z);
                result.Normalize();
                result *= distance;
                break;
            }
            case KnockbackType.PerpendicularPullToAimDir:
                result = new Vector2(offset.x, offset.z);
                result = -result;
                if (distance > 0f && result.magnitude > distance)
                {
                    result.Normalize();
                    result *= distance;
                }

                break;
            case KnockbackType.AwayFromSource:
            {
                result = new Vector2(targetPos.x - sourcePos.x, targetPos.z - sourcePos.z);
                result.Normalize();
                result *= distance;
                break;
            }
            case KnockbackType.PullToSource:
            {
                result = new Vector2(
                    (sourcePos.x - targetPos.x) / squareSize,
                    (sourcePos.z - targetPos.z) / squareSize);
                if (distance > 0f && result.magnitude > distance)
                {
                    result.Normalize();
                    result *= distance;
                }

                break;
            }
            case KnockbackType.PullToSourceOverShoot:
            {
                result = new Vector2(
                    (sourcePos.x - targetPos.x) / squareSize,
                    (sourcePos.z - targetPos.z) / squareSize);
                float num2 = result.magnitude + 0.9f;
                float num3 = distance > 0f ? Mathf.Min(distance, num2) : num2;
                result.Normalize();
                result *= num3;
                break;
            }
            case KnockbackType.PullToSourceActor:
            {
                result = new Vector2(
                    (sourcePos.x - targetPos.x) / squareSize,
                    (sourcePos.z - targetPos.z) / squareSize);
                float magnitude = result.magnitude;
                magnitude = Mathf.Max(0f, magnitude - 0.71f);
                float num = magnitude;
                if (distance > 0f && magnitude > distance)
                {
                    num = distance;
                }

                result.Normalize();
                result *= num;
                break;
            }
            default:
            {
                result = Vector2.zero;
                break;
            }
        }

        return result;
    }

    public static BoardSquarePathInfo BuildKnockbackPath(
        ActorData target,
        KnockbackType type,
        Vector3 aimDir,
        Vector3 sourcePos,
        float distance)
    {
        Vector2 knockbackDeltaForType = GetKnockbackDeltaForType(target, type, aimDir, sourcePos, distance);
        return BuildKnockbackPathFromVector(target, knockbackDeltaForType);
    }

    public static BoardSquarePathInfo BuildKnockbackPathFromVector(ActorData target, Vector2 idealMoveDir)
    {
        BoardSquare currentBoardSquare = target.GetCurrentBoardSquare();
        BoardSquarePathInfo path = new BoardSquarePathInfo
        {
            square = currentBoardSquare
        };
        BoardSquarePathInfo step = path;
        Vector2 destination = new Vector2(currentBoardSquare.x + idealMoveDir.x, currentBoardSquare.y + idealMoveDir.y);
        BoardSquare currentSquare = currentBoardSquare;
        BoardSquare nextSquare = GetClosestAdjacentSquareTo(currentSquare, currentBoardSquare, destination, false);
        bool isKnockbackResistant = target.GetComponent<ActorStatus>().HasStatus(StatusType.KnockbackResistant);
        while (nextSquare != null)
        {
            BoardSquarePathInfo nextStep = new BoardSquarePathInfo
            {
                square = nextSquare,
                prev = step
            };
            step.next = nextStep;
            step = nextStep;
            currentSquare = nextSquare;
            if (isKnockbackResistant)
            {
                break;
            }

            nextSquare = GetClosestAdjacentSquareTo(currentSquare, currentBoardSquare, destination, false);
        }

        bool isValidPath = step.square != null && step.square.IsValidForGameplay();
        while (!isValidPath)
        {
            if (step.prev == null)
            {
                isValidPath = true;
            }
            else
            {
                step.square = null;
                step.prev.next = null;
                step = step.prev;
                isValidPath = step.square != null && step.square.IsValidForGameplay();
            }
        }

        path.CalcAndSetMoveCostToEnd();
        return path;
    }

    public static BoardSquare GetLastValidBoardSquareInLine(
        Vector3 start,
        Vector3 end,
        bool passThroughInvalidSquares = false,
        bool considerHalfHeightWallValid = false,
        float maxDistance = float.MaxValue)
    {
        BoardSquare endSquare = Board.Get().GetSquareFromVec3(end);
        bool isValid = false;
        if (endSquare != null)
        {
            isValid = considerHalfHeightWallValid
                ? endSquare.IsValidForKnockbackAndCharge()
                : endSquare.IsValidForGameplay();
        }

        if (isValid && maxDistance >= float.MaxValue)
        {
            return endSquare;
        }

        BoardSquare result = null;
        BoardSquare startSquare = Board.Get().GetSquareFromVec3(start);
        if (startSquare != null)
        {
            result = startSquare;
            GridPos endPos = GridPos.FromVector3(end);
            BoardSquare currentSquare = startSquare;
            Vector2 destination = new Vector2(endPos.x, endPos.y);
            BoardSquare nextSquare = GetClosestAdjacentSquareTo(
                currentSquare,
                startSquare,
                destination,
                passThroughInvalidSquares);
            while (nextSquare != null)
            {
                float dist = startSquare.HorizontalDistanceInWorldTo(nextSquare);
                if (dist > maxDistance)
                {
                    break;
                }

                currentSquare = nextSquare;
                if (considerHalfHeightWallValid
                        ? currentSquare.IsValidForKnockbackAndCharge()
                        : currentSquare.IsValidForGameplay())
                {
                    result = currentSquare;
                }

                nextSquare = GetClosestAdjacentSquareTo(
                    currentSquare,
                    startSquare,
                    destination,
                    passThroughInvalidSquares);
            }
        }

        return result;
    }

    public static BoardSquarePathInfo BuildStraightLineChargePath(
        ActorData mover,
        BoardSquare destination,
        BoardSquare startSquare,
        bool passThroughInvalidSquares)
    {
        BoardSquarePathInfo path = new BoardSquarePathInfo
        {
            square = startSquare
        };

        if (destination == startSquare)
        {
            path.next = new BoardSquarePathInfo
            {
                prev = path,
                square = destination
            };
            return path;
        }

        if (destination == null)
        {
            return null;
        }

        if (!passThroughInvalidSquares && !destination.IsValidForKnockbackAndCharge())
        {
            return null;
        }

        BoardSquarePathInfo step = path;
        BoardSquare currentSquare = startSquare;
        BoardSquare nextSquare = GetClosestAdjacentSquareTo(
            currentSquare,
            startSquare,
            destination,
            passThroughInvalidSquares);
        while (nextSquare != null)
        {
            BoardSquarePathInfo nextStep = new BoardSquarePathInfo
            {
                square = nextSquare,
                prev = step
            };
            step.next = nextStep;
            step = nextStep;
            currentSquare = nextSquare;
            nextSquare = GetClosestAdjacentSquareTo(currentSquare, startSquare, destination, passThroughInvalidSquares);
        }

        if (currentSquare != null && currentSquare == destination)
        {
            return path;
        }

        return null;
    }

    public static BoardSquarePathInfo BuildStraightLineChargePath(ActorData mover, BoardSquare destination)
    {
        return BuildStraightLineChargePath(mover, destination, mover.GetCurrentBoardSquare(), false);
    }

    public static bool CanBuildStraightLineChargePath(
        ActorData mover,
        BoardSquare destination,
        BoardSquare startSquare,
        bool passThroughInvalidSquares,
        out int numSquaresInPath)
    {
        numSquaresInPath = 0;
        if (destination == null || startSquare == null)
        {
            return false;
        }

        bool isValid;
        int num = 1;
        if (destination == startSquare)
        {
            isValid = true;
            num = 2;
        }
        else
        {
            if (destination != null && (passThroughInvalidSquares || destination.IsValidForKnockbackAndCharge()))
            {
                BoardSquare currentSquare = startSquare;
                BoardSquare nextSquare = GetClosestAdjacentSquareTo(
                    currentSquare,
                    startSquare,
                    destination,
                    passThroughInvalidSquares);
                while (nextSquare != null)
                {
                    num++;
                    currentSquare = nextSquare;
                    nextSquare = GetClosestAdjacentSquareTo(
                        currentSquare,
                        startSquare,
                        destination,
                        passThroughInvalidSquares);
                }

                isValid = currentSquare != null && currentSquare == destination;
            }
            else
            {
                isValid = false;
            }
        }

        numSquaresInPath = num;
        return isValid;
    }

    private static BoardSquare GetClosestAdjacentSquareTo(
        BoardSquare currentSquare,
        BoardSquare originalSquare,
        BoardSquare destinationSquare,
        bool passThroughInvalidSquares)
    {
        if (destinationSquare == null)
        {
            return null;
        }

        Vector2 destination = new Vector2(destinationSquare.x, destinationSquare.y);
        return GetClosestAdjacentSquareTo(currentSquare, originalSquare, destination, passThroughInvalidSquares);
    }

    public static BoardSquare GetClosestAdjacentSquareTo(
        BoardSquare currentSquare,
        BoardSquare originalSquare,
        Vector2 destination,
        bool passThroughInvalidSquares)
    {
        Vector2 originalSquarePos = new Vector2(originalSquare.x, originalSquare.y);
        Vector2 currentSquarePos = new Vector2(currentSquare.x, currentSquare.y);
        float distInSquares = (destination - currentSquarePos).magnitude;
        Vector2 dir = (destination - originalSquarePos).normalized;
        float minDistToProjectedInSquares = 1E+08f;
        float minAngleToProjected = 1000f;
        BoardSquare result = null;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                BoardSquare testSquare = Board.Get().GetSquareFromIndex(currentSquare.x + i, currentSquare.y + j);
                if (testSquare == null)
                {
                    continue;
                }

                Vector2 testSquarePos = new Vector2(testSquare.x, testSquare.y);
                float testDistInSquares = (destination - testSquarePos).magnitude;
                if (testDistInSquares + 0.2f >= distInSquares)
                {
                    continue;
                }

                Vector2 delta = testSquarePos - originalSquarePos;
                Vector2 deltaProjected = originalSquarePos + dir * Vector2.Dot(delta, dir);
                float distToProjectedInSquares = (testSquarePos - deltaProjected).magnitude;
                if (Mathf.Approximately(distToProjectedInSquares, minDistToProjectedInSquares))
                {
                    float angleToProjected = Vector2.Angle(delta, dir);
                    if (angleToProjected < minAngleToProjected)
                    {
                        minAngleToProjected = angleToProjected;
                        result = testSquare;
                    }
                }
                else if (distToProjectedInSquares < minDistToProjectedInSquares)
                {
                    float angleToProjected = Vector2.Angle(delta, dir);
                    minDistToProjectedInSquares = distToProjectedInSquares;
                    minAngleToProjected = angleToProjected;
                    result = testSquare;
                }
            }
        }

        if (result != null
            && !passThroughInvalidSquares
            && !CanForceMoveToAdjacentSquare(currentSquare, result))
        {
            result = null;
        }

        return result;
    }

    private static bool CanForceMoveToAdjacentSquare(BoardSquare src, BoardSquare dest)
    {
        if (!dest.IsValidForKnockbackAndCharge() || !Board.Get().GetSquaresAreAdjacent(src, dest))
        {
            return false;
        }

        if (Board.Get().GetSquaresAreDiagonallyAdjacent(src, dest))
        {
            BoardSquare leftSquare = Board.Get().GetSquareFromIndex(src.x, dest.y);
            BoardSquare rightSquare = Board.Get().GetSquareFromIndex(dest.x, src.y);
            return CanForceMoveToAdjacentSquare(src, leftSquare) && CanForceMoveToAdjacentSquare(leftSquare, dest)
                   || CanForceMoveToAdjacentSquare(src, rightSquare) && CanForceMoveToAdjacentSquare(rightSquare, dest);
        }

        return src.GetThinCover(VectorUtils.GetCoverDirection(src, dest)) != ThinCover.CoverType.Full;
    }

    public static List<Vector3> BuildDrawablePath(BoardSquarePathInfo path, bool directLine)
    {
        List<Vector3> list = new List<Vector3>();

        if (path == null)
        {
            Debug.LogError("Calling BuildDrawablePath with a null path.");
            return list;
        }

        if (path.square == null)
        {
            Debug.LogError("Calling BuildDrawablePath, but its first square is null.");
            return list;
        }

        if (Board.Get() == null)
        {
            Debug.LogError("Calling BuildDrawablePath, but Board is null.");
            return list;
        }

        GridPos startGridPos = path.square.GetGridPos();
        float y = Board.Get().BaselineHeight + 0.1f;
        Vector3 startNode = new Vector3(startGridPos.worldX, y, startGridPos.worldY);
        list.Add(startNode);
        while (path.next != null && path.next.square != null)
        {
            path = path.next;
            if (!directLine || path.next == null)
            {
                GridPos gridPos = path.square.GetGridPos();
                Vector3 node = new Vector3(gridPos.worldX, y, gridPos.worldY);
                list.Add(node);
            }
        }

        return list;
    }

    public static List<GridPos> BuildGridPosPath(BoardSquarePathInfo path, bool directLine)
    {
        List<GridPos> list = new List<GridPos>();
        GridPos startGridPos = path.square.GetGridPos();
        list.Add(startGridPos);
        while (path.next != null)
        {
            path = path.next;
            if (!directLine || path.next == null)
            {
                list.Add(path.square.GetGridPos());
            }
        }

        return list;
    }
}