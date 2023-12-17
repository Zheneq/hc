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
                return $"account #{m_accountId} {m_turnsAsPlayer} turns";
            }

            if (m_turnsAsPlayer == 0)
            {
                return $"{m_botDifficulty} bot {m_turnsAsBot} turns";
            }

            return $"account #{m_accountId} {m_turnsAsPlayer} turns, {m_turnsAsBot} turns as {m_botDifficulty} bot";
        }
    }

    public static ELODancecard Create(long accountId, long groupId, byte groupSize)
    {
        if (groupSize == 0)
        {
            Log.Error("ELODancecard: Bad group size of zero for accountId {0}, groupId {1}!", accountId, groupId);
            groupSize = (byte)(groupId == 0 ? 1 : 2);
        }

        return new ELODancecard
        {
            m_accountId = accountId,
            m_groupId = groupId,
            m_groupSize = groupSize
        };
    }

    public static ELODancecard Create(long accountId, bool isBot, BotDifficulty dif)
    {
        ELODancecard dancecard = new ELODancecard
        {
            m_accountId = accountId
        };
        dancecard.Increment(isBot, dif);
        return dancecard;
    }

    public void Increment(bool isBot, BotDifficulty dif)
    {
        if (isBot)
        {
            m_turnsAsBot++;
            m_botDifficulty = dif;
            m_playedLastTurn = false;
        }
        else
        {
            m_turnsAsPlayer++;
            m_playedLastTurn = true;
        }
    }
}