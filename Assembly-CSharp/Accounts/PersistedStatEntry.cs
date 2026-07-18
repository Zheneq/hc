using System;

[Serializable]
public class PersistedStatEntry : ICloneable, IPersistedGameplayStat
{
	public int Sum { get; set; } = 0;
	public int NumGamesInSum { get; set; } = 0;
	public int Min { get; set; } = 0;
	public int Max { get; set; } = 0;

	public float Average()
	{
		if (NumGamesInSum == 0)
		{
			return 0f;
		}
		return Sum / (float)NumGamesInSum;
	}

	public void CombineStats(PersistedStatEntry entry)
	{
		Sum += entry.Sum;
		NumGamesInSum += entry.NumGamesInSum;
		Max = Math.Max(Max, entry.Max);
		Min = Math.Min(Min, entry.Min);
	}

	public void Adjust(int val)
	{
		bool isFirst = NumGamesInSum == 0;
		Sum += val;
		NumGamesInSum++;
		if (val > Max || isFirst)
		{
			Max = val;
		}
		if (val < Min || isFirst)
		{
			Min = val;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public PersistedStatEntry GetCopy()
	{
		return (PersistedStatEntry)MemberwiseClone();
	}

	public float GetSum()
	{
		return Sum;
	}

	public float GetMin()
	{
		return Min;
	}

	public float GetMax()
	{
		return Max;
	}

	public int GetNumGames()
	{
		return NumGamesInSum;
	}
}
