using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		this.m_OnServerStartHolder = delegate()
		{
		};
		
		this.m_OnServerConnectHolder = delegate(NetworkConnection A_0)
			{
			};
		
		this.m_OnServerDisconnectHolder = delegate(NetworkConnection A_0)
			{
			};
		
		this.m_OnServerReadyHolder = delegate(NetworkConnection A_0)
			{
			};
		
		this.m_OnServerAddPlayerHolder = delegate(NetworkConnection A_0, short A_1)
			{
			};
		
		this.m_OnServerRemovePlayerHolder = delegate(NetworkConnection A_0, PlayerController A_1)
			{
			};
		
		this.m_OnServerErrorHolder = delegate(NetworkConnection A_0, NetworkError A_1)
			{
			};
		
		this.m_OnClientConnectHolder = delegate(NetworkConnection A_0)
			{
			};
		this.m_OnClientDisconnectHolder = delegate(NetworkConnection A_0)
		{
		};
		
		this.m_OnClientNotReadyHolder = delegate(NetworkConnection A_0)
			{
			};
		
		this.m_OnClientErrorHolder = delegate(NetworkConnection A_0, NetworkError A_1)
			{
			};
		this.m_nextPlayerChannelIndex = 6;
		
	}

	internal static MyNetworkManager Get()
	{
		return (MyNetworkManager)NetworkManager.singleton;
	}

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
				action = Interlocked.CompareExchange<Action>(ref this.m_OnServerStartHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action action = this.m_OnServerStartHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.m_OnServerStartHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnServerConnectHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnServerConnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnServerConnectHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnServerDisconnectHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnServerDisconnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnServerDisconnectHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
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
			Action<NetworkConnection, short> action = this.m_OnServerAddPlayerHolder;
			Action<NetworkConnection, short> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection, short>>(ref this.m_OnServerAddPlayerHolder, (Action<NetworkConnection, short>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection, short> action = this.m_OnServerAddPlayerHolder;
			Action<NetworkConnection, short> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection, short>>(ref this.m_OnServerAddPlayerHolder, (Action<NetworkConnection, short>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				action = Interlocked.CompareExchange<Action<NetworkConnection, PlayerController>>(ref this.m_OnServerRemovePlayerHolder, (Action<NetworkConnection, PlayerController>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection, PlayerController> action = this.m_OnServerRemovePlayerHolder;
			Action<NetworkConnection, PlayerController> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection, PlayerController>>(ref this.m_OnServerRemovePlayerHolder, (Action<NetworkConnection, PlayerController>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				action = Interlocked.CompareExchange<Action<NetworkConnection, NetworkError>>(ref this.m_OnServerErrorHolder, (Action<NetworkConnection, NetworkError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection, NetworkError> action = this.m_OnServerErrorHolder;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection, NetworkError>>(ref this.m_OnServerErrorHolder, (Action<NetworkConnection, NetworkError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnClientConnectHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnClientConnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnClientConnectHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnClientDisconnectHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnClientDisconnectHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnClientDisconnectHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnClientNotReadyHolder, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnClientNotReadyHolder;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnClientNotReadyHolder, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				action = Interlocked.CompareExchange<Action<NetworkConnection, NetworkError>>(ref this.m_OnClientErrorHolder, (Action<NetworkConnection, NetworkError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection, NetworkError> action = this.m_OnClientErrorHolder;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection, NetworkError>>(ref this.m_OnClientErrorHolder, (Action<NetworkConnection, NetworkError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	private void Start()
	{
		base.connectionConfig.PacketSize = 0x4B0;
		base.connectionConfig.NetworkDropThreshold = 0x32;
		base.connectionConfig.OverflowDropThreshold = 0x32;
		base.connectionConfig.IsAcksLong = true;
		base.connectionConfig.MaxSentMessageQueueSize = 0x400;
		base.connectionConfig.PingTimeout = (uint)HydrogenConfig.Get().HeartbeatPeriod.TotalMilliseconds;
		base.connectionConfig.DisconnectTimeout = (uint)HydrogenConfig.Get().HeartbeatTimeout.TotalMilliseconds;
		base.logLevel = LogFilter.FilterLevel.Info;
		if (0x24 < base.connectionConfig.ChannelCount)
		{
			Log.Error("BootstrapSingletons.prefab>My Network Manager>QoS Channels only has {0} channels but our script requires {1}", new object[]
			{
				base.connectionConfig.ChannelCount,
				NetworkChannelId.Count
			});
		}
	}

	internal NetworkClient MyStartClient(string gameServerAddress, string userHandle)
	{
		this.matchInfo = null;
		Uri uri = new Uri(gameServerAddress);
		NetworkManager.singleton.networkAddress = uri.Host;
		NetworkManager.singleton.networkPort = uri.Port;
		bool useWebSockets;
		if (!(uri.Scheme == "ws"))
		{
			useWebSockets = (uri.Scheme == "wss");
		}
		else
		{
			useWebSockets = true;
		}
		base.useWebSockets = useWebSockets;
		bool useSSL = uri.Scheme == "wss";
		NetworkTransport.Init(base.globalConfig);
		if (base.useWebSockets)
		{
			MyNetworkClient myNetworkClient = new MyNetworkClient();
			myNetworkClient.UserHandle = userHandle;
			myNetworkClient.UseSSL = useSSL;
			myNetworkClient.SetNetworkConnectionClass<MyNetworkClientConnection>();
			this.client = myNetworkClient;
		}
		else
		{
			this.client = new NetworkClient();
			NetworkClient client = this.client;
			
			client.messageDispatcher = delegate(NetworkMessageDelegate networkMessageFunction, NetworkMessage data)
				{
					if (AsyncPump.Current != null)
					{
						NetworkMessage mirror = new NetworkMessage();
						mirror.channelId = data.channelId;
						mirror.conn = data.conn;
						mirror.msgType = data.msgType;
						mirror.reader = data.reader;
						AsyncPump.Current.Post(delegate(object _)
						{
							networkMessageFunction(mirror);
						}, null, null);
					}
					else
					{
						networkMessageFunction(data);
					}
				};
		}
		this.ConfigureClient();
		base.UseExternalClient(this.client);
		if (this.matchInfo != null)
		{
			if (LogFilter.logDebug)
			{
				UnityEngine.Debug.Log("NetworkManager StartClient match: " + this.matchInfo);
			}
			this.client.Connect(this.matchInfo);
		}
		else if (base.secureTunnelEndpoint != null)
		{
			if (LogFilter.logDebug)
			{
				UnityEngine.Debug.Log("NetworkManager StartClient using provided SecureTunnel");
			}
			this.client.Connect(base.secureTunnelEndpoint);
		}
		else
		{
			if (string.IsNullOrEmpty(base.networkAddress))
			{
				if (LogFilter.logError)
				{
					UnityEngine.Debug.LogError("Must set the Network Address field in the manager");
				}
				return null;
			}
			if (LogFilter.logDebug)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"NetworkManager StartClient address:",
					base.networkAddress,
					" port:",
					base.networkPort
				}));
			}
			if (base.useSimulator)
			{
				this.client.ConnectWithSimulator(base.networkAddress, base.networkPort, base.simulatedLatency, base.packetLossPercentage);
			}
			else
			{
				this.client.Connect(base.networkAddress, base.networkPort);
			}
		}
		if (base.migrationManager != null)
		{
			base.migrationManager.Initialize(this.client, this.matchInfo);
		}
		return this.client;
	}

	internal NetworkClient MyStartClientStub()
	{
		this.matchInfo = null;
		NetworkTransport.Init(base.globalConfig);
		MyNetworkClient myNetworkClient = new MyNetworkClient();
		myNetworkClient.SetNetworkConnectionClass<StubClientConnection>();
		this.client = myNetworkClient;
		this.ConfigureClient();
		base.UseExternalClient(this.client);
		this.client.Connect("localhost", 0);
		if (base.migrationManager != null)
		{
			base.migrationManager.Initialize(this.client, this.matchInfo);
		}
		return this.client;
	}

	private void ConfigureClient()
	{
		if (base.customConfig)
		{
			base.connectionConfig.Channels.Clear();
			using (List<QosType>.Enumerator enumerator = base.channels.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QosType value = enumerator.Current;
					base.connectionConfig.AddChannel(value);
				}
			}
			base.connectionConfig.PingTimeout = (uint)HydrogenConfig.Get().HeartbeatPeriod.TotalMilliseconds;
			base.connectionConfig.DisconnectTimeout = (uint)HydrogenConfig.Get().HeartbeatTimeout.TotalMilliseconds;
			if (base.connectionConfig.UsePlatformSpecificProtocols && Application.platform != RuntimePlatform.PS4)
			{
				throw new ArgumentOutOfRangeException("Platform specific protocols are not supported on this platform");
			}
			HostTopology hostTopology = new HostTopology(base.connectionConfig, 1);
			hostTopology.SentMessagePoolSize = 0x400;
			hostTopology.ReceivedMessagePoolSize = 0x400;
			this.client.Configure(hostTopology);
		}
	}

	internal void ConfigureServer()
	{
	}

	public override void OnStartServer()
	{
		this.m_nextPlayerChannelIndex = 6;
		if (this.m_OnServerStartHolder != null)
		{
			this.m_OnServerStartHolder();
		}
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		this.SetDefaultChannelOptions(conn);
		if (this.m_OnServerConnectHolder != null)
		{
			this.m_OnServerConnectHolder(conn);
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
		if (this.m_OnServerReadyHolder != null)
		{
			this.m_OnServerReadyHolder(conn);
		}
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		if (this.m_OnServerAddPlayerHolder != null)
		{
			this.m_OnServerAddPlayerHolder(conn, playerControllerId);
		}
	}

	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
	{
		if (this.m_OnServerRemovePlayerHolder != null)
		{
			this.m_OnServerRemovePlayerHolder(conn, player);
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
		this.SetDefaultChannelOptions(conn);
		if (this.m_OnClientConnectHolder != null)
		{
			this.m_OnClientConnectHolder(conn);
		}
	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		if (this.m_OnClientDisconnectHolder != null)
		{
			this.m_OnClientDisconnectHolder(conn);
		}
	}

	public override void OnClientError(NetworkConnection conn, int errorCode)
	{
		if (this.m_OnClientErrorHolder != null)
		{
			this.m_OnClientErrorHolder(conn, (NetworkError)errorCode);
		}
	}

	public override void OnClientNotReady(NetworkConnection conn)
	{
		if (this.m_OnClientNotReadyHolder != null)
		{
			this.m_OnClientNotReadyHolder(conn);
		}
	}

	private void SetDefaultChannelOptions(NetworkConnection conn)
	{
		for (int i = 0; i < base.channels.Count; i++)
		{
			if (i != 1)
			{
				conn.SetChannelOption(i, ChannelOption.MaxPendingBuffers, 0x80);
			}
		}
	}

	internal int GetNextPlayerChannelIndex()
	{
		if (this.m_nextPlayerChannelIndex >= base.channels.Count)
		{
			Log.Error("Ran out of unique channels for players, server to client packets may be lost! Allocate more channels in MyNetworkManager.cs and BootstrapSingletons.prefab", new object[0]);
			this.m_nextPlayerChannelIndex = 6;
		}
		return this.m_nextPlayerChannelIndex++;
	}

	public int AllocateLocalConnectionId()
	{
		if (this.m_nextLocalConnectionId == 0)
		{
			this.m_nextLocalConnectionId = base.maxConnections + 1;
		}
		return this.m_nextLocalConnectionId++;
	}
}
