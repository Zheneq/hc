using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using LobbyGameClientMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

	public HydrogenConfig()
	{
		this.LogFilePath = "${CONFIGPATH}/../Logs/";
		this.DebugLogFilePath = "${CONFIGPATH}/../DebugLogs/";
		this.GameStatsSpreadsheetLogFilePath = "${CONFIGPATH}/../StatsLogs/";
		this.MinFileLogLevel = "Info";
		this.MinConsoleLogLevel = "Info";
		this.MatchLogsPath = "${CONFIGPATH}/../MatchLogs";
		this.ReplaysPath = "${CONFIGPATH}/../Replays";
		this.DeckFile = "${CONFIGPATH}/../Game.ini";
		this.TargetFrameRate = -1;
		this.WebSocketIsCompressed = false;
		this.WebSocketIsBinary = true;
		this.UseNewUI = true;
		this.UseIdleTimer = true;
		this.SkipAudioEvents = false;
		this.UseFastBotAI = true;
		this.SkipCharacterModelSpawnOnServer = true;
		this.HueEnabled = false;
		this.HueAddress = "http://127.0.0.1";
		this.HuePutString = "/api/newdeveloper/lights/1/state";
		this.MaxAIIterationTime = 0f;
		this.PlatformConfig = new PlatformConfig();
		this.HeartbeatPeriod = TimeSpan.FromSeconds(5.0);
		this.HeartbeatTimeout = TimeSpan.FromSeconds(30.0);
		this.MaxSendBufferSize = 0x100000;
		this.MaxWaitTime = TimeSpan.FromSeconds(10.0);
		this.AutoLaunchTutorial = true;
		this.AutoLaunchGameType = GameType.None;
		this.AutoLaunchCustomGameConfig = new AutoLaunchGameConfig();
		this.AcceptableSslPolicyErrors = (SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateChainErrors);
		this.NoInputIdleWarningTime = 120f;
		this.NoInputIdleDisconnectTime = 140f;
		this.NoInputIdleWarningTimeMatchStart = 30f;
		this.NoInputIdleDisconnectTimeMatchStart = 40f;
		this.EnableNoInputIdleDisconnect = true;
		this.EnableRandomFrameHitchDetection = false;
		this.EnableHitchDetection = false;
	}

	public string ProcessName
	{
		get
		{
			return string.Format("{0}-{1}", this.ProcessType.ToString(), this.ProcessCode);
		}
	}

	public static HydrogenConfig Get()
	{
		return HydrogenConfig.s_instance;
	}

	public override void PostProcess()
	{
		this.DirectoryServerAddress = this.DirectoryServerAddress.SafeReplace("${HOSTNAME}", base.HostName);
		this.MonitorServerAddress = this.MonitorServerAddress.SafeReplace("${HOSTNAME}", base.HostName);
		this.LogFilePath = this.LogFilePath.SafeReplace("${DATAPATH}", Application.dataPath);
		this.DebugLogFilePath = this.DebugLogFilePath.SafeReplace("${DATAPATH}", Application.dataPath);
		this.GameStatsSpreadsheetLogFilePath = this.GameStatsSpreadsheetLogFilePath.SafeReplace("${DATAPATH}", Application.dataPath);
		this.LogFilePath = this.LogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		this.DebugLogFilePath = this.DebugLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		this.GameStatsSpreadsheetLogFilePath = this.GameStatsSpreadsheetLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		this.LogFilePath = Path.GetFullPath(this.LogFilePath);
		this.DebugLogFilePath = Path.GetFullPath(this.DebugLogFilePath);
		this.GameStatsSpreadsheetLogFilePath = Path.GetFullPath(this.GameStatsSpreadsheetLogFilePath);
		this.ReplaysPath = this.ReplaysPath.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		this.MatchLogsPath = this.MatchLogsPath.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		this.DeckFile = this.DeckFile.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		if (this.MaxAIIterationTime <= 0f)
		{
			if (this.UseFastBotAI)
			{
				this.MaxAIIterationTime = 0.5f;
			}
			else
			{
				this.MaxAIIterationTime = 0.02f;
			}
		}
	}

	public static bool MatchesPath(string path)
	{
		string fileName = Path.GetFileName(path);
		bool result;
		if (fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HydrogenConfig.MatchesPath(string)).MethodHandle;
			}
			if (!fileName.StartsWith("AtlasReactorConfig", StringComparison.OrdinalIgnoreCase))
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
				if (!fileName.StartsWith("AtlasReactorDevConfig", StringComparison.OrdinalIgnoreCase))
				{
					result = fileName.StartsWith("AtlasReactorServerConfig", StringComparison.OrdinalIgnoreCase);
					goto IL_67;
				}
			}
			result = true;
			IL_67:;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool IsInGroupB()
	{
		return this.Ticket != null && this.Ticket.AccountId % 2L == 1L;
	}

	public bool CanAutoLaunchGame()
	{
		if (this.AutoLaunchGameType != GameType.Practice)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HydrogenConfig.CanAutoLaunchGame()).MethodHandle;
			}
			if (this.AutoLaunchGameType == GameType.Tutorial)
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
			}
			else
			{
				if (this.AutoLaunchGameType == GameType.Custom)
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
					if (this.AutoLaunchCustomGameConfig != null)
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
						if (this.AutoLaunchCustomGameConfig.GameConfig != null)
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
							if (this.AutoLaunchCustomGameConfig.GameConfig.GameType.IsAutoLaunchable())
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
								return !this.AutoLaunchCustomGameConfig.GameConfig.Map.IsNullOrEmpty();
							}
						}
					}
					return false;
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HydrogenConfig.GetSavedSubTypes(GameType, Dictionary<ushort, GameSubType>)).MethodHandle;
			}
			try
			{
				string value = File.ReadAllText(fullPath);
				Dictionary<GameType, List<string>> dictionary = new Dictionary<GameType, List<string>>();
				JsonSerializerSettings settings = new JsonSerializerSettings
				{
					ObjectCreationHandling = ObjectCreationHandling.Replace
				};
				JsonConvert.PopulateObject(value, dictionary, settings);
				if (dictionary.ContainsKey(gameType))
				{
					List<string> list = dictionary[gameType];
					using (Dictionary<ushort, GameSubType>.Enumerator enumerator = subTypes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<ushort, GameSubType> keyValuePair = enumerator.Current;
							if (list.Contains(keyValuePair.Value.LocalizedName))
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
								num |= keyValuePair.Key;
							}
						}
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
				num = 0;
				this.SaveGameTypeSubMaskPreference(gameType, num, ClientGameManager.Get().GameTypeAvailabilies);
			}
		}
		return num;
	}

	public void SaveGameTypeSubMaskPreference(GameType gameType, ushort mask, Dictionary<GameType, GameTypeAvailability> GameTypeAvailabilies)
	{
		List<string> list = new List<string>();
		GameTypeAvailability gameTypeAvailability;
		if (GameTypeAvailabilies.TryGetValue(gameType, out gameTypeAvailability))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HydrogenConfig.SaveGameTypeSubMaskPreference(GameType, ushort, Dictionary<GameType, GameTypeAvailability>)).MethodHandle;
			}
			if (!gameTypeAvailability.SubTypes.IsNullOrEmpty<GameSubType>())
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
				ushort num = 1;
				foreach (GameSubType gameSubType in gameTypeAvailability.SubTypes)
				{
					if ((num & mask) != 0)
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
						list.Add(gameSubType.LocalizedName);
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
				string value = File.ReadAllText(fullPath);
				JsonSerializerSettings settings = new JsonSerializerSettings
				{
					ObjectCreationHandling = ObjectCreationHandling.Replace
				};
				JsonConvert.PopulateObject(value, dictionary, settings);
			}
			else
			{
				Directory.CreateDirectory(Application.dataPath + "/../LocalSettings/");
			}
			dictionary[gameType] = list;
			string contents = JsonConvert.SerializeObject(dictionary, Formatting.Indented, new JsonConverter[]
			{
				new StringEnumConverter()
			});
			File.WriteAllText(fullPath, contents);
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}
}
