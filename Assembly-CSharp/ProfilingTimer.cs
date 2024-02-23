using System;

public class ProfilingTimer
{
	public int Id
	{
		get;
		private set;
	}

	public string Name
	{
		get;
		private set;
	}

	public long VolatileCalls
	{
		get;
		private set;
	}

	public long VolatileCallsSkipped
	{
		get;
		private set;
	}

	public long VolatileElapsedTicks
	{
		get;
		private set;
	}

	public long VolatileBytes
	{
		get;
		private set;
	}

	public long RecentCalls
	{
		get;
		private set;
	}

	public long RecentCallsSkipped
	{
		get;
		private set;
	}

	public long RecentElapsedTicks
	{
		get;
		private set;
	}

	public long RecentBytes
	{
		get;
		private set;
	}

	public double RecentLoad
	{
		get { return (double)RecentElapsedTicks / (double)ProfilingTimers.Get().RecentElapsedTicks * 100.0; }
	}

	public double RecentBytesRate
	{
		get { return (double)RecentBytes * (double)ProfilingTimers.Get().Frequency / (double)ProfilingTimers.Get().RecentElapsedTicks; }
	}

	public DateTime LastWarning
	{
		get;
		set;
	}

	public ProfilingTimer(string name, int id)
	{
		Name = name;
		Id = id;
	}

	public void Increment(long ticks = 0L, long bytes = 0L, bool skipped = false)
	{
		VolatileElapsedTicks += ticks;
		VolatileCalls++;
		VolatileBytes += bytes;
		if (!skipped)
		{
			return;
		}
		while (true)
		{
			VolatileCallsSkipped++;
			return;
		}
	}

	public void Update()
	{
		RecentCalls = VolatileCalls;
		RecentCallsSkipped = VolatileCallsSkipped;
		RecentElapsedTicks = VolatileElapsedTicks;
		RecentBytes = VolatileBytes;
		VolatileCalls = 0L;
		VolatileCallsSkipped = 0L;
		VolatileElapsedTicks = 0L;
		VolatileBytes = 0L;
	}
}
