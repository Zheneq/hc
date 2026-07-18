using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class LobbyTeamInfo
{
	public List<LobbyPlayerInfo> TeamPlayerInfo;
	[JsonIgnore]
	public IEnumerable<LobbyPlayerInfo> TeamAPlayerInfo => TeamInfo(Team.TeamA);
	[JsonIgnore]
	public IEnumerable<LobbyPlayerInfo> TeamBPlayerInfo => TeamInfo(Team.TeamB);
	[JsonIgnore]
	public IEnumerable<LobbyPlayerInfo> SpectatorInfo => TeamInfo(Team.Spectator);
	[JsonIgnore]
	public int TotalPlayerCount => TeamPlayerInfo?.Count ?? 0;

	public IEnumerable<LobbyPlayerInfo> TeamInfo(Team team)
	{
		if (TeamPlayerInfo == null)
		{
			return Enumerable.Empty<LobbyPlayerInfo>();
		}
		return TeamPlayerInfo.Where(p => p.TeamId == team);
	}

	public static LobbyTeamInfo FromServer(LobbyServerTeamInfo serverInfo, int maxPlayerLevel, MatchmakingQueueConfig queueConfig)
	{
		if (serverInfo == null)
		{
			return null;
		}
		LobbyTeamInfo lobbyTeamInfo = new LobbyTeamInfo();
		if (serverInfo.TeamPlayerInfo != null)
		{
			lobbyTeamInfo.TeamPlayerInfo = new List<LobbyPlayerInfo>();
			foreach (LobbyServerPlayerInfo current in serverInfo.TeamPlayerInfo)
			{
				lobbyTeamInfo.TeamPlayerInfo.Add(LobbyPlayerInfo.FromServer(current, maxPlayerLevel, queueConfig));
			}
		}
		return lobbyTeamInfo;
	}

	public LobbyPlayerInfo GetPlayer(long account)
	{
		if (TeamPlayerInfo == null)
		{
			return null;
		}
		foreach (LobbyPlayerInfo playerInfo in TeamPlayerInfo)
		{
			if (playerInfo.AccountId == account)
			{
				return playerInfo;
			}
		}
		return null;
	}

	public LobbyTeamInfo Clone()
	{
		return (LobbyTeamInfo)MemberwiseClone();
	}
}
