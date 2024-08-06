using LobbyGameClientMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using UnityEngine;

[Serializable]
public class HydrogenConfig : JsonConfig
{
	public string DirectoryServerAddress;

	public string MonitorServerAddress;

	public int PreferredLobbyServerIndex;

	public ProcessType ProcessType;

	public string ProcessCode;

	public string EnvironmentName;

	public string Language;

	public bool WebSocketIsCompressed;

	public bool WebSocketIsBinary;

	public string TicketFile;

	public AuthTicket Ticket;

	public string PlatformUserName;

	public string PlatformPassword;

	public bool DevMode;

	public bool ServerMode;

	public bool HeadlessMode;

	public string ReplaysPath;

	public string MatchLogsPath;

	public string DeckFile;

	public string LogFilePath;

	public string DebugLogFilePath;

	public string GameStatsSpreadsheetLogFilePath;

	public string MinFileLogLevel;

	public string MinConsoleLogLevel;

	public bool EnableLogging;

	public bool EnableHitchDetection;

	public bool EnableRandomFrameHitchDetection;

	public bool EnableNoInputIdleDisconnect;

	public float NoInputIdleWarningTime;

	public float NoInputIdleDisconnectTime;

	public float NoInputIdleWarningTimeMatchStart;

	public float NoInputIdleDisconnectTimeMatchStart;

	public PlatformConfig PlatformConfig;

	public int TargetFrameRate;

	public bool WaitForDebugger;

	public TimeSpan HeartbeatPeriod;

	public TimeSpan HeartbeatTimeout;

	public int MaxSendBufferSize;

	public TimeSpan MaxWaitTime;

	public bool UseTempSceneVisuals;

	public bool UseNewUI;

	public bool UseIdleTimer;

	public bool AllowDebugCommands;

	public bool AutoLaunchTutorial;

	public GameType AutoLaunchGameType;

	public AutoLaunchGameConfig AutoLaunchCustomGameConfig;

	public bool SkipAudioEvents;

	public bool UseTempVisualsInEditor;

	public bool UseFastBotAI;

	public float MaxAIIterationTime;

	public bool SkipCharacterModelSpawnOnServer;

	public bool HueEnabled;

	public string HueAddress;

	public string HuePutString;

	public SslPolicyErrors AcceptableSslPolicyErrors;

	private static HydrogenConfig s_instance = new HydrogenConfig();

	public string ProcessName => $"{ProcessType.ToString()}-{ProcessCode}";
	
#if SERVER
	// TODO LOW use CommonServerConfig? It is not loaded now
	// custom
	public string PublicAddress = "127.0.0.1";
	public int PublicPort = 6061;
	public int MonitorServerPort = 6060;
	public string ServerName = "Atlas";

	public int PendingReconnectTurnTime = 60;
	public int PendingReconnectMaxTurnsConsecutive = 3;
	public int PendingReconnectMaxTurnsTotal = 4;
	
	public string ReplayUploadUrl;
	public Dictionary<string, string> ReplayUploadHeaders;
#endif
	
#if !VANILLA
	public string CrashReportUrl = "https://evos-emu.com/api/archive/";
#endif

#if !VANILLA && !SERVER
    //Custom titles
    public string ApiTitleUrl = "https://stats-production.evos.live/api/titles";
#endif
	public HydrogenConfig()
	{
		LogFilePath = "${CONFIGPATH}/../Logs/";
		DebugLogFilePath = "${CONFIGPATH}/../DebugLogs/";
		GameStatsSpreadsheetLogFilePath = "${CONFIGPATH}/../StatsLogs/";
		MinFileLogLevel = "Info";
		MinConsoleLogLevel = "Info";
		MatchLogsPath = "${CONFIGPATH}/../MatchLogs";
		ReplaysPath = "${CONFIGPATH}/../Replays";
		DeckFile = "${CONFIGPATH}/../Game.ini";
		TargetFrameRate = -1;
		WebSocketIsCompressed = false;
		WebSocketIsBinary = true;
		UseNewUI = true;
		UseIdleTimer = true;
		SkipAudioEvents = false;
		UseFastBotAI = true;
		SkipCharacterModelSpawnOnServer = true;
		HueEnabled = false;
		HueAddress = "http://127.0.0.1";
		HuePutString = "/api/newdeveloper/lights/1/state";
		MaxAIIterationTime = 0f;
		PlatformConfig = new PlatformConfig();
		HeartbeatPeriod = TimeSpan.FromSeconds(5.0);
		HeartbeatTimeout = TimeSpan.FromSeconds(30.0);
		MaxSendBufferSize = 1048576;
		MaxWaitTime = TimeSpan.FromSeconds(10.0);
		AutoLaunchTutorial = true;
		AutoLaunchGameType = GameType.None;
		AutoLaunchCustomGameConfig = new AutoLaunchGameConfig();
		AcceptableSslPolicyErrors = (SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors);
		NoInputIdleWarningTime = 120f;
		NoInputIdleDisconnectTime = 140f;
		NoInputIdleWarningTimeMatchStart = 30f;
		NoInputIdleDisconnectTimeMatchStart = 40f;
		EnableNoInputIdleDisconnect = true;
		EnableRandomFrameHitchDetection = false;
		EnableHitchDetection = false;
	}

	public static HydrogenConfig Get()
	{
		return s_instance;
	}

	public override void PostProcess()
	{
		DirectoryServerAddress = DirectoryServerAddress.SafeReplace("${HOSTNAME}", base.HostName);
		MonitorServerAddress = MonitorServerAddress.SafeReplace("${HOSTNAME}", base.HostName);
		LogFilePath = LogFilePath.SafeReplace("${DATAPATH}", Application.dataPath);
		DebugLogFilePath = DebugLogFilePath.SafeReplace("${DATAPATH}", Application.dataPath);
		GameStatsSpreadsheetLogFilePath = GameStatsSpreadsheetLogFilePath.SafeReplace("${DATAPATH}", Application.dataPath);
		LogFilePath = LogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		DebugLogFilePath = DebugLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		GameStatsSpreadsheetLogFilePath = GameStatsSpreadsheetLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		LogFilePath = Path.GetFullPath(LogFilePath);
		DebugLogFilePath = Path.GetFullPath(DebugLogFilePath);
		GameStatsSpreadsheetLogFilePath = Path.GetFullPath(GameStatsSpreadsheetLogFilePath);
		ReplaysPath = ReplaysPath.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		MatchLogsPath = MatchLogsPath.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		DeckFile = DeckFile.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		if (MaxAIIterationTime <= 0f)
		{
			if (UseFastBotAI)
			{
				MaxAIIterationTime = 0.5f;
			}
			else
			{
				MaxAIIterationTime = 0.02f;
			}
		}
	}

	public static bool MatchesPath(string path)
	{
		string fileName = Path.GetFileName(path);
		int result;
		if (fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
		{
			if (!fileName.StartsWith("AtlasReactorConfig", StringComparison.OrdinalIgnoreCase))
			{
				if (!fileName.StartsWith("AtlasReactorDevConfig", StringComparison.OrdinalIgnoreCase))
				{
					result = (fileName.StartsWith("AtlasReactorServerConfig", StringComparison.OrdinalIgnoreCase) ? 1 : 0);
					goto IL_006a;
				}
			}
			result = 1;
		}
		else
		{
			result = 0;
		}
		goto IL_006a;
		IL_006a:
		return (byte)result != 0;
	}

	public bool IsInGroupB()
	{
		return Ticket != null && Ticket.AccountId % 2 == 1;
	}

	public bool CanAutoLaunchGame()
	{
		if (AutoLaunchGameType != GameType.Practice)
		{
			if (AutoLaunchGameType != GameType.Tutorial)
			{
				if (AutoLaunchGameType == GameType.Custom)
				{
					while (true)
					{
						int result;
						switch (7)
						{
						case 0:
							break;
						default:
							{
								if (AutoLaunchCustomGameConfig != null)
								{
									if (AutoLaunchCustomGameConfig.GameConfig != null)
									{
										if (AutoLaunchCustomGameConfig.GameConfig.GameType.IsAutoLaunchable())
										{
											result = ((!AutoLaunchCustomGameConfig.GameConfig.Map.IsNullOrEmpty()) ? 1 : 0);
											goto IL_00aa;
										}
									}
								}
								result = 0;
								goto IL_00aa;
							}
							IL_00aa:
							return (byte)result != 0;
						}
					}
				}
				return false;
			}
		}
		return true;
	}

	public ushort GetSavedSubTypes(GameType gameType, Dictionary<ushort, GameSubType> subTypes)
	{
		ushort num = 0;
		string fullPath = Path.GetFullPath(Application.dataPath + "/../LocalSettings/SelectedGameSubTypes.json");
		if (File.Exists(fullPath))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					try
					{
						string value = File.ReadAllText(fullPath);
						Dictionary<GameType, List<string>> dictionary = new Dictionary<GameType, List<string>>();
						JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
						jsonSerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
						JsonSerializerSettings settings = jsonSerializerSettings;
						JsonConvert.PopulateObject(value, dictionary, settings);
						if (dictionary.ContainsKey(gameType))
						{
							List<string> list = dictionary[gameType];
							using (Dictionary<ushort, GameSubType>.Enumerator enumerator = subTypes.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									KeyValuePair<ushort, GameSubType> current = enumerator.Current;
									if (list.Contains(current.Value.LocalizedName))
									{
										num = (ushort)(num | current.Key);
									}
								}
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										return num;
									}
								}
							}
						}
						return num;
					}
					catch (Exception exception)
					{
						Log.Exception(exception);
						num = 0;
						SaveGameTypeSubMaskPreference(gameType, num, ClientGameManager.Get().GameTypeAvailabilies);
						return num;
					}
				}
			}
		}
		return num;
	}

	public void SaveGameTypeSubMaskPreference(GameType gameType, ushort mask, Dictionary<GameType, GameTypeAvailability> GameTypeAvailabilies)
	{
		List<string> list = new List<string>();
		if (GameTypeAvailabilies.TryGetValue(gameType, out GameTypeAvailability value))
		{
			if (!value.SubTypes.IsNullOrEmpty())
			{
				ushort num = 1;
				foreach (GameSubType subType in value.SubTypes)
				{
					if ((num & mask) != 0)
					{
						list.Add(subType.LocalizedName);
					}
					num = (ushort)(num << 1);
				}
			}
		}
		try
		{
			string fullPath = Path.GetFullPath(Application.dataPath + "/../LocalSettings/SelectedGameSubTypes.json");
			Dictionary<GameType, List<string>> dictionary = new Dictionary<GameType, List<string>>();
			FileInfo fileInfo = new FileInfo(fullPath);
			if (fileInfo.Exists)
			{
				string text = "{}";
				text = File.ReadAllText(fullPath);
				JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
				jsonSerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;
				JsonSerializerSettings settings = jsonSerializerSettings;
				JsonConvert.PopulateObject(text, dictionary, settings);
			}
			else
			{
				Directory.CreateDirectory(Application.dataPath + "/../LocalSettings/");
			}
			dictionary[gameType] = list;
			string contents = JsonConvert.SerializeObject(dictionary, Formatting.Indented, new StringEnumConverter());
			File.WriteAllText(fullPath, contents);
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}
}
