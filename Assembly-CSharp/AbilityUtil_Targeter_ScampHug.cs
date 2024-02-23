using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ScampHug : AbilityUtil_Targeter
{
	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	private float m_dashWidthInSquares;
	public float m_dashRangeInSquares;

	private AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;
	private bool m_directHitIgnoreCover;

	public IsAffectingCasterDelegate m_affectCasterDelegate;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;
	private ScampHug.TargetingMode m_targetingMode;
	private Scamp_SyncComponent m_syncComp;
	private float m_enemyKnockbackDist;
	private KnockbackType m_enemyKnockbackType;

	public int LastUpdatePathSquareCount { get; set; }

	public AbilityUtil_Targeter_ScampHug(
		Ability ability,
		Scamp_SyncComponent syncComp,
		ScampHug.TargetingMode targetingMode,
		float dashWidthInSquares,
		float dashRangeInSquares,
		AbilityAreaShape aoeShape,
		bool directHitIgnoreCover,
		float enemyKnockbackDist,
		KnockbackType enemyKnockbackType)
		: base(ability)
	{
		m_syncComp = syncComp;
		m_targetingMode = targetingMode;
		m_dashWidthInSquares = dashWidthInSquares;
		m_dashRangeInSquares = dashRangeInSquares;
		m_aoeShape = aoeShape;
		m_directHitIgnoreCover = directHitIgnoreCover;
		m_enemyKnockbackDist = enemyKnockbackDist;
		m_enemyKnockbackType = enemyKnockbackType;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	private bool IsInKnockbackMode()
	{
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		LastUpdatePathSquareCount = 0;
		if (m_highlights == null || m_highlights.Count < 3)
		{
			m_highlights = new List<GameObject>
			{
				HighlightUtils.Get().CreateRectangularCursor(1f, 1f),
				HighlightUtils.Get().CreateShapeCursor(m_aoeShape, targetingActor == GameFlowData.Get().activeOwnedActorData),
				HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData)
			};
		}
		GameObject highlightRect = m_highlights[0];
		GameObject highlightShape = m_highlights[1];
		GameObject highlightSquare = m_highlights[2];
		if (IsInKnockbackMode())
		{
			bool isLaser = m_targetingMode == ScampHug.TargetingMode.Laser;
			ActorData firstHitActor;
			List<ActorData> aoeHitActors;
			BoardSquare knockbackDestSquare;
			ScampHug.GetHitActorsAndKnockbackDestinationStatic(
				currentTarget,
				targetingActor,
				m_targetingMode,
				false,
				m_dashWidthInSquares,
				m_dashRangeInSquares,
				m_aoeShape,
				out firstHitActor,
				out aoeHitActors,
				out knockbackDestSquare);
			bool active = false;
			Vector3 damageOrigin = Vector3.zero;
			if (firstHitActor != null)
			{
				Vector3 firstHitOrigin = m_directHitIgnoreCover ? firstHitActor.GetFreePos() : targetingActor.GetFreePos();
				AddActorInRange(firstHitActor, firstHitOrigin, targetingActor);
				BoardSquare firstTargetSquare = firstHitActor.GetCurrentBoardSquare();
				active = true;
				Vector3 firstTargetPos = firstTargetSquare.ToVector3();
				firstTargetPos.y = HighlightUtils.GetHighlightHeight();
				highlightShape.transform.position = firstTargetPos;
				damageOrigin = firstHitActor.GetCurrentBoardSquare().ToVector3();
			}
			else if (!isLaser)
			{
				active = true;
				Vector3 knockbackDestHighlightPos = knockbackDestSquare.ToVector3();
				knockbackDestHighlightPos.y = HighlightUtils.GetHighlightHeight();
				highlightShape.transform.position = knockbackDestHighlightPos;
				damageOrigin = knockbackDestSquare.ToVector3();
			}
			foreach (ActorData current in aoeHitActors)
			{
				AddActorInRange(current, damageOrigin, targetingActor, AbilityTooltipSubject.Secondary);
			}
			if (m_affectCasterDelegate != null && m_affectCasterDelegate(targetingActor, GetVisibleActorsInRange()))
			{
				AddActorInRange(targetingActor, targetingActor.GetFreePos(), targetingActor, AbilityTooltipSubject.Self);
			}
			highlightShape.SetActive(active);
			Vector3 losCheckPos = targetingActor.GetLoSCheckPos();
			Vector3 knockbackDestPos = knockbackDestSquare.ToVector3();
			Vector3 aimDir = knockbackDestPos - losCheckPos;
			aimDir.y = 0f;
			float distance = aimDir.magnitude / Board.SquareSizeStatic;
			BoardSquarePathInfo knockbackPath = KnockbackUtils.BuildKnockbackPath(
				targetingActor,
				KnockbackType.ForwardAlongAimDir,
				aimDir,
				knockbackDestPos,
				distance);
			int arrowIndex = 0;
			EnableAllMovementArrows();
			arrowIndex = AddMovementArrowWithPrevious(targetingActor, knockbackPath, TargeterMovementType.Knockback, arrowIndex);
			if (m_enemyKnockbackDist > 0f)
			{
				if (firstHitActor != null && !aoeHitActors.Contains(firstHitActor))
				{
					aoeHitActors.Add(firstHitActor);
				}
				foreach (ActorData hitActor in aoeHitActors)
				{
					Vector3 aimDir2 = hitActor.GetFreePos() - knockbackDestPos;
					aimDir.y = 0f;
					if (aimDir.sqrMagnitude > 0f)
					{
						aimDir.Normalize();
					}
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(
						hitActor,
						m_enemyKnockbackType,
						aimDir2,
						knockbackDestPos,
						m_enemyKnockbackDist);
					arrowIndex = AddMovementArrowWithPrevious(hitActor, path, TargeterMovementType.Knockback, arrowIndex);
				}
			}
			SetMovementArrowEnabledFromIndex(arrowIndex, false);
			if (knockbackPath != null)
			{
				LastUpdatePathSquareCount = knockbackPath.GetNumSquaresToEnd();
			}
			if (isLaser)
			{
				Vector3 knockbackDest = knockbackPath != null
					? knockbackPath.GetPathEndpoint().square.ToVector3()
					: knockbackDestPos;
				Vector3 vector = knockbackDest - losCheckPos;
				vector.y = 0f;
				float d = Vector3.Dot(vector, currentTarget.AimDirection) + 0.5f;
				Vector3 endPos = losCheckPos + d * currentTarget.AimDirection;
				endPos.y = HighlightUtils.GetHighlightHeight();
				HighlightUtils.Get().RotateAndResizeRectangularCursor(highlightRect, losCheckPos, endPos, m_dashWidthInSquares);
			}
			else
			{
				highlightSquare.transform.position = highlightShape.transform.position;
			}
			highlightRect.SetActive(isLaser);
			highlightSquare.SetActive(!isLaser);
		}
		else
		{
			highlightRect.SetActive(false);
			highlightShape.SetActive(false);
			BoardSquare currentTargetSquare = Board.Get().GetSquare(currentTarget.GridPos);
			if (currentTargetSquare != null)
			{
				Vector3 currentTargetPos = currentTargetSquare.ToVector3();
				currentTargetPos.y = HighlightUtils.GetHighlightHeight();
				highlightSquare.transform.position = currentTargetPos;
				highlightSquare.SetActive(true);
				BoardSquarePathInfo chargePath = KnockbackUtils.BuildStraightLineChargePath(
					targetingActor,
					currentTargetSquare,
					targetingActor.GetCurrentBoardSquare(),
					false);
				EnableAllMovementArrows();
				int fromIndex = AddMovementArrowWithPrevious(targetingActor, chargePath, TargeterMovementType.Movement, 0);
				SetMovementArrowEnabledFromIndex(fromIndex, false);
			}
			else
			{
				highlightSquare.SetActive(false);
			}
		}
	}
}
