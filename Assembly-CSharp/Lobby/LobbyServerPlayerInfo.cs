using System;
using System.Collections.Generic;
using UnityEngine.Networking;

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

#if SERVER
    // added in rogues
    public override void Deserialize(NetworkReader reader)
    {
        base.Deserialize(reader);
        AccountLevel = reader.ReadInt32();
        NumWins = reader.ReadInt32();
        AccMatchmakingCount = reader.ReadInt32();
        int num = reader.ReadInt32();
        CharMatchmakingCount = new Dictionary<CharacterType, int>(num);
        for (int i = 0; i < num; i++)
        {
            CharacterType key = (CharacterType)reader.ReadInt16();
            int value = reader.ReadInt32();
            CharMatchmakingCount[key] = value;
        }
        num = reader.ReadInt32();
        ProxyPlayerIds = new List<int>(num);
        for (int i = 0; i < num; i++)
        {
            ProxyPlayerIds.Add(reader.ReadInt32());
        }
        GroupIdAtStartOfMatch = reader.ReadInt64();
        GroupSizeAtStartOfMatch = reader.ReadInt32();
        GroupLeader = reader.ReadBoolean();
        EffectiveClientAccessLevel = (ClientAccessLevel)reader.ReadInt16();
    }

    // added in rogues
    public override void Serialize(NetworkWriter writer)
    {
        base.Serialize(writer);
        writer.Write(AccountLevel);
        writer.Write(NumWins);
        writer.Write(AccMatchmakingCount);
        int count = CharMatchmakingCount.Count;
        writer.Write(count);
        foreach (KeyValuePair<CharacterType, int> keyValuePair in CharMatchmakingCount)
        {
            writer.Write((short)keyValuePair.Key);
            writer.Write(keyValuePair.Value);
        }
        count = ProxyPlayerIds.Count;
        writer.Write(count);
        foreach (int num in ProxyPlayerIds)
        {
            writer.Write(num);
        }
        writer.Write(GroupIdAtStartOfMatch);
        writer.Write(GroupSizeAtStartOfMatch);
        writer.Write(GroupLeader);
        writer.Write((short)EffectiveClientAccessLevel);
    }
#endif
}
