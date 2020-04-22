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
	public string Name
	{
		get
		{
			if (!GameServerProcessCode.IsNullOrEmpty())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (GameConfig != null)
				{
					if (!GameConfig.HasSelectedSubType)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return $"{GameServerProcessCode} ({GameServerAddress}) [{GameConfig.Map} {GameConfig.GameType}]";
							}
						}
					}
					return $"{GameServerProcessCode} ({GameServerAddress}) [{GameConfig.Map} {GameConfig.GameType} {GameConfig.InstanceSubType.GetNameAsPayload().Term}]";
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			return "unknown";
		}
	}

	[JsonIgnore]
	public bool IsCustomGame => GameConfig != null && GameConfig.GameType == GameType.Custom;

	[JsonIgnore]
	public bool IsQueuedGame
	{
		get
		{
			int result;
			if (GameConfig != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				result = (GameConfig.GameType.IsQueueable() ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}

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
