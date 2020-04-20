using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_CrossBeam : AbilityUtil_Targeter
{
	public float m_distanceInSquares;

	private float m_widthInSquares;

	private bool m_penetrateLoS;

	private int m_numLasers;

	private KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	private float m_knockbackDistance;

	private float m_knockbackThresholdDistance = -1f;

	private List<AbilityUtil_Targeter_CrossBeam.HitActorContext> m_hitActorContext = new List<AbilityUtil_Targeter_CrossBeam.HitActorContext>();

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public AbilityUtil_Targeter_CrossBeam(Ability ability, int numLasers, float distanceInSquares, float widthInSquares, bool penetrateLoS, bool includeAllies = false, bool affectsCaster = false) : base(ability)
	{
		this.m_numLasers = numLasers;
		this.m_distanceInSquares = distanceInSquares;
		this.m_widthInSquares = widthInSquares;
		this.m_penetrateLoS = penetrateLoS;
		this.m_affectsAllies = includeAllies;
		this.m_affectsTargetingActor = affectsCaster;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		for (int i = 0; i < numLasers; i++)
		{
			this.m_squarePosCheckerList.Add(new SquareInsideChecker_Box(widthInSquares));
		}
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public List<AbilityUtil_Targeter_CrossBeam.HitActorContext> GetHitActorContext()
	{
		return this.m_hitActorContext;
	}

	public void SetKnockbackParams(float knockbackDistance, KnockbackType knockbackType, float knockbackThresholdDistance)
	{
		this.m_knockbackDistance = knockbackDistance;
		this.m_knockbackType = knockbackType;
		this.m_knockbackThresholdDistance = knockbackThresholdDistance;
	}

	private int GetNumLasers()
	{
		return Mathf.Max(1, this.m_numLasers);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		this.m_hitActorContext.Clear();
		if (this.m_highlights.Count != this.GetNumLasers())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_CrossBeam.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			this.ClearHighlightCursors(true);
			float squareSize = Board.Get().squareSize;
			float lengthInWorld = this.m_distanceInSquares * squareSize;
			float widthInWorld = this.m_widthInSquares * squareSize;
			for (int i = 0; i < this.GetNumLasers(); i++)
			{
				this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(widthInWorld, lengthInWorld, null));
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		List<Vector3> laserEndPoints = this.GetLaserEndPoints(currentTarget, targetingActor);
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		Vector3 position = travelBoardSquareWorldPosition + new Vector3(0f, 0.1f, 0f);
		for (int j = 0; j < laserEndPoints.Count; j++)
		{
			this.m_highlights[j].transform.position = position;
			this.m_highlights[j].transform.rotation = Quaternion.LookRotation(laserEndPoints[j] - travelBoardSquareWorldPositionForLos);
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		HashSet<ActorData> hashSet = new HashSet<ActorData>();
		for (int k = 0; k < laserEndPoints.Count; k++)
		{
			Vector3 vector = laserEndPoints[k];
			Vector3 a = vector - travelBoardSquareWorldPositionForLos;
			a.y = 0f;
			a.Normalize();
			Vector3 startPos = travelBoardSquareWorldPositionForLos + Board.Get().squareSize * a;
			List<ActorData> actorsInBoxByActorRadius = AreaEffectUtils.GetActorsInBoxByActorRadius(startPos, vector, this.m_widthInSquares, this.m_penetrateLoS, targetingActor, base.GetAffectedTeams(), null, null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInBoxByActorRadius);
			List<ActorData> list = new List<ActorData>();
			using (List<ActorData>.Enumerator enumerator = actorsInBoxByActorRadius.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					if (actorData != targetingActor)
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
						bool affectsTarget = base.GetAffectsTarget(actorData, targetingActor);
						if (affectsTarget)
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
							list.Add(actorData);
						}
					}
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
			}
			using (List<ActorData>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData actorData2 = enumerator2.Current;
					if (!hashSet.Contains(actorData2))
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
						base.AddActorInRange(actorData2, travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Primary, false);
						AbilityUtil_Targeter_CrossBeam.HitActorContext item;
						item.actor = actorData2;
						item.totalTargetsInLaser = list.Count;
						this.m_hitActorContext.Add(item);
						hashSet.Add(actorData2);
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
			this.UpdateLaserEndPointsForHiddenSquares(startPos, vector, k, targetingActor);
		}
		if (this.m_affectsTargetingActor)
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
			base.AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPositionForLos(), targetingActor, AbilityTooltipSubject.Primary, false);
		}
		if (this.m_knockbackDistance > 0f)
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
			int num = 0;
			base.EnableAllMovementArrows();
			List<ActorData> visibleActorsInRange = this.GetVisibleActorsInRange();
			using (List<ActorData>.Enumerator enumerator3 = visibleActorsInRange.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					ActorData actorData3 = enumerator3.Current;
					if (actorData3.GetTeam() != targetingActor.GetTeam())
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
						if (this.ActorMeetKnockbackConditions(actorData3, targetingActor))
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
							BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData3, this.m_knockbackType, currentTarget.AimDirection, targetingActor.GetTravelBoardSquareWorldPosition(), this.m_knockbackDistance);
							num = base.AddMovementArrowWithPrevious(actorData3, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
						}
					}
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			base.SetMovementArrowEnabledFromIndex(num, false);
		}
		if (GameFlowData.Get().activeOwnedActorData == targetingActor)
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
			this.HandleShowSquareIndicators(targetingActor);
		}
	}

	private bool ActorMeetKnockbackConditions(ActorData target, ActorData caster)
	{
		if (this.m_knockbackDistance > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_CrossBeam.ActorMeetKnockbackConditions(ActorData, ActorData)).MethodHandle;
			}
			bool result;
			if (this.m_knockbackThresholdDistance > 0f)
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
				result = (VectorUtils.HorizontalPlaneDistInSquares(target.GetTravelBoardSquareWorldPosition(), caster.GetTravelBoardSquareWorldPosition()) < this.m_knockbackThresholdDistance);
			}
			else
			{
				result = true;
			}
			return result;
		}
		return false;
	}

	private List<Vector3> GetLaserDirections(AbilityTarget target, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		float num = VectorUtils.HorizontalAngle_Deg(target.AimDirection);
		int numLasers = this.GetNumLasers();
		float num2 = 360f / (float)numLasers;
		for (int i = 0; i < numLasers; i++)
		{
			Vector3 item = VectorUtils.AngleDegreesToVector(num + (float)i * num2);
			list.Add(item);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_CrossBeam.GetLaserDirections(AbilityTarget, ActorData)).MethodHandle;
		}
		return list;
	}

	private List<Vector3> GetLaserEndPoints(AbilityTarget target, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
		float maxDistanceInWorld = this.m_distanceInSquares * Board.Get().squareSize;
		List<Vector3> laserDirections = this.GetLaserDirections(target, caster);
		using (List<Vector3>.Enumerator enumerator = laserDirections.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Vector3 dir = enumerator.Current;
				Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(travelBoardSquareWorldPositionForLos, dir, maxDistanceInWorld, this.m_penetrateLoS, caster, null, true);
				list.Add(laserEndPoint);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_CrossBeam.GetLaserEndPoints(AbilityTarget, ActorData)).MethodHandle;
			}
		}
		return list;
	}

	private void UpdateLaserEndPointsForHiddenSquares(Vector3 startPos, Vector3 endPos, int index, ActorData targetingActor)
	{
		SquareInsideChecker_Box squareInsideChecker_Box = this.m_squarePosCheckerList[index] as SquareInsideChecker_Box;
		squareInsideChecker_Box.UpdateBoxProperties(startPos, endPos, targetingActor);
	}

	private void HandleShowSquareIndicators(ActorData targetingActor)
	{
		base.ResetSquareIndicatorIndexToUse();
		for (int i = 0; i < this.m_squarePosCheckerList.Count; i++)
		{
			SquareInsideChecker_Box squareInsideChecker_Box = this.m_squarePosCheckerList[i] as SquareInsideChecker_Box;
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, squareInsideChecker_Box.GetStartPos(), squareInsideChecker_Box.GetEndPos(), this.m_widthInSquares, targetingActor, this.m_penetrateLoS, null, this.m_squarePosCheckerList, false);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_CrossBeam.HandleShowSquareIndicators(ActorData)).MethodHandle;
		}
		base.HideUnusedSquareIndicators();
	}

	public struct HitActorContext
	{
		public ActorData actor;

		public int totalTargetsInLaser;
	}
}
