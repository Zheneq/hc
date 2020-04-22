public class CoinSpawnPointTrackingData
{
	private CoinSpawnRule m_spawnRules;

	private BoardSquare m_square;

	public int TurnOfLastPickup
	{
		get;
		set;
	}

	public int TurnOfLastSpawn
	{
		get;
		set;
	}

	public int NumSpawnedSoFar
	{
		get;
		set;
	}

	public CoinSpawnPointTrackingData(CoinSpawnRule rule, BoardSquare square)
	{
		m_spawnRules = rule;
		m_square = square;
	}

	public BoardSquare GetSpawnSquare()
	{
		return m_square;
	}

	public bool CanSpawnThisTurn(int turn)
	{
		bool flag = false;
		if (m_spawnRules.m_maxLifetimeSpawns > 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (NumSpawnedSoFar > m_spawnRules.m_maxLifetimeSpawns)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
		}
		if (NumSpawnedSoFar > 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return turn > TurnOfLastPickup + m_spawnRules.m_turnsBeforeSpawnAfterPickup;
				}
			}
		}
		return turn > m_spawnRules.m_turnsBeforeInitialSpawn;
	}
}
