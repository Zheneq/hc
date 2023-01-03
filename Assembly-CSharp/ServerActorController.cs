// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
//using Escalation;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

// was empty in reactor
// TODO HIGH was reworked in rogues
public class ServerActorController : MonoBehaviour
{
#if SERVER
	// rogues
	public virtual void ProcessSetSquareRequest(int x, int y, bool setWaypoint, bool forceDelayExecution = false) // there is no forceDelayExecution in reactor
	{
		ActorData actorData = GetComponent<ActorData>();
		ActorMovement actorMovement = GetComponent<ActorMovement>();
		ActorTurnSM actorTurnSm = GetComponent<ActorTurnSM>();
		AbilityData abilityData = GetComponent<AbilityData>();
		bool success = false;
		if (actorData != null
		    && actorMovement != null
		    && actorTurnSm != null
		    && abilityData != null
		    && GameFlowData.Get() != null
		    && ServerActionBuffer.Get() != null
		    && Board.Get() != null)
		{
			bool amDecidingMovement = actorTurnSm.AmDecidingMovement();
			bool isInDecisionState = GameFlowData.Get().IsInDecisionState();
			
			// rogues
			//bool isAcceptingInput = ServerActionBuffer.Get().GetPlayerActionFSM().IsAcceptingInput();
			//bool executeImmediately = PlayerActionStateMachine.ExecutePlayerActionImmediately() && !forceDelayExecution && isAcceptingInput;
			// custom
			bool executeImmediately = false;
			
			if (amDecidingMovement && isInDecisionState)
			{
				if (ServerActionBuffer.Get().HasPendingMovementRequest(actorData) && !executeImmediately)
				{
					if (setWaypoint && FirstTurnMovement.CanWaypoint())
					{
						if (actorData.CanMoveToBoardSquare(x, y))
						{
							ServerActionBuffer.Get().AppendToMovementRequest(x, y, actorData);
							success = true;
						}
					}
					else
					{
						float maxMovement = actorData.GetActorMovement().CalculateMaxHorizontalMovement();
						BoardSquarePathInfo path = actorData.GetActorMovement().BuildCompletePathTo(
							actorData.InitialMoveStartSquare,
							Board.Get().GetSquareFromIndex(x, y),
							false,
							null);
						if (path != null && path.IsValidPathForMaxMovement(maxMovement))
						{
							ServerActionBuffer.Get().CancelMovementRequests(actorData);
							ServerActionBuffer.Get().StoreMovementRequest(x, y, actorData, path);
							success = true;
						}
					}
				}
				else if (actorData.CanMoveToBoardSquare(x, y))
				{
					if (ServerActionBuffer.Get().HasPendingMovementRequest(actorData))
					{
						ServerActionBuffer.Get().CancelMovementRequests(actorData);
					}
					BoardSquare targetSquare = Board.Get().GetSquareFromIndex(x, y);
					if (targetSquare.OccupantActor == null
					    || !targetSquare.OccupantActor.IsActorVisibleToActor(actorData)
					    || !executeImmediately)
					{
						BoardSquarePathInfo path = actorData.GetActorMovement().BuildCompletePathTo(
							actorData.InitialMoveStartSquare,
							targetSquare,
							false,
							null);
						if (path != null)
						{
							ServerActionBuffer.Get().StoreMovementRequest(targetSquare.x, targetSquare.y, actorData, path);

							// rogues
							//if (executeImmediately)
							//{
							//	ServerActionBuffer.Get().GetPlayerActionFSM().RunQueuedActionsFromActor(component);
							//}
						}
						success = true;
					}
				}
			}
		}
		actorTurnSm.OnMessage(success ? TurnMessage.MOVEMENT_ACCEPTED : TurnMessage.MOVEMENT_REJECTED);
	}

	// custom
	public virtual void ProcessChaseRequest(int x, int y)
	{
		ActorData actorData = GetComponent<ActorData>();
		ActorMovement actorMovement = GetComponent<ActorMovement>();
		ActorTurnSM actorTurnSm = GetComponent<ActorTurnSM>();
		Board board = Board.Get();
		bool success = false;
		if (actorData != null
		    && actorMovement != null
		    && actorTurnSm != null
		    && GameFlowData.Get() != null
		    && ServerActionBuffer.Get() != null
		    && board != null)
		{
			ActorData target = board.GetSquareFromIndex(x, y)?.occupant?.GetComponent<ActorData>();
			if (actorTurnSm.AmDecidingMovement()
			    && GameFlowData.Get().IsInDecisionState()
			    && target != null
			    && (target.GetTeam() == actorData.GetTeam() || target.IsActorVisibleToAnyEnemy()))
			{
				if (ServerActionBuffer.Get().HasPendingMovementRequest(actorData))
				{
					ServerActionBuffer.Get().CancelMovementRequests(actorData);
				}
				ServerActionBuffer.Get().StoreChaseRequest(target, actorData, false, true);
				success = true;
			}
		}
		actorTurnSm.OnMessage(success ? TurnMessage.MOVEMENT_ACCEPTED : TurnMessage.MOVEMENT_REJECTED);
	}

	// rogues ?
	//public virtual void ProcessGroupMoveRequestForOwnedActors(int actorIndex, int x, int y)
	//{
	//	if (!ServerActionBuffer.Get().GetPlayerActionFSM().IsAcceptingInput() || GameFlowData.Get().IsInCombat)
	//	{
	//		return;
	//	}
	//	ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
	//	if (actorData != null && actorData.GetAccountId() > 0L)
	//	{
	//		long accountId = actorData.GetAccountId();
	//		List<ActorData> list = new List<ActorData>();
	//		foreach (ActorData actorData2 in GameFlowData.Get().GetActors())
	//		{
	//			if (actorData2.GetTeam() == actorData.GetTeam() && actorData2.GetAccountId() == accountId && actorData2.IsHumanControlled())
	//			{
	//				list.Add(actorData2);
	//			}
	//		}
	//		if (list.Count > 0)
	//		{
	//			ServerActionBuffer.Get().ProcessGroupMoveRequest(Board.Get().GetSquareFromIndex(x, y), list, actorData);
	//		}
	//	}
	//}

	// rogues
	internal virtual void ProcessSelectAbilityRequest()
	{
		ActorData actorData = GetComponent<ActorData>();
		ActorTurnSM actorTurnSM = GetComponent<ActorTurnSM>();
		if (actorData == null
		    || actorTurnSM == null
		    || !actorTurnSM.CanSelectAbility())
		{
			return;
		}
		BoardSquare boardSquare = null;
		AbilityData abilityData = GetComponent<AbilityData>();
		if (abilityData != null)
		{
			boardSquare = abilityData.GetAutoSelectTarget();
		}
		bool isAutoSelect = boardSquare != null;
		if (SinglePlayerManager.Get())
		{
			SinglePlayerManager.Get().OnActorAbilitySelected(actorData);
		}
		if (isAutoSelect)
		{
			List<AbilityTarget> targets = AbilityTarget.AbilityTargetList(AbilityTarget.CreateAbilityTargetFromBoardSquare(boardSquare, actorData.GetFreePos()));
			ProcessCastAbilityRequest(targets, abilityData.GetSelectedActionType(), false);
		}
		else
		{
			actorTurnSM.OnMessage(TurnMessage.SELECTED_ABILITY);
		}
	}

	// rogues
	public virtual void ProcessQueueSimpleActionRequest(AbilityData.ActionType actionType, bool forceDelayExecution)
	{
		ActorData actorData = GetComponent<ActorData>();
		ActorTurnSM actorTurnSM = actorData.GetActorTurnSM();
		if (actorData != null
		    && actorTurnSM != null
		    && actorTurnSM.CanQueueSimpleAction())
		{
			ProcessCastSimpleActionRequest(actionType, forceDelayExecution);
		}
		if (SinglePlayerManager.Get())
		{
			SinglePlayerManager.Get().OnActorAbilitySelected(actorData);
		}
	}

	// rogues
	public virtual void ProcessCancelAbilitySelection()
	{
		ActorData actorData = GetComponent<ActorData>();
		ActorTurnSM actorTurnSm = GetComponent<ActorTurnSM>();
		if (actorData != null
		    && actorTurnSm != null
		    && actorTurnSm.CurrentState == TurnStateEnum.TARGETING_ACTION)
		{
			Ability ability = null;

            // rogues?
            //if (component.m_tauntRequestedForNextAbility == (int)actorData.GetAbilityData().GetSelectedActionType())
            //{
            //    component.Networkm_tauntRequestedForNextAbility = -1;
            //    CharacterTaunt characterTaunt = actorData.GetCharacterResourceLink().m_taunts.Find((CharacterTaunt t) => t.m_actionForTaunt == actorData.GetAbilityData().GetSelectedActionType());
            //    if (characterTaunt != null && characterTaunt.m_modToApplyOnTaunt >= 0)
            //    {
            //        ability = actorData.GetAbilityData().GetAbilityOfActionType(actorData.GetAbilityData().GetSelectedActionType());
            //    }
            //}

            GetComponent<AbilityData>().ClearSelectedAbility();
			if (ability)
			{
				ability.ClearAbilityMod(actorData);
			}
			CheckAndCancelMovementRequestBeyondRange(actorData, false);
		}
	}

	// rogues
	internal virtual void ProcessCastAbilityRequest(List<AbilityTarget> targets, AbilityData.ActionType actionType, bool forceDelayExecution)
	{
		ActorData actorData = GetComponent<ActorData>();
		ActorTurnSM actorTurnSM = GetComponent<ActorTurnSM>();
		bool isAutoSelect = false;
		AbilityData abilityData = GetComponent<AbilityData>();
		if (abilityData != null && abilityData.GetAutoSelectTarget() != null)
		{
			isAutoSelect = true;
		}
		Ability ability = abilityData.GetAbilityOfActionType(actionType);
		if (ability != null && ability.IsSimpleAction())
		{
			Log.Error($"Ability {ability.m_abilityName} is trying to be cast as a non-simple ability, but it's simple.");
		}
		if (actionType >= AbilityData.ActionType.CARD_0
			&& actionType <= AbilityData.ActionType.CARD_2
			&& ability != null
			&& ability.m_actionAnimType != ActorModelData.ActionAnimationType.None)
		{
			Log.Warning("Setting Animation Type to None for card ability");
			ability.m_actionAnimType = ActorModelData.ActionAnimationType.None;
		}
		if (actorData != null && actorTurnSM != null)
		{
			bool isValidState = actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION
				|| (actorTurnSM.CurrentState == TurnStateEnum.DECIDING && isAutoSelect)
				|| (actorTurnSM.CurrentState == TurnStateEnum.DECIDING && GameplayUtils.IsBot(this));

			// custom
			Log.Info($"ProcessCastAbilityRequest isValidState {isValidState}");

			// rogues
			//bool isAcceptingInput = ServerActionBuffer.Get().GetPlayerActionFSM().IsAcceptingInput();
			// custom random
			bool isAcceptingInput = true;

			bool isAccepted = false;
			if (isValidState && abilityData.ValidateActionRequest(actionType, targets))
			{
				StoreAbilityRequest_FCFS(ability, actionType, targets, actorData, forceDelayExecution || !isAcceptingInput);
				isAccepted = true;
			}
			if (!isAccepted)
			{
				actorTurnSM.OnMessage(TurnMessage.ABILITY_REQUEST_REJECTED);
			}
		}
	}

	// rogues
	public virtual void ProcessCastSimpleActionRequest(AbilityData.ActionType actionType, bool forceDelayExecution)
	{
		ActorData actorData = GetComponent<ActorData>();
		ActorTurnSM actorTurnSm = GetComponent<ActorTurnSM>();
		AbilityData abilityData = GetComponent<AbilityData>();
		if (actorData == null || actorTurnSm == null || abilityData == null)
		{
			Log.Warning($"ProcessCastSimpleActionRequest being called with action {actionType.ToString()}, " +
			            $"but the actor is missing a critial component.");
			return;
		}
		Ability ability = abilityData.GetAbilityOfActionType(actionType);
		if (ability == null)
		{
			Log.Warning($"ProcessCastSimpleActionRequest being called with action {actionType.ToString()}, " +
			            $"but the actor has no ability for that action.  (Actor = {actorData.DebugNameString()})");
			return;
		}
		if (!ability.IsSimpleAction())
		{
			Log.Error($"Ability {ability.m_abilityName} is trying to be cast as a simple action, " +
			          $"but isn't a simple action.");
		}
		if (actionType >= AbilityData.ActionType.CARD_0
		    && actionType <= AbilityData.ActionType.CARD_2
		    && ability != null
		    && ability.m_actionAnimType != ActorModelData.ActionAnimationType.None)
		{
			Log.Warning("Setting Animation Type to None for card ability");
			ability.m_actionAnimType = ActorModelData.ActionAnimationType.None;
		}
		if (actorData != null && actorTurnSm != null)
		{
			List<AbilityTarget> targets = AbilityTarget.AbilityTargetList(ability.CreateAbilityTargetForSimpleAction(actorData));
			bool canQueueSimpleAction = actorTurnSm.CanQueueSimpleAction();
			bool validateActionRequest = abilityData.ValidateActionRequest(actionType, targets);

			// rogues
			//bool isAcceptingInput = ServerActionBuffer.Get().GetPlayerActionFSM().IsAcceptingInput();
			// custom random
			bool isAcceptingInput = true;

			if (canQueueSimpleAction && validateActionRequest)
			{
				StoreAbilityRequest_FCFS(ability, actionType, targets, actorData, forceDelayExecution || !isAcceptingInput);
			}
		}
	}

	// rogues
	public virtual void ProcessCancelActionRequest(AbilityData.ActionType actionType, bool hasPendingStoreRequest)
	{
		ActorData actorData = GetComponent<ActorData>();
		ActorTurnSM actorTurnSm = GetComponent<ActorTurnSM>();
		AbilityData abilityData = GetComponent<AbilityData>();
		Ability ability = abilityData.GetAbilityOfActionType(actionType);
		if (actorData != null
		    && actorTurnSm != null
		    && ability != null
		    && (!ability.ShouldAutoQueueIfValid() || ability.AllowCancelWhenAutoQueued()))
		{
			// rogues?
			// if (actorTurnSm.m_tauntRequestedForNextAbility == (int)actionType)
			// {
			// 	CharacterTaunt characterTaunt = actorData.GetCharacterResourceLink().m_taunts.Find((CharacterTaunt t) => t.m_actionForTaunt == abilityData.GetSelectedActionType());
			// 	if (characterTaunt != null && characterTaunt.m_modToApplyOnTaunt >= 0)
			// 	{
			// 		abilityOfActionType.ClearAbilityMod(component);
			// 	}
			// }

			ServerActionBuffer.Get().CancelAbilityRequest(actorData, ability, true, false);
			
			// custom
			// TODO hack? Is there a better way to recalculate this flag?
			if (ability.m_movementAdjustment != Ability.MovementAdjustment.FullMovement)
			{
				actorData.QueuedMovementAllowsAbility = true;
			}
			// end custom
			
			BoardSquare initialMoveStartSquare = actorData.InitialMoveStartSquare;
			if (actorData.GetServerMoveRequestStartSquare() != initialMoveStartSquare)
			{
				actorData.InitialMoveStartSquare = actorData.GetServerMoveRequestStartSquare();
				ServerActionBuffer.Get().CancelMovementRequests(actorData);
			}
			if (ability.GetStatusToApplyWhenRequested().Count > 0)
			{
				actorData.GetActorMovement().UpdateSquaresCanMoveTo();
				if (NetworkServer.active && actorData.GetActorController() != null)
				{
					actorData.GetActorController().CallRpcUpdateRemainingMovement(
						actorData.RemainingHorizontalMovement,
						actorData.RemainingMovementWithQueuedAbility);
				}
				CheckAndCancelMovementRequestBeyondRange(actorData, hasPendingStoreRequest);
			}
		}
	}

	// rogues
	private void StoreAbilityRequest_FCFS(Ability ability, AbilityData.ActionType actionType, List<AbilityTarget> targets, ActorData actorData, bool forceDelayExecution)
	{
		ServerActionBuffer.Get().StoreAbilityRequest(ability, actionType, targets, actorData);
		actorData.GetActorTurnSM().OnMessage(TurnMessage.ABILITY_REQUEST_ACCEPTED, (int)actionType);

		// rogues
		//if (PlayerActionStateMachine.ExecutePlayerActionImmediately() && !forceDelayExecution)
		//{
		//	ServerActionBuffer.Get().GetPlayerActionFSM().RunQueuedActionsFromActor(actorData);
		//}

		actorData.InitialMoveStartSquare = actorData.GetServerMoveRequestStartSquare();
		if (SinglePlayerManager.Get())
		{
			SinglePlayerManager.Get().OnActorAbilityTargeted(actorData);
		}
		if (ability.GetStatusToApplyWhenRequested().Count > 0)
		{
			actorData.GetActorMovement().UpdateSquaresCanMoveTo();
		}
		if (NetworkServer.active && actorData.GetActorController() != null)
		{
			actorData.GetActorController().CallRpcUpdateRemainingMovement(actorData.RemainingHorizontalMovement, actorData.RemainingMovementWithQueuedAbility);
		}
		CheckAndCancelMovementRequestBeyondRange(actorData, false);
	}

	// rogues
	public virtual void CancelActionRequestsForTurnStart()
	{
		ActorData actorData = GetComponent<ActorData>();
		ServerActionBuffer.Get().CancelActionRequests(actorData, true);
		AbilityData abilityData = GetComponent<AbilityData>();
		if (abilityData)
		{
			abilityData.ClearSelectedAbility();
		}
	}

	// rogues
	protected virtual void CheckAndCancelMovementRequestBeyondRange(ActorData actor, bool hasPendingStoreRequest)
	{
		// rogues
		//if (PlayerActionStateMachine.ExecutePlayerActionImmediately())
		//{
		//	return;
		//}

		float maxMovement = actor.GetActorMovement().CalculateMaxHorizontalMovement();
		if (ServerActionBuffer.Get().HasNormalMovementRequestOutsideOfRange(actor, maxMovement))
		{
			ServerActionBuffer.Get().CancelMovementRequests(actor);
			return;
		}
		if (ServerActionBuffer.Get().HasPendingMovementRequest(actor)
		    && actor.QueuedMovementAllowsAbility
		    && !hasPendingStoreRequest)
		{
			ServerActionBuffer.Get().GatherMovementInfo(actor, out BoardSquare destination, out _, out bool isChasing);
			if (!isChasing && !actor.GetActorMovement().SquaresCanMoveToWithQueuedAbility.Contains(destination))
			{
				actor.QueuedMovementAllowsAbility = false;
			}
		}
	}

	// rogues
	public virtual void ProcessCancelMovementRequests()
	{
		// rogues
		//if (ServerActionBuffer.Get().GetPlayerActionFSM().IsAcceptingInput())
		//{
			ActorData component = GetComponent<ActorData>();
			ServerActionBuffer.Get().CancelMovementRequests(component);
		//	return;
		//}
		//PveLog.DebugLog("cancel_movement_request ignored, not accepting player input", null);
	}

	// rogues
	public virtual void DebugTeleport(BoardSquare destinationSquare)
	{
		if (DebugParameters.Get() != null
		    && DebugParameters.Get().GetParameterAsBool("AllowTeleport")
		    && destinationSquare != null
		    && destinationSquare.IsValidForGameplay())
		{
			ActorData component = GetComponent<ActorData>();
			if (component != null)
			{
				component.TeleportToBoardSquare(destinationSquare, Vector3.zero, ActorData.TeleportType.Debug, null);
			}
		}
	}

	// rogues
	//public virtual void ProcessReviveAllyRequest(int x, int y)
	//{
	//	if (NetworkServer.active)
	//	{
	//		ActorData component = base.GetComponent<ActorData>();
	//		if (component.GetActorTurnSM().HasRemainingAbilityUse(true))
	//		{
	//			List<ActorData> allActorsOnTeam = GameFlowData.Get().GetAllActorsOnTeam(component.GetTeam());
	//			ActorData actorData = null;
	//			foreach (ActorData actorData2 in allActorsOnTeam)
	//			{
	//				if (actorData2.IsDead() && actorData2.RespawnPickedPositionSquare == null && actorData2.NextRespawnTurn <= 0)
	//				{
	//					BoardSquare squareFromVec = Board.Get().GetSquareFromVec3(actorData2.LastDeathPosition);
	//					if (squareFromVec != null && squareFromVec.x == x && squareFromVec.y == y)
	//					{
	//						actorData = actorData2;
	//						break;
	//					}
	//				}
	//			}
	//			if (actorData != null)
	//			{
	//				EscalationProducer.Get().SpendReviveToken();
	//				int modifiedStatInt = actorData.GetActorStats().GetModifiedStatInt(StatType.ReviveDelay);
	//				GameFlowData.Get().NotifyOnActorRevived(actorData);
	//				if (GameFlowData.Get().CurrentTurn > actorData.LastDeathTurn + modifiedStatInt)
	//				{
	//					component.GetActorTurnSM().IncrementPveNumFreeActions(1);
	//					actorData.NextRespawnTurn = GameFlowData.Get().CurrentTurn + 1;
	//					actorData.GetComponent<ActorTurnSM>().OnMessage(TurnMessage.REVIVED_BY_ALLY, true);
	//					actorData.GetActorStatus().AddStatus(StatusType.ReviveRecovery, 0);
	//					return;
	//				}
	//			}
	//			else
	//			{
	//				Log.Warning(string.Concat(new object[]
	//				{
	//					"No ally found to revive at square ",
	//					x,
	//					", ",
	//					y
	//				}));
	//			}
	//		}
	//	}
	//}

	// rogues
	public virtual void PickRespawn(List<BoardSquare> availableSquares)
	{
		if (NetworkServer.active)
		{
			GetComponent<ActorData>().respawnSquares = availableSquares;
		}
	}

	// rogues
	public virtual void ProcessPickedRespawnRequest(int x, int y)
	{
		BoardSquare square = Board.Get().GetSquareFromIndex(x, y);
		ActorData actorData = GetComponent<ActorData>();
		SpawnPointManager spawnPointManager = SpawnPointManager.Get();
		if (spawnPointManager == null || !spawnPointManager.m_playersSelectRespawn)
		{
			return;
		}
		if (actorData.respawnSquares.Contains(square))
		{
			actorData.RespawnPickedPositionSquare = square;
		}
		else if (actorData.respawnSquares.Count > 0)
		{
			Log.Error($"Client for dead actor {actorData.DisplayName} requested an illegal respawn location {x}, {y}");
			actorData.RespawnPickedPositionSquare = actorData.respawnSquares[0];
		}
		else
		{
			Log.Error($"Client for dead actor {actorData.DisplayName} requested a respawn location when no locations are available. This is bad.");
			actorData.RespawnPickedPositionSquare = SpawnPointManager.Get().GetInitialSpawnSquare(actorData, new List<ActorData>());
		}
		List<BoardSquare> blocked = new List<BoardSquare>();
		foreach (GameObject player in GameFlowData.Get().GetPlayers())
		{
			ActorData otherActorData = player.GetComponent<ActorData>();
			if (otherActorData != actorData
			    && otherActorData != null
			    && otherActorData.IsDead()
			    && (otherActorData.GetTeam() == actorData.GetTeam()
			        || (otherActorData.TeamSensitiveData_authority != null
			            && otherActorData.TeamSensitiveData_authority.RespawnPickedSquare != null)))
			{
				blocked.Add(otherActorData.RespawnPickedPositionSquare);
			}
		}
		ActorData occupantActor = actorData.RespawnPickedPositionSquare.OccupantActor;
		if ((occupantActor != null && occupantActor.IsActorVisibleToActor(actorData))
		    || blocked.Contains(actorData.RespawnPickedPositionSquare))
		{
			HashSet<BoardSquare> squaresToAvoid = new HashSet<BoardSquare>();
			if (PowerUpManager.Get() != null)
			{
				PowerUpManager.Get().CollectSquaresToAvoidForRespawn(squaresToAvoid, actorData);
			}
			if (SpoilsManager.Get() != null)
			{
				SpoilsManager.Get().AddToSquaresToAvoidForRespawn(squaresToAvoid, actorData);
			}
			List<BoardSquare> fallbackSquares = new List<BoardSquare>();
			for (int i = 1; i <= 3; i++)
			{
				fallbackSquares.AddRange(AreaEffectUtils.GetSquaresInBorderLayer(actorData.RespawnPickedPositionSquare, i, false));
			}
			foreach (BoardSquare boardSquare in fallbackSquares)
			{
				if (boardSquare.IsValidForGameplay()
				    && (boardSquare.OccupantActor == null
				        || !boardSquare.OccupantActor.IsActorVisibleToActor(actorData))
				    && !blocked.Contains(boardSquare)
				    && !squaresToAvoid.Contains(boardSquare))
				{
					Log.Info("Adjusting respawn position during Decision to adjacent square to avoid spawning " +
					         "on a square that is already claimed by another visible actor or respawn.");
					actorData.RespawnPickedPositionSquare = boardSquare;
					break;
				}
			}
		}

		// rogues?
		//component.GetActorTurnSM().IncrementRespawnPickInput();

		GetComponent<ActorTurnSM>().OnMessage(TurnMessage.PICK_RESPAWN);
	}

	// rogues
	public virtual void RespawnOnSquare(BoardSquare spawnSquare)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		ActorData actorData = GetComponent<ActorData>();
		if (GameFlowData.Get().CurrentTurn == actorData.LastDeathTurn)
		{
			Log.Error("Code error: respawning same turn as death is not currently supported. It may look wrong on clients.");
		}
		actorData.ClearRespawnSquares();
		actorData.SetHitPoints(actorData.GetMaxHitPoints());
		actorData.UnresolvedDamage = 0;
		actorData.UnresolvedHealing = 0;
		if (!GameplayData.Get().m_keepTechPointsOnRespawn)
		{
			actorData.SetTechPoints(actorData.m_techPointsOnRespawn);
		}
		if (SpawnPointManager.Get() != null && SpawnPointManager.Get().m_spawnInDuringMovement)  // SpawnPointManager.Get().SpawnInDuringMovement() in rogues
		{
			actorData.IgnoreForAbilityHits = true;
		}
		actorData.ReservedTechPoints = 0;
		if (spawnSquare)
		{
			if (spawnSquare.occupant != null)
			{
				List<BoardSquare> fallbackSquares = new List<BoardSquare>();
				for (int i = 1; i <= 3; i++)
				{
					fallbackSquares.AddRange(AreaEffectUtils.GetSquaresInBorderLayer(spawnSquare, i, false));
				}
				HashSet<BoardSquare> squaresToAvoid = new HashSet<BoardSquare>();
				if (PowerUpManager.Get() != null)
				{
					PowerUpManager.Get().CollectSquaresToAvoidForRespawn(squaresToAvoid, actorData);
				}
				if (SpoilsManager.Get() != null)
				{
					SpoilsManager.Get().AddToSquaresToAvoidForRespawn(squaresToAvoid, actorData);
				}
				bool fallbackFound = false;
				foreach (BoardSquare boardSquare in fallbackSquares)
				{
					if (boardSquare.IsValidForGameplay()
					    && boardSquare.OccupantActor == null
					    && !squaresToAvoid.Contains(boardSquare))
					{
						Log.Info("Adjusting respawn position to adjacent square to avoid spawning on a square that is already occupied");
						spawnSquare = boardSquare;
						fallbackFound = true;
						break;
					}
				}
				if (!fallbackFound)
				{
					Debug.LogError("Debugging, trying to respawn on square that is already occupied");
				}
			}
			actorData.RespawnPickedPositionSquare = spawnSquare;
			actorData.TeleportToBoardSquare(spawnSquare, Vector3.zero, ActorData.TeleportType.Respawn, null);
		}
		else
		{
			Debug.LogError("Debugging, trying to respawn on Null square");
		}
		if (actorData == GameFlowData.Get().activeOwnedActorData)
		{
			CameraManager.Get().SetTargetObject(actorData.gameObject, CameraManager.CameraTargetReason.ClientActorRespawned);
		}
		if (actorData.GetPassiveData() != null)
		{
			actorData.GetPassiveData().OnActorRespawn();
		}
		if (PowerUpManager.Get() != null)
		{
			PowerUpManager.Get().ActorBecameAbleToCollectPowerups(actorData);
		}
		GetComponent<ActorTurnSM>().OnMessage(TurnMessage.RESPAWN);
		if (SpawnPointManager.Get() == null || !SpawnPointManager.Get().m_spawnInDuringMovement) // !SpawnPointManager.Get().SpawnInDuringMovement() in rogues
		{
			actorData.RespawnPickedPositionSquare = null;
		}
		if (NetworkClient.active
		    && spawnSquare != null
		    && FogOfWar.GetClientFog() != null
		    && actorData.GetActorVFX() != null)
		{
			actorData.OnRespawnTeleport();
			actorData.ForceUpdateIsVisibleToClientCache();
			PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
			if (localPlayerData != null
			    && (localPlayerData.GetTeamViewing() == actorData.GetTeam() || FogOfWar.GetClientFog().IsVisible(spawnSquare))
			    && SpawnPointManager.Get() != null
			    && SpawnPointManager.Get().m_spawnInDuringMovement)  // SpawnPointManager.Get().SpawnInDuringMovement() in rogues
			{
				actorData.GetActorModelData().DisableAndHideRenderers();
				if (HighlightUtils.Get().m_recentlySpawnedShader != null)
				{
					TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(
						actorData.GetActorModelData(),
						localPlayerData.GetTeamViewing() == actorData.GetTeam(),
						0.5f,
						HighlightUtils.Get().m_recentlySpawnedShader,
						false);
				}
			}
		}
	}

	// rogues
	public virtual void Respawn(HashSet<BoardSquare> squaresToAvoid)
	{
		if (NetworkServer.active)
		{
			ActorData actorData = GetComponent<ActorData>();
			BoardSquare spawnSquare = SpawnPointManager.Get().GetSpawnSquare(actorData, true, null, squaresToAvoid);
			RespawnOnSquare(spawnSquare);
		}
	}
#endif
}
