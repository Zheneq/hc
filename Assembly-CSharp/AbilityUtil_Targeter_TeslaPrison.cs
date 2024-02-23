using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_TeslaPrison : AbilityUtil_Targeter_TrackerDrone
{
	private TrackerTeslaPrison.PrisonWallSegmentType m_wallSegmentType = TrackerTeslaPrison.PrisonWallSegmentType.SquareMadeOfCornersAndMidsection;

	private int m_squareCornerLength;

	private int m_squareMidsectionLength;

	private int m_sides;

	private float m_radius;

	private bool m_moveDrone;

	private bool m_useShapeForSquarePrison = true;

	private AbilityAreaShape m_squarePrisonShape = AbilityAreaShape.Three_x_Three;

	public AbilityAreaShape m_shapeForActorHits;

	public AbilityUtil_Targeter_TeslaPrison(Ability ability, TrackerTeslaPrison.PrisonWallSegmentType wallSegmentType, int squareCornerLength, int squareMidsectionLength, int sides, float radius, AbilityAreaShape shapeForActorHits = AbilityAreaShape.SingleSquare, bool penetrateLoS = false)
		: base(ability)
	{
		m_wallSegmentType = wallSegmentType;
		m_squareCornerLength = squareCornerLength;
		m_squareMidsectionLength = squareMidsectionLength;
		m_sides = sides;
		m_radius = radius;
		m_shapeForActorHits = shapeForActorHits;
		m_penetrateLoS = penetrateLoS;
		m_moveDrone = false;
	}

	public AbilityUtil_Targeter_TeslaPrison(Ability ability, TrackerTeslaPrison.PrisonWallSegmentType wallSegmentType, int squareCornerLength, int squareMidsectionLength, int sides, float barrierRadius, TrackerDroneTrackerComponent droneTracker, float radiusAroundStart, float radiusAroundEnd, float rangeFromDir, int maxTargets, bool ignoreTargetsCover, bool penetrateLoS, bool addTargets, bool hitUntrackedTargets)
		: base(ability, droneTracker, radiusAroundStart, radiusAroundEnd, rangeFromDir, maxTargets, ignoreTargetsCover, penetrateLoS, addTargets, hitUntrackedTargets)
	{
		m_wallSegmentType = wallSegmentType;
		m_sides = sides;
		m_radius = barrierRadius;
		m_squareCornerLength = squareCornerLength;
		m_squareMidsectionLength = squareMidsectionLength;
		m_moveDrone = true;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		int num = GetNumHighlights();
		if (m_moveDrone)
		{
			base.UpdateTargeting(currentTarget, targetingActor);
		}
		else
		{
			num = 0;
		}
		BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTarget.GridPos);
		float squareSize = Board.Get().squareSize;
		if (m_shapeForActorHits > AbilityAreaShape.SingleSquare)
		{
			ClearActorsInRange();
			List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_shapeForActorHits, currentTarget, m_penetrateLoS, targetingActor, GetAffectedTeams(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			AddActorsInRange(actors, boardSquareSafe.ToVector3(), targetingActor);
		}
		if (m_wallSegmentType == TrackerTeslaPrison.PrisonWallSegmentType.SquareMadeOfCornersAndMidsection)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					BoardSquare boardSquareSafe2 = Board.Get().GetSquare(currentTarget.GridPos);
					if (m_squareCornerLength > 0 && m_squareMidsectionLength >= 0)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								if (!(boardSquareSafe2 == null))
								{
									if (m_highlights == null)
									{
										m_highlights = new List<GameObject>();
									}
									if (!m_useShapeForSquarePrison)
									{
										int num2;
										if (m_squareMidsectionLength > 0)
										{
											num2 = 12;
										}
										else
										{
											num2 = 8;
										}
										int num3 = num2;
										if (m_highlights.Count < num3 + num)
										{
											for (int i = 0; i < num3; i++)
											{
												float squareSize2 = Board.Get().squareSize;
												int num4;
												if (i < 8)
												{
													num4 = m_squareCornerLength;
												}
												else
												{
													num4 = m_squareMidsectionLength;
												}
												float x = squareSize2 * (float)num4;
												m_highlights.Add(Object.Instantiate(HighlightUtils.Get().m_rectangleCursorPrefab.GetComponent<UIRectangleCursor>().m_endWidthLine));
												m_highlights[i + num].transform.localScale = new Vector3(x, 1f, 1f);
											}
										}
										int num5 = num;
										float num6 = 0.05f;
										List<List<BarrierPoseInfo>> corners;
										List<BarrierPoseInfo> midSections;
										BarrierPoseInfo.GetBarrierPosesForSquaresMadeOfCornerAndMidsection(boardSquareSafe2, m_squareCornerLength, m_squareMidsectionLength, 0f - num6, out corners, out midSections);
										using (List<List<BarrierPoseInfo>>.Enumerator enumerator = corners.GetEnumerator())
										{
											while (enumerator.MoveNext())
											{
												List<BarrierPoseInfo> current = enumerator.Current;
												foreach (BarrierPoseInfo item in current)
												{
													m_highlights[num5].transform.position = item.midpoint + new Vector3(0f, 0.1f, 0f);
													m_highlights[num5].transform.rotation = Quaternion.LookRotation(-item.facingDirection);
													num5++;
												}
											}
										}
										using (List<BarrierPoseInfo>.Enumerator enumerator3 = midSections.GetEnumerator())
										{
											while (enumerator3.MoveNext())
											{
												BarrierPoseInfo current3 = enumerator3.Current;
												m_highlights[num5].transform.position = current3.midpoint + new Vector3(0f, 0.1f, 0f);
												m_highlights[num5].transform.rotation = Quaternion.LookRotation(-current3.facingDirection);
												num5++;
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
									if (m_highlights.Count < 1 + num)
									{
										m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_squarePrisonShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
									}
									GameObject gameObject = m_highlights[m_highlights.Count - 1];
									Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_squarePrisonShape, currentTarget);
									centerOfShape.y = (float)Board.Get().BaselineHeight + 0.1f;
									gameObject.transform.position = centerOfShape;
								}
								return;
							}
						}
					}
					return;
				}
				}
			}
		}
		if (m_wallSegmentType != 0)
		{
			return;
		}
		while (true)
		{
			if (m_sides < 3)
			{
				return;
			}
			while (true)
			{
				if (m_radius < 0f)
				{
					return;
				}
				Vector3 centerPos = boardSquareSafe.ToVector3();
				centerPos.y = Board.Get().BaselineHeight;
				List<BarrierPoseInfo> barrierPosesForRegularPolygon = BarrierPoseInfo.GetBarrierPosesForRegularPolygon(centerPos, m_sides, m_radius * squareSize);
				if (barrierPosesForRegularPolygon == null)
				{
					return;
				}
				while (true)
				{
					float widthInWorld = barrierPosesForRegularPolygon[0].widthInWorld;
					if (m_highlights == null)
					{
						m_highlights = new List<GameObject>();
					}
					if (m_highlights.Count < m_sides + num)
					{
						for (int j = 0; j < m_sides; j++)
						{
							m_highlights.Add(Object.Instantiate(HighlightUtils.Get().m_rectangleCursorPrefab.GetComponent<UIRectangleCursor>().m_endWidthLine));
							m_highlights[j + num].transform.localScale = new Vector3(widthInWorld, 1f, 1f);
						}
					}
					for (int k = 0; k < barrierPosesForRegularPolygon.Count; k++)
					{
						m_highlights[k + num].transform.position = barrierPosesForRegularPolygon[k].midpoint + new Vector3(0f, 0.1f, 0f);
						m_highlights[k + num].transform.rotation = Quaternion.LookRotation(-barrierPosesForRegularPolygon[k].facingDirection);
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
		}
	}
}
