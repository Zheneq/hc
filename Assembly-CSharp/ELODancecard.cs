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

	public static ELODancecard Create(long accountId, long groupId, byte groupSize)
	{
		if (groupSize == 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ELODancecard.Create(long, long, byte)).MethodHandle;
			}
			Log.Error("ELODancecard: Bad group size of zero for accountId {0}, groupId {1}!", new object[]
			{
				accountId,
				groupId
			});
			byte b;
			if (groupId == 0L)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				b = 1;
			}
			else
			{
				b = 2;
			}
			groupSize = b;
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
		ELODancecard elodancecard = new ELODancecard
		{
			m_accountId = accountId
		};
		elodancecard.Increment(isBot, dif);
		return elodancecard;
	}

	public void Increment(bool isBot, BotDifficulty dif)
	{
		if (isBot)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ELODancecard.Increment(bool, BotDifficulty)).MethodHandle;
			}
			this.m_turnsAsBot += 1U;
			this.m_botDifficulty = dif;
			this.m_playedLastTurn = false;
		}
		else
		{
			this.m_turnsAsPlayer += 1U;
			this.m_playedLastTurn = true;
		}
	}

	public string LogString
	{
		get
		{
			if (this.m_turnsAsBot == 0U)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ELODancecard.get_LogString()).MethodHandle;
				}
				return string.Format("account #{0} {1} turns", this.m_accountId, this.m_turnsAsPlayer);
			}
			if (this.m_turnsAsPlayer == 0U)
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
				return string.Format("{0} bot {1} turns", this.m_botDifficulty, this.m_turnsAsBot);
			}
			return string.Format("account #{0} {1} turns, {2} turns as {3} bot", new object[]
			{
				this.m_accountId,
				this.m_turnsAsPlayer,
				this.m_turnsAsBot,
				this.m_botDifficulty
			});
		}
	}
}
