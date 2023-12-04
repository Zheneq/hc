using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class LobbyGameInfo
{
	public string MonitorServerProcessCode;
	public string GameServerProcessCode;
	public string GameServerAddress;
	public string GameServerHost;
	public long CreateTimestamp;
	public long UpdateTimestamp;
	public long SelectionStartTimestamp;
	public long SelectionSubPhaseStartTimestamp;
	public long LoadoutSelectionStartTimestamp;
	public GameStatus GameStatus = GameStatus.Stopped;
	public GameResult GameResult;
	public FreelancerResolutionPhaseSubType SelectionSubPhase = FreelancerResolutionPhaseSubType.WAITING_FOR_ALL_PLAYERS;
	public TimeSpan AcceptTimeout;
	public TimeSpan SelectTimeout;
	public TimeSpan LoadoutSelectTimeout;
	public TimeSpan SelectSubPhaseBan1Timeout;
	public TimeSpan SelectSubPhaseBan2Timeout;
	public TimeSpan SelectSubPhaseFreelancerSelectTimeout;
	public TimeSpan SelectSubPhaseTradeTimeout;
	public bool IsActive;
	public int ActivePlayers;
	public int ActiveHumanPlayers;
	public int ActiveSpectators;
	public int AcceptedPlayers;
	public BotDifficulty SelectedBotSkillTeamA;
	public BotDifficulty SelectedBotSkillTeamB;
	public Dictionary<long, int> ggPackUsedAccountIDs;
	public Dictionary<long, Dictionary<int, int>> AccountIdToOverconIdToCount;
	public LobbyGameConfig GameConfig;

	[JsonIgnore]
	public string Name =>
		!GameServerProcessCode.IsNullOrEmpty() && GameConfig != null
			? !GameConfig.HasSelectedSubType
				? $"{GameServerProcessCode} ({GameServerAddress}) [{GameConfig.Map} {GameConfig.GameType}]"
				: $"{GameServerProcessCode} ({GameServerAddress}) [{GameConfig.Map} {GameConfig.GameType} {GameConfig.InstanceSubType.GetNameAsPayload().Term}]"
			: "unknown";

	[JsonIgnore]
	public bool IsCustomGame => GameConfig != null && GameConfig.GameType == GameType.Custom;

	[JsonIgnore]
	public bool IsQueuedGame => GameConfig != null && GameConfig.GameType.IsQueueable();

	public LobbyGameInfo()
	{
		ggPackUsedAccountIDs = new Dictionary<long, int>();
	}

	public LobbyGameInfo Clone()
	{
		return (LobbyGameInfo)MemberwiseClone();
	}

	public override string ToString()
	{
		return $"{Name} (gameStatus {GameStatus}, gameResult {GameResult})";
	}
}
