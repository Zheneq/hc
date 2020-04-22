using System.Collections.Generic;
using UnityEngine;

public class GrydCardinalSegmentInfo
{
	public BoardSquare m_startSquare;

	public BoardSquare m_endSquare;

	public Vector3 m_direction;

	public List<GrydCardinalSegmentInfo> m_childSegments;

	public int m_segmentIndex = -1;

	public int m_parentSegIndex = -1;

	public List<NonActorTargetInfo> m_nonActorTargetInfo;

	public Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> m_hitActorsMap;

	public GrydCardinalSegmentInfo(BoardSquare startSquare, Vector3 direction)
	{
		m_startSquare = startSquare;
		m_direction = direction;
		m_childSegments = new List<GrydCardinalSegmentInfo>();
	}

	public bool IsValidSegment()
	{
		int result;
		if (m_endSquare != null)
		{
			result = ((m_endSquare != m_startSquare) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public void TrackActorHitInfo(Dictionary<ActorData, ActorMultiHitContext> actorToHitContext)
	{
		if (m_hitActorsMap == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		using (Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>.Enumerator enumerator = m_hitActorsMap.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> current = enumerator.Current;
				ActorData key = current.Key;
				AreaEffectUtils.BouncingLaserInfo value = current.Value;
				Vector3 segmentOrigin = value.m_segmentOrigin;
				ActorCover actorCover = key.GetActorCover();
				bool flag = actorCover.IsInCoverWrt(segmentOrigin);
				if (actorToHitContext.ContainsKey(key))
				{
					actorToHitContext[key].m_numHits++;
					if (flag)
					{
						actorToHitContext[key].m_numHitsFromCover++;
					}
					if (actorCover != null && !actorCover.IsInCoverWrt(actorToHitContext[key].m_hitOrigin) && actorCover.IsInCoverWrt(segmentOrigin))
					{
						actorToHitContext[key].m_hitOrigin = segmentOrigin;
					}
				}
				else
				{
					ActorMultiHitContext actorMultiHitContext = new ActorMultiHitContext();
					actorMultiHitContext.m_numHits = 1;
					int numHitsFromCover;
					if (flag)
					{
						numHitsFromCover = 1;
					}
					else
					{
						numHitsFromCover = 0;
					}
					actorMultiHitContext.m_numHitsFromCover = numHitsFromCover;
					actorMultiHitContext.m_hitOrigin = segmentOrigin;
					actorToHitContext[key] = actorMultiHitContext;
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
	}

	public static void CalculateSegmentInfo(GrydCardinalSegmentInfo parentSegment, float maxDistInSquares, float maxBranchDistInSquares, int splitsRemaining, bool splitOnWall, bool splitOnActor, bool continueAfterActorHit, bool includeInvisibles, int segmentIndex, ActorData caster, List<Team> relevantTeams, List<NonActorTargetInfo> nonActorTargetInfo, Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> hitActorsMap, List<ActorData> actorsToExclude)
	{
		Vector3 vector = parentSegment.m_startSquare.ToVector3();
		parentSegment.m_segmentIndex = segmentIndex;
		vector.y = Board.Get().LosCheckHeight;
		float num = maxDistInSquares * Board.SquareSizeStatic;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, parentSegment.m_direction, num, false, caster, nonActorTargetInfo);
		Vector3 vector2 = laserEndPoint - vector;
		float magnitude = vector2.magnitude;
		vector2.Normalize();
		bool flag = magnitude < num - 0.1f;
		BoardSquare boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(vector, laserEndPoint, true);
		if (boardSquare == null || boardSquare == parentSegment.m_startSquare)
		{
			boardSquare = parentSegment.m_startSquare;
			return;
		}
		int num2;
		if (splitOnWall)
		{
			num2 = (flag ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool flag2 = (byte)num2 != 0;
		int num3;
		if (continueAfterActorHit)
		{
			num3 = -1;
		}
		else
		{
			num3 = 1;
		}
		int maxTargets = num3;
		Vector3 startPos = vector;
		if (segmentIndex == 0)
		{
			startPos += 0.49f * Board.SquareSizeStatic * vector2;
		}
		Vector3 laserEndPos;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(startPos, vector2, num / Board.SquareSizeStatic, 0.5f, caster, relevantTeams, false, maxTargets, false, includeInvisibles, out laserEndPos, nonActorTargetInfo, actorsToExclude, true);
		BoardSquare startSquare = boardSquare;
		if (actorsInLaser.Count > 0)
		{
			if (splitOnActor)
			{
				startSquare = actorsInLaser[0].GetCurrentBoardSquare();
				flag2 = true;
			}
		}
		if (actorsInLaser.Count > 0 && !continueAfterActorHit)
		{
			boardSquare = actorsInLaser[0].GetCurrentBoardSquare();
		}
		for (int i = 0; i < actorsInLaser.Count; i++)
		{
			ActorData actorData = actorsInLaser[i];
			if (!hitActorsMap.ContainsKey(actorData))
			{
				AreaEffectUtils.BouncingLaserInfo value = new AreaEffectUtils.BouncingLaserInfo(vector, segmentIndex);
				hitActorsMap.Add(actorData, value);
				actorsToExclude.Add(actorData);
			}
		}
		parentSegment.m_endSquare = boardSquare;
		if (splitsRemaining > 0)
		{
			if (flag2)
			{
				Vector3 direction = Quaternion.AngleAxis(-90f, Vector3.up) * parentSegment.m_direction;
				GrydCardinalSegmentInfo grydCardinalSegmentInfo = new GrydCardinalSegmentInfo(startSquare, direction);
				CalculateSegmentInfo(grydCardinalSegmentInfo, maxBranchDistInSquares, maxBranchDistInSquares, splitsRemaining - 1, splitOnWall, splitOnActor, continueAfterActorHit, includeInvisibles, segmentIndex + 1, caster, relevantTeams, nonActorTargetInfo, hitActorsMap, actorsToExclude);
				Vector3 direction2 = Quaternion.AngleAxis(90f, Vector3.up) * parentSegment.m_direction;
				GrydCardinalSegmentInfo grydCardinalSegmentInfo2 = new GrydCardinalSegmentInfo(startSquare, direction2);
				CalculateSegmentInfo(grydCardinalSegmentInfo2, maxBranchDistInSquares, maxBranchDistInSquares, splitsRemaining - 1, splitOnWall, splitOnActor, continueAfterActorHit, includeInvisibles, segmentIndex + 2, caster, relevantTeams, nonActorTargetInfo, hitActorsMap, actorsToExclude);
				grydCardinalSegmentInfo.m_parentSegIndex = segmentIndex;
				grydCardinalSegmentInfo2.m_parentSegIndex = segmentIndex;
				parentSegment.m_childSegments.Add(grydCardinalSegmentInfo);
				parentSegment.m_childSegments.Add(grydCardinalSegmentInfo2);
			}
		}
		Debug.DrawLine(parentSegment.m_startSquare.ToVector3(), parentSegment.m_endSquare.ToVector3(), Color.red, 3f);
	}

	public static void AssembleSequenceParamData(GrydCardinalSegmentInfo parentSegment, List<GrydCardinalBombSequence.SegmentDataEntry> segmentDataList)
	{
		if (!parentSegment.IsValidSegment())
		{
			return;
		}
		while (true)
		{
			GrydCardinalBombSequence.SegmentDataEntry segmentDataEntry = new GrydCardinalBombSequence.SegmentDataEntry();
			segmentDataEntry.m_segmentIndex = (sbyte)parentSegment.m_segmentIndex;
			segmentDataEntry.m_prevSegmentIndex = (sbyte)parentSegment.m_parentSegIndex;
			segmentDataEntry.m_startSquare = parentSegment.m_startSquare;
			segmentDataEntry.m_endSquare = parentSegment.m_endSquare;
			segmentDataList.Add(segmentDataEntry);
			foreach (GrydCardinalSegmentInfo childSegment in parentSegment.m_childSegments)
			{
				AssembleSequenceParamData(childSegment, segmentDataList);
			}
			return;
		}
	}

	public static void EncapsulateBoundForCamPos(GrydCardinalSegmentInfo parentSegment, ref Bounds bound)
	{
		if (!parentSegment.IsValidSegment())
		{
			return;
		}
		while (true)
		{
			bound.Encapsulate(parentSegment.m_startSquare.ToVector3());
			bound.Encapsulate(parentSegment.m_endSquare.ToVector3());
			using (List<GrydCardinalSegmentInfo>.Enumerator enumerator = parentSegment.m_childSegments.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GrydCardinalSegmentInfo current = enumerator.Current;
					EncapsulateBoundForCamPos(current, ref bound);
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

	public static void HandleTargeterHighlights(GrydCardinalSegmentInfo parentSegment, bool thinnerLine, List<GameObject> highlights, ref int nextHighlightIndex)
	{
		if (!parentSegment.IsValidSegment())
		{
			return;
		}
		while (true)
		{
			Vector3 vector = parentSegment.m_startSquare.ToVector3();
			Vector3 vector2 = parentSegment.m_endSquare.ToVector3() - vector;
			vector2.y = 0f;
			float magnitude = vector2.magnitude;
			GameObject gameObject = null;
			if (highlights.Count <= nextHighlightIndex)
			{
				gameObject = HighlightUtils.Get().CreateRectangularCursor(1f, magnitude);
				highlights.Add(gameObject);
			}
			else
			{
				gameObject = highlights[nextHighlightIndex];
				gameObject.SetActive(true);
			}
			nextHighlightIndex++;
			float num = Board.SquareSizeStatic;
			if (thinnerLine)
			{
				num *= 0.5f;
			}
			HighlightUtils.Get().ResizeRectangularCursor(num, magnitude, gameObject);
			vector.y = HighlightUtils.GetHighlightHeight();
			gameObject.transform.position = vector;
			gameObject.transform.rotation = Quaternion.LookRotation(parentSegment.m_direction);
			for (int i = 0; i < parentSegment.m_childSegments.Count; i++)
			{
				HandleTargeterHighlights(parentSegment.m_childSegments[i], true, highlights, ref nextHighlightIndex);
			}
			return;
		}
	}
}
