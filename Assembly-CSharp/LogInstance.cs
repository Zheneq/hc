using System;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;

public class LogInstance
{
	private Log.Message m_lastLogEventArgs;

	private object m_lock;

	public LogInstance()
	{
		if (LogInstance.<>f__am$cache0 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LogInstance..ctor()).MethodHandle;
			}
			LogInstance.<>f__am$cache0 = delegate(Log.Message A_0)
			{
			};
		}
		this.OnLogMessage = LogInstance.<>f__am$cache0;
		base..ctor();
		this.m_lock = new object();
	}

	public bool TrackRepeats { get; set; }

	public void AddLogHandler(Action<Log.Message> handler)
	{
		this.OnLogMessage += handler;
	}

	public void RemoveLogHandler(Action<Log.Message> handler)
	{
		this.OnLogMessage -= handler;
	}

	[Conditional("HYDROGEN_DEBUG")]
	public void \u001D(Log.Category \u001D, string \u000E, params object[] \u0012)
	{
		this.Write(Log.Level.\u001D, \u001D, \u000E, \u0012);
	}

	public void Info(Log.Category category, string message, params object[] args)
	{
		this.Write(Log.Level.Info, category, message, args);
	}

	public void Warning(Log.Category category, string message, params object[] args)
	{
		this.Write(Log.Level.Warning, category, message, args);
	}

	public void Notice(Log.Category category, string message, params object[] args)
	{
		this.Write(Log.Level.Notice, category, message, args);
	}

	[Conditional("HYDROGEN_DEBUG")]
	public void \u001D(string \u001D, params object[] \u000E)
	{
		this.Write(Log.Level.\u001D, Log.Category.None, \u001D, \u000E);
	}

	public void Info(string message, params object[] args)
	{
		this.Write(Log.Level.Info, Log.Category.None, message, args);
	}

	public void Warning(string message, params object[] args)
	{
		this.Write(Log.Level.Warning, Log.Category.None, message, args);
	}

	public void Notice(string message, params object[] args)
	{
		this.Write(Log.Level.Notice, Log.Category.None, message, args);
	}

	public void Error(string message, params object[] args)
	{
		this.Write(Log.Level.Error, Log.Category.Error, message, args);
	}

	public void Critical(string message, params object[] args)
	{
		this.Write(Log.Level.Critical, Log.Category.Error, message, args);
	}

	public void Exception(string message, params object[] args)
	{
		Exception ex = null;
		foreach (object obj in args)
		{
			if (obj is Exception)
			{
				ex = (Exception)obj;
				break;
			}
		}
		string file = "unknown";
		int line = 0;
		try
		{
			StackTrace stackTrace = new StackTrace(ex, true);
			StackFrame frame = stackTrace.GetFrame(0);
			if (frame != null)
			{
				file = frame.GetFileName();
				line = frame.GetFileLineNumber();
			}
		}
		catch (Exception)
		{
		}
		string formattedMessage = Log.Message.Format(message, args);
		Log.Message obj2 = new Log.Message
		{
			level = Log.Level.Error,
			category = Log.Category.Error,
			message = message,
			formattedMessage = formattedMessage,
			timestamp = DateTime.Now,
			exception = ex,
			file = file,
			line = line
		};
		this.OnLogMessage(obj2);
	}

	public void Exception(Exception exception)
	{
		JsonSerializationException ex = exception as JsonSerializationException;
		if (ex != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LogInstance.Exception(System.Exception)).MethodHandle;
			}
			this.Exception(ex.ToReadableString(), new object[0]);
			return;
		}
		if (exception.InnerException != null)
		{
			exception = exception.InnerException;
		}
		this.Exception(exception.ToReadableString(), new object[0]);
	}

	public void Write(Log.Level level, Log.Category category, string message, params object[] args)
	{
		this.Write(level, category, null, 0, message.Trim(), args);
	}

	public void Write(Log.Level level, Log.Category category, string fileName, int lineNumber, string message, params object[] args)
	{
		object @lock = this.m_lock;
		lock (@lock)
		{
			string text = Log.Message.Format(message, args);
			if (this.TrackRepeats)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LogInstance.Write(Log.Level, Log.Category, string, int, string, object[])).MethodHandle;
				}
				if (this.m_lastLogEventArgs.formattedMessage == text)
				{
					this.m_lastLogEventArgs.repeatCount = this.m_lastLogEventArgs.repeatCount + 1;
					this.m_lastLogEventArgs.timestamp = DateTime.Now;
					return;
				}
			}
			if (this.m_lastLogEventArgs.repeatCount > 0)
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
				this.OnLogMessage(this.m_lastLogEventArgs);
			}
			this.m_lastLogEventArgs = new Log.Message
			{
				level = level,
				category = category,
				message = message,
				formattedMessage = text,
				timestamp = DateTime.Now,
				file = fileName,
				line = lineNumber
			};
			this.Write(this.m_lastLogEventArgs);
		}
	}

	public void Write(Log.Message message)
	{
		this.OnLogMessage(message);
	}

	public void Update()
	{
		if (this.m_lastLogEventArgs.repeatCount > 0 && DateTime.Now - this.m_lastLogEventArgs.timestamp > TimeSpan.FromSeconds(1.0))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LogInstance.Update()).MethodHandle;
			}
			this.OnLogMessage(this.m_lastLogEventArgs);
			this.m_lastLogEventArgs = default(Log.Message);
		}
	}

	private event Action<Log.Message> OnLogMessage
	{
		add
		{
			Action<Log.Message> action = this.OnLogMessage;
			Action<Log.Message> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<Log.Message>>(ref this.OnLogMessage, (Action<Log.Message>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LogInstance.add_OnLogMessage(Action<Log.Message>)).MethodHandle;
			}
		}
		remove
		{
			Action<Log.Message> action = this.OnLogMessage;
			Action<Log.Message> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange<Action<Log.Message>>(ref this.OnLogMessage, (Action<Log.Message>)Delegate.Remove(action2, value), action);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LogInstance.remove_OnLogMessage(Action<Log.Message>)).MethodHandle;
			}
		}
	}
}
