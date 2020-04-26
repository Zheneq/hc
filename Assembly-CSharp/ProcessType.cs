using System;

[Flags]
public enum ProcessType
{
	None = 0x0,
	DirectoryServer = 0x1,
	LobbyServer = 0x2,
	MatchmakingServer = 0x4,
	RelayServer = 0x8,
	MonitorServer = 0x10,
	LoadTestServer = 0x20,
	ReactorConsole = 0x400,
	AtlasReactor = 0x8000,
	AtlasReactorServer = 0x10000,
	AtlasReactorDev = 0x20000,
	All = -1
}
