using System;
using System.Collections.Generic;

[Serializable]
public class LobbyServerPlayerInfo : LobbyPlayerCommonInfo
{
	public int AccountLevel;
	public int TotalLevel;
	public int NumWins;
	public float AccMatchmakingElo;
	public int AccMatchmakingCount;
	public Dictionary<CharacterType, float> CharMatchmakingElo;
	public Dictionary<CharacterType, int> CharMatchmakingCount;
	public float UsedMatchmakingElo;
	public int RankedTier;
	public float RankedPoints;
	public string MatchmakingEloKey;
	public List<int> ProxyPlayerIds = new List<int>();
	public long GroupIdAtStartOfMatch;
	public int GroupSizeAtStartOfMatch;
	public bool GroupLeader;
	public ClientAccessLevel EffectiveClientAccessLevel;
	public int RankedSortKarma;

	public LobbyServerPlayerInfo Clone()
	{
		return (LobbyServerPlayerInfo)MemberwiseClone();
	}
}
