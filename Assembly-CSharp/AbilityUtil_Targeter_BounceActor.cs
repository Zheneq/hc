using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BounceActor : AbilityUtil_Targeter
{
	public struct HitActorContext
	{
		public ActorData actor;

		public int segmentIndex;
	}

	public float m_width = 1f;

	public float m_maxDistancePerBounce = 15f;

	public float m_maxTotalDistance = 50f;

	public int m_maxBounces = 5;

	public int m_maxTargetsHit = 1;

	public bool m_bounceOnEnemyActor;

	public bool m_includeAlliesInBetween;

	private List<HitActorContext> m_hitActorContext = new List<HitActorContext>();

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_BounceActor(Ability ability, float width, float distancePerBounce, float totalDistance, int maxBounces, int maxTargetsHit, bool bounceOnEnemyActor, bool includeAlliesInBetween = false, bool includeSelf = false)
		: base(ability)
	{
		m_width = width;
		m_maxDistancePerBounce = distancePerBounce;
		m_maxTotalDistance = totalDistance;
		m_maxBounces = maxBounces;
		m_maxTargetsHit = maxTargetsHit;
		m_bounceOnEnemyActor = bounceOnEnemyActor;
		m_includeAlliesInBetween = includeAlliesInBetween;
		m_affectsTargetingActor = includeSelf;
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public List<HitActorContext> GetHitActorContext()
	{
		return m_hitActorContext;
	}

	public void SetMaxBounces(int maxBounces)
	{
		m_maxBounces = maxBounces;
	}

	public void SetMaxTargets(int maxTargets)
	{
		m_maxTargetsHit = maxTargets;
	}

	public void CreateLaserHighlights(Vector3 originalStart, List<Vector3> laserAnglePoints, bool showDestinationHighlight)
	{
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 vector = originalStart + new Vector3(0f, y, 0f);
		Vector3 originalStart2 = vector;
		float num = m_width * Board.Get().squareSize;
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 2)
			{
				goto IL_00ae;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(HighlightUtils.Get().CreateBouncingLaserCursor(originalStart2, laserAnglePoints, num));
		m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, true));
		goto IL_00ae;
		IL_00ae:
		GameObject gameObject = m_highlights[0];
		UIBouncingLaserCursor component = gameObject.GetComponent<UIBouncingLaserCursor>();
		component.OnUpdated(originalStart2, laserAnglePoints, num);
		GameObject gameObject2 = m_highlights[1];
		bool active = false;
		if (showDestinationHighlight)
		{
			Vector3 a = laserAnglePoints[laserAnglePoints.Count - 1];
			Vector3 vector2;
			if (laserAnglePoints.Count >= 2)
			{
				vector2 = laserAnglePoints[laserAnglePoints.Count - 2];
			}
			else
			{
				vector2 = originalStart;
			}
			Vector3 vector3 = vector2;
			Vector3 a2 = a - vector3;
			float magnitude = a2.magnitude;
			a2.Normalize();
			Vector3 end = a - Mathf.Min(0.5f, magnitude / 2f) * a2;
			BoardSquare lastValidBoardSquareInLine = KnockbackUtils.GetLastValidBoardSquareInLine(vector3, end, true);
			if (lastValidBoardSquareInLine != null)
			{
				active = true;
				Vector3 position = lastValidBoardSquareInLine.ToVector3();
				position.y -= 0.1f;
				gameObject2.transform.position = position;
			}
		}
		gameObject2.SetActive(active);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetLoSCheckPos();
		Vector3 vector;
		if (currentTarget == null)
		{
			vector = targetingActor.transform.forward;
		}
		else
		{
			vector = currentTarget.AimDirection;
		}
		Vector3 forwardDirection = vector;
		int num;
		if (m_bounceOnEnemyActor)
		{
			num = ((m_maxTargetsHit != 1) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool bounceOnActors = (byte)num != 0;
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors;
		List<ActorData> orderedHitActors;
		List<Vector3> laserAnglePoints = VectorUtils.CalculateBouncingActorEndpoints(travelBoardSquareWorldPositionForLos, forwardDirection, m_maxDistancePerBounce, m_maxTotalDistance, m_maxBounces, targetingActor, bounceOnActors, m_width, targetingActor.GetEnemyTeamAsList(), m_maxTargetsHit, out bounceHitActors, out orderedHitActors, false, null);
		ClearActorsInRange();
		m_hitActorContext.Clear();
		using (Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>.Enumerator enumerator = bounceHitActors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> current = enumerator.Current;
				ActorData key = current.Key;
				AreaEffectUtils.BouncingLaserInfo value = current.Value;
				AddActorInRange(key, value.m_segmentOrigin, targetingActor);
			}
		}
		HitActorContext item = default(HitActorContext);
		for (int i = 0; i < orderedHitActors.Count; i++)
		{
			item.actor = orderedHitActors[i];
			AreaEffectUtils.BouncingLaserInfo bouncingLaserInfo = bounceHitActors[orderedHitActors[i]];
			item.segmentIndex = bouncingLaserInfo.m_endpointIndex;
			m_hitActorContext.Add(item);
		}
		List<BoardSquare> chargePathSquares = GetChargePathSquares(targetingActor, laserAnglePoints, bounceHitActors, orderedHitActors);
		BoardSquarePathInfo chargePathFromSquareList = GetChargePathFromSquareList(targetingActor, chargePathSquares);
		int fromIndex = 0;
		EnableAllMovementArrows();
		if (chargePathFromSquareList != null)
		{
			fromIndex = AddMovementArrowWithPrevious(targetingActor, chargePathFromSquareList, TargeterMovementType.Movement, 0);
		}
		SetMovementArrowEnabledFromIndex(fromIndex, false);
		int num2;
		if (m_maxTargetsHit > 0)
		{
			num2 = ((orderedHitActors.Count >= m_maxTargetsHit) ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		if (num2 != 0)
		{
			float num3 = Board.Get().squareSize * AreaEffectUtils.GetActorTargetingRadius();
			Vector3 a = laserAnglePoints[laserAnglePoints.Count - 1];
			Vector3 vector2;
			if (laserAnglePoints.Count > 1)
			{
				vector2 = laserAnglePoints[laserAnglePoints.Count - 2];
			}
			else
			{
				vector2 = travelBoardSquareWorldPositionForLos;
			}
			Vector3 vector3 = vector2;
			Vector3 normalized = (a - vector3).normalized;
			Vector3 lhs = orderedHitActors[orderedHitActors.Count - 1].GetFreePos() - vector3;
			laserAnglePoints[laserAnglePoints.Count - 1] = vector3 + (Vector3.Dot(lhs, normalized) + num3) * normalized;
		}
		if (m_includeAlliesInBetween)
		{
			List<ActorData> orderedHitActors2 = new List<ActorData>();
			Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary = AreaEffectUtils.FindBouncingLaserTargets(travelBoardSquareWorldPositionForLos, ref laserAnglePoints, m_width, targetingActor.GetTeamAsList(), -1, false, targetingActor, orderedHitActors2);
			foreach (KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> item2 in dictionary)
			{
				ActorData key2 = item2.Key;
				AreaEffectUtils.BouncingLaserInfo value2 = item2.Value;
				AddActorInRange(key2, value2.m_segmentOrigin, targetingActor, AbilityTooltipSubject.Secondary);
			}
		}
		if (m_affectsTargetingActor)
		{
			AddActorInRange(targetingActor, targetingActor.GetFreePos(), targetingActor, AbilityTooltipSubject.Self);
		}
		CreateLaserHighlights(travelBoardSquareWorldPositionForLos, laserAnglePoints, false);
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBounceLaser(m_indicatorHandler, travelBoardSquareWorldPositionForLos, laserAnglePoints, m_width, targetingActor, false);
			HideUnusedSquareIndicators();
			return;
		}
	}

	public List<BoardSquare> GetChargePathSquares(ActorData caster, List<Vector3> endPoints, Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> laserTargets, List<ActorData> orderedHitActors)
	{
		int num = endPoints.Count - 1;
		while (true)
		{
			if (num > 0)
			{
				Vector3 start = endPoints[num - 1];
				Vector3 end = endPoints[num];
				BoardSquare lastValidBoardSquareInLine = KnockbackUtils.GetLastValidBoardSquareInLine(start, end, true);
				if (!(lastValidBoardSquareInLine == null))
				{
					if (lastValidBoardSquareInLine.IsValidForGameplay())
					{
						break;
					}
				}
				endPoints.RemoveAt(num);
				num--;
				continue;
			}
			break;
		}
		Vector3 vector = endPoints[endPoints.Count - 1];
		Vector3 vector2;
		if (endPoints.Count >= 2)
		{
			vector2 = endPoints[endPoints.Count - 2];
		}
		else
		{
			vector2 = caster.GetLoSCheckPos();
		}
		Vector3 vector3 = vector2;
		Vector3 a = vector - vector3;
		float magnitude = a.magnitude;
		a.Normalize();
		Vector3 end2 = vector - Mathf.Min(0.5f, magnitude / 2f) * a;
		BoardSquare boardSquare = null;
		float num2 = 0f;
		if (m_maxTargetsHit > 0 && laserTargets.Count >= m_maxTargetsHit)
		{
			boardSquare = orderedHitActors[orderedHitActors.Count - 1].GetCurrentBoardSquare();
			num2 = -0.5f;
		}
		else
		{
			boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(vector3, end2, true);
		}
		if (boardSquare != null)
		{
			if (boardSquare != caster.GetCurrentBoardSquare())
			{
				float num3 = VectorUtils.HorizontalPlaneDistInWorld(vector3, boardSquare.ToVector3());
				float maxDistance = Mathf.Max(0f, num3 + num2);
				boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(vector3, vector, true, false, maxDistance);
			}
		}
		List<BoardSquare> list = new List<BoardSquare>();
		if (boardSquare != null)
		{
			list.Add(caster.GetCurrentBoardSquare());
			for (int i = 0; i < endPoints.Count; i++)
			{
				Vector3 vector4;
				if (i == 0)
				{
					vector4 = list[i].ToVector3();
				}
				else
				{
					vector4 = endPoints[i - 1];
				}
				BoardSquare y = list[list.Count - 1];
				Vector3 a2 = endPoints[i] - vector4;
				a2.y = 0f;
				a2.Normalize();
				Vector3 b = a2 / 2f;
				if (i > 0)
				{
					BoardSquare boardSquare2 = Board.Get().GetSquareFromVec3(vector4 + b);
					if (boardSquare2 != null)
					{
						if (boardSquare2 != y && boardSquare2.IsValidForGameplay())
						{
							list.Add(boardSquare2);
							y = boardSquare2;
						}
					}
				}
				if (i == endPoints.Count - 1)
				{
					if (boardSquare != y)
					{
						list.Add(boardSquare);
					}
					continue;
				}
				BoardSquare boardSquare3 = Board.Get().GetSquareFromVec3(endPoints[i] - b);
				if (boardSquare3 != null)
				{
					if (!boardSquare3.IsValidForGameplay())
					{
						BoardSquare lastValidBoardSquareInLine2 = KnockbackUtils.GetLastValidBoardSquareInLine(vector4, endPoints[i], true);
						if (lastValidBoardSquareInLine2 != null)
						{
							if (lastValidBoardSquareInLine2.IsValidForGameplay())
							{
								boardSquare3 = lastValidBoardSquareInLine2;
							}
						}
					}
				}
				if (!(boardSquare3 != null))
				{
					continue;
				}
				if (boardSquare3 != y)
				{
					list.Add(boardSquare3);
				}
			}
			ActorData occupantActor = boardSquare.OccupantActor;
			if (occupantActor != null && occupantActor != caster)
			{
				if (occupantActor.IsActorVisibleToClient())
				{
					Vector3 testDir = vector3 - boardSquare.ToVector3();
					testDir.y = 0f;
					testDir.Normalize();
					BoardSquare secondToLastInOrigPath = null;
					if (list.Count > 1)
					{
						secondToLastInOrigPath = list[list.Count - 2];
					}
					BoardSquare endSquareForOccupant = GetEndSquareForOccupant(boardSquare, testDir, caster, secondToLastInOrigPath);
					list.Add(endSquareForOccupant);
				}
			}
		}
		else
		{
			list.Add(caster.GetCurrentBoardSquare());
		}
		if (list.Count == 1)
		{
			list.Add(caster.GetCurrentBoardSquare());
		}
		return list;
	}

	private BoardSquare GetEndSquareForOccupant(BoardSquare lastSquare, Vector3 testDir, ActorData caster, BoardSquare secondToLastInOrigPath)
	{
		BoardSquare boardSquare = null;
		float num = -1f;
		for (int i = 0; i < 3; i++)
		{
			if (boardSquare == null)
			{
				List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(lastSquare, i, true);
				for (int j = 0; j < squaresInBorderLayer.Count; j++)
				{
					BoardSquare boardSquare2 = squaresInBorderLayer[j];
					if (!boardSquare2.IsValidForGameplay())
					{
						continue;
					}
					if (!(boardSquare2.OccupantActor == null))
					{
						if (!(boardSquare2.OccupantActor == caster))
						{
							if (boardSquare2.OccupantActor.IsActorVisibleToClient())
							{
								continue;
							}
						}
					}

					int foo;
					if (!KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquare2, lastSquare, false, out foo))
					{
						continue;
					}
					Vector3 rhs = boardSquare2.ToVector3() - lastSquare.ToVector3();
					rhs.y = 0f;
					rhs.Normalize();
					float num2 = Vector3.Dot(testDir, rhs);
					if (secondToLastInOrigPath != null)
					{
						if (secondToLastInOrigPath == boardSquare2)
						{
							num2 += 0.5f;
						}
					}
					if (lastSquare.GetLOS(boardSquare2.x, boardSquare2.y))
					{
						num2 -= 2f;
					}
					if (!(boardSquare == null))
					{
						if (!(num2 > num))
						{
							continue;
						}
					}
					boardSquare = boardSquare2;
					num = num2;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						goto end_IL_0194;
					}
					continue;
					end_IL_0194:
					break;
				}
				continue;
			}
			break;
		}
		if (boardSquare == null)
		{
			boardSquare = lastSquare;
		}
		return boardSquare;
	}

	private BoardSquarePathInfo GetChargePathFromSquareList(ActorData charger, List<BoardSquare> squaresInPath)
	{
		BoardSquarePathInfo boardSquarePathInfo = null;
		int num = 1;
		while (num < squaresInPath.Count)
		{
			BoardSquare destination = squaresInPath[num];
			BoardSquare startSquare = squaresInPath[num - 1];
			BoardSquarePathInfo boardSquarePathInfo2 = null;
			boardSquarePathInfo2 = KnockbackUtils.BuildStraightLineChargePath(charger, destination, startSquare, true);
			for (BoardSquarePathInfo boardSquarePathInfo3 = boardSquarePathInfo2; boardSquarePathInfo3 != null; boardSquarePathInfo3 = boardSquarePathInfo3.next)
			{
			}
			while (true)
			{
				if (boardSquarePathInfo != null)
				{
					BoardSquarePathInfo pathEndpoint = boardSquarePathInfo.GetPathEndpoint();
					if (boardSquarePathInfo2 != null)
					{
						if (boardSquarePathInfo2.next != null)
						{
							if (pathEndpoint.square == boardSquarePathInfo2.square)
							{
								pathEndpoint.m_unskippable = true;
								pathEndpoint.next = boardSquarePathInfo2.next;
								boardSquarePathInfo2.next.prev = pathEndpoint;
							}
						}
					}
				}
				else
				{
					boardSquarePathInfo = boardSquarePathInfo2;
				}
				num++;
				goto IL_00dc;
			}
			IL_00dc:;
		}
		while (true)
		{
			return boardSquarePathInfo;
		}
	}
}
