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
	public enum SocketState
	{
		Unknown,
		Accepting,
		Connecting,
		Open,
		Closing,
		Closed
	}

	private WebSocketSharp.WebSocket m_webSocket;

	private HttpSocket m_httpSocket;

	private object m_lock;

	private Stopwatch m_heartbeatSendTimer;

	private LeakyBucket m_messagesRateLimiter;

	private static TimeSpan m_messagesRateLimiterPeriod = TimeSpan.FromSeconds(5.0);

	public SocketState State
	{
		get;
		private set;
	}

	public bool IsAsync
	{
		get
		{
			return m_webSocket.EnableAsyncIO;
		}
		set
		{
			m_webSocket.EnableAsyncIO = value;
		}
	}

	public bool IsCompressed
	{
		get
		{
			return m_webSocket.Compression == CompressionMethod.Deflate;
		}
		set
		{
			WebSocketSharp.WebSocket webSocket = m_webSocket;
			int compression;
			if (value)
			{
				compression = 1;
			}
			else
			{
				compression = 0;
			}
			webSocket.Compression = (CompressionMethod)compression;
		}
	}

	public TimeSpan MaxWaitTime
	{
		get
		{
			return m_webSocket.WaitTime;
		}
		set
		{
			m_webSocket.WaitTime = value;
		}
	}

	public int MaxSendBufferSize
	{
		get
		{
			return m_webSocket.MaxSendBufferSize;
		}
		set
		{
			m_webSocket.MaxSendBufferSize = value;
		}
	}

	public int MaxMessageSize
	{
		get;
		set;
	}

	public int MaxMessagesPerSecond
	{
		get
		{
			return (int)m_messagesRateLimiter.LeakRate.AmountPerSecond;
		}
		set
		{
			m_messagesRateLimiter.LeakRate = new Rate((double)value * m_messagesRateLimiterPeriod.TotalSeconds, m_messagesRateLimiterPeriod);
		}
	}

	public bool IsRaw
	{
		get;
		set;
	}

	public bool IsBinary
	{
		get;
		set;
	}

	public TimeSpan HeartbeatPeriod
	{
		get;
		set;
	}

	public TimeSpan HeartbeatTimeout
	{
		get;
		set;
	}

	public long RoundtripTime
	{
		get;
		set;
	}

	public bool IsOutbound
	{
		get;
		private set;
	}

	public string ConnectionAddress
	{
		get;
		private set;
	}

	public string ForwardedConnectionAddress
	{
		get;
		private set;
	}

	public string LastErrorMessage
	{
		get;
		private set;
	}

	public int LastMaxMesssageSize
	{
		get;
		private set;
	}

	public Type LastMaxMesssageType
	{
		get;
		private set;
	}

	public int LastMesssageSize
	{
		get;
		private set;
	}

	public Type LastMesssageType
	{
		get;
		private set;
	}

	public Uri Url => m_webSocket.Url;

	public WebSocketMessageFactory MessageFactory
	{
		get;
		private set;
	}

	private Action<WebSocketMessage> OnMessageHolder;
	public event Action<WebSocketMessage> OnMessage
	{
		add
		{
			Action<WebSocketMessage> action = this.OnMessageHolder;
			Action<WebSocketMessage> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnMessageHolder, (Action<WebSocketMessage>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<WebSocketMessage> action = this.OnMessageHolder;
			Action<WebSocketMessage> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnMessageHolder, (Action<WebSocketMessage>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	private WebSocket(WebSocketMessageFactory factory)
	{
		
		this.OnMessageHolder = delegate
			{
			};
		
		State = SocketState.Closed;
		HeartbeatPeriod = TimeSpan.Zero;
		HeartbeatTimeout = TimeSpan.Zero;
		MessageFactory = factory;
		m_lock = new object();
		m_heartbeatSendTimer = new Stopwatch();
		m_messagesRateLimiter = new LeakyBucket(0.0, m_messagesRateLimiterPeriod);
	}

	public WebSocket(WebSocketMessageFactory factory, string address)
		: this(factory, new WebSocketSharp.WebSocket(address), address)
	{
	}

	public WebSocket(WebSocketMessageFactory factory, WebSocketSharp.WebSocket webSocket, string address)
		: this(factory)
	{
		if (address.IndexOf("://") == -1)
		{
			address = "ws://" + address;
		}
		ConnectionAddress = address;
		m_webSocket = webSocket;
		MaxWaitTime = TimeSpan.FromSeconds(10.0);
		MaxSendBufferSize = 1048576;
		MaxMessageSize = 0;
		MaxMessagesPerSecond = 0;
		IsAsync = false;
		m_webSocket.OnOpen += HandleOpen;
		m_webSocket.OnClose += HandleClose;
		m_webSocket.OnError += HandleError;
		m_webSocket.OnMessage += HandleMessage;
		m_webSocket.EmitOnPing = true;
		if (m_webSocket.SslConfiguration == null)
		{
			return;
		}
		while (true)
		{
			m_webSocket.SslConfiguration.ServerCertificateValidationCallback = ValidateServerCertificate;
			m_webSocket.SslConfiguration.ClientCertificateSelectionCallback = null;
			return;
		}
	}

	public WebSocket(WebSocketMessageFactory factory, HttpSocket httpSocket)
		: this(factory)
	{
		ConnectionAddress = httpSocket.ConnectionAddress;
		ForwardedConnectionAddress = httpSocket.ForwardedConnectionAddress;
		m_httpSocket = httpSocket;
	}

	public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		Thread.Sleep(500);
		return true;
	}

	public void Accept()
	{
		lock (m_lock)
		{
			State = SocketState.Accepting;
			IsOutbound = false;
		}
	}

	public void Connect()
	{
		lock (m_lock)
		{
			State = SocketState.Connecting;
			m_webSocket.ConnectAsync();
			IsOutbound = true;
		}
	}

	public void Close()
	{
		CloseStatusCode statusCode = CloseStatusCode.Normal;
		string message = (!IsOutbound) ? "Server connection closed" : "Client connection closed";
		Close(statusCode, message);
	}

	public void Close(CloseStatusCode statusCode, string message)
	{
		if (message == null)
		{
			object obj;
			if (IsOutbound)
			{
				obj = "Client connection closed";
			}
			else
			{
				obj = "Server connection closed";
			}
			message = (string)obj;
		}
		lock (m_lock)
		{
			if (State == SocketState.Open)
			{
				State = SocketState.Closing;
				m_webSocket.CloseAsync(statusCode, message);
			}
			else if (State == SocketState.Connecting)
			{
				State = SocketState.Closing;
				m_webSocket.CloseAsync(statusCode, message);
				HandleClose(this, new CloseEventArgs(statusCode, message));
			}
		}
	}

	public void Dispose()
	{
		Close();
	}

	public void Update()
	{
		lock (m_lock)
		{
			if (State == SocketState.Open)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (HeartbeatPeriod != TimeSpan.Zero)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									if (HeartbeatTimeout != TimeSpan.Zero)
									{
										if (m_webSocket.LastReceiveTime > HeartbeatTimeout && m_heartbeatSendTimer.IsRunning)
										{
											if (m_heartbeatSendTimer.Elapsed > HeartbeatPeriod)
											{
												WebSocketSharp.ErrorEventArgs args = new WebSocketSharp.ErrorEventArgs("Timed out waiting for a heartbeat response", new TimeoutException("Timed out waiting for a heartbeat response"));
												HandleError(this, args);
												Close(CloseStatusCode.PingTimeout, "Timed out waiting for a heartbeat response");
												return;
											}
										}
									}
									if (m_webSocket.LastReceiveTime > HeartbeatPeriod && !m_heartbeatSendTimer.IsRunning)
									{
										while (true)
										{
											switch (4)
											{
											case 0:
												break;
											default:
												m_webSocket.Ping(TimeSpan.Zero);
												m_heartbeatSendTimer.Start();
												return;
											}
										}
									}
									return;
								}
							}
						}
						return;
					}
				}
			}
		}
	}

	protected void HandleOpen(object sender, EventArgs args)
	{
		try
		{
			lock (m_lock)
			{
				if (State != SocketState.Connecting && State != SocketState.Accepting)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							m_webSocket.CloseAsync(CloseStatusCode.Normal);
							return;
						}
					}
				}
				State = SocketState.Open;
			}
			WebSocketMessage obj = new ConnectionOpenedNotification();
			this.OnMessageHolder(obj);
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
			lock (m_lock)
			{
				if (State == SocketState.Closed)
				{
					return;
				}
				State = SocketState.Closed;
				if (m_heartbeatSendTimer.IsRunning)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							m_heartbeatSendTimer.Reset();
							goto end_IL_000d;
						}
					}
				}
				end_IL_000d:;
			}
			string text = args.Reason;
			if (text.IsNullOrEmpty() && !LastErrorMessage.IsNullOrEmpty())
			{
				text = LastErrorMessage;
			}
			ConnectionClosedNotification connectionClosedNotification = new ConnectionClosedNotification();
			connectionClosedNotification.Reason = text;
			connectionClosedNotification.Code = (CloseStatusCode)args.Code;
			WebSocketMessage obj = connectionClosedNotification;
			this.OnMessageHolder(obj);
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
			LastErrorMessage = args.Message;
			ConnectionErrorNotification connectionErrorNotification = new ConnectionErrorNotification();
			connectionErrorNotification.ErrorMessage = args.Message;
			WebSocketMessage obj = connectionErrorNotification;
			this.OnMessageHolder(obj);
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
			lock (m_lock)
			{
				if (m_heartbeatSendTimer.IsRunning)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							if (args.Type == Opcode.Pong)
							{
								RoundtripTime = m_heartbeatSendTimer.ElapsedMilliseconds;
							}
							m_heartbeatSendTimer.Reset();
							goto end_IL_000d;
						}
					}
				}
				end_IL_000d:;
			}
			if (MaxMessageSize != 0 && args.RawData.Length > MaxMessageSize)
			{
				string message = $"Message size {args.RawData.Length}";
				WebSocketSharp.ErrorEventArgs args2 = new WebSocketSharp.ErrorEventArgs(message, new Exception(message));
				HandleError(this, args2);
				Close(CloseStatusCode.TooBig, message);
			}
			else
			{
				if (MaxMessagesPerSecond != 0)
				{
					if (!m_messagesRateLimiter.TryAdd())
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
							{
								string message2 = $"Message rate {(m_messagesRateLimiter.CurrentPoints + 1.0) / m_messagesRateLimiterPeriod.TotalSeconds:F2}/sec";
								WebSocketSharp.ErrorEventArgs args3 = new WebSocketSharp.ErrorEventArgs(message2, new Exception(message2));
								HandleError(this, args3);
								Close(CloseStatusCode.TooBig, message2);
								return;
							}
							}
						}
					}
				}
				if (args.Type != Opcode.Ping && args.Type != Opcode.Pong)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							if (!args.Data.IsNullOrEmpty())
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										if (args.Type == Opcode.Text)
										{
											WebSocketMessage obj;
											if (IsRaw)
											{
												TextMessageNotification textMessageNotification = new TextMessageNotification();
												textMessageNotification.Data = args.Data;
												obj = textMessageNotification;
											}
											else
											{
												obj = MessageFactory.DeserializeFromText(args.Data);
											}
											if (!IsOutbound)
											{
												IsBinary = false;
											}
											this.OnMessageHolder(obj);
										}
										else if (args.Type == Opcode.Binary)
										{
											while (true)
											{
												switch (2)
												{
												case 0:
													break;
												default:
												{
													WebSocketMessage obj2;
													if (IsRaw)
													{
														BinaryMessageNotification binaryMessageNotification = new BinaryMessageNotification();
														binaryMessageNotification.RawData = args.RawData;
														obj2 = binaryMessageNotification;
													}
													else
													{
														obj2 = MessageFactory.DeserializeFromBytes(args.RawData);
													}
													if (!IsOutbound)
													{
														IsBinary = true;
													}
													this.OnMessageHolder(obj2);
													return;
												}
												}
											}
										}
										return;
									}
								}
							}
							return;
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
		if (m_httpSocket == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					throw new ArgumentNullException();
				}
			}
		}
		try
		{
			lock (m_lock)
			{
				State = SocketState.Open;
			}
			if (m_httpSocket.Message == null)
			{
				throw new Exception("Failed to read message from HTTP request");
			}
			this.OnMessageHolder(m_httpSocket.Message);
			m_httpSocket.WaitForSend();
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}

	public void Send(WebSocketMessage message)
	{
		lock (m_lock)
		{
			if (m_webSocket != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						int num = 0;
						if (IsBinary)
						{
							byte[] array = MessageFactory.SerializeToBytes(message);
							num = array.Length;
							m_webSocket.Send(array);
						}
						else
						{
							string text = MessageFactory.SerializeToText(message);
							num = text.Length;
							m_webSocket.Send(text);
						}
						LastMesssageSize = num;
						LastMesssageType = message.GetType();
						if (LastMesssageSize > LastMaxMesssageSize)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									LastMaxMesssageSize = LastMesssageSize;
									LastMaxMesssageType = LastMesssageType;
									return;
								}
							}
						}
						return;
					}
					}
				}
			}
			if (m_httpSocket != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						m_httpSocket.Send(message);
						return;
					}
				}
			}
		}
	}

	public void Send(string data)
	{
		lock (m_lock)
		{
			if (m_webSocket != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						m_webSocket.Send(data);
						return;
					}
				}
			}
			if (m_httpSocket != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_httpSocket.Send(data);
						return;
					}
				}
			}
		}
	}

	public void Send(byte[] data)
	{
		lock (m_lock)
		{
			if (m_webSocket != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_webSocket.Send(data);
						return;
					}
				}
			}
		}
	}

	public static void Broadcast(WebSocketMessage message, IEnumerable<WebSocket> webSockets)
	{
		if (webSockets.IsNullOrEmpty())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		MemoryStream memoryStream = null;
		MemoryStream memoryStream2 = null;
		Dictionary<CompressionMethod, Stream> cache = null;
		Dictionary<CompressionMethod, Stream> cache2 = null;
		IEnumerator<WebSocket> enumerator = webSockets.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				WebSocket current = enumerator.Current;
				if (current != null)
				{
					lock (current.m_lock)
					{
						if (current.State == SocketState.Open)
						{
							if (current.IsBinary)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										if (memoryStream == null)
										{
											memoryStream = new MemoryStream(current.MessageFactory.SerializeToBytes(message));
											cache = new Dictionary<CompressionMethod, Stream>();
										}
										current.m_webSocket.Send(Opcode.Binary, memoryStream, cache);
										goto end_IL_005f;
									}
								}
							}
						}
						if (current.State == SocketState.Open)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									if (!current.IsBinary)
									{
										if (memoryStream2 == null)
										{
											memoryStream2 = new MemoryStream(Encoding.UTF8.GetBytes(current.MessageFactory.SerializeToText(message)));
											cache2 = new Dictionary<CompressionMethod, Stream>();
										}
										current.m_webSocket.Send(Opcode.Text, memoryStream2, cache2);
									}
									goto end_IL_005f;
								}
							}
						}
						end_IL_005f:;
					}
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_0141;
					}
				}
			}
			end_IL_0141:;
		}
	}

	public static void Broadcast(string data, IEnumerable<WebSocket> webSockets)
	{
		if (!webSockets.IsNullOrEmpty())
		{
			MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
			Dictionary<CompressionMethod, Stream> cache = new Dictionary<CompressionMethod, Stream>();
			IEnumerator<WebSocket> enumerator = webSockets.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					WebSocket current = enumerator.Current;
					if (current != null)
					{
						lock (current.m_lock)
						{
							if (current.State == SocketState.Open)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										if (current.IsBinary)
										{
											while (true)
											{
												switch (1)
												{
												case 0:
													break;
												default:
													throw new Exception("Cannot broadcast text messages on a binary socket");
												}
											}
										}
										current.m_webSocket.Send(Opcode.Text, stream, cache);
										goto end_IL_005c;
									}
								}
							}
							end_IL_005c:;
						}
					}
				}
			}
			finally
			{
				if (enumerator != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							enumerator.Dispose();
							goto end_IL_00b7;
						}
					}
				}
				end_IL_00b7:;
			}
		}
	}

	public static void Broadcast(byte[] data, IEnumerable<WebSocket> webSockets)
	{
		if (webSockets.IsNullOrEmpty())
		{
			return;
		}
		MemoryStream stream = new MemoryStream(data);
		Dictionary<CompressionMethod, Stream> cache = new Dictionary<CompressionMethod, Stream>();
		using (IEnumerator<WebSocket> enumerator = webSockets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				WebSocket current = enumerator.Current;
				if (current != null)
				{
					lock (current.m_lock)
					{
						if (current.State == SocketState.Open)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									if (!current.IsBinary)
									{
										while (true)
										{
											switch (3)
											{
											case 0:
												break;
											default:
												throw new Exception("Cannot broadcast binary messages on a text socket");
											}
										}
									}
									current.m_webSocket.Send(Opcode.Binary, stream, cache);
									goto end_IL_0054;
								}
							}
						}
						end_IL_0054:;
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
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
		case Log.Level.Debug:
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
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_webSocket.Logger.Output = HandleLogMessage;
					m_webSocket.Logger.Level = logLevel;
					return;
				}
			}
		}
		Logger logger = m_webSocket.Logger;
		
		logger.Output = delegate
			{
			};
	}

	public void HandleLogMessage(LogData data, string unused)
	{
		Log.Level level;
		switch (data.Level)
		{
		default:
			return;
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
		}
		Log.Write(level, Log.Category.WebSocketClient, string.Empty, 0, data.Message);
	}
}
