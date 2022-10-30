using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using WebSocketSharp;

namespace ArtemisServer.BridgeServer
{
    public class ArtemisBridgeServerInterface : ArtemisClientBase
    {
        private LobbyGameInfo m_gameInfo;
        private LobbyTeamInfo m_teamInfo; // TODO use ServerTeamInfo
        private LobbySessionInfo m_sessionInfo;

        protected bool m_registered;
        private int ConnectionTimeout;
        public event Action<RegisterGameServerResponse> OnConnectedHandler = delegate { };
        public event Action<string> OnDisconnectedHandler = delegate { };
        public event Action<LaunchGameRequest> OnLaunchGameRequest = delegate { };
        public event Action<JoinGameServerRequest> OnJoinGameServerRequest = delegate { };
        public event Action<JoinGameAsObserverRequest> OnJoinGameAsObserverRequest = delegate { };
        public event Action<ShutdownGameRequest> OnShutdownGameRequest = delegate { };
        public event Action<DisconnectPlayerRequest> OnDisconnectPlayerRequest = delegate { };
        public event Action<ReconnectPlayerRequest> OnReconnectPlayerRequest = delegate { };
        public event Action<MonitorHeartbeatResponse> OnMonitorHeartbeatResponse = delegate { };

        private LaunchGameRequest pendingLaunchGameRequest = null;
        
        public static readonly List<Type> BridgeMessageTypes = new List<Type>
        {
            typeof(RegisterGameServerRequest),
            typeof(RegisterGameServerResponse),
            typeof(LaunchGameRequest),
            typeof(JoinGameServerRequest),
            typeof(JoinGameAsObserverRequest),
            typeof(ShutdownGameRequest),
            typeof(DisconnectPlayerRequest),
            typeof(ReconnectPlayerRequest),
            typeof(MonitorHeartbeatResponse),
            typeof(ServerGameSummaryNotification),
            typeof(PlayerDisconnectedNotification),
            typeof(ServerGameMetricsNotification),
            typeof(ServerGameStatusNotification),
            typeof(MonitorHeartbeatNotification),
            typeof(LaunchGameResponse),
            typeof(JoinGameServerResponse),
            typeof(JoinGameAsObserverResponse)
        };

        protected override List<Type> GetMessageTypes()
        {
            return BridgeMessageTypes;
        }

        private ArtemisBridgeServerInterface()
        {
            // rogues
            // this.AutoStart = false;
            // this.AutoReconnect = false;
            m_registered = false;
            m_sessionInfo = new LobbySessionInfo();
            ConnectionTimeout = 30;
            m_overallConnectionTimer = new Stopwatch();
            m_reconnectDelayTimer = new Stopwatch();
            // Log = Log.LogInstance;
        }

        public void Initialize(string lobbyServerAddress, short port, ProcessType processType, string processCode, long accountId = 0L)
        {
            networkAddress = lobbyServerAddress;
            m_sessionInfo.BuildVersion = BuildVersion.ShortVersionString;
            m_sessionInfo.ProtocolVersion = "";
            m_sessionInfo.ProcessType = processType;
            m_sessionInfo.ProcessCode = processCode;
            m_sessionInfo.AccountId = accountId;

            RegisterMessageDelegate<LaunchGameRequest>(HandleLaunchGameRequest);
            RegisterMessageDelegate<JoinGameServerRequest>(HandleJoinGameServerRequest);
            RegisterMessageDelegate<JoinGameAsObserverRequest>(HandleJoinGameAsObserverRequest);
            RegisterMessageDelegate<ShutdownGameRequest>(HandleShutdownGameRequest);
            RegisterMessageDelegate<DisconnectPlayerRequest>(HandleDisconnectPlayerRequest);
            RegisterMessageDelegate<ReconnectPlayerRequest>(HandleReconnectPlayerRequest);
            RegisterMessageDelegate<MonitorHeartbeatResponse>(HandleMonitorHeartbeatResponse);
            
            // custom
            Log.Info($"ArtemisBridgeServerInterface initialized for {processType} {lobbyServerAddress} - {processCode}");
            m_sessionInfo.ConnectionAddress = HydrogenConfig.Get().PublicAddress + ":" + ServerGameManager.s_port;
        }

        protected override void OnConnecting()
        {
            UIFrontendLoadingScreen.Get().StartDisplayError("trying to connect to bridge server ", "at address: " + networkAddress);
        }

        protected override void OnConnected()
        {
            Log.Info("Successfully connected to lobby's bridge server");
            UIFrontendLoadingScreen.Get().StartDisplayError("connected to bridge server");
            
            // StartInsight();
            m_registered = false;
            RegisterGameServer();
        }

        protected override void OnDisconnected()
        {
            Log.Info("ArtemisBridgeServerInterface::OnDisconnected");
            UIFrontendLoadingScreen.Get().StartDisplayError("Disconnected");
            
            // if (m_registered)
            // {
            //     Log.Info($"Disconnected from {networkAddress}");
            //     UIFrontendLoadingScreen.Get().StartDisplayError("Disconnected");
            //     OnDisconnectedHandler("");
            //     return;
            // }
            // if (m_overallConnectionTimer.IsRunning)
            // {
            //     if (m_overallConnectionTimer.Elapsed.TotalSeconds < ConnectionTimeout)
            //     {
            //         Log.Info($"Retrying connection to {networkAddress}");
            //         UIFrontendLoadingScreen.Get().StartDisplayError("Retrying connection");
            //         Reconnect();
            //         return;
            //     }
            //     Log.Info($"Failed to connect to {networkAddress}");
            //     UIFrontendLoadingScreen.Get().StartDisplayError("Failed to connect");
            //     m_overallConnectionTimer.Reset();
            //     OnConnectedHandler(new RegisterGameServerResponse
            //     {
            //         Success = false,
            //         ErrorMessage = "connection failure"
            //     });
            // }
        }
        
        protected override void OnError(ErrorEventArgs e)
        {
            UIFrontendLoadingScreen.Get().StartDisplayError("network error", e.Message);
        }

        public void Update()
        {
            // TODO handle connection loss?

            // custom
            if (pendingLaunchGameRequest != null)
            {
                OnLaunchGameRequest(pendingLaunchGameRequest);
                pendingLaunchGameRequest = null;
            }
            // end custom
        }

        public void StartGame(string game)
        {
            StartGame(JsonConvert.DeserializeObject<ServerGame>(game));
        }

        private void StartGame(ServerGame game)
        {
            m_gameInfo = game.gameInfo;
            m_teamInfo = game.teamInfo;
            OnLaunchGameRequest(new LaunchGameRequest
            {
                GameInfo = m_gameInfo,
                TeamInfo = new LobbyServerTeamInfo
                {
                    TeamPlayerInfo = m_teamInfo.TeamPlayerInfo.Select(consLobbyServerPlayerInfo).ToList()
                },
                SessionInfo = m_teamInfo.TeamPlayerInfo.ToDictionary(x => x.PlayerId, consLobbySessionInfo),
                GameplayOverrides = new LobbyGameplayOverrides()
            });
        }

        private void RegisterGameServer()
        {
            Log.Info($"Registering game server {m_sessionInfo.ProcessCode}");
            RegisterGameServerRequest registerGameServerRequest = new RegisterGameServerRequest
            {
                SessionInfo = m_sessionInfo,
                isPrivate = false
            };
            CallbackHandler callback = delegate(CallbackStatus status, AllianceMessageBase msg)
            {
                HandleRegisterGameServerResponse((RegisterGameServerResponse)msg);
            };
            Send(registerGameServerRequest, callback);
        }

        private void HandleRegisterGameServerResponse(RegisterGameServerResponse response)
        {
            if (!response.Success)
            {
                Log.Error($"Failed to register game server with monitor server: {response.ErrorMessage}");
                UIFrontendLoadingScreen.Get().StartDisplayError("connected to bridge server", "failed to register");
                m_registered = false;
                OnConnectedHandler(response);
                Disconnect();
            }
            else
            {
                Log.Info("Registered game server with monitor server");
                UIFrontendLoadingScreen.Get().StartDisplayError("connected to bridge server", "registered as generic server");
                m_registered = true;
                m_overallConnectionTimer.Reset();
                OnConnectedHandler(response);
            }
        }

        private void HandleLaunchGameRequest(AllianceMessageBase msg)
        {
            pendingLaunchGameRequest = (LaunchGameRequest)msg; // cannot do it on this thread, handling it in Update
        }

        private void HandleJoinGameServerRequest(AllianceMessageBase msg)
        {
            OnJoinGameServerRequest((JoinGameServerRequest)msg);
        }

        private void HandleJoinGameAsObserverRequest(AllianceMessageBase msg)
        {
            OnJoinGameAsObserverRequest((JoinGameAsObserverRequest)msg);
        }

        private void HandleShutdownGameRequest(AllianceMessageBase msg)
        {
            OnShutdownGameRequest((ShutdownGameRequest)msg);
        }

        private void HandleDisconnectPlayerRequest(AllianceMessageBase msg)
        {
            OnDisconnectPlayerRequest((DisconnectPlayerRequest)msg);
        }

        private void HandleReconnectPlayerRequest(AllianceMessageBase msg)
        {
            OnReconnectPlayerRequest((ReconnectPlayerRequest)msg);
        }

        private void HandleMonitorHeartbeatResponse(AllianceMessageBase msg)
        {
            OnMonitorHeartbeatResponse((MonitorHeartbeatResponse)msg);
        }

        public void SendGameStatusNotification(GameStatus gameStatus)
        {
            Send(new ServerGameStatusNotification
            {
                GameStatus = gameStatus
            });
        }

        public void SendGameMetricsNotification(ServerGameMetrics gameMetrics)
        {
            Send(new ServerGameMetricsNotification
            {
                GameMetrics = gameMetrics
            });
        }

        public void SendGameSummaryNotification(LobbyGameSummary gameSummary, LobbyGameSummaryOverrides gameSummaryOverrides = null)
        {
            Send(new ServerGameSummaryNotification
            {
                GameSummary = gameSummary,
                GameSummaryOverrides = gameSummaryOverrides
            });
        }

        public void SendPlayerDisconnectedNotification(LobbySessionInfo sessionInfo, LobbyServerPlayerInfo playerInfo)
        {
            Send(new PlayerDisconnectedNotification
            {
                SessionInfo = sessionInfo,
                PlayerInfo = playerInfo
            });
        }

        public void SendMonitorHeartbeatNotification()
        {
            Send(new MonitorHeartbeatNotification());
        }

        public void SendLaunchGameResponse(bool success, LobbyGameInfo gameInfo)
        {
            Send(new LaunchGameResponse
            {
                Success = success,
                GameInfo = gameInfo
            });
        }

        public void SendJoinGameServerResponse(
            int responseId,
            bool success,
            string responseText,
            int origRequestId,
            LobbyServerPlayerInfo playerInfo,
            string gameServerProcessCode)
        {
            Send(new JoinGameServerResponse
            {
                ResponseId = responseId,
                ErrorMessage = responseText,
                Success = success,
                OrigRequestId = origRequestId,
                PlayerInfo = playerInfo,
                GameServerProcessCode = gameServerProcessCode
            });
        }

        public void SendJoinGameAsObserverResponse(
            int responseId,
            bool success,
            string responseText,
            LobbyGameplayOverrides gameplayOverrides,
            LobbyGameInfo gameInfo,
            LobbyTeamInfo teamInfo,
            LobbyPlayerInfo playerInfo)
        {
            Send(new JoinGameAsObserverResponse
            {
                ResponseId = responseId,
                ErrorMessage = responseText,
                Success = success,
                GameplayOverrides = gameplayOverrides,
                GameInfo = gameInfo,
                TeamInfo = teamInfo,
                PlayerInfo = playerInfo
            });
        }


        public static LobbySessionInfo consLobbySessionInfo(LobbyPlayerInfo playerInfo)
        {
            return new LobbySessionInfo
            {
                AccountId = playerInfo.AccountId,
                UserName = playerInfo.Handle,
                BuildVersion = "",
                ProtocolVersion = "",
                SessionToken = 0,
                ReconnectSessionToken = 0,
                ProcessCode = "",
                ProcessType = ProcessType.AtlasReactor,
                ConnectionAddress = "",
                Handle = playerInfo.Handle,
                IsBinary = false,
                FakeEntitlements = "",
                Region = Region.EU,
                LanguageCode = "en",
            };
        }

        public static LobbyServerPlayerInfo consLobbyServerPlayerInfo(LobbyPlayerInfo playerInfo)
        {
            var result = new LobbyServerPlayerInfo
            {
                AccountId = playerInfo.AccountId,
                PlayerId = playerInfo.PlayerId,
                Handle = playerInfo.Handle,
                CustomGameVisualSlot = playerInfo.CustomGameVisualSlot,
                TitleID = playerInfo.TitleID,
                TitleLevel = playerInfo.TitleLevel,
                BannerID = playerInfo.BannerID,
                EmblemID = playerInfo.EmblemID,
                RibbonID = playerInfo.RibbonID,
                IsGameOwner = playerInfo.IsGameOwner,
                //IsReplayGenerator = playerInfo.IsReplayGenerator,
                Difficulty = playerInfo.Difficulty,
                BotCanTaunt = playerInfo.BotCanTaunt,
                TeamId = playerInfo.TeamId,
                CharacterInfo = playerInfo.CharacterInfo,
                RemoteCharacterInfos = playerInfo.RemoteCharacterInfos,
                ReadyState = playerInfo.ReadyState,
                ControllingPlayerId = playerInfo.ControllingPlayerId,
                GameAccountType = playerInfo.IsAIControlled ? PlayerGameAccountType.None : PlayerGameAccountType.Human,
                //GameConnectionType = playerInfo.GameConnectionType,
                //GameOptionFlags = playerInfo.GameOptionFlags,
                AccountLevel = 100,
                TotalLevel = 100,
                NumWins = 1,
                AccMatchmakingElo = 1000,
                AccMatchmakingCount = 1000,
                CharMatchmakingElo = new Dictionary<CharacterType, float>(),
                CharMatchmakingCount = new Dictionary<CharacterType, int>(),
                UsedMatchmakingElo = 1000,
                RankedTier = 1,
                RankedPoints = 1000,
                MatchmakingEloKey = "foo",
                ProxyPlayerIds = new List<int>(),
                GroupIdAtStartOfMatch = -1,
                GroupSizeAtStartOfMatch = 1,
                GroupLeader = true,
                EffectiveClientAccessLevel = ClientAccessLevel.Full,
                RankedSortKarma = 100500
            };
            result.ControllingPlayerInfo = playerInfo.IsAIControlled ? null : result; // TODO ????
            return result;
        }

        class ServerGame
        {
            public LobbyGameInfo gameInfo;
            public LobbyTeamInfo teamInfo;
        }
    }
}