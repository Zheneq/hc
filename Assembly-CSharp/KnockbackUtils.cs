using System;
using System.Collections.Generic;
using UnityEngine;

public static class KnockbackUtils
{
	public static Vector2 GetKnockbackDeltaForType(KnockbackHitData hitData)
	{
		return KnockbackUtils.GetKnockbackDeltaForType(hitData.m_target, hitData.m_type, hitData.m_aimDir, hitData.m_sourcePos, hitData.m_distance);
	}

	public static Vector2 GetKnockbackDeltaForType(ActorData target, KnockbackType type, Vector3 aimDir, Vector3 sourcePos, float distance)
	{
		aimDir.Normalize();
		Vector3 travelBoardSquareWorldPosition = target.GetTravelBoardSquareWorldPosition();
		Vector3 lhs = travelBoardSquareWorldPosition - sourcePos;
		Vector3 b = sourcePos + Vector3.Dot(lhs, aimDir) * aimDir;
		Vector3 vector = travelBoardSquareWorldPosition - b;
		float squareSize = Board.Get().squareSize;
		Vector2 vector2;
		switch (type)
		{
		case KnockbackType.ForwardAlongAimDir:
			vector2 = new Vector2(aimDir.x, aimDir.z);
			vector2.Normalize();
			vector2 *= distance;
			break;
		case KnockbackType.BackwardAgainstAimDir:
			vector2 = new Vector2(aimDir.x, aimDir.z);
			vector2 = -vector2;
			vector2.Normalize();
			vector2 *= distance;
			break;
		case KnockbackType.PerpendicularAwayFromAimDir:
			vector2 = new Vector2(vector.x, vector.z);
			vector2.Normalize();
			vector2 *= distance;
			break;
		case KnockbackType.PerpendicularPullToAimDir:
			vector2 = new Vector2(vector.x, vector.z);
			vector2 = -vector2;
			if (distance > 0f && vector2.magnitude > distance)
			{
				vector2.Normalize();
				vector2 *= distance;
			}
			break;
		case KnockbackType.AwayFromSource:
			vector2 = new Vector2(travelBoardSquareWorldPosition.x - sourcePos.x, travelBoardSquareWorldPosition.z - sourcePos.z);
			vector2.Normalize();
			vector2 *= distance;
			break;
		case KnockbackType.PullToSource:
			vector2 = new Vector2((sourcePos.x - travelBoardSquareWorldPosition.x) / squareSize, (sourcePos.z - travelBoardSquareWorldPosition.z) / squareSize);
			if (distance > 0f && vector2.magnitude > distance)
			{
				vector2.Normalize();
				vector2 *= distance;
			}
			break;
		case KnockbackType.PullToSourceOverShoot:
		{
			vector2 = new Vector2((sourcePos.x - travelBoardSquareWorldPosition.x) / squareSize, (sourcePos.z - travelBoardSquareWorldPosition.z) / squareSize);
			float num = vector2.magnitude + 0.9f;
			float d = (distance <= 0f) ? num : Mathf.Min(distance, num);
			vector2.Normalize();
			vector2 *= d;
			break;
		}
		case KnockbackType.PullToSourceActor:
		{
			vector2 = new Vector2((sourcePos.x - travelBoardSquareWorldPosition.x) / squareSize, (sourcePos.z - travelBoardSquareWorldPosition.z) / squareSize);
			float num2 = vector2.magnitude;
			num2 = Mathf.Max(0f, num2 - 0.71f);
			float d2 = num2;
			if (distance > 0f)
			{
				if (num2 > distance)
				{
					d2 = distance;
				}
			}
			vector2.Normalize();
			vector2 *= d2;
			break;
		}
		default:
			vector2 = Vector2.zero;
			break;
		}
		return vector2;
	}

	public static BoardSquarePathInfo BuildKnockbackPath(ActorData target, KnockbackType type, Vector3 aimDir, Vector3 sourcePos, float distance)
	{
		Vector2 knockbackDeltaForType = KnockbackUtils.GetKnockbackDeltaForType(target, type, aimDir, sourcePos, distance);
		return KnockbackUtils.BuildKnockbackPathFromVector(target, knockbackDeltaForType);
	}

	public static BoardSquarePathInfo BuildKnockbackPathFromVector(ActorData target, Vector2 idealMoveDir)
	{
		BoardSquare currentBoardSquare = target.GetCurrentBoardSquare();
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo();
		boardSquarePathInfo.square = currentBoardSquare;
		BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
		Vector2 destination = new Vector2((float)currentBoardSquare.x + idealMoveDir.x, (float)currentBoardSquare.y + idealMoveDir.y);
		BoardSquare currentSquare = currentBoardSquare;
		BoardSquare boardSquare = KnockbackUtils.GetClosestAdjacentSquareTo(currentSquare, currentBoardSquare, destination, false);
		bool flag = target.GetComponent<ActorStatus>().HasStatus(StatusType.KnockbackResistant, true);
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
				boardSquare = KnockbackUtils.GetClosestAdjacentSquareTo(currentSquare, currentBoardSquare, destination, false);
			}
		}
		bool flag2;
		if (boardSquarePathInfo2.square != null)
		{
			flag2 = boardSquarePathInfo2.square.IsBaselineHeight();
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		while (!flag3)
		{
			if (boardSquarePathInfo2.prev == null)
			{
				flag3 = true;
			}
			else
			{
				boardSquarePathInfo2.square = null;
				boardSquarePathInfo2.prev.next = null;
				boardSquarePathInfo2 = boardSquarePathInfo2.prev;
				flag3 = (boardSquarePathInfo2.square != null && boardSquarePathInfo2.square.IsBaselineHeight());
			}
		}
		boardSquarePathInfo.CalcAndSetMoveCostToEnd();
		return boardSquarePathInfo;
	}

	public static BoardSquare GetLastValidBoardSquareInLine(Vector3 start, Vector3 end, bool passThroughInvalidSquares = false, bool considerHalfHeightWallValid = false, float maxDistance = 3.40282347E+38f)
	{
		BoardSquare result = null;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(end);
		bool flag = false;
		if (boardSquare != null)
		{
			if (considerHalfHeightWallValid)
			{
				flag = boardSquare.symbol_0015();
			}
			else
			{
				flag = boardSquare.IsBaselineHeight();
			}
		}
		if (flag)
		{
			if (maxDistance >= 3.40282347E+38f)
			{
				return boardSquare;
			}
		}
		BoardSquare boardSquare2 = Board.Get().GetBoardSquare(start);
		if (boardSquare2 != null)
		{
			result = boardSquare2;
			GridPos gridPos = GridPos.FromVector3(end);
			BoardSquare boardSquare3 = boardSquare2;
			Vector2 destination = new Vector2((float)gridPos.x, (float)gridPos.y);
			BoardSquare closestAdjacentSquareTo = KnockbackUtils.GetClosestAdjacentSquareTo(boardSquare3, boardSquare2, destination, passThroughInvalidSquares);
			while (closestAdjacentSquareTo != null)
			{
				float num = boardSquare2.HorizontalDistanceInWorldTo(closestAdjacentSquareTo);
				if (num > maxDistance)
				{
					return result;
				}
				boardSquare3 = closestAdjacentSquareTo;
				bool flag2 = (!considerHalfHeightWallValid) ? boardSquare3.IsBaselineHeight() : boardSquare3.symbol_0015();
				if (flag2)
				{
					result = boardSquare3;
				}
				closestAdjacentSquareTo = KnockbackUtils.GetClosestAdjacentSquareTo(boardSquare3, boardSquare2, destination, passThroughInvalidSquares);
			}
		}
		return result;
	}

	public static BoardSquarePathInfo BuildStraightLineChargePath(ActorData mover, BoardSquare destination, BoardSquare startSquare, bool passThroughInvalidSquares)
	{
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo();
		boardSquarePathInfo.square = startSquare;
		if (destination == startSquare)
		{
			boardSquarePathInfo.next = new BoardSquarePathInfo();
			boardSquarePathInfo.next.prev = boardSquarePathInfo;
			boardSquarePathInfo.next.square = destination;
			return boardSquarePathInfo;
		}
		if (!(destination == null))
		{
			if (!passThroughInvalidSquares)
			{
				if (!destination.symbol_0015())
				{
					goto IL_7A;
				}
			}
			BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
			BoardSquare boardSquare = startSquare;
			BoardSquare closestAdjacentSquareTo = KnockbackUtils.GetClosestAdjacentSquareTo(boardSquare, startSquare, destination, passThroughInvalidSquares);
			while (closestAdjacentSquareTo != null)
			{
				BoardSquarePathInfo boardSquarePathInfo3 = new BoardSquarePathInfo();
				boardSquarePathInfo3.square = closestAdjacentSquareTo;
				boardSquarePathInfo3.prev = boardSquarePathInfo2;
				boardSquarePathInfo2.next = boardSquarePathInfo3;
				boardSquarePathInfo2 = boardSquarePathInfo3;
				boardSquare = closestAdjacentSquareTo;
				closestAdjacentSquareTo = KnockbackUtils.GetClosestAdjacentSquareTo(boardSquare, startSquare, destination, passThroughInvalidSquares);
			}
			if (boardSquare != null)
			{
				if (boardSquare == destination)
				{
					return boardSquarePathInfo;
				}
			}
			return null;
		}
		IL_7A:
		return null;
	}

	public static BoardSquarePathInfo BuildStraightLineChargePath(ActorData mover, BoardSquare destination)
	{
		return KnockbackUtils.BuildStraightLineChargePath(mover, destination, mover.GetCurrentBoardSquare(), false);
	}

	public unsafe static bool CanBuildStraightLineChargePath(ActorData mover, BoardSquare destination, BoardSquare startSquare, bool passThroughInvalidSquares, out int numSquaresInPath)
	{
		int num = 1;
		numSquaresInPath = 0;
		if (!(destination == null))
		{
			if (!(startSquare == null))
			{
				bool result;
				if (destination == startSquare)
				{
					result = true;
					num = 2;
				}
				else
				{
					if (!(destination == null))
					{
						if (passThroughInvalidSquares || destination.symbol_0015())
						{
							BoardSquare boardSquare = startSquare;
							BoardSquare closestAdjacentSquareTo = KnockbackUtils.GetClosestAdjacentSquareTo(boardSquare, startSquare, destination, passThroughInvalidSquares);
							while (closestAdjacentSquareTo != null)
							{
								num++;
								boardSquare = closestAdjacentSquareTo;
								closestAdjacentSquareTo = KnockbackUtils.GetClosestAdjacentSquareTo(boardSquare, startSquare, destination, passThroughInvalidSquares);
							}
							if (boardSquare != null)
							{
								if (boardSquare == destination)
								{
									result = true;
									goto IL_DE;
								}
							}
							result = false;
							goto IL_DE;
						}
					}
					result = false;
				}
				IL_DE:
				numSquaresInPath = num;
				return result;
			}
		}
		return false;
	}

	private static BoardSquare GetClosestAdjacentSquareTo(BoardSquare currentSquare, BoardSquare originalSquare, BoardSquare destinationSquare, bool passThroughInvalidSquares)
	{
		if (destinationSquare == null)
		{
			return null;
		}
		Vector2 destination = new Vector2((float)destinationSquare.x, (float)destinationSquare.y);
		return KnockbackUtils.GetClosestAdjacentSquareTo(currentSquare, originalSquare, destination, passThroughInvalidSquares);
	}

	public static BoardSquare GetClosestAdjacentSquareTo(BoardSquare currentSquare, BoardSquare originalSquare, Vector2 destination, bool passThroughInvalidSquares)
	{
		Vector2 vector = new Vector2((float)originalSquare.x, (float)originalSquare.y);
		Vector2 b = new Vector2((float)currentSquare.x, (float)currentSquare.y);
		float magnitude = (destination - b).magnitude;
		Vector2 normalized = (destination - vector).normalized;
		float num = 1E+08f;
		float num2 = 1000f;
		BoardSquare boardSquare = null;
		for (int i = -1; i <= 1; i++)
		{
			int j = -1;
			while (j <= 1)
			{
				if (i != 0)
				{
					goto IL_97;
				}
				if (j != 0)
				{
					goto IL_97;
				}
				IL_1A3:
				j++;
				continue;
				IL_97:
				BoardSquare boardSquare2 = Board.Get().GetBoardSquare(currentSquare.x + i, currentSquare.y + j);
				if (boardSquare2 == null)
				{
					goto IL_1A3;
				}
				Vector2 vector2 = new Vector2((float)boardSquare2.x, (float)boardSquare2.y);
				float magnitude2 = (destination - vector2).magnitude;
				if (magnitude2 + 0.2f >= magnitude)
				{
					goto IL_1A3;
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
					goto IL_1A3;
				}
				if (magnitude3 < num)
				{
					float num4 = Vector2.Angle(vector3, normalized);
					num = magnitude3;
					num2 = num4;
					boardSquare = boardSquare2;
					goto IL_1A3;
				}
				goto IL_1A3;
			}
		}
		if (boardSquare != null && !passThroughInvalidSquares)
		{
			if (!KnockbackUtils.CanForceMoveToAdjacentSquare(currentSquare, boardSquare))
			{
				boardSquare = null;
			}
		}
		return boardSquare;
	}

	private static bool CanForceMoveToAdjacentSquare(BoardSquare src, BoardSquare dest)
	{
		if (!dest.symbol_0015())
		{
			return false;
		}
		if (!Board.Get().AreAdjacent(src, dest))
		{
			return false;
		}
		bool flag = true;
		bool flag2 = true;
		if (Board.Get().AreDiagonallyAdjacent(src, dest))
		{
			BoardSquare boardSquare = Board.Get().GetBoardSquare(src.x, dest.y);
			BoardSquare boardSquare2 = Board.Get().GetBoardSquare(dest.x, src.y);
			if (KnockbackUtils.CanForceMoveToAdjacentSquare(src, boardSquare))
			{
				if (KnockbackUtils.CanForceMoveToAdjacentSquare(boardSquare, dest))
				{
					flag = true;
					goto IL_E3;
				}
			}
			if (KnockbackUtils.CanForceMoveToAdjacentSquare(src, boardSquare2))
			{
				if (KnockbackUtils.CanForceMoveToAdjacentSquare(boardSquare2, dest))
				{
					flag = true;
					goto IL_E3;
				}
			}
			flag = false;
			IL_E3:;
		}
		else if (src.GetCoverInDirection(VectorUtils.GetCoverDirection(src, dest)) == ThinCover.CoverType.Full)
		{
			flag2 = false;
		}
		if (!flag)
		{
			return false;
		}
		if (!flag2)
		{
			return false;
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
				if (!(path.next.square != null))
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						return list;
					}
				}
				else
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
				}
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
		return list;
	}
}
