// ROGUES
// SERVER
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

#if SERVER
// custom
public class ReplayRecorder
{
    private readonly Replay m_replay;
    private readonly ServerPlayerState m_recorderPlayerState;
    private MyNetworkClient m_client;

    public ReplayRecorder(ServerPlayerState playerState)
    {
        m_replay = new Replay();
        m_recorderPlayerState = playerState;
        Connect();
    }

    public void SaveReplayToFile(string path)
    {
        string json = JsonUtility.ToJson(m_replay);
        new FileInfo(path).Directory?.Create();
        File.WriteAllText(path, json);
        Log.Info($"Saved {m_replay.m_messages.Count} messages into {path}");
    }

    public void SaveReplay()
    {
        string filename = $"{DateTime.Now:yyyy_MM_dd__HH_mm_ss}__" +
                          $"{GameManager.Get().GameInfo.GameServerProcessCode}__" +
                          $"{BuildVersion.MiniVersionString}.arr";
        SaveReplayToFile(Path.Combine(HydrogenConfig.Get().ReplaysPath, filename));
        if (m_client.connection is ReplayRecorderNetworkConnection conn)
        {
            conn.OnMessage -= m_replay.RecordRawNetworkMessage;
        }
    }

    private void PopulateReplayData()
    {
        int playerId = m_recorderPlayerState.PlayerInfo.PlayerId;
        GameManager gameManager = GameManager.Get();
        LobbyTeamInfo teamInfo = gameManager.TeamInfo;
        m_replay.m_gameInfo_Serialized = JsonUtility.ToJson(gameManager.GameInfo);
        m_replay.m_gameplayOverrides_Serialized = JsonUtility.ToJson(gameManager.GameplayOverrides);
        m_replay.m_teamInfo_Serialized = JsonUtility.ToJson(teamInfo);
        m_replay.m_playerInfo_Index = teamInfo.TeamPlayerInfo.FindIndex(pi => pi.PlayerId == playerId);
        m_replay.m_versionMini = BuildVersion.MiniVersionString;
        m_replay.m_versionFull = BuildVersion.FullVersionString;
    }

    public void Connect()
    {
        Log.Info($"Replay recorder connecting");
        MyNetworkClient myNetworkClient = new MyNetworkClient
        {
            UserHandle = "replay_recorder",
            UseSSL = false
        };
        myNetworkClient.SetNetworkConnectionClass<ReplayRecorderNetworkConnection>();
        m_client = myNetworkClient;
        MyNetworkManager.Get().UseExternalClient(m_client);
        MyNetworkManager.Get().m_OnClientConnect += HandleConnected;
        m_client.RegisterHandler((short)MyMsgType.LoginResponse, HandleLoginResponse);
        NetworkManager.singleton.networkAddress = "127.0.0.1";
        m_client.Connect("127.0.0.1", ServerGameManager.s_port);
    }

    private void HandleConnected(NetworkConnection conn)
    {
        Log.Info($"Replay recorder connected");
        GameManager.LoginRequest loginRequest = new GameManager.LoginRequest
        {
            AccountId = Convert.ToString(m_recorderPlayerState.SessionInfo.AccountId),
            SessionToken = Convert.ToString(m_recorderPlayerState.SessionInfo.SessionToken),
            PlayerId = m_recorderPlayerState.PlayerInfo.PlayerId,
            LastReceivedMsgSeqNum = 0
        };
        m_client.Send((int)MyMsgType.LoginRequest, loginRequest);
        Log.Info($"Sent login request");
    }

    private void HandleLoginResponse(NetworkMessage msg)
    {
        GameManager.LoginResponse loginResponse = msg.ReadMessage<GameManager.LoginResponse>();
        if (loginResponse == null || !loginResponse.Success)
        {
            Log.Info($"Failed to log in to record replay");
            return;
        }
        Log.Info($"Successfully logged in to record replay");
        if (m_client.connection is ReplayRecorderNetworkConnection conn)
        {
            conn.OnMessage += m_replay.RecordRawNetworkMessage;
            conn.ConsumeMessages = true;
        }
        else
        {
            Log.Info($"Client connection is bad {m_client.connection}");
        }
        Log.Info($"Recording replay");

        GameManager.AssetsLoadedNotification notification = new GameManager.AssetsLoadedNotification()
        {
            AccountId = m_recorderPlayerState.SessionInfo.AccountId,
            PlayerId = m_recorderPlayerState.PlayerInfo.PlayerId
        };
        m_client.Send((int)MyMsgType.AssetsLoadedNotification, notification);
        
        PopulateReplayData();
    }
}
#endif