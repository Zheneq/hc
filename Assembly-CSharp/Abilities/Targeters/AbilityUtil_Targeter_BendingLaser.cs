using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BendingLaser : AbilityUtil_Targeter
{
	public float m_width = 1f;

	public float m_minDistanceBeforeBend = 5f;

	public float m_maxDistanceBeforeBend = 10f;

	public float m_maxTotalDistance = 15f;

	public float m_maxBendAngle = 45f;

	public bool m_penetrateLoS;

	public int m_maxTargets = -1;

	public bool m_showAngleIndicators = true;

	public bool m_startFadeAtActorRadius;

	private bool m_stoppedShort;

	private List<int> m_highlightsToFade = new List<int>();

	private UIRectangleCursor m_laserStartRect;

	private UIRectangleCursor m_laserEndRect;

	private const int numHighlightObjects = 4;

	private const int laserHighlightStartIndex = 0;

	private const int laserHighlightEndIndex = 1;

	private const int leftSideHighlightIndex = 2;

	private const int rightSideHighlightIndex = 3;

	private const float ghostedHighlightOpacity = 0.06f;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public List<ActorData> m_ordererdHitActors = new List<ActorData>();

	public AbilityUtil_Targeter_BendingLaser(Ability ability, float width, float minDistanceBeforeBend, float maxDistanceBeforeBend, float totalDistance, float maxBendAngle, bool penetrateLoS, int maxTargets = -1, bool affectsAllies = false, bool affectsCaster = false)
		: base(ability)
	{
		m_width = width;
		m_minDistanceBeforeBend = minDistanceBeforeBend;
		m_maxDistanceBeforeBend = maxDistanceBeforeBend;
		m_maxTotalDistance = totalDistance;
		m_maxBendAngle = maxBendAngle;
		m_penetrateLoS = penetrateLoS;
		m_maxTargets = maxTargets;
		m_affectsAllies = affectsAllies;
		SetAffectedGroups(true, m_affectsAllies, affectsCaster);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public bool DidStopShort()
	{
		return m_stoppedShort;
	}

	private float GetClampedRangeInSquares(ActorData targetingActor, AbilityTarget currentTarget)
	{
		Vector3 loSCheckPos = targetingActor.GetLoSCheckPos();
		float magnitude = (currentTarget.FreePos - loSCheckPos).magnitude;
		if (magnitude < m_minDistanceBeforeBend * Board.Get().squareSize)
		{
			return m_minDistanceBeforeBend;
		}
		if (magnitude > m_maxDistanceBeforeBend * Board.Get().squareSize)
		{
			return m_maxDistanceBeforeBend;
		}
		return magnitude / Board.Get().squareSize;
	}

	private float GetDistanceRemaining(ActorData targetingActor, AbilityTarget previousTarget, out Vector3 bendPos)
	{
		Vector3 loSCheckPos = bendPos = targetingActor.GetLoSCheckPos();
		if (m_stoppedShort)
		{
			return 0f;
		}
		float clampedRangeInSquares = GetClampedRangeInSquares(targetingActor, previousTarget);
		bendPos = loSCheckPos + previousTarget.AimDirection * clampedRangeInSquares * Board.Get().squareSize;
		return m_maxTotalDistance - clampedRangeInSquares;
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (m_highlights.Count >= 4)
		{
			GameObject gameObject = m_highlights[2];
			GameObject gameObject2 = m_highlights[3];
			gameObject.SetActive(false);
			gameObject2.SetActive(false);
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, new List<AbilityTarget>
		{
			currentTarget
		});
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		float widthInWorld = m_width * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		ClearActorsInRange();
		m_ordererdHitActors.Clear();
		List<ActorData> list = new List<ActorData>();
		m_stoppedShort = false;
		m_highlightsToFade.Clear();
		bool showAngleIndicators = m_showAngleIndicators && currentTargetIndex == 0;
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		float rangeInSquares;
		Vector3 aimDir;
		float initialOffsetInSquares;
		if (currentTargetIndex == 0)
		{
			laserCoords.start = targetingActor.GetLoSCheckPos();
			rangeInSquares = GetClampedRangeInSquares(targetingActor, currentTarget);
			aimDir = currentTarget.AimDirection;
			initialOffsetInSquares = GameWideData.Get().m_laserInitialOffsetInSquares;
		}
		else
		{
			Vector3 aimDirection = targets[currentTargetIndex - 1].AimDirection;
			rangeInSquares = GetDistanceRemaining(targetingActor, targets[0], out laserCoords.start);
			
			Vector3 freePos = currentTarget.FreePos;
			if ((currentTarget.FreePos - targets[currentTargetIndex - 1].FreePos).magnitude < Mathf.Epsilon)
			{
				freePos += aimDirection * 10f;
			}
			aimDir = freePos - laserCoords.start;
			aimDir.y = 0f;
			aimDir.Normalize();
			
			initialOffsetInSquares = -0.2f;
			if (m_maxBendAngle > 0f && m_maxBendAngle < 360f)
			{
				aimDir = Vector3.RotateTowards(aimDirection, aimDir, (float)Math.PI / 180f * m_maxBendAngle, 0f);
			}
			laserCoords.start = VectorUtils.GetAdjustedStartPosWithOffset(laserCoords.start, laserCoords.start + aimDir, initialOffsetInSquares);
		}
		if (currentTargetIndex > 0)
		{
			Vector3 lineEndPoint = VectorUtils.GetLineEndPoint(laserCoords.start, aimDir, rangeInSquares * Board.SquareSizeStatic);
			rangeInSquares = Mathf.Min(VectorUtils.HorizontalPlaneDistInSquares(lineEndPoint, laserCoords.start), rangeInSquares);
		}
		List<ActorData> actors = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			aimDir,
			rangeInSquares,
			m_width,
			targetingActor,
			GetAffectedTeams(),
			m_penetrateLoS,
			0,
			false,
			false,
			out laserCoords.end,
			null,
			null,
			currentTargetIndex > 0);
		TargeterUtils.SortActorsByDistanceToPos(ref actors, laserCoords.start);
		bool isMultiHit = false;
		int numActorsInRange = m_maxTargets;
		if (currentTargetIndex > 0
		    && m_ability != null
		    && currentTargetIndex < m_ability.Targeters.Count)
		{
			AbilityUtil_Targeter abilityUtil_Targeter = m_ability.Targeters[currentTargetIndex - 1];
			for (int i = actors.Count - 1; i >= 0; i--)
			{
				if (abilityUtil_Targeter.IsActorInTargetRange(actors[i]))
				{
					actors.RemoveAt(i);
				}
			}
			isMultiHit = abilityUtil_Targeter.GetNumActorsInRange() > 0;
			numActorsInRange -= abilityUtil_Targeter.GetNumActorsInRange();
		}
		if (actors.Contains(targetingActor))
		{
			actors.Remove(targetingActor);
		}
		if (actors.Count > numActorsInRange)
		{
			actors.RemoveRange(numActorsInRange, actors.Count - numActorsInRange);
		}
		
		float magnitude = (laserCoords.end - laserCoords.start).magnitude;
		if (currentTargetIndex == 0 && magnitude < rangeInSquares * Board.Get().squareSize - 0.1f)
		{
			m_stoppedShort = true;
		}
		magnitude -= initialOffsetInSquares;
		
		if (currentTargetIndex == 0)
		{
			laserCoords.start = VectorUtils.GetAdjustedStartPosWithOffset(laserCoords.start, laserCoords.end, initialOffsetInSquares);
		}
		float lengthInWorld1 = magnitude;
		float lengthInWorld2 = 0f;
		float lengthInSquares = m_maxTotalDistance - rangeInSquares + 0.5f * widthInWorld;
		if (m_highlights.IsNullOrEmpty())
		{
			m_highlights = new List<GameObject>(4)
			{
				HighlightUtils.Get().CreateRectangularCursor(widthInWorld, lengthInWorld1),
				HighlightUtils.Get().CreateRectangularCursor(widthInWorld, lengthInWorld2),
				HighlightUtils.Get().CreateDynamicLineSegmentMesh(lengthInSquares, 0.2f, true, Color.cyan),
				HighlightUtils.Get().CreateDynamicLineSegmentMesh(lengthInSquares, 0.2f, true, Color.cyan)
			};
			m_laserStartRect = m_highlights[0].GetComponent<UIRectangleCursor>();
			m_laserEndRect = m_highlights[1].GetComponent<UIRectangleCursor>();
			m_highlights[2].SetActive(showAngleIndicators);
			m_highlights[3].SetActive(showAngleIndicators);
		}

		bool hitMaxTargets = numActorsInRange > 0 && actors.Count == numActorsInRange;
		if (hitMaxTargets)
		{
			Vector3 lastTargetPos = actors[actors.Count - 1].GetFreePos();
			lastTargetPos.y = laserCoords.start.y;
			lengthInWorld1 = (lastTargetPos - laserCoords.start).magnitude;
			if (m_startFadeAtActorRadius)
			{
				lengthInWorld1 -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.SquareSizeStatic;
			}
			lengthInWorld2 = magnitude - lengthInWorld1;
			if (m_laserStartRect != null)
			{
				if (lengthInWorld2 < m_laserStartRect.m_lengthPerCorner)
				{
					hitMaxTargets = false;
					lengthInWorld1 = magnitude;
				}
				else
				{
					lengthInWorld2 += m_laserStartRect.m_distCasterToInterior;
					if (currentTargetIndex != 0)
					{
						lengthInWorld1 += initialOffsetInSquares + m_laserStartRect.m_distCasterToStart;
					}
					lengthInWorld1 += m_laserStartRect.m_lengthPerCorner;
				}
			}
		}
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, lengthInWorld1, m_highlights[0]);
		m_laserStartRect.SetRectangleEndVisible(!hitMaxTargets);
		m_laserEndRect.SetRectangleStartVisible(!hitMaxTargets);
		m_highlights[1].SetActive(hitMaxTargets);
		if (hitMaxTargets)
		{
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, lengthInWorld2, m_highlights[1]);
			m_highlightsToFade.Add(1);
		}
		Vector3 normalized = (laserCoords.end - laserCoords.start).normalized;
		m_highlights[0].transform.position = laserCoords.start + new Vector3(0f, y, 0f);
		m_highlights[0].transform.rotation = Quaternion.LookRotation(normalized);
		if (hitMaxTargets)
		{
			m_highlights[1].transform.position = laserCoords.start + normalized * (lengthInWorld1 - m_laserEndRect.m_lengthPerCorner - m_laserEndRect.m_distCasterToInterior) + new Vector3(0f, y, 0f);
			m_highlights[1].transform.rotation = Quaternion.LookRotation(normalized);
		}
		if (numActorsInRange > 0)
		{
			int actorsHit = 0;
			for (int i = 0; i < actors.Count; i++)
			{
				ActorData actorData = actors[i];
				Vector3 start = laserCoords.start;
				if (currentTargetIndex > 0 && Board.Get().GetSquareFromVec3(start) == actorData.GetCurrentBoardSquare())
				{
					start = targetingActor.GetLoSCheckPos();
				}
				AddActorInRange(actorData, start, targetingActor);
				if (!isMultiHit && i == 0)
				{
					AddActorInRange(actorData, start, targetingActor, AbilityTooltipSubject.Near, true);
				}
				if (currentTargetIndex > 0)
				{
					SetIgnoreCoverMinDist(actorData, true);
				}
				list.Add(actorData);
				m_ordererdHitActors.Add(actorData);
				actorsHit++;
			}
			if (m_affectsTargetingActor)
			{
				AddActorInRange(targetingActor, laserCoords.start, targetingActor, AbilityTooltipSubject.Secondary);
			}
		}
		else
		{
			m_highlightsToFade.Add(0);
		}
		if (showAngleIndicators)
		{
			GameObject highlight2 = m_highlights[2];
			GameObject highlight3 = m_highlights[3];
			Vector3 aimDirection2 = currentTarget.AimDirection;
			aimDirection2.y = 0f;
			if (aimDirection2.magnitude > 0f && !m_stoppedShort)
			{
				highlight2.SetActive(true);
				highlight3.SetActive(true);
				float num11 = VectorUtils.HorizontalAngle_Deg(aimDirection2);
				float angle = num11 + m_maxBendAngle;
				float angle2 = num11 - m_maxBendAngle;
				Vector3 end = laserCoords.end;
				end -= aimDirection2 * ((widthInWorld + 0.2f) * 0.5f);
				end.y = HighlightUtils.GetHighlightHeight();
				highlight2.transform.position = end;
				highlight3.transform.position = end;
				highlight2.transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(angle));
				highlight3.transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(angle2));
				HighlightUtils.Get().AdjustDynamicLineSegmentLength(highlight2, lengthInSquares);
				HighlightUtils.Get().AdjustDynamicLineSegmentLength(highlight3, lengthInSquares);
			}
			else
			{
				highlight2.SetActive(false);
				highlight3.SetActive(false);
			}
		}
		DrawInvalidSquareIndicators(currentTarget, targetingActor, laserCoords.start, laserCoords.end);
	}

	public override void AdjustOpacityWhileTargeting()
	{
		base.AdjustOpacityWhileTargeting();
		if (!m_highlights.IsNullOrEmpty())
		{
			using (List<int>.Enumerator enumerator = m_highlightsToFade.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					List<GameObject> list = new List<GameObject>();
					list.Add(m_highlights[current]);
					AbilityUtil_Targeter.SetTargeterHighlightOpacity(list, 0.06f);
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
		}
	}

	public override void UpdateConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateConfirmedTargeting(currentTarget, targetingActor);
		if (m_highlights.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			using (List<int>.Enumerator enumerator = m_highlightsToFade.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					List<GameObject> list = new List<GameObject>();
					list.Add(m_highlights[current]);
					AbilityUtil_Targeter.SetTargeterHighlightOpacity(list, 0.06f);
				}
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, startPos, endPos, m_width, targetingActor, m_penetrateLoS);
			HideUnusedSquareIndicators();
		}
	}
}
