using System;

[Serializable]
public class LocalizationArg_AccessLevel : LocalizationArg
{
	public ClientAccessLevel m_value;

	public static LocalizationArg_AccessLevel Create(ClientAccessLevel value)
	{
		return new LocalizationArg_AccessLevel
		{
			m_value = value
		};
	}

	public override string TR()
	{
		if (this.m_value <= ClientAccessLevel.Free)
		{
			return StringUtil.TR("Free", "ClientAccessLevel");
		}
		if (this.m_value >= ClientAccessLevel.VIP)
		{
			return StringUtil.TR("VIP", "ClientAccessLevel");
		}
		return StringUtil.TR("Full", "ClientAccessLevel");
	}
}
