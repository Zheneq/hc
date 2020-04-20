using System;
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
		this.m_startSquare = startSquare;
		this.m_direction = direction;
		this.m_childSegments = new List<GrydCardinalSegmentInfo>();
	}

	public bool IsValidSegment()
	{
		bool result;
		if (this.m_endSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalSegmentInfo.IsValidSegment()).MethodHandle;
			}
			result = (this.m_endSquare != this.m_startSquare);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void TrackActorHitInfo(Dictionary<ActorData, ActorMultiHitContext> actorToHitContext)
	{
		if (this.m_hitActorsMap == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalSegmentInfo.TrackActorHitInfo(Dictionary<ActorData, ActorMultiHitContext>)).MethodHandle;
			}
			return;
		}
		using (Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>.Enumerator enumerator = this.m_hitActorsMap.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> keyValuePair = enumerator.Current;
				ActorData key = keyValuePair.Key;
				Vector3 segmentOrigin = keyValuePair.Value.m_segmentOrigin;
				ActorCover actorCover = key.GetActorCover();
				bool flag = actorCover.IsInCoverWrt(segmentOrigin);
				if (actorToHitContext.ContainsKey(key))
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
					actorToHitContext[key].m_numHits++;
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
						actorToHitContext[key].m_numHitsFromCover++;
					}
					if (actorCover != null && !actorCover.IsInCoverWrt(actorToHitContext[key].m_hitOrigin) && actorCover.IsInCoverWrt(segmentOrigin))
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
						actorToHitContext[key].m_hitOrigin = segmentOrigin;
					}
				}
				else
				{
					ActorMultiHitContext actorMultiHitContext = new ActorMultiHitContext();
					actorMultiHitContext.m_numHits = 1;
					ActorMultiHitContext actorMultiHitContext2 = actorMultiHitContext;
					int numHitsFromCover;
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
						numHitsFromCover = 1;
					}
					else
					{
						numHitsFromCover = 0;
					}
					actorMultiHitContext2.m_numHitsFromCover = numHitsFromCover;
					actorMultiHitContext.m_hitOrigin = segmentOrigin;
					actorToHitContext[key] = actorMultiHitContext;
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

	public static void CalculateSegmentInfo(GrydCardinalSegmentInfo parentSegment, float maxDistInSquares, float maxBranchDistInSquares, int splitsRemaining, bool splitOnWall, bool splitOnActor, bool continueAfterActorHit, bool includeInvisibles, int segmentIndex, ActorData caster, List<Team> relevantTeams, List<NonActorTargetInfo> nonActorTargetInfo, Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> hitActorsMap, List<ActorData> actorsToExclude)
	{
		Vector3 vector = parentSegment.m_startSquare.ToVector3();
		parentSegment.m_segmentIndex = segmentIndex;
		vector.y = Board.Get().LosCheckHeight;
		float num = maxDistInSquares * Board.SquareSizeStatic;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, parentSegment.m_direction, num, false, caster, nonActorTargetInfo, true);
		Vector3 vector2 = laserEndPoint - vector;
		float magnitude = vector2.magnitude;
		vector2.Normalize();
		bool flag = magnitude < num - 0.1f;
		BoardSquare boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(vector, laserEndPoint, true, false, float.MaxValue);
		if (boardSquare == null || boardSquare == parentSegment.m_startSquare)
		{
			boardSquare = parentSegment.m_startSquare;
		}
		else
		{
			bool flag2;
			if (splitOnWall)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalSegmentInfo.CalculateSegmentInfo(GrydCardinalSegmentInfo, float, float, int, bool, bool, bool, bool, int, ActorData, List<Team>, List<NonActorTargetInfo>, Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>, List<ActorData>)).MethodHandle;
				}
				flag2 = flag;
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			int num2;
			if (continueAfterActorHit)
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
				num2 = -1;
			}
			else
			{
				num2 = 1;
			}
			int maxTargets = num2;
			Vector3 vector3 = vector;
			if (segmentIndex == 0)
			{
				vector3 += 0.49f * Board.SquareSizeStatic * vector2;
			}
			Vector3 vector4;
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(vector3, vector2, num / Board.SquareSizeStatic, 0.5f, caster, relevantTeams, false, maxTargets, false, includeInvisibles, out vector4, nonActorTargetInfo, actorsToExclude, true, true);
			BoardSquare startSquare = boardSquare;
			if (actorsInLaser.Count > 0)
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
				if (splitOnActor)
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
					startSquare = actorsInLaser[0].GetCurrentBoardSquare();
					flag3 = true;
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
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					AreaEffectUtils.BouncingLaserInfo value = new AreaEffectUtils.BouncingLaserInfo(vector, segmentIndex);
					hitActorsMap.Add(actorData, value);
					actorsToExclude.Add(actorData);
				}
			}
			parentSegment.m_endSquare = boardSquare;
			if (splitsRemaining > 0)
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
					Vector3 direction = Quaternion.AngleAxis(-90f, Vector3.up) * parentSegment.m_direction;
					GrydCardinalSegmentInfo grydCardinalSegmentInfo = new GrydCardinalSegmentInfo(startSquare, direction);
					GrydCardinalSegmentInfo.CalculateSegmentInfo(grydCardinalSegmentInfo, maxBranchDistInSquares, maxBranchDistInSquares, splitsRemaining - 1, splitOnWall, splitOnActor, continueAfterActorHit, includeInvisibles, segmentIndex + 1, caster, relevantTeams, nonActorTargetInfo, hitActorsMap, actorsToExclude);
					Vector3 direction2 = Quaternion.AngleAxis(90f, Vector3.up) * parentSegment.m_direction;
					GrydCardinalSegmentInfo grydCardinalSegmentInfo2 = new GrydCardinalSegmentInfo(startSquare, direction2);
					GrydCardinalSegmentInfo.CalculateSegmentInfo(grydCardinalSegmentInfo2, maxBranchDistInSquares, maxBranchDistInSquares, splitsRemaining - 1, splitOnWall, splitOnActor, continueAfterActorHit, includeInvisibles, segmentIndex + 2, caster, relevantTeams, nonActorTargetInfo, hitActorsMap, actorsToExclude);
					grydCardinalSegmentInfo.m_parentSegIndex = segmentIndex;
					grydCardinalSegmentInfo2.m_parentSegIndex = segmentIndex;
					parentSegment.m_childSegments.Add(grydCardinalSegmentInfo);
					parentSegment.m_childSegments.Add(grydCardinalSegmentInfo2);
				}
			}
			Debug.DrawLine(parentSegment.m_startSquare.ToVector3(), parentSegment.m_endSquare.ToVector3(), Color.red, 3f);
		}
	}

	public static void AssembleSequenceParamData(GrydCardinalSegmentInfo parentSegment, List<GrydCardinalBombSequence.SegmentDataEntry> segmentDataList)
	{
		if (parentSegment.IsValidSegment())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalSegmentInfo.AssembleSequenceParamData(GrydCardinalSegmentInfo, List<GrydCardinalBombSequence.SegmentDataEntry>)).MethodHandle;
			}
			segmentDataList.Add(new GrydCardinalBombSequence.SegmentDataEntry
			{
				m_segmentIndex = (sbyte)parentSegment.m_segmentIndex,
				m_prevSegmentIndex = (sbyte)parentSegment.m_parentSegIndex,
				m_startSquare = parentSegment.m_startSquare,
				m_endSquare = parentSegment.m_endSquare
			});
			foreach (GrydCardinalSegmentInfo parentSegment2 in parentSegment.m_childSegments)
			{
				GrydCardinalSegmentInfo.AssembleSequenceParamData(parentSegment2, segmentDataList);
			}
		}
	}

	public unsafe static void EncapsulateBoundForCamPos(GrydCardinalSegmentInfo parentSegment, ref Bounds bound)
	{
		if (parentSegment.IsValidSegment())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalSegmentInfo.EncapsulateBoundForCamPos(GrydCardinalSegmentInfo, Bounds*)).MethodHandle;
			}
			bound.Encapsulate(parentSegment.m_startSquare.ToVector3());
			bound.Encapsulate(parentSegment.m_endSquare.ToVector3());
			using (List<GrydCardinalSegmentInfo>.Enumerator enumerator = parentSegment.m_childSegments.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GrydCardinalSegmentInfo parentSegment2 = enumerator.Current;
					GrydCardinalSegmentInfo.EncapsulateBoundForCamPos(parentSegment2, ref bound);
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

	public unsafe static void HandleTargeterHighlights(GrydCardinalSegmentInfo parentSegment, bool thinnerLine, List<GameObject> highlights, ref int nextHighlightIndex)
	{
		if (parentSegment.IsValidSegment())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydCardinalSegmentInfo.HandleTargeterHighlights(GrydCardinalSegmentInfo, bool, List<GameObject>, int*)).MethodHandle;
			}
			Vector3 vector = parentSegment.m_startSquare.ToVector3();
			Vector3 vector2 = parentSegment.m_endSquare.ToVector3() - vector;
			vector2.y = 0f;
			float magnitude = vector2.magnitude;
			GameObject gameObject;
			if (highlights.Count <= nextHighlightIndex)
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
				gameObject = HighlightUtils.Get().CreateRectangularCursor(1f, magnitude, null);
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
				GrydCardinalSegmentInfo.HandleTargeterHighlights(parentSegment.m_childSegments[i], true, highlights, ref nextHighlightIndex);
			}
		}
	}
}
