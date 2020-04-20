using System;

public class AsyncPumpProcessingLatency
{
	public long Current;

	public long Max;

	public long Sum;

	public long Count;

	public AsyncPumpProcessingLatency()
	{
		this.Reset();
	}

	public double Avg
	{
		get
		{
			double result;
			if (this.Count > 0L)
			{
				result = (double)this.Sum / (double)this.Count;
			}
			else
			{
				result = 0.0;
			}
			return result;
		}
	}

	public void Update(long ticks)
	{
		this.Current = ticks;
		if (this.Max < ticks)
		{
			this.Max = ticks;
		}
		this.Sum += ticks;
		this.Count += 1L;
	}

	public void Reset()
	{
		this.Max = long.MinValue;
		this.Sum = 0L;
		this.Count = 0L;
	}
}
