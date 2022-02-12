// ROGUES
// SERVER
using System.Collections.Generic;

// server-only, missing in reactor
#if SERVER
public class QueuedPlayerActionsContainer
{
	private List<PlayerAction> m_queuedActions = new List<PlayerAction>();
	private bool m_isExecutingEffects;
	private List<ActorData> m_actorsWithActions = new List<ActorData>();

	public int NextActionIndex { get; private set; }

	public void AddQueuedAction(PlayerAction action)
	{
		this.m_queuedActions.Add(action);
	}

	public void ClearQueuedActions()
	{
		// rogues
		//this.SendExecutionDoneMessages();
		this.m_actorsWithActions.Clear();
		this.m_queuedActions.Clear();
	}

	public int GetNumQueuedActions()
	{
		return this.m_queuedActions.Count;
	}

	// rogues
	//public void SendExecutionStartMessage()
	//{
	//	foreach (ActorData actorData in this.m_actorsWithActions)
	//	{
	//		actorData.GetActorTurnSM().OnMessage(TurnMessage.EXECUTE_ACTION_START, true);
	//	}
	//}

	// rogues
	//public void SendExecutionDoneMessages()
	//{
	//	foreach (ActorData actorData in this.m_actorsWithActions)
	//	{
	//		actorData.GetActorTurnSM().OnMessage(TurnMessage.EXECUTE_ACTION_DONE, true);
	//	}
	//}

	public void BeginExecutingEffects()
	{
		this.m_isExecutingEffects = true;
	}

	public void EndExecutingEffects()
	{
		this.m_isExecutingEffects = false;
	}

	public bool IsExecutingEffects()
	{
		return this.m_isExecutingEffects;
	}

	public void InitEffectsForExecution(Team castersTeam)
	{
		this.NextActionIndex = 0;
		if (this.m_actorsWithActions.Count > 0)
		{
			Log.Error("Has uncleared list of relevant actors when initializing for Effects execution");

			// rogues
			//this.SendExecutionDoneMessages();
			this.m_actorsWithActions.Clear();
		}
		this.ClearQueuedActions();
		Dictionary<ActorData, List<global::Effect>> allActorEffects = ServerEffectManager.Get().GetAllActorEffects();
		List<global::Effect> worldEffects = ServerEffectManager.Get().GetWorldEffects();
		bool flag = false;
		for (int i = 0; i < 7; i++)
		{
			AbilityPriority abilityPriority = (AbilityPriority)i;
			List<global::Effect> list = new List<global::Effect>();
			foreach (ActorData actorData in allActorEffects.Keys)
			{
				foreach (global::Effect effect in allActorEffects[actorData])
				{
					if (effect.HitPhase == abilityPriority)
					{
						bool flag2 = false;
						// rogues
						//EffectSystem.Effect effect2;
						//if ((effect2 = (effect as EffectSystem.Effect)) != null)
						//{
						//	if (effect2.HasResolutionAction(abilityPriority))
						//	{
						//		flag2 = true;
						//	}
						//}
						//else
						//{
						EffectResults resultsForPhase = effect.GetResultsForPhase(abilityPriority, true);
							if (effect.HitPhase == abilityPriority && effect.CanExecuteForTeam_FCFS(castersTeam) && (resultsForPhase == null || !resultsForPhase.GatheredResults))
							{
								effect.Resolve();
								flag2 = true;
							}
						//}
						if (flag2)
						{
							list.Add(effect);
							if (!this.m_actorsWithActions.Contains(actorData))
							{
								this.m_actorsWithActions.Add(actorData);
							}
						}
					}
				}
			}
			foreach (global::Effect effect3 in worldEffects)
			{
				if (effect3.HitPhase == abilityPriority && effect3.CanExecuteForTeam_FCFS(castersTeam))
				{
					// rogues
					//EffectSystem.Effect effect4;
					//if ((effect4 = (effect3 as EffectSystem.Effect)) != null)
					//{
					//	if (effect4.HasResolutionAction(abilityPriority))
					//	{
					//		this.m_actorsWithActions.Add(effect3.Caster);
					//		list.Add(effect3);
					//	}
					//}
					//else
					//{
					EffectResults resultsForPhase2 = effect3.GetResultsForPhase(abilityPriority, true);
						if (effect3.HitPhase == abilityPriority && (resultsForPhase2 == null || !resultsForPhase2.GatheredResults))
						{
							effect3.Resolve();
							this.m_actorsWithActions.Add(effect3.Caster);
							list.Add(effect3);
						}
					//}
				}
			}
			if (list.Count > 0)
			{
				PlayerAction_Effect action = new PlayerAction_Effect(list, abilityPriority);
				this.AddQueuedAction(action);
				flag = true;
			}
		}
		// rogues
		//this.SendExecutionStartMessage();
		if (!flag)
		{
			this.EndExecutingEffects();
		}
	}

	public void InitForAbilityAndMoveExecution(bool constructActionsFromStoredRequests, ActorData contextActor)
	{
		this.NextActionIndex = 0;
		if (constructActionsFromStoredRequests)
		{
			if (this.m_actorsWithActions.Count > 0)
			{
				Log.Error("Has uncleared list of relevant actors when initializing for execution");
				// rogues
				//this.SendExecutionDoneMessages();
				this.m_actorsWithActions.Clear();
			}
			this.ClearQueuedActions();
			List<AbilityRequest> allStoredAbilityRequests = ServerActionBuffer.Get().GetAllStoredAbilityRequests();
			for (int i = 0; i < 7; i++)
			{
				AbilityPriority abilityPriority = (AbilityPriority)i;
				List<AbilityRequest> list = new List<AbilityRequest>();
				for (int j = 0; j < allStoredAbilityRequests.Count; j++)
				{
					AbilityRequest abilityRequest = allStoredAbilityRequests[j];
					if (abilityRequest.m_ability != null && abilityRequest.m_ability.GetRunPriority() == abilityPriority && (contextActor == null || abilityRequest.m_caster == contextActor))
					{
						list.Add(abilityRequest);
						if (!this.m_actorsWithActions.Contains(abilityRequest.m_caster))
						{
							this.m_actorsWithActions.Add(abilityRequest.m_caster);
						}
					}
				}
				if (list.Count > 0)
				{
					PlayerAction_Ability action = new PlayerAction_Ability(list, abilityPriority);
					this.AddQueuedAction(action);
				}
			}
			foreach (MovementRequest movementRequest in ServerActionBuffer.Get().GetAllStoredMovementRequests())
			{
				if (movementRequest.m_targetSquare != null)
				{
					if (contextActor == null || contextActor == movementRequest.m_actor)
					{
						PlayerAction_Movement action2 = new PlayerAction_Movement(new List<MovementRequest>
						{
							movementRequest
						});
						this.AddQueuedAction(action2);
						if (!this.m_actorsWithActions.Contains(movementRequest.m_actor))
						{
							this.m_actorsWithActions.Add(movementRequest.m_actor);
						}
					}
				}
				else
				{
					Log.Error("Move request has no target square");
				}
			}
			// rogues
			//this.SendExecutionStartMessage();
		}
	}

	public void InitForGroupMoveExecution(List<ActorData> actors)
	{
		this.NextActionIndex = 0;
		if (this.m_actorsWithActions.Count > 0)
		{
			Log.Error("Has uncleared list of relevant actors when initializing for execution");
			// rogues
			//this.SendExecutionDoneMessages();
			this.m_actorsWithActions.Clear();
		}
		this.ClearQueuedActions();
		List<MovementRequest> list = new List<MovementRequest>();
		foreach (MovementRequest movementRequest in ServerActionBuffer.Get().GetAllStoredMovementRequests())
		{
			if (movementRequest.m_targetSquare != null)
			{
				if (actors.Contains(movementRequest.m_actor))
				{
					list.Add(movementRequest);
					if (!this.m_actorsWithActions.Contains(movementRequest.m_actor))
					{
						this.m_actorsWithActions.Add(movementRequest.m_actor);
					}
				}
			}
			else
			{
				Log.Error("Move request has no target square");
			}
		}
		PlayerAction_Movement action = new PlayerAction_Movement(list);
		this.AddQueuedAction(action);
		// rogues
		//this.SendExecutionStartMessage();
	}

	public void IncrementNextActionIndex()
	{
		this.NextActionIndex++;
	}

	public PlayerAction GetNextActionToExecute()
	{
		if (this.NextActionIndex >= 0 && this.NextActionIndex < this.m_queuedActions.Count)
		{
			return this.m_queuedActions[this.NextActionIndex];
		}
		return null;
	}
}
#endif
