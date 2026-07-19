// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// server-only -- empty in reactor
public class BotManager : MonoBehaviour
{
#if SERVER

	public class PendingBotMovement
	{
		public int currentTurn;
		public ActorData actorData;
		public BoardSquare destination;
		public float optimalRange;
	}

	public enum PveBotManagerState
	{
		DecidingActions,
		RequestingAction,
		// WaitingForActionsToFinish,  // rogues
		BotUpdateEnded
	}
	
	private static BotManager s_instance;
	
	private string[] m_botNames;
	private List<Player> m_botAgents;
	public List<PendingBotMovement> m_pendingBotMovement;
	private PveBotManagerState m_botManagerState;
	private List<ActorData> m_pveBotsToProcess = new List<ActorData>();
	private int m_pveThinkingBotIdx;
	private bool m_currentBotDecidedMove;
	private bool m_currentBotDecidedAbilities;
	private int m_currentBotAbilityDecisionsMade;
	private ActorData m_pendingActorToExecute;
	private bool m_calledOnUpdateEnded;

	public static BotManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		Log.Info("BotManager::Awake"); // custom log
		s_instance = this;
		m_botAgents = new List<Player>();
		if (m_botNames == null || m_botNames.Length == 0)
		{
			m_botNames = new string[10];
			m_botNames[0] = "RockMan";
			m_botNames[1] = "ProtoMan";
			m_botNames[2] = "MetalMan";
			m_botNames[3] = "FlashMan";
			m_botNames[4] = "QuickMan";
			m_botNames[5] = "HeatMan";
			m_botNames[6] = "WoodMan";
			m_botNames[7] = "AirMan";
			m_botNames[8] = "CrashMan";
			m_botNames[9] = "ElecMan";
		}
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public void OnTurnEnd()
	{
		if (m_pendingBotMovement != null)
		{
			m_pendingBotMovement.Clear();
		}
	}

	public void AddExistingBot(Player newBot)
	{
		Log.Info("BotManager::AddExistingBot"); // custom log
		m_botAgents.Add(newBot);
	}

	public ActorData GetCurrentBotForAI()
	{
		if (m_pveThinkingBotIdx >= 0 && m_pveThinkingBotIdx < m_pveBotsToProcess.Count)
		{
			return m_pveBotsToProcess[m_pveThinkingBotIdx];
		}
		return null;
	}

	public void BotAIAbilitySelected(ActorData actorData, BoardSquare dashTarget, float optimalRange)
	{
		// int currentTurn = GameFlowData.Get().CurrentTurn;
		// if (m_pendingBotMovement == null)
		// {
		// 	m_pendingBotMovement = new List<PendingBotMovement>();
		// }
		// m_pendingBotMovement.Clear();
	}

	// custom
	public bool IsDestinationSelected(ActorData actorData, BoardSquare destination)
	{
		return m_pendingBotMovement.Any(pendingBotMovement
			=> pendingBotMovement.actorData.GetTeam() == actorData.GetTeam()
			   && pendingBotMovement.destination == destination);
	}
	// rogues
	// public bool IsDestinationSelected(BoardSquare boardSquare)
	// {
	// 	return m_pendingBotMovement.Any(x => x.destination == boardSquare);
	// }

	// custom
	public void SelectDestination(ActorData actorData, BoardSquare destination)
	{
		foreach (PendingBotMovement pendingBotMovement in m_pendingBotMovement)
		{
			if (pendingBotMovement.actorData == actorData)
			{
				pendingBotMovement.destination = destination;
				pendingBotMovement.currentTurn = GameFlowData.Get().CurrentTurn;
				return;
			}
		}
		m_pendingBotMovement.Add(new PendingBotMovement
		{
			actorData = actorData,
			currentTurn = GameFlowData.Get().CurrentTurn,
			destination = destination,
			optimalRange = actorData.GetComponent<NPCBrain_Adaptive>()?.m_optimalRange ?? 0
		});
	}
	// rogues
	// public void SelectDestination(ActorData actorData, BoardSquare destination)
	// {
	// 	foreach (PendingBotMovement pendingBotMovement in m_pendingBotMovement)
	// 	{
	// 		if (pendingBotMovement.actorData == actorData)
	// 		{
	// 			pendingBotMovement.destination = destination;
	// 		}
	// 	}
	// }

	public BoardSquare GetPendingDestinationOrCurrentSquare(ActorData actorData, ActorData requester) // custom requester
	{
		// custom
		if (actorData.GetTeam() != requester.GetTeam()) {
			return actorData.GetCurrentBoardSquare();
		}
		// end custom
		
		foreach (PendingBotMovement pendingBotMovement in m_pendingBotMovement)
		{
			if (pendingBotMovement.actorData == actorData)
			{
				if (pendingBotMovement.destination != null)
				{
					return pendingBotMovement.destination;
				}
				break;
			}
		}
		return actorData.GetCurrentBoardSquare();
	}

	private void Update()
	{
		// this.CheckForAlert();  // rogues
		UpdateStates_OneBotAtATime();  // TODO BOTS call it on turn start instead?
	}

	// rogues
	// public void AlertTeam(Team alertedTeam)
	// {
	// 	foreach (ActorData actorData in GameFlowData.Get().GetAllActorsOnTeam(alertedTeam))
	// 	{
	// 		if (actorData.HasBotController)
	// 		{
	// 			actorData.Alerted = true;
	// 		}
	// 	}
	// }

	// rogues
	// public void CheckForAlertForTeam(Team team)
	// {
	// 	foreach (ActorData actorData in GameFlowData.Get().GetAllActorsOnTeam(team))
	// 	{
	// 		bool flag = false;
	// 		if (actorData.HasBotController && !actorData.IsDead())
	// 		{
	// 			actorData.GetComponent<BotController>();
	// 			if (actorData.HasBotController)
	// 			{
	// 				if (actorData.Alerted)
	// 				{
	// 					flag = true;
	// 				}
	// 				else
	// 				{
	// 					BoardSquare travelBoardSquare = actorData.GetTravelBoardSquare();
	// 					if (travelBoardSquare != null)
	// 					{
	// 						foreach (ActorData actorData2 in actorData.GetOtherTeams().SelectMany((Team otherTeam) => GameFlowData.Get().GetAllTeamMembers(otherTeam)).ToList<ActorData>())
	// 						{
	// 							if (!actorData2.IsInBrush())
	// 							{
	// 								BoardSquare travelBoardSquare2 = actorData2.GetTravelBoardSquare();
	// 								if (travelBoardSquare2 != null && travelBoardSquare.GetLOS(travelBoardSquare2.x, travelBoardSquare2.y) && travelBoardSquare.HorizontalDistanceInSquaresTo(travelBoardSquare2) < actorData.m_alertDist)
	// 								{
	// 									flag = true;
	// 									break;
	// 								}
	// 							}
	// 						}
	// 					}
	// 				}
	// 			}
	// 		}
	// 		if (flag)
	// 		{
	// 			this.AlertTeam(team);
	// 			break;
	// 		}
	// 	}
	// }
	
	// rogues?
	// public void CheckForAlert()
	// {
	// 	this.CheckForAlertForTeam(Team.TeamA);
	// 	this.CheckForAlertForTeam(Team.TeamB);
	// }

	private void UpdateStates_OneBotAtATime()
	{
		ServerActionBuffer serverActionBuffer = ServerActionBuffer.Get();
		
		// custom
		bool flag = GameFlowData.Get().IsInDecisionState();
		// rogues
		// PlayerActionStateMachine playerActionStateMachine = (serverActionBuffer != null) ? serverActionBuffer.GetPlayerActionFSM() : null;
		// bool flag = playerActionStateMachine != null && playerActionStateMachine.IsAcceptingInput() && !GameFlowData.Get().GetPause();
		
		if (m_botManagerState == PveBotManagerState.DecidingActions)
		{
			if (m_pveBotsToProcess.Count == 0 || m_pveThinkingBotIdx >= m_pveBotsToProcess.Count)
			{
				m_botManagerState = PveBotManagerState.BotUpdateEnded;
			}
		}
		else if (m_botManagerState == PveBotManagerState.RequestingAction)
		{
		// 	if (flag && m_pendingActorToExecute != null && serverActionBuffer != null && playerActionStateMachine != null)
		// 	{
		// 		if (serverActionBuffer.HasPendingAbilityRequest(m_pendingActorToExecute, true)
		// 		    || serverActionBuffer.HasPendingMovementRequest(m_pendingActorToExecute))
		// 		{
		// 			playerActionStateMachine.RunQueuedActionsFromActor(m_pendingActorToExecute);
		// 			m_botManagerState = PveBotManagerState.WaitingForActionsToFinish;
		// 		}
		// 		else
		// 		{
		// 			m_botManagerState = PveBotManagerState.WaitingForActionsToFinish;
		// 		}
		// 	}
		// 	if (m_pendingActorToExecute == null)
		// 	{
		// 		Log.Error("BotManager, null actor when waiting to request actions");
		// 		m_botManagerState = PveBotManagerState.WaitingForActionsToFinish;
		// 	}
		// 	if (serverActionBuffer == null)
		// 	{
		// 		Log.Error("BotManager, null ServerActionBuffer when waiting to request actions");
		// 		m_botManagerState = PveBotManagerState.WaitingForActionsToFinish;
		// 	}
		// }
		// else if (m_botManagerState == PveBotManagerState.WaitingForActionsToFinish)
		// {
			if (flag)
			{
				bool flag2 = false;
				if (m_pveThinkingBotIdx < m_pveBotsToProcess.Count)
				{
					BotController component = m_pveBotsToProcess[m_pveThinkingBotIdx].GetComponent<BotController>();
					if (m_currentBotDecidedMove && !m_currentBotDecidedAbilities)
					{
						component.StartDecideAbilities_FCFS();
					}
					else if (m_currentBotDecidedAbilities && !m_currentBotDecidedMove)
					{
						component.StartDecideMovement_FCFS();
					}
					else if (!m_currentBotDecidedAbilities && !m_currentBotDecidedMove)
					{
						component.StartDecideAbilities_FCFS();
					}
					else if (m_currentBotDecidedMove && m_currentBotDecidedAbilities)
					{
						flag2 = true;
					}
				}
				m_botManagerState = PveBotManagerState.DecidingActions;
				m_pendingActorToExecute = null;
				if (flag2)
				{
					m_pveThinkingBotIdx++;
					m_currentBotDecidedAbilities = false;
					m_currentBotDecidedMove = false;
					m_currentBotAbilityDecisionsMade = 0;
					if (m_pveThinkingBotIdx < m_pveBotsToProcess.Count)
					{
						BotController component2 = m_pveBotsToProcess[m_pveThinkingBotIdx].GetComponent<BotController>();
						if (component2.ShouldDoAbilityBeforeMovement())
						{
							component2.StartDecideAbilities_FCFS();
							return;
						}
						component2.StartDecideMovement_FCFS();
					}
				}
			}
		}
		else if (m_botManagerState == PveBotManagerState.BotUpdateEnded && !m_calledOnUpdateEnded)
		{
			OnTurnUpdateEnded();
			m_calledOnUpdateEnded = true;
		}
	}

	public void OnMovementDecided(ActorData forBot)
	{
		m_currentBotDecidedMove = true;
		StartRequestingAction();
	}

	public void OnAbilitiesDecided(ActorData forBot)
	{
		ActorTurnSM actorTurnSM = forBot.GetActorTurnSM();
		m_currentBotAbilityDecisionsMade++;
		
		// reactor
		m_currentBotDecidedAbilities = true;
		// rogues
		// this.m_currentBotDecidedAbilities = ((long)this.m_currentBotAbilityDecisionsMade >= (long)((ulong)actorTurnSM.NumAbilityActionsPerTurn));
		
		StartRequestingAction();
	}

	private void StartRequestingAction()
	{
		if (m_pveThinkingBotIdx < m_pveBotsToProcess.Count)
		{
			m_pendingActorToExecute = m_pveBotsToProcess[m_pveThinkingBotIdx];
		}
		else
		{
			m_pendingActorToExecute = null;
		}
		m_botManagerState = PveBotManagerState.RequestingAction;
	}

	private void UpdateBrainStack()
	{
		foreach (ActorData actorData in m_pveBotsToProcess)
		{
			BotController component = actorData.GetComponent<BotController>();
			if (component != null)
			{
				component.UpdateBrainStack();
			}
		}
	}

	private void ChoosePvEBotBrainParameters()
	{
		foreach (ActorData actorData in m_pveBotsToProcess)
		{
			BotController component = actorData.GetComponent<BotController>();
			if (component != null)
			{
				component.ChooseBrainParameters();
			}
		}
	}

	
	private void SortPvEBotsToProcess()
	{
		m_pveBotsToProcess.Sort(TurnPriorityComparison);
	}

	private int DecisionPriorityComparison(ActorData a1, ActorData a2)
	{
		int num = 1;
		if (a1 != null)
		{
			NPCBrain npcbrain = a1.GetComponent<BotController>().ActiveBrain();
			if (npcbrain != null)
			{
				num = npcbrain.m_decisionPriority;
			}
		}
		int value = 1;
		if (a2 != null)
		{
			NPCBrain npcbrain2 = a2.GetComponent<BotController>().ActiveBrain();
			if (npcbrain2 != null)
			{
				value = npcbrain2.m_decisionPriority;
			}
		}
		return num.CompareTo(value);
	}

	private int TurnPriorityComparison(ActorData a1, ActorData a2)
	{
		// custom
		CharacterRole value = a1 != null ? a1.GetCharacterResourceLink().m_characterRole : CharacterRole.None;
		return value.CompareTo(a2 != null ? a2.GetCharacterResourceLink().m_characterRole : CharacterRole.None);
		// rogues
		// int value = (a1 != null) ? a1.TurnPriority : 0;
		// return ((a2 != null) ? a2.TurnPriority : 0).CompareTo(value);
	}
	
	// custom
	public void OnTurnStart()
	{
		SetupBotsForDecision_FCFS();
	}

	public void SetupBotsForDecision_FCFS()
	{
		Log.Info("BotManager, on turn update started");  // custom log
		m_botManagerState = PveBotManagerState.DecidingActions;
		m_pveBotsToProcess.Clear();
		m_pveThinkingBotIdx = 0;
		m_currentBotDecidedMove = false;
		m_currentBotDecidedAbilities = false;
		m_currentBotAbilityDecisionsMade = 0;
		m_pendingActorToExecute = null;
		m_calledOnUpdateEnded = false;
		if (m_pendingBotMovement == null)
		{
			m_pendingBotMovement = new List<PendingBotMovement>();
		}
		m_pendingBotMovement.Clear();
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (actorData.HasBotController)  // actorData.GetTeam() == GameFlowData.Get().ActingTeam &&  in rogues
			{
				TurnStateEnum currentState = actorData.GetActorTurnSM().CurrentState;
				BotController component = actorData.GetComponent<BotController>();
				if (currentState == TurnStateEnum.PICKING_RESPAWN)
				{
					HandlePickRespawn(actorData);
				}
				else if ((currentState == TurnStateEnum.DECIDING || currentState == TurnStateEnum.RESPAWNING) && component != null)
				{
					m_pveBotsToProcess.Add(actorData);
				}
				else
				{
					actorData.GetActorTurnSM().RequestEndTurn();  // (true) in rogues
				}
			}
		}
		if (m_pveBotsToProcess.Count > 0)
		{
			UpdateBrainStack();
			ChoosePvEBotBrainParameters();
			SortPvEBotsToProcess();
			foreach (ActorData actorData2 in m_pveBotsToProcess)
			{
				BotController component2 = actorData2.GetComponent<BotController>();
				if (component2.ActiveBrain() is NPCBrain_Constructed)
				{
					(component2.ActiveBrain() as NPCBrain_Constructed).DecideBehavior();
				}
			}
			BotController component3 = m_pveBotsToProcess[0].GetComponent<BotController>();
			if (component3.ShouldDoAbilityBeforeMovement())
			{
				component3.StartDecideAbilities_FCFS();
				return;
			}
			component3.StartDecideMovement_FCFS();
		}
	}

	private void OnTurnUpdateEnded()
	{
		Log.Info("BotManager, on turn update ended");  // PveLog.DebugLog
		foreach (ActorData actorData in GameFlowData.Get().GetActors())
		{
			if (!actorData.IsDead()
			    // && actorData.GetTeam() == GameFlowData.Get().ActingTeam  // rogues
			    && actorData.HasBotController
			    && actorData.GetActorTurnSM().CurrentState == TurnStateEnum.DECIDING)
			{
				actorData.GetActorTurnSM().OnMessage(TurnMessage.DONE_BUTTON_CLICKED);
			}
		}
	}

	private void HandlePickRespawn(ActorData actorData)
	{
		if (actorData.respawnSquares.Count > 0)
		{
			NPCBrain component = actorData.GetComponent<NPCBrain_Adaptive>();
			if (component is NPCBrain_Adaptive)
			{
				((NPCBrain_Adaptive)component).PickRespawnSquare();
			}
			else
			{
				foreach (GameObject gameObject in GameFlowData.Get().GetPlayers())
				{
					ActorData component2 = gameObject.GetComponent<ActorData>();
					if (component2 != null && component2.IsDead() && component2 != actorData)
					{
						actorData.respawnSquares.Remove(component2.RespawnPickedPositionSquare);
					}
				}
				int index = (100 + GameFlowData.Get().CurrentTurn + actorData.respawnSquares[0].x + actorData.respawnSquares[0].y) % actorData.respawnSquares.Count;
				GetComponent<ServerActorController>().ProcessPickedRespawnRequest(actorData.respawnSquares[index].x, actorData.respawnSquares[index].y);
			}
		}
		actorData.GetActorTurnSM().RequestEndTurn();  // (true) in rogues
	}
#endif
}
