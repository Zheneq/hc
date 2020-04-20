using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ClaymoreCharge : AbilityUtil_Targeter
{
	private float m_dashWidthInSquares;

	public float m_dashRangeInSquares;

	private AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;

	private bool m_directHitIgnoreCover;

	public AbilityUtil_Targeter_ClaymoreCharge.IsAffectingCasterDelegate m_affectCasterDelegate;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private SquareInsideChecker_Box m_laserLosChecker;

	private SquareInsideChecker_Shape m_shapeLosChecker;

	private SquareInsideChecker_Path m_pathLosChecker;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	private List<ISquareInsideChecker> m_squarePosCheckerListNoAoe = new List<ISquareInsideChecker>();

	public AbilityUtil_Targeter_ClaymoreCharge(Ability ability, float dashWidthInSquares, float dashRangeInSquares, AbilityAreaShape aoeShape, bool directHitIgnoreCover) : base(ability)
	{
		this.m_dashWidthInSquares = dashWidthInSquares;
		this.m_dashRangeInSquares = dashRangeInSquares;
		this.m_aoeShape = aoeShape;
		this.m_directHitIgnoreCover = directHitIgnoreCover;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_laserLosChecker = new SquareInsideChecker_Box(this.m_dashWidthInSquares);
		this.m_shapeLosChecker = new SquareInsideChecker_Shape(this.m_aoeShape);
		this.m_pathLosChecker = new SquareInsideChecker_Path();
		this.m_squarePosCheckerList.Add(this.m_laserLosChecker);
		this.m_squarePosCheckerList.Add(this.m_shapeLosChecker);
		this.m_squarePosCheckerList.Add(this.m_pathLosChecker);
		this.m_squarePosCheckerListNoAoe.Add(this.m_laserLosChecker);
		this.m_squarePosCheckerListNoAoe.Add(this.m_pathLosChecker);
	}

	public int LastUpdatePathSquareCount { get; set; }

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		this.LastUpdatePathSquareCount = 0;
		if (this.m_highlights != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ClaymoreCharge.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			if (this.m_highlights.Count >= 2)
			{
				goto IL_92;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
		this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_aoeShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		IL_92:
		GameObject highlightObj = this.m_highlights[0];
		GameObject gameObject = this.m_highlights[1];
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, this.m_dashRangeInSquares * Board.Get().squareSize, false, targetingActor, null, true);
		float num = (laserEndPoint - travelBoardSquareWorldPositionForLos).magnitude;
		BoardSquare boardSquare;
		num = ClaymoreCharge.GetMaxPotentialChargeDistance(travelBoardSquareWorldPositionForLos, laserEndPoint, currentTarget.AimDirection, num, targetingActor, out boardSquare);
		BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare, targetingActor.GetCurrentBoardSquare(), true);
		List<ActorData> actorsOnPath = ClaymoreCharge.GetActorsOnPath(path, targetingActor.GetOpposingTeams(), targetingActor);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsOnPath);
		Vector3 vector;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, num / Board.Get().squareSize, this.m_dashWidthInSquares, targetingActor, targetingActor.GetOpposingTeams(), false, 1, true, false, out vector, null, null, false, true);
		actorsInLaser.AddRange(actorsOnPath);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInLaser, travelBoardSquareWorldPositionForLos);
		BoardSquare boardSquare2 = null;
		bool active = false;
		if (actorsInLaser.Count > 0)
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
			Vector3 vector2 = (!this.m_directHitIgnoreCover) ? targetingActor.GetTravelBoardSquareWorldPosition() : actorsInLaser[0].GetTravelBoardSquareWorldPosition();
			base.AddActorInRange(actorsInLaser[0], vector2, targetingActor, AbilityTooltipSubject.Primary, false);
			Vector3 lhs = vector2 - travelBoardSquareWorldPositionForLos;
			lhs.y = 0f;
			Vector3 vector3 = travelBoardSquareWorldPositionForLos + Vector3.Dot(lhs, currentTarget.AimDirection) * currentTarget.AimDirection;
			vector = vector3;
			BoardSquarePathInfo pathToDesired = KnockbackUtils.BuildStraightLineChargePath(targetingActor, actorsInLaser[0].GetCurrentBoardSquare(), targetingActor.GetCurrentBoardSquare(), true);
			BoardSquare chargeDestination = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(targetingActor, actorsInLaser[0].GetCurrentBoardSquare(), pathToDesired);
			if (chargeDestination != null)
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
				List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_aoeShape, chargeDestination.ToVector3(), chargeDestination, false, targetingActor, targetingActor.GetOpposingTeam(), null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
				using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actor = enumerator.Current;
						base.AddActorInRange(actor, chargeDestination.ToVector3(), targetingActor, AbilityTooltipSubject.Secondary, false);
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
				active = true;
				gameObject.transform.position = chargeDestination.ToVector3();
				boardSquare2 = chargeDestination;
				this.m_shapeLosChecker.UpdateShapeProperties(chargeDestination.ToVector3(), chargeDestination, targetingActor);
			}
		}
		this.m_laserLosChecker.UpdateBoxProperties(travelBoardSquareWorldPositionForLos, vector, targetingActor);
		if (this.m_affectCasterDelegate != null)
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
			if (this.m_affectCasterDelegate(targetingActor, this.GetVisibleActorsInRange()))
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
				base.AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self, false);
			}
		}
		gameObject.SetActive(active);
		Vector3 vector4 = laserEndPoint - travelBoardSquareWorldPositionForLos;
		vector4.y = 0f;
		float magnitude = vector4.magnitude;
		vector4.Normalize();
		Vector3 vector5 = laserEndPoint;
		BoardSquare boardSquare3;
		if (boardSquare2 != null)
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
			boardSquare3 = boardSquare2;
		}
		else
		{
			boardSquare3 = boardSquare;
		}
		BoardSquare boardSquare4 = boardSquare3;
		if (boardSquare4 == null)
		{
			boardSquare4 = KnockbackUtils.GetLastValidBoardSquareInLine(travelBoardSquareWorldPositionForLos, vector5, false, false, float.MaxValue);
		}
		BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare4, targetingActor.GetCurrentBoardSquare(), true);
		bool flag = false;
		if (boardSquarePathInfo != null)
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
			if (boardSquarePathInfo.next != null && boardSquare2 == null)
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
				boardSquare4 = ClaymoreCharge.GetTrimmedDestinationInPath(boardSquarePathInfo, out flag);
			}
		}
		if (boardSquare4 != null && boardSquare4.OccupantActor != null)
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
			if (boardSquare4.OccupantActor != targetingActor)
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
				if (boardSquare4.OccupantActor.IsVisibleToClient())
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
					boardSquare4 = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(targetingActor, boardSquare4.OccupantActor.GetCurrentBoardSquare(), boardSquarePathInfo);
					flag = true;
				}
			}
		}
		if (flag)
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
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare4, targetingActor.GetCurrentBoardSquare(), true);
		}
		int num2 = 0;
		base.EnableAllMovementArrows();
		num2 = base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, num2, false);
		base.SetMovementArrowEnabledFromIndex(num2, false);
		this.m_pathLosChecker.UpdateSquaresInPath(boardSquarePathInfo);
		if (boardSquarePathInfo != null)
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
			this.LastUpdatePathSquareCount = boardSquarePathInfo.GetNumSquaresToEnd(true);
		}
		Vector3 a = vector5;
		if (boardSquarePathInfo != null)
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
			BoardSquarePathInfo pathEndpoint = boardSquarePathInfo.GetPathEndpoint();
			a = pathEndpoint.square.ToVector3();
		}
		Vector3 lhs2 = a - travelBoardSquareWorldPositionForLos;
		lhs2.y = 0f;
		float d = Vector3.Dot(lhs2, currentTarget.AimDirection) + 0.5f;
		Vector3 endPos = travelBoardSquareWorldPositionForLos + d * currentTarget.AimDirection;
		endPos.y = HighlightUtils.GetHighlightHeight();
		HighlightUtils.Get().RotateAndResizeRectangularCursor(highlightObj, travelBoardSquareWorldPositionForLos, endPos, this.m_dashWidthInSquares);
		if (GameFlowData.Get().activeOwnedActorData == targetingActor)
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
			base.ResetSquareIndicatorIndexToUse();
			IOperationOnSquare indicatorHandler = this.m_indicatorHandler;
			Vector3 startPos = travelBoardSquareWorldPositionForLos;
			Vector3 endPos2 = vector;
			float dashWidthInSquares = this.m_dashWidthInSquares;
			bool ignoreLos = false;
			List<Vector3> additionalLosSources = null;
			List<ISquareInsideChecker> losCheckOverrides;
			if (boardSquare2 != null)
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
				losCheckOverrides = this.m_squarePosCheckerList;
			}
			else
			{
				losCheckOverrides = this.m_squarePosCheckerListNoAoe;
			}
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(indicatorHandler, startPos, endPos2, dashWidthInSquares, targetingActor, ignoreLos, additionalLosSources, losCheckOverrides, true);
			if (boardSquare2 != null)
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
				AreaEffectUtils.OperateOnSquaresInShape(this.m_indicatorHandler, this.m_aoeShape, this.m_shapeLosChecker.m_freePos, this.m_shapeLosChecker.m_centerSquare, false, targetingActor, this.m_squarePosCheckerList);
			}
			base.HideUnusedSquareIndicators();
		}
	}

	public static BoardSquare GetChargeDestination(ActorData caster, BoardSquare desiredDest, BoardSquarePathInfo pathToDesired)
	{
		BoardSquare secondToLastInOrigPath = null;
		if (pathToDesired != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(ActorData, BoardSquare, BoardSquarePathInfo)).MethodHandle;
			}
			BoardSquarePathInfo pathEndpoint = pathToDesired.GetPathEndpoint();
			if (pathEndpoint.prev != null)
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
				secondToLastInOrigPath = pathEndpoint.prev.square;
			}
		}
		Vector3 b = desiredDest.ToVector3();
		Vector3 idealTestVector = caster.GetTravelBoardSquareWorldPosition() - b;
		idealTestVector.y = 0f;
		idealTestVector.Normalize();
		BoardSquare currentBoardSquare = caster.GetCurrentBoardSquare();
		BoardSquare bestDestinationInLayers = AbilityUtil_Targeter_ClaymoreCharge.GetBestDestinationInLayers(desiredDest, currentBoardSquare, caster, idealTestVector, secondToLastInOrigPath, 3, true);
		if (bestDestinationInLayers == null)
		{
			bestDestinationInLayers = AbilityUtil_Targeter_ClaymoreCharge.GetBestDestinationInLayers(desiredDest, currentBoardSquare, caster, idealTestVector, secondToLastInOrigPath, 3, false);
		}
		return bestDestinationInLayers;
	}

	private static BoardSquare GetBestDestinationInLayers(BoardSquare desiredDestSquare, BoardSquare startSquare, ActorData caster, Vector3 idealTestVector, BoardSquare secondToLastInOrigPath, int maxLayers, bool requireLosToStart)
	{
		BoardSquare boardSquare = null;
		float num = -1000f;
		Vector3 b = desiredDestSquare.ToVector3();
		int i = 1;
		while (i < maxLayers)
		{
			if (!(boardSquare == null))
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					return boardSquare;
				}
			}
			else
			{
				List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(desiredDestSquare, i, true);
				for (int j = 0; j < squaresInBorderLayer.Count; j++)
				{
					BoardSquare boardSquare2 = squaresInBorderLayer[j];
					if (boardSquare2.IsBaselineHeight())
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ClaymoreCharge.GetBestDestinationInLayers(BoardSquare, BoardSquare, ActorData, Vector3, BoardSquare, int, bool)).MethodHandle;
						}
						if (!(boardSquare2.OccupantActor == null))
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
							if (boardSquare2.OccupantActor.IsVisibleToClient())
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
								if (!(boardSquare2.OccupantActor == caster))
								{
									goto IL_180;
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
						Vector3 rhs = boardSquare2.ToVector3() - b;
						rhs.y = 0f;
						rhs.Normalize();
						float num2 = Vector3.Dot(idealTestVector, rhs);
						if (secondToLastInOrigPath != null)
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
							if (boardSquare2 == secondToLastInOrigPath)
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
								num2 += 0.5f;
							}
						}
						bool flag = startSquare.\u0013(boardSquare2.x, boardSquare2.y);
						if (!flag && requireLosToStart)
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
						}
						else
						{
							if (!flag)
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
								num2 -= 2f;
							}
							if (!(boardSquare == null))
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
								if (num2 <= num)
								{
									goto IL_180;
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
							boardSquare = boardSquare2;
							num = num2;
						}
					}
					IL_180:;
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
				i++;
			}
		}
		return boardSquare;
	}

	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);
}
