using System;

public class CoinSpawnPointTrackingData
{
	private CoinSpawnRule m_spawnRules;

	private BoardSquare m_square;

	public CoinSpawnPointTrackingData(CoinSpawnRule rule, BoardSquare square)
	{
		this.m_spawnRules = rule;
		this.m_square = square;
	}

	public int TurnOfLastPickup { get; set; }

	public int TurnOfLastSpawn { get; set; }

	public int NumSpawnedSoFar { get; set; }

	public BoardSquare GetSpawnSquare()
	{
		return this.m_square;
	}

	public bool CanSpawnThisTurn(int turn)
	{
		if (this.m_spawnRules.m_maxLifetimeSpawns > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinSpawnPointTrackingData.CanSpawnThisTurn(int)).MethodHandle;
			}
			if (this.NumSpawnedSoFar > this.m_spawnRules.m_maxLifetimeSpawns)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				return false;
			}
		}
		bool result;
		if (this.NumSpawnedSoFar > 0)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			result = (turn > this.TurnOfLastPickup + this.m_spawnRules.m_turnsBeforeSpawnAfterPickup);
		}
		else
		{
			result = (turn > this.m_spawnRules.m_turnsBeforeInitialSpawn);
		}
		return result;
	}
}
