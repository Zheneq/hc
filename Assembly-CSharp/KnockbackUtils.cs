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
		Vector3 a = target.\u0016();
		Vector3 lhs = a - sourcePos;
		Vector3 b = sourcePos + Vector3.Dot(lhs, aimDir) * aimDir;
		Vector3 vector = a - b;
		float squareSize = Board.\u000E().squareSize;
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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(KnockbackUtils.GetKnockbackDeltaForType(ActorData, KnockbackType, Vector3, Vector3, float)).MethodHandle;
				}
				vector2.Normalize();
				vector2 *= distance;
			}
			break;
		case KnockbackType.AwayFromSource:
			vector2 = new Vector2(a.x - sourcePos.x, a.z - sourcePos.z);
			vector2.Normalize();
			vector2 *= distance;
			break;
		case KnockbackType.PullToSource:
			vector2 = new Vector2((sourcePos.x - a.x) / squareSize, (sourcePos.z - a.z) / squareSize);
			if (distance > 0f && vector2.magnitude > distance)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				vector2.Normalize();
				vector2 *= distance;
			}
			break;
		case KnockbackType.PullToSourceOverShoot:
		{
			vector2 = new Vector2((sourcePos.x - a.x) / squareSize, (sourcePos.z - a.z) / squareSize);
			float num = vector2.magnitude + 0.9f;
			float d = (distance <= 0f) ? num : Mathf.Min(distance, num);
			vector2.Normalize();
			vector2 *= d;
			break;
		}
		case KnockbackType.PullToSourceActor:
		{
			vector2 = new Vector2((sourcePos.x - a.x) / squareSize, (sourcePos.z - a.z) / squareSize);
			float num2 = vector2.magnitude;
			num2 = Mathf.Max(0f, num2 - 0.71f);
			float d2 = num2;
			if (distance > 0f)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (num2 > distance)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
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
		BoardSquare boardSquare = target.\u0012();
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo();
		boardSquarePathInfo.square = boardSquare;
		BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
		Vector2 destination = new Vector2((float)boardSquare.x + idealMoveDir.x, (float)boardSquare.y + idealMoveDir.y);
		BoardSquare currentSquare = boardSquare;
		BoardSquare boardSquare2 = KnockbackUtils.GetClosestAdjacentSquareTo(currentSquare, boardSquare, destination, false);
		bool flag = target.GetComponent<ActorStatus>().HasStatus(StatusType.KnockbackResistant, true);
		while (boardSquare2 != null)
		{
			BoardSquarePathInfo boardSquarePathInfo3 = new BoardSquarePathInfo();
			boardSquarePathInfo3.square = boardSquare2;
			boardSquarePathInfo3.prev = boardSquarePathInfo2;
			boardSquarePathInfo2.next = boardSquarePathInfo3;
			boardSquarePathInfo2 = boardSquarePathInfo3;
			currentSquare = boardSquare2;
			if (flag)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(KnockbackUtils.BuildKnockbackPathFromVector(ActorData, Vector2)).MethodHandle;
				}
				boardSquare2 = null;
			}
			else
			{
				boardSquare2 = KnockbackUtils.GetClosestAdjacentSquareTo(currentSquare, boardSquare, destination, false);
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		bool flag2;
		if (boardSquarePathInfo2.square != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			flag2 = boardSquarePathInfo2.square.\u0016();
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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				flag3 = true;
			}
			else
			{
				boardSquarePathInfo2.square = null;
				boardSquarePathInfo2.prev.next = null;
				boardSquarePathInfo2 = boardSquarePathInfo2.prev;
				flag3 = (boardSquarePathInfo2.square != null && boardSquarePathInfo2.square.\u0016());
			}
		}
		boardSquarePathInfo.CalcAndSetMoveCostToEnd();
		return boardSquarePathInfo;
	}

	public static BoardSquare GetLastValidBoardSquareInLine(Vector3 start, Vector3 end, bool passThroughInvalidSquares = false, bool considerHalfHeightWallValid = false, float maxDistance = 3.40282347E+38f)
	{
		BoardSquare result = null;
		BoardSquare boardSquare = Board.\u000E().\u000E(end);
		bool flag = false;
		if (boardSquare != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(KnockbackUtils.GetLastValidBoardSquareInLine(Vector3, Vector3, bool, bool, float)).MethodHandle;
			}
			if (considerHalfHeightWallValid)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				flag = boardSquare.\u0015();
			}
			else
			{
				flag = boardSquare.\u0016();
			}
		}
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (maxDistance >= 3.40282347E+38f)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				return boardSquare;
			}
		}
		BoardSquare boardSquare2 = Board.\u000E().\u000E(start);
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
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					return result;
				}
				boardSquare3 = closestAdjacentSquareTo;
				bool flag2 = (!considerHalfHeightWallValid) ? boardSquare3.\u0016() : boardSquare3.\u0015();
				if (flag2)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					result = boardSquare3;
				}
				closestAdjacentSquareTo = KnockbackUtils.GetClosestAdjacentSquareTo(boardSquare3, boardSquare2, destination, passThroughInvalidSquares);
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(KnockbackUtils.BuildStraightLineChargePath(ActorData, BoardSquare, BoardSquare, bool)).MethodHandle;
			}
			boardSquarePathInfo.next = new BoardSquarePathInfo();
			boardSquarePathInfo.next.prev = boardSquarePathInfo;
			boardSquarePathInfo.next.square = destination;
			return boardSquarePathInfo;
		}
		if (!(destination == null))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!passThroughInvalidSquares)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!destination.\u0015())
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (boardSquare != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (boardSquare == destination)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
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
		return KnockbackUtils.BuildStraightLineChargePath(mover, destination, mover.\u0012(), false);
	}

	public unsafe static bool CanBuildStraightLineChargePath(ActorData mover, BoardSquare destination, BoardSquare startSquare, bool passThroughInvalidSquares, out int numSquaresInPath)
	{
		int num = 1;
		numSquaresInPath = 0;
		if (!(destination == null))
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(KnockbackUtils.CanBuildStraightLineChargePath(ActorData, BoardSquare, BoardSquare, bool, int*)).MethodHandle;
			}
			if (!(startSquare == null))
			{
				bool result;
				if (destination == startSquare)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					result = true;
					num = 2;
				}
				else
				{
					if (!(destination == null))
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (passThroughInvalidSquares || destination.\u0015())
						{
							BoardSquare boardSquare = startSquare;
							BoardSquare closestAdjacentSquareTo = KnockbackUtils.GetClosestAdjacentSquareTo(boardSquare, startSquare, destination, passThroughInvalidSquares);
							while (closestAdjacentSquareTo != null)
							{
								num++;
								boardSquare = closestAdjacentSquareTo;
								closestAdjacentSquareTo = KnockbackUtils.GetClosestAdjacentSquareTo(boardSquare, startSquare, destination, passThroughInvalidSquares);
							}
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (boardSquare != null)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (boardSquare == destination)
								{
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(KnockbackUtils.GetClosestAdjacentSquareTo(BoardSquare, BoardSquare, BoardSquare, bool)).MethodHandle;
			}
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(KnockbackUtils.GetClosestAdjacentSquareTo(BoardSquare, BoardSquare, Vector2, bool)).MethodHandle;
				}
				if (j != 0)
				{
					goto IL_97;
				}
				IL_1A3:
				j++;
				continue;
				IL_97:
				BoardSquare boardSquare2 = Board.\u000E().\u0016(currentSquare.x + i, currentSquare.y + j);
				if (boardSquare2 == null)
				{
					goto IL_1A3;
				}
				Vector2 vector2 = new Vector2((float)boardSquare2.x, (float)boardSquare2.y);
				float magnitude2 = (destination - vector2).magnitude;
				if (magnitude2 + 0.2f >= magnitude)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
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
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					float num4 = Vector2.Angle(vector3, normalized);
					num = magnitude3;
					num2 = num4;
					boardSquare = boardSquare2;
					goto IL_1A3;
				}
				goto IL_1A3;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (boardSquare != null && !passThroughInvalidSquares)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!KnockbackUtils.CanForceMoveToAdjacentSquare(currentSquare, boardSquare))
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				boardSquare = null;
			}
		}
		return boardSquare;
	}

	private static bool CanForceMoveToAdjacentSquare(BoardSquare src, BoardSquare dest)
	{
		if (!dest.\u0015())
		{
			return false;
		}
		if (!Board.\u000E().\u000E(src, dest))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(KnockbackUtils.CanForceMoveToAdjacentSquare(BoardSquare, BoardSquare)).MethodHandle;
			}
			return false;
		}
		bool flag = true;
		bool flag2 = true;
		if (Board.\u000E().\u0015(src, dest))
		{
			BoardSquare boardSquare = Board.\u000E().\u0016(src.x, dest.y);
			BoardSquare boardSquare2 = Board.\u000E().\u0016(dest.x, src.y);
			if (KnockbackUtils.CanForceMoveToAdjacentSquare(src, boardSquare))
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (KnockbackUtils.CanForceMoveToAdjacentSquare(boardSquare, dest))
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
					goto IL_E3;
				}
			}
			if (KnockbackUtils.CanForceMoveToAdjacentSquare(src, boardSquare2))
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (KnockbackUtils.CanForceMoveToAdjacentSquare(boardSquare2, dest))
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
					goto IL_E3;
				}
			}
			flag = false;
			IL_E3:;
		}
		else if (src.\u001D(VectorUtils.GetCoverDirection(src, dest)) == ThinCover.CoverType.Full)
		{
			flag2 = false;
		}
		if (!flag)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			return false;
		}
		if (!flag2)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			return false;
		}
		return true;
	}

	public static List<Vector3> BuildDrawablePath(BoardSquarePathInfo path, bool directLine)
	{
		List<Vector3> list = new List<Vector3>();
		if (path == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(KnockbackUtils.BuildDrawablePath(BoardSquarePathInfo, bool)).MethodHandle;
			}
			Debug.LogError("Calling BuildDrawablePath with a null path.");
		}
		else if (path.square == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			Debug.LogError("Calling BuildDrawablePath, but its first square is null.");
		}
		else if (Board.\u000E() == null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			Debug.LogError("Calling BuildDrawablePath, but Board is null.");
		}
		else
		{
			Board board = Board.\u000E();
			GridPos gridPos = path.square.\u001D();
			float y = (float)board.BaselineHeight + 0.1f;
			Vector3 item = new Vector3(gridPos.worldX, y, gridPos.worldY);
			list.Add(item);
			while (path.next != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
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
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (path.next != null)
						{
							continue;
						}
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					GridPos gridPos2 = path.square.\u001D();
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
		GridPos item = path.square.\u001D();
		list.Add(item);
		while (path.next != null)
		{
			path = path.next;
			if (directLine)
			{
				if (path.next != null)
				{
					continue;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(KnockbackUtils.BuildGridPosPath(BoardSquarePathInfo, bool)).MethodHandle;
				}
			}
			GridPos item2 = path.square.\u001D();
			list.Add(item2);
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		return list;
	}
}
