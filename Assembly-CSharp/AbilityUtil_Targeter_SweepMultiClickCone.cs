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

	public AbilityUtil_Targeter_SweepMultiClickCone(Ability ability, float minAngle, float maxAngle, float rangeInSquares, float coneBackwardOffset, float lineWidthInSquares, bool penetrateLos, int maxTargets) : base(ability)
	{
		this.m_minAngle = Mathf.Max(0f, minAngle);
		this.m_maxAngle = maxAngle;
		this.m_rangeInSquares = rangeInSquares;
		this.m_coneBackwardOffset = coneBackwardOffset;
		this.m_lineWidthInSquares = lineWidthInSquares;
		this.m_penetrateLos = penetrateLos;
		this.m_maxTargets = maxTargets;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		base.SetUseMultiTargetUpdate(true);
	}

	public float sweepAngle
	{
		get
		{
			return this.m_sweepAngle;
		}
		private set
		{
			this.m_sweepAngle = value;
		}
	}

	public void SetIncludeTeams(bool includeAllies, bool includeEnemies, bool includeSelf = false)
	{
		this.m_affectsAllies = includeAllies;
		this.m_affectsEnemies = includeEnemies;
		this.m_affectsTargetingActor = includeSelf;
	}

	public virtual float GetLineWidth()
	{
		return this.m_lineWidthInSquares;
	}

	public virtual float GetLineRange()
	{
		return this.m_rangeInSquares;
	}

	public virtual int GetLineMaxTargets()
	{
		return this.m_maxTargets;
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> previousTargets)
	{
		base.ClearActorsInRange();
		Vector3 sweepStartAimDirection = default(Vector3);
		bool flag;
		if (currentTargetIndex > 0)
		{
			if (previousTargets != null)
			{
				flag = (previousTargets.Count > 0);
				goto IL_42;
			}
		}
		flag = false;
		IL_42:
		bool flag2 = flag;
		if (flag2)
		{
			sweepStartAimDirection = previousTargets[0].AimDirection;
		}
		List<ActorData> list = this.UpdateHighlightLine(targetingActor, currentTarget.AimDirection, flag2, sweepStartAimDirection);
		if (list != null)
		{
			if (this.m_maxTargets > 0)
			{
				TargeterUtils.SortActorsByDistanceToPos(ref list, targetingActor.GetTravelBoardSquareWorldPositionForLos());
				TargeterUtils.LimitActorsToMaxNumber(ref list, this.m_maxTargets);
			}
			using (List<ActorData>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actor = enumerator.Current;
					base.AddActorInRange(actor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Primary, true);
				}
			}
		}
	}

	public List<ActorData> UpdateHighlightLine(ActorData targetingActor, Vector3 aimDirection, bool useAngleRestrictions, Vector3 sweepStartAimDirection)
	{
		float squareSize = Board.Get().squareSize;
		float y = 0.1f;
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 1)
			{
				goto IL_AE;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
		this.m_highlights[0].transform.position = targetingActor.GetTravelBoardSquareWorldPosition() + new Vector3(0f, y, 0f);
		IL_AE:
		Vector3 vector = aimDirection;
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		List<ActorData> result;
		if (useAngleRestrictions)
		{
			Vector3 vector2;
			result = this.GetSweepHitActorsAndAngles(sweepStartAimDirection, ref vector, targetingActor, out this.m_sweepAngle, out vector2);
			if (this.m_highlights.Count < 2)
			{
				this.m_highlights.Add(HighlightUtils.Get().CreateDynamicConeMesh(this.m_rangeInSquares, this.m_sweepAngle, true, null));
				this.m_highlights[1].SetActive(true);
				this.m_highlights[1].transform.position = targetingActor.GetTravelBoardSquareWorldPosition() + new Vector3(0f, y, 0f);
			}
			this.m_highlights[1].transform.rotation = Quaternion.LookRotation(vector2);
			HighlightUtils.Get().AdjustDynamicConeMesh(this.m_highlights[1], this.m_rangeInSquares, this.m_sweepAngle);
			this.DrawSquareIndicators_ConeSweep(targetingActor, this.m_sweepAngle, vector2);
			Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(travelBoardSquareWorldPositionForLos, vector, this.m_rangeInSquares * Board.Get().squareSize, !this.ClampBorderLineForSweep, targetingActor, null, true);
			laserEndPoint.y = travelBoardSquareWorldPositionForLos.y;
			HighlightUtils.Get().ResizeRectangularCursor(this.GetLineWidth() * squareSize, (laserEndPoint - travelBoardSquareWorldPositionForLos).magnitude, this.m_highlights[0]);
		}
		else
		{
			Vector3 vector3;
			result = AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, aimDirection, this.GetLineRange(), this.GetLineWidth(), targetingActor, base.GetAffectedTeams(), this.m_penetrateLos, this.GetLineMaxTargets(), this.m_penetrateLos, false, out vector3, null, null, false, true);
			HighlightUtils.Get().ResizeRectangularCursor(this.GetLineWidth() * squareSize, (vector3 - travelBoardSquareWorldPositionForLos).magnitude, this.m_highlights[0]);
			this.DrawSquareIndicators_Line(targetingActor, travelBoardSquareWorldPositionForLos, vector3, this.GetLineWidth(), this.m_penetrateLos);
		}
		this.m_highlights[0].transform.rotation = Quaternion.LookRotation(vector);
		return result;
	}

	public unsafe List<ActorData> GetSweepHitActorsAndAngles(Vector3 startAimDirection, ref Vector3 endAimDirection, ActorData caster, out float sweepAngle, out Vector3 coneCenterAngle)
	{
		float num = VectorUtils.HorizontalAngle_Deg(startAimDirection);
		float num2 = num;
		sweepAngle = Vector3.Angle(startAimDirection, endAimDirection);
		if (this.m_maxAngle > 0f && sweepAngle > this.m_maxAngle)
		{
			endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, 0.0174532924f * (sweepAngle - this.m_maxAngle), 0f);
			sweepAngle = this.m_maxAngle;
		}
		else if (this.m_minAngle > 0f)
		{
			if (sweepAngle < this.m_minAngle)
			{
				if (sweepAngle == 0f)
				{
					Vector3 vector = new Vector3(-endAimDirection.z, endAimDirection.y, endAimDirection.x);
					endAimDirection = vector;
					sweepAngle = 90f;
				}
				endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, 0.0174532924f * (sweepAngle - this.m_minAngle), 0f);
				sweepAngle = this.m_minAngle;
			}
		}
		if (Vector3.Cross(startAimDirection, endAimDirection).y > 0f)
		{
			num2 -= sweepAngle * 0.5f;
		}
		else
		{
			num2 += sweepAngle * 0.5f;
		}
		coneCenterAngle = Vector3.RotateTowards(startAimDirection, endAimDirection, 0.0174532924f * (sweepAngle * 0.5f), 0f);
		List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(caster.GetTravelBoardSquareWorldPosition(), num2, sweepAngle, this.m_rangeInSquares, this.m_coneBackwardOffset, this.m_penetrateLos, caster, base.GetAffectedTeams(caster), null, false, default(Vector3));
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
		return actorsInCone;
	}

	protected void DrawSquareIndicators_ConeSweep(ActorData targetingActor, float sweepAngle, Vector3 coneCenterAngle)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(coneCenterAngle);
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, sweepAngle, this.m_rangeInSquares, this.m_coneBackwardOffset, targetingActor, this.m_penetrateLos, null);
			base.HideUnusedSquareIndicators();
		}
	}

	protected void DrawSquareIndicators_Line(ActorData targetingActor, Vector3 startPos, Vector3 endPos, float widthInSquares, bool ignoreLos)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, startPos, endPos, widthInSquares, targetingActor, ignoreLos, null, null, true);
			base.HideUnusedSquareIndicators();
		}
	}
}
