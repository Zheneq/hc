using AbilityContextNamespace;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class AbilityUtil_Targeter_BounceLaser : AbilityUtil_Targeter
{
	public struct HitActorContext
	{
		public ActorData actor;

		public int segmentIndex;
	}

	public delegate float FloatAccessorDelegate();
	public delegate float ExtraKnockbackDelegate(ActorData hitActor);

	public float m_width = 1f;
	public float m_maxDistancePerBounce = 15f;
	public float m_maxTotalDistance = 50f;
	public int m_maxBounces = 5;
	public int m_maxTargetsHit = 1;
	public bool m_bounceOnActors;

	public bool m_penetrateTargetsAndHitCaster;

	public int m_maxKnockbackTargets;
	public float m_knockbackDistance;
	public KnockbackType m_knockbackType;

	private List<HitActorContext> m_hitActorContext = new List<HitActorContext>();
	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public FloatAccessorDelegate m_extraTotalDistanceDelegate;
	public FloatAccessorDelegate m_extraDistancePerBounceDelegate;
	public FloatAccessorDelegate m_extraBouncesDelegate;
	public ExtraKnockbackDelegate m_extraKnockdownDelegate;

	public AbilityUtil_Targeter_BounceLaser(Ability ability, float width, float distancePerBounce, float totalDistance, int maxBounces, int maxTargetsHit, bool bounceOnActors)
		: base(ability)
	{
		m_width = width;
		m_maxDistancePerBounce = distancePerBounce;
		m_maxTotalDistance = totalDistance;
		m_maxBounces = maxBounces;
		m_maxTargetsHit = maxTargetsHit;
		m_bounceOnActors = bounceOnActors;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public ReadOnlyCollection<HitActorContext> GetHitActorContext()
	{
		return m_hitActorContext.AsReadOnly();
	}

	public void SetMaxBounces(int maxBounces)
	{
		m_maxBounces = maxBounces;
	}

	public void SetMaxTargets(int maxTargets)
	{
		m_maxTargetsHit = maxTargets;
	}

	public void InitKnockbackData(float knockbackDistance, KnockbackType knockbackType, int maxKnockbacks, ExtraKnockbackDelegate extraKnockdownDelegate)
	{
		m_knockbackDistance = knockbackDistance;
		m_knockbackType = knockbackType;
		m_maxKnockbackTargets = maxKnockbacks;
		m_extraKnockdownDelegate = extraKnockdownDelegate;
	}

	public void SetTargeterRangeDelegates(FloatAccessorDelegate extraDistance, FloatAccessorDelegate extraDistancePerBounce, FloatAccessorDelegate extraBounces)
	{
		m_extraTotalDistanceDelegate = extraDistance;
		m_extraDistancePerBounceDelegate = extraDistancePerBounce;
		m_extraBouncesDelegate = extraBounces;
	}

	public void CreateLaserHighlights(Vector3 originalStart, List<Vector3> laserAnglePoints)
	{
		Vector3 start = originalStart + new Vector3(0f, 0.1f - BoardSquare.s_LoSHeightOffset, 0f);
		float width = m_width * Board.Get().squareSize;
		if (Highlight == null)
		{
			Highlight = HighlightUtils.Get().CreateBouncingLaserCursor(start, laserAnglePoints, width);
		}
		else
		{
			Highlight.GetComponent<UIBouncingLaserCursor>().OnUpdated(start, laserAnglePoints, width);
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		m_hitActorContext.Clear();
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 aimDirection = currentTarget?.AimDirection ?? targetingActor.transform.forward;
		float maxDistancePerBounce = m_maxDistancePerBounce + (m_extraDistancePerBounceDelegate != null ? m_extraDistancePerBounceDelegate() : 0f);
		float maxTotalDistance = m_maxTotalDistance + (m_extraTotalDistanceDelegate != null ? m_extraTotalDistanceDelegate() : 0f);
		int maxBounces = m_maxBounces + (m_extraBouncesDelegate != null ? Mathf.RoundToInt(m_extraBouncesDelegate()) : 0);
		int maxTargetsHit = m_maxTargetsHit;
		if (m_ability is ScoundrelBouncingLaser && CollectTheCoins.Get() != null)
		{
			maxTotalDistance += CollectTheCoins.Get().m_bouncingLaserTotalDistance.GetBonus_Client(targetingActor);
			maxDistancePerBounce += CollectTheCoins.Get().m_bouncingLaserBounceDistance.GetBonus_Client(targetingActor);
			maxBounces += Mathf.RoundToInt(CollectTheCoins.Get().m_bouncingLaserBounces.GetBonus_Client(targetingActor));
			maxTargetsHit += Mathf.RoundToInt(CollectTheCoins.Get().m_bouncingLaserPierces.GetBonus_Client(targetingActor));
		}
		bool penetrateTargetsAndHitCaster = m_penetrateTargetsAndHitCaster;
		List<Vector3> endpoints = VectorUtils.CalculateBouncingLaserEndpoints(
			travelBoardSquareWorldPositionForLos,
			aimDirection,
			maxDistancePerBounce,
			maxTotalDistance,
			maxBounces,
			targetingActor,
			m_width,
			maxTargetsHit,
			false,
			GetAffectedTeams(),
			m_bounceOnActors,
			out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors,
			out List<ActorData> orderedHitActors,
			null,
			penetrateTargetsAndHitCaster);
		if (penetrateTargetsAndHitCaster && endpoints.Count > 1)
		{
			float totalMaxDistanceInSquares = maxTotalDistance - (endpoints[0] - travelBoardSquareWorldPositionForLos).magnitude / Board.Get().squareSize;
			Vector3 normalized = (endpoints[1] - endpoints[0]).normalized;
			VectorUtils.CalculateBouncingLaserEndpoints(
				endpoints[0],
				normalized,
				maxDistancePerBounce,
				totalMaxDistanceInSquares,
				maxBounces,
				targetingActor,
				m_width,
				0,
				false,
				targetingActor.GetTeams(),
				m_bounceOnActors,
				out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> _,
				out List<ActorData> orderedHitActors2,
				null,
				false,
				false);
			if (orderedHitActors2.Contains(targetingActor))
			{
				AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPositionForLos(), targetingActor, AbilityTooltipSubject.Self);
			}
		}
		foreach (var hitActor in bounceHitActors)
		{
			AddActorInRange(hitActor.Key, hitActor.Value.m_segmentOrigin, targetingActor);
			if (hitActor.Value.m_endpointIndex > 0)
			{
				SetIgnoreCoverMinDist(hitActor.Key, true);
			}
		}
		
		HitActorContext item = default(HitActorContext);
		for (int i = 0; i < orderedHitActors.Count; i++)
		{
			ActorData hitActor = orderedHitActors[i];
			AreaEffectUtils.BouncingLaserInfo bouncingLaserInfo = bounceHitActors[hitActor];
			item.actor = hitActor;
			item.segmentIndex = bouncingLaserInfo.m_endpointIndex;
			m_hitActorContext.Add(item);
			ActorHitContext actorHitContext = m_actorContextVars[hitActor];
			actorHitContext.source = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			actorHitContext.context.SetInt(TargetSelect_BouncingLaser.s_cvarEndpointIndex.GetKey(), bouncingLaserInfo.m_endpointIndex);
			actorHitContext.context.SetInt(TargetSelect_BouncingLaser.s_cvarHitOrder.GetKey(), i);
		}

		CreateLaserHighlights(travelBoardSquareWorldPositionForLos, endpoints);
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBounceLaser(m_indicatorHandler, travelBoardSquareWorldPositionForLos, endpoints, m_width, targetingActor, false);
			HideUnusedSquareIndicators();
		}

		if (m_knockbackDistance > 0f)
		{
			int movementArrowIndex = 0;
			EnableAllMovementArrows();
			for (int i = 0; i < orderedHitActors.Count; i++)
			{
				ActorData hitActor = orderedHitActors[i];
				if (hitActor.GetTeam() == targetingActor.GetTeam() ||
					m_maxKnockbackTargets > 0 && i >= m_maxKnockbackTargets)
				{
					continue;
				}
				float knockbackDistance = m_knockbackDistance + (m_extraKnockdownDelegate != null ? m_extraKnockdownDelegate(hitActor) : 0f);
				AreaEffectUtils.BouncingLaserInfo bouncingLaserInfo = bounceHitActors[hitActor];
				Vector3 aimDir = endpoints[bouncingLaserInfo.m_endpointIndex] - bouncingLaserInfo.m_segmentOrigin;
				BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(hitActor, m_knockbackType, aimDir, bouncingLaserInfo.m_segmentOrigin, knockbackDistance);
				movementArrowIndex = AddMovementArrowWithPrevious(hitActor, path, TargeterMovementType.Knockback, movementArrowIndex);
			}
			SetMovementArrowEnabledFromIndex(movementArrowIndex, false);
		}
		
	}
}
