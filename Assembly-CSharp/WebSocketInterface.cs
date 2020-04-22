using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

public class WebSocketInterface
{
	public Action<string, string, Action<string, string>> HttpPostHandler;

	protected string m_serverAddress;

	protected WebSocket m_webSocket;

	protected Stopwatch m_overallConnectionTimer;

	protected Stopwatch m_reconnectDelayTimer;

	protected TimeSpan m_heartbeatPeriod;

	protected TimeSpan m_heartbeatTimeout;

	protected bool m_isCompressed;

	protected bool m_isRaw;

	protected bool m_isBinary;

	protected TimeSpan m_maxWaitTime;

	protected int m_maxMessageSize;

	protected int m_maxMessagesPerSecond;

	protected int m_maxSendBufferSize;

	public float ConnectionTimeout
	{
		get;
		set;
	}

	public TimeSpan HeartbeatTimeout
	{
		get
		{
			return m_heartbeatTimeout;
		}
		set
		{
			m_heartbeatTimeout = value;
			if (m_webSocket == null)
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
				m_webSocket.HeartbeatTimeout = value;
				return;
			}
		}
	}

	public TimeSpan HeartbeatPeriod
	{
		get
		{
			return m_heartbeatPeriod;
		}
		set
		{
			m_heartbeatPeriod = value;
			if (m_webSocket == null)
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
				m_webSocket.HeartbeatPeriod = value;
				return;
			}
		}
	}

	public bool IsCompressed
	{
		get
		{
			return m_isCompressed;
		}
		set
		{
			m_isCompressed = value;
			if (m_webSocket == null)
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
				m_webSocket.IsCompressed = value;
				return;
			}
		}
	}

	public bool IsRaw
	{
		get
		{
			return m_isRaw;
		}
		set
		{
			m_isRaw = value;
			if (m_webSocket == null)
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
				m_webSocket.IsRaw = value;
				return;
			}
		}
	}

	public bool IsBinary
	{
		get
		{
			return m_isBinary;
		}
		set
		{
			m_isBinary = value;
			if (m_webSocket == null)
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
				m_webSocket.IsBinary = value;
				return;
			}
		}
	}

	public TimeSpan MaxWaitTime
	{
		get
		{
			return m_maxWaitTime;
		}
		set
		{
			m_maxWaitTime = value;
			if (m_webSocket != null)
			{
				m_webSocket.MaxWaitTime = value;
			}
		}
	}

	public int MaxMessageSize
	{
		get
		{
			return m_maxMessageSize;
		}
		set
		{
			m_maxMessageSize = value;
			if (m_webSocket != null)
			{
				m_webSocket.MaxMessageSize = value;
			}
		}
	}

	public int MaxMessagesPerSecond
	{
		get
		{
			return m_maxMessagesPerSecond;
		}
		set
		{
			m_maxMessagesPerSecond = value;
			if (m_webSocket == null)
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
				m_webSocket.MaxMessagesPerSecond = value;
				return;
			}
		}
	}

	public int MaxSendBufferSize
	{
		get
		{
			return m_maxSendBufferSize;
		}
		set
		{
			m_maxSendBufferSize = value;
			if (m_webSocket != null)
			{
				m_webSocket.MaxSendBufferSize = value;
			}
		}
	}

	public WebSocket WebSocket => m_webSocket;

	public WebSocket.SocketState State
	{
		get
		{
			int result;
			if (m_webSocket != null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				result = (int)m_webSocket.State;
			}
			else
			{
				result = 0;
			}
			return (WebSocket.SocketState)result;
		}
	}

	public LogInstance Logger
	{
		get;
		set;
	}

	public WebSocketMessageFactory MessageFactory
	{
		get;
		private set;
	}

	public string ServerAddress => m_serverAddress;

	public WebSocketInterface(WebSocketMessageFactory factory)
	{
		m_overallConnectionTimer = new Stopwatch();
		m_reconnectDelayTimer = new Stopwatch();
		Logger = Log.LogInstance;
		MessageFactory = factory;
		ConnectionTimeout = 30f;
		HttpPostHandler = HttpPost;
		m_heartbeatPeriod = TimeSpan.Zero;
		m_heartbeatTimeout = TimeSpan.Zero;
		m_isCompressed = false;
		m_isRaw = false;
		m_isBinary = true;
		m_maxMessageSize = 0;
		m_maxMessagesPerSecond = 0;
		m_maxSendBufferSize = 1048576;
		m_maxWaitTime = TimeSpan.FromSeconds(10.0);
	}

	public void InitializeSocket(string serverAddress, int defaultPort, string defaultPath)
	{
		if (serverAddress.IndexOf("://") == -1)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			serverAddress = "ws://" + serverAddress;
		}
		Uri uri = new Uri(serverAddress);
		UriBuilder uriBuilder = new UriBuilder();
		uriBuilder.Scheme = uri.Scheme;
		uriBuilder.Host = uri.Host;
		int port;
		if (uri.Port > 0)
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
			if (!uri.IsDefaultPort)
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
				port = uri.Port;
				goto IL_008e;
			}
		}
		port = defaultPort;
		goto IL_008e;
		IL_008e:
		uriBuilder.Port = port;
		uriBuilder.Path = ((!(uri.AbsolutePath != "/")) ? ("/" + defaultPath) : uri.AbsolutePath);
		m_serverAddress = uriBuilder.ToString();
		m_webSocket = new WebSocket(MessageFactory, m_serverAddress);
		m_webSocket.HeartbeatPeriod = m_heartbeatPeriod;
		m_webSocket.HeartbeatTimeout = m_heartbeatTimeout;
		m_webSocket.IsCompressed = m_isCompressed;
		m_webSocket.IsRaw = m_isRaw;
		m_webSocket.IsBinary = m_isBinary;
		m_webSocket.MaxMessageSize = m_maxMessageSize;
		m_webSocket.MaxSendBufferSize = m_maxSendBufferSize;
		m_webSocket.MaxMessagesPerSecond = m_maxMessagesPerSecond;
		m_webSocket.MaxWaitTime = m_maxWaitTime;
		m_webSocket.OnMessage += HandleMessage;
		m_webSocket.IsAsync = false;
		SetMinSocketLogLevel(Log.Level.Nothing);
	}

	public void SetMinSocketLogLevel(Log.Level level)
	{
		if (m_webSocket == null)
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
			m_webSocket.SetMinSocketLogLevel(level);
			return;
		}
	}

	public virtual void Connect()
	{
		if (!m_overallConnectionTimer.IsRunning)
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
			m_overallConnectionTimer.Start();
		}
		if (State != WebSocket.SocketState.Closed)
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
			Logger.Info("Connecting to {0}", m_serverAddress);
			m_webSocket.Connect();
			return;
		}
	}

	public virtual void Disconnect()
	{
		m_overallConnectionTimer.Reset();
		if (m_webSocket == null)
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
			m_webSocket.Close();
			return;
		}
	}

	public void Reconnect()
	{
		m_reconnectDelayTimer.Reset();
		m_reconnectDelayTimer.Start();
	}

	public virtual void Update()
	{
		if (m_reconnectDelayTimer.IsRunning)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_reconnectDelayTimer.Elapsed >= TimeSpan.FromSeconds(1.0))
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
				m_reconnectDelayTimer.Reset();
				Connect();
			}
		}
		if (m_overallConnectionTimer.IsRunning)
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
			if (m_overallConnectionTimer.Elapsed >= TimeSpan.FromSeconds(ConnectionTimeout) && m_webSocket != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				Logger.Error("Connection timed out to {0} (elapsed time: {1})", m_webSocket.ConnectionAddress, m_overallConnectionTimer.Elapsed);
				m_webSocket.Close();
				m_webSocket = null;
			}
		}
		if (m_webSocket == null)
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
			m_webSocket.Update();
			return;
		}
	}

	protected virtual void HandleMessage(WebSocketMessage message)
	{
	}

	public bool SendMessage(WebSocketMessage message)
	{
		if (m_webSocket != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_webSocket.State == WebSocket.SocketState.Open)
			{
				m_webSocket.Send(message);
				return true;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	protected bool SendRequestMessage<ResponseType, ResponseHandlerType>(WebSocketMessage request, Action<ResponseType> callback, WebSocketMessageDispatcher<ResponseHandlerType> dispatcher) where ResponseType : WebSocketResponseMessage, new()where ResponseHandlerType : class
	{
		if (callback != null)
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
			Action<ResponseHandlerType, ResponseType> messageDelegate = delegate(ResponseHandlerType r, ResponseType m)
			{
				callback(m);
			};
			dispatcher.RegisterMessageDelegate(messageDelegate, request.RequestId);
		}
		if (!SendMessage(request))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (callback != null)
					{
						ResponseType val = new ResponseType();
						val.Success = false;
						val.ErrorMessage = "Failed to send request";
						ResponseType obj = val;
						callback(obj);
						dispatcher.UnregisterMessageDelegate<ResponseType>(request.RequestId);
					}
					return false;
				}
			}
		}
		return true;
	}

	protected void SendHttpRequest<ResponseType>(string url, WebSocketMessage request, Action<ResponseType, Exception> callback) where ResponseType : WebSocketResponseMessage, new()
	{
		try
		{
			string arg = JsonConvert.SerializeObject(request);
			HttpPostHandler(url, arg, delegate(string messageJson, string error)
			{
				try
				{
					if (!error.IsNullOrEmpty())
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								if (1 == 0)
								{
									/*OpCode not supported: LdMemberToken*/;
								}
								throw new Exception(error);
							}
						}
					}
					if (messageJson.IsNullOrEmpty())
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								throw new Exception("Empty response from server");
							}
						}
					}
					ResponseType arg3 = JsonConvert.DeserializeObject<ResponseType>(messageJson);
					callback(arg3, null);
				}
				catch (Exception arg4)
				{
					callback((ResponseType)null, arg4);
				}
			});
		}
		catch (Exception arg2)
		{
			callback((ResponseType)null, arg2);
		}
	}

	public void HttpPost(string url, string postString, Action<string, string> callback)
	{
		try
		{
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			byte[] postBytes = uTF8Encoding.GetBytes(postString);
			HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
			httpRequest.Proxy = null;
			httpRequest.Method = "POST";
			httpRequest.ContentLength = postBytes.Length;
			httpRequest.BeginGetRequestStream(delegate(IAsyncResult getRequestStreamAr)
			{
				try
				{
					Stream stream = httpRequest.EndGetRequestStream(getRequestStreamAr);
					stream.Write(postBytes, 0, postBytes.Length);
					stream.Flush();
					stream.Close();
					httpRequest.BeginGetResponse(delegate(IAsyncResult getResponseAr)
					{
						try
						{
							HttpWebResponse httpWebResponse = (HttpWebResponse)httpRequest.EndGetResponse(getResponseAr);
							string arg = new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd();
							callback(arg, null);
						}
						catch (Exception ex3)
						{
							callback(null, ex3.Message);
						}
					}, null);
				}
				catch (Exception ex2)
				{
					callback(null, ex2.Message);
				}
			}, null);
		}
		catch (Exception ex)
		{
			callback(null, ex.Message);
		}
	}
}
