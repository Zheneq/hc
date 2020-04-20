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
			asyncPump.Post(callback, state, methodInfo);
		}
		else
		{
			context.Post(callback, state);
		}
	}
}
