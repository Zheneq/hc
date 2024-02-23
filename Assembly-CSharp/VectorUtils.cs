using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class VectorUtils
{
	public struct LaserCoords
	{
		public Vector3 start;

		public Vector3 end;

		public float Length()
		{
			return Vector3.Magnitude(start - end);
		}

		public Vector3 Direction()
		{
			return (end - start).normalized;
		}
	}

	private static float s_positionOffset = 0.3f;

	public static float s_laserOffset = 0.3f;

	private static float s_laserInitialLengthOffset = 0.71f;

	public static int s_raycastLayerLineOfSight = LayerMask.NameToLayer("LineOfSight");

	public static int s_raycastLayerDynamicLineOfSight = LayerMask.NameToLayer("DynamicLineOfSight");

	public static ActorCover.CoverDirections GetCoverDirection(BoardSquare srcSquare, BoardSquare destSquare)
	{
		int x = srcSquare.x;
		int y = srcSquare.y;
		int x2 = destSquare.x;
		int y2 = destSquare.y;
		if (Mathf.Abs(x - x2) > Mathf.Abs(y - y2))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (x > x2)
					{
						return ActorCover.CoverDirections.X_NEG;
					}
					return ActorCover.CoverDirections.X_POS;
				}
			}
		}
		if (y > y2)
		{
			return ActorCover.CoverDirections.Y_NEG;
		}
		return ActorCover.CoverDirections.Y_POS;
	}

	private static BoardSquare GetAdjSquare(BoardSquare square, ActorCover.CoverDirections direction)
	{
		BoardSquare result = null;
		switch (direction)
		{
		case ActorCover.CoverDirections.X_POS:
			result = Board.Get().GetSquareFromIndex(square.x + 1, square.y);
			break;
		case ActorCover.CoverDirections.X_NEG:
			result = Board.Get().GetSquareFromIndex(square.x - 1, square.y);
			break;
		case ActorCover.CoverDirections.Y_POS:
			result = Board.Get().GetSquareFromIndex(square.x, square.y + 1);
			break;
		case ActorCover.CoverDirections.Y_NEG:
			result = Board.Get().GetSquareFromIndex(square.x, square.y - 1);
			break;
		}
		return result;
	}

	public static bool HasCoverInDirection(BoardSquare square, ActorCover.CoverDirections coverDirection)
	{
		bool result = false;
		BoardSquare adjSquare = GetAdjSquare(square, coverDirection);
		if (adjSquare != null)
		{
			if ((float)(adjSquare.height - square.height) > 1f)
			{
				result = true;
			}
		}
		return result;
	}

	private static bool IsSuitableAdditionalCoverSquare(BoardSquare src, ActorCover.CoverDirections adjDirection, ActorCover.CoverDirections coverDirection)
	{
		bool result = false;
		if (!HasCoverInDirection(src, adjDirection))
		{
			BoardSquare adjSquare = GetAdjSquare(src, adjDirection);
			if (adjSquare != null && !HasCoverInDirection(adjSquare, coverDirection))
			{
				result = true;
			}
		}
		return result;
	}

	private static List<BoardSquare> GetAdditionalCoverSquares(BoardSquare src, BoardSquare dst)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		ActorCover.CoverDirections coverDirection = GetCoverDirection(src, dst);
		list.Add(src);
		if (HasCoverInDirection(src, coverDirection))
		{
			if (coverDirection != ActorCover.CoverDirections.X_NEG)
			{
				if (coverDirection != 0)
				{
					if (coverDirection != ActorCover.CoverDirections.Y_NEG)
					{
						if (coverDirection != ActorCover.CoverDirections.Y_POS)
						{
							goto IL_00ed;
						}
					}
					if (IsSuitableAdditionalCoverSquare(src, ActorCover.CoverDirections.X_NEG, coverDirection))
					{
						list.Add(GetAdjSquare(src, ActorCover.CoverDirections.X_NEG));
					}
					if (IsSuitableAdditionalCoverSquare(src, ActorCover.CoverDirections.X_POS, coverDirection))
					{
						list.Add(GetAdjSquare(src, ActorCover.CoverDirections.X_POS));
					}
					goto IL_00ed;
				}
			}
			if (IsSuitableAdditionalCoverSquare(src, ActorCover.CoverDirections.Y_NEG, coverDirection))
			{
				list.Add(GetAdjSquare(src, ActorCover.CoverDirections.Y_NEG));
			}
			if (IsSuitableAdditionalCoverSquare(src, ActorCover.CoverDirections.Y_POS, coverDirection))
			{
				list.Add(GetAdjSquare(src, ActorCover.CoverDirections.Y_POS));
			}
		}
		goto IL_00ed;
		IL_00ed:
		return list;
	}

	public static bool HasLineOfSightFromIndex(int startX, int startY, int endX, int endY, Board board, float heightOffset, string layerName)
	{
		float squareSize = board.squareSize;
		float y;
		if (board.GetHeightAt(startX, startY) < 0f)
		{
			y = heightOffset + (float)board.BaselineHeight;
		}
		else
		{
			y = heightOffset + board.GetHeightAt(startX, startY);
		}
		float y2;
		if (board.GetHeightAt(endX, endY) < 0f)
		{
			y2 = heightOffset + (float)board.BaselineHeight;
		}
		else
		{
			y2 = heightOffset + board.GetHeightAt(endX, endY);
		}
		Vector3 vector = new Vector3((float)startX * squareSize, y, (float)startY * squareSize);
		Vector3 vector2 = new Vector3((float)endX * squareSize, y2, (float)endY * squareSize);
		Vector3[] array = new Vector3[3];
		Vector3[] array2 = new Vector3[3];
		bool flag = false;
		if (Mathf.Abs(startX - endX) > Mathf.Abs(startY - endY))
		{
			Vector3 b = new Vector3(0f, 0f, squareSize * s_positionOffset);
			array[0] = vector - b;
			array[1] = vector;
			array[2] = vector + b;
			array2[0] = vector2 - b;
			array2[1] = vector2;
			array2[2] = vector2 + b;
		}
		else
		{
			Vector3 b2 = new Vector3(squareSize * s_positionOffset, 0f, 0f);
			array[0] = vector - b2;
			array[1] = vector;
			array[2] = vector + b2;
			array2[0] = vector2 - b2;
			array2[1] = vector2;
			array2[2] = vector2 + b2;
		}
		for (int i = 0; i < array.Length; i++)
		{
			int num = 0;
			while (true)
			{
				if (num < array2.Length)
				{
					flag = HasLineOfSight(array[i], array2[num], layerName);
					if (flag)
					{
						break;
					}
					num++;
					continue;
				}
				break;
			}
			if (flag)
			{
				break;
			}
		}
		return flag;
	}

	private static bool HasLineOfSight(Vector3 startPos, Vector3 endPos, string layerName)
	{
		Vector3 direction = endPos - startPos;
		float magnitude = direction.magnitude;
		direction.Normalize();
		LayerMask mask = (1 << LayerMask.NameToLayer(layerName)) | (1 << LayerMask.NameToLayer("DynamicLineOfSight"));
		RaycastHit hitInfo;
		return !Physics.Raycast(startPos, direction, out hitInfo, magnitude, mask);
	}

	public static float GetLineOfSightPercentDistance(int startX, int startY, int endX, int endY, Board board, float heightOffset, string layerName)
	{
		float squareSize = board.squareSize;
		float y;
		if (board.GetHeightAt(startX, startY) < 0f)
		{
			y = heightOffset + (float)board.BaselineHeight;
		}
		else
		{
			y = heightOffset + board.GetHeightAt(startX, startY);
		}
		float y2;
		if (board.GetHeightAt(endX, endY) < 0f)
		{
			y2 = heightOffset + (float)board.BaselineHeight;
		}
		else
		{
			y2 = heightOffset + board.GetHeightAt(endX, endY);
		}
		Vector3 vector = new Vector3((float)startX * squareSize, y, (float)startY * squareSize);
		Vector3 vector2 = new Vector3((float)endX * squareSize, y2, (float)endY * squareSize);
		Vector3[] array = new Vector3[3];
		Vector3[] array2 = new Vector3[3];
		if (Mathf.Abs(startX - endX) > Mathf.Abs(startY - endY))
		{
			Vector3 b = new Vector3(0f, 0f, squareSize * s_positionOffset);
			array[0] = vector - b;
			array[1] = vector;
			array[2] = vector + b;
			array2[0] = vector2 - b;
			array2[1] = vector2;
			array2[2] = vector2 + b;
		}
		else
		{
			Vector3 b2 = new Vector3(squareSize * s_positionOffset, 0f, 0f);
			array[0] = vector - b2;
			array[1] = vector;
			array[2] = vector + b2;
			array2[0] = vector2 - b2;
			array2[1] = vector2;
			array2[2] = vector2 + b2;
		}
		float num = 0f;
		int num2 = 0;
		while (true)
		{
			if (num2 < array.Length)
			{
				for (int i = 0; i < array2.Length; i++)
				{
					float lineOfSightPercentDistance = GetLineOfSightPercentDistance(array[num2], array2[i], layerName);
					if (!(lineOfSightPercentDistance > num))
					{
						continue;
					}
					num = lineOfSightPercentDistance;
					if (num == 1f)
					{
						break;
					}
				}
				if (num == 1f)
				{
					break;
				}
				num2++;
				continue;
			}
			break;
		}
		return num;
	}

	private static float GetLineOfSightPercentDistance(Vector3 startPos, Vector3 endPos, string layerName)
	{
		Vector3 direction = endPos - startPos;
		float magnitude = direction.magnitude;
		direction.Normalize();
		LayerMask mask = (1 << LayerMask.NameToLayer(layerName)) | (1 << LayerMask.NameToLayer("DynamicLineOfSight"));
		RaycastHit hitInfo;
		if (!Physics.Raycast(startPos, direction, out hitInfo, magnitude, mask))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return 1f;
				}
			}
		}
		return hitInfo.distance / magnitude;
	}

	public static LaserCoords GetLaserCoordinates(Vector3 startPos, Vector3 dir, float maxDistanceInWorld, float widthInWorld, bool penetrateLoS, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo = null)
	{
		LaserCoords result = default(LaserCoords);
		result.start = startPos;
		result.end = GetLaserEndPoint(startPos, dir, maxDistanceInWorld, penetrateLoS, caster, nonActorTargetInfo);
		return result;
	}

	public static Vector3 GetLaserEndPoint(Vector3 startPos, Vector3 dir, float maxDistanceInWorld, bool penetrateLoS, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo = null, bool checkBarriers = true)
	{
		dir.y = 0f;
		Vector3 vector;
		if (penetrateLoS)
		{
			vector = startPos + dir * maxDistanceInWorld;
			goto IL_0490;
		}
		Vector3[] array = new Vector3[3]
		{
			startPos,
			default(Vector3),
			default(Vector3)
		};
		float num = s_laserOffset * Board.Get().squareSize;
		float num2 = s_laserInitialLengthOffset * Board.Get().squareSize;
		Vector3 b = Vector3.Cross(Vector3.up, dir);
		b.Normalize();
		b *= num;
		array[1] = startPos + b;
		array[2] = startPos - b;
		float num3 = 0f;
		int num4;
		if (BarrierManager.Get() != null)
		{
			num4 = (BarrierManager.Get().HasAbilityBlockingBarriers() ? 1 : 0);
		}
		else
		{
			num4 = 0;
		}
		bool flag = (byte)num4 != 0;
		bool flag2 = true;
		bool flag3 = false;
		Vector3 a = array[0] + num2 * dir;
		if (maxDistanceInWorld > num2)
		{
			for (int i = 0; i < array.Length; i++)
			{
				Vector3 b2 = array[i];
				Vector3 dir2 = a - b2;
				float magnitude = dir2.magnitude;
				dir2.Normalize();
				RaycastHit hit;
				if (RaycastInDirection(array[i], dir2, magnitude, out hit))
				{
					flag2 = false;
					if ((hit.collider.gameObject.layer & s_raycastLayerDynamicLineOfSight) != 0)
					{
						flag3 = true;
					}
					break;
				}
			}
		}
		List<NonActorTargetInfo> list = null;
		if (!(maxDistanceInWorld <= num2))
		{
			if (flag2)
			{
				Vector3 vector2 = array[0] + num2 * dir;
				Vector3 lineEndPoint = GetLineEndPoint(vector2, dir, maxDistanceInWorld - num2);
				float num5 = (vector2 - lineEndPoint).magnitude + num2;
				float num6 = num5 * num5;
				if (num6 > num3)
				{
					num3 = num6;
				}
				goto IL_03ea;
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			object obj;
			if (nonActorTargetInfo != null)
			{
				obj = new List<NonActorTargetInfo>();
			}
			else
			{
				obj = null;
			}
			List<NonActorTargetInfo> list2 = (List<NonActorTargetInfo>)obj;
			Vector3 vector3 = array[j];
			Vector3 vector4 = GetLineEndPoint(vector3, dir, maxDistanceInWorld);
			if (checkBarriers && flag)
			{
				Vector3 vector1;
				bool bool1;
				vector4 = BarrierManager.Get().GetAbilityLineEndpoint(caster, vector3, vector4, out bool1, out vector1, list2);
			}
			float sqrMagnitude = (vector3 - vector4).sqrMagnitude;
			if (!(sqrMagnitude > num3))
			{
				continue;
			}
			Vector3 b3 = array[j] - array[0];
			Vector3 vector5 = (vector3 + vector4) / 2f - b3;
			if (flag3)
			{
				vector5 = startPos + (num2 + 0.3f) * dir;
			}
			float maxDistance = Mathf.Max(0f, (vector5 - startPos).magnitude - num2);
			Vector3 lineEndPoint2 = GetLineEndPoint(vector5, -dir, maxDistance);
			float maxDistance2 = maxDistanceInWorld - (lineEndPoint2 - startPos).magnitude;
			Vector3 lineEndPoint3 = GetLineEndPoint(lineEndPoint2, dir, maxDistance2);
			float sqrMagnitude2 = (array[0] - lineEndPoint3).sqrMagnitude;
			float num7 = Mathf.Min(sqrMagnitude, sqrMagnitude2);
			if (num7 > num3)
			{
				num3 = num7;
				list = list2;
			}
			else
			{
				if (!Mathf.Approximately(num7, num3))
				{
					continue;
				}
				if (list != null && list.Count == 0)
				{
					list = list2;
				}
			}
		}
		goto IL_03ea;
		IL_0490:
		return vector;
		IL_03ea:
		float num8 = Mathf.Sqrt(num3);
		if (num8 < maxDistanceInWorld - 0.1f)
		{
			num8 = Mathf.Max(0f, num8 - 0.05f);
		}
		vector = startPos + dir * num8;
		if (BarrierManager.Get() != null && checkBarriers)
		{
			bool collision2;
			Vector3 vector1;
			vector = BarrierManager.Get().GetAbilityLineEndpoint(caster, startPos, vector, out collision2, out vector1, nonActorTargetInfo);
			if (!collision2)
			{
				if (nonActorTargetInfo != null && list != null)
				{
					nonActorTargetInfo.AddRange(list);
				}
			}
		}
		goto IL_0490;
	}

	public static Vector3 GetLineEndPoint(Vector3 startPos, Vector3 dir, float maxDistance)
	{
		dir.Normalize();
		LayerMask mask = (1 << s_raycastLayerLineOfSight) | (1 << s_raycastLayerDynamicLineOfSight);
		RaycastHit hitInfo;
		if (Physics.Raycast(startPos, dir, out hitInfo, maxDistance, mask))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return hitInfo.point;
				}
			}
		}
		return startPos + dir * maxDistance;
	}

	public static bool RaycastInDirection(Vector3 startPos, Vector3 dir, float maxDistance, out RaycastHit hit)
	{
		dir.Normalize();
		LayerMask mask = (1 << s_raycastLayerLineOfSight) | (1 << s_raycastLayerDynamicLineOfSight);
		return Physics.Raycast(startPos, dir, out hit, maxDistance, mask);
	}

	public static bool RaycastInDirection(Vector3 startPos, Vector3 endPos, out RaycastHit hit)
	{
		Vector3 direction = endPos - startPos;
		direction.y = 0f;
		direction.Normalize();
		float magnitude = direction.magnitude;
		LayerMask mask = (1 << s_raycastLayerLineOfSight) | (1 << s_raycastLayerDynamicLineOfSight);
		return Physics.Raycast(startPos, direction, out hit, magnitude, mask);
	}

	public static Vector3 GetAdjustedStartPosWithOffset(Vector3 startPos, Vector3 endPos, float offsetInSquares)
	{
		Vector3 vector = endPos - startPos;
		vector.y = 0f;
		Vector3 result = startPos;
		if (offsetInSquares != 0f)
		{
			float d = Mathf.Min(vector.magnitude, offsetInSquares * Board.Get().squareSize);
			result = startPos + d * vector.normalized;
		}
		return result;
	}

	public static bool SquareOnSameSideAsBounceBend(BoardSquare testSquare, Vector3 bounceFromPos, Vector3 collisionNormal)
	{
		Vector3 rhs = testSquare.ToVector3() - bounceFromPos;
		rhs.y = 0f;
		return Vector3.Dot(collisionNormal, rhs) >= 0f;
	}

	public static List<Vector3> CalculateBouncingLaserEndpoints(Vector3 laserStartPos, Vector3 forwardDirection, float maxDistancePerBounceInSquares, float totalMaxDistanceInSquares, int maxBounces, ActorData caster, float widthInSquares, int maxTargets, bool includeInvisibles, List<Team> validTeams, bool bounceOnActors, out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors, out List<ActorData> orderedHitActors, List<List<NonActorTargetInfo>> nonActorTargetInfoInSegments, bool calculateLaserPastMaxTargets = false, bool skipHitsOnCaster = true)
	{
		Vector3 vector = laserStartPos;
		List<Vector3> list = new List<Vector3>();
		bounceHitActors = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
		orderedHitActors = new List<ActorData>();
		float num = Board.Get().squareSize * AreaEffectUtils.GetActorTargetingRadius();
		float num2 = maxDistancePerBounceInSquares * Board.Get().squareSize;
		float num3 = totalMaxDistanceInSquares * Board.Get().squareSize;
		Vector3 vector2 = forwardDirection;
		int num4 = 0;
		int num5 = 0;
		float num6 = 0f;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		ActorData excludeActor = null;
		float maxDistanceInWorld = Board.Get().squareSize * 1.8f;
		List<NonActorTargetInfo> list2 = new List<NonActorTargetInfo>();
		LaserCoords laserCoordinates = GetLaserCoordinates(laserStartPos, forwardDirection, maxDistanceInWorld, 0f, false, caster, list2);
		Vector3 end = laserCoordinates.end;
		Vector3 a = end - laserStartPos;
		Vector3 vector3 = a * 0.5f;
		float magnitude = vector3.magnitude;
		laserStartPos += vector3;
		num6 += magnitude;
		num2 -= magnitude;
		int num8;
		for (int i = 0; !flag3; flag3 = ((byte)num8 != 0), i++)
		{
			bool flag4 = nonActorTargetInfoInSegments != null;
			if (nonActorTargetInfoInSegments != null)
			{
				nonActorTargetInfoInSegments.Add(new List<NonActorTargetInfo>());
			}
			vector2.Normalize();
			float maxDistance = Mathf.Min(num2, num3 - num6);
			num2 = maxDistancePerBounceInSquares * Board.Get().squareSize;
			Vector3 collisionNormal = Vector3.zero;
			Vector3 endPoint = Vector3.zero;
			bool collisionWithGeo = false;
			Vector3 prevStartPos = Vector3.zero;
			if (i == 1)
			{
				prevStartPos = vector;
			}
			else if (i > 1)
			{
				prevStartPos = list[i - 2];
			}
			Vector3 startPosForBounce = laserStartPos;
			Vector3 startPosForGameplay;
			if (i == 0)
			{
				startPosForGameplay = vector;
			}
			else
			{
				startPosForGameplay = laserStartPos;
			}
			Vector3 dir = vector2;
			object nonActorTargetInfo;
			if (flag4)
			{
				nonActorTargetInfo = nonActorTargetInfoInSegments[i];
			}
			else
			{
				nonActorTargetInfo = null;
			}
			bool hitActorFirst;
			ActorData bounceHitActor;
			List<ActorData> list3 = CalculateLaserBounce(startPosForBounce, startPosForGameplay, dir, maxDistance, caster, out endPoint, out collisionWithGeo, out collisionNormal, widthInSquares, validTeams, (List<NonActorTargetInfo>)nonActorTargetInfo, includeInvisibles, i, prevStartPos, bounceOnActors, excludeActor, out hitActorFirst, out bounceHitActor, skipHitsOnCaster);
			excludeActor = bounceHitActor;
			using (List<ActorData>.Enumerator enumerator = list3.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (!bounceHitActors.ContainsKey(current))
					{
						if (maxTargets > 0)
						{
							if (orderedHitActors.Count >= maxTargets)
							{
								continue;
							}
						}
						AreaEffectUtils.BouncingLaserInfo value = new AreaEffectUtils.BouncingLaserInfo((i != 0) ? laserStartPos : vector, i);
						bounceHitActors.Add(current, value);
						orderedHitActors.Add(current);
					}
				}
			}
			int num7;
			if (maxTargets > 0 && orderedHitActors.Count >= maxTargets)
			{
				num7 = ((!calculateLaserPastMaxTargets) ? 1 : 0);
			}
			else
			{
				num7 = 0;
			}
			bool flag5 = (byte)num7 != 0;
			if (flag5)
			{
				Vector3 normalized = (endPoint - laserStartPos).normalized;
				Vector3 rhs = orderedHitActors[orderedHitActors.Count - 1].GetFreePos() - laserStartPos;
				endPoint = laserStartPos + (Vector3.Dot(normalized, rhs) + num) * normalized;
				if (flag4)
				{
					nonActorTargetInfoInSegments[i].Clear();
					flag4 = false;
				}
			}
			if (flag4)
			{
				if (list.Count == 0 && list2.Count > 0)
				{
					using (List<NonActorTargetInfo>.Enumerator enumerator2 = list2.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							NonActorTargetInfo current2 = enumerator2.Current;
							nonActorTargetInfoInSegments[i].Add(current2);
						}
					}
				}
			}
			list.Add(endPoint);
			float magnitude2 = (laserStartPos - endPoint).magnitude;
			num6 += magnitude2;
			if (num6 >= num3 - 0.01f)
			{
				flag2 = true;
			}
			if (hitActorFirst)
			{
				num4++;
				num5++;
				laserStartPos = endPoint;
				vector2 -= 2f * Vector3.Dot(vector2, collisionNormal) * collisionNormal;
			}
			else if (collisionWithGeo)
			{
				num4++;
				laserStartPos = endPoint;
				vector2 -= 2f * Vector3.Dot(vector2, collisionNormal) * collisionNormal;
			}
			else
			{
				flag = true;
			}
			if (!flag && !flag2)
			{
				if (num4 <= maxBounces)
				{
					num8 = (flag5 ? 1 : 0);
					continue;
				}
			}
			num8 = 1;
		}
		return list;
	}

	public static List<ActorData> CalculateLaserBounce(Vector3 startPosForBounce, Vector3 startPosForGameplay, Vector3 dir, float maxDistance, ActorData caster, out Vector3 endPoint, out bool collisionWithGeo, out Vector3 collisionNormal, float widthInSquares, List<Team> validTeams, List<NonActorTargetInfo> nonActorTargetInfo, bool includeInvisibles, int segmentIndex, Vector3 prevStartPos, bool bounceOnActors, ActorData excludeActor, out bool hitActorFirst, out ActorData bounceHitActor, bool skipHitsOnCaster = true)
	{
		LayerMask mask = (1 << LayerMask.NameToLayer("LineOfSight")) | (1 << LayerMask.NameToLayer("DynamicLineOfSight"));
		RaycastHit hitInfo;
		collisionWithGeo = Physics.Raycast(startPosForBounce, dir, out hitInfo, maxDistance, mask);
		if (collisionWithGeo)
		{
			Vector3 a = hitInfo.point - startPosForBounce;
			a.y = 0f;
			float magnitude = a.magnitude;
			a.Normalize();
			endPoint = startPosForBounce + a * Mathf.Max(0f, magnitude - 0.1f);
			collisionNormal = hitInfo.normal;
		}
		else
		{
			endPoint = startPosForBounce + dir * maxDistance;
			collisionNormal = Vector3.zero;
		}
		if (BarrierManager.Get() != null)
		{
			bool collision;
			Vector3 collisionNormal2;
			Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(caster, startPosForBounce, endPoint, out collision, out collisionNormal2, nonActorTargetInfo);
			if (collision)
			{
				endPoint = abilityLineEndpoint;
				collisionWithGeo = true;
				collisionNormal = collisionNormal2;
			}
		}
		List<ActorData> list;
		if (GameWideData.Get().UseActorRadiusForLaser())
		{
			list = AreaEffectUtils.GetActorsInBoxByActorRadius(startPosForGameplay, endPoint, widthInSquares, false, caster, validTeams);
		}
		else
		{
			list = AreaEffectUtils.GetActorsInBox(startPosForGameplay, endPoint, widthInSquares, true, caster, validTeams);
		}
		List<ActorData> actors = list;
		if (skipHitsOnCaster)
		{
			actors.Remove(caster);
		}
		if (!includeInvisibles)
		{
			if (NetworkServer.active)
			{
				TargeterUtils.RemoveActorsInvisibleToActor(ref actors, caster);
			}
			else
			{
				TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			}
		}
		if (excludeActor != null)
		{
			actors.Remove(excludeActor);
		}
		Vector3 a2 = endPoint - startPosForBounce;
		a2.y = 0f;
		a2.Normalize();
		if (segmentIndex > 0)
		{
			if (actors.Count > 0)
			{
				Vector3 b = prevStartPos - startPosForBounce;
				b.y = 0f;
				b.Normalize();
				Vector3 collisionNormal3 = 0.5f * (a2 + b);
				for (int num = actors.Count - 1; num >= 0; num--)
				{
					BoardSquare currentBoardSquare = actors[num].GetCurrentBoardSquare();
					if (!SquareOnSameSideAsBounceBend(currentBoardSquare, startPosForBounce, collisionNormal3))
					{
						actors.RemoveAt(num);
					}
				}
			}
		}
		TargeterUtils.SortActorsByDistanceToPos(ref actors, startPosForBounce);
		hitActorFirst = false;
		bounceHitActor = null;
		if (bounceOnActors)
		{
			int num2 = 0;
			float num3 = GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
			for (int i = 0; i < actors.Count; i++)
			{
				if (!hitActorFirst)
				{
					ActorData actorData = actors[i];
					Vector3 vector = actorData.GetFreePos() - startPosForBounce;
					vector.y = 0f;
					float magnitude2 = vector.magnitude;
					Vector3 vector2 = endPoint - startPosForBounce;
					vector2.y = 0f;
					float magnitude3 = vector2.magnitude;
					if (!(magnitude2 < magnitude3))
					{
						continue;
					}
					Vector3 travelBoardSquareWorldPosition = actorData.GetFreePos();
					float num4 = 0.5f * widthInSquares * Board.Get().squareSize;
					if (GameWideData.Get().UseActorRadiusForLaser())
					{
						num4 += num3;
					}
					num4 = Mathf.Min(0.4f * Board.Get().squareSize, num4);
					Vector3 intersectP;
					Vector3 intersectP2;
					int lineCircleIntersections = GetLineCircleIntersections(startPosForGameplay, endPoint, travelBoardSquareWorldPosition, num4, out intersectP, out intersectP2);
					if (lineCircleIntersections <= 1)
					{
						continue;
					}
					float num5 = HorizontalPlaneDistInWorld(intersectP, startPosForGameplay);
					float num6 = HorizontalPlaneDistInWorld(intersectP2, startPosForGameplay);
					Vector3 vector3;
					if (num5 <= num6)
					{
						vector3 = intersectP;
					}
					else
					{
						vector3 = intersectP2;
					}
					endPoint = vector3;
					endPoint.y = startPosForBounce.y;
					collisionNormal = endPoint - travelBoardSquareWorldPosition;
					collisionNormal.y = 0f;
					collisionNormal.Normalize();
					float num7 = 0.5f * AreaEffectUtils.GetMaxAngleForActorBounce();
					collisionNormal = Vector3.RotateTowards(-a2, collisionNormal, (float)Math.PI / 180f * num7, 0f);
					hitActorFirst = true;
					collisionWithGeo = false;
					bounceHitActor = actorData;
					num2 = i;
					continue;
				}
				break;
			}
			if (hitActorFirst)
			{
				TargeterUtils.LimitActorsToMaxNumber(ref actors, num2 + 1);
			}
		}
		return actors;
	}

	public static List<Vector3> CalculateBouncingActorEndpoints(Vector3 laserStartPos, Vector3 forwardDirection, float maxDistancePerBounceInSquares, float totalMaxDistanceInSquares, int maxBounces, ActorData caster, bool bounceOnActors, float bounceTestWidthInSquares, List<Team> bounceActorTeams, int maxTargets, out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors, out List<ActorData> orderedHitActors, bool includeInvisibles, List<List<NonActorTargetInfo>> nonActorTargetInfoInSegments)
	{
		Vector3 vector = laserStartPos;
		List<Vector3> list = new List<Vector3>();
		bounceHitActors = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
		orderedHitActors = new List<ActorData>();
		float num = maxDistancePerBounceInSquares * Board.Get().squareSize;
		float num2 = totalMaxDistanceInSquares * Board.Get().squareSize;
		Vector3 vector2 = forwardDirection;
		int num3 = 0;
		float num4 = 0f;
		int num5 = 0;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		float maxDistanceInWorld = Board.Get().squareSize * 1.8f;
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetLoSCheckPos();
		laserStartPos.y = travelBoardSquareWorldPositionForLos.y;
		List<NonActorTargetInfo> list2 = new List<NonActorTargetInfo>();
		LaserCoords laserCoordinates = GetLaserCoordinates(laserStartPos, forwardDirection, maxDistanceInWorld, 0f, false, caster, list2);
		Vector3 end = laserCoordinates.end;
		Vector3 a = end - laserStartPos;
		Vector3 vector3 = a * 0.5f;
		float magnitude = vector3.magnitude;
		laserStartPos += vector3;
		num4 += magnitude;
		num -= magnitude;
		int i = 0;
		ActorData actorData = null;
		int num7;
		for (; !flag3; flag3 = ((byte)num7 != 0), i++)
		{
			bool flag4 = nonActorTargetInfoInSegments != null;
			if (nonActorTargetInfoInSegments != null) nonActorTargetInfoInSegments.Add(new List<NonActorTargetInfo>());
			vector2.Normalize();
			float maxDistance = Mathf.Min(num, num2 - num4);
			num = maxDistancePerBounceInSquares * Board.Get().squareSize;
			Vector3 collisionNormal = Vector3.zero;
			Vector3 endPoint = Vector3.zero;
			bool collisionWithGeo = false;
			bool hitActorFirst = false;
			ActorData bounceHitActor = null;
			Vector3 prevStartPos = Vector3.zero;
			if (i == 1)
			{
				prevStartPos = vector;
			}
			else if (i > 1)
			{
				prevStartPos = list[i - 2];
			}
			Vector3 startPosForBounce = laserStartPos;
			Vector3 startPosForGameplay;
			if (i == 0)
			{
				startPosForGameplay = vector;
			}
			else
			{
				startPosForGameplay = laserStartPos;
			}
			Vector3 dir = vector2;
			ActorData excludeActor = actorData;
			object nonActorTargetInfo;
			if (flag4)
			{
				nonActorTargetInfo = nonActorTargetInfoInSegments[i];
			}
			else
			{
				nonActorTargetInfo = null;
			}
			List<ActorData> list3 = CalculateActorBounce(startPosForBounce, startPosForGameplay, dir, maxDistance, caster, bounceOnActors, bounceTestWidthInSquares, bounceActorTeams, excludeActor, includeInvisibles, out endPoint, out collisionWithGeo, out collisionNormal, out hitActorFirst, out bounceHitActor, (List<NonActorTargetInfo>)nonActorTargetInfo, i, prevStartPos);
			actorData = bounceHitActor;
			using (List<ActorData>.Enumerator enumerator = list3.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (!bounceHitActors.ContainsKey(current))
					{
						if (maxTargets > 0)
						{
							if (orderedHitActors.Count >= maxTargets)
							{
								continue;
							}
						}
						AreaEffectUtils.BouncingLaserInfo value = new AreaEffectUtils.BouncingLaserInfo(laserStartPos, i);
						bounceHitActors.Add(current, value);
						orderedHitActors.Add(current);
					}
				}
			}
			int num6;
			if (maxTargets > 0)
			{
				num6 = ((orderedHitActors.Count >= maxTargets) ? 1 : 0);
			}
			else
			{
				num6 = 0;
			}
			bool flag5 = (byte)num6 != 0;
			if (flag5 && flag4)
			{
				nonActorTargetInfoInSegments[i].Clear();
				flag4 = false;
			}
			if (flag4)
			{
				if (list.Count == 0)
				{
					if (list2.Count > 0)
					{
						using (List<NonActorTargetInfo>.Enumerator enumerator2 = list2.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								NonActorTargetInfo current2 = enumerator2.Current;
								nonActorTargetInfoInSegments[i].Add(current2);
							}
						}
					}
				}
			}
			list.Add(endPoint);
			float magnitude2 = (laserStartPos - endPoint).magnitude;
			num4 += magnitude2;
			if (num4 >= num2 - 0.01f)
			{
				flag2 = true;
			}
			if (hitActorFirst)
			{
				num3++;
				num5++;
				laserStartPos = endPoint;
				vector2 -= 2f * Vector3.Dot(vector2, collisionNormal) * collisionNormal;
			}
			else if (collisionWithGeo)
			{
				num3++;
				laserStartPos = endPoint;
				vector2 -= 2f * Vector3.Dot(vector2, collisionNormal) * collisionNormal;
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				if (!flag2)
				{
					if (num3 <= maxBounces)
					{
						num7 = (flag5 ? 1 : 0);
						continue;
					}
				}
			}
			num7 = 1;
		}
		return list;
	}

	public static List<ActorData> CalculateActorBounce(Vector3 startPosForBounce, Vector3 startPosForGameplay, Vector3 dir, float maxDistance, ActorData caster, bool bounceOnActors, float bounceTestWidthInSquares, List<Team> hitTeams, ActorData excludeActor, bool includeInvisibles, out Vector3 endPoint, out bool collisionWithGeo, out Vector3 collisionNormal, out bool hitActorFirst, out ActorData bounceHitActor, List<NonActorTargetInfo> nonActorTargetInfo, int segmentIndex, Vector3 prevStartPos)
	{
		LayerMask mask = (1 << LayerMask.NameToLayer("LineOfSight")) | (1 << LayerMask.NameToLayer("DynamicLineOfSight"));
		RaycastHit hitInfo;
		collisionWithGeo = Physics.Raycast(startPosForBounce, dir, out hitInfo, maxDistance, mask);
		if (collisionWithGeo)
		{
			Vector3 a = hitInfo.point - startPosForBounce;
			a.y = 0f;
			float magnitude = a.magnitude;
			a.Normalize();
			endPoint = startPosForBounce + a * Mathf.Max(0f, magnitude - 0.05f);
			collisionNormal = hitInfo.normal;
		}
		else
		{
			endPoint = startPosForBounce + dir * maxDistance;
			collisionNormal = Vector3.zero;
		}
		if (BarrierManager.Get() != null)
		{
			bool collision;
			Vector3 collisionNormal2;
			Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(caster, startPosForBounce, endPoint, out collision, out collisionNormal2, nonActorTargetInfo);
			if (collision)
			{
				endPoint = abilityLineEndpoint;
				collisionWithGeo = true;
				collisionNormal = collisionNormal2;
			}
		}
		hitActorFirst = false;
		bounceHitActor = null;
		float actorTargetingRadius = AreaEffectUtils.GetActorTargetingRadius();
		List<ActorData> list;
		if (GameWideData.Get().UseActorRadiusForLaser())
		{
			list = AreaEffectUtils.GetActorsInBoxByActorRadius(startPosForGameplay, endPoint, bounceTestWidthInSquares, false, caster, hitTeams);
		}
		else
		{
			list = AreaEffectUtils.GetActorsInBox(startPosForGameplay, endPoint, bounceTestWidthInSquares, true, caster, hitTeams);
		}
		List<ActorData> actors = list;
		actors.Remove(caster);
		if (excludeActor != null)
		{
			actors.Remove(excludeActor);
		}
		if (!includeInvisibles)
		{
			if (NetworkServer.active)
			{
				TargeterUtils.RemoveActorsInvisibleToActor(ref actors, caster);
			}
			else
			{
				TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			}
		}
		Vector3 a2 = endPoint - startPosForBounce;
		a2.y = 0f;
		a2.Normalize();
		if (segmentIndex > 0)
		{
			if (actors.Count > 0)
			{
				Vector3 b = prevStartPos - startPosForBounce;
				b.y = 0f;
				b.Normalize();
				Vector3 collisionNormal3 = 0.5f * (a2 + b);
				for (int num = actors.Count - 1; num >= 0; num--)
				{
					BoardSquare currentBoardSquare = actors[num].GetCurrentBoardSquare();
					if (!SquareOnSameSideAsBounceBend(currentBoardSquare, startPosForBounce, collisionNormal3))
					{
						actors.RemoveAt(num);
					}
				}
			}
		}
		TargeterUtils.SortActorsByDistanceToPos(ref actors, startPosForBounce);
		if (bounceOnActors)
		{
			int num2 = 0;
			for (int i = 0; i < actors.Count; i++)
			{
				if (hitActorFirst)
				{
					break;
				}
				ActorData actorData = actors[i];
				Vector3 vector = actorData.GetFreePos() - startPosForBounce;
				vector.y = 0f;
				float magnitude2 = vector.magnitude;
				Vector3 vector2 = endPoint - startPosForBounce;
				vector2.y = 0f;
				float magnitude3 = vector2.magnitude;
				if (!(magnitude2 < magnitude3))
				{
					continue;
				}
				Vector3 travelBoardSquareWorldPosition = actorData.GetFreePos();
				float num3 = 0.5f * bounceTestWidthInSquares * Board.Get().squareSize;
				if (GameWideData.Get().UseActorRadiusForLaser())
				{
					num3 += actorTargetingRadius * Board.Get().squareSize;
				}
				num3 = Mathf.Min(0.4f * Board.Get().squareSize, num3);
				Vector3 intersectP;
				Vector3 intersectP2;
				int lineCircleIntersections = GetLineCircleIntersections(startPosForBounce, endPoint, travelBoardSquareWorldPosition, num3, out intersectP, out intersectP2);
				if (lineCircleIntersections > 1)
				{
					float num4 = HorizontalPlaneDistInWorld(intersectP, startPosForBounce);
					float num5 = HorizontalPlaneDistInWorld(intersectP2, startPosForBounce);
					endPoint = ((!(num4 <= num5)) ? intersectP2 : intersectP);
					endPoint.y = startPosForBounce.y;
					collisionNormal = endPoint - travelBoardSquareWorldPosition;
					collisionNormal.y = 0f;
					collisionNormal.Normalize();
					float num6 = 0.5f * AreaEffectUtils.GetMaxAngleForActorBounce();
					collisionNormal = Vector3.RotateTowards(-a2, collisionNormal, (float)Math.PI / 180f * num6, 0f);
					hitActorFirst = true;
					collisionWithGeo = false;
					bounceHitActor = actorData;
					num2 = i;
				}
			}
			if (hitActorFirst)
			{
				TargeterUtils.LimitActorsToMaxNumber(ref actors, num2 + 1);
			}
		}
		return actors;
	}

	public static Vector3 GetLineLineIntersection(Vector3 pointOnFirst, Vector3 directionOfFirst, Vector3 pointOnSecond, Vector3 directionOfSecond, out bool intersecting)
	{
		Vector3 vector = pointOnFirst;
		Vector3 a = pointOnSecond;
		Vector3 vector2 = directionOfFirst;
		Vector3 rhs = directionOfSecond;
		vector.y = 0f;
		a.y = 0f;
		vector2.y = 0f;
		vector2.Normalize();
		rhs.y = 0f;
		rhs.Normalize();
		Vector3 vector3 = Vector3.Cross(vector2, rhs);
		Vector3 lhs = a - vector;
		if (vector3.magnitude == 0f)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					intersecting = false;
					return Vector3.zero;
				}
			}
		}
		intersecting = true;
		float d = Vector3.Cross(lhs, rhs).magnitude / vector3.magnitude;
		return vector + d * vector2;
	}

	public static int GetLineCircleIntersections(Vector3 testPoint1, Vector3 testPoint2, Vector3 circleCenter, float radius, out Vector3 intersectP1, out Vector3 intersectP2)
	{
		intersectP1 = Vector3.zero;
		intersectP2 = Vector3.zero;
		testPoint1.y = 0f;
		testPoint2.y = 0f;
		circleCenter.y = 0f;
		Vector3 vector = testPoint1 - circleCenter;
		Vector3 vector2 = testPoint2 - circleCenter;
		float num = vector2.x - vector.x;
		float num2 = vector2.z - vector.z;
		float num3 = num * num + num2 * num2;
		float num4 = vector.x * vector2.z - vector2.x * vector.z;
		float num5 = radius * radius * num3 - num4 * num4;
		if (num3 == 0f)
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
		if (Mathf.Abs(num5) < 0.001f)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					intersectP1.x = num4 * num2 / num3;
					intersectP1.z = (0f - num4) * num / num3;
					intersectP1 += circleCenter;
					return 1;
				}
			}
		}
		if (num5 < 0f)
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
		float num6;
		if (num2 < 0f)
		{
			num6 = -1f;
		}
		else
		{
			num6 = 1f;
		}
		float num7 = num6;
		float num8 = Mathf.Sqrt(num5);
		intersectP1.x = (num4 * num2 + num7 * num * num8) / num3;
		intersectP1.z = ((0f - num4) * num + Mathf.Abs(num2) * num8) / num3;
		intersectP2.x = (num4 * num2 - num7 * num * num8) / num3;
		intersectP2.z = ((0f - num4) * num - Mathf.Abs(num2) * num8) / num3;
		intersectP1 += circleCenter;
		intersectP2 += circleCenter;
		return 2;
	}

	public static bool IsSegmentIntersectingCircle(Vector3 startPos, Vector3 endPos, Vector3 circleCenter, float radius)
	{
		startPos.y = 0f;
		endPos.y = 0f;
		circleCenter.y = 0f;
		Vector3 vector = endPos - startPos;
		Vector3 intersectP;
		Vector3 intersectP2;
		int lineCircleIntersections = GetLineCircleIntersections(startPos, endPos, circleCenter, radius, out intersectP, out intersectP2);
		if (lineCircleIntersections >= 1)
		{
			float num = Vector3.Dot(vector, vector);
			Vector3 rhs = intersectP - startPos;
			float num2 = Vector3.Dot(vector, rhs);
			if (num2 >= 0f)
			{
				if (num2 <= num)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
			if (lineCircleIntersections >= 2)
			{
				rhs = intersectP2 - startPos;
				num2 = Vector3.Dot(vector, rhs);
				if (num2 >= 0f)
				{
					if (num2 <= num)
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
		return false;
	}

	public static bool OnSameSideOfLine(Vector3 testPoint1, Vector3 testPoint2, Vector3 linePtA, Vector3 linePtB)
	{
		Vector3 lhs = linePtB - linePtA;
		lhs.y = 0f;
		Vector3 rhs = testPoint1 - linePtA;
		rhs.y = 0f;
		Vector3 rhs2 = testPoint2 - linePtA;
		rhs2.y = 0f;
		Vector3 lhs2 = Vector3.Cross(lhs, rhs);
		Vector3 rhs3 = Vector3.Cross(lhs, rhs2);
		float num = Vector3.Dot(lhs2, rhs3);
		return num >= 0f;
	}

	public static bool IsPointInTriangle(Vector3 triA, Vector3 triB, Vector3 triC, Vector3 testPt)
	{
		bool flag = OnSameSideOfLine(testPt, triA, triB, triC);
		bool flag2 = OnSameSideOfLine(testPt, triB, triA, triC);
		int result;
		if (OnSameSideOfLine(testPt, triC, triA, triB))
		{
			if (flag)
			{
				result = (flag2 ? 1 : 0);
				goto IL_003f;
			}
		}
		result = 0;
		goto IL_003f;
		IL_003f:
		return (byte)result != 0;
	}

	public static bool IsPointInLaser(Vector3 testPoint, Vector3 laserStartPos, Vector3 laserEndPos, float laserWidthInWorld)
	{
		testPoint.y = 0f;
		laserStartPos.y = 0f;
		laserEndPos.y = 0f;
		float sqrMagnitude = (laserEndPos - laserStartPos).sqrMagnitude;
		Vector3 normalized = (laserEndPos - laserStartPos).normalized;
		Vector3 lhs = testPoint - laserStartPos;
		Vector3 vector = laserStartPos + Vector3.Dot(lhs, normalized) * normalized;
		float sqrMagnitude2 = (vector - laserStartPos).sqrMagnitude;
		float sqrMagnitude3 = (laserEndPos - vector).sqrMagnitude;
		int num;
		if (sqrMagnitude2 < sqrMagnitude)
		{
			num = ((sqrMagnitude3 < sqrMagnitude) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		float sqrMagnitude4 = (vector - testPoint).sqrMagnitude;
		float num2 = laserWidthInWorld / 2f * (laserWidthInWorld / 2f);
		bool flag2 = sqrMagnitude4 < num2;
		int result;
		if (flag)
		{
			result = (flag2 ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public static Vector3 GetProjectionPoint(Vector3 normalizedDir, Vector3 startPos, Vector3 pointToProject)
	{
		Vector3 lhs = pointToProject - startPos;
		return startPos + Vector3.Dot(lhs, normalizedDir) * normalizedDir;
	}

	public static float HorizontalAngle_Rad(Vector3 vec)
	{
		Vector2 vector = new Vector2(vec.x, vec.z);
		vector.Normalize();
		return Mathf.Atan2(vector.y, vector.x);
	}

	public static float HorizontalAngle_Deg(Vector3 vec)
	{
		float num = HorizontalAngle_Rad(vec);
		float num2 = num * 57.29578f;
		if (num2 < 0f)
		{
			num2 += 360f;
		}
		return num2;
	}

	public static float ClampAngle_Deg(float angle, float min, float max)
	{
		float num;
		if (min > max)
		{
			Debug.LogError(new StringBuilder().Append("Clamping an angle ").Append(angle).Append(" to a min of ").Append(min).Append(" and a max of ").Append(max).Append(", but min is greater than max.").ToString());
			num = angle;
		}
		else if (min == max)
		{
			num = min;
		}
		else
		{
			if (angle >= min)
			{
				if (angle <= max)
				{
					num = angle;
					goto IL_0152;
				}
			}
			if (angle + 360f >= min)
			{
				if (angle + 360f <= max)
				{
					num = angle;
					goto IL_0152;
				}
			}
			if (angle - 360f >= min)
			{
				if (angle - 360f <= max)
				{
					num = angle;
					goto IL_0152;
				}
			}
			float num2 = Mathf.Clamp(angle, min, max);
			float num3 = Mathf.Clamp(angle, min + 360f, max + 360f);
			float num4 = Mathf.Clamp(angle, min - 360f, max - 360f);
			float num5 = Mathf.Abs(angle - num2);
			float num6 = Mathf.Abs(angle - num3);
			float num7 = Mathf.Abs(angle - num4);
			if (num5 <= num6 && num5 <= num7)
			{
				num = num2;
			}
			else if (num6 <= num5 && num6 <= num7)
			{
				num = num3;
			}
			else
			{
				num = num4;
			}
		}
		goto IL_0152;
		IL_0152:
		while (num > 360f)
		{
			num -= 360f;
		}
		for (; num < 0f; num += 360f)
		{
		}
		while (true)
		{
			return num;
		}
	}

	public static Vector3 AngleRadToVector(float angle)
	{
		return new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
	}

	public static Vector3 AngleDegreesToVector(float angle)
	{
		float angle2 = angle * ((float)Math.PI / 180f);
		return AngleRadToVector(angle2);
	}

	public static Vector3 GetDirectionToClosestSide(BoardSquare square, Vector3 testPos)
	{
		Vector3 result = new Vector3(1f, 0f, 0f);
		if (square != null)
		{
			Vector3 b = square.ToVector3();
			Vector3 vec = testPos - b;
			vec.y = 0f;
			if (!(vec.magnitude < 0.1f))
			{
				int angleWithHorizontal = Mathf.RoundToInt(HorizontalAngle_Deg(vec));
				return HorizontalAngleToClosestCardinalDirection(angleWithHorizontal);
			}
			result = new Vector3(1f, 0f, 0f);
		}
		return result;
	}

	public static Vector3 HorizontalAngleToClosestCardinalDirection(int angleWithHorizontal)
	{
		Vector3 result;
		if (angleWithHorizontal > 45 && angleWithHorizontal <= 135)
		{
			result = new Vector3(0f, 0f, 1f);
		}
		else
		{
			if (angleWithHorizontal > 135)
			{
				if (angleWithHorizontal <= 225)
				{
					result = new Vector3(-1f, 0f, 0f);
					goto IL_00c9;
				}
			}
			if (angleWithHorizontal > 225)
			{
				if (angleWithHorizontal <= 315)
				{
					result = new Vector3(0f, 0f, -1f);
					goto IL_00c9;
				}
			}
			result = new Vector3(1f, 0f, 0f);
		}
		goto IL_00c9;
		IL_00c9:
		return result;
	}

	public static Vector3 GetDirectionAndOffsetToClosestSide(BoardSquare square, Vector3 testPos, bool allowDiagonalAim, out Vector3 offset)
	{
		Vector3 vector = new Vector3(1f, 0f, 0f);
		offset = 0.5f * Board.Get().squareSize * vector;
		bool flag;
		float num2;
		if (square != null)
		{
			Vector3 b = square.ToVector3();
			Vector3 vec = testPos - b;
			vec.y = 0f;
			if (vec.magnitude < 0.1f)
			{
				while (true)
				{
					return vector;
				}
			}
			if (allowDiagonalAim)
			{
				int num = Mathf.RoundToInt(HorizontalAngle_Deg(vec));
				num2 = 0f;
				flag = false;
				if (num < 337)
				{
					if (num > 23)
					{
						if (num < 67)
						{
							num2 = 45f;
							flag = true;
						}
						else if (num <= 113)
						{
							num2 = 90f;
						}
						else if (num < 157)
						{
							num2 = 135f;
							flag = true;
						}
						else if (num <= 203)
						{
							num2 = 180f;
						}
						else if (num < 247)
						{
							num2 = 225f;
							flag = true;
						}
						else if (num <= 293)
						{
							num2 = 270f;
						}
						else
						{
							num2 = 315f;
							flag = true;
						}
						goto IL_01c2;
					}
				}
				num2 = 0f;
				goto IL_01c2;
			}
			vector = GetDirectionToClosestSide(square, testPos);
			offset = 0.5f * Board.Get().squareSize * vector;
		}
		goto IL_0231;
		IL_0231:
		return vector;
		IL_01c2:
		vector = AngleDegreesToVector(num2);
		if (flag)
		{
			float num3 = 0.5f * Mathf.Sqrt(2f) - 0.01f;
			offset = num3 * Board.Get().squareSize * vector;
		}
		else
		{
			offset = 0.5f * Board.Get().squareSize * vector;
		}
		goto IL_0231;
	}

	public static float HorizontalPlaneDistInWorld(Vector3 a, Vector3 b)
	{
		Vector3 vector = b - a;
		vector.y = 0f;
		return vector.magnitude;
	}

	public static float HorizontalPlaneDistInSquares(Vector3 a, Vector3 b)
	{
		return HorizontalPlaneDistInWorld(a, b) / Board.Get().squareSize;
	}
}
