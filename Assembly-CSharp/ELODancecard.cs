using System;

[Serializable]
public class ELODancecard
{
	public BotDifficulty m_botDifficulty = BotDifficulty.Expert;

	public uint m_turnsAsPlayer;

	public uint m_turnsAsBot;

	public long m_accountId;

	public long m_groupId;

	public byte m_groupSize = 1;

	public bool m_playedLastTurn;

	public string LogString
	{
		get
		{
			if (m_turnsAsBot == 0)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return $"account #{m_accountId} {m_turnsAsPlayer} turns";
					}
				}
			}
			if (m_turnsAsPlayer == 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return $"{m_botDifficulty} bot {m_turnsAsBot} turns";
					}
				}
			}
			return $"account #{m_accountId} {m_turnsAsPlayer} turns, {m_turnsAsBot} turns as {m_botDifficulty} bot";
		}
	}

	public static ELODancecard Create(long accountId, long groupId, byte groupSize)
	{
		if (groupSize == 0)
		{
			Log.Error("ELODancecard: Bad group size of zero for accountId {0}, groupId {1}!", accountId, groupId);
			int num;
			if (groupId == 0)
			{
				num = 1;
			}
			else
			{
				num = 2;
			}
			groupSize = (byte)num;
		}
		ELODancecard eLODancecard = new ELODancecard();
		eLODancecard.m_accountId = accountId;
		eLODancecard.m_groupId = groupId;
		eLODancecard.m_groupSize = groupSize;
		return eLODancecard;
	}

	public static ELODancecard Create(long accountId, bool isBot, BotDifficulty dif)
	{
		ELODancecard eLODancecard = new ELODancecard();
		eLODancecard.m_accountId = accountId;
		ELODancecard eLODancecard2 = eLODancecard;
		eLODancecard2.Increment(isBot, dif);
		return eLODancecard2;
	}

	public void Increment(bool isBot, BotDifficulty dif)
	{
		if (isBot)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					m_turnsAsBot++;
					m_botDifficulty = dif;
					m_playedLastTurn = false;
					return;
				}
			}
		}
		m_turnsAsPlayer++;
		m_playedLastTurn = true;
	}
}
