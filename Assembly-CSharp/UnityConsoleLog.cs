using System;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public static class UnityConsoleLog
{
	public static Action<Log.Message> InterceptHandler;

	private static object s_lock;

	private static bool s_isLogging;

	private static string s_stackTraceSeparator;

	[CompilerGenerated]
	private static Action<Log.Message> _003C_003Ef__mg_0024cache0;

	//[CompilerGenerated]
	//private static Application.LogCallback HandleUnityLogMessage;

	//[CompilerGenerated]
	//private static Action<Log.Message> HandleLogMessage;

	//[CompilerGenerated]
	//private static Application.LogCallback HandleUnityLogMessage;

	public static Log.Level MinLevel
	{
		get;
		set;
	}

	public static bool Started
	{
		get;
		set;
	}

	public static bool LogInfoAsDebug
	{
		get;
		set;
	}

	public static bool LogErrorAsWarning
	{
		get;
		set;
	}

	static UnityConsoleLog()
	{
		s_stackTraceSeparator = "\n   at ";
		if (Debug.isDebugBuild)
		{
			MinLevel = Log.Level.Info;
		}
		else
		{
			MinLevel = Log.Level.Nothing;
		}
		LogInfoAsDebug = false;
		LogErrorAsWarning = false;
		s_lock = new object();
		s_isLogging = false;
	}

	public static void HandleLogMessage(Log.Message args)
	{
		lock (s_lock)
		{
			if (InterceptHandler != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						InterceptHandler(args);
						return;
					}
				}
			}
			if (!s_isLogging)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if (args.level >= MinLevel)
						{
							try
							{
								s_isLogging = true;
								string text = args.ToString();
								if (!text.IsNullOrEmpty())
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											break;
										default:
											switch (args.level)
											{
											case Log.Level.Critical:
												break;
											case Log.Level.Debug:
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
											return;
										}
									}
								}
							}
							finally
							{
								s_isLogging = false;
							}
						}
						return;
					}
				}
			}
		}
	}

	public static void Start()
	{
		if (Started)
		{
			return;
		}
		Started = true;
		Log.AddLogHandler(HandleLogMessage);
		
		Application.logMessageReceived += HandleUnityLogMessage;
	}

	public static void Stop()
	{
		if (!Started)
		{
			return;
		}
		while (true)
		{
			Started = false;
			
			Log.RemoveLogHandler(HandleLogMessage);
			
			Application.logMessageReceived -= HandleUnityLogMessage;
			return;
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
			level = ((!LogErrorAsWarning) ? Log.Level.Error : Log.Level.Warning);
			flag = true;
			flag2 = true;
			break;
		case LogType.Warning:
			level = Log.Level.Warning;
			flag = true;
			break;
		case LogType.Log:
			level = ((!LogInfoAsDebug) ? Log.Level.Info : Log.Level.Debug);
			flag = false;
			break;
		}
		flag2 = false;
		if (!s_isLogging)
		{
			try
			{
				s_isLogging = true;
				string file = string.Empty;
				int line = 0;
				if (!stackTrace.IsNullOrEmpty())
				{
					TryGetFileLineFromStack(stackTrace, out file, out line);
				}
				if (flag)
				{
					if (!stackTrace.IsNullOrEmpty())
					{
						string arg = stackTrace.Replace("\n", s_stackTraceSeparator);
						logString = new StringBuilder().Append(logString).Append(s_stackTraceSeparator).Append(arg).ToString();
					}
				}
				Log.Write(level, Log.Category.Unity, file, line, logString);
			}
			finally
			{
				s_isLogging = false;
			}
		}
		if (!flag2)
		{
			return;
		}
		while (true)
		{
			if (TextConsole.Get() == null)
			{
				return;
			}
			while (true)
			{
				int num;
				if (type == LogType.Exception)
				{
					num = 8;
				}
				else
				{
					num = 7;
				}
				ConsoleMessageType messageType = (ConsoleMessageType)num;
				TextConsole.Get().Write(new TextConsole.Message
				{
					Text = new StringBuilder().Append(logString).Append("\n").Append(stackTrace).ToString(),
					MessageType = messageType
				});
				return;
			}
		}
	}

	private static void TryGetFileLineFromStack(string stack, out string file, out int line)
	{
		file = string.Empty;
		line = 0;
		try
		{
			int num = stack.IndexOf(".cs:");
			if (num < 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return;
					}
				}
			}
			int num2 = stack.LastIndexOf("(at ", num);
			if (num2 < 0)
			{
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
			num2 += "(at ".Length;
			file = new StringBuilder().Append(stack, num2, num - num2).Append(".cs").ToString();
			int num3 = stack.IndexOf(')', num);
			string value = stack.Substring(num + 4, num3 - (num + 4));
			line = Convert.ToInt32(value);
		}
		catch (Exception)
		{
		}
	}
}
