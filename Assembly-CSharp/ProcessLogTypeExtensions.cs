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
				return "stdout.log";
			}
		default:
			return null;
		}
	}
}
