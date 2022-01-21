using AbilityContextNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ThiefFanLaser : AbilityUtil_Targeter
{
	public delegate bool IsAffectingCasterDelegate(ActorData caster, bool hitEnemy, bool powerupHit);

	public delegate Vector3 CustomDamageOriginDelegate(ActorData potentialActor, ActorData caster, Vector3 defaultPos);

	public delegate float LaserLengthDelegate(ActorData caster, float baseLength);

	public delegate float LaserWidthDelegate(ActorData caster, float baseWidth);

	private float m_minAngle;

	private float m_maxAngle;

	protected float m_fixedAngleInBetween = 10f;

	protected bool m_changeAngleByCursorDist = true;

	protected float m_interpMinDistanceInSquares;

	protected float m_interpMaxDistanceInSquares;

	protected float m_interpStep;

	protected float m_startAngleOffset;

	protected int m_count;

	public float m_rangeInSquares;

	protected float m_widthInSquares;

	private int m_maxTargets;

	protected bool m_penetrateLos;

	private bool m_highlightPowerup;

	private bool m_stopOnPowerUp;

	private bool m_includeSpoils;

	private bool m_pickUpIgnoreTeamRestriction;

	private bool m_useHitActorPosForLaserEnd = true;

	private int m_maxPowerupCount;

	protected List<Vector3> m_laserEndPoints;

	public HashSet<PowerUp> m_powerupsHitSoFar;

	public List<bool> m_hitActorInLaser;

	public List<bool> m_hitPowerupInLaser;

	public int m_lastNumAlliesHit;

	public int m_lastNumEnemiesHit;

	public IsAffectingCasterDelegate m_affectCasterDelegate;

	private List<ActorData> m_actorsHitSoFar = new List<ActorData>();

	public CustomDamageOriginDelegate m_customDamageOriginDelegate;

	public LaserLengthDelegate m_delegateLaserLength;

	public LaserWidthDelegate m_delegateLaserWidth;

	private TargeterPart_Laser m_laserPart;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public Dictionary<ActorData, int> m_actorToHitCount = new Dictionary<ActorData, int>();

	public AbilityUtil_Targeter_ThiefFanLaser(Ability ability, float minAngle, float maxAngle, float angleInterpMinDistance, float angleInterpMaxDistance, float rangeInSquares, float widthInSquares, int maxTargets, int count, bool penetrateLos, bool highlightPowerup, bool stopOnPowerUp, bool includeSpoils, bool pickUpIgnoreTeamRestriction, int maxPowerupCount, float interpStep = 0f, float startAngleOffset = 0f)
		: base(ability)
	{
		m_minAngle = Mathf.Max(0f, minAngle);
		m_maxAngle = maxAngle;
		m_interpMinDistanceInSquares = angleInterpMinDistance;
		m_interpMaxDistanceInSquares = angleInterpMaxDistance;
		m_interpStep = interpStep;
		m_startAngleOffset = startAngleOffset;
		m_rangeInSquares = rangeInSquares;
		m_widthInSquares = widthInSquares;
		m_count = count;
		m_maxTargets = maxTargets;
		m_penetrateLos = penetrateLos;
		m_highlightPowerup = highlightPowerup;
		m_stopOnPowerUp = stopOnPowerUp;
		m_includeSpoils = includeSpoils;
		m_pickUpIgnoreTeamRestriction = pickUpIgnoreTeamRestriction;
		m_maxPowerupCount = maxPowerupCount;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_powerupsHitSoFar = new HashSet<PowerUp>();
		m_hitActorInLaser = new List<bool>();
		m_hitPowerupInLaser = new List<bool>();
		m_laserPart = new TargeterPart_Laser(m_widthInSquares, m_rangeInSquares, m_penetrateLos, m_maxTargets);
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		for (int i = 0; i < m_count; i++)
		{
			SquareInsideChecker_Box item = new SquareInsideChecker_Box(m_widthInSquares);
			m_squarePosCheckerList.Add(item);
		}
		while (true)
		{
			return;
		}
	}

	public void SetIncludeTeams(bool includeAllies, bool includeEnemies, bool includeSelf = false)
	{
		m_affectsAllies = includeAllies;
		m_affectsEnemies = includeEnemies;
		m_affectsTargetingActor = includeSelf;
	}

	public void SetFixedAngle(bool changeAngleByCursorDist, float fixedAngle)
	{
		m_changeAngleByCursorDist = changeAngleByCursorDist;
		m_fixedAngleInBetween = fixedAngle;
	}

	public void SetUseHitActorPosForLaserEnd(bool val)
	{
		m_useHitActorPosForLaserEnd = val;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		m_actorToHitCount.Clear();
		m_powerupsHitSoFar.Clear();
		m_hitActorInLaser.Clear();
		m_hitPowerupInLaser.Clear();
		ResetSquareIndicatorIndexToUse();
		m_lastNumAlliesHit = 0;
		m_lastNumEnemiesHit = 0;
		if (m_highlights != null)
		{
			if (m_highlights.Count >= m_count)
			{
				goto IL_00d3;
			}
		}
		m_highlights = new List<GameObject>();
		m_laserEndPoints = new List<Vector3>();
		for (int i = 0; i < m_count; i++)
		{
			m_highlights.Add(m_laserPart.CreateHighlightObject(this));
			m_laserEndPoints.Add(currentTarget.FreePos);
		}
		goto IL_00d3;
		IL_00d3:
		float num = m_fixedAngleInBetween;
		if (m_changeAngleByCursorDist)
		{
			float num2;
			if (m_count > 1)
			{
				num2 = CalculateFanAngleDegrees(currentTarget, targetingActor, m_interpStep);
			}
			else
			{
				num2 = 0f;
			}
			float num3 = num2;
			float num4;
			if (m_count > 1)
			{
				num4 = num3 / (float)(m_count - 1);
			}
			else
			{
				num4 = 0f;
			}
			num = num4;
		}
		float num5 = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection) + m_startAngleOffset;
		float num6 = num5 - 0.5f * (float)(m_count - 1) * num;
		if (m_affectsTargetingActor)
		{
			AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self);
		}
		bool flag = false;
		bool flag2 = false;
		for (int j = 0; j < m_count; j++)
		{
			Vector3 vector = VectorUtils.AngleDegreesToVector(num6 + (float)j * num);
			Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			VectorUtils.LaserCoords coords;
			List<PowerUp> powerupsHit;
			List<ActorData> laserHitActors = GetLaserHitActors(travelBoardSquareWorldPositionForLos, vector, targetingActor, out coords, out powerupsHit);
			flag2 = (laserHitActors.Count > 0);
			flag = (powerupsHit.Count > 0);
			using (List<ActorData>.Enumerator enumerator = laserHitActors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (current.GetTeam() == targetingActor.GetTeam())
					{
						m_lastNumAlliesHit++;
					}
					else
					{
						m_lastNumEnemiesHit++;
					}
					Vector3 vector2 = coords.start;
					if (m_customDamageOriginDelegate != null)
					{
						vector2 = m_customDamageOriginDelegate(current, targetingActor, vector2);
					}
					AddActorInRange(current, vector2, targetingActor, AbilityTooltipSubject.Primary, true);
					ActorHitContext actorHitContext = m_actorContextVars[current];
					actorHitContext.m_hitOrigin = vector2;
					if (m_actorToHitCount.ContainsKey(current))
					{
						m_actorToHitCount[current]++;
					}
					else
					{
						m_actorToHitCount[current] = 1;
					}
				}
			}
			m_hitPowerupInLaser.Add(flag);
			m_hitActorInLaser.Add(flag2);
			m_laserEndPoints[j] = coords.end;
			GameObject highlightObj = m_highlights[j];
			float magnitude = (coords.end - coords.start).magnitude;
			m_laserPart.AdjustHighlight(highlightObj, travelBoardSquareWorldPositionForLos, travelBoardSquareWorldPositionForLos + magnitude * vector);
			UpdateLaserEndPointsForHiddenSquares(coords.start, coords.end, j, targetingActor);
		}
		while (true)
		{
			int hash = ContextKeys.s_HitCount.GetKey();
			int hash2 = ContextKeys.s_InAoe.GetKey();
			using (Dictionary<ActorData, int>.Enumerator enumerator2 = m_actorToHitCount.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<ActorData, int> current2 = enumerator2.Current;
					ActorHitContext actorHitContext2 = m_actorContextVars[current2.Key];
					actorHitContext2.m_contextVars.SetValue(hash, current2.Value);
					actorHitContext2.m_contextVars.SetValue(hash2, 0);
				}
			}
			HandlePowerupHighlight(targetingActor, m_count);
			bool num7;
			if (m_affectCasterDelegate == null)
			{
				num7 = m_affectsTargetingActor;
			}
			else
			{
				num7 = m_affectCasterDelegate(targetingActor, flag2, flag);
			}
			if (num7)
			{
				AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self, true);
			}
			if (ShouldShowHiddenSquareIndicator(targetingActor))
			{
				HandleShowHiddenSquares(targetingActor);
			}
			HideUnusedSquareIndicators();
			return;
		}
	}

	protected virtual void UpdateLaserEndPointsForHiddenSquares(Vector3 startPos, Vector3 endPos, int index, ActorData targetingActor)
	{
		SquareInsideChecker_Box squareInsideChecker_Box = m_squarePosCheckerList[index] as SquareInsideChecker_Box;
		squareInsideChecker_Box.UpdateBoxProperties(startPos, endPos, targetingActor);
	}

	protected virtual void HandleShowHiddenSquares(ActorData targetingActor)
	{
		for (int i = 0; i < m_squarePosCheckerList.Count; i++)
		{
			SquareInsideChecker_Box squareInsideChecker_Box = m_squarePosCheckerList[i] as SquareInsideChecker_Box;
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, squareInsideChecker_Box.GetStartPos(), squareInsideChecker_Box.GetEndPos(), m_widthInSquares, targetingActor, m_penetrateLos, null, m_squarePosCheckerList);
		}
		while (true)
		{
			return;
		}
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> previousTargets)
	{
		ClearActorsInRange();
		m_powerupsHitSoFar.Clear();
		m_hitActorInLaser.Clear();
		m_hitPowerupInLaser.Clear();
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 1)
			{
				goto IL_0076;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(m_laserPart.CreateHighlightObject(this));
		goto IL_0076;
		IL_0076:
		Vector3 vector = currentTarget.AimDirection;
		if (currentTargetIndex > 0)
		{
			if (m_maxAngle > 0f)
			{
				Vector3 aimDirection = previousTargets[currentTargetIndex - 1].AimDirection;
				float num = Vector3.Angle(vector, aimDirection);
				if (num > m_maxAngle)
				{
					vector = Vector3.RotateTowards(vector, aimDirection, (float)Math.PI / 180f * (num - m_maxAngle), 0f);
				}
			}
		}
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		VectorUtils.LaserCoords coords;
		List<PowerUp> powerupsHit;
		List<ActorData> laserHitActors = GetLaserHitActors(travelBoardSquareWorldPositionForLos, vector, targetingActor, out coords, out powerupsHit);
		using (List<ActorData>.Enumerator enumerator = laserHitActors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				AddActorInRange(current, coords.start, targetingActor, AbilityTooltipSubject.Primary, true);
			}
		}
		bool num2;
		if (m_affectCasterDelegate == null)
		{
			num2 = m_affectsTargetingActor;
		}
		else
		{
			num2 = m_affectCasterDelegate(targetingActor, laserHitActors.Count > 0, powerupsHit.Count > 0);
		}
		if (num2)
		{
			AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self, true);
		}
		m_hitActorInLaser.Add(laserHitActors.Count > 0);
		m_hitPowerupInLaser.Add(powerupsHit.Count > 0);
		float num3 = m_rangeInSquares;
		float num4 = m_widthInSquares;
		if (m_delegateLaserLength != null)
		{
			num3 = m_delegateLaserLength(targetingActor, num3);
		}
		if (m_delegateLaserWidth != null)
		{
			num4 = m_delegateLaserWidth(targetingActor, num4);
		}
		m_laserPart.UpdateDimensions(num4, num3);
		GameObject highlightObj = m_highlights[0];
		float magnitude = (coords.end - coords.start).magnitude;
		m_laserPart.AdjustHighlight(highlightObj, travelBoardSquareWorldPositionForLos, travelBoardSquareWorldPositionForLos + magnitude * vector);
		HandlePowerupHighlight(targetingActor, 1);
		if (!ShouldShowHiddenSquareIndicator(targetingActor))
		{
			return;
		}
		while (true)
		{
			ResetSquareIndicatorIndexToUse();
			m_laserPart.ShowHiddenSquares(m_indicatorHandler, coords.start, coords.end, targetingActor, m_penetrateLos);
			HideUnusedSquareIndicators();
			return;
		}
	}

	private float CalculateFanAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor, float interpStep)
	{
		float value = (currentTarget.FreePos - targetingActor.GetTravelBoardSquareWorldPosition()).magnitude / Board.Get().squareSize;
		float num = Mathf.Clamp(value, m_interpMinDistanceInSquares, m_interpMaxDistanceInSquares) - m_interpMinDistanceInSquares;
		if (interpStep > 0f)
		{
			float num2 = num % interpStep;
			num -= num2;
		}
		return Mathf.Max(m_minAngle, m_maxAngle * (1f - num / (m_interpMaxDistanceInSquares - m_interpMinDistanceInSquares)));
	}

	private void HandlePowerupHighlight(ActorData targetingActor, int startingFromIndex)
	{
		int num = startingFromIndex;
		if (!m_highlightPowerup)
		{
			return;
		}
		while (true)
		{
			using (HashSet<PowerUp>.Enumerator enumerator = m_powerupsHitSoFar.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PowerUp current = enumerator.Current;
					if (m_highlights.Count <= num)
					{
						m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
					}
					Vector3 position = current.boardSquare.ToVector3();
					position.y = HighlightUtils.GetHighlightHeight();
					m_highlights[num].transform.position = position;
					m_highlights[num].SetActive(true);
					num++;
				}
			}
			for (int i = num; i < m_highlights.Count; i++)
			{
				m_highlights[i].SetActive(false);
			}
			return;
		}
	}

	private List<ActorData> GetLaserHitActors(Vector3 startPos, Vector3 direction, ActorData targetingActor, out VectorUtils.LaserCoords coords, out List<PowerUp> powerupsHit)
	{
		float num = m_rangeInSquares;
		float num2 = m_widthInSquares;
		if (m_delegateLaserLength != null)
		{
			num = m_delegateLaserLength(targetingActor, num);
		}
		if (m_delegateLaserWidth != null)
		{
			num2 = m_delegateLaserWidth(targetingActor, num2);
		}
		return ThiefBasicAttack.GetHitActorsInDirectionStatic(startPos, direction, targetingActor, num, num2, m_penetrateLos, m_maxTargets, m_affectsAllies, m_affectsEnemies, false, m_maxPowerupCount, m_highlightPowerup, m_stopOnPowerUp, m_includeSpoils, m_pickUpIgnoreTeamRestriction, m_powerupsHitSoFar, out coords, out powerupsHit, null, true, m_useHitActorPosForLaserEnd);
	}

	private bool ShouldShowHiddenSquareIndicator(ActorData targetingActor)
	{
		return targetingActor == GameFlowData.Get().activeOwnedActorData;
	}
}
