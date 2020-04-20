using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using I2.Loc;
using LobbyGameClientMessages;
using UnityEngine;

public class ClientBootstrap : MonoBehaviour
{
	private FileLog m_fileLog;

	private string[] m_commandLine;

	private AsyncPump m_asyncPump;

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		UnityConsoleLog.MinLevel = Log.Level.Warning;
		UnityConsoleLog.Start();
		this.m_commandLine = Environment.GetCommandLineArgs();
		this.m_asyncPump = new AsyncPump();
		SynchronizationContext.SetSynchronizationContext(this.m_asyncPump);
		this.ParseCommandLine();
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		hydrogenConfig.ProcessType = ProcessType.AtlasReactor;
		if (hydrogenConfig.ProcessCode.IsNullOrEmpty())
		{
			hydrogenConfig.ProcessCode = ProcessManager.Get().GetNextProcessCode(null, true);
		}
		if (hydrogenConfig.EnableLogging)
		{
			UnityConsoleLog.MinLevel = Log.FromString(hydrogenConfig.MinConsoleLogLevel);
			if (Path.GetFileName(hydrogenConfig.LogFilePath).IsNullOrEmpty())
			{
				hydrogenConfig.LogFilePath = string.Format("{0}/AtlasReactor-{1}.log", hydrogenConfig.LogFilePath, hydrogenConfig.ProcessCode);
			}
			this.m_fileLog = new FileLog();
			this.m_fileLog.UseDatedFolder = true;
			this.m_fileLog.MinLevel = Log.FromString(hydrogenConfig.MinFileLogLevel);
			this.m_fileLog.Open(hydrogenConfig.LogFilePath);
			this.m_fileLog.Register();
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
		if (!hydrogenConfig.ProcessCode.IsNullOrEmpty())
		{
			Log.Notice("Process: AtlasReactor-{0}", new object[]
			{
				hydrogenConfig.ProcessCode
			});
		}
		if (hydrogenConfig.Language == null)
		{
			hydrogenConfig.Language = "en";
		}
		LocalizationManager.SetBootupLanguage(hydrogenConfig.Language);
		hydrogenConfig.Language = LocalizationManager.CurrentLanguageCode;
		hydrogenConfig.ServerMode = false;
		SslValidator.AcceptableSslPolicyErrors = hydrogenConfig.AcceptableSslPolicyErrors;
		ClientBootstrap.Instance = this;
	}

	private void Start()
	{
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
		AppState_InGameDecision.Create();
		AppState_InGameResolve.Create();
		AppState_InGameStarting.Create();
		AppState_InGameDeployment.Create();
		AppState_InGameEnding.Create();
		AppState_GameLoading.Create();
		AppState_GroupCharacterSelect.Create();
		AppState_RankModeDraft.Create();
		AppState_LandingPage.Create();
		if (base.GetComponent<ClientIdleTimer>() == null)
		{
			base.gameObject.AddComponent<ClientIdleTimer>();
		}
		CommerceClient.Get();
		DebugCommands.Instantiate();
		SlashCommands.Instantiate();
		TextConsole.Instantiate();
		if (ClientBootstrap.LoadTest)
		{
			ClientGameManager.Get().OnConnectedToLobbyServer += this.HandleConnectedToLobbyServer;
		}
	}

	private void Update()
	{
		if (AppState.GetCurrent() == null)
		{
			AppState_Startup.Get().Enter();
		}
		if (ClientBootstrap.LoadTest)
		{
			if (AppState.GetCurrent() == AppState_LandingPage.Get())
			{
				if (UIFrontEnd.Get() != null)
				{
					AppState_LandingPage.Get().OnQuickPlayClicked();
					ClientGameManager.Get().GroupInfo.SelectedQueueType = GameType.PvP;
					AppState_GroupCharacterSelect.Get().UpdateReadyState(true);
				}
			}
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

	private void OnDestroy()
	{
		if (this.m_fileLog != null)
		{
			this.m_fileLog.Close();
		}
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnConnectedToLobbyServer -= this.HandleConnectedToLobbyServer;
		}
	}

	private void ParseCommandLine()
	{
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		string str = string.Join(" ", this.m_commandLine);
		Log.Info("Command line: " + str, new object[0]);
		string text = Application.dataPath + "/../../../Build/AtlasReactor/Config/AtlasReactorConfig.json";
		string text2 = Application.dataPath + "/../../Config/AtlasReactorConfig.json";
		string text3 = string.Empty;
		if (Application.isEditor)
		{
			text2 = text;
			text3 = "dev";
		}
		List<string> list = new List<string>();
		int i = 1;
		while (i < this.m_commandLine.Length)
		{
			string a = this.m_commandLine[i].ToLower();
			if (a == "-o")
			{
				goto IL_F1;
			}
			if (a == "-option")
			{
				goto IL_F1;
			}
			if (!(a == "--option"))
			{
				goto IL_18D;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				goto IL_F1;
			}
			IL_4CD:
			i++;
			continue;
			IL_F1:
			if (i + 1 < this.m_commandLine.Length)
			{
				string text4 = this.m_commandLine[i + 1];
				string[] array = text4.Split(new char[]
				{
					'='
				});
				string text5 = array[0];
				string text6;
				if (array.Length > 1)
				{
					text6 = array[1];
				}
				else
				{
					text6 = "True";
				}
				string arg = text6;
				text5 = text5.TrimStart(new char[]
				{
					'-'
				});
				string item = string.Format("{{ \"{0}\" : \"{1}\" }}", text5, arg);
				list.Add(item);
				i++;
				goto IL_4CD;
			}
			IL_18D:
			if (!(a == "-c") && !(a == "-config"))
			{
				if (!(a == "--config"))
				{
					goto IL_1EA;
				}
			}
			if (i + 1 < this.m_commandLine.Length)
			{
				text2 = this.m_commandLine[i + 1];
				i++;
				goto IL_4CD;
			}
			IL_1EA:
			if (!(a == "-s"))
			{
				if (!(a == "-server"))
				{
					if (!(a == "--server"))
					{
						goto IL_281;
					}
				}
			}
			if (i + 1 < this.m_commandLine.Length)
			{
				string arg2 = this.m_commandLine[i + 1];
				string item2 = string.Format("{{ \"DirectoryServerAddress\" : \"{0}\" }}", arg2);
				list.Add(item2);
				i++;
				goto IL_4CD;
			}
			IL_281:
			if (!(a == "-p"))
			{
				if (!(a == "-processcode"))
				{
					if (!(a == "--processcode"))
					{
						goto IL_302;
					}
				}
			}
			if (i + 1 < this.m_commandLine.Length)
			{
				string arg3 = this.m_commandLine[i + 1];
				string item3 = string.Format("{{ \"ProcessCode\" : \"{0}\" }}", arg3);
				list.Add(item3);
				i++;
				goto IL_4CD;
			}
			IL_302:
			if (!(a == "-t") && !(a == "-ticket"))
			{
				if (!(a == "--ticket"))
				{
					goto IL_39A;
				}
			}
			if (i + 1 < this.m_commandLine.Length)
			{
				string text7 = this.m_commandLine[i + 1];
				text7 = text7.Replace('\\', '/');
				string item4 = string.Format("{{ \"TicketFile\" : \"{0}\" }}", text7);
				list.Add(item4);
				i++;
				goto IL_4CD;
			}
			IL_39A:
			if (!(a == "-l"))
			{
				if (!(a == "-language"))
				{
					if (!(a == "--language"))
					{
						goto IL_434;
					}
				}
			}
			if (i + 1 < this.m_commandLine.Length)
			{
				string text8 = this.m_commandLine[i + 1];
				text8 = text8.Replace('\\', '/');
				string item5 = string.Format("{{ \"Language\" : \"{0}\" }}", text8);
				list.Add(item5);
				i++;
				goto IL_4CD;
			}
			IL_434:
			if (!(a == "-e"))
			{
				if (!(a == "-environment"))
				{
					if (!(a == "--environment"))
					{
						goto IL_4AD;
					}
				}
			}
			if (i + 1 < this.m_commandLine.Length)
			{
				text3 = this.m_commandLine[i + 1];
				i++;
				goto IL_4CD;
			}
			IL_4AD:
			if (a == "-loadtest")
			{
				ClientBootstrap.LoadTest = true;
				goto IL_4CD;
			}
			goto IL_4CD;
		}
		if (ClientBootstrap.LoadTest)
		{
			Debug.Log("-loadtest");
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
			text3.IsNullOrEmpty() ? "default" : text3
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
		using (List<string>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string data = enumerator.Current;
				hydrogenConfig.Load(data);
			}
		}
		hydrogenConfig.EnvironmentName = text3;
	}

	public void WriteLine(string format, params object[] args)
	{
		if (this.m_fileLog != null)
		{
			if (this.m_fileLog.File != null)
			{
				this.m_fileLog.File.Write(DateTime.Now.ToString(Log.TimestampFormat) + " [SYS] ");
				this.m_fileLog.File.WriteLine(format, args);
				this.m_fileLog.File.Flush();
			}
		}
	}

	private void OnApplicationQuit()
	{
		this.WriteLine("Quit application", new object[0]);
	}

	private void HandleConnectedToLobbyServer(RegisterGameClientResponse response)
	{
	}

	private void HandlePlayerInfoUpdateResponse(PlayerInfoUpdateResponse response)
	{
		string message = "Loadtest lobby ready response: {0}";
		object[] array = new object[1];
		int num = 0;
		object obj;
		if (response.Success)
		{
			obj = "Success";
		}
		else
		{
			obj = response.ErrorMessage;
		}
		array[num] = obj;
		Log.Info(message, array);
		LobbyGameConfig lobbyGameConfig = new LobbyGameConfig();
		lobbyGameConfig.GameType = GameType.Coop;
		Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(lobbyGameConfig.GameType);
		if (!gameTypeSubTypes.IsNullOrEmpty<KeyValuePair<ushort, GameSubType>>())
		{
			lobbyGameConfig.InstanceSubTypeBit = gameTypeSubTypes.First<KeyValuePair<ushort, GameSubType>>().Key;
			lobbyGameConfig.SubTypes = gameTypeSubTypes.Values.ToList<GameSubType>();
			using (Dictionary<ushort, GameSubType>.Enumerator enumerator = gameTypeSubTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ushort, GameSubType> keyValuePair = enumerator.Current;
					if (keyValuePair.Value.HasMod(GameSubType.SubTypeMods.AntiSocial))
					{
						lobbyGameConfig.InstanceSubTypeBit = keyValuePair.Key;
						goto IL_FA;
					}
				}
			}
		}
		IL_FA:
		BotDifficulty selectedBotSkillTeamA = BotDifficulty.Easy;
		BotDifficulty selectedBotSkillTeamB = BotDifficulty.Easy;
		ClientGameManager.Get().CreateGame(lobbyGameConfig, ReadyState.Ready, selectedBotSkillTeamA, selectedBotSkillTeamB, delegate(CreateGameResponse r)
		{
			if (!r.Success)
			{
				Log.Warning("Failed to create game: {0}", new object[]
				{
					r.ErrorMessage
				});
			}
		});
	}

	internal string GetFileLogCurrentPath()
	{
		string result;
		if (this.m_fileLog == null)
		{
			result = string.Empty;
		}
		else
		{
			result = this.m_fileLog.CurrentFilePath;
		}
		return result;
	}

	internal static ClientBootstrap Instance { get; private set; }

	internal static bool LoadTest { get; private set; }
}
