// ROGUES
using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
//using CoOp;
using I2.Loc;
using UnityEngine;

public class ClientBootstrap : MonoBehaviour
{
	private FileLog m_fileLog;
	private string[] m_commandLine;
	private AsyncPump m_asyncPump;

#if !VANILLA && !SERVER
    // Custom titles
    private float timeSinceLastRefresh = 0f;
    private const float refreshInterval = 300f; //5min
#endif

    internal static ClientBootstrap Instance { get; private set; }
	internal static bool LoadTest { get; private set; }

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		UnityConsoleLog.MinLevel = Log.Level.Warning;
		UnityConsoleLog.Start();
		m_commandLine = Environment.GetCommandLineArgs();
		m_asyncPump = new AsyncPump();
		SynchronizationContext.SetSynchronizationContext(m_asyncPump);
#if SERVER
		InitializeServerConfig(); // added in rogues
#endif
		ParseCommandLine();
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		hydrogenConfig.ProcessType = ProcessType.AtlasReactor;  // ProcessType.AtlasRogues in rogues
		if (hydrogenConfig.ProcessCode.IsNullOrEmpty())
		{
			hydrogenConfig.ProcessCode = ProcessManager.Get().GetNextProcessCode(null, true);  // .GetNextProcessCode(null, false) in rogues
		}
		if (hydrogenConfig.EnableLogging)
		{
			UnityConsoleLog.MinLevel = Log.FromString(hydrogenConfig.MinConsoleLogLevel);
			if (Path.GetFileName(hydrogenConfig.LogFilePath).IsNullOrEmpty())
			{
				hydrogenConfig.LogFilePath = $"{hydrogenConfig.LogFilePath}/AtlasReactor-{hydrogenConfig.ProcessCode}.log";   // AtlasRogues- in rogues
			}
			m_fileLog = new FileLog
			{
				UseDatedFolder = true,
				MinLevel = Log.FromString(hydrogenConfig.MinFileLogLevel)
			};
			m_fileLog.Open(hydrogenConfig.LogFilePath);
			m_fileLog.Register();
		}
		else
		{
			UnityConsoleLog.Stop();
		}
		string buildInfoString = BuildInfo.GetBuildInfoString();
		Log.Notice("{0}", buildInfoString);
		Console.WriteLine(buildInfoString);

		// reactor
		if (!hydrogenConfig.ProcessCode.IsNullOrEmpty())
		{
			Log.Notice("Process: AtlasReactor-{0}", hydrogenConfig.ProcessCode);
		}
		// rogues
		//Log.Notice("Process: AtlasRogues-{0}", hydrogenConfig.ProcessCode);

		//hydrogenConfig.DevMode = false;  // added in rogues
		if (hydrogenConfig.Language == null)
		{
			hydrogenConfig.Language = "en";
		}
		LocalizationManager.SetBootupLanguage(hydrogenConfig.Language);
		hydrogenConfig.Language = LocalizationManager.CurrentLanguageCode;  // removed in rogues
		hydrogenConfig.ServerMode = false;
		SslValidator.AcceptableSslPolicyErrors = hydrogenConfig.AcceptableSslPolicyErrors;
		Instance = this;
	}

	private void Start()
	{
		// rogues
		//HydrogenConfig hydrogenConfig = HydrogenConfig.Get();

		AppState_Startup.Create();
		AppState_Shutdown.Create();
		AppState_LandingPage.Create();
		AppState_FrontendLoadingScreen.Create();
		AppState_GameTypeSelect.Create();
		AppState_CharacterSelect.Create();
		AppState_JoinGame.Create();
		AppState_CreateGame.Create();
		AppState_WaitingForGame.Create();
		AppState_FoundGame.Create();
		AppState_GameTeardown.Create();
		AppState_FullScreenMovie.Create();

		// rogues
		//if (hydrogenConfig.UseMissionFrontendFlow)
		//{
		//	if (hydrogenConfig.UseMissionSelect)
		//	{
		//		AppState_MissionSelect.Create();
		//		AppState_MissionCharacterSelect.Create();
		//	}
		//	else
		//	{
		//		AppState_AllstarsTitle.Create();
		//		AppState_AllstarsMap.Create();
		//		AppState_AllstarsCharacter.Create();
		//	}
		//}

		AppState_InGameDecision.Create();
		AppState_InGameResolve.Create();
		AppState_InGameStarting.Create();
		AppState_InGameDeployment.Create();
		AppState_InGameEnding.Create();
		AppState_GameLoading.Create();
		AppState_GroupCharacterSelect.Create();
		AppState_RankModeDraft.Create(); // removed in rogues
#if VANILLA
		AppState_LandingPage.Create(); // duplicate removed in rogues
#endif
		if (GetComponent<ClientIdleTimer>() == null)
		{
			gameObject.AddComponent<ClientIdleTimer>();
		}
		CommerceClient.Get();
		DebugCommands.Instantiate(); // removed in rogues
		SlashCommands.Instantiate();
		TextConsole.Instantiate();
		if (LoadTest)
		{
			ClientGameManager.Get().OnConnectedToLobbyServer += HandleConnectedToLobbyServer;
		}

#if !VANILLA && !SERVER
        // Custom titles Init and fetch
        PlayerTitleManager.GetInstance().Init();
#endif
	}

	private void Update()
	{
		if (AppState.GetCurrent() == null)
		{
			AppState_Startup.Get().Enter();
		}
		if (LoadTest
		    && AppState.GetCurrent() == AppState_LandingPage.Get()
		    && UIFrontEnd.Get() != null)
		{
			// broken code in rogues
			AppState_LandingPage.Get().OnQuickPlayClicked();
			ClientGameManager.Get().GroupInfo.SelectedQueueType = GameType.PvP;
			AppState_GroupCharacterSelect.Get().UpdateReadyState(true);
		}
		if (m_fileLog != null)
		{
			m_fileLog.Update();
		}
		if (m_asyncPump != null)
		{
			m_asyncPump.Run(0);
		}

#if !VANILLA && !SERVER
        // Custom titles refresh
        timeSinceLastRefresh += Time.deltaTime;
        if (timeSinceLastRefresh >= refreshInterval)
        {
            PlayerTitleManager.GetInstance().RefreshTitles();
            timeSinceLastRefresh = 0f;
        }
#endif
    }

    private void OnDestroy()
	{
		if (m_fileLog != null)
		{
			m_fileLog.Close();
		}
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnConnectedToLobbyServer -= HandleConnectedToLobbyServer;
		}
	}

	// reactor
	private void ParseCommandLine()
	{
		HydrogenConfig envConfig = HydrogenConfig.Get();
		Log.Info("Command line: " + string.Join(" ", m_commandLine));
		string buildConfigPath = Application.dataPath + "/../../../Build/AtlasReactor/Config/AtlasReactorConfig.json";
		string configPath = Application.dataPath + "/../../Config/AtlasReactorConfig.json";
		string environment = "";
		if (Application.isEditor)
		{
			configPath = buildConfigPath;
			environment = "dev";
		}
		List<string> cliConfig = new List<string>();
		for (int i = 1; i < m_commandLine.Length; i++)
		{
			string cliItem = m_commandLine[i].ToLower();
			bool hasNext = i + 1 < m_commandLine.Length;
			if ((cliItem == "-o" || cliItem == "-option" || cliItem == "--option") && hasNext)
			{
				string[] optionAndValue = m_commandLine[i + 1].Split('=');
				string option = optionAndValue[0];
				string value = optionAndValue.Length > 1 ? optionAndValue[1] : "True";
				option = option.TrimStart('-');
				cliConfig.Add($"{{ \"{option}\" : \"{value}\" }}");
				i++;
			}
			else if ((cliItem == "-c" || cliItem == "-config" || cliItem == "--config") && hasNext)
			{
				configPath = m_commandLine[i + 1];
				i++;
			}
			else if ((cliItem == "-s" || cliItem == "-server" || cliItem == "--server") && hasNext)
			{
				cliConfig.Add($"{{ \"DirectoryServerAddress\" : \"{m_commandLine[i + 1]}\" }}");
				i++;
			}
			else if ((cliItem == "-p" || cliItem == "-processcode" || cliItem == "--processcode") && hasNext)
			{
				cliConfig.Add($"{{ \"ProcessCode\" : \"{m_commandLine[i + 1]}\" }}");
				i++;
			}
			else if ((cliItem == "-t" || cliItem == "-ticket" || cliItem == "--ticket") && hasNext)
			{
				cliConfig.Add($"{{ \"TicketFile\" : \"{m_commandLine[i + 1].Replace('\\', '/')}\" }}");
				i++;
			}
			else if ((cliItem == "-l" || cliItem == "-language" || cliItem == "--language") && hasNext)
			{
				cliConfig.Add($"{{ \"Language\" : \"{m_commandLine[i + 1].Replace('\\', '/')}\" }}");
				i++;
			}
			else if ((cliItem == "-e" || cliItem == "-environment" || cliItem == "--environment") && hasNext)
			{
				environment = m_commandLine[i + 1];
				i++;
			}
			else if (cliItem == "-loadtest")
			{
				LoadTest = true;
			}
		}
		if (LoadTest)
		{
			Debug.Log("-loadtest");
		}
		string localConfigPath = configPath.Replace(".json", ".local.json");
		string saltConfigPath = configPath.Replace(".json", ".salt.json");
		if (environment.IsNullOrEmpty())
		{
			HydrogenConfig saltConfig = new HydrogenConfig();
			saltConfig.LoadFromFile(saltConfigPath, false);
			environment = saltConfig.EnvironmentName;
		}
		if (environment.IsNullOrEmpty())
		{
			HydrogenConfig localConfig = new HydrogenConfig();
			localConfig.LoadFromFile(localConfigPath, false);
			environment = localConfig.EnvironmentName;
		}
		Log.Notice("Configured for {0} environment", environment.IsNullOrEmpty() ? "default" : environment);
		envConfig.LoadFromFile(configPath, false);
		if (!environment.IsNullOrEmpty())
		{
			string envConfigPath = configPath.Replace(".json", $".{environment}.json");
			envConfig.LoadFromFile(envConfigPath, false);
		}
		envConfig.LoadFromFile(saltConfigPath, false);
		envConfig.LoadFromFile(localConfigPath, false);
		foreach (string param in cliConfig)
		{
			envConfig.Load(param);
		}
		envConfig.EnvironmentName = environment;
	}

	// removed in rogues
	public void WriteLine(string format, params object[] args)
	{
		if (m_fileLog != null && m_fileLog.File != null)
		{
			m_fileLog.File.Write(DateTime.Now.ToString(Log.TimestampFormat) + " [SYS] ");
			m_fileLog.File.WriteLine(format, args);
			m_fileLog.File.Flush();
		}
	}

	// added in rogues
#if SERVER
	private void InitializeServerConfig()
	{
		string text = "unknown";
		try
		{
			text = Application.dataPath + "/../Config/CommonServerConfig.json";
			if (Application.isEditor)
			{
				text = Application.dataPath + "/../../../Build/AtlasRogues/Config/CommonServerConfig.json";
			}
			CommonServerConfig commonServerConfig = new CommonServerConfig();
			commonServerConfig.Initialize(text);
			CommonServerConfig.Set(commonServerConfig);
		}
		catch (Exception ex)
		{
			Log.Warning("Failed to load {0}: {1}", new object[]
			{
				text,
				ex.Message
			});
		}
	}
#endif

	private void OnApplicationQuit()
	{
		// removed in rogues
		WriteLine("Quit application");
	}

	private void HandleConnectedToLobbyServer(RegisterGameClientResponse response)
	{
	}

	private void HandlePlayerInfoUpdateResponse(PlayerInfoUpdateResponse response)
	{
		string responseStr = response.Success ? "Success" : response.ErrorMessage;
		Log.Info($"Loadtest lobby ready response: {responseStr}");
		// removed in rogues
		LobbyGameConfig lobbyGameConfig = new LobbyGameConfig
		{
			GameType = GameType.Coop
		};
		Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(lobbyGameConfig.GameType);
		if (!gameTypeSubTypes.IsNullOrEmpty())
		{
			lobbyGameConfig.InstanceSubTypeBit = gameTypeSubTypes.First().Key;
			lobbyGameConfig.SubTypes = gameTypeSubTypes.Values.ToList();
			foreach (KeyValuePair<ushort, GameSubType> subType in gameTypeSubTypes)
			{
				if (subType.Value.HasMod(GameSubType.SubTypeMods.AntiSocial))
				{
					lobbyGameConfig.InstanceSubTypeBit = subType.Key;
					break;
				}
			}
		}
		ClientGameManager.Get().CreateGame(
			lobbyGameConfig,  // null in rogues
			ReadyState.Ready,
			BotDifficulty.Easy,  // 0 in rogues
			BotDifficulty.Easy,  // 0 in rogues
			delegate (CreateGameResponse r)
			{
				if (!r.Success)
				{
					Log.Warning("Failed to create game: {0}", r.ErrorMessage);
					return;
				}
			});
	}

	internal string GetFileLogCurrentPath()
	{
		return m_fileLog != null ? m_fileLog.CurrentFilePath : "";
	}
}
