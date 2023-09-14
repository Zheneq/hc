using System;
using System.Collections.Generic;
using UnityEngine.Networking;

#if SERVER
// custom
public class MockServerNetworkConnection : NetworkConnection
{
    private readonly Replay replay = new Replay();
    private bool isRecording;
    private readonly NetworkWriter writer = new NetworkWriter();
    private uint lastMessageOutgoingSeqNumStub;
    private readonly Func<short, MessageBase, bool> OnServerToClientMessage = (_, __) => true;
    private readonly HashSet<uint> m_bannedRpcs = new HashSet<uint>();
    
    public MockServerNetworkConnection(HashSet<uint> bannedRpcs = null, Func<short, MessageBase, bool> onServerToClientMessage = null)
    {
        if (!(onServerToClientMessage is null))
        {
            OnServerToClientMessage = onServerToClientMessage;
        }

        if (!(bannedRpcs is null))
        {
            m_bannedRpcs = bannedRpcs;
        }
    }
    
    public override bool SendByChannel(short msgType, MessageBase msg, int channelId)
    {
        if (OnServerToClientMessage(msgType, msg))
        {
            return base.SendByChannel(msgType, msg, channelId);
        }
        return true;
    }

    public void SendMessageToServer(short msgType, MessageBase msg)
    {
        writer.StartMessage(msgType);
        msg.Serialize(writer);
        writer.FinishMessage();
        writer.WriteSeqNum(++lastMessageOutgoingSeqNumStub);
        var buffer = writer.AsArray();
        HandleBytes(buffer, buffer.Length, 0);
    }

    public override bool SendBytes(byte[] bytes, int numBytes, int channelId)
    {
        if (bytes[6] == 2 && bytes[7] == 0)
        {
            NetworkReader reader = new NetworkReader(bytes);
            reader.ReadBytes(8);
            uint rpcCode = reader.ReadPackedUInt32();
            NetworkInstanceId netId = reader.ReadNetworkId();
            bool skip = m_bannedRpcs.Contains(rpcCode);
            // Log.Info($"Detected rpc {rpcCode} from {netId}, skip={skip}");
            if (skip)
            {
                return true;
            }
        }
        
        uint num = ++lastMessageOutgoingSeqNum;
        bytes[0] = (byte) (num & byte.MaxValue);
        bytes[1] = (byte) (num >> 8 & byte.MaxValue);
        bytes[2] = (byte) (num >> 16 & byte.MaxValue);
        bytes[3] = (byte) (num >> 24 & byte.MaxValue);
        ReceiveMessage(bytes, numBytes);
        return true;
    }

    public override bool ResendBytes(byte[] bytes, int numBytes, int channelId)
    {
        Log.Warning($"Replay recorder {connectionId} Resend {numBytes} Bytes");
        // TODO REPLAYS ignore? doubt we can miss anything
        // ReceiveMessage(bytes, numBytes);
        return true;
    }

    public override bool SendWriter(NetworkWriter writer, int channelId)
    {
        writer.WriteSeqNum(++lastMessageOutgoingSeqNum);
        ReceiveMessage(writer.AsArray(), writer.Position);
        return true;
    }

    private void ReceiveMessage(byte[] bytes, int numBytes)
    {
        if (!isRecording) return;
		
        replay.RecordRawNetworkMessage(bytes, numBytes);
    }

    public Replay GetReplay()
    {
        return replay;
    }

    public void StartRecordingReplay()
    {
        isRecording = true;
    }

    public void StopRecordingReplay()
    {
        isRecording = false;
    }
}
#endif
