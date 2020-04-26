using System;

[Serializable]
public class LocalizationArg_AccessLevel : LocalizationArg
{
	public ClientAccessLevel m_value;

	public static LocalizationArg_AccessLevel Create(ClientAccessLevel value)
	{
		LocalizationArg_AccessLevel localizationArg_AccessLevel = new LocalizationArg_AccessLevel();
		localizationArg_AccessLevel.m_value = value;
		return localizationArg_AccessLevel;
	}

	public override string TR()
	{
		if (m_value <= ClientAccessLevel.Free)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return StringUtil.TR("Free", "ClientAccessLevel");
				}
			}
		}
		if (m_value >= ClientAccessLevel.VIP)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return StringUtil.TR("VIP", "ClientAccessLevel");
				}
			}
		}
		return StringUtil.TR("Full", "ClientAccessLevel");
	}
}
