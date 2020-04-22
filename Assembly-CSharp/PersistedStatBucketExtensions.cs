public static class PersistedStatBucketExtensions
{
	public static bool IsTracked(this PersistedStatBucket psb)
	{
		int result;
		if (psb != 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = ((psb != PersistedStatBucket.DoNotPersist) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}
}
