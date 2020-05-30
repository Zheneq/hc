using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ConeOrLaser : AbilityUtil_Targeter
{
	public delegate bool ShouldAddCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	private ConeTargetingInfo m_coneInfo;

	private LaserTargetingInfo m_laserInfo;

	private float m_toConeThreshold;

	private const int c_coneHighlightIndex = 0;

	private const int c_laserHighlightIndex = 1;

	private const int c_threshHighlightIndex = 2;

	private TargeterPart_Cone m_conePart;

	private TargeterPart_Laser m_laserPart;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public ShouldAddCasterDelegate m_customShouldAddCasterDelegate;

	public bool m_useCasterLocationForAllMultiTargets;

	public bool m_updatingWithCone;

	public AbilityUtil_Targeter_ConeOrLaser(Ability ability, ConeTargetingInfo coneInfo, LaserTargetingInfo laserInfo, float coneThresholdDist)
		: base(ability)
	{
		m_coneInfo = coneInfo;
		m_laserInfo = laserInfo;
		m_toConeThreshold = coneThresholdDist;
		SetAffectedGroups(coneInfo.m_affectsEnemies, coneInfo.m_affectsAllies, coneInfo.m_affectsCaster);
		m_conePart = new TargeterPart_Cone(m_coneInfo);
		m_laserPart = new TargeterPart_Laser(m_laserInfo);
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		int shouldShowActorRadius;
		if (!GameWideData.Get().UseActorRadiusForLaser())
		{
			shouldShowActorRadius = (GameWideData.Get().UseActorRadiusForCone() ? 1 : 0);
		}
		else
		{
			shouldShowActorRadius = 1;
		}
		m_shouldShowActorRadius = ((byte)shouldShowActorRadius != 0);
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (m_highlights == null)
		{
			return;
		}
		while (true)
		{
			if (m_highlights.Count >= 3)
			{
				while (true)
				{
					m_highlights[2].SetActive(false);
					return;
				}
			}
			return;
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		CreateHighlights();
		Vector3 vector = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector2 = currentTarget.AimDirection;
		if (currentTargetIndex > 0)
		{
			if (targets != null)
			{
				BoardSquare boardSquareSafe = Board.Get().GetSquare(targets[currentTargetIndex - 1].GridPos);
				if (boardSquareSafe != null)
				{
					if (!m_useCasterLocationForAllMultiTargets)
					{
						vector = boardSquareSafe.GetWorldPositionForLoS();
					}
					vector2 = currentTarget.FreePos - vector;
					vector2.y = 0f;
					if (vector2.sqrMagnitude == 0f)
					{
						vector2 = Vector3.forward;
					}
					else
					{
						vector2.Normalize();
					}
				}
			}
		}
		Vector3 vector3 = currentTarget.FreePos - vector;
		vector3.y = 0f;
		float magnitude = vector3.magnitude;
		bool flag = m_updatingWithCone = (magnitude <= m_toConeThreshold);
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, m_affectsAllies, m_affectsEnemies);
		GameObject gameObject = m_highlights[0];
		GameObject gameObject2 = m_highlights[1];
		GameObject gameObject3 = m_highlights[2];
		List<ActorData> actors;
		if (flag)
		{
			actors = m_conePart.GetHitActors(vector, vector2, targetingActor, relevantTeams);
			m_conePart.AdjustHighlight(gameObject, vector, vector2);
			gameObject.SetActive(true);
			gameObject2.SetActive(false);
			DrawInvalidSquareIndicators_Cone(currentTarget, targetingActor, vector, vector2);
		}
		else
		{
			VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
			laserCoords.start = vector;
			actors = m_laserPart.GetHitActors(laserCoords.start, vector2, targetingActor, relevantTeams, out laserCoords.end);
			m_laserPart.AdjustHighlight(gameObject2, laserCoords.start, laserCoords.end);
			gameObject.SetActive(false);
			gameObject2.SetActive(true);
			DrawInvalidSquareIndicators_Laser(currentTarget, targetingActor, laserCoords.start, laserCoords.end);
		}
		if (gameObject3 != null)
		{
			Vector3 position = vector;
			position.y = HighlightUtils.GetHighlightHeight();
			gameObject3.transform.position = position;
			gameObject3.transform.rotation = Quaternion.LookRotation(vector2);
		}
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				Vector3 damageOrigin = vector;
				int subjectType;
				if (flag)
				{
					subjectType = 1;
				}
				else
				{
					subjectType = 2;
				}
				AddActorInRange(current, damageOrigin, targetingActor, (AbilityTooltipSubject)subjectType);
				ActorHitContext actorHitContext = m_actorContextVars[current];
				float value = VectorUtils.HorizontalPlaneDistInSquares(vector, current.GetTravelBoardSquareWorldPosition());
				actorHitContext._0015.SetFloat(ContextKeys._0018.GetHash(), value);
				ContextVars contextVars = actorHitContext._0015;
				int hash = TargetSelect_ConeOrLaser.s_cvarInCone.GetHash();
				int value2;
				if (flag)
				{
					value2 = 1;
				}
				else
				{
					value2 = 0;
				}
				contextVars.SetInt(hash, value2);
				if (current != targetingActor)
				{
					float num = 0f;
					if (flag)
					{
						Vector3 to = current.GetTravelBoardSquareWorldPosition() - targetingActor.GetTravelBoardSquareWorldPosition();
						to.y = 0f;
						num = Vector3.Angle(vector2, to);
						actorHitContext._0015.SetFloat(ContextKeys._001D.GetHash(), num);
					}
				}
			}
		}
		if (!m_affectsTargetingActor || actors.Contains(targetingActor))
		{
			return;
		}
		while (true)
		{
			if (m_customShouldAddCasterDelegate != null)
			{
				if (!m_customShouldAddCasterDelegate(targetingActor, actors))
				{
					return;
				}
			}
			AddActorInRange(targetingActor, vector, targetingActor, AbilityTooltipSubject.Self);
			return;
		}
	}

	private void CreateHighlights()
	{
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 3)
			{
				return;
			}
		}
		m_highlights = new List<GameObject>();
		GameObject item = m_conePart.CreateHighlightObject(this);
		m_highlights.Add(item);
		m_highlights[0].SetActive(false);
		GameObject item2 = m_laserPart.CreateHighlightObject(this);
		m_highlights.Add(item2);
		m_highlights[0].SetActive(false);
		float num = 1.2f;
		GameObject gameObject = HighlightUtils.Get().CreateDynamicLineSegmentMesh(num, 0.5f, false, Color.cyan);
		gameObject.transform.localPosition = new Vector3(-0.5f * Board.Get().squareSize * num, 0f, m_toConeThreshold);
		gameObject.transform.localRotation = Quaternion.LookRotation(new Vector3(1f, 0f, 0f));
		GameObject gameObject2 = new GameObject();
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject.transform.parent = gameObject2.transform;
		m_highlights.Add(gameObject2);
	}

	private void DrawInvalidSquareIndicators_Cone(AbilityTarget currentTarget, ActorData targetingActor, Vector3 coneStartPos, Vector3 forwardDirection)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			ResetSquareIndicatorIndexToUse();
			m_conePart.ShowHiddenSquares(m_indicatorHandler, coneStartPos, VectorUtils.HorizontalAngle_Deg(forwardDirection), targetingActor, m_coneInfo.m_penetrateLos);
			HideUnusedSquareIndicators();
			return;
		}
	}

	private void DrawInvalidSquareIndicators_Laser(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			ResetSquareIndicatorIndexToUse();
			m_laserPart.ShowHiddenSquares(m_indicatorHandler, startPos, endPos, targetingActor, m_laserInfo.penetrateLos);
			HideUnusedSquareIndicators();
			return;
		}
	}
}
