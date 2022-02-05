// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only -- missing in reactor
#if SERVER
public class ServerKnockbackManager
{
	private static ServerKnockbackManager s_instance;

	private ServerKnockbackStabilizer m_knockbackStabilizer;
	private Dictionary<ActorData, KnockbackHits> m_actorIncomingKnockbacks;
	private Dictionary<ActorData, KnockbackHits> m_actorOutgoingKnockbacks;
	private Dictionary<ActorData, List<ActorData>> m_knockedbackActorToSources;
	private List<ActorData> m_knockedBackActors;
	private float m_mostRecentRelevantEventTime;
	private bool m_failsafeErrorLogged;

	public static ServerKnockbackManager Get()
	{
		return s_instance;
	}

	~ServerKnockbackManager()
	{
		if (s_instance != null && s_instance == this)
		{
			s_instance = null;
		}
	}

	public ServerKnockbackManager()
	{
		s_instance = this;
		m_knockbackStabilizer = new ServerKnockbackStabilizer();
		m_actorIncomingKnockbacks = new Dictionary<ActorData, KnockbackHits>();
		m_actorOutgoingKnockbacks = new Dictionary<ActorData, KnockbackHits>();
		m_knockedbackActorToSources = new Dictionary<ActorData, List<ActorData>>();
		m_knockedBackActors = new List<ActorData>();
	}

	public List<ActorData> GetActorsBeingKnockedBack()
	{
		return new List<ActorData>(m_actorIncomingKnockbacks.Keys);
	}

	public List<ActorData> GetKnockbackSourceActorsOnTarget(ActorData target)
	{
		if (m_knockedbackActorToSources.ContainsKey(target))
		{
			return m_knockedbackActorToSources[target];
		}
		return null;
	}

	public void ProcessKnockbacks(List<AbilityRequest> allRequests, out List<ActorData> actorsThatWillBeSeenButArentMoving)
	{
		m_knockedBackActors.Clear();
		m_mostRecentRelevantEventTime = Time.time;
		List<ActorData> list = ServerActionBuffer.Get().IdentifyActorsDyingBeforeKnockbackMovement();
		foreach (AbilityRequest abilityRequest in allRequests)
		{
			if (abilityRequest.m_ability.RunPriority == AbilityPriority.Combat_Knockback)
			{
				foreach (KeyValuePair<ActorData, Vector2> keyValuePair in abilityRequest.m_additionalData.m_abilityResults.GetKnockbackTargets())
				{
					ActorData key = keyValuePair.Key;
					Vector2 value = keyValuePair.Value;
					if (key != null && !key.GetActorStatus().IsKnockbackImmune(true) && !list.Contains(key))
					{
						if (!m_actorIncomingKnockbacks.ContainsKey(key))
						{
							KnockbackHits value2 = new KnockbackHits();
							m_actorIncomingKnockbacks.Add(key, value2);
						}
						m_actorIncomingKnockbacks[key].OnKnockbackProcessed(value);
						if (!m_actorOutgoingKnockbacks.ContainsKey(abilityRequest.m_caster))
						{
							KnockbackHits value3 = new KnockbackHits();
							m_actorOutgoingKnockbacks.Add(abilityRequest.m_caster, value3);
						}
						m_actorOutgoingKnockbacks[abilityRequest.m_caster].OnKnockbackProcessed(value);
						if (!m_knockedbackActorToSources.ContainsKey(key))
						{
							m_knockedbackActorToSources[key] = new List<ActorData>();
						}
						if (!m_knockedbackActorToSources[key].Contains(abilityRequest.m_caster))
						{
							m_knockedbackActorToSources[key].Add(abilityRequest.m_caster);
						}
						if (key.GetActorBehavior() != null)
						{
							key.GetActorBehavior().AddRootOrKnockbackSourceActor(abilityRequest.m_caster);
						}
					}
				}
			}
		}
		foreach (Effect.EffectKnockbackTargets effectKnockbackTargets in ServerEffectManager.Get().FindKnockbackTargetSets())
		{
			ActorData sourceActor = effectKnockbackTargets.m_sourceActor;
			foreach (KeyValuePair<ActorData, Vector2> keyValuePair2 in effectKnockbackTargets.m_knockbackTargets)
			{
				ActorData key2 = keyValuePair2.Key;
				Vector2 value4 = keyValuePair2.Value;
				if (key2 != null && !key2.GetActorStatus().IsKnockbackImmune(true) && !list.Contains(key2))
				{
					if (!m_actorIncomingKnockbacks.ContainsKey(key2))
					{
						KnockbackHits value5 = new KnockbackHits();
						m_actorIncomingKnockbacks.Add(key2, value5);
					}
					m_actorIncomingKnockbacks[key2].OnKnockbackProcessed(value4);
					if (!m_actorOutgoingKnockbacks.ContainsKey(sourceActor))
					{
						KnockbackHits value6 = new KnockbackHits();
						m_actorOutgoingKnockbacks.Add(sourceActor, value6);
					}
					m_actorOutgoingKnockbacks[sourceActor].OnKnockbackProcessed(value4);
					if (key2.GetActorBehavior() != null)
					{
						key2.GetActorBehavior().AddRootOrKnockbackSourceActor(sourceActor);
					}
				}
			}
		}
		foreach (KeyValuePair<ActorData, KnockbackHits> keyValuePair3 in m_actorIncomingKnockbacks)
		{
			keyValuePair3.Value.CalculateTotalKnockback(keyValuePair3.Key);
		}
		List<BoardSquare> list2 = new List<BoardSquare>();
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (!actorData.IsDead() && actorData.GetPassiveData() != null)
			{
				actorData.GetPassiveData().AddInvalidKnockbackDestinations(m_actorIncomingKnockbacks, list2);
			}
		}
		List<KnockbackHits> list3 = new List<KnockbackHits>(m_actorIncomingKnockbacks.Values);
		for (int i = list3.Count - 1; i >= 0; i--)
		{
			if (list3[i].GetKnockbackPath() == null)
			{
				list3.RemoveAt(i);
			}
		}
		if (list3.Count > 0)
		{
			List<ActorData> knockbackedActors = new List<ActorData>(m_actorIncomingKnockbacks.Keys);
			List<BoardSquare> list4 = new List<BoardSquare>();
			list3.Sort(delegate(KnockbackHits knockback1, KnockbackHits knockback2)
			{
				float num5 = knockback1.GetKnockbackPath().FindDistanceToEnd();
				float value7 = knockback2.GetKnockbackPath().FindDistanceToEnd();
				return num5.CompareTo(value7);
			});
			foreach (KnockbackHits knockbackHits in list3)
			{
				ActorData knockbackedActor = knockbackHits.GetKnockbackedActor();
				BoardSquare knockbackEndSquare = knockbackHits.GetKnockbackEndSquare();
				List<BoardSquare> list5 = new List<BoardSquare>();
				int num = 0;
				while (num <= 2 || (list5.Count == 0 && num <= 4))
				{
					List<BoardSquare> potentialDestinationForKnockback = GetPotentialDestinationForKnockback(knockbackedActor, knockbackEndSquare, num, list4, list2, knockbackedActors, true);
					list5.AddRange(potentialDestinationForKnockback);
					if (potentialDestinationForKnockback.Count > 0 && num == 0)
					{
						break;
					}
					num++;
				}
				BoardSquare boardSquare = null;
				float num2 = -100f;
				Vector3 vector = knockbackedActor.GetCurrentBoardSquare().ToVector3();
				Vector3 vector2 = knockbackEndSquare.ToVector3() - vector;
				vector2.y = 0f;
				vector2.Normalize();
				Vector3 vector3 = -vector2;
				foreach (BoardSquare boardSquare2 in list5)
				{
					float num3;
					if (boardSquare2 == knockbackEndSquare)
					{
						num3 = 10000f;
					}
					else
					{
						Vector3 vector4 = boardSquare2.ToVector3() - knockbackEndSquare.ToVector3();
						vector4.y = 0f;
						vector4.Normalize();
						float num4 = Vector3.Angle(vector3, vector4);
						Vector3 vector5 = boardSquare2.ToVector3() - vector;
						vector5.y = 0f;
						bool flag = Vector3.Dot(vector2, vector5) >= 0f;
						num3 = 0.45f * Vector3.Dot(vector3, vector4);
						if (flag)
						{
							if (num4 <= 15f)
							{
								num3 += 0.2f;
							}
						}
						else
						{
							num3 -= 2f;
						}
						if (Board.Get().GetSquaresAreAdjacent(boardSquare2, knockbackEndSquare))
						{
							num3 += 1f;
						}
					}
					if (boardSquare == null || num3 > num2)
					{
						boardSquare = boardSquare2;
						num2 = num3;
					}
				}
				if (boardSquare != null)
				{
					if (boardSquare != knockbackEndSquare)
					{
						knockbackHits.ReassignDestinationBeforeStabilization(boardSquare);
					}
					list4.Add(boardSquare);
				}
			}
		}
		m_knockbackStabilizer.StabilizeKnockbacks(m_actorIncomingKnockbacks, list2);
		GatherGameplayResultsInResponseToKnockbacks(out actorsThatWillBeSeenButArentMoving);
	}

	private List<BoardSquare> GetPotentialDestinationForKnockback(ActorData mover, BoardSquare center, int borderRadius, List<BoardSquare> destinationsSoFar, List<BoardSquare> invalidSquares, List<ActorData> knockbackedActors, bool requireLosToCenter)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(center, borderRadius, requireLosToCenter);
		for (int i = 0; i < squaresInBorderLayer.Count; i++)
		{
			BoardSquare boardSquare = squaresInBorderLayer[i];
			if (IsValidAsPotentialDestination(mover, boardSquare, destinationsSoFar, invalidSquares, knockbackedActors))
			{
				list.Add(boardSquare);
			}
		}
		return list;
	}

	private bool IsValidAsPotentialDestination(ActorData mover, BoardSquare square, List<BoardSquare> destinationsSoFar, List<BoardSquare> invalidSquares, List<ActorData> knockbackActors)
	{
		bool result;
		if (square == null || !square.IsValidForGameplay() || invalidSquares.Contains(square) || destinationsSoFar.Contains(square))
		{
			result = false;
		}
		else
		{
			bool flag = true;
			if (square.OccupantActor != null)
			{
				ActorData occupantActor = square.OccupantActor;
				flag = (occupantActor == mover || knockbackActors.Contains(occupantActor));
			}
			int num;
			bool flag2 = flag && KnockbackUtils.CanBuildStraightLineChargePath(mover, square, mover.GetCurrentBoardSquare(), true, out num);
			result = (flag && flag2);
		}
		return result;
	}

	public void GatherGameplayResultsInResponseToKnockbacks(out List<ActorData> actorsThatWillBeSeenButArentMoving)
	{
		MovementCollection movementCollection = new MovementCollection(m_actorIncomingKnockbacks);
		ServerEffectManager.Get().GatherAllEffectResultsInResponseToKnockbacks(movementCollection);
		BarrierManager.Get().GatherAllBarrierResultsInResponseToKnockbacks(movementCollection);
		PowerUpManager.Get().GatherAllPowerupResultsInResponseToKnockbacks(movementCollection);
		// TODO CTF CTC
		//if (CaptureTheFlag.Get() != null)
		//{
		//	CaptureTheFlag.Get().GatherResultsInResponseToKnockbacks(movementCollection);
		//}
		//if (CollectTheCoins.Get() != null)
		//{
		//	CollectTheCoins.Get().GatherResultsInResponseToKnockbacks(movementCollection);
		//}
		if (m_actorIncomingKnockbacks.Count > 0)
		{
			List<ActorData> list;
			ServerGameplayUtils.SetServerLastKnownPositionsForMovement(movementCollection, out actorsThatWillBeSeenButArentMoving, out list);
		}
		else
		{
			actorsThatWillBeSeenButArentMoving = new List<ActorData>();
		}
		ServerMovementManager.Get().ServerMovementManager_OnMovementStart(movementCollection, ServerMovementManager.MovementType.Knockback);
	}

	public void OnKnockbackTauntProgressed()
	{
		m_mostRecentRelevantEventTime = Time.time;
	}

	public void OnKnockbackAnimStarted(ActorData caster)
	{
		if (m_actorIncomingKnockbacks.ContainsKey(caster))
		{
			bool flag = !m_actorIncomingKnockbacks[caster].ObservedLastHit();
			bool flag2 = m_actorOutgoingKnockbacks.ContainsKey(caster) && !m_actorOutgoingKnockbacks[caster].ObservedLastHit();
			if (!flag && !flag2)
			{
				ResolveKnockbacksForActor(caster);
			}
		}
	}

	public void ResolveKnockbacksForActor(ActorData actor)
	{
		ActorStatus actorStatus = actor.GetActorStatus();
		if (actor.IsDead())
		{
			return;
		}
		if (actorStatus.IsKnockbackImmune(true))
		{
			m_actorIncomingKnockbacks.Remove(actor);
			return;
		}
		if (!m_actorIncomingKnockbacks.ContainsKey(actor))
		{
			Log.Error("Resolving knockbacks for an actor that isn't tracked.");
			return;
		}
		KnockbackHits knockbackHits = m_actorIncomingKnockbacks[actor];
		BoardSquarePathInfo knockbackPath = knockbackHits.GetKnockbackPath();
		BoardSquare knockbackEndSquare = knockbackHits.GetKnockbackEndSquare();
		if (knockbackPath != null && knockbackEndSquare != null)
		{
			actor.BroadcastMoveToBoardSquare(knockbackEndSquare, ActorData.MovementType.Knockback, knockbackPath, ActorData.TeleportType.NotATeleport, GameEventManager.EventType.Invalid, false);
			m_knockedBackActors.Add(actor);
		}
		m_actorIncomingKnockbacks.Remove(actor);
		m_mostRecentRelevantEventTime = Time.time;
		if (actor.GetActorStatus() != null && !actor.GetActorStatus().HasStatus(StatusType.KnockedBack, true))
		{
			actor.GetActorStatus().AddStatus(StatusType.KnockedBack, 0);
		}
		if (m_actorIncomingKnockbacks.Count == 0)
		{
			ServerMovementManager.Get().OnKnockbackHitsConcluded();
		}
	}

	public void OnAbilityPhaseEnd(AbilityPriority abilityPhase)
	{
		if (abilityPhase == AbilityPriority.Combat_Knockback && m_actorIncomingKnockbacks.Count > 0)
		{
			string text = "";
			foreach (ActorData actorData in m_actorIncomingKnockbacks.Keys)
			{
				text = text + " " + actorData.DisplayName;
			}
			Log.Error($"Ending knockback phase with {m_actorIncomingKnockbacks.Count} tracked, un-removed knockbacks for actors {text}:");
			ClearStoredData();
		}
	}

	public void ClearStoredData()
	{
		m_actorIncomingKnockbacks.Clear();
		m_actorOutgoingKnockbacks.Clear();
		m_knockedbackActorToSources.Clear();
		m_mostRecentRelevantEventTime = 0f;
		m_failsafeErrorLogged = false;
		m_knockedBackActors.Clear();
	}

	public bool KnockbacksDoneResolving()
	{
		bool flag = true;
		if (m_actorIncomingKnockbacks.Count > 0)
		{
			flag = false;
		}
		else if (ServerMovementManager.Get() != null && ServerMovementManager.Get().IsKnockbackMovementCurrentlyHappening())
		{
			flag = false;
		}
		if (!flag && m_mostRecentRelevantEventTime != 0f && Time.time - m_mostRecentRelevantEventTime > 8f)
		{
			if (!m_failsafeErrorLogged)
			{
				Log.Warning($"The ServerKnockbackManager failsafe activated since there have not been any relevant knockback events in the last {8f} seconds.");
				m_failsafeErrorLogged = true;
			}
			flag = true;
		}
		return flag;
	}

	public BoardSquare GetIncomingKnockbackEndSquare(ActorData actorData)
	{
		if (m_actorIncomingKnockbacks == null || !m_actorIncomingKnockbacks.ContainsKey(actorData))
		{
			return null;
		}
		return m_actorIncomingKnockbacks[actorData].GetKnockbackEndSquare();
	}

	public class KnockbackHits
	{
		private int m_numHitsExpected;
		private int m_numHitsObserved;
		private List<Vector2> m_impulses;
		private Vector2 m_totalKnockback;
		private BoardSquarePathInfo m_knockbackPath;
		private BoardSquare m_knockbackEndSquare;
		private ActorData m_knockbackedActor;

		public KnockbackHits()
		{
			m_numHitsExpected = 0;
			m_numHitsObserved = 0;
			m_impulses = new List<Vector2>();
		}

		public bool ObservedLastHit()
		{
			if (m_numHitsObserved > m_numHitsExpected)
			{
				Log.Error("Checking if an actor has taken his last hit, but he's already gone past that.");
			}
			return m_numHitsObserved >= m_numHitsExpected;
		}

		public void CalculateTotalKnockback(ActorData knockbackedActor)
		{
			m_knockbackedActor = knockbackedActor;
			m_totalKnockback = new Vector2(0f, 0f);
			m_knockbackPath = null;
			m_knockbackEndSquare = null;
			foreach (Vector2 vector in m_impulses)
			{
				m_totalKnockback += vector;
			}
			BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildKnockbackPathFromVector(knockbackedActor, m_totalKnockback);
			if (boardSquarePathInfo == null)
			{
				Log.Error($"Couldn't build a knockback path for {knockbackedActor.DisplayName} in direction ({m_totalKnockback.x}, {m_totalKnockback.y})");
				return;
			}
			BoardSquarePathInfo pathEndpoint = boardSquarePathInfo.GetPathEndpoint();
			if (pathEndpoint == null)
			{
				Log.Error($"Couldn't get a knockback path end for {knockbackedActor.DisplayName} in direction ({m_totalKnockback.x}, {m_totalKnockback.y})");
				return;
			}
			BoardSquare square = pathEndpoint.square;
			if (square != null)
			{
				m_knockbackPath = boardSquarePathInfo;
				m_knockbackEndSquare = square;
				return;
			}
			Log.Error($"Knockback path end is null for {knockbackedActor.DisplayName} in direction ({m_totalKnockback.x}, {m_totalKnockback.y})");
		}

		public void ReassignDestinationBeforeStabilization(BoardSquare newDestination)
		{
			if (newDestination != null && m_knockbackEndSquare != newDestination)
			{
				BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(m_knockbackedActor, newDestination, m_knockbackedActor.GetCurrentBoardSquare(), true);
				if (boardSquarePathInfo != null)
				{
					m_knockbackPath = boardSquarePathInfo;
					m_knockbackEndSquare = newDestination;
				}
			}
		}

		public Vector2 GetTotalKnockback()
		{
			return m_totalKnockback;
		}

		public BoardSquarePathInfo GetKnockbackPath()
		{
			return m_knockbackPath;
		}

		public BoardSquare GetKnockbackEndSquare()
		{
			return m_knockbackEndSquare;
		}

		public ActorData GetKnockbackedActor()
		{
			return m_knockbackedActor;
		}

		public void OnKnockbackPathStabilized(BoardSquare newEndSquare)
		{
			BoardSquarePathInfo pathEndpoint = m_knockbackPath.GetPathEndpoint();
			if (newEndSquare != pathEndpoint.square)
			{
				Log.Error("Stabilizing a knockback path, but the new end square mismatches the path's end square.");
			}
			m_knockbackEndSquare = newEndSquare;
		}

		public void OnKnockbackProcessed(Vector2 delta)
		{
			m_impulses.Add(delta);
			m_numHitsExpected++;
		}

		public void OnKnockbackHit()
		{
			m_numHitsObserved++;
		}
	}
}
#endif
