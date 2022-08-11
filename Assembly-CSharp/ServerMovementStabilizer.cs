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

	private bool HaveSetupNPCMovement
	{
		get
		{
			int currentTurn = GameFlowData.Get().CurrentTurn;
			return m_lastTurnStabilizedNPCs >= currentTurn;
		}
	}

	private ServerMovementStabilizer.NPCStabilizationMode CurrentStabilizationMode
	{
		get
		{
			if (HaveSetupNPCMovement)
			{
				return NPCStabilizationMode.IncludeMinions;
			}
			return NPCStabilizationMode.SkipMinions;
		}
	}

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

	public void StabilizeMovement(List<MovementRequest> storedMovementRequests, bool alsoStabilizeChasers)
	{
		PrepMoversForStabilization(storedMovementRequests);
		bool flag = true;
		while (flag)
		{
			bool flag2 = StabilizeMoversVsSnares(storedMovementRequests);
			if (!flag2)
			{
				flag2 = StabilizeMoversVsObstacles(storedMovementRequests);
			}
			if (!flag2)
			{
				flag2 = StabilizeMoversVsStationaries(storedMovementRequests);
			}
			if (!flag2)
			{
				flag2 = StabilizeNormalMoversVsStationaryChasers(storedMovementRequests);
			}
			if (!flag2)
			{
				flag2 = StabilizeMoversVsAfterImages(storedMovementRequests);
			}
			if (!flag2)
			{
				flag2 = StabilizeMoversVsMovers(storedMovementRequests);
			}
			if (alsoStabilizeChasers)
			{
				if (!flag2)
				{
					flag2 = StabilizeChasersVsInvisibles(storedMovementRequests);
				}
				if (!flag2)
				{
					flag2 = StabilizeChasersVsStationaries(storedMovementRequests);
				}
				if (!flag2)
				{
					flag2 = StabilizeChasersVsMovers(storedMovementRequests);
				}
				if (!flag2)
				{
					flag2 = StabilizeChasersVsChasers(storedMovementRequests);
				}
			}
			flag = flag2;
		}
		if (alsoStabilizeChasers)
		{
			SanitizeMovement(storedMovementRequests);
		}
	}

	private void SanitizeMovement(List<MovementRequest> storedMovementRequests)
	{
		List<MovementRequest> list = new List<MovementRequest>();
		for (int i = 0; i < storedMovementRequests.Count; i++)
		{
			MovementRequest movementRequest = storedMovementRequests[i];
			if (movementRequest == null)
			{
				Log.Error("SanitizeMovement has null moveRequest.");
				list.Add(movementRequest);
			}
			else if (movementRequest.m_targetSquare == null)
			{
				string text = "SanitizeMovement has a request with a null target square.";
				if (movementRequest.m_actor == null)
				{
					text += ", mover = null";
				}
				else
				{
					text += string.Format(", mover = {0}", movementRequest.m_actor.DisplayName);
				}
				if (movementRequest.m_chaseTarget != null)
				{
					text += string.Format(", chaseTarget = {0}", movementRequest.m_chaseTarget.DisplayName);
				}
				if (movementRequest.m_resolveState != MovementRequest.MovementResolveState.QUEUED)
				{
					text += string.Format(", resolveState = {0}", movementRequest.m_resolveState.ToString());
				}
				Log.Error(text);
				list.Add(movementRequest);
			}
			if (movementRequest != null && movementRequest.m_path != null)
			{
				movementRequest.m_path.CheckPathConnectionForSelfReference();
			}
		}
		foreach (MovementRequest movementRequest2 in list)
		{
			movementRequest2.m_actor.GetComponent<ActorTurnSM>().OnMessage(TurnMessage.MOVEMENT_RESOLVED, true);
			storedMovementRequests.Remove(movementRequest2);
		}
	}

	public void AdjustMovementStartsForMoveAfterEvade(List<MovementRequest> storedMovementRequests)
	{
		foreach (MovementRequest movementRequest in storedMovementRequests)
		{
			if (!movementRequest.IsChasing() && movementRequest.m_path != null)
			{
				BoardSquare currentBoardSquare = movementRequest.m_actor.GetCurrentBoardSquare();
				if (currentBoardSquare != null && currentBoardSquare != movementRequest.m_path.square)
				{
					BoardSquarePathInfo boardSquarePathInfo = movementRequest.m_actor.GetActorMovement().BuildPathTo(currentBoardSquare, movementRequest.m_path.square, 15f, true, null);
					if (boardSquarePathInfo != null)
					{
						float moveRangeCompensation = boardSquarePathInfo.FindMoveCostToEnd();
						movementRequest.m_actor.GetActorMovement().MoveRangeCompensation = moveRangeCompensation;
						BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo.GetPathEndpoint();
						if (boardSquarePathInfo2.prev != null)
						{
							boardSquarePathInfo2 = boardSquarePathInfo2.prev;
						}
						boardSquarePathInfo2.next = movementRequest.m_path;
						movementRequest.m_path.prev = boardSquarePathInfo2;
						movementRequest.m_path = boardSquarePathInfo;
						movementRequest.m_path.CalcAndSetMoveCostToEnd();
					}
					else
					{
						BoardSquarePathInfo boardSquarePathInfo3 = new BoardSquarePathInfo();
						boardSquarePathInfo3.square = currentBoardSquare;
						boardSquarePathInfo3.CalcAndSetMoveCostToEnd();
						movementRequest.m_path = boardSquarePathInfo3;
						movementRequest.m_targetSquare = currentBoardSquare;
					}
				}
			}
		}
	}

	private static void BackUpPath(ActorData mover, BoardSquare squareToDepart, ref BoardSquarePathInfo path, out BoardSquare newDestination, out BoardSquarePathInfo lostPath, string caller)
	{
		lostPath = null;
		if (path == null && mover == null)
		{
			Log.Error("Calling BackUpPath from " + caller + " with an invalid path and an invalid mover-actor.");
			newDestination = null;
			return;
		}
		if (path == null)
		{
			Log.Error(string.Concat(new string[]
			{
				"Calling BackUpPath from ",
				caller,
				" with an invalid path (mover = ",
				mover.DisplayName,
				")."
			}));
			newDestination = null;
			return;
		}
		if (mover == null)
		{
			Log.Error("Calling BackUpPath from " + caller + " with an invalid mover-actor.");
			newDestination = null;
			return;
		}
		BoardSquarePathInfo boardSquarePathInfo = path.GetPathEndpoint();
		int num = 0;
		bool flag = true;
		float maxMovement = mover.GetActorMovement().CalculateMaxHorizontalMovement(false, false);
		while (boardSquarePathInfo.prev != null && flag)
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
			lostPath.square = boardSquarePathInfo.square;
			lostPath.m_unskippable = boardSquarePathInfo.m_unskippable;
			BoardSquarePathInfo prev = boardSquarePathInfo.prev;
			prev.next = null;
			boardSquarePathInfo.next = null;
			boardSquarePathInfo.prev = null;
			boardSquarePathInfo.square = null;
			boardSquarePathInfo = prev;
			num++;
			bool flag2 = !path.IsValidPathForMaxMovement(maxMovement);
			bool flag3 = squareToDepart != null && boardSquarePathInfo.square == squareToDepart;
			flag = (flag2 || flag3);
		}
		if (boardSquarePathInfo != null)
		{
			newDestination = boardSquarePathInfo.square;
			return;
		}
		if (num == 0)
		{
			newDestination = path.square;
			return;
		}
		newDestination = null;
	}

	private bool StabilizeMoversVsSnares(List<MovementRequest> storedMovementRequests)
	{
		bool flag = false;
		for (int i = 0; i < storedMovementRequests.Count; i++)
		{
			MovementRequest moveRequest = storedMovementRequests[i];
			BoardSquarePathInfo boardSquarePathInfo;
			flag |= StabilizeMovementRequestVsSnares(moveRequest, out boardSquarePathInfo);
		}
		return flag;
	}

	public bool StabilizeMovementRequestVsSnares(MovementRequest moveRequest, out BoardSquarePathInfo lostPath)
	{
		bool result;
		if (moveRequest.IsChasing())
		{
			result = false;
			lostPath = null;
		}
		else if (moveRequest.m_path.WillDieAtEnd())
		{
			result = false;
			lostPath = null;
		}
		else if (moveRequest.IsClashStabilized())
		{
			result = false;
			lostPath = null;
		}
		else
		{
			ActorData actor = moveRequest.m_actor;
			float maxMovement = actor.GetActorMovement().CalculateMaxHorizontalMovement(false, false);
			if (!moveRequest.m_path.IsValidPathForMaxMovement(maxMovement))
			{
				BoardSquare targetSquare = null;
				float num = moveRequest.m_path.FindMoveCostToEnd();
                BackUpPath(actor, null, ref moveRequest.m_path, out targetSquare, out lostPath, "StabilizeMoversVsSnares");
				float num2 = moveRequest.m_path.FindMoveCostToEnd();
				if (num != num2)
				{
					moveRequest.m_targetSquare = targetSquare;
					result = true;
					if (num > num2 && actor.GetActorBehavior() != null)
					{
						actor.GetActorBehavior().TrackPathLostDuringStabilization(num - num2);
					}
				}
				else
				{
					Log.Error(string.Format("Failed to back up {0} in StabilizeMoversVsSnares.", moveRequest.m_actor));
					result = false;
				}
			}
			else
			{
				result = false;
				lostPath = null;
			}
		}
		return result;
	}

	private bool StabilizeMoversVsObstacles(List<MovementRequest> storedMovementRequests)
	{
		bool flag = false;
		for (int i = 0; i < storedMovementRequests.Count; i++)
		{
			MovementRequest moveRequest = storedMovementRequests[i];
			BoardSquarePathInfo boardSquarePathInfo;
			flag |= StabilizeMovementRequestVsObstacles(moveRequest, out boardSquarePathInfo);
		}
		return flag;
	}

	public bool StabilizeMovementRequestVsObstacles(MovementRequest moveRequest, out BoardSquarePathInfo lostPath)
	{
		bool result = false;
		lostPath = null;
		if (moveRequest.IsChasing())
		{
			result = false;
			lostPath = null;
		}
		else if (moveRequest.m_path.WillDieAtEnd())
		{
			result = false;
			lostPath = null;
		}
		else if (moveRequest.IsClashStabilized())
		{
			result = false;
			lostPath = null;
		}
		else
		{
			ActorData actor = moveRequest.m_actor;
			BoardSquarePathInfo boardSquarePathInfo = moveRequest.m_path;
			BoardSquarePathInfo boardSquarePathInfo2 = moveRequest.m_path.next;
			BoardSquarePathInfo prev = moveRequest.m_path.prev;
			while (boardSquarePathInfo2 != null)
			{
				bool flag = !boardSquarePathInfo2.square.IsValidForGameplay() || (prev != null && BarrierManager.Get() != null && BarrierManager.Get().IsMovementBlockedOnCrossover(actor, prev.square, boardSquarePathInfo.square)) || (BarrierManager.Get() != null && BarrierManager.Get().IsMovementBlocked(actor, boardSquarePathInfo.square, boardSquarePathInfo2.square)) || (ServerEffectManager.Get() != null && ServerEffectManager.Get().IsMovementBlockedOnEnterSquare(actor, (prev != null) ? prev.square : null, boardSquarePathInfo.square));
				if (flag)
				{
					lostPath = boardSquarePathInfo2;
					boardSquarePathInfo.next = null;
					boardSquarePathInfo2.prev = null;
					boardSquarePathInfo2 = null;
					moveRequest.m_targetSquare = boardSquarePathInfo.square;
					result = true;
				}
				else
				{
					boardSquarePathInfo = boardSquarePathInfo2;
					prev = boardSquarePathInfo2.prev;
					boardSquarePathInfo2 = boardSquarePathInfo2.next;
				}
			}
		}
		return result;
	}

	private bool StabilizeNormalMoversVsStationaryChasers(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		for (int i = 0; i < storedMovementRequests.Count; i++)
		{
			MovementRequest movementRequest = storedMovementRequests[i];
			if (!movementRequest.WasEverChasing() && !movementRequest.m_path.WillDieAtEnd() && !movementRequest.IsClashStabilized())
			{
				for (int j = 0; j < storedMovementRequests.Count; j++)
				{
					MovementRequest movementRequest2 = storedMovementRequests[j];
					if (movementRequest2.WasEverChasing())
					{
						Object targetSquare = movementRequest.m_targetSquare;
						BoardSquare currentBoardSquare = movementRequest2.m_actor.GetCurrentBoardSquare();
						if (targetSquare == currentBoardSquare)
						{
							BoardSquare boardSquare = null;
							BoardSquarePathInfo boardSquarePathInfo;
                            BackUpPath(movementRequest.m_actor, currentBoardSquare, ref movementRequest.m_path, out boardSquare, out boardSquarePathInfo, "StabilizeNormalMoversVsStationaryChasers");
							if (movementRequest.m_targetSquare != boardSquare)
							{
								movementRequest.m_targetSquare = boardSquare;
								result = true;
							}
							else
							{
								Log.Error(string.Format("Failed to back up {0} in StabilizeMoversVsStationaries.", movementRequest.m_actor));
							}
						}
					}
				}
			}
		}
		return result;
	}

	private bool StabilizeMoversVsStationaries(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		foreach (ActorData actorData in ServerActionBuffer.Get().GetStationaryActors())
		{
			if (!actorData.IsDead())
			{
				for (int i = 0; i < storedMovementRequests.Count; i++)
				{
					MovementRequest movementRequest = storedMovementRequests[i];
					if (!movementRequest.IsChasing() && !movementRequest.m_path.WillDieAtEnd() && !movementRequest.IsClashStabilized())
					{
						Object targetSquare = movementRequest.m_targetSquare;
						BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
						if (targetSquare == currentBoardSquare)
						{
							BoardSquare boardSquare = null;
							BoardSquarePathInfo boardSquarePathInfo;
                            BackUpPath(movementRequest.m_actor, currentBoardSquare, ref movementRequest.m_path, out boardSquare, out boardSquarePathInfo, "StabilizeMoversVsStationaries");
							if (movementRequest.m_targetSquare != boardSquare)
							{
								movementRequest.m_targetSquare = boardSquare;
								result = true;
							}
							else
							{
								Log.Error(string.Format("Failed to back up {0} in StabilizeMoversVsStationaries.", movementRequest.m_actor));
							}
						}
					}
				}
			}
		}
		return result;
	}

	private bool StabilizeMoversVsAfterImages(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		List<ActorData> list = new List<ActorData>();
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (!actorData.IsDead() && actorData.GetComponent<Passive_TricksterAfterImage>() != null)
			{
				list.Add(actorData);
			}
		}
		foreach (ActorData actorData2 in list)
		{
			for (int i = 0; i < storedMovementRequests.Count; i++)
			{
				MovementRequest movementRequest = storedMovementRequests[i];
				if (!movementRequest.IsChasing() && !(movementRequest.m_actor == actorData2) && !movementRequest.m_path.WillDieAtEnd() && !movementRequest.IsClashStabilized())
				{
					Object targetSquare = movementRequest.m_targetSquare;
					BoardSquare currentBoardSquare = actorData2.GetCurrentBoardSquare();
					if (targetSquare == currentBoardSquare)
					{
						BoardSquare boardSquare = null;
						BoardSquarePathInfo boardSquarePathInfo;
                        BackUpPath(movementRequest.m_actor, currentBoardSquare, ref movementRequest.m_path, out boardSquare, out boardSquarePathInfo, "StabilizeMoversVsAfterImages");
						if (movementRequest.m_targetSquare != boardSquare)
						{
							movementRequest.m_targetSquare = boardSquare;
							result = true;
						}
						else
						{
							Log.Error(string.Format("Failed to back up {0} in StabilizeMoversVsAfterImages.", movementRequest.m_actor));
						}
					}
				}
			}
		}
		return result;
	}

	private bool StabilizeMoversVsMovers(List<MovementRequest> storedMovementRequests)
	{
		bool flag = false;
		storedMovementRequests.Sort();
		int num = 0;
		while (num < storedMovementRequests.Count && !flag)
		{
			MovementRequest movementRequest = storedMovementRequests[num];
			if (!movementRequest.IsChasing() && !movementRequest.m_path.WillDieAtEnd())
			{
				for (int i = num + 1; i < storedMovementRequests.Count; i++)
				{
					MovementRequest movementRequest2 = storedMovementRequests[i];
					if (!movementRequest2.IsChasing() && !movementRequest2.m_path.WillDieAtEnd() && !(movementRequest.m_targetSquare != movementRequest2.m_targetSquare) && (movementRequest.m_actor.GetTeam() == movementRequest2.m_actor.GetTeam() || movementRequest.m_path.FindMoveCostToEnd() != movementRequest2.m_path.FindMoveCostToEnd() || movementRequest.WasEverChasing() != movementRequest2.WasEverChasing()) && !movementRequest2.IsClashStabilized())
					{
						BoardSquare boardSquare = null;
						BoardSquarePathInfo boardSquarePathInfo;
                        BackUpPath(movementRequest2.m_actor, movementRequest.m_targetSquare, ref movementRequest2.m_path, out boardSquare, out boardSquarePathInfo, "StabilizeMoversVsMovers");
						if (movementRequest2.m_targetSquare != boardSquare)
						{
							movementRequest2.m_targetSquare = boardSquare;
							flag = true;
						}
						else
						{
							Log.Error(string.Format("Failed to back up {0} in StabilizeMoversVsMovers.", movementRequest2.m_actor));
						}
					}
				}
			}
			num++;
		}
		return flag;
	}

	private bool StabilizeChasersVsInvisibles(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		for (int i = 0; i < storedMovementRequests.Count; i++)
		{
			MovementRequest movementRequest = storedMovementRequests[i];
			ActorData actor = movementRequest.m_actor;
			ActorData chaseTarget = movementRequest.m_chaseTarget;
			if (movementRequest.IsChasing()
			    && !movementRequest.IsBeingDragged()
			    && chaseTarget.IsNeverVisibleTo(actor.PlayerData)
			    && !chaseTarget.IsAlwaysVisibleTo(actor.PlayerData))
			{
				BoardSquare serverLastKnownPosSquare = chaseTarget.ServerLastKnownPosSquare;
				ConvertChaseToDirectMovement(movementRequest, serverLastKnownPosSquare);
				result = true;
			}
		}
		return result;
	}

	private bool StabilizeChasersVsStationaries(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		List<ActorData> stationaryActors = ServerActionBuffer.Get().GetStationaryActors();
		for (int i = 0; i < storedMovementRequests.Count; i++)
		{
			MovementRequest movementRequest = storedMovementRequests[i];
			ActorData chaseTarget = movementRequest.m_chaseTarget;
			if (movementRequest.IsChasing() && stationaryActors.Contains(chaseTarget))
			{
				ActorData actor = movementRequest.m_actor;
				ActorMovement component = movementRequest.m_actor.GetComponent<ActorMovement>();
				ActorBehavior actorBehavior = chaseTarget.GetActorBehavior();
				if (chaseTarget.IsDead() || movementRequest.IsBeingDragged() || chaseTarget.IsActorVisibleToActor(actor))
				{
					BoardSquare targetSquare;
					if (chaseTarget.IsDead())
					{
						targetSquare = chaseTarget.GetMostRecentDeathSquare();
					}
					else
					{
						targetSquare = chaseTarget.GetCurrentBoardSquare();
					}
					ConvertChaseToDirectMovement(movementRequest, targetSquare);
				}
				else
				{
					bool flag = true;
					if (actorBehavior.CurrentTurn.Charged || actorBehavior.CurrentTurn.KnockedBack)
					{
						BoardSquarePathInfo path = actorBehavior.CurrentTurn.Path;
						if (path != null)
						{
							BoardSquarePathInfo boardSquarePathInfo = FindChaserLastVisiblePartOfMoverPath(actor, chaseTarget, path);
							if (boardSquarePathInfo != null)
							{
								flag = false;
								BoardSquare square = boardSquarePathInfo.square;
								if (!component.CanMoveToBoardSquare(square))
								{
									ConvertChaseToDirectMovement(movementRequest, square);
								}
								else
								{
									movementRequest.m_path = component.BuildPathTo(actor.GetCurrentBoardSquare(), square);
									BoardSquarePathInfo pathEndpoint = movementRequest.m_path.GetPathEndpoint();
									pathEndpoint.next = boardSquarePathInfo.Clone(pathEndpoint);
									movementRequest.m_path.CalcAndSetMoveCostToEnd();
									BoardSquare targetSquare2;
									BoardSquarePathInfo boardSquarePathInfo2;
                                    BackUpPath(actor, null, ref movementRequest.m_path, out targetSquare2, out boardSquarePathInfo2, "StabilizeChasersVsStationaries");
									movementRequest.m_chaseTarget = null;
									movementRequest.m_targetSquare = targetSquare2;
								}
							}
						}
					}
					if (flag)
					{
						BoardSquare serverLastKnownPosSquare = chaseTarget.ServerLastKnownPosSquare;
						ConvertChaseToDirectMovement(movementRequest, serverLastKnownPosSquare);
					}
				}
				result = true;
			}
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
		ActorMovement component = chaseRequest.m_actor.GetComponent<ActorMovement>();
		bool flag;
		List<BoardSquare> claimedSquares;
		if (targetSquare == null || !component.CanMoveToBoardSquare(targetSquare))
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
			BoardSquare closestValidForGameplaySquareTo = Board.Get().GetClosestValidForGameplaySquareTo(targetSquare, null);
			Log.Error(string.Concat(new string[]
			{
				"ConvertChaseToDirectMovement being called for invalid-for-gameplay targetSquare ",
				targetSquare.ToString(),
				".  Going instead toward closest valid square, ",
				closestValidForGameplaySquareTo.ToString(),
				".  Scene = ",
				SceneManager.GetActiveScene().name
			}));
			targetSquare = closestValidForGameplaySquareTo;
		}
		BoardSquare currentBoardSquare = actor.GetCurrentBoardSquare();
		if (currentBoardSquare == null)
		{
			Log.Error(actor.DebugNameString() + " has null for current square when trying to convert chase movement to normal movement");
		}
		BoardSquarePathInfo boardSquarePathInfo = component.BuildCompletePathTo(actor.GetCurrentBoardSquare(), targetSquare, true, claimedSquares);
		if (boardSquarePathInfo == null)
		{
			Log.Error(string.Concat(new string[]
			{
				"ConvertChaseToDirectMovement failed to build path to target, from square ",
				(currentBoardSquare != null) ? currentBoardSquare.ToString() : "null",
				" to square ",
				(targetSquare != null) ? targetSquare.ToString() : "null",
				".  Scene = ",
				SceneManager.GetActiveScene().name
			}));
			if (currentBoardSquare != null)
			{
				boardSquarePathInfo = new BoardSquarePathInfo();
				boardSquarePathInfo.square = currentBoardSquare;
				targetSquare = currentBoardSquare;
			}
			flag = false;
		}
		chaseRequest.m_path = boardSquarePathInfo;
		if (flag)
		{
			float pathCostBeforeBackup = chaseRequest.m_path.FindMoveCostToEnd();
			BoardSquare boardSquare;
			BoardSquarePathInfo boardSquarePathInfo2;
            BackUpPath(actor, actor.GetCurrentBoardSquare(), ref chaseRequest.m_path, out boardSquare, out boardSquarePathInfo2, "ConvertChaseToDirectMovement");
			targetSquare = boardSquare;
			float pathCostAfterBackup = chaseRequest.m_path.FindMoveCostToEnd();
			TrackChaseMovementLostToSnare(actor, pathCostBeforeBackup, pathCostAfterBackup);
		}
		chaseRequest.m_chaseTarget = null;
		chaseRequest.m_targetSquare = targetSquare;
	}

	private void TrackChaseMovementLostToSnare(ActorData chaserActor, float pathCostBeforeBackup, float pathCostAfterBackup)
	{
		if (chaserActor.GetActorBehavior() != null)
		{
			float num = (float)chaserActor.GetActorStats().GetModifiedStatInt(StatType.Movement_Horizontal);
			if (chaserActor.GetAbilityData() != null)
			{
				num += chaserActor.GetAbilityData().GetQueuedAbilitiesMovementAdjust();
			}
			num += chaserActor.GetActorMovement().MoveRangeCompensation;
			float num2 = num;
			if (chaserActor.GetActorStatus().HasStatus(StatusType.Hasted, true))
			{
				float num3;
				int num4;
				int num5;
				ActorMovement.CalcHastedMovementAdjustments(out num3, out num4, out num5);
				num2 *= num3;
			}
			float num6 = Mathf.Min(pathCostBeforeBackup, num2);
			if (num6 > pathCostAfterBackup)
			{
				if (GameplayMetricHelper.GameplayMetricDebugTraceOn)
				{
					GameplayMetricHelper.DebugLogMoveDenied(string.Concat(new object[]
					{
						"on converting chase to normal move, accounting for lost path. Desired= ",
						num6,
						" | Actual= ",
						pathCostAfterBackup,
						" | Diff= ",
						num6 - pathCostAfterBackup
					}));
				}
				chaserActor.GetActorBehavior().TrackPathLostDuringStabilization(num6 - pathCostAfterBackup);
			}
		}
	}

	private bool StabilizeChasersVsMovers(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		for (int i = 0; i < storedMovementRequests.Count; i++)
		{
			MovementRequest movementRequest = storedMovementRequests[i];
			if (movementRequest.IsChasing())
			{
				for (int j = 0; j < storedMovementRequests.Count; j++)
				{
					MovementRequest movementRequest2 = storedMovementRequests[j];
					if (!(movementRequest.m_chaseTarget != movementRequest2.m_actor) && !movementRequest2.IsChasing())
					{
						BoardSquare targetSquare = movementRequest2.m_targetSquare;
						ActorData actor = movementRequest.m_actor;
						FogOfWar component = actor.GetComponent<FogOfWar>();
						ActorMovement component2 = movementRequest.m_actor.GetComponent<ActorMovement>();
						bool flag = movementRequest2.m_path.WillDieAtEnd();
						bool flag2 = movementRequest2.m_actor.GetActorStatus().HasStatus(StatusType.Revealed, true);
						if (movementRequest.IsBeingDragged() || component.IsVisible(targetSquare) || flag2)
						{
							ConvertChaseToDirectMovement(movementRequest, targetSquare);
							if (movementRequest.m_targetSquare == targetSquare && !flag)
							{
								BoardSquarePathInfo boardSquarePathInfo = movementRequest.m_path.BackUpOnceFromEnd();
								movementRequest.m_targetSquare = boardSquarePathInfo.square;
							}
						}
						else
						{
							BoardSquarePathInfo path = movementRequest2.m_path;
							if (path != null)
							{
								BoardSquarePathInfo boardSquarePathInfo2 = FindChaserLastVisiblePartOfMoverPath(actor, movementRequest2.m_actor, path);
								if (boardSquarePathInfo2 != null)
								{
									BoardSquare square = boardSquarePathInfo2.square;
									if (!component2.CanMoveToBoardSquare(square))
									{
										ConvertChaseToDirectMovement(movementRequest, square);
									}
									else
									{
										movementRequest.m_path = component2.BuildPathTo(actor.GetCurrentBoardSquare(), square);
										BoardSquarePathInfo pathEndpoint = movementRequest.m_path.GetPathEndpoint();
										if (boardSquarePathInfo2.next != null)
										{
											BoardSquarePathInfo boardSquarePathInfo3 = boardSquarePathInfo2.next.Clone(pathEndpoint);
											for (BoardSquarePathInfo boardSquarePathInfo4 = boardSquarePathInfo3; boardSquarePathInfo4 != null; boardSquarePathInfo4 = boardSquarePathInfo4.next)
											{
												boardSquarePathInfo4.m_moverBumpedFromClash = false;
												boardSquarePathInfo4.m_moverClashesHere = false;
												boardSquarePathInfo4.m_moverDiesHere = false;
												boardSquarePathInfo4.m_moverHasGameplayHitHere = false;
												boardSquarePathInfo4.m_visibleToEnemies = false;
												boardSquarePathInfo4.m_updateLastKnownPos = false;
											}
											pathEndpoint.next = boardSquarePathInfo3;
										}
										movementRequest.m_path.CalcAndSetMoveCostToEnd();
										BoardSquare square2;
										if (!flag)
										{
											BoardSquarePathInfo boardSquarePathInfo5;
                                            BackUpPath(actor, null, ref movementRequest.m_path, out square2, out boardSquarePathInfo5, "StabilizeChasersVsMovers");
										}
										else
										{
											square2 = movementRequest.m_path.GetPathEndpoint().square;
										}
										movementRequest.m_chaseTarget = null;
										movementRequest.m_targetSquare = square2;
									}
								}
							}
						}
						if (movementRequest.IsChasing())
						{
							Log.Warning(string.Format("StabilizeChasersVsMovers failed to stabilize chaser {0}'s pursuit of target {1}.  Going to target's square at turn start...", actor.DisplayName, movementRequest2.m_actor.DisplayName));
							BoardSquare square3 = movementRequest2.m_actor.GetActorBehavior().CurrentTurn.Square;
							ConvertChaseToDirectMovement(movementRequest, square3);
						}
						result = true;
						break;
					}
				}
			}
		}
		return result;
	}

	private bool StabilizeChasersVsChasers(List<MovementRequest> storedMovementRequests)
	{
		bool result = false;
		for (int i = 0; i < storedMovementRequests.Count; i++)
		{
			MovementRequest movementRequest = storedMovementRequests[i];
			if (movementRequest.IsChasing())
			{
				for (int j = 0; j < storedMovementRequests.Count; j++)
				{
					MovementRequest movementRequest2 = storedMovementRequests[j];
					if (!(movementRequest.m_chaseTarget != movementRequest2.m_actor) && movementRequest2.IsChasing())
					{
						bool flag = false;
						ActorMovement component = movementRequest.m_actor.GetComponent<ActorMovement>();
						ActorData actor = movementRequest.m_actor;
						ActorMovement component2 = movementRequest2.m_actor.GetComponent<ActorMovement>();
						ActorData actor2 = movementRequest2.m_actor;
						BoardSquarePathInfo boardSquarePathInfo = component.BuildPathTo(actor.GetCurrentBoardSquare(), actor2.GetCurrentBoardSquare());
						if (boardSquarePathInfo != null && movementRequest2.m_chaseTarget == movementRequest.m_actor)
						{
							BoardSquarePathInfo pathMidpoint = boardSquarePathInfo.GetPathMidpoint();
							if (component2.CanMoveToBoardSquare(pathMidpoint.square))
							{
								pathMidpoint.next = null;
								movementRequest.m_path = boardSquarePathInfo;
								movementRequest.m_targetSquare = pathMidpoint.square;
								movementRequest.m_chaseTarget = null;
								movementRequest2.m_path = component2.BuildPathTo(actor2.GetCurrentBoardSquare(), pathMidpoint.square);
								movementRequest2.m_targetSquare = pathMidpoint.square;
								movementRequest2.m_chaseTarget = null;
								result = true;
							}
							else
							{
								BoardSquarePathInfo boardSquarePathInfo2 = component2.BuildPathTo(actor2.GetCurrentBoardSquare(), actor.GetCurrentBoardSquare());
								if (boardSquarePathInfo2 != null)
								{
									BoardSquarePathInfo pathMidpoint2 = boardSquarePathInfo2.GetPathMidpoint();
									if (component.CanMoveToBoardSquare(pathMidpoint2.square))
									{
										pathMidpoint2.next = null;
										movementRequest2.m_path = boardSquarePathInfo2;
										movementRequest2.m_targetSquare = pathMidpoint2.square;
										movementRequest2.m_chaseTarget = null;
										movementRequest.m_path = component.BuildPathTo(actor.GetCurrentBoardSquare(), pathMidpoint2.square);
										movementRequest.m_targetSquare = pathMidpoint2.square;
										movementRequest.m_chaseTarget = null;
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
						if (flag && boardSquarePathInfo == null)
						{
							BoardSquare currentBoardSquare = actor2.GetCurrentBoardSquare();
							BoardSquare closestMoveableSquareTo = component.GetClosestMoveableSquareTo(currentBoardSquare);
							movementRequest.m_path = component.BuildPathTo(actor.GetCurrentBoardSquare(), closestMoveableSquareTo);
							movementRequest.m_chaseTarget = null;
							movementRequest.m_targetSquare = closestMoveableSquareTo;
							result = true;
						}
						else if (flag)
						{
							movementRequest.m_path = boardSquarePathInfo;
							movementRequest.m_chaseTarget = null;
							movementRequest.m_targetSquare = movementRequest2.m_actor.GetCurrentBoardSquare();
							result = true;
						}
					}
				}
			}
		}
		return result;
	}

	private static BoardSquarePathInfo FindChaserLastVisiblePartOfMoverPath(ActorData chaser, ActorData mover, BoardSquarePathInfo moverPath)
	{
		FogOfWar component = chaser.GetComponent<FogOfWar>();
		BoardSquarePathInfo boardSquarePathInfo = moverPath;
		BoardSquarePathInfo boardSquarePathInfo2 = null;
		bool flag = false;
		while (boardSquarePathInfo != null && boardSquarePathInfo.square != null)
		{
			if (boardSquarePathInfo.square.IsValidForGameplay() && component.IsVisible(boardSquarePathInfo.square))
			{
				boardSquarePathInfo2 = boardSquarePathInfo;
				flag = true;
			}
			else if (!boardSquarePathInfo.square.IsValidForGameplay() && boardSquarePathInfo.prev != null && boardSquarePathInfo.prev == boardSquarePathInfo2)
			{
				boardSquarePathInfo2 = boardSquarePathInfo;
				flag = false;
			}
			else if (boardSquarePathInfo.prev != null && !boardSquarePathInfo.prev.square.IsValidForGameplay() && boardSquarePathInfo.prev == boardSquarePathInfo2)
			{
				boardSquarePathInfo2 = boardSquarePathInfo;
				flag = false;
			}
			boardSquarePathInfo = boardSquarePathInfo.next;
		}
		if (boardSquarePathInfo2 != null && flag && boardSquarePathInfo2.next != null)
		{
			boardSquarePathInfo2 = boardSquarePathInfo2.next;
		}
		return boardSquarePathInfo2;
	}

	public void ModifyPathForMaxMovement(ActorData mover, BoardSquarePathInfo path, bool modifyAsIfSnared)
	{
		if (mover != null && path != null)
		{
			int num = 0;
			bool flag = true;
			float maxMovement = mover.GetActorMovement().CalculateMaxHorizontalMovement(false, modifyAsIfSnared);
			while (!path.IsValidPathForMaxMovement(maxMovement) && flag && num < 100)
			{
				BoardSquare boardSquare = null;
				float num2 = path.FindMoveCostToEnd();
				BoardSquarePathInfo boardSquarePathInfo;
                BackUpPath(mover, null, ref path, out boardSquare, out boardSquarePathInfo, "ModifyPathForMaxMovement");
				float num3 = path.FindMoveCostToEnd();
				if (num2 == num3)
				{
					flag = false;
				}
				num++;
			}
			if (num >= 100)
			{
				Log.Error("ModifyPathForMaxMovement did not finish within 100 iterations");
			}
		}
	}

	public void RemoveStationaryMovementRequests(ref List<MovementRequest> movementRequests)
	{
		List<MovementRequest> list = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in movementRequests)
		{
			if (movementRequest.m_targetSquare == movementRequest.m_actor.GetCurrentBoardSquare() && movementRequest.m_path.next == null)
			{
				list.Add(movementRequest);
			}
		}
		foreach (MovementRequest item in list)
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
