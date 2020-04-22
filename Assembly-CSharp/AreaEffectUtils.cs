using System.Collections.Generic;
using UnityEngine;

public static class AreaEffectUtils
{
	public enum StretchConeStyle
	{
		Linear,
		DistanceSquared,
		DistanceCubed,
		DistanceToTheFourthPower,
		DistanceSquareRoot
	}

	public struct BouncingLaserInfo
	{
		public Vector3 m_segmentOrigin;

		public int m_endpointIndex;

		public BouncingLaserInfo(Vector3 segmentOrigin, int endpointIndex)
		{
			m_segmentOrigin = segmentOrigin;
			m_endpointIndex = endpointIndex;
		}
	}

	private static List<ISquareInsideChecker> s_radiusOfLineLosCheckers = new List<ISquareInsideChecker>();

	public static float GetActorTargetingRadius()
	{
		if (GameWideData.Get() != null)
		{
			return GameWideData.Get().m_actorTargetingRadiusInSquares;
		}
		return 0.5f;
	}

	public static float GetMaxAngleForActorBounce()
	{
		float num = 90f;
		if (GameWideData.Get() != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = GameWideData.Get().m_maxAngleForBounceOnActor;
			if (num <= 0f)
			{
				num = 360f;
			}
		}
		return num;
	}

	public static bool IsActorTargetable(ActorData actor, List<Team> validTeams = null)
	{
		int result;
		if (actor != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!actor.IsDead() && !actor.IgnoreForAbilityHits)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (actor.GetCurrentBoardSquare() != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					result = (IsRelevantTeam(validTeams, actor.GetTeam()) ? 1 : 0);
					goto IL_0065;
				}
			}
		}
		result = 0;
		goto IL_0065;
		IL_0065:
		return (byte)result != 0;
	}

	public static bool IsRelevantTeam(List<Team> validTeams, Team team)
	{
		if (validTeams == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return true;
				}
			}
		}
		for (int i = 0; i < validTeams.Count; i++)
		{
			if (validTeams[i] != team)
			{
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				return true;
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			return false;
		}
	}

	public static List<BoardSquare> GetValidRespawnSquaresInDonut(float centerX, float centerY, float innerRadius, float outerRadius)
	{
		float squareSize = Board.Get().squareSize;
		innerRadius *= squareSize;
		outerRadius *= squareSize;
		List<BoardSquare> list = new List<BoardSquare>();
		Vector3 a = new Vector3(centerX, 0f, centerY);
		int maxX = Board.Get().GetMaxX();
		int maxY = Board.Get().GetMaxY();
		int num = (int)Mathf.Max(0f, (centerX - outerRadius) / squareSize);
		int num2 = (int)Mathf.Min(maxX, (centerX + outerRadius) / squareSize);
		int num3 = (int)Mathf.Max(0f, (centerY - outerRadius) / squareSize);
		int num4 = (int)Mathf.Min(maxY, (centerY + outerRadius) / squareSize);
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				Vector3 b = new Vector3((float)i * squareSize, 0f, (float)j * squareSize);
				float sqrMagnitude = (a - b).sqrMagnitude;
				if (!(sqrMagnitude < outerRadius * outerRadius) || !(sqrMagnitude > innerRadius * innerRadius))
				{
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				BoardSquare boardSquare = Board.Get().GetBoardSquare(i, j);
				if (!(boardSquare != null))
				{
					continue;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!boardSquare.IsBaselineHeight())
				{
					continue;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (boardSquare.OccupantActor == null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					list.Add(boardSquare);
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					goto end_IL_0167;
				}
				continue;
				end_IL_0167:
				break;
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			return list;
		}
	}

	private static void AddSquareAtIndexToListIfValid(int x, int y, List<BoardSquare> list)
	{
		BoardSquare boardSquare = Board.Get().GetBoardSquare(x, y);
		if (!(boardSquare != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			list.Add(boardSquare);
			return;
		}
	}

	public static List<BoardSquare> GetSquaresInBorderLayer(BoardSquare center, int borderLayerNumber, bool requireLosToCenter)
	{
		if (!(center == null))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (borderLayerNumber >= 0)
			{
				List<BoardSquare> list = new List<BoardSquare>();
				if (borderLayerNumber == 0)
				{
					list.Add(center);
				}
				else
				{
					AddSquareAtIndexToListIfValid(center.x + borderLayerNumber, center.y, list);
					AddSquareAtIndexToListIfValid(center.x - borderLayerNumber, center.y, list);
					AddSquareAtIndexToListIfValid(center.x, center.y + borderLayerNumber, list);
					AddSquareAtIndexToListIfValid(center.x, center.y - borderLayerNumber, list);
					for (int i = 1; i <= borderLayerNumber; i++)
					{
						int num = borderLayerNumber;
						int num2 = i;
						AddSquareAtIndexToListIfValid(center.x + num, center.y + num2, list);
						AddSquareAtIndexToListIfValid(center.x - num, center.y + num2, list);
						AddSquareAtIndexToListIfValid(center.x + num, center.y - num2, list);
						AddSquareAtIndexToListIfValid(center.x - num, center.y - num2, list);
						if (num != num2)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							num = i;
							num2 = borderLayerNumber;
							AddSquareAtIndexToListIfValid(center.x + num, center.y + num2, list);
							AddSquareAtIndexToListIfValid(center.x - num, center.y + num2, list);
							AddSquareAtIndexToListIfValid(center.x + num, center.y - num2, list);
							AddSquareAtIndexToListIfValid(center.x - num, center.y - num2, list);
						}
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (requireLosToCenter)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						for (int num3 = list.Count - 1; num3 >= 0; num3--)
						{
							BoardSquare boardSquare = list[num3];
							if (!center._0013(boardSquare.x, boardSquare.y))
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								list.RemoveAt(num3);
							}
						}
					}
				}
				return list;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return null;
	}

	public static float PointToLineDistance(Vector3 testPt, Vector3 pt1, Vector3 pt2)
	{
		Vector3 lhs = testPt - pt1;
		Vector3 vector = pt2 - pt1;
		vector.Normalize();
		float d = Vector3.Dot(lhs, vector);
		Vector3 b = d * vector + pt1;
		return (testPt - b).magnitude;
	}

	public static float PointToLineDistance2D(Vector3 testPt, Vector3 pt1, Vector3 pt2)
	{
		testPt.y = 0f;
		pt1.y = 0f;
		pt2.y = 0f;
		Vector3 lhs = testPt - pt1;
		Vector3 vector = pt2 - pt1;
		vector.Normalize();
		float d = Vector3.Dot(lhs, vector);
		Vector3 b = d * vector + pt1;
		return (testPt - b).magnitude;
	}

	public static bool PointInBox(Vector3 testPt, Vector3 pt1, Vector3 pt2, float halfWidth)
	{
		bool result = false;
		float num = Vector3.Dot(testPt - pt1, pt1 - pt2);
		float num2 = Vector3.Dot(testPt - pt2, pt1 - pt2);
		bool flag = num >= 0f;
		bool flag2 = num2 >= 0f;
		if (flag != flag2)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (PointToLineDistance(testPt, pt1, pt2) < halfWidth)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				result = true;
			}
		}
		return result;
	}

	public static void SortSquaresByDistanceToPos(ref List<BoardSquare> squares, Vector3 pos)
	{
		squares.Sort(delegate(BoardSquare x, BoardSquare y)
		{
			if (x == y)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return 0;
					}
				}
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return 1;
					}
				}
			}
			Vector3 a = x.ToVector3();
			a.y = pos.y;
			Vector3 a2 = y.ToVector3();
			a2.y = pos.y;
			float sqrMagnitude = (a - pos).sqrMagnitude;
			float sqrMagnitude2 = (a2 - pos).sqrMagnitude;
			if (sqrMagnitude == sqrMagnitude2)
			{
				if (x.x != y.x)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return x.x.CompareTo(y.x);
						}
					}
				}
				if (x.y != y.y)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							return x.y.CompareTo(y.y);
						}
					}
				}
			}
			return sqrMagnitude.CompareTo(sqrMagnitude2);
		});
	}

	public static bool GetEndPointForValidGameplaySquare(Vector3 start, Vector3 end, out Vector3 adjustedEndPoint)
	{
		adjustedEndPoint = end;
		Vector3 a = start - end;
		a.y = 0f;
		float magnitude = a.magnitude;
		a.Normalize();
		if (magnitude > 0f)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (float num = 0f; num <= magnitude; num += 0.5f * Board.Get().squareSize)
			{
				Vector3 vector = end + num * a;
				BoardSquare boardSquare = Board.Get().GetBoardSquare(vector);
				if (!(boardSquare != null) || !boardSquare.IsBaselineHeight())
				{
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					adjustedEndPoint = vector;
					return true;
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			BoardSquare boardSquare2 = Board.Get().GetBoardSquare(start);
			if (boardSquare2 != null && boardSquare2.IsBaselineHeight())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						adjustedEndPoint = start;
						return true;
					}
				}
			}
		}
		else
		{
			BoardSquare boardSquare3 = Board.Get().GetBoardSquare(end);
			if (boardSquare3 != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (boardSquare3.IsBaselineHeight())
				{
					adjustedEndPoint = end;
					return true;
				}
			}
		}
		return false;
	}

	public static bool SquaresHaveLoSForAbilities(BoardSquare source, BoardSquare dest, ActorData caster, bool checkBarreirs = true, List<NonActorTargetInfo> nonActorTargetInfo = null)
	{
		if (!(source == null))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(dest == null))
			{
				if (source == dest)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
				if (source.IsBaselineHeight())
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!source._0013(dest.x, dest.y))
					{
						return false;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (checkBarreirs)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return HasLosByBarriers(source, dest, caster, VectorUtils.s_laserOffset * Board.SquareSizeStatic, nonActorTargetInfo);
						}
					}
				}
				return true;
			}
		}
		return false;
	}

	public static bool HasLosByBarriers(BoardSquare source, BoardSquare dest, ActorData caster, float offsetToUseInWorld, List<NonActorTargetInfo> nonActorTargetInfo = null)
	{
		bool flag = false;
		int num;
		if (BarrierManager.Get() != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = (BarrierManager.Get().HasAbilityBlockingBarriers() ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag2 = (byte)num != 0;
		if (!(caster == null))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!(BarrierManager.Get() == null))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (flag2)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(offsetToUseInWorld > 0f))
					{
						if (BarrierManager.Get().AreAbilitiesBlocked(caster, source, dest, nonActorTargetInfo))
						{
							flag = false;
							goto IL_0252;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
			}
		}
		flag = true;
		object obj;
		if (nonActorTargetInfo != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			obj = new List<NonActorTargetInfo>();
		}
		else
		{
			obj = null;
		}
		List<NonActorTargetInfo> list = (List<NonActorTargetInfo>)obj;
		if (BarrierManager.Get() != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (flag2)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 vector = source.ToVector3();
				Vector3 a = dest.ToVector3();
				Vector3 vector2 = a - vector;
				vector2.y = 0f;
				float magnitude = vector2.magnitude;
				vector2.Normalize();
				Vector3 b = Vector3.Cross(Vector3.up, vector2);
				b.Normalize();
				b *= offsetToUseInWorld;
				vector.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
				a.y = vector.y;
				bool flag3 = BarrierManager.Get().AreAbilitiesBlocked(caster, vector + b, a + b, list);
				bool flag4 = BarrierManager.Get().AreAbilitiesBlocked(caster, vector - b, a - b, list);
				if (flag3 || flag4)
				{
					int num2;
					RaycastHit hit;
					if (!flag3)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						num2 = (VectorUtils.RaycastInDirection(vector + b, vector2, magnitude, out hit) ? 1 : 0);
					}
					else
					{
						num2 = 1;
					}
					bool flag5 = (byte)num2 != 0;
					int num3;
					if (!flag4)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						num3 = (VectorUtils.RaycastInDirection(vector - b, vector2, magnitude, out hit) ? 1 : 0);
					}
					else
					{
						num3 = 1;
					}
					bool flag6 = (byte)num3 != 0;
					if (flag5)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (flag6)
						{
							flag = false;
						}
					}
				}
				if (!flag)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (list != null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (nonActorTargetInfo != null)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							nonActorTargetInfo.AddRange(list);
						}
					}
				}
			}
		}
		goto IL_0252;
		IL_0252:
		return flag;
	}

	public static int GetCircleCircleIntersections(Vector3 centerA, Vector3 centerB, float radiusAInSquares, float radiusBInSquares, out Vector3 intersectP1, out Vector3 intersectP2)
	{
		intersectP1 = Vector3.zero;
		intersectP2 = Vector3.zero;
		centerA.y = 0f;
		centerB.y = 0f;
		float x = centerA.x;
		float z = centerA.z;
		float x2 = centerB.x;
		float z2 = centerB.z;
		float squareSize = Board.Get().squareSize;
		float num = radiusAInSquares * squareSize;
		float num2 = radiusBInSquares * squareSize;
		Vector3 a = centerB - centerA;
		a.y = 0f;
		float magnitude = a.magnitude;
		if (magnitude > num + num2)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return 0;
				}
			}
		}
		if (magnitude < Mathf.Abs(num - num2))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return 0;
				}
			}
		}
		if (magnitude == 0f)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (num == num2)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return -1;
					}
				}
			}
		}
		int num3;
		if (Mathf.Abs(magnitude - (num + num2)) < 0.001f)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			num3 = 1;
		}
		else
		{
			num3 = 2;
		}
		int result = num3;
		float num4 = (num * num - num2 * num2 + magnitude * magnitude) / (2f * magnitude);
		float num5 = Mathf.Sqrt(num * num - num4 * num4);
		Vector3 vector = centerA + num4 / magnitude * a;
		intersectP1.x = vector.x + num5 * (z2 - z) / magnitude;
		intersectP1.z = vector.z - num5 * (x2 - x) / magnitude;
		intersectP2.x = vector.x - num5 * (z2 - z) / magnitude;
		intersectP2.z = vector.z + num5 * (x2 - x) / magnitude;
		return result;
	}

	public static bool SquareHasLosByAreaCheckers(BoardSquare testSquare, List<ISquareInsideChecker> inAreaCheckers)
	{
		bool result = false;
		if (inAreaCheckers != null)
		{
			int num = 0;
			while (true)
			{
				if (num < inAreaCheckers.Count)
				{
					ISquareInsideChecker squareInsideChecker = inAreaCheckers[num];
					if (squareInsideChecker.IsSquareInside(testSquare, out bool inLos))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (inLos)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							result = true;
							break;
						}
					}
					num++;
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
		}
		return result;
	}

	public static ActorData GetTargetableActorOnSquare(BoardSquare square, bool allowEnemy, bool allowAlly, ActorData caster)
	{
		ActorData result = null;
		if (square != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (square.OccupantActor != null && IsActorTargetable(square.OccupantActor))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				Team team = square.OccupantActor.GetTeam();
				if (caster != null)
				{
					bool flag = team == caster.GetTeam();
					if (flag)
					{
						if (allowAlly)
						{
							goto IL_0097;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (!flag)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (allowEnemy)
						{
							goto IL_0097;
						}
					}
				}
				else
				{
					result = square.OccupantActor;
				}
			}
		}
		goto IL_00a9;
		IL_0097:
		result = square.OccupantActor;
		goto IL_00a9;
		IL_00a9:
		return result;
	}

	public static List<BoardSquare> GetSquaresInRadius(BoardSquare centerSquare, float radiusInSquares, bool ignoreLoS, ActorData caster)
	{
		return GetSquaresInRadius(centerSquare.worldX, centerSquare.worldY, radiusInSquares, ignoreLoS, caster);
	}

	public static List<BoardSquare> GetSquaresInRadius(float centerX, float centerY, float radiusInSquares, bool ignoreLoS, ActorData caster)
	{
		float squareSize = Board.Get().squareSize;
		float num = radiusInSquares * squareSize;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(centerX, centerY);
		List<BoardSquare> list = new List<BoardSquare>();
		if (boardSquareSafe == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return list;
				}
			}
		}
		Vector3 a = new Vector3(centerX, 0f, centerY);
		int maxX = Board.Get().GetMaxX();
		int maxY = Board.Get().GetMaxY();
		int num2 = (int)Mathf.Max(0f, (centerX - num) / squareSize - 1f);
		int num3 = (int)Mathf.Min(maxX, (centerX + num) / squareSize + 1f);
		int num4 = (int)Mathf.Max(0f, (centerY - num) / squareSize - 1f);
		int num5 = (int)Mathf.Min(maxY, (centerY + num) / squareSize + 1f);
		for (int i = num2; i < num3; i++)
		{
			for (int j = num4; j < num5; j++)
			{
				Vector3 b = new Vector3((float)i * squareSize, 0f, (float)j * squareSize);
				float sqrMagnitude = (a - b).sqrMagnitude;
				if (!(sqrMagnitude < num * num))
				{
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				BoardSquare boardSquare = Board.Get().GetBoardSquare(i, j);
				if (!(boardSquare != null))
				{
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (boardSquare.height <= -Board.Get().BaselineHeight)
				{
					continue;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag = true;
				if (!ignoreLoS)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = SquaresHaveLoSForAbilities(boardSquareSafe, boardSquare, caster);
				}
				if (flag)
				{
					list.Add(boardSquare);
				}
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			return list;
		}
	}

	public static bool IsSquareInRadius(BoardSquare testSquare, float centerX, float centerY, float radiusInSquares, bool ignoreLoS, ActorData caster)
	{
		if (testSquare == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		bool result = false;
		float squareSize = Board.Get().squareSize;
		float num = radiusInSquares * squareSize;
		int x = testSquare.x;
		int y = testSquare.y;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(centerX, centerY);
		if (boardSquareSafe == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		float worldX = boardSquareSafe.worldX;
		float worldY = boardSquareSafe.worldY;
		Vector3 a = new Vector3(worldX, 0f, worldY);
		Vector3 b = new Vector3((float)x * squareSize, 0f, (float)y * squareSize);
		float sqrMagnitude = (a - b).sqrMagnitude;
		if (sqrMagnitude < num * num)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			BoardSquare boardSquare = Board.Get().GetBoardSquare(x, y);
			if (boardSquare != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (boardSquare.height >= Board.Get().BaselineHeight)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					int num2;
					if (!ignoreLoS)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						num2 = (SquaresHaveLoSForAbilities(boardSquareSafe, boardSquare, caster) ? 1 : 0);
					}
					else
					{
						num2 = 1;
					}
					bool flag = (byte)num2 != 0;
					result = flag;
				}
			}
		}
		return result;
	}

	public static List<ActorData> GetActorsInRadius(Vector3 centerPos, float radiusInSquares, bool ignoreLoS, ActorData caster, Team onlyValidTeam, List<NonActorTargetInfo> nonActorTargetInfo, bool useLosOverridePos = false, Vector3 losOverridePos = default(Vector3))
	{
		List<Team> list = new List<Team>();
		list.Add(onlyValidTeam);
		return GetActorsInRadius(centerPos, radiusInSquares, ignoreLoS, caster, list, nonActorTargetInfo, useLosOverridePos, losOverridePos);
	}

	public static List<ActorData> GetActorsInRadius(Vector3 centerPos, float radiusInSquares, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams, List<NonActorTargetInfo> nonActorTargetInfo, bool useLosOverridePos = false, Vector3 losOverridePos = default(Vector3))
	{
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return GetActorsInConeByActorRadius(centerPos, 0f, 360f, radiusInSquares, 0f, ignoreLoS, caster, onlyValidTeams, nonActorTargetInfo, useLosOverridePos, losOverridePos);
				}
			}
		}
		List<ActorData> list = new List<ActorData>();
		List<BoardSquare> squaresInRadius = GetSquaresInRadius(centerPos.x, centerPos.z, radiusInSquares, ignoreLoS, caster);
		using (List<BoardSquare>.Enumerator enumerator = squaresInRadius.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				ActorData occupantActor = current.OccupantActor;
				if (IsActorTargetable(occupantActor, onlyValidTeams))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					bool flag = !list.Contains(occupantActor);
					bool flag2 = squaresInRadius.Contains(occupantActor.GetTravelBoardSquare());
					if (flag)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (flag2)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							list.Add(occupantActor);
						}
					}
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
	}

	public static bool IsPosInCone(Vector3 testPos, Vector3 centerPos, float radiusInWorld, float coneCenterAngle, float coneWidthAngleDeg)
	{
		coneWidthAngleDeg = Mathf.Clamp(coneWidthAngleDeg, 0f, 360f);
		bool result = false;
		testPos.y = 0f;
		centerPos.y = 0f;
		float sqrMagnitude = (centerPos - testPos).sqrMagnitude;
		if (sqrMagnitude <= radiusInWorld * radiusInWorld)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			float maxAngleWithCenter = 0.5f * coneWidthAngleDeg;
			Vector3 vec = testPos - centerPos;
			if (vec.sqrMagnitude > 0.001f)
			{
				float testAngle = VectorUtils.HorizontalAngle_Deg(vec);
				result = IsAngleWithinCone(testAngle, coneCenterAngle, maxAngleWithCenter);
			}
			else
			{
				result = true;
			}
		}
		return result;
	}

	public static bool IsPosInAngleOfCone(Vector3 testPos, Vector3 centerPos, float coneCenterAngle, float coneWidthAngleDeg)
	{
		int value = Mathf.RoundToInt(coneWidthAngleDeg);
		value = Mathf.Clamp(value, 0, 360);
		bool result = false;
		testPos.y = 0f;
		centerPos.y = 0f;
		float maxAngleWithCenter = 0.5f * (float)value;
		Vector3 vec = testPos - centerPos;
		if (vec.sqrMagnitude > 0f)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			float testAngle = VectorUtils.HorizontalAngle_Deg(vec);
			result = IsAngleWithinCone(testAngle, coneCenterAngle, maxAngleWithCenter);
		}
		return result;
	}

	public static bool IsAngleWithinCone(float testAngle, float coneCenterAngle, float maxAngleWithCenter)
	{
		float num = coneCenterAngle - maxAngleWithCenter;
		float num2 = coneCenterAngle + maxAngleWithCenter;
		if (testAngle < num)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			testAngle += 360f;
		}
		else if (testAngle > num2)
		{
			testAngle -= 360f;
		}
		int result;
		if (num <= testAngle)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			result = ((testAngle <= num2) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public static List<BoardSquare> GetSquaresInCone(Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, bool ignoreLoS, ActorData caster)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		Vector3 vector = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float d = coneBackwardOffsetInSquares * Board.Get().squareSize;
		Vector3 b = coneStart - vector * d;
		float radiusInSquares = coneLengthRadiusInSquares + coneBackwardOffsetInSquares;
		List<BoardSquare> list2 = null;
		List<BoardSquare> list3 = null;
		if (coneBackwardOffsetInSquares == 0f)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			list2 = GetSquaresInRadius(coneStart.x, coneStart.z, radiusInSquares, ignoreLoS, caster);
			list3 = list2;
		}
		else
		{
			list2 = GetSquaresInRadius(b.x, b.z, radiusInSquares, true, caster);
			list3 = GetSquaresInRadius(coneStart.x, coneStart.z, radiusInSquares, ignoreLoS, caster);
		}
		float num = coneWidthDegrees / 2f;
		foreach (BoardSquare item in list2)
		{
			Vector3 from = item.ToVector3() - b;
			from.y = 0f;
			if (from.sqrMagnitude != 0f)
			{
				bool flag = false;
				float num2 = Vector3.Angle(from, vector);
				flag = (num2 <= num);
				bool flag2 = ignoreLoS;
				if (flag && !ignoreLoS)
				{
					int num3;
					if (coneBackwardOffsetInSquares != 0f)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						num3 = (list3.Contains(item) ? 1 : 0);
					}
					else
					{
						num3 = 1;
					}
					flag2 = ((byte)num3 != 0);
				}
				if (flag)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (flag2)
					{
						list.Add(item);
					}
				}
			}
		}
		return list;
	}

	public static List<BoardSquare> GetSquaresInConeByActorRadius(Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, bool ignoreLoS, ActorData caster)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		Board board = Board.Get();
		Vector3 a = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float d = coneBackwardOffsetInSquares * board.squareSize;
		Vector3 vector = coneStart - a * d;
		float x = vector.x;
		float z = vector.z;
		BoardSquare boardSquareSafe = board.GetBoardSquareSafe(x, z);
		if (boardSquareSafe == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return list;
				}
			}
		}
		GetMaxConeBounds(coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, out int minX, out int maxX, out int minY, out int maxY);
		for (int i = minX; i < maxX; i++)
		{
			for (int j = minY; j < maxY; j++)
			{
				BoardSquare boardSquare = board.GetBoardSquare(i, j);
				if (!(boardSquare != null))
				{
					continue;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (IsSquareInConeByActorRadius(boardSquare, coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, ignoreLoS, caster))
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					list.Add(boardSquare);
				}
			}
		}
		return list;
	}

	public static bool IsSquareInCone(BoardSquare testSquare, Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, bool ignoreLoS, ActorData caster)
	{
		Vector3 a = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float d = coneBackwardOffsetInSquares * Board.Get().squareSize;
		Vector3 b = coneStart - a * d;
		float radiusInSquares = coneLengthRadiusInSquares + coneBackwardOffsetInSquares;
		float maxAngleWithCenter = coneWidthDegrees / 2f;
		bool result = false;
		if (IsSquareInRadius(testSquare, b.x, b.z, radiusInSquares, true, caster))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (IsSquareInRadius(testSquare, coneStart.x, coneStart.z, radiusInSquares, ignoreLoS, caster))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 vec = testSquare.ToVector3() - b;
				vec.y = 0f;
				if (vec.sqrMagnitude > 0f)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					float testAngle = VectorUtils.HorizontalAngle_Deg(vec);
					result = IsAngleWithinCone(testAngle, coneCenterAngleDegrees, maxAngleWithCenter);
				}
			}
		}
		return result;
	}

	public static bool IsSquareInConeByActorRadius(BoardSquare testSquare, Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, bool ignoreLoS, ActorData caster, bool useLosOverridePos = false, Vector3 losOverridePos = default(Vector3))
	{
		float squareSize = Board.Get().squareSize;
		Vector3 vector = testSquare.ToVector3();
		Vector3 a = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float d = coneBackwardOffsetInSquares * squareSize;
		Vector3 vector2 = coneStart - a * d;
		float num = coneLengthRadiusInSquares + coneBackwardOffsetInSquares;
		float num2 = num * squareSize;
		bool flag = false;
		if (IsPosInCone(vector, vector2, num2, coneCenterAngleDegrees, coneWidthDegrees))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			flag = true;
		}
		Vector3 vector3 = vector - vector2;
		vector3.y = 0f;
		float magnitude = vector3.magnitude;
		if (!flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (magnitude > 0f)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				float num3 = GetActorTargetingRadius() * squareSize;
				Vector3 b = default(Vector3);
				b.x = vector3.z;
				b.z = 0f - vector3.x;
				b.y = 0f;
				b.Normalize();
				b *= num3;
				if (magnitude > num2 + num3)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = false;
				}
				else
				{
					float num4 = 0.5f * coneWidthDegrees;
					Vector3 a2 = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees - num4);
					Vector3 a3 = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees + num4);
					int num5;
					if (!VectorUtils.IsSegmentIntersectingCircle(vector2, vector2 + num2 * a2, vector, num3))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						num5 = (VectorUtils.IsSegmentIntersectingCircle(vector2, vector2 + num2 * a3, vector, num3) ? 1 : 0);
					}
					else
					{
						num5 = 1;
					}
					if (num5 != 0)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						flag = true;
					}
					else if (magnitude < Mathf.Abs(num2 - num3))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						int num6;
						if (!IsPosInCone(vector + b, vector2, num2, coneCenterAngleDegrees, coneWidthDegrees))
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							num6 = (IsPosInCone(vector - b, vector2, num2, coneCenterAngleDegrees, coneWidthDegrees) ? 1 : 0);
						}
						else
						{
							num6 = 1;
						}
						flag = ((byte)num6 != 0);
					}
					else if (magnitude > 0f)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						Vector3 intersectP;
						Vector3 intersectP2;
						int circleCircleIntersections = GetCircleCircleIntersections(vector2, vector, num, GetActorTargetingRadius(), out intersectP, out intersectP2);
						if (circleCircleIntersections > 0)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							flag = IsPosInAngleOfCone(intersectP, vector2, coneCenterAngleDegrees, coneWidthDegrees);
						}
						if (!flag)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (circleCircleIntersections > 1)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								flag = IsPosInAngleOfCone(intersectP2, vector2, coneCenterAngleDegrees, coneWidthDegrees);
							}
						}
					}
				}
			}
		}
		bool flag2 = true;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!ignoreLoS)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 vector4;
				if (useLosOverridePos)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					vector4 = losOverridePos;
				}
				else
				{
					vector4 = vector2;
				}
				Vector3 vector5 = vector4;
				BoardSquare boardSquare = Board.Get().GetBoardSquare(vector5);
				if (boardSquare != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					flag2 = SquareHasLosForCone(vector5, boardSquare, testSquare, caster);
				}
				else
				{
					flag2 = false;
				}
			}
		}
		int result;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			result = (flag2 ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public static bool SquareHasLosForCone(Vector3 startPos, BoardSquare centerSquare, BoardSquare testSquare, ActorData caster)
	{
		if (SquaresHaveLoSForAbilities(centerSquare, testSquare, caster))
		{
			Vector3 a = centerSquare.ToVector3();
			Vector3 vector = a - startPos;
			vector.y = 0f;
			if (vector.sqrMagnitude > 0.1f)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						Vector3 a2 = testSquare.ToVector3();
						Vector3 rhs = a2 - startPos;
						rhs.y = 0f;
						rhs.Normalize();
						Vector3 a3 = Vector3.Cross(Vector3.up, rhs);
						a3.Normalize();
						float d = VectorUtils.s_laserOffset * Board.Get().squareSize;
						Vector3 b = a3 * d;
						float d2 = VectorUtils.s_laserOffset * Board.Get().squareSize;
						Vector3 b2 = a3 * d2;
						startPos.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
						a2.y = startPos.y;
						Vector3 vector2 = startPos + b2;
						Vector3 vector3 = startPos - b2;
						Vector3 dir = a2 + b - vector2;
						Vector3 dir2 = a2 - b - vector3;
						int result;
						if (VectorUtils.RaycastInDirection(vector2, dir, dir.magnitude, out RaycastHit hit))
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							result = ((!VectorUtils.RaycastInDirection(vector3, dir2, dir2.magnitude, out hit)) ? 1 : 0);
						}
						else
						{
							result = 1;
						}
						return (byte)result != 0;
					}
					}
				}
			}
			return true;
		}
		return false;
	}

	public static List<ActorData> GetActorsInCone(Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadius, float coneBackwardOffsetInSquares, bool ignoreLoS, ActorData caster, Team onlyValidTeam, List<NonActorTargetInfo> nonActorHitInfo, bool useLosOverridePos = false, Vector3 losOverridePos = default(Vector3))
	{
		List<Team> list = new List<Team>();
		list.Add(onlyValidTeam);
		return GetActorsInCone(coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadius, coneBackwardOffsetInSquares, ignoreLoS, caster, list, nonActorHitInfo, useLosOverridePos, losOverridePos);
	}

	public static List<ActorData> GetActorsInCone(Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams, List<NonActorTargetInfo> nonActorTargetInfo, bool useLosOverridePos = false, Vector3 losOverridePos = default(Vector3))
	{
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return GetActorsInConeByActorRadius(coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, ignoreLoS, caster, onlyValidTeams, nonActorTargetInfo, useLosOverridePos, losOverridePos);
				}
			}
		}
		List<ActorData> list = new List<ActorData>();
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (!IsActorTargetable(actorData, onlyValidTeams))
			{
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
			if (IsSquareInCone(currentBoardSquare, coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, ignoreLoS, caster))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				list.Add(actorData);
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return list;
		}
	}

	public static List<ActorData> GetActorsInConeByActorRadius(Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams = null, List<NonActorTargetInfo> nonActorHitInfo = null, bool useLosOverridePos = false, Vector3 losOverridePos = default(Vector3))
	{
		List<ActorData> list = new List<ActorData>();
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (!IsActorTargetable(actorData, onlyValidTeams))
			{
				continue;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
			if (IsSquareInConeByActorRadius(currentBoardSquare, coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, ignoreLoS, caster, useLosOverridePos, losOverridePos))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				list.Add(actorData);
			}
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			return list;
		}
	}

	public static void GatherStretchConeDimensions(Vector3 aimPos, Vector3 coneStartPos, float minLengthInSquares, float maxLengthInSquares, float minWidthDegrees, float maxWidthDegrees, StretchConeStyle style, out float lengthInSquares, out float angleInDegrees, bool widthChangeDiscrete = false, int numWidthDiscreteChanges = 0, float interpMinDistOverrideInSquares = -1f, float interpRangeOverrideInSquares = -1f)
	{
		float num = maxLengthInSquares - minLengthInSquares;
		float num2 = maxWidthDegrees - minWidthDegrees;
		float num3 = maxLengthInSquares;
		float num4 = minLengthInSquares;
		if (interpMinDistOverrideInSquares > 0f)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (interpRangeOverrideInSquares > 0f)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				num4 = interpMinDistOverrideInSquares;
				num3 = interpMinDistOverrideInSquares + interpRangeOverrideInSquares;
				num = interpRangeOverrideInSquares;
				goto IL_0075;
			}
		}
		if (num <= 0f)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			num3 = Mathf.Max(3f, maxLengthInSquares);
			num4 = 2.5f;
			num = num3 - num4;
		}
		goto IL_0075;
		IL_0075:
		if (num <= 0f)
		{
			lengthInSquares = maxLengthInSquares;
			angleInDegrees = maxWidthDegrees;
			return;
		}
		Vector3 vector = aimPos - coneStartPos;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		float value = magnitude / Board.Get().squareSize;
		lengthInSquares = Mathf.Clamp(value, minLengthInSquares, maxLengthInSquares);
		float num5 = Mathf.Clamp(value, num4, num3);
		float num6 = (num5 - num4) / num;
		float num7;
		if (style == StretchConeStyle.Linear)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			num7 = num6;
		}
		else if (style == StretchConeStyle.DistanceSquared)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			num7 = num6 * num6;
		}
		else if (style == StretchConeStyle.DistanceCubed)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			num7 = num6 * num6 * num6;
		}
		else if (style == StretchConeStyle.DistanceToTheFourthPower)
		{
			num7 = num6 * num6 * num6 * num6;
		}
		else if (style == StretchConeStyle.DistanceSquareRoot)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			num7 = Mathf.Sqrt(num6);
		}
		else
		{
			num7 = 0f;
		}
		float num8 = 1f - num7;
		float num9 = num2 * num8;
		if (widthChangeDiscrete)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (numWidthDiscreteChanges > 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (num8 < 0.02f)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					num8 = 0f;
				}
				float num10 = 1f / (float)numWidthDiscreteChanges;
				int num11 = Mathf.CeilToInt(num8 / num10);
				float value2 = (float)num11 * (num2 / (float)numWidthDiscreteChanges);
				num9 = Mathf.Clamp(value2, 0f, num2);
			}
		}
		angleInDegrees = minWidthDegrees + num9;
	}

	private static void AdjustMinMaxBounds(Vector3 point, ref Vector3 minBounds, ref Vector3 maxBounds)
	{
		minBounds.x = Mathf.Min(point.x, minBounds.x);
		minBounds.z = Mathf.Min(point.z, minBounds.z);
		maxBounds.x = Mathf.Max(point.x, maxBounds.x);
		maxBounds.z = Mathf.Max(point.z, maxBounds.z);
	}

	public static bool GetMaxConeBounds(Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, out int minX, out int maxX, out int minY, out int maxY)
	{
		Board board = Board.Get();
		float num = coneLengthRadiusInSquares + coneBackwardOffsetInSquares;
		Vector3 a = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float d = coneBackwardOffsetInSquares * board.squareSize;
		Vector3 vector = coneStart - a * d;
		float x = vector.x;
		float z = vector.z;
		float squareSize = board.squareSize;
		float num2 = num * squareSize;
		BoardSquare boardSquareSafe = board.GetBoardSquareSafe(x, z);
		if (boardSquareSafe == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					minX = 0;
					maxX = 0;
					minY = 0;
					maxY = 0;
					return false;
				}
			}
		}
		int maxX2 = board.GetMaxX();
		int maxY2 = board.GetMaxY();
		float num3 = 0.5f * coneWidthDegrees;
		float num4 = coneCenterAngleDegrees - num3;
		float num5 = coneCenterAngleDegrees + num3;
		Vector3 minBounds = vector;
		Vector3 maxBounds = vector;
		float num6 = GetActorTargetingRadius() * board.squareSize;
		float d2 = num2 + num6;
		Vector3 point = vector + d2 * VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees + num3);
		Vector3 point2 = vector + d2 * VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees - num3);
		AdjustMinMaxBounds(point, ref minBounds, ref maxBounds);
		AdjustMinMaxBounds(point2, ref minBounds, ref maxBounds);
		for (int i = -90; i <= 450; i += 90)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!(num5 > (float)i))
			{
				break;
			}
			if (!(num4 < (float)i))
			{
				continue;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if ((float)i < num5)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 point3 = vector + d2 * VectorUtils.AngleDegreesToVector(i);
				AdjustMinMaxBounds(point3, ref minBounds, ref maxBounds);
			}
		}
		minX = (int)Mathf.Max(0f, minBounds.x / squareSize - 2f);
		maxX = (int)Mathf.Min(maxX2, maxBounds.x / squareSize + 2f);
		minY = (int)Mathf.Max(0f, minBounds.z / squareSize - 2f);
		maxY = (int)Mathf.Min(maxY2, maxBounds.z / squareSize + 2f);
		return true;
	}

	public static void OperateOnSquaresInCone(IOperationOnSquare operationObj, Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, ActorData caster, bool ignoreLos, List<ISquareInsideChecker> losCheckOverrides = null)
	{
		Board board = Board.Get();
		Vector3 a = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float d = coneBackwardOffsetInSquares * board.squareSize;
		Vector3 startPos = coneStart - a * d;
		float x = startPos.x;
		float z = startPos.z;
		BoardSquare boardSquareSafe = board.GetBoardSquareSafe(x, z);
		if (boardSquareSafe == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		GetMaxConeBounds(coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, out int minX, out int maxX, out int minY, out int maxY);
		bool flag = false;
		int num = minX;
		while (num < maxX)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (flag)
				{
					return;
				}
				for (int i = minY; i < maxY; i++)
				{
					if (flag)
					{
						break;
					}
					BoardSquare boardSquare = board.GetBoardSquare(num, i);
					if (!(boardSquare != null) || !boardSquare.IsBaselineHeight() || !IsSquareInConeByActorRadius(boardSquare, coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, true, caster))
					{
						continue;
					}
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					bool flag2 = false;
					if (ignoreLos)
					{
						flag2 = true;
					}
					else if (losCheckOverrides != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						flag2 = SquareHasLosByAreaCheckers(boardSquare, losCheckOverrides);
					}
					else
					{
						flag2 = SquareHasLosForCone(startPos, boardSquareSafe, boardSquare, caster);
					}
					operationObj.OperateOnSquare(boardSquare, caster, flag2);
					flag = operationObj.ShouldEarlyOut();
				}
				num++;
				goto IL_0156;
			}
			IL_0156:;
		}
	}

	public static List<ActorData> GetActorsInRadiusOfLine(Vector3 startPos, Vector3 endPos, float startRadiusInSquares, float endRadiusInSquares, float rangeFromLineInSquares, bool ignoreLoS, ActorData caster, Team onlyValidTeam, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> list = new List<Team>();
		list.Add(onlyValidTeam);
		return GetActorsInRadiusOfLine(startPos, endPos, startRadiusInSquares, endRadiusInSquares, rangeFromLineInSquares, ignoreLoS, caster, list, nonActorTargetInfo);
	}

	public static List<ActorData> GetActorsInRadiusOfLine(Vector3 startPos, Vector3 endPos, float startRadiusInSquares, float endRadiusInSquares, float rangeFromLineInSquares, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list = new List<ActorData>();
		if (rangeFromLineInSquares > 0f)
		{
			float num = rangeFromLineInSquares * 2f;
			List<Vector3> list2 = new List<Vector3>();
			if (!ignoreLoS)
			{
				list2.Add(startPos);
				list2.Add((startPos + endPos) * 0.5f);
				list2.Add(endPos);
			}
			List<ActorData> list3;
			if (GameWideData.Get().UseActorRadiusForLaser())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				list3 = GetActorsInBoxByActorRadius(startPos, endPos, num, ignoreLoS, caster, onlyValidTeams, list2);
			}
			else
			{
				list3 = GetActorsInBox(startPos, endPos, num, list2, caster, onlyValidTeams);
			}
			List<ActorData> list4 = list3;
			using (List<ActorData>.Enumerator enumerator = list4.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (!list.Contains(current))
					{
						list.Add(current);
					}
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (startRadiusInSquares > 0f)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			List<ActorData> actorsInRadius = GetActorsInRadius(startPos, startRadiusInSquares, ignoreLoS, caster, onlyValidTeams, nonActorTargetInfo);
			foreach (ActorData item in actorsInRadius)
			{
				if (!list.Contains(item))
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					list.Add(item);
				}
			}
		}
		if (endRadiusInSquares > 0f)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					List<ActorData> actorsInRadius2 = GetActorsInRadius(endPos, endRadiusInSquares, ignoreLoS, caster, onlyValidTeams, nonActorTargetInfo);
					using (List<ActorData>.Enumerator enumerator3 = actorsInRadius2.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							ActorData current3 = enumerator3.Current;
							if (!list.Contains(current3))
							{
								list.Add(current3);
							}
						}
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return list;
							}
						}
					}
				}
				}
			}
		}
		return list;
	}

	public static void OperateOnSquaresInRadiusOfLine(IOperationOnSquare operationObj, Vector3 startPos, Vector3 endPos, float startRadiusInSquares, float endRadiusInSquares, float rangeFromLineInSquares, bool ignoreLoS, ActorData caster)
	{
		if (s_radiusOfLineLosCheckers.Count == 0)
		{
			s_radiusOfLineLosCheckers.Add(new SquareInsideChecker_Box(1f));
			s_radiusOfLineLosCheckers.Add(new SquareInsideChecker_Cone());
			s_radiusOfLineLosCheckers.Add(new SquareInsideChecker_Cone());
		}
		List<Vector3> list = new List<Vector3>();
		if (!ignoreLoS)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			list.Add(startPos);
			list.Add((startPos + endPos) * 0.5f);
			list.Add(endPos);
		}
		SquareInsideChecker_Box squareInsideChecker_Box = s_radiusOfLineLosCheckers[0] as SquareInsideChecker_Box;
		SquareInsideChecker_Cone squareInsideChecker_Cone = s_radiusOfLineLosCheckers[1] as SquareInsideChecker_Cone;
		SquareInsideChecker_Cone squareInsideChecker_Cone2 = s_radiusOfLineLosCheckers[2] as SquareInsideChecker_Cone;
		float widthInSquares = rangeFromLineInSquares * 2f;
		squareInsideChecker_Box.UpdateBoxProperties(startPos, endPos, caster);
		squareInsideChecker_Box.m_penetrateLos = ignoreLoS;
		squareInsideChecker_Box.m_widthInSquares = widthInSquares;
		squareInsideChecker_Box.m_additionalLosSources = list;
		squareInsideChecker_Cone.UpdateConeProperties(startPos, 360f, startRadiusInSquares, 0f, 0f, caster);
		squareInsideChecker_Cone2.UpdateConeProperties(endPos, 360f, endRadiusInSquares, 0f, 0f, caster);
		if (rangeFromLineInSquares > 0f)
		{
			OperateOnSquaresInBoxByActorRadius(operationObj, startPos, endPos, widthInSquares, caster, ignoreLoS, list, s_radiusOfLineLosCheckers);
		}
		if (startRadiusInSquares > 0f)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			OperateOnSquaresInCone(operationObj, startPos, 0f, 360f, startRadiusInSquares, 0f, caster, ignoreLoS, s_radiusOfLineLosCheckers);
		}
		if (endRadiusInSquares > 0f)
		{
			OperateOnSquaresInCone(operationObj, endPos, 0f, 360f, endRadiusInSquares, 0f, caster, ignoreLoS, s_radiusOfLineLosCheckers);
		}
	}

	public static List<BoardSquare> GetSquaresInBox(Vector3 pt1, Vector3 pt2, float halfWidthInSquares, bool ignoreLoS, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		if (!ignoreLoS)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			list.Add(pt1);
		}
		return GetSquaresInBox(pt1, pt2, halfWidthInSquares, list, caster);
	}

	public static List<BoardSquare> GetSquaresInBox(Vector3 pt1, Vector3 pt2, float halfWidthInSquares, List<Vector3> losCheckPoints, ActorData caster)
	{
		float squareSize = Board.Get().squareSize;
		float num = halfWidthInSquares * squareSize;
		pt1.y = 0f;
		pt2.y = 0f;
		int maxX = Board.Get().GetMaxX();
		int maxY = Board.Get().GetMaxY();
		int num2 = Mathf.Max(0, (int)(Mathf.Min(pt1.x - num, pt2.x - num) / squareSize));
		int num3 = Mathf.Max(0, (int)(Mathf.Min(pt1.z - num, pt2.z - num) / squareSize));
		int num4 = Mathf.Min(maxX, (int)(Mathf.Max(pt1.x + num, pt2.x + num) / squareSize) + 1);
		int num5 = Mathf.Min(maxY, (int)(Mathf.Max(pt1.z + num, pt2.z + num) / squareSize) + 1);
		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = num2; i < num4; i++)
		{
			for (int j = num3; j < num5; j++)
			{
				BoardSquare boardSquare = Board.Get().GetBoardSquare(i, j);
				Vector3 testPt = new Vector3(boardSquare.worldX, 0f, boardSquare.worldY);
				if (!PointInBox(testPt, pt1, pt2, num))
				{
					continue;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				bool flag;
				if (losCheckPoints != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (losCheckPoints.Count > 0)
					{
						flag = false;
						foreach (Vector3 losCheckPoint in losCheckPoints)
						{
							BoardSquare boardSquare2 = Board.Get().GetBoardSquare(losCheckPoint);
							if (boardSquare2 != null)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (SquaresHaveLoSForAbilities(boardSquare2, boardSquare, caster))
								{
									flag = true;
									break;
								}
							}
						}
						goto IL_01e6;
					}
				}
				flag = true;
				goto IL_01e6;
				IL_01e6:
				if (flag)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					list.Add(boardSquare);
				}
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			return list;
		}
	}

	public static List<ActorData> GetActorsInBox(Vector3 startPos, Vector3 endPos, float boxWidthInSquares, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams)
	{
		List<Vector3> list = new List<Vector3>();
		if (!ignoreLoS)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			list.Add(startPos);
		}
		return GetActorsInBox(startPos, endPos, boxWidthInSquares, list, caster, onlyValidTeams);
	}

	public static List<ActorData> GetActorsInBox(Vector3 startPos, Vector3 endPos, float boxWidthInSquares, List<Vector3> losCheckPoints, ActorData caster, List<Team> onlyValidTeams)
	{
		List<ActorData> list = new List<ActorData>();
		if (boxWidthInSquares > 0f)
		{
			List<BoardSquare> squaresInBox = GetSquaresInBox(startPos, endPos, boxWidthInSquares / 2f, losCheckPoints, caster);
			using (List<BoardSquare>.Enumerator enumerator = squaresInBox.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoardSquare current = enumerator.Current;
					ActorData occupantActor = current.OccupantActor;
					if (IsActorTargetable(occupantActor, onlyValidTeams))
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (!list.Contains(occupantActor))
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							list.Add(occupantActor);
						}
					}
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return list;
					}
				}
			}
		}
		return list;
	}

	internal static void GetBoxBoundsInGridPos(Vector3 startPos, Vector3 endPos, float adjustAmount, out int minX, out int minY, out int maxX, out int maxY)
	{
		float squareSize = Board.Get().squareSize;
		int maxX2 = Board.Get().GetMaxX();
		int maxY2 = Board.Get().GetMaxY();
		minX = Mathf.Max(0, (int)(Mathf.Min(startPos.x - adjustAmount, endPos.x - adjustAmount) / squareSize) - 1);
		minY = Mathf.Max(0, (int)(Mathf.Min(startPos.z - adjustAmount, endPos.z - adjustAmount) / squareSize) - 1);
		maxX = Mathf.Min(maxX2, (int)(Mathf.Max(startPos.x + adjustAmount, endPos.x + adjustAmount) / squareSize) + 1);
		maxY = Mathf.Min(maxY2, (int)(Mathf.Max(startPos.z + adjustAmount, endPos.z + adjustAmount) / squareSize) + 1);
	}

	private static void GetBoxCorners(Vector3 startPos, Vector3 endPos, float widthInSquares, out Vector3 ptA, out Vector3 ptB, out Vector3 ptC, out Vector3 ptD)
	{
		Vector3 normalized = (endPos - startPos).normalized;
		Vector3 normalized2 = Vector3.Cross(normalized, Vector3.up).normalized;
		float num = widthInSquares * Board.Get().squareSize;
		Vector3 b = 0.5f * num * normalized2;
		ptA = endPos - b;
		ptB = endPos + b;
		ptC = startPos - b;
		ptD = startPos + b;
	}

	private static bool IsBoxBorderTouchingCircle(Vector3 ptA, Vector3 ptB, Vector3 ptC, Vector3 ptD, Vector3 circleCenter, float radiusInWorld)
	{
		circleCenter.y = 0f;
		int result;
		if (!VectorUtils.IsSegmentIntersectingCircle(ptA, ptB, circleCenter, radiusInWorld))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!VectorUtils.IsSegmentIntersectingCircle(ptB, ptD, circleCenter, radiusInWorld) && !VectorUtils.IsSegmentIntersectingCircle(ptD, ptC, circleCenter, radiusInWorld))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				result = (VectorUtils.IsSegmentIntersectingCircle(ptC, ptA, circleCenter, radiusInWorld) ? 1 : 0);
				goto IL_0064;
			}
		}
		result = 1;
		goto IL_0064;
		IL_0064:
		return (byte)result != 0;
	}

	public static List<BoardSquare> GetSquaresInBoxByActorRadius(Vector3 startPos, Vector3 endPos, float laserWidthInSquares, bool penetrateLos, ActorData caster, List<Vector3> additionalLosSources = null)
	{
		startPos.y = 0f;
		endPos.y = 0f;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(startPos);
		float num = GetActorTargetingRadius() * Board.Get().squareSize;
		float num2 = laserWidthInSquares * Board.Get().squareSize;
		float num3 = 0.5f * num2;
		GetBoxCorners(startPos, endPos, laserWidthInSquares, out Vector3 ptA, out Vector3 ptB, out Vector3 ptC, out Vector3 ptD);
		float adjustAmount = num3 + num;
		GetBoxBoundsInGridPos(startPos, endPos, adjustAmount, out int minX, out int minY, out int maxX, out int maxY);
		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = minX; i <= maxX; i++)
		{
			for (int j = minY; j <= maxY; j++)
			{
				BoardSquare boardSquare2 = Board.Get().GetBoardSquare(i, j);
				if (boardSquare2 == null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					continue;
				}
				Vector3 vector = boardSquare2.ToVector3();
				vector.y = 0f;
				int num4;
				if (!PointInBox(vector, startPos, endPos, num3))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					num4 = (IsBoxBorderTouchingCircle(ptA, ptB, ptC, ptD, vector, num) ? 1 : 0);
				}
				else
				{
					num4 = 1;
				}
				if (num4 == 0)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				else if (penetrateLos)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					list.Add(boardSquare2);
				}
				else if (HasLosForLaserByActorRadius(boardSquare, boardSquare2, startPos, endPos, laserWidthInSquares, caster))
				{
					list.Add(boardSquare2);
				}
				else if (additionalLosSources != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					using (List<Vector3>.Enumerator enumerator = additionalLosSources.GetEnumerator())
					{
						while (true)
						{
							if (!enumerator.MoveNext())
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								break;
							}
							Vector3 current = enumerator.Current;
							BoardSquare boardSquare3 = Board.Get().GetBoardSquare(current);
							if (boardSquare3 != null)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (SquaresHaveLoSForAbilities(boardSquare3, boardSquare2, caster))
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											break;
										default:
											list.Add(boardSquare2);
											goto end_IL_0175;
										}
									}
								}
							}
						}
						end_IL_0175:;
					}
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					goto end_IL_0204;
				}
				continue;
				end_IL_0204:
				break;
			}
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			return list;
		}
	}

	public static bool IsSquareInBoxByActorRadius(BoardSquare testSquare, Vector3 startPos, Vector3 endPos, float laserWidthInSquares)
	{
		if (testSquare != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					startPos.y = 0f;
					endPos.y = 0f;
					Vector3 vector = testSquare.ToVector3();
					vector.y = 0f;
					GetBoxCorners(startPos, endPos, laserWidthInSquares, out Vector3 ptA, out Vector3 ptB, out Vector3 ptC, out Vector3 ptD);
					float radiusInWorld = GetActorTargetingRadius() * Board.Get().squareSize;
					float num = laserWidthInSquares * Board.Get().squareSize;
					float halfWidth = 0.5f * num;
					return PointInBox(vector, startPos, endPos, halfWidth) || IsBoxBorderTouchingCircle(ptA, ptB, ptC, ptD, vector, radiusInWorld);
				}
				}
			}
		}
		return false;
	}

	public static bool IsSquareInLosForBox(BoardSquare square, Vector3 startPos, Vector3 endPos, float laserWidthInSquares, bool penetrateLos, ActorData caster, List<Vector3> additionalLosSources = null)
	{
		BoardSquare boardSquare = Board.Get().GetBoardSquare(startPos);
		bool result = false;
		if (penetrateLos)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = true;
		}
		else if (HasLosForLaserByActorRadius(boardSquare, square, startPos, endPos, laserWidthInSquares, caster))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			result = true;
		}
		else if (additionalLosSources != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					using (List<Vector3>.Enumerator enumerator = additionalLosSources.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Vector3 current = enumerator.Current;
							BoardSquare boardSquare2 = Board.Get().GetBoardSquare(current);
							if (boardSquare2 != null)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (SquaresHaveLoSForAbilities(boardSquare2, square, caster))
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
											return true;
										}
									}
								}
							}
						}
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								return result;
							}
						}
					}
				}
				}
			}
		}
		return result;
	}

	public static List<ActorData> GetActorsInBoxByActorRadius(Vector3 startPos, Vector3 endPos, float laserWidthInSquares, bool penetrateLos, ActorData caster, List<Team> validTeams, List<Vector3> additionalLosSources = null, List<NonActorTargetInfo> nonActorTargetInfo = null)
	{
		List<ActorData> list = new List<ActorData>();
		if (laserWidthInSquares > 0f)
		{
			startPos.y = 0f;
			endPos.y = 0f;
			BoardSquare boardSquare = Board.Get().GetBoardSquare(startPos);
			float radiusInWorld = GetActorTargetingRadius() * Board.Get().squareSize;
			float num = laserWidthInSquares * Board.Get().squareSize;
			float halfWidth = 0.5f * num;
			GetBoxCorners(startPos, endPos, laserWidthInSquares, out Vector3 ptA, out Vector3 ptB, out Vector3 ptC, out Vector3 ptD);
			List<ActorData> actors = GameFlowData.Get().GetActors();
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (IsActorTargetable(current, validTeams))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						Vector3 travelBoardSquareWorldPosition = current.GetTravelBoardSquareWorldPosition();
						travelBoardSquareWorldPosition.y = 0f;
						int num2;
						if (!PointInBox(travelBoardSquareWorldPosition, startPos, endPos, halfWidth))
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							num2 = (IsBoxBorderTouchingCircle(ptA, ptB, ptC, ptD, travelBoardSquareWorldPosition, radiusInWorld) ? 1 : 0);
						}
						else
						{
							num2 = 1;
						}
						if (num2 == 0)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						else if (penetrateLos)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							list.Add(current);
						}
						else
						{
							BoardSquare currentBoardSquare = current.GetCurrentBoardSquare();
							if (HasLosForLaserByActorRadius(boardSquare, currentBoardSquare, startPos, endPos, laserWidthInSquares, caster, nonActorTargetInfo))
							{
								list.Add(current);
							}
							else if (additionalLosSources != null)
							{
								using (List<Vector3>.Enumerator enumerator2 = additionalLosSources.GetEnumerator())
								{
									while (true)
									{
										if (!enumerator2.MoveNext())
										{
											while (true)
											{
												switch (2)
												{
												case 0:
													continue;
												}
												break;
											}
											break;
										}
										Vector3 current2 = enumerator2.Current;
										BoardSquare boardSquare2 = Board.Get().GetBoardSquare(current2);
										if (boardSquare2 != null)
										{
											while (true)
											{
												switch (1)
												{
												case 0:
													continue;
												}
												break;
											}
											if (SquaresHaveLoSForAbilities(boardSquare2, currentBoardSquare, caster))
											{
												while (true)
												{
													switch (6)
													{
													case 0:
														break;
													default:
														list.Add(current);
														goto end_IL_0175;
													}
												}
											}
										}
									}
									end_IL_0175:;
								}
							}
						}
					}
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return list;
					}
				}
			}
		}
		return list;
	}

	public static bool HasLosForLaserByActorRadius(BoardSquare startSquare, BoardSquare testSquare, Vector3 laserStart, Vector3 laserEnd, float widthInSquares, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo = null)
	{
		Vector3 vector = laserEnd - laserStart;
		vector.y = 0f;
		Vector3 normalized = vector.normalized;
		Vector3 vector2 = testSquare.ToVector3();
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector3 = startSquare.ToVector3();
		vector3.y = travelBoardSquareWorldPositionForLos.y;
		Vector3 vector4 = vector2 - vector3;
		vector4.y = 0f;
		float squareSize = Board.Get().squareSize;
		bool flag = SquaresHaveLoSForAbilities(startSquare, testSquare, caster, false);
		object obj;
		if (nonActorTargetInfo != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			obj = new List<NonActorTargetInfo>();
		}
		else
		{
			obj = null;
		}
		List<NonActorTargetInfo> list = (List<NonActorTargetInfo>)obj;
		bool flag2 = HasLosByBarriers(startSquare, testSquare, caster, VectorUtils.s_laserOffset * Board.SquareSizeStatic, list);
		float num = 0.72f;
		if (caster.GetCurrentBoardSquare() == startSquare)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			num += GameWideData.Get().m_actorTargetingRadiusInSquares;
		}
		Vector3 a = vector2 - num * squareSize * normalized;
		Vector3 lhs = a - laserStart;
		lhs.y = 0f;
		Vector3 vector5 = laserStart + Vector3.Dot(lhs, normalized) * normalized;
		if (Vector3.Dot(normalized, vector5 - laserStart) <= 0f)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			vector5 = vector3;
		}
		bool flag3 = true;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(vector5);
		if (boardSquare != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (boardSquare != testSquare)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (boardSquare.IsBaselineHeight())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					flag3 = boardSquare._0013(testSquare.x, testSquare.y);
					if (flag2)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						flag2 = HasLosByBarriers(boardSquare, testSquare, caster, VectorUtils.s_laserOffset * Board.SquareSizeStatic, list);
						if (!flag2)
						{
							flag3 = false;
						}
					}
				}
			}
		}
		float num2 = 0.35f * Board.Get().squareSize;
		Vector3 zero = Vector3.zero;
		if (!(Mathf.Abs(normalized.x) > Mathf.Abs(normalized.z)))
		{
			zero = new Vector3(num2, 0f, 0f);
		}
		else
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			zero = new Vector3(0f, 0f, num2);
		}
		bool flag4 = !flag2;
		if (flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (flag4)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				nonActorTargetInfo?.AddRange(list);
			}
		}
		if (!flag4 && flag3)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 a2 = vector2;
				a2.y = travelBoardSquareWorldPositionForLos.y;
				Vector3 dir = a2 - vector3;
				dir.y = 0f;
				RaycastHit hit;
				bool flag5 = VectorUtils.RaycastInDirection(vector3 + zero, dir, dir.magnitude, out hit);
				int num3;
				if (flag5)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					num3 = (VectorUtils.RaycastInDirection(vector3 - zero, dir, dir.magnitude, out hit) ? 1 : 0);
				}
				else
				{
					num3 = 0;
				}
				bool flag6 = (byte)num3 != 0;
				flag = (!flag5 || !flag6);
			}
		}
		int result;
		if (!flag4)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (flag)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				result = (flag3 ? 1 : 0);
				goto IL_036c;
			}
		}
		result = 0;
		goto IL_036c;
		IL_036c:
		return (byte)result != 0;
	}

	public static void OperateOnSquaresInBoxByActorRadius(IOperationOnSquare operationObj, Vector3 startPos, Vector3 endPos, float widthInSquares, ActorData caster, bool ignoreLos, List<Vector3> additionalLosSources = null, List<ISquareInsideChecker> losCheckOverrides = null, bool applyLaserStartOffset = true)
	{
		startPos.y = 0f;
		endPos.y = 0f;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(startPos);
		if (applyLaserStartOffset)
		{
			float actorTargetingRadiusInSquares = GameWideData.Get().m_actorTargetingRadiusInSquares;
			if (actorTargetingRadiusInSquares > 0f)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				startPos = VectorUtils.GetAdjustedStartPosWithOffset(startPos, endPos, actorTargetingRadiusInSquares);
			}
		}
		float num = GetActorTargetingRadius() * Board.Get().squareSize;
		float num2 = widthInSquares * Board.Get().squareSize;
		float num3 = 0.5f * num2;
		GetBoxCorners(startPos, endPos, widthInSquares, out Vector3 ptA, out Vector3 ptB, out Vector3 ptC, out Vector3 ptD);
		float adjustAmount = num3 + num;
		GetBoxBoundsInGridPos(startPos, endPos, adjustAmount, out int minX, out int minY, out int maxX, out int maxY);
		for (int i = minX; i <= maxX; i++)
		{
			for (int j = minY; j <= maxY; j++)
			{
				BoardSquare boardSquare2 = Board.Get().GetBoardSquare(i, j);
				if (boardSquare2 == null)
				{
					continue;
				}
				Vector3 vector = boardSquare2.ToVector3();
				vector.y = 0f;
				int num4;
				if (!PointInBox(vector, startPos, endPos, num3))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					num4 = (IsBoxBorderTouchingCircle(ptA, ptB, ptC, ptD, vector, num) ? 1 : 0);
				}
				else
				{
					num4 = 1;
				}
				if (num4 == 0)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					continue;
				}
				bool flag = false;
				if (ignoreLos)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
				}
				else if (losCheckOverrides != null)
				{
					flag = SquareHasLosByAreaCheckers(boardSquare2, losCheckOverrides);
				}
				else
				{
					flag = HasLosForLaserByActorRadius(boardSquare, boardSquare2, startPos, endPos, widthInSquares, caster);
					if (!flag && additionalLosSources != null)
					{
						using (List<Vector3>.Enumerator enumerator = additionalLosSources.GetEnumerator())
						{
							while (true)
							{
								if (!enumerator.MoveNext())
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									break;
								}
								Vector3 current = enumerator.Current;
								BoardSquare boardSquare3 = Board.Get().GetBoardSquare(current);
								if (boardSquare3 != null)
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									if (SquaresHaveLoSForAbilities(boardSquare3, boardSquare2, caster))
									{
										while (true)
										{
											switch (7)
											{
											case 0:
												break;
											default:
												flag = true;
												goto end_IL_019d;
											}
										}
									}
								}
							}
							end_IL_019d:;
						}
					}
				}
				operationObj.OperateOnSquare(boardSquare2, caster, flag);
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					goto end_IL_0232;
				}
				continue;
				end_IL_0232:
				break;
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public static List<ActorData> GetActorsInLaser(Vector3 startPos, Vector3 dir, float laserRangeInSquares, float laserWidthInSquares, ActorData caster, List<Team> validTeams, bool penetrateLos, int maxTargets, bool lengthIgnoreLevelGeo, bool includeInvisibles, out Vector3 laserEndPos, List<NonActorTargetInfo> nonActorTargets, List<ActorData> actorsToExclude = null, bool ignoreStartOffset = false, bool excludeCaster = true)
	{
		dir.y = 0f;
		dir.Normalize();
		float maxDistanceInWorld = laserRangeInSquares * Board.Get().squareSize;
		int num;
		if (!penetrateLos)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = (lengthIgnoreLevelGeo ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		object obj;
		if (nonActorTargets != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			obj = new List<NonActorTargetInfo>();
		}
		else
		{
			obj = null;
		}
		List<NonActorTargetInfo> list = (List<NonActorTargetInfo>)obj;
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = startPos;
		bool checkBarriers = laserWidthInSquares > 0f;
		laserCoords.end = VectorUtils.GetLaserEndPoint(startPos, dir, maxDistanceInWorld, penetrateLos, caster, list, checkBarriers);
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!penetrateLos)
			{
				laserCoords.end = VectorUtils.GetLaserEndPoint(startPos, dir, maxDistanceInWorld, flag, caster, null, checkBarriers);
			}
		}
		if (!penetrateLos)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (laserWidthInSquares > 2f)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (list != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (list.Count == 0)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						Vector3 b = Vector3.Cross(Vector3.up, dir);
						b.Normalize();
						b *= 0.5f * laserWidthInSquares * Board.SquareSizeStatic;
						Vector3 startPos2 = startPos + b;
						Vector3 startPos3 = startPos - b;
						float magnitude = (laserCoords.end - laserCoords.start).magnitude;
						VectorUtils.GetLaserEndPoint(startPos2, dir, magnitude, penetrateLos, caster, list, checkBarriers);
						if (list.Count == 0)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							VectorUtils.GetLaserEndPoint(startPos3, dir, magnitude, penetrateLos, caster, list, checkBarriers);
						}
					}
				}
			}
		}
		float actorTargetingRadiusInSquares = GameWideData.Get().m_actorTargetingRadiusInSquares;
		bool flag2 = GameWideData.Get().UseActorRadiusForLaser();
		if (actorTargetingRadiusInSquares > 0f)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!ignoreStartOffset)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				laserCoords.start = VectorUtils.GetAdjustedStartPosWithOffset(laserCoords.start, laserCoords.end, actorTargetingRadiusInSquares);
			}
		}
		List<ActorData> list2;
		if (flag2)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			list2 = GetActorsInBoxByActorRadius(laserCoords.start, laserCoords.end, laserWidthInSquares, penetrateLos, caster, validTeams, null, nonActorTargets);
		}
		else
		{
			list2 = GetActorsInBox(laserCoords.start, laserCoords.end, laserWidthInSquares, penetrateLos, caster, validTeams);
		}
		List<ActorData> actors = list2;
		if (!includeInvisibles)
		{
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		}
		if (actorsToExclude != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int i = 0; i < actorsToExclude.Count; i++)
			{
				if (actors.Contains(actorsToExclude[i]))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					actors.Remove(actorsToExclude[i]);
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (excludeCaster)
		{
			actors.Remove(caster);
		}
		TargeterUtils.SortActorsByDistanceToPos(ref actors, startPos, dir);
		TargeterUtils.LimitActorsToMaxNumber(ref actors, maxTargets);
		if (maxTargets > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (actors.Count == maxTargets)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				ActorData actorData = actors[actors.Count - 1];
				Vector3 lhs = actorData.GetTravelBoardSquareWorldPosition() - laserCoords.start;
				lhs.y = 0f;
				Vector3 b2 = Vector3.Dot(lhs, dir) * dir;
				laserEndPos = laserCoords.start + b2;
				goto IL_03a1;
			}
		}
		laserEndPos = laserCoords.end;
		if (nonActorTargets != null && list != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int j = 0; j < list.Count; j++)
			{
				nonActorTargets.Add(list[j]);
			}
		}
		goto IL_03a1;
		IL_03a1:
		return actors;
	}

	public static bool LaserHitWorldGeo(float maxDistanceInSquares, VectorUtils.LaserCoords adjustedCoords, bool penetrateLos, List<ActorData> actorsHit)
	{
		if (!penetrateLos)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (actorsHit == null || actorsHit.Count <= 0)
			{
				bool result = false;
				float num = maxDistanceInSquares * Board.Get().squareSize;
				if (adjustedCoords.Length() < num - 0.1f)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					result = true;
				}
				return result;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public static Dictionary<ActorData, BouncingLaserInfo> FindBouncingLaserTargets(Vector3 originalStart, ref List<Vector3> laserAnglePoints, float widthInSquares, List<Team> validTeamsToHit, int maxTargetsHit, bool includeInvisibles, ActorData caster, List<ActorData> orderedHitActors = null, bool includeActorsAtAnglePoints = false)
	{
		Dictionary<ActorData, BouncingLaserInfo> dictionary = new Dictionary<ActorData, BouncingLaserInfo>();
		if (orderedHitActors != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			orderedHitActors.Clear();
		}
		Vector3 vector = originalStart;
		int num = 0;
		int num2 = -1;
		Vector3 value = originalStart;
		int num3 = 0;
		while (true)
		{
			if (num3 < laserAnglePoints.Count)
			{
				Vector3 vector2 = laserAnglePoints[num3];
				List<ActorData> list;
				if (GameWideData.Get().UseActorRadiusForLaser())
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					list = GetActorsInBoxByActorRadius(vector, vector2, widthInSquares, false, caster, validTeamsToHit);
				}
				else
				{
					list = GetActorsInBox(vector, vector2, widthInSquares, true, caster, validTeamsToHit);
				}
				List<ActorData> actors = list;
				actors.Remove(caster);
				if (includeActorsAtAnglePoints)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					BoardSquare boardSquare = Board.Get().GetBoardSquare(vector2);
					ActorData occupantActor = boardSquare.OccupantActor;
					if (occupantActor != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!actors.Contains(occupantActor) && IsRelevantTeam(validTeamsToHit, occupantActor.GetTeam()))
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							actors.Add(occupantActor);
						}
					}
				}
				TargeterUtils.SortActorsByDistanceToPos(ref actors, vector);
				int num4 = 0;
				while (true)
				{
					if (num4 < actors.Count)
					{
						ActorData actorData = actors[num4];
						if (!dictionary.ContainsKey(actorData))
						{
							if (!includeInvisibles && !actorData.IsVisibleToClient())
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							else
							{
								BouncingLaserInfo value2 = new BouncingLaserInfo(vector, num3);
								dictionary.Add(actorData, value2);
								if (orderedHitActors != null)
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									orderedHitActors.Add(actorData);
								}
								num++;
								if (num >= maxTargetsHit)
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									if (maxTargetsHit > 0)
									{
										while (true)
										{
											switch (1)
											{
											case 0:
												continue;
											}
											break;
										}
										num2 = num3;
										Vector3 normalized = (vector2 - vector).normalized;
										Vector3 rhs = actorData.GetTravelBoardSquareWorldPosition() - vector;
										value = vector + Vector3.Dot(normalized, rhs) * normalized;
										break;
									}
								}
							}
						}
						num4++;
						continue;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					break;
				}
				if (num2 != -1)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					break;
				}
				vector = vector2;
				num3++;
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
		}
		if (num2 != -1 && maxTargetsHit > 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			laserAnglePoints[num2] = value;
			int num5 = laserAnglePoints.Count - 1 - num2;
			if (num5 > 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				laserAnglePoints.RemoveRange(num2 + 1, num5);
			}
		}
		return dictionary;
	}

	public static void OperateOnSquaresInBounceLaser(IOperationOnSquare operationObj, Vector3 originalStart, List<Vector3> laserAnglePoints, float widthInSquares, ActorData caster, bool ignoreLos)
	{
		if (laserAnglePoints.Count <= 0)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			List<Vector3> list = new List<Vector3>();
			list.Add(originalStart);
			list.AddRange(laserAnglePoints);
			List<ISquareInsideChecker> list2 = new List<ISquareInsideChecker>();
			for (int i = 1; i < list.Count; i++)
			{
				if (i >= 2)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					SquareInsideChecker_BounceSegment squareInsideChecker_BounceSegment = new SquareInsideChecker_BounceSegment(widthInSquares);
					Vector3 normalized = (list[i] - list[i - 1]).normalized;
					Vector3 normalized2 = (list[i - 2] - list[i - 1]).normalized;
					Vector3 collisionNormal = 0.5f * (normalized + normalized2);
					squareInsideChecker_BounceSegment.UpdateBoxProperties(list[i - 1], list[i], collisionNormal, caster);
					list2.Add(squareInsideChecker_BounceSegment);
				}
				else
				{
					SquareInsideChecker_Box squareInsideChecker_Box = new SquareInsideChecker_Box(widthInSquares);
					squareInsideChecker_Box.UpdateBoxProperties(list[i - 1], list[i], caster);
					list2.Add(squareInsideChecker_Box);
				}
			}
			for (int j = 1; j < list.Count; j++)
			{
				OperateOnSquaresInBoxByActorRadius(operationObj, list[j - 1], list[j], widthInSquares, caster, ignoreLos, null, list2, false);
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public static List<BoardSquare> GetSquaresInTriangle(Vector3 pA, Vector3 pB, Vector3 pC, bool ignoreLoS, ActorData caster)
	{
		float x = Mathf.Min(pA.x, Mathf.Min(pB.x, pC.x));
		float y = Mathf.Min(pA.z, Mathf.Min(pB.z, pC.z));
		float x2 = Mathf.Max(pA.x, Mathf.Max(pB.x, pC.x));
		float y2 = Mathf.Max(pA.z, Mathf.Max(pB.z, pC.z));
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(x, y);
		BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(x2, y2);
		List<BoardSquare> squaresInRect = Board.Get().GetSquaresInRect(boardSquareSafe, boardSquareSafe2);
		List<BoardSquare> list = new List<BoardSquare>();
		BoardSquare boardSquare = Board.Get().GetBoardSquare(pA);
		using (List<BoardSquare>.Enumerator enumerator = squaresInRect.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				if (VectorUtils.IsPointInTriangle(pA, pB, pC, current.ToVector3()))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					bool flag = true;
					if (!ignoreLoS)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						flag = SquaresHaveLoSForAbilities(boardSquare, current, caster);
					}
					if (flag)
					{
						list.Add(current);
					}
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
	}

	public static List<ActorData> GetActorsInTriangle(Vector3 pA, Vector3 pB, Vector3 pC, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams)
	{
		List<ActorData> list = new List<ActorData>();
		List<BoardSquare> squaresInTriangle = GetSquaresInTriangle(pA, pB, pC, ignoreLoS, caster);
		using (List<BoardSquare>.Enumerator enumerator = squaresInTriangle.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				ActorData occupantActor = current.OccupantActor;
				if (IsActorTargetable(occupantActor, onlyValidTeams))
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (!list.Contains(occupantActor))
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						list.Add(occupantActor);
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
	}

	public static List<BoardSquare> GetSquaresInShape_EvenByEven(Vector3 cornerPos, int dimensions, int cornersToSubtract, bool ignoreLoS, ActorData caster)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		List<BoardSquare> list2 = new List<BoardSquare>();
		float num = Board.Get().squareSize / 2f;
		if (!ignoreLoS)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			List<BoardSquare> squaresAroundEvenShapeCornerPos = GetSquaresAroundEvenShapeCornerPos(cornerPos);
			list2 = GetCenterSquaresForEvenShapeLos(squaresAroundEvenShapeCornerPos, caster);
		}
		float num2 = cornerPos.x + num - Board.Get().squareSize * (float)(dimensions / 2);
		float num3 = cornerPos.z + num - Board.Get().squareSize * (float)(dimensions / 2);
		for (int i = 0; i < dimensions; i++)
		{
			float x = num2 + Board.Get().squareSize * (float)i;
			for (int j = 0; j < dimensions; j++)
			{
				if (cornersToSubtract > 0)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					int num4 = Mathf.Min(dimensions - 1 - i, i);
					int num5 = Mathf.Min(dimensions - 1 - j, j);
					int num6 = num4 + num5;
					if (num6 < cornersToSubtract)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						continue;
					}
				}
				float y = num3 + Board.Get().squareSize * (float)j;
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(x, y);
				if (!(boardSquareSafe != null))
				{
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!boardSquareSafe.IsBaselineHeight())
				{
					continue;
				}
				bool flag;
				if (ignoreLoS)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
				}
				else if (list2.Contains(boardSquareSafe))
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
				}
				else
				{
					int num7 = 0;
					using (List<BoardSquare>.Enumerator enumerator = list2.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BoardSquare current = enumerator.Current;
							if (SquaresHaveLoSForAbilities(current, boardSquareSafe, caster))
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								num7++;
							}
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					flag = ((num7 >= 3) ? true : false);
				}
				if (flag)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					list.Add(boardSquareSafe);
				}
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			return list;
		}
	}

	private static List<BoardSquare> GetSquaresAroundEvenShapeCornerPos(Vector3 cornerPos)
	{
		float num = Board.Get().squareSize / 2f;
		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = -1; i <= 1; i += 2)
		{
			for (int j = -1; j <= 1; j += 2)
			{
				BoardSquare boardSquare = Board.Get().GetBoardSquare(cornerPos + new Vector3(num * (float)i, 0f, num * (float)j));
				if (boardSquare != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					list.Add(boardSquare);
				}
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return list;
		}
	}

	private static List<BoardSquare> GetCenterSquaresForEvenShapeLos(List<BoardSquare> squaresByCorner, ActorData caster)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		using (List<BoardSquare>.Enumerator enumerator = squaresByCorner.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				int num = 0;
				using (List<BoardSquare>.Enumerator enumerator2 = squaresByCorner.GetEnumerator())
				{
					while (true)
					{
						if (!enumerator2.MoveNext())
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							break;
						}
						BoardSquare current2 = enumerator2.Current;
						if (current != current2 && SquaresHaveLoSForAbilities(current, current2, caster))
						{
							num++;
							if (num >= 2)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										if (1 == 0)
										{
											/*OpCode not supported: LdMemberToken*/;
										}
										list.Add(current);
										goto end_IL_0028;
									}
								}
							}
						}
					}
					end_IL_0028:;
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
	}

	public static List<BoardSquare> GetSquaresInShape_OddByOdd(BoardSquare centerSquare, int dimensions, int cornersToSubtract, bool ignoreLoS, ActorData caster)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		int num = dimensions / 2;
		int num2 = centerSquare.x - num;
		int num3 = centerSquare.x + num;
		int num4 = centerSquare.y - num;
		int num5 = centerSquare.y + num;
		for (int i = num2; i <= num3; i++)
		{
			for (int j = num4; j <= num5; j++)
			{
				if (cornersToSubtract > 0)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					int num6 = Mathf.Min(num3 - i, i - num2);
					int num7 = Mathf.Min(num5 - j, j - num4);
					int num8 = num6 + num7;
					if (num8 < cornersToSubtract)
					{
						continue;
					}
				}
				BoardSquare boardSquare = Board.Get().GetBoardSquare(i, j);
				if (!(boardSquare != null))
				{
					continue;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!boardSquare.IsBaselineHeight())
				{
					continue;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				int num9;
				if (!ignoreLoS)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(boardSquare == centerSquare))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						num9 = (SquaresHaveLoSForAbilities(centerSquare, boardSquare, caster) ? 1 : 0);
						goto IL_0102;
					}
				}
				num9 = 1;
				goto IL_0102;
				IL_0102:
				if (num9 != 0)
				{
					list.Add(boardSquare);
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					goto end_IL_011f;
				}
				continue;
				end_IL_011f:
				break;
			}
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			return list;
		}
	}

	public static List<BoardSquare> GetSquaresInShape(AbilityAreaShape shape, AbilityTarget target, bool ignoreLoS, ActorData caster)
	{
		Vector3 freePos = target.FreePos;
		GridPos gridPos = target.GridPos;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
		return GetSquaresInShape(shape, freePos, boardSquareSafe, ignoreLoS, caster);
	}

	private static void GetSquareDimentionAndCornersToSubtract(AbilityAreaShape shape, out int dimensions, out int cornersToSubtract)
	{
		cornersToSubtract = 0;
		switch (shape)
		{
		case AbilityAreaShape.SingleSquare:
			dimensions = 1;
			break;
		case AbilityAreaShape.Two_x_Two:
			dimensions = 2;
			break;
		case AbilityAreaShape.Three_x_Three_NoCorners:
			cornersToSubtract = 1;
			dimensions = 3;
			break;
		case AbilityAreaShape.Three_x_Three:
			dimensions = 3;
			break;
		case AbilityAreaShape.Four_x_Four_NoCorners:
			cornersToSubtract = 1;
			dimensions = 4;
			break;
		case AbilityAreaShape.Four_x_Four:
			dimensions = 4;
			break;
		case AbilityAreaShape.Five_x_Five_ExtraNoCorners:
			cornersToSubtract = 2;
			dimensions = 5;
			break;
		case AbilityAreaShape.Five_x_Five_NoCorners:
			cornersToSubtract = 1;
			dimensions = 5;
			break;
		case AbilityAreaShape.Five_x_Five:
			dimensions = 5;
			break;
		case AbilityAreaShape.Six_x_Six_ExtraNoCorners:
			cornersToSubtract = 2;
			dimensions = 6;
			break;
		case AbilityAreaShape.Six_x_Six_NoCorners:
			cornersToSubtract = 1;
			dimensions = 6;
			break;
		case AbilityAreaShape.Six_x_Six:
			dimensions = 6;
			break;
		case AbilityAreaShape.Seven_x_Seven_ExtraNoCorners:
			cornersToSubtract = 2;
			dimensions = 7;
			break;
		case AbilityAreaShape.Seven_x_Seven_NoCorners:
			cornersToSubtract = 1;
			dimensions = 7;
			break;
		case AbilityAreaShape.Seven_x_Seven:
			dimensions = 7;
			break;
		case AbilityAreaShape.Eight_x_Eight_NoCorners:
			cornersToSubtract = 1;
			dimensions = 8;
			break;
		case AbilityAreaShape.Eight_x_Eight:
			dimensions = 8;
			break;
		case AbilityAreaShape.Nine_x_Nine_NoCorners:
			cornersToSubtract = 2;
			dimensions = 9;
			break;
		case AbilityAreaShape.Nine_x_Nine:
			dimensions = 9;
			break;
		case AbilityAreaShape.Ten_x_Ten_NoCorners:
			cornersToSubtract = 2;
			dimensions = 10;
			break;
		case AbilityAreaShape.Ten_x_Ten:
			dimensions = 10;
			break;
		case AbilityAreaShape.Eleven_x_Eleven_NoCorners:
			cornersToSubtract = 2;
			dimensions = 11;
			break;
		case AbilityAreaShape.Eleven_x_Eleven:
			dimensions = 11;
			break;
		default:
			dimensions = 1;
			break;
		}
	}

	public static List<BoardSquare> GetSquaresInShape(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare, bool ignoreLoS, ActorData caster)
	{
		GetSquareDimentionAndCornersToSubtract(shape, out int dimensions, out int cornersToSubtract);
		if (dimensions % 2 == 1)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return GetSquaresInShape_OddByOdd(centerSquare, dimensions, cornersToSubtract, ignoreLoS, caster);
				}
			}
		}
		Vector3 cornerPos = Board._000E(freePos, centerSquare);
		return GetSquaresInShape_EvenByEven(cornerPos, dimensions, cornersToSubtract, ignoreLoS, caster);
	}

	public static List<ActorData> GetActorsInShape(AbilityAreaShape shape, AbilityTarget target, bool ignoreLoS, ActorData caster, Team onlyValidTeam, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 freePos = target.FreePos;
		GridPos gridPos = target.GridPos;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
		List<Team> list = new List<Team>();
		list.Add(onlyValidTeam);
		return GetActorsInShape(shape, freePos, boardSquareSafe, ignoreLoS, caster, list, nonActorTargetInfo);
	}

	public static List<ActorData> GetActorsInShape(AbilityAreaShape shape, AbilityTarget target, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 freePos = target.FreePos;
		GridPos gridPos = target.GridPos;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
		return GetActorsInShape(shape, freePos, boardSquareSafe, ignoreLoS, caster, onlyValidTeams, nonActorTargetInfo);
	}

	public static List<ActorData> GetActorsInShape(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare, bool ignoreLoS, ActorData caster, Team onlyValidTeam, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> list = new List<Team>();
		list.Add(onlyValidTeam);
		return GetActorsInShape(shape, freePos, centerSquare, ignoreLoS, caster, list, nonActorTargetInfo);
	}

	public static List<ActorData> GetActorsInShape(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list = new List<ActorData>();
		List<ActorData> actors = GameFlowData.Get().GetActors();
		using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (IsActorTargetable(current, onlyValidTeams))
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (IsSquareInShape(current.GetCurrentBoardSquare(), shape, freePos, centerSquare, ignoreLoS, caster))
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						list.Add(current);
					}
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
	}

	public static List<ActorData> GetActorsInShapeLayers(List<AbilityAreaShape> shapes, Vector3 freePos, BoardSquare centerSquare, bool ignoreLos, ActorData caster, List<Team> onlyValidTeams, out List<List<ActorData>> actorsInLayers, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list = new List<ActorData>();
		actorsInLayers = new List<List<ActorData>>();
		for (int i = 0; i < shapes.Count; i++)
		{
			actorsInLayers.Add(new List<ActorData>());
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			List<ActorData> actors = GameFlowData.Get().GetActors();
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (IsActorTargetable(current, onlyValidTeams))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						int num = 0;
						while (true)
						{
							if (num >= shapes.Count)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								break;
							}
							AbilityAreaShape shape = shapes[num];
							if (IsSquareInShape(current.GetCurrentBoardSquare(), shape, freePos, centerSquare, ignoreLos, caster))
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								actorsInLayers[num].Add(current);
								list.Add(current);
								break;
							}
							num++;
						}
					}
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return list;
					}
				}
			}
		}
	}

	public static void OperateOnSquaresInShape(IOperationOnSquare operationObj, AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare, bool ignoreLoS, ActorData caster, List<ISquareInsideChecker> losCheckOverrides = null)
	{
		GetSquareDimentionAndCornersToSubtract(shape, out int dimensions, out int cornersToSubtract);
		if (dimensions % 2 == 1)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					OperateOnSquaresInShape_OddByOdd(operationObj, centerSquare, dimensions, cornersToSubtract, ignoreLoS, caster, losCheckOverrides);
					return;
				}
			}
		}
		Vector3 cornerPos = Board._000E(freePos, centerSquare);
		OperateOnSquaresInShape_EvenByEven(operationObj, cornerPos, dimensions, cornersToSubtract, ignoreLoS, caster, losCheckOverrides);
	}

	public static void OperateOnSquaresInShape_EvenByEven(IOperationOnSquare operationObj, Vector3 cornerPos, int dimensions, int cornersToSubtract, bool ignoreLoS, ActorData caster, List<ISquareInsideChecker> losCheckOverrides = null)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		float num = Board.Get().squareSize / 2f;
		if (!ignoreLoS)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			List<BoardSquare> squaresAroundEvenShapeCornerPos = GetSquaresAroundEvenShapeCornerPos(cornerPos);
			list = GetCenterSquaresForEvenShapeLos(squaresAroundEvenShapeCornerPos, caster);
		}
		float num2 = cornerPos.x + num - Board.Get().squareSize * (float)(dimensions / 2);
		float num3 = cornerPos.z + num - Board.Get().squareSize * (float)(dimensions / 2);
		for (int i = 0; i < dimensions; i++)
		{
			float x = num2 + Board.Get().squareSize * (float)i;
			for (int j = 0; j < dimensions; j++)
			{
				if (cornersToSubtract > 0)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					int num4 = Mathf.Min(dimensions - 1 - i, i);
					int num5 = Mathf.Min(dimensions - 1 - j, j);
					int num6 = num4 + num5;
					if (num6 < cornersToSubtract)
					{
						continue;
					}
				}
				float y = num3 + Board.Get().squareSize * (float)j;
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(x, y);
				if (!(boardSquareSafe != null))
				{
					continue;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!boardSquareSafe.IsBaselineHeight())
				{
					continue;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				bool squareHasLos;
				if (ignoreLoS)
				{
					squareHasLos = true;
				}
				else if (list.Contains(boardSquareSafe))
				{
					squareHasLos = true;
				}
				else if (losCheckOverrides != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					squareHasLos = SquareHasLosByAreaCheckers(boardSquareSafe, losCheckOverrides);
				}
				else
				{
					int num7 = 0;
					using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BoardSquare current = enumerator.Current;
							if (SquaresHaveLoSForAbilities(current, boardSquareSafe, caster))
							{
								num7++;
							}
						}
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					if (num7 >= 3)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						squareHasLos = true;
					}
					else
					{
						squareHasLos = false;
					}
				}
				operationObj.OperateOnSquare(boardSquareSafe, caster, squareHasLos);
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					goto end_IL_01fc;
				}
				continue;
				end_IL_01fc:
				break;
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public static void OperateOnSquaresInShape_OddByOdd(IOperationOnSquare operationObj, BoardSquare centerSquare, int dimensions, int cornersToSubtract, bool ignoreLoS, ActorData caster, List<ISquareInsideChecker> losCheckOverrides = null)
	{
		int num = dimensions / 2;
		int num2 = centerSquare.x - num;
		int num3 = centerSquare.x + num;
		int num4 = centerSquare.y - num;
		int num5 = centerSquare.y + num;
		for (int i = num2; i <= num3; i++)
		{
			for (int j = num4; j <= num5; j++)
			{
				if (cornersToSubtract > 0)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					int num6 = Mathf.Min(num3 - i, i - num2);
					int num7 = Mathf.Min(num5 - j, j - num4);
					int num8 = num6 + num7;
					if (num8 < cornersToSubtract)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						continue;
					}
				}
				BoardSquare boardSquare = Board.Get().GetBoardSquare(i, j);
				if (!(boardSquare != null))
				{
					continue;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!boardSquare.IsBaselineHeight())
				{
					continue;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag = false;
				if (ignoreLoS)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
				}
				else if (losCheckOverrides != null)
				{
					flag = SquareHasLosByAreaCheckers(boardSquare, losCheckOverrides);
				}
				else
				{
					int num9;
					if (!(boardSquare == centerSquare))
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						num9 = (SquaresHaveLoSForAbilities(centerSquare, boardSquare, caster) ? 1 : 0);
					}
					else
					{
						num9 = 1;
					}
					flag = ((byte)num9 != 0);
				}
				operationObj.OperateOnSquare(boardSquare, caster, flag);
			}
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public static bool IsSquareInShape(BoardSquare testSquare, AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare, bool ignoreLoS, ActorData caster)
	{
		GetSquareDimentionAndCornersToSubtract(shape, out int dimensions, out int cornersToSubtract);
		if (dimensions % 2 == 1)
		{
			return IsSquareInShape_OddByOdd(testSquare, centerSquare, dimensions, cornersToSubtract, ignoreLoS, caster);
		}
		Vector3 cornerPos = Board._000E(freePos, centerSquare);
		return IsSquareInShape_EvenByEven(testSquare, cornerPos, dimensions, cornersToSubtract, ignoreLoS, caster);
	}

	private static List<BoardSquare> GetSquaresAroundCornerPos(Vector3 cornerPos)
	{
		float num = Board.Get().squareSize / 2f;
		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = -1; i <= 1; i += 2)
		{
			for (int j = -1; j <= 1; j += 2)
			{
				BoardSquare boardSquare = Board.Get().GetBoardSquare(cornerPos + new Vector3(num * (float)i, 0f, num * (float)j));
				if (boardSquare != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					list.Add(boardSquare);
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					goto end_IL_007a;
				}
				continue;
				end_IL_007a:
				break;
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return list;
		}
	}

	private static List<BoardSquare> GetCenterSquaresForShapeLos(List<BoardSquare> squaresByCorner, ActorData caster)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		using (List<BoardSquare>.Enumerator enumerator = squaresByCorner.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare current = enumerator.Current;
				int num = 0;
				using (List<BoardSquare>.Enumerator enumerator2 = squaresByCorner.GetEnumerator())
				{
					while (true)
					{
						if (!enumerator2.MoveNext())
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							break;
						}
						BoardSquare current2 = enumerator2.Current;
						if (current != current2)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							if (SquaresHaveLoSForAbilities(current, current2, caster))
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								num++;
								if (num >= 2)
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
											list.Add(current);
											goto end_IL_0028;
										}
									}
								}
							}
						}
					}
					end_IL_0028:;
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
	}

	private static bool IsSquareInShape_EvenByEven(BoardSquare testSquare, Vector3 cornerPos, List<BoardSquare> squaresInCenter, int dimensions, int cornersToSubtract, bool ignoreLoS, ActorData caster)
	{
		if (testSquare == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		bool result = false;
		float num = Board.Get().squareSize / 2f;
		float num2 = cornerPos.x + num;
		float num3 = cornerPos.z + num;
		float squareSize = Board.Get().squareSize;
		int num4 = dimensions / 2;
		int num5 = 0;
		int num6 = Mathf.RoundToInt((num2 - testSquare.worldX) / squareSize);
		if (num6 > 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			num5 = num4 - num6;
		}
		else
		{
			num5 = num4 - Mathf.Abs(num6) - 1;
		}
		int num7 = 0;
		int num8 = Mathf.RoundToInt((num3 - testSquare.worldY) / squareSize);
		if (num8 > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			num7 = num4 - num8;
		}
		else
		{
			num7 = num4 - Mathf.Abs(num8) - 1;
		}
		if (num5 >= 0 && num7 >= 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			int num9 = num5 + num7;
			if (num9 >= cornersToSubtract)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				result = true;
				if (!ignoreLoS)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					bool flag = false;
					if (squaresInCenter.Contains(testSquare))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						flag = true;
					}
					else
					{
						int num10 = 0;
						foreach (BoardSquare item in squaresInCenter)
						{
							if (SquaresHaveLoSForAbilities(item, testSquare, caster))
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								num10++;
							}
						}
						flag = (num10 >= 3);
					}
					result = flag;
				}
			}
		}
		return result;
	}

	private static bool IsSquareInShape_EvenByEven(BoardSquare testSquare, Vector3 cornerPos, int dimensions, int cornersToSubtract, bool ignoreLoS, ActorData caster)
	{
		if (testSquare == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		List<BoardSquare> squaresInCenter = null;
		if (!ignoreLoS)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			List<BoardSquare> squaresAroundCornerPos = GetSquaresAroundCornerPos(cornerPos);
			squaresInCenter = GetCenterSquaresForShapeLos(squaresAroundCornerPos, caster);
		}
		return IsSquareInShape_EvenByEven(testSquare, cornerPos, squaresInCenter, dimensions, cornersToSubtract, ignoreLoS, caster);
	}

	private static bool IsSquareInShape_OddByOdd(BoardSquare testSquare, BoardSquare centerSquare, int dimensions, int cornersToSubtract, bool ignoreLoS, ActorData caster)
	{
		if (testSquare == null || centerSquare == null)
		{
			return false;
		}
		bool result = false;
		int num = dimensions / 2;
		int num2 = Mathf.Abs(testSquare.x - centerSquare.x);
		int num3 = Mathf.Abs(testSquare.y - centerSquare.y);
		int num7;
		if (num2 <= num)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (num3 <= num)
			{
				int num4 = num - num2;
				int num5 = num - num3;
				int num6 = num4 + num5;
				if (num6 >= cornersToSubtract)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!ignoreLoS)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!(testSquare == centerSquare))
						{
							num7 = (SquaresHaveLoSForAbilities(centerSquare, testSquare, caster) ? 1 : 0);
							goto IL_00b2;
						}
					}
					num7 = 1;
					goto IL_00b2;
				}
			}
		}
		goto IL_00b3;
		IL_00b2:
		result = ((byte)num7 != 0);
		goto IL_00b3;
		IL_00b3:
		return result;
	}

	public static bool IsPosInShape(Vector3 testPos, AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		GetSquareDimentionAndCornersToSubtract(shape, out int _, out int _);
		BoardSquare boardSquare = Board.Get().GetBoardSquare(testPos);
		if (boardSquare != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return IsSquareInShape(boardSquare, shape, freePos, centerSquare, true, null);
				}
			}
		}
		return false;
	}

	private static bool IsPosInShape_OddByOdd(Vector3 testPos, BoardSquare centerSquare, int dimensions, int cornersToSubtract)
	{
		if (centerSquare == null)
		{
			return false;
		}
		float num = 0.5f * Board.Get().squareSize;
		float squareSize = Board.Get().squareSize;
		bool result = false;
		int num2 = dimensions / 2;
		Vector3 vector = centerSquare.ToVector3();
		float num3 = Mathf.Abs(testPos.x - vector.x);
		float num4 = Mathf.Abs(testPos.z - vector.z);
		num3 = Mathf.Max(0f, num3 - num + 0.05f);
		num4 = Mathf.Max(0f, num4 - num + 0.05f);
		float num5 = num3 / squareSize;
		float num6 = num4 / squareSize;
		if (num5 <= (float)num2)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (num6 <= (float)num2)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				float num7 = (float)num2 - num5;
				float num8 = (float)num2 - num6;
				float num9 = num7 + num8;
				if (num9 >= (float)cornersToSubtract)
				{
					result = true;
				}
			}
		}
		return result;
	}

	private static bool IsPosInShape_EvenByEven(Vector3 testPos, Vector3 cornerPos, int dimensions, int cornersToSubtract)
	{
		bool result = false;
		float num = Board.Get().squareSize / 2f;
		float num2 = cornerPos.x + num;
		float num3 = cornerPos.z + num;
		float squareSize = Board.Get().squareSize;
		int num4 = dimensions / 2;
		int num5 = 0;
		int num6 = Mathf.RoundToInt((num2 - testPos.x) / squareSize);
		num5 = ((num6 <= 0) ? (num4 - Mathf.Abs(num6) - 1) : (num4 - num6));
		int num7 = 0;
		int num8 = Mathf.RoundToInt((num3 - testPos.z) / squareSize);
		if (num8 > 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num7 = num4 - num8;
		}
		else
		{
			num7 = num4 - Mathf.Abs(num8) - 1;
		}
		if (num5 >= 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (num7 >= 0)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				int num9 = num5 + num7;
				if (num9 >= cornersToSubtract)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					result = true;
				}
			}
		}
		return result;
	}

	public static Vector3 GetCenterOfShape(AbilityAreaShape shape, AbilityTarget target)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		return GetCenterOfShape(shape, target.FreePos, boardSquareSafe);
	}

	public static bool IsShapeOddByOdd(AbilityAreaShape shape)
	{
		bool flag = false;
		switch (shape)
		{
		case AbilityAreaShape.SingleSquare:
			return true;
		case AbilityAreaShape.Two_x_Two:
			return false;
		case AbilityAreaShape.Three_x_Three:
			return true;
		case AbilityAreaShape.Three_x_Three_NoCorners:
			return true;
		case AbilityAreaShape.Four_x_Four:
			return false;
		case AbilityAreaShape.Four_x_Four_NoCorners:
			return false;
		case AbilityAreaShape.Five_x_Five:
			return true;
		case AbilityAreaShape.Five_x_Five_NoCorners:
			return true;
		case AbilityAreaShape.Five_x_Five_ExtraNoCorners:
			return true;
		case AbilityAreaShape.Six_x_Six:
			return false;
		case AbilityAreaShape.Six_x_Six_NoCorners:
			return false;
		case AbilityAreaShape.Six_x_Six_ExtraNoCorners:
			return false;
		case AbilityAreaShape.Seven_x_Seven:
			return true;
		case AbilityAreaShape.Seven_x_Seven_NoCorners:
			return true;
		case AbilityAreaShape.Seven_x_Seven_ExtraNoCorners:
			return true;
		default:
			return true;
		}
	}

	public static Vector3 GetCenterOfShape(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		if (IsShapeOddByOdd(shape))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return centerSquare.ToVector3();
				}
			}
		}
		return Board._000E(freePos, centerSquare);
	}

	public static Vector3 GetCenterOfGridPattern(AbilityGridPattern pattern, AbilityTarget target)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		return GetCenterOfGridPattern(pattern, target.FreePos, boardSquareSafe);
	}

	public static Vector3 GetCenterOfGridPattern(AbilityGridPattern pattern, Vector3 freePos, BoardSquare centerSquare)
	{
		bool flag;
		if (pattern != AbilityGridPattern.Plus_Two_x_Two)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (pattern != AbilityGridPattern.Plus_Four_x_Four)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				flag = true;
			}
			else
			{
				flag = false;
			}
		}
		else
		{
			flag = false;
		}
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return centerSquare.ToVector3();
				}
			}
		}
		return Board._000E(freePos, centerSquare);
	}

	public static bool HasAdjacentActorOfTeam(ActorData aroundActor, List<Team> teams)
	{
		if (IsActorTargetable(aroundActor))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			List<ActorData> actors = GameFlowData.Get().GetActors();
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (current != aroundActor)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (IsActorTargetable(current, teams))
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							if (Board.Get()._000E(aroundActor.GetCurrentBoardSquare(), current.GetCurrentBoardSquare()))
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
										return true;
									}
								}
							}
						}
					}
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return false;
	}

	public static void AddShapeCornersToList(ref List<Vector3> points, AbilityAreaShape shape, AbilityTarget target)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		AddShapeCornersToList(ref points, shape, target.FreePos, boardSquareSafe);
	}

	public static void AddShapeCornersToList(ref List<Vector3> points, AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		Vector3 centerOfShape = GetCenterOfShape(shape, freePos, centerSquare);
		float num;
		if (shape == AbilityAreaShape.SingleSquare)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = 1f;
		}
		else if (shape == AbilityAreaShape.Two_x_Two)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			num = 2f;
		}
		else
		{
			if (shape != AbilityAreaShape.Three_x_Three)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (shape != AbilityAreaShape.Three_x_Three_NoCorners)
				{
					if (shape != AbilityAreaShape.Four_x_Four)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (shape != AbilityAreaShape.Four_x_Four_NoCorners)
						{
							if (shape != AbilityAreaShape.Five_x_Five)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (shape != AbilityAreaShape.Five_x_Five_NoCorners)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									if (shape != AbilityAreaShape.Five_x_Five_ExtraNoCorners)
									{
										if (shape != AbilityAreaShape.Six_x_Six)
										{
											while (true)
											{
												switch (7)
												{
												case 0:
													continue;
												}
												break;
											}
											if (shape != AbilityAreaShape.Six_x_Six_NoCorners)
											{
												while (true)
												{
													switch (3)
													{
													case 0:
														continue;
													}
													break;
												}
												if (shape != AbilityAreaShape.Six_x_Six_ExtraNoCorners)
												{
													if (shape != AbilityAreaShape.Seven_x_Seven)
													{
														while (true)
														{
															switch (1)
															{
															case 0:
																continue;
															}
															break;
														}
														if (shape != AbilityAreaShape.Seven_x_Seven_NoCorners)
														{
															while (true)
															{
																switch (7)
																{
																case 0:
																	continue;
																}
																break;
															}
															if (shape != AbilityAreaShape.Seven_x_Seven_ExtraNoCorners)
															{
																num = 0f;
																goto IL_012e;
															}
															while (true)
															{
																switch (7)
																{
																case 0:
																	continue;
																}
																break;
															}
														}
													}
													num = 7f;
													goto IL_012e;
												}
												while (true)
												{
													switch (2)
													{
													case 0:
														continue;
													}
													break;
												}
											}
										}
										num = 6f;
										goto IL_012e;
									}
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
								}
							}
							num = 5f;
							goto IL_012e;
						}
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					num = 4f;
					goto IL_012e;
				}
			}
			num = 3f;
		}
		goto IL_012e;
		IL_012e:
		float num2 = num / 2f;
		float num3 = num2 * Board.Get().squareSize;
		points.Add(new Vector3(centerOfShape.x + num3, centerOfShape.y, centerOfShape.z + num3));
		points.Add(new Vector3(centerOfShape.x + num3, centerOfShape.y, centerOfShape.z - num3));
		points.Add(new Vector3(centerOfShape.x - num3, centerOfShape.y, centerOfShape.z + num3));
		points.Add(new Vector3(centerOfShape.x - num3, centerOfShape.y, centerOfShape.z - num3));
	}

	public static List<Vector3> BuildShapeCornersList(AbilityAreaShape shape, AbilityTarget target)
	{
		List<Vector3> points = new List<Vector3>();
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		AddShapeCornersToList(ref points, shape, target.FreePos, boardSquareSafe);
		return points;
	}

	public static List<Vector3> BuildShapeCornersList(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		List<Vector3> points = new List<Vector3>();
		AddShapeCornersToList(ref points, shape, freePos, centerSquare);
		return points;
	}

	public static void AddConeExtremaToList(ref List<Vector3> points, Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares)
	{
		Vector3 a = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float d = coneBackwardOffsetInSquares * Board.Get().squareSize;
		Vector3 vector = coneStart - a * d;
		float num = coneLengthRadiusInSquares + coneBackwardOffsetInSquares;
		float d2 = num * Board.Get().squareSize;
		float num2 = coneWidthDegrees / 2f;
		float angle = coneCenterAngleDegrees - num2;
		float angle2 = coneCenterAngleDegrees + num2;
		Vector3 a2 = VectorUtils.AngleDegreesToVector(angle);
		Vector3 a3 = VectorUtils.AngleDegreesToVector(angle2);
		points.Add(vector);
		points.Add(vector + a * d2);
		points.Add(vector + a2 * d2);
		points.Add(vector + a3 * d2);
	}

	public static void AddBoxExtremaToList(ref List<Vector3> points, Vector3 startPos, Vector3 endPos, float boxWidthInSquares)
	{
		float d = boxWidthInSquares * Board.Get().squareSize;
		Vector3 normalized = (endPos - startPos).normalized;
		Vector3 a = Vector3.Cross(normalized, Vector3.up);
		Vector3 b = a * 0.5f * d;
		points.Add(startPos + b);
		points.Add(startPos - b);
		points.Add(endPos + b);
		points.Add(endPos - b);
	}

	public static void AddRadiusExtremaToList(ref List<Vector3> points, Vector3 startPos, float radiusInSquares)
	{
		float num = radiusInSquares * Board.Get().squareSize;
		Vector3 b = new Vector3(num, 0f, 0f);
		Vector3 b2 = new Vector3(0f, 0f, num);
		points.Add(startPos + b + b2);
		points.Add(startPos + b - b2);
		points.Add(startPos - b + b2);
		points.Add(startPos - b - b2);
	}
}
