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
		using (Dictionary<Team, Dictionary<int, ELODancecard>>.Enumerator enumerator = m_ELODancecard.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				foreach (ELODancecard value in enumerator.Current.Value.Values)
				{
					if (value.m_accountId == accountId)
					{
						return value;
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					goto end_IL_000e;
				}
			}
			end_IL_000e:;
		}
		Log.Warning("Account {0}'s dancecard not found in game", accountId);
		return null;
	}

	public ELODancecard GetEloDancecardByPlayerId(int playerId)
	{
		using (Dictionary<Team, Dictionary<int, ELODancecard>>.Enumerator enumerator = m_ELODancecard.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Value.TryGetValue(playerId, out ELODancecard value))
				{
					return value;
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					goto end_IL_000c;
				}
			}
			end_IL_000c:;
		}
		Log.Warning("PlayerId {0}'s dancecard not found in game", playerId);
		return null;
	}

	public void CreateELODancecard(int playerId, Team teamId, long accountId, long groupId, byte groupSize)
	{
		if (m_ELODancecard.TryGetValue(teamId, out Dictionary<int, ELODancecard> value))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					value.Add(playerId, ELODancecard.Create(accountId, groupId, groupSize));
					return;
				}
			}
		}
		m_ELODancecard.Add(teamId, new Dictionary<int, ELODancecard>());
		m_ELODancecard[teamId].Add(playerId, ELODancecard.Create(accountId, groupId, groupSize));
	}

	public void UpdateELODancecard(int playerId, Team teamId, long accountId, bool isBot, BotDifficulty difficulty)
	{
		if (m_ELODancecard.TryGetValue(teamId, out Dictionary<int, ELODancecard> value))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					if (value.TryGetValue(playerId, out ELODancecard value2))
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								value2.Increment(isBot, difficulty);
								return;
							}
						}
					}
					value.Add(playerId, ELODancecard.Create(accountId, isBot, difficulty));
					return;
				}
				}
			}
		}
		m_ELODancecard.Add(teamId, new Dictionary<int, ELODancecard>());
		m_ELODancecard[teamId].Add(playerId, ELODancecard.Create(accountId, isBot, difficulty));
	}
}
