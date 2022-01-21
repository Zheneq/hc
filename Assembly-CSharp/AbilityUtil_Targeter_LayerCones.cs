using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_LayerCones : AbilityUtil_Targeter
{
	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	public delegate int NumActiveLayerDelegate(int maxLayers);

	public List<float> m_coneRadiusList;

	public float m_coneWidthAngle;

	public bool m_penetrateLoS;

	public float m_coneBackwardOffsetInSquares;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public IsAffectingCasterDelegate m_affectCasterDelegate;

	public NumActiveLayerDelegate m_delegateNumActiveLayers;

	public AbilityUtil_Targeter_LayerCones(Ability ability, float coneWidthAngle, List<float> coneRadiusList, float coneBackwardOffsetInSquares, bool penetrateLoS)
		: base(ability)
	{
		m_coneWidthAngle = coneWidthAngle;
		m_coneRadiusList = coneRadiusList;
		m_penetrateLoS = penetrateLoS;
		m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public float GetMaxConeRadius()
	{
		float result = 0f;
		int numActiveLayers = GetNumActiveLayers();
		if (numActiveLayers > 0)
		{
			result = m_coneRadiusList[numActiveLayers - 1];
		}
		return result;
	}

	public float GetActiveLayerRadius()
	{
		return m_coneRadiusList[GetNumActiveLayers() - 1];
	}

	public int GetNumActiveLayers()
	{
		if (m_delegateNumActiveLayers != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					int count = m_coneRadiusList.Count;
					return Mathf.Min(count, m_delegateNumActiveLayers(count));
				}
				}
			}
		}
		return m_coneRadiusList.Count;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector;
		if (currentTarget == null)
		{
			vector = targetingActor.transform.forward;
		}
		else
		{
			vector = currentTarget.AimDirection;
		}
		Vector3 vector2 = vector;
		int numActiveLayers = GetNumActiveLayers();
		m_nonActorSpecificContext.SetValue(ContextKeys.s_LayersActive.GetKey(), numActiveLayers);
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vector2);
		HandleConeCursorHighlights(travelBoardSquareWorldPositionForLos, vector2, numActiveLayers);
		List<ActorData> actors = AreaEffectUtils.GetActorsInCone(travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, m_coneWidthAngle, GetMaxConeRadius(), m_coneBackwardOffsetInSquares, m_penetrateLoS, targetingActor, TargeterUtils.GetRelevantTeams(targetingActor, m_affectsAllies, m_affectsEnemies), null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		actors.Remove(targetingActor);
		if (m_affectsTargetingActor)
		{
			if (m_affectCasterDelegate != null)
			{
				if (!m_affectCasterDelegate(targetingActor, actors))
				{
					goto IL_010c;
				}
			}
			actors.Add(targetingActor);
		}
		goto IL_010c;
		IL_010c:
		using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (ShouldAddActor(current, targetingActor))
				{
					AddActorInRange(current, travelBoardSquareWorldPositionForLos, targetingActor);
					for (int i = 0; i < m_coneRadiusList.Count; i++)
					{
						if (i >= numActiveLayers)
						{
							break;
						}
						if (AreaEffectUtils.IsSquareInConeByActorRadius(current.GetCurrentBoardSquare(), travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, m_coneWidthAngle, m_coneRadiusList[i], m_coneBackwardOffsetInSquares, true, targetingActor))
						{
							ActorHitContext actorHitContext = m_actorContextVars[current];
							actorHitContext.m_contextVars.SetValue(ContextKeys.s_Layer.GetKey(), i);
							break;
						}
					}
				}
			}
		}
		DrawInvalidSquareIndicators(currentTarget, targetingActor);
	}

	private bool ShouldAddActor(ActorData actor, ActorData caster)
	{
		bool result = false;
		if (actor == caster)
		{
			result = m_affectsTargetingActor;
		}
		else
		{
			if (actor.GetTeam() == caster.GetTeam())
			{
				if (m_affectsAllies)
				{
					result = true;
					goto IL_007f;
				}
			}
			if (actor.GetTeam() != caster.GetTeam())
			{
				if (m_affectsEnemies)
				{
					result = true;
				}
			}
		}
		goto IL_007f;
		IL_007f:
		return result;
	}

	public void HandleConeCursorHighlights(Vector3 casterPos, Vector3 centerAimDir, int numConesActive)
	{
		float d = m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = casterPos + new Vector3(0f, y, 0f) - centerAimDir * d;
		if (m_highlights != null)
		{
			if (m_highlights.Count >= m_coneRadiusList.Count)
			{
				goto IL_0125;
			}
		}
		m_highlights = new List<GameObject>();
		for (int i = 0; i < m_coneRadiusList.Count; i++)
		{
			float radiusInWorld = (m_coneRadiusList[i] + m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
			GameObject gameObject = HighlightUtils.Get().CreateConeCursor(radiusInWorld, m_coneWidthAngle);
			UIDynamicCone component = gameObject.GetComponent<UIDynamicCone>();
			if (component != null)
			{
				component.SetConeObjectActive(false);
			}
			m_highlights.Add(gameObject);
		}
		goto IL_0125;
		IL_0125:
		for (int j = 0; j < m_highlights.Count; j++)
		{
			if (j < numConesActive)
			{
				m_highlights[j].transform.position = position;
				m_highlights[j].transform.rotation = Quaternion.LookRotation(centerAimDir);
				m_highlights[j].gameObject.SetActiveIfNeeded(true);
			}
			else
			{
				m_highlights[j].gameObject.SetActiveIfNeeded(false);
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

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			ResetSquareIndicatorIndexToUse();
			Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			Vector3 vec = currentTarget?.AimDirection ?? targetingActor.transform.forward;
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
			AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, m_coneWidthAngle, GetMaxConeRadius(), m_coneBackwardOffsetInSquares, targetingActor, m_penetrateLoS);
			HideUnusedSquareIndicators();
		}
	}
}
