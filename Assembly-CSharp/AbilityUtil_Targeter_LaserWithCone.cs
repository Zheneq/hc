using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_LaserWithCone : AbilityUtil_Targeter
{
	public float m_width = 1f;
	public float m_distance = 15f;
	public float m_coneBackwardOffsetInSquares;
	public bool m_penetrateLoS;

	private float m_coneWidthAngle = 60f;
	protected float m_coneLengthRadiusInSquares = 4f;
	private bool m_explodeOnPathEnd;
	private bool m_explodeOnEnvironmentHit;
	private bool m_clampToCursorPos;
	private bool m_snapToTargetSquareWhenClampRange;
	
	public float m_minRangeIfClampToCursor;

	private bool m_laserIgnoreCover;
	private bool m_explosionIgnoreCover;
	private bool m_explosionPenetrateLos;
	private bool m_addLaserHitActorAsPrimary = true;
	protected int m_maxLaserTargets = 1;
	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;
	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();
	private List<ISquareInsideChecker> m_coneOnlyCheckerList = new List<ISquareInsideChecker>();
	private SquareInsideChecker_Box m_laserChecker;
	private SquareInsideChecker_Cone m_coneChecker;

	public Vector3 m_lastLaserEndPos;

	public AbilityUtil_Targeter_LaserWithCone(
		Ability ability,
		float width,
		float distance,
		bool penetrateLoS,
		bool affectsAllies,
		float coneWidthAngle,
		float coneLengthRadiusInSquares,
		float coneBackwardOffsetInSquares)
		: base(ability)
	{
		m_width = width;
		m_distance = distance;
		m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		m_penetrateLoS = penetrateLoS;
		m_affectsAllies = affectsAllies;
		m_coneWidthAngle = coneWidthAngle;
		m_coneLengthRadiusInSquares = coneLengthRadiusInSquares;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_laserChecker = new SquareInsideChecker_Box(m_width);
		m_coneChecker = new SquareInsideChecker_Cone();
		m_squarePosCheckerList.Add(m_laserChecker);
		m_squarePosCheckerList.Add(m_coneChecker);
		m_coneOnlyCheckerList.Add(m_coneChecker);
	}

	public void SetClampToCursorPos(bool value)
	{
		m_clampToCursorPos = value;
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
		m_explodeOnPathEnd = value;
	}

	public void SetCoverAndLosConfig(bool laserIgnoreCover, bool explosionIgnoreCover, bool explosionPenetrateLos)
	{
		m_laserIgnoreCover = laserIgnoreCover;
		m_explosionIgnoreCover = explosionIgnoreCover;
		m_explosionPenetrateLos = explosionPenetrateLos;
	}

	public void SetMaxLaserTargets(int maxLaserTargets)
	{
		m_maxLaserTargets = maxLaserTargets;
	}

	public void SetAddDirectHitActorAsPrimary(bool value)
	{
		m_addLaserHitActorAsPrimary = value;
	}

	private bool SnapToTargetSquare()
	{
		return m_clampToCursorPos && m_snapToTargetSquareWhenClampRange;
	}

	public virtual float GetWidth()
	{
		return m_width;
	}

	public virtual float GetDistance()
	{
		return m_distance;
	}

	public virtual bool GetPenetrateLoS()
	{
		return m_penetrateLoS;
	}

	public virtual int GetLaserMaxTargets()
	{
		return m_maxLaserTargets;
	}

	public virtual float GetConeRadius()
	{
		return m_coneLengthRadiusInSquares;
	}

	public virtual float GetConeWidthAngle()
	{
		return m_coneWidthAngle;
	}

	public virtual bool GetConeAffectsTarget(ActorData potentialTarget, ActorData targetingActor)
	{
		return GetAffectsTarget(potentialTarget, targetingActor);
	}

	public virtual void AddTargetedActor(
		ActorData actor,
		Vector3 damageOrigin,
		ActorData targetingActor,
		AbilityTooltipSubject subjectType = AbilityTooltipSubject.Primary)
	{
		AddActorInRange(actor, damageOrigin, targetingActor, subjectType);
	}

	private void DisableConeHighlights()
	{
		if (m_highlights != null)
		{
			for (int i = 1; i < m_highlights.Count; i++)
			{
				m_highlights[i].SetActive(false);
			}
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		Vector3 aimDirection = currentTarget != null ? currentTarget.AimDirection : targetingActor.transform.forward;
		Vector3 targetPos = currentTarget.FreePos;
		BoardSquare targetSquare = Board.Get().GetSquare(currentTarget.GridPos);
		if (SnapToTargetSquare()
		    && targetSquare != null
		    && targetSquare != targetingActor.GetCurrentBoardSquare())
		{
			aimDirection = targetSquare.ToVector3() - targetingActor.GetFreePos();
			aimDirection.y = 0f;
			aimDirection.Normalize();
			targetPos = targetSquare.ToVector3();
		}
		Vector3 losCheckPos = targetingActor.GetLoSCheckPos();
		float distance = GetDistance();
		if (m_clampToCursorPos)
		{
			float clampedDistance = VectorUtils.HorizontalPlaneDistInSquares(targetingActor.GetFreePos(), targetPos);
			if (m_minRangeIfClampToCursor > 0f && clampedDistance < m_minRangeIfClampToCursor)
			{
				clampedDistance = m_minRangeIfClampToCursor;
			}
			distance = Mathf.Min(clampedDistance, distance);
		}
		VectorUtils.LaserCoords adjustedCoords = default(VectorUtils.LaserCoords);
		adjustedCoords.start = targetingActor.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			adjustedCoords.start, 
			aimDirection,
			distance,
			GetWidth(),
			targetingActor,
			GetAffectedTeams(),
			GetPenetrateLoS(),
			GetLaserMaxTargets(),
			false,
			false,
			out adjustedCoords.end,
			null);
		bool hitEnv = AreaEffectUtils.LaserHitWorldGeo(distance, adjustedCoords, GetPenetrateLoS(), actorsInLaser);
		float widthInWorld = GetWidth() * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 start = adjustedCoords.start;
		Vector3 end = adjustedCoords.end;
		float magnitude = (end - losCheckPos).magnitude;
		if (Highlight == null)
		{
			Highlight = HighlightUtils.Get().CreateRectangularCursor(widthInWorld, magnitude);
		}
		else
		{
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, Highlight);
		}
		Highlight.transform.position = start + new Vector3(0f, y, 0f);
		Highlight.transform.rotation = Quaternion.LookRotation(aimDirection);
		if (m_addLaserHitActorAsPrimary)
		{
			foreach (ActorData item in actorsInLaser)
			{
				Vector3 damageOrigin = m_laserIgnoreCover ? item.GetFreePos() : losCheckPos;
				AddTargetedActor(item, damageOrigin, targetingActor);
			}
		}
		m_lastLaserEndPos = end;
		Vector3 coneStart = end;
		Vector3 losOverridePos = coneStart;
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDirection);
		bool flag2 = m_explodeOnPathEnd || hitEnv && m_explodeOnEnvironmentHit || actorsInLaser.Count > 0;
		if (flag2)
		{
			CreateConeHighlights(coneStart, coneCenterAngleDegrees);
			if (!m_explosionPenetrateLos)
			{
				losOverridePos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(adjustedCoords.start, coneStart);
			}
			List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(
				coneStart,
				coneCenterAngleDegrees,
				GetConeWidthAngle(),
				GetConeRadius(),
				m_coneBackwardOffsetInSquares,
				m_explosionPenetrateLos,
				targetingActor, 
				null,
				null,
				true,
				losOverridePos);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
			foreach (ActorData actor in actorsInCone)
			{
				if (actor != null && GetConeAffectsTarget(actor, targetingActor))
				{
					Vector3 damageOrigin = m_explosionIgnoreCover ? actor.GetFreePos() : coneStart;
					AddTargetedActor(actor, damageOrigin, targetingActor, AbilityTooltipSubject.Secondary);
				}
			}
		}
		else
		{
			DisableConeHighlights();
		}
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			m_laserChecker.UpdateBoxProperties(adjustedCoords.start, adjustedCoords.end, targetingActor);
			m_coneChecker.UpdateConeProperties(coneStart, GetConeWidthAngle(), GetConeRadius(), m_coneBackwardOffsetInSquares, coneCenterAngleDegrees, targetingActor);
			if (!GetPenetrateLoS())
			{
				m_coneChecker.SetLosPosOverride(true, losOverridePos, true);
			}
			ResetSquareIndicatorIndexToUse();
			bool flag3 = GetWidth() > 0f;
			if (flag3)
			{
				List<ISquareInsideChecker> losCheckOverrides = flag2 ? m_squarePosCheckerList : null;
				AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, adjustedCoords.start, adjustedCoords.end, GetWidth(), targetingActor, GetPenetrateLoS(), null, losCheckOverrides);
			}

			if (flag2)
			{
				List<ISquareInsideChecker> losCheckOverrides = flag3 ? m_squarePosCheckerList : m_coneOnlyCheckerList;
				AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, coneStart, coneCenterAngleDegrees, GetConeWidthAngle(), GetConeRadius(), m_coneBackwardOffsetInSquares, targetingActor, GetPenetrateLoS(), losCheckOverrides);
			}

			HideUnusedSquareIndicators();
		}
	}

	private void CreateConeHighlights(Vector3 coneOrigin, float aimDir_degrees)
	{
		Vector3 vector = VectorUtils.AngleDegreesToVector(aimDir_degrees);
		float d = m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = coneOrigin + new Vector3(0f, y, 0f) - vector * d;
		AllocateConeHighlights();
		for (int i = 1; i < m_highlights.Count; i++)
		{
			m_highlights[i].transform.position = position;
			m_highlights[i].transform.rotation = Quaternion.LookRotation(vector);
			m_highlights[i].gameObject.SetActive(true);
		}
	}

	protected virtual void AllocateConeHighlights()
	{
		if (m_highlights.Count == 1)
		{
			float radiusInWorld = (GetConeRadius() + m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
			GameObject item = HighlightUtils.Get().CreateConeCursor(radiusInWorld, GetConeWidthAngle());
			m_highlights.Add(item);
		}
	}

	protected override Vector3 GetTargetingArcEndPosition(ActorData targetingActor)
	{
		return m_highlights != null
		       && m_highlights.Count > 1
		       && m_highlights[1] != null
			? m_highlights[1].transform.position
			: base.GetTargetingArcEndPosition(targetingActor);
	}
}
