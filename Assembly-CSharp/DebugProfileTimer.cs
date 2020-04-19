using System;
using System.Diagnostics;

public class DebugProfileTimer
{
	private Stopwatch m_stopWatch;

	public DebugProfileTimer()
	{
		this.m_stopWatch = new Stopwatch();
		this.m_stopWatch.Start();
	}

	public float ElapsedMs()
	{
		return (float)this.m_stopWatch.ElapsedMilliseconds;
	}

	public void Restart()
	{
		this.m_stopWatch.Reset();
		this.m_stopWatch.Start();
	}

	public void Stop()
	{
		this.m_stopWatch.Stop();
	}
}
