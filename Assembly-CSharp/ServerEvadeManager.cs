// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only, missing in reactor
#if SERVER
public class ServerEvadeManager
{
	private List<ServerEvadeUtils.EvadeInfo> m_processedEvades;
	private List<ServerEvadeManager.EvaderSwapInfo> m_evaderSwapData;
	private ServerEvadeManager.EvaderSwapState m_currentSwapState;

	public ServerEvadeManager()
	{
		this.m_processedEvades = new List<ServerEvadeUtils.EvadeInfo>();
		this.m_evaderSwapData = new List<ServerEvadeManager.EvaderSwapInfo>();
	}

	private bool DebugShowChargeGizmo
	{
		get
		{
			return false;
		}
	}

	private void AppendNonPlayerEvades(ref List<ServerEvadeUtils.EvadeInfo> evades, List<ServerEvadeUtils.NonPlayerEvadeData> attachedEvadeDataList)
	{
		if (attachedEvadeDataList != null)
		{
			foreach (ServerEvadeUtils.NonPlayerEvadeData nonPlayerEvadeData in attachedEvadeDataList)
			{
				if (nonPlayerEvadeData.ShouldAddToEvades() && nonPlayerEvadeData.IsTeleport())
				{
					evades.Add(new ServerEvadeUtils.NonPlayerTeleportInfo(nonPlayerEvadeData.m_mover, nonPlayerEvadeData.m_start, nonPlayerEvadeData.m_idealDestination, nonPlayerEvadeData));
				}
			}
		}
	}

	public void ProcessEvades(List<AbilityRequest> allRequests, AbilityPriority currentPriority)
	{
		List<ServerEvadeUtils.EvadeInfo> list = new List<ServerEvadeUtils.EvadeInfo>();
		foreach (AbilityRequest abilityRequest in allRequests)
		{
			if (abilityRequest.m_ability.RunPriority == currentPriority && abilityRequest.m_ability.IsCharge())
			{
				list.Add(new ServerEvadeUtils.ChargeInfo(abilityRequest));
				List<ServerEvadeUtils.NonPlayerEvadeData> nonPlayerEvades = abilityRequest.m_ability.GetNonPlayerEvades(abilityRequest.m_targets, abilityRequest.m_caster, abilityRequest.m_additionalData);
				this.AppendNonPlayerEvades(ref list, nonPlayerEvades);
			}
			else if (abilityRequest.m_ability.RunPriority == currentPriority && abilityRequest.m_ability.IsTeleport())
			{
				list.Add(new ServerEvadeUtils.TeleportInfo(abilityRequest));
				List<ServerEvadeUtils.NonPlayerEvadeData> nonPlayerEvades2 = abilityRequest.m_ability.GetNonPlayerEvades(abilityRequest.m_targets, abilityRequest.m_caster, abilityRequest.m_additionalData);
				this.AppendNonPlayerEvades(ref list, nonPlayerEvades2);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		list.Sort(delegate(ServerEvadeUtils.EvadeInfo evade1, ServerEvadeUtils.EvadeInfo evade2)
		{
			float evadePathDistance = evade1.GetEvadePathDistance(evade1.GetIdealDestination());
			float evadePathDistance2 = evade2.GetEvadePathDistance(evade2.GetIdealDestination());
			return evadePathDistance.CompareTo(evadePathDistance2);
		});
		List<BoardSquare> list2 = new List<BoardSquare>();
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in list)
		{
			if (evadeInfo.GetMover() != null && evadeInfo.GetMover().GetPassiveData() != null)
			{
				evadeInfo.GetMover().GetPassiveData().AddInvalidEvadeDestinations(list, list2);
			}
		}
		List<BoardSquare> additionalInvalidSquares_evaderSpecific = new List<BoardSquare>();
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo2 in list)
		{
			BoardSquare idealDestination = evadeInfo2.GetIdealDestination();
			List<BoardSquare> list3 = new List<BoardSquare>();
			int num = 0;
			while (list3.Count == 0 && num <= 4)
			{
				list3 = this.GetDestinationSquaresInBorderOf(idealDestination, num, evadeInfo2, list, list2, additionalInvalidSquares_evaderSpecific, true);
				num++;
			}
			num = 0;
			while (list3.Count == 0 && num <= 4)
			{
				list3 = this.GetDestinationSquaresInBorderOf(idealDestination, num, evadeInfo2, list, list2, additionalInvalidSquares_evaderSpecific, false);
				num++;
			}
			BoardSquare boardSquare = null;
			float num2 = -1f;
			Vector3 bestSquareTestVector = evadeInfo2.GetBestSquareTestVector();
			foreach (BoardSquare boardSquare2 in list3)
			{
				Vector3 vector = boardSquare2.ToVector3() - idealDestination.ToVector3();
				vector.y = 0f;
				vector.Normalize();
				float num3 = Vector3.Dot(bestSquareTestVector, vector);
				if (boardSquare == null || num3 > num2)
				{
					boardSquare = boardSquare2;
					num2 = num3;
				}
			}
			if (boardSquare == null)
			{
				evadeInfo2.MarkAsInvalid();
			}
			else
			{
				evadeInfo2.ModifyDestination(boardSquare);
			}
		}
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo3 in list)
		{
			evadeInfo3.ProcessEvadeDodge(list);
		}
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo4 in list)
		{
			evadeInfo4.StorePath();
			if (evadeInfo4.m_evadePath != null)
			{
				evadeInfo4.m_evadePath.CheckPathConnectionForSelfReference();
			}
		}
		this.ProcessClashes(allRequests, currentPriority, ref list);
		this.m_processedEvades = list;
	}

	public void ProcessClashes(List<AbilityRequest> allRequests, AbilityPriority currentPriority, ref List<ServerEvadeUtils.EvadeInfo> evades)
	{
		ServerClashUtils.MovementClashCollection movementClashCollection = ServerClashUtils.IdentifyClashSegments_Evade(evades);
		List<BoardSquare> list = new List<BoardSquare>();
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in evades)
		{
			if (evadeInfo.GetMover() != null && evadeInfo.GetMover().GetPassiveData() != null)
			{
				evadeInfo.GetMover().GetPassiveData().AddInvalidEvadeDestinations(evades, list);
			}
		}
		List<BoardSquare> list2 = new List<BoardSquare>();
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo2 in evades)
		{
			if (evadeInfo2.m_evadeDest != null && !list2.Contains(evadeInfo2.m_evadeDest))
			{
				list2.Add(evadeInfo2.m_evadeDest);
			}
		}
		bool flag = false;
		List<ServerClashUtils.MovementClash> list3 = new List<ServerClashUtils.MovementClash>();
		foreach (ServerClashUtils.MovementClash movementClash in movementClashCollection.m_clashes)
		{
			if (!movementClash.m_continuing)
			{
				list3.Add(movementClash);
				flag = true;
				if (movementClash.m_clashSquare != null && !list2.Contains(movementClash.m_clashSquare))
				{
					list2.Add(movementClash.m_clashSquare);
				}
			}
		}
		foreach (ServerClashUtils.MovementClash movementClash2 in list3)
		{
			List<ServerClashUtils.MovementClashParticipant> allParticipants = movementClash2.GetAllParticipants();
			List<ServerEvadeUtils.EvadeInfo> list4 = new List<ServerEvadeUtils.EvadeInfo>();
			foreach (ServerEvadeUtils.EvadeInfo evadeInfo3 in evades)
			{
				bool flag2 = false;
				using (List<ServerClashUtils.MovementClashParticipant>.Enumerator enumerator3 = allParticipants.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.Actor == evadeInfo3.GetMover())
						{
							flag2 = true;
							break;
						}
					}
				}
				if (flag2)
				{
					list4.Add(evadeInfo3);
				}
			}
			foreach (ServerEvadeUtils.EvadeInfo evadeInfo4 in list4)
			{
				evadeInfo4.ResetDestinationData();
			}
			foreach (ServerEvadeUtils.EvadeInfo evadeInfo5 in list4)
			{
				BoardSquare idealDestination = evadeInfo5.GetIdealDestination();
				List<BoardSquare> list5 = new List<BoardSquare>();
				int num = 0;
				while (list5.Count == 0 && num <= 4)
				{
					list5 = this.GetDestinationSquaresInBorderOf(idealDestination, num, evadeInfo5, evades, list, list2, true);
					num++;
				}
				num = 0;
				while (list5.Count == 0 && num <= 4)
				{
					list5 = this.GetDestinationSquaresInBorderOf(idealDestination, num, evadeInfo5, evades, list, list2, false);
					num++;
				}
				BoardSquare boardSquare = null;
				float num2 = -1f;
				Vector3 bestSquareTestVector = evadeInfo5.GetBestSquareTestVector();
				foreach (BoardSquare boardSquare2 in list5)
				{
					Vector3 vector = boardSquare2.ToVector3() - idealDestination.ToVector3();
					vector.y = 0f;
					vector.Normalize();
					float num3 = Vector3.Dot(bestSquareTestVector, vector);
					if (boardSquare == null || num3 > num2)
					{
						boardSquare = boardSquare2;
						num2 = num3;
					}
				}
				if (boardSquare == null)
				{
					evadeInfo5.MarkAsInvalid();
				}
				else
				{
					evadeInfo5.ModifyDestination(boardSquare);
					if (!list2.Contains(boardSquare))
					{
						list2.Add(boardSquare);
					}
				}
			}
			foreach (ServerEvadeUtils.EvadeInfo evadeInfo6 in list4)
			{
				evadeInfo6.ProcessEvadeDodge(list4);
			}
			foreach (ServerEvadeUtils.EvadeInfo evadeInfo7 in list4)
			{
				evadeInfo7.StorePath();
				if (evadeInfo7.m_evadePath != null)
				{
					evadeInfo7.m_evadePath.CheckPathConnectionForSelfReference();
				}
			}
		}
		if (flag)
		{
			ClientClashManager.SendClashesAtEndOfMovementMsgToClients(list3);
			movementClashCollection = ServerClashUtils.IdentifyClashSegments_Evade(evades);
		}
	}

	public void RunEvades()
	{
		if (this.m_processedEvades == null)
		{
			Log.Error("Trying to run evades, but they have not been processed.");
			return;
		}
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in this.m_processedEvades)
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
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo2 in this.m_processedEvades)
		{
			if (evadeInfo2.m_request == null && !(evadeInfo2 is ServerEvadeUtils.NonPlayerTeleportInfo))
			{
				Log.Error("Trying to run evade has null ability request.");
			}
			else if (evadeInfo2.IsStillValid())
			{
				if (evadeInfo2 is ServerEvadeUtils.ChargeInfo)
				{
					ServerEvadeUtils.ChargeInfo chargeInfo = evadeInfo2 as ServerEvadeUtils.ChargeInfo;
					Ability ability = chargeInfo.m_request.m_ability;
					Vector3 facingDirAfterMovement = (ability != null) ? ability.GetFacingDirAfterMovement(evadeInfo2) : Vector3.zero;
					ServerEvadeManager.ChargeToPos(chargeInfo.m_request.m_caster, chargeInfo.m_evadePath, facingDirAfterMovement, chargeInfo.m_request.m_ability.GetMovementType());
					if (Application.isEditor && this.DebugShowChargeGizmo)
					{
						for (BoardSquarePathInfo boardSquarePathInfo = chargeInfo.m_evadePath; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
						{
							if (boardSquarePathInfo.next == null)
							{
								break;
							}
							Debug.DrawLine(boardSquarePathInfo.square.ToVector3(), boardSquarePathInfo.next.square.ToVector3(), Color.red, 10f);
						}
					}
				}
				else if (evadeInfo2 is ServerEvadeUtils.TeleportInfo)
				{
					ServerEvadeUtils.TeleportInfo teleportInfo = evadeInfo2 as ServerEvadeUtils.TeleportInfo;
					ActorData caster = teleportInfo.m_request.m_caster;
					Ability ability2 = teleportInfo.m_request.m_ability;
					caster.TeleportToBoardSquare(teleportInfo.m_evadeDest, ability2.GetFacingDirAfterMovement(evadeInfo2), ability2.GetEvasionTeleportType(), teleportInfo.m_evadePath, ability2.CalcMovementSpeed(evadeInfo2.GetEvadeDistance()), ability2.GetMovementType(), GameEventManager.EventType.TheatricsEvasionMoveStart, null);
				}
				else if (evadeInfo2 is ServerEvadeUtils.NonPlayerTeleportInfo)
				{
					ServerEvadeUtils.NonPlayerTeleportInfo nonPlayerTeleportInfo = evadeInfo2 as ServerEvadeUtils.NonPlayerTeleportInfo;
					ActorData mover = nonPlayerTeleportInfo.GetMover();
					if (mover != null)
					{
						mover.TeleportToBoardSquare(nonPlayerTeleportInfo.m_evadeDest, nonPlayerTeleportInfo.m_attachedEvadeData.m_facingDirection, ActorData.TeleportType.Evasion_DontAdjustToVision, nonPlayerTeleportInfo.m_evadePath, nonPlayerTeleportInfo.m_attachedEvadeData.m_moveSpeed, nonPlayerTeleportInfo.m_attachedEvadeData.m_movementType, GameEventManager.EventType.TheatricsEvasionMoveStart, nonPlayerTeleportInfo.m_teleportStart);
					}
				}
			}
		}
		this.m_processedEvades.Clear();
	}

	public void GatherGameplayResultsInResponseToEvades(out List<ActorData> actorsThatWillBeSeenButArentMoving)
	{
		MovementCollection movementCollection = new MovementCollection(this.m_processedEvades);
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
		List<ActorData> list;
		ServerGameplayUtils.SetServerLastKnownPositionsForMovement(movementCollection, out actorsThatWillBeSeenButArentMoving, out list);
	}

	public void SwapEvaderSquaresWithDestinations()
	{
		if (this.m_currentSwapState != ServerEvadeManager.EvaderSwapState.Present)
		{
			Log.Error("Swapping evader squares to gather results, but we already swapped, and are in '" + this.m_currentSwapState.ToString() + "' mode.");
		}
		this.m_evaderSwapData.Clear();
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in this.m_processedEvades)
		{
			this.m_evaderSwapData.Add(new ServerEvadeManager.EvaderSwapInfo(evadeInfo.GetMover(), evadeInfo.GetMover().GetCurrentBoardSquare(), evadeInfo.m_evadeDest));
		}
		ServerEvadeManager.SwapActorSquares(this.m_evaderSwapData);
		this.m_currentSwapState = ServerEvadeManager.EvaderSwapState.Future;
	}

	public void SwapEvaderCurrentSquaresWithPreEvadeSquares(out bool swapsOccured)
	{
		swapsOccured = false;
		if (this.m_currentSwapState != ServerEvadeManager.EvaderSwapState.Present)
		{
			Log.Error("Swapping evader squares to gather results, but we already swapped, and are in '" + this.m_currentSwapState.ToString() + "' mode.");
		}
		this.m_evaderSwapData.Clear();
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (!(actorData == null) && !actorData.IsDead() && !(actorData.CurrentBoardSquare == null) && actorData.CurrentBoardSquare != actorData.SquareAtResolveStart)
			{
				this.m_evaderSwapData.Add(new ServerEvadeManager.EvaderSwapInfo(actorData, actorData.CurrentBoardSquare, actorData.SquareAtResolveStart));
				swapsOccured = true;
			}
		}
		if (swapsOccured)
		{
			ServerEvadeManager.SwapActorSquares(this.m_evaderSwapData);
		}
		this.m_currentSwapState = ServerEvadeManager.EvaderSwapState.AlternateReality;
	}

	private static void SwapActorSquares(List<ServerEvadeManager.EvaderSwapInfo> swapData)
	{
		foreach (ServerEvadeManager.EvaderSwapInfo evaderSwapInfo in swapData)
		{
			evaderSwapInfo.m_actor.UnoccupyCurrentBoardSquare();
		}
		foreach (ServerEvadeManager.EvaderSwapInfo evaderSwapInfo2 in swapData)
		{
			evaderSwapInfo2.m_actor.SwapBoardSquare(evaderSwapInfo2.m_postSwapSquare);
		}
		foreach (ServerEvadeManager.EvaderSwapInfo evaderSwapInfo3 in swapData)
		{
			evaderSwapInfo3.m_actor.OccupyCurrentBoardSquare();
		}
	}

	public void UndoEvaderDestinationsSwap()
	{
		if (this.m_currentSwapState == ServerEvadeManager.EvaderSwapState.Present)
		{
			Log.Error("Undoing swap of evader squares to gather results, but we already undid that, and are in '" + this.m_currentSwapState.ToString() + "' mode.");
		}
		foreach (ServerEvadeManager.EvaderSwapInfo evaderSwapInfo in this.m_evaderSwapData)
		{
			evaderSwapInfo.m_actor.UnoccupyCurrentBoardSquare();
		}
		foreach (ServerEvadeManager.EvaderSwapInfo evaderSwapInfo2 in this.m_evaderSwapData)
		{
			evaderSwapInfo2.m_actor.SwapBoardSquare(evaderSwapInfo2.m_preSwapSquare);
		}
		foreach (ServerEvadeManager.EvaderSwapInfo evaderSwapInfo3 in this.m_evaderSwapData)
		{
			evaderSwapInfo3.m_actor.OccupyCurrentBoardSquare();
		}
		this.m_evaderSwapData.Clear();
		this.m_currentSwapState = ServerEvadeManager.EvaderSwapState.Present;
	}

	public bool HasEvades()
	{
		return this.m_processedEvades.Count > 0;
	}

	public bool HasProcessedEvadeForActor(ActorData actor)
	{
		bool result = false;
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in this.m_processedEvades)
		{
			if (evadeInfo != null && evadeInfo.GetMover() == actor)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public int GetNumSquaresInProcessedEvade(ActorData actor)
	{
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in this.m_processedEvades)
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
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in this.m_processedEvades)
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
		BoardSquare result = null;
		foreach (ServerEvadeUtils.EvadeInfo evadeInfo in this.m_processedEvades)
		{
			if (evadeInfo != null && evadeInfo.GetMover() == actor)
			{
				result = evadeInfo.m_evadeDest;
				break;
			}
		}
		return result;
	}

	private List<BoardSquare> GetDestinationSquaresInBorderOf(BoardSquare center, int borderRadius, ServerEvadeUtils.EvadeInfo evade, List<ServerEvadeUtils.EvadeInfo> allEvades, List<BoardSquare> additionalInvalidSquares_general, List<BoardSquare> additionalInvalidSquares_evaderSpecific, bool requireLosToCenter)
	{
		List<BoardSquare> list = new List<BoardSquare>();
		if (borderRadius == 0)
		{
			if (evade.IsDestinationReserved())
			{
				if (evade.IsValidEvadeDestination(center, allEvades))
				{
					list.Add(center);
				}
			}
			else if (evade.IsValidEvadeDestination(center, allEvades) && !additionalInvalidSquares_general.Contains(center) && !additionalInvalidSquares_evaderSpecific.Contains(center))
			{
				list.Add(center);
			}
		}
		else
		{
			List<BoardSquare> squaresInBorderLayer = AreaEffectUtils.GetSquaresInBorderLayer(center, borderRadius, requireLosToCenter);
			for (int i = 0; i < squaresInBorderLayer.Count; i++)
			{
				BoardSquare boardSquare = squaresInBorderLayer[i];
				if (evade.IsValidEvadeDestination(boardSquare, allEvades) && !additionalInvalidSquares_general.Contains(boardSquare) && !additionalInvalidSquares_evaderSpecific.Contains(boardSquare))
				{
					list.Add(boardSquare);
				}
			}
		}
		return list;
	}

	public static void ChargeToPos(ActorData charger, BoardSquarePathInfo chargePath, Vector3 facingDirAfterMovement, ActorData.MovementType movementType)
	{
		if (chargePath != null)
		{
			BoardSquare square = chargePath.GetPathEndpoint().square;
			charger.QueueMoveToBoardSquareOnEvent(square, movementType, ActorData.TeleportType.NotATeleport, chargePath, facingDirAfterMovement, GameEventManager.EventType.TheatricsEvasionMoveStart);
		}
	}

	public static BoardSquarePathInfo BuildPathForCharge(ActorData charger, ServerEvadeUtils.ChargeSegment[] targetPositions, ActorData.MovementType movementType, float speed, bool passThroughInvalidSquares)
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
				BoardSquarePathInfo boardSquarePathInfo2 = new BoardSquarePathInfo();
				boardSquarePathInfo2.square = pos3;
				BoardSquarePathInfo boardSquarePathInfo3 = new BoardSquarePathInfo();
				boardSquarePathInfo3.square = pos2;
				boardSquarePathInfo2.next = boardSquarePathInfo3;
				boardSquarePathInfo3.prev = boardSquarePathInfo2;
				boardSquarePathInfo4 = boardSquarePathInfo2;
			}
			else
			{
				boardSquarePathInfo4 = KnockbackUtils.BuildStraightLineChargePath(charger, pos2, pos3, passThroughInvalidSquares);
			}
			for (BoardSquarePathInfo boardSquarePathInfo5 = boardSquarePathInfo4; boardSquarePathInfo5 != null; boardSquarePathInfo5 = boardSquarePathInfo5.next)
			{
				boardSquarePathInfo5.chargeCycleType = targetPositions[i].m_cycle;
				boardSquarePathInfo5.chargeEndType = targetPositions[i].m_end;
				boardSquarePathInfo5.segmentMovementSpeed = targetPositions[i].m_segmentMovementSpeed;
				boardSquarePathInfo5.segmentMovementDuration = targetPositions[i].m_segmentMovementDuration;
				boardSquarePathInfo5.m_reverse = targetPositions[i].m_reverseFacing;
			}
			if (boardSquarePathInfo != null)
			{
				BoardSquarePathInfo pathEndpoint = boardSquarePathInfo.GetPathEndpoint();
				if (boardSquarePathInfo4 != null && boardSquarePathInfo4.next != null && pathEndpoint.square == boardSquarePathInfo4.square)
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
			Log.Error("{0} is building charge path to {1} from {2}, but the path node is null.", new object[]
			{
				charger.DisplayName,
				gridPos.ToString(),
				charger.GetGridPos().ToString()
			});
		}
		else if (boardSquarePathInfo7.square == null)
		{
			Log.Error("{0} is building charge path to {1} from {2}, but the path square is null.", new object[]
			{
				charger.DisplayName,
				gridPos.ToString(),
				charger.GetGridPos().ToString()
			});
		}
		else if (boardSquarePathInfo7.square != pos)
		{
			Log.Error("{0} is building charge path to {1} from {2}, but he was trying to go to {3}.", new object[]
			{
				charger.DisplayName,
				boardSquarePathInfo7.square.GetGridPos().ToString(),
				charger.GetGridPos().ToString(),
				pos.GetGridPos().ToString()
			});
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
			this.m_actor = actor;
			this.m_preSwapSquare = preSwapSquare;
			this.m_postSwapSquare = postSwapSquare;
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
