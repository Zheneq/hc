using System;

public static class ServerLibCrashReportUtil
{
	public static string CreateArchiveNameForClient(bool devBuild, int numArchiveBytes)
	{
		object buildName;
		if (devBuild)
		{
			buildName = "AtlasReactorDev";
		}
		else
		{
			buildName = "AtlasReactor";
		}
		return CreateArchiveName((string)buildName, numArchiveBytes);
	}

	public static string CreateArchiveNameForServer(int numArchiveBytes)
	{
		return CreateArchiveName("AtlasReactorServer", numArchiveBytes);
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
