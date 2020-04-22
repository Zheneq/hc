using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public class ClientCrashReportThreadedJob : ThreadedJob
{
	internal enum State
	{
		CreatingArchive,
		WaitingForClientToConnectToLobbyServer,
		WaitingForArchiveNameResponse,
		MoveArchiveAndReadBytes,
		CleanUp,
		Succeeded,
		Failed,
		Canceled
	}

	private State m_state;

	private EasedFloat m_progress = new EasedFloat(0f);

	private Dictionary<string, string> m_userKeyValues = new Dictionary<string, string>();

	private string m_userMessage;

	private string m_logMessage;

	private string m_outputLogPath;

	private string m_outputLogPathCopy;

	private BugReportType m_bugReportType;

	private string m_temporaryCachePath;

	private string m_crashDumpDirectoryPath;

	private bool m_stateSucceeeded;

	private string m_tempArchivePath;

	private int m_tempArchiveFileSize;

	private float m_progressValue;

	private string m_archiveFileName;

	private byte[] m_crashReportBytes;

	internal bool IsFinished
	{
		get
		{
			switch (m_state)
			{
			case State.Succeeded:
			case State.Failed:
			case State.Canceled:
				return true;
			default:
				return false;
			}
		}
	}

	internal bool IsSucceeded => m_stateSucceeeded;

	internal ClientCrashReportThreadedJob(string crashDumpDirectoryPath, BugReportType bugReportType = BugReportType.Crash, string userMessage = null, string logMessage = null)
	{
		m_userMessage = userMessage;
		m_logMessage = logMessage;
		m_bugReportType = bugReportType;
		try
		{
			m_outputLogPath = Path.Combine(Application.dataPath, "output_log.txt");
			string temporaryCachePath;
			if (Application.temporaryCachePath == null)
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
				temporaryCachePath = string.Empty;
			}
			else
			{
				temporaryCachePath = Application.temporaryCachePath;
			}
			m_temporaryCachePath = temporaryCachePath;
			if (Path.DirectorySeparatorChar == '\\')
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
				m_temporaryCachePath = m_temporaryCachePath.Replace("/", "\\");
			}
			m_outputLogPathCopy = Path.Combine(m_temporaryCachePath, Path.GetFileName(m_outputLogPath));
			m_crashDumpDirectoryPath = crashDumpDirectoryPath;
			try
			{
				long num = SystemInfo.systemMemorySize;
				m_userKeyValues["MemoryTotalPhysical"] = (num * 1048576).ToString();
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				if (commandLineArgs == null)
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
					commandLineArgs = new string[0];
				}
				m_userKeyValues["CommandLine"] = $"\"{string.Join(string.Empty, Environment.GetCommandLineArgs())}\"";
				m_userKeyValues["Language"] = Application.systemLanguage.ToString();
				m_userKeyValues["SettingsPath"] = Application.persistentDataPath;
				m_userKeyValues["SupportsRenderTextureFormat_Depth"] = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth).ToString();
				m_userKeyValues["SupportsRenderTextureFormat_RFloat"] = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat).ToString();
				m_userKeyValues["SupportsRenderTextureFormat_RHalf"] = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RHalf).ToString();
				m_userKeyValues["CurrentResolution"] = Screen.currentResolution.ToString();
				if (Options_UI.Get() != null)
				{
					m_userKeyValues["GraphicsQuality"] = Options_UI.Get().GetCurrentGraphicsQuality().ToString();
					m_userKeyValues["GraphicsQualityEverSetManually"] = Options_UI.Get().GetGraphicsQualityEverSetManually().ToString();
				}
				Resolution[] resolutions = Screen.resolutions;
				if (resolutions != null)
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
					for (int i = 0; i < resolutions.Length; i++)
					{
						m_userKeyValues[$"Resolution{i}"] = resolutions[i].ToString();
					}
				}
				if (userMessage != null)
				{
					m_userKeyValues[" UserMessage"] = userMessage;
				}
				PropertyInfo[] properties = typeof(SystemInfo).GetProperties(BindingFlags.Static | BindingFlags.Public);
				foreach (PropertyInfo propertyInfo in properties)
				{
					m_userKeyValues.Add(propertyInfo.Name, propertyInfo.GetValue(null, null).ToString());
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
			}
			StartThread();
		}
		catch (Exception exception2)
		{
			Log.Exception(exception2);
		}
	}

	internal void Cancel()
	{
		if (ClientGameManager.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ClientGameManager.Get().OnConnectedToLobbyServer -= OnRegisterGameClientResponse;
		}
		try
		{
			Abort();
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		m_state = State.Canceled;
	}

	protected override void ThreadFunction()
	{
		try
		{
			State state = m_state;
			if (state != 0)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (state != State.MoveArchiveAndReadBytes)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									if (state != State.CleanUp)
									{
										while (true)
										{
											switch (6)
											{
											default:
												return;
											case 0:
												break;
											}
										}
									}
									CrashReportArchiver.DeleteArchives(m_temporaryCachePath);
									m_progressValue = 0.95f;
									if (m_stateSucceeeded)
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
										try
										{
											Directory.Delete(m_crashDumpDirectoryPath, true);
										}
										catch (Exception exception)
										{
											Log.Exception(exception);
										}
									}
									m_progressValue = 1f;
									m_stateSucceeeded = true;
									return;
								}
							}
						}
						m_progressValue = 0.4f;
						string text = Path.Combine(m_temporaryCachePath, m_archiveFileName);
						m_progressValue = 0.5f;
						try
						{
							File.Move(m_tempArchivePath, text);
						}
						catch (Exception exception2)
						{
							Log.Exception(exception2);
						}
						m_progressValue = 0.6f;
						if (File.Exists(text))
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									try
									{
										m_crashReportBytes = File.ReadAllBytes(text);
										m_stateSucceeeded = (m_crashReportBytes != null && m_crashReportBytes.Length > 0);
									}
									catch (Exception exception3)
									{
										Log.Exception(exception3);
										m_stateSucceeeded = false;
									}
									return;
								}
							}
						}
						Log.Error("Failed to move file {0} to {1}", m_tempArchivePath, text);
						m_state = State.Failed;
						return;
					}
					}
				}
			}
			m_progressValue = 0.05f;
			m_stateSucceeeded = true;
			if (!Directory.Exists(m_crashDumpDirectoryPath))
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
				if (m_bugReportType != 0)
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
					try
					{
						DirectoryInfo directoryInfo = Directory.CreateDirectory(m_crashDumpDirectoryPath);
						if (directoryInfo != null)
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
							if (directoryInfo.Exists)
							{
								goto IL_00d6;
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
						Log.Error("Failed to create temp directory for user bug report at {0}", m_crashDumpDirectoryPath);
						m_stateSucceeeded = false;
						goto IL_00d6;
						IL_00d6:
						using (new StreamWriter(m_crashDumpDirectoryPath + "\\crash.dmp"))
						{
						}
					}
					catch (Exception exception4)
					{
						Log.Exception(exception4);
						m_stateSucceeeded = false;
					}
				}
				else
				{
					Log.Error("Directory does not exist for crash report {0}", m_crashDumpDirectoryPath);
					m_stateSucceeeded = false;
				}
			}
			if (m_stateSucceeeded)
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
				if (!string.IsNullOrEmpty(m_userMessage))
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
					try
					{
						StreamWriter streamWriter2 = new StreamWriter(m_crashDumpDirectoryPath + "\\UserMessage.txt");
						try
						{
							streamWriter2.Write(m_userMessage);
						}
						finally
						{
							if (streamWriter2 != null)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										((IDisposable)streamWriter2).Dispose();
										goto end_IL_0187;
									}
								}
							}
							end_IL_0187:;
						}
					}
					catch (Exception exception5)
					{
						Log.Exception(exception5);
						int stateSucceeeded;
						if (m_bugReportType != BugReportType.Bug)
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
							stateSucceeeded = ((m_bugReportType != BugReportType.Exception) ? 1 : 0);
						}
						else
						{
							stateSucceeeded = 0;
						}
						m_stateSucceeeded = ((byte)stateSucceeeded != 0);
					}
				}
			}
			if (m_stateSucceeeded)
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
				if (m_bugReportType != 0)
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
					try
					{
						StringBuilder stringBuilder = new StringBuilder((m_logMessage != null) ? (m_logMessage + "\n") : string.Empty);
						if (File.Exists(m_outputLogPath))
						{
							File.Copy(m_outputLogPath, m_outputLogPathCopy, true);
							if (File.Exists(m_outputLogPathCopy))
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
									{
										StreamReader streamReader = File.OpenText(m_outputLogPathCopy);
										try
										{
											if (streamReader.BaseStream.Length <= 262144)
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
												streamReader.BaseStream.Seek(-streamReader.BaseStream.Length, SeekOrigin.End);
											}
											else
											{
												streamReader.BaseStream.Seek(-262144L, SeekOrigin.End);
												stringBuilder.AppendFormat("(Skipped {0} bytes)\n", streamReader.BaseStream.Length - 262144);
											}
											stringBuilder.Append(streamReader.ReadToEnd());
										}
										finally
										{
											if (streamReader != null)
											{
												while (true)
												{
													switch (2)
													{
													case 0:
														break;
													default:
														((IDisposable)streamReader).Dispose();
														goto end_IL_030a;
													}
												}
											}
											end_IL_030a:;
										}
										StreamWriter streamWriter3 = new StreamWriter(Path.Combine(m_crashDumpDirectoryPath, Path.GetFileName(m_outputLogPath)));
										try
										{
											streamWriter3.Write(stringBuilder.ToString());
										}
										finally
										{
											if (streamWriter3 != null)
											{
												while (true)
												{
													switch (5)
													{
													case 0:
														break;
													default:
														((IDisposable)streamWriter3).Dispose();
														goto end_IL_034f;
													}
												}
											}
											end_IL_034f:;
										}
										goto end_IL_01fc;
									}
									}
								}
							}
						}
						end_IL_01fc:;
					}
					catch (Exception exception6)
					{
						Log.Exception(exception6);
					}
				}
			}
			if (m_stateSucceeeded)
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
				try
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					if (ClientBootstrap.Instance != null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								if (File.Exists(ClientBootstrap.Instance.GetFileLogCurrentPath()))
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											break;
										default:
										{
											string path = "AtlasReactor.txt";
											string text2 = Path.Combine(m_temporaryCachePath, path);
											File.Copy(ClientBootstrap.Instance.GetFileLogCurrentPath(), text2, true);
											if (File.Exists(text2))
											{
												while (true)
												{
													switch (1)
													{
													case 0:
														break;
													default:
													{
														StreamReader streamReader2 = File.OpenText(text2);
														try
														{
															if (streamReader2.BaseStream.Length <= 262144)
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
																streamReader2.BaseStream.Seek(-streamReader2.BaseStream.Length, SeekOrigin.End);
															}
															else
															{
																streamReader2.BaseStream.Seek(-262144L, SeekOrigin.End);
																stringBuilder2.AppendFormat("(Skipped {0} bytes)\n", streamReader2.BaseStream.Length - 262144);
															}
															stringBuilder2.Append(streamReader2.ReadToEnd());
														}
														finally
														{
															if (streamReader2 != null)
															{
																while (true)
																{
																	switch (4)
																	{
																	case 0:
																		break;
																	default:
																		((IDisposable)streamReader2).Dispose();
																		goto end_IL_04b0;
																	}
																}
															}
															end_IL_04b0:;
														}
														StreamWriter streamWriter4 = new StreamWriter(Path.Combine(m_crashDumpDirectoryPath, path));
														try
														{
															streamWriter4.Write(stringBuilder2.ToString());
														}
														finally
														{
															if (streamWriter4 != null)
															{
																while (true)
																{
																	switch (5)
																	{
																	case 0:
																		break;
																	default:
																		((IDisposable)streamWriter4).Dispose();
																		goto end_IL_04ee;
																	}
																}
															}
															end_IL_04ee:;
														}
														goto end_IL_0387;
													}
													}
												}
											}
											goto end_IL_0387;
										}
										}
									}
								}
								goto end_IL_0387;
							}
						}
					}
					end_IL_0387:;
				}
				catch (Exception exception7)
				{
					Log.Exception(exception7);
				}
			}
			if (m_stateSucceeeded)
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
				m_stateSucceeeded = CrashReportArchiver.CreateArchiveFromCrashDumpDirectory(out m_tempArchivePath, out m_tempArchiveFileSize, m_crashDumpDirectoryPath, m_temporaryCachePath, m_userKeyValues, m_bugReportType);
			}
			m_progressValue = 0.2f;
		}
		catch (Exception exception8)
		{
			Log.Exception(exception8);
			m_state = State.Failed;
		}
	}

	protected override void OnThreadFunctionReturned()
	{
		switch (m_state)
		{
		case State.Failed:
		case State.Canceled:
			break;
		case State.CreatingArchive:
			if (m_stateSucceeeded)
			{
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
						if (!ClientGameManager.Get().IsConnectedToLobbyServer)
						{
							ClientGameManager.Get().OnConnectedToLobbyServer += OnRegisterGameClientResponse;
							m_state = State.WaitingForClientToConnectToLobbyServer;
						}
						else
						{
							RequestArchiveName();
						}
						return;
					}
				}
			}
			m_state = State.Failed;
			break;
		case State.MoveArchiveAndReadBytes:
			if (m_stateSucceeeded)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						Log.Info("Move and read of {0} succeeded: {1}", m_archiveFileName, m_stateSucceeeded);
						try
						{
							Log.Info("Attempting to build URL to send crash report {0}", m_archiveFileName);
							string archiveFileName = m_archiveFileName;
							string crashServerAndArchiveURL = $"http://debug.triongames.com/v2/archive/{archiveFileName}";
							ClientCrashReportDetector.Get().UploadArchive(crashServerAndArchiveURL, m_crashReportBytes, OnUploadArchiveEndStatus);
						}
						catch (Exception exception)
						{
							Log.Exception(exception);
						}
						return;
					}
				}
			}
			m_state = State.Failed;
			break;
		case State.CleanUp:
			if (m_stateSucceeeded)
			{
				Log.Info("Clean up of {0} succeeded: {1}", m_archiveFileName, m_stateSucceeeded);
				m_state = State.Succeeded;
			}
			else
			{
				m_state = State.Failed;
			}
			break;
		default:
			m_state = State.Failed;
			Log.Error("Unexpected state for completed worker thread: {0}", m_state.ToString());
			break;
		}
	}

	private void RequestArchiveName()
	{
		if (ClientGameManager.Get().RequestCrashReportArchiveName(m_tempArchiveFileSize, OnCrashReportArchiveNameResponse))
		{
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
					m_state = State.WaitingForArchiveNameResponse;
					return;
				}
			}
		}
		Log.Error("Failed to request crash report archive name");
		m_state = State.Failed;
	}

	private void OnCrashReportArchiveNameResponse(CrashReportArchiveNameResponse response)
	{
		if (response.Success)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					State state = m_state;
					if (state != State.Failed)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								if (state != State.Canceled)
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											break;
										default:
											if (state != State.WaitingForArchiveNameResponse)
											{
												while (true)
												{
													switch (6)
													{
													case 0:
														break;
													default:
														Log.Error("Unexpected worker thread state when receiving archive name: {0}", m_state.ToString());
														Cancel();
														m_state = State.Failed;
														return;
													}
												}
											}
											if (base.IsThreadAlive)
											{
												Log.Error("Unexpected busy worker thread when receiving archive name");
												Cancel();
												m_state = State.Failed;
											}
											else if (string.IsNullOrEmpty(response.ArchiveName))
											{
												Log.Error("Unexpected empty archive name received");
												Cancel();
												m_state = State.Failed;
											}
											else
											{
												m_archiveFileName = string.Copy(response.ArchiveName);
												m_state = State.MoveArchiveAndReadBytes;
												StartThread();
											}
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
		Log.Error("Crash archive naming failed: {0}", response.ErrorMessage);
		Cancel();
		m_state = State.Failed;
	}

	private void OnRegisterGameClientResponse(RegisterGameClientResponse response)
	{
		if (!response.Success)
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
					Log.Error("Client encountered an error while connecting to lobby server {0}", response.ErrorMessage);
					Cancel();
					m_state = State.Failed;
					return;
				}
			}
		}
		ClientGameManager.Get().OnConnectedToLobbyServer -= OnRegisterGameClientResponse;
		State state = m_state;
		if (state == State.WaitingForClientToConnectToLobbyServer)
		{
			RequestArchiveName();
			return;
		}
		Log.Error("Unexpected worker thread state when receiving lobby server connect: {0}", m_state.ToString());
		Cancel();
		m_state = State.Failed;
	}

	private void OnUploadArchiveEndStatus(bool success)
	{
		if (success)
		{
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
					m_state = State.CleanUp;
					StartThread();
					return;
				}
			}
		}
		m_state = State.Failed;
	}
}
