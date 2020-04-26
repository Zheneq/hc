using System;
using System.Threading;

public abstract class ThreadedJob
{
	private bool m_threadFunctionReturned;

	private object m_lockObject = new object();

	private Thread m_thread;

	private bool ThreadFunctionReturned
	{
		get
		{
			lock (m_lockObject)
			{
				return m_threadFunctionReturned;
			}
		}
		set
		{
			lock (m_lockObject)
			{
				m_threadFunctionReturned = value;
			}
		}
	}

	protected bool IsThreadAlive
	{
		get
		{
			int result;
			if (m_thread != null)
			{
				result = (m_thread.IsAlive ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}

	internal void StartThread()
	{
		ThreadFunctionReturned = false;
		m_thread = new Thread(Run);
		m_thread.Start();
	}

	internal void Abort()
	{
		if (m_thread == null)
		{
			return;
		}
		while (true)
		{
			m_thread.Abort();
			return;
		}
	}

	protected abstract void ThreadFunction();

	protected virtual void OnThreadFunctionReturned()
	{
	}

	internal virtual void Update()
	{
		if (!ThreadFunctionReturned)
		{
			return;
		}
		while (true)
		{
			OnThreadFunctionReturned();
			ThreadFunctionReturned = false;
			return;
		}
	}

	private void Run()
	{
		try
		{
			ThreadFunction();
		}
		catch (ThreadAbortException exception)
		{
			Log.Exception(exception);
		}
		catch (Exception exception2)
		{
			Log.Exception(exception2);
		}
		ThreadFunctionReturned = true;
	}
}
