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
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		if (boardSquare != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ScoundrelEvasionRoll.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
			}
			if (currentTargetIndex != 0)
			{
				for (;;)
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
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.IsUsingMultiTargetUpdate())
					{
						goto IL_7B;
					}
					for (;;)
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
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare);
			goto IL_B0;
		}
		IL_7B:
		if (boardSquare != null)
		{
			BoardSquare startSquare = Board.\u000E().\u000E(targets[currentTargetIndex - 1].GridPos);
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare, startSquare, false);
		}
		IL_B0:
		if (boardSquarePathInfo != null)
		{
			for (;;)
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
					this.m_numNodesInPath++;
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		base.EnableAllMovementArrows();
		int fromIndex = base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
		base.SetMovementArrowEnabledFromIndex(fromIndex, false);
		if (this.m_spawnTrapwireInStart && this.m_trapwirePattern != AbilityGridPattern.NoPattern)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			Vector3 trapwireHighlightPos = this.GetTrapwireHighlightPos(currentTarget, targetingActor);
			if (base.Highlight == null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_affectsCaster != AbilityUtil_Targeter.AffectsActor.Possible)
			{
				return;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!boardSquare.\u0012())
			{
				return;
			}
		}
		base.AddActorInRange(targetingActor, currentTarget.FreePos, targetingActor, AbilityTooltipSubject.Self, false);
	}

	private Vector3 GetTrapwireHighlightPos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		Vector3 vector;
		if (boardSquare != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ScoundrelEvasionRoll.GetTrapwireHighlightPos(AbilityTarget, ActorData)).MethodHandle;
			}
			vector = boardSquare.ToVector3();
		}
		else
		{
			vector = currentTarget.FreePos;
		}
		Vector3 freePos = vector;
		Vector3 centerOfGridPattern = AreaEffectUtils.GetCenterOfGridPattern(this.m_trapwirePattern, freePos, targetingActor.\u0012());
		centerOfGridPattern.y = HighlightUtils.GetHighlightHeight();
		return centerOfGridPattern;
	}
}
