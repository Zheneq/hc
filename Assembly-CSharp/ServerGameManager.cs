// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ArtemisServer.BridgeServer;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
//using System.Net;
//using System.Threading.Tasks;
//using CoOp;
//using Mirror;
//using Mirror.Websocket;
//using Open.Nat;

// server-only -- was empty in reactor
public class ServerGameManager : MonoBehaviour
{
#if SERVER
	// custom
	private const bool ENABLE_RECONNECT_REPLAY = false;
	
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

	private ArtemisBridgeServerInterface m_monitorGameServerInterface; // MonitorGameServerInterface in rogues

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

	public event Action<RegisterGameServerResponse> OnConnectedToMonitorServer = delegate { };
	public event Action<string> OnDisconnectedFromMonitorServer = delegate { };

	public string ListenAddress { get; private set; }
	public int ListenPort { get; private set; }

	public bool IsConnectedToMonitorServer => m_monitorGameServerInterface != null && m_monitorGameServerInterface.isConnected;

	// custom
	private readonly Dictionary<Team, ReplayRecorder> m_replayRecorders = new Dictionary<Team, ReplayRecorder>();
	
	// custom Artemis
	public static Dictionary<string, GameObject> ResourceNetworkObjects = new Dictionary<string, GameObject>();

	// custom Artemis
	private GameObject SpawnObject(string name, bool activate = true, bool network = true)
	{
		Log.Info($"Spawning {name}");
		GameObject prefab = ResourceNetworkObjects[name];

		if (prefab == null)
		{
			Log.Error($"Not found: {name}");
			return null;
		}
		Log.Info($"Prefab {name}");
		foreach (var ni in prefab.GetComponents<NetworkIdentity>())
		{
			Log.Info($"Prefab {name} - ni {ni.GetType().Name}");
		}
		foreach (var nb in prefab.GetComponents<NetworkBehaviour>())
		{
			Log.Info($"Prefab {name} - nb {nb.GetType().Name}");
		}

		GameObject obj = Instantiate(prefab);
		Log.Info($"Instantiated {name}");

		if (activate)
		{
			obj.SetActive(true);
			Log.Info($"Activated {name}");
		}

		if (network)
		{
			NetworkServer.Spawn(obj);
			Log.Info($"Network spawned {name}");
		}
		return obj;
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

		// TODO HACK
		GameObject ServerBootstrap = new GameObject("ServerBootstrap");
		ServerBootstrap.AddComponent<ServerBootstrap>();
		ServerBootstrap.AddComponent<ClientGamePrefabInstantiator>();
		ServerBootstrap.AddComponent<MatchLogger>();
		//ServerBootstrap.AddComponent<GameFlow>();
		//ServerBootstrap.AddComponent<GameFlowData>();
		//ServerBootstrap.AddComponent<PowerUpManager>();
		//ServerBootstrap.AddComponent<ServerEffectManager>();
		//ServerBootstrap.AddComponent<SequenceManager>();
		//ServerBootstrap.AddComponent<TriggerCoordinator>();
		ServerBootstrap.AddComponent<TheatricsManager>();

		ServerBootstrap.AddComponent<TeamStatusDisplay>();
		//ServerBootstrap.AddComponent<GameplayData>();
		//ServerBootstrap.AddComponent<SpawnPointManager>(); // should be loaded (probably, when the map is loaded) causes NPE when not properly initialized
		// end TODO HACK

		// custom
		SceneManager.sceneLoaded += OnSceneLoaded; // TODO remove callback on destroy

		// TODO HACK Artemis
		NetworkIdentity[] objects = Resources.FindObjectsOfTypeAll<NetworkIdentity>();
		foreach (NetworkIdentity netid in objects)
		{
			GameObject obj = netid.gameObject;
			ResourceNetworkObjects.Add(obj.name, obj);
		}
		// end TODO HACK Artemis
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Log.Info($"Loaded scene {scene.name}");

		//if (scene.name == "DevEnvironmentSingletons")
		//{
		//	// TODO HACK -- trying to get HighlightUtils and stuff
		//	StartCoroutine(AssetBundleManager.Get().LoadSceneAsync("ServerEnvironmentSingletons", "frontend", LoadSceneMode.Additive));
		//	return;
		//}

		foreach (GameObject g in scene.GetRootGameObjects())
		{
			foreach (var c in g.GetComponents(typeof(Component)))
			{
				if (c)
				{
					Log.Info($"Scene {scene.name} - GameObject {g.name} - Component {c.GetType()}");

					if (c.GetType() == typeof(PrefabInstantiator))
					{
						String prefabList = String.Join(", ", g.GetComponent<PrefabInstantiator>().m_prefabs.Select(x => "{" + x.name + "}[" + String.Join(", ", x.GetComponents<MonoBehaviour>().Select(y => y.GetType().ToString()).ToArray()) + "]").ToArray());
						Log.Info($"Prefab instantiator: {prefabList}");
					}

					if (c.GetType() == typeof(ClientGamePrefabInstantiator))
					{
						String prefabList = String.Join(", ", g.GetComponent<ClientGamePrefabInstantiator>().m_prefabs.Select(x => "{" + x.name + "}[" + String.Join(", ", x.GetComponents<MonoBehaviour>().Select(y => y.GetType().ToString()).ToArray()) + "]").ToArray());
						Log.Info($"Client game prefab instantiator: {prefabList}");
					}
				}
			}
		}
	}

	protected void Start()
	{
		GameManager.Get().OnGameLaunched += HandleGameLaunched;
		GameManager.Get().OnGameStopped += HandleGameStopped;
		GameManager.Get().OnGameStatusChanged += HandleGameStatusChanged;
		m_heartBeat.Start();
		// rogues
		// HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		// if (hydrogenConfig != null && hydrogenConfig.MonitorHeartbeatCooldown.TotalSeconds > 0.0)
		// {
		// 	m_heartBeatDuration = hydrogenConfig.MonitorHeartbeatCooldown;
		// }
		// custom temp
		m_heartBeatDuration = TimeSpan.FromMinutes(1);
		ConnectToMonitorServer($"ws://{HydrogenConfig.Get().MonitorServerAddress}:{HydrogenConfig.Get().MonitorServerPort}/BridgeServer", -1);
	}

	protected void Update()
	{
		if (m_monitorGameServerInterface != null)
		{
			m_monitorGameServerInterface.Update();
		}
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
						Log.Info("Allowing reconnecting player {0} ({1}) back in.", serverPlayerState.SessionInfo.Handle, serverPlayerState.SessionInfo.AccountId);
						// custom
						SetClientReady(serverPlayerState);
						// rogues
						// NetworkServer.SetClientReady(serverPlayerState.ConnectionPersistent);
						if (m_reconnectingAccountIds.Contains(serverPlayerState.SessionInfo.AccountId))
						{
							m_reconnectingAccountIds.Remove(serverPlayerState.SessionInfo.AccountId);
							SendConsoleMessageWithHandle("PlayerReconnected", "Disconnect", serverPlayerState.PlayerInfo.LobbyPlayerInfo.Handle, serverPlayerState.PlayerInfo.TeamId);
						}
						// rogues -- happens in ServerGameManager#SetClientReady in reactor
						// serverPlayerState.ConnectionReady = true;
					}
					catch (Exception ex)
					{
						Log.Warning("Exception during NetworkServer.SetClientReady for {0} | {1}", serverPlayerState.ToString(), ex);
					}
				}
				m_playersToReadyNextDecisionPhase.Clear();
			}
			if (ObjectivePoints.Get() != null)
			{
				ServerGameMetrics serverGameMetrics = new ServerGameMetrics
				{
					CurrentTurn = m_lastUpdateTurn,
					TeamAPoints = ObjectivePoints.Get().GetPointsForTeam(Team.TeamA),
					TeamBPoints = ObjectivePoints.Get().GetPointsForTeam(Team.TeamB),
					AverageFrameTime = (from f in m_recentFrameTimes where f != 0f select f).Average()
				};
				Log.Info($"Frame time: {serverGameMetrics.AverageFrameTime * 1000f} ms");
				if (m_monitorGameServerInterface != null)
				{
					m_monitorGameServerInterface.SendGameMetricsNotification(serverGameMetrics);
				}
			}
		}
		if (m_heartBeat.IsRunning && m_heartBeat.Elapsed > m_heartBeatDuration)
		{
			m_monitorGameServerInterface.SendMonitorHeartbeatNotification();
			// custom
			m_heartBeat.Reset();
			m_heartBeat.Start();
			// rogues
			// m_heartBeat.Restart();
		}
	}

	internal void StartServerOrHost() // async Task in rogues
	{
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		if (hydrogenConfig == null)
		{
			throw new Exception("CommonServerConfig must be loaded to run servers or host games");
		}

		ListenAddress = "";
		ListenPort = 0;
		NetworkManager.singleton.networkAddress = "0.0.0.0";

		// artemis
		ListenPort = HydrogenConfig.Get().PublicPort;
		Log.Info("Starting Server...");
		UIFrontendLoadingScreen.Get()?.StartDisplayError("Starting Server...");
		NetworkManager.singleton.useWebSockets = true;
		NetworkManager.singleton.networkPort = HydrogenConfig.Get().PublicPort;
		NetworkManager.singleton.StartServer();
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

		// custom
		NetworkServer.RegisterHandler((short)MyMsgType.ClientRequestTimeUpdate, HandleClientRequestTimeUpdate);

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
		ListenAddress = (commonServerConfig != null ? commonServerConfig.GameServerListenAddress : null) ?? ListenAddress;
		Log.Info("Game Server initialized at {0}:{1}", ListenAddress, ListenPort);

		// new networking lib cont
		//}
		//	throw new Exception("Failed to initialize Game Server");
	}

	internal void ConnectToMonitorServer(string address, short port, long accountId = 0L)
	{
		if (m_monitorGameServerInterface != null)
		{
			return;
		}

		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();

		// custom
		m_monitorGameServerInterface = gameObject.AddComponent<ArtemisBridgeServerInterface>();
		// rogues
		// WebsocketTransport component = base.GetComponent<WebsocketTransport>();
		// component.port = (int)port;
		// component.Secure = true;
		// component.serverName = "MonitorGameServerSessionManager";
		// m_monitorGameServerInterface = base.gameObject.AddComponent<MonitorGameServerInterface>();

		m_monitorGameServerInterface.Initialize(address, port, hydrogenConfig.ProcessType, hydrogenConfig.ProcessCode, accountId);
		m_monitorGameServerInterface.OnConnectedHandler += HandleConnectedToMonitorServer;
		m_monitorGameServerInterface.OnDisconnectedHandler += HandleDisconnectedFromMonitorServer;
		m_monitorGameServerInterface.OnLaunchGameRequest += HandleLaunchGameRequest;
		m_monitorGameServerInterface.OnJoinGameServerRequest += HandleJoinGameServerRequest;
		m_monitorGameServerInterface.OnJoinGameAsObserverRequest += HandleJoinGameAsObserverRequest;
		m_monitorGameServerInterface.OnShutdownGameRequest += HandleShutdownGameRequest;
		m_monitorGameServerInterface.OnDisconnectPlayerRequest += HandleDisconnectPlayerRequest;
		m_monitorGameServerInterface.OnReconnectPlayerRequest += HandleReconnectPlayerRequest;
		m_monitorGameServerInterface.OnMonitorHeartbeatResponse += HandleMonitorHeartbeatResponse;
		m_monitorGameServerInterface.Reconnect();
	}

	public void DisconnectFromMonitorServer()
	{
		if (m_monitorGameServerInterface != null)
		{
			m_monitorGameServerInterface.Disconnect();
		}
	}

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
		// custom
		foreach (ReplayRecorder replayRecorder in m_replayRecorders.Values)
		{
			replayRecorder?.StopRecording();
		}
		SaveReplay();
		// end custom
		
		if (m_monitorGameServerInterface != null)
		{
			Log.Info("Sending game summary to lobby server");
			Log.Info($"{DefaultJsonSerializer.Serialize(GameManager.Get().GameSummary)}");
			m_monitorGameServerInterface.SendGameSummaryNotification(GameManager.Get().GameSummary, GameManager.Get().GameSummaryOverrides);
			m_sentGameSummary = true;
			foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
			{
				if (serverPlayerState.SessionInfo != null
				    && serverPlayerState.PlayerInfo != null
				    && !serverPlayerState.PlayerInfo.IsNPCBot)
				{
					serverPlayerState.LogGameExit(GameManager.Get().GameSummary.GameResult);
				}
			}
		}
		
		// custom
		SendReplay();
		// end custom
	}
	
	// custom
	private void SendReplay()
	{
		int batchSize = 30000;
		m_replayRecorders.TryGetValue(Team.Spectator, out ReplayRecorder replayRecorder);
		string replayJson = replayRecorder?.GetReplayAsJson();

		if (replayJson == null)
		{
			Log.Error($"Failed to send replay (replay not found)");
			return;
		}

		List<GameManager.ReplayManagerFile> replay = new List<GameManager.ReplayManagerFile>();
		for (int start = 0; start < replayJson.Length; start += batchSize)
		{
			replay.Add(new GameManager.ReplayManagerFile
			{
				Fragment = replayJson.Substring(start, Math.Min(batchSize, replayJson.Length - start)),
				Restart = start == 0,
				Save = start + batchSize >= replayJson.Length
			});
		}
		
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			if (serverPlayerState.SessionInfo != null
			    && serverPlayerState.PlayerInfo != null
			    && !serverPlayerState.PlayerInfo.IsAIControlled)
			{
				Log.Info($"Sending replay to {serverPlayerState.PlayerInfo.Handle}");
				try
				{
					foreach (GameManager.ReplayManagerFile replayMsg in replay)
					{
						serverPlayerState.ConnectionPersistent.Send((short)MyMsgType.ReplayManagerFile,
							replayMsg);
					}
				}
				catch (Exception e)
				{
					Log.Info($"Failed to send replay: {e}");
				}
			}
		}
	}

	// custom
	public void SaveReplay()
	{
		foreach (ReplayRecorder replayRecorder in m_replayRecorders.Values)
		{
			replayRecorder?.SaveReplay();
		}
	}

	private void OnDestroy()
	{
		if (GameManager.Get() != null)
		{
			GameManager.Get().StopGame();
			GameManager.Get().OnGameLaunched -= HandleGameLaunched;
			GameManager.Get().OnGameStopped -= HandleGameStopped;
			GameManager.Get().OnGameStatusChanged -= HandleGameStatusChanged;
		}
		DisconnectFromMonitorServer();
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

	private void HandleNetworkError(NetworkConnection conn, NetworkError errorCode) // byte errorCode in rogues
	{
		Log.Error("Client network error from {0}: (connectionId {1}) {2}", conn.address, conn.connectionId, errorCode.ToString());
	}

	private void HandleNetworkConnect(NetworkConnection conn)
	{
		if (!(conn.address == "localhost") || conn.connectionId <= NetworkManager.singleton.maxConnections)
		{
			Log.Info("Client network connected from {0} (connectionId {1})", conn.address, conn.connectionId);
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
			Log.Info("{0} Connection is queued to disconnect (connectionId {1}) ({2}secs)| {3}", playerStateByConnectionId, conn.connectionId, gameServerClientReconnectTimeout.TotalSeconds, gameInfo.Name);
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
			Log.Info("Unknown player disconnected from {0} (connectionId {1})", conn.address, conn.connectionId);
		}
		else
		{
			if (m_monitorGameServerInterface != null && serverPlayerState.SessionInfo != null && serverPlayerState.PlayerInfo != null)
			{
				m_monitorGameServerInterface.SendPlayerDisconnectedNotification(serverPlayerState.SessionInfo, serverPlayerState.PlayerInfo.LobbyPlayerInfo);
			}

			serverPlayerState.ConnectionReady = false;
			bool isGameOver = GameManager.Get().GameStatus == GameStatus.Stopped || (ObjectivePoints.Get() != null && ObjectivePoints.Get().m_matchState == ObjectivePoints.MatchState.MatchEnd);
			bool isSpectator = serverPlayerState.PlayerInfo != null && serverPlayerState.PlayerInfo.TeamId == Team.Spectator;
			long accountId = serverPlayerState.PlayerInfo != null ? serverPlayerState.PlayerInfo.LobbyPlayerInfo.AccountId : -1L;
			GameResult gameResult = GameResult.GameServerNetworkErrorToClient;
			string empty = string.Empty;
			serverPlayerState.LogGameExit(gameResult);
			GameFlow gameFlow = GameFlow.Get();
			if (!serverPlayerState.IsAIControlled && !isSpectator && !isGameOver)
			{
				Log.Info("Player {0} [{1}] (connectionId {2}, connectionAddress {3}) has disconnected from game {4} (listenAddress {5}), and will now be controlled by a bot | closeStatusCode= {6}", serverPlayerState.PlayerInfo.LobbyPlayerInfo.Handle, serverPlayerState.SessionInfo.AccountId, serverPlayerState.ConnectionId, serverPlayerState.ConnectionAddress, serverPlayerState.GameInfo.Name, ListenAddress, empty);
				SendConsoleMessageWithHandle("PlayerDisconnectedBot", "Disconnect", serverPlayerState.PlayerInfo.LobbyPlayerInfo.Handle, serverPlayerState.PlayerInfo.TeamId);
				if (gameFlow != null)
				{
					gameFlow.ReplaceWithBots(serverPlayerState.ConnectionPersistent);
				}
				serverPlayerState.OnReplaceWithBots();
			}
			else
			{
				Log.Info("Player {0} [{1}] (connectionId {2}, connectionAddress {3}) has disconnected from game {4} (listenAddress {5}) | closeStatusCode= {6}", serverPlayerState.PlayerInfo.LobbyPlayerInfo.Handle, serverPlayerState.SessionInfo.AccountId, serverPlayerState.ConnectionId, serverPlayerState.ConnectionAddress, serverPlayerState.GameInfo.Name, ListenAddress, empty);
				SendConsoleMessageWithHandle("PlayerDisconnected", "Disconnect", serverPlayerState.PlayerInfo.LobbyPlayerInfo.Handle, serverPlayerState.PlayerInfo.TeamId);
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
		if (m_monitorGameServerInterface != null && gameStatus >= GameStatus.LoadoutSelecting)
		{
			if (gameStatus == GameStatus.Stopped
				&& !m_sentGameSummary
				&& GameManager.Get() != null
				&& GameManager.Get().GameSummary != null)
			{
				GameManager.Get().GameSummary.TimeText = MatchLogger.Get() != null ? MatchLogger.Get().GetTimeForLogging(true) : "0";
				GameManager.Get().GameSummary.MatchTime = MatchLogger.Get() != null ? MatchLogger.Get().GetMatchTime() : new TimeSpan(0L);
				GameManager.Get().GameSummary.NumOfTurns = GameFlowData.Get() != null ? GameFlowData.Get().CurrentTurn : 0;
				SendGameSummaryNotification();
			}
			m_monitorGameServerInterface.SendGameStatusNotification(gameStatus);
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

	private void HandleConnectedToMonitorServer(RegisterGameServerResponse response)
	{
		if (!response.Success)
		{
			DisconnectFromMonitorServer();
			// OnConnectedToMonitorServer(response);
			// GameManager.Get().StopGame();
			// return;
		}

		OnConnectedToMonitorServer(response);
	}

	private void HandleDisconnectedFromMonitorServer(string lastLobbyErrorMessage)
	{
		Log.Info("Disconnected from Monitor Server");
		m_monitorGameServerInterface = null;
		GameResult gameResult = GameResult.GameServerNetworkErrorToMonitorServer;
		OnDisconnectedFromMonitorServer(lastLobbyErrorMessage);
		GameManager.Get().StopGame(gameResult);
	}

	private void SetGameStatus(GameStatus gameStatus, bool notify = true)
	{
		GameManager.Get().SetGameStatus(gameStatus, GameResult.NoResult, notify);
	}

	// was private, public for for quick Artemis hook
	public void HandleLaunchGameRequest(LaunchGameRequest request) // async in rogues
	{
		// TODO HACK
		// var panels = FindObjectsOfType<UILoadingScreenPanel>();
		// GameObject obj;
		// UILoadingScreenPanel uILoadingScreenPanel;
		// if (panels.IsNullOrEmpty())
		// {
		// 	obj = new GameObject("UILoadingScreenPanel");
		// 	uILoadingScreenPanel = obj.AddComponent<UILoadingScreenPanel>();
		// }
		// else
		// {
		// 	uILoadingScreenPanel = panels[0];
		// 	obj = uILoadingScreenPanel.gameObject;
		// }
		// RectTransform rectTransform = obj.AddComponent<RectTransform>();
		// rectTransform.offsetMax = new Vector2(500, 500);
		// uILoadingScreenPanel.m_container = rectTransform;


		// TODO LOW pass config from lobby server
		// custom
		MatchmakingQueueConfig config = new MatchmakingQueueConfig();

		GameManager gameManager = GameManager.Get();
		gameManager.SetGameInfo(request.GameInfo);
		gameManager.SetTeamInfo(LobbyTeamInfo.FromServer(request.TeamInfo, GameBalanceVars.Get().MaxPlayerLevel, config)); // no config in rogues
		Log.Info("set LobbyTeamInfo & LobbyGameInfo in GameManager");
		if (request.GameplayOverrides != null)
		{
			gameManager.SetGameplayOverrides(request.GameplayOverrides);
		}
		StartServerOrHost(); // await StartServerOrHost(); in rogues
		m_serverPlayerStates.Clear();
		List<LobbyServerPlayerInfo> localPlayers = (from p in request.TeamInfo.TeamPlayerInfo where !p.IsNPCBot && !p.IsRemoteControlled select p).ToList();
		List<LobbyServerPlayerInfo> bots = (from p in request.TeamInfo.TeamPlayerInfo where p.IsNPCBot select p).ToList();
		Log.Info($"ServerGameManager::HandleLaunchGameRequest: {localPlayers.Count} local players and {bots.Count} bots");
		int num = -1;
		foreach (LobbyServerPlayerInfo lobbyServerPlayerInfo in localPlayers)
		{
			LobbySessionInfo sessionInfo = request.SessionInfo[lobbyServerPlayerInfo.PlayerId];
			List<LobbyServerPlayerInfo> proxyPlayerInfos = new List<LobbyServerPlayerInfo>();
			foreach (int proxyPlayerId in lobbyServerPlayerInfo.ProxyPlayerIds)
			{
				LobbyServerPlayerInfo proxyPlayerInfo = (from p in request.TeamInfo.TeamPlayerInfo where p.PlayerId == proxyPlayerId select p).FirstOrDefault();
				if (proxyPlayerInfo != null)
				{
					proxyPlayerInfo.CharacterInfo.CharacterCards = lobbyServerPlayerInfo.CharacterInfo.CharacterCards;
					proxyPlayerInfos.Add(proxyPlayerInfo);
				}
			}
			AddPlayerState(sessionInfo, lobbyServerPlayerInfo, proxyPlayerInfos, num--);
			Log.Info($"ServerGameManager::HandleLaunchGameRequest: Added local player {lobbyServerPlayerInfo.Handle} {lobbyServerPlayerInfo.CharacterType}");
		}
		foreach (LobbyServerPlayerInfo primaryPlayerInfo in bots)
		{
			AddPlayerState(null, primaryPlayerInfo, new List<LobbyServerPlayerInfo>(), -999);
			Log.Info($"ServerGameManager::HandleLaunchGameRequest: Added bot {primaryPlayerInfo.Handle} {primaryPlayerInfo.CharacterType}");
		}

		// custom
		AddReplayGeneratorSpectator();
		
		gameManager.SetGameSummary(new LobbyGameSummary());
		m_sentGameSummary = false;
		CommonServerConfig commonServerConfig = CommonServerConfig.Get();
		gameManager.GameInfo.GameServerAddress = $"{ListenAddress}:{ListenPort}";
		gameManager.GameInfo.GameServerHost = commonServerConfig.PrivateHostName;
		m_monitorGameServerInterface.SendLaunchGameResponse(true, gameManager.GameInfo);
		SetGameStatus(GameStatus.Launched);
		SetGameStatus(GameStatus.Connecting);
		LoadAssets();
	}

	// custom
	public static int GetReplayRecorderAccountId(Team team)
	{
		return -2 - (int)team;
	}

	// custom
	public static Team GetReplayRecorderTeam(long id)
	{
		if (id > 0 || id < int.MinValue) return Team.Invalid;
		int value = -2 - (int)id;
		return Enum.IsDefined(typeof(Team), value) ? (Team)value : Team.Invalid;
	}

	// custom
	private void AddReplayGeneratorSpectator()
	{
		foreach (Team team in new[] { Team.Spectator, Team.TeamA, Team.TeamB })
		{
			int playerId = GetReplayRecorderAccountId(team);
			string suffix = team == Team.Spectator ? string.Empty : $"_{team}";
			string handle = "replay_generator" + suffix;

			LobbyServerPlayerInfo lobbyServerPlayerInfo = new LobbyServerPlayerInfo()
			{
				AccountId = playerId,
				PlayerId = playerId,
				TeamId = Team.Spectator,
				Handle = handle,
				IsLoadTestBot = true,
				IsReplayGenerator = true
			};
			AddPlayerState(
				new LobbySessionInfo
				{
					AccountId = playerId
				},
				lobbyServerPlayerInfo,
				new List<LobbyServerPlayerInfo>(),
				-1);
			ServerPlayerState serverPlayerState = m_serverPlayerStates[playerId];
			GameManager.Get().TeamInfo.TeamPlayerInfo.Add(LobbyPlayerInfo.FromServer(lobbyServerPlayerInfo, 0, new MatchmakingQueueConfig()));
			m_replayRecorders[team] = new ReplayRecorder(serverPlayerState, suffix);
		}
	}

	private void AddPlayerState(
		LobbySessionInfo sessionInfo,
		LobbyServerPlayerInfo primaryPlayerInfo,
		List<LobbyServerPlayerInfo> proxyPlayerInfos,
		int connectionTemporaryId)
	{
		GameManager gameManager = GameManager.Get();
		ServerPlayerState serverPlayerState = new ServerPlayerState
		{
			PlayerInfo = new ServerPlayerInfo(primaryPlayerInfo, proxyPlayerInfos),
			SessionInfo = sessionInfo,
			ConnectionPersistent = null,
			ConnectionReady = false,
			GameLoadingState = new GameLoadingState(),
		};
		m_serverPlayerStates.Add(primaryPlayerInfo.PlayerId, serverPlayerState);
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

		if (!serverPlayerState.IsAIControlled
		    || primaryPlayerInfo.ReplacedWithBots
		    || serverPlayerState.IsLoadTestBot)
		{
			if (serverPlayerState.IsAIControlled && primaryPlayerInfo.ReplacedWithBots)
			{
				Log.Info($"Player {serverPlayerState.PlayerInfo.Handle}[{serverPlayerState.SessionInfo.AccountId}] " +
				         $"Is AI Controlled and replaced with bots when adding player state");
			}
			string host = "localhost";

			// custom
			string address = serverPlayerState.SessionInfo.ConnectionAddress;
			// rogues
			//string text2 = (!serverPlayerState.SessionInfo.ExternalConnectionAddress.IsNullOrEmpty()) ? serverPlayerState.SessionInfo.ExternalConnectionAddress : serverPlayerState.SessionInfo.ConnectionAddress;

			if (serverPlayerState != null && serverPlayerState.SessionInfo != null && !address.IsNullOrEmpty())
			{
				Debug.Log("Expecting connection from client at address " + address);
				string[] addressParts = address.Split(':', '/');
				if (!addressParts.IsNullOrEmpty())
				{
					host = addressParts.Length > 1 ? addressParts[addressParts.Length - 2] : addressParts[0];
				}
			}

			// TODO probably will have to set serverPlayerState.ConnectionPersistent somewhere else
			// Though null is fine, it will be set when client actually connects.
			//serverPlayerState.ConnectionPersistent = new NetworkConnection(); // new NetworkConnection(host, connectionTemporaryId); in rogues
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
				Log.Info("Player {0}[{1}] is disconnected, and will now be controlled by a bot", serverPlayerState.PlayerInfo.Handle, serverPlayerState.SessionInfo.AccountId);
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
		Log.Info($"[FROMLOBBY] {request.GetType()} {DefaultJsonSerializer.Serialize(request)}");
		bool flag = true;
		string responseText = null;
		try
		{
			List<LobbyServerPlayerInfo> proxyPlayerInfos = new List<LobbyServerPlayerInfo>();
			if (m_serverPlayerStates.ContainsKey(request.PlayerInfo.PlayerId))
			{
				Log.Error("Player {0} trying to join game server, but ServerPlayerState already exists for this player", request.PlayerInfo.Handle, request.PlayerInfo.AccountId);
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
			Log.Error("Player {0} trying to join game server, but exception occurred", request.PlayerInfo.Handle, request.PlayerInfo.AccountId);
			responseText = "Server error";
			flag = false;
		}

		m_monitorGameServerInterface.SendJoinGameServerResponse(request.RequestId, flag, responseText, request.OrigRequestId, request.PlayerInfo, request.GameServerProcessCode);
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
			Log.Error("Player {0} trying to join game server as observer, but exception occurred", request.AccountId);
			responseText = "Server error";
			success = false;
		}

		m_monitorGameServerInterface.SendJoinGameAsObserverResponse(request.RequestId, success, responseText, gameplayOverrides, gameInfo, teamInfo, playerInfo);
	}

	private void CheckConnected()
	{
		if (!IsServer())
		{
			return;
		}
		CommonServerConfig commonServerConfig = CommonServerConfig.Get();
		GameManager gameManager = GameManager.Get();
		if (gameManager.GameStatus != GameStatus.Connecting
		    && gameManager.GameStatus != GameStatus.Loading)
		{
			return;
		}
		bool isWaitingForPlayers = false;
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			if (serverPlayerState.PlayerInfo != null
			    && serverPlayerState.SessionInfo != null
			    && (serverPlayerState.ConnectionPersistent == null || serverPlayerState.ConnectionPersistent.connectionId < 0))
			{
				isWaitingForPlayers = true;
				break;
			}
		}
		bool isTimeOut = Time.unscaledTime - gameManager.GameStatusTime >= commonServerConfig.GameServerClientConnectTimeout.TotalSeconds;
		if (isWaitingForPlayers && isTimeOut)
		{
			foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
			{
				if (serverPlayerState.PlayerInfo != null
				    && serverPlayerState.SessionInfo != null
				    && (serverPlayerState.ConnectionPersistent == null || !serverPlayerState.ConnectionPersistent.isReady))
				{
					Log.Info($"Player {serverPlayerState.PlayerInfo.Handle}[{serverPlayerState.SessionInfo.AccountId}] " +
					         $"has failed to connect to game {gameManager.GameInfo.Name}, and will now be controlled by a bot " +
					         $"| {ListenAddress}:{ListenPort}");
					SendConsoleMessageWithHandle("PlayerFailedToConnect", "Disconnect", serverPlayerState.PlayerInfo.Handle);
					serverPlayerState.DisconnectAndReplaceWithBots(GameResult.ClientConnectionFailedToGameServer);
				}
			}
			isWaitingForPlayers = false;
		}

		if (!isWaitingForPlayers && gameManager.GameStatus == GameStatus.Connecting)
		{
			SetGameStatus(GameStatus.Connected);
			SetGameStatus(GameStatus.Authenticated);
			SetGameStatus(GameStatus.Loading);
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
						Log.Warning("Reconnecting client has disconnected {0}", serverPlayerState);
						m_reconnectingAccountIds.Remove(serverPlayerState.SessionInfo.AccountId);
					}
					else if (flag)
					{
						Log.Warning("Reconnecting client has connected on game ending {0}", serverPlayerState);
						m_reconnectingAccountIds.Remove(serverPlayerState.SessionInfo.AccountId);
						GameManager.EndGameNotification endGameNotification = new GameManager.EndGameNotification();

						// custom
						connectionPersistent.Send((short)MyMsgType.EndGameNotification, endGameNotification);
						// rogues
						//NetworkServer.SendToClient<GameManager.EndGameNotification>(connectionPersistent.connectionId, endGameNotification);
					}
				}
			}
		}
	}

	// custom
	private static NetworkMessageDelegate Wrap<T>(Action<NetworkConnection, T> handler) where T : MessageBase, new()
	{
		return msg => handler(msg.conn, msg.ReadMessage<T>());
	}

	private void HandleLoginRequest(NetworkConnection conn, GameManager.LoginRequest request)
	{
		if (!IsServer() || request == null)
		{
			return;
		}
		long accountId = Convert.ToInt64(request.AccountId);
		GameManager.LoginResponse loginResponse = new GameManager.LoginResponse();
		if (!m_serverPlayerStates.TryGetValue(request.PlayerId, out ServerPlayerState serverPlayerState)
			|| serverPlayerState.SessionInfo == null
			|| accountId != serverPlayerState.SessionInfo.AccountId)
		{
			loginResponse.Success = false;
			if (serverPlayerState != null && serverPlayerState.ReplacedWithBots)
			{
				Log.Error("Received login request from {0} after they were disconnected and replaced with a bot for timing out.", conn.address);
				loginResponse.ErrorMessage = "Login denied, your client timed out and was replaced with a bot.";
			}
			else
			{
				Log.Error("Received invalid login request from {0} | request= {1} {2} {3}", conn.address, request.AccountId, request.SessionToken, request.PlayerId);
				loginResponse.ErrorMessage = "Invalid login request";
			}

			// custom
			conn.Send((short)MyMsgType.LoginResponse, loginResponse);
			// rogues
			//NetworkServer.SendToClient<GameManager.LoginResponse>(conn.connectionId, loginResponse);
			return;
		}

		Log.Info("LoginRequest from {0} is using type {1} connection", serverPlayerState, conn.GetType().ToString());
		bool isReconnecting = false;
		if (Convert.ToInt64(request.SessionToken) != serverPlayerState.SessionInfo.SessionToken
			&& (serverPlayerState.SessionInfo.ReconnectSessionToken == 0L
				|| Convert.ToInt64(request.SessionToken) != serverPlayerState.SessionInfo.ReconnectSessionToken))
		{
			Log.Error("Received invalid reconnect request from {0} (connectionId {1}) {2} to game {3} | request= {4} {5} {6} | playerState= {7} {8}", conn.address, conn.connectionId, serverPlayerState.SessionInfo.Name, serverPlayerState.GameInfo.GameServerProcessCode, request.AccountId, request.SessionToken, request.PlayerId, serverPlayerState.SessionInfo.SessionToken, serverPlayerState.SessionInfo.ReconnectSessionToken);
			loginResponse.ErrorMessage = "Invalid reconnect request";

			// custom
			conn.Send((short)MyMsgType.LoginResponse, loginResponse);
			// rogues
			//NetworkServer.SendToClient<GameManager.LoginResponse>(conn.connectionId, loginResponse);
			return;
		}
		if (serverPlayerState.IsAIControlled && !serverPlayerState.IsLoadTestBot)
		{
			Log.Info("This is a reconnect request from {0} (connectionId {1}) to game {2}...  {3} | PlayerId {4}, LastReceivedMsgSeqNum {5}", conn.address, conn.connectionId, serverPlayerState.GameInfo.GameServerProcessCode, serverPlayerState.SessionInfo.Name, request.PlayerId, request.LastReceivedMsgSeqNum);
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
			Log.Error("LoginRequest from {0} is using type {1} connection, not ServerExternalConnection", serverPlayerState, conn.GetType().ToString());
			loginResponse.ErrorMessage = string.Format("{0} is not a ServerExternalConnection", conn.GetType());

			// custom
			conn.Send((short)MyMsgType.LoginResponse, loginResponse);
			// rogues
			//NetworkServer.SendToClient<GameManager.LoginResponse>(conn.connectionId, loginResponse);
			return;
		}
		if (connectionPersistent != null && connectionPersistent != conn)
		{
			Log.Info("Player {0} already has connection from {1} to game {2}. disconnecting | persistent=({3})", serverPlayerState, connectionPersistent.address, serverPlayerState.GameInfo.GameServerProcessCode, connectionPersistent.connectionId);
			MyNetworkManager.Get().OnServerDisconnect(connectionPersistent);
		}
		Log.Info("Player {0} has authenticated from {1} to game {2} | persistent=({3})", serverPlayerState, conn.address, serverPlayerState.GameInfo.GameServerProcessCode, conn.connectionId);
		serverPlayerState.ConnectionPersistent = conn;
		serverPlayerState.ConnectionId = conn.connectionId;
		serverPlayerState.ConnectionAddress = conn.address;
		loginResponse.Success = true;

		// custom
		conn.Send((short)MyMsgType.LoginResponse, loginResponse);
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
			Log.Error("Received invalid assets loading notification from {0} | conn=({1}), connPersistent=({2}), playerState={3}, accountId={4}", conn.address, conn, serverPlayerState != null ? serverPlayerState.ConnectionPersistent.ToString() : "(none)", serverPlayerState, notification.AccountId);
			return;
		}
		Log.Info("Player {0} has loaded all assets ({1:0.00} secs). {2}", serverPlayerState.SessionInfo.Name, Time.unscaledTime - GameManager.Get().GameStatusTime, GameManager.Get().GameInfo.Name);
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
		if (!m_serverPlayerStates.TryGetValue(notification.PlayerId, out serverPlayerState) || serverPlayerState.SessionInfo == null || conn != serverPlayerState.ConnectionPersistent)
		{
			Log.Error("Received invalid leave game notification from {0}", conn.address);
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
			Log.Info("Delaying spawn message for player {0} until the resolve phase is complete", playerState.SessionInfo.Name);
			m_playersToReadyNextDecisionPhase.Add(playerState);
			return;
		}
		
		// custom
		bool isGameLoaded = GameManager.Get() != null && GameManager.Get().GameStatus >= GameStatus.Loaded;

		// custom Artemis (ReconnectReplayStatus is not used in rogues at all
		if (!playerState.IsAIControlled)
		{
			Log.Info($"Entering reconnection replay state for {playerState}");
			playerState.ConnectionPersistent.Send((short)MyMsgType.ReconnectReplayStatus, new GameManager.ReconnectReplayStatus { WithinReconnectReplay = true });
		}
		// end custom Artemis

		int num = playerState.PlayerInfo.LobbyPlayerInfo.IsSpectator ? 2 : 1;
		int playerObjectCount = m_serverPlayerStates.Count * (1 + num);
		// TODO LOW check that the number is correct (7 iirc)
		int totalObjectCount = NetworkServer.objects.Count; //  int count = NetworkIdentity.spawned.Count;
		Log.Info("Sending spawn message for player {0} (playerObjectCount={1} totalObjectCount={2})", playerState.SessionInfo != null ? playerState.SessionInfo.Name : "(unnamed player)", playerObjectCount, totalObjectCount);
		GameManager.SpawningObjectsNotification spawningObjectsNotification = new GameManager.SpawningObjectsNotification();
		spawningObjectsNotification.PlayerId = playerState.PlayerInfo.PlayerId;
		spawningObjectsNotification.SpawnableObjectCount = totalObjectCount;

		// custom
		playerState.ConnectionPersistent.Send((short)MyMsgType.SpawningObjectsNotification, spawningObjectsNotification);
		// rogues
		//playerState.ConnectionPersistent.Send<GameManager.SpawningObjectsNotification>(spawningObjectsNotification, 0);
		
		// custom
		if (ENABLE_RECONNECT_REPLAY
			&& isGameLoaded
		    && !playerState.IsAIControlled
		    && m_replayRecorders.TryGetValue(playerState.PlayerInfo.TeamId, out ReplayRecorder replayRecorder))
		{
			List<Replay.Message> reconnectionData = replayRecorder.Replay.m_messages;
			Log.Info($"Sending {reconnectionData.Count} messages as reconnect replay");
			for (var i = 0; i < reconnectionData.Count; i++)
			{
				Replay.Message msg = reconnectionData[i];
				// WARN sending ReconnectReplayStatus this way throws the client into infinite recursion of entering/leaving reconnection replay phase
				// Just resending the messages seems to work fine too: playerState.ConnectionPersistent.SendBytes(msg.data, msg.data.Length, 0);
				playerState.ConnectionPersistent.Send((short)MyMsgType.ObserverMessage, new GameManager.ObserverMessage { Message = msg });
			}
		}

		// custom Artemis (ReconnectReplayStatus is not user in rogues at all
		if (!playerState.IsAIControlled)
		{
			Log.Info($"Exiting reconnection replay state for {playerState}");
			playerState.ConnectionPersistent.Send((short)MyMsgType.ReconnectReplayStatus, new GameManager.ReconnectReplayStatus { WithinReconnectReplay = false });
		}
		// end custom Artemis

		NetworkServer.SetClientReady(playerState.ConnectionPersistent);
		playerState.ConnectionReady = true;
		Log.Warning("Not calling SendReconnectData...");
		
		// custom
		// TODO HACK 
		if (isGameLoaded && GameFlowData.Get() != null && !playerState.IsAIControlled)
		{
			foreach (ActorData actorData in GameFlowData.Get().GetAllActorsForPlayer(playerState.PlayerInfo.PlayerId))
			{
				// fog of war fix
				Log.Info($"Teleporting reconnected {actorData}");
				actorData.TeleportToBoardSquare(
					actorData.GetCurrentBoardSquare(),
					actorData.transform.localRotation.eulerAngles,
					ActorData.TeleportType.Failsafe,
					null
				);
				// ability fix
				actorData.GetActorTurnSM().CallRpcTurnMessage((int)TurnMessage.TURN_START, 0);
				// authority fix
				if (!NetworkServer.ReplacePlayerForConnection(playerState.ConnectionPersistent, actorData.gameObject, 0))
				{
					Log.Error("Failed to replace reconnecting player as a fix");
				}
			}
		}
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
			Log.Error("Received invalid assets loading progress from {0} | conn=({1}), playerState={2}, accountId={3}", conn.address, conn, serverPlayerState, loadingProgressInfo.AccountId);
			return;
		}

		serverPlayerState.GameLoadingState.TotalLoadingProgress = loadingProgressInfo.TotalLoadingProgress;
		serverPlayerState.GameLoadingState.LevelLoadingProgress = loadingProgressInfo.LevelLoadingProgress;
		serverPlayerState.GameLoadingState.CharacterLoadingProgress = loadingProgressInfo.CharacterLoadingProgress;
		serverPlayerState.GameLoadingState.VfxLoadingProgress = loadingProgressInfo.VfxLoadingProgress;
		serverPlayerState.GameLoadingState.SpawningProgress = loadingProgressInfo.SpawningProgress;
		GameLoadingState gameLoadingState = serverPlayerState.GameLoadingState;
		gameLoadingState.LoadingProgressUpdateCount += 1;
		float loadingProgress = loadingProgressInfo.TotalLoadingProgress / 100f;
		UILoadingScreenPanel.Get()?.SetLoadingProgress(loadingProgressInfo.PlayerId, loadingProgress, false);  // no check in rogues
		foreach (ServerPlayerState serverPlayerState2 in m_serverPlayerStates.Values)
		{
			// custom
			if (serverPlayerState2.IsAIControlled) continue;
			// end custom
			
			if (serverPlayerState2.ConnectionPersistent != null && !serverPlayerState2.LocalClient)
			{
				// custom
				serverPlayerState2.ConnectionPersistent.Send((short)MyMsgType.ServerAssetsLoadingProgressUpdate, loadingProgressInfo);
				// rogues
				//NetworkServer.SendToClient<GameManager.AssetsLoadingProgress>(serverPlayerState2.ConnectionPersistent.connectionId, loadingProgressInfo);
			}
		}
	}

	public void ClientPreparedForGameStart(int playerId)
	{
		ServerPlayerState serverPlayerState;
		if (m_serverPlayerStates.TryGetValue(playerId, out serverPlayerState))
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
		if (!m_serverPlayerStates.TryGetValue(notification.PlayerId, out serverPlayerState) || serverPlayerState.SessionInfo == null || conn != serverPlayerState.ConnectionPersistent)
		{
			Log.Error("Received invalid PlayerObjectStartedOnClientNotification from {0}", conn.address);
			return;
		}

		serverPlayerState.GameLoadingState.IsReady = true;
		Log.Info("Player {0} ActorData started on client", serverPlayerState.SessionInfo.Name);
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
		if (!m_serverPlayerStates.TryGetValue(playerObjectStartedOnClientNotification.PlayerId, out serverPlayerState) || serverPlayerState.SessionInfo == null || msg.conn != serverPlayerState.ConnectionPersistent)
		{
			Log.Error("Received invalid PlayerObjectStartedOnClientNotification from {0}", msg.conn.address);
			return;
		}
		if (GameFlowData.Get() != null && GameFlowData.Get().CurrentTurn > 1)
		{
			float timeSinceMatchStart = MatchLogger.Get().GetTimeSinceMatchStart();
			GameFlow.Get().SendMatchTime(timeSinceMatchStart);
		}

		Log.Info("Player {0} requested time update", serverPlayerState.SessionInfo.Name);
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
		if (!m_serverPlayerStates.TryGetValue(request.PlayerId, out serverPlayerState) || serverPlayerState.SessionInfo == null || conn != serverPlayerState.ConnectionPersistent)
		{
			Log.Error("Received invalid FakeActionRequest from {0}", conn.address);
			return;
		}
		GameManager.FakeActionResponse fakeActionResponse = new GameManager.FakeActionResponse();
		fakeActionResponse.msgSize = Random.Range(32, 1500); // CryptoRandom.RangeInt32(32, 1500) in rogues
		fakeActionResponse.msgData = new byte[fakeActionResponse.msgSize];

		// custom
		conn.Send((short)MyMsgType.ServerFakeActionResponse, fakeActionResponse);
		// rogues
		//NetworkServer.SendToClient<GameManager.FakeActionResponse>(conn.connectionId, fakeActionResponse);
	}

	public bool ClientsPreparedForGameStart()
	{
		bool result = true;
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			//Log.Info($"ClientsPreparedForGameStart {serverPlayerState.PlayerInfo.Handle} connected:{serverPlayerState.ConnectionPersistent != null} bot:{serverPlayerState.PlayerInfo.IsAIControlled} ready:{serverPlayerState.GameLoadingState.IsReady}");
			if (serverPlayerState.ConnectionPersistent != null
			    && !serverPlayerState.PlayerInfo.IsAIControlled
			    && !serverPlayerState.GameLoadingState.IsReady)
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
				GameObject gameObject = Instantiate(prefabsForGameScenes[i]);
				DontDestroyOnLoad(gameObject);
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
		SetGameStatus(GameStatus.Loading);
		m_loadLevelOperation = new AssetBundleManager.LoadSceneAsyncOperation
		{
			sceneName = gameManager.GameConfig.Map,  //  gameManager.GameMission.Map in rogues
			bundleName = bundleName,
			loadSceneMode = 0
		};
		StartCoroutine(AssetBundleManager.Get().LoadSceneAsync(m_loadLevelOperation));
		Log.Info($"ServerGameManager::LoadAssets: Loading {m_loadLevelOperation.bundleName}/{m_loadLevelOperation.sceneName}");
		m_loadingCharacterResources.Clear();
		Log.Info($"ServerGameManager::LoadAssets: Got {m_serverPlayerStates.Count} player states");
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			Log.Info($"ServerGameManager::LoadAssets: Processing {serverPlayerState.PlayerInfo.Handle}");
			if (serverPlayerState.PlayerInfo.TeamId == Team.TeamA || serverPlayerState.PlayerInfo.TeamId == Team.TeamB)
			{
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(serverPlayerState.PlayerInfo.CharacterType);
				m_loadingCharacterResources.Add(characterResourceLink);
				ServerPlayerInfo closureServerPlayerInfo = serverPlayerState.PlayerInfo;
				characterResourceLink.LoadAsync(serverPlayerState.PlayerInfo.CharacterSkin, delegate(LoadedCharacterSelection loadedCharacter)
				{
					Log.Info($"ServerGameManager::LoadAsync: Done loading {loadedCharacter.resourceLink.m_displayName}");
					m_loadingCharacterResources.Remove(loadedCharacter.resourceLink);
					m_loadedCharacterResourceCount++;
					closureServerPlayerInfo.SelectedCharacter = loadedCharacter;
				});
				Log.Info($"ServerGameManager::LoadAssets: Loading {serverPlayerState.PlayerInfo.CharacterType}");
				foreach (ServerPlayerInfo serverPlayerInfo in closureServerPlayerInfo.ProxyPlayerInfos)
				{
					CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(serverPlayerInfo.CharacterType);
					m_loadingCharacterResources.Add(characterResourceLink2);
					ServerPlayerInfo closureProxyPlayerInfo = serverPlayerInfo;
					characterResourceLink2.LoadAsync(serverPlayerInfo.CharacterSkin, delegate(LoadedCharacterSelection loadedCharacter)
					{
						Log.Info($"ServerGameManager::LoadAsync: Done loading {loadedCharacter.resourceLink.m_displayName}");
						m_loadingCharacterResources.Remove(loadedCharacter.resourceLink);
						m_loadedCharacterResourceCount++;
						closureProxyPlayerInfo.SelectedCharacter = loadedCharacter;
					});
					Log.Info($"ServerGameManager::LoadAssets: Loading proxy {serverPlayerInfo.CharacterType}");
				}
			}
			Log.Info($"ServerGameManager::LoadAssets: Finished processing {serverPlayerState.PlayerInfo.Handle}");
		}
	}

	public void LoadCharacterResourceLink(CharacterResourceLink characterResourceLink, CharacterVisualInfo linkVisualInfo) // linkVisualInfo = null in rogues
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
				m_assetsLoadingState.LevelLoadProgress = m_loadLevelOperationDone ? 1f : m_loadLevelOperation != null ? m_loadLevelOperation.progress : 0f;
				m_assetsLoadingState.CharacterLoadProgress = Mathf.Clamp(m_loadedCharacterResourceCount / (float)gameManager.TeamInfo.TotalPlayerCount, 0f, 1f);
				m_assetsLoadingState.VfxPreloadProgress = ClientVFXLoader.Get() != null ? ClientVFXLoader.Get().Progress : 0f;
				if (ClientScene.spawnableObjects != null)
				{
					int count = ClientScene.spawnableObjects.Count;
					int num = (from s in ClientScene.spawnableObjects
						where s.Value.gameObject.activeSelf
						select s).Count();
					m_assetsLoadingState.SpawningProgress = count > 0 ? Mathf.Clamp(num / (float)count, 0f, 1f) : 1f;
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
						// custom
						serverPlayerState.ConnectionPersistent.Send((short)MyMsgType.ServerAssetsLoadingProgressUpdate, assetsLoadingProgress);
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
			UpdateLoadProgress();
		}
		if (gameManager.GameStatus == GameStatus.Loading)
		{
			if (m_loadLevelOperation != null && m_loadLevelOperation.isDone)
			{
				m_loadLevelOperation = null;
				m_loadLevelOperationDone = true;
				VisualsLoader visualsLoader = VisualsLoader.Get();
				if (visualsLoader != null && visualsLoader.LevelLoaded()) // (visualsLoader != null && visualsLoader.EncounterLoaded()) in rogues, probably can be removed
				{
					NetworkServer.SpawnObjects();
				}
				else // custom
				{
					Log.Error("Visuals loader failed");
				}

				// custom artemis
				NetworkServer.SpawnObjects();

				//List<LobbyPlayerInfo> playerInfoList = GameManager.Get().TeamInfo.TeamPlayerInfo;
				//foreach(LobbyPlayerInfo playerInfo in playerInfoList)
				//{
				//	AddCharacterActor(playerInfo);
				//}
				// end custom artemis
			}
			bool flag2 = true;
			if (NPCCoordinator.Get() != null)
			{
				flag2 = NPCCoordinator.Get().LoadingState == NPCCoordinator.LoadingStateEnum.Done;
			}
			if (m_loadLevelOperation == null
			    && m_loadingCharacterResources.Count == 0
			    && (VisualsLoader.Get() == null || VisualsLoader.Get().LevelLoaded())
			    && flag2)
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

						SendConsoleMessageWithHandle("PlayerFailedToLoad", "Disconnect", sps.PlayerInfo.Handle, sps.PlayerInfo.TeamId);
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
				SetGameStatus(GameStatus.Loaded);
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
			// rogues
			// if (serverPlayerState.PlayerInfo.IsGameOwner && NetworkClient.active)
			// {
			// 	ClientScene.AddPlayer(serverPlayerState.ConnectionPersistent, 0); // ClientScene.AddPlayer(serverPlayerState.ConnectionPersistent); in rogues
			// }
			GameFlow.Get().AddPlayer(serverPlayerState, serverPlayerState.SessionInfo.AccountId < 0);  // custom replay generator flag
		}

		// custom artemis
		// seems to be crucial for this to happen before spawning players
		SpawnObject("ApplicationSingletonsNetId");
		SpawnObject("GameSceneSingletons");

		//SharedActionBuffer.Get().Networkm_actionPhase = ActionBufferPhase.Done;
		// end custom artemis

		GameFlowData.Get().gameState = GameState.SpawningPlayers;
		SetGameStatus(GameStatus.Started);
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
			if (ObjectivePoints.Get().Networkm_matchState == ObjectivePoints.MatchState.InMatch)
			{
				ObjectivePoints.Get().EndGame();
			}
			GameManager.Get().StopGame();
		}
	}

	public bool AreClientsConnected()
	{
		foreach (ServerPlayerState serverPlayerState in m_serverPlayerStates.Values)
		{
			if ((serverPlayerState.ConnectionPersistent != null
			     && serverPlayerState.ConnectionPersistent.connectionId > 0
			     // TODO HACK
			     // custom
			     && serverPlayerState.PlayerInfo.LobbyPlayerInfo.AccountId > 0
			     && !(GameManager.Get().GameStatus >= GameStatus.Loaded && serverPlayerState.GameResult > GameResult.Requeued))
			     // end custom
			    || serverPlayerState.LocalClient)
			{
				return true;
			}
		}
		return false;
	}

	private void HandleDisconnectPlayerRequest(DisconnectPlayerRequest request)
	{
		ServerPlayerState serverPlayerState = null;
		m_serverPlayerStates.TryGetValue(request.PlayerInfo.PlayerId, out serverPlayerState);
		if (serverPlayerState == null)
		{
			return;
		}
		if (serverPlayerState.SessionInfo == null
		    || serverPlayerState.SessionInfo.AccountId != request.SessionInfo.AccountId
		    || serverPlayerState.SessionInfo.SessionToken != request.SessionInfo.SessionToken
		    || serverPlayerState.ConnectionPersistent == null)
		{
			return;
		}
		serverPlayerState.LogGameExit(request.GameResult);
		Log.Info("Disconnecting player {0} {1} (connectionId {2})", request.SessionInfo.Handle, request.SessionInfo.AccountId, serverPlayerState.ConnectionPersistent.connectionId);
		
		// rogues
		// serverPlayerState.ConnectionPersistent.Disconnect();
		// custom
		DisconnectNow(serverPlayerState.ConnectionPersistent);
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
			Log.Info("Lobby gave us a new session token for player {0} {1}", playerStateByAccountId.SessionInfo.Name, playerStateByAccountId.SessionInfo.ReconnectSessionToken);
		}
		else
		{
			reconnectPlayerResponse.ErrorMessage = string.Format("Lobby gave use a new session token for account id {0} but we don't have that account as a player!", request.AccountId);
			reconnectPlayerResponse.Success = false;
			Log.Warning(reconnectPlayerResponse.ErrorMessage);
		}
		m_monitorGameServerInterface.Send(reconnectPlayerResponse);
	}

	private void HandleShutdownGameRequest(ShutdownGameRequest request)
	{
		Log.Info("Received shutdown game request");
		if (ObjectivePoints.Get()?.Networkm_matchState == ObjectivePoints.MatchState.InMatch)
		{
			ObjectivePoints.Get().EndGame();
		}
		GameManager.Get()?.StopGame();
		
		// custom
		Log.Info("Shutting down");
		Application.Quit();
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
		private set { }
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
		private set { }
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
		private set { }
	}

	private void HandleMonitorHeartbeatResponse(MonitorHeartbeatResponse response)
	{
	}

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