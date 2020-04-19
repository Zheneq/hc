using System;

public static class ProcessLogTypeExtensions
{
	public static string GetFileExtension(this ProcessLogType processLogType)
	{
		if (processLogType == ProcessLogType.Log)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProcessLogType.GetFileExtension()).MethodHandle;
			}
			return "log";
		}
		if (processLogType == ProcessLogType.\u001D)
		{
			return "debug.log";
		}
		if (processLogType == ProcessLogType.OutputLog)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			return "stdout.log";
		}
		return null;
	}
}
