using System.Diagnostics;

public class DebugProfileTimer
{
	private Stopwatch m_stopWatch;

	public DebugProfileTimer()
	{
		m_stopWatch = new Stopwatch();
		m_stopWatch.Start();
	}

	public float ElapsedMs()
	{
		return m_stopWatch.ElapsedMilliseconds;
	}

	public void Restart()
	{
		m_stopWatch.Reset();
		m_stopWatch.Start();
	}

	public void Stop()
	{
		m_stopWatch.Stop();
	}
}
