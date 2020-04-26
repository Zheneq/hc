using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class ClientExceptionDetector : MonoBehaviour
{
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

	private const int MAX_ERROR_REPORT_BYTES_PER_SECOND = 128;

	private const int MAX_ERROR_REPORTS_QUEUED = 128;

	private Queue<ClientErrorReport> m_errorReportQueue = new Queue<ClientErrorReport>();

	private string m_exceptionFilePath;

	private static ClientExceptionDetector s_instance;

	private Dictionary<uint, ClientErrorReport> m_errorBestiary = new Dictionary<uint, ClientErrorReport>();

	private Dictionary<uint, uint> m_errorUnsentCount = new Dictionary<uint, uint>();

	internal float SecondsBetweenSendingErrorPackets = 300f;

	internal static ClientExceptionDetector Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		try
		{
			m_exceptionFilePath = Path.GetFullPath(Path.Combine(Application.dataPath, "../exception.txt"));
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Failed to create path for exception log, with new exception: {0}", ex.ToString());
		}
		Application.logMessageReceived += HandleUnityLogMessage;
	}

	private void Update()
	{
		if (m_crashServerReportThreadedJob != null)
		{
			m_crashServerReportThreadedJob.Update();
			if (m_crashServerReportThreadedJob.IsFinished)
			{
				m_crashServerReportThreadedJob = null;
			}
		}
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (!ClientGameManager.Get().IsConnectedToLobbyServer)
			{
				return;
			}
			while (true)
			{
				if (!m_lobbyServerAndDialogReady && UIDialogPopupManager.Ready)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
						{
							m_lobbyServerAndDialogReady = true;
							string[] array = new string[5];
							try
							{
								if (File.Exists(m_exceptionFilePath))
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											break;
										default:
										{
											string[] array2 = File.ReadAllLines(m_exceptionFilePath);
											using (StreamReader streamReader = File.OpenText(m_exceptionFilePath))
											{
												array[4] = streamReader.ReadLine();
												array[0] = streamReader.ReadLine();
												array[1] = streamReader.ReadToEnd();
											}
											goto end_IL_00a8;
										}
										}
									}
								}
								end_IL_00a8:;
							}
							catch (Exception ex)
							{
								Debug.LogErrorFormat("Reading of exception report cached in file failed, with new exception: {0}", ex.ToString());
							}
							if (!string.IsNullOrEmpty(array[4]))
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										if (UploadExceptionReport(array[0], array[1], string.Empty, string.Empty, array[4]))
										{
											while (true)
											{
												switch (2)
												{
												case 0:
													break;
												default:
													try
													{
														File.Delete(m_exceptionFilePath);
													}
													catch (Exception ex2)
													{
														Debug.LogErrorFormat("Deletion of exception report cached in file failed, with new exception: {0}", ex2.ToString());
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
				if (m_errorReportQueue.Count <= 0)
				{
					if (m_errorUnsentCount.IsNullOrEmpty())
					{
						return;
					}
				}
				UpdateLobbyWithErrors();
				return;
			}
		}
	}

	private void OnDestroy()
	{
		if (m_crashServerReportThreadedJob == null)
		{
			return;
		}
		while (true)
		{
			m_crashServerReportThreadedJob.Cancel();
			return;
		}
	}

	internal void HandleBelowMinSpec(string logString, string stackTrace)
	{
		UploadExceptionReport(logString, stackTrace, StringUtil.TR("SystemRequirementsNotMet", "Global"), StringUtil.TR("UploadGeneratedReport", "Global"), null, true);
	}

	private bool UploadExceptionReport(string logString, string stackTrace, string dlgTitle, string dlgDescription, string fileDateTime = null, bool belowMinSpecDialogNotException = false)
	{
		bool flag = !string.IsNullOrEmpty(fileDateTime);
		int realExceptionOccurred;
		if (!m_realExceptionOccurred)
		{
			realExceptionOccurred = ((!belowMinSpecDialogNotException && !flag) ? 1 : 0);
		}
		else
		{
			realExceptionOccurred = 1;
		}
		m_realExceptionOccurred = ((byte)realExceptionOccurred != 0);
		bool flag2 = false;
		string text;
		string crashDumpDirectoryPath;
		object arg;
		object arg2;
		if (!m_stopUploadingReports)
		{
			m_exceptionLogString = logString.Trim().Replace("\r", string.Empty);
			int num = m_exceptionLogString.IndexOf("\n");
			if (num >= 0)
			{
				m_exceptionLogString = m_exceptionLogString.Substring(0, num);
			}
			m_exceptionStackTrace = stackTrace;
			m_exceptionStackTrace = m_exceptionStackTrace.Trim();
			m_exceptionStackTrace = s_stackTraceSeparator + m_exceptionStackTrace.Replace("\n", s_stackTraceSeparator);
			m_exceptionDateTime = ((!flag) ? DateTime.Now.ToString("MM/dd/yy hh:mm:ss") : fileDateTime);
			text = $"{m_exceptionLogString}\n{m_exceptionStackTrace}";
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().IsConnectedToLobbyServer)
				{
					if (m_crashServerReportThreadedJob == null)
					{
						crashDumpDirectoryPath = Path.Combine(Application.temporaryCachePath, Guid.NewGuid().ToString());
						if (flag)
						{
							arg = "FileDateTime";
						}
						else
						{
							arg = "SessionToken";
						}
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
									goto IL_0219;
								}
							}
							arg2 = "unknown";
						}
						goto IL_0219;
					}
					goto IL_022a;
				}
			}
			if (!belowMinSpecDialogNotException)
			{
				if (!flag)
				{
					try
					{
						StreamWriter streamWriter = File.CreateText(m_exceptionFilePath);
						try
						{
							streamWriter.WriteLine(m_exceptionDateTime);
							streamWriter.WriteLine(m_exceptionLogString);
							streamWriter.Write(m_exceptionStackTrace);
							streamWriter.Flush();
						}
						finally
						{
							if (streamWriter != null)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
										((IDisposable)streamWriter).Dispose();
										goto end_IL_036a;
									}
								}
							}
							end_IL_036a:;
						}
					}
					catch
					{
					}
				}
			}
		}
		goto IL_0385;
		IL_022a:
		ClientStatusReport clientStatusReport = new ClientStatusReport();
		int status;
		if (belowMinSpecDialogNotException)
		{
			status = 4;
		}
		else
		{
			status = 1;
		}
		clientStatusReport.Status = (ClientStatusReport.ClientStatusReportType)status;
		clientStatusReport.StatusDetails = text;
		clientStatusReport.DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;
		string fileDateTime2;
		if (flag)
		{
			fileDateTime2 = fileDateTime;
		}
		else
		{
			fileDateTime2 = string.Empty;
		}
		clientStatusReport.FileDateTime = fileDateTime2;
		ClientGameManager.Get().SendStatusReport(clientStatusReport);
		flag2 = true;
		if (UIDialogPopupManager.Ready && !belowMinSpecDialogNotException)
		{
			if (!flag && UIDialogPopupManager.OpenReportBugDialog(dlgTitle, dlgDescription, StringUtil.TR("Ok", "Global"), StringUtil.TR("Cancel", "Global"), HandleExceptionDialogOKButton) == null)
			{
				Log.Error("Failed to create dialog");
			}
		}
		goto IL_0385;
		IL_0219:
		m_crashServerReportThreadedJob = new ClientCrashReportThreadedJob(crashDumpDirectoryPath, BugReportType.Exception, text, $"{arg}: {arg2}");
		goto IL_022a;
		IL_0385:
		int stopUploadingReports;
		if (!m_stopUploadingReports)
		{
			if (flag2)
			{
				stopUploadingReports = ((!flag) ? 1 : 0);
			}
			else
			{
				stopUploadingReports = 0;
			}
		}
		else
		{
			stopUploadingReports = 1;
		}
		m_stopUploadingReports = ((byte)stopUploadingReports != 0);
		return flag2;
	}

	private void HandleUnityLogMessage(string logString, string stackTrace, LogType type)
	{
		if (m_realExceptionOccurred)
		{
			return;
		}
		while (true)
		{
			if (ClientMinSpecDetector.BelowMinSpecDetected)
			{
				return;
			}
			switch (type)
			{
			case LogType.Warning:
			case LogType.Log:
				break;
			case LogType.Exception:
				UploadExceptionReport(logString, stackTrace, StringUtil.TR("UnhandledException", "Global"), StringUtil.TR("SubmitBugReport", "Global"));
				break;
			case LogType.Error:
			case LogType.Assert:
				if (m_errorReportQueue.Count >= 128)
				{
					break;
				}
				while (true)
				{
					ClientErrorReport clientErrorReport = new ClientErrorReport();
					clientErrorReport.Time = Time.unscaledTime;
					clientErrorReport.LogString = logString.Trim().Replace("\r", string.Empty);
					string[] array = clientErrorReport.LogString.Split(new char[1]
					{
						'\n'
					}, 2, StringSplitOptions.RemoveEmptyEntries);
					if (array != null)
					{
						if (array.Length == 2)
						{
							clientErrorReport.LogString = array[0].Trim();
							clientErrorReport.StackTrace = array[1];
							goto IL_011d;
						}
					}
					clientErrorReport.StackTrace = string.Empty;
					goto IL_011d;
					IL_011d:
					clientErrorReport.StackTraceHash = GenerateStackTraceHash(clientErrorReport);
					m_errorReportQueue.Enqueue(clientErrorReport);
					return;
				}
			}
			return;
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
		clientStatusReport.Status = ClientStatusReport.ClientStatusReportType._0015;
		clientStatusReport.StatusDetails = m_exceptionLogString;
		clientStatusReport.UserMessage = ((UIReportBugDialogBox)boxReference).m_descriptionBoxInputField.text;
		clientStatusReport.DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;
		ClientGameManager.Get().SendStatusReport(clientStatusReport);
		m_stopUploadingReports = true;
	}

	private bool IsSendingErrorsToLobbyASAP()
	{
		return SecondsBetweenSendingErrorPackets <= 0f;
	}

	private void UpdateLobbyWithErrors()
	{
		float num = Time.unscaledTime - m_lastErrorReportTime;
		if (IsSendingErrorsToLobbyASAP())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (num > float.Epsilon)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								if ((float)m_lastErrorReportBytes / num < 128f)
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
										{
											ClientErrorReport clientErrorReport = m_errorReportQueue.Peek();
											m_lastErrorReportTime = Time.unscaledTime;
											m_lastErrorReportBytes = clientErrorReport._001D();
											if (ClientGameManager.Get().SendErrorReport(clientErrorReport))
											{
												while (true)
												{
													switch (4)
													{
													case 0:
														break;
													default:
														m_errorReportQueue.Dequeue();
														return;
													}
												}
											}
											Log.Warning("Failed to send exception report");
											return;
										}
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
		foreach (ClientErrorReport item in m_errorReportQueue)
		{
			uint stackTraceHash = item.StackTraceHash;
			if (!m_errorBestiary.ContainsKey(stackTraceHash))
			{
				m_errorBestiary.Add(stackTraceHash, item);
			}
			uint value = 0u;
			if (m_errorUnsentCount.TryGetValue(stackTraceHash, out value))
			{
				m_errorUnsentCount[stackTraceHash] = value + 1;
			}
			else
			{
				m_errorUnsentCount.Add(stackTraceHash, 1u);
			}
		}
		m_errorReportQueue.Clear();
		if (!(num > SecondsBetweenSendingErrorPackets))
		{
			return;
		}
		while (true)
		{
			m_lastErrorReportTime = Time.unscaledTime;
			FlushErrorsToLobby();
			return;
		}
	}

	public bool GetClientErrorReport(uint crashReportHash, out ClientErrorReport clientErrorReport)
	{
		return m_errorBestiary.TryGetValue(crashReportHash, out clientErrorReport);
	}

	public void FlushErrorsToLobby()
	{
		if (!m_errorUnsentCount.IsNullOrEmpty())
		{
			ClientErrorSummary clientErrorSummary = new ClientErrorSummary();
			clientErrorSummary.ReportCount = m_errorUnsentCount;
			ClientErrorSummary summary = clientErrorSummary;
			if (ClientGameManager.Get().SendErrorSummary(summary))
			{
				m_errorUnsentCount.Clear();
			}
			else
			{
				Log.Warning("Failed to send exception summary");
			}
		}
	}
}
