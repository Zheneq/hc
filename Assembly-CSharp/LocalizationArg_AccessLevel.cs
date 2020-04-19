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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationArg_AccessLevel.TR()).MethodHandle;
			}
			return StringUtil.TR("Free", "ClientAccessLevel");
		}
		if (this.m_value >= ClientAccessLevel.VIP)
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
			return StringUtil.TR("VIP", "ClientAccessLevel");
		}
		return StringUtil.TR("Full", "ClientAccessLevel");
	}
}
