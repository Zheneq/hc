using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BombingRun : AbilityUtil_Targeter
{
	private int m_distanceBetweenExplosionsInSquares;

	private AbilityAreaShape m_explosionShape;

	private int m_maxExplosions;

	public AbilityUtil_Targeter_BombingRun(Ability ability, AbilityAreaShape explosionShape, int distanceBetweenExplosionsInSquares)
		: base(ability)
	{
		m_cursorType = HighlightUtils.CursorType.MouseOverCursorType;
		m_explosionShape = explosionShape;
		m_distanceBetweenExplosionsInSquares = distanceBetweenExplosionsInSquares;
		m_maxExplosions = (int)m_ability.GetRangeInSquares(0) / m_distanceBetweenExplosionsInSquares + 1;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		ClearActorsInRange();
		if (boardSquareSafe == null)
		{
			while (true)
			{
				DisableHighlightCursors();
				return;
			}
		}
		BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe);
		AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, 0);
		if (m_highlights != null)
		{
			if (m_highlights.Count >= m_maxExplosions)
			{
				goto IL_00dc;
			}
		}
		m_highlights = new List<GameObject>();
		for (int i = 0; i < m_maxExplosions; i++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_explosionShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		goto IL_00dc;
		IL_00dc:
		if (boardSquarePathInfo != null)
		{
			BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
			int num = 0;
			int num2 = 0;
			while (boardSquarePathInfo2 != null)
			{
				if (num % m_distanceBetweenExplosionsInSquares == 0)
				{
					Vector3 freePos = boardSquarePathInfo2.square.ToVector3();
					Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_explosionShape, freePos, boardSquarePathInfo2.square);
					List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_explosionShape, freePos, boardSquarePathInfo2.square, false, targetingActor, targetingActor.GetOpposingTeams(), null);
					TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
					using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData current = enumerator.Current;
							AddActorInRange(current, centerOfShape, targetingActor);
						}
					}
					centerOfShape.y = (float)Board.Get().BaselineHeight + 0.1f;
					m_highlights[num2].transform.position = centerOfShape;
					m_highlights[num2].SetActive(true);
					num2++;
				}
				boardSquarePathInfo2 = boardSquarePathInfo2.next;
				num++;
			}
			while (true)
			{
				for (int j = num2; j < m_maxExplosions; j++)
				{
					m_highlights[j].SetActive(false);
				}
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		BoardSquare currentBoardSquare = targetingActor.GetCurrentBoardSquare();
		Vector3 centerOfShape2 = AreaEffectUtils.GetCenterOfShape(m_explosionShape, travelBoardSquareWorldPosition, currentBoardSquare);
		List<ActorData> actors2 = AreaEffectUtils.GetActorsInShape(m_explosionShape, travelBoardSquareWorldPosition, currentBoardSquare, false, targetingActor, targetingActor.GetOpposingTeam(), null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors2);
		foreach (ActorData item in actors2)
		{
			AddActorInRange(item, centerOfShape2, targetingActor);
		}
		centerOfShape2.y = (float)Board.Get().BaselineHeight + 0.1f;
		m_highlights[0].transform.position = centerOfShape2;
		m_highlights[0].SetActive(true);
		for (int k = 1; k < m_maxExplosions; k++)
		{
			m_highlights[k].SetActive(false);
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		ClearActorsInRange();
		if (boardSquareSafe == null)
		{
			while (true)
			{
				DisableHighlightCursors();
				return;
			}
		}
		int num = 1;
		if (currentTargetIndex == 0)
		{
			num = 2;
		}
		if (m_highlights != null)
		{
			if (m_highlights.Count == num)
			{
				goto IL_00cf;
			}
		}
		m_highlights = new List<GameObject>();
		for (int i = 0; i < num; i++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_explosionShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		goto IL_00cf;
		IL_00cf:
		if (currentTargetIndex == 0)
		{
			Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
			BoardSquare currentBoardSquare = targetingActor.GetCurrentBoardSquare();
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_explosionShape, travelBoardSquareWorldPosition, currentBoardSquare);
			List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_explosionShape, travelBoardSquareWorldPosition, currentBoardSquare, false, targetingActor, targetingActor.GetOpposingTeam(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					AddActorInRange(current, centerOfShape, targetingActor);
				}
			}
			centerOfShape.y = (float)Board.Get().BaselineHeight + 0.1f;
			m_highlights[0].transform.position = centerOfShape;
			m_highlights[0].SetActive(true);
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
		EnableAllMovementArrows();
		if (boardSquarePathInfo != null)
		{
			num2 = AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, num2);
			Vector3 freePos = boardSquareSafe.ToVector3();
			BoardSquare centerSquare = boardSquareSafe;
			Vector3 centerOfShape2 = AreaEffectUtils.GetCenterOfShape(m_explosionShape, freePos, centerSquare);
			List<ActorData> actors2 = AreaEffectUtils.GetActorsInShape(m_explosionShape, centerOfShape2, centerSquare, false, targetingActor, targetingActor.GetOpposingTeam(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors2);
			using (List<ActorData>.Enumerator enumerator2 = actors2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData current2 = enumerator2.Current;
					AddActorInRange(current2, centerOfShape2, targetingActor);
				}
			}
			centerOfShape2.y = (float)Board.Get().BaselineHeight + 0.1f;
			m_highlights[num - 1].transform.position = centerOfShape2;
			m_highlights[num - 1].SetActive(true);
		}
		SetMovementArrowEnabledFromIndex(num2, false);
	}
}
