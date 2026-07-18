using System;

[Serializable]
public class PersistedStatFloatEntry : ICloneable, IPersistedGameplayStat
{
	public float Sum { get; set; } = 0f;
	public int NumGamesInSum { get; set; } = 0;
	public float Min { get; set; } = 0f;
	public float Max { get; set; } = 0f;

	public float Average()
	{
		if (NumGamesInSum == 0)
		{
			return 0f;
		}
		return Sum / (float)NumGamesInSum;
	}

	public void CombineStats(PersistedStatFloatEntry entry)
	{
		Sum += entry.Sum;
		NumGamesInSum += entry.NumGamesInSum;
		Max = Math.Max(Max, entry.Max);
		Min = Math.Min(Min, entry.Min);
	}

	public void Adjust(float val)
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

	public PersistedStatFloatEntry GetCopy()
	{
		return (PersistedStatFloatEntry)MemberwiseClone();
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
