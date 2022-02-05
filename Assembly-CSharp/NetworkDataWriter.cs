// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// server-only, missing in reactor
#if SERVER
public class NetworkDataWriter
{
	private object m_lock;

	private TcpClient m_tcpClient;

	private NetworkStream m_networkStream;

	private LinkedList<string> m_pendingMessages;

	private bool m_lostData;

	private Encoding m_encoding;

	private DateTime m_lastConnectionAttempt;

	private IPAddress IPAddress;

	private string Host;

	private int Port;

	public NetworkDataWriter()
	{
		m_lock = new object();
		m_pendingMessages = new LinkedList<string>();
		m_lastConnectionAttempt = DateTime.MinValue;
		m_encoding = new UTF8Encoding();
		MaxQueuedMessages = 100;
		MinConnectionInterval = TimeSpan.FromSeconds(5.0);
		Name = "network listener";
	}

	public int DefaultPort { get; set; }

	public int MaxQueuedMessages { get; set; }

	public TimeSpan MinConnectionInterval { get; set; }

	public string Name { get; set; }

	public bool IsConnected
	{
		get
		{
			return m_tcpClient != null && m_tcpClient.Connected;
		}
	}

	public bool IsConnecting { get; private set; }

	public bool IsWriting { get; private set; }

	public string RemoteAddress
	{
		get
		{
			return string.Format("{0}:{1}", Host, Port);
		}
		set
		{
			Uri uri = new Uri("tcp://" + value);
			Host = uri.Host;
			IPAddress = NetUtil.GetIPv4Address(Host);
			Port = (uri.IsDefaultPort ? DefaultPort : uri.Port);
		}
	}

	public void Write(string message)
	{
        lock (m_lock)
		{
			if (IPAddress != null)
			{
				m_pendingMessages.AddLast(message);
				if (m_pendingMessages.Count > MaxQueuedMessages)
				{
					if (!m_lostData)
					{
						Log.Critical("Too much data queued for {0}; data has been lost", new object[]
						{
							Name
						});
						m_lostData = true;
					}
					while (m_pendingMessages.Count > MaxQueuedMessages)
					{
						m_pendingMessages.RemoveFirst();
					}
					if (IsConnected)
					{
						Disconnect();
					}
				}
				if (!IsConnected)
				{
					TryConnect();
				}
				else
				{
					TryWrite();
				}
			}
		}
	}

	public void Shutdown(TimeSpan timeout = default(TimeSpan))
	{
		if (m_pendingMessages.Count > 0)
		{
			Log.Info("Trying to send {0} remaining messages to {1}", new object[]
			{
				m_pendingMessages.Count,
				Name
			});
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			while (m_pendingMessages.Count > 0 && stopwatch.Elapsed < timeout)
			{
				if (!IsConnected)
				{
					TryConnect();
				}
				else
				{
					TryWrite();
				}
				Thread.Sleep(100);
			}
			if (m_pendingMessages.Count > 0)
			{
				Log.Critical("Timed out trying to send {0} remaining messages to {1}; data has been lost", new object[]
				{
					m_pendingMessages.Count,
					Name
				});
			}
			else
			{
				Log.Info("All remaining messages have been sent to {0}", new object[]
				{
					Name
				});
			}
			Disconnect();
		}
	}

	public void Disconnect()
	{
		if (IsConnected)
		{
			Log.Info("Disconnecting from {0} at {1}", new object[]
			{
				Name,
				RemoteAddress
			});
			m_tcpClient.Close();
		}
		m_tcpClient = null;
		m_networkStream = null;
		IsConnecting = false;
		IsWriting = false;
	}

	private void TryConnect()
	{
        lock (m_lock)
		{
			if (!IsConnected && !IsConnecting)
			{
				try
				{
					DateTime utcNow = DateTime.UtcNow;
					if (utcNow - m_lastConnectionAttempt >= MinConnectionInterval)
					{
						m_tcpClient = new TcpClient();
						m_networkStream = null;
						m_lastConnectionAttempt = utcNow;
						IsConnecting = true;
						m_tcpClient.BeginConnect(IPAddress, Port, new AsyncCallback(HandleConnect), null);
					}
				}
				catch (Exception ex)
				{
					Log.Warning("Failed to connect to {0} ({1})", new object[]
					{
						Name,
						ex.Message
					});
					Disconnect();
				}
			}
		}
	}

	private void HandleConnect(IAsyncResult ar)
	{
        lock (m_lock)
		{
			try
			{
				IsConnecting = false;
				m_tcpClient.EndConnect(ar);
				if (m_tcpClient.Connected)
				{
					m_lastConnectionAttempt = DateTime.MinValue;
					m_networkStream = m_tcpClient.GetStream();
					Log.Info("Connected to {0} at {1}", new object[]
					{
						Name,
						RemoteAddress
					});
					m_lostData = false;
					TryWrite();
				}
			}
			catch (Exception ex)
			{
				Log.Warning("Failed to connect to {0} ({1})", new object[]
				{
					Name,
					ex.Message
				});
				Disconnect();
			}
		}
	}

	private void TryWrite()
	{
        lock (m_lock)
		{
			if (IsConnected && !IsConnecting && !IsWriting && m_pendingMessages.Count != 0)
			{
				try
				{
					string s = m_pendingMessages.First<string>();
					byte[] bytes = m_encoding.GetBytes(s);
					IsWriting = true;
					m_networkStream.BeginWrite(bytes, 0, bytes.Length, new AsyncCallback(HandleWrite), m_networkStream);
				}
				catch (Exception ex)
				{
					Log.Warning("Disconnected from {0} ({1})", new object[]
					{
						Name,
						ex.Message
					});
					Disconnect();
				}
			}
		}
	}

	private void HandleWrite(IAsyncResult ar)
	{
		lock (m_lock)
		{
			try
			{
				NetworkStream networkStream = (NetworkStream)ar.AsyncState;
				if (networkStream.CanWrite)
				{
					networkStream.EndWrite(ar);
				}
				m_pendingMessages.RemoveFirst();
			}
			catch (Exception ex)
			{
				Log.Warning("Disconnected from {0} ({1})", new object[]
				{
					Name,
					ex.Message
				});
				Disconnect();
			}
			finally
			{
				IsWriting = false;
			}
			TryWrite();
		}
	}
}
#endif
