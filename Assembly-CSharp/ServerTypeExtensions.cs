public static class ServerTypeExtensions
{
	public static bool NeedsMaster(this ProcessType serverType)
	{
		int result;
		if (serverType != ProcessType.DirectoryServer)
		{
			result = ((serverType == ProcessType.MatchmakingServer) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public static bool UsesInterconnect(this ProcessType serverType)
	{
		int result;
		if (serverType >= ProcessType.DirectoryServer)
		{
			result = ((serverType <= ProcessType.LoadTestServer) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}
}
