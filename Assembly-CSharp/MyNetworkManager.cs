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
		this.m_OnServerStart = delegate()
		{
		};
		if (MyNetworkManager.<>f__am$cache2 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager..ctor()).MethodHandle;
			}
			MyNetworkManager.<>f__am$cache2 = delegate(NetworkConnection A_0)
			{
			};
		}
		this.m_OnServerConnect = MyNetworkManager.<>f__am$cache2;
		if (MyNetworkManager.<>f__am$cache3 == null)
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
			MyNetworkManager.<>f__am$cache3 = delegate(NetworkConnection A_0)
			{
			};
		}
		this.m_OnServerDisconnect = MyNetworkManager.<>f__am$cache3;
		if (MyNetworkManager.<>f__am$cache4 == null)
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
			MyNetworkManager.<>f__am$cache4 = delegate(NetworkConnection A_0)
			{
			};
		}
		this.m_OnServerReady = MyNetworkManager.<>f__am$cache4;
		if (MyNetworkManager.<>f__am$cache5 == null)
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
			MyNetworkManager.<>f__am$cache5 = delegate(NetworkConnection A_0, short A_1)
			{
			};
		}
		this.m_OnServerAddPlayer = MyNetworkManager.<>f__am$cache5;
		if (MyNetworkManager.<>f__am$cache6 == null)
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
			MyNetworkManager.<>f__am$cache6 = delegate(NetworkConnection A_0, PlayerController A_1)
			{
			};
		}
		this.m_OnServerRemovePlayer = MyNetworkManager.<>f__am$cache6;
		if (MyNetworkManager.<>f__am$cache7 == null)
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
			MyNetworkManager.<>f__am$cache7 = delegate(NetworkConnection A_0, NetworkError A_1)
			{
			};
		}
		this.m_OnServerError = MyNetworkManager.<>f__am$cache7;
		if (MyNetworkManager.<>f__am$cache8 == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			MyNetworkManager.<>f__am$cache8 = delegate(NetworkConnection A_0)
			{
			};
		}
		this.m_OnClientConnect = MyNetworkManager.<>f__am$cache8;
		this.m_OnClientDisconnect = delegate(NetworkConnection A_0)
		{
		};
		if (MyNetworkManager.<>f__am$cacheA == null)
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
			MyNetworkManager.<>f__am$cacheA = delegate(NetworkConnection A_0)
			{
			};
		}
		this.m_OnClientNotReady = MyNetworkManager.<>f__am$cacheA;
		if (MyNetworkManager.<>f__am$cacheB == null)
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
			MyNetworkManager.<>f__am$cacheB = delegate(NetworkConnection A_0, NetworkError A_1)
			{
			};
		}
		this.m_OnClientError = MyNetworkManager.<>f__am$cacheB;
		this.m_nextPlayerChannelIndex = 6;
		base..ctor();
	}

	internal static MyNetworkManager Get()
	{
		return (MyNetworkManager)NetworkManager.singleton;
	}

	public event Action m_OnServerStart
	{
		add
		{
			Action action = this.m_OnServerStart;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.m_OnServerStart, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.add_m_OnServerStart(Action)).MethodHandle;
			}
		}
		remove
		{
			Action action = this.m_OnServerStart;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.m_OnServerStart, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.remove_m_OnServerStart(Action)).MethodHandle;
			}
		}
	}

	public event Action<NetworkConnection> m_OnServerConnect
	{
		add
		{
			Action<NetworkConnection> action = this.m_OnServerConnect;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnServerConnect, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.add_m_OnServerConnect(Action<NetworkConnection>)).MethodHandle;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnServerConnect;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnServerConnect, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<NetworkConnection> m_OnServerDisconnect
	{
		add
		{
			Action<NetworkConnection> action = this.m_OnServerDisconnect;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnServerDisconnect, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.add_m_OnServerDisconnect(Action<NetworkConnection>)).MethodHandle;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnServerDisconnect;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnServerDisconnect, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.remove_m_OnServerDisconnect(Action<NetworkConnection>)).MethodHandle;
			}
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<NetworkConnection> m_OnServerReady;

	public event Action<NetworkConnection, short> m_OnServerAddPlayer
	{
		add
		{
			Action<NetworkConnection, short> action = this.m_OnServerAddPlayer;
			Action<NetworkConnection, short> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection, short>>(ref this.m_OnServerAddPlayer, (Action<NetworkConnection, short>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.add_m_OnServerAddPlayer(Action<NetworkConnection, short>)).MethodHandle;
			}
		}
		remove
		{
			Action<NetworkConnection, short> action = this.m_OnServerAddPlayer;
			Action<NetworkConnection, short> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection, short>>(ref this.m_OnServerAddPlayer, (Action<NetworkConnection, short>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.remove_m_OnServerAddPlayer(Action<NetworkConnection, short>)).MethodHandle;
			}
		}
	}

	public event Action<NetworkConnection, PlayerController> m_OnServerRemovePlayer
	{
		add
		{
			Action<NetworkConnection, PlayerController> action = this.m_OnServerRemovePlayer;
			Action<NetworkConnection, PlayerController> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection, PlayerController>>(ref this.m_OnServerRemovePlayer, (Action<NetworkConnection, PlayerController>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.add_m_OnServerRemovePlayer(Action<NetworkConnection, PlayerController>)).MethodHandle;
			}
		}
		remove
		{
			Action<NetworkConnection, PlayerController> action = this.m_OnServerRemovePlayer;
			Action<NetworkConnection, PlayerController> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection, PlayerController>>(ref this.m_OnServerRemovePlayer, (Action<NetworkConnection, PlayerController>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<NetworkConnection, NetworkError> m_OnServerError
	{
		add
		{
			Action<NetworkConnection, NetworkError> action = this.m_OnServerError;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection, NetworkError>>(ref this.m_OnServerError, (Action<NetworkConnection, NetworkError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.add_m_OnServerError(Action<NetworkConnection, NetworkError>)).MethodHandle;
			}
		}
		remove
		{
			Action<NetworkConnection, NetworkError> action = this.m_OnServerError;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection, NetworkError>>(ref this.m_OnServerError, (Action<NetworkConnection, NetworkError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public event Action<NetworkConnection> m_OnClientConnect
	{
		add
		{
			Action<NetworkConnection> action = this.m_OnClientConnect;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnClientConnect, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.add_m_OnClientConnect(Action<NetworkConnection>)).MethodHandle;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnClientConnect;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnClientConnect, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.remove_m_OnClientConnect(Action<NetworkConnection>)).MethodHandle;
			}
		}
	}

	public event Action<NetworkConnection> m_OnClientDisconnect
	{
		add
		{
			Action<NetworkConnection> action = this.m_OnClientDisconnect;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnClientDisconnect, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.add_m_OnClientDisconnect(Action<NetworkConnection>)).MethodHandle;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnClientDisconnect;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnClientDisconnect, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.remove_m_OnClientDisconnect(Action<NetworkConnection>)).MethodHandle;
			}
		}
	}

	public event Action<NetworkConnection> m_OnClientNotReady
	{
		add
		{
			Action<NetworkConnection> action = this.m_OnClientNotReady;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnClientNotReady, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.add_m_OnClientNotReady(Action<NetworkConnection>)).MethodHandle;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnClientNotReady;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection>>(ref this.m_OnClientNotReady, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.remove_m_OnClientNotReady(Action<NetworkConnection>)).MethodHandle;
			}
		}
	}

	public event Action<NetworkConnection, NetworkError> m_OnClientError
	{
		add
		{
			Action<NetworkConnection, NetworkError> action = this.m_OnClientError;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection, NetworkError>>(ref this.m_OnClientError, (Action<NetworkConnection, NetworkError>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<NetworkConnection, NetworkError> action = this.m_OnClientError;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<NetworkConnection, NetworkError>>(ref this.m_OnClientError, (Action<NetworkConnection, NetworkError>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.remove_m_OnClientError(Action<NetworkConnection, NetworkError>)).MethodHandle;
			}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.Start()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.MyStartClient(string, string)).MethodHandle;
			}
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
			if (MyNetworkManager.<>f__am$cache0 == null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				MyNetworkManager.<>f__am$cache0 = delegate(NetworkMessageDelegate networkMessageFunction, NetworkMessage data)
				{
					if (AsyncPump.Current != null)
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
							RuntimeMethodHandle runtimeMethodHandle2 = methodof(MyNetworkManager.<MyStartClient>m__0(NetworkMessageDelegate, NetworkMessage)).MethodHandle;
						}
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
			client.messageDispatcher = MyNetworkManager.<>f__am$cache0;
		}
		this.ConfigureClient();
		base.UseExternalClient(this.client);
		if (this.matchInfo != null)
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
			if (LogFilter.logDebug)
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
				UnityEngine.Debug.Log("NetworkManager StartClient match: " + this.matchInfo);
			}
			this.client.Connect(this.matchInfo);
		}
		else if (base.secureTunnelEndpoint != null)
		{
			if (LogFilter.logDebug)
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
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.MyStartClientStub()).MethodHandle;
			}
			base.migrationManager.Initialize(this.client, this.matchInfo);
		}
		return this.client;
	}

	private void ConfigureClient()
	{
		if (base.customConfig)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.ConfigureClient()).MethodHandle;
			}
			base.connectionConfig.Channels.Clear();
			using (List<QosType>.Enumerator enumerator = base.channels.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QosType value = enumerator.Current;
					base.connectionConfig.AddChannel(value);
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			base.connectionConfig.PingTimeout = (uint)HydrogenConfig.Get().HeartbeatPeriod.TotalMilliseconds;
			base.connectionConfig.DisconnectTimeout = (uint)HydrogenConfig.Get().HeartbeatTimeout.TotalMilliseconds;
			if (base.connectionConfig.UsePlatformSpecificProtocols && Application.platform != RuntimePlatform.PS4)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
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
		if (this.m_OnServerStart != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.OnStartServer()).MethodHandle;
			}
			this.m_OnServerStart();
		}
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		this.SetDefaultChannelOptions(conn);
		if (this.m_OnServerConnect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.OnServerConnect(NetworkConnection)).MethodHandle;
			}
			this.m_OnServerConnect(conn);
		}
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		if (this.m_OnServerDisconnect != null)
		{
			this.m_OnServerDisconnect(conn);
		}
	}

	public override void OnServerReady(NetworkConnection conn)
	{
		if (this.m_OnServerReady != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.OnServerReady(NetworkConnection)).MethodHandle;
			}
			this.m_OnServerReady(conn);
		}
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		if (this.m_OnServerAddPlayer != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.OnServerAddPlayer(NetworkConnection, short)).MethodHandle;
			}
			this.m_OnServerAddPlayer(conn, playerControllerId);
		}
	}

	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
	{
		if (this.m_OnServerRemovePlayer != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.OnServerRemovePlayer(NetworkConnection, PlayerController)).MethodHandle;
			}
			this.m_OnServerRemovePlayer(conn, player);
		}
	}

	public override void OnServerError(NetworkConnection conn, int errorCode)
	{
		if (this.m_OnServerError != null)
		{
			this.m_OnServerError(conn, (NetworkError)errorCode);
		}
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		this.SetDefaultChannelOptions(conn);
		if (this.m_OnClientConnect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.OnClientConnect(NetworkConnection)).MethodHandle;
			}
			this.m_OnClientConnect(conn);
		}
	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		if (this.m_OnClientDisconnect != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.OnClientDisconnect(NetworkConnection)).MethodHandle;
			}
			this.m_OnClientDisconnect(conn);
		}
	}

	public override void OnClientError(NetworkConnection conn, int errorCode)
	{
		if (this.m_OnClientError != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.OnClientError(NetworkConnection, int)).MethodHandle;
			}
			this.m_OnClientError(conn, (NetworkError)errorCode);
		}
	}

	public override void OnClientNotReady(NetworkConnection conn)
	{
		if (this.m_OnClientNotReady != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.OnClientNotReady(NetworkConnection)).MethodHandle;
			}
			this.m_OnClientNotReady(conn);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.SetDefaultChannelOptions(NetworkConnection)).MethodHandle;
		}
	}

	internal int GetNextPlayerChannelIndex()
	{
		if (this.m_nextPlayerChannelIndex >= base.channels.Count)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.GetNextPlayerChannelIndex()).MethodHandle;
			}
			Log.Error("Ran out of unique channels for players, server to client packets may be lost! Allocate more channels in MyNetworkManager.cs and BootstrapSingletons.prefab", new object[0]);
			this.m_nextPlayerChannelIndex = 6;
		}
		return this.m_nextPlayerChannelIndex++;
	}

	public int AllocateLocalConnectionId()
	{
		if (this.m_nextLocalConnectionId == 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkManager.AllocateLocalConnectionId()).MethodHandle;
			}
			this.m_nextLocalConnectionId = base.maxConnections + 1;
		}
		return this.m_nextLocalConnectionId++;
	}
}
