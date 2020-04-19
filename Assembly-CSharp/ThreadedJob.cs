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
			object lockObject = this.m_lockObject;
			bool threadFunctionReturned;
			lock (lockObject)
			{
				threadFunctionReturned = this.m_threadFunctionReturned;
			}
			return threadFunctionReturned;
		}
		set
		{
			object lockObject = this.m_lockObject;
			lock (lockObject)
			{
				this.m_threadFunctionReturned = value;
			}
		}
	}

	protected bool IsThreadAlive
	{
		get
		{
			bool result;
			if (this.m_thread != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ThreadedJob.get_IsThreadAlive()).MethodHandle;
				}
				result = this.m_thread.IsAlive;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	internal void StartThread()
	{
		this.ThreadFunctionReturned = false;
		this.m_thread = new Thread(new ThreadStart(this.Run));
		this.m_thread.Start();
	}

	internal void Abort()
	{
		if (this.m_thread != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThreadedJob.Abort()).MethodHandle;
			}
			this.m_thread.Abort();
		}
	}

	protected abstract void ThreadFunction();

	protected virtual void OnThreadFunctionReturned()
	{
	}

	internal virtual void Update()
	{
		if (this.ThreadFunctionReturned)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThreadedJob.Update()).MethodHandle;
			}
			this.OnThreadFunctionReturned();
			this.ThreadFunctionReturned = false;
		}
	}

	private void Run()
	{
		try
		{
			this.ThreadFunction();
		}
		catch (ThreadAbortException exception)
		{
			Log.Exception(exception);
		}
		catch (Exception exception2)
		{
			Log.Exception(exception2);
		}
		this.ThreadFunctionReturned = true;
	}
}
