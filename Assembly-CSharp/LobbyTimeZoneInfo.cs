using System;

public static class LobbyTimeZoneInfo
{
	private static TimeZoneInfo m_pacificTimeZoneInfo;

	public static TimeZoneInfo PacificTimeZoneInfo
	{
		get
		{
			if (m_pacificTimeZoneInfo == null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				try
				{
					m_pacificTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
				}
				catch (TimeZoneNotFoundException)
				{
					try
					{
						m_pacificTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("US/Pacific");
					}
					catch (TimeZoneNotFoundException)
					{
						m_pacificTimeZoneInfo = null;
					}
				}
				if (m_pacificTimeZoneInfo == null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							throw new Exception("Could not find the Pacific time zone information");
						}
					}
				}
			}
			return m_pacificTimeZoneInfo;
		}
	}
}
