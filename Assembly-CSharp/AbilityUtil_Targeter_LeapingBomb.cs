using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_LeapingBomb : AbilityUtil_Targeter
{
	private AbilityAreaShape m_shape = AbilityAreaShape.Three_x_Three;

	private int m_minSeparationInSquares = 2;

	private int m_maxSeparationInSquares = 5;

	private float m_secondBombLineWidthInSquares = 1f;

	private const int m_totalExplosionCount = 2;

	public AbilityUtil_Targeter_LeapingBomb(Ability parent, AbilityAreaShape shape, int minSeparation, int maxSeparation, float bombLineWidth)
		: base(parent)
	{
		m_shape = shape;
		m_minSeparationInSquares = minSeparation;
		m_maxSeparationInSquares = maxSeparation;
		m_secondBombLineWidthInSquares = bombLineWidth;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 6)
			{
				goto IL_00ec;
			}
		}
		m_highlights = new List<GameObject>();
		for (int i = 0; i < 2; i++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		for (int j = 0; j < 2; j++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
		}
		for (int k = 0; k < 2; k++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		goto IL_00ec;
		IL_00ec:
		int num = 2;
		int num2 = 4;
		ClearActorsInRange();
		float squareSize = Board.Get().squareSize;
		List<BoardSquare> list = new List<BoardSquare>();
		List<VectorUtils.LaserCoords> list2 = new List<VectorUtils.LaserCoords>();
		BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTarget.GridPos);
		list.Add(boardSquareSafe);
		VectorUtils.LaserCoords item = default(VectorUtils.LaserCoords);
		item.start = targetingActor.GetFreePos();
		item.end = boardSquareSafe.ToVector3();
		list2.Add(item);
		BoardSquare boardSquare = null;
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		laserCoords.start = boardSquareSafe.ToVector3();
		Vector3 a = laserCoords.start - targetingActor.GetFreePos();
		a.y = 0f;
		a.Normalize();
		laserCoords.end = laserCoords.start + (float)m_maxSeparationInSquares * 1.45f * squareSize * a;
		List<BoardSquare> squares = AreaEffectUtils.GetSquaresInBox(laserCoords.start, laserCoords.end, m_secondBombLineWidthInSquares / 2f, true, targetingActor);
		AreaEffectUtils.SortSquaresByDistanceToPos(ref squares, laserCoords.start);
		BoardSquare boardSquare2 = null;
		int num3 = squares.Count - 1;
		while (true)
		{
			if (num3 >= 0)
			{
				if (squares[num3].IsValidForGameplay())
				{
					boardSquare2 = squares[num3];
					break;
				}
				num3--;
				continue;
			}
			break;
		}
		if (boardSquare2 != null)
		{
			int num4 = 0;
			BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare2, boardSquareSafe, true);
			while (true)
			{
				if (boardSquarePathInfo != null)
				{
					if (boardSquarePathInfo.square.IsValidForGameplay())
					{
						if (num4 >= m_minSeparationInSquares)
						{
							if (num4 <= m_maxSeparationInSquares)
							{
								boardSquare = boardSquarePathInfo.square;
								break;
							}
						}
					}
					num4++;
					boardSquarePathInfo = boardSquarePathInfo.next;
					continue;
				}
				break;
			}
		}
		if (boardSquare != null)
		{
			list.Add(boardSquare);
			VectorUtils.LaserCoords item2 = default(VectorUtils.LaserCoords);
			item2.start = item.end;
			item2.end = boardSquare.ToVector3();
			list2.Add(item2);
		}
		int l = 0;
		using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
		{
			GameObject gameObject;
			VectorUtils.LaserCoords laserCoords2;
			Vector3 start;
			for (; enumerator.MoveNext(); gameObject = m_highlights[l + num], HighlightUtils.Get().ResizeRectangularCursor(0.5f, list2[l].Length(), gameObject), laserCoords2 = list2[l], start = laserCoords2.start, start.y = (float)Board.Get().BaselineHeight + 0.1f, gameObject.transform.position = start, gameObject.transform.rotation = Quaternion.LookRotation(list2[l].Direction()), gameObject.SetActive(true), l++)
			{
				BoardSquare current = enumerator.Current;
				Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, current.ToVector3(), current);
				ActorData actorData = null;
				if (current.occupant != null)
				{
					actorData = current.occupant.GetComponent<ActorData>();
				}
				centerOfShape.y = (float)Board.Get().BaselineHeight + 0.1f;
				if (actorData != null)
				{
					if (actorData.GetTeam() != targetingActor.GetTeam())
					{
						if (actorData.IsActorVisibleToClient())
						{
							List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_shape, centerOfShape, current, false, targetingActor, targetingActor.GetEnemyTeam(), null);
							TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
							Vector3 damageOrigin = centerOfShape;
							using (List<ActorData>.Enumerator enumerator2 = actors.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									ActorData current2 = enumerator2.Current;
									AddActorInRange(current2, damageOrigin, targetingActor, AbilityTooltipSubject.Primary, true);
								}
							}
							m_highlights[l].transform.position = centerOfShape;
							m_highlights[l].SetActive(true);
							m_highlights[l + num2].SetActive(false);
							continue;
						}
					}
				}
				m_highlights[l + num2].transform.position = centerOfShape;
				m_highlights[l + num2].SetActive(true);
				m_highlights[l].SetActive(false);
			}
		}
		for (int m = l; m < 2; m++)
		{
			m_highlights[m].SetActive(false);
			m_highlights[m + num].SetActive(false);
			m_highlights[m + num2].SetActive(false);
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}
}
