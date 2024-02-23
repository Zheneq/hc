using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_SweepMultiClickCone : AbilityUtil_Targeter
{
	private float m_minAngle;

	private float m_maxAngle;

	public float m_rangeInSquares;

	private float m_coneBackwardOffset;

	private float m_lineWidthInSquares;

	private bool m_penetrateLos;

	private int m_maxTargets;

	protected float m_sweepAngle = 5f;

	public bool ClampBorderLineForSweep = true;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public float sweepAngle
	{
		get
		{
			return m_sweepAngle;
		}
		private set
		{
			m_sweepAngle = value;
		}
	}

	public AbilityUtil_Targeter_SweepMultiClickCone(Ability ability, float minAngle, float maxAngle, float rangeInSquares, float coneBackwardOffset, float lineWidthInSquares, bool penetrateLos, int maxTargets)
		: base(ability)
	{
		m_minAngle = Mathf.Max(0f, minAngle);
		m_maxAngle = maxAngle;
		m_rangeInSquares = rangeInSquares;
		m_coneBackwardOffset = coneBackwardOffset;
		m_lineWidthInSquares = lineWidthInSquares;
		m_penetrateLos = penetrateLos;
		m_maxTargets = maxTargets;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		SetUseMultiTargetUpdate(true);
	}

	public void SetIncludeTeams(bool includeAllies, bool includeEnemies, bool includeSelf = false)
	{
		m_affectsAllies = includeAllies;
		m_affectsEnemies = includeEnemies;
		m_affectsTargetingActor = includeSelf;
	}

	public virtual float GetLineWidth()
	{
		return m_lineWidthInSquares;
	}

	public virtual float GetLineRange()
	{
		return m_rangeInSquares;
	}

	public virtual int GetLineMaxTargets()
	{
		return m_maxTargets;
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> previousTargets)
	{
		ClearActorsInRange();
		Vector3 sweepStartAimDirection = default(Vector3);
		int num;
		if (currentTargetIndex > 0)
		{
			if (previousTargets != null)
			{
				num = ((previousTargets.Count > 0) ? 1 : 0);
				goto IL_0042;
			}
		}
		num = 0;
		goto IL_0042;
		IL_0042:
		bool flag = (byte)num != 0;
		if (flag)
		{
			sweepStartAimDirection = previousTargets[0].AimDirection;
		}
		List<ActorData> actors = UpdateHighlightLine(targetingActor, currentTarget.AimDirection, flag, sweepStartAimDirection);
		if (actors != null)
		{
			if (m_maxTargets > 0)
			{
				TargeterUtils.SortActorsByDistanceToPos(ref actors, targetingActor.GetLoSCheckPos());
				TargeterUtils.LimitActorsToMaxNumber(ref actors, m_maxTargets);
			}
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					AddActorInRange(current, targetingActor.GetFreePos(), targetingActor, AbilityTooltipSubject.Primary, true);
				}
				while (true)
				{
					switch (7)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	public List<ActorData> UpdateHighlightLine(ActorData targetingActor, Vector3 aimDirection, bool useAngleRestrictions, Vector3 sweepStartAimDirection)
	{
		List<ActorData> list = null;
		float squareSize = Board.Get().squareSize;
		float y = 0.1f;
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 1)
			{
				goto IL_00ae;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
		m_highlights[0].transform.position = targetingActor.GetFreePos() + new Vector3(0f, y, 0f);
		goto IL_00ae;
		IL_00ae:
		Vector3 endAimDirection = aimDirection;
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetLoSCheckPos();
		if (useAngleRestrictions)
		{
			Vector3 coneCenterAngle;
			list = GetSweepHitActorsAndAngles(sweepStartAimDirection, ref endAimDirection, targetingActor, out m_sweepAngle, out coneCenterAngle);
			if (m_highlights.Count < 2)
			{
				m_highlights.Add(HighlightUtils.Get().CreateDynamicConeMesh(m_rangeInSquares, m_sweepAngle, true));
				m_highlights[1].SetActive(true);
				m_highlights[1].transform.position = targetingActor.GetFreePos() + new Vector3(0f, y, 0f);
			}
			m_highlights[1].transform.rotation = Quaternion.LookRotation(coneCenterAngle);
			HighlightUtils.Get().AdjustDynamicConeMesh(m_highlights[1], m_rangeInSquares, m_sweepAngle);
			DrawSquareIndicators_ConeSweep(targetingActor, m_sweepAngle, coneCenterAngle);
			Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(travelBoardSquareWorldPositionForLos, endAimDirection, m_rangeInSquares * Board.Get().squareSize, !ClampBorderLineForSweep, targetingActor);
			laserEndPoint.y = travelBoardSquareWorldPositionForLos.y;
			HighlightUtils.Get().ResizeRectangularCursor(GetLineWidth() * squareSize, (laserEndPoint - travelBoardSquareWorldPositionForLos).magnitude, m_highlights[0]);
		}
		else
		{
			Vector3 laserEndPos;
			list = AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, aimDirection, GetLineRange(), GetLineWidth(), targetingActor, GetAffectedTeams(), m_penetrateLos, GetLineMaxTargets(), m_penetrateLos, false, out laserEndPos, null);
			HighlightUtils.Get().ResizeRectangularCursor(GetLineWidth() * squareSize, (laserEndPos - travelBoardSquareWorldPositionForLos).magnitude, m_highlights[0]);
			DrawSquareIndicators_Line(targetingActor, travelBoardSquareWorldPositionForLos, laserEndPos, GetLineWidth(), m_penetrateLos);
		}
		m_highlights[0].transform.rotation = Quaternion.LookRotation(endAimDirection);
		return list;
	}

	public List<ActorData> GetSweepHitActorsAndAngles(Vector3 startAimDirection, ref Vector3 endAimDirection, ActorData caster, out float sweepAngle, out Vector3 coneCenterAngle)
	{
		float num = VectorUtils.HorizontalAngle_Deg(startAimDirection);
		float num2 = num;
		sweepAngle = Vector3.Angle(startAimDirection, endAimDirection);
		if (m_maxAngle > 0f && sweepAngle > m_maxAngle)
		{
			endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, (float)Math.PI / 180f * (sweepAngle - m_maxAngle), 0f);
			sweepAngle = m_maxAngle;
		}
		else if (m_minAngle > 0f)
		{
			if (sweepAngle < m_minAngle)
			{
				if (sweepAngle == 0f)
				{
					Vector3 vector = endAimDirection = new Vector3(0f - endAimDirection.z, endAimDirection.y, endAimDirection.x);
					sweepAngle = 90f;
				}
				endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, (float)Math.PI / 180f * (sweepAngle - m_minAngle), 0f);
				sweepAngle = m_minAngle;
			}
		}
		Vector3 vector2 = Vector3.Cross(startAimDirection, endAimDirection);
		if (vector2.y > 0f)
		{
			num2 -= sweepAngle * 0.5f;
		}
		else
		{
			num2 += sweepAngle * 0.5f;
		}
		coneCenterAngle = Vector3.RotateTowards(startAimDirection, endAimDirection, (float)Math.PI / 180f * (sweepAngle * 0.5f), 0f);
		List<ActorData> actors = AreaEffectUtils.GetActorsInCone(caster.GetFreePos(), num2, sweepAngle, m_rangeInSquares, m_coneBackwardOffset, m_penetrateLos, caster, GetAffectedTeams(caster), null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		return actors;
	}

	protected void DrawSquareIndicators_ConeSweep(ActorData targetingActor, float sweepAngle, Vector3 coneCenterAngle)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			ResetSquareIndicatorIndexToUse();
			Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetLoSCheckPos();
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(coneCenterAngle);
			AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, sweepAngle, m_rangeInSquares, m_coneBackwardOffset, targetingActor, m_penetrateLos);
			HideUnusedSquareIndicators();
			return;
		}
	}

	protected void DrawSquareIndicators_Line(ActorData targetingActor, Vector3 startPos, Vector3 endPos, float widthInSquares, bool ignoreLos)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, startPos, endPos, widthInSquares, targetingActor, ignoreLos);
			HideUnusedSquareIndicators();
			return;
		}
	}
}
