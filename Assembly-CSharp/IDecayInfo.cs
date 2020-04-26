using System;

public interface IDecayInfo
{
	bool IsActive
	{
		get;
	}

	DateTime UtcNow
	{
		get;
	}

	bool GetDecayAmount(int tierIndex, out int amount, out TimeSpan start);

	bool DoesTierHaveLimitlessLesserNeighborTier(int tierIndex);
}
