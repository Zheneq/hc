using System.Collections.Generic;
using UnityEngine.Networking;

#if SERVER
// custom
public class MyNetworkServerConnection : NetworkConnection
{
	private readonly bool m_consumeMessages;
	private readonly Replay m_replay = new Replay();
	private bool m_recording = false;
	
	public MyNetworkServerConnection() : this(false)
	{
	}

	public MyNetworkServerConnection(bool consumeMessages, List<Replay.Message> messageHistory = null)
	{
		m_consumeMessages = consumeMessages;
		if (!(messageHistory is null))
		{
			m_replay.m_messages.AddRange(messageHistory);
		}
	}

	public override bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
	{
		ReceiveMessage(bytes, numBytes);
		if (m_consumeMessages)
		{
			Log.Info("Consumed a message");
			error = 0;
			return true;
		}
		return base.TransportSend(bytes, numBytes, channelId, out error);
	}
	
	private void ReceiveMessage(byte[] bytes, int numBytes)
	{
		if (!m_recording) return;
		
		m_replay.RecordRawNetworkMessage(bytes, numBytes);
	}

	public List<Replay.Message> GetMessages()
	{
		Log.Info($"Reconnection: {m_replay.GetSecondsSinceLastMessage()} seconds since last recorded message");
		return m_replay.m_messages;
	}

	public void StartRecordingReconnectionData()
	{
		m_recording = true;
	}
}
#endif
