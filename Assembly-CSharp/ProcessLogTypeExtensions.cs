public static class ProcessLogTypeExtensions
{
	public static string GetFileExtension(this ProcessLogType processLogType)
	{
		if (processLogType == ProcessLogType.Log)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return "log";
				}
			}
		}
		switch (processLogType)
		{
		case ProcessLogType._001D:
			return "debug.log";
		case ProcessLogType.OutputLog:
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				return "stdout.log";
			}
		default:
			return null;
		}
	}
}
