using System;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using WebSocketSharp;

public class MyNetworkClientConnection : NetworkConnection
{
	public static Action<UNetMessage> OnSending = delegate(UNetMessage A_0)
	{
	};

	public CloseStatusCode CloseStatusCode;

	private string m_gameServerAddress;

	private MyNetworkClient m_myNetworkClient;

	private GameClientInterface m_gameClientInterface;

	public TimeSpan HeartbeatTimeout
	{
		get
		{
			TimeSpan result;
			if (this.m_gameClientInterface != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkClientConnection.get_HeartbeatTimeout()).MethodHandle;
				}
				result = this.m_gameClientInterface.HeartbeatTimeout;
			}
			else
			{
				result = TimeSpan.Zero;
			}
			return result;
		}
		set
		{
			if (this.m_gameClientInterface != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkClientConnection.set_HeartbeatTimeout(TimeSpan)).MethodHandle;
				}
				this.m_gameClientInterface.HeartbeatTimeout = value;
			}
		}
	}

	public void Connect()
	{
		this.m_gameClientInterface = new GameClientInterface();
		this.m_gameClientInterface.Initialize(this.m_gameServerAddress, this.m_myNetworkClient.UserHandle);
		this.m_gameClientInterface.OnConnected += this.HandleConnectedToGameServer;
		this.m_gameClientInterface.OnDisconnected += this.HandleDisconnectedFromGameServer;
		this.m_gameClientInterface.OnConnectionError += this.HandleConnectionErrorToGameServer;
		this.m_gameClientInterface.OnMessage += this.HandleMessageFromGameServer;
		this.m_gameClientInterface.Connect();
	}

	public override void Initialize(string networkAddress, int networkHostId, int networkConnectionId, HostTopology hostTopology)
	{
		base.Initialize(networkAddress, networkHostId, networkConnectionId, hostTopology);
		this.m_myNetworkClient = (NetworkManager.singleton.client as MyNetworkClient);
		string text;
		if (this.m_myNetworkClient.UseSSL)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkClientConnection.Initialize(string, int, int, HostTopology)).MethodHandle;
			}
			text = "wss://";
		}
		else
		{
			text = "ws://";
		}
		string arg = text;
		this.m_gameServerAddress = string.Format("{0}{1}:{2}", arg, NetworkManager.singleton.networkAddress, NetworkManager.singleton.networkPort);
		Log.Info("MyNetworkClientConnection.Initialize address={0}", new object[]
		{
			this.m_gameServerAddress
		});
		this.Connect();
	}

	protected void HandleConnectedToGameServer()
	{
		Log.Info("MyNetworkClientConnection.HandleConnectedToGameServer {0}", new object[]
		{
			this.m_gameServerAddress
		});
		this.m_myNetworkClient.IsConnected = true;
		this.connectionId = 0;
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		this.m_gameClientInterface.HeartbeatPeriod = hydrogenConfig.HeartbeatPeriod;
		this.m_gameClientInterface.HeartbeatTimeout = hydrogenConfig.HeartbeatTimeout;
		this.m_gameClientInterface.MaxSendBufferSize = hydrogenConfig.MaxSendBufferSize;
		this.m_gameClientInterface.MaxWaitTime = hydrogenConfig.MaxWaitTime;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager.IsReconnectingInstantly)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkClientConnection.HandleConnectedToGameServer()).MethodHandle;
			}
			clientGameManager.ReloginToGameServerInstantly(this);
		}
		else
		{
			base.InvokeHandlerNoData(0x20);
		}
	}

	protected void HandleDisconnectedFromGameServer(string reason, bool allowRelogin, CloseStatusCode code)
	{
		Log.Warning("MyNetworkClientConnection.HandleDisconnectedFromGameServer {0} reason={1} allowRelogin={2} CloseStatusCode={3}", new object[]
		{
			this.m_gameServerAddress,
			reason,
			allowRelogin,
			code
		});
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (allowRelogin)
		{
			if (clientGameManager.ReconnectToGameServerInstantly(this))
			{
				return;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkClientConnection.HandleDisconnectedFromGameServer(string, bool, CloseStatusCode)).MethodHandle;
			}
		}
		this.m_myNetworkClient.IsConnected = false;
		this.CloseStatusCode = code;
		this.connectionId = -1;
		base.InvokeHandlerNoData(0x21);
	}

	protected void HandleConnectionErrorToGameServer(string errorMessage)
	{
		Log.Info("MyNetworkClientConnection.HandleConnectionErrorToGameServer {0} {1}", new object[]
		{
			this.m_gameServerAddress,
			errorMessage
		});
		StringMessage stringMessage = new StringMessage();
		stringMessage.value = errorMessage;
		byte[] buffer = new byte[0x2000];
		NetworkWriter writer = new NetworkWriter(buffer);
		stringMessage.Serialize(writer);
		NetworkReader reader = new NetworkReader(buffer);
		base.InvokeHandler(new NetworkMessage
		{
			msgType = 0x22,
			reader = reader,
			conn = this,
			channelId = 0
		});
	}

	protected void HandleMessageFromGameServer(BinaryMessageNotification notification)
	{
		UNetMessage unetMessage = new UNetMessage();
		unetMessage.Deserialize(notification.RawData);
		this.TransportReceive(unetMessage.Bytes, unetMessage.NumBytes, 0);
	}

	public unsafe override bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
	{
		error = 0;
		UNetMessage unetMessage = new UNetMessage
		{
			Bytes = bytes,
			NumBytes = numBytes
		};
		byte[] bytes2 = unetMessage.Serialize();
		if (this.m_gameClientInterface != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkClientConnection.TransportSend(byte[], int, int, byte*)).MethodHandle;
			}
			this.m_gameClientInterface.SendMessage(bytes2);
		}
		MyNetworkClientConnection.OnSending(unetMessage);
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkClientConnection.SendBytes(byte[], int, int)).MethodHandle;
			}
			return base.SendBytes(bytes, numBytes, channelId);
		}
		return true;
	}

	public override bool SendWriter(NetworkWriter writer, int channelId)
	{
		if (!ClientGameManager.Get().IsFastForward)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkClientConnection.SendWriter(NetworkWriter, int)).MethodHandle;
			}
			return base.SendWriter(writer, channelId);
		}
		return true;
	}

	public void Update()
	{
		if (this.m_gameClientInterface != null)
		{
			this.m_gameClientInterface.Update();
		}
	}

	public void Close()
	{
		if (this.m_gameClientInterface != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkClientConnection.Close()).MethodHandle;
			}
			this.m_gameClientInterface.Disconnect();
			this.m_gameClientInterface.OnConnected -= this.HandleConnectedToGameServer;
			this.m_gameClientInterface.OnDisconnected -= this.HandleDisconnectedFromGameServer;
			this.m_gameClientInterface.OnConnectionError -= this.HandleConnectionErrorToGameServer;
			this.m_gameClientInterface.OnMessage -= this.HandleMessageFromGameServer;
		}
	}
}
