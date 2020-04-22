using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_CrossBeam : AbilityUtil_Targeter
{
	public struct HitActorContext
	{
		public ActorData actor;

		public int totalTargetsInLaser;
	}

	public float m_distanceInSquares;

	private float m_widthInSquares;

	private bool m_penetrateLoS;

	private int m_numLasers;

	private KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	private float m_knockbackDistance;

	private float m_knockbackThresholdDistance = -1f;

	private List<HitActorContext> m_hitActorContext = new List<HitActorContext>();

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public AbilityUtil_Targeter_CrossBeam(Ability ability, int numLasers, float distanceInSquares, float widthInSquares, bool penetrateLoS, bool includeAllies = false, bool affectsCaster = false)
		: base(ability)
	{
		m_numLasers = numLasers;
		m_distanceInSquares = distanceInSquares;
		m_widthInSquares = widthInSquares;
		m_penetrateLoS = penetrateLoS;
		m_affectsAllies = includeAllies;
		m_affectsTargetingActor = affectsCaster;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		for (int i = 0; i < numLasers; i++)
		{
			m_squarePosCheckerList.Add(new SquareInsideChecker_Box(widthInSquares));
		}
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
	}

	public List<HitActorContext> GetHitActorContext()
	{
		return m_hitActorContext;
	}

	public void SetKnockbackParams(float knockbackDistance, KnockbackType knockbackType, float knockbackThresholdDistance)
	{
		m_knockbackDistance = knockbackDistance;
		m_knockbackType = knockbackType;
		m_knockbackThresholdDistance = knockbackThresholdDistance;
	}

	private int GetNumLasers()
	{
		return Mathf.Max(1, m_numLasers);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		m_hitActorContext.Clear();
		if (m_highlights.Count != GetNumLasers())
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClearHighlightCursors();
			float squareSize = Board.Get().squareSize;
			float lengthInWorld = m_distanceInSquares * squareSize;
			float widthInWorld = m_widthInSquares * squareSize;
			for (int i = 0; i < GetNumLasers(); i++)
			{
				m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(widthInWorld, lengthInWorld));
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		List<Vector3> laserEndPoints = GetLaserEndPoints(currentTarget, targetingActor);
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		Vector3 position = travelBoardSquareWorldPosition + new Vector3(0f, 0.1f, 0f);
		for (int j = 0; j < laserEndPoints.Count; j++)
		{
			m_highlights[j].transform.position = position;
			m_highlights[j].transform.rotation = Quaternion.LookRotation(laserEndPoints[j] - travelBoardSquareWorldPositionForLos);
		}
		HitActorContext item = default(HitActorContext);
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			HashSet<ActorData> hashSet = new HashSet<ActorData>();
			for (int k = 0; k < laserEndPoints.Count; k++)
			{
				Vector3 vector = laserEndPoints[k];
				Vector3 a = vector - travelBoardSquareWorldPositionForLos;
				a.y = 0f;
				a.Normalize();
				Vector3 startPos = travelBoardSquareWorldPositionForLos + Board.Get().squareSize * a;
				List<ActorData> actors = AreaEffectUtils.GetActorsInBoxByActorRadius(startPos, vector, m_widthInSquares, m_penetrateLoS, targetingActor, GetAffectedTeams());
				TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
				List<ActorData> list = new List<ActorData>();
				using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData current = enumerator.Current;
						if (current != targetingActor)
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
							if (GetAffectsTarget(current, targetingActor))
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
								list.Add(current);
							}
						}
					}
					while (true)
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
						ActorData current2 = enumerator2.Current;
						if (!hashSet.Contains(current2))
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
							AddActorInRange(current2, travelBoardSquareWorldPositionForLos, targetingActor);
							item.actor = current2;
							item.totalTargetsInLaser = list.Count;
							m_hitActorContext.Add(item);
							hashSet.Add(current2);
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
				UpdateLaserEndPointsForHiddenSquares(startPos, vector, k, targetingActor);
			}
			if (m_affectsTargetingActor)
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
				AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPositionForLos(), targetingActor);
			}
			if (m_knockbackDistance > 0f)
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
				int num = 0;
				EnableAllMovementArrows();
				List<ActorData> visibleActorsInRange = GetVisibleActorsInRange();
				using (List<ActorData>.Enumerator enumerator3 = visibleActorsInRange.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						ActorData current3 = enumerator3.Current;
						if (current3.GetTeam() != targetingActor.GetTeam())
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
							if (ActorMeetKnockbackConditions(current3, targetingActor))
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
								BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(current3, m_knockbackType, currentTarget.AimDirection, targetingActor.GetTravelBoardSquareWorldPosition(), m_knockbackDistance);
								num = AddMovementArrowWithPrevious(current3, path, TargeterMovementType.Knockback, num);
							}
						}
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				SetMovementArrowEnabledFromIndex(num, false);
			}
			if (GameFlowData.Get().activeOwnedActorData == targetingActor)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					HandleShowSquareIndicators(targetingActor);
					return;
				}
			}
			return;
		}
	}

	private bool ActorMeetKnockbackConditions(ActorData target, ActorData caster)
	{
		if (m_knockbackDistance > 0f)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					int result;
					if (!(m_knockbackThresholdDistance <= 0f))
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
						result = ((VectorUtils.HorizontalPlaneDistInSquares(target.GetTravelBoardSquareWorldPosition(), caster.GetTravelBoardSquareWorldPosition()) < m_knockbackThresholdDistance) ? 1 : 0);
					}
					else
					{
						result = 1;
					}
					return (byte)result != 0;
				}
				}
			}
		}
		return false;
	}

	private List<Vector3> GetLaserDirections(AbilityTarget target, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		float num = VectorUtils.HorizontalAngle_Deg(target.AimDirection);
		int numLasers = GetNumLasers();
		float num2 = 360f / (float)numLasers;
		for (int i = 0; i < numLasers; i++)
		{
			Vector3 item = VectorUtils.AngleDegreesToVector(num + (float)i * num2);
			list.Add(item);
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	private List<Vector3> GetLaserEndPoints(AbilityTarget target, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
		float maxDistanceInWorld = m_distanceInSquares * Board.Get().squareSize;
		List<Vector3> laserDirections = GetLaserDirections(target, caster);
		using (List<Vector3>.Enumerator enumerator = laserDirections.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Vector3 current = enumerator.Current;
				Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(travelBoardSquareWorldPositionForLos, current, maxDistanceInWorld, m_penetrateLoS, caster);
				list.Add(laserEndPoint);
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return list;
					}
					/*OpCode not supported: LdMemberToken*/;
					return list;
				}
			}
		}
	}

	private void UpdateLaserEndPointsForHiddenSquares(Vector3 startPos, Vector3 endPos, int index, ActorData targetingActor)
	{
		SquareInsideChecker_Box squareInsideChecker_Box = m_squarePosCheckerList[index] as SquareInsideChecker_Box;
		squareInsideChecker_Box.UpdateBoxProperties(startPos, endPos, targetingActor);
	}

	private void HandleShowSquareIndicators(ActorData targetingActor)
	{
		ResetSquareIndicatorIndexToUse();
		for (int i = 0; i < m_squarePosCheckerList.Count; i++)
		{
			SquareInsideChecker_Box squareInsideChecker_Box = m_squarePosCheckerList[i] as SquareInsideChecker_Box;
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, squareInsideChecker_Box.GetStartPos(), squareInsideChecker_Box.GetEndPos(), m_widthInSquares, targetingActor, m_penetrateLoS, null, m_squarePosCheckerList, false);
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			HideUnusedSquareIndicators();
			return;
		}
	}
}
