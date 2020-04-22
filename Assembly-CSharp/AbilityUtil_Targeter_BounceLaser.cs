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

	public int m_maxKnockbackTargets;

	public bool m_bounceOnActors;

	public bool m_penetrateTargetsAndHitCaster;

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
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 vector = originalStart + new Vector3(0f, y, 0f);
		Vector3 originalStart2 = vector;
		float num = m_width * Board.Get().squareSize;
		if (base.Highlight == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					base.Highlight = HighlightUtils.Get().CreateBouncingLaserCursor(originalStart2, laserAnglePoints, num);
					return;
				}
			}
		}
		UIBouncingLaserCursor component = base.Highlight.GetComponent<UIBouncingLaserCursor>();
		component.OnUpdated(originalStart2, laserAnglePoints, num);
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
		Vector3 vector;
		if (currentTarget == null)
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
			vector = targetingActor.transform.forward;
		}
		else
		{
			vector = currentTarget.AimDirection;
		}
		Vector3 forwardDirection = vector;
		float num = m_maxDistancePerBounce;
		if (m_extraDistancePerBounceDelegate != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			num += m_extraDistancePerBounceDelegate();
		}
		float num2 = m_maxTotalDistance;
		if (m_extraTotalDistanceDelegate != null)
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
			num2 += m_extraTotalDistanceDelegate();
		}
		int num3 = m_maxBounces;
		if (m_extraBouncesDelegate != null)
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
			num3 += Mathf.RoundToInt(m_extraBouncesDelegate());
		}
		int num4 = m_maxTargetsHit;
		if (m_ability is ScoundrelBouncingLaser && CollectTheCoins.Get() != null)
		{
			float bonus_Client = CollectTheCoins.Get().m_bouncingLaserTotalDistance.GetBonus_Client(targetingActor);
			float bonus_Client2 = CollectTheCoins.Get().m_bouncingLaserBounceDistance.GetBonus_Client(targetingActor);
			int num5 = Mathf.RoundToInt(CollectTheCoins.Get().m_bouncingLaserBounces.GetBonus_Client(targetingActor));
			int num6 = Mathf.RoundToInt(CollectTheCoins.Get().m_bouncingLaserPierces.GetBonus_Client(targetingActor));
			num2 += bonus_Client;
			num += bonus_Client2;
			num3 += num5;
			num4 += num6;
		}
		bool penetrateTargetsAndHitCaster = m_penetrateTargetsAndHitCaster;
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors;
		List<ActorData> orderedHitActors;
		List<Vector3> list = VectorUtils.CalculateBouncingLaserEndpoints(travelBoardSquareWorldPositionForLos, forwardDirection, num, num2, num3, targetingActor, m_width, num4, false, GetAffectedTeams(), m_bounceOnActors, out bounceHitActors, out orderedHitActors, null, penetrateTargetsAndHitCaster);
		if (penetrateTargetsAndHitCaster)
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
			if (list.Count > 1)
			{
				float totalMaxDistanceInSquares = num2 - (list[0] - travelBoardSquareWorldPositionForLos).magnitude / Board.Get().squareSize;
				Vector3 normalized = (list[1] - list[0]).normalized;
				VectorUtils.CalculateBouncingLaserEndpoints(list[0], normalized, num, totalMaxDistanceInSquares, num3, targetingActor, m_width, 0, false, targetingActor.GetTeams(), m_bounceOnActors, out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> _, out List<ActorData> orderedHitActors2, null, false, false);
				if (orderedHitActors2.Contains(targetingActor))
				{
					AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPositionForLos(), targetingActor, AbilityTooltipSubject.Self);
				}
			}
		}
		using (Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>.Enumerator enumerator = bounceHitActors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> current = enumerator.Current;
				AreaEffectUtils.BouncingLaserInfo value = current.Value;
				Vector3 segmentOrigin = value.m_segmentOrigin;
				AreaEffectUtils.BouncingLaserInfo value2 = current.Value;
				int endpointIndex = value2.m_endpointIndex;
				AddActorInRange(current.Key, segmentOrigin, targetingActor);
				if (endpointIndex > 0)
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
					SetIgnoreCoverMinDist(current.Key, true);
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		HitActorContext item = default(HitActorContext);
		for (int i = 0; i < orderedHitActors.Count; i++)
		{
			ActorData actorData = orderedHitActors[i];
			AreaEffectUtils.BouncingLaserInfo bouncingLaserInfo = bounceHitActors[orderedHitActors[i]];
			int endpointIndex2 = bouncingLaserInfo.m_endpointIndex;
			item.actor = actorData;
			item.segmentIndex = endpointIndex2;
			m_hitActorContext.Add(item);
			ActorHitContext actorHitContext = m_actorContextVars[actorData];
			actorHitContext._001D = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			actorHitContext._0015.SetInt(TargetSelect_BouncingLaser.s_cvarEndpointIndex.GetHash(), endpointIndex2);
			actorHitContext._0015.SetInt(TargetSelect_BouncingLaser.s_cvarHitOrder.GetHash(), i);
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			CreateLaserHighlights(travelBoardSquareWorldPositionForLos, list);
			if (targetingActor == GameFlowData.Get().activeOwnedActorData)
			{
				ResetSquareIndicatorIndexToUse();
				AreaEffectUtils.OperateOnSquaresInBounceLaser(m_indicatorHandler, travelBoardSquareWorldPositionForLos, list, m_width, targetingActor, false);
				HideUnusedSquareIndicators();
			}
			if (!(m_knockbackDistance > 0f))
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
				int num7 = 0;
				EnableAllMovementArrows();
				for (int j = 0; j < orderedHitActors.Count; j++)
				{
					ActorData actorData2 = orderedHitActors[j];
					if (actorData2.GetTeam() == targetingActor.GetTeam())
					{
						continue;
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_maxKnockbackTargets > 0)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (j >= m_maxKnockbackTargets)
						{
							continue;
						}
					}
					float num8;
					if (m_extraKnockdownDelegate != null)
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
						num8 = m_extraKnockdownDelegate(actorData2);
					}
					else
					{
						num8 = 0f;
					}
					float num9 = num8;
					AreaEffectUtils.BouncingLaserInfo bouncingLaserInfo2 = bounceHitActors[actorData2];
					Vector3 segmentOrigin2 = bouncingLaserInfo2.m_segmentOrigin;
					AreaEffectUtils.BouncingLaserInfo bouncingLaserInfo3 = bounceHitActors[actorData2];
					Vector3 aimDir = list[bouncingLaserInfo3.m_endpointIndex] - segmentOrigin2;
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData2, m_knockbackType, aimDir, segmentOrigin2, m_knockbackDistance + num9);
					num7 = AddMovementArrowWithPrevious(actorData2, path, TargeterMovementType.Knockback, num7);
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					SetMovementArrowEnabledFromIndex(num7, false);
					return;
				}
			}
		}
	}
}
