// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;

// rogues-only, missing in reactor
#if SERVER
public class PlayerAction_Movement : PlayerAction
{
	private List<MovementRequest> m_moveRequests;
	
	// custom
	private bool m_isChase;

	public PlayerAction_Movement(List<MovementRequest> moveRequests, bool isChase = false)
	{
		m_moveRequests = moveRequests;
		m_isChase = isChase;
	}

	// rogues+custom: no chasing in rogues
	public override bool ExecuteAction()
	{
		if (m_moveRequests == null)
		{
			Log.Error("No movement requests");
			return false;
		}
		base.ExecuteAction();
		for (int i = m_moveRequests.Count - 1; i >= 0; i--)
		{
			MovementRequest movementRequest = m_moveRequests[i];
			if (movementRequest.m_actor.IsDead())
			{
				Log.Info($"Cancelling ${movementRequest.m_actor.m_displayName}'s movement request because they are dead");
				ServerActionBuffer.Get().CancelMovementRequests(movementRequest.m_actor, false);
				m_moveRequests.RemoveAt(i);
			}
		}
		if (m_moveRequests.Count == 0)
		{
			Log.Info("No movement requests");
			return false;
		}
		List<MovementRequest> validRequests = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in m_moveRequests)
		{
			BoardSquare targetSquare = movementRequest.m_targetSquare;
			if (movementRequest.m_path != null && movementRequest.m_path.next != null && targetSquare != null || movementRequest.IsChasing())
			{
				// rogues
				//movementRequest.m_actor.GetActorTurnSM().OnMessage(TurnMessage.EXECUTE_ACTION_START, true);
				validRequests.Add(movementRequest);
			}
		}
		Log.Info($"{validRequests.Count} valid out of {m_moveRequests.Count} movement requests");
		ServerActionBuffer.Get().GetMoveStabilizer().StabilizeMovement(validRequests, m_isChase);
		for (int j = validRequests.Count - 1; j >= 0; j--)
		{
			MovementRequest movementRequest = validRequests[j];
			if (movementRequest.m_path == null || movementRequest.m_path.next == null)
			{
				Log.Warning($"{movementRequest.m_actor.m_displayName}'s movement path is null after stabilization");
				ServerActionBuffer.Get().CancelMovementRequests(movementRequest.m_actor, false);
				movementRequest.m_actor.GetActorMovement().UpdateSquaresCanMoveTo();
				validRequests.RemoveAt(j);
			}
		}
		ServerActionBuffer.Get().ClearNormalMovementResults();
		ServerGameplayUtils.GatherGameplayResultsForNormalMovement(validRequests, m_isChase);
		MovementCollection movementCollection = new MovementCollection(validRequests.Where(r => r.WasEverChasing() == m_isChase).ToList());
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
		ServerMovementManager.Get().ServerMovementManager_OnMovementStart(movementCollection, m_isChase
			? ServerMovementManager.MovementType.NormalMovement_Chase
			: ServerMovementManager.MovementType.NormalMovement_NonChase);
		foreach (MovementRequest movementRequest in validRequests)
		{
			ServerActionBuffer.Get().RunMovementOnRequest(movementRequest);
			ActorStatus actorStatus = movementRequest.m_actor.GetActorStatus();
			if (actorStatus != null && actorStatus.HasStatus(StatusType.KnockedBack, true))
			{
				actorStatus.RemoveStatus(StatusType.KnockedBack);
			}
			// rogues
			// float increment = movementRequest.m_path.FindMoveCostToEnd();
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
			// rogues
			//bool flag3 = !flag2;
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
