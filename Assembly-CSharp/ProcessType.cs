using System;

[Flags]
public enum ProcessType
{
	None = 0,
	DirectoryServer = 1,
	LobbyServer = 2,
	MatchmakingServer = 4,
	RelayServer = 8,
	MonitorServer = 0x10,
	LoadTestServer = 0x20,
	ReactorConsole = 0x400,
	AtlasReactor = 0x8000,
	AtlasReactorServer = 0x10000,
	AtlasReactorDev = 0x20000,
	All = -1
}
