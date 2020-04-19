using System;

[Serializable]
public class PersistedStatEntry : ICloneable, IPersistedGameplayStat
{
	public PersistedStatEntry()
	{
		this.Sum = 0;
		this.NumGamesInSum = 0;
		this.Min = 0;
		this.Max = 0;
	}

	public int Sum { get; set; }

	public int NumGamesInSum { get; set; }

	public int Min { get; set; }

	public int Max { get; set; }

	public float Average()
	{
		if (this.NumGamesInSum == 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PersistedStatEntry.Average()).MethodHandle;
			}
			return 0f;
		}
		return (float)this.Sum / (float)this.NumGamesInSum;
	}

	public void CombineStats(PersistedStatEntry entry)
	{
		this.Sum += entry.Sum;
		this.NumGamesInSum += entry.NumGamesInSum;
		this.Max = Math.Max(this.Max, entry.Max);
		this.Min = Math.Min(this.Min, entry.Min);
	}

	public void Adjust(int val)
	{
		bool flag = this.NumGamesInSum == 0;
		this.Sum += val;
		this.NumGamesInSum++;
		if (val <= this.Max)
		{
			if (!flag)
			{
				goto IL_54;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PersistedStatEntry.Adjust(int)).MethodHandle;
			}
		}
		this.Max = val;
		IL_54:
		if (val >= this.Min)
		{
			if (!flag)
			{
				return;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.Min = val;
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	public PersistedStatEntry GetCopy()
	{
		return (PersistedStatEntry)base.MemberwiseClone();
	}

	public float GetSum()
	{
		return (float)this.Sum;
	}

	public float GetMin()
	{
		return (float)this.Min;
	}

	public float GetMax()
	{
		return (float)this.Max;
	}

	public int GetNumGames()
	{
		return this.NumGamesInSum;
	}
}
