using System;

public class ProfilingTimer
{
	public ProfilingTimer(string name, int id)
	{
		this.Name = name;
		this.Id = id;
	}

	public void Increment(long ticks = 0L, long bytes = 0L, bool skipped = false)
	{
		this.VolatileElapsedTicks += ticks;
		this.VolatileCalls += 1L;
		this.VolatileBytes += bytes;
		if (skipped)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProfilingTimer.Increment(long, long, bool)).MethodHandle;
			}
			this.VolatileCallsSkipped += 1L;
		}
	}

	public void Update()
	{
		this.RecentCalls = this.VolatileCalls;
		this.RecentCallsSkipped = this.VolatileCallsSkipped;
		this.RecentElapsedTicks = this.VolatileElapsedTicks;
		this.RecentBytes = this.VolatileBytes;
		this.VolatileCalls = 0L;
		this.VolatileCallsSkipped = 0L;
		this.VolatileElapsedTicks = 0L;
		this.VolatileBytes = 0L;
	}

	public int Id { get; private set; }

	public string Name { get; private set; }

	public long VolatileCalls { get; private set; }

	public long VolatileCallsSkipped { get; private set; }

	public long VolatileElapsedTicks { get; private set; }

	public long VolatileBytes { get; private set; }

	public long RecentCalls { get; private set; }

	public long RecentCallsSkipped { get; private set; }

	public long RecentElapsedTicks { get; private set; }

	public long RecentBytes { get; private set; }

	public double RecentLoad
	{
		get
		{
			return (double)this.RecentElapsedTicks / (double)ProfilingTimers.Get().RecentElapsedTicks * 100.0;
		}
	}

	public double RecentBytesRate
	{
		get
		{
			return (double)this.RecentBytes * (double)ProfilingTimers.Get().Frequency / (double)ProfilingTimers.Get().RecentElapsedTicks;
		}
	}

	public DateTime LastWarning { get; set; }
}
