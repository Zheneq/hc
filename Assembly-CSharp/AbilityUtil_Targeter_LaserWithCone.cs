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

	public AbilityUtil_Targeter_LaserWithCone(Ability ability, float width, float distance, bool penetrateLoS, bool affectsAllies, float coneWidthAngle, float coneLengthRadiusInSquares, float coneBackwardOffsetInSquares)
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
		int result;
		if (m_clampToCursorPos)
		{
			result = (m_snapToTargetSquareWhenClampRange ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
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

	public virtual void AddTargetedActor(ActorData actor, Vector3 damageOrigin, ActorData targetingActor, AbilityTooltipSubject subjectType = AbilityTooltipSubject.Primary)
	{
		AddActorInRange(actor, damageOrigin, targetingActor, subjectType);
	}

	private void DisableConeHighlights()
	{
		if (m_highlights == null)
		{
			return;
		}
		while (true)
		{
			for (int i = 1; i < m_highlights.Count; i++)
			{
				m_highlights[i].SetActive(false);
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		Vector3 vector;
		if (currentTarget == null)
		{
			vector = targetingActor.transform.forward;
		}
		else
		{
			vector = currentTarget.AimDirection;
		}
		Vector3 vector2 = vector;
		Vector3 b = currentTarget.FreePos;
		BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTarget.GridPos);
		if (SnapToTargetSquare() && boardSquareSafe != null)
		{
			if (boardSquareSafe != targetingActor.GetCurrentBoardSquare())
			{
				vector2 = boardSquareSafe.ToVector3() - targetingActor.GetFreePos();
				vector2.y = 0f;
				vector2.Normalize();
				b = boardSquareSafe.ToVector3();
			}
		}
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetLoSCheckPos();
		float num = GetDistance();
		if (m_clampToCursorPos)
		{
			float num2 = VectorUtils.HorizontalPlaneDistInSquares(targetingActor.GetFreePos(), b);
			if (m_minRangeIfClampToCursor > 0f)
			{
				if (num2 < m_minRangeIfClampToCursor)
				{
					num2 = m_minRangeIfClampToCursor;
				}
			}
			num = Mathf.Min(num2, num);
		}
		VectorUtils.LaserCoords adjustedCoords = default(VectorUtils.LaserCoords);
		adjustedCoords.start = targetingActor.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(adjustedCoords.start, vector2, num, GetWidth(), targetingActor, GetAffectedTeams(), GetPenetrateLoS(), GetLaserMaxTargets(), false, false, out adjustedCoords.end, null);
		bool flag = AreaEffectUtils.LaserHitWorldGeo(num, adjustedCoords, GetPenetrateLoS(), actorsInLaser);
		float widthInWorld = GetWidth() * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 start = adjustedCoords.start;
		Vector3 end = adjustedCoords.end;
		float magnitude = (end - travelBoardSquareWorldPositionForLos).magnitude;
		if (base.Highlight == null)
		{
			base.Highlight = HighlightUtils.Get().CreateRectangularCursor(widthInWorld, magnitude);
		}
		else
		{
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, base.Highlight);
		}
		base.Highlight.transform.position = start + new Vector3(0f, y, 0f);
		base.Highlight.transform.rotation = Quaternion.LookRotation(vector2);
		if (m_addLaserHitActorAsPrimary)
		{
			foreach (ActorData item in actorsInLaser)
			{
				Vector3 vector3;
				if (m_laserIgnoreCover)
				{
					vector3 = item.GetFreePos();
				}
				else
				{
					vector3 = travelBoardSquareWorldPositionForLos;
				}
				Vector3 damageOrigin = vector3;
				AddTargetedActor(item, damageOrigin, targetingActor);
			}
		}
		m_lastLaserEndPos = end;
		Vector3 vector4 = end;
		Vector3 vector5 = vector4;
		float num3 = VectorUtils.HorizontalAngle_Deg(vector2);
		if (m_explodeOnPathEnd)
		{
			goto IL_0336;
		}
		if (flag)
		{
			if (m_explodeOnEnvironmentHit)
			{
				goto IL_0336;
			}
		}
		int num4 = (actorsInLaser.Count > 0) ? 1 : 0;
		goto IL_0337;
		IL_0337:
		bool flag2 = (byte)num4 != 0;
		if (flag2)
		{
			CreateConeHighlights(vector4, num3);
			if (!m_explosionPenetrateLos)
			{
				vector5 = AbilityCommon_LaserWithCone.GetConeLosCheckPos(adjustedCoords.start, vector4);
			}
			List<ActorData> actors = AreaEffectUtils.GetActorsInCone(vector4, num3, GetConeWidthAngle(), GetConeRadius(), m_coneBackwardOffsetInSquares, m_explosionPenetrateLos, targetingActor, null, null, true, vector5);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			using (List<ActorData>.Enumerator enumerator2 = actors.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData current2 = enumerator2.Current;
					if (current2 != null && GetConeAffectsTarget(current2, targetingActor))
					{
						Vector3 vector6;
						if (m_explosionIgnoreCover)
						{
							vector6 = current2.GetFreePos();
						}
						else
						{
							vector6 = vector4;
						}
						Vector3 damageOrigin2 = vector6;
						AddTargetedActor(current2, damageOrigin2, targetingActor, AbilityTooltipSubject.Secondary);
					}
				}
			}
		}
		else
		{
			DisableConeHighlights();
		}
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			m_laserChecker.UpdateBoxProperties(adjustedCoords.start, adjustedCoords.end, targetingActor);
			m_coneChecker.UpdateConeProperties(vector4, GetConeWidthAngle(), GetConeRadius(), m_coneBackwardOffsetInSquares, num3, targetingActor);
			if (!GetPenetrateLoS())
			{
				m_coneChecker.SetLosPosOverride(true, vector5, true);
			}
			ResetSquareIndicatorIndexToUse();
			bool flag3 = GetWidth() > 0f;
			if (flag3)
			{
				OperationOnSquare_TurnOnHiddenSquareIndicator indicatorHandler = m_indicatorHandler;
				Vector3 start2 = adjustedCoords.start;
				Vector3 end2 = adjustedCoords.end;
				float width = GetWidth();
				bool penetrateLoS = GetPenetrateLoS();
				object losCheckOverrides;
				if (flag2)
				{
					losCheckOverrides = m_squarePosCheckerList;
				}
				else
				{
					losCheckOverrides = null;
				}
				AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(indicatorHandler, start2, end2, width, targetingActor, penetrateLoS, null, (List<ISquareInsideChecker>)losCheckOverrides);
			}
			if (flag2)
			{
				OperationOnSquare_TurnOnHiddenSquareIndicator indicatorHandler2 = m_indicatorHandler;
				float coneWidthAngle = GetConeWidthAngle();
				float coneRadius = GetConeRadius();
				float coneBackwardOffsetInSquares = m_coneBackwardOffsetInSquares;
				bool penetrateLoS2 = GetPenetrateLoS();
				List<ISquareInsideChecker> losCheckOverrides2;
				if (flag3)
				{
					losCheckOverrides2 = m_squarePosCheckerList;
				}
				else
				{
					losCheckOverrides2 = m_coneOnlyCheckerList;
				}
				AreaEffectUtils.OperateOnSquaresInCone(indicatorHandler2, vector4, num3, coneWidthAngle, coneRadius, coneBackwardOffsetInSquares, targetingActor, penetrateLoS2, losCheckOverrides2);
			}
			HideUnusedSquareIndicators();
			return;
		}
		IL_0336:
		num4 = 1;
		goto IL_0337;
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
		if (m_highlights.Count != 1)
		{
			return;
		}
		while (true)
		{
			float radiusInWorld = (GetConeRadius() + m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
			GameObject item = HighlightUtils.Get().CreateConeCursor(radiusInWorld, GetConeWidthAngle());
			m_highlights.Add(item);
			return;
		}
	}

	protected override Vector3 GetTargetingArcEndPosition(ActorData targetingActor)
	{
		if (m_highlights != null)
		{
			if (m_highlights.Count > 1)
			{
				if (m_highlights[1] != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return m_highlights[1].transform.position;
						}
					}
				}
			}
		}
		return base.GetTargetingArcEndPosition(targetingActor);
	}
}
