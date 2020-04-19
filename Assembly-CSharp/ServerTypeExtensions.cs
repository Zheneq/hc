using System;

public static class ServerTypeExtensions
{
	public static bool NeedsMaster(this ProcessType serverType)
	{
		bool result;
		if (serverType != ProcessType.DirectoryServer)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProcessType.NeedsMaster()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProcessType.UsesInterconnect()).MethodHandle;
			}
			result = (serverType <= ProcessType.LoadTestServer);
		}
		else
		{
			result = false;
		}
		return result;
	}
}
