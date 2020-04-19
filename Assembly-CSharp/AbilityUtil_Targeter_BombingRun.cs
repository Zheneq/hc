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
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		base.ClearActorsInRange();
		if (boardSquare == null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BombingRun.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			this.DisableHighlightCursors();
			return;
		}
		BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare);
		base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
		if (this.m_highlights != null)
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
			if (this.m_highlights.Count >= this.m_maxExplosions)
			{
				goto IL_DC;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
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
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					Vector3 freePos = boardSquarePathInfo2.square.ToVector3();
					Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_explosionShape, freePos, boardSquarePathInfo2.square);
					List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_explosionShape, freePos, boardSquarePathInfo2.square, false, targetingActor, targetingActor.\u0015(), null);
					TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
					using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData actor = enumerator.Current;
							base.AddActorInRange(actor, centerOfShape, targetingActor, AbilityTooltipSubject.Primary, false);
						}
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					centerOfShape.y = (float)Board.\u000E().BaselineHeight + 0.1f;
					this.m_highlights[num2].transform.position = centerOfShape;
					this.m_highlights[num2].SetActive(true);
					num2++;
				}
				boardSquarePathInfo2 = boardSquarePathInfo2.next;
				num++;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int j = num2; j < this.m_maxExplosions; j++)
			{
				this.m_highlights[j].SetActive(false);
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		else
		{
			Vector3 freePos2 = targetingActor.\u0016();
			BoardSquare centerSquare = targetingActor.\u0012();
			Vector3 centerOfShape2 = AreaEffectUtils.GetCenterOfShape(this.m_explosionShape, freePos2, centerSquare);
			List<ActorData> actorsInShape2 = AreaEffectUtils.GetActorsInShape(this.m_explosionShape, freePos2, centerSquare, false, targetingActor, targetingActor.\u0012(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape2);
			foreach (ActorData actor2 in actorsInShape2)
			{
				base.AddActorInRange(actor2, centerOfShape2, targetingActor, AbilityTooltipSubject.Primary, false);
			}
			centerOfShape2.y = (float)Board.\u000E().BaselineHeight + 0.1f;
			this.m_highlights[0].transform.position = centerOfShape2;
			this.m_highlights[0].SetActive(true);
			for (int k = 1; k < this.m_maxExplosions; k++)
			{
				this.m_highlights[k].SetActive(false);
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		base.ClearActorsInRange();
		if (boardSquare == null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BombingRun.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
			}
			this.DisableHighlightCursors();
			return;
		}
		int num = 1;
		if (currentTargetIndex == 0)
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
			num = 2;
		}
		if (this.m_highlights != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_highlights.Count == num)
			{
				goto IL_CF;
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
		this.m_highlights = new List<GameObject>();
		for (int i = 0; i < num; i++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_explosionShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		IL_CF:
		if (currentTargetIndex == 0)
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
			Vector3 freePos = targetingActor.\u0016();
			BoardSquare centerSquare = targetingActor.\u0012();
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_explosionShape, freePos, centerSquare);
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_explosionShape, freePos, centerSquare, false, targetingActor, targetingActor.\u0012(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
			using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actor = enumerator.Current;
					base.AddActorInRange(actor, centerOfShape, targetingActor, AbilityTooltipSubject.Primary, false);
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			centerOfShape.y = (float)Board.\u000E().BaselineHeight + 0.1f;
			this.m_highlights[0].transform.position = centerOfShape;
			this.m_highlights[0].SetActive(true);
		}
		BoardSquarePathInfo boardSquarePathInfo;
		if (currentTargetIndex == 0)
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
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare);
		}
		else
		{
			BoardSquare startSquare = Board.\u000E().\u000E(targets[currentTargetIndex - 1].GridPos);
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare, startSquare, false);
		}
		int num2 = 0;
		base.EnableAllMovementArrows();
		if (boardSquarePathInfo != null)
		{
			num2 = base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, num2, false);
			Vector3 freePos2 = boardSquare.ToVector3();
			BoardSquare centerSquare2 = boardSquare;
			Vector3 centerOfShape2 = AreaEffectUtils.GetCenterOfShape(this.m_explosionShape, freePos2, centerSquare2);
			List<ActorData> actorsInShape2 = AreaEffectUtils.GetActorsInShape(this.m_explosionShape, centerOfShape2, centerSquare2, false, targetingActor, targetingActor.\u0012(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape2);
			using (List<ActorData>.Enumerator enumerator2 = actorsInShape2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actor2 = enumerator2.Current;
					base.AddActorInRange(actor2, centerOfShape2, targetingActor, AbilityTooltipSubject.Primary, false);
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			centerOfShape2.y = (float)Board.\u000E().BaselineHeight + 0.1f;
			this.m_highlights[num - 1].transform.position = centerOfShape2;
			this.m_highlights[num - 1].SetActive(true);
		}
		base.SetMovementArrowEnabledFromIndex(num2, false);
	}
}
