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
	// TODO there is no forceDelayExecution in reactor
	public virtual void ProcessSetSquareRequest(int x, int y, bool setWaypoint, bool forceDelayExecution = false)
	{
		ActorData component = GetComponent<ActorData>();
		ActorMovement component2 = GetComponent<ActorMovement>();
		ActorTurnSM component3 = GetComponent<ActorTurnSM>();
		AbilityData component4 = GetComponent<AbilityData>();
		GameFlowData gameFlowData = GameFlowData.Get();
		ServerActionBuffer serverActionBuffer = ServerActionBuffer.Get();
		Board board = Board.Get();
		bool flag = false;
		if (component && component2 && component3 && component4 && gameFlowData && serverActionBuffer && board)
		{
			bool flag2 = component3.AmDecidingMovement();
			bool flag3 = gameFlowData.IsInDecisionState();
			// rogues
			//bool flag4 = ServerActionBuffer.Get().GetPlayerActionFSM().IsAcceptingInput();
			//bool flag5 = PlayerActionStateMachine.ExecutePlayerActionImmediately() && !forceDelayExecution && flag4;
			// custom random
			bool flag5 = false;

			if (flag2 && flag3)
			{
				if (ServerActionBuffer.Get().HasPendingMovementRequest(component) && !flag5)
				{
					if (setWaypoint && FirstTurnMovement.CanWaypoint())
					{
						if (component.CanMoveToBoardSquare(x, y))
						{
							ServerActionBuffer.Get().AppendToMovementRequest(x, y, component);
							flag = true;
						}
					}
					else
					{
						float maxMovement = component.GetActorMovement().CalculateMaxHorizontalMovement(false, false);
						BoardSquare initialMoveStartSquare = component.InitialMoveStartSquare;
						BoardSquare squareFromIndex = Board.Get().GetSquareFromIndex(x, y);
						BoardSquarePathInfo boardSquarePathInfo = component.GetActorMovement().BuildCompletePathTo(initialMoveStartSquare, squareFromIndex, false, null);
						if (boardSquarePathInfo != null && boardSquarePathInfo.IsValidPathForMaxMovement(maxMovement))
						{
							ServerActionBuffer.Get().CancelMovementRequests(component, false);
							ServerActionBuffer.Get().StoreMovementRequest(x, y, component, boardSquarePathInfo);
							flag = true;
						}
					}
				}
				else if (component.CanMoveToBoardSquare(x, y))
				{
					if (ServerActionBuffer.Get().HasPendingMovementRequest(component))
					{
						ServerActionBuffer.Get().CancelMovementRequests(component, false);
					}
					BoardSquare initialMoveStartSquare2 = component.InitialMoveStartSquare;
					BoardSquare squareFromIndex2 = Board.Get().GetSquareFromIndex(x, y);
					if (squareFromIndex2.OccupantActor == null || !squareFromIndex2.OccupantActor.IsActorVisibleToActor(component) || !flag5)
					{
						BoardSquarePathInfo boardSquarePathInfo2 = component.GetActorMovement().BuildCompletePathTo(initialMoveStartSquare2, squareFromIndex2, false, null);
						if (boardSquarePathInfo2 != null)
						{
							ServerActionBuffer.Get().StoreMovementRequest(squareFromIndex2.x, squareFromIndex2.y, component, boardSquarePathInfo2);

							// rogues
							//if (flag5)
							//{
							//	ServerActionBuffer.Get().GetPlayerActionFSM().RunQueuedActionsFromActor(component);
							//}
						}
						flag = true;
					}
				}
			}
		}
		if (flag)
		{
			component3.OnMessage(TurnMessage.MOVEMENT_ACCEPTED, true);
			return;
		}
		component3.OnMessage(TurnMessage.MOVEMENT_REJECTED, true);
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

	internal virtual void ProcessSelectAbilityRequest()
	{
		ActorData actorData = base.GetComponent<ActorData>();
		ActorTurnSM actorTurnSM = base.GetComponent<ActorTurnSM>();
		if (actorData && actorTurnSM && actorTurnSM.CanSelectAbility())
		{
			BoardSquare boardSquare = null;
			AbilityData abilityData = base.GetComponent<AbilityData>();
			if (abilityData != null)
			{
				boardSquare = abilityData.GetAutoSelectTarget();
			}
			bool isAutoSelect = boardSquare != null;
			if (SinglePlayerManager.Get())
			{
				SinglePlayerManager.Get().OnActorAbilitySelected(actorData);
			}
			if (!isAutoSelect)
			{
				actorTurnSM.OnMessage(TurnMessage.SELECTED_ABILITY, true);
				return;
			}
			List<AbilityTarget> targets = AbilityTarget.AbilityTargetList(AbilityTarget.CreateAbilityTargetFromBoardSquare(boardSquare, actorData.GetFreePos()));
			this.ProcessCastAbilityRequest(targets, abilityData.GetSelectedActionType(), false);
		}
	}

	public virtual void ProcessQueueSimpleActionRequest(AbilityData.ActionType actionType, bool forceDelayExecution)
	{
		ActorData component = base.GetComponent<ActorData>();
		ActorTurnSM actorTurnSM = component.GetActorTurnSM();
		if (component && actorTurnSM && actorTurnSM.CanQueueSimpleAction())
		{
			this.ProcessCastSimpleActionRequest(actionType, forceDelayExecution);
		}
		if (SinglePlayerManager.Get())
		{
			SinglePlayerManager.Get().OnActorAbilitySelected(component);
		}
	}

	public virtual void ProcessCancelAbilitySelection()
	{
		ActorData actorData = base.GetComponent<ActorData>();
		ActorTurnSM component = base.GetComponent<ActorTurnSM>();
		if (actorData && component && component.CurrentState == TurnStateEnum.TARGETING_ACTION)
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

            base.GetComponent<AbilityData>().ClearSelectedAbility();
			if (ability)
			{
				ability.ClearAbilityMod(actorData);
			}
			this.CheckAndCancelMovementRequestBeyondRange(actorData, false);
		}
	}

	internal virtual void ProcessCastAbilityRequest(List<AbilityTarget> targets, AbilityData.ActionType actionType, bool forceDelayExecution)
	{
		ActorData actorData = base.GetComponent<ActorData>();
		ActorTurnSM actorTurnSM = base.GetComponent<ActorTurnSM>();
		bool isAutoSelect = false;
		AbilityData abilityData = base.GetComponent<AbilityData>();
		if (abilityData && abilityData.GetAutoSelectTarget() != null)
		{
			isAutoSelect = true;
		}
		Ability ability = abilityData.GetAbilityOfActionType(actionType);
		if (ability && ability.IsSimpleAction())
		{
			Log.Error($"Ability {ability.m_abilityName} is trying to be cast as a non-simple ability, but it's simple.");
		}
		if (actionType >= AbilityData.ActionType.CARD_0
			&& actionType <= AbilityData.ActionType.CARD_2
			&& ability
			&& ability.m_actionAnimType != ActorModelData.ActionAnimationType.None)
		{
			Log.Warning("Setting Animation Type to None for card ability");
			ability.m_actionAnimType = ActorModelData.ActionAnimationType.None;
		}
		if (actorData && actorTurnSM)
		{
			bool isValidState = actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION
				|| (actorTurnSM.CurrentState == TurnStateEnum.DECIDING && isAutoSelect)
				|| (actorTurnSM.CurrentState == TurnStateEnum.DECIDING && GameplayUtils.IsBot(this));

			// custom
			Log.Info($"ProcessCastAbilityRequest isValidState {isValidState}");

			//rogues
			//bool flag3 = ServerActionBuffer.Get().GetPlayerActionFSM().IsAcceptingInput();
			// custom random
			bool flag3 = true;

			bool isAccepted = false;
			if (isValidState && abilityData.ValidateActionRequest(actionType, targets))
			{
				this.StoreAbilityRequest_FCFS(ability, actionType, targets, actorData, forceDelayExecution || !flag3);
				isAccepted = true;
			}
			if (!isAccepted)
			{
				actorTurnSM.OnMessage(TurnMessage.ABILITY_REQUEST_REJECTED, true);
			}
		}
	}

	public virtual void ProcessCastSimpleActionRequest(AbilityData.ActionType actionType, bool forceDelayExecution)
	{
		ActorData component = base.GetComponent<ActorData>();
		ActorTurnSM component2 = base.GetComponent<ActorTurnSM>();
		AbilityData component3 = base.GetComponent<AbilityData>();
		if (component == null || component2 == null || component3 == null)
		{
			Log.Warning(string.Format("ProcessCastSimpleActionRequest being called with action {0}, but the actor is missing a critial component.", actionType.ToString()));
			return;
		}
		Ability abilityOfActionType = component3.GetAbilityOfActionType(actionType);
		if (abilityOfActionType == null)
		{
			Log.Warning(string.Format("ProcessCastSimpleActionRequest being called with action {0}, but the actor has no ability for that action.  (Actor = {1})", actionType.ToString(), component.DebugNameString()));
			return;
		}
		if (!abilityOfActionType.IsSimpleAction())
		{
			Log.Error(string.Format("Ability {0} is trying to be cast as a simple action, but isn't a simple action.", abilityOfActionType.m_abilityName));
		}
		if (actionType >= AbilityData.ActionType.CARD_0 && actionType <= AbilityData.ActionType.CARD_2 && abilityOfActionType && abilityOfActionType.m_actionAnimType != ActorModelData.ActionAnimationType.None)
		{
			Log.Warning("Setting Animation Type to None for card ability");
			abilityOfActionType.m_actionAnimType = ActorModelData.ActionAnimationType.None;
		}
		if (component && component2)
		{
			List<AbilityTarget> targets = AbilityTarget.AbilityTargetList(abilityOfActionType.CreateAbilityTargetForSimpleAction(component));
			bool flag = component2.CanQueueSimpleAction();
			bool flag2 = component3.ValidateActionRequest(actionType, targets);

			// rogues
			//bool flag3 = ServerActionBuffer.Get().GetPlayerActionFSM().IsAcceptingInput();
			// custom random
			bool flag3 = true;

			if (flag && flag2)
			{
				this.StoreAbilityRequest_FCFS(abilityOfActionType, actionType, targets, component, forceDelayExecution || !flag3);
			}
		}
	}

	public virtual void ProcessCancelActionRequest(AbilityData.ActionType actionType, bool hasPendingStoreRequest)
	{
		ActorData component = base.GetComponent<ActorData>();
		ActorTurnSM component2 = base.GetComponent<ActorTurnSM>();
		AbilityData abilityData = base.GetComponent<AbilityData>();
		Ability abilityOfActionType = abilityData.GetAbilityOfActionType(actionType);
		if (component && component2 && abilityOfActionType && (!abilityOfActionType.ShouldAutoQueueIfValid() || abilityOfActionType.AllowCancelWhenAutoQueued()))
		{
			// rogues?
			//if (component2.m_tauntRequestedForNextAbility == (int)actionType)
			//{
			//	CharacterTaunt characterTaunt = component.GetCharacterResourceLink().m_taunts.Find((CharacterTaunt t) => t.m_actionForTaunt == abilityData.GetSelectedActionType());
			//	if (characterTaunt != null && characterTaunt.m_modToApplyOnTaunt >= 0)
			//	{
			//		abilityOfActionType.ClearAbilityMod(component);
			//	}
			//}

			ServerActionBuffer.Get().CancelAbilityRequest(component, abilityOfActionType, true, false);
			BoardSquare initialMoveStartSquare = component.InitialMoveStartSquare;
			if (component.GetServerMoveRequestStartSquare() != initialMoveStartSquare)
			{
				component.InitialMoveStartSquare = component.GetServerMoveRequestStartSquare();
				ServerActionBuffer.Get().CancelMovementRequests(component, false);
			}
			if (abilityOfActionType.GetStatusToApplyWhenRequested().Count > 0)
			{
				component.GetActorMovement().UpdateSquaresCanMoveTo();
				if (NetworkServer.active && component.GetActorController() != null)
				{
					component.GetActorController().CallRpcUpdateRemainingMovement(component.RemainingHorizontalMovement, component.RemainingMovementWithQueuedAbility);
				}
				this.CheckAndCancelMovementRequestBeyondRange(component, hasPendingStoreRequest);
			}
		}
	}

	private void StoreAbilityRequest_FCFS(Ability ability, AbilityData.ActionType actionType, List<AbilityTarget> targets, ActorData actorData, bool forceDelayExecution)
	{
		ServerActionBuffer.Get().StoreAbilityRequest(ability, actionType, targets, actorData, null, null, false);
		actorData.GetActorTurnSM().OnMessage(TurnMessage.ABILITY_REQUEST_ACCEPTED, (int)actionType, true);

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
		this.CheckAndCancelMovementRequestBeyondRange(actorData, false);
	}

	public virtual void CancelActionRequestsForTurnStart()
	{
		ActorData component = base.GetComponent<ActorData>();
		ServerActionBuffer.Get().CancelActionRequests(component, true);
		AbilityData component2 = base.GetComponent<AbilityData>();
		if (component2)
		{
			component2.ClearSelectedAbility();
		}
	}

	protected virtual void CheckAndCancelMovementRequestBeyondRange(ActorData actor, bool hasPendingStoreRequest)
	{
		// rogues
		//if (PlayerActionStateMachine.ExecutePlayerActionImmediately())
		//{
		//	return;
		//}

		float maxMovement = actor.GetActorMovement().CalculateMaxHorizontalMovement(false, false);
		if (ServerActionBuffer.Get().HasNormalMovementRequestOutsideOfRange(actor, maxMovement))
		{
			ServerActionBuffer.Get().CancelMovementRequests(actor, false);
			return;
		}
		if (ServerActionBuffer.Get().HasPendingMovementRequest(actor) && actor.QueuedMovementAllowsAbility && !hasPendingStoreRequest)
		{
			BoardSquare item;
			float num;
			bool flag;
			ServerActionBuffer.Get().GatherMovementInfo(actor, out item, out num, out flag);
			if (!flag && !actor.GetActorMovement().SquaresCanMoveToWithQueuedAbility.Contains(item))
			{
				actor.QueuedMovementAllowsAbility = false;
			}
		}
	}

	public virtual void ProcessCancelMovementRequests()
	{
		// rogues
		//if (ServerActionBuffer.Get().GetPlayerActionFSM().IsAcceptingInput())
		//{
			ActorData component = base.GetComponent<ActorData>();
			ServerActionBuffer.Get().CancelMovementRequests(component, false);
		//	return;
		//}
		//PveLog.DebugLog("cancel_movement_request ignored, not accepting player input", null);
	}

	public virtual void DebugTeleport(BoardSquare destinationSquare)
	{
		if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("AllowTeleport") && destinationSquare != null && destinationSquare.IsValidForGameplay())
		{
			ActorData component = base.GetComponent<ActorData>();
			if (component != null)
			{
				component.TeleportToBoardSquare(destinationSquare, Vector3.zero, ActorData.TeleportType.Debug, null, 20f, ActorData.MovementType.Teleport, GameEventManager.EventType.Invalid, null);
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

	public virtual void PickRespawn(List<BoardSquare> availableSquares)
	{
		if (NetworkServer.active)
		{
			base.GetComponent<ActorData>().respawnSquares = availableSquares;
		}
	}

	public virtual void ProcessPickedRespawnRequest(int x, int y)
	{
		BoardSquare squareFromIndex = Board.Get().GetSquareFromIndex(x, y);
		ActorData component = base.GetComponent<ActorData>();
		SpawnPointManager spawnPointManager = SpawnPointManager.Get();
		if (spawnPointManager != null && spawnPointManager.m_playersSelectRespawn)
		{
			if (component.respawnSquares.Contains(squareFromIndex))
			{
				component.RespawnPickedPositionSquare = squareFromIndex;
			}
			else if (component.respawnSquares.Count > 0)
			{
				Log.Error("Client for dead actor {0} requested an illegal respawn location {1}, {2}", new object[]
				{
					component.DisplayName,
					x,
					y
				});
				component.RespawnPickedPositionSquare = component.respawnSquares[0];
			}
			else
			{
				Log.Error("Client for dead actor {0} requested a respawn location when no locations are available. This is bad.", new object[]
				{
					component.DisplayName
				});
				component.RespawnPickedPositionSquare = SpawnPointManager.Get().GetInitialSpawnSquare(component, new List<ActorData>());
			}
			List<BoardSquare> list = new List<BoardSquare>();
			foreach (GameObject gameObject in GameFlowData.Get().GetPlayers())
			{
				ActorData component2 = gameObject.GetComponent<ActorData>();
				if (component2 != component && component2 != null && component2.IsDead() && (component2.GetTeam() == component.GetTeam() || (component2.TeamSensitiveData_authority != null && component2.TeamSensitiveData_authority.RespawnPickedSquare != null)))
				{
					list.Add(component2.RespawnPickedPositionSquare);
				}
			}
			ActorData occupantActor = component.RespawnPickedPositionSquare.OccupantActor;
			if ((occupantActor != null && occupantActor.IsActorVisibleToActor(component)) || list.Contains(component.RespawnPickedPositionSquare))
			{
				HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
				if (PowerUpManager.Get() != null)
				{
					PowerUpManager.Get().CollectSquaresToAvoidForRespawn(hashSet, component);
				}
				if (SpoilsManager.Get() != null)
				{
					SpoilsManager.Get().AddToSquaresToAvoidForRespawn(hashSet, component);
				}
				List<BoardSquare> list2 = new List<BoardSquare>();
				for (int i = 1; i <= 3; i++)
				{
					list2.AddRange(AreaEffectUtils.GetSquaresInBorderLayer(component.RespawnPickedPositionSquare, i, false));
				}
				foreach (BoardSquare boardSquare in list2)
				{
					if (boardSquare.IsValidForGameplay() && (boardSquare.OccupantActor == null || !boardSquare.OccupantActor.IsActorVisibleToActor(component)) && !list.Contains(boardSquare) && !hashSet.Contains(boardSquare))
					{
						Log.Info("Adjusting respawn position during Decision to adjacent square to avoid spawning on a square that is already claimed by another visible actor or respawn.");
						component.RespawnPickedPositionSquare = boardSquare;
						break;
					}
				}
			}

			// rogues?
			//component.GetActorTurnSM().IncrementRespawnPickInput();

			base.GetComponent<ActorTurnSM>().OnMessage(TurnMessage.PICK_RESPAWN, true);
		}
	}

	public virtual void RespawnOnSquare(BoardSquare spawnSquare)
	{
		if (NetworkServer.active)
		{
			ActorData component = base.GetComponent<ActorData>();
			if (GameFlowData.Get().CurrentTurn == component.LastDeathTurn)
			{
				Log.Error("Code error: respawning same turn as death is not currently supported. It may look wrong on clients.");
			}
			component.ClearRespawnSquares();
			component.SetHitPoints(component.GetMaxHitPoints());
			component.UnresolvedDamage = 0;
			component.UnresolvedHealing = 0;
			if (!GameplayData.Get().m_keepTechPointsOnRespawn)
			{
				component.SetTechPoints(component.m_techPointsOnRespawn, false, null, null);
			}
			if (SpawnPointManager.Get() != null && SpawnPointManager.Get().m_spawnInDuringMovement)  // SpawnPointManager.Get().SpawnInDuringMovement() in rogues
			{
				component.IgnoreForAbilityHits = true;
			}
			component.ReservedTechPoints = 0;
			if (spawnSquare)
			{
				if (spawnSquare.occupant != null)
				{
					List<BoardSquare> list = new List<BoardSquare>();
					for (int i = 1; i <= 3; i++)
					{
						list.AddRange(AreaEffectUtils.GetSquaresInBorderLayer(spawnSquare, i, false));
					}
					HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
					if (PowerUpManager.Get() != null)
					{
						PowerUpManager.Get().CollectSquaresToAvoidForRespawn(hashSet, component);
					}
					if (SpoilsManager.Get() != null)
					{
						SpoilsManager.Get().AddToSquaresToAvoidForRespawn(hashSet, component);
					}
					bool flag = false;
					foreach (BoardSquare boardSquare in list)
					{
						if (boardSquare.IsValidForGameplay() && boardSquare.OccupantActor == null && !hashSet.Contains(boardSquare))
						{
							Log.Info("Adjusting respawn position to adjacent square to avoid spawning on a square that is already occupied");
							spawnSquare = boardSquare;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Debug.LogError("Debugging, trying to respawn on square that is already occupied");
					}
				}
				component.RespawnPickedPositionSquare = spawnSquare;
				component.TeleportToBoardSquare(spawnSquare, Vector3.zero, ActorData.TeleportType.Respawn, null, 20f, ActorData.MovementType.Teleport, GameEventManager.EventType.Invalid, null);
			}
			else
			{
				Debug.LogError("Debugging, trying to respawn on Null square");
			}
			if (component == GameFlowData.Get().activeOwnedActorData)
			{
				CameraManager.Get().SetTargetObject(component.gameObject, CameraManager.CameraTargetReason.ClientActorRespawned);
			}
			if (component.GetPassiveData() != null)
			{
				component.GetPassiveData().OnActorRespawn();
			}
			if (PowerUpManager.Get() != null)
			{
				PowerUpManager.Get().ActorBecameAbleToCollectPowerups(component);
			}
			base.GetComponent<ActorTurnSM>().OnMessage(TurnMessage.RESPAWN, true);
			if (SpawnPointManager.Get() == null || !SpawnPointManager.Get().m_spawnInDuringMovement) // !SpawnPointManager.Get().SpawnInDuringMovement() in rogues
			{
				component.RespawnPickedPositionSquare = null;
			}
			if (NetworkClient.active && spawnSquare != null && FogOfWar.GetClientFog() != null && component.GetActorVFX() != null)
			{
				component.OnRespawnTeleport();
				component.ForceUpdateIsVisibleToClientCache();
				PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
				if (localPlayerData != null
					&& (localPlayerData.GetTeamViewing() == component.GetTeam() || FogOfWar.GetClientFog().IsVisible(spawnSquare))
					&& SpawnPointManager.Get() != null
					&& SpawnPointManager.Get().m_spawnInDuringMovement)  // SpawnPointManager.Get().SpawnInDuringMovement() in rogues
				{
					component.GetActorModelData().DisableAndHideRenderers();
					if (HighlightUtils.Get().m_recentlySpawnedShader != null)
					{
						TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(component.GetActorModelData(), localPlayerData.GetTeamViewing() == component.GetTeam(), 0.5f, HighlightUtils.Get().m_recentlySpawnedShader, false);
					}
				}
			}
		}
	}

	public virtual void Respawn(HashSet<BoardSquare> squaresToAvoid)
	{
		if (NetworkServer.active)
		{
			ActorData component = base.GetComponent<ActorData>();
			BoardSquare spawnSquare = SpawnPointManager.Get().GetSpawnSquare(component, true, null, squaresToAvoid);
			this.RespawnOnSquare(spawnSquare);
		}
	}
#endif
}
