using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class AbilityUtil_Targeter_FanOfBouncingLasers : AbilityUtil_Targeter
{
	public struct HitActorContext
	{
		public ActorData actor;

		public int laserIndex;

		public int segmentIndex;
	}

	private float m_angle;

	private int m_count;

	private float m_distancePerBounce;

	private float m_totalDistance;

	private int m_bounces;

	private float m_widthInSquares;

	private int m_maxTargets;

	public List<bool> m_hitActorInLaser;

	private List<ActorData> m_actorsHitSoFar = new List<ActorData>();

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<HitActorContext> m_hitActorContext = new List<HitActorContext>();

	public AbilityUtil_Targeter_FanOfBouncingLasers(Ability ability, float angle, float rangePerBounceInSquares, float rangeInSquares, float widthInSquares, int bounces, int maxTargets, int count)
		: base(ability)
	{
		m_angle = Mathf.Max(0f, angle);
		m_distancePerBounce = rangePerBounceInSquares;
		m_totalDistance = rangeInSquares;
		m_widthInSquares = widthInSquares;
		m_bounces = bounces;
		m_count = count;
		m_maxTargets = maxTargets;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_hitActorInLaser = new List<bool>();
	}

	public ReadOnlyCollection<HitActorContext> GetHitActorContext()
	{
		return m_hitActorContext.AsReadOnly();
	}

	public void SetIncludeTeams(bool includeAllies, bool includeEnemies, bool includeSelf = false)
	{
		m_affectsAllies = includeAllies;
		m_affectsEnemies = includeEnemies;
		m_affectsTargetingActor = includeSelf;
	}

	public void CreateLaserHighlights(Vector3 originalStart, List<Vector3> laserAnglePoints, int laserIndex)
	{
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 vector = originalStart + new Vector3(0f, y, 0f);
		Vector3 originalStart2 = vector;
		float num = m_widthInSquares * Board.Get().squareSize;
		if (m_highlights.Count <= laserIndex)
		{
			m_highlights.Add(HighlightUtils.Get().CreateBouncingLaserCursor(originalStart2, laserAnglePoints, num));
			return;
		}
		UIBouncingLaserCursor component = m_highlights[laserIndex].GetComponent<UIBouncingLaserCursor>();
		component.OnUpdated(originalStart2, laserAnglePoints, num);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		m_hitActorContext.Clear();
		ClearActorsInRange();
		float num;
		if (m_count > 1)
		{
			num = m_angle;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		float num3;
		if (m_count > 1)
		{
			num3 = num2 / (float)(m_count - 1);
		}
		else
		{
			num3 = 0f;
		}
		float num4 = num3;
		float num5 = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection) - 0.5f * num2;
		m_hitActorInLaser.Clear();
		if (m_affectsTargetingActor)
		{
			AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self);
		}
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetLoSCheckPos();
		HitActorContext item = default(HitActorContext);
		for (int i = 0; i < m_count; i++)
		{
			Vector3 forwardDirection = VectorUtils.AngleDegreesToVector(num5 + (float)i * num4);
			Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors;
			List<ActorData> orderedHitActors;
			List<Vector3> laserAnglePoints = VectorUtils.CalculateBouncingLaserEndpoints(travelBoardSquareWorldPositionForLos, forwardDirection, m_distancePerBounce, m_totalDistance, m_bounces, targetingActor, m_widthInSquares, m_maxTargets, false, GetAffectedTeams(), false, out bounceHitActors, out orderedHitActors, null);
			foreach (ActorData item2 in orderedHitActors)
			{
				AreaEffectUtils.BouncingLaserInfo bouncingLaserInfo = bounceHitActors[item2];
				AddActorInRange(item2, bouncingLaserInfo.m_segmentOrigin, targetingActor, AbilityTooltipSubject.Primary, true);
				AreaEffectUtils.BouncingLaserInfo bouncingLaserInfo2 = bounceHitActors[item2];
				if (bouncingLaserInfo2.m_endpointIndex > 0)
				{
					SetIgnoreCoverMinDist(item2, true);
				}
				item.actor = item2;
				item.laserIndex = i;
				AreaEffectUtils.BouncingLaserInfo bouncingLaserInfo3 = bounceHitActors[item2];
				item.segmentIndex = bouncingLaserInfo3.m_endpointIndex;
				m_hitActorContext.Add(item);
			}
			m_hitActorInLaser.Add(orderedHitActors.Count > 0);
			CreateLaserHighlights(travelBoardSquareWorldPositionForLos, laserAnglePoints, i);
			if (targetingActor == GameFlowData.Get().activeOwnedActorData)
			{
				ResetSquareIndicatorIndexToUse();
				AreaEffectUtils.OperateOnSquaresInBounceLaser(m_indicatorHandler, travelBoardSquareWorldPositionForLos, laserAnglePoints, m_widthInSquares, targetingActor, false);
				HideUnusedSquareIndicators();
			}
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
