using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BombingRun : AbilityUtil_Targeter
{
	private int m_distanceBetweenExplosionsInSquares;

	private AbilityAreaShape m_explosionShape;

	private int m_maxExplosions;

	public AbilityUtil_Targeter_BombingRun(Ability ability, AbilityAreaShape explosionShape, int distanceBetweenExplosionsInSquares) : base(ability)
	{
		this.m_cursorType = HighlightUtils.CursorType.MouseOverCursorType;
		this.m_explosionShape = explosionShape;
		this.m_distanceBetweenExplosionsInSquares = distanceBetweenExplosionsInSquares;
		this.m_maxExplosions = (int)this.m_ability.GetRangeInSquares(0) / this.m_distanceBetweenExplosionsInSquares + 1;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		base.ClearActorsInRange();
		if (boardSquareSafe == null)
		{
			this.DisableHighlightCursors();
			return;
		}
		BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe);
		base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= this.m_maxExplosions)
			{
				goto IL_DC;
			}
		}
		this.m_highlights = new List<GameObject>();
		for (int i = 0; i < this.m_maxExplosions; i++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_explosionShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		IL_DC:
		if (boardSquarePathInfo != null)
		{
			BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
			int num = 0;
			int num2 = 0;
			while (boardSquarePathInfo2 != null)
			{
				if (num % this.m_distanceBetweenExplosionsInSquares == 0)
				{
					Vector3 freePos = boardSquarePathInfo2.square.ToVector3();
					Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_explosionShape, freePos, boardSquarePathInfo2.square);
					List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_explosionShape, freePos, boardSquarePathInfo2.square, false, targetingActor, targetingActor.GetOpposingTeams(), null);
					TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
					using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData actor = enumerator.Current;
							base.AddActorInRange(actor, centerOfShape, targetingActor, AbilityTooltipSubject.Primary, false);
						}
					}
					centerOfShape.y = (float)Board.Get().BaselineHeight + 0.1f;
					this.m_highlights[num2].transform.position = centerOfShape;
					this.m_highlights[num2].SetActive(true);
					num2++;
				}
				boardSquarePathInfo2 = boardSquarePathInfo2.next;
				num++;
			}
			for (int j = num2; j < this.m_maxExplosions; j++)
			{
				this.m_highlights[j].SetActive(false);
			}
		}
		else
		{
			Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
			BoardSquare currentBoardSquare = targetingActor.GetCurrentBoardSquare();
			Vector3 centerOfShape2 = AreaEffectUtils.GetCenterOfShape(this.m_explosionShape, travelBoardSquareWorldPosition, currentBoardSquare);
			List<ActorData> actorsInShape2 = AreaEffectUtils.GetActorsInShape(this.m_explosionShape, travelBoardSquareWorldPosition, currentBoardSquare, false, targetingActor, targetingActor.GetOpposingTeam(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape2);
			foreach (ActorData actor2 in actorsInShape2)
			{
				base.AddActorInRange(actor2, centerOfShape2, targetingActor, AbilityTooltipSubject.Primary, false);
			}
			centerOfShape2.y = (float)Board.Get().BaselineHeight + 0.1f;
			this.m_highlights[0].transform.position = centerOfShape2;
			this.m_highlights[0].SetActive(true);
			for (int k = 1; k < this.m_maxExplosions; k++)
			{
				this.m_highlights[k].SetActive(false);
			}
		}
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		base.ClearActorsInRange();
		if (boardSquareSafe == null)
		{
			this.DisableHighlightCursors();
			return;
		}
		int num = 1;
		if (currentTargetIndex == 0)
		{
			num = 2;
		}
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count == num)
			{
				goto IL_CF;
			}
		}
		this.m_highlights = new List<GameObject>();
		for (int i = 0; i < num; i++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_explosionShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		IL_CF:
		if (currentTargetIndex == 0)
		{
			Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
			BoardSquare currentBoardSquare = targetingActor.GetCurrentBoardSquare();
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_explosionShape, travelBoardSquareWorldPosition, currentBoardSquare);
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_explosionShape, travelBoardSquareWorldPosition, currentBoardSquare, false, targetingActor, targetingActor.GetOpposingTeam(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
			using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actor = enumerator.Current;
					base.AddActorInRange(actor, centerOfShape, targetingActor, AbilityTooltipSubject.Primary, false);
				}
			}
			centerOfShape.y = (float)Board.Get().BaselineHeight + 0.1f;
			this.m_highlights[0].transform.position = centerOfShape;
			this.m_highlights[0].SetActive(true);
		}
		BoardSquarePathInfo boardSquarePathInfo;
		if (currentTargetIndex == 0)
		{
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe);
		}
		else
		{
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(targets[currentTargetIndex - 1].GridPos);
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe, boardSquareSafe2, false);
		}
		int num2 = 0;
		base.EnableAllMovementArrows();
		if (boardSquarePathInfo != null)
		{
			num2 = base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, num2, false);
			Vector3 freePos = boardSquareSafe.ToVector3();
			BoardSquare centerSquare = boardSquareSafe;
			Vector3 centerOfShape2 = AreaEffectUtils.GetCenterOfShape(this.m_explosionShape, freePos, centerSquare);
			List<ActorData> actorsInShape2 = AreaEffectUtils.GetActorsInShape(this.m_explosionShape, centerOfShape2, centerSquare, false, targetingActor, targetingActor.GetOpposingTeam(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape2);
			using (List<ActorData>.Enumerator enumerator2 = actorsInShape2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actor2 = enumerator2.Current;
					base.AddActorInRange(actor2, centerOfShape2, targetingActor, AbilityTooltipSubject.Primary, false);
				}
			}
			centerOfShape2.y = (float)Board.Get().BaselineHeight + 0.1f;
			this.m_highlights[num - 1].transform.position = centerOfShape2;
			this.m_highlights[num - 1].SetActive(true);
		}
		base.SetMovementArrowEnabledFromIndex(num2, false);
	}
}
