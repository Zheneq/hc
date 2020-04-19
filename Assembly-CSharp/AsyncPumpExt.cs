using System;
using System.Reflection;
using System.Threading;

public static class AsyncPumpExt
{
	public static void Post(this SynchronizationContext context, SendOrPostCallback callback, object state = null, MethodBase methodInfo = null)
	{
		AsyncPump asyncPump = context as AsyncPump;
		if (asyncPump != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SynchronizationContext.Post(SendOrPostCallback, object, MethodBase)).MethodHandle;
			}
			asyncPump.Post(callback, state, methodInfo);
		}
		else
		{
			context.Post(callback, state);
		}
	}
}
