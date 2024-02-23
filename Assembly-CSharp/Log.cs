using System;
using System.Diagnostics;
using System.Text;

public static class Log
{
	public enum Level
	{
		Unknown,
		Everything,
		Debug,
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
		public Level level;
		public Category category;
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
			if (args != null && args.Length != 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i] is Exception ex)
					{
						args[i] = ex.ToReadableString();
					}
				}
				return string.Format(message, args);
			}
			return message;
		}

		public override string ToString()
		{
			if (repeatCount > 0)
			{
				return new StringBuilder().Append(formattedMessage).Append(" (repeated ").Append(repeatCount).Append(" times)").ToString();
			}
			return formattedMessage;
		}
	}

	public static string TimestampFormat;

	public static LogInstance LogInstance { get; private set; }

	static Log()
	{
		TimestampFormat = "yyyy-MM-dd HH:mm:ss.fffK";
		LogInstance = new LogInstance();
	}

	public static void AddLogHandler(Action<Message> handler)
	{
		LogInstance.AddLogHandler(handler);
	}

	public static void RemoveLogHandler(Action<Message> handler)
	{
		LogInstance.RemoveLogHandler(handler);
	}

	[Conditional("HYDROGEN_DEBUG")]
	public static void Debug(Log.Category category, string message, params object[] args)
	{
	}

	public static void Info(Category category, string message, params object[] args)
	{
		LogInstance.Info(category, message, args);
	}

	public static void Warning(Category category, string message, params object[] args)
	{
		LogInstance.Warning(category, message, args);
	}

	public static void Notice(Category category, string message, params object[] args)
	{
		LogInstance.Notice(category, message, args);
	}

	[Conditional("HYDROGEN_DEBUG")]
	public static void Debug(string message, params object[] args)
	{
	}

	public static void Info(string message, params object[] args)
	{
		LogInstance.Info(message, args);
	}

	public static void Warning(string message, params object[] args)
	{
		LogInstance.Warning(message, args);
	}

	public static void Notice(string message, params object[] args)
	{
		LogInstance.Notice(message, args);
	}

	public static void Error(string message, params object[] args)
	{
		LogInstance.Error(message, args);
	}

	public static void Critical(string message, params object[] args)
	{
		LogInstance.Critical(message, args);
	}

	public static void Exception(string message, params object[] args)
	{
		LogInstance.Exception(message, args);
	}

	public static void Exception(Exception exception)
	{
		LogInstance.Exception(exception);
	}

	public static void Write(Level level, Category category, string fileName, int lineNumber, string message, params object[] args)
	{
		LogInstance.Write(level, category, fileName, lineNumber, message, args);
	}

	public static void Write(Message message)
	{
		LogInstance.Write(message);
	}

	public static void Update()
	{
		LogInstance.Update();
	}

	public static string ToString(Level level)
	{
		switch (level)
		{
		case Level.Everything:
			return "Everything";
		case Level.Debug:
			return "Debug";
		case Level.Info:
			return "Info";
		case Level.Warning:
			return "Warning";
		case Level.Error:
			return "Error";
		case Level.Critical:
			return "Critical";
		case Level.Nothing:
			return "Nothing";
		case Level.Notice:
			return "Notice";
		default:
			return "Unknown";
		}
	}

	public static string ToStringCode(Level level)
	{
		switch (level)
		{
		case Level.Debug:
			return "DBG";
		case Level.Info:
			return "INF";
		case Level.Warning:
			return "WRN";
		case Level.Error:
			return "ERR";
		case Level.Critical:
			return "CRI";
		case Level.Notice:
			return "NOT";
		default:
			return "UNK";
		}
	}

	public static Level FromString(string level)
	{
		if (string.Equals(level, ToString(Level.Everything), StringComparison.OrdinalIgnoreCase))
		{
			return Level.Everything;
		}
		if (string.Equals(level, ToString(Level.Debug), StringComparison.OrdinalIgnoreCase))
		{
			return Level.Debug;
		}
		if (string.Equals(level, ToString(Level.Info), StringComparison.OrdinalIgnoreCase))
		{
			return Level.Info;
		}
		if (string.Equals(level, ToString(Level.Warning), StringComparison.OrdinalIgnoreCase))
		{
			return Level.Warning;
		}
		if (string.Equals(level, ToString(Level.Error), StringComparison.OrdinalIgnoreCase))
		{
			return Level.Error;
		}
		if (string.Equals(level, ToString(Level.Critical), StringComparison.OrdinalIgnoreCase))
		{
			return Level.Critical;
		}
		if (string.Equals(level, ToString(Level.Notice), StringComparison.OrdinalIgnoreCase))
		{
			return Level.Notice;
		}
		if (string.Equals(level, ToString(Level.Nothing), StringComparison.OrdinalIgnoreCase))
		{
			return Level.Nothing;
		}
		return Level.Unknown;
	}
}
