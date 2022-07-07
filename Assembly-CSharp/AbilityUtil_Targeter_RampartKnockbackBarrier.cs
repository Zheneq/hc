// ROGUES
// SERVER

using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_RampartKnockbackBarrier : AbilityUtil_Targeter
{
	private float m_width;
	private float m_laserRange;
	private bool m_lengthIgnoreLos;
	private float m_knockbackDistance;
	private KnockbackType m_knockbackType;
	private bool m_penetrateLos;
	private bool m_snapToBorder;
	private bool m_allowAimAtDiagonals;
	private AbilityTooltipSubject m_enemySubjectType = AbilityTooltipSubject.Primary;
	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_RampartKnockbackBarrier(Ability ability, float width, float laserRange, bool lengthIgnoreLos, float knockbackDistance, KnockbackType knockbackType, bool penetrateLos, bool snapToBorder, bool allowAimAtDiagonals)
		: base(ability)
	{
		m_width = width;
		m_laserRange = laserRange;
		m_lengthIgnoreLos = lengthIgnoreLos;
		m_knockbackDistance = knockbackDistance;
		m_knockbackType = knockbackType;
		m_penetrateLos = penetrateLos;
		m_snapToBorder = snapToBorder;
		m_allowAimAtDiagonals = allowAimAtDiagonals;
		m_affectsEnemies = true;
		m_affectsAllies = false;
		m_affectsTargetingActor = false;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public void SetTooltipSubjectType(AbilityTooltipSubject enemySubjectType)
	{
		m_enemySubjectType = enemySubjectType;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		Vector3 currentTargetPos = currentTarget.FreePos;
		Vector3 dir = currentTargetPos - targetingActor.GetFreePos();
		bool active = false;
		Vector3 firstTargetPos = currentTargetPos;
		BoardSquare firstTargetSquare = null;
		if (m_snapToBorder)
		{
			if (currentTargetIndex > 0)
			{
				firstTargetSquare = Board.Get().GetSquare(targets[currentTargetIndex - 1].GridPos);
			}
			else
			{
				firstTargetSquare = Board.Get().GetSquare(currentTarget.GridPos);
			}
			if (firstTargetSquare != null)
			{
				active = true;
				firstTargetPos = firstTargetSquare.ToVector3();
				dir = VectorUtils.GetDirectionAndOffsetToClosestSide(firstTargetSquare, currentTarget.FreePos, m_allowAimAtDiagonals, out Vector3 offset);
				currentTargetPos = firstTargetPos + offset;
			}
		}
		dir.y = 0f;
		dir.Normalize();
		float widthInWorld = m_width * Board.Get().squareSize;
		float rangeInWorld = m_laserRange * Board.Get().squareSize;
		if (m_highlights == null || m_highlights.Count < 2)
		{
			m_highlights = new List<GameObject>();
			m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, true));
			HighlightUtils.Get().ResizeBoundaryLine(m_width, m_highlights[0]);
			m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(widthInWorld, 1f));
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, rangeInWorld, m_highlights[1]);
		}
		float y = 0.1f;
		Vector3 left = Vector3.Cross(dir, Vector3.up);
		m_highlights[0].transform.position = currentTargetPos - 0.5f * widthInWorld * left + new Vector3(0f, 0.1f, 0f);
		m_highlights[0].transform.rotation = Quaternion.LookRotation(-left);
		Vector3 losCheckPos = targetingActor.GetLoSCheckPos();
		Vector3 start = firstTargetSquare != null ? firstTargetSquare.ToVector3() : losCheckPos;
		start.y = losCheckPos.y;
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = start;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, m_affectsAllies, m_affectsEnemies);
		float laserRangeInSquares = m_laserRange + (m_snapToBorder ? 0.5f : 0f);
		List<ActorData> actorsInFront = AreaEffectUtils.GetActorsInLaser(laserCoords.start, dir, laserRangeInSquares, m_width, targetingActor, relevantTeams, m_penetrateLos, -1, m_lengthIgnoreLos, false, out laserCoords.end, null, null, true);
		if (actorsInFront.Count > 0)
		{
			List<ActorData> actorBehind = AreaEffectUtils.GetActorsInLaser(laserCoords.start, -1f * dir, 2f, m_width, targetingActor, relevantTeams, true, -1, true, true, out Vector3 laserEndPos, null, null, true);
			for (int i = actorsInFront.Count - 1; i >= 0; i--)
			{
				if (actorBehind.Contains(actorsInFront[i]))
				{
					actorsInFront.RemoveAt(i);
				}
			}
		}
		float lengthInWorld = rangeInWorld;
		if (!m_lengthIgnoreLos)
		{
			Vector3 vec = laserCoords.end - currentTargetPos;
			vec.y = 0f;
			lengthInWorld = vec.magnitude;
		}
		m_highlights[1].transform.position = currentTargetPos + new Vector3(0f, y, 0f);
		m_highlights[1].transform.rotation = Quaternion.LookRotation(dir);
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, lengthInWorld, m_highlights[1]);
		int arrowIndex = 0;
		EnableAllMovementArrows();
		foreach (ActorData actor in actorsInFront)
		{
			AddActorInRange(actor, laserCoords.start, targetingActor, m_enemySubjectType);
			BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actor, m_knockbackType, dir, laserCoords.start, m_knockbackDistance);
			arrowIndex = AddMovementArrowWithPrevious(actor, path, TargeterMovementType.Knockback, arrowIndex);
		}
		SetMovementArrowEnabledFromIndex(arrowIndex, false);
		if (m_affectsTargetingActor)
		{
			AddActorInRange(targetingActor, targetingActor.GetFreePos(), targetingActor, AbilityTooltipSubject.Self);
		}
		if (m_snapToBorder)
		{
			if (m_highlights.Count < 3)
			{
				m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
			}
			m_highlights[2].transform.position = firstTargetPos;
			m_highlights[2].SetActive(active);
		}
		if (GameFlowData.Get().activeOwnedActorData == targetingActor)
		{
			ResetSquareIndicatorIndexToUse();
			Vector3 a3 = laserCoords.end - laserCoords.start;
			a3.y = 0f;
			a3.Normalize();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, laserCoords.start + 0.49f * Board.SquareSizeStatic * a3, laserCoords.end, m_width, targetingActor, m_penetrateLos, null, null, false);
			HideUnusedSquareIndicators();
		}
	}
}
