using System.Reflection;
using System.Threading;

public static class AsyncPumpExt
{
	public static void Post(this SynchronizationContext context, SendOrPostCallback callback, object state = null, MethodBase methodInfo = null)
	{
		AsyncPump asyncPump = context as AsyncPump;
		if (asyncPump != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					asyncPump.Post(callback, state, methodInfo);
					return;
				}
			}
		}
		context.Post(callback, state);
	}
}
