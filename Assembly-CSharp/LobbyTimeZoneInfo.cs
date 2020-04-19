using System;

public static class LobbyTimeZoneInfo
{
	private static TimeZoneInfo m_pacificTimeZoneInfo;

	public static TimeZoneInfo PacificTimeZoneInfo
	{
		get
		{
			if (LobbyTimeZoneInfo.m_pacificTimeZoneInfo == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyTimeZoneInfo.get_PacificTimeZoneInfo()).MethodHandle;
				}
				try
				{
					LobbyTimeZoneInfo.m_pacificTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
				}
				catch (TimeZoneNotFoundException)
				{
					try
					{
						LobbyTimeZoneInfo.m_pacificTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("US/Pacific");
					}
					catch (TimeZoneNotFoundException)
					{
						LobbyTimeZoneInfo.m_pacificTimeZoneInfo = null;
					}
				}
				if (LobbyTimeZoneInfo.m_pacificTimeZoneInfo == null)
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
					throw new Exception("Could not find the Pacific time zone information");
				}
			}
			return LobbyTimeZoneInfo.m_pacificTimeZoneInfo;
		}
	}
}
