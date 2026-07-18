// ROGUES
// SERVER
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
		return actor != null
			&& !actor.IsDead()
			&& !actor.IgnoreForAbilityHits
			&& actor.GetCurrentBoardSquare() != null
			&& IsRelevantTeam(validTeams, actor.GetTeam());
	}

	// inlined in rogues
	public static bool IsRelevantTeam(List<Team> validTeams, Team team)
	{
		if (validTeams == null)
		{
			return true;
		}
		foreach (Team validTeam in validTeams)
		{
			if (validTeam == team)
			{
				return true;
			}
		}
		return false;
	}

	public static List<BoardSquare> GetValidRespawnSquaresInDonut(float centerX, float centerY, float innerRadius, float outerRadius)
	{
		float squareSize = Board.Get().squareSize;
		innerRadius *= squareSize;
		outerRadius *= squareSize;
		List<BoardSquare> result = new List<BoardSquare>();
		Vector3 a = new Vector3(centerX, 0f, centerY);
		int maxX = Board.Get().GetMaxX();
		int maxY = Board.Get().GetMaxY();
		int x1 = (int)Mathf.Max(0f, (centerX - outerRadius) / squareSize);
		int x2 = (int)Mathf.Min(maxX, (centerX + outerRadius) / squareSize);
		int y1 = (int)Mathf.Max(0f, (centerY - outerRadius) / squareSize);
		int y2 = (int)Mathf.Min(maxY, (centerY + outerRadius) / squareSize);
		for (int i = x1; i < x2; i++)
		{
			for (int j = y1; j < y2; j++)
			{
				Vector3 b = new Vector3(i * squareSize, 0f, j * squareSize);
				float sqrMagnitude = (a - b).sqrMagnitude;
				if (sqrMagnitude < outerRadius * outerRadius && sqrMagnitude > innerRadius * innerRadius)
				{
					BoardSquare boardSquare = Board.Get().GetSquareFromIndex(i, j);
					if (boardSquare != null && boardSquare.IsValidForGameplay() && boardSquare.OccupantActor == null)
					{
						result.Add(boardSquare);
					}
				}
			}
		}
		return result;
	}

	private static void AddSquareAtIndexToListIfValid(int x, int y, List<BoardSquare> list)
	{
		BoardSquare boardSquare = Board.Get().GetSquareFromIndex(x, y);
		if (boardSquare != null)
		{
			list.Add(boardSquare);
		}
	}

	public static List<BoardSquare> GetSquaresInBorderLayer(BoardSquare center, int borderLayerNumber, bool requireLosToCenter)
	{
		if (center == null || borderLayerNumber < 0)
		{
			return null;
		}
		List<BoardSquare> result = new List<BoardSquare>();
		if (borderLayerNumber == 0)
		{
			result.Add(center);
			return result;
		}
		
		AddSquareAtIndexToListIfValid(center.x + borderLayerNumber, center.y, result);
		AddSquareAtIndexToListIfValid(center.x - borderLayerNumber, center.y, result);
		AddSquareAtIndexToListIfValid(center.x, center.y + borderLayerNumber, result);
		AddSquareAtIndexToListIfValid(center.x, center.y - borderLayerNumber, result);
		for (int i = 1; i <= borderLayerNumber; i++)
		{
			int dx = borderLayerNumber;
			int dy = i;
			AddSquareAtIndexToListIfValid(center.x + dx, center.y + dy, result);
			AddSquareAtIndexToListIfValid(center.x - dx, center.y + dy, result);
			AddSquareAtIndexToListIfValid(center.x + dx, center.y - dy, result);
			AddSquareAtIndexToListIfValid(center.x - dx, center.y - dy, result);
			if (dx != dy)
			{
				dx = i;
				dy = borderLayerNumber;
				AddSquareAtIndexToListIfValid(center.x + dx, center.y + dy, result);
				AddSquareAtIndexToListIfValid(center.x - dx, center.y + dy, result);
				AddSquareAtIndexToListIfValid(center.x + dx, center.y - dy, result);
				AddSquareAtIndexToListIfValid(center.x - dx, center.y - dy, result);
			}
		}
		if (requireLosToCenter)
		{
			for (int i = result.Count - 1; i >= 0; i--)
			{
				BoardSquare boardSquare = result[i];
				if (!center.GetLOS(boardSquare.x, boardSquare.y))
				{
					result.RemoveAt(i);
				}
			}
		}
		return result;
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
		if (flag != flag2 && PointToLineDistance(testPt, pt1, pt2) < halfWidth)
		{
			result = true;
		}
		return result;
	}

	public static void SortSquaresByDistanceToPos(ref List<BoardSquare> squares, Vector3 pos)
	{
		squares.Sort(delegate(BoardSquare a, BoardSquare b)
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
			Vector3 aVec = a.ToVector3();
			aVec.y = pos.y;
			Vector3 bVec = b.ToVector3();
			bVec.y = pos.y;
			float aDistSqr = (aVec - pos).sqrMagnitude;
			float bDistSqr = (bVec - pos).sqrMagnitude;
			if (aDistSqr == bDistSqr)
			{
				if (a.x != b.x)
				{
					return a.x.CompareTo(b.x);
				}
				if (a.y != b.y)
				{
					return a.y.CompareTo(b.y);
				}
			}
			return aDistSqr.CompareTo(bDistSqr);
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
			for (float num = 0f; num <= magnitude; num += 0.5f * Board.Get().squareSize)
			{
				Vector3 vector = end + num * a;
				BoardSquare boardSquare = Board.Get().GetSquareFromVec3(vector);
				if (boardSquare != null && boardSquare.IsValidForGameplay())
				{
					adjustedEndPoint = vector;
					return true;
				}
			}
			BoardSquare startSquare = Board.Get().GetSquareFromVec3(start);
			if (startSquare != null && startSquare.IsValidForGameplay())
			{
				adjustedEndPoint = start;
				return true;
			}
		}
		else
		{
			BoardSquare endSquare = Board.Get().GetSquareFromVec3(end);
			if (endSquare != null && endSquare.IsValidForGameplay())
			{
				adjustedEndPoint = end;
				return true;
			}
		}
		return false;
	}

	public static bool SquaresHaveLoSForAbilities(
		BoardSquare source,
		BoardSquare dest,
		ActorData caster,
		bool checkBarreirs = true,
		List<NonActorTargetInfo> nonActorTargetInfo = null)
	{
		if (source == null || dest == null)
		{
			return false;
		}
		if (source == dest)
		{
			return true;
		}
		if (source.IsValidForGameplay() && !source.GetLOS(dest.x, dest.y))
		{
			return false;
		}
		if (checkBarreirs)
		{
			return HasLosByBarriers(source, dest, caster, VectorUtils.s_laserOffset * Board.SquareSizeStatic, nonActorTargetInfo);
		}
		return true;
	}

	public static bool HasLosByBarriers(
		BoardSquare source,
		BoardSquare dest,
		ActorData caster,
		float offsetToUseInWorld,
		List<NonActorTargetInfo> nonActorTargetInfo = null)
	{
		bool hasAbilityBlockingBarriers = BarrierManager.Get() != null && BarrierManager.Get().HasAbilityBlockingBarriers();
		if (caster != null
			&& BarrierManager.Get() != null
			&& hasAbilityBlockingBarriers
			&& offsetToUseInWorld <= 0f
			&& BarrierManager.Get().AreAbilitiesBlocked(caster, source, dest, nonActorTargetInfo))
		{
			return false;
		}
		bool hasLoS = true;
		List<NonActorTargetInfo> list = nonActorTargetInfo != null ? new List<NonActorTargetInfo>() : null;
		if (BarrierManager.Get() != null && hasAbilityBlockingBarriers)
		{
			Vector3 srcVec = source.ToVector3();
			Vector3 dstVec = dest.ToVector3();
			Vector3 dir = dstVec - srcVec;
			dir.y = 0f;
			float dist = dir.magnitude;
			dir.Normalize();
			Vector3 offset = Vector3.Cross(Vector3.up, dir);
			offset.Normalize();
			offset *= offsetToUseInWorld;
			srcVec.y = Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
			dstVec.y = srcVec.y;
			bool isAbilityBlockedA = BarrierManager.Get().AreAbilitiesBlocked(caster, srcVec + offset, dstVec + offset, list);
			bool isAbilityBlockedB = BarrierManager.Get().AreAbilitiesBlocked(caster, srcVec - offset, dstVec - offset, list);
			if (isAbilityBlockedA || isAbilityBlockedB)
			{
				bool isLosBlockedA = isAbilityBlockedA || VectorUtils.RaycastInDirection(srcVec + offset, dir, dist, out _);
				bool isLosBlockedB = isAbilityBlockedB || VectorUtils.RaycastInDirection(srcVec - offset, dir, dist, out _);
				if (isLosBlockedA && isLosBlockedB)
				{
					hasLoS = false;
				}
			}
			if (!hasLoS && list != null && nonActorTargetInfo != null)
			{
				nonActorTargetInfo.AddRange(list);
			}
		}
		return hasLoS;
	}

	public static int GetCircleCircleIntersections(
		Vector3 centerA,
		Vector3 centerB,
		float radiusAInSquares,
		float radiusBInSquares,
		out Vector3 intersectP1,
		out Vector3 intersectP2)
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
			return 0;
		}
		if (magnitude < Mathf.Abs(num - num2))
		{
			return 0;
		}
		if (magnitude == 0f && num == num2)
		{
			return -1;
		}

		int result = Mathf.Abs(magnitude - (num + num2)) < 0.001f ? 1 : 2;
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
		if (inAreaCheckers != null)
		{
			foreach (ISquareInsideChecker squareInsideChecker in inAreaCheckers)
			{
				if (squareInsideChecker.IsSquareInside(testSquare, out bool inLos) && inLos)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static ActorData GetTargetableActorOnSquare(BoardSquare square, bool allowEnemy, bool allowAlly, ActorData caster)
	{
		if (square != null && square.OccupantActor != null && IsActorTargetable(square.OccupantActor))
		{
			Team team = square.OccupantActor.GetTeam();
			if (caster != null)
			{
				bool isAlly = team == caster.GetTeam();
				if (isAlly && allowAlly || !isAlly && allowEnemy)
				{
					return square.OccupantActor;
				}
			}
			else
			{
				return square.OccupantActor;
			}
		}
		return null;
	}

#if SERVER
	// added in rogues
	public static ActorData GetActorOnSquareOnPhaseStart(BoardSquare square)
	{
		if (square == null)
		{
			return null;
		}
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData != null
			    && !actorData.IsDead()
			    && !actorData.IgnoreForAbilityHits
			    && actorData.GetSquareAtPhaseStart() != null
			    && actorData.GetSquareAtPhaseStart() == square)
			{
				return actorData;
			}
		}
		return null;
	}
#endif

	public static List<BoardSquare> GetSquaresInRadius(BoardSquare centerSquare, float radiusInSquares, bool ignoreLoS, ActorData caster)
	{
		return GetSquaresInRadius(centerSquare.worldX, centerSquare.worldY, radiusInSquares, ignoreLoS, caster);
	}

	public static List<BoardSquare> GetSquaresInRadius(float centerX, float centerY, float radiusInSquares, bool ignoreLoS, ActorData caster)
	{
		float squareSize = Board.Get().squareSize;
		float num = radiusInSquares * squareSize;
		BoardSquare boardSquareSafe = Board.Get().GetSquareFromPos(centerX, centerY);
		List<BoardSquare> list = new List<BoardSquare>();
		if (boardSquareSafe == null)
		{
			return list;
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
				Vector3 b = new Vector3(i * squareSize, 0f, j * squareSize);
				float sqrMagnitude = (a - b).sqrMagnitude;
				if (sqrMagnitude < num * num)
				{
					BoardSquare boardSquare = Board.Get().GetSquareFromIndex(i, j);
					if (boardSquare != null && boardSquare.height > -Board.Get().BaselineHeight)
					{
						bool flag = true;
						if (!ignoreLoS)
						{
							flag = SquaresHaveLoSForAbilities(boardSquareSafe, boardSquare, caster);
						}
						if (flag)
						{
							list.Add(boardSquare);
						}
					}
				}
			}
		}
		return list;
	}

	public static bool IsSquareInRadius(BoardSquare testSquare, float centerX, float centerY, float radiusInSquares, bool ignoreLoS, ActorData caster)
	{
		if (testSquare == null)
		{
			return false;
		}
		bool result = false;
		float squareSize = Board.Get().squareSize;
		float num = radiusInSquares * squareSize;
		int x = testSquare.x;
		int y = testSquare.y;
		BoardSquare boardSquareSafe = Board.Get().GetSquareFromPos(centerX, centerY);
		if (boardSquareSafe == null)
		{
			return false;
		}
		float worldX = boardSquareSafe.worldX;
		float worldY = boardSquareSafe.worldY;
		Vector3 a = new Vector3(worldX, 0f, worldY);
		Vector3 b = new Vector3(x * squareSize, 0f, y * squareSize);
		float sqrMagnitude = (a - b).sqrMagnitude;
		if (sqrMagnitude < num * num)
		{
			BoardSquare boardSquare = Board.Get().GetSquareFromIndex(x, y);
			if (boardSquare != null && boardSquare.height >= Board.Get().BaselineHeight)
			{
				result = ignoreLoS || SquaresHaveLoSForAbilities(boardSquareSafe, boardSquare, caster);
			}
		}
		return result;
	}

	public static List<ActorData> GetActorsInRadius(
		Vector3 centerPos,
		float radiusInSquares,
		bool ignoreLoS,
		ActorData caster,
		Team onlyValidTeam,
		List<NonActorTargetInfo> nonActorTargetInfo,
		bool useLosOverridePos = false, // removed in rogues
		Vector3 losOverridePos = default(Vector3)) // removed in rogues
	{
		return GetActorsInRadius(
			centerPos,
			radiusInSquares,
			ignoreLoS,
			caster,
			new List<Team> { onlyValidTeam },
			nonActorTargetInfo,
			useLosOverridePos, // removed in rogues
			losOverridePos); // removed in rogues
	}

	public static List<ActorData> GetActorsInRadius(
		Vector3 centerPos,
		float radiusInSquares,
		bool ignoreLoS,
		ActorData caster,
		List<Team> onlyValidTeams,
		List<NonActorTargetInfo> nonActorTargetInfo,
		bool useLosOverridePos = false, // removed in rogues
		Vector3 losOverridePos = default(Vector3)) // removed in rogues
	{
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			return GetActorsInConeByActorRadius(
				centerPos,
				0f,
				360f,
				radiusInSquares,
				0f,
				ignoreLoS,
				caster,
				onlyValidTeams,
				nonActorTargetInfo,
				useLosOverridePos, // removed in rogues
				losOverridePos); // removed in rogues
		}
		List<ActorData> result = new List<ActorData>();
		List<BoardSquare> squaresInRadius = GetSquaresInRadius(centerPos.x, centerPos.z, radiusInSquares, ignoreLoS, caster);
		foreach (BoardSquare current in squaresInRadius)
		{
			ActorData occupantActor = current.OccupantActor;
			if (IsActorTargetable(occupantActor, onlyValidTeams)
			    && !result.Contains(occupantActor)
			    && squaresInRadius.Contains(occupantActor.GetTravelBoardSquare()))
			{
				result.Add(occupantActor);
			}
		}
		return result;
	}

	public static bool IsPosInCone(Vector3 testPos, Vector3 centerPos, float radiusInWorld, float coneCenterAngle, float coneWidthAngleDeg)
	{
		coneWidthAngleDeg = Mathf.Clamp(coneWidthAngleDeg, 0f, 360f);
		testPos.y = 0f;
		centerPos.y = 0f;
		float sqrMagnitude = (centerPos - testPos).sqrMagnitude;
		if (sqrMagnitude > radiusInWorld * radiusInWorld)
		{
			return false;
		}
		
		float maxAngleWithCenter = 0.5f * coneWidthAngleDeg;
		Vector3 vec = testPos - centerPos;
		return vec.sqrMagnitude <= 0.001f
		       || IsAngleWithinCone(VectorUtils.HorizontalAngle_Deg(vec), coneCenterAngle, maxAngleWithCenter);
	}

	public static bool IsPosInAngleOfCone(Vector3 testPos, Vector3 centerPos, float coneCenterAngle, float coneWidthAngleDeg)
	{
		int value = Mathf.RoundToInt(coneWidthAngleDeg);
		value = Mathf.Clamp(value, 0, 360);
		bool result = false;
		testPos.y = 0f;
		centerPos.y = 0f;
		float maxAngleWithCenter = 0.5f * value;
		Vector3 vec = testPos - centerPos;
		if (vec.sqrMagnitude > 0f)
		{
			float testAngle = VectorUtils.HorizontalAngle_Deg(vec);
			result = IsAngleWithinCone(testAngle, coneCenterAngle, maxAngleWithCenter);
		}
		return result;
	}

	public static bool IsAngleWithinCone(float testAngle, float coneCenterAngle, float maxAngleWithCenter)
	{
		float min = coneCenterAngle - maxAngleWithCenter;
		float max = coneCenterAngle + maxAngleWithCenter;
		if (testAngle < min)
		{
			testAngle += 360f;
		}
		else if (testAngle > max)
		{
			testAngle -= 360f;
		}
		return min <= testAngle && testAngle <= max;
	}

	public static List<BoardSquare> GetSquaresInCone(
		Vector3 coneStart,
		float coneCenterAngleDegrees,
		float coneWidthDegrees,
		float coneLengthRadiusInSquares,
		float coneBackwardOffsetInSquares,
		bool ignoreLoS,
		ActorData caster)
	{
		Vector3 coneDir = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float backwardOffset = coneBackwardOffsetInSquares * Board.Get().squareSize;
		Vector3 offsetStart = coneStart - coneDir * backwardOffset;
		float radiusInSquares = coneLengthRadiusInSquares + coneBackwardOffsetInSquares;
		List<BoardSquare> squaresInOffsetCone = null;
		List<BoardSquare> squaresInNonOffsetCone = null;
		if (coneBackwardOffsetInSquares == 0f)
		{
			squaresInOffsetCone = GetSquaresInRadius(coneStart.x, coneStart.z, radiusInSquares, ignoreLoS, caster);
			squaresInNonOffsetCone = squaresInOffsetCone;
		}
		else
		{
			squaresInOffsetCone = GetSquaresInRadius(offsetStart.x, offsetStart.z, radiusInSquares, true, caster);
			squaresInNonOffsetCone = GetSquaresInRadius(coneStart.x, coneStart.z, radiusInSquares, ignoreLoS, caster);
		}
		
		List<BoardSquare> result = new List<BoardSquare>();
		float maxAngleWithCenter = coneWidthDegrees / 2f;
		foreach (BoardSquare square in squaresInOffsetCone)
		{
			Vector3 dir = square.ToVector3() - offsetStart;
			dir.y = 0f;
			if (dir.sqrMagnitude != 0f)
			{
				bool isInCone = Vector3.Angle(dir, coneDir) <= maxAngleWithCenter;
				bool isInLoS = ignoreLoS;
				if (isInCone && !ignoreLoS)
				{
					isInLoS = coneBackwardOffsetInSquares == 0f || squaresInNonOffsetCone.Contains(square);
				}
				if (isInCone && isInLoS)
				{
					result.Add(square);
				}
			}
		}
		return result;
	}

	public static List<BoardSquare> GetSquaresInConeByActorRadius(
		Vector3 coneStart,
		float coneCenterAngleDegrees,
		float coneWidthDegrees,
		float coneLengthRadiusInSquares,
		float coneBackwardOffsetInSquares,
		bool ignoreLoS,
		ActorData caster)
	{
		List<BoardSquare> result = new List<BoardSquare>();
		Vector3 coneDir = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float backwardOffset = coneBackwardOffsetInSquares * Board.Get().squareSize;
		Vector3 offsetConeStart = coneStart - coneDir * backwardOffset;
		BoardSquare offsetConeStartSquare = Board.Get().GetSquareFromPos(offsetConeStart.x, offsetConeStart.z);
		if (offsetConeStartSquare == null)
		{
			return result;
		}
		GetMaxConeBounds(
			coneStart,
			coneCenterAngleDegrees,
			coneWidthDegrees,
			coneLengthRadiusInSquares,
			coneBackwardOffsetInSquares,
			out int minX,
			out int maxX,
			out int minY,
			out int maxY);
		for (int i = minX; i < maxX; i++)
		{
			for (int j = minY; j < maxY; j++)
			{
				BoardSquare square = Board.Get().GetSquareFromIndex(i, j);
				if (square == null)
				{
					continue;
				}
				if (IsSquareInConeByActorRadius(
					    square,
					    coneStart,
					    coneCenterAngleDegrees,
					    coneWidthDegrees,
					    coneLengthRadiusInSquares,
					    coneBackwardOffsetInSquares,
					    ignoreLoS,
					    caster))
				{
					result.Add(square);
				}
			}
		}
		return result;
	}

	public static bool IsSquareInCone(
		BoardSquare testSquare,
		Vector3 coneStart,
		float coneCenterAngleDegrees,
		float coneWidthDegrees,
		float coneLengthRadiusInSquares,
		float coneBackwardOffsetInSquares,
		bool ignoreLoS,
		ActorData caster)
	{
		Vector3 coneDir = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float backwardOffset = coneBackwardOffsetInSquares * Board.Get().squareSize;
		Vector3 offsetConeStart = coneStart - coneDir * backwardOffset;
		float radiusInSquares = coneLengthRadiusInSquares + coneBackwardOffsetInSquares;
		float maxAngleWithCenter = coneWidthDegrees / 2f;
		bool result = false;
		if (IsSquareInRadius(testSquare, offsetConeStart.x, offsetConeStart.z, radiusInSquares, true, caster)
		    && IsSquareInRadius(testSquare, coneStart.x, coneStart.z, radiusInSquares, ignoreLoS, caster))
		{
			Vector3 dir = testSquare.ToVector3() - offsetConeStart;
			dir.y = 0f;
			if (dir.sqrMagnitude > 0f)
			{
				float testAngle = VectorUtils.HorizontalAngle_Deg(dir);
				result = IsAngleWithinCone(testAngle, coneCenterAngleDegrees, maxAngleWithCenter);
			}
		}
		return result;
	}

	public static bool IsSquareInConeByActorRadius(
		BoardSquare testSquare,
		Vector3 coneStart,
		float coneCenterAngleDegrees,
		float coneWidthDegrees,
		float coneLengthRadiusInSquares,
		float coneBackwardOffsetInSquares,
		bool ignoreLoS,
		ActorData caster,
		bool useLosOverridePos = false, // removed in rogues
		Vector3 losOverridePos = default(Vector3)) // removed in rogues
	{
		float squareSize = Board.Get().squareSize;
		Vector3 testPos = testSquare.ToVector3();
		Vector3 coneDir = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float backwardOffset = coneBackwardOffsetInSquares * squareSize;
		Vector3 offsetConeStart = coneStart - coneDir * backwardOffset;
		float radiusInSquares = coneLengthRadiusInSquares + coneBackwardOffsetInSquares;
		float radiusInWorld = radiusInSquares * squareSize;
		bool isInCone = IsPosInCone(testPos, offsetConeStart, radiusInWorld, coneCenterAngleDegrees, coneWidthDegrees);
		Vector3 dir = testPos - offsetConeStart;
		dir.y = 0f;
		float magnitude = dir.magnitude;
		if (!isInCone && magnitude > 0f)
		{
			float testRadius = GetActorTargetingRadius() * squareSize;
			Vector3 b = default(Vector3);
			b.x = dir.z;
			b.z = 0f - dir.x;
			b.y = 0f;
			b.Normalize();
			b *= testRadius;
			if (magnitude > radiusInWorld + testRadius)
			{
				isInCone = false;
			}
			else
			{
				float maxAngleWithCenter = 0.5f * coneWidthDegrees;
				Vector3 coneSideA = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees - maxAngleWithCenter);
				Vector3 coneSideB = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees + maxAngleWithCenter);
				if (VectorUtils.IsSegmentIntersectingCircle(offsetConeStart, offsetConeStart + radiusInWorld * coneSideA, testPos, testRadius)
				    || VectorUtils.IsSegmentIntersectingCircle(offsetConeStart, offsetConeStart + radiusInWorld * coneSideB, testPos, testRadius))
				{
					isInCone = true;
				}
				else if (magnitude < Mathf.Abs(radiusInWorld - testRadius))
				{
					isInCone = IsPosInCone(testPos + b, offsetConeStart, radiusInWorld, coneCenterAngleDegrees, coneWidthDegrees)
					           || IsPosInCone(testPos - b, offsetConeStart, radiusInWorld, coneCenterAngleDegrees, coneWidthDegrees);
				}
				else if (magnitude > 0f)
				{
					int circleCircleIntersections = GetCircleCircleIntersections(
						offsetConeStart,
						testPos,
						radiusInSquares,
						GetActorTargetingRadius(),
						out Vector3 intersectP1,
						out Vector3 intersectP2);
					if (circleCircleIntersections > 0)
					{
						isInCone = IsPosInAngleOfCone(intersectP1, offsetConeStart, coneCenterAngleDegrees, coneWidthDegrees);
					}
					if (!isInCone && circleCircleIntersections > 1)
					{
						isInCone = IsPosInAngleOfCone(intersectP2, offsetConeStart, coneCenterAngleDegrees, coneWidthDegrees);
					}
				}
			}
		}
		bool isInLoS = true;
		if (isInCone && !ignoreLoS)
		{
			Vector3 startPosForLoS = useLosOverridePos ? losOverridePos : offsetConeStart;  // always offsetConeStart in rogues
			BoardSquare startSquareForLoS = Board.Get().GetSquareFromVec3(startPosForLoS);
			isInLoS = startSquareForLoS != null && SquareHasLosForCone(startPosForLoS, startSquareForLoS, testSquare, caster);
		}

		return isInCone && isInLoS;
	}

	public static bool SquareHasLosForCone(Vector3 startPos, BoardSquare centerSquare, BoardSquare testSquare, ActorData caster)
	{
		if (!SquaresHaveLoSForAbilities(centerSquare, testSquare, caster))
		{
			return false;
		}
		
		Vector3 centerPos = centerSquare.ToVector3();
		Vector3 centerDir = centerPos - startPos;
		centerDir.y = 0f;
		if (centerDir.sqrMagnitude <= 0.1f)
		{
			return true;
		}
		
		Vector3 testPos = testSquare.ToVector3();
		Vector3 testDir = testPos - startPos;
		testDir.y = 0f;
		testDir.Normalize();
		Vector3 right = Vector3.Cross(Vector3.up, testDir);
		right.Normalize();
		Vector3 laserOffset = right * (VectorUtils.s_laserOffset * Board.Get().squareSize);
		startPos.y = Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		testPos.y = startPos.y;
		Vector3 offsetStartPosA = startPos + laserOffset;
		Vector3 offsetStartPosB = startPos - laserOffset;
		Vector3 dirA = testPos + laserOffset - offsetStartPosA;
		Vector3 dirB = testPos - laserOffset - offsetStartPosB;
		return !VectorUtils.RaycastInDirection(offsetStartPosA, dirA, dirA.magnitude, out _)
		       || !VectorUtils.RaycastInDirection(offsetStartPosB, dirB, dirB.magnitude, out _);
	}

	public static List<ActorData> GetActorsInCone(
		Vector3 coneStart,
		float coneCenterAngleDegrees,
		float coneWidthDegrees,
		float coneLengthRadius,
		float coneBackwardOffsetInSquares,
		bool ignoreLoS,
		ActorData caster,
		Team onlyValidTeam,
		List<NonActorTargetInfo> nonActorHitInfo,
		bool useLosOverridePos = false, // removed in rogues
		Vector3 losOverridePos = default(Vector3)) // removed in rogues
	{
		return GetActorsInCone(
			coneStart,
			coneCenterAngleDegrees,
			coneWidthDegrees,
			coneLengthRadius,
			coneBackwardOffsetInSquares,
			ignoreLoS,
			caster,
			new List<Team> { onlyValidTeam },
			nonActorHitInfo,
			useLosOverridePos, // removed in rogues
			losOverridePos); // removed in rogues
	}

	public static List<ActorData> GetActorsInCone(
		Vector3 coneStart,
		float coneCenterAngleDegrees,
		float coneWidthDegrees,
		float coneLengthRadiusInSquares,
		float coneBackwardOffsetInSquares,
		bool ignoreLoS,
		ActorData caster,
		List<Team> onlyValidTeams,
		List<NonActorTargetInfo> nonActorTargetInfo,
		bool useLosOverridePos = false, // removed in rogues
		Vector3 losOverridePos = default(Vector3)) // removed in rogues
	{
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			return GetActorsInConeByActorRadius(
				coneStart,
				coneCenterAngleDegrees,
				coneWidthDegrees,
				coneLengthRadiusInSquares,
				coneBackwardOffsetInSquares,
				ignoreLoS,
				caster,
				onlyValidTeams,
				nonActorTargetInfo,
				useLosOverridePos, // removed in rogues
				losOverridePos); // removed in rogues
		}
		List<ActorData> result = new List<ActorData>();
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (!IsActorTargetable(actorData, onlyValidTeams))
			{
				continue;
			}
			if (IsSquareInCone(
				    actorData.GetCurrentBoardSquare(),
				    coneStart,
				    coneCenterAngleDegrees,
				    coneWidthDegrees,
				    coneLengthRadiusInSquares,
				    coneBackwardOffsetInSquares,
				    ignoreLoS,
				    caster))
			{
				result.Add(actorData);
			}
		}
		return result;
	}

	public static List<ActorData> GetActorsInConeByActorRadius(
		Vector3 coneStart,
		float coneCenterAngleDegrees,
		float coneWidthDegrees,
		float coneLengthRadiusInSquares,
		float coneBackwardOffsetInSquares,
		bool ignoreLoS,
		ActorData caster,
		List<Team> onlyValidTeams = null,
		List<NonActorTargetInfo> nonActorHitInfo = null,
		bool useLosOverridePos = false, // removed in rogues
		Vector3 losOverridePos = default(Vector3)) // removed in rogues
	{
		List<ActorData> result = new List<ActorData>();
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (!IsActorTargetable(actorData, onlyValidTeams))
			{
				continue;
			}
			if (IsSquareInConeByActorRadius(
				    actorData.GetCurrentBoardSquare(),
				    coneStart,
				    coneCenterAngleDegrees,
				    coneWidthDegrees,
				    coneLengthRadiusInSquares,
				    coneBackwardOffsetInSquares,
				    ignoreLoS,
				    caster,
				    useLosOverridePos, // removed in rogues
				    losOverridePos)) // removed in rogues
			{
				result.Add(actorData);
			}
		}
#if SERVER
		// added in rogues
		if (nonActorHitInfo != null && !ignoreLoS)
		{
			PosInsideChecker_Cone checker = new PosInsideChecker_Cone(coneStart, coneLengthRadiusInSquares, coneCenterAngleDegrees, coneWidthDegrees);
			if (BarrierManager.Get() != null)
			{
				BarrierManager.Get().AddNonActorTargetInfoFromPos(caster, coneStart, nonActorHitInfo, checker);
			}
			// rogues
			// if (DynamicMissionGeoManager.Get() != null)
			// {
			// 	DynamicMissionGeoManager.Get().AddDestructibleGeoHits(nonActorHitInfo, posInsideChecker_Cone);
			// }
		}
#endif
		return result;
	}

	public static void GatherStretchConeDimensions(
		Vector3 aimPos,
		Vector3 coneStartPos,
		float minLengthInSquares,
		float maxLengthInSquares,
		float minWidthDegrees,
		float maxWidthDegrees,
		StretchConeStyle style,
		out float lengthInSquares,
		out float angleInDegrees,
		bool widthChangeDiscrete = false,
		int numWidthDiscreteChanges = 0,
		float interpMinDistOverrideInSquares = -1f,
		float interpRangeOverrideInSquares = -1f)
	{
		float lengthRange = maxLengthInSquares - minLengthInSquares;
		float widthRangeDegrees = maxWidthDegrees - minWidthDegrees;
		float maxLengthForStretchInSquares = maxLengthInSquares;
		float minLengthForStretchInSquares = minLengthInSquares;
		if (interpMinDistOverrideInSquares > 0f && interpRangeOverrideInSquares > 0f)
		{
			minLengthForStretchInSquares = interpMinDistOverrideInSquares;
			maxLengthForStretchInSquares = interpMinDistOverrideInSquares + interpRangeOverrideInSquares;
			lengthRange = interpRangeOverrideInSquares;
		}
		else if (lengthRange <= 0f)
		{
			maxLengthForStretchInSquares = Mathf.Max(3f, maxLengthInSquares);
			minLengthForStretchInSquares = 2.5f;
			lengthRange = maxLengthForStretchInSquares - minLengthForStretchInSquares;
		}
		
		if (lengthRange <= 0f)
		{
			lengthInSquares = maxLengthInSquares;
			angleInDegrees = maxWidthDegrees;
			return;
		}
		
		Vector3 aimVector = aimPos - coneStartPos;
		aimVector.y = 0f;
		float aimDist = aimVector.magnitude;
		float aimDistInSquares = aimDist / Board.Get().squareSize;
		lengthInSquares = Mathf.Clamp(aimDistInSquares, minLengthInSquares, maxLengthInSquares);
		float lengthForStretchInSquares = Mathf.Clamp(aimDistInSquares, minLengthForStretchInSquares, maxLengthForStretchInSquares);
		float lengthAlpha = (lengthForStretchInSquares - minLengthForStretchInSquares) / lengthRange;
		float widthAlphaInv;
		if (style == StretchConeStyle.Linear)
		{
			widthAlphaInv = lengthAlpha;
		}
		else if (style == StretchConeStyle.DistanceSquared)
		{
			widthAlphaInv = lengthAlpha * lengthAlpha;
		}
		else if (style == StretchConeStyle.DistanceCubed)
		{
			widthAlphaInv = lengthAlpha * lengthAlpha * lengthAlpha;
		}
		else if (style == StretchConeStyle.DistanceToTheFourthPower)
		{
			widthAlphaInv = lengthAlpha * lengthAlpha * lengthAlpha * lengthAlpha;
		}
		else if (style == StretchConeStyle.DistanceSquareRoot)
		{
			widthAlphaInv = Mathf.Sqrt(lengthAlpha);
		}
		else
		{
			widthAlphaInv = 0f;
		}
		float widthAlpha = 1f - widthAlphaInv;
		float angleAdd = widthRangeDegrees * widthAlpha;
		if (widthChangeDiscrete && numWidthDiscreteChanges > 0)
		{
			if (widthAlpha < 0.02f)
			{
				widthAlpha = 0f;
			}
			float step = 1f / numWidthDiscreteChanges;
			int stepIndex = Mathf.CeilToInt(widthAlpha / step);
			float widthAlphaStepped = stepIndex * (widthRangeDegrees / numWidthDiscreteChanges);
			angleAdd = Mathf.Clamp(widthAlphaStepped, 0f, widthRangeDegrees);
		}
		angleInDegrees = minWidthDegrees + angleAdd;
	}

	private static void AdjustMinMaxBounds(Vector3 point, ref Vector3 minBounds, ref Vector3 maxBounds)
	{
		minBounds.x = Mathf.Min(point.x, minBounds.x);
		minBounds.z = Mathf.Min(point.z, minBounds.z);
		maxBounds.x = Mathf.Max(point.x, maxBounds.x);
		maxBounds.z = Mathf.Max(point.z, maxBounds.z);
	}

	public static bool GetMaxConeBounds(
		Vector3 coneStart,
		float coneCenterAngleDegrees,
		float coneWidthDegrees,
		float coneLengthRadiusInSquares,
		float coneBackwardOffsetInSquares,
		out int minX,
		out int maxX,
		out int minY,
		out int maxY)
	{
		float radiusInSquares = coneLengthRadiusInSquares + coneBackwardOffsetInSquares;
		Vector3 coneDir = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float backwardOffset = coneBackwardOffsetInSquares * Board.Get().squareSize;
		Vector3 offsetConeStart = coneStart - coneDir * backwardOffset;
		float squareSize = Board.Get().squareSize;
		float radiusInWorld = radiusInSquares * squareSize;
		BoardSquare offsetConeStartSquare = Board.Get().GetSquareFromPos(offsetConeStart.x, offsetConeStart.z);
		if (offsetConeStartSquare == null)
		{
			minX = 0;
			maxX = 0;
			minY = 0;
			maxY = 0;
			return false;
		}

		float maxAngleWithCenter = 0.5f * coneWidthDegrees;
		float coneSideAngleA = coneCenterAngleDegrees - maxAngleWithCenter;
		float coneSideAngleB = coneCenterAngleDegrees + maxAngleWithCenter;
		float actorTargetingRadius = GetActorTargetingRadius() * Board.Get().squareSize;
		float radius = radiusInWorld + actorTargetingRadius;
		Vector3 coneEndA = offsetConeStart + radius * VectorUtils.AngleDegreesToVector(coneSideAngleA);
		Vector3 coneEndB = offsetConeStart + radius * VectorUtils.AngleDegreesToVector(coneSideAngleB);
		
		Vector3 minBounds = offsetConeStart;
		Vector3 maxBounds = offsetConeStart;
		AdjustMinMaxBounds(coneEndB, ref minBounds, ref maxBounds);
		AdjustMinMaxBounds(coneEndA, ref minBounds, ref maxBounds);
		
		for (int i = -90; i <= 450; i += 90)
		{
			if (coneSideAngleB <= i)
			{
				break;
			}
			if (coneSideAngleA >= i)
			{
				continue;
			}
			if (i < coneSideAngleB)
			{
				Vector3 coneEnd = offsetConeStart + radius * VectorUtils.AngleDegreesToVector(i);
				AdjustMinMaxBounds(coneEnd, ref minBounds, ref maxBounds);
			}
		}
		
		minX = (int)Mathf.Max(0f, minBounds.x / squareSize - 2f);
		maxX = (int)Mathf.Min(Board.Get().GetMaxX(), maxBounds.x / squareSize + 2f);
		minY = (int)Mathf.Max(0f, minBounds.z / squareSize - 2f);
		maxY = (int)Mathf.Min(Board.Get().GetMaxY(), maxBounds.z / squareSize + 2f);
		
		return true;
	}

	public static void OperateOnSquaresInCone(
		IOperationOnSquare operationObj,
		Vector3 coneStart,
		float coneCenterAngleDegrees,
		float coneWidthDegrees,
		float coneLengthRadiusInSquares,
		float coneBackwardOffsetInSquares,
		ActorData caster,
		bool ignoreLos,
		List<ISquareInsideChecker> losCheckOverrides = null)
	{
		Vector3 coneDir = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float backwardOffset = coneBackwardOffsetInSquares * Board.Get().squareSize;
		Vector3 offsetConeStart = coneStart - coneDir * backwardOffset;
		BoardSquare offsetConeStartSquare = Board.Get().GetSquareFromPos(offsetConeStart.x, offsetConeStart.z);
		if (offsetConeStartSquare == null)
		{
			return;
		}

		GetMaxConeBounds(
			coneStart,
			coneCenterAngleDegrees,
			coneWidthDegrees,
			coneLengthRadiusInSquares,
			coneBackwardOffsetInSquares,
			out int minX,
			out int maxX,
			out int minY,
			out int maxY);
		
		bool shouldEarlyOut = false;
		for (int i = minX; i < maxX; i++)
		{
			if (shouldEarlyOut)
			{
				return;
			}
			for (int j = minY; j < maxY; j++)
			{
				if (shouldEarlyOut)
				{
					break;
				}
				BoardSquare boardSquare = Board.Get().GetSquareFromIndex(i, j);
				if (boardSquare == null
				    || !boardSquare.IsValidForGameplay()
				    || !IsSquareInConeByActorRadius(
					    boardSquare,
					    coneStart,
					    coneCenterAngleDegrees,
					    coneWidthDegrees,
					    coneLengthRadiusInSquares,
					    coneBackwardOffsetInSquares,
					    true,
					    caster))
				{
					continue;
				}
				bool squareHasLoS = false;
				if (ignoreLos)
				{
					squareHasLoS = true;
				}
				else if (losCheckOverrides != null)
				{
					squareHasLoS = SquareHasLosByAreaCheckers(boardSquare, losCheckOverrides);
				}
				else
				{
					squareHasLoS = SquareHasLosForCone(offsetConeStart, offsetConeStartSquare, boardSquare, caster);
				}
				operationObj.OperateOnSquare(boardSquare, caster, squareHasLoS);
				shouldEarlyOut = operationObj.ShouldEarlyOut();
			}
			
		}
	}

	public static List<ActorData> GetActorsInRadiusOfLine(
		Vector3 startPos,
		Vector3 endPos,
		float startRadiusInSquares,
		float endRadiusInSquares,
		float rangeFromLineInSquares,
		bool ignoreLoS,
		ActorData caster,
		Team onlyValidTeam,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return GetActorsInRadiusOfLine(
			startPos,
			endPos,
			startRadiusInSquares,
			endRadiusInSquares,
			rangeFromLineInSquares,
			ignoreLoS,
			caster,
			new List<Team> { onlyValidTeam },
			nonActorTargetInfo);
	}

	public static List<ActorData> GetActorsInRadiusOfLine(
		Vector3 startPos,
		Vector3 endPos,
		float startRadiusInSquares,
		float endRadiusInSquares,
		float rangeFromLineInSquares,
		bool ignoreLoS,
		ActorData caster,
		List<Team> onlyValidTeams,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> result = new List<ActorData>();
		if (rangeFromLineInSquares > 0f)
		{
			float laserWidthInSquares = rangeFromLineInSquares * 2f;
			List<Vector3> additionalLosSources = new List<Vector3>();
			if (!ignoreLoS)
			{
				additionalLosSources.Add(startPos);
				additionalLosSources.Add((startPos + endPos) * 0.5f);
				additionalLosSources.Add(endPos);
			}
			List<ActorData> actorsInBox = GameWideData.Get().UseActorRadiusForLaser()
				? GetActorsInBoxByActorRadius(startPos, endPos, laserWidthInSquares, ignoreLoS, caster, onlyValidTeams, additionalLosSources)
				: GetActorsInBox(startPos, endPos, laserWidthInSquares, additionalLosSources, caster, onlyValidTeams);
			foreach (ActorData actor in actorsInBox)
			{
				if (!result.Contains(actor))
				{
					result.Add(actor);
				}
			}
			
#if SERVER
			// added in rogues
			if (nonActorTargetInfo != null && !ignoreLoS)
			{
				PosInsideChecker_Box checker = new PosInsideChecker_Box(startPos, endPos, rangeFromLineInSquares);
				if (BarrierManager.Get() != null)
				{
					List<Vector3> additionalLosCheckPos = new List<Vector3>();
					for (int i = 1; i < additionalLosSources.Count; i++)
					{
						additionalLosCheckPos.Add(additionalLosSources[i]);
					}
					BarrierManager.Get().AddNonActorTargetInfoFromPos(caster, startPos, nonActorTargetInfo, checker, additionalLosCheckPos);
				}
				// rogues
				// if (DynamicMissionGeoManager.Get() != null)
				// {
				// 	DynamicMissionGeoManager.Get().AddDestructibleGeoHits(nonActorTargetInfo, posInsideChecker_Box);
				// }
			}
#endif
		}

		if (startRadiusInSquares > 0f)
		{
			List<ActorData> actorsInRadius = GetActorsInRadius(startPos, startRadiusInSquares, ignoreLoS, caster, onlyValidTeams, nonActorTargetInfo);
			foreach (ActorData item in actorsInRadius)
			{
				if (!result.Contains(item))
				{
					result.Add(item);
				}
			}
		}
		if (endRadiusInSquares > 0f)
		{
			List<ActorData> actorsInRadius = GetActorsInRadius(endPos, endRadiusInSquares, ignoreLoS, caster, onlyValidTeams, nonActorTargetInfo);
			foreach (ActorData actor in actorsInRadius)
			{
				if (!result.Contains(actor))
				{
					result.Add(actor);
				}
			}
		}
		return result;
	}

	public static void OperateOnSquaresInRadiusOfLine(
		IOperationOnSquare operationObj,
		Vector3 startPos,
		Vector3 endPos,
		float startRadiusInSquares,
		float endRadiusInSquares,
		float rangeFromLineInSquares,
		bool ignoreLoS,
		ActorData caster)
	{
		if (s_radiusOfLineLosCheckers.Count == 0)
		{
			s_radiusOfLineLosCheckers.Add(new SquareInsideChecker_Box(1f));
			s_radiusOfLineLosCheckers.Add(new SquareInsideChecker_Cone());
			s_radiusOfLineLosCheckers.Add(new SquareInsideChecker_Cone());
		}
		List<Vector3> additionalLosSources = new List<Vector3>();
		if (!ignoreLoS)
		{
			additionalLosSources.Add(startPos);
			additionalLosSources.Add((startPos + endPos) * 0.5f);
			additionalLosSources.Add(endPos);
		}
		SquareInsideChecker_Box checkerBox = s_radiusOfLineLosCheckers[0] as SquareInsideChecker_Box;
		SquareInsideChecker_Cone checkerConeA = s_radiusOfLineLosCheckers[1] as SquareInsideChecker_Cone;
		SquareInsideChecker_Cone checkerConeB = s_radiusOfLineLosCheckers[2] as SquareInsideChecker_Cone;
		float widthInSquares = rangeFromLineInSquares * 2f;
		checkerBox.UpdateBoxProperties(startPos, endPos, caster);
		checkerBox.m_penetrateLos = ignoreLoS;
		checkerBox.m_widthInSquares = widthInSquares;
		checkerBox.m_additionalLosSources = additionalLosSources;
		checkerConeA.UpdateConeProperties(startPos, 360f, startRadiusInSquares, 0f, 0f, caster);  // , ignoreLoS in rogues
		checkerConeB.UpdateConeProperties(endPos, 360f, endRadiusInSquares, 0f, 0f, caster);  // , ignoreLoS in rogues
		if (rangeFromLineInSquares > 0f)
		{
			OperateOnSquaresInBoxByActorRadius(
				operationObj,
				startPos,
				endPos,
				widthInSquares,
				caster,
				ignoreLoS,
				additionalLosSources,
				s_radiusOfLineLosCheckers);
		}
		if (startRadiusInSquares > 0f)
		{
			OperateOnSquaresInCone(
				operationObj,
				startPos,
				0f,
				360f,
				startRadiusInSquares,
				0f,
				caster,
				ignoreLoS,
				s_radiusOfLineLosCheckers);
		}
		if (endRadiusInSquares > 0f)
		{
			OperateOnSquaresInCone(
				operationObj,
				endPos,
				0f,
				360f,
				endRadiusInSquares,
				0f,
				caster,
				ignoreLoS,
				s_radiusOfLineLosCheckers);
		}
	}

	public static List<BoardSquare> GetSquaresInBox(Vector3 pt1, Vector3 pt2, float halfWidthInSquares, bool ignoreLoS, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		if (!ignoreLoS)
		{
			list.Add(pt1);
		}
		return GetSquaresInBox(pt1, pt2, halfWidthInSquares, list, caster);
	}

	public static List<BoardSquare> GetSquaresInBox(Vector3 pt1, Vector3 pt2, float halfWidthInSquares, List<Vector3> losCheckPoints, ActorData caster)
	{
		float squareSize = Board.Get().squareSize;
		float halfWidth = halfWidthInSquares * squareSize;
		pt1.y = 0f;
		pt2.y = 0f;
		int xMin = Mathf.Max(0, (int)(Mathf.Min(pt1.x - halfWidth, pt2.x - halfWidth) / squareSize));
		int yMin = Mathf.Max(0, (int)(Mathf.Min(pt1.z - halfWidth, pt2.z - halfWidth) / squareSize));
		int xMax = Mathf.Min(Board.Get().GetMaxX(), (int)(Mathf.Max(pt1.x + halfWidth, pt2.x + halfWidth) / squareSize) + 1);
		int yMax = Mathf.Min(Board.Get().GetMaxY(), (int)(Mathf.Max(pt1.z + halfWidth, pt2.z + halfWidth) / squareSize) + 1);
		List<BoardSquare> result = new List<BoardSquare>();
		for (int i = xMin; i < xMax; i++)
		{
			for (int j = yMin; j < yMax; j++)
			{
				BoardSquare square = Board.Get().GetSquareFromIndex(i, j);
				Vector3 testPt = new Vector3(square.worldX, 0f, square.worldY);
				if (!PointInBox(testPt, pt1, pt2, halfWidth))
				{
					continue;
				}
				bool hasLoS;
				if (losCheckPoints != null && losCheckPoints.Count > 0)
				{
					hasLoS = false;
					foreach (Vector3 losCheckPoint in losCheckPoints)
					{
						BoardSquare losCheckSquare = Board.Get().GetSquareFromVec3(losCheckPoint);
						if (losCheckSquare != null && SquaresHaveLoSForAbilities(losCheckSquare, square, caster))
						{
							hasLoS = true;
							break;
						}
					}
				}
				else
				{
					hasLoS = true;
				}
				if (hasLoS)
				{
					result.Add(square);
				}
			}
		}
		return result;
	}

	public static List<ActorData> GetActorsInBox(
		Vector3 startPos,
		Vector3 endPos,
		float boxWidthInSquares,
		bool ignoreLoS,
		ActorData caster,
		List<Team> onlyValidTeams)
	{
		List<Vector3> list = new List<Vector3>();
		if (!ignoreLoS)
		{
			list.Add(startPos);
		}
		return GetActorsInBox(startPos, endPos, boxWidthInSquares, list, caster, onlyValidTeams);
	}

	public static List<ActorData> GetActorsInBox(
		Vector3 startPos,
		Vector3 endPos,
		float boxWidthInSquares,
		List<Vector3> losCheckPoints,
		ActorData caster,
		List<Team> onlyValidTeams)
	{
		List<ActorData> result = new List<ActorData>();
		if (!(boxWidthInSquares > 0f))
		{
			return result;
		}
		
		List<BoardSquare> squaresInBox = GetSquaresInBox(startPos, endPos, boxWidthInSquares / 2f, losCheckPoints, caster);
		foreach (BoardSquare square in squaresInBox)
		{
			ActorData occupantActor = square.OccupantActor;
			if (IsActorTargetable(occupantActor, onlyValidTeams) && !result.Contains(occupantActor))
			{
				result.Add(occupantActor);
			}
		}
		return result;
	}

	internal static void GetBoxBoundsInGridPos(
		Vector3 startPos,
		Vector3 endPos,
		float adjustAmount,
		out int minX,
		out int minY,
		out int maxX,
		out int maxY)
	{
		float squareSize = Board.Get().squareSize;
		int maxX2 = Board.Get().GetMaxX();
		int maxY2 = Board.Get().GetMaxY();
		minX = Mathf.Max(0, (int)(Mathf.Min(startPos.x - adjustAmount, endPos.x - adjustAmount) / squareSize) - 1);
		minY = Mathf.Max(0, (int)(Mathf.Min(startPos.z - adjustAmount, endPos.z - adjustAmount) / squareSize) - 1);
		maxX = Mathf.Min(maxX2, (int)(Mathf.Max(startPos.x + adjustAmount, endPos.x + adjustAmount) / squareSize) + 1);
		maxY = Mathf.Min(maxY2, (int)(Mathf.Max(startPos.z + adjustAmount, endPos.z + adjustAmount) / squareSize) + 1);
	}

	private static void GetBoxCorners(
		Vector3 startPos,
		Vector3 endPos,
		float widthInSquares,
		out Vector3 ptA,
		out Vector3 ptB,
		out Vector3 ptC,
		out Vector3 ptD)
	{
		Vector3 dir = (endPos - startPos).normalized;
		Vector3 right = Vector3.Cross(dir, Vector3.up).normalized;
		float widthInWorld = widthInSquares * Board.Get().squareSize;
		Vector3 halfWithVector = 0.5f * widthInWorld * right;
		ptA = endPos - halfWithVector;
		ptB = endPos + halfWithVector;
		ptC = startPos - halfWithVector;
		ptD = startPos + halfWithVector;
	}

	private static bool IsBoxBorderTouchingCircle(
		Vector3 ptA,
		Vector3 ptB,
		Vector3 ptC,
		Vector3 ptD,
		Vector3 circleCenter,
		float radiusInWorld)
	{
		circleCenter.y = 0f;
		return VectorUtils.IsSegmentIntersectingCircle(ptA, ptB, circleCenter, radiusInWorld)
		       || VectorUtils.IsSegmentIntersectingCircle(ptB, ptD, circleCenter, radiusInWorld)
		       || VectorUtils.IsSegmentIntersectingCircle(ptD, ptC, circleCenter, radiusInWorld)
		       || VectorUtils.IsSegmentIntersectingCircle(ptC, ptA, circleCenter, radiusInWorld);
	}

	public static List<BoardSquare> GetSquaresInBoxByActorRadius(
		Vector3 startPos,
		Vector3 endPos,
		float laserWidthInSquares,
		bool penetrateLos,
		ActorData caster,
		List<Vector3> additionalLosSources = null)
	{
		startPos.y = 0f;
		endPos.y = 0f;
		BoardSquare startSquare = Board.Get().GetSquareFromVec3(startPos);
		float actorTargetingRadius = GetActorTargetingRadius() * Board.Get().squareSize;
		float laserWidth = laserWidthInSquares * Board.Get().squareSize;
		float laserHalfWidth = 0.5f * laserWidth;
		GetBoxCorners(startPos, endPos, laserWidthInSquares, out Vector3 ptA, out Vector3 ptB, out Vector3 ptC, out Vector3 ptD);
		float adjustAmount = laserHalfWidth + actorTargetingRadius;
		GetBoxBoundsInGridPos(startPos, endPos, adjustAmount, out int minX, out int minY, out int maxX, out int maxY);
		List<BoardSquare> result = new List<BoardSquare>();
		for (int i = minX; i <= maxX; i++)
		{
			for (int j = minY; j <= maxY; j++)
			{
				BoardSquare square = Board.Get().GetSquareFromIndex(i, j);
				if (square == null)
				{
					continue;
				}
				Vector3 pos = square.ToVector3();
				pos.y = 0f;

				if (!PointInBox(pos, startPos, endPos, laserHalfWidth)
				    && !IsBoxBorderTouchingCircle(ptA, ptB, ptC, ptD, pos, actorTargetingRadius))
				{
					continue;
				}
				
				if (penetrateLos)
				{
					result.Add(square);
				}
				else if (HasLosForLaserByActorRadius(startSquare, square, startPos, endPos, laserWidthInSquares, caster))
				{
					result.Add(square);
				}
				else if (additionalLosSources != null)
				{
					foreach (Vector3 additionalLosSource in additionalLosSources)
					{
						BoardSquare losSquare = Board.Get().GetSquareFromVec3(additionalLosSource);
						if (losSquare != null && SquaresHaveLoSForAbilities(losSquare, square, caster))
						{
							result.Add(square);
							break;
						}
					}
				}
			}
		}
		return result;
	}

	public static bool IsSquareInBoxByActorRadius(BoardSquare testSquare, Vector3 startPos, Vector3 endPos, float laserWidthInSquares)
	{
		if (testSquare == null)
		{
			return false;
		}
		
		startPos.y = 0f;
		endPos.y = 0f;
		Vector3 vector = testSquare.ToVector3();
		vector.y = 0f;
		GetBoxCorners(startPos, endPos, laserWidthInSquares, out Vector3 ptA, out Vector3 ptB, out Vector3 ptC, out Vector3 ptD);
		float actorTargetingRadius = GetActorTargetingRadius() * Board.Get().squareSize;
		float laserWidth = laserWidthInSquares * Board.Get().squareSize;
		float laserHalfWidth = 0.5f * laserWidth;
		return PointInBox(vector, startPos, endPos, laserHalfWidth)
		       || IsBoxBorderTouchingCircle(ptA, ptB, ptC, ptD, vector, actorTargetingRadius);
	}

	public static bool IsSquareInLosForBox(
		BoardSquare square,
		Vector3 startPos,
		Vector3 endPos,
		float laserWidthInSquares,
		bool penetrateLos,
		ActorData caster,
		List<Vector3> additionalLosSources = null)
	{
		BoardSquare startSquare = Board.Get().GetSquareFromVec3(startPos);
		bool result = false;
		if (penetrateLos)
		{
			result = true;
		}
		else if (HasLosForLaserByActorRadius(startSquare, square, startPos, endPos, laserWidthInSquares, caster))
		{
			result = true;
		}
		else if (additionalLosSources != null)
		{
			foreach (Vector3 additionalLosSource in additionalLosSources)
			{
				BoardSquare losSourceSquare = Board.Get().GetSquareFromVec3(additionalLosSource);
				if (losSourceSquare != null && SquaresHaveLoSForAbilities(losSourceSquare, square, caster))
				{
					return true;
				}
			}
		}
		return result;
	}

	public static List<ActorData> GetActorsInBoxByActorRadius(
		Vector3 startPos,
		Vector3 endPos,
		float laserWidthInSquares,
		bool penetrateLos,
		ActorData caster,
		List<Team> validTeams,
		List<Vector3> additionalLosSources = null,
		List<NonActorTargetInfo> nonActorTargetInfo = null)
	{
		List<ActorData> result = new List<ActorData>();
		if (!(laserWidthInSquares > 0f))
		{
			return result;
		}
		
		startPos.y = 0f;
		endPos.y = 0f;
		BoardSquare startSquare = Board.Get().GetSquareFromVec3(startPos);
		float actorTargetingRadius = GetActorTargetingRadius() * Board.Get().squareSize;
		float laserWidth = laserWidthInSquares * Board.Get().squareSize;
		float laserHalfWidth = 0.5f * laserWidth;
		GetBoxCorners(startPos, endPos, laserWidthInSquares, out Vector3 ptA, out Vector3 ptB, out Vector3 ptC, out Vector3 ptD);
		foreach (ActorData actor in GameFlowData.Get().GetActors())
		{
			if (!IsActorTargetable(actor, validTeams))
			{
				continue;
			}
					
			Vector3 actorPos = actor.GetFreePos();
			actorPos.y = 0f;
			if (!PointInBox(actorPos, startPos, endPos, laserHalfWidth)
			    && !IsBoxBorderTouchingCircle(ptA, ptB, ptC, ptD, actorPos, actorTargetingRadius))
			{
				continue;
			}
					
			if (penetrateLos)
			{
				result.Add(actor);
			}
			else
			{
				BoardSquare actorSquare = actor.GetCurrentBoardSquare();
				if (HasLosForLaserByActorRadius(startSquare, actorSquare, startPos, endPos, laserWidthInSquares, caster, nonActorTargetInfo))
				{
					result.Add(actor);
				}
				else if (additionalLosSources != null)
				{
					foreach (Vector3 additionalLosSource in additionalLosSources)
					{
						BoardSquare losSourceSquare = Board.Get().GetSquareFromVec3(additionalLosSource);
						if (losSourceSquare != null && SquaresHaveLoSForAbilities(losSourceSquare, actorSquare, caster))
						{
							result.Add(actor);
							break;
						}
					}
				}
			}
		}
		return result;
	}

	public static bool HasLosForLaserByActorRadius(
		BoardSquare startSquare,
		BoardSquare testSquare,
		Vector3 laserStart,
		Vector3 laserEnd,
		float widthInSquares,
		ActorData caster,
		List<NonActorTargetInfo> nonActorTargetInfo = null)
	{
		List<NonActorTargetInfo> nonActorTargetInfoLocal = nonActorTargetInfo != null ? new List<NonActorTargetInfo>() : null;
		Vector3 laserVector = laserEnd - laserStart;
		laserVector.y = 0f;
		Vector3 laserDir = laserVector.normalized;
		Vector3 testPos = testSquare.ToVector3();
		Vector3 casterLosCheckPos = caster.GetLoSCheckPos();
		Vector3 startPos = startSquare.ToVector3();
		startPos.y = casterLosCheckPos.y;
		Vector3 testVector = testPos - startPos;
		testVector.y = 0f;
		bool hasLosForAbilities = SquaresHaveLoSForAbilities(startSquare, testSquare, caster, false);
		bool hasLosByBarriers = HasLosByBarriers(
			startSquare,
			testSquare,
			caster,
			VectorUtils.s_laserOffset * Board.SquareSizeStatic,
			nonActorTargetInfoLocal);
		
		float targetingRadiusInSquares = 0.72f;
		if (caster.GetCurrentBoardSquare() == startSquare)
		{
			targetingRadiusInSquares += GameWideData.Get().m_actorTargetingRadiusInSquares;
		}
		Vector3 adjustedTestPos = testPos - targetingRadiusInSquares * Board.Get().squareSize * laserDir;
		Vector3 adjustedTestVector = adjustedTestPos - laserStart;
		adjustedTestVector.y = 0f;
		Vector3 losCheckPos = laserStart + Vector3.Dot(adjustedTestVector, laserDir) * laserDir;
		if (Vector3.Dot(laserDir, losCheckPos - laserStart) <= 0f)
		{
			losCheckPos = startPos;
		}
		bool hasLos = true;
		BoardSquare losCheckSquare = Board.Get().GetSquareFromVec3(losCheckPos);
		if (losCheckSquare != null && losCheckSquare != testSquare && losCheckSquare.IsValidForGameplay())
		{
			hasLos = losCheckSquare.GetLOS(testSquare.x, testSquare.y);
			if (hasLosByBarriers)
			{
				hasLosByBarriers = HasLosByBarriers(
					losCheckSquare,
					testSquare,
					caster,
					VectorUtils.s_laserOffset * Board.SquareSizeStatic,
					nonActorTargetInfoLocal);
				if (!hasLosByBarriers)
				{
					hasLos = false;
				}
			}
		}
		
		float offsetRange = 0.35f * Board.Get().squareSize;
		Vector3 offset = Mathf.Abs(laserDir.x) <= Mathf.Abs(laserDir.z)
			? new Vector3(offsetRange, 0f, 0f)
			: new Vector3(0f, 0f, offsetRange);
		if (hasLosForAbilities && !hasLosByBarriers)
		{
			nonActorTargetInfo?.AddRange(nonActorTargetInfoLocal);
		}
		if (hasLosByBarriers && hasLos && !hasLosForAbilities)
		{
			Vector3 testPos2 = testPos;
			testPos2.y = casterLosCheckPos.y;
			Vector3 testVector2 = testPos2 - startPos;
			testVector2.y = 0f;
			hasLosForAbilities = !VectorUtils.RaycastInDirection(startPos + offset, testVector2, testVector2.magnitude, out _)
			                     || !VectorUtils.RaycastInDirection(startPos - offset, testVector2, testVector2.magnitude, out _);
		}

		return hasLosByBarriers && hasLosForAbilities && hasLos;
	}
	
	public static void OperateOnSquaresInBoxByActorRadius(
		IOperationOnSquare operationObj,
		Vector3 startPos,
		Vector3 endPos,
		float widthInSquares,
		ActorData caster,
		bool ignoreLos,
		List<Vector3> additionalLosSources = null,
		List<ISquareInsideChecker> losCheckOverrides = null,
		bool applyLaserStartOffset = true)
	{
		startPos.y = 0f;
		endPos.y = 0f;
		BoardSquare startSquare = Board.Get().GetSquareFromVec3(startPos);
		if (applyLaserStartOffset)
		{
			float actorTargetingRadiusInSquares = GameWideData.Get().m_actorTargetingRadiusInSquares;
			if (actorTargetingRadiusInSquares > 0f)
			{
				startPos = VectorUtils.GetAdjustedStartPosWithOffset(startPos, endPos, actorTargetingRadiusInSquares);
			}
		}
		float actorTargetingRadius = GetActorTargetingRadius() * Board.Get().squareSize;
		float width = widthInSquares * Board.Get().squareSize;
		float halfWidth = 0.5f * width;
		GetBoxCorners(startPos, endPos, widthInSquares, out Vector3 ptA, out Vector3 ptB, out Vector3 ptC, out Vector3 ptD);
		float adjustAmount = halfWidth + actorTargetingRadius;
		GetBoxBoundsInGridPos(startPos, endPos, adjustAmount, out int minX, out int minY, out int maxX, out int maxY);
		for (int i = minX; i <= maxX; i++)
		{
			for (int j = minY; j <= maxY; j++)
			{
				BoardSquare square = Board.Get().GetSquareFromIndex(i, j);
				if (square == null)
				{
					continue;
				}
				Vector3 pos = square.ToVector3();
				pos.y = 0f;
				if (!PointInBox(pos, startPos, endPos, halfWidth)
				    && !IsBoxBorderTouchingCircle(ptA, ptB, ptC, ptD, pos, actorTargetingRadius))
				{
					continue;
				}
				bool squareHasLos = false;
				if (ignoreLos)
				{
					squareHasLos = true;
				}
				else if (losCheckOverrides != null)
				{
					squareHasLos = SquareHasLosByAreaCheckers(square, losCheckOverrides);
				}
				else
				{
					squareHasLos = HasLosForLaserByActorRadius(startSquare, square, startPos, endPos, widthInSquares, caster);
					if (!squareHasLos && additionalLosSources != null)
					{
						foreach (Vector3 additionalLosSource in additionalLosSources)
						{
							BoardSquare losSourceSquare = Board.Get().GetSquareFromVec3(additionalLosSource);
							if (losSourceSquare != null && SquaresHaveLoSForAbilities(losSourceSquare, square, caster))
							{
								squareHasLos = true;
								break;
							}
						}
					}
				}
				operationObj.OperateOnSquare(square, caster, squareHasLos);
			}
		}
	}

#if SERVER
	// added in rogues
	public static Vector3 GetKnockbackOriginFromLaser(
		List<ActorData> distanceSortedActors,
		ActorData caster,
		Vector3 aimDir,
		Vector3 endPos)
	{
		if (distanceSortedActors.Count <= 0)
		{
			return endPos;
		}
		ActorData closestActor = distanceSortedActors[0];
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		Vector3 vector = Vector3.Project(closestActor.GetLoSCheckPos() - loSCheckPos, aimDir.normalized);
		return vector.sqrMagnitude < Board.SquareSizeStatic * Board.SquareSizeStatic
			? caster.GetLoSCheckPos()
			: loSCheckPos + vector - aimDir * Board.SquareSizeStatic;
	}
#endif

	// TODO make sure we properly use laserEndPos on server -- it is modified even if there are not enough targets hit!
	public static List<ActorData> GetActorsInLaser(
		Vector3 startPos,
		Vector3 dir,
		float laserRangeInSquares,
		float laserWidthInSquares,
		ActorData caster,
		List<Team> validTeams,
		bool penetrateLos,
		int maxTargets,
		bool lengthIgnoreLevelGeo,
		bool includeInvisibles,
		out Vector3 laserEndPos,
		List<NonActorTargetInfo> nonActorTargets,
		List<ActorData> actorsToExclude = null,
		bool ignoreStartOffset = false,
		bool excludeCaster = true)
	{
		dir.y = 0f;
		dir.Normalize();
		float maxDistanceInWorld = laserRangeInSquares * Board.Get().squareSize;
		bool ignoreLos = penetrateLos || lengthIgnoreLevelGeo;
		List<NonActorTargetInfo> nonActorTargetsLocal = nonActorTargets != null ? new List<NonActorTargetInfo>() : null;
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = startPos;
		bool checkBarriers = laserWidthInSquares > 0f;
		laserCoords.end = VectorUtils.GetLaserEndPoint(startPos, dir, maxDistanceInWorld, penetrateLos, caster, nonActorTargetsLocal, checkBarriers);
		if (ignoreLos && !penetrateLos)
		{
			laserCoords.end = VectorUtils.GetLaserEndPoint(startPos, dir, maxDistanceInWorld, ignoreLos, caster, null, checkBarriers);
		}
		if (!penetrateLos
		    && laserWidthInSquares > 2f
		    && nonActorTargetsLocal != null
		    && nonActorTargetsLocal.Count == 0)
		{
			Vector3 right = Vector3.Cross(Vector3.up, dir);
			right.Normalize();
			right *= 0.5f * laserWidthInSquares * Board.SquareSizeStatic;
			Vector3 startPosA = startPos + right;
			Vector3 startPosB = startPos - right;
			float magnitude = (laserCoords.end - laserCoords.start).magnitude;
			VectorUtils.GetLaserEndPoint(startPosA, dir, magnitude, penetrateLos, caster, nonActorTargetsLocal, checkBarriers);
			if (nonActorTargetsLocal.Count == 0)
			{
				VectorUtils.GetLaserEndPoint(startPosB, dir, magnitude, penetrateLos, caster, nonActorTargetsLocal, checkBarriers);
			}
		}
		float actorTargetingRadiusInSquares = GameWideData.Get().m_actorTargetingRadiusInSquares;
		if (actorTargetingRadiusInSquares > 0f && !ignoreStartOffset)
		{
			laserCoords.start = VectorUtils.GetAdjustedStartPosWithOffset(laserCoords.start, laserCoords.end, actorTargetingRadiusInSquares);
		}

		List<ActorData> actors = GameWideData.Get().UseActorRadiusForLaser()
			? GetActorsInBoxByActorRadius(
				laserCoords.start,
				laserCoords.end,
				laserWidthInSquares,
				penetrateLos,
				caster,
				validTeams,
				null,
				nonActorTargets)
			: GetActorsInBox(laserCoords.start, laserCoords.end, laserWidthInSquares, penetrateLos, caster, validTeams);
		if (!includeInvisibles)
		{
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		}
		if (actorsToExclude != null)
		{
			foreach (ActorData actor in actorsToExclude)
			{
				if (actors.Contains(actor))
				{
					actors.Remove(actor);
				}
			}
		}
		if (excludeCaster)
		{
			actors.Remove(caster);
		}
		TargeterUtils.SortActorsByDistanceToPos(ref actors, startPos, dir);
		TargeterUtils.LimitActorsToMaxNumber(ref actors, maxTargets);
		if (maxTargets > 0 && actors.Count == maxTargets)
		{
			ActorData actorData = actors[actors.Count - 1];
			Vector3 lhs = actorData.GetFreePos() - laserCoords.start;
			lhs.y = 0f;
			Vector3 b2 = Vector3.Dot(lhs, dir) * dir;
			laserEndPos = laserCoords.start + b2;  // laserCoords.start + dir * b2.magnitude; in rogues
		}
		else
		{
			laserEndPos = laserCoords.end;  // laserCoords.start + dir * laserCoords.Length(); in rogues
			if (nonActorTargets != null && nonActorTargetsLocal != null)
			{
				foreach (NonActorTargetInfo nonActorTargetInfo in nonActorTargetsLocal)
				{
					nonActorTargets.Add(nonActorTargetInfo);
				}
			}
		}
		return actors;
	}

	public static bool LaserHitWorldGeo(
		float maxDistanceInSquares,
		VectorUtils.LaserCoords adjustedCoords,
		bool penetrateLos,
		List<ActorData> actorsHit)
	{
		if (penetrateLos || (actorsHit != null && actorsHit.Count > 0))
		{
			return false;
		}
		float maxDist = maxDistanceInSquares * Board.Get().squareSize;
		return adjustedCoords.Length() < maxDist - 0.1f;
	}

	public static Dictionary<ActorData, BouncingLaserInfo> FindBouncingLaserTargets(
		Vector3 originalStart,
		ref List<Vector3> laserAnglePoints,
		float widthInSquares,
		List<Team> validTeamsToHit,
		int maxTargetsHit,
		bool includeInvisibles,
		ActorData caster,
		List<ActorData> orderedHitActors = null,
		bool includeActorsAtAnglePoints = false)
	{
		Dictionary<ActorData, BouncingLaserInfo> dictionary = new Dictionary<ActorData, BouncingLaserInfo>();
		if (orderedHitActors != null)
		{
			orderedHitActors.Clear();
		}
		Vector3 startPos = originalStart;
		int numTargetsHit = 0;
		int endSegment = -1;
		Vector3 lastBounceVector = originalStart;
		
		for (int i = 0; i < laserAnglePoints.Count; i++)
		{
			Vector3 endPos = laserAnglePoints[i];
			List<ActorData> actors = GameWideData.Get().UseActorRadiusForLaser()
				? GetActorsInBoxByActorRadius(startPos, endPos, widthInSquares, false, caster, validTeamsToHit)
				: GetActorsInBox(startPos, endPos, widthInSquares, true, caster, validTeamsToHit);
			actors.Remove(caster);
			if (includeActorsAtAnglePoints)
			{
				BoardSquare endSquare = Board.Get().GetSquareFromVec3(endPos);
				ActorData occupantActor = endSquare.OccupantActor;
				if (occupantActor != null
				    && !actors.Contains(occupantActor)
				    && IsRelevantTeam(validTeamsToHit, occupantActor.GetTeam()))
				{
					actors.Add(occupantActor);
				}
			}
			TargeterUtils.SortActorsByDistanceToPos(ref actors, startPos);
				
			foreach (ActorData actorData in actors)
			{
				if (!dictionary.ContainsKey(actorData) && (includeInvisibles || actorData.IsActorVisibleToClient()))
				{
					BouncingLaserInfo bounceInfo = new BouncingLaserInfo(startPos, i);
					dictionary.Add(actorData, bounceInfo);
					if (orderedHitActors != null)
					{
						orderedHitActors.Add(actorData);
					}

					numTargetsHit++;
					if (numTargetsHit >= maxTargetsHit && maxTargetsHit > 0)
					{
						endSegment = i;
						Vector3 bounceDir = (endPos - startPos).normalized;
						Vector3 bounceHitVector = actorData.GetFreePos() - startPos;
						lastBounceVector = startPos + Vector3.Dot(bounceDir, bounceHitVector) * bounceDir;
						break;
					}
				}
			}
			if (endSegment != -1)
			{
				break;
			}
			startPos = endPos;
		}
		if (endSegment != -1 && maxTargetsHit > 0)
		{
			laserAnglePoints[endSegment] = lastBounceVector;
			int numSegmentsToRemove = laserAnglePoints.Count - 1 - endSegment;
			if (numSegmentsToRemove > 0)
			{
				laserAnglePoints.RemoveRange(endSegment + 1, numSegmentsToRemove);
			}
		}
		return dictionary;
	}

	public static void OperateOnSquaresInBounceLaser(
		IOperationOnSquare operationObj,
		Vector3 originalStart,
		List<Vector3> laserAnglePoints,
		float widthInSquares,
		ActorData caster,
		bool ignoreLos)
	{
		if (laserAnglePoints.Count <= 0)
		{
			return;
		}
		
		List<Vector3> bouncePoints = new List<Vector3>();
		bouncePoints.Add(originalStart);
		bouncePoints.AddRange(laserAnglePoints);
		
		List<ISquareInsideChecker> segments = new List<ISquareInsideChecker>();
		for (int i = 1; i < bouncePoints.Count; i++)
		{
			if (i >= 2)
			{
				SquareInsideChecker_BounceSegment segment = new SquareInsideChecker_BounceSegment(widthInSquares);
				Vector3 bounceDir = (bouncePoints[i] - bouncePoints[i - 1]).normalized;
				Vector3 prevBounceDirInv = (bouncePoints[i - 2] - bouncePoints[i - 1]).normalized;
				Vector3 collisionNormal = 0.5f * (bounceDir + prevBounceDirInv);
				segment.UpdateBoxProperties(bouncePoints[i - 1], bouncePoints[i], collisionNormal, caster);
				segments.Add(segment);
			}
			else
			{
				SquareInsideChecker_Box segment = new SquareInsideChecker_Box(widthInSquares);
				segment.UpdateBoxProperties(bouncePoints[i - 1], bouncePoints[i], caster);
				segments.Add(segment);
			}
		}
		for (int j = 1; j < bouncePoints.Count; j++)
		{
			OperateOnSquaresInBoxByActorRadius(
				operationObj,
				bouncePoints[j - 1],
				bouncePoints[j],
				widthInSquares,
				caster,
				ignoreLos,
				null,
				segments,
				false);
		}
	}

	public static List<BoardSquare> GetSquaresInTriangle(Vector3 pA, Vector3 pB, Vector3 pC, bool ignoreLoS, ActorData caster)
	{
		float x = Mathf.Min(pA.x, Mathf.Min(pB.x, pC.x));
		float y = Mathf.Min(pA.z, Mathf.Min(pB.z, pC.z));
		float x2 = Mathf.Max(pA.x, Mathf.Max(pB.x, pC.x));
		float y2 = Mathf.Max(pA.z, Mathf.Max(pB.z, pC.z));
		BoardSquare cornerSquareA = Board.Get().GetSquareFromPos(x, y);
		BoardSquare cornerSquareB = Board.Get().GetSquareFromPos(x2, y2);
		List<BoardSquare> squaresInRect = Board.Get().GetSquaresBoundedBy(cornerSquareA, cornerSquareB);
		List<BoardSquare> result = new List<BoardSquare>();
		BoardSquare losSourceSquare = Board.Get().GetSquareFromVec3(pA);
		foreach (BoardSquare square in squaresInRect)
		{
			if (VectorUtils.IsPointInTriangle(pA, pB, pC, square.ToVector3()))
			{
				bool flag = true;
				if (!ignoreLoS)
				{
					flag = SquaresHaveLoSForAbilities(losSourceSquare, square, caster);
				}
				if (flag)
				{
					result.Add(square);
				}
			}
		}
		return result;
	}

	public static List<ActorData> GetActorsInTriangle(
		Vector3 pA,
		Vector3 pB,
		Vector3 pC,
		bool ignoreLoS,
		ActorData caster,
		List<Team> onlyValidTeams)
	{
		List<ActorData> result = new List<ActorData>();
		List<BoardSquare> squaresInTriangle = GetSquaresInTriangle(pA, pB, pC, ignoreLoS, caster);
		foreach (BoardSquare square in squaresInTriangle)
		{
			ActorData occupantActor = square.OccupantActor;
			if (IsActorTargetable(occupantActor, onlyValidTeams) && !result.Contains(occupantActor))
			{
				result.Add(occupantActor);
			}
		}
		return result;
	}

	public static List<BoardSquare> GetSquaresInShape_EvenByEven(
		Vector3 cornerPos,
		int dimensions,
		int cornersToSubtract,
		bool ignoreLoS,
		ActorData caster)
	{
		List<BoardSquare> result = new List<BoardSquare>();
		List<BoardSquare> centerSquares = new List<BoardSquare>();
		float halfSquareSize = Board.Get().squareSize / 2f;
		if (!ignoreLoS)
		{
			centerSquares = GetCenterSquaresForEvenShapeLos(GetSquaresAroundEvenShapeCornerPos(cornerPos), caster);
		}
		float num2 = cornerPos.x + halfSquareSize - Board.Get().squareSize * (dimensions / 2);
		float num3 = cornerPos.z + halfSquareSize - Board.Get().squareSize * (dimensions / 2);
		for (int i = 0; i < dimensions; i++)
		{
			float x = num2 + Board.Get().squareSize * i;
			for (int j = 0; j < dimensions; j++)
			{
				if (cornersToSubtract > 0)
				{
					int xNum = Mathf.Min(dimensions - 1 - i, i);
					int yNum = Mathf.Min(dimensions - 1 - j, j);
					if (xNum + yNum < cornersToSubtract)
					{
						continue;
					}
				}
				float y = num3 + Board.Get().squareSize * j;
				BoardSquare boardSquareSafe = Board.Get().GetSquareFromPos(x, y);
				if (boardSquareSafe == null || !boardSquareSafe.IsValidForGameplay())
				{
					continue;
				}
				bool isInLos;
				if (ignoreLoS)
				{
					isInLos = true;
				}
				else if (centerSquares.Contains(boardSquareSafe))
				{
					isInLos = true;
				}
				else
				{
					int numCenterSquaresWithLos = 0;
					foreach (BoardSquare square in centerSquares)
					{
						if (SquaresHaveLoSForAbilities(square, boardSquareSafe, caster))
						{
							numCenterSquaresWithLos++;
						}
					}
					isInLos = numCenterSquaresWithLos >= 3;
				}
				if (isInLos)
				{
					result.Add(boardSquareSafe);
				}
			}
		}
		return result;
	}

	private static List<BoardSquare> GetSquaresAroundEvenShapeCornerPos(Vector3 cornerPos)
	{
		float num = Board.Get().squareSize / 2f;
		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = -1; i <= 1; i += 2)
		{
			for (int j = -1; j <= 1; j += 2)
			{
				BoardSquare boardSquare = Board.Get().GetSquareFromVec3(cornerPos + new Vector3(num * i, 0f, num * j));
				if (boardSquare != null)
				{
					list.Add(boardSquare);
				}
			}
		}
		return list;
	}

	private static List<BoardSquare> GetCenterSquaresForEvenShapeLos(List<BoardSquare> squaresByCorner, ActorData caster)
	{
		List<BoardSquare> result = new List<BoardSquare>();
		foreach (BoardSquare squareA in squaresByCorner)
		{
			int num = 0;
			foreach (BoardSquare squareB in squaresByCorner)
			{
				if (squareA != squareB && SquaresHaveLoSForAbilities(squareA, squareB, caster))
				{
					num++;
					if (num >= 2)
					{
						result.Add(squareA);
						break;
					}
				}
			}
		}
		return result;
	}

	public static List<BoardSquare> GetSquaresInShape_OddByOdd(BoardSquare centerSquare, int dimensions, int cornersToSubtract, bool ignoreLoS, ActorData caster)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		int halfDimensions = dimensions / 2;
		int xStart = centerSquare.x - halfDimensions;
		int xEnd = centerSquare.x + halfDimensions;
		int yStart = centerSquare.y - halfDimensions;
		int yEnd = centerSquare.y + halfDimensions;
		for (int i = xStart; i <= xEnd; i++)
		{
			for (int j = yStart; j <= yEnd; j++)
			{
				if (cornersToSubtract > 0)
				{
					int num6 = Mathf.Min(xEnd - i, i - xStart);
					int num7 = Mathf.Min(yEnd - j, j - yStart);
					int num8 = num6 + num7;
					if (num8 < cornersToSubtract)
					{
						continue;
					}
				}
				BoardSquare square = Board.Get().GetSquareFromIndex(i, j);
				if (square == null || !square.IsValidForGameplay())
				{
					continue;
				}
				if (ignoreLoS
				    || square == centerSquare
				    || SquaresHaveLoSForAbilities(centerSquare, square, caster))
				{
					list.Add(square);
				}
			}
		}
		return list;
	}

	public static List<BoardSquare> GetSquaresInShape(AbilityAreaShape shape, AbilityTarget target, bool ignoreLoS, ActorData caster)
	{
		return GetSquaresInShape(shape, target.FreePos, Board.Get().GetSquare(target.GridPos), ignoreLoS, caster);
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
			return GetSquaresInShape_OddByOdd(centerSquare, dimensions, cornersToSubtract, ignoreLoS, caster);
		}
		else
		{
			Vector3 cornerPos = Board.GetBestCornerPos(freePos, centerSquare);
			return GetSquaresInShape_EvenByEven(cornerPos, dimensions, cornersToSubtract, ignoreLoS, caster);
		}
	}

	public static List<ActorData> GetActorsInShape(
		AbilityAreaShape shape,
		AbilityTarget target,
		bool ignoreLoS,
		ActorData caster,
		Team onlyValidTeam,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return GetActorsInShape(
			shape,
			target.FreePos,
			Board.Get().GetSquare(target.GridPos),
			ignoreLoS,
			caster,
			new List<Team> { onlyValidTeam },
			nonActorTargetInfo);
	}

	public static List<ActorData> GetActorsInShape(
		AbilityAreaShape shape,
		AbilityTarget target,
		bool ignoreLoS,
		ActorData caster,
		List<Team> onlyValidTeams,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return GetActorsInShape(
			shape,
			target.FreePos,
			Board.Get().GetSquare(target.GridPos),
			ignoreLoS,
			caster,
			onlyValidTeams,
			nonActorTargetInfo);
	}

	public static List<ActorData> GetActorsInShape(
		AbilityAreaShape shape,
		Vector3 freePos,
		BoardSquare centerSquare,
		bool ignoreLoS,
		ActorData caster,
		Team onlyValidTeam,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return GetActorsInShape(
			shape,
			freePos,
			centerSquare,
			ignoreLoS,
			caster,
			new List<Team> { onlyValidTeam },
			nonActorTargetInfo);
	}

	public static List<ActorData> GetActorsInShape(
		AbilityAreaShape shape,
		Vector3 freePos,
		BoardSquare centerSquare,
		bool ignoreLoS,
		ActorData caster,
		List<Team> onlyValidTeams,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> result = new List<ActorData>();
		foreach (ActorData current in GameFlowData.Get().GetActors())
		{
			if (IsActorTargetable(current, onlyValidTeams)
			    && IsSquareInShape(current.GetCurrentBoardSquare(), shape, freePos, centerSquare, ignoreLoS, caster))
			{
				result.Add(current);
			}
		}
#if SERVER
		// added in rogues
		if (nonActorTargetInfo != null && !ignoreLoS && shape != AbilityAreaShape.SingleSquare)
		{
			PosInsideChecker_Shape checker = new PosInsideChecker_Shape(shape, freePos, centerSquare);
			Vector3 centerOfShape = GetCenterOfShape(shape, freePos, centerSquare);
			if (BarrierManager.Get() != null)
			{
				BarrierManager.Get().AddNonActorTargetInfoFromPos(caster, centerOfShape, nonActorTargetInfo, checker);
			}
			// rogues
			// if (DynamicMissionGeoManager.Get() != null)
			// {
			// 	DynamicMissionGeoManager.Get().AddDestructibleGeoHits(nonActorTargetInfo, posInsideChecker_Shape);
			// }
		}
#endif
		return result;
	}

	public static List<ActorData> GetActorsInShapeLayers(
		List<AbilityAreaShape> shapes,
		Vector3 freePos,
		BoardSquare centerSquare,
		bool ignoreLos,
		ActorData caster,
		List<Team> onlyValidTeams,
		out List<List<ActorData>> actorsInLayers,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> result = new List<ActorData>();
		actorsInLayers = new List<List<ActorData>>();
		for (int i = 0; i < shapes.Count; i++)
		{
			actorsInLayers.Add(new List<ActorData>());
		}
		foreach (ActorData current in GameFlowData.Get().GetActors())
		{
			if (!IsActorTargetable(current, onlyValidTeams))
			{
				continue;
			}
			for (int i = 0; i < shapes.Count; i++)
			{
				AbilityAreaShape shape = shapes[i];
				if (IsSquareInShape(current.GetCurrentBoardSquare(), shape, freePos, centerSquare, ignoreLos, caster))
				{
					actorsInLayers[i].Add(current);
					result.Add(current);
					break;
				}
			}
		}
#if SERVER
		// added in rogues
		if (shapes.Count > 0)
		{
			AbilityAreaShape shape = shapes[shapes.Count - 1];
			if (nonActorTargetInfo != null && !ignoreLos)
			{
				PosInsideChecker_Shape checker = new PosInsideChecker_Shape(shape, freePos, centerSquare);
				Vector3 centerOfShape = GetCenterOfShape(shape, freePos, centerSquare);
				if (BarrierManager.Get() != null)
				{
					BarrierManager.Get().AddNonActorTargetInfoFromPos(caster, centerOfShape, nonActorTargetInfo, checker);
				}
				// if (DynamicMissionGeoManager.Get() != null)
				// {
				// 	DynamicMissionGeoManager.Get().AddDestructibleGeoHits(nonActorTargetInfo, posInsideChecker_Shape);
				// }
			}
		}
#endif
		return result;
	}

	public static void OperateOnSquaresInShape(
		IOperationOnSquare operationObj,
		AbilityAreaShape shape,
		Vector3 freePos,
		BoardSquare centerSquare,
		bool ignoreLoS,
		ActorData caster,
		List<ISquareInsideChecker> losCheckOverrides = null)
	{
		GetSquareDimentionAndCornersToSubtract(shape, out int dimensions, out int cornersToSubtract);
		if (dimensions % 2 == 1)
		{
			OperateOnSquaresInShape_OddByOdd(operationObj, centerSquare, dimensions, cornersToSubtract, ignoreLoS, caster, losCheckOverrides);
		}
		else
		{
			Vector3 cornerPos = Board.GetBestCornerPos(freePos, centerSquare);
			OperateOnSquaresInShape_EvenByEven(operationObj, cornerPos, dimensions, cornersToSubtract, ignoreLoS, caster, losCheckOverrides);
		}
	}

	public static void OperateOnSquaresInShape_EvenByEven(
		IOperationOnSquare operationObj,
		Vector3 cornerPos,
		int dimensions,
		int cornersToSubtract,
		bool ignoreLoS,
		ActorData caster,
		List<ISquareInsideChecker> losCheckOverrides = null)
	{
		List<BoardSquare> centerSquares = new List<BoardSquare>();
		float halfSquareSize = Board.Get().squareSize / 2f;
		if (!ignoreLoS)
		{
			centerSquares = GetCenterSquaresForEvenShapeLos(GetSquaresAroundEvenShapeCornerPos(cornerPos), caster);
		}
		float startX = cornerPos.x + halfSquareSize - Board.Get().squareSize * (dimensions / 2);
		float startY = cornerPos.z + halfSquareSize - Board.Get().squareSize * (dimensions / 2);
		for (int i = 0; i < dimensions; i++)
		{
			float x = startX + Board.Get().squareSize * i;
			for (int j = 0; j < dimensions; j++)
			{
				if (cornersToSubtract > 0)
				{
					int xNum = Mathf.Min(dimensions - 1 - i, i);
					int yNum = Mathf.Min(dimensions - 1 - j, j);
					if (xNum + yNum < cornersToSubtract)
					{
						continue;
					}
				}
				float y = startY + Board.Get().squareSize * j;
				BoardSquare square = Board.Get().GetSquareFromPos(x, y);
				if (square == null || !square.IsValidForGameplay())
				{
					continue;
				}
				bool squareHasLos;
				if (ignoreLoS || centerSquares.Contains(square))
				{
					squareHasLos = true;
				}
				else if (losCheckOverrides != null)
				{
					squareHasLos = SquareHasLosByAreaCheckers(square, losCheckOverrides);
				}
				else
				{
					int numCenterSquaresWithLos = 0;
					foreach (BoardSquare centerSquare in centerSquares)
					{
						if (SquaresHaveLoSForAbilities(centerSquare, square, caster))
						{
							numCenterSquaresWithLos++;
						}
					}
					squareHasLos = numCenterSquaresWithLos >= 3;
				}
				operationObj.OperateOnSquare(square, caster, squareHasLos);
			}
		}
	}

	public static void OperateOnSquaresInShape_OddByOdd(
		IOperationOnSquare operationObj,
		BoardSquare centerSquare,
		int dimensions,
		int cornersToSubtract,
		bool ignoreLoS,
		ActorData caster,
		List<ISquareInsideChecker> losCheckOverrides = null)
	{
		int halfDimensions = dimensions / 2;
		int xStart = centerSquare.x - halfDimensions;
		int xEnd = centerSquare.x + halfDimensions;
		int yStart = centerSquare.y - halfDimensions;
		int yEnd = centerSquare.y + halfDimensions;
		for (int i = xStart; i <= xEnd; i++)
		{
			for (int j = yStart; j <= yEnd; j++)
			{
				if (cornersToSubtract > 0)
				{
					int xNum = Mathf.Min(xEnd - i, i - xStart);
					int yNum = Mathf.Min(yEnd - j, j - yStart);
					if (xNum + yNum < cornersToSubtract)
					{
						continue;
					}
				}
				BoardSquare square = Board.Get().GetSquareFromIndex(i, j);
				if (square == null || !square.IsValidForGameplay())
				{
					continue;
				}
				bool squareHasLos;
				if (ignoreLoS)
				{
					squareHasLos = true;
				}
				else if (losCheckOverrides != null)
				{
					squareHasLos = SquareHasLosByAreaCheckers(square, losCheckOverrides);
				}
				else
				{
					squareHasLos = square == centerSquare
					               || SquaresHaveLoSForAbilities(centerSquare, square, caster);
				}
				operationObj.OperateOnSquare(square, caster, squareHasLos);
			}
		}
	}

	public static bool IsSquareInShape(
		BoardSquare testSquare,
		AbilityAreaShape shape,
		Vector3 freePos,
		BoardSquare centerSquare,
		bool ignoreLoS,
		ActorData caster)
	{
		GetSquareDimentionAndCornersToSubtract(shape, out int dimensions, out int cornersToSubtract);
		if (dimensions % 2 == 1)
		{
			return IsSquareInShape_OddByOdd(testSquare, centerSquare, dimensions, cornersToSubtract, ignoreLoS, caster);
		}
		else
		{
			Vector3 cornerPos = Board.GetBestCornerPos(freePos, centerSquare);
			return IsSquareInShape_EvenByEven(testSquare, cornerPos, dimensions, cornersToSubtract, ignoreLoS, caster);
		}
	}

	private static List<BoardSquare> GetSquaresAroundCornerPos(Vector3 cornerPos)
	{
		float num = Board.Get().squareSize / 2f;
		List<BoardSquare> result = new List<BoardSquare>();
		for (int i = -1; i <= 1; i += 2)
		{
			for (int j = -1; j <= 1; j += 2)
			{
				BoardSquare square = Board.Get().GetSquareFromVec3(cornerPos + new Vector3(num * i, 0f, num * j));
				if (square != null)
				{
					result.Add(square);
				}
			}
		}
		return result;
	}

	private static List<BoardSquare> GetCenterSquaresForShapeLos(List<BoardSquare> squaresByCorner, ActorData caster)
	{
		List<BoardSquare> result = new List<BoardSquare>();
		foreach (BoardSquare squareA in squaresByCorner)
		{
			int num = 0;
			foreach (BoardSquare squareB in squaresByCorner)
			{
				if (squareA != squareB && SquaresHaveLoSForAbilities(squareA, squareB, caster))
				{
					num++;
					if (num >= 2)
					{
						result.Add(squareA);
						break;
					}
				}
			}
		}
		return result;
	}

	private static bool IsSquareInShape_EvenByEven(
		BoardSquare testSquare,
		Vector3 cornerPos,
		List<BoardSquare> squaresInCenter,
		int dimensions,
		int cornersToSubtract,
		bool ignoreLoS,
		ActorData caster)
	{
		if (testSquare == null)
		{
			return false;
		}
		
		float squareSize = Board.Get().squareSize;
		float halfSquareSize = squareSize / 2f;
		float centerX = cornerPos.x + halfSquareSize;
		float centerY = cornerPos.z + halfSquareSize;
		int halfDimensions = dimensions / 2;
		int di = Mathf.RoundToInt((centerX - testSquare.worldX) / squareSize);
		int numX = di > 0
			? halfDimensions - di
			: halfDimensions - Mathf.Abs(di) - 1;
		int dj = Mathf.RoundToInt((centerY - testSquare.worldY) / squareSize);
		int numY = dj > 0
			? halfDimensions - dj
			: halfDimensions - Mathf.Abs(dj) - 1;
		
		if (numX < 0 || numY < 0 || numX + numY < cornersToSubtract)
		{
			return false;
		}
		if (ignoreLoS)
		{
			return true;
		}
		if (squaresInCenter.Contains(testSquare))
		{
			return true;
		}
		
		int centerSquaresWithLos = 0;
		foreach (BoardSquare item in squaresInCenter)
		{
			if (SquaresHaveLoSForAbilities(item, testSquare, caster))
			{
				centerSquaresWithLos++;
			}
		}
		return centerSquaresWithLos >= 3;
	}

	private static bool IsSquareInShape_EvenByEven(
		BoardSquare testSquare,
		Vector3 cornerPos,
		int dimensions,
		int cornersToSubtract,
		bool ignoreLoS,
		ActorData caster)
	{
		if (testSquare == null)
		{
			return false;
		}
		List<BoardSquare> squaresInCenter = null;
		if (!ignoreLoS)
		{
			squaresInCenter = GetCenterSquaresForShapeLos(GetSquaresAroundCornerPos(cornerPos), caster);
		}
		return IsSquareInShape_EvenByEven(testSquare, cornerPos, squaresInCenter, dimensions, cornersToSubtract, ignoreLoS, caster);
	}

	private static bool IsSquareInShape_OddByOdd(
		BoardSquare testSquare,
		BoardSquare centerSquare,
		int dimensions,
		int cornersToSubtract,
		bool ignoreLoS,
		ActorData caster)
	{
		if (testSquare == null || centerSquare == null)
		{
			return false;
		}
		int halfDimensions = dimensions / 2;
		int di = Mathf.Abs(testSquare.x - centerSquare.x);
		int dj = Mathf.Abs(testSquare.y - centerSquare.y);
		
		if (di > halfDimensions || dj > halfDimensions)
		{
			return false;
		}
		
		int numX = halfDimensions - di;
		int numY = halfDimensions - dj;
		if (numX + numY < cornersToSubtract)
		{
			return false;
		}
		
		return ignoreLoS
		       || testSquare == centerSquare
		       || SquaresHaveLoSForAbilities(centerSquare, testSquare, caster);
	}

	public static bool IsPosInShape(Vector3 testPos, AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		GetSquareDimentionAndCornersToSubtract(shape, out int _, out int _);
		BoardSquare square = Board.Get().GetSquareFromVec3(testPos);
		return square != null && IsSquareInShape(square, shape, freePos, centerSquare, true, null);
	}

	private static bool IsPosInShape_OddByOdd(Vector3 testPos, BoardSquare centerSquare, int dimensions, int cornersToSubtract)
	{
		if (centerSquare == null)
		{
			return false;
		}
		float squareSize = Board.Get().squareSize;
		float halfSquareSize = 0.5f * squareSize;
		int halfDimensions = dimensions / 2;
		Vector3 centerPos = centerSquare.ToVector3();
		float dx = Mathf.Abs(testPos.x - centerPos.x);
		float dy = Mathf.Abs(testPos.z - centerPos.z);
		dx = Mathf.Max(0f, dx - halfSquareSize + 0.05f);
		dy = Mathf.Max(0f, dy - halfSquareSize + 0.05f);
		float di = dx / squareSize;
		float dj = dy / squareSize;
		if (di <= halfDimensions && dj <= halfDimensions)
		{
			float numX = halfDimensions - di;
			float numY = halfDimensions - dj;
			if (numX + numY >= cornersToSubtract)
			{
				return true;
			}
		}
		return false;
	}

	private static bool IsPosInShape_EvenByEven(Vector3 testPos, Vector3 cornerPos, int dimensions, int cornersToSubtract)
	{
		float squareSize = Board.Get().squareSize;
		float halfSquareSize = squareSize / 2f;
		float centerPosX = cornerPos.x + halfSquareSize;
		float centerPosY = cornerPos.z + halfSquareSize;
		int halfDimensions = dimensions / 2;
		int di = Mathf.RoundToInt((centerPosX - testPos.x) / squareSize);
		int numX = di > 0
			? halfDimensions - di
			: halfDimensions - Mathf.Abs(di) - 1;
		int dj = Mathf.RoundToInt((centerPosY - testPos.z) / squareSize);
		int numY = dj > 0
			? halfDimensions - dj
			: halfDimensions - Mathf.Abs(dj) - 1;
		
		return numX >= 0 && numY >= 0 && numX + numY >= cornersToSubtract;
	}

	public static Vector3 GetCenterOfShape(AbilityAreaShape shape, AbilityTarget target)
	{
		return GetCenterOfShape(shape, target.FreePos, Board.Get().GetSquare(target.GridPos));
	}

	public static bool IsShapeOddByOdd(AbilityAreaShape shape)
	{
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
	
#if SERVER
	// added in rogues
	public static int GetNumberOfSidesForShape(AbilityAreaShape shape)
	{
		switch (shape)
		{
			case AbilityAreaShape.SingleSquare:
			case AbilityAreaShape.Two_x_Two:
			case AbilityAreaShape.Three_x_Three:
			case AbilityAreaShape.Four_x_Four:
			case AbilityAreaShape.Five_x_Five:
			case AbilityAreaShape.Six_x_Six:
			case AbilityAreaShape.Seven_x_Seven:
				return 4;
			case AbilityAreaShape.Three_x_Three_NoCorners:
			case AbilityAreaShape.Four_x_Four_NoCorners:
			case AbilityAreaShape.Five_x_Five_NoCorners:
			case AbilityAreaShape.Six_x_Six_NoCorners:
			case AbilityAreaShape.Seven_x_Seven_NoCorners:
				return 8;
			case AbilityAreaShape.Five_x_Five_ExtraNoCorners:
			case AbilityAreaShape.Six_x_Six_ExtraNoCorners:
			case AbilityAreaShape.Seven_x_Seven_ExtraNoCorners:
				return 8;
		}
		return 0;
	}
#endif
	
	// added in rogues, inlined in reactor
	public static float GetWidthForShape(AbilityAreaShape shape)
	{
		switch (shape)
		{
			case AbilityAreaShape.SingleSquare:
				return 1f;
			case AbilityAreaShape.Two_x_Two:
				return 2f;
			case AbilityAreaShape.Three_x_Three:
			case AbilityAreaShape.Three_x_Three_NoCorners:
				return 3f;
			case AbilityAreaShape.Four_x_Four:
			case AbilityAreaShape.Four_x_Four_NoCorners:
				return 4f;
			case AbilityAreaShape.Five_x_Five:
			case AbilityAreaShape.Five_x_Five_NoCorners:
			case AbilityAreaShape.Five_x_Five_ExtraNoCorners:
				return 5f;
			case AbilityAreaShape.Six_x_Six:
			case AbilityAreaShape.Six_x_Six_NoCorners:
			case AbilityAreaShape.Six_x_Six_ExtraNoCorners:
				return 6f;
			case AbilityAreaShape.Seven_x_Seven:
			case AbilityAreaShape.Seven_x_Seven_NoCorners:
			case AbilityAreaShape.Seven_x_Seven_ExtraNoCorners:
				return 7f;
			default:
				return 0f;
		}
	}
	
	public static Vector3 GetCenterOfShape(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		return IsShapeOddByOdd(shape)
			? centerSquare.ToVector3()
			: Board.GetBestCornerPos(freePos, centerSquare);
	}

	public static Vector3 GetCenterOfGridPattern(AbilityGridPattern pattern, AbilityTarget target)
	{
		return GetCenterOfGridPattern(pattern, target.FreePos, Board.Get().GetSquare(target.GridPos));
	}

	public static Vector3 GetCenterOfGridPattern(AbilityGridPattern pattern, Vector3 freePos, BoardSquare centerSquare)
	{
		return pattern != AbilityGridPattern.Plus_Two_x_Two && pattern != AbilityGridPattern.Plus_Four_x_Four
			? centerSquare.ToVector3()
			: Board.GetBestCornerPos(freePos, centerSquare);
	}

	public static bool HasAdjacentActorOfTeam(ActorData aroundActor, List<Team> teams)
	{
		if (!IsActorTargetable(aroundActor))
		{
			return false;
		}
		foreach (ActorData actor in GameFlowData.Get().GetActors())
		{
			if (actor != aroundActor
			    && IsActorTargetable(actor, teams)
			    && Board.Get().GetSquaresAreAdjacent(aroundActor.GetCurrentBoardSquare(), actor.GetCurrentBoardSquare()))
			{
				return true;
			}
		}
		return false;
	}

	public static void AddShapeCornersToList(ref List<Vector3> points, AbilityAreaShape shape, AbilityTarget target)
	{
		AddShapeCornersToList(ref points, shape, target.FreePos, Board.Get().GetSquare(target.GridPos));
	}

	public static void AddShapeCornersToList(ref List<Vector3> points, AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		Vector3 centerOfShape = GetCenterOfShape(shape, freePos, centerSquare);
		float dimensions = GetWidthForShape(shape);  // inlined in reactor
		float halfDimensions = dimensions / 2f;
		float halfSize = halfDimensions * Board.Get().squareSize;
		points.Add(new Vector3(centerOfShape.x + halfSize, centerOfShape.y, centerOfShape.z + halfSize));
		points.Add(new Vector3(centerOfShape.x + halfSize, centerOfShape.y, centerOfShape.z - halfSize));
		points.Add(new Vector3(centerOfShape.x - halfSize, centerOfShape.y, centerOfShape.z + halfSize));
		points.Add(new Vector3(centerOfShape.x - halfSize, centerOfShape.y, centerOfShape.z - halfSize));
	}

	public static List<Vector3> BuildShapeCornersList(AbilityAreaShape shape, AbilityTarget target)
	{
		List<Vector3> points = new List<Vector3>();
		AddShapeCornersToList(ref points, shape, target.FreePos, Board.Get().GetSquare(target.GridPos));
		return points;
	}

	public static List<Vector3> BuildShapeCornersList(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		List<Vector3> points = new List<Vector3>();
		AddShapeCornersToList(ref points, shape, freePos, centerSquare);
		return points;
	}

	public static void AddConeExtremaToList(
		ref List<Vector3> points,
		Vector3 coneStart,
		float coneCenterAngleDegrees,
		float coneWidthDegrees,
		float coneLengthRadiusInSquares,
		float coneBackwardOffsetInSquares)
	{
		Vector3 coneDir = VectorUtils.AngleDegreesToVector(coneCenterAngleDegrees);
		float backwardOffset = coneBackwardOffsetInSquares * Board.Get().squareSize;
		Vector3 offsetConeStart = coneStart - coneDir * backwardOffset;
		float radiusInSquares = coneLengthRadiusInSquares + coneBackwardOffsetInSquares;
		float radiusInWorld = radiusInSquares * Board.Get().squareSize;
		float maxAngleWithCenter = coneWidthDegrees / 2f;
		float coneSideAngleA = coneCenterAngleDegrees - maxAngleWithCenter;
		float coneSideAngleB = coneCenterAngleDegrees + maxAngleWithCenter;
		Vector3 coneSideDirA = VectorUtils.AngleDegreesToVector(coneSideAngleA);
		Vector3 coneSideDirB = VectorUtils.AngleDegreesToVector(coneSideAngleB);
		points.Add(offsetConeStart);
		points.Add(offsetConeStart + coneDir * radiusInWorld);
		points.Add(offsetConeStart + coneSideDirA * radiusInWorld);
		points.Add(offsetConeStart + coneSideDirB * radiusInWorld);
	}

	public static void AddBoxExtremaToList(ref List<Vector3> points, Vector3 startPos, Vector3 endPos, float boxWidthInSquares)
	{
		float boxWidth = boxWidthInSquares * Board.Get().squareSize;
		Vector3 dir = (endPos - startPos).normalized;
		Vector3 right = Vector3.Cross(dir, Vector3.up);
		Vector3 offset = right * 0.5f * boxWidth;
		points.Add(startPos + offset);
		points.Add(startPos - offset);
		points.Add(endPos + offset);
		points.Add(endPos - offset);
	}

	public static void AddRadiusExtremaToList(ref List<Vector3> points, Vector3 startPos, float radiusInSquares)
	{
		float radius = radiusInSquares * Board.Get().squareSize;
		Vector3 xOffset = new Vector3(radius, 0f, 0f);
		Vector3 yOffset = new Vector3(0f, 0f, radius);
		points.Add(startPos + xOffset + yOffset);
		points.Add(startPos + xOffset - yOffset);
		points.Add(startPos - xOffset + yOffset);
		points.Add(startPos - xOffset - yOffset);
	}
}
