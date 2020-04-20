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
					throw new Exception("Could not find the Pacific time zone information");
				}
			}
			return LobbyTimeZoneInfo.m_pacificTimeZoneInfo;
		}
	}
}
