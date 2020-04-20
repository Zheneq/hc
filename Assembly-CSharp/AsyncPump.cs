using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using C5;

public class AsyncPump : SynchronizationContext
{
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

	public AsyncPump()
	{
		this.TicksPerMillisecond = Stopwatch.Frequency / 0x3E8L;
		this.MaxWaitMilliseconds = 0x3E8;
		this.m_clock = new Stopwatch();
		this.m_clock.Start();
		this.m_profilingTimer = new Stopwatch();
		this.m_queuedDelegates = new System.Collections.Generic.LinkedList<AsyncDelegate>();
		this.m_scheduledTimers = new IntervalHeap<AsyncTimer>(new AsyncTimer.ScheduledTickComparer());
		this.m_waitEvent = new object();
		this.m_currentRunLoopStartTick = this.CurrentTick;
		this.ProcessingLatency = new AsyncPumpProcessingLatency();
	}

	public new static AsyncPump Current
	{
		get
		{
			return SynchronizationContext.Current as AsyncPump;
		}
	}

	public bool IsRunning { get; private set; }

	public long ProcessingLatencyTicks
	{
		get
		{
			return this.ProcessingLatency.Current;
		}
	}

	public int ProcessingLatencyMilliseconds
	{
		get
		{
			return (int)(this.ProcessingLatencyTicks / this.TicksPerMillisecond);
		}
	}

	public int ArtificalLatencyMilliseconds { get; set; }

	public int CurrentRunLoopMilliseconds
	{
		get
		{
			return (int)((this.CurrentTick - this.m_currentRunLoopStartTick) / this.TicksPerMillisecond);
		}
	}

	public long CurrentTick
	{
		get
		{
			return this.Now();
		}
	}

	public int MaxWaitMilliseconds { get; set; }

	public override void Send(SendOrPostCallback d, object state)
	{
		throw new NotSupportedException("Send() is not supported.");
	}

	public override void Post(SendOrPostCallback callback, object state)
	{
		this.Post(new AsyncDelegate(callback, state, null));
	}

	public void Post(SendOrPostCallback callback, object state = null, MethodBase methodInfo = null)
	{
		if (methodInfo == null)
		{
			methodInfo = callback.Method;
		}
		this.Post(new AsyncDelegate(callback, state, methodInfo));
	}

	public void Post(AsyncDelegate asyncDelegate)
	{
		object waitEvent = this.m_waitEvent;
		lock (waitEvent)
		{
			asyncDelegate.ScheduledTick = this.Now();
			this.m_queuedDelegates.AddLast(asyncDelegate);
			Monitor.Pulse(this.m_waitEvent);
		}
	}

	public void Schedule(Action callback, long intervalMilliseconds, bool isOneShot = false)
	{
		this.Schedule(new AsyncTimer(callback, intervalMilliseconds, isOneShot));
	}

	public void Schedule(AsyncTimer asyncTimer)
	{
		asyncTimer.AsyncPump = this;
		asyncTimer.IsScheduled = true;
		asyncTimer.ScheduledTick = this.When(asyncTimer.IntervalMilliseconds);
		object waitEvent = this.m_waitEvent;
		lock (waitEvent)
		{
			this.m_scheduledTimers.Add(asyncTimer);
			Monitor.Pulse(this.m_waitEvent);
		}
	}

	public void UnscheduleAll()
	{
		if (this.IsRunning)
		{
			throw new Exception("Cannot unschedule timers/delegates while the async pump is running");
		}
		this.m_queuedDelegates = new System.Collections.Generic.LinkedList<AsyncDelegate>();
		this.m_scheduledTimers = new IntervalHeap<AsyncTimer>(new AsyncTimer.ScheduledTickComparer());
	}

	public void Run(int timeoutMilliseconds = 0x7FFFFFFF)
	{
		if (this.IsRunning)
		{
			throw new Exception("This AsyncPump is already running");
		}
		this.IsRunning = true;
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		try
		{
			this.m_break = false;
			while (this.IsRunning)
			{
				this.m_currentRunLoopStartTick = this.CurrentTick;
				object waitEvent = this.m_waitEvent;
				AsyncDelegate[] array;
				lock (waitEvent)
				{
					int maxMilliseconds = Math.Max((int)((long)timeoutMilliseconds - stopwatch.ElapsedMilliseconds), 0);
					int waitTime = this.GetWaitTime(maxMilliseconds);
					if (waitTime != 0)
					{
						Monitor.Wait(this.m_waitEvent, waitTime);
					}
					if (!this.IsRunning)
					{
						goto IL_20B;
					}
					this.CheckTimers();
					array = this.m_queuedDelegates.ToArray<AsyncDelegate>();
					this.m_queuedDelegates.Clear();
				}
				if (array.Length != 0)
				{
					if (this.ArtificalLatencyMilliseconds != 0)
					{
						Thread.Sleep(this.ArtificalLatencyMilliseconds);
					}
					long ticks = this.Now() - array[0].ScheduledTick;
					this.ProcessingLatency.Update(ticks);
					for (int i = 0; i < array.Length; i++)
					{
						this.ExecuteDelegate(array[i]);
						if (this.m_break)
						{
							object waitEvent2 = this.m_waitEvent;
							lock (waitEvent2)
							{
								for (int j = array.Length - 1; j > i; j--)
								{
									this.m_queuedDelegates.AddFirst(array[j]);
								}
							}
							goto IL_1CC;
						}
					}
				}
				IL_1CC:
				if (timeoutMilliseconds != 0x7FFFFFFF && stopwatch.ElapsedMilliseconds >= (long)timeoutMilliseconds)
				{
				}
				else if (!this.m_break)
				{
					continue;
				}
				IL_20B:
				return;
			}
		}
		finally
		{
			this.IsRunning = false;
			this.m_break = false;
		}
	}

	public void Stop()
	{
		object waitEvent = this.m_waitEvent;
		lock (waitEvent)
		{
			this.IsRunning = false;
			Monitor.Pulse(this.m_waitEvent);
		}
	}

	public void Break()
	{
		this.m_break = true;
	}

	public bool BreakRequested()
	{
		return this.m_break;
	}

	private int GetWaitTime(int maxMilliseconds = 0x7FFFFFFF)
	{
		object waitEvent = this.m_waitEvent;
		int num;
		lock (waitEvent)
		{
			if (!this.IsRunning)
			{
				num = 0;
			}
			if (this.m_queuedDelegates.Count > 0)
			{
				num = 0;
			}
			else if (!this.m_scheduledTimers.IsEmpty)
			{
				AsyncTimer asyncTimer = this.m_scheduledTimers.FindMin();
				long num2 = this.Now();
				if (asyncTimer.ScheduledTick < num2)
				{
					num = 0;
				}
				else
				{
					long num3 = asyncTimer.ScheduledTick - num2;
					num = (int)(num3 / this.TicksPerMillisecond);
				}
			}
			else
			{
				num = this.MaxWaitMilliseconds;
			}
		}
		if (num > maxMilliseconds)
		{
			num = maxMilliseconds;
		}
		return num;
	}

	private void CheckTimers()
	{
		object waitEvent = this.m_waitEvent;
		lock (waitEvent)
		{
			long num = this.Now();
			while (!this.m_scheduledTimers.IsEmpty)
			{
				AsyncTimer asyncTimer = this.m_scheduledTimers.FindMin();
				if (asyncTimer.ScheduledTick > num)
				{
					return;
				}
				this.m_scheduledTimers.DeleteMin();
				if (asyncTimer.IsScheduled)
				{
					this.m_queuedDelegates.AddLast(asyncTimer.AsyncDelegate);
					asyncTimer.Unschedule();
					if (!asyncTimer.IsOneShot)
					{
						asyncTimer.Schedule();
					}
				}
			}
		}
	}

	private void ExecuteDelegate(AsyncDelegate asyncDelegate)
	{
		this.m_profilingTimer.Reset();
		this.m_profilingTimer.Start();
		try
		{
			this.m_currentMethodName = null;
			asyncDelegate.Callback(asyncDelegate.State);
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		finally
		{
			this.m_profilingTimer.Stop();
			if (asyncDelegate.MethodInfo == null)
			{
				if (this.m_currentMethodName != null)
				{
					ProfilingTimers.Get().OnMethodExecuted(this.m_currentMethodName, this.m_profilingTimer.ElapsedTicks);
					goto IL_B7;
				}
			}
			ProfilingTimers.Get().OnMethodExecuted(asyncDelegate.MethodInfo, this.m_profilingTimer.ElapsedTicks);
			IL_B7:
			this.m_currentMethodName = null;
		}
	}

	private long Now()
	{
		return this.m_clock.ElapsedTicks;
	}

	private long When(long intervalMilliseconds)
	{
		return this.Now() + intervalMilliseconds * this.TicksPerMillisecond;
	}

	public delegate void AsyncCallback(object state);
}
