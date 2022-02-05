// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
//using System.Threading.Tasks;
//using CoOp;
//using Mirror;
//using Mirror.Websocket;
//using Open.Nat;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


// server-only -- was empty in reactor
// TODO fix networking & asyncs
// TODO ARTEMIS
public class ServerGameManager : MonoBehaviour
{
#if SERVER
	private static ServerGameManager s_instance;
	public static readonly string FirewallRuleName = "Atlas Reactor Game Server"; //  "Atlas Rogues Co-Op Game Server" in rogues

	public GameObject[] PrefabsForGameScenes;
	private FileEventLogger m_fileEventLogger;
	private NetworkEventLogger m_networkEventLogger;

	private List<long> m_reconnectingAccountIds;
	private List<ServerPlayerState> m_playersToReadyNextDecisionPhase;
	private int m_lastUpdateTurn;
	private float[] m_recentFrameTimes = new float[10];
	private bool m_sentGameSummary;
	protected Stopwatch m_heartBeat;
	private TimeSpan m_heartBeatDuration = TimeSpan.FromMinutes(20.0);
	private float m_lastLoadProgressUpdateSent;
	private float m_loadingProgressUpdateFrequency = 0.5f;
	private List<GameObject> m_instancesForGameScenes = new List<GameObject>();

	// TODO LOW look into this if we want to collect events and stats
	// monitor
	//private MonitorGameServerInterface m_monitorGameServerInterface;  // will be null for now

    private const int m_maxPlayers = 32;

	private bool m_loading;
	private bool m_loadLevelOperationDone;
	private int m_loadedCharacterResourceCount;

	private AssetBundleManager.LoadSceneAsyncOperation m_loadLevelOperation;
	private List<CharacterResourceLink> m_loadingCharacterResources;
	private AssetsLoadingState m_assetsLoadingState;
	private Dictionary<long, ServerPlayerState> m_serverPlayerStates;
	private Dictionary<NetworkConnection, float> m_pendingDisconnects;
	private List<QueuedConsoleMessage> m_queuedConsoleMessages;

	public static ServerGameManager Get()
	{
		return s_instance;
	}

	// monitor
	//public event Action<RegisterGameServerResponse> OnConnectedToMonitorServer = delegate(RegisterGameServerResponse p0) {};
	//public event Action<string> OnDisconnectedFromMonitorServer = delegate(string p0) {};

	public string ListenAddress { get; private set; }
	public int ListenPort { get; private set; }

    public bool IsConnectedToMonitorServer
    {
        get
        {
			// custom
			return false;
            //return m_monitorGameServerInterface != null && m_monitorGameServerInterface.isConnected;
        }
	}

	protected void Awake()
	{
		s_instance = this;
		m_loadingCharacterResources = new List<CharacterResourceLink>();
		m_serverPlayerStates = new Dictionary<long, ServerPlayerState>();
		m_reconnectingAccountIds = new List<long>();
		m_playersToReadyNextDecisionPhase = new List<ServerPlayerState>();
		m_queuedConsoleMessages = new List<QueuedConsoleMessage>();
		m_loading = false;
		m_assetsLoadingState = new AssetsLoadingState();
		m_pendingDisconnects = new Dictionary<NetworkConnection, float>();
		m_heartBeat = new Stopwatch();
	}

	protected void Start()
	{
		GameManager.Get().OnGameLaunched += HandleGameLaunched;
		GameManager.Get().OnGameStopped += HandleGameStopped;
		GameManager.Get().OnGameStatusChanged += HandleGameStatusChanged;
		m_heartBeat.Start();
		// monitor
		//HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		//if (hydrogenConfig != null && hydrogenConfig.MonitorHeartbeatCooldown.TotalSeconds > 0.0)
		//{
		//	m_heartBeatDuration = hydrogenConfig.MonitorHeartbeatCooldown;
		//}
	}

	protected void Update()
	{
		// monitor
        //if (m_monitorGameServerInterface != null)
        //{
        //    m_monitorGameServerInterface.Update();
        //}
        CheckConnected();
		CheckLoaded();
		CheckDisconnected();
		CheckDisconnecting();
		CheckReconnecting();
		for (int i = m_recentFrameTimes.Length - 1; i >= 1; i--)
		{
			m_recentFrameTimes[i - 1] = m_recentFrameTimes[i];
		}
		m_recentFrameTimes[m_recentFrameTimes.Length - 1] = Time.unscaledDeltaTime;
		if (GameFlowData.Get() != null
			&& m_lastUpdateTurn != GameFlowData.Get().CurrentTurn
			&& GameFlowData.Get().gameState <= GameState.BothTeams_Decision)
		{
			m_lastUpdateTurn = GameFlowData.Get().CurrentTurn;
			if (m_playersToReadyNextDecisionPhase.Count > 0)
			{
				foreach (ServerPlayerState serverPlayerState in m_playersToReadyNextDecisionPhase)
				{
					try
					{
						Log.Info("Allowing reconnecting player {0} ({1}) back in.", new object[]
						{
							serverPlayerState.SessionInfo.Handle,
							serverPlayerState.SessionInfo.AccountId
						});
						NetworkServer.SetClientReady(serverPlayerState.ConnectionPersistent);
						if (m_reconnectingAccountIds.Contains(serverPlayerState.SessionInfo.AccountId))
						{
							m_reconnectingAccountIds.Remove(serverPlayerState.SessionInfo.AccountId);
							SendConsoleMessageWithHandle("PlayerReconnected", "Disconnect", serverPlayerState.PlayerInfo.LobbyPlayerInfo.Handle, serverPlayerState.PlayerInfo.TeamId, ConsoleMessageType.SystemMessage, null);
						}
						serverPlayerState.ConnectionReady = true;
					}
					catch (Exception ex)
					{
						Log.Warning("Exception during NetworkServer.SetClientReady for {0} | {1}", new object[]
						{
							serverPlayerState.ToString(),
							ex
						});
					}
				}
				m_playersToReadyNextDecisionPhase.Clear();
			}
			if (ObjectivePoints.Get() != null)
			{
				ServerGameMetrics serverGameMetrics = new ServerGameMetrics();
				serverGameMetrics.CurrentTurn = m_lastUpdateTurn;
				serverGameMetrics.TeamAPoints = ObjectivePoints.Get().GetPointsForTeam(Team.TeamA);
				serverGameMetrics.TeamBPoints = ObjectivePoints.Get().GetPointsForTeam(Team.TeamB);
				serverGameMetrics.AverageFrameTime = (from f in m_recentFrameTimes where f != 0f select f).Average();
				Log.Info($"Frame time: {serverGameMetrics.AverageFrameTime * 1000f} ms");
				// monitor
				//if (m_monitorGameServerInterface != null)
				//{
				//	m_monitorGameServerInterface.SendGameMetricsNotification(serverGameMetrics);
				//}
			}
		}
		if (m_heartBeat.IsRunning && m_heartBeat.Elapsed > m_heartBeatDuration)
		{
			// monitor
			//m_monitorGameServerInterface.SendMonitorHeartbeatNotification();
			m_heartBeat.Reset();  // m_heartBeat.Restart(); in rogues
		}
	}

	internal void StartServerOrHost()  // async Task in rogues
	{
        HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
        if (hydrogenConfig == null)
        {
            throw new Exception("CommonServerConfig must be loaded to run servers or host games");
        }
        ListenAddress = "";
        ListenPort = 0;
        NetworkManager.singleton.networkAddress = "0.0.0.0";

		// new networking lib
		//TelepathyTransport telepathyTransport = Transport.activeTransport as TelepathyTransport;
		//bool flag = false;
		//int num = (int)telepathyTransport.port;
		//while (!flag)
		//{
		//	if (telepathyTransport != null)
		//	{
		//		telepathyTransport.port = (ushort)num;
		//	}
		//	if (HydrogenConfig.Get().ServerMode)
		//	{
		//		flag = NetworkManager.singleton.StartServer();
		//	}
		//	else
		//	{
		//		NetworkManager.singleton.StartHost();
		//		flag = NetworkClient.isConnected;
		//	}
		//	if (!flag)
		//	{
		//		num++;
		//	}
		//	if (num > 65535)
		//	{
		//		break;
		//	}
		//}
		//if (flag)

		MyNetworkManager myNetworkManager = MyNetworkManager.Get();
        myNetworkManager.m_OnServerError += HandleNetworkError;
        myNetworkManager.m_OnServerConnect += HandleNetworkConnect;
        myNetworkManager.m_OnServerDisconnect += HandleNetworkDisconnect;

		// custom
		NetworkServer.RegisterHandler((short)MyMsgType.LoginRequest, Wrap<GameManager.LoginRequest>(HandleLoginRequest));
		NetworkServer.RegisterHandler((short)MyMsgType.AssetsLoadedNotification, Wrap<GameManager.AssetsLoadedNotification>(HandleAssetsLoadedNotification));
		NetworkServer.RegisterHandler((short)MyMsgType.LeaveGameNotification, Wrap<GameManager.LeaveGameNotification>(HandleLeaveGameNotification));
		NetworkServer.RegisterHandler((short)MyMsgType.ClientAssetsLoadingProgressUpdate, Wrap<GameManager.AssetsLoadingProgress>(HandleClientAssetsLoadingProgressUpdate));
		NetworkServer.RegisterHandler((short)MyMsgType.ClientPreparedForGameStartNotification, Wrap<GameManager.PlayerObjectStartedOnClientNotification>(HandleClientPreparedForGameStartNotification));
		NetworkServer.RegisterHandler((short)MyMsgType.ClientFakeActionRequest, Wrap<GameManager.FakeActionRequest>(HandleClientFakeActionRequest));
		// rogues
		//NetworkServer.RegisterHandler<GameManager.LoginRequest>(new Action<NetworkConnection, GameManager.LoginRequest>(HandleLoginRequest));
		//      NetworkServer.RegisterHandler<GameManager.AssetsLoadedNotification>(new Action<NetworkConnection, GameManager.AssetsLoadedNotification>(HandleAssetsLoadedNotification));
		//      NetworkServer.RegisterHandler<GameManager.LeaveGameNotification>(new Action<NetworkConnection, GameManager.LeaveGameNotification>(HandleLeaveGameNotification));
		//      NetworkServer.RegisterHandler<GameManager.AssetsLoadingProgress>(new Action<NetworkConnection, GameManager.AssetsLoadingProgress>(HandleClientAssetsLoadingProgressUpdate));
		//      NetworkServer.RegisterHandler<GameManager.PlayerObjectStartedOnClientNotification>(new Action<NetworkConnection, GameManager.PlayerObjectStartedOnClientNotification>(HandleClientPreparedForGameStartNotification));
		//      NetworkServer.RegisterHandler<GameManager.FakeActionRequest>(new Action<NetworkConnection, GameManager.FakeActionRequest>(HandleClientFakeActionRequest));


		// Firewall and stuff
		//ListenAddress = NetUtil.GetIPv4Address(hydrogenConfig.HostName).ToString();
		//ListenPort = (int)((telepathyTransport != null) ? telepathyTransport.port : 0);
		//string lobbyServerAddress = ClientGameManager.Get().LobbyInterface.LobbyServerAddress;
		//IPAddress ipv4Address = NetUtil.GetIPv4Address(NetUtil.GetHostName());
		//string localNetMask = NetUtil.GetLocalNetMask();
		//bool flag2 = NetUtil.IsInSameNetwork(lobbyServerAddress, localNetMask, ipv4Address.ToString());
		//if (CoOpSessionManager.Get().IsHosting() && !flag2)
		//{
		//	try
		//	{
		//		NatDevice natDevice2 = await NATUtils.GetNATDevice();
		//		NatDevice natDevice = natDevice2;
		//		if (natDevice != null)
		//		{
		//			IPAddress external = await NATUtils.GetExternalAddress();
		//			if (external != null)
		//			{
		//				Mapping mapping = await NATUtils.AddPortMapping(natDevice, ListenPort, ListenPort);
		//				if (mapping != null && mapping.PublicPort != 0)
		//				{
		//					bool firewallExists = await FirewallUtils.CheckFirewallExists(FirewallRuleName, ListenPort);
		//					bool flag3 = false;
		//					if (!firewallExists)
		//					{
		//						flag3 = await FirewallUtils.OpenFirewall(FirewallRuleName, ListenPort);
		//					}
		//					ListenAddress = external.ToString();
		//					ListenPort = (int)((short)mapping.PublicPort);
		//					string text = (!firewallExists) ? ((!flag3) ? "Unknown/Blocked" : "Permitted (Newly Initialized)") : "Permitted (Previously Setup)";
		//					Log.Info("Game Server using an external address/port {0}:{1} -> {2} -> {3}:{4}", new object[]
		//					{
		//							ListenAddress,
		//							ListenPort,
		//							text,
		//							mapping.PrivateIP,
		//							mapping.PrivatePort
		//					});
		//				}
		//				mapping = null;
		//			}
		//			external = null;
		//		}
		//		natDevice = null;
		//	}
		//	catch (NatDeviceNotFoundException)
		//	{
		//	}
		//	catch (MappingException)
		//	{
		//	}
		//	catch (TaskCanceledException)
		//	{
		//	}
		//	catch (Exception ex)
		//	{
		//		Log.Info("Handled exception while attempting to create a forwarded port mapped to the Game Server{0}", new object[]
		//		{
		//				ex.Message
		//		});
		//	}
		//}

		CommonServerConfig commonServerConfig = CommonServerConfig.Get();
        ListenAddress = (((commonServerConfig != null) ? commonServerConfig.GameServerListenAddress : null) ?? ListenAddress);
        Log.Info("Game Server initialized at {0}:{1}", new object[]
        {
                ListenAddress,
                ListenPort
        });
        return;

	// new networking lib cont
	//}
	//	throw new Exception("Failed to initialize Game Server");
}

	// monitor
	//internal void ConnectToMonitorServer(string address, short port, long accountId = 0L)
	//   {
	//       if (m_monitorGameServerInterface != null)
	//       {
	//           return;
	//       }
	//       HydrogenConfig hydrogenConfig = HydrogenConfig.Get();

	//	WebsocketTransport component = base.GetComponent<WebsocketTransport>();
	//	component.port = (int)port;
	//	component.Secure = true;
	//	component.serverName = "MonitorGameServerSessionManager";

	//	m_monitorGameServerInterface = base.gameObject.AddComponent<MonitorGameServerInterface>();
	//	m_monitorGameServerInterface.Initialize(address, port, hydrogenConfig.ProcessType, hydrogenConfig.ProcessCode, accountId);
	//	m_monitorGameServerInterface.OnConnectedHandler += HandleConnectedToMonitorServer;
	//	m_monitorGameServerInterface.OnDisconnectedHandler += HandleDisconnectedFromMonitorServer;
	//	m_monitorGameServerInterface.OnLaunchGameRequest += HandleLaunchGameRequest;
	//	m_monitorGameServerInterface.OnJoinGameServerRequest += HandleJoinGameServerRequest;
	//	m_monitorGameServerInterface.OnJoinGameAsObserverRequest += HandleJoinGameAsObserverRequest;
	//	m_monitorGameServerInterface.OnShutdownGameRequest += HandleShutdownGameRequest;
	//	m_monitorGameServerInterface.OnDisconnectPlayerRequest += HandleDisconnectPlayerRequest;
	//	m_monitorGameServerInterface.OnReconnectPlayerRequest += HandleReconnectPlayerRequest;
	//	m_monitorGameServerInterface.OnMonitorHeartbeatResponse += HandleMonitorHeartbeatResponse;
	//	m_monitorGameServerInterface.Reconnect();
	//}

	// monitor
	//public void DisconnectFromMonitorServer()
 //   {
 //       if (m_monitorGameServerInterface != null)
 //       {
 //           m_monitorGameServerInterface.Disconnect();
 //       }
 //   }

    internal void DisconnectClient(int connectionId)
	{
		for (int i = 0; i < NetworkServer.connections.Count; i++)
		{
			NetworkConnection networkConnection = NetworkServer.connections[i];
			if (networkConnection != null && networkConnection.connectionId == connectionId)
			{
				networkConnection.Disconnect();
				return;
			}
		}
	}

	internal void OnClientReplacedWithBots(NetworkConnection connection)
	{
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			if (serverPlayerState != null && serverPlayerState.ConnectionPersistent == connection)
			{
				serverPlayerState.OnReplaceWithBots();
			}
		}
	}

	internal void OnClientReplacedWithHumans(NetworkConnection connection)
	{
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			if (serverPlayerState != null && serverPlayerState.ConnectionPersistent == connection)
			{
				serverPlayerState.OnReplaceWithHumans();
			}
		}
	}

	public void SendGameSummaryNotification()
	{
		// monitor
		//if (m_monitorGameServerInterface != null)
  //      {
  //          Log.Info("Sending game summary to lobby server");
  //          m_monitorGameServerInterface.SendGameSummaryNotification(GameManager.Get().GameSummary, GameManager.Get().GameSummaryOverrides);
  //          m_sentGameSummary = true;
  //          foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
  //          {
  //              if (serverPlayerState.SessionInfo != null && serverPlayerState.PlayerInfo != null && !serverPlayerState.PlayerInfo.IsNPCBot)
  //              {
  //                  serverPlayerState.LogGameExit(GameManager.Get().GameSummary.GameResult);
  //              }
  //          }
  //      }
    }

	private void OnDestroy()
	{
		if (GameManager.Get() != null)
		{
			GameManager.Get().StopGame(GameResult.NoResult);
			GameManager.Get().OnGameLaunched -= HandleGameLaunched;
			GameManager.Get().OnGameStopped -= HandleGameStopped;
			GameManager.Get().OnGameStatusChanged -= HandleGameStatusChanged;
		}
		// monitor
		//DisconnectFromMonitorServer();
		DisconnectClients();
		if (EventLogger.Get() != null)
		{
			EventLogger.Get().RemoveChildLogger(m_fileEventLogger);
			EventLogger.Get().RemoveChildLogger(m_networkEventLogger);
		}
		m_fileEventLogger = null;
		m_networkEventLogger = null;
		s_instance = null;
	}

	private void HandleNetworkError(NetworkConnection conn, NetworkError errorCode)  // byte errorCode in rogues
	{
		Log.Error("Client network error from {0}: (connectionId {1}) {2}", new object[]
		{
			conn.address,
			conn.connectionId,
			errorCode.ToString()
		});
	}

	private void HandleNetworkConnect(NetworkConnection conn)
	{
		if (!(conn.address == "localhost") || conn.connectionId <= NetworkManager.singleton.maxConnections)
		{
			Log.Info("Client network connected from {0} (connectionId {1})", new object[]
			{
				conn.address,
				conn.connectionId
			});
		}
	}

	private void HandleNetworkDisconnect(NetworkConnection conn)
	{
		if (CommonServerConfig.Get().AllowReconnectingToGameInstantly)
		{
			DisconnectPending(conn);
			return;
		}
		DisconnectNow(conn);
	}

	private void DisconnectPending(NetworkConnection conn)
	{
		if (!m_pendingDisconnects.ContainsKey(conn))
		{
			ServerPlayerState playerStateByConnectionId = GetPlayerStateByConnectionId(conn.connectionId);
			LobbyGameInfo gameInfo = GameManager.Get().GameInfo;
			TimeSpan gameServerClientReconnectTimeout = CommonServerConfig.Get().GameServerClientReconnectTimeout;
			Log.Info("{0} Connection is queued to disconnect (connectionId {1}) ({2}secs)| {3}", new object[]
			{
				playerStateByConnectionId,
				conn.connectionId,
				gameServerClientReconnectTimeout.TotalSeconds,
				gameInfo.Name
			});
			m_pendingDisconnects.Add(conn, Time.realtimeSinceStartup + (float)gameServerClientReconnectTimeout.TotalSeconds);
		}
	}

	public bool IsDisconnectPending(NetworkConnection conn)
	{
		return m_pendingDisconnects.ContainsKey(conn);
	}

	private void DisconnectNow(NetworkConnection conn)
	{
		m_pendingDisconnects.Remove(conn);
		ServerPlayerState serverPlayerState = null;
		foreach (ServerPlayerState serverPlayerState2 in m_serverPlayerStates.Values)
		{
			if (serverPlayerState2.ConnectionPersistent == conn)
			{
				serverPlayerState = serverPlayerState2;
				break;
			}
		}
		if (serverPlayerState == null)
		{
			Log.Info("Unknown player disconnected from {0} (connectionId {1})", new object[]
			{
				conn.address,
				conn.connectionId
			});
		}
		else
		{
			// monitor
			//if (m_monitorGameServerInterface != null && serverPlayerState.SessionInfo != null && serverPlayerState.PlayerInfo != null)
			//{
			//	m_monitorGameServerInterface.SendPlayerDisconnectedNotification(serverPlayerState.SessionInfo, serverPlayerState.PlayerInfo.LobbyPlayerInfo);
			//}
			serverPlayerState.ConnectionReady = false;
			bool flag = GameManager.Get().GameStatus == GameStatus.Stopped || (ObjectivePoints.Get() != null && ObjectivePoints.Get().m_matchState == ObjectivePoints.MatchState.MatchEnd);
			bool flag2 = serverPlayerState.PlayerInfo != null && serverPlayerState.PlayerInfo.TeamId == Team.Spectator;
			long accountId = (serverPlayerState.PlayerInfo != null) ? serverPlayerState.PlayerInfo.LobbyPlayerInfo.AccountId : -1L;
			GameResult gameResult = GameResult.GameServerNetworkErrorToClient;
			string empty = string.Empty;
			serverPlayerState.LogGameExit(gameResult);
			GameFlow gameFlow = GameFlow.Get();
			if (!serverPlayerState.IsAIControlled && !flag2 && !flag)
			{
				Log.Info("Player {0} [{1}] (connectionId {2}, connectionAddress {3}) has disconnected from game {4} (listenAddress {5}), and will now be controlled by a bot | closeStatusCode= {6}", new object[]
				{
					serverPlayerState.PlayerInfo.LobbyPlayerInfo.Handle,
					serverPlayerState.SessionInfo.AccountId,
					serverPlayerState.ConnectionId,
					serverPlayerState.ConnectionAddress,
					serverPlayerState.GameInfo.Name,
					ListenAddress,
					empty
				});
				SendConsoleMessageWithHandle("PlayerDisconnectedBot", "Disconnect", serverPlayerState.PlayerInfo.LobbyPlayerInfo.Handle, serverPlayerState.PlayerInfo.TeamId, ConsoleMessageType.SystemMessage, null);
				if (gameFlow != null)
				{
					gameFlow.ReplaceWithBots(serverPlayerState.ConnectionPersistent);
				}
				serverPlayerState.OnReplaceWithBots();
			}
			else
			{
				Log.Info("Player {0} [{1}] (connectionId {2}, connectionAddress {3}) has disconnected from game {4} (listenAddress {5}) | closeStatusCode= {6}", new object[]
				{
					serverPlayerState.PlayerInfo.LobbyPlayerInfo.Handle,
					serverPlayerState.SessionInfo.AccountId,
					serverPlayerState.ConnectionId,
					serverPlayerState.ConnectionAddress,
					serverPlayerState.GameInfo.Name,
					ListenAddress,
					empty
				});
				SendConsoleMessageWithHandle("PlayerDisconnected", "Disconnect", serverPlayerState.PlayerInfo.LobbyPlayerInfo.Handle, serverPlayerState.PlayerInfo.TeamId, ConsoleMessageType.SystemMessage, null);
				if (gameFlow != null)
				{
					gameFlow.FlagPlayerAsDisconnected(serverPlayerState.ConnectionPersistent);
				}
			}
			if (ServerResolutionManager.Get() != null)
			{
				ServerResolutionManager.Get().OnActorDisconnected();
			}
			if (ServerMovementManager.Get() != null)
			{
				ServerMovementManager.Get().OnActorDisconnected();
			}
			if (TheatricsManager.Get() != null)
			{
				TheatricsManager.Get().StopWaitingForConnectionId(accountId);
			}
		}
		CheckDisconnected();
	}

	private void HandleGameStatusChanged(GameStatus gameStatus)
	{
		if (
			// monitor
			//m_monitorGameServerInterface != null && 
			gameStatus >= GameStatus.LoadoutSelecting)
		{
			if (gameStatus == GameStatus.Stopped && !m_sentGameSummary && GameManager.Get() != null && GameManager.Get().GameSummary != null)
			{
				GameManager.Get().GameSummary.TimeText = ((MatchLogger.Get() != null) ? MatchLogger.Get().GetTimeForLogging(true) : "0");
				GameManager.Get().GameSummary.MatchTime = ((MatchLogger.Get() != null) ? MatchLogger.Get().GetMatchTime() : new TimeSpan(0L));
				GameManager.Get().GameSummary.NumOfTurns = ((GameFlowData.Get() != null) ? GameFlowData.Get().CurrentTurn : 0);
				SendGameSummaryNotification();
			}
			// monitor
			//m_monitorGameServerInterface.SendGameStatusNotification(gameStatus);
		}
	}

	private void HandleGameLaunched(GameType gameType)
	{
	}

	private void HandleGameStopped(GameResult gameResult)
	{
		if (NetworkServer.active)
		{
			DestroyPrefabsForGameScenes();
			m_loading = false;
			m_loadLevelOperationDone = false;
			m_loadedCharacterResourceCount = 0;
		}
	}

	public void DisconnectClients()
	{
		if (NetworkManager.singleton.isNetworkActive)
		{
			if (HydrogenConfig.Get().ServerMode)
			{
				NetworkManager.singleton.StopServer();
			}
			else
			{
				NetworkManager.singleton.StopHost();
			}
			MyNetworkManager myNetworkManager = MyNetworkManager.Get();
			myNetworkManager.m_OnServerError -= HandleNetworkError;
			myNetworkManager.m_OnServerConnect -= HandleNetworkConnect;
		}
	}

	internal static NetworkConnection Find(int serverConnectionId)
	{
		NetworkConnection networkConnection = null;
		for (int i = 0; i < NetworkServer.connections.Count; i++)
		{
			if (NetworkServer.connections[i] != null && NetworkServer.connections[i].connectionId == serverConnectionId)
			{
				networkConnection = NetworkServer.connections[i];
				break;
			}
		}
		// TODO LOW NETWORKING backport to unet if needed
		//if (networkConnection == null && NetworkServer.localConnection != null && NetworkServer.localConnection.connectionId == serverConnectionId)
		//{
		//	networkConnection = NetworkServer.localConnection;
		//}
		return networkConnection;
	}

	internal static void DestroyPlayersForConnection(int connectionId)
	{
		// TODO LOW NETWORKING backport to unet if needed
		//NetworkConnection networkConnection = ServerGameManager.Find(connectionId);
		//if (networkConnection != null)
		//{
		//	NetworkServer.DestroyPlayerForConnection(networkConnection);
		//}
	}

	// monitor
	//private void HandleConnectedToMonitorServer(RegisterGameServerResponse response)
	//{
	//	if (!response.Success)
	//	{
	//		DisconnectFromMonitorServer();
	//		OnConnectedToMonitorServer(response);
	//		GameManager.Get().StopGame(GameResult.NoResult);
	//		return;
	//	}
	//	OnConnectedToMonitorServer(response);
	//}

	// monitor
	//private void HandleDisconnectedFromMonitorServer(string lastLobbyErrorMessage)
	//{
	//	Log.Info("Disconnected from Monitor Server");
	//	// monitor
	//	//m_monitorGameServerInterface = null;
	//	GameResult gameResult = GameResult.GameServerNetworkErrorToMonitorServer;
	//	OnDisconnectedFromMonitorServer(lastLobbyErrorMessage);
	//	GameManager.Get().StopGame(gameResult);
	//}

	private void SetGameStatus(GameStatus gameStatus, bool notify = true)
	{
		GameManager.Get().SetGameStatus(gameStatus, GameResult.NoResult, notify);
	}

	private void HandleLaunchGameRequest(LaunchGameRequest request)  // async in rogues
	{
        // custom
        MatchmakingQueueConfig config = new MatchmakingQueueConfig()
		{

		};

		GameManager gameManager = GameManager.Get();
		gameManager.SetGameInfo(request.GameInfo);
		gameManager.SetTeamInfo(LobbyTeamInfo.FromServer(request.TeamInfo, GameBalanceVars.Get().MaxPlayerLevel, config)); // no config in rogues
		if (request.GameplayOverrides != null)
		{
			gameManager.SetGameplayOverrides(request.GameplayOverrides);
		}
		StartServerOrHost(); // await StartServerOrHost(); in rogues
		m_serverPlayerStates.Clear();
		List<LobbyServerPlayerInfo> localPlayers = (from p in request.TeamInfo.TeamPlayerInfo where !p.IsNPCBot && !p.IsRemoteControlled select p).ToList<LobbyServerPlayerInfo>();
		List<LobbyServerPlayerInfo> bots = (from p in request.TeamInfo.TeamPlayerInfo where p.IsNPCBot select p).ToList<LobbyServerPlayerInfo>();
		int num = -1;
		foreach (LobbyServerPlayerInfo lobbyServerPlayerInfo in localPlayers)
		{
			LobbySessionInfo sessionInfo = request.SessionInfo[lobbyServerPlayerInfo.PlayerId];
			List<LobbyServerPlayerInfo> list3 = new List<LobbyServerPlayerInfo>();
			using (List<int>.Enumerator enumerator2 = lobbyServerPlayerInfo.ProxyPlayerIds.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					int proxyPlayerId = enumerator2.Current;
					LobbyServerPlayerInfo lobbyServerPlayerInfo2 = (from p in request.TeamInfo.TeamPlayerInfo where p.PlayerId == proxyPlayerId select p).FirstOrDefault<LobbyServerPlayerInfo>();
					if (lobbyServerPlayerInfo2 != null)
					{
						lobbyServerPlayerInfo2.CharacterInfo.CharacterCards = lobbyServerPlayerInfo.CharacterInfo.CharacterCards;
						list3.Add(lobbyServerPlayerInfo2);
					}
				}
			}
			AddPlayerState(sessionInfo, lobbyServerPlayerInfo, list3, num--);
			}
		foreach (LobbyServerPlayerInfo primaryPlayerInfo in bots)
		{
			AddPlayerState(null, primaryPlayerInfo, new List<LobbyServerPlayerInfo>(), -999);
		}
		gameManager.SetGameSummary(new LobbyGameSummary());
		m_sentGameSummary = false;
		CommonServerConfig commonServerConfig = CommonServerConfig.Get();
		gameManager.GameInfo.GameServerAddress = string.Format("{0}:{1}", ListenAddress, ListenPort);
		gameManager.GameInfo.GameServerHost = commonServerConfig.PrivateHostName;
		// monitor
		//m_monitorGameServerInterface.SendLaunchGameResponse(true, gameManager.GameInfo);
		SetGameStatus(GameStatus.Launched, true);
		SetGameStatus(GameStatus.Connecting, true);
		LoadAssets();
	}

	private void AddPlayerState(LobbySessionInfo sessionInfo, LobbyServerPlayerInfo primaryPlayerInfo, List<LobbyServerPlayerInfo> proxyPlayerInfos, int connectionTemporaryId)
	{
		GameManager gameManager = GameManager.Get();
		ServerPlayerState serverPlayerState = new ServerPlayerState()
		{
			PlayerInfo = new ServerPlayerInfo(primaryPlayerInfo, proxyPlayerInfos),
			SessionInfo = sessionInfo,
			ConnectionPersistent = null,
			ConnectionReady = false,
			GameLoadingState = new GameLoadingState(),
		};
		m_serverPlayerStates.Add((long)primaryPlayerInfo.PlayerId, serverPlayerState);
		ApplyOverrides(gameManager.GameplayOverrides, serverPlayerState.PlayerInfo);
		foreach (ServerPlayerInfo playerInfo in serverPlayerState.PlayerInfo.ProxyPlayerInfos)
		{
			ApplyOverrides(gameManager.GameplayOverrides, playerInfo);
		}

		// rogues
		//if (primaryPlayerInfo.IsGameOwner && NetworkClient.active)
		//{
		//	serverPlayerState.LocalClient = true;
		//	serverPlayerState.ConnectionPersistent = NetworkClient.connection;
		//	return;
		//}

		if (!serverPlayerState.IsAIControlled || primaryPlayerInfo.ReplacedWithBots || serverPlayerState.IsLoadTestBot)
		{
			if (serverPlayerState.IsAIControlled && primaryPlayerInfo.ReplacedWithBots)
			{
				Log.Info("Player {0}[{1}] Is AI Controlled and replaced with bots when adding player state", new object[]
				{
					serverPlayerState.PlayerInfo.Handle,
					serverPlayerState.SessionInfo.AccountId
				});
			}
			string text = "localhost";

			// custom
			string text2 = serverPlayerState.SessionInfo.ConnectionAddress;
			// rogues
			//string text2 = (!serverPlayerState.SessionInfo.ExternalConnectionAddress.IsNullOrEmpty()) ? serverPlayerState.SessionInfo.ExternalConnectionAddress : serverPlayerState.SessionInfo.ConnectionAddress;

			if (serverPlayerState != null && serverPlayerState.SessionInfo != null && !text2.IsNullOrEmpty())
			{
				UnityEngine.Debug.Log("Expecting connection from client at address " + text2);
				string[] array = text2.Split(new char[] { ':', '/' });
				if (!array.IsNullOrEmpty())
				{
					if (array.Length > 1)
					{
						text = array[array.Length - 2];
					}
					else
					{
						text = array[0];
					}
				}
			}

			// TODO probably will have to set serverPlayerState.ConnectionPersistent somewhere else
			// Though null is fine, it will be set when client actually connects.
			//serverPlayerState.ConnectionPersistent = new NetworkConnection(); // new NetworkConnection(text, connectionTemporaryId); in rogues
			//NetworkServer.AddConnection(serverPlayerState.ConnectionPersistent);

			// TODO LOW load-test bots
			//if (GameManager.Get().GameMission != null
			//	&& GameManager.Get().GameMission.IsMissionTagActive(MissionData.s_missionTagFakeClients)
			//	&& serverPlayerState.IsLoadTestBot)
			//{
			//	Log.Info("Player {0}[{1}] is a load test client, and will now be controlled by a bot", new object[]
			//	{
			//		serverPlayerState.PlayerInfo.Handle,
			//		serverPlayerState.SessionInfo.AccountId
			//	});
			//	serverPlayerState.DisconnectAndReplaceWithBots(GameResult.EmptyPlayers);
			//	return;
			//}
			if (primaryPlayerInfo.ReplacedWithBots)
			{
				Log.Info("Player {0}[{1}] is disconnected, and will now be controlled by a bot", new object[]
				{
					serverPlayerState.PlayerInfo.Handle,
					serverPlayerState.SessionInfo.AccountId
				});
				serverPlayerState.DisconnectAndReplaceWithBots(GameResult.ClientDisconnectedAtLaunch);
			}
		}
	}

	private void ApplyOverrides(LobbyGameplayOverrides gameplayOverrides, ServerPlayerInfo playerInfo)
	{
		if (gameplayOverrides != null)
		{
			if (!gameplayOverrides.EnableCards)
			{
				playerInfo.CharacterCards.Reset();
			}
			else
			{
				if (!gameplayOverrides.IsCardAllowed(playerInfo.CharacterCards.PrepCard))
				{
					playerInfo.CharacterCards.PrepCard = CardType.None;
				}
				if (!gameplayOverrides.IsCardAllowed(playerInfo.CharacterCards.DashCard))
				{
					playerInfo.CharacterCards.DashCard = CardType.None;
				}
				if (!gameplayOverrides.IsCardAllowed(playerInfo.CharacterCards.CombatCard))
				{
					playerInfo.CharacterCards.CombatCard = CardType.None;
				}
			}
			// rogues
			//if (!gameplayOverrides.EnableAllGear)
			//{
			//	playerInfo.CharacterGear.Reset();
			//	return;
			//}
			//for (int i = 0; i < CharacterGearInfo.AbilityCount; i++)
			//{
			//	int gearIDForAbility = playerInfo.CharacterGear.GetGearIDForAbility(i);
			//	if (gearIDForAbility != -1)
			//	{
			//		Gear gearForID = GearHelper.GetGearForID(gearIDForAbility);
			//		if (!gameplayOverrides.IsGearAllowed(playerInfo.CharacterType, i, gearForID))
			//		{
			//			playerInfo.CharacterGear.SetGearIDForAbility(i, -1);
			//		}
			//	}
			//}
		}
	}

	private void HandleJoinGameServerRequest(JoinGameServerRequest request)
	{
		bool flag = true;
		string responseText = null;
		try
		{
			List<LobbyServerPlayerInfo> proxyPlayerInfos = new List<LobbyServerPlayerInfo>();
			if (m_serverPlayerStates.ContainsKey((long)request.PlayerInfo.PlayerId))
			{
				Log.Error("Player {0} trying to join game server, but ServerPlayerState already exists for this player", new object[]
				{
					request.PlayerInfo.Handle,
					request.PlayerInfo.AccountId
				});
				responseText = "Already joined";
				flag = false;
			}
			if (flag)
			{
				AddPlayerState(request.SessionInfo, request.PlayerInfo, proxyPlayerInfos, -1);
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
			Log.Error("Player {0} trying to join game server, but exception occurred", new object[]
			{
				request.PlayerInfo.Handle,
				request.PlayerInfo.AccountId
			});
			responseText = "Server error";
			flag = false;
		}
		// monitor
		//m_monitorGameServerInterface.SendJoinGameServerResponse(request.RequestId, flag, responseText, request.OrigRequestId, request.PlayerInfo, request.GameServerProcessCode);
	}

	private void HandleJoinGameAsObserverRequest(JoinGameAsObserverRequest request)
	{
		bool success = true;
		string responseText = null;
		LobbyGameplayOverrides gameplayOverrides = null;
		LobbyGameInfo gameInfo = null;
		LobbyTeamInfo teamInfo = null;
		LobbyPlayerInfo playerInfo = null;
		try
		{
			success = false;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
			Log.Error("Player {0} trying to join game server as observer, but exception occurred", new object[]
			{
				request.AccountId
			});
			responseText = "Server error";
			success = false;
		}
		// monitor
		//m_monitorGameServerInterface.SendJoinGameAsObserverResponse(request.RequestId, success, responseText, gameplayOverrides, gameInfo, teamInfo, playerInfo);
	}

	private void CheckConnected()
	{
		if (!IsServer())
		{
			return;
		}
		CommonServerConfig commonServerConfig = CommonServerConfig.Get();
		GameManager gameManager = GameManager.Get();
		if (gameManager.GameStatus == GameStatus.Connecting || gameManager.GameStatus == GameStatus.Loading)
		{
			bool flag = false;
			foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
			{
				if (serverPlayerState.PlayerInfo != null && serverPlayerState.SessionInfo != null && (serverPlayerState.ConnectionPersistent == null || serverPlayerState.ConnectionPersistent.connectionId < 0))
				{
					flag = true;
					break;
				}
			}
			if (flag && (double)(Time.unscaledTime - gameManager.GameStatusTime) >= commonServerConfig.GameServerClientConnectTimeout.TotalSeconds)
			{
				foreach (ServerPlayerState serverPlayerState2 in m_serverPlayerStates.Values)
				{
					if (serverPlayerState2.PlayerInfo != null && serverPlayerState2.SessionInfo != null && (serverPlayerState2.ConnectionPersistent == null || !serverPlayerState2.ConnectionPersistent.isReady))
					{
						Log.Info("Player {0}[{1}] has failed to connect to game {2}, and will now be controlled by a bot | {3}:{4}", new object[]
						{
							serverPlayerState2.PlayerInfo.Handle,
							serverPlayerState2.SessionInfo.AccountId,
							gameManager.GameInfo.Name,
							ListenAddress,
							ListenPort
						});
						SendConsoleMessageWithHandle("PlayerFailedToConnect", "Disconnect", serverPlayerState2.PlayerInfo.Handle, Team.Invalid, ConsoleMessageType.SystemMessage, null);
						serverPlayerState2.DisconnectAndReplaceWithBots(GameResult.ClientConnectionFailedToGameServer);
					}
				}
				flag = false;
			}
			if (!flag && gameManager.GameStatus == GameStatus.Connecting)
			{
				SetGameStatus(GameStatus.Connected, true);
				SetGameStatus(GameStatus.Authenticated, true);
				SetGameStatus(GameStatus.Loading, true);
			}
		}
	}

	private void CheckDisconnecting()
	{
		if (!IsServer())
		{
			return;
		}
		if (m_pendingDisconnects.Count == 0)
		{
			return;
		}
		foreach (NetworkConnection networkConnection in (from kvp in m_pendingDisconnects
		where kvp.Value < Time.realtimeSinceStartup
		select kvp.Key).ToArray())
		{
			m_pendingDisconnects.Remove(networkConnection);
			DisconnectNow(networkConnection);
		}
	}

	private void CheckReconnecting()
	{
		if (!IsServer())
		{
			return;
		}
		if (m_reconnectingAccountIds.Count == 0)
		{
			return;
		}
		if (GameManager.Get().GameStatus == GameStatus.Started)
		{
			bool flag = GameFlowData.Get() != null && m_lastUpdateTurn == GameFlowData.Get().CurrentTurn && GameFlowData.Get().gameState == GameState.EndingGame;
			foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
			{
				bool flag2 = serverPlayerState.SessionInfo != null && m_reconnectingAccountIds.Contains(serverPlayerState.SessionInfo.AccountId);
				NetworkConnection connectionPersistent = serverPlayerState.ConnectionPersistent;
				if (flag2)
				{
					if (connectionPersistent == null)
					{
						Log.Warning("Reconnecting client has disconnected {0}", new object[]
						{
							serverPlayerState
						});
						m_reconnectingAccountIds.Remove(serverPlayerState.SessionInfo.AccountId);
					}
					else if (flag)
					{
						Log.Warning("Reconnecting client has connected on game ending {0}", new object[]
						{
							serverPlayerState
						});
						m_reconnectingAccountIds.Remove(serverPlayerState.SessionInfo.AccountId);
						GameManager.EndGameNotification endGameNotification = new GameManager.EndGameNotification();

						// TODO LOW check
						// custom
						NetworkServer.SendToClient(connectionPersistent.connectionId, (short)MyMsgType.EndGameNotification, endGameNotification);
						// rogues
						//NetworkServer.SendToClient<GameManager.EndGameNotification>(connectionPersistent.connectionId, endGameNotification);
					}
				}
			}
		}
	}

	// custom
	static private NetworkMessageDelegate Wrap<T>(Action<NetworkConnection, T> handler) where T : MessageBase, new()
	{
		return (msg) => handler(msg.conn, msg.ReadMessage<T>());
    }

	private void HandleLoginRequest(NetworkConnection conn, GameManager.LoginRequest request)
	{
		if (!IsServer() || request == null)
		{
			return;
		}
		long num = Convert.ToInt64(request.AccountId);
		GameManager.LoginResponse loginResponse = new GameManager.LoginResponse();
		if (!m_serverPlayerStates.TryGetValue(request.PlayerId, out ServerPlayerState serverPlayerState)
			|| serverPlayerState.SessionInfo == null
			|| num != serverPlayerState.SessionInfo.AccountId)
		{
			loginResponse.Success = false;
			if (serverPlayerState != null && serverPlayerState.ReplacedWithBots)
			{
				Log.Error("Received login request from {0} after they were disconnected and replaced with a bot for timing out.", new object[]
				{
					conn.address
				});
				loginResponse.ErrorMessage = "Login denied, your client timed out and was replaced with a bot.";
			}
			else
			{
				Log.Error("Received invalid login request from {0} | request= {1} {2} {3}", new object[]
				{
					conn.address,
					request.AccountId,
					request.SessionToken,
					request.PlayerId
				});
				loginResponse.ErrorMessage = "Invalid login request";
			}

			// TODO LOW check
			// custom
			NetworkServer.SendToClient(conn.connectionId, (short)MyMsgType.LoginResponse, loginResponse);
			// rogues
			//NetworkServer.SendToClient<GameManager.LoginResponse>(conn.connectionId, loginResponse);
			return;
		}
		Log.Info("LoginRequest from {0} is using type {1} connection", new object[]
		{
			serverPlayerState,
			conn.GetType().ToString()
		});
		bool isReconnecting = false;
		if (Convert.ToInt64(request.SessionToken) != serverPlayerState.SessionInfo.SessionToken
			&& (serverPlayerState.SessionInfo.ReconnectSessionToken == 0L
				|| Convert.ToInt64(request.SessionToken) != serverPlayerState.SessionInfo.ReconnectSessionToken))
		{
			Log.Error("Received invalid reconnect request from {0} (connectionId {1}) {2} to game {3} | request= {4} {5} {6} | playerState= {7} {8}", new object[]
			{
				conn.address,
				conn.connectionId,
				serverPlayerState.SessionInfo.Name,
				serverPlayerState.GameInfo.GameServerProcessCode,
				request.AccountId,
				request.SessionToken,
				request.PlayerId,
				serverPlayerState.SessionInfo.SessionToken,
				serverPlayerState.SessionInfo.ReconnectSessionToken
			});
			loginResponse.ErrorMessage = "Invalid reconnect request";

			// TODO LOW check
			// custom
			NetworkServer.SendToClient(conn.connectionId, (short)MyMsgType.LoginResponse, loginResponse);
			// rogues
			//NetworkServer.SendToClient<GameManager.LoginResponse>(conn.connectionId, loginResponse);
			return;
		}
		if (serverPlayerState.IsAIControlled && !serverPlayerState.IsLoadTestBot)
		{
			Log.Info("This is a reconnect request from {0} (connectionId {1}) to game {2}...  {3} | PlayerId {4}, LastReceivedMsgSeqNum {5}", new object[]
			{
				conn.address,
				conn.connectionId,
				serverPlayerState.GameInfo.GameServerProcessCode,
				serverPlayerState.SessionInfo.Name,
				request.PlayerId,
				request.LastReceivedMsgSeqNum
			});
			m_reconnectingAccountIds.Add(serverPlayerState.SessionInfo.AccountId);
			isReconnecting = true;
			serverPlayerState.ConnectionReady = false;
		}
		else
		{
			loginResponse.LastReceivedMsgSeqNum = 0U;
			serverPlayerState.ConnectionReady = false;
		}
		NetworkConnection connectionPersistent = serverPlayerState.ConnectionPersistent;
		if (conn == null)
		{
			Log.Error("LoginRequest from {0} is using type {1} connection, not ServerExternalConnection", new object[]
			{
				serverPlayerState,
				conn.GetType().ToString()
			});
			loginResponse.ErrorMessage = string.Format("{0} is not a ServerExternalConnection", conn.GetType().ToString());

			// TODO LOW check
			// custom
			NetworkServer.SendToClient(conn.connectionId, (short)MyMsgType.LoginResponse, loginResponse);
			// rogues
			//NetworkServer.SendToClient<GameManager.LoginResponse>(conn.connectionId, loginResponse);
			return;
		}
		if (connectionPersistent != null && connectionPersistent != conn)
		{
			Log.Info("Player {0} already has connection from {1} to game {2}. disconnecting | persistent=({3})", new object[]
			{
				serverPlayerState,
				connectionPersistent.address,
				serverPlayerState.GameInfo.GameServerProcessCode,
				connectionPersistent.connectionId
			});
			MyNetworkManager.Get().OnServerDisconnect(connectionPersistent);
		}
		Log.Info("Player {0} has authenticated from {1} to game {2} | persistent=({3})", new object[]
		{
			serverPlayerState,
			conn.address,
			serverPlayerState.GameInfo.GameServerProcessCode,
			conn.connectionId
		});
		serverPlayerState.ConnectionPersistent = conn;
		serverPlayerState.ConnectionId = conn.connectionId;
		serverPlayerState.ConnectionAddress = conn.address;
		loginResponse.Success = true;

		// TODO LOW check
		// custom
		NetworkServer.SendToClient(conn.connectionId, (short)MyMsgType.LoginResponse, loginResponse);
		// rogues
		//NetworkServer.SendToClient<GameManager.LoginResponse>(conn.connectionId, loginResponse);

		serverPlayerState.LogGameEnter();
		if (isReconnecting)
		{
			GameFlow.Get().ReplaceWithHumans(serverPlayerState.ConnectionPersistent, serverPlayerState.SessionInfo.AccountId);
		}
	}

	private void HandleAssetsLoadedNotification(NetworkConnection conn, GameManager.AssetsLoadedNotification notification)
	{
		if (!IsServer() || notification == null)
		{
			return;
		}

		if (!m_serverPlayerStates.TryGetValue(notification.PlayerId, out ServerPlayerState serverPlayerState)
			|| serverPlayerState.SessionInfo == null
			|| serverPlayerState.ConnectionPersistent != conn)
		{
			Log.Error("Received invalid assets loading notification from {0} | conn=({1}), connPersistent=({2}), playerState={3}, accountId={4}", new object[]
			{
				conn.address,
				conn,
				(serverPlayerState != null) ? serverPlayerState.ConnectionPersistent.ToString() : "(none)",
				serverPlayerState,
				notification.AccountId
			});
			return;
		}
		Log.Info("Player {0} has loaded all assets ({1:0.00} secs). {2}", new object[]
		{
			serverPlayerState.SessionInfo.Name,
			Time.unscaledTime - GameManager.Get().GameStatusTime,
			GameManager.Get().GameInfo.Name
		});
		serverPlayerState.GameLoadingState.IsLoaded = true;
		SetClientReady(serverPlayerState);
	}

	private void HandleLeaveGameNotification(NetworkConnection conn, GameManager.LeaveGameNotification notification)
	{
		if (!IsServer())
		{
			return;
		}
		if (notification == null)
		{
			return;
		}
		ServerPlayerState serverPlayerState;
		if (!m_serverPlayerStates.TryGetValue((long)notification.PlayerId, out serverPlayerState) || serverPlayerState.SessionInfo == null || conn != serverPlayerState.ConnectionPersistent)
		{
			Log.Error("Received invalid leave game notification from {0}", new object[]
			{
				conn.address
			});
			return;
		}
		serverPlayerState.LogGameExit(notification.GameResult);
	}

	private void SetClientReady(ServerPlayerState playerState)
	{
        if (GameFlowData.Get() != null
            && (GameFlowData.Get().gameState == GameState.BothTeams_Resolve
                || GameFlowData.Get().CurrentTurn > 1)
            && (!GameFlowData.Get().IsInDecisionState()
                || GameFlowData.Get().GetTimeRemainingInDecision() / GameFlowData.Get().m_turnTime <= 0.8f)
            && GameFlowData.Get().gameState != GameState.EndingGame)
        {
            Log.Info("Delaying spawn message for player {0} until the resolve phase is complete", new object[]
            {
            playerState.SessionInfo.Name
            });
            m_playersToReadyNextDecisionPhase.Add(playerState);
			return;
        }

		int num = playerState.PlayerInfo.LobbyPlayerInfo.IsSpectator ? 2 : 1;
        int playerObjectCount = m_serverPlayerStates.Count * (1 + num);
        int totalObjectCount = NetworkServer.objects.Count; //  int count = NetworkIdentity.spawned.Count; in rogues
        Log.Info("Sending spawn message for player {0} (playerObjectCount={1} totalObjectCount={2})", new object[]
        {
            (playerState.SessionInfo != null) ? playerState.SessionInfo.Name : "(unnamed player)",
            playerObjectCount,
            totalObjectCount
        });
        GameManager.SpawningObjectsNotification spawningObjectsNotification = new GameManager.SpawningObjectsNotification();
        spawningObjectsNotification.PlayerId = playerState.PlayerInfo.PlayerId;
        spawningObjectsNotification.SpawnableObjectCount = totalObjectCount;

        // TODO LOW check
        // custom
        playerState.ConnectionPersistent.Send((short)MyMsgType.SpawningObjectsNotification, spawningObjectsNotification);
		// rogues
		//playerState.ConnectionPersistent.Send<GameManager.SpawningObjectsNotification>(spawningObjectsNotification, 0);

		NetworkServer.SetClientReady(playerState.ConnectionPersistent);
        playerState.ConnectionReady = true;
        Log.Warning("Not calling SendReconnectData...");
	}

	private void HandleClientAssetsLoadingProgressUpdate(NetworkConnection conn, GameManager.AssetsLoadingProgress loadingProgressInfo)
	{
		if (!IsServer() || loadingProgressInfo == null)
		{
			return;
		}

        ServerPlayerState serverPlayerState = m_serverPlayerStates.TryGetValue(loadingProgressInfo.PlayerId);
		if (serverPlayerState == null || serverPlayerState.SessionInfo == null || serverPlayerState.ConnectionPersistent != conn)
		{
			Log.Error("Received invalid assets loading progress from {0} | conn=({1}), playerState={2}, accountId={3}", new object[]
			{
				conn.address,
				conn,
				serverPlayerState,
				loadingProgressInfo.AccountId
			});
			return;
		}
		serverPlayerState.GameLoadingState.TotalLoadingProgress = loadingProgressInfo.TotalLoadingProgress;
		serverPlayerState.GameLoadingState.LevelLoadingProgress = loadingProgressInfo.LevelLoadingProgress;
		serverPlayerState.GameLoadingState.CharacterLoadingProgress = loadingProgressInfo.CharacterLoadingProgress;
		serverPlayerState.GameLoadingState.VfxLoadingProgress = loadingProgressInfo.VfxLoadingProgress;
		serverPlayerState.GameLoadingState.SpawningProgress = loadingProgressInfo.SpawningProgress;
		GameLoadingState gameLoadingState = serverPlayerState.GameLoadingState;
		gameLoadingState.LoadingProgressUpdateCount += 1;
		float loadingProgress = (float)loadingProgressInfo.TotalLoadingProgress / 100f;
		UILoadingScreenPanel.Get().SetLoadingProgress(loadingProgressInfo.PlayerId, loadingProgress, false);
		foreach (ServerPlayerState serverPlayerState2 in m_serverPlayerStates.Values)
		{
			if (serverPlayerState2.ConnectionPersistent != null && !serverPlayerState2.LocalClient)
			{
				// TODO LOW check
				// custom
				NetworkServer.SendToClient(serverPlayerState2.ConnectionPersistent.connectionId, (short)MyMsgType.ServerAssetsLoadingProgressUpdate, loadingProgressInfo);
				// rogues
				//NetworkServer.SendToClient<GameManager.AssetsLoadingProgress>(serverPlayerState2.ConnectionPersistent.connectionId, loadingProgressInfo);
			}
		}
	}

	public void ClientPreparedForGameStart(int playerId)
	{
		ServerPlayerState serverPlayerState;
		if (m_serverPlayerStates.TryGetValue((long)playerId, out serverPlayerState))
		{
			serverPlayerState.GameLoadingState.IsReady = true;
		}
	}

	private void HandleClientPreparedForGameStartNotification(NetworkConnection conn, GameManager.PlayerObjectStartedOnClientNotification notification)
	{
		if (!IsServer())
		{
			return;
		}
		if (notification == null)
		{
			return;
		}
		ServerPlayerState serverPlayerState;
		if (!m_serverPlayerStates.TryGetValue((long)notification.PlayerId, out serverPlayerState) || serverPlayerState.SessionInfo == null || conn != serverPlayerState.ConnectionPersistent)
		{
			Log.Error("Received invalid PlayerObjectStartedOnClientNotification from {0}", new object[]
			{
				conn.address
			});
			return;
		}
		serverPlayerState.GameLoadingState.IsReady = true;
		Log.Info("Player {0} ActorData started on client", new object[]
		{
			serverPlayerState.SessionInfo.Name
		});
	}

	private void HandleClientRequestTimeUpdate(NetworkMessage msg)
	{
		if (!IsServer())
		{
			return;
		}
		GameManager.PlayerObjectStartedOnClientNotification playerObjectStartedOnClientNotification = msg.ReadMessage<GameManager.PlayerObjectStartedOnClientNotification>();
		if (playerObjectStartedOnClientNotification == null)
		{
			return;
		}
		ServerPlayerState serverPlayerState;
		if (!m_serverPlayerStates.TryGetValue((long)playerObjectStartedOnClientNotification.PlayerId, out serverPlayerState) || serverPlayerState.SessionInfo == null || msg.conn != serverPlayerState.ConnectionPersistent)
		{
			Log.Error("Received invalid PlayerObjectStartedOnClientNotification from {0}", new object[]
			{
				msg.conn.address
			});
			return;
		}
		if (GameFlowData.Get() != null && GameFlowData.Get().CurrentTurn > 1)
		{
			float timeSinceMatchStart = MatchLogger.Get().GetTimeSinceMatchStart();
			GameFlow.Get().SendMatchTime(timeSinceMatchStart);
		}
		Log.Info("Player {0} requested time update", new object[]
		{
			serverPlayerState.SessionInfo.Name
		});
	}

	private void HandleClientFakeActionRequest(NetworkConnection conn, GameManager.FakeActionRequest request)
	{
		if (!IsServer())
		{
			return;
		}
		if (request == null)
		{
			return;
		}
		ServerPlayerState serverPlayerState;
		if (!m_serverPlayerStates.TryGetValue((long)request.PlayerId, out serverPlayerState) || serverPlayerState.SessionInfo == null || conn != serverPlayerState.ConnectionPersistent)
		{
			Log.Error("Received invalid FakeActionRequest from {0}", new object[]
			{
				conn.address
			});
			return;
		}
		GameManager.FakeActionResponse fakeActionResponse = new GameManager.FakeActionResponse();
		fakeActionResponse.msgSize = UnityEngine.Random.Range(32, 1500);  // CryptoRandom.RangeInt32(32, 1500) in rogues
		fakeActionResponse.msgData = new byte[fakeActionResponse.msgSize];

		// TODO LOW check
		// custom
		NetworkServer.SendToClient(conn.connectionId, (short)MyMsgType.ServerFakeActionResponse, fakeActionResponse);
		// rogues
		//NetworkServer.SendToClient<GameManager.FakeActionResponse>(conn.connectionId, fakeActionResponse);
	}

	public bool ClientsPreparedForGameStart()
	{
		bool result = true;
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			if (serverPlayerState.ConnectionPersistent != null && !serverPlayerState.PlayerInfo.IsAIControlled && !serverPlayerState.GameLoadingState.IsReady)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public void LogAllClientStates()
	{
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			Log.Info(serverPlayerState.GameLoadingState.ToString());
		}
	}

	private void DestroyPrefabsForGameScenes()
	{
		Log.Info("DestroyPrefabsForGameScenes");
		if (NetworkServer.active)
		{
			foreach (GameObject gameObject in m_instancesForGameScenes)
			{
				NetworkServer.Destroy(gameObject);
			}
		}
		m_instancesForGameScenes.Clear();
	}

	private void InstantiatePrefabsForGameScenes()
	{
		Log.Info("InstantiatePrefabsForGameScenes");
		if (PrefabsForGameScenes != null && NetworkServer.active)
		{
			GameObject[] prefabsForGameScenes = PrefabsForGameScenes;
			for (int i = 0; i < prefabsForGameScenes.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefabsForGameScenes[i]);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				m_instancesForGameScenes.Add(gameObject);
				NetworkServer.Spawn(gameObject);
			}
		}
	}

	private void LoadAssets()
	{
		Log.Info("ServerGameManager::LoadAssets");
		if (!IsServer())
		{
			return;
		}
		m_loading = true;
		m_loadLevelOperationDone = false;
		m_loadedCharacterResourceCount = 0;
		m_assetsLoadingState.Reset();
		GameManager gameManager = GameManager.Get();
		InstantiatePrefabsForGameScenes();
		string bundleName;
		if (AssetBundleManager.Get().SceneExistsInBundle("testing", gameManager.GameConfig.Map))  //  gameManager.GameMission.Map in rogues
		{
			bundleName = "testing";
		}
		else
		{
			bundleName = "maps";
		}
		SetGameStatus(GameStatus.Loading, true);
		m_loadLevelOperation = new AssetBundleManager.LoadSceneAsyncOperation
		{
			sceneName = gameManager.GameConfig.Map,  //  gameManager.GameMission.Map in rogues
			bundleName = bundleName,
			loadSceneMode = 0
		};
		StartCoroutine(AssetBundleManager.Get().LoadSceneAsync(m_loadLevelOperation));
		m_loadingCharacterResources.Clear();
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			if (serverPlayerState.PlayerInfo.TeamId == Team.TeamA || serverPlayerState.PlayerInfo.TeamId == Team.TeamB)
			{
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(serverPlayerState.PlayerInfo.CharacterType);
				m_loadingCharacterResources.Add(characterResourceLink);
				ServerPlayerInfo closureServerPlayerInfo = serverPlayerState.PlayerInfo;
				characterResourceLink.LoadAsync(serverPlayerState.PlayerInfo.CharacterSkin, delegate(LoadedCharacterSelection loadedCharacter)
				{
					m_loadingCharacterResources.Remove(loadedCharacter.resourceLink);
					m_loadedCharacterResourceCount++;
					closureServerPlayerInfo.SelectedCharacter = loadedCharacter;
				});
				foreach (ServerPlayerInfo serverPlayerInfo in closureServerPlayerInfo.ProxyPlayerInfos)
				{
					CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(serverPlayerInfo.CharacterType);
					m_loadingCharacterResources.Add(characterResourceLink2);
					ServerPlayerInfo closureProxyPlayerInfo = serverPlayerInfo;
					characterResourceLink2.LoadAsync(serverPlayerInfo.CharacterSkin, delegate(LoadedCharacterSelection loadedCharacter)
					{
						m_loadingCharacterResources.Remove(loadedCharacter.resourceLink);
						m_loadedCharacterResourceCount++;
						closureProxyPlayerInfo.SelectedCharacter = loadedCharacter;
					});
				}
			}
		}
	}

	public void LoadCharacterResourceLink(CharacterResourceLink characterResourceLink, CharacterVisualInfo linkVisualInfo)  // linkVisualInfo = null in rogues
	{
		m_loadingCharacterResources.Add(characterResourceLink);
		characterResourceLink.LoadAsync(linkVisualInfo, delegate(LoadedCharacterSelection loadedCharacter)
		{
			m_loadingCharacterResources.Remove(loadedCharacter.resourceLink);
			m_loadedCharacterResourceCount++;
		});
	}

	private void UpdateLoadProgress(bool force = false)
	{
		if (Time.unscaledTime > m_lastLoadProgressUpdateSent + m_loadingProgressUpdateFrequency || force)
		{
			m_lastLoadProgressUpdateSent = Time.unscaledTime;
			GameManager gameManager = GameManager.Get();
			if (gameManager.PlayerInfo != null && gameManager.TeamInfo != null)
			{
				m_assetsLoadingState.LevelLoadProgress = (m_loadLevelOperationDone ? 1f : ((m_loadLevelOperation != null) ? m_loadLevelOperation.progress : 0f));
				m_assetsLoadingState.CharacterLoadProgress = Mathf.Clamp((float)m_loadedCharacterResourceCount / (float)gameManager.TeamInfo.TotalPlayerCount, 0f, 1f);
				m_assetsLoadingState.VfxPreloadProgress = ((ClientVFXLoader.Get() != null) ? ClientVFXLoader.Get().Progress : 0f);
				if (ClientScene.spawnableObjects != null)
				{
					int count = ClientScene.spawnableObjects.Count;
					int num = (from s in ClientScene.spawnableObjects
					where s.Value.gameObject.activeSelf
					select s).Count();
					m_assetsLoadingState.SpawningProgress = ((count > 0) ? Mathf.Clamp((float)num / (float)count, 0f, 1f) : 1f);
				}
				else
				{
					m_assetsLoadingState.SpawningProgress = 1f;
				}
				GameManager.AssetsLoadingProgress assetsLoadingProgress = new GameManager.AssetsLoadingProgress();
				assetsLoadingProgress.AccountId = gameManager.PlayerInfo.AccountId;
				assetsLoadingProgress.PlayerId = gameManager.PlayerInfo.PlayerId;
				assetsLoadingProgress.TotalLoadingProgress = (byte)(m_assetsLoadingState.TotalProgress * 100f);
				assetsLoadingProgress.LevelLoadingProgress = (byte)(m_assetsLoadingState.LevelLoadProgress * 100f);
				assetsLoadingProgress.CharacterLoadingProgress = (byte)(m_assetsLoadingState.CharacterLoadProgress * 100f);
				assetsLoadingProgress.VfxLoadingProgress = (byte)(m_assetsLoadingState.VfxPreloadProgress * 100f);
				assetsLoadingProgress.SpawningProgress = (byte)(m_assetsLoadingState.SpawningProgress * 100f);
				UILoadingScreenPanel.Get().SetLoadingProgress(assetsLoadingProgress.PlayerId, m_assetsLoadingState.TotalProgress, true);
				foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
				{
					if (serverPlayerState.ConnectionPersistent != null && !serverPlayerState.LocalClient)
					{
						// TODO LOW check
						// custom
						NetworkServer.SendToClient(serverPlayerState.ConnectionPersistent.connectionId, (short)MyMsgType.ServerAssetsLoadingProgressUpdate, assetsLoadingProgress);
						// rogues
						//NetworkServer.SendToClient<GameManager.AssetsLoadingProgress>(serverPlayerState.ConnectionPersistent.connectionId, assetsLoadingProgress);
					}
				}
			}
		}
	}

	private void CheckLoaded()
	{
		if (!IsServer())
		{
			return;
		}
		CommonServerConfig commonServerConfig = CommonServerConfig.Get();
		GameManager gameManager = GameManager.Get();
		bool flag = m_loadLevelOperationDone && (GameFlowData.Get() == null || GameFlowData.Get().gameState < GameState.Deployment);
		if (m_loading || flag)
		{
			UpdateLoadProgress(false);
		}
		if (gameManager.GameStatus == GameStatus.Loading)
		{
			if (m_loadLevelOperation != null && m_loadLevelOperation.isDone)
			{
				m_loadLevelOperation = null;
				m_loadLevelOperationDone = true;
				VisualsLoader visualsLoader = VisualsLoader.Get();
				if (visualsLoader != null && visualsLoader.LevelLoaded())  // (visualsLoader != null && visualsLoader.EncounterLoaded()) in rogues, probably can be removed
				{
					NetworkServer.SpawnObjects();
				}
			}
			bool flag2 = true;
			if (NPCCoordinator.Get() != null)
			{
				flag2 = (NPCCoordinator.Get().LoadingState == NPCCoordinator.LoadingStateEnum.Done);
			}
			if (m_loadLevelOperation == null && m_loadingCharacterResources.Count == 0 && (VisualsLoader.Get() == null || VisualsLoader.Get().LevelLoaded()) && flag2)
			{
				m_loading = false;
				foreach (ServerPlayerState sps in m_serverPlayerStates.Values)
				{
					if (sps.LocalClient && !sps.GameLoadingState.IsLoaded)
					{
						sps.GameLoadingState.IsLoaded = true;
						break;
					}
				}
				UpdateLoadProgress(true);
			}
			bool arePlayersStillLoading = false;
			foreach (ServerPlayerState sps in m_serverPlayerStates.Values)
			{
				if (sps.ConnectionPersistent != null
					&& !sps.PlayerInfo.IsAIControlled
					&& !sps.LocalClient
					&& !sps.GameLoadingState.IsLoaded)
				{
					arePlayersStillLoading = true;
					break;
				}
				if ((sps.ConnectionPersistent == null
						|| sps.ConnectionPersistent.connectionId < 0)
					&& !sps.PlayerInfo.IsAIControlled
					&& !sps.LocalClient)
				{
					arePlayersStillLoading = true;
					break;
				}
			}
			if (arePlayersStillLoading
				&& Time.unscaledTime - gameManager.GameStatusTime > commonServerConfig.GameServerClientLoadTimeout.TotalSeconds)
			{
				foreach (ServerPlayerState sps in m_serverPlayerStates.Values)
				{
					if (!sps.PlayerInfo.IsAIControlled
						&& !sps.LocalClient
						&& !sps.GameLoadingState.IsLoaded)
					{
						int connectionId = sps?.ConnectionPersistent.connectionId ?? -1;
						Log.Info($"Player {sps.PlayerInfo.Handle} {sps.SessionInfo.AccountId} (connectionId {connectionId})" +
							$" has failed to load, and will now be controlled by a bot ({sps.GameLoadingState}). {gameManager.GameInfo.Name}");

						SendConsoleMessageWithHandle("PlayerFailedToLoad", "Disconnect", sps.PlayerInfo.Handle, sps.PlayerInfo.TeamId, ConsoleMessageType.SystemMessage, null);
						sps.DisconnectAndReplaceWithBots(GameResult.ClientLoadingTimeout);
					}
				}
				Log.Info($"Loading timed out with {m_serverPlayerStates.Count} server player states");
				arePlayersStillLoading = false;
			}
			if (!m_loading && !arePlayersStillLoading && GameFlow.Get() != null && GameFlowData.Get() != null)
			{
				Log.Info($"Changing to loaded state with {m_serverPlayerStates.Count} server player states");
				foreach (ServerPlayerState sps in m_serverPlayerStates.Values)
				{
					if (sps.ConnectionPersistent != null
						&& !sps.ConnectionPersistent.isReady
						&& !sps.LocalClient)
					{
						SetClientReady(sps);
					}
				}
				SetGameStatus(GameStatus.Loaded, true);
				StartGame();
			}
		}
	}

	private void StartGame()
	{
		if (!IsServer())
		{
			return;
		}
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			if (serverPlayerState.PlayerInfo.IsGameOwner && NetworkClient.active)
			{
				// TODO check
				ClientScene.AddPlayer(serverPlayerState.ConnectionPersistent, 0);  // ClientScene.AddPlayer(serverPlayerState.ConnectionPersistent); in rogues
			}
			GameFlow.Get().AddPlayer(serverPlayerState);
		}
		GameFlowData.Get().gameState = GameState.SpawningPlayers;
		SetGameStatus(GameStatus.Started, true);
		SendQueuedConsoleMessages();
	}

	private void CheckDisconnected()
	{
		if (!IsServer())
		{
			return;
		}
		GameManager gameManager = GameManager.Get();
		if (gameManager.GameStatus == GameStatus.Started
			// TODO LOW check this
			// rogues
			//&& !gameManager.GameMission.IsMissionTagActive(MissionData.s_missionTagFakeClients)
			&& !AreClientsConnected())
		{
			Log.Notice("All clients have disconnected, stopping game");
			GameManager.Get().StopGame(GameResult.NoResult);
		}
	}

	public bool AreClientsConnected()
	{
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			if ((serverPlayerState.ConnectionPersistent != null && serverPlayerState.ConnectionPersistent.connectionId > 0) || serverPlayerState.LocalClient)
			{
				return true;
			}
		}
		return false;
	}

	private void HandleDisconnectPlayerRequest(DisconnectPlayerRequest request)
	{
		ServerPlayerState serverPlayerState = null;
		m_serverPlayerStates.TryGetValue((long)request.PlayerInfo.PlayerId, out serverPlayerState);
		if (serverPlayerState == null)
		{
			return;
		}
		if (serverPlayerState.SessionInfo == null || serverPlayerState.SessionInfo.AccountId != request.SessionInfo.AccountId || serverPlayerState.SessionInfo.SessionToken != request.SessionInfo.SessionToken || serverPlayerState.ConnectionPersistent == null)
		{
			return;
		}
		serverPlayerState.LogGameExit(request.GameResult);
		Log.Info("Disconnecting player {0} {1} (connectionId {2})", new object[]
		{
			request.SessionInfo.Handle,
			request.SessionInfo.AccountId,
			serverPlayerState.ConnectionPersistent.connectionId
		});
		serverPlayerState.ConnectionPersistent.Disconnect();
	}

	private void HandleReconnectPlayerRequest(ReconnectPlayerRequest request)
	{
		ReconnectPlayerResponse reconnectPlayerResponse = new ReconnectPlayerResponse();
		reconnectPlayerResponse.ResponseId = request.RequestId;
		ServerPlayerState playerStateByAccountId = GetPlayerStateByAccountId(request.AccountId);
		if (playerStateByAccountId != null)
		{
			playerStateByAccountId.SessionInfo.ReconnectSessionToken = request.NewSessionId;
			reconnectPlayerResponse.Success = true;
			Log.Info("Lobby gave us a new session token for player {0} {1}", new object[]
			{
				playerStateByAccountId.SessionInfo.Name,
				playerStateByAccountId.SessionInfo.ReconnectSessionToken
			});
		}
		else
		{
			reconnectPlayerResponse.ErrorMessage = string.Format("Lobby gave use a new session token for account id {0} but we don't have that account as a player!", request.AccountId);
			reconnectPlayerResponse.Success = false;
			Log.Warning(reconnectPlayerResponse.ErrorMessage);
		}
		// monitor
		//m_monitorGameServerInterface.Send(reconnectPlayerResponse);
	}

	private void HandleShutdownGameRequest(ShutdownGameRequest request)
	{
		Log.Info("Received shutdown game request");
		GameManager.Get().StopGame(GameResult.NoResult);
	}

	public bool IsServer()
	{
		return NetworkServer.active;
	}

	public ServerPlayerState GetPlayerStateByAccountId(long accountId)
	{
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			if (serverPlayerState.SessionInfo != null && serverPlayerState.SessionInfo.AccountId == accountId)
			{
				return serverPlayerState;
			}
		}
		return null;
	}

	public ServerPlayerState GetPlayerStateByConnectionId(int connectionId)
	{
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			if (serverPlayerState.ConnectionPersistent != null && serverPlayerState.ConnectionPersistent.connectionId == connectionId)
			{
				return serverPlayerState;
			}
		}
		return null;
	}

	public long GetPlayerAccountIdByConnectionId(int connectionId)
	{
		ServerPlayerState playerStateByConnectionId = GetPlayerStateByConnectionId(connectionId);
		if (playerStateByConnectionId != null && playerStateByConnectionId.PlayerInfo != null)
		{
			return playerStateByConnectionId.PlayerInfo.LobbyPlayerInfo.AccountId;
		}
		return -1L;
	}

	private static string GetNetworkErrorString(byte b)
	{
		return "unknownNetError";
	}

	internal int GetServerConnectionCurrentRtt(int connectionId)
	{
		ServerPlayerState playerStateByConnectionId = GetPlayerStateByConnectionId(connectionId);
		if (playerStateByConnectionId == null)
		{
			return -1;
		}
		if (playerStateByConnectionId.ConnectionPersistent == null)
		{
			return -1;
		}
		NetworkConnection connectionPersistent = playerStateByConnectionId.ConnectionPersistent;

		// custom
		return NetworkTransport.GetCurrentRTT(connectionPersistent.hostId, connectionPersistent.connectionId, out byte _);
		// rogues
		//return (int)NetworkTime.rtt;
	}

	internal int GetPlayerConnectionId(long accountId)
	{
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			if (serverPlayerState.SessionInfo != null && serverPlayerState.SessionInfo.AccountId == accountId)
			{
				return serverPlayerState.ConnectionId;
			}
		}
		return -1;
	}

	public bool IsAccountReconnecting(long accountId)
	{
		if (m_reconnectingAccountIds.Contains(accountId))
		{
			return true;
		}
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			if (serverPlayerState.SessionInfo != null && serverPlayerState.SessionInfo.AccountId == accountId)
			{
				if (m_playersToReadyNextDecisionPhase.Contains(serverPlayerState))
				{
					return true;
				}
				break;
			}
		}
		return false;
	}

	public void SendConsoleMessage(string term, string context, Team team = Team.Invalid, ConsoleMessageType messageType = ConsoleMessageType.SystemMessage, string senderHandle = null)
	{
		if (GameManager.Get().GameStatus == GameStatus.Started)
		{
			if (GameFlow.Get() != null)
			{
				GameFlow.Get().DisplayConsoleText(term, context, "", "", messageType, team, senderHandle);
				return;
			}
		}
		else
		{
			m_queuedConsoleMessages.Add(new QueuedConsoleMessage(term, context, "", "", messageType, team, senderHandle));
		}
	}

	public void SendConsoleMessageWithHandle(string term, string context, string handle, Team team = Team.Invalid, ConsoleMessageType messageType = ConsoleMessageType.SystemMessage, string senderHandle = null)
	{
		if (GameManager.Get().GameStatus == GameStatus.Started)
		{
			if (GameFlow.Get() != null)
			{
				GameFlow.Get().DisplayConsoleText(term, context, handle, "", messageType, team, senderHandle);
				return;
			}
		}
		else
		{
			m_queuedConsoleMessages.Add(new QueuedConsoleMessage(term, context, handle, "", messageType, team, senderHandle));
		}
	}

	public void SendUnlocalizedConsoleMessage(string message, Team team = Team.Invalid, ConsoleMessageType messageType = ConsoleMessageType.SystemMessage, string senderHandle = null)
	{
		if (GameManager.Get().GameStatus == GameStatus.Started)
		{
			if (GameFlow.Get() != null)
			{
				GameFlow.Get().DisplayConsoleText("", "", "", message, messageType, team, senderHandle);
				return;
			}
		}
		else
		{
			m_queuedConsoleMessages.Add(new QueuedConsoleMessage("", "", "", message, messageType, team, senderHandle));
		}
	}

	private void SendQueuedConsoleMessages()
	{
		if (GameFlow.Get() != null)
		{
			foreach (QueuedConsoleMessage queuedConsoleMessage in m_queuedConsoleMessages)
			{
				GameFlow.Get().DisplayConsoleText(queuedConsoleMessage.m_term, queuedConsoleMessage.m_context, queuedConsoleMessage.m_token, queuedConsoleMessage.m_unlocalized, queuedConsoleMessage.m_type, queuedConsoleMessage.m_team, queuedConsoleMessage.m_senderHandle);
			}
		}
		m_queuedConsoleMessages.Clear();
	}

	public int NumCharacterResourcesCurrentlyLoading
	{
		get
		{
			if (m_loadingCharacterResources == null)
			{
				return 0;
			}
			return m_loadingCharacterResources.Count;
		}
		private set
		{
		}
	}

	public int NumCharactersInGame
	{
		get
		{
			if (m_serverPlayerStates == null)
			{
				return 0;
			}
			int num = 0;
			foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
			{
				if (serverPlayerState != null)
				{
					num++;
					if (serverPlayerState.PlayerInfo != null && serverPlayerState.PlayerInfo.ProxyPlayerInfos != null)
					{
						num += serverPlayerState.PlayerInfo.ProxyPlayerInfos.Count;
					}
				}
			}
			return num;
		}
		private set
		{
		}
	}

	public int NumHumanControlledCharactersInGame
	{
		get
		{
			if (m_serverPlayerStates == null)
			{
				return 0;
			}
			int num = 0;
			foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
			{
				if (serverPlayerState != null && !serverPlayerState.IsAIControlled)
				{
					num++;
					if (serverPlayerState.PlayerInfo != null && serverPlayerState.PlayerInfo.ProxyPlayerInfos != null)
					{
						num += serverPlayerState.PlayerInfo.ProxyPlayerInfos.Count;
					}
				}
			}
			return num;
		}
		private set
		{
		}
	}

	// monitor
	//private void HandleMonitorHeartbeatResponse(MonitorHeartbeatResponse response)
	//{
	//}

	private class QueuedConsoleMessage
	{
		public string m_term;

		public string m_context;

		public string m_token;

		public string m_unlocalized;

		public ConsoleMessageType m_type;

		public Team m_team;

		public string m_senderHandle;

		public QueuedConsoleMessage(string term, string context, string token, string unlocalized, ConsoleMessageType type, Team team, string senderHandle)
		{
			m_term = term;
			m_context = context;
			m_token = token;
			m_unlocalized = unlocalized;
			m_type = type;
			m_team = team;
			m_senderHandle = senderHandle;
		}
	}
#endif
}
