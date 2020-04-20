using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_SoldierCardinalLines : AbilityUtil_Targeter
{
	private AbilityAreaShape m_positionShape;

	private float m_lineWidthInSquares;

	private bool m_penetrateLos;

	private bool m_useBothCardinalDirs;

	private bool m_useAoeHits;

	private AbilityAreaShape m_aoeShape;

	private float m_targetLineLengthInSquares;

	private int m_numDirs = 1;

	public Dictionary<ActorData, float> m_directHitActorToCenterDist = new Dictionary<ActorData, float>();

	public HashSet<ActorData> m_aoeHitActors = new HashSet<ActorData>();

	public AbilityUtil_Targeter_SoldierCardinalLines(Ability ability, AbilityAreaShape positionShape, float lineWidthInSquares, bool penetrateLos, bool useBothCardinalDirs, bool useAoeHits, AbilityAreaShape aoeShape = AbilityAreaShape.SingleSquare) : base(ability)
	{
		this.m_positionShape = positionShape;
		this.m_lineWidthInSquares = lineWidthInSquares;
		this.m_penetrateLos = penetrateLos;
		this.m_useBothCardinalDirs = useBothCardinalDirs;
		this.m_useAoeHits = useAoeHits;
		this.m_aoeShape = aoeShape;
		this.m_shouldShowActorRadius = true;
		int maxX = Board.Get().GetMaxX();
		int maxY = Board.Get().GetMaxY();
		this.m_targetLineLengthInSquares = (float)Mathf.Max(maxX, maxY) + 10f;
		int numDirs;
		if (this.m_useBothCardinalDirs)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SoldierCardinalLines..ctor(Ability, AbilityAreaShape, float, bool, bool, bool, AbilityAreaShape)).MethodHandle;
			}
			numDirs = 2;
		}
		else
		{
			numDirs = 1;
		}
		this.m_numDirs = numDirs;
		base.SetAffectedGroups(true, false, false);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.ClearActorsInRange();
		if (currentTargetIndex > 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SoldierCardinalLines.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
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
				if (targets.Count > 0)
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
					this.CreateHighlights();
					float num = this.m_targetLineLengthInSquares * Board.Get().squareSize;
					AbilityTarget target = targets[currentTargetIndex - 1];
					Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_positionShape, target);
					Vector3 vector = currentTarget.FreePos - centerOfShape;
					vector.y = 0f;
					Vector3 vec = Vector3.forward;
					if (vector.magnitude > 0.1f)
					{
						vec = vector.normalized;
					}
					List<Vector3> list = new List<Vector3>();
					if (this.m_useBothCardinalDirs)
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
						float angle = 0f;
						if (vec.x < 0f)
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
							angle = 180f;
						}
						list.Add(VectorUtils.AngleDegreesToVector(angle));
						float angle2 = 90f;
						if (vec.z < 0f)
						{
							angle2 = 270f;
						}
						list.Add(VectorUtils.AngleDegreesToVector(angle2));
					}
					else
					{
						int angleWithHorizontal = Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vec));
						Vector3 item = VectorUtils.HorizontalAngleToClosestCardinalDirection(angleWithHorizontal);
						list.Add(item);
					}
					Dictionary<ActorData, Vector3> dictionary;
					List<List<ActorData>> list2;
					List<Vector3> list3;
					List<Vector3> list4;
					this.GetHitActors(list, centerOfShape, targetingActor, out dictionary, out list2, out list3, out list4, out this.m_directHitActorToCenterDist, out this.m_aoeHitActors);
					for (int i = 0; i < list.Count; i++)
					{
						Vector3 vector2 = list[i];
						Vector3 damageOrigin = centerOfShape - 0.5f * num * vector2;
						Vector3 b = 0.5f * num * vector2;
						List<ActorData> list5 = list2[i];
						for (int j = 0; j < list5.Count; j++)
						{
							ActorData actor = list5[j];
							base.AddActorInRange(actor, damageOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
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
						Vector3 forward = vector2;
						vector2.x = Mathf.Abs(vector2.x);
						vector2.z = Mathf.Abs(vector2.z);
						Vector3 a = new Vector3(vector2.z, 0f, vector2.x);
						float d = 0.5f * this.m_lineWidthInSquares * Board.Get().squareSize;
						Vector3 a2 = centerOfShape - a * d;
						Vector3 a3 = centerOfShape + a * d;
						b = 0.5f * num * vector2;
						int num2 = i * 3;
						this.m_highlights[num2].transform.position = a2 + b;
						this.m_highlights[num2].transform.rotation = Quaternion.LookRotation(vector2);
						this.m_highlights[num2 + 1].transform.position = a3 + b;
						this.m_highlights[num2 + 1].transform.rotation = Quaternion.LookRotation(vector2);
						Vector3 position = centerOfShape;
						position.y = HighlightUtils.GetHighlightHeight();
						this.m_highlights[num2 + 2].transform.position = position;
						this.m_highlights[num2 + 2].transform.rotation = Quaternion.LookRotation(forward);
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
					if (this.m_useAoeHits)
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
						int num3 = this.m_numDirs * 3;
						foreach (KeyValuePair<ActorData, float> keyValuePair in this.m_directHitActorToCenterDist)
						{
							if (this.m_highlights.Count <= num3)
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
								this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_aoeShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
							}
							Vector3 travelBoardSquareWorldPosition = keyValuePair.Key.GetTravelBoardSquareWorldPosition();
							travelBoardSquareWorldPosition.y = HighlightUtils.GetHighlightHeight();
							this.m_highlights[num3].transform.position = travelBoardSquareWorldPosition;
							this.m_highlights[num3].SetActive(true);
							num3++;
						}
						for (int k = num3; k < this.m_highlights.Count; k++)
						{
							this.m_highlights[k].SetActive(false);
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
					}
				}
			}
		}
	}

	private void CreateHighlights()
	{
		if (this.m_highlights != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SoldierCardinalLines.CreateHighlights()).MethodHandle;
			}
			if (this.m_highlights.Count >= 3 * this.m_numDirs)
			{
				return;
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
		this.m_highlights = new List<GameObject>();
		for (int i = 0; i < this.m_numDirs; i++)
		{
			GameObject item = HighlightUtils.Get().CreateBoundaryLine(this.m_targetLineLengthInSquares, true, true);
			GameObject item2 = HighlightUtils.Get().CreateBoundaryLine(this.m_targetLineLengthInSquares, true, false);
			this.m_highlights.Add(item);
			this.m_highlights.Add(item2);
			GameObject item3 = AbilityUtil_Targeter_SoldierCardinalLines.CreateArrowPointerHighlight(1.5f);
			this.m_highlights.Add(item3);
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

	public static GameObject CreateArrowPointerHighlight(float centerLineLenInSquares = 1.5f)
	{
		float num = Board.Get().squareSize * 0.5f;
		float num2 = Board.Get().squareSize * centerLineLenInSquares;
		GameObject gameObject = HighlightUtils.Get().CreateDynamicLineSegmentMesh(centerLineLenInSquares, 0.2f, false, Color.cyan);
		gameObject.transform.localPosition = new Vector3(0f, 0f, num);
		float lengthInSquares = 0.3f;
		float num3 = 135f;
		GameObject gameObject2 = HighlightUtils.Get().CreateDynamicLineSegmentMesh(lengthInSquares, 0.2f, false, Color.cyan);
		GameObject gameObject3 = HighlightUtils.Get().CreateDynamicLineSegmentMesh(lengthInSquares, 0.2f, false, Color.cyan);
		gameObject2.transform.localPosition = new Vector3(0f, 0f, num + num2);
		gameObject3.transform.localPosition = new Vector3(0f, 0f, num + num2);
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject3.transform.localRotation = Quaternion.identity;
		gameObject2.transform.Rotate(Vector3.up, num3);
		gameObject3.transform.Rotate(Vector3.up, -num3);
		GameObject gameObject4 = new GameObject();
		gameObject4.transform.localPosition = Vector3.zero;
		gameObject4.transform.localRotation = Quaternion.identity;
		gameObject.transform.parent = gameObject4.transform;
		gameObject2.transform.parent = gameObject4.transform;
		gameObject3.transform.parent = gameObject4.transform;
		return gameObject4;
	}

	private unsafe void GetHitActors(List<Vector3> lineDirs, Vector3 shapeCenter, ActorData caster, out Dictionary<ActorData, Vector3> actorToHitOrigin, out List<List<ActorData>> actorsInDirsDirect, out List<Vector3> dirStartPosList, out List<Vector3> dirEndPosList, out Dictionary<ActorData, float> directHitActorToWorldDist, out HashSet<ActorData> aoeHitActorsInDirs)
	{
		actorToHitOrigin = new Dictionary<ActorData, Vector3>();
		actorsInDirsDirect = new List<List<ActorData>>();
		dirStartPosList = new List<Vector3>();
		dirEndPosList = new List<Vector3>();
		directHitActorToWorldDist = new Dictionary<ActorData, float>();
		aoeHitActorsInDirs = new HashSet<ActorData>();
		int maxX = Board.Get().GetMaxX();
		int maxY = Board.Get().GetMaxY();
		float num = (float)Mathf.Max(maxX, maxY) + 10f;
		float squareSize = Board.Get().squareSize;
		float num2 = num * squareSize;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, false, true);
		for (int i = 0; i < lineDirs.Count; i++)
		{
			Vector3 a = lineDirs[i];
			Vector3 b = 0.5f * num2 * a;
			Vector3 vector = shapeCenter - b;
			List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(vector, shapeCenter + b, 0f, 0f, 0.5f * this.m_lineWidthInSquares, this.m_penetrateLos, caster, relevantTeams, null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadiusOfLine);
			vector.x = Mathf.Clamp(vector.x, 0f, (float)maxX * squareSize);
			vector.z = Mathf.Clamp(vector.z, 0f, (float)maxY * squareSize);
			Vector3 vector2 = shapeCenter + b;
			vector2.x = Mathf.Clamp(vector2.x, -1f, (float)maxX * squareSize + 1f);
			vector2.z = Mathf.Clamp(vector2.z, -1f, (float)maxY * squareSize + 1f);
			actorsInDirsDirect.Add(new List<ActorData>(actorsInRadiusOfLine));
			dirStartPosList.Add(vector);
			dirEndPosList.Add(vector2);
			using (List<ActorData>.Enumerator enumerator = actorsInRadiusOfLine.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					float num3 = AreaEffectUtils.PointToLineDistance2D(actorData.GetTravelBoardSquareWorldPosition(), vector, vector2);
					if (!directHitActorToWorldDist.ContainsKey(actorData))
					{
						goto IL_1F9;
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SoldierCardinalLines.GetHitActors(List<Vector3>, Vector3, ActorData, Dictionary<ActorData, Vector3>*, List<List<ActorData>>*, List<Vector3>*, List<Vector3>*, Dictionary<ActorData, float>*, HashSet<ActorData>*)).MethodHandle;
					}
					if (directHitActorToWorldDist[actorData] > num3)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							goto IL_1F9;
						}
					}
					IL_205:
					if (!actorToHitOrigin.ContainsKey(actorData))
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
						actorToHitOrigin[actorData] = vector;
						continue;
					}
					if (actorData.GetActorCover().IsInCoverWrt(actorToHitOrigin[actorData]))
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
						actorToHitOrigin[actorData] = vector;
						continue;
					}
					continue;
					IL_1F9:
					directHitActorToWorldDist[actorData] = num3;
					goto IL_205;
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
			}
			if (this.m_useAoeHits)
			{
				foreach (ActorData actorData2 in actorsInRadiusOfLine)
				{
					List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_aoeShape, actorData2.GetTravelBoardSquareWorldPosition(), actorData2.GetCurrentBoardSquare(), this.m_penetrateLos, caster, relevantTeams, null);
					actorsInShape.Remove(actorData2);
					TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
					using (List<ActorData>.Enumerator enumerator3 = actorsInShape.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							ActorData actorData3 = enumerator3.Current;
							aoeHitActorsInDirs.Add(actorData3);
							actorsInDirsDirect[i].Add(actorData3);
							if (!actorToHitOrigin.ContainsKey(actorData3))
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
								actorToHitOrigin[actorData3] = vector;
							}
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
}
