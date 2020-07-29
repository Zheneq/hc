using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_Cross : AbilityUtil_Targeter
{
	private float m_minDistanceInSquares;

	private float m_maxDistanceInSquares;

	private float m_crossDistanceInSquares;

	private float m_widthInSquares;

	public float m_crossLengthDecreaseOverDistance;

	private bool m_lockToCardinalDirs;

	private bool m_discreteStepsForRange;

	private bool m_penetrateLoS;

	private int m_maxTargets;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_Cross(Ability ability, float minDistanceInSquares, float maxDistanceInSquares, float crossDistanceInSquares, float widthInSquares, bool penetrateLoS, int maxTargets, bool lockToCardinalDirs, bool discreteStepsForRange, bool includeAllies = false, bool affectsCaster = false, float crossLengthDecreaseOverDistance = 0f)
		: base(ability)
	{
		m_minDistanceInSquares = minDistanceInSquares;
		m_maxDistanceInSquares = maxDistanceInSquares;
		m_crossDistanceInSquares = crossDistanceInSquares;
		m_widthInSquares = widthInSquares;
		m_crossLengthDecreaseOverDistance = crossLengthDecreaseOverDistance;
		m_lockToCardinalDirs = lockToCardinalDirs;
		m_discreteStepsForRange = discreteStepsForRange;
		m_maxTargets = maxTargets;
		m_penetrateLoS = penetrateLoS;
		m_affectsAllies = includeAllies;
		m_affectsTargetingActor = affectsCaster;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	private Vector3 GetClampedTargeterRange(AbilityTarget currentTarget, Vector3 startPos, Vector3 aimDir, ref float dist, ref float crossLength)
	{
		return GrydLaserT.GetClampedTargeterRangeStatic(currentTarget, startPos, aimDir, m_minDistanceInSquares, m_maxDistanceInSquares, m_discreteStepsForRange, m_crossLengthDecreaseOverDistance, ref dist, ref crossLength);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		float squareSize = Board.Get().squareSize;
		float dist = m_minDistanceInSquares * squareSize;
		float crossLength = m_crossDistanceInSquares * squareSize;
		float widthInWorld = m_widthInSquares * squareSize;
		if (m_highlights.Count < 3)
		{
			ClearHighlightCursors();
			m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(widthInWorld, dist));
			m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(widthInWorld, crossLength));
			m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(widthInWorld, crossLength));
		}
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		Vector3 vector = travelBoardSquareWorldPosition + new Vector3(0f, 0.1f, 0f);
		Vector3 vector2 = currentTarget.AimDirection;
		if (m_lockToCardinalDirs)
		{
			vector2 = VectorUtils.HorizontalAngleToClosestCardinalDirection(Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vector2)));
		}
		Vector3 clampedTargeterRange = GetClampedTargeterRange(currentTarget, vector, vector2, ref dist, ref crossLength);
		float widthInWorld2 = (float)Mathf.CeilToInt(m_widthInSquares) * Board.SquareSizeStatic;
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld2, dist, m_highlights[0]);
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld2, 0.5f * crossLength, m_highlights[1]);
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld2, 0.5f * crossLength, m_highlights[2]);
		Vector3 normalized = Vector3.Cross(vector2, Vector3.up).normalized;
		m_highlights[0].transform.position = vector;
		m_highlights[0].transform.rotation = Quaternion.LookRotation(vector2);
		m_highlights[1].transform.position = clampedTargeterRange;
		m_highlights[1].transform.rotation = Quaternion.LookRotation(normalized);
		m_highlights[2].transform.position = clampedTargeterRange;
		m_highlights[2].transform.rotation = Quaternion.LookRotation(-normalized);
		float laserRangeInSquares = 0.5f * (crossLength / squareSize);
		Vector3 laserEndPos;
		List<ActorData> actors = AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, vector2, dist / squareSize, m_widthInSquares, targetingActor, GetAffectedTeams(), m_penetrateLoS, m_maxTargets, m_penetrateLoS, false, out laserEndPos, null);
		clampedTargeterRange.y = travelBoardSquareWorldPositionForLos.y;
		BoardSquare boardSquare = Board.Get().GetSquare(clampedTargeterRange);
		List<ActorData> list = new List<ActorData>();
		List<ActorData> list2 = new List<ActorData>();
		if (boardSquare != null && boardSquare.height <= Board.Get().BaselineHeight)
		{
			if (targetingActor.GetCurrentBoardSquare().LOSDistanceIsOne_zq(boardSquare.x, boardSquare.y))
			{
				BarrierManager.Get().GetAbilityLineEndpoint(targetingActor, travelBoardSquareWorldPositionForLos, clampedTargeterRange, out bool collision, out Vector3 _);
				if (!collision)
				{
					list2 = AreaEffectUtils.GetActorsInLaser(clampedTargeterRange, normalized, laserRangeInSquares, m_widthInSquares, targetingActor, GetAffectedTeams(), m_penetrateLoS, m_maxTargets, m_penetrateLoS, false, out laserEndPos, null, actors);
					actors.AddRange(list2);
					list = AreaEffectUtils.GetActorsInLaser(clampedTargeterRange, -1f * normalized, laserRangeInSquares, m_widthInSquares, targetingActor, GetAffectedTeams(), m_penetrateLoS, m_maxTargets, m_penetrateLoS, false, out laserEndPos, null, actors);
					actors.AddRange(list);
					TargeterUtils.LimitActorsToMaxNumber(ref actors, m_maxTargets);
				}
			}
		}
		using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (!list.Contains(current))
				{
					if (!list2.Contains(current))
					{
						AddActorInRange(current, travelBoardSquareWorldPosition, targetingActor);
						continue;
					}
				}
				AddActorInRange(current, clampedTargeterRange, targetingActor);
			}
		}
		if (m_affectsTargetingActor)
		{
			AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPositionForLos(), targetingActor);
		}
		Vector3 crossStart = clampedTargeterRange + (0.5f * crossLength - 0.1f) * normalized;
		Vector3 crossEnd = clampedTargeterRange - (0.5f * crossLength - 0.1f) * normalized;
		DrawInvalidSquareIndicators(currentTarget, targetingActor, travelBoardSquareWorldPositionForLos, clampedTargeterRange, crossStart, crossEnd);
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos, Vector3 crossStart, Vector3 crossEnd)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		ResetSquareIndicatorIndexToUse();
		AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, startPos, endPos, m_widthInSquares, targetingActor, m_penetrateLoS);
		BoardSquare boardSquare = Board.Get().GetSquare(endPos);
		if (boardSquare != null)
		{
			if (boardSquare.height <= Board.Get().BaselineHeight && targetingActor.GetCurrentBoardSquare().LOSDistanceIsOne_zq(boardSquare.x, boardSquare.y))
			{
				AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, endPos, crossStart, m_widthInSquares, targetingActor, m_penetrateLoS);
				AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, endPos, crossEnd, m_widthInSquares, targetingActor, m_penetrateLoS);
				goto IL_017f;
			}
		}
		float adjustAmount = 0f;
		AreaEffectUtils.GetBoxBoundsInGridPos(crossStart, crossEnd, adjustAmount, out int minX, out int minY, out int maxX, out int maxY);
		for (int i = minX; i <= maxX; i++)
		{
			for (int j = minY; j <= maxY; j++)
			{
				BoardSquare boardSquare2 = Board.Get().GetSquare(i, j);
				if (!(boardSquare2 == null) && AreaEffectUtils.IsSquareInBoxByActorRadius(boardSquare2, crossEnd, crossStart, m_widthInSquares))
				{
					m_indicatorHandler.OperateOnSquare(boardSquare2, targetingActor, false);
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					goto end_IL_015f;
				}
				continue;
				end_IL_015f:
				break;
			}
		}
		goto IL_017f;
		IL_017f:
		HideUnusedSquareIndicators();
	}
}
