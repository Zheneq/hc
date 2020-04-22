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
			while (true)
			{
				switch (7)
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
			if (!flag)
			{
				goto IL_005c;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		Max = val;
		goto IL_005c;
		IL_005c:
		if (!(val < Min))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
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
