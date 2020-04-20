using System;

public static class ServerTypeExtensions
{
	public static bool NeedsMaster(this ProcessType serverType)
	{
		bool result;
		if (serverType != ProcessType.DirectoryServer)
		{
			result = (serverType == ProcessType.MatchmakingServer);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public static bool UsesInterconnect(this ProcessType serverType)
	{
		bool result;
		if (serverType >= ProcessType.DirectoryServer)
		{
			result = (serverType <= ProcessType.LoadTestServer);
		}
		else
		{
			result = false;
		}
		return result;
	}
}
