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
		Vector3 vector = targetingActor.\u0015();
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(vector, currentTarget.AimDirection, this.m_dashRangeInSquares * Board.\u000E().squareSize, false, targetingActor, null, true);
		float num = (laserEndPoint - vector).magnitude;
		BoardSquare boardSquare;
		num = ClaymoreCharge.GetMaxPotentialChargeDistance(vector, laserEndPoint, currentTarget.AimDirection, num, targetingActor, out boardSquare);
		BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare, targetingActor.\u0012(), true);
		List<ActorData> actorsOnPath = ClaymoreCharge.GetActorsOnPath(path, targetingActor.\u0015(), targetingActor);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsOnPath);
		Vector3 vector2;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(vector, currentTarget.AimDirection, num / Board.\u000E().squareSize, this.m_dashWidthInSquares, targetingActor, targetingActor.\u0015(), false, 1, true, false, out vector2, null, null, false, true);
		actorsInLaser.AddRange(actorsOnPath);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInLaser, vector);
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
			Vector3 vector3 = (!this.m_directHitIgnoreCover) ? targetingActor.\u0016() : actorsInLaser[0].\u0016();
			base.AddActorInRange(actorsInLaser[0], vector3, targetingActor, AbilityTooltipSubject.Primary, false);
			Vector3 lhs = vector3 - vector;
			lhs.y = 0f;
			Vector3 vector4 = vector + Vector3.Dot(lhs, currentTarget.AimDirection) * currentTarget.AimDirection;
			vector2 = vector4;
			BoardSquarePathInfo pathToDesired = KnockbackUtils.BuildStraightLineChargePath(targetingActor, actorsInLaser[0].\u0012(), targetingActor.\u0012(), true);
			BoardSquare chargeDestination = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(targetingActor, actorsInLaser[0].\u0012(), pathToDesired);
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
				List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_aoeShape, chargeDestination.ToVector3(), chargeDestination, false, targetingActor, targetingActor.\u0012(), null);
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
		this.m_laserLosChecker.UpdateBoxProperties(vector, vector2, targetingActor);
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
				base.AddActorInRange(targetingActor, targetingActor.\u0016(), targetingActor, AbilityTooltipSubject.Self, false);
			}
		}
		gameObject.SetActive(active);
		Vector3 vector5 = laserEndPoint - vector;
		vector5.y = 0f;
		float magnitude = vector5.magnitude;
		vector5.Normalize();
		Vector3 vector6 = laserEndPoint;
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
			boardSquare4 = KnockbackUtils.GetLastValidBoardSquareInLine(vector, vector6, false, false, float.MaxValue);
		}
		BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare4, targetingActor.\u0012(), true);
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
				if (boardSquare4.OccupantActor.\u0018())
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
					boardSquare4 = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(targetingActor, boardSquare4.OccupantActor.\u0012(), boardSquarePathInfo);
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
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare4, targetingActor.\u0012(), true);
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
		Vector3 a = vector6;
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
		Vector3 lhs2 = a - vector;
		lhs2.y = 0f;
		float d = Vector3.Dot(lhs2, currentTarget.AimDirection) + 0.5f;
		Vector3 endPos = vector + d * currentTarget.AimDirection;
		endPos.y = HighlightUtils.GetHighlightHeight();
		HighlightUtils.Get().RotateAndResizeRectangularCursor(highlightObj, vector, endPos, this.m_dashWidthInSquares);
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
			Vector3 startPos = vector;
			Vector3 endPos2 = vector2;
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
		Vector3 idealTestVector = caster.\u0016() - b;
		idealTestVector.y = 0f;
		idealTestVector.Normalize();
		BoardSquare startSquare = caster.\u0012();
		BoardSquare bestDestinationInLayers = AbilityUtil_Targeter_ClaymoreCharge.GetBestDestinationInLayers(desiredDest, startSquare, caster, idealTestVector, secondToLastInOrigPath, 3, true);
		if (bestDestinationInLayers == null)
		{
			bestDestinationInLayers = AbilityUtil_Targeter_ClaymoreCharge.GetBestDestinationInLayers(desiredDest, startSquare, caster, idealTestVector, secondToLastInOrigPath, 3, false);
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
					if (boardSquare2.\u0016())
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
							if (boardSquare2.OccupantActor.\u0018())
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
