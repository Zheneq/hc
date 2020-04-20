using System;
using System.Collections.Generic;
using UnityEngine;

public static class AreaEffectUtils
{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetMaxAngleForActorBounce()).MethodHandle;
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
		if (actor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsActorTargetable(ActorData, List<Team>)).MethodHandle;
			}
			if (!actor.IsDead() && !actor.IgnoreForAbilityHits)
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
				if (actor.GetCurrentBoardSquare() != null)
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
					return AreaEffectUtils.IsRelevantTeam(validTeams, actor.GetTeam());
				}
			}
		}
		return false;
	}

	public static bool IsRelevantTeam(List<Team> validTeams, Team team)
	{
		if (validTeams == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsRelevantTeam(List<Team>, Team)).MethodHandle;
			}
			return true;
		}
		for (int i = 0; i < validTeams.Count; i++)
		{
			if (validTeams[i] == team)
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
				return true;
			}
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
		return false;
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
		int num2 = (int)Mathf.Min((float)maxX, (centerX + outerRadius) / squareSize);
		int num3 = (int)Mathf.Max(0f, (centerY - outerRadius) / squareSize);
		int num4 = (int)Mathf.Min((float)maxY, (centerY + outerRadius) / squareSize);
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				Vector3 b = new Vector3((float)i * squareSize, 0f, (float)j * squareSize);
				float sqrMagnitude = (a - b).sqrMagnitude;
				if (sqrMagnitude < outerRadius * outerRadius && sqrMagnitude > innerRadius * innerRadius)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetValidRespawnSquaresInDonut(float, float, float, float)).MethodHandle;
					}
					BoardSquare boardSquare = Board.Get().GetBoardSquare(i, j);
					if (boardSquare != null)
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
						if (boardSquare.IsBaselineHeight())
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
							if (boardSquare.OccupantActor == null)
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
								list.Add(boardSquare);
							}
						}
					}
				}
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

	private static void AddSquareAtIndexToListIfValid(int x, int y, List<BoardSquare> list)
	{
		BoardSquare boardSquare = Board.Get().GetBoardSquare(x, y);
		if (boardSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.AddSquareAtIndexToListIfValid(int, int, List<BoardSquare>)).MethodHandle;
			}
			list.Add(boardSquare);
		}
	}

	public static List<BoardSquare> GetSquaresInBorderLayer(BoardSquare center, int borderLayerNumber, bool requireLosToCenter)
	{
		if (!(center == null))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetSquaresInBorderLayer(BoardSquare, int, bool)).MethodHandle;
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
					AreaEffectUtils.AddSquareAtIndexToListIfValid(center.x + borderLayerNumber, center.y, list);
					AreaEffectUtils.AddSquareAtIndexToListIfValid(center.x - borderLayerNumber, center.y, list);
					AreaEffectUtils.AddSquareAtIndexToListIfValid(center.x, center.y + borderLayerNumber, list);
					AreaEffectUtils.AddSquareAtIndexToListIfValid(center.x, center.y - borderLayerNumber, list);
					for (int i = 1; i <= borderLayerNumber; i++)
					{
						int num = i;
						AreaEffectUtils.AddSquareAtIndexToListIfValid(center.x + borderLayerNumber, center.y + num, list);
						AreaEffectUtils.AddSquareAtIndexToListIfValid(center.x - borderLayerNumber, center.y + num, list);
						AreaEffectUtils.AddSquareAtIndexToListIfValid(center.x + borderLayerNumber, center.y - num, list);
						AreaEffectUtils.AddSquareAtIndexToListIfValid(center.x - borderLayerNumber, center.y - num, list);
						if (borderLayerNumber != num)
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
							int num2 = i;
							AreaEffectUtils.AddSquareAtIndexToListIfValid(center.x + num2, center.y + borderLayerNumber, list);
							AreaEffectUtils.AddSquareAtIndexToListIfValid(center.x - num2, center.y + borderLayerNumber, list);
							AreaEffectUtils.AddSquareAtIndexToListIfValid(center.x + num2, center.y - borderLayerNumber, list);
							AreaEffectUtils.AddSquareAtIndexToListIfValid(center.x - num2, center.y - borderLayerNumber, list);
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
					if (requireLosToCenter)
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
						for (int j = list.Count - 1; j >= 0; j--)
						{
							BoardSquare boardSquare = list[j];
							if (!center.\u0013(boardSquare.x, boardSquare.y))
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
								list.RemoveAt(j);
							}
						}
					}
				}
				return list;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.PointInBox(Vector3, Vector3, Vector3, float)).MethodHandle;
			}
			if (AreaEffectUtils.PointToLineDistance(testPt, pt1, pt2) < halfWidth)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.<SortSquaresByDistanceToPos>c__AnonStorey0.<>m__0(BoardSquare, BoardSquare)).MethodHandle;
				}
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
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
				return 1;
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
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					return x.x.CompareTo(y.x);
				}
				if (x.y != y.y)
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
					return x.y.CompareTo(y.y);
				}
			}
			return sqrMagnitude.CompareTo(sqrMagnitude2);
		});
	}

	public unsafe static bool GetEndPointForValidGameplaySquare(Vector3 start, Vector3 end, out Vector3 adjustedEndPoint)
	{
		adjustedEndPoint = end;
		Vector3 a = start - end;
		a.y = 0f;
		float magnitude = a.magnitude;
		a.Normalize();
		if (magnitude > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetEndPointForValidGameplaySquare(Vector3, Vector3, Vector3*)).MethodHandle;
			}
			for (float num = 0f; num <= magnitude; num += 0.5f * Board.Get().squareSize)
			{
				Vector3 vector = end + num * a;
				BoardSquare boardSquare = Board.Get().GetBoardSquare(vector);
				if (boardSquare != null && boardSquare.IsBaselineHeight())
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
					adjustedEndPoint = vector;
					return true;
				}
			}
			for (;;)
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				adjustedEndPoint = start;
				return true;
			}
		}
		else
		{
			BoardSquare boardSquare3 = Board.Get().GetBoardSquare(end);
			if (boardSquare3 != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.SquaresHaveLoSForAbilities(BoardSquare, BoardSquare, ActorData, bool, List<NonActorTargetInfo>)).MethodHandle;
			}
			if (!(dest == null))
			{
				if (source == dest)
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
					return true;
				}
				if (source.IsBaselineHeight())
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
					if (!source.\u0013(dest.x, dest.y))
					{
						return false;
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
				}
				if (checkBarreirs)
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
					return AreaEffectUtils.HasLosByBarriers(source, dest, caster, VectorUtils.s_laserOffset * Board.SquareSizeStatic, nonActorTargetInfo);
				}
				return true;
			}
		}
		return false;
	}

	public static bool HasLosByBarriers(BoardSquare source, BoardSquare dest, ActorData caster, float offsetToUseInWorld, List<NonActorTargetInfo> nonActorTargetInfo = null)
	{
		bool flag;
		if (BarrierManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.HasLosByBarriers(BoardSquare, BoardSquare, ActorData, float, List<NonActorTargetInfo>)).MethodHandle;
			}
			flag = BarrierManager.Get().HasAbilityBlockingBarriers();
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (!(caster == null))
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
			if (!(BarrierManager.Get() == null))
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
				if (flag2)
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
					if (offsetToUseInWorld <= 0f)
					{
						if (BarrierManager.Get().AreAbilitiesBlocked(caster, source, dest, nonActorTargetInfo))
						{
							return false;
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
					}
				}
			}
		}
		bool flag3 = true;
		List<NonActorTargetInfo> list;
		if (nonActorTargetInfo != null)
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
			list = new List<NonActorTargetInfo>();
		}
		else
		{
			list = null;
		}
		List<NonActorTargetInfo> list2 = list;
		if (BarrierManager.Get() != null)
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
			if (flag2)
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
				Vector3 vector = source.ToVector3();
				Vector3 a = dest.ToVector3();
				Vector3 vector2 = a - vector;
				vector2.y = 0f;
				float magnitude = vector2.magnitude;
				vector2.Normalize();
				Vector3 vector3 = Vector3.Cross(Vector3.up, vector2);
				vector3.Normalize();
				vector3 *= offsetToUseInWorld;
				vector.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
				a.y = vector.y;
				bool flag4 = BarrierManager.Get().AreAbilitiesBlocked(caster, vector + vector3, a + vector3, list2);
				bool flag5 = BarrierManager.Get().AreAbilitiesBlocked(caster, vector - vector3, a - vector3, list2);
				if (flag4 || flag5)
				{
					bool flag6;
					if (!flag4)
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
						RaycastHit raycastHit;
						flag6 = VectorUtils.RaycastInDirection(vector + vector3, vector2, magnitude, out raycastHit);
					}
					else
					{
						flag6 = true;
					}
					bool flag7 = flag6;
					bool flag8;
					if (!flag5)
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
						RaycastHit raycastHit;
						flag8 = VectorUtils.RaycastInDirection(vector - vector3, vector2, magnitude, out raycastHit);
					}
					else
					{
						flag8 = true;
					}
					bool flag9 = flag8;
					if (flag7)
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
						if (flag9)
						{
							flag3 = false;
						}
					}
				}
				if (!flag3)
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
					if (list2 != null)
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
						if (nonActorTargetInfo != null)
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
							nonActorTargetInfo.AddRange(list2);
						}
					}
				}
			}
		}
		return flag3;
	}

	public unsafe static int GetCircleCircleIntersections(Vector3 centerA, Vector3 centerB, float radiusAInSquares, float radiusBInSquares, out Vector3 intersectP1, out Vector3 intersectP2)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetCircleCircleIntersections(Vector3, Vector3, float, float, Vector3*, Vector3*)).MethodHandle;
			}
			return 0;
		}
		if (magnitude < Mathf.Abs(num - num2))
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
			return 0;
		}
		if (magnitude == 0f)
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
			if (num == num2)
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
				return -1;
			}
		}
		int num3;
		if (Mathf.Abs(magnitude - (num + num2)) < 0.001f)
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
			for (int i = 0; i < inAreaCheckers.Count; i++)
			{
				ISquareInsideChecker squareInsideChecker = inAreaCheckers[i];
				bool flag;
				if (squareInsideChecker.IsSquareInside(testSquare, out flag))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.SquareHasLosByAreaCheckers(BoardSquare, List<ISquareInsideChecker>)).MethodHandle;
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
						return true;
					}
				}
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

	public static ActorData GetTargetableActorOnSquare(BoardSquare square, bool allowEnemy, bool allowAlly, ActorData caster)
	{
		ActorData result = null;
		if (square != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetTargetableActorOnSquare(BoardSquare, bool, bool, ActorData)).MethodHandle;
			}
			if (square.OccupantActor != null && AreaEffectUtils.IsActorTargetable(square.OccupantActor, null))
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
				Team team = square.OccupantActor.GetTeam();
				if (caster != null)
				{
					bool flag = team == caster.GetTeam();
					if (flag)
					{
						if (allowAlly)
						{
							goto IL_97;
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
					}
					if (flag)
					{
						goto IL_9E;
					}
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!allowEnemy)
					{
						goto IL_9E;
					}
					IL_97:
					result = square.OccupantActor;
					IL_9E:;
				}
				else
				{
					result = square.OccupantActor;
				}
			}
		}
		return result;
	}

	public static List<BoardSquare> GetSquaresInRadius(BoardSquare centerSquare, float radiusInSquares, bool ignoreLoS, ActorData caster)
	{
		return AreaEffectUtils.GetSquaresInRadius(centerSquare.worldX, centerSquare.worldY, radiusInSquares, ignoreLoS, caster);
	}

	public static List<BoardSquare> GetSquaresInRadius(float centerX, float centerY, float radiusInSquares, bool ignoreLoS, ActorData caster)
	{
		float squareSize = Board.Get().squareSize;
		float num = radiusInSquares * squareSize;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(centerX, centerY);
		List<BoardSquare> list = new List<BoardSquare>();
		if (boardSquareSafe == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetSquaresInRadius(float, float, float, bool, ActorData)).MethodHandle;
			}
			return list;
		}
		Vector3 a = new Vector3(centerX, 0f, centerY);
		int maxX = Board.Get().GetMaxX();
		int maxY = Board.Get().GetMaxY();
		int num2 = (int)Mathf.Max(0f, (centerX - num) / squareSize - 1f);
		int num3 = (int)Mathf.Min((float)maxX, (centerX + num) / squareSize + 1f);
		int num4 = (int)Mathf.Max(0f, (centerY - num) / squareSize - 1f);
		int num5 = (int)Mathf.Min((float)maxY, (centerY + num) / squareSize + 1f);
		for (int i = num2; i < num3; i++)
		{
			for (int j = num4; j < num5; j++)
			{
				Vector3 b = new Vector3((float)i * squareSize, 0f, (float)j * squareSize);
				float sqrMagnitude = (a - b).sqrMagnitude;
				if (sqrMagnitude < num * num)
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
					BoardSquare boardSquare = Board.Get().GetBoardSquare(i, j);
					if (boardSquare != null)
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
						if (boardSquare.height > -Board.Get().BaselineHeight)
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
							bool flag = true;
							if (!ignoreLoS)
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
								flag = AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquareSafe, boardSquare, caster, true, null);
							}
							if (flag)
							{
								list.Add(boardSquare);
							}
						}
					}
				}
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		return list;
	}

	public static bool IsSquareInRadius(BoardSquare testSquare, float centerX, float centerY, float radiusInSquares, bool ignoreLoS, ActorData caster)
	{
		if (testSquare == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsSquareInRadius(BoardSquare, float, float, float, bool, ActorData)).MethodHandle;
			}
			return false;
		}
		bool result = false;
		float squareSize = Board.Get().squareSize;
		float num = radiusInSquares * squareSize;
		int x = testSquare.x;
		int y = testSquare.y;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(centerX, centerY);
		if (boardSquareSafe == null)
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
			return false;
		}
		float worldX = boardSquareSafe.worldX;
		float worldY = boardSquareSafe.worldY;
		Vector3 a = new Vector3(worldX, 0f, worldY);
		Vector3 b = new Vector3((float)x * squareSize, 0f, (float)y * squareSize);
		float sqrMagnitude = (a - b).sqrMagnitude;
		if (sqrMagnitude < num * num)
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
			BoardSquare boardSquare = Board.Get().GetBoardSquare(x, y);
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
				if (boardSquare.height >= Board.Get().BaselineHeight)
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
					bool flag;
					if (!ignoreLoS)
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
						flag = AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquareSafe, boardSquare, caster, true, null);
					}
					else
					{
						flag = true;
					}
					bool flag2 = flag;
					result = flag2;
				}
			}
		}
		return result;
	}

	public static List<ActorData> GetActorsInRadius(Vector3 centerPos, float radiusInSquares, bool ignoreLoS, ActorData caster, Team onlyValidTeam, List<NonActorTargetInfo> nonActorTargetInfo, bool useLosOverridePos = false, Vector3 losOverridePos = default(Vector3))
	{
		return AreaEffectUtils.GetActorsInRadius(centerPos, radiusInSquares, ignoreLoS, caster, new List<Team>
		{
			onlyValidTeam
		}, nonActorTargetInfo, useLosOverridePos, losOverridePos);
	}

	public static List<ActorData> GetActorsInRadius(Vector3 centerPos, float radiusInSquares, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams, List<NonActorTargetInfo> nonActorTargetInfo, bool useLosOverridePos = false, Vector3 losOverridePos = default(Vector3))
	{
		if (GameWideData.Get().UseActorRadiusForCone())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetActorsInRadius(Vector3, float, bool, ActorData, List<Team>, List<NonActorTargetInfo>, bool, Vector3)).MethodHandle;
			}
			return AreaEffectUtils.GetActorsInConeByActorRadius(centerPos, 0f, 360f, radiusInSquares, 0f, ignoreLoS, caster, onlyValidTeams, nonActorTargetInfo, useLosOverridePos, losOverridePos);
		}
		List<ActorData> list = new List<ActorData>();
		List<BoardSquare> squaresInRadius = AreaEffectUtils.GetSquaresInRadius(centerPos.x, centerPos.z, radiusInSquares, ignoreLoS, caster);
		using (List<BoardSquare>.Enumerator enumerator = squaresInRadius.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				ActorData occupantActor = boardSquare.OccupantActor;
				if (AreaEffectUtils.IsActorTargetable(occupantActor, onlyValidTeams))
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
					bool flag = !list.Contains(occupantActor);
					bool flag2 = squaresInRadius.Contains(occupantActor.GetTravelBoardSquare());
					if (flag)
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
						if (flag2)
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
							list.Add(occupantActor);
						}
					}
				}
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
		return list;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsPosInCone(Vector3, Vector3, float, float, float)).MethodHandle;
			}
			float maxAngleWithCenter = 0.5f * coneWidthAngleDeg;
			Vector3 vec = testPos - centerPos;
			if (vec.sqrMagnitude > 0.001f)
			{
				float testAngle = VectorUtils.HorizontalAngle_Deg(vec);
				result = AreaEffectUtils.IsAngleWithinCone(testAngle, coneCenterAngle, maxAngleWithCenter);
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
		int num = Mathf.RoundToInt(coneWidthAngleDeg);
		num = Mathf.Clamp(num, 0, 0x168);
		bool result = false;
		testPos.y = 0f;
		centerPos.y = 0f;
		float maxAngleWithCenter = 0.5f * (float)num;
		Vector3 vec = testPos - centerPos;
		if (vec.sqrMagnitude > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsPosInAngleOfCone(Vector3, Vector3, float, float)).MethodHandle;
			}
			float testAngle = VectorUtils.HorizontalAngle_Deg(vec);
			result = AreaEffectUtils.IsAngleWithinCone(testAngle, coneCenterAngle, maxAngleWithCenter);
		}
		return result;
	}

	public static bool IsAngleWithinCone(float testAngle, float coneCenterAngle, float maxAngleWithCenter)
	{
		float num = coneCenterAngle - maxAngleWithCenter;
		float num2 = coneCenterAngle + maxAngleWithCenter;
		if (testAngle < num)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsAngleWithinCone(float, float, float)).MethodHandle;
			}
			testAngle += 360f;
		}
		else if (testAngle > num2)
		{
			testAngle -= 360f;
		}
		bool result;
		if (num <= testAngle)
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
			result = (testAngle <= num2);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public static List<BoardSquare> GetSquaresInCone(Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, bool ignoreLoS, ActorData caster)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		Vector3 vector = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float d = coneBackwardOffsetInSquares * Board.Get().squareSize;
		Vector3 b = coneStart - vector * d;
		float radiusInSquares = coneLengthRadiusInSquares + coneBackwardOffsetInSquares;
		List<BoardSquare> list2 = null;
		List<BoardSquare> squaresInRadius;
		if (coneBackwardOffsetInSquares == 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetSquaresInCone(Vector3, float, float, float, float, bool, ActorData)).MethodHandle;
			}
			squaresInRadius = AreaEffectUtils.GetSquaresInRadius(coneStart.x, coneStart.z, radiusInSquares, ignoreLoS, caster);
			list2 = squaresInRadius;
		}
		else
		{
			squaresInRadius = AreaEffectUtils.GetSquaresInRadius(b.x, b.z, radiusInSquares, true, caster);
			list2 = AreaEffectUtils.GetSquaresInRadius(coneStart.x, coneStart.z, radiusInSquares, ignoreLoS, caster);
		}
		float num = coneWidthDegrees / 2f;
		foreach (BoardSquare boardSquare in squaresInRadius)
		{
			Vector3 from = boardSquare.ToVector3() - b;
			from.y = 0f;
			if (from.sqrMagnitude != 0f)
			{
				float num2 = Vector3.Angle(from, vector);
				bool flag = num2 <= num;
				bool flag2 = ignoreLoS;
				if (flag && !ignoreLoS)
				{
					bool flag3;
					if (coneBackwardOffsetInSquares != 0f)
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
						flag3 = list2.Contains(boardSquare);
					}
					else
					{
						flag3 = true;
					}
					flag2 = flag3;
				}
				if (flag)
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
					if (flag2)
					{
						list.Add(boardSquare);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetSquaresInConeByActorRadius(Vector3, float, float, float, float, bool, ActorData)).MethodHandle;
			}
			return list;
		}
		int num;
		int num2;
		int num3;
		int num4;
		AreaEffectUtils.GetMaxConeBounds(coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, out num, out num2, out num3, out num4);
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				BoardSquare boardSquare = board.GetBoardSquare(i, j);
				if (boardSquare != null)
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
					if (AreaEffectUtils.IsSquareInConeByActorRadius(boardSquare, coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, ignoreLoS, caster, false, default(Vector3)))
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
						list.Add(boardSquare);
					}
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
		if (AreaEffectUtils.IsSquareInRadius(testSquare, b.x, b.z, radiusInSquares, true, caster))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsSquareInCone(BoardSquare, Vector3, float, float, float, float, bool, ActorData)).MethodHandle;
			}
			if (AreaEffectUtils.IsSquareInRadius(testSquare, coneStart.x, coneStart.z, radiusInSquares, ignoreLoS, caster))
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
				Vector3 vec = testSquare.ToVector3() - b;
				vec.y = 0f;
				if (vec.sqrMagnitude > 0f)
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
					float testAngle = VectorUtils.HorizontalAngle_Deg(vec);
					result = AreaEffectUtils.IsAngleWithinCone(testAngle, coneCenterAngleDegrees, maxAngleWithCenter);
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
		if (AreaEffectUtils.IsPosInCone(vector, vector2, num2, coneCenterAngleDegrees, coneWidthDegrees))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsSquareInConeByActorRadius(BoardSquare, Vector3, float, float, float, float, bool, ActorData, bool, Vector3)).MethodHandle;
			}
			flag = true;
		}
		Vector3 vector3 = vector - vector2;
		vector3.y = 0f;
		float magnitude = vector3.magnitude;
		if (!flag)
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
			if (magnitude > 0f)
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
				float num3 = AreaEffectUtils.GetActorTargetingRadius() * squareSize;
				Vector3 vector4;
				vector4.x = vector3.z;
				vector4.z = -vector3.x;
				vector4.y = 0f;
				vector4.Normalize();
				vector4 *= num3;
				if (magnitude > num2 + num3)
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
					flag = false;
				}
				else
				{
					float num4 = 0.5f * coneWidthDegrees;
					Vector3 a2 = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees - num4);
					Vector3 a3 = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees + num4);
					bool flag2;
					if (!VectorUtils.IsSegmentIntersectingCircle(vector2, vector2 + num2 * a2, vector, num3))
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
						flag2 = VectorUtils.IsSegmentIntersectingCircle(vector2, vector2 + num2 * a3, vector, num3);
					}
					else
					{
						flag2 = true;
					}
					bool flag3 = flag2;
					if (flag3)
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
						flag = true;
					}
					else if (magnitude < Mathf.Abs(num2 - num3))
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
						bool flag4;
						if (!AreaEffectUtils.IsPosInCone(vector + vector4, vector2, num2, coneCenterAngleDegrees, coneWidthDegrees))
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
							flag4 = AreaEffectUtils.IsPosInCone(vector - vector4, vector2, num2, coneCenterAngleDegrees, coneWidthDegrees);
						}
						else
						{
							flag4 = true;
						}
						flag = flag4;
					}
					else if (magnitude > 0f)
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
						Vector3 testPos;
						Vector3 testPos2;
						int circleCircleIntersections = AreaEffectUtils.GetCircleCircleIntersections(vector2, vector, num, AreaEffectUtils.GetActorTargetingRadius(), out testPos, out testPos2);
						if (circleCircleIntersections > 0)
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
							flag = AreaEffectUtils.IsPosInAngleOfCone(testPos, vector2, coneCenterAngleDegrees, coneWidthDegrees);
						}
						if (!flag)
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
							if (circleCircleIntersections > 1)
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
								flag = AreaEffectUtils.IsPosInAngleOfCone(testPos2, vector2, coneCenterAngleDegrees, coneWidthDegrees);
							}
						}
					}
				}
			}
		}
		bool flag5 = true;
		if (flag)
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
			if (!ignoreLoS)
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
				Vector3 vector5;
				if (useLosOverridePos)
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
					vector5 = losOverridePos;
				}
				else
				{
					vector5 = vector2;
				}
				Vector3 vector6 = vector5;
				BoardSquare boardSquare = Board.Get().GetBoardSquare(vector6);
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
					flag5 = AreaEffectUtils.SquareHasLosForCone(vector6, boardSquare, testSquare, caster);
				}
				else
				{
					flag5 = false;
				}
			}
		}
		bool result;
		if (flag)
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
			result = flag5;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public static bool SquareHasLosForCone(Vector3 startPos, BoardSquare centerSquare, BoardSquare testSquare, ActorData caster)
	{
		if (!AreaEffectUtils.SquaresHaveLoSForAbilities(centerSquare, testSquare, caster, true, null))
		{
			return false;
		}
		Vector3 a = centerSquare.ToVector3();
		Vector3 vector = a - startPos;
		vector.y = 0f;
		if (vector.sqrMagnitude > 0.1f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.SquareHasLosForCone(Vector3, BoardSquare, BoardSquare, ActorData)).MethodHandle;
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
			RaycastHit raycastHit;
			bool result;
			if (VectorUtils.RaycastInDirection(vector2, dir, dir.magnitude, out raycastHit))
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
				result = !VectorUtils.RaycastInDirection(vector3, dir2, dir2.magnitude, out raycastHit);
			}
			else
			{
				result = true;
			}
			return result;
		}
		return true;
	}

	public static List<ActorData> GetActorsInCone(Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadius, float coneBackwardOffsetInSquares, bool ignoreLoS, ActorData caster, Team onlyValidTeam, List<NonActorTargetInfo> nonActorHitInfo, bool useLosOverridePos = false, Vector3 losOverridePos = default(Vector3))
	{
		return AreaEffectUtils.GetActorsInCone(coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadius, coneBackwardOffsetInSquares, ignoreLoS, caster, new List<Team>
		{
			onlyValidTeam
		}, nonActorHitInfo, useLosOverridePos, losOverridePos);
	}

	public static List<ActorData> GetActorsInCone(Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams, List<NonActorTargetInfo> nonActorTargetInfo, bool useLosOverridePos = false, Vector3 losOverridePos = default(Vector3))
	{
		if (GameWideData.Get().UseActorRadiusForCone())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetActorsInCone(Vector3, float, float, float, float, bool, ActorData, List<Team>, List<NonActorTargetInfo>, bool, Vector3)).MethodHandle;
			}
			return AreaEffectUtils.GetActorsInConeByActorRadius(coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, ignoreLoS, caster, onlyValidTeams, nonActorTargetInfo, useLosOverridePos, losOverridePos);
		}
		List<ActorData> list = new List<ActorData>();
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (AreaEffectUtils.IsActorTargetable(actorData, onlyValidTeams))
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
				BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
				if (AreaEffectUtils.IsSquareInCone(currentBoardSquare, coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, ignoreLoS, caster))
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
					list.Add(actorData);
				}
			}
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
		return list;
	}

	public static List<ActorData> GetActorsInConeByActorRadius(Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams = null, List<NonActorTargetInfo> nonActorHitInfo = null, bool useLosOverridePos = false, Vector3 losOverridePos = default(Vector3))
	{
		List<ActorData> list = new List<ActorData>();
		List<ActorData> actors = GameFlowData.Get().GetActors();
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (AreaEffectUtils.IsActorTargetable(actorData, onlyValidTeams))
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetActorsInConeByActorRadius(Vector3, float, float, float, float, bool, ActorData, List<Team>, List<NonActorTargetInfo>, bool, Vector3)).MethodHandle;
				}
				BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
				if (AreaEffectUtils.IsSquareInConeByActorRadius(currentBoardSquare, coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, ignoreLoS, caster, useLosOverridePos, losOverridePos))
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
					list.Add(actorData);
				}
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
		return list;
	}

	public unsafe static void GatherStretchConeDimensions(Vector3 aimPos, Vector3 coneStartPos, float minLengthInSquares, float maxLengthInSquares, float minWidthDegrees, float maxWidthDegrees, AreaEffectUtils.StretchConeStyle style, out float lengthInSquares, out float angleInDegrees, bool widthChangeDiscrete = false, int numWidthDiscreteChanges = 0, float interpMinDistOverrideInSquares = -1f, float interpRangeOverrideInSquares = -1f)
	{
		float num = maxLengthInSquares - minLengthInSquares;
		float num2 = maxWidthDegrees - minWidthDegrees;
		float num3 = maxLengthInSquares;
		float num4 = minLengthInSquares;
		if (interpMinDistOverrideInSquares > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GatherStretchConeDimensions(Vector3, Vector3, float, float, float, float, AreaEffectUtils.StretchConeStyle, float*, float*, bool, int, float, float)).MethodHandle;
			}
			if (interpRangeOverrideInSquares > 0f)
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
				num4 = interpMinDistOverrideInSquares;
				num3 = interpMinDistOverrideInSquares + interpRangeOverrideInSquares;
				num = interpRangeOverrideInSquares;
				goto IL_75;
			}
		}
		if (num <= 0f)
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
			num3 = Mathf.Max(3f, maxLengthInSquares);
			num4 = 2.5f;
			num = num3 - num4;
		}
		IL_75:
		if (num <= 0f)
		{
			lengthInSquares = maxLengthInSquares;
			angleInDegrees = maxWidthDegrees;
		}
		else
		{
			Vector3 vector = aimPos - coneStartPos;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			float value = magnitude / Board.Get().squareSize;
			lengthInSquares = Mathf.Clamp(value, minLengthInSquares, maxLengthInSquares);
			float num5 = Mathf.Clamp(value, num4, num3);
			float num6 = (num5 - num4) / num;
			float num7;
			if (style == AreaEffectUtils.StretchConeStyle.Linear)
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
				num7 = num6;
			}
			else if (style == AreaEffectUtils.StretchConeStyle.DistanceSquared)
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
				num7 = num6 * num6;
			}
			else if (style == AreaEffectUtils.StretchConeStyle.DistanceCubed)
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
				num7 = num6 * num6 * num6;
			}
			else if (style == AreaEffectUtils.StretchConeStyle.DistanceToTheFourthPower)
			{
				num7 = num6 * num6 * num6 * num6;
			}
			else if (style == AreaEffectUtils.StretchConeStyle.DistanceSquareRoot)
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
				for (;;)
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
					for (;;)
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
						for (;;)
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
	}

	private static void AdjustMinMaxBounds(Vector3 point, ref Vector3 minBounds, ref Vector3 maxBounds)
	{
		minBounds.x = Mathf.Min(point.x, minBounds.x);
		minBounds.z = Mathf.Min(point.z, minBounds.z);
		maxBounds.x = Mathf.Max(point.x, maxBounds.x);
		maxBounds.z = Mathf.Max(point.z, maxBounds.z);
	}

	public unsafe static bool GetMaxConeBounds(Vector3 coneStart, float coneCenterAngleDegrees, float coneWidthDegrees, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares, out int minX, out int maxX, out int minY, out int maxY)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetMaxConeBounds(Vector3, float, float, float, float, int*, int*, int*, int*)).MethodHandle;
			}
			minX = 0;
			maxX = 0;
			minY = 0;
			maxY = 0;
			return false;
		}
		int maxX2 = board.GetMaxX();
		int maxY2 = board.GetMaxY();
		float num3 = 0.5f * coneWidthDegrees;
		float num4 = coneCenterAngleDegrees - num3;
		float num5 = coneCenterAngleDegrees + num3;
		Vector3 vector2 = vector;
		Vector3 vector3 = vector;
		float num6 = AreaEffectUtils.GetActorTargetingRadius() * board.squareSize;
		float d2 = num2 + num6;
		Vector3 point = vector + d2 * VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees + num3);
		Vector3 point2 = vector + d2 * VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees - num3);
		AreaEffectUtils.AdjustMinMaxBounds(point, ref vector2, ref vector3);
		AreaEffectUtils.AdjustMinMaxBounds(point2, ref vector2, ref vector3);
		for (int i = -0x5A; i <= 0x1C2; i += 0x5A)
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
			if (num5 <= (float)i)
			{
				break;
			}
			if (num4 < (float)i)
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
				if ((float)i < num5)
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
					Vector3 point3 = vector + d2 * VectorUtils.AngleDegreesToVector((float)i);
					AreaEffectUtils.AdjustMinMaxBounds(point3, ref vector2, ref vector3);
				}
			}
		}
		minX = (int)Mathf.Max(0f, vector2.x / squareSize - 2f);
		maxX = (int)Mathf.Min((float)maxX2, vector3.x / squareSize + 2f);
		minY = (int)Mathf.Max(0f, vector2.z / squareSize - 2f);
		maxY = (int)Mathf.Min((float)maxY2, vector3.z / squareSize + 2f);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.OperateOnSquaresInCone(IOperationOnSquare, Vector3, float, float, float, float, ActorData, bool, List<ISquareInsideChecker>)).MethodHandle;
			}
			return;
		}
		int num;
		int num2;
		int num3;
		int num4;
		AreaEffectUtils.GetMaxConeBounds(coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, out num, out num2, out num3, out num4);
		bool flag = false;
		for (int i = num; i < num2; i++)
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
			if (flag)
			{
				break;
			}
			int num5 = num3;
			while (num5 < num4 && !flag)
			{
				BoardSquare boardSquare = board.GetBoardSquare(i, num5);
				if (boardSquare != null && boardSquare.IsBaselineHeight())
				{
					bool flag2 = AreaEffectUtils.IsSquareInConeByActorRadius(boardSquare, coneStart, coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, coneBackwardOffsetInSquares, true, caster, false, default(Vector3));
					if (flag2)
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
						bool squareHasLos;
						if (ignoreLos)
						{
							squareHasLos = true;
						}
						else if (losCheckOverrides != null)
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
							squareHasLos = AreaEffectUtils.SquareHasLosByAreaCheckers(boardSquare, losCheckOverrides);
						}
						else
						{
							squareHasLos = AreaEffectUtils.SquareHasLosForCone(startPos, boardSquareSafe, boardSquare, caster);
						}
						operationObj.OperateOnSquare(boardSquare, caster, squareHasLos);
						flag = operationObj.ShouldEarlyOut();
					}
				}
				num5++;
			}
		}
	}

	public static List<ActorData> GetActorsInRadiusOfLine(Vector3 startPos, Vector3 endPos, float startRadiusInSquares, float endRadiusInSquares, float rangeFromLineInSquares, bool ignoreLoS, ActorData caster, Team onlyValidTeam, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return AreaEffectUtils.GetActorsInRadiusOfLine(startPos, endPos, startRadiusInSquares, endRadiusInSquares, rangeFromLineInSquares, ignoreLoS, caster, new List<Team>
		{
			onlyValidTeam
		}, nonActorTargetInfo);
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetActorsInRadiusOfLine(Vector3, Vector3, float, float, float, bool, ActorData, List<Team>, List<NonActorTargetInfo>)).MethodHandle;
				}
				list3 = AreaEffectUtils.GetActorsInBoxByActorRadius(startPos, endPos, num, ignoreLoS, caster, onlyValidTeams, list2, null);
			}
			else
			{
				list3 = AreaEffectUtils.GetActorsInBox(startPos, endPos, num, list2, caster, onlyValidTeams);
			}
			List<ActorData> list4 = list3;
			using (List<ActorData>.Enumerator enumerator = list4.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData item = enumerator.Current;
					if (!list.Contains(item))
					{
						list.Add(item);
					}
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
		}
		if (startRadiusInSquares > 0f)
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
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(startPos, startRadiusInSquares, ignoreLoS, caster, onlyValidTeams, nonActorTargetInfo, false, default(Vector3));
			foreach (ActorData item2 in actorsInRadius)
			{
				if (!list.Contains(item2))
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
					list.Add(item2);
				}
			}
		}
		if (endRadiusInSquares > 0f)
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
			List<ActorData> actorsInRadius2 = AreaEffectUtils.GetActorsInRadius(endPos, endRadiusInSquares, ignoreLoS, caster, onlyValidTeams, nonActorTargetInfo, false, default(Vector3));
			using (List<ActorData>.Enumerator enumerator3 = actorsInRadius2.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					ActorData item3 = enumerator3.Current;
					if (!list.Contains(item3))
					{
						list.Add(item3);
					}
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
		}
		return list;
	}

	public static void OperateOnSquaresInRadiusOfLine(IOperationOnSquare operationObj, Vector3 startPos, Vector3 endPos, float startRadiusInSquares, float endRadiusInSquares, float rangeFromLineInSquares, bool ignoreLoS, ActorData caster)
	{
		if (AreaEffectUtils.s_radiusOfLineLosCheckers.Count == 0)
		{
			AreaEffectUtils.s_radiusOfLineLosCheckers.Add(new SquareInsideChecker_Box(1f));
			AreaEffectUtils.s_radiusOfLineLosCheckers.Add(new SquareInsideChecker_Cone());
			AreaEffectUtils.s_radiusOfLineLosCheckers.Add(new SquareInsideChecker_Cone());
		}
		List<Vector3> list = new List<Vector3>();
		if (!ignoreLoS)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.OperateOnSquaresInRadiusOfLine(IOperationOnSquare, Vector3, Vector3, float, float, float, bool, ActorData)).MethodHandle;
			}
			list.Add(startPos);
			list.Add((startPos + endPos) * 0.5f);
			list.Add(endPos);
		}
		SquareInsideChecker_Box squareInsideChecker_Box = AreaEffectUtils.s_radiusOfLineLosCheckers[0] as SquareInsideChecker_Box;
		SquareInsideChecker_Cone squareInsideChecker_Cone = AreaEffectUtils.s_radiusOfLineLosCheckers[1] as SquareInsideChecker_Cone;
		SquareInsideChecker_Cone squareInsideChecker_Cone2 = AreaEffectUtils.s_radiusOfLineLosCheckers[2] as SquareInsideChecker_Cone;
		float widthInSquares = rangeFromLineInSquares * 2f;
		squareInsideChecker_Box.UpdateBoxProperties(startPos, endPos, caster);
		squareInsideChecker_Box.m_penetrateLos = ignoreLoS;
		squareInsideChecker_Box.m_widthInSquares = widthInSquares;
		squareInsideChecker_Box.m_additionalLosSources = list;
		squareInsideChecker_Cone.UpdateConeProperties(startPos, 360f, startRadiusInSquares, 0f, 0f, caster);
		squareInsideChecker_Cone2.UpdateConeProperties(endPos, 360f, endRadiusInSquares, 0f, 0f, caster);
		if (rangeFromLineInSquares > 0f)
		{
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(operationObj, startPos, endPos, widthInSquares, caster, ignoreLoS, list, AreaEffectUtils.s_radiusOfLineLosCheckers, true);
		}
		if (startRadiusInSquares > 0f)
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
			AreaEffectUtils.OperateOnSquaresInCone(operationObj, startPos, 0f, 360f, startRadiusInSquares, 0f, caster, ignoreLoS, AreaEffectUtils.s_radiusOfLineLosCheckers);
		}
		if (endRadiusInSquares > 0f)
		{
			AreaEffectUtils.OperateOnSquaresInCone(operationObj, endPos, 0f, 360f, endRadiusInSquares, 0f, caster, ignoreLoS, AreaEffectUtils.s_radiusOfLineLosCheckers);
		}
	}

	public static List<BoardSquare> GetSquaresInBox(Vector3 pt1, Vector3 pt2, float halfWidthInSquares, bool ignoreLoS, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		if (!ignoreLoS)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetSquaresInBox(Vector3, Vector3, float, bool, ActorData)).MethodHandle;
			}
			list.Add(pt1);
		}
		return AreaEffectUtils.GetSquaresInBox(pt1, pt2, halfWidthInSquares, list, caster);
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
				if (AreaEffectUtils.PointInBox(testPt, pt1, pt2, num))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetSquaresInBox(Vector3, Vector3, float, List<Vector3>, ActorData)).MethodHandle;
					}
					if (losCheckPoints == null)
					{
						goto IL_1E3;
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
					if (losCheckPoints.Count <= 0)
					{
						goto IL_1E3;
					}
					bool flag = false;
					foreach (Vector3 vector2D in losCheckPoints)
					{
						BoardSquare boardSquare2 = Board.Get().GetBoardSquare(vector2D);
						if (boardSquare2 != null)
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
							bool flag2 = AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquare2, boardSquare, caster, true, null);
							if (flag2)
							{
								flag = true;
								break;
							}
						}
					}
					IL_1E6:
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
						list.Add(boardSquare);
						goto IL_1FD;
					}
					goto IL_1FD;
					IL_1E3:
					flag = true;
					goto IL_1E6;
				}
				IL_1FD:;
			}
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

	public static List<ActorData> GetActorsInBox(Vector3 startPos, Vector3 endPos, float boxWidthInSquares, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams)
	{
		List<Vector3> list = new List<Vector3>();
		if (!ignoreLoS)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetActorsInBox(Vector3, Vector3, float, bool, ActorData, List<Team>)).MethodHandle;
			}
			list.Add(startPos);
		}
		return AreaEffectUtils.GetActorsInBox(startPos, endPos, boxWidthInSquares, list, caster, onlyValidTeams);
	}

	public static List<ActorData> GetActorsInBox(Vector3 startPos, Vector3 endPos, float boxWidthInSquares, List<Vector3> losCheckPoints, ActorData caster, List<Team> onlyValidTeams)
	{
		List<ActorData> list = new List<ActorData>();
		if (boxWidthInSquares > 0f)
		{
			List<BoardSquare> squaresInBox = AreaEffectUtils.GetSquaresInBox(startPos, endPos, boxWidthInSquares / 2f, losCheckPoints, caster);
			using (List<BoardSquare>.Enumerator enumerator = squaresInBox.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoardSquare boardSquare = enumerator.Current;
					ActorData occupantActor = boardSquare.OccupantActor;
					if (AreaEffectUtils.IsActorTargetable(occupantActor, onlyValidTeams))
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
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetActorsInBox(Vector3, Vector3, float, List<Vector3>, ActorData, List<Team>)).MethodHandle;
						}
						if (!list.Contains(occupantActor))
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
							list.Add(occupantActor);
						}
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
		if (!VectorUtils.IsSegmentIntersectingCircle(ptA, ptB, circleCenter, radiusInWorld))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsBoxBorderTouchingCircle(Vector3, Vector3, Vector3, Vector3, Vector3, float)).MethodHandle;
			}
			if (!VectorUtils.IsSegmentIntersectingCircle(ptB, ptD, circleCenter, radiusInWorld) && !VectorUtils.IsSegmentIntersectingCircle(ptD, ptC, circleCenter, radiusInWorld))
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
				return VectorUtils.IsSegmentIntersectingCircle(ptC, ptA, circleCenter, radiusInWorld);
			}
		}
		return true;
	}

	public static List<BoardSquare> GetSquaresInBoxByActorRadius(Vector3 startPos, Vector3 endPos, float laserWidthInSquares, bool penetrateLos, ActorData caster, List<Vector3> additionalLosSources = null)
	{
		startPos.y = 0f;
		endPos.y = 0f;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(startPos);
		float num = AreaEffectUtils.GetActorTargetingRadius() * Board.Get().squareSize;
		float num2 = laserWidthInSquares * Board.Get().squareSize;
		float num3 = 0.5f * num2;
		Vector3 ptA;
		Vector3 ptB;
		Vector3 ptC;
		Vector3 ptD;
		AreaEffectUtils.GetBoxCorners(startPos, endPos, laserWidthInSquares, out ptA, out ptB, out ptC, out ptD);
		float adjustAmount = num3 + num;
		int num4;
		int num5;
		int num6;
		int num7;
		AreaEffectUtils.GetBoxBoundsInGridPos(startPos, endPos, adjustAmount, out num4, out num5, out num6, out num7);
		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = num4; i <= num6; i++)
		{
			for (int j = num5; j <= num7; j++)
			{
				BoardSquare boardSquare2 = Board.Get().GetBoardSquare(i, j);
				if (boardSquare2 == null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetSquaresInBoxByActorRadius(Vector3, Vector3, float, bool, ActorData, List<Vector3>)).MethodHandle;
					}
				}
				else
				{
					Vector3 vector = boardSquare2.ToVector3();
					vector.y = 0f;
					bool flag;
					if (!AreaEffectUtils.PointInBox(vector, startPos, endPos, num3))
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
						flag = AreaEffectUtils.IsBoxBorderTouchingCircle(ptA, ptB, ptC, ptD, vector, num);
					}
					else
					{
						flag = true;
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
					}
					else if (penetrateLos)
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
						list.Add(boardSquare2);
					}
					else if (AreaEffectUtils.HasLosForLaserByActorRadius(boardSquare, boardSquare2, startPos, endPos, laserWidthInSquares, caster, null))
					{
						list.Add(boardSquare2);
					}
					else if (additionalLosSources != null)
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
						using (List<Vector3>.Enumerator enumerator = additionalLosSources.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Vector3 vector2D = enumerator.Current;
								BoardSquare boardSquare3 = Board.Get().GetBoardSquare(vector2D);
								if (boardSquare3 != null)
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
									if (AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquare3, boardSquare2, caster, true, null))
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
										list.Add(boardSquare2);
										goto IL_1F5;
									}
								}
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
						}
					}
				}
				IL_1F5:;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
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
		return list;
	}

	public static bool IsSquareInBoxByActorRadius(BoardSquare testSquare, Vector3 startPos, Vector3 endPos, float laserWidthInSquares)
	{
		if (testSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsSquareInBoxByActorRadius(BoardSquare, Vector3, Vector3, float)).MethodHandle;
			}
			startPos.y = 0f;
			endPos.y = 0f;
			Vector3 vector = testSquare.ToVector3();
			vector.y = 0f;
			Vector3 ptA;
			Vector3 ptB;
			Vector3 ptC;
			Vector3 ptD;
			AreaEffectUtils.GetBoxCorners(startPos, endPos, laserWidthInSquares, out ptA, out ptB, out ptC, out ptD);
			float radiusInWorld = AreaEffectUtils.GetActorTargetingRadius() * Board.Get().squareSize;
			float num = laserWidthInSquares * Board.Get().squareSize;
			float halfWidth = 0.5f * num;
			return AreaEffectUtils.PointInBox(vector, startPos, endPos, halfWidth) || AreaEffectUtils.IsBoxBorderTouchingCircle(ptA, ptB, ptC, ptD, vector, radiusInWorld);
		}
		return false;
	}

	public static bool IsSquareInLosForBox(BoardSquare square, Vector3 startPos, Vector3 endPos, float laserWidthInSquares, bool penetrateLos, ActorData caster, List<Vector3> additionalLosSources = null)
	{
		BoardSquare boardSquare = Board.Get().GetBoardSquare(startPos);
		bool result = false;
		if (penetrateLos)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsSquareInLosForBox(BoardSquare, Vector3, Vector3, float, bool, ActorData, List<Vector3>)).MethodHandle;
			}
			result = true;
		}
		else if (AreaEffectUtils.HasLosForLaserByActorRadius(boardSquare, square, startPos, endPos, laserWidthInSquares, caster, null))
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
			result = true;
		}
		else if (additionalLosSources != null)
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
			using (List<Vector3>.Enumerator enumerator = additionalLosSources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Vector3 vector2D = enumerator.Current;
					BoardSquare boardSquare2 = Board.Get().GetBoardSquare(vector2D);
					if (boardSquare2 != null)
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
						if (AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquare2, square, caster, true, null))
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
							return true;
						}
					}
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
			float radiusInWorld = AreaEffectUtils.GetActorTargetingRadius() * Board.Get().squareSize;
			float num = laserWidthInSquares * Board.Get().squareSize;
			float halfWidth = 0.5f * num;
			Vector3 ptA;
			Vector3 ptB;
			Vector3 ptC;
			Vector3 ptD;
			AreaEffectUtils.GetBoxCorners(startPos, endPos, laserWidthInSquares, out ptA, out ptB, out ptC, out ptD);
			List<ActorData> actors = GameFlowData.Get().GetActors();
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				IL_1F2:
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					if (AreaEffectUtils.IsActorTargetable(actorData, validTeams))
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetActorsInBoxByActorRadius(Vector3, Vector3, float, bool, ActorData, List<Team>, List<Vector3>, List<NonActorTargetInfo>)).MethodHandle;
						}
						Vector3 travelBoardSquareWorldPosition = actorData.GetTravelBoardSquareWorldPosition();
						travelBoardSquareWorldPosition.y = 0f;
						bool flag;
						if (!AreaEffectUtils.PointInBox(travelBoardSquareWorldPosition, startPos, endPos, halfWidth))
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
							flag = AreaEffectUtils.IsBoxBorderTouchingCircle(ptA, ptB, ptC, ptD, travelBoardSquareWorldPosition, radiusInWorld);
						}
						else
						{
							flag = true;
						}
						if (!flag)
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
						}
						else if (penetrateLos)
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
							list.Add(actorData);
						}
						else
						{
							BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
							if (AreaEffectUtils.HasLosForLaserByActorRadius(boardSquare, currentBoardSquare, startPos, endPos, laserWidthInSquares, caster, nonActorTargetInfo))
							{
								list.Add(actorData);
							}
							else if (additionalLosSources != null)
							{
								using (List<Vector3>.Enumerator enumerator2 = additionalLosSources.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										Vector3 vector2D = enumerator2.Current;
										BoardSquare boardSquare2 = Board.Get().GetBoardSquare(vector2D);
										if (boardSquare2 != null)
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
											if (AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquare2, currentBoardSquare, caster, true, null))
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
												list.Add(actorData);
												goto IL_1F2;
											}
										}
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
								}
							}
						}
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
		(vector2 - vector3).y = 0f;
		float squareSize = Board.Get().squareSize;
		bool flag = AreaEffectUtils.SquaresHaveLoSForAbilities(startSquare, testSquare, caster, false, null);
		List<NonActorTargetInfo> list;
		if (nonActorTargetInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.HasLosForLaserByActorRadius(BoardSquare, BoardSquare, Vector3, Vector3, float, ActorData, List<NonActorTargetInfo>)).MethodHandle;
			}
			list = new List<NonActorTargetInfo>();
		}
		else
		{
			list = null;
		}
		List<NonActorTargetInfo> list2 = list;
		bool flag2 = AreaEffectUtils.HasLosByBarriers(startSquare, testSquare, caster, VectorUtils.s_laserOffset * Board.SquareSizeStatic, list2);
		float num = 0.72f;
		if (caster.GetCurrentBoardSquare() == startSquare)
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
			num += GameWideData.Get().m_actorTargetingRadiusInSquares;
		}
		Vector3 a = vector2 - num * squareSize * normalized;
		Vector3 lhs = a - laserStart;
		lhs.y = 0f;
		Vector3 vector4 = laserStart + Vector3.Dot(lhs, normalized) * normalized;
		if (Vector3.Dot(normalized, vector4 - laserStart) <= 0f)
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
			vector4 = vector3;
		}
		bool flag3 = true;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(vector4);
		if (boardSquare != null)
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
			if (boardSquare != testSquare)
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
				if (boardSquare.IsBaselineHeight())
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
					flag3 = boardSquare.\u0013(testSquare.x, testSquare.y);
					if (flag2)
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
						flag2 = AreaEffectUtils.HasLosByBarriers(boardSquare, testSquare, caster, VectorUtils.s_laserOffset * Board.SquareSizeStatic, list2);
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
		if (Mathf.Abs(normalized.x) > Mathf.Abs(normalized.z))
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
			zero = new Vector3(0f, 0f, num2);
		}
		else
		{
			zero = new Vector3(num2, 0f, 0f);
		}
		bool flag4 = !flag2;
		if (flag)
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
			if (flag4)
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
				if (nonActorTargetInfo != null)
				{
					nonActorTargetInfo.AddRange(list2);
				}
			}
		}
		if (!flag4 && flag3)
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
				Vector3 a2 = vector2;
				a2.y = travelBoardSquareWorldPositionForLos.y;
				Vector3 dir = a2 - vector3;
				dir.y = 0f;
				RaycastHit raycastHit;
				bool flag5 = VectorUtils.RaycastInDirection(vector3 + zero, dir, dir.magnitude, out raycastHit);
				bool flag6;
				if (flag5)
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
					flag6 = VectorUtils.RaycastInDirection(vector3 - zero, dir, dir.magnitude, out raycastHit);
				}
				else
				{
					flag6 = false;
				}
				bool flag7 = flag6;
				flag = (!flag5 || !flag7);
			}
		}
		if (!flag4)
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
			if (flag)
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
				return flag3;
			}
		}
		return false;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(IOperationOnSquare, Vector3, Vector3, float, ActorData, bool, List<Vector3>, List<ISquareInsideChecker>, bool)).MethodHandle;
				}
				startPos = VectorUtils.GetAdjustedStartPosWithOffset(startPos, endPos, actorTargetingRadiusInSquares);
			}
		}
		float num = AreaEffectUtils.GetActorTargetingRadius() * Board.Get().squareSize;
		float num2 = widthInSquares * Board.Get().squareSize;
		float num3 = 0.5f * num2;
		Vector3 ptA;
		Vector3 ptB;
		Vector3 ptC;
		Vector3 ptD;
		AreaEffectUtils.GetBoxCorners(startPos, endPos, widthInSquares, out ptA, out ptB, out ptC, out ptD);
		float adjustAmount = num3 + num;
		int num4;
		int num5;
		int num6;
		int num7;
		AreaEffectUtils.GetBoxBoundsInGridPos(startPos, endPos, adjustAmount, out num4, out num5, out num6, out num7);
		for (int i = num4; i <= num6; i++)
		{
			for (int j = num5; j <= num7; j++)
			{
				BoardSquare boardSquare2 = Board.Get().GetBoardSquare(i, j);
				if (!(boardSquare2 == null))
				{
					Vector3 vector = boardSquare2.ToVector3();
					vector.y = 0f;
					bool flag;
					if (!AreaEffectUtils.PointInBox(vector, startPos, endPos, num3))
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
						flag = AreaEffectUtils.IsBoxBorderTouchingCircle(ptA, ptB, ptC, ptD, vector, num);
					}
					else
					{
						flag = true;
					}
					if (!flag)
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
					}
					else
					{
						bool flag2 = false;
						if (ignoreLos)
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
							flag2 = true;
						}
						else if (losCheckOverrides != null)
						{
							flag2 = AreaEffectUtils.SquareHasLosByAreaCheckers(boardSquare2, losCheckOverrides);
						}
						else
						{
							flag2 = AreaEffectUtils.HasLosForLaserByActorRadius(boardSquare, boardSquare2, startPos, endPos, widthInSquares, caster, null);
							if (!flag2 && additionalLosSources != null)
							{
								using (List<Vector3>.Enumerator enumerator = additionalLosSources.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										Vector3 vector2D = enumerator.Current;
										BoardSquare boardSquare3 = Board.Get().GetBoardSquare(vector2D);
										if (boardSquare3 != null)
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
											if (AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquare3, boardSquare2, caster, true, null))
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
												flag2 = true;
												goto IL_217;
											}
										}
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
							}
						}
						IL_217:
						operationObj.OperateOnSquare(boardSquare2, caster, flag2);
					}
				}
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

	public unsafe static List<ActorData> GetActorsInLaser(Vector3 startPos, Vector3 dir, float laserRangeInSquares, float laserWidthInSquares, ActorData caster, List<Team> validTeams, bool penetrateLos, int maxTargets, bool lengthIgnoreLevelGeo, bool includeInvisibles, out Vector3 laserEndPos, List<NonActorTargetInfo> nonActorTargets, List<ActorData> actorsToExclude = null, bool ignoreStartOffset = false, bool excludeCaster = true)
	{
		dir.y = 0f;
		dir.Normalize();
		float maxDistanceInWorld = laserRangeInSquares * Board.Get().squareSize;
		bool flag;
		if (!penetrateLos)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetActorsInLaser(Vector3, Vector3, float, float, ActorData, List<Team>, bool, int, bool, bool, Vector3*, List<NonActorTargetInfo>, List<ActorData>, bool, bool)).MethodHandle;
			}
			flag = lengthIgnoreLevelGeo;
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		List<NonActorTargetInfo> list;
		if (nonActorTargets != null)
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
			list = new List<NonActorTargetInfo>();
		}
		else
		{
			list = null;
		}
		List<NonActorTargetInfo> list2 = list;
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = startPos;
		bool checkBarriers = laserWidthInSquares > 0f;
		laserCoords.end = VectorUtils.GetLaserEndPoint(startPos, dir, maxDistanceInWorld, penetrateLos, caster, list2, checkBarriers);
		if (flag2)
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
			if (!penetrateLos)
			{
				laserCoords.end = VectorUtils.GetLaserEndPoint(startPos, dir, maxDistanceInWorld, flag2, caster, null, checkBarriers);
			}
		}
		if (!penetrateLos)
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
			if (laserWidthInSquares > 2f)
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
				if (list2 != null)
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
					if (list2.Count == 0)
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
						Vector3 vector = Vector3.Cross(Vector3.up, dir);
						vector.Normalize();
						vector *= 0.5f * laserWidthInSquares * Board.SquareSizeStatic;
						Vector3 startPos2 = startPos + vector;
						Vector3 startPos3 = startPos - vector;
						float magnitude = (laserCoords.end - laserCoords.start).magnitude;
						VectorUtils.GetLaserEndPoint(startPos2, dir, magnitude, penetrateLos, caster, list2, checkBarriers);
						if (list2.Count == 0)
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
							VectorUtils.GetLaserEndPoint(startPos3, dir, magnitude, penetrateLos, caster, list2, checkBarriers);
						}
					}
				}
			}
		}
		float actorTargetingRadiusInSquares = GameWideData.Get().m_actorTargetingRadiusInSquares;
		bool flag3 = GameWideData.Get().UseActorRadiusForLaser();
		if (actorTargetingRadiusInSquares > 0f)
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
			if (!ignoreStartOffset)
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
				laserCoords.start = VectorUtils.GetAdjustedStartPosWithOffset(laserCoords.start, laserCoords.end, actorTargetingRadiusInSquares);
			}
		}
		List<ActorData> list3;
		if (flag3)
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
			list3 = AreaEffectUtils.GetActorsInBoxByActorRadius(laserCoords.start, laserCoords.end, laserWidthInSquares, penetrateLos, caster, validTeams, null, nonActorTargets);
		}
		else
		{
			list3 = AreaEffectUtils.GetActorsInBox(laserCoords.start, laserCoords.end, laserWidthInSquares, penetrateLos, caster, validTeams);
		}
		List<ActorData> list4 = list3;
		if (!includeInvisibles)
		{
			TargeterUtils.RemoveActorsInvisibleToClient(ref list4);
		}
		if (actorsToExclude != null)
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
			for (int i = 0; i < actorsToExclude.Count; i++)
			{
				if (list4.Contains(actorsToExclude[i]))
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
					list4.Remove(actorsToExclude[i]);
				}
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
		if (excludeCaster)
		{
			list4.Remove(caster);
		}
		TargeterUtils.SortActorsByDistanceToPos(ref list4, startPos, dir);
		TargeterUtils.LimitActorsToMaxNumber(ref list4, maxTargets);
		if (maxTargets > 0)
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
			if (list4.Count == maxTargets)
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
				ActorData actorData = list4[list4.Count - 1];
				Vector3 lhs = actorData.GetTravelBoardSquareWorldPosition() - laserCoords.start;
				lhs.y = 0f;
				Vector3 b = Vector3.Dot(lhs, dir) * dir;
				laserEndPos = laserCoords.start + b;
				return list4;
			}
		}
		laserEndPos = laserCoords.end;
		if (nonActorTargets != null && list2 != null)
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
			for (int j = 0; j < list2.Count; j++)
			{
				nonActorTargets.Add(list2[j]);
			}
		}
		return list4;
	}

	public static bool LaserHitWorldGeo(float maxDistanceInSquares, VectorUtils.LaserCoords adjustedCoords, bool penetrateLos, List<ActorData> actorsHit)
	{
		if (!penetrateLos)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.LaserHitWorldGeo(float, VectorUtils.LaserCoords, bool, List<ActorData>)).MethodHandle;
			}
			if (actorsHit == null || actorsHit.Count <= 0)
			{
				bool result = false;
				float num = maxDistanceInSquares * Board.Get().squareSize;
				if (adjustedCoords.Length() < num - 0.1f)
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
					result = true;
				}
				return result;
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
		}
		return false;
	}

	public unsafe static Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> FindBouncingLaserTargets(Vector3 originalStart, ref List<Vector3> laserAnglePoints, float widthInSquares, List<Team> validTeamsToHit, int maxTargetsHit, bool includeInvisibles, ActorData caster, List<ActorData> orderedHitActors = null, bool includeActorsAtAnglePoints = false)
	{
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
		if (orderedHitActors != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.FindBouncingLaserTargets(Vector3, List<Vector3>*, float, List<Team>, int, bool, ActorData, List<ActorData>, bool)).MethodHandle;
			}
			orderedHitActors.Clear();
		}
		Vector3 vector = originalStart;
		int num = 0;
		int num2 = -1;
		Vector3 value = originalStart;
		int i = 0;
		IL_211:
		while (i < laserAnglePoints.Count)
		{
			Vector3 vector2 = laserAnglePoints[i];
			List<ActorData> list;
			if (GameWideData.Get().UseActorRadiusForLaser())
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
				list = AreaEffectUtils.GetActorsInBoxByActorRadius(vector, vector2, widthInSquares, false, caster, validTeamsToHit, null, null);
			}
			else
			{
				list = AreaEffectUtils.GetActorsInBox(vector, vector2, widthInSquares, true, caster, validTeamsToHit);
			}
			List<ActorData> list2 = list;
			list2.Remove(caster);
			if (includeActorsAtAnglePoints)
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
				BoardSquare boardSquare = Board.Get().GetBoardSquare(vector2);
				ActorData occupantActor = boardSquare.OccupantActor;
				if (occupantActor != null)
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
					if (!list2.Contains(occupantActor) && AreaEffectUtils.IsRelevantTeam(validTeamsToHit, occupantActor.GetTeam()))
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
						list2.Add(occupantActor);
					}
				}
			}
			TargeterUtils.SortActorsByDistanceToPos(ref list2, vector);
			for (int j = 0; j < list2.Count; j++)
			{
				ActorData actorData = list2[j];
				if (!dictionary.ContainsKey(actorData))
				{
					if (!includeInvisibles && !actorData.IsVisibleToClient())
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
					}
					else
					{
						AreaEffectUtils.BouncingLaserInfo value2 = new AreaEffectUtils.BouncingLaserInfo(vector, i);
						dictionary.Add(actorData, value2);
						if (orderedHitActors != null)
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
							orderedHitActors.Add(actorData);
						}
						num++;
						if (num >= maxTargetsHit)
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
							if (maxTargetsHit > 0)
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
								num2 = i;
								Vector3 normalized = (vector2 - vector).normalized;
								Vector3 rhs = actorData.GetTravelBoardSquareWorldPosition() - vector;
								value = vector + Vector3.Dot(normalized, rhs) * normalized;
								IL_1F8:
								if (num2 != -1)
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
									goto IL_22B;
								}
								vector = vector2;
								i++;
								goto IL_211;
							}
						}
					}
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				goto IL_1F8;
			}
			IL_22B:
			if (num2 != -1 && maxTargetsHit > 0)
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
				laserAnglePoints[num2] = value;
				int num3 = laserAnglePoints.Count - 1 - num2;
				if (num3 > 0)
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
					laserAnglePoints.RemoveRange(num2 + 1, num3);
				}
			}
			return dictionary;
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			goto IL_22B;
		}
	}

	public static void OperateOnSquaresInBounceLaser(IOperationOnSquare operationObj, Vector3 originalStart, List<Vector3> laserAnglePoints, float widthInSquares, ActorData caster, bool ignoreLos)
	{
		if (laserAnglePoints.Count > 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.OperateOnSquaresInBounceLaser(IOperationOnSquare, Vector3, List<Vector3>, float, ActorData, bool)).MethodHandle;
			}
			List<Vector3> list = new List<Vector3>();
			list.Add(originalStart);
			list.AddRange(laserAnglePoints);
			List<ISquareInsideChecker> list2 = new List<ISquareInsideChecker>();
			for (int i = 1; i < list.Count; i++)
			{
				if (i >= 2)
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
				AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(operationObj, list[j - 1], list[j], widthInSquares, caster, ignoreLos, null, list2, false);
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
				BoardSquare boardSquare2 = enumerator.Current;
				if (VectorUtils.IsPointInTriangle(pA, pB, pC, boardSquare2.ToVector3()))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetSquaresInTriangle(Vector3, Vector3, Vector3, bool, ActorData)).MethodHandle;
					}
					bool flag = true;
					if (!ignoreLoS)
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
						flag = AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquare, boardSquare2, caster, true, null);
					}
					if (flag)
					{
						list.Add(boardSquare2);
					}
				}
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
		return list;
	}

	public static List<ActorData> GetActorsInTriangle(Vector3 pA, Vector3 pB, Vector3 pC, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams)
	{
		List<ActorData> list = new List<ActorData>();
		List<BoardSquare> squaresInTriangle = AreaEffectUtils.GetSquaresInTriangle(pA, pB, pC, ignoreLoS, caster);
		using (List<BoardSquare>.Enumerator enumerator = squaresInTriangle.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				ActorData occupantActor = boardSquare.OccupantActor;
				if (AreaEffectUtils.IsActorTargetable(occupantActor, onlyValidTeams))
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetActorsInTriangle(Vector3, Vector3, Vector3, bool, ActorData, List<Team>)).MethodHandle;
					}
					if (!list.Contains(occupantActor))
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
						list.Add(occupantActor);
					}
				}
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
		}
		return list;
	}

	public static List<BoardSquare> GetSquaresInShape_EvenByEven(Vector3 cornerPos, int dimensions, int cornersToSubtract, bool ignoreLoS, ActorData caster)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		List<BoardSquare> list2 = new List<BoardSquare>();
		float num = Board.Get().squareSize / 2f;
		if (!ignoreLoS)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetSquaresInShape_EvenByEven(Vector3, int, int, bool, ActorData)).MethodHandle;
			}
			List<BoardSquare> squaresAroundEvenShapeCornerPos = AreaEffectUtils.GetSquaresAroundEvenShapeCornerPos(cornerPos);
			list2 = AreaEffectUtils.GetCenterSquaresForEvenShapeLos(squaresAroundEvenShapeCornerPos, caster);
		}
		float num2 = cornerPos.x + num - Board.Get().squareSize * (float)(dimensions / 2);
		float num3 = cornerPos.z + num - Board.Get().squareSize * (float)(dimensions / 2);
		for (int i = 0; i < dimensions; i++)
		{
			float x = num2 + Board.Get().squareSize * (float)i;
			int j = 0;
			while (j < dimensions)
			{
				if (cornersToSubtract <= 0)
				{
					goto IL_F9;
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
				int num4 = Mathf.Min(dimensions - 1 - i, i);
				int num5 = Mathf.Min(dimensions - 1 - j, j);
				int num6 = num4 + num5;
				if (num6 >= cornersToSubtract)
				{
					goto IL_F9;
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
				IL_1F6:
				j++;
				continue;
				IL_F9:
				float y = num3 + Board.Get().squareSize * (float)j;
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(x, y);
				if (!(boardSquareSafe != null))
				{
					goto IL_1F6;
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
				if (!boardSquareSafe.IsBaselineHeight())
				{
					goto IL_1F6;
				}
				bool flag;
				if (ignoreLoS)
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
				}
				else if (list2.Contains(boardSquareSafe))
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
					flag = true;
				}
				else
				{
					int num7 = 0;
					using (List<BoardSquare>.Enumerator enumerator = list2.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BoardSquare source = enumerator.Current;
							if (AreaEffectUtils.SquaresHaveLoSForAbilities(source, boardSquareSafe, caster, true, null))
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
								num7++;
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
					}
					flag = (num7 >= 3);
				}
				if (flag)
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
					list.Add(boardSquareSafe);
					goto IL_1F6;
				}
				goto IL_1F6;
			}
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
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetSquaresAroundEvenShapeCornerPos(Vector3)).MethodHandle;
					}
					list.Add(boardSquare);
				}
			}
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
		return list;
	}

	private static List<BoardSquare> GetCenterSquaresForEvenShapeLos(List<BoardSquare> squaresByCorner, ActorData caster)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		using (List<BoardSquare>.Enumerator enumerator = squaresByCorner.GetEnumerator())
		{
			IL_97:
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				int num = 0;
				using (List<BoardSquare>.Enumerator enumerator2 = squaresByCorner.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						BoardSquare boardSquare2 = enumerator2.Current;
						if (boardSquare != boardSquare2 && AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquare, boardSquare2, caster, true, null))
						{
							num++;
							if (num >= 2)
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
									RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetCenterSquaresForEvenShapeLos(List<BoardSquare>, ActorData)).MethodHandle;
								}
								list.Add(boardSquare);
								goto IL_97;
							}
						}
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
		}
		return list;
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
			int j = num4;
			while (j <= num5)
			{
				if (cornersToSubtract <= 0)
				{
					goto IL_93;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetSquaresInShape_OddByOdd(BoardSquare, int, int, bool, ActorData)).MethodHandle;
				}
				int num6 = Mathf.Min(num3 - i, i - num2);
				int num7 = Mathf.Min(num5 - j, j - num4);
				int num8 = num6 + num7;
				if (num8 >= cornersToSubtract)
				{
					goto IL_93;
				}
				IL_110:
				j++;
				continue;
				IL_93:
				BoardSquare boardSquare = Board.Get().GetBoardSquare(i, j);
				if (!(boardSquare != null))
				{
					goto IL_110;
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
				if (boardSquare.IsBaselineHeight())
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
					if (ignoreLoS)
					{
						goto IL_101;
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
					if (boardSquare == centerSquare)
					{
						goto IL_101;
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
					bool flag = AreaEffectUtils.SquaresHaveLoSForAbilities(centerSquare, boardSquare, caster, true, null);
					IL_102:
					bool flag2 = flag;
					if (flag2)
					{
						list.Add(boardSquare);
						goto IL_110;
					}
					goto IL_110;
					IL_101:
					flag = true;
					goto IL_102;
				}
				goto IL_110;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
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
		return list;
	}

	public static List<BoardSquare> GetSquaresInShape(AbilityAreaShape shape, AbilityTarget target, bool ignoreLoS, ActorData caster)
	{
		Vector3 freePos = target.FreePos;
		GridPos gridPos = target.GridPos;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
		return AreaEffectUtils.GetSquaresInShape(shape, freePos, boardSquareSafe, ignoreLoS, caster);
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
			dimensions = 0xA;
			break;
		case AbilityAreaShape.Ten_x_Ten:
			dimensions = 0xA;
			break;
		case AbilityAreaShape.Eleven_x_Eleven_NoCorners:
			cornersToSubtract = 2;
			dimensions = 0xB;
			break;
		case AbilityAreaShape.Eleven_x_Eleven:
			dimensions = 0xB;
			break;
		default:
			dimensions = 1;
			break;
		}
	}

	public static List<BoardSquare> GetSquaresInShape(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare, bool ignoreLoS, ActorData caster)
	{
		int num;
		int cornersToSubtract;
		AreaEffectUtils.GetSquareDimentionAndCornersToSubtract(shape, out num, out cornersToSubtract);
		bool flag = num % 2 == 1;
		if (flag)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetSquaresInShape(AbilityAreaShape, Vector3, BoardSquare, bool, ActorData)).MethodHandle;
			}
			return AreaEffectUtils.GetSquaresInShape_OddByOdd(centerSquare, num, cornersToSubtract, ignoreLoS, caster);
		}
		Vector3 cornerPos = Board.\u000E(freePos, centerSquare);
		return AreaEffectUtils.GetSquaresInShape_EvenByEven(cornerPos, num, cornersToSubtract, ignoreLoS, caster);
	}

	public static List<ActorData> GetActorsInShape(AbilityAreaShape shape, AbilityTarget target, bool ignoreLoS, ActorData caster, Team onlyValidTeam, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 freePos = target.FreePos;
		GridPos gridPos = target.GridPos;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
		return AreaEffectUtils.GetActorsInShape(shape, freePos, boardSquareSafe, ignoreLoS, caster, new List<Team>
		{
			onlyValidTeam
		}, nonActorTargetInfo);
	}

	public static List<ActorData> GetActorsInShape(AbilityAreaShape shape, AbilityTarget target, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 freePos = target.FreePos;
		GridPos gridPos = target.GridPos;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
		return AreaEffectUtils.GetActorsInShape(shape, freePos, boardSquareSafe, ignoreLoS, caster, onlyValidTeams, nonActorTargetInfo);
	}

	public static List<ActorData> GetActorsInShape(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare, bool ignoreLoS, ActorData caster, Team onlyValidTeam, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return AreaEffectUtils.GetActorsInShape(shape, freePos, centerSquare, ignoreLoS, caster, new List<Team>
		{
			onlyValidTeam
		}, nonActorTargetInfo);
	}

	public static List<ActorData> GetActorsInShape(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare, bool ignoreLoS, ActorData caster, List<Team> onlyValidTeams, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list = new List<ActorData>();
		List<ActorData> actors = GameFlowData.Get().GetActors();
		using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (AreaEffectUtils.IsActorTargetable(actorData, onlyValidTeams))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetActorsInShape(AbilityAreaShape, Vector3, BoardSquare, bool, ActorData, List<Team>, List<NonActorTargetInfo>)).MethodHandle;
					}
					if (AreaEffectUtils.IsSquareInShape(actorData.GetCurrentBoardSquare(), shape, freePos, centerSquare, ignoreLoS, caster))
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
						list.Add(actorData);
					}
				}
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
		}
		return list;
	}

	public unsafe static List<ActorData> GetActorsInShapeLayers(List<AbilityAreaShape> shapes, Vector3 freePos, BoardSquare centerSquare, bool ignoreLos, ActorData caster, List<Team> onlyValidTeams, out List<List<ActorData>> actorsInLayers, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> list = new List<ActorData>();
		actorsInLayers = new List<List<ActorData>>();
		for (int i = 0; i < shapes.Count; i++)
		{
			actorsInLayers.Add(new List<ActorData>());
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetActorsInShapeLayers(List<AbilityAreaShape>, Vector3, BoardSquare, bool, ActorData, List<Team>, List<List<ActorData>>*, List<NonActorTargetInfo>)).MethodHandle;
		}
		List<ActorData> actors = GameFlowData.Get().GetActors();
		using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
		{
			IL_E3:
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (AreaEffectUtils.IsActorTargetable(actorData, onlyValidTeams))
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
					for (int j = 0; j < shapes.Count; j++)
					{
						AbilityAreaShape shape = shapes[j];
						if (AreaEffectUtils.IsSquareInShape(actorData.GetCurrentBoardSquare(), shape, freePos, centerSquare, ignoreLos, caster))
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
							actorsInLayers[j].Add(actorData);
							list.Add(actorData);
							goto IL_E3;
						}
					}
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
				}
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
		return list;
	}

	public static void OperateOnSquaresInShape(IOperationOnSquare operationObj, AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare, bool ignoreLoS, ActorData caster, List<ISquareInsideChecker> losCheckOverrides = null)
	{
		int num;
		int cornersToSubtract;
		AreaEffectUtils.GetSquareDimentionAndCornersToSubtract(shape, out num, out cornersToSubtract);
		bool flag = num % 2 == 1;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.OperateOnSquaresInShape(IOperationOnSquare, AbilityAreaShape, Vector3, BoardSquare, bool, ActorData, List<ISquareInsideChecker>)).MethodHandle;
			}
			AreaEffectUtils.OperateOnSquaresInShape_OddByOdd(operationObj, centerSquare, num, cornersToSubtract, ignoreLoS, caster, losCheckOverrides);
		}
		else
		{
			Vector3 cornerPos = Board.\u000E(freePos, centerSquare);
			AreaEffectUtils.OperateOnSquaresInShape_EvenByEven(operationObj, cornerPos, num, cornersToSubtract, ignoreLoS, caster, losCheckOverrides);
		}
	}

	public static void OperateOnSquaresInShape_EvenByEven(IOperationOnSquare operationObj, Vector3 cornerPos, int dimensions, int cornersToSubtract, bool ignoreLoS, ActorData caster, List<ISquareInsideChecker> losCheckOverrides = null)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		float num = Board.Get().squareSize / 2f;
		if (!ignoreLoS)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.OperateOnSquaresInShape_EvenByEven(IOperationOnSquare, Vector3, int, int, bool, ActorData, List<ISquareInsideChecker>)).MethodHandle;
			}
			List<BoardSquare> squaresAroundEvenShapeCornerPos = AreaEffectUtils.GetSquaresAroundEvenShapeCornerPos(cornerPos);
			list = AreaEffectUtils.GetCenterSquaresForEvenShapeLos(squaresAroundEvenShapeCornerPos, caster);
		}
		float num2 = cornerPos.x + num - Board.Get().squareSize * (float)(dimensions / 2);
		float num3 = cornerPos.z + num - Board.Get().squareSize * (float)(dimensions / 2);
		for (int i = 0; i < dimensions; i++)
		{
			float x = num2 + Board.Get().squareSize * (float)i;
			int j = 0;
			while (j < dimensions)
			{
				if (cornersToSubtract <= 0)
				{
					goto IL_E4;
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
				int num4 = Mathf.Min(dimensions - 1 - i, i);
				int num5 = Mathf.Min(dimensions - 1 - j, j);
				int num6 = num4 + num5;
				if (num6 >= cornersToSubtract)
				{
					goto IL_E4;
				}
				IL_1EE:
				j++;
				continue;
				IL_E4:
				float y = num3 + Board.Get().squareSize * (float)j;
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(x, y);
				if (!(boardSquareSafe != null))
				{
					goto IL_1EE;
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
				if (boardSquareSafe.IsBaselineHeight())
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
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						squareHasLos = AreaEffectUtils.SquareHasLosByAreaCheckers(boardSquareSafe, losCheckOverrides);
					}
					else
					{
						int num7 = 0;
						using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								BoardSquare source = enumerator.Current;
								if (AreaEffectUtils.SquaresHaveLoSForAbilities(source, boardSquareSafe, caster, true, null))
								{
									num7++;
								}
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
						if (num7 >= 3)
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
							squareHasLos = true;
						}
						else
						{
							squareHasLos = false;
						}
					}
					operationObj.OperateOnSquare(boardSquareSafe, caster, squareHasLos);
					goto IL_1EE;
				}
				goto IL_1EE;
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

	public static void OperateOnSquaresInShape_OddByOdd(IOperationOnSquare operationObj, BoardSquare centerSquare, int dimensions, int cornersToSubtract, bool ignoreLoS, ActorData caster, List<ISquareInsideChecker> losCheckOverrides = null)
	{
		int num = dimensions / 2;
		int num2 = centerSquare.x - num;
		int num3 = centerSquare.x + num;
		int num4 = centerSquare.y - num;
		int num5 = centerSquare.y + num;
		for (int i = num2; i <= num3; i++)
		{
			int j = num4;
			while (j <= num5)
			{
				if (cornersToSubtract <= 0)
				{
					goto IL_94;
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.OperateOnSquaresInShape_OddByOdd(IOperationOnSquare, BoardSquare, int, int, bool, ActorData, List<ISquareInsideChecker>)).MethodHandle;
				}
				int num6 = Mathf.Min(num3 - i, i - num2);
				int num7 = Mathf.Min(num5 - j, j - num4);
				int num8 = num6 + num7;
				if (num8 >= cornersToSubtract)
				{
					goto IL_94;
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
				IL_132:
				j++;
				continue;
				IL_94:
				BoardSquare boardSquare = Board.Get().GetBoardSquare(i, j);
				if (!(boardSquare != null))
				{
					goto IL_132;
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
				if (boardSquare.IsBaselineHeight())
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
					bool squareHasLos;
					if (ignoreLoS)
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
						squareHasLos = true;
					}
					else if (losCheckOverrides != null)
					{
						squareHasLos = AreaEffectUtils.SquareHasLosByAreaCheckers(boardSquare, losCheckOverrides);
					}
					else
					{
						bool flag;
						if (!(boardSquare == centerSquare))
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
							flag = AreaEffectUtils.SquaresHaveLoSForAbilities(centerSquare, boardSquare, caster, true, null);
						}
						else
						{
							flag = true;
						}
						squareHasLos = flag;
					}
					operationObj.OperateOnSquare(boardSquare, caster, squareHasLos);
					goto IL_132;
				}
				goto IL_132;
			}
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

	public static bool IsSquareInShape(BoardSquare testSquare, AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare, bool ignoreLoS, ActorData caster)
	{
		int num;
		int cornersToSubtract;
		AreaEffectUtils.GetSquareDimentionAndCornersToSubtract(shape, out num, out cornersToSubtract);
		bool flag = num % 2 == 1;
		if (flag)
		{
			return AreaEffectUtils.IsSquareInShape_OddByOdd(testSquare, centerSquare, num, cornersToSubtract, ignoreLoS, caster);
		}
		Vector3 cornerPos = Board.\u000E(freePos, centerSquare);
		return AreaEffectUtils.IsSquareInShape_EvenByEven(testSquare, cornerPos, num, cornersToSubtract, ignoreLoS, caster);
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetSquaresAroundCornerPos(Vector3)).MethodHandle;
					}
					list.Add(boardSquare);
				}
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
		return list;
	}

	private static List<BoardSquare> GetCenterSquaresForShapeLos(List<BoardSquare> squaresByCorner, ActorData caster)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		using (List<BoardSquare>.Enumerator enumerator = squaresByCorner.GetEnumerator())
		{
			IL_AB:
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				int num = 0;
				using (List<BoardSquare>.Enumerator enumerator2 = squaresByCorner.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						BoardSquare boardSquare2 = enumerator2.Current;
						if (boardSquare != boardSquare2)
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
								RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetCenterSquaresForShapeLos(List<BoardSquare>, ActorData)).MethodHandle;
							}
							if (AreaEffectUtils.SquaresHaveLoSForAbilities(boardSquare, boardSquare2, caster, true, null))
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
								num++;
								if (num >= 2)
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
									list.Add(boardSquare);
									goto IL_AB;
								}
							}
						}
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
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return list;
	}

	private static bool IsSquareInShape_EvenByEven(BoardSquare testSquare, Vector3 cornerPos, List<BoardSquare> squaresInCenter, int dimensions, int cornersToSubtract, bool ignoreLoS, ActorData caster)
	{
		if (testSquare == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsSquareInShape_EvenByEven(BoardSquare, Vector3, List<BoardSquare>, int, int, bool, ActorData)).MethodHandle;
			}
			return false;
		}
		bool result = false;
		float num = Board.Get().squareSize / 2f;
		float num2 = cornerPos.x + num;
		float num3 = cornerPos.z + num;
		float squareSize = Board.Get().squareSize;
		int num4 = dimensions / 2;
		int num5 = Mathf.RoundToInt((num2 - testSquare.worldX) / squareSize);
		int num6;
		if (num5 > 0)
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
			num6 = num4 - num5;
		}
		else
		{
			num6 = num4 - Mathf.Abs(num5) - 1;
		}
		int num7 = Mathf.RoundToInt((num3 - testSquare.worldY) / squareSize);
		int num8;
		if (num7 > 0)
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
			num8 = num4 - num7;
		}
		else
		{
			num8 = num4 - Mathf.Abs(num7) - 1;
		}
		if (num6 >= 0 && num8 >= 0)
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
			int num9 = num6 + num8;
			if (num9 >= cornersToSubtract)
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
				result = true;
				if (!ignoreLoS)
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
					bool flag;
					if (squaresInCenter.Contains(testSquare))
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
						flag = true;
					}
					else
					{
						int num10 = 0;
						foreach (BoardSquare source in squaresInCenter)
						{
							if (AreaEffectUtils.SquaresHaveLoSForAbilities(source, testSquare, caster, true, null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsSquareInShape_EvenByEven(BoardSquare, Vector3, int, int, bool, ActorData)).MethodHandle;
			}
			return false;
		}
		List<BoardSquare> squaresInCenter = null;
		if (!ignoreLoS)
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
			List<BoardSquare> squaresAroundCornerPos = AreaEffectUtils.GetSquaresAroundCornerPos(cornerPos);
			squaresInCenter = AreaEffectUtils.GetCenterSquaresForShapeLos(squaresAroundCornerPos, caster);
		}
		return AreaEffectUtils.IsSquareInShape_EvenByEven(testSquare, cornerPos, squaresInCenter, dimensions, cornersToSubtract, ignoreLoS, caster);
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
		if (num2 <= num)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsSquareInShape_OddByOdd(BoardSquare, BoardSquare, int, int, bool, ActorData)).MethodHandle;
			}
			if (num3 <= num)
			{
				int num4 = num - num2;
				int num5 = num - num3;
				int num6 = num4 + num5;
				if (num6 >= cornersToSubtract)
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
					bool flag;
					if (!ignoreLoS)
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
						if (!(testSquare == centerSquare))
						{
							flag = AreaEffectUtils.SquaresHaveLoSForAbilities(centerSquare, testSquare, caster, true, null);
							goto IL_B2;
						}
					}
					flag = true;
					IL_B2:
					result = flag;
				}
			}
		}
		return result;
	}

	public static bool IsPosInShape(Vector3 testPos, AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		int num;
		int num2;
		AreaEffectUtils.GetSquareDimentionAndCornersToSubtract(shape, out num, out num2);
		BoardSquare boardSquare = Board.Get().GetBoardSquare(testPos);
		if (boardSquare != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsPosInShape(Vector3, AbilityAreaShape, Vector3, BoardSquare)).MethodHandle;
			}
			return AreaEffectUtils.IsSquareInShape(boardSquare, shape, freePos, centerSquare, true, null);
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsPosInShape_OddByOdd(Vector3, BoardSquare, int, int)).MethodHandle;
			}
			if (num6 <= (float)num2)
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
		int num5 = Mathf.RoundToInt((num2 - testPos.x) / squareSize);
		int num6;
		if (num5 > 0)
		{
			num6 = num4 - num5;
		}
		else
		{
			num6 = num4 - Mathf.Abs(num5) - 1;
		}
		int num7 = Mathf.RoundToInt((num3 - testPos.z) / squareSize);
		int num8;
		if (num7 > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.IsPosInShape_EvenByEven(Vector3, Vector3, int, int)).MethodHandle;
			}
			num8 = num4 - num7;
		}
		else
		{
			num8 = num4 - Mathf.Abs(num7) - 1;
		}
		if (num6 >= 0)
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
			if (num8 >= 0)
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
				int num9 = num6 + num8;
				if (num9 >= cornersToSubtract)
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
					result = true;
				}
			}
		}
		return result;
	}

	public static Vector3 GetCenterOfShape(AbilityAreaShape shape, AbilityTarget target)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		return AreaEffectUtils.GetCenterOfShape(shape, target.FreePos, boardSquareSafe);
	}

	public static bool IsShapeOddByOdd(AbilityAreaShape shape)
	{
		bool result;
		switch (shape)
		{
		case AbilityAreaShape.SingleSquare:
			result = true;
			break;
		case AbilityAreaShape.Two_x_Two:
			result = false;
			break;
		case AbilityAreaShape.Three_x_Three_NoCorners:
			result = true;
			break;
		case AbilityAreaShape.Three_x_Three:
			result = true;
			break;
		case AbilityAreaShape.Four_x_Four_NoCorners:
			result = false;
			break;
		case AbilityAreaShape.Four_x_Four:
			result = false;
			break;
		case AbilityAreaShape.Five_x_Five_ExtraNoCorners:
			result = true;
			break;
		case AbilityAreaShape.Five_x_Five_NoCorners:
			result = true;
			break;
		case AbilityAreaShape.Five_x_Five:
			result = true;
			break;
		case AbilityAreaShape.Six_x_Six_ExtraNoCorners:
			result = false;
			break;
		case AbilityAreaShape.Six_x_Six_NoCorners:
			result = false;
			break;
		case AbilityAreaShape.Six_x_Six:
			result = false;
			break;
		case AbilityAreaShape.Seven_x_Seven_ExtraNoCorners:
			result = true;
			break;
		case AbilityAreaShape.Seven_x_Seven_NoCorners:
			result = true;
			break;
		case AbilityAreaShape.Seven_x_Seven:
			result = true;
			break;
		default:
			result = true;
			break;
		}
		return result;
	}

	public static Vector3 GetCenterOfShape(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		bool flag = AreaEffectUtils.IsShapeOddByOdd(shape);
		Vector3 result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetCenterOfShape(AbilityAreaShape, Vector3, BoardSquare)).MethodHandle;
			}
			result = centerSquare.ToVector3();
		}
		else
		{
			result = Board.\u000E(freePos, centerSquare);
		}
		return result;
	}

	public static Vector3 GetCenterOfGridPattern(AbilityGridPattern pattern, AbilityTarget target)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		return AreaEffectUtils.GetCenterOfGridPattern(pattern, target.FreePos, boardSquareSafe);
	}

	public static Vector3 GetCenterOfGridPattern(AbilityGridPattern pattern, Vector3 freePos, BoardSquare centerSquare)
	{
		bool flag;
		if (pattern != AbilityGridPattern.Plus_Two_x_Two)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.GetCenterOfGridPattern(AbilityGridPattern, Vector3, BoardSquare)).MethodHandle;
			}
			if (pattern != AbilityGridPattern.Plus_Four_x_Four)
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
		Vector3 result;
		if (flag)
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
			result = centerSquare.ToVector3();
		}
		else
		{
			result = Board.\u000E(freePos, centerSquare);
		}
		return result;
	}

	public static bool HasAdjacentActorOfTeam(ActorData aroundActor, List<Team> teams)
	{
		if (AreaEffectUtils.IsActorTargetable(aroundActor, null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.HasAdjacentActorOfTeam(ActorData, List<Team>)).MethodHandle;
			}
			List<ActorData> actors = GameFlowData.Get().GetActors();
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					if (actorData != aroundActor)
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
						if (AreaEffectUtils.IsActorTargetable(actorData, teams))
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
							if (Board.Get().\u000E(aroundActor.GetCurrentBoardSquare(), actorData.GetCurrentBoardSquare()))
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
								return true;
							}
						}
					}
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
			return false;
		}
		return false;
	}

	public static void AddShapeCornersToList(ref List<Vector3> points, AbilityAreaShape shape, AbilityTarget target)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		AreaEffectUtils.AddShapeCornersToList(ref points, shape, target.FreePos, boardSquareSafe);
	}

	public unsafe static void AddShapeCornersToList(ref List<Vector3> points, AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(shape, freePos, centerSquare);
		float num;
		if (shape == AbilityAreaShape.SingleSquare)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AreaEffectUtils.AddShapeCornersToList(List<Vector3>*, AbilityAreaShape, Vector3, BoardSquare)).MethodHandle;
			}
			num = 1f;
		}
		else if (shape == AbilityAreaShape.Two_x_Two)
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
			num = 2f;
		}
		else
		{
			if (shape != AbilityAreaShape.Three_x_Three)
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
				if (shape != AbilityAreaShape.Three_x_Three_NoCorners)
				{
					if (shape != AbilityAreaShape.Four_x_Four)
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
						if (shape != AbilityAreaShape.Four_x_Four_NoCorners)
						{
							if (shape != AbilityAreaShape.Five_x_Five)
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
								if (shape != AbilityAreaShape.Five_x_Five_NoCorners)
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
									if (shape != AbilityAreaShape.Five_x_Five_ExtraNoCorners)
									{
										if (shape != AbilityAreaShape.Six_x_Six)
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
											if (shape != AbilityAreaShape.Six_x_Six_NoCorners)
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
												if (shape != AbilityAreaShape.Six_x_Six_ExtraNoCorners)
												{
													if (shape != AbilityAreaShape.Seven_x_Seven)
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
														if (shape != AbilityAreaShape.Seven_x_Seven_NoCorners)
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
															if (shape != AbilityAreaShape.Seven_x_Seven_ExtraNoCorners)
															{
																num = 0f;
																goto IL_12E;
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
														}
													}
													num = 7f;
													goto IL_12E;
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
											}
										}
										num = 6f;
										goto IL_12E;
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
								}
							}
							num = 5f;
							goto IL_12E;
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
					num = 4f;
					goto IL_12E;
				}
			}
			num = 3f;
		}
		IL_12E:
		float num2 = num / 2f;
		float num3 = num2 * Board.Get().squareSize;
		points.Add(new Vector3(centerOfShape.x + num3, centerOfShape.y, centerOfShape.z + num3));
		points.Add(new Vector3(centerOfShape.x + num3, centerOfShape.y, centerOfShape.z - num3));
		points.Add(new Vector3(centerOfShape.x - num3, centerOfShape.y, centerOfShape.z + num3));
		points.Add(new Vector3(centerOfShape.x - num3, centerOfShape.y, centerOfShape.z - num3));
	}

	public static List<Vector3> BuildShapeCornersList(AbilityAreaShape shape, AbilityTarget target)
	{
		List<Vector3> result = new List<Vector3>();
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		AreaEffectUtils.AddShapeCornersToList(ref result, shape, target.FreePos, boardSquareSafe);
		return result;
	}

	public static List<Vector3> BuildShapeCornersList(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		List<Vector3> result = new List<Vector3>();
		AreaEffectUtils.AddShapeCornersToList(ref result, shape, freePos, centerSquare);
		return result;
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
			this.m_segmentOrigin = segmentOrigin;
			this.m_endpointIndex = endpointIndex;
		}
	}
}
