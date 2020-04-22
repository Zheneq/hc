using LobbyGameClientMessages;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class ClientCrashReportDetector : MonoBehaviour
{
	private ClientCrashReportThreadedJob m_threadedJob;

	private string m_crashDumpDirectoryPath;

	private static ClientCrashReportDetector s_instance;

	internal UIReportBugDialogBox m_crashDialog;

	internal static ClientCrashReportDetector Get()
	{
		return s_instance;
	}

	private void Start()
	{
		s_instance = this;
		try
		{
			DirectoryInfo parent = Directory.GetParent(Application.dataPath);
			string text;
			if (parent == null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				text = string.Empty;
			}
			else
			{
				text = parent.FullName;
			}
			string path = text;
			string[] directories = Directory.GetDirectories(path);
			int num = 0;
			while (true)
			{
				if (num >= directories.Length)
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
				string text2 = directories[num];
				if (Directory.GetFiles(text2, "crash.dmp").Length > 0)
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
					m_crashDumpDirectoryPath = text2;
					break;
				}
				if (m_crashDumpDirectoryPath != null)
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
					break;
				}
				num++;
			}
			if (m_crashDumpDirectoryPath != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						Log.Warning("Detected crash dump directory: " + m_crashDumpDirectoryPath);
						if (UIDialogPopupManager.Ready)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									CreateFirstDialog();
									return;
								}
							}
						}
						UIDialogPopupManager.OnReady += HandleUIDialogPopupManagerReady;
						return;
					}
				}
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}

	private void Update()
	{
		if (m_threadedJob == null)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_threadedJob.Update();
			return;
		}
	}

	private void HandleUIDialogPopupManagerReady()
	{
		CreateFirstDialog();
	}

	private void CreateFirstDialog()
	{
		if (ClientMinSpecDetector.BelowMinSpecDetected)
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
					UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("RecoveredFromCrash", "Global"), StringUtil.TR("BelowMinimumSpec", "Global"), StringUtil.TR("Ok", "Global"));
					DeleteCrashDumpDirectory();
					return;
				}
			}
		}
		if (ClientGameManager.Get() != null)
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
			if (ClientGameManager.Get().IsConnectedToLobbyServer)
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
				ClientStatusReport clientStatusReport = new ClientStatusReport();
				clientStatusReport.Status = ClientStatusReport.ClientStatusReportType._001D;
				clientStatusReport.StatusDetails = m_crashDumpDirectoryPath;
				clientStatusReport.DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;
				ClientGameManager.Get().SendStatusReport(clientStatusReport);
				m_crashDialog = UIDialogPopupManager.OpenReportBugDialog(StringUtil.TR("RecoveredFromCrash", "Global"), StringUtil.TR("UploadCrashReport", "Global"), StringUtil.TR("Ok", "Global"), StringUtil.TR("Cancel", "Global"), HandleDialogOKButton, HandleDialogCancelButton);
			}
		}
		if (m_threadedJob != null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			string crashDumpDirectoryPath = m_crashDumpDirectoryPath;
			object arg;
			if (ClientGameManager.Get() != null)
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
				if (ClientGameManager.Get().SessionInfo != null)
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
					arg = ClientGameManager.Get().SessionInfo.SessionToken.ToString();
					goto IL_01ab;
				}
			}
			arg = "unknown";
			goto IL_01ab;
			IL_01ab:
			m_threadedJob = new ClientCrashReportThreadedJob(crashDumpDirectoryPath, BugReportType.Crash, $"SessionToken: {arg}");
			return;
		}
	}

	private void HandleDialogOKButton(UIDialogBox boxReference)
	{
		if (ClientGameManager.Get() != null && ClientGameManager.Get().IsConnectedToLobbyServer)
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
			ClientStatusReport clientStatusReport = new ClientStatusReport();
			clientStatusReport.Status = ClientStatusReport.ClientStatusReportType._0012;
			clientStatusReport.StatusDetails = m_crashDumpDirectoryPath;
			clientStatusReport.DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;
			clientStatusReport.UserMessage = m_crashDialog.m_descriptionBoxInputField.text;
			ClientGameManager.Get().SendStatusReport(clientStatusReport);
		}
		m_crashDialog = null;
	}

	private void HandleDialogCancelButton(UIDialogBox boxReference)
	{
		DeleteCrashDumpDirectory();
		m_crashDialog = null;
	}

	private void DeleteCrashDumpDirectory()
	{
		if (Directory.Exists(m_crashDumpDirectoryPath))
		{
			try
			{
				Directory.Delete(m_crashDumpDirectoryPath, true);
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
			}
		}
	}

	private void OnDestroy()
	{
		if (m_threadedJob != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_threadedJob.Cancel();
		}
		s_instance = null;
	}

	internal void UploadArchive(string crashServerAndArchiveURL, byte[] crashReportBytes, Action<bool> endEvent)
	{
		StartCoroutine(UploadArchiveCoroutine(crashServerAndArchiveURL, crashReportBytes, endEvent));
	}

	private IEnumerator UploadArchiveCoroutine(string crashServerAndArchiveURL, byte[] crashReportBytes, Action<bool> endEvent)
	{
		Log.Info("Attempting to start WWW to post {0} crash report bytes to URL {1}", crashReportBytes.Length, crashServerAndArchiveURL);
		WWW client = new WWW(crashServerAndArchiveURL, crashReportBytes);
		try
		{
			yield return client;
			/*Error: Unable to find new state assignment for yield return*/;
		}
		finally
		{
			base._003C_003E__Finally0();
			goto end_IL_0141;
			IL_0144:
			switch (1)
			{
			default:
				goto end_IL_0141;
			case 0:
				goto IL_0144;
			}
			end_IL_0141:;
		}
	}
}
