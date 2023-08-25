using System;
using UnityEngine.Networking;

#if SERVER
// custom
public class MockServerNetworkConnection : NetworkConnection
{
    private readonly Replay replay = new Replay();
    private bool isRecording;
    private readonly NetworkWriter writer = new NetworkWriter();
    private uint lastMessageOutgoingSeqNumStub;
    
    public Action<short, MessageBase> OnServerToClientMessage = delegate {};

    public override bool Send(short msgType, MessageBase msg)
    {
        OnServerToClientMessage?.Invoke(msgType, msg);
        return base.Send(msgType, msg);
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
