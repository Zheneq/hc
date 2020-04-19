using System;
using System.Collections.Generic;
using I2.Loc;

[Serializable]
public class LocalizationArg_BroadcastMessage : LocalizationArg
{
	public Dictionary<string, string> m_packedMessages;

	private TimeSpan m_timeInAdvance;

	public static LocalizationArg_BroadcastMessage Create(Dictionary<string, string> messages)
	{
		return new LocalizationArg_BroadcastMessage
		{
			m_packedMessages = messages
		};
	}

	public static LocalizationArg_BroadcastMessage Create(Dictionary<string, string> messages, TimeSpan timeValue)
	{
		return new LocalizationArg_BroadcastMessage
		{
			m_packedMessages = messages,
			m_timeInAdvance = timeValue
		};
	}

	public override string TR()
	{
		string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
		if (this.m_packedMessages.ContainsKey(currentLanguageCode))
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(LocalizationArg_BroadcastMessage.TR()).MethodHandle;
			}
			if (!this.m_packedMessages[currentLanguageCode].IsNullOrEmpty())
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.m_packedMessages[currentLanguageCode].SafeReplace("${TIME}", StringUtil.GetTimeDifferenceText(this.m_timeInAdvance, true));
			}
		}
		if (this.m_packedMessages.ContainsKey("en"))
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			return this.m_packedMessages["en"].SafeReplace("${TIME}", StringUtil.GetTimeDifferenceText(this.m_timeInAdvance, true));
		}
		return string.Empty;
	}
}
