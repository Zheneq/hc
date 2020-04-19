using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NetworkStats
{
	private static NetworkStats s_instance;

	public float m_outputRate;

	public bool m_outputClient;

	public bool m_outputServer;

	private List<NetworkConnection> m_serverConnections;

	private List<NetworkConnection> m_clientConnections;

	private Dictionary<int, int> m_lastClientOutNumBytes;

	private Dictionary<int, int> m_lastClientOutNumMsgs;

	private Dictionary<int, int> m_lastServerOutNumBytes;

	private Dictionary<int, int> m_lastServerOutNumMsgs;

	private Dictionary<int, DateTime> m_clientConnectTime;

	private Dictionary<int, DateTime> m_serverConnectTime;

	private DateTime m_lastStatsTime;

	public static NetworkStats Get()
	{
		if (NetworkStats.s_instance == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NetworkStats.Get()).MethodHandle;
			}
			NetworkStats.s_instance = new NetworkStats();
			NetworkStats.s_instance.Start();
		}
		return NetworkStats.s_instance;
	}

	~NetworkStats()
	{
		NetworkStats.s_instance = null;
	}

	private void Start()
	{
		this.m_lastStatsTime = DateTime.UtcNow;
		this.m_serverConnections = new List<NetworkConnection>();
		this.m_clientConnections = new List<NetworkConnection>();
		this.m_lastClientOutNumBytes = new Dictionary<int, int>();
		this.m_lastClientOutNumMsgs = new Dictionary<int, int>();
		this.m_lastServerOutNumBytes = new Dictionary<int, int>();
		this.m_lastServerOutNumMsgs = new Dictionary<int, int>();
		this.m_clientConnectTime = new Dictionary<int, DateTime>();
		this.m_serverConnectTime = new Dictionary<int, DateTime>();
		MyNetworkManager.Get().m_OnServerConnect += this.OnServerConnect;
		MyNetworkManager.Get().m_OnServerDisconnect += this.OnServerDisconnect;
		MyNetworkManager.Get().m_OnClientConnect += this.OnClientConnect;
		MyNetworkManager.Get().m_OnClientDisconnect += this.OnClientDisconnect;
	}

	public void Update()
	{
		TimeSpan timeSpan = DateTime.UtcNow - this.m_lastStatsTime;
		if (this.m_outputRate != 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NetworkStats.Update()).MethodHandle;
			}
			if (timeSpan.TotalSeconds > (double)this.m_outputRate)
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
				if (this.m_outputClient)
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
					using (List<NetworkConnection>.Enumerator enumerator = this.m_clientConnections.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							NetworkConnection networkConnection = enumerator.Current;
							int num;
							int num2;
							int num3;
							int num4;
							networkConnection.GetStatsOut(out num, out num2, out num3, out num4);
							int num5 = 0;
							if (this.m_lastClientOutNumBytes.ContainsKey(networkConnection.connectionId))
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
								num5 = this.m_lastClientOutNumBytes[networkConnection.connectionId];
							}
							int num6 = 0;
							if (this.m_lastClientOutNumMsgs.ContainsKey(networkConnection.connectionId))
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
								num6 = this.m_lastClientOutNumMsgs[networkConnection.connectionId];
							}
							Log.Info("NS: Client Connection {0} - Over the last {1} ms - OutBytes: {2} (total: {3}) OutMsgs: {4} (total: {5}) OutBufferedMsgs: {6} (total: {7}) ", new object[]
							{
								networkConnection.connectionId,
								(int)timeSpan.TotalMilliseconds,
								num3 - num5,
								num3,
								num - num6,
								num,
								num4,
								num2
							});
							this.m_lastClientOutNumBytes[networkConnection.connectionId] = num3;
							this.m_lastClientOutNumMsgs[networkConnection.connectionId] = num;
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
				}
				if (this.m_outputServer)
				{
					using (List<NetworkConnection>.Enumerator enumerator2 = this.m_serverConnections.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							NetworkConnection networkConnection2 = enumerator2.Current;
							int num7;
							int num8;
							int num9;
							int num10;
							networkConnection2.GetStatsOut(out num7, out num8, out num9, out num10);
							int num11 = 0;
							if (this.m_lastServerOutNumBytes.ContainsKey(networkConnection2.connectionId))
							{
								num11 = this.m_lastServerOutNumBytes[networkConnection2.connectionId];
							}
							int num12 = 0;
							if (this.m_lastServerOutNumMsgs.ContainsKey(networkConnection2.connectionId))
							{
								num12 = this.m_lastServerOutNumMsgs[networkConnection2.connectionId];
							}
							Log.Info("NS: Server Connection {0} - Over the last {1} ms - OutBytes: {2} (total: {3}) OutMsgs: {4} (total: {5}) OutBufferedMsgs: {6} (total: {7}) ", new object[]
							{
								networkConnection2.connectionId,
								(int)timeSpan.TotalMilliseconds,
								num9 - num11,
								num9,
								num7 - num12,
								num7,
								num10,
								num8
							});
							this.m_lastServerOutNumBytes[networkConnection2.connectionId] = num9;
							this.m_lastServerOutNumMsgs[networkConnection2.connectionId] = num7;
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
					}
				}
				this.m_lastStatsTime = DateTime.UtcNow;
			}
		}
	}

	private void OnServerConnect(NetworkConnection conn)
	{
		this.m_serverConnections.Add(conn);
		Log.Info("NS: Added server connection {0}", new object[]
		{
			conn.connectionId
		});
		this.m_serverConnectTime[conn.connectionId] = DateTime.UtcNow;
	}

	private void OnServerDisconnect(NetworkConnection conn)
	{
		TimeSpan timeSpan = DateTime.UtcNow - this.m_serverConnectTime[conn.connectionId];
		int num;
		int num2;
		int num3;
		int num4;
		conn.GetStatsOut(out num, out num2, out num3, out num4);
		Log.Info("NS: Removed server connection {0} - OutNumMsgs: {1} OutNumBufferedMsgs: {2} OutNumBytes: {3} OutLastBufferedPerSecond: {4} SecondsConnected: {5} AverageBytesPerSecond: {6}", new object[]
		{
			conn.connectionId,
			num,
			num2,
			num3,
			num4,
			(int)timeSpan.TotalSeconds,
			(int)((double)num3 / timeSpan.TotalSeconds)
		});
		this.m_serverConnections.Remove(conn);
	}

	private void OnClientConnect(NetworkConnection conn)
	{
		this.m_clientConnections.Add(conn);
		Log.Info("NS: Added client connection {0}", new object[]
		{
			conn.connectionId
		});
		this.m_clientConnectTime[conn.connectionId] = DateTime.UtcNow;
	}

	private void OnClientDisconnect(NetworkConnection conn)
	{
		DateTime d;
		if (this.m_clientConnectTime.TryGetValue(conn.connectionId, out d))
		{
			TimeSpan timeSpan = DateTime.UtcNow - d;
			int num;
			int num2;
			int num3;
			int num4;
			conn.GetStatsOut(out num, out num2, out num3, out num4);
			Log.Info("NS: Removed client connection {0} - OutNumMsgs: {1} OutNumBufferedMsgs: {2} OutNumBytes: {3} OutLastBufferedPerSecond: {4} SecondsConnected: {5} AverageBytesPerSecond: {6}", new object[]
			{
				conn.connectionId,
				num,
				num2,
				num3,
				num4,
				(int)timeSpan.TotalSeconds,
				(int)((double)num3 / timeSpan.TotalSeconds)
			});
			this.m_clientConnections.Remove(conn);
		}
	}
}
