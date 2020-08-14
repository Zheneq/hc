using System.Collections.Generic;
using UnityEngine;

public static class KnockbackUtils
{
	public static Vector2 GetKnockbackDeltaForType(KnockbackHitData hitData)
	{
		return GetKnockbackDeltaForType(hitData.m_target, hitData.m_type, hitData.m_aimDir, hitData.m_sourcePos, hitData.m_distance);
	}

	public static Vector2 GetKnockbackDeltaForType(ActorData target, KnockbackType type, Vector3 aimDir, Vector3 sourcePos, float distance)
	{
		aimDir.Normalize();
		Vector3 travelBoardSquareWorldPosition = target.GetTravelBoardSquareWorldPosition();
		Vector3 lhs = travelBoardSquareWorldPosition - sourcePos;
		Vector3 b = sourcePos + Vector3.Dot(lhs, aimDir) * aimDir;
		Vector3 vector = travelBoardSquareWorldPosition - b;
		float squareSize = Board.Get().squareSize;
		Vector2 result;
		switch (type)
		{
		case KnockbackType.ForwardAlongAimDir:
			result = new Vector2(aimDir.x, aimDir.z);
			result.Normalize();
			result *= distance;
			break;
		case KnockbackType.BackwardAgainstAimDir:
			result = new Vector2(aimDir.x, aimDir.z);
			result = -result;
			result.Normalize();
			result *= distance;
			break;
		case KnockbackType.PerpendicularAwayFromAimDir:
			result = new Vector2(vector.x, vector.z);
			result.Normalize();
			result *= distance;
			break;
		case KnockbackType.PerpendicularPullToAimDir:
			result = new Vector2(vector.x, vector.z);
			result = -result;
			if (distance > 0f && result.magnitude > distance)
			{
				result.Normalize();
				result *= distance;
			}
			break;
		case KnockbackType.AwayFromSource:
			result = new Vector2(travelBoardSquareWorldPosition.x - sourcePos.x, travelBoardSquareWorldPosition.z - sourcePos.z);
			result.Normalize();
			result *= distance;
			break;
		case KnockbackType.PullToSource:
			result = new Vector2((sourcePos.x - travelBoardSquareWorldPosition.x) / squareSize, (sourcePos.z - travelBoardSquareWorldPosition.z) / squareSize);
			if (distance > 0f && result.magnitude > distance)
			{
				result.Normalize();
				result *= distance;
			}
			break;
		case KnockbackType.PullToSourceOverShoot:
		{
			result = new Vector2((sourcePos.x - travelBoardSquareWorldPosition.x) / squareSize, (sourcePos.z - travelBoardSquareWorldPosition.z) / squareSize);
			float num2 = result.magnitude + 0.9f;
			float num3 = (!(distance > 0f)) ? num2 : Mathf.Min(distance, num2);
			result.Normalize();
			result *= num3;
			break;
		}
		case KnockbackType.PullToSourceActor:
		{
			result = new Vector2((sourcePos.x - travelBoardSquareWorldPosition.x) / squareSize, (sourcePos.z - travelBoardSquareWorldPosition.z) / squareSize);
			float magnitude = result.magnitude;
			magnitude = Mathf.Max(0f, magnitude - 0.71f);
			float num = magnitude;
			if (distance > 0f)
			{
				if (magnitude > distance)
				{
					num = distance;
				}
			}
			result.Normalize();
			result *= num;
			break;
		}
		default:
			result = Vector2.zero;
			break;
		}
		return result;
	}

	public static BoardSquarePathInfo BuildKnockbackPath(ActorData target, KnockbackType type, Vector3 aimDir, Vector3 sourcePos, float distance)
	{
		Vector2 knockbackDeltaForType = GetKnockbackDeltaForType(target, type, aimDir, sourcePos, distance);
		return BuildKnockbackPathFromVector(target, knockbackDeltaForType);
	}

	public static BoardSquarePathInfo BuildKnockbackPathFromVector(ActorData target, Vector2 idealMoveDir)
	{
		BoardSquare currentBoardSquare = target.GetCurrentBoardSquare();
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo();
		boardSquarePathInfo.square = currentBoardSquare;
		BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
		Vector2 destination = new Vector2((float)currentBoardSquare.x + idealMoveDir.x, (float)currentBoardSquare.y + idealMoveDir.y);
		BoardSquare currentSquare = currentBoardSquare;
		BoardSquare boardSquare = GetClosestAdjacentSquareTo(currentSquare, currentBoardSquare, destination, false);
		bool flag = target.GetComponent<ActorStatus>().HasStatus(StatusType.KnockbackResistant);
		while (boardSquare != null)
		{
			BoardSquarePathInfo boardSquarePathInfo3 = new BoardSquarePathInfo();
			boardSquarePathInfo3.square = boardSquare;
			boardSquarePathInfo3.prev = boardSquarePathInfo2;
			boardSquarePathInfo2.next = boardSquarePathInfo3;
			boardSquarePathInfo2 = boardSquarePathInfo3;
			currentSquare = boardSquare;
			if (flag)
			{
				boardSquare = null;
			}
			else
			{
				boardSquare = GetClosestAdjacentSquareTo(currentSquare, currentBoardSquare, destination, false);
			}
		}
		while (true)
		{
			int num;
			if (boardSquarePathInfo2.square != null)
			{
				num = (boardSquarePathInfo2.square.IsBaselineHeight() ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag2 = (byte)num != 0;
			while (!flag2)
			{
				if (boardSquarePathInfo2.prev == null)
				{
					flag2 = true;
				}
				else
				{
					boardSquarePathInfo2.square = null;
					boardSquarePathInfo2.prev.next = null;
					boardSquarePathInfo2 = boardSquarePathInfo2.prev;
					flag2 = (boardSquarePathInfo2.square != null && boardSquarePathInfo2.square.IsBaselineHeight());
				}
			}
			boardSquarePathInfo.CalcAndSetMoveCostToEnd();
			return boardSquarePathInfo;
		}
	}

	public static BoardSquare GetLastValidBoardSquareInLine(Vector3 start, Vector3 end, bool passThroughInvalidSquares = false, bool considerHalfHeightWallValid = false, float maxDistance = float.MaxValue)
	{
		BoardSquare result = null;
		BoardSquare boardSquare = Board.Get().GetSquare(end);
		bool flag = false;
		if (boardSquare != null)
		{
			if (considerHalfHeightWallValid)
			{
				flag = boardSquare._0015();
			}
			else
			{
				flag = boardSquare.IsBaselineHeight();
			}
		}
		if (flag)
		{
			if (maxDistance >= float.MaxValue)
			{
				result = boardSquare;
				goto IL_0136;
			}
		}
		BoardSquare boardSquare2 = Board.Get().GetSquare(start);
		if (boardSquare2 != null)
		{
			result = boardSquare2;
			GridPos gridPos = GridPos.FromVector3(end);
			BoardSquare currentSquare = boardSquare2;
			Vector2 destination = new Vector2(gridPos.x, gridPos.y);
			BoardSquare closestAdjacentSquareTo = GetClosestAdjacentSquareTo(currentSquare, boardSquare2, destination, passThroughInvalidSquares);
			while (true)
			{
				if (closestAdjacentSquareTo != null)
				{
					float num = boardSquare2.HorizontalDistanceInWorldTo(closestAdjacentSquareTo);
					if (num > maxDistance)
					{
						break;
					}
					currentSquare = closestAdjacentSquareTo;
					if ((!considerHalfHeightWallValid) ? currentSquare.IsBaselineHeight() : currentSquare._0015())
					{
						result = currentSquare;
					}
					closestAdjacentSquareTo = GetClosestAdjacentSquareTo(currentSquare, boardSquare2, destination, passThroughInvalidSquares);
					continue;
				}
				break;
			}
		}
		goto IL_0136;
		IL_0136:
		return result;
	}

	public static BoardSquarePathInfo BuildStraightLineChargePath(ActorData mover, BoardSquare destination, BoardSquare startSquare, bool passThroughInvalidSquares)
	{
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo();
		boardSquarePathInfo.square = startSquare;
		if (destination == startSquare)
		{
			while (true)
			{
				boardSquarePathInfo.next = new BoardSquarePathInfo();
				boardSquarePathInfo.next.prev = boardSquarePathInfo;
				boardSquarePathInfo.next.square = destination;
				return boardSquarePathInfo;
			}
		}
		if (!(destination == null))
		{
			if (!passThroughInvalidSquares)
			{
				if (!destination._0015())
				{
					goto IL_007a;
				}
			}
			BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
			BoardSquare boardSquare = startSquare;
			BoardSquare closestAdjacentSquareTo = GetClosestAdjacentSquareTo(boardSquare, startSquare, destination, passThroughInvalidSquares);
			while (closestAdjacentSquareTo != null)
			{
				BoardSquarePathInfo boardSquarePathInfo3 = new BoardSquarePathInfo();
				boardSquarePathInfo3.square = closestAdjacentSquareTo;
				boardSquarePathInfo3.prev = boardSquarePathInfo2;
				boardSquarePathInfo2.next = boardSquarePathInfo3;
				boardSquarePathInfo2 = boardSquarePathInfo3;
				boardSquare = closestAdjacentSquareTo;
				closestAdjacentSquareTo = GetClosestAdjacentSquareTo(boardSquare, startSquare, destination, passThroughInvalidSquares);
			}
			while (true)
			{
				if (boardSquare != null)
				{
					if (boardSquare == destination)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return boardSquarePathInfo;
							}
						}
					}
				}
				return null;
			}
		}
		goto IL_007a;
		IL_007a:
		return null;
	}

	public static BoardSquarePathInfo BuildStraightLineChargePath(ActorData mover, BoardSquare destination)
	{
		return BuildStraightLineChargePath(mover, destination, mover.GetCurrentBoardSquare(), false);
	}

	public static bool CanBuildStraightLineChargePath(ActorData mover, BoardSquare destination, BoardSquare startSquare, bool passThroughInvalidSquares, out int numSquaresInPath)
	{
		bool flag = false;
		int num = 1;
		numSquaresInPath = 0;
		if (!(destination == null))
		{
			if (!(startSquare == null))
			{
				if (destination == startSquare)
				{
					flag = true;
					num = 2;
				}
				else
				{
					if (!(destination == null))
					{
						if (passThroughInvalidSquares || destination._0015())
						{
							BoardSquare boardSquare = startSquare;
							BoardSquare closestAdjacentSquareTo = GetClosestAdjacentSquareTo(boardSquare, startSquare, destination, passThroughInvalidSquares);
							while (closestAdjacentSquareTo != null)
							{
								num++;
								boardSquare = closestAdjacentSquareTo;
								closestAdjacentSquareTo = GetClosestAdjacentSquareTo(boardSquare, startSquare, destination, passThroughInvalidSquares);
							}
							if (boardSquare != null)
							{
								if (boardSquare == destination)
								{
									flag = true;
									goto IL_00de;
								}
							}
							flag = false;
							goto IL_00de;
						}
					}
					flag = false;
				}
				goto IL_00de;
			}
		}
		return false;
		IL_00de:
		numSquaresInPath = num;
		return flag;
	}

	private static BoardSquare GetClosestAdjacentSquareTo(BoardSquare currentSquare, BoardSquare originalSquare, BoardSquare destinationSquare, bool passThroughInvalidSquares)
	{
		if (destinationSquare == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		Vector2 destination = new Vector2(destinationSquare.x, destinationSquare.y);
		return GetClosestAdjacentSquareTo(currentSquare, originalSquare, destination, passThroughInvalidSquares);
	}

	public static BoardSquare GetClosestAdjacentSquareTo(BoardSquare currentSquare, BoardSquare originalSquare, Vector2 destination, bool passThroughInvalidSquares)
	{
		Vector2 vector = new Vector2(originalSquare.x, originalSquare.y);
		Vector2 b = new Vector2(currentSquare.x, currentSquare.y);
		float magnitude = (destination - b).magnitude;
		Vector2 normalized = (destination - vector).normalized;
		float num = 1E+08f;
		float num2 = 1000f;
		BoardSquare boardSquare = null;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (i == 0)
				{
					if (j == 0)
					{
						continue;
					}
				}
				BoardSquare boardSquare2 = Board.Get().GetSquare(currentSquare.x + i, currentSquare.y + j);
				if (boardSquare2 == null)
				{
					continue;
				}
				Vector2 vector2 = new Vector2(boardSquare2.x, boardSquare2.y);
				float magnitude2 = (destination - vector2).magnitude;
				if (magnitude2 + 0.2f >= magnitude)
				{
					continue;
				}
				Vector2 vector3 = vector2 - vector;
				float d = Vector2.Dot(vector3, normalized);
				Vector2 b2 = vector + normalized * d;
				float magnitude3 = (vector2 - b2).magnitude;
				if (Mathf.Approximately(magnitude3, num))
				{
					float num3 = Vector2.Angle(vector3, normalized);
					if (num3 < num2)
					{
						num2 = num3;
						boardSquare = boardSquare2;
					}
				}
				else if (magnitude3 < num)
				{
					float num4 = Vector2.Angle(vector3, normalized);
					num = magnitude3;
					num2 = num4;
					boardSquare = boardSquare2;
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					goto end_IL_01b1;
				}
				continue;
				end_IL_01b1:
				break;
			}
		}
		if (boardSquare != null && !passThroughInvalidSquares)
		{
			if (!CanForceMoveToAdjacentSquare(currentSquare, boardSquare))
			{
				boardSquare = null;
			}
		}
		return boardSquare;
	}

	private static bool CanForceMoveToAdjacentSquare(BoardSquare src, BoardSquare dest)
	{
		if (!dest._0015())
		{
			return false;
		}
		if (!Board.Get().AreAdjacent(src, dest))
		{
			while (true)
			{
				return false;
			}
		}
		bool flag = true;
		bool flag2 = true;
		if (Board.Get().AreDiagonallyAdjacent(src, dest))
		{
			BoardSquare boardSquare = Board.Get().GetSquare(src.x, dest.y);
			BoardSquare boardSquare2 = Board.Get().GetSquare(dest.x, src.y);
			if (CanForceMoveToAdjacentSquare(src, boardSquare))
			{
				if (CanForceMoveToAdjacentSquare(boardSquare, dest))
				{
					flag = true;
					goto IL_00f7;
				}
			}
			if (CanForceMoveToAdjacentSquare(src, boardSquare2))
			{
				if (CanForceMoveToAdjacentSquare(boardSquare2, dest))
				{
					flag = true;
					goto IL_00f7;
				}
			}
			flag = false;
		}
		else if (src.GetCoverInDirection(VectorUtils.GetCoverDirection(src, dest)) == ThinCover.CoverType.Full)
		{
			flag2 = false;
		}
		goto IL_00f7;
		IL_00f7:
		if (!flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (!flag2)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return true;
	}

	public static List<Vector3> BuildDrawablePath(BoardSquarePathInfo path, bool directLine)
	{
		List<Vector3> list = new List<Vector3>();
		if (path == null)
		{
			Debug.LogError("Calling BuildDrawablePath with a null path.");
		}
		else if (path.square == null)
		{
			Debug.LogError("Calling BuildDrawablePath, but its first square is null.");
		}
		else if (Board.Get() == null)
		{
			Debug.LogError("Calling BuildDrawablePath, but Board is null.");
		}
		else
		{
			Board board = Board.Get();
			GridPos gridPos = path.square.GetGridPos();
			float y = (float)board.BaselineHeight + 0.1f;
			Vector3 item = new Vector3(gridPos.worldX, y, gridPos.worldY);
			list.Add(item);
			while (path.next != null)
			{
				if (path.next.square != null)
				{
					path = path.next;
					if (directLine)
					{
						if (path.next != null)
						{
							continue;
						}
					}
					GridPos gridPos2 = path.square.GetGridPos();
					Vector3 item2 = new Vector3(gridPos2.worldX, y, gridPos2.worldY);
					list.Add(item2);
					continue;
				}
				break;
			}
		}
		return list;
	}

	public static List<GridPos> BuildGridPosPath(BoardSquarePathInfo path, bool directLine)
	{
		List<GridPos> list = new List<GridPos>();
		GridPos gridPos = path.square.GetGridPos();
		list.Add(gridPos);
		while (path.next != null)
		{
			path = path.next;
			if (directLine)
			{
				if (path.next != null)
				{
					continue;
				}
			}
			GridPos gridPos2 = path.square.GetGridPos();
			list.Add(gridPos2);
		}
		while (true)
		{
			return list;
		}
	}
}
