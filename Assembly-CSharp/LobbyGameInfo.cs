using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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

	public LobbyGameInfo()
	{
		this.ggPackUsedAccountIDs = new Dictionary<long, int>();
	}

	public LobbyGameInfo Clone()
	{
		return (LobbyGameInfo)base.MemberwiseClone();
	}

	[JsonIgnore]
	public string Name
	{
		get
		{
			if (!this.GameServerProcessCode.IsNullOrEmpty())
			{
				if (this.GameConfig == null)
				{
				}
				else
				{
					if (!this.GameConfig.HasSelectedSubType)
					{
						return string.Format("{0} ({1}) [{2} {3}]", new object[]
						{
							this.GameServerProcessCode,
							this.GameServerAddress,
							this.GameConfig.Map,
							this.GameConfig.GameType
						});
					}
					return string.Format("{0} ({1}) [{2} {3} {4}]", new object[]
					{
						this.GameServerProcessCode,
						this.GameServerAddress,
						this.GameConfig.Map,
						this.GameConfig.GameType,
						this.GameConfig.InstanceSubType.GetNameAsPayload().Term
					});
				}
			}
			return "unknown";
		}
	}

	[JsonIgnore]
	public bool IsCustomGame
	{
		get
		{
			return this.GameConfig != null && this.GameConfig.GameType == GameType.Custom;
		}
	}

	[JsonIgnore]
	public bool IsQueuedGame
	{
		get
		{
			bool result;
			if (this.GameConfig != null)
			{
				result = this.GameConfig.GameType.IsQueueable();
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	public override string ToString()
	{
		return string.Format("{0} (gameStatus {1}, gameResult {2})", this.Name, this.GameStatus, this.GameResult);
	}
}
