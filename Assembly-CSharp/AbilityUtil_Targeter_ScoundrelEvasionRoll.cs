using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ScoundrelEvasionRoll : AbilityUtil_Targeter
{
	private AbilityGridPattern m_trapwirePattern;

	private bool m_spawnTrapwireInStart;

	private float m_curSpeed;

	public AbilityUtil_Targeter.AffectsActor m_affectsCaster;

	public int m_numNodesInPath;

	public AbilityUtil_Targeter_ScoundrelEvasionRoll(Ability ability, bool spawnTrapwireOnStart, AbilityGridPattern trapwirePattern) : base(ability)
	{
		this.m_spawnTrapwireInStart = spawnTrapwireOnStart;
		this.m_trapwirePattern = trapwirePattern;
		this.m_curSpeed = 0f;
		this.m_cursorType = HighlightUtils.CursorType.MouseOverCursorType;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.ClearActorsInRange();
		this.m_numNodesInPath = 0;
		BoardSquarePathInfo boardSquarePathInfo = null;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		if (boardSquareSafe != null)
		{
			if (currentTargetIndex != 0)
			{
				if (targets != null)
				{
					if (this.IsUsingMultiTargetUpdate())
					{
						goto IL_7B;
					}
				}
			}
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe);
			goto IL_B0;
		}
		IL_7B:
		if (boardSquareSafe != null)
		{
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(targets[currentTargetIndex - 1].GridPos);
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe, boardSquareSafe2, false);
		}
		IL_B0:
		if (boardSquarePathInfo != null)
		{
			for (BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo; boardSquarePathInfo2 != null; boardSquarePathInfo2 = boardSquarePathInfo2.next)
			{
				if (boardSquarePathInfo2.next == null || boardSquarePathInfo2.next.square != boardSquarePathInfo2.square)
				{
					this.m_numNodesInPath++;
				}
			}
		}
		base.EnableAllMovementArrows();
		int fromIndex = base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
		base.SetMovementArrowEnabledFromIndex(fromIndex, false);
		if (this.m_spawnTrapwireInStart && this.m_trapwirePattern != AbilityGridPattern.NoPattern)
		{
			Vector3 trapwireHighlightPos = this.GetTrapwireHighlightPos(currentTarget, targetingActor);
			if (base.Highlight == null)
			{
				base.Highlight = HighlightUtils.Get().CreateGridPatternHighlight(this.m_trapwirePattern, 1f);
				base.Highlight.transform.position = trapwireHighlightPos;
			}
			else
			{
				base.Highlight.transform.position = TargeterUtils.MoveHighlightTowards(trapwireHighlightPos, base.Highlight, ref this.m_curSpeed);
			}
			base.Highlight.SetActive(true);
		}
		if (this.m_affectsCaster != AbilityUtil_Targeter.AffectsActor.Always)
		{
			if (this.m_affectsCaster != AbilityUtil_Targeter.AffectsActor.Possible)
			{
				return;
			}
			if (!boardSquareSafe.IsInBrushRegion())
			{
				return;
			}
		}
		base.AddActorInRange(targetingActor, currentTarget.FreePos, targetingActor, AbilityTooltipSubject.Self, false);
	}

	private Vector3 GetTrapwireHighlightPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
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
		Vector3 centerOfGridPattern = AreaEffectUtils.GetCenterOfGridPattern(this.m_trapwirePattern, freePos, targetingActor.GetCurrentBoardSquare());
		centerOfGridPattern.y = HighlightUtils.GetHighlightHeight();
		return centerOfGridPattern;
	}
}
