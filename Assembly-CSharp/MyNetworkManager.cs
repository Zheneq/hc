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

	public event Action m_OnServerStart
	{
		add
		{
			Action action = this.m_OnServerStart;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerStart, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		remove
		{
			Action action = this.m_OnServerStart;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerStart, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
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
				action = Interlocked.CompareExchange(ref this.m_OnServerConnect, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnServerConnect;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerConnect, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.m_OnServerDisconnect, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnServerDisconnect;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerDisconnect, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
	}

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
				action = Interlocked.CompareExchange(ref this.m_OnServerAddPlayer, (Action<NetworkConnection, short>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		remove
		{
			Action<NetworkConnection, short> action = this.m_OnServerAddPlayer;
			Action<NetworkConnection, short> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerAddPlayer, (Action<NetworkConnection, short>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
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
				action = Interlocked.CompareExchange(ref this.m_OnServerRemovePlayer, (Action<NetworkConnection, PlayerController>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		remove
		{
			Action<NetworkConnection, PlayerController> action = this.m_OnServerRemovePlayer;
			Action<NetworkConnection, PlayerController> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerRemovePlayer, (Action<NetworkConnection, PlayerController>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.m_OnServerError, (Action<NetworkConnection, NetworkError>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		remove
		{
			Action<NetworkConnection, NetworkError> action = this.m_OnServerError;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnServerError, (Action<NetworkConnection, NetworkError>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
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
				action = Interlocked.CompareExchange(ref this.m_OnClientConnect, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnClientConnect;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClientConnect, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
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
				action = Interlocked.CompareExchange(ref this.m_OnClientDisconnect, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnClientDisconnect;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClientDisconnect, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
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
				action = Interlocked.CompareExchange(ref this.m_OnClientNotReady, (Action<NetworkConnection>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		remove
		{
			Action<NetworkConnection> action = this.m_OnClientNotReady;
			Action<NetworkConnection> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClientNotReady, (Action<NetworkConnection>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
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
				action = Interlocked.CompareExchange(ref this.m_OnClientError, (Action<NetworkConnection, NetworkError>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
		}
		remove
		{
			Action<NetworkConnection, NetworkError> action = this.m_OnClientError;
			Action<NetworkConnection, NetworkError> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.m_OnClientError, (Action<NetworkConnection, NetworkError>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
	}

	public MyNetworkManager()
	{
		this.m_OnServerStart = delegate
		{
		};
		if (_003C_003Ef__am_0024cache2 == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			_003C_003Ef__am_0024cache2 = delegate
			{
			};
		}
		this.m_OnServerConnect = _003C_003Ef__am_0024cache2;
		if (_003C_003Ef__am_0024cache3 == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			_003C_003Ef__am_0024cache3 = delegate
			{
			};
		}
		this.m_OnServerDisconnect = _003C_003Ef__am_0024cache3;
		if (_003C_003Ef__am_0024cache4 == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			_003C_003Ef__am_0024cache4 = delegate
			{
			};
		}
		this.m_OnServerReady = _003C_003Ef__am_0024cache4;
		if (_003C_003Ef__am_0024cache5 == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			_003C_003Ef__am_0024cache5 = delegate
			{
			};
		}
		this.m_OnServerAddPlayer = _003C_003Ef__am_0024cache5;
		if (_003C_003Ef__am_0024cache6 == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			_003C_003Ef__am_0024cache6 = delegate
			{
			};
		}
		this.m_OnServerRemovePlayer = _003C_003Ef__am_0024cache6;
		if (_003C_003Ef__am_0024cache7 == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			_003C_003Ef__am_0024cache7 = delegate
			{
			};
		}
		this.m_OnServerError = _003C_003Ef__am_0024cache7;
		if (_003C_003Ef__am_0024cache8 == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			_003C_003Ef__am_0024cache8 = delegate
			{
			};
		}
		this.m_OnClientConnect = _003C_003Ef__am_0024cache8;
		this.m_OnClientDisconnect = delegate
		{
		};
		if (_003C_003Ef__am_0024cacheA == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			_003C_003Ef__am_0024cacheA = delegate
			{
			};
		}
		this.m_OnClientNotReady = _003C_003Ef__am_0024cacheA;
		if (_003C_003Ef__am_0024cacheB == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			_003C_003Ef__am_0024cacheB = delegate
			{
			};
		}
		this.m_OnClientError = _003C_003Ef__am_0024cacheB;
		m_nextPlayerChannelIndex = 6;
		base._002Ector();
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
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
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
			base.client = myNetworkClient;
		}
		else
		{
			base.client = new NetworkClient();
			NetworkClient client = base.client;
			if (_003C_003Ef__am_0024cache0 == null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				_003C_003Ef__am_0024cache0 = delegate(NetworkMessageDelegate networkMessageFunction, NetworkMessage data)
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
								if (1 == 0)
								{
									/*OpCode not supported: LdMemberToken*/;
								}
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
			client.messageDispatcher = _003C_003Ef__am_0024cache0;
		}
		ConfigureClient();
		UseExternalClient(base.client);
		if (matchInfo != null)
		{
			while (true)
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
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				Debug.Log("NetworkManager StartClient match: " + matchInfo);
			}
			base.client.Connect(matchInfo);
		}
		else if (base.secureTunnelEndpoint != null)
		{
			if (LogFilter.logDebug)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
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
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			base.connectionConfig.Channels.Clear();
			using (List<QosType>.Enumerator enumerator = base.channels.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QosType current = enumerator.Current;
					base.connectionConfig.AddChannel(current);
				}
				while (true)
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
		if (this.m_OnServerStart == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			this.m_OnServerStart();
			return;
		}
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		SetDefaultChannelOptions(conn);
		if (this.m_OnServerConnect == null)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			this.m_OnServerConnect(conn);
			return;
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
		if (this.m_OnServerReady == null)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			this.m_OnServerReady(conn);
			return;
		}
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		if (this.m_OnServerAddPlayer == null)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			this.m_OnServerAddPlayer(conn, playerControllerId);
			return;
		}
	}

	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
	{
		if (this.m_OnServerRemovePlayer == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			this.m_OnServerRemovePlayer(conn, player);
			return;
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
		SetDefaultChannelOptions(conn);
		if (this.m_OnClientConnect == null)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			this.m_OnClientConnect(conn);
			return;
		}
	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		if (this.m_OnClientDisconnect == null)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			this.m_OnClientDisconnect(conn);
			return;
		}
	}

	public override void OnClientError(NetworkConnection conn, int errorCode)
	{
		if (this.m_OnClientError == null)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			this.m_OnClientError(conn, (NetworkError)errorCode);
			return;
		}
	}

	public override void OnClientNotReady(NetworkConnection conn)
	{
		if (this.m_OnClientNotReady == null)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			this.m_OnClientNotReady(conn);
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
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	internal int GetNextPlayerChannelIndex()
	{
		if (m_nextPlayerChannelIndex >= base.channels.Count)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Log.Error("Ran out of unique channels for players, server to client packets may be lost! Allocate more channels in MyNetworkManager.cs and BootstrapSingletons.prefab");
			m_nextPlayerChannelIndex = 6;
		}
		return m_nextPlayerChannelIndex++;
	}

	public int AllocateLocalConnectionId()
	{
		if (m_nextLocalConnectionId == 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_nextLocalConnectionId = base.maxConnections + 1;
		}
		return m_nextLocalConnectionId++;
	}
}
