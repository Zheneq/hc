using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
	internal const ushort MAX_PACKET_BYTES = 1200;

	internal const short MAX_PACKET_PAYLOAD_BYTES = 1024;

	internal const short MAX_PENDING_BUFFERS = 128;

	internal const int FIRST_PLAYER_CHANNEL_INDEX = 6;

	internal const ushort MESSAGE_QUEUE_AND_POOL_SIZE = 1024;

	private int m_nextPlayerChannelIndex;

	private int m_nextLocalConnectionId;

	private Action m_OnServerStartHolder;
	public event Action m_OnServerStart
	{
		add
		{
			Action action = this.m_OnServerStartHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerStartHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action action = this.m_OnServerStartHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerStartHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	private Action<NetworkConnection> m_OnServerConnectHolder;
	public event Action<NetworkConnection> m_OnServerConnect
	{
		add
		{
			Action<NetworkConnection> action = this.m_OnServerConnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerConnectHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnServerConnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerConnectHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	private Action<NetworkConnection> m_OnServerDisconnectHolder;
	public event Action<NetworkConnection> m_OnServerDisconnect
	{
		add
		{
			Action<NetworkConnection> action = this.m_OnServerDisconnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerDisconnectHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnServerDisconnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerDisconnectHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	private Action<NetworkConnection> m_OnServerReadyHolder;
	public event Action<NetworkConnection> m_OnServerReady;

	private Action<NetworkConnection, short> m_OnServerAddPlayerHolder;
	public event Action<NetworkConnection, short> m_OnServerAddPlayer
	{
		add
		{
			Action<NetworkConnection, short> action = this.m_OnServerAddPlayerHolder;
			Action<NetworkConnection, short> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerAddPlayerHolder, (Action<NetworkConnection, short>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<NetworkConnection, short> action = this.m_OnServerAddPlayerHolder;
			Action<NetworkConnection, short> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerAddPlayerHolder, (Action<NetworkConnection, short>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	private Action<NetworkConnection, PlayerController> m_OnServerRemovePlayerHolder;
	public event Action<NetworkConnection, PlayerController> m_OnServerRemovePlayer
	{
		add
		{
			Action<NetworkConnection, PlayerController> action = this.m_OnServerRemovePlayerHolder;
			Action<NetworkConnection, PlayerController> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerRemovePlayerHolder, (Action<NetworkConnection, PlayerController>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<NetworkConnection, PlayerController> action = this.m_OnServerRemovePlayerHolder;
			Action<NetworkConnection, PlayerController> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerRemovePlayerHolder, (Action<NetworkConnection, PlayerController>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	private Action<NetworkConnection, NetworkError> m_OnServerErrorHolder;
	public event Action<NetworkConnection, NetworkError> m_OnServerError
	{
		add
		{
			Action<NetworkConnection, NetworkError> action = this.m_OnServerErrorHolder;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerErrorHolder, (Action<NetworkConnection, NetworkError>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<NetworkConnection, NetworkError> action = this.m_OnServerErrorHolder;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerErrorHolder, (Action<NetworkConnection, NetworkError>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
		}
	}

	private Action<NetworkConnection> m_OnClientConnectHolder;
	public event Action<NetworkConnection> m_OnClientConnect
	{
		add
		{
			Action<NetworkConnection> action = this.m_OnClientConnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClientConnectHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnClientConnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClientConnectHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	private Action<NetworkConnection> m_OnClientDisconnectHolder;
	public event Action<NetworkConnection> m_OnClientDisconnect
	{
		add
		{
			Action<NetworkConnection> action = this.m_OnClientDisconnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClientDisconnectHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnClientDisconnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClientDisconnectHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	private Action<NetworkConnection> m_OnClientNotReadyHolder;
	public event Action<NetworkConnection> m_OnClientNotReady
	{
		add
		{
			Action<NetworkConnection> action = this.m_OnClientNotReadyHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClientNotReadyHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnClientNotReadyHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClientNotReadyHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	private Action<NetworkConnection, NetworkError> m_OnClientErrorHolder;
	public event Action<NetworkConnection, NetworkError> m_OnClientError
	{
		add
		{
			Action<NetworkConnection, NetworkError> action = this.m_OnClientErrorHolder;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClientErrorHolder, (Action<NetworkConnection, NetworkError>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<NetworkConnection, NetworkError> action = this.m_OnClientErrorHolder;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClientErrorHolder, (Action<NetworkConnection, NetworkError>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	public MyNetworkManager()
	{
		this.m_OnServerStartHolder = delegate
		{
		};
		
		this.m_OnServerConnectHolder = delegate
			{
			};
		
		this.m_OnServerDisconnectHolder = delegate
			{
			};
		
		this.m_OnServerReadyHolder = delegate
			{
			};
		
		this.m_OnServerAddPlayerHolder = delegate
			{
			};
		
		this.m_OnServerRemovePlayerHolder = delegate
			{
			};
		
		this.m_OnServerErrorHolder = delegate
			{
			};
		
		this.m_OnClientConnectHolder = delegate
			{
			};
		this.m_OnClientDisconnectHolder = delegate
		{
		};
		
		this.m_OnClientNotReadyHolder = delegate
			{
			};
		
		this.m_OnClientErrorHolder = delegate
			{
			};
		m_nextPlayerChannelIndex = 6;
		
	}

	internal static MyNetworkManager Get()
	{
		return (MyNetworkManager)NetworkManager.singleton;
	}

	private void Start()
	{
		base.connectionConfig.PacketSize = 1200;
		base.connectionConfig.NetworkDropThreshold = 50;
		base.connectionConfig.OverflowDropThreshold = 50;
		base.connectionConfig.IsAcksLong = true;
		base.connectionConfig.MaxSentMessageQueueSize = 1024;
		base.connectionConfig.PingTimeout = (uint)HydrogenConfig.Get().HeartbeatPeriod.TotalMilliseconds;
		base.connectionConfig.DisconnectTimeout = (uint)HydrogenConfig.Get().HeartbeatTimeout.TotalMilliseconds;
		base.logLevel = LogFilter.FilterLevel.Info;
		if (36 >= base.connectionConfig.ChannelCount)
		{
			return;
		}
		while (true)
		{
			Log.Error("BootstrapSingletons.prefab>My Network Manager>QoS Channels only has {0} channels but our script requires {1}", base.connectionConfig.ChannelCount, NetworkChannelId.Count);
			return;
		}
	}

	internal NetworkClient MyStartClient(string gameServerAddress, string userHandle)
	{
		matchInfo = null;
		Uri uri = new Uri(gameServerAddress);
		NetworkManager.singleton.networkAddress = uri.Host;
		NetworkManager.singleton.networkPort = uri.Port;
		int useWebSockets;
		if (!(uri.Scheme == "ws"))
		{
			useWebSockets = ((uri.Scheme == "wss") ? 1 : 0);
		}
		else
		{
			useWebSockets = 1;
		}
		base.useWebSockets = ((byte)useWebSockets != 0);
		bool useSSL = uri.Scheme == "wss";
		NetworkTransport.Init(base.globalConfig);
		if (base.useWebSockets)
		{
			MyNetworkClient myNetworkClient = new MyNetworkClient();
			myNetworkClient.UserHandle = userHandle;
			myNetworkClient.UseSSL = useSSL;
			myNetworkClient.SetNetworkConnectionClass<MyNetworkClientConnection>();
			base.client = myNetworkClient;
		}
		else
		{
			base.client = new NetworkClient();
			NetworkClient client = base.client;
			
			client.messageDispatcher = delegate(NetworkMessageDelegate networkMessageFunction, NetworkMessage data)
				{
					NetworkMessage mirror = default(NetworkMessage);
					if (AsyncPump.Current != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								mirror = new NetworkMessage();
								mirror.channelId = data.channelId;
								mirror.conn = data.conn;
								mirror.msgType = data.msgType;
								mirror.reader = data.reader;
								AsyncPump.Current.Post(delegate
								{
									networkMessageFunction(mirror);
								});
								return;
							}
						}
					}
					networkMessageFunction(data);
				};
		}
		ConfigureClient();
		UseExternalClient(base.client);
		if (matchInfo != null)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager StartClient match: " + matchInfo);
			}
			base.client.Connect(matchInfo);
		}
		else if (base.secureTunnelEndpoint != null)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager StartClient using provided SecureTunnel");
			}
			base.client.Connect(base.secureTunnelEndpoint);
		}
		else
		{
			if (string.IsNullOrEmpty(base.networkAddress))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("Must set the Network Address field in the manager");
				}
				return null;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager StartClient address:" + base.networkAddress + " port:" + base.networkPort);
			}
			if (base.useSimulator)
			{
				base.client.ConnectWithSimulator(base.networkAddress, base.networkPort, base.simulatedLatency, base.packetLossPercentage);
			}
			else
			{
				base.client.Connect(base.networkAddress, base.networkPort);
			}
		}
		if (base.migrationManager != null)
		{
			base.migrationManager.Initialize(base.client, matchInfo);
		}
		return base.client;
	}

	internal NetworkClient MyStartClientStub()
	{
		matchInfo = null;
		NetworkTransport.Init(base.globalConfig);
		MyNetworkClient myNetworkClient = new MyNetworkClient();
		myNetworkClient.SetNetworkConnectionClass<StubClientConnection>();
		client = myNetworkClient;
		ConfigureClient();
		UseExternalClient(client);
		client.Connect("localhost", 0);
		if (base.migrationManager != null)
		{
			base.migrationManager.Initialize(client, matchInfo);
		}
		return client;
	}

	private void ConfigureClient()
	{
		if (!base.customConfig)
		{
			return;
		}
		while (true)
		{
			base.connectionConfig.Channels.Clear();
			using (List<QosType>.Enumerator enumerator = base.channels.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QosType current = enumerator.Current;
					base.connectionConfig.AddChannel(current);
				}
			}
			base.connectionConfig.PingTimeout = (uint)HydrogenConfig.Get().HeartbeatPeriod.TotalMilliseconds;
			base.connectionConfig.DisconnectTimeout = (uint)HydrogenConfig.Get().HeartbeatTimeout.TotalMilliseconds;
			if (base.connectionConfig.UsePlatformSpecificProtocols && Application.platform != RuntimePlatform.PS4)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						throw new ArgumentOutOfRangeException("Platform specific protocols are not supported on this platform");
					}
				}
			}
			HostTopology hostTopology = new HostTopology(base.connectionConfig, 1);
			hostTopology.SentMessagePoolSize = 1024;
			hostTopology.ReceivedMessagePoolSize = 1024;
			client.Configure(hostTopology);
			return;
		}
	}

	internal void ConfigureServer()
	{
	}

	public override void OnStartServer()
	{
		m_nextPlayerChannelIndex = 6;
		if (this.m_OnServerStartHolder == null)
		{
			return;
		}
		while (true)
		{
			this.m_OnServerStartHolder();
			return;
		}
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		SetDefaultChannelOptions(conn);
		if (this.m_OnServerConnectHolder == null)
		{
			return;
		}
		while (true)
		{
			this.m_OnServerConnectHolder(conn);
			return;
		}
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		if (this.m_OnServerDisconnectHolder != null)
		{
			this.m_OnServerDisconnectHolder(conn);
		}
	}

	public override void OnServerReady(NetworkConnection conn)
	{
		if (this.m_OnServerReadyHolder == null)
		{
			return;
		}
		while (true)
		{
			this.m_OnServerReadyHolder(conn);
			return;
		}
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		if (this.m_OnServerAddPlayerHolder == null)
		{
			return;
		}
		while (true)
		{
			this.m_OnServerAddPlayerHolder(conn, playerControllerId);
			return;
		}
	}

	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
	{
		if (this.m_OnServerRemovePlayerHolder == null)
		{
			return;
		}
		while (true)
		{
			this.m_OnServerRemovePlayerHolder(conn, player);
			return;
		}
	}

	public override void OnServerError(NetworkConnection conn, int errorCode)
	{
		if (this.m_OnServerErrorHolder != null)
		{
			this.m_OnServerErrorHolder(conn, (NetworkError)errorCode);
		}
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		SetDefaultChannelOptions(conn);
		if (this.m_OnClientConnectHolder == null)
		{
			return;
		}
		while (true)
		{
			this.m_OnClientConnectHolder(conn);
			return;
		}
	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		if (this.m_OnClientDisconnectHolder == null)
		{
			return;
		}
		while (true)
		{
			this.m_OnClientDisconnectHolder(conn);
			return;
		}
	}

	public override void OnClientError(NetworkConnection conn, int errorCode)
	{
		if (this.m_OnClientErrorHolder == null)
		{
			return;
		}
		while (true)
		{
			this.m_OnClientErrorHolder(conn, (NetworkError)errorCode);
			return;
		}
	}

	public override void OnClientNotReady(NetworkConnection conn)
	{
		if (this.m_OnClientNotReadyHolder == null)
		{
			return;
		}
		while (true)
		{
			this.m_OnClientNotReadyHolder(conn);
			return;
		}
	}

	private void SetDefaultChannelOptions(NetworkConnection conn)
	{
		for (int i = 0; i < base.channels.Count; i++)
		{
			if (i != 1)
			{
				conn.SetChannelOption(i, ChannelOption.MaxPendingBuffers, 128);
			}
		}
		while (true)
		{
			return;
		}
	}

	internal int GetNextPlayerChannelIndex()
	{
		if (m_nextPlayerChannelIndex >= base.channels.Count)
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
			m_nextLocalConnectionId = base.maxConnections + 1;
		}
		return m_nextLocalConnectionId++;
	}
}
