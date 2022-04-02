using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebSocketSharp;
using Newtonsoft.Json;
using UnityEngine;

namespace ArtemisServer.BridgeServer
{
    public class ArtemisBridgeServerInterface
    {
        private static ArtemisBridgeServerInterface Instance;
        private static WebSocketSharp.WebSocket ws;
        private static string BridgeServerAddress = "ws://127.0.0.1:6060/BridgeServer";

        private LobbyGameInfo m_gameInfo;
        private LobbyTeamInfo m_teamInfo; // TODO use ServerTeamInfo

        public enum BridgeMessageType
        {
            InitialConfig,
            SetLobbyGameInfo,
            SetTeamInfo,
            Start,
            Stop,
            GameStatusChange
        }

        private ArtemisBridgeServerInterface()
        {
            UIFrontendLoadingScreen.Get().StartDisplayError("trying to connect to bridge server ", "at address: "+BridgeServerAddress);
            ws = new WebSocketSharp.WebSocket(BridgeServerAddress);
            ws.OnMessage += Ws_OnMessage;
            ws.OnError += Ws_OnError;
            ws.OnOpen += Ws_OnOpen;
            ws.Connect();
        }

        private void Ws_OnOpen(object sender, EventArgs e)
        {
            Log.Info("Successfully connected to lobby's bridge server");
            UIFrontendLoadingScreen.Get().StartDisplayError("connected to bridge server");
            MemoryStream stream = new MemoryStream();
            stream.WriteByte((byte)BridgeMessageType.InitialConfig);
            string addressAndPort = ServerGameManager.s_address + ":" + ServerGameManager.s_port;
            
            stream.Write(GetByteArray(addressAndPort), 0, addressAndPort.Length);

            byte[] buffer = stream.ToArray();
            ws.Send(buffer);

            GameObject artemisServerObject = new GameObject("ArtemisServerComponent");
            ArtemisGamePoller gm = artemisServerObject.AddComponent<ArtemisGamePoller>();
            gm.Poll(this);
        }

        private void Ws_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            Log.Info("--- Websocket Error ---");
            Log.Info(e.Exception.Source);
            Log.Info(e.Message);
            Log.Info(e.Exception.StackTrace);
        }

        public static void Init()
        {
            Log.Info("Init ArtemisBridgeServerInterface");
            Instance = new ArtemisBridgeServerInterface();
        }

        public void StartGame(string game)
		{
            StartGame(JsonConvert.DeserializeObject<ServerGame>(game));
        }

        private void StartGame(ServerGame game)
		{
            m_gameInfo = game.gameInfo;
            m_teamInfo = game.teamInfo;
            ServerGameManager.Get().HandleLaunchGameRequest(new LaunchGameRequest()
            {
                GameInfo = m_gameInfo,
                TeamInfo = new LobbyServerTeamInfo()
                {
                    TeamPlayerInfo = m_teamInfo.TeamPlayerInfo.Select(consLobbyServerPlayerInfo).ToList()
                },
                SessionInfo = m_teamInfo.TeamPlayerInfo.ToDictionary(x => x.PlayerId, consLobbySessionInfo),
                GameplayOverrides = new LobbyGameplayOverrides()
            });
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            MemoryStream stream = new MemoryStream(e.RawData);
            BridgeMessageType messageType;
            string data;

            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                messageType = (BridgeMessageType)reader.Read();
                data = reader.ReadToEnd();
            }
            
            switch (messageType)
            {
                case BridgeMessageType.SetLobbyGameInfo:
                    m_gameInfo = JsonConvert.DeserializeObject<LobbyGameInfo>(data);
                    break;
                case BridgeMessageType.SetTeamInfo:
                    m_teamInfo = JsonConvert.DeserializeObject<LobbyTeamInfo>(data);
                    break;
                case BridgeMessageType.Start:
                    ServerGameManager.Get().HandleLaunchGameRequest(new LaunchGameRequest()
                    {
                        GameInfo = m_gameInfo,
                        TeamInfo = new LobbyServerTeamInfo()
                        {
                            TeamPlayerInfo = m_teamInfo.TeamPlayerInfo.Select(consLobbyServerPlayerInfo).ToList()
                        },
                        SessionInfo = m_teamInfo.TeamPlayerInfo.ToDictionary(x => x.PlayerId, consLobbySessionInfo),
                        GameplayOverrides = new LobbyGameplayOverrides()
                    });
                    break;
                default:
                    Log.Error("Received unhandled ws message type: " + messageType.ToString());
                    break;
            }
        }

        public static void ReportGameReady()
        {
            ws.Send(new byte[] { (byte)BridgeMessageType.Start }); // tell the lobby server that we started successfully
        }

        private byte[] GetByteArray(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static LobbySessionInfo consLobbySessionInfo(LobbyPlayerInfo playerInfo)
        {
            return new LobbySessionInfo()
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
            var result = new LobbyServerPlayerInfo()
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

        class ServerGame {
            public LobbyGameInfo gameInfo;
            public LobbyTeamInfo teamInfo;
        }
    }
}
