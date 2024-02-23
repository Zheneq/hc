using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_LaserWithShape : AbilityUtil_Targeter
{
	public struct HitActorContext
	{
		public ActorData actor;
		public int hitOrderIndex;
		public float squaresFromCaster;
	}

	private LaserTargetingInfo m_laserInfo;
	private AbilityAreaShape m_shape;
	private float m_cursorSpeed;

	public bool m_explodeOnEndOfPath;
	public bool m_explodeOnEnvironmentHit;
	public bool m_explodeIfHitActor = true;
	public bool m_clampToCursorPos;
	public bool m_snapToTargetShapeCenterWhenClampRange;
	public bool m_snapToTargetSquareWhenClampRange;
	public bool m_addLaserHitActorAsPrimary = true;

	private List<ActorData> m_lastLaserHitActors = new List<ActorData>();

	public AbilityUtil_Targeter_LaserWithShape(Ability ability, LaserTargetingInfo laserInfo, AbilityAreaShape shape)
		: base(ability)
	{
		m_laserInfo = laserInfo;
		m_shape = shape;
		SetAffectedGroups(m_laserInfo.affectsEnemies, m_laserInfo.affectsAllies, m_laserInfo.affectsCaster);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public AbilityUtil_Targeter_LaserWithShape(
		Ability ability,
		AbilityAreaShape shape,
		float width,
		float distance,
		bool penetrateLos,
		int maxTargets,
		bool affectsAllies = false,
		bool affectsCaster = false,
		bool affectsEnemies = true)
		: base(ability)
	{
		m_shape = shape;
		m_laserInfo = new LaserTargetingInfo
		{
			width = width,
			range = distance,
			penetrateLos = penetrateLos,
			maxTargets = maxTargets,
			affectsAllies = affectsAllies,
			affectsCaster = affectsCaster,
			affectsEnemies = affectsEnemies
		};
		SetAffectedGroups(affectsEnemies, affectsAllies, affectsCaster);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public List<ActorData> GetLastLaserHitActors()
	{
		return m_lastLaserHitActors;
	}

	public void SetClampToCursorPos(bool value)
	{
		m_clampToCursorPos = value;
	}

	public void SetSnapToTargetShapeCenterWhenClampRange(bool value)
	{
		m_snapToTargetShapeCenterWhenClampRange = value;
	}

	public void SetSnapToTargetSquareWhenClampRange(bool value)
	{
		m_snapToTargetSquareWhenClampRange = value;
	}

	public void SetExplodeOnEnvironmentHit(bool value)
	{
		m_explodeOnEnvironmentHit = value;
	}

	public void SetExplodeOnPathEnd(bool value)
	{
		m_explodeOnEndOfPath = value;
	}

	public void SetExplodeIfHitActor(bool value)
	{
		m_explodeIfHitActor = value;
	}

	public void SetAddDirectHitActorAsPrimary(bool value)
	{
		m_addLaserHitActorAsPrimary = value;
	}

	public bool SnapAimDirection()
	{
		return SnapToTargetSquare() || SnapToTargetShapeCenter();
	}

	public bool SnapToTargetSquare()
	{
		return m_clampToCursorPos
		       && m_snapToTargetSquareWhenClampRange
		       && !m_snapToTargetShapeCenterWhenClampRange;
	}

	public bool SnapToTargetShapeCenter()
	{
		return m_clampToCursorPos && m_snapToTargetShapeCenterWhenClampRange;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		Vector3 aimDirection = currentTarget != null ? currentTarget.AimDirection : targetingActor.transform.forward;
		Vector3 targetPos = currentTarget.FreePos;
		BoardSquare targetSquare = Board.Get().GetSquare(currentTarget.GridPos);
		if (SnapAimDirection()
		    && targetSquare != null
		    && targetSquare != targetingActor.GetCurrentBoardSquare())
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, targetSquare.ToVector3(), targetSquare);
			Vector3 snapTargetPos = SnapToTargetShapeCenter() ? centerOfShape : targetSquare.ToVector3();
			aimDirection = snapTargetPos - targetingActor.GetFreePos();
			aimDirection.y = 0f;
			aimDirection.Normalize();
			targetPos = snapTargetPos;
		}
		float distance = m_laserInfo.range;
		if (m_clampToCursorPos)
		{
			float clampedDistance = VectorUtils.HorizontalPlaneDistInSquares(targetingActor.GetFreePos(), targetPos);
			distance = Mathf.Min(clampedDistance, distance);
		}
		float widthInWorld = m_laserInfo.width * Board.Get().squareSize;
		VectorUtils.LaserCoords adjustedCoords = default(VectorUtils.LaserCoords);
		adjustedCoords.start = targetingActor.GetLoSCheckPos();
		List<ActorData> actorsInLaser = m_lastLaserHitActors = AreaEffectUtils.GetActorsInLaser(
			adjustedCoords.start,
			aimDirection, 
			distance,
			m_laserInfo.width,
			targetingActor,
			GetAffectedTeams(),
			m_laserInfo.penetrateLos,
			m_laserInfo.maxTargets,
			false, 
			false,
			out adjustedCoords.end,
			null);
		bool hitEnv = AreaEffectUtils.LaserHitWorldGeo(distance, adjustedCoords, m_laserInfo.penetrateLos, actorsInLaser);
		foreach (ActorData item in actorsInLaser)
		{
			AddActorInRange(item, adjustedCoords.start, targetingActor);
		}
		if (m_laserInfo.affectsCaster)
		{
			AddActorInRange(targetingActor, adjustedCoords.start, targetingActor);
		}
		bool flag2 = false;
		int expectedHighlightNum = SnapAimDirection() ? 3 : 2;
		if (m_highlights == null || m_highlights.Count < expectedHighlightNum)
		{
			m_highlights = new List<GameObject>
			{
				HighlightUtils.Get().CreateRectangularCursor(1f, 1f),
				HighlightUtils.Get().CreateShapeCursor(m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData)
			};
			if (SnapAimDirection())
			{
				m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
			}
			flag2 = true;
		}

		GameObject highlight0 = m_highlights[0];
		GameObject highlight1 = m_highlights[1];
		GameObject highlight2 = null;
		if (SnapAimDirection())
		{
			highlight2 = m_highlights[2];
		}

		if (m_explodeOnEndOfPath
		    || hitEnv && m_explodeOnEnvironmentHit
		    || m_explodeIfHitActor && actorsInLaser.Count > 0)
		{
			Vector3 adjustedEndPoint;
			AreaEffectUtils.GetEndPointForValidGameplaySquare(adjustedCoords.start, adjustedCoords.end, out adjustedEndPoint);
			BoardSquare endPointSquare = Board.Get().GetSquareFromVec3(adjustedEndPoint);
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, adjustedEndPoint, endPointSquare);
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(m_shape, centerOfShape, endPointSquare, false, targetingActor, targetingActor.GetEnemyTeam(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
			foreach (ActorData current2 in actorsInShape)
			{
				if (!actorsInLaser.Contains(current2))
				{
					AddActorInRange(current2, centerOfShape, targetingActor, AbilityTooltipSubject.Secondary, true);
				}
			}
			Vector3 position = centerOfShape;
			if (SnapAimDirection())
			{
				position = centerOfShape;
			}
			else if (!flag2)
			{
				position = TargeterUtils.MoveHighlightTowards(centerOfShape, highlight1, ref m_cursorSpeed);
			}
			position.y = Board.Get().BaselineHeight + 0.1f;
			highlight1.transform.position = position;
			highlight1.SetActive(true);
		}
		else
		{
			highlight1.SetActive(false);
		}
		Vector3 adjustedTargetPos = adjustedCoords.end;
		if (SnapAimDirection() && targetSquare != null)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, targetSquare.ToVector3(), targetSquare);
			float snapAdjustment = SnapAimDirection() ? -0.05f : 0.1f;
			centerOfShape.y = Board.Get().BaselineHeight + snapAdjustment;
			highlight2.transform.position = centerOfShape;
			adjustedTargetPos = SnapToTargetShapeCenter() ? centerOfShape : targetSquare.ToVector3();
			adjustedTargetPos.y = adjustedCoords.start.y;
		}
		float magnitude = (adjustedTargetPos - adjustedCoords.start).magnitude;
		Vector3 normalized = (adjustedTargetPos - adjustedCoords.start).normalized;
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, highlight0);
		highlight0.transform.position = adjustedCoords.start + new Vector3(0f, 0.1f - BoardSquare.s_LoSHeightOffset, 0f);
		highlight0.transform.rotation = Quaternion.LookRotation(normalized);
	}
}
