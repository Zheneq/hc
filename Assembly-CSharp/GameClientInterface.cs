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

	public event Action OnConnected
	{
		add
		{
			Action action = this.OnConnected;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnConnected, (Action)Delegate.Combine(action2, value), action);
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
			Action action = this.OnConnected;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnConnected, (Action)Delegate.Remove(action2, value), action);
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
	}

	public event Action<string, bool, CloseStatusCode> OnDisconnected;

	public event Action<string> OnConnectionError
	{
		add
		{
			Action<string> action = this.OnConnectionError;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnConnectionError, (Action<string>)Delegate.Combine(action2, value), action);
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
			Action<string> action = this.OnConnectionError;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnConnectionError, (Action<string>)Delegate.Remove(action2, value), action);
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

	public event Action<BinaryMessageNotification> OnMessage
	{
		add
		{
			Action<BinaryMessageNotification> action = this.OnMessage;
			Action<BinaryMessageNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnMessage, (Action<BinaryMessageNotification>)Delegate.Combine(action2, value), action);
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
			Action<BinaryMessageNotification> action = this.OnMessage;
			Action<BinaryMessageNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnMessage, (Action<BinaryMessageNotification>)Delegate.Remove(action2, value), action);
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
	}

	public GameClientInterface()
	{
		if (_003C_003Ef__am_0024cache0 == null)
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
			_003C_003Ef__am_0024cache0 = delegate
			{
			};
		}
		this.OnConnected = _003C_003Ef__am_0024cache0;
		if (_003C_003Ef__am_0024cache1 == null)
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
			_003C_003Ef__am_0024cache1 = delegate
			{
			};
		}
		this.OnDisconnected = _003C_003Ef__am_0024cache1;
		this.OnConnectionError = delegate
		{
		};
		if (_003C_003Ef__am_0024cache3 == null)
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
			_003C_003Ef__am_0024cache3 = delegate
			{
			};
		}
		this.OnMessage = _003C_003Ef__am_0024cache3;
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
		this.OnConnected();
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					base.Logger.Info("Disconnected from {0} ({1}) CloseStatusCode={2}", m_serverAddress, notification.Message.Trim(), notification.Code);
					m_allowRelogin = true;
					this.OnDisconnected(notification.Message, m_allowRelogin, notification.Code);
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
			switch (4)
			{
			case 0:
				continue;
			}
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
			this.OnDisconnected(notification.Message, m_allowRelogin, notification.Code);
			return;
		}
	}

	private void HandleConnectionErrorNotification(ConnectionErrorNotification notification)
	{
		this.OnConnectionError(notification.ErrorMessage);
	}

	protected override void HandleMessage(WebSocketMessage message)
	{
		base.HandleMessage(message);
		m_messageDispatcher.HandleMessage(this, message);
	}

	private void HandleBinaryMessageNotification(BinaryMessageNotification notification)
	{
		this.OnMessage(notification);
	}

	public bool SendMessage(byte[] bytes)
	{
		if (m_webSocket != null)
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
			if (m_webSocket.State == WebSocket.SocketState.Open)
			{
				m_webSocket.Send(bytes);
				return true;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}
}
