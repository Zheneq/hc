// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

#if SERVER
// custom
public class ReplayRecorder
{
    private readonly ServerPlayerState m_recorderPlayerState;
    private MockServerNetworkConnection conn;
    private readonly DateTime m_time;
    private readonly string m_suffix;
    private bool isInitialized = false;

    private string Handle => m_recorderPlayerState.PlayerInfo.Handle;

    public Replay Replay => conn.GetReplay();

    public ReplayRecorder(ServerPlayerState playerState, string suffix = "")
    {
        m_recorderPlayerState = playerState;
        m_suffix = suffix;
        long? timestamp = GameManager.Get().GameInfo?.CreateTimestamp;
        m_time = timestamp.HasValue ? new DateTime(timestamp.Value) : DateTime.Now;
        Connect();
    }

    public void SaveReplayToFile(string path)
    {
        PopulateReplayData();
        string json = GetReplayAsJson();
        new FileInfo(path).Directory?.Create();
        File.WriteAllText(path, json);
        Log.Info($"Saved {Replay.m_messages.Count} messages into {path}");
    }

    public void SaveReplay()
    {
        // TODO REPLAY check ClientGameManager#HandleReplayManagerFile
        string filename = $"{m_time:yyyy_MM_dd__HH_mm_ss}__" +
                          $"{GameManager.Get().GameInfo.GameServerProcessCode}{m_suffix}__" +
                          $"{BuildVersion.MiniVersionString}.arr";
        SaveReplayToFile(Path.Combine(HydrogenConfig.Get().ReplaysPath, filename));
    }

    public string GetReplayAsJson()
    {
        return JsonUtility.ToJson(Replay);
    }

    public void StopRecording()
    {
        conn?.StopRecordingReplay();
        try
        {
            Replay.m_messages = Optimize(Replay.m_messages);
        }
        catch (Exception e)
        {
            Log.Error("Failed to optimize replay: {0}\n{1}", e, e.StackTrace);
        }
    }

    private void PopulateReplayData()
    {
        int playerId = m_recorderPlayerState.PlayerInfo.PlayerId;
        GameManager gameManager = GameManager.Get();
        LobbyTeamInfo teamInfo = gameManager.TeamInfo;
        Replay.m_gameInfo_Serialized = JsonUtility.ToJson(gameManager.GameInfo);
        Replay.m_gameplayOverrides_Serialized = JsonUtility.ToJson(gameManager.GameplayOverrides);
        Replay.m_teamInfo_Serialized = JsonUtility.ToJson(teamInfo);
        Replay.m_playerInfo_Index = teamInfo.TeamPlayerInfo.FindIndex(pi => pi.PlayerId == playerId);
        Replay.m_versionMini = BuildVersion.MiniVersionString;
        Replay.m_versionFull = BuildVersion.FullVersionString;
    }

    public void Connect()
    {
        conn = new MockServerNetworkConnection(HandleMessageFromServer);
        conn.connectionId = Math.Max(NetworkServer.connections.Count, 20);  // TODO REPLAYS hack to not get our connection id overwritten
        conn.hostId = 0;
        NetworkServer.AddExternalConnection(conn);
        conn.RegisterHandler(32, HandleConnected);
        Log.Info($"{Handle} connecting");
    }
    
    private void HandleConnected(NetworkMessage _)
    {
        if (isInitialized)
        {
            // TODO RECONNECTION REPLAY figure out why it happens
            Log.Warning($"{Handle} received repeated connection acknowledgement");
            return;
        }
        isInitialized = true;
        Log.Info($"{Handle} connected as conn {conn.connectionId}");
        GameManager.LoginRequest loginRequest = new GameManager.LoginRequest
        {
            AccountId = Convert.ToString(m_recorderPlayerState.SessionInfo.AccountId),
            SessionToken = Convert.ToString(m_recorderPlayerState.SessionInfo.SessionToken),
            PlayerId = m_recorderPlayerState.PlayerInfo.PlayerId,
            LastReceivedMsgSeqNum = 0
        };
        Log.Info($"{Handle} sent login request");
        conn.SendMessageToServer((int)MyMsgType.LoginRequest, loginRequest);
    }

    private bool HandleMessageFromServer(short msgType, MessageBase msg)
    {
        if (msgType == (short)MyMsgType.LoginResponse)
        {
            Log.Info($"{Handle} received login response");
            HandleLoginResponse((GameManager.LoginResponse)msg);
        }
        else if (msgType == 12) // connection ready
        {
            StartRecording();
        }
        else if (msgType == 4 // owner message - no need to tell reconnecting players that they own spectator player (it converts them into actual spectators)
                 || msgType == 15 // set client authority true/false - irrelevant, might cause authority problems in reconnect replay (just in case)
                 || msgType == (short)MyMsgType.ReconnectReplayStatus) // throws client into infinite recursion if present in reconnect replay (just in case)
        {
            Log.Info($"{Handle} dropping msg type {msgType} ({DefaultJsonSerializer.Serialize(msg)})");
            return false;
        }
        return true;
    }

    private void HandleLoginResponse(GameManager.LoginResponse loginResponse)
    {
        if (loginResponse == null || !loginResponse.Success)
        {
            Log.Info($"{Handle} failed to log in to record replay");
            return;
        }
        Log.Info($"{Handle} successfully logged in to record replay");
        
        // Note: we used to start recording here

        GameManager.AssetsLoadedNotification notification = new GameManager.AssetsLoadedNotification()
        {
            AccountId = m_recorderPlayerState.SessionInfo.AccountId,
            PlayerId = m_recorderPlayerState.PlayerInfo.PlayerId
        };
        conn.SendMessageToServer((int)MyMsgType.AssetsLoadedNotification, notification);
        PopulateReplayData();
    }

    public void StartRecording()
    {
        conn.StartRecordingReplay();
        Log.Info($"{Handle} recording replay");
    }

    public static List<Replay.Message> Optimize(List<Replay.Message> replay, float threshold = 0.1f)
    {
        Log.Info("Optimizing replay...");
        if (replay.IsNullOrEmpty())
        {
            return replay;
        }

        List<Replay.Message> result = new List<Replay.Message>();
        
        List<Replay.Message> buffer = new List<Replay.Message> { replay[0] };
        for (int i = 1; i < replay.Count; i++)
        {
            Replay.Message msg = replay[i];
            if (buffer[0].timestamp + threshold < msg.timestamp)
            {
                result.Add(Merge(buffer));
                buffer.Clear();
            }

            buffer.Add(msg);
        }
        
        result.Add(Merge(buffer));

        Log.Info($"Optimized replay {replay.Count} -> {result.Count}");
        return result;
    }

    private static Replay.Message Merge(List<Replay.Message> messages)
    {
        if (messages.IsNullOrEmpty())
        {
            throw new ArgumentException();
        }
        if (messages.Count == 1)
        {
            return messages[0];
        }
        
        int size = messages.Select(msg => msg.data.Length).Sum();
        byte[] data = new byte[size];
        int offset = 0;
        foreach (Replay.Message message in messages)
        {
            Buffer.BlockCopy(message.data, 0, data, offset, message.data.Length);
            offset += message.data.Length;
        }

        return new Replay.Message
        {
            timestamp = messages[0].timestamp,
            data = data
        };
    }
}
#endif