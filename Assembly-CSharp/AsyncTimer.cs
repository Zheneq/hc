using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

public class AsyncTimer
{
	private AsyncDelegate m_delegate;

	public AsyncTimer(Action callback, long intervalMilliseconds, bool isOneShot = false)
	{
		this.Initialize(delegate(object o)
		{
			callback();
		}, null, callback.Method, intervalMilliseconds, isOneShot);
	}

	private void Initialize(SendOrPostCallback callback, object state, MethodBase methodInfo, long intervalMilliseconds, bool isOneShot)
	{
		if (intervalMilliseconds < 0xAL)
		{
			if (!isOneShot)
			{
				throw new ArgumentOutOfRangeException("IntervalMilliseconds");
			}
		}
		if (methodInfo == null)
		{
			methodInfo = callback.Method;
		}
		this.m_delegate = new AsyncDelegate(callback, state, methodInfo);
		this.IntervalMilliseconds = intervalMilliseconds;
		this.IsOneShot = isOneShot;
		this.AsyncPump = AsyncPump.Current;
	}

	public void Schedule()
	{
		if (this.IsScheduled)
		{
			throw new Exception("Already scheduled");
		}
		this.IsScheduled = true;
		this.AsyncPump.Schedule(this);
	}

	public void Unschedule()
	{
		this.IsScheduled = false;
	}

	public AsyncDelegate AsyncDelegate
	{
		get
		{
			return this.m_delegate;
		}
	}

	public AsyncPump AsyncPump { get; set; }

	public long IntervalMilliseconds { get; set; }

	public long ScheduledTick
	{
		get
		{
			return this.m_delegate.ScheduledTick;
		}
		set
		{
			this.m_delegate.ScheduledTick = value;
		}
	}

	public bool IsScheduled { get; set; }

	public bool IsOneShot { get; set; }

	public class ScheduledTickComparer : Comparer<AsyncTimer>
	{
		public override int Compare(AsyncTimer obj1, AsyncTimer obj2)
		{
			if (obj1 == null)
			{
				if (obj2 == null)
				{
					return 0;
				}
			}
			if (obj1 == null)
			{
				return -1;
			}
			if (obj2 == null)
			{
				return 1;
			}
			return obj1.ScheduledTick.CompareTo(obj2.ScheduledTick);
		}
	}
}
