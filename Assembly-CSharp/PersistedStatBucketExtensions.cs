using System;

public static class PersistedStatBucketExtensions
{
	public static bool IsTracked(this PersistedStatBucket psb)
	{
		bool result;
		if (psb != PersistedStatBucket.None)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PersistedStatBucket.IsTracked()).MethodHandle;
			}
			result = (psb != PersistedStatBucket.DoNotPersist);
		}
		else
		{
			result = false;
		}
		return result;
	}
}
