using System;

public interface IPersistedGameplayStat
{
	float GetSum();

	float GetMin();

	float GetMax();

	int GetNumGames();

	float Average();
}
