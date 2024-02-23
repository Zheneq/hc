using System;
using System.Collections.Generic;

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
	public Dictionary<Team, Dictionary<int, ELODancecard>> m_ELODancecard = new Dictionary<Team, Dictionary<int, ELODancecard>>();
	public List<BadgeAndParticipantInfo> BadgeAndParticipantsInfo;

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

	public ELODancecard GetEloDancecardByPlayerId(int playerId)
	{
		foreach (Dictionary<int, ELODancecard> eloDancecards in m_ELODancecard.Values)
		{
			ELODancecard value;
			if (eloDancecards.TryGetValue(playerId, out value))
			{
				return value;
			}
		}
		Log.Warning("PlayerId {0}'s dancecard not found in game", playerId);
		return null;
	}

	public void CreateELODancecard(int playerId, Team teamId, long accountId, long groupId, byte groupSize)
	{
		Dictionary<int, ELODancecard> value;
		if (m_ELODancecard.TryGetValue(teamId, out value))
		{
			value.Add(playerId, ELODancecard.Create(accountId, groupId, groupSize));
		}
		else
		{
			m_ELODancecard.Add(teamId, new Dictionary<int, ELODancecard>());
			m_ELODancecard[teamId].Add(playerId, ELODancecard.Create(accountId, groupId, groupSize));
		}
	}

	public void UpdateELODancecard(int playerId, Team teamId, long accountId, bool isBot, BotDifficulty difficulty)
	{
		Dictionary<int, ELODancecard> value;
		if (m_ELODancecard.TryGetValue(teamId, out value))
		{
			ELODancecard value2;
			if (value.TryGetValue(playerId, out value2))
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
}
