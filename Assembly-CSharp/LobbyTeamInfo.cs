using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class LobbyTeamInfo
{
	public List<LobbyPlayerInfo> TeamPlayerInfo;

	[JsonIgnore]
	public IEnumerable<LobbyPlayerInfo> TeamAPlayerInfo
	{
		get
		{
			return this.TeamInfo(Team.TeamA);
		}
	}

	[JsonIgnore]
	public IEnumerable<LobbyPlayerInfo> TeamBPlayerInfo
	{
		get
		{
			return this.TeamInfo(Team.TeamB);
		}
	}

	[JsonIgnore]
	public IEnumerable<LobbyPlayerInfo> SpectatorInfo
	{
		get
		{
			return this.TeamInfo(Team.Spectator);
		}
	}

	[JsonIgnore]
	public int TotalPlayerCount
	{
		get
		{
			return (this.TeamPlayerInfo == null) ? 0 : this.TeamPlayerInfo.Count;
		}
	}

	public IEnumerable<LobbyPlayerInfo> TeamInfo(Team team)
	{
		if (this.TeamPlayerInfo == null)
		{
			return Enumerable.Empty<LobbyPlayerInfo>();
		}
		return from p in this.TeamPlayerInfo
		where p.TeamId == team
		select p;
	}

	public static LobbyTeamInfo FromServer(LobbyServerTeamInfo serverInfo, int maxPlayerLevel, MatchmakingQueueConfig queueConfig)
	{
		LobbyTeamInfo lobbyTeamInfo = null;
		if (serverInfo != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyTeamInfo.FromServer(LobbyServerTeamInfo, int, MatchmakingQueueConfig)).MethodHandle;
			}
			lobbyTeamInfo = new LobbyTeamInfo();
			if (serverInfo.TeamPlayerInfo != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				lobbyTeamInfo.TeamPlayerInfo = new List<LobbyPlayerInfo>();
				using (List<LobbyServerPlayerInfo>.Enumerator enumerator = serverInfo.TeamPlayerInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LobbyServerPlayerInfo serverInfo2 = enumerator.Current;
						lobbyTeamInfo.TeamPlayerInfo.Add(LobbyPlayerInfo.FromServer(serverInfo2, maxPlayerLevel, queueConfig));
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}
		return lobbyTeamInfo;
	}

	public LobbyPlayerInfo GetPlayer(long account)
	{
		if (this.TeamPlayerInfo == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyTeamInfo.GetPlayer(long)).MethodHandle;
			}
			return null;
		}
		for (int i = 0; i < this.TeamPlayerInfo.Count; i++)
		{
			if (this.TeamPlayerInfo[i].AccountId == account)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.TeamPlayerInfo[i];
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		return null;
	}

	public LobbyTeamInfo Clone()
	{
		return (LobbyTeamInfo)base.MemberwiseClone();
	}
}
