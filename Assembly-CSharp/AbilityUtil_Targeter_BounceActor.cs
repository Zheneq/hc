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
		float num = this.m_width * Board.\u000E().squareSize;
		if (this.m_highlights != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BounceActor.CreateLaserHighlights(Vector3, List<Vector3>, bool)).MethodHandle;
			}
			if (this.m_highlights.Count >= 2)
			{
				goto IL_AE;
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			Vector3 a = laserAnglePoints[laserAnglePoints.Count - 1];
			Vector3 vector2;
			if (laserAnglePoints.Count >= 2)
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
		Vector3 vector = targetingActor.\u0015();
		Vector3 vector2;
		if (currentTarget == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BounceActor.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			vector2 = targetingActor.transform.forward;
		}
		else
		{
			vector2 = currentTarget.AimDirection;
		}
		Vector3 forwardDirection = vector2;
		bool flag;
		if (this.m_bounceOnEnemyActor)
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
			flag = (this.m_maxTargetsHit != 1);
		}
		else
		{
			flag = false;
		}
		bool bounceOnActors = flag;
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary;
		List<ActorData> list2;
		List<Vector3> list = VectorUtils.CalculateBouncingActorEndpoints(vector, forwardDirection, this.m_maxDistancePerBounce, this.m_maxTotalDistance, this.m_maxBounces, targetingActor, bounceOnActors, this.m_width, targetingActor.\u0015(), this.m_maxTargetsHit, out dictionary, out list2, false, null);
		base.ClearActorsInRange();
		this.m_hitActorContext.Clear();
		using (Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> keyValuePair = enumerator.Current;
				base.AddActorInRange(keyValuePair.Key, keyValuePair.Value.m_segmentOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			fromIndex = base.AddMovementArrowWithPrevious(targetingActor, chargePathFromSquareList, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
		}
		base.SetMovementArrowEnabledFromIndex(fromIndex, false);
		bool flag2;
		if (this.m_maxTargetsHit > 0)
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
			flag2 = (list2.Count >= this.m_maxTargetsHit);
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		if (flag3)
		{
			float num = Board.\u000E().squareSize * AreaEffectUtils.GetActorTargetingRadius();
			Vector3 a = list[list.Count - 1];
			Vector3 vector3;
			if (list.Count > 1)
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
				vector3 = list[list.Count - 2];
			}
			else
			{
				vector3 = vector;
			}
			Vector3 vector4 = vector3;
			Vector3 normalized = (a - vector4).normalized;
			Vector3 lhs = list2[list2.Count - 1].\u0016() - vector4;
			list[list.Count - 1] = vector4 + (Vector3.Dot(lhs, normalized) + num) * normalized;
		}
		if (this.m_includeAlliesInBetween)
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
			List<ActorData> orderedHitActors = new List<ActorData>();
			Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary2 = AreaEffectUtils.FindBouncingLaserTargets(vector, ref list, this.m_width, targetingActor.\u0012(), -1, false, targetingActor, orderedHitActors, false);
			foreach (KeyValuePair<ActorData, AreaEffectUtils.BouncingLaserInfo> keyValuePair2 in dictionary2)
			{
				base.AddActorInRange(keyValuePair2.Key, keyValuePair2.Value.m_segmentOrigin, targetingActor, AbilityTooltipSubject.Secondary, false);
			}
		}
		if (this.m_affectsTargetingActor)
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
			base.AddActorInRange(targetingActor, targetingActor.\u0016(), targetingActor, AbilityTooltipSubject.Self, false);
		}
		this.CreateLaserHighlights(vector, list, false);
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
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
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBounceLaser(this.m_indicatorHandler, vector, list, this.m_width, targetingActor, false);
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
			if (!(lastValidBoardSquareInLine == null))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BounceActor.GetChargePathSquares(ActorData, List<Vector3>, Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>, List<ActorData>)).MethodHandle;
				}
				if (lastValidBoardSquareInLine.\u0016())
				{
					IL_79:
					Vector3 vector = endPoints[endPoints.Count - 1];
					Vector3 vector2;
					if (endPoints.Count >= 2)
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
						vector2 = endPoints[endPoints.Count - 2];
					}
					else
					{
						vector2 = caster.\u0015();
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
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						boardSquare = orderedHitActors[orderedHitActors.Count - 1].\u0012();
						num = -0.5f;
					}
					else
					{
						boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(vector3, end2, true, false, float.MaxValue);
					}
					if (boardSquare != null)
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
						if (boardSquare != caster.\u0012())
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
							float num2 = VectorUtils.HorizontalPlaneDistInWorld(vector3, boardSquare.ToVector3());
							float maxDistance = Mathf.Max(0f, num2 + num);
							boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(vector3, vector, true, false, maxDistance);
						}
					}
					List<BoardSquare> list = new List<BoardSquare>();
					if (boardSquare != null)
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
						list.Add(caster.\u0012());
						for (int j = 0; j < endPoints.Count; j++)
						{
							Vector3 vector4;
							if (j == 0)
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
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								BoardSquare boardSquare2 = Board.\u000E().\u000E(vector4 + b);
								if (boardSquare2 != null)
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
									if (boardSquare2 != y && boardSquare2.\u0016())
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
										list.Add(boardSquare2);
										y = boardSquare2;
									}
								}
							}
							if (j == endPoints.Count - 1)
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
								if (boardSquare != y)
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
									list.Add(boardSquare);
								}
							}
							else
							{
								BoardSquare boardSquare3 = Board.\u000E().\u000E(endPoints[j] - b);
								if (boardSquare3 != null)
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
									if (!boardSquare3.\u0016())
									{
										BoardSquare lastValidBoardSquareInLine2 = KnockbackUtils.GetLastValidBoardSquareInLine(vector4, endPoints[j], true, false, float.MaxValue);
										if (lastValidBoardSquareInLine2 != null)
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
											if (lastValidBoardSquareInLine2.\u0016())
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
												boardSquare3 = lastValidBoardSquareInLine2;
											}
										}
									}
								}
								if (boardSquare3 != null)
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
									if (boardSquare3 != y)
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
										list.Add(boardSquare3);
									}
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
						ActorData occupantActor = boardSquare.OccupantActor;
						if (occupantActor != null && occupantActor != caster)
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
							if (occupantActor.\u0018())
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
								Vector3 testDir = vector3 - boardSquare.ToVector3();
								testDir.y = 0f;
								testDir.Normalize();
								BoardSquare secondToLastInOrigPath = null;
								if (list.Count > 1)
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
									secondToLastInOrigPath = list[list.Count - 2];
								}
								BoardSquare endSquareForOccupant = this.GetEndSquareForOccupant(boardSquare, testDir, caster, secondToLastInOrigPath);
								list.Add(endSquareForOccupant);
							}
						}
					}
					else
					{
						list.Add(caster.\u0012());
					}
					if (list.Count == 1)
					{
						list.Add(caster.\u0012());
					}
					return list;
				}
			}
			endPoints.RemoveAt(i);
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			goto IL_79;
		}
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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					goto IL_1BE;
				}
			}
			else
			{
				List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(lastSquare, i, true);
				for (int j = 0; j < squaresInBorderLayer.Count; j++)
				{
					BoardSquare boardSquare2 = squaresInBorderLayer[j];
					if (boardSquare2.\u0016())
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BounceActor.GetEndSquareForOccupant(BoardSquare, Vector3, ActorData, BoardSquare)).MethodHandle;
						}
						if (!(boardSquare2.OccupantActor == null))
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
							if (!(boardSquare2.OccupantActor == caster))
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
								if (boardSquare2.OccupantActor.\u0018())
								{
									goto IL_17F;
								}
							}
						}
						int num2;
						bool flag = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquare2, lastSquare, false, out num2);
						if (flag)
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
							Vector3 rhs = boardSquare2.ToVector3() - lastSquare.ToVector3();
							rhs.y = 0f;
							rhs.Normalize();
							float num3 = Vector3.Dot(testDir, rhs);
							if (secondToLastInOrigPath != null)
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
								if (secondToLastInOrigPath == boardSquare2)
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
									num3 += 0.5f;
								}
							}
							if (lastSquare.\u0013(boardSquare2.x, boardSquare2.y))
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
								num3 -= 2f;
							}
							if (!(boardSquare == null))
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
								if (num3 <= num)
								{
									goto IL_17F;
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
							boardSquare = boardSquare2;
							num = num3;
						}
					}
					IL_17F:;
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
				i++;
			}
		}
		IL_1BE:
		if (boardSquare == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BounceActor.GetChargePathFromSquareList(ActorData, List<BoardSquare>)).MethodHandle;
			}
			if (boardSquarePathInfo != null)
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
				BoardSquarePathInfo pathEndpoint = boardSquarePathInfo.GetPathEndpoint();
				if (boardSquarePathInfo2 != null)
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
					if (boardSquarePathInfo2.next != null)
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
						if (pathEndpoint.square == boardSquarePathInfo2.square)
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
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		return boardSquarePathInfo;
	}

	public struct HitActorContext
	{
		public ActorData actor;

		public int segmentIndex;
	}
}
