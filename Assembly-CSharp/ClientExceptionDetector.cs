using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using LobbyGameClientMessages;
using UnityEngine;

public class ClientExceptionDetector : MonoBehaviour
{
	private string m_exceptionLogString;

	private string m_exceptionStackTrace;

	private string m_exceptionDateTime;

	private ClientCrashReportThreadedJob m_crashServerReportThreadedJob;

	private static string s_stackTraceSeparator = "\n   at ";

	private bool m_realExceptionOccurred;

	private bool m_stopUploadingReports;

	private bool m_lobbyServerAndDialogReady;

	private float m_lastErrorReportTime;

	private int m_lastErrorReportBytes;

	private const int MAX_ERROR_REPORT_BYTES_PER_SECOND = 0x80;

	private const int MAX_ERROR_REPORTS_QUEUED = 0x80;

	private Queue<ClientErrorReport> m_errorReportQueue = new Queue<ClientErrorReport>();

	private string m_exceptionFilePath;

	private static ClientExceptionDetector s_instance;

	private Dictionary<uint, ClientErrorReport> m_errorBestiary = new Dictionary<uint, ClientErrorReport>();

	private Dictionary<uint, uint> m_errorUnsentCount = new Dictionary<uint, uint>();

	internal float SecondsBetweenSendingErrorPackets = 300f;

	internal static ClientExceptionDetector Get()
	{
		return ClientExceptionDetector.s_instance;
	}

	private void Awake()
	{
		ClientExceptionDetector.s_instance = this;
		try
		{
			this.m_exceptionFilePath = Path.GetFullPath(Path.Combine(Application.dataPath, "../exception.txt"));
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Failed to create path for exception log, with new exception: {0}", new object[]
			{
				ex.ToString()
			});
		}
		Application.logMessageReceived += this.HandleUnityLogMessage;
	}

	private void Update()
	{
		if (this.m_crashServerReportThreadedJob != null)
		{
			this.m_crashServerReportThreadedJob.Update();
			if (this.m_crashServerReportThreadedJob.IsFinished)
			{
				this.m_crashServerReportThreadedJob = null;
			}
		}
		if (ClientGameManager.Get() != null)
		{
			if (ClientGameManager.Get().IsConnectedToLobbyServer)
			{
				if (!this.m_lobbyServerAndDialogReady && UIDialogPopupManager.Ready)
				{
					this.m_lobbyServerAndDialogReady = true;
					string[] array = new string[5];
					try
					{
						if (File.Exists(this.m_exceptionFilePath))
						{
							string[] array2 = File.ReadAllLines(this.m_exceptionFilePath);
							using (StreamReader streamReader = File.OpenText(this.m_exceptionFilePath))
							{
								array[4] = streamReader.ReadLine();
								array[0] = streamReader.ReadLine();
								array[1] = streamReader.ReadToEnd();
							}
						}
					}
					catch (Exception ex)
					{
						Debug.LogErrorFormat("Reading of exception report cached in file failed, with new exception: {0}", new object[]
						{
							ex.ToString()
						});
					}
					if (!string.IsNullOrEmpty(array[4]))
					{
						if (this.UploadExceptionReport(array[0], array[1], string.Empty, string.Empty, array[4], false))
						{
							try
							{
								File.Delete(this.m_exceptionFilePath);
							}
							catch (Exception ex2)
							{
								Debug.LogErrorFormat("Deletion of exception report cached in file failed, with new exception: {0}", new object[]
								{
									ex2.ToString()
								});
							}
						}
					}
				}
				else
				{
					if (this.m_errorReportQueue.Count <= 0)
					{
						if (this.m_errorUnsentCount.IsNullOrEmpty<KeyValuePair<uint, uint>>())
						{
							return;
						}
					}
					this.UpdateLobbyWithErrors();
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (this.m_crashServerReportThreadedJob != null)
		{
			this.m_crashServerReportThreadedJob.Cancel();
		}
	}

	internal void HandleBelowMinSpec(string logString, string stackTrace)
	{
		this.UploadExceptionReport(logString, stackTrace, StringUtil.TR("SystemRequirementsNotMet", "Global"), StringUtil.TR("UploadGeneratedReport", "Global"), null, true);
	}

	private bool UploadExceptionReport(string logString, string stackTrace, string dlgTitle, string dlgDescription, string fileDateTime = null, bool belowMinSpecDialogNotException = false)
	{
		bool flag = !string.IsNullOrEmpty(fileDateTime);
		bool realExceptionOccurred;
		if (!this.m_realExceptionOccurred)
		{
			realExceptionOccurred = (!belowMinSpecDialogNotException && !flag);
		}
		else
		{
			realExceptionOccurred = true;
		}
		this.m_realExceptionOccurred = realExceptionOccurred;
		bool flag2 = false;
		if (!this.m_stopUploadingReports)
		{
			this.m_exceptionLogString = logString.Trim().Replace("\r", string.Empty);
			int num = this.m_exceptionLogString.IndexOf("\n");
			if (num >= 0)
			{
				this.m_exceptionLogString = this.m_exceptionLogString.Substring(0, num);
			}
			this.m_exceptionStackTrace = stackTrace;
			this.m_exceptionStackTrace = this.m_exceptionStackTrace.Trim();
			this.m_exceptionStackTrace = ClientExceptionDetector.s_stackTraceSeparator + this.m_exceptionStackTrace.Replace("\n", ClientExceptionDetector.s_stackTraceSeparator);
			this.m_exceptionDateTime = ((!flag) ? DateTime.Now.ToString("MM/dd/yy hh:mm:ss") : fileDateTime);
			string text = string.Format("{0}\n{1}", this.m_exceptionLogString, this.m_exceptionStackTrace);
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().IsConnectedToLobbyServer)
				{
					if (this.m_crashServerReportThreadedJob == null)
					{
						string crashDumpDirectoryPath = Path.Combine(Application.temporaryCachePath, Guid.NewGuid().ToString());
						BugReportType bugReportType = BugReportType.Exception;
						string userMessage = text;
						string format = "{0}: {1}";
						object arg;
						if (flag)
						{
							arg = "FileDateTime";
						}
						else
						{
							arg = "SessionToken";
						}
						object arg2;
						if (flag)
						{
							arg2 = fileDateTime;
						}
						else
						{
							if (ClientGameManager.Get() != null)
							{
								if (ClientGameManager.Get().SessionInfo != null)
								{
									arg2 = ClientGameManager.Get().SessionInfo.SessionToken.ToString();
									goto IL_219;
								}
							}
							arg2 = "unknown";
						}
						IL_219:
						this.m_crashServerReportThreadedJob = new ClientCrashReportThreadedJob(crashDumpDirectoryPath, bugReportType, userMessage, string.Format(format, arg, arg2));
					}
					ClientStatusReport clientStatusReport = new ClientStatusReport();
					ClientStatusReport clientStatusReport2 = clientStatusReport;
					ClientStatusReport.ClientStatusReportType status;
					if (belowMinSpecDialogNotException)
					{
						status = ClientStatusReport.ClientStatusReportType.symbol_0016;
					}
					else
					{
						status = ClientStatusReport.ClientStatusReportType.symbol_000E;
					}
					clientStatusReport2.Status = status;
					clientStatusReport.StatusDetails = text;
					clientStatusReport.DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;
					ClientStatusReport clientStatusReport3 = clientStatusReport;
					string fileDateTime2;
					if (flag)
					{
						fileDateTime2 = fileDateTime;
					}
					else
					{
						fileDateTime2 = string.Empty;
					}
					clientStatusReport3.FileDateTime = fileDateTime2;
					ClientGameManager.Get().SendStatusReport(clientStatusReport);
					flag2 = true;
					if (UIDialogPopupManager.Ready && !belowMinSpecDialogNotException)
					{
						if (!flag && UIDialogPopupManager.OpenReportBugDialog(dlgTitle, dlgDescription, StringUtil.TR("Ok", "Global"), StringUtil.TR("Cancel", "Global"), new UIDialogBox.DialogButtonCallback(this.HandleExceptionDialogOKButton), null) == null)
						{
							Log.Error("Failed to create dialog", new object[0]);
						}
					}
					goto IL_385;
				}
			}
			if (!belowMinSpecDialogNotException)
			{
				if (!flag)
				{
					try
					{
						StreamWriter streamWriter = File.CreateText(this.m_exceptionFilePath);
						try
						{
							streamWriter.WriteLine(this.m_exceptionDateTime);
							streamWriter.WriteLine(this.m_exceptionLogString);
							streamWriter.Write(this.m_exceptionStackTrace);
							streamWriter.Flush();
						}
						finally
						{
							if (streamWriter != null)
							{
								((IDisposable)streamWriter).Dispose();
							}
						}
					}
					catch
					{
					}
				}
			}
		}
		IL_385:
		bool stopUploadingReports;
		if (!this.m_stopUploadingReports)
		{
			if (flag2)
			{
				stopUploadingReports = !flag;
			}
			else
			{
				stopUploadingReports = false;
			}
		}
		else
		{
			stopUploadingReports = true;
		}
		this.m_stopUploadingReports = stopUploadingReports;
		return flag2;
	}

	private void HandleUnityLogMessage(string logString, string stackTrace, LogType type)
	{
		if (!this.m_realExceptionOccurred)
		{
			if (!ClientMinSpecDetector.BelowMinSpecDetected)
			{
				switch (type)
				{
				case LogType.Error:
				case LogType.Assert:
					if (this.m_errorReportQueue.Count < 0x80)
					{
						ClientErrorReport clientErrorReport = new ClientErrorReport();
						clientErrorReport.Time = Time.unscaledTime;
						clientErrorReport.LogString = logString.Trim().Replace("\r", string.Empty);
						string[] array = clientErrorReport.LogString.Split(new char[]
						{
							'\n'
						}, 2, StringSplitOptions.RemoveEmptyEntries);
						if (array != null)
						{
							if (array.Length == 2)
							{
								clientErrorReport.LogString = array[0].Trim();
								clientErrorReport.StackTrace = array[1];
								goto IL_11D;
							}
						}
						clientErrorReport.StackTrace = string.Empty;
						IL_11D:
						clientErrorReport.StackTraceHash = this.GenerateStackTraceHash(clientErrorReport);
						this.m_errorReportQueue.Enqueue(clientErrorReport);
					}
					break;
				case LogType.Exception:
					this.UploadExceptionReport(logString, stackTrace, StringUtil.TR("UnhandledException", "Global"), StringUtil.TR("SubmitBugReport", "Global"), null, false);
					break;
				}
				return;
			}
		}
	}

	private uint GenerateStackTraceHash(ClientErrorReport errReport)
	{
		if (string.IsNullOrEmpty(errReport.StackTrace))
		{
			string name = Regex.Replace(errReport.LogString, "\\d+", "#");
			return StringUtil.CaseInsensitiveHash(name);
		}
		string name2 = Regex.Replace(errReport.StackTrace, "\\d+", "#");
		return StringUtil.CaseInsensitiveHash(name2);
	}

	private void HandleExceptionDialogOKButton(UIDialogBox boxReference)
	{
		ClientStatusReport clientStatusReport = new ClientStatusReport();
		clientStatusReport.Status = ClientStatusReport.ClientStatusReportType.symbol_0015;
		clientStatusReport.StatusDetails = this.m_exceptionLogString;
		clientStatusReport.UserMessage = ((UIReportBugDialogBox)boxReference).m_descriptionBoxInputField.text;
		clientStatusReport.DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;
		ClientGameManager.Get().SendStatusReport(clientStatusReport);
		this.m_stopUploadingReports = true;
	}

	private bool IsSendingErrorsToLobbyASAP()
	{
		return this.SecondsBetweenSendingErrorPackets <= 0f;
	}

	private void UpdateLobbyWithErrors()
	{
		float num = Time.unscaledTime - this.m_lastErrorReportTime;
		if (this.IsSendingErrorsToLobbyASAP())
		{
			if (num > 1.401298E-45f)
			{
				if ((float)this.m_lastErrorReportBytes / num < 128f)
				{
					ClientErrorReport clientErrorReport = this.m_errorReportQueue.Peek();
					this.m_lastErrorReportTime = Time.unscaledTime;
					this.m_lastErrorReportBytes = clientErrorReport.symbol_001D();
					if (ClientGameManager.Get().SendErrorReport(clientErrorReport))
					{
						this.m_errorReportQueue.Dequeue();
					}
					else
					{
						Log.Warning("Failed to send exception report", new object[0]);
					}
				}
			}
		}
		else
		{
			foreach (ClientErrorReport clientErrorReport2 in this.m_errorReportQueue)
			{
				uint stackTraceHash = clientErrorReport2.StackTraceHash;
				if (!this.m_errorBestiary.ContainsKey(stackTraceHash))
				{
					this.m_errorBestiary.Add(stackTraceHash, clientErrorReport2);
				}
				uint num2 = 0U;
				if (this.m_errorUnsentCount.TryGetValue(stackTraceHash, out num2))
				{
					this.m_errorUnsentCount[stackTraceHash] = num2 + 1U;
				}
				else
				{
					this.m_errorUnsentCount.Add(stackTraceHash, 1U);
				}
			}
			this.m_errorReportQueue.Clear();
			if (num > this.SecondsBetweenSendingErrorPackets)
			{
				this.m_lastErrorReportTime = Time.unscaledTime;
				this.FlushErrorsToLobby();
			}
		}
	}

	public bool GetClientErrorReport(uint crashReportHash, out ClientErrorReport clientErrorReport)
	{
		return this.m_errorBestiary.TryGetValue(crashReportHash, out clientErrorReport);
	}

	public void FlushErrorsToLobby()
	{
		if (!this.m_errorUnsentCount.IsNullOrEmpty<KeyValuePair<uint, uint>>())
		{
			ClientErrorSummary summary = new ClientErrorSummary
			{
				ReportCount = this.m_errorUnsentCount
			};
			if (ClientGameManager.Get().SendErrorSummary(summary))
			{
				this.m_errorUnsentCount.Clear();
			}
			else
			{
				Log.Warning("Failed to send exception summary", new object[0]);
			}
		}
	}

	private enum FileStringIndex
	{
		FileDateTime,
		Log,
		StackTraceToEOF
	}

	private enum QueueStringIndex
	{
		Log,
		StackTrace,
		DlgTitle,
		DlgDescription,
		FileDateTime,
		Count
	}
}
