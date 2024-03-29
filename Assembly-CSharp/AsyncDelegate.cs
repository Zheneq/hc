using System.Reflection;
using System.Threading;

public struct AsyncDelegate
{
	public SendOrPostCallback Callback;

	public object State;

	public MethodBase MethodInfo;

	public long ScheduledTick;

	public AsyncDelegate(SendOrPostCallback callback, object state = null, MethodBase methodInfo = null)
	{
		Callback = callback;
		State = state;
		MethodInfo = methodInfo;
		ScheduledTick = 0L;
	}
}
