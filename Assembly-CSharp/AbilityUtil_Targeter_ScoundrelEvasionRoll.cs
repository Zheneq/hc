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
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		if (!(boardSquareSafe != null))
		{
			goto IL_007b;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		if (1 == 0)
		{
			/*OpCode not supported: LdMemberToken*/;
		}
		if (currentTargetIndex != 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (targets != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (IsUsingMultiTargetUpdate())
				{
					goto IL_007b;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe);
		goto IL_00b0;
		IL_00b0:
		if (boardSquarePathInfo != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			for (BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo; boardSquarePathInfo2 != null; boardSquarePathInfo2 = boardSquarePathInfo2.next)
			{
				if (boardSquarePathInfo2.next == null || boardSquarePathInfo2.next.square != boardSquarePathInfo2.square)
				{
					m_numNodesInPath++;
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		EnableAllMovementArrows();
		int fromIndex = AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, 0);
		SetMovementArrowEnabledFromIndex(fromIndex, false);
		if (m_spawnTrapwireInStart && m_trapwirePattern != 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			Vector3 trapwireHighlightPos = GetTrapwireHighlightPos(currentTarget, targetingActor);
			if (base.Highlight == null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_affectsCaster != AffectsActor.Possible)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!boardSquareSafe.IsInBrushRegion())
			{
				return;
			}
		}
		AddActorInRange(targetingActor, currentTarget.FreePos, targetingActor, AbilityTooltipSubject.Self);
		return;
		IL_007b:
		if (boardSquareSafe != null)
		{
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(targets[currentTargetIndex - 1].GridPos);
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe, boardSquareSafe2, false);
		}
		goto IL_00b0;
	}

	private Vector3 GetTrapwireHighlightPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		Vector3 vector;
		if (boardSquareSafe != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
