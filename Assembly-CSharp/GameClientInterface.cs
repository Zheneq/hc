using System;
using System.Threading;
using WebSocketSharp;

public class GameClientInterface : WebSocketInterface
{
	protected bool m_registered;

	protected bool m_allowRelogin;

	protected string m_gameClientHandle;

	protected string m_gameServerAddress;

	protected WebSocketMessageDispatcher<GameClientInterface> m_messageDispatcher;

	public bool IsConnected => base.State == WebSocket.SocketState.Open;

	private Action OnConnectedHolder;
	public event Action OnConnected
	{
		add
		{
			Action action = this.OnConnectedHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnConnectedHolder, (Action)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action action = this.OnConnectedHolder;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnConnectedHolder, (Action)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	private Action<string, bool, CloseStatusCode> OnDisconnectedHolder;
	public event Action<string, bool, CloseStatusCode> OnDisconnected;

	private Action<string> OnConnectionErrorHolder;
	public event Action<string> OnConnectionError
	{
		add
		{
			Action<string> action = this.OnConnectionErrorHolder;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnConnectionErrorHolder, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<string> action = this.OnConnectionErrorHolder;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnConnectionErrorHolder, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	private Action<BinaryMessageNotification> OnMessageHolder;
	public event Action<BinaryMessageNotification> OnMessage
	{
		add
		{
			Action<BinaryMessageNotification> action = this.OnMessageHolder;
			Action<BinaryMessageNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnMessageHolder, (Action<BinaryMessageNotification>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
		remove
		{
			Action<BinaryMessageNotification> action = this.OnMessageHolder;
			Action<BinaryMessageNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnMessageHolder, (Action<BinaryMessageNotification>)Delegate.Remove(action2, value), action);
			}
			while ((object)action != action2);
			while (true)
			{
				return;
			}
		}
	}

	public GameClientInterface()
	{
		
		this.OnConnectedHolder = delegate
			{
			};
		
		this.OnDisconnectedHolder = delegate
			{
			};
		this.OnConnectionErrorHolder = delegate
		{
		};
		
		this.OnMessageHolder = delegate
			{
			};
		base._002Ector(new WebSocketMessageFactory());
		base.HeartbeatPeriod = TimeSpan.FromMinutes(1.0);
		base.HeartbeatTimeout = TimeSpan.FromMinutes(5.0);
		base.IsRaw = true;
		base.MaxMessagesPerSecond = 0;
		m_registered = false;
		base.ConnectionTimeout = 30f;
	}

	public void Initialize(string gameServerAddress, string gameClientHandle)
	{
		m_gameClientHandle = gameClientHandle;
		m_gameServerAddress = gameServerAddress;
		m_messageDispatcher = new WebSocketMessageDispatcher<GameClientInterface>();
		m_messageDispatcher.RegisterMessageDelegate<ConnectionOpenedNotification>(HandleConnectionOpenedNotification);
		m_messageDispatcher.RegisterMessageDelegate<ConnectionClosedNotification>(HandleConnectionClosedNotification);
		m_messageDispatcher.RegisterMessageDelegate<ConnectionErrorNotification>(HandleConnectionErrorNotification);
		m_messageDispatcher.RegisterMessageDelegate<BinaryMessageNotification>(HandleBinaryMessageNotification);
	}

	public override void Connect()
	{
		m_registered = false;
		InitializeSocket(m_gameServerAddress, 6700, "GameClientInterface");
		base.Connect();
	}

	public override void Update()
	{
		base.Update();
	}

	private void HandleConnectionOpenedNotification(ConnectionOpenedNotification notification)
	{
		base.Logger.Info("Connected to game server {0}", m_serverAddress);
		m_overallConnectionTimer.Reset();
		m_registered = true;
		this.OnConnectedHolder();
	}

	private void HandleConnectionClosedNotification(ConnectionClosedNotification notification)
	{
		if (m_registered)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					base.Logger.Info("Disconnected from {0} ({1}) CloseStatusCode={2}", m_serverAddress, notification.Message.Trim(), notification.Code);
					m_allowRelogin = true;
					this.OnDisconnectedHolder(notification.Message, m_allowRelogin, notification.Code);
					return;
				}
			}
		}
		if (!m_overallConnectionTimer.IsRunning)
		{
			return;
		}
		while (true)
		{
			if (m_overallConnectionTimer.Elapsed.TotalSeconds < (double)base.ConnectionTimeout)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						base.Logger.Info("Retrying connection to game server {0}: {1} CloseStatusCode={2}", m_serverAddress, notification.Message.Trim(), notification.Code);
						Reconnect();
						return;
					}
				}
			}
			base.Logger.Info("Failed to connect to game server {0}: {1} CloseStatusCode={2}", m_serverAddress, notification.Message.Trim(), notification.Code);
			m_overallConnectionTimer.Reset();
			m_allowRelogin = false;
			this.OnDisconnectedHolder(notification.Message, m_allowRelogin, notification.Code);
			return;
		}
	}

	private void HandleConnectionErrorNotification(ConnectionErrorNotification notification)
	{
		this.OnConnectionErrorHolder(notification.ErrorMessage);
	}

	protected override void HandleMessage(WebSocketMessage message)
	{
		base.HandleMessage(message);
		m_messageDispatcher.HandleMessage(this, message);
	}

	private void HandleBinaryMessageNotification(BinaryMessageNotification notification)
	{
		this.OnMessageHolder(notification);
	}

	public bool SendMessage(byte[] bytes)
	{
		if (m_webSocket != null)
		{
			if (m_webSocket.State == WebSocket.SocketState.Open)
			{
				m_webSocket.Send(bytes);
				return true;
			}
		}
		return false;
	}
}
