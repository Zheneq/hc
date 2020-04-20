using System;

public static class PersistedStatBucketExtensions
{
	public static bool IsTracked(this PersistedStatBucket psb)
	{
		bool result;
		if (psb != PersistedStatBucket.None)
		{
			result = (psb != PersistedStatBucket.DoNotPersist);
		}
		else
		{
			result = false;
		}
		return result;
	}
}
