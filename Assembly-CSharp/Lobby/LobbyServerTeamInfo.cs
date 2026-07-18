using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Networking;

[Serializable]
public class LobbyServerTeamInfo
{
    public List<LobbyServerPlayerInfo> TeamPlayerInfo;
    public Dictionary<long, TierPlacement> TierChangeMins;
    public Dictionary<long, TierPlacement> TierChangeMaxs;
    public Dictionary<long, TierPlacement> TierCurrents;

    [JsonIgnore]
    public IEnumerable<LobbyServerPlayerInfo> TeamAPlayerInfo => TeamInfo(Team.TeamA);

    [JsonIgnore]
    public IEnumerable<LobbyServerPlayerInfo> TeamBPlayerInfo => TeamInfo(Team.TeamB);

    [JsonIgnore]
    public IEnumerable<LobbyServerPlayerInfo> SpectatorInfo => TeamInfo(Team.Spectator);

    public IEnumerable<LobbyServerPlayerInfo> TeamInfo(Team team)
    {
        if (TeamPlayerInfo == null)
        {
            return Enumerable.Empty<LobbyServerPlayerInfo>();
        }

        return TeamPlayerInfo.Where(p => p.TeamId == team);
    }

#if SERVER
    // added in rogues
    public void Deserialize(NetworkReader reader)
    {
        int num = reader.ReadInt32();
        TeamPlayerInfo = new List<LobbyServerPlayerInfo>(num);
        for (int i = 0; i < num; i++)
        {
            LobbyServerPlayerInfo lobbyServerPlayerInfo = new LobbyServerPlayerInfo();
            lobbyServerPlayerInfo.Deserialize(reader);
            TeamPlayerInfo.Add(lobbyServerPlayerInfo);
        }
    }

    // custom
    public void Serialize(NetworkWriter writer)
    {
        writer.Write(TeamPlayerInfo.Count);
        foreach (var lobbyServerPlayerInfo in TeamPlayerInfo)
        {
            lobbyServerPlayerInfo.Serialize(writer);
        }
    }
#endif
}