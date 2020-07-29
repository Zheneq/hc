using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ClaymoreCharge : AbilityUtil_Targeter
{
	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	private float m_dashWidthInSquares;

	public float m_dashRangeInSquares;

	private AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;

	private bool m_directHitIgnoreCover;

	public IsAffectingCasterDelegate m_affectCasterDelegate;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private SquareInsideChecker_Box m_laserLosChecker;

	private SquareInsideChecker_Shape m_shapeLosChecker;

	private SquareInsideChecker_Path m_pathLosChecker;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	private List<ISquareInsideChecker> m_squarePosCheckerListNoAoe = new List<ISquareInsideChecker>();

	public int LastUpdatePathSquareCount
	{
		get;
		set;
	}

	public AbilityUtil_Targeter_ClaymoreCharge(Ability ability, float dashWidthInSquares, float dashRangeInSquares, AbilityAreaShape aoeShape, bool directHitIgnoreCover)
		: base(ability)
	{
		m_dashWidthInSquares = dashWidthInSquares;
		m_dashRangeInSquares = dashRangeInSquares;
		m_aoeShape = aoeShape;
		m_directHitIgnoreCover = directHitIgnoreCover;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_laserLosChecker = new SquareInsideChecker_Box(m_dashWidthInSquares);
		m_shapeLosChecker = new SquareInsideChecker_Shape(m_aoeShape);
		m_pathLosChecker = new SquareInsideChecker_Path();
		m_squarePosCheckerList.Add(m_laserLosChecker);
		m_squarePosCheckerList.Add(m_shapeLosChecker);
		m_squarePosCheckerList.Add(m_pathLosChecker);
		m_squarePosCheckerListNoAoe.Add(m_laserLosChecker);
		m_squarePosCheckerListNoAoe.Add(m_pathLosChecker);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		LastUpdatePathSquareCount = 0;
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 2)
			{
				goto IL_0092;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
		m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_aoeShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		goto IL_0092;
		IL_0092:
		GameObject highlightObj = m_highlights[0];
		GameObject gameObject = m_highlights[1];
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, m_dashRangeInSquares * Board.Get().squareSize, false, targetingActor);
		float magnitude = (laserEndPoint - travelBoardSquareWorldPositionForLos).magnitude;
		magnitude = ClaymoreCharge.GetMaxPotentialChargeDistance(travelBoardSquareWorldPositionForLos, laserEndPoint, currentTarget.AimDirection, magnitude, targetingActor, out BoardSquare pathEndSquare);
		BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, pathEndSquare, targetingActor.GetCurrentBoardSquare(), true);
		List<ActorData> actors = ClaymoreCharge.GetActorsOnPath(path, targetingActor.GetOpposingTeams(), targetingActor);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		Vector3 laserEndPos;
		List<ActorData> actors2 = AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, magnitude / Board.Get().squareSize, m_dashWidthInSquares, targetingActor, targetingActor.GetOpposingTeams(), false, 1, true, false, out laserEndPos, null);
		actors2.AddRange(actors);
		TargeterUtils.SortActorsByDistanceToPos(ref actors2, travelBoardSquareWorldPositionForLos);
		BoardSquare boardSquare = null;
		bool active = false;
		if (actors2.Count > 0)
		{
			Vector3 vector = (!m_directHitIgnoreCover) ? targetingActor.GetTravelBoardSquareWorldPosition() : actors2[0].GetTravelBoardSquareWorldPosition();
			AddActorInRange(actors2[0], vector, targetingActor);
			Vector3 lhs = vector - travelBoardSquareWorldPositionForLos;
			lhs.y = 0f;
			Vector3 vector2 = travelBoardSquareWorldPositionForLos + Vector3.Dot(lhs, currentTarget.AimDirection) * currentTarget.AimDirection;
			laserEndPos = vector2;
			BoardSquarePathInfo pathToDesired = KnockbackUtils.BuildStraightLineChargePath(targetingActor, actors2[0].GetCurrentBoardSquare(), targetingActor.GetCurrentBoardSquare(), true);
			BoardSquare chargeDestination = GetChargeDestination(targetingActor, actors2[0].GetCurrentBoardSquare(), pathToDesired);
			if (chargeDestination != null)
			{
				List<ActorData> actors3 = AreaEffectUtils.GetActorsInShape(m_aoeShape, chargeDestination.ToVector3(), chargeDestination, false, targetingActor, targetingActor.GetOpposingTeam(), null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actors3);
				using (List<ActorData>.Enumerator enumerator = actors3.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData current = enumerator.Current;
						AddActorInRange(current, chargeDestination.ToVector3(), targetingActor, AbilityTooltipSubject.Secondary);
					}
				}
				active = true;
				gameObject.transform.position = chargeDestination.ToVector3();
				boardSquare = chargeDestination;
				m_shapeLosChecker.UpdateShapeProperties(chargeDestination.ToVector3(), chargeDestination, targetingActor);
			}
		}
		m_laserLosChecker.UpdateBoxProperties(travelBoardSquareWorldPositionForLos, laserEndPos, targetingActor);
		if (m_affectCasterDelegate != null)
		{
			if (m_affectCasterDelegate(targetingActor, GetVisibleActorsInRange()))
			{
				AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self);
			}
		}
		gameObject.SetActive(active);
		Vector3 vector3 = laserEndPoint - travelBoardSquareWorldPositionForLos;
		vector3.y = 0f;
		float magnitude2 = vector3.magnitude;
		vector3.Normalize();
		Vector3 vector4 = laserEndPoint;
		BoardSquare boardSquare2;
		if (boardSquare != null)
		{
			boardSquare2 = boardSquare;
		}
		else
		{
			boardSquare2 = pathEndSquare;
		}
		BoardSquare boardSquare3 = boardSquare2;
		if (boardSquare3 == null)
		{
			boardSquare3 = KnockbackUtils.GetLastValidBoardSquareInLine(travelBoardSquareWorldPositionForLos, vector4);
		}
		BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare3, targetingActor.GetCurrentBoardSquare(), true);
		bool differentFromInputDest = false;
		if (boardSquarePathInfo != null)
		{
			if (boardSquarePathInfo.next != null && boardSquare == null)
			{
				boardSquare3 = ClaymoreCharge.GetTrimmedDestinationInPath(boardSquarePathInfo, out differentFromInputDest);
			}
		}
		if (boardSquare3 != null && boardSquare3.OccupantActor != null)
		{
			if (boardSquare3.OccupantActor != targetingActor)
			{
				if (boardSquare3.OccupantActor.IsVisibleToClient())
				{
					boardSquare3 = GetChargeDestination(targetingActor, boardSquare3.OccupantActor.GetCurrentBoardSquare(), boardSquarePathInfo);
					differentFromInputDest = true;
				}
			}
		}
		if (differentFromInputDest)
		{
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare3, targetingActor.GetCurrentBoardSquare(), true);
		}
		int arrowIndex = 0;
		EnableAllMovementArrows();
		arrowIndex = AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, arrowIndex);
		SetMovementArrowEnabledFromIndex(arrowIndex, false);
		m_pathLosChecker.UpdateSquaresInPath(boardSquarePathInfo);
		if (boardSquarePathInfo != null)
		{
			LastUpdatePathSquareCount = boardSquarePathInfo.GetNumSquaresToEnd();
		}
		Vector3 a = vector4;
		if (boardSquarePathInfo != null)
		{
			BoardSquarePathInfo pathEndpoint = boardSquarePathInfo.GetPathEndpoint();
			a = pathEndpoint.square.ToVector3();
		}
		Vector3 lhs2 = a - travelBoardSquareWorldPositionForLos;
		lhs2.y = 0f;
		float d = Vector3.Dot(lhs2, currentTarget.AimDirection) + 0.5f;
		Vector3 endPos = travelBoardSquareWorldPositionForLos + d * currentTarget.AimDirection;
		endPos.y = HighlightUtils.GetHighlightHeight();
		HighlightUtils.Get().RotateAndResizeRectangularCursor(highlightObj, travelBoardSquareWorldPositionForLos, endPos, m_dashWidthInSquares);
		if (!(GameFlowData.Get().activeOwnedActorData == targetingActor))
		{
			return;
		}
		while (true)
		{
			ResetSquareIndicatorIndexToUse();
			OperationOnSquare_TurnOnHiddenSquareIndicator indicatorHandler = m_indicatorHandler;
			Vector3 endPos2 = laserEndPos;
			float dashWidthInSquares = m_dashWidthInSquares;
			List<ISquareInsideChecker> losCheckOverrides;
			if (boardSquare != null)
			{
				losCheckOverrides = m_squarePosCheckerList;
			}
			else
			{
				losCheckOverrides = m_squarePosCheckerListNoAoe;
			}
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(indicatorHandler, travelBoardSquareWorldPositionForLos, endPos2, dashWidthInSquares, targetingActor, false, null, losCheckOverrides);
			if (boardSquare != null)
			{
				AreaEffectUtils.OperateOnSquaresInShape(m_indicatorHandler, m_aoeShape, m_shapeLosChecker.m_freePos, m_shapeLosChecker.m_centerSquare, false, targetingActor, m_squarePosCheckerList);
			}
			HideUnusedSquareIndicators();
			return;
		}
	}

	public static BoardSquare GetChargeDestination(ActorData caster, BoardSquare desiredDest, BoardSquarePathInfo pathToDesired)
	{
		BoardSquare secondToLastInOrigPath = null;
		if (pathToDesired != null)
		{
			BoardSquarePathInfo pathEndpoint = pathToDesired.GetPathEndpoint();
			if (pathEndpoint.prev != null)
			{
				secondToLastInOrigPath = pathEndpoint.prev.square;
			}
		}
		BoardSquare boardSquare = null;
		Vector3 b = desiredDest.ToVector3();
		Vector3 idealTestVector = caster.GetTravelBoardSquareWorldPosition() - b;
		idealTestVector.y = 0f;
		idealTestVector.Normalize();
		BoardSquare currentBoardSquare = caster.GetCurrentBoardSquare();
		boardSquare = GetBestDestinationInLayers(desiredDest, currentBoardSquare, caster, idealTestVector, secondToLastInOrigPath, 3, true);
		if (boardSquare == null)
		{
			boardSquare = GetBestDestinationInLayers(desiredDest, currentBoardSquare, caster, idealTestVector, secondToLastInOrigPath, 3, false);
		}
		return boardSquare;
	}

	private static BoardSquare GetBestDestinationInLayers(BoardSquare desiredDestSquare, BoardSquare startSquare, ActorData caster, Vector3 idealTestVector, BoardSquare secondToLastInOrigPath, int maxLayers, bool requireLosToStart)
	{
		BoardSquare boardSquare = null;
		float num = -1000f;
		Vector3 b = desiredDestSquare.ToVector3();
		for (int i = 1; i < maxLayers; i++)
		{
			if (boardSquare == null)
			{
				List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(desiredDestSquare, i, true);
				for (int j = 0; j < squaresInBorderLayer.Count; j++)
				{
					BoardSquare boardSquare2 = squaresInBorderLayer[j];
					if (!boardSquare2.IsBaselineHeight())
					{
						continue;
					}
					if (!(boardSquare2.OccupantActor == null))
					{
						if (boardSquare2.OccupantActor.IsVisibleToClient())
						{
							if (!(boardSquare2.OccupantActor == caster))
							{
								continue;
							}
						}
					}
					Vector3 rhs = boardSquare2.ToVector3() - b;
					rhs.y = 0f;
					rhs.Normalize();
					float num2 = Vector3.Dot(idealTestVector, rhs);
					if (secondToLastInOrigPath != null)
					{
						if (boardSquare2 == secondToLastInOrigPath)
						{
							num2 += 0.5f;
						}
					}
					bool flag = startSquare.LOSDistanceIsOne_zq(boardSquare2.x, boardSquare2.y);
					if (!flag && requireLosToStart)
					{
						continue;
					}
					if (!flag)
					{
						num2 -= 2f;
					}
					if (!(boardSquare == null))
					{
						if (!(num2 > num))
						{
							continue;
						}
					}
					boardSquare = boardSquare2;
					num = num2;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						goto end_IL_0196;
					}
					continue;
					end_IL_0196:
					break;
				}
				continue;
			}
			break;
		}
		return boardSquare;
	}
}
