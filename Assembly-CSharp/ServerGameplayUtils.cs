// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// was empty in reactor
#if SERVER
public static class ServerGameplayUtils
{
	public static void SetServerLastKnownPositionsForMovement(
		MovementCollection stabilizedMovement,
		out List<ActorData> actorsThatWillBeSeenButArentMoving_normal,
		out List<ActorData> actorsThatWillBeSeenButArentMoving_chase)
	{
		Dictionary<ActorData, LastKnownPosData> dictionary = new Dictionary<ActorData, LastKnownPosData>();
		foreach (MovementInstance movementInstance in stabilizedMovement.m_movementInstances)
		{
			dictionary.Add(movementInstance.m_mover, new LastKnownPosData(movementInstance));
			movementInstance.m_mover.OnServerLastKnownPosUpdateBegin();
		}
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData != null
			    && !actorData.IsDead()
			    && actorData.GetCurrentBoardSquare() != null
			    && actorData.ServerLastKnownPosSquare != actorData.GetCurrentBoardSquare()
			    && actorData.IsActorVisibleToAnyEnemy())
			{
				actorData.SetServerLastKnownPosSquare(actorData.CurrentBoardSquare, "SetServerLastKnownPositionsForMovement");
			}
		}
		IterateOverLastKnownPosData(ref dictionary, out actorsThatWillBeSeenButArentMoving_normal, stabilizedMovement, false);
		actorsThatWillBeSeenButArentMoving_chase = null;
		foreach (LastKnownPosData lastKnownPosData in dictionary.Values)
		{
			lastKnownPosData.Actor.SwapBoardSquare(lastKnownPosData.m_actorOriginalBoardSquare);
		}
		foreach (ActorData actorData in dictionary.Keys)
		{
			actorData.GetFogOfWar().ImmediateUpdateVisibilityOfSquares();
		}
		foreach (MovementInstance movementInstance in stabilizedMovement.m_movementInstances)
		{
			movementInstance.m_mover.OnServerLastKnownPosUpdateEnd();
		}
	}

	private static void IterateOverLastKnownPosData(
		ref Dictionary<ActorData, LastKnownPosData> moverToLastKnownPosData,
		out List<ActorData> actorsThatWillBeSeenButArentMoving,
		MovementCollection stabilizedMovement,
		bool consideringChasers)
	{
		int num = Mathf.RoundToInt(GetDistanceToConsider(stabilizedMovement, consideringChasers) * 2f) + 1;
		actorsThatWillBeSeenButArentMoving = new List<ActorData>();
		for (int i = 0; i < num; i++)
		{
			float distance = i * 0.5f - 0.01f;
			foreach (LastKnownPosData lastKnownPosData in moverToLastKnownPosData.Values)
			{
				if (lastKnownPosData.WasChase == consideringChasers)
				{
					lastKnownPosData.UpdateCurrentlyConsideredPathFromDistance(distance);
				}
			}
			bool updateFog = false;
			foreach (LastKnownPosData lastKnownPosData in moverToLastKnownPosData.Values)
			{
				if (lastKnownPosData.WasChase == consideringChasers && !updateFog)
				{
					BoardSquare currentBoardSquare = lastKnownPosData.Actor.CurrentBoardSquare;
					lastKnownPosData.Actor.SwapBoardSquare(lastKnownPosData.m_currentlyConsideredPath.square);
					if (currentBoardSquare != lastKnownPosData.Actor.CurrentBoardSquare)
					{
						updateFog = true;
					}
				}
			}
			if (updateFog)
			{
				foreach (ActorData actorData in moverToLastKnownPosData.Keys)
				{
					actorData.GetFogOfWar().ImmediateUpdateVisibilityOfSquares();
				}
			}
			foreach (LastKnownPosData lastKnownPosData in moverToLastKnownPosData.Values)
			{
				if (lastKnownPosData.WasChase == consideringChasers)
				{
					lastKnownPosData.UpdateLastKnownPath(stabilizedMovement.m_movementStage);
				}
				else if (!lastKnownPosData.Actor.IsDead()
				         && lastKnownPosData.Actor.ServerLastKnownPosSquare != lastKnownPosData.Actor.GetCurrentBoardSquare()
				         && (lastKnownPosData.Actor.GetActorStatus().HasStatus(StatusType.Revealed)
				             || CaptureTheFlag.IsActorRevealedByFlag_Server(lastKnownPosData.Actor)
				             || !lastKnownPosData.m_movementInstance.m_willBeStealthed)
				         && lastKnownPosData.Actor.IsActorVisibleToAnyEnemy())
				{
					lastKnownPosData.Actor.SetServerLastKnownPosSquare(lastKnownPosData.Actor.CurrentBoardSquare, "IterateOverLastKnownPosData (movers)");
					if (!actorsThatWillBeSeenButArentMoving.Contains(lastKnownPosData.Actor))
					{
						actorsThatWillBeSeenButArentMoving.Add(lastKnownPosData.Actor);
					}
				}
			}
			foreach (ActorData actorData in GameFlowData.Get().GetActors())
			{
				if (!moverToLastKnownPosData.ContainsKey(actorData)
				    && !actorData.IsDead()
				    && actorData.ServerLastKnownPosSquare != actorData.GetCurrentBoardSquare()
				    && actorData.IsActorVisibleToAnyEnemy())
				{
					actorData.SetServerLastKnownPosSquare(actorData.CurrentBoardSquare, "IterateOverLastKnownPosData (non-movers)");
					if (!actorsThatWillBeSeenButArentMoving.Contains(actorData))
					{
						actorsThatWillBeSeenButArentMoving.Add(actorData);
					}
				}
			}
		}
		foreach (KeyValuePair<ActorData, LastKnownPosData> keyValuePair in moverToLastKnownPosData)
		{
			ActorData key = keyValuePair.Key;
			LastKnownPosData value = keyValuePair.Value;
			if (value.WasChase == consideringChasers)
			{
				BoardSquarePathInfo boardSquarePathInfo = value.m_movementInstance.m_path;
				BoardSquare boardSquare = null;
				while (boardSquarePathInfo != null)
				{
					if (boardSquarePathInfo.m_updateLastKnownPos)
					{
						boardSquare = boardSquarePathInfo.square;
					}
					boardSquarePathInfo = boardSquarePathInfo.next;
				}
				if (boardSquare != null)
				{
					key.SetServerLastKnownPosSquare(boardSquare, "IterateOverLastKnownPosData (path)");
				}
			}
		}
	}

	private static float GetDistanceToConsider(MovementCollection stabilizedMovement, bool forChase)
	{
		float res = 0f;
		foreach (MovementInstance movementInstance in stabilizedMovement.m_movementInstances)
		{
			if (movementInstance.m_wasChase == forChase)
			{
				float cost = movementInstance.m_path.FindMoveCostToEnd();
				if (cost > res)
				{
					res = cost;
				}
			}
		}
		return res;
	}

	private static float GetDistanceToConsider(MovementCollection stabilizedMovement)
	{
		float res = 0f;
		foreach (MovementInstance movementInstance in stabilizedMovement.m_movementInstances)
		{
			float cost = movementInstance.m_path.FindMoveCostToEnd();
			if (cost > res)
			{
				res = cost;
			}
		}
		return res;
	}

	private static List<BoardSquare> FindAllSquaresClaimedByStationaryActors(bool includeAfterImages = true)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		List<ActorData> stationaryActors = ServerActionBuffer.Get().GetStationaryActors();
		foreach (ActorData actorData in stationaryActors)
		{
			if (actorData == null || actorData.IsDead())
			{
				continue;
			}
			if (GameplayUtils.IsPlayerControlled(actorData) && actorData.CurrentBoardSquare != null)
			{
				list.Add(actorData.CurrentBoardSquare);
			}
			if (includeAfterImages)
			{
				TricksterAfterImageNetworkBehaviour component = actorData.GetComponent<TricksterAfterImageNetworkBehaviour>();
				if (component != null)
				{
					List<ActorData> validAfterImages = component.GetValidAfterImages();
					foreach (ActorData afterImageActorData in validAfterImages)
					{
						list.Add(afterImageActorData.CurrentBoardSquare);
					}
				}
			}
		}
		return list;
	}

	private static List<BoardSquare> FindAllSquaresClaimedByMovers(
		bool includeNormalMovers,
		bool includeChasers,
		bool includeCurrentSquareOfMovingTricksters = true)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		List<MovementRequest> allStoredMovementRequests = ServerActionBuffer.Get().GetAllStoredMovementRequests();
		foreach (MovementRequest movementRequest in allStoredMovementRequests)
		{
			if ((movementRequest.WasEverChasing() && includeChasers)
			    || (!movementRequest.WasEverChasing() && includeNormalMovers))
			{
				list.Add(movementRequest.m_targetSquare);
				if (includeCurrentSquareOfMovingTricksters && movementRequest.m_actor.GetComponent<TricksterAfterImageNetworkBehaviour>() != null)
				{
					list.Add(movementRequest.m_actor.CurrentBoardSquare);
				}
			}
		}
		return list;
	}

	private static List<BoardSquare> FindAllCurrentSquaresOfMovers(
		bool includeNormalMovers,
		bool includeChasers,
		bool includeCurrentSquaresOfTricksterAfterimages = true)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		List<MovementRequest> allStoredMovementRequests = ServerActionBuffer.Get().GetAllStoredMovementRequests();
		foreach (MovementRequest movementRequest in allStoredMovementRequests)
		{
			if ((movementRequest.WasEverChasing() && includeChasers)
			    || (!movementRequest.WasEverChasing() && includeNormalMovers))
			{
				list.Add(movementRequest.m_actor.CurrentBoardSquare);
				if (includeCurrentSquaresOfTricksterAfterimages)
				{
					TricksterAfterImageNetworkBehaviour component = movementRequest.m_actor.GetComponent<TricksterAfterImageNetworkBehaviour>();
					if (component != null)
					{
						List<ActorData> validAfterImages = component.GetValidAfterImages();
						foreach (ActorData afterImageActorData in validAfterImages)
						{
							list.Add(afterImageActorData.CurrentBoardSquare);
						}
					}
				}
			}
		}
		return list;
	}

	public static List<BoardSquare> FindAllClaimedSquaresForNormalMovement()
	{
		List<BoardSquare> list = new List<BoardSquare>();
		list.AddRange(FindAllSquaresClaimedByStationaryActors(true));
		list.AddRange(FindAllSquaresClaimedByMovers(true, false, true));
		list.AddRange(FindAllCurrentSquaresOfMovers(false, true, true));
		return list;
	}

	public static List<BoardSquare> FindAllClaimedSquaresForChaseMovement()
	{
		List<BoardSquare> list = new List<BoardSquare>();
		list.AddRange(FindAllSquaresClaimedByStationaryActors(true));
		list.AddRange(FindAllSquaresClaimedByMovers(true, true, true));
		return list;
	}

	private static List<ClashData> FindAllClashesThatExistBecauseOf(ActorData clasher, BoardSquarePathInfo clasherPathNode, bool isChaseClash)
	{
		List<ClashData> list = new List<ClashData>();
		bool flag = clasherPathNode.next != null && clasherPathNode.next.m_moverBumpedFromClash;
		bool flag2 = false;
		List<MovementRequest> allStoredMovementRequests = ServerActionBuffer.Get().GetAllStoredMovementRequests();
		int num = 0;
		while (num < allStoredMovementRequests.Count && !flag2)
		{
			MovementRequest movementRequest = allStoredMovementRequests[num];
			if (!(movementRequest.m_actor == clasher) && movementRequest.WasEverChasing() == isChaseClash)
			{
				BoardSquarePathInfo boardSquarePathInfo = movementRequest.m_path;
				while (boardSquarePathInfo != null && !flag2)
				{
					if (boardSquarePathInfo.m_moverClashesHere && boardSquarePathInfo.square == clasherPathNode.square && ServerClashUtils.AreMoveCostsEqual(boardSquarePathInfo.moveCost, clasherPathNode.moveCost, false))
					{
						bool flag3 = boardSquarePathInfo.next != null && boardSquarePathInfo.next.m_moverBumpedFromClash;
						if (flag == flag3)
						{
							if (movementRequest.m_actor.GetTeam() == clasher.GetTeam())
							{
								flag2 = true;
								list.Clear();
								break;
							}
							list.Add(new ClashData(movementRequest, boardSquarePathInfo));
						}
					}
					boardSquarePathInfo = boardSquarePathInfo.next;
				}
			}
			num++;
		}
		return list;
	}

	public static void GatherGameplayResultsForNormalMovement(List<MovementRequest> stabilizedMovementRequests, bool forChaseMovement)
	{
		Dictionary<ActorData, MovementGameplayData> dictionary = new Dictionary<ActorData, MovementGameplayData>();
		List<MovementRequest> list = new List<MovementRequest>();
		for (int i = 0; i < stabilizedMovementRequests.Count; i++)
		{
			MovementRequest movementRequest = stabilizedMovementRequests[i];
			if (forChaseMovement && movementRequest.WasEverChasing())
			{
				MovementGameplayData value = new MovementGameplayData(movementRequest);
				dictionary.Add(movementRequest.m_actor, value);
				list.Add(movementRequest);
			}
			else if (!forChaseMovement && !movementRequest.WasEverChasing())
			{
				MovementGameplayData value2 = new MovementGameplayData(movementRequest);
				dictionary.Add(movementRequest.m_actor, value2);
				list.Add(movementRequest);
			}
		}
		MovementCollection stabilizedMovement = new MovementCollection(list);
		IterateOverMovementGameplayData(ref dictionary, stabilizedMovement);
	}

	public static void IterateOverMovementGameplayData(ref Dictionary<ActorData, MovementGameplayData> moverToGameplayData, MovementCollection stabilizedMovement)
	{
		int num = Mathf.RoundToInt(GetDistanceToConsider(stabilizedMovement) * 2f);
		for (int i = 0; i < num; i++)
		{
			float num2 = 1f + i * 0.5f - 0.01f;
			foreach (MovementGameplayData movementGameplayData in moverToGameplayData.Values)
			{
				if (!movementGameplayData.WillHaveDiedByNow(num2))
				{
					movementGameplayData.UpdateCurrentlyConsideredPathFromDistance(num2);
					if (movementGameplayData.IsMovingForDistanceCheckValue(num2))
					{
						movementGameplayData.GatherMovementResultsForCurrentlyConsideredPath(stabilizedMovement.m_movementStage);
					}
				}
			}
		}
	}

	public static void IntegrateHpDeltas(Dictionary<ActorData, int> source, ref Dictionary<ActorData, int> dest)
	{
		if (source != null && dest != null)
		{
			foreach (KeyValuePair<ActorData, int> keyValuePair in source)
			{
				ActorData key = keyValuePair.Key;
				int value = keyValuePair.Value;
				if (dest.ContainsKey(key))
				{
					Dictionary<ActorData, int> dictionary = dest;
					ActorData key2 = key;
					dictionary[key2] += value;
				}
				else
				{
					dest.Add(key, value);
				}
			}
		}
	}

	public static void CountDamageAndHeal(Dictionary<ActorData, int> source, ActorData target, ref int damage, ref int healing)
	{
		if (source != null && target != null && source.ContainsKey(target))
		{
			int num = source[target];
			if (num >= 0)
			{
				healing += num;
				return;
			}
			damage -= num;
		}
	}

	public static void CalcDamageDodgedAndIntercepted(Dictionary<ActorData, int> trueDamageTaken, Dictionary<ActorData, int> fakeDamageTaken, ref Dictionary<ActorData, DamageDodgedStats> stats)
	{
		if (trueDamageTaken == null || fakeDamageTaken == null || stats == null)
		{
			return;
		}
		if (stats.Count == 0)
		{
			return;
		}
		foreach (ActorData key in stats.Keys)
		{
			int num;
			if (trueDamageTaken.ContainsKey(key))
			{
				num = trueDamageTaken[key];
			}
			else
			{
				num = 0;
			}
			int num2;
			if (fakeDamageTaken.ContainsKey(key))
			{
				num2 = fakeDamageTaken[key];
			}
			else
			{
				num2 = 0;
			}
			if (num < num2)
			{
				stats[key].m_damageDodged += num2 - num;
			}
			else if (num > num2)
			{
				stats[key].m_damageIntercepted += num - num2;
			}
		}
	}

	public static Dictionary<ActorData, DamageDodgedStats> CalcDamageTakenDueToEvadesStats(AbilityPriority phase, List<AbilityRequest> storedAbilityRequests)
	{
		Dictionary<ActorData, DamageDodgedStats> dictionary = new Dictionary<ActorData, DamageDodgedStats>();
		List<ActorData> list = new List<ActorData>();
		foreach (AbilityRequest abilityRequest in storedAbilityRequests)
		{
			if (abilityRequest.m_ability.GetRunPriority() == AbilityPriority.Evasion && !list.Contains(abilityRequest.m_caster))
			{
				list.Add(abilityRequest.m_caster);
				if (!dictionary.ContainsKey(abilityRequest.m_caster))
				{
					dictionary.Add(abilityRequest.m_caster, new DamageDodgedStats());
				}
			}
		}
		Dictionary<ActorData, int> dictionary2 = new Dictionary<ActorData, int>();
		Dictionary<ActorData, int> dictionary3 = new Dictionary<ActorData, int>();
		foreach (AbilityRequest abilityRequest2 in storedAbilityRequests)
		{
			if (abilityRequest2.m_ability.GetRunPriority() == phase && abilityRequest2.m_additionalData.m_abilityResults.GatheredResults && abilityRequest2.m_additionalData.m_abilityResults_fake.GatheredResults)
			{
				Dictionary<ActorData, int> damageResults_Gross = abilityRequest2.m_additionalData.m_abilityResults.DamageResults_Gross;
				Dictionary<ActorData, int> damageResults_Gross2 = abilityRequest2.m_additionalData.m_abilityResults_fake.DamageResults_Gross;
				CalcDamageDodgedAndIntercepted(damageResults_Gross, damageResults_Gross2, ref dictionary);
				IntegrateHpDeltas(damageResults_Gross, ref dictionary2);
				IntegrateHpDeltas(damageResults_Gross2, ref dictionary3);
			}
		}
		ServerEffectManager.Get().GatherGrossDamageResults_Effects(phase, ref dictionary2, ref dictionary3, ref dictionary);
		if (phase == AbilityPriority.Evasion)
		{
			ServerEffectManager.Get().GatherGrossDamageResults_Effects_Evasion(ref dictionary2, ref dictionary);
			BarrierManager.Get().GatherGrossDamageResults_Barriers_Evasion(ref dictionary2, ref dictionary);
			PowerUpManager.Get().GatherGrossDamageResults_PowerUps_Evasion(ref dictionary2, ref dictionary);
			// TODO CTF CTC
			//if (CaptureTheFlag.Get() != null)
			//{
			//	CaptureTheFlag.Get().GatherGrossDamageResults_Ctf_Evasion(ref dictionary2, ref dictionary);
			//}
			//if (CollectTheCoins.Get() != null)
			//{
			//	CollectTheCoins.Get().GatherGrossDamageResults_Ctc_Evasion(ref dictionary2, ref dictionary);
			//}
		}
		return dictionary;
	}

	public static void AdjustStatsForDamageTakenWithEvades(AbilityPriority phase, List<AbilityRequest> storedAbilityRequests)
	{
		foreach (KeyValuePair<ActorData, DamageDodgedStats> keyValuePair in CalcDamageTakenDueToEvadesStats(phase, storedAbilityRequests))
		{
			ActorData key = keyValuePair.Key;
			DamageDodgedStats value = keyValuePair.Value;
			ActorBehavior actorBehavior = key.GetActorBehavior();
			AbilityData abilityData = key.GetAbilityData();
			if (actorBehavior != null)
			{
				actorBehavior.OnCalculatedDamageTakenWithEvadesStats(value);
			}
			if (abilityData != null)
			{
				abilityData.OnCalculatedDamageTakenWithEvadesStats(value);
			}
		}
	}

	public class LastKnownPosData
	{
		public MovementInstance m_movementInstance;
		public BoardSquarePathInfo m_serverLastKnownPath;
		public BoardSquarePathInfo m_currentlyConsideredPath;
		public BoardSquare m_actorOriginalBoardSquare;

		public LastKnownPosData(MovementInstance movementInstance)
		{
			m_movementInstance = movementInstance;
			m_serverLastKnownPath = null;
			m_currentlyConsideredPath = movementInstance.m_path;
			m_actorOriginalBoardSquare = Actor.GetCurrentBoardSquare();
		}

		public bool WasChase
		{
			get
			{
				return m_movementInstance.m_wasChase;
			}
			private set
			{
			}
		}

		public ActorData Actor
		{
			get
			{
				return m_movementInstance.m_mover;
			}
			private set
			{
			}
		}

		public void UpdateCurrentlyConsideredPathFromDistance(float distance)
		{
			BoardSquarePathInfo boardSquarePathInfo = m_currentlyConsideredPath;
			while (boardSquarePathInfo != null && boardSquarePathInfo.next != null && boardSquarePathInfo.moveCost < distance)
			{
				boardSquarePathInfo = boardSquarePathInfo.next;
			}
			if (m_currentlyConsideredPath != boardSquarePathInfo)
			{
				m_currentlyConsideredPath = boardSquarePathInfo;
			}
		}

		public void UpdateLastKnownPath(MovementStage movementStage)
		{
			if (m_movementInstance.m_groundBased || m_currentlyConsideredPath.next == null || m_currentlyConsideredPath.prev == null)
			{
				bool flag = Actor.GetActorStatus().HasStatus(StatusType.Revealed, true) || CaptureTheFlag.IsActorRevealedByFlag_Server(Actor) || !m_movementInstance.m_willBeStealthed;
				bool flag2 = movementStage == MovementStage.Knockback;
				bool flag3 = flag && Actor.IsActorVisibleToAnyEnemy();
				bool flag4 = m_currentlyConsideredPath.prev != null && m_currentlyConsideredPath.prev.m_updateLastKnownPos;
				bool flag5 = m_movementInstance.m_groundBased && flag4 && !m_currentlyConsideredPath.square.IsValidForGameplay();
				if (flag2 || flag3 || flag5)
				{
					m_serverLastKnownPath = m_currentlyConsideredPath;
					m_serverLastKnownPath.m_visibleToEnemies = true;
					m_serverLastKnownPath.m_updateLastKnownPos = true;
					if (m_movementInstance.m_groundBased && m_serverLastKnownPath.next != null)
					{
						m_serverLastKnownPath.next.m_updateLastKnownPos = true;
					}
				}
			}
		}
	}

	private class ClashData
	{
		public MovementRequest m_request;
		public BoardSquarePathInfo m_clashNode;

		public ClashData(MovementRequest request, BoardSquarePathInfo clashNode)
		{
			m_request = request;
			m_clashNode = clashNode;
		}
	}

	public class MovementGameplayData
	{
		public MovementRequest m_moveRequest;
		public MovementInstance m_movementInstance;
		public BoardSquarePathInfo m_currentlyConsideredPath;
		public float m_currentlyConsideredDistance;
		public BoardSquare m_actorOriginalBoardSquare;
		public int m_healthDeltaForDistance;
		public int m_damageTakenForDistance;
		public int m_damageTakenThisSegment;

		public MovementGameplayData(MovementRequest moveRequest)
		{
			m_moveRequest = moveRequest;
			m_movementInstance = new MovementInstance(moveRequest.m_actor, moveRequest.m_path, true, moveRequest.WasEverChasing(), false);
			m_currentlyConsideredPath = null;
			m_currentlyConsideredDistance = -1f;
			m_healthDeltaForDistance = 0;
			m_damageTakenForDistance = 0;
			m_damageTakenThisSegment = 0;
			m_actorOriginalBoardSquare = Actor.GetCurrentBoardSquare();
		}

		public bool WasChase
		{
			get
			{
				return m_movementInstance.m_wasChase;
			}
			private set
			{
			}
		}

		public ActorData Actor
		{
			get
			{
				return m_movementInstance.m_mover;
			}
			private set
			{
			}
		}

		public void UpdateCurrentlyConsideredPathFromDistance(float distance)
		{
			if (distance == m_currentlyConsideredDistance)
			{
				return;
			}
			m_currentlyConsideredDistance = distance;
			BoardSquarePathInfo next = m_movementInstance.m_path.next;
			BoardSquarePathInfo boardSquarePathInfo = null;
			while (next != null)
			{
				if (next.prev.moveCost < distance && next.moveCost >= distance && next.moveCost < distance + 0.5f)
				{
					boardSquarePathInfo = next;
					break;
				}
				next = next.next;
			}
			if (m_currentlyConsideredPath != boardSquarePathInfo)
			{
				m_currentlyConsideredPath = next;
				m_damageTakenThisSegment = 0;
			}
		}

		public void GatherMovementResultsForCurrentlyConsideredPath(MovementStage movementStage)
		{
			List<MovementResults> movementResultsList = new List<MovementResults>();
			List<MovementResults> movementResultsList2 = new List<MovementResults>();
			List<MovementResults> movementResultsList3 = new List<MovementResults>();
			List<MovementResults> movementResultsList4 = new List<MovementResults>();
			BarrierManager.Get().GatherAllBarrierResultsInResponseToMovementSegment(this, movementStage, ref movementResultsList);
			ProcessMovementResults(movementResultsList);
			if (WillHaveDiedByNow(m_currentlyConsideredDistance))
			{
				OnWillDie();
			}
			else
			{
				ServerEffectManager.Get().GatherEffectResultsInResponseToMovementSegment(this, movementStage, ref movementResultsList2);
				ProcessMovementResults(movementResultsList2);
				PowerUpManager.Get().GatherAllPowerupResultsInResponseToMovementSegment(this, movementStage, ref movementResultsList3);
				ProcessMovementResults(movementResultsList3);
				if (WillHaveDiedByNow(m_currentlyConsideredDistance))
				{
					OnWillDie();
				}
			}
			// TODO CTF CTC
			//if (CaptureTheFlag.Get() != null)
			//{
			//	CaptureTheFlag.Get().GatherCtfResultsInResponseToMovementSegment(this, movementStage, ref movementResultsList4);
			//	ProcessMovementResults(movementResultsList4);
			//}
			//if (CollectTheCoins.Get() != null)
			//{
			//	CollectTheCoins.Get().GatherCtcResultsInResponseToMovementSegment(this, movementStage, ref movementResultsList4);
			//	ProcessMovementResults(movementResultsList4);
			//}
		}

		public bool WillHaveDiedByNow(float distanceGoal)
		{
			return Actor.HitPoints + Actor.AbsorbPoints + m_healthDeltaForDistance <= 0;
		}

		public bool IsMovingForDistanceCheckValue(float distance)
		{
			bool result;
			if (m_currentlyConsideredDistance == distance)
			{
				result = (m_currentlyConsideredPath != null);
			}
			else
			{
				UpdateCurrentlyConsideredPathFromDistance(distance);
				result = (m_currentlyConsideredPath != null);
			}
			return result;
		}

		public void ProcessMovementResults(List<MovementResults> movementResultsList)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < movementResultsList.Count; i++)
			{
				MovementResults movementResults = movementResultsList[i];
				Dictionary<ActorData, int> movementDamageResults = movementResults.GetMovementDamageResults();
				if (movementDamageResults != null && movementDamageResults.ContainsKey(Actor))
				{
					num += movementDamageResults[Actor];
					if (movementDamageResults[Actor] < 0)
					{
						num2 += Mathf.Abs(movementDamageResults[Actor]);
					}
				}
				AdjustPathsFromMovementResults(movementResults);
			}
			m_healthDeltaForDistance += num;
			m_damageTakenForDistance += num2;
			m_damageTakenThisSegment += num2;
		}

		private void AdjustPathsFromMovementResults(MovementResults movementResults)
		{
			BoardSquarePathInfo boardSquarePathInfo = null;
			List<BoardSquare> list;
			if (WasChase)
			{
				list = FindAllClaimedSquaresForChaseMovement();
			}
			else
			{
				list = FindAllClaimedSquaresForNormalMovement();
			}
			list.Remove(m_moveRequest.m_targetSquare);
			List<BoardSquarePathInfo> list2 = new List<BoardSquarePathInfo>();
			if (movementResults.AppliesStatusToMover(StatusType.CantSprint_Absolute))
			{
				float num = Actor.GetActorMovement().CalculateMaxHorizontalMovement(true, false);
				for (BoardSquarePathInfo boardSquarePathInfo2 = m_currentlyConsideredPath; boardSquarePathInfo2 != null; boardSquarePathInfo2 = boardSquarePathInfo2.next)
				{
					bool flag = boardSquarePathInfo == null;
					bool flag2 = boardSquarePathInfo2.moveCost >= num;
					bool moverClashesHere = boardSquarePathInfo2.m_moverClashesHere;
					bool flag3 = boardSquarePathInfo2.m_moverBumpedFromClash || (boardSquarePathInfo2.next != null && boardSquarePathInfo2.next.m_moverBumpedFromClash);
					bool flag4 = list.Contains(boardSquarePathInfo2.square);
					if (flag && flag2 && !moverClashesHere && !flag3 && !flag4)
					{
						boardSquarePathInfo = boardSquarePathInfo2;
					}
					bool flag5 = boardSquarePathInfo != null;
					bool moverClashesHere2 = boardSquarePathInfo2.m_moverClashesHere;
					bool flag6 = boardSquarePathInfo2 == boardSquarePathInfo;
					if (flag5 && !flag6 && moverClashesHere2 && !list2.Contains(boardSquarePathInfo2))
					{
						list2.Add(boardSquarePathInfo2);
					}
				}
			}
			if (boardSquarePathInfo != null)
			{
				list.Add(boardSquarePathInfo.square);
				for (int i = 0; i < list2.Count; i++)
				{
					BoardSquarePathInfo boardSquarePathInfo3 = list2[i];
					List<ClashData> list3 = FindAllClashesThatExistBecauseOf(Actor, boardSquarePathInfo3, WasChase);
					bool flag7 = boardSquarePathInfo3.next != null && boardSquarePathInfo3.next.m_moverBumpedFromClash;
					for (int j = 0; j < list3.Count; j++)
					{
						ClashData clashData = list3[j];
						if (flag7)
						{
							if (!list.Contains(clashData.m_clashNode.square))
							{
								list.Remove(clashData.m_request.m_targetSquare);
								clashData.m_clashNode.m_moverClashesHere = false;
								clashData.m_clashNode.next = null;
								clashData.m_request.m_path.CalcAndSetMoveCostToEnd();
								clashData.m_request.m_targetSquare = clashData.m_clashNode.square;
								list.Add(clashData.m_request.m_targetSquare);
							}
							else
							{
								BoardSquarePathInfo prev = clashData.m_clashNode.prev;
								while (prev != null && !prev.m_moverHasGameplayHitHere)
								{
									if (prev.m_moverClashesHere)
									{
										break;
									}
									if (!list.Contains(prev.square))
									{
										list.Remove(clashData.m_request.m_targetSquare);
										clashData.m_clashNode.m_moverClashesHere = false;
										prev.next = null;
										clashData.m_request.m_path.CalcAndSetMoveCostToEnd();
										clashData.m_request.m_targetSquare = prev.square;
										list.Add(clashData.m_request.m_targetSquare);
										break;
									}
									prev = prev.prev;
								}
							}
						}
						else
						{
							clashData.m_clashNode.m_moverClashesHere = false;
						}
					}
				}
				boardSquarePathInfo.next = null;
				m_moveRequest.m_path.CalcAndSetMoveCostToEnd();
				m_moveRequest.m_targetSquare = boardSquarePathInfo.square;
			}
		}

		private void OnWillDie()
		{
			m_currentlyConsideredPath.m_moverDiesHere = true;
			while (m_currentlyConsideredPath.next != null)
			{
				m_currentlyConsideredPath.BackUpOnceFromEnd();
			}
			m_movementInstance.m_path.CalcAndSetMoveCostToEnd();
			m_moveRequest.m_targetSquare = m_currentlyConsideredPath.square;
		}
	}

	public class DamageDodgedStats
	{
		public int m_damageDodged;
		public int m_damageIntercepted;
	}

	public class DamageStatAdjustments
	{
		private ActorData m_caster;
		private ActorData m_target;

		private List<Effect> m_effectsGivingEmpoweredToCaster;
		private List<ActorData> m_actorsGivingEmpoweredToCaster;
		private List<Effect> m_effectsGivingWeakenedToCaster;
		private List<ActorData> m_actorsGivingWeakenedToCaster;
		private List<Effect> m_effectsGivingVulnerableToTarget;
		private List<ActorData> m_actorsGivingVulnerableToTarget;
		private List<Effect> m_effectsGivingArmoredToTarget;
		private List<ActorData> m_actorsGivingArmoredToTarget;

		public int m_damageAddedFromEmpowered;
		public int m_damageMitigatedFromWeakened;
		public int m_damageAddedFromVulnerable;
		public int m_damageMitigatedFromArmored;
		public int m_damageMitigatedFromCover;

		public DamageStatAdjustments(
			ActorData caster,
			ActorData target,
			int damage_actual,
			int damage_outgoingNormal,
			int damage_outgoingEmpowered,
			int damage_outgoingWeakened,
			int damage_incomingNormal,
			int damage_incomingVulnerable,
			int damage_incomingArmored,
			int damageBeforeCover)
		{
			m_caster = caster;
			m_target = target;
			bool isEmpowered = false;
			bool isWeakened = false;
			bool isVulnerable = false;
			bool isArmored = false;
			if (caster != null && caster.GetActorStatus() != null)
			{
				ActorStatus actorStatus = caster.GetActorStatus();
				isEmpowered = actorStatus.HasStatus(StatusType.Empowered);
				isWeakened = actorStatus.HasStatus(StatusType.Weakened);
			}
			if (target != null && target.GetActorStatus() != null)
			{
				ActorStatus actorStatus2 = target.GetActorStatus();
				isVulnerable = actorStatus2.HasStatus(StatusType.Vulnerable);
				isArmored = actorStatus2.HasStatus(StatusType.Armored);
			}
			if (isEmpowered)
			{
				if (isWeakened)
				{
					m_damageAddedFromEmpowered = damage_actual - damage_outgoingWeakened;
				}
				else
				{
					m_damageAddedFromEmpowered = damage_actual - damage_outgoingNormal;
				}
			}
			else
			{
				m_damageAddedFromEmpowered = 0;
			}
			if (isWeakened)
			{
				if (isEmpowered)
				{
					m_damageMitigatedFromWeakened = damage_outgoingEmpowered - damage_actual;
				}
				else
				{
					m_damageMitigatedFromWeakened = damage_outgoingNormal - damage_actual;
				}
			}
			else
			{
				m_damageMitigatedFromWeakened = 0;
			}
			if (isVulnerable)
			{
				if (isArmored)
				{
					m_damageAddedFromVulnerable = damage_actual - damage_incomingArmored;
				}
				else
				{
					m_damageAddedFromVulnerable = damage_actual - damage_incomingNormal;
				}
			}
			else
			{
				m_damageAddedFromVulnerable = 0;
			}
			if (isArmored)
			{
				if (isVulnerable)
				{
					m_damageMitigatedFromArmored = damage_incomingVulnerable - damage_actual;
				}
				else
				{
					m_damageMitigatedFromArmored = damage_incomingNormal - damage_actual;
				}
			}
			else
			{
				m_damageMitigatedFromArmored = 0;
			}
			if (damageBeforeCover != damage_actual)
			{
				m_damageMitigatedFromCover = damageBeforeCover - damage_actual;
			}
			else
			{
				m_damageMitigatedFromCover = 0;
			}
			if (isEmpowered)
			{
				m_actorsGivingEmpoweredToCaster = ServerEffectManager.Get().GetCastersOfEffectsOnTargetGrantingStatus(caster, StatusType.Empowered);
				m_effectsGivingEmpoweredToCaster = ServerEffectManager.Get().GetEffectsOnTargetGrantingStatus(caster, StatusType.Empowered);
			}
			else
			{
				m_actorsGivingEmpoweredToCaster = null;
				m_effectsGivingEmpoweredToCaster = null;
			}
			if (isWeakened)
			{
				m_actorsGivingWeakenedToCaster = ServerEffectManager.Get().GetCastersOfEffectsOnTargetGrantingStatus(caster, StatusType.Weakened);
				m_effectsGivingWeakenedToCaster = ServerEffectManager.Get().GetEffectsOnTargetGrantingStatus(caster, StatusType.Weakened);
			}
			else
			{
				m_actorsGivingWeakenedToCaster = null;
				m_effectsGivingWeakenedToCaster = null;
			}
			if (isVulnerable)
			{
				m_actorsGivingVulnerableToTarget = ServerEffectManager.Get().GetCastersOfEffectsOnTargetGrantingStatus(target, StatusType.Vulnerable);
				m_effectsGivingVulnerableToTarget = ServerEffectManager.Get().GetEffectsOnTargetGrantingStatus(target, StatusType.Vulnerable);
			}
			else
			{
				m_actorsGivingVulnerableToTarget = null;
				m_effectsGivingVulnerableToTarget = null;
			}
			if (isArmored)
			{
				m_actorsGivingArmoredToTarget = ServerEffectManager.Get().GetCastersOfEffectsOnTargetGrantingStatus(target, StatusType.Armored);
				m_effectsGivingArmoredToTarget = ServerEffectManager.Get().GetEffectsOnTargetGrantingStatus(target, StatusType.Armored);
				return;
			}
			m_actorsGivingArmoredToTarget = null;
			m_effectsGivingArmoredToTarget = null;
		}

		public void ApplyDamageAdjustmentStats()
		{
			if (m_caster != null && m_caster.GetActorBehavior() != null)
			{
				m_caster.GetActorBehavior().OnCalculatedDamageAdjustmentsOfHitByMe(m_damageAddedFromEmpowered, m_damageMitigatedFromWeakened, m_damageAddedFromVulnerable, m_damageMitigatedFromArmored, m_damageMitigatedFromCover);
			}
			if (m_target != null && m_target.GetActorBehavior() != null)
			{
				m_target.GetActorBehavior().OnCalculatedDamageAdjustmentsOfHitOnMe(m_damageAddedFromEmpowered, m_damageMitigatedFromWeakened, m_damageAddedFromVulnerable, m_damageMitigatedFromArmored, m_damageMitigatedFromCover);
			}
			if (m_actorsGivingEmpoweredToCaster != null)
			{
				foreach (ActorData actorData in m_actorsGivingEmpoweredToCaster)
				{
					if (actorData != m_caster)
					{
						ActorBehavior actorBehavior = actorData.GetActorBehavior();
						if (actorBehavior != null)
						{
							actorBehavior.OnCalculatedExtraDamageFromEmpoweredGrantedByMe(m_damageAddedFromEmpowered);
						}
					}
				}
				foreach (Effect effect in m_effectsGivingEmpoweredToCaster)
				{
					effect.OnCalculatedExtraDamageFromEmpoweredGrantedByThisEffect(m_caster, m_damageAddedFromEmpowered);
				}
			}
			if (m_actorsGivingWeakenedToCaster != null)
			{
				foreach (ActorData actorData2 in m_actorsGivingWeakenedToCaster)
				{
					if (actorData2 != m_caster)
					{
						ActorBehavior actorBehavior2 = actorData2.GetActorBehavior();
						if (actorBehavior2 != null)
						{
							actorBehavior2.OnCalculatedDamageReducedFromWeakenedGrantedByMe(m_damageMitigatedFromWeakened);
						}
					}
				}
				foreach (Effect effect2 in m_effectsGivingWeakenedToCaster)
				{
					effect2.OnCalculatedDamageReducedFromWeakenedGrantedByThisEffect(m_caster, m_damageMitigatedFromWeakened);
				}
			}
			if (m_actorsGivingVulnerableToTarget != null)
			{
				foreach (ActorData actorData3 in m_actorsGivingVulnerableToTarget)
				{
					if (actorData3 != m_caster)
					{
						ActorBehavior actorBehavior3 = actorData3.GetActorBehavior();
						if (actorBehavior3 != null)
						{
							actorBehavior3.OnCalculatedExtraDamageFromVulnerableGrantedByMe(m_damageAddedFromVulnerable);
						}
					}
				}
				foreach (Effect effect3 in m_effectsGivingVulnerableToTarget)
				{
					effect3.OnCalculatedExtraDamageFromVulnerableGrantedByThisEffect(m_target, m_damageAddedFromVulnerable);
				}
			}
			if (m_actorsGivingArmoredToTarget != null)
			{
				foreach (ActorData actorData4 in m_actorsGivingArmoredToTarget)
				{
					if (actorData4 != m_caster)
					{
						ActorBehavior actorBehavior4 = actorData4.GetActorBehavior();
						if (actorBehavior4 != null)
						{
							actorBehavior4.OnCalculatedDamageReducedFromArmoredGrantedByMe(m_damageMitigatedFromArmored);
						}
					}
				}
				foreach (Effect effect4 in m_effectsGivingArmoredToTarget)
				{
					effect4.OnCalculatedDamageReducedFromArmoredGrantedByThisEffect(m_target, m_damageMitigatedFromArmored);
				}
			}
		}
	}

	public class EnergyStatAdjustments
	{
		private ActorData m_target;
		private int m_inputEnergyActual;
		private int m_inputEnergyBase;
		private int m_inputEnergyIfBoosted;
		private int m_inputEnergyIfReduced;
		private bool m_calculatedAdjustments;
		private int m_extraEnergyFromEnergized;
		private List<ActorData> m_energizedSourceActors;

		public EnergyStatAdjustments(ActorData caster, ActorData target)
		{
			m_target = target;
		}

		public void IncrementTotals(int actual, int baseAmount, int ifBoosted, int ifReduced)
		{
			m_inputEnergyActual += actual;
			m_inputEnergyBase += baseAmount;
			m_inputEnergyIfBoosted += ifBoosted;
			m_inputEnergyIfReduced += ifReduced;
		}

		public void CalculateAdjustments()
		{
			m_calculatedAdjustments = true;
			bool isEnergized = false;
			bool hasSlowEnergyGain = false;
			if (m_target != null && m_target.GetActorStatus() != null)
			{
				isEnergized = m_target.GetActorStatus().HasStatus(StatusType.Energized, true);
				hasSlowEnergyGain = m_target.GetActorStatus().HasStatus(StatusType.SlowEnergyGain, true);
			}
			if (isEnergized)
			{
				if (hasSlowEnergyGain)
				{
					m_extraEnergyFromEnergized = m_inputEnergyActual - m_inputEnergyIfReduced;
				}
				else
				{
					m_extraEnergyFromEnergized = m_inputEnergyActual - m_inputEnergyBase;
				}
			}
			if (isEnergized)
			{
				m_energizedSourceActors = ServerEffectManager.Get().GetCastersOfEffectsOnTargetGrantingStatus(m_target, StatusType.Energized);
			}
		}

		public void ApplyStatAdjustments()
		{
			if (!m_calculatedAdjustments)
			{
				Debug.LogError("Trying to apply stat adjustments before calculation of final values");
				return;
			}
			if (m_energizedSourceActors != null && m_extraEnergyFromEnergized > 0)
			{
				foreach (ActorData actorData in m_energizedSourceActors)
				{
					if (!(actorData == m_target))
					{
						ActorBehavior actorBehavior = actorData.GetActorBehavior();
						if (actorBehavior != null)
						{
							actorBehavior.OnCalculatedExtraEnergyByMe(m_extraEnergyFromEnergized);
						}
					}
				}
			}
		}
	}
}
#endif
