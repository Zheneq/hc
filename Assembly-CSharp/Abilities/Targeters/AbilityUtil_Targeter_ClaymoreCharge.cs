using System.Collections.Generic;
using UnityEngine;

// reworked in rogues
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

	public AbilityUtil_Targeter_ClaymoreCharge(
		Ability ability,
		float dashWidthInSquares,
		float dashRangeInSquares,
		AbilityAreaShape aoeShape,
		bool directHitIgnoreCover)
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
		if (m_highlights == null || m_highlights.Count < 2)
		{
			m_highlights = new List<GameObject>
			{
				HighlightUtils.Get().CreateRectangularCursor(1f, 1f),
				HighlightUtils.Get().CreateShapeCursor(m_aoeShape, targetingActor == GameFlowData.Get().activeOwnedActorData)
			};
		}
		GameObject highlight0 = m_highlights[0];
		GameObject highlight1 = m_highlights[1];
		Vector3 loSCheckPos = targetingActor.GetLoSCheckPos();
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(
			loSCheckPos,
			currentTarget.AimDirection,
			m_dashRangeInSquares * Board.Get().squareSize,
			false,
			targetingActor);
		float magnitude = (laserEndPoint - loSCheckPos).magnitude;
		magnitude = ClaymoreCharge.GetMaxPotentialChargeDistance(
			loSCheckPos,
			laserEndPoint,
			currentTarget.AimDirection,
			magnitude,
			targetingActor,
			out BoardSquare pathEndSquare);
		BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(
			targetingActor,
			pathEndSquare,
			targetingActor.GetCurrentBoardSquare(),
			true);
		List<ActorData> actorsOnPath = ClaymoreCharge.GetActorsOnPath(path, targetingActor.GetEnemyTeamAsList(), targetingActor);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsOnPath);
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			loSCheckPos,
			currentTarget.AimDirection,
			magnitude / Board.Get().squareSize,
			m_dashWidthInSquares,
			targetingActor,
			targetingActor.GetEnemyTeamAsList(),
			false,
			1,
			true,
			false,
			out Vector3 laserEndPos,
			null);
		actorsInLaser.AddRange(actorsOnPath);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInLaser, loSCheckPos);
		BoardSquare boardSquare = null;
		bool active = false;
		if (actorsInLaser.Count > 0)
		{
			Vector3 vector = m_directHitIgnoreCover ? actorsInLaser[0].GetFreePos() : targetingActor.GetFreePos();
			AddActorInRange(actorsInLaser[0], vector, targetingActor);
			Vector3 lhs = vector - loSCheckPos;
			lhs.y = 0f;
			Vector3 vector2 = loSCheckPos + Vector3.Dot(lhs, currentTarget.AimDirection) * currentTarget.AimDirection;
			laserEndPos = vector2;
			BoardSquarePathInfo pathToDesired = KnockbackUtils.BuildStraightLineChargePath(
				targetingActor,
				actorsInLaser[0].GetCurrentBoardSquare(),
				targetingActor.GetCurrentBoardSquare(),
				true);
			BoardSquare chargeDestination = GetChargeDestination(
				targetingActor,
				actorsInLaser[0].GetCurrentBoardSquare(),
				pathToDesired);
			if (chargeDestination != null)
			{
				List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
					m_aoeShape,
					chargeDestination.ToVector3(),
					chargeDestination,
					false,
					targetingActor,
					targetingActor.GetEnemyTeam(),
					null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
				foreach (ActorData actor in actorsInShape)
				{
					AddActorInRange(actor, chargeDestination.ToVector3(), targetingActor, AbilityTooltipSubject.Secondary);
				}
				active = true;
				highlight1.transform.position = chargeDestination.ToVector3();
				boardSquare = chargeDestination;
				m_shapeLosChecker.UpdateShapeProperties(chargeDestination.ToVector3(), chargeDestination, targetingActor);
			}
		}
		m_laserLosChecker.UpdateBoxProperties(loSCheckPos, laserEndPos, targetingActor);
		if (m_affectCasterDelegate != null && m_affectCasterDelegate(targetingActor, GetVisibleActorsInRange()))
		{
			AddActorInRange(targetingActor, targetingActor.GetFreePos(), targetingActor, AbilityTooltipSubject.Self);
		}
		highlight1.SetActive(active);
		Vector3 vector3 = laserEndPoint - loSCheckPos;
		vector3.y = 0f;
		float magnitude2 = vector3.magnitude;
		vector3.Normalize();
		Vector3 vector4 = laserEndPoint;
		BoardSquare finishSquare = boardSquare != null ? boardSquare : pathEndSquare;
		if (finishSquare == null)
		{
			finishSquare = KnockbackUtils.GetLastValidBoardSquareInLine(loSCheckPos, vector4);
		}
		BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(
			targetingActor,
			finishSquare,
			targetingActor.GetCurrentBoardSquare(),
			true);
		bool differentFromInputDest = false;
		if (boardSquarePathInfo != null
		    && boardSquarePathInfo.next != null
		    && boardSquare == null)
		{
			finishSquare = ClaymoreCharge.GetTrimmedDestinationInPath(boardSquarePathInfo, out differentFromInputDest);
		}
		if (finishSquare != null
		    && finishSquare.OccupantActor != null
		    && finishSquare.OccupantActor != targetingActor
		    && finishSquare.OccupantActor.IsActorVisibleToClient())
		{
			finishSquare = GetChargeDestination(targetingActor, finishSquare.OccupantActor.GetCurrentBoardSquare(), boardSquarePathInfo);
			differentFromInputDest = true;
		}
		if (differentFromInputDest)
		{
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(
				targetingActor,
				finishSquare,
				targetingActor.GetCurrentBoardSquare(),
				true);
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
		Vector3 lhs2 = a - loSCheckPos;
		lhs2.y = 0f;
		float d = Vector3.Dot(lhs2, currentTarget.AimDirection) + 0.5f;
		Vector3 endPos = loSCheckPos + d * currentTarget.AimDirection;
		endPos.y = HighlightUtils.GetHighlightHeight();
		HighlightUtils.Get().RotateAndResizeRectangularCursor(highlight0, loSCheckPos, endPos, m_dashWidthInSquares);
		if (GameFlowData.Get().activeOwnedActorData == targetingActor)
		{
			ResetSquareIndicatorIndexToUse();
			OperationOnSquare_TurnOnHiddenSquareIndicator indicatorHandler = m_indicatorHandler;
			float dashWidthInSquares = m_dashWidthInSquares;

			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(
				indicatorHandler,
				loSCheckPos,
				laserEndPos,
				dashWidthInSquares,
				targetingActor,
				false,
				null,
				boardSquare != null ? m_squarePosCheckerList : m_squarePosCheckerListNoAoe);
			if (boardSquare != null)
			{
				AreaEffectUtils.OperateOnSquaresInShape(
					m_indicatorHandler,
					m_aoeShape,
					m_shapeLosChecker.m_freePos,
					m_shapeLosChecker.m_centerSquare,
					false,
					targetingActor,
					m_squarePosCheckerList);
			}
			HideUnusedSquareIndicators();
		}
	}

	public static BoardSquare GetChargeDestination(ActorData caster, BoardSquare desiredDest, BoardSquarePathInfo pathToDesired)
	{
		BoardSquare secondToLastInOrigPath = null;
		BoardSquarePathInfo pathEndpoint = pathToDesired?.GetPathEndpoint();
		if (pathEndpoint?.prev != null)
		{
			secondToLastInOrigPath = pathEndpoint.prev.square;
		}
		BoardSquare boardSquare = null;
		Vector3 b = desiredDest.ToVector3();
		Vector3 idealTestVector = caster.GetFreePos() - b;
		idealTestVector.y = 0f;
		idealTestVector.Normalize();
		// TODO DASH
#if SERVER
		BoardSquare currentBoardSquare = caster.GetSquareAtPhaseStart(); // custom
#else
		BoardSquare currentBoardSquare = caster.GetCurrentBoardSquare();
#endif
		boardSquare = GetBestDestinationInLayers(
			desiredDest,
			currentBoardSquare,
			caster,
			idealTestVector,
			secondToLastInOrigPath,
			3,
			true);
		if (boardSquare == null)
		{
			boardSquare = GetBestDestinationInLayers(
				desiredDest,
				currentBoardSquare,
				caster,
				idealTestVector,
				secondToLastInOrigPath,
				3,
				false);
		}
		return boardSquare;
	}

	private static BoardSquare GetBestDestinationInLayers(
		BoardSquare desiredDestSquare,
		BoardSquare startSquare,
		ActorData caster,
		Vector3 idealTestVector,
		BoardSquare secondToLastInOrigPath,
		int maxLayers,
		bool requireLosToStart)
	{
		BoardSquare dest = null;
		float num = -1000f;
		Vector3 b = desiredDestSquare.ToVector3();
		for (int i = 1; i < maxLayers; i++)
		{
			if (dest != null)
			{
				break;
			}
			List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(desiredDestSquare, i, true);
			foreach (BoardSquare square in squaresInBorderLayer)
			{
				if (!square.IsValidForGameplay())
				{
					continue;
				}
				if (square.OccupantActor != null
				    // TODO DASH
#if SERVER
				    && !ServerActionBuffer.Get().ActorIsEvading(square.OccupantActor) // custom
#else
				    && square.OccupantActor.IsActorVisibleToClient()
#endif
				    && square.OccupantActor != caster)
				{
					continue;
				}
				Vector3 rhs = square.ToVector3() - b;
				rhs.y = 0f;
				rhs.Normalize();
				float num2 = Vector3.Dot(idealTestVector, rhs);
				if (secondToLastInOrigPath != null && square == secondToLastInOrigPath)
				{
					num2 += 0.5f;
				}
				bool flag = startSquare.GetLOS(square.x, square.y);
				if (flag || !requireLosToStart)
				{
					if (!flag)
					{
						num2 -= 2f;
					}
					if (dest == null || num2 > num)
					{
						dest = square;
						num = num2;
					}
				}
			}
		}
		return dest;
	}
}
