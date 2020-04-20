using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_DashAndAim : AbilityUtil_Targeter
{
	private float m_cursorSpeed = 1f;

	private float m_aoeRadius;

	private bool m_aoePenetratesLoS;

	private float m_laserWidth;

	private float m_laserRange;

	private float m_maxAngleForLaser;

	private bool m_aimBackward;

	private bool m_teleportPath;

	private int m_maxTargets;

	private AbilityUtil_Targeter_DashAndAim.GetClampedLaserDirection getClampedLaserDirection;

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public AbilityUtil_Targeter_DashAndAim(Ability ability, float aoeRadius, bool aoePenetratesLoS, float laserWidth, float laserRange, float maxAngleForLaser, AbilityUtil_Targeter_DashAndAim.GetClampedLaserDirection laserDirDelegate, bool aimBackward, bool teleportPath, int maxTargets) : base(ability)
	{
		this.m_showArcToShape = false;
		this.AllowChargeThroughInvalidSquares = true;
		this.m_aoeRadius = aoeRadius;
		this.m_aoePenetratesLoS = aoePenetratesLoS;
		this.m_laserWidth = laserWidth;
		this.m_laserRange = laserRange;
		this.m_maxAngleForLaser = maxAngleForLaser;
		this.m_aimBackward = aimBackward;
		this.m_teleportPath = teleportPath;
		this.m_maxTargets = maxTargets;
		this.getClampedLaserDirection = laserDirDelegate;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_squarePosCheckerList.Add(new SquareInsideChecker_Box(this.m_laserWidth));
		this.m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
	}

	public bool AllowChargeThroughInvalidSquares { get; set; }

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (this.m_highlights.Count >= 2)
		{
			this.m_highlights[0].SetActive(false);
			this.m_highlights[1].SetActive(false);
		}
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.ClearActorsInRange();
		if (this.m_highlights == null || this.m_highlights.Count < 2)
		{
			this.m_highlights = new List<GameObject>();
			this.m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(this.m_laserRange + this.m_aoeRadius, 0.2f, true, Color.cyan));
			this.m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(this.m_laserRange + this.m_aoeRadius, 0.2f, true, Color.cyan));
		}
		GameObject gameObject = this.m_highlights[0];
		GameObject gameObject2 = this.m_highlights[1];
		int num = 2;
		AbilityTarget abilityTarget;
		if (currentTargetIndex == 0)
		{
			abilityTarget = currentTarget;
		}
		else
		{
			abilityTarget = targets[0];
		}
		AbilityTarget abilityTarget2 = abilityTarget;
		Vector3 vector = targetingActor.GetSquareWorldPositionForLoS(Board.Get().GetBoardSquareSafe(abilityTarget2.GridPos));
		Vector3 vector2;
		if (this.m_aimBackward)
		{
			vector2 = targetingActor.GetTravelBoardSquareWorldPositionForLos() - vector;
		}
		else
		{
			vector2 = vector - targetingActor.GetTravelBoardSquareWorldPositionForLos();
		}
		if (vector2.magnitude > 0.5f)
		{
			gameObject.SetActive(true);
			gameObject2.SetActive(true);
			float num2 = VectorUtils.HorizontalAngle_Deg(vector2);
			float angle = num2 + this.m_maxAngleForLaser;
			float angle2 = num2 - this.m_maxAngleForLaser;
			vector -= vector2.normalized * 0.5f * this.m_laserWidth * Board.Get().squareSize;
			vector.y = HighlightUtils.GetHighlightHeight();
			gameObject.transform.position = vector;
			gameObject2.transform.position = vector;
			gameObject.transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(angle));
			gameObject2.transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(angle2));
		}
		else
		{
			gameObject.SetActive(false);
			gameObject2.SetActive(false);
		}
		if (currentTargetIndex > 0)
		{
			float laserRange = this.m_laserRange;
			float widthInWorld = this.m_laserWidth * Board.Get().squareSize;
			Vector3 vector3 = this.getClampedLaserDirection(targets[0], currentTarget, vector2);
			vector3.y = 0f;
			if (vector3 != Vector3.zero)
			{
				vector3.Normalize();
			}
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(targets[0].GridPos);
			VectorUtils.LaserCoords laserCoords;
			laserCoords.start = targetingActor.GetSquareWorldPositionForLoS(boardSquareSafe);
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, vector3, laserRange, this.m_laserWidth, targetingActor, base.GetAffectedTeams(), false, this.m_maxTargets, false, false, out laserCoords.end, null, null, false, true);
			using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actor = enumerator.Current;
					base.AddActorInRange(actor, laserCoords.start, targetingActor, AbilityTooltipSubject.Primary, false);
				}
			}
			bool flag = false;
			if (this.m_highlights.Count < num + 2)
			{
				this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
				this.m_highlights.Add(HighlightUtils.Get().CreateConeCursor(this.m_aoeRadius * Board.Get().squareSize, 360f));
				flag = true;
			}
			GameObject gameObject3 = this.m_highlights[num];
			GameObject gameObject4 = this.m_highlights[num + 1];
			Vector3 end = laserCoords.end;
			Vector3 vector4 = end;
			if (!this.m_aoePenetratesLoS)
			{
				vector4 = AbilityCommon_LaserWithCone.GetConeLosCheckPos(laserCoords.start, laserCoords.end);
			}
			List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(end, 0f, 360f, this.m_aoeRadius, 0f, this.m_aoePenetratesLoS, targetingActor, targetingActor.GetOpposingTeam(), null, true, vector4);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
			using (List<ActorData>.Enumerator enumerator2 = actorsInCone.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actorData = enumerator2.Current;
					if (!actorsInLaser.Contains(actorData))
					{
						base.AddActorInRange(actorData, end, targetingActor, AbilityTooltipSubject.Secondary, true);
					}
				}
			}
			Vector3 position = end;
			if (!flag)
			{
				position = TargeterUtils.MoveHighlightTowards(end, gameObject4, ref this.m_cursorSpeed);
			}
			position.y = (float)Board.Get().BaselineHeight + 0.1f;
			gameObject4.transform.position = position;
			gameObject4.SetActive(this.m_aoeRadius > 0f);
			float magnitude = (laserCoords.end - laserCoords.start).magnitude;
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, gameObject3);
			gameObject3.transform.position = laserCoords.start + new Vector3(0f, 0.1f - BoardSquare.s_LoSHeightOffset, 0f);
			if (vector3 != Vector3.zero)
			{
				gameObject3.transform.rotation = Quaternion.LookRotation(vector3);
			}
			if (GameFlowData.Get().activeOwnedActorData == targetingActor)
			{
				SquareInsideChecker_Box squareInsideChecker_Box = this.m_squarePosCheckerList[0] as SquareInsideChecker_Box;
				SquareInsideChecker_Cone squareInsideChecker_Cone = this.m_squarePosCheckerList[1] as SquareInsideChecker_Cone;
				squareInsideChecker_Box.UpdateBoxProperties(laserCoords.start, laserCoords.end, targetingActor);
				squareInsideChecker_Cone.UpdateConeProperties(end, 360f, this.m_aoeRadius, 0f, 0f, targetingActor);
				if (!this.m_aoePenetratesLoS)
				{
					squareInsideChecker_Cone.SetLosPosOverride(true, vector4, true);
				}
				base.ResetSquareIndicatorIndexToUse();
				AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, laserCoords.start, laserCoords.end, this.m_laserWidth, targetingActor, false, null, this.m_squarePosCheckerList, true);
				AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, end, 0f, 360f, this.m_aoeRadius, 0f, targetingActor, this.m_aoePenetratesLoS, this.m_squarePosCheckerList);
				base.HideUnusedSquareIndicators();
			}
		}
		if (currentTargetIndex == 0)
		{
			BoardSquare boardSquareSafe2;
			if (currentTargetIndex == 0)
			{
				boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
			}
			else
			{
				boardSquareSafe2 = Board.Get().GetBoardSquareSafe(targets[0].GridPos);
			}
			if (boardSquareSafe2 != null)
			{
				BoardSquarePathInfo boardSquarePathInfo;
				if (this.m_teleportPath)
				{
					boardSquarePathInfo = new BoardSquarePathInfo();
					boardSquarePathInfo.square = targetingActor.GetCurrentBoardSquare();
					boardSquarePathInfo.next = new BoardSquarePathInfo();
					boardSquarePathInfo.next.prev = boardSquarePathInfo;
					boardSquarePathInfo.next.square = boardSquareSafe2;
				}
				else
				{
					boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe2, targetingActor.GetCurrentBoardSquare(), this.AllowChargeThroughInvalidSquares);
				}
				base.EnableAllMovementArrows();
				int fromIndex = base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
				base.SetMovementArrowEnabledFromIndex(fromIndex, false);
			}
		}
		if (this.m_affectsTargetingActor)
		{
			base.AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self, false);
		}
	}

	public delegate Vector3 GetClampedLaserDirection(AbilityTarget dashTarget, AbilityTarget laserTarget, Vector3 neutralDir);
}
