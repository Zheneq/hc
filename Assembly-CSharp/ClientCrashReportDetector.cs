using System;
using System.Collections;
using System.IO;
using System.Text;
using LobbyGameClientMessages;
using UnityEngine;

public class ClientCrashReportDetector : MonoBehaviour
{
	private ClientCrashReportThreadedJob m_threadedJob;

	private string m_crashDumpDirectoryPath;

	private static ClientCrashReportDetector s_instance;

	internal UIReportBugDialogBox m_crashDialog;

	internal static ClientCrashReportDetector Get()
	{
		return ClientCrashReportDetector.s_instance;
	}

	private void Start()
	{
		ClientCrashReportDetector.s_instance = this;
		try
		{
			DirectoryInfo parent = Directory.GetParent(Application.dataPath);
			string path = parent?.FullName ?? string.Empty;
			string[] directories = Directory.GetDirectories(path);
			int i = 0;
			while (i < directories.Length)
			{
				string text2 = directories[i];
				if (Directory.GetFiles(text2, "crash.dmp").Length > 0)
				{
					this.m_crashDumpDirectoryPath = text2;
				}
				else
				{
					if (this.m_crashDumpDirectoryPath == null)
					{
						i++;
						continue;
					}
				}
				break;
			}
			if (this.m_crashDumpDirectoryPath != null)
			{
				Log.Warning(new StringBuilder().Append("Detected crash dump directory: ").Append(this.m_crashDumpDirectoryPath).ToString(), new object[0]);
				if (UIDialogPopupManager.Ready)
				{
					this.CreateFirstDialog();
				}
				else
				{
					UIDialogPopupManager.OnReady += this.HandleUIDialogPopupManagerReady;
				}
			}
			return;

		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}

	private void Update()
	{
		if (this.m_threadedJob != null)
		{
			this.m_threadedJob.Update();
		}
	}

	private void HandleUIDialogPopupManagerReady()
	{
		this.CreateFirstDialog();
	}

	private void CreateFirstDialog()
	{
		if (ClientMinSpecDetector.BelowMinSpecDetected)
		{
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("RecoveredFromCrash", "Global"), StringUtil.TR("BelowMinimumSpec", "Global"), StringUtil.TR("Ok", "Global"), null, -1, false);
			this.DeleteCrashDumpDirectory();
		}
		else
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().IsConnectedToLobbyServer)
				{
					ClientStatusReport clientStatusReport = new ClientStatusReport();
					clientStatusReport.Status = ClientStatusReport.ClientStatusReportType._001D;
					clientStatusReport.StatusDetails = this.m_crashDumpDirectoryPath;
					clientStatusReport.DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;
					ClientGameManager.Get().SendStatusReport(clientStatusReport);
					this.m_crashDialog = UIDialogPopupManager.OpenReportBugDialog(StringUtil.TR("RecoveredFromCrash", "Global"), StringUtil.TR("UploadCrashReport", "Global"), StringUtil.TR("Ok", "Global"), StringUtil.TR("Cancel", "Global"), new UIDialogBox.DialogButtonCallback(this.HandleDialogOKButton), new UIDialogBox.DialogButtonCallback(this.HandleDialogCancelButton));
				}
			}
			if (this.m_threadedJob == null)
			{
				string crashDumpDirectoryPath = this.m_crashDumpDirectoryPath;
				BugReportType bugReportType = BugReportType.Crash;
				string format = "SessionToken: {0}";
				object arg;
				if (ClientGameManager.Get() != null)
				{
					if (ClientGameManager.Get().SessionInfo != null)
					{
						arg = ClientGameManager.Get().SessionInfo.SessionToken.ToString();
						goto IL_1AB;
					}
				}
				arg = "unknown";
				IL_1AB:
				this.m_threadedJob = new ClientCrashReportThreadedJob(crashDumpDirectoryPath, bugReportType, string.Format(format, arg), null);
			}
		}
	}

	private void HandleDialogOKButton(UIDialogBox boxReference)
	{
		if (ClientGameManager.Get() != null && ClientGameManager.Get().IsConnectedToLobbyServer)
		{
			ClientStatusReport clientStatusReport = new ClientStatusReport();
			clientStatusReport.Status = ClientStatusReport.ClientStatusReportType._0012;
			clientStatusReport.StatusDetails = this.m_crashDumpDirectoryPath;
			clientStatusReport.DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;
			clientStatusReport.UserMessage = this.m_crashDialog.m_descriptionBoxInputField.text;
			ClientGameManager.Get().SendStatusReport(clientStatusReport);
		}
		this.m_crashDialog = null;
	}

	private void HandleDialogCancelButton(UIDialogBox boxReference)
	{
		this.DeleteCrashDumpDirectory();
		this.m_crashDialog = null;
	}

	private void DeleteCrashDumpDirectory()
	{
		if (Directory.Exists(this.m_crashDumpDirectoryPath))
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
	}

	private void OnDestroy()
	{
		if (this.m_threadedJob != null)
		{
			this.m_threadedJob.Cancel();
		}
		ClientCrashReportDetector.s_instance = null;
	}

	internal void UploadArchive(string crashServerAndArchiveURL, byte[] crashReportBytes, Action<bool> endEvent)
	{
		base.StartCoroutine(this.UploadArchiveCoroutine(crashServerAndArchiveURL, crashReportBytes, endEvent));
	}

	private IEnumerator UploadArchiveCoroutine(string crashServerAndArchiveURL, byte[] crashReportBytes, Action<bool> endEvent)
	{
		bool flag = false;
		WWW client;
		Log.Info("Attempting to start WWW to post {0} crash report bytes to URL {1}", new object[]
		{
			crashReportBytes.Length,
			crashServerAndArchiveURL
		});
		client = new WWW(crashServerAndArchiveURL, crashReportBytes);
			
		try
		{
			yield return client;
			flag = true;
			if (string.IsNullOrEmpty(client.error))
			{
				string message = "\nResponse from Crash Service received was {0}";
				object[] array = new object[1];
				int num2 = 0;
				object obj;
				if (client.text == null)
				{
					obj = "NULL";
				}
				else
				{
					obj = client.text;
				}
				array[num2] = obj;
				Log.Info(message, array);
				endEvent(true);
			}
			else
			{
				Log.Error("\nError from Crash Service received was {0}", new object[]
				{
					client.error
				});
				endEvent(false);
			}
		}
		finally
		{
			if (flag)
			{
			}
			else if (client != null)
			{
				((IDisposable)client).Dispose();
			}
		}
		yield break;
	}
}
