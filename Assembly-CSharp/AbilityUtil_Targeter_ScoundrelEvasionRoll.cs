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
		BoardSquarePathInfo boardSquarePathInfo = null;
		BoardSquare currentTargetSquare = Board.Get().GetSquare(currentTarget.GridPos);

		if (currentTargetSquare == null || currentTargetIndex != 0 && targets != null && IsUsingMultiTargetUpdate())
		{
			if (currentTargetSquare != null)
			{
				BoardSquare prevSquare = Board.Get().GetSquare(targets[currentTargetIndex - 1].GridPos);
				boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, currentTargetSquare, prevSquare, false);
			}
		}
		else
		{
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, currentTargetSquare);
		}
		if (boardSquarePathInfo != null)
		{
			for (BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo; boardSquarePathInfo2 != null; boardSquarePathInfo2 = boardSquarePathInfo2.next)
			{
				if (boardSquarePathInfo2.next == null || boardSquarePathInfo2.next.square != boardSquarePathInfo2.square)
				{
					m_numNodesInPath++;
				}
			}
		}
		EnableAllMovementArrows();
		int fromIndex = AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, 0);
		SetMovementArrowEnabledFromIndex(fromIndex, false);
		if (m_spawnTrapwireInStart && m_trapwirePattern != 0)
		{
			Vector3 trapwireHighlightPos = GetTrapwireHighlightPos(currentTarget, targetingActor);
			if (base.Highlight == null)
			{
				base.Highlight = HighlightUtils.Get().CreateGridPatternHighlight(m_trapwirePattern, 1f);
				base.Highlight.transform.position = trapwireHighlightPos;
			}
			else
			{
				base.Highlight.transform.position = TargeterUtils.MoveHighlightTowards(trapwireHighlightPos, base.Highlight, ref m_curSpeed);
			}
			base.Highlight.SetActive(true);
		}
		if (m_affectsCaster != AffectsActor.Always)
		{
			if (m_affectsCaster != AffectsActor.Possible)
			{
				return;
			}
			if (!currentTargetSquare.IsInBrushRegion())
			{
				return;
			}
		}
		AddActorInRange(targetingActor, currentTarget.FreePos, targetingActor, AbilityTooltipSubject.Self);
	}

	private Vector3 GetTrapwireHighlightPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTarget.GridPos);
		Vector3 vector;
		if (boardSquareSafe != null)
		{
			vector = boardSquareSafe.ToVector3();
		}
		else
		{
			vector = currentTarget.FreePos;
		}
		Vector3 freePos = vector;
		Vector3 centerOfGridPattern = AreaEffectUtils.GetCenterOfGridPattern(m_trapwirePattern, freePos, targetingActor.GetCurrentBoardSquare());
		centerOfGridPattern.y = HighlightUtils.GetHighlightHeight();
		return centerOfGridPattern;
	}
}
