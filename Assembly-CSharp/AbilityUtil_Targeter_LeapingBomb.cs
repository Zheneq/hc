using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_LeapingBomb : AbilityUtil_Targeter
{
	private AbilityAreaShape m_shape = AbilityAreaShape.Three_x_Three;

	private int m_minSeparationInSquares = 2;

	private int m_maxSeparationInSquares = 5;

	private float m_secondBombLineWidthInSquares = 1f;

	private const int m_totalExplosionCount = 2;

	public AbilityUtil_Targeter_LeapingBomb(Ability parent, AbilityAreaShape shape, int minSeparation, int maxSeparation, float bombLineWidth) : base(parent)
	{
		this.m_shape = shape;
		this.m_minSeparationInSquares = minSeparation;
		this.m_maxSeparationInSquares = maxSeparation;
		this.m_secondBombLineWidthInSquares = bombLineWidth;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 6)
			{
				goto IL_EC;
			}
		}
		this.m_highlights = new List<GameObject>();
		for (int i = 0; i < 2; i++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		for (int j = 0; j < 2; j++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
		}
		for (int k = 0; k < 2; k++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		IL_EC:
		int num = 2;
		int num2 = 4;
		base.ClearActorsInRange();
		float squareSize = Board.Get().squareSize;
		List<BoardSquare> list = new List<BoardSquare>();
		List<VectorUtils.LaserCoords> list2 = new List<VectorUtils.LaserCoords>();
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		list.Add(boardSquareSafe);
		VectorUtils.LaserCoords item;
		item.start = targetingActor.GetTravelBoardSquareWorldPosition();
		item.end = boardSquareSafe.ToVector3();
		list2.Add(item);
		BoardSquare boardSquare = null;
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = boardSquareSafe.ToVector3();
		Vector3 a = laserCoords.start - targetingActor.GetTravelBoardSquareWorldPosition();
		a.y = 0f;
		a.Normalize();
		laserCoords.end = laserCoords.start + (float)this.m_maxSeparationInSquares * 1.45f * squareSize * a;
		List<BoardSquare> squaresInBox = AreaEffectUtils.GetSquaresInBox(laserCoords.start, laserCoords.end, this.m_secondBombLineWidthInSquares / 2f, true, targetingActor);
		AreaEffectUtils.SortSquaresByDistanceToPos(ref squaresInBox, laserCoords.start);
		BoardSquare boardSquare2 = null;
		for (int l = squaresInBox.Count - 1; l >= 0; l--)
		{
			if (squaresInBox[l].IsBaselineHeight())
			{
				boardSquare2 = squaresInBox[l];
				IL_249:
				if (boardSquare2 != null)
				{
					int num3 = 0;
					for (BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare2, boardSquareSafe, true); boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
					{
						if (boardSquarePathInfo.square.IsBaselineHeight())
						{
							if (num3 >= this.m_minSeparationInSquares)
							{
								if (num3 <= this.m_maxSeparationInSquares)
								{
									boardSquare = boardSquarePathInfo.square;
									goto IL_2E0;
								}
							}
						}
						num3++;
					}
				}
				IL_2E0:
				if (boardSquare != null)
				{
					list.Add(boardSquare);
					VectorUtils.LaserCoords item2;
					item2.start = item.end;
					item2.end = boardSquare.ToVector3();
					list2.Add(item2);
				}
				int num4 = 0;
				foreach (BoardSquare boardSquare3 in list)
				{
					Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_shape, boardSquare3.ToVector3(), boardSquare3);
					ActorData actorData = null;
					if (boardSquare3.occupant != null)
					{
						actorData = boardSquare3.occupant.GetComponent<ActorData>();
					}
					centerOfShape.y = (float)Board.Get().BaselineHeight + 0.1f;
					if (!(actorData != null))
					{
						goto IL_4AC;
					}
					if (actorData.GetTeam() == targetingActor.GetTeam())
					{
						goto IL_4AC;
					}
					if (!actorData.IsVisibleToClient())
					{
						goto IL_4AC;
					}
					List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_shape, centerOfShape, boardSquare3, false, targetingActor, targetingActor.GetOpposingTeam(), null);
					TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
					Vector3 damageOrigin = centerOfShape;
					using (List<ActorData>.Enumerator enumerator2 = actorsInShape.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ActorData actor = enumerator2.Current;
							base.AddActorInRange(actor, damageOrigin, targetingActor, AbilityTooltipSubject.Primary, true);
						}
					}
					this.m_highlights[num4].transform.position = centerOfShape;
					this.m_highlights[num4].SetActive(true);
					this.m_highlights[num4 + num2].SetActive(false);
					IL_4F7:
					GameObject gameObject = this.m_highlights[num4 + num];
					HighlightUtils.Get().ResizeRectangularCursor(0.5f, list2[num4].Length(), gameObject);
					Vector3 start = list2[num4].start;
					start.y = (float)Board.Get().BaselineHeight + 0.1f;
					gameObject.transform.position = start;
					gameObject.transform.rotation = Quaternion.LookRotation(list2[num4].Direction());
					gameObject.SetActive(true);
					num4++;
					continue;
					IL_4AC:
					this.m_highlights[num4 + num2].transform.position = centerOfShape;
					this.m_highlights[num4 + num2].SetActive(true);
					this.m_highlights[num4].SetActive(false);
					goto IL_4F7;
				}
				for (int m = num4; m < 2; m++)
				{
					this.m_highlights[m].SetActive(false);
					this.m_highlights[m + num].SetActive(false);
					this.m_highlights[m + num2].SetActive(false);
				}
				return;
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			goto IL_249;
		}
	}
}
