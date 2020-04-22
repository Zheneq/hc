using System;

[Flags]
public enum GameOptionFlag
{
	None = 0x0,
	AllowDuplicateCharacters = 0x1,
	AllowPausing = 0x2,
	SkipEndOfGameCheck = 0x4,
	ReplaceHumansWithBots = 0x8,
	FakeClientConnections = 0x10,
	FakeGameServer = 0x20,
	NoInputIdleDisconnect = 0x40,
	EnableTeamAIOutput = 0x80,
	AutoLaunch = 0x100
}
