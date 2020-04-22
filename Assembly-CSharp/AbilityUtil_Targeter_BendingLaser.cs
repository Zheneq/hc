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
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		float magnitude = (currentTarget.FreePos - travelBoardSquareWorldPositionForLos).magnitude;
		if (magnitude < m_minDistanceBeforeBend * Board.Get().squareSize)
		{
			return m_minDistanceBeforeBend;
		}
		if (magnitude > m_maxDistanceBeforeBend * Board.Get().squareSize)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_maxDistanceBeforeBend;
				}
			}
		}
		return magnitude / Board.Get().squareSize;
	}

	private float GetDistanceRemaining(ActorData targetingActor, AbilityTarget previousTarget, out Vector3 bendPos)
	{
		Vector3 a = bendPos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		if (m_stoppedShort)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return 0f;
				}
			}
		}
		float clampedRangeInSquares = GetClampedRangeInSquares(targetingActor, previousTarget);
		bendPos = a + previousTarget.AimDirection * clampedRangeInSquares * Board.Get().squareSize;
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
		float num = m_width * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		ClearActorsInRange();
		m_ordererdHitActors.Clear();
		List<ActorData> list = new List<ActorData>();
		m_stoppedShort = false;
		m_highlightsToFade.Clear();
		int num2;
		if (m_showAngleIndicators)
		{
			num2 = ((currentTargetIndex == 0) ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool flag = (byte)num2 != 0;
		VectorUtils.LaserCoords laserCoords = default(VectorUtils.LaserCoords);
		float num3;
		Vector3 vector;
		float num4;
		if (currentTargetIndex == 0)
		{
			laserCoords.start = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			num3 = GetClampedRangeInSquares(targetingActor, currentTarget);
			vector = currentTarget.AimDirection;
			num4 = GameWideData.Get().m_laserInitialOffsetInSquares;
		}
		else
		{
			Vector3 aimDirection = targets[currentTargetIndex - 1].AimDirection;
			num3 = GetDistanceRemaining(targetingActor, targets[0], out laserCoords.start);
			Vector3 freePos = currentTarget.FreePos;
			if ((currentTarget.FreePos - targets[currentTargetIndex - 1].FreePos).magnitude < Mathf.Epsilon)
			{
				freePos += aimDirection * 10f;
			}
			vector = freePos - laserCoords.start;
			vector.y = 0f;
			vector.Normalize();
			num4 = -0.2f;
			if (m_maxBendAngle > 0f)
			{
				if (m_maxBendAngle < 360f)
				{
					vector = Vector3.RotateTowards(aimDirection, vector, (float)Math.PI / 180f * m_maxBendAngle, 0f);
				}
			}
			laserCoords.start = VectorUtils.GetAdjustedStartPosWithOffset(laserCoords.start, laserCoords.start + vector, num4);
		}
		if (currentTargetIndex > 0)
		{
			Vector3 lineEndPoint = VectorUtils.GetLineEndPoint(laserCoords.start, vector, num3 * Board.SquareSizeStatic);
			num3 = Mathf.Min(VectorUtils.HorizontalPlaneDistInSquares(lineEndPoint, laserCoords.start), num3);
		}
		List<ActorData> actors = AreaEffectUtils.GetActorsInLaser(laserCoords.start, vector, num3, m_width, targetingActor, GetAffectedTeams(), m_penetrateLoS, 0, false, false, out laserCoords.end, null, null, currentTargetIndex > 0);
		TargeterUtils.SortActorsByDistanceToPos(ref actors, laserCoords.start);
		bool flag2 = false;
		int num5 = m_maxTargets;
		if (currentTargetIndex > 0 && m_ability != null)
		{
			if (currentTargetIndex < m_ability.Targeters.Count)
			{
				AbilityUtil_Targeter abilityUtil_Targeter = m_ability.Targeters[currentTargetIndex - 1];
				for (int num6 = actors.Count - 1; num6 >= 0; num6--)
				{
					ActorData actor = actors[num6];
					if (abilityUtil_Targeter.IsActorInTargetRange(actor, out bool _))
					{
						actors.RemoveAt(num6);
					}
				}
				flag2 = (abilityUtil_Targeter.GetNumActorsInRange() > 0);
				num5 -= abilityUtil_Targeter.GetNumActorsInRange();
			}
		}
		if (actors.Contains(targetingActor))
		{
			actors.Remove(targetingActor);
		}
		if (actors.Count > num5)
		{
			actors.RemoveRange(num5, actors.Count - num5);
		}
		float magnitude = (laserCoords.end - laserCoords.start).magnitude;
		if (currentTargetIndex == 0)
		{
			if (magnitude < num3 * Board.Get().squareSize - 0.1f)
			{
				m_stoppedShort = true;
			}
		}
		magnitude -= num4;
		if (currentTargetIndex == 0)
		{
			laserCoords.start = VectorUtils.GetAdjustedStartPosWithOffset(laserCoords.start, laserCoords.end, num4);
		}
		float num7 = magnitude;
		float num8 = 0f;
		float lengthInSquares = m_maxTotalDistance - num3 + 0.5f * num;
		if (m_highlights.IsNullOrEmpty())
		{
			m_highlights = new List<GameObject>(4);
			m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(num, num7));
			m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(num, num8));
			m_laserStartRect = m_highlights[0].GetComponent<UIRectangleCursor>();
			m_laserEndRect = m_highlights[1].GetComponent<UIRectangleCursor>();
			m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(lengthInSquares, 0.2f, true, Color.cyan));
			m_highlights.Add(HighlightUtils.Get().CreateDynamicLineSegmentMesh(lengthInSquares, 0.2f, true, Color.cyan));
			m_highlights[2].SetActive(flag);
			m_highlights[3].SetActive(flag);
		}
		int num9;
		if (num5 > 0)
		{
			num9 = ((actors.Count == num5) ? 1 : 0);
		}
		else
		{
			num9 = 0;
		}
		bool flag3 = (byte)num9 != 0;
		if (flag3)
		{
			Vector3 travelBoardSquareWorldPosition = actors[actors.Count - 1].GetTravelBoardSquareWorldPosition();
			travelBoardSquareWorldPosition.y = laserCoords.start.y;
			num7 = (travelBoardSquareWorldPosition - laserCoords.start).magnitude;
			if (m_startFadeAtActorRadius)
			{
				num7 -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.SquareSizeStatic;
			}
			num8 = magnitude - num7;
			if (m_laserStartRect != null)
			{
				if (num8 < m_laserStartRect.m_lengthPerCorner)
				{
					flag3 = false;
					num7 = magnitude;
				}
				else
				{
					num8 += m_laserStartRect.m_distCasterToInterior;
					if (currentTargetIndex != 0)
					{
						num7 += num4 + m_laserStartRect.m_distCasterToStart;
					}
					num7 += m_laserStartRect.m_lengthPerCorner;
				}
			}
		}
		HighlightUtils.Get().ResizeRectangularCursor(num, num7, m_highlights[0]);
		m_laserStartRect.SetRectangleEndVisible(!flag3);
		m_laserEndRect.SetRectangleStartVisible(!flag3);
		m_highlights[1].SetActive(flag3);
		if (flag3)
		{
			HighlightUtils.Get().ResizeRectangularCursor(num, num8, m_highlights[1]);
			m_highlightsToFade.Add(1);
		}
		Vector3 normalized = (laserCoords.end - laserCoords.start).normalized;
		m_highlights[0].transform.position = laserCoords.start + new Vector3(0f, y, 0f);
		m_highlights[0].transform.rotation = Quaternion.LookRotation(normalized);
		if (flag3)
		{
			m_highlights[1].transform.position = laserCoords.start + normalized * (num7 - m_laserEndRect.m_lengthPerCorner - m_laserEndRect.m_distCasterToInterior) + new Vector3(0f, y, 0f);
			m_highlights[1].transform.rotation = Quaternion.LookRotation(normalized);
		}
		if (num5 > 0)
		{
			int num10 = 0;
			for (int i = 0; i < actors.Count; i++)
			{
				ActorData actorData = actors[i];
				Vector3 vector2 = laserCoords.start;
				if (currentTargetIndex > 0)
				{
					if (Board.Get().GetBoardSquare(vector2) == actorData.GetCurrentBoardSquare())
					{
						vector2 = targetingActor.GetTravelBoardSquareWorldPositionForLos();
					}
				}
				AddActorInRange(actorData, vector2, targetingActor);
				if (!flag2 && i == 0)
				{
					AddActorInRange(actorData, vector2, targetingActor, AbilityTooltipSubject.Near, true);
				}
				if (currentTargetIndex > 0)
				{
					SetIgnoreCoverMinDist(actorData, true);
				}
				list.Add(actorData);
				m_ordererdHitActors.Add(actorData);
				num10++;
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
		if (flag)
		{
			GameObject gameObject = m_highlights[2];
			GameObject gameObject2 = m_highlights[3];
			Vector3 aimDirection2 = currentTarget.AimDirection;
			aimDirection2.y = 0f;
			if (aimDirection2.magnitude > 0f)
			{
				if (!m_stoppedShort)
				{
					gameObject.SetActive(true);
					gameObject2.SetActive(true);
					float num11 = VectorUtils.HorizontalAngle_Deg(aimDirection2);
					float angle = num11 + m_maxBendAngle;
					float angle2 = num11 - m_maxBendAngle;
					Vector3 end = laserCoords.end;
					end -= aimDirection2 * ((num + 0.2f) * 0.5f);
					end.y = HighlightUtils.GetHighlightHeight();
					gameObject.transform.position = end;
					gameObject2.transform.position = end;
					gameObject.transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(angle));
					gameObject2.transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(angle2));
					HighlightUtils.Get().AdjustDynamicLineSegmentLength(gameObject, lengthInSquares);
					HighlightUtils.Get().AdjustDynamicLineSegmentLength(gameObject2, lengthInSquares);
					goto IL_0a33;
				}
			}
			gameObject.SetActive(false);
			gameObject2.SetActive(false);
		}
		goto IL_0a33;
		IL_0a33:
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
