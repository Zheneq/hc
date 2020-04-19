using System;
using System.Collections;
using System.IO;
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
			string text;
			if (parent == null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientCrashReportDetector.Start()).MethodHandle;
				}
				text = string.Empty;
			}
			else
			{
				text = parent.FullName;
			}
			string path = text;
			string[] directories = Directory.GetDirectories(path);
			int i = 0;
			while (i < directories.Length)
			{
				string text2 = directories[i];
				if (Directory.GetFiles(text2, "crash.dmp").Length > 0)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_crashDumpDirectoryPath = text2;
				}
				else
				{
					if (this.m_crashDumpDirectoryPath == null)
					{
						i++;
						continue;
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				IL_9A:
				if (this.m_crashDumpDirectoryPath != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					Log.Warning("Detected crash dump directory: " + this.m_crashDumpDirectoryPath, new object[0]);
					if (UIDialogPopupManager.Ready)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						this.CreateFirstDialog();
					}
					else
					{
						UIDialogPopupManager.OnReady += this.HandleUIDialogPopupManagerReady;
					}
				}
				return;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				goto IL_9A;
			}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientCrashReportDetector.Update()).MethodHandle;
			}
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientCrashReportDetector.CreateFirstDialog()).MethodHandle;
			}
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("RecoveredFromCrash", "Global"), StringUtil.TR("BelowMinimumSpec", "Global"), StringUtil.TR("Ok", "Global"), null, -1, false);
			this.DeleteCrashDumpDirectory();
		}
		else
		{
			if (ClientGameManager.Get() != null)
			{
				for (;;)
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
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					ClientStatusReport clientStatusReport = new ClientStatusReport();
					clientStatusReport.Status = ClientStatusReport.ClientStatusReportType.\u001D;
					clientStatusReport.StatusDetails = this.m_crashDumpDirectoryPath;
					clientStatusReport.DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;
					ClientGameManager.Get().SendStatusReport(clientStatusReport);
					this.m_crashDialog = UIDialogPopupManager.OpenReportBugDialog(StringUtil.TR("RecoveredFromCrash", "Global"), StringUtil.TR("UploadCrashReport", "Global"), StringUtil.TR("Ok", "Global"), StringUtil.TR("Cancel", "Global"), new UIDialogBox.DialogButtonCallback(this.HandleDialogOKButton), new UIDialogBox.DialogButtonCallback(this.HandleDialogCancelButton));
				}
			}
			if (this.m_threadedJob == null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				string crashDumpDirectoryPath = this.m_crashDumpDirectoryPath;
				BugReportType bugReportType = BugReportType.Crash;
				string format = "SessionToken: {0}";
				object arg;
				if (ClientGameManager.Get() != null)
				{
					for (;;)
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
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientCrashReportDetector.HandleDialogOKButton(UIDialogBox)).MethodHandle;
			}
			ClientStatusReport clientStatusReport = new ClientStatusReport();
			clientStatusReport.Status = ClientStatusReport.ClientStatusReportType.\u0012;
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientCrashReportDetector.OnDestroy()).MethodHandle;
			}
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
		uint num;
		WWW client;
		switch (num)
		{
		case 0U:
			Log.Info("Attempting to start WWW to post {0} crash report bytes to URL {1}", new object[]
			{
				crashReportBytes.Length,
				crashServerAndArchiveURL
			});
			client = new WWW(crashServerAndArchiveURL, crashReportBytes);
			break;
		case 1U:
			break;
		default:
			yield break;
		}
		try
		{
			yield return client;
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientCrashReportDetector.<UploadArchiveCoroutine>c__Iterator0.MoveNext()).MethodHandle;
			}
			flag = true;
			if (string.IsNullOrEmpty(client.error))
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				string message = "\nResponse from Crash Service received was {0}";
				object[] array = new object[1];
				int num2 = 0;
				object obj;
				if (client.text == null)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			else if (client != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(ClientCrashReportDetector.<UploadArchiveCoroutine>c__Iterator0.<>__Finally0()).MethodHandle;
				}
				((IDisposable)client).Dispose();
			}
		}
		yield break;
	}
}
