// ROGUES
// SERVER

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// server-only, missing in reactor
#if SERVER
public class ServerMovementStabilizer
{
	private int m_lastTurnStabilizedNPCs = -1;

	private bool HaveSetupNPCMovement => m_lastTurnStabilizedNPCs >= GameFlowData.Get().CurrentTurn;
	private NPCStabilizationMode CurrentStabilizationMode => HaveSetupNPCMovement ? NPCStabilizationMode.IncludeMinions : NPCStabilizationMode.SkipMinions;

	private void PrepMoversForStabilization(List<MovementRequest> storedMovementRequests)
	{
		foreach (MovementRequest movementRequest in storedMovementRequests)
		{
			if (movementRequest != null && movementRequest.m_actor != null)
			{
				movementRequest.m_actor.GetActorMovement().UpdateSquaresCanMoveTo();
				if (movementRequest.IsForcedChase())
				{
					movementRequest.m_actor.GetActorMovement().IgnoreCantSprintStatus = true;
				}
			}
		}
	}

	public void StabilizeMovement(List<MovementRequest> storedMovementRequests, bool stabilizeChasers)  // alsoStabilizeChasers in rogues
	{
		PrepMoversForStabilization(storedMovementRequests);
		bool pendingUpdate = true;
		while (pendingUpdate)
		{
			bool updated = false;
			if (!stabilizeChasers) // unconditional in rogues
			{
				updated = StabilizeMoversVsSnares(storedMovementRequests);
				if (!updated)
				{
					updated = StabilizeMoversVsObstacles(storedMovementRequests);
				}
				if (!updated)
				{
					updated = StabilizeMoversVsStationaries(storedMovementRequests);
				}
				if (!updated)
				{
					updated = StabilizeNormalMoversVsStationaryChasers(storedMovementRequests);
				}
				if (!updated)
				{
					updated = StabilizeMoversVsAfterImages(storedMovementRequests);
				}
				if (!updated)
				{
					updated = StabilizeMoversVsMovers(storedMovementRequests);
				}
			}
			if (stabilizeChasers)
			{
				if (!updated)
				{
					updated = StabilizeChasersVsInvisibles(storedMovementRequests);
				}
				if (!updated)
				{
					updated = StabilizeChasersVsStationaries(storedMovementRequests);
				}
				if (!updated)
				{
					updated = StabilizeChasersVsMovers(storedMovementRequests);
				}
				if (!updated)
				{
					updated = StabilizeChasersVsChasers(storedMovementRequests);
				}
			}
			pendingUpdate = updated;
		}
		if (stabilizeChasers)
		{
			SanitizeMovement(storedMovementRequests);
		}
	}

	private void SanitizeMovement(List<MovementRequest> storedMovementRequests)
	{
		List<MovementRequest> badRequests = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in storedMovementRequests)
		{
			if (movementRequest == null)
			{
				Log.Error("SanitizeMovement has null moveRequest.");
				badRequests.Add(movementRequest);
				continue;
			}
			if (movementRequest.m_targetSquare == null)
			{
				string text = "SanitizeMovement has a request with a null target square.";
				if (movementRequest.m_actor == null)
				{
					text += ", mover = null";
				}
				else
				{
					text += $", mover = {movementRequest.m_actor.DisplayName}";
				}
				if (movementRequest.m_chaseTarget != null)
				{
					text += $", chaseTarget = {movementRequest.m_chaseTarget.DisplayName}";
				}
				if (movementRequest.m_resolveState != MovementRequest.MovementResolveState.QUEUED)
				{
					text += $", resolveState = {movementRequest.m_resolveState.ToString()}";
				}
				Log.Error(text);
				badRequests.Add(movementRequest);
			}
			if (movementRequest.m_path != null)
			{
				movementRequest.m_path.CheckPathConnectionForSelfReference();
			}
		}
		foreach (MovementRequest movementRequest in badRequests)
		{
			movementRequest.m_actor.GetComponent<ActorTurnSM>().OnMessage(TurnMessage.MOVEMENT_RESOLVED);
			storedMovementRequests.Remove(movementRequest);
		}
	}

	public void AdjustMovementStartsForMoveAfterEvade(List<MovementRequest> storedMovementRequests)
	{
		foreach (MovementRequest movementRequest in storedMovementRequests)
		{
			if (movementRequest.IsChasing() || movementRequest.m_path == null)
			{
				continue;
			}
			BoardSquare currentBoardSquare = movementRequest.m_actor.GetCurrentBoardSquare();
			if (currentBoardSquare == null || currentBoardSquare == movementRequest.m_path.square)
			{
				continue;
			}
			BoardSquarePathInfo pathToRequestedStart = movementRequest.m_actor.GetActorMovement()
				.BuildPathTo(currentBoardSquare, movementRequest.m_path.square, 15f, true, null);
			if (pathToRequestedStart != null)
			{
				movementRequest.m_actor.GetActorMovement().MoveRangeCompensation = pathToRequestedStart.FindMoveCostToEnd();
				BoardSquarePathInfo endpoint = pathToRequestedStart.GetPathEndpoint();
				if (endpoint.prev != null)
				{
					endpoint = endpoint.prev;
				}
				endpoint.next = movementRequest.m_path;
				movementRequest.m_path.prev = endpoint;
				movementRequest.m_path = pathToRequestedStart;
				movementRequest.m_path.CalcAndSetMoveCostToEnd();
			}
			else
			{
				BoardSquarePathInfo newPath = new BoardSquarePathInfo
				{
					square = currentBoardSquare
				};
				newPath.CalcAndSetMoveCostToEnd();
				movementRequest.m_path = newPath;
				movementRequest.m_targetSquare = currentBoardSquare;
			}
		}
	}

	private static void BackUpPath(
		ActorData mover,
		BoardSquare squareToDepart,
		ref BoardSquarePathInfo path,
		out BoardSquare newDestination,
		out BoardSquarePathInfo lostPath,
		string caller)
	{
		lostPath = null;
		if (path == null && mover == null)
		{
			Log.Error($"Calling BackUpPath from {caller} with an invalid path and an invalid mover-actor.");
			newDestination = null;
			return;
		}
		if (path == null)
		{
			Log.Error($"Calling BackUpPath from {caller} with an invalid path (mover = {mover.DisplayName}).");
			newDestination = null;
			return;
		}
		if (mover == null)
		{
			Log.Error($"Calling BackUpPath from {caller} with an invalid mover-actor.");
			newDestination = null;
			return;
		}
		BoardSquarePathInfo endpoint = path.GetPathEndpoint();
		int stepsLost = 0;
		bool isBackingUp = true;
		float maxMovement = mover.GetActorMovement().CalculateMaxHorizontalMovement();
		while (endpoint.prev != null && isBackingUp)
		{
			if (lostPath == null)
			{
				lostPath = new BoardSquarePathInfo();
			}
			else
			{
				lostPath.prev = new BoardSquarePathInfo();
				lostPath.prev.next = lostPath;
				lostPath = lostPath.prev;
			}
			lostPath.square = endpoint.square;
			lostPath.m_unskippable = endpoint.m_unskippable;
			BoardSquarePathInfo prev = endpoint.prev;
			prev.next = null;
			endpoint.next = null;
			endpoint.prev = null;
			endpoint.square = null;
			endpoint = prev;
			stepsLost++;
			bool isTooLong = !path.IsValidPathForMaxMovement(maxMovement);
			bool isEndpointOccupied = squareToDepart != null && endpoint.square == squareToDepart;
			isBackingUp = isTooLong || isEndpointOccupied;
		}
		if (endpoint != null)
		{
			newDestination = endpoint.square;
		}
		else
		{
			newDestination = stepsLost == 0 ? path.square : null;
		}
	}

	private bool StabilizeMoversVsSnares(List<MovementRequest> storedMovementRequests)
	{
		bool flag = false;
		foreach (MovementRequest moveRequest in storedMovementRequests)
		{
			flag |= StabilizeMovementRequestVsSnares(moveRequest, out _);
		}
		return flag;
	}

	public bool StabilizeMovementRequestVsSnares(MovementRequest moveRequest, out BoardSquarePathInfo lostPath)
	{
		if (moveRequest.IsChasing() || moveRequest.m_path.WillDieAtEnd() || moveRequest.IsClashStabilized())
		{
			lostPath = null;
			return false;
		}
		
		bool result;
		ActorData actor = moveRequest.m_actor;
		float maxMovement = actor.GetActorMovement().CalculateMaxHorizontalMovement();
		if (!moveRequest.m_path.IsValidPathForMaxMovement(maxMovement))
		{
			float originalMoveCost = moveRequest.m_path.FindMoveCostToEnd();
			BackUpPath(
				actor,
				null,
				ref moveRequest.m_path,
				out BoardSquare newDestination,
				out lostPath,
				"StabilizeMoversVsSnares");
			float newMoveCost = moveRequest.m_path.FindMoveCostToEnd();
			if (originalMoveCost != newMoveCost)
			{
				moveRequest.m_targetSquare = newDestination;
				result = true;
				if (originalMoveCost > newMoveCost && actor.GetActorBehavior() != null)
				{
					actor.GetActorBehavior().TrackPathLostDuringStabilization(originalMoveCost - newMoveCost);
				}
			}
			else
			{
				Log.Error($"Failed to back up {moveRequest.m_actor} in StabilizeMoversVsSnares.");
				result = false;
			}
		}
		else
		{
			result = false;
			lostPath = null;
		}
		return result;
	}

	private bool StabilizeMoversVsObstacles(List<MovementRequest> storedMovementRequests)
	{
		bool flag = false;
		foreach (MovementRequest moveRequest in storedMovementRequests)
		{
			flag |= StabilizeMovementRequestVsObstacles(moveRequest, out _);
		}
		return flag;
	}

	public bool StabilizeMovementRequestVsObstacles(MovementRequest moveRequest, out BoardSquarePathInfo lostPath)
	{
		lostPath = null;
		if (moveRequest.IsChasing() || moveRequest.m_path.WillDieAtEnd() || moveRequest.IsClashStabilized())
		{
			return false;
		}
		
		bool result = false;
		ActorData actor = moveRequest.m_actor;
		BoardSquarePathInfo step = moveRequest.m_path;
		BoardSquarePathInfo next = moveRequest.m_path.next;
		BoardSquarePathInfo prev = moveRequest.m_path.prev;
		while (next != null)
		{
			if (!next.square.IsValidForGameplay()
			    || (prev != null
			        && BarrierManager.Get() != null
			        && BarrierManager.Get().IsMovementBlockedOnCrossover(actor, prev.square, step.square))
			    || (BarrierManager.Get() != null
			        && BarrierManager.Get().IsMovementBlocked(actor, step.square, next.square))
			    || (ServerEffectManager.Get() != null
			        && ServerEffectManager.Get().IsMovementBlockedOnEnterSquare(actor, prev?.square, step.square)))
			{
				lostPath = next;
				step.next = null;
				next.prev = null;
				next = null;
				moveRequest.m_targetSquare = step.square;
				result = true;
			}
			else
			{
				step = next;
				prev = next.prev;
				next = next.next;
			}
		}
		return result;
	}

	private bool StabilizeNormalMoversVsStationaryChasers(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		foreach (MovementRequest movementRequest in storedMovementRequests)
		{
			if (movementRequest.WasEverChasing()
			    || movementRequest.m_path.WillDieAtEnd()
			    || movementRequest.IsClashStabilized())
			{
				continue;
			}
			foreach (MovementRequest chaseRequest in storedMovementRequests)
			{
				if (!chaseRequest.WasEverChasing())
				{
					continue;
				}

				BoardSquare chaserSquare = chaseRequest.m_actor.GetCurrentBoardSquare();
				if (movementRequest.m_targetSquare == chaserSquare)
				{
					BackUpPath(
						movementRequest.m_actor,
						chaserSquare,
						ref movementRequest.m_path,
						out BoardSquare newDestination,
						out _,
						"StabilizeNormalMoversVsStationaryChasers");
					if (movementRequest.m_targetSquare != newDestination)
					{
						movementRequest.m_targetSquare = newDestination;
						result = true;
					}
					else
					{
						Log.Error($"Failed to back up {movementRequest.m_actor} in StabilizeMoversVsStationaries.");
					}
				}
			}
		}
		return result;
	}

	private bool StabilizeMoversVsStationaries(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		foreach (ActorData stationaryActor in ServerActionBuffer.Get().GetStationaryActors())
		{
			if (stationaryActor.IsDead())
			{
				continue;
			}

			foreach (MovementRequest movementRequest in storedMovementRequests)
			{
				if (movementRequest.IsChasing()
				    || movementRequest.m_path.WillDieAtEnd()
				    || movementRequest.IsClashStabilized())
				{
					continue;
				}
				Object targetSquare = movementRequest.m_targetSquare;
				BoardSquare stationaryActorSquare = stationaryActor.GetCurrentBoardSquare();
				if (targetSquare != stationaryActorSquare)
				{
					continue;
				}
				BackUpPath(
					movementRequest.m_actor,
					stationaryActorSquare,
					ref movementRequest.m_path,
					out BoardSquare newDestination,
					out _,
					"StabilizeMoversVsStationaries");
				if (movementRequest.m_targetSquare != newDestination)
				{
					movementRequest.m_targetSquare = newDestination;
					result = true;
				}
				else
				{
					Log.Error($"Failed to back up {movementRequest.m_actor} in StabilizeMoversVsStationaries.");
				}
			}
		}
		return result;
	}

	private bool StabilizeMoversVsAfterImages(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		List<ActorData> afterImageActors = new List<ActorData>();
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (!actorData.IsDead() && actorData.GetComponent<Passive_TricksterAfterImage>() != null)
			{
				afterImageActors.Add(actorData);
			}
		}
		foreach (ActorData afterImageActor in afterImageActors)
		{
			foreach (MovementRequest movementRequest in storedMovementRequests)
			{
				if (movementRequest.IsChasing()
				    || movementRequest.m_actor == afterImageActor
				    || movementRequest.m_path.WillDieAtEnd()
				    || movementRequest.IsClashStabilized())
				{
					continue;
				}
				Object targetSquare = movementRequest.m_targetSquare;
				BoardSquare afterImageSquare = afterImageActor.GetCurrentBoardSquare();
				if (targetSquare != afterImageSquare)
				{
					continue;
				}
				BackUpPath(
					movementRequest.m_actor,
					afterImageSquare,
					ref movementRequest.m_path,
					out BoardSquare newDestination,
					out _,
					"StabilizeMoversVsAfterImages");
				if (movementRequest.m_targetSquare != newDestination)
				{
					movementRequest.m_targetSquare = newDestination;
					result = true;
				}
				else
				{
					Log.Error($"Failed to back up {movementRequest.m_actor} in StabilizeMoversVsAfterImages.");
				}
			}
		}
		return result;
	}

	private bool StabilizeMoversVsMovers(List<MovementRequest> storedMovementRequests)
	{
		bool flag = false;
		storedMovementRequests.Sort();
		
		for (int i = 0; i < storedMovementRequests.Count; i++)
		{
			if (flag)
			{
				break;
			}
			MovementRequest movementRequest = storedMovementRequests[i];
			if (movementRequest.IsChasing() || movementRequest.m_path.WillDieAtEnd())
			{
				continue;
			}
			for (int j = i + 1; j < storedMovementRequests.Count; j++)
			{
				MovementRequest movementRequest2 = storedMovementRequests[j];
				if (movementRequest2.IsChasing()
				    || movementRequest2.m_path.WillDieAtEnd()
				    || movementRequest.m_targetSquare != movementRequest2.m_targetSquare
				    || (movementRequest.m_actor.GetTeam() != movementRequest2.m_actor.GetTeam()
				        && movementRequest.m_path.FindMoveCostToEnd() == movementRequest2.m_path.FindMoveCostToEnd()
				        && movementRequest.WasEverChasing() == movementRequest2.WasEverChasing())
				    || movementRequest2.IsClashStabilized())
				{
					continue;
				}
				BackUpPath(
					movementRequest2.m_actor,
					movementRequest.m_targetSquare,
					ref movementRequest2.m_path,
					out BoardSquare newDestination,
					out _,
					"StabilizeMoversVsMovers");
				if (movementRequest2.m_targetSquare != newDestination)
				{
					movementRequest2.m_targetSquare = newDestination;
					flag = true;
				}
				else
				{
					Log.Error($"Failed to back up {movementRequest2.m_actor} in StabilizeMoversVsMovers.");
				}
			}

		}
		return flag;
	}

	private bool StabilizeChasersVsInvisibles(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		foreach (MovementRequest movementRequest in storedMovementRequests)
		{
			ActorData actor = movementRequest.m_actor;
			ActorData chaseTarget = movementRequest.m_chaseTarget;
			if (movementRequest.IsChasing()
			    && !movementRequest.IsBeingDragged()
			    && chaseTarget.IsNeverVisibleTo(actor.PlayerData)
			    && !chaseTarget.IsAlwaysVisibleTo(actor.PlayerData))
			{
				ConvertChaseToDirectMovement(movementRequest, chaseTarget.ServerLastKnownPosSquare);
				result = true;
			}
		}
		return result;
	}

	private bool StabilizeChasersVsStationaries(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		List<ActorData> stationaryActors = ServerActionBuffer.Get().GetStationaryActors();
		foreach (MovementRequest movementRequest in storedMovementRequests)
		{
			ActorData chaseTarget = movementRequest.m_chaseTarget;
			if (!movementRequest.IsChasing() || !stationaryActors.Contains(chaseTarget))
			{
				continue;
			}
			ActorData actor = movementRequest.m_actor;
			ActorMovement actorMovement = movementRequest.m_actor.GetComponent<ActorMovement>();
			ActorBehavior chaseTargetBehavior = chaseTarget.GetActorBehavior();
			if (chaseTarget.IsDead()
			    || movementRequest.IsBeingDragged()
			    || chaseTarget.IsActorVisibleToActor(actor))
			{
				BoardSquare targetSquare = chaseTarget.IsDead() ? chaseTarget.GetMostRecentDeathSquare() : chaseTarget.GetCurrentBoardSquare();
				ConvertChaseToDirectMovement(movementRequest, targetSquare);
			}
			else
			{
				bool needToConvertPath = true;
				if (chaseTargetBehavior.CurrentTurn.Charged || chaseTargetBehavior.CurrentTurn.KnockedBack)
				{
					BoardSquarePathInfo path = chaseTargetBehavior.CurrentTurn.Path;
					if (path != null)
					{
						BoardSquarePathInfo lastSeenStep = FindChaserLastVisiblePartOfMoverPath(actor, chaseTarget, path);
						if (lastSeenStep != null)
						{
							needToConvertPath = false;
							BoardSquare lastSeenSquare = lastSeenStep.square;
							if (!actorMovement.CanMoveToBoardSquare(lastSeenSquare))
							{
								ConvertChaseToDirectMovement(movementRequest, lastSeenSquare);
							}
							else
							{
								movementRequest.m_path = actorMovement.BuildPathTo(actor.GetCurrentBoardSquare(), lastSeenSquare);
								BoardSquarePathInfo pathEndpoint = movementRequest.m_path.GetPathEndpoint();
								pathEndpoint.next = lastSeenStep.Clone(pathEndpoint);
								movementRequest.m_path.CalcAndSetMoveCostToEnd();
								BackUpPath(
									actor,
									null,
									ref movementRequest.m_path,
									out BoardSquare newDestination,
									out _,
									"StabilizeChasersVsStationaries");
								movementRequest.m_chaseTarget = null;
								movementRequest.m_targetSquare = newDestination;
							}
						}
					}
				}
				if (needToConvertPath)
				{
					ConvertChaseToDirectMovement(movementRequest, chaseTarget.ServerLastKnownPosSquare);
				}
			}
			result = true;
		}
		return result;
	}

	private void ConvertChaseToDirectMovement(MovementRequest chaseRequest, BoardSquare targetSquare)
	{
		if (chaseRequest == null)
		{
			Log.Warning("ConvertChaseToDirectMovement being called for a null chaseRequest");
			return;
		}
		ActorData actor = chaseRequest.m_actor;
		ActorMovement actorMovement = chaseRequest.m_actor.GetComponent<ActorMovement>();
		bool flag;
		List<BoardSquare> claimedSquares;
		if (targetSquare == null || !actorMovement.CanMoveToBoardSquare(targetSquare))
		{
			flag = true;
			claimedSquares = null;
		}
		else
		{
			flag = false;
			claimedSquares = ServerActionBuffer.Get().GetReservedSquares_PreChaseStabilization();
		}
		if (targetSquare == null)
		{
			Log.Error("ConvertChaseToDirectMovement being called for a null targetSquare");
			targetSquare = actor.GetCurrentBoardSquare();
		}
		else if (!targetSquare.IsValidForGameplay())
		{
			BoardSquare closestValidForGameplaySquareTo = Board.Get().GetClosestValidForGameplaySquareTo(targetSquare);
			Log.Error($"ConvertChaseToDirectMovement being called for invalid-for-gameplay targetSquare {targetSquare}.  " +
			          $"Going instead toward closest valid square, {closestValidForGameplaySquareTo}.  " +
			          $"Scene = {SceneManager.GetActiveScene().name}");
			targetSquare = closestValidForGameplaySquareTo;
		}
		BoardSquare currentBoardSquare = actor.GetCurrentBoardSquare();
		if (currentBoardSquare == null)
		{
			Log.Error(actor.DebugNameString() + " has null for current square when trying to convert chase movement to normal movement");
		}
		BoardSquarePathInfo boardSquarePathInfo = actorMovement.BuildCompletePathTo(actor.GetCurrentBoardSquare(), targetSquare, true, claimedSquares);
		if (boardSquarePathInfo == null)
		{
			Log.Error($"ConvertChaseToDirectMovement failed to build path to target, " +
			          $"from square {(currentBoardSquare != null ? currentBoardSquare.ToString() : "null")} " +
			          $"to square {(targetSquare != null ? targetSquare.ToString() : "null")}.  " +
			          $"Scene = {SceneManager.GetActiveScene().name}");
			if (currentBoardSquare != null)
			{
				boardSquarePathInfo = new BoardSquarePathInfo
				{
					square = currentBoardSquare
				};
				targetSquare = currentBoardSquare;
			}
			flag = false;
		}
		chaseRequest.m_path = boardSquarePathInfo;
		if (flag)
		{
			float pathCostBeforeBackup = chaseRequest.m_path.FindMoveCostToEnd();
			BackUpPath(
				actor,
				actor.GetCurrentBoardSquare(),
				ref chaseRequest.m_path,
				out BoardSquare newDestination,
				out _,
				"ConvertChaseToDirectMovement");
			targetSquare = newDestination;
			float pathCostAfterBackup = chaseRequest.m_path.FindMoveCostToEnd();
			TrackChaseMovementLostToSnare(actor, pathCostBeforeBackup, pathCostAfterBackup);
		}
		chaseRequest.m_chaseTarget = null;
		chaseRequest.m_targetSquare = targetSquare;
	}

	private void TrackChaseMovementLostToSnare(ActorData chaserActor, float pathCostBeforeBackup, float pathCostAfterBackup)
	{
		if (chaserActor.GetActorBehavior() == null)
		{
			return;
		}
		float movementRange = chaserActor.GetActorStats().GetModifiedStatInt(StatType.Movement_Horizontal);
		if (chaserActor.GetAbilityData() != null)
		{
			movementRange += chaserActor.GetAbilityData().GetQueuedAbilitiesMovementAdjust();
		}
		movementRange += chaserActor.GetActorMovement().MoveRangeCompensation;
		float adjustedMovementRange = movementRange;
		if (chaserActor.GetActorStatus().HasStatus(StatusType.Hasted))
		{
			ActorMovement.CalcHastedMovementAdjustments(out float mult, out _, out _);
			adjustedMovementRange *= mult;
		}
		float adjustedPathCostBeforeBackup = Mathf.Min(pathCostBeforeBackup, adjustedMovementRange);
		if (adjustedPathCostBeforeBackup > pathCostAfterBackup)
		{
			if (GameplayMetricHelper.GameplayMetricDebugTraceOn)
			{
				GameplayMetricHelper.DebugLogMoveDenied($"on converting chase to normal move, accounting for lost path. " +
				                                        $"Desired= {adjustedPathCostBeforeBackup} | " +
				                                        $"Actual= {pathCostAfterBackup} | " +
				                                        $"Diff= {adjustedPathCostBeforeBackup - pathCostAfterBackup}");
			}
			chaserActor.GetActorBehavior().TrackPathLostDuringStabilization(adjustedPathCostBeforeBackup - pathCostAfterBackup);
		}
	}

	private bool StabilizeChasersVsMovers(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		foreach (MovementRequest chaseRequest in storedMovementRequests)
		{
			if (!chaseRequest.IsChasing())
			{
				continue;
			}
			foreach (MovementRequest movementRequest in storedMovementRequests)
			{
				if (chaseRequest.m_chaseTarget != movementRequest.m_actor
				    || movementRequest.IsChasing())
				{
					continue;
				}
				BoardSquare targetSquare = movementRequest.m_targetSquare;
				ActorData chaser = chaseRequest.m_actor;
				FogOfWar chaserFogOfWar = chaser.GetComponent<FogOfWar>();
				ActorMovement chaserMovement = chaseRequest.m_actor.GetComponent<ActorMovement>();
				bool targetWillDieAtEnd = movementRequest.m_path.WillDieAtEnd();
				if (chaseRequest.IsBeingDragged()
				    || chaserFogOfWar.IsVisible(targetSquare)
				    || movementRequest.m_actor.GetActorStatus().HasStatus(StatusType.Revealed))
				{
					ConvertChaseToDirectMovement(chaseRequest, targetSquare);
					if (chaseRequest.m_targetSquare == targetSquare && !targetWillDieAtEnd)
					{
						chaseRequest.m_targetSquare = chaseRequest.m_path.BackUpOnceFromEnd().square;
					}
				}
				else
				{
					BoardSquarePathInfo path = movementRequest.m_path;
					if (path != null)
					{
						BoardSquarePathInfo lastSeenStep = FindChaserLastVisiblePartOfMoverPath(chaser, movementRequest.m_actor, path);
						if (lastSeenStep != null)
						{
							BoardSquare lastSeenSquare = lastSeenStep.square;
							if (!chaserMovement.CanMoveToBoardSquare(lastSeenSquare))
							{
								ConvertChaseToDirectMovement(chaseRequest, lastSeenSquare);
							}
							else
							{
								chaseRequest.m_path = chaserMovement.BuildPathTo(chaser.GetCurrentBoardSquare(), lastSeenSquare);
								BoardSquarePathInfo chaseEndpoint = chaseRequest.m_path.GetPathEndpoint();
								if (lastSeenStep.next != null)
								{
									BoardSquarePathInfo unseenStep = lastSeenStep.next.Clone(chaseEndpoint);
									for (BoardSquarePathInfo step = unseenStep; step != null; step = step.next)
									{
										step.m_moverBumpedFromClash = false;
										step.m_moverClashesHere = false;
										step.m_moverDiesHere = false;
										step.m_moverHasGameplayHitHere = false;
										step.m_visibleToEnemies = false;
										step.m_updateLastKnownPos = false;
									}
									chaseEndpoint.next = unseenStep;
								}
								chaseRequest.m_path.CalcAndSetMoveCostToEnd();
								BoardSquare newDestination;
								if (!targetWillDieAtEnd)
								{
									BackUpPath(chaser, null, ref chaseRequest.m_path, out newDestination, out _, "StabilizeChasersVsMovers");
								}
								else
								{
									newDestination = chaseRequest.m_path.GetPathEndpoint().square;
								}
								chaseRequest.m_chaseTarget = null;
								chaseRequest.m_targetSquare = newDestination;
							}
						}
					}
				}
				if (chaseRequest.IsChasing())
				{
					Log.Warning($"StabilizeChasersVsMovers failed to stabilize chaser {chaser.DisplayName}'s pursuit " +
					            $"of target {movementRequest.m_actor.DisplayName}.  " + 
					            "Going to target's square at turn start...");
					BoardSquare squareAtTurnStart = movementRequest.m_actor.GetActorBehavior().CurrentTurn.Square;
					ConvertChaseToDirectMovement(chaseRequest, squareAtTurnStart);
				}
				result = true;
				break;
			}
		}
		return result;
	}

	private bool StabilizeChasersVsChasers(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		foreach (MovementRequest chaseRequest in storedMovementRequests)
		{
			if (!chaseRequest.IsChasing())
			{
				continue;
			}
			foreach (MovementRequest movementRequest in storedMovementRequests)
			{
				if (chaseRequest.m_chaseTarget != movementRequest.m_actor
				    || !movementRequest.IsChasing())
				{
					continue;
				}
				bool flag = false;
				ActorMovement chaserMovement = chaseRequest.m_actor.GetComponent<ActorMovement>();
				ActorData chaser = chaseRequest.m_actor;
				ActorMovement targetMovement = movementRequest.m_actor.GetComponent<ActorMovement>();
				ActorData targetActor = movementRequest.m_actor;
				BoardSquarePathInfo pathToCurrentTargetPos = chaserMovement.BuildPathTo(chaser.GetCurrentBoardSquare(), targetActor.GetCurrentBoardSquare());
				if (pathToCurrentTargetPos != null && movementRequest.m_chaseTarget == chaseRequest.m_actor)
				{
					BoardSquarePathInfo pathMidpoint = pathToCurrentTargetPos.GetPathMidpoint();
					if (targetMovement.CanMoveToBoardSquare(pathMidpoint.square))
					{
						pathMidpoint.next = null;
						chaseRequest.m_path = pathToCurrentTargetPos;
						chaseRequest.m_targetSquare = pathMidpoint.square;
						chaseRequest.m_chaseTarget = null;
						movementRequest.m_path = targetMovement.BuildPathTo(targetActor.GetCurrentBoardSquare(), pathMidpoint.square);
						movementRequest.m_targetSquare = pathMidpoint.square;
						movementRequest.m_chaseTarget = null;
						result = true;
					}
					else
					{
						BoardSquarePathInfo pathFromTargetCurrentPos = targetMovement.BuildPathTo(targetActor.GetCurrentBoardSquare(), chaser.GetCurrentBoardSquare());
						if (pathFromTargetCurrentPos != null)
						{
							BoardSquarePathInfo pathMidpoint2 = pathFromTargetCurrentPos.GetPathMidpoint();
							if (chaserMovement.CanMoveToBoardSquare(pathMidpoint2.square))
							{
								pathMidpoint2.next = null;
								movementRequest.m_path = pathFromTargetCurrentPos;
								movementRequest.m_targetSquare = pathMidpoint2.square;
								movementRequest.m_chaseTarget = null;
								chaseRequest.m_path = chaserMovement.BuildPathTo(chaser.GetCurrentBoardSquare(), pathMidpoint2.square);
								chaseRequest.m_targetSquare = pathMidpoint2.square;
								chaseRequest.m_chaseTarget = null;
								result = true;
							}
							else
							{
								flag = true;
							}
						}
						else
						{
							flag = true;
						}
					}
				}
				else
				{
					flag = true;
				}
				if (flag && pathToCurrentTargetPos == null)
				{
					BoardSquare targetCurrentBoardSquare = targetActor.GetCurrentBoardSquare();
					BoardSquare closestMoveableSquareTo = chaserMovement.GetClosestMoveableSquareTo(targetCurrentBoardSquare);
					chaseRequest.m_path = chaserMovement.BuildPathTo(chaser.GetCurrentBoardSquare(), closestMoveableSquareTo);
					chaseRequest.m_chaseTarget = null;
					chaseRequest.m_targetSquare = closestMoveableSquareTo;
					result = true;
				}
				else if (flag)
				{
					chaseRequest.m_path = pathToCurrentTargetPos;
					chaseRequest.m_chaseTarget = null;
					chaseRequest.m_targetSquare = movementRequest.m_actor.GetCurrentBoardSquare();
					result = true;
				}
			}
		}
		return result;
	}

	private static BoardSquarePathInfo FindChaserLastVisiblePartOfMoverPath(ActorData chaser, ActorData mover, BoardSquarePathInfo moverPath)
	{
		FogOfWar chaserFogOfWar = chaser.GetComponent<FogOfWar>();
		
		BoardSquarePathInfo lastSeenStep = null;
		bool isStepTargetVisible = false;
		for (BoardSquarePathInfo step = moverPath; step != null && step.square != null; step = step.next)
		{
			if (step.square.IsValidForGameplay() && chaserFogOfWar.IsVisible(step.square))
			{
				lastSeenStep = step;
				isStepTargetVisible = true;
			}
			else if (!step.square.IsValidForGameplay() && step.prev != null && step.prev == lastSeenStep)
			{
				lastSeenStep = step;
				isStepTargetVisible = false;
			}
			else if (step.prev != null && !step.prev.square.IsValidForGameplay() && step.prev == lastSeenStep)
			{
				lastSeenStep = step;
				isStepTargetVisible = false;
			}
		}
		if (lastSeenStep != null && isStepTargetVisible && lastSeenStep.next != null)
		{
			lastSeenStep = lastSeenStep.next;
		}
		return lastSeenStep;
	}

	public void ModifyPathForMaxMovement(ActorData mover, BoardSquarePathInfo path, bool modifyAsIfSnared)
	{
		if (mover == null || path == null)
		{
			return;
		}
		int num = 0;
		bool canBackUp = true;
		float maxMovement = mover.GetActorMovement().CalculateMaxHorizontalMovement(false, modifyAsIfSnared);
		while (!path.IsValidPathForMaxMovement(maxMovement) && canBackUp && num < 100)
		{
			float oldCost = path.FindMoveCostToEnd();
			BackUpPath(mover, null, ref path, out _, out _, "ModifyPathForMaxMovement");
			float newCost = path.FindMoveCostToEnd();
			if (oldCost == newCost)
			{
				canBackUp = false;
			}
			num++;
		}
		if (num >= 100)
		{
			Log.Error("ModifyPathForMaxMovement did not finish within 100 iterations");
		}
	}

	public void RemoveStationaryMovementRequests(ref List<MovementRequest> movementRequests)
	{
		List<MovementRequest> badRequests = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in movementRequests)
		{
			if (movementRequest.m_targetSquare == movementRequest.m_actor.GetCurrentBoardSquare() && movementRequest.m_path.next == null)
			{
				badRequests.Add(movementRequest);
			}
		}
		foreach (MovementRequest item in badRequests)
		{
			movementRequests.Remove(item);
		}
	}

	private enum NPCStabilizationMode
	{
		SkipMinions,
		IncludeMinions
	}
}
#endif
