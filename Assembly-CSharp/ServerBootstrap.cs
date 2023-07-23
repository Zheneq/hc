// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

// server-only
#if SERVER
public class ServerBootstrap : MonoBehaviour
{
	public string[] m_commandLine;

	private FileLog m_fileLog;
	private FileLog m_debugFileLog;
	private AsyncPump m_asyncPump;

	private void Awake()
	{
		try
		{
			DontDestroyOnLoad(gameObject);
			UnityConsoleLog.MinLevel = Log.Level.Warning;
			UnityConsoleLog.Start();
			ConsoleLog.MinStdOutLevel = Log.Level.Nothing;
			ConsoleLog.MinStdErrLevel = Log.Level.Info;
			ConsoleLog.Start();
			this.m_commandLine = Environment.GetCommandLineArgs();
			this.m_asyncPump = new AsyncPump();
			SynchronizationContext.SetSynchronizationContext(this.m_asyncPump);
			HandleConfig();
			this.ParseCommandLine();
			HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
			hydrogenConfig.ProcessType = ProcessType.AtlasReactorServer;
			if (hydrogenConfig.ProcessCode.IsNullOrEmpty())
			{
				// rogues
				// hydrogenConfig.ProcessCode = ProcessManager.Get().GetNextProcessCode(null, false);
				// custom
				hydrogenConfig.ProcessCode = $"{HydrogenConfig.Get().ServerName}-{ProcessManager.Get().GetNextProcessCode()}";
			}
			if (hydrogenConfig.EnableLogging)
			{
				UnityConsoleLog.MinLevel = Log.FromString(hydrogenConfig.MinConsoleLogLevel);
				UnityConsoleLog.LogInfoAsDebug = true;
				UnityConsoleLog.LogErrorAsWarning = true;
				if (Path.GetFileName(hydrogenConfig.LogFilePath).IsNullOrEmpty())
				{
					hydrogenConfig.LogFilePath = string.Format("{0}/AtlasReactorServer-{1}.log", hydrogenConfig.LogFilePath, hydrogenConfig.ProcessCode);
				}
				this.m_fileLog = new FileLog();
				this.m_fileLog.UseDatedFolder = true;
				this.m_fileLog.MinLevel = Log.FromString(hydrogenConfig.MinFileLogLevel);
				this.m_fileLog.Open(hydrogenConfig.LogFilePath);
				this.m_fileLog.Register();
				if (Path.GetFileName(hydrogenConfig.DebugLogFilePath).IsNullOrEmpty())
				{
					hydrogenConfig.DebugLogFilePath = string.Format("{0}/AtlasReactorServer-{1}.debug.log", hydrogenConfig.DebugLogFilePath, hydrogenConfig.ProcessCode);
				}
				this.m_debugFileLog = new FileLog();
				this.m_debugFileLog.UseDatedFolder = true;
				this.m_debugFileLog.MinLevel = Log.Level.Everything;
				this.m_debugFileLog.Open(hydrogenConfig.DebugLogFilePath);
				this.m_debugFileLog.Register();
			}
			else
			{
				UnityConsoleLog.Stop();
			}
			string buildInfoString = BuildInfo.GetBuildInfoString();
			Log.Notice("{0}", new object[]
			{
				buildInfoString
			});
			Console.WriteLine(buildInfoString);
			Log.Notice("Process: AtlasReactorServer-{0}", new object[]
			{
				hydrogenConfig.ProcessCode
			});
			hydrogenConfig.DevMode = false;
			hydrogenConfig.ServerMode = true;
			if (hydrogenConfig.Ticket == null)
			{
				hydrogenConfig.Ticket = new AuthTicket();
			}
			if (hydrogenConfig.TargetFrameRate < 0 || hydrogenConfig.TargetFrameRate > 60)
			{
				hydrogenConfig.TargetFrameRate = 60;
			}
			Application.targetFrameRate = hydrogenConfig.TargetFrameRate;
			SslValidator.AcceptableSslPolicyErrors = hydrogenConfig.AcceptableSslPolicyErrors;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
			Application.Quit();
		}
	}

	private void Start()
	{
		AppState_Startup.Create();
		AppState_Shutdown.Create();
		AppState_GameLoading.Create();
		AppState_InGameDecision.Create();
		AppState_InGameResolve.Create();
		AppState_InGameStarting.Create();
		AppState_InGameDeployment.Create();
		AppState_InGameEnding.Create();
		AppState_GameTeardown.Create();
		if (HydrogenConfig.Get().AllowDebugCommands)
		{
			DebugParameters.Instantiate();
			DebugCommands.Instantiate();
		}
	}

	private void OnDestroy()
	{
		if (this.m_fileLog != null)
		{
			this.m_fileLog.Close();
		}
	}

	private void Update()
	{
		if (AppState.GetCurrent() == null)
		{
			AppState_Startup.Get().Enter();
		}
		if (this.m_fileLog != null)
		{
			this.m_fileLog.Update();
		}
		if (this.m_asyncPump != null)
		{
			this.m_asyncPump.Run(0);
		}
	}

	private void ParseCommandLine()
	{
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		string str = string.Join(" ", this.m_commandLine);
		Log.Info("Command line: " + str);
		string text = Application.dataPath + "/../../../Build/AtlasReactorServer/Config/AtlasReactorServerConfig.json";
		string text2 = Application.dataPath + "/../../Config/AtlasReactorServerConfig.json";
		string text3 = "";
		if (Application.isEditor)
		{
			text2 = text;
		}
		List<string> list = new List<string>();
		for (int i = 1; i < this.m_commandLine.Length; i++)
		{
			string a = this.m_commandLine[i].ToLower();
			bool hasNext = i + 1 < this.m_commandLine.Length;
			if ((a == "-o" || a == "-option" || a == "--option") && hasNext)
			{
				string[] array = this.m_commandLine[i + 1].Split('=');
				string text4 = array[0];
				string arg = (array.Length > 1) ? array[1] : "True";
				text4 = text4.TrimStart('-');
				string item = string.Format("{{ \"{0}\" : \"{1}\" }}", text4, arg);
				list.Add(item);
			}
			else if ((a == "-c" || a == "-config" || a == "--config") && hasNext)
			{
				text2 = this.m_commandLine[i + 1];
			}
			else if ((a == "-p" || a == "-processcode" || a == "--processcode") && hasNext)
			{
				string arg2 = this.m_commandLine[i + 1];
				string item2 = string.Format("{{ \"ProcessCode\" : \"{0}\" }}", arg2);
				list.Add(item2);
				i++;
			}
			else if ((a == "-s" || a == "-server" || a == "--server") && hasNext)
			{
				string arg3 = this.m_commandLine[i + 1];
				string item3 = string.Format("{{ \"MonitorServerAddress\" : \"{0}\" }}", arg3);
				list.Add(item3);
				i++;
			}
			else if ((a == "-r" || a == "-recommendedport" || a == "--recommendedport") && hasNext)
			{
				string arg4 = this.m_commandLine[i + 1];
				string item4 = string.Format("{{ \"GameServerListenRecommendedPort\" : \"{0}\" }}", arg4);
				list.Add(item4);
				i++;
			}
			else if ((a == "-e" || a == "-environment" || a == "--environment") && hasNext)
			{
				text3 = this.m_commandLine[i + 1];
				i++;
			}
			else if (a == "-batchmode")
			{
				HydrogenConfig.Get().HeadlessMode = true;
			}
		}
		string fileName = text2.Replace(".json", ".local.json");
		string fileName2 = text2.Replace(".json", ".salt.json");
		if (text3.IsNullOrEmpty())
		{
			HydrogenConfig hydrogenConfig2 = new HydrogenConfig();
			hydrogenConfig2.LoadFromFile(fileName2, false);
			text3 = hydrogenConfig2.EnvironmentName;
		}
		if (text3.IsNullOrEmpty())
		{
			HydrogenConfig hydrogenConfig3 = new HydrogenConfig();
			hydrogenConfig3.LoadFromFile(fileName, false);
			text3 = hydrogenConfig3.EnvironmentName;
		}
		Log.Notice("Configured for {0} environment", new object[]
		{
			(!text3.IsNullOrEmpty()) ? text3 : "default"
		});
		hydrogenConfig.LoadFromFile(text2, false);
		if (!text3.IsNullOrEmpty())
		{
			string newValue = string.Format(".{0}.json", text3);
			string fileName3 = text2.Replace(".json", newValue);
			hydrogenConfig.LoadFromFile(fileName3, false);
		}
		hydrogenConfig.LoadFromFile(fileName2, false);
		hydrogenConfig.LoadFromFile(fileName, false);
		foreach (string data in list)
		{
			hydrogenConfig.Load(data);  // LoadAppend(data) in rogues
		}
		CommonServerConfig commonServerConfig = CommonServerConfig.Get();
		if (commonServerConfig != null)
		{
			foreach (string data2 in list)
			{
				commonServerConfig.Load(data2);
			}
		}
		commonServerConfig.ProcessType = ProcessType.AtlasReactorServer;
		hydrogenConfig.EnvironmentName = text3;
	}

	public static void CheckConfig()
	{
		CommonServerConfig commonServerConfig = CommonServerConfig.Get();
		if (commonServerConfig != null && commonServerConfig.HasBeenModified())
		{
			Log.Notice("Configuration files have changed, reloading");
			HandleConfig();
		}
	}

	private static void HandleConfig()
	{
		string text = "unknown";
		try
		{
			string text2 = Application.dataPath + "/../../../Build/AtlasReactorServer/Config/AtlasReactorServerConfig.json";
			text = Application.dataPath + "/../../Config/AtlasReactorServerConfig.json";  // custom (Config folder is different in Rogues)
			if (Application.isEditor)
			{
				text = text2;
			}
			CommonServerConfig commonServerConfig = new CommonServerConfig();
			commonServerConfig.Initialize(text);
			CommonServerConfig.Set(commonServerConfig);
		}
		catch (Exception ex)
		{
			throw new Exception(string.Format("Failed to load {0}: {1}", text, ex.Message));
		}
	}
}
#endif
