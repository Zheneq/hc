using System;

public static class ServerLibCrashReportUtil
{
	public static string CreateArchiveNameForClient(bool devBuild, int numArchiveBytes)
	{
		string buildName;
		if (devBuild)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ServerLibCrashReportUtil.CreateArchiveNameForClient(bool, int)).MethodHandle;
			}
			buildName = "AtlasReactorDev";
		}
		else
		{
			buildName = "AtlasReactor";
		}
		return ServerLibCrashReportUtil.CreateArchiveName(buildName, numArchiveBytes);
	}

	public static string CreateArchiveNameForServer(int numArchiveBytes)
	{
		return ServerLibCrashReportUtil.CreateArchiveName("AtlasReactorServer", numArchiveBytes);
	}

	private static string CreateArchiveName(string buildName, int numArchiveBytes)
	{
		throw new NotSupportedException();
	}

	private static string GetHashResultString(string fileNameNoSuffix, uint fileSize)
	{
		throw new NotSupportedException();
	}
}
