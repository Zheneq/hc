using C5;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

public class AsyncPump : SynchronizationContext
{
	public delegate void AsyncCallback(object state);

	public AsyncPumpProcessingLatency ProcessingLatency;

	private System.Collections.Generic.LinkedList<AsyncDelegate> m_queuedDelegates;

	private IntervalHeap<AsyncTimer> m_scheduledTimers;

	private Stopwatch m_clock;

	private Stopwatch m_profilingTimer;

	private object m_waitEvent;

	private long m_currentRunLoopStartTick;

	private string m_currentMethodName;

	private bool m_break;

	private readonly long TicksPerMillisecond;

	public new static AsyncPump Current => SynchronizationContext.Current as AsyncPump;

	public bool IsRunning
	{
		get;
		private set;
	}

	public long ProcessingLatencyTicks => ProcessingLatency.Current;

	public int ProcessingLatencyMilliseconds => (int)(ProcessingLatencyTicks / TicksPerMillisecond);

	public int ArtificalLatencyMilliseconds
	{
		get;
		set;
	}

	public int CurrentRunLoopMilliseconds => (int)((CurrentTick - m_currentRunLoopStartTick) / TicksPerMillisecond);

	public long CurrentTick => Now();

	public int MaxWaitMilliseconds
	{
		get;
		set;
	}

	public AsyncPump()
	{
		TicksPerMillisecond = Stopwatch.Frequency / 1000;
		MaxWaitMilliseconds = 1000;
		m_clock = new Stopwatch();
		m_clock.Start();
		m_profilingTimer = new Stopwatch();
		m_queuedDelegates = new System.Collections.Generic.LinkedList<AsyncDelegate>();
		m_scheduledTimers = new IntervalHeap<AsyncTimer>(new AsyncTimer.ScheduledTickComparer());
		m_waitEvent = new object();
		m_currentRunLoopStartTick = CurrentTick;
		ProcessingLatency = new AsyncPumpProcessingLatency();
	}

	public override void Send(SendOrPostCallback d, object state)
	{
		throw new NotSupportedException("Send() is not supported.");
	}

	public override void Post(SendOrPostCallback callback, object state)
	{
		Post(new AsyncDelegate(callback, state));
	}

	public void Post(SendOrPostCallback callback, object state = null, MethodBase methodInfo = null)
	{
		if (methodInfo == null)
		{
			while (true)
			{
				switch (5)
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
			methodInfo = callback.Method;
		}
		Post(new AsyncDelegate(callback, state, methodInfo));
	}

	public void Post(AsyncDelegate asyncDelegate)
	{
		lock (m_waitEvent)
		{
			asyncDelegate.ScheduledTick = Now();
			m_queuedDelegates.AddLast(asyncDelegate);
			Monitor.Pulse(m_waitEvent);
		}
	}

	public void Schedule(Action callback, long intervalMilliseconds, bool isOneShot = false)
	{
		Schedule(new AsyncTimer(callback, intervalMilliseconds, isOneShot));
	}

	public void Schedule(AsyncTimer asyncTimer)
	{
		asyncTimer.AsyncPump = this;
		asyncTimer.IsScheduled = true;
		asyncTimer.ScheduledTick = When(asyncTimer.IntervalMilliseconds);
		lock (m_waitEvent)
		{
			m_scheduledTimers.Add(asyncTimer);
			Monitor.Pulse(m_waitEvent);
		}
	}

	public void UnscheduleAll()
	{
		if (IsRunning)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					throw new Exception("Cannot unschedule timers/delegates while the async pump is running");
				}
			}
		}
		m_queuedDelegates = new System.Collections.Generic.LinkedList<AsyncDelegate>();
		m_scheduledTimers = new IntervalHeap<AsyncTimer>(new AsyncTimer.ScheduledTickComparer());
	}

	public void Run(int timeoutMilliseconds = int.MaxValue)
	{
		if (IsRunning)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					throw new Exception("This AsyncPump is already running");
				}
			}
		}
		IsRunning = true;
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		try
		{
			m_break = false;
			while (IsRunning)
			{
				m_currentRunLoopStartTick = CurrentTick;
				AsyncDelegate[] array;
				lock (m_waitEvent)
				{
					int maxMilliseconds = Math.Max((int)(timeoutMilliseconds - stopwatch.ElapsedMilliseconds), 0);
					int waitTime = GetWaitTime(maxMilliseconds);
					if (waitTime != 0)
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
						Monitor.Wait(m_waitEvent, waitTime);
					}
					if (!IsRunning)
					{
						while (true)
						{
							switch (6)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					CheckTimers();
					array = m_queuedDelegates.ToArray();
					m_queuedDelegates.Clear();
				}
				if (array.Length != 0)
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
					if (ArtificalLatencyMilliseconds != 0)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						Thread.Sleep(ArtificalLatencyMilliseconds);
					}
					long ticks = Now() - array[0].ScheduledTick;
					ProcessingLatency.Update(ticks);
					int num = 0;
					while (true)
					{
						if (num >= array.Length)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							break;
						}
						ExecuteDelegate(array[num]);
						if (m_break)
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
							lock (m_waitEvent)
							{
								for (int num2 = array.Length - 1; num2 > num; num2--)
								{
									m_queuedDelegates.AddFirst(array[num2]);
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
							break;
						}
						num++;
					}
				}
				if (timeoutMilliseconds != int.MaxValue && stopwatch.ElapsedMilliseconds >= timeoutMilliseconds)
				{
					while (true)
					{
						switch (7)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				if (m_break)
				{
					return;
				}
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		finally
		{
			IsRunning = false;
			m_break = false;
		}
	}

	public void Stop()
	{
		lock (m_waitEvent)
		{
			IsRunning = false;
			Monitor.Pulse(m_waitEvent);
		}
	}

	public void Break()
	{
		m_break = true;
	}

	public bool BreakRequested()
	{
		return m_break;
	}

	private int GetWaitTime(int maxMilliseconds = int.MaxValue)
	{
		int num;
		lock (m_waitEvent)
		{
			if (!IsRunning)
			{
				while (true)
				{
					switch (5)
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
				num = 0;
			}
			if (m_queuedDelegates.Count > 0)
			{
				num = 0;
			}
			else
			{
				if (!m_scheduledTimers.IsEmpty)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
						{
							AsyncTimer asyncTimer = m_scheduledTimers.FindMin();
							long num2 = Now();
							if (asyncTimer.ScheduledTick < num2)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										num = 0;
										goto end_IL_000d;
									}
								}
							}
							long num3 = asyncTimer.ScheduledTick - num2;
							num = (int)(num3 / TicksPerMillisecond);
							goto end_IL_000d;
						}
						}
					}
				}
				num = MaxWaitMilliseconds;
			}
			end_IL_000d:;
		}
		if (num > maxMilliseconds)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			num = maxMilliseconds;
		}
		return num;
	}

	private void CheckTimers()
	{
		lock (m_waitEvent)
		{
			long num = Now();
			while (!m_scheduledTimers.IsEmpty)
			{
				AsyncTimer asyncTimer = m_scheduledTimers.FindMin();
				if (asyncTimer.ScheduledTick > num)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							return;
						}
					}
				}
				m_scheduledTimers.DeleteMin();
				if (asyncTimer.IsScheduled)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					m_queuedDelegates.AddLast(asyncTimer.AsyncDelegate);
					asyncTimer.Unschedule();
					if (!asyncTimer.IsOneShot)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						asyncTimer.Schedule();
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void ExecuteDelegate(AsyncDelegate asyncDelegate)
	{
		m_profilingTimer.Reset();
		m_profilingTimer.Start();
		try
		{
			m_currentMethodName = null;
			asyncDelegate.Callback(asyncDelegate.State);
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		finally
		{
			m_profilingTimer.Stop();
			if (asyncDelegate.MethodInfo == null)
			{
				while (true)
				{
					switch (5)
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
				if (m_currentMethodName != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					ProfilingTimers.Get().OnMethodExecuted(m_currentMethodName, m_profilingTimer.ElapsedTicks);
					goto IL_00b7;
				}
			}
			ProfilingTimers.Get().OnMethodExecuted(asyncDelegate.MethodInfo, m_profilingTimer.ElapsedTicks);
			goto IL_00b7;
			IL_00b7:
			m_currentMethodName = null;
		}
	}

	private long Now()
	{
		return m_clock.ElapsedTicks;
	}

	private long When(long intervalMilliseconds)
	{
		return Now() + intervalMilliseconds * TicksPerMillisecond;
	}
}
