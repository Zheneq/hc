using System;
using System.IO;

public class FileLog
{
	private StreamWriter m_file;

	private object m_lock;

	private DateTime m_logFileCreationTime;

	public Log.Level MinLevel
	{
		get;
		set;
	}

	public bool RawLogging
	{
		get;
		set;
	}

	public bool UseDatedFolder
	{
		get;
		set;
	}

	public string BaseFilePath
	{
		get;
		private set;
	}

	public string CurrentFilePath
	{
		get;
		private set;
	}

	public StreamWriter File => m_file;

	public bool IsOpen => m_file != null;

	public FileLog()
	{
		MinLevel = Log.Level.Warning;
		m_lock = new object();
	}

	public static string AsDatedDirectory(string basePath, DateTime dateTime = default(DateTime))
	{
		if (dateTime == default(DateTime))
		{
			dateTime = DateTime.Now;
		}
		return $"{basePath}/{dateTime.Year:d4}-{dateTime.Month:d2}-{dateTime.Day:d2}";
	}

	public void Open(string filePath)
	{
		lock (m_lock)
		{
			try
			{
				BaseFilePath = filePath;
				m_logFileCreationTime = DateTime.Now;
				if (UseDatedFolder)
				{
					string text = AsDatedDirectory(Path.GetDirectoryName(BaseFilePath), m_logFileCreationTime);
					Directory.CreateDirectory(text);
					CurrentFilePath = $"{text}/{Path.GetFileName(BaseFilePath)}";
				}
				else
				{
					CurrentFilePath = filePath;
				}
				Directory.CreateDirectory(Path.GetDirectoryName(CurrentFilePath));
				FileStream fileStream = new FileStream(CurrentFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
				m_file = new StreamWriter(fileStream);
				if (fileStream.Length > 0)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							m_file.WriteLine();
							m_file.WriteLine();
							Write(Log.Level.Notice, "    ***    Logging restarted    ***");
							m_file.WriteLine();
							m_file.WriteLine();
							return;
						}
					}
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
		lock (m_lock)
		{
			if (m_file != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						try
						{
							m_file.Close();
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.Message);
						}
						m_file = null;
						return;
					}
				}
			}
		}
	}

	public void Register()
	{
		Log.AddLogHandler(HandleLogMessage);
	}

	public void Unregister()
	{
		Log.RemoveLogHandler(HandleLogMessage);
	}

	public void HandleLogMessage(Log.Message args)
	{
		lock (m_lock)
		{
			if (m_file != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (args.level >= MinLevel)
						{
							try
							{
								string text = args.ToString();
								if (!text.IsNullOrEmpty())
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											{
												if (!RawLogging)
												{
													m_file.Write($"{args.timestamp.ToString(Log.TimestampFormat)} [{Log.ToStringCode(args.level)}] ");
												}
												if (args.level >= Log.Level.Warning)
												{
													if (args.level <= Log.Level.Critical)
													{
														if (!args.message.IsNullOrEmpty())
														{
															string[] array = text.Split(new char[2]
															{
																'\r',
																'\n'
															}, StringSplitOptions.RemoveEmptyEntries);
															string[] array2;
															string[] array3 = array2 = array;
															string str = array2[0];
															object arg;
															if (args.level == Log.Level.Warning)
															{
																arg = "warningid";
															}
															else
															{
																arg = "errorid";
															}
															array3[0] = str + $" ({arg} {args.message.GetHashCode():x8})";
															string[] array4 = array;
															foreach (string value in array4)
															{
																m_file.WriteLine(value);
															}
															goto IL_019f;
														}
													}
												}
												m_file.WriteLine(text);
												goto IL_019f;
											}
											IL_019f:
											m_file.Flush();
											return;
										}
									}
								}
							}
							catch (Exception value2)
							{
								Console.WriteLine(value2);
							}
						}
						return;
					}
				}
			}
		}
	}

	public void Write(Log.Level level, string message)
	{
		Log.Message message2 = default(Log.Message);
		message2.level = level;
		message2.message = message;
		message2.formattedMessage = message;
		message2.timestamp = DateTime.Now;
		Log.Message args = message2;
		HandleLogMessage(args);
	}

	public void Update()
	{
		Log.Update();
		lock (m_lock)
		{
			try
			{
				if (UseDatedFolder)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							if (m_file != null)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										if (DateTime.Now.Day != m_logFileCreationTime.Day)
										{
											while (true)
											{
												switch (4)
												{
												case 0:
													break;
												default:
													Close();
													Open(BaseFilePath);
													Write(Log.Level.Notice, "    ***    Logging continued from previous day    ***");
													m_file.WriteLine();
													m_file.WriteLine();
													return;
												}
											}
										}
										return;
									}
								}
							}
							return;
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
