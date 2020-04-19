using System;
using System.Runtime.CompilerServices;

public static class ConsoleLog
{
	private static object s_lock;

	[CompilerGenerated]
	private static Action<Log.Message> <>f__mg$cache0;

	[CompilerGenerated]
	private static Action<Log.Message> <>f__mg$cache1;

	static ConsoleLog()
	{
		ConsoleLog.MinStdOutLevel = Log.Level.Info;
		ConsoleLog.MinStdErrLevel = Log.Level.Warning;
		ConsoleLog.s_lock = new object();
	}

	public static Log.Level MinStdOutLevel { get; set; }

	public static Log.Level MinStdErrLevel { get; set; }

	public static bool RawLogging { get; set; }

	public static bool Started { get; set; } = false;

	public static void HandleLogMessage(Log.Message args)
	{
		object obj = ConsoleLog.s_lock;
		lock (obj)
		{
			if (args.level < ConsoleLog.MinStdOutLevel)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ConsoleLog.HandleLogMessage(Log.Message)).MethodHandle;
				}
				if (args.level < ConsoleLog.MinStdErrLevel)
				{
					goto IL_11F;
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
			string text = args.ToString();
			if (!text.IsNullOrEmpty())
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
				string value = string.Empty;
				if (!ConsoleLog.RawLogging)
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
					value = string.Format("{0} [{1}] ", args.timestamp.ToString(Log.TimestampFormat), Log.ToStringCode(args.level));
				}
				bool flag = false;
				if (args.level >= ConsoleLog.MinStdErrLevel)
				{
					Console.Error.Write(value);
					Console.Error.WriteLine(text);
					flag = true;
				}
				else if (args.level >= ConsoleLog.MinStdOutLevel)
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
					Console.Out.Write(value);
					Console.Out.WriteLine(text);
					flag = true;
				}
				if (flag)
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
				}
			}
			IL_11F:;
		}
	}

	public static void Start()
	{
		if (!ConsoleLog.Started)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ConsoleLog.Start()).MethodHandle;
			}
			ConsoleLog.Started = true;
			if (ConsoleLog.<>f__mg$cache0 == null)
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
				ConsoleLog.<>f__mg$cache0 = new Action<Log.Message>(ConsoleLog.HandleLogMessage);
			}
			Log.AddLogHandler(ConsoleLog.<>f__mg$cache0);
		}
	}

	public static void Stop()
	{
		if (ConsoleLog.Started)
		{
			ConsoleLog.Started = false;
			if (ConsoleLog.<>f__mg$cache1 == null)
			{
				ConsoleLog.<>f__mg$cache1 = new Action<Log.Message>(ConsoleLog.HandleLogMessage);
			}
			Log.RemoveLogHandler(ConsoleLog.<>f__mg$cache1);
		}
	}
}
