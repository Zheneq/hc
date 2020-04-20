using System;
using System.IO;

public class FileLog
{
	private StreamWriter m_file;

	private object m_lock;

	private DateTime m_logFileCreationTime;

	public FileLog()
	{
		this.MinLevel = Log.Level.Warning;
		this.m_lock = new object();
	}

	public Log.Level MinLevel { get; set; }

	public bool RawLogging { get; set; }

	public bool UseDatedFolder { get; set; }

	public string BaseFilePath { get; private set; }

	public string CurrentFilePath { get; private set; }

	public StreamWriter File
	{
		get
		{
			return this.m_file;
		}
	}

	public bool IsOpen
	{
		get
		{
			return this.m_file != null;
		}
	}

	public static string AsDatedDirectory(string basePath, DateTime dateTime = default(DateTime))
	{
		if (dateTime == default(DateTime))
		{
			dateTime = DateTime.Now;
		}
		return string.Format("{0}/{1:d4}-{2:d2}-{3:d2}", new object[]
		{
			basePath,
			dateTime.Year,
			dateTime.Month,
			dateTime.Day
		});
	}

	public void Open(string filePath)
	{
		object @lock = this.m_lock;
		lock (@lock)
		{
			try
			{
				this.BaseFilePath = filePath;
				this.m_logFileCreationTime = DateTime.Now;
				if (this.UseDatedFolder)
				{
					string text = FileLog.AsDatedDirectory(Path.GetDirectoryName(this.BaseFilePath), this.m_logFileCreationTime);
					Directory.CreateDirectory(text);
					this.CurrentFilePath = string.Format("{0}/{1}", text, Path.GetFileName(this.BaseFilePath));
				}
				else
				{
					this.CurrentFilePath = filePath;
				}
				Directory.CreateDirectory(Path.GetDirectoryName(this.CurrentFilePath));
				FileStream fileStream = new FileStream(this.CurrentFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
				this.m_file = new StreamWriter(fileStream);
				if (fileStream.Length > 0L)
				{
					this.m_file.WriteLine();
					this.m_file.WriteLine();
					this.Write(Log.Level.Notice, "    ***    Logging restarted    ***");
					this.m_file.WriteLine();
					this.m_file.WriteLine();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}

	public void Close()
	{
		object @lock = this.m_lock;
		lock (@lock)
		{
			if (this.m_file != null)
			{
				try
				{
					this.m_file.Close();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
				this.m_file = null;
			}
		}
	}

	public void Register()
	{
		Log.AddLogHandler(new Action<Log.Message>(this.HandleLogMessage));
	}

	public void Unregister()
	{
		Log.RemoveLogHandler(new Action<Log.Message>(this.HandleLogMessage));
	}

	public void HandleLogMessage(Log.Message args)
	{
		object @lock = this.m_lock;
		lock (@lock)
		{
			if (this.m_file != null)
			{
				if (args.level >= this.MinLevel)
				{
					try
					{
						string text = args.ToString();
						if (!text.IsNullOrEmpty())
						{
							if (!this.RawLogging)
							{
								this.m_file.Write(string.Format("{0} [{1}] ", args.timestamp.ToString(Log.TimestampFormat), Log.ToStringCode(args.level)));
							}
							if (args.level >= Log.Level.Warning)
							{
								if (args.level <= Log.Level.Critical)
								{
									if (!args.message.IsNullOrEmpty())
									{
										string[] array = text.Split(new char[]
										{
											'\r',
											'\n'
										}, StringSplitOptions.RemoveEmptyEntries);
										string[] array3;
										string[] array2 = array3 = array;
										int num = 0;
										string str = array3[0];
										string format = " ({0} {1:x8})";
										object arg;
										if (args.level == Log.Level.Warning)
										{
											arg = "warningid";
										}
										else
										{
											arg = "errorid";
										}
										array2[num] = str + string.Format(format, arg, args.message.GetHashCode());
										foreach (string value in array)
										{
											this.m_file.WriteLine(value);
										}
										goto IL_19F;
									}
								}
							}
							this.m_file.WriteLine(text);
							IL_19F:
							this.m_file.Flush();
						}
					}
					catch (Exception value2)
					{
						Console.WriteLine(value2);
					}
				}
			}
		}
	}

	public void Write(Log.Level level, string message)
	{
		Log.Message args = new Log.Message
		{
			level = level,
			message = message,
			formattedMessage = message,
			timestamp = DateTime.Now
		};
		this.HandleLogMessage(args);
	}

	public void Update()
	{
		Log.Update();
		object @lock = this.m_lock;
		lock (@lock)
		{
			try
			{
				if (this.UseDatedFolder)
				{
					if (this.m_file != null)
					{
						if (DateTime.Now.Day != this.m_logFileCreationTime.Day)
						{
							this.Close();
							this.Open(this.BaseFilePath);
							this.Write(Log.Level.Notice, "    ***    Logging continued from previous day    ***");
							this.m_file.WriteLine();
							this.m_file.WriteLine();
						}
					}
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}
	}
}
