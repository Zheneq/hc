using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class UnityConsoleLog
{
	public static Action<Log.Message> InterceptHandler;

	private static object s_lock;

	private static bool s_isLogging;

	private static string s_stackTraceSeparator = "\n   at ";

	[CompilerGenerated]
	private static Action<Log.Message> f__mg_cache0;

	[CompilerGenerated]
	private static Application.LogCallback f__mg_cache1;

	[CompilerGenerated]
	private static Action<Log.Message> f__mg_cache2;

	[CompilerGenerated]
	private static Application.LogCallback f__mg_cache3;

	static UnityConsoleLog()
	{
		if (Debug.isDebugBuild)
		{
			UnityConsoleLog.MinLevel = Log.Level.Info;
		}
		else
		{
			UnityConsoleLog.MinLevel = Log.Level.Nothing;
		}
		UnityConsoleLog.LogInfoAsDebug = false;
		UnityConsoleLog.LogErrorAsWarning = false;
		UnityConsoleLog.s_lock = new object();
		UnityConsoleLog.s_isLogging = false;
	}

	public static Log.Level MinLevel { get; set; }

	public static bool Started { get; set; }

	public static bool LogInfoAsDebug { get; set; }

	public static bool LogErrorAsWarning { get; set; }

	public static void HandleLogMessage(Log.Message args)
	{
		object obj = UnityConsoleLog.s_lock;
		lock (obj)
		{
			if (UnityConsoleLog.InterceptHandler != null)
			{
				UnityConsoleLog.InterceptHandler(args);
			}
			else if (!UnityConsoleLog.s_isLogging)
			{
				if (args.level >= UnityConsoleLog.MinLevel)
				{
					try
					{
						UnityConsoleLog.s_isLogging = true;
						string text = args.ToString();
						if (!text.IsNullOrEmpty())
						{
							switch (args.level)
							{
							case Log.Level.symbol_001D:
							case Log.Level.Info:
							case Log.Level.Notice:
								Debug.Log(text);
								break;
							case Log.Level.Warning:
								Debug.LogWarning(text);
								break;
							case Log.Level.Error:
								Debug.LogError(text);
								break;
							}
						}
					}
					finally
					{
						UnityConsoleLog.s_isLogging = false;
					}
				}
			}
		}
	}

	public static void Start()
	{
		if (!UnityConsoleLog.Started)
		{
			UnityConsoleLog.Started = true;
			
			Log.AddLogHandler(new Action<Log.Message>(UnityConsoleLog.HandleLogMessage));
			
			Application.logMessageReceived += new Application.LogCallback(UnityConsoleLog.HandleUnityLogMessage);
		}
	}

	public static void Stop()
	{
		if (UnityConsoleLog.Started)
		{
			UnityConsoleLog.Started = false;
			
			Log.RemoveLogHandler(new Action<Log.Message>(UnityConsoleLog.HandleLogMessage));
			
			Application.logMessageReceived -= new Application.LogCallback(UnityConsoleLog.HandleUnityLogMessage);
		}
	}

	private static void HandleUnityLogMessage(string logString, string stackTrace, LogType type)
	{
		stackTrace = stackTrace.Trim();
		Log.Level level = Log.Level.Nothing;
		bool flag = false;
		bool flag2 = false;
		switch (type)
		{
		case LogType.Error:
		case LogType.Assert:
		case LogType.Exception:
			level = ((!UnityConsoleLog.LogErrorAsWarning) ? Log.Level.Error : Log.Level.Warning);
			flag = true;
			flag2 = true;
			break;
		case LogType.Warning:
			level = Log.Level.Warning;
			flag = true;
			break;
		case LogType.Log:
			level = ((!UnityConsoleLog.LogInfoAsDebug) ? Log.Level.Info : Log.Level.symbol_001D);
			flag = false;
			break;
		}
		flag2 = false;
		if (!UnityConsoleLog.s_isLogging)
		{
			try
			{
				UnityConsoleLog.s_isLogging = true;
				string empty = string.Empty;
				int lineNumber = 0;
				if (!stackTrace.IsNullOrEmpty())
				{
					UnityConsoleLog.TryGetFileLineFromStack(stackTrace, out empty, out lineNumber);
				}
				if (flag)
				{
					if (!stackTrace.IsNullOrEmpty())
					{
						string arg = stackTrace.Replace("\n", UnityConsoleLog.s_stackTraceSeparator);
						logString = string.Format("{0}{1}{2}", logString, UnityConsoleLog.s_stackTraceSeparator, arg);
					}
				}
				Log.Write(level, Log.Category.Unity, empty, lineNumber, logString, new object[0]);
			}
			finally
			{
				UnityConsoleLog.s_isLogging = false;
			}
		}
		if (flag2)
		{
			if (TextConsole.Get() != null)
			{
				ConsoleMessageType consoleMessageType;
				if (type == LogType.Exception)
				{
					consoleMessageType = ConsoleMessageType.Exception;
				}
				else
				{
					consoleMessageType = ConsoleMessageType.Error;
				}
				ConsoleMessageType messageType = consoleMessageType;
				TextConsole.Get().Write(new TextConsole.Message
				{
					Text = string.Format("{0}\n{1}", logString, stackTrace),
					MessageType = messageType
				}, null);
			}
		}
	}

	private unsafe static void TryGetFileLineFromStack(string stack, out string file, out int line)
	{
		file = string.Empty;
		line = 0;
		try
		{
			int num = stack.IndexOf(".cs:");
			if (num < 0)
			{
			}
			else
			{
				int num2 = stack.LastIndexOf("(at ", num);
				if (num2 < 0)
				{
				}
				else
				{
					num2 += "(at ".Length;
					file = stack.Substring(num2, num - num2) + ".cs";
					int num3 = stack.IndexOf(')', num);
					string value = stack.Substring(num + 4, num3 - (num + 4));
					line = Convert.ToInt32(value);
				}
			}
		}
		catch (Exception)
		{
		}
	}
}
