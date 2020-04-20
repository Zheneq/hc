using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using WebSocketSharp;

public class WebSocket : IDisposable
{
	private WebSocketSharp.WebSocket m_webSocket;

	private HttpSocket m_httpSocket;

	private object m_lock;

	private Stopwatch m_heartbeatSendTimer;

	private LeakyBucket m_messagesRateLimiter;

	private static TimeSpan m_messagesRateLimiterPeriod = TimeSpan.FromSeconds(5.0);

	private WebSocket(WebSocketMessageFactory factory)
	{
		
		this.OnMessage = delegate(WebSocketMessage A_0)
			{
			};
		
		this.State = global::WebSocket.SocketState.Closed;
		this.HeartbeatPeriod = TimeSpan.Zero;
		this.HeartbeatTimeout = TimeSpan.Zero;
		this.MessageFactory = factory;
		this.m_lock = new object();
		this.m_heartbeatSendTimer = new Stopwatch();
		this.m_messagesRateLimiter = new LeakyBucket(0.0, global::WebSocket.m_messagesRateLimiterPeriod);
	}

	public WebSocket(WebSocketMessageFactory factory, string address) : this(factory, new WebSocketSharp.WebSocket(address, new string[0]), address)
	{
	}

	public WebSocket(WebSocketMessageFactory factory, WebSocketSharp.WebSocket webSocket, string address) : this(factory)
	{
		if (address.IndexOf("://") == -1)
		{
			address = "ws://" + address;
		}
		this.ConnectionAddress = address;
		this.m_webSocket = webSocket;
		this.MaxWaitTime = TimeSpan.FromSeconds(10.0);
		this.MaxSendBufferSize = 0x100000;
		this.MaxMessageSize = 0;
		this.MaxMessagesPerSecond = 0;
		this.IsAsync = false;
		this.m_webSocket.OnOpen += this.HandleOpen;
		this.m_webSocket.OnClose += this.HandleClose;
		this.m_webSocket.OnError += this.HandleError;
		this.m_webSocket.OnMessage += this.HandleMessage;
		this.m_webSocket.EmitOnPing = true;
		if (this.m_webSocket.SslConfiguration != null)
		{
			this.m_webSocket.SslConfiguration.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(global::WebSocket.ValidateServerCertificate);
			this.m_webSocket.SslConfiguration.ClientCertificateSelectionCallback = null;
		}
	}

	public WebSocket(WebSocketMessageFactory factory, HttpSocket httpSocket) : this(factory)
	{
		this.ConnectionAddress = httpSocket.ConnectionAddress;
		this.ForwardedConnectionAddress = httpSocket.ForwardedConnectionAddress;
		this.m_httpSocket = httpSocket;
	}

	public event Action<WebSocketMessage> OnMessage
	{
		add
		{
			Action<WebSocketMessage> action = this.OnMessage;
			Action<WebSocketMessage> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<WebSocketMessage>>(ref this.OnMessage, (Action<WebSocketMessage>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<WebSocketMessage> action = this.OnMessage;
			Action<WebSocketMessage> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<WebSocketMessage>>(ref this.OnMessage, (Action<WebSocketMessage>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public global::WebSocket.SocketState State { get; private set; }

	public bool IsAsync
	{
		get
		{
			return this.m_webSocket.EnableAsyncIO;
		}
		set
		{
			this.m_webSocket.EnableAsyncIO = value;
		}
	}

	public bool IsCompressed
	{
		get
		{
			return this.m_webSocket.Compression == CompressionMethod.Deflate;
		}
		set
		{
			WebSocketSharp.WebSocket webSocket = this.m_webSocket;
			CompressionMethod compression;
			if (value)
			{
				compression = CompressionMethod.Deflate;
			}
			else
			{
				compression = CompressionMethod.None;
			}
			webSocket.Compression = compression;
		}
	}

	public TimeSpan MaxWaitTime
	{
		get
		{
			return this.m_webSocket.WaitTime;
		}
		set
		{
			this.m_webSocket.WaitTime = value;
		}
	}

	public int MaxSendBufferSize
	{
		get
		{
			return this.m_webSocket.MaxSendBufferSize;
		}
		set
		{
			this.m_webSocket.MaxSendBufferSize = value;
		}
	}

	public int MaxMessageSize { get; set; }

	public int MaxMessagesPerSecond
	{
		get
		{
			return (int)this.m_messagesRateLimiter.LeakRate.AmountPerSecond;
		}
		set
		{
			this.m_messagesRateLimiter.LeakRate = new Rate((double)value * global::WebSocket.m_messagesRateLimiterPeriod.TotalSeconds, global::WebSocket.m_messagesRateLimiterPeriod);
		}
	}

	public bool IsRaw { get; set; }

	public bool IsBinary { get; set; }

	public TimeSpan HeartbeatPeriod { get; set; }

	public TimeSpan HeartbeatTimeout { get; set; }

	public long RoundtripTime { get; set; }

	public bool IsOutbound { get; private set; }

	public string ConnectionAddress { get; private set; }

	public string ForwardedConnectionAddress { get; private set; }

	public string LastErrorMessage { get; private set; }

	public int LastMaxMesssageSize { get; private set; }

	public Type LastMaxMesssageType { get; private set; }

	public int LastMesssageSize { get; private set; }

	public Type LastMesssageType { get; private set; }

	public Uri Url
	{
		get
		{
			return this.m_webSocket.Url;
		}
	}

	public WebSocketMessageFactory MessageFactory { get; private set; }

	public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		Thread.Sleep(0x1F4);
		return true;
	}

	public void Accept()
	{
		object @lock = this.m_lock;
		lock (@lock)
		{
			this.State = global::WebSocket.SocketState.Accepting;
			this.IsOutbound = false;
		}
	}

	public void Connect()
	{
		object @lock = this.m_lock;
		lock (@lock)
		{
			this.State = global::WebSocket.SocketState.Connecting;
			this.m_webSocket.ConnectAsync();
			this.IsOutbound = true;
		}
	}

	public void Close()
	{
		CloseStatusCode statusCode = CloseStatusCode.Normal;
		string message = (!this.IsOutbound) ? "Server connection closed" : "Client connection closed";
		this.Close(statusCode, message);
	}

	public void Close(CloseStatusCode statusCode, string message)
	{
		if (message == null)
		{
			string text;
			if (this.IsOutbound)
			{
				text = "Client connection closed";
			}
			else
			{
				text = "Server connection closed";
			}
			message = text;
		}
		object @lock = this.m_lock;
		lock (@lock)
		{
			if (this.State == global::WebSocket.SocketState.Open)
			{
				this.State = global::WebSocket.SocketState.Closing;
				this.m_webSocket.CloseAsync(statusCode, message);
			}
			else if (this.State == global::WebSocket.SocketState.Connecting)
			{
				this.State = global::WebSocket.SocketState.Closing;
				this.m_webSocket.CloseAsync(statusCode, message);
				this.HandleClose(this, new CloseEventArgs(statusCode, message));
			}
		}
	}

	public void Dispose()
	{
		this.Close();
	}

	public void Update()
	{
		object @lock = this.m_lock;
		lock (@lock)
		{
			if (this.State == global::WebSocket.SocketState.Open)
			{
				if (this.HeartbeatPeriod != TimeSpan.Zero)
				{
					if (this.HeartbeatTimeout != TimeSpan.Zero)
					{
						if (this.m_webSocket.LastReceiveTime > this.HeartbeatTimeout && this.m_heartbeatSendTimer.IsRunning)
						{
							if (this.m_heartbeatSendTimer.Elapsed > this.HeartbeatPeriod)
							{
								WebSocketSharp.ErrorEventArgs args = new WebSocketSharp.ErrorEventArgs("Timed out waiting for a heartbeat response", new TimeoutException("Timed out waiting for a heartbeat response"));
								this.HandleError(this, args);
								this.Close(CloseStatusCode.PingTimeout, "Timed out waiting for a heartbeat response");
								goto IL_143;
							}
						}
					}
					if (this.m_webSocket.LastReceiveTime > this.HeartbeatPeriod && !this.m_heartbeatSendTimer.IsRunning)
					{
						this.m_webSocket.Ping(TimeSpan.Zero);
						this.m_heartbeatSendTimer.Start();
					}
				}
			}
			IL_143:;
		}
	}

	protected void HandleOpen(object sender, EventArgs args)
	{
		try
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				if (this.State != global::WebSocket.SocketState.Connecting && this.State != global::WebSocket.SocketState.Accepting)
				{
					this.m_webSocket.CloseAsync(CloseStatusCode.Normal);
					return;
				}
				this.State = global::WebSocket.SocketState.Open;
			}
			WebSocketMessage obj = new ConnectionOpenedNotification();
			this.OnMessage(obj);
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}

	protected void HandleClose(object sender, CloseEventArgs args)
	{
		try
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				if (this.State == global::WebSocket.SocketState.Closed)
				{
					return;
				}
				this.State = global::WebSocket.SocketState.Closed;
				if (this.m_heartbeatSendTimer.IsRunning)
				{
					this.m_heartbeatSendTimer.Reset();
				}
			}
			string text = args.Reason;
			if (text.IsNullOrEmpty() && !this.LastErrorMessage.IsNullOrEmpty())
			{
				text = this.LastErrorMessage;
			}
			WebSocketMessage obj = new ConnectionClosedNotification
			{
				Reason = text,
				Code = (CloseStatusCode)args.Code
			};
			this.OnMessage(obj);
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}

	protected void HandleError(object sender, WebSocketSharp.ErrorEventArgs args)
	{
		try
		{
			this.LastErrorMessage = args.Message;
			WebSocketMessage obj = new ConnectionErrorNotification
			{
				ErrorMessage = args.Message
			};
			this.OnMessage(obj);
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}

	protected void HandleMessage(object sender, MessageEventArgs args)
	{
		try
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				if (this.m_heartbeatSendTimer.IsRunning)
				{
					if (args.Type == Opcode.Pong)
					{
						this.RoundtripTime = this.m_heartbeatSendTimer.ElapsedMilliseconds;
					}
					this.m_heartbeatSendTimer.Reset();
				}
			}
			if (this.MaxMessageSize != 0 && args.RawData.Length > this.MaxMessageSize)
			{
				string message = string.Format("Message size {0}", args.RawData.Length);
				WebSocketSharp.ErrorEventArgs args2 = new WebSocketSharp.ErrorEventArgs(message, new Exception(message));
				this.HandleError(this, args2);
				this.Close(CloseStatusCode.TooBig, message);
			}
			else
			{
				if (this.MaxMessagesPerSecond != 0)
				{
					if (!this.m_messagesRateLimiter.TryAdd(1.0))
					{
						string message2 = string.Format("Message rate {0:F2}/sec", (this.m_messagesRateLimiter.CurrentPoints + 1.0) / global::WebSocket.m_messagesRateLimiterPeriod.TotalSeconds);
						WebSocketSharp.ErrorEventArgs args3 = new WebSocketSharp.ErrorEventArgs(message2, new Exception(message2));
						this.HandleError(this, args3);
						this.Close(CloseStatusCode.TooBig, message2);
						return;
					}
				}
				if (args.Type != Opcode.Ping && args.Type != Opcode.Pong)
				{
					if (!args.Data.IsNullOrEmpty())
					{
						if (args.Type == Opcode.Text)
						{
							WebSocketMessage obj;
							if (this.IsRaw)
							{
								obj = new TextMessageNotification
								{
									Data = args.Data
								};
							}
							else
							{
								obj = this.MessageFactory.DeserializeFromText(args.Data);
							}
							if (!this.IsOutbound)
							{
								this.IsBinary = false;
							}
							this.OnMessage(obj);
						}
						else if (args.Type == Opcode.Binary)
						{
							WebSocketMessage obj2;
							if (this.IsRaw)
							{
								obj2 = new BinaryMessageNotification
								{
									RawData = args.RawData
								};
							}
							else
							{
								obj2 = this.MessageFactory.DeserializeFromBytes(args.RawData);
							}
							if (!this.IsOutbound)
							{
								this.IsBinary = true;
							}
							this.OnMessage(obj2);
						}
					}
				}
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}

	public void HandleHttpRequest(object sender, EventArgs args)
	{
		if (this.m_httpSocket == null)
		{
			throw new ArgumentNullException();
		}
		try
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.State = global::WebSocket.SocketState.Open;
			}
			if (this.m_httpSocket.Message == null)
			{
				throw new Exception("Failed to read message from HTTP request");
			}
			this.OnMessage(this.m_httpSocket.Message);
			this.m_httpSocket.WaitForSend();
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}

	public void Send(WebSocketMessage message)
	{
		object @lock = this.m_lock;
		lock (@lock)
		{
			if (this.m_webSocket != null)
			{
				int lastMesssageSize;
				if (this.IsBinary)
				{
					byte[] array = this.MessageFactory.SerializeToBytes(message);
					lastMesssageSize = array.Length;
					this.m_webSocket.Send(array);
				}
				else
				{
					string text = this.MessageFactory.SerializeToText(message);
					lastMesssageSize = text.Length;
					this.m_webSocket.Send(text);
				}
				this.LastMesssageSize = lastMesssageSize;
				this.LastMesssageType = message.GetType();
				if (this.LastMesssageSize > this.LastMaxMesssageSize)
				{
					this.LastMaxMesssageSize = this.LastMesssageSize;
					this.LastMaxMesssageType = this.LastMesssageType;
				}
			}
			else if (this.m_httpSocket != null)
			{
				this.m_httpSocket.Send(message);
			}
		}
	}

	public void Send(string data)
	{
		object @lock = this.m_lock;
		lock (@lock)
		{
			if (this.m_webSocket != null)
			{
				this.m_webSocket.Send(data);
			}
			else if (this.m_httpSocket != null)
			{
				this.m_httpSocket.Send(data);
			}
		}
	}

	public void Send(byte[] data)
	{
		object @lock = this.m_lock;
		lock (@lock)
		{
			if (this.m_webSocket != null)
			{
				this.m_webSocket.Send(data);
			}
		}
	}

	public static void Broadcast(WebSocketMessage message, IEnumerable<global::WebSocket> webSockets)
	{
		if (webSockets.IsNullOrEmpty<global::WebSocket>())
		{
			return;
		}
		MemoryStream memoryStream = null;
		MemoryStream memoryStream2 = null;
		Dictionary<CompressionMethod, Stream> cache = null;
		Dictionary<CompressionMethod, Stream> cache2 = null;
		IEnumerator<global::WebSocket> enumerator = webSockets.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				global::WebSocket webSocket = enumerator.Current;
				if (webSocket != null)
				{
					object @lock = webSocket.m_lock;
					lock (@lock)
					{
						if (webSocket.State == global::WebSocket.SocketState.Open)
						{
							if (webSocket.IsBinary)
							{
								if (memoryStream == null)
								{
									memoryStream = new MemoryStream(webSocket.MessageFactory.SerializeToBytes(message));
									cache = new Dictionary<CompressionMethod, Stream>();
								}
								webSocket.m_webSocket.Send(Opcode.Binary, memoryStream, cache);
								continue;
							}
						}
						if (webSocket.State == global::WebSocket.SocketState.Open)
						{
							if (!webSocket.IsBinary)
							{
								if (memoryStream2 == null)
								{
									memoryStream2 = new MemoryStream(Encoding.UTF8.GetBytes(webSocket.MessageFactory.SerializeToText(message)));
									cache2 = new Dictionary<CompressionMethod, Stream>();
								}
								webSocket.m_webSocket.Send(Opcode.Text, memoryStream2, cache2);
							}
						}
					}
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
		}
	}

	public static void Broadcast(string data, IEnumerable<global::WebSocket> webSockets)
	{
		if (webSockets.IsNullOrEmpty<global::WebSocket>())
		{
			return;
		}
		MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
		Dictionary<CompressionMethod, Stream> cache = new Dictionary<CompressionMethod, Stream>();
		IEnumerator<global::WebSocket> enumerator = webSockets.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				global::WebSocket webSocket = enumerator.Current;
				if (webSocket != null)
				{
					object @lock = webSocket.m_lock;
					lock (@lock)
					{
						if (webSocket.State == global::WebSocket.SocketState.Open)
						{
							if (webSocket.IsBinary)
							{
								throw new Exception("Cannot broadcast text messages on a binary socket");
							}
							webSocket.m_webSocket.Send(Opcode.Text, stream, cache);
						}
					}
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
		}
	}

	public static void Broadcast(byte[] data, IEnumerable<global::WebSocket> webSockets)
	{
		if (webSockets.IsNullOrEmpty<global::WebSocket>())
		{
			return;
		}
		MemoryStream stream = new MemoryStream(data);
		Dictionary<CompressionMethod, Stream> cache = new Dictionary<CompressionMethod, Stream>();
		using (IEnumerator<global::WebSocket> enumerator = webSockets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				global::WebSocket webSocket = enumerator.Current;
				if (webSocket != null)
				{
					object @lock = webSocket.m_lock;
					lock (@lock)
					{
						if (webSocket.State == global::WebSocket.SocketState.Open)
						{
							if (!webSocket.IsBinary)
							{
								throw new Exception("Cannot broadcast binary messages on a text socket");
							}
							webSocket.m_webSocket.Send(Opcode.Binary, stream, cache);
						}
					}
				}
			}
		}
	}

	public void SetMinSocketLogLevel(Log.Level level)
	{
		LogLevel logLevel = (LogLevel)(-1);
		switch (level)
		{
		case Log.Level.Everything:
			logLevel = LogLevel.Trace;
			break;
		case Log.Level.symbol_001D:
			logLevel = LogLevel.Debug;
			break;
		case Log.Level.Info:
			logLevel = LogLevel.Info;
			break;
		case Log.Level.Warning:
			logLevel = LogLevel.Warn;
			break;
		case Log.Level.Error:
			logLevel = LogLevel.Error;
			break;
		case Log.Level.Notice:
			logLevel = LogLevel.Fatal;
			break;
		}
		if (logLevel != (LogLevel)(-1))
		{
			this.m_webSocket.Logger.Output = new Action<LogData, string>(this.HandleLogMessage);
			this.m_webSocket.Logger.Level = logLevel;
		}
		else
		{
			Logger logger = this.m_webSocket.Logger;
			
			logger.Output = delegate(LogData A_0, string A_1)
				{
				};
		}
	}

	public void HandleLogMessage(LogData data, string unused)
	{
		Log.Level level;
		switch (data.Level)
		{
		case LogLevel.Trace:
		case LogLevel.Debug:
		case LogLevel.Info:
			level = Log.Level.Info;
			break;
		case LogLevel.Warn:
			level = Log.Level.Warning;
			break;
		case LogLevel.Error:
		case LogLevel.Fatal:
			level = Log.Level.Error;
			break;
		default:
			return;
		}
		Log.Write(level, Log.Category.WebSocketClient, string.Empty, 0, data.Message, new object[0]);
	}

	public enum SocketState
	{
		Unknown,
		Accepting,
		Connecting,
		Open,
		Closing,
		Closed
	}
}
