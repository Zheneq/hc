using System.Collections.Generic;
using UnityEngine;

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
		circleHighlight.transform.position = pos + new Vector3(0f, GetHeightAdjustDelta(adjustType), 0f);
	}

	private static float GetHeightAdjustDelta(HeightAdjustType adjustType)
	{
		switch (adjustType)
		{
			case HeightAdjustType.DontAdjustHeight:
				return 0f;
			case HeightAdjustType.FromCasterLoS:
				return 0.1f - BoardSquare.s_LoSHeightOffset;
			case HeightAdjustType.FromPathArrow:
				return 0f;
			case HeightAdjustType.FromBoardSquare:
				return 0.1f;
			default:
				Log.Error("Trying to adjust the height of a laser box with an invalid HeightAdjustType: " + adjustType);
				return 0f;
		}
	}

	public static void SortActorsByDistanceToPos(ref List<ActorData> actors, Vector3 pos)
	{
		actors.Sort(delegate (ActorData a, ActorData b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			float distASqr = (a.GetFreePos() - pos).sqrMagnitude;
			float distBSqr = (b.GetFreePos() - pos).sqrMagnitude;
			if (distASqr == distBSqr)
			{
				GridPos posA = a.GetGridPos();
				GridPos posB = b.GetGridPos();
				if (posA.x != posB.x)
				{
					return posA.x.CompareTo(posB.x);
				}
				if (posA.y != posB.y)
				{
					return posA.y.CompareTo(posB.y);
				}
			}
			return distASqr.CompareTo(distBSqr);
		});
	}

	public static void SortActorsByDistanceToPos(ref List<ActorData> actors, Vector3 pos, Vector3 preferredDir)
	{
		actors.Sort(delegate (ActorData a, ActorData b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			Vector3 vecA = a.GetFreePos() - pos;
			Vector3 vecB = b.GetFreePos() - pos;
			float distASqr = vecA.sqrMagnitude;
			float distBSqr = vecB.sqrMagnitude;
			if (distASqr == distBSqr)
			{
				float angleA = Vector3.Angle(preferredDir, vecA);
				float angleB = Vector3.Angle(preferredDir, vecB);
				return angleA.CompareTo(angleB);
			}
			return distASqr.CompareTo(distBSqr);
		});
	}

	public static void SortActorsByDistanceAlongLaser(ref List<ActorData> actors, VectorUtils.LaserCoords coords)
	{
		Vector3 laserDir = coords.end - coords.start;
		laserDir.Normalize();
		SortActorsByDistanceAlongLaser(ref actors, coords.start, laserDir);
	}

	public static void SortActorsByDistanceAlongLaser(ref List<ActorData> actors, Vector3 laserStart, Vector3 laserDir)
	{
		actors.Sort(delegate (ActorData a, ActorData b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			float distA = GetSignedDistanceAlongLaser(a, laserStart, laserDir);
			float distB = GetSignedDistanceAlongLaser(b, laserStart, laserDir);
			return distA.CompareTo(distB);
		});
	}

	public static float GetSignedDistanceAlongLaser(ActorData actor, Vector3 laserStart, Vector3 laserDir)
	{
		Vector3 vec = new Vector3(actor.GetFreePos().x - laserStart.x, 0f, actor.GetFreePos().z - laserStart.z);
		return Vector3.Dot(vec, laserDir);
	}

	public static void RemoveActorsInvisibleToClient(ref List<ActorData> actors)
	{
		for (int i = actors.Count - 1; i >= 0; i--)
		{
			if (!actors[i].IsActorVisibleToClient())
			{
				actors.RemoveAt(i);
			}
		}
	}

	public static void RemoveActorsInvisibleToActor(ref List<ActorData> actors, ActorData observer)
	{
		if (actors != null)
		{
			for (int i = actors.Count - 1; i >= 0; i--)
			{
				if (!actors[i].IsActorVisibleToActor(observer))
				{
					actors.RemoveAt(i);
				}
			}
		}
	}

	public static void RemoveActorsWithoutLosToSquare(ref List<ActorData> actors, BoardSquare sourceSquare, ActorData caster)
	{
		if (sourceSquare != null
			&& actors != null
			&& caster != null)
		{
			for (int i = actors.Count - 1; i >= 0; i--)
			{
				BoardSquare square = actors[i].GetCurrentBoardSquare();
				if (square != null && !AreaEffectUtils.SquaresHaveLoSForAbilities(sourceSquare, square, caster))
				{
					actors.RemoveAt(i);
				}
			}
		}
	}

	public static void LimitActorsToMaxNumber(ref List<ActorData> actors, int max)
	{
		if (actors.Count > max && max > 0)
		{
			actors.RemoveRange(max, actors.Count - max);
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
		return new VectorUtils.LaserCoords
		{
			start = coords.start,
			end = end
		};
	}

	public static VectorUtils.LaserCoords TrimTargetsAndGetLaserCoordsToFarthestTarget(ref List<ActorData> actorsInRange, int maxTargets, VectorUtils.LaserCoords coords)
	{
		if (actorsInRange.Count > maxTargets && maxTargets > 0)
		{
			SortActorsByDistanceAlongLaser(ref actorsInRange, coords);
			LimitActorsToMaxNumber(ref actorsInRange, maxTargets);
			return GetLaserCoordsToFarthestTarget(coords, actorsInRange);
		}
		else if (actorsInRange.Count == maxTargets && maxTargets > 0)
		{
			return GetLaserCoordsToFarthestTarget(coords, actorsInRange);
		}
		return coords;
	}

	public static Vector3 GetEndPointAndLimitToFurthestSquare(Vector3 startPos, Vector3 endPos, float widthInSquares, float maxDistanceInSquares, bool penetrateLos, ActorData targetingActor, float thresholdInSquares = 0.71f)
	{
		endPos.y = startPos.y;
		if (startPos == endPos)
		{
			return startPos;
		}
		float squareSize = Board.Get().squareSize;
		Vector3 normalized = (endPos - startPos).normalized;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(startPos, normalized, maxDistanceInSquares * squareSize, penetrateLos, targetingActor);
		Vector3 result = laserEndPoint;
		List<BoardSquare> squares = AreaEffectUtils.GetSquaresInBox(startPos, laserEndPoint, widthInSquares / 2f, penetrateLos, targetingActor);
		AreaEffectUtils.SortSquaresByDistanceToPos(ref squares, startPos);
		bool flag = false;
		for (int i = squares.Count - 1; i >= 0; i--)
		{
			if (squares[i].IsValidForGameplay())
			{
				break;
			}
			if (i > 0
				&& squares[i - 1].IsValidForGameplay()
				&& (squares[i].x == squares[i - 1].x
					|| squares[i].y == squares[i - 1].y))
			{
				flag = true;
			}
			squares.RemoveAt(i);
		}
		if (squares.Count > 0)
		{
			float length = (laserEndPoint - startPos).magnitude;
			float halfSquareSize = 0.5f * squareSize;
			bool isNarrow = widthInSquares <= 1f;
			BoardSquare lastSquare = squares[squares.Count - 1];
			Vector3 lastSquarePos = lastSquare.ToVector3();
			lastSquarePos.y = startPos.y;
			float num3 = Vector3.Dot(lastSquarePos - startPos, normalized);
			float num4 = !flag && isNarrow ? squareSize : halfSquareSize;
			float num5 = Mathf.Min(length, num3 + num4);
			if (isNarrow && squares.Count > 1)
			{
				BoardSquare boardSquare2 = squares[squares.Count - 2];
				if (boardSquare2.IsValidForGameplay())
				{
					bool intersecting = false;
					Vector3 a = Vector3.zero;
					float num7 = VectorUtils.HorizontalAngle_Deg(normalized);
					if (boardSquare2.x == lastSquare.x)
					{
						Vector3 pointOnSecond2 = lastSquarePos;
						if (num7 >= 90f && num7 <= 270f)
						{
							pointOnSecond2 -= halfSquareSize * Vector3.right;
						}
						else
						{
							pointOnSecond2 += halfSquareSize * Vector3.right;
						}
						a = VectorUtils.GetLineLineIntersection(startPos, normalized, pointOnSecond2, Vector3.forward, out intersecting);
					}
					else if (boardSquare2.y == lastSquare.y)
					{
						Vector3 pointOnSecond = lastSquarePos;
						if (num7 >= 0f && num7 <= 180f)
						{
							pointOnSecond += halfSquareSize * Vector3.forward;
						}
						else
						{
							pointOnSecond -= halfSquareSize * Vector3.forward;
						}
						a = VectorUtils.GetLineLineIntersection(startPos, normalized, pointOnSecond, Vector3.right, out intersecting);
					}
					if (intersecting)
					{
						float b = Vector3.Dot(a - startPos, normalized);
						num5 = Mathf.Max(Mathf.Min(length, num3), Mathf.Min(num5, b));
					}
				}
			}
			Vector3 vector2 = startPos + num5 * normalized;
			float magnitude2 = (laserEndPoint - vector2).magnitude;
			if (magnitude2 > thresholdInSquares * squareSize)
			{
				result = vector2;
			}
		}
		return result;
	}

	public static Vector3 MoveHighlightTowards(Vector3 goalPos, GameObject highlight, ref float currentSpeed)
	{
		Vector3 position = highlight.transform.position;
		if (position == goalPos)
		{
			currentSpeed = s_nearAcceleratingMinSpeed;
			return goalPos;
		}

		float deltaTime = Time.deltaTime;
		float distToGoSqr = (goalPos - position).sqrMagnitude;
		float acc;
		float min;
		float max;
		if (distToGoSqr >= s_farDistance)
		{
			acc = s_farAcceleration;
			min = s_farAcceleratingMinSpeed;
			max = s_farAcceleratingMaxSpeed;
		}
		else if (distToGoSqr > s_brakeDistance)
		{
			acc = s_nearAcceleration;
			min = s_nearAcceleratingMinSpeed;
			max = s_nearAcceleratingMaxSpeed;
		}
		else
		{
			acc = -s_brakeAcceleration;
			min = s_brakeMinSpeed;
			max = s_brakeMaxSpeed;
		}
		float newSpeed = Mathf.Clamp(currentSpeed + acc * deltaTime, min, max);
		float deltaDist = newSpeed * deltaTime;
		Vector3 vecToGo = goalPos - position;
		float distToGo = vecToGo.magnitude;
		if (deltaDist >= distToGo)
		{
			currentSpeed = s_nearAcceleratingMinSpeed;
			return goalPos;
		}
		else
		{
			vecToGo.Normalize();
			currentSpeed = newSpeed;
			return position + vecToGo * deltaDist;
		}
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
		powerups.Sort(delegate(PowerUp a, PowerUp b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			Vector3 posA = a.boardSquare.ToVector3();
			posA.y = pos.y;
			Vector3 posB = b.boardSquare.ToVector3();
			posB.y = pos.y;
			float distASqr = (posA - pos).sqrMagnitude;
			float distBSqr = (posB - pos).sqrMagnitude;
			return distASqr.CompareTo(distBSqr);
		});
	}

	public static void DrawGizmo_LaserBox(Vector3 startPos, Vector3 endPos, float widthInWorld, Color color)
	{
		startPos.y = Board.Get().BaselineHeight;
		endPos.y = Board.Get().BaselineHeight;
		Vector3 vec = endPos - startPos;
		float dist = vec.magnitude;
		vec.Normalize();
		Vector3 center = (startPos + endPos) * 0.5f;
		Vector3 halfWidth = Vector3.Cross(vec, Vector3.up) * (widthInWorld / 2f);
		Vector3 halfLength = vec * (dist / 2f);
		Vector3 startA = center - halfWidth - halfLength;
		Vector3 endA = center - halfWidth + halfLength;
		Vector3 startB = center + halfWidth - halfLength;
		Vector3 endB = center + halfWidth + halfLength;
		Gizmos.color = color;
		Gizmos.DrawLine(startA, endA);
		Gizmos.DrawLine(endA, endB);
		Gizmos.DrawLine(endB, startB);
		Gizmos.DrawLine(startB, startA);
		Gizmos.DrawLine(center - halfLength, center + halfLength);
	}
}
