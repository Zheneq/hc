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
		if (s_instance == null)
		{
			s_instance = new NetworkStats();
			s_instance.Start();
		}
		return s_instance;
	}

	~NetworkStats()
	{
		s_instance = null;
	}

	private void Start()
	{
		m_lastStatsTime = DateTime.UtcNow;
		m_serverConnections = new List<NetworkConnection>();
		m_clientConnections = new List<NetworkConnection>();
		m_lastClientOutNumBytes = new Dictionary<int, int>();
		m_lastClientOutNumMsgs = new Dictionary<int, int>();
		m_lastServerOutNumBytes = new Dictionary<int, int>();
		m_lastServerOutNumMsgs = new Dictionary<int, int>();
		m_clientConnectTime = new Dictionary<int, DateTime>();
		m_serverConnectTime = new Dictionary<int, DateTime>();
		MyNetworkManager.Get().m_OnServerConnect += OnServerConnect;
		MyNetworkManager.Get().m_OnServerDisconnect += OnServerDisconnect;
		MyNetworkManager.Get().m_OnClientConnect += OnClientConnect;
		MyNetworkManager.Get().m_OnClientDisconnect += OnClientDisconnect;
	}

	public void Update()
	{
		TimeSpan timeSpan = DateTime.UtcNow - m_lastStatsTime;
		if (m_outputRate == 0f)
		{
			return;
		}
		while (true)
		{
			if (!(timeSpan.TotalSeconds > (double)m_outputRate))
			{
				return;
			}
			while (true)
			{
				if (m_outputClient)
				{
					using (List<NetworkConnection>.Enumerator enumerator = m_clientConnections.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							NetworkConnection current = enumerator.Current;
							current.GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond);
							int num = 0;
							if (m_lastClientOutNumBytes.ContainsKey(current.connectionId))
							{
								num = m_lastClientOutNumBytes[current.connectionId];
							}
							int num2 = 0;
							if (m_lastClientOutNumMsgs.ContainsKey(current.connectionId))
							{
								num2 = m_lastClientOutNumMsgs[current.connectionId];
							}
							Log.Info("NS: Client Connection {0} - Over the last {1} ms - OutBytes: {2} (total: {3}) OutMsgs: {4} (total: {5}) OutBufferedMsgs: {6} (total: {7}) ", current.connectionId, (int)timeSpan.TotalMilliseconds, numBytes - num, numBytes, numMsgs - num2, numMsgs, lastBufferedPerSecond, numBufferedMsgs);
							m_lastClientOutNumBytes[current.connectionId] = numBytes;
							m_lastClientOutNumMsgs[current.connectionId] = numMsgs;
						}
					}
				}
				if (m_outputServer)
				{
					using (List<NetworkConnection>.Enumerator enumerator2 = m_serverConnections.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							NetworkConnection current2 = enumerator2.Current;
							current2.GetStatsOut(out int numMsgs2, out int numBufferedMsgs2, out int numBytes2, out int lastBufferedPerSecond2);
							int num3 = 0;
							if (m_lastServerOutNumBytes.ContainsKey(current2.connectionId))
							{
								num3 = m_lastServerOutNumBytes[current2.connectionId];
							}
							int num4 = 0;
							if (m_lastServerOutNumMsgs.ContainsKey(current2.connectionId))
							{
								num4 = m_lastServerOutNumMsgs[current2.connectionId];
							}
							Log.Info("NS: Server Connection {0} - Over the last {1} ms - OutBytes: {2} (total: {3}) OutMsgs: {4} (total: {5}) OutBufferedMsgs: {6} (total: {7}) ", current2.connectionId, (int)timeSpan.TotalMilliseconds, numBytes2 - num3, numBytes2, numMsgs2 - num4, numMsgs2, lastBufferedPerSecond2, numBufferedMsgs2);
							m_lastServerOutNumBytes[current2.connectionId] = numBytes2;
							m_lastServerOutNumMsgs[current2.connectionId] = numMsgs2;
						}
					}
				}
				m_lastStatsTime = DateTime.UtcNow;
				return;
			}
		}
	}

	private void OnServerConnect(NetworkConnection conn)
	{
		m_serverConnections.Add(conn);
		Log.Info("NS: Added server connection {0}", conn.connectionId);
		m_serverConnectTime[conn.connectionId] = DateTime.UtcNow;
	}

	private void OnServerDisconnect(NetworkConnection conn)
	{
		TimeSpan timeSpan = DateTime.UtcNow - m_serverConnectTime[conn.connectionId];
		conn.GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond);
		Log.Info("NS: Removed server connection {0} - OutNumMsgs: {1} OutNumBufferedMsgs: {2} OutNumBytes: {3} OutLastBufferedPerSecond: {4} SecondsConnected: {5} AverageBytesPerSecond: {6}", conn.connectionId, numMsgs, numBufferedMsgs, numBytes, lastBufferedPerSecond, (int)timeSpan.TotalSeconds, (int)((double)numBytes / timeSpan.TotalSeconds));
		m_serverConnections.Remove(conn);
	}

	private void OnClientConnect(NetworkConnection conn)
	{
		m_clientConnections.Add(conn);
		Log.Info("NS: Added client connection {0}", conn.connectionId);
		m_clientConnectTime[conn.connectionId] = DateTime.UtcNow;
	}

	private void OnClientDisconnect(NetworkConnection conn)
	{
		if (m_clientConnectTime.TryGetValue(conn.connectionId, out DateTime value))
		{
			TimeSpan timeSpan = DateTime.UtcNow - value;
			conn.GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond);
			Log.Info("NS: Removed client connection {0} - OutNumMsgs: {1} OutNumBufferedMsgs: {2} OutNumBytes: {3} OutLastBufferedPerSecond: {4} SecondsConnected: {5} AverageBytesPerSecond: {6}", conn.connectionId, numMsgs, numBufferedMsgs, numBytes, lastBufferedPerSecond, (int)timeSpan.TotalSeconds, (int)((double)numBytes / timeSpan.TotalSeconds));
			m_clientConnections.Remove(conn);
		}
	}
}
