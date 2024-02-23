using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

public class AsyncTimer
{
	public class ScheduledTickComparer : Comparer<AsyncTimer>
	{
		public override int Compare(AsyncTimer obj1, AsyncTimer obj2)
		{
			if (obj1 == null)
			{
				if (obj2 == null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return 0;
						}
					}
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

	private AsyncDelegate m_delegate;

	public AsyncDelegate AsyncDelegate
	{
		get { return m_delegate; }
	}

	public AsyncPump AsyncPump
	{
		get;
		set;
	}

	public long IntervalMilliseconds
	{
		get;
		set;
	}

	public long ScheduledTick
	{
		get
		{
			return m_delegate.ScheduledTick;
		}
		set
		{
			m_delegate.ScheduledTick = value;
		}
	}

	public bool IsScheduled
	{
		get;
		set;
	}

	public bool IsOneShot
	{
		get;
		set;
	}

	public AsyncTimer(Action callback, long intervalMilliseconds, bool isOneShot = false)
	{
		Initialize(delegate
		{
			callback();
		}, null, callback.Method, intervalMilliseconds, isOneShot);
	}

	private void Initialize(SendOrPostCallback callback, object state, MethodBase methodInfo, long intervalMilliseconds, bool isOneShot)
	{
		if (intervalMilliseconds < 10)
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
		m_delegate = new AsyncDelegate(callback, state, methodInfo);
		IntervalMilliseconds = intervalMilliseconds;
		IsOneShot = isOneShot;
		AsyncPump = AsyncPump.Current;
	}

	public void Schedule()
	{
		if (IsScheduled)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					throw new Exception("Already scheduled");
				}
			}
		}
		IsScheduled = true;
		AsyncPump.Schedule(this);
	}

	public void Unschedule()
	{
		IsScheduled = false;
	}
}
