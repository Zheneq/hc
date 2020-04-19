using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

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

	public WebSocketInterface(WebSocketMessageFactory factory)
	{
		this.m_overallConnectionTimer = new Stopwatch();
		this.m_reconnectDelayTimer = new Stopwatch();
		this.Logger = Log.LogInstance;
		this.MessageFactory = factory;
		this.ConnectionTimeout = 30f;
		this.HttpPostHandler = new Action<string, string, Action<string, string>>(this.HttpPost);
		this.m_heartbeatPeriod = TimeSpan.Zero;
		this.m_heartbeatTimeout = TimeSpan.Zero;
		this.m_isCompressed = false;
		this.m_isRaw = false;
		this.m_isBinary = true;
		this.m_maxMessageSize = 0;
		this.m_maxMessagesPerSecond = 0;
		this.m_maxSendBufferSize = 0x100000;
		this.m_maxWaitTime = TimeSpan.FromSeconds(10.0);
	}

	public float ConnectionTimeout { get; set; }

	public TimeSpan HeartbeatTimeout
	{
		get
		{
			return this.m_heartbeatTimeout;
		}
		set
		{
			this.m_heartbeatTimeout = value;
			if (this.m_webSocket != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.set_HeartbeatTimeout(TimeSpan)).MethodHandle;
				}
				this.m_webSocket.HeartbeatTimeout = value;
			}
		}
	}

	public TimeSpan HeartbeatPeriod
	{
		get
		{
			return this.m_heartbeatPeriod;
		}
		set
		{
			this.m_heartbeatPeriod = value;
			if (this.m_webSocket != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.set_HeartbeatPeriod(TimeSpan)).MethodHandle;
				}
				this.m_webSocket.HeartbeatPeriod = value;
			}
		}
	}

	public bool IsCompressed
	{
		get
		{
			return this.m_isCompressed;
		}
		set
		{
			this.m_isCompressed = value;
			if (this.m_webSocket != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.set_IsCompressed(bool)).MethodHandle;
				}
				this.m_webSocket.IsCompressed = value;
			}
		}
	}

	public bool IsRaw
	{
		get
		{
			return this.m_isRaw;
		}
		set
		{
			this.m_isRaw = value;
			if (this.m_webSocket != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.set_IsRaw(bool)).MethodHandle;
				}
				this.m_webSocket.IsRaw = value;
			}
		}
	}

	public bool IsBinary
	{
		get
		{
			return this.m_isBinary;
		}
		set
		{
			this.m_isBinary = value;
			if (this.m_webSocket != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.set_IsBinary(bool)).MethodHandle;
				}
				this.m_webSocket.IsBinary = value;
			}
		}
	}

	public TimeSpan MaxWaitTime
	{
		get
		{
			return this.m_maxWaitTime;
		}
		set
		{
			this.m_maxWaitTime = value;
			if (this.m_webSocket != null)
			{
				this.m_webSocket.MaxWaitTime = value;
			}
		}
	}

	public int MaxMessageSize
	{
		get
		{
			return this.m_maxMessageSize;
		}
		set
		{
			this.m_maxMessageSize = value;
			if (this.m_webSocket != null)
			{
				this.m_webSocket.MaxMessageSize = value;
			}
		}
	}

	public int MaxMessagesPerSecond
	{
		get
		{
			return this.m_maxMessagesPerSecond;
		}
		set
		{
			this.m_maxMessagesPerSecond = value;
			if (this.m_webSocket != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.set_MaxMessagesPerSecond(int)).MethodHandle;
				}
				this.m_webSocket.MaxMessagesPerSecond = value;
			}
		}
	}

	public int MaxSendBufferSize
	{
		get
		{
			return this.m_maxSendBufferSize;
		}
		set
		{
			this.m_maxSendBufferSize = value;
			if (this.m_webSocket != null)
			{
				this.m_webSocket.MaxSendBufferSize = value;
			}
		}
	}

	public WebSocket WebSocket
	{
		get
		{
			return this.m_webSocket;
		}
	}

	public WebSocket.SocketState State
	{
		get
		{
			WebSocket.SocketState result;
			if (this.m_webSocket != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.get_State()).MethodHandle;
				}
				result = this.m_webSocket.State;
			}
			else
			{
				result = WebSocket.SocketState.Unknown;
			}
			return result;
		}
	}

	public LogInstance Logger { get; set; }

	public WebSocketMessageFactory MessageFactory { get; private set; }

	public string ServerAddress
	{
		get
		{
			return this.m_serverAddress;
		}
	}

	public void InitializeSocket(string serverAddress, int defaultPort, string defaultPath)
	{
		if (serverAddress.IndexOf("://") == -1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.InitializeSocket(string, int, string)).MethodHandle;
			}
			serverAddress = "ws://" + serverAddress;
		}
		Uri uri = new Uri(serverAddress);
		UriBuilder uriBuilder = new UriBuilder();
		uriBuilder.Scheme = uri.Scheme;
		uriBuilder.Host = uri.Host;
		UriBuilder uriBuilder2 = uriBuilder;
		int port;
		if (uri.Port > 0)
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
			if (!uri.IsDefaultPort)
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
				port = uri.Port;
				goto IL_8E;
			}
		}
		port = defaultPort;
		IL_8E:
		uriBuilder2.Port = port;
		uriBuilder.Path = ((!(uri.AbsolutePath != "/")) ? ("/" + defaultPath) : uri.AbsolutePath);
		this.m_serverAddress = uriBuilder.ToString();
		this.m_webSocket = new WebSocket(this.MessageFactory, this.m_serverAddress);
		this.m_webSocket.HeartbeatPeriod = this.m_heartbeatPeriod;
		this.m_webSocket.HeartbeatTimeout = this.m_heartbeatTimeout;
		this.m_webSocket.IsCompressed = this.m_isCompressed;
		this.m_webSocket.IsRaw = this.m_isRaw;
		this.m_webSocket.IsBinary = this.m_isBinary;
		this.m_webSocket.MaxMessageSize = this.m_maxMessageSize;
		this.m_webSocket.MaxSendBufferSize = this.m_maxSendBufferSize;
		this.m_webSocket.MaxMessagesPerSecond = this.m_maxMessagesPerSecond;
		this.m_webSocket.MaxWaitTime = this.m_maxWaitTime;
		this.m_webSocket.OnMessage += this.HandleMessage;
		this.m_webSocket.IsAsync = false;
		this.SetMinSocketLogLevel(Log.Level.Nothing);
	}

	public void SetMinSocketLogLevel(Log.Level level)
	{
		if (this.m_webSocket != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.SetMinSocketLogLevel(Log.Level)).MethodHandle;
			}
			this.m_webSocket.SetMinSocketLogLevel(level);
		}
	}

	public virtual void Connect()
	{
		if (!this.m_overallConnectionTimer.IsRunning)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.Connect()).MethodHandle;
			}
			this.m_overallConnectionTimer.Start();
		}
		if (this.State == WebSocket.SocketState.Closed)
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
			this.Logger.Info("Connecting to {0}", new object[]
			{
				this.m_serverAddress
			});
			this.m_webSocket.Connect();
		}
	}

	public virtual void Disconnect()
	{
		this.m_overallConnectionTimer.Reset();
		if (this.m_webSocket != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.Disconnect()).MethodHandle;
			}
			this.m_webSocket.Close();
		}
	}

	public void Reconnect()
	{
		this.m_reconnectDelayTimer.Reset();
		this.m_reconnectDelayTimer.Start();
	}

	public virtual void Update()
	{
		if (this.m_reconnectDelayTimer.IsRunning)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.Update()).MethodHandle;
			}
			if (this.m_reconnectDelayTimer.Elapsed >= TimeSpan.FromSeconds(1.0))
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
				this.m_reconnectDelayTimer.Reset();
				this.Connect();
			}
		}
		if (this.m_overallConnectionTimer.IsRunning)
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
			if (this.m_overallConnectionTimer.Elapsed >= TimeSpan.FromSeconds((double)this.ConnectionTimeout) && this.m_webSocket != null)
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
				this.Logger.Error("Connection timed out to {0} (elapsed time: {1})", new object[]
				{
					this.m_webSocket.ConnectionAddress,
					this.m_overallConnectionTimer.Elapsed
				});
				this.m_webSocket.Close();
				this.m_webSocket = null;
			}
		}
		if (this.m_webSocket != null)
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
			this.m_webSocket.Update();
		}
	}

	protected virtual void HandleMessage(WebSocketMessage message)
	{
	}

	public bool SendMessage(WebSocketMessage message)
	{
		if (this.m_webSocket != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.SendMessage(WebSocketMessage)).MethodHandle;
			}
			if (this.m_webSocket.State == WebSocket.SocketState.Open)
			{
				this.m_webSocket.Send(message);
				return true;
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
		}
		return false;
	}

	protected bool SendRequestMessage<ResponseType, ResponseHandlerType>(WebSocketMessage request, Action<ResponseType> callback, WebSocketMessageDispatcher<ResponseHandlerType> dispatcher) where ResponseType : WebSocketResponseMessage, new() where ResponseHandlerType : class
	{
		if (callback != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.SendRequestMessage(WebSocketMessage, Action<ResponseType>, WebSocketMessageDispatcher<ResponseHandlerType>)).MethodHandle;
			}
			Action<ResponseHandlerType, ResponseType> messageDelegate = delegate(ResponseHandlerType r, ResponseType m)
			{
				callback(m);
			};
			dispatcher.RegisterMessageDelegate<ResponseType>(messageDelegate, request.RequestId);
		}
		if (!this.SendMessage(request))
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
			if (callback != null)
			{
				ResponseType responseType = Activator.CreateInstance<ResponseType>();
				responseType.Success = false;
				responseType.ErrorMessage = "Failed to send request";
				ResponseType obj = responseType;
				callback(obj);
				dispatcher.UnregisterMessageDelegate<ResponseType>(request.RequestId);
			}
			return false;
		}
		return true;
	}

	protected void SendHttpRequest<ResponseType>(string url, WebSocketMessage request, Action<ResponseType, Exception> callback) where ResponseType : WebSocketResponseMessage, new()
	{
		try
		{
			string arg = JsonConvert.SerializeObject(request);
			this.HttpPostHandler(url, arg, delegate(string messageJson, string error)
			{
				try
				{
					if (!error.IsNullOrEmpty())
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketInterface.<SendHttpRequest>c__AnonStorey1.<>m__0(string, string)).MethodHandle;
						}
						throw new Exception(error);
					}
					if (messageJson.IsNullOrEmpty())
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
						throw new Exception("Empty response from server");
					}
					ResponseType arg3 = JsonConvert.DeserializeObject<ResponseType>(messageJson);
					callback(arg3, null);
				}
				catch (Exception arg4)
				{
					callback((ResponseType)((object)null), arg4);
				}
			});
		}
		catch (Exception arg2)
		{
			callback((ResponseType)((object)null), arg2);
		}
	}

	public void HttpPost(string url, string postString, Action<string, string> callback)
	{
		try
		{
			UTF8Encoding utf8Encoding = new UTF8Encoding();
			byte[] postBytes = utf8Encoding.GetBytes(postString);
			HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
			httpRequest.Proxy = null;
			httpRequest.Method = "POST";
			httpRequest.ContentLength = (long)postBytes.Length;
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
