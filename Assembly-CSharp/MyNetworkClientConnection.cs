using System;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using WebSocketSharp;

public class MyNetworkClientConnection : NetworkConnection
{
	public static Action<UNetMessage> OnSending = delegate { };

	public CloseStatusCode CloseStatusCode;

	private string m_gameServerAddress;
	private MyNetworkClient m_myNetworkClient;
	private GameClientInterface m_gameClientInterface;

	public TimeSpan HeartbeatTimeout
	{
		get => m_gameClientInterface?.HeartbeatTimeout ?? TimeSpan.Zero;
		set
		{
			if (m_gameClientInterface != null)
			{
				m_gameClientInterface.HeartbeatTimeout = value;
			}
		}
	}

	public void Connect()
	{
		m_gameClientInterface = new GameClientInterface();
		m_gameClientInterface.Initialize(m_gameServerAddress, m_myNetworkClient.UserHandle);
		m_gameClientInterface.OnConnected += HandleConnectedToGameServer;
		m_gameClientInterface.OnDisconnected += HandleDisconnectedFromGameServer;
		m_gameClientInterface.OnConnectionError += HandleConnectionErrorToGameServer;
		m_gameClientInterface.OnMessage += HandleMessageFromGameServer;
		m_gameClientInterface.Connect();
	}

	public override void Initialize(string networkAddress, int networkHostId, int networkConnectionId, HostTopology hostTopology)
	{
		base.Initialize(networkAddress, networkHostId, networkConnectionId, hostTopology);
		m_myNetworkClient = NetworkManager.singleton.client as MyNetworkClient;
		string arg = m_myNetworkClient.UseSSL ? "wss://" : "ws://";
		m_gameServerAddress = $"{arg}{NetworkManager.singleton.networkAddress}:{NetworkManager.singleton.networkPort}";
		Log.Info("MyNetworkClientConnection.Initialize address={0}", m_gameServerAddress);
		Connect();
	}

	protected void HandleConnectedToGameServer()
	{
		Log.Info("MyNetworkClientConnection.HandleConnectedToGameServer {0}", m_gameServerAddress);
		m_myNetworkClient.IsConnected = true;
		connectionId = 0;
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		m_gameClientInterface.HeartbeatPeriod = hydrogenConfig.HeartbeatPeriod;
		m_gameClientInterface.HeartbeatTimeout = hydrogenConfig.HeartbeatTimeout;
		m_gameClientInterface.MaxSendBufferSize = hydrogenConfig.MaxSendBufferSize;
		m_gameClientInterface.MaxWaitTime = hydrogenConfig.MaxWaitTime;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager.IsReconnectingInstantly)
		{
			clientGameManager.ReloginToGameServerInstantly(this);
		}
		else
		{
			InvokeHandlerNoData(MsgType.Connect);
		}
	}

	protected void HandleDisconnectedFromGameServer(string reason, bool allowRelogin, CloseStatusCode code)
	{
		Log.Warning("MyNetworkClientConnection.HandleDisconnectedFromGameServer {0} reason={1} allowRelogin={2} CloseStatusCode={3}", m_gameServerAddress, reason, allowRelogin, code);
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (allowRelogin && clientGameManager.ReconnectToGameServerInstantly(this))
		{
			return;
		}
		m_myNetworkClient.IsConnected = false;
		CloseStatusCode = code;
		connectionId = -1;
		InvokeHandlerNoData(MsgType.Disconnect);
	}

	protected void HandleConnectionErrorToGameServer(string errorMessage)
	{
		Log.Info("MyNetworkClientConnection.HandleConnectionErrorToGameServer {0} {1}", m_gameServerAddress, errorMessage);
		StringMessage stringMessage = new StringMessage
		{
			value = errorMessage
		};
		byte[] buffer = new byte[8192];
		NetworkWriter writer = new NetworkWriter(buffer);
		stringMessage.Serialize(writer);
		NetworkReader reader = new NetworkReader(buffer);
		NetworkMessage networkMessage = new NetworkMessage
		{
			msgType = MsgType.Error,
			reader = reader,
			conn = this,
			channelId = 0
		};
		InvokeHandler(networkMessage);
	}

	protected void HandleMessageFromGameServer(BinaryMessageNotification notification)
	{
		UNetMessage uNetMessage = new UNetMessage();
		uNetMessage.Deserialize(notification.RawData);
		TransportReceive(uNetMessage.Bytes, uNetMessage.NumBytes, 0);
	}

	public override bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
	{
		error = 0;
		UNetMessage uNetMessage = new UNetMessage
		{
			Bytes = bytes,
			NumBytes = numBytes
		};
		byte[] uNetMessageBytes = uNetMessage.Serialize();
		if (m_gameClientInterface != null)
		{
			m_gameClientInterface.SendMessage(uNetMessageBytes);
		}
		OnSending(uNetMessage);
		return true;
	}

	public override void TransportReceive(byte[] bytes, int numBytes, int channelId)
	{
		base.TransportReceive(bytes, numBytes, channelId);
	}

	public override bool SendBytes(byte[] bytes, int numBytes, int channelId)
	{
		if (!ClientGameManager.Get().IsFastForward)
		{
			return base.SendBytes(bytes, numBytes, channelId);
		}
		return true;
	}

	public override bool SendWriter(NetworkWriter writer, int channelId)
	{
		if (!ClientGameManager.Get().IsFastForward)
		{
			return base.SendWriter(writer, channelId);
		}
		return true;
	}

	public void Update()
	{
		if (m_gameClientInterface != null)
		{
			m_gameClientInterface.Update();
		}
	}

	public void Close()
	{
		if (m_gameClientInterface != null)
		{
			m_gameClientInterface.Disconnect();
			m_gameClientInterface.OnConnected -= HandleConnectedToGameServer;
			m_gameClientInterface.OnDisconnected -= HandleDisconnectedFromGameServer;
			m_gameClientInterface.OnConnectionError -= HandleConnectionErrorToGameServer;
			m_gameClientInterface.OnMessage -= HandleMessageFromGameServer;
		}
	}
}
