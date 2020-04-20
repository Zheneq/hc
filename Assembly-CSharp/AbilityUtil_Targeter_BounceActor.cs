using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BounceActor : AbilityUtil_Targeter
{
	public float m_width = 1f;

	public float m_maxDistancePerBounce = 15f;

	public float m_maxTotalDistance = 50f;

	public int m_maxBounces = 5;

	public int m_maxTargetsHit = 1;

	public bool m_bounceOnEnemyActor;

	public bool m_includeAlliesInBetween;

	private List<AbilityUtil_Targeter_BounceActor.HitActorContext> m_hitActorContext = new List<AbilityUtil_Targeter_BounceActor.HitActorContext>();

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_BounceActor(Ability ability, float width, float distancePerBounce, float totalDistance, int maxBounces, int maxTargetsHit, bool bounceOnEnemyActor, bool includeAlliesInBetween = false, bool includeSelf = false) : base(ability)
	{
		this.m_width = width;
		this.m_maxDistancePerBounce = distancePerBounce;
		this.m_maxTotalDistance = totalDistance;
		this.m_maxBounces = maxBounces;
		this.m_maxTargetsHit = maxTargetsHit;
		this.m_bounceOnEnemyActor = bounceOnEnemyActor;
		this.m_includeAlliesInBetween = includeAlliesInBetween;
		this.m_affectsTargetingActor = includeSelf;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public List<AbilityUtil_Targeter_BounceActor.HitActorContext> GetHitActorContext()
	{
		return this.m_hitActorContext;
	}

	public void SetMaxBounces(int maxBounces)
	{
		this.m_maxBounces = maxBounces;
	}

	public void SetMaxTargets(int maxTargets)
	{
		this.m_maxTargetsHit = maxTargets;
	}

	public void CreateLaserHighlights(Vector3 originalStart, List<Vector3> laserAnglePoints, bool showDestinationHighlight)
	{
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 vector = originalStart + new Vector3(0f, y, 0f);
		Vector3 originalStart2 = vector;
		float num = this.m_width * Board.Get().squareSize;
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 2)
			{
				goto IL_AE;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateBouncingLaserCursor(originalStart2, laserAnglePoints, num));
		this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, true));
		IL_AE:
		GameObject gameObject = this.m_highlights[0];
		UIBouncingLaserCursor component = gameObject.GetComponent<UIBouncingLaserCursor>();
		component.OnUpdated(originalStart2, laserAnglePoints, num);
		GameObject gameObject2 = this.m_highlights[1];
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
			BoardSquare lastValidBoardSquareInLine = KnockbackUtils.GetLastValidBoardSquareInLine(vector3, end, true, false, float.MaxValue);
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
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
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
		bool flag;
		if (this.m_bounceOnEnemyActor)
		{
			flag = (this.m_maxTargetsHit != 1);
		}
		else
		{
			flag = false;
		}
		bool bounceOnActors = flag;
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary;
		List<ActorData> list2;
		List<Vector3> list = VectorUtils.CalculateBouncingActorEndpoints(travelBoardSquareWorldPositionForLos, forwardDirection, this.m_maxDistancePerBounce, this.m_maxTotalDistance, this.m_maxBounces, targetingActor, bounceOnActors, this.m_width, targetingActor.GetOpposingTeams(), this.m_maxTargetsHit, out dictionary, out list2, false, null);
		base.ClearActorsInRange();
		this.m_hitActorContext.Clear();
		using (Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> keyValuePair = enumerator.Current;
				base.AddActorInRange(keyValuePair.Key, keyValuePair.Value.m_segmentOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
			}
		}
		for (int i = 0; i < list2.Count; i++)
		{
			AbilityUtil_Targeter_BounceActor.HitActorContext item;
			item.actor = list2[i];
			item.segmentIndex = dictionary[list2[i]].m_endpointIndex;
			this.m_hitActorContext.Add(item);
		}
		List<BoardSquare> chargePathSquares = this.GetChargePathSquares(targetingActor, list, dictionary, list2);
		BoardSquarePathInfo chargePathFromSquareList = this.GetChargePathFromSquareList(targetingActor, chargePathSquares);
		int fromIndex = 0;
		base.EnableAllMovementArrows();
		if (chargePathFromSquareList != null)
		{
			fromIndex = base.AddMovementArrowWithPrevious(targetingActor, chargePathFromSquareList, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
		}
		base.SetMovementArrowEnabledFromIndex(fromIndex, false);
		bool flag2;
		if (this.m_maxTargetsHit > 0)
		{
			flag2 = (list2.Count >= this.m_maxTargetsHit);
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		if (flag3)
		{
			float num = Board.Get().squareSize * AreaEffectUtils.GetActorTargetingRadius();
			Vector3 a = list[list.Count - 1];
			Vector3 vector2;
			if (list.Count > 1)
			{
				vector2 = list[list.Count - 2];
			}
			else
			{
				vector2 = travelBoardSquareWorldPositionForLos;
			}
			Vector3 vector3 = vector2;
			Vector3 normalized = (a - vector3).normalized;
			Vector3 lhs = list2[list2.Count - 1].GetTravelBoardSquareWorldPosition() - vector3;
			list[list.Count - 1] = vector3 + (Vector3.Dot(lhs, normalized) + num) * normalized;
		}
		if (this.m_includeAlliesInBetween)
		{
			List<ActorData> orderedHitActors = new List<ActorData>();
			Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary2 = AreaEffectUtils.FindBouncingLaserTargets(travelBoardSquareWorldPositionForLos, ref list, this.m_width, targetingActor.GetTeams(), -1, false, targetingActor, orderedHitActors, false);
			foreach (KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> keyValuePair2 in dictionary2)
			{
				base.AddActorInRange(keyValuePair2.Key, keyValuePair2.Value.m_segmentOrigin, targetingActor, AbilityTooltipSubject.Secondary, false);
			}
		}
		if (this.m_affectsTargetingActor)
		{
			base.AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self, false);
		}
		this.CreateLaserHighlights(travelBoardSquareWorldPositionForLos, list, false);
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBounceLaser(this.m_indicatorHandler, travelBoardSquareWorldPositionForLos, list, this.m_width, targetingActor, false);
			base.HideUnusedSquareIndicators();
		}
	}

	public List<BoardSquare> GetChargePathSquares(ActorData caster, List<Vector3> endPoints, Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> laserTargets, List<ActorData> orderedHitActors)
	{
		for (int i = endPoints.Count - 1; i > 0; i--)
		{
			Vector3 start = endPoints[i - 1];
			Vector3 end = endPoints[i];
			BoardSquare lastValidBoardSquareInLine = KnockbackUtils.GetLastValidBoardSquareInLine(start, end, true, false, float.MaxValue);
			if (lastValidBoardSquareInLine != null && lastValidBoardSquareInLine.IsBaselineHeight())
			{
				break;
			}
			endPoints.RemoveAt(i);
		}
		Vector3 vector = endPoints[endPoints.Count - 1];
		Vector3 vector2;
		if (endPoints.Count >= 2)
		{
			vector2 = endPoints[endPoints.Count - 2];
		}
		else
		{
			vector2 = caster.GetTravelBoardSquareWorldPositionForLos();
		}
		Vector3 vector3 = vector2;
		Vector3 a = vector - vector3;
		float magnitude = a.magnitude;
		a.Normalize();
		Vector3 end2 = vector - Mathf.Min(0.5f, magnitude / 2f) * a;
		float num = 0f;
		BoardSquare boardSquare;
		if (this.m_maxTargetsHit > 0 && laserTargets.Count >= this.m_maxTargetsHit)
		{
			boardSquare = orderedHitActors[orderedHitActors.Count - 1].GetCurrentBoardSquare();
			num = -0.5f;
		}
		else
		{
			boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(vector3, end2, true, false, float.MaxValue);
		}
		if (boardSquare != null && boardSquare != caster.GetCurrentBoardSquare())
		{
			float num2 = VectorUtils.HorizontalPlaneDistInWorld(vector3, boardSquare.ToVector3());
			float maxDistance = Mathf.Max(0f, num2 + num);
			boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(vector3, vector, true, false, maxDistance);
		}
		List<BoardSquare> list = new List<BoardSquare>();
		if (boardSquare != null)
		{
			list.Add(caster.GetCurrentBoardSquare());
			for (int j = 0; j < endPoints.Count; j++)
			{
				Vector3 vector4;
				if (j == 0)
				{
					vector4 = list[j].ToVector3();
				}
				else
				{
					vector4 = endPoints[j - 1];
				}
				BoardSquare y = list[list.Count - 1];
				Vector3 a2 = endPoints[j] - vector4;
				a2.y = 0f;
				a2.Normalize();
				Vector3 b = a2 / 2f;
				if (j > 0)
				{
					BoardSquare boardSquare2 = Board.Get().GetBoardSquare(vector4 + b);
					if (boardSquare2 != null)
					{
						if (boardSquare2 != y && boardSquare2.IsBaselineHeight())
						{
							list.Add(boardSquare2);
							y = boardSquare2;
						}
					}
				}
				if (j == endPoints.Count - 1 && boardSquare != y)
				{
					list.Add(boardSquare);
				}
				else
				{
					BoardSquare boardSquare3 = Board.Get().GetBoardSquare(endPoints[j] - b);
					if (boardSquare3 != null && !boardSquare3.IsBaselineHeight())
					{
						BoardSquare lastValidBoardSquareInLine2 = KnockbackUtils.GetLastValidBoardSquareInLine(vector4, endPoints[j], true, false, float.MaxValue);
						if (lastValidBoardSquareInLine2 != null && lastValidBoardSquareInLine2.IsBaselineHeight())
						{
							boardSquare3 = lastValidBoardSquareInLine2;
						}
					}
					if (boardSquare3 != null && boardSquare3 != y)
					{
						list.Add(boardSquare3);
					}
				}
			}
			ActorData occupantActor = boardSquare.OccupantActor;
			if (occupantActor != null && occupantActor != caster && occupantActor.IsVisibleToClient())
			{
				Vector3 testDir = vector3 - boardSquare.ToVector3();
				testDir.y = 0f;
				testDir.Normalize();
				BoardSquare secondToLastInOrigPath = null;
				if (list.Count > 1)
				{
					secondToLastInOrigPath = list[list.Count - 2];
				}
				BoardSquare endSquareForOccupant = this.GetEndSquareForOccupant(boardSquare, testDir, caster, secondToLastInOrigPath);
				list.Add(endSquareForOccupant);
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
		int i = 0;
		while (i < 3)
		{
			if (!(boardSquare == null))
			{
				goto IL_1BE;
			}
			else
			{
				List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(lastSquare, i, true);
				for (int j = 0; j < squaresInBorderLayer.Count; j++)
				{
					BoardSquare boardSquare2 = squaresInBorderLayer[j];
					if (boardSquare2.IsBaselineHeight())
					{
						if (!(boardSquare2.OccupantActor == null))
						{
							if (!(boardSquare2.OccupantActor == caster))
							{
								if (boardSquare2.OccupantActor.IsVisibleToClient())
								{
									goto IL_17F;
								}
							}
						}
						int num2;
						bool flag = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquare2, lastSquare, false, out num2);
						if (flag)
						{
							Vector3 rhs = boardSquare2.ToVector3() - lastSquare.ToVector3();
							rhs.y = 0f;
							rhs.Normalize();
							float num3 = Vector3.Dot(testDir, rhs);
							if (secondToLastInOrigPath != null)
							{
								if (secondToLastInOrigPath == boardSquare2)
								{
									num3 += 0.5f;
								}
							}
							if (lastSquare.symbol_0013(boardSquare2.x, boardSquare2.y))
							{
								num3 -= 2f;
							}
							if (!(boardSquare == null))
							{
								if (num3 <= num)
								{
									goto IL_17F;
								}
							}
							boardSquare = boardSquare2;
							num = num3;
						}
					}
					IL_17F:;
				}
				i++;
			}
		}
		IL_1BE:
		if (boardSquare == null)
		{
			boardSquare = lastSquare;
		}
		return boardSquare;
	}

	private BoardSquarePathInfo GetChargePathFromSquareList(ActorData charger, List<BoardSquare> squaresInPath)
	{
		BoardSquarePathInfo boardSquarePathInfo = null;
		for (int i = 1; i < squaresInPath.Count; i++)
		{
			BoardSquare destination = squaresInPath[i];
			BoardSquare startSquare = squaresInPath[i - 1];
			BoardSquarePathInfo boardSquarePathInfo2 = KnockbackUtils.BuildStraightLineChargePath(charger, destination, startSquare, true);
			for (BoardSquarePathInfo boardSquarePathInfo3 = boardSquarePathInfo2; boardSquarePathInfo3 != null; boardSquarePathInfo3 = boardSquarePathInfo3.next)
			{
			}
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
		}
		return boardSquarePathInfo;
	}

	public struct HitActorContext
	{
		public ActorData actor;

		public int segmentIndex;
	}
}
