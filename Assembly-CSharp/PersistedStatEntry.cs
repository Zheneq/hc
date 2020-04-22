using System;

[Serializable]
public class PersistedStatEntry : ICloneable, IPersistedGameplayStat
{
	public int Sum
	{
		get;
		set;
	}

	public int NumGamesInSum
	{
		get;
		set;
	}

	public int Min
	{
		get;
		set;
	}

	public int Max
	{
		get;
		set;
	}

	public PersistedStatEntry()
	{
		Sum = 0;
		NumGamesInSum = 0;
		Min = 0;
		Max = 0;
	}

	public float Average()
	{
		if (NumGamesInSum == 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return 0f;
				}
			}
		}
		return (float)Sum / (float)NumGamesInSum;
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
		bool flag = NumGamesInSum == 0;
		Sum += val;
		NumGamesInSum++;
		if (val <= Max)
		{
			if (!flag)
			{
				goto IL_0054;
			}
		}
		Max = val;
		goto IL_0054;
		IL_0054:
		if (val >= Min)
		{
			if (!flag)
			{
				return;
			}
		}
		Min = val;
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
