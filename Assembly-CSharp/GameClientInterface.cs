using System;
using System.Diagnostics;
using System.Threading;
using WebSocketSharp;

public class GameClientInterface : WebSocketInterface
{
	protected bool m_registered;

	protected bool m_allowRelogin;

	protected string m_gameClientHandle;

	protected string m_gameServerAddress;

	protected WebSocketMessageDispatcher<GameClientInterface> m_messageDispatcher;

	public GameClientInterface()
	{
		if (GameClientInterface.<>f__am$cache0 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameClientInterface..ctor()).MethodHandle;
			}
			GameClientInterface.<>f__am$cache0 = delegate()
			{
			};
		}
		this.OnConnected = GameClientInterface.<>f__am$cache0;
		if (GameClientInterface.<>f__am$cache1 == null)
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
			GameClientInterface.<>f__am$cache1 = delegate(string A_0, bool A_1, CloseStatusCode A_2)
			{
			};
		}
		this.OnDisconnected = GameClientInterface.<>f__am$cache1;
		this.OnConnectionError = delegate(string A_0)
		{
		};
		if (GameClientInterface.<>f__am$cache3 == null)
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
			GameClientInterface.<>f__am$cache3 = delegate(BinaryMessageNotification A_0)
			{
			};
		}
		this.OnMessage = GameClientInterface.<>f__am$cache3;
		base..ctor(new WebSocketMessageFactory());
		base.HeartbeatPeriod = TimeSpan.FromMinutes(1.0);
		base.HeartbeatTimeout = TimeSpan.FromMinutes(5.0);
		base.IsRaw = true;
		base.MaxMessagesPerSecond = 0;
		this.m_registered = false;
		base.ConnectionTimeout = 30f;
	}

	public event Action OnConnected
	{
		add
		{
			Action action = this.OnConnected;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnConnected, (Action)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameClientInterface.add_OnConnected(Action)).MethodHandle;
			}
		}
		remove
		{
			Action action = this.OnConnected;
			Action action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action>(ref this.OnConnected, (Action)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameClientInterface.remove_OnConnected(Action)).MethodHandle;
			}
		}
	}

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
				action = Interlocked.CompareExchange<Action<string>>(ref this.OnConnectionError, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameClientInterface.add_OnConnectionError(Action<string>)).MethodHandle;
			}
		}
		remove
		{
			Action<string> action = this.OnConnectionError;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<string>>(ref this.OnConnectionError, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameClientInterface.remove_OnConnectionError(Action<string>)).MethodHandle;
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
				action = Interlocked.CompareExchange<Action<BinaryMessageNotification>>(ref this.OnMessage, (Action<BinaryMessageNotification>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameClientInterface.add_OnMessage(Action<BinaryMessageNotification>)).MethodHandle;
			}
		}
		remove
		{
			Action<BinaryMessageNotification> action = this.OnMessage;
			Action<BinaryMessageNotification> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<BinaryMessageNotification>>(ref this.OnMessage, (Action<BinaryMessageNotification>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameClientInterface.remove_OnMessage(Action<BinaryMessageNotification>)).MethodHandle;
			}
		}
	}

	public bool IsConnected
	{
		get
		{
			return base.State == global::WebSocket.SocketState.Open;
		}
	}

	public void Initialize(string gameServerAddress, string gameClientHandle)
	{
		this.m_gameClientHandle = gameClientHandle;
		this.m_gameServerAddress = gameServerAddress;
		this.m_messageDispatcher = new WebSocketMessageDispatcher<GameClientInterface>();
		this.m_messageDispatcher.RegisterMessageDelegate<ConnectionOpenedNotification>(new Action<ConnectionOpenedNotification>(this.HandleConnectionOpenedNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<ConnectionClosedNotification>(new Action<ConnectionClosedNotification>(this.HandleConnectionClosedNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<ConnectionErrorNotification>(new Action<ConnectionErrorNotification>(this.HandleConnectionErrorNotification));
		this.m_messageDispatcher.RegisterMessageDelegate<BinaryMessageNotification>(new Action<BinaryMessageNotification>(this.HandleBinaryMessageNotification));
	}

	public override void Connect()
	{
		this.m_registered = false;
		base.InitializeSocket(this.m_gameServerAddress, 0x1A2C, "GameClientInterface");
		base.Connect();
	}

	public override void Update()
	{
		base.Update();
	}

	private void HandleConnectionOpenedNotification(ConnectionOpenedNotification notification)
	{
		base.Logger.Info("Connected to game server {0}", new object[]
		{
			this.m_serverAddress
		});
		this.m_overallConnectionTimer.Reset();
		this.m_registered = true;
		this.OnConnected();
	}

	private void HandleConnectionClosedNotification(ConnectionClosedNotification notification)
	{
		if (this.m_registered)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameClientInterface.HandleConnectionClosedNotification(ConnectionClosedNotification)).MethodHandle;
			}
			base.Logger.Info("Disconnected from {0} ({1}) CloseStatusCode={2}", new object[]
			{
				this.m_serverAddress,
				notification.Message.Trim(),
				notification.Code
			});
			this.m_allowRelogin = true;
			this.OnDisconnected(notification.Message, this.m_allowRelogin, notification.Code);
		}
		else if (this.m_overallConnectionTimer.IsRunning)
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
			if (this.m_overallConnectionTimer.Elapsed.TotalSeconds < (double)base.ConnectionTimeout)
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
				base.Logger.Info("Retrying connection to game server {0}: {1} CloseStatusCode={2}", new object[]
				{
					this.m_serverAddress,
					notification.Message.Trim(),
					notification.Code
				});
				base.Reconnect();
			}
			else
			{
				base.Logger.Info("Failed to connect to game server {0}: {1} CloseStatusCode={2}", new object[]
				{
					this.m_serverAddress,
					notification.Message.Trim(),
					notification.Code
				});
				this.m_overallConnectionTimer.Reset();
				this.m_allowRelogin = false;
				this.OnDisconnected(notification.Message, this.m_allowRelogin, notification.Code);
			}
		}
	}

	private void HandleConnectionErrorNotification(ConnectionErrorNotification notification)
	{
		this.OnConnectionError(notification.ErrorMessage);
	}

	protected override void HandleMessage(WebSocketMessage message)
	{
		base.HandleMessage(message);
		this.m_messageDispatcher.HandleMessage(this, message);
	}

	private void HandleBinaryMessageNotification(BinaryMessageNotification notification)
	{
		this.OnMessage(notification);
	}

	public bool SendMessage(byte[] bytes)
	{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameClientInterface.SendMessage(byte[])).MethodHandle;
			}
			if (this.m_webSocket.State == global::WebSocket.SocketState.Open)
			{
				this.m_webSocket.Send(bytes);
				return true;
			}
			for (;;)
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
