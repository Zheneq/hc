using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ConeOrLaser : AbilityUtil_Targeter
{
	public delegate bool ShouldAddCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	private const int c_coneHighlightIndex = 0;
	private const int c_laserHighlightIndex = 1;
	private const int c_threshHighlightIndex = 2;

	private ConeTargetingInfo m_coneInfo;
	private LaserTargetingInfo m_laserInfo;
	private float m_toConeThreshold;
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
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser() || GameWideData.Get().UseActorRadiusForCone();
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (m_highlights == null)
		{
			return;
		}
		if (m_highlights.Count >= 3)
		{
			m_highlights[2].SetActive(false);
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
		Vector3 src = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 aimDirection = currentTarget.AimDirection;
		if (currentTargetIndex > 0 && targets != null)
		{
			BoardSquare prevTargetSquare = Board.Get().GetSquare(targets[currentTargetIndex - 1].GridPos);
			if (prevTargetSquare != null)
			{
				if (!m_useCasterLocationForAllMultiTargets)
				{
					src = prevTargetSquare.GetWorldPositionForLoS();
				}
				aimDirection = currentTarget.FreePos - src;
				aimDirection.y = 0f;
				if (aimDirection.sqrMagnitude == 0f)
				{
					aimDirection = Vector3.forward;
				}
				else
				{
					aimDirection.Normalize();
				}
			}
		}
		Vector3 aimVector = currentTarget.FreePos - src;
		aimVector.y = 0f;
		float aimDistance = aimVector.magnitude;
		bool updatingWithCone = m_updatingWithCone = (aimDistance <= m_toConeThreshold);
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, m_affectsAllies, m_affectsEnemies);
		GameObject gameObject = m_highlights[0];
		GameObject gameObject2 = m_highlights[1];
		GameObject gameObject3 = m_highlights[2];
		List<ActorData> actors;
		if (updatingWithCone)
		{
			actors = m_conePart.GetHitActors(src, aimDirection, targetingActor, relevantTeams);
			m_conePart.AdjustHighlight(gameObject, src, aimDirection);
			gameObject.SetActive(true);
			gameObject2.SetActive(false);
			DrawInvalidSquareIndicators_Cone(currentTarget, targetingActor, src, aimDirection);
		}
		else
		{
			VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
			laserCoords.start = src;
			actors = m_laserPart.GetHitActors(laserCoords.start, aimDirection, targetingActor, relevantTeams, out laserCoords.end);
			m_laserPart.AdjustHighlight(gameObject2, laserCoords.start, laserCoords.end);
			gameObject.SetActive(false);
			gameObject2.SetActive(true);
			DrawInvalidSquareIndicators_Laser(currentTarget, targetingActor, laserCoords.start, laserCoords.end);
		}
		if (gameObject3 != null)
		{
			Vector3 position = src;
			position.y = HighlightUtils.GetHighlightHeight();
			gameObject3.transform.position = position;
			gameObject3.transform.rotation = Quaternion.LookRotation(aimDirection);
		}
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				Vector3 damageOrigin = src;
				int subjectType;
				if (updatingWithCone)
				{
					subjectType = 1;
				}
				else
				{
					subjectType = 2;
				}
				AddActorInRange(current, damageOrigin, targetingActor, (AbilityTooltipSubject)subjectType);
				ActorHitContext actorHitContext = m_actorContextVars[current];
				float value = VectorUtils.HorizontalPlaneDistInSquares(src, current.GetTravelBoardSquareWorldPosition());
				actorHitContext._0015.SetFloat(ContextKeys._0018.GetHash(), value);
				ContextVars contextVars = actorHitContext._0015;
				int hash = TargetSelect_ConeOrLaser.s_cvarInCone.GetHash();
				int value2;
				if (updatingWithCone)
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
					if (updatingWithCone)
					{
						Vector3 to = current.GetTravelBoardSquareWorldPosition() - targetingActor.GetTravelBoardSquareWorldPosition();
						to.y = 0f;
						num = Vector3.Angle(aimDirection, to);
						actorHitContext._0015.SetFloat(ContextKeys._001D.GetHash(), num);
					}
				}
			}
		}
		if (!m_affectsTargetingActor || actors.Contains(targetingActor))
		{
			return;
		}
		if (m_customShouldAddCasterDelegate != null && !m_customShouldAddCasterDelegate(targetingActor, actors))
		{
			return;
		}
		AddActorInRange(targetingActor, src, targetingActor, AbilityTooltipSubject.Self);
	}

	private void CreateHighlights()
	{
		if (m_highlights != null && m_highlights.Count >= 3)
		{
			return;
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
		if (targetingActor != GameFlowData.Get().activeOwnedActorData)
		{
			return;
		}
		ResetSquareIndicatorIndexToUse();
		m_conePart.ShowHiddenSquares(m_indicatorHandler, coneStartPos, VectorUtils.HorizontalAngle_Deg(forwardDirection), targetingActor, m_coneInfo.m_penetrateLos);
		HideUnusedSquareIndicators();
	}

	private void DrawInvalidSquareIndicators_Laser(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos)
	{
		if (targetingActor != GameFlowData.Get().activeOwnedActorData)
		{
			return;
		}
		ResetSquareIndicatorIndexToUse();
		m_laserPart.ShowHiddenSquares(m_indicatorHandler, startPos, endPos, targetingActor, m_laserInfo.penetrateLos);
		HideUnusedSquareIndicators();
	}
}
