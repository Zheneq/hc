using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class AbilityUtil_Targeter_FanOfBouncingLasers : AbilityUtil_Targeter
{
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

	private List<AbilityUtil_Targeter_FanOfBouncingLasers.HitActorContext> m_hitActorContext = new List<AbilityUtil_Targeter_FanOfBouncingLasers.HitActorContext>();

	public AbilityUtil_Targeter_FanOfBouncingLasers(Ability ability, float angle, float rangePerBounceInSquares, float rangeInSquares, float widthInSquares, int bounces, int maxTargets, int count) : base(ability)
	{
		this.m_angle = Mathf.Max(0f, angle);
		this.m_distancePerBounce = rangePerBounceInSquares;
		this.m_totalDistance = rangeInSquares;
		this.m_widthInSquares = widthInSquares;
		this.m_bounces = bounces;
		this.m_count = count;
		this.m_maxTargets = maxTargets;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_hitActorInLaser = new List<bool>();
	}

	public ReadOnlyCollection<AbilityUtil_Targeter_FanOfBouncingLasers.HitActorContext> GetHitActorContext()
	{
		return this.m_hitActorContext.AsReadOnly();
	}

	public void SetIncludeTeams(bool includeAllies, bool includeEnemies, bool includeSelf = false)
	{
		this.m_affectsAllies = includeAllies;
		this.m_affectsEnemies = includeEnemies;
		this.m_affectsTargetingActor = includeSelf;
	}

	public void CreateLaserHighlights(Vector3 originalStart, List<Vector3> laserAnglePoints, int laserIndex)
	{
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 vector = originalStart + new Vector3(0f, y, 0f);
		Vector3 originalStart2 = vector;
		float num = this.m_widthInSquares * Board.\u000E().squareSize;
		if (this.m_highlights.Count <= laserIndex)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateBouncingLaserCursor(originalStart2, laserAnglePoints, num));
		}
		else
		{
			UIBouncingLaserCursor component = this.m_highlights[laserIndex].GetComponent<UIBouncingLaserCursor>();
			component.OnUpdated(originalStart2, laserAnglePoints, num);
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.m_hitActorContext.Clear();
		base.ClearActorsInRange();
		float num;
		if (this.m_count > 1)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_FanOfBouncingLasers.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			num = this.m_angle;
		}
		else
		{
			num = 0f;
		}
		float num2 = num;
		float num3;
		if (this.m_count > 1)
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
			num3 = num2 / (float)(this.m_count - 1);
		}
		else
		{
			num3 = 0f;
		}
		float num4 = num3;
		float num5 = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection) - 0.5f * num2;
		this.m_hitActorInLaser.Clear();
		if (this.m_affectsTargetingActor)
		{
			base.AddActorInRange(targetingActor, targetingActor.\u0016(), targetingActor, AbilityTooltipSubject.Self, false);
		}
		Vector3 vector = targetingActor.\u0015();
		for (int i = 0; i < this.m_count; i++)
		{
			Vector3 forwardDirection = VectorUtils.AngleDegreesToVector(num5 + (float)i * num4);
			Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary;
			List<ActorData> list;
			List<Vector3> laserAnglePoints = VectorUtils.CalculateBouncingLaserEndpoints(vector, forwardDirection, this.m_distancePerBounce, this.m_totalDistance, this.m_bounces, targetingActor, this.m_widthInSquares, this.m_maxTargets, false, base.GetAffectedTeams(), false, out dictionary, out list, null, false, true);
			foreach (ActorData actorData in list)
			{
				base.AddActorInRange(actorData, dictionary[actorData].m_segmentOrigin, targetingActor, AbilityTooltipSubject.Primary, true);
				if (dictionary[actorData].m_endpointIndex > 0)
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
					base.SetIgnoreCoverMinDist(actorData, true);
				}
				AbilityUtil_Targeter_FanOfBouncingLasers.HitActorContext item;
				item.actor = actorData;
				item.laserIndex = i;
				item.segmentIndex = dictionary[actorData].m_endpointIndex;
				this.m_hitActorContext.Add(item);
			}
			this.m_hitActorInLaser.Add(list.Count > 0);
			this.CreateLaserHighlights(vector, laserAnglePoints, i);
			if (targetingActor == GameFlowData.Get().activeOwnedActorData)
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
				base.ResetSquareIndicatorIndexToUse();
				AreaEffectUtils.OperateOnSquaresInBounceLaser(this.m_indicatorHandler, vector, laserAnglePoints, this.m_widthInSquares, targetingActor, false);
				base.HideUnusedSquareIndicators();
			}
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

	public struct HitActorContext
	{
		public ActorData actor;

		public int laserIndex;

		public int segmentIndex;
	}
}
