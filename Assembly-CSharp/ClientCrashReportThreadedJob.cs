using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using LobbyGameClientMessages;
using UnityEngine;

public class ClientCrashReportThreadedJob : ThreadedJob
{
	private ClientCrashReportThreadedJob.State m_state;

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

	internal ClientCrashReportThreadedJob(string crashDumpDirectoryPath, BugReportType bugReportType = BugReportType.Crash, string userMessage = null, string logMessage = null)
	{
		this.m_userMessage = userMessage;
		this.m_logMessage = logMessage;
		this.m_bugReportType = bugReportType;
		try
		{
			this.m_outputLogPath = Path.Combine(Application.dataPath, "output_log.txt");
			string temporaryCachePath;
			if (Application.temporaryCachePath == null)
			{
				temporaryCachePath = string.Empty;
			}
			else
			{
				temporaryCachePath = Application.temporaryCachePath;
			}
			this.m_temporaryCachePath = temporaryCachePath;
			if (Path.DirectorySeparatorChar == '\\')
			{
				this.m_temporaryCachePath = this.m_temporaryCachePath.Replace("/", "\\");
			}
			this.m_outputLogPathCopy = Path.Combine(this.m_temporaryCachePath, Path.GetFileName(this.m_outputLogPath));
			this.m_crashDumpDirectoryPath = crashDumpDirectoryPath;
			try
			{
				long num = (long)SystemInfo.systemMemorySize;
				num *= 0x100000L;
				this.m_userKeyValues["MemoryTotalPhysical"] = num.ToString();
				if (Environment.GetCommandLineArgs() == null)
				{
					string[] array = new string[0];
				}
				this.m_userKeyValues["CommandLine"] = string.Format("\"{0}\"", string.Join(string.Empty, Environment.GetCommandLineArgs()));
				this.m_userKeyValues["Language"] = Application.systemLanguage.ToString();
				this.m_userKeyValues["SettingsPath"] = Application.persistentDataPath;
				this.m_userKeyValues["SupportsRenderTextureFormat_Depth"] = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth).ToString();
				this.m_userKeyValues["SupportsRenderTextureFormat_RFloat"] = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat).ToString();
				this.m_userKeyValues["SupportsRenderTextureFormat_RHalf"] = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RHalf).ToString();
				this.m_userKeyValues["CurrentResolution"] = Screen.currentResolution.ToString();
				if (Options_UI.Get() != null)
				{
					this.m_userKeyValues["GraphicsQuality"] = Options_UI.Get().GetCurrentGraphicsQuality().ToString();
					this.m_userKeyValues["GraphicsQualityEverSetManually"] = Options_UI.Get().GetGraphicsQualityEverSetManually().ToString();
				}
				Resolution[] resolutions = Screen.resolutions;
				if (resolutions != null)
				{
					for (int i = 0; i < resolutions.Length; i++)
					{
						this.m_userKeyValues[string.Format("Resolution{0}", i)] = resolutions[i].ToString();
					}
				}
				if (userMessage != null)
				{
					this.m_userKeyValues[" UserMessage"] = userMessage;
				}
				foreach (PropertyInfo propertyInfo in typeof(SystemInfo).GetProperties(BindingFlags.Static | BindingFlags.Public))
				{
					this.m_userKeyValues.Add(propertyInfo.Name, propertyInfo.GetValue(null, null).ToString());
				}
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
			}
			base.StartThread();
		}
		catch (Exception exception2)
		{
			Log.Exception(exception2);
		}
	}

	internal bool IsFinished
	{
		get
		{
			switch (this.m_state)
			{
			case ClientCrashReportThreadedJob.State.Succeeded:
			case ClientCrashReportThreadedJob.State.Failed:
			case ClientCrashReportThreadedJob.State.Canceled:
				return true;
			default:
				return false;
			}
		}
	}

	internal bool IsSucceeded
	{
		get
		{
			return this.m_stateSucceeeded;
		}
	}

	internal void Cancel()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnConnectedToLobbyServer -= this.OnRegisterGameClientResponse;
		}
		try
		{
			base.Abort();
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		this.m_state = ClientCrashReportThreadedJob.State.Canceled;
	}

	protected override void ThreadFunction()
	{
		try
		{
			ClientCrashReportThreadedJob.State state = this.m_state;
			if (state != ClientCrashReportThreadedJob.State.CreatingArchive)
			{
				if (state != ClientCrashReportThreadedJob.State.MoveArchiveAndReadBytes)
				{
					if (state != ClientCrashReportThreadedJob.State.CleanUp)
					{
					}
					else
					{
						CrashReportArchiver.DeleteArchives(this.m_temporaryCachePath);
						this.m_progressValue = 0.95f;
						if (this.m_stateSucceeeded)
						{
							try
							{
								Directory.Delete(this.m_crashDumpDirectoryPath, true);
							}
							catch (Exception exception)
							{
								Log.Exception(exception);
							}
						}
						this.m_progressValue = 1f;
						this.m_stateSucceeeded = true;
					}
				}
				else
				{
					this.m_progressValue = 0.4f;
					string text = Path.Combine(this.m_temporaryCachePath, this.m_archiveFileName);
					this.m_progressValue = 0.5f;
					try
					{
						File.Move(this.m_tempArchivePath, text);
					}
					catch (Exception exception2)
					{
						Log.Exception(exception2);
					}
					this.m_progressValue = 0.6f;
					if (File.Exists(text))
					{
						try
						{
							this.m_crashReportBytes = File.ReadAllBytes(text);
							this.m_stateSucceeeded = (this.m_crashReportBytes != null && this.m_crashReportBytes.Length > 0);
						}
						catch (Exception exception3)
						{
							Log.Exception(exception3);
							this.m_stateSucceeeded = false;
						}
					}
					else
					{
						Log.Error("Failed to move file {0} to {1}", new object[]
						{
							this.m_tempArchivePath,
							text
						});
						this.m_state = ClientCrashReportThreadedJob.State.Failed;
					}
				}
			}
			else
			{
				this.m_progressValue = 0.05f;
				this.m_stateSucceeeded = true;
				if (!Directory.Exists(this.m_crashDumpDirectoryPath))
				{
					if (this.m_bugReportType != BugReportType.Crash)
					{
						try
						{
							DirectoryInfo directoryInfo = Directory.CreateDirectory(this.m_crashDumpDirectoryPath);
							if (directoryInfo != null)
							{
								if (directoryInfo.Exists)
								{
									goto IL_D6;
								}
							}
							Log.Error("Failed to create temp directory for user bug report at {0}", new object[]
							{
								this.m_crashDumpDirectoryPath
							});
							this.m_stateSucceeeded = false;
							IL_D6:
							using (new StreamWriter(this.m_crashDumpDirectoryPath + "\\crash.dmp"))
							{
							}
						}
						catch (Exception exception4)
						{
							Log.Exception(exception4);
							this.m_stateSucceeeded = false;
						}
					}
					else
					{
						Log.Error("Directory does not exist for crash report {0}", new object[]
						{
							this.m_crashDumpDirectoryPath
						});
						this.m_stateSucceeeded = false;
					}
				}
				if (this.m_stateSucceeeded)
				{
					if (!string.IsNullOrEmpty(this.m_userMessage))
					{
						try
						{
							StreamWriter streamWriter2 = new StreamWriter(this.m_crashDumpDirectoryPath + "\\UserMessage.txt");
							try
							{
								streamWriter2.Write(this.m_userMessage);
							}
							finally
							{
								if (streamWriter2 != null)
								{
									((IDisposable)streamWriter2).Dispose();
								}
							}
						}
						catch (Exception exception5)
						{
							Log.Exception(exception5);
							bool stateSucceeeded;
							if (this.m_bugReportType != BugReportType.Bug)
							{
								stateSucceeeded = (this.m_bugReportType != BugReportType.Exception);
							}
							else
							{
								stateSucceeeded = false;
							}
							this.m_stateSucceeeded = stateSucceeeded;
						}
					}
				}
				if (this.m_stateSucceeeded)
				{
					if (this.m_bugReportType != BugReportType.Crash)
					{
						try
						{
							StringBuilder stringBuilder = new StringBuilder((this.m_logMessage != null) ? (this.m_logMessage + "\n") : string.Empty);
							if (File.Exists(this.m_outputLogPath))
							{
								File.Copy(this.m_outputLogPath, this.m_outputLogPathCopy, true);
								if (File.Exists(this.m_outputLogPathCopy))
								{
									StreamReader streamReader = File.OpenText(this.m_outputLogPathCopy);
									try
									{
										if (streamReader.BaseStream.Length <= 0x40000L)
										{
											streamReader.BaseStream.Seek(-streamReader.BaseStream.Length, SeekOrigin.End);
										}
										else
										{
											streamReader.BaseStream.Seek(-0x40000L, SeekOrigin.End);
											stringBuilder.AppendFormat("(Skipped {0} bytes)\n", streamReader.BaseStream.Length - 0x40000L);
										}
										stringBuilder.Append(streamReader.ReadToEnd());
									}
									finally
									{
										if (streamReader != null)
										{
											((IDisposable)streamReader).Dispose();
										}
									}
									StreamWriter streamWriter3 = new StreamWriter(Path.Combine(this.m_crashDumpDirectoryPath, Path.GetFileName(this.m_outputLogPath)));
									try
									{
										streamWriter3.Write(stringBuilder.ToString());
									}
									finally
									{
										if (streamWriter3 != null)
										{
											((IDisposable)streamWriter3).Dispose();
										}
									}
								}
							}
						}
						catch (Exception exception6)
						{
							Log.Exception(exception6);
						}
					}
				}
				if (this.m_stateSucceeeded)
				{
					try
					{
						StringBuilder stringBuilder2 = new StringBuilder();
						if (ClientBootstrap.Instance != null)
						{
							if (File.Exists(ClientBootstrap.Instance.GetFileLogCurrentPath()))
							{
								string path = "AtlasReactor.txt";
								string text2 = Path.Combine(this.m_temporaryCachePath, path);
								File.Copy(ClientBootstrap.Instance.GetFileLogCurrentPath(), text2, true);
								if (File.Exists(text2))
								{
									StreamReader streamReader2 = File.OpenText(text2);
									try
									{
										if (streamReader2.BaseStream.Length <= 0x40000L)
										{
											streamReader2.BaseStream.Seek(-streamReader2.BaseStream.Length, SeekOrigin.End);
										}
										else
										{
											streamReader2.BaseStream.Seek(-0x40000L, SeekOrigin.End);
											stringBuilder2.AppendFormat("(Skipped {0} bytes)\n", streamReader2.BaseStream.Length - 0x40000L);
										}
										stringBuilder2.Append(streamReader2.ReadToEnd());
									}
									finally
									{
										if (streamReader2 != null)
										{
											((IDisposable)streamReader2).Dispose();
										}
									}
									StreamWriter streamWriter4 = new StreamWriter(Path.Combine(this.m_crashDumpDirectoryPath, path));
									try
									{
										streamWriter4.Write(stringBuilder2.ToString());
									}
									finally
									{
										if (streamWriter4 != null)
										{
											((IDisposable)streamWriter4).Dispose();
										}
									}
								}
							}
						}
					}
					catch (Exception exception7)
					{
						Log.Exception(exception7);
					}
				}
				if (this.m_stateSucceeeded)
				{
					this.m_stateSucceeeded = CrashReportArchiver.CreateArchiveFromCrashDumpDirectory(out this.m_tempArchivePath, out this.m_tempArchiveFileSize, this.m_crashDumpDirectoryPath, this.m_temporaryCachePath, this.m_userKeyValues, this.m_bugReportType);
				}
				this.m_progressValue = 0.2f;
			}
		}
		catch (Exception exception8)
		{
			Log.Exception(exception8);
			this.m_state = ClientCrashReportThreadedJob.State.Failed;
		}
	}

	protected override void OnThreadFunctionReturned()
	{
		switch (this.m_state)
		{
		case ClientCrashReportThreadedJob.State.CreatingArchive:
			if (this.m_stateSucceeeded)
			{
				if (!ClientGameManager.Get().IsConnectedToLobbyServer)
				{
					ClientGameManager.Get().OnConnectedToLobbyServer += this.OnRegisterGameClientResponse;
					this.m_state = ClientCrashReportThreadedJob.State.WaitingForClientToConnectToLobbyServer;
				}
				else
				{
					this.RequestArchiveName();
				}
			}
			else
			{
				this.m_state = ClientCrashReportThreadedJob.State.Failed;
			}
			return;
		case ClientCrashReportThreadedJob.State.MoveArchiveAndReadBytes:
			if (this.m_stateSucceeeded)
			{
				Log.Info("Move and read of {0} succeeded: {1}", new object[]
				{
					this.m_archiveFileName,
					this.m_stateSucceeeded
				});
				try
				{
					Log.Info("Attempting to build URL to send crash report {0}", new object[]
					{
						this.m_archiveFileName
					});
					string archiveFileName = this.m_archiveFileName;
					string crashServerAndArchiveURL = string.Format("http://debug.triongames.com/v2/archive/{0}", archiveFileName);
					ClientCrashReportDetector.Get().UploadArchive(crashServerAndArchiveURL, this.m_crashReportBytes, new Action<bool>(this.OnUploadArchiveEndStatus));
				}
				catch (Exception exception)
				{
					Log.Exception(exception);
				}
			}
			else
			{
				this.m_state = ClientCrashReportThreadedJob.State.Failed;
			}
			return;
		case ClientCrashReportThreadedJob.State.CleanUp:
			if (this.m_stateSucceeeded)
			{
				Log.Info("Clean up of {0} succeeded: {1}", new object[]
				{
					this.m_archiveFileName,
					this.m_stateSucceeeded
				});
				this.m_state = ClientCrashReportThreadedJob.State.Succeeded;
			}
			else
			{
				this.m_state = ClientCrashReportThreadedJob.State.Failed;
			}
			return;
		case ClientCrashReportThreadedJob.State.Failed:
		case ClientCrashReportThreadedJob.State.Canceled:
			return;
		}
		this.m_state = ClientCrashReportThreadedJob.State.Failed;
		Log.Error("Unexpected state for completed worker thread: {0}", new object[]
		{
			this.m_state.ToString()
		});
	}

	private void RequestArchiveName()
	{
		if (ClientGameManager.Get().RequestCrashReportArchiveName(this.m_tempArchiveFileSize, new Action<CrashReportArchiveNameResponse>(this.OnCrashReportArchiveNameResponse)))
		{
			this.m_state = ClientCrashReportThreadedJob.State.WaitingForArchiveNameResponse;
		}
		else
		{
			Log.Error("Failed to request crash report archive name", new object[0]);
			this.m_state = ClientCrashReportThreadedJob.State.Failed;
		}
	}

	private void OnCrashReportArchiveNameResponse(CrashReportArchiveNameResponse response)
	{
		if (response.Success)
		{
			ClientCrashReportThreadedJob.State state = this.m_state;
			if (state != ClientCrashReportThreadedJob.State.Failed)
			{
				if (state != ClientCrashReportThreadedJob.State.Canceled)
				{
					if (state != ClientCrashReportThreadedJob.State.WaitingForArchiveNameResponse)
					{
						Log.Error("Unexpected worker thread state when receiving archive name: {0}", new object[]
						{
							this.m_state.ToString()
						});
						this.Cancel();
						this.m_state = ClientCrashReportThreadedJob.State.Failed;
					}
					else if (base.IsThreadAlive)
					{
						Log.Error("Unexpected busy worker thread when receiving archive name", new object[0]);
						this.Cancel();
						this.m_state = ClientCrashReportThreadedJob.State.Failed;
					}
					else if (string.IsNullOrEmpty(response.ArchiveName))
					{
						Log.Error("Unexpected empty archive name received", new object[0]);
						this.Cancel();
						this.m_state = ClientCrashReportThreadedJob.State.Failed;
					}
					else
					{
						this.m_archiveFileName = string.Copy(response.ArchiveName);
						this.m_state = ClientCrashReportThreadedJob.State.MoveArchiveAndReadBytes;
						base.StartThread();
					}
				}
			}
		}
		else
		{
			Log.Error("Crash archive naming failed: {0}", new object[]
			{
				response.ErrorMessage
			});
			this.Cancel();
			this.m_state = ClientCrashReportThreadedJob.State.Failed;
		}
	}

	private void OnRegisterGameClientResponse(RegisterGameClientResponse response)
	{
		if (!response.Success)
		{
			Log.Error("Client encountered an error while connecting to lobby server {0}", new object[]
			{
				response.ErrorMessage
			});
			this.Cancel();
			this.m_state = ClientCrashReportThreadedJob.State.Failed;
		}
		else
		{
			ClientGameManager.Get().OnConnectedToLobbyServer -= this.OnRegisterGameClientResponse;
			ClientCrashReportThreadedJob.State state = this.m_state;
			if (state != ClientCrashReportThreadedJob.State.WaitingForClientToConnectToLobbyServer)
			{
				Log.Error("Unexpected worker thread state when receiving lobby server connect: {0}", new object[]
				{
					this.m_state.ToString()
				});
				this.Cancel();
				this.m_state = ClientCrashReportThreadedJob.State.Failed;
			}
			else
			{
				this.RequestArchiveName();
			}
		}
	}

	private void OnUploadArchiveEndStatus(bool success)
	{
		if (success)
		{
			this.m_state = ClientCrashReportThreadedJob.State.CleanUp;
			base.StartThread();
		}
		else
		{
			this.m_state = ClientCrashReportThreadedJob.State.Failed;
		}
	}

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
}
