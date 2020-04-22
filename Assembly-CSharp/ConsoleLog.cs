using System;
using System.Runtime.CompilerServices;

public static class ConsoleLog
{
	private static object s_lock;

	[CompilerGenerated]
	private static Action<Log.Message> _003C_003Ef__mg_0024cache0;

	[CompilerGenerated]
	private static Action<Log.Message> _003C_003Ef__mg_0024cache1;

	public static Log.Level MinStdOutLevel
	{
		get;
		set;
	}

	public static Log.Level MinStdErrLevel
	{
		get;
		set;
	}

	public static bool RawLogging
	{
		get;
		set;
	}

	public static bool Started
	{
		get;
		set;
	}

	static ConsoleLog()
	{
		Started = false;
		MinStdOutLevel = Log.Level.Info;
		MinStdErrLevel = Log.Level.Warning;
		s_lock = new object();
	}

	public static void HandleLogMessage(Log.Message args)
	{
		lock (s_lock)
		{
			if (args.level >= MinStdOutLevel)
			{
				goto IL_0048;
			}
			if (args.level >= MinStdErrLevel)
			{
				goto IL_0048;
			}
			goto end_IL_000c;
			IL_0048:
			string text = args.ToString();
			if (!text.IsNullOrEmpty())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						string value = string.Empty;
						if (!RawLogging)
						{
							value = $"{args.timestamp.ToString(Log.TimestampFormat)} [{Log.ToStringCode(args.level)}] ";
						}
						bool flag = false;
						if (args.level >= MinStdErrLevel)
						{
							Console.Error.Write(value);
							Console.Error.WriteLine(text);
							flag = true;
						}
						else if (args.level >= MinStdOutLevel)
						{
							Console.Out.Write(value);
							Console.Out.WriteLine(text);
							flag = true;
						}
						if (flag)
						{
							while (true)
							{
								switch (1)
								{
								default:
									return;
								case 0:
									break;
								}
							}
						}
						return;
					}
					}
				}
			}
			end_IL_000c:;
		}
	}

	public static void Start()
	{
		if (Started)
		{
			return;
		}
		while (true)
		{
			Started = true;
			if (_003C_003Ef__mg_0024cache0 == null)
			{
				_003C_003Ef__mg_0024cache0 = HandleLogMessage;
			}
			Log.AddLogHandler(_003C_003Ef__mg_0024cache0);
			return;
		}
	}

	public static void Stop()
	{
		if (Started)
		{
			Started = false;
			Log.RemoveLogHandler(HandleLogMessage);
		}
	}
}
