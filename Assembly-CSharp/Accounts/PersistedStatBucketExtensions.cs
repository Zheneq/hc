public static class PersistedStatBucketExtensions
{
	public static bool IsTracked(this PersistedStatBucket psb)
	{
		int result;
		if (psb != 0)
		{
			result = ((psb != PersistedStatBucket.DoNotPersist) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}
}
