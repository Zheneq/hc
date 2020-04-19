using System;
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

	public AbilityUtil_Targeter_TeslaPrison(Ability ability, TrackerTeslaPrison.PrisonWallSegmentType wallSegmentType, int squareCornerLength, int squareMidsectionLength, int sides, float radius, AbilityAreaShape shapeForActorHits = AbilityAreaShape.SingleSquare, bool penetrateLoS = false) : base(ability)
	{
		this.m_wallSegmentType = wallSegmentType;
		this.m_squareCornerLength = squareCornerLength;
		this.m_squareMidsectionLength = squareMidsectionLength;
		this.m_sides = sides;
		this.m_radius = radius;
		this.m_shapeForActorHits = shapeForActorHits;
		this.m_penetrateLoS = penetrateLoS;
		this.m_moveDrone = false;
	}

	public AbilityUtil_Targeter_TeslaPrison(Ability ability, TrackerTeslaPrison.PrisonWallSegmentType wallSegmentType, int squareCornerLength, int squareMidsectionLength, int sides, float barrierRadius, TrackerDroneTrackerComponent droneTracker, float radiusAroundStart, float radiusAroundEnd, float rangeFromDir, int maxTargets, bool ignoreTargetsCover, bool penetrateLoS, bool addTargets, bool hitUntrackedTargets) : base(ability, droneTracker, radiusAroundStart, radiusAroundEnd, rangeFromDir, maxTargets, ignoreTargetsCover, penetrateLoS, addTargets, hitUntrackedTargets)
	{
		this.m_wallSegmentType = wallSegmentType;
		this.m_sides = sides;
		this.m_radius = barrierRadius;
		this.m_squareCornerLength = squareCornerLength;
		this.m_squareMidsectionLength = squareMidsectionLength;
		this.m_moveDrone = true;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		int num = base.GetNumHighlights();
		if (this.m_moveDrone)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_TeslaPrison.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			base.UpdateTargeting(currentTarget, targetingActor);
		}
		else
		{
			num = 0;
		}
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		float squareSize = Board.\u000E().squareSize;
		if (this.m_shapeForActorHits > AbilityAreaShape.SingleSquare)
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
			base.ClearActorsInRange();
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_shapeForActorHits, currentTarget, this.m_penetrateLoS, targetingActor, base.GetAffectedTeams(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
			base.AddActorsInRange(actorsInShape, boardSquare.ToVector3(), targetingActor, AbilityTooltipSubject.Primary, false);
		}
		if (this.m_wallSegmentType == TrackerTeslaPrison.PrisonWallSegmentType.SquareMadeOfCornersAndMidsection)
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
			BoardSquare boardSquare2 = Board.\u000E().\u000E(currentTarget.GridPos);
			if (this.m_squareCornerLength > 0 && this.m_squareMidsectionLength >= 0)
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
				if (!(boardSquare2 == null))
				{
					if (this.m_highlights == null)
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
						this.m_highlights = new List<GameObject>();
					}
					if (this.m_useShapeForSquarePrison)
					{
						if (this.m_highlights.Count < 1 + num)
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
							this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_squarePrisonShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
						}
						GameObject gameObject = this.m_highlights[this.m_highlights.Count - 1];
						Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_squarePrisonShape, currentTarget);
						centerOfShape.y = (float)Board.\u000E().BaselineHeight + 0.1f;
						gameObject.transform.position = centerOfShape;
					}
					else
					{
						int num2;
						if (this.m_squareMidsectionLength > 0)
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
							num2 = 0xC;
						}
						else
						{
							num2 = 8;
						}
						int num3 = num2;
						if (this.m_highlights.Count < num3 + num)
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
							for (int i = 0; i < num3; i++)
							{
								float squareSize2 = Board.\u000E().squareSize;
								float num4;
								if (i < 8)
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
									num4 = (float)this.m_squareCornerLength;
								}
								else
								{
									num4 = (float)this.m_squareMidsectionLength;
								}
								float x = squareSize2 * num4;
								this.m_highlights.Add(UnityEngine.Object.Instantiate<GameObject>(HighlightUtils.Get().m_rectangleCursorPrefab.GetComponent<UIRectangleCursor>().m_endWidthLine));
								this.m_highlights[i + num].transform.localScale = new Vector3(x, 1f, 1f);
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
						int num5 = num;
						float num6 = 0.05f;
						List<List<BarrierPoseInfo>> list;
						List<BarrierPoseInfo> list2;
						BarrierPoseInfo.GetBarrierPosesForSquaresMadeOfCornerAndMidsection(boardSquare2, (float)this.m_squareCornerLength, (float)this.m_squareMidsectionLength, -num6, out list, out list2);
						using (List<List<BarrierPoseInfo>>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								List<BarrierPoseInfo> list3 = enumerator.Current;
								foreach (BarrierPoseInfo barrierPoseInfo in list3)
								{
									this.m_highlights[num5].transform.position = barrierPoseInfo.midpoint + new Vector3(0f, 0.1f, 0f);
									this.m_highlights[num5].transform.rotation = Quaternion.LookRotation(-barrierPoseInfo.facingDirection);
									num5++;
								}
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
						using (List<BarrierPoseInfo>.Enumerator enumerator3 = list2.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								BarrierPoseInfo barrierPoseInfo2 = enumerator3.Current;
								this.m_highlights[num5].transform.position = barrierPoseInfo2.midpoint + new Vector3(0f, 0.1f, 0f);
								this.m_highlights[num5].transform.rotation = Quaternion.LookRotation(-barrierPoseInfo2.facingDirection);
								num5++;
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
					}
					return;
				}
			}
			return;
		}
		if (this.m_wallSegmentType == TrackerTeslaPrison.PrisonWallSegmentType.RegularPolygon)
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
			if (this.m_sides >= 3)
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
				if (this.m_radius >= 0f)
				{
					Vector3 centerPos = boardSquare.ToVector3();
					centerPos.y = (float)Board.\u000E().BaselineHeight;
					List<BarrierPoseInfo> barrierPosesForRegularPolygon = BarrierPoseInfo.GetBarrierPosesForRegularPolygon(centerPos, this.m_sides, this.m_radius * squareSize, 0f);
					if (barrierPosesForRegularPolygon == null)
					{
						return;
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
					float widthInWorld = barrierPosesForRegularPolygon[0].widthInWorld;
					if (this.m_highlights == null)
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
						this.m_highlights = new List<GameObject>();
					}
					if (this.m_highlights.Count < this.m_sides + num)
					{
						for (int j = 0; j < this.m_sides; j++)
						{
							this.m_highlights.Add(UnityEngine.Object.Instantiate<GameObject>(HighlightUtils.Get().m_rectangleCursorPrefab.GetComponent<UIRectangleCursor>().m_endWidthLine));
							this.m_highlights[j + num].transform.localScale = new Vector3(widthInWorld, 1f, 1f);
						}
					}
					for (int k = 0; k < barrierPosesForRegularPolygon.Count; k++)
					{
						this.m_highlights[k + num].transform.position = barrierPosesForRegularPolygon[k].midpoint + new Vector3(0f, 0.1f, 0f);
						this.m_highlights[k + num].transform.rotation = Quaternion.LookRotation(-barrierPosesForRegularPolygon[k].facingDirection);
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						return;
					}
				}
			}
			return;
		}
	}
}
