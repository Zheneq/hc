using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ScoundrelEvasionRoll : AbilityUtil_Targeter
{
	private AbilityGridPattern m_trapwirePattern;
	private bool m_spawnTrapwireInStart;
	private float m_curSpeed;

	public AffectsActor m_affectsCaster;
	public int m_numNodesInPath;

	public AbilityUtil_Targeter_ScoundrelEvasionRoll(Ability ability, bool spawnTrapwireOnStart, AbilityGridPattern trapwirePattern)
		: base(ability)
	{
		m_spawnTrapwireInStart = spawnTrapwireOnStart;
		m_trapwirePattern = trapwirePattern;
		m_curSpeed = 0f;
		m_cursorType = HighlightUtils.CursorType.MouseOverCursorType;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		m_numNodesInPath = 0;
		BoardSquarePathInfo path = null;
		BoardSquare currentTargetSquare = Board.Get().GetSquare(currentTarget.GridPos);

		if (currentTargetSquare != null
			&& (currentTargetIndex == 0 || targets == null || !IsUsingMultiTargetUpdate()))
		{
			path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, currentTargetSquare);
		}
		else if (currentTargetSquare != null)
		{
			BoardSquare prevSquare = Board.Get().GetSquare(targets[currentTargetIndex - 1].GridPos);
			path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, currentTargetSquare, prevSquare, false);
		}
		if (path != null)
		{
			for (BoardSquarePathInfo pathNode = path; pathNode != null; pathNode = pathNode.next)
			{
				if (pathNode.next == null || pathNode.next.square != pathNode.square)
				{
					m_numNodesInPath++;
				}
			}
		}
		EnableAllMovementArrows();
		int fromIndex = AddMovementArrowWithPrevious(targetingActor, path, TargeterMovementType.Movement, 0);
		SetMovementArrowEnabledFromIndex(fromIndex, false);
		if (m_spawnTrapwireInStart && m_trapwirePattern != AbilityGridPattern.NoPattern)
		{
			Vector3 trapwireHighlightPos = GetTrapwireHighlightPos(currentTarget, targetingActor);
			if (Highlight == null)
			{
				Highlight = HighlightUtils.Get().CreateGridPatternHighlight(m_trapwirePattern, 1f);
				Highlight.transform.position = trapwireHighlightPos;
			}
			else
			{
				Highlight.transform.position = TargeterUtils.MoveHighlightTowards(trapwireHighlightPos, Highlight, ref m_curSpeed);
			}
			Highlight.SetActive(true);
		}
		if (m_affectsCaster == AffectsActor.Always
			|| m_affectsCaster == AffectsActor.Possible && currentTargetSquare.IsInBrush())
		{
			AddActorInRange(targetingActor, currentTarget.FreePos, targetingActor, AbilityTooltipSubject.Self);
		}
	}

	private Vector3 GetTrapwireHighlightPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(currentTarget.GridPos);
		Vector3 freePos = targetSquare != null ? targetSquare.ToVector3() : currentTarget.FreePos;
		Vector3 centerOfGridPattern = AreaEffectUtils.GetCenterOfGridPattern(m_trapwirePattern, freePos, targetingActor.GetCurrentBoardSquare());
		centerOfGridPattern.y = HighlightUtils.GetHighlightHeight();
		return centerOfGridPattern;
	}
}
