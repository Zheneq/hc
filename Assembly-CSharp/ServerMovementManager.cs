// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

// server-only, empty in reactor (save for MovementType and empty serialization)
public class ServerMovementManager : NetworkBehaviour
{
	public enum MovementType
	{
		None,
		Evade,
		Knockback,
		NormalMovement_NonChase,
		NormalMovement_Chase
	}

#if SERVER
	private static ServerMovementManager s_instance;

	private bool m_waitingOnClients;
	private MovementType m_currentMovementType;
	private MovementCollection m_currentMovementCollection;
	private List<ActorData> m_playersStillResolving;
	private List<ActorData> m_playersDoneResolving;

	private const float c_fractionOfClientsDoneBeforeStartingFailsafe = 0.3f;
	private const float c_someClientsDoneFailsafeTime_hurrySlowClients = 2f;
	private const float c_someClientsDoneFailsafeTime_forceCompletePhase = 4f;
	private float m_failsafeTime_hurrySlowClients = -1f;
	private float m_failsafeTime_forceCompletePhase = -1f;
	private float m_generalFailsafeDuration = -1f;
	private float m_failsafeTime_general = -1f;

	public static ServerMovementManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		m_playersStillResolving = new List<ActorData>();
		m_playersDoneResolving = new List<ActorData>();
		m_currentMovementType = MovementType.None;
		m_currentMovementCollection = null;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	[Server]
	public override void OnStartServer()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ServerMovementManager::OnStartServer()' called on client");
			return;
		}
		// custom
		NetworkServer.RegisterHandler((short)MyMsgType.ClientMovementPhaseCompleted, MsgClientMovementCompleted_StaticHandler);
		// rogues
		//NetworkServer.RegisterHandler<ClientMovementPhaseCompleted>(new Action<NetworkConnection, ClientMovementPhaseCompleted>(MsgClientMovementCompleted_StaticHandler));
	}

	[Server]
	public override void OnNetworkDestroy()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ServerMovementManager::OnNetworkDestroy()' called on client");
			return;
		}
		// custom
		NetworkServer.UnregisterHandler((short)MyMsgType.ClientMovementPhaseCompleted);
		//rogues
		//NetworkServer.UnregisterHandler<ClientMovementPhaseCompleted>();
	}

	// custom
	[Server]
	private static void MsgClientMovementCompleted_StaticHandler(NetworkMessage message)
	{
		// TODO LOW check serialization is the same as in AR
		ClientMovementPhaseCompleted msg = message.ReadMessage<ClientMovementPhaseCompleted>();
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ServerMovementManager::MsgClientMovementCompleted_StaticHandler(Mirror.NetworkConnection,ClientMovementPhaseCompleted)' called on client");
			return;
		}
		Get().MsgClientMovementCompleted(msg);
	}

	// rogues
	//[Server]
	//private static void MsgClientMovementCompleted_StaticHandler(NetworkConnection conn, ClientMovementPhaseCompleted msg)
	//{
	//	if (!NetworkServer.active)
	//	{
	//		Debug.LogWarning("[Server] function 'System.Void ServerMovementManager::MsgClientMovementCompleted_StaticHandler(Mirror.NetworkConnection,ClientMovementPhaseCompleted)' called on client");
	//		return;
	//	}
	//	Get().MsgClientMovementCompleted(msg);
	//}

	public bool WaitingOnClients
	{
		get
		{
			return m_waitingOnClients;
		}
		private set
		{
			if (m_waitingOnClients != value)
			{
				m_waitingOnClients = value;
			}
		}
	}

	public bool IsKnockbackMovementCurrentlyHappening()
	{
		return m_currentMovementType == MovementType.Knockback && WaitingOnClients;
	}

	[Server]
	private void MsgClientMovementCompleted(ClientMovementPhaseCompleted msg)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ServerMovementManager::MsgClientMovementCompleted(ClientMovementPhaseCompleted)' called on client");
			return;
		}
		int actorIndex = msg.m_actorIndex;
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
		bool asFailsafe = msg.m_asFailsafe;
		if (!WaitingOnClients)
		{
			Log.Warning("ServerMovementManager received ClientMovementPhaseCompleted message, but the server isn't waiting for clients.");
			return;
		}
		if (actorData == null)
		{
			Log.Warning($"ServerMovementManager received ClientMovementPhaseCompleted message for actor index {actorIndex}, but that actor is null.  Ignoring...");
			return;
		}
		if (!m_playersStillResolving.Contains(actorData))
		{
			Log.Warning($"ServerMovementManager received ClientMovementPhaseCompleted message for player {actorData.DebugNameString()}, but that player isn't in m_playersStillResolvingAbilityPhase.  Ignoring...");
			return;
		}
		if (asFailsafe)
		{
			Log.Warning($"ServerMovementManager received ClientMovementPhaseCompleted message for player {actorData.DebugNameString()}, but it was sent as a failsafe.");
		}
		m_playersStillResolving.Remove(actorData);
		m_playersDoneResolving.Add(actorData);
		if (m_playersStillResolving.Count == 0)
		{
			ConcludeMovement();
			return;
		}
		ConsiderFailsafe();
	}

	public void ResetPlayersStillResolving()
	{
		m_playersStillResolving.Clear();
		m_playersDoneResolving.Clear();
		m_failsafeTime_hurrySlowClients = -1f;
		m_failsafeTime_forceCompletePhase = -1f;
		foreach (GameObject gameObject in GameFlowData.Get().GetPlayers())
		{
			ActorData component = gameObject.GetComponent<ActorData>();
			if (component != null && PlayerHasActiveClient(component))
			{
				m_playersStillResolving.Add(component);
			}
		}
	}

	public void RemoveStationaryMovementInstances(ref MovementCollection movementCollection)
	{
		List<MovementInstance> list = new List<MovementInstance>();
		foreach (MovementInstance movementInstance in movementCollection.m_movementInstances)
		{
			if (movementInstance.m_path == null)
			{
				Debug.LogWarning(string.Format("MovementInstance has a null path, but it should have been stabilized.\n\tActor = {0};\n\tChasing = {1}.", movementInstance.m_mover.DebugNameString(), movementInstance.m_wasChase.ToString()));
				list.Add(movementInstance);
			}
			else if (movementInstance.m_path.next == null)
			{
				list.Add(movementInstance);
			}
		}
		foreach (MovementInstance movementInstance2 in list)
		{
			if (movementCollection.m_movementStage == MovementStage.Normal)
			{
				Debug.Log(string.Format("ServerMovementManager encountered a stationary movement instance for normal movement for actor {0}.  Resolving and removing it...", movementInstance2.m_mover.DebugNameString()));
				BoardSquare square = movementInstance2.m_path.GetPathEndpoint().square;
				ProcessMovementResolveForActor(movementInstance2.m_mover, square, false);
			}
			movementCollection.m_movementInstances.Remove(movementInstance2);
		}
	}

	public void ServerMovementManager_OnMovementStart(MovementCollection movementCollection, MovementType movementType)
	{
		ResetPlayersStillResolving();
		RemoveStationaryMovementInstances(ref movementCollection);
		if (movementCollection.m_movementInstances.Count > 0)
		{
			m_currentMovementType = movementType;
			m_currentMovementCollection = movementCollection;
			SendMovementStartingMessageToClients(movementCollection);
			m_waitingOnClients = true;
			if (m_playersStillResolving.Count == 0)
			{
				m_generalFailsafeDuration = -1f;
				m_failsafeTime_general = -1f;
				float num = 5f;
				base.Invoke("SimulateClientPhaseResolution", num);
				return;
			}
			float num2 = 0f;
			for (int i = 0; i < movementCollection.m_movementInstances.Count; i++)
			{
				BoardSquarePathInfo path = movementCollection.m_movementInstances[i].m_path;
				num2 = Mathf.Max(num2, path.FindMoveCostToEnd());
			}
			m_generalFailsafeDuration = 3f + num2;
			if (movementType == MovementType.NormalMovement_Chase || movementType == MovementType.NormalMovement_NonChase)
			{
				m_failsafeTime_general = Time.time + m_generalFailsafeDuration;
				return;
			}
		}
		else
		{
			m_currentMovementType = MovementType.None;
			m_currentMovementCollection = null;
			m_waitingOnClients = false;

			// rogues
			//PveLog.DebugLog("Concluding Movement due to no movement in collection", null);

			ConcludeMovement();
		}
	}

	public void SendMovementStartingMessageToClients(MovementCollection movementCollection)
	{
		// TODO LOW check serialization is the same as in AR
		ServerMovementStarting serverMovementStarting = new ServerMovementStarting
		{
			m_actorIndices = new List<int>(movementCollection.m_movementInstances.Count),
			m_doomed = new List<bool>(movementCollection.m_movementInstances.Count)
		};
		sbyte currentMovementType = (sbyte)m_currentMovementType;
		serverMovementStarting.m_currentMovementType = currentMovementType;
		foreach (MovementInstance move in movementCollection.m_movementInstances)
		{
			int actorIndex = move.m_mover.ActorIndex;
			bool item = move.m_path.WillDieAtEnd();
			serverMovementStarting.m_actorIndices.Add(actorIndex);
			serverMovementStarting.m_doomed.Add(item);
		}
		if (ClientAbilityResults.DebugTraceOn || ClientAbilityResults.DebugSerializeSizeOn)
		{
			Log.Warning(ClientAbilityResults.s_clientResolutionNetMsgHeader + "Sending movement starting to clients");
		}

		// custom
		NetworkServer.SendByChannelToAll((short)MyMsgType.ServerMovementStarting, serverMovementStarting, (int)NetworkChannelId.StartResolutionPhase);
		// rogues
		//NetworkServer.SendToAll<ServerMovementStarting>(serverMovementStarting, 3);
	}

	private void Update()
	{
		if (m_failsafeTime_forceCompletePhase > 0f && m_failsafeTime_forceCompletePhase <= Time.time)
		{
			OnFailsafe_ForceCompleteMovement();
			return;
		}
		if (m_failsafeTime_hurrySlowClients > 0f && m_failsafeTime_hurrySlowClients <= Time.time)
		{
			OnFailsafe_HurrySlowClients();
			return;
		}
		if (m_failsafeTime_general > 0f && m_failsafeTime_general <= Time.time)
		{
			OnGeneralFailsafe();
		}
	}

	public void OnActorDisconnected()
	{
		Log.Info("ServerMovementManager handling actor disconnect");
		List<ActorData> list = new List<ActorData>();
		List<ActorData> list2 = new List<ActorData>();
		foreach (ActorData actorData in m_playersStillResolving)
		{
			if (!PlayerHasActiveClient(actorData))
			{
				list.Add(actorData);
			}
		}
		foreach (ActorData actorData2 in m_playersDoneResolving)
		{
			if (!PlayerHasActiveClient(actorData2))
			{
				list2.Add(actorData2);
			}
		}
		foreach (ActorData item in list)
		{
			m_playersStillResolving.Remove(item);
		}
		foreach (ActorData item2 in list2)
		{
			m_playersDoneResolving.Remove(item2);
		}
		if (m_playersStillResolving.Count == 0)
		{
			ConcludeMovement();
			return;
		}
		ConsiderFailsafe();
	}

	private bool PlayerHasActiveClient(ActorData playerActor)
	{
		PlayerDetails playerDetails;
		if (!GameFlow.Get().playerDetails.TryGetValue(playerActor.PlayerData.GetPlayer(), out playerDetails))
		{
			return false;
		}
		bool flag = GameplayUtils.IsHumanControlled(playerActor);
		bool isConnected = playerDetails.IsConnected;
		bool flag2 = !playerActor.PlayerData.IsReconnecting();
		return flag && isConnected && flag2;
	}

	private void SimulateClientPhaseResolution()
	{
		ConcludeMovement();
	}

	private void ConcludeMovement()
	{
		if (WaitingOnClients)
		{
			m_waitingOnClients = false;
			bool beingKnockedBack = m_currentMovementType == MovementType.Knockback;
			if (m_currentMovementCollection != null)
			{
				for (int i = 0; i < m_currentMovementCollection.m_movementInstances.Count; i++)
				{
					ActorData mover = m_currentMovementCollection.m_movementInstances[i].m_mover;
					BoardSquare square = m_currentMovementCollection.m_movementInstances[i].m_path.GetPathEndpoint().square;
					ProcessMovementResolveForActor(mover, square, beingKnockedBack);
				}
			}
			m_currentMovementType = MovementType.None;
			m_currentMovementCollection = null;
		}
		else
		{
			m_waitingOnClients = false;
		}
		m_failsafeTime_hurrySlowClients = -1f;
		m_failsafeTime_forceCompletePhase = -1f;
		m_failsafeTime_general = -1f;
		m_generalFailsafeDuration = -1f;

		// rogues
		//ServerActionBuffer.Get().GetPlayerActionFSM().OnMoveCompleteMsgReceived();
	}

	private void ProcessMovementResolveForActor(ActorData actor, BoardSquare goalSquare, bool beingKnockedBack)
	{
		ActorTurnSM actorTurnSM = actor.GetActorTurnSM();
		if (ServerActionBuffer.Get().HasUnresolvedMovementRequest(actor))
		{
			TurnStateEnum currentState = actorTurnSM.CurrentState;
		}
	}

	public void OnKnockbackHitsConcluded()
	{
		if (m_generalFailsafeDuration >= 0f)
		{
			m_failsafeTime_general = Time.time + m_generalFailsafeDuration;
		}
	}

	private void ConsiderFailsafe()
	{
		if (m_failsafeTime_forceCompletePhase < 0f)
		{
			int num = m_playersDoneResolving.Count + m_playersStillResolving.Count;
			if (num > 0)
			{
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				for (int i = 0; i < m_playersStillResolving.Count; i++)
				{
					if (m_playersStillResolving[i].GetTeam() == Team.TeamA)
					{
						num3++;
						num4++;
					}
					else if (m_playersStillResolving[i].GetTeam() == Team.TeamB)
					{
						num6++;
						num7++;
					}
				}
				for (int j = 0; j < m_playersDoneResolving.Count; j++)
				{
					if (m_playersDoneResolving[j].GetTeam() == Team.TeamA)
					{
						num2++;
						num4++;
					}
					else if (m_playersDoneResolving[j].GetTeam() == Team.TeamB)
					{
						num5++;
						num7++;
					}
				}
				bool flag = IsActorGroupDoneResolving(m_playersDoneResolving.Count, num, 0.3f);
				bool flag2 = IsActorGroupDoneResolving(num2, num4, 0.3f);
				bool flag3 = IsActorGroupDoneResolving(num5, num7, 0.3f);
				if (flag && flag2 && flag3)
				{
					m_failsafeTime_hurrySlowClients = Time.time + 2f;
					m_failsafeTime_forceCompletePhase = Time.time + 4f;
				}
			}
		}
	}

	private bool IsActorGroupDoneResolving(int numDoneResolving, int numTotalResolving, float fractionThreshold)
	{
		return numTotalResolving <= 0 || (float)numDoneResolving / (float)numTotalResolving >= fractionThreshold;
	}

	public void OnFailsafe(bool forSomeClientFailsafe = true)
	{
		if (forSomeClientFailsafe)
		{
			Log.Warning("ServerMovementManager triggered the some-clients-done failsafe.{0}", new object[]
			{
				GetResolutionStateLogString()
			});
		}
		ConcludeMovement();
	}

	public void OnFailsafe_HurrySlowClients()
	{
		m_failsafeTime_hurrySlowClients = -1f;
		Log.Warning("ServerMovementManager triggered the some-clients-done failsafe, and is sending 'hurry up' messages to them.{0}", new object[]
		{
			GetResolutionStateLogString()
		});
		// TODO LOW check serialization is the same as in AR
		Failsafe_HurryMovementPhase failsafe_HurryMovementPhase = new Failsafe_HurryMovementPhase();
		failsafe_HurryMovementPhase.m_currentTurn = GameFlowData.Get().CurrentTurn;
		failsafe_HurryMovementPhase.m_actorsNotDone = new List<int>();
		for (int i = 0; i < m_playersStillResolving.Count; i++)
		{
			failsafe_HurryMovementPhase.m_actorsNotDone.Add(m_playersStillResolving[i].ActorIndex);
		}
		// custom
		NetworkServer.SendByChannelToAll((short)MyMsgType.Failsafe_HurryMovementPhase, failsafe_HurryMovementPhase, (int)NetworkChannelId.StartResolutionPhase);
		// rogues
		//NetworkServer.SendToAll<Failsafe_HurryMovementPhase>(failsafe_HurryMovementPhase, 3);
	}

	public void OnFailsafe_ForceCompleteMovement()
	{
		Log.Warning("ServerMovementManager failed to hurry the slow clients in time, and triggered the force-complete-movement failsafe.{0}", new object[]
		{
			GetResolutionStateLogString()
		});
		ConcludeMovement();
	}

	public void OnGeneralFailsafe()
	{
		Log.Warning("ServerMovementManager waited a long time after movement started, but still isn't finished.  Triggering the general failsafe.{0}", new object[]
		{
			GetResolutionStateLogString()
		});
		ConcludeMovement();
	}

	public string GetResolutionStateLogString()
	{
		string text = "\n\tTurn = " + GameFlowData.Get().CurrentTurn.ToString() + ";";
		text = text + "\n\tMovementType = " + m_currentMovementType.ToString() + ";";
		text = text + "\n\tWaiting for clients = " + WaitingOnClients.ToString() + ";";
		text = string.Concat(new object[]
		{
			text,
			"\n\tGeneral Failsafe Time = ",
			m_failsafeTime_general.ToString(),
			" (current time = ",
			Time.time,
			");"
		});
		text += "\n\tClients not done resolving: ";
		for (int i = 0; i < m_playersStillResolving.Count; i++)
		{
			text = text + "\n\t\t" + m_playersStillResolving[i].DebugNameString();
		}
		return text;
	}

	//private void MirrorProcessed()
	//{
	//}
#endif

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
