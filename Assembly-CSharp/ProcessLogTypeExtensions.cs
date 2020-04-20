using System;

public static class ProcessLogTypeExtensions
{
	public static string GetFileExtension(this ProcessLogType processLogType)
	{
		if (processLogType == ProcessLogType.Log)
		{
			return "log";
		}
		if (processLogType == ProcessLogType.symbol_001D)
		{
			return "debug.log";
		}
		if (processLogType == ProcessLogType.OutputLog)
		{
			return "stdout.log";
		}
		return null;
	}
}
