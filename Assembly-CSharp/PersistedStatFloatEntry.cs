using System;

[Serializable]
public class PersistedStatFloatEntry : ICloneable, IPersistedGameplayStat
{
	public PersistedStatFloatEntry()
	{
		this.Sum = 0f;
		this.NumGamesInSum = 0;
		this.Min = 0f;
		this.Max = 0f;
	}

	public float Sum { get; set; }

	public int NumGamesInSum { get; set; }

	public float Min { get; set; }

	public float Max { get; set; }

	public float Average()
	{
		if (this.NumGamesInSum == 0)
		{
			return 0f;
		}
		return this.Sum / (float)this.NumGamesInSum;
	}

	public void CombineStats(PersistedStatFloatEntry entry)
	{
		this.Sum += entry.Sum;
		this.NumGamesInSum += entry.NumGamesInSum;
		this.Max = Math.Max(this.Max, entry.Max);
		this.Min = Math.Min(this.Min, entry.Min);
	}

	public void Adjust(float val)
	{
		bool flag = this.NumGamesInSum == 0;
		this.Sum += val;
		this.NumGamesInSum++;
		if (val <= this.Max)
		{
			if (!flag)
			{
				goto IL_5C;
			}
		}
		this.Max = val;
		IL_5C:
		if (val >= this.Min)
		{
			if (!flag)
			{
				return;
			}
		}
		this.Min = val;
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	public PersistedStatFloatEntry GetCopy()
	{
		return (PersistedStatFloatEntry)base.MemberwiseClone();
	}

	public float GetSum()
	{
		return this.Sum;
	}

	public float GetMin()
	{
		return this.Min;
	}

	public float GetMax()
	{
		return this.Max;
	}

	public int GetNumGames()
	{
		return this.NumGamesInSum;
	}
}
