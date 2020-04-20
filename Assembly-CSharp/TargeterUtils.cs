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
			result = 0.1f - BoardSquare.s_LoSHeightOffset;
		}
		else if (adjustType == TargeterUtils.HeightAdjustType.FromPathArrow)
		{
			result = 0f;
		}
		else if (adjustType == TargeterUtils.HeightAdjustType.FromBoardSquare)
		{
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
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			float sqrMagnitude = (x.GetTravelBoardSquareWorldPosition() - pos).sqrMagnitude;
			float sqrMagnitude2 = (y.GetTravelBoardSquareWorldPosition() - pos).sqrMagnitude;
			if (sqrMagnitude == sqrMagnitude2)
			{
				GridPos gridPosWithIncrementedHeight = x.GetGridPosWithIncrementedHeight();
				GridPos gridPosWithIncrementedHeight2 = y.GetGridPosWithIncrementedHeight();
				if (gridPosWithIncrementedHeight.x != gridPosWithIncrementedHeight2.x)
				{
					return gridPosWithIncrementedHeight.x.CompareTo(gridPosWithIncrementedHeight2.x);
				}
				if (gridPosWithIncrementedHeight.y != gridPosWithIncrementedHeight2.y)
				{
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
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			Vector3 to = x.GetTravelBoardSquareWorldPosition() - pos;
			Vector3 to2 = y.GetTravelBoardSquareWorldPosition() - pos;
			float sqrMagnitude = to.sqrMagnitude;
			float sqrMagnitude2 = to2.sqrMagnitude;
			if (sqrMagnitude == sqrMagnitude2)
			{
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
				actors.RemoveAt(i);
			}
		}
	}

	public unsafe static void RemoveActorsInvisibleToActor(ref List<ActorData> actors, ActorData observer)
	{
		if (actors != null)
		{
			for (int i = actors.Count - 1; i >= 0; i--)
			{
				if (!actors[i].IsActorVisibleToActor(observer, false))
				{
					actors.RemoveAt(i);
				}
			}
		}
	}

	public unsafe static void RemoveActorsWithoutLosToSquare(ref List<ActorData> actors, BoardSquare sourceSquare, ActorData caster)
	{
		if (sourceSquare != null)
		{
			if (actors != null)
			{
				if (caster != null)
				{
					for (int i = actors.Count - 1; i >= 0; i--)
					{
						BoardSquare currentBoardSquare = actors[i].GetCurrentBoardSquare();
						if (currentBoardSquare != null && !AreaEffectUtils.SquaresHaveLoSForAbilities(sourceSquare, currentBoardSquare, caster, true, null))
						{
							actors.RemoveAt(i);
						}
					}
				}
			}
		}
	}

	public unsafe static void LimitActorsToMaxNumber(ref List<ActorData> actors, int max)
	{
		if (actors.Count > max)
		{
			if (max > 0)
			{
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
			if (maxTargets > 0)
			{
				TargeterUtils.SortActorsByDistanceAlongLaser(ref actorsInRange, coords);
				TargeterUtils.LimitActorsToMaxNumber(ref actorsInRange, maxTargets);
				return TargeterUtils.GetLaserCoordsToFarthestTarget(coords, actorsInRange);
			}
		}
		if (actorsInRange.Count == maxTargets)
		{
			if (maxTargets > 0)
			{
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
				break;
			}
			if (i > 0)
			{
				if (squaresInBox[i - 1].IsBaselineHeight())
				{
					if (squaresInBox[i].x != squaresInBox[i - 1].x)
					{
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
				if (flag2)
				{
					num3 = squareSize;
					goto IL_208;
				}
			}
			num3 = num;
			IL_208:
			float num4 = num3;
			float num5 = Mathf.Min(magnitude, num2 + num4);
			if (flag2 && squaresInBox.Count > 1)
			{
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
						Vector3 forward = Vector3.forward;
						Vector3 vector2 = vector;
						if (num6 >= 90f)
						{
							if (num6 <= 270f)
							{
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
						Vector3 right = Vector3.right;
						Vector3 vector3 = vector;
						if (num6 >= 0f)
						{
							if (num6 <= 180f)
							{
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
			list.Add(allyActor.GetTeam());
		}
		if (includeEnemies)
		{
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
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
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
