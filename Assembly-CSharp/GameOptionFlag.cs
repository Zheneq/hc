using System;

[Flags]
public enum GameOptionFlag
{
	None = 0,
	AllowDuplicateCharacters = 1,
	AllowPausing = 2,
	SkipEndOfGameCheck = 4,
	ReplaceHumansWithBots = 8,
	FakeClientConnections = 0x10,
	FakeGameServer = 0x20,
	NoInputIdleDisconnect = 0x40,
	EnableTeamAIOutput = 0x80,
	AutoLaunch = 0x100
}
