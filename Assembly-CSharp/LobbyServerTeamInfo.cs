using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class LobbyServerTeamInfo
{
	public List<LobbyServerPlayerInfo> TeamPlayerInfo;

	public Dictionary<long, TierPlacement> TierChangeMins;

	public Dictionary<long, TierPlacement> TierChangeMaxs;

	public Dictionary<long, TierPlacement> TierCurrents;

	[JsonIgnore]
	public IEnumerable<LobbyServerPlayerInfo> TeamAPlayerInfo
	{
		get { return TeamInfo(Team.TeamA); }
	}

	[JsonIgnore]
	public IEnumerable<LobbyServerPlayerInfo> TeamBPlayerInfo
	{
		get { return TeamInfo(Team.TeamB); }
	}

	[JsonIgnore]
	public IEnumerable<LobbyServerPlayerInfo> SpectatorInfo
	{
		get { return TeamInfo(Team.Spectator); }
	}

	public IEnumerable<LobbyServerPlayerInfo> TeamInfo(Team team)
	{
		if (TeamPlayerInfo == null)
		{
			return Enumerable.Empty<LobbyServerPlayerInfo>();
		}
		return TeamPlayerInfo.Where((LobbyServerPlayerInfo p) => p.TeamId == team);
	}
}
