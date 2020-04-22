using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;

public static class CrashReportArchiver
{
	private const int MAX_ARCHIVE_SIZE = 4194304;

	internal const int MAX_LOG_SIZE = 262144;

	internal static bool CreateArchiveFromCrashDumpDirectory(out string resultPath, out int resultArchiveNumBytes, string crashDumpDirectoryPath, string persistentDataPath, Dictionary<string, string> userKeyValues, BugReportType bugReportType = BugReportType.Crash)
	{
		resultPath = null;
		resultArchiveNumBytes = 0;
		try
		{
			string text = Path.Combine(crashDumpDirectoryPath, "extra");
			DirectoryInfo directoryInfo = Directory.CreateDirectory(text);
			if (!directoryInfo.Exists)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						Log.Error("Failed to create " + text);
						return false;
					}
				}
			}
			string[] files = Directory.GetFiles(crashDumpDirectoryPath);
			foreach (string text2 in files)
			{
				string fileName = Path.GetFileName(text2);
				string path = fileName.ToLower();
				if (Path.GetFileNameWithoutExtension(path) != "system_info")
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (Path.GetExtension(path) != ".dmp")
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						string destFileName = Path.Combine(text, fileName);
						File.Move(text2, destFileName);
					}
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					files = Directory.GetFiles(text);
					string text3 = null;
					foreach (string text4 in files)
					{
						if (bugReportType != 0)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (bugReportType != BugReportType.Exception)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (bugReportType != BugReportType.Error)
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									continue;
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
									while (true)
									{
										switch (5)
										{
										case 0:
											break;
										default:
										{
											string name = stringBuilder.ToString();
											userKeyValues["StackTraceHash"] = $"{StringUtil.CaseInsensitiveHash(name):X8}";
											goto end_IL_030c;
										}
										}
									}
									end_IL_030c:;
								}
								finally
								{
									if (streamReader != null)
									{
										while (true)
										{
											switch (6)
											{
											case 0:
												break;
											default:
												((IDisposable)streamReader).Dispose();
												goto end_IL_0371;
											}
										}
									}
									end_IL_0371:;
								}
							}
						}
						else if (text4.EndsWith("output_log.txt"))
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
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
										string[] array = text3.Split(',', ' ');
										if (array != null)
										{
											while (true)
											{
												switch (1)
												{
												case 0:
													continue;
												}
												break;
											}
											if (array.Length > 0)
											{
												while (true)
												{
													switch (4)
													{
													case 0:
														continue;
													}
													break;
												}
												if (!string.IsNullOrEmpty(array[0]))
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
													text3 = array[0];
												}
											}
										}
									}
									if (text6.StartsWith("========== OUTPUTING STACK TRACE"))
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
										break;
									}
								}
								while (true)
								{
									if ((text6 = streamReader2.ReadLine()) == null)
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
										break;
									}
									string text7 = text6.Trim();
									if (text7.Length > 0)
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
										if (!text6.StartsWith(" "))
										{
											while (true)
											{
												switch (5)
												{
												case 0:
													continue;
												}
												break;
											}
											stringBuilder2.Append(text7);
										}
									}
									else if (stringBuilder2.Length > 0)
									{
										while (true)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											break;
										}
										break;
									}
								}
								string name2 = stringBuilder2.ToString();
								userKeyValues["StackTraceHash"] = $"{StringUtil.CaseInsensitiveHash(name2):X8}";
							}
							finally
							{
								if (streamReader2 != null)
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
											((IDisposable)streamReader2).Dispose();
											goto end_IL_02d5;
										}
									}
								}
								end_IL_02d5:;
							}
						}
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
						{
							CreateSystemInfoFile(crashDumpDirectoryPath, userKeyValues, bugReportType, text3);
							CompressResult compressResult = CompressDirectoryToPersistentDataPath(out resultPath, out resultArchiveNumBytes, crashDumpDirectoryPath, persistentDataPath, true);
							if (compressResult == CompressResult.FailureExceedsMaxArchiveSize)
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
								compressResult = CompressDirectoryToPersistentDataPath(out resultPath, out resultArchiveNumBytes, crashDumpDirectoryPath, persistentDataPath, false);
							}
							if (compressResult == CompressResult.Success)
							{
								Log.Info("Created crash report archive: " + resultPath);
								return true;
							}
							Log.Error("CrashReportArchiver.CompressDirectoryToPersistentDataPath failed: " + compressResult);
							return false;
						}
						}
					}
				}
				}
			}
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
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
			while (true)
			{
				switch (4)
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
			text = "AtlasReactor_UnityEditor";
		}
		return text;
	}

	private static CompressResult CompressDirectoryToPersistentDataPath(out string resultPath, out int resultArchiveBytes, string sourceDirectoryPath, string destinationDirectoryPath, bool includeLogFiles)
	{
		resultPath = null;
		resultArchiveBytes = 0;
		if (!Directory.Exists(sourceDirectoryPath))
		{
			Log.Warning("Cannot find directory '{0}'", sourceDirectoryPath);
			return CompressResult.Failure;
		}
		if (!Directory.Exists(destinationDirectoryPath))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Log.Warning("Cannot find directory '{0}'", destinationDirectoryPath);
					return CompressResult.Failure;
				}
			}
		}
		try
		{
			string[] files = Directory.GetFiles(sourceDirectoryPath);
			string[] directories = Directory.GetDirectories(sourceDirectoryPath);
			string processName = GetProcessName();
			string path = $"Channel150_{processName}_temp.zip";
			string text = Path.Combine(destinationDirectoryPath, path);
			long num = 0L;
			FileStream fileStream = File.Create(text);
			try
			{
				using (ZipOutputStream zipOutputStream = new ZipOutputStream(fileStream))
				{
					zipOutputStream.SetLevel(9);
					byte[] buffer = new byte[4096];
					string[] array = files;
					foreach (string text2 in array)
					{
						WriteFileToZip(text2, Path.GetFileName(text2), zipOutputStream, buffer);
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							if (includeLogFiles)
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
								foreach (string path2 in directories)
								{
									string fileName = Path.GetFileName(path2);
									string[] files2 = Directory.GetFiles(path2);
									string text3;
									string text4;
									for (int k = 0; k < files2.Length; WriteFileToZip(text3, text4, zipOutputStream, buffer), k++)
									{
										text3 = files2[k];
										text4 = null;
										if (text3.LastIndexOf(".txt") < 0)
										{
											while (true)
											{
												switch (5)
												{
												case 0:
													continue;
												}
												break;
											}
											if (text3.LastIndexOf(".htm") < 0)
											{
												while (true)
												{
													switch (1)
													{
													case 0:
														continue;
													}
													break;
												}
												text4 = Path.Combine(fileName, Path.GetFileName(text3) + ".txt");
												continue;
											}
										}
										text4 = Path.Combine(fileName, Path.GetFileName(text3));
									}
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
											goto end_IL_01c1;
										}
										continue;
										end_IL_01c1:
										break;
									}
								}
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							zipOutputStream.Finish();
							num = fileStream.Length;
							zipOutputStream.Close();
							goto end_IL_00a4;
						}
					}
					end_IL_00a4:;
				}
			}
			finally
			{
				if (fileStream != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							((IDisposable)fileStream).Dispose();
							goto end_IL_020c;
						}
					}
				}
				end_IL_020c:;
			}
			if (num > 4194304)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return CompressResult.FailureExceedsMaxArchiveSize;
					}
				}
			}
			if (num <= 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return CompressResult.Failure;
					}
				}
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
		ZipEntry zipEntry = new ZipEntry(fileZipPath);
		zipEntry.DateTime = DateTime.Now;
		zipOutputStream.PutNextEntry(zipEntry);
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		finally
		{
			if (fileStream != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						((IDisposable)fileStream).Dispose();
						goto end_IL_0050;
					}
				}
			}
			end_IL_0050:;
		}
	}

	private static bool CreateSystemInfoFile(string directoryPath, Dictionary<string, string> userKeyValues, BugReportType bugReportType, string fullVersionString)
	{
		string text = Path.Combine(directoryPath, "system_info.xml");
		Log.Info($"Creating system info file: {text}");
		bool result = true;
		try
		{
			StreamWriter streamWriter = new StreamWriter(text, false, new UnicodeEncoding(false, true));
			try
			{
				streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-16\" standalone=\"no\"?>");
				streamWriter.WriteLine("<CrashCollector xmlns=\"http://www.trionworld.com\">");
				PopulateFiles(streamWriter);
				PopulateProcessInfo(streamWriter, userKeyValues);
				PopulateEnvironmentVariables(streamWriter);
				PopulateSystemInfo(streamWriter, userKeyValues, bugReportType, fullVersionString);
				PopulateKeyValueData(streamWriter, userKeyValues);
				streamWriter.WriteLine("</CrashCollector>");
				return result;
			}
			finally
			{
				if (streamWriter != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							((IDisposable)streamWriter).Dispose();
							goto end_IL_007b;
						}
					}
				}
				end_IL_007b:;
			}
		}
		catch (Exception ex)
		{
			Log.Error($"Error creating system info file. Exception {ex.ToString()}.");
			return false;
		}
	}

	private static void PopulateFiles(StreamWriter text)
	{
		text.WriteLine("  <IncludedFiles>");
		text.WriteLine("    <Item>crash.dmp</Item>");
		text.WriteLine("  </IncludedFiles>");
	}

	private static string TryGetMemoryCounter(Dictionary<string, string> userKeyValues, string key)
	{
		object input;
		if (userKeyValues.TryGetValue(key, out string value))
		{
			while (true)
			{
				switch (1)
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
			if (!string.IsNullOrEmpty(value))
			{
				input = value;
				goto IL_0034;
			}
		}
		input = "0";
		goto IL_0034;
		IL_0034:
		return SanitizeXml((string)input);
	}

	private static void PopulateProcessInfo(StreamWriter text, Dictionary<string, string> userKeyValues)
	{
		text.WriteLine("  <Process>");
		text.WriteLine("    <PageFaultCount>" + TryGetMemoryCounter(userKeyValues, "MemoryPageFaultCount") + "</PageFaultCount>");
		text.WriteLine("    <WorkingSetSize>" + TryGetMemoryCounter(userKeyValues, "MemoryWorkingSetSize") + "</WorkingSetSize>");
		text.WriteLine("    <PeakWorkingSetSize>" + TryGetMemoryCounter(userKeyValues, "MemoryPeakWorkingSetSize") + "</PeakWorkingSetSize>");
		text.WriteLine("    <PagefileUsage>" + TryGetMemoryCounter(userKeyValues, "MemoryPagefileUsage") + "</PagefileUsage>");
		text.WriteLine("    <PeakPagefileUsage>" + TryGetMemoryCounter(userKeyValues, "MemoryPeakPagefileUsage") + "</PeakPagefileUsage>");
		text.WriteLine("    <PrivateUsage>" + TryGetMemoryCounter(userKeyValues, "MemoryPrivateUsage") + "</PrivateUsage>");
		text.WriteLine("    <VirtualMemoryAvailable>" + TryGetMemoryCounter(userKeyValues, "MemoryTotalVirtual") + "</VirtualMemoryAvailable>");
		text.WriteLine("    <VirtualMemoryUsage>" + TryGetMemoryCounter(userKeyValues, "MemoryUsedVirtual") + "</VirtualMemoryUsage>");
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
			while (true)
			{
				switch (4)
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
			fullVersionString = BuildVersion.FullVersionString;
		}
		string[] array = fullVersionString.Split('-');
		if (array != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (array.Length > 1)
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
				if (!string.IsNullOrEmpty(array[1]))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					input = array[1];
				}
			}
		}
		text.WriteLine("  <System>");
		text.WriteLine("    <MachineName>" + SanitizeXml(Environment.MachineName) + "</MachineName>");
		text.WriteLine("    <Channel>" + SanitizeXml("Channel_150") + "</Channel>");
		text.WriteLine("    <BuildVersion>" + SanitizeXml(fullVersionString) + "</BuildVersion>");
		text.WriteLine("    <BuildType>" + SanitizeXml(HydrogenConfig.Get().EnvironmentName) + "</BuildType>");
		string input2 = DateTime.Now.ToString("MM/dd/yy hh:mm:ss");
		text.WriteLine("    <Time>" + SanitizeXml(input2) + "</Time>");
		text.WriteLine("    <BuildNum>" + SanitizeXml(input) + "</BuildNum>");
		text.WriteLine("    <Application>" + SanitizeXml(GetProcessName()) + "</Application>");
		if (bugReportType == BugReportType.Crash)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			text.WriteLine("    <ProcessName>" + SanitizeXml(GetProcessName()) + "</ProcessName>");
		}
		else
		{
			text.WriteLine("    <ProcessName>" + SanitizeXml($"{GetProcessName()}_{bugReportType.ToString()}Report") + "</ProcessName>");
		}
		text.WriteLine("    <CorrelationID>" + SanitizeXml(Guid.NewGuid().ToString()) + "</CorrelationID>");
		object input3;
		if (HydrogenConfig.Get().Ticket == null)
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
			input3 = "NULL";
		}
		else
		{
			input3 = HydrogenConfig.Get().Ticket.UserName;
		}
		text.WriteLine("    <UserID>" + SanitizeXml((string)input3) + "</UserID>");
		text.WriteLine("    <TotalMemory>" + TryGetMemoryCounter(userKeyValues, "MemoryTotalPhysical") + "</TotalMemory>");
		text.WriteLine("    <UsedMemory>" + TryGetMemoryCounter(userKeyValues, "MemoryUsedPhysical") + "</UsedMemory>");
		PopulateNetworkCards(text);
		text.WriteLine("  </System>");
	}

	private static void PopulateNetworkCards(StreamWriter text)
	{
		text.WriteLine("    <NetworkCards>");
		try
		{
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface networkInterface in allNetworkInterfaces)
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
				text.WriteLine("        <Description>" + FormatEscapedXml("{0}", networkInterface.Description) + "</Description>");
				text.WriteLine("      </Card>");
			}
		}
		catch (Exception ex)
		{
			Log.Error("Exception trying to enumerate net cards for exception reporting: {0}:{1}", ex, ex.Message);
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
				KeyValuePair<string, string> current = enumerator.Current;
				text.WriteLine("    <Pair>");
				text.WriteLine("      <Key>" + SanitizeXml(current.Key) + "</Key>");
				text.WriteLine("      <Value>" + SanitizeXml(current.Value) + "</Value>");
				text.WriteLine("    </Pair>");
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					goto end_IL_0014;
				}
			}
			end_IL_0014:;
		}
		text.WriteLine("  </UserKeyValue>");
	}

	private static string SanitizeXml(string input)
	{
		string text = "XML sanitization error";
		try
		{
			text = ((input != null) ? input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;")
				.Replace("\"", "&quot;")
				.Replace("'", "&apos;") : "NULL");
			string pattern = "[^\\x09\\x0A\\x0D\\x20-\\xD7FF\\xE000-\\xFFFD\\x10000-x10FFFF]";
			text = Regex.Replace(text, pattern, string.Empty);
			if (text.Length >= 2048)
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
						text = $"{text.Substring(0, 2044)}...";
						return text;
					}
				}
			}
			return text;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
			return text;
		}
	}

	private static string FormatEscapedXml<T1>(string format, T1 t1)
	{
		return SanitizeXml(string.Format(format, t1));
	}
}
