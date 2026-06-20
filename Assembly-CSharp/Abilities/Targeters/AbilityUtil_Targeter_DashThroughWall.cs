using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_DashThroughWall : AbilityUtil_Targeter
{
	private float m_dashWidthInSquares;
	public float m_dashRangeInSquares;
	private float m_maxWallThicknessInSquares;
	public float m_extraTotalDistanceIfThroughWalls = 1.5f;
	private float m_coneWidth;
	private float m_coneLength;
	private float m_throughWallConeWidth;
	private float m_throughWallConeLength;
	private bool m_directHitIgnoreCover;
	private Color m_normalHighlightColor = Color.green;
	private Color m_throughWallsHighlightColor = Color.yellow;
	private bool m_throughWallConeClampedToWall;
	private bool m_aoeWithMiss;
	private float m_coneBackwardOffset;
	private UIDynamicCone m_coneHighlightMesh;
	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public int LastUpdatePathSquareCount { get; set; }

	public AbilityUtil_Targeter_DashThroughWall(
		Ability ability,
		float dashWidthInSquares,
		float dashRangeInSquares,
		float wallThicknessInSquares,
		float coneWidthDegrees,
		float coneThroughWallsWidthDegrees,
		float coneLengthInSquares,
		float coneThroughWallsLengthInSquares,
		float extraTotalRangeIfThroughWalls,
		float coneBackwardOffset,
		bool directHitIgnoreCover,
		bool throughWallConeClampedToWall,
		bool aoeWithoutDirectHit)
		: base(ability)
	{
		m_dashWidthInSquares = dashWidthInSquares;
		m_dashRangeInSquares = dashRangeInSquares;
		m_maxWallThicknessInSquares = wallThicknessInSquares;
		m_coneWidth = coneWidthDegrees;
		m_throughWallConeWidth = coneThroughWallsWidthDegrees;
		m_coneLength = coneLengthInSquares;
		m_throughWallConeLength = coneThroughWallsLengthInSquares;
		m_directHitIgnoreCover = directHitIgnoreCover;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_extraTotalDistanceIfThroughWalls = extraTotalRangeIfThroughWalls;
		m_coneBackwardOffset = coneBackwardOffset;
		m_throughWallConeClampedToWall = throughWallConeClampedToWall;
		m_aoeWithMiss = aoeWithoutDirectHit;
		MantaDashThroughWall mantaDashThroughWall = ability as MantaDashThroughWall;
		if (mantaDashThroughWall != null)
		{
			m_normalHighlightColor = mantaDashThroughWall.m_normalHighlightColor;
			m_throughWallsHighlightColor = mantaDashThroughWall.m_throughWallsHighlightColor;
		}
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
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
				HighlightUtils.Get().CreateDynamicConeMesh(m_coneWidth, m_coneLength * Board.Get().squareSize, false)
			};
			m_coneHighlightMesh = m_highlights[1].GetComponent<UIDynamicCone>();
		}

		GameObject highlightTargetSquare = m_highlights[0];
		GameObject highlightCone = m_highlights[1];
		Vector3 loSCheckPos = targetingActor.GetLoSCheckPos();
		List<ActorData> chargeHitActors = GetChargeHitActors(
			currentTarget.AimDirection,
			loSCheckPos,
			targetingActor,
			out Vector3 chargeEndPoint,
			out bool traveledFullDistance);
		Vector3 vector = chargeEndPoint - loSCheckPos;
		float magnitude = vector.magnitude;
		vector.Normalize();
		BoardSquare boardSquare = null;
		bool canPenetrateWall = chargeHitActors.Count == 0 && !traveledFullDistance;
		bool useDestSquareAsConeStart = false;
		bool active = false;
		if (!canPenetrateWall && (chargeHitActors.Count > 0 || m_aoeWithMiss))
		{
			Vector3 stepBackPos = chargeEndPoint - vector * Mathf.Min(0.5f, magnitude / 2f);
			BoardSquare destSquare = Board.Get().GetSquareFromVec3(stepBackPos);
			if (chargeHitActors.Count > 0)
			{
				Vector3 damageOrigin = m_directHitIgnoreCover
					? chargeHitActors[0].GetFreePos()
					: targetingActor.GetFreePos();
				AddActorInRange(chargeHitActors[0], damageOrigin, targetingActor);
				BoardSquarePathInfo pathToDesired = KnockbackUtils.BuildStraightLineChargePath(
					targetingActor,
					chargeHitActors[0].GetCurrentBoardSquare(),
					targetingActor.GetCurrentBoardSquare(),
					true);
				destSquare = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(
					targetingActor,
					chargeHitActors[0].GetCurrentBoardSquare(),
					pathToDesired);
			}
			else
			{
				destSquare = KnockbackUtils.GetLastValidBoardSquareInLine(loSCheckPos, stepBackPos, true);
				if (Board.Get().GetSquareFromVec3(stepBackPos) == null)
				{
					useDestSquareAsConeStart = true;
				}
			}

			if (destSquare != null)
			{
				Vector3 coneStartPos = chargeEndPoint - vector.normalized * m_coneBackwardOffset * Board.Get().squareSize;
				if (useDestSquareAsConeStart)
				{
					coneStartPos = destSquare.ToVector3();
				}

				float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vector);
				List<ActorData> aoeHitActors = AreaEffectUtils.GetActorsInCone(
					coneStartPos,
					coneCenterAngleDegrees,
					m_coneWidth,
					m_coneLength,
					0f,
					false,
					targetingActor,
					targetingActor.GetEnemyTeam(),
					null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref aoeHitActors);
				foreach (ActorData hitActor in aoeHitActors)
				{
					AddActorInRange(hitActor, destSquare.ToVector3(), targetingActor, AbilityTooltipSubject.Secondary);
				}

				active = true;
				coneStartPos.y = HighlightUtils.GetHighlightHeight();
				highlightCone.transform.position = coneStartPos;
				highlightCone.transform.rotation = Quaternion.LookRotation(vector);
				if (m_coneHighlightMesh != null)
				{
					m_coneHighlightMesh.AdjustConeMeshVertices(m_coneWidth, m_coneLength * Board.Get().squareSize);
				}

				boardSquare = destSquare;
				DrawInvalidSquareIndicators(targetingActor, coneStartPos, coneCenterAngleDegrees, m_coneLength, m_coneWidth);
			}
		}
		BoardSquare squareBeyondWall = null;
		if (canPenetrateWall)
		{
			float rangeRemaining = m_maxWallThicknessInSquares * Board.Get().squareSize;
			rangeRemaining = Mathf.Min(rangeRemaining, (m_dashRangeInSquares + m_extraTotalDistanceIfThroughWalls) * Board.Get().squareSize - magnitude);
			Vector3 potentialEndPos = chargeEndPoint + vector * rangeRemaining;
			Vector3 coneStartPos = potentialEndPos;
			Vector3 aoeDirection = vector;
			Vector3 perpendicularFromWall = aoeDirection;
			squareBeyondWall = MantaDashThroughWall.GetSquareBeyondWall(
				loSCheckPos,
				potentialEndPos,
				targetingActor,
				rangeRemaining,
				ref coneStartPos,
				ref perpendicularFromWall);
			if (m_throughWallConeClampedToWall)
			{
				aoeDirection = perpendicularFromWall;
			}
			if (squareBeyondWall != null)
			{
				float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aoeDirection);
				List<ActorData> aoeHitActors = AreaEffectUtils.GetActorsInCone(
					coneStartPos,
					coneCenterAngleDegrees,
					m_throughWallConeWidth,
					m_throughWallConeLength,
					0f,
					false,
					targetingActor,
					targetingActor.GetEnemyTeam(),
					null);
				if (aoeHitActors != null)
				{
					TargeterUtils.RemoveActorsInvisibleToClient(ref aoeHitActors);
					foreach (ActorData hitActor in aoeHitActors)
					{
						AddActorInRange(hitActor, squareBeyondWall.ToVector3(), targetingActor, AbilityTooltipSubject.Tertiary);
					}
				}
				active = true;
				coneStartPos.y = HighlightUtils.GetHighlightHeight();
				highlightCone.transform.position = coneStartPos;
				highlightCone.transform.rotation = Quaternion.LookRotation(aoeDirection);
				if (m_coneHighlightMesh != null)
				{
					m_coneHighlightMesh.AdjustConeMeshVertices(m_throughWallConeWidth, m_throughWallConeLength * Board.Get().squareSize);
				}
				SetHighlightColor(highlightTargetSquare, m_throughWallsHighlightColor);
				DrawInvalidSquareIndicators(targetingActor, coneStartPos, coneCenterAngleDegrees, m_throughWallConeLength, m_throughWallConeWidth);
			}
			else
			{
				SetHighlightColor(highlightTargetSquare, m_normalHighlightColor);
			}
		}
		else
		{
			SetHighlightColor(highlightTargetSquare, m_normalHighlightColor);
		}
		highlightCone.SetActive(active);
		if (squareBeyondWall == null)
		{
			float d2 = Mathf.Min(0.5f, magnitude / 2f);
			chargeEndPoint -= vector * d2;
			if (boardSquare != null)
			{
				squareBeyondWall = boardSquare;
			}
			else
			{
				squareBeyondWall = KnockbackUtils.GetLastValidBoardSquareInLine(loSCheckPos, chargeEndPoint, true);
				if (squareBeyondWall == null)
				{
					squareBeyondWall = targetingActor.GetCurrentBoardSquare();
				}
			}
		}
		if (chargeHitActors.Count > 0)
		{
			squareBeyondWall = chargeHitActors[0].GetCurrentBoardSquare();
		}
		BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(
			targetingActor,
			squareBeyondWall,
			targetingActor.GetCurrentBoardSquare(),
			true);
		bool adjustPath = false;
		if (squareBeyondWall != null
		    && squareBeyondWall.OccupantActor != null
		    && squareBeyondWall.OccupantActor != targetingActor
		    && squareBeyondWall.OccupantActor.IsActorVisibleToClient())
		{
			BoardSquare chargeDestination = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(targetingActor, squareBeyondWall, path);
			if (chargeDestination != squareBeyondWall)
			{
				squareBeyondWall = chargeDestination;
				adjustPath = true;
			}
		}
		if (adjustPath)
		{
			path = KnockbackUtils.BuildStraightLineChargePath(
				targetingActor,
				squareBeyondWall,
				targetingActor.GetCurrentBoardSquare(),
				true);
		}
		int arrowIndex = 0;
		EnableAllMovementArrows();
		arrowIndex = AddMovementArrowWithPrevious(targetingActor, path, TargeterMovementType.Movement, arrowIndex);
		SetMovementArrowEnabledFromIndex(arrowIndex, false);
		if (path != null)
		{
			LastUpdatePathSquareCount = path.GetNumSquaresToEnd();
		}
		Vector3 endpointPos = chargeEndPoint;
		if (canPenetrateWall && path != null)
		{
			endpointPos = path.GetPathEndpoint().square.ToVector3();
		}
		if (useDestSquareAsConeStart)
		{
			endpointPos = squareBeyondWall.ToVector3();
		}
		Vector3 lhs = endpointPos - loSCheckPos;
		lhs.y = 0f;
		float d3 = Vector3.Dot(lhs, currentTarget.AimDirection) + 0.5f;
		Vector3 endPos = loSCheckPos + d3 * currentTarget.AimDirection;
		endPos.y = HighlightUtils.GetHighlightHeight();
		HighlightUtils.Get().RotateAndResizeRectangularCursor(highlightTargetSquare, loSCheckPos, endPos, m_dashWidthInSquares);
	}

	private List<ActorData> GetChargeHitActors(
		Vector3 aimDir,
		Vector3 startPos,
		ActorData caster,
		out Vector3 chargeEndPoint,
		out bool traveledFullDistance)
	{
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			startPos,
			aimDir,
			m_dashRangeInSquares,
			m_dashWidthInSquares,
			caster,
			caster.GetEnemyTeamAsList(),
			false,
			1,
			false,
			false,
			out chargeEndPoint,
			null,
			null,
			true);
		float num = (m_dashRangeInSquares - 0.25f) * Board.Get().squareSize;
		traveledFullDistance = (startPos - chargeEndPoint).magnitude >= num;
		return actorsInLaser;
	}

	private void SetHighlightColor(GameObject highlight, Color color)
	{
		Renderer component = highlight.GetComponent<Renderer>();
		if (component != null && component.material != null)
		{
			component.material.SetColor("_TintColor", color);
		}
		foreach (Renderer renderer in highlight.GetComponentsInChildren<Renderer>())
		{
			if (renderer != null && renderer.material != null)
			{
				renderer.material.SetColor("_TintColor", color);
			}
		}
	}

	private void DrawInvalidSquareIndicators(
		ActorData targetingActor,
		Vector3 coneStartPos,
		float forwardDir_degrees,
		float coneLengthSquares,
		float coneWidthDegrees)
	{
		if (targetingActor != GameFlowData.Get().activeOwnedActorData)
		{
			return;
		}
		
		ResetSquareIndicatorIndexToUse();
		AreaEffectUtils.OperateOnSquaresInCone(
			m_indicatorHandler,
			coneStartPos,
			forwardDir_degrees,
			coneWidthDegrees,
			coneLengthSquares,
			0f,
			targetingActor,
			false);
		HideUnusedSquareIndicators();
	}
}
