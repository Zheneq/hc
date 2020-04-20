using System;
using System.Collections.Generic;
using UnityEngine;

public static class TargeterUtils
{
	private static float s_nearAcceleratingMinSpeed = 20f;

	private static float s_farAcceleratingMinSpeed = 10f;

	private static float s_brakeMinSpeed = 1f;

	private static float s_nearAcceleratingMaxSpeed = 60f;

	private static float s_farAcceleratingMaxSpeed = 99999f;

	private static float s_brakeMaxSpeed = 20f;

	private static float s_nearAcceleration = 40f;

	private static float s_farAcceleration = 1250f;

	private static float s_brakeAcceleration = 40f;

	private static float s_brakeDistance = 1f;

	private static float s_farDistance = 6f;

	public static GameObject CreateLaserBoxHighlight(Vector3 start, Vector3 end, float widthInSquares, TargeterUtils.HeightAdjustType adjustType)
	{
		float widthInWorld = widthInSquares * Board.Get().squareSize;
		float magnitude = (start - end).magnitude;
		GameObject gameObject = HighlightUtils.Get().CreateRectangularCursor(widthInWorld, magnitude, null);
		TargeterUtils.RefreshLaserBoxHighlight(gameObject, start, end, widthInSquares, adjustType);
		return gameObject;
	}

	public static void RefreshLaserBoxHighlight(GameObject boxHighlight, Vector3 start, Vector3 end, float widthInSquares, TargeterUtils.HeightAdjustType adjustType)
	{
		float widthInWorld = widthInSquares * Board.Get().squareSize;
		float magnitude = (start - end).magnitude;
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, boxHighlight);
		float heightAdjustDelta = TargeterUtils.GetHeightAdjustDelta(adjustType);
		boxHighlight.transform.position = start + new Vector3(0f, heightAdjustDelta, 0f);
		Vector3 normalized = (end - start).normalized;
		boxHighlight.transform.rotation = Quaternion.LookRotation(normalized);
	}

	public static GameObject CreateCircleHighlight(Vector3 pos, float radiusInSquares, TargeterUtils.HeightAdjustType adjustType, bool isForLocalPlayer)
	{
		float radiusInWorld = radiusInSquares * Board.Get().squareSize;
		GameObject gameObject = HighlightUtils.Get().CreateAoECursor(radiusInWorld, isForLocalPlayer);
		TargeterUtils.RefreshCircleHighlight(gameObject, pos, adjustType);
		return gameObject;
	}

	public static void RefreshCircleHighlight(GameObject circleHighlight, Vector3 pos, TargeterUtils.HeightAdjustType adjustType)
	{
		float heightAdjustDelta = TargeterUtils.GetHeightAdjustDelta(adjustType);
		circleHighlight.transform.position = pos + new Vector3(0f, heightAdjustDelta, 0f);
	}

	private static float GetHeightAdjustDelta(TargeterUtils.HeightAdjustType adjustType)
	{
		float result;
		if (adjustType == TargeterUtils.HeightAdjustType.DontAdjustHeight)
		{
			result = 0f;
		}
		else if (adjustType == TargeterUtils.HeightAdjustType.FromCasterLoS)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.GetHeightAdjustDelta(TargeterUtils.HeightAdjustType)).MethodHandle;
			}
			result = 0.1f - BoardSquare.s_LoSHeightOffset;
		}
		else if (adjustType == TargeterUtils.HeightAdjustType.FromPathArrow)
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
			result = 0f;
		}
		else if (adjustType == TargeterUtils.HeightAdjustType.FromBoardSquare)
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
			result = 0.1f;
		}
		else
		{
			Log.Error("Trying to adjust the height of a laser box with an invalid HeightAdjustType: " + adjustType.ToString(), new object[0]);
			result = 0f;
		}
		return result;
	}

	public static void SortActorsByDistanceToPos(ref List<ActorData> actors, Vector3 pos)
	{
		actors.Sort(delegate(ActorData x, ActorData y)
		{
			if (x == y)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.<SortActorsByDistanceToPos>c__AnonStorey0.<>m__0(ActorData, ActorData)).MethodHandle;
				}
				return 0;
			}
			if (x == null)
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
				return -1;
			}
			if (y == null)
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
				return 1;
			}
			float sqrMagnitude = (x.GetTravelBoardSquareWorldPosition() - pos).sqrMagnitude;
			float sqrMagnitude2 = (y.GetTravelBoardSquareWorldPosition() - pos).sqrMagnitude;
			if (sqrMagnitude == sqrMagnitude2)
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
				GridPos gridPosWithIncrementedHeight = x.GetGridPosWithIncrementedHeight();
				GridPos gridPosWithIncrementedHeight2 = y.GetGridPosWithIncrementedHeight();
				if (gridPosWithIncrementedHeight.x != gridPosWithIncrementedHeight2.x)
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
					return gridPosWithIncrementedHeight.x.CompareTo(gridPosWithIncrementedHeight2.x);
				}
				if (gridPosWithIncrementedHeight.y != gridPosWithIncrementedHeight2.y)
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
					return gridPosWithIncrementedHeight.y.CompareTo(gridPosWithIncrementedHeight2.y);
				}
			}
			return sqrMagnitude.CompareTo(sqrMagnitude2);
		});
	}

	public static void SortActorsByDistanceToPos(ref List<ActorData> actors, Vector3 pos, Vector3 preferredDir)
	{
		actors.Sort(delegate(ActorData x, ActorData y)
		{
			if (x == y)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.<SortActorsByDistanceToPos>c__AnonStorey1.<>m__0(ActorData, ActorData)).MethodHandle;
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
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				return 1;
			}
			Vector3 to = x.GetTravelBoardSquareWorldPosition() - pos;
			Vector3 to2 = y.GetTravelBoardSquareWorldPosition() - pos;
			float sqrMagnitude = to.sqrMagnitude;
			float sqrMagnitude2 = to2.sqrMagnitude;
			if (sqrMagnitude == sqrMagnitude2)
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
				float num = Vector3.Angle(preferredDir, to);
				float value = Vector3.Angle(preferredDir, to2);
				return num.CompareTo(value);
			}
			return sqrMagnitude.CompareTo(sqrMagnitude2);
		});
	}

	public static void SortActorsByDistanceAlongLaser(ref List<ActorData> actors, VectorUtils.LaserCoords coords)
	{
		Vector3 start = coords.start;
		Vector3 laserDir = coords.end - coords.start;
		laserDir.Normalize();
		TargeterUtils.SortActorsByDistanceAlongLaser(ref actors, start, laserDir);
	}

	public static void SortActorsByDistanceAlongLaser(ref List<ActorData> actors, Vector3 laserStart, Vector3 laserDir)
	{
		actors.Sort(delegate(ActorData a, ActorData b)
		{
			if (a == b)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.<SortActorsByDistanceAlongLaser>c__AnonStorey2.<>m__0(ActorData, ActorData)).MethodHandle;
				}
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
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
				return 1;
			}
			float signedDistanceAlongLaser = TargeterUtils.GetSignedDistanceAlongLaser(a, laserStart, laserDir);
			float signedDistanceAlongLaser2 = TargeterUtils.GetSignedDistanceAlongLaser(b, laserStart, laserDir);
			return signedDistanceAlongLaser.CompareTo(signedDistanceAlongLaser2);
		});
	}

	public static float GetSignedDistanceAlongLaser(ActorData actor, Vector3 laserStart, Vector3 laserDir)
	{
		Vector3 lhs = new Vector3(actor.GetTravelBoardSquareWorldPosition().x - laserStart.x, 0f, actor.GetTravelBoardSquareWorldPosition().z - laserStart.z);
		return Vector3.Dot(lhs, laserDir);
	}

	public unsafe static void RemoveActorsInvisibleToClient(ref List<ActorData> actors)
	{
		for (int i = actors.Count - 1; i >= 0; i--)
		{
			if (!actors[i].IsVisibleToClient())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.RemoveActorsInvisibleToClient(List<ActorData>*)).MethodHandle;
				}
				actors.RemoveAt(i);
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

	public unsafe static void RemoveActorsInvisibleToActor(ref List<ActorData> actors, ActorData observer)
	{
		if (actors != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.RemoveActorsInvisibleToActor(List<ActorData>*, ActorData)).MethodHandle;
			}
			for (int i = actors.Count - 1; i >= 0; i--)
			{
				if (!actors[i].IsActorVisibleToActor(observer, false))
				{
					actors.RemoveAt(i);
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

	public unsafe static void RemoveActorsWithoutLosToSquare(ref List<ActorData> actors, BoardSquare sourceSquare, ActorData caster)
	{
		if (sourceSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.RemoveActorsWithoutLosToSquare(List<ActorData>*, BoardSquare, ActorData)).MethodHandle;
			}
			if (actors != null)
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
				if (caster != null)
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
					for (int i = actors.Count - 1; i >= 0; i--)
					{
						BoardSquare currentBoardSquare = actors[i].GetCurrentBoardSquare();
						if (currentBoardSquare != null && !AreaEffectUtils.SquaresHaveLoSForAbilities(sourceSquare, currentBoardSquare, caster, true, null))
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
							actors.RemoveAt(i);
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
		}
	}

	public unsafe static void LimitActorsToMaxNumber(ref List<ActorData> actors, int max)
	{
		if (actors.Count > max)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.LimitActorsToMaxNumber(List<ActorData>*, int)).MethodHandle;
			}
			if (max > 0)
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
				int count = actors.Count - max;
				actors.RemoveRange(max, count);
			}
		}
	}

	public static VectorUtils.LaserCoords GetLaserCoordsToFarthestTarget(VectorUtils.LaserCoords coords, List<ActorData> targets)
	{
		float num = 0f;
		Vector3 end = coords.start;
		Vector3 start = coords.start;
		Vector3 vector = coords.end - coords.start;
		vector.Normalize();
		foreach (ActorData actor in targets)
		{
			float signedDistanceAlongLaser = TargeterUtils.GetSignedDistanceAlongLaser(actor, start, vector);
			if (signedDistanceAlongLaser > num)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.GetLaserCoordsToFarthestTarget(VectorUtils.LaserCoords, List<ActorData>)).MethodHandle;
				}
				num = signedDistanceAlongLaser;
				end = start + vector * num;
			}
		}
		return new VectorUtils.LaserCoords
		{
			start = coords.start,
			end = end
		};
	}

	public unsafe static VectorUtils.LaserCoords TrimTargetsAndGetLaserCoordsToFarthestTarget(ref List<ActorData> actorsInRange, int maxTargets, VectorUtils.LaserCoords coords)
	{
		VectorUtils.LaserCoords result = coords;
		if (actorsInRange.Count > maxTargets)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.TrimTargetsAndGetLaserCoordsToFarthestTarget(List<ActorData>*, int, VectorUtils.LaserCoords)).MethodHandle;
			}
			if (maxTargets > 0)
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
				TargeterUtils.SortActorsByDistanceAlongLaser(ref actorsInRange, coords);
				TargeterUtils.LimitActorsToMaxNumber(ref actorsInRange, maxTargets);
				return TargeterUtils.GetLaserCoordsToFarthestTarget(coords, actorsInRange);
			}
		}
		if (actorsInRange.Count == maxTargets)
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
				result = TargeterUtils.GetLaserCoordsToFarthestTarget(coords, actorsInRange);
			}
		}
		return result;
	}

	public static Vector3 GetEndPointAndLimitToFurthestSquare(Vector3 startPos, Vector3 endPos, float widthInSquares, float maxDistanceInSquares, bool penetrateLos, ActorData targetingActor, float thresholdInSquares = 0.71f)
	{
		endPos.y = startPos.y;
		if (startPos == endPos)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.GetEndPointAndLimitToFurthestSquare(Vector3, Vector3, float, float, bool, ActorData, float)).MethodHandle;
			}
			return startPos;
		}
		float squareSize = Board.Get().squareSize;
		Vector3 normalized = (endPos - startPos).normalized;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(startPos, normalized, maxDistanceInSquares * squareSize, penetrateLos, targetingActor, null, true);
		Vector3 result = laserEndPoint;
		List<BoardSquare> squaresInBox = AreaEffectUtils.GetSquaresInBox(startPos, laserEndPoint, widthInSquares / 2f, penetrateLos, targetingActor);
		AreaEffectUtils.SortSquaresByDistanceToPos(ref squaresInBox, startPos);
		bool flag = false;
		for (int i = squaresInBox.Count - 1; i >= 0; i--)
		{
			if (squaresInBox[i].IsBaselineHeight())
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
				break;
			}
			if (i > 0)
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
				if (squaresInBox[i - 1].IsBaselineHeight())
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
					if (squaresInBox[i].x != squaresInBox[i - 1].x)
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
						if (squaresInBox[i].y != squaresInBox[i - 1].y)
						{
							goto IL_148;
						}
					}
					flag = true;
				}
			}
			IL_148:
			squaresInBox.RemoveAt(i);
		}
		if (squaresInBox.Count > 0)
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
			float magnitude = (laserEndPoint - startPos).magnitude;
			float num = 0.5f * squareSize;
			bool flag2 = widthInSquares <= 1f;
			BoardSquare boardSquare = squaresInBox[squaresInBox.Count - 1];
			Vector3 vector = boardSquare.ToVector3();
			vector.y = startPos.y;
			Vector3 lhs = vector - startPos;
			float num2 = Vector3.Dot(lhs, normalized);
			float num3;
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
				if (flag2)
				{
					num3 = squareSize;
					goto IL_208;
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
			num3 = num;
			IL_208:
			float num4 = num3;
			float num5 = Mathf.Min(magnitude, num2 + num4);
			if (flag2 && squaresInBox.Count > 1)
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
				float d = 0.5f * squareSize;
				float a = Mathf.Min(magnitude, num2);
				BoardSquare boardSquare2 = squaresInBox[squaresInBox.Count - 2];
				if (boardSquare2.IsBaselineHeight())
				{
					bool flag3 = false;
					Vector3 a2 = Vector3.zero;
					float num6 = VectorUtils.HorizontalAngle_Deg(normalized);
					if (boardSquare2.x == boardSquare.x)
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
						Vector3 forward = Vector3.forward;
						Vector3 vector2 = vector;
						if (num6 >= 90f)
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
							if (num6 <= 270f)
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
								vector2 -= d * Vector3.right;
								goto IL_310;
							}
						}
						vector2 += d * Vector3.right;
						IL_310:
						a2 = VectorUtils.GetLineLineIntersection(startPos, normalized, vector2, forward, out flag3);
					}
					else if (boardSquare2.y == boardSquare.y)
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
						Vector3 right = Vector3.right;
						Vector3 vector3 = vector;
						if (num6 >= 0f)
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
							if (num6 <= 180f)
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
								vector3 += d * Vector3.forward;
								goto IL_3AE;
							}
						}
						vector3 -= d * Vector3.forward;
						IL_3AE:
						a2 = VectorUtils.GetLineLineIntersection(startPos, normalized, vector3, right, out flag3);
					}
					if (flag3)
					{
						float b = Vector3.Dot(a2 - startPos, normalized);
						num5 = Mathf.Min(num5, b);
						num5 = Mathf.Max(a, num5);
					}
				}
			}
			Vector3 vector4 = startPos + num5 * normalized;
			float magnitude2 = (laserEndPoint - vector4).magnitude;
			if (magnitude2 > thresholdInSquares * squareSize)
			{
				result = vector4;
			}
		}
		return result;
	}

	public unsafe static Vector3 MoveHighlightTowards(Vector3 goalPos, GameObject highlight, ref float currentSpeed)
	{
		Vector3 position = highlight.transform.position;
		Vector3 result;
		if (position == goalPos)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.MoveHighlightTowards(Vector3, GameObject, float*)).MethodHandle;
			}
			currentSpeed = TargeterUtils.s_nearAcceleratingMinSpeed;
			result = goalPos;
		}
		else
		{
			float deltaTime = Time.deltaTime;
			float sqrMagnitude = (goalPos - position).sqrMagnitude;
			bool flag = sqrMagnitude >= TargeterUtils.s_farDistance;
			bool flag2 = sqrMagnitude <= TargeterUtils.s_brakeDistance;
			float num;
			float min;
			float max;
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
				num = TargeterUtils.s_farAcceleration;
				min = TargeterUtils.s_farAcceleratingMinSpeed;
				max = TargeterUtils.s_farAcceleratingMaxSpeed;
			}
			else if (!flag2)
			{
				num = TargeterUtils.s_nearAcceleration;
				min = TargeterUtils.s_nearAcceleratingMinSpeed;
				max = TargeterUtils.s_nearAcceleratingMaxSpeed;
			}
			else
			{
				num = -TargeterUtils.s_brakeAcceleration;
				min = TargeterUtils.s_brakeMinSpeed;
				max = TargeterUtils.s_brakeMaxSpeed;
			}
			float num2 = currentSpeed + num * deltaTime;
			num2 = Mathf.Clamp(num2, min, max);
			float num3 = num2 * deltaTime;
			Vector3 a = goalPos - position;
			float magnitude = a.magnitude;
			if (num3 >= magnitude)
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
				currentSpeed = TargeterUtils.s_nearAcceleratingMinSpeed;
				result = goalPos;
			}
			else
			{
				a.Normalize();
				currentSpeed = num2;
				result = position + a * num3;
			}
		}
		return result;
	}

	public static List<Team> GetRelevantTeams(ActorData allyActor, bool includeAllies, bool includeEnemies)
	{
		List<Team> list = new List<Team>();
		if (includeAllies)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.GetRelevantTeams(ActorData, bool, bool)).MethodHandle;
			}
			list.Add(allyActor.GetTeam());
		}
		if (includeEnemies)
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
			list.Add(allyActor.GetOpposingTeam());
		}
		return list;
	}

	public static void SortPowerupsByDistanceToPos(ref List<PowerUp> powerups, Vector3 pos)
	{
		powerups.Sort(delegate(PowerUp x, PowerUp y)
		{
			if (x == y)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterUtils.<SortPowerupsByDistanceToPos>c__AnonStorey3.<>m__0(PowerUp, PowerUp)).MethodHandle;
				}
				return 0;
			}
			if (x == null)
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
				return -1;
			}
			if (y == null)
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
				return 1;
			}
			Vector3 a = x.boardSquare.ToVector3();
			a.y = pos.y;
			Vector3 a2 = y.boardSquare.ToVector3();
			a2.y = pos.y;
			float sqrMagnitude = (a - pos).sqrMagnitude;
			float sqrMagnitude2 = (a2 - pos).sqrMagnitude;
			return sqrMagnitude.CompareTo(sqrMagnitude2);
		});
	}

	public static void DrawGizmo_LaserBox(Vector3 startPos, Vector3 endPos, float widthInWorld, Color color)
	{
		startPos.y = (float)Board.Get().BaselineHeight;
		endPos.y = (float)Board.Get().BaselineHeight;
		Vector3 vector = endPos - startPos;
		float magnitude = vector.magnitude;
		vector.Normalize();
		Vector3 a = Vector3.Cross(vector, Vector3.up);
		Vector3 a2 = (startPos + endPos) * 0.5f;
		Vector3 b = a * (widthInWorld / 2f);
		Vector3 b2 = vector * (magnitude / 2f);
		Vector3 vector2 = a2 - b - b2;
		Vector3 vector3 = a2 - b + b2;
		Vector3 vector4 = a2 + b - b2;
		Vector3 vector5 = a2 + b + b2;
		Gizmos.color = color;
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(vector3, vector5);
		Gizmos.DrawLine(vector5, vector4);
		Gizmos.DrawLine(vector4, vector2);
		Gizmos.DrawLine(a2 - b2, a2 + b2);
	}

	public enum VariableType
	{
		MechanicPoints,
		Energy,
		HitPoints
	}

	public enum HeightAdjustType
	{
		DontAdjustHeight,
		FromCasterLoS,
		FromPathArrow,
		FromBoardSquare
	}
}
