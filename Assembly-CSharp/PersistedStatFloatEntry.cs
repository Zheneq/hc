using System;

[Serializable]
public class PersistedStatFloatEntry : ICloneable, IPersistedGameplayStat
{
	public float Sum
	{
		get;
		set;
	}

	public int NumGamesInSum
	{
		get;
		set;
	}

	public float Min
	{
		get;
		set;
	}

	public float Max
	{
		get;
		set;
	}

	public PersistedStatFloatEntry()
	{
		Sum = 0f;
		NumGamesInSum = 0;
		Min = 0f;
		Max = 0f;
	}

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
		bool flag = NumGamesInSum == 0;
		Sum += val;
		NumGamesInSum++;
		if (!(val > Max))
		{
			if (!flag)
			{
				goto IL_005c;
			}
		}
		Max = val;
		goto IL_005c;
		IL_005c:
		if (!(val < Min))
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
