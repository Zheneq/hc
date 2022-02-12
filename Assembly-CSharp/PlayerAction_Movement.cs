// ROGUES
// SERVER
using System.Collections.Generic;

// rogues-only, missing in reactor
#if SERVER
public class PlayerAction_Movement : PlayerAction
{
	private List<MovementRequest> m_moveRequests;

	public PlayerAction_Movement(List<MovementRequest> moveRequests)
	{
		m_moveRequests = moveRequests;
	}

	public override bool ExecuteAction()
	{
		if (m_moveRequests == null)
		{
			return false;
		}
		base.ExecuteAction();
		for (int i = m_moveRequests.Count - 1; i >= 0; i--)
		{
			MovementRequest movementRequest = m_moveRequests[i];
			if (movementRequest.m_actor.IsDead())
			{
				//ServerActionBuffer.Get().CancelMovementRequests(movementRequest.m_actor, false);
				m_moveRequests.RemoveAt(i);
			}
		}
		if (m_moveRequests.Count == 0)
		{
			return false;
		}
		List<MovementRequest> validRequests = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in m_moveRequests)
		{
			BoardSquare targetSquare = movementRequest.m_targetSquare;
			if (movementRequest.m_path != null && movementRequest.m_path.next != null && targetSquare != null)
			{
				// rogues
				//movementRequest.m_actor.GetActorTurnSM().OnMessage(TurnMessage.EXECUTE_ACTION_START, true);
				validRequests.Add(movementRequest);
			}
		}
		ServerActionBuffer.Get().GetMoveStabilizer().StabilizeMovement(validRequests, false);
		for (int j = validRequests.Count - 1; j >= 0; j--)
		{
			MovementRequest movementRequest = validRequests[j];
			if (movementRequest.m_path == null || movementRequest.m_path.next == null)
			{
				ServerActionBuffer.Get().CancelMovementRequests(movementRequest.m_actor, false);
				movementRequest.m_actor.GetActorMovement().UpdateSquaresCanMoveTo();
				validRequests.RemoveAt(j);
			}
		}
		ServerActionBuffer.Get().ClearNormalMovementResults();
		ServerGameplayUtils.GatherGameplayResultsForNormalMovement(validRequests, false);
		MovementCollection movementCollection = new MovementCollection(validRequests);
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData.GetPassiveData() != null)
			{
				actorData.GetPassiveData().OnMovementResultsGathered(movementCollection);
			}
			actorData.GetActorMovement().ClearPath();
			actorData.UpdateServerLastVisibleTurn();
		}
		ServerGameplayUtils.SetServerLastKnownPositionsForMovement(movementCollection, out _, out _);
		ServerResolutionManager.Get().OnNormalMovementStart();
		ServerMovementManager.Get().ServerMovementManager_OnMovementStart(movementCollection, ServerMovementManager.MovementType.NormalMovement_NonChase);
		foreach (MovementRequest movementRequest in validRequests)
		{
			ServerActionBuffer.Get().RunMovementOnRequest(movementRequest);
			ActorStatus actorStatus = movementRequest.m_actor.GetActorStatus();
			if (actorStatus != null && actorStatus.HasStatus(StatusType.KnockedBack, true))
			{
				actorStatus.RemoveStatus(StatusType.KnockedBack);
			}
			//float increment = movementRequest.m_path.FindMoveCostToEnd();
			float num = movementRequest.m_actor.GetActorMovement().CalculateMaxHorizontalMovement(true, false);
			BoardSquarePathInfo boardSquarePathInfo = movementRequest.m_path;
			bool isRoundingDown = GameplayData.Get() != null && GameplayData.Get().m_movementMaximumType == GameplayData.MovementMaximumType.CannotExceedMax;
			bool canContinue = true;
			while (boardSquarePathInfo != null && canContinue)
			{
				if (isRoundingDown)
				{
					canContinue = boardSquarePathInfo.moveCost <= num;
				}
				else if (boardSquarePathInfo.next != null)
				{
					canContinue = boardSquarePathInfo.moveCost < num;
				}
				boardSquarePathInfo = boardSquarePathInfo.next;
			}
			//bool flag3 = !flag2;
			// rogues
			//movementRequest.m_actor.GetActorTurnSM().IncrementPveMoveCostUsed(increment);
			//movementRequest.m_actor.GetActorTurnSM().IncrementPveNumMoveActions(flag3 ? 2 : 1);
		}
		return validRequests.Count > 0;
	}

	public override void OnExecutionComplete(bool isLastAction)
	{
		foreach (MovementRequest movementRequest in m_moveRequests)
		{
			ServerActionBuffer.Get().CancelMovementRequests(movementRequest.m_actor, false);
		}
		if (ServerCombatManager.Get().HasUnresolvedHealthEntries())
		{
			ServerCombatManager.Get().ResolveHitPoints();
		}
		if (ServerCombatManager.Get().HasUnresolvedTechPointsEntries())
		{
			ServerCombatManager.Get().ResolveTechPoints();
		}
	}
}
#endif
