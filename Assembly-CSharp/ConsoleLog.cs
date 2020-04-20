using System;
using System.Runtime.CompilerServices;

public static class ConsoleLog
{
	private static object s_lock;

	[CompilerGenerated]
	private static Action<Log.Message> f__mg_cache0;

	[CompilerGenerated]
	private static Action<Log.Message> f__mg_cache1;

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
				if (args.level < ConsoleLog.MinStdErrLevel)
				{
					goto IL_11F;
				}
			}
			string text = args.ToString();
			if (!text.IsNullOrEmpty())
			{
				string value = string.Empty;
				if (!ConsoleLog.RawLogging)
				{
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
					Console.Out.Write(value);
					Console.Out.WriteLine(text);
					flag = true;
				}
				if (flag)
				{
				}
			}
			IL_11F:;
		}
	}

	public static void Start()
	{
		if (!ConsoleLog.Started)
		{
			ConsoleLog.Started = true;
			
			Log.AddLogHandler(new Action<Log.Message>(ConsoleLog.HandleLogMessage));
		}
	}

	public static void Stop()
	{
		if (ConsoleLog.Started)
		{
			ConsoleLog.Started = false;
			
			Log.RemoveLogHandler(new Action<Log.Message>(ConsoleLog.HandleLogMessage));
		}
	}
}
