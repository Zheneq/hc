// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using Theatrics;
using UnityEngine;
using UnityEngine.Networking;

// server-only -- was empty in reactor (empty serialization only)
public class ServerResolutionManager : NetworkBehaviour
{
#if SERVER
	public enum ServerResolutionManagerState
	{
		WaitingForClients_AbilityPhase,
		Resolving_AbilityPhase,
		WaitingForClients_Movement,
		Resolving_Movement,
		WaitingForNextPhase
	}

	private static ServerResolutionManager s_instance;

	private ServerResolutionManagerState m_resolutionState = ServerResolutionManagerState.WaitingForNextPhase;
	private AbilityPriority m_currentAbilityPhase;
	private List<ActorData> m_playersStillResolving = new List<ActorData>();
	private List<ActorData> m_playersDoneResolving = new List<ActorData>();
	private Dictionary<ActorData, List<ActorData>> m_knockbackedActorToPlayersStillResolvingHits = new Dictionary<ActorData, List<ActorData>>();
	private List<ResolutionAction> m_currentResolutionActions = new List<ResolutionAction>();
	private List<ActorAnimation> m_animEntries = new List<ActorAnimation>();
	// TODO RESOLUTION unused
	private List<CastAction> m_currentCastActions = new List<CastAction>();

	private const float c_fractionOfClientsDoneBeforeStartingFailsafe = 0.3f;
	private const float c_someClientsDoneFailsafeTime_hurrySlowClients = 2f;
	private const float c_someClientsDoneFailsafeTime_forceCompletePhase = 4f;
	private float m_failsafeTime_hurrySlowClients = -1f;
	private float m_failsafeTime_forceCompletePhase = -1f;

	public static ServerResolutionManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		m_resolutionState = ServerResolutionManagerState.WaitingForNextPhase;
		m_currentAbilityPhase = AbilityPriority.INVALID;
		m_playersStillResolving = new List<ActorData>();
		m_playersDoneResolving = new List<ActorData>();
		m_knockbackedActorToPlayersStillResolvingHits = new Dictionary<ActorData, List<ActorData>>();
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
			Debug.LogWarning("[Server] function 'System.Void ServerResolutionManager::OnStartServer()' called on client");
			return;
		}

		// custom
		NetworkServer.RegisterHandler((short)MyMsgType.ClientResolutionPhaseCompleted, new NetworkMessageDelegate(MsgClientResolutionPhaseCompleted_StaticHandler));
		NetworkServer.RegisterHandler((short)MyMsgType.ResolveKnockbacksForActor, new NetworkMessageDelegate(MsgResolveKnockbacksForActor_StaticHandler));
		// rogues
		//NetworkServer.RegisterHandler<ClientResolutionPhaseCompleted>(new Action<NetworkConnection, ClientResolutionPhaseCompleted>(MsgClientResolutionPhaseCompleted_StaticHandler));
		//NetworkServer.RegisterHandler<ResolveKnockbacksForActor>(new Action<NetworkConnection, ResolveKnockbacksForActor>(MsgResolveKnockbacksForActor_StaticHandler));
	}

	[Server]
	public override void OnNetworkDestroy()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ServerResolutionManager::OnNetworkDestroy()' called on client");
			return;
		}
		// custom
		NetworkServer.UnregisterHandler((short)MyMsgType.ClientResolutionPhaseCompleted);
		NetworkServer.UnregisterHandler((short)MyMsgType.ResolveKnockbacksForActor);
		// rogues
		//NetworkServer.UnregisterHandler<ClientResolutionPhaseCompleted>();
		//NetworkServer.UnregisterHandler<ResolveKnockbacksForActor>();
	}

	// custom
	[Server]
	private static void MsgClientResolutionPhaseCompleted_StaticHandler(NetworkMessage msg)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ServerResolutionManager::MsgClientResolutionPhaseCompleted_StaticHandler(NetworkMessage)' called on client");
			return;
		}
		Get().MsgClientResolutionPhaseCompleted(msg.ReadMessage<ClientResolutionPhaseCompleted>());
	}

	// rogues
	//[Server]
	//private static void MsgClientResolutionPhaseCompleted_StaticHandler(NetworkConnection conn, ClientResolutionPhaseCompleted msg)
	//{
	//	if (!NetworkServer.active)
	//	{
	//		Debug.LogWarning("[Server] function 'System.Void ServerResolutionManager::MsgClientResolutionPhaseCompleted_StaticHandler(Mirror.NetworkConnection,ClientResolutionPhaseCompleted)' called on client");
	//		return;
	//	}
	//	Get().MsgClientResolutionPhaseCompleted(msg);
	//}

	// custom
	[Server]
	private static void MsgResolveKnockbacksForActor_StaticHandler(NetworkMessage msg)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ServerResolutionManager::MsgResolveKnockbacksForActor_StaticHandler(NetworkMessage)' called on client");
			return;
		}
		Get().MsgResolveKnockbacksForActor(msg.ReadMessage<ResolveKnockbacksForActor>());
	}

	// rogues
	//[Server]
	//private static void MsgResolveKnockbacksForActor_StaticHandler(NetworkConnection conn, ResolveKnockbacksForActor msg)
	//{
	//	if (!NetworkServer.active)
	//	{
	//		Debug.LogWarning("[Server] function 'System.Void ServerResolutionManager::MsgResolveKnockbacksForActor_StaticHandler(Mirror.NetworkConnection,ResolveKnockbacksForActor)' called on client");
	//		return;
	//	}
	//	Get().MsgResolveKnockbacksForActor(msg);
	//}

	public ServerResolutionManagerState GetCurrentState()
	{
		return m_resolutionState;
	}

	public bool ActionsDoneResolving()
	{
		//Log.Info($"RESOLUTIONSTATE {m_resolutionState}");
		return m_resolutionState == ServerResolutionManagerState.WaitingForNextPhase;
	}

	public string CurrentActionsDebugStr { get; set; }

	[Server]
	private void MsgClientResolutionPhaseCompleted(ClientResolutionPhaseCompleted msg)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ServerResolutionManager::MsgClientResolutionPhaseCompleted(ClientResolutionPhaseCompleted)' called on client");
			return;
		}
		AbilityPriority abilityPhase = (AbilityPriority)msg.m_abilityPhase;
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(msg.m_actorIndex);
		if (actorData == null)
		{
			Log.Warning("ServerResolutionManager received ClientResolutionPhaseCompleted message for actor index {0}, but that actor is null.  Ignoring...", new object[]
			{
				msg.m_actorIndex
			});
			return;
		}
		Log.Info($"MsgClientResolutionPhaseCompleted {actorData.DisplayName} {abilityPhase}");
		if (m_resolutionState != ServerResolutionManagerState.WaitingForClients_AbilityPhase && m_resolutionState != ServerResolutionManagerState.WaitingForClients_Movement)
		{
			Log.Warning("ServerResolutionManager received ClientResolutionPhaseCompleted message from {0} for phase '{1}', but the server isn't waiting for clients.\nServer: state = '{2}', phase = '{3}'", new object[]
			{
				actorData.DebugNameString(),
				abilityPhase,
				m_resolutionState,
				m_currentAbilityPhase
			});
			return;
		}
		if (m_resolutionState == ServerResolutionManagerState.WaitingForClients_AbilityPhase && abilityPhase != m_currentAbilityPhase)
		{
			Log.Warning("ServerResolutionManager received ClientResolutionPhaseCompleted message from {0} for phase '{1}', but the server is on phase '{2}'.  Ignoring...", new object[]
			{
				actorData.DebugNameString(),
				abilityPhase.ToString(),
				m_currentAbilityPhase.ToString()
			});
			return;
		}
		if (!m_playersStillResolving.Contains(actorData))
		{
			if (!msg.m_asResend)
			{
				Log.Warning("ServerResolutionManager received ClientResolutionPhaseCompleted message for player {0}, but that player isn't in m_playersStillResolvingAbilityPhase.  Ignoring...", new object[]
				{
					actorData.DebugNameString()
				});
				return;
			}
			Log.Warning("ServerResolutionManager received (re-sent) ClientResolutionPhaseCompleted message for player {0}, but that player isn't in m_playersStillResolvingAbilityPhase.  Probably already handled the first message, ignoring...", new object[]
			{
				actorData.DebugNameString()
			});
			return;
		}
		else
		{
			if (msg.m_asResend && msg.m_asFailsafe)
			{
				Log.Warning("ServerResolutionManager received re-sent ClientResolutionPhaseCompleted message for player {0}, sent this time as a failsafe.", new object[]
				{
					actorData.DebugNameString()
				});
			}
			else if (msg.m_asFailsafe)
			{
				Log.Warning("ServerResolutionManager received ClientResolutionPhaseCompleted message for player {0}, but it was sent as a failsafe.", new object[]
				{
					actorData.DebugNameString()
				});
			}
			m_playersStillResolving.Remove(actorData);
			m_playersDoneResolving.Add(actorData);
			if (m_playersStillResolving.Count == 0)
			{
				ConcludeResolutionPhase();
				return;
			}
			ConsiderFailsafe();
			return;
		}
	}

	[Server]
	private void MsgResolveKnockbacksForActor(ResolveKnockbacksForActor msg)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ServerResolutionManager::MsgResolveKnockbacksForActor(ResolveKnockbacksForActor)' called on client");
			return;
		}
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(msg.m_targetActorIndex);
		ActorData actorData2 = GameFlowData.Get().FindActorByActorIndex(msg.m_sourceActorIndex);
		if (m_currentAbilityPhase != AbilityPriority.Combat_Knockback)
		{
			Log.Warning("ServerResolutionManager received ResolveKnockbacksForActor message, but the server is on phase '{0}'.  Ignoring...", new object[]
			{
				m_currentAbilityPhase
			});
			return;
		}
		if (actorData == null)
		{
			Log.Warning("ServerResolutionManager received ResolveKnockbacksForActor message for knockbacked actor with index {0}, but that actor is null.  Ignoring...", new object[]
			{
				msg.m_targetActorIndex
			});
			return;
		}
		if (actorData2 == null)
		{
			Log.Warning("ServerResolutionManager received ResolveKnockbacksForActor message for (sending) actor index {0}, but that player's actor is null.  Ignoring...", new object[]
			{
				msg.m_sourceActorIndex
			});
			return;
		}
		if (m_knockbackedActorToPlayersStillResolvingHits == null)
		{
			Log.Warning("ServerResolutionManager received ResolveKnockbacksForActor message, but m_knockbackedActorToPlayersStillResolvingHits is null.  Ignoring...");
			return;
		}
		if (!m_knockbackedActorToPlayersStillResolvingHits.ContainsKey(actorData))
		{
			if (!actorData.GetActorStatus().IsKnockbackImmune(true))
			{
				Log.Warning("ServerResolutionManager received ResolveKnockbacksForActor message for actor {0}, but that actor isn't in m_knockbackedActorToPlayersStillResolvingHits.  Ignoring...", new object[]
				{
					actorData.DebugNameString()
				});
				return;
			}
		}
		else
		{
			if (m_knockbackedActorToPlayersStillResolvingHits[actorData] == null)
			{
				Log.Warning("ServerResolutionManager received ResolveKnockbacksForActor message for actor {0}, but that actor-key's value in m_knockbackedActorToPlayersStillResolvingHits is null.  Ignoring...", new object[]
				{
					actorData.DebugNameString()
				});
				return;
			}
			if (!m_knockbackedActorToPlayersStillResolvingHits[actorData].Contains(actorData2))
			{
				Log.Warning("ServerResolutionManager received ResolveKnockbacksForActor message for actor {0} from sender {1}, but the knockbacked actor's entry in m_knockbackedActorToPlayersStillResolvingHits doesn't contain the sender.  Ignoring...", new object[]
				{
					actorData.DebugNameString(),
					actorData2.DebugNameString()
				});
				return;
			}
			m_knockbackedActorToPlayersStillResolvingHits[actorData].Remove(actorData2);
			if (m_knockbackedActorToPlayersStillResolvingHits[actorData].Count == 0)
			{
				ServerKnockbackManager.Get().ResolveKnockbacksForActor(actorData);
				m_knockbackedActorToPlayersStillResolvingHits.Remove(actorData);
			}
		}
	}

	public void OnAbilityPhaseStart(AbilityPriority phase)
	{
		m_currentAbilityPhase = phase;
		ResetPlayersStillResolving();
		m_currentResolutionActions = BuildResolutionActionsForAbilityPhase(m_currentAbilityPhase);

		// custom
		TheatricsManager.Get().ResetTimeToTimeoutPhase();

		// Test
		//foreach (ResolutionAction action in m_currentResolutionActions)
		//{
		//    action.m_abilityResults.m_positionToHitResults.Add(Vector3.zero, new PositionHitResults(new PositionHitParameters(Vector3.zero)));
		//}

		m_currentCastActions = new List<CastAction>();
		CurrentActionsDebugStr = BuildDebugStringForActionList(m_currentResolutionActions);
		if (phase == AbilityPriority.Combat_Knockback)
		{
			InitKnockbackActors();
		}
		m_resolutionState = ServerResolutionManagerState.WaitingForClients_AbilityPhase;

		// custom
		//if (!NetworkClient.active)
		//{
		//	PlayerAction_Ability.InitializeTheatricsForPhaseActions(phase, list);
		//}
		if (phase == AbilityPriority.Evasion)
		{
			ServerEvadeManager evadeManager = ServerActionBuffer.Get().GetEvadeManager();
			evadeManager.UndoEvaderDestinationsSwap();
			if (evadeManager.HasEvades())
			{
				ServerActionBuffer.Get().ImmediateUpdateAllFogOfWar();
			}
			evadeManager.RunEvades();
		}
		// end custom

		SendPhaseResolutionActionsToClients();

		// custom
		Log.Info($"Resolved {m_currentAbilityPhase}: {CurrentActionsDebugStr}");
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

	public void InitKnockbackActors()
	{
		m_knockbackedActorToPlayersStillResolvingHits.Clear();
		List<ActorData> actorsBeingKnockedBack = ServerKnockbackManager.Get().GetActorsBeingKnockedBack();
		List<ActorData> list = new List<ActorData>();
		foreach (GameObject gameObject in GameFlowData.Get().GetPlayers())
		{
			ActorData component = gameObject.GetComponent<ActorData>();
			if (component != null && PlayerHasActiveClient(component))
			{
				list.Add(component);
			}
		}
		foreach (ActorData key in actorsBeingKnockedBack)
		{
			m_knockbackedActorToPlayersStillResolvingHits.Add(key, new List<ActorData>(list));
		}
	}

	public void OnNormalMovementStart()
	{
		m_currentAbilityPhase = AbilityPriority.INVALID;
		ResetPlayersStillResolving();
		m_currentResolutionActions = BuildResolutionActionsForNormalMovement();
		m_currentCastActions.Clear();
		m_animEntries = new List<ActorAnimation>();
		m_resolutionState = ServerResolutionManagerState.WaitingForClients_Movement;
		SendPhaseResolutionActionsToClients();
	}

	private void Update()
	{
		if (m_failsafeTime_forceCompletePhase > 0f && m_failsafeTime_forceCompletePhase <= Time.time)
		{
			OnFailsafe_ForceCompletePhase();
			return;
		}
		if (m_failsafeTime_hurrySlowClients > 0f && m_failsafeTime_hurrySlowClients <= Time.time)
		{
			OnFailsafe_HurrySlowClients();
		}
	}

	public void OnActorDisconnected()
	{
		Log.Info("ServerResolutionManager handling actor disconnect");
		List<ActorData> list = new List<ActorData>();
		List<ActorData> list2 = new List<ActorData>();
		foreach (ActorData actorData in m_playersStillResolving)
		{
			if (actorData != null && !PlayerHasActiveClient(actorData))
			{
				list.Add(actorData);
			}
		}
		foreach (ActorData actorData2 in m_playersDoneResolving)
		{
			if (actorData2 != null && !PlayerHasActiveClient(actorData2))
			{
				list2.Add(actorData2);
			}
		}
		AbilityPriority currentAbilityPhase = m_currentAbilityPhase;
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
			ConcludeResolutionPhase();
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

	public void SendPhaseResolutionActionsToClients()
	{
		sbyte b = (sbyte)m_currentResolutionActions.Count;
		if (b > 0)
		{
			SendStartResolutionMessageToClients();
			for (int i = 0; i < m_currentResolutionActions.Count; i++)
			{
				ResolutionAction action = m_currentResolutionActions[i];
				SendSinglePhaseResolutionActionToClients(action, i);
			}
			if (m_playersStillResolving.Count == 0)
			{
				float num;
				if (m_currentAbilityPhase == AbilityPriority.Combat_Knockback || m_currentAbilityPhase == AbilityPriority.Evasion)
				{
					num = 4f;
				}
				else
				{
					num = (float)(b * 2);
				}
				base.Invoke("SimulateClientPhaseResolution", num);
				return;
			}
		}
		else
		{
			ConcludeResolutionPhase();
		}
	}

	private void SimulateClientPhaseResolution()
	{
		foreach (ActorData actorData in m_knockbackedActorToPlayersStillResolvingHits.Keys.ToArray<ActorData>())
		{
			ServerKnockbackManager.Get().ResolveKnockbacksForActor(actorData);
			m_knockbackedActorToPlayersStillResolvingHits.Remove(actorData);
		}
		ConcludeResolutionPhase();
	}

	public void SendStartResolutionMessageToClients()
	{
		StartResolutionPhase startResolutionPhase = new StartResolutionPhase();
		startResolutionPhase.m_currentTurn = GameFlowData.Get().CurrentTurn;
		startResolutionPhase.m_currentAbilityPhaseSbyte = (sbyte)m_currentAbilityPhase;
		startResolutionPhase.m_currentResolutionActionsCountSbyte = (sbyte)m_currentResolutionActions.Count;
		startResolutionPhase.m_numAnimEntries = (sbyte)m_animEntries.Count;
		if (ClientAbilityResults.DebugTraceOn || ClientAbilityResults.DebugSerializeSizeOn)
		{
			Log.Warning(ClientAbilityResults.s_clientResolutionNetMsgHeader + "Sending phase resolution actions to clients for phase " + m_currentAbilityPhase.ToString() + ".");
		}
		// custom reactor
		NetworkServer.SendByChannelToAll((short)MyMsgType.StartResolutionPhase, startResolutionPhase, (int)NetworkChannelId.StartResolutionPhase);
		// rogues
		//NetworkServer.SendToAll<StartResolutionPhase>(startResolutionPhase, (int)NetworkChannelId.StartResolutionPhase);
	}

	public void SerializeCastActionsToClients(ref NetworkWriter writer, List<CastAction> actions)
	{
		sbyte b = (sbyte)m_currentCastActions.Count;
		writer.Write(b);
		for (int i = 0; i < b; i++)
		{
			m_currentCastActions[i].CastAction_SerializeToStream(ref writer);
		}
	}

	public void SendSinglePhaseResolutionActionToClients(ResolutionAction action, int actionIndex)
	{
		if (action != null)
		{
			SingleResolutionAction singleResolutionAction = new SingleResolutionAction();
			singleResolutionAction.m_turn = GameFlowData.Get().CurrentTurn;
			singleResolutionAction.m_abilityPhase = m_currentAbilityPhase;
			singleResolutionAction.m_actionServer = action;
			if (ClientAbilityResults.DebugTraceOn || ClientAbilityResults.DebugSerializeSizeOn)
			{
				Log.Warning(ClientAbilityResults.s_clientResolutionNetMsgHeader + "Sending single actions to clients for phase " + m_currentAbilityPhase.ToString() + ".");
			}
			// custom reactor
			NetworkServer.SendByChannelToAll((short)MyMsgType.SingleResolutionAction, singleResolutionAction, (int)NetworkChannelId.SingleResolutionAction);
			// rogues
			//NetworkServer.SendToAll<SingleResolutionAction>(singleResolutionAction, (int)NetworkChannelId.SingleResolutionAction);
		}
	}

	// seems to be rogues-specific, leaving an error message for now
	// TODO LOW check for usages?
	public void SendSingleTheatricsEntryToClients(ActorAnimation actorAnimEntry)
	{
		Log.Error("Called SendSingleTheatricsEntryToClients -- not implemented");
		//SingleTheatricsActorAnimation singleTheatricsActorAnimation = new SingleTheatricsActorAnimation
		//{
		//	m_actorAnimEntry = actorAnimEntry
		//};
		//// custom reactor
		//NetworkServer.SendByChannelToAll((short)MyMsgType.SingleTheatricsActorAnimation, singleTheatricsActorAnimation, (int)NetworkChannelId.SingleResolutionAction);
		//// rogues
		////NetworkServer.SendToAll<SingleTheatricsActorAnimation>(singleTheatricsActorAnimation, (int)NetworkChannelId.SingleResolutionAction);
	}

	private void ConcludeResolutionPhase()
	{
		// rogues
		//PveLog.DebugLog("ServerResolutionManager: Concluding phase " + m_currentAbilityPhase, "white");
		if (m_resolutionState == ServerResolutionManagerState.WaitingForClients_AbilityPhase)
		{
			m_resolutionState = ServerResolutionManagerState.Resolving_AbilityPhase;
			m_currentResolutionActions.Clear();
			m_currentCastActions.Clear();
			ServerActionBuffer.Get().ExecuteUnexecutedHits(m_currentAbilityPhase, false);
			if (m_currentAbilityPhase == AbilityPriority.Combat_Knockback)
			{
				foreach (ActorData actor in m_knockbackedActorToPlayersStillResolvingHits.Keys)
				{
					ServerKnockbackManager.Get().ResolveKnockbacksForActor(actor);
				}
				m_knockbackedActorToPlayersStillResolvingHits.Clear();
			}
			m_resolutionState = ServerResolutionManagerState.WaitingForNextPhase;
		}
		else if (m_resolutionState == ServerResolutionManagerState.WaitingForClients_Movement)
		{
			m_resolutionState = ServerResolutionManagerState.Resolving_Movement;
			m_currentResolutionActions.Clear();
			m_currentCastActions.Clear();
			Log.Info($"Resolving movement hits {ServerActionBuffer.Get().ActionPhase}"); // custom debug
			ServerActionBuffer.Get().ExecuteUnexecutedNormalMovementHits(false);
			m_resolutionState = ServerResolutionManagerState.WaitingForNextPhase;
		}
		else if (m_resolutionState != ServerResolutionManagerState.WaitingForNextPhase)
		{
			Log.Error("ServerResolutionManager trying to conclude resolution phase, but not in an expected state. Current state: {0}", new object[]
			{
				m_resolutionState.ToString()
			});
			m_resolutionState = ServerResolutionManagerState.WaitingForNextPhase;
		}
		m_failsafeTime_hurrySlowClients = -1f;
		m_failsafeTime_forceCompletePhase = -1f;

		// rogues
		//ServerActionBuffer.Get().GetPlayerActionFSM().OnAbilitiesCompleted(m_currentAbilityPhase);
	}

	private void ConsiderFailsafe()
	{
		if (m_failsafeTime_forceCompletePhase < 0f && (GameFlowData.Get() == null || !GameFlowData.Get().IsResolutionPaused()))
		{
			int num = m_playersDoneResolving.Count + m_playersStillResolving.Count;
			if (num > 0 && (float)m_playersDoneResolving.Count / (float)num >= 0.3f)
			{
				m_failsafeTime_hurrySlowClients = Time.time + 2f;
				m_failsafeTime_forceCompletePhase = Time.time + 4f;
			}
		}
	}

	public void OnFailsafe(bool forSomeClientFailsafe = true)
	{
		if (forSomeClientFailsafe)
		{
			Log.Warning("ServerResolutionManager triggered the some-clients-done failsafe.\n{0}", new object[]
			{
				GetResolutionStateLogString()
			});
		}
		if (m_currentAbilityPhase == AbilityPriority.Combat_Knockback)
		{
			foreach (ActorData actor in m_knockbackedActorToPlayersStillResolvingHits.Keys)
			{
				ServerKnockbackManager.Get().ResolveKnockbacksForActor(actor);
			}
			m_knockbackedActorToPlayersStillResolvingHits.Clear();
		}
		ConcludeResolutionPhase();
	}

	public void OnFailsafe_HurrySlowClients()
	{
		m_failsafeTime_hurrySlowClients = -1f;
		Log.Warning("ServerResolutionManager triggered the some-clients-done failsafe, and is sending 'hurry up' messages to them.\n{0}", new object[]
		{
			GetResolutionStateLogString()
		});
		Failsafe_HurryResolutionPhase failsafe_HurryResolutionPhase = new Failsafe_HurryResolutionPhase();
		failsafe_HurryResolutionPhase.m_currentTurn = GameFlowData.Get().CurrentTurn;
		failsafe_HurryResolutionPhase.m_currentAbilityPhaseSbyte = (sbyte)m_currentAbilityPhase;
		failsafe_HurryResolutionPhase.m_playersStillResolving = new List<int>(m_playersStillResolving.Count);
		for (int i = 0; i < m_playersStillResolving.Count; i++)
		{
			failsafe_HurryResolutionPhase.m_playersStillResolving.Add(m_playersStillResolving[i].ActorIndex);
		}
		// custom reactor
		NetworkServer.SendByChannelToAll((short)MyMsgType.Failsafe_HurryResolutionPhase, failsafe_HurryResolutionPhase, (int)NetworkChannelId.StartResolutionPhase);
		// rogues
		//NetworkServer.SendToAll<Failsafe_HurryResolutionPhase>(failsafe_HurryResolutionPhase, (int)NetworkChannelId.StartResolutionPhase);
	}

	public void OnFailsafe_ForceCompletePhase()
	{
		Log.Warning("ServerResolutionManager failed to hurry the slow clients in time, and triggered the force-complete-phase failsafe.\n{0}", new object[]
		{
			GetResolutionStateLogString()
		});
		if (m_currentAbilityPhase == AbilityPriority.Combat_Knockback)
		{
			foreach (ActorData actor in m_knockbackedActorToPlayersStillResolvingHits.Keys)
			{
				ServerKnockbackManager.Get().ResolveKnockbacksForActor(actor);
			}
			m_knockbackedActorToPlayersStillResolvingHits.Clear();
		}
		ConcludeResolutionPhase();
	}

	private string GetResolutionStateLogString()
	{
		string text = "";
		for (int i = 0; i < m_playersStillResolving.Count; i++)
		{
			text = text + "\n\t" + m_playersStillResolving[i].DebugNameString();
		}
		return string.Format("Phase = {0}, turn = {1}, state = {2}. Clients not done resolving: {3}", new object[]
		{
			m_currentAbilityPhase,
			GameFlowData.Get().CurrentTurn,
			m_resolutionState,
			text
		});
	}

	public List<ResolutionAction> BuildResolutionActionsForAbilityPhase(AbilityPriority phase)
	{
		if (phase == AbilityPriority.INVALID)
		{
			Log.Error("Calling BuildResolutionActionsForAbilityPhase for the 'INVALID' phase.");
			return null;
		}
		List<ResolutionAction> list = new List<ResolutionAction>();
		foreach (AbilityRequest abilityRequest in ServerActionBuffer.Get().GetAllStoredAbilityRequests())
		{
			if (abilityRequest.m_ability.RunPriority == phase)
			{
				ResolutionAction item = ResolutionAction.ConstructFromAbilityRequest(abilityRequest);
				list.Add(item);
			}
		}
		IEnumerable<global::Effect> source = ServerEffectManager.Get().GetAllActorEffects().SelectMany((KeyValuePair<ActorData, List<global::Effect>> kvp) => kvp.Value);
		foreach (global::Effect effect3 in source.Where((global::Effect effect) => effect.HasResolutionAction(phase)))
		{
			list.AddRange(ResolutionAction.ConstructFromEffect(effect3));
		}
		foreach (global::Effect effect2 in ServerEffectManager.Get().GetWorldEffects())
		{
			if (effect2.HasResolutionAction(phase))
			{
				list.AddRange(ResolutionAction.ConstructFromEffect(effect2));
			}
		}
		if (phase == AbilityPriority.Evasion)
		{
			BuildResolutionActionsForMovementStage(MovementStage.Evasion, ref list);
		}
		else if (phase == AbilityPriority.Combat_Knockback)
		{
			BuildResolutionActionsForMovementStage(MovementStage.Knockback, ref list);
		}
		return list;
	}

	// rogues
	// public void SendActionsToClients_FCFS(List<AbilityRequest> requests, List<ActorAnimation> animEntries, AbilityPriority phase)
	// {
	// 	m_currentAbilityPhase = phase;
	// 	ResetPlayersStillResolving();
	// 	m_currentResolutionActions = BuildResolutionActionsFromRequests(requests, phase);
	// 	m_currentCastActions = new List<CastAction>();
	// 	m_animEntries = animEntries;
	// 	CurrentActionsDebugStr = BuildDebugStringForActionList(m_currentResolutionActions);
	// 	if (phase == AbilityPriority.Combat_Knockback)
	// 	{
	// 		InitKnockbackActors();
	// 	}
	// 	m_resolutionState = ServerResolutionManagerState.WaitingForClients_AbilityPhase;
	// 	//SendPhaseResolutionActionsToClients();
	// }

	// rogues?
	// public void SendEffectActionsToClients_FCFS(List<EffectResults> requests, List<ActorAnimation> animEntries, AbilityPriority phase)
	// {
	// 	m_currentAbilityPhase = phase;
	// 	ResetPlayersStillResolving();
	// 	m_currentResolutionActions = new List<ResolutionAction>();
	// 	foreach (EffectResults effectResults in requests)
	// 	{
	// 		// rogues
	// 		//EffectSystem.Effect effect;
	// 		//if ((effect = (effectResults.Effect as EffectSystem.Effect)) != null)
	// 		//{
	// 		//	foreach (ResolutionAction resolutionAction in ResolutionAction.ConstructFromEffect(effect))
	// 		//	{
	// 		//		effect.pendingEffectResults.Remove(resolutionAction.EffectResults);
	// 		//		effect.executingEffectResults.Add(resolutionAction.EffectResults);
	// 		//		m_currentResolutionActions.Add(resolutionAction);
	// 		//	}
	// 		//	continue;
	// 		//}
	// 		m_currentResolutionActions.AddRange(ResolutionAction.ConstructFromEffect(effectResults.Effect));
	// 	}
	// 	m_currentCastActions = new List<CastAction>();
	// 	m_animEntries = animEntries;
	// 	CurrentActionsDebugStr = BuildDebugStringForActionList(m_currentResolutionActions);
	// 	if (phase == AbilityPriority.Combat_Knockback)
	// 	{
	// 		InitKnockbackActors();
	// 	}
	// 	m_resolutionState = ServerResolutionManagerState.WaitingForClients_AbilityPhase;
	// 	//SendPhaseResolutionActionsToClients();
	// }

	// rogues?
	// public List<ResolutionAction> BuildResolutionActionsFromRequests(List<AbilityRequest> requests, AbilityPriority phase)
	// {
	// 	List<ResolutionAction> list = new List<ResolutionAction>();
	// 	foreach (AbilityRequest request in requests)
	// 	{
	// 		ResolutionAction item = ResolutionAction.ConstructFromAbilityRequest(request);
	// 		list.Add(item);
	// 	}
	// 	if (phase == AbilityPriority.Evasion)
	// 	{
	// 		BuildResolutionActionsForMovementStage(MovementStage.Evasion, ref list);
	// 	}
	// 	else if (phase == AbilityPriority.Combat_Knockback)
	// 	{
	// 		BuildResolutionActionsForMovementStage(MovementStage.Knockback, ref list);
	// 	}
	// 	return list;
	// }

	public void SendPhaseResolutionActionsToClients_FCFS()
	{
		sbyte b = (sbyte)m_currentResolutionActions.Count;
		sbyte b2 = (sbyte)m_animEntries.Count;
		if (b > 0 || b2 > 0)
		{
			SendStartResolutionMessageToClients();
			for (int i = 0; i < m_animEntries.Count; i++)
			{
				SendSingleTheatricsEntryToClients(m_animEntries[i]);
			}
			for (int j = 0; j < m_currentResolutionActions.Count; j++)
			{
				ResolutionAction action = m_currentResolutionActions[j];
				SendSinglePhaseResolutionActionToClients(action, j);
			}
			if (m_playersStillResolving.Count == 0)
			{
				float num;
				if (m_currentAbilityPhase == AbilityPriority.Combat_Knockback || m_currentAbilityPhase == AbilityPriority.Evasion)
				{
					num = 4f;
				}
				else
				{
					num = (float)(b * 2);
				}
				base.Invoke("SimulateClientPhaseResolution", num);
				return;
			}
		}
		else
		{
			ConcludeResolutionPhase();
		}
	}

	public List<CastAction> BuildCastActionsForAbilityPhase(AbilityPriority phase)
	{
		if (phase == AbilityPriority.INVALID)
		{
			Log.Error("Calling BuildCastActionsForAbilityPhase for the 'INVALID' phase.");
			return null;
		}
		List<CastAction> list = new List<CastAction>();
		foreach (AbilityRequest abilityRequest in ServerActionBuffer.Get().GetAllStoredAbilityRequests())
		{
			if (abilityRequest.m_ability.RunPriority == phase)
			{
				int techPointRewardForInteraction = AbilityUtils.GetTechPointRewardForInteraction(abilityRequest.m_ability, AbilityInteractionType.Cast, true, false, false);
				int moddedCost = abilityRequest.m_ability.GetModdedCost();
				if (techPointRewardForInteraction > 0 || moddedCost > 0)
				{
					CastAction item = new CastAction(abilityRequest.m_caster, abilityRequest.m_ability, techPointRewardForInteraction, moddedCost);
					list.Add(item);
				}
			}
		}
		return list;
	}

	public string BuildDebugStringForActionList(List<ResolutionAction> actions)
	{
		string text = actions.Count + " resolution actions";
		for (int i = 0; i < actions.Count; i++)
		{
			ResolutionAction resolutionAction = actions[i];
			text = string.Concat(new object[]
			{
				text,
				"\n\t",
				i + 1,
				". ",
				resolutionAction.m_debugStr
			});
		}
		return text;
	}

	public List<ResolutionAction> BuildResolutionActionsForNormalMovement()
	{
		List<ResolutionAction> result = new List<ResolutionAction>();
		BuildResolutionActionsForMovementStage(MovementStage.Normal, ref result);
		return result;
	}

	private static void AddMovementResultsListToResolutionActionList(List<MovementResults> movementResults, ref List<ResolutionAction> actions)
	{
		if (movementResults != null)
		{
			foreach (MovementResults results in movementResults)
			{
				ResolutionAction item = ResolutionAction.ConstructFromMoveResults(results);
				actions.Add(item);
			}
		}
	}

	public void BuildResolutionActionsForMovementStage(MovementStage stage, ref List<ResolutionAction> actions)
	{
		foreach (KeyValuePair<ActorData, List<global::Effect>> keyValuePair in ServerEffectManager.Get().GetAllActorEffects())
		{
			foreach (global::Effect effect in keyValuePair.Value)
			{
				AddMovementResultsListToResolutionActionList(effect.GetMovementResultsForMovementStage(stage), ref actions);
			}
		}
		foreach (global::Effect effect2 in ServerEffectManager.Get().GetWorldEffects())
		{
			AddMovementResultsListToResolutionActionList(effect2.GetMovementResultsForMovementStage(stage), ref actions);
		}
		foreach (Barrier barrier in BarrierManager.Get().GetAllBarriers())
		{
			AddMovementResultsListToResolutionActionList(barrier.GetMovementResultsForMovementStage(stage), ref actions);
		}
		foreach (PowerUp.IPowerUpListener powerUpListener in PowerUpManager.Get().powerUpListeners)
		{
			if (powerUpListener != null)
			{
				PowerUp[] activePowerUps = powerUpListener.GetActivePowerUps();
				for (int i = 0; i < activePowerUps.Length; i++)
				{
					if (activePowerUps[i] != null && activePowerUps[i].boardSquare != null)
					{
						AddMovementResultsListToResolutionActionList(activePowerUps[i].GetMovementResultsForMovementStage(stage), ref actions);
					}
				}
			}
		}
		// TODO CTF CTC
		//if (CaptureTheFlag.Get() != null)
		//{
		//	AddMovementResultsListToResolutionActionList(CaptureTheFlag.Get().GetMovementResultsForMovementStage(stage), ref actions);
		//}
		//if (CollectTheCoins.Get() != null)
		//{
		//	AddMovementResultsListToResolutionActionList(CollectTheCoins.Get().GetMovementResultsForMovementStage(stage), ref actions);
		//}
	}

	public void SendNonResolutionActionToClients(MovementResults moveResults)
	{
		ResolutionAction item = ResolutionAction.ConstructFromMoveResults(moveResults);
		SendRunResolutionActionsMsgToClients(new List<ResolutionAction>(1)
		{
			item
		});
	}

	public void SendRunResolutionActionsMsgToClients(List<ResolutionAction> actions)
	{
		RunResolutionActionsOutsideResolve runResolutionActionsOutsideResolve = new RunResolutionActionsOutsideResolve
		{
			m_actionsServer = actions
		};
		if (actions.Count > 0)
		{
			// custom reactor
			NetworkServer.SendByChannelToAll((short)MyMsgType.RunResolutionActionsOutsideResolve, runResolutionActionsOutsideResolve, (int)NetworkChannelId.RunResolutionActionsOutsideResolve);
			// rogues
			//NetworkServer.SendToAll<RunResolutionActionsOutsideResolve>(runResolutionActionsOutsideResolve, (int)NetworkChannelId.RunResolutionActionsOutsideResolve);
		}
	}
#endif

	private void UNetVersion() // MirrorProcessed in rogues
	{
	}

	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	// removed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
