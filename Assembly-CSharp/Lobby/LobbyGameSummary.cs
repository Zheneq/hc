// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

[Serializable]
public class LobbyGameSummary
{
	public string GameServerAddress;
	public GameResult GameResult;
	public float GameResultFraction = 0.5f;
	public string TimeText = string.Empty;
	public int NumOfTurns;
	public int TeamAPoints;
	public int TeamBPoints;
	public TimeSpan MatchTime;
	public List<PlayerGameSummary> PlayerGameSummaryList = new List<PlayerGameSummary>();
	
	// TODO LOW serialize?
	// removed in rogues
	public Dictionary<Team, Dictionary<int, ELODancecard>> m_ELODancecard = new Dictionary<Team, Dictionary<int, ELODancecard>>();
	
	public List<BadgeAndParticipantInfo> BadgeAndParticipantsInfo;

	// removed in rogues
	public ELODancecard GetEloDancecardByAccountId(long accountId)
	{
		foreach (Dictionary<int, ELODancecard> eloDancecards in m_ELODancecard.Values)
		{
			foreach (ELODancecard value in eloDancecards.Values)
			{
				if (value.m_accountId == accountId)
				{
					return value;
				}
			}
		}
		Log.Warning("Account {0}'s dancecard not found in game", accountId);
		return null;
	}

	// removed in rogues
	public ELODancecard GetEloDancecardByPlayerId(int playerId)
	{
		foreach (Dictionary<int, ELODancecard> eloDancecards in m_ELODancecard.Values)
		{
			if (eloDancecards.TryGetValue(playerId, out ELODancecard value))
			{
				return value;
			}
		}
		Log.Warning("PlayerId {0}'s dancecard not found in game", playerId);
		return null;
	}

	// removed in rogues
	public void CreateELODancecard(int playerId, Team teamId, long accountId, long groupId, byte groupSize)
	{
		if (m_ELODancecard.TryGetValue(teamId, out Dictionary<int, ELODancecard> value))
		{
			value.Add(playerId, ELODancecard.Create(accountId, groupId, groupSize));
		}
		else
		{
			m_ELODancecard.Add(teamId, new Dictionary<int, ELODancecard>());
			m_ELODancecard[teamId].Add(playerId, ELODancecard.Create(accountId, groupId, groupSize));
		}
	}

	// removed in rogues
	public void UpdateELODancecard(int playerId, Team teamId, long accountId, bool isBot, BotDifficulty difficulty)
	{
		if (m_ELODancecard.TryGetValue(teamId, out Dictionary<int, ELODancecard> value))
		{
			if (value.TryGetValue(playerId, out ELODancecard value2))
			{
				value2.Increment(isBot, difficulty);
			}
			else
			{
				value.Add(playerId, ELODancecard.Create(accountId, isBot, difficulty));
			}
		}
		else
		{
			m_ELODancecard.Add(teamId, new Dictionary<int, ELODancecard>());
			m_ELODancecard[teamId].Add(playerId, ELODancecard.Create(accountId, isBot, difficulty));
		}
	}

#if SERVER
	// added in rogues
	public void Deserialize(NetworkReader reader)
	{
		GameResult = (GameResult)reader.ReadInt16();
		GameResultFraction = reader.ReadSingle();
		TimeText = reader.ReadString();
		NumOfTurns = reader.ReadInt32();
		TeamAPoints = reader.ReadInt32();
		TeamBPoints = reader.ReadInt32();
		MatchTime = new TimeSpan(reader.ReadInt64());
		int num = reader.ReadInt32();
		PlayerGameSummaryList = new List<PlayerGameSummary>(num);
		for (int i = 0; i < num; i++)
		{
			PlayerGameSummary playerGameSummary = new PlayerGameSummary();
			playerGameSummary.Deserialize(reader);
			PlayerGameSummaryList.Add(playerGameSummary);
		}
	}

	// added in rogues
	public void Serialize(NetworkWriter writer)
	{
		writer.Write((short)GameResult);
		writer.Write(GameResultFraction);
		writer.Write(TimeText);
		writer.Write(NumOfTurns);
		writer.Write(TeamAPoints);
		writer.Write(TeamBPoints);
		writer.Write(MatchTime.Ticks);
		int count = PlayerGameSummaryList.Count;
		writer.Write(count);
		foreach (PlayerGameSummary playerGameSummary in PlayerGameSummaryList)
		{
			playerGameSummary.Serialize(writer);
		}
	}
#endif
}
