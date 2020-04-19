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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_LeapingBomb.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
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
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int j = 0; j < 2; j++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
		}
		for (int k = 0; k < 2; k++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		IL_EC:
		int num = 2;
		int num2 = 4;
		base.ClearActorsInRange();
		float squareSize = Board.\u000E().squareSize;
		List<BoardSquare> list = new List<BoardSquare>();
		List<VectorUtils.LaserCoords> list2 = new List<VectorUtils.LaserCoords>();
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		list.Add(boardSquare);
		VectorUtils.LaserCoords item;
		item.start = targetingActor.\u0016();
		item.end = boardSquare.ToVector3();
		list2.Add(item);
		BoardSquare boardSquare2 = null;
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = boardSquare.ToVector3();
		Vector3 a = laserCoords.start - targetingActor.\u0016();
		a.y = 0f;
		a.Normalize();
		laserCoords.end = laserCoords.start + (float)this.m_maxSeparationInSquares * 1.45f * squareSize * a;
		List<BoardSquare> squaresInBox = AreaEffectUtils.GetSquaresInBox(laserCoords.start, laserCoords.end, this.m_secondBombLineWidthInSquares / 2f, true, targetingActor);
		AreaEffectUtils.SortSquaresByDistanceToPos(ref squaresInBox, laserCoords.start);
		BoardSquare boardSquare3 = null;
		for (int l = squaresInBox.Count - 1; l >= 0; l--)
		{
			if (squaresInBox[l].\u0016())
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
				boardSquare3 = squaresInBox[l];
				IL_249:
				if (boardSquare3 != null)
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
					int num3 = 0;
					for (BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare3, boardSquare, true); boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
					{
						if (boardSquarePathInfo.square.\u0016())
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
							if (num3 >= this.m_minSeparationInSquares)
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
								if (num3 <= this.m_maxSeparationInSquares)
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
									boardSquare2 = boardSquarePathInfo.square;
									goto IL_2E0;
								}
							}
						}
						num3++;
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
				IL_2E0:
				if (boardSquare2 != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					list.Add(boardSquare2);
					VectorUtils.LaserCoords item2;
					item2.start = item.end;
					item2.end = boardSquare2.ToVector3();
					list2.Add(item2);
				}
				int num4 = 0;
				foreach (BoardSquare boardSquare4 in list)
				{
					Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_shape, boardSquare4.ToVector3(), boardSquare4);
					ActorData actorData = null;
					if (boardSquare4.occupant != null)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						actorData = boardSquare4.occupant.GetComponent<ActorData>();
					}
					centerOfShape.y = (float)Board.\u000E().BaselineHeight + 0.1f;
					if (!(actorData != null))
					{
						goto IL_4AC;
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (actorData.\u000E() == targetingActor.\u000E())
					{
						goto IL_4AC;
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
					if (!actorData.\u0018())
					{
						goto IL_4AC;
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
					List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_shape, centerOfShape, boardSquare4, false, targetingActor, targetingActor.\u0012(), null);
					TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
					Vector3 damageOrigin = centerOfShape;
					using (List<ActorData>.Enumerator enumerator2 = actorsInShape.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ActorData actor = enumerator2.Current;
							base.AddActorInRange(actor, damageOrigin, targetingActor, AbilityTooltipSubject.Primary, true);
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
					this.m_highlights[num4].transform.position = centerOfShape;
					this.m_highlights[num4].SetActive(true);
					this.m_highlights[num4 + num2].SetActive(false);
					IL_4F7:
					GameObject gameObject = this.m_highlights[num4 + num];
					HighlightUtils.Get().ResizeRectangularCursor(0.5f, list2[num4].Length(), gameObject);
					Vector3 start = list2[num4].start;
					start.y = (float)Board.\u000E().BaselineHeight + 0.1f;
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
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
