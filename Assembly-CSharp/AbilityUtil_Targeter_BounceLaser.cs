using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_BounceLaser : AbilityUtil_Targeter
{
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

	private List<AbilityUtil_Targeter_BounceLaser.HitActorContext> m_hitActorContext = new List<AbilityUtil_Targeter_BounceLaser.HitActorContext>();

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_BounceLaser.FloatAccessorDelegate m_extraTotalDistanceDelegate;

	public AbilityUtil_Targeter_BounceLaser.FloatAccessorDelegate m_extraDistancePerBounceDelegate;

	public AbilityUtil_Targeter_BounceLaser.FloatAccessorDelegate m_extraBouncesDelegate;

	public AbilityUtil_Targeter_BounceLaser.ExtraKnockbackDelegate m_extraKnockdownDelegate;

	public AbilityUtil_Targeter_BounceLaser(Ability ability, float width, float distancePerBounce, float totalDistance, int maxBounces, int maxTargetsHit, bool bounceOnActors) : base(ability)
	{
		this.m_width = width;
		this.m_maxDistancePerBounce = distancePerBounce;
		this.m_maxTotalDistance = totalDistance;
		this.m_maxBounces = maxBounces;
		this.m_maxTargetsHit = maxTargetsHit;
		this.m_bounceOnActors = bounceOnActors;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> GetHitActorContext()
	{
		return this.m_hitActorContext.AsReadOnly();
	}

	public void SetMaxBounces(int maxBounces)
	{
		this.m_maxBounces = maxBounces;
	}

	public void SetMaxTargets(int maxTargets)
	{
		this.m_maxTargetsHit = maxTargets;
	}

	public void InitKnockbackData(float knockbackDistance, KnockbackType knockbackType, int maxKnockbacks, AbilityUtil_Targeter_BounceLaser.ExtraKnockbackDelegate extraKnockdownDelegate)
	{
		this.m_knockbackDistance = knockbackDistance;
		this.m_knockbackType = knockbackType;
		this.m_maxKnockbackTargets = maxKnockbacks;
		this.m_extraKnockdownDelegate = extraKnockdownDelegate;
	}

	public void SetTargeterRangeDelegates(AbilityUtil_Targeter_BounceLaser.FloatAccessorDelegate extraDistance, AbilityUtil_Targeter_BounceLaser.FloatAccessorDelegate extraDistancePerBounce, AbilityUtil_Targeter_BounceLaser.FloatAccessorDelegate extraBounces)
	{
		this.m_extraTotalDistanceDelegate = extraDistance;
		this.m_extraDistancePerBounceDelegate = extraDistancePerBounce;
		this.m_extraBouncesDelegate = extraBounces;
	}

	public void CreateLaserHighlights(Vector3 originalStart, List<Vector3> laserAnglePoints)
	{
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 vector = originalStart + new Vector3(0f, y, 0f);
		Vector3 originalStart2 = vector;
		float num = this.m_width * Board.\u000E().squareSize;
		if (base.Highlight == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BounceLaser.CreateLaserHighlights(Vector3, List<Vector3>)).MethodHandle;
			}
			base.Highlight = HighlightUtils.Get().CreateBouncingLaserCursor(originalStart2, laserAnglePoints, num);
		}
		else
		{
			UIBouncingLaserCursor component = base.Highlight.GetComponent<UIBouncingLaserCursor>();
			component.OnUpdated(originalStart2, laserAnglePoints, num);
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.ClearActorsInRange();
		this.m_hitActorContext.Clear();
		Vector3 vector = targetingActor.\u0015();
		Vector3 vector2;
		if (currentTarget == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BounceLaser.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
			}
			vector2 = targetingActor.transform.forward;
		}
		else
		{
			vector2 = currentTarget.AimDirection;
		}
		Vector3 forwardDirection = vector2;
		float num = this.m_maxDistancePerBounce;
		if (this.m_extraDistancePerBounceDelegate != null)
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
			num += this.m_extraDistancePerBounceDelegate();
		}
		float num2 = this.m_maxTotalDistance;
		if (this.m_extraTotalDistanceDelegate != null)
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
			num2 += this.m_extraTotalDistanceDelegate();
		}
		int num3 = this.m_maxBounces;
		if (this.m_extraBouncesDelegate != null)
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
			num3 += Mathf.RoundToInt(this.m_extraBouncesDelegate());
		}
		int num4 = this.m_maxTargetsHit;
		if (this.m_ability is ScoundrelBouncingLaser && CollectTheCoins.Get() != null)
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
		bool penetrateTargetsAndHitCaster = this.m_penetrateTargetsAndHitCaster;
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary;
		List<ActorData> list2;
		List<Vector3> list = VectorUtils.CalculateBouncingLaserEndpoints(vector, forwardDirection, num, num2, num3, targetingActor, this.m_width, num4, false, base.GetAffectedTeams(), this.m_bounceOnActors, out dictionary, out list2, null, penetrateTargetsAndHitCaster, true);
		if (penetrateTargetsAndHitCaster)
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
			if (list.Count > 1)
			{
				float totalMaxDistanceInSquares = num2 - (list[0] - vector).magnitude / Board.\u000E().squareSize;
				Vector3 normalized = (list[1] - list[0]).normalized;
				Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary2;
				List<ActorData> list3;
				VectorUtils.CalculateBouncingLaserEndpoints(list[0], normalized, num, totalMaxDistanceInSquares, num3, targetingActor, this.m_width, 0, false, targetingActor.\u0012(), this.m_bounceOnActors, out dictionary2, out list3, null, false, false);
				bool flag = list3.Contains(targetingActor);
				if (flag)
				{
					base.AddActorInRange(targetingActor, targetingActor.\u0015(), targetingActor, AbilityTooltipSubject.Self, false);
				}
			}
		}
		using (Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> keyValuePair = enumerator.Current;
				Vector3 segmentOrigin = keyValuePair.Value.m_segmentOrigin;
				int endpointIndex = keyValuePair.Value.m_endpointIndex;
				base.AddActorInRange(keyValuePair.Key, segmentOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
				if (endpointIndex > 0)
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
					base.SetIgnoreCoverMinDist(keyValuePair.Key, true);
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
		for (int i = 0; i < list2.Count; i++)
		{
			ActorData actorData = list2[i];
			int endpointIndex2 = dictionary[list2[i]].m_endpointIndex;
			AbilityUtil_Targeter_BounceLaser.HitActorContext item;
			item.actor = actorData;
			item.segmentIndex = endpointIndex2;
			this.m_hitActorContext.Add(item);
			ActorHitContext actorHitContext = this.m_actorContextVars[actorData];
			actorHitContext.\u001D = targetingActor.\u0015();
			actorHitContext.\u0015.\u0016(TargetSelect_BouncingLaser.s_cvarEndpointIndex.\u0012(), endpointIndex2);
			actorHitContext.\u0015.\u0016(TargetSelect_BouncingLaser.s_cvarHitOrder.\u0012(), i);
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
		this.CreateLaserHighlights(vector, list);
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBounceLaser(this.m_indicatorHandler, vector, list, this.m_width, targetingActor, false);
			base.HideUnusedSquareIndicators();
		}
		if (this.m_knockbackDistance > 0f)
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
			int num7 = 0;
			base.EnableAllMovementArrows();
			for (int j = 0; j < list2.Count; j++)
			{
				ActorData actorData2 = list2[j];
				if (actorData2.\u000E() != targetingActor.\u000E())
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
					if (this.m_maxKnockbackTargets > 0)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (j >= this.m_maxKnockbackTargets)
						{
							goto IL_504;
						}
					}
					float num8;
					if (this.m_extraKnockdownDelegate != null)
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
						num8 = this.m_extraKnockdownDelegate(actorData2);
					}
					else
					{
						num8 = 0f;
					}
					float num9 = num8;
					Vector3 segmentOrigin2 = dictionary[actorData2].m_segmentOrigin;
					Vector3 aimDir = list[dictionary[actorData2].m_endpointIndex] - segmentOrigin2;
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData2, this.m_knockbackType, aimDir, segmentOrigin2, this.m_knockbackDistance + num9);
					num7 = base.AddMovementArrowWithPrevious(actorData2, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num7, false);
				}
				IL_504:;
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
			base.SetMovementArrowEnabledFromIndex(num7, false);
		}
	}

	public struct HitActorContext
	{
		public ActorData actor;

		public int segmentIndex;
	}

	public delegate float FloatAccessorDelegate();

	public delegate float ExtraKnockbackDelegate(ActorData hitActor);
}
