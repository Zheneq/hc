using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class TargeterUtils
{
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

	public static GameObject CreateLaserBoxHighlight(Vector3 start, Vector3 end, float widthInSquares, HeightAdjustType adjustType)
	{
		float widthInWorld = widthInSquares * Board.Get().squareSize;
		float magnitude = (start - end).magnitude;
		GameObject gameObject = HighlightUtils.Get().CreateRectangularCursor(widthInWorld, magnitude);
		RefreshLaserBoxHighlight(gameObject, start, end, widthInSquares, adjustType);
		return gameObject;
	}

	public static void RefreshLaserBoxHighlight(GameObject boxHighlight, Vector3 start, Vector3 end, float widthInSquares, HeightAdjustType adjustType)
	{
		float widthInWorld = widthInSquares * Board.Get().squareSize;
		float magnitude = (start - end).magnitude;
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, boxHighlight);
		float heightAdjustDelta = GetHeightAdjustDelta(adjustType);
		boxHighlight.transform.position = start + new Vector3(0f, heightAdjustDelta, 0f);
		Vector3 normalized = (end - start).normalized;
		boxHighlight.transform.rotation = Quaternion.LookRotation(normalized);
	}

	public static GameObject CreateCircleHighlight(Vector3 pos, float radiusInSquares, HeightAdjustType adjustType, bool isForLocalPlayer)
	{
		float radiusInWorld = radiusInSquares * Board.Get().squareSize;
		GameObject gameObject = HighlightUtils.Get().CreateAoECursor(radiusInWorld, isForLocalPlayer);
		RefreshCircleHighlight(gameObject, pos, adjustType);
		return gameObject;
	}

	public static void RefreshCircleHighlight(GameObject circleHighlight, Vector3 pos, HeightAdjustType adjustType)
	{
		float heightAdjustDelta = GetHeightAdjustDelta(adjustType);
		circleHighlight.transform.position = pos + new Vector3(0f, heightAdjustDelta, 0f);
	}

	private static float GetHeightAdjustDelta(HeightAdjustType adjustType)
	{
		if (adjustType == HeightAdjustType.DontAdjustHeight)
		{
			return 0f;
		}
		if (adjustType == HeightAdjustType.FromCasterLoS)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return 0.1f - BoardSquare.s_LoSHeightOffset;
				}
			}
		}
		if (adjustType == HeightAdjustType.FromPathArrow)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return 0f;
				}
			}
		}
		if (adjustType == HeightAdjustType.FromBoardSquare)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return 0.1f;
				}
			}
		}
		Log.Error("Trying to adjust the height of a laser box with an invalid HeightAdjustType: " + adjustType);
		return 0f;
	}

	public static void SortActorsByDistanceToPos(ref List<ActorData> actors, Vector3 pos)
	{
		actors.Sort(delegate(ActorData x, ActorData y)
		{
			if (x == y)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return 0;
					}
				}
			}
			if (x == null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return -1;
					}
				}
			}
			if (y == null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return 1;
					}
				}
			}
			float sqrMagnitude = (x.GetTravelBoardSquareWorldPosition() - pos).sqrMagnitude;
			float sqrMagnitude2 = (y.GetTravelBoardSquareWorldPosition() - pos).sqrMagnitude;
			if (sqrMagnitude == sqrMagnitude2)
			{
				GridPos gridPosWithIncrementedHeight = x.GetGridPosWithIncrementedHeight();
				GridPos gridPosWithIncrementedHeight2 = y.GetGridPosWithIncrementedHeight();
				if (gridPosWithIncrementedHeight.x != gridPosWithIncrementedHeight2.x)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return gridPosWithIncrementedHeight.x.CompareTo(gridPosWithIncrementedHeight2.x);
						}
					}
				}
				if (gridPosWithIncrementedHeight.y != gridPosWithIncrementedHeight2.y)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return gridPosWithIncrementedHeight.y.CompareTo(gridPosWithIncrementedHeight2.y);
						}
					}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
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
					switch (5)
					{
					case 0:
						break;
					default:
						return 1;
					}
				}
			}
			Vector3 to = x.GetTravelBoardSquareWorldPosition() - pos;
			Vector3 to2 = y.GetTravelBoardSquareWorldPosition() - pos;
			float sqrMagnitude = to.sqrMagnitude;
			float sqrMagnitude2 = to2.sqrMagnitude;
			if (sqrMagnitude == sqrMagnitude2)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						float num = Vector3.Angle(preferredDir, to);
						float value = Vector3.Angle(preferredDir, to2);
						return num.CompareTo(value);
					}
					}
				}
			}
			return sqrMagnitude.CompareTo(sqrMagnitude2);
		});
	}

	public static void SortActorsByDistanceAlongLaser(ref List<ActorData> actors, VectorUtils.LaserCoords coords)
	{
		Vector3 start = coords.start;
		Vector3 laserDir = coords.end - coords.start;
		laserDir.Normalize();
		SortActorsByDistanceAlongLaser(ref actors, start, laserDir);
	}

	public static void SortActorsByDistanceAlongLaser(ref List<ActorData> actors, Vector3 laserStart, Vector3 laserDir)
	{
		actors.Sort(delegate(ActorData a, ActorData b)
		{
			if (a == b)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return 0;
					}
				}
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return 1;
					}
				}
			}
			float signedDistanceAlongLaser = GetSignedDistanceAlongLaser(a, laserStart, laserDir);
			float signedDistanceAlongLaser2 = GetSignedDistanceAlongLaser(b, laserStart, laserDir);
			return signedDistanceAlongLaser.CompareTo(signedDistanceAlongLaser2);
		});
	}

	public static float GetSignedDistanceAlongLaser(ActorData actor, Vector3 laserStart, Vector3 laserDir)
	{
		Vector3 travelBoardSquareWorldPosition = actor.GetTravelBoardSquareWorldPosition();
		float x = travelBoardSquareWorldPosition.x - laserStart.x;
		Vector3 travelBoardSquareWorldPosition2 = actor.GetTravelBoardSquareWorldPosition();
		Vector3 lhs = new Vector3(x, 0f, travelBoardSquareWorldPosition2.z - laserStart.z);
		return Vector3.Dot(lhs, laserDir);
	}

	public static void RemoveActorsInvisibleToClient(ref List<ActorData> actors)
	{
		if (NetworkServer.active)
		{
			return;
		}
		for (int num = actors.Count - 1; num >= 0; num--)
		{
			if (!actors[num].IsVisibleToClient())
			{
				actors.RemoveAt(num);
			}
		}
	}

	public static void RemoveActorsInvisibleToActor(ref List<ActorData> actors, ActorData observer)
	{
		if (actors == null)
		{
			return;
		}
		while (true)
		{
			for (int num = actors.Count - 1; num >= 0; num--)
			{
				if (!actors[num].IsActorVisibleToActor(observer))
				{
					actors.RemoveAt(num);
				}
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public static void RemoveActorsWithoutLosToSquare(ref List<ActorData> actors, BoardSquare sourceSquare, ActorData caster)
	{
		if (!(sourceSquare != null))
		{
			return;
		}
		while (true)
		{
			if (actors == null)
			{
				return;
			}
			while (true)
			{
				if (!(caster != null))
				{
					return;
				}
				while (true)
				{
					for (int num = actors.Count - 1; num >= 0; num--)
					{
						BoardSquare currentBoardSquare = actors[num].GetCurrentBoardSquare();
						if (currentBoardSquare != null && !AreaEffectUtils.SquaresHaveLoSForAbilities(sourceSquare, currentBoardSquare, caster))
						{
							actors.RemoveAt(num);
						}
					}
					while (true)
					{
						switch (3)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
			}
		}
	}

	public static void LimitActorsToMaxNumber(ref List<ActorData> actors, int max)
	{
		if (actors.Count <= max)
		{
			return;
		}
		while (true)
		{
			if (max > 0)
			{
				while (true)
				{
					int count = actors.Count - max;
					actors.RemoveRange(max, count);
					return;
				}
			}
			return;
		}
	}

	public static VectorUtils.LaserCoords GetLaserCoordsToFarthestTarget(VectorUtils.LaserCoords coords, List<ActorData> targets)
	{
		float num = 0f;
		Vector3 end = coords.start;
		Vector3 start = coords.start;
		Vector3 vector = coords.end - coords.start;
		vector.Normalize();
		foreach (ActorData target in targets)
		{
			float signedDistanceAlongLaser = GetSignedDistanceAlongLaser(target, start, vector);
			if (signedDistanceAlongLaser > num)
			{
				num = signedDistanceAlongLaser;
				end = start + vector * num;
			}
		}
		VectorUtils.LaserCoords result = default(VectorUtils.LaserCoords);
		result.start = coords.start;
		result.end = end;
		return result;
	}

	public static VectorUtils.LaserCoords TrimTargetsAndGetLaserCoordsToFarthestTarget(ref List<ActorData> actorsInRange, int maxTargets, VectorUtils.LaserCoords coords)
	{
		VectorUtils.LaserCoords result = coords;
		if (actorsInRange.Count > maxTargets)
		{
			if (maxTargets > 0)
			{
				SortActorsByDistanceAlongLaser(ref actorsInRange, coords);
				LimitActorsToMaxNumber(ref actorsInRange, maxTargets);
				result = GetLaserCoordsToFarthestTarget(coords, actorsInRange);
				goto IL_0077;
			}
		}
		if (actorsInRange.Count == maxTargets)
		{
			if (maxTargets > 0)
			{
				result = GetLaserCoordsToFarthestTarget(coords, actorsInRange);
			}
		}
		goto IL_0077;
		IL_0077:
		return result;
	}

	public static Vector3 GetEndPointAndLimitToFurthestSquare(Vector3 startPos, Vector3 endPos, float widthInSquares, float maxDistanceInSquares, bool penetrateLos, ActorData targetingActor, float thresholdInSquares = 0.71f)
	{
		endPos.y = startPos.y;
		if (startPos == endPos)
		{
			while (true)
			{
				return startPos;
			}
		}
		float squareSize = Board.Get().squareSize;
		Vector3 normalized = (endPos - startPos).normalized;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(startPos, normalized, maxDistanceInSquares * squareSize, penetrateLos, targetingActor);
		Vector3 result = laserEndPoint;
		List<BoardSquare> squares = AreaEffectUtils.GetSquaresInBox(startPos, laserEndPoint, widthInSquares / 2f, penetrateLos, targetingActor);
		AreaEffectUtils.SortSquaresByDistanceToPos(ref squares, startPos);
		bool flag = false;
		for (int num = squares.Count - 1; num >= 0; squares.RemoveAt(num), num--)
		{
			if (squares[num].IsBaselineHeight())
			{
				break;
			}
			if (num <= 0)
			{
				continue;
			}
			if (!squares[num - 1].IsBaselineHeight())
			{
				continue;
			}
			if (squares[num].x != squares[num - 1].x)
			{
				if (squares[num].y != squares[num - 1].y)
				{
					continue;
				}
			}
			flag = true;
		}
		float magnitude;
		bool flag2;
		BoardSquare boardSquare;
		Vector3 vector;
		float num3;
		float num4;
		if (squares.Count > 0)
		{
			magnitude = (laserEndPoint - startPos).magnitude;
			float num2 = 0.5f * squareSize;
			flag2 = (widthInSquares <= 1f);
			boardSquare = squares[squares.Count - 1];
			vector = boardSquare.ToVector3();
			vector.y = startPos.y;
			Vector3 lhs = vector - startPos;
			num3 = Vector3.Dot(lhs, normalized);
			if (!flag)
			{
				if (flag2)
				{
					num4 = squareSize;
					goto IL_0208;
				}
			}
			num4 = num2;
			goto IL_0208;
		}
		goto IL_0420;
		IL_03ae:
		Vector3 pointOnSecond;
		Vector3 right;
		bool intersecting;
		Vector3 a = VectorUtils.GetLineLineIntersection(startPos, normalized, pointOnSecond, right, out intersecting);
		goto IL_03bd;
		IL_03ed:
		float num5;
		Vector3 vector2 = startPos + num5 * normalized;
		float magnitude2 = (laserEndPoint - vector2).magnitude;
		if (magnitude2 > thresholdInSquares * squareSize)
		{
			result = vector2;
		}
		goto IL_0420;
		IL_0310:
		Vector3 pointOnSecond2;
		Vector3 forward;
		a = VectorUtils.GetLineLineIntersection(startPos, normalized, pointOnSecond2, forward, out intersecting);
		goto IL_03bd;
		IL_03bd:
		float a2;
		if (intersecting)
		{
			float b = Vector3.Dot(a - startPos, normalized);
			num5 = Mathf.Min(num5, b);
			num5 = Mathf.Max(a2, num5);
		}
		goto IL_03ed;
		IL_0420:
		return result;
		IL_0208:
		float num6 = num4;
		num5 = Mathf.Min(magnitude, num3 + num6);
		if (flag2 && squares.Count > 1)
		{
			float d = 0.5f * squareSize;
			a2 = Mathf.Min(magnitude, num3);
			BoardSquare boardSquare2 = squares[squares.Count - 2];
			if (boardSquare2.IsBaselineHeight())
			{
				intersecting = false;
				a = Vector3.zero;
				float num7 = VectorUtils.HorizontalAngle_Deg(normalized);
				if (boardSquare2.x == boardSquare.x)
				{
					forward = Vector3.forward;
					pointOnSecond2 = vector;
					if (num7 >= 90f)
					{
						if (num7 <= 270f)
						{
							pointOnSecond2 -= d * Vector3.right;
							goto IL_0310;
						}
					}
					pointOnSecond2 += d * Vector3.right;
					goto IL_0310;
				}
				if (boardSquare2.y == boardSquare.y)
				{
					right = Vector3.right;
					pointOnSecond = vector;
					if (num7 >= 0f)
					{
						if (num7 <= 180f)
						{
							pointOnSecond += d * Vector3.forward;
							goto IL_03ae;
						}
					}
					pointOnSecond -= d * Vector3.forward;
					goto IL_03ae;
				}
				goto IL_03bd;
			}
		}
		goto IL_03ed;
	}

	public static Vector3 MoveHighlightTowards(Vector3 goalPos, GameObject highlight, ref float currentSpeed)
	{
		Vector3 position = highlight.transform.position;
		if (position == goalPos)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					currentSpeed = s_nearAcceleratingMinSpeed;
					return goalPos;
				}
			}
		}
		float deltaTime = Time.deltaTime;
		float sqrMagnitude = (goalPos - position).sqrMagnitude;
		bool flag = sqrMagnitude >= s_farDistance;
		bool flag2 = sqrMagnitude <= s_brakeDistance;
		float num;
		float min;
		float max;
		if (flag)
		{
			num = s_farAcceleration;
			min = s_farAcceleratingMinSpeed;
			max = s_farAcceleratingMaxSpeed;
		}
		else if (!flag2)
		{
			num = s_nearAcceleration;
			min = s_nearAcceleratingMinSpeed;
			max = s_nearAcceleratingMaxSpeed;
		}
		else
		{
			num = 0f - s_brakeAcceleration;
			min = s_brakeMinSpeed;
			max = s_brakeMaxSpeed;
		}
		float value = currentSpeed + num * deltaTime;
		value = Mathf.Clamp(value, min, max);
		float num2 = value * deltaTime;
		Vector3 a = goalPos - position;
		float magnitude = a.magnitude;
		if (num2 >= magnitude)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					currentSpeed = s_nearAcceleratingMinSpeed;
					return goalPos;
				}
			}
		}
		a.Normalize();
		currentSpeed = value;
		return position + a * num2;
	}

	public static List<Team> GetRelevantTeams(ActorData allyActor, bool includeAllies, bool includeEnemies)
	{
		List<Team> list = new List<Team>();
		if (includeAllies)
		{
			list.Add(allyActor.GetTeam());
		}
		if (includeEnemies)
		{
			list.Add(allyActor.GetEnemyTeam());
		}
		return list;
	}

	public static void SortPowerupsByDistanceToPos(ref List<PowerUp> powerups, Vector3 pos)
	{
		powerups.Sort(delegate(PowerUp x, PowerUp y)
		{
			if (x == y)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return 0;
					}
				}
			}
			if (x == null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return -1;
					}
				}
			}
			if (y == null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return 1;
					}
				}
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
		startPos.y = Board.Get().BaselineHeight;
		endPos.y = Board.Get().BaselineHeight;
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
}
