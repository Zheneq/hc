using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_DashAndAim : AbilityUtil_Targeter
{
	public delegate Vector3 GetClampedLaserDirection(AbilityTarget dashTarget, AbilityTarget laserTarget, Vector3 neutralDir);

	private float m_cursorSpeed = 1f;

	private float m_aoeRadius;

	private bool m_aoePenetratesLoS;

	private float m_laserWidth;

	private float m_laserRange;

	private float m_maxAngleForLaser;

	private bool m_aimBackward;

	private bool m_teleportPath;

	private int m_maxTargets;

	private GetClampedLaserDirection getClampedLaserDirection;

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public bool AllowChargeThroughInvalidSquares
	{
		get;
		set;
	}

	public AbilityUtil_Targeter_DashAndAim(Ability ability, float aoeRadius, bool aoePenetratesLoS, float laserWidth, float laserRange, float maxAngleForLaser, GetClampedLaserDirection laserDirDelegate, bool aimBackward, bool teleportPath, int maxTargets)
		: base(ability)
	{
		m_showArcToShape = false;
		AllowChargeThroughInvalidSquares = true;
		m_aoeRadius = aoeRadius;
		m_aoePenetratesLoS = aoePenetratesLoS;
		m_laserWidth = laserWidth;
		m_laserRange = laserRange;
		m_maxAngleForLaser = maxAngleForLaser;
		m_aimBackward = aimBackward;
		m_teleportPath = teleportPath;
		m_maxTargets = maxTargets;
		getClampedLaserDirection = laserDirDelegate;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_squarePosCheckerList.Add(new SquareInsideChecker_Box(m_laserWidth));
		m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (m_highlights.Count < 2)
		{
			return;
		}
		while (true)
		{
			m_highlights[0].SetActive(false);
			m_highlights[1].SetActive(false);
			return;
		}
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		if (m_highlights == null || m_highlights.Count < 2)
		{
			m_highlights = new List<GameObject>();
			m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(m_laserRange + m_aoeRadius, 0.2f, true, Color.cyan));
			m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(m_laserRange + m_aoeRadius, 0.2f, true, Color.cyan));
		}
		GameObject gameObject = m_highlights[0];
		GameObject gameObject2 = m_highlights[1];
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
		Vector3 squareWorldPositionForLoS = targetingActor.GetSquareWorldPositionForLoS(Board.Get().GetSquare(abilityTarget2.GridPos));
		Vector3 vector = (!m_aimBackward) ? (squareWorldPositionForLoS - targetingActor.GetLoSCheckPos()) : (targetingActor.GetLoSCheckPos() - squareWorldPositionForLoS);
		if (vector.magnitude > 0.5f)
		{
			gameObject.SetActive(true);
			gameObject2.SetActive(true);
			float num2 = VectorUtils.HorizontalAngle_Deg(vector);
			float angle = num2 + m_maxAngleForLaser;
			float angle2 = num2 - m_maxAngleForLaser;
			squareWorldPositionForLoS -= vector.normalized * 0.5f * m_laserWidth * Board.Get().squareSize;
			squareWorldPositionForLoS.y = HighlightUtils.GetHighlightHeight();
			gameObject.transform.position = squareWorldPositionForLoS;
			gameObject2.transform.position = squareWorldPositionForLoS;
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
			float laserRange = m_laserRange;
			float widthInWorld = m_laserWidth * Board.Get().squareSize;
			Vector3 vector2 = getClampedLaserDirection(targets[0], currentTarget, vector);
			vector2.y = 0f;
			if (vector2 != Vector3.zero)
			{
				vector2.Normalize();
			}
			BoardSquare boardSquareSafe = Board.Get().GetSquare(targets[0].GridPos);
			VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
			laserCoords.start = targetingActor.GetSquareWorldPositionForLoS(boardSquareSafe);
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, vector2, laserRange, m_laserWidth, targetingActor, GetAffectedTeams(), false, m_maxTargets, false, false, out laserCoords.end, null);
			using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					AddActorInRange(current, laserCoords.start, targetingActor);
				}
			}
			bool flag = false;
			if (m_highlights.Count < num + 2)
			{
				m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
				m_highlights.Add(HighlightUtils.Get().CreateConeCursor(m_aoeRadius * Board.Get().squareSize, 360f));
				flag = true;
			}
			GameObject gameObject3 = m_highlights[num];
			GameObject gameObject4 = m_highlights[num + 1];
			Vector3 end = laserCoords.end;
			Vector3 vector3 = end;
			if (!m_aoePenetratesLoS)
			{
				vector3 = AbilityCommon_LaserWithCone.GetConeLosCheckPos(laserCoords.start, laserCoords.end);
			}
			List<ActorData> actors = AreaEffectUtils.GetActorsInCone(end, 0f, 360f, m_aoeRadius, 0f, m_aoePenetratesLoS, targetingActor, targetingActor.GetEnemyTeam(), null, true, vector3);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			using (List<ActorData>.Enumerator enumerator2 = actors.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData current2 = enumerator2.Current;
					if (!actorsInLaser.Contains(current2))
					{
						AddActorInRange(current2, end, targetingActor, AbilityTooltipSubject.Secondary, true);
					}
				}
			}
			Vector3 position = end;
			if (!flag)
			{
				position = TargeterUtils.MoveHighlightTowards(end, gameObject4, ref m_cursorSpeed);
			}
			position.y = (float)Board.Get().BaselineHeight + 0.1f;
			gameObject4.transform.position = position;
			gameObject4.SetActive(m_aoeRadius > 0f);
			float magnitude = (laserCoords.end - laserCoords.start).magnitude;
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, gameObject3);
			gameObject3.transform.position = laserCoords.start + new Vector3(0f, 0.1f - BoardSquare.s_LoSHeightOffset, 0f);
			if (vector2 != Vector3.zero)
			{
				gameObject3.transform.rotation = Quaternion.LookRotation(vector2);
			}
			if (GameFlowData.Get().activeOwnedActorData == targetingActor)
			{
				SquareInsideChecker_Box squareInsideChecker_Box = m_squarePosCheckerList[0] as SquareInsideChecker_Box;
				SquareInsideChecker_Cone squareInsideChecker_Cone = m_squarePosCheckerList[1] as SquareInsideChecker_Cone;
				squareInsideChecker_Box.UpdateBoxProperties(laserCoords.start, laserCoords.end, targetingActor);
				squareInsideChecker_Cone.UpdateConeProperties(end, 360f, m_aoeRadius, 0f, 0f, targetingActor);
				if (!m_aoePenetratesLoS)
				{
					squareInsideChecker_Cone.SetLosPosOverride(true, vector3, true);
				}
				ResetSquareIndicatorIndexToUse();
				AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, laserCoords.start, laserCoords.end, m_laserWidth, targetingActor, false, null, m_squarePosCheckerList);
				AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, end, 0f, 360f, m_aoeRadius, 0f, targetingActor, m_aoePenetratesLoS, m_squarePosCheckerList);
				HideUnusedSquareIndicators();
			}
		}
		if (currentTargetIndex == 0)
		{
			BoardSquarePathInfo boardSquarePathInfo = null;
			BoardSquare boardSquareSafe2;
			if (currentTargetIndex == 0)
			{
				boardSquareSafe2 = Board.Get().GetSquare(currentTarget.GridPos);
			}
			else
			{
				boardSquareSafe2 = Board.Get().GetSquare(targets[0].GridPos);
			}
			if (boardSquareSafe2 != null)
			{
				if (m_teleportPath)
				{
					boardSquarePathInfo = new BoardSquarePathInfo();
					boardSquarePathInfo.square = targetingActor.GetCurrentBoardSquare();
					boardSquarePathInfo.next = new BoardSquarePathInfo();
					boardSquarePathInfo.next.prev = boardSquarePathInfo;
					boardSquarePathInfo.next.square = boardSquareSafe2;
				}
				else
				{
					boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe2, targetingActor.GetCurrentBoardSquare(), AllowChargeThroughInvalidSquares);
				}
				EnableAllMovementArrows();
				int fromIndex = AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, 0);
				SetMovementArrowEnabledFromIndex(fromIndex, false);
			}
		}
		if (!m_affectsTargetingActor)
		{
			return;
		}
		while (true)
		{
			AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self);
			return;
		}
	}
}
