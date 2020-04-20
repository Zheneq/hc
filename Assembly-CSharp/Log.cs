using System;
using System.Diagnostics;

public static class Log
{
	public static string TimestampFormat = "yyyy-MM-dd HH:mm:ss.fffK";

	public static LogInstance LogInstance { get; private set; } = new LogInstance();

	public static void AddLogHandler(Action<Log.Message> handler)
	{
		Log.LogInstance.AddLogHandler(handler);
	}

	public static void RemoveLogHandler(Action<Log.Message> handler)
	{
		Log.LogInstance.RemoveLogHandler(handler);
	}

	[Conditional("HYDROGEN_DEBUG")]
	public static void symbol_001D(Log.Category symbol_001D, string symbol_000E, params object[] symbol_0012)
	{
	}

	public static void Info(Log.Category category, string message, params object[] args)
	{
		Log.LogInstance.Info(category, message, args);
	}

	public static void Warning(Log.Category category, string message, params object[] args)
	{
		Log.LogInstance.Warning(category, message, args);
	}

	public static void Notice(Log.Category category, string message, params object[] args)
	{
		Log.LogInstance.Notice(category, message, args);
	}

	[Conditional("HYDROGEN_DEBUG")]
	public static void symbol_001D(string symbol_001D, params object[] symbol_000E)
	{
	}

	public static void Info(string message, params object[] args)
	{
		Log.LogInstance.Info(message, args);
	}

	public static void Warning(string message, params object[] args)
	{
		Log.LogInstance.Warning(message, args);
	}

	public static void Notice(string message, params object[] args)
	{
		Log.LogInstance.Notice(message, args);
	}

	public static void Error(string message, params object[] args)
	{
		Log.LogInstance.Error(message, args);
	}

	public static void Critical(string message, params object[] args)
	{
		Log.LogInstance.Critical(message, args);
	}

	public static void Exception(string message, params object[] args)
	{
		Log.LogInstance.Exception(message, args);
	}

	public static void Exception(Exception exception)
	{
		Log.LogInstance.Exception(exception);
	}

	public static void Write(Log.Level level, Log.Category category, string fileName, int lineNumber, string message, params object[] args)
	{
		Log.LogInstance.Write(level, category, fileName, lineNumber, message, args);
	}

	public static void Write(Log.Message message)
	{
		Log.LogInstance.Write(message);
	}

	public static void Update()
	{
		Log.LogInstance.Update();
	}

	public static string ToString(Log.Level level)
	{
		switch (level)
		{
		case Log.Level.Everything:
			return "Everything";
		case Log.Level.symbol_001D:
			return "Debug";
		case Log.Level.Info:
			return "Info";
		case Log.Level.Warning:
			return "Warning";
		case Log.Level.Error:
			return "Error";
		case Log.Level.Critical:
			return "Critical";
		case Log.Level.Notice:
			return "Notice";
		case Log.Level.Nothing:
			return "Nothing";
		default:
			return "Unknown";
		}
	}

	public static string ToStringCode(Log.Level level)
	{
		switch (level)
		{
		case Log.Level.symbol_001D:
			return "DBG";
		case Log.Level.Info:
			return "INF";
		case Log.Level.Warning:
			return "WRN";
		case Log.Level.Error:
			return "ERR";
		case Log.Level.Critical:
			return "CRI";
		case Log.Level.Notice:
			return "NOT";
		default:
			return "UNK";
		}
	}

	public static Log.Level FromString(string level)
	{
		if (string.Equals(level, Log.ToString(Log.Level.Everything), StringComparison.OrdinalIgnoreCase))
		{
			return Log.Level.Everything;
		}
		if (string.Equals(level, Log.ToString(Log.Level.symbol_001D), StringComparison.OrdinalIgnoreCase))
		{
			return Log.Level.symbol_001D;
		}
		if (string.Equals(level, Log.ToString(Log.Level.Info), StringComparison.OrdinalIgnoreCase))
		{
			return Log.Level.Info;
		}
		if (string.Equals(level, Log.ToString(Log.Level.Warning), StringComparison.OrdinalIgnoreCase))
		{
			return Log.Level.Warning;
		}
		if (string.Equals(level, Log.ToString(Log.Level.Error), StringComparison.OrdinalIgnoreCase))
		{
			return Log.Level.Error;
		}
		if (string.Equals(level, Log.ToString(Log.Level.Critical), StringComparison.OrdinalIgnoreCase))
		{
			return Log.Level.Critical;
		}
		if (string.Equals(level, Log.ToString(Log.Level.Notice), StringComparison.OrdinalIgnoreCase))
		{
			return Log.Level.Notice;
		}
		if (string.Equals(level, Log.ToString(Log.Level.Nothing), StringComparison.OrdinalIgnoreCase))
		{
			return Log.Level.Nothing;
		}
		return Log.Level.Unknown;
	}

	public enum Level
	{
		Unknown,
		Everything,
		symbol_001D,
		Info,
		Warning,
		Error,
		Critical,
		Notice,
		Nothing
	}

	public enum Category
	{
		None,
		Error,
		Temp,
		Unity,
		WebSocketServer,
		WebSocketClient,
		LobbyGameClient,
		LobbyServer,
		Combat,
		ActorData,
		AbilityAnimation,
		UI,
		ChatterAudio,
		GameEvent,
		AudioManager,
		NetGameServerSerialization,
		Camera,
		CameraAbilities,
		Messaging,
		ControlPoints,
		ReactorConsoleAdmin,
		Loading,
		ActorActions,
		ActorTurnSM,
		GameplayMetrics
	}

	public struct Message
	{
		public Log.Level level;

		public Log.Category category;

		public string message;

		public string formattedMessage;

		public DateTime timestamp;

		public Exception exception;

		public string file;

		public int line;

		public int repeatCount;

		public static string Format(string message, object[] args)
		{
			if (message == null)
			{
				return null;
			}
			if (args != null)
			{
				if (args.Length != 0)
				{
					for (int i = 0; i < args.Length; i++)
					{
						Exception ex = args[i] as Exception;
						if (ex != null)
						{
							args[i] = ex.ToReadableString();
						}
					}
					return string.Format(message, args);
				}
			}
			return message;
		}

		public override string ToString()
		{
			if (this.repeatCount > 0)
			{
				return string.Format("{0} (repeated {1} times)", this.formattedMessage, this.repeatCount);
			}
			return this.formattedMessage;
		}
	}
}
