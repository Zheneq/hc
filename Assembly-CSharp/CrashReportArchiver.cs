using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip;

public static class CrashReportArchiver
{
	private const int MAX_ARCHIVE_SIZE = 0x400000;

	internal const int MAX_LOG_SIZE = 0x40000;

	internal unsafe static bool CreateArchiveFromCrashDumpDirectory(out string resultPath, out int resultArchiveNumBytes, string crashDumpDirectoryPath, string persistentDataPath, Dictionary<string, string> userKeyValues, BugReportType bugReportType = BugReportType.Crash)
	{
		resultPath = null;
		resultArchiveNumBytes = 0;
		try
		{
			string text = Path.Combine(crashDumpDirectoryPath, "extra");
			DirectoryInfo directoryInfo = Directory.CreateDirectory(text);
			if (!directoryInfo.Exists)
			{
				Log.Error("Failed to create " + text, new object[0]);
				return false;
			}
			foreach (string text2 in Directory.GetFiles(crashDumpDirectoryPath))
			{
				string fileName = Path.GetFileName(text2);
				string path = fileName.ToLower();
				if (Path.GetFileNameWithoutExtension(path) != "system_info")
				{
					if (Path.GetExtension(path) != ".dmp")
					{
						string destFileName = Path.Combine(text, fileName);
						File.Move(text2, destFileName);
					}
				}
			}
			string[] files = Directory.GetFiles(text);
			string text3 = null;
			foreach (string text4 in files)
			{
				if (bugReportType != BugReportType.Crash)
				{
					if (bugReportType != BugReportType.Exception)
					{
						if (bugReportType != BugReportType.Error)
						{
							goto IL_389;
						}
					}
					if (text4.EndsWith("UserMessage.txt"))
					{
						StreamReader streamReader = new StreamReader(text4);
						try
						{
							StringBuilder stringBuilder = new StringBuilder();
							string text5;
							while ((text5 = streamReader.ReadLine()) != null)
							{
								string value = text5.Trim();
								stringBuilder.Append(value);
							}
							string name = stringBuilder.ToString();
							userKeyValues["StackTraceHash"] = string.Format("{0:X8}", StringUtil.CaseInsensitiveHash(name));
						}
						finally
						{
							if (streamReader != null)
							{
								((IDisposable)streamReader).Dispose();
							}
						}
					}
				}
				else
				{
					if (text4.EndsWith("output_log.txt"))
					{
						text3 = string.Empty;
						StreamReader streamReader2 = new StreamReader(text4);
						try
						{
							StringBuilder stringBuilder2 = new StringBuilder();
							string text6;
							while ((text6 = streamReader2.ReadLine()) != null)
							{
								if (!string.IsNullOrEmpty("Hydrogen Version: ") && text6.StartsWith("Hydrogen Version: "))
								{
									text3 = text6.Substring("Hydrogen Version: ".Length);
									string[] array = text3.Split(new char[]
									{
										',',
										' '
									});
									if (array != null)
									{
										if (array.Length > 0)
										{
											if (!string.IsNullOrEmpty(array[0]))
											{
												text3 = array[0];
											}
										}
									}
								}
								if (text6.StartsWith("========== OUTPUTING STACK TRACE"))
								{
									break;
								}
							}
							while ((text6 = streamReader2.ReadLine()) != null)
							{
								string text7 = text6.Trim();
								if (text7.Length > 0)
								{
									if (!text6.StartsWith(" "))
									{
										stringBuilder2.Append(text7);
									}
								}
								else if (stringBuilder2.Length > 0)
								{
									break;
								}
							}
							string name2 = stringBuilder2.ToString();
							userKeyValues["StackTraceHash"] = string.Format("{0:X8}", StringUtil.CaseInsensitiveHash(name2));
							goto IL_2EB;
						}
						finally
						{
							if (streamReader2 != null)
							{
								((IDisposable)streamReader2).Dispose();
							}
						}
					}
					IL_2EB:;
				}
				IL_389:;
			}
			CrashReportArchiver.CreateSystemInfoFile(crashDumpDirectoryPath, userKeyValues, bugReportType, text3);
			CompressResult compressResult = CrashReportArchiver.CompressDirectoryToPersistentDataPath(out resultPath, out resultArchiveNumBytes, crashDumpDirectoryPath, persistentDataPath, true);
			if (compressResult == CompressResult.FailureExceedsMaxArchiveSize)
			{
				compressResult = CrashReportArchiver.CompressDirectoryToPersistentDataPath(out resultPath, out resultArchiveNumBytes, crashDumpDirectoryPath, persistentDataPath, false);
			}
			if (compressResult != CompressResult.Success)
			{
				Log.Error("CrashReportArchiver.CompressDirectoryToPersistentDataPath failed: " + compressResult.ToString(), new object[0]);
				return false;
			}
			Log.Info("Created crash report archive: " + resultPath, new object[0]);
			return true;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		return false;
	}

	internal static void DeleteArchives(string persistentDataPath)
	{
		try
		{
			FileInfo[] files = new DirectoryInfo(persistentDataPath).GetFiles("Channel150*.zip");
			for (int i = 0; i < files.Length; i++)
			{
				try
				{
					files[i].Delete();
				}
				catch (Exception exception)
				{
					Log.Exception(exception);
				}
			}
		}
		catch (Exception exception2)
		{
			Log.Exception(exception2);
		}
	}

	private static string GetProcessName()
	{
		string text = Process.GetCurrentProcess().ProcessName;
		if (text == "Unity")
		{
			text = "AtlasReactor_UnityEditor";
		}
		return text;
	}

	private unsafe static CompressResult CompressDirectoryToPersistentDataPath(out string resultPath, out int resultArchiveBytes, string sourceDirectoryPath, string destinationDirectoryPath, bool includeLogFiles)
	{
		resultPath = null;
		resultArchiveBytes = 0;
		if (!Directory.Exists(sourceDirectoryPath))
		{
			Log.Warning("Cannot find directory '{0}'", new object[]
			{
				sourceDirectoryPath
			});
			return CompressResult.Failure;
		}
		if (!Directory.Exists(destinationDirectoryPath))
		{
			Log.Warning("Cannot find directory '{0}'", new object[]
			{
				destinationDirectoryPath
			});
			return CompressResult.Failure;
		}
		try
		{
			string[] files = Directory.GetFiles(sourceDirectoryPath);
			string[] directories = Directory.GetDirectories(sourceDirectoryPath);
			string processName = CrashReportArchiver.GetProcessName();
			string path = string.Format("Channel150_{0}_temp.zip", processName);
			string text = Path.Combine(destinationDirectoryPath, path);
			long num = 0L;
			FileStream fileStream = File.Create(text);
			try
			{
				using (ZipOutputStream zipOutputStream = new ZipOutputStream(fileStream))
				{
					zipOutputStream.SetLevel(9);
					byte[] buffer = new byte[0x1000];
					foreach (string text2 in files)
					{
						CrashReportArchiver.WriteFileToZip(text2, Path.GetFileName(text2), zipOutputStream, buffer);
					}
					if (includeLogFiles)
					{
						foreach (string path2 in directories)
						{
							string fileName = Path.GetFileName(path2);
							string[] files2 = Directory.GetFiles(path2);
							int k = 0;
							while (k < files2.Length)
							{
								string text3 = files2[k];
								if (text3.LastIndexOf(".txt") >= 0)
								{
									goto IL_191;
								}
								if (text3.LastIndexOf(".htm") >= 0)
								{
									goto IL_191;
								}
								string fileZipPath = Path.Combine(fileName, Path.GetFileName(text3) + ".txt");
								IL_1A3:
								CrashReportArchiver.WriteFileToZip(text3, fileZipPath, zipOutputStream, buffer);
								k++;
								continue;
								IL_191:
								fileZipPath = Path.Combine(fileName, Path.GetFileName(text3));
								goto IL_1A3;
							}
						}
					}
					zipOutputStream.Finish();
					num = fileStream.Length;
					zipOutputStream.Close();
				}
			}
			finally
			{
				if (fileStream != null)
				{
					((IDisposable)fileStream).Dispose();
				}
			}
			if (num > 0x400000L)
			{
				return CompressResult.FailureExceedsMaxArchiveSize;
			}
			if (num <= 0L)
			{
				return CompressResult.Failure;
			}
			resultPath = text;
			resultArchiveBytes = (int)num;
			return CompressResult.Success;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		return CompressResult.Failure;
	}

	private static void WriteFileToZip(string file, string fileZipPath, ZipOutputStream zipOutputStream, byte[] buffer)
	{
		zipOutputStream.PutNextEntry(new ZipEntry(fileZipPath)
		{
			DateTime = DateTime.Now
		});
		FileStream fileStream = File.OpenRead(file);
		try
		{
			int num;
			do
			{
				num = fileStream.Read(buffer, 0, buffer.Length);
				zipOutputStream.Write(buffer, 0, num);
			}
			while (num > 0);
		}
		finally
		{
			if (fileStream != null)
			{
				((IDisposable)fileStream).Dispose();
			}
		}
	}

	private static bool CreateSystemInfoFile(string directoryPath, Dictionary<string, string> userKeyValues, BugReportType bugReportType, string fullVersionString)
	{
		string text = Path.Combine(directoryPath, "system_info.xml");
		Log.Info(string.Format("Creating system info file: {0}", text), new object[0]);
		bool result = true;
		try
		{
			StreamWriter streamWriter = new StreamWriter(text, false, new UnicodeEncoding(false, true));
			try
			{
				streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-16\" standalone=\"no\"?>");
				streamWriter.WriteLine("<CrashCollector xmlns=\"http://www.trionworld.com\">");
				CrashReportArchiver.PopulateFiles(streamWriter);
				CrashReportArchiver.PopulateProcessInfo(streamWriter, userKeyValues);
				CrashReportArchiver.PopulateEnvironmentVariables(streamWriter);
				CrashReportArchiver.PopulateSystemInfo(streamWriter, userKeyValues, bugReportType, fullVersionString);
				CrashReportArchiver.PopulateKeyValueData(streamWriter, userKeyValues);
				streamWriter.WriteLine("</CrashCollector>");
			}
			finally
			{
				if (streamWriter != null)
				{
					((IDisposable)streamWriter).Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(string.Format("Error creating system info file. Exception {0}.", ex.ToString()), new object[0]);
			result = false;
		}
		return result;
	}

	private static void PopulateFiles(StreamWriter text)
	{
		text.WriteLine("  <IncludedFiles>");
		text.WriteLine("    <Item>crash.dmp</Item>");
		text.WriteLine("  </IncludedFiles>");
	}

	private static string TryGetMemoryCounter(Dictionary<string, string> userKeyValues, string key)
	{
		string text;
		bool flag = userKeyValues.TryGetValue(key, out text);
		string input;
		if (flag)
		{
			if (!string.IsNullOrEmpty(text))
			{
				input = text;
				goto IL_34;
			}
		}
		input = "0";
		IL_34:
		return CrashReportArchiver.SanitizeXml(input);
	}

	private static void PopulateProcessInfo(StreamWriter text, Dictionary<string, string> userKeyValues)
	{
		text.WriteLine("  <Process>");
		text.WriteLine("    <PageFaultCount>" + CrashReportArchiver.TryGetMemoryCounter(userKeyValues, "MemoryPageFaultCount") + "</PageFaultCount>");
		text.WriteLine("    <WorkingSetSize>" + CrashReportArchiver.TryGetMemoryCounter(userKeyValues, "MemoryWorkingSetSize") + "</WorkingSetSize>");
		text.WriteLine("    <PeakWorkingSetSize>" + CrashReportArchiver.TryGetMemoryCounter(userKeyValues, "MemoryPeakWorkingSetSize") + "</PeakWorkingSetSize>");
		text.WriteLine("    <PagefileUsage>" + CrashReportArchiver.TryGetMemoryCounter(userKeyValues, "MemoryPagefileUsage") + "</PagefileUsage>");
		text.WriteLine("    <PeakPagefileUsage>" + CrashReportArchiver.TryGetMemoryCounter(userKeyValues, "MemoryPeakPagefileUsage") + "</PeakPagefileUsage>");
		text.WriteLine("    <PrivateUsage>" + CrashReportArchiver.TryGetMemoryCounter(userKeyValues, "MemoryPrivateUsage") + "</PrivateUsage>");
		text.WriteLine("    <VirtualMemoryAvailable>" + CrashReportArchiver.TryGetMemoryCounter(userKeyValues, "MemoryTotalVirtual") + "</VirtualMemoryAvailable>");
		text.WriteLine("    <VirtualMemoryUsage>" + CrashReportArchiver.TryGetMemoryCounter(userKeyValues, "MemoryUsedVirtual") + "</VirtualMemoryUsage>");
		text.WriteLine("  </Process>");
	}

	private static void PopulateEnvironmentVariables(StreamWriter text)
	{
		text.WriteLine("  <Environment>");
		text.WriteLine("    <Variables>");
		text.WriteLine("    </Variables>");
		text.WriteLine("  </Environment>");
	}

	private static void PopulateSystemInfo(StreamWriter text, Dictionary<string, string> userKeyValues, BugReportType bugReportType, string fullVersionString)
	{
		string input = string.Empty;
		if (fullVersionString == null)
		{
			fullVersionString = BuildVersion.FullVersionString;
		}
		string[] array = fullVersionString.Split(new char[]
		{
			'-'
		});
		if (array != null)
		{
			if (array.Length > 1)
			{
				if (!string.IsNullOrEmpty(array[1]))
				{
					input = array[1];
				}
			}
		}
		text.WriteLine("  <System>");
		text.WriteLine("    <MachineName>" + CrashReportArchiver.SanitizeXml(Environment.MachineName) + "</MachineName>");
		text.WriteLine("    <Channel>" + CrashReportArchiver.SanitizeXml("Channel_150") + "</Channel>");
		text.WriteLine("    <BuildVersion>" + CrashReportArchiver.SanitizeXml(fullVersionString) + "</BuildVersion>");
		text.WriteLine("    <BuildType>" + CrashReportArchiver.SanitizeXml(HydrogenConfig.Get().EnvironmentName) + "</BuildType>");
		string input2 = DateTime.Now.ToString("MM/dd/yy hh:mm:ss");
		text.WriteLine("    <Time>" + CrashReportArchiver.SanitizeXml(input2) + "</Time>");
		text.WriteLine("    <BuildNum>" + CrashReportArchiver.SanitizeXml(input) + "</BuildNum>");
		text.WriteLine("    <Application>" + CrashReportArchiver.SanitizeXml(CrashReportArchiver.GetProcessName()) + "</Application>");
		if (bugReportType == BugReportType.Crash)
		{
			text.WriteLine("    <ProcessName>" + CrashReportArchiver.SanitizeXml(CrashReportArchiver.GetProcessName()) + "</ProcessName>");
		}
		else
		{
			text.WriteLine("    <ProcessName>" + CrashReportArchiver.SanitizeXml(string.Format("{0}_{1}Report", CrashReportArchiver.GetProcessName(), bugReportType.ToString())) + "</ProcessName>");
		}
		text.WriteLine("    <CorrelationID>" + CrashReportArchiver.SanitizeXml(Guid.NewGuid().ToString()) + "</CorrelationID>");
		string str = "    <UserID>";
		string input3;
		if (HydrogenConfig.Get().Ticket == null)
		{
			input3 = "NULL";
		}
		else
		{
			input3 = HydrogenConfig.Get().Ticket.UserName;
		}
		text.WriteLine(str + CrashReportArchiver.SanitizeXml(input3) + "</UserID>");
		text.WriteLine("    <TotalMemory>" + CrashReportArchiver.TryGetMemoryCounter(userKeyValues, "MemoryTotalPhysical") + "</TotalMemory>");
		text.WriteLine("    <UsedMemory>" + CrashReportArchiver.TryGetMemoryCounter(userKeyValues, "MemoryUsedPhysical") + "</UsedMemory>");
		CrashReportArchiver.PopulateNetworkCards(text);
		text.WriteLine("  </System>");
	}

	private static void PopulateNetworkCards(StreamWriter text)
	{
		text.WriteLine("    <NetworkCards>");
		try
		{
			foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
			{
				text.WriteLine("      <Card>");
				text.WriteLine("        <Address>");
				PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
				byte[] addressBytes = physicalAddress.GetAddressBytes();
				for (int j = 0; j < addressBytes.Length; j++)
				{
					text.Write(addressBytes[j].ToString("x2"));
				}
				text.WriteLine("</Address>");
				text.WriteLine("        <Description>" + CrashReportArchiver.FormatEscapedXml<string>("{0}", networkInterface.Description) + "</Description>");
				text.WriteLine("      </Card>");
			}
		}
		catch (Exception ex)
		{
			Log.Error("Exception trying to enumerate net cards for exception reporting: {0}:{1}", new object[]
			{
				ex,
				ex.Message
			});
		}
		text.WriteLine("    </NetworkCards>");
	}

	private static void PopulateKeyValueData(StreamWriter text, Dictionary<string, string> userKeyValues)
	{
		text.WriteLine("  <UserKeyValue>");
		using (Dictionary<string, string>.Enumerator enumerator = userKeyValues.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> keyValuePair = enumerator.Current;
				text.WriteLine("    <Pair>");
				text.WriteLine("      <Key>" + CrashReportArchiver.SanitizeXml(keyValuePair.Key) + "</Key>");
				text.WriteLine("      <Value>" + CrashReportArchiver.SanitizeXml(keyValuePair.Value) + "</Value>");
				text.WriteLine("    </Pair>");
			}
		}
		text.WriteLine("  </UserKeyValue>");
	}

	private static string SanitizeXml(string input)
	{
		string text = "XML sanitization error";
		try
		{
			text = ((input != null) ? input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;") : "NULL");
			string pattern = "[^\\x09\\x0A\\x0D\\x20-\\xD7FF\\xE000-\\xFFFD\\x10000-x10FFFF]";
			text = Regex.Replace(text, pattern, string.Empty);
			if (text.Length >= 0x800)
			{
				text = string.Format("{0}...", text.Substring(0, 0x7FC));
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		return text;
	}

	private static string FormatEscapedXml<T1>(string format, T1 t1)
	{
		return CrashReportArchiver.SanitizeXml(string.Format(format, t1));
	}
}
