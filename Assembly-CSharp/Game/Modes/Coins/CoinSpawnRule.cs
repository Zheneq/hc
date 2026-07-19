using System;

[Serializable]
public class CoinSpawnRule
{
	public int m_turnsBeforeInitialSpawn;

	public int m_turnsBeforeSpawnAfterPickup = 2;

	public int m_maxLifetimeSpawns = -1;
}
