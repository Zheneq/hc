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

	public AbilityUtil_Targeter_SoldierCardinalLines(Ability ability, AbilityAreaShape positionShape, float lineWidthInSquares, bool penetrateLos, bool useBothCardinalDirs, bool useAoeHits, AbilityAreaShape aoeShape = AbilityAreaShape.SingleSquare)
		: base(ability)
	{
		m_positionShape = positionShape;
		m_lineWidthInSquares = lineWidthInSquares;
		m_penetrateLos = penetrateLos;
		m_useBothCardinalDirs = useBothCardinalDirs;
		m_useAoeHits = useAoeHits;
		m_aoeShape = aoeShape;
		m_shouldShowActorRadius = true;
		int maxX = Board.Get().GetMaxX();
		int maxY = Board.Get().GetMaxY();
		m_targetLineLengthInSquares = (float)Mathf.Max(maxX, maxY) + 10f;
		int numDirs;
		if (m_useBothCardinalDirs)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			numDirs = 2;
		}
		else
		{
			numDirs = 1;
		}
		m_numDirs = numDirs;
		SetAffectedGroups(true, false, false);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		if (currentTargetIndex <= 0)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (targets == null)
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (targets.Count <= 0)
				{
					return;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					CreateHighlights();
					float num = m_targetLineLengthInSquares * Board.Get().squareSize;
					AbilityTarget target = targets[currentTargetIndex - 1];
					Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_positionShape, target);
					Vector3 vector = currentTarget.FreePos - centerOfShape;
					vector.y = 0f;
					Vector3 vec = Vector3.forward;
					if (vector.magnitude > 0.1f)
					{
						vec = vector.normalized;
					}
					List<Vector3> list = new List<Vector3>();
					if (m_useBothCardinalDirs)
					{
						while (true)
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
							while (true)
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
					GetHitActors(list, centerOfShape, targetingActor, out Dictionary<ActorData, Vector3> _, out List<List<ActorData>> actorsInDirsDirect, out List<Vector3> _, out List<Vector3> _, out m_directHitActorToCenterDist, out m_aoeHitActors);
					for (int i = 0; i < list.Count; i++)
					{
						Vector3 vector2 = list[i];
						Vector3 damageOrigin = centerOfShape - 0.5f * num * vector2;
						Vector3 vector3 = 0.5f * num * vector2;
						List<ActorData> list2 = actorsInDirsDirect[i];
						for (int j = 0; j < list2.Count; j++)
						{
							ActorData actor = list2[j];
							AddActorInRange(actor, damageOrigin, targetingActor);
						}
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								goto end_IL_01fa;
							}
							continue;
							end_IL_01fa:
							break;
						}
						Vector3 forward = vector2;
						vector2.x = Mathf.Abs(vector2.x);
						vector2.z = Mathf.Abs(vector2.z);
						Vector3 a = new Vector3(vector2.z, 0f, vector2.x);
						float d = 0.5f * m_lineWidthInSquares * Board.Get().squareSize;
						Vector3 a2 = centerOfShape - a * d;
						Vector3 a3 = centerOfShape + a * d;
						vector3 = 0.5f * num * vector2;
						int num2 = i * 3;
						m_highlights[num2].transform.position = a2 + vector3;
						m_highlights[num2].transform.rotation = Quaternion.LookRotation(vector2);
						m_highlights[num2 + 1].transform.position = a3 + vector3;
						m_highlights[num2 + 1].transform.rotation = Quaternion.LookRotation(vector2);
						Vector3 position = centerOfShape;
						position.y = HighlightUtils.GetHighlightHeight();
						m_highlights[num2 + 2].transform.position = position;
						m_highlights[num2 + 2].transform.rotation = Quaternion.LookRotation(forward);
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						if (!m_useAoeHits)
						{
							return;
						}
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							int num3 = m_numDirs * 3;
							foreach (KeyValuePair<ActorData, float> item2 in m_directHitActorToCenterDist)
							{
								if (m_highlights.Count <= num3)
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_aoeShape, targetingActor == GameFlowData.Get().activeOwnedActorData));
								}
								Vector3 travelBoardSquareWorldPosition = item2.Key.GetTravelBoardSquareWorldPosition();
								travelBoardSquareWorldPosition.y = HighlightUtils.GetHighlightHeight();
								m_highlights[num3].transform.position = travelBoardSquareWorldPosition;
								m_highlights[num3].SetActive(true);
								num3++;
							}
							for (int k = num3; k < m_highlights.Count; k++)
							{
								m_highlights[k].SetActive(false);
							}
							while (true)
							{
								switch (7)
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
	}

	private void CreateHighlights()
	{
		if (m_highlights != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_highlights.Count >= 3 * m_numDirs)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_highlights = new List<GameObject>();
		for (int i = 0; i < m_numDirs; i++)
		{
			GameObject item = HighlightUtils.Get().CreateBoundaryLine(m_targetLineLengthInSquares, true, true);
			GameObject item2 = HighlightUtils.Get().CreateBoundaryLine(m_targetLineLengthInSquares, true, false);
			m_highlights.Add(item);
			m_highlights.Add(item2);
			GameObject item3 = CreateArrowPointerHighlight();
			m_highlights.Add(item3);
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
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
		gameObject3.transform.Rotate(Vector3.up, 0f - num3);
		GameObject gameObject4 = new GameObject();
		gameObject4.transform.localPosition = Vector3.zero;
		gameObject4.transform.localRotation = Quaternion.identity;
		gameObject.transform.parent = gameObject4.transform;
		gameObject2.transform.parent = gameObject4.transform;
		gameObject3.transform.parent = gameObject4.transform;
		return gameObject4;
	}

	private void GetHitActors(List<Vector3> lineDirs, Vector3 shapeCenter, ActorData caster, out Dictionary<ActorData, Vector3> actorToHitOrigin, out List<List<ActorData>> actorsInDirsDirect, out List<Vector3> dirStartPosList, out List<Vector3> dirEndPosList, out Dictionary<ActorData, float> directHitActorToWorldDist, out HashSet<ActorData> aoeHitActorsInDirs)
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
			List<ActorData> actors = AreaEffectUtils.GetActorsInRadiusOfLine(vector, shapeCenter + b, 0f, 0f, 0.5f * m_lineWidthInSquares, m_penetrateLos, caster, relevantTeams, null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			vector.x = Mathf.Clamp(vector.x, 0f, (float)maxX * squareSize);
			vector.z = Mathf.Clamp(vector.z, 0f, (float)maxY * squareSize);
			Vector3 vector2 = shapeCenter + b;
			vector2.x = Mathf.Clamp(vector2.x, -1f, (float)maxX * squareSize + 1f);
			vector2.z = Mathf.Clamp(vector2.z, -1f, (float)maxY * squareSize + 1f);
			actorsInDirsDirect.Add(new List<ActorData>(actors));
			dirStartPosList.Add(vector);
			dirEndPosList.Add(vector2);
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					float num3 = AreaEffectUtils.PointToLineDistance2D(current.GetTravelBoardSquareWorldPosition(), vector, vector2);
					if (directHitActorToWorldDist.ContainsKey(current))
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (!(directHitActorToWorldDist[current] > num3))
						{
							goto IL_0205;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					directHitActorToWorldDist[current] = num3;
					goto IL_0205;
					IL_0205:
					if (!actorToHitOrigin.ContainsKey(current))
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						actorToHitOrigin[current] = vector;
					}
					else if (current.GetActorCover().IsInCoverWrt(actorToHitOrigin[current]))
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						actorToHitOrigin[current] = vector;
					}
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (m_useAoeHits)
			{
				foreach (ActorData item in actors)
				{
					List<ActorData> actors2 = AreaEffectUtils.GetActorsInShape(m_aoeShape, item.GetTravelBoardSquareWorldPosition(), item.GetCurrentBoardSquare(), m_penetrateLos, caster, relevantTeams, null);
					actors2.Remove(item);
					TargeterUtils.RemoveActorsInvisibleToClient(ref actors2);
					using (List<ActorData>.Enumerator enumerator3 = actors2.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							ActorData current3 = enumerator3.Current;
							aoeHitActorsInDirs.Add(current3);
							actorsInDirsDirect[i].Add(current3);
							if (!actorToHitOrigin.ContainsKey(current3))
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								actorToHitOrigin[current3] = vector;
							}
						}
						while (true)
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
