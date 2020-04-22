using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;

public class LogInstance
{
	private Log.Message m_lastLogEventArgs;

	private object m_lock;

	public bool TrackRepeats
	{
		get;
		set;
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
				action = Interlocked.CompareExchange(ref this.OnLogMessage, (Action<Log.Message>)Delegate.Combine(action2, value), action);
			}
			while ((object)action != action2);
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
				return;
			}
		}
		remove
		{
			Action<Log.Message> action = this.OnLogMessage;
			Action<Log.Message> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref this.OnLogMessage, (Action<Log.Message>)Delegate.Remove(action2, value), action);
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
	}

	public LogInstance()
	{
		if (_003C_003Ef__am_0024cache0 == null)
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
			_003C_003Ef__am_0024cache0 = delegate
			{
			};
		}
		this.OnLogMessage = _003C_003Ef__am_0024cache0;
		base._002Ector();
		m_lock = new object();
	}

	public void AddLogHandler(Action<Log.Message> handler)
	{
		OnLogMessage += handler;
	}

	public void RemoveLogHandler(Action<Log.Message> handler)
	{
		OnLogMessage -= handler;
	}

	[Conditional("HYDROGEN_DEBUG")]
	public void _001D(Log.Category _001D, string _000E, params object[] _0012)
	{
		Write(Log.Level._001D, _001D, _000E, _0012);
	}

	public void Info(Log.Category category, string message, params object[] args)
	{
		Write(Log.Level.Info, category, message, args);
	}

	public void Warning(Log.Category category, string message, params object[] args)
	{
		Write(Log.Level.Warning, category, message, args);
	}

	public void Notice(Log.Category category, string message, params object[] args)
	{
		Write(Log.Level.Notice, category, message, args);
	}

	[Conditional("HYDROGEN_DEBUG")]
	public void _001D(string _001D, params object[] _000E)
	{
		Write(Log.Level._001D, Log.Category.None, _001D, _000E);
	}

	public void Info(string message, params object[] args)
	{
		Write(Log.Level.Info, Log.Category.None, message, args);
	}

	public void Warning(string message, params object[] args)
	{
		Write(Log.Level.Warning, Log.Category.None, message, args);
	}

	public void Notice(string message, params object[] args)
	{
		Write(Log.Level.Notice, Log.Category.None, message, args);
	}

	public void Error(string message, params object[] args)
	{
		Write(Log.Level.Error, Log.Category.Error, message, args);
	}

	public void Critical(string message, params object[] args)
	{
		Write(Log.Level.Critical, Log.Category.Error, message, args);
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
		Log.Message message2 = default(Log.Message);
		message2.level = Log.Level.Error;
		message2.category = Log.Category.Error;
		message2.message = message;
		message2.formattedMessage = formattedMessage;
		message2.timestamp = DateTime.Now;
		message2.exception = ex;
		message2.file = file;
		message2.line = line;
		Log.Message obj2 = message2;
		this.OnLogMessage(obj2);
	}

	public void Exception(Exception exception)
	{
		JsonSerializationException ex = exception as JsonSerializationException;
		if (ex != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Exception(ex.ToReadableString());
					return;
				}
			}
		}
		if (exception.InnerException != null)
		{
			exception = exception.InnerException;
		}
		Exception(exception.ToReadableString());
	}

	public void Write(Log.Level level, Log.Category category, string message, params object[] args)
	{
		Write(level, category, null, 0, message.Trim(), args);
	}

	public void Write(Log.Level level, Log.Category category, string fileName, int lineNumber, string message, params object[] args)
	{
		lock (m_lock)
		{
			string text = Log.Message.Format(message, args);
			if (!TrackRepeats)
			{
				goto IL_0073;
			}
			while (true)
			{
				switch (2)
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
			if (!(m_lastLogEventArgs.formattedMessage == text))
			{
				goto IL_0073;
			}
			m_lastLogEventArgs.repeatCount++;
			m_lastLogEventArgs.timestamp = DateTime.Now;
			goto end_IL_000d;
			IL_0073:
			if (m_lastLogEventArgs.repeatCount > 0)
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
				this.OnLogMessage(m_lastLogEventArgs);
			}
			m_lastLogEventArgs = new Log.Message
			{
				level = level,
				category = category,
				message = message,
				formattedMessage = text,
				timestamp = DateTime.Now,
				file = fileName,
				line = lineNumber
			};
			Write(m_lastLogEventArgs);
			end_IL_000d:;
		}
	}

	public void Write(Log.Message message)
	{
		this.OnLogMessage(message);
	}

	public void Update()
	{
		if (m_lastLogEventArgs.repeatCount <= 0 || !(DateTime.Now - m_lastLogEventArgs.timestamp > TimeSpan.FromSeconds(1.0)))
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
			this.OnLogMessage(m_lastLogEventArgs);
			m_lastLogEventArgs = default(Log.Message);
			return;
		}
	}
}
