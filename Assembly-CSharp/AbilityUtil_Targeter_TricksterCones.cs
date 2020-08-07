using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_TricksterCones : AbilityUtil_Targeter
{
	public delegate int GetCurrentNumberOfConesDelegate();

	public delegate Vector3 GetClampedTargetPosDelegate(AbilityTarget currentTarget, ActorData caster);

	public delegate List<Vector3> GetConeInfoDelegate(AbilityTarget currentTarget, Vector3 freeTargetPos, ActorData caster);

	public delegate Vector3 DamageOriginDelegate(AbilityTarget currentTarget, Vector3 defaultOrigin, ActorData actorToAdd, ActorData caster);

	public ConeTargetingInfo m_coneInfo;

	private int m_maxCones;

	public bool m_showHitIndicatorLine;

	private bool m_useCasterPosForLoS;

	public Dictionary<ActorData, int> m_actorToHitCount = new Dictionary<ActorData, int>();

	public Dictionary<ActorData, int> m_actorToCoverCount = new Dictionary<ActorData, int>();

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	private GetCurrentNumberOfConesDelegate GetCurrentNumberOfCones;

	private GetClampedTargetPosDelegate GetClampedTargetPos;

	private GetConeInfoDelegate GetConeOrigins;

	private GetConeInfoDelegate GetConeDirections;

	public DamageOriginDelegate m_customDamageOriginDelegate;

	private Dictionary<ActorData, Vector3> m_tempActorToDamageOrigins = new Dictionary<ActorData, Vector3>();

	public AbilityUtil_Targeter_TricksterCones(Ability ability, ConeTargetingInfo coneTargetingInfo, int maxCones, GetCurrentNumberOfConesDelegate numConesDelegate, GetConeInfoDelegate coneOriginsDelegate, GetConeInfoDelegate coneDirectionsDelegate, GetClampedTargetPosDelegate clampedTargetPosDelegate, bool showHitIndicatorLine, bool useCasterPosForLoS)
		: base(ability)
	{
		m_coneInfo = coneTargetingInfo;
		m_maxCones = maxCones;
		m_useCasterPosForLoS = useCasterPosForLoS;
		GetCurrentNumberOfCones = numConesDelegate;
		GetClampedTargetPos = clampedTargetPosDelegate;
		GetConeOrigins = coneOriginsDelegate;
		GetConeDirections = coneDirectionsDelegate;
		SetAffectedGroups(m_coneInfo.m_affectsEnemies, m_coneInfo.m_affectsAllies, m_coneInfo.m_affectsCaster);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		for (int i = 0; i < m_maxCones; i++)
		{
			m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
		}
		m_showHitIndicatorLine = showHitIndicatorLine;
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (m_highlights == null || m_highlights.Count != m_maxCones * 2)
		{
			return;
		}
		while (true)
		{
			for (int i = m_maxCones; i < m_highlights.Count; i++)
			{
				m_highlights[i].SetActive(false);
			}
			return;
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		float squareSize = Board.Get().squareSize;
		float radiusInWorld = (m_coneInfo.m_radiusInSquares + m_coneInfo.m_backwardsOffset) * squareSize;
		if (m_highlights != null)
		{
			if (m_highlights.Count >= m_maxCones * 2)
			{
				goto IL_00ff;
			}
		}
		m_highlights = new List<GameObject>();
		for (int i = 0; i < m_maxCones; i++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateConeCursor(radiusInWorld, m_coneInfo.m_widthAngleDeg));
		}
		for (int j = 0; j < m_maxCones; j++)
		{
			GameObject item = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, 0.3f, true, Color.cyan);
			m_highlights.Add(item);
		}
		goto IL_00ff;
		IL_00ff:
		int num;
		if (GameFlowData.Get() != null)
		{
			num = ((GameFlowData.Get().activeOwnedActorData == targetingActor) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool active = (byte)num != 0;
		int num2 = GetCurrentNumberOfCones();
		for (int k = 0; k < m_maxCones; k++)
		{
			if (k < num2)
			{
				m_highlights[k].SetActive(true);
				m_highlights[m_maxCones + k].SetActive(active);
			}
			else
			{
				m_highlights[k].SetActive(false);
				m_highlights[m_maxCones + k].SetActive(false);
			}
		}
		while (true)
		{
			Vector3 freeTargetPos = GetClampedTargetPos(currentTarget, targetingActor);
			List<Vector3> list = GetConeOrigins(currentTarget, freeTargetPos, targetingActor);
			List<Vector3> list2 = GetConeDirections(currentTarget, freeTargetPos, targetingActor);
			m_tempActorToDamageOrigins.Clear();
			Dictionary<ActorData, Vector3> tempActorToDamageOrigins = m_tempActorToDamageOrigins;
			m_actorToHitCount.Clear();
			m_actorToCoverCount.Clear();
			for (int l = 0; l < num2; l++)
			{
				if (l < m_maxCones)
				{
					Vector3 vector = list[l];
					Vector3 vector2 = list2[l];
					vector2.y = 0f;
					float magnitude = vector2.magnitude;
					vector2.Normalize();
					float num3 = VectorUtils.HorizontalAngle_Deg(vector2);
					List<ActorData> actors = AreaEffectUtils.GetActorsInCone(vector, num3, m_coneInfo.m_widthAngleDeg, m_coneInfo.m_radiusInSquares, m_coneInfo.m_backwardsOffset, m_coneInfo.m_penetrateLos, targetingActor, GetAffectedTeams(), null);
					TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
					if (actors.Contains(targetingActor))
					{
						actors.Remove(targetingActor);
					}
					foreach (ActorData item2 in actors)
					{
						ActorCover actorCover = item2.GetActorCover();
						bool flag = actorCover.IsInCoverWrt(vector);
						if (tempActorToDamageOrigins.ContainsKey(item2))
						{
							m_actorToHitCount[item2]++;
							Dictionary<ActorData, int> actorToCoverCount;
							Dictionary<ActorData, int> dictionary = actorToCoverCount = m_actorToCoverCount;
							ActorData key;
							ActorData key2 = key = item2;
							int num4 = actorToCoverCount[key];
							int num5;
							if (flag)
							{
								num5 = 1;
							}
							else
							{
								num5 = 0;
							}
							dictionary[key2] = num4 + num5;
							if (actorCover != null)
							{
								if (!actorCover.IsInCoverWrt(tempActorToDamageOrigins[item2]))
								{
									if (actorCover.IsInCoverWrt(vector))
									{
										tempActorToDamageOrigins[item2] = vector;
									}
								}
							}
						}
						else
						{
							m_actorToHitCount[item2] = 1;
							Dictionary<ActorData, int> actorToCoverCount2 = m_actorToCoverCount;
							int value;
							if (flag)
							{
								value = 1;
							}
							else
							{
								value = 0;
							}
							actorToCoverCount2[item2] = value;
							tempActorToDamageOrigins[item2] = vector;
						}
					}
					if (m_affectsTargetingActor)
					{
						AddActorInRange(targetingActor, vector, targetingActor, AbilityTooltipSubject.Tertiary);
					}
					float d = m_coneInfo.m_backwardsOffset * squareSize;
					Vector3 position = vector - vector2 * d;
					position.y = HighlightUtils.GetHighlightHeight();
					m_highlights[l].transform.position = position;
					m_highlights[l].transform.rotation = Quaternion.LookRotation(vector2);
					if (m_showHitIndicatorLine)
					{
						m_highlights[m_maxCones + l].transform.position = position;
						m_highlights[m_maxCones + l].transform.rotation = Quaternion.LookRotation(vector2);
					}
					else if (m_highlights[m_maxCones + l].activeSelf)
					{
						m_highlights[m_maxCones + l].SetActive(false);
					}
					Color color;
					if (actors.Count > 0)
					{
						color = Color.red;
					}
					else
					{
						color = Color.cyan;
					}
					Color color2 = color;
					HighlightUtils.Get().AdjustDynamicLineSegmentMesh(m_highlights[m_maxCones + l], magnitude / Board.Get().squareSize, color2);
					if (l == num2 - 1)
					{
						if (l < m_squarePosCheckerList.Count - 1)
						{
							for (int m = l; m < m_squarePosCheckerList.Count; m++)
							{
								SquareInsideChecker_Cone squareInsideChecker_Cone = m_squarePosCheckerList[m] as SquareInsideChecker_Cone;
								squareInsideChecker_Cone.UpdateConeProperties(vector, m_coneInfo.m_widthAngleDeg, m_coneInfo.m_radiusInSquares, m_coneInfo.m_backwardsOffset, num3, targetingActor);
								if (m_useCasterPosForLoS)
								{
									squareInsideChecker_Cone.SetLosPosOverride(true, targetingActor.GetTravelBoardSquareWorldPositionForLos(), true);
								}
							}
							continue;
						}
					}
					SquareInsideChecker_Cone squareInsideChecker_Cone2 = m_squarePosCheckerList[l] as SquareInsideChecker_Cone;
					squareInsideChecker_Cone2.UpdateConeProperties(vector, m_coneInfo.m_widthAngleDeg, m_coneInfo.m_radiusInSquares, m_coneInfo.m_backwardsOffset, num3, targetingActor);
					if (m_useCasterPosForLoS)
					{
						squareInsideChecker_Cone2.SetLosPosOverride(true, targetingActor.GetTravelBoardSquareWorldPositionForLos(), true);
					}
					continue;
				}
				break;
			}
			foreach (KeyValuePair<ActorData, Vector3> item3 in tempActorToDamageOrigins)
			{
				ActorData key3 = item3.Key;
				Vector3 vector3 = item3.Value;
				if (m_customDamageOriginDelegate != null)
				{
					vector3 = m_customDamageOriginDelegate(currentTarget, vector3, key3, targetingActor);
				}
				for (int n = 0; n < m_actorToHitCount[key3]; n++)
				{
					AbilityTooltipSubject subjectType = (key3.GetTeam() != targetingActor.GetTeam()) ? AbilityTooltipSubject.Primary : AbilityTooltipSubject.Secondary;
					AddActorInRange(key3, vector3, targetingActor, subjectType, true);
					ActorHitContext actorHitContext = m_actorContextVars[key3];
					actorHitContext.context.SetInt(ContextKeys._0019.GetKey(), m_actorToHitCount[key3]);
				}
			}
			if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
			{
				return;
			}
			while (true)
			{
				ResetSquareIndicatorIndexToUse();
				for (int num6 = 0; num6 < num2; num6++)
				{
					if (num6 >= m_maxCones)
					{
						break;
					}
					Vector3 coneStart = list[num6];
					Vector3 vec = list2[num6];
					vec.y = 0f;
					vec.Normalize();
					float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
					AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, coneStart, coneCenterAngleDegrees, m_coneInfo.m_widthAngleDeg, m_coneInfo.m_radiusInSquares, m_coneInfo.m_backwardsOffset, targetingActor, m_coneInfo.m_penetrateLos, m_squarePosCheckerList);
				}
				HideUnusedSquareIndicators();
				return;
			}
		}
	}
}
