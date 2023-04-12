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
		return m_knockedbackActorToSources.TryGetValue(target, out List<ActorData> onTarget) ? onTarget : null;
	}

	public void ProcessKnockbacks(List<AbilityRequest> allRequests) // , out List<ActorData> actorsThatWillBeSeenButArentMoving in rogues
	{
		m_knockedBackActors.Clear();
		m_mostRecentRelevantEventTime = Time.time;
		List<ActorData> list = ServerActionBuffer.Get().IdentifyActorsDyingBeforeKnockbackMovement();
		foreach (AbilityRequest abilityRequest in allRequests)
		{
			if (abilityRequest.m_ability.RunPriority != AbilityPriority.Combat_Knockback)
			{
				continue;
			}
			foreach (KeyValuePair<ActorData, Vector2> target in abilityRequest.m_additionalData.m_abilityResults.GetKnockbackTargets())
			{
				ActorData actor = target.Key;
				Vector2 knockbackVector = target.Value;
				if (actor == null
				    || actor.GetActorStatus().IsKnockbackImmune(true)
				    || list.Contains(actor))
				{
					continue;
				}
				if (!m_actorIncomingKnockbacks.ContainsKey(actor))
				{
					m_actorIncomingKnockbacks.Add(actor, new KnockbackHits());
				}
				m_actorIncomingKnockbacks[actor].OnKnockbackProcessed(knockbackVector);
				if (!m_actorOutgoingKnockbacks.ContainsKey(abilityRequest.m_caster))
				{
					m_actorOutgoingKnockbacks.Add(abilityRequest.m_caster, new KnockbackHits());
				}
				m_actorOutgoingKnockbacks[abilityRequest.m_caster].OnKnockbackProcessed(knockbackVector);
				if (!m_knockedbackActorToSources.ContainsKey(actor))
				{
					m_knockedbackActorToSources[actor] = new List<ActorData>();
				}
				if (!m_knockedbackActorToSources[actor].Contains(abilityRequest.m_caster))
				{
					m_knockedbackActorToSources[actor].Add(abilityRequest.m_caster);
				}
				if (actor.GetActorBehavior() != null)
				{
					actor.GetActorBehavior().AddRootOrKnockbackSourceActor(abilityRequest.m_caster);
				}
			}
		}
		foreach (Effect.EffectKnockbackTargets effectKnockbackTargets in ServerEffectManager.Get().FindKnockbackTargetSets())
		{
			ActorData sourceActor = effectKnockbackTargets.m_sourceActor;
			foreach (KeyValuePair<ActorData, Vector2> target in effectKnockbackTargets.m_knockbackTargets)
			{
				ActorData actor = target.Key;
				Vector2 knockbackVector = target.Value;
				if (actor == null
				    || actor.GetActorStatus().IsKnockbackImmune(true)
				    || list.Contains(actor))
				{
					continue;
				}
				if (!m_actorIncomingKnockbacks.ContainsKey(actor))
				{
					m_actorIncomingKnockbacks.Add(actor, new KnockbackHits());
				}
				m_actorIncomingKnockbacks[actor].OnKnockbackProcessed(knockbackVector);
				if (!m_actorOutgoingKnockbacks.ContainsKey(sourceActor))
				{
					m_actorOutgoingKnockbacks.Add(sourceActor, new KnockbackHits());
				}
				m_actorOutgoingKnockbacks[sourceActor].OnKnockbackProcessed(knockbackVector);
				if (actor.GetActorBehavior() != null)
				{
					actor.GetActorBehavior().AddRootOrKnockbackSourceActor(sourceActor);
				}
			}
		}
		foreach (KeyValuePair<ActorData, KnockbackHits> actorKnockbackHits in m_actorIncomingKnockbacks)
		{
			actorKnockbackHits.Value.CalculateTotalKnockback(actorKnockbackHits.Key);
		}
		List<BoardSquare> invalidSquares = new List<BoardSquare>();
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (!actorData.IsDead() && actorData.GetPassiveData() != null)
			{
				actorData.GetPassiveData().AddInvalidKnockbackDestinations(m_actorIncomingKnockbacks, invalidSquares);
			}
		}
		List<KnockbackHits> allKnockbackHits = new List<KnockbackHits>(m_actorIncomingKnockbacks.Values);
		for (int i = allKnockbackHits.Count - 1; i >= 0; i--)
		{
			if (allKnockbackHits[i].GetKnockbackPath() == null)
			{
				allKnockbackHits.RemoveAt(i);
			}
		}
		if (allKnockbackHits.Count > 0)
		{
			List<ActorData> knockbackedActors = new List<ActorData>(m_actorIncomingKnockbacks.Keys);
			List<BoardSquare> destinationsSoFar = new List<BoardSquare>();
			allKnockbackHits.Sort(delegate(KnockbackHits knockback1, KnockbackHits knockback2)
			{
				float dist1 = knockback1.GetKnockbackPath().FindDistanceToEnd();
				float dist2 = knockback2.GetKnockbackPath().FindDistanceToEnd();
				return dist1.CompareTo(dist2);
			});
			foreach (KnockbackHits knockbackHits in allKnockbackHits)
			{
				ActorData knockbackedActor = knockbackHits.GetKnockbackedActor();
				BoardSquare knockbackEndSquare = knockbackHits.GetKnockbackEndSquare();
				List<BoardSquare> potentialDestinations = new List<BoardSquare>();
				int borderRadius = 0;
				while (borderRadius <= 2 || (potentialDestinations.Count == 0 && borderRadius <= 4))
				{
					List<BoardSquare> potentialDestinationForKnockback = GetPotentialDestinationForKnockback(
						knockbackedActor,
						knockbackEndSquare,
						borderRadius, 
						destinationsSoFar,
						invalidSquares,
						knockbackedActors,
						true);
					potentialDestinations.AddRange(potentialDestinationForKnockback);
					if (potentialDestinationForKnockback.Count > 0 && borderRadius == 0)
					{
						break;
					}
					borderRadius++;
				}
				BoardSquare boardSquare = null;
				float maxWeight = -100f;
				Vector3 src = knockbackedActor.GetCurrentBoardSquare().ToVector3();
				Vector3 desiredVector = knockbackEndSquare.ToVector3() - src;
				desiredVector.y = 0f;
				desiredVector.Normalize();
				Vector3 backVector = -desiredVector;
				foreach (BoardSquare dst in potentialDestinations)
				{
					float weight;
					if (dst == knockbackEndSquare)
					{
						weight = 10000f;
					}
					else
					{
						Vector3 offset = dst.ToVector3() - knockbackEndSquare.ToVector3();
						offset.y = 0f;
						offset.Normalize();
						float angleFromSource = Vector3.Angle(backVector, offset);
						Vector3 vector = dst.ToVector3() - src;
						vector.y = 0f;
						bool isDesiredDirection = Vector3.Dot(desiredVector, vector) >= 0f;
						weight = 0.45f * Vector3.Dot(backVector, offset);
						if (!isDesiredDirection)
						{
							weight -= 2f;
						}
						else if (angleFromSource <= 15f)
						{
							weight += 0.2f;
						}
						if (Board.Get().GetSquaresAreAdjacent(dst, knockbackEndSquare))
						{
							weight += 1f;
						}
					}
					if (boardSquare == null || weight > maxWeight)
					{
						boardSquare = dst;
						maxWeight = weight;
					}
				}
				if (boardSquare != null)
				{
					if (boardSquare != knockbackEndSquare)
					{
						knockbackHits.ReassignDestinationBeforeStabilization(boardSquare);
					}
					destinationsSoFar.Add(boardSquare);
				}
			}
		}
		m_knockbackStabilizer.StabilizeKnockbacks(m_actorIncomingKnockbacks, invalidSquares);
		// GatherGameplayResultsInResponseToKnockbacks(out actorsThatWillBeSeenButArentMoving); // rogues
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
			ServerGameplayUtils.SetServerLastKnownPositionsForMovement(
				movementCollection,
				out actorsThatWillBeSeenButArentMoving,
				out _);
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
