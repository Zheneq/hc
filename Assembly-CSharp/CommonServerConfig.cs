// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

// server-only
// TODO review default config
#if SERVER
[Serializable]
public class CommonServerConfig : JsonConfig
{
	public bool PrivateListenerEnabled;
	public string PrivateHostName;
	public string PrivateDomain;
	public int PrivatePort;
	public string PrivateSslPath;
	public bool PrivateSslEnabled;
	public TimeSpan PrivateHeartbeatPeriod;
	public TimeSpan PrivateHeartbeatTimeout;
	public int PrivateMaxSendBufferSize;
	public int PrivateMaxMessageSize;
	public int PrivateMaxMessagesPerSecond;
	public TimeSpan PrivateMaxWaitTime;

	public bool PublicListenerEnabled;
	public string PublicHostName;
	public string PublicDomain;
	public int PublicPort;
	public string PublicSslPath;
	public bool PublicSslEnabled;
	public TimeSpan PublicHeartbeatPeriod;
	public TimeSpan PublicHeartbeatTimeout;
	public int PublicMaxSendBufferSize;
	public int PublicMaxMessageSize;
	public int PublicMaxMessagesPerSecond;
	public TimeSpan PublicMaxWaitTime;

	public int ServerInterconnectPort;
	public TimeSpan ServerInterconnectClusterConnectTimeout;
	public TimeSpan ServerInterconnectClusterStartupTimeout;
	public TimeSpan ServerInterconnectRequestTimeout;
	public TimeSpan MetricsBroadcastInterval;
	public TimeSpan ProxyBroadcastInterval;
	public TimeSpan WatchdogTimeout;
	public int PortOffset;

	public string[] CoreServers;
	public string[] DirectoryServers;
	public string[] LobbyServers;
	public string[] MatchmakingServers;
	public string[] RelayServers;

	public string BasePath;
	public string GameplayDataPath;
	public string DatabaseScriptsPath;
	public string PlayerDataPath;
	public string ServerDataPath;
	public string HtmlPath;
	public string LoadTestAccountsPath;

	public string MonitorServerPath;
	public string MonitorServerLogFilePath;
	public string MonitorServerDebugLogFilePath;
	public string MonitorServerMetricsLogFilePath;

	public string LobbyServerPath;
	public string LobbyServerLogFilePath;
	public string LobbyServerDebugLogFilePath;
	public string LobbyServerMetricsLogFilePath;

	public string DirectoryServerPath;
	public string DirectoryServerLogFilePath;
	public string DirectoryServerDebugLogFilePath;
	public string DirectoryServerMetricsLogFilePath;

	public string MatchmakingServerPath;
	public string MatchmakingServerLogFilePath;
	public string MatchmakingServerDebugLogFilePath;
	public string MatchmakingServerMetricsLogFilePath;

	public string RelayServerPath;
	public string RelayServerLogFilePath;
	public string RelayServerDebugLogFilePath;
	public string RelayServerMetricsLogFilePath;

	public string GameServerPath;
	public string GameServerLogFilePath;
	public string GameServerDebugLogFilePath;
	public string GameServerOutputLogFilePath;

	public string LoadTestServerPath;
	public string LoadTestServerLogFilePath;
	public string LoadTestServerDebugLogFilePath;
	public string LoadTestServerMetricsLogFilePath;
	public string LoadTestServerStatusLogFilePath;

	public string MinLogLevel;
	public bool ConsoleIntegration;
	public bool AutomationIntegration;
	public bool ReadStdInput;

	[SensitiveData]
	public string AutomationPassword;
	public string Command;
	public string EnvironmentName;
	public EnvironmentType EnvironmentType;
	public Region Region;
	public ProcessType ProcessType;
	public string ProcessCode;
	public int TimeCode;

	public string EventLogFilePath;
	public string EventLogServerAddress;
	public int EventLogServerPort;
	public string EventLogRedisAddress;
	public int EventLogRedisPort;
	public int EventLogRedisDb;

	public string AccountLogServerAddress;
	public bool DatabaseEnabled;
	public bool DatabaseMirroringEnabled;
	public string DatabasePrimaryConnectionString;
	public string DatabaseSecondaryConnectionString;
	public string[] DatabasePrimaryConnectionStrings = new string[10];
	public string[] DatabaseSecondaryConnectionStrings = new string[10];
	public string DatabaseName;
	public string DatabaseUserName;
	[SensitiveData]
	public string DatabasePassword;
	public string DatabaseUpdateUserName;
	[SensitiveData]
	public string DatabaseUpdatePassword;
	public TimeSpan DatabaseUpdateInterval;
	public TimeSpan DatabaseFlushInterval;
	public TimeSpan DatabaseRetryTimeout;
	public int DatabaseMaxFlushSize;
	public bool DatabaseExecuteSynchronously;
	public TimeSpan DatabaseExecutionTimeout;
	public TimeSpan DatabaseArtificialExecutionLatency;
	public bool DatabaseCacheEnabled;
	public bool DatabaseSnapshotEnabled;

	public PlatformConfig PlatformConfig;
	public List<LobbyGameConfig> GameConfigs;
	public List<Team> FreelancerSelectOrder;

	public string RssFtpUrl;
	public string RssHttpUrl;
	public string RssLanguages;
	public string FreeUpsellExternalBrowserUrl;
	public string FreeUpsellExternalBrowserSteamUrl;
	public int MinWorkerThreads;
	public int MaxWorkerThreads;
	public int MinIOThreads;
	public int MaxIOThreads;
	public TimeSpan ArtificialLatency;
	public int ServicePointManagerDefaultConnectionLimit;
	public TimeSpan GameServerStartupTimeout;
	public TimeSpan GameServerClientConnectTimeout;
	public TimeSpan GameServerClientReconnectTimeout;
	public TimeSpan GameServerClientLoadTimeout;
	public TimeSpan GameServerClientLatencyWarningThreshold;
	public int GameServerMaxInstances;
	public int GameServerMaxLaunchingInstances;
	public int GameServerMaxPreLaunchingInstances;
	public string GameServerListenAddress;
	public int GameServerListenStartPort;
	public int GameServerListenEndPort;
	public int GameServerListenRecommendedPort;
	public int[] GameServerListenDisallowedPorts;
	public int GameServerMaxConnections;
	public ushort GameServerMaxPacketSize;
	public ushort GameServerSentMessagePoolSize;
	public ushort GameServerReceivedMessagePoolSize;
	public bool GameServerUseWebSockets;

	public bool AllowReconnectingToGameInstantly;

	public TimeSpan FakeGameDuration;

	public int LoadTestMaxClients;

	public int LobbyServerMinInstances;

	public int LobbyServerMaxInstances;

	public string CoOpSessionHostListenAddress;

	public int CoOpSessionHostListenPort;

	public int ClientSessionsSoftMax;

	public int ClientSessionsHardMax;

	public TimeSpan ClientSessionsHistoricalPeriod;

	public TimeSpan ClientSessionReconnectTimeout;

	public TimeSpan ClientErrorReportRate;

	public bool WriteClientErrorLogs;

	public TimeSpan NewGameAcceptTimeout;

	public bool NewGameAcceptEnabled;

	public bool EnableHiddenCharacters;

	public bool EnableServerConnectionQueueing;

	public bool EnableGlobalChat;

	public bool EnableCrossServerGlobalChat;

	public bool EnableFacebook;

	public bool EnableConversations;

	public bool EnableBlacklisting;

	public bool EnableClientPerformanceCollecting;

	public bool EnableSteamAchievements;

	public bool AllowDevServers;

	public bool AutoLockServer;

	public int SpectatorOutsideCustomTurnDelay;

	public bool AllowPreferredLobbyServer;

	public bool SoloGameNoAutoLockinOnTimeout;

	public bool DisableControlPadInput;

	public bool ShowDevServerErrorsToClients;

	public bool ShowDevServerInfoToClients;

	public bool ShowNetworkErrors;

	public bool SkipLoadoutSelection;

	public TimeSpan SelectionTimeout;

	public TimeSpan LoadoutSelectionTimeout;

	public uint MaximumGroupSize;

	public TimeSpan InviteExpirationTimeLimit;

	public TimeSpan ClientPerformanceCollectingFrequency;

	public Dictionary<string, string> SpamFilterConfig;

	public bool MedianCalculationsEnabled;

	public string PidFilePath;

	public bool StartServers;

	public string MonoPath;

	public string MonoOptions;

	public string MonoProfilingOptions;

	public Dictionary<string, string> MonoEnvironmentVariables;

	public string MonoProfilingIp;

	public int MonoProfilingPort;

	public TimeSpan StartupDelay;

	public TimeSpan StartupTimeout;

	public TimeSpan ShutdownTimeout;

	public bool OutageEmailEnabled;

	public string OutageEmailTemplate;

	public string OutageEmailSmtpServer;

	public string OutageEmailSender;

	public List<string> OutageEmailRecipients;

	public bool PlayerReportAutoMuteEnabled;

	public int MaxSquelchReports;

	public int SquelchPenaltyDuration;

	public float ReportDecayValue;

	public string PublicEnvironmentUrl;

	public string PrivateEnvironmentUrl;

	public int PublicHttpListenPort;

	public int PublicHttpsListenPort;

	public int PrivateHttpListenPort;

	public int PrivateHttpsListenPort;

	public Dictionary<string, string> DnsOverrides;

	public int LogFileUncompressedRetentionDays;

	public int LogFileCompressedRetentionDays;

	public bool GameServerLogFileAutoCompressionEnabled;

	public TimeSpan GameServerLogFileAutoCompressionDelay;

	[SensitiveData]
	public string DiscordOAuth2ClientSecret;

	[SensitiveData]
	public string DiscordOAuth2Token;

	public string DiscordOAuth2ClientId;

	public string DiscordOAuth2RedirectUri;

	public string DiscordOAuth2RpcOrigin;

	public string DiscordServerTeamInstanceName;

	public string DiscordServerGroupInstanceName;

	[SensitiveData]
	public string FacebookOAuth2ClientSecret;

	public string FacebookOAuth2ClientId;

	public string FacebookOAuth2RedirectUri;

	public string FacebookOAuth2Scope;

	public Rate PaidPlayerWhisperChatRateLimit;

	public Rate PaidPlayerTeamChatRateLimit;

	public Rate PaidPlayerGlobalChatRateLimit;

	public Rate FreePlayerWhisperChatRateLimit;

	public Rate FreePlayerTeamChatRateLimit;

	public Rate FreePlayerGlobalChatRateLimit;

	public float FreelancerExperienceBonus;

	public int DefaultDirectoryServerInterconnectPort = 9050;

	public int DefaultLobbyServerInterconnectPort = 9060;

	public int DefaultMonitorServerInterconnectPort = 9020;

	public int DefaultMatchmakingServerInterconnectPort = 9010;

	public int DefaultRelayServerInterconnectPort = 9000;

	public int DefaultLoadTestServerInterconnectPort = 9030;

	public TimeSpan MaxWaitOnLoginRemoteSubsytemLoads;

	public bool AllowBadges;

	private static CommonServerConfig s_instance;

	public static event Action OnChanged = delegate () {};

	public string PrivateProtocol
	{
		get
		{
			if (!PrivateSslEnabled)
			{
				return "ws";
			}
			return "wss";
		}
	}

	public string PrivateAddress
	{
		get
		{
			return string.Format("{0}://{1}:{2}", PrivateProtocol, PrivateHostName, PrivatePort);
		}
	}

	public string PrivateSslPfxPath
	{
		get
		{
			return StringUtil.PathChangeExtension(PrivateSslPath, ".pfx");
		}
	}

	public string PrivateSslCrtPath
	{
		get
		{
			return StringUtil.PathChangeExtension(PrivateSslPath, ".crt");
		}
	}

	public string PrivateSslKeyPath
	{
		get
		{
			return StringUtil.PathChangeExtension(PrivateSslPath, ".key");
		}
	}

	public string PrivateSslChainCrtPath
	{
		get
		{
			return StringUtil.PathChangeExtension(PrivateSslPath, ".chain.crt");
		}
	}

	public string PublicProtocol
	{
		get
		{
			if (!PublicSslEnabled)
			{
				return "ws";
			}
			return "wss";
		}
	}

	public string PublicAddress
	{
		get
		{
			return string.Format("{0}://{1}:{2}", PublicProtocol, PublicHostName, PublicPort);
		}
	}

	public string PublicSslPfxPath
	{
		get
		{
			return StringUtil.PathChangeExtension(PublicSslPath, ".pfx");
		}
	}

	public string PublicSslCrtPath
	{
		get
		{
			return StringUtil.PathChangeExtension(PublicSslPath, ".crt");
		}
	}

	public string PublicSslKeyPath
	{
		get
		{
			return StringUtil.PathChangeExtension(PublicSslPath, ".key");
		}
	}

	public string PublicSslChainCrtPath
	{
		get
		{
			return StringUtil.PathChangeExtension(PublicSslPath, ".chain.crt");
		}
	}

	public string ProcessName
	{
		get
		{
			return string.Format("{0}-{1}", ProcessType.ToString(), ProcessCode);
		}
	}

	public static CommonServerConfig Get()
	{
		return s_instance;
	}

	public static void Set(CommonServerConfig config)
	{
		s_instance = config;
		OnChanged();
	}

	public CommonServerConfig()
	{
		CoreServers = new string[0];
		DirectoryServers = new string[0];
		LobbyServers = new string[0];
		MatchmakingServers = new string[0];
		RelayServers = new string[0];
		PrivateSslEnabled = false;
		PrivateHeartbeatPeriod = TimeSpan.FromSeconds(10.0);
		PrivateHeartbeatTimeout = TimeSpan.FromSeconds(30.0);
		PrivateMaxMessageSize = 10485760;
		PrivateMaxSendBufferSize = 10485760;
		PrivateMaxWaitTime = TimeSpan.FromSeconds(10.0);
		PublicSslEnabled = true;
		PublicHeartbeatPeriod = TimeSpan.FromSeconds(10.0);
		PublicHeartbeatTimeout = TimeSpan.FromSeconds(30.0);
		PublicMaxMessageSize = 1048576;
		PublicMaxMessagesPerSecond = 10;
		PublicMaxSendBufferSize = 1048576;
		PublicMaxWaitTime = TimeSpan.FromSeconds(10.0);
		ServerInterconnectClusterConnectTimeout = TimeSpan.FromMinutes(1.0);
		ServerInterconnectClusterStartupTimeout = TimeSpan.FromMinutes(2.0);
		ServerInterconnectRequestTimeout = TimeSpan.FromSeconds(5.0);
		Region = Region.US;
		MinLogLevel = "Info";
		PlatformConfig = new PlatformConfig();
		MinWorkerThreads = 0;
		MaxWorkerThreads = 0;
		MinIOThreads = 0;
		MaxIOThreads = 0;
		DatabaseMirroringEnabled = true;
		DatabaseUpdateInterval = TimeSpan.FromSeconds(1.0);
		DatabaseFlushInterval = TimeSpan.FromSeconds(60.0);
		DatabaseRetryTimeout = TimeSpan.FromMinutes(60.0);
		DatabaseMaxFlushSize = 100;
		DatabaseExecuteSynchronously = false;
		DatabaseExecutionTimeout = TimeSpan.FromSeconds(30.0);
		DatabaseArtificialExecutionLatency = TimeSpan.Zero;
		DatabaseCacheEnabled = true;
		DatabaseSnapshotEnabled = true;
		GameConfigs = new List<LobbyGameConfig>();
		RssLanguages = "en";
		WatchdogTimeout = TimeSpan.FromMinutes(1.0);
		MetricsBroadcastInterval = TimeSpan.FromSeconds(5.0);
		ServicePointManagerDefaultConnectionLimit = 16;
		AllowDevServers = false;
		ShowDevServerErrorsToClients = false;
		ShowDevServerInfoToClients = false;
		ShowNetworkErrors = true;
		GameServerStartupTimeout = TimeSpan.FromSeconds(60.0);
		GameServerClientConnectTimeout = TimeSpan.FromSeconds(60.0);
		GameServerClientReconnectTimeout = TimeSpan.FromSeconds(30.0);
		GameServerClientLoadTimeout = TimeSpan.FromSeconds(120.0);
		GameServerClientLatencyWarningThreshold = TimeSpan.FromSeconds(2.0);
		GameServerMaxInstances = 100;
		GameServerMaxLaunchingInstances = 5;
		GameServerMaxPreLaunchingInstances = 0;
		GameServerListenStartPort = 6100;
		GameServerListenEndPort = 6999;
		GameServerListenDisallowedPorts = new int[]
		{
			6667
		};
		GameServerUseWebSockets = true;
		GameServerMaxConnections = 32;
		GameServerMaxPacketSize = 32768;
		GameServerSentMessagePoolSize = 1024;
		GameServerReceivedMessagePoolSize = 1024;
		NewGameAcceptTimeout = TimeSpan.FromSeconds(30.0);
		NewGameAcceptEnabled = true;
		EnableHiddenCharacters = false;
		EnableGlobalChat = true;
		EnableCrossServerGlobalChat = true;
		EnableBlacklisting = true;
		EnableClientPerformanceCollecting = false;
		EnableSteamAchievements = false;
		SpamFilterConfig = new Dictionary<string, string>();
		MedianCalculationsEnabled = true;
		SelectionTimeout = TimeSpan.FromSeconds(60.0);
		LoadoutSelectionTimeout = TimeSpan.FromSeconds(20.0);
		SkipLoadoutSelection = false;
		MaximumGroupSize = 4U;
		InviteExpirationTimeLimit = TimeSpan.FromMinutes(5.0);
		ClientPerformanceCollectingFrequency = TimeSpan.FromMinutes(5.0);
		ClientSessionsSoftMax = 2000;
		ClientSessionsHardMax = 4000;
		ClientSessionsHistoricalPeriod = TimeSpan.FromMinutes(1.0);
		ClientSessionReconnectTimeout = TimeSpan.FromMinutes(5.0);
		ClientErrorReportRate = TimeSpan.FromMinutes(3.0);
		WriteClientErrorLogs = false;
		FakeGameDuration = TimeSpan.FromMinutes(5.0);
		AllowReconnectingToGameInstantly = false;
		SpectatorOutsideCustomTurnDelay = 2;
		DisableControlPadInput = false;
		SoloGameNoAutoLockinOnTimeout = true;
		AllowBadges = false;
		OutageEmailEnabled = true;
		OutageEmailRecipients = new List<string>();
		StartServers = true;
		MonoEnvironmentVariables = new Dictionary<string, string>();
		MonoProfilingPort = 7100;
		StartupTimeout = TimeSpan.FromMinutes(3.0);
		ShutdownTimeout = TimeSpan.FromMinutes(1.0);
		PlayerReportAutoMuteEnabled = true;
		MaxSquelchReports = 25;
		SquelchPenaltyDuration = 120;
		ReportDecayValue = 0.15f;
		PublicEnvironmentUrl = "${HOSTNAME}";
		PrivateEnvironmentUrl = "${HOSTNAME}";
		PublicHttpListenPort = 6080;
		PublicHttpsListenPort = 6043;
		PrivateHttpListenPort = 7080;
		PrivateHttpsListenPort = 7043;
		DnsOverrides = new Dictionary<string, string>();
		LogFileUncompressedRetentionDays = 7;
		LogFileCompressedRetentionDays = 30;
		GameServerLogFileAutoCompressionEnabled = false;
		GameServerLogFileAutoCompressionDelay = TimeSpan.FromMinutes(1.0);
		LobbyServerMaxInstances = 8;
		DiscordServerTeamInstanceName = "Atlas Reactor Team Chat";
		DiscordServerGroupInstanceName = "Atlas Reactor Group Chat";
		PaidPlayerWhisperChatRateLimit = "10 per 00:00:10";
		PaidPlayerTeamChatRateLimit = "10 per 00:00:10";
		PaidPlayerGlobalChatRateLimit = "10 per 00:00:10";
		FreePlayerWhisperChatRateLimit = "10 per 00:01:00";
		FreePlayerTeamChatRateLimit = "10 per 00:01:00";
		FreePlayerGlobalChatRateLimit = "10 per 00:01:00";
		FreelancerExperienceBonus = 2f;
		MaxWaitOnLoginRemoteSubsytemLoads = TimeSpan.FromSeconds(15.0);
		CoOpSessionHostListenPort = 6090;
	}

	public override void PostProcess()
	{
		if (PrivateHostName.IsNullOrEmpty() || PrivateHostName == base.HostName)
		{
			if (!PrivateDomain.IsNullOrEmpty())
			{
				PrivateHostName = string.Format("{0}.{1}", base.HostName, PrivateDomain);
			}
			else
			{
				PrivateHostName = base.HostName;
			}
		}
		if (PublicHostName.IsNullOrEmpty() || PublicHostName == base.HostName)
		{
			if (!PublicDomain.IsNullOrEmpty())
			{
				PublicHostName = string.Format("{0}.{1}", base.HostName, PublicDomain);
			}
			else
			{
				PublicHostName = base.HostName;
			}
		}
		BasePath = BasePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		EventLogFilePath = EventLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		GameplayDataPath = GameplayDataPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		PlayerDataPath = PlayerDataPath.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		ServerDataPath = ServerDataPath.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		HtmlPath = HtmlPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		LoadTestAccountsPath = LoadTestAccountsPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		MonoPath = MonoPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		MonitorServerPath = MonitorServerPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		MonitorServerLogFilePath = MonitorServerLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		MonitorServerDebugLogFilePath = MonitorServerDebugLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		MonitorServerMetricsLogFilePath = MonitorServerMetricsLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		LobbyServerPath = LobbyServerPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		LobbyServerLogFilePath = LobbyServerLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		LobbyServerDebugLogFilePath = LobbyServerDebugLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		LobbyServerMetricsLogFilePath = LobbyServerMetricsLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		DirectoryServerPath = DirectoryServerPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		DirectoryServerLogFilePath = DirectoryServerLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		DirectoryServerDebugLogFilePath = DirectoryServerDebugLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		DirectoryServerMetricsLogFilePath = DirectoryServerMetricsLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		MatchmakingServerPath = MatchmakingServerPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		MatchmakingServerLogFilePath = MatchmakingServerLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		MatchmakingServerDebugLogFilePath = MatchmakingServerDebugLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		MatchmakingServerMetricsLogFilePath = MatchmakingServerMetricsLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		RelayServerPath = RelayServerPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		RelayServerLogFilePath = RelayServerLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		RelayServerDebugLogFilePath = RelayServerDebugLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		RelayServerMetricsLogFilePath = RelayServerMetricsLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		GameServerPath = GameServerPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		GameServerLogFilePath = GameServerLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		GameServerDebugLogFilePath = GameServerDebugLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		GameServerOutputLogFilePath = GameServerOutputLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		LoadTestServerPath = LoadTestServerPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		LoadTestServerLogFilePath = LoadTestServerLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		LoadTestServerDebugLogFilePath = LoadTestServerDebugLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		LoadTestServerMetricsLogFilePath = LoadTestServerMetricsLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		LoadTestServerStatusLogFilePath = LoadTestServerStatusLogFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		DatabaseScriptsPath = DatabaseScriptsPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		PublicSslPath = PublicSslPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		PrivateSslPath = PrivateSslPath.SafeReplace("${CONFIGPATH}", base.ConfigPath).SafeGetFullPath();
		PidFilePath = PidFilePath.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		OutageEmailTemplate = OutageEmailTemplate.SafeReplace("${CONFIGPATH}", base.ConfigPath);
		OutageEmailRecipients = (from recipient in OutageEmailRecipients
		select recipient.SafeReplace("${SYSTEMUSERNAME}", base.SystemUserName)).ToList<string>();
		PublicEnvironmentUrl = PublicEnvironmentUrl.SafeReplace("${HOSTNAME}", base.HostName);
		PrivateEnvironmentUrl = PrivateEnvironmentUrl.SafeReplace("${HOSTNAME}", base.HostName);
		CoOpSessionHostListenAddress = CoOpSessionHostListenAddress.SafeReplace("${HOSTNAME}", base.HostName);
		GameServerListenAddress = GameServerListenAddress.SafeReplace("${HOSTNAME}", base.HostName);
		for (int i = 0; i < CoreServers.Length; i++)
		{
			CoreServers[i] = CoreServers[i].SafeReplace("${HOSTNAME}", base.HostName);
		}
		for (int j = 0; j < DirectoryServers.Length; j++)
		{
			DirectoryServers[j] = DirectoryServers[j].SafeReplace("${HOSTNAME}", base.HostName);
		}
		for (int k = 0; k < LobbyServers.Length; k++)
		{
			LobbyServers[k] = LobbyServers[k].SafeReplace("${HOSTNAME}", base.HostName);
		}
		for (int l = 0; l < MatchmakingServers.Length; l++)
		{
			MatchmakingServers[l] = MatchmakingServers[l].SafeReplace("${HOSTNAME}", base.HostName);
		}
		for (int m = 0; m < RelayServers.Length; m++)
		{
			RelayServers[m] = RelayServers[m].SafeReplace("${HOSTNAME}", base.HostName);
		}
		EventLogServerAddress = EventLogServerAddress.SafeReplace("${HOSTNAME}", base.HostName);
		PlatformID platform = Environment.OSVersion.Platform;
		string replacement;
		if (platform != PlatformID.Win32NT)
		{
			if (platform != PlatformID.Unix)
			{
				replacement = "Unknown";
			}
			else
			{
				replacement = "Linux64";
			}
		}
		else
		{
			replacement = "Win32";
		}
		string replacement2 = "Release";
		MonitorServerPath = MonitorServerPath.SafeReplace("${OS}", replacement);
		MonitorServerPath = MonitorServerPath.SafeReplace("${CONFIGURATION}", replacement2);
		MonitorServerPath = MonitorServerPath.SafeGetFullPath();
		LobbyServerPath = LobbyServerPath.SafeReplace("${OS}", replacement);
		LobbyServerPath = LobbyServerPath.SafeReplace("${CONFIGURATION}", replacement2);
		LobbyServerPath = LobbyServerPath.SafeGetFullPath();
		DirectoryServerPath = DirectoryServerPath.SafeReplace("${OS}", replacement);
		DirectoryServerPath = DirectoryServerPath.SafeReplace("${CONFIGURATION}", replacement2);
		DirectoryServerPath = DirectoryServerPath.SafeGetFullPath();
		MatchmakingServerPath = MatchmakingServerPath.SafeReplace("${OS}", replacement);
		MatchmakingServerPath = MatchmakingServerPath.SafeReplace("${CONFIGURATION}", replacement2);
		MatchmakingServerPath = MatchmakingServerPath.SafeGetFullPath();
		RelayServerPath = RelayServerPath.SafeReplace("${OS}", replacement);
		RelayServerPath = RelayServerPath.SafeReplace("${CONFIGURATION}", replacement2);
		RelayServerPath = RelayServerPath.SafeGetFullPath();
		LoadTestServerPath = LoadTestServerPath.SafeReplace("${OS}", replacement);
		LoadTestServerPath = LoadTestServerPath.SafeReplace("${CONFIGURATION}", replacement2);
		LoadTestServerPath = LoadTestServerPath.SafeGetFullPath();
		GameServerPath = GameServerPath.SafeReplace("${OS}", replacement);
		GameServerPath = GameServerPath.SafeGetFullPath();
		string value;
		if (SpamFilterConfig.TryGetValue("DictionaryFileName", out value))
		{
			value = value.SafeReplace("${CONFIGPATH}", base.ConfigPath);
			SpamFilterConfig["DictionaryFileName"] = value;
		}
		string value2;
		if (SpamFilterConfig.TryGetValue("LogFileName", out value2))
		{
			value2 = value2.SafeReplace("${CONFIGPATH}", base.ConfigPath);
			SpamFilterConfig["LogFileName"] = value2;
		}
	}

	private void FinalPostProcess()
	{
		DatabasePrimaryConnectionString = DatabasePrimaryConnectionString.SafeReplace("${HOSTNAME}", base.HostName);
		for (int i = 0; i < DatabasePrimaryConnectionStrings.Length; i++)
		{
			if (DatabasePrimaryConnectionStrings[i].IsNullOrEmpty())
			{
				DatabasePrimaryConnectionStrings[i] = DatabasePrimaryConnectionString;
			}
			DatabasePrimaryConnectionStrings[i] = DatabasePrimaryConnectionStrings[i].SafeReplace("${HOSTNAME}", base.HostName);
		}
		if (DatabaseMirroringEnabled)
		{
			DatabaseSecondaryConnectionString = DatabaseSecondaryConnectionString.SafeReplace("${HOSTNAME}", base.HostName);
			for (int j = 0; j < DatabaseSecondaryConnectionStrings.Length; j++)
			{
				if (DatabaseSecondaryConnectionStrings[j].IsNullOrEmpty())
				{
					DatabaseSecondaryConnectionStrings[j] = DatabaseSecondaryConnectionString;
				}
				DatabaseSecondaryConnectionStrings[j] = DatabaseSecondaryConnectionStrings[j].SafeReplace("${HOSTNAME}", base.HostName);
			}
		}
		else
		{
			DatabaseSecondaryConnectionString = null;
			DatabaseSecondaryConnectionStrings = new string[10];
		}
		if (FacebookOAuth2RedirectUri == null)
		{
			string scheme = PublicSslEnabled ? "https" : "http";
			string host = (EnvironmentType == EnvironmentType.Development) ? "localhost" : PublicEnvironmentUrl;
			int port = PublicSslEnabled ? PublicHttpsListenPort : PublicHttpListenPort;
			FacebookOAuth2RedirectUri = new UriBuilder(scheme, host, port, "/RelayOAuthSessionManager").ToString();
		}
		if (DirectoryServers.IsNullOrEmpty<string>())
		{
			DirectoryServers = CoreServers;
		}
		if (LobbyServers.IsNullOrEmpty<string>())
		{
			LobbyServers = CoreServers;
		}
		if (MatchmakingServers.IsNullOrEmpty<string>())
		{
			MatchmakingServers = CoreServers;
		}
		if (RelayServers.IsNullOrEmpty<string>())
		{
			RelayServers = CoreServers;
		}
	}

	public virtual void Initialize(string configPath)
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		string text = "";
		string path = configPath;
		if (!Directory.Exists(configPath))
		{
			path = Path.GetDirectoryName(configPath);
		}
		string text2 = Path.Combine(path, "../../CommonServer/Config/json");
		if (!Application.isEditor)
		{
			text2 = Path.Combine(path, "json");
		}
		List<string> list = new List<string>();
		if (commandLineArgs != null)
		{
			for (int i = 1; i < commandLineArgs.Length; i++)
			{
				string text3 = commandLineArgs[i];
				if ((text3 == "-o" || text3 == "-option" || text3 == "--option") && i + 1 < commandLineArgs.Length)
				{
					string[] array = commandLineArgs[i + 1].Split(new char[]
					{
						'='
					}, 2);
					string text4 = array[0];
					string arg = (array.Length > 1) ? array[1] : "True";
					text4 = text4.TrimStart(new char[]
					{
						'-'
					});
					string item = string.Format("{{ \"{0}\" : \"{1}\" }}", text4, arg);
					list.Add(item);
					i++;
				}
				else if ((text3 == "-c" || text3 == "-config" || text3 == "--config") && i + 1 < commandLineArgs.Length)
				{
					configPath = commandLineArgs[i + 1];
				}
				else if ((text3 == "-p" || text3 == "-processcode" || text3 == "--processcode") && i + 1 < commandLineArgs.Length)
				{
					string arg2 = commandLineArgs[i + 1];
					string item2 = string.Format("{{ \"ProcessCode\" : \"{0}\" }}", arg2);
					list.Add(item2);
					i++;
				}
				else if ((text3 == "-e" || text3 == "-environment" || text3 == "--environment") && i + 1 < commandLineArgs.Length)
				{
					text = commandLineArgs[i + 1];
					string item3 = string.Format("{{ \"EnvironmentName\" : \"{0}\" }}", text);
					list.Add(item3);
					i++;
				}
				else if (!text3.StartsWith("-"))
				{
					string text5 = commandLineArgs[i];
					if (text5.IndexOfAny(new char[]
					{
						'\\',
						'\'',
						'"'
					}) < 0)
					{
						string item4 = string.Format("{{ \"Command\" : \"{0}\" }}", text5);
						list.Add(item4);
					}
				}
			}
		}
		string fileName = configPath.Replace(".json", ".local.json");
		string fileName2 = configPath.Replace(".json", ".salt.json");
		string fileName3 = text2.Replace(".json", ".local.json");
		string fileName4 = text2.Replace(".json", ".salt.json");
		if (text.IsNullOrEmpty())
		{
			CommonServerConfig commonServerConfig = new CommonServerConfig();
			commonServerConfig.LoadFromFile(fileName, false);
			text = commonServerConfig.EnvironmentName;
		}
		if (text.IsNullOrEmpty())
		{
			CommonServerConfig commonServerConfig2 = new CommonServerConfig();
			commonServerConfig2.LoadFromFile(fileName3, false);
			text = commonServerConfig2.EnvironmentName;
		}
		if (text.IsNullOrEmpty())
		{
			CommonServerConfig commonServerConfig3 = new CommonServerConfig();
			commonServerConfig3.LoadFromFile(fileName2, false);
			text = commonServerConfig3.EnvironmentName;
		}
		if (text.IsNullOrEmpty())
		{
			CommonServerConfig commonServerConfig4 = new CommonServerConfig();
			commonServerConfig4.LoadFromFile(fileName4, false);
			text = commonServerConfig4.EnvironmentName;
		}
		LoadFromFile(text2, true);
		LoadFromFile(configPath, true);
		if (!text.IsNullOrEmpty())
		{
			string newValue = string.Format(".{0}.json", text);
			string fileName5 = text2.Replace(".json", newValue);
			LoadFromFile(fileName5, false);
			string fileName6 = configPath.Replace(".json", newValue);
			LoadFromFile(fileName6, false);
		}
		LoadFromFile(fileName4, false);
		LoadFromFile(fileName2, false);
		LoadFromFile(fileName3, false);
		LoadFromFile(fileName, false);
		foreach (string data in list)
		{
			LoadAppend(data);
		}
		FinalPostProcess();
		EnvironmentName = text;
	}

	public List<string> GetAllMapNames()
	{
		HashSet<string> hashSet = new HashSet<string>();
		foreach (LobbyGameConfig lobbyGameConfig in GameConfigs)
		{
			hashSet.Add(lobbyGameConfig.Map);
		}
		return hashSet.ToList<string>();
	}

	public CommonServerConfig Clone()
	{
		return JsonConvert.DeserializeObject<CommonServerConfig>(JsonConvert.SerializeObject(this));
	}

	public static bool MatchesPath(string path)
	{
		string fileName = Path.GetFileName(path);
		return fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase) && (fileName.StartsWith("CommonServerConfig", StringComparison.OrdinalIgnoreCase) || fileName.StartsWith("MonitorServer", StringComparison.OrdinalIgnoreCase) || fileName.StartsWith("DirectoryServer", StringComparison.OrdinalIgnoreCase) || fileName.StartsWith("LobbyServerConfig", StringComparison.OrdinalIgnoreCase) || fileName.StartsWith("MatchmakingServer", StringComparison.OrdinalIgnoreCase) || fileName.StartsWith("RelayServer", StringComparison.OrdinalIgnoreCase) || fileName.StartsWith("LoadTestServer", StringComparison.OrdinalIgnoreCase));
	}

	// from JsonConfig
	public virtual void LoadAppend(string data)
	{
		JsonConvert.PopulateObject(data, this);
		if (EnablePostProcess)
		{
			PostProcess();
		}
	}
}
#endif
