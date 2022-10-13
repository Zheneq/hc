// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only, missing in reactor
#if SERVER
public class ServerEvadeManager
{
	private List<ServerEvadeUtils.EvadeInfo> m_processedEvades;
	private List<EvaderSwapInfo> m_evaderSwapData;
	private EvaderSwapState m_currentSwapState;

	public ServerEvadeManager()
	{
		m_processedEvades = new List<ServerEvadeUtils.EvadeInfo>();
		m_evaderSwapData = new List<EvaderSwapInfo>();
	}

	private bool DebugShowChargeGizmo => false;

	private void AppendNonPlayerEvades(
		ref List<ServerEvadeUtils.EvadeInfo> evades,
		List<ServerEvadeUtils.NonPlayerEvadeData> attachedEvadeDataList)
	{
		if (attachedEvadeDataList == null)
		{
			return;
		}
		foreach (ServerEvadeUtils.NonPlayerEvadeData nonPlayerEvadeData in attachedEvadeDataList)
		{
			if (nonPlayerEvadeData.ShouldAddToEvades() && nonPlayerEvadeData.IsTeleport())
			{
				evades.Add(new ServerEvadeUtils.NonPlayerTeleportInfo(
					nonPlayerEvadeData.m_mover,
					nonPlayerEvadeData.m_start,
					nonPlayerEvadeData.m_idealDestination,
					nonPlayerEvadeData));
			}
		}
	}

	public void ProcessEvades(List<AbilityRequest> allRequests, AbilityPriority currentPriority)
	{
		List<ServerEvadeUtils.EvadeInfo> allEvades = new List<ServerEvadeUtils.EvadeInfo>();
		foreach (AbilityRequest abilityRequest in allRequests)
		{
			if (abilityRequest.m_ability.RunPriority == currentPriority && abilityRequest.m_ability.IsCharge())
			{
				allEvades.Add(new ServerEvadeUtils.ChargeInfo(abilityRequest));
				List<ServerEvadeUtils.NonPlayerEvadeData> nonPlayerEvades = abilityRequest.m_ability.GetNonPlayerEvades(
					abilityRequest.m_targets, abilityRequest.m_caster, abilityRequest.m_additionalData);
				AppendNonPlayerEvades(ref allEvades, nonPlayerEvades);
			}
			else if (abilityRequest.m_ability.RunPriority == currentPriority && abilityRequest.m_ability.IsTeleport())
			{
				allEvades.Add(new ServerEvadeUtils.TeleportInfo(abilityRequest));
				List<ServerEvadeUtils.NonPlayerEvadeData> nonPlayerEvades = abilityRequest.m_ability.GetNonPlayerEvades(
					abilityRequest.m_targets, abilityRequest.m_caster, abilityRequest.m_additionalData);
				AppendNonPlayerEvades(ref allEvades, nonPlayerEvades);
			}
		}
		if (allEvades.Count == 0)
		{
			return;
		}
		allEvades.Sort(delegate(ServerEvadeUtils.EvadeInfo evade1, ServerEvadeUtils.EvadeInfo evade2)
		{
			float dist1 = evade1.GetEvadePathDistance(evade1.GetIdealDestination());
			float dist2 = evade2.GetEvadePathDistance(evade2.GetIdealDestination());
			return dist1.CompareTo(dist2);
		});
		List<BoardSquare> invalidSquares = new List<BoardSquare>();
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in allEvades)
		{
			if (evadeInfo.GetMover() != null && evadeInfo.GetMover().GetPassiveData() != null)
			{
				evadeInfo.GetMover().GetPassiveData().AddInvalidEvadeDestinations(allEvades, invalidSquares);
			}
		}
		List<BoardSquare> additionalInvalidSquares_evaderSpecific = new List<BoardSquare>();
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in allEvades)
		{
			BoardSquare idealDestination = evadeInfo.GetIdealDestination();
			List<BoardSquare> destinationSquares = new List<BoardSquare>();
			int borderRadius = 0;
			while (destinationSquares.Count == 0 && borderRadius <= 4)
			{
				destinationSquares = GetDestinationSquaresInBorderOf(
					idealDestination, borderRadius, evadeInfo, allEvades, invalidSquares,
					additionalInvalidSquares_evaderSpecific, true);
				borderRadius++;
			}
			borderRadius = 0;
			while (destinationSquares.Count == 0 && borderRadius <= 4)
			{
				destinationSquares = GetDestinationSquaresInBorderOf(
					idealDestination, borderRadius, evadeInfo, allEvades, invalidSquares,
					additionalInvalidSquares_evaderSpecific, false);
				borderRadius++;
			}
			BoardSquare bestDestination = null;
			float bestDotProduct = -1f;
			Vector3 bestSquareTestVector = evadeInfo.GetBestSquareTestVector();
			foreach (BoardSquare square in destinationSquares)
			{
				Vector3 vector = square.ToVector3() - idealDestination.ToVector3();
				vector.y = 0f;
				vector.Normalize();
				float dotProduct = Vector3.Dot(bestSquareTestVector, vector);
				if (bestDestination == null || dotProduct > bestDotProduct)
				{
					bestDestination = square;
					bestDotProduct = dotProduct;
				}
			}
			if (bestDestination == null)
			{
				evadeInfo.MarkAsInvalid();
			}
			else
			{
				evadeInfo.ModifyDestination(bestDestination);
			}
		}
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in allEvades)
		{
			evadeInfo.ProcessEvadeDodge(allEvades);
		}
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in allEvades)
		{
			evadeInfo.StorePath();
			if (evadeInfo.m_evadePath != null)
			{
				evadeInfo.m_evadePath.CheckPathConnectionForSelfReference();
			}
		}
		ProcessClashes(allRequests, currentPriority, ref allEvades);
		m_processedEvades = allEvades;
	}

	public void ProcessClashes(
		List<AbilityRequest> allRequests,
		AbilityPriority currentPriority,
		ref List<ServerEvadeUtils.EvadeInfo> evades)
	{
		ServerClashUtils.MovementClashCollection movementClashCollection = ServerClashUtils.IdentifyClashSegments_Evade(evades);
		List<BoardSquare> invalidSquares = new List<BoardSquare>();
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in evades)
		{
			if (evadeInfo.GetMover() != null && evadeInfo.GetMover().GetPassiveData() != null)
			{
				evadeInfo.GetMover().GetPassiveData().AddInvalidEvadeDestinations(evades, invalidSquares);
			}
		}
		List<BoardSquare> squaresOfInterest = new List<BoardSquare>();
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in evades)
		{
			if (evadeInfo.m_evadeDest != null && !squaresOfInterest.Contains(evadeInfo.m_evadeDest))
			{
				squaresOfInterest.Add(evadeInfo.m_evadeDest);
			}
		}
		bool hasClashes = false;
		List<ServerClashUtils.MovementClash> clashes = new List<ServerClashUtils.MovementClash>();
		foreach (ServerClashUtils.MovementClash movementClash in movementClashCollection.m_clashes)
		{
			if (!movementClash.m_continuing)
			{
				clashes.Add(movementClash);
				hasClashes = true;
				if (movementClash.m_clashSquare != null && !squaresOfInterest.Contains(movementClash.m_clashSquare))
				{
					squaresOfInterest.Add(movementClash.m_clashSquare);
				}
			}
		}
		foreach (ServerClashUtils.MovementClash movementClash in clashes)
		{
			List<ServerClashUtils.MovementClashParticipant> allParticipants = movementClash.GetAllParticipants();
			List<ServerEvadeUtils.EvadeInfo> evadesInClash = new List<ServerEvadeUtils.EvadeInfo>();
			foreach (ServerEvadeUtils.EvadeInfo evadeInfo in evades)
			{
				foreach (ServerClashUtils.MovementClashParticipant participant in allParticipants)
				{
					if (participant.Actor == evadeInfo.GetMover())
					{
						evadesInClash.Add(evadeInfo);
						break;
					}
				}
			}
			foreach (ServerEvadeUtils.EvadeInfo evadeInfo in evadesInClash)
			{
				evadeInfo.ResetDestinationData();
			}
			foreach (ServerEvadeUtils.EvadeInfo evadeInfo in evadesInClash)
			{
				BoardSquare idealDestination = evadeInfo.GetIdealDestination();
				List<BoardSquare> destinationSquares = new List<BoardSquare>();
				int borderRadius = 0;
				while (destinationSquares.Count == 0 && borderRadius <= 4)
				{
					destinationSquares = GetDestinationSquaresInBorderOf(
						idealDestination, borderRadius, evadeInfo, evades, invalidSquares,
						squaresOfInterest, true);
					borderRadius++;
				}
				borderRadius = 0;
				while (destinationSquares.Count == 0 && borderRadius <= 4)
				{
					destinationSquares = GetDestinationSquaresInBorderOf(
						idealDestination, borderRadius, evadeInfo, evades, invalidSquares,
						squaresOfInterest, false);
					borderRadius++;
				}
				BoardSquare bestDestination = null;
				float bestDotProduct = -1f;
				Vector3 bestSquareTestVector = evadeInfo.GetBestSquareTestVector();
				foreach (BoardSquare destination in destinationSquares)
				{
					Vector3 vector = destination.ToVector3() - idealDestination.ToVector3();
					vector.y = 0f;
					vector.Normalize();
					float dotProduct = Vector3.Dot(bestSquareTestVector, vector);
					if (bestDestination == null || dotProduct > bestDotProduct)
					{
						bestDestination = destination;
						bestDotProduct = dotProduct;
					}
				}
				if (bestDestination == null)
				{
					evadeInfo.MarkAsInvalid();
				}
				else
				{
					evadeInfo.ModifyDestination(bestDestination);
					if (!squaresOfInterest.Contains(bestDestination))
					{
						squaresOfInterest.Add(bestDestination);
					}
				}
			}
			foreach (ServerEvadeUtils.EvadeInfo evadeInfo in evadesInClash)
			{
				evadeInfo.ProcessEvadeDodge(evadesInClash);
			}
			foreach (ServerEvadeUtils.EvadeInfo evadeInfo in evadesInClash)
			{
				evadeInfo.StorePath();
				if (evadeInfo.m_evadePath != null)
				{
					evadeInfo.m_evadePath.CheckPathConnectionForSelfReference();
				}
			}
		}
		if (hasClashes)
		{
			ClientClashManager.SendClashesAtEndOfMovementMsgToClients(clashes);
			movementClashCollection = ServerClashUtils.IdentifyClashSegments_Evade(evades);
		}
	}

	public void RunEvades()
	{
		if (m_processedEvades == null)
		{
			Log.Error("Trying to run evades, but they have not been processed.");
			return;
		}
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in m_processedEvades)
		{
			if (evadeInfo.m_request == null && !(evadeInfo is ServerEvadeUtils.NonPlayerTeleportInfo))
			{
				Log.Error("Trying to run evade has null ability request.");
			}
			else if (evadeInfo.IsStillValid())
			{
				evadeInfo.GetMover().UnoccupyCurrentBoardSquare();
			}
		}
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in m_processedEvades)
		{
			if (evadeInfo.m_request == null && !(evadeInfo is ServerEvadeUtils.NonPlayerTeleportInfo))
			{
				Log.Error("Trying to run evade has null ability request.");
			}
			else if (evadeInfo.IsStillValid())
			{
				if (evadeInfo is ServerEvadeUtils.ChargeInfo chargeInfo)
				{
					Ability ability = chargeInfo.m_request.m_ability;
					Vector3 facingDirAfterMovement = ability != null
						? ability.GetFacingDirAfterMovement(evadeInfo)
						: Vector3.zero;
					ChargeToPos(
						chargeInfo.m_request.m_caster,
						chargeInfo.m_evadePath,
						facingDirAfterMovement,
						chargeInfo.m_request.m_ability.GetMovementType());
					if (Application.isEditor && DebugShowChargeGizmo)
					{
						for (BoardSquarePathInfo step = chargeInfo.m_evadePath; step != null; step = step.next)
						{
							if (step.next == null)
							{
								break;
							}
							Debug.DrawLine(step.square.ToVector3(), step.next.square.ToVector3(), Color.red, 10f);
						}
					}
				}
				else if (evadeInfo is ServerEvadeUtils.TeleportInfo teleportInfo)
				{
					ActorData caster = teleportInfo.m_request.m_caster;
					Ability ability = teleportInfo.m_request.m_ability;
					caster.TeleportToBoardSquare(
						teleportInfo.m_evadeDest,
						ability.GetFacingDirAfterMovement(evadeInfo),
						ability.GetEvasionTeleportType(),
						teleportInfo.m_evadePath,
						ability.CalcMovementSpeed(evadeInfo.GetEvadeDistance()), 
						ability.GetMovementType(),
						GameEventManager.EventType.TheatricsEvasionMoveStart);
				}
				else if (evadeInfo is ServerEvadeUtils.NonPlayerTeleportInfo nonPlayerTeleportInfo)
				{
					ActorData mover = nonPlayerTeleportInfo.GetMover();
					if (mover != null)
					{
						mover.TeleportToBoardSquare(
							nonPlayerTeleportInfo.m_evadeDest,
							nonPlayerTeleportInfo.m_attachedEvadeData.m_facingDirection,
							ActorData.TeleportType.Evasion_DontAdjustToVision,
							nonPlayerTeleportInfo.m_evadePath,
							nonPlayerTeleportInfo.m_attachedEvadeData.m_moveSpeed,
							nonPlayerTeleportInfo.m_attachedEvadeData.m_movementType,
							GameEventManager.EventType.TheatricsEvasionMoveStart,
							nonPlayerTeleportInfo.m_teleportStart);
					}
				}
			}
		}
		m_processedEvades.Clear();
	}

	public void GatherGameplayResultsInResponseToEvades(out List<ActorData> actorsThatWillBeSeenButArentMoving)
	{
		MovementCollection movementCollection = new MovementCollection(m_processedEvades);
		ServerEffectManager.Get().GatherAllEffectResultsInResponseToEvades(movementCollection);
		BarrierManager.Get().GatherAllBarrierResultsInResponseToEvades(movementCollection);
		PowerUpManager.Get().GatherAllPowerupResultsInResponseToEvades(movementCollection);
		// TODO CTF CTC
		//if (CaptureTheFlag.Get() != null)
		//{
		//	CaptureTheFlag.Get().GatherResultsInResponseToEvades(movementCollection);
		//}
		//if (CollectTheCoins.Get() != null)
		//{
		//	CollectTheCoins.Get().GatherResultsInResponseToEvades(movementCollection);
		//}
		ServerGameplayUtils.SetServerLastKnownPositionsForMovement(
			movementCollection, 
			out actorsThatWillBeSeenButArentMoving,
			out _);
	}

	public void SwapEvaderSquaresWithDestinations()
	{
		if (m_currentSwapState != EvaderSwapState.Present)
		{
			Log.Error("Swapping evader squares to gather results, but we already swapped, and are in '" + m_currentSwapState + "' mode.");
		}
		m_evaderSwapData.Clear();
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in m_processedEvades)
		{
			m_evaderSwapData.Add(new EvaderSwapInfo(evadeInfo.GetMover(), evadeInfo.GetMover().GetCurrentBoardSquare(), evadeInfo.m_evadeDest));
		}
		SwapActorSquares(m_evaderSwapData);
		m_currentSwapState = EvaderSwapState.Future;
	}

	public void SwapEvaderCurrentSquaresWithPreEvadeSquares(out bool swapsOccured)
	{
		swapsOccured = false;
		if (m_currentSwapState != EvaderSwapState.Present)
		{
			Log.Error("Swapping evader squares to gather results, but we already swapped, and are in '" + m_currentSwapState + "' mode.");
		}
		m_evaderSwapData.Clear();
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData != null
			    && !actorData.IsDead()
			    && actorData.CurrentBoardSquare != null
			    && actorData.CurrentBoardSquare != actorData.SquareAtResolveStart)
			{
				m_evaderSwapData.Add(new EvaderSwapInfo(actorData, actorData.CurrentBoardSquare, actorData.SquareAtResolveStart));
				swapsOccured = true;
			}
		}
		if (swapsOccured)
		{
			SwapActorSquares(m_evaderSwapData);
		}
		m_currentSwapState = EvaderSwapState.AlternateReality;
	}

	private static void SwapActorSquares(List<EvaderSwapInfo> swapData)
	{
		foreach (EvaderSwapInfo evaderSwapInfo in swapData)
		{
			evaderSwapInfo.m_actor.UnoccupyCurrentBoardSquare();
		}
		foreach (EvaderSwapInfo evaderSwapInfo in swapData)
		{
			evaderSwapInfo.m_actor.SwapBoardSquare(evaderSwapInfo.m_postSwapSquare);
		}
		foreach (EvaderSwapInfo evaderSwapInfo in swapData)
		{
			evaderSwapInfo.m_actor.OccupyCurrentBoardSquare();
		}
	}

	public void UndoEvaderDestinationsSwap()
	{
		if (m_currentSwapState == EvaderSwapState.Present)
		{
			Log.Error("Undoing swap of evader squares to gather results, but we already undid that, and are in '" + m_currentSwapState + "' mode.");
		}
		foreach (EvaderSwapInfo evaderSwapInfo in m_evaderSwapData)
		{
			evaderSwapInfo.m_actor.UnoccupyCurrentBoardSquare();
		}
		foreach (EvaderSwapInfo evaderSwapInfo in m_evaderSwapData)
		{
			evaderSwapInfo.m_actor.SwapBoardSquare(evaderSwapInfo.m_preSwapSquare);
		}
		foreach (EvaderSwapInfo evaderSwapInfo in m_evaderSwapData)
		{
			evaderSwapInfo.m_actor.OccupyCurrentBoardSquare();
		}
		m_evaderSwapData.Clear();
		m_currentSwapState = EvaderSwapState.Present;
	}

	public bool HasEvades()
	{
		return m_processedEvades.Count > 0;
	}

	public bool HasProcessedEvadeForActor(ActorData actor)
	{
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in m_processedEvades)
		{
			if (evadeInfo != null && evadeInfo.GetMover() == actor)
			{
				return true;
			}
		}
		return false;
	}

	public int GetNumSquaresInProcessedEvade(ActorData actor)
	{
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in m_processedEvades)
		{
			if (evadeInfo != null && evadeInfo.GetMover() == actor)
			{
				return evadeInfo.GetNumSquaresInPath();
			}
		}
		return 0;
	}

	public List<BoardSquare> GetSquaresInProcessedEvade(ActorData actor)
	{
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in m_processedEvades)
		{
			if (evadeInfo != null && evadeInfo.GetMover() == actor)
			{
				return evadeInfo.GetSquaresInPath();
			}
		}
		return new List<BoardSquare>();
	}

	public BoardSquare GetProcessedEvadeDestination(ActorData actor)
	{
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in m_processedEvades)
		{
			if (evadeInfo != null && evadeInfo.GetMover() == actor)
			{
				return evadeInfo.m_evadeDest;
			}
		}
		return null;
	}

	private List<BoardSquare> GetDestinationSquaresInBorderOf(
		BoardSquare center,
		int borderRadius,
		ServerEvadeUtils.EvadeInfo evade,
		List<ServerEvadeUtils.EvadeInfo> allEvades,
		List<BoardSquare> additionalInvalidSquares_general,
		List<BoardSquare> additionalInvalidSquares_evaderSpecific,
		bool requireLosToCenter)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		if (borderRadius != 0)
		{
			List<BoardSquare> squaresInBorderLayer =
				AreaEffectUtils.GetSquaresInBorderLayer(center, borderRadius, requireLosToCenter);
			foreach (var boardSquare in squaresInBorderLayer)
			{
				if (evade.IsValidEvadeDestination(boardSquare, allEvades)
				    && !additionalInvalidSquares_general.Contains(boardSquare)
				    && !additionalInvalidSquares_evaderSpecific.Contains(boardSquare))
				{
					list.Add(boardSquare);
				}
			}
		}
		else if (evade.IsDestinationReserved())
		{
			if (evade.IsValidEvadeDestination(center, allEvades))
			{
				list.Add(center);
			}
		}
		else if (evade.IsValidEvadeDestination(center, allEvades) &&
		         !additionalInvalidSquares_general.Contains(center) &&
		         !additionalInvalidSquares_evaderSpecific.Contains(center))
		{
			list.Add(center);
		}
		return list;
	}

	public static void ChargeToPos(
		ActorData charger,
		BoardSquarePathInfo chargePath,
		Vector3 facingDirAfterMovement,
		ActorData.MovementType movementType)
	{
		if (chargePath != null)
		{
			BoardSquare square = chargePath.GetPathEndpoint().square;
			charger.QueueMoveToBoardSquareOnEvent(
				square, movementType, ActorData.TeleportType.NotATeleport, chargePath, facingDirAfterMovement);
		}
	}

	public static BoardSquarePathInfo BuildPathForCharge(
		ActorData charger,
		ServerEvadeUtils.ChargeSegment[] targetPositions,
		ActorData.MovementType movementType,
		float speed,
		bool passThroughInvalidSquares)
	{
		BoardSquarePathInfo boardSquarePathInfo = null;
		BoardSquare pos = targetPositions[targetPositions.Length - 1].m_pos;
		GridPos gridPos = pos.GetGridPos();
		for (int i = 1; i < targetPositions.Length; i++)
		{
			BoardSquare pos2 = targetPositions[i].m_pos;
			BoardSquare pos3 = targetPositions[i - 1].m_pos;
			BoardSquarePathInfo boardSquarePathInfo4;
			if (movementType == ActorData.MovementType.WaypointFlight)
			{
				BoardSquarePathInfo boardSquarePathInfo2 = new BoardSquarePathInfo
				{
					square = pos3
				};
				BoardSquarePathInfo boardSquarePathInfo3 = new BoardSquarePathInfo
				{
					square = pos2
				};
				boardSquarePathInfo2.next = boardSquarePathInfo3;
				boardSquarePathInfo3.prev = boardSquarePathInfo2;
				boardSquarePathInfo4 = boardSquarePathInfo2;
			}
			else
			{
				boardSquarePathInfo4 = KnockbackUtils.BuildStraightLineChargePath(
					charger, pos2, pos3, passThroughInvalidSquares);
			}
			for (BoardSquarePathInfo step = boardSquarePathInfo4; step != null; step = step.next)
			{
				step.chargeCycleType = targetPositions[i].m_cycle;
				step.chargeEndType = targetPositions[i].m_end;
				step.segmentMovementSpeed = targetPositions[i].m_segmentMovementSpeed;
				step.segmentMovementDuration = targetPositions[i].m_segmentMovementDuration;
				step.m_reverse = targetPositions[i].m_reverseFacing;
			}
			if (boardSquarePathInfo != null)
			{
				BoardSquarePathInfo pathEndpoint = boardSquarePathInfo.GetPathEndpoint();
				if (boardSquarePathInfo4 != null
				    && boardSquarePathInfo4.next != null
				    && pathEndpoint.square == boardSquarePathInfo4.square)
				{
					pathEndpoint.m_unskippable = true;
					pathEndpoint.next = boardSquarePathInfo4.next;
					boardSquarePathInfo4.next.prev = pathEndpoint;
				}
			}
			else
			{
				boardSquarePathInfo = boardSquarePathInfo4;
			}
		}
		BoardSquarePathInfo boardSquarePathInfo6 = boardSquarePathInfo;
		BoardSquarePathInfo boardSquarePathInfo7 = boardSquarePathInfo;
		while (boardSquarePathInfo6 != null && boardSquarePathInfo6.square != null)
		{
			if (boardSquarePathInfo6.prev == null)
			{
				boardSquarePathInfo6.moveCost = 0f;
			}
			else
			{
				boardSquarePathInfo6.moveCost = boardSquarePathInfo6.prev.moveCost + boardSquarePathInfo6.prev.square.HorizontalDistanceOnBoardTo(boardSquarePathInfo6.square);
			}
			boardSquarePathInfo6.connectionType = BoardSquarePathInfo.ConnectionType.Charge;
			if (movementType == ActorData.MovementType.Flight || movementType == ActorData.MovementType.WaypointFlight)
			{
				boardSquarePathInfo6.connectionType = BoardSquarePathInfo.ConnectionType.Flight;
			}
			if (movementType == ActorData.MovementType.Teleport)
			{
				boardSquarePathInfo6.connectionType = BoardSquarePathInfo.ConnectionType.Teleport;
			}
			boardSquarePathInfo7 = boardSquarePathInfo6;
			boardSquarePathInfo6 = boardSquarePathInfo6.next;
		}
		BoardSquarePathInfo result = null;
		if (boardSquarePathInfo7 == null)
		{
			Log.Error("{0} is building charge path to {1} from {2}, but the path node is null.", charger.DisplayName, gridPos.ToString(), charger.GetGridPos().ToString());
		}
		else if (boardSquarePathInfo7.square == null)
		{
			Log.Error("{0} is building charge path to {1} from {2}, but the path square is null.", charger.DisplayName, gridPos.ToString(), charger.GetGridPos().ToString());
		}
		else if (boardSquarePathInfo7.square != pos)
		{
			Log.Error("{0} is building charge path to {1} from {2}, but he was trying to go to {3}.", charger.DisplayName, boardSquarePathInfo7.square.GetGridPos().ToString(), charger.GetGridPos().ToString(), pos.GetGridPos().ToString());
		}
		else
		{
			result = boardSquarePathInfo;
		}
		return result;
	}

	private struct EvaderSwapInfo
	{
		public ActorData m_actor;
		public BoardSquare m_preSwapSquare;
		public BoardSquare m_postSwapSquare;

		public EvaderSwapInfo(ActorData actor, BoardSquare preSwapSquare, BoardSquare postSwapSquare)
		{
			m_actor = actor;
			m_preSwapSquare = preSwapSquare;
			m_postSwapSquare = postSwapSquare;
		}
	}

	private enum EvaderSwapState
	{
		Present,
		Future,
		AlternateReality
	}
}
#endif
