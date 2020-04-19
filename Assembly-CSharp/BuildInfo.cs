using System;
using System.Globalization;

public static class BuildInfo
{
	public static DateTime GetBuildDateUtc()
	{
		return DateTime.ParseExact("2019-04-10T21:33:02.7091615Z", "o", CultureInfo.InvariantCulture).ToUniversalTime();
	}

	public static string GetBuildMachine()
	{
		return "rwc-hybuild4";
	}

	public static string GetBuildInfoString()
	{
		return BuildVersion.GetBuildDescriptionString(BuildInfo.GetBuildDateUtc(), BuildInfo.GetBuildMachine());
	}
}
