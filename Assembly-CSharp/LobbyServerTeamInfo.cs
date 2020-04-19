using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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
		get
		{
			return this.TeamInfo(Team.TeamA);
		}
	}

	[JsonIgnore]
	public IEnumerable<LobbyServerPlayerInfo> TeamBPlayerInfo
	{
		get
		{
			return this.TeamInfo(Team.TeamB);
		}
	}

	[JsonIgnore]
	public IEnumerable<LobbyServerPlayerInfo> SpectatorInfo
	{
		get
		{
			return this.TeamInfo(Team.Spectator);
		}
	}

	public IEnumerable<LobbyServerPlayerInfo> TeamInfo(Team team)
	{
		if (this.TeamPlayerInfo == null)
		{
			return Enumerable.Empty<LobbyServerPlayerInfo>();
		}
		return from p in this.TeamPlayerInfo
		where p.TeamId == team
		select p;
	}
}
