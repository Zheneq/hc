using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
	public int TotalPlayerCount => (TeamPlayerInfo != null) ? TeamPlayerInfo.Count : 0;

	public IEnumerable<LobbyPlayerInfo> TeamInfo(Team team)
	{
		if (TeamPlayerInfo == null)
		{
			return Enumerable.Empty<LobbyPlayerInfo>();
		}
		return TeamPlayerInfo.Where((LobbyPlayerInfo p) => p.TeamId == team);
	}

	public static LobbyTeamInfo FromServer(LobbyServerTeamInfo serverInfo, int maxPlayerLevel, MatchmakingQueueConfig queueConfig)
	{
		LobbyTeamInfo lobbyTeamInfo = null;
		if (serverInfo != null)
		{
			lobbyTeamInfo = new LobbyTeamInfo();
			if (serverInfo.TeamPlayerInfo != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						lobbyTeamInfo.TeamPlayerInfo = new List<LobbyPlayerInfo>();
						using (List<LobbyServerPlayerInfo>.Enumerator enumerator = serverInfo.TeamPlayerInfo.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								LobbyServerPlayerInfo current = enumerator.Current;
								lobbyTeamInfo.TeamPlayerInfo.Add(LobbyPlayerInfo.FromServer(current, maxPlayerLevel, queueConfig));
							}
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									return lobbyTeamInfo;
								}
							}
						}
					}
					}
				}
			}
		}
		return lobbyTeamInfo;
	}

	public LobbyPlayerInfo GetPlayer(long account)
	{
		if (TeamPlayerInfo == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		for (int i = 0; i < TeamPlayerInfo.Count; i++)
		{
			if (TeamPlayerInfo[i].AccountId != account)
			{
				continue;
			}
			while (true)
			{
				return TeamPlayerInfo[i];
			}
		}
		while (true)
		{
			return null;
		}
	}

	public LobbyTeamInfo Clone()
	{
		return (LobbyTeamInfo)MemberwiseClone();
	}
}
