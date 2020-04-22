using I2.Loc;
using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

public class ClientBootstrap : MonoBehaviour
{
	private FileLog m_fileLog;

	private string[] m_commandLine;

	private AsyncPump m_asyncPump;

	internal static ClientBootstrap Instance
	{
		get;
		private set;
	}

	internal static bool LoadTest
	{
		get;
		private set;
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		UnityConsoleLog.MinLevel = Log.Level.Warning;
		UnityConsoleLog.Start();
		m_commandLine = Environment.GetCommandLineArgs();
		m_asyncPump = new AsyncPump();
		SynchronizationContext.SetSynchronizationContext(m_asyncPump);
		ParseCommandLine();
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
				hydrogenConfig.LogFilePath = $"{hydrogenConfig.LogFilePath}/AtlasReactor-{hydrogenConfig.ProcessCode}.log";
			}
			m_fileLog = new FileLog();
			m_fileLog.UseDatedFolder = true;
			m_fileLog.MinLevel = Log.FromString(hydrogenConfig.MinFileLogLevel);
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
		if (!hydrogenConfig.ProcessCode.IsNullOrEmpty())
		{
			Log.Notice("Process: AtlasReactor-{0}", hydrogenConfig.ProcessCode);
		}
		if (hydrogenConfig.Language == null)
		{
			hydrogenConfig.Language = "en";
		}
		LocalizationManager.SetBootupLanguage(hydrogenConfig.Language);
		hydrogenConfig.Language = LocalizationManager.CurrentLanguageCode;
		hydrogenConfig.ServerMode = false;
		SslValidator.AcceptableSslPolicyErrors = hydrogenConfig.AcceptableSslPolicyErrors;
		Instance = this;
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
		if (GetComponent<ClientIdleTimer>() == null)
		{
			base.gameObject.AddComponent<ClientIdleTimer>();
		}
		CommerceClient.Get();
		DebugCommands.Instantiate();
		SlashCommands.Instantiate();
		TextConsole.Instantiate();
		if (!LoadTest)
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().OnConnectedToLobbyServer += HandleConnectedToLobbyServer;
			return;
		}
	}

	private void Update()
	{
		if (AppState.GetCurrent() == null)
		{
			AppState_Startup.Get().Enter();
		}
		if (LoadTest)
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
		if (m_fileLog != null)
		{
			m_fileLog.Update();
		}
		if (m_asyncPump == null)
		{
			return;
		}
		while (true)
		{
			m_asyncPump.Run(0);
			return;
		}
	}

	private void OnDestroy()
	{
		if (m_fileLog != null)
		{
			m_fileLog.Close();
		}
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().OnConnectedToLobbyServer -= HandleConnectedToLobbyServer;
			return;
		}
	}

	private void ParseCommandLine()
	{
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		string str = string.Join(" ", m_commandLine);
		Log.Info("Command line: " + str);
		string text = Application.dataPath + "/../../../Build/AtlasReactor/Config/AtlasReactorConfig.json";
		string text2 = Application.dataPath + "/../../Config/AtlasReactorConfig.json";
		string text3 = string.Empty;
		if (Application.isEditor)
		{
			text2 = text;
			text3 = "dev";
		}
		List<string> list = new List<string>();
		for (int i = 1; i < m_commandLine.Length; i++)
		{
			string a = m_commandLine[i].ToLower();
			if (!(a == "-o"))
			{
				if (!(a == "-option"))
				{
					if (!(a == "--option"))
					{
						goto IL_018d;
					}
				}
			}
			if (i + 1 < m_commandLine.Length)
			{
				string text4 = m_commandLine[i + 1];
				string[] array = text4.Split('=');
				string text5 = array[0];
				object obj;
				if (array.Length > 1)
				{
					obj = array[1];
				}
				else
				{
					obj = "True";
				}
				string arg = (string)obj;
				text5 = text5.TrimStart('-');
				string item = $"{{ \"{text5}\" : \"{arg}\" }}";
				list.Add(item);
				i++;
				continue;
			}
			goto IL_018d;
			IL_04ad:
			if (a == "-loadtest")
			{
				LoadTest = true;
			}
			continue;
			IL_01ea:
			if (!(a == "-s"))
			{
				if (!(a == "-server"))
				{
					if (!(a == "--server"))
					{
						goto IL_0281;
					}
				}
			}
			if (i + 1 < m_commandLine.Length)
			{
				string arg2 = m_commandLine[i + 1];
				string item2 = $"{{ \"DirectoryServerAddress\" : \"{arg2}\" }}";
				list.Add(item2);
				i++;
				continue;
			}
			goto IL_0281;
			IL_0281:
			if (!(a == "-p"))
			{
				if (!(a == "-processcode"))
				{
					if (!(a == "--processcode"))
					{
						goto IL_0302;
					}
				}
			}
			if (i + 1 < m_commandLine.Length)
			{
				string arg3 = m_commandLine[i + 1];
				string item3 = $"{{ \"ProcessCode\" : \"{arg3}\" }}";
				list.Add(item3);
				i++;
				continue;
			}
			goto IL_0302;
			IL_018d:
			if (!(a == "-c") && !(a == "-config"))
			{
				if (!(a == "--config"))
				{
					goto IL_01ea;
				}
			}
			if (i + 1 < m_commandLine.Length)
			{
				text2 = m_commandLine[i + 1];
				i++;
				continue;
			}
			goto IL_01ea;
			IL_0302:
			if (!(a == "-t") && !(a == "-ticket"))
			{
				if (!(a == "--ticket"))
				{
					goto IL_039a;
				}
			}
			if (i + 1 < m_commandLine.Length)
			{
				string text6 = m_commandLine[i + 1];
				text6 = text6.Replace('\\', '/');
				string item4 = $"{{ \"TicketFile\" : \"{text6}\" }}";
				list.Add(item4);
				i++;
				continue;
			}
			goto IL_039a;
			IL_039a:
			if (!(a == "-l"))
			{
				if (!(a == "-language"))
				{
					if (!(a == "--language"))
					{
						goto IL_0434;
					}
				}
			}
			if (i + 1 < m_commandLine.Length)
			{
				string text7 = m_commandLine[i + 1];
				text7 = text7.Replace('\\', '/');
				string item5 = $"{{ \"Language\" : \"{text7}\" }}";
				list.Add(item5);
				i++;
				continue;
			}
			goto IL_0434;
			IL_0434:
			if (!(a == "-e"))
			{
				if (!(a == "-environment"))
				{
					if (!(a == "--environment"))
					{
						goto IL_04ad;
					}
				}
			}
			if (i + 1 < m_commandLine.Length)
			{
				text3 = m_commandLine[i + 1];
				i++;
				continue;
			}
			goto IL_04ad;
		}
		while (true)
		{
			if (LoadTest)
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
			Log.Notice("Configured for {0} environment", text3.IsNullOrEmpty() ? "default" : text3);
			hydrogenConfig.LoadFromFile(text2, false);
			if (!text3.IsNullOrEmpty())
			{
				string newValue = $".{text3}.json";
				string fileName3 = text2.Replace(".json", newValue);
				hydrogenConfig.LoadFromFile(fileName3, false);
			}
			hydrogenConfig.LoadFromFile(fileName2, false);
			hydrogenConfig.LoadFromFile(fileName, false);
			using (List<string>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					hydrogenConfig.Load(current);
				}
			}
			hydrogenConfig.EnvironmentName = text3;
			return;
		}
	}

	public void WriteLine(string format, params object[] args)
	{
		if (m_fileLog == null)
		{
			return;
		}
		while (true)
		{
			if (m_fileLog.File != null)
			{
				while (true)
				{
					m_fileLog.File.Write(DateTime.Now.ToString(Log.TimestampFormat) + " [SYS] ");
					m_fileLog.File.WriteLine(format, args);
					m_fileLog.File.Flush();
					return;
				}
			}
			return;
		}
	}

	private void OnApplicationQuit()
	{
		WriteLine("Quit application");
	}

	private void HandleConnectedToLobbyServer(RegisterGameClientResponse response)
	{
	}

	private void HandlePlayerInfoUpdateResponse(PlayerInfoUpdateResponse response)
	{
		object[] array = new object[1];
		object obj;
		if (response.Success)
		{
			obj = "Success";
		}
		else
		{
			obj = response.ErrorMessage;
		}
		array[0] = obj;
		Log.Info("Loadtest lobby ready response: {0}", array);
		LobbyGameConfig lobbyGameConfig = new LobbyGameConfig();
		lobbyGameConfig.GameType = GameType.Coop;
		Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(lobbyGameConfig.GameType);
		if (!gameTypeSubTypes.IsNullOrEmpty())
		{
			lobbyGameConfig.InstanceSubTypeBit = gameTypeSubTypes.First().Key;
			lobbyGameConfig.SubTypes = gameTypeSubTypes.Values.ToList();
			using (Dictionary<ushort, GameSubType>.Enumerator enumerator = gameTypeSubTypes.GetEnumerator())
			{
				while (true)
				{
					if (!enumerator.MoveNext())
					{
						break;
					}
					KeyValuePair<ushort, GameSubType> current = enumerator.Current;
					if (current.Value.HasMod(GameSubType.SubTypeMods.AntiSocial))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								lobbyGameConfig.InstanceSubTypeBit = current.Key;
								goto end_IL_009f;
							}
						}
					}
				}
				end_IL_009f:;
			}
		}
		BotDifficulty selectedBotSkillTeamA = BotDifficulty.Easy;
		BotDifficulty selectedBotSkillTeamB = BotDifficulty.Easy;
		ClientGameManager.Get().CreateGame(lobbyGameConfig, ReadyState.Ready, selectedBotSkillTeamA, selectedBotSkillTeamB, delegate(CreateGameResponse r)
		{
			if (!r.Success)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						Log.Warning("Failed to create game: {0}", r.ErrorMessage);
						return;
					}
				}
			}
		});
	}

	internal string GetFileLogCurrentPath()
	{
		string result;
		if (m_fileLog == null)
		{
			result = string.Empty;
		}
		else
		{
			result = m_fileLog.CurrentFilePath;
		}
		return result;
	}
}
