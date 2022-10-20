using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
	internal const ushort MAX_PACKET_BYTES = 0x4B0;
	internal const short MAX_PACKET_PAYLOAD_BYTES = 0x400;
	internal const short MAX_PENDING_BUFFERS = 0x80;
	internal const int FIRST_PLAYER_CHANNEL_INDEX = 6;
	internal const ushort MESSAGE_QUEUE_AND_POOL_SIZE = 0x400;

	private int m_nextPlayerChannelIndex;
	private int m_nextLocalConnectionId;

	public MyNetworkManager()
	{
		m_OnServerStartHolder = delegate {};
		m_OnServerConnectHolder = delegate {};
		m_OnServerDisconnectHolder = delegate {};
		m_OnServerReadyHolder = delegate {};		
		m_OnServerAddPlayerHolder = delegate {};		
		m_OnServerRemovePlayerHolder = delegate {};		
		m_OnServerErrorHolder = delegate {};		
		m_OnClientConnectHolder = delegate {};
		m_OnClientDisconnectHolder = delegate {};		
		m_OnClientNotReadyHolder = delegate {};		
		m_OnClientErrorHolder = delegate {};
		m_nextPlayerChannelIndex = 6;
	}

	internal static MyNetworkManager Get()
	{
		return (MyNetworkManager)singleton;
	}

	private Action m_OnServerStartHolder;
	public event Action m_OnServerStart
	{
		add
		{
			Action action = m_OnServerStartHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnServerStartHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = m_OnServerStartHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnServerStartHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<NetworkConnection> m_OnServerConnectHolder;
	public event Action<NetworkConnection> m_OnServerConnect
	{
		add
		{
			Action<NetworkConnection> action = m_OnServerConnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnServerConnectHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection> action = m_OnServerConnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnServerConnectHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<NetworkConnection> m_OnServerDisconnectHolder;
	public event Action<NetworkConnection> m_OnServerDisconnect
	{
		add
		{
			Action<NetworkConnection> action = m_OnServerDisconnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnServerDisconnectHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection> action = m_OnServerDisconnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnServerDisconnectHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<NetworkConnection> m_OnServerReadyHolder;
	public event Action<NetworkConnection> m_OnServerReady;

	private Action<NetworkConnection, short> m_OnServerAddPlayerHolder;
	public event Action<NetworkConnection, short> m_OnServerAddPlayer
	{
		add
		{
			Action<NetworkConnection, short> action = m_OnServerAddPlayerHolder;
			Action<NetworkConnection, short> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnServerAddPlayerHolder, (Action<NetworkConnection, short>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection, short> action = m_OnServerAddPlayerHolder;
			Action<NetworkConnection, short> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnServerAddPlayerHolder, (Action<NetworkConnection, short>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<NetworkConnection, PlayerController> m_OnServerRemovePlayerHolder;
	public event Action<NetworkConnection, PlayerController> m_OnServerRemovePlayer
	{
		add
		{
			Action<NetworkConnection, PlayerController> action = m_OnServerRemovePlayerHolder;
			Action<NetworkConnection, PlayerController> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnServerRemovePlayerHolder, (Action<NetworkConnection, PlayerController>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection, PlayerController> action = m_OnServerRemovePlayerHolder;
			Action<NetworkConnection, PlayerController> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnServerRemovePlayerHolder, (Action<NetworkConnection, PlayerController>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<NetworkConnection, NetworkError> m_OnServerErrorHolder;
	public event Action<NetworkConnection, NetworkError> m_OnServerError
	{
		add
		{
			Action<NetworkConnection, NetworkError> action = m_OnServerErrorHolder;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnServerErrorHolder, (Action<NetworkConnection, NetworkError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection, NetworkError> action = m_OnServerErrorHolder;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnServerErrorHolder, (Action<NetworkConnection, NetworkError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<NetworkConnection> m_OnClientConnectHolder;
	public event Action<NetworkConnection> m_OnClientConnect
	{
		add
		{
			Action<NetworkConnection> action = m_OnClientConnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnClientConnectHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection> action = m_OnClientConnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnClientConnectHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<NetworkConnection> m_OnClientDisconnectHolder;
	public event Action<NetworkConnection> m_OnClientDisconnect
	{
		add
		{
			Action<NetworkConnection> action = m_OnClientDisconnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnClientDisconnectHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection> action = m_OnClientDisconnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnClientDisconnectHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<NetworkConnection> m_OnClientNotReadyHolder;
	public event Action<NetworkConnection> m_OnClientNotReady
	{
		add
		{
			Action<NetworkConnection> action = m_OnClientNotReadyHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnClientNotReadyHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection> action = m_OnClientNotReadyHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnClientNotReadyHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private Action<NetworkConnection, NetworkError> m_OnClientErrorHolder;
	public event Action<NetworkConnection, NetworkError> m_OnClientError
	{
		add
		{
			Action<NetworkConnection, NetworkError> action = m_OnClientErrorHolder;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnClientErrorHolder, (Action<NetworkConnection, NetworkError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection, NetworkError> action = m_OnClientErrorHolder;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref m_OnClientErrorHolder, (Action<NetworkConnection, NetworkError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Start()
	{
		connectionConfig.PacketSize = 0x4B0;
		connectionConfig.NetworkDropThreshold = 0x32;
		connectionConfig.OverflowDropThreshold = 0x32;
		connectionConfig.IsAcksLong = true;
		connectionConfig.MaxSentMessageQueueSize = 0x400;
		connectionConfig.PingTimeout = (uint)HydrogenConfig.Get().HeartbeatPeriod.TotalMilliseconds;
		connectionConfig.DisconnectTimeout = (uint)HydrogenConfig.Get().HeartbeatTimeout.TotalMilliseconds;
		logLevel = LogFilter.FilterLevel.Info;
		if (0x24 < connectionConfig.ChannelCount)
		{
			Log.Error("BootstrapSingletons.prefab>My Network Manager>QoS Channels only has {0} channels but our script requires {1}", connectionConfig.ChannelCount, NetworkChannelId.Count);
		}
	}

	internal NetworkClient MyStartClient(string gameServerAddress, string userHandle)
	{
		matchInfo = null;
		Uri uri = new Uri(gameServerAddress);
		singleton.networkAddress = uri.Host;
		singleton.networkPort = uri.Port;
		useWebSockets = uri.Scheme == "ws" || uri.Scheme == "wss";
		bool useSSL = uri.Scheme == "wss";
		NetworkTransport.Init(globalConfig);
		if (useWebSockets)
		{
			MyNetworkClient myNetworkClient = new MyNetworkClient
			{
				UserHandle = userHandle,
				UseSSL = useSSL
			};
			myNetworkClient.SetNetworkConnectionClass<MyNetworkClientConnection>();
			client = myNetworkClient;
		}
		else
		{
			client = new NetworkClient
			{
				messageDispatcher = delegate(NetworkMessageDelegate networkMessageFunction, NetworkMessage data)
				{
					if (AsyncPump.Current != null)
					{
						NetworkMessage mirror = new NetworkMessage
						{
							channelId = data.channelId,
							conn = data.conn,
							msgType = data.msgType,
							reader = data.reader
						};
						AsyncPump.Current.Post(delegate
						{
							networkMessageFunction(mirror);
						});
					}
					else
					{
						networkMessageFunction(data);
					}
				}
			};
		}
		ConfigureClient();
		UseExternalClient(client);
		if (matchInfo != null)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager StartClient match: " + matchInfo);
			}
			client.Connect(matchInfo);
		}
		else if (secureTunnelEndpoint != null)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager StartClient using provided SecureTunnel");
			}
			client.Connect(secureTunnelEndpoint);
		}
		else
		{
			if (string.IsNullOrEmpty(networkAddress))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("Must set the Network Address field in the manager");
				}
				return null;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log(string.Concat("NetworkManager StartClient address:", networkAddress, " port:", networkPort));
			}
			if (useSimulator)
			{
				client.ConnectWithSimulator(networkAddress, networkPort, simulatedLatency, packetLossPercentage);
			}
			else
			{
				client.Connect(networkAddress, networkPort);
			}
		}
		if (migrationManager != null)
		{
			migrationManager.Initialize(client, matchInfo);
		}
		return client;
	}

	internal NetworkClient MyStartClientStub()
	{
		matchInfo = null;
		NetworkTransport.Init(globalConfig);
		MyNetworkClient myNetworkClient = new MyNetworkClient();
		myNetworkClient.SetNetworkConnectionClass<StubClientConnection>();
		client = myNetworkClient;
		ConfigureClient();
		UseExternalClient(client);
		client.Connect("localhost", 0);
		if (migrationManager != null)
		{
			migrationManager.Initialize(client, matchInfo);
		}
		return client;
	}

	private void ConfigureClient()
	{
		if (customConfig)
		{
			connectionConfig.Channels.Clear();
			foreach (QosType value in channels)
			{
				connectionConfig.AddChannel(value);
			}
			connectionConfig.PingTimeout = (uint)HydrogenConfig.Get().HeartbeatPeriod.TotalMilliseconds;
			connectionConfig.DisconnectTimeout = (uint)HydrogenConfig.Get().HeartbeatTimeout.TotalMilliseconds;
			if (connectionConfig.UsePlatformSpecificProtocols && Application.platform != RuntimePlatform.PS4)
			{
				throw new ArgumentOutOfRangeException("Platform specific protocols are not supported on this platform");
			}
			HostTopology hostTopology = new HostTopology(connectionConfig, 1)
			{
				SentMessagePoolSize = 0x400,
				ReceivedMessagePoolSize = 0x400
			};
			client.Configure(hostTopology);
		}
	}

	internal void ConfigureServer()
	{
	}

	public override void OnStartServer()
	{
		m_nextPlayerChannelIndex = 6;
		if (m_OnServerStartHolder != null)
		{
			m_OnServerStartHolder();
		}
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		SetDefaultChannelOptions(conn);
		if (m_OnServerConnectHolder != null)
		{
			m_OnServerConnectHolder(conn);
		}
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		if (m_OnServerDisconnectHolder != null)
		{
			m_OnServerDisconnectHolder(conn);
		}
	}

	public override void OnServerReady(NetworkConnection conn)
	{
		if (m_OnServerReadyHolder != null)
		{
			m_OnServerReadyHolder(conn);
		}
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		if (m_OnServerAddPlayerHolder != null)
		{
			m_OnServerAddPlayerHolder(conn, playerControllerId);
		}
	}

	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
	{
		if (m_OnServerRemovePlayerHolder != null)
		{
			m_OnServerRemovePlayerHolder(conn, player);
		}
	}

	public override void OnServerError(NetworkConnection conn, int errorCode)
	{
		if (m_OnServerErrorHolder != null)
		{
			m_OnServerErrorHolder(conn, (NetworkError)errorCode);
		}
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		SetDefaultChannelOptions(conn);
		if (m_OnClientConnectHolder != null)
		{
			m_OnClientConnectHolder(conn);
		}
	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		if (m_OnClientDisconnectHolder != null)
		{
			m_OnClientDisconnectHolder(conn);
		}
	}

	public override void OnClientError(NetworkConnection conn, int errorCode)
	{
		if (m_OnClientErrorHolder != null)
		{
			m_OnClientErrorHolder(conn, (NetworkError)errorCode);
		}
	}

	public override void OnClientNotReady(NetworkConnection conn)
	{
		if (m_OnClientNotReadyHolder != null)
		{
			m_OnClientNotReadyHolder(conn);
		}
	}

	private void SetDefaultChannelOptions(NetworkConnection conn)
	{
		for (int i = 0; i < channels.Count; i++)
		{
			if (i != 1)
			{
				conn.SetChannelOption(i, ChannelOption.MaxPendingBuffers, 0x80);
			}
		}
	}

	internal int GetNextPlayerChannelIndex()
	{
		if (m_nextPlayerChannelIndex >= channels.Count)
		{
			Log.Error("Ran out of unique channels for players, server to client packets may be lost! Allocate more channels in MyNetworkManager.cs and BootstrapSingletons.prefab");
			m_nextPlayerChannelIndex = 6;
		}
		return m_nextPlayerChannelIndex++;
	}

	public int AllocateLocalConnectionId()
	{
		if (m_nextLocalConnectionId == 0)
		{
			m_nextLocalConnectionId = maxConnections + 1;
		}
		return m_nextLocalConnectionId++;
	}
}
